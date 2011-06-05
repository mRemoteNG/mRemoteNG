Imports System.IO
Imports System.Net
Imports mRemoteNG.App.Runtime

Namespace App
    Public Class Update
        Public Event DownloadProgressChanged(ByVal sender As Object, ByVal e As System.Net.DownloadProgressChangedEventArgs)
        Public Event DownloadCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs, ByVal Success As Boolean)

#Region "Public Properties"
        Private _curUI As Info
        Public ReadOnly Property curUI() As Info
            Get
                Return _curUI
            End Get
        End Property
#End Region

#Region "Private Properties"
        Private wCl As WebClient
        Private wPr As WebProxy
#End Region

#Region "Public Methods"
        Public Function IsProxyOK() As Boolean
            Try
                Dim uI As Info = GetUpdateInfo()

                Return uI.InfoOk
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "IsProxyOK (App.Update) failed" & vbNewLine & ex.Message, False)
                Return False
            End Try
        End Function

        Public Function IsUpdateAvailable() As Boolean
            Try
                Dim uI As Info = GetUpdateInfo()

                If uI.InfoOk = False Then
                    Return False
                End If

                If uI.Version > My.Application.Info.Version Then
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "IsUpdateAvailable failed" & vbNewLine & ex.Message, True)
                Return False
            End Try
        End Function

        Public Function GetUpdateInfo() As Info
            Try
                Dim strUpdate As String = GetUpdateFile()

                CreateWebClient()

                Dim uI As New Info()

                If strUpdate <> "" Then
                    uI.InfoOk = True

                    Try
                        'get Version
                        Dim strV As String = strUpdate.Substring(strUpdate.IndexOf("Version: ") + 9, strUpdate.IndexOf(vbNewLine) - 9)
                        uI.Version = New Version(strV)

                        strUpdate = strUpdate.Remove(0, strUpdate.IndexOf(vbNewLine) + 2)

                        'get Download URL
                        Dim strU As String = ""

                        strU = strUpdate.Substring(strUpdate.IndexOf("dURL: ") + 6, strUpdate.IndexOf(vbNewLine) - 6)

                        uI.DownloadUrl = strU

                        strUpdate = strUpdate.Remove(0, strUpdate.IndexOf(vbNewLine) + 2)

                        'get Change Log
                        Dim strClURL As String = strUpdate.Substring(strUpdate.IndexOf("clURL: ") + 7, strUpdate.IndexOf(vbNewLine) - 7)
                        Dim strCl As String = wCl.DownloadString(strClURL)
                        uI.ChangeLog = strCl

                        strUpdate = strUpdate.Remove(0, strUpdate.IndexOf(vbNewLine) + 2)

                        Try
                            'get Image
                            Dim strImgURL As String = strUpdate.Substring(strUpdate.IndexOf("imgURL: ") + 8, strUpdate.IndexOf(vbNewLine) - 8)
                            uI.ImageURL = strImgURL

                            strUpdate = strUpdate.Remove(0, strUpdate.IndexOf(vbNewLine) + 2)

                            'get Image Link
                            Dim strImgURLLink As String = strUpdate.Substring(strUpdate.IndexOf("imgURLLink: ") + 12, strUpdate.IndexOf(vbNewLine) - 12)
                            uI.ImageURLLink = strImgURLLink
                        Catch ex As Exception
                            MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "Update Image Info could not be read." & vbNewLine & ex.Message, True)
                        End Try
                    Catch ex As Exception
                        uI.InfoOk = False
                    End Try
                Else
                    uI.InfoOk = False
                End If

                _curUI = uI
                Return uI
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "Getting update info failed" & vbNewLine & ex.Message, True)
                Return Nothing
            End Try
        End Function

        Public Function DownloadUpdate(ByVal dURL As String) As Boolean
            Try
                CreateWebClient()

                AddHandler wCl.DownloadProgressChanged, AddressOf DLProgressChanged
                AddHandler wCl.DownloadFileCompleted, AddressOf DLCompleted

                _curUI.UpdateLocation = My.Computer.FileSystem.SpecialDirectories.Temp & "\mRemote_Update.exe"
                wCl.DownloadFileAsync(New Uri(dURL), _curUI.UpdateLocation)

                Return True
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "Update download failed" & vbNewLine & ex.Message, True)
                Return False
            End Try
        End Function
#End Region

#Region "Private Methods"
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

        Private Function GetUpdateFile() As String
            Try
                CreateWebClient()

                Dim strTemp As String

                Try
                    strTemp = wCl.DownloadString(App.Info.Update.URL & App.Info.Update.File)
                Catch ex As Exception
                    strTemp = ""
                End Try

                Return strTemp
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "GetUpdateFile failed" & vbNewLine & ex.Message, True)
                Return ""
            End Try
        End Function

        Private Sub DLProgressChanged(ByVal sender As Object, ByVal e As System.Net.DownloadProgressChangedEventArgs)
            RaiseEvent DownloadProgressChanged(sender, e)
        End Sub

        Private Sub DLCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)
            Try
                Dim fInfo As New FileInfo(_curUI.UpdateLocation)

                If fInfo.Length > 0 Then
                    RaiseEvent DownloadCompleted(sender, e, True)
                Else
                    fInfo.Delete()
                    RaiseEvent DownloadCompleted(sender, e, False)
                End If
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "DLCompleted failed" & vbNewLine & ex.Message, True)
            End Try
        End Sub
#End Region

        Public Class Info
            Private _Version As Version
            Public Property Version() As Version
                Get
                    Return _Version
                End Get
                Set(ByVal value As Version)
                    _Version = value
                End Set
            End Property

            Private _DownloadUrl As String
            Public Property DownloadUrl() As String
                Get
                    Return _DownloadUrl
                End Get
                Set(ByVal value As String)
                    _DownloadUrl = value
                End Set
            End Property

            Private _UpdateLocation As String
            Public Property UpdateLocation() As String
                Get
                    Return _UpdateLocation
                End Get
                Set(ByVal value As String)
                    _UpdateLocation = value
                End Set
            End Property

            Private _ChangeLog As String
            Public Property ChangeLog() As String
                Get
                    Return _ChangeLog
                End Get
                Set(ByVal value As String)
                    _ChangeLog = value
                End Set
            End Property

            Private _ImageURL As String
            Public Property ImageURL() As String
                Get
                    Return _ImageURL
                End Get
                Set(ByVal value As String)
                    _ImageURL = value
                End Set
            End Property

            Private _ImageURLLink As String
            Public Property ImageURLLink() As String
                Get
                    Return _ImageURLLink
                End Get
                Set(ByVal value As String)
                    _ImageURLLink = value
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