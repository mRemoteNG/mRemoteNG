using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using mRemoteNG.App;
using mRemoteNG.My;
using SharedLibraryNG;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.App.Runtime;
using System.Reflection;
using System.Runtime.InteropServices;
using Crownwood;
using mRemoteNG.App.Native;
using PSTaskDialog;
using mRemoteNG.Config;
using mRemoteNG.Themes;
namespace mRemoteNG
{

	public partial class frmMain
	{
		private FormWindowState _previousWindowState;
		public FormWindowState PreviousWindowState {
			get { return _previousWindowState; }
			set { _previousWindowState = value; }
		}
		public static event clipboardchangeEventHandler clipboardchange;
		public static delegate void clipboardchangeEventHandler();

		private IntPtr fpChainedWindowHandle;
		#region "Properties"
		private bool _isClosing = false;
		public bool IsClosing {
			get { return _isClosing; }
		}

		private bool _usingSqlServer = false;
		public bool UsingSqlServer {
			get { return _usingSqlServer; }
			set {
				if (_usingSqlServer == value)
					return;
				_usingSqlServer = value;
				UpdateWindowTitle();
			}
		}

		private string _connectionsFileName = null;
		public string ConnectionsFileName {
			get { return _connectionsFileName; }
			set {
				if (_connectionsFileName == value)
					return;
				_connectionsFileName = value;
				UpdateWindowTitle();
			}
		}

		private bool _showFullPathInTitle = mRemoteNG.My.Settings.ShowCompleteConsPathInTitle;
		public bool ShowFullPathInTitle {
			get { return _showFullPathInTitle; }
			set {
				if (_showFullPathInTitle == value)
					return;
				_showFullPathInTitle = value;
				UpdateWindowTitle();
			}
		}

		private Connection.Info _selectedConnection = null;
		public Connection.Info SelectedConnection {
			get { return _selectedConnection; }
			set {
				if (object.ReferenceEquals(_selectedConnection, value))
					return;
				_selectedConnection = value;
				UpdateWindowTitle();
			}
		}
		#endregion

		#region "Startup & Shutdown"
		private void frmMain_Load(object sender, System.EventArgs e)
		{
			Runtime.MainForm = this;

			Startup.CheckCompatibility();

			Startup.CreateLogger();

			// Create gui config load and save objects
			Config.Settings.Load SettingsLoad = new Config.Settings.Load(this);

			// Load GUI Configuration
			SettingsLoad.Load();

			Debug.Print("---------------------------" + Constants.vbNewLine + "[START] - " + DateAndTime.Now);

			Startup.ParseCommandLineArgs();

			ApplyLanguage();
			PopulateQuickConnectProtocolMenu();

			ThemeManager.ThemeChanged += ApplyThemes;
			ApplyThemes();

			fpChainedWindowHandle = Native.SetClipboardViewer(this.Handle);

			Runtime.MessageCollector = new Messages.Collector(Windows.errorsForm);

			Runtime.WindowList = new UI.Window.List();

			mRemoteNG.Tools.IeBrowserEmulation.Register();

			Startup.GetConnectionIcons();
			Windows.treePanel.Focus();

			mRemoteNG.Tree.Node.TreeView = Windows.treeForm.tvConnections;

			if (mRemoteNG.My.Settings.FirstStart & !mRemoteNG.My.Settings.LoadConsFromCustomLocation & !System.IO.File.Exists(Runtime.GetStartupConnectionFileName())) {
				Runtime.NewConnections(Runtime.GetStartupConnectionFileName());
			}

			//LoadCredentials()
			Runtime.LoadConnections();
			if (!Runtime.IsConnectionsFileLoaded) {
				System.Windows.Forms.Application.Exit();
				return;
			}

			mRemoteNG.Config.Putty.Sessions.StartWatcher();

			if (mRemoteNG.My.Settings.StartupComponentsCheck) {
				Windows.Show(mRemoteNG.UI.Window.Type.ComponentsCheck);
			}

			#if PORTABLE
			mMenInfoAnnouncements.Visible = false;
			mMenToolsUpdate.Visible = false;
			mMenInfoSep2.Visible = false;
			#endif

			Startup.CreateSQLUpdateHandlerAndStartTimer();

			AddSysMenuItems();
			Microsoft.Win32.SystemEvents.DisplaySettingsChanged += DisplayChanged;

			this.Opacity = 1;

			KeyboardShortcuts.RequestKeyNotifications(Handle);
		}

