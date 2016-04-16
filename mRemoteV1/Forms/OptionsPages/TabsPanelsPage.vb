


Namespace Forms.OptionsPages
    Public Class TabsPanelsPage
        Public Overrides Property PageName As String
            Get
                Return Language.Language.strTabsAndPanels.Replace("&&", "&")
            End Get
            Set
            End Set
        End Property

        Public Overrides Sub ApplyLanguage()
            MyBase.ApplyLanguage()

            chkAlwaysShowPanelTabs.Text = Language.Language.strAlwaysShowPanelTabs
            chkOpenNewTabRightOfSelected.Text = Language.Language.strOpenNewTabRight
            chkShowLogonInfoOnTabs.Text = Language.Language.strShowLogonInfoOnTabs
            chkShowProtocolOnTabs.Text = Language.Language.strShowProtocolOnTabs
            chkIdentifyQuickConnectTabs.Text = Language.Language.strIdentifyQuickConnectTabs
            chkDoubleClickClosesTab.Text = Language.Language.strDoubleClickTabClosesIt
            chkAlwaysShowPanelSelectionDlg.Text = Language.Language.strAlwaysShowPanelSelection

            chkUseOnlyErrorsAndInfosPanel.Text = Language.Language.strUseOnlyErrorsAndInfosPanel
            lblSwitchToErrorsAndInfos.Text = Language.Language.strSwitchToErrorsAndInfos
            chkMCInformation.Text = Language.Language.strInformations
            chkMCWarnings.Text = Language.Language.strWarnings
            chkMCErrors.Text = Language.Language.strErrors
        End Sub

        Public Overrides Sub LoadSettings()
            MyBase.SaveSettings()

            chkAlwaysShowPanelTabs.Checked = My.Settings.AlwaysShowPanelTabs
            chkOpenNewTabRightOfSelected.Checked = My.Settings.OpenTabsRightOfSelected
            chkShowLogonInfoOnTabs.Checked = My.Settings.ShowLogonInfoOnTabs
            chkShowProtocolOnTabs.Checked = My.Settings.ShowProtocolOnTabs
            chkIdentifyQuickConnectTabs.Checked = My.Settings.IdentifyQuickConnectTabs
            chkDoubleClickClosesTab.Checked = My.Settings.DoubleClickOnTabClosesIt
            chkAlwaysShowPanelSelectionDlg.Checked = My.Settings.AlwaysShowPanelSelectionDlg

            chkUseOnlyErrorsAndInfosPanel.Checked = My.Settings.ShowNoMessageBoxes
            chkMCInformation.Checked = My.Settings.SwitchToMCOnInformation
            chkMCWarnings.Checked = My.Settings.SwitchToMCOnWarning
            chkMCErrors.Checked = My.Settings.SwitchToMCOnError
        End Sub

        Public Overrides Sub SaveSettings()
            MyBase.SaveSettings()

            My.Settings.AlwaysShowPanelTabs = chkAlwaysShowPanelTabs.Checked
            frmMain.ShowHidePanelTabs()

            My.Settings.OpenTabsRightOfSelected = chkOpenNewTabRightOfSelected.Checked
            My.Settings.ShowLogonInfoOnTabs = chkShowLogonInfoOnTabs.Checked
            My.Settings.ShowProtocolOnTabs = chkShowProtocolOnTabs.Checked
            My.Settings.IdentifyQuickConnectTabs = chkIdentifyQuickConnectTabs.Checked
            My.Settings.DoubleClickOnTabClosesIt = chkDoubleClickClosesTab.Checked
            My.Settings.AlwaysShowPanelSelectionDlg = chkAlwaysShowPanelSelectionDlg.Checked

            My.Settings.ShowNoMessageBoxes = chkUseOnlyErrorsAndInfosPanel.Checked
            My.Settings.SwitchToMCOnInformation = chkMCInformation.Checked
            My.Settings.SwitchToMCOnWarning = chkMCWarnings.Checked
            My.Settings.SwitchToMCOnError = chkMCErrors.Checked
        End Sub

        Private Sub chkUseOnlyErrorsAndInfosPanel_CheckedChanged(sender As Object, e As EventArgs) _
            Handles chkUseOnlyErrorsAndInfosPanel.CheckedChanged
            chkMCInformation.Enabled = chkUseOnlyErrorsAndInfosPanel.Checked
            chkMCWarnings.Enabled = chkUseOnlyErrorsAndInfosPanel.Checked
            chkMCErrors.Enabled = chkUseOnlyErrorsAndInfosPanel.Checked
        End Sub
    End Class
End Namespace