using System;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Root.PuttySessions;
using mRemoteNG.Tools;
using mRemoteNG.Tree.Root;


namespace mRemoteNG.UI.Controls
{
    internal class ConnectionContextMenu : ContextMenuStrip
    {
        private ToolStripMenuItem _cMenTreeAddConnection;
        private ToolStripMenuItem _cMenTreeAddFolder;
        private ToolStripSeparator _cMenTreeSep1;
        private ToolStripMenuItem _cMenTreeConnect;
        private ToolStripMenuItem _cMenTreeConnectWithOptions;
        private ToolStripMenuItem _cMenTreeConnectWithOptionsConnectToConsoleSession;
        private ToolStripMenuItem _cMenTreeConnectWithOptionsNoCredentials;
        private ToolStripMenuItem _cMenTreeConnectWithOptionsConnectInFullscreen;
        private ToolStripMenuItem _cMenTreeDisconnect;
        private ToolStripSeparator _cMenTreeSep2;
        private ToolStripMenuItem _cMenTreeToolsTransferFile;
        private ToolStripMenuItem _cMenTreeToolsSort;
        private ToolStripMenuItem _cMenTreeToolsSortAscending;
        private ToolStripMenuItem _cMenTreeToolsSortDescending;
        private ToolStripSeparator _cMenTreeSep3;
        private ToolStripMenuItem _cMenTreeRename;
        private ToolStripMenuItem _cMenTreeDelete;
        private ToolStripSeparator _cMenTreeSep4;
        private ToolStripMenuItem _cMenTreeMoveUp;
        private ToolStripMenuItem _cMenTreeMoveDown;
        private PictureBox _pictureBox1;
        private ToolStripMenuItem _cMenTreeToolsExternalApps;
        private ToolStripMenuItem _cMenTreeDuplicate;
        private ToolStripMenuItem _cMenTreeConnectWithOptionsChoosePanelBeforeConnecting;
        private ToolStripMenuItem _cMenTreeConnectWithOptionsDontConnectToConsoleSession;
        private ToolStripMenuItem _cMenTreeImport;
        private ToolStripMenuItem _cMenTreeExportFile;
        private ToolStripSeparator _toolStripSeparator1;
        private ToolStripMenuItem _cMenTreeImportFile;
        private ToolStripMenuItem _cMenTreeImportActiveDirectory;
        private ToolStripMenuItem _cMenTreeImportPortScan;


        public ConnectionContextMenu()
        {
            InitializeComponent();
            ApplyLanguage();
            Opening += (sender, args) => AddExternalApps();
        }

