Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Web
Imports mRemote3G.App
Imports mRemote3G.App.Info
Imports mRemote3G.Connection.Protocol
Imports mRemote3G.Connection.PuttySession
Imports mRemote3G.Messages

Namespace Config.Putty
    Public Class XmingProvider
        Inherits Provider

#Region "Public Methods"

        Public Overrides Function GetSessionNames(Optional ByVal raw As Boolean = False) As String()
            Dim sessionsFolderPath As String = GetSessionsFolderPath()
            If Not Directory.Exists(sessionsFolderPath) Then Return New String() {}

            Dim sessionNames As New List(Of String)
            For Each sessionName As String In Directory.GetFiles(sessionsFolderPath)
                sessionName = Path.GetFileName(sessionName)
                If raw Then
                    sessionNames.Add(sessionName)
                Else
                    sessionNames.Add(HttpUtility.UrlDecode(sessionName.Replace("+", "%2B")))
                End If
            Next

            If raw Then
                If Not sessionNames.Contains("Default%20Settings") Then ' Do not localize
                    sessionNames.Insert(0, "Default%20Settings")
                End If
            Else
                If Not sessionNames.Contains("Default Settings") Then
                    sessionNames.Insert(0, "Default Settings")
                End If
            End If

            Dim registrySessionNames As New List(Of String)
            For Each sessionName As String In RegistryProvider.GetSessionNames(raw)
                registrySessionNames.Add(String.Format(RegistrySessionNameFormat, sessionName))
            Next

            sessionNames.AddRange(registrySessionNames)
            sessionNames.Sort()

            Return sessionNames.ToArray()
        End Function

        Public Overrides Function GetSession(sessionName As String) As PuttyInfo
            Dim registrySessionName As String = GetRegistrySessionName(sessionName)
            If Not String.IsNullOrEmpty(registrySessionName) Then
                Return ModifyRegistrySessionInfo(RegistryProvider.GetSession(registrySessionName))
            End If

            Dim sessionsFolderPath As String = GetSessionsFolderPath()
            If Not Directory.Exists(sessionsFolderPath) Then Return Nothing

            Dim sessionFile As String = Path.Combine(sessionsFolderPath, sessionName)
            If Not File.Exists(sessionFile) Then Return Nothing

            sessionName = HttpUtility.UrlDecode(sessionName.Replace("+", "%2B"))

            Dim sessionFileReader As New SessionFileReader(sessionFile)
            Dim sessionInfo As New PuttyInfo
            With sessionInfo
                .PuttySession = sessionName
                .Name = sessionName
                .Hostname = sessionFileReader.GetValue("HostName")
                .Username = sessionFileReader.GetValue("UserName")
                Dim protocol As String = sessionFileReader.GetValue("Protocol")
                If protocol Is Nothing Then protocol = "ssh"
                Select Case protocol.ToLowerInvariant()
                    Case "raw"
                        .Protocol = Protocols.RAW
                    Case "rlogin"
                        .Protocol = Protocols.Rlogin
                    Case "serial"
                        Return Nothing
                    Case "ssh"
                        Dim sshVersionObject As Object = sessionFileReader.GetValue("SshProt")
                        If sshVersionObject IsNot Nothing Then
                            Dim sshVersion = CType(sshVersionObject, Integer)
                            If sshVersion >= 2 Then
                                .Protocol = Protocols.SSH2
                            Else
                                .Protocol = Protocols.SSH1
                            End If
                        Else
                            .Protocol = Protocols.SSH2
                        End If
                    Case "telnet"
                        .Protocol = Protocols.Telnet
                    Case Else
                        Return Nothing
                End Select
                .Port = sessionFileReader.GetValue("PortNumber")
            End With

            Return sessionInfo
        End Function

        Public Overrides Sub StartWatcher()
            RegistryProvider.StartWatcher()
            AddHandler RegistryProvider.SessionChanged, AddressOf OnRegistrySessionChanged

            If _eventWatcher IsNot Nothing Then Return

            Dim sessDir = GetSessionsFolderPath()
            If Not Directory.Exists(sessDir) Then
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "XmingProvider: Could not find Session Dir. Cannot watch sessions.", True)
                Return
            End If

            Try
                _eventWatcher = New FileSystemWatcher(sessDir)
                _eventWatcher.NotifyFilter = (NotifyFilters.FileName Or NotifyFilters.LastWrite)
                AddHandler _eventWatcher.Changed, AddressOf OnFileSystemEventArrived
                AddHandler _eventWatcher.Created, AddressOf OnFileSystemEventArrived
                AddHandler _eventWatcher.Deleted, AddressOf OnFileSystemEventArrived
                AddHandler _eventWatcher.Renamed, AddressOf OnFileSystemEventArrived
                _eventWatcher.EnableRaisingEvents = True
            Catch ex As Exception
                Runtime.MessageCollector.AddExceptionMessage(
                    "XmingPortablePuttySessions.Watcher.StartWatching() failed.", ex, MessageClass.WarningMsg, True)
            End Try
        End Sub

        Public Overrides Sub StopWatcher()
            RegistryProvider.StopWatcher()
            RemoveHandler RegistryProvider.SessionChanged, AddressOf OnRegistrySessionChanged

            If _eventWatcher Is Nothing Then Return
            _eventWatcher.EnableRaisingEvents = False
            _eventWatcher.Dispose()
            _eventWatcher = Nothing
        End Sub

