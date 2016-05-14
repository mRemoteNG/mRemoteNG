using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Config;
using mRemoteNG.Config.Settings;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Controls;
using mRemoteNG.Messages;
using mRemoteNG.Themes;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.UI.Window;
using PSTaskDialog;
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
			_showFullPathInTitle = Settings.Default.ShowCompleteConsPathInTitle;
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
        public FormWindowState PreviousWindowState { get; set; }

	    public bool IsClosing => _isClosing;

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
            var settingsLoader = new SettingsLoader(this);
			settingsLoader.LoadSettings();
			
			Debug.Print("---------------------------" + Environment.NewLine + "[START] - " + Convert.ToString(DateTime.Now));
            Startup.ParseCommandLineArgs();
			ApplyLanguage();
			PopulateQuickConnectProtocolMenu();
			ThemeManager.ThemeChanged += ApplyThemes;
			ApplyThemes();
			fpChainedWindowHandle = Native.SetClipboardViewer(Handle);
            Runtime.MessageCollector = new MessageCollector(Windows.errorsForm);
            Runtime.WindowList = new WindowList();
            //Startup.CreatePanels();
            //Startup.SetDefaultLayout();
            IeBrowserEmulation.Register();
            Startup.GetConnectionIcons();
            Windows.treePanel.Focus();
            ConnectionTree.TreeView = Windows.treeForm.tvConnections;

            if (Settings.Default.FirstStart && !Settings.Default.LoadConsFromCustomLocation && !File.Exists(Runtime.GetStartupConnectionFileName()))
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
			if (Settings.Default.StartupComponentsCheck)
			{
                Windows.Show(WindowType.ComponentsCheck);
			}

            ApplySpecialSettingsForPortableVersion();

            Startup.CreateConnectionsProvider();
			AddSysMenuItems();
			Microsoft.Win32.SystemEvents.DisplaySettingsChanged += DisplayChanged;
            Opacity = 1;
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
			var menuItem = default(ToolStripMenuItem);
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
			    var openConnections = 0;
                foreach (BaseWindow window in Runtime.WindowList)
                {
                    var connectionWindow = window as ConnectionWindow;
                    if (connectionWindow != null)
						openConnections = openConnections + connectionWindow.TabController.TabPages.Count;
                }

			    if (openConnections > 0 && (Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.All | (Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.Multiple & openConnections > 1) || Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.Exit))
				{
					var result = cTaskDialog.MessageBox(this, Application.ProductName, Language.strConfirmExitMainInstruction, "", "", "", Language.strCheckboxDoNotShowThisMessageAgain, eTaskDialogButtons.YesNo, eSysIcons.Question, eSysIcons.Question);
					if (cTaskDialog.VerificationChecked)
					{
                        Settings.Default.ConfirmCloseConnection--;
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
		public void tmrAutoSave_Tick(object sender, EventArgs e)
		{
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Doing AutoSave", true);
			Runtime.SaveConnections();
		}
        #endregion
		
        #region Ext Apps Toolbar
		public void cMenToolbarShowText_Click(object sender, EventArgs e)
		{
			SwitchToolBarText(!cMenToolbarShowText.Checked);
		}
								
		public void AddExternalToolsToToolBar()
		{
			try
			{
				for (var index = tsExternalTools.Items.Count - 1; index >= 0; index--)
				{
					tsExternalTools.Items[index].Dispose();
				}
				tsExternalTools.Items.Clear();

			    foreach (ExternalTool tool in Runtime.ExternalTools)
				{
					var button = (ToolStripButton)tsExternalTools.Items.Add(tool.DisplayName, tool.Image, tsExtAppEntry_Click);
											
					if (cMenToolbarShowText.Checked)
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
								
		private void tsExtAppEntry_Click(object sender, EventArgs e)
		{
            var extA = (ExternalTool)((Control)sender).Tag;

            if (ConnectionTreeNode.GetNodeType(ConnectionTree.SelectedNode) == TreeNodeType.Connection | ConnectionTreeNode.GetNodeType(ConnectionTree.SelectedNode) == TreeNodeType.PuttySession)
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
					tItem.DisplayStyle = tItem.Image != null ? ToolStripItemDisplayStyle.Image : ToolStripItemDisplayStyle.ImageAndText;
				}
			}
									
			cMenToolbarShowText.Checked = show;
		}
        #endregion
								
        #region Menu
        #region File
		public void mMenFile_DropDownOpening(Object sender, EventArgs e)
		{
            if (ConnectionTreeNode.GetNodeType(ConnectionTree.SelectedNode) == TreeNodeType.Root)
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
            else if (ConnectionTreeNode.GetNodeType(ConnectionTree.SelectedNode) == TreeNodeType.Container)
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
            else if (ConnectionTreeNode.GetNodeType(ConnectionTree.SelectedNode) == TreeNodeType.Connection)
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
            else if ((ConnectionTreeNode.GetNodeType(ConnectionTree.SelectedNode) == TreeNodeType.PuttyRoot) || (ConnectionTreeNode.GetNodeType(ConnectionTree.SelectedNode) == TreeNodeType.PuttySession))
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
								
		public static void mMenFileNewConnection_Click(object sender, EventArgs e)
		{
			Windows.treeForm.AddConnection();
            Runtime.SaveConnectionsBG();
		}
								
		public static void mMenFileNewFolder_Click(object sender, EventArgs e)
		{
            Windows.treeForm.AddFolder();
            Runtime.SaveConnectionsBG();
		}
								
		public static void mMenFileNew_Click(object sender, EventArgs e)
		{
			var saveFileDialog = Tools.Controls.ConnectionsSaveAsDialog();
			if (!(saveFileDialog.ShowDialog() == DialogResult.OK))
			{
				return ;
			}

            Runtime.NewConnections(saveFileDialog.FileName);
		}
								
		public static void mMenFileLoad_Click(object sender, EventArgs e)
		{
            if (Runtime.IsConnectionsFileLoaded)
			{
                var msgBoxResult = MessageBox.Show(Language.strSaveConnectionsFileBeforeOpeningAnother, Language.strSave, MessageBoxButtons.YesNoCancel);
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
								
		public static void mMenFileSave_Click(object sender, EventArgs e)
		{
            Runtime.SaveConnections();
		}
								
		public static void mMenFileSaveAs_Click(object sender, EventArgs e)
		{
            Runtime.SaveConnectionsAs();
		}
								
		public static void mMenFileDelete_Click(object sender, EventArgs e)
		{
            ConnectionTree.DeleteSelectedNode();
            Runtime.SaveConnectionsBG();
		}
								
		public static void mMenFileRename_Click(object sender, EventArgs e)
		{
			ConnectionTree.StartRenameSelectedNode();
            Runtime.SaveConnectionsBG();
		}
								
		public static void mMenFileDuplicate_Click(object sender, EventArgs e)
		{
            ConnectionTreeNode.CloneNode(ConnectionTree.SelectedNode);
            Runtime.SaveConnectionsBG();
		}
								
		public static void mMenFileImportFromFile_Click(object sender, EventArgs e)
		{
            Import.ImportFromFile(Windows.treeForm.tvConnections.Nodes[0], Windows.treeForm.tvConnections.SelectedNode);
		}
								
		public static void mMenFileImportFromActiveDirectory_Click(object sender, EventArgs e)
		{
            Windows.Show(WindowType.ActiveDirectoryImport);
		}
								
		public static void mMenFileImportFromPortScan_Click(object sender, EventArgs e)
		{
            Windows.Show(WindowType.PortScan, true);
		}
								
		public static void mMenFileExport_Click(object sender, EventArgs e)
		{
            Export.ExportToFile(Windows.treeForm.tvConnections.Nodes[0], Windows.treeForm.tvConnections.SelectedNode);
		}
								
		public static void mMenFileExit_Click(object sender, EventArgs e)
		{
            Shutdown.Quit();
		}
        #endregion
								
        #region View
		public void mMenView_DropDownOpening(object sender, EventArgs e)
		{
            mMenViewConnections.Checked = !Windows.treeForm.IsHidden;
            mMenViewConfig.Checked = !Windows.configForm.IsHidden;
            mMenViewErrorsAndInfos.Checked = !Windows.errorsForm.IsHidden;
            mMenViewScreenshotManager.Checked = !Windows.screenshotForm.IsHidden;

            mMenViewExtAppsToolbar.Checked = tsExternalTools.Visible;
            mMenViewQuickConnectToolbar.Checked = tsQuickConnect.Visible;

            mMenViewConnectionPanels.DropDownItems.Clear();

            for (var i = 0; i <= Runtime.WindowList.Count - 1; i++)
			{
                var tItem = new ToolStripMenuItem(Convert.ToString(Runtime.WindowList[i].Text), Runtime.WindowList[i].Icon.ToBitmap(), ConnectionPanelMenuItem_Click);
                tItem.Tag = Runtime.WindowList[i];
                mMenViewConnectionPanels.DropDownItems.Add(tItem);
			}
									
			mMenViewConnectionPanels.Enabled = mMenViewConnectionPanels.DropDownItems.Count > 0;
		}
								
		private void ConnectionPanelMenuItem_Click(object sender, EventArgs e)
		{
			(((Control)sender).Tag as BaseWindow).Show(pnlDock);
            (((Control)sender).Tag as BaseWindow).Focus();
		}
								
		public void mMenViewConnections_Click(object sender, EventArgs e)
		{
			if (mMenViewConnections.Checked == false)
			{
                Windows.treePanel.Show(pnlDock);
                mMenViewConnections.Checked = true;
			}
			else
			{
                Windows.treePanel.Hide();
                mMenViewConnections.Checked = false;
			}
		}
								
		public void mMenViewConfig_Click(object sender, EventArgs e)
		{
			if (mMenViewConfig.Checked == false)
			{
                Windows.configPanel.Show(pnlDock);
                mMenViewConfig.Checked = true;
			}
			else
			{
                Windows.configPanel.Hide();
                mMenViewConfig.Checked = false;
			}
		}
								
		public void mMenViewErrorsAndInfos_Click(object sender, EventArgs e)
		{
			if (mMenViewErrorsAndInfos.Checked == false)
			{
                Windows.errorsPanel.Show(pnlDock);
                mMenViewErrorsAndInfos.Checked = true;
			}
			else
			{
                Windows.errorsPanel.Hide();
                mMenViewErrorsAndInfos.Checked = false;
			}
		}
								
		public void mMenViewScreenshotManager_Click(object sender, EventArgs e)
		{
			if (mMenViewScreenshotManager.Checked == false)
			{
                Windows.screenshotPanel.Show(pnlDock);
                mMenViewScreenshotManager.Checked = true;
			}
			else
			{
                Windows.screenshotPanel.Hide();
                mMenViewScreenshotManager.Checked = false;
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
								
		public void mMenViewResetLayout_Click(object sender, EventArgs e)
		{
            var msgBoxResult = MessageBox.Show(Language.strConfirmResetLayout, "", MessageBoxButtons.YesNo);
            if (msgBoxResult == DialogResult.Yes)
			{
				Startup.SetDefaultLayout();
			}
		}
								
		public void mMenViewAddConnectionPanel_Click(object sender, EventArgs e)
		{
            Runtime.AddPanel();
		}
								
		public void mMenViewExtAppsToolbar_Click(object sender, EventArgs e)
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
								
		public void mMenViewQuickConnectToolbar_Click(object sender, EventArgs e)
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
		
		public void mMenViewFullscreen_Click(object sender, EventArgs e)
		{
			Fullscreen.Value = !Fullscreen.Value;
			mMenViewFullscreen.Checked = Fullscreen.Value;
		}
        #endregion
								
        #region Tools
		public void mMenToolsUpdate_Click(object sender, EventArgs e)
		{
			Windows.Show(WindowType.Update);
		}
								
		public void mMenToolsSSHTransfer_Click(object sender, EventArgs e)
		{
			Windows.Show(WindowType.SSHTransfer);
		}
								
		public void mMenToolsUVNCSC_Click(object sender, EventArgs e)
		{
			Windows.Show(WindowType.UltraVNCSC);
		}
								
		public void mMenToolsExternalApps_Click(object sender, EventArgs e)
		{
			Windows.Show(WindowType.ExternalApps);
		}
								
		public void mMenToolsPortScan_Click(object sender, EventArgs e)
		{
			Windows.Show(WindowType.PortScan, false);
		}
								
		public void mMenToolsComponentsCheck_Click(object sender, EventArgs e)
		{
			Windows.Show(WindowType.ComponentsCheck);
		}
								
		public void mMenToolsOptions_Click(object sender, EventArgs e)
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
				foreach (var fieldInfo in typeof(ProtocolType).GetFields())
				{
					if (!(fieldInfo.Name == "value__" || fieldInfo.Name == "IntApp"))
					{
						var menuItem = new ToolStripMenuItem(fieldInfo.Name);
						if (fieldInfo.Name == Settings.Default.QuickConnectProtocol)
						{
							menuItem.Checked = true;
							btnQuickConnect.Text = Settings.Default.QuickConnectProtocol;
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
								
		public void lblQuickConnect_Click(object sender, EventArgs e)
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
				var connectionInfo = Runtime.CreateQuickConnect(cmbQuickConnect.Text.Trim(), Converter.StringToProtocol(Settings.Default.QuickConnectProtocol));
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
			SetQuickConnectProtocol(Converter.ProtocolToString(e.Protocol));
		}
	    
		public void btnQuickConnect_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			SetQuickConnectProtocol(e.ClickedItem.Text);
		}
		
		private void SetQuickConnectProtocol(string protocol)
		{
            Settings.Default.QuickConnectProtocol = protocol;
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
		public void mMenInfoHelp_Click(object sender, EventArgs e)
		{
			Windows.Show(WindowType.Help);
		}
								
		public void mMenInfoForum_Click(object sender, EventArgs e)
		{
			Runtime.GoToForum();
		}
								
		public void mMenInfoBugReport_Click(object sender, EventArgs e)
		{
			Runtime.GoToBugs();
		}
								
		public void mMenInfoWebsite_Click(object sender, EventArgs e)
		{
			Runtime.GoToWebsite();
		}
								
		public void mMenInfoDonate_Click(object sender, EventArgs e)
		{
			Runtime.GoToDonate();
		}
								
		public void mMenInfoAnnouncements_Click(object sender, EventArgs e)
		{
			Windows.Show(WindowType.Announcement);
		}
								
		public void mMenInfoAbout_Click(object sender, EventArgs e)
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
					var menuItem = new ToolStripMenuItem();
					menuItem.Text = treeNode.Text;
					menuItem.Tag = treeNode;
											
					if (ConnectionTreeNode.GetNodeType(treeNode) == TreeNodeType.Container)
					{
						menuItem.Image = Resources.Folder;
						menuItem.Tag = treeNode.Tag;
												
						toolStripMenuItem.DropDownItems.Add(menuItem);
						AddNodeToMenu(treeNode.Nodes, menuItem);
					}
					else if (ConnectionTreeNode.GetNodeType(treeNode) == TreeNodeType.Connection | ConnectionTreeNode.GetNodeType(treeNode) == TreeNodeType.PuttySession)
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
								
		private static void ConnectionsMenuItem_MouseUp(object sender, MouseEventArgs e)
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
				if (Settings.Default.MinimizeToTray)
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
                    if (Native.LOWORD(m.WParam) != Native.WA_CLICKACTIVE) { }

                    var control = FromChildHandle(Native.WindowFromPoint(MousePosition));
					if (control != null)
					{
						// Let TreeViews and ComboBoxes get focus but don't simulate a mouse event
						if (control is TreeView|| control is ComboBox) { }
												
						if (control.CanSelect || control is MenuStrip|| control is ToolStrip|| control is Crownwood.Magic.Controls.TabControl|| control is Crownwood.Magic.Controls.InertButton)
						{
							// Simulate a mouse event since one wasn't generated by Windows
							var clientMousePosition = control.PointToClient(MousePosition);
							var temp_wLow = clientMousePosition.X;
							var temp_wHigh = clientMousePosition.Y;
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
                    var windowPos = (Native.WINDOWPOS)Marshal.PtrToStructure(m.LParam, typeof(Native.WINDOWPOS));
                    if ((windowPos.flags & Native.SWP_NOACTIVATE) != 0)
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
					for (var i = 0; i <= SysMenSubItems.Length - 1; i++)
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
				    clipboardchangeEvent?.Invoke();
				}
                else if (m.Msg == Native.WM_CHANGECBCHAIN)
				{
					//Send to the next window
                    Native.SendMessage(fpChainedWindowHandle, m.Msg, m.LParam.ToInt32(), m.WParam.ToInt32());
					fpChainedWindowHandle = m.LParam;
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
                var cW = pnlDock.ActiveDocument as ConnectionWindow;
				if (cW.TabController.SelectedTab != null)
				{
					var tab = cW.TabController.SelectedTab;
                    var ifc = (InterfaceControl)tab.Tag;
					ifc.Protocol.Focus();
					(ifc.FindForm() as ConnectionWindow).RefreshIC();
				}
			}
		}
		
		public void pnlDock_ActiveDocumentChanged(object sender, EventArgs e)
		{
			ActivateConnection();
            var connectionWindow = pnlDock.ActiveDocument as ConnectionWindow;
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
									
			var titleBuilder = new StringBuilder(Application.ProductName);
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
						if (Settings.Default.ShowCompleteConsPathInTitle)
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

            Text = titleBuilder.ToString();
		}
		
		public void ShowHidePanelTabs(DockContent closingDocument = null)
		{
			var newDocumentStyle = pnlDock.DocumentStyle;
									
			if (Settings.Default.AlwaysShowPanelTabs)
			{
				newDocumentStyle = DocumentStyle.DockingWindow; // Show the panel tabs
			}
			else
			{
				var nonConnectionPanelCount = 0;
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

            var connectionWindow = (ConnectionWindow)pnlDock.ActiveDocument;
			var tabController = connectionWindow.TabController;
									
			var newIndex = tabController.SelectedIndex + relativeIndex;
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
		private void DisplayChanged(object sender, EventArgs e)
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
            Runtime.SystemMenu = new SystemMenu(Handle);
            var popMen = Runtime.SystemMenu.CreatePopupMenuItem();
									
			for (var i = 0; i <= Screen.AllScreens.Length - 1; i++)
			{
				SysMenSubItems[i] = 200 + i;
                Runtime.SystemMenu.AppendMenuItem(popMen, SystemMenu.Flags.MF_STRING, new IntPtr(SysMenSubItems[i]), Language.strScreen + " " + Convert.ToString(i + 1));
			}

            Runtime.SystemMenu.InsertMenuItem(Runtime.SystemMenu.SystemMenuHandle, 0, SystemMenu.Flags.MF_POPUP | SystemMenu.Flags.MF_BYPOSITION, popMen, Language.strSendTo);
            Runtime.SystemMenu.InsertMenuItem(Runtime.SystemMenu.SystemMenuHandle, 1, SystemMenu.Flags.MF_BYPOSITION | SystemMenu.Flags.MF_SEPARATOR, IntPtr.Zero, null);
		}
        #endregion

        #region Events
        public delegate void clipboardchangeEventHandler();
        public static event clipboardchangeEventHandler clipboardchange
        {
            add
            {
                clipboardchangeEvent = (clipboardchangeEventHandler)Delegate.Combine(clipboardchangeEvent, value);
            }
            remove
            {
                clipboardchangeEvent = (clipboardchangeEventHandler)Delegate.Remove(clipboardchangeEvent, value);
            }
        }
        #endregion
	}					
}