        private void InitializeComponent()
        {
            _cMenTreeConnect = new ToolStripMenuItem();
            _cMenTreeConnectWithOptions = new ToolStripMenuItem();
            _cMenTreeConnectWithOptionsConnectToConsoleSession = new ToolStripMenuItem();
            _cMenTreeConnectWithOptionsDontConnectToConsoleSession = new ToolStripMenuItem();
            _cMenTreeConnectWithOptionsConnectInFullscreen = new ToolStripMenuItem();
            _cMenTreeConnectWithOptionsNoCredentials = new ToolStripMenuItem();
            _cMenTreeConnectWithOptionsChoosePanelBeforeConnecting = new ToolStripMenuItem();
            _cMenTreeDisconnect = new ToolStripMenuItem();
            _cMenTreeSep1 = new ToolStripSeparator();
            _cMenTreeToolsExternalApps = new ToolStripMenuItem();
            _cMenTreeToolsTransferFile = new ToolStripMenuItem();
            _cMenTreeSep2 = new ToolStripSeparator();
            _cMenTreeDuplicate = new ToolStripMenuItem();
            _cMenTreeRename = new ToolStripMenuItem();
            _cMenTreeDelete = new ToolStripMenuItem();
            _cMenTreeSep3 = new ToolStripSeparator();
            _cMenTreeImport = new ToolStripMenuItem();
            _cMenTreeImportFile = new ToolStripMenuItem();
            _cMenTreeImportActiveDirectory = new ToolStripMenuItem();
            _cMenTreeImportPortScan = new ToolStripMenuItem();
            _cMenTreeExportFile = new ToolStripMenuItem();
            _cMenTreeSep4 = new ToolStripSeparator();
            _cMenTreeAddConnection = new ToolStripMenuItem();
            _cMenTreeAddFolder = new ToolStripMenuItem();
            _toolStripSeparator1 = new ToolStripSeparator();
            _cMenTreeToolsSort = new ToolStripMenuItem();
            _cMenTreeToolsSortAscending = new ToolStripMenuItem();
            _cMenTreeToolsSortDescending = new ToolStripMenuItem();
            _cMenTreeMoveUp = new ToolStripMenuItem();
            _cMenTreeMoveDown = new ToolStripMenuItem();


            // 
            // cMenTree
            // 
            Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            Items.AddRange(new ToolStripItem[] {
                _cMenTreeConnect,
                _cMenTreeConnectWithOptions,
                _cMenTreeDisconnect,
                _cMenTreeSep1,
                _cMenTreeToolsExternalApps,
                _cMenTreeToolsTransferFile,
                _cMenTreeSep2,
                _cMenTreeDuplicate,
                _cMenTreeRename,
                _cMenTreeDelete,
                _cMenTreeSep3,
                _cMenTreeImport,
                _cMenTreeExportFile,
                _cMenTreeSep4,
                _cMenTreeAddConnection,
                _cMenTreeAddFolder,
                _toolStripSeparator1,
                _cMenTreeToolsSort,
                _cMenTreeMoveUp,
                _cMenTreeMoveDown
            });
            Name = "cMenTree";
            RenderMode = ToolStripRenderMode.Professional;
            Size = new System.Drawing.Size(200, 364);
            // 
            // cMenTreeConnect
            // 
            _cMenTreeConnect.Image = Resources.Play;
            _cMenTreeConnect.Name = "_cMenTreeConnect";
            _cMenTreeConnect.ShortcutKeys = ((Keys.Control | Keys.Shift) | Keys.C);
            _cMenTreeConnect.Size = new System.Drawing.Size(199, 22);
            _cMenTreeConnect.Text = "Connect";
            _cMenTreeConnect.Click += (sender, args) => OnConnectClicked(args);
            // 
            // cMenTreeConnectWithOptions
            // 
            _cMenTreeConnectWithOptions.DropDownItems.AddRange(new ToolStripItem[] {
                _cMenTreeConnectWithOptionsConnectToConsoleSession,
                _cMenTreeConnectWithOptionsDontConnectToConsoleSession,
                _cMenTreeConnectWithOptionsConnectInFullscreen,
                _cMenTreeConnectWithOptionsNoCredentials,
                _cMenTreeConnectWithOptionsChoosePanelBeforeConnecting
            });
            _cMenTreeConnectWithOptions.Name = "_cMenTreeConnectWithOptions";
            _cMenTreeConnectWithOptions.Size = new System.Drawing.Size(199, 22);
            _cMenTreeConnectWithOptions.Text = "Connect (with options)";
            // 
            // cMenTreeConnectWithOptionsConnectToConsoleSession
            // 
            _cMenTreeConnectWithOptionsConnectToConsoleSession.Image = Resources.monitor_go;
            _cMenTreeConnectWithOptionsConnectToConsoleSession.Name = "_cMenTreeConnectWithOptionsConnectToConsoleSession";
            _cMenTreeConnectWithOptionsConnectToConsoleSession.Size = new System.Drawing.Size(245, 22);
            _cMenTreeConnectWithOptionsConnectToConsoleSession.Text = "Connect to console session";
            _cMenTreeConnectWithOptionsConnectToConsoleSession.Click += (sender, args) => OnConnectToConsoleSessionClicked(args);
            // 
            // cMenTreeConnectWithOptionsDontConnectToConsoleSession
            // 
            _cMenTreeConnectWithOptionsDontConnectToConsoleSession.Image = Resources.monitor_delete;
            _cMenTreeConnectWithOptionsDontConnectToConsoleSession.Name = "_cMenTreeConnectWithOptionsDontConnectToConsoleSession";
            _cMenTreeConnectWithOptionsDontConnectToConsoleSession.Size = new System.Drawing.Size(245, 22);
            _cMenTreeConnectWithOptionsDontConnectToConsoleSession.Text = "Don\'t connect to console session";
            _cMenTreeConnectWithOptionsDontConnectToConsoleSession.Visible = false;
            _cMenTreeConnectWithOptionsDontConnectToConsoleSession.Click += (sender, args) => OnDontConnectToConsoleSessionClicked(args);
            // 
            // cMenTreeConnectWithOptionsConnectInFullscreen
            // 
            _cMenTreeConnectWithOptionsConnectInFullscreen.Image = Resources.arrow_out;
            _cMenTreeConnectWithOptionsConnectInFullscreen.Name = "_cMenTreeConnectWithOptionsConnectInFullscreen";
            _cMenTreeConnectWithOptionsConnectInFullscreen.Size = new System.Drawing.Size(245, 22);
            _cMenTreeConnectWithOptionsConnectInFullscreen.Text = "Connect in fullscreen";
            _cMenTreeConnectWithOptionsConnectInFullscreen.Click += (sender, args) => OnConnectInFullscreenClicked(args);
            // 
            // cMenTreeConnectWithOptionsNoCredentials
            // 
            _cMenTreeConnectWithOptionsNoCredentials.Image = Resources.key_delete;
            _cMenTreeConnectWithOptionsNoCredentials.Name = "_cMenTreeConnectWithOptionsNoCredentials";
            _cMenTreeConnectWithOptionsNoCredentials.Size = new System.Drawing.Size(245, 22);
            _cMenTreeConnectWithOptionsNoCredentials.Text = "Connect without credentials";
            _cMenTreeConnectWithOptionsNoCredentials.Click += (sender, args) => OnConnectWithNoCredentialsClick(args);
            // 
            // cMenTreeConnectWithOptionsChoosePanelBeforeConnecting
            // 
            _cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Image = Resources.Panels;
            _cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Name = "_cMenTreeConnectWithOptionsChoosePanelBeforeConnecting";
            _cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Size = new System.Drawing.Size(245, 22);
            _cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Text = "Choose panel before connecting";
            _cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Click += (sender, args) => OnChoosePanelBeforeConnectingClicked(args);
            // 
            // cMenTreeDisconnect
            // 
            _cMenTreeDisconnect.Image = Resources.Pause;
            _cMenTreeDisconnect.Name = "_cMenTreeDisconnect";
            _cMenTreeDisconnect.Size = new System.Drawing.Size(199, 22);
            _cMenTreeDisconnect.Text = "Disconnect";
            _cMenTreeDisconnect.Click += (sender, args) => OnDisconnectClicked(args);
            // 
            // cMenTreeSep1
            // 
            _cMenTreeSep1.Name = "_cMenTreeSep1";
            _cMenTreeSep1.Size = new System.Drawing.Size(196, 6);
            // 
            // cMenTreeToolsExternalApps
            // 
            _cMenTreeToolsExternalApps.Image = Resources.ExtApp;
            _cMenTreeToolsExternalApps.Name = "_cMenTreeToolsExternalApps";
            _cMenTreeToolsExternalApps.Size = new System.Drawing.Size(199, 22);
            _cMenTreeToolsExternalApps.Text = "External Applications";
            // 
            // cMenTreeToolsTransferFile
            // 
            _cMenTreeToolsTransferFile.Image = Resources.SSHTransfer;
            _cMenTreeToolsTransferFile.Name = "_cMenTreeToolsTransferFile";
            _cMenTreeToolsTransferFile.Size = new System.Drawing.Size(199, 22);
            _cMenTreeToolsTransferFile.Text = "Transfer File (SSH)";
            _cMenTreeToolsTransferFile.Click += (sender, args) => OnTransferFileClicked(args);
            // 
            // cMenTreeSep2
            // 
            _cMenTreeSep2.Name = "_cMenTreeSep2";
            _cMenTreeSep2.Size = new System.Drawing.Size(196, 6);
            // 
            // cMenTreeDuplicate
            // 
            _cMenTreeDuplicate.Image = Resources.page_copy;
            _cMenTreeDuplicate.Name = "_cMenTreeDuplicate";
            _cMenTreeDuplicate.ShortcutKeys = Keys.Control | Keys.D;
            _cMenTreeDuplicate.Size = new System.Drawing.Size(199, 22);
            _cMenTreeDuplicate.Text = "Duplicate";
            _cMenTreeDuplicate.Click += (sender, args) => OnDuplicateClicked(args);
            // 
            // cMenTreeRename
            // 
            _cMenTreeRename.Image = Resources.Rename;
            _cMenTreeRename.Name = "_cMenTreeRename";
            _cMenTreeRename.ShortcutKeys = Keys.F2;
            _cMenTreeRename.Size = new System.Drawing.Size(199, 22);
            _cMenTreeRename.Text = "Rename";
            _cMenTreeRename.Click += (sender, args) => OnRenameClicked(args);
            // 
            // cMenTreeDelete
            // 
            _cMenTreeDelete.Image = Resources.Delete;
            _cMenTreeDelete.Name = "_cMenTreeDelete";
            _cMenTreeDelete.ShortcutKeys = Keys.Delete;
            _cMenTreeDelete.Size = new System.Drawing.Size(199, 22);
            _cMenTreeDelete.Text = "Delete";
            _cMenTreeDelete.Click += (sender, args) => OnDeleteClicked(args);
            // 
            // cMenTreeSep3
            // 
            _cMenTreeSep3.Name = "_cMenTreeSep3";
            _cMenTreeSep3.Size = new System.Drawing.Size(196, 6);
            // 
            // cMenTreeImport
            // 
            _cMenTreeImport.DropDownItems.AddRange(new ToolStripItem[] {
                _cMenTreeImportFile,
                _cMenTreeImportActiveDirectory,
                _cMenTreeImportPortScan
            });
            _cMenTreeImport.Name = "_cMenTreeImport";
            _cMenTreeImport.Size = new System.Drawing.Size(199, 22);
            _cMenTreeImport.Text = "&Import";
            // 
            // cMenTreeImportFile
            // 
            _cMenTreeImportFile.Name = "_cMenTreeImportFile";
            _cMenTreeImportFile.Size = new System.Drawing.Size(226, 22);
            _cMenTreeImportFile.Text = "Import from &File...";
            _cMenTreeImportFile.Click += (sender, args) => OnImportFileClicked(args);
            // 
            // cMenTreeImportActiveDirectory
            // 
            _cMenTreeImportActiveDirectory.Name = "_cMenTreeImportActiveDirectory";
            _cMenTreeImportActiveDirectory.Size = new System.Drawing.Size(226, 22);
            _cMenTreeImportActiveDirectory.Text = "Import from &Active Directory...";
            _cMenTreeImportActiveDirectory.Click += (sender, args) => OnImportActiveDirectoryClicked(args);
            // 
            // cMenTreeImportPortScan
            // 
            _cMenTreeImportPortScan.Name = "_cMenTreeImportPortScan";
            _cMenTreeImportPortScan.Size = new System.Drawing.Size(226, 22);
            _cMenTreeImportPortScan.Text = "Import from &Port Scan...";
            _cMenTreeImportPortScan.Click += (sender, args) => OnImportPortScanClicked(args);
            // 
            // cMenTreeExportFile
            // 
            _cMenTreeExportFile.Name = "_cMenTreeExportFile";
            _cMenTreeExportFile.Size = new System.Drawing.Size(199, 22);
            _cMenTreeExportFile.Text = "&Export to File...";
            _cMenTreeExportFile.Click += (sender, args) => OnExportFileClicked(args);
            // 
            // cMenTreeSep4
            // 
            _cMenTreeSep4.Name = "_cMenTreeSep4";
            _cMenTreeSep4.Size = new System.Drawing.Size(196, 6);
            // 
            // cMenTreeAddConnection
            // 
            _cMenTreeAddConnection.Image = Resources.Connection_Add;
            _cMenTreeAddConnection.Name = "_cMenTreeAddConnection";
            _cMenTreeAddConnection.Size = new System.Drawing.Size(199, 22);
            _cMenTreeAddConnection.Text = "New Connection";
            _cMenTreeAddConnection.Click += (sender, args) => OnAddConnectionClicked(args);
            // 
            // cMenTreeAddFolder
            // 
            _cMenTreeAddFolder.Image = Resources.Folder_Add;
            _cMenTreeAddFolder.Name = "_cMenTreeAddFolder";
            _cMenTreeAddFolder.Size = new System.Drawing.Size(199, 22);
            _cMenTreeAddFolder.Text = "New Folder";
            _cMenTreeAddFolder.Click += (sender, args) => OnAddFolderClicked(args);
            // 
            // ToolStripSeparator1
            // 
            _toolStripSeparator1.Name = "_toolStripSeparator1";
            _toolStripSeparator1.Size = new System.Drawing.Size(196, 6);
            // 
            // cMenTreeToolsSort
            // 
            _cMenTreeToolsSort.DropDownItems.AddRange(new ToolStripItem[] {
                _cMenTreeToolsSortAscending,
                _cMenTreeToolsSortDescending
            });
            _cMenTreeToolsSort.Name = "_cMenTreeToolsSort";
            _cMenTreeToolsSort.Size = new System.Drawing.Size(199, 22);
            _cMenTreeToolsSort.Text = "Sort";
            // 
            // cMenTreeToolsSortAscending
            // 
            _cMenTreeToolsSortAscending.Image = Resources.Sort_AZ;
            _cMenTreeToolsSortAscending.Name = "_cMenTreeToolsSortAscending";
            _cMenTreeToolsSortAscending.Size = new System.Drawing.Size(161, 22);
            _cMenTreeToolsSortAscending.Text = "Ascending (A-Z)";
            _cMenTreeToolsSortAscending.Click += (sender, args) => OnSortAscendingClicked(args);
            // 
            // cMenTreeToolsSortDescending
            // 
            _cMenTreeToolsSortDescending.Image = Resources.Sort_ZA;
            _cMenTreeToolsSortDescending.Name = "_cMenTreeToolsSortDescending";
            _cMenTreeToolsSortDescending.Size = new System.Drawing.Size(161, 22);
            _cMenTreeToolsSortDescending.Text = "Descending (Z-A)";
            _cMenTreeToolsSortDescending.Click += (sender, args) => OnSortDescendingClicked(args);
            // 
            // cMenTreeMoveUp
            // 
            _cMenTreeMoveUp.Image = Resources.Arrow_Up;
            _cMenTreeMoveUp.Name = "_cMenTreeMoveUp";
            _cMenTreeMoveUp.ShortcutKeys = Keys.Control | Keys.Up;
            _cMenTreeMoveUp.Size = new System.Drawing.Size(199, 22);
            _cMenTreeMoveUp.Text = "Move up";
            _cMenTreeMoveUp.Click += (sender, args) => OnMoveUpClicked(args);
            // 
            // cMenTreeMoveDown
            // 
            _cMenTreeMoveDown.Image = Resources.Arrow_Down;
            _cMenTreeMoveDown.Name = "_cMenTreeMoveDown";
            _cMenTreeMoveDown.ShortcutKeys = Keys.Control | Keys.Down;
            _cMenTreeMoveDown.Size = new System.Drawing.Size(199, 22);
            _cMenTreeMoveDown.Text = "Move down";
            _cMenTreeMoveDown.Click += (sender, args) => OnMoveDownClicked(args);
        }

