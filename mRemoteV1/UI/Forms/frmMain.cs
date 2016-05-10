using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Config;
using mRemoteNG.Config.KeyboardShortcuts;
using mRemoteNG.Config.Settings;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Controls;
using mRemoteNG.Messages;
using mRemoteNG.My;
using mRemoteNG.Themes;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.UI.Window;
using PSTaskDialog;
using SharedLibraryNG;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.UI.Forms
{
	public partial class frmMain
    {
        #region Private Variables
        private static clipboardchangeEventHandler clipboardchangeEvent;
        private bool _inSizeMove = false;
        private bool _inMouseActivate = false;
        private IntPtr fpChainedWindowHandle;
        private int[] SysMenSubItems = new int[51];
        private FormWindowState _previousWindowState;
        private bool _isClosing = false;
        private bool _usingSqlServer = false;
        private string _connectionsFileName = null;
        private bool _showFullPathInTitle;
        private ConnectionInfo _selectedConnection = null;
        public MiscTools.Fullscreen _fullscreen;
        #endregion

        #region Constructors
        public frmMain()
		{
			_showFullPathInTitle = mRemoteNG.Settings.Default.ShowCompleteConsPathInTitle;
			InitializeComponent();
			//Added to support default instance behavour in C#. This should be removed at the earliest opportunity.
            if (_defaultInstance == null)
                _defaultInstance = this;
		}
        #endregion

        #region Default Instance
        private static frmMain _defaultInstance;
		
		/// <summary>
		/// Added by the VB.Net to C# Converter to support default instance behavour in C#
		/// </summary>
        public static frmMain Default
		{
			get
			{
				if (_defaultInstance == null)
				{
					_defaultInstance = new frmMain();
					_defaultInstance.FormClosed += defaultInstance_FormClosed;
				}
				
				return _defaultInstance;
			}
			set
			{
				_defaultInstance = value;
			}
		}
		
		static void defaultInstance_FormClosed(object sender, FormClosedEventArgs e)
		{
			_defaultInstance = null;
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
		
        public bool AreWeUsingSqlServerForSavingConnections
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
		
        public ConnectionInfo SelectedConnection
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

        public MiscTools.Fullscreen Fullscreen
        {
            get { return _fullscreen; }
            set { _fullscreen = value; }
        }
        #endregion
		
        #region Startup & Shutdown
		public void frmMain_Load(object sender, EventArgs e)
		{
            Runtime.MainForm = this;
            
            Startup.CreateLogger();
            Startup.LogStartupData();

            //Runtime.Startup.CheckCompatibility();

            // Create gui config load and save objects
            SettingsLoader settingsLoader = new SettingsLoader(this);
			settingsLoader.LoadSettings();
			
			Debug.Print("---------------------------" + Environment.NewLine + "[START] - " + Convert.ToString(DateTime.Now));
            Startup.ParseCommandLineArgs();
			ApplyLanguage();
			PopulateQuickConnectProtocolMenu();
			ThemeManager.ThemeChanged += ApplyThemes;
			ApplyThemes();
			fpChainedWindowHandle = Native.SetClipboardViewer(this.Handle);
            Runtime.MessageCollector = new MessageCollector(Windows.errorsForm);
            Runtime.WindowList = new WindowList();
            //Startup.CreatePanels();
            //Startup.SetDefaultLayout();
            IeBrowserEmulation.Register();
            Startup.GetConnectionIcons();
            Windows.treePanel.Focus();
            ConnectionTree.TreeView = Windows.treeForm.tvConnections;

            if (mRemoteNG.Settings.Default.FirstStart && !mRemoteNG.Settings.Default.LoadConsFromCustomLocation && !System.IO.File.Exists(Runtime.GetStartupConnectionFileName()))
			{
                Runtime.NewConnections(Runtime.GetStartupConnectionFileName());
			}

            //LoadCredentials()
            Runtime.LoadConnections();
            if (!Runtime.IsConnectionsFileLoaded)
			{
				Application.Exit();
				return ;
			}
			Config.Putty.Sessions.StartWatcher();
			if (mRemoteNG.Settings.Default.StartupComponentsCheck)
			{
                Windows.Show(WindowType.ComponentsCheck);
			}

            ApplySpecialSettingsForPortableVersion();

            Startup.CreateConnectionsProvider();
			AddSysMenuItems();
			Microsoft.Win32.SystemEvents.DisplaySettingsChanged += DisplayChanged;
			this.Opacity = 1;
			KeyboardShortcuts.RequestKeyNotifications(Handle);
		}

        private void ApplySpecialSettingsForPortableVersion()
        {
            #if PORTABLE
            mMenInfoAnnouncements.Visible = false;
            mMenToolsUpdate.Visible = false;
            mMenInfoSep2.Visible = false;
            #endif
        }
		
		private void ApplyLanguage()
		{
			mMenFile.Text = Language.strMenuFile;
			mMenFileNew.Text = Language.strMenuNewConnectionFile;
			mMenFileNewConnection.Text = Language.strNewConnection;
			mMenFileNewFolder.Text = Language.strNewFolder;
			mMenFileLoad.Text = Language.strMenuOpenConnectionFile;
			mMenFileSave.Text = Language.strMenuSaveConnectionFile;
			mMenFileSaveAs.Text = Language.strMenuSaveConnectionFileAs;
            mMenFileImport.Text = Language.strImportMenuItem;
            mMenFileImportFromFile.Text = Language.strImportFromFileMenuItem;
            mMenFileImportFromActiveDirectory.Text = Language.strImportAD;
            mMenFileImportFromPortScan.Text = Language.strImportPortScan;
            mMenFileExport.Text = Language.strExportToFileMenuItem;
			mMenFileExit.Text = Language.strMenuExit;
			
			mMenView.Text = Language.strMenuView;
			mMenViewAddConnectionPanel.Text = Language.strMenuAddConnectionPanel;
			mMenViewConnectionPanels.Text = Language.strMenuConnectionPanels;
			mMenViewConnections.Text = Language.strMenuConnections;
			mMenViewConfig.Text = Language.strMenuConfig;
			mMenViewErrorsAndInfos.Text = Language.strMenuNotifications;
			mMenViewScreenshotManager.Text = Language.strScreenshots;
			mMenViewJumpTo.Text = Language.strMenuJumpTo;
			mMenViewJumpToConnectionsConfig.Text = Language.strMenuConnectionsAndConfig;
			mMenViewJumpToErrorsInfos.Text = Language.strMenuNotifications;
			mMenViewResetLayout.Text = Language.strMenuResetLayout;
			mMenViewQuickConnectToolbar.Text = Language.strMenuQuickConnectToolbar;
			mMenViewExtAppsToolbar.Text = Language.strMenuExternalToolsToolbar;
			mMenViewFullscreen.Text = Language.strMenuFullScreen;
			
			mMenTools.Text = Language.strMenuTools;
			mMenToolsSSHTransfer.Text = Language.strMenuSSHFileTransfer;
			mMenToolsExternalApps.Text = Language.strMenuExternalTools;
			mMenToolsPortScan.Text = Language.strMenuPortScan;
			mMenToolsComponentsCheck.Text = Language.strComponentsCheck;
			mMenToolsUpdate.Text = Language.strMenuCheckForUpdates;
			mMenToolsOptions.Text = Language.strMenuOptions;
			
			mMenInfo.Text = Language.strMenuHelp;
			mMenInfoHelp.Text = Language.strMenuHelpContents;
			mMenInfoForum.Text = Language.strMenuSupportForum;
			mMenInfoBugReport.Text = Language.strMenuReportBug;
			mMenInfoDonate.Text = Language.strMenuDonate;
			mMenInfoWebsite.Text = Language.strMenuWebsite;
			mMenInfoAbout.Text = Language.strMenuAbout;
			mMenInfoAnnouncements.Text = Language.strMenuAnnouncements;
			
			lblQuickConnect.Text = Language.strLabelConnect;
			btnQuickConnect.Text = Language.strMenuConnect;
			btnConnections.Text = Language.strMenuConnections;
			
			cMenToolbarShowText.Text = Language.strMenuShowText;
			
			ToolStripButton1.Text = Language.strConnect;
			ToolStripButton2.Text = Language.strScreenshot;
			ToolStripButton3.Text = Language.strRefresh;
			
			ToolStripSplitButton1.Text = Language.strSpecialKeys;
			ToolStripMenuItem1.Text = Language.strKeysCtrlAltDel;
			ToolStripMenuItem2.Text = Language.strKeysCtrlEsc;
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
//			if (!mRemoteNG.Settings.Default.CheckForUpdatesAsked)
//			{
//				string[] commandButtons = new string[] {Language.strAskUpdatesCommandRecommended, Language.strAskUpdatesCommandCustom, Language.strAskUpdatesCommandAskLater};
//				cTaskDialog.ShowTaskDialogBox(this, (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.ProductName, Language.strAskUpdatesMainInstruction, string.Format(Language.strAskUpdatesContent, (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.ProductName), "", "", "", "", string.Join("|", commandButtons), eTaskDialogButtons.None, eSysIcons.Question, eSysIcons.Question);
//				if (cTaskDialog.CommandButtonResult == 0 | cTaskDialog.CommandButtonResult == 1)
//				{
//					mRemoteNG.Settings.Default.CheckForUpdatesAsked = true;
//					}
//					if (cTaskDialog.CommandButtonResult == 1)
//					{
//						Windows.ShowUpdatesTab();
//						}
//						return ;
//						}
						
//						if (!mRemoteNG.Settings.Default.CheckForUpdatesOnStartup)
//						{
//							return ;
//							}
							
//							DateTime nextUpdateCheck = System.Convert.ToDateTime(mRemoteNG.Settings.Default.CheckForUpdatesLastCheck.Add(TimeSpan.FromDays(System.Convert.ToDouble(mRemoteNG.Settings.Default.CheckForUpdatesFrequencyDays))));
//							if (mRemoteNG.Settings.Default.UpdatePending || DateTime.UtcNow > nextUpdateCheck)
//							{
//								if (!IsHandleCreated)
//								{
//									CreateHandle(); // Make sure the handle is created so that InvokeRequired returns the correct result
//									}
//									Startup.CheckForUpdate();
//									Startup.CheckForAnnouncement();
//									}
		}
								
		public void frmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
            if (!(Runtime.WindowList == null || Runtime.WindowList.Count == 0))
			{
				ConnectionWindow connectionWindow = default(ConnectionWindow);
				int openConnections = 0;
                foreach (BaseWindow window in Runtime.WindowList)
				{
					connectionWindow = window as ConnectionWindow;
					if (connectionWindow != null)
						openConnections = openConnections + connectionWindow.TabController.TabPages.Count;
				}

                if (openConnections > 0 && (mRemoteNG.Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.All | (mRemoteNG.Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.Multiple & openConnections > 1) || mRemoteNG.Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.Exit))
				{
					DialogResult result = cTaskDialog.MessageBox(this, System.Windows.Forms.Application.ProductName, Language.strConfirmExitMainInstruction, "", "", "", Language.strCheckboxDoNotShowThisMessageAgain, eTaskDialogButtons.YesNo, eSysIcons.Question, eSysIcons.Question);
					if (cTaskDialog.VerificationChecked)
					{
                        mRemoteNG.Settings.Default.ConfirmCloseConnection--;
					}
					if (result == DialogResult.No)
					{
						e.Cancel = true;
						return ;
					}
				}
			}

            Shutdown.Cleanup();
									
			_isClosing = true;

            if (Runtime.WindowList != null)
			{
                foreach (BaseWindow window in Runtime.WindowList)
				{
					window.Close();
				}
			}

            Shutdown.StartUpdate();
									
			Debug.Print("[END] - " + Convert.ToString(DateTime.Now));
		}
        #endregion
								
        #region Timer
		public void tmrAutoSave_Tick(Object sender, EventArgs e)
		{
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Doing AutoSave", true);
			Runtime.SaveConnections();
		}
        #endregion
		
        #region Ext Apps Toolbar
		public void cMenToolbarShowText_Click(Object sender, EventArgs e)
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
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, string.Format(Language.strErrorAddExternalToolsToToolBarFailed, ex.Message), true);
			}
		}
								
		private void tsExtAppEntry_Click(Object sender, EventArgs e)
		{
            Tools.ExternalTool extA = (Tools.ExternalTool)((Control)sender).Tag;

            if (Tree.ConnectionTreeNode.GetNodeType(ConnectionTree.SelectedNode) == TreeNodeType.Connection | Tree.ConnectionTreeNode.GetNodeType(ConnectionTree.SelectedNode) == TreeNodeType.PuttySession)
			{
                extA.Start((ConnectionInfo)ConnectionTree.SelectedNode.Tag);
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
						tItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
					else
						tItem.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
				}
			}
									
			cMenToolbarShowText.Checked = show;
		}
        #endregion
								
        #region Menu
        #region File
		public void mMenFile_DropDownOpening(System.Object sender, System.EventArgs e)
		{
            if (Tree.ConnectionTreeNode.GetNodeType(ConnectionTree.SelectedNode) == TreeNodeType.Root)
			{
				mMenFileNewConnection.Enabled = true;
				mMenFileNewFolder.Enabled = true;
				mMenFileDelete.Enabled = false;
				mMenFileRename.Enabled = true;
				mMenFileDuplicate.Enabled = false;
				mMenFileDelete.Text = Language.strMenuDelete;
				mMenFileRename.Text = Language.strMenuRenameFolder;
				mMenFileDuplicate.Text = Language.strMenuDuplicate;
			}
            else if (Tree.ConnectionTreeNode.GetNodeType(ConnectionTree.SelectedNode) == TreeNodeType.Container)
			{
				mMenFileNewConnection.Enabled = true;
				mMenFileNewFolder.Enabled = true;
				mMenFileDelete.Enabled = true;
				mMenFileRename.Enabled = true;
				mMenFileDuplicate.Enabled = true;
				mMenFileDelete.Text = Language.strMenuDeleteFolder;
				mMenFileRename.Text = Language.strMenuRenameFolder;
				mMenFileDuplicate.Text = Language.strMenuDuplicateFolder;
			}
            else if (Tree.ConnectionTreeNode.GetNodeType(ConnectionTree.SelectedNode) == TreeNodeType.Connection)
			{
				mMenFileNewConnection.Enabled = true;
				mMenFileNewFolder.Enabled = true;
				mMenFileDelete.Enabled = true;
				mMenFileRename.Enabled = true;
				mMenFileDuplicate.Enabled = true;
				mMenFileDelete.Text = Language.strMenuDeleteConnection;
				mMenFileRename.Text = Language.strMenuRenameConnection;
				mMenFileDuplicate.Text = Language.strMenuDuplicateConnection;
			}
            else if ((Tree.ConnectionTreeNode.GetNodeType(ConnectionTree.SelectedNode) == TreeNodeType.PuttyRoot) || (Tree.ConnectionTreeNode.GetNodeType(ConnectionTree.SelectedNode) == TreeNodeType.PuttySession))
			{
				mMenFileNewConnection.Enabled = false;
				mMenFileNewFolder.Enabled = false;
				mMenFileDelete.Enabled = false;
				mMenFileRename.Enabled = false;
				mMenFileDuplicate.Enabled = false;
				mMenFileDelete.Text = Language.strMenuDelete;
				mMenFileRename.Text = Language.strMenuRename;
				mMenFileDuplicate.Text = Language.strMenuDuplicate;
			}
			else
			{
				mMenFileNewConnection.Enabled = true;
				mMenFileNewFolder.Enabled = true;
				mMenFileDelete.Enabled = false;
				mMenFileRename.Enabled = false;
				mMenFileDuplicate.Enabled = false;
				mMenFileDelete.Text = Language.strMenuDelete;
				mMenFileRename.Text = Language.strMenuRename;
				mMenFileDuplicate.Text = Language.strMenuDuplicate;
			}
		}
								
		static public void mMenFileNewConnection_Click(Object sender, EventArgs e)
		{
			Windows.treeForm.AddConnection();
            Runtime.SaveConnectionsBG();
		}
								
		static public void mMenFileNewFolder_Click(Object sender, EventArgs e)
		{
            Windows.treeForm.AddFolder();
            Runtime.SaveConnectionsBG();
		}
								
		static public void mMenFileNew_Click(Object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog = Tools.Controls.ConnectionsSaveAsDialog();
			if (!(saveFileDialog.ShowDialog() == DialogResult.OK))
			{
				return ;
			}

            Runtime.NewConnections(saveFileDialog.FileName);
		}
								
		static public void mMenFileLoad_Click(Object sender, EventArgs e)
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
								
		static public void mMenFileSave_Click(Object sender, EventArgs e)
		{
            Runtime.SaveConnections();
		}
								
		static public void mMenFileSaveAs_Click(Object sender, EventArgs e)
		{
            Runtime.SaveConnectionsAs();
		}
								
		static public void mMenFileDelete_Click(Object sender, EventArgs e)
		{
            ConnectionTree.DeleteSelectedNode();
            Runtime.SaveConnectionsBG();
		}
								
		static public void mMenFileRename_Click(Object sender, EventArgs e)
		{
			ConnectionTree.StartRenameSelectedNode();
            Runtime.SaveConnectionsBG();
		}
								
		static public void mMenFileDuplicate_Click(Object sender, EventArgs e)
		{
            Tree.ConnectionTreeNode.CloneNode(ConnectionTree.SelectedNode);
            Runtime.SaveConnectionsBG();
		}
								
		static public void mMenFileImportFromFile_Click(Object sender, EventArgs e)
		{
            Import.ImportFromFile(Windows.treeForm.tvConnections.Nodes[0], Windows.treeForm.tvConnections.SelectedNode);
		}
								
		static public void mMenFileImportFromActiveDirectory_Click(Object sender, EventArgs e)
		{
            Windows.Show(UI.Window.WindowType.ActiveDirectoryImport);
		}
								
		static public void mMenFileImportFromPortScan_Click(Object sender, EventArgs e)
		{
            Windows.Show(UI.Window.WindowType.PortScan, true);
		}
								
		static public void mMenFileExport_Click(Object sender, EventArgs e)
		{
            Export.ExportToFile(Windows.treeForm.tvConnections.Nodes[0], Windows.treeForm.tvConnections.SelectedNode);
		}
								
		static public void mMenFileExit_Click(Object sender, EventArgs e)
		{
            Shutdown.Quit();
		}
        #endregion
								
        #region View
		public void mMenView_DropDownOpening(object sender, EventArgs e)
		{
            this.mMenViewConnections.Checked = !Windows.treeForm.IsHidden;
            this.mMenViewConfig.Checked = !Windows.configForm.IsHidden;
            this.mMenViewErrorsAndInfos.Checked = !Windows.errorsForm.IsHidden;
            this.mMenViewScreenshotManager.Checked = !Windows.screenshotForm.IsHidden;
									
			this.mMenViewExtAppsToolbar.Checked = tsExternalTools.Visible;
			this.mMenViewQuickConnectToolbar.Checked = tsQuickConnect.Visible;
									
			this.mMenViewConnectionPanels.DropDownItems.Clear();

            for (int i = 0; i <= Runtime.WindowList.Count - 1; i++)
			{
                ToolStripMenuItem tItem = new ToolStripMenuItem(Convert.ToString(Runtime.WindowList[i].Text), Runtime.WindowList[i].Icon.ToBitmap(), ConnectionPanelMenuItem_Click);
                tItem.Tag = Runtime.WindowList[i];
				this.mMenViewConnectionPanels.DropDownItems.Add(tItem);
			}
									
			if (this.mMenViewConnectionPanels.DropDownItems.Count > 0)
				this.mMenViewConnectionPanels.Enabled = true;
			else
				this.mMenViewConnectionPanels.Enabled = false;
		}
								
		private void ConnectionPanelMenuItem_Click(object sender, EventArgs e)
		{
			(((Control)sender).Tag as BaseWindow).Show(this.pnlDock);
            (((Control)sender).Tag as BaseWindow).Focus();
		}
								
		public void mMenViewConnections_Click(Object sender, EventArgs e)
		{
			if (this.mMenViewConnections.Checked == false)
			{
                Windows.treePanel.Show(this.pnlDock);
				this.mMenViewConnections.Checked = true;
			}
			else
			{
                Windows.treePanel.Hide();
				this.mMenViewConnections.Checked = false;
			}
		}
								
		public void mMenViewConfig_Click(Object sender, EventArgs e)
		{
			if (this.mMenViewConfig.Checked == false)
			{
                Windows.configPanel.Show(this.pnlDock);
				this.mMenViewConfig.Checked = true;
			}
			else
			{
                Windows.configPanel.Hide();
				this.mMenViewConfig.Checked = false;
			}
		}
								
		public void mMenViewErrorsAndInfos_Click(Object sender, EventArgs e)
		{
			if (this.mMenViewErrorsAndInfos.Checked == false)
			{
                Windows.errorsPanel.Show(this.pnlDock);
				this.mMenViewErrorsAndInfos.Checked = true;
			}
			else
			{
                Windows.errorsPanel.Hide();
				this.mMenViewErrorsAndInfos.Checked = false;
			}
		}
								
		public void mMenViewScreenshotManager_Click(Object sender, EventArgs e)
		{
			if (this.mMenViewScreenshotManager.Checked == false)
			{
                Windows.screenshotPanel.Show(this.pnlDock);
				this.mMenViewScreenshotManager.Checked = true;
			}
			else
			{
                Windows.screenshotPanel.Hide();
				this.mMenViewScreenshotManager.Checked = false;
			}
		}
								
		public void mMenViewJumpToConnectionsConfig_Click(object sender, EventArgs e)
		{
            if (pnlDock.ActiveContent == Windows.treePanel)
			{
                Windows.configForm.Activate();
			}
			else
			{
                Windows.treeForm.Activate();
			}
		}
								
		public void mMenViewJumpToErrorsInfos_Click(object sender, EventArgs e)
		{
            Windows.errorsForm.Activate();
		}
								
		public void mMenViewResetLayout_Click(Object sender, EventArgs e)
		{
            DialogResult msgBoxResult = MessageBox.Show(Language.strConfirmResetLayout, "", MessageBoxButtons.YesNo);
            if (msgBoxResult == DialogResult.Yes)
			{
				Startup.SetDefaultLayout();
			}
		}
								
		public void mMenViewAddConnectionPanel_Click(Object sender, EventArgs e)
		{
            Runtime.AddPanel();
		}
								
		public void mMenViewExtAppsToolbar_Click(Object sender, EventArgs e)
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
								
		public void mMenViewQuickConnectToolbar_Click(Object sender, EventArgs e)
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
		
		public void mMenViewFullscreen_Click(Object sender, EventArgs e)
		{
			Fullscreen.Value = !Fullscreen.Value;
			mMenViewFullscreen.Checked = Fullscreen.Value;
		}
        #endregion
								
        #region Tools
		public void mMenToolsUpdate_Click(Object sender, EventArgs e)
		{
			Windows.Show(WindowType.Update);
		}
								
		public void mMenToolsSSHTransfer_Click(Object sender, EventArgs e)
		{
			Windows.Show(WindowType.SSHTransfer);
		}
								
		public void mMenToolsUVNCSC_Click(Object sender, EventArgs e)
		{
			Windows.Show(WindowType.UltraVNCSC);
		}
								
		public void mMenToolsExternalApps_Click(Object sender, EventArgs e)
		{
			Windows.Show(WindowType.ExternalApps);
		}
								
		public void mMenToolsPortScan_Click(Object sender, EventArgs e)
		{
			Windows.Show(WindowType.PortScan, false);
		}
								
		public void mMenToolsComponentsCheck_Click(Object sender, EventArgs e)
		{
			Windows.Show(WindowType.ComponentsCheck);
		}
								
		public void mMenToolsOptions_Click(Object sender, EventArgs e)
		{
			Windows.Show(WindowType.Options);
		}
        #endregion
								
        #region Quick Connect
		private void PopulateQuickConnectProtocolMenu()
		{
			try
			{
				mnuQuickConnectProtocol.Items.Clear();
				foreach (FieldInfo fieldInfo in typeof(ProtocolType).GetFields())
				{
					if (!(fieldInfo.Name == "value__" || fieldInfo.Name == "IntApp"))
					{
						ToolStripMenuItem menuItem = new ToolStripMenuItem(fieldInfo.Name);
						if (fieldInfo.Name == mRemoteNG.Settings.Default.QuickConnectProtocol)
						{
							menuItem.Checked = true;
							btnQuickConnect.Text = mRemoteNG.Settings.Default.QuickConnectProtocol;
						}
						mnuQuickConnectProtocol.Items.Add(menuItem);
					}
				}
			}
			catch (Exception ex)
			{
                Runtime.MessageCollector.AddExceptionMessage("PopulateQuickConnectProtocolMenu() failed.", ex, MessageClass.ErrorMsg, true);
			}
		}
								
		public void lblQuickConnect_Click(Object sender, EventArgs e)
		{
			cmbQuickConnect.Focus();
		}
								
		public void cmbQuickConnect_ConnectRequested(object sender, QuickConnectComboBox.ConnectRequestedEventArgs e)
		{
			btnQuickConnect_ButtonClick(sender, e);
		}
								
		public void btnQuickConnect_ButtonClick(object sender, EventArgs e)
		{
			try
			{
				ConnectionInfo connectionInfo = Runtime.CreateQuickConnect(cmbQuickConnect.Text.Trim(), Connection.Protocol.Converter.StringToProtocol(mRemoteNG.Settings.Default.QuickConnectProtocol));
				if (connectionInfo == null)
				{
					cmbQuickConnect.Focus();
					return ;
				}
				cmbQuickConnect.Add(connectionInfo);
				Runtime.OpenConnection(connectionInfo, ConnectionInfo.Force.DoNotJump);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage("btnQuickConnect_ButtonClick() failed.", ex, MessageClass.ErrorMsg, true);
			}
		}
		
		public void cmbQuickConnect_ProtocolChanged(object sender, QuickConnectComboBox.ProtocolChangedEventArgs e)
		{
			SetQuickConnectProtocol(Connection.Protocol.Converter.ProtocolToString(e.Protocol));
		}
	    
		public void btnQuickConnect_DropDownItemClicked(Object sender, ToolStripItemClickedEventArgs e)
		{
			SetQuickConnectProtocol(e.ClickedItem.Text);
		}
		
		private void SetQuickConnectProtocol(string protocol)
		{
            mRemoteNG.Settings.Default.QuickConnectProtocol = protocol;
			btnQuickConnect.Text = protocol;
			foreach (ToolStripMenuItem menuItem in mnuQuickConnectProtocol.Items)
			{
				if (menuItem.Text == protocol)
					menuItem.Checked = true;
				else
					menuItem.Checked = false;
			}
		}
        #endregion
								
        #region Info
		public void mMenInfoHelp_Click(Object sender, EventArgs e)
		{
			Windows.Show(WindowType.Help);
		}
								
		public void mMenInfoForum_Click(Object sender, EventArgs e)
		{
			Runtime.GoToForum();
		}
								
		public void mMenInfoBugReport_Click(Object sender, EventArgs e)
		{
			Runtime.GoToBugs();
		}
								
		public void mMenInfoWebsite_Click(Object sender, EventArgs e)
		{
			Runtime.GoToWebsite();
		}
								
		public void mMenInfoDonate_Click(Object sender, EventArgs e)
		{
			Runtime.GoToDonate();
		}
								
		public void mMenInfoAnnouncements_Click(Object sender, EventArgs e)
		{
			Windows.Show(WindowType.Announcement);
		}
								
		public void mMenInfoAbout_Click(Object sender, EventArgs e)
		{
			Windows.Show(WindowType.About);
		}
        #endregion
        #endregion
		
        #region Connections DropDown
		public void btnConnections_DropDownOpening(object sender, EventArgs e)
		{
			btnConnections.DropDownItems.Clear();	
			foreach (TreeNode treeNode in Windows.treeForm.tvConnections.Nodes)
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
											
					if (Tree.ConnectionTreeNode.GetNodeType(treeNode) == TreeNodeType.Container)
					{
						menuItem.Image = Resources.Folder;
						menuItem.Tag = treeNode.Tag;
												
						toolStripMenuItem.DropDownItems.Add(menuItem);
						AddNodeToMenu(treeNode.Nodes, menuItem);
					}
					else if (Tree.ConnectionTreeNode.GetNodeType(treeNode) == TreeNodeType.Connection | Tree.ConnectionTreeNode.GetNodeType(treeNode) == TreeNodeType.PuttySession)
					{
						menuItem.Image = Windows.treeForm.imgListTree.Images[treeNode.ImageIndex];
						menuItem.Tag = treeNode.Tag;
												
						toolStripMenuItem.DropDownItems.Add(menuItem);
					}
											
					menuItem.MouseUp += ConnectionsMenuItem_MouseUp;
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage("frmMain.AddNodeToMenu() failed", ex, MessageClass.ErrorMsg, true);
			}
		}
								
		private static void ConnectionsMenuItem_MouseUp(Object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
                if (((Control)sender).Tag is ConnectionInfo)
				{
                    Runtime.OpenConnection((ConnectionInfo)((Control)sender).Tag);
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
				if (mRemoteNG.Settings.Default.MinimizeToTray)
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
		
		protected override void WndProc(ref System.Windows.Forms.Message m)
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
							Int32 temp_wLow = clientMousePosition.X;
							Int32 temp_wHigh = clientMousePosition.Y;
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
							Screens.SendFormToScreen(Screen.AllScreens[i]);
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
			if (pnlDock.ActiveDocument is ConnectionWindow)
			{
                ConnectionWindow cW = pnlDock.ActiveDocument as ConnectionWindow;
				if (cW.TabController.SelectedTab != null)
				{
					Crownwood.Magic.Controls.TabPage tab = cW.TabController.SelectedTab;
                    Connection.InterfaceControl ifc = (Connection.InterfaceControl)tab.Tag;
					ifc.Protocol.Focus();
					(ifc.FindForm() as ConnectionWindow).RefreshIC();
				}
			}
		}
		
		public void pnlDock_ActiveDocumentChanged(object sender, EventArgs e)
		{
			ActivateConnection();
            ConnectionWindow connectionWindow = pnlDock.ActiveDocument as ConnectionWindow;
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
				if (AreWeUsingSqlServerForSavingConnections)
				{
					titleBuilder.Append(separator);
					titleBuilder.Append(Language.strSQLServer.TrimEnd(':'));
				}
				else
				{
					if (!string.IsNullOrEmpty(ConnectionsFileName))
					{
						titleBuilder.Append(separator);
						if (mRemoteNG.Settings.Default.ShowCompleteConsPathInTitle)
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
									
			if (mRemoteNG.Settings.Default.AlwaysShowPanelTabs)
			{
				newDocumentStyle = DocumentStyle.DockingWindow; // Show the panel tabs
			}
			else
			{
				int nonConnectionPanelCount = 0;
				foreach (DockContent document in pnlDock.Documents)
				{
					if ((closingDocument == null || document != closingDocument) && !(document is ConnectionWindow))
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
			if (!(pnlDock.ActiveDocument is ConnectionWindow))
			{
				return ;
			}

            ConnectionWindow connectionWindow = (ConnectionWindow)pnlDock.ActiveDocument;
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
                Runtime.SystemMenu.AppendMenuItem(popMen, Tools.SystemMenu.Flags.MF_STRING, new IntPtr(SysMenSubItems[i]), Language.strScreen + " " + Convert.ToString(i + 1));
			}

            Runtime.SystemMenu.InsertMenuItem(Runtime.SystemMenu.SystemMenuHandle, 0, Tools.SystemMenu.Flags.MF_POPUP | Tools.SystemMenu.Flags.MF_BYPOSITION, popMen, Language.strSendTo);
            Runtime.SystemMenu.InsertMenuItem(Runtime.SystemMenu.SystemMenuHandle, 1, Tools.SystemMenu.Flags.MF_BYPOSITION | Tools.SystemMenu.Flags.MF_SEPARATOR, IntPtr.Zero, null);
		}
        #endregion

        #region Events
        public delegate void clipboardchangeEventHandler();
        public static event clipboardchangeEventHandler clipboardchange
        {
            add
            {
                clipboardchangeEvent = (clipboardchangeEventHandler)System.Delegate.Combine(clipboardchangeEvent, value);
            }
            remove
            {
                clipboardchangeEvent = (clipboardchangeEventHandler)System.Delegate.Remove(clipboardchangeEvent, value);
            }
        }
        #endregion
	}					
}