using mRemoteNG.Controls;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.UI.Forms
{
    public partial class frmMain : Form
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
			components = new System.ComponentModel.Container();
			Load += new EventHandler(frmMain_Load);
			Shown += new EventHandler(frmMain_Shown);
			FormClosing += new FormClosingEventHandler(frmMain_FormClosing);
			ResizeBegin += new EventHandler(frmMain_ResizeBegin);
			Resize += new EventHandler(frmMain_Resize);
			ResizeEnd += new EventHandler(frmMain_ResizeEnd);
			AutoHideStripSkin AutoHideStripSkin2 = new AutoHideStripSkin();
			DockPanelGradient DockPanelGradient4 = new DockPanelGradient();
			TabGradient TabGradient8 = new TabGradient();
			DockPaneStripSkin DockPaneStripSkin2 = new DockPaneStripSkin();
			DockPaneStripGradient DockPaneStripGradient2 = new DockPaneStripGradient();
			TabGradient TabGradient9 = new TabGradient();
			DockPanelGradient DockPanelGradient5 = new DockPanelGradient();
			TabGradient TabGradient10 = new TabGradient();
			DockPaneStripToolWindowGradient DockPaneStripToolWindowGradient2 = new DockPaneStripToolWindowGradient();
			TabGradient TabGradient11 = new TabGradient();
			TabGradient TabGradient12 = new TabGradient();
			DockPanelGradient DockPanelGradient6 = new DockPanelGradient();
			TabGradient TabGradient13 = new TabGradient();
			TabGradient TabGradient14 = new TabGradient();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			pnlDock = new DockPanel();
			pnlDock.ActiveDocumentChanged += new EventHandler(pnlDock_ActiveDocumentChanged);
			msMain = new MenuStrip();
			mMenFile = new ToolStripMenuItem();
			mMenFile.DropDownOpening += new EventHandler(mMenFile_DropDownOpening);
			mMenFileNewConnection = new ToolStripMenuItem();
			mMenFileNewConnection.Click += new EventHandler(mMenFileNewConnection_Click);
			mMenFileNewFolder = new ToolStripMenuItem();
			mMenFileNewFolder.Click += new EventHandler(mMenFileNewFolder_Click);
			mMenFileSep1 = new ToolStripSeparator();
			mMenFileNew = new ToolStripMenuItem();
			mMenFileNew.Click += new EventHandler(mMenFileNew_Click);
			mMenFileLoad = new ToolStripMenuItem();
			mMenFileLoad.Click += new EventHandler(mMenFileLoad_Click);
			mMenFileSave = new ToolStripMenuItem();
			mMenFileSave.Click += new EventHandler(mMenFileSave_Click);
			mMenFileSaveAs = new ToolStripMenuItem();
			mMenFileSaveAs.Click += new EventHandler(mMenFileSaveAs_Click);
			mMenFileSep2 = new ToolStripSeparator();
			mMenFileImport = new ToolStripMenuItem();
			mMenFileImportFromFile = new ToolStripMenuItem();
			mMenFileImportFromFile.Click += new EventHandler(mMenFileImportFromFile_Click);
			mMenFileImportFromActiveDirectory = new ToolStripMenuItem();
			mMenFileImportFromActiveDirectory.Click += new EventHandler(mMenFileImportFromActiveDirectory_Click);
			mMenFileImportFromPortScan = new ToolStripMenuItem();
			mMenFileImportFromPortScan.Click += new EventHandler(mMenFileImportFromPortScan_Click);
			mMenFileExport = new ToolStripMenuItem();
			mMenFileExport.Click += new EventHandler(mMenFileExport_Click);
			mMenFileSep3 = new ToolStripSeparator();
			mMenFileDelete = new ToolStripMenuItem();
			mMenFileDelete.Click += new EventHandler(mMenFileDelete_Click);
			mMenFileRename = new ToolStripMenuItem();
			mMenFileRename.Click += new EventHandler(mMenFileRename_Click);
			mMenFileDuplicate = new ToolStripMenuItem();
			mMenFileDuplicate.Click += new EventHandler(mMenFileDuplicate_Click);
			mMenFileSep4 = new ToolStripSeparator();
			mMenFileExit = new ToolStripMenuItem();
			mMenFileExit.Click += new EventHandler(mMenFileExit_Click);
			mMenView = new ToolStripMenuItem();
			mMenView.DropDownOpening += new EventHandler(mMenView_DropDownOpening);
			mMenViewAddConnectionPanel = new ToolStripMenuItem();
			mMenViewAddConnectionPanel.Click += new EventHandler(mMenViewAddConnectionPanel_Click);
			mMenViewConnectionPanels = new ToolStripMenuItem();
			mMenViewSep1 = new ToolStripSeparator();
			mMenViewConnections = new ToolStripMenuItem();
			mMenViewConnections.Click += new EventHandler(mMenViewConnections_Click);
			mMenViewConfig = new ToolStripMenuItem();
			mMenViewConfig.Click += new EventHandler(mMenViewConfig_Click);
			mMenViewErrorsAndInfos = new ToolStripMenuItem();
			mMenViewErrorsAndInfos.Click += new EventHandler(mMenViewErrorsAndInfos_Click);
			mMenViewScreenshotManager = new ToolStripMenuItem();
			mMenViewScreenshotManager.Click += new EventHandler(mMenViewScreenshotManager_Click);
			ToolStripSeparator1 = new ToolStripSeparator();
			mMenViewJumpTo = new ToolStripMenuItem();
			mMenViewJumpToConnectionsConfig = new ToolStripMenuItem();
			mMenViewJumpToConnectionsConfig.Click += new EventHandler(mMenViewJumpToConnectionsConfig_Click);
			mMenViewJumpToErrorsInfos = new ToolStripMenuItem();
			mMenViewJumpToErrorsInfos.Click += new EventHandler(mMenViewJumpToErrorsInfos_Click);
			mMenViewResetLayout = new ToolStripMenuItem();
			mMenViewResetLayout.Click += new EventHandler(mMenViewResetLayout_Click);
			mMenViewSep2 = new ToolStripSeparator();
			mMenViewQuickConnectToolbar = new ToolStripMenuItem();
			mMenViewQuickConnectToolbar.Click += new EventHandler(mMenViewQuickConnectToolbar_Click);
			mMenViewExtAppsToolbar = new ToolStripMenuItem();
			mMenViewExtAppsToolbar.Click += new EventHandler(mMenViewExtAppsToolbar_Click);
			mMenViewSep3 = new ToolStripSeparator();
			mMenViewFullscreen = new ToolStripMenuItem();
			mMenViewFullscreen.Click += new EventHandler(mMenViewFullscreen_Click);
			mMenTools = new ToolStripMenuItem();
			mMenToolsSSHTransfer = new ToolStripMenuItem();
			mMenToolsSSHTransfer.Click += new EventHandler(mMenToolsSSHTransfer_Click);
			mMenToolsUVNCSC = new ToolStripMenuItem();
			mMenToolsUVNCSC.Click += new EventHandler(mMenToolsUVNCSC_Click);
			mMenToolsExternalApps = new ToolStripMenuItem();
			mMenToolsExternalApps.Click += new EventHandler(mMenToolsExternalApps_Click);
			mMenToolsPortScan = new ToolStripMenuItem();
			mMenToolsPortScan.Click += new EventHandler(mMenToolsPortScan_Click);
			mMenToolsSep1 = new ToolStripSeparator();
			mMenToolsComponentsCheck = new ToolStripMenuItem();
			mMenToolsComponentsCheck.Click += new EventHandler(mMenToolsComponentsCheck_Click);
			mMenToolsOptions = new ToolStripMenuItem();
			mMenToolsOptions.Click += new EventHandler(mMenToolsOptions_Click);
			mMenInfo = new ToolStripMenuItem();
			mMenInfoHelp = new ToolStripMenuItem();
			mMenInfoHelp.Click += new EventHandler(mMenInfoHelp_Click);
			mMenInfoSep1 = new ToolStripSeparator();
			mMenInfoWebsite = new ToolStripMenuItem();
			mMenInfoWebsite.Click += new EventHandler(mMenInfoWebsite_Click);
			mMenInfoDonate = new ToolStripMenuItem();
			mMenInfoDonate.Click += new EventHandler(mMenInfoDonate_Click);
			mMenInfoForum = new ToolStripMenuItem();
			mMenInfoForum.Click += new EventHandler(mMenInfoForum_Click);
			mMenInfoBugReport = new ToolStripMenuItem();
			mMenInfoBugReport.Click += new EventHandler(mMenInfoBugReport_Click);
			ToolStripSeparator2 = new ToolStripSeparator();
			mMenInfoAnnouncements = new ToolStripMenuItem();
			mMenInfoAnnouncements.Click += new EventHandler(mMenInfoAnnouncements_Click);
			mMenToolsUpdate = new ToolStripMenuItem();
			mMenToolsUpdate.Click += new EventHandler(mMenToolsUpdate_Click);
			mMenInfoSep2 = new ToolStripSeparator();
			mMenInfoAbout = new ToolStripMenuItem();
			mMenInfoAbout.Click += new EventHandler(mMenInfoAbout_Click);
			mMenSep3 = new ToolStripSeparator();
			lblQuickConnect = new ToolStripLabel();
			lblQuickConnect.Click += new EventHandler(lblQuickConnect_Click);
			cmbQuickConnect = new mRemoteNG.Controls.QuickConnectComboBox();
			cmbQuickConnect.ConnectRequested += new mRemoteNG.Controls.QuickConnectComboBox.ConnectRequestedEventHandler(cmbQuickConnect_ConnectRequested);
			cmbQuickConnect.ProtocolChanged += new mRemoteNG.Controls.QuickConnectComboBox.ProtocolChangedEventHandler(cmbQuickConnect_ProtocolChanged);
			tsContainer = new ToolStripContainer();
			tsQuickConnect = new ToolStrip();
			btnQuickConnect = new mRemoteNG.Controls.ToolStripSplitButton();
			btnQuickConnect.ButtonClick += new EventHandler(btnQuickConnect_ButtonClick);
			btnQuickConnect.DropDownItemClicked += new ToolStripItemClickedEventHandler(btnQuickConnect_DropDownItemClicked);
			mnuQuickConnectProtocol = new ContextMenuStrip(components);
			btnConnections = new ToolStripDropDownButton();
			btnConnections.DropDownOpening += new EventHandler(btnConnections_DropDownOpening);
			mnuConnections = new ContextMenuStrip(components);
			tsExternalTools = new ToolStrip();
			cMenExtAppsToolbar = new ContextMenuStrip(components);
			cMenToolbarShowText = new ToolStripMenuItem();
			cMenToolbarShowText.Click += new EventHandler(cMenToolbarShowText_Click);
			ToolStrip1 = new ToolStrip();
			ToolStripButton1 = new ToolStripButton();
			ToolStripButton2 = new ToolStripButton();
			ToolStripButton3 = new ToolStripButton();
			ToolStripSplitButton1 = new ToolStripDropDownButton();
			ToolStripMenuItem1 = new ToolStripMenuItem();
			ToolStripMenuItem2 = new ToolStripMenuItem();
			tmrAutoSave = new Timer(components);
			tmrAutoSave.Tick += new EventHandler(tmrAutoSave_Tick);
			msMain.SuspendLayout();
			tsContainer.ContentPanel.SuspendLayout();
			tsContainer.TopToolStripPanel.SuspendLayout();
			tsContainer.SuspendLayout();
			tsQuickConnect.SuspendLayout();
			cMenExtAppsToolbar.SuspendLayout();
			ToolStrip1.SuspendLayout();
			SuspendLayout();
			//
			//pnlDock
			//
			pnlDock.ActiveAutoHideContent = null;
			pnlDock.Dock = DockStyle.Fill;
			pnlDock.DockBackColor = SystemColors.Control;
			pnlDock.DockLeftPortion = 230.0D;
			pnlDock.DockRightPortion = 230.0D;
			pnlDock.DocumentStyle = DocumentStyle.DockingSdi;
			pnlDock.Location = new Point(0, 0);
			pnlDock.Name = "pnlDock";
			pnlDock.Size = new Size(842, 449);
			DockPanelGradient4.EndColor = SystemColors.ControlLight;
			DockPanelGradient4.StartColor = SystemColors.ControlLight;
			AutoHideStripSkin2.DockStripGradient = DockPanelGradient4;
			TabGradient8.EndColor = SystemColors.Control;
			TabGradient8.StartColor = SystemColors.Control;
			TabGradient8.TextColor = SystemColors.ControlDarkDark;
			AutoHideStripSkin2.TabGradient = TabGradient8;
			AutoHideStripSkin2.TextFont = new Font("Segoe UI", 9.0F);
			TabGradient9.EndColor = SystemColors.ControlLightLight;
			TabGradient9.StartColor = SystemColors.ControlLightLight;
			TabGradient9.TextColor = SystemColors.ControlText;
			DockPaneStripGradient2.ActiveTabGradient = TabGradient9;
			DockPanelGradient5.EndColor = SystemColors.Control;
			DockPanelGradient5.StartColor = SystemColors.Control;
			DockPaneStripGradient2.DockStripGradient = DockPanelGradient5;
			TabGradient10.EndColor = SystemColors.ControlLight;
			TabGradient10.StartColor = SystemColors.ControlLight;
			TabGradient10.TextColor = SystemColors.ControlText;
			DockPaneStripGradient2.InactiveTabGradient = TabGradient10;
			DockPaneStripSkin2.DocumentGradient = DockPaneStripGradient2;
			DockPaneStripSkin2.TextFont = new Font("Segoe UI", 9.0F);
			TabGradient11.EndColor = SystemColors.ActiveCaption;
			TabGradient11.LinearGradientMode = LinearGradientMode.Vertical;
			TabGradient11.StartColor = SystemColors.GradientActiveCaption;
			TabGradient11.TextColor = SystemColors.ActiveCaptionText;
			DockPaneStripToolWindowGradient2.ActiveCaptionGradient = TabGradient11;
			TabGradient12.EndColor = SystemColors.Control;
			TabGradient12.StartColor = SystemColors.Control;
			TabGradient12.TextColor = SystemColors.ControlText;
			DockPaneStripToolWindowGradient2.ActiveTabGradient = TabGradient12;
			DockPanelGradient6.EndColor = SystemColors.ControlLight;
			DockPanelGradient6.StartColor = SystemColors.ControlLight;
			DockPaneStripToolWindowGradient2.DockStripGradient = DockPanelGradient6;
			TabGradient13.EndColor = SystemColors.GradientInactiveCaption;
			TabGradient13.LinearGradientMode = LinearGradientMode.Vertical;
			TabGradient13.StartColor = SystemColors.GradientInactiveCaption;
			TabGradient13.TextColor = SystemColors.ControlText;
			DockPaneStripToolWindowGradient2.InactiveCaptionGradient = TabGradient13;
			TabGradient14.EndColor = Color.Transparent;
			TabGradient14.StartColor = Color.Transparent;
			TabGradient14.TextColor = SystemColors.ControlDarkDark;
			DockPaneStripToolWindowGradient2.InactiveTabGradient = TabGradient14;
			DockPaneStripSkin2.ToolWindowGradient = DockPaneStripToolWindowGradient2;
			pnlDock.Theme = new VS2012LightTheme();
			pnlDock.TabIndex = 13;
			//
			//msMain
			//
			msMain.Dock = DockStyle.None;
			msMain.GripMargin = new Padding(0);
			msMain.GripStyle = ToolStripGripStyle.Visible;
			msMain.Items.AddRange(new ToolStripItem[] {mMenFile, mMenView, mMenTools, mMenInfo});
			msMain.Location = new Point(3, 0);
			msMain.Name = "msMain";
			msMain.Padding = new Padding(2, 2, 0, 2);
			msMain.Size = new Size(274, 24);
			msMain.Stretch = false;
			msMain.TabIndex = 16;
			msMain.Text = "Main Toolbar";
			//
			//mMenFile
			//
			mMenFile.DropDownItems.AddRange(new ToolStripItem[] {mMenFileNewConnection, mMenFileNewFolder, mMenFileSep1, mMenFileNew, mMenFileLoad, mMenFileSave, mMenFileSaveAs, mMenFileSep2, mMenFileDelete, mMenFileRename, mMenFileDuplicate, mMenFileSep3, mMenFileImport, mMenFileExport, mMenFileSep4, mMenFileExit});
			mMenFile.Name = "mMenFile";
			mMenFile.Size = new Size(37, 20);
			mMenFile.Text = "&File";
			//
			//mMenFileNewConnection
			//
			mMenFileNewConnection.Image = Resources.Connection_Add;
			mMenFileNewConnection.Name = "mMenFileNewConnection";
			mMenFileNewConnection.ShortcutKeys = (Keys) (Keys.Control | Keys.N);
			mMenFileNewConnection.Size = new Size(281, 22);
			mMenFileNewConnection.Text = "New Connection";
			//
			//mMenFileNewFolder
			//
			mMenFileNewFolder.Image = Resources.Folder_Add;
			mMenFileNewFolder.Name = "mMenFileNewFolder";
			mMenFileNewFolder.ShortcutKeys = (Keys) ((Keys.Control | Keys.Shift) 
				| Keys.N);
			mMenFileNewFolder.Size = new Size(281, 22);
			mMenFileNewFolder.Text = "New Folder";
			//
			//mMenFileSep1
			//
			mMenFileSep1.Name = "mMenFileSep1";
			mMenFileSep1.Size = new Size(278, 6);
			//
			//mMenFileNew
			//
			mMenFileNew.Image = Resources.Connections_New;
			mMenFileNew.Name = "mMenFileNew";
			mMenFileNew.Size = new Size(281, 22);
			mMenFileNew.Text = "New Connection File";
			//
			//mMenFileLoad
			//
			mMenFileLoad.Image = Resources.Connections_Load;
			mMenFileLoad.Name = "mMenFileLoad";
			mMenFileLoad.ShortcutKeys = (Keys) (Keys.Control | Keys.O);
			mMenFileLoad.Size = new Size(281, 22);
			mMenFileLoad.Text = "Open Connection File...";
			//
			//mMenFileSave
			//
			mMenFileSave.Image = Resources.Connections_Save;
			mMenFileSave.Name = "mMenFileSave";
			mMenFileSave.ShortcutKeys = (Keys) (Keys.Control | Keys.S);
			mMenFileSave.Size = new Size(281, 22);
			mMenFileSave.Text = "Save Connection File";
			//
			//mMenFileSaveAs
			//
			mMenFileSaveAs.Image = Resources.Connections_SaveAs;
			mMenFileSaveAs.Name = "mMenFileSaveAs";
			mMenFileSaveAs.ShortcutKeys = (Keys) ((Keys.Control | Keys.Shift) 
				| Keys.S);
			mMenFileSaveAs.Size = new Size(281, 22);
			mMenFileSaveAs.Text = "Save Connection File As...";
			//
			//mMenFileSep2
			//
			mMenFileSep2.Name = "mMenFileSep2";
			mMenFileSep2.Size = new Size(278, 6);
			//
			//mMenFileImport
			//
			mMenFileImport.DropDownItems.AddRange(new ToolStripItem[] {mMenFileImportFromFile, mMenFileImportFromActiveDirectory, mMenFileImportFromPortScan});
			mMenFileImport.Name = "mMenFileImport";
			mMenFileImport.Size = new Size(281, 22);
			mMenFileImport.Text = "&Import";
			//
			//mMenFileImportFromFile
			//
			mMenFileImportFromFile.Name = "mMenFileImportFromFile";
			mMenFileImportFromFile.Size = new Size(235, 22);
			mMenFileImportFromFile.Text = "Import from &File...";
			//
			//mMenFileImportFromActiveDirectory
			//
			mMenFileImportFromActiveDirectory.Name = "mMenFileImportFromActiveDirectory";
			mMenFileImportFromActiveDirectory.Size = new Size(235, 22);
			mMenFileImportFromActiveDirectory.Text = "Import from &Active Directory...";
			//
			//mMenFileImportFromPortScan
			//
			mMenFileImportFromPortScan.Name = "mMenFileImportFromPortScan";
			mMenFileImportFromPortScan.Size = new Size(235, 22);
			mMenFileImportFromPortScan.Text = "Import from &Port Scan...";
			//
			//mMenFileExport
			//
			mMenFileExport.Name = "mMenFileExport";
			mMenFileExport.Size = new Size(281, 22);
			mMenFileExport.Text = "&Export to File...";
			//
			//mMenFileSep3
			//
			mMenFileSep3.Name = "mMenFileSep3";
			mMenFileSep3.Size = new Size(278, 6);
			//
			//mMenFileDelete
			//
			mMenFileDelete.Image = Resources.Delete;
			mMenFileDelete.Name = "mMenFileDelete";
			mMenFileDelete.Size = new Size(281, 22);
			mMenFileDelete.Text = "Delete...";
			//
			//mMenFileRename
			//
			mMenFileRename.Image = Resources.Rename;
			mMenFileRename.Name = "mMenFileRename";
			mMenFileRename.Size = new Size(281, 22);
			mMenFileRename.Text = "Rename";
			//
			//mMenFileDuplicate
			//
			mMenFileDuplicate.Image = Resources.page_copy;
			mMenFileDuplicate.Name = "mMenFileDuplicate";
			mMenFileDuplicate.Size = new Size(281, 22);
			mMenFileDuplicate.Text = "Duplicate";
			//
			//mMenFileSep4
			//
			mMenFileSep4.Name = "mMenFileSep4";
			mMenFileSep4.Size = new Size(278, 6);
			//
			//mMenFileExit
			//
			mMenFileExit.Image = Resources.Quit;
			mMenFileExit.Name = "mMenFileExit";
			mMenFileExit.ShortcutKeys = (Keys) (Keys.Alt | Keys.F4);
			mMenFileExit.Size = new Size(281, 22);
			mMenFileExit.Text = "Exit";
			//
			//mMenView
			//
			mMenView.DropDownItems.AddRange(new ToolStripItem[] {mMenViewAddConnectionPanel, mMenViewConnectionPanels, mMenViewSep1, mMenViewConnections, mMenViewConfig, mMenViewErrorsAndInfos, mMenViewScreenshotManager, ToolStripSeparator1, mMenViewJumpTo, mMenViewResetLayout, mMenViewSep2, mMenViewQuickConnectToolbar, mMenViewExtAppsToolbar, mMenViewSep3, mMenViewFullscreen});
			mMenView.Name = "mMenView";
			mMenView.Size = new Size(44, 20);
			mMenView.Text = "&View";
			//
			//mMenViewAddConnectionPanel
			//
			mMenViewAddConnectionPanel.Image = Resources.Panel_Add;
			mMenViewAddConnectionPanel.Name = "mMenViewAddConnectionPanel";
			mMenViewAddConnectionPanel.Size = new Size(228, 22);
			mMenViewAddConnectionPanel.Text = "Add Connection Panel";
			//
			//mMenViewConnectionPanels
			//
			mMenViewConnectionPanels.Image = Resources.Panels;
			mMenViewConnectionPanels.Name = "mMenViewConnectionPanels";
			mMenViewConnectionPanels.Size = new Size(228, 22);
			mMenViewConnectionPanels.Text = "Connection Panels";
			//
			//mMenViewSep1
			//
			mMenViewSep1.Name = "mMenViewSep1";
			mMenViewSep1.Size = new Size(225, 6);
			//
			//mMenViewConnections
			//
			mMenViewConnections.Checked = true;
			mMenViewConnections.CheckState = CheckState.Checked;
			mMenViewConnections.Image = Resources.Root;
			mMenViewConnections.Name = "mMenViewConnections";
			mMenViewConnections.Size = new Size(228, 22);
			mMenViewConnections.Text = "Connections";
			//
			//mMenViewConfig
			//
			mMenViewConfig.Checked = true;
			mMenViewConfig.CheckState = CheckState.Checked;
			mMenViewConfig.Image = Resources.cog;
			mMenViewConfig.Name = "mMenViewConfig";
			mMenViewConfig.Size = new Size(228, 22);
			mMenViewConfig.Text = "Config";
			//
			//mMenViewErrorsAndInfos
			//
			mMenViewErrorsAndInfos.Checked = true;
			mMenViewErrorsAndInfos.CheckState = CheckState.Checked;
			mMenViewErrorsAndInfos.Image = Resources.ErrorsAndInfos;
			mMenViewErrorsAndInfos.Name = "mMenViewErrorsAndInfos";
			mMenViewErrorsAndInfos.Size = new Size(228, 22);
			mMenViewErrorsAndInfos.Text = "Errors and Infos";
			//
			//mMenViewScreenshotManager
			//
			mMenViewScreenshotManager.Image = (Image) (resources.GetObject("mMenViewScreenshotManager.Image"));
			mMenViewScreenshotManager.Name = "mMenViewScreenshotManager";
			mMenViewScreenshotManager.Size = new Size(228, 22);
			mMenViewScreenshotManager.Text = "Screenshot Manager";
			//
			//ToolStripSeparator1
			//
			ToolStripSeparator1.Name = "ToolStripSeparator1";
			ToolStripSeparator1.Size = new Size(225, 6);
			//
			//mMenViewJumpTo
			//
			mMenViewJumpTo.DropDownItems.AddRange(new ToolStripItem[] {mMenViewJumpToConnectionsConfig, mMenViewJumpToErrorsInfos});
			mMenViewJumpTo.Image = Resources.JumpTo;
			mMenViewJumpTo.Name = "mMenViewJumpTo";
			mMenViewJumpTo.Size = new Size(228, 22);
			mMenViewJumpTo.Text = "Jump To";
			//
			//mMenViewJumpToConnectionsConfig
			//
			mMenViewJumpToConnectionsConfig.Image = Resources.Root;
			mMenViewJumpToConnectionsConfig.Name = "mMenViewJumpToConnectionsConfig";
			mMenViewJumpToConnectionsConfig.ShortcutKeys = (Keys) ((Keys.Control | Keys.Alt) 
				| Keys.C);
			mMenViewJumpToConnectionsConfig.Size = new Size(260, 22);
			mMenViewJumpToConnectionsConfig.Text = "Connections && Config";
			//
			//mMenViewJumpToErrorsInfos
			//
			mMenViewJumpToErrorsInfos.Image = Resources.InformationSmall;
			mMenViewJumpToErrorsInfos.Name = "mMenViewJumpToErrorsInfos";
			mMenViewJumpToErrorsInfos.ShortcutKeys = (Keys) ((Keys.Control | Keys.Alt) 
				| Keys.E);
			mMenViewJumpToErrorsInfos.Size = new Size(260, 22);
			mMenViewJumpToErrorsInfos.Text = "Errors && Infos";
			//
			//mMenViewResetLayout
			//
			mMenViewResetLayout.Image = Resources.application_side_tree;
			mMenViewResetLayout.Name = "mMenViewResetLayout";
			mMenViewResetLayout.Size = new Size(228, 22);
			mMenViewResetLayout.Text = "Reset Layout";
			//
			//mMenViewSep2
			//
			mMenViewSep2.Name = "mMenViewSep2";
			mMenViewSep2.Size = new Size(225, 6);
			//
			//mMenViewQuickConnectToolbar
			//
			mMenViewQuickConnectToolbar.Image = Resources.Play_Quick;
			mMenViewQuickConnectToolbar.Name = "mMenViewQuickConnectToolbar";
			mMenViewQuickConnectToolbar.Size = new Size(228, 22);
			mMenViewQuickConnectToolbar.Text = "Quick Connect Toolbar";
			//
			//mMenViewExtAppsToolbar
			//
			mMenViewExtAppsToolbar.Image = Resources.ExtApp;
			mMenViewExtAppsToolbar.Name = "mMenViewExtAppsToolbar";
			mMenViewExtAppsToolbar.Size = new Size(228, 22);
			mMenViewExtAppsToolbar.Text = "External Applications Toolbar";
			//
			//mMenViewSep3
			//
			mMenViewSep3.Name = "mMenViewSep3";
			mMenViewSep3.Size = new Size(225, 6);
			//
			//mMenViewFullscreen
			//
			mMenViewFullscreen.Image = Resources.arrow_out;
			mMenViewFullscreen.Name = "mMenViewFullscreen";
			mMenViewFullscreen.ShortcutKeys = Keys.F11;
			mMenViewFullscreen.Size = new Size(228, 22);
			mMenViewFullscreen.Text = "Full Screen";
			//
			//mMenTools
			//
			mMenTools.DropDownItems.AddRange(new ToolStripItem[] {mMenToolsSSHTransfer, mMenToolsUVNCSC, mMenToolsExternalApps, mMenToolsPortScan, mMenToolsSep1, mMenToolsComponentsCheck, mMenToolsOptions});
			mMenTools.Name = "mMenTools";
			mMenTools.Size = new Size(48, 20);
			mMenTools.Text = "&Tools";
			//
			//mMenToolsSSHTransfer
			//
			mMenToolsSSHTransfer.Image = Resources.SSHTransfer;
			mMenToolsSSHTransfer.Name = "mMenToolsSSHTransfer";
			mMenToolsSSHTransfer.Size = new Size(184, 22);
			mMenToolsSSHTransfer.Text = "SSH File Transfer";
			//
			//mMenToolsUVNCSC
			//
			mMenToolsUVNCSC.Image = Resources.UVNC_SC;
			mMenToolsUVNCSC.Name = "mMenToolsUVNCSC";
			mMenToolsUVNCSC.Size = new Size(184, 22);
			mMenToolsUVNCSC.Text = "UltraVNC SingleClick";
			mMenToolsUVNCSC.Visible = false;
			//
			//mMenToolsExternalApps
			//
			mMenToolsExternalApps.Image = Resources.ExtApp;
			mMenToolsExternalApps.Name = "mMenToolsExternalApps";
			mMenToolsExternalApps.Size = new Size(184, 22);
			mMenToolsExternalApps.Text = "External Applications";
			//
			//mMenToolsPortScan
			//
			mMenToolsPortScan.Image = Resources.PortScan;
			mMenToolsPortScan.Name = "mMenToolsPortScan";
			mMenToolsPortScan.Size = new Size(184, 22);
			mMenToolsPortScan.Text = "Port Scan";
			//
			//mMenToolsSep1
			//
			mMenToolsSep1.Name = "mMenToolsSep1";
			mMenToolsSep1.Size = new Size(181, 6);
			//
			//mMenToolsComponentsCheck
			//
			mMenToolsComponentsCheck.Image = Resources.cog_error;
			mMenToolsComponentsCheck.Name = "mMenToolsComponentsCheck";
			mMenToolsComponentsCheck.Size = new Size(184, 22);
			mMenToolsComponentsCheck.Text = "Components Check";
			//
			//mMenToolsOptions
			//
			mMenToolsOptions.Image = (Image) (resources.GetObject("mMenToolsOptions.Image"));
			mMenToolsOptions.Name = "mMenToolsOptions";
			mMenToolsOptions.Size = new Size(184, 22);
			mMenToolsOptions.Text = "Options";
			//
			//mMenInfo
			//
			mMenInfo.DropDownItems.AddRange(new ToolStripItem[] {mMenInfoHelp, mMenInfoSep1, mMenInfoWebsite, mMenInfoDonate, mMenInfoForum, mMenInfoBugReport, ToolStripSeparator2, mMenInfoAnnouncements, mMenToolsUpdate, mMenInfoSep2, mMenInfoAbout});
			mMenInfo.Name = "mMenInfo";
			mMenInfo.Size = new Size(44, 20);
			mMenInfo.Text = "&Help";
			mMenInfo.TextDirection = ToolStripTextDirection.Horizontal;
			//
			//mMenInfoHelp
			//
			mMenInfoHelp.Image = (Image) (resources.GetObject("mMenInfoHelp.Image"));
			mMenInfoHelp.Name = "mMenInfoHelp";
			mMenInfoHelp.ShortcutKeys = Keys.F1;
			mMenInfoHelp.Size = new Size(190, 22);
			mMenInfoHelp.Text = "mRemoteNG Help";
			//
			//mMenInfoSep1
			//
			mMenInfoSep1.Name = "mMenInfoSep1";
			mMenInfoSep1.Size = new Size(187, 6);
			//
			//mMenInfoWebsite
			//
			mMenInfoWebsite.Image = (Image) (resources.GetObject("mMenInfoWebsite.Image"));
			mMenInfoWebsite.Name = "mMenInfoWebsite";
			mMenInfoWebsite.Size = new Size(190, 22);
			mMenInfoWebsite.Text = "Website";
			//
			//mMenInfoDonate
			//
			mMenInfoDonate.Image = Resources.Donate;
			mMenInfoDonate.Name = "mMenInfoDonate";
			mMenInfoDonate.Size = new Size(190, 22);
			mMenInfoDonate.Text = "Donate";
			//
			//mMenInfoForum
			//
			mMenInfoForum.Image = Resources.user_comment;
			mMenInfoForum.Name = "mMenInfoForum";
			mMenInfoForum.Size = new Size(190, 22);
			mMenInfoForum.Text = "Support Forum";
			//
			//mMenInfoBugReport
			//
			mMenInfoBugReport.Image = Resources.Bug;
			mMenInfoBugReport.Name = "mMenInfoBugReport";
			mMenInfoBugReport.Size = new Size(190, 22);
			mMenInfoBugReport.Text = "Report a Bug";
			//
			//ToolStripSeparator2
			//
			ToolStripSeparator2.Name = "ToolStripSeparator2";
			ToolStripSeparator2.Size = new Size(187, 6);
			//
			//mMenInfoAnnouncements
			//
			mMenInfoAnnouncements.Image = Resources.News;
			mMenInfoAnnouncements.Name = "mMenInfoAnnouncements";
			mMenInfoAnnouncements.Size = new Size(190, 22);
			mMenInfoAnnouncements.Text = "Announcements";
			//
			//mMenToolsUpdate
			//
			mMenToolsUpdate.Image = Resources.Update;
			mMenToolsUpdate.Name = "mMenToolsUpdate";
			mMenToolsUpdate.Size = new Size(190, 22);
			mMenToolsUpdate.Text = "Check for Updates";
			//
			//mMenInfoSep2
			//
			mMenInfoSep2.Name = "mMenInfoSep2";
			mMenInfoSep2.Size = new Size(187, 6);
			//
			//mMenInfoAbout
			//
			mMenInfoAbout.Image = Resources.mRemote;
			mMenInfoAbout.Name = "mMenInfoAbout";
			mMenInfoAbout.Size = new Size(190, 22);
			mMenInfoAbout.Text = "About mRemoteNG";
			//
			//mMenSep3
			//
			mMenSep3.Name = "mMenSep3";
			mMenSep3.Size = new Size(211, 6);
			//
			//lblQuickConnect
			//
			lblQuickConnect.Name = "lblQuickConnect";
			lblQuickConnect.Size = new Size(55, 22);
			lblQuickConnect.Text = "&Connect:";
			//
			//cmbQuickConnect
			//
			cmbQuickConnect.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			cmbQuickConnect.AutoCompleteSource = AutoCompleteSource.ListItems;
			cmbQuickConnect.Margin = new Padding(1, 0, 3, 0);
			cmbQuickConnect.Name = "cmbQuickConnect";
			cmbQuickConnect.Size = new Size(200, 25);
			//
			//tsContainer
			//
			//
			//tsContainer.BottomToolStripPanel
			//
			tsContainer.BottomToolStripPanel.RenderMode = ToolStripRenderMode.Professional;
			//
			//tsContainer.ContentPanel
			//
			tsContainer.ContentPanel.Controls.Add(pnlDock);
			tsContainer.ContentPanel.RenderMode = ToolStripRenderMode.Professional;
			tsContainer.ContentPanel.Size = new Size(842, 449);
			tsContainer.Dock = DockStyle.Fill;
			//
			//tsContainer.LeftToolStripPanel
			//
			tsContainer.LeftToolStripPanel.RenderMode = ToolStripRenderMode.Professional;
			tsContainer.Location = new Point(0, 0);
			tsContainer.Name = "tsContainer";
			//
			//tsContainer.RightToolStripPanel
			//
			tsContainer.RightToolStripPanel.RenderMode = ToolStripRenderMode.Professional;
			tsContainer.Size = new Size(842, 523);
			tsContainer.TabIndex = 17;
			tsContainer.Text = "ToolStripContainer1";
			//
			//tsContainer.TopToolStripPanel
			//
			tsContainer.TopToolStripPanel.Controls.Add(msMain);
			tsContainer.TopToolStripPanel.Controls.Add(tsQuickConnect);
			tsContainer.TopToolStripPanel.Controls.Add(tsExternalTools);
			tsContainer.TopToolStripPanel.Controls.Add(ToolStrip1);
			tsContainer.TopToolStripPanel.RenderMode = ToolStripRenderMode.Professional;
			//
			//tsQuickConnect
			//
			tsQuickConnect.Dock = DockStyle.None;
			tsQuickConnect.Items.AddRange(new ToolStripItem[] {lblQuickConnect, cmbQuickConnect, btnQuickConnect, btnConnections});
			tsQuickConnect.Location = new Point(3, 24);
			tsQuickConnect.MaximumSize = new Size(0, 25);
			tsQuickConnect.Name = "tsQuickConnect";
			tsQuickConnect.Size = new Size(387, 25);
			tsQuickConnect.TabIndex = 18;
			//
			//btnQuickConnect
			//
			btnQuickConnect.DropDown = mnuQuickConnectProtocol;
			btnQuickConnect.Image = Resources.Play_Quick;
			btnQuickConnect.ImageTransparentColor = Color.Magenta;
			btnQuickConnect.Margin = new Padding(0, 1, 3, 2);
			btnQuickConnect.Name = "btnQuickConnect";
			btnQuickConnect.Size = new Size(84, 22);
			btnQuickConnect.Text = "Connect";
			//
			//mnuQuickConnectProtocol
			//
			mnuQuickConnectProtocol.Name = "mnuQuickConnectProtocol";
			mnuQuickConnectProtocol.OwnerItem = btnQuickConnect;
			mnuQuickConnectProtocol.ShowCheckMargin = true;
			mnuQuickConnectProtocol.ShowImageMargin = false;
			mnuQuickConnectProtocol.Size = new Size(61, 4);
			//
			//btnConnections
			//
			btnConnections.DisplayStyle = ToolStripItemDisplayStyle.Image;
			btnConnections.DropDown = mnuConnections;
			btnConnections.Image = Resources.Root;
			btnConnections.ImageScaling = ToolStripItemImageScaling.None;
			btnConnections.ImageTransparentColor = Color.Magenta;
			btnConnections.Name = "btnConnections";
			btnConnections.Size = new Size(29, 22);
			btnConnections.Text = "Connections";
			//
			//mnuConnections
			//
			mnuConnections.Name = "mnuConnections";
			mnuConnections.OwnerItem = btnConnections;
			mnuConnections.Size = new Size(61, 4);
			//
			//tsExternalTools
			//
			tsExternalTools.ContextMenuStrip = cMenExtAppsToolbar;
			tsExternalTools.Dock = DockStyle.None;
			tsExternalTools.Location = new Point(39, 49);
			tsExternalTools.MaximumSize = new Size(0, 25);
			tsExternalTools.Name = "tsExternalTools";
			tsExternalTools.Size = new Size(111, 25);
			tsExternalTools.TabIndex = 17;
			//
			//cMenExtAppsToolbar
			//
			cMenExtAppsToolbar.Items.AddRange(new ToolStripItem[] {cMenToolbarShowText});
			cMenExtAppsToolbar.Name = "cMenToolbar";
			cMenExtAppsToolbar.Size = new Size(129, 26);
			//
			//cMenToolbarShowText
			//
			cMenToolbarShowText.Checked = true;
			cMenToolbarShowText.CheckState = CheckState.Checked;
			cMenToolbarShowText.Name = "cMenToolbarShowText";
			cMenToolbarShowText.Size = new Size(128, 22);
			cMenToolbarShowText.Text = "Show Text";
			//
			//ToolStrip1
			//
			ToolStrip1.Dock = DockStyle.None;
			ToolStrip1.Items.AddRange(new ToolStripItem[] {ToolStripButton1, ToolStripButton2, ToolStripButton3, ToolStripSplitButton1});
			ToolStrip1.Location = new Point(3, 74);
			ToolStrip1.MaximumSize = new Size(0, 25);
			ToolStrip1.Name = "ToolStrip1";
			ToolStrip1.Size = new Size(0, 25);
			ToolStrip1.TabIndex = 19;
			ToolStrip1.Visible = false;
			//
			//ToolStripButton1
			//
			ToolStripButton1.Image = Resources.Play;
			ToolStripButton1.ImageTransparentColor = Color.Magenta;
			ToolStripButton1.Name = "ToolStripButton1";
			ToolStripButton1.Size = new Size(72, 22);
			ToolStripButton1.Text = "Connect";
			//
			//ToolStripButton2
			//
			ToolStripButton2.Image = Resources.Screenshot;
			ToolStripButton2.ImageTransparentColor = Color.Magenta;
			ToolStripButton2.Name = "ToolStripButton2";
			ToolStripButton2.Size = new Size(85, 22);
			ToolStripButton2.Text = "Screenshot";
			//
			//ToolStripButton3
			//
			ToolStripButton3.Image = Resources.Refresh;
			ToolStripButton3.ImageTransparentColor = Color.Magenta;
			ToolStripButton3.Name = "ToolStripButton3";
			ToolStripButton3.Size = new Size(66, 22);
			ToolStripButton3.Text = "Refresh";
			//
			//ToolStripSplitButton1
			//
			ToolStripSplitButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
			ToolStripSplitButton1.DropDownItems.AddRange(new ToolStripItem[] {ToolStripMenuItem1, ToolStripMenuItem2});
			ToolStripSplitButton1.Image = Resources.Keyboard;
			ToolStripSplitButton1.ImageTransparentColor = Color.Magenta;
			ToolStripSplitButton1.Name = "ToolStripSplitButton1";
			ToolStripSplitButton1.Size = new Size(29, 22);
			ToolStripSplitButton1.Text = "Special Keys";
			//
			//ToolStripMenuItem1
			//
			ToolStripMenuItem1.Name = "ToolStripMenuItem1";
			ToolStripMenuItem1.Size = new Size(135, 22);
			ToolStripMenuItem1.Text = "Ctrl-Alt-Del";
			//
			//ToolStripMenuItem2
			//
			ToolStripMenuItem2.Name = "ToolStripMenuItem2";
			ToolStripMenuItem2.Size = new Size(135, 22);
			ToolStripMenuItem2.Text = "Ctrl-Esc";
			//
			//tmrAutoSave
			//
			tmrAutoSave.Interval = 10000;
			//
			//frmMain
			//
			AutoScaleDimensions = new SizeF((float) (6.0F), (float) (13.0F));
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(842, 523);
			Controls.Add(tsContainer);
			Icon = Resources.mRemote_Icon;
			MainMenuStrip = msMain;
			Name = "frmMain";
			Opacity = 0.0D;
			Text = "mRemoteNG";
			msMain.ResumeLayout(false);
			msMain.PerformLayout();
			tsContainer.ContentPanel.ResumeLayout(false);
			tsContainer.TopToolStripPanel.ResumeLayout(false);
			tsContainer.TopToolStripPanel.PerformLayout();
			tsContainer.ResumeLayout(false);
			tsContainer.PerformLayout();
			tsQuickConnect.ResumeLayout(false);
			tsQuickConnect.PerformLayout();
			cMenExtAppsToolbar.ResumeLayout(false);
			ToolStrip1.ResumeLayout(false);
			ToolStrip1.PerformLayout();
			ResumeLayout(false);
			
		}
		internal DockPanel pnlDock;
		internal MenuStrip msMain;
		internal ToolStripMenuItem mMenFile;
		internal ToolStripMenuItem mMenView;
		internal ToolStripMenuItem mMenTools;
		internal ToolStripLabel lblQuickConnect;
		internal ToolStripMenuItem mMenInfo;
		internal ToolStripMenuItem mMenFileNew;
		internal ToolStripMenuItem mMenFileLoad;
		internal ToolStripMenuItem mMenFileSave;
		internal ToolStripMenuItem mMenFileSaveAs;
		internal ToolStripSeparator mMenFileSep1;
		internal ToolStripMenuItem mMenFileExit;
		internal ToolStripSeparator mMenToolsSep1;
		internal ToolStripMenuItem mMenToolsOptions;
		internal ToolStripMenuItem mMenInfoHelp;
		internal ToolStripMenuItem mMenInfoWebsite;
		internal ToolStripSeparator mMenInfoSep1;
		internal ToolStripMenuItem mMenInfoAbout;
		internal ToolStripMenuItem mMenViewConnectionPanels;
		internal ToolStripSeparator mMenViewSep1;
		internal ToolStripMenuItem mMenViewConnections;
		internal ToolStripMenuItem mMenViewConfig;
		internal ToolStripMenuItem mMenViewErrorsAndInfos;
		internal ToolStripMenuItem mMenViewScreenshotManager;
		internal ToolStripMenuItem mMenViewAddConnectionPanel;
		internal QuickConnectComboBox cmbQuickConnect;
		internal ToolStripSeparator mMenViewSep2;
		internal ToolStripMenuItem mMenViewFullscreen;
		internal ToolStripMenuItem mMenToolsSSHTransfer;
		internal ToolStripContainer tsContainer;
		internal ToolStripMenuItem mMenToolsExternalApps;
		internal Timer tmrAutoSave;
		internal ToolStrip tsExternalTools;
		internal ToolStripMenuItem mMenViewExtAppsToolbar;
		internal ContextMenuStrip cMenExtAppsToolbar;
		internal ToolStripMenuItem cMenToolbarShowText;
		internal ToolStripMenuItem mMenToolsPortScan;
		internal ToolStrip tsQuickConnect;
		internal ToolStripMenuItem mMenViewQuickConnectToolbar;
		internal ToolStripSeparator mMenSep3;
		internal ToolStripMenuItem mMenInfoDonate;
		internal ToolStripSeparator mMenViewSep3;
		internal mRemoteNG.Controls.ToolStripSplitButton btnQuickConnect;
		internal ToolStripMenuItem mMenViewJumpTo;
		internal ToolStripMenuItem mMenViewJumpToConnectionsConfig;
		internal ToolStripMenuItem mMenViewJumpToErrorsInfos;
		internal ToolStripSeparator ToolStripSeparator1;
		internal ToolStripMenuItem mMenToolsUVNCSC;
		internal ToolStripMenuItem mMenToolsComponentsCheck;
		internal ToolStripMenuItem mMenInfoAnnouncements;
		internal ToolStripSeparator mMenInfoSep2;
		internal ToolStripMenuItem mMenInfoBugReport;
		internal ToolStripSeparator ToolStripSeparator2;
		internal ToolStripMenuItem mMenInfoForum;
		internal ToolStripMenuItem mMenToolsUpdate;
		internal ToolStripMenuItem mMenViewResetLayout;
		internal ToolStripMenuItem mMenFileDuplicate;
		internal ToolStripSeparator mMenFileSep2;
		internal ToolStripMenuItem mMenFileNewConnection;
		internal ToolStripMenuItem mMenFileNewFolder;
		internal ToolStripSeparator mMenFileSep3;
		internal ToolStripMenuItem mMenFileDelete;
		internal ToolStripMenuItem mMenFileRename;
		internal ToolStripSeparator mMenFileSep4;
		internal ToolStrip ToolStrip1;
		internal ToolStripButton ToolStripButton1;
		internal ToolStripButton ToolStripButton2;
		internal ToolStripButton ToolStripButton3;
		internal ToolStripDropDownButton ToolStripSplitButton1;
		internal ToolStripMenuItem ToolStripMenuItem1;
		internal ToolStripMenuItem ToolStripMenuItem2;
		internal ContextMenuStrip mnuQuickConnectProtocol;
		internal ToolStripDropDownButton btnConnections;
		internal ContextMenuStrip mnuConnections;
		internal ToolStripMenuItem mMenFileExport;
		internal ToolStripMenuItem mMenFileImportFromFile;
		internal ToolStripMenuItem mMenFileImportFromActiveDirectory;
		internal ToolStripMenuItem mMenFileImportFromPortScan;
		internal ToolStripMenuItem mMenFileImport;
		
	}
}
