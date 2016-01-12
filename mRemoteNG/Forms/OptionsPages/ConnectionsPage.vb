Imports mRemoteNG.My
Imports mRemoteNG.Config
Imports mRemoteNG.App.Info
Imports mRemoteNG.Security

Namespace Forms.OptionsPages
    Public Class ConnectionsPage
        Public Overrides Property PageName() As String
            Get
                Return Language.strConnections
            End Get
            Set(value As String)
            End Set
        End Property

        Public Overrides Sub ApplyLanguage()
            MyBase.ApplyLanguage()

            chkSingleClickOnConnectionOpensIt.Text = Language.strSingleClickOnConnectionOpensIt
            chkSingleClickOnOpenedConnectionSwitchesToIt.Text = Language.strSingleClickOnOpenConnectionSwitchesToIt
            chkHostnameLikeDisplayName.Text = Language.strSetHostnameLikeDisplayName

            lblRdpReconnectionCount.Text = Language.strRdpReconnectCount

            lblAutoSave1.Text = Language.strAutoSaveEvery
            lblAutoSave2.Text = Language.strAutoSaveMins

            lblDefaultCredentials.Text = Language.strEmptyUsernamePasswordDomainFields
            radCredentialsNoInfo.Text = Language.strNoInformation
            radCredentialsWindows.Text = Language.strMyCurrentWindowsCreds
            radCredentialsCustom.Text = Language.strTheFollowing
            lblCredentialsUsername.Text = Language.strLabelUsername
            lblCredentialsPassword.Text = Language.strLabelPassword
            lblCredentialsDomain.Text = Language.strLabelDomain

            lblClosingConnections.Text = Language.strLabelClosingConnections
            radCloseWarnAll.Text = Language.strRadioCloseWarnAll
            radCloseWarnMultiple.Text = Language.strRadioCloseWarnMultiple
            radCloseWarnExit.Text = Language.strRadioCloseWarnExit
            radCloseWarnNever.Text = Language.strRadioCloseWarnNever
        End Sub

        Public Overrides Sub LoadSettings()
            MyBase.SaveSettings()

            chkSingleClickOnConnectionOpensIt.Checked = My.Settings.SingleClickOnConnectionOpensIt
            chkSingleClickOnOpenedConnectionSwitchesToIt.Checked = My.Settings.SingleClickSwitchesToOpenConnection
            chkHostnameLikeDisplayName.Checked = My.Settings.SetHostnameLikeDisplayName

            numRdpReconnectionCount.Value = My.Settings.RdpReconnectionCount

            numAutoSave.Value = My.Settings.AutoSaveEveryMinutes

            Select Case My.Settings.EmptyCredentials
                ' ReSharper disable once StringLiteralTypo
                Case "noinfo"
                    radCredentialsNoInfo.Checked = True
                Case "windows"
                    radCredentialsWindows.Checked = True
                Case "custom"
                    radCredentialsCustom.Checked = True
            End Select

            txtCredentialsUsername.Text = My.Settings.DefaultUsername
            txtCredentialsPassword.Text = Crypt.Decrypt(My.Settings.DefaultPassword, General.EncryptionKey)
            txtCredentialsDomain.Text = My.Settings.DefaultDomain

            Select Case My.Settings.ConfirmCloseConnection
                Case ConfirmClose.Never
                    radCloseWarnNever.Checked = True
                Case ConfirmClose.Exit
                    radCloseWarnExit.Checked = True
                Case ConfirmClose.Multiple
                    radCloseWarnMultiple.Checked = True
                Case Else
                    radCloseWarnAll.Checked = True
            End Select
        End Sub

        Public Overrides Sub SaveSettings()
            MyBase.SaveSettings()

            My.Settings.SingleClickOnConnectionOpensIt = chkSingleClickOnConnectionOpensIt.Checked
            My.Settings.SingleClickSwitchesToOpenConnection = chkSingleClickOnOpenedConnectionSwitchesToIt.Checked
            My.Settings.SetHostnameLikeDisplayName = chkHostnameLikeDisplayName.Checked

            My.Settings.RdpReconnectionCount = numRdpReconnectionCount.Value

            My.Settings.AutoSaveEveryMinutes = numAutoSave.Value
            If My.Settings.AutoSaveEveryMinutes > 0 Then
                frmMain.tmrAutoSave.Interval = My.Settings.AutoSaveEveryMinutes * 60000
                frmMain.tmrAutoSave.Enabled = True
            Else
                frmMain.tmrAutoSave.Enabled = False
            End If

            If radCredentialsNoInfo.Checked Then
                ' ReSharper disable once StringLiteralTypo
                My.Settings.EmptyCredentials = "noinfo"
            ElseIf radCredentialsWindows.Checked Then
                My.Settings.EmptyCredentials = "windows"
            ElseIf radCredentialsCustom.Checked Then
                My.Settings.EmptyCredentials = "custom"
            End If

            My.Settings.DefaultUsername = txtCredentialsUsername.Text
            My.Settings.DefaultPassword = Crypt.Encrypt(txtCredentialsPassword.Text, General.EncryptionKey)
            My.Settings.DefaultDomain = txtCredentialsDomain.Text

            If radCloseWarnAll.Checked Then My.Settings.ConfirmCloseConnection = ConfirmClose.All
            If radCloseWarnMultiple.Checked Then My.Settings.ConfirmCloseConnection = ConfirmClose.Multiple
            If radCloseWarnExit.Checked Then My.Settings.ConfirmCloseConnection = ConfirmClose.Exit
            If radCloseWarnNever.Checked Then My.Settings.ConfirmCloseConnection = ConfirmClose.Never
        End Sub

        Private Sub radCredentialsCustom_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radCredentialsCustom.CheckedChanged
            lblCredentialsUsername.Enabled = radCredentialsCustom.Checked
            lblCredentialsPassword.Enabled = radCredentialsCustom.Checked
            lblCredentialsDomain.Enabled = radCredentialsCustom.Checked
            txtCredentialsUsername.Enabled = radCredentialsCustom.Checked
            txtCredentialsPassword.Enabled = radCredentialsCustom.Checked
            txtCredentialsDomain.Enabled = radCredentialsCustom.Checked
        End Sub
    End Class
End Namespace