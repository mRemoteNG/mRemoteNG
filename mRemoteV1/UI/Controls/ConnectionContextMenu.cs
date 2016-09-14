using System.Windows.Forms;


namespace mRemoteNG.UI.Controls
{
    internal class ConnectionContextMenu : ContextMenuStrip
    {
        private ToolStripMenuItem cMenTreeAddConnection;
        private ToolStripMenuItem cMenTreeAddFolder;
        private ToolStripSeparator cMenTreeSep1;
        private ToolStripMenuItem cMenTreeConnect;
        private ToolStripMenuItem cMenTreeConnectWithOptions;
        private ToolStripMenuItem cMenTreeConnectWithOptionsConnectToConsoleSession;
        private ToolStripMenuItem cMenTreeConnectWithOptionsNoCredentials;
        private ToolStripMenuItem cMenTreeConnectWithOptionsConnectInFullscreen;
        private ToolStripMenuItem cMenTreeDisconnect;
        private ToolStripSeparator cMenTreeSep2;
        private ToolStripMenuItem cMenTreeToolsTransferFile;
        private ToolStripMenuItem cMenTreeToolsSort;
        private ToolStripMenuItem cMenTreeToolsSortAscending;
        private ToolStripMenuItem cMenTreeToolsSortDescending;
        private ToolStripSeparator cMenTreeSep3;
        private ToolStripMenuItem cMenTreeRename;
        private ToolStripMenuItem cMenTreeDelete;
        private ToolStripSeparator cMenTreeSep4;
        private ToolStripMenuItem cMenTreeMoveUp;
        private ToolStripMenuItem cMenTreeMoveDown;
        private PictureBox PictureBox1;
        private ToolStripMenuItem cMenTreeToolsExternalApps;
        private ToolStripMenuItem cMenTreeDuplicate;
        private ToolStripMenuItem cMenTreeConnectWithOptionsChoosePanelBeforeConnecting;
        private ToolStripMenuItem cMenTreeConnectWithOptionsDontConnectToConsoleSession;
        private ToolStripMenuItem cMenTreeImport;
        private ToolStripMenuItem cMenTreeExportFile;
        private ToolStripSeparator ToolStripSeparator1;
        private ToolStripMenuItem cMenTreeImportFile;
        private ToolStripMenuItem cMenTreeImportActiveDirectory;
        private ToolStripMenuItem cMenTreeImportPortScan;


        public ConnectionContextMenu()
        {
            
        }

