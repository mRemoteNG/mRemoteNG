using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Security.SymmetricEncryption;

namespace mRemoteNG.Config.DatabaseConnectors
{
    public class DatabaseProfile
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = DatabaseConnectorFactory.MsSqlType;
        public string Host { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string EncryptedPassword { get; set; } = string.Empty;
        public bool ReadOnly { get; set; }
        public string AuthType { get; set; } = string.Empty; // For SQL Server specific auth types
        
        // Add other properties that are present in OptionsDBsPage if needed
        // For now covering the main ones used in SqlServerPage.cs
    }

    public static class DatabaseProfileManager
    {
        private static readonly string ProfilesPath = Path.Combine(GeneralAppInfo.HomePath, "databaseProfiles.json");
        private static readonly JsonSerializerOptions s_jsonOptions = new() { WriteIndented = true };
        private static IList<DatabaseProfile> _profiles = new List<DatabaseProfile>();

        public static IList<DatabaseProfile> Profiles
        {
            get
            {
                if (_profiles.Count == 0)
                {
                    LoadProfiles();
                }
                return _profiles;
            }
        }

        public static void LoadProfiles()
        {
            if (File.Exists(ProfilesPath))
            {
                try
                {
                    string json = File.ReadAllText(ProfilesPath);
                    var loadedProfiles = JsonSerializer.Deserialize<List<DatabaseProfile>>(json);
                    if (loadedProfiles != null)
                    {
                        _profiles = loadedProfiles;
                    }
                }
                catch (Exception ex)
                {
                    Runtime.MessageCollector.AddExceptionMessage("Failed to load database profiles", ex);
                }
            }
        }

        public static void SaveProfiles()
        {
            try
            {
                string json = JsonSerializer.Serialize(_profiles, s_jsonOptions);
                File.WriteAllText(ProfilesPath, json);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("Failed to save database profiles", ex);
            }
        }

        public static void AddProfile(DatabaseProfile profile)
        {
            // Remove existing profile with same name if any (upsert behavior)
            var existing = _profiles.FirstOrDefault(p => p.Name.Equals(profile.Name, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                _profiles.Remove(existing);
            }
            _profiles.Add(profile);
            SaveProfiles();
        }

        public static void RemoveProfile(string profileName)
        {
            var existing = _profiles.FirstOrDefault(p => p.Name.Equals(profileName, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                _profiles.Remove(existing);
                SaveProfiles();
            }
        }

        public static void ApplyProfileToSettings(DatabaseProfile profile)
        {
             Properties.OptionsDBsPage.Default.SQLServerType = profile.Type;
             Properties.OptionsDBsPage.Default.SQLHost = profile.Host;
             Properties.OptionsDBsPage.Default.SQLDatabaseName = profile.DatabaseName;
             Properties.OptionsDBsPage.Default.SQLUser = profile.Username;
             Properties.OptionsDBsPage.Default.SQLPass = profile.EncryptedPassword; // Already encrypted
             Properties.OptionsDBsPage.Default.SQLReadOnly = profile.ReadOnly;
             if (!string.IsNullOrEmpty(profile.AuthType))
                 Properties.OptionsDBsPage.Default.SQLAuthType = profile.AuthType;
        }
        
        public static DatabaseProfile CreateProfileFromCurrentSettings(string name)
        {
             return new DatabaseProfile
             {
                 Name = name,
                 Type = Properties.OptionsDBsPage.Default.SQLServerType,
                 Host = Properties.OptionsDBsPage.Default.SQLHost,
                 DatabaseName = Properties.OptionsDBsPage.Default.SQLDatabaseName,
                 Username = Properties.OptionsDBsPage.Default.SQLUser,
                 EncryptedPassword = Properties.OptionsDBsPage.Default.SQLPass,
                 ReadOnly = Properties.OptionsDBsPage.Default.SQLReadOnly,
                 AuthType = Properties.OptionsDBsPage.Default.SQLAuthType
             };
        }
    }
}
