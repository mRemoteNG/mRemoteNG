Imports System.Management
Imports System.Security.Principal
Imports System.Web
Imports Microsoft.Win32
Imports mRemote3G.App
Imports mRemote3G.Connection.Protocol
Imports mRemote3G.Connection.PuttySession
Imports mRemote3G.Messages

Namespace Config.Putty
    Public Class RegistryProvider
        Inherits Provider

#Region "Public Methods"

        Public Overrides Function GetSessionNames(Optional ByVal raw As Boolean = False) As String()
            Dim sessionsKey As RegistryKey = Registry.CurrentUser.OpenSubKey(PuttySessionsKey)
            If sessionsKey Is Nothing Then Return New String() {}

            Dim sessionNames As New List(Of String)
            For Each sessionName As String In sessionsKey.GetSubKeyNames()
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

            Return sessionNames.ToArray()
        End Function

        Public Overrides Function GetSession(sessionName As String) As PuttyInfo
            Dim sessionsKey As RegistryKey = Registry.CurrentUser.OpenSubKey(PuttySessionsKey)
            If sessionsKey Is Nothing Then Return Nothing

            Dim sessionKey As RegistryKey = sessionsKey.OpenSubKey(sessionName)
            If sessionKey Is Nothing Then Return Nothing

            sessionName = HttpUtility.UrlDecode(sessionName.Replace("+", "%2B"))

            Dim sessionInfo As New PuttyInfo
            With sessionInfo
                .PuttySession = sessionName
                .Name = sessionName
                .Hostname = sessionKey.GetValue("HostName")
                .Username = sessionKey.GetValue("UserName")
                Dim protocol As String = sessionKey.GetValue("Protocol")
                If protocol Is Nothing Then protocol = "ssh"
                Select Case protocol.ToLowerInvariant()
                    Case "raw"
                        .Protocol = Protocols.RAW
                    Case "rlogin"
                        .Protocol = Protocols.Rlogin
                    Case "serial"
                        Return Nothing
                    Case "ssh"
                        Dim sshVersionObject As Object = sessionKey.GetValue("SshProt")
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
                .Port = sessionKey.GetValue("PortNumber")
            End With

            Return sessionInfo
        End Function

        Public Overrides Sub StartWatcher()
            If _eventWatcher IsNot Nothing Then Return

            Try
                Dim currentUserSid As String = WindowsIdentity.GetCurrent().User.Value
                Dim key As String = String.Join("\", {currentUserSid, PuttySessionsKey}).Replace("\", "\\")
                Dim _
                    query As _
                        New WqlEventQuery(
                            String.Format(
                                "SELECT * FROM RegistryTreeChangeEvent WHERE Hive = 'HKEY_USERS' AND RootPath = '{0}'",
                                key))
                _eventWatcher = New ManagementEventWatcher(query)
                AddHandler _eventWatcher.EventArrived, AddressOf OnManagementEventArrived
                _eventWatcher.Start()
            Catch ex As Exception
                Runtime.MessageCollector.AddExceptionMessage("PuttySessions.Watcher.StartWatching() failed.", ex,
                                                             MessageClass.WarningMsg, True)
            End Try
        End Sub

        Public Overrides Sub StopWatcher()
            If _eventWatcher Is Nothing Then Return
            _eventWatcher.Stop()
            _eventWatcher.Dispose()
            _eventWatcher = Nothing
        End Sub

#End Region

#Region "Private Fields"

        Private Const PuttySessionsKey As String = "Software\SimonTatham\PuTTY\Sessions"
        Private Shared _eventWatcher As ManagementEventWatcher

#End Region

#Region "Private Methods"

        Private Sub OnManagementEventArrived(sender As Object, e As EventArrivedEventArgs)
            OnSessionChanged(New SessionChangedEventArgs())
        End Sub

#End Region
    End Class
End Namespace

