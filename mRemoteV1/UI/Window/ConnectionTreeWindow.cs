using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using mRemoteNG.My;
using mRemoteNG.Tree;
using System;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;


namespace mRemoteNG.UI.Window
{
	public partial class ConnectionTreeWindow : BaseWindow
	{
        #region Form Stuff
		public void Tree_Load(object sender, EventArgs e)
		{
			ApplyLanguage();
					
			Themes.ThemeManager.ThemeChanged += ApplyTheme;
			ApplyTheme();
					
			txtSearch.Multiline = true;
			txtSearch.MinimumSize = new Size(0, 14);
			txtSearch.Size = new Size(txtSearch.Size.Width, 14);
			txtSearch.Multiline = false;
		}
				
		private void ApplyLanguage()
		{
			Text = Language.strConnections;
			TabText = Language.strConnections;
					
			mMenAddConnection.ToolTipText = Language.strAddConnection;
			mMenAddFolder.ToolTipText = Language.strAddFolder;
			mMenView.ToolTipText = Language.strMenuView.Replace("&", "");
			mMenViewExpandAllFolders.Text = Language.strExpandAllFolders;
			mMenViewCollapseAllFolders.Text = Language.strCollapseAllFolders;
			mMenSortAscending.ToolTipText = Language.strSortAsc;
					
			cMenTreeConnect.Text = Language.strConnect;
			cMenTreeConnectWithOptions.Text = Language.strConnectWithOptions;
			cMenTreeConnectWithOptionsConnectToConsoleSession.Text = Language.strConnectToConsoleSession;
			cMenTreeConnectWithOptionsDontConnectToConsoleSession.Text = Language.strDontConnectToConsoleSessionMenuItem;
			cMenTreeConnectWithOptionsConnectInFullscreen.Text = Language.strConnectInFullscreen;
			cMenTreeConnectWithOptionsNoCredentials.Text = Language.strConnectNoCredentials;
			cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Text = Language.strChoosePanelBeforeConnecting;
			cMenTreeDisconnect.Text = Language.strMenuDisconnect;
					
			cMenTreeToolsExternalApps.Text = Language.strMenuExternalTools;
			cMenTreeToolsTransferFile.Text = Language.strMenuTransferFile;
					
			cMenTreeDuplicate.Text = Language.strDuplicate;
			cMenTreeRename.Text = Language.strRename;
			cMenTreeDelete.Text = Language.strMenuDelete;
					
			cMenTreeImport.Text = Language.strImportMenuItem;
			cMenTreeImportFile.Text = Language.strImportFromFileMenuItem;
			cMenTreeImportActiveDirectory.Text = Language.strImportAD;
			cMenTreeImportPortScan.Text = Language.strImportPortScan;
			cMenTreeExportFile.Text = Language.strExportToFileMenuItem;
					
			cMenTreeAddConnection.Text = Language.strAddConnection;
			cMenTreeAddFolder.Text = Language.strAddFolder;
					
			cMenTreeToolsSort.Text = Language.strSort;
			cMenTreeToolsSortAscending.Text = Language.strSortAsc;
			cMenTreeToolsSortDescending.Text = Language.strSortDesc;
			cMenTreeMoveUp.Text = Language.strMoveUp;
			cMenTreeMoveDown.Text = Language.strMoveDown;
					
			txtSearch.Text = Language.strSearchPrompt;
		}
				
		public void ApplyTheme()
		{
			msMain.BackColor = Themes.ThemeManager.ActiveTheme.ToolbarBackgroundColor;
			msMain.ForeColor = Themes.ThemeManager.ActiveTheme.ToolbarTextColor;
			tvConnections.BackColor = Themes.ThemeManager.ActiveTheme.ConnectionsPanelBackgroundColor;
			tvConnections.ForeColor = Themes.ThemeManager.ActiveTheme.ConnectionsPanelTextColor;
			tvConnections.LineColor = Themes.ThemeManager.ActiveTheme.ConnectionsPanelTreeLineColor;
			BackColor = Themes.ThemeManager.ActiveTheme.ToolbarBackgroundColor;
			txtSearch.BackColor = Themes.ThemeManager.ActiveTheme.SearchBoxBackgroundColor;
			txtSearch.ForeColor = Themes.ThemeManager.ActiveTheme.SearchBoxTextPromptColor;
		}
        #endregion
		
