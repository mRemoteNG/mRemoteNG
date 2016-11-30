using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using mRemoteNG.Config.Putty;
using mRemoteNG.Tools;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.Controls;
using WeifenLuo.WinFormsUI.Docking;


namespace mRemoteNG.UI.Window
{
	public partial class ConnectionTreeWindow
	{
	    private ConnectionTreeModel _connectionTreeModel;
	    private readonly ConnectionTreeDragAndDropHandler _dragAndDropHandler = new ConnectionTreeDragAndDropHandler();
        private NodeSearcher _nodeSearcher;
	    private readonly ConnectionContextMenu _contextMenu = new ConnectionContextMenu();
        private readonly PuttySessionsManager _puttySessionsManager = PuttySessionsManager.Instance;

	    public ConnectionInfo SelectedNode => (ConnectionInfo) olvConnections.SelectedObject;

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

            FillImageList();
            LinkModelToView();
		    SetupDropSink();
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
            olvConnections.CanExpandGetter = item =>
            {
                var itemAsContainer = item as ContainerInfo;
                return itemAsContainer?.Children.Count > 0;
            };
            olvConnections.ChildrenGetter = item => ((ContainerInfo)item).Children;
            olvConnections.ContextMenuStrip = _contextMenu;
	    }

	    private void SetupDropSink()
	    {
	        var dropSink = (SimpleDropSink)olvConnections.DropSink;
	        dropSink.CanDropBetween = true;
	    }

	    private static object ConnectionImageGetter(object rowObject)
	    {
            if (rowObject is RootPuttySessionsNodeInfo) return "PuttySessions";
            if (rowObject is RootNodeInfo) return "Root";
	        if (rowObject is ContainerInfo) return "Folder";
	        var connection = rowObject as ConnectionInfo;
	        if (connection == null) return "";
	        return connection.OpenConnections.Count > 0 ? "Play" : "Pause";
	    }

	    private void SetEventHandlers()
	    {
	        SetTreeEventHandlers();
	        SetContextMenuEventHandlers();
            SetMenuEventHandlers();
	    }

	    private void SetTreeEventHandlers()
	    {
	        olvConnections.Collapsed += (sender, args) =>
	        {
	            var container = args.Model as ContainerInfo;
                if (container != null)
                    container.IsExpanded = false;
	        };
            olvConnections.Expanded += (sender, args) =>
            {
                var container = args.Model as ContainerInfo;
                if (container != null)
                    container.IsExpanded = true;
            };
            olvConnections.BeforeLabelEdit += tvConnections_BeforeLabelEdit;
            olvConnections.AfterLabelEdit += tvConnections_AfterLabelEdit;
            olvConnections.SelectionChanged += tvConnections_AfterSelect;
            olvConnections.CellClick += tvConnections_NodeMouseSingleClick;
            olvConnections.CellClick += tvConnections_NodeMouseDoubleClick;
            olvConnections.CellToolTipShowing += tvConnections_CellToolTipShowing;
            olvConnections.ModelCanDrop += _dragAndDropHandler.HandleEvent_ModelCanDrop;
            olvConnections.ModelDropped += _dragAndDropHandler.HandleEvent_ModelDropped;
	        olvConnections.KeyDown += tvConnections_KeyDown;
	        olvConnections.KeyPress += tvConnections_KeyPress;
	    }

