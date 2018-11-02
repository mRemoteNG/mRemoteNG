using mRemoteNG.App;
using mRemoteNG.Config.Connections;
using mRemoteNG.Connection;
using mRemoteNG.Themes;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.UI.Window
{
    public partial class ConnectionTreeWindow
	{
        private readonly IConnectionInitiator _connectionInitiator = new ConnectionInitiator();
		private ThemeManager _themeManager;

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
			SetMenuEventHandlers();
		    SetConnectionTreeEventHandlers();
		    Settings.Default.PropertyChanged += OnAppSettingsChanged;
        }

	    private void OnAppSettingsChanged(object o, PropertyChangedEventArgs propertyChangedEventArgs)
	    {
	        if (propertyChangedEventArgs.PropertyName == nameof(Settings.UseFilterSearch))
	        {
	            ConnectionTree.UseFiltering = Settings.Default.UseFilterSearch;
	            ApplyFiltering();
            }
	    }


	    #region Form Stuff
        private void Tree_Load(object sender, EventArgs e)
        {
            ApplyLanguage();
            //work on the theme change
            _themeManager = ThemeManager.getInstance();
            _themeManager.ThemeChanged += ApplyTheme;
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

        private new void ApplyTheme()
        {
            if (!_themeManager.ThemingActive) return;
            vsToolStripExtender.SetStyle(msMain, _themeManager.ActiveTheme.Version, _themeManager.ActiveTheme.Theme);
            vsToolStripExtender.SetStyle(olvConnections.ContextMenuStrip, _themeManager.ActiveTheme.Version, _themeManager.ActiveTheme.Theme);
            //Treelistview need to be manually themed
            olvConnections.BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TreeView_Background");
            olvConnections.ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TreeView_Foreground");
            olvConnections.SelectedBackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Treeview_SelectedItem_Active_Background");
            olvConnections.SelectedForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Treeview_SelectedItem_Active_Foreground"); 
            olvConnections.UnfocusedSelectedBackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Treeview_SelectedItem_Inactive_Background"); 
            olvConnections.UnfocusedSelectedForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Treeview_SelectedItem_Inactive_Foreground");
            //There is a border around txtSearch that dont theme well
            txtSearch.BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Background");
            txtSearch.ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Foreground");
        }
        #endregion

        #region ConnectionTree
	    private void SetConnectionTreeEventHandlers()
	    {
	        olvConnections.NodeDeletionConfirmer = new SelectedConnectionDeletionConfirmer(MessageBox.Show);
            olvConnections.KeyDown += tvConnections_KeyDown;
            olvConnections.KeyPress += tvConnections_KeyPress;
            SetTreePostSetupActions();
            SetConnectionTreeDoubleClickHandlers();
	        SetConnectionTreeSingleClickHandlers();
	        Runtime.ConnectionsService.ConnectionsLoaded += ConnectionsServiceOnConnectionsLoaded;
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
	            ClickHandlers = new ITreeNodeClickHandler<ConnectionInfo>[]
	            {
	                new ExpandNodeClickHandler(olvConnections),
	                new OpenConnectionClickHandler(_connectionInitiator)
	            }
	        };
	        olvConnections.DoubleClickHandler = doubleClickHandler;
	    }

        private void SetConnectionTreeSingleClickHandlers()
        {
            var handlers = new List<ITreeNodeClickHandler<ConnectionInfo>>();
            if (Settings.Default.SingleClickOnConnectionOpensIt)
                handlers.Add(new OpenConnectionClickHandler(_connectionInitiator));
            if (Settings.Default.SingleClickSwitchesToOpenConnection)
                handlers.Add(new SwitchToConnectionClickHandler(_connectionInitiator));
            var singleClickHandler = new TreeNodeCompositeClickHandler {ClickHandlers = handlers};
            olvConnections.SingleClickHandler = singleClickHandler;
        }

	    private void ConnectionsServiceOnConnectionsLoaded(object o, ConnectionsLoadedEventArgs connectionsLoadedEventArgs)
	    {
	        if (olvConnections.InvokeRequired)
	        {
	            olvConnections.Invoke(() => ConnectionsServiceOnConnectionsLoaded(o, connectionsLoadedEventArgs));
                return;
	        }

	        olvConnections.ConnectionTreeModel = connectionsLoadedEventArgs.NewConnectionTreeModel;
	        olvConnections.SelectedObject = connectionsLoadedEventArgs.NewConnectionTreeModel.RootNodes
	            .OfType<RootNodeInfo>().FirstOrDefault();
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
            mMenSortAscending.Click += (sender, args) => olvConnections.SortRecursive(olvConnections.GetRootConnectionNode(), ListSortDirection.Ascending);
        }
        #endregion

        #region Tree Context Menu
        private void cMenTreeAddConnection_Click(object sender, EventArgs e)
		{
			olvConnections.AddConnection();
		}

        private void cMenTreeAddFolder_Click(object sender, EventArgs e)
		{
            olvConnections.AddFolder();
		}
        #endregion

        #region Search
        private void txtSearch_GotFocus(object sender, EventArgs e)
		{
			if (txtSearch.Text == Language.strSearchPrompt)
				txtSearch.Text = "";
		}

        private void txtSearch_LostFocus(object sender, EventArgs e)
		{
            if (txtSearch.Text != "") return;
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
            ApplyFiltering();
        }

	    private void ApplyFiltering()
	    {
	        if (Settings.Default.UseFilterSearch)
	        {
	            if (txtSearch.Text == "" || txtSearch.Text == Language.strSearchPrompt)
	            {
	                olvConnections.RemoveFilter();
	                return;
	            }
	            olvConnections.ApplyFilter(txtSearch.Text);
	        }
	        else
	        {
	            if (txtSearch.Text == "") return;
	            olvConnections.NodeSearcher?.SearchByName(txtSearch.Text);
	            JumpToNode(olvConnections.NodeSearcher?.CurrentMatch);
	        }
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