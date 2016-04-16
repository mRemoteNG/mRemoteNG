Imports System.IO
Imports mRemote3G.Config.Import
Imports mRemote3G.Connection.Protocol
Imports mRemote3G.Tools
Imports mRemote3G.Tree
Imports PSTaskDialog

Namespace App
    Public Class Import

#Region "Public Methods"

        Public Shared Sub ImportFromFile(rootTreeNode As TreeNode, selectedTreeNode As TreeNode,
                                         Optional ByVal alwaysUseSelectedTreeNode As Boolean = False)
            Try
                Using openFileDialog As New OpenFileDialog()
                    With openFileDialog
                        .CheckFileExists = True
                        .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
                        .Multiselect = True

                        Dim fileTypes As New List(Of String)
                        fileTypes.AddRange({Language.Language.strFilterAllImportable, "*.xml;*.rdp;*.rdg;*.dat"})
                        fileTypes.AddRange({Language.Language.strFiltermRemoteXML, "*.xml"})
                        fileTypes.AddRange({Language.Language.strFilterRDP, "*.rdp"})
                        fileTypes.AddRange({Language.Language.strFilterRdgFiles, "*.rdg"})
                        fileTypes.AddRange({Language.Language.strFilterPuttyConnectionManager, "*.dat"})
                        fileTypes.AddRange({Language.Language.strFilterAll, "*.*"})

                        .Filter = String.Join("|", fileTypes.ToArray())
                    End With

                    If Not openFileDialog.ShowDialog = DialogResult.OK Then Return

                    Dim parentTreeNode As TreeNode = GetParentTreeNode(rootTreeNode, selectedTreeNode,
                                                                       alwaysUseSelectedTreeNode)
                    If parentTreeNode Is Nothing Then Return

                    For Each fileName As String In openFileDialog.FileNames
                        Try
                            Select Case DetermineFileType(fileName)
                                Case FileType.mRemoteXml
                                    Config.Import.mRemote3G.Import(fileName, parentTreeNode)
                                Case FileType.RemoteDesktopConnection
                                    RemoteDesktopConnection.Import(fileName, parentTreeNode)
                                Case FileType.RemoteDesktopConnectionManager
                                    RemoteDesktopConnectionManager.Import(fileName, parentTreeNode)
                                Case FileType.PuttyConnectionManager
                                    PuttyConnectionManager.Import(fileName, parentTreeNode)
                                Case Else
                                    Throw New FileFormatException("Unrecognized file format.")
                            End Select
                        Catch ex As Exception
                            cTaskDialog.ShowTaskDialogBox(Application.ProductName, Language.Language.strImportFileFailedMainInstruction, String.Format(Language.Language.strImportFileFailedContent, fileName), Tools.Misc.GetExceptionMessageRecursive(ex), "", "", "", "", eTaskDialogButtons.OK, eSysIcons.Error, Nothing)
                        End Try
                    Next

                    parentTreeNode.Expand()
                    Dim parentContainer = TryCast(parentTreeNode.Tag, Container.Info)
                    If parentContainer IsNot Nothing Then parentContainer.IsExpanded = True

                    Runtime.SaveConnectionsBG()
                End Using
            Catch ex As Exception
                Runtime.MessageCollector.AddExceptionMessage("App.Import.ImportFromFile() failed.", ex, , True)
            End Try
        End Sub

        Public Shared Sub ImportFromActiveDirectory(ldapPath As String)
            Try
                Dim rootTreeNode As TreeNode = Node.TreeView.Nodes(0)
                Dim selectedTreeNode As TreeNode = Node.TreeView.SelectedNode

                Dim parentTreeNode As TreeNode = GetParentTreeNode(rootTreeNode, selectedTreeNode)
                If parentTreeNode Is Nothing Then Return

                ActiveDirectory.Import(ldapPath, parentTreeNode)

                parentTreeNode.Expand()
                Dim parentContainer = TryCast(parentTreeNode.Tag, Container.Info)
                If parentContainer IsNot Nothing Then parentContainer.IsExpanded = True

                Runtime.SaveConnectionsBG()
            Catch ex As Exception
                Runtime.MessageCollector.AddExceptionMessage("App.Import.ImportFromActiveDirectory() failed.", ex, ,
                                                             True)
            End Try
        End Sub

        Public Shared Sub ImportFromPortScan(hosts As IEnumerable, protocol As Protocols)
            Try
                Dim rootTreeNode As TreeNode = Node.TreeView.Nodes(0)
                Dim selectedTreeNode As TreeNode = Node.TreeView.SelectedNode

                Dim parentTreeNode As TreeNode = GetParentTreeNode(rootTreeNode, selectedTreeNode)
                If parentTreeNode Is Nothing Then Return

                PortScan.Import(hosts, protocol, parentTreeNode)

                parentTreeNode.Expand()
                Dim parentContainer = TryCast(parentTreeNode.Tag, Container.Info)
                If parentContainer IsNot Nothing Then parentContainer.IsExpanded = True

                Runtime.SaveConnectionsBG()
            Catch ex As Exception
                Runtime.MessageCollector.AddExceptionMessage("App.Import.ImportFromPortScan() failed.", ex, , True)
            End Try
        End Sub

