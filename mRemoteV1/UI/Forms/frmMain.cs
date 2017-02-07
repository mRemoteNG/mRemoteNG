using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.App.Initialization;
using mRemoteNG.Config;
using mRemoteNG.Config.Putty;
using mRemoteNG.Config.Settings;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Credential;
using mRemoteNG.Messages;
using mRemoteNG.Themes;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Menu;
using mRemoteNG.UI.TaskDialog;
using mRemoteNG.UI.Window;
using Microsoft.Win32;
using WeifenLuo.WinFormsUI.Docking;
// ReSharper disable MemberCanBePrivate.Global

namespace mRemoteNG.UI.Forms
{
    public partial class FrmMain
    {
        public static FrmMain Default { get; } = new FrmMain();

        private static ClipboardchangeEventHandler _clipboardChangedEvent;
        private bool _inSizeMove;
        private bool _inMouseActivate;
        private IntPtr _fpChainedWindowHandle;
        private bool _usingSqlServer;
        private string _connectionsFileName;
        private bool _showFullPathInTitle;
        private readonly ScreenSelectionSystemMenu _screenSystemMenu;
        private ConnectionInfo _selectedConnection;
        internal FullscreenHandler _fullscreen { get; set; }
        private ConnectionTreeWindow ConnectionTreeWindow { get; set; }
        private readonly IConnectionInitiator _connectionInitiator = new ConnectionInitiator();
        private readonly string _credentialFilePath = Path.Combine(CredentialsFileInfo.CredentialsPath, CredentialsFileInfo.CredentialsFile);
        private readonly CredentialManager _credentialManager = Runtime.CredentialManager;



        private FrmMain()
		{
			_showFullPathInTitle = Settings.Default.ShowCompleteConsPathInTitle;
			InitializeComponent();
            _fullscreen = new FullscreenHandler(this);
            pnlDock.Theme = new VS2012LightTheme();
            _screenSystemMenu = new ScreenSelectionSystemMenu(this);
		}

        static FrmMain()
        {
        }

        #region Properties
        public FormWindowState PreviousWindowState { get; set; }

	    public bool IsClosing { get; private set; }

        public bool AreWeUsingSqlServerForSavingConnections
		{
			get { return _usingSqlServer; }
			set
			{
				if (_usingSqlServer == value)
				{
					return;
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
					return;
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
					return;
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
					return;
				}
				_selectedConnection = value;
				UpdateWindowTitle();
			}
		}
        #endregion

        #region Startup & Shutdown
        private void frmMain_Load(object sender, EventArgs e)
        {
            var messageCollector = Runtime.MessageCollector;
            MessageCollectorSetup.SetupMessageCollector(messageCollector, Runtime.MessageWriters);
            MessageCollectorSetup.BuildMessageWritersFromSettings(Runtime.MessageWriters);

            Startup.Instance.InitializeProgram(messageCollector);

            var settingsLoader = new SettingsLoader(this, new MessageCollector());
            settingsLoader.LoadSettings();

            var uiLoader = new LayoutSettingsLoader(this, messageCollector);
            uiLoader.LoadPanelsFromXml();

            ApplyLanguage();
			PopulateQuickConnectProtocolMenu();
			ThemeManager.ThemeChanged += ApplyThemes;
			ApplyThemes();

			_fpChainedWindowHandle = NativeMethods.SetClipboardViewer(Handle);

            Runtime.WindowList = new WindowList();

            new CredsAndConsSetup(_credentialManager, _credentialFilePath).LoadCredsAndCons();

            Windows.TreeForm.Focus();

            PuttySessionsManager.Instance.StartWatcher();
			if (Settings.Default.StartupComponentsCheck)
                Windows.Show(WindowType.ComponentsCheck);

            Startup.Instance.CreateConnectionsProvider(messageCollector);

            _screenSystemMenu.BuildScreenList();
			SystemEvents.DisplaySettingsChanged += _screenSystemMenu.OnDisplayChanged;
            Opacity = 1;

            ConnectionTreeWindow = Windows.TreeForm;

            msMain.Items.Add(new MainFileMenu(ConnectionTreeWindow, _connectionInitiator));
            msMain.Items.Add(new ViewMenu(tsExternalTools, tsQuickConnect, _fullscreen, this));
        }