		private void ApplyLanguage()
		{
			mMenFile.Text = mRemoteNG.My.Language.strMenuFile;
			mMenFileNew.Text = mRemoteNG.My.Language.strMenuNewConnectionFile;
			mMenFileNewConnection.Text = mRemoteNG.My.Language.strNewConnection;
			mMenFileNewFolder.Text = mRemoteNG.My.Language.strNewFolder;
			mMenFileLoad.Text = mRemoteNG.My.Language.strMenuOpenConnectionFile;
			mMenFileSave.Text = mRemoteNG.My.Language.strMenuSaveConnectionFile;
			mMenFileSaveAs.Text = mRemoteNG.My.Language.strMenuSaveConnectionFileAs;
			mMenFileImport.Text = Language.strImportMenuItem;
			mMenFileImportFromFile.Text = Language.strImportFromFileMenuItem;
			mMenFileImportFromActiveDirectory.Text = Language.strImportAD;
			mMenFileImportFromPortScan.Text = Language.strImportPortScan;
			mMenFileExport.Text = Language.strExportToFileMenuItem;
			mMenFileExit.Text = mRemoteNG.My.Language.strMenuExit;

			mMenView.Text = mRemoteNG.My.Language.strMenuView;
			mMenViewAddConnectionPanel.Text = mRemoteNG.My.Language.strMenuAddConnectionPanel;
			mMenViewConnectionPanels.Text = mRemoteNG.My.Language.strMenuConnectionPanels;
			mMenViewConnections.Text = mRemoteNG.My.Language.strMenuConnections;
			mMenViewConfig.Text = mRemoteNG.My.Language.strMenuConfig;
			mMenViewSessions.Text = mRemoteNG.My.Language.strMenuSessions;
			mMenViewErrorsAndInfos.Text = mRemoteNG.My.Language.strMenuNotifications;
			mMenViewScreenshotManager.Text = mRemoteNG.My.Language.strScreenshots;
			mMenViewJumpTo.Text = mRemoteNG.My.Language.strMenuJumpTo;
			mMenViewJumpToConnectionsConfig.Text = mRemoteNG.My.Language.strMenuConnectionsAndConfig;
			mMenViewJumpToSessionsScreenshots.Text = mRemoteNG.My.Language.strMenuSessionsAndScreenshots;
			mMenViewJumpToErrorsInfos.Text = mRemoteNG.My.Language.strMenuNotifications;
			mMenViewResetLayout.Text = mRemoteNG.My.Language.strMenuResetLayout;
			mMenViewQuickConnectToolbar.Text = mRemoteNG.My.Language.strMenuQuickConnectToolbar;
			mMenViewExtAppsToolbar.Text = mRemoteNG.My.Language.strMenuExternalToolsToolbar;
			mMenViewFullscreen.Text = mRemoteNG.My.Language.strMenuFullScreen;

			mMenTools.Text = mRemoteNG.My.Language.strMenuTools;
			mMenToolsSSHTransfer.Text = mRemoteNG.My.Language.strMenuSSHFileTransfer;
			mMenToolsExternalApps.Text = mRemoteNG.My.Language.strMenuExternalTools;
			mMenToolsPortScan.Text = mRemoteNG.My.Language.strMenuPortScan;
			mMenToolsComponentsCheck.Text = mRemoteNG.My.Language.strComponentsCheck;
			mMenToolsUpdate.Text = mRemoteNG.My.Language.strMenuCheckForUpdates;
			mMenToolsOptions.Text = mRemoteNG.My.Language.strMenuOptions;

			mMenInfo.Text = mRemoteNG.My.Language.strMenuHelp;
			mMenInfoHelp.Text = mRemoteNG.My.Language.strMenuHelpContents;
			mMenInfoForum.Text = mRemoteNG.My.Language.strMenuSupportForum;
			mMenInfoBugReport.Text = mRemoteNG.My.Language.strMenuReportBug;
			mMenInfoDonate.Text = mRemoteNG.My.Language.strMenuDonate;
			mMenInfoWebsite.Text = mRemoteNG.My.Language.strMenuWebsite;
			mMenInfoAbout.Text = mRemoteNG.My.Language.strMenuAbout;
			mMenInfoAnnouncements.Text = mRemoteNG.My.Language.strMenuAnnouncements;

			lblQuickConnect.Text = mRemoteNG.My.Language.strLabelConnect;
			btnQuickConnect.Text = mRemoteNG.My.Language.strMenuConnect;
			btnConnections.Text = mRemoteNG.My.Language.strMenuConnections;

			cMenToolbarShowText.Text = mRemoteNG.My.Language.strMenuShowText;

			ToolStripButton1.Text = mRemoteNG.My.Language.strConnect;
			ToolStripButton2.Text = mRemoteNG.My.Language.strScreenshot;
			ToolStripButton3.Text = mRemoteNG.My.Language.strRefresh;

			ToolStripSplitButton1.Text = mRemoteNG.My.Language.strSpecialKeys;
			ToolStripMenuItem1.Text = mRemoteNG.My.Language.strKeysCtrlAltDel;
			ToolStripMenuItem2.Text = mRemoteNG.My.Language.strKeysCtrlEsc;
		}

		public void ApplyThemes()
		{
			var _with1 = ThemeManager.ActiveTheme;
			pnlDock.DockBackColor = _with1.WindowBackgroundColor;
			tsContainer.BackColor = _with1.ToolbarBackgroundColor;
			tsContainer.ForeColor = _with1.ToolbarTextColor;
			tsContainer.TopToolStripPanel.BackColor = _with1.ToolbarBackgroundColor;
			tsContainer.TopToolStripPanel.ForeColor = _with1.ToolbarTextColor;
			tsContainer.BottomToolStripPanel.BackColor = _with1.ToolbarBackgroundColor;
			tsContainer.BottomToolStripPanel.ForeColor = _with1.ToolbarTextColor;
			tsContainer.LeftToolStripPanel.BackColor = _with1.ToolbarBackgroundColor;
			tsContainer.LeftToolStripPanel.ForeColor = _with1.ToolbarTextColor;
			tsContainer.RightToolStripPanel.BackColor = _with1.ToolbarBackgroundColor;
			tsContainer.RightToolStripPanel.ForeColor = _with1.ToolbarTextColor;
			tsContainer.ContentPanel.BackColor = _with1.ToolbarBackgroundColor;
			tsContainer.ContentPanel.ForeColor = _with1.ToolbarTextColor;
			msMain.BackColor = _with1.ToolbarBackgroundColor;
			msMain.ForeColor = _with1.ToolbarTextColor;
			ApplyMenuColors(msMain.Items);
			tsExternalTools.BackColor = _with1.ToolbarBackgroundColor;
			tsExternalTools.ForeColor = _with1.ToolbarTextColor;
			tsQuickConnect.BackColor = _with1.ToolbarBackgroundColor;
			tsQuickConnect.ForeColor = _with1.ToolbarTextColor;
		}

		private static void ApplyMenuColors(ToolStripItemCollection itemCollection)
		{
			var _with2 = ThemeManager.ActiveTheme;
			ToolStripMenuItem menuItem = null;
			foreach (ToolStripItem item in itemCollection) {
				item.BackColor = _with2.MenuBackgroundColor;
				item.ForeColor = _with2.MenuTextColor;

				menuItem = item as ToolStripMenuItem;
				if (menuItem != null) {
					ApplyMenuColors(menuItem.DropDownItems);
				}
			}
		}

