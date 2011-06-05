Imports System.Windows.Forms
Imports mRemoteNG.App.Runtime
Imports System.DirectoryServices

Namespace Tree
    Public Class Node
        Public Enum Type
            Root = 0
            Container = 1
            Connection = 2
            NONE = 66
        End Enum

        Private Shared _TreeView As TreeView
        Public Shared Property TreeView() As TreeView
            Get
                Return _TreeView
            End Get
            Set(ByVal value As TreeView)
                _TreeView = value
            End Set
        End Property

        Public Shared Property SelectedNode() As TreeNode
            Get
                Return _TreeView.SelectedNode
            End Get
            Set(ByVal value As TreeNode)
                treeNodeToBeSelected = value
                SelectNode()
            End Set
        End Property

        Private Shared treeNodeToBeSelected As TreeNode
        Private Delegate Sub SelectNodeCB()
        Private Shared Sub SelectNode()
            If _TreeView.InvokeRequired = True Then
                Dim d As New SelectNodeCB(AddressOf SelectNode)
                _TreeView.Invoke(d)
            Else
                _TreeView.SelectedNode = treeNodeToBeSelected
            End If
        End Sub


        Public Shared Function GetConstantID(ByVal node As TreeNode) As String
            Select Case GetNodeType(node)
                Case Type.Connection
                    Return TryCast(node.Tag, mRemoteNG.Connection.Info).ConstantID
                Case Type.Container
                    Return TryCast(node.Tag, mRemoteNG.Container.Info).ConnectionInfo.ConstantID
            End Select

            Return Nothing
        End Function

        Public Shared Function GetNodeFromPositionID(ByVal id As Integer) As TreeNode
            For Each conI As Connection.Info In connectionList
                If conI.PositionID = id Then
                    If conI.IsContainer Then
                        Return TryCast(conI.Parent, Container.Info).TreeNode
                    Else
                        Return conI.TreeNode
                    End If
                End If
            Next

            Return Nothing
        End Function

        Public Shared Function GetNodeFromConstantID(ByVal id As String) As TreeNode
            For Each conI As Connection.Info In connectionList
                If conI.ConstantID = id Then
                    If conI.IsContainer Then
                        Return TryCast(conI.Parent, Container.Info).TreeNode
                    Else
                        Return conI.TreeNode
                    End If
                End If
            Next

            Return Nothing
        End Function

        Public Shared Function GetNodeType(ByVal treeNode As TreeNode) As Tree.Node.Type
            Try
                If treeNode Is Nothing Then
                    Return Type.NONE
                End If

                If treeNode.Tag Is Nothing Then
                    Return Type.NONE
                End If

                If TypeOf treeNode.Tag Is Root.Info Then
                    Return Type.Root
                ElseIf TypeOf treeNode.Tag Is Container.Info Then
                    Return Type.Container
                ElseIf TypeOf treeNode.Tag Is Connection.Info Then
                    Return Type.Connection
                End If
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn't get node type" & vbNewLine & ex.Message, True)
            End Try

            Return Type.NONE
        End Function

        Public Shared Function GetNodeTypeFromString(ByVal str As String) As Tree.Node.Type
            Try
                Select Case LCase(str)
                    Case "root"
                        Return Type.Root
                    Case "container"
                        Return Type.Container
                    Case "connection"
                        Return Type.Connection
                End Select
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn't get node type from string" & vbNewLine & ex.Message, True)
            End Try

            Return Type.NONE
        End Function

        Public Shared Function Find(ByVal treeNode As TreeNode, ByVal searchFor As String) As TreeNode
            Dim tmpNode As TreeNode

            Try
                If InStr(LCase(treeNode.Text), LCase(searchFor)) > 0 Then
                    Return treeNode
                Else
                    For Each childNode As TreeNode In treeNode.Nodes
                        tmpNode = Find(childNode, searchFor)
                        If Not tmpNode Is Nothing Then
                            Return tmpNode
                        End If
                    Next
                End If
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Find node failed" & vbNewLine & ex.Message, True)
            End Try

            Return Nothing
        End Function

        Public Shared Function Find(ByVal treeNode As TreeNode, ByVal conInfo As Connection.Info) As TreeNode
            Dim tmpNode As TreeNode

            Try
                If treeNode.Tag Is conInfo Then
                    Return treeNode
                Else
                    For Each childNode As TreeNode In treeNode.Nodes
                        tmpNode = Find(childNode, conInfo)
                        If Not tmpNode Is Nothing Then
                            Return tmpNode
                        End If
                    Next
                End If
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Find node failed" & vbNewLine & ex.Message, True)
            End Try

            Return Nothing
        End Function

        Public Shared Function IsEmpty(ByVal treeNode As TreeNode) As Boolean
            Try
                If treeNode.Nodes.Count <= 0 Then
                    Return False
                End If
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "IsEmpty (Tree.Node) failed" & vbNewLine & ex.Message, True)
            End Try

            Return True
        End Function



        Public Shared Function AddNode(ByVal NodeType As Tree.Node.Type, Optional ByVal Text As String = "") As TreeNode
            Try
                Dim nNode As New TreeNode

                Select Case NodeType
                    Case Type.Connection
                        nNode.Text = My.Resources.strNewConnection
                        nNode.ImageIndex = Images.Enums.TreeImage.ConnectionClosed
                        nNode.SelectedImageIndex = Images.Enums.TreeImage.ConnectionClosed
                    Case Type.Container
                        nNode.Text = My.Resources.strNewFolder
                        nNode.ImageIndex = Images.Enums.TreeImage.Container
                        nNode.SelectedImageIndex = Images.Enums.TreeImage.Container
                    Case Type.Root
                        nNode.Text = My.Resources.strNewRoot
                        nNode.ImageIndex = Images.Enums.TreeImage.Root
                        nNode.SelectedImageIndex = Images.Enums.TreeImage.Root
                End Select

                If Text <> "" Then
                    nNode.Text = Text
                End If

                Return nNode
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "AddNode failed" & vbNewLine & ex.Message, True)
            End Try

            Return Nothing
        End Function

        Public Shared Sub AddADNodes(ByVal ldapPath As String)
            Try
                Dim adCNode As TreeNode = Tree.Node.AddNode(Type.Container)

                Dim nContI As New mRemoteNG.Container.Info()
                nContI.TreeNode = adCNode
                nContI.ConnectionInfo = New mRemoteNG.Connection.Info(nContI)

                If Tree.Node.SelectedNode IsNot Nothing Then
                    If Tree.Node.GetNodeType(Tree.Node.SelectedNode) = Tree.Node.Type.Container Then
                        nContI.Parent = Tree.Node.SelectedNode.Tag
                    End If
                End If

                Dim strDisplayName As String
                strDisplayName = ldapPath.Remove(0, ldapPath.IndexOf("OU=") + 3)
                strDisplayName = strDisplayName.Substring(0, strDisplayName.IndexOf(","))

                nContI.Name = strDisplayName
                nContI.TreeNode.Text = strDisplayName

                adCNode.Tag = nContI
                containerList.Add(nContI)


                CreateADSubNodes(adCNode, ldapPath)


                SelectedNode.Nodes.Add(adCNode)
                SelectedNode = SelectedNode.Nodes(SelectedNode.Nodes.Count - 1)
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "AddADNodes failed" & vbNewLine & ex.Message, True)
            End Try
        End Sub

        Private Shared Sub CreateADSubNodes(ByVal rNode As TreeNode, ByVal ldapPath As String)
            Try
                Dim strDisplayName, strDescription, strHostName As String

                Dim ldapFilter As String = "(objectClass=computer)" '"sAMAccountName=*"

                Dim ldapSearcher As New DirectorySearcher
                Dim ldapResults As SearchResultCollection
                Dim ldapResult As SearchResult

                Dim ResultFields() As String = {"securityEquals", "cn"}

                With ldapSearcher
                    .SearchRoot = New DirectoryEntry(ldapPath)
                    .PropertiesToLoad.AddRange(ResultFields)
                    .Filter = ldapFilter
                    .SearchScope = SearchScope.OneLevel
                    ldapResults = .FindAll
                End With

                For Each ldapResult In ldapResults
                    With ldapResult.GetDirectoryEntry()
                        strDisplayName = .Properties("cn").Value
                        strDescription = .Properties("Description").Value
                        strHostName = .Properties("dNSHostName").Value
                    End With

                    Dim adNode As TreeNode = Tree.Node.AddNode(Type.Connection, strDisplayName)

                    Dim nConI As New mRemoteNG.Connection.Info()
                    Dim nInh As New mRemoteNG.Connection.Info.Inheritance(nConI, True)
                    nInh.Description = False
                    If TypeOf rNode.Tag Is mRemoteNG.Container.Info Then
                        nConI.Parent = rNode.Tag
                    End If
                    nConI.Inherit = nInh
                    nConI.Name = strDisplayName
                    nConI.Hostname = strHostName
                    nConI.Description = strDescription
                    nConI.TreeNode = adNode
                    adNode.Tag = nConI 'set the nodes tag to the conI
                    'add connection to connections
                    connectionList.Add(nConI)

                    rNode.Nodes.Add(adNode)
                Next
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "CreateADSubNodes failed" & vbNewLine & ex.Message, True)
            End Try
        End Sub

        Public Shared Sub CloneNode(ByVal oldTreeNode As TreeNode, Optional ByVal parentNode As TreeNode = Nothing)
            Try
                If GetNodeType(oldTreeNode) = Type.Connection Then
                    Dim oldConnectionInfo As Connection.Info = oldTreeNode.Tag

                    Dim newConnectionInfo As Connection.Info = oldConnectionInfo.Copy
                    Dim newInheritance As Connection.Info.Inheritance = oldConnectionInfo.Inherit.Copy()
                    newInheritance.Parent = newConnectionInfo
                    newConnectionInfo.Inherit = newInheritance

                    connectionList.Add(newConnectionInfo)

                    Dim newTreeNode As New TreeNode(newConnectionInfo.Name)
                    newTreeNode.Tag = newConnectionInfo
                    newTreeNode.ImageIndex = Images.Enums.TreeImage.ConnectionClosed
                    newTreeNode.SelectedImageIndex = Images.Enums.TreeImage.ConnectionClosed

                    newConnectionInfo.TreeNode = newTreeNode

                    If parentNode Is Nothing Then
                        oldTreeNode.Parent.Nodes.Add(newTreeNode)
                        Tree.Node.TreeView.SelectedNode = newTreeNode
                    Else
                        parentNode.Nodes.Add(newTreeNode)
                    End If
                ElseIf GetNodeType(oldTreeNode) = Type.Container Then
                    Dim newContainerInfo As Container.Info = TryCast(oldTreeNode.Tag, Container.Info).Copy
                    Dim newConnectionInfo As Connection.Info = TryCast(oldTreeNode.Tag, Container.Info).ConnectionInfo.Copy
                    newContainerInfo.ConnectionInfo = newConnectionInfo

                    Dim newTreeNode As New TreeNode(newContainerInfo.Name)
                    newTreeNode.Tag = newContainerInfo
                    newTreeNode.ImageIndex = Images.Enums.TreeImage.Container
                    newTreeNode.SelectedImageIndex = Images.Enums.TreeImage.Container
                    newContainerInfo.ConnectionInfo.Parent = newContainerInfo

                    containerList.Add(newContainerInfo)

                    oldTreeNode.Parent.Nodes.Add(newTreeNode)

                    Tree.Node.TreeView.SelectedNode = newTreeNode

                    For Each childTreeNode As TreeNode In oldTreeNode.Nodes
                        CloneNode(childTreeNode, newTreeNode)
                    Next
                End If
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, String.Format(My.Resources.strErrorCloneNodeFailed, ex.Message))
            End Try
        End Sub

        Public Shared Sub SetNodeImage(ByVal treeNode As TreeNode, ByVal Img As Images.Enums.TreeImage)
            SetNodeImageIndex(treeNode, Img)
        End Sub

        Private Delegate Sub SetNodeImageIndexCB(ByVal tNode As TreeNode, ByVal ImgIndex As Integer)
        Private Shared Sub SetNodeImageIndex(ByVal tNode As TreeNode, ByVal ImgIndex As Integer)
            If _TreeView.InvokeRequired Then
                Dim s As New SetNodeImageIndexCB(AddressOf SetNodeImageIndex)
                _TreeView.Invoke(s, New Object() {tNode, ImgIndex})
            Else
                tNode.ImageIndex = ImgIndex
                tNode.SelectedImageIndex = ImgIndex
            End If
        End Sub

        Public Shared Sub SetNodeToolTip(ByVal e As MouseEventArgs, ByVal tTip As ToolTip)
            Try
                If My.Settings.ShowDescriptionTooltipsInTree Then
                    'Find the node under the mouse.
                    Static old_node As TreeNode
                    Dim new_node As TreeNode = _TreeView.GetNodeAt(e.X, e.Y)
                    If new_node Is old_node Then Exit Sub
                    old_node = new_node

                    'See if we have a node.
                    If old_node Is Nothing Then
                        tTip.SetToolTip(_TreeView, "")
                    Else
                        'Get this node's object data.
                        If GetNodeType(old_node) = Type.Connection Then
                            tTip.SetToolTip(_TreeView, TryCast(old_node.Tag, mRemoteNG.Connection.Info).Description)
                        End If
                    End If
                End If
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "SetNodeToolTip failed" & vbNewLine & ex.Message, True)
            End Try
        End Sub


        Public Shared Sub DeleteSelectedNode()
            Try
                If SelectedNode Is Nothing Then Return

                Select Case Tree.Node.GetNodeType(SelectedNode)
                    Case Type.Root
                        MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "The root item cannot be deleted!")
                    Case Type.Container
                        If Tree.Node.IsEmpty(SelectedNode) = False Then
                            If MsgBox(String.Format(My.Resources.strConfirmDeleteNodeFolder, SelectedNode.Text), MsgBoxStyle.YesNo Or MsgBoxStyle.Question) = MsgBoxResult.Yes Then
                                SelectedNode.Remove()
                            End If
                        Else
                            If MsgBox(String.Format(My.Resources.strConfirmDeleteNodeFolderNotEmpty, SelectedNode.Text), MsgBoxStyle.YesNo Or MsgBoxStyle.Question) = MsgBoxResult.Yes Then
                                For Each tNode As TreeNode In SelectedNode.Nodes
                                    tNode.Remove()
                                Next
                                SelectedNode.Remove()
                            End If
                        End If
                    Case Type.Connection
                        If MsgBox(String.Format(My.Resources.strConfirmDeleteNodeConnection, SelectedNode.Text), MsgBoxStyle.YesNo Or MsgBoxStyle.Question) = MsgBoxResult.Yes Then
                            SelectedNode.Remove()
                        End If
                    Case Else
                        MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "Tree item type is unknown so it cannot be deleted!")
                End Select
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Deleting selected node failed" & vbNewLine & ex.Message, True)
            End Try
        End Sub

        Public Shared Sub StartRenameSelectedNode()
            If SelectedNode IsNot Nothing Then
                Windows.treeForm.cMenTreeDelete.ShortcutKeys = Keys.None
                SelectedNode.BeginEdit()
            End If
        End Sub

        Public Shared Sub FinishRenameSelectedNode(ByVal NewName As String)
            Windows.treeForm.cMenTreeDelete.ShortcutKeys = Keys.Delete

            If NewName IsNot Nothing Then
                If NewName.Length > 0 Then
                    SelectedNode.Tag.Name = NewName
                End If
            End If
        End Sub

        Public Shared Sub MoveNodeUp()
            Try
                If SelectedNode IsNot Nothing Then
                    If Not (SelectedNode.PrevNode Is Nothing) Then
                        Dim oldParent As TreeNode = SelectedNode.Parent
                        Dim nNode As TreeNode = SelectedNode.Clone
                        SelectedNode.Parent.Nodes.Insert(SelectedNode.Index - 1, nNode)
                        SelectedNode.Remove()
                        SelectedNode = nNode
                    End If
                End If
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "MoveNodeUp failed" & vbNewLine & ex.Message, True)
            End Try
        End Sub

        Public Shared Sub MoveNodeDown()
            Try
                If SelectedNode IsNot Nothing Then
                    If Not (SelectedNode.NextNode Is Nothing) Then
                        Dim oldParent As TreeNode = SelectedNode.Parent
                        Dim nNode As TreeNode = SelectedNode.Clone
                        SelectedNode.Parent.Nodes.Insert(SelectedNode.Index + 2, nNode)
                        SelectedNode.Remove()
                        SelectedNode = nNode
                    End If
                End If
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "MoveNodeDown failed" & vbNewLine & ex.Message, True)
            End Try
        End Sub

        Public Shared Sub ExpandAllNodes()
            _TreeView.ExpandAll()
        End Sub

        Public Shared Sub CollapseAllNodes()
            For Each tNode As TreeNode In _TreeView.Nodes(0).Nodes
                tNode.Collapse(False)
            Next
        End Sub

        Public Shared Sub Sort(ByVal treeNode As TreeNode, ByVal sortType As Tools.Controls.TreeNodeSorter.SortType)
            Try
                If treeNode Is Nothing Then
                    treeNode = _TreeView.Nodes.Item(0)
                ElseIf Tree.Node.GetNodeType(treeNode) = Type.Connection Then
                    treeNode = treeNode.Parent
                End If

                Dim ns As New Tools.Controls.TreeNodeSorter(treeNode, sortType)

                _TreeView.TreeViewNodeSorter = ns
                _TreeView.Sort()

                For Each childNode As TreeNode In treeNode.Nodes
                    If GetNodeType(childNode) = Type.Container Then Sort(childNode, sortType)
                Next
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Sort nodes failed" & vbNewLine & ex.Message, True)
            End Try
        End Sub

        Private Delegate Sub ResetTreeCB()
        Public Shared Sub ResetTree()
            If _TreeView.InvokeRequired Then
                Dim d As New ResetTreeCB(AddressOf ResetTree)
                Windows.treeForm.Invoke(d)
            Else
                _TreeView.Nodes.Clear()
                '_TreeView.Nodes.Add("Credentials")
                _TreeView.Nodes.Add(My.Resources.strConnections)
            End If
        End Sub

    End Class
End Namespace
