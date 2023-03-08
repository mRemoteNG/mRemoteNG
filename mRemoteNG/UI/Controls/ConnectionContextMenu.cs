using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Tools;
using mRemoteNG.Tools.Clipboard;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

// ReSharper disable UnusedParameter.Local


namespace mRemoteNG.UI.Controls
{
    [SupportedOSPlatform("windows")]
    public sealed class ConnectionContextMenu : ContextMenuStrip
    {
        private ToolStripMenuItem _cMenTreeAddConnection;
        private ToolStripMenuItem _cMenTreeAddFolder;
        private ToolStripSeparator _cMenTreeSep1;
        private ToolStripMenuItem _cMenTreeConnect;
        private ToolStripMenuItem _cMenTreeConnectWithOptions;
        private ToolStripMenuItem _cMenTreeConnectWithOptionsConnectToConsoleSession;
        private ToolStripMenuItem _cMenTreeConnectWithOptionsNoCredentials;
        private ToolStripMenuItem _cMenTreeConnectWithOptionsConnectInFullscreen;
        private ToolStripMenuItem _cMenTreeConnectWithOptionsViewOnly;
        private ToolStripMenuItem _cMenTreeDisconnect;
        private ToolStripSeparator _cMenTreeSep2;
        private ToolStripMenuItem _cMenTreeToolsTransferFile;
        private ToolStripMenuItem _cMenTreeToolsSort;
        private ToolStripMenuItem _cMenTreeToolsSortAscending;
        private ToolStripMenuItem _cMenTreeToolsSortDescending;
        private ToolStripSeparator _cMenTreeSep3;
        private ToolStripMenuItem _cMenTreeRename;
        private ToolStripMenuItem _cMenTreeDelete;
        private ToolStripMenuItem _cMenTreeCopyHostname;
        private ToolStripSeparator _cMenTreeSep4;
        private ToolStripMenuItem _cMenTreeMoveUp;
        private ToolStripMenuItem _cMenTreeMoveDown;
        private ToolStripMenuItem _cMenTreeToolsExternalApps;
        private ToolStripMenuItem _cMenTreeDuplicate;
        private ToolStripMenuItem _cMenInheritanceSubMenu;
        private ToolStripMenuItem _cMenTreeConnectWithOptionsChoosePanelBeforeConnecting;
        private ToolStripMenuItem _cMenTreeConnectWithOptionsDontConnectToConsoleSession;
        private ToolStripMenuItem _cMenTreeImport;
        private ToolStripMenuItem _cMenTreeExportFile;
        private ToolStripSeparator _toolStripSeparator1;
        private ToolStripMenuItem _cMenTreeImportFile;
        private ToolStripMenuItem _cMenTreeImportFromRemoteDesktopManager;
        private ToolStripMenuItem _cMenTreeImportActiveDirectory;
        private ToolStripMenuItem _cMenTreeImportPortScan;
        private ToolStripMenuItem _cMenTreeApplyInheritanceToChildren;
        private ToolStripMenuItem _cMenTreeApplyDefaultInheritance;
        private readonly ConnectionTree.ConnectionTree _connectionTree;


