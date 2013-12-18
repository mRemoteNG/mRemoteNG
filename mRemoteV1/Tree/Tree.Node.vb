Imports System.Windows.Forms
Imports mRemoteNG.App.Runtime
Imports System.DirectoryServices

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

                If TypeOf treeNode.Tag Is Root.PuttySessions.Info Then
                    Return Type.PuttyRoot
                ElseIf TypeOf treeNode.Tag Is Root.Info Then
                    Return Type.Root
                ElseIf TypeOf treeNode.Tag Is Container.Info Then
                    Return Type.Container
                ElseIf TypeOf treeNode.Tag Is Connection.PuttySession.Info Then
                    Return Type.PuttySession
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



        Public Shared Function AddNode(ByVal nodeType As Type, Optional ByVal name As String = Nothing) As TreeNode
            Try
                Dim treeNode As New TreeNode
                Dim defaultName As String = ""

                Select Case nodeType
                    Case Type.Connection, Type.PuttySession
                        defaultName = My.Language.strNewConnection
                        treeNode.ImageIndex = Images.Enums.TreeImage.ConnectionClosed
                        treeNode.SelectedImageIndex = Images.Enums.TreeImage.ConnectionClosed
                    Case Type.Container
                        defaultName = My.Language.strNewFolder
                        treeNode.ImageIndex = Images.Enums.TreeImage.Container
                        treeNode.SelectedImageIndex = Images.Enums.TreeImage.Container
                    Case Type.Root
                        defaultName = My.Language.strNewRoot
                        treeNode.ImageIndex = Images.Enums.TreeImage.Root
                        treeNode.SelectedImageIndex = Images.Enums.TreeImage.Root
                End Select

                If Not String.IsNullOrEmpty(name) Then
                    treeNode.Name = name
                Else
                    treeNode.Name = defaultName
                End If
                treeNode.Text = treeNode.Name

                Return treeNode
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "AddNode failed" & vbNewLine & ex.Message, True)
            End Try

            Return Nothing
        End Function

        Public Shared Sub CloneNode(ByVal oldTreeNode As TreeNode, Optional ByVal parentNode As TreeNode = Nothing)
            Try
                If GetNodeType(oldTreeNode) = Type.Connection Then
                    Dim oldConnectionInfo As Connection.Info = DirectCast(oldTreeNode.Tag, Connection.Info)

                    Dim newConnectionInfo As Connection.Info = oldConnectionInfo.Copy()
                    Dim newInheritance As Connection.Info.Inheritance = oldConnectionInfo.Inherit.Copy()
                    newInheritance.Parent = newConnectionInfo
                    newConnectionInfo.Inherit = newInheritance

                    ConnectionList.Add(newConnectionInfo)

                    Dim newTreeNode As New TreeNode(newConnectionInfo.Name)
                    newTreeNode.Tag = newConnectionInfo
                    newTreeNode.ImageIndex = Images.Enums.TreeImage.ConnectionClosed
                    newTreeNode.SelectedImageIndex = Images.Enums.TreeImage.ConnectionClosed

                    newConnectionInfo.TreeNode = newTreeNode

                    If parentNode Is Nothing Then
                        oldTreeNode.Parent.Nodes.Insert(oldTreeNode.Index + 1, newTreeNode)
                        TreeView.SelectedNode = newTreeNode
                    Else
                        Dim parentContainerInfo As Container.Info = TryCast(parentNode.Tag, Container.Info)
                        If parentContainerInfo IsNot Nothing Then
                            newConnectionInfo.Parent = parentContainerInfo
                        End If
                        parentNode.Nodes.Add(newTreeNode)
                    End If
                ElseIf GetNodeType(oldTreeNode) = Type.Container Then
                    Dim oldContainerInfo As Container.Info = DirectCast(oldTreeNode.Tag, Container.Info)

                    Dim newContainerInfo As Container.Info = oldContainerInfo.Copy()
                    Dim newConnectionInfo As Connection.Info = oldContainerInfo.ConnectionInfo.Copy()
                    newContainerInfo.ConnectionInfo = newConnectionInfo

                    Dim newTreeNode As New TreeNode(newContainerInfo.Name)
                    newTreeNode.Tag = newContainerInfo
                    newTreeNode.ImageIndex = Images.Enums.TreeImage.Container
                    newTreeNode.SelectedImageIndex = Images.Enums.TreeImage.Container
                    newContainerInfo.ConnectionInfo.Parent = newContainerInfo

                    ContainerList.Add(newContainerInfo)

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
                MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, String.Format(My.Language.strErrorCloneNodeFailed, ex.Message))
            End Try
        End Sub

        Public Shared Sub SetNodeImage(ByVal treeNode As TreeNode, ByVal Img As Images.Enums.TreeImage)
            SetNodeImageIndex(treeNode, Img)
        End Sub

        Private Delegate Sub SetNodeImageIndexDelegate(ByVal treeNode As TreeNode, ByVal imageIndex As Integer)
        Private Shared Sub SetNodeImageIndex(ByVal treeNode As TreeNode, ByVal imageIndex As Integer)
            If treeNode Is Nothing OrElse treeNode.TreeView Is Nothing Then Return
            If treeNode.TreeView.InvokeRequired Then
                treeNode.TreeView.Invoke(New SetNodeImageIndexDelegate(AddressOf SetNodeImageIndex), New Object() {treeNode, imageIndex})
                Return
            End If

            treeNode.ImageIndex = imageIndex
            treeNode.SelectedImageIndex = imageIndex
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
                            If MsgBox(String.Format(My.Language.strConfirmDeleteNodeFolder, SelectedNode.Text), MsgBoxStyle.YesNo Or MsgBoxStyle.Question) = MsgBoxResult.Yes Then
                                SelectedNode.Remove()
                            End If
                        Else
                            If MsgBox(String.Format(My.Language.strConfirmDeleteNodeFolderNotEmpty, SelectedNode.Text), MsgBoxStyle.YesNo Or MsgBoxStyle.Question) = MsgBoxResult.Yes Then
                                For Each tNode As TreeNode In SelectedNode.Nodes
                                    tNode.Remove()
                                Next
                                SelectedNode.Remove()
                            End If
                        End If
                    Case Type.Connection
                        If MsgBox(String.Format(My.Language.strConfirmDeleteNodeConnection, SelectedNode.Text), MsgBoxStyle.YesNo Or MsgBoxStyle.Question) = MsgBoxResult.Yes Then
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
            If SelectedNode IsNot Nothing Then SelectedNode.BeginEdit()
        End Sub

        Public Shared Sub FinishRenameSelectedNode(ByVal newName As String)
            If newName Is Nothing Then Return

            If newName.Length > 0 Then
                SelectedNode.Tag.Name = newName

                If My.Settings.SetHostnameLikeDisplayName Then
                    Dim connectionInfo As Connection.Info = TryCast(SelectedNode.Tag, Connection.Info)
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
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "MoveNodeUp failed" & vbNewLine & ex.Message, True)
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
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "MoveNodeDown failed" & vbNewLine & ex.Message, True)
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

        Public Shared Sub Sort(ByVal treeNode As TreeNode, ByVal sorting As System.Windows.Forms.SortOrder)
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
        Private Shared Sub Sort(ByVal treeNode As TreeNode, ByVal nodeSorter As Tools.Controls.TreeNodeSorter)
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
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Sort nodes failed" & vbNewLine & ex.Message, True)
            End Try
        End Sub

        Private Delegate Sub ResetTreeDelegate()
        Public Shared Sub ResetTree()
            If TreeView.InvokeRequired Then
                Dim resetTreeDelegate As New ResetTreeDelegate(AddressOf ResetTree)
                Windows.treeForm.Invoke(resetTreeDelegate)
            Else
                TreeView.BeginUpdate()
                TreeView.Nodes.Clear()
                TreeView.Nodes.Add(My.Language.strConnections)
                TreeView.EndUpdate()
            End If
        End Sub
    End Class
End Namespace
