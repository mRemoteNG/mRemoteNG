Imports System.Threading
Imports mRemote3G.My
Imports mRemote3G.Tools

Namespace App

    Namespace Info
        Public Class General
            Public Shared ReadOnly URLHome As String = "https://github.com/kmscode/mRemote3G"
            Public Shared ReadOnly URLForum As String = "https://github.com/kmscode/mRemote3G/issues"
            Public Shared ReadOnly URLBugs As String = "https://github.com/kmscode/mRemote3G/issues"
            Public Shared ReadOnly HomePath As String = Application.Info.DirectoryPath
            Public Shared EncryptionKey As String = "mR3m"
            Public Shared ReportingFilePath As String = ""
            Public Shared ReadOnly PuttyPath As String = Application.Info.DirectoryPath & "\PuTTYNG.exe"

            Public Shared ReadOnly Property UserAgent As String
                Get
                    Dim details As New List(Of String)
                    details.Add("compatible")
                    If Environment.OSVersion.Platform = PlatformID.Win32NT Then
                        details.Add(String.Format("Windows NT {0}.{1}", Environment.OSVersion.Version.Major,
                                                  Environment.OSVersion.Version.Minor))
                    Else
                        details.Add(Environment.OSVersion.VersionString)
                    End If
                    If EnvironmentInfo.IsWow64 Then details.Add("WOW64")
                    details.Add(Thread.CurrentThread.CurrentUICulture.Name)
                    details.Add(String.Format(".NET CLR {0}", Environment.Version.ToString()))
                    Dim detailsString As String = String.Join("; ", details.ToArray())

                    Return _
                        String.Format("Mozilla/4.0 ({0}) {1}/{2}", detailsString,
                                      System.Windows.Forms.Application.ProductName,
                                      System.Windows.Forms.Application.ProductVersion)
                End Get
            End Property

            Private Sub New()
            End Sub
        End Class

        Public Class Settings
#If Not PORTABLE Then

            Public Shared ReadOnly _
                SettingsPath As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" &
                                         System.Windows.Forms.Application.ProductName

#Else
            Public Shared ReadOnly SettingsPath As String = My.Application.Info.DirectoryPath
#End If
            Public Shared ReadOnly LayoutFileName As String = "pnlLayout.xml"
            Public Shared ReadOnly ExtAppsFilesName As String = "extApps.xml"
            Public Const ThemesFileName As String = "Themes.xml"

            Private Sub New()
            End Sub
        End Class

        Public Class UpdateApp
            Public Shared ReadOnly Property FileName As String
                Get
#If DEBUG Then
                    Return "update-debug.txt"
#End If
                    Select Case MySettingsProperty.Settings.UpdateChannel.ToLowerInvariant()
                        Case "beta"
                            Return "update-beta.txt"
                        Case "debug"
                            Return "update-debug.txt"
                        Case Else
                            Return "update.txt"
                    End Select
                End Get
            End Property

            Private Sub New()
            End Sub
        End Class

        Public Class Connections
            Public Shared ReadOnly DefaultConnectionsPath As String = Settings.SettingsPath
            Public Shared ReadOnly DefaultConnectionsFile As String = "confCons.xml"
            Public Shared ReadOnly DefaultConnectionsFileNew As String = "confConsNew.xml"
            Public Shared ReadOnly ConnectionFileVersion As Double = 2.5

            Private Sub New()
            End Sub
        End Class

        Public Class Credentials
            Public Shared ReadOnly CredentialsPath As String = Settings.SettingsPath
            Public Shared ReadOnly CredentialsFile As String = "confCreds.xml"
            Public Shared ReadOnly CredentialsFileNew As String = "confCredsNew.xml"
            Public Shared ReadOnly CredentialsFileVersion As Double = 1.0

            Private Sub New()
            End Sub
        End Class
    End Namespace

End Namespace
