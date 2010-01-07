Imports System.Environment

Namespace App
    Namespace Info
        Public Class General
            Public Shared ReadOnly URLHome As String = "http://www.mRemoteNG.org/"
            Public Shared ReadOnly URLDonate As String = "http://donate.mRemoteNG.org/"
            Public Shared ReadOnly URLBugs As String = "http://bugs.mRemoteNG.org/"
            Public Shared ReadOnly URLAnnouncement As String = "http://update.mRemoteNG.org/announcement.txt"
            Public Shared ReadOnly LogFile As String = GetFolderPath(SpecialFolder.LocalApplicationData) & "\mRemoteNG.log"
            Public Shared ReadOnly HomePath As String = My.Application.Info.DirectoryPath
            Public Shared EncryptionKey As String = "mR3m"
            Public Shared ReportingFilePath As String = ""
            Public Shared SmartCodeURL As String = "http://www.s-code.com/products/viewerx/"
            Public Shared FamFamFamURL As String = "http://www.famfamfam.com/"
        End Class

        Public Class Settings
            'Exchange to make portable/normal
            Public Shared ReadOnly SettingsPath As String = GetFolderPath(SpecialFolder.LocalApplicationData) & "\" & My.Application.Info.ProductName
            'Public Shared ReadOnly SettingsPath As String = My.Application.Info.DirectoryPath

            Public Shared ReadOnly LayoutFileName As String = "pnlLayout.xml"
            Public Shared ReadOnly ExtAppsFilesName As String = "extApps.xml"
        End Class

        Public Class Update
            Public Shared ReadOnly URL As String = "http://update.mRemoteNG.org/"
#If DEBUG Then
            Public Shared ReadOnly File As String = "update-debug.txt"
#Else
            Public Shared ReadOnly File As String = "update.txt"
#End If
        End Class

        Public Class Connections
            Public Shared ReadOnly DefaultConnectionsPath As String = App.Info.Settings.SettingsPath
            Public Shared ReadOnly DefaultConnectionsFile As String = "confCons.xml"
            Public Shared ReadOnly DefaultConnectionsFileNew As String = "confConsNew.xml"
            Public Shared ReadOnly ConnectionFileVersion As Double = 2.1
        End Class

        Public Class Credentials
            Public Shared ReadOnly CredentialsPath As String = App.Info.Settings.SettingsPath
            Public Shared ReadOnly CredentialsFile As String = "confCreds.xml"
            Public Shared ReadOnly CredentialsFileNew As String = "confCredsNew.xml"
            Public Shared ReadOnly CredentialsFileVersion As Double = 1.0
        End Class
    End Namespace
End Namespace
