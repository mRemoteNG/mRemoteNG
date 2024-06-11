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

        // Credential
        // Registry subkey for general application credentials settings
        // Registry subkey for credentials options page settings
        public const string Credential = RootKey + "\\Credentials";
        public const string CredentialOptions = Credential + "\\" + OptionsSubKey;

        // Updates
        // Registry subkey for general application update settings
        // Registry subkey for updates options page settings
        public const string Update = RootKey + "\\Updates";
        public const string UpdateOptions = Update + "\\" + OptionsSubKey;
        #endregion
    }
}