#End Region

#Region "Private Methods"
        Private Shared Function GetParentTreeNode(ByVal rootTreeNode As TreeNode, ByVal selectedTreeNode As TreeNode, Optional ByVal alwaysUseSelectedTreeNode As Boolean = False) As TreeNode
            Dim parentTreeNode As TreeNode

            selectedTreeNode = GetContainerTreeNode(selectedTreeNode)
            If selectedTreeNode Is Nothing OrElse selectedTreeNode Is rootTreeNode Then
                parentTreeNode = rootTreeNode
            Else
                If alwaysUseSelectedTreeNode Then
                    parentTreeNode = GetContainerTreeNode(selectedTreeNode)
                Else
                    cTaskDialog.ShowCommandBox(Application.ProductName, Language.Language.strImportLocationMainInstruction, Language.Language.strImportLocationContent, "", "", "", String.Format(Language.Language.strImportLocationCommandButtons, vbLf, rootTreeNode.Text, selectedTreeNode.Text), True, eSysIcons.Question, 0)
                    Select Case cTaskDialog.CommandButtonResult
                        Case 0 ' Root
                            parentTreeNode = rootTreeNode
                        Case 1 ' Selected Folder
                            parentTreeNode = GetContainerTreeNode(selectedTreeNode)
                        Case Else ' Cancel
                            parentTreeNode = Nothing
                    End Select
                End If
            End If

            Return parentTreeNode
        End Function

        Private Shared Function GetContainerTreeNode(treeNode As TreeNode) As TreeNode
            Select Case Node.GetNodeType(treeNode)
                Case Node.Type.Root, Node.Type.Container
                    Return treeNode
                Case Node.Type.Connection
                    Return treeNode.Parent
                Case Else
                    Return Nothing
            End Select
        End Function

        Private Shared Function DetermineFileType(fileName As String) As FileType
            ' TODO: Use the file contents to determine the file type instead of trusting the extension
            Dim fileExtension As String = Path.GetExtension(fileName).ToLowerInvariant()
            Select Case fileExtension
                Case ".xml"
                    Return FileType.mRemoteXml
                Case ".rdp"
                    Return FileType.RemoteDesktopConnection
                Case ".rdg"
                    Return FileType.RemoteDesktopConnectionManager
                Case ".dat"
                    Return FileType.PuttyConnectionManager
                Case Else
                    Return FileType.Unknown
            End Select
        End Function

#End Region

#Region "Private Enumerations"

        Private Enum FileType As Integer
            Unknown = 0
            ' ReSharper disable once InconsistentNaming
            mRemoteXml
            RemoteDesktopConnection
            RemoteDesktopConnectionManager
            PuttyConnectionManager
        End Enum

        Private Sub New()
        End Sub

#End Region
    End Class
End Namespace