using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Linq;
using LiteDB;
using mRemoteNG.Config.MachineIdentifier;
using System.Runtime.Versioning;

namespace mRemoteNG.Config.Settings;

[SupportedOSPlatform("windows")]
public class LocalDBManager
{
    private static readonly JsonSerializerOptions s_jsonOptions = new() { WriteIndented = true };
    private readonly string _dbPath;
    private readonly string _schemaPath;
    private readonly string _mRIdentifier = string.Empty; // Initialize to non-null default
    private readonly bool? _useEncryption;

  
    /// <summary>
    /// Creates a new local DB, encrypt it or decrypt it.
    /// </summary>
    /// <param name="dbPath">The path to the database file.</param>
    /// <param name="useEncryption">Indicates whether to use encryption for the database. If null, no change is made to an existing database.</param>
    /// <param name="schemaFilePath">Optional path to a schema file for creating the database structure.</param>
    public LocalDBManager(string? dbPath = null, bool? useEncryption = null, string? schemaFilePath = null)
    {
        _dbPath = string.IsNullOrWhiteSpace(dbPath) ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mRemoteNG.appSettings") : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dbPath);
        _schemaPath = string.IsNullOrWhiteSpace(schemaFilePath) ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Schemas\\mremoteng_default_settings_v1_0.json") : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, schemaFilePath);
        _useEncryption = useEncryption;
        
        /// <summary>
        /// Generate a unique identifier for the machine
        /// </summary>
        
        try
        {
            // Generate the machine identifier
            _mRIdentifier = MachineIdentifierGenerator.GenerateMachineIdentifier();
            Console.WriteLine($"Generated Identifier: {_mRIdentifier}");
        }
        catch (PlatformNotSupportedException ex)
        {
            Console.WriteLine(ex.Message);
            _mRIdentifier = string.Empty; // Ensure initialization on error
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            _mRIdentifier = string.Empty; // Ensure initialization on error
        }


        // Check if disk identifier is empty and prevent database creation if true
        if (string.IsNullOrEmpty(_mRIdentifier))
        {
            Console.WriteLine("Calculated identifier is empty. Database creation aborted.");
            return;
        }

        // Check if the database exists and handle accordingly
        if (!File.Exists(_dbPath))
        {
            CreateDatabase(_schemaPath);
        }
        else if (_useEncryption.HasValue)
        {
            if (_useEncryption.Value)
            {
                EncryptDatabase();
            }
            else
            {
                DecryptDatabase();
            }
        }
    }


    /// <summary>
    /// Ensures default settings are imported if the database is empty.
    /// </summary>
    /// <param name="importFilePath">Path to the JSON file for importing default settings.</param>
    public void EnsureDefaultSettingsImported(string importFilePath)
    {
        var connectionString = _useEncryption.HasValue && _useEncryption.Value
            ? $"Filename={_dbPath};Password={_mRIdentifier}"
            : $"Filename={_dbPath}";

        using (var db = new LiteDatabase(connectionString))
        {
            if (db.GetCollectionNames().All(name => db.GetCollection<Setting>(name).Count() == 0))
            {
                Console.WriteLine("No settings found in database. Importing default settings...");
                ImportSettings(importFilePath);
            }
            else
            {
                Console.WriteLine("Database already contains settings. Skipping import.");
            }
        }
    }

    /// <summary>
    /// Checks if the database is encrypted.
    /// </summary>
    /// <returns>True if the database is encrypted, otherwise false.</returns>
    private bool IsDatabaseEncrypted()
    {
        try
        {
            using (var db = new LiteDatabase($"Filename={_dbPath}"))
            {
                // If we can open the database without a password, it means it is not encrypted.
                return false;
            }
        }
        catch (LiteException)
        {
            // If an exception is thrown, it means the database is likely encrypted.
            return true;
        }
    }

