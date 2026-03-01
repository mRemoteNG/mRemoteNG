using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.Connection;
using mRemoteNG.Properties;

namespace mRemoteNG.App.Info
{
    [SupportedOSPlatform("windows")]
    public static class SettingsFileInfo
    {
        private static readonly string ExePath = Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConnectionInfo))?.Location) ?? string.Empty;
        private static readonly Lazy<string> InstalledSettingsPath = new(GetInstalledSettingsPath);

        internal const string PortableSettingsFolderName = "Settings";

        // For portable edition running from a read-only or WebDAV drive, fall back to %APPDATA%.
        // Config files are stored in a dedicated "Settings" subfolder to keep the exe directory clean.
        private static readonly Lazy<string> _portableWritablePath = new(() =>
        {
            if (!string.IsNullOrEmpty(ExePath) && IsDirectoryWritable(ExePath))
            {
                string settingsDir = Path.Combine(ExePath, PortableSettingsFolderName);
                if (!Directory.Exists(settingsDir))
                    Directory.CreateDirectory(settingsDir);
                MigratePortableSettings(ExePath, settingsDir);
                return settingsDir;
            }
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Application.ProductName ?? "mRemoteNG");
        });

        private static bool IsDirectoryWritable(string dirPath)
        {
            try
            {
                string testFile = Path.Combine(dirPath, Path.GetRandomFileName());
                using var fs = File.Create(testFile, 1, FileOptions.DeleteOnClose);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string DefaultSettingsPath =>
            Runtime.IsPortableEdition
                ? _portableWritablePath.Value
                : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Application.ProductName ?? string.Empty);

        public static string SettingsPath =>
            Runtime.IsPortableEdition
                ? _portableWritablePath.Value
                : InstalledSettingsPath.Value;

        public static string UserSettingsFilePath =>
            Runtime.IsPortableEdition
                ? Path.Combine(_portableWritablePath.Value, $"{Path.GetFileNameWithoutExtension(Application.ExecutablePath)}.settings")
                : GetInstalledUserSettingsFilePath();

        public static string UserSettingsFolderPath =>
            string.IsNullOrWhiteSpace(UserSettingsFilePath)
                ? string.Empty
                : Path.GetDirectoryName(UserSettingsFilePath) ?? string.Empty;

        public static string LayoutFileName { get; } = "pnlLayout.xml";
        public static string ExtAppsFilesName { get; } = "extApps.xml";

        public static string ExtAppsFilePath
        {
            get
            {
                string customPath = Settings.Default.CustomExtAppsFilePath?.Trim() ?? string.Empty;
                if (!string.IsNullOrWhiteSpace(customPath))
                {
                    try
                    {
                        string expandedPath = Environment.ExpandEnvironmentVariables(customPath);
                        return Path.GetFullPath(expandedPath);
                    }
                    catch (Exception ex) when (ex is ArgumentException or NotSupportedException or PathTooLongException)
                    {
                    }
                }
                return Path.Combine(SettingsPath, ExtAppsFilesName);
            }
        }
        public static string CmdSnippetsFileName { get; } = "cmdSnippets.xml";
        public static string ThemesFileName { get; } = "Themes.xml";
        public static string LocalConnectionProperties { get; } = "LocalConnectionProperties.xml";
        public static string SqlConnectionsCache { get; } = "SqlConnectionsCache.xml";
        public static string QuickConnectHistoryFileName { get; } = "quickConnectHistory.xml";

        public static string ThemeFolder =>
            string.IsNullOrWhiteSpace(SettingsPath)
                ? string.Empty
                : Path.Combine(SettingsPath, "Themes");

        public static string InstalledThemeFolder =>
            string.IsNullOrWhiteSpace(ExePath)
                ? string.Empty
                : Path.Combine(ExePath, "Themes");

        private static string GetInstalledSettingsPath()
        {
            string configuredPath = GetConfiguredSettingsPath();
            return string.IsNullOrWhiteSpace(configuredPath) ? DefaultSettingsPath : configuredPath;
        }

        private static string GetConfiguredSettingsPath()
        {
            try
            {
                string configuredPath = Settings.Default.CustomConfigurationPath;
                if (string.IsNullOrWhiteSpace(configuredPath))
                    return string.Empty;

                string expandedPath = Environment.ExpandEnvironmentVariables(configuredPath.Trim());
                return Path.GetFullPath(expandedPath);
            }
            catch (ConfigurationErrorsException)
            {
                return string.Empty;
            }
            catch (Exception ex) when (ex is ArgumentException or NotSupportedException or PathTooLongException)
            {
                return string.Empty;
            }
        }

        private static string GetInstalledUserSettingsFilePath()
        {
            try
            {
                return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
            }
            catch (ConfigurationErrorsException)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Migrates portable config files from the exe root to the Settings subfolder.
        /// Only moves files that exist in the old location but not in the new location.
        /// </summary>
        private static void MigratePortableSettings(string oldDir, string newDir)
        {
            string[] configFiles =
            [
                "confCons.xml",
                "confConsNew.xml",
                "extApps.xml",
                "cmdSnippets.xml",
                "pnlLayout.xml",
                "Themes.xml",
                "LocalConnectionProperties.xml",
                "quickConnectHistory.xml",
                "SqlConnectionsCache.xml",
            ];

            // Also migrate the .settings file (e.g. mRemoteNG.settings)
            string settingsFileName = $"{Path.GetFileNameWithoutExtension(Application.ExecutablePath)}.settings";

            try
            {
                foreach (string fileName in configFiles)
                    MigrateFile(oldDir, newDir, fileName);

                MigrateFile(oldDir, newDir, settingsFileName);

                // Migrate confCons backup files (confCons.xml.*.backup)
                foreach (string backupFile in Directory.GetFiles(oldDir, "confCons.xml.*.backup"))
                {
                    string name = Path.GetFileName(backupFile);
                    string dest = Path.Combine(newDir, name);
                    if (!File.Exists(dest))
                        File.Move(backupFile, dest);
                }

                // Migrate Themes subfolder
                string oldThemes = Path.Combine(oldDir, "Themes");
                string newThemes = Path.Combine(newDir, "Themes");
                if (Directory.Exists(oldThemes) && !Directory.Exists(newThemes))
                    Directory.Move(oldThemes, newThemes);
            }
            catch
            {
                // Migration is best-effort; don't crash on failure
            }
        }

        private static void MigrateFile(string oldDir, string newDir, string fileName)
        {
            string oldPath = Path.Combine(oldDir, fileName);
            string newPath = Path.Combine(newDir, fileName);
            if (File.Exists(oldPath) && !File.Exists(newPath))
                File.Move(oldPath, newPath);
        }
    }
}