		#if PORTABLE
		private void frmMain_Shown(object sender, EventArgs e)
		{
			return;
			#endif
			if (!mRemoteNG.My.Settings.CheckForUpdatesAsked) {
				string[] commandButtons = {
					mRemoteNG.My.Language.strAskUpdatesCommandRecommended,
					mRemoteNG.My.Language.strAskUpdatesCommandCustom,
					mRemoteNG.My.Language.strAskUpdatesCommandAskLater
				};
				cTaskDialog.ShowTaskDialogBox(this, MyProject.Application.Info.ProductName, mRemoteNG.My.Language.strAskUpdatesMainInstruction, string.Format(mRemoteNG.My.Language.strAskUpdatesContent, MyProject.Application.Info.ProductName), "", "", "", "", string.Join("|", commandButtons), eTaskDialogButtons.None,
				eSysIcons.Question, eSysIcons.Question);
				if (cTaskDialog.CommandButtonResult == 0 | cTaskDialog.CommandButtonResult == 1) {
					mRemoteNG.My.Settings.CheckForUpdatesAsked = true;
				}
				if (cTaskDialog.CommandButtonResult == 1) {
					Windows.ShowUpdatesTab();
				}
				return;
			}

			if (!mRemoteNG.My.Settings.CheckForUpdatesOnStartup)
				return;

			System.DateTime nextUpdateCheck = mRemoteNG.My.Settings.CheckForUpdatesLastCheck.Add(TimeSpan.FromDays(mRemoteNG.My.Settings.CheckForUpdatesFrequencyDays));
			if (mRemoteNG.My.Settings.UpdatePending | System.DateTime.UtcNow > nextUpdateCheck) {
				if (!IsHandleCreated)
					CreateHandle();
				// Make sure the handle is created so that InvokeRequired returns the correct result
				Startup.CheckForUpdate();
				Startup.CheckForAnnouncement();
			}
		}

		private void frmMain_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
		{
			if (!(Runtime.WindowList == null || Runtime.WindowList.Count == 0)) {
				UI.Window.Connection connectionWindow = null;
				int openConnections = 0;
				foreach (UI.Window.Base window in Runtime.WindowList) {
					connectionWindow = window as UI.Window.Connection;
					if (connectionWindow != null) {
						openConnections = openConnections + connectionWindow.TabController.TabPages.Count;
					}
				}

				if (openConnections > 0 & (mRemoteNG.My.Settings.ConfirmCloseConnection == ConfirmClose.All | (mRemoteNG.My.Settings.ConfirmCloseConnection == ConfirmClose.Multiple & openConnections > 1) | mRemoteNG.My.Settings.ConfirmCloseConnection == ConfirmClose.Exit)) {
					DialogResult result = cTaskDialog.MessageBox(this, MyProject.Application.Info.ProductName, mRemoteNG.My.Language.strConfirmExitMainInstruction, "", "", "", mRemoteNG.My.Language.strCheckboxDoNotShowThisMessageAgain, eTaskDialogButtons.YesNo, eSysIcons.Question, null);
					if (cTaskDialog.VerificationChecked) {
						mRemoteNG.My.Settings.ConfirmCloseConnection = mRemoteNG.My.Settings.ConfirmCloseConnection - 1;
					}
					if (result == DialogResult.No) {
						e.Cancel = true;
						return;
					}
				}
			}

			Shutdown.Cleanup();

			_isClosing = true;

			if (Runtime.WindowList != null) {
				foreach (UI.Window.Base window in Runtime.WindowList) {
					window.Close();
				}
			}

			Shutdown.StartUpdate();

			Debug.Print("[END] - " + DateAndTime.Now);
		}
		#endregion

		#region "Timer"
		private void tmrAutoSave_Tick(System.Object sender, System.EventArgs e)
		{
			Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.InformationMsg, "Doing AutoSave", true);
			mRemoteNG.App.Runtime.SaveConnections();
		}
		#endregion

		#region "Ext Apps Toolbar"
		private void cMenToolbarShowText_Click(System.Object sender, System.EventArgs e)
		{
			SwitchToolBarText(!cMenToolbarShowText.Checked);
		}

