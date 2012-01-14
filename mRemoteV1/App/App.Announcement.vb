Imports System.Net
Imports mRemoteNG.App.Runtime

Namespace App
    Public Class Announcement
        Implements IDisposable

#Region "Private Variables"
        Private webClient As WebClient
        Private webProxy As WebProxy
#End Region

#Region "Public Properties"
        Private _currentAnnouncementInfo As Info
        Public ReadOnly Property CurrentAnnouncementInfo() As Info
            Get
                Return _currentAnnouncementInfo
            End Get
        End Property
#End Region

#Region "Public Methods"
        Public Function IsAnnouncementAvailable() As Boolean
            Try
                Dim aI As Info = GetAnnouncementInfo()

                If aI.InfoOk = False Then
                    Return False
                End If

                If aI.Name <> My.Settings.LastAnnouncement Then
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "IsAnnouncementAvailable failed" & vbNewLine & ex.Message, True)
                Return False
            End Try
        End Function

        Public Function GetAnnouncementInfo() As Info
            Try
                Dim strAnnouncement As String = GetAnnouncementFile()

                CreateWebClient()

                Dim aI As New Info()

                If strAnnouncement <> "" Then
                    aI.InfoOk = True

                    Try
                        'get Name
                        Dim strName As String = strAnnouncement.Substring(strAnnouncement.IndexOf("Name: ") + 6, strAnnouncement.IndexOf(vbNewLine) - 6)
                        aI.Name = strName

                        strAnnouncement = strAnnouncement.Remove(0, strAnnouncement.IndexOf(vbNewLine) + 2)

                        'get Download URL
                        Dim strU As String = ""

                        strU = strAnnouncement.Substring(strAnnouncement.IndexOf("URL: ") + 5, strAnnouncement.IndexOf(vbNewLine) - 5)

                        aI.Url = strU
                    Catch ex As Exception
                        aI.InfoOk = False
                    End Try
                Else
                    aI.InfoOk = False
                End If

                _currentAnnouncementInfo = aI
                Return aI
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "Getting Announcement info failed" & vbNewLine & ex.Message, True)
                Return Nothing
            End Try
        End Function

        Private Function GetAnnouncementFile() As String
            Try
                CreateWebClient()

                Dim strTemp As String

                Try
                    strTemp = webClient.DownloadString(App.Info.General.URLAnnouncement)
                Catch ex As Exception
                    strTemp = ""
                End Try

                Return strTemp
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "GetAnnouncementFile failed" & vbNewLine & ex.Message, True)
                Return ""
            End Try
        End Function

        Private Sub CreateWebClient()
            webClient = New WebClient()

            If My.Settings.UpdateUseProxy Then
                webProxy = New WebProxy(My.Settings.UpdateProxyAddress, My.Settings.UpdateProxyPort)

                If My.Settings.UpdateProxyUseAuthentication Then
                    Dim cred As ICredentials
                    cred = New NetworkCredential(My.Settings.UpdateProxyAuthUser, Security.Crypt.Decrypt(My.Settings.UpdateProxyAuthPass, App.Info.General.EncryptionKey))

                    webProxy.Credentials = cred
                End If

                webClient.Proxy = webProxy
            End If
        End Sub
#End Region

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

            Private _url As String
            Public Property Url() As String
                Get
                    Return _url
                End Get
                Set(ByVal value As String)
                    _url = value
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

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    If webClient IsNot Nothing Then webClient.Dispose()
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class
End Namespace