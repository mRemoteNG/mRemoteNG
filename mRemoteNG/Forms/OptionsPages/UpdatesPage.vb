Imports System.ComponentModel
Imports mRemoteNG.My
Imports mRemoteNG.App
Imports mRemoteNG.App.Info
Imports mRemoteNG.Security
Imports mRemoteNG.Tools
Imports mRemoteNG.UI.Window
Imports PSTaskDialog

Namespace Forms.OptionsPages
    Public Class UpdatesPage
#Region "Public Methods"
        Public Overrides Property PageName() As String
            Get
                Return Language.strTabUpdates
            End Get
            Set(value As String)
            End Set
        End Property

        Public Overrides Sub ApplyLanguage()
            MyBase.ApplyLanguage()

#If Not PORTABLE Then
            lblUpdatesExplanation.Text = Language.strUpdateCheck
#Else
            lblUpdatesExplanation.Text = Language.strUpdateCheckPortableEdition
#End If

            chkCheckForUpdatesOnStartup.Text = Language.strCheckForUpdatesOnStartup
            btnUpdateCheckNow.Text = Language.strCheckNow

            chkUseProxyForAutomaticUpdates.Text = Language.strCheckboxUpdateUseProxy
            lblProxyAddress.Text = Language.strLabelAddress
            lblProxyPort.Text = Language.strLabelPort

            chkUseProxyAuthentication.Text = Language.strCheckboxProxyAuthentication
            lblProxyUsername.Text = Language.strLabelUsername
            lblProxyPassword.Text = Language.strLabelPassword

            btnTestProxy.Text = Language.strButtonTestProxy
        End Sub

        Public Overrides Sub LoadSettings()
            MyBase.SaveSettings()

            chkCheckForUpdatesOnStartup.Checked = My.Settings.CheckForUpdatesOnStartup
            cboUpdateCheckFrequency.Enabled = chkCheckForUpdatesOnStartup.Checked
            cboUpdateCheckFrequency.Items.Clear()
            Dim nDaily As Integer = cboUpdateCheckFrequency.Items.Add(Language.strUpdateFrequencyDaily)
            Dim nWeekly As Integer = cboUpdateCheckFrequency.Items.Add(Language.strUpdateFrequencyWeekly)
            Dim nMonthly As Integer = cboUpdateCheckFrequency.Items.Add(Language.strUpdateFrequencyMonthly)
            Select Case My.Settings.CheckForUpdatesFrequencyDays
                Case Is < 1
                    chkCheckForUpdatesOnStartup.Checked = False
                    cboUpdateCheckFrequency.SelectedIndex = nDaily
                Case 1 ' Daily
                    cboUpdateCheckFrequency.SelectedIndex = nDaily
                Case 7 ' Weekly
                    cboUpdateCheckFrequency.SelectedIndex = nWeekly
                Case 31 ' Monthly
                    cboUpdateCheckFrequency.SelectedIndex = nMonthly
                Case Else
                    Dim nCustom As Integer = cboUpdateCheckFrequency.Items.Add(String.Format(Language.strUpdateFrequencyCustom, My.Settings.CheckForUpdatesFrequencyDays))
                    cboUpdateCheckFrequency.SelectedIndex = nCustom
            End Select

            chkUseProxyForAutomaticUpdates.Checked = My.Settings.UpdateUseProxy
            pnlProxyBasic.Enabled = My.Settings.UpdateUseProxy
            txtProxyAddress.Text = My.Settings.UpdateProxyAddress
            numProxyPort.Value = My.Settings.UpdateProxyPort

            chkUseProxyAuthentication.Checked = My.Settings.UpdateProxyUseAuthentication
            pnlProxyAuthentication.Enabled = My.Settings.UpdateProxyUseAuthentication
            txtProxyUsername.Text = My.Settings.UpdateProxyAuthUser
            txtProxyPassword.Text = Crypt.Decrypt(My.Settings.UpdateProxyAuthPass, General.EncryptionKey)

            btnTestProxy.Enabled = My.Settings.UpdateUseProxy

#If PORTABLE Then
        For Each Control As Control In Controls
            If Control IsNot lblUpdatesExplanation Then
                Control.Visible = False
            End If
        Next
