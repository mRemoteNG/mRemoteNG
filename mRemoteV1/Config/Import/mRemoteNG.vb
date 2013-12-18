Imports System.IO
Imports mRemoteNG.App.Runtime

Namespace Config.Import
    ' ReSharper disable once InconsistentNaming
    Public Class mRemoteNG
        Public Shared Sub Import(ByVal fileName As String, ByVal parentTreeNode As TreeNode)
            Dim name As String = Path.GetFileNameWithoutExtension(fileName)
            Dim treeNode As New TreeNode(name)
            parentTreeNode.Nodes.Add(treeNode)

            Dim containerInfo As New Container.Info
            containerInfo.TreeNode = treeNode
            containerInfo.Name = name

            Dim connectionInfo As New Connection.Info
            connectionInfo.Inherit = New Connection.Info.Inheritance(connectionInfo)
            connectionInfo.Name = name
            connectionInfo.TreeNode = treeNode
            connectionInfo.Parent = containerInfo
            connectionInfo.IsContainer = True
            containerInfo.ConnectionInfo = connectionInfo

            ' We can only inherit from a container node, not the root node or connection nodes
            If Tree.Node.GetNodeType(parentTreeNode) = Tree.Node.Type.Container Then
                containerInfo.Parent = parentTreeNode.Tag
            Else
                connectionInfo.Inherit.TurnOffInheritanceCompletely()
            End If

            treeNode.Name = name
            treeNode.Tag = containerInfo
            treeNode.ImageIndex = Images.Enums.TreeImage.Container
            treeNode.SelectedImageIndex = Images.Enums.TreeImage.Container

            Dim connectionsLoad As New Connections.Load
            With connectionsLoad
                .ConnectionFileName = fileName
                .RootTreeNode = treeNode
                .ConnectionList = ConnectionList
                .ContainerList = ContainerList
            End With

            connectionsLoad.Load(True)

            ContainerList.Add(containerInfo)
        End Sub
    End Class
End Namespace
