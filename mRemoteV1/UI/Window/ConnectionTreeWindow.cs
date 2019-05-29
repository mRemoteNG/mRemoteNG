﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Config.Connections;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Themes;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.TaskDialog;
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
            Text = Language.strConnections;
            TabText = Language.strConnections;

            mMenAddConnection.ToolTipText = Language.strAddConnection;
            mMenAddFolder.ToolTipText = Language.strAddFolder;
            mMenViewExpandAllFolders.ToolTipText = Language.strExpandAllFolders;
            mMenViewCollapseAllFolders.ToolTipText = Language.strCollapseAllFolders;
            mMenSortAscending.ToolTipText = Language.strSortAsc;
            mMenFavorites.ToolTipText = Language.Favorites;

            txtSearch.Text = Language.strSearchPrompt;
        }

        private new void ApplyTheme()
        {
            if (!_themeManager.ThemingActive)
                return;

            var activeTheme = _themeManager.ActiveTheme;
            vsToolStripExtender.SetStyle(msMain, activeTheme.Version, activeTheme.Theme);
            vsToolStripExtender.SetStyle(olvConnections.ContextMenuStrip, activeTheme.Version,
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
            olvConnections.NodeDeletionConfirmer = new SelectedConnectionDeletionConfirmer(prompt =>
                                                                                               CTaskDialog
                                                                                                   .MessageBox(Application.ProductName,
                                                                                                               prompt,
                                                                                                               "",
                                                                                                               ETaskDialogButtons
                                                                                                                   .YesNo,
                                                                                                               ESysIcons
                                                                                                                   .Question));
            olvConnections.KeyDown += tvConnections_KeyDown;
            olvConnections.KeyPress += tvConnections_KeyPress;
            SetTreePostSetupActions();
            SetConnectionTreeClickHandlers();
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

        private void SetConnectionTreeClickHandlers()
        {
            var singleClickHandlers = new List<ITreeNodeClickHandler<ConnectionInfo>>();
            var doubleClickHandlers = new List<ITreeNodeClickHandler<ConnectionInfo>>
            {
                new ExpandNodeClickHandler(olvConnections)
            };

            if (Settings.Default.SingleClickOnConnectionOpensIt)
                singleClickHandlers.Add(new OpenConnectionClickHandler(_connectionInitiator));
            else
                doubleClickHandlers.Add(new OpenConnectionClickHandler(_connectionInitiator));

            if (Settings.Default.SingleClickSwitchesToOpenConnection)
                singleClickHandlers.Add(new SwitchToConnectionClickHandler(_connectionInitiator));

            olvConnections.SingleClickHandler = new TreeNodeCompositeClickHandler {ClickHandlers = singleClickHandlers};
            olvConnections.DoubleClickHandler = new TreeNodeCompositeClickHandler {ClickHandlers = doubleClickHandlers};
        }

        private void ConnectionsServiceOnConnectionsLoaded(object o,
                                                           ConnectionsLoadedEventArgs connectionsLoadedEventArgs)
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
            mMenFavorites.Click += (sender, args) =>
            {
                mMenFavorites.DropDownItems.Clear();
                var rootNodes = Runtime.ConnectionsService.ConnectionTreeModel.RootNodes;
                List<ToolStripMenuItem> favoritesList = new List<ToolStripMenuItem>();

                foreach (var node in rootNodes)
                {
                    foreach (var containerInfo in Runtime.ConnectionsService.ConnectionTreeModel.GetRecursiveFavoriteChildList(node))
                    {
                        var favoriteMenuItem = new ToolStripMenuItem
                        {
                            Text = containerInfo.Name,
                            Tag = containerInfo,
                            Image = containerInfo.OpenConnections.Count > 0 ? Resources.Play : Resources.Pause
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
            _connectionInitiator.OpenConnection((ConnectionInfo)((ToolStripMenuItem)sender).Tag);
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

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        e.Handled = true;
                        olvConnections.Focus();
                        break;
                    case Keys.Up:
                    {
                        var match = olvConnections.NodeSearcher.PreviousMatch();
                        JumpToNode(match);
                        e.Handled = true;
                        break;
                    }
                    case Keys.Down:
                    {
                        var match = olvConnections.NodeSearcher.NextMatch();
                        JumpToNode(match);
                        e.Handled = true;
                        break;
                    }
                    default:
                        tvConnections_KeyDown(sender, e);
                        break;
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

        public void JumpToNode(ConnectionInfo connectionInfo)
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
                txtSearch.Focus();
                txtSearch.Text = e.KeyChar.ToString();
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
                    if (SelectedNode == null)
                        return;
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