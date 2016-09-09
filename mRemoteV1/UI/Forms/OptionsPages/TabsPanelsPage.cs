using System;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public partial class TabsPanelsPage
    {
        public TabsPanelsPage()
        {
            InitializeComponent();
        }

        public override string PageName
        {
            get { return Language.strTabsAndPanels.Replace("&&", "&"); }
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            chkAlwaysShowPanelTabs.Text = Language.strAlwaysShowPanelTabs;
            chkOpenNewTabRightOfSelected.Text = Language.strOpenNewTabRight;
            chkShowLogonInfoOnTabs.Text = Language.strShowLogonInfoOnTabs;
            chkShowProtocolOnTabs.Text = Language.strShowProtocolOnTabs;
            chkIdentifyQuickConnectTabs.Text = Language.strIdentifyQuickConnectTabs;
            chkDoubleClickClosesTab.Text = Language.strDoubleClickTabClosesIt;
            chkAlwaysShowPanelSelectionDlg.Text = Language.strAlwaysShowPanelSelection;

            chkUseOnlyErrorsAndInfosPanel.Text = Language.strUseOnlyErrorsAndInfosPanel;
            lblSwitchToErrorsAndInfos.Text = Language.strSwitchToErrorsAndInfos;
            chkMCInformation.Text = Language.strInformations;
            chkMCWarnings.Text = Language.strWarnings;
            chkMCErrors.Text = Language.strErrors;
        }

        public override void LoadSettings()
        {
            base.SaveSettings();

            chkAlwaysShowPanelTabs.Checked = Settings.Default.AlwaysShowPanelTabs;
            chkOpenNewTabRightOfSelected.Checked = Settings.Default.OpenTabsRightOfSelected;
            chkShowLogonInfoOnTabs.Checked = Settings.Default.ShowLogonInfoOnTabs;
            chkShowProtocolOnTabs.Checked = Settings.Default.ShowProtocolOnTabs;
            chkIdentifyQuickConnectTabs.Checked = Settings.Default.IdentifyQuickConnectTabs;
            chkDoubleClickClosesTab.Checked = Settings.Default.DoubleClickOnTabClosesIt;
            chkAlwaysShowPanelSelectionDlg.Checked = Settings.Default.AlwaysShowPanelSelectionDlg;

            chkUseOnlyErrorsAndInfosPanel.Checked = Settings.Default.ShowNoMessageBoxes;
            chkMCInformation.Checked = Settings.Default.SwitchToMCOnInformation;
            chkMCWarnings.Checked = Settings.Default.SwitchToMCOnWarning;
            chkMCErrors.Checked = Settings.Default.SwitchToMCOnError;
        }

        public override void SaveSettings()
        {
            base.SaveSettings();

            Settings.Default.AlwaysShowPanelTabs = chkAlwaysShowPanelTabs.Checked;
            frmMain.Default.ShowHidePanelTabs();

            Settings.Default.OpenTabsRightOfSelected = chkOpenNewTabRightOfSelected.Checked;
            Settings.Default.ShowLogonInfoOnTabs = chkShowLogonInfoOnTabs.Checked;
            Settings.Default.ShowProtocolOnTabs = chkShowProtocolOnTabs.Checked;
            Settings.Default.IdentifyQuickConnectTabs = chkIdentifyQuickConnectTabs.Checked;
            Settings.Default.DoubleClickOnTabClosesIt = chkDoubleClickClosesTab.Checked;
            Settings.Default.AlwaysShowPanelSelectionDlg = chkAlwaysShowPanelSelectionDlg.Checked;

            Settings.Default.ShowNoMessageBoxes = chkUseOnlyErrorsAndInfosPanel.Checked;
            Settings.Default.SwitchToMCOnInformation = chkMCInformation.Checked;
            Settings.Default.SwitchToMCOnWarning = chkMCWarnings.Checked;
            Settings.Default.SwitchToMCOnError = chkMCErrors.Checked;

            Settings.Default.Save();
        }

        private void chkUseOnlyErrorsAndInfosPanel_CheckedChanged(object sender, EventArgs e)
        {
            chkMCInformation.Enabled = chkUseOnlyErrorsAndInfosPanel.Checked;
            chkMCWarnings.Enabled = chkUseOnlyErrorsAndInfosPanel.Checked;
            chkMCErrors.Enabled = chkUseOnlyErrorsAndInfosPanel.Checked;
        }
    }
}