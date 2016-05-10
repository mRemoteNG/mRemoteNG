using System;
using mRemoteNG.My;

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

            chkAlwaysShowPanelTabs.Checked = mRemoteNG.Settings.Default.AlwaysShowPanelTabs;
            chkOpenNewTabRightOfSelected.Checked = mRemoteNG.Settings.Default.OpenTabsRightOfSelected;
            chkShowLogonInfoOnTabs.Checked = mRemoteNG.Settings.Default.ShowLogonInfoOnTabs;
            chkShowProtocolOnTabs.Checked = mRemoteNG.Settings.Default.ShowProtocolOnTabs;
            chkIdentifyQuickConnectTabs.Checked = mRemoteNG.Settings.Default.IdentifyQuickConnectTabs;
            chkDoubleClickClosesTab.Checked = mRemoteNG.Settings.Default.DoubleClickOnTabClosesIt;
            chkAlwaysShowPanelSelectionDlg.Checked = mRemoteNG.Settings.Default.AlwaysShowPanelSelectionDlg;

            chkUseOnlyErrorsAndInfosPanel.Checked = mRemoteNG.Settings.Default.ShowNoMessageBoxes;
            chkMCInformation.Checked = mRemoteNG.Settings.Default.SwitchToMCOnInformation;
            chkMCWarnings.Checked = mRemoteNG.Settings.Default.SwitchToMCOnWarning;
            chkMCErrors.Checked = mRemoteNG.Settings.Default.SwitchToMCOnError;
        }

        public override void SaveSettings()
        {
            base.SaveSettings();

            mRemoteNG.Settings.Default.AlwaysShowPanelTabs = chkAlwaysShowPanelTabs.Checked;
            frmMain.Default.ShowHidePanelTabs();

            mRemoteNG.Settings.Default.OpenTabsRightOfSelected = chkOpenNewTabRightOfSelected.Checked;
            mRemoteNG.Settings.Default.ShowLogonInfoOnTabs = chkShowLogonInfoOnTabs.Checked;
            mRemoteNG.Settings.Default.ShowProtocolOnTabs = chkShowProtocolOnTabs.Checked;
            mRemoteNG.Settings.Default.IdentifyQuickConnectTabs = chkIdentifyQuickConnectTabs.Checked;
            mRemoteNG.Settings.Default.DoubleClickOnTabClosesIt = chkDoubleClickClosesTab.Checked;
            mRemoteNG.Settings.Default.AlwaysShowPanelSelectionDlg = chkAlwaysShowPanelSelectionDlg.Checked;

            mRemoteNG.Settings.Default.ShowNoMessageBoxes = chkUseOnlyErrorsAndInfosPanel.Checked;
            mRemoteNG.Settings.Default.SwitchToMCOnInformation = chkMCInformation.Checked;
            mRemoteNG.Settings.Default.SwitchToMCOnWarning = chkMCWarnings.Checked;
            mRemoteNG.Settings.Default.SwitchToMCOnError = chkMCErrors.Checked;
        }

        public void chkUseOnlyErrorsAndInfosPanel_CheckedChanged(object sender, EventArgs e)
        {
            chkMCInformation.Enabled = chkUseOnlyErrorsAndInfosPanel.Checked;
            chkMCWarnings.Enabled = chkUseOnlyErrorsAndInfosPanel.Checked;
            chkMCErrors.Enabled = chkUseOnlyErrorsAndInfosPanel.Checked;
        }
    }
}