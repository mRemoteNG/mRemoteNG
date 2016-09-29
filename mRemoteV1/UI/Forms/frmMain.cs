using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using mRemoteNG.App;
using mRemoteNG.Config;
using mRemoteNG.Config.Putty;
using mRemoteNG.Config.Settings;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using mRemoteNG.Themes;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.TaskDialog;
using mRemoteNG.UI.Window;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.UI.Forms
{
    public partial class frmMain
    {
        private static readonly frmMain _defaultInstance = new frmMain();
        public static frmMain Default => _defaultInstance;
        private static clipboardchangeEventHandler clipboardchangeEvent;
        private bool _inSizeMove;
        private bool _inMouseActivate;
        private IntPtr fpChainedWindowHandle;
        private int[] SysMenSubItems = new int[51];
	    private bool _isClosing;
        private bool _usingSqlServer;
        private string _connectionsFileName;
        private bool _showFullPathInTitle;
        private ConnectionInfo _selectedConnection;
        private SystemMenu _systemMenu;
        private MiscTools.Fullscreen _fullscreen;
        private ConnectionTreeWindow ConnectionTreeWindow { get; set; }



        private frmMain()
		{
			_showFullPathInTitle = Settings.Default.ShowCompleteConsPathInTitle;
			InitializeComponent();
            _fullscreen = new MiscTools.Fullscreen(this);
            pnlDock.Theme = new VS2012LightTheme();
		}

        static frmMain()
        {
        }

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
        private void frmMain_Load(object sender, EventArgs e)
		{
            // Create gui config load and save objects
            var settingsLoader = new SettingsLoader(this);
			settingsLoader.LoadSettings();
			
			ApplyLanguage();
			PopulateQuickConnectProtocolMenu();
			ThemeManager.ThemeChanged += ApplyThemes;
			ApplyThemes();

			fpChainedWindowHandle = NativeMethods.SetClipboardViewer(Handle);

            Runtime.MessageCollector = new MessageCollector(Windows.ErrorsForm);
            Runtime.WindowList = new WindowList();

            if (Settings.Default.FirstStart && !Settings.Default.LoadConsFromCustomLocation && !File.Exists(Runtime.GetStartupConnectionFileName()))
			{
                Runtime.NewConnections(Runtime.GetStartupConnectionFileName());
			}

            Runtime.LoadConnections();
            if (!Runtime.IsConnectionsFileLoaded)
			{
				//Application.Exit();
				//return ;
			}

            Windows.TreePanel.Focus();

            PuttySessionsManager.Instance.StartWatcher();
			if (Settings.Default.StartupComponentsCheck)
			{
                Windows.Show(WindowType.ComponentsCheck);
			}

            ApplySpecialSettingsForPortableVersion();

            Startup.Instance.CreateConnectionsProvider();
			AddSysMenuItems();
			Microsoft.Win32.SystemEvents.DisplaySettingsChanged += DisplayChanged;
            Opacity = 1;

            ConnectionTreeWindow = Windows.TreeForm;
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

        private void ApplyThemes()
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
		    foreach (ToolStripItem item in itemCollection)
			{
				item.BackColor = ThemeManager.ActiveTheme.MenuBackgroundColor;
				item.ForeColor = ThemeManager.ActiveTheme.MenuTextColor;
				
				var menuItem = item as ToolStripMenuItem;
				if (menuItem != null)
				{
					ApplyMenuColors(menuItem.DropDownItems);
				}
			}
		}

        private void frmMain_Shown(object sender, EventArgs e)
		{
            #if PORTABLE
		    // ReSharper disable once RedundantJumpStatement
			return ;
            #endif
//			if (!mRemoteNG.Settings.Default.CheckForUpdatesAsked)
//			{
//				string[] commandButtons = new string[] {Language.strAskUpdatesCommandRecommended, Language.strAskUpdatesCommandCustom, Language.strAskUpdatesCommandAskLater};
//				cTaskDialog.ShowTaskDialogBox(this, GeneralAppInfo.ProdName, Language.strAskUpdatesMainInstruction, string.Format(Language.strAskUpdatesContent, GeneralAppInfo.ProdName, "", "", "", "", string.Join("|", commandButtons), eTaskDialogButtons.None, eSysIcons.Question, eSysIcons.Question);
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

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
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
					var result = CTaskDialog.MessageBox(this, Application.ProductName, Language.strConfirmExitMainInstruction, "", "", "", Language.strCheckboxDoNotShowThisMessageAgain, ETaskDialogButtons.YesNo, ESysIcons.Question, ESysIcons.Question);
					if (CTaskDialog.VerificationChecked)
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
									
			Debug.Print("[END] - " + Convert.ToString(DateTime.Now, CultureInfo.InvariantCulture));
		}
        #endregion
								
        #region Timer
		private void tmrAutoSave_Tick(object sender, EventArgs e)
		{
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Doing AutoSave", true);
			Runtime.SaveConnections();
		}
        #endregion
		
        #region Ext Apps Toolbar
		private void cMenToolbarShowText_Click(object sender, EventArgs e)
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
            var extA = (ExternalTool)((ToolStripButton)sender).Tag;

		    var selectedTreeNode = Windows.TreeForm.SelectedNode;
            if (selectedTreeNode.GetTreeNodeType() == TreeNodeType.Connection | selectedTreeNode.GetTreeNodeType() == TreeNodeType.PuttySession)
			{
                extA.Start(selectedTreeNode);
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
				if (show)
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
        private void mMenFile_DropDownOpening(Object sender, EventArgs e)
        {
            var selectedNodeType = ConnectionTreeWindow.SelectedNode?.GetTreeNodeType();
            if (selectedNodeType == TreeNodeType.Root)
			{
				mMenFileNewConnection.Enabled = true;
				mMenFileNewFolder.Enabled = true;
				mMenFileDelete.Enabled = false;
				mMenFileRename.Enabled = true;
				mMenFileDuplicate.Enabled = false;
                mMenReconnectAll.Enabled = true;
				mMenFileDelete.Text = Language.strMenuDelete;
				mMenFileRename.Text = Language.strMenuRenameFolder;
				mMenFileDuplicate.Text = Language.strMenuDuplicate;
                mMenReconnectAll.Text = Language.strMenuReconnectAll;
            }
            else if (selectedNodeType == TreeNodeType.Container)
			{
				mMenFileNewConnection.Enabled = true;
				mMenFileNewFolder.Enabled = true;
				mMenFileDelete.Enabled = true;
				mMenFileRename.Enabled = true;
				mMenFileDuplicate.Enabled = true;
                mMenReconnectAll.Enabled = true;
                mMenFileDelete.Text = Language.strMenuDeleteFolder;
				mMenFileRename.Text = Language.strMenuRenameFolder;
				mMenFileDuplicate.Text = Language.strMenuDuplicateFolder;
                mMenReconnectAll.Text = Language.strMenuReconnectAll;

            }
            else if (selectedNodeType == TreeNodeType.Connection)
			{
				mMenFileNewConnection.Enabled = true;
				mMenFileNewFolder.Enabled = true;
				mMenFileDelete.Enabled = true;
				mMenFileRename.Enabled = true;
				mMenFileDuplicate.Enabled = true;
                mMenReconnectAll.Enabled = true;
                mMenFileDelete.Text = Language.strMenuDeleteConnection;
				mMenFileRename.Text = Language.strMenuRenameConnection;
				mMenFileDuplicate.Text = Language.strMenuDuplicateConnection;
                mMenReconnectAll.Text = Language.strMenuReconnectAll;
            }
            else if (selectedNodeType == TreeNodeType.PuttyRoot || selectedNodeType == TreeNodeType.PuttySession)
			{
				mMenFileNewConnection.Enabled = false;
				mMenFileNewFolder.Enabled = false;
				mMenFileDelete.Enabled = false;
				mMenFileRename.Enabled = false;
				mMenFileDuplicate.Enabled = false;
                mMenReconnectAll.Enabled = true;
                mMenFileDelete.Text = Language.strMenuDelete;
				mMenFileRename.Text = Language.strMenuRename;
				mMenFileDuplicate.Text = Language.strMenuDuplicate;
                mMenReconnectAll.Text = Language.strMenuReconnectAll;
            }
			else
			{
				mMenFileNewConnection.Enabled = true;
				mMenFileNewFolder.Enabled = true;
				mMenFileDelete.Enabled = false;
				mMenFileRename.Enabled = false;
				mMenFileDuplicate.Enabled = false;
                mMenReconnectAll.Enabled = true;
                mMenFileDelete.Text = Language.strMenuDelete;
				mMenFileRename.Text = Language.strMenuRename;
				mMenFileDuplicate.Text = Language.strMenuDuplicate;
                mMenReconnectAll.Text = Language.strMenuReconnectAll;
            }
		}

        private void mMenFileNewConnection_Click(object sender, EventArgs e)
		{
            ConnectionTreeWindow.AddConnection();
            Runtime.SaveConnectionsBG();
		}

        private void mMenFileNewFolder_Click(object sender, EventArgs e)
		{
            ConnectionTreeWindow.AddFolder();
            Runtime.SaveConnectionsBG();
		}

        private void mMenFileNew_Click(object sender, EventArgs e)
		{
			var saveFileDialog = Tools.Controls.ConnectionsSaveAsDialog();
			if (saveFileDialog.ShowDialog() != DialogResult.OK)
			{
				return ;
			}

            Runtime.NewConnections(saveFileDialog.FileName);
		}

        private void mMenFileLoad_Click(object sender, EventArgs e)
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

        private void mMenFileSave_Click(object sender, EventArgs e)
		{
            Runtime.SaveConnections();
		}

        private void mMenFileSaveAs_Click(object sender, EventArgs e)
		{
            Runtime.SaveConnectionsAs();
		}

        private void mMenFileDelete_Click(object sender, EventArgs e)
		{
            ConnectionTreeWindow.DeleteSelectedNode();
            Runtime.SaveConnectionsBG();
		}

        private void mMenFileRename_Click(object sender, EventArgs e)
		{
            ConnectionTreeWindow.RenameSelectedNode();
            Runtime.SaveConnectionsBG();
		}

        private void mMenFileDuplicate_Click(object sender, EventArgs e)
		{
            ConnectionTreeWindow.DuplicateSelectedNode();
            Runtime.SaveConnectionsBG();
		}

        private void mMenReconnectAll_Click(object sender, EventArgs e)
        {
            if (Runtime.WindowList == null || Runtime.WindowList.Count == 0) return;
            foreach (BaseWindow window in Runtime.WindowList)
            {
                var connectionWindow = window as ConnectionWindow;
                if (connectionWindow == null)
                    return;

                var ICList = new List<InterfaceControl>();
                foreach (Crownwood.Magic.Controls.TabPage tab in connectionWindow.TabController.TabPages)
                {
                    var tag = tab.Tag as InterfaceControl;
                    if (tag != null)
                    {
                        ICList.Add(tag);
                    }
                }

                foreach (var i in ICList)
                {
                    i.Protocol.Close();
                    ConnectionInitiator.OpenConnection(i.Info, ConnectionInfo.Force.DoNotJump);
                }

                // throw it on the garbage collector
                // ReSharper disable once RedundantAssignment
                ICList = null;
            }
        }

        private void mMenFileImportFromFile_Click(object sender, EventArgs e)
        {
            var selectedNode = ConnectionTreeWindow.SelectedNode;
            var selectedNodeAsContainer = selectedNode as ContainerInfo ?? selectedNode.Parent;
            Import.ImportFromFile(selectedNodeAsContainer);
		}

        private void mMenFileImportFromActiveDirectory_Click(object sender, EventArgs e)
		{
            Windows.Show(WindowType.ActiveDirectoryImport);
		}

        private void mMenFileImportFromPortScan_Click(object sender, EventArgs e)
		{
            Windows.Show(WindowType.PortScan);
		}

        private void mMenFileExport_Click(object sender, EventArgs e)
		{
            Export.ExportToFile(Windows.TreeForm.SelectedNode, Runtime.ConnectionTreeModel);
		}

        private void mMenFileExit_Click(object sender, EventArgs e)
		{
            Shutdown.Quit();
		}
        #endregion

        #region View
        private void mMenView_DropDownOpening(object sender, EventArgs e)
		{
            mMenViewConnections.Checked = !Windows.TreeForm.IsHidden;
            mMenViewConfig.Checked = !Windows.ConfigForm.IsHidden;
            mMenViewErrorsAndInfos.Checked = !Windows.ErrorsForm.IsHidden;
            mMenViewScreenshotManager.Checked = !Windows.ScreenshotForm.IsHidden;

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
			((BaseWindow) ((ToolStripMenuItem)sender).Tag).Show(pnlDock);
            ((BaseWindow) ((ToolStripMenuItem)sender).Tag).Focus();
		}

        private void mMenViewConnections_Click(object sender, EventArgs e)
		{
			if (mMenViewConnections.Checked == false)
			{
                Windows.TreePanel.Show(pnlDock);
                mMenViewConnections.Checked = true;
			}
			else
			{
                Windows.TreePanel.Hide();
                mMenViewConnections.Checked = false;
			}
		}

        private void mMenViewConfig_Click(object sender, EventArgs e)
		{
			if (mMenViewConfig.Checked == false)
			{
                Windows.ConfigPanel.Show(pnlDock);
                mMenViewConfig.Checked = true;
			}
			else
			{
                Windows.ConfigPanel.Hide();
                mMenViewConfig.Checked = false;
			}
		}

        private void mMenViewErrorsAndInfos_Click(object sender, EventArgs e)
		{
			if (mMenViewErrorsAndInfos.Checked == false)
			{
                Windows.ErrorsPanel.Show(pnlDock);
                mMenViewErrorsAndInfos.Checked = true;
			}
			else
			{
                Windows.ErrorsPanel.Hide();
                mMenViewErrorsAndInfos.Checked = false;
			}
		}

        private void mMenViewScreenshotManager_Click(object sender, EventArgs e)
		{
			if (mMenViewScreenshotManager.Checked == false)
			{
                Windows.ScreenshotPanel.Show(pnlDock);
                mMenViewScreenshotManager.Checked = true;
			}
			else
			{
                Windows.ScreenshotPanel.Hide();
                mMenViewScreenshotManager.Checked = false;
			}
		}

        private void mMenViewJumpToConnectionsConfig_Click(object sender, EventArgs e)
		{
            if (pnlDock.ActiveContent == Windows.TreePanel)
			{
                Windows.ConfigForm.Activate();
			}
			else
			{
                Windows.TreeForm.Activate();
			}
		}

        private void mMenViewJumpToErrorsInfos_Click(object sender, EventArgs e)
		{
            Windows.ErrorsForm.Activate();
		}

        private void mMenViewResetLayout_Click(object sender, EventArgs e)
		{
            var msgBoxResult = MessageBox.Show(Language.strConfirmResetLayout, string.Empty, MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (msgBoxResult == DialogResult.Yes)
			{
				Startup.Instance.SetDefaultLayout();
			}
		}

        private void mMenViewAddConnectionPanel_Click(object sender, EventArgs e)
		{
            Runtime.AddPanel();
		}

        private void mMenViewExtAppsToolbar_Click(object sender, EventArgs e)
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

        private void mMenViewQuickConnectToolbar_Click(object sender, EventArgs e)
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

        private void mMenViewFullscreen_Click(object sender, EventArgs e)
		{
			Fullscreen.Value = !Fullscreen.Value;
			mMenViewFullscreen.Checked = Fullscreen.Value;
		}
        #endregion

        #region Tools
        private void mMenToolsUpdate_Click(object sender, EventArgs e)
		{
			Windows.Show(WindowType.Update);
		}

        private void mMenToolsSSHTransfer_Click(object sender, EventArgs e)
		{
			Windows.Show(WindowType.SSHTransfer);
		}

        private void mMenToolsUVNCSC_Click(object sender, EventArgs e)
		{
			Windows.Show(WindowType.UltraVNCSC);
		}

        private void mMenToolsExternalApps_Click(object sender, EventArgs e)
		{
			Windows.Show(WindowType.ExternalApps);
		}

        private void mMenToolsPortScan_Click(object sender, EventArgs e)
		{
			Windows.Show(WindowType.PortScan);
		}

        private void mMenToolsComponentsCheck_Click(object sender, EventArgs e)
		{
			Windows.Show(WindowType.ComponentsCheck);
		}

        private void mMenToolsOptions_Click(object sender, EventArgs e)
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

        private void lblQuickConnect_Click(object sender, EventArgs e)
		{
			cmbQuickConnect.Focus();
		}

        private void cmbQuickConnect_ConnectRequested(object sender, QuickConnectComboBox.ConnectRequestedEventArgs e)
		{
			btnQuickConnect_ButtonClick(sender, e);
		}

        private void btnQuickConnect_ButtonClick(object sender, EventArgs e)
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
                ConnectionInitiator.OpenConnection(connectionInfo, ConnectionInfo.Force.DoNotJump);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage("btnQuickConnect_ButtonClick() failed.", ex, MessageClass.ErrorMsg, true);
			}
		}

        private void cmbQuickConnect_ProtocolChanged(object sender, QuickConnectComboBox.ProtocolChangedEventArgs e)
		{
			SetQuickConnectProtocol(Converter.ProtocolToString(e.Protocol));
		}

        private void btnQuickConnect_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
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
        private void mMenInfoHelp_Click(object sender, EventArgs e)
		{
			Windows.Show(WindowType.Help);
		}

        private void mMenInfoForum_Click(object sender, EventArgs e)
		{
			Runtime.GoToForum();
		}

        private void mMenInfoBugReport_Click(object sender, EventArgs e)
		{
			Runtime.GoToBugs();
		}

        private void mMenInfoWebsite_Click(object sender, EventArgs e)
		{
			Runtime.GoToWebsite();
		}

        private void mMenInfoDonate_Click(object sender, EventArgs e)
		{
			Runtime.GoToDonate();
		}

        private void mMenInfoAnnouncements_Click(object sender, EventArgs e)
		{
			Windows.Show(WindowType.Announcement);
		}

        private void mMenInfoAbout_Click(object sender, EventArgs e)
		{
			Windows.Show(WindowType.About);
		}
        #endregion
        #endregion

        #region Connections DropDown
        private void btnConnections_DropDownOpening(object sender, EventArgs e)
		{
            btnConnections.DropDownItems.Clear();
            var menuItemsConverter = new ConnectionsTreeToMenuItemsConverter
            {
                MouseUpEventHandler = ConnectionsMenuItem_MouseUp
            };

            ToolStripItem[] rootMenuItems = menuItemsConverter.CreateToolStripDropDownItems(Runtime.ConnectionTreeModel).ToArray();
            btnConnections.DropDownItems.AddRange(rootMenuItems);
		}
										
		private static void ConnectionsMenuItem_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
			    var tag = ((ToolStripMenuItem)sender).Tag as ConnectionInfo;
			    if (tag != null)
				{
                    ConnectionInitiator.OpenConnection(tag);
				}
			}
		}
        #endregion

        #region Window Overrides and DockPanel Stuff
        private void frmMain_ResizeBegin(object sender, EventArgs e)
		{
			_inSizeMove = true;
		}

        private void frmMain_Resize(object sender, EventArgs e)
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

        private void frmMain_ResizeEnd(object sender, EventArgs e)
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
				if (m.Msg == NativeMethods.WM_MOUSEACTIVATE)
				{
					_inMouseActivate = true;
				}
                else if (m.Msg == NativeMethods.WM_ACTIVATEAPP) // occurs when mRemoteNG becomes the active app
				{
					_inMouseActivate = false;
				}
                else if (m.Msg == NativeMethods.WM_ACTIVATE)
				{
                    // Ingore this message if it wasn't triggered by a click
                    if (NativeMethods.LOWORD(m.WParam) == NativeMethods.WA_CLICKACTIVE)
                    {
                        var control = FromChildHandle(NativeMethods.WindowFromPoint(MousePosition));
                        if (control != null)
                        {
                            // Let TreeViews and ComboBoxes get focus but don't simulate a mouse event
                            if (control is TreeView || control is ComboBox)
                            { }
                            else
                            {
                                if (control.CanSelect || control is MenuStrip || control is ToolStrip || control is Crownwood.Magic.Controls.TabControl || control is Crownwood.Magic.Controls.InertButton)
                                {
                                    // Simulate a mouse event since one wasn't generated by Windows
                                    var clientMousePosition = control.PointToClient(MousePosition);
                                    var temp_wLow = clientMousePosition.X;
                                    var temp_wHigh = clientMousePosition.Y;
                                    NativeMethods.SendMessage(control.Handle, NativeMethods.WM_LBUTTONDOWN, NativeMethods.MK_LBUTTON, NativeMethods.MAKELPARAM(ref temp_wLow, ref temp_wHigh));
                                    clientMousePosition.X = temp_wLow;
                                    clientMousePosition.Y = temp_wHigh;
                                    control.Focus();
                                }
                            }
                        }

                        // This handles activations from clicks that did not start a size/move operation
                        ActivateConnection();
                    }
				}
                else if (m.Msg == NativeMethods.WM_WINDOWPOSCHANGED)
				{
					// Ignore this message if the window wasn't activated
                    var windowPos = (NativeMethods.WINDOWPOS)Marshal.PtrToStructure(m.LParam, typeof(NativeMethods.WINDOWPOS));
                    if ((windowPos.flags & NativeMethods.SWP_NOACTIVATE) != 0)
                    {
                    }
                    else
                    {
                        // This handles all other activations
                        if (!_inMouseActivate && !_inSizeMove)
                        {
                            ActivateConnection();
                        }
                    }
				}
                else if (m.Msg == NativeMethods.WM_SYSCOMMAND)
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
                else if (m.Msg == NativeMethods.WM_DRAWCLIPBOARD)
				{
					NativeMethods.SendMessage(fpChainedWindowHandle, m.Msg, m.LParam.ToInt32(), m.WParam.ToInt32());
				    clipboardchangeEvent?.Invoke();
				}
                else if (m.Msg == NativeMethods.WM_CHANGECBCHAIN)
				{
					//Send to the next window
                    NativeMethods.SendMessage(fpChainedWindowHandle, m.Msg, m.LParam.ToInt32(), m.WParam.ToInt32());
					fpChainedWindowHandle = m.LParam;
				}
			}
			catch (Exception ex)
			{
                Runtime.MessageCollector.AddExceptionMessage("frmMain WndProc failed", ex, MessageClass.ErrorMsg, true);
            }
									
			base.WndProc(ref m);
		}
        
		private void ActivateConnection()
		{
		    var w = pnlDock.ActiveDocument as ConnectionWindow;
		    if (w?.TabController.SelectedTab == null) return;
		    var tab = w.TabController.SelectedTab;
		    var ifc = (InterfaceControl)tab.Tag;
		    ifc.Protocol.Focus();
		    ((ConnectionWindow) ifc.FindForm())?.RefreshInterfaceController();
		}

        private void pnlDock_ActiveDocumentChanged(object sender, EventArgs e)
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
									
			if (!(string.IsNullOrEmpty(SelectedConnection?.Name)))
			{
				titleBuilder.Append(separator);
				titleBuilder.Append(SelectedConnection.Name);
			}

            Text = titleBuilder.ToString();
		}
		
		public void ShowHidePanelTabs(DockContent closingDocument = null)
		{
			DocumentStyle newDocumentStyle;
									
			if (Settings.Default.AlwaysShowPanelTabs)
			{
				newDocumentStyle = DocumentStyle.DockingWindow; // Show the panel tabs
			}
			else
			{
				var nonConnectionPanelCount = 0;
				foreach (var dockContent in pnlDock.Documents)
				{
				    var document = (DockContent) dockContent;
				    if ((closingDocument == null || document != closingDocument) && !(document is ConnectionWindow))
					{
						nonConnectionPanelCount++;
					}
				}

			    newDocumentStyle = nonConnectionPanelCount == 0 ? DocumentStyle.DockingSdi : DocumentStyle.DockingWindow;
			}
									
			if (pnlDock.DocumentStyle != newDocumentStyle)
			{
				pnlDock.DocumentStyle = newDocumentStyle;
				pnlDock.Size = new Size(1, 1);
			}
		}