        #region Public Methods
		public ConnectionTreeWindow(DockContent panel)
		{
			WindowType = WindowType.Tree;
			DockPnl = panel;
			InitializeComponent();
			FillImageList();
					
			DescriptionTooltip = new ToolTip();
			DescriptionTooltip.InitialDelay = 300;
			DescriptionTooltip.ReshowDelay = 0;
		}
				
		public void InitialRefresh()
		{
			tvConnections_AfterSelect(tvConnections, new TreeViewEventArgs(tvConnections.SelectedNode, TreeViewAction.ByMouse));
		}
        #endregion
		
        #region Public Properties
		public ToolTip DescriptionTooltip {get; set;}
        #endregion
		
        #region Private Methods
		private void FillImageList()
		{
			try
			{
				imgListTree.Images.Add(Resources.Root);
				imgListTree.Images.Add(Resources.Folder);
				imgListTree.Images.Add(Resources.Play);
				imgListTree.Images.Add(Resources.Pause);
				imgListTree.Images.Add(Resources.PuttySessions);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "FillImageList (UI.Window.Tree) failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		public void tvConnections_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			cMenTreeDelete.ShortcutKeys = Keys.None;
		}
				
		public void tvConnections_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			try
			{
				cMenTreeDelete.ShortcutKeys = Keys.Delete;

                ConnectionTree.FinishRenameSelectedNode(e.Label);
                Windows.configForm.pGrid_SelectedObjectChanged();
				ShowHideTreeContextMenuItems(e.Node);
                Runtime.SaveConnectionsBG();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "tvConnections_AfterLabelEdit (UI.Window.Tree) failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		public void tvConnections_AfterSelect(object sender, TreeViewEventArgs e)
		{
			try
			{
				if ((ConnectionTreeNode.GetNodeType(e.Node) == TreeNodeType.Connection) || (ConnectionTreeNode.GetNodeType(e.Node) == TreeNodeType.PuttySession))
				{
                    Windows.configForm.SetPropertyGridObject(e.Node.Tag);
				}
				else if (ConnectionTreeNode.GetNodeType(e.Node) == TreeNodeType.Container)
				{
                    Windows.configForm.SetPropertyGridObject((e.Node.Tag as ContainerInfo).ConnectionInfo);
				}
				else if ((ConnectionTreeNode.GetNodeType(e.Node) == TreeNodeType.Root) || (ConnectionTreeNode.GetNodeType(e.Node) == TreeNodeType.PuttyRoot))
				{
                    Windows.configForm.SetPropertyGridObject(e.Node.Tag);
				}
				else
				{
					return;
				}

                Windows.configForm.pGrid_SelectedObjectChanged();
				ShowHideTreeContextMenuItems(e.Node);

                Runtime.LastSelected = ConnectionTreeNode.GetConstantID(e.Node);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "tvConnections_AfterSelect (UI.Window.Tree) failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		public void tvConnections_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			try
			{
				ShowHideTreeContextMenuItems(tvConnections.SelectedNode);
				tvConnections.SelectedNode = e.Node;
						
				if (e.Button == MouseButtons.Left)
				{
					if (mRemoteNG.Settings.Default.SingleClickOnConnectionOpensIt && 
						(ConnectionTreeNode.GetNodeType(e.Node) == TreeNodeType.Connection |
                        ConnectionTreeNode.GetNodeType(e.Node) == TreeNodeType.PuttySession))
					{
						Runtime.OpenConnection();
					}
							
					if (mRemoteNG.Settings.Default.SingleClickSwitchesToOpenConnection && ConnectionTreeNode.GetNodeType(e.Node) == TreeNodeType.Connection)
					{
                        Runtime.SwitchToOpenConnection((ConnectionInfo)e.Node.Tag);
					}
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "tvConnections_NodeMouseClick (UI.Window.Tree) failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		static public void tvConnections_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
            if (ConnectionTreeNode.GetNodeType(ConnectionTree.SelectedNode) == TreeNodeType.Connection |
                ConnectionTreeNode.GetNodeType(ConnectionTree.SelectedNode) == TreeNodeType.PuttySession)
			{
                Runtime.OpenConnection();
			}
		}
				
		public void tvConnections_MouseMove(object sender, MouseEventArgs e)
		{
			try
			{
                ConnectionTree.SetNodeToolTip(e, DescriptionTooltip);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "tvConnections_MouseMove (UI.Window.Tree) failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		private static void EnableMenuItemsRecursive(ToolStripItemCollection items, bool enable = true)
		{
			ToolStripMenuItem menuItem = default(ToolStripMenuItem);
			foreach (ToolStripItem item in items)
			{
				menuItem = item as ToolStripMenuItem;
				if (menuItem == null)
				{
					continue;
				}
				menuItem.Enabled = enable;
				if (menuItem.HasDropDownItems)
				{
					EnableMenuItemsRecursive(menuItem.DropDownItems, enable);
				}
			}
		}
				
		private void ShowHideTreeContextMenuItems(TreeNode selectedNode)
		{
			if (selectedNode == null)
			{
				return ;
			}
					
			try
			{
				cMenTree.Enabled = true;
				EnableMenuItemsRecursive(cMenTree.Items);
						
				if (ConnectionTreeNode.GetNodeType(selectedNode) == TreeNodeType.Connection)
				{
                    ConnectionInfo connectionInfo = (ConnectionInfo)selectedNode.Tag;
							
					if (connectionInfo.OpenConnections.Count == 0)
					{
						cMenTreeDisconnect.Enabled = false;
					}
							
					if (!(connectionInfo.Protocol == ProtocolType.SSH1 | 
						connectionInfo.Protocol == ProtocolType.SSH2))
					{
						cMenTreeToolsTransferFile.Enabled = false;
					}
							
					if (!(connectionInfo.Protocol == ProtocolType.RDP | connectionInfo.Protocol == ProtocolType.ICA))
					{
						cMenTreeConnectWithOptionsConnectInFullscreen.Enabled = false;
						cMenTreeConnectWithOptionsConnectToConsoleSession.Enabled = false;
					}
							
					if (connectionInfo.Protocol == ProtocolType.IntApp)
					{
						cMenTreeConnectWithOptionsNoCredentials.Enabled = false;
					}
				}
				else if (ConnectionTreeNode.GetNodeType(selectedNode) == TreeNodeType.PuttySession)
				{
                    PuttySessionInfo puttySessionInfo = (PuttySessionInfo)selectedNode.Tag;
							
					cMenTreeAddConnection.Enabled = false;
					cMenTreeAddFolder.Enabled = false;
							
					if (puttySessionInfo.OpenConnections.Count == 0)
					{
						cMenTreeDisconnect.Enabled = false;
					}
							
					if (!(puttySessionInfo.Protocol == ProtocolType.SSH1 | puttySessionInfo.Protocol == ProtocolType.SSH2))
					{
						cMenTreeToolsTransferFile.Enabled = false;
					}
							
					cMenTreeConnectWithOptionsConnectInFullscreen.Enabled = false;
					cMenTreeConnectWithOptionsConnectToConsoleSession.Enabled = false;
					cMenTreeToolsSort.Enabled = false;
					cMenTreeDuplicate.Enabled = false;
					cMenTreeRename.Enabled = false;
					cMenTreeDelete.Enabled = false;
					cMenTreeMoveUp.Enabled = false;
					cMenTreeMoveDown.Enabled = false;
				}
				else if (ConnectionTreeNode.GetNodeType(selectedNode) == TreeNodeType.Container)
				{
					cMenTreeConnectWithOptionsConnectInFullscreen.Enabled = false;
					cMenTreeConnectWithOptionsConnectToConsoleSession.Enabled = false;
					cMenTreeDisconnect.Enabled = false;
							
					int openConnections = 0;
					ConnectionInfo connectionInfo = default(ConnectionInfo);
					foreach (TreeNode node in selectedNode.Nodes)
					{
						if (node.Tag is ConnectionInfo)
						{
                            connectionInfo = (ConnectionInfo)node.Tag;
							openConnections = openConnections + connectionInfo.OpenConnections.Count;
						}
					}
					if (openConnections == 0)
					{
						cMenTreeDisconnect.Enabled = false;
					}
							
					cMenTreeToolsTransferFile.Enabled = false;
					cMenTreeToolsExternalApps.Enabled = false;
				}
				else if (ConnectionTreeNode.GetNodeType(selectedNode) == TreeNodeType.Root)
				{
					cMenTreeConnect.Enabled = false;
					cMenTreeConnectWithOptions.Enabled = false;
					cMenTreeConnectWithOptionsConnectInFullscreen.Enabled = false;
					cMenTreeConnectWithOptionsConnectToConsoleSession.Enabled = false;
					cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Enabled = false;
					cMenTreeDisconnect.Enabled = false;
					cMenTreeToolsTransferFile.Enabled = false;
					cMenTreeToolsExternalApps.Enabled = false;
					cMenTreeDuplicate.Enabled = false;
					cMenTreeDelete.Enabled = false;
					cMenTreeMoveUp.Enabled = false;
					cMenTreeMoveDown.Enabled = false;
				}
				else if (ConnectionTreeNode.GetNodeType(selectedNode) == TreeNodeType.PuttyRoot)
				{
					cMenTreeAddConnection.Enabled = false;
					cMenTreeAddFolder.Enabled = false;
					cMenTreeConnect.Enabled = false;
					cMenTreeConnectWithOptions.Enabled = false;
					cMenTreeDisconnect.Enabled = false;
					cMenTreeToolsTransferFile.Enabled = false;
					cMenTreeConnectWithOptions.Enabled = false;
					cMenTreeToolsSort.Enabled = false;
					cMenTreeToolsExternalApps.Enabled = false;
					cMenTreeDuplicate.Enabled = false;
					cMenTreeRename.Enabled = true;
					cMenTreeDelete.Enabled = false;
					cMenTreeMoveUp.Enabled = false;
					cMenTreeMoveDown.Enabled = false;
				}
				else
				{
					cMenTree.Enabled = false;
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "ShowHideTreeContextMenuItems (UI.Window.Tree) failed" + Environment.NewLine + ex.Message, true);
			}
		}
        #endregion
		
        #region Drag and Drop
		static public void tvConnections_DragDrop(object sender, DragEventArgs e)
		{
			try
            {
                //Check that there is a TreeNode being dragged
                if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", true) == false)
                    return;

                TreeView treeviewThatSentTheEvent = (TreeView)sender;
                TreeNode nodeBeingDragged = (TreeNode)(e.Data.GetData("System.Windows.Forms.TreeNode"));
                TreeNode nodeBeingTargetedByDragOverEvent = treeviewThatSentTheEvent.SelectedNode;

                TreeNodeMover treeNodeMover = new TreeNodeMover(nodeBeingDragged);
                treeNodeMover.MoveNode(nodeBeingTargetedByDragOverEvent);
            }
            catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "tvConnections_DragDrop (UI.Window.Tree) failed" + Environment.NewLine + ex.Message, true);
			}
		}

        

        static public void tvConnections_DragEnter(object sender, DragEventArgs e)
		{
			try
			{
				//See if there is a TreeNode being dragged
				if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", true))
				{
					//TreeNode found allow move effect
					e.Effect = DragDropEffects.Move;
				}
				else
				{
					//No TreeNode found, prevent move
					e.Effect = DragDropEffects.None;
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "tvConnections_DragEnter (UI.Window.Tree) failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		static public void tvConnections_DragOver(object sender, DragEventArgs e)
		{
			try
			{
				//Check that there is a TreeNode being dragged
				if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", true) == false)
				{
					return;
				}
						
				//Get the TreeView raising the event (in case multiple on form)
				TreeView selectedTreeview = (TreeView) sender;
						
				//As the mouse moves over nodes, provide feedback to
				//the user by highlighting the node that is the
				//current drop target
				Point pt = ((TreeView) sender).PointToClient(new Point(e.X, e.Y));
				TreeNode targetNode = selectedTreeview.GetNodeAt(pt);
						
				//Select the node currently under the cursor
				selectedTreeview.SelectedNode = targetNode;
						
				//Check that the selected node is not the dropNode and
				//also that it is not a child of the dropNode and
				//therefore an invalid target
				TreeNode dropNode = (TreeNode) (e.Data.GetData("System.Windows.Forms.TreeNode"));
						
				Root.PuttySessions.PuttySessionsNodeInfo puttyRootInfo = default(Root.PuttySessions.PuttySessionsNodeInfo);
				while (!(targetNode == null))
				{
					puttyRootInfo = targetNode.Tag as Root.PuttySessions.PuttySessionsNodeInfo;
					if (puttyRootInfo != null || targetNode == dropNode)
					{
						e.Effect = DragDropEffects.None;
						return ;
					}
					targetNode = targetNode.Parent;
				}
						
				//Currently selected node is a suitable target
				e.Effect = DragDropEffects.Move;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "tvConnections_DragOver (UI.Window.Tree) failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		public void tvConnections_ItemDrag(object sender, ItemDragEventArgs e)
		{
			try
			{
				TreeNode dragTreeNode = e.Item as TreeNode;
				if (dragTreeNode == null)
				{
					return ;
				}
						
				if (dragTreeNode.Tag == null)
				{
					return ;
				}
				if (dragTreeNode.Tag is PuttySessionInfo|| !(dragTreeNode.Tag is ConnectionInfo|| dragTreeNode.Tag is ContainerInfo))
				{
					tvConnections.SelectedNode = dragTreeNode;
					return ;
				}
						
				//Set the drag node and initiate the DragDrop
				DoDragDrop(e.Item, DragDropEffects.Move);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "tvConnections_ItemDrag (UI.Window.Tree) failed" + Environment.NewLine + ex.Message, true);
			}
		}
        #endregion
		
        #region Tree Context Menu
		public void cMenTreeAddConnection_Click(System.Object sender, EventArgs e)
		{
			AddConnection();
            Runtime.SaveConnectionsBG();
		}
				
		public void cMenTreeAddFolder_Click(System.Object sender, EventArgs e)
		{
			AddFolder();
            Runtime.SaveConnectionsBG();
		}
				
		static public void cMenTreeConnect_Click(System.Object sender, EventArgs e)
		{
            Runtime.OpenConnection(mRemoteNG.Connection.ConnectionInfo.Force.DoNotJump);
		}
				
		static public void cMenTreeConnectWithOptionsConnectToConsoleSession_Click(System.Object sender, EventArgs e)
		{
            Runtime.OpenConnection(mRemoteNG.Connection.ConnectionInfo.Force.UseConsoleSession | mRemoteNG.Connection.ConnectionInfo.Force.DoNotJump);
		}
				
		static public void cMenTreeConnectWithOptionsNoCredentials_Click(System.Object sender, EventArgs e)
		{
            Runtime.OpenConnection(mRemoteNG.Connection.ConnectionInfo.Force.NoCredentials);
		}
				
		static public void cMenTreeConnectWithOptionsDontConnectToConsoleSession_Click(System.Object sender, EventArgs e)
		{
            Runtime.OpenConnection(mRemoteNG.Connection.ConnectionInfo.Force.DontUseConsoleSession | mRemoteNG.Connection.ConnectionInfo.Force.DoNotJump);
		}
				
		static public void cMenTreeConnectWithOptionsConnectInFullscreen_Click(System.Object sender, EventArgs e)
		{
            Runtime.OpenConnection(mRemoteNG.Connection.ConnectionInfo.Force.Fullscreen | mRemoteNG.Connection.ConnectionInfo.Force.DoNotJump);
		}
				
		static public void cMenTreeConnectWithOptionsChoosePanelBeforeConnecting_Click(System.Object sender, EventArgs e)
		{
            Runtime.OpenConnection(mRemoteNG.Connection.ConnectionInfo.Force.OverridePanel | mRemoteNG.Connection.ConnectionInfo.Force.DoNotJump);
		}
				
		public void cMenTreeDisconnect_Click(System.Object sender, EventArgs e)
		{
			DisconnectConnection();
		}
				
		static public void cMenTreeToolsTransferFile_Click(System.Object sender, EventArgs e)
		{
			SshTransferFile();
		}
				
		public void mMenSortAscending_Click(System.Object sender, EventArgs e)
		{
			tvConnections.BeginUpdate();
            ConnectionTree.Sort(tvConnections.Nodes[0], SortOrder.Ascending);
			tvConnections.EndUpdate();
            Runtime.SaveConnectionsBG();
		}
				
		public void cMenTreeToolsSortAscending_Click(System.Object sender, EventArgs e)
		{
			tvConnections.BeginUpdate();
            ConnectionTree.Sort(tvConnections.SelectedNode, SortOrder.Ascending);
			tvConnections.EndUpdate();
            Runtime.SaveConnectionsBG();
		}
				
		public void cMenTreeToolsSortDescending_Click(System.Object sender, EventArgs e)
		{
			tvConnections.BeginUpdate();
            ConnectionTree.Sort(tvConnections.SelectedNode, SortOrder.Descending);
			tvConnections.EndUpdate();
            Runtime.SaveConnectionsBG();
		}
				
		public void cMenTree_DropDownOpening(object sender, EventArgs e)
		{
			AddExternalApps();
		}
				
		private static void cMenTreeToolsExternalAppsEntry_Click(object sender, EventArgs e)
		{
			StartExternalApp((mRemoteNG.Tools.ExternalTool)((System.Windows.Forms.ToolStripMenuItem)sender).Tag);
		}
				
		public void cMenTreeDuplicate_Click(System.Object sender, EventArgs e)
		{
            ConnectionTreeNode.CloneNode(tvConnections.SelectedNode);
            Runtime.SaveConnectionsBG();
		}
				
		static public void cMenTreeRename_Click(System.Object sender, EventArgs e)
		{
            ConnectionTree.StartRenameSelectedNode();
            Runtime.SaveConnectionsBG();
		}
				
		static public void cMenTreeDelete_Click(System.Object sender, EventArgs e)
		{
            ConnectionTree.DeleteSelectedNode();
            Runtime.SaveConnectionsBG();
		}
				
		static public void cMenTreeImportFile_Click(System.Object sender, EventArgs e)
		{
            Import.ImportFromFile(Windows.treeForm.tvConnections.Nodes[0], Windows.treeForm.tvConnections.SelectedNode, true);
		}
				
		static public void cMenTreeImportActiveDirectory_Click(System.Object sender, EventArgs e)
		{
            Windows.Show(WindowType.ActiveDirectoryImport);
		}
				
		static public void cMenTreeImportPortScan_Click(System.Object sender, EventArgs e)
		{
            Windows.Show(UI.Window.WindowType.PortScan, true);
		}
				
		static public void cMenTreeExportFile_Click(System.Object sender, EventArgs e)
		{
            Export.ExportToFile(Windows.treeForm.tvConnections.Nodes[0], Windows.treeForm.tvConnections.SelectedNode);
		}
		static public void cMenTreeMoveUp_Click(System.Object sender, EventArgs e)
		{
            ConnectionTree.MoveNodeUp();
            Runtime.SaveConnectionsBG();
		}
				
		static public void cMenTreeMoveDown_Click(System.Object sender, EventArgs e)
		{
            ConnectionTree.MoveNodeDown();
            Runtime.SaveConnectionsBG();
		}
        #endregion
		
        #region Context Menu Actions
		public void AddConnection()
		{
			try
			{
				if (tvConnections.SelectedNode == null)
				{
					tvConnections.SelectedNode = tvConnections.Nodes[0];
				}
						
				TreeNode newTreeNode = ConnectionTreeNode.AddNode(TreeNodeType.Connection);
				if (newTreeNode == null)
				{
					Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "UI.Window.Tree.AddConnection() failed." + Environment.NewLine + "mRemoteNG.Tree.Node.AddNode() returned Nothing.", true);
					return ;
				}
						
				TreeNode containerNode = tvConnections.SelectedNode;
				if (ConnectionTreeNode.GetNodeType(containerNode) == TreeNodeType.Connection)
				{
					containerNode = containerNode.Parent;
				}

                ConnectionInfo newConnectionInfo = new ConnectionInfo();
				if (ConnectionTreeNode.GetNodeType(containerNode) == TreeNodeType.Root)
				{
					newConnectionInfo.Inheritance.DisableInheritance();
				}
				else
				{
                    newConnectionInfo.Parent = (ContainerInfo)containerNode.Tag;
				}
						
				newConnectionInfo.TreeNode = newTreeNode;
				newTreeNode.Tag = newConnectionInfo;
                Runtime.ConnectionList.Add(newConnectionInfo);
						
				containerNode.Nodes.Add(newTreeNode);
						
				tvConnections.SelectedNode = newTreeNode;
				tvConnections.SelectedNode.BeginEdit();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "UI.Window.Tree.AddConnection() failed." + Environment.NewLine + ex.Message, true);
			}
		}
				
		public void AddFolder()
		{
			try
			{
				TreeNode newNode = ConnectionTreeNode.AddNode(TreeNodeType.Container);
				ContainerInfo newContainerInfo = new ContainerInfo();
				newNode.Tag = newContainerInfo;
				newContainerInfo.TreeNode = newNode;

                TreeNode selectedNode = ConnectionTree.SelectedNode;
				TreeNode parentNode = default(TreeNode);
				if (selectedNode == null)
				{
					parentNode = tvConnections.Nodes[0];
				}
				else
				{
					if (ConnectionTreeNode.GetNodeType(selectedNode) == TreeNodeType.Connection)
						parentNode = selectedNode.Parent;
					else
						parentNode = selectedNode;
				}
						
				newContainerInfo.ConnectionInfo = new ConnectionInfo(newContainerInfo);
				newContainerInfo.ConnectionInfo.Name = newNode.Text;
						
				// We can only inherit from a container node, not the root node or connection nodes
				if (ConnectionTreeNode.GetNodeType(parentNode) == TreeNodeType.Container)
				{
					newContainerInfo.Parent = (ContainerInfo)parentNode.Tag;
				}
				else
				{
					newContainerInfo.ConnectionInfo.Inheritance.DisableInheritance();
				}

                Runtime.ContainerList.Add(newContainerInfo);
				parentNode.Nodes.Add(newNode);
						
				tvConnections.SelectedNode = newNode;
				tvConnections.SelectedNode.BeginEdit();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, string.Format(Language.strErrorAddFolderFailed, ex.Message), true);
			}
		}
				
		private void DisconnectConnection()
		{
			try
			{
				if (tvConnections.SelectedNode != null)
				{
					if (tvConnections.SelectedNode.Tag is ConnectionInfo)
					{
                        ConnectionInfo conI = (ConnectionInfo)tvConnections.SelectedNode.Tag;
						for (int i = 0; i <= conI.OpenConnections.Count - 1; i++)
						{
							conI.OpenConnections[i].Disconnect();
						}
					}
							
					if (tvConnections.SelectedNode.Tag is ContainerInfo)
					{
						foreach (TreeNode n in tvConnections.SelectedNode.Nodes)
						{
							if (n.Tag is ConnectionInfo)
							{
                                ConnectionInfo conI = (ConnectionInfo)n.Tag;
								for (int i = 0; i <= conI.OpenConnections.Count - 1; i++)
								{
									conI.OpenConnections[i].Disconnect();
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "DisconnectConnection (UI.Window.Tree) failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		private static void SshTransferFile()
		{
			try
			{
                Windows.Show(WindowType.SSHTransfer);

                ConnectionInfo conI = (ConnectionInfo)ConnectionTree.SelectedNode.Tag;

                Windows.sshtransferForm.Hostname = conI.Hostname;
                Windows.sshtransferForm.Username = conI.Username;
                Windows.sshtransferForm.Password = conI.Password;
                Windows.sshtransferForm.Port = Convert.ToString(conI.Port);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "SSHTransferFile (UI.Window.Tree) failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void AddExternalApps()
		{
			try
			{
                //clean up
                //since new items are added below, we have to dispose of any previous items first
                if (cMenTreeToolsExternalApps.DropDownItems.Count > 0)
                {
                    for (int i = cMenTreeToolsExternalApps.DropDownItems.Count - 1; i >= 0; i--)
                        cMenTreeToolsExternalApps.DropDownItems[i].Dispose();

                    cMenTreeToolsExternalApps.DropDownItems.Clear();
                }
						
				//add ext apps
				foreach (Tools.ExternalTool extA in Runtime.ExternalTools)
				{
					ToolStripMenuItem nItem = new ToolStripMenuItem();
					nItem.Text = extA.DisplayName;
					nItem.Tag = extA;
							
					nItem.Image = extA.Image;
							
					nItem.Click += cMenTreeToolsExternalAppsEntry_Click;
							
					cMenTreeToolsExternalApps.DropDownItems.Add(nItem);
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "cMenTreeTools_DropDownOpening failed (UI.Window.Tree)" + Environment.NewLine + ex.Message, true);
			}
		}
				
		private static void StartExternalApp(Tools.ExternalTool externalTool)
		{
			try
			{
                if (ConnectionTreeNode.GetNodeType(ConnectionTree.SelectedNode) == TreeNodeType.Connection | ConnectionTreeNode.GetNodeType(ConnectionTree.SelectedNode) == TreeNodeType.PuttySession)
				{
                    externalTool.Start((ConnectionInfo)ConnectionTree.SelectedNode.Tag);
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "cMenTreeToolsExternalAppsEntry_Click failed (UI.Window.Tree)" + Environment.NewLine + ex.Message, true);
			}
		}
        #endregion
		
        #region Menu
		static public void mMenViewExpandAllFolders_Click(System.Object sender, EventArgs e)
		{
            ConnectionTree.ExpandAllNodes();
		}
				
		public void mMenViewCollapseAllFolders_Click(System.Object sender, EventArgs e)
		{
			if (tvConnections.SelectedNode != null)
			{
				if (tvConnections.SelectedNode.IsEditing)
				{
					tvConnections.SelectedNode.EndEdit(false);
				}
			}
            ConnectionTree.CollapseAllNodes();
		}
        #endregion
		
        #region Search
		public void txtSearch_GotFocus(object sender, EventArgs e)
		{
			txtSearch.ForeColor = Themes.ThemeManager.ActiveTheme.SearchBoxTextColor;
			if (txtSearch.Text == Language.strSearchPrompt)
			{
				txtSearch.Text = "";
			}
		}
				
		public void txtSearch_LostFocus(object sender, EventArgs e)
		{
			if (txtSearch.Text == "")
			{
				txtSearch.ForeColor = Themes.ThemeManager.ActiveTheme.SearchBoxTextPromptColor;
				txtSearch.Text = Language.strSearchPrompt;
			}
		}
				
		public void txtSearch_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Escape)
				{
					e.Handled = true;
					tvConnections.Focus();
				}
				else if (e.KeyCode == Keys.Up)
				{
					tvConnections.SelectedNode = tvConnections.SelectedNode.PrevVisibleNode;
				}
				else if (e.KeyCode == Keys.Down)
				{
					tvConnections.SelectedNode = tvConnections.SelectedNode.NextVisibleNode;
				}
				else
				{
					tvConnections_KeyDown(sender, e);
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "txtSearch_KeyDown (UI.Window.Tree) failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		public void txtSearch_TextChanged(System.Object sender, EventArgs e)
		{
			tvConnections.SelectedNode = ConnectionTree.Find(tvConnections.Nodes[0], txtSearch.Text);
		}
				
		public void tvConnections_KeyPress(object sender, KeyPressEventArgs e)
		{
			try
			{
				if (char.IsLetterOrDigit(e.KeyChar))
				{
					txtSearch.Text = e.KeyChar.ToString();
							
					txtSearch.Focus();
					txtSearch.SelectionStart = txtSearch.TextLength;
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "tvConnections_KeyPress (UI.Window.Tree) failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		public void tvConnections_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Enter)
				{
					if (tvConnections.SelectedNode.Tag is ConnectionInfo)
					{
						e.Handled = true;
                        Runtime.OpenConnection();
					}
					else
					{
						if (tvConnections.SelectedNode.IsExpanded)
						{
							tvConnections.SelectedNode.Collapse(true);
						}
						else
						{
							tvConnections.SelectedNode.Expand();
						}
					}
				}
				else if (e.KeyCode == Keys.Escape ^ e.KeyCode == Keys.Control | e.KeyCode == Keys.F)
				{
					txtSearch.Focus();
					txtSearch.SelectionStart = txtSearch.TextLength;
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "tvConnections_KeyDown (UI.Window.Tree) failed" + Environment.NewLine + ex.Message, true);
			}
		}
        #endregion
	}
}