        public ConnectionContextMenu(ConnectionTree.ConnectionTree connectionTree)
        {
            _connectionTree = connectionTree;
            InitializeComponent();
            ApplyLanguage();
            EnableShortcutKeys();
            Opening += (sender, args) =>
            {
                AddExternalApps();
                if (_connectionTree.SelectedNode == null)
                {
                    args.Cancel = true;
                    return;
                }

                ShowHideMenuItems();
            };
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
            _cMenTreeConnectWithOptionsViewOnly = new ToolStripMenuItem();
            _cMenTreeDisconnect = new ToolStripMenuItem();
            _cMenTreeSep1 = new ToolStripSeparator();
            _cMenTreeToolsExternalApps = new ToolStripMenuItem();
            _cMenTreeToolsTransferFile = new ToolStripMenuItem();
            _cMenTreeSep2 = new ToolStripSeparator();
            _cMenTreeDuplicate = new ToolStripMenuItem();
            _cMenTreeRename = new ToolStripMenuItem();
            _cMenTreeDelete = new ToolStripMenuItem();
            _cMenTreeCopyHostname = new ToolStripMenuItem();
            _cMenTreeSep3 = new ToolStripSeparator();
            _cMenTreeImport = new ToolStripMenuItem();
            _cMenTreeImportFile = new ToolStripMenuItem();
            _cMenTreeImportFromRemoteDesktopManager = new ToolStripMenuItem();
            _cMenTreeImportActiveDirectory = new ToolStripMenuItem();
            _cMenTreeImportPortScan = new ToolStripMenuItem();
            _cMenInheritanceSubMenu = new ToolStripMenuItem();
            _cMenTreeApplyInheritanceToChildren = new ToolStripMenuItem();
            _cMenTreeApplyDefaultInheritance = new ToolStripMenuItem();
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
            Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
                                           System.Drawing.GraphicsUnit.Point, 0);
            Items.AddRange(new ToolStripItem[]
            {
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
                _cMenTreeCopyHostname,
                _cMenInheritanceSubMenu,
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
            _cMenTreeConnect.Image = Properties.Resources.Run_16x;
            _cMenTreeConnect.Name = "_cMenTreeConnect";
            _cMenTreeConnect.Size = new System.Drawing.Size(199, 22);
            _cMenTreeConnect.Text = "Connect";
            _cMenTreeConnect.Click += OnConnectClicked;
            //
            // cMenTreeConnectWithOptions
            //
            _cMenTreeConnectWithOptions.DropDownItems.AddRange(new ToolStripItem[]
            {
                _cMenTreeConnectWithOptionsConnectToConsoleSession,
                _cMenTreeConnectWithOptionsDontConnectToConsoleSession,
                _cMenTreeConnectWithOptionsConnectInFullscreen,
                _cMenTreeConnectWithOptionsNoCredentials,
                _cMenTreeConnectWithOptionsChoosePanelBeforeConnecting,
                _cMenTreeConnectWithOptionsViewOnly
            });
            _cMenTreeConnectWithOptions.Name = "_cMenTreeConnectWithOptions";
            _cMenTreeConnectWithOptions.Size = new System.Drawing.Size(199, 22);
            _cMenTreeConnectWithOptions.Text = "Connect (with options)";
            //
            // cMenTreeConnectWithOptionsConnectToConsoleSession
            //
            _cMenTreeConnectWithOptionsConnectToConsoleSession.Name =
                "_cMenTreeConnectWithOptionsConnectToConsoleSession";
            _cMenTreeConnectWithOptionsConnectToConsoleSession.Size = new System.Drawing.Size(245, 22);
            _cMenTreeConnectWithOptionsConnectToConsoleSession.Text = "Connect to console session";
            _cMenTreeConnectWithOptionsConnectToConsoleSession.Click += OnConnectToConsoleSessionClicked;
            //
            // cMenTreeConnectWithOptionsDontConnectToConsoleSession
            //
            _cMenTreeConnectWithOptionsDontConnectToConsoleSession.Name =
                "_cMenTreeConnectWithOptionsDontConnectToConsoleSession";
            _cMenTreeConnectWithOptionsDontConnectToConsoleSession.Size = new System.Drawing.Size(245, 22);
            _cMenTreeConnectWithOptionsDontConnectToConsoleSession.Text = "Don\'t connect to console session";
            _cMenTreeConnectWithOptionsDontConnectToConsoleSession.Visible = false;
            _cMenTreeConnectWithOptionsDontConnectToConsoleSession.Click += OnDontConnectToConsoleSessionClicked;
            //
            // cMenTreeConnectWithOptionsConnectInFullscreen
            //
            _cMenTreeConnectWithOptionsConnectInFullscreen.Image = Properties.Resources.FullScreen_16x;
            _cMenTreeConnectWithOptionsConnectInFullscreen.Name = "_cMenTreeConnectWithOptionsConnectInFullscreen";
            _cMenTreeConnectWithOptionsConnectInFullscreen.Size = new System.Drawing.Size(245, 22);
            _cMenTreeConnectWithOptionsConnectInFullscreen.Text = "Connect in fullscreen";
            _cMenTreeConnectWithOptionsConnectInFullscreen.Click += OnConnectInFullscreenClicked;
            //
            // cMenTreeConnectWithOptionsNoCredentials
            //
            _cMenTreeConnectWithOptionsNoCredentials.Image = Properties.Resources.UniqueKeyError_16x;
            _cMenTreeConnectWithOptionsNoCredentials.Name = "_cMenTreeConnectWithOptionsNoCredentials";
            _cMenTreeConnectWithOptionsNoCredentials.Size = new System.Drawing.Size(245, 22);
            _cMenTreeConnectWithOptionsNoCredentials.Text = "Connect without credentials";
            _cMenTreeConnectWithOptionsNoCredentials.Click += OnConnectWithNoCredentialsClick;
            //
            // cMenTreeConnectWithOptionsChoosePanelBeforeConnecting
            //
            _cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Image = Properties.Resources.Panel_16x;
            _cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Name =
                "_cMenTreeConnectWithOptionsChoosePanelBeforeConnecting";
            _cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Size = new System.Drawing.Size(245, 22);
            _cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Text = "Choose panel before connecting";
            _cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Click += OnChoosePanelBeforeConnectingClicked;
            //
            // cMenTreeConnectWithOptionsViewOnly
            //
            _cMenTreeConnectWithOptionsViewOnly.Image = Properties.Resources.Monitor_16x;
            _cMenTreeConnectWithOptionsViewOnly.Name =
                "_cMenTreeConnectWithOptionsViewOnly";
            _cMenTreeConnectWithOptionsViewOnly.Size = new System.Drawing.Size(245, 22);
            _cMenTreeConnectWithOptionsViewOnly.Text = Language.ConnectInViewOnlyMode;
            _cMenTreeConnectWithOptionsViewOnly.Click += ConnectWithOptionsViewOnlyOnClick;
            //
            // cMenTreeDisconnect
            //
            _cMenTreeDisconnect.Image = Properties.Resources.Stop_16x;
            _cMenTreeDisconnect.Name = "_cMenTreeDisconnect";
            _cMenTreeDisconnect.Size = new System.Drawing.Size(199, 22);
            _cMenTreeDisconnect.Text = "Disconnect";
            _cMenTreeDisconnect.Click += OnDisconnectClicked;
            //
            // cMenTreeSep1
            //
            _cMenTreeSep1.Name = "_cMenTreeSep1";
            _cMenTreeSep1.Size = new System.Drawing.Size(196, 6);
            //
            // cMenTreeToolsExternalApps
            //
            _cMenTreeToolsExternalApps.Image = Properties.Resources.Console_16x;
            _cMenTreeToolsExternalApps.Name = "_cMenTreeToolsExternalApps";
            _cMenTreeToolsExternalApps.Size = new System.Drawing.Size(199, 22);
            _cMenTreeToolsExternalApps.Text = "External Applications";
            //
            // cMenTreeToolsTransferFile
            //
            _cMenTreeToolsTransferFile.Image = Properties.Resources.SyncArrow_16x;
            _cMenTreeToolsTransferFile.Name = "_cMenTreeToolsTransferFile";
            _cMenTreeToolsTransferFile.Size = new System.Drawing.Size(199, 22);
            _cMenTreeToolsTransferFile.Text = "Transfer File (SSH)";
            _cMenTreeToolsTransferFile.Click += OnTransferFileClicked;
            //
            // cMenTreeSep2
            //
            _cMenTreeSep2.Name = "_cMenTreeSep2";
            _cMenTreeSep2.Size = new System.Drawing.Size(196, 6);
            //
            // cMenTreeDuplicate
            //
            _cMenTreeDuplicate.Image = Properties.Resources.Copy_16x;
            _cMenTreeDuplicate.Name = "_cMenTreeDuplicate";
            _cMenTreeDuplicate.Size = new System.Drawing.Size(199, 22);
            _cMenTreeDuplicate.Text = "Duplicate";
            _cMenTreeDuplicate.Click += OnDuplicateClicked;
            //
            // cMenTreeRename
            //
            _cMenTreeRename.Image = Properties.Resources.Rename_16x;
            _cMenTreeRename.Name = "_cMenTreeRename";
            _cMenTreeRename.Size = new System.Drawing.Size(199, 22);
            _cMenTreeRename.Text = "Rename";
            _cMenTreeRename.Click += OnRenameClicked;
            //
            // cMenTreeDelete
            //
            _cMenTreeDelete.Image = Properties.Resources.Close_16x;
            _cMenTreeDelete.Name = "_cMenTreeDelete";
            _cMenTreeDelete.Size = new System.Drawing.Size(199, 22);
            _cMenTreeDelete.Text = "Delete";
            _cMenTreeDelete.Click += OnDeleteClicked;
            //
            // cMenTreeCopyHostname
            //
            _cMenTreeCopyHostname.Name = "_cMenTreeCopyHostname";
            _cMenTreeCopyHostname.Size = new System.Drawing.Size(199, 22);
            _cMenTreeCopyHostname.Text = "Copy Hostname";
            _cMenTreeCopyHostname.Click += OnCopyHostnameClicked;
            //
            // cMenTreeSep3
            //
            _cMenTreeSep3.Name = "_cMenTreeSep3";
            _cMenTreeSep3.Size = new System.Drawing.Size(196, 6);
            //
            // cMenTreeImport
            //
            _cMenTreeImport.DropDownItems.AddRange(new ToolStripItem[]
            {
                _cMenTreeImportFile,
                _cMenTreeImportFromRemoteDesktopManager,
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
            _cMenTreeImportFile.Click += OnImportFileClicked;

            // cMenTreeImportFromRemoteDesktopManager
            _cMenTreeImportFromRemoteDesktopManager.Name = "_cMenTreeImportFromRemoteDesktopManager";
            _cMenTreeImportFromRemoteDesktopManager.Size = new System.Drawing.Size(226, 22);
            _cMenTreeImportFromRemoteDesktopManager.Text = "Import from &Remote Desktop Manager";
            _cMenTreeImportFromRemoteDesktopManager.Click += OnImportRemoteDesktopManagerClicked;
            //
            // cMenTreeImportActiveDirectory
            //
            _cMenTreeImportActiveDirectory.Name = "_cMenTreeImportActiveDirectory";
            _cMenTreeImportActiveDirectory.Size = new System.Drawing.Size(226, 22);
            _cMenTreeImportActiveDirectory.Text = "Import from &Active Directory...";
            _cMenTreeImportActiveDirectory.Click += OnImportActiveDirectoryClicked;
            //
            // cMenTreeImportPortScan
            //
            _cMenTreeImportPortScan.Name = "_cMenTreeImportPortScan";
            _cMenTreeImportPortScan.Size = new System.Drawing.Size(226, 22);
            _cMenTreeImportPortScan.Text = "Import from &Port Scan...";
            _cMenTreeImportPortScan.Click += OnImportPortScanClicked;
            //
            // cMenTreeExportFile
            //
            _cMenTreeExportFile.Name = "_cMenTreeExportFile";
            _cMenTreeExportFile.Size = new System.Drawing.Size(199, 22);
            _cMenTreeExportFile.Text = "&Export to File...";
            _cMenTreeExportFile.Click += OnExportFileClicked;
            //
            // cMenTreeSep4
            //
            _cMenTreeSep4.Name = "_cMenTreeSep4";
            _cMenTreeSep4.Size = new System.Drawing.Size(196, 6);
            //
            // cMenTreeAddConnection
            //
            _cMenTreeAddConnection.Image = Properties.Resources.AddItem_16x;
            _cMenTreeAddConnection.Name = "_cMenTreeAddConnection";
            _cMenTreeAddConnection.Size = new System.Drawing.Size(199, 22);
            _cMenTreeAddConnection.Text = "New Connection";
            _cMenTreeAddConnection.Click += OnAddConnectionClicked;
            //
            // cMenTreeAddFolder
            //
            _cMenTreeAddFolder.Image = Properties.Resources.AddFolder_16x;
            _cMenTreeAddFolder.Name = "_cMenTreeAddFolder";
            _cMenTreeAddFolder.Size = new System.Drawing.Size(199, 22);
            _cMenTreeAddFolder.Text = "New Folder";
            _cMenTreeAddFolder.Click += OnAddFolderClicked;
            //
            // ToolStripSeparator1
            //
            _toolStripSeparator1.Name = "_toolStripSeparator1";
            _toolStripSeparator1.Size = new System.Drawing.Size(196, 6);
            //
            // cMenTreeToolsSort
            //
            _cMenTreeToolsSort.DropDownItems.AddRange(new ToolStripItem[]
            {
                _cMenTreeToolsSortAscending,
                _cMenTreeToolsSortDescending
            });
            _cMenTreeToolsSort.Name = "_cMenTreeToolsSort";
            _cMenTreeToolsSort.Size = new System.Drawing.Size(199, 22);
            _cMenTreeToolsSort.Text = "Sort";
            //
            // cMenTreeToolsSortAscending
            //
            _cMenTreeToolsSortAscending.Image = Properties.Resources.SortAscending_16x;
            _cMenTreeToolsSortAscending.Name = "_cMenTreeToolsSortAscending";
            _cMenTreeToolsSortAscending.Size = new System.Drawing.Size(161, 22);
            _cMenTreeToolsSortAscending.Text = "Ascending (A-Z)";
            _cMenTreeToolsSortAscending.Click += OnSortAscendingClicked;
            //
            // cMenTreeToolsSortDescending
            //
            _cMenTreeToolsSortDescending.Image = Properties.Resources.SortDescending_16x;
            _cMenTreeToolsSortDescending.Name = "_cMenTreeToolsSortDescending";
            _cMenTreeToolsSortDescending.Size = new System.Drawing.Size(161, 22);
            _cMenTreeToolsSortDescending.Text = "Descending (Z-A)";
            _cMenTreeToolsSortDescending.Click += OnSortDescendingClicked;
            //
            // cMenTreeMoveUp
            //
            _cMenTreeMoveUp.Image = Properties.Resources.GlyphUp_16x;
            _cMenTreeMoveUp.Name = "_cMenTreeMoveUp";
            _cMenTreeMoveUp.Size = new System.Drawing.Size(199, 22);
            _cMenTreeMoveUp.Text = "Move up";
            _cMenTreeMoveUp.Click += OnMoveUpClicked;
            //
            // cMenTreeMoveDown
            //
            _cMenTreeMoveDown.Image = Properties.Resources.GlyphDown_16x;
            _cMenTreeMoveDown.Name = "_cMenTreeMoveDown";
            _cMenTreeMoveDown.Size = new System.Drawing.Size(199, 22);
            _cMenTreeMoveDown.Text = "Move down";
            _cMenTreeMoveDown.Click += OnMoveDownClicked;
            //
            // cMenEditSubMenu
            //
            _cMenInheritanceSubMenu.DropDownItems.AddRange(new ToolStripItem[]
            {
                _cMenTreeApplyInheritanceToChildren,
                _cMenTreeApplyDefaultInheritance
            });
            _cMenInheritanceSubMenu.Name = "_cMenInheritanceSubMenu";
            _cMenInheritanceSubMenu.Size = new System.Drawing.Size(199, 22);
            _cMenInheritanceSubMenu.Text = "Inheritance";
            //
            // _cMenTreeApplyInheritanceToChildren
            //
            _cMenTreeApplyInheritanceToChildren.Name = "_cMenTreeApplyInheritanceToChildren";
            _cMenTreeApplyInheritanceToChildren.Size = new System.Drawing.Size(199, 22);
            _cMenTreeApplyInheritanceToChildren.Text = "Apply inheritance to children";
            _cMenTreeApplyInheritanceToChildren.Click += OnApplyInheritanceToChildrenClicked;
            //
            // _cMenTreeApplyDefaultInheritance
            //
            _cMenTreeApplyDefaultInheritance.Name = "_cMenTreeApplyDefaultInheritance";
            _cMenTreeApplyDefaultInheritance.Size = new System.Drawing.Size(199, 22);
            _cMenTreeApplyDefaultInheritance.Text = "Apply default inheritance";
            _cMenTreeApplyDefaultInheritance.Click += OnApplyDefaultInheritanceClicked;
        }


        private void ApplyLanguage()
        {
            _cMenTreeConnect.Text = Language.Connect;
            _cMenTreeConnectWithOptions.Text = Language.ConnectWithOptions;
            _cMenTreeConnectWithOptionsConnectToConsoleSession.Text = Language.ConnectToConsoleSession;
            _cMenTreeConnectWithOptionsDontConnectToConsoleSession.Text = Language.DontConnectToConsoleSession;
            _cMenTreeConnectWithOptionsConnectInFullscreen.Text = Language.ConnectInFullscreen;
            _cMenTreeConnectWithOptionsNoCredentials.Text = Language.ConnectNoCredentials;
            _cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Text = Language.ChoosePanelBeforeConnecting;
            _cMenTreeConnectWithOptionsViewOnly.Text = Language.ConnectInViewOnlyMode;
            _cMenTreeDisconnect.Text = Language.Disconnect;

            _cMenTreeToolsExternalApps.Text = Language._Tools;
            _cMenTreeToolsTransferFile.Text = Language.TransferFile;

            _cMenTreeDuplicate.Text = Language.Duplicate;
            _cMenTreeRename.Text = Language.Rename;
            _cMenTreeDelete.Text = Language.Delete;
            _cMenTreeCopyHostname.Text = Language.CopyHostname;

            _cMenTreeImport.Text = Language._Import;
            _cMenTreeImportFile.Text = Language.ImportFromFile;
            _cMenTreeImportActiveDirectory.Text = Language.ImportAD;
            _cMenTreeImportPortScan.Text = Language.ImportPortScan;
            _cMenTreeExportFile.Text = Language._ExportToFile;

            _cMenTreeAddConnection.Text = Language.NewConnection;
            _cMenTreeAddFolder.Text = Language.NewFolder;

            _cMenTreeToolsSort.Text = Language.Sort;
            _cMenTreeToolsSortAscending.Text = Language.SortAsc;
            _cMenTreeToolsSortDescending.Text = Language.SortDesc;
            _cMenTreeMoveUp.Text = Language.MoveUp;
            _cMenTreeMoveDown.Text = Language.MoveDown;

            _cMenInheritanceSubMenu.Text = Language.Inheritance;
            _cMenTreeApplyInheritanceToChildren.Text = Language.ApplyInheritanceToChildren;
            _cMenTreeApplyDefaultInheritance.Text = Language.ApplyDefaultInheritance;
        }

        internal void ShowHideMenuItems()
        {
            try
            {
                Enabled = true;
                EnableMenuItemsRecursive(Items);
                if (_connectionTree.SelectedNode is RootPuttySessionsNodeInfo)
                {
                    ShowHideMenuItemsForRootPuttyNode();
                }
                else if (_connectionTree.SelectedNode is RootNodeInfo)
                {
                    ShowHideMenuItemsForRootConnectionNode();
                }
                else if (_connectionTree.SelectedNode is ContainerInfo containerInfo)
                {
                    ShowHideMenuItemsForContainer(containerInfo);
                }
                else if (_connectionTree.SelectedNode is PuttySessionInfo puttyNode)
                {
                    ShowHideMenuItemsForPuttyNode(puttyNode);
                }
                else
                {
                    ShowHideMenuItemsForConnectionNode(_connectionTree.SelectedNode);
                }

                _cMenInheritanceSubMenu.Enabled = _cMenInheritanceSubMenu.DropDownItems
                    .OfType<ToolStripMenuItem>().Any(i => i.Enabled);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(
                                                                "ShowHideMenuItems (UI.Controls.ConnectionContextMenu) failed",
                                                                ex);
            }
        }

        internal void ShowHideMenuItemsForRootPuttyNode()
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
            _cMenTreeImport.Enabled = false;
            _cMenTreeExportFile.Enabled = false;
            _cMenTreeRename.Enabled = false;
            _cMenTreeDelete.Enabled = false;
            _cMenTreeMoveUp.Enabled = false;
            _cMenTreeMoveDown.Enabled = false;
            _cMenTreeConnectWithOptionsViewOnly.Enabled = false;
            _cMenTreeApplyInheritanceToChildren.Enabled = false;
            _cMenTreeApplyDefaultInheritance.Enabled = false;
            _cMenTreeCopyHostname.Enabled = false;
        }

        internal void ShowHideMenuItemsForRootConnectionNode()
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
            _cMenTreeConnectWithOptionsViewOnly.Enabled = false;
            _cMenTreeApplyInheritanceToChildren.Enabled = false;
            _cMenTreeApplyDefaultInheritance.Enabled = false;
        }

        internal void ShowHideMenuItemsForContainer(ContainerInfo containerInfo)
        {
            _cMenTreeConnectWithOptionsConnectInFullscreen.Enabled = false;
            _cMenTreeConnectWithOptionsConnectToConsoleSession.Enabled = false;

            var hasOpenConnections = containerInfo.Children.Any(child => child.OpenConnections.Count > 0);
            _cMenTreeDisconnect.Enabled = hasOpenConnections;

            _cMenTreeToolsTransferFile.Enabled = false;
            _cMenTreeConnectWithOptionsViewOnly.Enabled = false;
        }

        internal void ShowHideMenuItemsForPuttyNode(PuttySessionInfo connectionInfo)
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
            _cMenTreeImport.Enabled = false;
            _cMenTreeExportFile.Enabled = false;
            _cMenTreeConnectWithOptionsViewOnly.Enabled = false;
            _cMenTreeApplyInheritanceToChildren.Enabled = false;
            _cMenTreeApplyDefaultInheritance.Enabled = false;
        }