#End Region

#Region "Private Fields"

        Private Const RegistrySessionNameFormat As String = "{0} [registry]"
        Private Const RegistrySessionNamePattern As String = "(.*)\ \[registry\]"

        Private Shared ReadOnly RegistryProvider As New RegistryProvider
        Private Shared _eventWatcher As FileSystemWatcher

#End Region

#Region "Private Methods"

        Private Shared Function GetPuttyConfPath() As String
            Dim puttyPath As String
            If My.Settings.UseCustomPuttyPath Then
                puttyPath = My.Settings.CustomPuttyPath
            Else
                puttyPath = General.PuttyPath
            End If
            Return Path.Combine(Path.GetDirectoryName(puttyPath), "putty.conf")
        End Function

        Private Shared Function GetSessionsFolderPath() As String
            Dim puttyConfPath As String = GetPuttyConfPath()
            Dim sessionFileReader As New PuttyConfFileReader(puttyConfPath)
            Dim basePath As String = Environment.ExpandEnvironmentVariables(sessionFileReader.GetValue("sshk&sess"))
            Return Path.Combine(basePath, "sessions")
        End Function

        Private Shared Function GetRegistrySessionName(sessionName As String) As String
            Dim regex As New Regex(RegistrySessionNamePattern)

            Dim matches As MatchCollection = regex.Matches(sessionName)
            If matches.Count < 1 Then Return String.Empty

            Dim groups As GroupCollection = matches(0).Groups
            If groups.Count < 1 Then Return String.Empty _
            ' This should always include at least one item, but check anyway

            Return groups(1).Value
        End Function

        Private Shared Function ModifyRegistrySessionInfo(sessionInfo As PuttyInfo) As PuttyInfo
            sessionInfo.Name = String.Format(RegistrySessionNameFormat, sessionInfo.Name)
            sessionInfo.PuttySession = String.Format(RegistrySessionNameFormat, sessionInfo.PuttySession)
            Return sessionInfo
        End Function

        Private Sub OnFileSystemEventArrived(sender As Object, e As FileSystemEventArgs)
            OnSessionChanged(New SessionChangedEventArgs())
        End Sub

        Private Sub OnRegistrySessionChanged(sender As Object, e As SessionChangedEventArgs)
            OnSessionChanged(New SessionChangedEventArgs())
        End Sub

#End Region

#Region "Private Classes"

        Private Class PuttyConfFileReader
            Public Sub New(puttyConfFile As String)
                _puttyConfFile = puttyConfFile
            End Sub

            Private ReadOnly _puttyConfFile As String
            Private _configurationLoaded As Boolean = False
            Private ReadOnly _configuration As New Dictionary(Of String, String)

            Private Sub LoadConfiguration()
                _configurationLoaded = True
                Try
                    If Not File.Exists(_puttyConfFile) Then Return
                    Using streamReader As New StreamReader(_puttyConfFile)
                        Dim line As String
                        Do
                            line = streamReader.ReadLine()
                            If line Is Nothing Then Exit Do
                            line = line.Trim()
                            If line = String.Empty Then Continue Do ' Blank line
                            If line.Substring(0, 1) = ";" Then Continue Do ' Comment
                            Dim parts() As String = line.Split(New Char() {"="}, 2)
                            If parts.Length < 2 Then Continue Do
                            If _configuration.ContainsKey(parts(0)) Then Continue Do _
                            ' As per http://www.straightrunning.com/XmingNotes/portableputty.php only first entry is used
                            _configuration.Add(parts(0), parts(1))
                        Loop
                    End Using
                Catch ex As Exception
                    Runtime.MessageCollector.AddExceptionMessage("PuttyConfFileReader.LoadConfiguration() failed.", ex,
                                                                 MessageClass.ErrorMsg, True)
                End Try
            End Sub

            Public Function GetValue(setting As String) As String
                If Not _configurationLoaded Then LoadConfiguration()
                If Not _configuration.ContainsKey(setting) Then Return String.Empty
                Return _configuration(setting)
            End Function
        End Class

        Private Class SessionFileReader
            Public Sub New(sessionFile As String)
                _sessionFile = sessionFile
            End Sub

            Private ReadOnly _sessionFile As String
            Private _sessionInfoLoaded As Boolean = False
            Private ReadOnly _sessionInfo As New Dictionary(Of String, String)

            Private Sub LoadSessionInfo()
                _sessionInfoLoaded = True
                Try
                    If Not File.Exists(_sessionFile) Then Return
                    Using streamReader As New StreamReader(_sessionFile)
                        Dim line As String
                        Do
                            line = streamReader.ReadLine()
                            If line Is Nothing Then Exit Do
                            Dim parts() As String = line.Split(New Char() {"\"})
                            If parts.Length < 2 Then Continue Do
                            _sessionInfo.Add(parts(0), parts(1))
                        Loop
                    End Using
                Catch ex As Exception
                    Runtime.MessageCollector.AddExceptionMessage("SessionFileReader.LoadSessionInfo() failed.", ex,
                                                                 MessageClass.ErrorMsg, True)
                End Try
            End Sub

            Public Function GetValue(setting As String) As String
                If Not _sessionInfoLoaded Then LoadSessionInfo()
                If Not _sessionInfo.ContainsKey(setting) Then Return String.Empty
                Return _sessionInfo(setting)
            End Function
        End Class

#End Region
    End Class
End Namespace
