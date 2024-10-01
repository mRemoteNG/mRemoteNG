using mRemoteNG.Config.Settings.Registry;
using mRemoteNG.Properties;
using mRemoteNG.Resources.Language;
using System;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    [SupportedOSPlatform("windows")]
    public sealed partial class TabsPanelsPage
    {
        #region Private Fields

        private OptRegistryTabsPanelsPage pageRegSettingsInstance;

        #endregion

        public TabsPanelsPage()
        {
            InitializeComponent();
            ApplyTheme();
            PageIcon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.Tab_16x);
        }

        public override string PageName
        {
            get => Language.TabsAndPanels.Replace("&&", "&");
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            chkAlwaysShowPanelTabs.Text = Language.AlwaysShowPanelTabs;
            /* 
             * Comments added: June 16, 2024
             * UI control (chkAlwaysShowConnectionTabs) is not visible and poperty never used
            */
            //chkAlwaysShowConnectionTabs.Text = Language.AlwaysShowConnectionTabs;
            chkOpenNewTabRightOfSelected.Text = Language.OpenNewTabRight;
            chkShowLogonInfoOnTabs.Text = Language.ShowLogonInfoOnTabs;
            chkShowProtocolOnTabs.Text = Language.ShowProtocolOnTabs;
            chkIdentifyQuickConnectTabs.Text = Language.IdentifyQuickConnectTabs;
            chkDoubleClickClosesTab.Text = Language.DoubleClickTabClosesIt;
            chkAlwaysShowPanelSelectionDlg.Text = Language.AlwaysShowPanelSelection;
            chkCreateEmptyPanelOnStart.Text = Language.CreateEmptyPanelOnStartUp;
            lblPanelName.Text = $@"{Language.PanelName}:";

            lblRegistrySettingsUsedInfo.Text = Language.OptionsCompanyPolicyMessage;
        }

        public override void LoadSettings()
        {
            chkAlwaysShowPanelTabs.Checked = Properties.OptionsTabsPanelsPage.Default.AlwaysShowPanelTabs;

            /* 
             * Comment added: June 16, 2024
             * Properties.OptionsTabsPanelsPage.Default.AlwaysShowConnectionTabs nerver used
             *  UI control (chkAlwaysShowConnectionTabs) is not visible
            */
            //chkAlwaysShowConnectionTabs.Checked = Properties.OptionsTabsPanelsPage.Default.AlwaysShowConnectionTabs;

            /* 
             * Comment added: June 16, 2024
             * Properties.OptionsTabsPanelsPage.Default.OpenTabsRightOfSelected nerver used
             *  Set Visible = false
            */
            //chkOpenNewTabRightOfSelected.Checked = Properties.OptionsTabsPanelsPage.Default.OpenTabsRightOfSelected;
            chkOpenNewTabRightOfSelected.Visible = false;

            chkShowLogonInfoOnTabs.Checked = Properties.OptionsTabsPanelsPage.Default.ShowLogonInfoOnTabs;
            chkShowProtocolOnTabs.Checked = Properties.OptionsTabsPanelsPage.Default.ShowProtocolOnTabs;
            chkIdentifyQuickConnectTabs.Checked = Properties.OptionsTabsPanelsPage.Default.IdentifyQuickConnectTabs;
            chkDoubleClickClosesTab.Checked = Properties.OptionsTabsPanelsPage.Default.DoubleClickOnTabClosesIt;
            chkAlwaysShowPanelSelectionDlg.Checked = Properties.OptionsTabsPanelsPage.Default.AlwaysShowPanelSelectionDlg;
            chkCreateEmptyPanelOnStart.Checked = Properties.OptionsTabsPanelsPage.Default.CreateEmptyPanelOnStartUp;
            txtBoxPanelName.Text = Properties.OptionsTabsPanelsPage.Default.StartUpPanelName;
            UpdatePanelNameTextBox();
        }

        public override void SaveSettings()
        {
            base.SaveSettings();

            Properties.OptionsTabsPanelsPage.Default.AlwaysShowPanelTabs = chkAlwaysShowPanelTabs.Checked;
            /* 
             * Comment added: June 16, 2024
             * Properties.OptionsTabsPanelsPage.Default.AlwaysShowConnectionTabs nerver used
             */
            //Properties.OptionsTabsPanelsPage.Default.AlwaysShowConnectionTabs = chkAlwaysShowConnectionTabs.Checked;
            FrmMain.Default.ShowHidePanelTabs();

            /* 
             * Comment added: June 16, 2024
             * Properties.OptionsTabsPanelsPage.Default.OpenTabsRightOfSelected nerver used
            */
            //Properties.OptionsTabsPanelsPage.Default.OpenTabsRightOfSelected = chkOpenNewTabRightOfSelected.Checked;

            Properties.OptionsTabsPanelsPage.Default.ShowLogonInfoOnTabs = chkShowLogonInfoOnTabs.Checked;
            Properties.OptionsTabsPanelsPage.Default.ShowProtocolOnTabs = chkShowProtocolOnTabs.Checked;
            Properties.OptionsTabsPanelsPage.Default.IdentifyQuickConnectTabs = chkIdentifyQuickConnectTabs.Checked;
            Properties.OptionsTabsPanelsPage.Default.DoubleClickOnTabClosesIt = chkDoubleClickClosesTab.Checked;
            Properties.OptionsTabsPanelsPage.Default.AlwaysShowPanelSelectionDlg = chkAlwaysShowPanelSelectionDlg.Checked;
            Properties.OptionsTabsPanelsPage.Default.CreateEmptyPanelOnStartUp = chkCreateEmptyPanelOnStart.Checked;
            Properties.OptionsTabsPanelsPage.Default.StartUpPanelName = txtBoxPanelName.Text;
        }

        public override void LoadRegistrySettings()
        {
            Type settingsType = typeof(OptRegistryTabsPanelsPage);
            RegistryLoader.RegistrySettings.TryGetValue(settingsType, out var settings);
            pageRegSettingsInstance = settings as OptRegistryTabsPanelsPage;

            RegistryLoader.Cleanup(settingsType);

            // ***
            // Disable controls based on the registry settings.
            //
            if (pageRegSettingsInstance.AlwaysShowPanelTabs.IsSet)
                DisableControl(chkAlwaysShowPanelTabs);

            if (pageRegSettingsInstance.ShowLogonInfoOnTabs.IsSet)
                DisableControl(chkShowLogonInfoOnTabs);

            if (pageRegSettingsInstance.ShowProtocolOnTabs.IsSet)
                DisableControl(chkShowProtocolOnTabs);

            if (pageRegSettingsInstance.IdentifyQuickConnectTabs.IsSet)
                DisableControl(chkIdentifyQuickConnectTabs);

            if (pageRegSettingsInstance.DoubleClickOnTabClosesIt.IsSet)
                DisableControl(chkDoubleClickClosesTab);

            if (pageRegSettingsInstance.AlwaysShowPanelSelectionDlg.IsSet)
                DisableControl(chkAlwaysShowPanelSelectionDlg);

            if (pageRegSettingsInstance.CreateEmptyPanelOnStartUp.IsSet)
                DisableControl(chkCreateEmptyPanelOnStart);

            if (pageRegSettingsInstance.StartUpPanelName.IsSet)
                DisableControl(txtBoxPanelName);

            // Updates the visibility of the information label indicating whether registry settings are used.
            lblRegistrySettingsUsedInfo.Visible = ShowRegistrySettingsUsedInfo();
        }

        /// <summary>
        /// Checks if specific registry settings related to appearence page are used.
        /// </summary>
        private bool ShowRegistrySettingsUsedInfo()
        {
            return pageRegSettingsInstance.AlwaysShowPanelTabs.IsSet
                || pageRegSettingsInstance.ShowLogonInfoOnTabs.IsSet
                || pageRegSettingsInstance.ShowProtocolOnTabs.IsSet
                || pageRegSettingsInstance.IdentifyQuickConnectTabs.IsSet
                || pageRegSettingsInstance.DoubleClickOnTabClosesIt.IsSet
                || pageRegSettingsInstance.AlwaysShowPanelSelectionDlg.IsSet
                || pageRegSettingsInstance.CreateEmptyPanelOnStartUp.IsSet
                || pageRegSettingsInstance.StartUpPanelName.IsSet;
        }

        private void UpdatePanelNameTextBox()
        {
            if (! pageRegSettingsInstance.StartUpPanelName.IsSet)
                txtBoxPanelName.Enabled = chkCreateEmptyPanelOnStart.Checked;
        }

        private void chkCreateEmptyPanelOnStart_CheckedChanged(object sender, System.EventArgs e)
        {
            UpdatePanelNameTextBox();
        }
    }
}