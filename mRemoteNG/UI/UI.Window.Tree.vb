Imports mRemoteNG.App
Imports mRemoteNG.My
Imports WeifenLuo.WinFormsUI.Docking
Imports mRemoteNG.App.Runtime

Namespace UI
    Namespace Window
        Public Class Tree
            Inherits Base
#Region "Form Stuff"
            Private Sub Tree_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
                ApplyLanguage()

                AddHandler Themes.ThemeManager.ThemeChanged, AddressOf ApplyTheme
                ApplyTheme()

                txtSearch.Multiline = True
                txtSearch.MinimumSize = New Size(0, 14)
                txtSearch.Size = New Size(txtSearch.Size.Width, 14)
                txtSearch.Multiline = False
            End Sub

            Private Sub ApplyLanguage()
                Text = Language.strConnections
                TabText = Language.strConnections

                mMenAddConnection.ToolTipText = Language.strAddConnection
                mMenAddFolder.ToolTipText = Language.strAddFolder
                mMenView.ToolTipText = Language.strMenuView.Replace("&", "")
                mMenViewExpandAllFolders.Text = Language.strExpandAllFolders
                mMenViewCollapseAllFolders.Text = Language.strCollapseAllFolders
                mMenSortAscending.ToolTipText = Language.strSortAsc

                cMenTreeConnect.Text = Language.strConnect
                cMenTreeConnectWithOptions.Text = Language.strConnectWithOptions
                cMenTreeConnectWithOptionsConnectToConsoleSession.Text = Language.strConnectToConsoleSession
                cMenTreeConnectWithOptionsDontConnectToConsoleSession.Text = Language.strDontConnectToConsoleSessionMenuItem
                cMenTreeConnectWithOptionsConnectInFullscreen.Text = Language.strConnectInFullscreen
                cMenTreeConnectWithOptionsNoCredentials.Text = Language.strConnectNoCredentials
                cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Text = Language.strChoosePanelBeforeConnecting
                cMenTreeDisconnect.Text = Language.strMenuDisconnect

                cMenTreeToolsExternalApps.Text = Language.strMenuExternalTools
                cMenTreeToolsTransferFile.Text = Language.strMenuTransferFile

                cMenTreeDuplicate.Text = Language.strDuplicate
                cMenTreeRename.Text = Language.strRename
                cMenTreeDelete.Text = Language.strMenuDelete

                cMenTreeImport.Text = Language.strImportMenuItem
                cMenTreeImportFile.Text = Language.strImportFromFileMenuItem
                cMenTreeImportActiveDirectory.Text = Language.strImportAD
                cMenTreeImportPortScan.Text = Language.strImportPortScan
                cMenTreeExportFile.Text = Language.strExportToFileMenuItem

                cMenTreeAddConnection.Text = Language.strAddConnection
                cMenTreeAddFolder.Text = Language.strAddFolder

                cMenTreeToolsSort.Text = Language.strSort
                cMenTreeToolsSortAscending.Text = Language.strSortAsc
                cMenTreeToolsSortDescending.Text = Language.strSortDesc
                cMenTreeMoveUp.Text = Language.strMoveUp
                cMenTreeMoveDown.Text = Language.strMoveDown

                txtSearch.Text = Language.strSearchPrompt
            End Sub

            Public Sub ApplyTheme()
                With Themes.ThemeManager.ActiveTheme
                    msMain.BackColor = .ToolbarBackgroundColor
                    msMain.ForeColor = .ToolbarTextColor
                    tvConnections.BackColor = .ConnectionsPanelBackgroundColor
                    tvConnections.ForeColor = .ConnectionsPanelTextColor
                    tvConnections.LineColor = .ConnectionsPanelTreeLineColor
                    BackColor = .ToolbarBackgroundColor
                    txtSearch.BackColor = .SearchBoxBackgroundColor
                    txtSearch.ForeColor = .SearchBoxTextPromptColor
                End With
            End Sub
#End Region

#Region "Public Methods"
            Public Sub New(ByVal panel As DockContent)
                WindowType = Type.Tree
                DockPnl = panel
                InitializeComponent()
                FillImageList()

                DescriptionTooltip = New ToolTip
                DescriptionTooltip.InitialDelay = 300
                DescriptionTooltip.ReshowDelay = 0
            End Sub

            Public Sub InitialRefresh()
                tvConnections_AfterSelect(tvConnections, New TreeViewEventArgs(tvConnections.SelectedNode, TreeViewAction.ByMouse))
            End Sub
#End Region

#Region "Public Properties"
            Public Property DescriptionTooltip() As ToolTip
