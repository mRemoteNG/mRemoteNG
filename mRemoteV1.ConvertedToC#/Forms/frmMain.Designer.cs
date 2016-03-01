using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
namespace mRemoteNG
{
	[Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
	partial class frmMain : System.Windows.Forms.Form
	{

		//Form overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]
		protected override void Dispose(bool disposing)
		{
			try {
				if (disposing && components != null) {
					components.Dispose();
				}
			} finally {
				base.Dispose(disposing);
			}
		}

		//Required by the Windows Form Designer

		private System.ComponentModel.IContainer components;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
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
			this.mMenFileImport = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenFileImportFromFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenFileImportFromActiveDirectory = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenFileImportFromPortScan = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenFileExport = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenFileSep3 = new System.Windows.Forms.ToolStripSeparator();
			this.mMenFileDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenFileRename = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenFileDuplicate = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenFileSep4 = new System.Windows.Forms.ToolStripSeparator();
			this.mMenFileExit = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenView = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenViewAddConnectionPanel = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenViewConnectionPanels = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenViewSep1 = new System.Windows.Forms.ToolStripSeparator();
			this.mMenViewConnections = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenViewConfig = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenViewSessions = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenViewErrorsAndInfos = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenViewScreenshotManager = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.mMenViewJumpTo = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenViewJumpToConnectionsConfig = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenViewJumpToSessionsScreenshots = new System.Windows.Forms.ToolStripMenuItem();
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
			this.mMenInfoAnnouncements = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenToolsUpdate = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenInfoSep2 = new System.Windows.Forms.ToolStripSeparator();
			this.mMenInfoAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenSep3 = new System.Windows.Forms.ToolStripSeparator();
			this.lblQuickConnect = new System.Windows.Forms.ToolStripLabel();
			this.cmbQuickConnect = new mRemoteNG.Controls.QuickConnectComboBox();
			this.tsContainer = new System.Windows.Forms.ToolStripContainer();
			this.tsQuickConnect = new System.Windows.Forms.ToolStrip();
			this.btnQuickConnect = new mRemoteNG.Controls.ToolStripSplitButton();
			this.mnuQuickConnectProtocol = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.btnConnections = new System.Windows.Forms.ToolStripDropDownButton();
			this.mnuConnections = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsExternalTools = new System.Windows.Forms.ToolStrip();
			this.cMenExtAppsToolbar = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cMenToolbarShowText = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStrip1 = new System.Windows.Forms.ToolStrip();
			this.ToolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.ToolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.ToolStripButton3 = new System.Windows.Forms.ToolStripButton();
			this.ToolStripSplitButton1 = new System.Windows.Forms.ToolStripDropDownButton();
			this.ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.tmrAutoSave = new System.Windows.Forms.Timer(this.components);
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
			this.pnlDock.DockLeftPortion = 230.0;
			this.pnlDock.DockRightPortion = 230.0;
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
			AutoHideStripSkin2.TextFont = new System.Drawing.Font("Segoe UI", 9f);
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
			DockPaneStripSkin2.TextFont = new System.Drawing.Font("Segoe UI", 9f);
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
			this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.mMenFile,
				this.mMenView,
				this.mMenTools,
				this.mMenInfo
			});
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
				this.mMenFileSep3,
				this.mMenFileImport,
				this.mMenFileExport,
				this.mMenFileSep4,
				this.mMenFileExit
			});
			this.mMenFile.Name = "mMenFile";
			this.mMenFile.Size = new System.Drawing.Size(37, 20);
			this.mMenFile.Text = "&File";
			//
			//mMenFileNewConnection
			//
			this.mMenFileNewConnection.Image = global::mRemoteNG.My.Resources.Resources.Connection_Add;
			this.mMenFileNewConnection.Name = "mMenFileNewConnection";
			this.mMenFileNewConnection.ShortcutKeys = (System.Windows.Forms.Keys)(System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N);
			this.mMenFileNewConnection.Size = new System.Drawing.Size(281, 22);
			this.mMenFileNewConnection.Text = "New Connection";
			//
			//mMenFileNewFolder
			//
			this.mMenFileNewFolder.Image = global::mRemoteNG.My.Resources.Resources.Folder_Add;
			this.mMenFileNewFolder.Name = "mMenFileNewFolder";
			this.mMenFileNewFolder.ShortcutKeys = (System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) | System.Windows.Forms.Keys.N);
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
			this.mMenFileNew.Image = global::mRemoteNG.My.Resources.Resources.Connections_New;
			this.mMenFileNew.Name = "mMenFileNew";
			this.mMenFileNew.Size = new System.Drawing.Size(281, 22);
			this.mMenFileNew.Text = "New Connection File";
			//
			//mMenFileLoad
			//
			this.mMenFileLoad.Image = global::mRemoteNG.My.Resources.Resources.Connections_Load;
			this.mMenFileLoad.Name = "mMenFileLoad";
			this.mMenFileLoad.ShortcutKeys = (System.Windows.Forms.Keys)(System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O);
			this.mMenFileLoad.Size = new System.Drawing.Size(281, 22);
			this.mMenFileLoad.Text = "Open Connection File...";
			//
			//mMenFileSave
			//
			this.mMenFileSave.Image = global::mRemoteNG.My.Resources.Resources.Connections_Save;
			this.mMenFileSave.Name = "mMenFileSave";
			this.mMenFileSave.ShortcutKeys = (System.Windows.Forms.Keys)(System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S);
			this.mMenFileSave.Size = new System.Drawing.Size(281, 22);
			this.mMenFileSave.Text = "Save Connection File";
			//
			//mMenFileSaveAs
			//
			this.mMenFileSaveAs.Image = global::mRemoteNG.My.Resources.Resources.Connections_SaveAs;
			this.mMenFileSaveAs.Name = "mMenFileSaveAs";
			this.mMenFileSaveAs.ShortcutKeys = (System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) | System.Windows.Forms.Keys.S);
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
			this.mMenFileImport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.mMenFileImportFromFile,
				this.mMenFileImportFromActiveDirectory,
				this.mMenFileImportFromPortScan
			});
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
			this.mMenFileDelete.Image = global::mRemoteNG.My.Resources.Resources.Delete;
			this.mMenFileDelete.Name = "mMenFileDelete";
			this.mMenFileDelete.Size = new System.Drawing.Size(281, 22);
			this.mMenFileDelete.Text = "Delete...";
			//
			//mMenFileRename
			//
			this.mMenFileRename.Image = global::mRemoteNG.My.Resources.Resources.Rename;
			this.mMenFileRename.Name = "mMenFileRename";
			this.mMenFileRename.Size = new System.Drawing.Size(281, 22);
			this.mMenFileRename.Text = "Rename";
			//
			//mMenFileDuplicate
			//
			this.mMenFileDuplicate.Image = global::mRemoteNG.My.Resources.Resources.page_copy;
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
			this.mMenFileExit.Image = global::mRemoteNG.My.Resources.Resources.Quit;
			this.mMenFileExit.Name = "mMenFileExit";
			this.mMenFileExit.ShortcutKeys = (System.Windows.Forms.Keys)(System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4);
			this.mMenFileExit.Size = new System.Drawing.Size(281, 22);
			this.mMenFileExit.Text = "Exit";
			//
			//mMenView
			//
			this.mMenView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.mMenViewAddConnectionPanel,
				this.mMenViewConnectionPanels,
				this.mMenViewSep1,
				this.mMenViewConnections,
				this.mMenViewConfig,
				this.mMenViewSessions,
				this.mMenViewErrorsAndInfos,
				this.mMenViewScreenshotManager,
				this.ToolStripSeparator1,
				this.mMenViewJumpTo,
				this.mMenViewResetLayout,
				this.mMenViewSep2,
				this.mMenViewQuickConnectToolbar,
				this.mMenViewExtAppsToolbar,
				this.mMenViewSep3,
				this.mMenViewFullscreen
			});
			this.mMenView.Name = "mMenView";
			this.mMenView.Size = new System.Drawing.Size(44, 20);
			this.mMenView.Text = "&View";
			//
			//mMenViewAddConnectionPanel
			//
			this.mMenViewAddConnectionPanel.Image = global::mRemoteNG.My.Resources.Resources.Panel_Add;
			this.mMenViewAddConnectionPanel.Name = "mMenViewAddConnectionPanel";
			this.mMenViewAddConnectionPanel.Size = new System.Drawing.Size(228, 22);
			this.mMenViewAddConnectionPanel.Text = "Add Connection Panel";
			//
			//mMenViewConnectionPanels
			//
			this.mMenViewConnectionPanels.Image = global::mRemoteNG.My.Resources.Resources.Panels;
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
			this.mMenViewConnections.Image = global::mRemoteNG.My.Resources.Resources.Root;
			this.mMenViewConnections.Name = "mMenViewConnections";
			this.mMenViewConnections.Size = new System.Drawing.Size(228, 22);
			this.mMenViewConnections.Text = "Connections";
			//
			//mMenViewConfig
			//
			this.mMenViewConfig.Checked = true;
			this.mMenViewConfig.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mMenViewConfig.Image = global::mRemoteNG.My.Resources.Resources.cog;
			this.mMenViewConfig.Name = "mMenViewConfig";
			this.mMenViewConfig.Size = new System.Drawing.Size(228, 22);
			this.mMenViewConfig.Text = "Config";
			//
			//mMenViewSessions
			//
			this.mMenViewSessions.Checked = true;
			this.mMenViewSessions.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mMenViewSessions.Image = (System.Drawing.Image)resources.GetObject("mMenViewSessions.Image");
			this.mMenViewSessions.Name = "mMenViewSessions";
			this.mMenViewSessions.Size = new System.Drawing.Size(228, 22);
			this.mMenViewSessions.Text = "Sessions";
			//
			//mMenViewErrorsAndInfos
			//
			this.mMenViewErrorsAndInfos.Checked = true;
			this.mMenViewErrorsAndInfos.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mMenViewErrorsAndInfos.Image = global::mRemoteNG.My.Resources.Resources.ErrorsAndInfos;
			this.mMenViewErrorsAndInfos.Name = "mMenViewErrorsAndInfos";
			this.mMenViewErrorsAndInfos.Size = new System.Drawing.Size(228, 22);
			this.mMenViewErrorsAndInfos.Text = "Errors and Infos";
			//
			//mMenViewScreenshotManager
			//
			this.mMenViewScreenshotManager.Image = (System.Drawing.Image)resources.GetObject("mMenViewScreenshotManager.Image");
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
			this.mMenViewJumpTo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.mMenViewJumpToConnectionsConfig,
				this.mMenViewJumpToSessionsScreenshots,
				this.mMenViewJumpToErrorsInfos
			});
			this.mMenViewJumpTo.Image = global::mRemoteNG.My.Resources.Resources.JumpTo;
			this.mMenViewJumpTo.Name = "mMenViewJumpTo";
			this.mMenViewJumpTo.Size = new System.Drawing.Size(228, 22);
			this.mMenViewJumpTo.Text = "Jump To";
			//
			//mMenViewJumpToConnectionsConfig
			//
			this.mMenViewJumpToConnectionsConfig.Image = global::mRemoteNG.My.Resources.Resources.Root;
			this.mMenViewJumpToConnectionsConfig.Name = "mMenViewJumpToConnectionsConfig";
			this.mMenViewJumpToConnectionsConfig.ShortcutKeys = (System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) | System.Windows.Forms.Keys.C);
			this.mMenViewJumpToConnectionsConfig.Size = new System.Drawing.Size(260, 22);
			this.mMenViewJumpToConnectionsConfig.Text = "Connections && Config";
			//
			//mMenViewJumpToSessionsScreenshots
			//
			this.mMenViewJumpToSessionsScreenshots.Image = global::mRemoteNG.My.Resources.Resources.Sessions;
			this.mMenViewJumpToSessionsScreenshots.Name = "mMenViewJumpToSessionsScreenshots";
			this.mMenViewJumpToSessionsScreenshots.ShortcutKeys = (System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) | System.Windows.Forms.Keys.S);
			this.mMenViewJumpToSessionsScreenshots.Size = new System.Drawing.Size(260, 22);
			this.mMenViewJumpToSessionsScreenshots.Text = "Sessions && Screenshots";
			//
			//mMenViewJumpToErrorsInfos
			//
			this.mMenViewJumpToErrorsInfos.Image = global::mRemoteNG.My.Resources.Resources.InformationSmall;
			this.mMenViewJumpToErrorsInfos.Name = "mMenViewJumpToErrorsInfos";
			this.mMenViewJumpToErrorsInfos.ShortcutKeys = (System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) | System.Windows.Forms.Keys.E);
			this.mMenViewJumpToErrorsInfos.Size = new System.Drawing.Size(260, 22);
			this.mMenViewJumpToErrorsInfos.Text = "Errors && Infos";
			//
			//mMenViewResetLayout
			//
			this.mMenViewResetLayout.Image = global::mRemoteNG.My.Resources.Resources.application_side_tree;
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
			this.mMenViewQuickConnectToolbar.Image = global::mRemoteNG.My.Resources.Resources.Play_Quick;
			this.mMenViewQuickConnectToolbar.Name = "mMenViewQuickConnectToolbar";
			this.mMenViewQuickConnectToolbar.Size = new System.Drawing.Size(228, 22);
			this.mMenViewQuickConnectToolbar.Text = "Quick Connect Toolbar";
			//
			//mMenViewExtAppsToolbar
			//
			this.mMenViewExtAppsToolbar.Image = global::mRemoteNG.My.Resources.Resources.ExtApp;
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
			this.mMenViewFullscreen.Image = global::mRemoteNG.My.Resources.Resources.arrow_out;
			this.mMenViewFullscreen.Name = "mMenViewFullscreen";
			this.mMenViewFullscreen.ShortcutKeys = System.Windows.Forms.Keys.F11;
			this.mMenViewFullscreen.Size = new System.Drawing.Size(228, 22);
			this.mMenViewFullscreen.Text = "Full Screen";
			//
			//mMenTools
			//
			this.mMenTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.mMenToolsSSHTransfer,
				this.mMenToolsUVNCSC,
				this.mMenToolsExternalApps,
				this.mMenToolsPortScan,
				this.mMenToolsSep1,
				this.mMenToolsComponentsCheck,
				this.mMenToolsOptions
			});
			this.mMenTools.Name = "mMenTools";
			this.mMenTools.Size = new System.Drawing.Size(48, 20);
			this.mMenTools.Text = "&Tools";
			//
			//mMenToolsSSHTransfer
			//
			this.mMenToolsSSHTransfer.Image = global::mRemoteNG.My.Resources.Resources.SSHTransfer;
			this.mMenToolsSSHTransfer.Name = "mMenToolsSSHTransfer";
			this.mMenToolsSSHTransfer.Size = new System.Drawing.Size(184, 22);
			this.mMenToolsSSHTransfer.Text = "SSH File Transfer";
			//
			//mMenToolsUVNCSC
			//
			this.mMenToolsUVNCSC.Image = global::mRemoteNG.My.Resources.Resources.UVNC_SC;
			this.mMenToolsUVNCSC.Name = "mMenToolsUVNCSC";
			this.mMenToolsUVNCSC.Size = new System.Drawing.Size(184, 22);
			this.mMenToolsUVNCSC.Text = "UltraVNC SingleClick";
			this.mMenToolsUVNCSC.Visible = false;
			//
			//mMenToolsExternalApps
			//
			this.mMenToolsExternalApps.Image = global::mRemoteNG.My.Resources.Resources.ExtApp;
			this.mMenToolsExternalApps.Name = "mMenToolsExternalApps";
			this.mMenToolsExternalApps.Size = new System.Drawing.Size(184, 22);
			this.mMenToolsExternalApps.Text = "External Applications";
			//
			//mMenToolsPortScan
			//
			this.mMenToolsPortScan.Image = global::mRemoteNG.My.Resources.Resources.PortScan;
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
			this.mMenToolsComponentsCheck.Image = global::mRemoteNG.My.Resources.Resources.cog_error;
			this.mMenToolsComponentsCheck.Name = "mMenToolsComponentsCheck";
			this.mMenToolsComponentsCheck.Size = new System.Drawing.Size(184, 22);
			this.mMenToolsComponentsCheck.Text = "Components Check";
			//
			//mMenToolsOptions
			//
			this.mMenToolsOptions.Image = (System.Drawing.Image)resources.GetObject("mMenToolsOptions.Image");
			this.mMenToolsOptions.Name = "mMenToolsOptions";
			this.mMenToolsOptions.Size = new System.Drawing.Size(184, 22);
			this.mMenToolsOptions.Text = "Options";
			//
			//mMenInfo
			//
			this.mMenInfo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.mMenInfoHelp,
				this.mMenInfoSep1,
				this.mMenInfoWebsite,
				this.mMenInfoDonate,
				this.mMenInfoForum,
				this.mMenInfoBugReport,
				this.ToolStripSeparator2,
				this.mMenInfoAnnouncements,
				this.mMenToolsUpdate,
				this.mMenInfoSep2,
				this.mMenInfoAbout
			});
			this.mMenInfo.Name = "mMenInfo";
			this.mMenInfo.Size = new System.Drawing.Size(44, 20);
			this.mMenInfo.Text = "&Help";
			this.mMenInfo.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
			//
			//mMenInfoHelp
			//
			this.mMenInfoHelp.Image = (System.Drawing.Image)resources.GetObject("mMenInfoHelp.Image");
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
			this.mMenInfoWebsite.Image = (System.Drawing.Image)resources.GetObject("mMenInfoWebsite.Image");
			this.mMenInfoWebsite.Name = "mMenInfoWebsite";
			this.mMenInfoWebsite.Size = new System.Drawing.Size(190, 22);
			this.mMenInfoWebsite.Text = "Website";
			//
			//mMenInfoDonate
			//
			this.mMenInfoDonate.Image = global::mRemoteNG.My.Resources.Resources.Donate;
			this.mMenInfoDonate.Name = "mMenInfoDonate";
			this.mMenInfoDonate.Size = new System.Drawing.Size(190, 22);
			this.mMenInfoDonate.Text = "Donate";
			//
			//mMenInfoForum
			//
			this.mMenInfoForum.Image = global::mRemoteNG.My.Resources.Resources.user_comment;
			this.mMenInfoForum.Name = "mMenInfoForum";
			this.mMenInfoForum.Size = new System.Drawing.Size(190, 22);
			this.mMenInfoForum.Text = "Support Forum";
			//
			//mMenInfoBugReport
			//
			this.mMenInfoBugReport.Image = global::mRemoteNG.My.Resources.Resources.Bug;
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
			this.mMenInfoAnnouncements.Image = global::mRemoteNG.My.Resources.Resources.News;
			this.mMenInfoAnnouncements.Name = "mMenInfoAnnouncements";
			this.mMenInfoAnnouncements.Size = new System.Drawing.Size(190, 22);
			this.mMenInfoAnnouncements.Text = "Announcements";
			//
			//mMenToolsUpdate
			//
			this.mMenToolsUpdate.Image = global::mRemoteNG.My.Resources.Resources.Update;
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
			this.mMenInfoAbout.Image = global::mRemoteNG.My.Resources.Resources.mRemote;
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
			this.tsQuickConnect.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.lblQuickConnect,
				this.cmbQuickConnect,
				this.btnQuickConnect,
				this.btnConnections
			});
			this.tsQuickConnect.Location = new System.Drawing.Point(3, 24);
			this.tsQuickConnect.MaximumSize = new System.Drawing.Size(0, 25);
			this.tsQuickConnect.Name = "tsQuickConnect";
			this.tsQuickConnect.Size = new System.Drawing.Size(387, 25);
			this.tsQuickConnect.TabIndex = 18;
			//
			//btnQuickConnect
			//
			this.btnQuickConnect.DropDown = this.mnuQuickConnectProtocol;
			this.btnQuickConnect.Image = global::mRemoteNG.My.Resources.Resources.Play_Quick;
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
			this.btnConnections.Image = global::mRemoteNG.My.Resources.Resources.Root;
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
			this.cMenExtAppsToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.cMenToolbarShowText });
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
			this.ToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.ToolStripButton1,
				this.ToolStripButton2,
				this.ToolStripButton3,
				this.ToolStripSplitButton1
			});
			this.ToolStrip1.Location = new System.Drawing.Point(3, 74);
			this.ToolStrip1.MaximumSize = new System.Drawing.Size(0, 25);
			this.ToolStrip1.Name = "ToolStrip1";
			this.ToolStrip1.Size = new System.Drawing.Size(0, 25);
			this.ToolStrip1.TabIndex = 19;
			this.ToolStrip1.Visible = false;
			//
			//ToolStripButton1
			//
			this.ToolStripButton1.Image = global::mRemoteNG.My.Resources.Resources.Play;
			this.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ToolStripButton1.Name = "ToolStripButton1";
			this.ToolStripButton1.Size = new System.Drawing.Size(72, 22);
			this.ToolStripButton1.Text = "Connect";
			//
			//ToolStripButton2
			//
			this.ToolStripButton2.Image = global::mRemoteNG.My.Resources.Resources.Screenshot;
			this.ToolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ToolStripButton2.Name = "ToolStripButton2";
			this.ToolStripButton2.Size = new System.Drawing.Size(85, 22);
			this.ToolStripButton2.Text = "Screenshot";
			//
			//ToolStripButton3
			//
			this.ToolStripButton3.Image = global::mRemoteNG.My.Resources.Resources.Refresh;
			this.ToolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ToolStripButton3.Name = "ToolStripButton3";
			this.ToolStripButton3.Size = new System.Drawing.Size(66, 22);
			this.ToolStripButton3.Text = "Refresh";
			//
			//ToolStripSplitButton1
			//
			this.ToolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ToolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.ToolStripMenuItem1,
				this.ToolStripMenuItem2
			});
			this.ToolStripSplitButton1.Image = global::mRemoteNG.My.Resources.Resources.Keyboard;
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
			this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(842, 523);
			this.Controls.Add(this.tsContainer);
			this.Icon = global::mRemoteNG.My.Resources.Resources.mRemote_Icon;
			this.MainMenuStrip = this.msMain;
			this.Name = "frmMain";
			this.Opacity = 0.0;
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
		private WeifenLuo.WinFormsUI.Docking.DockPanel withEventsField_pnlDock;
		internal WeifenLuo.WinFormsUI.Docking.DockPanel pnlDock {
			get { return withEventsField_pnlDock; }
			set {
				if (withEventsField_pnlDock != null) {
					withEventsField_pnlDock.ActiveDocumentChanged -= pnlDock_ActiveDocumentChanged;
				}
				withEventsField_pnlDock = value;
				if (withEventsField_pnlDock != null) {
					withEventsField_pnlDock.ActiveDocumentChanged += pnlDock_ActiveDocumentChanged;
				}
			}
		}
		internal System.Windows.Forms.MenuStrip msMain;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenFile;
		internal System.Windows.Forms.ToolStripMenuItem mMenFile {
			get { return withEventsField_mMenFile; }
			set {
				if (withEventsField_mMenFile != null) {
					withEventsField_mMenFile.DropDownOpening -= mMenFile_DropDownOpening;
				}
				withEventsField_mMenFile = value;
				if (withEventsField_mMenFile != null) {
					withEventsField_mMenFile.DropDownOpening += mMenFile_DropDownOpening;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenView;
		internal System.Windows.Forms.ToolStripMenuItem mMenView {
			get { return withEventsField_mMenView; }
			set {
				if (withEventsField_mMenView != null) {
					withEventsField_mMenView.DropDownOpening -= mMenView_DropDownOpening;
				}
				withEventsField_mMenView = value;
				if (withEventsField_mMenView != null) {
					withEventsField_mMenView.DropDownOpening += mMenView_DropDownOpening;
				}
			}
		}
		internal System.Windows.Forms.ToolStripMenuItem mMenTools;
		private System.Windows.Forms.ToolStripLabel withEventsField_lblQuickConnect;
		internal System.Windows.Forms.ToolStripLabel lblQuickConnect {
			get { return withEventsField_lblQuickConnect; }
			set {
				if (withEventsField_lblQuickConnect != null) {
					withEventsField_lblQuickConnect.Click -= lblQuickConnect_Click;
				}
				withEventsField_lblQuickConnect = value;
				if (withEventsField_lblQuickConnect != null) {
					withEventsField_lblQuickConnect.Click += lblQuickConnect_Click;
				}
			}
		}
		internal System.Windows.Forms.ToolStripMenuItem mMenInfo;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenFileNew;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileNew {
			get { return withEventsField_mMenFileNew; }
			set {
				if (withEventsField_mMenFileNew != null) {
					withEventsField_mMenFileNew.Click -= mMenFileNew_Click;
				}
				withEventsField_mMenFileNew = value;
				if (withEventsField_mMenFileNew != null) {
					withEventsField_mMenFileNew.Click += mMenFileNew_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenFileLoad;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileLoad {
			get { return withEventsField_mMenFileLoad; }
			set {
				if (withEventsField_mMenFileLoad != null) {
					withEventsField_mMenFileLoad.Click -= mMenFileLoad_Click;
				}
				withEventsField_mMenFileLoad = value;
				if (withEventsField_mMenFileLoad != null) {
					withEventsField_mMenFileLoad.Click += mMenFileLoad_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenFileSave;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileSave {
			get { return withEventsField_mMenFileSave; }
			set {
				if (withEventsField_mMenFileSave != null) {
					withEventsField_mMenFileSave.Click -= mMenFileSave_Click;
				}
				withEventsField_mMenFileSave = value;
				if (withEventsField_mMenFileSave != null) {
					withEventsField_mMenFileSave.Click += mMenFileSave_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenFileSaveAs;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileSaveAs {
			get { return withEventsField_mMenFileSaveAs; }
			set {
				if (withEventsField_mMenFileSaveAs != null) {
					withEventsField_mMenFileSaveAs.Click -= mMenFileSaveAs_Click;
				}
				withEventsField_mMenFileSaveAs = value;
				if (withEventsField_mMenFileSaveAs != null) {
					withEventsField_mMenFileSaveAs.Click += mMenFileSaveAs_Click;
				}
			}
		}
		internal System.Windows.Forms.ToolStripSeparator mMenFileSep1;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenFileExit;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileExit {
			get { return withEventsField_mMenFileExit; }
			set {
				if (withEventsField_mMenFileExit != null) {
					withEventsField_mMenFileExit.Click -= mMenFileExit_Click;
				}
				withEventsField_mMenFileExit = value;
				if (withEventsField_mMenFileExit != null) {
					withEventsField_mMenFileExit.Click += mMenFileExit_Click;
				}
			}
		}
		internal System.Windows.Forms.ToolStripSeparator mMenToolsSep1;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenToolsOptions;
		internal System.Windows.Forms.ToolStripMenuItem mMenToolsOptions {
			get { return withEventsField_mMenToolsOptions; }
			set {
				if (withEventsField_mMenToolsOptions != null) {
					withEventsField_mMenToolsOptions.Click -= mMenToolsOptions_Click;
				}
				withEventsField_mMenToolsOptions = value;
				if (withEventsField_mMenToolsOptions != null) {
					withEventsField_mMenToolsOptions.Click += mMenToolsOptions_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenInfoHelp;
		internal System.Windows.Forms.ToolStripMenuItem mMenInfoHelp {
			get { return withEventsField_mMenInfoHelp; }
			set {
				if (withEventsField_mMenInfoHelp != null) {
					withEventsField_mMenInfoHelp.Click -= mMenInfoHelp_Click;
				}
				withEventsField_mMenInfoHelp = value;
				if (withEventsField_mMenInfoHelp != null) {
					withEventsField_mMenInfoHelp.Click += mMenInfoHelp_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenInfoWebsite;
		internal System.Windows.Forms.ToolStripMenuItem mMenInfoWebsite {
			get { return withEventsField_mMenInfoWebsite; }
			set {
				if (withEventsField_mMenInfoWebsite != null) {
					withEventsField_mMenInfoWebsite.Click -= mMenInfoWebsite_Click;
				}
				withEventsField_mMenInfoWebsite = value;
				if (withEventsField_mMenInfoWebsite != null) {
					withEventsField_mMenInfoWebsite.Click += mMenInfoWebsite_Click;
				}
			}
		}
		internal System.Windows.Forms.ToolStripSeparator mMenInfoSep1;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenInfoAbout;
		internal System.Windows.Forms.ToolStripMenuItem mMenInfoAbout {
			get { return withEventsField_mMenInfoAbout; }
			set {
				if (withEventsField_mMenInfoAbout != null) {
					withEventsField_mMenInfoAbout.Click -= mMenInfoAbout_Click;
				}
				withEventsField_mMenInfoAbout = value;
				if (withEventsField_mMenInfoAbout != null) {
					withEventsField_mMenInfoAbout.Click += mMenInfoAbout_Click;
				}
			}
		}
		internal System.Windows.Forms.ToolStripMenuItem mMenViewConnectionPanels;
		internal System.Windows.Forms.ToolStripSeparator mMenViewSep1;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenViewSessions;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewSessions {
			get { return withEventsField_mMenViewSessions; }
			set {
				if (withEventsField_mMenViewSessions != null) {
					withEventsField_mMenViewSessions.Click -= mMenViewSessions_Click;
				}
				withEventsField_mMenViewSessions = value;
				if (withEventsField_mMenViewSessions != null) {
					withEventsField_mMenViewSessions.Click += mMenViewSessions_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenViewConnections;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewConnections {
			get { return withEventsField_mMenViewConnections; }
			set {
				if (withEventsField_mMenViewConnections != null) {
					withEventsField_mMenViewConnections.Click -= mMenViewConnections_Click;
				}
				withEventsField_mMenViewConnections = value;
				if (withEventsField_mMenViewConnections != null) {
					withEventsField_mMenViewConnections.Click += mMenViewConnections_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenViewConfig;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewConfig {
			get { return withEventsField_mMenViewConfig; }
			set {
				if (withEventsField_mMenViewConfig != null) {
					withEventsField_mMenViewConfig.Click -= mMenViewConfig_Click;
				}
				withEventsField_mMenViewConfig = value;
				if (withEventsField_mMenViewConfig != null) {
					withEventsField_mMenViewConfig.Click += mMenViewConfig_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenViewErrorsAndInfos;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewErrorsAndInfos {
			get { return withEventsField_mMenViewErrorsAndInfos; }
			set {
				if (withEventsField_mMenViewErrorsAndInfos != null) {
					withEventsField_mMenViewErrorsAndInfos.Click -= mMenViewErrorsAndInfos_Click;
				}
				withEventsField_mMenViewErrorsAndInfos = value;
				if (withEventsField_mMenViewErrorsAndInfos != null) {
					withEventsField_mMenViewErrorsAndInfos.Click += mMenViewErrorsAndInfos_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenViewScreenshotManager;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewScreenshotManager {
			get { return withEventsField_mMenViewScreenshotManager; }
			set {
				if (withEventsField_mMenViewScreenshotManager != null) {
					withEventsField_mMenViewScreenshotManager.Click -= mMenViewScreenshotManager_Click;
				}
				withEventsField_mMenViewScreenshotManager = value;
				if (withEventsField_mMenViewScreenshotManager != null) {
					withEventsField_mMenViewScreenshotManager.Click += mMenViewScreenshotManager_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenViewAddConnectionPanel;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewAddConnectionPanel {
			get { return withEventsField_mMenViewAddConnectionPanel; }
			set {
				if (withEventsField_mMenViewAddConnectionPanel != null) {
					withEventsField_mMenViewAddConnectionPanel.Click -= mMenViewAddConnectionPanel_Click;
				}
				withEventsField_mMenViewAddConnectionPanel = value;
				if (withEventsField_mMenViewAddConnectionPanel != null) {
					withEventsField_mMenViewAddConnectionPanel.Click += mMenViewAddConnectionPanel_Click;
				}
			}
		}
		private Controls.QuickConnectComboBox withEventsField_cmbQuickConnect;
		internal Controls.QuickConnectComboBox cmbQuickConnect {
			get { return withEventsField_cmbQuickConnect; }
			set {
				if (withEventsField_cmbQuickConnect != null) {
					withEventsField_cmbQuickConnect.ConnectRequested -= cmbQuickConnect_ConnectRequested;
					withEventsField_cmbQuickConnect.ProtocolChanged -= cmbQuickConnect_ProtocolChanged;
				}
				withEventsField_cmbQuickConnect = value;
				if (withEventsField_cmbQuickConnect != null) {
					withEventsField_cmbQuickConnect.ConnectRequested += cmbQuickConnect_ConnectRequested;
					withEventsField_cmbQuickConnect.ProtocolChanged += cmbQuickConnect_ProtocolChanged;
				}
			}
		}
		internal System.Windows.Forms.ToolStripSeparator mMenViewSep2;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenViewFullscreen;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewFullscreen {
			get { return withEventsField_mMenViewFullscreen; }
			set {
				if (withEventsField_mMenViewFullscreen != null) {
					withEventsField_mMenViewFullscreen.Click -= mMenViewFullscreen_Click;
				}
				withEventsField_mMenViewFullscreen = value;
				if (withEventsField_mMenViewFullscreen != null) {
					withEventsField_mMenViewFullscreen.Click += mMenViewFullscreen_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenToolsSSHTransfer;
		internal System.Windows.Forms.ToolStripMenuItem mMenToolsSSHTransfer {
			get { return withEventsField_mMenToolsSSHTransfer; }
			set {
				if (withEventsField_mMenToolsSSHTransfer != null) {
					withEventsField_mMenToolsSSHTransfer.Click -= mMenToolsSSHTransfer_Click;
				}
				withEventsField_mMenToolsSSHTransfer = value;
				if (withEventsField_mMenToolsSSHTransfer != null) {
					withEventsField_mMenToolsSSHTransfer.Click += mMenToolsSSHTransfer_Click;
				}
			}
		}
		internal System.Windows.Forms.ToolStripContainer tsContainer;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenToolsExternalApps;
		internal System.Windows.Forms.ToolStripMenuItem mMenToolsExternalApps {
			get { return withEventsField_mMenToolsExternalApps; }
			set {
				if (withEventsField_mMenToolsExternalApps != null) {
					withEventsField_mMenToolsExternalApps.Click -= mMenToolsExternalApps_Click;
				}
				withEventsField_mMenToolsExternalApps = value;
				if (withEventsField_mMenToolsExternalApps != null) {
					withEventsField_mMenToolsExternalApps.Click += mMenToolsExternalApps_Click;
				}
			}
		}
		private System.Windows.Forms.Timer withEventsField_tmrAutoSave;
		internal System.Windows.Forms.Timer tmrAutoSave {
			get { return withEventsField_tmrAutoSave; }
			set {
				if (withEventsField_tmrAutoSave != null) {
					withEventsField_tmrAutoSave.Tick -= tmrAutoSave_Tick;
				}
				withEventsField_tmrAutoSave = value;
				if (withEventsField_tmrAutoSave != null) {
					withEventsField_tmrAutoSave.Tick += tmrAutoSave_Tick;
				}
			}
		}
		internal System.Windows.Forms.ToolStrip tsExternalTools;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenViewExtAppsToolbar;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewExtAppsToolbar {
			get { return withEventsField_mMenViewExtAppsToolbar; }
			set {
				if (withEventsField_mMenViewExtAppsToolbar != null) {
					withEventsField_mMenViewExtAppsToolbar.Click -= mMenViewExtAppsToolbar_Click;
				}
				withEventsField_mMenViewExtAppsToolbar = value;
				if (withEventsField_mMenViewExtAppsToolbar != null) {
					withEventsField_mMenViewExtAppsToolbar.Click += mMenViewExtAppsToolbar_Click;
				}
			}
		}
		internal System.Windows.Forms.ContextMenuStrip cMenExtAppsToolbar;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_cMenToolbarShowText;
		internal System.Windows.Forms.ToolStripMenuItem cMenToolbarShowText {
			get { return withEventsField_cMenToolbarShowText; }
			set {
				if (withEventsField_cMenToolbarShowText != null) {
					withEventsField_cMenToolbarShowText.Click -= cMenToolbarShowText_Click;
				}
				withEventsField_cMenToolbarShowText = value;
				if (withEventsField_cMenToolbarShowText != null) {
					withEventsField_cMenToolbarShowText.Click += cMenToolbarShowText_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenToolsPortScan;
		internal System.Windows.Forms.ToolStripMenuItem mMenToolsPortScan {
			get { return withEventsField_mMenToolsPortScan; }
			set {
				if (withEventsField_mMenToolsPortScan != null) {
					withEventsField_mMenToolsPortScan.Click -= mMenToolsPortScan_Click;
				}
				withEventsField_mMenToolsPortScan = value;
				if (withEventsField_mMenToolsPortScan != null) {
					withEventsField_mMenToolsPortScan.Click += mMenToolsPortScan_Click;
				}
			}
		}
		internal System.Windows.Forms.ToolStrip tsQuickConnect;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenViewQuickConnectToolbar;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewQuickConnectToolbar {
			get { return withEventsField_mMenViewQuickConnectToolbar; }
			set {
				if (withEventsField_mMenViewQuickConnectToolbar != null) {
					withEventsField_mMenViewQuickConnectToolbar.Click -= mMenViewQuickConnectToolbar_Click;
				}
				withEventsField_mMenViewQuickConnectToolbar = value;
				if (withEventsField_mMenViewQuickConnectToolbar != null) {
					withEventsField_mMenViewQuickConnectToolbar.Click += mMenViewQuickConnectToolbar_Click;
				}
			}
		}
		internal System.Windows.Forms.ToolStripSeparator mMenSep3;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenInfoDonate;
		internal System.Windows.Forms.ToolStripMenuItem mMenInfoDonate {
			get { return withEventsField_mMenInfoDonate; }
			set {
				if (withEventsField_mMenInfoDonate != null) {
					withEventsField_mMenInfoDonate.Click -= mMenInfoDonate_Click;
				}
				withEventsField_mMenInfoDonate = value;
				if (withEventsField_mMenInfoDonate != null) {
					withEventsField_mMenInfoDonate.Click += mMenInfoDonate_Click;
				}
			}
		}
		internal System.Windows.Forms.ToolStripSeparator mMenViewSep3;
		private Controls.ToolStripSplitButton withEventsField_btnQuickConnect;
		internal Controls.ToolStripSplitButton btnQuickConnect {
			get { return withEventsField_btnQuickConnect; }
			set {
				if (withEventsField_btnQuickConnect != null) {
					withEventsField_btnQuickConnect.ButtonClick -= btnQuickConnect_ButtonClick;
					withEventsField_btnQuickConnect.DropDownItemClicked -= btnQuickConnect_DropDownItemClicked;
				}
				withEventsField_btnQuickConnect = value;
				if (withEventsField_btnQuickConnect != null) {
					withEventsField_btnQuickConnect.ButtonClick += btnQuickConnect_ButtonClick;
					withEventsField_btnQuickConnect.DropDownItemClicked += btnQuickConnect_DropDownItemClicked;
				}
			}
		}
		internal System.Windows.Forms.ToolStripMenuItem mMenViewJumpTo;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenViewJumpToConnectionsConfig;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewJumpToConnectionsConfig {
			get { return withEventsField_mMenViewJumpToConnectionsConfig; }
			set {
				if (withEventsField_mMenViewJumpToConnectionsConfig != null) {
					withEventsField_mMenViewJumpToConnectionsConfig.Click -= mMenViewJumpToConnectionsConfig_Click;
				}
				withEventsField_mMenViewJumpToConnectionsConfig = value;
				if (withEventsField_mMenViewJumpToConnectionsConfig != null) {
					withEventsField_mMenViewJumpToConnectionsConfig.Click += mMenViewJumpToConnectionsConfig_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenViewJumpToSessionsScreenshots;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewJumpToSessionsScreenshots {
			get { return withEventsField_mMenViewJumpToSessionsScreenshots; }
			set {
				if (withEventsField_mMenViewJumpToSessionsScreenshots != null) {
					withEventsField_mMenViewJumpToSessionsScreenshots.Click -= mMenViewJumpToSessionsScreenshots_Click;
				}
				withEventsField_mMenViewJumpToSessionsScreenshots = value;
				if (withEventsField_mMenViewJumpToSessionsScreenshots != null) {
					withEventsField_mMenViewJumpToSessionsScreenshots.Click += mMenViewJumpToSessionsScreenshots_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenViewJumpToErrorsInfos;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewJumpToErrorsInfos {
			get { return withEventsField_mMenViewJumpToErrorsInfos; }
			set {
				if (withEventsField_mMenViewJumpToErrorsInfos != null) {
					withEventsField_mMenViewJumpToErrorsInfos.Click -= mMenViewJumpToErrorsInfos_Click;
				}
				withEventsField_mMenViewJumpToErrorsInfos = value;
				if (withEventsField_mMenViewJumpToErrorsInfos != null) {
					withEventsField_mMenViewJumpToErrorsInfos.Click += mMenViewJumpToErrorsInfos_Click;
				}
			}
		}
		internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenToolsUVNCSC;
		internal System.Windows.Forms.ToolStripMenuItem mMenToolsUVNCSC {
			get { return withEventsField_mMenToolsUVNCSC; }
			set {
				if (withEventsField_mMenToolsUVNCSC != null) {
					withEventsField_mMenToolsUVNCSC.Click -= mMenToolsUVNCSC_Click;
				}
				withEventsField_mMenToolsUVNCSC = value;
				if (withEventsField_mMenToolsUVNCSC != null) {
					withEventsField_mMenToolsUVNCSC.Click += mMenToolsUVNCSC_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenToolsComponentsCheck;
		internal System.Windows.Forms.ToolStripMenuItem mMenToolsComponentsCheck {
			get { return withEventsField_mMenToolsComponentsCheck; }
			set {
				if (withEventsField_mMenToolsComponentsCheck != null) {
					withEventsField_mMenToolsComponentsCheck.Click -= mMenToolsComponentsCheck_Click;
				}
				withEventsField_mMenToolsComponentsCheck = value;
				if (withEventsField_mMenToolsComponentsCheck != null) {
					withEventsField_mMenToolsComponentsCheck.Click += mMenToolsComponentsCheck_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenInfoAnnouncements;
		internal System.Windows.Forms.ToolStripMenuItem mMenInfoAnnouncements {
			get { return withEventsField_mMenInfoAnnouncements; }
			set {
				if (withEventsField_mMenInfoAnnouncements != null) {
					withEventsField_mMenInfoAnnouncements.Click -= mMenInfoAnnouncements_Click;
				}
				withEventsField_mMenInfoAnnouncements = value;
				if (withEventsField_mMenInfoAnnouncements != null) {
					withEventsField_mMenInfoAnnouncements.Click += mMenInfoAnnouncements_Click;
				}
			}
		}
		internal System.Windows.Forms.ToolStripSeparator mMenInfoSep2;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenInfoBugReport;
		internal System.Windows.Forms.ToolStripMenuItem mMenInfoBugReport {
			get { return withEventsField_mMenInfoBugReport; }
			set {
				if (withEventsField_mMenInfoBugReport != null) {
					withEventsField_mMenInfoBugReport.Click -= mMenInfoBugReport_Click;
				}
				withEventsField_mMenInfoBugReport = value;
				if (withEventsField_mMenInfoBugReport != null) {
					withEventsField_mMenInfoBugReport.Click += mMenInfoBugReport_Click;
				}
			}
		}
		internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenInfoForum;
		internal System.Windows.Forms.ToolStripMenuItem mMenInfoForum {
			get { return withEventsField_mMenInfoForum; }
			set {
				if (withEventsField_mMenInfoForum != null) {
					withEventsField_mMenInfoForum.Click -= mMenInfoForum_Click;
				}
				withEventsField_mMenInfoForum = value;
				if (withEventsField_mMenInfoForum != null) {
					withEventsField_mMenInfoForum.Click += mMenInfoForum_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenToolsUpdate;
		internal System.Windows.Forms.ToolStripMenuItem mMenToolsUpdate {
			get { return withEventsField_mMenToolsUpdate; }
			set {
				if (withEventsField_mMenToolsUpdate != null) {
					withEventsField_mMenToolsUpdate.Click -= mMenToolsUpdate_Click;
				}
				withEventsField_mMenToolsUpdate = value;
				if (withEventsField_mMenToolsUpdate != null) {
					withEventsField_mMenToolsUpdate.Click += mMenToolsUpdate_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenViewResetLayout;
		internal System.Windows.Forms.ToolStripMenuItem mMenViewResetLayout {
			get { return withEventsField_mMenViewResetLayout; }
			set {
				if (withEventsField_mMenViewResetLayout != null) {
					withEventsField_mMenViewResetLayout.Click -= mMenViewResetLayout_Click;
				}
				withEventsField_mMenViewResetLayout = value;
				if (withEventsField_mMenViewResetLayout != null) {
					withEventsField_mMenViewResetLayout.Click += mMenViewResetLayout_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenFileDuplicate;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileDuplicate {
			get { return withEventsField_mMenFileDuplicate; }
			set {
				if (withEventsField_mMenFileDuplicate != null) {
					withEventsField_mMenFileDuplicate.Click -= mMenFileDuplicate_Click;
				}
				withEventsField_mMenFileDuplicate = value;
				if (withEventsField_mMenFileDuplicate != null) {
					withEventsField_mMenFileDuplicate.Click += mMenFileDuplicate_Click;
				}
			}
		}
		internal System.Windows.Forms.ToolStripSeparator mMenFileSep2;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenFileNewConnection;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileNewConnection {
			get { return withEventsField_mMenFileNewConnection; }
			set {
				if (withEventsField_mMenFileNewConnection != null) {
					withEventsField_mMenFileNewConnection.Click -= mMenFileNewConnection_Click;
				}
				withEventsField_mMenFileNewConnection = value;
				if (withEventsField_mMenFileNewConnection != null) {
					withEventsField_mMenFileNewConnection.Click += mMenFileNewConnection_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenFileNewFolder;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileNewFolder {
			get { return withEventsField_mMenFileNewFolder; }
			set {
				if (withEventsField_mMenFileNewFolder != null) {
					withEventsField_mMenFileNewFolder.Click -= mMenFileNewFolder_Click;
				}
				withEventsField_mMenFileNewFolder = value;
				if (withEventsField_mMenFileNewFolder != null) {
					withEventsField_mMenFileNewFolder.Click += mMenFileNewFolder_Click;
				}
			}
		}
		internal System.Windows.Forms.ToolStripSeparator mMenFileSep3;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenFileDelete;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileDelete {
			get { return withEventsField_mMenFileDelete; }
			set {
				if (withEventsField_mMenFileDelete != null) {
					withEventsField_mMenFileDelete.Click -= mMenFileDelete_Click;
				}
				withEventsField_mMenFileDelete = value;
				if (withEventsField_mMenFileDelete != null) {
					withEventsField_mMenFileDelete.Click += mMenFileDelete_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenFileRename;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileRename {
			get { return withEventsField_mMenFileRename; }
			set {
				if (withEventsField_mMenFileRename != null) {
					withEventsField_mMenFileRename.Click -= mMenFileRename_Click;
				}
				withEventsField_mMenFileRename = value;
				if (withEventsField_mMenFileRename != null) {
					withEventsField_mMenFileRename.Click += mMenFileRename_Click;
				}
			}
		}
		internal System.Windows.Forms.ToolStripSeparator mMenFileSep4;
		internal System.Windows.Forms.ToolStrip ToolStrip1;
		internal System.Windows.Forms.ToolStripButton ToolStripButton1;
		internal System.Windows.Forms.ToolStripButton ToolStripButton2;
		internal System.Windows.Forms.ToolStripButton ToolStripButton3;
		internal System.Windows.Forms.ToolStripDropDownButton ToolStripSplitButton1;
		internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem1;
		internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem2;
		internal System.Windows.Forms.ContextMenuStrip mnuQuickConnectProtocol;
		private System.Windows.Forms.ToolStripDropDownButton withEventsField_btnConnections;
		internal System.Windows.Forms.ToolStripDropDownButton btnConnections {
			get { return withEventsField_btnConnections; }
			set {
				if (withEventsField_btnConnections != null) {
					withEventsField_btnConnections.DropDownOpening -= btnConnections_DropDownOpening;
				}
				withEventsField_btnConnections = value;
				if (withEventsField_btnConnections != null) {
					withEventsField_btnConnections.DropDownOpening += btnConnections_DropDownOpening;
				}
			}
		}
		internal System.Windows.Forms.ContextMenuStrip mnuConnections;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenFileExport;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileExport {
			get { return withEventsField_mMenFileExport; }
			set {
				if (withEventsField_mMenFileExport != null) {
					withEventsField_mMenFileExport.Click -= mMenFileExport_Click;
				}
				withEventsField_mMenFileExport = value;
				if (withEventsField_mMenFileExport != null) {
					withEventsField_mMenFileExport.Click += mMenFileExport_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenFileImportFromFile;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileImportFromFile {
			get { return withEventsField_mMenFileImportFromFile; }
			set {
				if (withEventsField_mMenFileImportFromFile != null) {
					withEventsField_mMenFileImportFromFile.Click -= mMenFileImportFromFile_Click;
				}
				withEventsField_mMenFileImportFromFile = value;
				if (withEventsField_mMenFileImportFromFile != null) {
					withEventsField_mMenFileImportFromFile.Click += mMenFileImportFromFile_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenFileImportFromActiveDirectory;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileImportFromActiveDirectory {
			get { return withEventsField_mMenFileImportFromActiveDirectory; }
			set {
				if (withEventsField_mMenFileImportFromActiveDirectory != null) {
					withEventsField_mMenFileImportFromActiveDirectory.Click -= mMenFileImportFromActiveDirectory_Click;
				}
				withEventsField_mMenFileImportFromActiveDirectory = value;
				if (withEventsField_mMenFileImportFromActiveDirectory != null) {
					withEventsField_mMenFileImportFromActiveDirectory.Click += mMenFileImportFromActiveDirectory_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenFileImportFromPortScan;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileImportFromPortScan {
			get { return withEventsField_mMenFileImportFromPortScan; }
			set {
				if (withEventsField_mMenFileImportFromPortScan != null) {
					withEventsField_mMenFileImportFromPortScan.Click -= mMenFileImportFromPortScan_Click;
				}
				withEventsField_mMenFileImportFromPortScan = value;
				if (withEventsField_mMenFileImportFromPortScan != null) {
					withEventsField_mMenFileImportFromPortScan.Click += mMenFileImportFromPortScan_Click;
				}
			}
		}

		internal System.Windows.Forms.ToolStripMenuItem mMenFileImport;
		public frmMain()
		{
			ResizeEnd += frmMain_ResizeEnd;
			Resize += frmMain_Resize;
			ResizeBegin += frmMain_ResizeBegin;
			FormClosing += frmMain_FormClosing;
			Shown += frmMain_Shown;
			Load += frmMain_Load;
			InitializeComponent();
		}
	}
}
