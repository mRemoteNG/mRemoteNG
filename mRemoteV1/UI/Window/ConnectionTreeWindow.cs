using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using mRemoteNG.Tools;
using mRemoteNG.UI.Controls;
using WeifenLuo.WinFormsUI.Docking;


namespace mRemoteNG.UI.Window
{
	public partial class ConnectionTreeWindow
	{
	    private readonly ConnectionContextMenu _contextMenu = new ConnectionContextMenu();

	    public ConnectionInfo SelectedNode => olvConnections.SelectedNode;

	    public ConnectionTree ConnectionTree
	    {
	        get { return olvConnections; }
            set { olvConnections = value; }
	    }

		public ConnectionTreeWindow(DockContent panel)
		{
			WindowType = WindowType.Tree;
			DockPnl = panel;
			InitializeComponent();
            olvConnections.ContextMenuStrip = _contextMenu;
            SetMenuEventHandlers();
            SetContextMenuEventHandlers();
		    SetConnectionTreeEventHandlers();
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

        #region ConnectionTree
	    private void SetConnectionTreeEventHandlers()
	    {
	        olvConnections.DeletionConfirmer = ConnectionDeletionConfirmer.UserConfirmsDeletion;
            olvConnections.BeforeLabelEdit += tvConnections_BeforeLabelEdit;
            olvConnections.AfterLabelEdit += tvConnections_AfterLabelEdit;
            olvConnections.KeyDown += tvConnections_KeyDown;
            olvConnections.KeyPress += tvConnections_KeyPress;
        }
        #endregion

        #region Top Menu
        private void SetMenuEventHandlers()
        {
            mMenViewExpandAllFolders.Click += (sender, args) => olvConnections.ExpandAll();
            mMenViewCollapseAllFolders.Click += (sender, args) =>
            {
                olvConnections.CollapseAll();
                olvConnections.Expand(olvConnections.GetRootConnectionNode());
            };
            mMenSortAscending.Click += (sender, args) => SortNodesRecursive(olvConnections.GetRootConnectionNode(), ListSortDirection.Ascending);
        }
        #endregion

        #region Tree Context Menu
        private void SetContextMenuEventHandlers()
        {
            _contextMenu.Opening += (sender, args) => _contextMenu.ShowHideMenuItems(SelectedNode);
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
            _contextMenu.DisconnectClicked += (sender, args) => olvConnections.DisconnectConnection(SelectedNode);
            _contextMenu.TransferFileClicked += (sender, args) => olvConnections.SshTransferFile();
            _contextMenu.DuplicateClicked += (sender, args) => olvConnections.DuplicateSelectedNode();
            _contextMenu.RenameClicked += (sender, args) => olvConnections.RenameSelectedNode();
            _contextMenu.DeleteClicked += (sender, args) => olvConnections.DeleteSelectedNode();
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
            _contextMenu.ExternalToolClicked += (sender, args) => olvConnections.StartExternalApp((ExternalTool)((ToolStripMenuItem)sender).Tag);
        }

        private void cMenTreeAddConnection_Click(object sender, EventArgs e)
		{
			olvConnections.AddConnection();
            Runtime.SaveConnectionsAsync();
		}

        private void cMenTreeAddFolder_Click(object sender, EventArgs e)
		{
            olvConnections.AddFolder();
            Runtime.SaveConnectionsAsync();
		}

	    private void SortNodesRecursive(ConnectionInfo sortTarget, ListSortDirection sortDirection)
	    {
	        if (sortTarget == null)
	            sortTarget = olvConnections.GetRootConnectionNode();

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

        private void tvConnections_BeforeLabelEdit(object sender, LabelEditEventArgs e)
        {
            _contextMenu.DisableShortcutKeys();
        }

        private void tvConnections_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            try
            {
                _contextMenu.EnableShortcutKeys();
                ConnectionTree.ConnectionTreeModel.RenameNode(SelectedNode, e.Label);
                Windows.ConfigForm.SelectedTreeNode = SelectedNode;
                Runtime.SaveConnectionsAsync();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("tvConnections_AfterLabelEdit (UI.Window.ConnectionTreeWindow) failed", ex);
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
                    var match = olvConnections.NodeSearcher.PreviousMatch();
                    JumpToNode(match);
                    e.Handled = true;
				}
				else if (e.KeyCode == Keys.Down)
				{
				    var match = olvConnections.NodeSearcher.NextMatch();
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
            olvConnections.NodeSearcher?.SearchByName(txtSearch.Text);
            JumpToNode(olvConnections.NodeSearcher?.CurrentMatch);
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