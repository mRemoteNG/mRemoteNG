using mRemoteNG.App;
using mRemoteNG.Config;
using mRemoteNG.My;
using mRemoteNG.Themes;
using PSTaskDialog;
using SharedLibraryNG;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG
{
	public partial class frmMain
    {
        #region Private Variables
        // Static Variables
        private static clipboardchangeEventHandler clipboardchangeEvent;

        // Instance Variables that are private-only
        private bool _inSizeMove = false;
        private bool _inMouseActivate = false;
        private IntPtr fpChainedWindowHandle;
        private int[] SysMenSubItems = new int[51];

        // Instance Variables with public getters and setters
        private FormWindowState _previousWindowState;
        private bool _isClosing = false;
        private bool _usingSqlServer = false;
        private string _connectionsFileName = null;
        private bool _showFullPathInTitle;
        private Connection.ConnectionRecordImp _selectedConnection = null;
        public Tools.Misc.Fullscreen _fullscreen;
        #endregion

        #region Constructors
        public frmMain()
		{
			_showFullPathInTitle = System.Convert.ToBoolean(Settings.Default.ShowCompleteConsPathInTitle);
			InitializeComponent();
			//Added to support default instance behavour in C#. This should be removed at the earliest opportunity.
            if (defaultInstance == null)
            {
                defaultInstance = this;
            }
		}
        #endregion

        #region Default Instance
        private static frmMain defaultInstance;
		
		/// <summary>
		/// Added by the VB.Net to C# Converter to support default instance behavour in C#
		/// </summary>
        public static frmMain Default
		{
			get
			{
				if (defaultInstance == null)
				{
					defaultInstance = new frmMain();
					defaultInstance.FormClosed += new FormClosedEventHandler(defaultInstance_FormClosed);
				}
				
				return defaultInstance;
			}
			set
			{
				defaultInstance = value;
			}
		}
		
		static void defaultInstance_FormClosed(object sender, FormClosedEventArgs e)
		{
			defaultInstance = null;
		}
        #endregion

        #region Events
        public delegate void clipboardchangeEventHandler();
		
		public static event clipboardchangeEventHandler clipboardchange
		{
			add
			{
				clipboardchangeEvent = (clipboardchangeEventHandler) System.Delegate.Combine(clipboardchangeEvent, value);
			}
			remove
			{
				clipboardchangeEvent = (clipboardchangeEventHandler) System.Delegate.Remove(clipboardchangeEvent, value);
			}
		}
        #endregion

        #region Properties
        public FormWindowState PreviousWindowState
        {
            get { return _previousWindowState; }
            set { _previousWindowState = value; }
        }
		
        public bool IsClosing
		{
			get { return _isClosing; }
		}
		
        public bool UsingSqlServer
		{
			get { return _usingSqlServer; }
			set
			{
				if (_usingSqlServer == value)
				{
					return ;
				}
				_usingSqlServer = value;
				UpdateWindowTitle();
			}
		}
		
        public string ConnectionsFileName
		{
			get { return _connectionsFileName; }
			set
			{
				if (_connectionsFileName == value)
				{
					return ;
				}
				_connectionsFileName = value;
				UpdateWindowTitle();
			}
		}
		
        public bool ShowFullPathInTitle
		{
			get { return _showFullPathInTitle; }
			set
			{
				if (_showFullPathInTitle == value)
				{
					return ;
				}
				_showFullPathInTitle = value;
				UpdateWindowTitle();
			}
		}
		
        public Connection.ConnectionRecordImp SelectedConnection
		{
			get { return _selectedConnection; }
			set
			{
				if (_selectedConnection == value)
				{
					return ;
				}
				_selectedConnection = value;
				UpdateWindowTitle();
			}
		}

        public Tools.Misc.Fullscreen Fullscreen
        {
            get { return _fullscreen; }
            set { _fullscreen = value; }
        }
        #endregion
		
        #region Startup & Shutdown
		public void frmMain_Load(object sender, System.EventArgs e)
		{
            Runtime.MainForm = this;
            //Runtime.Startup.CheckCompatibility();
            Runtime.Startup.CreateLogger();
			
			// Create gui config load and save objects
			Config.Settings.Load SettingsLoad = new Config.Settings.Load(this);
			
			// Load GUI Configuration
			SettingsLoad.LoadSettings();
			
			Debug.Print("---------------------------" + Environment.NewLine + "[START] - " + System.Convert.ToString(DateTime.Now));
            Runtime.Startup.ParseCommandLineArgs();
			ApplyLanguage();
			PopulateQuickConnectProtocolMenu();
			ThemeManager.ThemeChanged += ApplyThemes;
			ApplyThemes();
			fpChainedWindowHandle = Native.SetClipboardViewer(this.Handle);
            Runtime.MessageCollector = new Messages.Collector(Runtime.Windows.errorsForm);
            Runtime.WindowList = new UI.Window.List();
			Tools.IeBrowserEmulation.Register();
            Runtime.Startup.GetConnectionIcons();
            Runtime.Windows.treePanel.Focus();
            Tree.Node.TreeView = Runtime.Windows.treeForm.tvConnections;

            if (My.Settings.Default.FirstStart && !My.Settings.Default.LoadConsFromCustomLocation && !System.IO.File.Exists(Runtime.GetStartupConnectionFileName()))
			{
                Runtime.NewConnections(Runtime.GetStartupConnectionFileName());
			}
			
			//LoadCredentials()
            Runtime.LoadConnections();
            if (!Runtime.IsConnectionsFileLoaded)
			{
				System.Windows.Forms.Application.Exit();
				return ;
			}
			Config.Putty.Sessions.StartWatcher();
			if (My.Settings.Default.StartupComponentsCheck)
			{
                Runtime.Windows.Show(UI.Window.Type.ComponentsCheck);
			}
			
            #if PORTABLE
			mMenInfoAnnouncements.Visible = false;
			mMenToolsUpdate.Visible = false;
			mMenInfoSep2.Visible = false;
            #endif

            Runtime.Startup.CreateSQLUpdateHandlerAndStartTimer();
			AddSysMenuItems();
			Microsoft.Win32.SystemEvents.DisplaySettingsChanged += DisplayChanged;
			this.Opacity = 1;
			KeyboardShortcuts.RequestKeyNotifications(Handle);
		}
		
		private void ApplyLanguage()
		{
			mMenFile.Text = My.Language.strMenuFile;
			mMenFileNew.Text = My.Language.strMenuNewConnectionFile;
			mMenFileNewConnection.Text = My.Language.strNewConnection;
			mMenFileNewFolder.Text = My.Language.strNewFolder;
			mMenFileLoad.Text = My.Language.strMenuOpenConnectionFile;
			mMenFileSave.Text = My.Language.strMenuSaveConnectionFile;
			mMenFileSaveAs.Text = My.Language.strMenuSaveConnectionFileAs;
            mMenFileImport.Text = My.Language.strImportMenuItem;
            mMenFileImportFromFile.Text = My.Language.strImportFromFileMenuItem;
            mMenFileImportFromActiveDirectory.Text = My.Language.strImportAD;
            mMenFileImportFromPortScan.Text = My.Language.strImportPortScan;
            mMenFileExport.Text = My.Language.strExportToFileMenuItem;
			mMenFileExit.Text = My.Language.strMenuExit;
			
			mMenView.Text = My.Language.strMenuView;
			mMenViewAddConnectionPanel.Text = My.Language.strMenuAddConnectionPanel;
			mMenViewConnectionPanels.Text = My.Language.strMenuConnectionPanels;
			mMenViewConnections.Text = My.Language.strMenuConnections;
			mMenViewConfig.Text = My.Language.strMenuConfig;
			mMenViewSessions.Text = My.Language.strMenuSessions;
			mMenViewErrorsAndInfos.Text = My.Language.strMenuNotifications;
			mMenViewScreenshotManager.Text = My.Language.strScreenshots;
			mMenViewJumpTo.Text = My.Language.strMenuJumpTo;
			mMenViewJumpToConnectionsConfig.Text = My.Language.strMenuConnectionsAndConfig;
			mMenViewJumpToSessionsScreenshots.Text = My.Language.strMenuSessionsAndScreenshots;
			mMenViewJumpToErrorsInfos.Text = My.Language.strMenuNotifications;
			mMenViewResetLayout.Text = My.Language.strMenuResetLayout;
			mMenViewQuickConnectToolbar.Text = My.Language.strMenuQuickConnectToolbar;
			mMenViewExtAppsToolbar.Text = My.Language.strMenuExternalToolsToolbar;
			mMenViewFullscreen.Text = My.Language.strMenuFullScreen;
			
			mMenTools.Text = My.Language.strMenuTools;
			mMenToolsSSHTransfer.Text = My.Language.strMenuSSHFileTransfer;
			mMenToolsExternalApps.Text = My.Language.strMenuExternalTools;
			mMenToolsPortScan.Text = My.Language.strMenuPortScan;
			mMenToolsComponentsCheck.Text = My.Language.strComponentsCheck;
			mMenToolsUpdate.Text = My.Language.strMenuCheckForUpdates;
			mMenToolsOptions.Text = My.Language.strMenuOptions;
			
			mMenInfo.Text = My.Language.strMenuHelp;
			mMenInfoHelp.Text = My.Language.strMenuHelpContents;
			mMenInfoForum.Text = My.Language.strMenuSupportForum;
			mMenInfoBugReport.Text = My.Language.strMenuReportBug;
			mMenInfoDonate.Text = My.Language.strMenuDonate;
			mMenInfoWebsite.Text = My.Language.strMenuWebsite;
			mMenInfoAbout.Text = My.Language.strMenuAbout;
			mMenInfoAnnouncements.Text = My.Language.strMenuAnnouncements;
			
			lblQuickConnect.Text = My.Language.strLabelConnect;
			btnQuickConnect.Text = My.Language.strMenuConnect;
			btnConnections.Text = My.Language.strMenuConnections;
			
			cMenToolbarShowText.Text = My.Language.strMenuShowText;
			
			ToolStripButton1.Text = My.Language.strConnect;
			ToolStripButton2.Text = My.Language.strScreenshot;
			ToolStripButton3.Text = My.Language.strRefresh;
			
			ToolStripSplitButton1.Text = My.Language.strSpecialKeys;
			ToolStripMenuItem1.Text = My.Language.strKeysCtrlAltDel;
			ToolStripMenuItem2.Text = My.Language.strKeysCtrlEsc;
		}
		
		public void ApplyThemes()
		{
			pnlDock.DockBackColor = ThemeManager.ActiveTheme.WindowBackgroundColor;
			tsContainer.BackColor = ThemeManager.ActiveTheme.ToolbarBackgroundColor;
			tsContainer.ForeColor = ThemeManager.ActiveTheme.ToolbarTextColor;
			tsContainer.TopToolStripPanel.BackColor = ThemeManager.ActiveTheme.ToolbarBackgroundColor;
			tsContainer.TopToolStripPanel.ForeColor = ThemeManager.ActiveTheme.ToolbarTextColor;
			tsContainer.BottomToolStripPanel.BackColor = ThemeManager.ActiveTheme.ToolbarBackgroundColor;
			tsContainer.BottomToolStripPanel.ForeColor = ThemeManager.ActiveTheme.ToolbarTextColor;
			tsContainer.LeftToolStripPanel.BackColor = ThemeManager.ActiveTheme.ToolbarBackgroundColor;
			tsContainer.LeftToolStripPanel.ForeColor = ThemeManager.ActiveTheme.ToolbarTextColor;
			tsContainer.RightToolStripPanel.BackColor = ThemeManager.ActiveTheme.ToolbarBackgroundColor;
			tsContainer.RightToolStripPanel.ForeColor = ThemeManager.ActiveTheme.ToolbarTextColor;
			tsContainer.ContentPanel.BackColor = ThemeManager.ActiveTheme.ToolbarBackgroundColor;
			tsContainer.ContentPanel.ForeColor = ThemeManager.ActiveTheme.ToolbarTextColor;
			msMain.BackColor = ThemeManager.ActiveTheme.ToolbarBackgroundColor;
			msMain.ForeColor = ThemeManager.ActiveTheme.ToolbarTextColor;
			ApplyMenuColors(msMain.Items);
			tsExternalTools.BackColor = ThemeManager.ActiveTheme.ToolbarBackgroundColor;
			tsExternalTools.ForeColor = ThemeManager.ActiveTheme.ToolbarTextColor;
			tsQuickConnect.BackColor = ThemeManager.ActiveTheme.ToolbarBackgroundColor;
			tsQuickConnect.ForeColor = ThemeManager.ActiveTheme.ToolbarTextColor;
		}
		
		private static void ApplyMenuColors(ToolStripItemCollection itemCollection)
		{
			ToolStripMenuItem menuItem = default(ToolStripMenuItem);
			foreach (ToolStripItem item in itemCollection)
			{
				item.BackColor = ThemeManager.ActiveTheme.MenuBackgroundColor;
				item.ForeColor = ThemeManager.ActiveTheme.MenuTextColor;
				
				menuItem = item as ToolStripMenuItem;
				if (menuItem != null)
				{
					ApplyMenuColors(menuItem.DropDownItems);
				}
			}
		}
		
		public void frmMain_Shown(object sender, EventArgs e)
		{
            #if PORTABLE
			return ;
            #endif
//			if (!My.Settings.Default.CheckForUpdatesAsked)
//			{
//				string[] commandButtons = new string[] {My.Language.strAskUpdatesCommandRecommended, My.Language.strAskUpdatesCommandCustom, My.Language.strAskUpdatesCommandAskLater};
//				cTaskDialog.ShowTaskDialogBox(this, (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.ProductName, My.Language.strAskUpdatesMainInstruction, string.Format(My.Language.strAskUpdatesContent, (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.ProductName), "", "", "", "", string.Join("|", commandButtons), eTaskDialogButtons.None, eSysIcons.Question, eSysIcons.Question);
//				if (cTaskDialog.CommandButtonResult == 0 | cTaskDialog.CommandButtonResult == 1)
//				{
//					My.Settings.Default.CheckForUpdatesAsked = true;
//					}
//					if (cTaskDialog.CommandButtonResult == 1)
//					{
//						Windows.ShowUpdatesTab();
//						}
//						return ;
//						}
						
//						if (!My.Settings.Default.CheckForUpdatesOnStartup)
//						{
//							return ;
//							}
							
//							DateTime nextUpdateCheck = System.Convert.ToDateTime(My.Settings.Default.CheckForUpdatesLastCheck.Add(TimeSpan.FromDays(System.Convert.ToDouble(My.Settings.Default.CheckForUpdatesFrequencyDays))));
//							if (My.Settings.Default.UpdatePending || DateTime.UtcNow > nextUpdateCheck)
//							{
//								if (!IsHandleCreated)
//								{
//									CreateHandle(); // Make sure the handle is created so that InvokeRequired returns the correct result
//									}
//									Startup.CheckForUpdate();
//									Startup.CheckForAnnouncement();
//									}
		}
								
		public void frmMain_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
		{
            if (!(Runtime.WindowList == null || Runtime.WindowList.Count == 0))
			{
				UI.Window.Connection connectionWindow = default(UI.Window.Connection);
				int openConnections = 0;
                foreach (UI.Window.Base window in Runtime.WindowList)
				{
					connectionWindow = window as UI.Window.Connection;
					if (connectionWindow != null)
					{
						openConnections = openConnections + connectionWindow.TabController.TabPages.Count;
					}
				}

                if (openConnections > 0 && (My.Settings.Default.ConfirmCloseConnection == (int)ConfirmClose.All | (My.Settings.Default.ConfirmCloseConnection == (int)ConfirmClose.Multiple & openConnections > 1) || My.Settings.Default.ConfirmCloseConnection == (int)ConfirmClose.Exit))
				{
                    
					DialogResult result = cTaskDialog.MessageBox(this, System.Windows.Forms.Application.ProductName, My.Language.strConfirmExitMainInstruction, "", "", "", My.Language.strCheckboxDoNotShowThisMessageAgain, eTaskDialogButtons.YesNo, eSysIcons.Question, eSysIcons.Question);
					if (cTaskDialog.VerificationChecked)
					{
						My.Settings.Default.ConfirmCloseConnection--;
					}
					if (result == DialogResult.No)
					{
						e.Cancel = true;
						return ;
					}
				}
			}

            Runtime.Shutdown.Cleanup();
									
			_isClosing = true;

            if (Runtime.WindowList != null)
			{
                foreach (UI.Window.Base window in Runtime.WindowList)
				{
					window.Close();
				}
			}

            Runtime.Shutdown.StartUpdate();
									
			Debug.Print("[END] - " + System.Convert.ToString(DateTime.Now));
		}
        #endregion
								
        #region Timer
		public void tmrAutoSave_Tick(System.Object sender, System.EventArgs e)
		{
            Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, "Doing AutoSave", true);
			Runtime.SaveConnections();
		}
        #endregion
		
        #region Ext Apps Toolbar
		public void cMenToolbarShowText_Click(System.Object sender, System.EventArgs e)
		{
			SwitchToolBarText(!cMenToolbarShowText.Checked);
		}
								
		public void AddExternalToolsToToolBar()
		{
			try
			{
				for (int index = tsExternalTools.Items.Count - 1; index >= 0; index--)
				{
					tsExternalTools.Items[index].Dispose();
				}
				tsExternalTools.Items.Clear();
										
				ToolStripButton button = default(ToolStripButton);
                foreach (Tools.ExternalTool tool in Runtime.ExternalTools)
				{
					button = (System.Windows.Forms.ToolStripButton)tsExternalTools.Items.Add(tool.DisplayName, tool.Image, tsExtAppEntry_Click);
											
					if (cMenToolbarShowText.Checked == true)
					{
						button.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
					}
					else
					{
						if (button.Image != null)
						{
							button.DisplayStyle = ToolStripItemDisplayStyle.Image;
						}
						else
						{
							button.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
						}
					}
											
					button.Tag = tool;
				}
			}
			catch (Exception ex)
			{
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, string.Format(My.Language.strErrorAddExternalToolsToToolBarFailed, ex.Message), true);
			}
		}
								
		private void tsExtAppEntry_Click(System.Object sender, System.EventArgs e)
		{
            Tools.ExternalTool extA = (Tools.ExternalTool)((Control)sender).Tag;
									
			if (Tree.Node.GetNodeType(Tree.Node.SelectedNode) == Tree.Node.Type.Connection | Tree.Node.GetNodeType(Tree.Node.SelectedNode) == Tree.Node.Type.PuttySession)
			{
				extA.Start((Connection.ConnectionRecordImp)Tree.Node.SelectedNode.Tag);
			}
			else
			{
				extA.Start();
			}
		}
								
		public void SwitchToolBarText(bool show)
		{
			foreach (ToolStripButton tItem in tsExternalTools.Items)
			{
				if (show == true)
				{
					tItem.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
				}
				else
				{
					if (tItem.Image != null)
					{
						tItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
					}
					else
					{
						tItem.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
					}
				}
			}
									
			cMenToolbarShowText.Checked = show;
		}
        #endregion
								
        #region Menu
        #region File
		public void mMenFile_DropDownOpening(System.Object sender, System.EventArgs e)
		{
			if (Tree.Node.GetNodeType(Tree.Node.SelectedNode) == Tree.Node.Type.Root)
			{
				mMenFileNewConnection.Enabled = true;
				mMenFileNewFolder.Enabled = true;
				mMenFileDelete.Enabled = false;
				mMenFileRename.Enabled = true;
				mMenFileDuplicate.Enabled = false;
				mMenFileDelete.Text = My.Language.strMenuDelete;
				mMenFileRename.Text = My.Language.strMenuRenameFolder;
				mMenFileDuplicate.Text = My.Language.strMenuDuplicate;
			}
			else if (Tree.Node.GetNodeType(Tree.Node.SelectedNode) == Tree.Node.Type.Container)
			{
				mMenFileNewConnection.Enabled = true;
				mMenFileNewFolder.Enabled = true;
				mMenFileDelete.Enabled = true;
				mMenFileRename.Enabled = true;
				mMenFileDuplicate.Enabled = true;
				mMenFileDelete.Text = My.Language.strMenuDeleteFolder;
				mMenFileRename.Text = My.Language.strMenuRenameFolder;
				mMenFileDuplicate.Text = My.Language.strMenuDuplicateFolder;
			}
			else if (Tree.Node.GetNodeType(Tree.Node.SelectedNode) == Tree.Node.Type.Connection)
			{
				mMenFileNewConnection.Enabled = true;
				mMenFileNewFolder.Enabled = true;
				mMenFileDelete.Enabled = true;
				mMenFileRename.Enabled = true;
				mMenFileDuplicate.Enabled = true;
				mMenFileDelete.Text = My.Language.strMenuDeleteConnection;
				mMenFileRename.Text = My.Language.strMenuRenameConnection;
				mMenFileDuplicate.Text = My.Language.strMenuDuplicateConnection;
			}
			else if ((Tree.Node.GetNodeType(Tree.Node.SelectedNode) == Tree.Node.Type.PuttyRoot) || (Tree.Node.GetNodeType(Tree.Node.SelectedNode) == Tree.Node.Type.PuttySession))
			{
				mMenFileNewConnection.Enabled = false;
				mMenFileNewFolder.Enabled = false;
				mMenFileDelete.Enabled = false;
				mMenFileRename.Enabled = false;
				mMenFileDuplicate.Enabled = false;
				mMenFileDelete.Text = My.Language.strMenuDelete;
				mMenFileRename.Text = My.Language.strMenuRename;
				mMenFileDuplicate.Text = My.Language.strMenuDuplicate;
			}
			else
			{
				mMenFileNewConnection.Enabled = true;
				mMenFileNewFolder.Enabled = true;
				mMenFileDelete.Enabled = false;
				mMenFileRename.Enabled = false;
				mMenFileDuplicate.Enabled = false;
				mMenFileDelete.Text = My.Language.strMenuDelete;
				mMenFileRename.Text = My.Language.strMenuRename;
				mMenFileDuplicate.Text = My.Language.strMenuDuplicate;
			}
		}
								
		static public void mMenFileNewConnection_Click(System.Object sender, EventArgs e)
		{
			Runtime.Windows.treeForm.AddConnection();
            Runtime.SaveConnectionsBG();
		}
								
		static public void mMenFileNewFolder_Click(System.Object sender, EventArgs e)
		{
            Runtime.Windows.treeForm.AddFolder();
            Runtime.SaveConnectionsBG();
		}
								
		static public void mMenFileNew_Click(System.Object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog = Tools.Controls.ConnectionsSaveAsDialog();
			if (!(saveFileDialog.ShowDialog() == DialogResult.OK))
			{
				return ;
			}

            Runtime.NewConnections(saveFileDialog.FileName);
		}
								
		static public void mMenFileLoad_Click(System.Object sender, EventArgs e)
		{
            if (Runtime.IsConnectionsFileLoaded)
			{
                DialogResult msgBoxResult = MessageBox.Show(Language.strSaveConnectionsFileBeforeOpeningAnother, Language.strSave, MessageBoxButtons.YesNoCancel);
                switch (msgBoxResult)
				{
                    case DialogResult.Yes:
						Runtime.SaveConnections();
						break;
                    case DialogResult.Cancel:
						return ;
				}
			}

            Runtime.LoadConnections(true);
		}
								
		static public void mMenFileSave_Click(System.Object sender, EventArgs e)
		{
            Runtime.SaveConnections();
		}
								
		static public void mMenFileSaveAs_Click(System.Object sender, EventArgs e)
		{
            Runtime.SaveConnectionsAs();
		}
								
		static public void mMenFileDelete_Click(System.Object sender, EventArgs e)
		{
			Tree.Node.DeleteSelectedNode();
            Runtime.SaveConnectionsBG();
		}
								
		static public void mMenFileRename_Click(System.Object sender, EventArgs e)
		{
			Tree.Node.StartRenameSelectedNode();
            Runtime.SaveConnectionsBG();
		}
								
		static public void mMenFileDuplicate_Click(System.Object sender, EventArgs e)
		{
			Tree.Node.CloneNode(Tree.Node.SelectedNode);
            Runtime.SaveConnectionsBG();
		}
								
		static public void mMenFileImportFromFile_Click(System.Object sender, EventArgs e)
		{
            Import.ImportFromFile(Runtime.Windows.treeForm.tvConnections.Nodes[0], Runtime.Windows.treeForm.tvConnections.SelectedNode);
		}
								
		static public void mMenFileImportFromActiveDirectory_Click(System.Object sender, EventArgs e)
		{
            Runtime.Windows.Show(UI.Window.Type.ActiveDirectoryImport);
		}
								
		static public void mMenFileImportFromPortScan_Click(System.Object sender, EventArgs e)
		{
            Runtime.Windows.Show(UI.Window.Type.PortScan, true);
		}
								
		static public void mMenFileExport_Click(System.Object sender, EventArgs e)
		{
            Export.ExportToFile(Runtime.Windows.treeForm.tvConnections.Nodes[0], Runtime.Windows.treeForm.tvConnections.SelectedNode);
		}
								
		static public void mMenFileExit_Click(System.Object sender, EventArgs e)
		{
            Runtime.Shutdown.Quit();
		}
        #endregion
								
        #region View
		public void mMenView_DropDownOpening(object sender, System.EventArgs e)
		{
            this.mMenViewConnections.Checked = !Runtime.Windows.treeForm.IsHidden;
            this.mMenViewConfig.Checked = !Runtime.Windows.configForm.IsHidden;
            this.mMenViewErrorsAndInfos.Checked = !Runtime.Windows.errorsForm.IsHidden;
            this.mMenViewSessions.Checked = !Runtime.Windows.sessionsForm.IsHidden;
            this.mMenViewScreenshotManager.Checked = !Runtime.Windows.screenshotForm.IsHidden;
									
			this.mMenViewExtAppsToolbar.Checked = tsExternalTools.Visible;
			this.mMenViewQuickConnectToolbar.Checked = tsQuickConnect.Visible;
									
			this.mMenViewConnectionPanels.DropDownItems.Clear();

            for (int i = 0; i <= Runtime.WindowList.Count - 1; i++)
			{
                ToolStripMenuItem tItem = new ToolStripMenuItem(System.Convert.ToString(Runtime.WindowList[i].Text), Runtime.WindowList[i].Icon.ToBitmap(), ConnectionPanelMenuItem_Click);
                tItem.Tag = Runtime.WindowList[i];
				this.mMenViewConnectionPanels.DropDownItems.Add(tItem);
			}
									
			if (this.mMenViewConnectionPanels.DropDownItems.Count > 0)
			{
				this.mMenViewConnectionPanels.Enabled = true;
			}
			else
			{
				this.mMenViewConnectionPanels.Enabled = false;
			}
		}
								
		private void ConnectionPanelMenuItem_Click(object sender, System.EventArgs e)
		{
			(((System.Windows.Forms.Control)sender).Tag as UI.Window.Base).Show(this.pnlDock);
            (((System.Windows.Forms.Control)sender).Tag as UI.Window.Base).Focus();
		}
								
		public void mMenViewSessions_Click(object sender, System.EventArgs e)
		{
			if (this.mMenViewSessions.Checked == false)
			{
                Runtime.Windows.sessionsPanel.Show(this.pnlDock);
				this.mMenViewSessions.Checked = true;
			}
			else
			{
                Runtime.Windows.sessionsPanel.Hide();
				this.mMenViewSessions.Checked = false;
			}
		}
								
		public void mMenViewConnections_Click(System.Object sender, System.EventArgs e)
		{
			if (this.mMenViewConnections.Checked == false)
			{
                Runtime.Windows.treePanel.Show(this.pnlDock);
				this.mMenViewConnections.Checked = true;
			}
			else
			{
                Runtime.Windows.treePanel.Hide();
				this.mMenViewConnections.Checked = false;
			}
		}
								
		public void mMenViewConfig_Click(System.Object sender, System.EventArgs e)
		{
			if (this.mMenViewConfig.Checked == false)
			{
                Runtime.Windows.configPanel.Show(this.pnlDock);
				this.mMenViewConfig.Checked = true;
			}
			else
			{
                Runtime.Windows.configPanel.Hide();
				this.mMenViewConfig.Checked = false;
			}
		}
								
		public void mMenViewErrorsAndInfos_Click(System.Object sender, System.EventArgs e)
		{
			if (this.mMenViewErrorsAndInfos.Checked == false)
			{
                Runtime.Windows.errorsPanel.Show(this.pnlDock);
				this.mMenViewErrorsAndInfos.Checked = true;
			}
			else
			{
                Runtime.Windows.errorsPanel.Hide();
				this.mMenViewErrorsAndInfos.Checked = false;
			}
		}
								
		public void mMenViewScreenshotManager_Click(System.Object sender, System.EventArgs e)
		{
			if (this.mMenViewScreenshotManager.Checked == false)
			{
                Runtime.Windows.screenshotPanel.Show(this.pnlDock);
				this.mMenViewScreenshotManager.Checked = true;
			}
			else
			{
                Runtime.Windows.screenshotPanel.Hide();
				this.mMenViewScreenshotManager.Checked = false;
			}
		}
								
		public void mMenViewJumpToConnectionsConfig_Click(object sender, System.EventArgs e)
		{
            if (pnlDock.ActiveContent == Runtime.Windows.treePanel)
			{
                Runtime.Windows.configForm.Activate();
			}
			else
			{
                Runtime.Windows.treeForm.Activate();
			}
		}
								
		public void mMenViewJumpToSessionsScreenshots_Click(object sender, System.EventArgs e)
		{
            if (pnlDock.ActiveContent == Runtime.Windows.sessionsPanel)
			{
                Runtime.Windows.screenshotForm.Activate();
			}
			else
			{
                Runtime.Windows.sessionsForm.Activate();
			}
		}
								
		public void mMenViewJumpToErrorsInfos_Click(object sender, System.EventArgs e)
		{
            Runtime.Windows.errorsForm.Activate();
		}
								
		public void mMenViewResetLayout_Click(System.Object sender, System.EventArgs e)
		{
            DialogResult msgBoxResult = MessageBox.Show(Language.strConfirmResetLayout, "", MessageBoxButtons.YesNo);
            if (msgBoxResult == DialogResult.Yes)
			{
				App.Runtime.Startup.SetDefaultLayout();
			}
		}
								
		public void mMenViewAddConnectionPanel_Click(System.Object sender, System.EventArgs e)
		{
            Runtime.AddPanel();
		}
								
		public void mMenViewExtAppsToolbar_Click(System.Object sender, System.EventArgs e)
		{
			if (mMenViewExtAppsToolbar.Checked == false)
			{
				tsExternalTools.Visible = true;
				mMenViewExtAppsToolbar.Checked = true;
			}
			else
			{
				tsExternalTools.Visible = false;
				mMenViewExtAppsToolbar.Checked = false;
			}
		}
								
		public void mMenViewQuickConnectToolbar_Click(System.Object sender, System.EventArgs e)
		{
			if (mMenViewQuickConnectToolbar.Checked == false)
			{
				tsQuickConnect.Visible = true;
				mMenViewQuickConnectToolbar.Checked = true;
			}
			else
			{
				tsQuickConnect.Visible = false;
				mMenViewQuickConnectToolbar.Checked = false;
			}
		}
		
		public void mMenViewFullscreen_Click(System.Object sender, System.EventArgs e)
		{
			Fullscreen.Value = !Fullscreen.Value;
			mMenViewFullscreen.Checked = Fullscreen.Value;
		}
        #endregion
								
        #region Tools
		public void mMenToolsUpdate_Click(System.Object sender, System.EventArgs e)
		{
			App.Runtime.Windows.Show(UI.Window.Type.Update);
		}
								
		public void mMenToolsSSHTransfer_Click(System.Object sender, System.EventArgs e)
		{
			App.Runtime.Windows.Show(UI.Window.Type.SSHTransfer);
		}
								
		public void mMenToolsUVNCSC_Click(System.Object sender, System.EventArgs e)
		{
			App.Runtime.Windows.Show(UI.Window.Type.UltraVNCSC);
		}
								
		public void mMenToolsExternalApps_Click(System.Object sender, System.EventArgs e)
		{
			App.Runtime.Windows.Show(UI.Window.Type.ExternalApps);
		}
								
		public void mMenToolsPortScan_Click(System.Object sender, EventArgs e)
		{
			App.Runtime.Windows.Show(UI.Window.Type.PortScan, false);
		}
								
		public void mMenToolsComponentsCheck_Click(System.Object sender, System.EventArgs e)
		{
			App.Runtime.Windows.Show(UI.Window.Type.ComponentsCheck);
		}
								
		public void mMenToolsOptions_Click(System.Object sender, System.EventArgs e)
		{
			App.Runtime.Windows.Show(UI.Window.Type.Options);
		}
        #endregion
								
        #region Quick Connect
		private void PopulateQuickConnectProtocolMenu()
		{
			try
			{
				mnuQuickConnectProtocol.Items.Clear();
				foreach (FieldInfo fieldInfo in typeof(Connection.Protocol.Protocols).GetFields())
				{
					if (!(fieldInfo.Name == "value__" || fieldInfo.Name == "IntApp"))
					{
						ToolStripMenuItem menuItem = new ToolStripMenuItem(fieldInfo.Name);
						if (fieldInfo.Name == My.Settings.Default.QuickConnectProtocol)
						{
							menuItem.Checked = true;
							btnQuickConnect.Text = System.Convert.ToString(My.Settings.Default.QuickConnectProtocol);
						}
						mnuQuickConnectProtocol.Items.Add(menuItem);
					}
				}
			}
			catch (Exception ex)
			{
                Runtime.MessageCollector.AddExceptionMessage("PopulateQuickConnectProtocolMenu() failed.", ex, Messages.MessageClass.ErrorMsg, true);
			}
		}
								
		public void lblQuickConnect_Click(System.Object sender, EventArgs e)
		{
			cmbQuickConnect.Focus();
		}
								
		public void cmbQuickConnect_ConnectRequested(object sender, Controls.QuickConnectComboBox.ConnectRequestedEventArgs e)
		{
			btnQuickConnect_ButtonClick(sender, e);
		}
								
		public void btnQuickConnect_ButtonClick(object sender, EventArgs e)
		{
			try
			{
				Connection.ConnectionRecordImp connectionInfo = Runtime.CreateQuickConnect(cmbQuickConnect.Text.Trim(), Connection.Protocol.ProtocolManagerImp.StringToProtocol(System.Convert.ToString(My.Settings.Default.QuickConnectProtocol)));
				if (connectionInfo == null)
				{
					cmbQuickConnect.Focus();
					return ;
				}
				cmbQuickConnect.Add(connectionInfo);
				Runtime.OpenConnection(connectionInfo, Connection.ConnectionRecordImp.Force.DoNotJump);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage("btnQuickConnect_ButtonClick() failed.", ex, Messages.MessageClass.ErrorMsg, true);
			}
		}
		
		public void cmbQuickConnect_ProtocolChanged(object sender, Controls.QuickConnectComboBox.ProtocolChangedEventArgs e)
		{
			SetQuickConnectProtocol(Connection.Protocol.ProtocolManagerImp.ProtocolToString(e.Protocol));
		}
	    
		public void btnQuickConnect_DropDownItemClicked(System.Object sender, System.Windows.Forms.ToolStripItemClickedEventArgs e)
		{
			SetQuickConnectProtocol(e.ClickedItem.Text);
		}
		
		private void SetQuickConnectProtocol(string protocol)
		{
			My.Settings.Default.QuickConnectProtocol = protocol;
			btnQuickConnect.Text = protocol;
			foreach (ToolStripMenuItem menuItem in mnuQuickConnectProtocol.Items)
			{
				if (menuItem.Text == protocol)
				{
					menuItem.Checked = true;
				}
				else
				{
					menuItem.Checked = false;
				}
			}
		}
        #endregion
								
        #region Info
		public void mMenInfoHelp_Click(System.Object sender, System.EventArgs e)
		{
			Runtime.Windows.Show(UI.Window.Type.Help);
		}
								
		public void mMenInfoForum_Click(System.Object sender, System.EventArgs e)
		{
			App.Runtime.GoToForum();
		}
								
		public void mMenInfoBugReport_Click(System.Object sender, System.EventArgs e)
		{
			App.Runtime.GoToBugs();
		}
								
		public void mMenInfoWebsite_Click(System.Object sender, System.EventArgs e)
		{
			App.Runtime.GoToWebsite();
		}
								
		public void mMenInfoDonate_Click(System.Object sender, System.EventArgs e)
		{
			App.Runtime.GoToDonate();
		}
								
		public void mMenInfoAnnouncements_Click(System.Object sender, System.EventArgs e)
		{
			App.Runtime.Windows.Show(UI.Window.Type.Announcement);
		}
								
		public void mMenInfoAbout_Click(System.Object sender, System.EventArgs e)
		{
			App.Runtime.Windows.Show(UI.Window.Type.About);
		}
        #endregion
        #endregion
		
        #region Connections DropDown
		public void btnConnections_DropDownOpening(object sender, EventArgs e)
		{
			btnConnections.DropDownItems.Clear();	
			foreach (TreeNode treeNode in Runtime.Windows.treeForm.tvConnections.Nodes)
			{
				AddNodeToMenu(treeNode.Nodes, btnConnections);
			}
		}
								
		private static void AddNodeToMenu(TreeNodeCollection treeNodeCollection, ToolStripDropDownItem toolStripMenuItem)
		{
			try
			{
				foreach (TreeNode treeNode in treeNodeCollection)
				{
					ToolStripMenuItem menuItem = new ToolStripMenuItem();
					menuItem.Text = treeNode.Text;
					menuItem.Tag = treeNode;
											
					if (Tree.Node.GetNodeType(treeNode) == Tree.Node.Type.Container)
					{
						menuItem.Image = My.Resources.Folder;
						menuItem.Tag = treeNode.Tag;
												
						toolStripMenuItem.DropDownItems.Add(menuItem);
						AddNodeToMenu(treeNode.Nodes, menuItem);
					}
					else if (Tree.Node.GetNodeType(treeNode) == Tree.Node.Type.Connection | Tree.Node.GetNodeType(treeNode) == Tree.Node.Type.PuttySession)
					{
						menuItem.Image = Runtime.Windows.treeForm.imgListTree.Images[treeNode.ImageIndex];
						menuItem.Tag = treeNode.Tag;
												
						toolStripMenuItem.DropDownItems.Add(menuItem);
					}
											
					menuItem.MouseUp += ConnectionsMenuItem_MouseUp;
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage("frmMain.AddNodeToMenu() failed", ex, Messages.MessageClass.ErrorMsg, true);
			}
		}
								
		private static void ConnectionsMenuItem_MouseUp(System.Object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
                if (((System.Windows.Forms.Control)sender).Tag is Connection.ConnectionRecordImp)
				{
                    Runtime.OpenConnection((Connection.ConnectionRecordImp)((System.Windows.Forms.Control)sender).Tag);
				}
			}
		}
        #endregion
		
        #region Window Overrides and DockPanel Stuff
		public void frmMain_ResizeBegin(object sender, EventArgs e)
		{
			_inSizeMove = true;
		}
	    
		public void frmMain_Resize(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Minimized)
			{
				if (My.Settings.Default.MinimizeToTray)
				{
					if (Runtime.NotificationAreaIcon == null)
					{
                        Runtime.NotificationAreaIcon = new Tools.Controls.NotificationAreaIcon();
					}
					Hide();
				}
			}
			else
			{
				PreviousWindowState = WindowState;
			}
		}
		
		public void frmMain_ResizeEnd(object sender, EventArgs e)
		{
			_inSizeMove = false;			
			// This handles activations from clicks that started a size/move operation
			ActivateConnection();
		}				
		
		protected override void WndProc(ref Message m)
		{
            // Listen for and handle operating system messages
			try
			{
				if (m.Msg == Native.WM_MOUSEACTIVATE)
				{
					_inMouseActivate = true;
				}
                else if (m.Msg == Native.WM_ACTIVATEAPP) // occurs when mRemoteNG becomes the active app
				{
					_inMouseActivate = false;
				}
                else if (m.Msg == Native.WM_ACTIVATE)
				{
					// Ingore this message if it wasn't triggered by a click
                    if (!(Native.LOWORD(m.WParam) == Native.WA_CLICKACTIVE)) { }

                    Control control = FromChildHandle(Native.WindowFromPoint(MousePosition));
					if (!(control == null))
					{
						// Let TreeViews and ComboBoxes get focus but don't simulate a mouse event
						if (control is TreeView|| control is ComboBox) { }
												
						if (control.CanSelect || control is MenuStrip|| control is ToolStrip|| control is Crownwood.Magic.Controls.TabControl|| control is Crownwood.Magic.Controls.InertButton)
						{
							// Simulate a mouse event since one wasn't generated by Windows
							Point clientMousePosition = control.PointToClient(MousePosition);
							System.Int32 temp_wLow = clientMousePosition.X;
							System.Int32 temp_wHigh = clientMousePosition.Y;
                            Native.SendMessage(control.Handle, Native.WM_LBUTTONDOWN, Native.MK_LBUTTON, Native.MAKELPARAM(ref temp_wLow, ref temp_wHigh));
							clientMousePosition.X = temp_wLow;
							clientMousePosition.Y = temp_wHigh;
							control.Focus();
						}
					}
											
					// This handles activations from clicks that did not start a size/move operation
					ActivateConnection();
				}
                else if (m.Msg == Native.WM_WINDOWPOSCHANGED)
				{
					// Ignore this message if the window wasn't activated
                    Native.WINDOWPOS windowPos = (Native.WINDOWPOS)Marshal.PtrToStructure(m.LParam, typeof(Native.WINDOWPOS));
                    if (!((windowPos.flags & Native.SWP_NOACTIVATE) == 0))
					{
					}
											
					// This handles all other activations
					if (!_inMouseActivate && !_inSizeMove)
					{
						ActivateConnection();
					}
				}
                else if (m.Msg == Native.WM_SYSCOMMAND)
				{
					for (int i = 0; i <= SysMenSubItems.Length - 1; i++)
					{
						if (SysMenSubItems[i] == m.WParam.ToInt32())
						{
							Runtime.Screens.SendFormToScreen(Screen.AllScreens[i]);
							break;
						}
					}
				}
                else if (m.Msg == Native.WM_DRAWCLIPBOARD)
				{
					Native.SendMessage(fpChainedWindowHandle, m.Msg, m.LParam.ToInt32(), m.WParam.ToInt32());
                    if (clipboardchangeEvent != null)
                    {
                        clipboardchangeEvent();
                    }
				}
                else if (m.Msg == Native.WM_CHANGECBCHAIN)
				{
					//Send to the next window
                    Native.SendMessage(fpChainedWindowHandle, m.Msg, m.LParam.ToInt32(), m.WParam.ToInt32());
					fpChainedWindowHandle = m.LParam;
				}
				else if (m.Msg == KeyboardHook.HookKeyMsg)
				{
					if (!(m.WParam.ToInt32() == Win32.WM_KEYDOWN))
					{
					}
											
					switch (KeyboardShortcuts.CommandFromHookKeyMessage(m))
					{
						case ShortcutCommand.PreviousTab:
							SelectTabRelative(-1);
							break;
						case ShortcutCommand.NextTab:
							SelectTabRelative(1);
							break;
					}
				}
			}
			catch (Exception)
			{
			}
									
			base.WndProc(ref m);
		}
        
		private void ActivateConnection()
		{
			if (pnlDock.ActiveDocument is UI.Window.Connection)
			{
                UI.Window.Connection cW = pnlDock.ActiveDocument as UI.Window.Connection;
				if (cW.TabController.SelectedTab != null)
				{
					Crownwood.Magic.Controls.TabPage tab = cW.TabController.SelectedTab;
                    Connection.InterfaceControl ifc = (Connection.InterfaceControl)tab.Tag;
					ifc.Protocol.Focus();
					(ifc.FindForm() as UI.Window.Connection).RefreshIC();
				}
			}
		}
		
		public void pnlDock_ActiveDocumentChanged(object sender, EventArgs e)
		{
			ActivateConnection();
            UI.Window.Connection connectionWindow = pnlDock.ActiveDocument as UI.Window.Connection;
			if (connectionWindow != null)
			{
				connectionWindow.UpdateSelectedConnection();
			}
		}
		
		private void UpdateWindowTitle()
		{
			if (InvokeRequired)
			{
				Invoke(new MethodInvoker(UpdateWindowTitle));
				return ;
			}
									
			StringBuilder titleBuilder = new StringBuilder(Application.ProductName);
			const string separator = " - ";
									
			if (Runtime.IsConnectionsFileLoaded)
			{
				if (UsingSqlServer)
				{
					titleBuilder.Append(separator);
					titleBuilder.Append(Language.strSQLServer.TrimEnd(':'));
				}
				else
				{
					if (!string.IsNullOrEmpty(ConnectionsFileName))
					{
						titleBuilder.Append(separator);
						if (My.Settings.Default.ShowCompleteConsPathInTitle)
						{
							titleBuilder.Append(ConnectionsFileName);
						}
						else
						{
							titleBuilder.Append(Path.GetFileName(ConnectionsFileName));
						}
					}
				}
			}
									
			if (!(SelectedConnection == null || string.IsNullOrEmpty(SelectedConnection.Name)))
			{
				titleBuilder.Append(separator);
				titleBuilder.Append(SelectedConnection.Name);
			}
		    
            this.Text = titleBuilder.ToString();
		}
		
		public void ShowHidePanelTabs(DockContent closingDocument = null)
		{
			DocumentStyle newDocumentStyle = pnlDock.DocumentStyle;
									
			if (My.Settings.Default.AlwaysShowPanelTabs)
			{
				newDocumentStyle = DocumentStyle.DockingWindow; // Show the panel tabs
			}
			else
			{
				int nonConnectionPanelCount = 0;
				foreach (DockContent document in pnlDock.Documents)
				{
					if ((closingDocument == null || document != closingDocument) && !(document is UI.Window.Connection))
					{
						nonConnectionPanelCount++;
					}
				}
										
				if (nonConnectionPanelCount == 0)
				{
					newDocumentStyle = DocumentStyle.DockingSdi; // Hide the panel tabs
				}
				else
				{
					newDocumentStyle = DocumentStyle.DockingWindow; // Show the panel tabs
				}
			}
									
			if (!(pnlDock.DocumentStyle == newDocumentStyle))
			{
				pnlDock.DocumentStyle = newDocumentStyle;
				pnlDock.Size = new Size(1, 1);
			}
		}
		
		private void SelectTabRelative(int relativeIndex)
		{
			if (!(pnlDock.ActiveDocument is UI.Window.Connection))
			{
				return ;
			}

            UI.Window.Connection connectionWindow = (UI.Window.Connection)pnlDock.ActiveDocument;
			Crownwood.Magic.Controls.TabControl tabController = connectionWindow.TabController;
									
			int newIndex = tabController.SelectedIndex + relativeIndex;
			while (newIndex < 0 | newIndex >= tabController.TabPages.Count)
			{
				if (newIndex < 0)
				{
					newIndex = tabController.TabPages.Count + newIndex;
				}
				if (newIndex >= tabController.TabPages.Count)
				{
					newIndex = newIndex - tabController.TabPages.Count;
				}
			}
									
			tabController.SelectedIndex = newIndex;
		}
        #endregion
		
        #region Screen Stuff
		private void DisplayChanged(object sender, System.EventArgs e)
		{
			ResetSysMenuItems();
			AddSysMenuItems();
		}
		
		private static void ResetSysMenuItems()
		{
			Runtime.SystemMenu.Reset();
		}
								
		private void AddSysMenuItems()
		{
            Runtime.SystemMenu = new Tools.SystemMenu(this.Handle);
            IntPtr popMen = Runtime.SystemMenu.CreatePopupMenuItem();
									
			for (int i = 0; i <= Screen.AllScreens.Length - 1; i++)
			{
				SysMenSubItems[i] = 200 + i;
                Runtime.SystemMenu.AppendMenuItem(popMen, Tools.SystemMenu.Flags.MF_STRING, new IntPtr(SysMenSubItems[i]), My.Language.strScreen + " " + System.Convert.ToString(i + 1));
			}

            Runtime.SystemMenu.InsertMenuItem(Runtime.SystemMenu.SystemMenuHandle, 0, Tools.SystemMenu.Flags.MF_POPUP | Tools.SystemMenu.Flags.MF_BYPOSITION, popMen, My.Language.strSendTo);
            Runtime.SystemMenu.InsertMenuItem(Runtime.SystemMenu.SystemMenuHandle, 1, Tools.SystemMenu.Flags.MF_BYPOSITION | Tools.SystemMenu.Flags.MF_SEPARATOR, IntPtr.Zero, null);
		}
        #endregion
	}					
}