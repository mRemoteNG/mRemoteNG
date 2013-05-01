Imports System.IO
Imports System.Net
Imports System.ComponentModel
Imports System.Threading
Imports mRemoteNG.Tools
Imports System.Reflection

Namespace App
    Public Class Update
#Region "Events"
        Public Event GetUpdateInfoCompletedEvent As AsyncCompletedEventHandler
        Public Event GetChangeLogCompletedEvent As AsyncCompletedEventHandler
        Public Event GetAnnouncementInfoCompletedEvent As AsyncCompletedEventHandler
        Public Event DownloadUpdateProgressChangedEvent As DownloadProgressChangedEventHandler
        Public Event DownloadUpdateCompletedEvent As AsyncCompletedEventHandler
#End Region

#Region "Public Properties"
        Private _currentUpdateInfo As UpdateInfo
        Public ReadOnly Property CurrentUpdateInfo() As UpdateInfo
            Get
                Return _currentUpdateInfo
            End Get
        End Property

        Private _changeLog As String
        Public ReadOnly Property ChangeLog() As String
            Get
                Return _changeLog
            End Get
        End Property

        Private _currentAnnouncementInfo As AnnouncementInfo
        Public ReadOnly Property CurrentAnnouncementInfo() As AnnouncementInfo
            Get
                Return _currentAnnouncementInfo
            End Get
        End Property

        Public ReadOnly Property IsGetUpdateInfoRunning() As Boolean
            Get
                If _getUpdateInfoThread IsNot Nothing Then
                    If _getUpdateInfoThread.IsAlive Then Return True
                End If
                Return False
            End Get
        End Property

        Public ReadOnly Property IsGetChangeLogRunning() As Boolean
            Get
                If _getChangeLogThread IsNot Nothing Then
                    If _getChangeLogThread.IsAlive Then Return True
                End If
                Return False
            End Get
        End Property

        Public ReadOnly Property IsGetAnnouncementInfoRunning() As Boolean
            Get
                If _getAnnouncementInfoThread IsNot Nothing Then
                    If _getAnnouncementInfoThread.IsAlive Then Return True
                End If
                Return False
            End Get
        End Property

        Public ReadOnly Property IsDownloadUpdateRunning() As Boolean
            Get
                Return (_downloadUpdateWebClient IsNot Nothing)
            End Get
        End Property
#End Region

