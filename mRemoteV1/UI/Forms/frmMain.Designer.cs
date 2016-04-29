namespace mRemoteNG
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
		
		//Required by the Windows Form Designer
		private System.ComponentModel.Container components = null;
		
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.Load += new System.EventHandler(frmMain_Load);
			this.Shown += new System.EventHandler(frmMain_Shown);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmMain_FormClosing);
			this.ResizeBegin += new System.EventHandler(frmMain_ResizeBegin);
			this.Resize += new System.EventHandler(frmMain_Resize);
			this.ResizeEnd += new System.EventHandler(frmMain_ResizeEnd);
			WeifenLuo.WinFormsUI.Docking.DockPanelSkin DockPanelSkin2 = new WeifenLuo.WinFormsUI.Docking.DockPanelSkin();
			WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin AutoHideStripSkin2 = new WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient DockPanelGradient4 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient TabGradient8 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin DockPaneStripSkin2 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient DockPaneStripGradient2 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient TabGradient9 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient DockPanelGradient5 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient TabGradient10 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient DockPaneStripToolWindowGradient2 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient TabGradient11 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient TabGradient12 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient DockPanelGradient6 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient TabGradient13 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient TabGradient14 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.pnlDock = new WeifenLuo.WinFormsUI.Docking.DockPanel();
			this.pnlDock.ActiveDocumentChanged += new System.EventHandler(this.pnlDock_ActiveDocumentChanged);
			this.msMain = new System.Windows.Forms.MenuStrip();
			this.mMenFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenFile.DropDownOpening += new System.EventHandler(this.mMenFile_DropDownOpening);
			this.mMenFileNewConnection = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenFileNewConnection.Click += new System.EventHandler(mMenFileNewConnection_Click);
			this.mMenFileNewFolder = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenFileNewFolder.Click += new System.EventHandler(mMenFileNewFolder_Click);
			this.mMenFileSep1 = new System.Windows.Forms.ToolStripSeparator();
			this.mMenFileNew = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenFileNew.Click += new System.EventHandler(mMenFileNew_Click);
			this.mMenFileLoad = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenFileLoad.Click += new System.EventHandler(mMenFileLoad_Click);
			this.mMenFileSave = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenFileSave.Click += new System.EventHandler(mMenFileSave_Click);
			this.mMenFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenFileSaveAs.Click += new System.EventHandler(mMenFileSaveAs_Click);
			this.mMenFileSep2 = new System.Windows.Forms.ToolStripSeparator();
			this.mMenFileImport = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenFileImportFromFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenFileImportFromFile.Click += new System.EventHandler(mMenFileImportFromFile_Click);
			this.mMenFileImportFromActiveDirectory = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenFileImportFromActiveDirectory.Click += new System.EventHandler(mMenFileImportFromActiveDirectory_Click);
			this.mMenFileImportFromPortScan = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenFileImportFromPortScan.Click += new System.EventHandler(mMenFileImportFromPortScan_Click);
			this.mMenFileExport = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenFileExport.Click += new System.EventHandler(mMenFileExport_Click);
			this.mMenFileSep3 = new System.Windows.Forms.ToolStripSeparator();
			this.mMenFileDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenFileDelete.Click += new System.EventHandler(mMenFileDelete_Click);
			this.mMenFileRename = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenFileRename.Click += new System.EventHandler(mMenFileRename_Click);
			this.mMenFileDuplicate = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenFileDuplicate.Click += new System.EventHandler(mMenFileDuplicate_Click);
			this.mMenFileSep4 = new System.Windows.Forms.ToolStripSeparator();
			this.mMenFileExit = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenFileExit.Click += new System.EventHandler(mMenFileExit_Click);
			this.mMenView = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenView.DropDownOpening += new System.EventHandler(this.mMenView_DropDownOpening);
			this.mMenViewAddConnectionPanel = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenViewAddConnectionPanel.Click += new System.EventHandler(this.mMenViewAddConnectionPanel_Click);
			this.mMenViewConnectionPanels = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenViewSep1 = new System.Windows.Forms.ToolStripSeparator();
			this.mMenViewConnections = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenViewConnections.Click += new System.EventHandler(this.mMenViewConnections_Click);
			this.mMenViewConfig = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenViewConfig.Click += new System.EventHandler(this.mMenViewConfig_Click);
			this.mMenViewErrorsAndInfos = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenViewErrorsAndInfos.Click += new System.EventHandler(this.mMenViewErrorsAndInfos_Click);
			this.mMenViewScreenshotManager = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenViewScreenshotManager.Click += new System.EventHandler(this.mMenViewScreenshotManager_Click);
			this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.mMenViewJumpTo = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenViewJumpToConnectionsConfig = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenViewJumpToConnectionsConfig.Click += new System.EventHandler(this.mMenViewJumpToConnectionsConfig_Click);
			this.mMenViewJumpToErrorsInfos = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenViewJumpToErrorsInfos.Click += new System.EventHandler(this.mMenViewJumpToErrorsInfos_Click);
			this.mMenViewResetLayout = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenViewResetLayout.Click += new System.EventHandler(this.mMenViewResetLayout_Click);
			this.mMenViewSep2 = new System.Windows.Forms.ToolStripSeparator();
			this.mMenViewQuickConnectToolbar = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenViewQuickConnectToolbar.Click += new System.EventHandler(this.mMenViewQuickConnectToolbar_Click);
			this.mMenViewExtAppsToolbar = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenViewExtAppsToolbar.Click += new System.EventHandler(this.mMenViewExtAppsToolbar_Click);
			this.mMenViewSep3 = new System.Windows.Forms.ToolStripSeparator();
			this.mMenViewFullscreen = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenViewFullscreen.Click += new System.EventHandler(this.mMenViewFullscreen_Click);
			this.mMenTools = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenToolsSSHTransfer = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenToolsSSHTransfer.Click += new System.EventHandler(this.mMenToolsSSHTransfer_Click);
			this.mMenToolsUVNCSC = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenToolsUVNCSC.Click += new System.EventHandler(this.mMenToolsUVNCSC_Click);
			this.mMenToolsExternalApps = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenToolsExternalApps.Click += new System.EventHandler(this.mMenToolsExternalApps_Click);
			this.mMenToolsPortScan = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenToolsPortScan.Click += new System.EventHandler(this.mMenToolsPortScan_Click);
			this.mMenToolsSep1 = new System.Windows.Forms.ToolStripSeparator();
			this.mMenToolsComponentsCheck = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenToolsComponentsCheck.Click += new System.EventHandler(this.mMenToolsComponentsCheck_Click);
			this.mMenToolsOptions = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenToolsOptions.Click += new System.EventHandler(this.mMenToolsOptions_Click);
			this.mMenInfo = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenInfoHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenInfoHelp.Click += new System.EventHandler(this.mMenInfoHelp_Click);
			this.mMenInfoSep1 = new System.Windows.Forms.ToolStripSeparator();
			this.mMenInfoWebsite = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenInfoWebsite.Click += new System.EventHandler(this.mMenInfoWebsite_Click);
			this.mMenInfoDonate = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenInfoDonate.Click += new System.EventHandler(this.mMenInfoDonate_Click);
			this.mMenInfoForum = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenInfoForum.Click += new System.EventHandler(this.mMenInfoForum_Click);
			this.mMenInfoBugReport = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenInfoBugReport.Click += new System.EventHandler(this.mMenInfoBugReport_Click);
			this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.mMenInfoAnnouncements = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenInfoAnnouncements.Click += new System.EventHandler(this.mMenInfoAnnouncements_Click);
			this.mMenToolsUpdate = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenToolsUpdate.Click += new System.EventHandler(this.mMenToolsUpdate_Click);
			this.mMenInfoSep2 = new System.Windows.Forms.ToolStripSeparator();
			this.mMenInfoAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenInfoAbout.Click += new System.EventHandler(this.mMenInfoAbout_Click);
			this.mMenSep3 = new System.Windows.Forms.ToolStripSeparator();
			this.lblQuickConnect = new System.Windows.Forms.ToolStripLabel();
			this.lblQuickConnect.Click += new System.EventHandler(this.lblQuickConnect_Click);
			this.cmbQuickConnect = new mRemoteNG.Controls.QuickConnectComboBox();
			this.cmbQuickConnect.ConnectRequested += new mRemoteNG.Controls.QuickConnectComboBox.ConnectRequestedEventHandler(this.cmbQuickConnect_ConnectRequested);
			this.cmbQuickConnect.ProtocolChanged += new mRemoteNG.Controls.QuickConnectComboBox.ProtocolChangedEventHandler(this.cmbQuickConnect_ProtocolChanged);
			this.tsContainer = new System.Windows.Forms.ToolStripContainer();
			this.tsQuickConnect = new System.Windows.Forms.ToolStrip();
			this.btnQuickConnect = new mRemoteNG.Controls.ToolStripSplitButton();
			this.btnQuickConnect.ButtonClick += new System.EventHandler(this.btnQuickConnect_ButtonClick);
			this.btnQuickConnect.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.btnQuickConnect_DropDownItemClicked);
			this.mnuQuickConnectProtocol = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.btnConnections = new System.Windows.Forms.ToolStripDropDownButton();
			this.btnConnections.DropDownOpening += new System.EventHandler(this.btnConnections_DropDownOpening);
			this.mnuConnections = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsExternalTools = new System.Windows.Forms.ToolStrip();
			this.cMenExtAppsToolbar = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cMenToolbarShowText = new System.Windows.Forms.ToolStripMenuItem();
			this.cMenToolbarShowText.Click += new System.EventHandler(this.cMenToolbarShowText_Click);
			this.ToolStrip1 = new System.Windows.Forms.ToolStrip();
			this.ToolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.ToolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.ToolStripButton3 = new System.Windows.Forms.ToolStripButton();
			this.ToolStripSplitButton1 = new System.Windows.Forms.ToolStripDropDownButton();
			this.ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.tmrAutoSave = new System.Windows.Forms.Timer(this.components);
			this.tmrAutoSave.Tick += new System.EventHandler(this.tmrAutoSave_Tick);
			this.msMain.SuspendLayout();
			this.tsContainer.ContentPanel.SuspendLayout();
			this.tsContainer.TopToolStripPanel.SuspendLayout();
			this.tsContainer.SuspendLayout();
			this.tsQuickConnect.SuspendLayout();
			this.cMenExtAppsToolbar.SuspendLayout();
			this.ToolStrip1.SuspendLayout();
			this.SuspendLayout();
			//
			//pnlDock
			//
			this.pnlDock.ActiveAutoHideContent = null;
			this.pnlDock.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlDock.DockBackColor = System.Drawing.SystemColors.Control;
			this.pnlDock.DockLeftPortion = 230.0D;
			this.pnlDock.DockRightPortion = 230.0D;
			this.pnlDock.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingSdi;
			this.pnlDock.Location = new System.Drawing.Point(0, 0);
			this.pnlDock.Name = "pnlDock";
			this.pnlDock.Size = new System.Drawing.Size(842, 449);
			DockPanelGradient4.EndColor = System.Drawing.SystemColors.ControlLight;
			DockPanelGradient4.StartColor = System.Drawing.SystemColors.ControlLight;
			AutoHideStripSkin2.DockStripGradient = DockPanelGradient4;
			TabGradient8.EndColor = System.Drawing.SystemColors.Control;
			TabGradient8.StartColor = System.Drawing.SystemColors.Control;
			TabGradient8.TextColor = System.Drawing.SystemColors.ControlDarkDark;
			AutoHideStripSkin2.TabGradient = TabGradient8;
			AutoHideStripSkin2.TextFont = new System.Drawing.Font("Segoe UI", (float) (9.0F));
			DockPanelSkin2.AutoHideStripSkin = AutoHideStripSkin2;
			TabGradient9.EndColor = System.Drawing.SystemColors.ControlLightLight;
			TabGradient9.StartColor = System.Drawing.SystemColors.ControlLightLight;
			TabGradient9.TextColor = System.Drawing.SystemColors.ControlText;
			DockPaneStripGradient2.ActiveTabGradient = TabGradient9;
			DockPanelGradient5.EndColor = System.Drawing.SystemColors.Control;
			DockPanelGradient5.StartColor = System.Drawing.SystemColors.Control;
			DockPaneStripGradient2.DockStripGradient = DockPanelGradient5;
			TabGradient10.EndColor = System.Drawing.SystemColors.ControlLight;
			TabGradient10.StartColor = System.Drawing.SystemColors.ControlLight;
			TabGradient10.TextColor = System.Drawing.SystemColors.ControlText;
			DockPaneStripGradient2.InactiveTabGradient = TabGradient10;
			DockPaneStripSkin2.DocumentGradient = DockPaneStripGradient2;
			DockPaneStripSkin2.TextFont = new System.Drawing.Font("Segoe UI", (float) (9.0F));
			TabGradient11.EndColor = System.Drawing.SystemColors.ActiveCaption;
			TabGradient11.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			TabGradient11.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
			TabGradient11.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
			DockPaneStripToolWindowGradient2.ActiveCaptionGradient = TabGradient11;
			TabGradient12.EndColor = System.Drawing.SystemColors.Control;
			TabGradient12.StartColor = System.Drawing.SystemColors.Control;
			TabGradient12.TextColor = System.Drawing.SystemColors.ControlText;
			DockPaneStripToolWindowGradient2.ActiveTabGradient = TabGradient12;
			DockPanelGradient6.EndColor = System.Drawing.SystemColors.ControlLight;
			DockPanelGradient6.StartColor = System.Drawing.SystemColors.ControlLight;
			DockPaneStripToolWindowGradient2.DockStripGradient = DockPanelGradient6;
			TabGradient13.EndColor = System.Drawing.SystemColors.GradientInactiveCaption;
			TabGradient13.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			TabGradient13.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
			TabGradient13.TextColor = System.Drawing.SystemColors.ControlText;
			DockPaneStripToolWindowGradient2.InactiveCaptionGradient = TabGradient13;
			TabGradient14.EndColor = System.Drawing.Color.Transparent;
			TabGradient14.StartColor = System.Drawing.Color.Transparent;
			TabGradient14.TextColor = System.Drawing.SystemColors.ControlDarkDark;
			DockPaneStripToolWindowGradient2.InactiveTabGradient = TabGradient14;
			DockPaneStripSkin2.ToolWindowGradient = DockPaneStripToolWindowGradient2;
			DockPanelSkin2.DockPaneStripSkin = DockPaneStripSkin2;
			this.pnlDock.Skin = DockPanelSkin2;
			this.pnlDock.TabIndex = 13;
			//
			//msMain
			//
			this.msMain.Dock = System.Windows.Forms.DockStyle.None;
			this.msMain.GripMargin = new System.Windows.Forms.Padding(0);
			this.msMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
			this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this.mMenFile, this.mMenView, this.mMenTools, this.mMenInfo});
			this.msMain.Location = new System.Drawing.Point(3, 0);
			this.msMain.Name = "msMain";
			this.msMain.Padding = new System.Windows.Forms.Padding(2, 2, 0, 2);
			this.msMain.Size = new System.Drawing.Size(274, 24);
			this.msMain.Stretch = false;
			this.msMain.TabIndex = 16;
			this.msMain.Text = "Main Toolbar";
			//
			//mMenFile
			//
			this.mMenFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {this.mMenFileNewConnection, this.mMenFileNewFolder, this.mMenFileSep1, this.mMenFileNew, this.mMenFileLoad, this.mMenFileSave, this.mMenFileSaveAs, this.mMenFileSep2, this.mMenFileDelete, this.mMenFileRename, this.mMenFileDuplicate, this.mMenFileSep3, this.mMenFileImport, this.mMenFileExport, this.mMenFileSep4, this.mMenFileExit});
			this.mMenFile.Name = "mMenFile";
			this.mMenFile.Size = new System.Drawing.Size(37, 20);
			this.mMenFile.Text = "&File";
			//
			//mMenFileNewConnection
			//
			this.mMenFileNewConnection.Image = My.Resources.Connection_Add;
			this.mMenFileNewConnection.Name = "mMenFileNewConnection";
			this.mMenFileNewConnection.ShortcutKeys = (System.Windows.Forms.Keys) (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N);
			this.mMenFileNewConnection.Size = new System.Drawing.Size(281, 22);
			this.mMenFileNewConnection.Text = "New Connection";
			//
			//mMenFileNewFolder
			//
			this.mMenFileNewFolder.Image = My.Resources.Folder_Add;
			this.mMenFileNewFolder.Name = "mMenFileNewFolder";
			this.mMenFileNewFolder.ShortcutKeys = (System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
				| System.Windows.Forms.Keys.N);
			this.mMenFileNewFolder.Size = new System.Drawing.Size(281, 22);
			this.mMenFileNewFolder.Text = "New Folder";
			//
			//mMenFileSep1
			//
			this.mMenFileSep1.Name = "mMenFileSep1";
			this.mMenFileSep1.Size = new System.Drawing.Size(278, 6);
			//
			//mMenFileNew
			//
			this.mMenFileNew.Image = My.Resources.Connections_New;
			this.mMenFileNew.Name = "mMenFileNew";
			this.mMenFileNew.Size = new System.Drawing.Size(281, 22);
			this.mMenFileNew.Text = "New Connection File";
			//
			//mMenFileLoad
			//
			this.mMenFileLoad.Image = My.Resources.Connections_Load;
			this.mMenFileLoad.Name = "mMenFileLoad";
			this.mMenFileLoad.ShortcutKeys = (System.Windows.Forms.Keys) (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O);
			this.mMenFileLoad.Size = new System.Drawing.Size(281, 22);
			this.mMenFileLoad.Text = "Open Connection File...";
			//
			//mMenFileSave
			//
			this.mMenFileSave.Image = My.Resources.Connections_Save;
			this.mMenFileSave.Name = "mMenFileSave";
			this.mMenFileSave.ShortcutKeys = (System.Windows.Forms.Keys) (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S);
			this.mMenFileSave.Size = new System.Drawing.Size(281, 22);
			this.mMenFileSave.Text = "Save Connection File";
			//
			//mMenFileSaveAs
			//
			this.mMenFileSaveAs.Image = My.Resources.Connections_SaveAs;
			this.mMenFileSaveAs.Name = "mMenFileSaveAs";
			this.mMenFileSaveAs.ShortcutKeys = (System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
				| System.Windows.Forms.Keys.S);
			this.mMenFileSaveAs.Size = new System.Drawing.Size(281, 22);
			this.mMenFileSaveAs.Text = "Save Connection File As...";
			//
			//mMenFileSep2
			//
			this.mMenFileSep2.Name = "mMenFileSep2";
			this.mMenFileSep2.Size = new System.Drawing.Size(278, 6);
			//
			//mMenFileImport
			//
			this.mMenFileImport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {this.mMenFileImportFromFile, this.mMenFileImportFromActiveDirectory, this.mMenFileImportFromPortScan});
			this.mMenFileImport.Name = "mMenFileImport";
			this.mMenFileImport.Size = new System.Drawing.Size(281, 22);
			this.mMenFileImport.Text = "&Import";
			//
			//mMenFileImportFromFile
			//
			this.mMenFileImportFromFile.Name = "mMenFileImportFromFile";
			this.mMenFileImportFromFile.Size = new System.Drawing.Size(235, 22);
			this.mMenFileImportFromFile.Text = "Import from &File...";
			//
			//mMenFileImportFromActiveDirectory
			//
			this.mMenFileImportFromActiveDirectory.Name = "mMenFileImportFromActiveDirectory";
			this.mMenFileImportFromActiveDirectory.Size = new System.Drawing.Size(235, 22);
			this.mMenFileImportFromActiveDirectory.Text = "Import from &Active Directory...";
			//
			//mMenFileImportFromPortScan
			//
			this.mMenFileImportFromPortScan.Name = "mMenFileImportFromPortScan";
			this.mMenFileImportFromPortScan.Size = new System.Drawing.Size(235, 22);
			this.mMenFileImportFromPortScan.Text = "Import from &Port Scan...";
			//
			//mMenFileExport
			//
			this.mMenFileExport.Name = "mMenFileExport";
			this.mMenFileExport.Size = new System.Drawing.Size(281, 22);
			this.mMenFileExport.Text = "&Export to File...";
			//
			//mMenFileSep3
			//
			this.mMenFileSep3.Name = "mMenFileSep3";
			this.mMenFileSep3.Size = new System.Drawing.Size(278, 6);
			//
			//mMenFileDelete
			//
			this.mMenFileDelete.Image = My.Resources.Delete;
			this.mMenFileDelete.Name = "mMenFileDelete";
			this.mMenFileDelete.Size = new System.Drawing.Size(281, 22);
			this.mMenFileDelete.Text = "Delete...";
			//
			//mMenFileRename
			//
			this.mMenFileRename.Image = My.Resources.Rename;
			this.mMenFileRename.Name = "mMenFileRename";
			this.mMenFileRename.Size = new System.Drawing.Size(281, 22);
			this.mMenFileRename.Text = "Rename";
			//
			//mMenFileDuplicate
			//
			this.mMenFileDuplicate.Image = My.Resources.page_copy;
			this.mMenFileDuplicate.Name = "mMenFileDuplicate";
			this.mMenFileDuplicate.Size = new System.Drawing.Size(281, 22);
			this.mMenFileDuplicate.Text = "Duplicate";
			//
			//mMenFileSep4
			//
			this.mMenFileSep4.Name = "mMenFileSep4";
			this.mMenFileSep4.Size = new System.Drawing.Size(278, 6);
			//
			//mMenFileExit
			//
			this.mMenFileExit.Image = My.Resources.Quit;
			this.mMenFileExit.Name = "mMenFileExit";
			this.mMenFileExit.ShortcutKeys = (System.Windows.Forms.Keys) (System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4);
			this.mMenFileExit.Size = new System.Drawing.Size(281, 22);
			this.mMenFileExit.Text = "Exit";
			//
			//mMenView
			//
			this.mMenView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {this.mMenViewAddConnectionPanel, this.mMenViewConnectionPanels, this.mMenViewSep1, this.mMenViewConnections, this.mMenViewConfig, this.mMenViewErrorsAndInfos, this.mMenViewScreenshotManager, this.ToolStripSeparator1, this.mMenViewJumpTo, this.mMenViewResetLayout, this.mMenViewSep2, this.mMenViewQuickConnectToolbar, this.mMenViewExtAppsToolbar, this.mMenViewSep3, this.mMenViewFullscreen});
			this.mMenView.Name = "mMenView";
			this.mMenView.Size = new System.Drawing.Size(44, 20);
			this.mMenView.Text = "&View";
			//
			//mMenViewAddConnectionPanel
			//
			this.mMenViewAddConnectionPanel.Image = My.Resources.Panel_Add;
			this.mMenViewAddConnectionPanel.Name = "mMenViewAddConnectionPanel";
			this.mMenViewAddConnectionPanel.Size = new System.Drawing.Size(228, 22);
			this.mMenViewAddConnectionPanel.Text = "Add Connection Panel";
			//
			//mMenViewConnectionPanels
			//
			this.mMenViewConnectionPanels.Image = My.Resources.Panels;
			this.mMenViewConnectionPanels.Name = "mMenViewConnectionPanels";
			this.mMenViewConnectionPanels.Size = new System.Drawing.Size(228, 22);
			this.mMenViewConnectionPanels.Text = "Connection Panels";
			//
			//mMenViewSep1
			//
			this.mMenViewSep1.Name = "mMenViewSep1";
			this.mMenViewSep1.Size = new System.Drawing.Size(225, 6);
			//
			//mMenViewConnections
			//
			this.mMenViewConnections.Checked = true;
			this.mMenViewConnections.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mMenViewConnections.Image = My.Resources.Root;
			this.mMenViewConnections.Name = "mMenViewConnections";
			this.mMenViewConnections.Size = new System.Drawing.Size(228, 22);
			this.mMenViewConnections.Text = "Connections";
			//
			//mMenViewConfig
			//
			this.mMenViewConfig.Checked = true;
			this.mMenViewConfig.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mMenViewConfig.Image = My.Resources.cog;
			this.mMenViewConfig.Name = "mMenViewConfig";
			this.mMenViewConfig.Size = new System.Drawing.Size(228, 22);
			this.mMenViewConfig.Text = "Config";
			//
			//mMenViewErrorsAndInfos
			//
			this.mMenViewErrorsAndInfos.Checked = true;
			this.mMenViewErrorsAndInfos.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mMenViewErrorsAndInfos.Image = My.Resources.ErrorsAndInfos;
			this.mMenViewErrorsAndInfos.Name = "mMenViewErrorsAndInfos";
			this.mMenViewErrorsAndInfos.Size = new System.Drawing.Size(228, 22);
			this.mMenViewErrorsAndInfos.Text = "Errors and Infos";
			//
			//mMenViewScreenshotManager
			//
			this.mMenViewScreenshotManager.Image = (System.Drawing.Image) (resources.GetObject("mMenViewScreenshotManager.Image"));
			this.mMenViewScreenshotManager.Name = "mMenViewScreenshotManager";
			this.mMenViewScreenshotManager.Size = new System.Drawing.Size(228, 22);
			this.mMenViewScreenshotManager.Text = "Screenshot Manager";
			//
			//ToolStripSeparator1
			//
			this.ToolStripSeparator1.Name = "ToolStripSeparator1";
			this.ToolStripSeparator1.Size = new System.Drawing.Size(225, 6);
			//
			//mMenViewJumpTo
			//
			this.mMenViewJumpTo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {this.mMenViewJumpToConnectionsConfig, this.mMenViewJumpToErrorsInfos});
			this.mMenViewJumpTo.Image = My.Resources.JumpTo;
			this.mMenViewJumpTo.Name = "mMenViewJumpTo";
			this.mMenViewJumpTo.Size = new System.Drawing.Size(228, 22);
			this.mMenViewJumpTo.Text = "Jump To";
			//
			//mMenViewJumpToConnectionsConfig
			//
			this.mMenViewJumpToConnectionsConfig.Image = My.Resources.Root;
			this.mMenViewJumpToConnectionsConfig.Name = "mMenViewJumpToConnectionsConfig";
			this.mMenViewJumpToConnectionsConfig.ShortcutKeys = (System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
				| System.Windows.Forms.Keys.C);
			this.mMenViewJumpToConnectionsConfig.Size = new System.Drawing.Size(260, 22);
			this.mMenViewJumpToConnectionsConfig.Text = "Connections && Config";
			//
			//mMenViewJumpToErrorsInfos
			//
			this.mMenViewJumpToErrorsInfos.Image = My.Resources.InformationSmall;
			this.mMenViewJumpToErrorsInfos.Name = "mMenViewJumpToErrorsInfos";
			this.mMenViewJumpToErrorsInfos.ShortcutKeys = (System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
				| System.Windows.Forms.Keys.E);
			this.mMenViewJumpToErrorsInfos.Size = new System.Drawing.Size(260, 22);
			this.mMenViewJumpToErrorsInfos.Text = "Errors && Infos";
			//
			//mMenViewResetLayout
			//
			this.mMenViewResetLayout.Image = My.Resources.application_side_tree;
			this.mMenViewResetLayout.Name = "mMenViewResetLayout";
			this.mMenViewResetLayout.Size = new System.Drawing.Size(228, 22);
			this.mMenViewResetLayout.Text = "Reset Layout";
			//
			//mMenViewSep2
			//
			this.mMenViewSep2.Name = "mMenViewSep2";
			this.mMenViewSep2.Size = new System.Drawing.Size(225, 6);
			//
			//mMenViewQuickConnectToolbar
			//
			this.mMenViewQuickConnectToolbar.Image = My.Resources.Play_Quick;
			this.mMenViewQuickConnectToolbar.Name = "mMenViewQuickConnectToolbar";
			this.mMenViewQuickConnectToolbar.Size = new System.Drawing.Size(228, 22);
			this.mMenViewQuickConnectToolbar.Text = "Quick Connect Toolbar";
			//
			//mMenViewExtAppsToolbar
			//
			this.mMenViewExtAppsToolbar.Image = My.Resources.ExtApp;
			this.mMenViewExtAppsToolbar.Name = "mMenViewExtAppsToolbar";
			this.mMenViewExtAppsToolbar.Size = new System.Drawing.Size(228, 22);
			this.mMenViewExtAppsToolbar.Text = "External Applications Toolbar";
			//
			//mMenViewSep3
			//
			this.mMenViewSep3.Name = "mMenViewSep3";
			this.mMenViewSep3.Size = new System.Drawing.Size(225, 6);
			//
			//mMenViewFullscreen
			//
			this.mMenViewFullscreen.Image = My.Resources.arrow_out;
			this.mMenViewFullscreen.Name = "mMenViewFullscreen";
			this.mMenViewFullscreen.ShortcutKeys = System.Windows.Forms.Keys.F11;
			this.mMenViewFullscreen.Size = new System.Drawing.Size(228, 22);
			this.mMenViewFullscreen.Text = "Full Screen";
			//
			//mMenTools
			//
			this.mMenTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {this.mMenToolsSSHTransfer, this.mMenToolsUVNCSC, this.mMenToolsExternalApps, this.mMenToolsPortScan, this.mMenToolsSep1, this.mMenToolsComponentsCheck, this.mMenToolsOptions});
			this.mMenTools.Name = "mMenTools";
			this.mMenTools.Size = new System.Drawing.Size(48, 20);
			this.mMenTools.Text = "&Tools";
			//
			//mMenToolsSSHTransfer
			//
			this.mMenToolsSSHTransfer.Image = My.Resources.SSHTransfer;
			this.mMenToolsSSHTransfer.Name = "mMenToolsSSHTransfer";
			this.mMenToolsSSHTransfer.Size = new System.Drawing.Size(184, 22);
			this.mMenToolsSSHTransfer.Text = "SSH File Transfer";
			//
			//mMenToolsUVNCSC
			//
			this.mMenToolsUVNCSC.Image = My.Resources.UVNC_SC;
			this.mMenToolsUVNCSC.Name = "mMenToolsUVNCSC";
			this.mMenToolsUVNCSC.Size = new System.Drawing.Size(184, 22);
			this.mMenToolsUVNCSC.Text = "UltraVNC SingleClick";
			this.mMenToolsUVNCSC.Visible = false;
			//
			//mMenToolsExternalApps
			//
			this.mMenToolsExternalApps.Image = My.Resources.ExtApp;
			this.mMenToolsExternalApps.Name = "mMenToolsExternalApps";
			this.mMenToolsExternalApps.Size = new System.Drawing.Size(184, 22);
			this.mMenToolsExternalApps.Text = "External Applications";
			//
			//mMenToolsPortScan
			//
			this.mMenToolsPortScan.Image = My.Resources.PortScan;
			this.mMenToolsPortScan.Name = "mMenToolsPortScan";
			this.mMenToolsPortScan.Size = new System.Drawing.Size(184, 22);
			this.mMenToolsPortScan.Text = "Port Scan";
			//
			//mMenToolsSep1
			//
			this.mMenToolsSep1.Name = "mMenToolsSep1";
			this.mMenToolsSep1.Size = new System.Drawing.Size(181, 6);
			//
			//mMenToolsComponentsCheck
			//
			this.mMenToolsComponentsCheck.Image = My.Resources.cog_error;
			this.mMenToolsComponentsCheck.Name = "mMenToolsComponentsCheck";
			this.mMenToolsComponentsCheck.Size = new System.Drawing.Size(184, 22);
			this.mMenToolsComponentsCheck.Text = "Components Check";
			//
			//mMenToolsOptions
			//
			this.mMenToolsOptions.Image = (System.Drawing.Image) (resources.GetObject("mMenToolsOptions.Image"));
			this.mMenToolsOptions.Name = "mMenToolsOptions";
			this.mMenToolsOptions.Size = new System.Drawing.Size(184, 22);
			this.mMenToolsOptions.Text = "Options";
			//
			//mMenInfo
			//
			this.mMenInfo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {this.mMenInfoHelp, this.mMenInfoSep1, this.mMenInfoWebsite, this.mMenInfoDonate, this.mMenInfoForum, this.mMenInfoBugReport, this.ToolStripSeparator2, this.mMenInfoAnnouncements, this.mMenToolsUpdate, this.mMenInfoSep2, this.mMenInfoAbout});
			this.mMenInfo.Name = "mMenInfo";
			this.mMenInfo.Size = new System.Drawing.Size(44, 20);
			this.mMenInfo.Text = "&Help";
			this.mMenInfo.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
			//
			//mMenInfoHelp
			//
			this.mMenInfoHelp.Image = (System.Drawing.Image) (resources.GetObject("mMenInfoHelp.Image"));
			this.mMenInfoHelp.Name = "mMenInfoHelp";
			this.mMenInfoHelp.ShortcutKeys = System.Windows.Forms.Keys.F1;
			this.mMenInfoHelp.Size = new System.Drawing.Size(190, 22);
			this.mMenInfoHelp.Text = "mRemoteNG Help";
			//
			//mMenInfoSep1
			//
			this.mMenInfoSep1.Name = "mMenInfoSep1";
			this.mMenInfoSep1.Size = new System.Drawing.Size(187, 6);
			//
			//mMenInfoWebsite
			//
			this.mMenInfoWebsite.Image = (System.Drawing.Image) (resources.GetObject("mMenInfoWebsite.Image"));
			this.mMenInfoWebsite.Name = "mMenInfoWebsite";
			this.mMenInfoWebsite.Size = new System.Drawing.Size(190, 22);
			this.mMenInfoWebsite.Text = "Website";
			//
			//mMenInfoDonate
			//
			this.mMenInfoDonate.Image = My.Resources.Donate;
			this.mMenInfoDonate.Name = "mMenInfoDonate";
			this.mMenInfoDonate.Size = new System.Drawing.Size(190, 22);
			this.mMenInfoDonate.Text = "Donate";
			//
			//mMenInfoForum
			//
			this.mMenInfoForum.Image = My.Resources.user_comment;
			this.mMenInfoForum.Name = "mMenInfoForum";
			this.mMenInfoForum.Size = new System.Drawing.Size(190, 22);
			this.mMenInfoForum.Text = "Support Forum";
			//
			//mMenInfoBugReport
			//
			this.mMenInfoBugReport.Image = My.Resources.Bug;
			this.mMenInfoBugReport.Name = "mMenInfoBugReport";
			this.mMenInfoBugReport.Size = new System.Drawing.Size(190, 22);
			this.mMenInfoBugReport.Text = "Report a Bug";
			//
			//ToolStripSeparator2
			//
			this.ToolStripSeparator2.Name = "ToolStripSeparator2";
			this.ToolStripSeparator2.Size = new System.Drawing.Size(187, 6);
			//
			//mMenInfoAnnouncements
			//
			this.mMenInfoAnnouncements.Image = My.Resources.News;
			this.mMenInfoAnnouncements.Name = "mMenInfoAnnouncements";
			this.mMenInfoAnnouncements.Size = new System.Drawing.Size(190, 22);
			this.mMenInfoAnnouncements.Text = "Announcements";
			//
			//mMenToolsUpdate
			//
			this.mMenToolsUpdate.Image = My.Resources.Update;
			this.mMenToolsUpdate.Name = "mMenToolsUpdate";
			this.mMenToolsUpdate.Size = new System.Drawing.Size(190, 22);
			this.mMenToolsUpdate.Text = "Check for Updates";
			//
			//mMenInfoSep2
			//
			this.mMenInfoSep2.Name = "mMenInfoSep2";
			this.mMenInfoSep2.Size = new System.Drawing.Size(187, 6);
			//
			//mMenInfoAbout
			//
			this.mMenInfoAbout.Image = My.Resources.mRemote;
			this.mMenInfoAbout.Name = "mMenInfoAbout";
			this.mMenInfoAbout.Size = new System.Drawing.Size(190, 22);
			this.mMenInfoAbout.Text = "About mRemoteNG";
			//
			//mMenSep3
			//
			this.mMenSep3.Name = "mMenSep3";
			this.mMenSep3.Size = new System.Drawing.Size(211, 6);
			//
			//lblQuickConnect
			//
			this.lblQuickConnect.Name = "lblQuickConnect";
			this.lblQuickConnect.Size = new System.Drawing.Size(55, 22);
			this.lblQuickConnect.Text = "&Connect:";
			//
			//cmbQuickConnect
			//
			this.cmbQuickConnect.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.cmbQuickConnect.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cmbQuickConnect.Margin = new System.Windows.Forms.Padding(1, 0, 3, 0);
			this.cmbQuickConnect.Name = "cmbQuickConnect";
			this.cmbQuickConnect.Size = new System.Drawing.Size(200, 25);
			//
			//tsContainer
			//
			//
			//tsContainer.BottomToolStripPanel
			//
			this.tsContainer.BottomToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			//
			//tsContainer.ContentPanel
			//
			this.tsContainer.ContentPanel.Controls.Add(this.pnlDock);
			this.tsContainer.ContentPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.tsContainer.ContentPanel.Size = new System.Drawing.Size(842, 449);
			this.tsContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			//
			//tsContainer.LeftToolStripPanel
			//
			this.tsContainer.LeftToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.tsContainer.Location = new System.Drawing.Point(0, 0);
			this.tsContainer.Name = "tsContainer";
			//
			//tsContainer.RightToolStripPanel
			//
			this.tsContainer.RightToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.tsContainer.Size = new System.Drawing.Size(842, 523);
			this.tsContainer.TabIndex = 17;
			this.tsContainer.Text = "ToolStripContainer1";
			//
			//tsContainer.TopToolStripPanel
			//
			this.tsContainer.TopToolStripPanel.Controls.Add(this.msMain);
			this.tsContainer.TopToolStripPanel.Controls.Add(this.tsQuickConnect);
			this.tsContainer.TopToolStripPanel.Controls.Add(this.tsExternalTools);
			this.tsContainer.TopToolStripPanel.Controls.Add(this.ToolStrip1);
			this.tsContainer.TopToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			//
			//tsQuickConnect
			//
			this.tsQuickConnect.Dock = System.Windows.Forms.DockStyle.None;
			this.tsQuickConnect.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this.lblQuickConnect, this.cmbQuickConnect, this.btnQuickConnect, this.btnConnections});
			this.tsQuickConnect.Location = new System.Drawing.Point(3, 24);
			this.tsQuickConnect.MaximumSize = new System.Drawing.Size(0, 25);
			this.tsQuickConnect.Name = "tsQuickConnect";
			this.tsQuickConnect.Size = new System.Drawing.Size(387, 25);
			this.tsQuickConnect.TabIndex = 18;
			//
			//btnQuickConnect
			//
			this.btnQuickConnect.DropDown = this.mnuQuickConnectProtocol;
			this.btnQuickConnect.Image = My.Resources.Play_Quick;
			this.btnQuickConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnQuickConnect.Margin = new System.Windows.Forms.Padding(0, 1, 3, 2);
			this.btnQuickConnect.Name = "btnQuickConnect";
			this.btnQuickConnect.Size = new System.Drawing.Size(84, 22);
			this.btnQuickConnect.Text = "Connect";
			//
			//mnuQuickConnectProtocol
			//
			this.mnuQuickConnectProtocol.Name = "mnuQuickConnectProtocol";
			this.mnuQuickConnectProtocol.OwnerItem = this.btnQuickConnect;
			this.mnuQuickConnectProtocol.ShowCheckMargin = true;
			this.mnuQuickConnectProtocol.ShowImageMargin = false;
			this.mnuQuickConnectProtocol.Size = new System.Drawing.Size(61, 4);
			//
			//btnConnections
			//
			this.btnConnections.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnConnections.DropDown = this.mnuConnections;
			this.btnConnections.Image = My.Resources.Root;
			this.btnConnections.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.btnConnections.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnConnections.Name = "btnConnections";
			this.btnConnections.Size = new System.Drawing.Size(29, 22);
			this.btnConnections.Text = "Connections";
			//
			//mnuConnections
			//
			this.mnuConnections.Name = "mnuConnections";
			this.mnuConnections.OwnerItem = this.btnConnections;
			this.mnuConnections.Size = new System.Drawing.Size(61, 4);
			//
			//tsExternalTools
			//
			this.tsExternalTools.ContextMenuStrip = this.cMenExtAppsToolbar;
			this.tsExternalTools.Dock = System.Windows.Forms.DockStyle.None;
			this.tsExternalTools.Location = new System.Drawing.Point(39, 49);
			this.tsExternalTools.MaximumSize = new System.Drawing.Size(0, 25);
			this.tsExternalTools.Name = "tsExternalTools";
			this.tsExternalTools.Size = new System.Drawing.Size(111, 25);
			this.tsExternalTools.TabIndex = 17;
			//
			//cMenExtAppsToolbar
			//
			this.cMenExtAppsToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this.cMenToolbarShowText});
			this.cMenExtAppsToolbar.Name = "cMenToolbar";
			this.cMenExtAppsToolbar.Size = new System.Drawing.Size(129, 26);
			//
			//cMenToolbarShowText
			//
			this.cMenToolbarShowText.Checked = true;
			this.cMenToolbarShowText.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cMenToolbarShowText.Name = "cMenToolbarShowText";
			this.cMenToolbarShowText.Size = new System.Drawing.Size(128, 22);
			this.cMenToolbarShowText.Text = "Show Text";
			//
			//ToolStrip1
			//
			this.ToolStrip1.Dock = System.Windows.Forms.DockStyle.None;
			this.ToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this.ToolStripButton1, this.ToolStripButton2, this.ToolStripButton3, this.ToolStripSplitButton1});
			this.ToolStrip1.Location = new System.Drawing.Point(3, 74);
			this.ToolStrip1.MaximumSize = new System.Drawing.Size(0, 25);
			this.ToolStrip1.Name = "ToolStrip1";
			this.ToolStrip1.Size = new System.Drawing.Size(0, 25);
			this.ToolStrip1.TabIndex = 19;
			this.ToolStrip1.Visible = false;
			//
			//ToolStripButton1
			//
			this.ToolStripButton1.Image = My.Resources.Play;
			this.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ToolStripButton1.Name = "ToolStripButton1";
			this.ToolStripButton1.Size = new System.Drawing.Size(72, 22);
			this.ToolStripButton1.Text = "Connect";
			//
			//ToolStripButton2
			//
			this.ToolStripButton2.Image = My.Resources.Screenshot;
			this.ToolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ToolStripButton2.Name = "ToolStripButton2";
			this.ToolStripButton2.Size = new System.Drawing.Size(85, 22);
			this.ToolStripButton2.Text = "Screenshot";
			//
			//ToolStripButton3
			//
			this.ToolStripButton3.Image = My.Resources.Refresh;
			this.ToolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ToolStripButton3.Name = "ToolStripButton3";
			this.ToolStripButton3.Size = new System.Drawing.Size(66, 22);
			this.ToolStripButton3.Text = "Refresh";
			//
			//ToolStripSplitButton1
			//
			this.ToolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ToolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {this.ToolStripMenuItem1, this.ToolStripMenuItem2});
			this.ToolStripSplitButton1.Image = My.Resources.Keyboard;
			this.ToolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ToolStripSplitButton1.Name = "ToolStripSplitButton1";
			this.ToolStripSplitButton1.Size = new System.Drawing.Size(29, 22);
			this.ToolStripSplitButton1.Text = "Special Keys";
			//
			//ToolStripMenuItem1
			//
			this.ToolStripMenuItem1.Name = "ToolStripMenuItem1";
			this.ToolStripMenuItem1.Size = new System.Drawing.Size(135, 22);
			this.ToolStripMenuItem1.Text = "Ctrl-Alt-Del";
			//
			//ToolStripMenuItem2
			//
			this.ToolStripMenuItem2.Name = "ToolStripMenuItem2";
			this.ToolStripMenuItem2.Size = new System.Drawing.Size(135, 22);
			this.ToolStripMenuItem2.Text = "Ctrl-Esc";
			//
			//tmrAutoSave
			//
			this.tmrAutoSave.Interval = 10000;
			//
			//frmMain
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF((float) (6.0F), (float) (13.0F));
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(842, 523);
			this.Controls.Add(this.tsContainer);
			this.Icon = My.Resources.mRemote_Icon;
			this.MainMenuStrip = this.msMain;
			this.Name = "frmMain";
			this.Opacity = 0.0D;
			this.Text = "mRemoteNG";
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
			this.ToolStrip1.ResumeLayout(false);
			this.ToolStrip1.PerformLayout();
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
		internal Controls.QuickConnectComboBox cmbQuickConnect;
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
		internal Controls.ToolStripSplitButton btnQuickConnect;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewJumpTo;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewJumpToConnectionsConfig;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewJumpToErrorsInfos;
		internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
		internal System.Windows.Forms.ToolStripMenuItem mMenToolsUVNCSC;
		internal System.Windows.Forms.ToolStripMenuItem mMenToolsComponentsCheck;
		internal System.Windows.Forms.ToolStripMenuItem mMenInfoAnnouncements;
		internal System.Windows.Forms.ToolStripSeparator mMenInfoSep2;
		internal System.Windows.Forms.ToolStripMenuItem mMenInfoBugReport;
		internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator2;
		internal System.Windows.Forms.ToolStripMenuItem mMenInfoForum;
		internal System.Windows.Forms.ToolStripMenuItem mMenToolsUpdate;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewResetLayout;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileDuplicate;
		internal System.Windows.Forms.ToolStripSeparator mMenFileSep2;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileNewConnection;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileNewFolder;
		internal System.Windows.Forms.ToolStripSeparator mMenFileSep3;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileDelete;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileRename;
		internal System.Windows.Forms.ToolStripSeparator mMenFileSep4;
		internal System.Windows.Forms.ToolStrip ToolStrip1;
		internal System.Windows.Forms.ToolStripButton ToolStripButton1;
		internal System.Windows.Forms.ToolStripButton ToolStripButton2;
		internal System.Windows.Forms.ToolStripButton ToolStripButton3;
		internal System.Windows.Forms.ToolStripDropDownButton ToolStripSplitButton1;
		internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem1;
		internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem2;
		internal System.Windows.Forms.ContextMenuStrip mnuQuickConnectProtocol;
		internal System.Windows.Forms.ToolStripDropDownButton btnConnections;
		internal System.Windows.Forms.ContextMenuStrip mnuConnections;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileExport;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileImportFromFile;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileImportFromActiveDirectory;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileImportFromPortScan;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileImport;
		
	}
}
