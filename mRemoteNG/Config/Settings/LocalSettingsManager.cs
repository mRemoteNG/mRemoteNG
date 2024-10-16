using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Management;
using JsonSerializer = System.Text.Json.JsonSerializer;
using LiteDB;
using System.Linq;

public class LocalSettingsDBManager
{
    private readonly string _dbPath;
    private readonly string _schemaPath;
    private readonly string _mRIdentifier;
    private readonly bool? _useEncryption;

  
    /// <summary>
    /// Creates a new local DB, encrypt it or decrypt it.
    /// </summary>
    /// <param name="dbPath">The path to the database file.</param>
    /// <param name="useEncryption">Indicates whether to use encryption for the database. If null, no change is made to an existing database.</param>
    /// <param name="schemaFilePath">Optional path to a schema file for creating the database structure.</param>
    public LocalSettingsDBManager(string dbPath = null, bool? useEncryption = null, string schemaFilePath = null)
    {
        _dbPath = string.IsNullOrWhiteSpace(dbPath) ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mRemoteNG.appSettings") : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dbPath);
        _schemaPath = string.IsNullOrWhiteSpace(schemaFilePath) ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Schemas\\mremoteng_default_settings_v1_0.json") : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, schemaFilePath);
        _useEncryption = useEncryption;
        _mRIdentifier = Convert.ToBase64String(System.Security.Cryptography.SHA256.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(GetDiskIdentifier() + "_" + Environment.MachineName)));

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
    private void CreateDatabase(string schemaFilePath = null)
    {
        var connectionString = _useEncryption.HasValue && _useEncryption.Value
            ? $"Filename={_dbPath};Password={_mRIdentifier}"
            : $"Filename={_dbPath}";
        using (var db = new LiteDatabase(connectionString))
        {
            if (!string.IsNullOrWhiteSpace(schemaFilePath) && File.Exists(schemaFilePath))
            {
                var schemaJson = File.ReadAllText(schemaFilePath);
                using (JsonDocument doc = JsonDocument.Parse(schemaJson))
                {
                    foreach (JsonElement table in doc.RootElement.GetProperty("tables").EnumerateArray())
                    {
                        string tableName = table.GetProperty("name").GetString();
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
                                    Key = column.GetProperty("name").GetString(),
                                    Value = column.GetProperty("value").ToString()
                                };
                                collection.Insert(settingsData);
                                Console.WriteLine($"Inserted default setting '{settingsData.Key}' for table '{tableName}'.");
                            }
                        };
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
            var json = File.ReadAllText(jsonFilePath);
            var settingsData = JsonSerializer.Deserialize<Dictionary<string, List<Setting>>>(json);

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
            var settingsData = new Dictionary<string, List<Setting>>();

            foreach (var tableName in db.GetCollectionNames())
            {
                var settings = db.GetCollection<Setting>(tableName).FindAll();
                settingsData[tableName] = new List<Setting>(settings);
            }

            var json = JsonSerializer.Serialize(settingsData, new JsonSerializerOptions { WriteIndented = true });
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

    /// <summary>
    /// Gets the unique machine identifier (serial number of the hard drive) combined with the machine name and encrypts it using SHA256.
    /// </summary>
    /// <returns>Unique machine identifier.</returns>
    private static string GetDiskIdentifier()
    {
        if (OperatingSystem.IsWindows())
        {
            try
            {
                // Use ManagementObject to get the serial number of the hard drive
                using (var searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_DiskDrive"))
                {
                    foreach (var disk in searcher.Get())
                    {
                        return disk["SerialNumber"].ToString().Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting disk identifier: {ex.Message}");
                throw new InvalidOperationException("Failed to retrieve disk identifier. Please ensure the disk information is accessible.");
            }
        }
        else
        {
            throw new PlatformNotSupportedException("This method is only supported on Windows.");
        }

        // Return an empty string if no serial number is found
        return string.Empty;
    }

 

    // Setting class
    public class Setting
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Group { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}