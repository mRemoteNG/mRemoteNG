using System.Runtime.Versioning;
using Microsoft.Win32;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools.WindowsRegistry;

namespace mRemoteNG.Config.Settings.Registry
{
    [SupportedOSPlatform("windows")]
    public sealed partial class OptRegistryUpdatesPage
    {
        /// <summary>
        /// Specifies whether a popup is shown to configure update preferences at startup.
        /// </summary>
        public WinRegistryEntry<bool> DisallowPromptForUpdatesPreference { get; private set; }

        /// <summary>
        /// Specifies the number of days between update checks.
        /// </summary>
        public WinRegistryEntry<int> CheckForUpdatesFrequencyDays { get; private set; }

        /// <summary>
        /// Specifies the update channel for updates.
        /// </summary>
        /// <remarks>
        /// The update channel should be one of the predefined values: Stable, Preview, Nightly.
        /// </remarks>
        public WinRegistryEntry<string> UpdateChannel { get; private set; }

        /// <summary>
        /// Indicates whether proxy usage for updates is enabled.
        /// </summary>
        public WinRegistryEntry<bool> UseProxyForUpdates { get; private set; }

        /// <summary>
        /// Specifies the proxy address for updates.
        /// </summary>
        public WinRegistryEntry<string> ProxyAddress { get; private set; }

        /// <summary>
        /// Specifies the proxy port for updates.
        /// </summary>
        public WinRegistryEntry<int> ProxyPort { get; private set; }

        /// <summary>
        /// Indicates whether proxy authentication is enabled.
        /// </summary>
        public WinRegistryEntry<bool> UseProxyAuthentication { get; private set; }

        /// <summary>
        /// Specifies the authentication username for the proxy.
        /// </summary>
        public WinRegistryEntry<string> ProxyAuthUser { get; private set; }

        /// <summary>
        /// Specifies the authentication password for the proxy.
        /// </summary>
        public WinRegistryEntry<string> ProxyAuthPass { get; private set; }

        public OptRegistryUpdatesPage()
        {
            RegistryHive hive = WindowsRegistryInfo.Hive;
            string subKey = WindowsRegistryInfo.UpdateOptions;

            DisallowPromptForUpdatesPreference = new WinRegistryEntry<bool>(hive, subKey, nameof(DisallowPromptForUpdatesPreference)).Read();
            CheckForUpdatesFrequencyDays = new WinRegistryEntry<int>(hive, subKey, nameof(CheckForUpdatesFrequencyDays)).Read();
            UpdateChannel = new WinRegistryEntry<string>(hive, subKey, nameof(UpdateChannel)).Read();
            UseProxyForUpdates = new WinRegistryEntry<bool>(hive, subKey, nameof(UseProxyForUpdates)).Read();
            ProxyAddress = new WinRegistryEntry<string>(hive, subKey, nameof(ProxyAddress)).Read();
            ProxyPort = new WinRegistryEntry<int>(hive, subKey, nameof(ProxyPort)).Read();
            UseProxyAuthentication = new WinRegistryEntry<bool>(hive, subKey, nameof(UseProxyAuthentication)).Read();
            ProxyAuthUser = new WinRegistryEntry<string>(hive, subKey, nameof(ProxyAuthUser)).Read();
            ProxyAuthPass = new WinRegistryEntry<string>(hive, subKey, nameof(ProxyAuthPass)).Read();

            SetupValidation();
            Apply();
        }

        /// <summary>
        /// Configures validation settings for various parameters
        /// </summary>
        private void SetupValidation()
        {
            var connectionsPage = new UI.Forms.OptionsPages.UpdatesPage();

            UpdateChannel.SetValidation(new string[] {
                UpdateChannelInfo.STABLE,
                UpdateChannelInfo.PREVIEW,
                UpdateChannelInfo.NIGHTLY
            });


            int proxyPortMin = (int)connectionsPage.numProxyPort.Minimum;
            int proxyPortMax = (int)connectionsPage.numProxyPort.Maximum;
            ProxyPort.SetValidation(proxyPortMin, proxyPortMax);
        }

        /// <summary>
        /// Applies registry settings and overrides various properties.
        /// </summary>
        private void Apply()
        {
            // Common settings were applied, all update settings are disabled.
            if (ApplyCommonUpdateCheckSettings())
                return;

            ApplyAllowPromptForUpdatesPreference();
            ApplyCheckForUpdatesOnStartup();
            ApplyCheckForUpdatesFrequencyDays();
            ApplyUpdateChannel();
            ApplyProxyForUpdates();
            ApplyAuthentication();
        }