#End Region

#Region "Private Methods"
            Private Sub FillImageList()
                Try
                    imgListTree.Images.Add(Resources.Root)
                    imgListTree.Images.Add(Resources.Folder)
                    imgListTree.Images.Add(Resources.Play)
                    imgListTree.Images.Add(Resources.Pause)
                    imgListTree.Images.Add(Resources.PuttySessions)
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "FillImageList (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub tvConnections_BeforeLabelEdit(ByVal sender As Object, ByVal e As NodeLabelEditEventArgs) Handles tvConnections.BeforeLabelEdit
                cMenTreeDelete.ShortcutKeys = Keys.None
            End Sub

            Private Sub tvConnections_AfterLabelEdit(ByVal sender As Object, ByVal e As NodeLabelEditEventArgs) Handles tvConnections.AfterLabelEdit
                Try
                    cMenTreeDelete.ShortcutKeys = Keys.Delete

                    mRemoteNG.Tree.Node.FinishRenameSelectedNode(e.Label)
                    Windows.configForm.pGrid_SelectedObjectChanged()
                    ShowHideTreeContextMenuItems(e.Node)
                    SaveConnectionsBG()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "tvConnections_AfterLabelEdit (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub tvConnections_AfterSelect(ByVal sender As Object, ByVal e As TreeViewEventArgs) Handles tvConnections.AfterSelect
                Try
                    Select Case mRemoteNG.Tree.Node.GetNodeType(e.Node)
                        Case mRemoteNG.Tree.Node.Type.Connection, mRemoteNG.Tree.Node.Type.PuttySession
                            Windows.configForm.SetPropertyGridObject(e.Node.Tag)
                        Case mRemoteNG.Tree.Node.Type.Container
                            Windows.configForm.SetPropertyGridObject(TryCast(e.Node.Tag, Container.Info).ConnectionInfo)
                        Case mRemoteNG.Tree.Node.Type.Root, mRemoteNG.Tree.Node.Type.PuttyRoot
                            Windows.configForm.SetPropertyGridObject(e.Node.Tag)
                        Case Else
                            Exit Sub
                    End Select

                    Windows.configForm.pGrid_SelectedObjectChanged()
                    ShowHideTreeContextMenuItems(e.Node)
                    Windows.sessionsForm.GetSessions(True)

                    LastSelected = mRemoteNG.Tree.Node.GetConstantID(e.Node)
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "tvConnections_AfterSelect (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub tvConnections_NodeMouseClick(ByVal sender As Object, ByVal e As TreeNodeMouseClickEventArgs) Handles tvConnections.NodeMouseClick
                Try
                    ShowHideTreeContextMenuItems(tvConnections.SelectedNode)
                    tvConnections.SelectedNode = e.Node

                    If e.Button = MouseButtons.Left Then
                        If Settings.SingleClickOnConnectionOpensIt And _
                            (mRemoteNG.Tree.Node.GetNodeType(e.Node) = mRemoteNG.Tree.Node.Type.Connection Or _
                             mRemoteNG.Tree.Node.GetNodeType(e.Node) = mRemoteNG.Tree.Node.Type.PuttySession) Then
                            OpenConnection()
                        End If

                        If Settings.SingleClickSwitchesToOpenConnection And mRemoteNG.Tree.Node.GetNodeType(e.Node) = mRemoteNG.Tree.Node.Type.Connection Then
                            SwitchToOpenConnection(e.Node.Tag)
                        End If
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "tvConnections_NodeMouseClick (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Shared Sub tvConnections_NodeMouseDoubleClick(ByVal sender As Object, ByVal e As TreeNodeMouseClickEventArgs) Handles tvConnections.NodeMouseDoubleClick
                If mRemoteNG.Tree.Node.GetNodeType(mRemoteNG.Tree.Node.SelectedNode) = mRemoteNG.Tree.Node.Type.Connection Or _
                   mRemoteNG.Tree.Node.GetNodeType(mRemoteNG.Tree.Node.SelectedNode) = mRemoteNG.Tree.Node.Type.PuttySession Then
                    OpenConnection()
                End If
            End Sub

            Private Sub tvConnections_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles tvConnections.MouseMove
                Try
                    mRemoteNG.Tree.Node.SetNodeToolTip(e, DescriptionTooltip)
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "tvConnections_MouseMove (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Shared Sub EnableMenuItemsRecursive(ByVal items As ToolStripItemCollection, Optional ByVal enable As Boolean = True)
                Dim menuItem As ToolStripMenuItem
                For Each item As ToolStripItem In items
                    menuItem = TryCast(item, ToolStripMenuItem)
                    If menuItem Is Nothing Then Continue For
                    menuItem.Enabled = enable
                    If menuItem.HasDropDownItems Then
                        EnableMenuItemsRecursive(menuItem.DropDownItems, enable)
                    End If
                Next
            End Sub

            Private Sub ShowHideTreeContextMenuItems(ByVal selectedNode As TreeNode)
                If selectedNode Is Nothing Then Return

                Try
                    cMenTree.Enabled = True
                    EnableMenuItemsRecursive(cMenTree.Items)

                    Select Case mRemoteNG.Tree.Node.GetNodeType(selectedNode)
                        Case mRemoteNG.Tree.Node.Type.Connection
                            Dim connectionInfo As mRemoteNG.Connection.Info = selectedNode.Tag

                            If connectionInfo.OpenConnections.Count = 0 Then
                                cMenTreeDisconnect.Enabled = False
                            End If

                            If Not (connectionInfo.Protocol = mRemoteNG.Connection.Protocol.Protocols.SSH1 Or _
                                    connectionInfo.Protocol = mRemoteNG.Connection.Protocol.Protocols.SSH2) Then
                                cMenTreeToolsTransferFile.Enabled = False
                            End If

                            If Not (connectionInfo.Protocol = mRemoteNG.Connection.Protocol.Protocols.RDP Or _
                                    connectionInfo.Protocol = mRemoteNG.Connection.Protocol.Protocols.ICA) Then
                                cMenTreeConnectWithOptionsConnectInFullscreen.Enabled = False
                                cMenTreeConnectWithOptionsConnectToConsoleSession.Enabled = False
                            End If

                            If (connectionInfo.Protocol = mRemoteNG.Connection.Protocol.Protocols.IntApp) Then
                                cMenTreeConnectWithOptionsNoCredentials.Enabled = False
                            End If
                        Case mRemoteNG.Tree.Node.Type.PuttySession
                            Dim puttySessionInfo As mRemoteNG.Connection.PuttySession.Info = selectedNode.Tag

                            cMenTreeAddConnection.Enabled = False
                            cMenTreeAddFolder.Enabled = False

                            If puttySessionInfo.OpenConnections.Count = 0 Then
                                cMenTreeDisconnect.Enabled = False
                            End If

                            If Not (puttySessionInfo.Protocol = mRemoteNG.Connection.Protocol.Protocols.SSH1 Or _
                                    puttySessionInfo.Protocol = mRemoteNG.Connection.Protocol.Protocols.SSH2) Then
                                cMenTreeToolsTransferFile.Enabled = False
                            End If

                            cMenTreeConnectWithOptionsConnectInFullscreen.Enabled = False
                            cMenTreeConnectWithOptionsConnectToConsoleSession.Enabled = False
                            cMenTreeToolsSort.Enabled = False
                            cMenTreeDuplicate.Enabled = False
                            cMenTreeRename.Enabled = False
                            cMenTreeDelete.Enabled = False
                            cMenTreeMoveUp.Enabled = False
                            cMenTreeMoveDown.Enabled = False
                        Case mRemoteNG.Tree.Node.Type.Container
                            cMenTreeConnectWithOptionsConnectInFullscreen.Enabled = False
                            cMenTreeConnectWithOptionsConnectToConsoleSession.Enabled = False
                            cMenTreeDisconnect.Enabled = False

                            Dim openConnections As Integer = 0
                            Dim connectionInfo As mRemoteNG.Connection.Info
                            For Each node As TreeNode In selectedNode.Nodes
                                If TypeOf node.Tag Is mRemoteNG.Connection.Info Then
                                    connectionInfo = node.Tag
                                    openConnections = openConnections + connectionInfo.OpenConnections.Count
                                End If
                            Next
                            If openConnections = 0 Then
                                cMenTreeDisconnect.Enabled = False
                            End If

                            cMenTreeToolsTransferFile.Enabled = False
                            cMenTreeToolsExternalApps.Enabled = False
                        Case mRemoteNG.Tree.Node.Type.Root
                            cMenTreeConnect.Enabled = False
                            cMenTreeConnectWithOptions.Enabled = False
                            cMenTreeConnectWithOptionsConnectInFullscreen.Enabled = False
                            cMenTreeConnectWithOptionsConnectToConsoleSession.Enabled = False
                            cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Enabled = False
                            cMenTreeDisconnect.Enabled = False
                            cMenTreeToolsTransferFile.Enabled = False
                            cMenTreeToolsExternalApps.Enabled = False
                            cMenTreeDuplicate.Enabled = False
                            cMenTreeDelete.Enabled = False
                            cMenTreeMoveUp.Enabled = False
                            cMenTreeMoveDown.Enabled = False
                        Case mRemoteNG.Tree.Node.Type.PuttyRoot
                            cMenTreeAddConnection.Enabled = False
                            cMenTreeAddFolder.Enabled = False
                            cMenTreeConnect.Enabled = False
                            cMenTreeConnectWithOptions.Enabled = False
                            cMenTreeDisconnect.Enabled = False
                            cMenTreeToolsTransferFile.Enabled = False
                            cMenTreeConnectWithOptions.Enabled = False
                            cMenTreeToolsSort.Enabled = False
                            cMenTreeToolsExternalApps.Enabled = False
                            cMenTreeDuplicate.Enabled = False
                            cMenTreeRename.Enabled = True
                            cMenTreeDelete.Enabled = False
                            cMenTreeMoveUp.Enabled = False
                            cMenTreeMoveDown.Enabled = False
                        Case Else
                            cMenTree.Enabled = False
                    End Select
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "ShowHideTreeContextMenuItems (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