	    private void SetContextMenuEventHandlers()
	    {
	        _contextMenu.Opening += (sender, args) => _contextMenu.ShowHideTreeContextMenuItems(SelectedNode);
	        _contextMenu.ConnectClicked += (sender, args) =>
	        {
	            var selectedNodeAsContainer = SelectedNode as ContainerInfo;
                if (selectedNodeAsContainer != null)
                    ConnectionInitiator.OpenConnection(selectedNodeAsContainer, ConnectionInfo.Force.DoNotJump);
                else
                    ConnectionInitiator.OpenConnection(SelectedNode, ConnectionInfo.Force.DoNotJump);
	        };
	        _contextMenu.ConnectToConsoleSessionClicked += (sender, args) =>
	        {
                var selectedNodeAsContainer = SelectedNode as ContainerInfo;
                if (selectedNodeAsContainer != null)
                    ConnectionInitiator.OpenConnection(selectedNodeAsContainer, ConnectionInfo.Force.UseConsoleSession | ConnectionInfo.Force.DoNotJump);
                else
                    ConnectionInitiator.OpenConnection(SelectedNode, ConnectionInfo.Force.UseConsoleSession | ConnectionInfo.Force.DoNotJump);
            };
            _contextMenu.DontConnectToConsoleSessionClicked += (sender, args) =>
            {
                var selectedNodeAsContainer = SelectedNode as ContainerInfo;
                if (selectedNodeAsContainer != null)
                    ConnectionInitiator.OpenConnection(selectedNodeAsContainer, ConnectionInfo.Force.DontUseConsoleSession | ConnectionInfo.Force.DoNotJump);
                else
                    ConnectionInitiator.OpenConnection(SelectedNode, ConnectionInfo.Force.DontUseConsoleSession | ConnectionInfo.Force.DoNotJump);
            };
            _contextMenu.ConnectInFullscreenClicked += (sender, args) =>
            {
                var selectedNodeAsContainer = SelectedNode as ContainerInfo;
                if (selectedNodeAsContainer != null)
                    ConnectionInitiator.OpenConnection(selectedNodeAsContainer, ConnectionInfo.Force.Fullscreen | ConnectionInfo.Force.DoNotJump);
                else
                    ConnectionInitiator.OpenConnection(SelectedNode, ConnectionInfo.Force.Fullscreen | ConnectionInfo.Force.DoNotJump);
            };
            _contextMenu.ConnectWithNoCredentialsClick += (sender, args) =>
            {
                var selectedNodeAsContainer = SelectedNode as ContainerInfo;
                if (selectedNodeAsContainer != null)
                    ConnectionInitiator.OpenConnection(selectedNodeAsContainer, ConnectionInfo.Force.NoCredentials);
                else
                    ConnectionInitiator.OpenConnection(SelectedNode, ConnectionInfo.Force.NoCredentials);
            };
            _contextMenu.ChoosePanelBeforeConnectingClicked += (sender, args) =>
            {
                var selectedNodeAsContainer = SelectedNode as ContainerInfo;
                if (selectedNodeAsContainer != null)
                    ConnectionInitiator.OpenConnection(selectedNodeAsContainer, ConnectionInfo.Force.OverridePanel | ConnectionInfo.Force.DoNotJump);
                else
                    ConnectionInitiator.OpenConnection(SelectedNode, ConnectionInfo.Force.OverridePanel | ConnectionInfo.Force.DoNotJump);
            };
            _contextMenu.DisconnectClicked += (sender, args) => DisconnectConnection(SelectedNode);
            _contextMenu.TransferFileClicked += (sender, args) => SshTransferFile();
            _contextMenu.DuplicateClicked += (sender, args) => DuplicateSelectedNode();
            _contextMenu.RenameClicked += (sender, args) => RenameSelectedNode();
            _contextMenu.DeleteClicked += (sender, args) => DeleteSelectedNode();
            _contextMenu.ImportFileClicked += (sender, args) =>
            {
                var selectedNodeAsContainer = SelectedNode as ContainerInfo ?? SelectedNode.Parent;
                Import.ImportFromFile(selectedNodeAsContainer);
            };
            _contextMenu.ImportActiveDirectoryClicked += (sender, args) => Windows.Show(WindowType.ActiveDirectoryImport);
            _contextMenu.ImportPortScanClicked += (sender, args) => Windows.Show(WindowType.PortScan);
            _contextMenu.ExportFileClicked += (sender, args) => Export.ExportToFile(SelectedNode, Runtime.ConnectionTreeModel);
            _contextMenu.AddConnectionClicked += cMenTreeAddConnection_Click;
            _contextMenu.AddFolderClicked += cMenTreeAddFolder_Click;
            _contextMenu.SortAscendingClicked += (sender, args) => SortNodesRecursive(SelectedNode, ListSortDirection.Ascending);
            _contextMenu.SortDescendingClicked += (sender, args) => SortNodesRecursive(SelectedNode, ListSortDirection.Descending);
            _contextMenu.MoveUpClicked += cMenTreeMoveUp_Click;
            _contextMenu.MoveDownClicked += cMenTreeMoveDown_Click;
            _contextMenu.ExternalToolClicked += (sender, args) => StartExternalApp((ExternalTool)((ToolStripMenuItem)sender).Tag);
	    }

