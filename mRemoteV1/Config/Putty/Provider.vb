Imports mRemoteNG.My

Namespace Config.Putty
    Public MustInherit Class Provider
        Private _rootTreeNode As TreeNode
        Public ReadOnly Property RootTreeNode As TreeNode
            Get
                If _rootTreeNode Is Nothing Then _rootTreeNode = CreateRootTreeNode()
                Return _rootTreeNode
            End Get
        End Property

        Private Delegate Function CreateRootTreeNodeDelegate() As TreeNode
        Protected Overridable Function CreateRootTreeNode() As TreeNode
            Dim treeView As TreeView = Tree.Node.TreeView
            If treeView Is Nothing Then Return Nothing
            If treeView.InvokeRequired Then
                Return treeView.Invoke(New CreateRootTreeNodeDelegate(AddressOf CreateRootTreeNode))
            End If

            Dim newTreeNode As New TreeNode
            RootInfo.TreeNode = newTreeNode

            newTreeNode.Name = _rootInfo.Name
            newTreeNode.Text = _rootInfo.Name
            newTreeNode.Tag = _rootInfo
            newTreeNode.ImageIndex = Images.Enums.TreeImage.PuttySessions
            newTreeNode.SelectedImageIndex = Images.Enums.TreeImage.PuttySessions

            Return newTreeNode
        End Function

        Private _rootInfo As Root.PuttySessions.Info
        Public ReadOnly Property RootInfo() As Root.PuttySessions.Info
            Get
                If _rootInfo Is Nothing Then _rootInfo = CreateRootInfo()
                Return _rootInfo
            End Get
        End Property

        Protected Overridable Function CreateRootInfo() As Root.PuttySessions.Info
            Dim newRootInfo As New Root.PuttySessions.Info

            If String.IsNullOrEmpty(My.Settings.PuttySavedSessionsName) Then
                newRootInfo.Name = Language.strPuttySavedSessionsRootName
            Else
                newRootInfo.Name = My.Settings.PuttySavedSessionsName
            End If

            If String.IsNullOrEmpty(My.Settings.PuttySavedSessionsPanel) Then
                newRootInfo.Panel = Language.strGeneral
            Else
                newRootInfo.Panel = My.Settings.PuttySavedSessionsPanel
            End If

            Return newRootInfo
        End Function

        Public MustOverride Function GetSessionNames(Optional ByVal raw As Boolean = False) As String()
        Public MustOverride Function GetSession(ByVal sessionName As String) As Connection.PuttySession.Info

        Public Overridable Function GetSessions() As Connection.PuttySession.Info()
            Dim sessionList As New List(Of Connection.PuttySession.Info)
            Dim sessionInfo As Connection.Info
            For Each sessionName As String In GetSessionNames(True)
                sessionInfo = GetSession(sessionName)
                If sessionInfo Is Nothing OrElse String.IsNullOrEmpty(sessionInfo.Hostname) Then Continue For
                sessionList.Add(sessionInfo)
            Next
            Return sessionList.ToArray()
        End Function

        Public Overridable Sub StartWatcher()

        End Sub

        Public Overridable Sub StopWatcher()

        End Sub

        Public Event SessionChanged(ByVal sender As Object, ByVal e As SessionChangedEventArgs)

        Protected Overridable Sub OnSessionChanged(ByVal e As SessionChangedEventArgs)
            RaiseEvent SessionChanged(Me, New SessionChangedEventArgs())
        End Sub

        Public Class SessionChangedEventArgs
            Inherits EventArgs
        End Class
    End Class
End Namespace