#Region "Drag and Drop"
            Private Shared Sub tvConnections_DragDrop(ByVal sender As Object, ByVal e As DragEventArgs) Handles tvConnections.DragDrop
                Try
                    'Check that there is a TreeNode being dragged
                    If e.Data.GetDataPresent("System.Windows.Forms.TreeNode", True) = False Then Exit Sub

                    'Get the TreeView raising the event (in case multiple on form)
                    Dim selectedTreeview As TreeView = CType(sender, TreeView)

                    'Get the TreeNode being dragged
                    Dim dropNode As TreeNode = CType(e.Data.GetData("System.Windows.Forms.TreeNode"), TreeNode)

                    'The target node should be selected from the DragOver event
                    Dim targetNode As TreeNode = selectedTreeview.SelectedNode

                    If dropNode Is targetNode Then
                        Exit Sub
                    End If

                    If mRemoteNG.Tree.Node.GetNodeType(dropNode) = mRemoteNG.Tree.Node.Type.Root Then
                        Exit Sub
                    End If

                    If dropNode Is targetNode.Parent Then
                        Exit Sub
                    End If

                    'Remove the drop node from its current location
                    dropNode.Remove()

                    'If there is no targetNode add dropNode to the bottom of
                    'the TreeView root nodes, otherwise add it to the end of
                    'the dropNode child nodes

                    If mRemoteNG.Tree.Node.GetNodeType(targetNode) = mRemoteNG.Tree.Node.Type.Root Or mRemoteNG.Tree.Node.GetNodeType(targetNode) = mRemoteNG.Tree.Node.Type.Container Then
                        targetNode.Nodes.Insert(0, dropNode)
                    Else
                        targetNode.Parent.Nodes.Insert(targetNode.Index + 1, dropNode)
                    End If

                    If mRemoteNG.Tree.Node.GetNodeType(dropNode) = mRemoteNG.Tree.Node.Type.Connection Or mRemoteNG.Tree.Node.GetNodeType(dropNode) = mRemoteNG.Tree.Node.Type.Container Then
                        If mRemoteNG.Tree.Node.GetNodeType(dropNode.Parent) = mRemoteNG.Tree.Node.Type.Container Then
                            dropNode.Tag.Parent = dropNode.Parent.Tag
                        ElseIf mRemoteNG.Tree.Node.GetNodeType(dropNode.Parent) = mRemoteNG.Tree.Node.Type.Root Then
                            dropNode.Tag.Parent = Nothing
                            If mRemoteNG.Tree.Node.GetNodeType(dropNode) = mRemoteNG.Tree.Node.Type.Connection Then
                                dropNode.Tag.Inherit.TurnOffInheritanceCompletely()
                            ElseIf mRemoteNG.Tree.Node.GetNodeType(dropNode) = mRemoteNG.Tree.Node.Type.Container Then
                                dropNode.Tag.ConnectionInfo.Inherit.TurnOffInheritanceCompletely()
                            End If
                        End If
                    End If

                    'Ensure the newly created node is visible to
                    'the user and select it
                    dropNode.EnsureVisible()
                    selectedTreeview.SelectedNode = dropNode

                    SaveConnectionsBG()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "tvConnections_DragDrop (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Shared Sub tvConnections_DragEnter(ByVal sender As Object, ByVal e As DragEventArgs) Handles tvConnections.DragEnter
                Try
                    'See if there is a TreeNode being dragged
                    If e.Data.GetDataPresent("System.Windows.Forms.TreeNode", True) Then
                        'TreeNode found allow move effect
                        e.Effect = DragDropEffects.Move
                    Else
                        'No TreeNode found, prevent move
                        e.Effect = DragDropEffects.None
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "tvConnections_DragEnter (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Shared Sub tvConnections_DragOver(ByVal sender As Object, ByVal e As DragEventArgs) Handles tvConnections.DragOver
                Try
                    'Check that there is a TreeNode being dragged 
                    If e.Data.GetDataPresent("System.Windows.Forms.TreeNode", True) = False Then Exit Sub

                    'Get the TreeView raising the event (in case multiple on form)
                    Dim selectedTreeview As TreeView = CType(sender, TreeView)

                    'As the mouse moves over nodes, provide feedback to 
                    'the user by highlighting the node that is the 
                    'current drop target
                    Dim pt As Point = CType(sender, TreeView).PointToClient(New Point(e.X, e.Y))
                    Dim targetNode As TreeNode = selectedTreeview.GetNodeAt(pt)

                    'Select the node currently under the cursor
                    selectedTreeview.SelectedNode = targetNode

                    'Check that the selected node is not the dropNode and
                    'also that it is not a child of the dropNode and 
                    'therefore an invalid target
                    Dim dropNode As TreeNode = CType(e.Data.GetData("System.Windows.Forms.TreeNode"), TreeNode)

                    Dim puttyRootInfo As Root.PuttySessions.Info
                    Do Until targetNode Is Nothing
                        puttyRootInfo = TryCast(targetNode.Tag, Root.PuttySessions.Info)
                        If puttyRootInfo IsNot Nothing Or targetNode Is dropNode Then
                            e.Effect = DragDropEffects.None
                            Return
                        End If
                        targetNode = targetNode.Parent
                    Loop

                    'Currently selected node is a suitable target
                    e.Effect = DragDropEffects.Move
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "tvConnections_DragOver (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub tvConnections_ItemDrag(ByVal sender As Object, ByVal e As ItemDragEventArgs) Handles tvConnections.ItemDrag
                Try
                    Dim dragTreeNode As TreeNode = TryCast(e.Item, TreeNode)
                    If dragTreeNode Is Nothing Then Return

                    If dragTreeNode.Tag Is Nothing Then Return
                    If TypeOf dragTreeNode.Tag Is mRemoteNG.Connection.PuttySession.Info Or _
                       Not (TypeOf dragTreeNode.Tag Is mRemoteNG.Connection.Info Or _
                            TypeOf dragTreeNode.Tag Is Container.Info) Then
                        tvConnections.SelectedNode = dragTreeNode
                        Return
                    End If

                    'Set the drag node and initiate the DragDrop 
                    DoDragDrop(e.Item, DragDropEffects.Move)
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "tvConnections_ItemDrag (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