	    private void SetMenuEventHandlers()
	    {
            mMenViewExpandAllFolders.Click += (sender, args) => olvConnections.ExpandAll();
	        mMenViewCollapseAllFolders.Click += (sender, args) =>
	        {
	            olvConnections.CollapseAll();
                olvConnections.Expand(GetRootConnectionNode());
	        };
	        mMenSortAscending.Click += (sender, args) => SortNodesRecursive(GetRootConnectionNode(), ListSortDirection.Ascending);
	    }

	    private void PopulateTreeView()
	    {
	        UnregisterModelUpdateHandlers();
            olvConnections.SetObjects(ConnectionTreeModel.RootNodes);
            RegisterModelUpdateHandlers();
            _nodeSearcher = new NodeSearcher(ConnectionTreeModel);
	        ExpandPreviouslyOpenedFolders();
            ExpandRootConnectionNode();
	        OpenConnectionsFromLastSession();
	    }

        private void RegisterModelUpdateHandlers()
        {
            _puttySessionsManager.PuttySessionsCollectionChanged += OnPuttySessionsCollectionChanged;
            ConnectionTreeModel.CollectionChanged += HandleCollectionChanged;
            ConnectionTreeModel.PropertyChanged += HandleCollectionPropertyChanged;
        }

	    private void UnregisterModelUpdateHandlers()
	    {
            _puttySessionsManager.PuttySessionsCollectionChanged -= OnPuttySessionsCollectionChanged;
            ConnectionTreeModel.CollectionChanged -= HandleCollectionChanged;
            ConnectionTreeModel.PropertyChanged -= HandleCollectionPropertyChanged;
        }

	    private void OnPuttySessionsCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
	    {
            RefreshTreeObjects(GetRootPuttyNodes().ToList());
        }

