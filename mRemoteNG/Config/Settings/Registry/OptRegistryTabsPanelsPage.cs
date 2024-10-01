using System.Runtime.Versioning;
using Microsoft.Win32;
using mRemoteNG.App.Info;
using mRemoteNG.Tools.WindowsRegistry;

namespace mRemoteNG.Config.Settings.Registry
{
    [SupportedOSPlatform("windows")]
    public sealed partial class OptRegistryTabsPanelsPage
    {
        /// <summary>
        /// Specifies whether panel tabs are always shown.
        /// </summary>
        public WinRegistryEntry<bool> AlwaysShowPanelTabs { get; private set; }

        /// <summary>
        /// Specifies whether logon information is shown on tabs.
        /// </summary>
        public WinRegistryEntry<bool> ShowLogonInfoOnTabs { get; private set; }

        /// <summary>
        /// Specifies whether protocol information is shown on tabs.
        /// </summary>
        public WinRegistryEntry<bool> ShowProtocolOnTabs { get; private set; }

        /// <summary>
        /// Specifies whether quick connect tabs are identified by the prefix "Quick:".
        /// </summary>
        public WinRegistryEntry<bool> IdentifyQuickConnectTabs { get; private set; }

        /// <summary>
        /// Specifies whether double-clicking on a tab closes it.
        /// </summary>
        public WinRegistryEntry<bool> DoubleClickOnTabClosesIt { get; private set; }

        /// <summary>
        /// Specifies whether the panel selection dialog is always shown.
        /// </summary>
        public WinRegistryEntry<bool> AlwaysShowPanelSelectionDlg { get; private set; }

        /// <summary>
        /// Specifies whether an empty panel is created on startup.
        /// </summary>
        public WinRegistryEntry<bool> CreateEmptyPanelOnStartUp { get; private set; }

        /// <summary>
        /// Specifies the name of the startup panel.
        /// </summary>
        public WinRegistryEntry<string> StartUpPanelName { get; private set; }

        public OptRegistryTabsPanelsPage()
        {
            RegistryHive hive = WindowsRegistryInfo.Hive;
            string subKey = WindowsRegistryInfo.TabsAndPanelsOptions;

            AlwaysShowPanelTabs = new WinRegistryEntry<bool>(hive, subKey, nameof(AlwaysShowPanelTabs)).Read();
            ShowLogonInfoOnTabs = new WinRegistryEntry<bool>(hive, subKey, nameof(ShowLogonInfoOnTabs)).Read();
            ShowProtocolOnTabs = new WinRegistryEntry<bool>(hive, subKey, nameof(ShowProtocolOnTabs)).Read();
            IdentifyQuickConnectTabs = new WinRegistryEntry<bool>(hive, subKey, nameof(IdentifyQuickConnectTabs)).Read();
            DoubleClickOnTabClosesIt = new WinRegistryEntry<bool>(hive, subKey, nameof(DoubleClickOnTabClosesIt)).Read();
            AlwaysShowPanelSelectionDlg = new WinRegistryEntry<bool>(hive, subKey, nameof(AlwaysShowPanelSelectionDlg)).Read();
            CreateEmptyPanelOnStartUp = new WinRegistryEntry<bool>(hive, subKey, nameof(CreateEmptyPanelOnStartUp)).Read();
            StartUpPanelName = new WinRegistryEntry<string>(hive, subKey, nameof(StartUpPanelName)).Read();

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
            ApplyAlwaysShowPanelTabs();
            ApplyShowLogonInfoOnTabs();
            ApplyShowProtocolOnTabs();
            ApplyIdentifyQuickConnectTabs();
            ApplyDoubleClickOnTabClosesIt();
            ApplyAlwaysShowPanelSelectionDlg();
            ApplyCreateEmptyPanelOnStartUp();
            ApplyStartUpPanelName();
        }

        private void ApplyAlwaysShowPanelTabs()
        {
            if (AlwaysShowPanelTabs.IsSet)
                Properties.OptionsTabsPanelsPage.Default.AlwaysShowPanelTabs = AlwaysShowPanelTabs.Value;
        }

        private void ApplyShowLogonInfoOnTabs()
        {
            if (ShowLogonInfoOnTabs.IsSet)
                Properties.OptionsTabsPanelsPage.Default.ShowLogonInfoOnTabs = ShowLogonInfoOnTabs.Value;
        }

        private void ApplyShowProtocolOnTabs()
        {
            if (ShowProtocolOnTabs.IsSet)
                Properties.OptionsTabsPanelsPage.Default.ShowProtocolOnTabs = ShowProtocolOnTabs.Value;
        }

        private void ApplyIdentifyQuickConnectTabs()
        {
            if (IdentifyQuickConnectTabs.IsSet)
                Properties.OptionsTabsPanelsPage.Default.IdentifyQuickConnectTabs = IdentifyQuickConnectTabs.Value;
        }

        private void ApplyDoubleClickOnTabClosesIt()
        {
            if (DoubleClickOnTabClosesIt.IsSet)
                Properties.OptionsTabsPanelsPage.Default.DoubleClickOnTabClosesIt = DoubleClickOnTabClosesIt.Value;
        }

        private void ApplyAlwaysShowPanelSelectionDlg()
        {
            if (AlwaysShowPanelSelectionDlg.IsSet)
                Properties.OptionsTabsPanelsPage.Default.AlwaysShowPanelSelectionDlg = AlwaysShowPanelSelectionDlg.Value;
        }

        private void ApplyCreateEmptyPanelOnStartUp()
        {
            if (CreateEmptyPanelOnStartUp.IsSet)
                Properties.OptionsTabsPanelsPage.Default.CreateEmptyPanelOnStartUp = CreateEmptyPanelOnStartUp.Value;
        }

        private void ApplyStartUpPanelName()
        {
            if (StartUpPanelName.IsSet)
                Properties.OptionsTabsPanelsPage.Default.StartUpPanelName = StartUpPanelName.Value;
        }
    }
}