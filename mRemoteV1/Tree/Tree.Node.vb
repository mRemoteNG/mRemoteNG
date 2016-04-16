Imports mRemote3G.App
Imports mRemote3G.Connection
Imports mRemote3G.Connection.PuttySession
Imports mRemote3G.Images
Imports mRemote3G.Messages
Imports mRemote3G.Root.PuttySessions

Namespace Tree
    Public Class Node
        Public Enum Type
            None = 0
            Root = 1
            Container = 2
            Connection = 3
            PuttyRoot = 4
            PuttySession = 5
        End Enum

        Private Shared _TreeView As TreeView

        Public Shared Property TreeView As TreeView
            Get
                Return _TreeView
            End Get
            Set
                _TreeView = value
            End Set
        End Property

        Public Shared Property SelectedNode As TreeNode
            Get
                Return _TreeView.SelectedNode
            End Get
            Set
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


        Public Shared Function GetConstantID(node As TreeNode) As String
            Select Case GetNodeType(node)
                Case Type.Connection
                    Return TryCast(node.Tag, Info).ConstantID
                Case Type.Container
                    Return TryCast(node.Tag, Container.Info).ConnectionInfo.ConstantID
            End Select

            Return Nothing
        End Function

        Public Shared Function GetNodeFromPositionID(id As Integer) As TreeNode
            For Each conI As Info In Runtime.ConnectionList
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

        Public Shared Function GetNodeFromConstantID(id As String) As TreeNode
            For Each conI As Info In Runtime.ConnectionList
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

        Public Shared Function GetNodeType(treeNode As TreeNode) As Type
            Try
                If treeNode Is Nothing Then
                    Return Type.NONE
                End If

                If treeNode.Tag Is Nothing Then
                    Return Type.NONE
                End If

                If TypeOf treeNode.Tag Is PuttySessionsInfo Then
                    Return Type.PuttyRoot
                ElseIf TypeOf treeNode.Tag Is Root.Info Then
                    Return Type.Root
                ElseIf TypeOf treeNode.Tag Is Container.Info Then
                    Return Type.Container
                ElseIf TypeOf treeNode.Tag Is PuttyInfo Then
                    Return Type.PuttySession
                ElseIf TypeOf treeNode.Tag Is Info Then
                    Return Type.Connection
                End If
            Catch ex As Exception
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    "Couldn't get node type" & vbNewLine & ex.ToString(), True)
            End Try

            Return Type.NONE
        End Function

        Public Shared Function GetNodeTypeFromString(str As String) As Type
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
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    "Couldn't get node type from string" & vbNewLine & ex.ToString(),
                                                    True)
            End Try

            Return Type.NONE
        End Function

        Public Shared Function Find(treeNode As TreeNode, searchFor As String) As TreeNode
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
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    "Find node failed" & vbNewLine & ex.ToString(), True)
            End Try

            Return Nothing
        End Function

        Public Shared Function Find(treeNode As TreeNode, conInfo As Info) As TreeNode
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
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    "Find node failed" & vbNewLine & ex.ToString(), True)
            End Try

            Return Nothing
        End Function

        Public Shared Function IsEmpty(treeNode As TreeNode) As Boolean
            Try
                If treeNode.Nodes.Count <= 0 Then
                    Return False
                End If
            Catch ex As Exception
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    "IsEmpty (Tree.Node) failed" & vbNewLine & ex.ToString(), True)
            End Try

            Return True
        End Function


        Public Shared Function AddNode(nodeType As Type, Optional ByVal name As String = Nothing) As TreeNode
            Try
                Dim treeNode As New TreeNode
                Dim defaultName = ""

                Select Case nodeType
                    Case Type.Connection, Type.PuttySession
                        defaultName = Language.Language.strNewConnection
                        treeNode.ImageIndex = Enums.TreeImage.ConnectionClosed
                        treeNode.SelectedImageIndex = Enums.TreeImage.ConnectionClosed
                    Case Type.Container
                        defaultName = Language.Language.strNewFolder
                        treeNode.ImageIndex = Enums.TreeImage.Container
                        treeNode.SelectedImageIndex = Enums.TreeImage.Container
                    Case Type.Root
                        defaultName = Language.Language.strNewRoot
                        treeNode.ImageIndex = Enums.TreeImage.Root
                        treeNode.SelectedImageIndex = Enums.TreeImage.Root
                End Select

                If Not String.IsNullOrEmpty(name) Then
                    treeNode.Name = name
                Else
                    treeNode.Name = defaultName
                End If
                treeNode.Text = treeNode.Name

                Return treeNode
            Catch ex As Exception
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "AddNode failed" & vbNewLine & ex.ToString(),
                                                    True)
            End Try

            Return Nothing
        End Function

        Public Shared Sub CloneNode(oldTreeNode As TreeNode, Optional ByVal parentNode As TreeNode = Nothing)
            Try
                If GetNodeType(oldTreeNode) = Type.Connection Then
                    Dim oldConnectionInfo = DirectCast(oldTreeNode.Tag, Info)

                    Dim newConnectionInfo As Info = oldConnectionInfo.Copy()
                    Dim newInheritance As Info.Inheritance = oldConnectionInfo.Inherit.Copy()
                    newInheritance.Parent = newConnectionInfo
                    newConnectionInfo.Inherit = newInheritance

                    Runtime.ConnectionList.Add(newConnectionInfo)

                    Dim newTreeNode As New TreeNode(newConnectionInfo.Name)
                    newTreeNode.Tag = newConnectionInfo
                    newTreeNode.ImageIndex = Enums.TreeImage.ConnectionClosed
                    newTreeNode.SelectedImageIndex = Enums.TreeImage.ConnectionClosed

                    newConnectionInfo.TreeNode = newTreeNode

                    If parentNode Is Nothing Then
                        oldTreeNode.Parent.Nodes.Insert(oldTreeNode.Index + 1, newTreeNode)
                        TreeView.SelectedNode = newTreeNode
                    Else
                        Dim parentContainerInfo = TryCast(parentNode.Tag, Container.Info)
                        If parentContainerInfo IsNot Nothing Then
                            newConnectionInfo.Parent = parentContainerInfo
                        End If
                        parentNode.Nodes.Add(newTreeNode)
                    End If
                ElseIf GetNodeType(oldTreeNode) = Type.Container Then
                    Dim oldContainerInfo = DirectCast(oldTreeNode.Tag, Container.Info)

                    Dim newContainerInfo As Container.Info = oldContainerInfo.Copy()
                    Dim newConnectionInfo As Info = oldContainerInfo.ConnectionInfo.Copy()
                    newContainerInfo.ConnectionInfo = newConnectionInfo

                    Dim newTreeNode As New TreeNode(newContainerInfo.Name)
                    newTreeNode.Tag = newContainerInfo
                    newTreeNode.ImageIndex = Enums.TreeImage.Container
                    newTreeNode.SelectedImageIndex = Enums.TreeImage.Container
                    newContainerInfo.ConnectionInfo.Parent = newContainerInfo

                    Runtime.ContainerList.Add(newContainerInfo)

                    If parentNode Is Nothing Then
                        oldTreeNode.Parent.Nodes.Insert(oldTreeNode.Index + 1, newTreeNode)
                        TreeView.SelectedNode = newTreeNode
                    Else
                        parentNode.Nodes.Add(newTreeNode)
                    End If

                    For Each childTreeNode As TreeNode In oldTreeNode.Nodes
                        CloneNode(childTreeNode, newTreeNode)
                    Next

                    newTreeNode.Expand()
                End If
            Catch ex As Exception
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                                                    String.Format(Language.Language.strErrorCloneNodeFailed,
                                                                  ex.ToString()))
            End Try
        End Sub

        Public Shared Sub SetNodeImage(treeNode As TreeNode, Img As Enums.TreeImage)
            SetNodeImageIndex(treeNode, Img)
        End Sub

        Private Delegate Sub SetNodeImageIndexDelegate(treeNode As TreeNode, imageIndex As Integer)

        Private Shared Sub SetNodeImageIndex(treeNode As TreeNode, imageIndex As Integer)
            If treeNode Is Nothing OrElse treeNode.TreeView Is Nothing Then Return
            If treeNode.TreeView.InvokeRequired Then
                treeNode.TreeView.Invoke(New SetNodeImageIndexDelegate(AddressOf SetNodeImageIndex),
                                         New Object() {treeNode, imageIndex})
                Return
            End If

            treeNode.ImageIndex = imageIndex
            treeNode.SelectedImageIndex = imageIndex
        End Sub

        Public Shared Sub SetNodeToolTip(e As MouseEventArgs, tTip As ToolTip)
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
                            tTip.SetToolTip(_TreeView, TryCast(old_node.Tag, Info).Description)
                        End If
                    End If
                End If
            Catch ex As Exception
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    "SetNodeToolTip failed" & vbNewLine & ex.ToString(), True)
            End Try
        End Sub


        Public Shared Sub DeleteSelectedNode()
            Try
                If SelectedNode Is Nothing Then Return

                Select Case GetNodeType(SelectedNode)
                    Case Type.Root
                        Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, "The root item cannot be deleted!")
                    Case Type.Container
                        If IsEmpty(SelectedNode) = False Then
                            If _
                                MsgBox(String.Format(Language.Language.strConfirmDeleteNodeFolder, SelectedNode.Text),
                                       MsgBoxStyle.YesNo Or MsgBoxStyle.Question) = MsgBoxResult.Yes Then
                                SelectedNode.Remove()
                            End If
                        Else
                            If _
                                MsgBox(
                                    String.Format(Language.Language.strConfirmDeleteNodeFolderNotEmpty,
                                                  SelectedNode.Text), MsgBoxStyle.YesNo Or MsgBoxStyle.Question) =
                                MsgBoxResult.Yes Then
                                For Each tNode As TreeNode In SelectedNode.Nodes
                                    tNode.Remove()
                                Next
                                SelectedNode.Remove()
                            End If
                        End If
                    Case Type.Connection
                        If _
                            MsgBox(String.Format(Language.Language.strConfirmDeleteNodeConnection, SelectedNode.Text),
                                   MsgBoxStyle.YesNo Or MsgBoxStyle.Question) = MsgBoxResult.Yes Then
                            SelectedNode.Remove()
                        End If
                    Case Else
                        Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                                                            "Tree item type is unknown so it cannot be deleted!")
                End Select
            Catch ex As Exception
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    "Deleting selected node failed" & vbNewLine & ex.ToString(), True)
            End Try
        End Sub

        Public Shared Sub StartRenameSelectedNode()
            If SelectedNode IsNot Nothing Then SelectedNode.BeginEdit()
        End Sub

        Public Shared Sub FinishRenameSelectedNode(newName As String)
            If newName Is Nothing Then Return

            If newName.Length > 0 Then
                SelectedNode.Tag.Name = newName

                If My.Settings.SetHostnameLikeDisplayName Then
                    Dim connectionInfo = TryCast(SelectedNode.Tag, Info)
                    If (connectionInfo IsNot Nothing) Then
                        connectionInfo.Hostname = newName
                    End If
                End If
            End If
        End Sub

        Public Shared Sub MoveNodeUp()
            Try
                If SelectedNode IsNot Nothing Then
                    If Not (SelectedNode.PrevNode Is Nothing) Then
                        TreeView.BeginUpdate()
                        TreeView.Sorted = False

                        Dim newNode As TreeNode = SelectedNode.Clone
                        SelectedNode.Parent.Nodes.Insert(SelectedNode.Index - 1, newNode)
                        SelectedNode.Remove()
                        SelectedNode = newNode

                        TreeView.EndUpdate()
                    End If
                End If
            Catch ex As Exception
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    "MoveNodeUp failed" & vbNewLine & ex.ToString(), True)
            End Try
        End Sub

        Public Shared Sub MoveNodeDown()
            Try
                If SelectedNode IsNot Nothing Then
                    If Not (SelectedNode.NextNode Is Nothing) Then
                        TreeView.BeginUpdate()
                        TreeView.Sorted = False

                        Dim newNode As TreeNode = SelectedNode.Clone
                        SelectedNode.Parent.Nodes.Insert(SelectedNode.Index + 2, newNode)
                        SelectedNode.Remove()
                        SelectedNode = newNode

                        TreeView.EndUpdate()
                    End If
                End If
            Catch ex As Exception
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    "MoveNodeDown failed" & vbNewLine & ex.ToString(), True)
            End Try
        End Sub

        Public Shared Sub ExpandAllNodes()
            TreeView.BeginUpdate()
            TreeView.ExpandAll()
            TreeView.EndUpdate()
        End Sub

        Public Shared Sub CollapseAllNodes()
            TreeView.BeginUpdate()
            For Each treeNode As TreeNode In TreeView.Nodes(0).Nodes
                treeNode.Collapse(False)
            Next
            TreeView.EndUpdate()
        End Sub

        Public Shared Sub Sort(treeNode As TreeNode, sorting As SortOrder)
            If TreeView Is Nothing Then Return

            TreeView.BeginUpdate()

            If treeNode Is Nothing Then
                If TreeView.Nodes.Count > 0 Then
                    treeNode = TreeView.Nodes.Item(0)
                Else
                    Return
                End If
            ElseIf GetNodeType(treeNode) = Type.Connection Then
                treeNode = treeNode.Parent
                If treeNode Is Nothing Then Return
            End If

            Sort(treeNode, New Tools.Controls.TreeNodeSorter(sorting))

            TreeView.EndUpdate()
        End Sub

        ' Adapted from http://www.codeproject.com/Tips/252234/ASP-NET-TreeView-Sort
        Private Shared Sub Sort(treeNode As TreeNode, nodeSorter As Tools.Controls.TreeNodeSorter)
            For Each childNode As TreeNode In treeNode.Nodes
                Sort(childNode, nodeSorter)
            Next

            Try
                Dim sortedNodes As New List(Of TreeNode)
                Dim currentNode As TreeNode = Nothing
                While (treeNode.Nodes.Count > 0)
                    For Each childNode As TreeNode In treeNode.Nodes
                        If (currentNode Is Nothing OrElse nodeSorter.Compare(childNode, currentNode) < 0) Then
                            currentNode = childNode
                        End If
                    Next
                    treeNode.Nodes.Remove(currentNode)
                    sortedNodes.Add(currentNode)
                    currentNode = Nothing
                End While

                For Each childNode As TreeNode In sortedNodes
                    treeNode.Nodes.Add(childNode)
                Next
            Catch ex As Exception
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    "Sort nodes failed" & vbNewLine & ex.ToString(), True)
            End Try
        End Sub

        Private Delegate Sub ResetTreeDelegate()

        Public Shared Sub ResetTree()
            If TreeView.InvokeRequired Then
                Dim resetTreeDelegate As New ResetTreeDelegate(AddressOf ResetTree)
                Runtime.Windows.treeForm.Invoke(resetTreeDelegate)
            Else
                TreeView.BeginUpdate()
                TreeView.Nodes.Clear()
                TreeView.Nodes.Add(Language.Language.strConnections)
                TreeView.EndUpdate()
            End If
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
