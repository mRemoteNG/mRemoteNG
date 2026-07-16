using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace mRemoteNG.Config.Settings.Store
{
    /// <summary>
    /// SQLite-backed options store for development-only option management.
    /// Provides asynchronous CRUD operations for options that can be added/edited/deleted at runtime.
    /// </summary>
    public class OptionsStore : IDisposable
    {
        private readonly string _dbPath;
        private readonly string _connectionString;
        private SqliteConnection _connection;
        private bool _disposed;

        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Creates a new OptionsStore instance.
        /// </summary>
        /// <param name="dbPath">Full path to the SQLite database file.</param>
        /// <param name="dekHex">
        /// Optional hex-encoded 256-bit data encryption key.
        /// When provided, SQLite encryption is used (requires SQLCipher bundle or compatible provider).
        /// Pass <c>null</c> for an unencrypted database.
        /// </param>
        public OptionsStore(string dbPath, string dekHex = null)
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
        /// Initializes the database, creating the schema if required.
        /// Must be called once before any read/write operations.
        /// </summary>
        public void Initialize()
        {
            EnsureNotDisposed();

            _connection = new SqliteConnection(_connectionString);
            _connection.Open();

            // Enable WAL mode for better concurrent performance
            Execute("PRAGMA journal_mode=WAL;");

            if (!TableExists("options"))
            {
                CreateSchema();
            }

            IsInitialized = true;
        }

        private void CreateSchema()
        {
            const string ddl = """
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
                """;

            Execute(ddl);
        }

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
        /// Flushes pending changes to disk.
        /// </summary>
        public void Flush()
        {
            EnsureReady();
            Execute("PRAGMA wal_checkpoint(TRUNCATE);");
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            if (_connection is { State: System.Data.ConnectionState.Open })
            {
                _connection.Close();
            }

            _connection?.Dispose();
            SqliteConnection.ClearAllPools();
        }

        #region Private Helpers

        private OptionInfo ReadOptionFromReader(SqliteDataReader reader)
        {
            return new OptionInfo
            {
                Id = reader.GetInt32(0),
                Key = reader.GetString(1),
                Value = reader.IsDBNull(2) ? null : reader.GetString(2),
                Category = reader.IsDBNull(3) ? null : reader.GetString(3),
                Description = reader.IsDBNull(4) ? null : reader.GetString(4),
                OptionType = reader.IsDBNull(5) ? "string" : reader.GetString(5),
                CreatedDate = reader.IsDBNull(6) ? DateTime.UtcNow : DateTime.Parse(reader.GetString(6), CultureInfo.InvariantCulture),
                ModifiedDate = reader.IsDBNull(7) ? DateTime.UtcNow : DateTime.Parse(reader.GetString(7), CultureInfo.InvariantCulture)
            };
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

        private bool TableExists(string tableName)
        {
            const string sql = "SELECT COUNT(1) FROM sqlite_master WHERE type='table' AND name=@table;";

            using SqliteCommand cmd = _connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@table", tableName);

            long count = (long)cmd.ExecuteScalar();
            return count > 0;
        }

        private void Execute(string sql)
        {
            using SqliteCommand cmd = _connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        private void EnsureNotDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        private void EnsureReady()
        {
            EnsureNotDisposed();
            if (!IsInitialized)
                throw new InvalidOperationException($"{nameof(OptionsStore)} has not been initialized. Call Initialize() first.");
        }

        #endregion
    }
}
