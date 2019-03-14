using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.App.Initialization;
using mRemoteNG.Config;
using mRemoteNG.Config.Connections;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Putty;
using mRemoteNG.Config.Settings;
using mRemoteNG.Connection;
using mRemoteNG.Messages;
using mRemoteNG.Messages.MessageWriters;
using mRemoteNG.Themes;
using mRemoteNG.Tools;
using mRemoteNG.UI.Menu;
using mRemoteNG.UI.Panels;
using mRemoteNG.UI.TaskDialog;
using mRemoteNG.UI.Window;
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
        private readonly IList<IMessageWriter> _messageWriters = new List<IMessageWriter>();
        private readonly ThemeManager _themeManager;
        private readonly FileBackupPruner _backupPruner = new FileBackupPruner();

        internal FullscreenHandler Fullscreen { get; set; }
        
        //Added theming support
        private readonly ToolStripRenderer _toolStripProfessionalRenderer = new ToolStripProfessionalRenderer();

        private FrmMain()
		{
			_showFullPathInTitle = Settings.Default.ShowCompleteConsPathInTitle;
			InitializeComponent();
            Fullscreen = new FullscreenHandler(this);

            //Theming support
            _themeManager = ThemeManager.getInstance();
            vsToolStripExtender.DefaultRenderer = _toolStripProfessionalRenderer;
            ApplyTheme();

            _screenSystemMenu = new ScreenSelectionSystemMenu(this);
        }

        #region Properties
        public FormWindowState PreviousWindowState { get; set; }

	    public bool IsClosing { get; private set; }

        public bool AreWeUsingSqlServerForSavingConnections
		{
			get => _usingSqlServer;
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
			get => _connectionsFileName;
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
			get => _showFullPathInTitle;
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
			get => _selectedConnection;
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
            MessageCollectorSetup.SetupMessageCollector(messageCollector, _messageWriters);
            MessageCollectorSetup.BuildMessageWritersFromSettings(_messageWriters);

            Startup.Instance.InitializeProgram(messageCollector);

            msMain.Location = Point.Empty;
            var settingsLoader = new SettingsLoader(this, messageCollector, _quickConnectToolStrip, _externalToolsToolStrip, _multiSshToolStrip, msMain);
            settingsLoader.LoadSettings();

            SetMenuDependencies();

            var uiLoader = new DockPanelLayoutLoader(this, messageCollector);
            uiLoader.LoadPanelsFromXml();

	        LockToolbarPositions(Settings.Default.LockToolbars);
			Settings.Default.PropertyChanged += OnApplicationSettingChanged;

    		_themeManager.ThemeChanged += ApplyTheme; 

			_fpChainedWindowHandle = NativeMethods.SetClipboardViewer(Handle);

            Runtime.WindowList = new WindowList();

            if (Settings.Default.ResetPanels)
                SetDefaultLayout();

            Runtime.ConnectionsService.ConnectionsLoaded += ConnectionsServiceOnConnectionsLoaded;
            Runtime.ConnectionsService.ConnectionsSaved += ConnectionsServiceOnConnectionsSaved;
            var credsAndConsSetup = new CredsAndConsSetup();
            credsAndConsSetup.LoadCredsAndCons();

            Windows.TreeForm.Focus();

            PuttySessionsManager.Instance.StartWatcher();
			if (Settings.Default.StartupComponentsCheck)
                Windows.Show(WindowType.ComponentsCheck);

            Startup.Instance.CreateConnectionsProvider(messageCollector);

            _screenSystemMenu.BuildScreenList();
			SystemEvents.DisplaySettingsChanged += _screenSystemMenu.OnDisplayChanged;
            ApplyLanguage();

            Opacity = 1;
            //Fix missing general panel at the first run
            if (Settings.Default.CreateEmptyPanelOnStartUp)
            {
                var panelName = !string.IsNullOrEmpty(Settings.Default.StartUpPanelName)
                    ? Settings.Default.StartUpPanelName
                    : Language.strNewPanel;

                var panelAdder = new PanelAdder();
                if (!panelAdder.DoesPanelExist(panelName))
                    panelAdder.AddPanel(panelName);
            }
        }

        private void ApplyLanguage()
        {
            fileMenu.ApplyLanguage();
            viewMenu.ApplyLanguage();
            toolsMenu.ApplyLanguage();
            helpMenu.ApplyLanguage();
        }

        private void OnApplicationSettingChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
	    {
		    if (propertyChangedEventArgs.PropertyName != nameof(Settings.LockToolbars))
				return;

		    LockToolbarPositions(Settings.Default.LockToolbars);
	    }

	    private void LockToolbarPositions(bool shouldBeLocked)
	    {
		    var toolbars = new ToolStrip[] { _quickConnectToolStrip, _multiSshToolStrip, _externalToolsToolStrip, msMain };
			foreach (var toolbar in toolbars)
			{
				toolbar.GripStyle = shouldBeLocked
					? ToolStripGripStyle.Hidden
					: ToolStripGripStyle.Visible;
			}
		}

        private void ConnectionsServiceOnConnectionsLoaded(object sender, ConnectionsLoadedEventArgs connectionsLoadedEventArgs)
        {
            UpdateWindowTitle();
        }

        private void ConnectionsServiceOnConnectionsSaved(object sender, ConnectionsSavedEventArgs connectionsSavedEventArgs)
        {
            if (connectionsSavedEventArgs.UsingDatabase)
                return;

            _backupPruner.PruneBackupFiles(connectionsSavedEventArgs.ConnectionFileName, Settings.Default.BackupFileKeepCount);
        }

        private void SetMenuDependencies()
        {
            var connectionInitiator = new ConnectionInitiator();
            fileMenu.TreeWindow = Windows.TreeForm;
            fileMenu.ConnectionInitiator = connectionInitiator;

            viewMenu.TsExternalTools = _externalToolsToolStrip;
            viewMenu.TsQuickConnect = _quickConnectToolStrip;
	        viewMenu.TsMultiSsh = _multiSshToolStrip;
            viewMenu.FullscreenHandler = Fullscreen;
            viewMenu.MainForm = this;

            toolsMenu.MainForm = this;
            toolsMenu.CredentialProviderCatalog = Runtime.CredentialProviderCatalog;

            _quickConnectToolStrip.ConnectionInitiator = connectionInitiator;
        }

        //Theming support
        private void ApplyTheme()
		{
		    if (!_themeManager.ThemingActive) return;

		    try
		    {
                // this will always throw when turning themes on from
                // the options menu.
		        pnlDock.Theme = _themeManager.ActiveTheme.Theme;
		    }
		    catch (Exception)
		    {
		        // intentionally ignore exception
		    }
            
		    // Persist settings when rebuilding UI
		    try
		    {
                vsToolStripExtender.SetStyle(msMain, _themeManager.ActiveTheme.Version, _themeManager.ActiveTheme.Theme);
		        vsToolStripExtender.SetStyle(_quickConnectToolStrip, _themeManager.ActiveTheme.Version, _themeManager.ActiveTheme.Theme);
		        vsToolStripExtender.SetStyle(_externalToolsToolStrip, _themeManager.ActiveTheme.Version, _themeManager.ActiveTheme.Theme);
		        vsToolStripExtender.SetStyle(_multiSshToolStrip, _themeManager.ActiveTheme.Version, _themeManager.ActiveTheme.Theme);
		        tsContainer.TopToolStripPanel.BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("CommandBarMenuDefault_Background");
		        BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
		        ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
		    }
		    catch (Exception ex)
		    {
                Runtime.MessageCollector.AddExceptionStackTrace("Error applying theme", ex, MessageClass.WarningMsg);
		    }
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            PromptForUpdatesPreference();
            CheckForUpdates();
        }

        private void PromptForUpdatesPreference()
        {
            if (Settings.Default.CheckForUpdatesAsked) return;
            string[] commandButtons =
            {
                Language.strAskUpdatesCommandRecommended,
                Language.strAskUpdatesCommandCustom,
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
        }

        private void CheckForUpdates()
        {
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

            Shutdown.Cleanup(_quickConnectToolStrip, _externalToolsToolStrip, _multiSshToolStrip, this);
									
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
			Runtime.ConnectionsService.SaveConnectionsAsync();
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
				        // Only handle this msg if it was triggered by a click
				        if (NativeMethods.LOWORD(m.WParam) == NativeMethods.WA_CLICKACTIVE)
				        {
				            var controlThatWasClicked = FromChildHandle(NativeMethods.WindowFromPoint(MousePosition))
				                ?? GetChildAtPoint(MousePosition);
				            if (controlThatWasClicked != null)
				            {
				                if (controlThatWasClicked is TreeView ||
				                    controlThatWasClicked is ComboBox ||
				                    controlThatWasClicked is TextBox)
				                {
				                    controlThatWasClicked.Focus();
				                }
				                else if (controlThatWasClicked.CanSelect ||
				                         controlThatWasClicked is MenuStrip ||
				                         controlThatWasClicked is ToolStrip ||
				                         controlThatWasClicked is Crownwood.Magic.Controls.TabControl ||
				                         controlThatWasClicked is Crownwood.Magic.Controls.InertButton)
				                {
                                    // Simulate a mouse event since one wasn't generated by Windows
                                    SimulateClick(controlThatWasClicked);
                                    controlThatWasClicked.Focus();
                                }
                                else if (controlThatWasClicked is AutoHideStripBase)
				                {
                                    // only focus the autohide toolstrip
                                    controlThatWasClicked.Focus();
				                }
				                else
				                {
				                    // This handles activations from clicks that did not start a size/move operation
				                    ActivateConnection();
				                }
				            }
				        }
				        break;
				    case NativeMethods.WM_WINDOWPOSCHANGED:
				        // Ignore this message if the window wasn't activated
				        var windowPos = (NativeMethods.WINDOWPOS)Marshal.PtrToStructure(m.LParam, typeof(NativeMethods.WINDOWPOS));
				        if ((windowPos.flags & NativeMethods.SWP_NOACTIVATE) == 0)
				        {
				            if (!_inMouseActivate && !_inSizeMove)
				                ActivateConnection();
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

        private void SimulateClick(Control control)
        {
            var clientMousePosition = control.PointToClient(MousePosition);
            var temp_wLow = clientMousePosition.X;
            var temp_wHigh = clientMousePosition.Y;
            NativeMethods.SendMessage(control.Handle, NativeMethods.WM_LBUTTONDOWN, (IntPtr)NativeMethods.MK_LBUTTON, (IntPtr)NativeMethods.MAKELPARAM(ref temp_wLow, ref temp_wHigh));
            clientMousePosition.X = temp_wLow;
            clientMousePosition.Y = temp_wHigh;
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
		
		internal void UpdateWindowTitle()
		{
			if (InvokeRequired)
			{
				Invoke(new MethodInvoker(UpdateWindowTitle));
				return;
			}
									
			var titleBuilder = new StringBuilder(Application.ProductName);
			const string separator = " - ";
									
			if (Runtime.ConnectionsService.IsConnectionsFileLoaded)
			{
				if (Runtime.ConnectionsService.UsingDatabase)
				{
					titleBuilder.Append(separator);
					titleBuilder.Append(Language.strSQLServer.TrimEnd(':'));
				}
				else
				{
					if (!string.IsNullOrEmpty(Runtime.ConnectionsService.ConnectionFileName))
					{
					    titleBuilder.Append(separator);
					    titleBuilder.Append(Settings.Default.ShowCompleteConsPathInTitle
					        ? Runtime.ConnectionsService.ConnectionFileName
                            : Path.GetFileName(Runtime.ConnectionsService.ConnectionFileName));
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
            pnlDock.Visible = false;

            pnlDock.DockLeftPortion = pnlDock.Width * 0.2;
            pnlDock.DockRightPortion = pnlDock.Width * 0.2;
            pnlDock.DockTopPortion = pnlDock.Height * 0.25;
            pnlDock.DockBottomPortion = pnlDock.Height * 0.25;

            Windows.TreeForm.Show(pnlDock, DockState.DockLeft);
            Windows.ConfigForm.Show(pnlDock);
            Windows.ConfigForm.DockTo(Windows.TreeForm.Pane, DockStyle.Bottom, -1);
            Windows.ErrorsForm.Show( pnlDock, DockState.DockBottomAutoHide );  
            Windows.ScreenshotForm.Hide(); 

            pnlDock.Visible = true;
        }
        #endregion

        #region Events
        public delegate void ClipboardchangeEventHandler();
        public static event ClipboardchangeEventHandler ClipboardChanged
        {
            add => _clipboardChangedEvent = (ClipboardchangeEventHandler)Delegate.Combine(_clipboardChangedEvent, value);
            remove => _clipboardChangedEvent = (ClipboardchangeEventHandler)Delegate.Remove(_clipboardChangedEvent, value);
        }
        #endregion

        private void ViewMenu_Opening(object sender, EventArgs e)
        {
            viewMenu.mMenView_DropDownOpening(sender, e);
        }

        private void mainFileMenu1_DropDownOpening(object sender, EventArgs e)
        {
            fileMenu.mMenFile_DropDownOpening(sender, e);
        }
    }
}