#Region "Public Methods"
        Public Sub New()
            SetProxySettings()
        End Sub

        Public Sub SetProxySettings()
            SetProxySettings(My.Settings.UpdateUseProxy, My.Settings.UpdateProxyAddress, My.Settings.UpdateProxyPort, My.Settings.UpdateProxyUseAuthentication, My.Settings.UpdateProxyAuthUser, Security.Crypt.Decrypt(My.Settings.UpdateProxyAuthPass, Info.General.EncryptionKey))
        End Sub

        Public Sub SetProxySettings(ByVal useProxy As Boolean, ByVal address As String, ByVal port As Integer, ByVal useAuthentication As Boolean, ByVal username As String, ByVal password As String)
            If useProxy And Not String.IsNullOrEmpty(address) Then
                If Not port = 0 Then
                    _webProxy = New WebProxy(address, port)
                Else
                    _webProxy = New WebProxy(address)
                End If

                If useAuthentication Then
                    _webProxy.Credentials = New NetworkCredential(username, password)
                Else
                    _webProxy.Credentials = Nothing
                End If
            Else
                _webProxy = Nothing
            End If
        End Sub

        Public Function IsUpdateAvailable() As Boolean
            If _currentUpdateInfo Is Nothing OrElse Not _currentUpdateInfo.IsValid Then Return False

            Return _currentUpdateInfo.Version > My.Application.Info.Version
        End Function

        Public Function IsAnnouncementAvailable() As Boolean
            If _currentAnnouncementInfo Is Nothing OrElse _
                (Not _currentAnnouncementInfo.IsValid Or _
                 String.IsNullOrEmpty(_currentAnnouncementInfo.Name)) Then Return False

            Return (Not _currentAnnouncementInfo.Name = My.Settings.LastAnnouncement)
        End Function

        Public Sub GetUpdateInfoAsync()
            If IsGetUpdateInfoRunning Then _getUpdateInfoThread.Abort()

            _getUpdateInfoThread = New Thread(AddressOf GetUpdateInfo)
            With _getUpdateInfoThread
                .SetApartmentState(ApartmentState.STA)
                .IsBackground = True
                .Start()
            End With
        End Sub

        Public Sub GetChangeLogAsync()
            If _currentUpdateInfo Is Nothing OrElse Not _currentUpdateInfo.IsValid Then
                Throw New InvalidOperationException("CurrentUpdateInfo is not valid. GetUpdateInfoAsync() must be called before calling GetChangeLogAsync().")
            End If

            If IsGetChangeLogRunning Then _getChangeLogThread.Abort()

            _getChangeLogThread = New Thread(AddressOf GetChangeLog)
            With _getChangeLogThread
                .SetApartmentState(ApartmentState.STA)
                .IsBackground = True
                .Start()
            End With
        End Sub

        Public Sub GetAnnouncementInfoAsync()
            If IsGetAnnouncementInfoRunning Then _getAnnouncementInfoThread.Abort()

            _getAnnouncementInfoThread = New Thread(AddressOf GetAnnouncementInfo)
            With _getAnnouncementInfoThread
                .SetApartmentState(ApartmentState.STA)
                .IsBackground = True
                .Start()
            End With
        End Sub

        Public Sub DownloadUpdateAsync()
            If _downloadUpdateWebClient IsNot Nothing Then
                Throw New InvalidOperationException("A previous call to DownloadUpdateAsync() is still in progress.")
            End If

            If _currentUpdateInfo Is Nothing OrElse Not _currentUpdateInfo.IsValid Then
                Throw New InvalidOperationException("CurrentUpdateInfo is not valid. GetUpdateInfoAsync() must be called before calling DownloadUpdateAsync().")
            End If

            _currentUpdateInfo.UpdateFilePath = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(Path.GetRandomFileName, "exe"))
            DownloadUpdateWebClient.DownloadFileAsync(CurrentUpdateInfo.DownloadAddress, _currentUpdateInfo.UpdateFilePath)
        End Sub
#End Region

#Region "Private Properties"
        Private _downloadUpdateWebClient As WebClient
        Private ReadOnly Property DownloadUpdateWebClient() As WebClient
            Get
                If _downloadUpdateWebClient IsNot Nothing Then Return _downloadUpdateWebClient

                _downloadUpdateWebClient = CreateWebClient()

                AddHandler _downloadUpdateWebClient.DownloadProgressChanged, AddressOf DownloadUpdateProgressChanged
                AddHandler _downloadUpdateWebClient.DownloadFileCompleted, AddressOf DownloadUpdateCompleted

                Return _downloadUpdateWebClient
            End Get
        End Property
#End Region

#Region "Private Fields"
        Private _webProxy As WebProxy
        Private _getUpdateInfoThread As Thread
        Private _getChangeLogThread As Thread
        Private _getAnnouncementInfoThread As Thread
#End Region

