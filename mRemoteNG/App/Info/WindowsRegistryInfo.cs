using Microsoft.Win32;
using System.Runtime.Versioning;

namespace mRemoteNG.App.Info
{
    [SupportedOSPlatform("windows")]
    public static class WindowsRegistryInfo
    {
        #region general parameters
        public const RegistryHive Hive = RegistryHive.LocalMachine;
        public const string RootKey = "SOFTWARE\\mRemoteNG";
        #endregion

        #region subkey location parameters
        // Credential
        public const string CredentialSubkey = RootKey + "\\Credentials"; // Registry subkey for general credential settings
        public const string CredentialOptionsSubkey = RootKey + "\\Credentials\\Options"; // Registry subkey for credential options within the credential settings

        // Updates
        public const string UpdateSubkey        = RootKey + "\\Updates"; // Registry subkey for general update settings
        public const string UpdateOptionsSubkey = RootKey + "\\Updates\\Options"; // Registry subkey for update options within the update settings
        #endregion
    }
}