        internal void ShowHideMenuItemsForConnectionNode(ConnectionInfo connectionInfo)
        {
            if (connectionInfo.OpenConnections.Count == 0)
                _cMenTreeDisconnect.Enabled = false;

            if (!(connectionInfo.Protocol == ProtocolType.SSH1 | connectionInfo.Protocol == ProtocolType.SSH2))
                _cMenTreeToolsTransferFile.Enabled = false;

            if (!(connectionInfo.Protocol == ProtocolType.RDP))
            {
                _cMenTreeConnectWithOptionsConnectInFullscreen.Enabled = false;
                _cMenTreeConnectWithOptionsConnectToConsoleSession.Enabled = false;
            }

            if (connectionInfo.Protocol == ProtocolType.IntApp)
                _cMenTreeConnectWithOptionsNoCredentials.Enabled = false;

            if (connectionInfo.Protocol != ProtocolType.RDP && connectionInfo.Protocol != ProtocolType.VNC)
                _cMenTreeConnectWithOptionsViewOnly.Enabled = false;

            _cMenTreeApplyInheritanceToChildren.Enabled = false;
        }

        internal void DisableShortcutKeys()
        {
            _cMenTreeConnect.ShortcutKeys = Keys.None;
            _cMenTreeDuplicate.ShortcutKeys = Keys.None;
            _cMenTreeRename.ShortcutKeys = Keys.None;
            _cMenTreeDelete.ShortcutKeys = Keys.None;
            _cMenTreeMoveUp.ShortcutKeys = Keys.None;
            _cMenTreeMoveDown.ShortcutKeys = Keys.None;
        }

