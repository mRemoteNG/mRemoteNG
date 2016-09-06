using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using mRemoteNG.Tree;
using System;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;


namespace mRemoteNG.UI.Window
{
	public partial class ConnectionTreeWindow
	{
	    private ConnectionTreeModel _connectionTreeModel;

        private ToolTip DescriptionTooltip { get; }

	    public ConnectionTreeModel ConnectionTreeModel
	    {
	        get { return _connectionTreeModel; }
	        set
	        {
	            _connectionTreeModel = value;
	            PopulateTreeView();
	        }
	    }

        #region Form Stuff
        private void Tree_Load(object sender, EventArgs e)
		{
			ApplyLanguage();
			Themes.ThemeManager.ThemeChanged += ApplyTheme;
			ApplyTheme();
					
			txtSearch.Multiline = true;
			txtSearch.MinimumSize = new Size(0, 14);
			txtSearch.Size = new Size(txtSearch.Size.Width, 14);
			txtSearch.Multiline = false;
            olvConnections.Show();
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

        private void ApplyTheme()
		{
			msMain.BackColor = Themes.ThemeManager.ActiveTheme.ToolbarBackgroundColor;
			msMain.ForeColor = Themes.ThemeManager.ActiveTheme.ToolbarTextColor;
			olvConnections.BackColor = Themes.ThemeManager.ActiveTheme.ConnectionsPanelBackgroundColor;
            olvConnections.ForeColor = Themes.ThemeManager.ActiveTheme.ConnectionsPanelTextColor;
			//tvConnections.LineColor = Themes.ThemeManager.ActiveTheme.ConnectionsPanelTreeLineColor;
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

		    DescriptionTooltip = new ToolTip
		    {
		        InitialDelay = 300,
		        ReshowDelay = 0
		    };
		}

	    private void PopulateTreeView()
	    {
	        olvColumn1.AspectGetter = item => ((ConnectionInfo) item).Name;
            olvConnections.CanExpandGetter = item => item is ContainerInfo;
	        olvConnections.ChildrenGetter = item => ((ContainerInfo) item).Children;
            olvConnections.Roots = ConnectionTreeModel.RootNodes;
            olvConnections.ExpandAll();
        }
				
		public void InitialRefresh()
		{
			tvConnections_AfterSelect(tvConnections, new TreeViewEventArgs(tvConnections.SelectedNode, TreeViewAction.ByMouse));
		}

        public void ExpandPreviouslyOpenedFolders()
        {
            foreach (ContainerInfo contI in Runtime.ContainerList)
            {
                if (contI.IsExpanded)
                    contI.TreeNode.Expand();
            }
        }

        public void OpenConnectionsFromLastSession()
        {
            if (!Settings.Default.OpenConsFromLastSession || Settings.Default.NoReconnect) return;
            foreach (ConnectionInfo conI in Runtime.ConnectionList)
            {
                if (conI.PleaseConnect)
                    Runtime.OpenConnection(conI);
            }
        }

	    public void EnsureRootNodeVisible()
	    {
	        var rootNode = tvConnections.Nodes[0];
            rootNode.EnsureVisible();
	    }
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
				Runtime.MessageCollector.AddExceptionStackTrace("FillImageList (UI.Window.ConnectionTreeWindow) failed", ex);
			}
		}
				
		private void tvConnections_BeforeLabelEdit(object sender, LabelEditEventArgs e)
		{
			cMenTreeDelete.ShortcutKeys = Keys.None;
		}

        private void tvConnections_AfterLabelEdit(object sender, LabelEditEventArgs e)
		{
			try
			{
				cMenTreeDelete.ShortcutKeys = Keys.Delete;

                ConnectionTree.FinishRenameSelectedNode(e.Label);
                Windows.configForm.pGrid_SelectedObjectChanged();
				//ShowHideTreeContextMenuItems(e.Node);
                Runtime.SaveConnectionsBG();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace("tvConnections_AfterLabelEdit (UI.Window.ConnectionTreeWindow) failed", ex);
			}
		}