        private void ApplyLanguage()
        {
            _cMenTreeConnect.Text = Language.strConnect;
            _cMenTreeConnectWithOptions.Text = Language.strConnectWithOptions;
            _cMenTreeConnectWithOptionsConnectToConsoleSession.Text = Language.strConnectToConsoleSession;
            _cMenTreeConnectWithOptionsDontConnectToConsoleSession.Text = Language.strDontConnectToConsoleSessionMenuItem;
            _cMenTreeConnectWithOptionsConnectInFullscreen.Text = Language.strConnectInFullscreen;
            _cMenTreeConnectWithOptionsNoCredentials.Text = Language.strConnectNoCredentials;
            _cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Text = Language.strChoosePanelBeforeConnecting;
            _cMenTreeDisconnect.Text = Language.strMenuDisconnect;

            _cMenTreeToolsExternalApps.Text = Language.strMenuExternalTools;
            _cMenTreeToolsTransferFile.Text = Language.strMenuTransferFile;

            _cMenTreeDuplicate.Text = Language.strDuplicate;
            _cMenTreeRename.Text = Language.strRename;
            _cMenTreeDelete.Text = Language.strMenuDelete;

            _cMenTreeImport.Text = Language.strImportMenuItem;
            _cMenTreeImportFile.Text = Language.strImportFromFileMenuItem;
            _cMenTreeImportActiveDirectory.Text = Language.strImportAD;
            _cMenTreeImportPortScan.Text = Language.strImportPortScan;
            _cMenTreeExportFile.Text = Language.strExportToFileMenuItem;

            _cMenTreeAddConnection.Text = Language.strAddConnection;
            _cMenTreeAddFolder.Text = Language.strAddFolder;

            _cMenTreeToolsSort.Text = Language.strSort;
            _cMenTreeToolsSortAscending.Text = Language.strSortAsc;
            _cMenTreeToolsSortDescending.Text = Language.strSortDesc;
            _cMenTreeMoveUp.Text = Language.strMoveUp;
            _cMenTreeMoveDown.Text = Language.strMoveDown;
        }

