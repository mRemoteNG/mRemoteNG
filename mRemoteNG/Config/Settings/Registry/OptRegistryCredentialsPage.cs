using System.Runtime.Versioning;
using Microsoft.Win32;
using mRemoteNG.App.Info;
using mRemoteNG.Tools.WindowsRegistry;

namespace mRemoteNG.Config.Settings.Registry
{
    [SupportedOSPlatform("windows")]
    /// Static utility class that provides access to and management of registry settings on the local machine.
    /// It abstracts complex registry operations and centralizes the handling of various registry keys.
    /// Benefits: Simplified code, enhances maintainability, and ensures consistency. #ReadOnly
    public sealed partial class OptRegistryCredentialsPage
    {
        #region option page credential registry settings

        /// <summary>
        /// Indicates whether modifying credential page settings is enabled.
        /// </summary>
        public bool CredentialPageEnabled { get; }

        /// <summary>
        /// Specifies the radio button is set to none, windows or custom on the credentials page.
        /// </summary>
        /// <remarks>
        /// When set to noinfo or windows, WindowsCredentials and CustomCredentials are not evaluated and disabled.
        /// </remarks>
        public WinRegistryEntry<string> UseCredentials { get; }

        /// <summary>
        /// Specifies the user set via API as the default username.
        /// </summary>
        /// <remarks>
        /// Only avaiable if UseCredentials is set to custom!
        /// </remarks>
        public WinRegistryEntry<string> UserViaAPIDefault { get; }

        /// <summary>
        /// Specifies the default username.
        /// </summary>
        /// <remarks>
        /// Only avaiable if UseCredentials is set to custom!
        /// </remarks>
        public WinRegistryEntry<string> DefaultUsername { get; }

        /// <summary>
        /// Specifies the default password.
        /// </summary>
        /// <remarks>
        /// Only avaiable if UseCredentials is set to custom!
        /// </remarks>
        public WinRegistryEntry<string> DefaultPassword { get; }

        /// <summary>
        /// Specifies the default domain.
        /// </summary>
        /// <remarks>
        /// Only avaiable if UseCredentials is set to custom!
        /// </remarks>
        public WinRegistryEntry<string> DefaultDomain { get; }

        #endregion

        public OptRegistryCredentialsPage()
        {
            IRegistry regValueUtility = new WinRegistry();
            RegistryHive hive = WindowsRegistryInfo.Hive;
            string subKey = WindowsRegistryInfo.CredentialOptions;

            CredentialPageEnabled = regValueUtility.GetBoolValue(hive, subKey, nameof(CredentialPageEnabled), true);
            UseCredentials = new WinRegistryEntry<string>(hive, subKey, nameof(UseCredentials)).SetValidation(
                new string[] {
                    "noinfo",
                    "windows",
                    "custom"
                }).Read();
            UserViaAPIDefault = new WinRegistryEntry<string>(hive, subKey, nameof(UserViaAPIDefault)).Read();
            DefaultUsername = new WinRegistryEntry<string>(hive, subKey, nameof(DefaultUsername)).Read();
            DefaultPassword = new WinRegistryEntry<string>(hive, subKey, nameof(DefaultPassword)).Read();
            DefaultDomain = new WinRegistryEntry<string>(hive, subKey, nameof(DefaultDomain)).Read();
        }
    }
}