		public void AddExternalToolsToToolBar()
		{
			try {
				for (int index = tsExternalTools.Items.Count - 1; index >= 0; index += -1) {
					tsExternalTools.Items[index].Dispose();
				}
				tsExternalTools.Items.Clear();

				ToolStripButton button = null;
				foreach (Tools.ExternalTool tool in Runtime.ExternalTools) {
					button = tsExternalTools.Items.Add(tool.DisplayName, tool.Image, tsExtAppEntry_Click);

					if (cMenToolbarShowText.Checked == true) {
						button.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
					} else {
						if (button.Image != null) {
							button.DisplayStyle = ToolStripItemDisplayStyle.Image;
						} else {
							button.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
						}
					}

					button.Tag = tool;
				}
			} catch (Exception ex) {
				Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, string.Format(mRemoteNG.My.Language.strErrorAddExternalToolsToToolBarFailed, ex.Message), true);
			}
		}

		private void tsExtAppEntry_Click(System.Object sender, System.EventArgs e)
		{
			Tools.ExternalTool extA = sender.Tag;

			if (mRemoteNG.Tree.Node.GetNodeType(mRemoteNG.Tree.Node.SelectedNode) == mRemoteNG.Tree.Node.Type.Connection | mRemoteNG.Tree.Node.GetNodeType(mRemoteNG.Tree.Node.SelectedNode) == mRemoteNG.Tree.Node.Type.PuttySession) {
				extA.Start(mRemoteNG.Tree.Node.SelectedNode.Tag);
			} else {
				extA.Start();
			}
		}

		public void SwitchToolBarText(bool show)
		{
			foreach (ToolStripButton tItem in tsExternalTools.Items) {
				if (show == true) {
					tItem.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
				} else {
					if (tItem.Image != null) {
						tItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
					} else {
						tItem.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
					}
				}
			}

			cMenToolbarShowText.Checked = show;
		}
		#endregion

		#region "Menu"
		#region "File"
		private void mMenFile_DropDownOpening(System.Object sender, System.EventArgs e)
		{
			switch (mRemoteNG.Tree.Node.GetNodeType(mRemoteNG.Tree.Node.SelectedNode)) {
				case mRemoteNG.Tree.Node.Type.Root:
					mMenFileNewConnection.Enabled = true;
					mMenFileNewFolder.Enabled = true;
					mMenFileDelete.Enabled = false;
					mMenFileRename.Enabled = true;
					mMenFileDuplicate.Enabled = false;
					mMenFileDelete.Text = mRemoteNG.My.Language.strMenuDelete;
					mMenFileRename.Text = mRemoteNG.My.Language.strMenuRenameFolder;
					mMenFileDuplicate.Text = mRemoteNG.My.Language.strMenuDuplicate;
					break;
				case mRemoteNG.Tree.Node.Type.Container:
					mMenFileNewConnection.Enabled = true;
					mMenFileNewFolder.Enabled = true;
					mMenFileDelete.Enabled = true;
					mMenFileRename.Enabled = true;
					mMenFileDuplicate.Enabled = true;
					mMenFileDelete.Text = mRemoteNG.My.Language.strMenuDeleteFolder;
					mMenFileRename.Text = mRemoteNG.My.Language.strMenuRenameFolder;
					mMenFileDuplicate.Text = mRemoteNG.My.Language.strMenuDuplicateFolder;
					break;
				case mRemoteNG.Tree.Node.Type.Connection:
					mMenFileNewConnection.Enabled = true;
					mMenFileNewFolder.Enabled = true;
					mMenFileDelete.Enabled = true;
					mMenFileRename.Enabled = true;
					mMenFileDuplicate.Enabled = true;
					mMenFileDelete.Text = mRemoteNG.My.Language.strMenuDeleteConnection;
					mMenFileRename.Text = mRemoteNG.My.Language.strMenuRenameConnection;
					mMenFileDuplicate.Text = mRemoteNG.My.Language.strMenuDuplicateConnection;
					break;
				case mRemoteNG.Tree.Node.Type.PuttyRoot:
				case mRemoteNG.Tree.Node.Type.PuttySession:
					mMenFileNewConnection.Enabled = false;
					mMenFileNewFolder.Enabled = false;
					mMenFileDelete.Enabled = false;
					mMenFileRename.Enabled = false;
					mMenFileDuplicate.Enabled = false;
					mMenFileDelete.Text = mRemoteNG.My.Language.strMenuDelete;
					mMenFileRename.Text = mRemoteNG.My.Language.strMenuRename;
					mMenFileDuplicate.Text = mRemoteNG.My.Language.strMenuDuplicate;
					break;
				default:
					mMenFileNewConnection.Enabled = true;
					mMenFileNewFolder.Enabled = true;
					mMenFileDelete.Enabled = false;
					mMenFileRename.Enabled = false;
					mMenFileDuplicate.Enabled = false;
					mMenFileDelete.Text = mRemoteNG.My.Language.strMenuDelete;
					mMenFileRename.Text = mRemoteNG.My.Language.strMenuRename;
					mMenFileDuplicate.Text = mRemoteNG.My.Language.strMenuDuplicate;
					break;
			}
		}

		private static void mMenFileNewConnection_Click(System.Object sender, EventArgs e)
		{
			Windows.treeForm.AddConnection();
			Runtime.SaveConnectionsBG();
		}

		private static void mMenFileNewFolder_Click(System.Object sender, EventArgs e)
		{
			Windows.treeForm.AddFolder();
			Runtime.SaveConnectionsBG();
		}

		private static void mMenFileNew_Click(System.Object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog = mRemoteNG.Tools.Controls.ConnectionsSaveAsDialog();
			if (!(saveFileDialog.ShowDialog() == DialogResult.OK))
				return;

			Runtime.NewConnections(saveFileDialog.FileName);
		}

		private static void mMenFileLoad_Click(System.Object sender, EventArgs e)
		{
			if (Runtime.IsConnectionsFileLoaded) {
				switch (Interaction.MsgBox(Language.strSaveConnectionsFileBeforeOpeningAnother, MsgBoxStyle.YesNoCancel | MsgBoxStyle.Question)) {
					case MsgBoxResult.Yes:
						Runtime.SaveConnections();
						break;
					case MsgBoxResult.Cancel:
						return;
				}
			}

			Runtime.LoadConnections(true);
		}

		private static void mMenFileSave_Click(System.Object sender, EventArgs e)
		{
			Runtime.SaveConnections();
		}

		private static void mMenFileSaveAs_Click(System.Object sender, EventArgs e)
		{
			Runtime.SaveConnectionsAs();
		}

		private static void mMenFileDelete_Click(System.Object sender, EventArgs e)
		{
			mRemoteNG.Tree.Node.DeleteSelectedNode();
			Runtime.SaveConnectionsBG();
		}

		private static void mMenFileRename_Click(System.Object sender, EventArgs e)
		{
			mRemoteNG.Tree.Node.StartRenameSelectedNode();
			Runtime.SaveConnectionsBG();
		}

		private static void mMenFileDuplicate_Click(System.Object sender, EventArgs e)
		{
			mRemoteNG.Tree.Node.CloneNode(mRemoteNG.Tree.Node.SelectedNode);
			Runtime.SaveConnectionsBG();
		}

		private static void mMenFileImportFromFile_Click(System.Object sender, EventArgs e)
		{
			Import.ImportFromFile(Windows.treeForm.tvConnections.Nodes[0], Windows.treeForm.tvConnections.SelectedNode);
		}

		private static void mMenFileImportFromActiveDirectory_Click(System.Object sender, EventArgs e)
		{
			Windows.Show(mRemoteNG.UI.Window.Type.ActiveDirectoryImport);
		}

		private static void mMenFileImportFromPortScan_Click(System.Object sender, EventArgs e)
		{
			Windows.Show(mRemoteNG.UI.Window.Type.PortScan, true);
		}

		private static void mMenFileExport_Click(System.Object sender, EventArgs e)
		{
			Export.ExportToFile(Windows.treeForm.tvConnections.Nodes[0], Windows.treeForm.tvConnections.SelectedNode);
		}

		private static void mMenFileExit_Click(System.Object sender, EventArgs e)
		{
			Shutdown.Quit();
		}
		#endregion

		#region "View"
		private void mMenView_DropDownOpening(object sender, System.EventArgs e)
		{
			this.mMenViewConnections.Checked = !Windows.treeForm.IsHidden;
			this.mMenViewConfig.Checked = !Windows.configForm.IsHidden;
			this.mMenViewErrorsAndInfos.Checked = !Windows.errorsForm.IsHidden;
			this.mMenViewSessions.Checked = !Windows.sessionsForm.IsHidden;
			this.mMenViewScreenshotManager.Checked = !Windows.screenshotForm.IsHidden;

			this.mMenViewExtAppsToolbar.Checked = tsExternalTools.Visible;
			this.mMenViewQuickConnectToolbar.Checked = tsQuickConnect.Visible;

			this.mMenViewConnectionPanels.DropDownItems.Clear();

			for (int i = 0; i <= Runtime.WindowList.Count - 1; i++) {
				ToolStripMenuItem tItem = new ToolStripMenuItem(Runtime.WindowList[i].Text, Runtime.WindowList[i].Icon.ToBitmap(), ConnectionPanelMenuItem_Click);
				tItem.Tag = Runtime.WindowList[i];

				this.mMenViewConnectionPanels.DropDownItems.Add(tItem);
			}

			if (this.mMenViewConnectionPanels.DropDownItems.Count > 0) {
				this.mMenViewConnectionPanels.Enabled = true;
			} else {
				this.mMenViewConnectionPanels.Enabled = false;
			}
		}

		private void ConnectionPanelMenuItem_Click(object sender, System.EventArgs e)
		{
			(sender.Tag as UI.Window.Base).Show(this.pnlDock);
			(sender.Tag as UI.Window.Base).Focus();
		}

		private void mMenViewSessions_Click(object sender, System.EventArgs e)
		{
			if (this.mMenViewSessions.Checked == false) {
				Windows.sessionsPanel.Show(this.pnlDock);
				this.mMenViewSessions.Checked = true;
			} else {
				Windows.sessionsPanel.Hide();
				this.mMenViewSessions.Checked = false;
			}
		}

		private void mMenViewConnections_Click(System.Object sender, System.EventArgs e)
		{
			if (this.mMenViewConnections.Checked == false) {
				Windows.treePanel.Show(this.pnlDock);
				this.mMenViewConnections.Checked = true;
			} else {
				Windows.treePanel.Hide();
				this.mMenViewConnections.Checked = false;
			}
		}

		private void mMenViewConfig_Click(System.Object sender, System.EventArgs e)
		{
			if (this.mMenViewConfig.Checked == false) {
				Windows.configPanel.Show(this.pnlDock);
				this.mMenViewConfig.Checked = true;
			} else {
				Windows.configPanel.Hide();
				this.mMenViewConfig.Checked = false;
			}
		}

		private void mMenViewErrorsAndInfos_Click(System.Object sender, System.EventArgs e)
		{
			if (this.mMenViewErrorsAndInfos.Checked == false) {
				Windows.errorsPanel.Show(this.pnlDock);
				this.mMenViewErrorsAndInfos.Checked = true;
			} else {
				Windows.errorsPanel.Hide();
				this.mMenViewErrorsAndInfos.Checked = false;
			}
		}

		private void mMenViewScreenshotManager_Click(System.Object sender, System.EventArgs e)
		{
			if (this.mMenViewScreenshotManager.Checked == false) {
				Windows.screenshotPanel.Show(this.pnlDock);
				this.mMenViewScreenshotManager.Checked = true;
			} else {
				Windows.screenshotPanel.Hide();
				this.mMenViewScreenshotManager.Checked = false;
			}
		}

		private void mMenViewJumpToConnectionsConfig_Click(object sender, System.EventArgs e)
		{
			if (object.ReferenceEquals(pnlDock.ActiveContent, Windows.treePanel)) {
				Windows.configForm.Activate();
			} else {
				Windows.treeForm.Activate();
			}
		}

		private void mMenViewJumpToSessionsScreenshots_Click(object sender, System.EventArgs e)
		{
			if (object.ReferenceEquals(pnlDock.ActiveContent, Windows.sessionsPanel)) {
				Windows.screenshotForm.Activate();
			} else {
				Windows.sessionsForm.Activate();
			}
		}

		private void mMenViewJumpToErrorsInfos_Click(object sender, System.EventArgs e)
		{
			Windows.errorsForm.Activate();
		}

		private void mMenViewResetLayout_Click(System.Object sender, System.EventArgs e)
		{
			if (Interaction.MsgBox(mRemoteNG.My.Language.strConfirmResetLayout, MsgBoxStyle.Question | MsgBoxStyle.YesNo) == MsgBoxResult.Yes) {
				mRemoteNG.App.Runtime.Startup.SetDefaultLayout();
			}
		}

		private void mMenViewAddConnectionPanel_Click(System.Object sender, System.EventArgs e)
		{
			Runtime.AddPanel();
		}

		private void mMenViewExtAppsToolbar_Click(System.Object sender, System.EventArgs e)
		{
			if (mMenViewExtAppsToolbar.Checked == false) {
				tsExternalTools.Visible = true;
				mMenViewExtAppsToolbar.Checked = true;
			} else {
				tsExternalTools.Visible = false;
				mMenViewExtAppsToolbar.Checked = false;
			}
		}

		private void mMenViewQuickConnectToolbar_Click(System.Object sender, System.EventArgs e)
		{
			if (mMenViewQuickConnectToolbar.Checked == false) {
				tsQuickConnect.Visible = true;
				mMenViewQuickConnectToolbar.Checked = true;
			} else {
				tsQuickConnect.Visible = false;
				mMenViewQuickConnectToolbar.Checked = false;
			}
		}

		public Tools.Misc.Fullscreen Fullscreen = new Tools.Misc.Fullscreen(this);
		private void mMenViewFullscreen_Click(System.Object sender, System.EventArgs e)
		{
			Fullscreen.Value = !Fullscreen.Value;
			mMenViewFullscreen.Checked = Fullscreen.Value;
		}
		#endregion

		#region "Tools"
		private void mMenToolsUpdate_Click(System.Object sender, System.EventArgs e)
		{
			mRemoteNG.App.Runtime.Windows.Show(mRemoteNG.UI.Window.Type.Update);
		}

		private void mMenToolsSSHTransfer_Click(System.Object sender, System.EventArgs e)
		{
			mRemoteNG.App.Runtime.Windows.Show(mRemoteNG.UI.Window.Type.SSHTransfer);
		}

		private void mMenToolsUVNCSC_Click(System.Object sender, System.EventArgs e)
		{
			mRemoteNG.App.Runtime.Windows.Show(mRemoteNG.UI.Window.Type.UltraVNCSC);
		}

		private void mMenToolsExternalApps_Click(System.Object sender, System.EventArgs e)
		{
			mRemoteNG.App.Runtime.Windows.Show(mRemoteNG.UI.Window.Type.ExternalApps);
		}

		private void mMenToolsPortScan_Click(System.Object sender, EventArgs e)
		{
			mRemoteNG.App.Runtime.Windows.Show(mRemoteNG.UI.Window.Type.PortScan, false);
		}

		private void mMenToolsComponentsCheck_Click(System.Object sender, System.EventArgs e)
		{
			mRemoteNG.App.Runtime.Windows.Show(mRemoteNG.UI.Window.Type.ComponentsCheck);
		}

		private void mMenToolsOptions_Click(System.Object sender, System.EventArgs e)
		{
			mRemoteNG.App.Runtime.Windows.Show(mRemoteNG.UI.Window.Type.Options);
		}
		#endregion

		#region "Quick Connect"
		private void PopulateQuickConnectProtocolMenu()
		{
			try {
				mnuQuickConnectProtocol.Items.Clear();
				foreach (FieldInfo fieldInfo in typeof(Connection.Protocol.Protocols).GetFields()) {
					if (!(fieldInfo.Name == "value__" | fieldInfo.Name == "IntApp")) {
						ToolStripMenuItem menuItem = new ToolStripMenuItem(fieldInfo.Name);
						if (fieldInfo.Name == mRemoteNG.My.Settings.QuickConnectProtocol) {
							menuItem.Checked = true;
							btnQuickConnect.Text = mRemoteNG.My.Settings.QuickConnectProtocol;
						}
						mnuQuickConnectProtocol.Items.Add(menuItem);
					}
				}
			} catch (Exception ex) {
				Runtime.MessageCollector.AddExceptionMessage("PopulateQuickConnectProtocolMenu() failed.", ex, mRemoteNG.Messages.MessageClass.ErrorMsg, true);
			}
		}

		private void lblQuickConnect_Click(System.Object sender, EventArgs e)
		{
			cmbQuickConnect.Focus();
		}

		private void cmbQuickConnect_ConnectRequested(object sender, Controls.QuickConnectComboBox.ConnectRequestedEventArgs e)
		{
			btnQuickConnect_ButtonClick(sender, e);
		}

		private void btnQuickConnect_ButtonClick(object sender, EventArgs e)
		{
			try {
				Connection.Info connectionInfo = Runtime.CreateQuickConnect(cmbQuickConnect.Text.Trim(), mRemoteNG.Connection.Protocol.Converter.StringToProtocol(mRemoteNG.My.Settings.QuickConnectProtocol));
				if (connectionInfo == null) {
					cmbQuickConnect.Focus();
					return;
				}

				cmbQuickConnect.Add(connectionInfo);

				Runtime.OpenConnection(connectionInfo, mRemoteNG.Connection.Info.Force.DoNotJump);
			} catch (Exception ex) {
				Runtime.MessageCollector.AddExceptionMessage("btnQuickConnect_ButtonClick() failed.", ex, mRemoteNG.Messages.MessageClass.ErrorMsg, true);
			}
		}

		private void cmbQuickConnect_ProtocolChanged(object sender, Controls.QuickConnectComboBox.ProtocolChangedEventArgs e)
		{
			SetQuickConnectProtocol(mRemoteNG.Connection.Protocol.Converter.ProtocolToString(e.Protocol));
		}

		private void btnQuickConnect_DropDownItemClicked(System.Object sender, System.Windows.Forms.ToolStripItemClickedEventArgs e)
		{
			SetQuickConnectProtocol(e.ClickedItem.Text);
		}

		private void SetQuickConnectProtocol(string protocol)
		{
			mRemoteNG.My.Settings.QuickConnectProtocol = protocol;
			btnQuickConnect.Text = protocol;
			foreach (ToolStripMenuItem menuItem in mnuQuickConnectProtocol.Items) {
				if (menuItem.Text == protocol) {
					menuItem.Checked = true;
				} else {
					menuItem.Checked = false;
				}
			}
		}
		#endregion

		#region "Info"
		private void mMenInfoHelp_Click(System.Object sender, System.EventArgs e)
		{
			mRemoteNG.App.Runtime.Windows.Show(mRemoteNG.UI.Window.Type.Help);
		}

		private void mMenInfoForum_Click(System.Object sender, System.EventArgs e)
		{
			mRemoteNG.App.Runtime.GoToForum();
		}

		private void mMenInfoBugReport_Click(System.Object sender, System.EventArgs e)
		{
			mRemoteNG.App.Runtime.GoToBugs();
		}

		private void mMenInfoWebsite_Click(System.Object sender, System.EventArgs e)
		{
			mRemoteNG.App.Runtime.GoToWebsite();
		}

		private void mMenInfoDonate_Click(System.Object sender, System.EventArgs e)
		{
			mRemoteNG.App.Runtime.GoToDonate();
		}

		private void mMenInfoAnnouncements_Click(System.Object sender, System.EventArgs e)
		{
			mRemoteNG.App.Runtime.Windows.Show(mRemoteNG.UI.Window.Type.Announcement);
		}

		private void mMenInfoAbout_Click(System.Object sender, System.EventArgs e)
		{
			mRemoteNG.App.Runtime.Windows.Show(mRemoteNG.UI.Window.Type.About);
		}
		#endregion
		#endregion

		#region "Connections DropDown"
		private void btnConnections_DropDownOpening(object sender, EventArgs e)
		{
			btnConnections.DropDownItems.Clear();

			foreach (TreeNode treeNode in Windows.treeForm.tvConnections.Nodes) {
				AddNodeToMenu(treeNode.Nodes, btnConnections);
			}
		}

		private static void AddNodeToMenu(TreeNodeCollection treeNodeCollection, ToolStripDropDownItem toolStripMenuItem)
		{
			try {
				foreach (TreeNode treeNode in treeNodeCollection) {
					ToolStripMenuItem menuItem = new ToolStripMenuItem();
					menuItem.Text = treeNode.Text;
					menuItem.Tag = treeNode;

					if (mRemoteNG.Tree.Node.GetNodeType(treeNode) == mRemoteNG.Tree.Node.Type.Container) {
						menuItem.Image = mRemoteNG.My.Resources.Folder;
						menuItem.Tag = treeNode.Tag;

						toolStripMenuItem.DropDownItems.Add(menuItem);
						AddNodeToMenu(treeNode.Nodes, menuItem);
					} else if (mRemoteNG.Tree.Node.GetNodeType(treeNode) == mRemoteNG.Tree.Node.Type.Connection | mRemoteNG.Tree.Node.GetNodeType(treeNode) == mRemoteNG.Tree.Node.Type.PuttySession) {
						menuItem.Image = Windows.treeForm.imgListTree.Images[treeNode.ImageIndex];
						menuItem.Tag = treeNode.Tag;

						toolStripMenuItem.DropDownItems.Add(menuItem);
					}

					menuItem.MouseUp += ConnectionsMenuItem_MouseUp;
				}
			} catch (Exception ex) {
				Runtime.MessageCollector.AddExceptionMessage("frmMain.AddNodeToMenu() failed", ex, mRemoteNG.Messages.MessageClass.ErrorMsg, true);
			}
		}

		private static void ConnectionsMenuItem_MouseUp(System.Object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left) {
				if (sender.Tag is Connection.Info) {
					mRemoteNG.App.Runtime.OpenConnection(sender.Tag);
				}
			}
		}
		#endregion

		#region "Window Overrides and DockPanel Stuff"
		private bool _inSizeMove = false;
		private void frmMain_ResizeBegin(object sender, EventArgs e)
		{
			_inSizeMove = true;
		}

		private void frmMain_Resize(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Minimized) {
				if (mRemoteNG.My.Settings.MinimizeToTray) {
					if (Runtime.NotificationAreaIcon == null) {
						Runtime.NotificationAreaIcon = new Tools.Controls.NotificationAreaIcon();
					}
					Hide();
				}
			} else {
				PreviousWindowState = WindowState;
			}
		}

		private void frmMain_ResizeEnd(object sender, EventArgs e)
		{
			_inSizeMove = false;

			// This handles activations from clicks that started a size/move operation
			ActivateConnection();
		}

		private bool _inMouseActivate = false;
		protected override void WndProc(ref Message m)
		{
			try {
				switch (m.Msg) {
					case Native.WM_MOUSEACTIVATE:
						_inMouseActivate = true;
						break;
					case Native.WM_ACTIVATEAPP:
						_inMouseActivate = false;
						break;
					case Native.WM_ACTIVATE:
						// Ingore this message if it wasn't triggered by a click
						if (!(Native.LOWORD(ref m.WParam) == Native.WA_CLICKACTIVE))
							break; // TODO: might not be correct. Was : Exit Select


						Control control = FromChildHandle(Native.WindowFromPoint(MousePosition));
						if ((control != null)) {
							// Let TreeViews and ComboBoxes get focus but don't simulate a mouse event
							if (control is TreeView | control is ComboBox)
								break; // TODO: might not be correct. Was : Exit Select

							if (control.CanSelect | control is MenuStrip | control is ToolStrip | control is Magic.Controls.TabControl | control is Magic.Controls.InertButton) {
								// Simulate a mouse event since one wasn't generated by Windows
								Point clientMousePosition = control.PointToClient(MousePosition);
								Native.SendMessage(control.Handle, Native.WM_LBUTTONDOWN, Native.MK_LBUTTON, Native.MAKELPARAM(ref clientMousePosition.X, ref clientMousePosition.Y));

								control.Focus();
								break; // TODO: might not be correct. Was : Exit Select
							}
						}

						// This handles activations from clicks that did not start a size/move operation
						ActivateConnection();
						break;
					case Native.WM_WINDOWPOSCHANGED:
						// Ignore this message if the window wasn't activated
						WINDOWPOS windowPos = Marshal.PtrToStructure(m.LParam, typeof(WINDOWPOS));
						if ((!((windowPos.flags & Native.SWP_NOACTIVATE) == 0)))
							break; // TODO: might not be correct. Was : Exit Select


						// This handles all other activations
						if (!_inMouseActivate & !_inSizeMove)
							ActivateConnection();
						break;
					case Native.WM_SYSCOMMAND:
						for (int i = 0; i <= SysMenSubItems.Length - 1; i++) {
							if (SysMenSubItems[i] == m.WParam) {
								Screens.SendFormToScreen(Screen.AllScreens[i]);
								break; // TODO: might not be correct. Was : Exit For
							}
						}

						break;
					case Native.WM_DRAWCLIPBOARD:
						Native.SendMessage(fpChainedWindowHandle, m.Msg, m.LParam, m.WParam);
						if (clipboardchange != null) {
							clipboardchange();
						}

						break;
					case Native.WM_CHANGECBCHAIN:
						//Send to the next window
						Native.SendMessage(fpChainedWindowHandle, m.Msg, m.LParam, m.WParam);
						fpChainedWindowHandle = m.LParam;
						break;
					case KeyboardHook.HookKeyMsg:
						if (!(m.WParam.ToInt32() == Win32.WM_KEYDOWN))
							break; // TODO: might not be correct. Was : Exit Select


						switch (KeyboardShortcuts.CommandFromHookKeyMessage(m)) {
							case ShortcutCommand.PreviousTab:
								SelectTabRelative(-1);
								break;
							case ShortcutCommand.NextTab:
								SelectTabRelative(1);
								break;
						}
						break;
				}
			} catch (Exception ex) {
			}

			base.WndProc(m);
		}

		private void ActivateConnection()
		{
			if (pnlDock.ActiveDocument is UI.Window.Connection) {
				UI.Window.Connection cW = pnlDock.ActiveDocument;
				if (cW.TabController.SelectedTab != null) {
					Crownwood.Magic.Controls.TabPage tab = cW.TabController.SelectedTab;
					Connection.InterfaceControl ifc = tab.Tag as Connection.InterfaceControl;
					ifc.Protocol.Focus();
					(ifc.FindForm() as UI.Window.Connection).RefreshIC();
				}
			}
		}

		private void pnlDock_ActiveDocumentChanged(object sender, EventArgs e)
		{
			ActivateConnection();
			UI.Window.Connection connectionWindow = pnlDock.ActiveDocument as UI.Window.Connection;
			if (connectionWindow != null)
				connectionWindow.UpdateSelectedConnection();
		}

		private void UpdateWindowTitle()
		{
			if (InvokeRequired) {
				Invoke(new MethodInvoker(UpdateWindowTitle));
				return;
			}

			StringBuilder titleBuilder = new StringBuilder(Application.Info.ProductName);
			const string separator = " - ";

			if (Runtime.IsConnectionsFileLoaded) {
				if (UsingSqlServer) {
					titleBuilder.Append(separator);
					titleBuilder.Append(Language.strSQLServer.TrimEnd(":"));
				} else {
					if (!string.IsNullOrEmpty(ConnectionsFileName)) {
						titleBuilder.Append(separator);
						if (mRemoteNG.My.Settings.ShowCompleteConsPathInTitle) {
							titleBuilder.Append(ConnectionsFileName);
						} else {
							titleBuilder.Append(Path.GetFileName(ConnectionsFileName));
						}
					}
				}
			}

			if (!(SelectedConnection == null || string.IsNullOrEmpty(SelectedConnection.Name))) {
				titleBuilder.Append(separator);
				titleBuilder.Append(SelectedConnection.Name);
			}

			Text = titleBuilder.ToString();
		}

		public void ShowHidePanelTabs(DockContent closingDocument = null)
		{
			DocumentStyle newDocumentStyle = pnlDock.DocumentStyle;

			if (mRemoteNG.My.Settings.AlwaysShowPanelTabs) {
				newDocumentStyle = DocumentStyle.DockingWindow;
				// Show the panel tabs
			} else {
				int nonConnectionPanelCount = 0;
				foreach (DockContent document in pnlDock.Documents) {
					if ((closingDocument == null || !object.ReferenceEquals(document, closingDocument)) & !document is UI.Window.Connection) {
						nonConnectionPanelCount = nonConnectionPanelCount + 1;
					}
				}

				if (nonConnectionPanelCount == 0) {
					newDocumentStyle = DocumentStyle.DockingSdi;
					// Hide the panel tabs
				} else {
					newDocumentStyle = DocumentStyle.DockingWindow;
					// Show the panel tabs
				}
			}

			if (!(pnlDock.DocumentStyle == newDocumentStyle)) {
				pnlDock.DocumentStyle = newDocumentStyle;
				pnlDock.Size = new Size(1, 1);
			}
		}

		private void SelectTabRelative(int relativeIndex)
		{
			if (!pnlDock.ActiveDocument is UI.Window.Connection)
				return;

			UI.Window.Connection connectionWindow = pnlDock.ActiveDocument;
			Crownwood.Magic.Controls.TabControl tabController = connectionWindow.TabController;

			int newIndex = tabController.SelectedIndex + relativeIndex;
			while (newIndex < 0 | newIndex >= tabController.TabPages.Count) {
				if (newIndex < 0)
					newIndex = tabController.TabPages.Count + newIndex;
				if (newIndex >= tabController.TabPages.Count)
					newIndex = newIndex - tabController.TabPages.Count;
			}

			tabController.SelectedIndex = newIndex;
		}
		#endregion

		#region "Screen Stuff"
		private void DisplayChanged(object sender, System.EventArgs e)
		{
			ResetSysMenuItems();
			AddSysMenuItems();
		}

		private int[] SysMenSubItems = new int[51];
		private static void ResetSysMenuItems()
		{
			Runtime.SystemMenu.Reset();
		}

		private void AddSysMenuItems()
		{
			Runtime.SystemMenu = new Tools.SystemMenu(this.Handle);
			IntPtr popMen = Runtime.SystemMenu.CreatePopupMenuItem();

			for (int i = 0; i <= Screen.AllScreens.Length - 1; i++) {
				SysMenSubItems[i] = 200 + i;
				Runtime.SystemMenu.AppendMenuItem(popMen, mRemoteNG.Tools.SystemMenu.Flags.MF_STRING, SysMenSubItems[i], mRemoteNG.My.Language.strScreen + " " + i + 1);
			}

			Runtime.SystemMenu.InsertMenuItem(Runtime.SystemMenu.SystemMenuHandle, 0, mRemoteNG.Tools.SystemMenu.Flags.MF_POPUP | mRemoteNG.Tools.SystemMenu.Flags.MF_BYPOSITION, popMen, mRemoteNG.My.Language.strSendTo);
			Runtime.SystemMenu.InsertMenuItem(Runtime.SystemMenu.SystemMenuHandle, 1, mRemoteNG.Tools.SystemMenu.Flags.MF_BYPOSITION | mRemoteNG.Tools.SystemMenu.Flags.MF_SEPARATOR, IntPtr.Zero, null);
		}
		#endregion
	}
}