        internal void EnableShortcutKeys()
        {
            _cMenTreeConnect.ShortcutKeys = ((Keys.Control | Keys.Shift) | Keys.C);
            _cMenTreeDuplicate.ShortcutKeys = Keys.Control | Keys.D;
            _cMenTreeRename.ShortcutKeys = Keys.F2;
            _cMenTreeDelete.ShortcutKeys = Keys.Delete;
            _cMenTreeMoveUp.ShortcutKeys = Keys.Control | Keys.Up;
            _cMenTreeMoveDown.ShortcutKeys = Keys.Control | Keys.Down;
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

                foreach (ExternalTool extA in Runtime.ExternalToolsService.ExternalTools)
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
                Runtime.MessageCollector.AddExceptionStackTrace(
                                                                "cMenTreeTools_DropDownOpening failed (UI.Window.ConnectionTreeWindow)",
                                                                ex);
            }
        }

        private void ResetExternalAppMenu()
        {
            if (_cMenTreeToolsExternalApps.DropDownItems.Count <= 0) return;
            for (var i = _cMenTreeToolsExternalApps.DropDownItems.Count - 1; i >= 0; i--)
                _cMenTreeToolsExternalApps.DropDownItems[i].Dispose();

            _cMenTreeToolsExternalApps.DropDownItems.Clear();
        }