        private void ApplyLanguage()
		{
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
			
			lblQuickConnect.Text = Language.strLabelConnect;
			btnQuickConnect.Text = Language.strMenuConnect;
			btnConnections.Text = Language.strMenuConnections;
			
			cMenToolbarShowText.Text = Language.strMenuShowText;
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
		
		private static void ApplyMenuColors(IEnumerable itemCollection)
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
            if (!Settings.Default.CheckForUpdatesAsked)
            {
                string[] commandButtons = 
                {
                    Language.strAskUpdatesCommandRecommended, Language.strAskUpdatesCommandCustom,
                    Language.strAskUpdatesCommandAskLater
                };

                CTaskDialog.ShowTaskDialogBox(this, GeneralAppInfo.ProductName, Language.strAskUpdatesMainInstruction, string.Format(Language.strAskUpdatesContent, GeneralAppInfo.ProductName),
                    "", "", "", "", string.Join(" | ", commandButtons), ETaskDialogButtons.None, ESysIcons.Question, ESysIcons.Question);

                if (CTaskDialog.CommandButtonResult == 0 | CTaskDialog.CommandButtonResult == 1)
                {
                    Settings.Default.CheckForUpdatesAsked = true;
                }

                if (CTaskDialog.CommandButtonResult != 1) return;

                using (var optionsForm = new frmOptions(Language.strTabUpdates))
                {
                    optionsForm.ShowDialog(this);
                }

                return;
            }

            if (!Settings.Default.CheckForUpdatesOnStartup) return;

            var nextUpdateCheck = Convert.ToDateTime(
                    Settings.Default.CheckForUpdatesLastCheck.Add(
                        TimeSpan.FromDays(Convert.ToDouble(Settings.Default.CheckForUpdatesFrequencyDays))));

            if (!Settings.Default.UpdatePending && DateTime.UtcNow <= nextUpdateCheck) return;
            if (!IsHandleCreated) CreateHandle(); // Make sure the handle is created so that InvokeRequired returns the correct result

            Startup.Instance.CheckForUpdate();
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
						return;
					}
				}
			}

            Shutdown.Cleanup();
									
			IsClosing = true;

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
            Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, "Doing AutoSave");
			Runtime.SaveConnectionsAsync();
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
						button.DisplayStyle = button.Image != null ? ToolStripItemDisplayStyle.Image : ToolStripItemDisplayStyle.ImageAndText;
					}
											
					button.Tag = tool;
				}
			}
			catch (Exception ex)
			{
                Runtime.MessageCollector.AddExceptionStackTrace(Language.strErrorAddExternalToolsToToolBarFailed, ex);
			}
		}
								
		private static void tsExtAppEntry_Click(object sender, EventArgs e)
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
        #region Tools
        private void credentialManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var credentialManagerForm = new CredentialManagerForm(_credentialManager)
            {
                DeletionConfirmer = new CredentialDeletionMsgBoxConfirmer(MessageBox.Show)
            };
            credentialManagerForm.CenterOnTarget(this);
            credentialManagerForm.Show();
        }

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
				    if (fieldInfo.Name == "value__" || fieldInfo.Name == "IntApp") continue;
				    var menuItem = new ToolStripMenuItem(fieldInfo.Name);
				    if (fieldInfo.Name == Settings.Default.QuickConnectProtocol)
				    {
				        menuItem.Checked = true;
				        btnQuickConnect.Text = Settings.Default.QuickConnectProtocol;
				    }
				    mnuQuickConnectProtocol.Items.Add(menuItem);
				}
			}
			catch (Exception ex)
			{
                Runtime.MessageCollector.AddExceptionStackTrace("PopulateQuickConnectProtocolMenu() failed.", ex);
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
					return;
				}
				cmbQuickConnect.Add(connectionInfo);
                _connectionInitiator.OpenConnection(connectionInfo, ConnectionInfo.Force.DoNotJump);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace("btnQuickConnect_ButtonClick() failed.", ex);
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
			    menuItem.Checked = menuItem.Text.Equals(protocol);
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

		    // ReSharper disable once CoVariantArrayConversion
            ToolStripItem[] rootMenuItems = menuItemsConverter.CreateToolStripDropDownItems(Runtime.ConnectionTreeModel).ToArray();
            btnConnections.DropDownItems.AddRange(rootMenuItems);
		}
										
		private void ConnectionsMenuItem_MouseUp(object sender, MouseEventArgs e)
		{
		    if (e.Button != MouseButtons.Left) return;
		    if (((ToolStripMenuItem) sender).Tag is ContainerInfo) return;
		    var tag = ((ToolStripMenuItem)sender).Tag as ConnectionInfo;
		    if (tag != null)
		    {
		        _connectionInitiator.OpenConnection(tag);
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
			    if (!Settings.Default.MinimizeToTray) return;
			    if (Runtime.NotificationAreaIcon == null)
			    {
			        Runtime.NotificationAreaIcon = new NotificationAreaIcon();
			    }
			    Hide();
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
			    // ReSharper disable once SwitchStatementMissingSomeCases
				switch (m.Msg)
				{
				    case NativeMethods.WM_MOUSEACTIVATE:
				        _inMouseActivate = true;
				        break;
				    case NativeMethods.WM_ACTIVATEAPP:
				        _inMouseActivate = false;
				        break;
				    case NativeMethods.WM_ACTIVATE:
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
				                        var tempWLow = clientMousePosition.X;
				                        var tempWHigh = clientMousePosition.Y;
				                        NativeMethods.SendMessage(control.Handle, NativeMethods.WM_LBUTTONDOWN, (IntPtr)NativeMethods.MK_LBUTTON, (IntPtr)NativeMethods.MAKELPARAM(ref tempWLow, ref tempWHigh));
				                        clientMousePosition.X = tempWLow;
				                        clientMousePosition.Y = tempWHigh;
				                        control.Focus();
				                    }
				                }
				            }

				            // This handles activations from clicks that did not start a size/move operation
				            ActivateConnection();
				        }
				        break;
				    case NativeMethods.WM_WINDOWPOSCHANGED:
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
				        break;
				    case NativeMethods.WM_SYSCOMMAND:
				        var screen = _screenSystemMenu.GetScreenById(m.WParam.ToInt32());
                        if (screen != null)
                            Screens.SendFormToScreen(screen);
				        break;
				    case NativeMethods.WM_DRAWCLIPBOARD:
				        NativeMethods.SendMessage(_fpChainedWindowHandle, m.Msg, m.LParam, m.WParam);
				        _clipboardChangedEvent?.Invoke();
				        break;
				    case NativeMethods.WM_CHANGECBCHAIN:
				        //Send to the next window
				        NativeMethods.SendMessage(_fpChainedWindowHandle, m.Msg, m.LParam, m.WParam);
				        _fpChainedWindowHandle = m.LParam;
				        break;
				}
			}
			catch (Exception ex)
			{
                Runtime.MessageCollector.AddExceptionStackTrace("frmMain WndProc failed", ex);
            }
									
			base.WndProc(ref m);
		}
        
		private void ActivateConnection()
		{
		    var w = pnlDock.ActiveDocument as ConnectionWindow;
		    if (w?.TabController.SelectedTab == null) return;
		    var tab = w.TabController.SelectedTab;
		    var ifc = (InterfaceControl)tab.Tag;

		    if (ifc == null) return;

		    ifc.Protocol.Focus();
		    ((ConnectionWindow) ifc.FindForm())?.RefreshInterfaceController();
		}

        private void pnlDock_ActiveDocumentChanged(object sender, EventArgs e)
		{
			ActivateConnection();
            var connectionWindow = pnlDock.ActiveDocument as ConnectionWindow;
		    connectionWindow?.UpdateSelectedConnection();
		}
		
		private void UpdateWindowTitle()
		{
			if (InvokeRequired)
			{
				Invoke(new MethodInvoker(UpdateWindowTitle));
				return;
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
					    titleBuilder.Append(Settings.Default.ShowCompleteConsPathInTitle
					        ? ConnectionsFileName
					        : Path.GetFileName(ConnectionsFileName));
					}
				}
			}
									
			if (!string.IsNullOrEmpty(SelectedConnection?.Name))
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

		    if (pnlDock.DocumentStyle == newDocumentStyle) return;
		    pnlDock.DocumentStyle = newDocumentStyle;
		    pnlDock.Size = new Size(1, 1);
		}

