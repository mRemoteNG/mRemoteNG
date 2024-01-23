using Microsoft.Win32;
using System.Runtime.Versioning;
using mRemoteNG.App.Info;
using mRemoteNG.Tools.WindowsRegistry;

namespace mRemoteNG.Config.Settings.Registry
{
    [SupportedOSPlatform("windows")]
    /// Static utility class that provides access to and management of registry settings on the local machine.
    /// It abstracts complex registry operations and centralizes the handling of various registry keys.
    /// Benefits: Simplified code, enhances maintainability, and ensures consistency. #ReadOnly
    public static class CommonRegistrySettings
    {
        private static readonly IRegistryAdvancedRead _WindowsRegistry = new WindowsRegistryAdvanced();
        private static readonly RegistryHive _Hive = WindowsRegistryInfo.Hive;

        private const string __Update = WindowsRegistryInfo.UpdateSubkey;
        private const string __Credential = WindowsRegistryInfo.CredentialSubkey;

        #region general update registry settings
        /// <summary>
        /// Indicates whether searching for updates is allowed. If false, there is no way to update directly from mRemoteNG.
        /// </summary>
        /// <remarks>
        /// Default value is true, which allows check for updates.
        /// If the registry key is set to true, no action will be taken because the key is not needed.
        /// </remarks>
        public static bool AllowCheckForUpdates { get; } = _WindowsRegistry.GetBoolValue(_Hive, __Update, nameof(AllowCheckForUpdates), true);

        /// <summary>
        /// Indicates whether automatic search for updates is allowed.
        /// </summary>
        /// <remarks>
        /// Default value is true, which allows check for updates automaticaly.
        /// If the registry key is set to true, no action will be taken because the key is not needed.
        /// Important: If AllowCheckForUpdates is false, the default for this value (AllowCheckForUpdatesAutomatical) is also false, manual update checks are disabled.
        /// </remarks>
        public static bool AllowCheckForUpdatesAutomatical { get; } = _WindowsRegistry.GetBoolValue(_Hive, __Update, nameof(AllowCheckForUpdatesAutomatical), AllowCheckForUpdates);

        /// <summary>
        /// Indicates whether a manual search for updates is allowed.
        /// </summary>
        /// <remarks>
        /// The default value is true, enabling the manual check for updates.
        /// If the registry key is set to true, no action will be taken because the key is not needed.
        /// Important: If AllowCheckForUpdates is false, the default for this value (AllowCheckForUpdatesManual) is also false, manual update checks are disabled.
        /// </remarks>
        public static bool AllowCheckForUpdatesManual { get; } = _WindowsRegistry.GetBoolValue(_Hive, __Update, nameof(AllowCheckForUpdatesManual), AllowCheckForUpdates);

        /// <summary>
        /// Specifies whether a question about checking for updates is displayed at startup.
        /// </summary>
        /// <remarks>
        /// Important: If the registry entry is set to true, a popup will appear every time you start
        /// </remarks>
        public static bool AllowPromptForUpdatesPreference { get; } = _WindowsRegistry.GetBoolValue(_Hive, __Update, nameof(AllowPromptForUpdatesPreference), AllowCheckForUpdates);
        #endregion


        
    }
}