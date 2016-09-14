

namespace mRemoteNG.UI.Window
{
	public partial class ConnectionTreeWindow : BaseWindow
	{
        #region  Windows Form Designer generated code
		internal System.Windows.Forms.TextBox txtSearch;
		internal System.Windows.Forms.Panel pnlConnections;
		internal System.Windows.Forms.ImageList imgListTree;
		internal System.Windows.Forms.MenuStrip msMain;
		internal System.Windows.Forms.ToolStripMenuItem mMenView;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewExpandAllFolders;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewCollapseAllFolders;
		internal System.Windows.Forms.ContextMenuStrip cMenTree;
		internal System.Windows.Forms.ToolStripMenuItem cMenTreeAddConnection;
		internal System.Windows.Forms.ToolStripMenuItem cMenTreeAddFolder;
		internal System.Windows.Forms.ToolStripSeparator cMenTreeSep1;
		internal System.Windows.Forms.ToolStripMenuItem cMenTreeConnect;
		internal System.Windows.Forms.ToolStripMenuItem cMenTreeConnectWithOptions;
		internal System.Windows.Forms.ToolStripMenuItem cMenTreeConnectWithOptionsConnectToConsoleSession;
		internal System.Windows.Forms.ToolStripMenuItem cMenTreeConnectWithOptionsNoCredentials;
		internal System.Windows.Forms.ToolStripMenuItem cMenTreeConnectWithOptionsConnectInFullscreen;
		internal System.Windows.Forms.ToolStripMenuItem cMenTreeDisconnect;
		internal System.Windows.Forms.ToolStripSeparator cMenTreeSep2;
		internal System.Windows.Forms.ToolStripMenuItem cMenTreeToolsTransferFile;
		internal System.Windows.Forms.ToolStripMenuItem cMenTreeToolsSort;
		internal System.Windows.Forms.ToolStripMenuItem cMenTreeToolsSortAscending;
		internal System.Windows.Forms.ToolStripMenuItem cMenTreeToolsSortDescending;
		internal System.Windows.Forms.ToolStripSeparator cMenTreeSep3;
		internal System.Windows.Forms.ToolStripMenuItem cMenTreeRename;
		internal System.Windows.Forms.ToolStripMenuItem cMenTreeDelete;
		internal System.Windows.Forms.ToolStripSeparator cMenTreeSep4;
		internal System.Windows.Forms.ToolStripMenuItem cMenTreeMoveUp;
		internal System.Windows.Forms.ToolStripMenuItem cMenTreeMoveDown;
		internal System.Windows.Forms.PictureBox PictureBox1;
		internal System.Windows.Forms.ToolStripMenuItem cMenTreeToolsExternalApps;
		internal System.Windows.Forms.ToolStripMenuItem cMenTreeDuplicate;
		internal System.Windows.Forms.ToolStripMenuItem cMenTreeConnectWithOptionsChoosePanelBeforeConnecting;
		internal System.Windows.Forms.ToolStripMenuItem cMenTreeConnectWithOptionsDontConnectToConsoleSession;
		internal System.Windows.Forms.ToolStripMenuItem mMenSortAscending;
		internal System.Windows.Forms.ToolStripMenuItem mMenAddConnection;
		internal System.Windows.Forms.ToolStripMenuItem mMenAddFolder;
		public System.Windows.Forms.TreeView tvConnections;
	    public BrightIdeasSoftware.TreeListView olvConnections;
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.olvConnections = new BrightIdeasSoftware.TreeListView();
            this.olvNameColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.cMenTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cMenTreeConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenTreeConnectWithOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenTreeConnectWithOptionsConnectToConsoleSession = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenTreeConnectWithOptionsDontConnectToConsoleSession = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenTreeConnectWithOptionsConnectInFullscreen = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenTreeConnectWithOptionsNoCredentials = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenTreeConnectWithOptionsChoosePanelBeforeConnecting = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenTreeDisconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenTreeSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenTreeToolsExternalApps = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenTreeToolsTransferFile = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenTreeSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenTreeDuplicate = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenTreeRename = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenTreeDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenTreeSep3 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenTreeImport = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenTreeImportFile = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenTreeImportActiveDirectory = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenTreeImportPortScan = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenTreeExportFile = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenTreeSep4 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenTreeAddConnection = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenTreeAddFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenTreeToolsSort = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenTreeToolsSortAscending = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenTreeToolsSortDescending = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenTreeMoveUp = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenTreeMoveDown = new System.Windows.Forms.ToolStripMenuItem();
            this.imgListTree = new System.Windows.Forms.ImageList(this.components);
            this.pnlConnections = new System.Windows.Forms.Panel();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.msMain = new System.Windows.Forms.MenuStrip();
            this.mMenAddConnection = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenAddFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenView = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenViewExpandAllFolders = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenViewCollapseAllFolders = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenSortAscending = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.olvConnections)).BeginInit();
            this.cMenTree.SuspendLayout();
            this.pnlConnections.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            this.msMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // olvConnections
            // 
            this.olvConnections.AllColumns.Add(this.olvNameColumn);
            this.olvConnections.AllowDrop = true;
            this.olvConnections.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvConnections.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.olvConnections.CellEditUseWholeCell = false;
            this.olvConnections.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvNameColumn});
            this.olvConnections.ContextMenuStrip = this.cMenTree;
            this.olvConnections.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvConnections.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.olvConnections.HideSelection = false;
            this.olvConnections.IsSimpleDragSource = true;
            this.olvConnections.IsSimpleDropSink = true;
            this.olvConnections.LabelEdit = true;
            this.olvConnections.Location = new System.Drawing.Point(0, 0);
            this.olvConnections.MultiSelect = false;
            this.olvConnections.Name = "olvConnections";
            this.olvConnections.SelectedBackColor = System.Drawing.SystemColors.Highlight;
            this.olvConnections.SelectedForeColor = System.Drawing.SystemColors.HighlightText;
            this.olvConnections.ShowGroups = false;
            this.olvConnections.Size = new System.Drawing.Size(192, 410);
            this.olvConnections.SmallImageList = this.imgListTree;
            this.olvConnections.TabIndex = 20;
            this.olvConnections.UnfocusedSelectedBackColor = System.Drawing.SystemColors.Highlight;
            this.olvConnections.UnfocusedSelectedForeColor = System.Drawing.SystemColors.HighlightText;
            this.olvConnections.UseCompatibleStateImageBehavior = false;
            this.olvConnections.View = System.Windows.Forms.View.Details;
            this.olvConnections.VirtualMode = true;
            // 
            // olvNameColumn
            // 
            this.olvNameColumn.AspectName = "Name";
            this.olvNameColumn.FillsFreeSpace = true;
            this.olvNameColumn.IsButton = true;
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
            this.cMenTree.Opening += new System.ComponentModel.CancelEventHandler(this.cMenTree_DropDownOpening);
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
            this.cMenTreeImportActiveDirectory.Click += new System.EventHandler(this.cMenTreeImportActiveDirectory_Click);
            // 
            // cMenTreeImportPortScan
            // 
            this.cMenTreeImportPortScan.Name = "cMenTreeImportPortScan";
            this.cMenTreeImportPortScan.Size = new System.Drawing.Size(226, 22);
            this.cMenTreeImportPortScan.Text = "Import from &Port Scan...";
            this.cMenTreeImportPortScan.Click += new System.EventHandler(this.cMenTreeImportPortScan_Click);
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
            // 
            // imgListTree
            // 
            this.imgListTree.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imgListTree.ImageSize = new System.Drawing.Size(16, 16);
            this.imgListTree.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // pnlConnections
            // 
            this.pnlConnections.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlConnections.Controls.Add(this.PictureBox1);
            this.pnlConnections.Controls.Add(this.txtSearch);
            this.pnlConnections.Controls.Add(this.olvConnections);
            this.pnlConnections.Location = new System.Drawing.Point(0, 25);
            this.pnlConnections.Name = "pnlConnections";
            this.pnlConnections.Size = new System.Drawing.Size(192, 428);
            this.pnlConnections.TabIndex = 9;
            // 
            // PictureBox1
            // 
            this.PictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.PictureBox1.Image = global::mRemoteNG.Resources.Search;
            this.PictureBox1.Location = new System.Drawing.Point(2, 412);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(16, 16);
            this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PictureBox1.TabIndex = 1;
            this.PictureBox1.TabStop = false;
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSearch.ForeColor = System.Drawing.SystemColors.GrayText;
            this.txtSearch.Location = new System.Drawing.Point(19, 413);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(171, 15);
            this.txtSearch.TabIndex = 30;
            this.txtSearch.TabStop = false;
            this.txtSearch.Text = "Search";
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.GotFocus += new System.EventHandler(this.txtSearch_GotFocus);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            this.txtSearch.LostFocus += new System.EventHandler(this.txtSearch_LostFocus);
            // 
            // msMain
            // 
            this.msMain.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mMenAddConnection,
            this.mMenAddFolder,
            this.mMenView,
            this.mMenSortAscending});
            this.msMain.Location = new System.Drawing.Point(0, 0);
            this.msMain.Name = "msMain";
            this.msMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.msMain.ShowItemToolTips = true;
            this.msMain.Size = new System.Drawing.Size(192, 24);
            this.msMain.TabIndex = 10;
            this.msMain.Text = "MenuStrip1";
            // 
            // mMenAddConnection
            // 
            this.mMenAddConnection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mMenAddConnection.Image = global::mRemoteNG.Resources.Connection_Add;
            this.mMenAddConnection.Name = "mMenAddConnection";
            this.mMenAddConnection.Size = new System.Drawing.Size(28, 20);
            this.mMenAddConnection.Click += new System.EventHandler(this.cMenTreeAddConnection_Click);
            // 
            // mMenAddFolder
            // 
            this.mMenAddFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mMenAddFolder.Image = global::mRemoteNG.Resources.Folder_Add;
            this.mMenAddFolder.Name = "mMenAddFolder";
            this.mMenAddFolder.Size = new System.Drawing.Size(28, 20);
            this.mMenAddFolder.Click += new System.EventHandler(this.cMenTreeAddFolder_Click);
            // 
            // mMenView
            // 
            this.mMenView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mMenView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mMenViewExpandAllFolders,
            this.mMenViewCollapseAllFolders});
            this.mMenView.Image = global::mRemoteNG.Resources.View;
            this.mMenView.Name = "mMenView";
            this.mMenView.Size = new System.Drawing.Size(28, 20);
            this.mMenView.Text = "&View";
            // 
            // mMenViewExpandAllFolders
            // 
            this.mMenViewExpandAllFolders.Image = global::mRemoteNG.Resources.Expand;
            this.mMenViewExpandAllFolders.Name = "mMenViewExpandAllFolders";
            this.mMenViewExpandAllFolders.Size = new System.Drawing.Size(172, 22);
            this.mMenViewExpandAllFolders.Text = "Expand all folders";
            // 
            // mMenViewCollapseAllFolders
            // 
            this.mMenViewCollapseAllFolders.Image = global::mRemoteNG.Resources.Collapse;
            this.mMenViewCollapseAllFolders.Name = "mMenViewCollapseAllFolders";
            this.mMenViewCollapseAllFolders.Size = new System.Drawing.Size(172, 22);
            this.mMenViewCollapseAllFolders.Text = "Collapse all folders";
            // 
            // mMenSortAscending
            // 
            this.mMenSortAscending.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mMenSortAscending.Image = global::mRemoteNG.Resources.Sort_AZ;
            this.mMenSortAscending.Name = "mMenSortAscending";
            this.mMenSortAscending.Size = new System.Drawing.Size(28, 20);
            this.mMenSortAscending.Click += new System.EventHandler(this.mMenSortAscending_Click);
            // 
            // ConnectionTreeWindow
            // 
            this.ClientSize = new System.Drawing.Size(192, 453);
            this.Controls.Add(this.msMain);
            this.Controls.Add(this.pnlConnections);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Icon = global::mRemoteNG.Resources.Root_Icon;
            this.Name = "ConnectionTreeWindow";
            this.TabText = "Connections";
            this.Text = "Connections";
            this.Load += new System.EventHandler(this.Tree_Load);
            ((System.ComponentModel.ISupportInitialize)(this.olvConnections)).EndInit();
            this.cMenTree.ResumeLayout(false);
            this.pnlConnections.ResumeLayout(false);
            this.pnlConnections.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            this.msMain.ResumeLayout(false);
            this.msMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		internal System.Windows.Forms.ToolStripMenuItem cMenTreeImport;
		internal System.Windows.Forms.ToolStripMenuItem cMenTreeExportFile;
		internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
		internal System.Windows.Forms.ToolStripMenuItem cMenTreeImportFile;
		internal System.Windows.Forms.ToolStripMenuItem cMenTreeImportActiveDirectory;
		internal System.Windows.Forms.ToolStripMenuItem cMenTreeImportPortScan;
        #endregion

        private System.ComponentModel.IContainer components;
        private BrightIdeasSoftware.OLVColumn olvNameColumn;
    }
}