        private void InitializeComponent()
        {
            cMenTreeConnect = new ToolStripMenuItem();
            cMenTreeConnectWithOptions = new ToolStripMenuItem();
            cMenTreeConnectWithOptionsConnectToConsoleSession = new ToolStripMenuItem();
            cMenTreeConnectWithOptionsDontConnectToConsoleSession = new ToolStripMenuItem();
            cMenTreeConnectWithOptionsConnectInFullscreen = new ToolStripMenuItem();
            cMenTreeConnectWithOptionsNoCredentials = new ToolStripMenuItem();
            cMenTreeConnectWithOptionsChoosePanelBeforeConnecting = new ToolStripMenuItem();
            cMenTreeDisconnect = new ToolStripMenuItem();
            cMenTreeSep1 = new ToolStripSeparator();
            cMenTreeToolsExternalApps = new ToolStripMenuItem();
            cMenTreeToolsTransferFile = new ToolStripMenuItem();
            cMenTreeSep2 = new ToolStripSeparator();
            cMenTreeDuplicate = new ToolStripMenuItem();
            cMenTreeRename = new ToolStripMenuItem();
            cMenTreeDelete = new ToolStripMenuItem();
            cMenTreeSep3 = new ToolStripSeparator();
            cMenTreeImport = new ToolStripMenuItem();
            cMenTreeImportFile = new ToolStripMenuItem();
            cMenTreeImportActiveDirectory = new ToolStripMenuItem();
            cMenTreeImportPortScan = new ToolStripMenuItem();
            cMenTreeExportFile = new ToolStripMenuItem();
            cMenTreeSep4 = new ToolStripSeparator();
            cMenTreeAddConnection = new ToolStripMenuItem();
            cMenTreeAddFolder = new ToolStripMenuItem();
            ToolStripSeparator1 = new ToolStripSeparator();
            cMenTreeToolsSort = new ToolStripMenuItem();
            cMenTreeToolsSortAscending = new ToolStripMenuItem();
            cMenTreeToolsSortDescending = new ToolStripMenuItem();
            cMenTreeMoveUp = new ToolStripMenuItem();
            cMenTreeMoveDown = new ToolStripMenuItem();


            // 
            // cMenTree
            // 
            this.cMenTree.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cMenTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cMenTreeConnect,
            this.cMenTreeConnectWithOptions,
            this.cMenTreeDisconnect,
            this.cMenTreeSep1,
            this.cMenTreeToolsExternalApps,
            this.cMenTreeToolsTransferFile,
            this.cMenTreeSep2,
            this.cMenTreeDuplicate,
            this.cMenTreeRename,
            this.cMenTreeDelete,
            this.cMenTreeSep3,
            this.cMenTreeImport,
            this.cMenTreeExportFile,
            this.cMenTreeSep4,
            this.cMenTreeAddConnection,
            this.cMenTreeAddFolder,
            this.ToolStripSeparator1,
            this.cMenTreeToolsSort,
            this.cMenTreeMoveUp,
            this.cMenTreeMoveDown});
            this.cMenTree.Name = "cMenTree";
            this.cMenTree.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.cMenTree.Size = new System.Drawing.Size(200, 364);
            // 
            // cMenTreeConnect
            // 
            this.cMenTreeConnect.Image = global::mRemoteNG.Resources.Play;
            this.cMenTreeConnect.Name = "cMenTreeConnect";
            this.cMenTreeConnect.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
            | System.Windows.Forms.Keys.C)));
            this.cMenTreeConnect.Size = new System.Drawing.Size(199, 22);
            this.cMenTreeConnect.Text = "Connect";
            this.cMenTreeConnect.Click += new System.EventHandler(this.cMenTreeConnect_Click);
            // 
            // cMenTreeConnectWithOptions
            // 
            this.cMenTreeConnectWithOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cMenTreeConnectWithOptionsConnectToConsoleSession,
            this.cMenTreeConnectWithOptionsDontConnectToConsoleSession,
            this.cMenTreeConnectWithOptionsConnectInFullscreen,
            this.cMenTreeConnectWithOptionsNoCredentials,
            this.cMenTreeConnectWithOptionsChoosePanelBeforeConnecting});
            this.cMenTreeConnectWithOptions.Name = "cMenTreeConnectWithOptions";
            this.cMenTreeConnectWithOptions.Size = new System.Drawing.Size(199, 22);
            this.cMenTreeConnectWithOptions.Text = "Connect (with options)";
            // 
            // cMenTreeConnectWithOptionsConnectToConsoleSession
            // 
            this.cMenTreeConnectWithOptionsConnectToConsoleSession.Image = global::mRemoteNG.Resources.monitor_go;
            this.cMenTreeConnectWithOptionsConnectToConsoleSession.Name = "cMenTreeConnectWithOptionsConnectToConsoleSession";
            this.cMenTreeConnectWithOptionsConnectToConsoleSession.Size = new System.Drawing.Size(245, 22);
            this.cMenTreeConnectWithOptionsConnectToConsoleSession.Text = "Connect to console session";
            this.cMenTreeConnectWithOptionsConnectToConsoleSession.Click += new System.EventHandler(this.cMenTreeConnectWithOptionsConnectToConsoleSession_Click);
            // 
            // cMenTreeConnectWithOptionsDontConnectToConsoleSession
            // 
            this.cMenTreeConnectWithOptionsDontConnectToConsoleSession.Image = global::mRemoteNG.Resources.monitor_delete;
            this.cMenTreeConnectWithOptionsDontConnectToConsoleSession.Name = "cMenTreeConnectWithOptionsDontConnectToConsoleSession";
            this.cMenTreeConnectWithOptionsDontConnectToConsoleSession.Size = new System.Drawing.Size(245, 22);
            this.cMenTreeConnectWithOptionsDontConnectToConsoleSession.Text = "Don\'t connect to console session";
            this.cMenTreeConnectWithOptionsDontConnectToConsoleSession.Visible = false;
            this.cMenTreeConnectWithOptionsDontConnectToConsoleSession.Click += new System.EventHandler(this.cMenTreeConnectWithOptionsDontConnectToConsoleSession_Click);
            // 
            // cMenTreeConnectWithOptionsConnectInFullscreen
            // 
            this.cMenTreeConnectWithOptionsConnectInFullscreen.Image = global::mRemoteNG.Resources.arrow_out;
            this.cMenTreeConnectWithOptionsConnectInFullscreen.Name = "cMenTreeConnectWithOptionsConnectInFullscreen";
            this.cMenTreeConnectWithOptionsConnectInFullscreen.Size = new System.Drawing.Size(245, 22);
            this.cMenTreeConnectWithOptionsConnectInFullscreen.Text = "Connect in fullscreen";
            this.cMenTreeConnectWithOptionsConnectInFullscreen.Click += new System.EventHandler(this.cMenTreeConnectWithOptionsConnectInFullscreen_Click);
            // 
            // cMenTreeConnectWithOptionsNoCredentials
            // 
            this.cMenTreeConnectWithOptionsNoCredentials.Image = global::mRemoteNG.Resources.key_delete;
            this.cMenTreeConnectWithOptionsNoCredentials.Name = "cMenTreeConnectWithOptionsNoCredentials";
            this.cMenTreeConnectWithOptionsNoCredentials.Size = new System.Drawing.Size(245, 22);
            this.cMenTreeConnectWithOptionsNoCredentials.Text = "Connect without credentials";
            this.cMenTreeConnectWithOptionsNoCredentials.Click += new System.EventHandler(this.cMenTreeConnectWithOptionsNoCredentials_Click);
            // 
            // cMenTreeConnectWithOptionsChoosePanelBeforeConnecting
            // 
            this.cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Image = global::mRemoteNG.Resources.Panels;
            this.cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Name = "cMenTreeConnectWithOptionsChoosePanelBeforeConnecting";
            this.cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Size = new System.Drawing.Size(245, 22);
            this.cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Text = "Choose panel before connecting";
            this.cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Click += new System.EventHandler(this.cMenTreeConnectWithOptionsChoosePanelBeforeConnecting_Click);
            // 
            // cMenTreeDisconnect
            // 
            this.cMenTreeDisconnect.Image = global::mRemoteNG.Resources.Pause;
            this.cMenTreeDisconnect.Name = "cMenTreeDisconnect";
            this.cMenTreeDisconnect.Size = new System.Drawing.Size(199, 22);
            this.cMenTreeDisconnect.Text = "Disconnect";
            this.cMenTreeDisconnect.Click += new System.EventHandler(this.cMenTreeDisconnect_Click);
            // 
            // cMenTreeSep1
            // 
            this.cMenTreeSep1.Name = "cMenTreeSep1";
            this.cMenTreeSep1.Size = new System.Drawing.Size(196, 6);
            // 
            // cMenTreeToolsExternalApps
            // 
            this.cMenTreeToolsExternalApps.Image = global::mRemoteNG.Resources.ExtApp;
            this.cMenTreeToolsExternalApps.Name = "cMenTreeToolsExternalApps";
            this.cMenTreeToolsExternalApps.Size = new System.Drawing.Size(199, 22);
            this.cMenTreeToolsExternalApps.Text = "External Applications";
            // 
            // cMenTreeToolsTransferFile
            // 
            this.cMenTreeToolsTransferFile.Image = global::mRemoteNG.Resources.SSHTransfer;
            this.cMenTreeToolsTransferFile.Name = "cMenTreeToolsTransferFile";
            this.cMenTreeToolsTransferFile.Size = new System.Drawing.Size(199, 22);
            this.cMenTreeToolsTransferFile.Text = "Transfer File (SSH)";
            this.cMenTreeToolsTransferFile.Click += new System.EventHandler(this.cMenTreeToolsTransferFile_Click);
            // 
            // cMenTreeSep2
            // 
            this.cMenTreeSep2.Name = "cMenTreeSep2";
            this.cMenTreeSep2.Size = new System.Drawing.Size(196, 6);
            // 
            // cMenTreeDuplicate
            // 
            this.cMenTreeDuplicate.Image = global::mRemoteNG.Resources.page_copy;
            this.cMenTreeDuplicate.Name = "cMenTreeDuplicate";
            this.cMenTreeDuplicate.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.cMenTreeDuplicate.Size = new System.Drawing.Size(199, 22);
            this.cMenTreeDuplicate.Text = "Duplicate";
            // 
            // cMenTreeRename
            // 
            this.cMenTreeRename.Image = global::mRemoteNG.Resources.Rename;
            this.cMenTreeRename.Name = "cMenTreeRename";
            this.cMenTreeRename.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.cMenTreeRename.Size = new System.Drawing.Size(199, 22);
            this.cMenTreeRename.Text = "Rename";
            // 
            // cMenTreeDelete
            // 
            this.cMenTreeDelete.Image = global::mRemoteNG.Resources.Delete;
            this.cMenTreeDelete.Name = "cMenTreeDelete";
            this.cMenTreeDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.cMenTreeDelete.Size = new System.Drawing.Size(199, 22);
            this.cMenTreeDelete.Text = "Delete";
            // 
            // cMenTreeSep3
            // 
            this.cMenTreeSep3.Name = "cMenTreeSep3";
            this.cMenTreeSep3.Size = new System.Drawing.Size(196, 6);
            // 
            // cMenTreeImport
            // 
            this.cMenTreeImport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cMenTreeImportFile,
            this.cMenTreeImportActiveDirectory,
            this.cMenTreeImportPortScan});
            this.cMenTreeImport.Name = "cMenTreeImport";
            this.cMenTreeImport.Size = new System.Drawing.Size(199, 22);
            this.cMenTreeImport.Text = "&Import";
            // 
            // cMenTreeImportFile
            // 
            this.cMenTreeImportFile.Name = "cMenTreeImportFile";
            this.cMenTreeImportFile.Size = new System.Drawing.Size(226, 22);
            this.cMenTreeImportFile.Text = "Import from &File...";
            this.cMenTreeImportFile.Click += new System.EventHandler(this.cMenTreeImportFile_Click);
            // 
            // cMenTreeImportActiveDirectory
            // 
            this.cMenTreeImportActiveDirectory.Name = "cMenTreeImportActiveDirectory";
            this.cMenTreeImportActiveDirectory.Size = new System.Drawing.Size(226, 22);
            this.cMenTreeImportActiveDirectory.Text = "Import from &Active Directory...";
            // 
            // cMenTreeImportPortScan
            // 
            this.cMenTreeImportPortScan.Name = "cMenTreeImportPortScan";
            this.cMenTreeImportPortScan.Size = new System.Drawing.Size(226, 22);
            this.cMenTreeImportPortScan.Text = "Import from &Port Scan...";
            // 
            // cMenTreeExportFile
            // 
            this.cMenTreeExportFile.Name = "cMenTreeExportFile";
            this.cMenTreeExportFile.Size = new System.Drawing.Size(199, 22);
            this.cMenTreeExportFile.Text = "&Export to File...";
            this.cMenTreeExportFile.Click += new System.EventHandler(this.cMenTreeExportFile_Click);
            // 
            // cMenTreeSep4
            // 
            this.cMenTreeSep4.Name = "cMenTreeSep4";
            this.cMenTreeSep4.Size = new System.Drawing.Size(196, 6);
            // 
            // cMenTreeAddConnection
            // 
            this.cMenTreeAddConnection.Image = global::mRemoteNG.Resources.Connection_Add;
            this.cMenTreeAddConnection.Name = "cMenTreeAddConnection";
            this.cMenTreeAddConnection.Size = new System.Drawing.Size(199, 22);
            this.cMenTreeAddConnection.Text = "New Connection";
            this.cMenTreeAddConnection.Click += new System.EventHandler(this.cMenTreeAddConnection_Click);
            // 
            // cMenTreeAddFolder
            // 
            this.cMenTreeAddFolder.Image = global::mRemoteNG.Resources.Folder_Add;
            this.cMenTreeAddFolder.Name = "cMenTreeAddFolder";
            this.cMenTreeAddFolder.Size = new System.Drawing.Size(199, 22);
            this.cMenTreeAddFolder.Text = "New Folder";
            this.cMenTreeAddFolder.Click += new System.EventHandler(this.cMenTreeAddFolder_Click);
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(196, 6);
            // 
            // cMenTreeToolsSort
            // 
            this.cMenTreeToolsSort.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cMenTreeToolsSortAscending,
            this.cMenTreeToolsSortDescending});
            this.cMenTreeToolsSort.Name = "cMenTreeToolsSort";
            this.cMenTreeToolsSort.Size = new System.Drawing.Size(199, 22);
            this.cMenTreeToolsSort.Text = "Sort";
            // 
            // cMenTreeToolsSortAscending
            // 
            this.cMenTreeToolsSortAscending.Image = global::mRemoteNG.Resources.Sort_AZ;
            this.cMenTreeToolsSortAscending.Name = "cMenTreeToolsSortAscending";
            this.cMenTreeToolsSortAscending.Size = new System.Drawing.Size(161, 22);
            this.cMenTreeToolsSortAscending.Text = "Ascending (A-Z)";
            this.cMenTreeToolsSortAscending.Click += new System.EventHandler(this.cMenTreeToolsSortAscending_Click);
            // 
            // cMenTreeToolsSortDescending
            // 
            this.cMenTreeToolsSortDescending.Image = global::mRemoteNG.Resources.Sort_ZA;
            this.cMenTreeToolsSortDescending.Name = "cMenTreeToolsSortDescending";
            this.cMenTreeToolsSortDescending.Size = new System.Drawing.Size(161, 22);
            this.cMenTreeToolsSortDescending.Text = "Descending (Z-A)";
            this.cMenTreeToolsSortDescending.Click += new System.EventHandler(this.cMenTreeToolsSortDescending_Click);
            // 
            // cMenTreeMoveUp
            // 
            this.cMenTreeMoveUp.Image = global::mRemoteNG.Resources.Arrow_Up;
            this.cMenTreeMoveUp.Name = "cMenTreeMoveUp";
            this.cMenTreeMoveUp.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Up)));
            this.cMenTreeMoveUp.Size = new System.Drawing.Size(199, 22);
            this.cMenTreeMoveUp.Text = "Move up";
            this.cMenTreeMoveUp.Click += new System.EventHandler(this.cMenTreeMoveUp_Click);
            // 
            // cMenTreeMoveDown
            // 
            this.cMenTreeMoveDown.Image = global::mRemoteNG.Resources.Arrow_Down;
            this.cMenTreeMoveDown.Name = "cMenTreeMoveDown";
            this.cMenTreeMoveDown.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Down)));
            this.cMenTreeMoveDown.Size = new System.Drawing.Size(199, 22);
            this.cMenTreeMoveDown.Text = "Move down";
            this.cMenTreeMoveDown.Click += new System.EventHandler(this.cMenTreeMoveDown_Click);
        }
    }
}