#End If
        End Sub

        Public Overrides Sub SaveSettings()
            MyBase.SaveSettings()

            My.Settings.CheckForUpdatesOnStartup = chkCheckForUpdatesOnStartup.Checked
            Select Case cboUpdateCheckFrequency.SelectedItem.ToString()
                Case Language.strUpdateFrequencyDaily
                    My.Settings.CheckForUpdatesFrequencyDays = 1
                Case Language.strUpdateFrequencyWeekly
                    My.Settings.CheckForUpdatesFrequencyDays = 7
                Case Language.strUpdateFrequencyMonthly
                    My.Settings.CheckForUpdatesFrequencyDays = 31
            End Select

            My.Settings.UpdateUseProxy = chkUseProxyForAutomaticUpdates.Checked
            My.Settings.UpdateProxyAddress = txtProxyAddress.Text
            My.Settings.UpdateProxyPort = numProxyPort.Value

            My.Settings.UpdateProxyUseAuthentication = chkUseProxyAuthentication.Checked
            My.Settings.UpdateProxyAuthUser = txtProxyUsername.Text
            My.Settings.UpdateProxyAuthPass = Crypt.Encrypt(txtProxyPassword.Text, General.EncryptionKey)
        End Sub
#End Region

#Region "Private Fields"
        Private _appUpdate As App.Update
#End Region

#Region "Private Methods"
#Region "Event Handlers"
        Private Sub chkCheckForUpdatesOnStartup_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkCheckForUpdatesOnStartup.CheckedChanged
            cboUpdateCheckFrequency.Enabled = chkCheckForUpdatesOnStartup.Checked
        End Sub

        Private Sub btnUpdateCheckNow_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdateCheckNow.Click
            Runtime.Windows.Show(Type.Update)
        End Sub

        Private Sub chkUseProxyForAutomaticUpdates_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkUseProxyForAutomaticUpdates.CheckedChanged
            pnlProxyBasic.Enabled = chkUseProxyForAutomaticUpdates.Checked
            btnTestProxy.Enabled = chkUseProxyForAutomaticUpdates.Checked

            If chkUseProxyForAutomaticUpdates.Checked Then
                chkUseProxyAuthentication.Enabled = True

                If chkUseProxyAuthentication.Checked Then
                    pnlProxyAuthentication.Enabled = True
                End If
            Else
                chkUseProxyAuthentication.Enabled = False
                pnlProxyAuthentication.Enabled = False
            End If
        End Sub

        Private Sub btnTestProxy_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTestProxy.Click
            If _appUpdate IsNot Nothing Then
                If _appUpdate.IsGetUpdateInfoRunning Then Return
            End If

            _appUpdate = New App.Update
            _appUpdate.SetProxySettings(chkUseProxyForAutomaticUpdates.Checked, txtProxyAddress.Text, numProxyPort.Value, chkUseProxyAuthentication.Checked, txtProxyUsername.Text, txtProxyPassword.Text)

            btnTestProxy.Enabled = False
            btnTestProxy.Text = Language.strOptionsProxyTesting

            AddHandler _appUpdate.GetUpdateInfoCompletedEvent, AddressOf GetUpdateInfoCompleted

            _appUpdate.GetUpdateInfoAsync()
        End Sub

        Private Sub chkUseProxyAuthentication_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkUseProxyAuthentication.CheckedChanged
            If chkUseProxyForAutomaticUpdates.Checked Then
                If chkUseProxyAuthentication.Checked Then
                    pnlProxyAuthentication.Enabled = True
                Else
                    pnlProxyAuthentication.Enabled = False
                End If
            End If
        End Sub
#End Region

        Private Sub GetUpdateInfoCompleted(ByVal sender As Object, ByVal e As AsyncCompletedEventArgs)
            If InvokeRequired Then
                Dim myDelegate As New AsyncCompletedEventHandler(AddressOf GetUpdateInfoCompleted)
                Invoke(myDelegate, New Object() {sender, e})
                Return
            End If

            Try
                RemoveHandler _appUpdate.GetUpdateInfoCompletedEvent, AddressOf GetUpdateInfoCompleted

                btnTestProxy.Enabled = True
                btnTestProxy.Text = Language.strButtonTestProxy

                If e.Cancelled Then Return
                If e.Error IsNot Nothing Then Throw e.Error

                cTaskDialog.ShowCommandBox(Me, Application.Info.ProductName, Language.strProxyTestSucceeded, "", Language.strButtonOK, False)
            Catch ex As Exception
                cTaskDialog.ShowCommandBox(Me, Application.Info.ProductName, Language.strProxyTestFailed, Misc.GetExceptionMessageRecursive(ex), "", "", "", Language.strButtonOK, False, eSysIcons.Error, 0)
            End Try
        End Sub
#End Region
    End Class
End Namespace