#Region "Private Methods"
        Private Function CreateWebClient() As WebClient
            Dim webClient As New WebClient
            webClient.Headers.Add("user-agent", Info.General.UserAgent)
            webClient.Proxy = _webProxy
            Return webClient
        End Function

        Private Shared Function NewDownloadStringCompletedEventArgs(ByVal result As String, ByVal exception As Exception, ByVal cancelled As Boolean, ByVal userToken As Object) As DownloadStringCompletedEventArgs
            Dim type As Type = GetType(DownloadStringCompletedEventArgs)
            Const bindingFlags As BindingFlags = bindingFlags.NonPublic Or bindingFlags.Instance
            Dim argumentTypes() As Type = {GetType(String), GetType(Exception), GetType(Boolean), GetType(Object)}
            Dim constructor As ConstructorInfo = type.GetConstructor(bindingFlags, Nothing, argumentTypes, Nothing)
            Dim arguments() As Object = {result, exception, cancelled, userToken}

            Return constructor.Invoke(arguments)
        End Function

        Private Function DownloadString(ByVal address As Uri) As DownloadStringCompletedEventArgs
            Dim webClient As WebClient = CreateWebClient()
            Dim result As String = String.Empty
            Dim exception As Exception = Nothing
            Dim cancelled As Boolean = False

            Try
                result = webClient.DownloadString(address)
            Catch ex As ThreadAbortException
                cancelled = True
            Catch ex As Exception
                exception = ex
            End Try

            Return NewDownloadStringCompletedEventArgs(result, exception, cancelled, Nothing)
        End Function

        Private Sub GetUpdateInfo()
            Dim updateFileUri As New Uri(New Uri(My.Settings.UpdateAddress), New Uri(Info.Update.FileName, UriKind.Relative))
            Dim e As DownloadStringCompletedEventArgs = DownloadString(updateFileUri)

            If Not e.Cancelled And e.Error Is Nothing Then
                _currentUpdateInfo = UpdateInfo.FromString(e.Result)

                My.Settings.CheckForUpdatesLastCheck = Date.UtcNow
                If Not My.Settings.UpdatePending Then
                    My.Settings.UpdatePending = IsUpdateAvailable()
                End If
            End If

            RaiseEvent GetUpdateInfoCompletedEvent(Me, e)
        End Sub

        Private Sub GetChangeLog()
            Dim e As DownloadStringCompletedEventArgs = DownloadString(_currentUpdateInfo.ChangeLogAddress)

            If Not e.Cancelled And e.Error Is Nothing Then _changeLog = e.Result

            RaiseEvent GetChangeLogCompletedEvent(Me, e)
        End Sub

        Private Sub GetAnnouncementInfo()
            Dim announcementFileUri As New Uri(My.Settings.AnnouncementAddress)
            Dim e As DownloadStringCompletedEventArgs = DownloadString(announcementFileUri)

            If Not e.Cancelled And e.Error Is Nothing Then
                _currentAnnouncementInfo = AnnouncementInfo.FromString(e.Result)

                If Not String.IsNullOrEmpty(_currentAnnouncementInfo.Name) Then
                    My.Settings.LastAnnouncement = _currentAnnouncementInfo.Name
                End If
            End If

            RaiseEvent GetAnnouncementInfoCompletedEvent(Me, e)
        End Sub

        Private Sub DownloadUpdateProgressChanged(ByVal sender As Object, ByVal e As DownloadProgressChangedEventArgs)
            RaiseEvent DownloadUpdateProgressChangedEvent(sender, e)
        End Sub

        Private Sub DownloadUpdateCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)
            Dim raiseEventArgs As AsyncCompletedEventArgs = e

            If Not e.Cancelled And e.Error Is Nothing Then
                Try
                    Dim updateAuthenticode As New Authenticode(_currentUpdateInfo.UpdateFilePath)
                    With updateAuthenticode
                        .RequireThumbprintMatch = True
                        .ThumbprintToMatch = _currentUpdateInfo.CertificateThumbprint

                        If Not .Verify() = Authenticode.StatusValue.Verified Then
                            If .Status = Authenticode.StatusValue.UnhandledException Then
                                Throw .Exception
                            Else
                                Throw New Exception(.StatusMessage)
                            End If
                        End If
                    End With
                Catch ex As Exception
                    raiseEventArgs = New AsyncCompletedEventArgs(ex, False, Nothing)
                End Try
            End If

            If raiseEventArgs.Cancelled Or raiseEventArgs.Error IsNot Nothing Then
                File.Delete(_currentUpdateInfo.UpdateFilePath)
            End If

            RaiseEvent DownloadUpdateCompletedEvent(Me, raiseEventArgs)

            _downloadUpdateWebClient.Dispose()
            _downloadUpdateWebClient = Nothing
        End Sub
#End Region

#Region "Public Classes"
        Public Class UpdateInfo
#Region "Public Properties"
            Public Property IsValid As Boolean
            Public Property Version As Version
            Public Property DownloadAddress As Uri
            Public Property UpdateFilePath As String
            Public Property ChangeLogAddress As Uri
            Public Property ImageAddress As Uri
            Public Property ImageLinkAddress As Uri
            Public Property CertificateThumbprint As String
