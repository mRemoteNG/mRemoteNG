using mRemoteNG.UI.Controls;

namespace mRemoteNG.UI.Forms
{
    public partial class frmMain : System.Windows.Forms.Form
	{
		
		//Form overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && components != null)
				{
					components.Dispose();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}
		
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.pnlDock = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.msMain = new System.Windows.Forms.MenuStrip();
            this.mMenFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenFileNewConnection = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenFileNewFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenFileSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.mMenFileNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenFileLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenFileSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.mMenFileDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenFileRename = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenFileDuplicate = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenFileSep4 = new System.Windows.Forms.ToolStripSeparator();
            this.mMenReconnectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenFileSep3 = new System.Windows.Forms.ToolStripSeparator();
            this.mMenFileImport = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenFileImportFromFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenFileImportFromActiveDirectory = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenFileImportFromPortScan = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenFileExport = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenFileSep5 = new System.Windows.Forms.ToolStripSeparator();
            this.mMenFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenView = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenViewAddConnectionPanel = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenViewConnectionPanels = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenViewSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.mMenViewConnections = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenViewConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenViewErrorsAndInfos = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenViewScreenshotManager = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mMenViewJumpTo = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenViewJumpToConnectionsConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenViewJumpToErrorsInfos = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenViewResetLayout = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenViewSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.mMenViewQuickConnectToolbar = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenViewExtAppsToolbar = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenViewSep3 = new System.Windows.Forms.ToolStripSeparator();
            this.mMenViewFullscreen = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenTools = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenToolsSSHTransfer = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenToolsUVNCSC = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenToolsExternalApps = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenToolsPortScan = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenToolsSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.mMenToolsComponentsCheck = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenToolsOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenInfoHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenInfoSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.mMenInfoWebsite = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenInfoDonate = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenInfoForum = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenInfoBugReport = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mMenToolsUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenInfoSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.mMenInfoAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenSep3 = new System.Windows.Forms.ToolStripSeparator();
            this.lblQuickConnect = new System.Windows.Forms.ToolStripLabel();
            this.cmbQuickConnect = new mRemoteNG.UI.Controls.QuickConnectComboBox();
            this.tsContainer = new System.Windows.Forms.ToolStripContainer();
            this.tsQuickConnect = new System.Windows.Forms.ToolStrip();
            this.btnQuickConnect = new mRemoteNG.UI.Controls.ToolStripSplitButton();
            this.mnuQuickConnectProtocol = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnConnections = new System.Windows.Forms.ToolStripDropDownButton();
            this.mnuConnections = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsExternalTools = new System.Windows.Forms.ToolStrip();
            this.cMenExtAppsToolbar = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cMenToolbarShowText = new System.Windows.Forms.ToolStripMenuItem();
            this.tmrAutoSave = new System.Windows.Forms.Timer(this.components);
            this.msMain.SuspendLayout();
            this.tsContainer.ContentPanel.SuspendLayout();
            this.tsContainer.TopToolStripPanel.SuspendLayout();
            this.tsContainer.SuspendLayout();
            this.tsQuickConnect.SuspendLayout();
            this.cMenExtAppsToolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDock
            // 
            this.pnlDock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDock.DockBackColor = System.Drawing.SystemColors.Control;
            this.pnlDock.DockLeftPortion = 230D;
            this.pnlDock.DockRightPortion = 230D;
            this.pnlDock.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingSdi;
            this.pnlDock.Location = new System.Drawing.Point(0, 0);
            this.pnlDock.Name = "pnlDock";
            this.pnlDock.Size = new System.Drawing.Size(966, 498);
            this.pnlDock.TabIndex = 13;
            this.pnlDock.ActiveDocumentChanged += new System.EventHandler(this.pnlDock_ActiveDocumentChanged);
            // 
            // msMain
            // 
            this.msMain.Dock = System.Windows.Forms.DockStyle.None;
            this.msMain.GripMargin = new System.Windows.Forms.Padding(0);
            this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mMenFile,
            this.mMenView,
            this.mMenTools,
            this.mMenInfo});
            this.msMain.Location = new System.Drawing.Point(3, 0);
            this.msMain.Name = "msMain";
            this.msMain.Padding = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.msMain.Size = new System.Drawing.Size(177, 24);
            this.msMain.Stretch = false;
            this.msMain.TabIndex = 16;
            this.msMain.Text = "Main Toolbar";
            // 
            // mMenFile
            // 
            this.mMenFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mMenFileNewConnection,
            this.mMenFileNewFolder,
            this.mMenFileSep1,
            this.mMenFileNew,
            this.mMenFileLoad,
            this.mMenFileSave,
            this.mMenFileSaveAs,
            this.mMenFileSep2,
            this.mMenFileDelete,
            this.mMenFileRename,
            this.mMenFileDuplicate,
            this.mMenFileSep4,
            this.mMenReconnectAll,
            this.mMenFileSep3,
            this.mMenFileImport,
            this.mMenFileExport,
            this.mMenFileSep5,
            this.mMenFileExit});
            this.mMenFile.Name = "mMenFile";
            this.mMenFile.Size = new System.Drawing.Size(37, 20);
            this.mMenFile.Text = "&File";
            this.mMenFile.DropDownOpening += new System.EventHandler(this.mMenFile_DropDownOpening);
            // 
            // mMenFileNewConnection
            // 
            this.mMenFileNewConnection.Image = global::mRemoteNG.Resources.Connection_Add;
            this.mMenFileNewConnection.Name = "mMenFileNewConnection";
            this.mMenFileNewConnection.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.mMenFileNewConnection.Size = new System.Drawing.Size(281, 22);
            this.mMenFileNewConnection.Text = "New Connection";
            this.mMenFileNewConnection.Click += new System.EventHandler(this.mMenFileNewConnection_Click);
            // 
            // mMenFileNewFolder
            // 
            this.mMenFileNewFolder.Image = global::mRemoteNG.Resources.Folder_Add;
            this.mMenFileNewFolder.Name = "mMenFileNewFolder";
            this.mMenFileNewFolder.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.N)));
            this.mMenFileNewFolder.Size = new System.Drawing.Size(281, 22);
            this.mMenFileNewFolder.Text = "New Folder";
            this.mMenFileNewFolder.Click += new System.EventHandler(this.mMenFileNewFolder_Click);
            // 
            // mMenFileSep1
            // 
            this.mMenFileSep1.Name = "mMenFileSep1";
            this.mMenFileSep1.Size = new System.Drawing.Size(278, 6);
            // 
            // mMenFileNew
            // 
            this.mMenFileNew.Image = global::mRemoteNG.Resources.Connections_New;
            this.mMenFileNew.Name = "mMenFileNew";
            this.mMenFileNew.Size = new System.Drawing.Size(281, 22);
            this.mMenFileNew.Text = "New Connection File";
            this.mMenFileNew.Click += new System.EventHandler(this.mMenFileNew_Click);
            // 
            // mMenFileLoad
            // 
            this.mMenFileLoad.Image = global::mRemoteNG.Resources.Connections_Load;
            this.mMenFileLoad.Name = "mMenFileLoad";
            this.mMenFileLoad.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.mMenFileLoad.Size = new System.Drawing.Size(281, 22);
            this.mMenFileLoad.Text = "Open Connection File...";
            this.mMenFileLoad.Click += new System.EventHandler(this.mMenFileLoad_Click);
            // 
            // mMenFileSave
            // 
            this.mMenFileSave.Image = global::mRemoteNG.Resources.Connections_Save;
            this.mMenFileSave.Name = "mMenFileSave";
            this.mMenFileSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.mMenFileSave.Size = new System.Drawing.Size(281, 22);
            this.mMenFileSave.Text = "Save Connection File";
            this.mMenFileSave.Click += new System.EventHandler(this.mMenFileSave_Click);
            // 
            // mMenFileSaveAs
            // 
            this.mMenFileSaveAs.Image = global::mRemoteNG.Resources.Connections_SaveAs;
            this.mMenFileSaveAs.Name = "mMenFileSaveAs";
            this.mMenFileSaveAs.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.mMenFileSaveAs.Size = new System.Drawing.Size(281, 22);
            this.mMenFileSaveAs.Text = "Save Connection File As...";
            this.mMenFileSaveAs.Click += new System.EventHandler(this.mMenFileSaveAs_Click);
            // 
            // mMenFileSep2
            // 
            this.mMenFileSep2.Name = "mMenFileSep2";
            this.mMenFileSep2.Size = new System.Drawing.Size(278, 6);
            // 
            // mMenFileDelete
            // 
            this.mMenFileDelete.Image = global::mRemoteNG.Resources.Delete;
            this.mMenFileDelete.Name = "mMenFileDelete";
            this.mMenFileDelete.Size = new System.Drawing.Size(281, 22);
            this.mMenFileDelete.Text = "Delete...";
            this.mMenFileDelete.Click += new System.EventHandler(this.mMenFileDelete_Click);
            // 
            // mMenFileRename
            // 
            this.mMenFileRename.Image = global::mRemoteNG.Resources.Rename;
            this.mMenFileRename.Name = "mMenFileRename";
            this.mMenFileRename.Size = new System.Drawing.Size(281, 22);
            this.mMenFileRename.Text = "Rename";
            this.mMenFileRename.Click += new System.EventHandler(this.mMenFileRename_Click);
            // 
            // mMenFileDuplicate
            // 
            this.mMenFileDuplicate.Image = global::mRemoteNG.Resources.page_copy;
            this.mMenFileDuplicate.Name = "mMenFileDuplicate";
            this.mMenFileDuplicate.Size = new System.Drawing.Size(281, 22);
            this.mMenFileDuplicate.Text = "Duplicate";
            this.mMenFileDuplicate.Click += new System.EventHandler(this.mMenFileDuplicate_Click);
            // 
            // mMenFileSep4
            // 
            this.mMenFileSep4.Name = "mMenFileSep4";
            this.mMenFileSep4.Size = new System.Drawing.Size(278, 6);
            // 
            // mMenReconnectAll
            // 
            this.mMenReconnectAll.Image = global::mRemoteNG.Resources.Refresh;
            this.mMenReconnectAll.Name = "mMenReconnectAll";
            this.mMenReconnectAll.Size = new System.Drawing.Size(281, 22);
            this.mMenReconnectAll.Text = "Reconnect All Connections";
            this.mMenReconnectAll.Click += new System.EventHandler(this.mMenReconnectAll_Click);
            // 
            // mMenFileSep3
            // 
            this.mMenFileSep3.Name = "mMenFileSep3";
            this.mMenFileSep3.Size = new System.Drawing.Size(278, 6);
            // 
            // mMenFileImport
            // 
            this.mMenFileImport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mMenFileImportFromFile,
            this.mMenFileImportFromActiveDirectory,
            this.mMenFileImportFromPortScan});
            this.mMenFileImport.Name = "mMenFileImport";
            this.mMenFileImport.Size = new System.Drawing.Size(281, 22);
            this.mMenFileImport.Text = "&Import";
            // 
            // mMenFileImportFromFile
            // 
            this.mMenFileImportFromFile.Name = "mMenFileImportFromFile";
            this.mMenFileImportFromFile.Size = new System.Drawing.Size(235, 22);
            this.mMenFileImportFromFile.Text = "Import from &File...";
            this.mMenFileImportFromFile.Click += new System.EventHandler(this.mMenFileImportFromFile_Click);
            // 
            // mMenFileImportFromActiveDirectory
            // 
            this.mMenFileImportFromActiveDirectory.Name = "mMenFileImportFromActiveDirectory";
            this.mMenFileImportFromActiveDirectory.Size = new System.Drawing.Size(235, 22);
            this.mMenFileImportFromActiveDirectory.Text = "Import from &Active Directory...";
            this.mMenFileImportFromActiveDirectory.Click += new System.EventHandler(this.mMenFileImportFromActiveDirectory_Click);
            // 
            // mMenFileImportFromPortScan
            // 
            this.mMenFileImportFromPortScan.Name = "mMenFileImportFromPortScan";
            this.mMenFileImportFromPortScan.Size = new System.Drawing.Size(235, 22);
            this.mMenFileImportFromPortScan.Text = "Import from &Port Scan...";
            this.mMenFileImportFromPortScan.Click += new System.EventHandler(this.mMenFileImportFromPortScan_Click);
            // 
            // mMenFileExport
            // 
            this.mMenFileExport.Name = "mMenFileExport";
            this.mMenFileExport.Size = new System.Drawing.Size(281, 22);
            this.mMenFileExport.Text = "&Export to File...";
            this.mMenFileExport.Click += new System.EventHandler(this.mMenFileExport_Click);
            // 
            // mMenFileSep5
            // 
            this.mMenFileSep5.Name = "mMenFileSep5";
            this.mMenFileSep5.Size = new System.Drawing.Size(278, 6);
            // 
            // mMenFileExit
            // 
            this.mMenFileExit.Image = global::mRemoteNG.Resources.Quit;
            this.mMenFileExit.Name = "mMenFileExit";
            this.mMenFileExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.mMenFileExit.Size = new System.Drawing.Size(281, 22);
            this.mMenFileExit.Text = "Exit";
            this.mMenFileExit.Click += new System.EventHandler(this.mMenFileExit_Click);
            // 
            // mMenView
            // 
            this.mMenView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mMenViewAddConnectionPanel,
            this.mMenViewConnectionPanels,
            this.mMenViewSep1,
            this.mMenViewConnections,
            this.mMenViewConfig,
            this.mMenViewErrorsAndInfos,
            this.mMenViewScreenshotManager,
            this.ToolStripSeparator1,
            this.mMenViewJumpTo,
            this.mMenViewResetLayout,
            this.mMenViewSep2,
            this.mMenViewQuickConnectToolbar,
            this.mMenViewExtAppsToolbar,
            this.mMenViewSep3,
            this.mMenViewFullscreen});
            this.mMenView.Name = "mMenView";
            this.mMenView.Size = new System.Drawing.Size(44, 20);
            this.mMenView.Text = "&View";
            this.mMenView.DropDownOpening += new System.EventHandler(this.mMenView_DropDownOpening);
            // 
            // mMenViewAddConnectionPanel
            // 
            this.mMenViewAddConnectionPanel.Image = global::mRemoteNG.Resources.Panel_Add;
            this.mMenViewAddConnectionPanel.Name = "mMenViewAddConnectionPanel";
            this.mMenViewAddConnectionPanel.Size = new System.Drawing.Size(228, 22);
            this.mMenViewAddConnectionPanel.Text = "Add Connection Panel";
            this.mMenViewAddConnectionPanel.Click += new System.EventHandler(this.mMenViewAddConnectionPanel_Click);
            // 
            // mMenViewConnectionPanels
            // 
            this.mMenViewConnectionPanels.Image = global::mRemoteNG.Resources.Panels;
            this.mMenViewConnectionPanels.Name = "mMenViewConnectionPanels";
            this.mMenViewConnectionPanels.Size = new System.Drawing.Size(228, 22);
            this.mMenViewConnectionPanels.Text = "Connection Panels";
            // 
            // mMenViewSep1
            // 
            this.mMenViewSep1.Name = "mMenViewSep1";
            this.mMenViewSep1.Size = new System.Drawing.Size(225, 6);
            // 
            // mMenViewConnections
            // 
            this.mMenViewConnections.Checked = true;
            this.mMenViewConnections.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mMenViewConnections.Image = global::mRemoteNG.Resources.Root;
            this.mMenViewConnections.Name = "mMenViewConnections";
            this.mMenViewConnections.Size = new System.Drawing.Size(228, 22);
            this.mMenViewConnections.Text = "Connections";
            this.mMenViewConnections.Click += new System.EventHandler(this.mMenViewConnections_Click);
            // 
            // mMenViewConfig
            // 
            this.mMenViewConfig.Checked = true;
            this.mMenViewConfig.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mMenViewConfig.Image = global::mRemoteNG.Resources.cog;
            this.mMenViewConfig.Name = "mMenViewConfig";
            this.mMenViewConfig.Size = new System.Drawing.Size(228, 22);
            this.mMenViewConfig.Text = "Config";
            this.mMenViewConfig.Click += new System.EventHandler(this.mMenViewConfig_Click);
            // 
            // mMenViewErrorsAndInfos
            // 
            this.mMenViewErrorsAndInfos.Checked = true;
            this.mMenViewErrorsAndInfos.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mMenViewErrorsAndInfos.Image = global::mRemoteNG.Resources.ErrorsAndInfos;
            this.mMenViewErrorsAndInfos.Name = "mMenViewErrorsAndInfos";
            this.mMenViewErrorsAndInfos.Size = new System.Drawing.Size(228, 22);
            this.mMenViewErrorsAndInfos.Text = "Errors and Infos";
            this.mMenViewErrorsAndInfos.Click += new System.EventHandler(this.mMenViewErrorsAndInfos_Click);
            // 
            // mMenViewScreenshotManager
            // 
		    this.mMenViewScreenshotManager.Image = global::mRemoteNG.Resources.Screenshot;
            this.mMenViewScreenshotManager.Name = "mMenViewScreenshotManager";
            this.mMenViewScreenshotManager.Size = new System.Drawing.Size(228, 22);
            this.mMenViewScreenshotManager.Text = "Screenshot Manager";
            this.mMenViewScreenshotManager.Click += new System.EventHandler(this.mMenViewScreenshotManager_Click);
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(225, 6);
            // 
            // mMenViewJumpTo
            // 
            this.mMenViewJumpTo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mMenViewJumpToConnectionsConfig,
            this.mMenViewJumpToErrorsInfos});
            this.mMenViewJumpTo.Image = global::mRemoteNG.Resources.JumpTo;
            this.mMenViewJumpTo.Name = "mMenViewJumpTo";
            this.mMenViewJumpTo.Size = new System.Drawing.Size(228, 22);
            this.mMenViewJumpTo.Text = "Jump To";
            // 
            // mMenViewJumpToConnectionsConfig
            // 
            this.mMenViewJumpToConnectionsConfig.Image = global::mRemoteNG.Resources.Root;
            this.mMenViewJumpToConnectionsConfig.Name = "mMenViewJumpToConnectionsConfig";
            this.mMenViewJumpToConnectionsConfig.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.C)));
            this.mMenViewJumpToConnectionsConfig.Size = new System.Drawing.Size(258, 22);
            this.mMenViewJumpToConnectionsConfig.Text = "Connections && Config";
            this.mMenViewJumpToConnectionsConfig.Click += new System.EventHandler(this.mMenViewJumpToConnectionsConfig_Click);
            // 
            // mMenViewJumpToErrorsInfos
            // 
            this.mMenViewJumpToErrorsInfos.Image = global::mRemoteNG.Resources.InformationSmall;
            this.mMenViewJumpToErrorsInfos.Name = "mMenViewJumpToErrorsInfos";
            this.mMenViewJumpToErrorsInfos.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.E)));
            this.mMenViewJumpToErrorsInfos.Size = new System.Drawing.Size(258, 22);
            this.mMenViewJumpToErrorsInfos.Text = "Errors && Infos";
            this.mMenViewJumpToErrorsInfos.Click += new System.EventHandler(this.mMenViewJumpToErrorsInfos_Click);
            // 
            // mMenViewResetLayout
            // 
            this.mMenViewResetLayout.Image = global::mRemoteNG.Resources.application_side_tree;
            this.mMenViewResetLayout.Name = "mMenViewResetLayout";
            this.mMenViewResetLayout.Size = new System.Drawing.Size(228, 22);
            this.mMenViewResetLayout.Text = "Reset Layout";
            this.mMenViewResetLayout.Click += new System.EventHandler(this.mMenViewResetLayout_Click);
            // 
            // mMenViewSep2
            // 
            this.mMenViewSep2.Name = "mMenViewSep2";
            this.mMenViewSep2.Size = new System.Drawing.Size(225, 6);
            // 
            // mMenViewQuickConnectToolbar
            // 
            this.mMenViewQuickConnectToolbar.Image = global::mRemoteNG.Resources.Play_Quick;
            this.mMenViewQuickConnectToolbar.Name = "mMenViewQuickConnectToolbar";
            this.mMenViewQuickConnectToolbar.Size = new System.Drawing.Size(228, 22);
            this.mMenViewQuickConnectToolbar.Text = "Quick Connect Toolbar";
            this.mMenViewQuickConnectToolbar.Click += new System.EventHandler(this.mMenViewQuickConnectToolbar_Click);
            // 
            // mMenViewExtAppsToolbar
            // 
            this.mMenViewExtAppsToolbar.Image = global::mRemoteNG.Resources.ExtApp;
            this.mMenViewExtAppsToolbar.Name = "mMenViewExtAppsToolbar";
            this.mMenViewExtAppsToolbar.Size = new System.Drawing.Size(228, 22);
            this.mMenViewExtAppsToolbar.Text = "External Applications Toolbar";
            this.mMenViewExtAppsToolbar.Click += new System.EventHandler(this.mMenViewExtAppsToolbar_Click);
            // 
            // mMenViewSep3
            // 
            this.mMenViewSep3.Name = "mMenViewSep3";
            this.mMenViewSep3.Size = new System.Drawing.Size(225, 6);
            // 
            // mMenViewFullscreen
            // 
            this.mMenViewFullscreen.Image = global::mRemoteNG.Resources.arrow_out;
            this.mMenViewFullscreen.Name = "mMenViewFullscreen";
            this.mMenViewFullscreen.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.mMenViewFullscreen.Size = new System.Drawing.Size(228, 22);
            this.mMenViewFullscreen.Text = "Full Screen";
            this.mMenViewFullscreen.Click += new System.EventHandler(this.mMenViewFullscreen_Click);
            // 
            // mMenTools
            // 
            this.mMenTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mMenToolsSSHTransfer,
            this.mMenToolsUVNCSC,
            this.mMenToolsExternalApps,
            this.mMenToolsPortScan,
            this.mMenToolsSep1,
            this.mMenToolsComponentsCheck,
            this.mMenToolsOptions});
            this.mMenTools.Name = "mMenTools";
            this.mMenTools.Size = new System.Drawing.Size(48, 20);
            this.mMenTools.Text = "&Tools";
            // 
            // mMenToolsSSHTransfer
            // 
            this.mMenToolsSSHTransfer.Image = global::mRemoteNG.Resources.SSHTransfer;
            this.mMenToolsSSHTransfer.Name = "mMenToolsSSHTransfer";
            this.mMenToolsSSHTransfer.Size = new System.Drawing.Size(184, 22);
            this.mMenToolsSSHTransfer.Text = "SSH File Transfer";
            this.mMenToolsSSHTransfer.Click += new System.EventHandler(this.mMenToolsSSHTransfer_Click);
            // 
            // mMenToolsUVNCSC
            // 
            this.mMenToolsUVNCSC.Image = global::mRemoteNG.Resources.UVNC_SC;
            this.mMenToolsUVNCSC.Name = "mMenToolsUVNCSC";
            this.mMenToolsUVNCSC.Size = new System.Drawing.Size(184, 22);
            this.mMenToolsUVNCSC.Text = "UltraVNC SingleClick";
            this.mMenToolsUVNCSC.Visible = false;
            this.mMenToolsUVNCSC.Click += new System.EventHandler(this.mMenToolsUVNCSC_Click);
            // 
            // mMenToolsExternalApps
            // 
            this.mMenToolsExternalApps.Image = global::mRemoteNG.Resources.ExtApp;
            this.mMenToolsExternalApps.Name = "mMenToolsExternalApps";
            this.mMenToolsExternalApps.Size = new System.Drawing.Size(184, 22);
            this.mMenToolsExternalApps.Text = "External Applications";
            this.mMenToolsExternalApps.Click += new System.EventHandler(this.mMenToolsExternalApps_Click);
            // 
            // mMenToolsPortScan
            // 
            this.mMenToolsPortScan.Image = global::mRemoteNG.Resources.PortScan;
            this.mMenToolsPortScan.Name = "mMenToolsPortScan";
            this.mMenToolsPortScan.Size = new System.Drawing.Size(184, 22);
            this.mMenToolsPortScan.Text = "Port Scan";
            this.mMenToolsPortScan.Click += new System.EventHandler(this.mMenToolsPortScan_Click);
            // 
            // mMenToolsSep1
            // 
            this.mMenToolsSep1.Name = "mMenToolsSep1";
            this.mMenToolsSep1.Size = new System.Drawing.Size(181, 6);
            // 
            // mMenToolsComponentsCheck
            // 
            this.mMenToolsComponentsCheck.Image = global::mRemoteNG.Resources.cog_error;
            this.mMenToolsComponentsCheck.Name = "mMenToolsComponentsCheck";
            this.mMenToolsComponentsCheck.Size = new System.Drawing.Size(184, 22);
            this.mMenToolsComponentsCheck.Text = "Components Check";
            this.mMenToolsComponentsCheck.Click += new System.EventHandler(this.mMenToolsComponentsCheck_Click);
            // 
            // mMenToolsOptions
            // 
            this.mMenToolsOptions.Image = global::mRemoteNG.Resources.Options;
            this.mMenToolsOptions.Name = "mMenToolsOptions";
            this.mMenToolsOptions.Size = new System.Drawing.Size(184, 22);
            this.mMenToolsOptions.Text = "Options";
            this.mMenToolsOptions.Click += new System.EventHandler(this.mMenToolsOptions_Click);
            // 
            // mMenInfo
            // 
            this.mMenInfo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mMenInfoHelp,
            this.mMenInfoSep1,
            this.mMenInfoWebsite,
            this.mMenInfoDonate,
            this.mMenInfoForum,
            this.mMenInfoBugReport,
            this.ToolStripSeparator2,
            this.mMenToolsUpdate,
            this.mMenInfoSep2,
            this.mMenInfoAbout});
            this.mMenInfo.Name = "mMenInfo";
            this.mMenInfo.Size = new System.Drawing.Size(44, 20);
            this.mMenInfo.Text = "&Help";
            this.mMenInfo.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            // 
            // mMenInfoHelp
            // 
		    this.mMenInfoHelp.Image = global::mRemoteNG.Resources.Help;
            this.mMenInfoHelp.Name = "mMenInfoHelp";
            this.mMenInfoHelp.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.mMenInfoHelp.Size = new System.Drawing.Size(190, 22);
            this.mMenInfoHelp.Text = "mRemoteNG Help";
            this.mMenInfoHelp.Click += new System.EventHandler(this.mMenInfoHelp_Click);
            // 
            // mMenInfoSep1
            // 
            this.mMenInfoSep1.Name = "mMenInfoSep1";
            this.mMenInfoSep1.Size = new System.Drawing.Size(187, 6);
            // 
            // mMenInfoWebsite
            // 
		    this.mMenInfoWebsite.Image = global::mRemoteNG.Resources.Website;
            this.mMenInfoWebsite.Name = "mMenInfoWebsite";
            this.mMenInfoWebsite.Size = new System.Drawing.Size(190, 22);
            this.mMenInfoWebsite.Text = "Website";
            this.mMenInfoWebsite.Click += new System.EventHandler(this.mMenInfoWebsite_Click);
            // 
            // mMenInfoDonate
            // 
            this.mMenInfoDonate.Image = global::mRemoteNG.Resources.Donate;
            this.mMenInfoDonate.Name = "mMenInfoDonate";
            this.mMenInfoDonate.Size = new System.Drawing.Size(190, 22);
            this.mMenInfoDonate.Text = "Donate";
            this.mMenInfoDonate.Click += new System.EventHandler(this.mMenInfoDonate_Click);
            // 
            // mMenInfoForum
            // 
            this.mMenInfoForum.Image = global::mRemoteNG.Resources.user_comment;
            this.mMenInfoForum.Name = "mMenInfoForum";
            this.mMenInfoForum.Size = new System.Drawing.Size(190, 22);
            this.mMenInfoForum.Text = "Support Forum";
            this.mMenInfoForum.Click += new System.EventHandler(this.mMenInfoForum_Click);
            // 
            // mMenInfoBugReport
            // 
            this.mMenInfoBugReport.Image = global::mRemoteNG.Resources.Bug;
            this.mMenInfoBugReport.Name = "mMenInfoBugReport";
            this.mMenInfoBugReport.Size = new System.Drawing.Size(190, 22);
            this.mMenInfoBugReport.Text = "Report a Bug";
            this.mMenInfoBugReport.Click += new System.EventHandler(this.mMenInfoBugReport_Click);
            // 
            // ToolStripSeparator2
            // 
            this.ToolStripSeparator2.Name = "ToolStripSeparator2";
            this.ToolStripSeparator2.Size = new System.Drawing.Size(187, 6);
            // 
            // mMenToolsUpdate
            // 
            this.mMenToolsUpdate.Image = global::mRemoteNG.Resources.Update;
            this.mMenToolsUpdate.Name = "mMenToolsUpdate";
            this.mMenToolsUpdate.Size = new System.Drawing.Size(190, 22);
            this.mMenToolsUpdate.Text = "Check for Updates";
            this.mMenToolsUpdate.Click += new System.EventHandler(this.mMenToolsUpdate_Click);
            // 
            // mMenInfoSep2
            // 
            this.mMenInfoSep2.Name = "mMenInfoSep2";
            this.mMenInfoSep2.Size = new System.Drawing.Size(187, 6);
            // 
            // mMenInfoAbout
            // 
            this.mMenInfoAbout.Image = global::mRemoteNG.Resources.mRemote;
            this.mMenInfoAbout.Name = "mMenInfoAbout";
            this.mMenInfoAbout.Size = new System.Drawing.Size(190, 22);
            this.mMenInfoAbout.Text = "About mRemoteNG";
            this.mMenInfoAbout.Click += new System.EventHandler(this.mMenInfoAbout_Click);
            // 
            // mMenSep3
            // 
            this.mMenSep3.Name = "mMenSep3";
            this.mMenSep3.Size = new System.Drawing.Size(211, 6);
            // 
            // lblQuickConnect
            // 
            this.lblQuickConnect.Name = "lblQuickConnect";
            this.lblQuickConnect.Size = new System.Drawing.Size(55, 22);
            this.lblQuickConnect.Text = "&Connect:";
            this.lblQuickConnect.Click += new System.EventHandler(this.lblQuickConnect_Click);
            // 
            // cmbQuickConnect
            // 
            this.cmbQuickConnect.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbQuickConnect.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbQuickConnect.Margin = new System.Windows.Forms.Padding(1, 0, 3, 0);
            this.cmbQuickConnect.Name = "cmbQuickConnect";
            this.cmbQuickConnect.Size = new System.Drawing.Size(200, 25);
            this.cmbQuickConnect.ConnectRequested += new mRemoteNG.UI.Controls.QuickConnectComboBox.ConnectRequestedEventHandler(this.cmbQuickConnect_ConnectRequested);
            this.cmbQuickConnect.ProtocolChanged += new mRemoteNG.UI.Controls.QuickConnectComboBox.ProtocolChangedEventHandler(this.cmbQuickConnect_ProtocolChanged);
            // 
            // tsContainer
            // 
            // 
            // tsContainer.ContentPanel
            // 
            this.tsContainer.ContentPanel.Controls.Add(this.pnlDock);
            this.tsContainer.ContentPanel.Size = new System.Drawing.Size(966, 498);
            this.tsContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tsContainer.Location = new System.Drawing.Point(0, 0);
            this.tsContainer.Name = "tsContainer";
            this.tsContainer.Size = new System.Drawing.Size(966, 523);
            this.tsContainer.TabIndex = 17;
            this.tsContainer.Text = "ToolStripContainer1";
            // 
            // tsContainer.TopToolStripPanel
            // 
            this.tsContainer.TopToolStripPanel.Controls.Add(this.msMain);
            this.tsContainer.TopToolStripPanel.Controls.Add(this.tsQuickConnect);
            this.tsContainer.TopToolStripPanel.Controls.Add(this.tsExternalTools);
            // 
            // tsQuickConnect
            // 
            this.tsQuickConnect.Dock = System.Windows.Forms.DockStyle.None;
            this.tsQuickConnect.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblQuickConnect,
            this.cmbQuickConnect,
            this.btnQuickConnect,
            this.btnConnections});
            this.tsQuickConnect.Location = new System.Drawing.Point(msMain.Location.X + msMain.Width + 1, 0);
            this.tsQuickConnect.MaximumSize = new System.Drawing.Size(0, 25);
            this.tsQuickConnect.Name = "tsQuickConnect";
            this.tsQuickConnect.Size = new System.Drawing.Size(387, 25);
            this.tsQuickConnect.TabIndex = 18;
            // 
            // btnQuickConnect
            // 
            this.btnQuickConnect.DropDown = this.mnuQuickConnectProtocol;
            this.btnQuickConnect.Image = global::mRemoteNG.Resources.Play_Quick;
            this.btnQuickConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnQuickConnect.Margin = new System.Windows.Forms.Padding(0, 1, 3, 2);
            this.btnQuickConnect.Name = "btnQuickConnect";
            this.btnQuickConnect.Size = new System.Drawing.Size(84, 22);
            this.btnQuickConnect.Text = "Connect";
            this.btnQuickConnect.ButtonClick += new System.EventHandler(this.btnQuickConnect_ButtonClick);
            this.btnQuickConnect.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.btnQuickConnect_DropDownItemClicked);
            // 
            // mnuQuickConnectProtocol
            // 
            this.mnuQuickConnectProtocol.Name = "mnuQuickConnectProtocol";
            this.mnuQuickConnectProtocol.OwnerItem = this.btnQuickConnect;
            this.mnuQuickConnectProtocol.ShowCheckMargin = true;
            this.mnuQuickConnectProtocol.ShowImageMargin = false;
            this.mnuQuickConnectProtocol.Size = new System.Drawing.Size(61, 4);
            // 
            // btnConnections
            // 
            this.btnConnections.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnConnections.DropDown = this.mnuConnections;
            this.btnConnections.Image = global::mRemoteNG.Resources.Root;
            this.btnConnections.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnConnections.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConnections.Name = "btnConnections";
            this.btnConnections.Size = new System.Drawing.Size(29, 22);
            this.btnConnections.Text = "Connections";
            this.btnConnections.DropDownOpening += new System.EventHandler(this.btnConnections_DropDownOpening);
            // 
            // mnuConnections
            // 
            this.mnuConnections.Name = "mnuConnections";
            this.mnuConnections.OwnerItem = this.btnConnections;
            this.mnuConnections.Size = new System.Drawing.Size(61, 4);
            // 
            // tsExternalTools
            // 
            this.tsExternalTools.ContextMenuStrip = this.cMenExtAppsToolbar;
            this.tsExternalTools.Dock = System.Windows.Forms.DockStyle.None;
            this.tsExternalTools.Location = new System.Drawing.Point(tsQuickConnect.Location.X + tsQuickConnect.Width + 1, 0);
            this.tsExternalTools.MaximumSize = new System.Drawing.Size(0, 25);
            this.tsExternalTools.Name = "tsExternalTools";
            this.tsExternalTools.Size = new System.Drawing.Size(111, 25);
            this.tsExternalTools.TabIndex = 17;
            // 
            // cMenExtAppsToolbar
            // 
            this.cMenExtAppsToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cMenToolbarShowText});
            this.cMenExtAppsToolbar.Name = "cMenToolbar";
            this.cMenExtAppsToolbar.Size = new System.Drawing.Size(129, 26);
            // 
            // cMenToolbarShowText
            // 
            this.cMenToolbarShowText.Checked = true;
            this.cMenToolbarShowText.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cMenToolbarShowText.Name = "cMenToolbarShowText";
            this.cMenToolbarShowText.Size = new System.Drawing.Size(128, 22);
            this.cMenToolbarShowText.Text = "Show Text";
            this.cMenToolbarShowText.Click += new System.EventHandler(this.cMenToolbarShowText_Click);
            // 
            // tmrAutoSave
            // 
            this.tmrAutoSave.Interval = 10000;
            this.tmrAutoSave.Tick += new System.EventHandler(this.tmrAutoSave_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(966, 523);
            this.Controls.Add(this.tsContainer);
            this.Icon = global::mRemoteNG.Resources.mRemote_Icon;
            this.MainMenuStrip = this.msMain;
            this.Name = "frmMain";
            this.Opacity = 0D;
            this.Text = "mRemoteNG";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            this.ResizeBegin += new System.EventHandler(this.frmMain_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.frmMain_ResizeEnd);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.msMain.ResumeLayout(false);
            this.msMain.PerformLayout();
            this.tsContainer.ContentPanel.ResumeLayout(false);
            this.tsContainer.TopToolStripPanel.ResumeLayout(false);
            this.tsContainer.TopToolStripPanel.PerformLayout();
            this.tsContainer.ResumeLayout(false);
            this.tsContainer.PerformLayout();
            this.tsQuickConnect.ResumeLayout(false);
            this.tsQuickConnect.PerformLayout();
            this.cMenExtAppsToolbar.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		internal WeifenLuo.WinFormsUI.Docking.DockPanel pnlDock;
		internal System.Windows.Forms.MenuStrip msMain;
		internal System.Windows.Forms.ToolStripMenuItem mMenFile;
		internal System.Windows.Forms.ToolStripMenuItem mMenView;
		internal System.Windows.Forms.ToolStripMenuItem mMenTools;
		internal System.Windows.Forms.ToolStripLabel lblQuickConnect;
		internal System.Windows.Forms.ToolStripMenuItem mMenInfo;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileNew;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileLoad;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileSave;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileSaveAs;
		internal System.Windows.Forms.ToolStripSeparator mMenFileSep1;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileExit;
		internal System.Windows.Forms.ToolStripSeparator mMenToolsSep1;
		internal System.Windows.Forms.ToolStripMenuItem mMenToolsOptions;
		internal System.Windows.Forms.ToolStripMenuItem mMenInfoHelp;
		internal System.Windows.Forms.ToolStripMenuItem mMenInfoWebsite;
		internal System.Windows.Forms.ToolStripSeparator mMenInfoSep1;
		internal System.Windows.Forms.ToolStripMenuItem mMenInfoAbout;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewConnectionPanels;
		internal System.Windows.Forms.ToolStripSeparator mMenViewSep1;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewConnections;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewConfig;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewErrorsAndInfos;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewScreenshotManager;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewAddConnectionPanel;
		internal QuickConnectComboBox cmbQuickConnect;
		internal System.Windows.Forms.ToolStripSeparator mMenViewSep2;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewFullscreen;
		internal System.Windows.Forms.ToolStripMenuItem mMenToolsSSHTransfer;
		internal System.Windows.Forms.ToolStripContainer tsContainer;
		internal System.Windows.Forms.ToolStripMenuItem mMenToolsExternalApps;
		internal System.Windows.Forms.Timer tmrAutoSave;
		internal System.Windows.Forms.ToolStrip tsExternalTools;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewExtAppsToolbar;
		internal System.Windows.Forms.ContextMenuStrip cMenExtAppsToolbar;
		internal System.Windows.Forms.ToolStripMenuItem cMenToolbarShowText;
		internal System.Windows.Forms.ToolStripMenuItem mMenToolsPortScan;
		internal System.Windows.Forms.ToolStrip tsQuickConnect;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewQuickConnectToolbar;
		internal System.Windows.Forms.ToolStripSeparator mMenSep3;
		internal System.Windows.Forms.ToolStripMenuItem mMenInfoDonate;
		internal System.Windows.Forms.ToolStripSeparator mMenViewSep3;
		internal ToolStripSplitButton btnQuickConnect;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewJumpTo;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewJumpToConnectionsConfig;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewJumpToErrorsInfos;
		internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
		internal System.Windows.Forms.ToolStripMenuItem mMenToolsUVNCSC;
		internal System.Windows.Forms.ToolStripMenuItem mMenToolsComponentsCheck;
		internal System.Windows.Forms.ToolStripSeparator mMenInfoSep2;
		internal System.Windows.Forms.ToolStripMenuItem mMenInfoBugReport;
		internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator2;
		internal System.Windows.Forms.ToolStripMenuItem mMenInfoForum;
		internal System.Windows.Forms.ToolStripMenuItem mMenToolsUpdate;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewResetLayout;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileDuplicate;
        internal System.Windows.Forms.ToolStripMenuItem mMenReconnectAll;
        internal System.Windows.Forms.ToolStripSeparator mMenFileSep2;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileNewConnection;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileNewFolder;
		internal System.Windows.Forms.ToolStripSeparator mMenFileSep3;
        internal System.Windows.Forms.ToolStripSeparator mMenFileSep4;
        internal System.Windows.Forms.ToolStripMenuItem mMenFileDelete;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileRename;
		internal System.Windows.Forms.ToolStripSeparator mMenFileSep5;
		internal System.Windows.Forms.ContextMenuStrip mnuQuickConnectProtocol;
		internal System.Windows.Forms.ToolStripDropDownButton btnConnections;
		internal System.Windows.Forms.ContextMenuStrip mnuConnections;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileExport;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileImportFromFile;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileImportFromActiveDirectory;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileImportFromPortScan;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileImport;
        private System.ComponentModel.IContainer components;
    }
}
