using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace mRemoteNG.Config.Settings.Store
{
    /// <summary>
    /// SQLite-backed settings store with optional encryption via a connection-level password.
    /// The password is the hex-encoded DEK provided by <see cref="KeyManagement.KeystoreManager"/>.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public sealed class SqliteSettingsStore : ISettingsStore
    {
        private readonly string _dbPath;
        private readonly string _connectionString;
        private SqliteConnection _connection;
        private bool _disposed;

        public bool IsInitialized { get; private set; }
        public int SchemaVersion => GetSchemaVersion();

        /// <summary>
        /// Opens or creates a SQLite settings database.
        /// </summary>
        /// <param name="dbPath">Full path to the .db file.</param>
        /// <param name="dekHex">
        /// Optional hex-encoded 256-bit data encryption key.
        /// When provided, SQLite encryption is used (requires SQLCipher bundle or compatible provider).
        /// Pass <c>null</c> for an unencrypted database.
        /// </param>
        public SqliteSettingsStore(string dbPath, string dekHex = null)
        {
            _dbPath = dbPath ?? throw new ArgumentNullException(nameof(dbPath));

            SqliteConnectionStringBuilder builder = new()
            {
                DataSource = _dbPath,
                Mode = SqliteOpenMode.ReadWriteCreate,
                Cache = SqliteCacheMode.Shared
            };

            if (!string.IsNullOrEmpty(dekHex))
            {
                builder.Password = dekHex;
            }

            _connectionString = builder.ToString();
        }

        /// <summary>
        /// Initialises the database, creating the schema if required.
        /// Must be called once before any read/write operations.
        /// </summary>
        public void Initialize()
        {
            EnsureNotDisposed();

            _connection = new SqliteConnection(_connectionString);
            _connection.Open();

            // Enable WAL mode for better concurrent performance
            Execute("PRAGMA journal_mode=WAL;");

            if (!TableExists("schema_info"))
            {
                CreateSchema();
            }

            IsInitialized = true;
        }

        public T Get<T>(string category, string key, T defaultValue = default)
        {
            EnsureReady();

            const string sql = "SELECT value FROM settings WHERE category = @category AND key = @key;";
            using SqliteCommand cmd = _connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@category", category);
            cmd.Parameters.AddWithValue("@key", key);

            object result = cmd.ExecuteScalar();
            if (result is null or DBNull)
                return defaultValue;

            return ConvertValue<T>((string)result);
        }

        public void Set<T>(string category, string key, T value)
        {
            EnsureReady();

            string serialized = SerializeValue(value);

            const string sql = """
                INSERT INTO settings (category, key, value, value_type, updated_at)
                VALUES (@category, @key, @value, @type, @now)
                ON CONFLICT(category, key) DO UPDATE SET
                    value = excluded.value,
                    value_type = excluded.value_type,
                    updated_at = excluded.updated_at;
                """;

            using SqliteCommand cmd = _connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@category", category);
            cmd.Parameters.AddWithValue("@key", key);
            cmd.Parameters.AddWithValue("@value", serialized ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@type", GetValueTypeName(typeof(T)));
            cmd.Parameters.AddWithValue("@now", DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture));
            cmd.ExecuteNonQuery();
        }

        public bool Remove(string category, string key)
        {
            EnsureReady();

            const string sql = "DELETE FROM settings WHERE category = @category AND key = @key;";
            using SqliteCommand cmd = _connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@category", category);
            cmd.Parameters.AddWithValue("@key", key);
            return cmd.ExecuteNonQuery() > 0;
        }

        public IReadOnlyDictionary<string, string> GetAll(string category)
        {
            EnsureReady();

            Dictionary<string, string> results = new(StringComparer.OrdinalIgnoreCase);
            const string sql = "SELECT key, value FROM settings WHERE category = @category;";
            using SqliteCommand cmd = _connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@category", category);

            using SqliteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results[reader.GetString(0)] = reader.IsDBNull(1) ? null : reader.GetString(1);
            }

            return results;
        }

        public IReadOnlyList<string> GetCategories()
        {
            EnsureReady();

            List<string> categories = [];
            const string sql = "SELECT DISTINCT category FROM settings ORDER BY category;";
            using SqliteCommand cmd = _connection.CreateCommand();
            cmd.CommandText = sql;

            using SqliteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                categories.Add(reader.GetString(0));
            }

            return categories;
        }

        public bool Exists(string category, string key)
        {
            EnsureReady();

            const string sql = "SELECT COUNT(1) FROM settings WHERE category = @category AND key = @key;";
            using SqliteCommand cmd = _connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@category", category);
            cmd.Parameters.AddWithValue("@key", key);
            return Convert.ToInt64(cmd.ExecuteScalar()) > 0;
        }

        public void Flush()
        {
            EnsureReady();
            Execute("PRAGMA wal_checkpoint(TRUNCATE);");
        }

        #region Option Management (Development Features)

        /// <summary>
        /// Retrieves all options from the store.
        /// </summary>
        public async Task<IEnumerable<OptionInfo>> GetAllOptionsAsync()
        {
            EnsureReady();

            return await Task.Run(() =>
            {
                List<OptionInfo> results = new();
                const string sql = """
                    SELECT id, key, value, category, description, option_type, created_at, modified_at 
                    FROM options
                    ORDER BY category, key;
                    """;

                using SqliteCommand cmd = _connection.CreateCommand();
                cmd.CommandText = sql;

                using SqliteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(ReadOptionFromReader(reader));
                }

                return results;
            });
        }

        /// <summary>
        /// Retrieves a specific option by its key.
        /// </summary>
        public async Task<OptionInfo> GetOptionByKeyAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or whitespace.", nameof(key));

            EnsureReady();

            return await Task.Run(() =>
            {
                const string sql = """
                    SELECT id, key, value, category, description, option_type, created_at, modified_at 
                    FROM options 
                    WHERE key = @key;
                    """;

                using SqliteCommand cmd = _connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@key", key);

                using SqliteDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return ReadOptionFromReader(reader);
                }

                return null;
            });
        }

        /// <summary>
        /// Retrieves a specific option by its ID.
        /// </summary>
        public async Task<OptionInfo> GetOptionByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID must be greater than 0.", nameof(id));

            EnsureReady();

            return await Task.Run(() =>
            {
                const string sql = """
                    SELECT id, key, value, category, description, option_type, created_at, modified_at 
                    FROM options 
                    WHERE id = @id;
                    """;

                using SqliteCommand cmd = _connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@id", id);

                using SqliteDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return ReadOptionFromReader(reader);
                }

                return null;
            });
        }

        /// <summary>
        /// Retrieves options filtered by category.
        /// </summary>
        public async Task<IEnumerable<OptionInfo>> GetOptionsByCategoryAsync(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException("Category cannot be null or whitespace.", nameof(category));

            EnsureReady();

            return await Task.Run(() =>
            {
                List<OptionInfo> results = new();
                const string sql = """
                    SELECT id, key, value, category, description, option_type, created_at, modified_at 
                    FROM options 
                    WHERE category = @category
                    ORDER BY key;
                    """;

                using SqliteCommand cmd = _connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@category", category);

                using SqliteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(ReadOptionFromReader(reader));
                }

                return results;
            });
        }

        /// <summary>
        /// Adds a new option to the store.
        /// </summary>
        public async Task<OptionInfo> AddOptionAsync(OptionInfo option)
        {
            if (option == null)
                throw new ArgumentNullException(nameof(option));
            if (string.IsNullOrWhiteSpace(option.Key))
                throw new ArgumentException("Option key cannot be null or whitespace.", nameof(option));

            EnsureReady();

            return await Task.Run(() =>
            {
                // Check if option key already exists
                if (OptionKeyExists(option.Key))
                {
                    throw new InvalidOperationException($"An option with key '{option.Key}' already exists.");
                }

                const string sql = """
                    INSERT INTO options (key, value, category, description, option_type, created_at, modified_at)
                    VALUES (@key, @value, @category, @description, @option_type, @created_at, @modified_at);
                    SELECT last_insert_rowid();
                    """;

                using SqliteCommand cmd = _connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@key", option.Key);
                cmd.Parameters.AddWithValue("@value", option.Value ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@category", option.Category ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@description", option.Description ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@option_type", option.OptionType ?? "string");
                cmd.Parameters.AddWithValue("@created_at", DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture));
                cmd.Parameters.AddWithValue("@modified_at", DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture));

                long id = (long)cmd.ExecuteScalar();
                option.Id = (int)id;
                option.CreatedDate = DateTime.UtcNow;
                option.ModifiedDate = DateTime.UtcNow;

                return option;
            });
        }

        /// <summary>
        /// Updates an existing option in the store.
        /// </summary>
        public async Task<bool> UpdateOptionAsync(OptionInfo option)
        {
            if (option == null)
                throw new ArgumentNullException(nameof(option));
            if (option.Id <= 0)
                throw new ArgumentException("Option ID must be greater than 0.", nameof(option));

            EnsureReady();

            return await Task.Run(() =>
            {
                const string sql = """
                    UPDATE options 
                    SET value = @value, 
                        category = @category, 
                        description = @description, 
                        option_type = @option_type,
                        modified_at = @modified_at
                    WHERE id = @id;
                    """;

                using SqliteCommand cmd = _connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@id", option.Id);
                cmd.Parameters.AddWithValue("@value", option.Value ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@category", option.Category ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@description", option.Description ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@option_type", option.OptionType ?? "string");
                cmd.Parameters.AddWithValue("@modified_at", DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture));

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    option.ModifiedDate = DateTime.UtcNow;
                }

                return rowsAffected > 0;
            });
        }

        /// <summary>
        /// Deletes an option from the store by its ID.
        /// </summary>
        public async Task<bool> DeleteOptionAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID must be greater than 0.", nameof(id));

            EnsureReady();

            return await Task.Run(() =>
            {
                const string sql = "DELETE FROM options WHERE id = @id;";

                using SqliteCommand cmd = _connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@id", id);

                return cmd.ExecuteNonQuery() > 0;
            });
        }

        /// <summary>
        /// Deletes an option from the store by its key.
        /// </summary>
        public async Task<bool> DeleteOptionByKeyAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or whitespace.", nameof(key));

            EnsureReady();

            return await Task.Run(() =>
            {
                const string sql = "DELETE FROM options WHERE key = @key;";

                using SqliteCommand cmd = _connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@key", key);

                return cmd.ExecuteNonQuery() > 0;
            });
        }

        /// <summary>
        /// Checks if an option with the given key exists.
        /// </summary>
        public async Task<bool> OptionExistsAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or whitespace.", nameof(key));

            EnsureReady();

            return await Task.Run(() =>
            {
                const string sql = "SELECT COUNT(1) FROM options WHERE key = @key;";

                using SqliteCommand cmd = _connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@key", key);

                long count = (long)cmd.ExecuteScalar();
                return count > 0;
            });
        }

        /// <summary>
        /// Gets the count of options in the store.
        /// </summary>
        public async Task<int> GetOptionCountAsync()
        {
            EnsureReady();

            return await Task.Run(() =>
            {
                const string sql = "SELECT COUNT(1) FROM options;";

                using SqliteCommand cmd = _connection.CreateCommand();
                cmd.CommandText = sql;

                long count = (long)cmd.ExecuteScalar();
                return (int)count;
            });
        }

        /// <summary>
        /// Clears all options from the store.
        /// </summary>
        public async Task ClearAllOptionsAsync()
        {
            EnsureReady();

            await Task.Run(() =>
            {
                const string sql = "DELETE FROM options;";

                using SqliteCommand cmd = _connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            });
        }

        /// <summary>
        /// Exports the options schema and data as SQL statements.
        /// Includes CREATE TABLE, CREATE INDEX, and INSERT statements for all options.
        /// </summary>
        public async Task<string> ExportSchemaAsync()
        {
            EnsureReady();

            return await Task.Run(() =>
            {
                List<string> statements = new();

                // Export schema (table and indices)
                const string schemaSql = """
                    SELECT sql
                    FROM sqlite_master
                    WHERE sql IS NOT NULL
                      AND ((type = 'table' AND name = 'options')
                           OR (type = 'index' AND name LIKE 'idx_options_%'))
                    ORDER BY CASE type WHEN 'table' THEN 0 ELSE 1 END, name;
                    """;

                using (SqliteCommand cmd = _connection.CreateCommand())
                {
                    cmd.CommandText = schemaSql;
                    using SqliteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string statement = reader.IsDBNull(0) ? null : reader.GetString(0);
                        if (string.IsNullOrWhiteSpace(statement))
                            continue;

                        statements.Add(statement.Trim().TrimEnd(';') + ";");
                    }
                }

                // Export data as INSERT statements
                const string dataSql = """
                    SELECT id, key, value, category, description, option_type, created_at, modified_at
                    FROM options
                    ORDER BY id;
                    """;

                using (SqliteCommand cmd = _connection.CreateCommand())
                {
                    cmd.CommandText = dataSql;
                    using SqliteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string key = reader.GetString(1);
                        string value = reader.IsDBNull(2) ? null : reader.GetString(2);
                        string category = reader.IsDBNull(3) ? null : reader.GetString(3);
                        string description = reader.IsDBNull(4) ? null : reader.GetString(4);
                        string optionType = reader.IsDBNull(5) ? "string" : reader.GetString(5);
                        string createdAt = reader.IsDBNull(6) ? DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture) : reader.GetString(6);
                        string modifiedAt = reader.IsDBNull(7) ? DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture) : reader.GetString(7);

                        string insertSql = $"""
                            INSERT INTO options (id, key, value, category, description, option_type, created_at, modified_at)
                            VALUES ({id}, '{EscapeSqlString(key)}', {(value == null ? "NULL" : $"'{EscapeSqlString(value)}'")}, {(category == null ? "NULL" : $"'{EscapeSqlString(category)}'")}, {(description == null ? "NULL" : $"'{EscapeSqlString(description)}'")}, '{EscapeSqlString(optionType)}', '{createdAt}', '{modifiedAt}');
                            """;

                        statements.Add(insertSql);
                    }
                }

                return string.Join(Environment.NewLine + Environment.NewLine, statements);
            });
        }

        private static string EscapeSqlString(string input)
        {
            return input?.Replace("'", "''") ?? string.Empty;
        }

        /// <summary>
        /// Imports and applies an options schema SQL script.
        /// Existing options schema objects are recreated.
        /// </summary>
        public async Task ImportSchemaAsync(string schemaSql)
        {
            if (string.IsNullOrWhiteSpace(schemaSql))
                throw new ArgumentException("Schema SQL cannot be null or whitespace.", nameof(schemaSql));

            EnsureReady();

            await Task.Run(() =>
            {
                using SqliteTransaction transaction = _connection.BeginTransaction();

                using (SqliteCommand dropCommand = _connection.CreateCommand())
                {
                    dropCommand.Transaction = transaction;
                    dropCommand.CommandText = "DROP TABLE IF EXISTS options;";
                    dropCommand.ExecuteNonQuery();
                }

                using (SqliteCommand importCommand = _connection.CreateCommand())
                {
                    importCommand.Transaction = transaction;
                    importCommand.CommandText = schemaSql;
                    importCommand.ExecuteNonQuery();
                }

                transaction.Commit();
            });
        }

        private bool OptionKeyExists(string key)
        {
            const string sql = "SELECT COUNT(1) FROM options WHERE key = @key;";
            using SqliteCommand cmd = _connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@key", key);
            long count = (long)cmd.ExecuteScalar();
            return count > 0;
        }

        private static OptionInfo ReadOptionFromReader(SqliteDataReader reader)
        {
            return new OptionInfo
            {
                Id = reader.GetInt32(0),
                Key = reader.GetString(1),
                Value = reader.IsDBNull(2) ? null : reader.GetString(2),
                Category = reader.IsDBNull(3) ? null : reader.GetString(3),
                Description = reader.IsDBNull(4) ? null : reader.GetString(4),
                OptionType = reader.IsDBNull(5) ? "string" : reader.GetString(5),
                CreatedDate = DateTime.Parse(reader.GetString(6), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                ModifiedDate = DateTime.Parse(reader.GetString(7), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)
            };
        }

        #endregion

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            if (_connection is { State: System.Data.ConnectionState.Open })
            {
                _connection.Close();
            }

            _connection?.Dispose();

            // Clear the connection pool so SQLite releases the file lock,
            // allowing callers to delete or move the database file.
            SqliteConnection.ClearAllPools();
        }

        #region Schema

        private void CreateSchema()
        {
            const string ddl = """
                CREATE TABLE IF NOT EXISTS schema_info (
                    version     INTEGER NOT NULL,
                    applied_at  TEXT    NOT NULL DEFAULT (datetime('now')),
                    description TEXT
                );

                CREATE TABLE IF NOT EXISTS settings (
                    id          INTEGER PRIMARY KEY AUTOINCREMENT,
                    category    TEXT    NOT NULL,
                    key         TEXT    NOT NULL,
                    value       TEXT,
                    value_type  TEXT    NOT NULL DEFAULT 'string',
                    created_at  TEXT    NOT NULL DEFAULT (datetime('now')),
                    updated_at  TEXT    NOT NULL DEFAULT (datetime('now')),
                    UNIQUE(category, key)
                );

                CREATE INDEX IF NOT EXISTS idx_settings_category_key ON settings(category, key);

                CREATE TABLE IF NOT EXISTS options (
                    id          INTEGER PRIMARY KEY AUTOINCREMENT,
                    key         TEXT    NOT NULL UNIQUE,
                    value       TEXT,
                    category    TEXT,
                    description TEXT,
                    option_type TEXT    NOT NULL DEFAULT 'string',
                    created_at  TEXT    NOT NULL DEFAULT (datetime('now')),
                    modified_at TEXT    NOT NULL DEFAULT (datetime('now'))
                );

                CREATE INDEX IF NOT EXISTS idx_options_key ON options(key);
                CREATE INDEX IF NOT EXISTS idx_options_category ON options(category);

                CREATE TABLE IF NOT EXISTS external_apps (
                    id           TEXT PRIMARY KEY,
                    display_name TEXT NOT NULL,
                    file_name    TEXT,
                    arguments    TEXT,
                    options      TEXT,
                    sort_order   INTEGER NOT NULL DEFAULT 0
                );

                CREATE TABLE IF NOT EXISTS panel_layout (
                    id       INTEGER PRIMARY KEY,
                    xml      TEXT NOT NULL,
                    saved_at TEXT NOT NULL DEFAULT (datetime('now'))
                );

                INSERT INTO schema_info (version, description)
                VALUES (1, 'Initial settings schema');
                """;

            Execute(ddl);
        }

        private int GetSchemaVersion()
        {
            if (_connection is null || !TableExists("schema_info"))
                return 0;

            const string sql = "SELECT MAX(version) FROM schema_info;";
            using SqliteCommand cmd = _connection.CreateCommand();
            cmd.CommandText = sql;
            object result = cmd.ExecuteScalar();
            return result is long v ? (int)v : 0;
        }

        private bool TableExists(string tableName)
        {
            const string sql = "SELECT COUNT(1) FROM sqlite_master WHERE type='table' AND name=@name;";
            using SqliteCommand cmd = _connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@name", tableName);
            return Convert.ToInt64(cmd.ExecuteScalar()) > 0;
        }

        #endregion

        #region Helpers

        private void Execute(string sql)
        {
            using SqliteCommand cmd = _connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        private void EnsureReady()
        {
            EnsureNotDisposed();
            if (!IsInitialized)
                throw new InvalidOperationException("Settings store has not been initialized. Call Initialize() first.");
        }

        private void EnsureNotDisposed()
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
        }

        private static string SerializeValue<T>(T value)
        {
            if (value is null) return null;

            return value switch
            {
                string s => s,
                bool b => b ? "true" : "false",
                int or long or double or float or decimal => Convert.ToString(value, CultureInfo.InvariantCulture),
                DateTime dt => dt.ToString("o", CultureInfo.InvariantCulture),
                System.Drawing.Point p => $"{p.X},{p.Y}",
                System.Drawing.Size s => $"{s.Width},{s.Height}",
                Enum e => e.ToString(),
                _ => System.Text.Json.JsonSerializer.Serialize(value)
            };
        }

        private static T ConvertValue<T>(string raw)
        {
            if (raw is null) return default;

            Type targetType = typeof(T);

            if (targetType == typeof(string))
                return (T)(object)raw;

            if (targetType == typeof(bool))
                return (T)(object)(raw.Equals("true", StringComparison.OrdinalIgnoreCase) || raw == "1");

            if (targetType == typeof(int))
                return (T)(object)int.Parse(raw, CultureInfo.InvariantCulture);

            if (targetType == typeof(long))
                return (T)(object)long.Parse(raw, CultureInfo.InvariantCulture);

            if (targetType == typeof(double))
                return (T)(object)double.Parse(raw, CultureInfo.InvariantCulture);

            if (targetType == typeof(float))
                return (T)(object)float.Parse(raw, CultureInfo.InvariantCulture);

            if (targetType == typeof(decimal))
                return (T)(object)decimal.Parse(raw, CultureInfo.InvariantCulture);

            if (targetType == typeof(DateTime))
                return (T)(object)DateTime.Parse(raw, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);

            if (targetType == typeof(System.Drawing.Point))
            {
                string[] parts = raw.Split(',');
                return (T)(object)new System.Drawing.Point(int.Parse(parts[0].Trim()), int.Parse(parts[1].Trim()));
            }

            if (targetType == typeof(System.Drawing.Size))
            {
                string[] parts = raw.Split(',');
                return (T)(object)new System.Drawing.Size(int.Parse(parts[0].Trim()), int.Parse(parts[1].Trim()));
            }

            if (targetType.IsEnum)
                return (T)Enum.Parse(targetType, raw, ignoreCase: true);

            // Fallback: JSON deserialization
            return System.Text.Json.JsonSerializer.Deserialize<T>(raw);
        }

        private static string GetValueTypeName(Type type)
        {
            if (type == typeof(string)) return "string";
            if (type == typeof(bool)) return "bool";
            if (type == typeof(int) || type == typeof(long)) return "int";
            if (type == typeof(double) || type == typeof(float) || type == typeof(decimal)) return "float";
            if (type == typeof(DateTime)) return "datetime";
            if (type == typeof(System.Drawing.Point)) return "point";
            if (type == typeof(System.Drawing.Size)) return "size";
            if (type.IsEnum) return "enum";
            return "json";
        }

        #endregion
    }
}
