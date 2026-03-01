using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Config.Connections;
using mRemoteNG.Config.Connections.Multiuser;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Properties;
using mRemoteNG.Themes;
using mRemoteNG.Tree;
using mRemoteNG.Tree.ClickHandlers;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.Controls.ConnectionTree;
using mRemoteNG.UI.Panels;
using mRemoteNG.UI.Tabs;
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
        private ThemeManager? _themeManager;
        private bool _sortedAz = true;
        private bool _suppressSelectionPreview;
        private ToolStripLabel? _syncStatusLabel;
        private RemoteConnectionsSyncronizer? _subscribedSyncronizer;

        public ConnectionInfo SelectedNode => ConnectionTree.SelectedNode;

        public ConnectionTree ConnectionTree { get; set; } = null!;

        public ConnectionTreeWindow() : this(new DockContent())
        {
        }

        public ConnectionTreeWindow(DockContent panel)
        {
            WindowType = WindowType.Tree;
            DockPnl = panel;
            Icon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.ASPWebSite_16x);
            InitializeComponent();
            InitializeSyncStatusLabel();
            SetMenuEventHandlers();
            SetConnectionTreeEventHandlers();
            Settings.Default.PropertyChanged += OnAppSettingsChanged;
            OptionsConnectionsPage.Default.PropertyChanged += OnConnectionsPageSettingChanged;
            ApplyLanguage();
        }

        private void OnAppSettingsChanged(object o, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == nameof(Settings.UseFilterSearch))
            {
                ConnectionTree.UseFiltering = Settings.Default.UseFilterSearch;
                ApplyFiltering();
            }

            PlaceSearchBar(Settings.Default.PlaceSearchBarAboveConnectionTree);
            SetConnectionTreeClickHandlers();
        }

        private void OnConnectionsPageSettingChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(OptionsConnectionsPage.Default.WatchConnectionFile))
                UpdateSyncStatusLabel();
        }

        private void InitializeSyncStatusLabel()
        {
            _syncStatusLabel = new ToolStripLabel
            {
                Alignment = ToolStripItemAlignment.Right,
                Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = SystemColors.GrayText,
                Name = "lblSyncStatus",
                Text = "",
                Visible = false
            };
            msMain.Items.Add(_syncStatusLabel);
        }

        private void SubscribeToSyncronizer()
        {
            RemoteConnectionsSyncronizer? current = Runtime.ConnectionsService.RemoteConnectionsSyncronizer;
            if (current == _subscribedSyncronizer) return;

            if (_subscribedSyncronizer != null)
                _subscribedSyncronizer.ConnectionsReloadedExternally -= OnConnectionsReloadedExternally;

            _subscribedSyncronizer = current;

            if (_subscribedSyncronizer != null)
                _subscribedSyncronizer.ConnectionsReloadedExternally += OnConnectionsReloadedExternally;
        }

        private void OnConnectionsReloadedExternally(object? sender, EventArgs e)
        {
            UpdateSyncStatusLabel();
        }

        private void UpdateSyncStatusLabel()
        {
            if (_syncStatusLabel == null) return;

            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(UpdateSyncStatusLabel));
                return;
            }

            RemoteConnectionsSyncronizer? sync = Runtime.ConnectionsService.RemoteConnectionsSyncronizer;
            if (sync != null)
            {
                if (sync.LastExternalSync.HasValue)
                {
                    string time = sync.LastExternalSync.Value.ToLocalTime().ToString("HH:mm:ss", CultureInfo.InvariantCulture);
                    _syncStatusLabel.Text = $"Synced {time}";
                    _syncStatusLabel.ToolTipText = $"Connection file sync is active. Last external update: {time}";
                }
                else
                {
                    _syncStatusLabel.Text = "Sync on";
                    _syncStatusLabel.ToolTipText = "Connection file sync is active. Watching for external changes.";
                }
                _syncStatusLabel.ForeColor = Color.Green;
                _syncStatusLabel.Visible = true;
            }
            else
            {
                _syncStatusLabel.Visible = false;
            }
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

            SubscribeToSyncronizer();
            UpdateSyncStatusLabel();
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
            if (_themeManager == null || !_themeManager.ThemingActive)
                return;

            ThemeInfo activeTheme = _themeManager.ActiveTheme;
            vsToolStripExtender.SetStyle(msMain, activeTheme.Version, activeTheme.Theme);
            vsToolStripExtender.SetStyle(ConnectionTree.ContextMenuStrip, activeTheme.Version,
                activeTheme.Theme);

            if (!_themeManager.ActiveAndExtended)
                return;

            var extendedPalette = activeTheme.ExtendedPalette;
            if (extendedPalette == null)
                return;

            // connection search area
            searchBoxLayoutPanel.BackColor = extendedPalette.getColor("Dialog_Background");
            searchBoxLayoutPanel.ForeColor = extendedPalette.getColor("Dialog_Foreground");
            txtSearch.BackColor = extendedPalette.getColor("TextBox_Background");
            txtSearch.ForeColor = extendedPalette.getColor("TextBox_Foreground");
            //Picturebox needs to be manually themed
            pbSearch.BackColor = extendedPalette.getColor("TreeView_Background");
            pbClearSearch.BackColor = extendedPalette.getColor("TreeView_Background");
        }

        #endregion

        #region ConnectionTree

        private void SetConnectionTreeEventHandlers()
        {
            ConnectionTree.MultiSelect = true;
            ConnectionTree.NodeDeletionConfirmer =
                new SelectedConnectionDeletionConfirmer(prompt => CTaskDialog.MessageBox(
                    Application.ProductName ?? "", prompt, "", ETaskDialogButtons.YesNo, ESysIcons.Question));
            ConnectionTree.KeyDown += TvConnections_KeyDown;
            ConnectionTree.KeyPress += TvConnections_KeyPress;
            // Preview-on-select disabled: it creates phantom tabs on every tree click,
            // stealing focus from the property grid and confusing navigation.
            // Connections open via double-click (standard behavior).
            // ConnectionTree.SelectionChanged += OnTreeSelectionChangedShowPreview;
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

            actions.Add(new CommandLineConnectionOpener(Runtime.ConnectionInitiator));

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

            // Middle-click always opens connection (standard UX: middle-click = open in new tab)
            List<ITreeNodeClickHandler<ConnectionInfo>> middleClickHandlers = new()
            {
                new OpenConnectionClickHandler(Runtime.ConnectionInitiator)
            };

            ConnectionTree.SingleClickHandler = new TreeNodeCompositeClickHandler { ClickHandlers = singleClickHandlers };
            ConnectionTree.DoubleClickHandler = new TreeNodeCompositeClickHandler { ClickHandlers = doubleClickHandlers };
            ConnectionTree.MiddleClickHandler = new TreeNodeCompositeClickHandler { ClickHandlers = middleClickHandlers };
        }

        private void OnTreeSelectionChangedShowPreview(object sender, EventArgs e)
        {
            if (_suppressSelectionPreview)
            {
                _suppressSelectionPreview = false;
                return;
            }

            try
            {
                ConnectionInfo? selected = ConnectionTree.SelectedNode;
                if (selected == null) return;

                TreeNodeType nodeType = selected.GetTreeNodeType();
                if (nodeType != TreeNodeType.Connection && nodeType != TreeNodeType.PuttySession)
                    return;

                // If the connection has open sessions, switch to its tab (#1921)
                if (selected.OpenConnections.Count > 0)
                {
                    Runtime.ConnectionInitiator.SwitchToOpenConnection(selected);
                    return;
                }

                // Already showing a tab for this connection somewhere — just focus it
                if (Runtime.ConnectionInitiator.SwitchToOpenConnection(selected))
                    return;

                // Determine target panel
                string panelName = !string.IsNullOrEmpty(selected.Panel) ? selected.Panel : "New Panel";

                ConnectionWindow? connectionForm = Runtime.WindowList.FromString(panelName) as ConnectionWindow;
                if (connectionForm == null)
                {
                    connectionForm = PanelAdder.AddPanel(panelName, showImmediately: true);
                    if (connectionForm == null) return;
                }

                ConnectionTab? tab = connectionForm.GetOrAddConnectionTab(selected, switchToConnection: true);
                tab?.ShowClosedState();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(
                    "OnTreeSelectionChangedShowPreview (UI.Window.ConnectionTreeWindow) failed", ex);
            }
        }

        private void ConnectionsServiceOnConnectionsLoaded(object o, ConnectionsLoadedEventArgs connectionsLoadedEventArgs)
        {
            if (ConnectionTree.InvokeRequired)
            {
                ConnectionTree.Invoke(() => ConnectionsServiceOnConnectionsLoaded(o, connectionsLoadedEventArgs));
                return;
            }

            var model = connectionsLoadedEventArgs.NewConnectionTreeModel;
            if (model != null)
            {
                var smartRoot = new mRemoteNG.Tree.Smart.SmartGroupRoot();

                var connected = new mRemoteNG.Tree.Smart.ConnectedGroupNode();
                smartRoot.AddChild(connected);

                var recent = new mRemoteNG.Tree.Smart.RecentGroupNode();
                smartRoot.AddChild(recent);

                model.AddRootNode(smartRoot);

                connected.Initialize();
                recent.Initialize();
            }

            ConnectionTree.ConnectionTreeModel = connectionsLoadedEventArgs.NewConnectionTreeModel;
            ConnectionTree.SelectedObject = connectionsLoadedEventArgs.NewConnectionTreeModel.RootNodes.FirstOrDefault();

            SubscribeToSyncronizer();
            UpdateSyncStatusLabel();
        }

        #endregion

        #region Top Menu

        private void SetMenuEventHandlers()
        {
            mMenViewExpandAllFolders.Click += (sender, args) => ConnectionTree.UserExpandAll();
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
                var connectionTreeModel = Runtime.ConnectionsService.ConnectionTreeModel;
                if (connectionTreeModel == null) return;
                List<ContainerInfo> rootNodes = connectionTreeModel.RootNodes;
                List<ToolStripMenuItem> favoritesList = new();

                foreach (ContainerInfo node in rootNodes)
                {
                    foreach (ConnectionInfo containerInfo in ConnectionTreeModel.GetRecursiveFavoriteChildList(node))
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
            if (((ToolStripMenuItem)sender).Tag is ConnectionInfo connectionInfo)
                Runtime.ConnectionInitiator.OpenConnection(connectionInfo);
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

        /// <summary>
        /// Applies a live filter to the connection tree from the quick-connect toolbar input.
        /// Pass an empty string to remove the filter.
        /// </summary>
        public void FilterByQuickConnect(string text)
        {
            if (string.IsNullOrEmpty(text))
                ConnectionTree.RemoveFilter();
            else
                ConnectionTree.ApplyFilter(text);
        }

        #region Search

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        e.Handled = true;
                        txtSearch.Text = string.Empty;
                        ConnectionTree.Focus();
                        break;
                    case Keys.Up:
                        {
                            ConnectionInfo? match = ConnectionTree.NodeSearcher?.PreviousMatch();
                            JumpToNode(match);
                            e.Handled = true;
                            break;
                        }
                    case Keys.Down:
                        {
                            ConnectionInfo? match = ConnectionTree.NodeSearcher?.NextMatch();
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
            pbClearSearch.Visible = txtSearch.Text.Length > 0
                                    && txtSearch.Text != Language.SearchPrompt;
            ApplyFiltering();
        }

        private void PbClearSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            txtSearch.Focus();
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
                ConnectionTree.NodeSearcher?.SearchByName(txtSearch.Text);
                JumpToNode(ConnectionTree.NodeSearcher?.CurrentMatch);
            }
            else
            {
                if (txtSearch.Text == "") return;
                ConnectionTree.NodeSearcher?.SearchByName(txtSearch.Text);
                JumpToNode(ConnectionTree.NodeSearcher?.CurrentMatch);
            }
        }

        public void JumpToNode(ConnectionInfo? connectionInfo, bool suppressPreview = false)
        {
            _suppressSelectionPreview = suppressPreview;

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
                    if (SelectedNode == null)
                        return;
                    Runtime.ConnectionInitiator.OpenConnection(SelectedNode);
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