        private void tvConnections_AfterSelect(object sender, EventArgs e)
		{
			//try
			//{
			//	if ((ConnectionTreeNode.GetNodeType(e.Node) == TreeNodeType.Connection) || (ConnectionTreeNode.GetNodeType(e.Node) == TreeNodeType.PuttySession))
			//	{
   //                 Windows.configForm.SetPropertyGridObject(e.Node.Tag);
			//	}
			//	else if (ConnectionTreeNode.GetNodeType(e.Node) == TreeNodeType.Container)
			//	{
   //                 Windows.configForm.SetPropertyGridObject((ContainerInfo) e.Node.Tag);
			//	}
			//	else if ((ConnectionTreeNode.GetNodeType(e.Node) == TreeNodeType.Root) || (ConnectionTreeNode.GetNodeType(e.Node) == TreeNodeType.PuttyRoot))
			//	{
   //                 Windows.configForm.SetPropertyGridObject(e.Node.Tag);
			//	}
			//	else
			//	{
			//		return;
			//	}

   //             Windows.configForm.pGrid_SelectedObjectChanged();
			//	ShowHideTreeContextMenuItems(e.Node);

   //             Runtime.LastSelected = ConnectionTreeNode.GetConstantID(e.Node);
			//}
			//catch (Exception ex)
			//{
			//	Runtime.MessageCollector.AddExceptionStackTrace("tvConnections_AfterSelect (UI.Window.ConnectionTreeWindow) failed", ex);
			//}
		}

        private void tvConnections_NodeMouseClick(object sender, MouseEventArgs e)
		{
			//try
			//{
			//	ShowHideTreeContextMenuItems(tvConnections.SelectedNode);
			//	tvConnections.SelectedNode = e.Node;
						
			//	if (e.Button == MouseButtons.Left)
			//	{
			//		if (Settings.Default.SingleClickOnConnectionOpensIt && 
			//			(ConnectionTreeNode.GetNodeType(e.Node) == TreeNodeType.Connection |
   //                     ConnectionTreeNode.GetNodeType(e.Node) == TreeNodeType.PuttySession))
			//		{
			//			Runtime.OpenConnection();
			//		}
							
			//		if (Settings.Default.SingleClickSwitchesToOpenConnection && ConnectionTreeNode.GetNodeType(e.Node) == TreeNodeType.Connection)
			//		{
   //                     Runtime.SwitchToOpenConnection((ConnectionInfo)e.Node.Tag);
			//		}
			//	}
			//}
			//catch (Exception ex)
			//{
			//	Runtime.MessageCollector.AddExceptionStackTrace("tvConnections_NodeMouseClick (UI.Window.ConnectionTreeWindow) failed", ex);
			//}
		}

        private static void tvConnections_NodeMouseDoubleClick(object sender, MouseEventArgs e)
		{
            if (ConnectionTreeNode.GetNodeType(ConnectionTree.SelectedNode) == TreeNodeType.Connection |
                ConnectionTreeNode.GetNodeType(ConnectionTree.SelectedNode) == TreeNodeType.PuttySession)
			{
                Runtime.OpenConnection();
			}
		}

        private void tvConnections_MouseMove(object sender, MouseEventArgs e)
		{
			try
			{
                ConnectionTree.SetNodeToolTip(e, DescriptionTooltip);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace("tvConnections_MouseMove (UI.Window.ConnectionTreeWindow) failed", ex);
			}
		}
				
		private static void EnableMenuItemsRecursive(ToolStripItemCollection items, bool enable = true)
		{
		    foreach (ToolStripItem item in items)
			{
				var menuItem = item as ToolStripMenuItem;
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
				    foreach (TreeNode node in selectedNode.Nodes)
					{
						if (node.Tag is ConnectionInfo)
						{
						    var connectionInfo = (ConnectionInfo)node.Tag;
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
				Runtime.MessageCollector.AddExceptionStackTrace("ShowHideTreeContextMenuItems (UI.Window.ConnectionTreeWindow) failed", ex);
			}
		}
        #endregion

        #region Drag and Drop
        private static void tvConnections_DragDrop(object sender, DragEventArgs e)
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
				Runtime.MessageCollector.AddExceptionStackTrace("tvConnections_DragDrop (UI.Window.ConnectionTreeWindow) failed", ex);
			}
		}



