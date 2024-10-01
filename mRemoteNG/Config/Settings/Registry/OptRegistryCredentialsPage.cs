using System.Runtime.Versioning;
using Microsoft.Win32;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools.WindowsRegistry;

namespace mRemoteNG.Config.Settings.Registry
{
    [SupportedOSPlatform("windows")]
    public sealed partial class OptRegistryCredentialsPage
    {
        #region option page credential registry settings

        /// <summary>
        /// Specifies the radio button is set to none, windows or custom on the credentials page.
        /// </summary>
        /// <remarks>
        /// When set to noinfo or windows, WindowsCredentials and CustomCredentials are not evaluated and disabled.
        /// </remarks>
        public WinRegistryEntry<string> UseCredentials { get; private set; }

        /// <summary>
        /// Specifies the user set via API as the default username.
        /// </summary>
        /// <remarks>
        /// Only avaiable if UseCredentials is set to custom!
        /// </remarks>
        public WinRegistryEntry<string> UserViaAPIDefault { get; private set; }

        /// <summary>
        /// Specifies the default username.
        /// </summary>
        /// <remarks>
        /// Only avaiable if UseCredentials is set to custom!
        /// </remarks>
        public WinRegistryEntry<string> DefaultUsername { get; private set; }

        /// <summary>
        /// Specifies the default password.
        /// </summary>
        /// <remarks>
        /// Only avaiable if UseCredentials is set to custom!
        /// </remarks>
        public WinRegistryEntry<string> DefaultPassword { get; private set; }

        /// <summary>
        /// Specifies the default domain.
        /// </summary>
        /// <remarks>
        /// Only avaiable if UseCredentials is set to custom!
        /// </remarks>
        public WinRegistryEntry<string> DefaultDomain { get; private set; }

        /// <summary>
        /// Specifies that entering the custom default username field is enabled.
        /// </summary>
        public bool DefaultUsernameEnabled { get; private set; }

        /// <summary>
        /// Specifies that entering the custom default password field is enabled.
        /// </summary>
        public bool DefaultPasswordEnabled { get; private set; }

        /// <summary>
        /// Specifies that entering the custom default api user field is enabled.
        /// </summary>
        public bool DefaultUserViaAPIEnabled { get; private set; }

        #endregion

        public OptRegistryCredentialsPage()
        {
            IRegistryRead regValueUtility = new WinRegistry();
            RegistryHive hive = WindowsRegistryInfo.Hive;
            string subKey = WindowsRegistryInfo.CredentialOptions;

            UseCredentials = new WinRegistryEntry<string>(hive, subKey, nameof(UseCredentials)).Read();
            UserViaAPIDefault = new WinRegistryEntry<string>(hive, subKey, nameof(UserViaAPIDefault)).Read();
            DefaultUsername = new WinRegistryEntry<string>(hive, subKey, nameof(DefaultUsername)).Read();
            DefaultPassword = new WinRegistryEntry<string>(hive, subKey, nameof(DefaultPassword)).Read();
            DefaultDomain = new WinRegistryEntry<string>(hive, subKey, nameof(DefaultDomain)).Read();

            DefaultUsernameEnabled = regValueUtility.GetBoolValue(hive, subKey, nameof(DefaultUsernameEnabled), true);
            DefaultPasswordEnabled = regValueUtility.GetBoolValue(hive, subKey, nameof(DefaultPasswordEnabled), true);
            DefaultUserViaAPIEnabled = regValueUtility.GetBoolValue(hive, subKey, nameof(DefaultUserViaAPIEnabled), true);

            SetupValidation();
            Apply();
        }

        /// <summary>
        /// Configures validation settings for various parameters
        /// </summary>
        private void SetupValidation()
        { 
            UseCredentials.SetValidation(
                new string[] {
                    "noinfo",
                    "windows",
                    "custom"
                });
        }

        /// <summary>
        /// Applies registry settings and overrides various properties.
        /// </summary>
        private void Apply()
        {
            // UseCredentials musst be present in registry.
            if (! UseCredentials.IsSet)
                return;
            ApplyUseCredentials();

            // UseCredentials musst be set to custom.
            if (UseCredentials.Value != "custom")
                return;

            ApplyDefaultUsername();
            ApplyDefaultPassword();
            ApplyDefaultDomain();
            ApplyUserViaAPIDefault();
        }

        private void ApplyUseCredentials()
        {
            if (UseCredentials.IsValid)
                Properties.OptionsCredentialsPage.Default.EmptyCredentials = UseCredentials.Value;
        }

        private void ApplyDefaultUsername()
        {
            if (DefaultUsername.IsSet && DefaultUsernameEnabled)
                Properties.OptionsCredentialsPage.Default.DefaultUsername = DefaultUsername.Value;
            else if (!DefaultUsernameEnabled)
                Properties.OptionsCredentialsPage.Default.DefaultUsername = "";
        }

        private void ApplyDefaultPassword()
        {
            if (DefaultPassword.IsSet && DefaultPasswordEnabled)
            {
                try
                {
                    LegacyRijndaelCryptographyProvider cryptographyProvider = new();
                    string decryptedPassword;
                    string defaultPassword = DefaultPassword.Value;

                    decryptedPassword = cryptographyProvider.Decrypt(defaultPassword, Runtime.EncryptionKey);
                    Properties.OptionsCredentialsPage.Default.DefaultPassword = defaultPassword;
                }
                catch
                {
                    // Fire-and-forget: The DefaultPassword in the registry is not encrypted.
                    DefaultPassword.Clear();
                }
            }
            else if (!DefaultPasswordEnabled)
            {
                Properties.OptionsCredentialsPage.Default.DefaultPassword = "";
            }
        }

        private void ApplyDefaultDomain()
        {
            if (DefaultDomain.IsSet)
                Properties.OptionsCredentialsPage.Default.DefaultDomain = DefaultDomain.Value;
         }

        private void ApplyUserViaAPIDefault()
        {
            if (UserViaAPIDefault.IsSet && DefaultUserViaAPIEnabled)
                Properties.OptionsCredentialsPage.Default.UserViaAPIDefault = UserViaAPIDefault.Value;
            else if (!DefaultUserViaAPIEnabled)
                Properties.OptionsCredentialsPage.Default.UserViaAPIDefault = "";
        }
    }
}