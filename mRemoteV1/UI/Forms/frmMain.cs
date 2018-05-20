using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.App.Initialization;
using mRemoteNG.App.Update;
using mRemoteNG.Config;
using mRemoteNG.Config.Connections;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Config.Putty;
using mRemoteNG.Config.Settings;
using mRemoteNG.Connection;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Messages;
using mRemoteNG.Messages.MessageWriters;
using mRemoteNG.Themes;
using mRemoteNG.Tools;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Controls.Adapters;
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
        private readonly ConnectionInitiator _connectionInitiator;
        private readonly PanelAdder _panelAdder;
        private readonly WebHelper _webHelper;
        private readonly WindowList _windowList;
        private readonly Windows _windows;
	    private readonly Startup _startup;
        private readonly Export _export;
        private readonly SettingsLoader _settingsLoader;
        private readonly SettingsSaver _settingsSaver;
        private readonly Shutdown _shutdown;
        private readonly ICredentialRepositoryList _credentialRepositoryList;
        private readonly Func<NotificationAreaIcon> _notificationAreaIconBuilder;
        private readonly ConnectionsService _connectionsService;
        private readonly Import _import;
        private readonly AppUpdater _appUpdater;
        private readonly DatabaseConnectorFactory _databaseConnectorFactory;

        internal FullscreenHandler Fullscreen { get; set; }
        
        //Added theming support
        private readonly ToolStripRenderer _toolStripProfessionalRenderer = new ToolStripProfessionalRenderer();

        private FrmMain()
		{
            _windowList = new WindowList();
            _credentialRepositoryList = new CredentialRepositoryList();
            var externalToolsService = new ExternalToolsService();
		    _import = new Import();
		    _connectionsService = new ConnectionsService(PuttySessionsManager.Instance, _import);
		    Runtime.ConnectionsService = _connectionsService;
		    _import.ConnectionsService = _connectionsService;
		    Func<SecureString> encryptionKeySelectionFunc = () => _connectionsService.EncryptionKey;
            _appUpdater = new AppUpdater(encryptionKeySelectionFunc);
            ExternalToolsTypeConverter.ExternalToolsService = externalToolsService;
            _export = new Export(_credentialRepositoryList, _connectionsService);
            _connectionInitiator = new ConnectionInitiator(_windowList, externalToolsService);
		    _webHelper = new WebHelper(_connectionInitiator);
		    var configWindow = new ConfigWindow(new DockContent());
		    var sshTransferWindow = new SSHTransferWindow();
		    var connectionTreeWindow = new ConnectionTreeWindow(new DockContent(), _connectionInitiator, _connectionsService);
			var connectionTree = connectionTreeWindow.ConnectionTree;
			connectionTree.SelectedNodeChanged += configWindow.HandleConnectionTreeSelectionChanged;
			var connectionTreeContextMenu = new ConnectionContextMenu(connectionTree, _connectionInitiator, sshTransferWindow, _export, externalToolsService, _import);
			connectionTree.ConnectionContextMenu = connectionTreeContextMenu;
			connectionTreeWindow.ConnectionTreeContextMenu = connectionTreeContextMenu;
		    var errorAndInfoWindow = new ErrorAndInfoWindow(new DockContent(), connectionTreeWindow);
		    var screenshotManagerWindow = new ScreenshotManagerWindow(new DockContent());
		    _settingsSaver = new SettingsSaver(externalToolsService);
            _shutdown = new Shutdown(_settingsSaver, _connectionsService);
		    Func<UpdateWindow> updateWindowBuilder = () => new UpdateWindow(new DockContent(), _shutdown, _appUpdater);
		    _notificationAreaIconBuilder = () => new NotificationAreaIcon(this, _connectionInitiator, _shutdown);
            Func<ExternalToolsWindow> externalToolsWindowBuilder = () => new ExternalToolsWindow(_connectionInitiator, externalToolsService);
		    Func<PortScanWindow> portScanWindowBuilder = () => new PortScanWindow(() => connectionTreeWindow.SelectedNode, _import);
		    Func<ActiveDirectoryImportWindow> activeDirectoryImportWindowBuilder = () => new ActiveDirectoryImportWindow(() => connectionTreeWindow.SelectedNode, _import);
		    _databaseConnectorFactory = new DatabaseConnectorFactory(encryptionKeySelectionFunc);
            _windows = new Windows(_connectionInitiator, connectionTreeWindow, configWindow, errorAndInfoWindow, screenshotManagerWindow, 
                sshTransferWindow, updateWindowBuilder, _notificationAreaIconBuilder, externalToolsWindowBuilder, _connectionsService, 
                portScanWindowBuilder, activeDirectoryImportWindowBuilder, _appUpdater, _databaseConnectorFactory);
            Func<ConnectionWindow> connectionWindowBuilder = () => new ConnectionWindow(new DockContent(), _connectionInitiator, _windows, externalToolsService);
            _panelAdder = new PanelAdder(_windowList, connectionWindowBuilder);
            _showFullPathInTitle = Settings.Default.ShowCompleteConsPathInTitle;
		    _connectionInitiator.Adder = _panelAdder;
		    _connectionsService.DatabaseConnectorFactory = _databaseConnectorFactory;
            _startup = new Startup(this, _windows, _connectionsService, _appUpdater, _databaseConnectorFactory);
            connectionTreeContextMenu.ShowWindowAction = _windows.Show;

			InitializeComponent();

            var externalAppsLoader = new ExternalAppsLoader(Runtime.MessageCollector, _externalToolsToolStrip, _connectionInitiator, externalToolsService);
		    _settingsLoader = new SettingsLoader(this, Runtime.MessageCollector, _quickConnectToolStrip, _externalToolsToolStrip, _multiSshToolStrip, externalAppsLoader, _notificationAreaIconBuilder);
		    _externalToolsToolStrip.ExternalToolsService = externalToolsService;
			_externalToolsToolStrip.GetSelectedConnectionFunc = () => SelectedConnection;
			_quickConnectToolStrip.ConnectionInitiator = _connectionInitiator;
		    _quickConnectToolStrip.ConnectionsService = _connectionsService;
		    CredentialRecordTypeConverter.CredentialRepositoryList = _credentialRepositoryList;
		    CredentialRecordListAdaptor.CredentialRepositoryList = _credentialRepositoryList;

            Fullscreen = new FullscreenHandler(this);

            //Theming support
            _themeManager = ThemeManager.getInstance();
            vsToolStripExtender.DefaultRenderer = _toolStripProfessionalRenderer;
            SetSchema();

            _screenSystemMenu = new ScreenSelectionSystemMenu(this);
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
            MessageCollectorSetup.SetupMessageCollector(messageCollector, _messageWriters);
            MessageCollectorSetup.BuildMessageWritersFromSettings(_messageWriters, this, _windows.ErrorsForm);

	        _startup.InitializeProgram(messageCollector);

            SetMenuDependencies();

            msMain.Location = Point.Empty;
            _settingsLoader.LoadSettings();

            var uiLoader = new DockPanelLayoutLoader(this, messageCollector, _windows);
            uiLoader.LoadPanelsFromXml();

	        LockToolbarPositions(Settings.Default.LockToolbars);
			Settings.Default.PropertyChanged += OnApplicationSettingChanged;

    		_themeManager.ThemeChanged += ApplyTheme; 

			_fpChainedWindowHandle = NativeMethods.SetClipboardViewer(Handle);

            if (Settings.Default.ResetPanels)
                SetDefaultLayout();

            _connectionsService.ConnectionsLoaded += ConnectionsServiceOnConnectionsLoaded;
            var credsAndConsSetup = new CredsAndConsSetup(_connectionsService);
            credsAndConsSetup.LoadCredsAndCons();

            _windows.TreeForm.Focus();

            PuttySessionsManager.Instance.StartWatcher();
			if (Settings.Default.StartupComponentsCheck)
			    _windows.Show(WindowType.ComponentsCheck);

	        _startup.CreateConnectionsProvider(messageCollector);

            _screenSystemMenu.BuildScreenList();
			SystemEvents.DisplaySettingsChanged += _screenSystemMenu.OnDisplayChanged;
            ApplyLanguage();

            Opacity = 1;
            //Fix missing general panel at the first run
            _panelAdder.AddPanel();
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

        private void SetMenuDependencies()
        {
            fileMenu.TreeWindow = _windows.TreeForm;
            fileMenu.ConnectionInitiator = _connectionInitiator;
            fileMenu.WindowList = _windowList;
            fileMenu.Windows = _windows;
            fileMenu.Export = _export;
            fileMenu.Shutdown = _shutdown;
            fileMenu.Import = _import;
            fileMenu.ConnectionsService = _connectionsService;

            viewMenu.TsExternalTools = _externalToolsToolStrip;
            viewMenu.TsQuickConnect = _quickConnectToolStrip;
            viewMenu.TsMultiSsh = _multiSshToolStrip;
            viewMenu.FullscreenHandler = Fullscreen;
            viewMenu.Adder = _panelAdder;
            viewMenu.WindowList = _windowList;
            viewMenu.Windows = _windows;
            viewMenu.MainForm = this;

            toolsMenu.MainForm = this;
            toolsMenu.CredentialProviderCatalog = _credentialRepositoryList;
            toolsMenu.Windows = _windows;

	        helpMenu.WebHelper = _webHelper;
	        helpMenu.Windows = _windows;
        }

        //Theming support
        private void SetSchema()
        {
            if (_themeManager.ThemingActive)
            {
                // Persist settings when rebuilding UI
                pnlDock.Theme = _themeManager.ActiveTheme.Theme;
                ApplyTheme();
            }
        }

        private void ApplyTheme()
		{
            if(_themeManager.ThemingActive)
            {
                vsToolStripExtender.SetStyle(msMain, _themeManager.ActiveTheme.Version, _themeManager.ActiveTheme.Theme);
                vsToolStripExtender.SetStyle(_quickConnectToolStrip, _themeManager.ActiveTheme.Version, _themeManager.ActiveTheme.Theme);
                vsToolStripExtender.SetStyle(_externalToolsToolStrip, _themeManager.ActiveTheme.Version, _themeManager.ActiveTheme.Theme);
                vsToolStripExtender.SetStyle(_multiSshToolStrip, _themeManager.ActiveTheme.Version, _themeManager.ActiveTheme.Theme);
                tsContainer.TopToolStripPanel.BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("CommandBarMenuDefault_Background");
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

            using (var optionsForm = new frmOptions(_connectionInitiator, _windows.Show, _notificationAreaIconBuilder, _connectionsService, _appUpdater, _databaseConnectorFactory, Language.strTabUpdates))
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

	        _startup.CheckForUpdate();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
            if (!(_windowList == null || _windowList.Count == 0))
			{
			    var openConnections = 0;
                foreach (BaseWindow window in _windowList)
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

		    _shutdown.Cleanup(_quickConnectToolStrip, _externalToolsToolStrip, _multiSshToolStrip, this);
									
			IsClosing = true;

            if (_windowList != null)
			{
                foreach (BaseWindow window in _windowList)
				{
					window.Close();
				}
			}

		    _shutdown.StartUpdate();
									
			Debug.Print("[END] - " + Convert.ToString(DateTime.Now, CultureInfo.InvariantCulture));
		}
        #endregion
								
        #region Timer
		private void tmrAutoSave_Tick(object sender, EventArgs e)
		{
            Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, "Doing AutoSave");
		    _connectionsService.SaveConnectionsAsync();
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
			        Runtime.NotificationAreaIcon = new NotificationAreaIcon(this, _connectionInitiator, _shutdown);
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
				            var controlThatWasClicked = FromChildHandle(NativeMethods.WindowFromPoint(MousePosition));
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
									
			if (_connectionsService.IsConnectionsFileLoaded)
			{
				if (_connectionsService.UsingDatabase)
				{
					titleBuilder.Append(separator);
					titleBuilder.Append(Language.strSQLServer.TrimEnd(':'));
				}
				else
				{
					if (!string.IsNullOrEmpty(_connectionsService.ConnectionFileName))
					{
					    titleBuilder.Append(separator);
					    titleBuilder.Append(Settings.Default.ShowCompleteConsPathInTitle
					        ? _connectionsService.ConnectionFileName
                            : Path.GetFileName(_connectionsService.ConnectionFileName));
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

            _windows.TreeForm.Show(pnlDock, DockState.DockLeft);
            _windows.ConfigForm.Show(pnlDock);
            _windows.ConfigForm.DockTo(_windows.TreeForm.Pane, DockStyle.Bottom, -1);
            _windows.ErrorsForm.Show( pnlDock, DockState.DockBottomAutoHide );
            _windows.ScreenshotForm.Hide(); 

            pnlDock.Visible = true;
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