#Region "Tree Context Menu"
            Private Sub cMenTreeAddConnection_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cMenTreeAddConnection.Click, mMenAddConnection.Click
                AddConnection()
                SaveConnectionsBG()
            End Sub

            Private Sub cMenTreeAddFolder_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cMenTreeAddFolder.Click, mMenAddFolder.Click
                AddFolder()
                SaveConnectionsBG()
            End Sub

            Private Shared Sub cMenTreeConnect_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cMenTreeConnect.Click
                OpenConnection(mRemoteNG.Connection.Info.Force.DoNotJump)
            End Sub

            Private Shared Sub cMenTreeConnectWithOptionsConnectToConsoleSession_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cMenTreeConnectWithOptionsConnectToConsoleSession.Click
                OpenConnection(mRemoteNG.Connection.Info.Force.UseConsoleSession Or mRemoteNG.Connection.Info.Force.DoNotJump)
            End Sub

            Private Shared Sub cMenTreeConnectWithOptionsNoCredentials_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cMenTreeConnectWithOptionsNoCredentials.Click
                OpenConnection(mRemoteNG.Connection.Info.Force.NoCredentials)
            End Sub

            Private Shared Sub cMenTreeConnectWithOptionsDontConnectToConsoleSession_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cMenTreeConnectWithOptionsDontConnectToConsoleSession.Click
                OpenConnection(mRemoteNG.Connection.Info.Force.DontUseConsoleSession Or mRemoteNG.Connection.Info.Force.DoNotJump)
            End Sub

            Private Shared Sub cMenTreeConnectWithOptionsConnectInFullscreen_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cMenTreeConnectWithOptionsConnectInFullscreen.Click
                OpenConnection(mRemoteNG.Connection.Info.Force.Fullscreen Or mRemoteNG.Connection.Info.Force.DoNotJump)
            End Sub

            Private Shared Sub cMenTreeConnectWithOptionsChoosePanelBeforeConnecting_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Click
                OpenConnection(mRemoteNG.Connection.Info.Force.OverridePanel Or mRemoteNG.Connection.Info.Force.DoNotJump)
            End Sub

            Private Sub cMenTreeDisconnect_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cMenTreeDisconnect.Click
                DisconnectConnection()
            End Sub

            Private Shared Sub cMenTreeToolsTransferFile_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cMenTreeToolsTransferFile.Click
                SshTransferFile()
            End Sub

            Private Sub mMenSortAscending_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles mMenSortAscending.Click
                tvConnections.BeginUpdate()
                mRemoteNG.Tree.Node.Sort(tvConnections.Nodes.Item(0), SortOrder.Ascending)
                tvConnections.EndUpdate()
                SaveConnectionsBG()
            End Sub

            Private Sub cMenTreeToolsSortAscending_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cMenTreeToolsSortAscending.Click
                tvConnections.BeginUpdate()
                mRemoteNG.Tree.Node.Sort(tvConnections.SelectedNode, SortOrder.Ascending)
                tvConnections.EndUpdate()
                SaveConnectionsBG()
            End Sub

            Private Sub cMenTreeToolsSortDescending_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cMenTreeToolsSortDescending.Click
                tvConnections.BeginUpdate()
                mRemoteNG.Tree.Node.Sort(tvConnections.SelectedNode, SortOrder.Descending)
                tvConnections.EndUpdate()
                SaveConnectionsBG()
            End Sub

            Private Sub cMenTree_DropDownOpening(ByVal sender As Object, ByVal e As EventArgs) Handles cMenTree.Opening
                AddExternalApps()
            End Sub

            Private Shared Sub cMenTreeToolsExternalAppsEntry_Click(ByVal sender As Object, ByVal e As EventArgs)
                StartExternalApp(sender.Tag)
            End Sub

            Private Sub cMenTreeDuplicate_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cMenTreeDuplicate.Click
                mRemoteNG.Tree.Node.CloneNode(tvConnections.SelectedNode)
                SaveConnectionsBG()
            End Sub

            Private Shared Sub cMenTreeRename_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cMenTreeRename.Click
                mRemoteNG.Tree.Node.StartRenameSelectedNode()
                SaveConnectionsBG()
            End Sub

            Private Shared Sub cMenTreeDelete_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cMenTreeDelete.Click
                mRemoteNG.Tree.Node.DeleteSelectedNode()
                SaveConnectionsBG()
            End Sub

            Private Shared Sub cMenTreeImportFile_Click(sender As System.Object, e As EventArgs) Handles cMenTreeImportFile.Click
                Import.ImportFromFile(Windows.treeForm.tvConnections.Nodes(0), Windows.treeForm.tvConnections.SelectedNode, True)
            End Sub

            Private Shared Sub cMenTreeImportActiveDirectory_Click(sender As System.Object, e As EventArgs) Handles cMenTreeImportActiveDirectory.Click
                Windows.Show(Type.ActiveDirectoryImport)
            End Sub

            Private Shared Sub cMenTreeImportPortScan_Click(sender As System.Object, e As EventArgs) Handles cMenTreeImportPortScan.Click
                Windows.Show(UI.Window.Type.PortScan, True)
            End Sub

            Private Shared Sub cMenTreeExportFile_Click(sender As System.Object, e As EventArgs) Handles cMenTreeExportFile.Click
                Export.ExportToFile(Windows.treeForm.tvConnections.Nodes(0), Windows.treeForm.tvConnections.SelectedNode)
            End Sub
            Private Shared Sub cMenTreeMoveUp_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cMenTreeMoveUp.Click
                mRemoteNG.Tree.Node.MoveNodeUp()
                SaveConnectionsBG()
            End Sub

            Private Shared Sub cMenTreeMoveDown_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cMenTreeMoveDown.Click
                mRemoteNG.Tree.Node.MoveNodeDown()
                SaveConnectionsBG()
            End Sub
