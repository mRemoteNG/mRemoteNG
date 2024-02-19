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
    public sealed partial class OptRegistryCredentialsPage : WindowsRegistryAdvanced
    {
        #region option page credential registry settings
        /// <summary>
        /// Specifies the radio button is set to none, windows or custom on the credentials page.
        /// </summary>
        /// <remarks>
        /// When set to noinfo or windows, WindowsCredentials and CustomCredentials are not evaluated and disabled.
        /// </remarks>
        public WindowsRegistryKeyString UseCredentials { get; }

        /// <summary>
        /// Specifies the user set via API as the default username.
        /// </summary>
        /// <remarks>
        /// Only avaiable if UseCredentials is set to custom!
        /// </remarks>
        public WindowsRegistryKeyString UserViaAPIDefault { get; }

        /// <summary>
        /// Specifies the default username.
        /// </summary>
        /// <remarks>
        /// Only avaiable if UseCredentials is set to custom!
        /// </remarks>
        public WindowsRegistryKeyString DefaultUsername { get; }

        /// <summary>
        /// Specifies the default password.
        /// </summary>
        /// <remarks>
        /// Only avaiable if UseCredentials is set to custom!
        /// </remarks>
        //public WindowsRegistryKeyString DefaultPassword { get; }

        /// <summary>
        /// Specifies the default domain.
        /// </summary>
        /// <remarks>
        /// Only avaiable if UseCredentials is set to custom!
        /// </remarks>
        public WindowsRegistryKeyString DefaultDomain { get; }
        #endregion

        public OptRegistryCredentialsPage()
        {
            RegistryHive hive = WindowsRegistryInfo.Hive;
            string subKey = WindowsRegistryInfo.CredentialOptionsSubkey;

            UseCredentials = GetStringValidated(hive, subKey, nameof(UseCredentials),
                new string[] {
                    "noinfo",
                    "windows",
                    "custom"
                }, true
             );

            UserViaAPIDefault = GetString(hive, subKey, nameof(UserViaAPIDefault), null);
            DefaultUsername = GetString(hive, subKey, nameof(DefaultUsername), null);
            //Currently not supported, but needed for configuration...
            //DefaultPassword = GetPassword(hive, subKey, nameof(DefaultPassword));

            DefaultDomain = GetString(hive, subKey, nameof(DefaultDomain), null);
        }
    }
}