using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Config.Connections;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Properties;
using mRemoteNG.Themes;
using mRemoteNG.Tree;
using mRemoteNG.Tree.ClickHandlers;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.Controls.ConnectionTree;
using mRemoteNG.UI.TaskDialog;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.UI.Window
{
    [SupportedOSPlatform("windows")]
    public partial class ConnectionTreeWindow
    {
        private ThemeManager _themeManager;
        private bool _sortedAz = true;

        public ConnectionInfo SelectedNode => ConnectionTree.SelectedNode;

        public ConnectionTree ConnectionTree { get; set; }

        public ConnectionTreeWindow() : this(new DockContent())
        {
        }

        public ConnectionTreeWindow(DockContent panel)
        {
            WindowType = WindowType.Tree;
            DockPnl = panel;
            Icon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.ASPWebSite_16x);
            InitializeComponent();
            SetMenuEventHandlers();
            SetConnectionTreeEventHandlers();
            Settings.Default.PropertyChanged += OnAppSettingsChanged;
            ApplyLanguage();
        }

        private void OnAppSettingsChanged(object o, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == nameof(Settings.UseFilterSearch))
            {
                ConnectionTree.UseFiltering = Settings.Default.UseFilterSearch;
                ApplyFiltering();
            }

            if (propertyChangedEventArgs.PropertyName == nameof(Settings.SlowClickRenameEnabled))
                ConnectionTree.SetupSlowClickRename();

            PlaceSearchBar(Settings.Default.PlaceSearchBarAboveConnectionTree);
            SetConnectionTreeClickHandlers();
        }

        private void PlaceSearchBar(bool placeSearchBarAboveConnectionTree)
        {
            searchBoxLayoutPanel.Dock = placeSearchBarAboveConnectionTree ? DockStyle.Top : DockStyle.Bottom;
        }


        #region Form Stuff

        private void Tree_Load(object sender, EventArgs e)
        {
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
            Text = Language.Connections;
            TabText = Language.Connections;

            mMenAddConnection.ToolTipText = Language.NewConnection;
            mMenAddFolder.ToolTipText = Language.NewFolder;
            mMenViewExpandAllFolders.ToolTipText = Language.ExpandAllFolders;
            mMenViewCollapseAllFolders.ToolTipText = Language.CollapseAllFolders;
            mMenSort.ToolTipText = Language.Sort;
            mMenFavorites.ToolTipText = Language.Favorites;

            txtSearch.Text = Language.SearchPrompt;
        }

        private new void ApplyTheme()
        {
            if (!_themeManager.ThemingActive)
                return;

            ThemeInfo activeTheme = _themeManager.ActiveTheme;
            vsToolStripExtender.SetStyle(msMain, activeTheme.Version, activeTheme.Theme);
            vsToolStripExtender.SetStyle(ConnectionTree.ContextMenuStrip, activeTheme.Version,
                activeTheme.Theme);

            if (!_themeManager.ActiveAndExtended)
                return;

            // connection search area
            searchBoxLayoutPanel.BackColor = activeTheme.ExtendedPalette.getColor("Dialog_Background");
            searchBoxLayoutPanel.ForeColor = activeTheme.ExtendedPalette.getColor("Dialog_Foreground");
            txtSearch.BackColor = activeTheme.ExtendedPalette.getColor("TextBox_Background");
            txtSearch.ForeColor = activeTheme.ExtendedPalette.getColor("TextBox_Foreground");
            //Picturebox needs to be manually themed
            pbSearch.BackColor = activeTheme.ExtendedPalette.getColor("TreeView_Background");
        }

        #endregion

        #region ConnectionTree

        private void SetConnectionTreeEventHandlers()
        {
            ConnectionTree.NodeDeletionConfirmer =
                new SelectedConnectionDeletionConfirmer(prompt => CTaskDialog.MessageBox(
                    Application.ProductName, prompt, "", ETaskDialogButtons.YesNo, ESysIcons.Question));
            ConnectionTree.KeyDown += TvConnections_KeyDown;
            ConnectionTree.KeyPress += TvConnections_KeyPress;
            SetTreePostSetupActions();
            SetConnectionTreeClickHandlers();
            Runtime.ConnectionsService.ConnectionsLoaded += ConnectionsServiceOnConnectionsLoaded;
        }

        private void SetTreePostSetupActions()
        {
            List<IConnectionTreeDelegate> actions = new()
            {
                new PreviouslyOpenedFolderExpander(),
                new RootNodeExpander()
            };

            if (Properties.OptionsStartupExitPage.Default.OpenConsFromLastSession && !Properties.OptionsAdvancedPage.Default.NoReconnect)
                actions.Add(new PreviousSessionOpener(Runtime.ConnectionInitiator));

            ConnectionTree.PostSetupActions = actions;
        }

        private void SetConnectionTreeClickHandlers()
        {
            List<ITreeNodeClickHandler<ConnectionInfo>> singleClickHandlers = new();
            List<ITreeNodeClickHandler<ConnectionInfo>> doubleClickHandlers = new()
            {
                new ExpandNodeClickHandler(ConnectionTree)
            };

            if (Settings.Default.SingleClickOnConnectionOpensIt)
                singleClickHandlers.Add(new OpenConnectionClickHandler(Runtime.ConnectionInitiator));
            else
                doubleClickHandlers.Add(new OpenConnectionClickHandler(Runtime.ConnectionInitiator));

            if (Settings.Default.SingleClickSwitchesToOpenConnection)
                singleClickHandlers.Add(new SwitchToConnectionClickHandler(Runtime.ConnectionInitiator));

            ConnectionTree.SingleClickHandler = new TreeNodeCompositeClickHandler { ClickHandlers = singleClickHandlers };
            ConnectionTree.DoubleClickHandler = new TreeNodeCompositeClickHandler { ClickHandlers = doubleClickHandlers };
        }

        private void ConnectionsServiceOnConnectionsLoaded(object o, ConnectionsLoadedEventArgs connectionsLoadedEventArgs)
        {
            if (ConnectionTree.InvokeRequired)
            {
                ConnectionTree.Invoke(() => ConnectionsServiceOnConnectionsLoaded(o, connectionsLoadedEventArgs));
                return;
            }

            ConnectionTree.ConnectionTreeModel = connectionsLoadedEventArgs.NewConnectionTreeModel;
            ConnectionTree.SelectedObject = connectionsLoadedEventArgs.NewConnectionTreeModel.RootNodes.FirstOrDefault();
        }

        #endregion

        #region Top Menu

        private void SetMenuEventHandlers()
        {
            mMenViewExpandAllFolders.Click += (sender, args) => ConnectionTree.ExpandAll();
            mMenViewCollapseAllFolders.Click += (sender, args) =>
            {
                ConnectionTree.CollapseAll();
                ConnectionTree.Expand(ConnectionTree.GetRootConnectionNode());
            };
            mMenSort.Click += (sender, args) =>
            {
                if (_sortedAz)
                {
                    ConnectionTree.SortRecursive(ConnectionTree.GetRootConnectionNode(), ListSortDirection.Ascending);
                    mMenSort.Image = Properties.Resources.SortDescending_16x;
                    _sortedAz = false;
                }
                else
                {
                    ConnectionTree.SortRecursive(ConnectionTree.GetRootConnectionNode(), ListSortDirection.Descending);
                    mMenSort.Image = Properties.Resources.SortAscending_16x;
                    _sortedAz = true;
                }
            };
            mMenFavorites.Click += (sender, args) =>
            {
                mMenFavorites.DropDownItems.Clear();
                List<ContainerInfo> rootNodes = Runtime.ConnectionsService.ConnectionTreeModel.RootNodes;
                List<ToolStripMenuItem> favoritesList = new();

                foreach (ContainerInfo node in rootNodes)
                {
                    foreach (ConnectionInfo containerInfo in Runtime.ConnectionsService.ConnectionTreeModel.GetRecursiveFavoriteChildList(node))
                    {
                        ToolStripMenuItem favoriteMenuItem = new()
                        {
                            Text = containerInfo.Name,
                            Tag = containerInfo,
                            Image = containerInfo.OpenConnections.Count > 0 ? Properties.Resources.Run_16x : Properties.Resources.Stop_16x
                        };
                        favoriteMenuItem.MouseUp += FavoriteMenuItem_MouseUp;
                        favoritesList.Add(favoriteMenuItem);
                    }
                }

                mMenFavorites.DropDownItems.AddRange(favoritesList.ToArray());
                mMenFavorites.ShowDropDown();
            };
        }

        private void FavoriteMenuItem_MouseUp(object sender, MouseEventArgs e)
        {
            if (((ToolStripMenuItem)sender).Tag is ContainerInfo) return;
            Runtime.ConnectionInitiator.OpenConnection((ConnectionInfo)((ToolStripMenuItem)sender).Tag);
        }

        #endregion

        #region Tree Context Menu

        private void CMenTreeAddConnection_Click(object sender, EventArgs e)
        {
            ConnectionTree.AddConnection();
        }

        private void CMenTreeAddFolder_Click(object sender, EventArgs e)
        {
            ConnectionTree.AddFolder();
        }

        #endregion

        #region Search

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        e.Handled = true;
                        ConnectionTree.Focus();
                        break;
                    case Keys.Up:
                        {
                            ConnectionInfo? match = ConnectionTree.NodeSearcher.PreviousMatch();
                            JumpToNode(match);
                            e.Handled = true;
                            break;
                        }
                    case Keys.Down:
                        {
                            ConnectionInfo? match = ConnectionTree.NodeSearcher.NextMatch();
                            JumpToNode(match);
                            e.Handled = true;
                            break;
                        }
                    default:
                        TvConnections_KeyDown(sender, e);
                        break;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("txtSearch_KeyDown (UI.Window.ConnectionTreeWindow) failed", ex);
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFiltering();
        }

        private void ApplyFiltering()
        {
            if (Settings.Default.UseFilterSearch)
            {
                if (txtSearch.Text == "" || txtSearch.Text == Language.SearchPrompt)
                {
                    ConnectionTree.RemoveFilter();
                    return;
                }

                ConnectionTree.ApplyFilter(txtSearch.Text);
            }
            else
            {
                if (txtSearch.Text == "") return;
                ConnectionTree.NodeSearcher?.SearchByName(txtSearch.Text);
                JumpToNode(ConnectionTree.NodeSearcher?.CurrentMatch);
            }
        }

        public void JumpToNode(ConnectionInfo? connectionInfo)
        {
            if (connectionInfo == null)
            {
                ConnectionTree.SelectedObject = null;
                return;
            }

            ExpandParentsRecursive(connectionInfo);
            ConnectionTree.SelectObject(connectionInfo);
            ConnectionTree.EnsureModelVisible(connectionInfo);
        }

        private void ExpandParentsRecursive(ConnectionInfo connectionInfo)
        {
            while (true)
            {
                if (connectionInfo?.Parent == null) return;
                ConnectionTree.Expand(connectionInfo.Parent);
                connectionInfo = connectionInfo.Parent;
            }
        }

        private void TvConnections_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                // Suppress the beep sound for Enter key
                if (e.KeyChar == (char)Keys.Return)
                {
                    e.Handled = true;
                    return;
                }

                if (!char.IsLetterOrDigit(e.KeyChar)) return;
                txtSearch.Focus();
                txtSearch.Text = e.KeyChar.ToString();
                txtSearch.SelectionStart = txtSearch.TextLength;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("tvConnections_KeyPress (UI.Window.ConnectionTreeWindow) failed", ex);
            }
        }

        private void TvConnections_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;

                    if (Settings.Default.OpenMultipleConnectionsWithEnter)
                    {
                        HandleEnterKeyMultiSelect();
                    }
                    else
                    {
                        if (SelectedNode == null)
                            return;
                        Runtime.ConnectionInitiator.OpenConnection(SelectedNode);
                    }
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

        /// <summary>
        /// Handles opening multiple selected connections when Enter is pressed.
        /// Opens explicitly selected connections, or if none are selected, opens direct children of selected folders.
        /// </summary>
        private void HandleEnterKeyMultiSelect()
        {
            var connectionsToOpen = GetExplicitConnectionsToOpen();

            if (connectionsToOpen.Count == 0)
            {
                connectionsToOpen.AddRange(GetFolderConnectionsToOpen());
            }

            foreach (var connection in connectionsToOpen)
            {
                Runtime.ConnectionInitiator.OpenConnection(connection);
            }
        }

        /// <summary>
        /// Gets explicitly selected connections that are not already open.
        /// </summary>
        private List<ConnectionInfo> GetExplicitConnectionsToOpen()
        {
            return ConnectionTree.SelectedObjects
                .OfType<ConnectionInfo>()
                .Where(n => n.GetTreeNodeType() == TreeNodeType.Connection
                         || n.GetTreeNodeType() == TreeNodeType.PuttySession)
                .Where(n => n.OpenConnections.Count == 0)
                .ToList();
        }

        /// <summary>
        /// Gets direct child connections from selected folders that are not already open.
        /// </summary>
        private List<ConnectionInfo> GetFolderConnectionsToOpen()
        {
            var connectionsFromFolders = new List<ConnectionInfo>();
            var selectedFolders = ConnectionTree.SelectedObjects
                .OfType<ConnectionInfo>()
                .Where(n => n.GetTreeNodeType() == TreeNodeType.Container)
                .ToList();

            foreach (var folder in selectedFolders)
            {
                var directChildren = GetDirectChildConnections(folder)
                    .Where(n => n.OpenConnections.Count == 0)
                    .ToList();
                connectionsFromFolders.AddRange(directChildren);
            }

            return connectionsFromFolders;
        }

        /// <summary>
        /// Gets direct child connections of a folder, excluding connections in nested subfolders.
        /// </summary>
        private static List<ConnectionInfo> GetDirectChildConnections(ConnectionInfo folder)
        {
            var directChildren = new List<ConnectionInfo>();

            if (folder is not ContainerInfo container)
                return directChildren;

            foreach (var child in container.Children)
            {
                if (child.GetTreeNodeType() == TreeNodeType.Connection || 
                    child.GetTreeNodeType() == TreeNodeType.PuttySession)
                {
                    directChildren.Add(child);
                }
            }

            return directChildren;
        }

        /// <summary>
        /// Public wrapper for testing GetDirectChildConnections method.
        /// </summary>
        public static List<ConnectionInfo> PublicGetDirectChildConnections(ConnectionInfo folder)
        {
            return GetDirectChildConnections(folder);
        }

        #endregion
    }
}