        #region Click handlers

        private void OnConnectClicked(object sender, EventArgs e)
        {
            var selectedNodeAsContainer = _connectionTree.SelectedNode as ContainerInfo;
            if (selectedNodeAsContainer != null)
                Runtime.ConnectionInitiator.OpenConnection(selectedNodeAsContainer, ConnectionInfo.Force.DoNotJump);
            else
                Runtime.ConnectionInitiator.OpenConnection(_connectionTree.SelectedNode, ConnectionInfo.Force.DoNotJump);
        }

        private void OnConnectToConsoleSessionClicked(object sender, EventArgs e)
        {
            var selectedNodeAsContainer = _connectionTree.SelectedNode as ContainerInfo;
            if (selectedNodeAsContainer != null)
                Runtime.ConnectionInitiator.OpenConnection(selectedNodeAsContainer,
                                                           ConnectionInfo.Force.UseConsoleSession |
                                                           ConnectionInfo.Force.DoNotJump);
            else
                Runtime.ConnectionInitiator.OpenConnection(_connectionTree.SelectedNode,
                                                           ConnectionInfo.Force.UseConsoleSession |
                                                           ConnectionInfo.Force.DoNotJump);

        }

        private void OnDontConnectToConsoleSessionClicked(object sender, EventArgs e)
        {
            var selectedNodeAsContainer = _connectionTree.SelectedNode as ContainerInfo;
            if (selectedNodeAsContainer != null)
                Runtime.ConnectionInitiator.OpenConnection(selectedNodeAsContainer,
                                                           ConnectionInfo.Force.DontUseConsoleSession |
                                                           ConnectionInfo.Force.DoNotJump);
            else
                Runtime.ConnectionInitiator.OpenConnection(_connectionTree.SelectedNode,
                                                           ConnectionInfo.Force.DontUseConsoleSession |
                                                           ConnectionInfo.Force.DoNotJump);
        }

