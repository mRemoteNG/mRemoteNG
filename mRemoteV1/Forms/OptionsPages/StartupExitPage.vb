


Namespace Forms.OptionsPages
    Public Class StartupExitPage
        Public Overrides Property PageName As String
            Get
                Return Language.Language.strStartupExit
            End Get
            Set
            End Set
        End Property

        Public Overrides Sub ApplyLanguage()
            MyBase.ApplyLanguage()

            chkSaveConsOnExit.Text = Language.Language.strSaveConsOnExit
            chkReconnectOnStart.Text = Language.Language.strReconnectAtStartup
            chkSingleInstance.Text = Language.Language.strAllowOnlySingleInstance
            chkProperInstallationOfComponentsAtStartup.Text =
                Language.Language.strCheckProperInstallationOfComponentsAtStartup
        End Sub

        Public Overrides Sub SaveSettings()
            MyBase.SaveSettings()

            My.Settings.SaveConsOnExit = chkSaveConsOnExit.Checked
            My.Settings.OpenConsFromLastSession = chkReconnectOnStart.Checked
            My.Settings.SingleInstance = chkSingleInstance.Checked
            My.Settings.StartupComponentsCheck = chkProperInstallationOfComponentsAtStartup.Checked
        End Sub

        Private Sub StartupExitPage_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            chkSaveConsOnExit.Checked = My.Settings.SaveConsOnExit
            chkReconnectOnStart.Checked = My.Settings.OpenConsFromLastSession
            chkSingleInstance.Checked = My.Settings.SingleInstance
            chkProperInstallationOfComponentsAtStartup.Checked = My.Settings.StartupComponentsCheck
        End Sub
    End Class
End Namespace