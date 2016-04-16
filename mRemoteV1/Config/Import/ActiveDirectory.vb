Imports System.DirectoryServices
Imports System.Text.RegularExpressions
Imports mRemote3G.App
Imports mRemote3G.Container
Imports mRemote3G.Tree

Namespace Config.Import
    Public Class ActiveDirectory
        Public Shared Sub Import(ldapPath As String, parentTreeNode As TreeNode)
            Try
                Dim treeNode As TreeNode = Node.AddNode(Node.Type.Container)

                Dim containerInfo As New Info()
                containerInfo.TreeNode = treeNode
                containerInfo.ConnectionInfo = New Connection.Info(containerInfo)

                Dim name As String
                Dim match As Match = Regex.Match(ldapPath, "ou=([^,]*)", RegexOptions.IgnoreCase)
                If match.Success Then
                    name = match.Groups(1).Captures(0).Value
                Else
                    name = Language.Language.strActiveDirectory
                End If

                containerInfo.Name = name

                ' We can only inherit from a container node, not the root node or connection nodes
                If Node.GetNodeType(parentTreeNode) = Node.Type.Container Then
                    containerInfo.Parent = parentTreeNode.Tag
                Else
                    containerInfo.ConnectionInfo.Inherit.TurnOffInheritanceCompletely()
                End If

                treeNode.Text = name
                treeNode.Name = name
                treeNode.Tag = containerInfo
                Runtime.ContainerList.Add(containerInfo)

                ImportComputers(ldapPath, treeNode)

                parentTreeNode.Nodes.Add(treeNode)
            Catch ex As Exception
                Runtime.MessageCollector.AddExceptionMessage("Config.Import.ActiveDirectory.Import() failed.", ex, ,
                                                             True)
            End Try
        End Sub

        Private Shared Sub ImportComputers(ldapPath As String, parentTreeNode As TreeNode)
            Try
                Dim strDisplayName, strDescription, strHostName As String

                Const ldapFilter = "(objectClass=computer)"

                Dim ldapSearcher As New DirectorySearcher
                Dim ldapResults As SearchResultCollection
                Dim ldapResult As SearchResult

                With ldapSearcher
                    .SearchRoot = New DirectoryEntry(ldapPath)
                    .PropertiesToLoad.AddRange({"securityEquals", "cn"})
                    .Filter = ldapFilter
                    .SearchScope = SearchScope.OneLevel
                End With

                ldapResults = ldapSearcher.FindAll()

                For Each ldapResult In ldapResults
                    With ldapResult.GetDirectoryEntry()
                        strDisplayName = .Properties("cn").Value
                        strDescription = .Properties("Description").Value
                        strHostName = .Properties("dNSHostName").Value
                    End With

                    Dim treeNode As TreeNode = Node.AddNode(Node.Type.Connection, strDisplayName)

                    Dim connectionInfo As New Connection.Info()
                    Dim inheritanceInfo As New Connection.Info.Inheritance(connectionInfo, True)
                    inheritanceInfo.Description = False
                    If TypeOf parentTreeNode.Tag Is Info Then
                        connectionInfo.Parent = parentTreeNode.Tag
                    End If
                    connectionInfo.Inherit = inheritanceInfo
                    connectionInfo.Name = strDisplayName
                    connectionInfo.Hostname = strHostName
                    connectionInfo.Description = strDescription
                    connectionInfo.TreeNode = treeNode
                    treeNode.Name = strDisplayName
                    treeNode.Tag = connectionInfo 'set the nodes tag to the conI
                    'add connection to connections
                    Runtime.ConnectionList.Add(connectionInfo)

                    parentTreeNode.Nodes.Add(treeNode)
                Next
            Catch ex As Exception
                Runtime.MessageCollector.AddExceptionMessage("Config.Import.ActiveDirectory.ImportComputers() failed.",
                                                             ex, , True)
            End Try
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace