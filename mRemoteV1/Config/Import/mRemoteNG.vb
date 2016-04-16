Imports System.IO
Imports mRemote3G.App
Imports mRemote3G.Config.Connections
Imports mRemote3G.Container
Imports mRemote3G.Images
Imports mRemote3G.Tree

Namespace Config.Import
    ' ReSharper disable once InconsistentNaming
    Public Class mRemote3G
        Public Shared Sub Import(fileName As String, parentTreeNode As TreeNode)
            Dim name As String = Path.GetFileNameWithoutExtension(fileName)
            Dim treeNode As New TreeNode(name)
            parentTreeNode.Nodes.Add(treeNode)

            Dim containerInfo As New Info
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
            If Node.GetNodeType(parentTreeNode) = Node.Type.Container Then
                containerInfo.Parent = parentTreeNode.Tag
            Else
                connectionInfo.Inherit.TurnOffInheritanceCompletely()
            End If

            treeNode.Name = name
            treeNode.Tag = containerInfo
            treeNode.ImageIndex = Enums.TreeImage.Container
            treeNode.SelectedImageIndex = Enums.TreeImage.Container

            Dim connectionsLoad As New ConnectionsLoad
            With connectionsLoad
                .ConnectionFileName = fileName
                .RootTreeNode = treeNode
                .ConnectionList = Runtime.ConnectionList
                .ContainerList = Runtime.ContainerList
            End With

            connectionsLoad.Load(True)

            Runtime.ContainerList.Add(containerInfo)
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