        private void OnConnectInFullscreenClicked(object sender, EventArgs e)
        {
            var selectedNodeAsContainer = _connectionTree.SelectedNode as ContainerInfo;
            if (selectedNodeAsContainer != null)
                Runtime.ConnectionInitiator.OpenConnection(selectedNodeAsContainer,
                                                           ConnectionInfo.Force.Fullscreen | ConnectionInfo.Force.DoNotJump);
            else
                Runtime.ConnectionInitiator.OpenConnection(_connectionTree.SelectedNode,
                                                           ConnectionInfo.Force.Fullscreen | ConnectionInfo.Force.DoNotJump);
        }

        private void OnConnectWithNoCredentialsClick(object sender, EventArgs e)
        {
            var selectedNodeAsContainer = _connectionTree.SelectedNode as ContainerInfo;
            if (selectedNodeAsContainer != null)
                Runtime.ConnectionInitiator.OpenConnection(selectedNodeAsContainer, ConnectionInfo.Force.NoCredentials);
            else
                Runtime.ConnectionInitiator.OpenConnection(_connectionTree.SelectedNode, ConnectionInfo.Force.NoCredentials);
        }

        private void OnChoosePanelBeforeConnectingClicked(object sender, EventArgs e)
        {
            var selectedNodeAsContainer = _connectionTree.SelectedNode as ContainerInfo;
            if (selectedNodeAsContainer != null)
                Runtime.ConnectionInitiator.OpenConnection(selectedNodeAsContainer,
                                                           ConnectionInfo.Force.OverridePanel |
                                                           ConnectionInfo.Force.DoNotJump);
            else
                Runtime.ConnectionInitiator.OpenConnection(_connectionTree.SelectedNode,
                                                           ConnectionInfo.Force.OverridePanel |
                                                           ConnectionInfo.Force.DoNotJump);
        }