#if false
        private void SelectTabRelative(int relativeIndex)
		{
			if (!(pnlDock.ActiveDocument is ConnectionWindow))
			{
				return;
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
        public void SetDefaultLayout()
        {
            Default.pnlDock.Visible = false;

            Default.pnlDock.DockLeftPortion = Default.pnlDock.Width * 0.2;
            Default.pnlDock.DockRightPortion = Default.pnlDock.Width * 0.2;
            Default.pnlDock.DockTopPortion = Default.pnlDock.Height * 0.25;
            Default.pnlDock.DockBottomPortion = Default.pnlDock.Height * 0.25;

            Windows.TreeForm.Show(Default.pnlDock, DockState.DockLeft);
            Windows.ConfigForm.Show(Default.pnlDock);
            Windows.ConfigForm.DockTo(Windows.TreeForm.Pane, DockStyle.Bottom, -1);
            Windows.ErrorsForm.Show(Default.pnlDock, DockState.Document);

            Windows.ErrorsForm.Hide();
            Windows.ScreenshotForm.Hide();

            Default.pnlDock.Visible = true;
        }
        #endregion

        #region Events
        public delegate void ClipboardchangeEventHandler();
        public static event ClipboardchangeEventHandler ClipboardChanged
        {
            add
            {
                _clipboardChangedEvent = (ClipboardchangeEventHandler)Delegate.Combine(_clipboardChangedEvent, value);
            }
            remove
            {
                _clipboardChangedEvent = (ClipboardchangeEventHandler)Delegate.Remove(_clipboardChangedEvent, value);
            }
        }
        #endregion
    }
}