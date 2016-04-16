
Imports mRemote3G.Connection
Imports mRemote3G.Connection.PuttySession
Imports mRemote3G.Images
Imports mRemote3G.Root.PuttySessions
Imports mRemote3G.Tree

Namespace Config.Putty
    Public MustInherit Class Provider

#Region "Public Methods"

        Private _rootTreeNode As TreeNode

        Public ReadOnly Property RootTreeNode As TreeNode
            Get
                If _rootTreeNode Is Nothing Then _rootTreeNode = CreateRootTreeNode()
                Return _rootTreeNode
            End Get
        End Property

        Private _rootInfo As PuttySessionsInfo

        Public ReadOnly Property RootInfo As PuttySessionsInfo
            Get
                If _rootInfo Is Nothing Then _rootInfo = CreateRootInfo()
                Return _rootInfo
            End Get
        End Property

        Public MustOverride Function GetSessionNames(Optional ByVal raw As Boolean = False) As String()
        Public MustOverride Function GetSession(sessionName As String) As PuttyInfo

        Public Overridable Function GetSessions() As PuttyInfo()
            Dim sessionList As New List(Of PuttyInfo)
            Dim sessionInfo As Info
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

#End Region

#Region "Public Events"

        Public Event SessionChanged(sender As Object, e As SessionChangedEventArgs)

#End Region

#Region "Public Classes"

        Public Class SessionChangedEventArgs
            Inherits EventArgs
        End Class

#End Region

#Region "Protected Methods"

        Private Delegate Function CreateRootTreeNodeDelegate() As TreeNode

        Protected Overridable Function CreateRootTreeNode() As TreeNode
            Dim treeView As TreeView = Node.TreeView
            If treeView Is Nothing Then Return Nothing
            If treeView.InvokeRequired Then
                Return treeView.Invoke(New CreateRootTreeNodeDelegate(AddressOf CreateRootTreeNode))
            End If

            Dim newTreeNode As New TreeNode
            RootInfo.TreeNode = newTreeNode

            newTreeNode.Name = _rootInfo.Name
            newTreeNode.Text = _rootInfo.Name
            newTreeNode.Tag = _rootInfo
            newTreeNode.ImageIndex = Enums.TreeImage.PuttySessions
            newTreeNode.SelectedImageIndex = Enums.TreeImage.PuttySessions

            Return newTreeNode
        End Function

        Protected Overridable Function CreateRootInfo() As PuttySessionsInfo
            Dim newRootInfo As New PuttySessionsInfo

            If String.IsNullOrEmpty(My.Settings.PuttySavedSessionsName) Then
                newRootInfo.Name = Language.Language.strPuttySavedSessionsRootName
            Else
                newRootInfo.Name = My.Settings.PuttySavedSessionsName
            End If

            If String.IsNullOrEmpty(My.Settings.PuttySavedSessionsPanel) Then
                newRootInfo.Panel = Language.Language.strGeneral
            Else
                newRootInfo.Panel = My.Settings.PuttySavedSessionsPanel
            End If

            Return newRootInfo
        End Function

        Protected Overridable Sub OnSessionChanged(e As SessionChangedEventArgs)
            RaiseEvent SessionChanged(Me, New SessionChangedEventArgs())
        End Sub

#End Region
    End Class
End Namespace
