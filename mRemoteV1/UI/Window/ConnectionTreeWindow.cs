using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using mRemoteNG.Tree;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using mRemoteNG.Root.PuttySessions;
using mRemoteNG.Tree.Root;
using WeifenLuo.WinFormsUI.Docking;


namespace mRemoteNG.UI.Window
{
	public partial class ConnectionTreeWindow
	{
	    private ConnectionTreeModel _connectionTreeModel;

        private ToolTip DescriptionTooltip { get; }
	    private ConnectionInfo SelectedNode => (ConnectionInfo) olvConnections.SelectedObject;

	    public ConnectionTreeModel ConnectionTreeModel
	    {
	        get { return _connectionTreeModel; }
	        set
	        {
	            _connectionTreeModel = value;
	            PopulateTreeView();
	        }
	    }

		public ConnectionTreeWindow(DockContent panel)
		{
			WindowType = WindowType.Tree;
			DockPnl = panel;
			InitializeComponent();
		    DescriptionTooltip = new ToolTip
		    {
		        InitialDelay = 300,
		        ReshowDelay = 0
		    };

            FillImageList();
            LinkModelToView();
		    SetEventHandlers();
		}

        private void FillImageList()
        {
            try
            {
                imgListTree.Images.Add(Resources.Root);
                imgListTree.Images.SetKeyName(0, "Root");
                imgListTree.Images.Add(Resources.Folder);
                imgListTree.Images.SetKeyName(1, "Folder");
                imgListTree.Images.Add(Resources.Play);
                imgListTree.Images.SetKeyName(2, "Play");
                imgListTree.Images.Add(Resources.Pause);
                imgListTree.Images.SetKeyName(3, "Pause");
                imgListTree.Images.Add(Resources.PuttySessions);
                imgListTree.Images.SetKeyName(4, "PuttySessions");
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("FillImageList (UI.Window.ConnectionTreeWindow) failed", ex);
            }
        }

        private void LinkModelToView()
	    {
            olvNameColumn.AspectGetter = item => ((ConnectionInfo)item).Name;
	        olvNameColumn.ImageGetter = ConnectionImageGetter;
            olvConnections.CanExpandGetter = item => item is ContainerInfo;
            olvConnections.ChildrenGetter = item => ((ContainerInfo)item).Children;
        }

	    private object ConnectionImageGetter(object rowObject)
	    {
            if (rowObject is RootNodeInfo) return "Root";
	        if (rowObject is ContainerInfo) return "Folder";
            if (rowObject is PuttySessionInfo) return "PuttySessions";
	        var connection = rowObject as ConnectionInfo;
	        if (connection == null) return "";
	        return connection.OpenConnections.Count > 0 ? "Play" : "Pause";
	    }

	    private void SetEventHandlers()
	    {
	        olvConnections.Collapsed += (sender, args) => ((ContainerInfo) args.Model).IsExpanded = false;
            olvConnections.Expanded += (sender, args) => ((ContainerInfo) args.Model).IsExpanded = true;
	        olvConnections.BeforeLabelEdit += tvConnections_BeforeLabelEdit;
	        olvConnections.AfterLabelEdit += tvConnections_AfterLabelEdit;
	        olvConnections.SelectionChanged += tvConnections_AfterSelect;
	        olvConnections.CellClick += tvConnections_NodeMouseSingleClick;
	        olvConnections.CellClick += tvConnections_NodeMouseDoubleClick;
	        olvConnections.CellToolTipShowing += tvConnections_CellToolTipShowing;
            olvConnections.ModelCanDrop += OlvConnections_OnModelCanDrop;
            olvConnections.ModelDropped += OlvConnections_OnModelDropped;
	    }

	    


	    private void PopulateTreeView()
	    {
            olvConnections.Roots = ConnectionTreeModel.RootNodes;
	        ExpandPreviouslyOpenedFolders();
            ExpandRootConnectionNode();
	        OpenConnectionsFromLastSession();
	    }

	    private void ExpandRootConnectionNode()
	    {
            var rootConnectionNode = GetRootConnectionNode();
            olvConnections.Expand(rootConnectionNode);
        }

