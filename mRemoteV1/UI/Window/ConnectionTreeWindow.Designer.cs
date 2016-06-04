

using mRemoteNG.My;

namespace mRemoteNG.UI.Window
{
	public partial class ConnectionTreeWindow : BaseWindow
	{
        #region  Windows Form Designer generated code
		private System.ComponentModel.Container components = null;
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
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.Load += new System.EventHandler(Tree_Load);
			System.Windows.Forms.TreeNode TreeNode1 = new System.Windows.Forms.TreeNode("Connections");
			this.tvConnections = new System.Windows.Forms.TreeView();
			this.tvConnections.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvConnections_BeforeLabelEdit);
			this.tvConnections.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvConnections_AfterLabelEdit);
			this.tvConnections.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvConnections_AfterSelect);
			this.tvConnections.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvConnections_NodeMouseClick);
			this.tvConnections.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(tvConnections_NodeMouseDoubleClick);
			this.tvConnections.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tvConnections_MouseMove);
			this.tvConnections.DragDrop += new System.Windows.Forms.DragEventHandler(tvConnections_DragDrop);
			this.tvConnections.DragEnter += new System.Windows.Forms.DragEventHandler(tvConnections_DragEnter);
			this.tvConnections.DragOver += new System.Windows.Forms.DragEventHandler(tvConnections_DragOver);
			this.tvConnections.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvConnections_ItemDrag);
			this.tvConnections.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tvConnections_KeyPress);
			this.tvConnections.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvConnections_KeyDown);
			this.cMenTree = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cMenTree.Opening += new System.ComponentModel.CancelEventHandler(this.cMenTree_DropDownOpening);
			this.cMenTreeConnect = new System.Windows.Forms.ToolStripMenuItem();
			this.cMenTreeConnect.Click += new System.EventHandler(cMenTreeConnect_Click);
			this.cMenTreeConnectWithOptions = new System.Windows.Forms.ToolStripMenuItem();
			this.cMenTreeConnectWithOptionsConnectToConsoleSession = new System.Windows.Forms.ToolStripMenuItem();
			this.cMenTreeConnectWithOptionsConnectToConsoleSession.Click += new System.EventHandler(cMenTreeConnectWithOptionsConnectToConsoleSession_Click);
			this.cMenTreeConnectWithOptionsDontConnectToConsoleSession = new System.Windows.Forms.ToolStripMenuItem();
			this.cMenTreeConnectWithOptionsDontConnectToConsoleSession.Click += new System.EventHandler(cMenTreeConnectWithOptionsDontConnectToConsoleSession_Click);
			this.cMenTreeConnectWithOptionsConnectInFullscreen = new System.Windows.Forms.ToolStripMenuItem();
			this.cMenTreeConnectWithOptionsConnectInFullscreen.Click += new System.EventHandler(cMenTreeConnectWithOptionsConnectInFullscreen_Click);
			this.cMenTreeConnectWithOptionsNoCredentials = new System.Windows.Forms.ToolStripMenuItem();
			this.cMenTreeConnectWithOptionsNoCredentials.Click += new System.EventHandler(cMenTreeConnectWithOptionsNoCredentials_Click);
			this.cMenTreeConnectWithOptionsChoosePanelBeforeConnecting = new System.Windows.Forms.ToolStripMenuItem();
			this.cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Click += new System.EventHandler(cMenTreeConnectWithOptionsChoosePanelBeforeConnecting_Click);
			this.cMenTreeDisconnect = new System.Windows.Forms.ToolStripMenuItem();
			this.cMenTreeDisconnect.Click += new System.EventHandler(this.cMenTreeDisconnect_Click);
			this.cMenTreeSep1 = new System.Windows.Forms.ToolStripSeparator();
			this.cMenTreeToolsExternalApps = new System.Windows.Forms.ToolStripMenuItem();
			this.cMenTreeToolsTransferFile = new System.Windows.Forms.ToolStripMenuItem();
			this.cMenTreeToolsTransferFile.Click += new System.EventHandler(cMenTreeToolsTransferFile_Click);
			this.cMenTreeSep2 = new System.Windows.Forms.ToolStripSeparator();
			this.cMenTreeDuplicate = new System.Windows.Forms.ToolStripMenuItem();
			this.cMenTreeDuplicate.Click += new System.EventHandler(cMenTreeDuplicate_Click);
			this.cMenTreeRename = new System.Windows.Forms.ToolStripMenuItem();
			this.cMenTreeRename.Click += new System.EventHandler(cMenTreeRename_Click);
			this.cMenTreeDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.cMenTreeDelete.Click += new System.EventHandler(cMenTreeDelete_Click);
			this.cMenTreeSep3 = new System.Windows.Forms.ToolStripSeparator();
			this.cMenTreeImport = new System.Windows.Forms.ToolStripMenuItem();
			this.cMenTreeImportFile = new System.Windows.Forms.ToolStripMenuItem();
			this.cMenTreeImportFile.Click += new System.EventHandler(cMenTreeImportFile_Click);
			this.cMenTreeImportActiveDirectory = new System.Windows.Forms.ToolStripMenuItem();
			this.cMenTreeImportActiveDirectory.Click += new System.EventHandler(cMenTreeImportActiveDirectory_Click);
			this.cMenTreeImportPortScan = new System.Windows.Forms.ToolStripMenuItem();
			this.cMenTreeImportPortScan.Click += new System.EventHandler(cMenTreeImportPortScan_Click);
			this.cMenTreeExportFile = new System.Windows.Forms.ToolStripMenuItem();
			this.cMenTreeExportFile.Click += new System.EventHandler(cMenTreeExportFile_Click);
			this.cMenTreeSep4 = new System.Windows.Forms.ToolStripSeparator();
			this.cMenTreeAddConnection = new System.Windows.Forms.ToolStripMenuItem();
			this.cMenTreeAddConnection.Click += new System.EventHandler(this.cMenTreeAddConnection_Click);
			this.cMenTreeAddFolder = new System.Windows.Forms.ToolStripMenuItem();
			this.cMenTreeAddFolder.Click += new System.EventHandler(this.cMenTreeAddFolder_Click);
			this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.cMenTreeToolsSort = new System.Windows.Forms.ToolStripMenuItem();
			this.cMenTreeToolsSortAscending = new System.Windows.Forms.ToolStripMenuItem();
			this.cMenTreeToolsSortAscending.Click += new System.EventHandler(this.cMenTreeToolsSortAscending_Click);
			this.cMenTreeToolsSortDescending = new System.Windows.Forms.ToolStripMenuItem();
			this.cMenTreeToolsSortDescending.Click += new System.EventHandler(this.cMenTreeToolsSortDescending_Click);
			this.cMenTreeMoveUp = new System.Windows.Forms.ToolStripMenuItem();
			this.cMenTreeMoveUp.Click += new System.EventHandler(cMenTreeMoveUp_Click);
			this.cMenTreeMoveDown = new System.Windows.Forms.ToolStripMenuItem();
			this.cMenTreeMoveDown.Click += new System.EventHandler(cMenTreeMoveDown_Click);
			this.imgListTree = new System.Windows.Forms.ImageList(this.components);
			this.pnlConnections = new System.Windows.Forms.Panel();
			this.PictureBox1 = new System.Windows.Forms.PictureBox();
			this.txtSearch = new System.Windows.Forms.TextBox();
			this.txtSearch.GotFocus += new System.EventHandler(this.txtSearch_GotFocus);
			this.txtSearch.LostFocus += new System.EventHandler(this.txtSearch_LostFocus);
			this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
			this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
			this.msMain = new System.Windows.Forms.MenuStrip();
			this.mMenAddConnection = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenAddConnection.Click += new System.EventHandler(this.cMenTreeAddConnection_Click);
			this.mMenAddFolder = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenAddFolder.Click += new System.EventHandler(this.cMenTreeAddFolder_Click);
			this.mMenView = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenViewExpandAllFolders = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenViewExpandAllFolders.Click += new System.EventHandler(mMenViewExpandAllFolders_Click);
			this.mMenViewCollapseAllFolders = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenViewCollapseAllFolders.Click += new System.EventHandler(this.mMenViewCollapseAllFolders_Click);
			this.mMenSortAscending = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenSortAscending.Click += new System.EventHandler(this.mMenSortAscending_Click);
			this.cMenTree.SuspendLayout();
			this.pnlConnections.SuspendLayout();
			((System.ComponentModel.ISupportInitialize) this.PictureBox1).BeginInit();
			this.msMain.SuspendLayout();
			this.SuspendLayout();
			//
			//tvConnections
			//
			this.tvConnections.AllowDrop = true;
			this.tvConnections.Anchor = (System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.tvConnections.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.tvConnections.ContextMenuStrip = this.cMenTree;
			this.tvConnections.HideSelection = false;
			this.tvConnections.ImageIndex = 0;
			this.tvConnections.ImageList = this.imgListTree;
			this.tvConnections.LabelEdit = true;
			this.tvConnections.Location = new System.Drawing.Point(0, 0);
			this.tvConnections.Name = "tvConnections";
			TreeNode1.Name = "nodeRoot";
			TreeNode1.Text = "Connections";
			this.tvConnections.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {TreeNode1});
			this.tvConnections.SelectedImageIndex = 0;
			this.tvConnections.Size = new System.Drawing.Size(192, 410);
			this.tvConnections.TabIndex = 20;
			//
			//cMenTree
			//
			this.cMenTree.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, System.Convert.ToByte(0));
			this.cMenTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this.cMenTreeConnect, this.cMenTreeConnectWithOptions, this.cMenTreeDisconnect, this.cMenTreeSep1, this.cMenTreeToolsExternalApps, this.cMenTreeToolsTransferFile, this.cMenTreeSep2, this.cMenTreeDuplicate, this.cMenTreeRename, this.cMenTreeDelete, this.cMenTreeSep3, this.cMenTreeImport, this.cMenTreeExportFile, this.cMenTreeSep4, this.cMenTreeAddConnection, this.cMenTreeAddFolder, this.ToolStripSeparator1, this.cMenTreeToolsSort, this.cMenTreeMoveUp, this.cMenTreeMoveDown});
			this.cMenTree.Name = "cMenTree";
			this.cMenTree.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.cMenTree.Size = new System.Drawing.Size(187, 386);
			//
			//cMenTreeConnect
			//
			this.cMenTreeConnect.Image = Resources.Play;
			this.cMenTreeConnect.Name = "cMenTreeConnect";
			this.cMenTreeConnect.ShortcutKeys = (System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
				| System.Windows.Forms.Keys.C);
			this.cMenTreeConnect.Size = new System.Drawing.Size(186, 22);
			this.cMenTreeConnect.Text = "Connect";
			//
			//cMenTreeConnectWithOptions
			//
			this.cMenTreeConnectWithOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {this.cMenTreeConnectWithOptionsConnectToConsoleSession, this.cMenTreeConnectWithOptionsDontConnectToConsoleSession, this.cMenTreeConnectWithOptionsConnectInFullscreen, this.cMenTreeConnectWithOptionsNoCredentials, this.cMenTreeConnectWithOptionsChoosePanelBeforeConnecting});
			this.cMenTreeConnectWithOptions.Name = "cMenTreeConnectWithOptions";
			this.cMenTreeConnectWithOptions.Size = new System.Drawing.Size(186, 22);
			this.cMenTreeConnectWithOptions.Text = "Connect (with options)";
			//
			//cMenTreeConnectWithOptionsConnectToConsoleSession
			//
			this.cMenTreeConnectWithOptionsConnectToConsoleSession.Image = Resources.monitor_go;
			this.cMenTreeConnectWithOptionsConnectToConsoleSession.Name = "cMenTreeConnectWithOptionsConnectToConsoleSession";
			this.cMenTreeConnectWithOptionsConnectToConsoleSession.Size = new System.Drawing.Size(231, 22);
			this.cMenTreeConnectWithOptionsConnectToConsoleSession.Text = "Connect to console session";
			//
			//cMenTreeConnectWithOptionsDontConnectToConsoleSession
			//
			this.cMenTreeConnectWithOptionsDontConnectToConsoleSession.Image = Resources.monitor_delete;
			this.cMenTreeConnectWithOptionsDontConnectToConsoleSession.Name = "cMenTreeConnectWithOptionsDontConnectToConsoleSession";
			this.cMenTreeConnectWithOptionsDontConnectToConsoleSession.Size = new System.Drawing.Size(231, 22);
			this.cMenTreeConnectWithOptionsDontConnectToConsoleSession.Text = "Don\'t connect to console session";
			this.cMenTreeConnectWithOptionsDontConnectToConsoleSession.Visible = false;
			//
			//cMenTreeConnectWithOptionsConnectInFullscreen
			//
			this.cMenTreeConnectWithOptionsConnectInFullscreen.Image = Resources.arrow_out;
			this.cMenTreeConnectWithOptionsConnectInFullscreen.Name = "cMenTreeConnectWithOptionsConnectInFullscreen";
			this.cMenTreeConnectWithOptionsConnectInFullscreen.Size = new System.Drawing.Size(231, 22);
			this.cMenTreeConnectWithOptionsConnectInFullscreen.Text = "Connect in fullscreen";
			//
			//cMenTreeConnectWithOptionsNoCredentials
			//
			this.cMenTreeConnectWithOptionsNoCredentials.Image = Resources.key_delete;
			this.cMenTreeConnectWithOptionsNoCredentials.Name = "cMenTreeConnectWithOptionsNoCredentials";
			this.cMenTreeConnectWithOptionsNoCredentials.Size = new System.Drawing.Size(231, 22);
			this.cMenTreeConnectWithOptionsNoCredentials.Text = "Connect without credentials";
			//
			//cMenTreeConnectWithOptionsChoosePanelBeforeConnecting
			//
			this.cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Image = Resources.Panels;
			this.cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Name = "cMenTreeConnectWithOptionsChoosePanelBeforeConnecting";
			this.cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Size = new System.Drawing.Size(231, 22);
			this.cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Text = "Choose panel before connecting";
			//
			//cMenTreeDisconnect
			//
			this.cMenTreeDisconnect.Image = Resources.Pause;
			this.cMenTreeDisconnect.Name = "cMenTreeDisconnect";
			this.cMenTreeDisconnect.Size = new System.Drawing.Size(186, 22);
			this.cMenTreeDisconnect.Text = "Disconnect";
			//
			//cMenTreeSep1
			//
			this.cMenTreeSep1.Name = "cMenTreeSep1";
			this.cMenTreeSep1.Size = new System.Drawing.Size(183, 6);
			//
			//cMenTreeToolsExternalApps
			//
			this.cMenTreeToolsExternalApps.Image = Resources.ExtApp;
			this.cMenTreeToolsExternalApps.Name = "cMenTreeToolsExternalApps";
			this.cMenTreeToolsExternalApps.Size = new System.Drawing.Size(186, 22);
			this.cMenTreeToolsExternalApps.Text = "External Applications";
			//
			//cMenTreeToolsTransferFile
			//
			this.cMenTreeToolsTransferFile.Image = Resources.SSHTransfer;
			this.cMenTreeToolsTransferFile.Name = "cMenTreeToolsTransferFile";
			this.cMenTreeToolsTransferFile.Size = new System.Drawing.Size(186, 22);
			this.cMenTreeToolsTransferFile.Text = "Transfer File (SSH)";
			//
			//cMenTreeSep2
			//
			this.cMenTreeSep2.Name = "cMenTreeSep2";
			this.cMenTreeSep2.Size = new System.Drawing.Size(183, 6);
			//
			//cMenTreeDuplicate
			//
			this.cMenTreeDuplicate.Image = Resources.page_copy;
			this.cMenTreeDuplicate.Name = "cMenTreeDuplicate";
			this.cMenTreeDuplicate.ShortcutKeys = (System.Windows.Forms.Keys) (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D);
			this.cMenTreeDuplicate.Size = new System.Drawing.Size(186, 22);
			this.cMenTreeDuplicate.Text = "Duplicate";
			//
			//cMenTreeRename
			//
			this.cMenTreeRename.Image = Resources.Rename;
			this.cMenTreeRename.Name = "cMenTreeRename";
			this.cMenTreeRename.ShortcutKeys = System.Windows.Forms.Keys.F2;
			this.cMenTreeRename.Size = new System.Drawing.Size(186, 22);
			this.cMenTreeRename.Text = "Rename";
			//
			//cMenTreeDelete
			//
			this.cMenTreeDelete.Image = Resources.Delete;
			this.cMenTreeDelete.Name = "cMenTreeDelete";
			this.cMenTreeDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.cMenTreeDelete.Size = new System.Drawing.Size(186, 22);
			this.cMenTreeDelete.Text = "Delete";
			//
			//cMenTreeSep3
			//
			this.cMenTreeSep3.Name = "cMenTreeSep3";
			this.cMenTreeSep3.Size = new System.Drawing.Size(183, 6);
			//
			//cMenTreeImport
			//
			this.cMenTreeImport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {this.cMenTreeImportFile, this.cMenTreeImportActiveDirectory, this.cMenTreeImportPortScan});
			this.cMenTreeImport.Name = "cMenTreeImport";
			this.cMenTreeImport.Size = new System.Drawing.Size(186, 22);
			this.cMenTreeImport.Text = "&Import";
			//
			//cMenTreeImportFile
			//
			this.cMenTreeImportFile.Name = "cMenTreeImportFile";
			this.cMenTreeImportFile.Size = new System.Drawing.Size(213, 22);
			this.cMenTreeImportFile.Text = "Import from &File...";
			//
			//cMenTreeImportActiveDirectory
			//
			this.cMenTreeImportActiveDirectory.Name = "cMenTreeImportActiveDirectory";
			this.cMenTreeImportActiveDirectory.Size = new System.Drawing.Size(213, 22);
			this.cMenTreeImportActiveDirectory.Text = "Import from &Active Directory...";
			//
			//cMenTreeImportPortScan
			//
			this.cMenTreeImportPortScan.Name = "cMenTreeImportPortScan";
			this.cMenTreeImportPortScan.Size = new System.Drawing.Size(213, 22);
			this.cMenTreeImportPortScan.Text = "Import from &Port Scan...";
			//
			//cMenTreeExportFile
			//
			this.cMenTreeExportFile.Name = "cMenTreeExportFile";
			this.cMenTreeExportFile.Size = new System.Drawing.Size(186, 22);
			this.cMenTreeExportFile.Text = "&Export to File...";
			//
			//cMenTreeSep4
			//
			this.cMenTreeSep4.Name = "cMenTreeSep4";
			this.cMenTreeSep4.Size = new System.Drawing.Size(183, 6);
			//
			//cMenTreeAddConnection
			//
			this.cMenTreeAddConnection.Image = Resources.Connection_Add;
			this.cMenTreeAddConnection.Name = "cMenTreeAddConnection";
			this.cMenTreeAddConnection.Size = new System.Drawing.Size(186, 22);
			this.cMenTreeAddConnection.Text = "New Connection";
			//
			//cMenTreeAddFolder
			//
			this.cMenTreeAddFolder.Image = Resources.Folder_Add;
			this.cMenTreeAddFolder.Name = "cMenTreeAddFolder";
			this.cMenTreeAddFolder.Size = new System.Drawing.Size(186, 22);
			this.cMenTreeAddFolder.Text = "New Folder";
			//
			//ToolStripSeparator1
			//
			this.ToolStripSeparator1.Name = "ToolStripSeparator1";
			this.ToolStripSeparator1.Size = new System.Drawing.Size(183, 6);
			//
			//cMenTreeToolsSort
			//
			this.cMenTreeToolsSort.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {this.cMenTreeToolsSortAscending, this.cMenTreeToolsSortDescending});
			this.cMenTreeToolsSort.Name = "cMenTreeToolsSort";
			this.cMenTreeToolsSort.Size = new System.Drawing.Size(186, 22);
			this.cMenTreeToolsSort.Text = "Sort";
			//
			//cMenTreeToolsSortAscending
			//
			this.cMenTreeToolsSortAscending.Image = Resources.Sort_AZ;
			this.cMenTreeToolsSortAscending.Name = "cMenTreeToolsSortAscending";
			this.cMenTreeToolsSortAscending.Size = new System.Drawing.Size(157, 22);
			this.cMenTreeToolsSortAscending.Text = "Ascending (A-Z)";
			//
			//cMenTreeToolsSortDescending
			//
			this.cMenTreeToolsSortDescending.Image = Resources.Sort_ZA;
			this.cMenTreeToolsSortDescending.Name = "cMenTreeToolsSortDescending";
			this.cMenTreeToolsSortDescending.Size = new System.Drawing.Size(157, 22);
			this.cMenTreeToolsSortDescending.Text = "Descending (Z-A)";
			//
			//cMenTreeMoveUp
			//
			this.cMenTreeMoveUp.Image = Resources.Arrow_Up;
			this.cMenTreeMoveUp.Name = "cMenTreeMoveUp";
			this.cMenTreeMoveUp.ShortcutKeys = (System.Windows.Forms.Keys) (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Up);
			this.cMenTreeMoveUp.Size = new System.Drawing.Size(186, 22);
			this.cMenTreeMoveUp.Text = "Move up";
			//
			//cMenTreeMoveDown
			//
			this.cMenTreeMoveDown.Image = Resources.Arrow_Down;
			this.cMenTreeMoveDown.Name = "cMenTreeMoveDown";
			this.cMenTreeMoveDown.ShortcutKeys = (System.Windows.Forms.Keys) (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Down);
			this.cMenTreeMoveDown.Size = new System.Drawing.Size(186, 22);
			this.cMenTreeMoveDown.Text = "Move down";
			//
			//imgListTree
			//
			this.imgListTree.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imgListTree.ImageSize = new System.Drawing.Size(16, 16);
			this.imgListTree.TransparentColor = System.Drawing.Color.Transparent;
			//
			//pnlConnections
			//
			this.pnlConnections.Anchor = (System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.pnlConnections.Controls.Add(this.PictureBox1);
			this.pnlConnections.Controls.Add(this.txtSearch);
			this.pnlConnections.Controls.Add(this.tvConnections);
			this.pnlConnections.Location = new System.Drawing.Point(0, 25);
			this.pnlConnections.Name = "pnlConnections";
			this.pnlConnections.Size = new System.Drawing.Size(192, 428);
			this.pnlConnections.TabIndex = 9;
			//
			//PictureBox1
			//
			this.PictureBox1.Anchor = (System.Windows.Forms.AnchorStyles) (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.PictureBox1.Image = Resources.Search;
			this.PictureBox1.Location = new System.Drawing.Point(2, 412);
			this.PictureBox1.Name = "PictureBox1";
			this.PictureBox1.Size = new System.Drawing.Size(16, 16);
			this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.PictureBox1.TabIndex = 1;
			this.PictureBox1.TabStop = false;
			//
			//txtSearch
			//
			this.txtSearch.Anchor = (System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtSearch.ForeColor = System.Drawing.SystemColors.GrayText;
			this.txtSearch.Location = new System.Drawing.Point(19, 413);
			this.txtSearch.Name = "txtSearch";
			this.txtSearch.Size = new System.Drawing.Size(171, 13);
			this.txtSearch.TabIndex = 30;
			this.txtSearch.TabStop = false;
			this.txtSearch.Text = "Search";
			//
			//msMain
			//
			this.msMain.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, System.Convert.ToByte(0));
			this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this.mMenAddConnection, this.mMenAddFolder, this.mMenView, this.mMenSortAscending});
			this.msMain.Location = new System.Drawing.Point(0, 0);
			this.msMain.Name = "msMain";
			this.msMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.msMain.ShowItemToolTips = true;
			this.msMain.Size = new System.Drawing.Size(192, 24);
			this.msMain.TabIndex = 10;
			this.msMain.Text = "MenuStrip1";
			//
			//mMenAddConnection
			//
			this.mMenAddConnection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.mMenAddConnection.Image = Resources.Connection_Add;
			this.mMenAddConnection.Name = "mMenAddConnection";
			this.mMenAddConnection.Size = new System.Drawing.Size(28, 20);
			//
			//mMenAddFolder
			//
			this.mMenAddFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.mMenAddFolder.Image = Resources.Folder_Add;
			this.mMenAddFolder.Name = "mMenAddFolder";
			this.mMenAddFolder.Size = new System.Drawing.Size(28, 20);
			//
			//mMenView
			//
			this.mMenView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.mMenView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {this.mMenViewExpandAllFolders, this.mMenViewCollapseAllFolders});
			this.mMenView.Image = Resources.View;
			this.mMenView.Name = "mMenView";
			this.mMenView.Size = new System.Drawing.Size(28, 20);
			this.mMenView.Text = "&View";
			//
			//mMenViewExpandAllFolders
			//
			this.mMenViewExpandAllFolders.Image = Resources.Expand;
			this.mMenViewExpandAllFolders.Name = "mMenViewExpandAllFolders";
			this.mMenViewExpandAllFolders.Size = new System.Drawing.Size(161, 22);
			this.mMenViewExpandAllFolders.Text = "Expand all folders";
			//
			//mMenViewCollapseAllFolders
			//
			this.mMenViewCollapseAllFolders.Image = Resources.Collapse;
			this.mMenViewCollapseAllFolders.Name = "mMenViewCollapseAllFolders";
			this.mMenViewCollapseAllFolders.Size = new System.Drawing.Size(161, 22);
			this.mMenViewCollapseAllFolders.Text = "Collapse all folders";
			//
			//mMenSortAscending
			//
			this.mMenSortAscending.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.mMenSortAscending.Image = Resources.Sort_AZ;
			this.mMenSortAscending.Name = "mMenSortAscending";
			this.mMenSortAscending.Size = new System.Drawing.Size(28, 20);
			//
			//Tree
			//
			this.ClientSize = new System.Drawing.Size(192, 453);
			this.Controls.Add(this.msMain);
			this.Controls.Add(this.pnlConnections);
			this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, System.Convert.ToByte(0));
			this.HideOnClose = true;
			this.Icon = Resources.Root_Icon;
			this.Name = "Tree";
			this.TabText = "Connections";
			this.Text = "Connections";
			this.cMenTree.ResumeLayout(false);
			this.pnlConnections.ResumeLayout(false);
			this.pnlConnections.PerformLayout();
			((System.ComponentModel.ISupportInitialize) this.PictureBox1).EndInit();
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
	}
}