        private static void tvConnections_DragEnter(object sender, DragEventArgs e)
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
				Runtime.MessageCollector.AddExceptionStackTrace("tvConnections_DragEnter (UI.Window.ConnectionTreeWindow) failed", ex);
			}
		}

        private static void tvConnections_DragOver(object sender, DragEventArgs e)
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

			    while (targetNode != null)
				{
					var puttyRootInfo = targetNode.Tag as Root.PuttySessions.PuttySessionsNodeInfo;
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
				Runtime.MessageCollector.AddExceptionStackTrace("tvConnections_DragOver (UI.Window.ConnectionTreeWindow) failed", ex);
			}
		}

        private void tvConnections_ItemDrag(object sender, ItemDragEventArgs e)
		{
			try
			{
				TreeNode dragTreeNode = e.Item as TreeNode;

			    if (dragTreeNode?.Tag == null)
				{
					return;
				}
				if (dragTreeNode.Tag is PuttySessionInfo|| !(dragTreeNode.Tag is ConnectionInfo|| dragTreeNode.Tag is ContainerInfo))
				{
					tvConnections.SelectedNode = dragTreeNode;
				    return;
				}
						
				//Set the drag node and initiate the DragDrop
				DoDragDrop(e.Item, DragDropEffects.Move);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace("tvConnections_ItemDrag (UI.Window.ConnectionTreeWindow) failed", ex);
			}
		}
        #endregion

        #region Tree Context Menu
        private void cMenTreeAddConnection_Click(object sender, EventArgs e)
		{
			AddConnection();
            Runtime.SaveConnectionsBG();
		}

        private void cMenTreeAddFolder_Click(object sender, EventArgs e)
		{
			AddFolder();
            Runtime.SaveConnectionsBG();
		}

        private void cMenTreeConnect_Click(object sender, EventArgs e)
		{
            Runtime.OpenConnection(ConnectionInfo.Force.DoNotJump);
		}

        private void cMenTreeConnectWithOptionsConnectToConsoleSession_Click(object sender, EventArgs e)
		{
            Runtime.OpenConnection(ConnectionInfo.Force.UseConsoleSession | ConnectionInfo.Force.DoNotJump);
		}

        private void cMenTreeConnectWithOptionsNoCredentials_Click(object sender, EventArgs e)
		{
            Runtime.OpenConnection(ConnectionInfo.Force.NoCredentials);
		}

        private void cMenTreeConnectWithOptionsDontConnectToConsoleSession_Click(object sender, EventArgs e)
		{
            Runtime.OpenConnection(ConnectionInfo.Force.DontUseConsoleSession | ConnectionInfo.Force.DoNotJump);
		}

        private void cMenTreeConnectWithOptionsConnectInFullscreen_Click(object sender, EventArgs e)
		{
            Runtime.OpenConnection(ConnectionInfo.Force.Fullscreen | ConnectionInfo.Force.DoNotJump);
		}

        private void cMenTreeConnectWithOptionsChoosePanelBeforeConnecting_Click(object sender, EventArgs e)
		{
            Runtime.OpenConnection(ConnectionInfo.Force.OverridePanel | ConnectionInfo.Force.DoNotJump);
		}

	    private void cMenTreeDisconnect_Click(object sender, EventArgs e)
		{
			DisconnectConnection();
		}

        private void cMenTreeToolsTransferFile_Click(object sender, EventArgs e)
		{
			SshTransferFile();
		}

        private void mMenSortAscending_Click(object sender, EventArgs e)
		{
			tvConnections.BeginUpdate();
            ConnectionTree.Sort(tvConnections.Nodes[0], SortOrder.Ascending);
			tvConnections.EndUpdate();
            Runtime.SaveConnectionsBG();
		}

        private void cMenTreeToolsSortAscending_Click(object sender, EventArgs e)
		{
			tvConnections.BeginUpdate();
            ConnectionTree.Sort(tvConnections.SelectedNode, SortOrder.Ascending);
			tvConnections.EndUpdate();
            Runtime.SaveConnectionsBG();
		}

        private void cMenTreeToolsSortDescending_Click(object sender, EventArgs e)
		{
			tvConnections.BeginUpdate();
            ConnectionTree.Sort(tvConnections.SelectedNode, SortOrder.Descending);
			tvConnections.EndUpdate();
            Runtime.SaveConnectionsBG();
		}

        private void cMenTree_DropDownOpening(object sender, EventArgs e)
		{
			AddExternalApps();
		}
				
		private void cMenTreeToolsExternalAppsEntry_Click(object sender, EventArgs e)
		{
			StartExternalApp((Tools.ExternalTool)((ToolStripMenuItem)sender).Tag);
		}

        private void cMenTreeDuplicate_Click(object sender, EventArgs e)
		{
            ConnectionTreeNode.CloneNode(tvConnections.SelectedNode);
            Runtime.SaveConnectionsBG();
		}

        private void cMenTreeRename_Click(object sender, EventArgs e)
		{
            ConnectionTree.StartRenameSelectedNode();
            Runtime.SaveConnectionsBG();
		}

        private void cMenTreeDelete_Click(object sender, EventArgs e)
		{
            ConnectionTree.DeleteSelectedNode();
            Runtime.SaveConnectionsBG();
		}

        private void cMenTreeImportFile_Click(object sender, EventArgs e)
		{
            Import.ImportFromFile(Windows.treeForm.tvConnections.Nodes[0], Windows.treeForm.tvConnections.SelectedNode, true);
		}

        private void cMenTreeImportActiveDirectory_Click(object sender, EventArgs e)
		{
            Windows.Show(WindowType.ActiveDirectoryImport);
		}

        private void cMenTreeImportPortScan_Click(object sender, EventArgs e)
		{
            Windows.Show(WindowType.PortScan);
		}

        private void cMenTreeExportFile_Click(object sender, EventArgs e)
		{
            Export.ExportToFile(Windows.treeForm.tvConnections.Nodes[0], Windows.treeForm.tvConnections.SelectedNode, Runtime.ConnectionTreeModel);
		}
        private void cMenTreeMoveUp_Click(object sender, EventArgs e)
		{
            ConnectionTree.MoveNodeUp();
            Runtime.SaveConnectionsBG();
		}

        private void cMenTreeMoveDown_Click(object sender, EventArgs e)
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
                newConnectionInfo.CopyFrom(DefaultConnectionInfo.Instance);
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
				Runtime.MessageCollector.AddExceptionStackTrace("UI.Window.Tree.AddConnection() failed.", ex);
			}
		}
				
		public void AddFolder()
		{
			try
			{
				TreeNode newNode = ConnectionTreeNode.AddNode(TreeNodeType.Container);
				ContainerInfo newContainerInfo = new ContainerInfo();
                newContainerInfo.CopyFrom(DefaultConnectionInfo.Instance);
				newNode.Tag = newContainerInfo;
				newContainerInfo.TreeNode = newNode;

                TreeNode selectedNode = ConnectionTree.SelectedNode;
				TreeNode parentNode;
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
						
				newContainerInfo.Name = newNode.Text;
						
				// We can only inherit from a container node, not the root node or connection nodes
				if (ConnectionTreeNode.GetNodeType(parentNode) == TreeNodeType.Container)
				{
					newContainerInfo.Parent = (ContainerInfo)parentNode.Tag;
				}
				else
				{
					newContainerInfo.Inheritance.DisableInheritance();
				}

                Runtime.ContainerList.Add(newContainerInfo);
				parentNode.Nodes.Add(newNode);
						
				tvConnections.SelectedNode = newNode;
				tvConnections.SelectedNode.BeginEdit();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace(Language.strErrorAddFolderFailed, ex);
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
				Runtime.MessageCollector.AddExceptionStackTrace("DisconnectConnection (UI.Window.ConnectionTreeWindow) failed", ex);
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
				Runtime.MessageCollector.AddExceptionStackTrace("SSHTransferFile (UI.Window.ConnectionTreeWindow) failed", ex);
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
				Runtime.MessageCollector.AddExceptionStackTrace("cMenTreeTools_DropDownOpening failed (UI.Window.ConnectionTreeWindow)", ex);
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
				Runtime.MessageCollector.AddExceptionStackTrace("cMenTreeToolsExternalAppsEntry_Click failed (UI.Window.ConnectionTreeWindow)", ex);
			}
		}
        #endregion

        #region Menu
        private void mMenViewExpandAllFolders_Click(object sender, EventArgs e)
		{
            ConnectionTree.ExpandAllNodes();
		}

        private void mMenViewCollapseAllFolders_Click(object sender, EventArgs e)
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
        private void txtSearch_GotFocus(object sender, EventArgs e)
		{
			txtSearch.ForeColor = Themes.ThemeManager.ActiveTheme.SearchBoxTextColor;
			if (txtSearch.Text == Language.strSearchPrompt)
			{
				txtSearch.Text = "";
			}
		}

        private void txtSearch_LostFocus(object sender, EventArgs e)
		{
			if (txtSearch.Text == "")
			{
				txtSearch.ForeColor = Themes.ThemeManager.ActiveTheme.SearchBoxTextPromptColor;
				txtSearch.Text = Language.strSearchPrompt;
			}
		}

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
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
				Runtime.MessageCollector.AddExceptionStackTrace("txtSearch_KeyDown (UI.Window.ConnectionTreeWindow) failed", ex);
			}
		}

        private void txtSearch_TextChanged(object sender, EventArgs e)
		{
		    try
		    {
                //tvConnections.SelectedNode = ConnectionTree.Find(tvConnections.Nodes[0], txtSearch.Text);
            }
		    catch (Exception)
		    {
		    }
		}

        private void tvConnections_KeyPress(object sender, KeyPressEventArgs e)
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
				Runtime.MessageCollector.AddExceptionStackTrace("tvConnections_KeyPress (UI.Window.ConnectionTreeWindow) failed", ex);
			}
		}

        private void tvConnections_KeyDown(object sender, KeyEventArgs e)
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
				Runtime.MessageCollector.AddExceptionStackTrace("tvConnections_KeyDown (UI.Window.ConnectionTreeWindow) failed", ex);
			}
		}
        #endregion
	}
}