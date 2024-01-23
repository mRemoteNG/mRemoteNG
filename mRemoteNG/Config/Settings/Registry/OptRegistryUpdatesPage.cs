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
    public sealed partial class OptRegistryUpdatesPage : WindowsRegistryAdvanced
    {
        /// <summary>
        /// Specifies the number of days between update checks.
        /// </summary>
        public WindowsRegistryKeyInteger CheckForUpdatesFrequencyDays { get; }

        /// <summary>
        /// Specifies the update channel for updates.
        /// </summary>
        /// <remarks>
        /// The update channel should be one of the predefined values: Stable, Preview, Nightly.
        /// </remarks>
        public WindowsRegistryKeyString UpdateChannel { get; }

        /// <summary>
        /// Indicates whether proxy usage for updates is enabled.
        /// </summary>
        public WindowsRegistryKeyBoolean UseProxyForUpdates { get; }

        /// <summary>
        /// Specifies the proxy address for updates.
        /// </summary>
        public WindowsRegistryKeyString ProxyAddress { get; }

        /// <summary>
        /// Specifies the proxy port for updates.
        /// </summary>
        public WindowsRegistryKeyInteger ProxyPort { get; }

        /// <summary>
        /// Indicates whether proxy authentication is enabled.
        /// </summary>
        public WindowsRegistryKeyBoolean UseProxyAuthentication { get; }

        /// <summary>
        /// Specifies the authentication username for the proxy.
        /// </summary>
        public WindowsRegistryKeyString ProxyAuthUser { get; }

        /// <summary>
        /// Specifies the authentication password for the proxy.
        /// </summary>
        /// <remarks>
        /// Please only store encrypted passwords in the registry.
        /// </remarks>
        //public string ProxyAuthPass { get; }

        public OptRegistryUpdatesPage()
        {
            RegistryHive hive = WindowsRegistryInfo.Hive;
            string subKey = WindowsRegistryInfo.UpdateOptionsSubkey;

            CheckForUpdatesFrequencyDays = GetInteger(hive, subKey, nameof(CheckForUpdatesFrequencyDays));

            UpdateChannel = GetStringValidated(hive, subKey, nameof(UpdateChannel),
                new string[] {
                    UpdateChannelInfo.STABLE,
                    UpdateChannelInfo.PREVIEW,
                    UpdateChannelInfo.NIGHTLY
                }
                ,true
            );

            UseProxyForUpdates = GetBoolean(hive, subKey, nameof(UseProxyForUpdates));
            ProxyAddress = GetString(hive, subKey, nameof(ProxyAddress), null);
            ProxyPort = GetInteger(hive, subKey, nameof(ProxyPort));

            UseProxyAuthentication = GetBoolean(hive, subKey, nameof(UseProxyAuthentication));
            ProxyAuthUser = GetString(hive, subKey, nameof(ProxyAuthUser), null);
            //Currently not supported:
            //ProxyAuthPass = GetPassword(Hive, _SubKey, nameof(ProxyAuthPass));
        }
    }
}