#if false
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
#endif
        #endregion
		
        #region Screen Stuff
		private void DisplayChanged(object sender, EventArgs e)
		{
			ResetSysMenuItems();
			AddSysMenuItems();
		}
		
		private void ResetSysMenuItems()
		{
			_systemMenu.Reset();
		}
								
		private void AddSysMenuItems()
		{
            _systemMenu = new SystemMenu(Handle);
            var popMen = _systemMenu.CreatePopupMenuItem();
									
			for (var i = 0; i <= Screen.AllScreens.Length - 1; i++)
			{
				SysMenSubItems[i] = 200 + i;
                _systemMenu.AppendMenuItem(popMen, SystemMenu.Flags.MF_STRING, new IntPtr(SysMenSubItems[i]), Language.strScreen + " " + Convert.ToString(i + 1));
			}

            _systemMenu.InsertMenuItem(_systemMenu.SystemMenuHandle, 0, SystemMenu.Flags.MF_POPUP | SystemMenu.Flags.MF_BYPOSITION, popMen, Language.strSendTo);
            _systemMenu.InsertMenuItem(_systemMenu.SystemMenuHandle, 1, SystemMenu.Flags.MF_BYPOSITION | SystemMenu.Flags.MF_SEPARATOR, IntPtr.Zero, null);
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