#End Region

#Region "Public Methods"
            Public Shared Function FromString(ByVal input As String) As UpdateInfo
                Dim newInfo As New UpdateInfo
                With newInfo
                    If String.IsNullOrEmpty(input) Then
                        .IsValid = False
                    Else
                        Dim updateFile As New UpdateFile(input)
                        .Version = updateFile.GetVersion("Version")
                        .DownloadAddress = updateFile.GetUri("dURL")
                        .ChangeLogAddress = updateFile.GetUri("clURL")
                        .ImageAddress = updateFile.GetUri("imgURL")
                        .ImageLinkAddress = updateFile.GetUri("imgURLLink")
                        .CertificateThumbprint = updateFile.GetThumbprint("CertificateThumbprint")
                        .IsValid = True
                    End If
                End With
                Return newInfo
            End Function
#End Region
        End Class

        Public Class AnnouncementInfo
#Region "Public Properties"
            Public Property IsValid As Boolean
            Public Property Name As String
            Public Property Address As Uri
#End Region

#Region "Public Methods"
            Public Shared Function FromString(ByVal input As String) As AnnouncementInfo
                Dim newInfo As New AnnouncementInfo
                With newInfo
                    If String.IsNullOrEmpty(input) Then
                        .IsValid = False
                    Else
                        Dim updateFile As New UpdateFile(input)
                        .Name = updateFile.GetString("Name")
                        .Address = updateFile.GetUri("URL")
                        .IsValid = True
                    End If
                End With
                Return newInfo
            End Function
#End Region
        End Class
#End Region

#Region "Private Classes"
        Private Class UpdateFile
#Region "Public Properties"
            Private ReadOnly _items As New Dictionary(Of String, String)(StringComparer.InvariantCultureIgnoreCase)
            ' ReSharper disable MemberCanBePrivate.Local
            Public ReadOnly Property Items() As Dictionary(Of String, String)
                ' ReSharper restore MemberCanBePrivate.Local
                Get
                    Return _items
                End Get
            End Property
#End Region

#Region "Public Methods"
            Public Sub New(ByVal content As String)
                FromString(content)
            End Sub

            ' ReSharper disable MemberCanBePrivate.Local
            Public Sub FromString(ByVal content As String)
                ' ReSharper restore MemberCanBePrivate.Local
                If String.IsNullOrEmpty(content) Then
                Else
                    Dim lineSeparators() As Char = New Char() {Chr(&HA), Chr(&HD)}
                    Dim keyValueSeparators() As Char = New Char() {":", "="}
                    Dim commentCharacters() As Char = New Char() {"#", ";", "'"}

                    Dim lines() As String = content.Split(lineSeparators, StringSplitOptions.RemoveEmptyEntries)
                    For Each line As String In lines
                        line = line.Trim()
                        If line.Length = 0 Then Continue For
                        If Not line.Substring(0, 1).IndexOfAny(commentCharacters) = -1 Then Continue For

                        Dim parts() As String = line.Split(keyValueSeparators, 2)
                        If Not parts.Length = 2 Then Continue For
                        Dim key As String = parts(0).Trim()
                        Dim value As String = parts(1).Trim()

                        _items.Add(key, value)
                    Next
                End If
            End Sub

            ' ReSharper disable MemberCanBePrivate.Local
            Public Function GetString(ByVal key As String) As String
                ' ReSharper restore MemberCanBePrivate.Local
                If Not Items.ContainsKey(key) Then Return String.Empty
                Return Items(key)
            End Function

            Public Function GetVersion(ByVal key As String) As Version
                Dim value As String = GetString(key)
                If String.IsNullOrEmpty(value) Then Return Nothing
                Return New Version(value)
            End Function

            Public Function GetUri(ByVal key As String) As Uri
                Dim value As String = GetString(key)
                If String.IsNullOrEmpty(value) Then Return Nothing
                Return New Uri(value)
            End Function

            Public Function GetThumbprint(ByVal key As String) As String
                Return GetString(key).Replace(" ", "").ToUpperInvariant()
            End Function
#End Region
        End Class
#End Region
    End Class
End Namespace