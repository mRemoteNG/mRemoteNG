Imports System.ComponentModel
Imports System.Management
Imports mRemoteNG.Messages
Imports Microsoft.Win32
Imports mRemoteNG.Connection.Protocol
Imports mRemoteNG.My
Imports mRemoteNG.App.Runtime
Imports System.Security.Principal

Namespace Config
    Public Class PuttySessions
        Private Const PuttySessionsKey As String = "Software\SimonTatham\PuTTY\Sessions"
        Private Shared _rootTreeNode As TreeNode
        Private Shared _eventWatcher As ManagementEventWatcher

        Private Delegate Sub AddSessionsToTreeDelegate()
        Public Shared Sub AddSessionsToTree()
            Dim treeView As TreeView = Tree.Node.TreeView
            If treeView Is Nothing Then Return
            If treeView.InvokeRequired Then
                treeView.Invoke(New AddSessionsToTreeDelegate(AddressOf AddSessionsToTree))
                Return
            End If

            Dim savedSessions As New List(Of Connection.Info)(LoadSessions())
            If savedSessions Is Nothing OrElse savedSessions.Count = 0 Then Return

            Dim puttyRootInfo As New Root.PuttySessions.Info()
            If String.IsNullOrEmpty(My.Settings.PuttySavedSessionsName) Then
                puttyRootInfo.Name = Language.strPuttySavedSessionsRootName
            Else
                puttyRootInfo.Name = My.Settings.PuttySavedSessionsName
            End If
            If String.IsNullOrEmpty(My.Settings.PuttySavedSessionsPanel) Then
                puttyRootInfo.Panel = Language.strGeneral
            Else
                puttyRootInfo.Panel = My.Settings.PuttySavedSessionsPanel
            End If

            Dim inUpdate As Boolean = False

            If _rootTreeNode Is Nothing Then
                _rootTreeNode = New TreeNode
                _rootTreeNode.Name = puttyRootInfo.Name
                _rootTreeNode.Text = puttyRootInfo.Name
                _rootTreeNode.Tag = puttyRootInfo
                _rootTreeNode.ImageIndex = Images.Enums.TreeImage.PuttySessions
                _rootTreeNode.SelectedImageIndex = Images.Enums.TreeImage.PuttySessions
            End If

            If Not treeView.Nodes.Contains(_rootTreeNode) Then
                If Not inUpdate Then
                    treeView.BeginUpdate()
                    inUpdate = True
                End If
                treeView.Nodes.Add(_rootTreeNode)
            End If

            puttyRootInfo.TreeNode = _rootTreeNode

            Dim newTreeNodes As New List(Of TreeNode)
            For Each sessionInfo As Connection.PuttySession.Info In savedSessions
                Dim treeNode As TreeNode
                Dim isNewNode As Boolean
                If _rootTreeNode.Nodes.ContainsKey(sessionInfo.Name) Then
                    treeNode = _rootTreeNode.Nodes.Item(sessionInfo.Name)
                    isNewNode = False
                Else
                    treeNode = Tree.Node.AddNode(Tree.Node.Type.PuttySession, sessionInfo.Name)
                    If treeNode Is Nothing Then Continue For
                    treeNode.Name = treeNode.Text
                    treeNode.ImageIndex = Images.Enums.TreeImage.ConnectionClosed
                    treeNode.SelectedImageIndex = Images.Enums.TreeImage.ConnectionClosed
                    isNewNode = True
                End If

                sessionInfo.RootPuttySessionsInfo = puttyRootInfo
                sessionInfo.TreeNode = treeNode
                sessionInfo.Inherit.TurnOffInheritanceCompletely()

                treeNode.Tag = sessionInfo

                If isNewNode Then newTreeNodes.Add(treeNode)
            Next

            For Each treeNode As TreeNode In _rootTreeNode.Nodes
                If Not savedSessions.Contains(treeNode.Tag) Then
                    If Not inUpdate Then
                        treeView.BeginUpdate()
                        inUpdate = True
                    End If
                    _rootTreeNode.Nodes.Remove(treeNode)
                End If
            Next

            If Not newTreeNodes.Count = 0 Then
                If Not inUpdate Then
                    treeView.BeginUpdate()
                    inUpdate = True
                End If
                _rootTreeNode.Nodes.AddRange(newTreeNodes.ToArray())
            End If

            If inUpdate Then
                Tree.Node.Sort(_rootTreeNode, SortOrder.Ascending)
                _rootTreeNode.Expand()
                treeView.EndUpdate()
            End If
        End Sub

        Protected Shared Function GetSessionNames(Optional ByVal raw As Boolean = False) As String()
            Dim sessionsKey As RegistryKey = Registry.CurrentUser.OpenSubKey(PuttySessionsKey)
            If sessionsKey Is Nothing Then Return New String() {}

            Dim sessionNames As New List(Of String)
            For Each sessionName As String In sessionsKey.GetSubKeyNames()
                If raw Then
                    sessionNames.Add(sessionName)
                Else
                    sessionNames.Add(Web.HttpUtility.UrlDecode(sessionName.Replace("+", "%2B")))
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

        Protected Shared Function LoadSessions() As Connection.PuttySession.Info()
            Dim sessionList As New List(Of Connection.PuttySession.Info)
            Dim sessionInfo As Connection.Info
            For Each sessionName As String In GetSessionNames(True)
                sessionInfo = SessionToConnectionInfo(sessionName)
                If sessionInfo Is Nothing OrElse String.IsNullOrEmpty(sessionInfo.Hostname) Then Continue For
                sessionList.Add(sessionInfo)
            Next
            Return sessionList.ToArray()
        End Function

        Protected Shared Function SessionToConnectionInfo(ByVal sessionName As String) As Connection.PuttySession.Info
            Dim sessionsKey As RegistryKey = Registry.CurrentUser.OpenSubKey(PuttySessionsKey)
            If sessionsKey Is Nothing Then Return Nothing

            Dim sessionKey As RegistryKey = sessionsKey.OpenSubKey(sessionName)
            If sessionKey Is Nothing Then Return Nothing

            sessionName = Web.HttpUtility.UrlDecode(sessionName.Replace("+", "%2B"))

            Dim sessionInfo As New Connection.PuttySession.Info
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
                            Dim sshVersion As Integer = CType(sshVersionObject, Integer)
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

        Public Shared Sub StartWatcher()
            If _eventWatcher IsNot Nothing Then Return

            Try
                Dim currentUserSid As String = WindowsIdentity.GetCurrent().User.Value
                Dim key As String = String.Join("\", {currentUserSid, PuttySessionsKey}).Replace("\", "\\")
                Dim query As New WqlEventQuery(String.Format("SELECT * FROM RegistryTreeChangeEvent WHERE Hive = 'HKEY_USERS' AND RootPath = '{0}'", key))
                _eventWatcher = New ManagementEventWatcher(query)
                AddHandler _eventWatcher.EventArrived, AddressOf OnManagementEventArrived
                _eventWatcher.Start()
            Catch ex As Exception
                MessageCollector.AddExceptionMessage("PuttySessions.Watcher.StartWatching() failed.", ex, MessageClass.WarningMsg, True)
            End Try
        End Sub

        Public Shared Sub StopWatcher()
            If _eventWatcher Is Nothing Then Return
            _eventWatcher.Stop()
            _eventWatcher.Dispose()
            _eventWatcher = Nothing
        End Sub

        Private Shared Sub OnManagementEventArrived(ByVal sender As Object, ByVal e As EventArrivedEventArgs)
            AddSessionsToTree()
        End Sub

        Public Class SessionList
            Inherits StringConverter

            Public Shared ReadOnly Property Names() As String()
                Get
                    Return GetSessionNames()
                End Get
            End Property

            Public Overloads Overrides Function GetStandardValues(ByVal context As System.ComponentModel.ITypeDescriptorContext) As System.ComponentModel.TypeConverter.StandardValuesCollection
                Return New StandardValuesCollection(Names)
            End Function

            Public Overloads Overrides Function GetStandardValuesExclusive(ByVal context As System.ComponentModel.ITypeDescriptorContext) As Boolean
                Return True
            End Function

            Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As ITypeDescriptorContext) As Boolean
                Return True
            End Function
        End Class
    End Class
End Namespace
