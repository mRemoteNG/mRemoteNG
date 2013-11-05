Imports mRemoteNG.My
Imports mRemoteNG.App
Imports mRemoteNG.App.Info
Imports mRemoteNG.Security

Namespace Forms.OptionsPages
    Public Class SqlServerPage
        Public Overrides Property PageName() As String
            Get
                Return Language.strSQLServer
            End Get
            Set(value As String)
            End Set
        End Property

        Public Overrides Sub ApplyLanguage()
            MyBase.ApplyLanguage()

            lblExperimental.Text = Language.strExperimental.ToUpper
            lblSQLInfo.Text = Language.strSQLInfo

            chkUseSQLServer.Text = Language.strUseSQLServer
            lblSQLServer.Text = Language.strLabelHostname
            lblSQLDatabaseName.Text = Language.strLabelSQLServerDatabaseName
            lblSQLUsername.Text = Language.strLabelUsername
            lblSQLPassword.Text = Language.strLabelPassword
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
            If My.Settings.UseSQLServer Then
                Runtime.SetMainFormText("SQL Server")
                Runtime.Startup.CreateSQLUpdateHandlerAndStartTimer()
            Else
                Runtime.SetMainFormText(Runtime.GetStartupConnectionFileName())
            End If
        End Sub

        Private Sub chkUseSQLServer_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkUseSQLServer.CheckedChanged
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