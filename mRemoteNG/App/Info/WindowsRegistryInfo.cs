using Microsoft.Win32;
using System.Runtime.Versioning;

namespace mRemoteNG.App.Info
{
    [SupportedOSPlatform("windows")]
    public static class WindowsRegistryInfo
    {
        #region General Parameters

        public const RegistryHive Hive = RegistryHive.LocalMachine;
        public const string RootKey = "SOFTWARE\\mRemoteNG";
        private const string OptionsSubKey = "Options";

        #endregion

        #region Key Locations

        // StartupExit
        // Registry subkey for general application startup and exit settings
        // Registry subkey for startup and exit options page settings
        public const string StartupExit = RootKey + "\\StartupExit";
        public const string StartupExitOptions = StartupExit + "\\" + OptionsSubKey;

        // Appearance
        // Registry subkey for general application appearance settings
        // Registry subkey for appearance options page settings
        public const string Appearance = RootKey + "\\Appearance";
        public const string AppearanceOptions = Appearance + "\\" + OptionsSubKey;

        // Connections
        // Registry subkey for general application connection settings
        // Registry subkey for connections options page settings
        public const string Connection = RootKey + "\\Connections";
        public const string ConnectionOptions = Connection + "\\" + OptionsSubKey;

        // Tabs & Panels
        // Registry subkey for general application tabs and panels settings
        // Registry subkey for tabs and panels options page settings
        public const string TabsAndPanels = RootKey + "\\TabsAndPanels";
        public const string TabsAndPanelsOptions = TabsAndPanels + "\\" + OptionsSubKey;

        // Notifications
        // Registry subkey for general application notifications settings
        // Registry subkey for notifications options page settings
        public const string Notification = RootKey + "\\Notifications";
        public const string NotificationOptions = Notification + "\\" + OptionsSubKey;

        // Credential
        // Registry subkey for general application credentials settings
        // Registry subkey for credentials options page settings
        public const string Credential = RootKey + "\\Credentials";
        public const string CredentialOptions = Credential + "\\" + OptionsSubKey;

        // SQL Server
        // Registry subkey for general application SQL server settings
        // Registry subkey for SQL server options page settings
        public const string SQLServer = RootKey + "\\SQLServer";
        public const string SQLServerOptions = SQLServer + "\\" + OptionsSubKey;

        // Updates
        // Registry subkey for general application update settings
        // Registry subkey for updates options page settings
        public const string Update = RootKey + "\\Updates";
        public const string UpdateOptions = Update + "\\" + OptionsSubKey;

        // Security
        // Registry subkey for general application security settings
        // Registry subkey for security options page settings
        public const string Security = RootKey + "\\Security";
        public const string SecurityOptions = Security + "\\" + OptionsSubKey;

        // Advanced
        // Registry subkey for general application advanced settings
        // Registry subkey for advanced options page settings
        public const string Advanced = RootKey + "\\Advanced";
        public const string AdvancedOptions = Advanced + "\\" + OptionsSubKey;

        // Backup
        // Registry subkey for general application backup settings
        // Registry subkey for backup options page settings
        public const string Backup = RootKey + "\\Backup";
        public const string BackupOptions = Backup + "\\" + OptionsSubKey;

        #endregion
    }
}