    /// <summary>
    /// Creates the database using the machine identifier as a password if encryption is enabled.
    /// </summary>
    /// <param name="schemaFilePath">Path to the schema file for creating the database structure.</param>
    private void CreateDatabase(string? schemaFilePath = null)
    {
        var connectionString = _useEncryption.HasValue && _useEncryption.Value
            ? $"Filename={_dbPath};Password={_mRIdentifier}"
            : $"Filename={_dbPath}";
        using (var db = new LiteDatabase(connectionString))
        {
            if (!string.IsNullOrWhiteSpace(schemaFilePath) && File.Exists(schemaFilePath))
            {
                if (string.IsNullOrWhiteSpace(schemaFilePath) || schemaFilePath.Contains("../", StringComparison.Ordinal) || schemaFilePath.Contains(@"..\", StringComparison.Ordinal))
                {
                    throw new ArgumentException("Invalid file path", nameof(schemaFilePath));
                }
                var schemaJson = File.ReadAllText(schemaFilePath);
                using (JsonDocument doc = JsonDocument.Parse(schemaJson))
                {
                    foreach (JsonElement table in doc.RootElement.GetProperty("tables").EnumerateArray())
                    {
                        string tableName = table.GetProperty("name").GetString() ?? string.Empty;
                        var collection = db.GetCollection<Setting>(tableName);
                        Console.WriteLine($"Table '{tableName}' created with structure from schema.");

                        // Insert default data into the collection if defined in the schema
                        if (table.TryGetProperty("columns", out JsonElement columnsElement))
                        {
                            foreach (JsonElement column in columnsElement.EnumerateArray())
                            {
                                var settingsData = new Setting
                                {
                                    Id = Guid.NewGuid(),
                                    Timestamp = DateTime.UtcNow,
                                    Group = "default",
                                    Key = column.GetProperty("name").GetString() ?? string.Empty,
                                    Value = column.GetProperty("value").ToString()
                                };
                                collection.Insert(settingsData);
                                Console.WriteLine($"Inserted default setting '{settingsData.Key}' for table '{tableName}'.");
                            }
                        }
                        Console.WriteLine($"Inserted default settings for table '{tableName}'.");
                    }
                }
            }
        }
        Console.WriteLine(_useEncryption.HasValue && _useEncryption.Value ? "Database created and encrypted." : "Database created without encryption.");
    }


/// <summary>
/// Encrypts an existing database if it is not encrypted.
/// </summary>
public void EncryptDatabase()
    {
        try
        {
            using (var db = new LiteDatabase($"Filename={_dbPath}"))
            {
                Console.WriteLine("Encrypting database...");
                var backupPath = _dbPath + ".backup";
                db.Checkpoint();
                File.Copy(_dbPath, backupPath, true);

                using (var encryptedDb = new LiteDatabase($"Filename={_dbPath};Password={_mRIdentifier}"))
                {
                    encryptedDb.Checkpoint();
                }

                File.Delete(backupPath);
                Console.WriteLine("Database successfully encrypted.");
            }
        }
        catch (LiteException ex)
        {
            Console.WriteLine($"Error encrypting database: {ex.Message}");
        }
    }

    /// <summary>
    /// Decrypts an existing database if it is encrypted.
    /// </summary>
    public void DecryptDatabase()
    {
        try
        {
            if (!IsDatabaseEncrypted())
            {
                Console.WriteLine("Database is not encrypted. Skipping decryption.");
                return;
            }
            var encryptedConnectionString = $"Filename={_dbPath};Password={_mRIdentifier}";
            using (var db = new LiteDatabase(encryptedConnectionString))
            {
                Console.WriteLine("Decrypting database...");
                var backupPath = _dbPath + ".backup";
                db.Checkpoint();
                File.Copy(_dbPath, backupPath, true);

                using (var decryptedDb = new LiteDatabase($"Filename={_dbPath}"))
                {
                    decryptedDb.Checkpoint();
                }

                File.Delete(backupPath);
                Console.WriteLine("Database successfully decrypted.");
            }
        }
        catch (LiteException ex)
        {
            Console.WriteLine($"Error decrypting database: {ex.Message}");
        }
    }

    /// <summary>
    /// Adds a new setting to the database.
    /// </summary>
    /// <param name="table">Table name.</param>
    /// <param name="group">Setting group.</param>
    /// <param name="key">Setting key.</param>
    /// <param name="value">Setting value.</param>
    public void AddSetting(string table, string group, string key, string value)
    {
        var connectionString = _useEncryption.HasValue && _useEncryption.Value
            ? $"Filename={_dbPath};Password={_mRIdentifier}"
            : $"Filename={_dbPath}";

        using (var db = new LiteDatabase(connectionString))
        {
            var settings = db.GetCollection<Setting>(table);
            var setting = new Setting
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                Group = group,
                Key = key,
                Value = value
            };
            settings.Insert(setting);
            Console.WriteLine($"Setting '{group}.{key}' added to table '{table}'.");
        }
    }

    /// <summary>
    /// Imports settings from a JSON file into the database.
    /// </summary>
    /// <param name="jsonFilePath">Path to the JSON file.</param>
    public void ImportSettings(string jsonFilePath)
    {
        if (File.Exists(jsonFilePath))
        {
            if (jsonFilePath == null || jsonFilePath.Contains("../", StringComparison.Ordinal) || jsonFilePath.Contains(@"..\", StringComparison.Ordinal))
            {
                throw new ArgumentException("Invalid file path", nameof(jsonFilePath));
            }
            var json = File.ReadAllText(jsonFilePath);
            var settingsData = JsonSerializer.Deserialize<Dictionary<string, List<Setting>>>(json);
            if (settingsData == null)
            {
                Console.WriteLine("Failed to deserialize settings from JSON file.");
                return;
            }

            foreach (var table in settingsData.Keys)
            {
                foreach (var setting in settingsData[table])
                {
                    AddSetting(table, setting.Group, setting.Key, setting.Value);
                }
            }
            Console.WriteLine("Settings successfully imported from JSON file.");
        }
        else
        {
            Console.WriteLine("JSON file not found.");
        }
    }

    /// <summary>
    /// Exports settings from the database to a JSON file.
    /// </summary>
    /// <param name="jsonFilePath">Path to the JSON file.</param>
    public void ExportSettings(string jsonFilePath)
    {
        var connectionString = _useEncryption.HasValue && _useEncryption.Value
            ? $"Filename={_dbPath};Password={_mRIdentifier}"
            : $"Filename={_dbPath}";

        using (var db = new LiteDatabase(connectionString))
        {
            var settingsData = new Dictionary<string, List<Setting>>(StringComparer.Ordinal);

            foreach (var tableName in db.GetCollectionNames())
            {
                var settings = db.GetCollection<Setting>(tableName).FindAll();
                settingsData[tableName] = new List<Setting>(settings);
            }

            var json = JsonSerializer.Serialize(settingsData, s_jsonOptions);
            if (jsonFilePath == null || jsonFilePath.Contains("../", StringComparison.Ordinal) || jsonFilePath.Contains(@"..\", StringComparison.Ordinal))
            {
                throw new ArgumentException("Invalid file path", nameof(jsonFilePath));
            }
            File.WriteAllText(jsonFilePath, json);
            Console.WriteLine("Settings successfully exported to JSON file.");
        }
    }

    /// <summary>
    /// Retrieves the value of a setting by table, group, and key.
    /// </summary>
    /// <param name="table">Table name.</param>
    /// <param name="group">Setting group.</param>
    /// <param name="key">Setting key.</param>
    /// <returns>Setting value or "Not Found" if the setting does not exist.</returns>
    public string GetSetting(string table, string group, string key)
    {
        var connectionString = _useEncryption.HasValue && _useEncryption.Value
            ? $"Filename={_dbPath};Password={_mRIdentifier}"
            : $"Filename={_dbPath}";

        using (var db = new LiteDatabase(connectionString))
        {
            var settings = db.GetCollection<Setting>(table);
            var setting = settings.FindOne(s => s.Group == group && s.Key == key);
            return setting != null ? setting.Value : "Not Found";
        }
    }

    /// <summary>
    /// Updates the value of an existing setting and updates the timestamp.
    /// </summary>
    /// <param name="table">Table name.</param>
    /// <param name="group">Setting group.</param>
    /// <param name="key">Setting key.</param>
    /// <param name="newValue">New value for the setting.</param>
    public void UpdateSetting(string table, string group, string key, string newValue)
    {
        var connectionString = _useEncryption.HasValue && _useEncryption.Value
            ? $"Filename={_dbPath};Password={_mRIdentifier}"
            : $"Filename={_dbPath}";

        using (var db = new LiteDatabase(connectionString))
        {
            var settings = db.GetCollection<Setting>(table);
            var setting = settings.FindOne(s => s.Group == group && s.Key == key);
            if (setting != null)
            {
                setting.Value = newValue;
                setting.Timestamp = DateTime.UtcNow;
                settings.Update(setting);
                Console.WriteLine($"Setting '{group}.{key}' updated in table '{table}'.");
            }
            else
            {
                Console.WriteLine($"Setting '{group}.{key}' not found in table '{table}'.");
            }
        }
    }

    /// <summary>
    /// Deletes a setting by table, group, and key.
    /// </summary>
    /// <param name="table">Table name.</param>
    /// <param name="group">Setting group.</param>
    /// <param name="key">Setting key.</param>
    public void DeleteSetting(string table, string group, string key)
    {
        var connectionString = _useEncryption.HasValue && _useEncryption.Value
            ? $"Filename={_dbPath};Password={_mRIdentifier}"
            : $"Filename={_dbPath}";

        using (var db = new LiteDatabase(connectionString))
        {
            var settings = db.GetCollection<Setting>(table);
            if (settings.DeleteMany(s => s.Group == group && s.Key == key) > 0)
            {
                Console.WriteLine($"Setting '{group}.{key}' deleted from table '{table}'.");
            }
            else
            {
                Console.WriteLine($"Setting '{group}.{key}' not found in table '{table}'.");
            }
        }
    }


    // Setting class
    public class Setting
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Group { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}