using System.Runtime.Versioning;
using Microsoft.Win32;
using mRemoteNG.App.Info;
using mRemoteNG.Tools.WindowsRegistry;

namespace mRemoteNG.Config.Settings.Registry
{
    [SupportedOSPlatform("windows")]
    public sealed partial class OptRegistryAppearancePage
    {
        /// <summary>
        /// Specifies whether to show tooltips with descriptions in the connection tree view.
        /// </summary>
        public WinRegistryEntry<bool> ShowDescriptionTooltipsInConTree { get; private set; }

        /// <summary>
        /// Specifies whether to show complete connection path in the window title.
        /// </summary>
        public WinRegistryEntry<bool> ShowCompleteConFilePathInTitle { get; private set; }

        /// <summary>
        /// Specifies whether to always show the system tray icon.
        /// </summary>
        public WinRegistryEntry<bool> AlwaysShowSystemTrayIcon { get; private set; }

        /// <summary>
        /// Specifies whether the application should minimize to the system tray.
        /// </summary>
        public WinRegistryEntry<bool> MinimizeToTray { get; private set; }

        /// <summary>
        /// Specifies whether the application should close to the system tray.
        /// </summary>
        public WinRegistryEntry<bool> CloseToTray { get; private set; }

        public OptRegistryAppearancePage()
        {
            RegistryHive hive = WindowsRegistryInfo.Hive;
            string subKey = WindowsRegistryInfo.AppearanceOptions;

            ShowDescriptionTooltipsInConTree = new WinRegistryEntry<bool>(hive, subKey, nameof(ShowDescriptionTooltipsInConTree)).Read();
            ShowCompleteConFilePathInTitle = new WinRegistryEntry<bool>(hive, subKey, nameof(ShowCompleteConFilePathInTitle)).Read();
            AlwaysShowSystemTrayIcon = new WinRegistryEntry<bool>(hive, subKey, nameof(AlwaysShowSystemTrayIcon)).Read();
            MinimizeToTray = new WinRegistryEntry<bool>(hive, subKey, nameof(MinimizeToTray)).Read();
            CloseToTray = new WinRegistryEntry<bool>(hive, subKey, nameof(CloseToTray)).Read();

            SetupValidation();
            Apply();
        }

        /// <summary>
        /// Configures validation settings for various parameters
        /// </summary>
        private void SetupValidation()
        {

        }

        /// <summary>
        /// Applies registry settings and overrides various properties.
        /// </summary>
        private void Apply()
        {
            ApplyShowDescriptionTooltipsInConTree();
            ApplyShowCompleteConFilePathInTitle();
            ApplyAlwaysShowSystemTrayIcon();
            ApplyMinimizeToTray();
            ApplyCloseToTray();
        }

        private void ApplyShowDescriptionTooltipsInConTree()
        {
            if (ShowDescriptionTooltipsInConTree.IsSet)
                Properties.OptionsAppearancePage.Default.ShowDescriptionTooltipsInTree = ShowDescriptionTooltipsInConTree.Value;
        }

        private void ApplyShowCompleteConFilePathInTitle()
        {
            if (ShowCompleteConFilePathInTitle.IsSet)
                Properties.OptionsAppearancePage.Default.ShowCompleteConsPathInTitle = ShowCompleteConFilePathInTitle.Value;
        }

        private void ApplyAlwaysShowSystemTrayIcon()
        {
            if (AlwaysShowSystemTrayIcon.IsSet)
                Properties.OptionsAppearancePage.Default.ShowSystemTrayIcon = AlwaysShowSystemTrayIcon.Value;
        }

        private void ApplyMinimizeToTray()
        {
            if (MinimizeToTray.IsSet)
                Properties.OptionsAppearancePage.Default.MinimizeToTray = MinimizeToTray.Value;
        }

        private void ApplyCloseToTray()
        {
            if (CloseToTray.IsSet)
                Properties.OptionsAppearancePage.Default.CloseToTray = CloseToTray.Value;
        }
    }
}