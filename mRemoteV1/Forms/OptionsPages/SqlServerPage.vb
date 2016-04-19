
Imports mRemote3G.App
Imports mRemote3G.App.Info
Imports mRemote3G.Security

Namespace Forms.OptionsPages
    Public Class SqlServerPage
        Public Overrides Property PageName As String
            Get
                Return Language.Language.strSQLServer.TrimEnd(":")
            End Get
            Set
            End Set
        End Property

        Public Overrides Sub ApplyLanguage()
            MyBase.ApplyLanguage()

            lblExperimental.Text = Language.Language.strExperimental.ToUpper
            lblSQLInfo.Text = Language.Language.strSQLInfo

            chkUseSQLServer.Text = Language.Language.strUseSQLServer
            lblSQLServer.Text = Language.Language.strLabelHostname
            lblSQLDatabaseName.Text = Language.Language.strLabelSQLServerDatabaseName
            lblSQLUsername.Text = Language.Language.strLabelUsername
            lblSQLPassword.Text = Language.Language.strLabelPassword
        End Sub

        Public Overrides Sub LoadSettings()
            MyBase.SaveSettings()

            chkUseSQLServer.Checked = My.Settings.UseSQLServer
            txtSQLServer.Text = My.Settings.SQLHost
            txtSQLDatabaseName.Text = My.Settings.SQLDatabaseName
            txtSQLUsername.Text = My.Settings.SQLUser
            txtSQLPassword.Text = Crypt.Decrypt(My.Settings.SQLPass, General.EncryptionKey)
        End Sub

        Public Overrides Sub SaveSettings()
            MyBase.SaveSettings()

            My.Settings.UseSQLServer = chkUseSQLServer.Checked
            My.Settings.SQLHost = txtSQLServer.Text
            My.Settings.SQLDatabaseName = txtSQLDatabaseName.Text
            My.Settings.SQLUser = txtSQLUsername.Text
            My.Settings.SQLPass = Crypt.Encrypt(txtSQLPassword.Text, General.EncryptionKey)

            Runtime.Startup.DestroySQLUpdateHandlerAndStopTimer()
            frmMain.UsingSqlServer = My.Settings.UseSQLServer
            If My.Settings.UseSQLServer Then
                Runtime.Startup.CreateSQLUpdateHandlerAndStartTimer()
            End If
        End Sub

        Private Sub chkUseSQLServer_CheckedChanged(sender As Object, e As EventArgs) _
            Handles chkUseSQLServer.CheckedChanged
            lblSQLServer.Enabled = chkUseSQLServer.Checked
            lblSQLDatabaseName.Enabled = chkUseSQLServer.Checked
            lblSQLUsername.Enabled = chkUseSQLServer.Checked
            lblSQLPassword.Enabled = chkUseSQLServer.Checked
            txtSQLServer.Enabled = chkUseSQLServer.Checked
            txtSQLDatabaseName.Enabled = chkUseSQLServer.Checked
            txtSQLUsername.Enabled = chkUseSQLServer.Checked
            txtSQLPassword.Enabled = chkUseSQLServer.Checked
        End Sub
    End Class
End Namespace