        private void ConnectWithOptionsViewOnlyOnClick(object sender, EventArgs e)
        {
            var connectionTarget = _connectionTree.SelectedNode as ContainerInfo
                                   ?? _connectionTree.SelectedNode;
            Runtime.ConnectionInitiator.OpenConnection(connectionTarget, ConnectionInfo.Force.ViewOnly);
        }

        private void OnDisconnectClicked(object sender, EventArgs e)
        {
            DisconnectConnection(_connectionTree.SelectedNode);
        }

        public void DisconnectConnection(ConnectionInfo connectionInfo)
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
                Runtime.MessageCollector.AddExceptionStackTrace(
                                                                "DisconnectConnection (UI.Window.ConnectionTreeWindow) failed",
                                                                ex);
            }
        }

        private void OnTransferFileClicked(object sender, EventArgs e)
        {
            SshTransferFile();
        }

        public void SshTransferFile()
        {
            try
            {
                Windows.Show(WindowType.SSHTransfer);
                Windows.SshtransferForm.Hostname = _connectionTree.SelectedNode.Hostname;
                Windows.SshtransferForm.Username = _connectionTree.SelectedNode.Username;
                Windows.SshtransferForm.Password = _connectionTree.SelectedNode.Password;
                Windows.SshtransferForm.Port = Convert.ToString(_connectionTree.SelectedNode.Port);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(
                                                                "SSHTransferFile (UI.Window.ConnectionTreeWindow) failed",
                                                                ex);
            }
        }

        private void OnDuplicateClicked(object sender, EventArgs e)
        {
            _connectionTree.DuplicateSelectedNode();
        }

        private void OnRenameClicked(object sender, EventArgs e)
        {
            _connectionTree.RenameSelectedNode();
        }

        private void OnDeleteClicked(object sender, EventArgs e)
        {
            _connectionTree.DeleteSelectedNode();
        }

        private void OnCopyHostnameClicked(object sender, EventArgs e)
        {
            _connectionTree.CopyHostnameSelectedNode(new WindowsClipboard());
        }

        private void OnImportFileClicked(object sender, EventArgs e)
        {
            ContainerInfo selectedNodeAsContainer;
            if (_connectionTree.SelectedNode == null)
                selectedNodeAsContainer = Runtime.ConnectionsService.ConnectionTreeModel.RootNodes.First();
            else
                selectedNodeAsContainer =
                    _connectionTree.SelectedNode as ContainerInfo ?? _connectionTree.SelectedNode.Parent;
            Import.ImportFromFile(selectedNodeAsContainer);
        }

        private void OnImportRemoteDesktopManagerClicked(object sender, EventArgs e)
        {
            ContainerInfo selectedNodeAsContainer;
            if (_connectionTree.SelectedNode == null)
                selectedNodeAsContainer = Runtime.ConnectionsService.ConnectionTreeModel.RootNodes.First();
            else
                selectedNodeAsContainer =
                    _connectionTree.SelectedNode as ContainerInfo ?? _connectionTree.SelectedNode.Parent;
            Import.ImportFromRemoteDesktopManagerCsv(selectedNodeAsContainer);
        }

        private void OnImportActiveDirectoryClicked(object sender, EventArgs e)
        {
            Windows.Show(WindowType.ActiveDirectoryImport);
        }

        private void OnImportPortScanClicked(object sender, EventArgs e)
        {
            Windows.Show(WindowType.PortScan);
        }

        private void OnExportFileClicked(object sender, EventArgs e)
        {
            Export.ExportToFile(_connectionTree.SelectedNode, Runtime.ConnectionsService.ConnectionTreeModel);
        }

        private void OnAddConnectionClicked(object sender, EventArgs e)
        {
            _connectionTree.AddConnection();
        }

        private void OnAddFolderClicked(object sender, EventArgs e)
        {
            _connectionTree.AddFolder();
        }

        private void OnSortAscendingClicked(object sender, EventArgs e)
        {
            _connectionTree.SortRecursive(_connectionTree.SelectedNode, ListSortDirection.Ascending);
        }

        private void OnSortDescendingClicked(object sender, EventArgs e)
        {
            _connectionTree.SortRecursive(_connectionTree.SelectedNode, ListSortDirection.Descending);
        }

        private void OnMoveUpClicked(object sender, EventArgs e)
        {
            _connectionTree.SelectedNode.Parent.PromoteChild(_connectionTree.SelectedNode);
        }

        private void OnMoveDownClicked(object sender, EventArgs e)
        {
            _connectionTree.SelectedNode.Parent.DemoteChild(_connectionTree.SelectedNode);
        }

        private void OnExternalToolClicked(object sender, EventArgs e)
        {
            StartExternalApp((ExternalTool)((ToolStripMenuItem)sender).Tag);
        }

        private void StartExternalApp(ExternalTool externalTool)
        {
            try
            {
                if (_connectionTree.SelectedNode.GetTreeNodeType() == TreeNodeType.Connection |
                    _connectionTree.SelectedNode.GetTreeNodeType() == TreeNodeType.PuttySession |
                    _connectionTree.SelectedNode.GetTreeNodeType() == TreeNodeType.Container)
                    externalTool.Start(_connectionTree.SelectedNode);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(
                                                                "cMenTreeToolsExternalAppsEntry_Click failed (UI.Window.ConnectionTreeWindow)",
                                                                ex);
            }
        }

        private void OnApplyInheritanceToChildrenClicked(object sender, EventArgs e)
        {
            if (!(_connectionTree.SelectedNode is ContainerInfo container))
                return;

            container.ApplyInheritancePropertiesToChildren();
        }

        private void OnApplyDefaultInheritanceClicked(object sender, EventArgs e)
        {
            if (_connectionTree.SelectedNode == null)
                return;

            DefaultConnectionInheritance.Instance.SaveTo(_connectionTree.SelectedNode.Inheritance);
        }

        #endregion
    }
}