#End Region

#Region "Context Menu Actions"
            Public Sub AddConnection()
                Try
                    If tvConnections.SelectedNode Is Nothing Then tvConnections.SelectedNode = tvConnections.Nodes.Item(0)

                    Dim newTreeNode As TreeNode = mRemoteNG.Tree.Node.AddNode(mRemoteNG.Tree.Node.Type.Connection)
                    If newTreeNode Is Nothing Then
                        MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "UI.Window.Tree.AddConnection() failed." & vbNewLine & "mRemoteNG.Tree.Node.AddNode() returned Nothing.", True)
                        Return
                    End If

                    Dim containerNode As TreeNode = tvConnections.SelectedNode
                    If mRemoteNG.Tree.Node.GetNodeType(containerNode) = mRemoteNG.Tree.Node.Type.Connection Then
                        containerNode = containerNode.Parent
                    End If

                    Dim newConnectionInfo As New mRemoteNG.Connection.Info()
                    If mRemoteNG.Tree.Node.GetNodeType(containerNode) = mRemoteNG.Tree.Node.Type.Root Then
                        newConnectionInfo.Inherit.TurnOffInheritanceCompletely()
                    Else
                        newConnectionInfo.Parent = containerNode.Tag
                    End If

                    newConnectionInfo.TreeNode = newTreeNode
                    newTreeNode.Tag = newConnectionInfo
                    ConnectionList.Add(newConnectionInfo)

                    containerNode.Nodes.Add(newTreeNode)

                    tvConnections.SelectedNode = newTreeNode
                    tvConnections.SelectedNode.BeginEdit()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "UI.Window.Tree.AddConnection() failed." & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Sub AddFolder()
                Try
                    Dim newNode As TreeNode = mRemoteNG.Tree.Node.AddNode(mRemoteNG.Tree.Node.Type.Container)
                    Dim newContainerInfo As New Container.Info()
                    newNode.Tag = newContainerInfo
                    newContainerInfo.TreeNode = newNode

                    Dim selectedNode As TreeNode = mRemoteNG.Tree.Node.SelectedNode
                    Dim parentNode As TreeNode
                    If selectedNode Is Nothing Then
                        parentNode = tvConnections.Nodes(0)
                    Else
                        If mRemoteNG.Tree.Node.GetNodeType(selectedNode) = mRemoteNG.Tree.Node.Type.Connection Then
                            parentNode = selectedNode.Parent
                        Else
                            parentNode = selectedNode
                        End If
                    End If

                    newContainerInfo.ConnectionInfo = New mRemoteNG.Connection.Info(newContainerInfo)
                    newContainerInfo.ConnectionInfo.Name = newNode.Text

                    ' We can only inherit from a container node, not the root node or connection nodes
                    If mRemoteNG.Tree.Node.GetNodeType(parentNode) = mRemoteNG.Tree.Node.Type.Container Then
                        newContainerInfo.Parent = parentNode.Tag
                    Else
                        newContainerInfo.ConnectionInfo.Inherit.TurnOffInheritanceCompletely()
                    End If

                    ContainerList.Add(newContainerInfo)
                    parentNode.Nodes.Add(newNode)

                    tvConnections.SelectedNode = newNode
                    tvConnections.SelectedNode.BeginEdit()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, String.Format(Language.strErrorAddFolderFailed, ex.Message), True)
                End Try
            End Sub

            Private Sub DisconnectConnection()
                Try
                    If tvConnections.SelectedNode IsNot Nothing Then
                        If TypeOf tvConnections.SelectedNode.Tag Is mRemoteNG.Connection.Info Then
                            Dim conI As mRemoteNG.Connection.Info = tvConnections.SelectedNode.Tag
                            For i As Integer = 0 To conI.OpenConnections.Count - 1
                                conI.OpenConnections(i).Disconnect()
                            Next
                        End If

                        If TypeOf tvConnections.SelectedNode.Tag Is Container.Info Then
                            For Each n As TreeNode In tvConnections.SelectedNode.Nodes
                                If TypeOf n.Tag Is mRemoteNG.Connection.Info Then
                                    Dim conI As mRemoteNG.Connection.Info = n.Tag
                                    For i As Integer = 0 To conI.OpenConnections.Count - 1
                                        conI.OpenConnections(i).Disconnect()
                                    Next
                                End If
                            Next
                        End If
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "DisconnectConnection (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Shared Sub SshTransferFile()
                Try
                    Windows.Show(Type.SSHTransfer)

                    Dim conI As mRemoteNG.Connection.Info = mRemoteNG.Tree.Node.SelectedNode.Tag

                    Windows.sshtransferForm.Hostname = conI.Hostname
                    Windows.sshtransferForm.Username = conI.Username
                    Windows.sshtransferForm.Password = conI.Password
                    Windows.sshtransferForm.Port = conI.Port
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "SSHTransferFile (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub AddExternalApps()
                Try
                    'clean up
                    cMenTreeToolsExternalApps.DropDownItems.Clear()

                    'add ext apps
                    For Each extA As Tools.ExternalTool In Runtime.ExternalTools
                        Dim nItem As New ToolStripMenuItem
                        nItem.Text = extA.DisplayName
                        nItem.Tag = extA

                        nItem.Image = extA.Image

                        AddHandler nItem.Click, AddressOf cMenTreeToolsExternalAppsEntry_Click

                        cMenTreeToolsExternalApps.DropDownItems.Add(nItem)
                    Next
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "cMenTreeTools_DropDownOpening failed (UI.Window.Tree)" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Shared Sub StartExternalApp(ByVal externalTool As Tools.ExternalTool)
                Try
                    If mRemoteNG.Tree.Node.GetNodeType(mRemoteNG.Tree.Node.SelectedNode) = mRemoteNG.Tree.Node.Type.Connection Or _
                       mRemoteNG.Tree.Node.GetNodeType(mRemoteNG.Tree.Node.SelectedNode) = mRemoteNG.Tree.Node.Type.PuttySession Then
                        externalTool.Start(mRemoteNG.Tree.Node.SelectedNode.Tag)
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "cMenTreeToolsExternalAppsEntry_Click failed (UI.Window.Tree)" & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

