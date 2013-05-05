Imports System.ComponentModel
Imports Microsoft.Win32
Imports mRemoteNG.Connection.Protocol
Imports mRemoteNG.Tree
Imports mRemoteNG.My

Namespace Config
    Public Class PuttySessions
        Private Const PuttySessionsKey As String = "Software\SimonTatham\PuTTY\Sessions"

        Public Shared Sub AddSessionsToTree(ByVal treeView As TreeView)
            Dim savedSessions() As Connection.Info = LoadSessions()
            If savedSessions Is Nothing OrElse savedSessions.Length = 0 Then Return

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

            Dim puttyRootNode As TreeNode = New TreeNode
            puttyRootNode.Text = puttyRootInfo.Name
            puttyRootNode.Tag = puttyRootInfo
            puttyRootNode.ImageIndex = Images.Enums.TreeImage.PuttySessions
            puttyRootNode.SelectedImageIndex = Images.Enums.TreeImage.PuttySessions

            puttyRootInfo.TreeNode = puttyRootNode

            treeView.BeginUpdate()
            treeView.Nodes.Add(puttyRootNode)

            Dim newTreeNode As TreeNode
            For Each sessionInfo As Connection.PuttySession.Info In savedSessions
                newTreeNode = Node.AddNode(Node.Type.PuttySession, sessionInfo.Name)
                If newTreeNode Is Nothing Then Continue For

                sessionInfo.RootPuttySessionsInfo = puttyRootInfo
                sessionInfo.TreeNode = newTreeNode
                sessionInfo.Inherit.TurnOffInheritanceCompletely()

                newTreeNode.Tag = sessionInfo
                newTreeNode.ImageIndex = Images.Enums.TreeImage.ConnectionClosed
                newTreeNode.SelectedImageIndex = Images.Enums.TreeImage.ConnectionClosed

                puttyRootNode.Nodes.Add(newTreeNode)
            Next

            puttyRootNode.Expand()
            treeView.EndUpdate()
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
                If sessionName = "Default%20Settings" Then Continue For ' Do not localize
                sessionInfo = SessionToConnectionInfo(sessionName)
                If sessionInfo Is Nothing Then Continue For
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
                If protocol Is Nothing Then Return Nothing
                Select Case protocol.ToLowerInvariant()
                    Case "raw"
                        .Protocol = Protocols.RAW
                    Case "rlogin"
                        .Protocol = Protocols.Rlogin
                    Case "serial"
                        Return Nothing
                    Case "ssh"
                        Dim sshVersion As Integer = sessionKey.GetValue("SshProt")
                        If sshVersion >= 2 Then
                            .Protocol = Protocols.SSH2
                        Else
                            .Protocol = Protocols.SSH1
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