	    private RootNodeInfo GetRootConnectionNode()
	    {
            return (RootNodeInfo)olvConnections.Roots.Cast<ConnectionInfo>().First(item => item is RootNodeInfo);
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

        public void ExpandPreviouslyOpenedFolders()
        {
            var containerList = ConnectionTreeModel.GetRecursiveChildList(GetRootConnectionNode()).OfType<ContainerInfo>();
            var previouslyExpandedNodes = containerList.Where(container => container.IsExpanded);
            olvConnections.ExpandedObjects = previouslyExpandedNodes;
            olvConnections.RebuildAll(true);
        }

        public void OpenConnectionsFromLastSession()
        {
            if (!Settings.Default.OpenConsFromLastSession || Settings.Default.NoReconnect) return;
            var connectionInfoList = GetRootConnectionNode().GetRecursiveChildList().Where(node => !(node is ContainerInfo));
            var previouslyOpenedConnections = connectionInfoList.Where(item => item.PleaseConnect);
            foreach (var connectionInfo in previouslyOpenedConnections)
            {
                Runtime.OpenConnection(connectionInfo);
            }
        }

        public void EnsureRootNodeVisible()
	    {
            olvConnections.EnsureModelVisible(GetRootConnectionNode());
	    }

        #region Private Methods
        private void tvConnections_BeforeLabelEdit(object sender, LabelEditEventArgs e)
		{
			cMenTreeDelete.ShortcutKeys = Keys.None;
		}

        private void tvConnections_AfterLabelEdit(object sender, LabelEditEventArgs e)
		{
			try
			{
				cMenTreeDelete.ShortcutKeys = Keys.Delete;
                ConnectionTreeModel.RenameNode(SelectedNode, e.Label);
                Windows.configForm.pGrid_SelectedObjectChanged();
				ShowHideTreeContextMenuItems(SelectedNode);
                Runtime.SaveConnectionsBG();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace("tvConnections_AfterLabelEdit (UI.Window.ConnectionTreeWindow) failed", ex);
			}
		}

        private void tvConnections_AfterSelect(object sender, EventArgs e)
		{
            try
            {
                Windows.configForm.SetPropertyGridObject(olvConnections.SelectedObject);
                ShowHideTreeContextMenuItems((ConnectionInfo)olvConnections.SelectedObject);
                Runtime.LastSelected = ((ConnectionInfo)olvConnections.SelectedObject)?.ConstantID;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("tvConnections_AfterSelect (UI.Window.ConnectionTreeWindow) failed", ex);
            }
        }

        private void tvConnections_NodeMouseSingleClick(object sender, CellClickEventArgs e)
		{
            try
            {
                if (e.ClickCount > 1) return;
                var clickedNode = (ConnectionInfo)e.Model;
                ShowHideTreeContextMenuItems(SelectedNode);
                
                //if (e.Button != MouseButtons.Left) return;
                if (clickedNode.GetTreeNodeType() != TreeNodeType.Connection && clickedNode.GetTreeNodeType() != TreeNodeType.PuttySession) return;
                if (Settings.Default.SingleClickOnConnectionOpensIt)
                    Runtime.OpenConnection();

                if (Settings.Default.SingleClickSwitchesToOpenConnection)
                    Runtime.SwitchToOpenConnection(SelectedNode);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("tvConnections_NodeMouseClick (UI.Window.ConnectionTreeWindow) failed", ex);
            }
        }

        private void tvConnections_NodeMouseDoubleClick(object sender, CellClickEventArgs e)
        {
            if (e.ClickCount < 2) return;
            var clickedNode = e.Model as ConnectionInfo;
            
            if (clickedNode?.GetTreeNodeType() == TreeNodeType.Connection |
                clickedNode?.GetTreeNodeType() == TreeNodeType.PuttySession)
			{
                Runtime.OpenConnection();
			}
		}

        private void tvConnections_CellToolTipShowing(object sender, ToolTipShowingEventArgs e)
		{
			try
			{
			    var nodeProducingTooltip = (ConnectionInfo) e.Model;
			    e.Text = nodeProducingTooltip.Description;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace("tvConnections_MouseMove (UI.Window.ConnectionTreeWindow) failed", ex);
			}
		}

        //TODO Fix for TreeListView
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

        //TODO Fix for TreeListView
        private void ShowHideTreeContextMenuItems(ConnectionInfo connectionInfo)
		{
			if (connectionInfo == null)
				return ;
					
			try
			{
				cMenTree.Enabled = true;
				EnableMenuItemsRecursive(cMenTree.Items);
				if (connectionInfo is RootPuttySessionsNodeInfo)
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
                else if (connectionInfo is RootNodeInfo)
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
                else if (connectionInfo is ContainerInfo)
                {
                    cMenTreeConnectWithOptionsConnectInFullscreen.Enabled = false;
                    cMenTreeConnectWithOptionsConnectToConsoleSession.Enabled = false;
                    cMenTreeDisconnect.Enabled = false;

                    var openConnections = 0;
                    //foreach (TreeNode node in selectedNode.Nodes)
                    //{
                    //    if (node.Tag is ConnectionInfo)
                    //    {
                    //        var connectionInfo = (ConnectionInfo)node.Tag;
                    //        openConnections = openConnections + connectionInfo.OpenConnections.Count;
                    //    }
                    //}
                    if (openConnections == 0)
                    {
                        cMenTreeDisconnect.Enabled = false;
                    }

                    cMenTreeToolsTransferFile.Enabled = false;
                    cMenTreeToolsExternalApps.Enabled = false;
                }
                else if (connectionInfo is PuttySessionInfo)
				{
					cMenTreeAddConnection.Enabled = false;
					cMenTreeAddFolder.Enabled = false;
							
					if (connectionInfo.OpenConnections.Count == 0)
					{
						cMenTreeDisconnect.Enabled = false;
					}
							
					if (!(connectionInfo.Protocol == ProtocolType.SSH1 | connectionInfo.Protocol == ProtocolType.SSH2))
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
                else
                {
                    if (connectionInfo.OpenConnections.Count == 0)
                        cMenTreeDisconnect.Enabled = false;

                    if (!(connectionInfo.Protocol == ProtocolType.SSH1 | connectionInfo.Protocol == ProtocolType.SSH2))
                        cMenTreeToolsTransferFile.Enabled = false;

                    if (!(connectionInfo.Protocol == ProtocolType.RDP | connectionInfo.Protocol == ProtocolType.ICA))
                    {
                        cMenTreeConnectWithOptionsConnectInFullscreen.Enabled = false;
                        cMenTreeConnectWithOptionsConnectToConsoleSession.Enabled = false;
                    }

                    if (connectionInfo.Protocol == ProtocolType.IntApp)
                        cMenTreeConnectWithOptionsNoCredentials.Enabled = false;
                }
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace("ShowHideTreeContextMenuItems (UI.Window.ConnectionTreeWindow) failed", ex);
			}
		}
        #endregion

        #region Drag and Drop
        private void OlvConnections_OnModelCanDrop(object sender, ModelDropEventArgs e)
        {
            var draggedObject = e.SourceModels[0] as ConnectionInfo;
            if (!NodeIsDraggable(draggedObject)) return;
            var dropTarget = e.TargetModel as ContainerInfo;
            if (dropTarget != null)
                e.Effect = DragDropEffects.Move;
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

	    private bool NodeIsDraggable(ConnectionInfo node)
	    {
            if (node == null || node is RootNodeInfo || node is PuttySessionInfo) return false;
	        return true;
	    }

        private void OlvConnections_OnModelDropped(object sender, ModelDropEventArgs e)
        {
            var draggedObject = (IHasParent)e.SourceModels[0];
            var dropTarget = e.TargetModel as ContainerInfo;
            if (dropTarget != null)
                draggedObject.SetParent(dropTarget);
            e.RefreshObjects();
        }
        #endregion

        #region Tree Context Menu
        //TODO Fix for TreeListView
        private void cMenTreeAddConnection_Click(object sender, EventArgs e)
		{
			AddConnection();
            Runtime.SaveConnectionsBG();
		}

        //TODO Fix for TreeListView
        private void cMenTreeAddFolder_Click(object sender, EventArgs e)
		{
			AddFolder();
            Runtime.SaveConnectionsBG();
		}

        //TODO Fix for TreeListView
        private void cMenTreeConnect_Click(object sender, EventArgs e)
		{
            Runtime.OpenConnection(ConnectionInfo.Force.DoNotJump);
		}

        //TODO Fix for TreeListView
        private void cMenTreeConnectWithOptionsConnectToConsoleSession_Click(object sender, EventArgs e)
		{
            Runtime.OpenConnection(ConnectionInfo.Force.UseConsoleSession | ConnectionInfo.Force.DoNotJump);
		}

        //TODO Fix for TreeListView
        private void cMenTreeConnectWithOptionsNoCredentials_Click(object sender, EventArgs e)
		{
            Runtime.OpenConnection(ConnectionInfo.Force.NoCredentials);
		}

        //TODO Fix for TreeListView
        private void cMenTreeConnectWithOptionsDontConnectToConsoleSession_Click(object sender, EventArgs e)
		{
            Runtime.OpenConnection(ConnectionInfo.Force.DontUseConsoleSession | ConnectionInfo.Force.DoNotJump);
		}

        //TODO Fix for TreeListView
        private void cMenTreeConnectWithOptionsConnectInFullscreen_Click(object sender, EventArgs e)
		{
            Runtime.OpenConnection(ConnectionInfo.Force.Fullscreen | ConnectionInfo.Force.DoNotJump);
		}

        //TODO Fix for TreeListView
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

        //TODO Fix for TreeListView
        private void mMenSortAscending_Click(object sender, EventArgs e)
		{
			tvConnections.BeginUpdate();
            ConnectionTree.Sort(tvConnections.Nodes[0], SortOrder.Ascending);
			tvConnections.EndUpdate();
            Runtime.SaveConnectionsBG();
		}

        //TODO Fix for TreeListView
        private void cMenTreeToolsSortAscending_Click(object sender, EventArgs e)
		{
			tvConnections.BeginUpdate();
            ConnectionTree.Sort(tvConnections.SelectedNode, SortOrder.Ascending);
			tvConnections.EndUpdate();
            Runtime.SaveConnectionsBG();
		}

        //TODO Fix for TreeListView
        private void cMenTreeToolsSortDescending_Click(object sender, EventArgs e)
		{
			tvConnections.BeginUpdate();
            ConnectionTree.Sort(tvConnections.SelectedNode, SortOrder.Descending);
			tvConnections.EndUpdate();
            Runtime.SaveConnectionsBG();
		}

        //TODO Fix for TreeListView
        private void cMenTree_DropDownOpening(object sender, EventArgs e)
		{
			AddExternalApps();
		}
        
        //TODO Fix for TreeListView
        private void cMenTreeToolsExternalAppsEntry_Click(object sender, EventArgs e)
		{
			StartExternalApp((Tools.ExternalTool)((ToolStripMenuItem)sender).Tag);
		}

        private void cMenTreeDuplicate_Click(object sender, EventArgs e)
        {
            SelectedNode.Clone();
            Runtime.SaveConnectionsBG();
            olvConnections.RebuildAll(true);
		}

        private void cMenTreeRename_Click(object sender, EventArgs e)
		{
            olvConnections.SelectedItem.BeginEdit();
            Runtime.SaveConnectionsBG();
		}

        private void cMenTreeDelete_Click(object sender, EventArgs e)
		{
            ConnectionTreeModel.DeleteNode(SelectedNode);
            Runtime.SaveConnectionsBG();
            olvConnections.RebuildAll(true);
		}

        //TODO Fix for TreeListView
        private void cMenTreeImportFile_Click(object sender, EventArgs e)
		{
            Import.ImportFromFile(Windows.treeForm.tvConnections.Nodes[0], Windows.treeForm.tvConnections.SelectedNode, true);
		}

        //TODO Fix for TreeListView
        private void cMenTreeImportActiveDirectory_Click(object sender, EventArgs e)
		{
            Windows.Show(WindowType.ActiveDirectoryImport);
		}

        //TODO Fix for TreeListView
        private void cMenTreeImportPortScan_Click(object sender, EventArgs e)
		{
            Windows.Show(WindowType.PortScan);
		}

        //TODO Fix for TreeListView
        private void cMenTreeExportFile_Click(object sender, EventArgs e)
		{
            Export.ExportToFile(Windows.treeForm.tvConnections.Nodes[0], Windows.treeForm.tvConnections.SelectedNode, Runtime.ConnectionTreeModel);
		}

        //TODO Fix for TreeListView
        private void cMenTreeMoveUp_Click(object sender, EventArgs e)
		{
            ConnectionTree.MoveNodeUp();
            Runtime.SaveConnectionsBG();
		}

        //TODO Fix for TreeListView
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
			    var newConnectionInfo = new ConnectionInfo();
			    var selectedContainer = SelectedNode as ContainerInfo;
			    newConnectionInfo.SetParent(selectedContainer ?? SelectedNode.Parent);
                olvConnections.RebuildAll(true);
                Runtime.ConnectionList.Add(newConnectionInfo);
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
				var newContainerInfo = new ContainerInfo();
                var selectedContainer = SelectedNode as ContainerInfo;
                newContainerInfo.SetParent(selectedContainer ?? SelectedNode.Parent);
                olvConnections.RebuildAll(true);
                Runtime.ContainerList.Add(newContainerInfo);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace(Language.strErrorAddFolderFailed, ex);
			}
		}

        //TODO Fix for TreeListView
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

        //TODO Fix for TreeListView
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

        //TODO Fix for TreeListView
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

        //TODO Fix for TreeListView
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
        //TODO Fix for TreeListView
        private void mMenViewExpandAllFolders_Click(object sender, EventArgs e)
		{
            ConnectionTree.ExpandAllNodes();
		}

        //TODO Fix for TreeListView
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

        //TODO Fix for TreeListView
        private void txtSearch_LostFocus(object sender, EventArgs e)
		{
			if (txtSearch.Text == "")
			{
				txtSearch.ForeColor = Themes.ThemeManager.ActiveTheme.SearchBoxTextPromptColor;
				txtSearch.Text = Language.strSearchPrompt;
			}
		}

        //TODO Fix for TreeListView
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

        //TODO Fix for TreeListView
        private void txtSearch_TextChanged(object sender, EventArgs e)
		{
		    try
		    {
                tvConnections.SelectedNode = ConnectionTree.Find(tvConnections.Nodes[0], txtSearch.Text);
            }
		    catch (Exception)
		    {
		    }
		}

        //TODO Fix for TreeListView
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

        //TODO Fix for TreeListView
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