#Region "Menu"
            Private Shared Sub mMenViewExpandAllFolders_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles mMenViewExpandAllFolders.Click
                mRemoteNG.Tree.Node.ExpandAllNodes()
            End Sub

            Private Sub mMenViewCollapseAllFolders_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles mMenViewCollapseAllFolders.Click
                If tvConnections.SelectedNode IsNot Nothing Then
                    If tvConnections.SelectedNode.IsEditing Then tvConnections.SelectedNode.EndEdit(False)
                End If
                mRemoteNG.Tree.Node.CollapseAllNodes()
            End Sub
#End Region

#Region "Search"
            Private Sub txtSearch_GotFocus(ByVal sender As Object, ByVal e As EventArgs) Handles txtSearch.GotFocus
                txtSearch.ForeColor = Themes.ThemeManager.ActiveTheme.SearchBoxTextColor
                If txtSearch.Text = Language.strSearchPrompt Then
                    txtSearch.Text = ""
                End If
            End Sub

            Private Sub txtSearch_LostFocus(ByVal sender As Object, ByVal e As EventArgs) Handles txtSearch.LostFocus
                If txtSearch.Text = "" Then
                    txtSearch.ForeColor = Themes.ThemeManager.ActiveTheme.SearchBoxTextPromptColor
                    txtSearch.Text = Language.strSearchPrompt
                End If
            End Sub

            Private Sub txtSearch_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles txtSearch.KeyDown
                Try
                    If e.KeyCode = Keys.Escape Then
                        e.Handled = True
                        tvConnections.Focus()
                    ElseIf e.KeyCode = Keys.Up Then
                        tvConnections.SelectedNode = tvConnections.SelectedNode.PrevVisibleNode
                    ElseIf e.KeyCode = Keys.Down Then
                        tvConnections.SelectedNode = tvConnections.SelectedNode.NextVisibleNode
                    Else
                        tvConnections_KeyDown(sender, e)
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "txtSearch_KeyDown (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub txtSearch_TextChanged(ByVal sender As System.Object, ByVal e As EventArgs) Handles txtSearch.TextChanged
                tvConnections.SelectedNode = mRemoteNG.Tree.Node.Find(tvConnections.Nodes(0), txtSearch.Text)
            End Sub

            Private Sub tvConnections_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles tvConnections.KeyPress
                Try
                    If Char.IsLetterOrDigit(e.KeyChar) Then
                        txtSearch.Text = e.KeyChar

                        txtSearch.Focus()
                        txtSearch.SelectionStart = txtSearch.TextLength
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "tvConnections_KeyPress (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub tvConnections_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles tvConnections.KeyDown
                Try
                    If e.KeyCode = Keys.Enter Then
                        If TypeOf tvConnections.SelectedNode.Tag Is mRemoteNG.Connection.Info Then
                            e.Handled = True
                            OpenConnection()
                        Else
                            If tvConnections.SelectedNode.IsExpanded Then
                                tvConnections.SelectedNode.Collapse(True)
                            Else
                                tvConnections.SelectedNode.Expand()
                            End If
                        End If
                    ElseIf e.KeyCode = Keys.Escape Xor e.KeyCode = Keys.Control Or e.KeyCode = Keys.F Then
                        txtSearch.Focus()
                        txtSearch.SelectionStart = txtSearch.TextLength
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "tvConnections_KeyDown (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region
        End Class
    End Namespace
End Namespace