        internal void ShowHideTreeContextMenuItems(ConnectionInfo connectionInfo)
        {
            if (connectionInfo == null)
                return;

            try
            {
                Enabled = true;
                EnableMenuItemsRecursive(Items);
                if (connectionInfo is RootPuttySessionsNodeInfo)
                {
                    _cMenTreeAddConnection.Enabled = false;
                    _cMenTreeAddFolder.Enabled = false;
                    _cMenTreeConnect.Enabled = false;
                    _cMenTreeConnectWithOptions.Enabled = false;
                    _cMenTreeDisconnect.Enabled = false;
                    _cMenTreeToolsTransferFile.Enabled = false;
                    _cMenTreeConnectWithOptions.Enabled = false;
                    _cMenTreeToolsSort.Enabled = false;
                    _cMenTreeToolsExternalApps.Enabled = false;
                    _cMenTreeDuplicate.Enabled = false;
                    _cMenTreeRename.Enabled = true;
                    _cMenTreeDelete.Enabled = false;
                    _cMenTreeMoveUp.Enabled = false;
                    _cMenTreeMoveDown.Enabled = false;
                }
                else if (connectionInfo is RootNodeInfo)
                {
                    _cMenTreeConnect.Enabled = false;
                    _cMenTreeConnectWithOptions.Enabled = false;
                    _cMenTreeConnectWithOptionsConnectInFullscreen.Enabled = false;
                    _cMenTreeConnectWithOptionsConnectToConsoleSession.Enabled = false;
                    _cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Enabled = false;
                    _cMenTreeDisconnect.Enabled = false;
                    _cMenTreeToolsTransferFile.Enabled = false;
                    _cMenTreeToolsExternalApps.Enabled = false;
                    _cMenTreeDuplicate.Enabled = false;
                    _cMenTreeDelete.Enabled = false;
                    _cMenTreeMoveUp.Enabled = false;
                    _cMenTreeMoveDown.Enabled = false;
                }
                else if (connectionInfo is ContainerInfo)
                {
                    _cMenTreeConnectWithOptionsConnectInFullscreen.Enabled = false;
                    _cMenTreeConnectWithOptionsConnectToConsoleSession.Enabled = false;
                    _cMenTreeDisconnect.Enabled = false;

                    var openConnections = ((ContainerInfo)connectionInfo).Children.Sum(child => child.OpenConnections.Count);
                    if (openConnections == 0)
                        _cMenTreeDisconnect.Enabled = false;

                    _cMenTreeToolsTransferFile.Enabled = false;
                    _cMenTreeToolsExternalApps.Enabled = false;
                }
                else if (connectionInfo is PuttySessionInfo)
                {
                    _cMenTreeAddConnection.Enabled = false;
                    _cMenTreeAddFolder.Enabled = false;

                    if (connectionInfo.OpenConnections.Count == 0)
                        _cMenTreeDisconnect.Enabled = false;

                    if (!(connectionInfo.Protocol == ProtocolType.SSH1 | connectionInfo.Protocol == ProtocolType.SSH2))
                        _cMenTreeToolsTransferFile.Enabled = false;

                    _cMenTreeConnectWithOptionsConnectInFullscreen.Enabled = false;
                    _cMenTreeConnectWithOptionsConnectToConsoleSession.Enabled = false;
                    _cMenTreeToolsSort.Enabled = false;
                    _cMenTreeDuplicate.Enabled = false;
                    _cMenTreeRename.Enabled = false;
                    _cMenTreeDelete.Enabled = false;
                    _cMenTreeMoveUp.Enabled = false;
                    _cMenTreeMoveDown.Enabled = false;
                }
                else
                {
                    if (connectionInfo.OpenConnections.Count == 0)
                        _cMenTreeDisconnect.Enabled = false;

                    if (!(connectionInfo.Protocol == ProtocolType.SSH1 | connectionInfo.Protocol == ProtocolType.SSH2))
                        _cMenTreeToolsTransferFile.Enabled = false;

                    if (!(connectionInfo.Protocol == ProtocolType.RDP | connectionInfo.Protocol == ProtocolType.ICA))
                    {
                        _cMenTreeConnectWithOptionsConnectInFullscreen.Enabled = false;
                        _cMenTreeConnectWithOptionsConnectToConsoleSession.Enabled = false;
                    }

                    if (connectionInfo.Protocol == ProtocolType.IntApp)
                        _cMenTreeConnectWithOptionsNoCredentials.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("ShowHideTreeContextMenuItems (UI.Window.ConnectionTreeWindow) failed", ex);
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

        private void AddExternalApps()
        {
            try
            {
                ResetExternalAppMenu();

                foreach (ExternalTool extA in Runtime.ExternalTools)
                {
                    var menuItem = new ToolStripMenuItem
                    {
                        Text = extA.DisplayName,
                        Tag = extA,
                        Image = extA.Image
                    };

                    menuItem.Click += OnExternalToolClicked;
                    _cMenTreeToolsExternalApps.DropDownItems.Add(menuItem);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("cMenTreeTools_DropDownOpening failed (UI.Window.ConnectionTreeWindow)", ex);
            }
        }

        private void ResetExternalAppMenu()
        {
            if (_cMenTreeToolsExternalApps.DropDownItems.Count <= 0) return;
            for (var i = _cMenTreeToolsExternalApps.DropDownItems.Count - 1; i >= 0; i--)
                _cMenTreeToolsExternalApps.DropDownItems[i].Dispose();

            _cMenTreeToolsExternalApps.DropDownItems.Clear();
        }

        #region Events
        public event EventHandler ConnectClicked;
        protected virtual void OnConnectClicked(EventArgs e)
        {
            var handler = ConnectClicked;
            handler?.Invoke(this, e);
        }

        public event EventHandler ConnectToConsoleSessionClicked;
        protected virtual void OnConnectToConsoleSessionClicked(EventArgs e)
        {
            var handler = ConnectToConsoleSessionClicked;
            handler?.Invoke(this, e);
        }
        
        public event EventHandler DontConnectToConsoleSessionClicked;
        protected virtual void OnDontConnectToConsoleSessionClicked(EventArgs e)
        {
            var handler = DontConnectToConsoleSessionClicked;
            handler?.Invoke(this, e);
        }

        public event EventHandler ConnectInFullscreenClicked;
        protected virtual void OnConnectInFullscreenClicked(EventArgs e)
        {
            var handler = ConnectInFullscreenClicked;
            handler?.Invoke(this, e);
        }

        public event EventHandler ConnectWithNoCredentialsClick;
        protected virtual void OnConnectWithNoCredentialsClick(EventArgs e)
        {
            var handler = ConnectWithNoCredentialsClick;
            handler?.Invoke(this, e);
        }

        public event EventHandler ChoosePanelBeforeConnectingClicked;
        protected virtual void OnChoosePanelBeforeConnectingClicked(EventArgs e)
        {
            var handler = ChoosePanelBeforeConnectingClicked;
            handler?.Invoke(this, e);
        }


        public event EventHandler DisconnectClicked;
        protected virtual void OnDisconnectClicked(EventArgs e)
        {
            var handler = DisconnectClicked;
            handler?.Invoke(this, e);
        }

        public event EventHandler TransferFileClicked;
        protected virtual void OnTransferFileClicked(EventArgs e)
        {
            var handler = TransferFileClicked;
            handler?.Invoke(this, e);
        }

        public event EventHandler DuplicateClicked;
        protected virtual void OnDuplicateClicked(EventArgs e)
        {
            var handler = DuplicateClicked;
            handler?.Invoke(this, e);
        }

        public event EventHandler RenameClicked;
        protected virtual void OnRenameClicked(EventArgs e)
        {
            var handler = RenameClicked;
            handler?.Invoke(this, e);
        }

        public event EventHandler DeleteClicked;
        protected virtual void OnDeleteClicked(EventArgs e)
        {
            var handler = DeleteClicked;
            handler?.Invoke(this, e);
        }

        public event EventHandler ImportFileClicked;
        protected virtual void OnImportFileClicked(EventArgs e)
        {
            var handler = ImportFileClicked;
            handler?.Invoke(this, e);
        }

        public event EventHandler ImportActiveDirectoryClicked;
        protected virtual void OnImportActiveDirectoryClicked(EventArgs e)
        {
            var handler = ImportActiveDirectoryClicked;
            handler?.Invoke(this, e);
        }

        public event EventHandler ImportPortScanClicked;
        protected virtual void OnImportPortScanClicked(EventArgs e)
        {
            var handler = ImportPortScanClicked;
            handler?.Invoke(this, e);
        }

        public event EventHandler ExportFileClicked;
        protected virtual void OnExportFileClicked(EventArgs e)
        {
            var handler = ExportFileClicked;
            handler?.Invoke(this, e);
        }

        public event EventHandler AddConnectionClicked;
        protected virtual void OnAddConnectionClicked(EventArgs e)
        {
            var handler = AddConnectionClicked;
            handler?.Invoke(this, e);
        }

        public event EventHandler AddFolderClicked;
        protected virtual void OnAddFolderClicked(EventArgs e)
        {
            var handler = AddFolderClicked;
            handler?.Invoke(this, e);
        }

        public event EventHandler SortAscendingClicked;
        protected virtual void OnSortAscendingClicked(EventArgs e)
        {
            var handler = SortAscendingClicked;
            handler?.Invoke(this, e);
        }

        public event EventHandler SortDescendingClicked;
        protected virtual void OnSortDescendingClicked(EventArgs e)
        {
            var handler = SortDescendingClicked;
            handler?.Invoke(this, e);
        }

        public event EventHandler MoveUpClicked;
        protected virtual void OnMoveUpClicked(EventArgs e)
        {
            var handler = MoveUpClicked;
            handler?.Invoke(this, e);
        }

        public event EventHandler MoveDownClicked;
        protected virtual void OnMoveDownClicked(EventArgs e)
        {
            var handler = MoveDownClicked;
            handler?.Invoke(this, e);
        }

        public event EventHandler ExternalToolClicked;
        protected virtual void OnExternalToolClicked(object sender, EventArgs e)
        {
            var handler = ExternalToolClicked;
            handler?.Invoke(sender, e);
        }
        #endregion
    }
}