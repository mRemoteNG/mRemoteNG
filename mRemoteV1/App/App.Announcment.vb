Imports System.IO
Imports System.Net
Imports mRemote.App.Runtime

Namespace App
    Public Class Announcment
#Region "Private Properties"
        Private wCl As WebClient
        Private wPr As WebProxy
#End Region

        Private _curAI As Info
        Public ReadOnly Property curAI() As Info
            Get
                Return _curAI
            End Get
        End Property

        Public Function IsAnnouncmentAvailable() As Boolean
            Try
                Dim aI As Info = GetAnnouncmentInfo()

                If aI.InfoOk = False Then
                    Return False
                End If

                If aI.Name <> My.Settings.LastAnnouncment Then
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.WarningMsg, "IsAnnouncmentAvailable failed" & vbNewLine & ex.Message, True)
                Return False
            End Try
        End Function

        Public Function GetAnnouncmentInfo() As Info
            Try
                Dim strAnnouncment As String = GetAnnouncmentFile()

                CreateWebClient()

                Dim aI As New Info()

                If strAnnouncment <> "" Then
                    aI.InfoOk = True

                    Try
                        'get Name
                        Dim strName As String = strAnnouncment.Substring(strAnnouncment.IndexOf("Name: ") + 6, strAnnouncment.IndexOf(vbNewLine) - 6)
                        aI.Name = strName

                        strAnnouncment = strAnnouncment.Remove(0, strAnnouncment.IndexOf(vbNewLine) + 2)

                        'get Download URL
                        Dim strU As String = ""

                        strU = strAnnouncment.Substring(strAnnouncment.IndexOf("URL: ") + 5, strAnnouncment.IndexOf(vbNewLine) - 5)

                        aI.URL = strU
                    Catch ex As Exception
                        aI.InfoOk = False
                    End Try
                Else
                    aI.InfoOk = False
                End If

                _curAI = aI
                Return aI
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.WarningMsg, "Getting announcment info failed" & vbNewLine & ex.Message, True)
                Return Nothing
            End Try
        End Function

        Private Function GetAnnouncmentFile() As String
            Try
                CreateWebClient()

                Dim strTemp As String

                Try
                    strTemp = wCl.DownloadString(App.Info.General.URLAnnouncment)
                Catch ex As Exception
                    strTemp = ""
                End Try

                Return strTemp
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.WarningMsg, "GetAnnouncmentFile failed" & vbNewLine & ex.Message, True)
                Return ""
            End Try
        End Function

        Private Sub CreateWebClient()
            wCl = New WebClient()

            If My.Settings.UpdateUseProxy Then
                wPr = New WebProxy(My.Settings.UpdateProxyAddress, My.Settings.UpdateProxyPort)

                If My.Settings.UpdateProxyUseAuthentication Then
                    Dim cred As ICredentials
                    cred = New NetworkCredential(My.Settings.UpdateProxyAuthUser, Security.Crypt.Decrypt(My.Settings.UpdateProxyAuthPass, App.Info.General.EncryptionKey))

                    wPr.Credentials = cred
                End If

                wCl.Proxy = wPr
            End If
        End Sub




        Public Class Info
            Private _Name As String
            Public Property Name() As String
                Get
                    Return _Name
                End Get
                Set(ByVal value As String)
                    _Name = value
                End Set
            End Property

            Private _URL As String
            Public Property URL() As String
                Get
                    Return _URL
                End Get
                Set(ByVal value As String)
                    _URL = value
                End Set
            End Property


            Private _InfoOk As Boolean
            Public Property InfoOk() As Boolean
                Get
                    Return _InfoOk
                End Get
                Set(ByVal value As Boolean)
                    _InfoOk = value
                End Set
            End Property
        End Class
    End Class
End Namespace