        private void HandleCollectionPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            //TODO for some reason property changed events are getting triggered twice for each changed property. should be just once. cant find source of duplication
            var property = propertyChangedEventArgs.PropertyName;
            if (property != "Name" && property != "OpenConnections") return;
            var senderAsConnectionInfo = sender as ConnectionInfo;
            if (senderAsConnectionInfo != null)
                RefreshTreeObject(senderAsConnectionInfo);
        }

        private void ExpandRootConnectionNode()
	    {
            var rootConnectionNode = GetRootConnectionNode();
            olvConnections.InvokeExpand(rootConnectionNode);
	    }

	    private RootNodeInfo GetRootConnectionNode()
	    {
            return (RootNodeInfo)olvConnections.Roots.Cast<ConnectionInfo>().First(item => item is RootNodeInfo);
        }

	    private IEnumerable<RootPuttySessionsNodeInfo> GetRootPuttyNodes()
	    {
	        return olvConnections.Objects.OfType<RootPuttySessionsNodeInfo>();
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

	    private void ExpandPreviouslyOpenedFolders()
        {
            var containerList = ConnectionTreeModel.GetRecursiveChildList(GetRootConnectionNode()).OfType<ContainerInfo>();
            var previouslyExpandedNodes = containerList.Where(container => container.IsExpanded);
            olvConnections.ExpandedObjects = previouslyExpandedNodes;
            olvConnections.InvokeRebuildAll(true);
        }

	    private void OpenConnectionsFromLastSession()
        {
            if (!Settings.Default.OpenConsFromLastSession || Settings.Default.NoReconnect) return;
            var connectionInfoList = GetRootConnectionNode().GetRecursiveChildList().Where(node => !(node is ContainerInfo));
            var previouslyOpenedConnections = connectionInfoList.Where(item => item.PleaseConnect);
            foreach (var connectionInfo in previouslyOpenedConnections)
            {
                ConnectionInitiator.OpenConnection(connectionInfo);
            }
        }

        public void EnsureRootNodeVisible()
	    {
            olvConnections.EnsureModelVisible(GetRootConnectionNode());
	    }

        public void DuplicateSelectedNode()
        {
            var newNode = SelectedNode.Clone();
            newNode.Parent.SetChildBelow(newNode, SelectedNode);
            Runtime.SaveConnectionsAsync();
        }

        public void RenameSelectedNode()
        {
            olvConnections.SelectedItem.BeginEdit();
            Runtime.SaveConnectionsAsync();
        }

        public void DeleteSelectedNode()
        {
            if (SelectedNode is RootNodeInfo || SelectedNode is PuttySessionInfo) return;
            if (!UserConfirmsDeletion()) return;
            ConnectionTreeModel.DeleteNode(SelectedNode);
            Runtime.SaveConnectionsAsync();
        }

	    private bool UserConfirmsDeletion()
	    {
	        var selectedNodeAsContainer = SelectedNode as ContainerInfo;
	        if (selectedNodeAsContainer != null)
	            return selectedNodeAsContainer.HasChildren()
	                ? UserConfirmsNonEmptyFolderDeletion()
	                : UserConfirmsEmptyFolderDeletion();
	        return UserConfirmsConnectionDeletion();
	    }

        private bool UserConfirmsEmptyFolderDeletion()
        {
            var messagePrompt = string.Format(Language.strConfirmDeleteNodeFolder, SelectedNode.Name);
            return PromptUser(messagePrompt);
        }

        private bool UserConfirmsNonEmptyFolderDeletion()
        {
            var messagePrompt = string.Format(Language.strConfirmDeleteNodeFolderNotEmpty, SelectedNode.Name);
            return PromptUser(messagePrompt);
        }

        private bool UserConfirmsConnectionDeletion()
        {
            var messagePrompt = string.Format(Language.strConfirmDeleteNodeConnection, SelectedNode.Name);
            return PromptUser(messagePrompt);
        }

        private static bool PromptUser(string promptMessage)
        {
            var msgBoxResponse = MessageBox.Show(promptMessage, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return (msgBoxResponse == DialogResult.Yes);
        }

        #region Private Methods
        private void tvConnections_BeforeLabelEdit(object sender, LabelEditEventArgs e)
        {
            _contextMenu.DisableShortcutKeys();
        }

        private void tvConnections_AfterLabelEdit(object sender, LabelEditEventArgs e)
		{
			try
			{
				_contextMenu.EnableShortcutKeys();
                ConnectionTreeModel.RenameNode(SelectedNode, e.Label);
			    Windows.ConfigForm.SelectedTreeNode = SelectedNode;
                Runtime.SaveConnectionsAsync();
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
                Windows.ConfigForm.SelectedTreeNode = SelectedNode;
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
                var clickedNode = e.Model as ConnectionInfo;

                if (clickedNode == null) return;
                if (clickedNode.GetTreeNodeType() != TreeNodeType.Connection && clickedNode.GetTreeNodeType() != TreeNodeType.PuttySession) return;
                if (Settings.Default.SingleClickOnConnectionOpensIt)
                    ConnectionInitiator.OpenConnection(SelectedNode);

                if (Settings.Default.SingleClickSwitchesToOpenConnection)
                    ConnectionInitiator.SwitchToOpenConnection(SelectedNode);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("tvConnections_NodeMouseClick (UI.Window.ConnectionTreeWindow) failed", ex);
            }
        }

        private void tvConnections_NodeMouseDoubleClick(object sender, CellClickEventArgs e)
        {
            if (e.ClickCount < 2) return;
            var clickedNodeAsContainer = e.Model as ContainerInfo;
            if (clickedNodeAsContainer != null)
            {
                olvConnections.ToggleExpansion(clickedNodeAsContainer);
            }

            var clickedNode = e.Model as ConnectionInfo;
            if (clickedNode?.GetTreeNodeType() == TreeNodeType.Connection |
                clickedNode?.GetTreeNodeType() == TreeNodeType.PuttySession)
			{
                ConnectionInitiator.OpenConnection(SelectedNode);
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

	    private void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
	    {
	        var senderAsContainerInfo = sender as ContainerInfo;
	        // ReSharper disable once SwitchStatementMissingSomeCases
	        switch (args?.Action)
	        {
	            case NotifyCollectionChangedAction.Add:
	                var childList = senderAsContainerInfo?.Children;
	                ConnectionInfo otherChild = null;
                    if (childList?.Count > 1)
                        otherChild = childList.First(child => !args.NewItems.Contains(child)); 
	                RefreshTreeObject(otherChild ?? senderAsContainerInfo);
	                break;
	            case NotifyCollectionChangedAction.Remove:
                    RefreshTreeObjects(args.OldItems);
	                break;
                case NotifyCollectionChangedAction.Move:
                    RefreshTreeObjects(args.OldItems);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    RefreshTreeObject(senderAsContainerInfo);
                    break;
	            case NotifyCollectionChangedAction.Replace:
	                break;
	            case null:
	                break;
	        }
	    }

	    private void RefreshTreeObject(ConnectionInfo modelObject)
	    {
            olvConnections.RefreshObject(modelObject);
	    }

	    private void RefreshTreeObjects(IList modelObjects)
	    {
            olvConnections.RefreshObjects(modelObjects);
	    }
        #endregion

        #region Tree Context Menu
        private void cMenTreeAddConnection_Click(object sender, EventArgs e)
		{
			AddConnection();
            Runtime.SaveConnectionsAsync();
		}

        private void cMenTreeAddFolder_Click(object sender, EventArgs e)
		{
			AddFolder();
            Runtime.SaveConnectionsAsync();
		}

	    private void SortNodesRecursive(ConnectionInfo sortTarget, ListSortDirection sortDirection)
	    {
	        if (sortTarget == null)
	            sortTarget = GetRootConnectionNode();

            var sortTargetAsContainer = sortTarget as ContainerInfo;
            if (sortTargetAsContainer != null)
                sortTargetAsContainer.SortRecursive(sortDirection);
            else
                SelectedNode.Parent.SortRecursive(sortDirection);

            Runtime.SaveConnectionsAsync();
        }

        private void cMenTreeMoveUp_Click(object sender, EventArgs e)
		{
            SelectedNode.Parent.PromoteChild(SelectedNode);
            Runtime.SaveConnectionsAsync();
		}

        private void cMenTreeMoveDown_Click(object sender, EventArgs e)
		{
            SelectedNode.Parent.DemoteChild(SelectedNode);
            Runtime.SaveConnectionsAsync();
		}
        #endregion

        #region Context Menu Actions
        public void AddConnection()
		{
			try
			{
			    AddNode(new ConnectionInfo());
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
                AddNode(new ContainerInfo());
            }
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace(Language.strErrorAddFolderFailed, ex);
			}
		}

	    private void AddNode(ConnectionInfo newNode)
	    {
            if (SelectedNode == null) return;
            DefaultConnectionInfo.Instance.SaveTo(newNode);
	        DefaultConnectionInheritance.Instance.SaveTo(newNode.Inheritance);
            var selectedContainer = SelectedNode as ContainerInfo;
            var parent = selectedContainer ?? SelectedNode?.Parent;
            newNode.SetParent(parent);
            olvConnections.Expand(parent);
            olvConnections.SelectObject(newNode);
            olvConnections.EnsureModelVisible(newNode);
        }

        private void DisconnectConnection(ConnectionInfo connectionInfo)
		{
			try
			{
			    if (connectionInfo == null) return;
			    var nodeAsContainer = connectionInfo as ContainerInfo;
                if (nodeAsContainer != null)
                {
                    foreach (var child in nodeAsContainer.Children)
                    {
                        for (var i = 0; i <= child.OpenConnections.Count - 1; i++)
                        {
                            child.OpenConnections[i].Disconnect();
                        }
                    }
                }
			    else
                {
			        for (var i = 0; i <= connectionInfo.OpenConnections.Count - 1; i++)
			        {
                        connectionInfo.OpenConnections[i].Disconnect();
			        }
			    }
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace("DisconnectConnection (UI.Window.ConnectionTreeWindow) failed", ex);
			}
		}

        private void SshTransferFile()
		{
			try
			{
                Windows.Show(WindowType.SSHTransfer);                
                Windows.SshtransferForm.Hostname = SelectedNode.Hostname;
                Windows.SshtransferForm.Username = SelectedNode.Username;
                Windows.SshtransferForm.Password = SelectedNode.Password;
                Windows.SshtransferForm.Port = Convert.ToString(SelectedNode.Port);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace("SSHTransferFile (UI.Window.ConnectionTreeWindow) failed", ex);
			}
		}

        private void StartExternalApp(ExternalTool externalTool)
		{
			try
			{
                if (SelectedNode.GetTreeNodeType() == TreeNodeType.Connection | SelectedNode.GetTreeNodeType() == TreeNodeType.PuttySession)
                    externalTool.Start(SelectedNode);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace("cMenTreeToolsExternalAppsEntry_Click failed (UI.Window.ConnectionTreeWindow)", ex);
			}
		}
        #endregion

        #region Search
        private void txtSearch_GotFocus(object sender, EventArgs e)
		{
			txtSearch.ForeColor = Themes.ThemeManager.ActiveTheme.SearchBoxTextColor;
			if (txtSearch.Text == Language.strSearchPrompt)
				txtSearch.Text = "";
		}

        private void txtSearch_LostFocus(object sender, EventArgs e)
		{
            if (txtSearch.Text != "") return;
            txtSearch.ForeColor = Themes.ThemeManager.ActiveTheme.SearchBoxTextPromptColor;
            txtSearch.Text = Language.strSearchPrompt;
		}

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Escape)
				{
					e.Handled = true;
				    olvConnections.Focus();
				}
				else if (e.KeyCode == Keys.Up)
				{
                    var match = _nodeSearcher.PreviousMatch();
                    JumpToNode(match);
                    e.Handled = true;
				}
				else if (e.KeyCode == Keys.Down)
				{
				    var match = _nodeSearcher.NextMatch();
				    JumpToNode(match);
                    e.Handled = true;
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
            if (txtSearch.Text == "") return;
            _nodeSearcher?.SearchByName(txtSearch.Text);
            JumpToNode(_nodeSearcher?.CurrentMatch);
        }

	    private void JumpToNode(ConnectionInfo connectionInfo)
	    {
	        if (connectionInfo == null)
	        {
	            olvConnections.SelectedObject = null;
                return;
	        }
	        ExpandParentsRecursive(connectionInfo);
            olvConnections.SelectObject(connectionInfo);
            olvConnections.EnsureModelVisible(connectionInfo);
        }

	    private void ExpandParentsRecursive(ConnectionInfo connectionInfo)
	    {
	        while (true)
	        {
	            if (connectionInfo?.Parent == null) return;
	            olvConnections.Expand(connectionInfo.Parent);
	            connectionInfo = connectionInfo.Parent;
	        }
	    }

	    private void tvConnections_KeyPress(object sender, KeyPressEventArgs e)
		{
			try
			{
			    if (!char.IsLetterOrDigit(e.KeyChar)) return;
			    txtSearch.Text = e.KeyChar.ToString();
			    txtSearch.Focus();
			    txtSearch.SelectionStart = txtSearch.TextLength;
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
					e.Handled = true;
                    ConnectionInitiator.OpenConnection(SelectedNode);
				}
				else if (e.Control && e.KeyCode == Keys.F)
				{
					txtSearch.Focus();
                    txtSearch.SelectAll();
				    e.Handled = true;
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