Imports mRemoteNG.My

Namespace Forms.OptionsPages
    Public Class StartupExitPage
        Public Overrides Property PageName() As String
            Get
                Return Language.strStartupExit
            End Get
            Set(value As String)
            End Set
        End Property

        Public Overrides Sub ApplyLanguage()
            MyBase.ApplyLanguage()

            chkSaveConsOnExit.Text = Language.strSaveConsOnExit
            chkReconnectOnStart.Text = Language.strReconnectAtStartup
            chkSingleInstance.Text = Language.strAllowOnlySingleInstance
            chkProperInstallationOfComponentsAtStartup.Text = Language.strCheckProperInstallationOfComponentsAtStartup
        End Sub

        Public Overrides Sub SaveSettings()
            MyBase.SaveSettings()

            Settings.SaveConsOnExit = chkSaveConsOnExit.Checked
            Settings.OpenConsFromLastSession = chkReconnectOnStart.Checked
            Settings.SingleInstance = chkSingleInstance.Checked
            Settings.StartupComponentsCheck = chkProperInstallationOfComponentsAtStartup.Checked
        End Sub

        Private Sub StartupExitPage_Load(sender As System.Object, e As EventArgs) Handles MyBase.Load
            chkSaveConsOnExit.Checked = Settings.SaveConsOnExit
            chkReconnectOnStart.Checked = Settings.OpenConsFromLastSession
            chkSingleInstance.Checked = Settings.SingleInstance
            chkProperInstallationOfComponentsAtStartup.Checked = Settings.StartupComponentsCheck
        End Sub
    End Class
End Namespace