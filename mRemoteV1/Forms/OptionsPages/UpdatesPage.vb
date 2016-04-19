Imports System.ComponentModel
Imports mRemote3G.App
Imports mRemote3G.App.Info
Imports mRemote3G.Security
Imports mRemote3G.Tools
Imports mRemote3G.UI.Window
Imports PSTaskDialog

Namespace Forms.OptionsPages
    Public Class UpdatesPage

#Region "Public Methods"

        Public Overrides Property PageName As String
            Get
                Return Language.Language.strTabUpdates
            End Get
            Set
            End Set
        End Property

        Public Overrides Sub ApplyLanguage()
            ' Temporary
            Return
            MyBase.ApplyLanguage()

#If Not PORTABLE Then
            lblUpdatesExplanation.Text = Language.Language.strUpdateCheck
#Else
            lblUpdatesExplanation.Text = Language.strUpdateCheckPortableEdition
#End If

            chkCheckForUpdatesOnStartup.Text = Language.Language.strCheckForUpdatesOnStartup
            btnUpdateCheckNow.Text = Language.Language.strCheckNow

            chkUseProxyForAutomaticUpdates.Text = Language.Language.strCheckboxUpdateUseProxy
            lblProxyAddress.Text = Language.Language.strLabelAddress
            lblProxyPort.Text = Language.Language.strLabelPort

            chkUseProxyAuthentication.Text = Language.Language.strCheckboxProxyAuthentication
            lblProxyUsername.Text = Language.Language.strLabelUsername
            lblProxyPassword.Text = Language.Language.strLabelPassword

            btnTestProxy.Text = Language.Language.strButtonTestProxy
        End Sub

        Public Overrides Sub LoadSettings()
            ' Temporary
            Return
            MyBase.SaveSettings()

            chkCheckForUpdatesOnStartup.Checked = My.Settings.CheckForUpdatesOnStartup
            cboUpdateCheckFrequency.Enabled = chkCheckForUpdatesOnStartup.Checked
            cboUpdateCheckFrequency.Items.Clear()
            Dim nDaily As Integer = cboUpdateCheckFrequency.Items.Add(Language.Language.strUpdateFrequencyDaily)
            Dim nWeekly As Integer = cboUpdateCheckFrequency.Items.Add(Language.Language.strUpdateFrequencyWeekly)
            Dim nMonthly As Integer = cboUpdateCheckFrequency.Items.Add(Language.Language.strUpdateFrequencyMonthly)
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
                    Dim nCustom As Integer =
                            cboUpdateCheckFrequency.Items.Add(String.Format(Language.Language.strUpdateFrequencyCustom,
                                                                            My.Settings.CheckForUpdatesFrequencyDays))
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
            ' Temporary
            Return
            MyBase.SaveSettings()

            My.Settings.CheckForUpdatesOnStartup = chkCheckForUpdatesOnStartup.Checked
            Select Case cboUpdateCheckFrequency.SelectedItem.ToString()
                Case Language.Language.strUpdateFrequencyDaily
                    My.Settings.CheckForUpdatesFrequencyDays = 1
                Case Language.Language.strUpdateFrequencyWeekly
                    My.Settings.CheckForUpdatesFrequencyDays = 7
                Case Language.Language.strUpdateFrequencyMonthly
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

        Private Sub chkCheckForUpdatesOnStartup_CheckedChanged(sender As Object, e As EventArgs) _
            Handles chkCheckForUpdatesOnStartup.CheckedChanged
            ' Temporary
            Return
            cboUpdateCheckFrequency.Enabled = chkCheckForUpdatesOnStartup.Checked
        End Sub

        Private Sub btnUpdateCheckNow_Click(sender As Object, e As EventArgs) Handles btnUpdateCheckNow.Click
            ' Temporary
            Return
            Runtime.Windows.Show(Type.Update)
        End Sub

        Private Sub chkUseProxyForAutomaticUpdates_CheckedChanged(sender As Object, e As EventArgs) _
            Handles chkUseProxyForAutomaticUpdates.CheckedChanged
            ' Temporary
            Return
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

        Private Sub btnTestProxy_Click(sender As Object, e As EventArgs) Handles btnTestProxy.Click
            ' Temporary
            Return
            If _appUpdate IsNot Nothing Then
                If _appUpdate.IsGetUpdateInfoRunning Then Return
            End If

            _appUpdate = New App.Update
            _appUpdate.SetProxySettings(chkUseProxyForAutomaticUpdates.Checked, txtProxyAddress.Text, numProxyPort.Value,
                                        chkUseProxyAuthentication.Checked, txtProxyUsername.Text, txtProxyPassword.Text)

            btnTestProxy.Enabled = False
            btnTestProxy.Text = Language.Language.strOptionsProxyTesting

            AddHandler _appUpdate.GetUpdateInfoCompletedEvent, AddressOf GetUpdateInfoCompleted

            _appUpdate.GetUpdateInfoAsync()
        End Sub

        Private Sub chkUseProxyAuthentication_CheckedChanged(sender As Object, e As EventArgs) _
            Handles chkUseProxyAuthentication.CheckedChanged
            ' Temporary
            Return
            If chkUseProxyForAutomaticUpdates.Checked Then
                If chkUseProxyAuthentication.Checked Then
                    pnlProxyAuthentication.Enabled = True
                Else
                    pnlProxyAuthentication.Enabled = False
                End If
            End If
        End Sub

#End Region

        Private Sub GetUpdateInfoCompleted(sender As Object, e As AsyncCompletedEventArgs)
            ' Temporary
            Return
            If InvokeRequired Then
                Dim myDelegate As New AsyncCompletedEventHandler(AddressOf GetUpdateInfoCompleted)
                Invoke(myDelegate, New Object() {sender, e})
                Return
            End If

            Try
                RemoveHandler _appUpdate.GetUpdateInfoCompletedEvent, AddressOf GetUpdateInfoCompleted

                btnTestProxy.Enabled = True
                btnTestProxy.Text = Language.Language.strButtonTestProxy

                If e.Cancelled Then Return
                If e.Error IsNot Nothing Then Throw e.Error

                cTaskDialog.ShowCommandBox(Me, Application.ProductName, Language.Language.strProxyTestSucceeded, "",
                                           Language.Language.strButtonOK, False)
            Catch ex As Exception
                cTaskDialog.ShowCommandBox(Me, Application.ProductName, Language.Language.strProxyTestFailed,
                                           Misc.GetExceptionMessageRecursive(ex), "", "", "",
                                           Language.Language.strButtonOK, False, eSysIcons.Error, 0)
            End Try
        End Sub

#End Region
    End Class
End Namespace