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
    public sealed partial class OptRegistryUpdatesPage
    {
        /// <summary>
        /// Specifies the number of days between update checks.
        /// </summary>
        public WinRegistryEntry<int> CheckForUpdatesFrequencyDays { get; }

        /// <summary>
        /// Specifies the update channel for updates.
        /// </summary>
        /// <remarks>
        /// The update channel should be one of the predefined values: Stable, Preview, Nightly.
        /// </remarks>
        public WinRegistryEntry<string> UpdateChannel { get; }

        /// <summary>
        /// Indicates whether proxy usage for updates is enabled.
        /// </summary>
        public WinRegistryEntry<bool> UseProxyForUpdates { get; }

        /// <summary>
        /// Specifies the proxy address for updates.
        /// </summary>
        public WinRegistryEntry<string> ProxyAddress { get; }

        /// <summary>
        /// Specifies the proxy port for updates.
        /// </summary>
        public WinRegistryEntry<int> ProxyPort { get; }

        /// <summary>
        /// Indicates whether proxy authentication is enabled.
        /// </summary>
        public WinRegistryEntry<bool> UseProxyAuthentication { get; }

        /// <summary>
        /// Specifies the authentication username for the proxy.
        /// </summary>
        public WinRegistryEntry<string> ProxyAuthUser { get; }

        /// <summary>
        /// Specifies the authentication password for the proxy.
        /// </summary>
        public WinRegistryEntry<string> ProxyAuthPass { get; }

        public OptRegistryUpdatesPage()
        {
            RegistryHive hive = WindowsRegistryInfo.Hive;
            string subKey = WindowsRegistryInfo.UpdateOptions;

            CheckForUpdatesFrequencyDays = new WinRegistryEntry<int>(hive, subKey, nameof(CheckForUpdatesFrequencyDays)).Read();
            UpdateChannel = new WinRegistryEntry<string>(hive, subKey, nameof(UpdateChannel))
                .SetValidation(new string[] {
                    UpdateChannelInfo.STABLE,
                    UpdateChannelInfo.PREVIEW,
                    UpdateChannelInfo.NIGHTLY
                }).Read();

            UseProxyForUpdates = new WinRegistryEntry<bool>(hive, subKey, nameof(UseProxyForUpdates)).Read();
            ProxyAddress = new WinRegistryEntry<string>(hive, subKey, nameof(ProxyAddress)).Read();
            ProxyPort = new WinRegistryEntry<int>(hive, subKey, nameof(ProxyPort)).Read();

            UseProxyAuthentication = new WinRegistryEntry<bool>(hive, subKey, nameof(UseProxyAuthentication)).Read();
            ProxyAuthUser = new WinRegistryEntry<string>(hive, subKey, nameof(ProxyAuthUser)).Read();
            ProxyAuthPass = new WinRegistryEntry<string>(hive, subKey, nameof(ProxyAuthPass)).Read();
        }
    }
}