        private static bool ApplyCommonUpdateCheckSettings()
        {
            bool updatesNotAllowed =
                !CommonRegistrySettings.AllowCheckForUpdates
                || (!CommonRegistrySettings.AllowCheckForUpdatesAutomatical
                && !CommonRegistrySettings.AllowCheckForUpdatesManual);

            if (updatesNotAllowed)
            {
                Properties.OptionsUpdatesPage.Default.CheckForUpdatesAsked = true;

                Properties.OptionsUpdatesPage.Default.CheckForUpdatesOnStartup = false;
                Properties.OptionsUpdatesPage.Default.CheckForUpdatesFrequencyDays = 14;


                Properties.OptionsUpdatesPage.Default.UpdateUseProxy = false;
                Properties.OptionsUpdatesPage.Default.UpdateProxyAddress = "";
                Properties.OptionsUpdatesPage.Default.UpdateProxyPort = 80;

                Properties.OptionsUpdatesPage.Default.UpdateProxyUseAuthentication = false;
                Properties.OptionsUpdatesPage.Default.UpdateProxyAuthUser = "";
                Properties.OptionsUpdatesPage.Default.UpdateProxyAuthPass = "";
            }

            return updatesNotAllowed;
        }

        private void ApplyAllowPromptForUpdatesPreference()
        {
            if (DisallowPromptForUpdatesPreference.IsSet && DisallowPromptForUpdatesPreference.Value == true)
                 Properties.OptionsUpdatesPage.Default.CheckForUpdatesAsked = true;              
        }

        private void ApplyCheckForUpdatesOnStartup()
        {
            if (!CommonRegistrySettings.AllowCheckForUpdatesAutomatical)
                Properties.OptionsUpdatesPage.Default.CheckForUpdatesOnStartup = false;
        }

        private void ApplyCheckForUpdatesFrequencyDays()
        {
            if (CommonRegistrySettings.AllowCheckForUpdatesAutomatical && CheckForUpdatesFrequencyDays.IsSet)
            {
                if (CheckForUpdatesFrequencyDays.Value < 0)
                {
                    Properties.OptionsUpdatesPage.Default.CheckForUpdatesOnStartup = false;
                }
                else if (CheckForUpdatesFrequencyDays.IsValid)
                {
                    Properties.OptionsUpdatesPage.Default.CheckForUpdatesOnStartup = true;
                    Properties.OptionsUpdatesPage.Default.CheckForUpdatesFrequencyDays = CheckForUpdatesFrequencyDays.Value;
                }
            }
        }

        private void ApplyUpdateChannel()
        {
            if (UpdateChannel.IsValid)
                Properties.OptionsUpdatesPage.Default.UpdateChannel = UpdateChannel.Value;
        }

        private void ApplyProxyForUpdates()
        {
            if (UseProxyForUpdates.IsSet)
            {
                Properties.OptionsUpdatesPage.Default.UpdateUseProxy = UseProxyForUpdates.Value;

                if (!UseProxyForUpdates.Value)
                {
                    Properties.OptionsUpdatesPage.Default.UpdateProxyAddress = "";
                    Properties.OptionsUpdatesPage.Default.UpdateProxyPort = 80;
                    Properties.OptionsUpdatesPage.Default.UpdateProxyUseAuthentication = false;
                }

                if (ProxyAddress.IsSet && UseProxyForUpdates.Value)
                {
                    Properties.OptionsUpdatesPage.Default.UpdateProxyAddress = ProxyAddress.Value;
                }

                if (ProxyPort.IsValid && UseProxyForUpdates.Value)
                {
                    Properties.OptionsUpdatesPage.Default.UpdateProxyPort = ProxyPort.Value;
                }
            }
        }

        private void ApplyAuthentication()
        {
            if (UseProxyForUpdates.Value && UseProxyAuthentication.IsSet)
            {
                Properties.OptionsUpdatesPage.Default.UpdateProxyUseAuthentication = UseProxyAuthentication.Value;

                if (!UseProxyAuthentication.Value)
                {
                    Properties.OptionsUpdatesPage.Default.UpdateProxyAuthUser = "";
                    Properties.OptionsUpdatesPage.Default.UpdateProxyAuthPass = "";
                }

                if (ProxyAuthUser.IsSet && UseProxyAuthentication.Value)
                {
                    Properties.OptionsUpdatesPage.Default.UpdateProxyAuthUser = ProxyAuthUser.Value;
                }

                if (ProxyAuthPass.IsSet && UseProxyAuthentication.Value)
                {
                    try
                    {
                        LegacyRijndaelCryptographyProvider cryptographyProvider = new();
                        string decryptedPassword;
                        string proxyAuthPass = ProxyAuthPass.Value;
                        decryptedPassword = cryptographyProvider.Decrypt(proxyAuthPass, Runtime.EncryptionKey);

                        Properties.OptionsUpdatesPage.Default.UpdateProxyAuthPass = decryptedPassword;
                    }
                    catch
                    {
                        // The password in the registry is not encrypted.
                    }
                }
            }
        }
    }
}