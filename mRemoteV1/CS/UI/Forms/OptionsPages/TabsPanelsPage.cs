using System.Collections.Generic;
using System;
using AxWFICALib;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using AxMSTSCLib;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
using mRemoteNG.My;


namespace mRemoteNG.Forms.OptionsPages
{
	public partial class TabsPanelsPage
	{
		public TabsPanelsPage()
		{
			InitializeComponent();
		}
public override string PageName
		{
			get
			{
				return Language.strTabsAndPanels.Replace("&&", "&");
			}
			set
			{
			}
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

            chkAlwaysShowPanelTabs.Checked = My.Settings.Default.AlwaysShowPanelTabs;
            chkOpenNewTabRightOfSelected.Checked = My.Settings.Default.OpenTabsRightOfSelected;
            chkShowLogonInfoOnTabs.Checked = My.Settings.Default.ShowLogonInfoOnTabs;
            chkShowProtocolOnTabs.Checked = My.Settings.Default.ShowProtocolOnTabs;
            chkIdentifyQuickConnectTabs.Checked = My.Settings.Default.IdentifyQuickConnectTabs;
            chkDoubleClickClosesTab.Checked = My.Settings.Default.DoubleClickOnTabClosesIt;
            chkAlwaysShowPanelSelectionDlg.Checked = My.Settings.Default.AlwaysShowPanelSelectionDlg;

            chkUseOnlyErrorsAndInfosPanel.Checked = My.Settings.Default.ShowNoMessageBoxes;
            chkMCInformation.Checked = My.Settings.Default.SwitchToMCOnInformation;
            chkMCWarnings.Checked = My.Settings.Default.SwitchToMCOnWarning;
            chkMCErrors.Checked = My.Settings.Default.SwitchToMCOnError;
		}
			
		public override void SaveSettings()
		{
			base.SaveSettings();

			My.Settings.Default.AlwaysShowPanelTabs = chkAlwaysShowPanelTabs.Checked;
			frmMain.Default.ShowHidePanelTabs();
				
			My.Settings.Default.OpenTabsRightOfSelected = chkOpenNewTabRightOfSelected.Checked;
            My.Settings.Default.ShowLogonInfoOnTabs = chkShowLogonInfoOnTabs.Checked;
            My.Settings.Default.ShowProtocolOnTabs = chkShowProtocolOnTabs.Checked;
            My.Settings.Default.IdentifyQuickConnectTabs = chkIdentifyQuickConnectTabs.Checked;
            My.Settings.Default.DoubleClickOnTabClosesIt = chkDoubleClickClosesTab.Checked;
            My.Settings.Default.AlwaysShowPanelSelectionDlg = chkAlwaysShowPanelSelectionDlg.Checked;

            My.Settings.Default.ShowNoMessageBoxes = chkUseOnlyErrorsAndInfosPanel.Checked;
            My.Settings.Default.SwitchToMCOnInformation = chkMCInformation.Checked;
            My.Settings.Default.SwitchToMCOnWarning = chkMCWarnings.Checked;
            My.Settings.Default.SwitchToMCOnError = chkMCErrors.Checked;
		}
			
		public void chkUseOnlyErrorsAndInfosPanel_CheckedChanged(object sender, EventArgs e)
		{
			chkMCInformation.Enabled = chkUseOnlyErrorsAndInfosPanel.Checked;
			chkMCWarnings.Enabled = chkUseOnlyErrorsAndInfosPanel.Checked;
			chkMCErrors.Enabled = chkUseOnlyErrorsAndInfosPanel.Checked;
		}
	}
}
