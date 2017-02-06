using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using mRemoteNG.UI.Controls;
using WeifenLuo.WinFormsUI.Docking;


namespace mRemoteNG.UI.Window
{
	public partial class ConnectionTreeWindow
	{
	    private readonly ConnectionContextMenu _contextMenu;
        private readonly IConnectionInitiator _connectionInitiator = new ConnectionInitiator();


        public ConnectionInfo SelectedNode => olvConnections.SelectedNode;

	    public ConnectionTree ConnectionTree
	    {
	        get { return olvConnections; }
            set { olvConnections = value; }
	    }

	    public ConnectionTreeWindow() : this(new DockContent())
	    {
	    }

		public ConnectionTreeWindow(DockContent panel)
		{
			WindowType = WindowType.Tree;
			DockPnl = panel;
			InitializeComponent();
            _contextMenu = new ConnectionContextMenu(olvConnections);
            olvConnections.ContextMenuStrip = _contextMenu;
            SetMenuEventHandlers();
		    SetConnectionTreeEventHandlers();
		    Settings.Default.PropertyChanged += (sender, args) => SetConnectionTreeEventHandlers();
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
	        olvConnections.NodeDeletionConfirmer = new SelectedConnectionDeletionConfirmer(MessageBox.Show);
            olvConnections.BeforeLabelEdit += tvConnections_BeforeLabelEdit;
            olvConnections.AfterLabelEdit += tvConnections_AfterLabelEdit;
            olvConnections.KeyDown += tvConnections_KeyDown;
            olvConnections.KeyPress += tvConnections_KeyPress;
            SetTreePostSetupActions();
            SetConnectionTreeDoubleClickHandlers();
	        SetConnectionTreeSingleClickHandlers();
	    }

	    private void SetTreePostSetupActions()
	    {
	        var actions = new List<IConnectionTreeDelegate>
	        {
	            new PreviouslyOpenedFolderExpander(),
	            new RootNodeExpander()
	        };

	        if (Settings.Default.OpenConsFromLastSession && !Settings.Default.NoReconnect)
                actions.Add(new PreviousSessionOpener(_connectionInitiator));

	        olvConnections.PostSetupActions = actions;
	    }

	    private void SetConnectionTreeDoubleClickHandlers()
	    {
	        var doubleClickHandler = new TreeNodeCompositeClickHandler
	        {
	            ClickHandlers = new ITreeNodeClickHandler[]
	            {
	                new ExpandNodeClickHandler(olvConnections),
	                new OpenConnectionClickHandler(_connectionInitiator)
	            }
	        };
	        olvConnections.DoubleClickHandler = doubleClickHandler;
	    }

        private void SetConnectionTreeSingleClickHandlers()
        {
            var handlers = new List<ITreeNodeClickHandler>();
            if (Settings.Default.SingleClickOnConnectionOpensIt)
                handlers.Add(new OpenConnectionClickHandler(_connectionInitiator));
            if (Settings.Default.SingleClickSwitchesToOpenConnection)
                handlers.Add(new SwitchToConnectionClickHandler(_connectionInitiator));
            var singleClickHandler = new TreeNodeCompositeClickHandler {ClickHandlers = handlers};
            olvConnections.SingleClickHandler = singleClickHandler;
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
                    _connectionInitiator.OpenConnection(SelectedNode);
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