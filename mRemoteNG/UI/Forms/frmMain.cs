#region Usings
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
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Messages;
using mRemoteNG.Messages.MessageWriters;
using mRemoteNG.Themes;
using mRemoteNG.Tools;
using mRemoteNG.Tools.Cmdline;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.Menu;
using mRemoteNG.UI.Tabs;
using mRemoteNG.UI.TaskDialog;
using mRemoteNG.UI.Window;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using mRemoteNG.UI.Panels;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.UI.Controls;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;
using mRemoteNG.Config.Settings.Registry;
using System.Threading; // ADDED
using mRemoteNG.Config.Connections.Multiuser;
#endregion

// ReSharper disable MemberCanBePrivate.Global

namespace mRemoteNG.UI.Forms
{
    [SupportedOSPlatform("windows")]
    public partial class FrmMain : IMessageFilter
    {
        // CHANGED: lazy, thread-safe, STA-enforced initialization
        private static readonly Lazy<FrmMain> s_default =
            new(InitializeOnSta, LazyThreadSafetyMode.ExecutionAndPublication);

        public static FrmMain Default => s_default.Value;

        public static bool IsCreated => s_default.IsValueCreated;

        private static FrmMain InitializeOnSta()
        {
            // Enforce STA to avoid OLE/WinForms threading violations
            if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
            {
                // If we're already on a WinForms UI thread with a sync context, marshal to it
                if (SynchronizationContext.Current is WindowsFormsSynchronizationContext ctx)
                {
                    FrmMain? created = null;
                    ctx.Send(_ => created = new FrmMain(), null);
                    return created!;
                }

                throw new ThreadStateException("FrmMain must be created on an STA thread.");
            }

            try
            {
                return new FrmMain();
            }
            catch (ConfigurationErrorsException ex)
            {
                ProgramRoot.HandleCorruptedUserConfig(ex);
                return new FrmMain();
            }
        }

        private static ClipboardchangeEventHandler? _clipboardChangedEvent;
        private bool _inSizeMove;
        private bool _inMouseActivate;
        private bool _isApplicationActivated = true;
        private bool _pendingActivateConnectionOnAppReactivation;
        private bool _usingSqlServer;
        private string? _connectionsFileName;
        private bool _showFullPathInTitle;
        private readonly AdvancedWindowMenu _advancedWindowMenu;
        private ConnectionInfo? _selectedConnection;
        private readonly IList<IMessageWriter> _messageWriters = [];
        private readonly ThemeManager _themeManager;
        private readonly NewCustomTab _screenshotsBottomTab = new();
        private readonly FileBackupPruner _backupPruner = new();
        private readonly System.Windows.Forms.Timer _autoLockTimer = new() { Interval = 1000 };
        private const int AutoLockIdleThresholdMs = 5 * 60 * 1000;
        private const int HOTKEY_ID_ACTIVATE = 1;
        private bool _isAutoLocked;
        private bool _unlockPromptInProgress;
        public static FrmOptions? OptionsForm;

        /// <summary>
        /// Recreates the OptionsForm if it has been disposed.
        /// This method should be called when OptionsForm is in an invalid state.
        /// </summary>
        public static void RecreateOptionsForm()
        {
            Logger.Instance.Log?.Debug("[FrmMain.RecreateOptionsForm] Recreating OptionsForm");

            // Dispose the old form if it exists
            if (OptionsForm != null && !OptionsForm.IsDisposed)
            {
                Logger.Instance.Log?.Debug("[FrmMain.RecreateOptionsForm] Disposing old OptionsForm");
                OptionsForm.Dispose();
            }

            // Create a new instance
            OptionsForm = new FrmOptions();
            Logger.Instance.Log?.Debug("[FrmMain.RecreateOptionsForm] New OptionsForm created");
        }

        internal FullscreenHandler Fullscreen { get; set; }
        internal mRemoteNG.UI.PresentationModeHandler PresentationMode { get; set; }

        /// <summary>
        /// The <see cref="Properties.Settings"/> instance used by this form.
        /// Defaults to <see cref="Properties.Settings.Default"/> but can be
        /// overridden via the <see cref="FrmMain(Properties.Settings)"/>
        /// constructor to enable in-memory-only acceptance testing (#1259).
        /// </summary>
        internal Properties.Settings Settings { get; }

        //Added theming support
        private readonly ToolStripRenderer _toolStripProfessionalRenderer = new ToolStripProfessionalRenderer();

        private FrmMain() : this(Properties.Settings.Default) { }

        /// <summary>
        /// Creates an <see cref="FrmMain"/> instance backed by the given
        /// <paramref name="settings"/> object instead of the file-persisted
        /// <see cref="Properties.Settings.Default"/> singleton.
        /// </summary>
        internal FrmMain(Properties.Settings settings)
        {
            Settings = settings;
            _showFullPathInTitle = Properties.OptionsAppearancePage.Default.ShowCompleteConsPathInTitle;
            InitializeComponent();

            Screen targetScreen = (Screen.AllScreens.Length > 1) ? Screen.AllScreens[1] : Screen.AllScreens[0];

            Rectangle viewport = targetScreen.WorkingArea;
            
            // normally it should be screens[1] however due DPI apply 1 size "same" as default with 100%
            this.Left = viewport.Left + (targetScreen.Bounds.Size.Width / 2) - (this.Width / 2);
            this.Top = viewport.Top + (targetScreen.Bounds.Size.Height / 2) - (this.Height / 2);

            Fullscreen = new FullscreenHandler(this);
            PresentationMode = new mRemoteNG.UI.PresentationModeHandler(this);

            //Theming support
            _themeManager = ThemeManager.getInstance();
            vsToolStripExtender.DefaultRenderer = _toolStripProfessionalRenderer;
            ApplyTheme();

            _advancedWindowMenu = new AdvancedWindowMenu(this);
            _autoLockTimer.Tick += AutoLockTimer_Tick;
            Application.AddMessageFilter(this);
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

        public string? ConnectionsFileName
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

        public ConnectionInfo? SelectedConnection
        {
            get => _selectedConnection;
            set
            {
                if (_selectedConnection != value)
                    _selectedConnection = value;

                UpdateWindowTitle();
            }
        }

        #endregion

        #region Startup & Shutdown

        private void FrmMain_Load(object sender, EventArgs e)
        {
            MessageCollector messageCollector = Runtime.MessageCollector;

            SettingsLoader settingsLoader = new(this, messageCollector, _quickConnectToolStrip, _externalToolsToolStrip, _multiSshToolStrip, msMain);
            settingsLoader.LoadSettings();
            ApplyWindowSizeLockSetting();

            MessageCollectorSetup.SetupMessageCollector(messageCollector, _messageWriters);
            MessageCollectorSetup.BuildMessageWritersFromSettings(_messageWriters);

            Startup.Instance.InitializeProgram(messageCollector);

            SetMenuDependencies();

            DockPanelLayoutLoader uiLoader = new(this, messageCollector);
            uiLoader.LoadPanelsFromXml();

            ShowHidePanelTabs();

            LockToolbarPositions(Settings.LockToolbars);
            Settings.PropertyChanged += OnApplicationSettingChanged;
            Properties.OptionsConnectionsPage.Default.PropertyChanged += OnConnectionsPageSettingChanged;

            _themeManager.ThemeChanged += ApplyTheme;

            NativeMethods.AddClipboardFormatListener(Handle);

            Runtime.WindowList = [];

            if (Properties.App.Default.ResetPanels)
                SetDefaultLayout();
            else
                SetLayout();

            ShowHidePanelTabs();
            SetPanelLock();

            Runtime.ConnectionsService.ConnectionsLoaded += ConnectionsServiceOnConnectionsLoaded;
            Runtime.ConnectionsService.ConnectionsSaved += ConnectionsServiceOnConnectionsSaved;
            
            // Close splash screen and shut down its WPF Dispatcher to prevent the
            // background WPF message pump from intercepting WinForms mouse events.
            ProgramRoot.CloseSplash();

            CredsAndConsSetup.LoadCredsAndCons();
            _autoLockTimer.Start();

            // Initialize panel binding for Connections and Config panels
            UI.Panels.PanelBinder.Instance.Initialize();

            // Respect the active panel restored from persisted dock layout.
            // Fallback to the Connections panel only when no active content was restored.
            if (pnlDock.ActiveContent == null && AppWindows.TreeForm?.Visible == true)
            {
                AppWindows.TreeForm.Focus();
            }

            PuttySessionsManager.Instance.StartWatcher();

            Startup.CreateConnectionsProvider(messageCollector);

            _advancedWindowMenu.BuildAdditionalMenuItems();
            SystemEvents.DisplaySettingsChanged += _advancedWindowMenu.OnDisplayChanged;
            SystemEvents.DisplaySettingsChanged += OnDisplaySettingsChanged;
            SystemEvents.PowerModeChanged += OnPowerModeChanged;
            ApplyLanguage();

            Opacity = 1;
            //Fix MagicRemove , revision on panel strategy for mdi

            pnlDock.ShowDocumentIcon = true;

            // Register global hotkey Ctrl+Alt+Home to activate mRemoteNG (#1169)
            NativeMethods.RegisterHotKey(Handle, HOTKEY_ID_ACTIVATE,
                NativeMethods.MOD_CONTROL | NativeMethods.MOD_ALT | NativeMethods.MOD_NOREPEAT,
                NativeMethods.VK_HOME);

            if (Properties.OptionsStartupExitPage.Default.StartMinimized)
            {
                WindowState = FormWindowState.Minimized;
                if (Properties.OptionsAppearancePage.Default.MinimizeToTray)
                    ShowInTaskbar = false;
            }
            if (Properties.OptionsStartupExitPage.Default.StartFullScreen)
            {
                Fullscreen.Value = true;
            }

            OptionsForm = new FrmOptions();

            // Auto-start external tools flagged with RunOnStartup (#318)
            foreach (var tool in Runtime.ExternalToolsService.ExternalTools)
            {
                if (tool.RunOnStartup)
                    tool.StartForAutoRun();
            }

            StartConnectionsRequestedOnStartupFromCommandLine();

            if (!Properties.OptionsTabsPanelsPage.Default.CreateEmptyPanelOnStartUp)
            {
                return;
            }
            string panelName = !string.IsNullOrEmpty(Properties.OptionsTabsPanelsPage.Default.StartUpPanelName) ? Properties.OptionsTabsPanelsPage.Default.StartUpPanelName : "New Panel";

            if (!PanelAdder.DoesPanelExist(panelName))
                PanelAdder.AddPanel(panelName);
        }

        private void StartConnectionsRequestedOnStartupFromCommandLine()
        {
            string? startupConnectTo = StartupArgumentsInterpreter.StartupConnectTo;
            if (!string.IsNullOrWhiteSpace(startupConnectTo))
            {
                RootNodeInfo? rootConnectionNode = Runtime.ConnectionsService.ConnectionTreeModel?.RootNodes
                    .OfType<RootNodeInfo>()
                    .FirstOrDefault();
                if (rootConnectionNode != null)
                    new CommandLineConnectionOpener(Runtime.ConnectionInitiator, startupConnectTo, "--startup").Execute(rootConnectionNode);
            }

            string? quickConnectTo = StartupArgumentsInterpreter.QuickConnectTo;
            if (!string.IsNullOrWhiteSpace(quickConnectTo))
            {
                string protocol = StartupArgumentsInterpreter.QuickConnectProtocol
                    ?? Properties.Settings.Default.QuickConnectProtocol;
                ConnectionInfo? connectionInfo = ConnectionsService.CreateQuickConnect(
                    quickConnectTo, Converter.StringToProtocol(protocol));
                if (connectionInfo != null)
                    Runtime.ConnectionInitiator.OpenConnection(connectionInfo, ConnectionInfo.Force.DoNotJump);
            }

            if (StartupArgumentsInterpreter.ExitAfterLastConnection)
                Runtime.ConnectionInitiator.ConnectionClosed += OnConnectionClosedExitAfterLast;
        }

        private void OnConnectionClosedExitAfterLast(string hostname, string protocol)
        {
            if (Runtime.ConnectionInitiator.ActiveConnections.Any())
                return;

            Runtime.ConnectionInitiator.ConnectionClosed -= OnConnectionClosedExitAfterLast;

            if (InvokeRequired)
                BeginInvoke(Application.Exit);
            else
                Application.Exit();
        }

        private void ApplyLanguage()
        {
            fileMenu.ApplyLanguage();
            sessionsMenu.ApplyLanguage();
            viewMenu.ApplyLanguage();
            connectionsMenu.ApplyLanguage();
            toolsMenu.ApplyLanguage();
            helpMenu.ApplyLanguage();
        }

        private void OnApplicationSettingChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            switch (propertyChangedEventArgs.PropertyName)
            {
                case nameof(Properties.Settings.LockToolbars):
                    LockToolbarPositions(Settings.LockToolbars);
                    break;
                case nameof(Properties.Settings.LockWindowSize):
                    ApplyWindowSizeLockSetting();
                    break;
                case nameof(Properties.Settings.ViewMenuExternalTools):
                    LockToolbarPositions(Settings.LockToolbars);
                    break;
                case nameof(Properties.Settings.ViewMenuMessages):
                    LockToolbarPositions(Settings.LockToolbars);
                    break;
                case nameof(Properties.Settings.ViewMenuMultiSSH):
                    LockToolbarPositions(Settings.LockToolbars);
                    break;
                case nameof(Properties.Settings.ViewMenuQuickConnect):
                    LockToolbarPositions(Settings.LockToolbars);
                    break;
                default:
                    return;
            }
        }

        private void OnConnectionsPageSettingChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Properties.OptionsConnectionsPage.Default.WatchConnectionFile))
            {
                if (Properties.OptionsConnectionsPage.Default.WatchConnectionFile)
                {
                    // Enable file watcher
                    if (!Properties.OptionsDBsPage.Default.UseSQLServer)
                    {
                        string startupFile = ConnectionsService.GetStartupConnectionFileName();
                        if (!string.IsNullOrEmpty(startupFile))
                        {
                            Runtime.ConnectionsService.RemoteConnectionsSyncronizer?.Dispose();
                            Runtime.ConnectionsService.RemoteConnectionsSyncronizer = new RemoteConnectionsSyncronizer(new FileConnectionsUpdateChecker(startupFile));
                            Runtime.ConnectionsService.RemoteConnectionsSyncronizer.Enable();
                        }
                    }
                }
                else
                {
                    // Disable file watcher (if active and not using SQL)
                    if (!Properties.OptionsDBsPage.Default.UseSQLServer)
                    {
                        Runtime.ConnectionsService.RemoteConnectionsSyncronizer?.Disable();
                        Runtime.ConnectionsService.RemoteConnectionsSyncronizer?.Dispose();
                        Runtime.ConnectionsService.RemoteConnectionsSyncronizer = null;
                    }
                }
            }
        }

        private void LockToolbarPositions(bool shouldBeLocked)
        {
            ToolStrip[] toolbars = [_quickConnectToolStrip, _multiSshToolStrip, _externalToolsToolStrip, msMain];
            foreach (ToolStrip toolbar in toolbars)
            {
                toolbar.GripStyle = shouldBeLocked ? ToolStripGripStyle.Hidden : ToolStripGripStyle.Visible;
            }
        }

        private void ApplyWindowSizeLockSetting()
        {
            bool lockWindowSize = Settings.LockWindowSize;
            FormBorderStyle = lockWindowSize ? FormBorderStyle.FixedSingle : FormBorderStyle.Sizable;
            MaximizeBox = !lockWindowSize;
        }

        private void ConnectionsServiceOnConnectionsLoaded(object? sender, ConnectionsLoadedEventArgs connectionsLoadedEventArgs)
        {
            UpdateWindowTitle();
            UI.Taskbar.JumpListManager.Initialize();
            StartRestApiIfConfigured();
        }

        private static void StartRestApiIfConfigured()
        {
            if (Runtime.RestApi is { IsRunning: true }) return;

            try
            {
                string configPath = System.IO.Path.Combine(App.Info.SettingsFileInfo.SettingsPath, "restapi.json");
                if (!System.IO.File.Exists(configPath)) return;

                string json = System.IO.File.ReadAllText(configPath);
                var doc = System.Text.Json.JsonDocument.Parse(json);
                var root = doc.RootElement;

                bool enabled = root.TryGetProperty("enabled", out var ep) && ep.GetBoolean();
                if (!enabled) return;

                int port = root.TryGetProperty("port", out var pp) ? pp.GetInt32() : 8234;
                string apiKey = root.TryGetProperty("apiKey", out var kp) ? kp.GetString() ?? "" : "";

                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    apiKey = RestApiService.GenerateApiKey();
                    // Write back the generated key
                    string updated = System.Text.Json.JsonSerializer.Serialize(new { enabled = true, port, apiKey });
                    System.IO.File.WriteAllText(configPath, updated);
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                        $"REST API key generated and saved to {configPath}");
                }

                Runtime.RestApi = new RestApiService(port, apiKey);
                Runtime.RestApi.Start();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                    $"Failed to start REST API: {ex.Message}");
            }
        }

        private void ConnectionsServiceOnConnectionsSaved(object sender, ConnectionsSavedEventArgs connectionsSavedEventArgs)
        {
            if (connectionsSavedEventArgs.UsingDatabase)
                return;

            FileBackupPruner.PruneBackupFiles(connectionsSavedEventArgs.ConnectionFileName, Properties.OptionsBackupPage.Default.BackupFileKeepCount);
        }

        private void SetMenuDependencies()
        {
            fileMenu.TreeWindow = AppWindows.TreeForm;

            viewMenu.TsExternalTools = _externalToolsToolStrip;
            viewMenu.TsQuickConnect = _quickConnectToolStrip;
            viewMenu.TsMultiSsh = _multiSshToolStrip;
            viewMenu.FullscreenHandler = Fullscreen;
            viewMenu.PresentationMode = PresentationMode;
            viewMenu.MainForm = this;

            toolsMenu.MainForm = this;
            toolsMenu.CredentialProviderCatalog = Runtime.CredentialProviderCatalog;

            // Wire quick-connect text to live-filter the connection tree (#1603)
            _quickConnectToolStrip.QuickConnectComboBox.TextChanged += OnQuickConnectTextChanged;
            _quickConnectToolStrip.QuickConnectComboBox.ConnectRequested += OnQuickConnectConnected;
        }

        private void OnQuickConnectTextChanged(object? sender, EventArgs e)
        {
            AppWindows.TreeForm?.FilterByQuickConnect(_quickConnectToolStrip.QuickConnectComboBox.Text.Trim());
        }

        private void OnQuickConnectConnected(object sender, QuickConnectComboBox.ConnectRequestedEventArgs e)
        {
            AppWindows.TreeForm?.FilterByQuickConnect(string.Empty);
        }

        //Theming support
        private void ApplyTheme()
        {
            if (!_themeManager.ThemingActive)
            {
                pnlDock.Theme = _themeManager.DefaultTheme.Theme;
                if (pnlDock.Theme?.Measures != null)
                {
                    pnlDock.Theme.Measures.SplitterSize = Properties.OptionsTabsPanelsPage.Default.SplitterSize;
                    pnlDock.Theme.Measures.DockPadding = Properties.OptionsTabsPanelsPage.Default.DockPadding;
                }
                return;
            }

            try
            {
                // this will always throw when turning themes on from
                // the options menu.
                pnlDock.Theme = _themeManager.ActiveTheme.Theme;
                if (pnlDock.Theme?.Measures != null)
                {
                    pnlDock.Theme.Measures.SplitterSize = Properties.OptionsTabsPanelsPage.Default.SplitterSize;
                    pnlDock.Theme.Measures.DockPadding = Properties.OptionsTabsPanelsPage.Default.DockPadding;
                }
            }
            catch (Exception)
            {
                _ = 0; // intentionally ignore exception
            }

            // Persist settings when rebuilding UI
            try
            {
                vsToolStripExtender.SetStyle(msMain, _themeManager.ActiveTheme.Version, _themeManager.ActiveTheme.Theme);
                vsToolStripExtender.SetStyle(_quickConnectToolStrip, _themeManager.ActiveTheme.Version, _themeManager.ActiveTheme.Theme);
                vsToolStripExtender.SetStyle(_externalToolsToolStrip, _themeManager.ActiveTheme.Version, _themeManager.ActiveTheme.Theme);
                vsToolStripExtender.SetStyle(_multiSshToolStrip, _themeManager.ActiveTheme.Version, _themeManager.ActiveTheme.Theme);

                if (!_themeManager.ActiveAndExtended) return;
                tsContainer.TopToolStripPanel.BackColor = _themeManager.ActiveTheme.ExtendedPalette?.getColor("CommandBarMenuDefault_Background") ?? BackColor;
                BackColor = _themeManager.ActiveTheme.ExtendedPalette?.getColor("Dialog_Background") ?? BackColor;
                ForeColor = _themeManager.ActiveTheme.ExtendedPalette?.getColor("Dialog_Foreground") ?? ForeColor;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Error applying theme", ex, MessageClass.WarningMsg);
            }
        }

        private async void FrmMain_Shown(object sender, EventArgs e)
        {
            // Bring the main window to the front after splash screen closes
            Activate();
            BringToFront();
            NativeMethods.SetForegroundWindow(Handle);

            PromptForUpdatesPreference();
            await CheckForUpdates();
        }

        private void PromptForUpdatesPreference()
        {
            if (!CommonRegistrySettings.AllowCheckForUpdates) return;
            if (!CommonRegistrySettings.AllowCheckForUpdatesAutomatical) return;

            if (Properties.OptionsUpdatesPage.Default.CheckForUpdatesAsked) return;

            // If the user has already explicitly disabled automatic updates via settings, don't ask again
            if (!Properties.OptionsUpdatesPage.Default.CheckForUpdatesOnStartup)
            {
                Properties.OptionsUpdatesPage.Default.CheckForUpdatesAsked = true;
                Properties.OptionsUpdatesPage.Default.Save();
                return;
            }

            string[] commandButtons =
            [
                Language.AskUpdatesCommandRecommended,
                Language.AskUpdatesCommandCustom,
                Language.AskUpdatesCommandAskLater
            ];

            CTaskDialog.ShowTaskDialogBox(this, GeneralAppInfo.ProductName, Language.AskUpdatesMainInstruction, string.Format(CultureInfo.CurrentCulture, Language.AskUpdatesContent, GeneralAppInfo.ProductName), "", "", "", "", string.Join(" | ", commandButtons), ETaskDialogButtons.None, ESysIcons.Question, ESysIcons.Question);

            if (CTaskDialog.CommandButtonResult == 0)
            {
                // Use Recommended Settings: enable automatic updates with the default frequency
                Properties.OptionsUpdatesPage.Default.CheckForUpdatesOnStartup = true;
                if (Properties.OptionsUpdatesPage.Default.CheckForUpdatesFrequencyDays < 1)
                    Properties.OptionsUpdatesPage.Default.CheckForUpdatesFrequencyDays = 14;
                Properties.OptionsUpdatesPage.Default.CheckForUpdatesAsked = true;
                Properties.OptionsUpdatesPage.Default.Save();
            }
            else if (CTaskDialog.CommandButtonResult == 1)
            {
                // Customize: let the user configure update settings manually, then open Options
                Properties.OptionsUpdatesPage.Default.CheckForUpdatesAsked = true;
                Properties.OptionsUpdatesPage.Default.Save();
                AppWindows.Show(WindowType.Options);
                if (AppWindows.OptionsFormWindow != null)
                    AppWindows.OptionsFormWindow.SetActivatedPage(Language.Updates);
            }
            // For "Ask Later" (button 2), CheckForUpdatesAsked remains false so the dialog will show again next startup
        }

        private async Task CheckForUpdates()
        {
            if (!CommonRegistrySettings.AllowCheckForUpdates) return;
            if (!CommonRegistrySettings.AllowCheckForUpdatesAutomatical) return;

            if (!Properties.OptionsUpdatesPage.Default.CheckForUpdatesOnStartup) return;
            if (Properties.OptionsUpdatesPage.Default.CheckForUpdatesFrequencyDays == 0) return;

            DateTime nextUpdateCheck = Convert.ToDateTime(Properties.OptionsUpdatesPage.Default.CheckForUpdatesLastCheck.Add(TimeSpan.FromDays(Convert.ToDouble(Properties.OptionsUpdatesPage.Default.CheckForUpdatesFrequencyDays))));

            if (!Properties.OptionsUpdatesPage.Default.UpdatePending && DateTime.UtcNow <= nextUpdateCheck) return;
            if (!IsHandleCreated)
                CreateHandle(); // Make sure the handle is created so that InvokeRequired returns the correct result

            await Startup.Instance.CheckForUpdate();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Properties.OptionsAppearancePage.Default.CloseToTray)
            {
                Runtime.NotificationAreaIcon ??= new NotificationAreaIcon();

                if (WindowState == FormWindowState.Normal || WindowState == FormWindowState.Maximized)
                {
                    Hide();
                    WindowState = FormWindowState.Minimized;
                    e.Cancel = true;
                    return;
                }
            }

            if (!(Runtime.WindowList == null || Runtime.WindowList.Count == 0))
            {
                int openConnections = GetOpenConnectionsCount();
                if (openConnections > 0 &&
                    (Settings.ConfirmCloseConnection == (int)ConfirmCloseEnum.All |
                     (Settings.ConfirmCloseConnection == (int)ConfirmCloseEnum.Multiple &
                      openConnections > 1) || Settings.ConfirmCloseConnection == (int)ConfirmCloseEnum.Exit))
                {
                    DialogResult result = CTaskDialog.MessageBox(this, Application.ProductName ?? string.Empty, Language.ConfirmExitMainInstruction, "", "", "", Language.CheckboxDoNotShowThisMessageAgain, ETaskDialogButtons.YesNo, ESysIcons.Question, ESysIcons.Question);
                    if (CTaskDialog.VerificationChecked)
                    {
                        Settings.ConfirmCloseConnection = (int)ConfirmCloseEnum.Never;
                    }

                    if (result == DialogResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }

            QuickConnectHistorySaver.CaptureOpenQuickConnectSessionsForShutdown(_quickConnectToolStrip.QuickConnectComboBox);

            // Save dock panel layout while ConnectionWindows are still docked.
            // Must happen before window.Close() which removes them from pnlDock.
            SettingsSaver.SaveDockPanelLayout();

            if (Runtime.WindowList != null)
            {
                BaseWindow[] windowsToClose = Runtime.WindowList.Cast<BaseWindow>().ToArray();
                foreach (BaseWindow window in windowsToClose)
                {
                    if (window == null || window.IsDisposed)
                        continue;

                    window.Close();
                }

                // If a child window/panel close is cancelled (for example user clicks "No"),
                // keep main app visible and abort this close request.
                if (GetOpenConnectionsCount() > 0)
                {
                    e.Cancel = true;
                    return;
                }
            }

            IsClosing = true;
            _autoLockTimer.Stop();

            NativeMethods.UnregisterHotKey(Handle, HOTKEY_ID_ACTIVATE);

            Hide();

            NativeMethods.RemoveClipboardFormatListener(Handle);
            Shutdown.Cleanup(_quickConnectToolStrip, _externalToolsToolStrip, _multiSshToolStrip, msMain, this);

            Shutdown.StartUpdate();

            Debug.Print("[END] - " + Convert.ToString(DateTime.Now, CultureInfo.InvariantCulture));
        }

        private void OnDisplaySettingsChanged(object sender, EventArgs e)
        {
            // Notify all active connections that display settings changed (monitor connect/disconnect)
            // so they can re-evaluate their resolution (fixes #2142)
            if (pnlDock.Contents.Count == 0) return;

            foreach (IDockContent dc in pnlDock.Contents)
            {
                if (dc is not ConnectionWindow cw) continue;
                if (cw.Controls.Count < 1) continue;
                if (cw.Controls[0] is not DockPanel dp) continue;

                foreach (IDockContent tab in dp.Contents)
                {
                    if (tab is not UI.Tabs.ConnectionTab ct) continue;
                    InterfaceControl? ifc = InterfaceControl.FindInterfaceControl(ct);
                    ifc?.Protocol?.OnDisplaySettingsChanged();
                }
            }
        }

        private void OnPowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            if (e.Mode != PowerModes.Resume) return;
            if (pnlDock.Contents.Count == 0) return;

            foreach (IDockContent dc in pnlDock.Contents)
            {
                if (dc is not ConnectionWindow cw) continue;
                if (cw.Controls.Count < 1) continue;
                if (cw.Controls[0] is not DockPanel dp) continue;

                foreach (IDockContent tab in dp.Contents)
                {
                    if (tab is not UI.Tabs.ConnectionTab ct) continue;
                    InterfaceControl? ifc = InterfaceControl.FindInterfaceControl(ct);
                    ifc?.Protocol?.OnPowerModeChanged(e.Mode);
                }
            }
        }

        private int GetOpenConnectionsCount()
        {
            int openConnections = 0;
            if (pnlDock.Contents.Count == 0)
                return openConnections;

            foreach (IDockContent dc in pnlDock.Contents)
            {
                if (dc is not ConnectionWindow cw) continue;
                if (cw.Controls.Count < 1) continue;
                if (cw.Controls[0] is not DockPanel dp) continue;
                if (dp.Contents.Count > 0)
                    openConnections += dp.Contents.Count;
            }

            return openConnections;
        }

        #endregion

        #region Timer

        private void TmrAutoSave_Tick(object sender, EventArgs e)
        {
            Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, "Doing AutoSave");
            Runtime.ConnectionsService.SaveConnectionsAsync();
        }

        private void AutoLockTimer_Tick(object sender, EventArgs e)
        {
            if (_isAutoLocked || IsClosing || !AutoLockEnabled())
                return;

            int idleMilliseconds = NativeMethods.GetIdleMilliseconds();
            if (idleMilliseconds < AutoLockIdleThresholdMs)
                return;

            EngageAutoLock("idle-timeout");
        }

        private static bool AutoLockEnabled()
        {
            RootNodeInfo? rootNodeInfo = GetConnectionRootNodeInfo();
            return rootNodeInfo is { Password: true, AutoLockOnMinimize: true };
        }

        private static RootNodeInfo? GetConnectionRootNodeInfo()
        {
            return Runtime.ConnectionsService.ConnectionTreeModel?.RootNodes
                ?.OfType<RootNodeInfo>()
                .FirstOrDefault(node => node.Type == RootNodeType.Connection);
        }

        private void EngageAutoLock(string reason)
        {
            if (_isAutoLocked || !AutoLockEnabled())
                return;

            _isAutoLocked = true;
            Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, $"Autolock engaged ({reason}).");

            if (WindowState != FormWindowState.Minimized)
            {
                PreviousWindowState = WindowState;
                WindowState = FormWindowState.Minimized;
            }

            if (!Properties.OptionsAppearancePage.Default.MinimizeToTray)
                return;

            Runtime.NotificationAreaIcon ??= new NotificationAreaIcon();
            Hide();
            ShowInTaskbar = false;
        }

        internal bool TryUnlockIfNeeded()
        {
            if (!_isAutoLocked || IsClosing)
                return true;

            RootNodeInfo? rootNodeInfo = GetConnectionRootNodeInfo();
            if (rootNodeInfo?.Password != true)
            {
                _isAutoLocked = false;
                return true;
            }

            if (_unlockPromptInProgress)
                return false;

            _unlockPromptInProgress = true;
            try
            {
                string passwordName = Properties.OptionsDBsPage.Default.UseSQLServer
                    ? Language.SQLServer.TrimEnd(':')
                    : Path.GetFileName(ConnectionsService.GetStartupConnectionFileName());

                Optional<System.Security.SecureString> password = MiscTools.PasswordDialog(passwordName, false);
                if (!password.Any() || password.First().Length == 0)
                    return false;

                bool matches = rootNodeInfo.IsPasswordMatch(password.First());
                if (matches)
                {
                    _isAutoLocked = false;
                    return true;
                }

                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                    "Autolock unlock request rejected: provided password did not match.");
                return false;
            }
            finally
            {
                _unlockPromptInProgress = false;
            }
        }

        #endregion

        #region Window Overrides and DockPanel Stuff

        private void FrmMain_ResizeBegin(object sender, EventArgs e)
        {
            _inSizeMove = true;
        }

        private void FrmMain_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                EngageAutoLock("minimized");

                if (!Properties.OptionsAppearancePage.Default.MinimizeToTray) return;
                Runtime.NotificationAreaIcon ??= new NotificationAreaIcon();

                Hide();
            }
            else
            {
                if (!TryUnlockIfNeeded())
                {
                    WindowState = FormWindowState.Minimized;

                    if (Properties.OptionsAppearancePage.Default.MinimizeToTray)
                    {
                        Runtime.NotificationAreaIcon ??= new NotificationAreaIcon();
                        Hide();
                        ShowInTaskbar = false;
                    }

                    return;
                }

                PreviousWindowState = WindowState;
            }
        }

        private void FrmMain_ResizeEnd(object sender, EventArgs e)
        {
            _inSizeMove = false;
            // This handles activations from clicks that started a size/move operation
            ActivateConnection();
        }

        public bool PreFilterMessage(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == NativeMethods.WM_MOUSEWHEEL)
            {
                IntPtr hWnd = NativeMethods.WindowFromPoint(MousePosition);
                InterfaceControl? ic = FindInterfaceControl(hWnd);

                if (ic?.Protocol is PuttyBase pb && pb.PuttyHandle != IntPtr.Zero)
                {
                    NativeMethods.SendMessage(pb.PuttyHandle, m.Msg, m.WParam, m.LParam);
                    return true;
                }

                // For RDP, VNC, and other protocols: redirect scroll to the window under the
                // mouse cursor rather than letting Windows route it to the focused window
                // (which may be the config panel). Fix for #633.
                if (ic != null && hWnd != IntPtr.Zero)
                {
                    NativeMethods.SendMessage(hWnd, m.Msg, m.WParam, m.LParam);
                    return true;
                }
            }

            return false;
        }

        private InterfaceControl? FindInterfaceControl(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero) return null;

            IntPtr current = hWnd;
            while (current != IntPtr.Zero && current != Handle)
            {
                Control? c = Control.FromHandle(current);
                if (c != null)
                {
                    // We found a managed control. Walk up the managed hierarchy.
                    while (c != null)
                    {
                        if (c is InterfaceControl ic) return ic;
                        c = c.Parent;
                    }
                    return null; // Hit top of managed hierarchy without finding InterfaceControl
                }

                // Still in unmanaged land (or external process window), go up one level
                current = NativeMethods.GetParent(current);
            }
            return null;
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            // Listen for and handle operating system messages
            try
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (m.Msg)
                {
                    case NativeMethods.WM_COPYDATA:
                        if (m.GetLParam(typeof(NativeMethods.COPYDATASTRUCT)) is NativeMethods.COPYDATASTRUCT cds)
                        {
                            if ((int)cds.dwData == 1)
                            {
                                string? message = Marshal.PtrToStringUni(cds.lpData);
                                HandleStartupArgs(message);
                            }
                            else if ((int)cds.dwData == 2)
                            {
                                // Activate signal from a second instance: bring this window to front.
                                // The running instance setting its own foreground is more reliable than
                                // SetForegroundWindow called from another process (#398).
                                if (WindowState == FormWindowState.Minimized)
                                    WindowState = PreviousWindowState == FormWindowState.Minimized
                                        ? FormWindowState.Normal
                                        : PreviousWindowState;
                                Activate();
                                BringToFront();
                                NativeMethods.SetForegroundWindow(Handle);
                            }
                        }
                        break;
                    case NativeMethods.WM_MOUSEACTIVATE:
                        _inMouseActivate = true;
                        break;
                    case NativeMethods.WM_ACTIVATEAPP:
                        bool appActivated = m.WParam != IntPtr.Zero;
                        bool appReactivated = appActivated && !_isApplicationActivated;
                        _isApplicationActivated = appActivated;

                        if (!appActivated)
                        {
                            _pendingActivateConnectionOnAppReactivation = false;
                        }
                        else if (appReactivated)
                        {
                            _pendingActivateConnectionOnAppReactivation = true;
                            Control? candidateTabToFocus = FromChildHandle(NativeMethods.WindowFromPoint(MousePosition))
                                                   ?? GetChildAtPoint(MousePosition);
                            if (candidateTabToFocus is InterfaceControl)
                            {
                                candidateTabToFocus.Parent?.Focus();
                            }

                            // When returning via Alt+Tab, ensure the active connection regains keyboard focus.
                            if (!Properties.OptionsStartupExitPage.Default.DisableRefocus &&
                                WindowState != FormWindowState.Minimized)
                            {
                                QueueActivateConnection();
                            }
                        }

                        _inMouseActivate = false;
                        break;
                    case NativeMethods.WM_ACTIVATE:
                        // Only handle this msg if it was triggered by a click
                        if (NativeMethods.LOWORD(m.WParam) == NativeMethods.WA_CLICKACTIVE)
                        {
                            Control? controlThatWasClicked = FromChildHandle(NativeMethods.WindowFromPoint(MousePosition))
                                                     ?? GetChildAtPoint(MousePosition);
                            if (controlThatWasClicked != null)
                            {
                                if (controlThatWasClicked is TreeView ||
                                    controlThatWasClicked is ComboBox ||
                                    controlThatWasClicked is MrngTextBox ||
                                    controlThatWasClicked is FrmMain)
                                {
                                    controlThatWasClicked.Focus();
                                }
                                else if (controlThatWasClicked.CanSelect ||
                                         controlThatWasClicked is MenuStrip ||
                                         controlThatWasClicked is ToolStrip)
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
                                else if (controlThatWasClicked.GetType().Namespace?.Contains("WinFormsUI", StringComparison.Ordinal) == true)
                                {
                                    // DockPanel infrastructure controls (splitters, pane dividers) handle their
                                    // own mouse drag. Calling ActivateConnection() here would refocus the
                                    // embedded connection (e.g. PuTTY via SetForegroundWindow) and break the
                                    // panel resize drag operation. (#2179)
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
                        if (!_isApplicationActivated || !_pendingActivateConnectionOnAppReactivation)
                            break;

                        if (WindowState == FormWindowState.Minimized)
                            break;

                        // Ignore this message if the window wasn't activated
                        var windowPos = Marshal.PtrToStructure<NativeMethods.WINDOWPOS>(m.LParam);
                        if ((windowPos.flags & NativeMethods.SWP_NOACTIVATE) == 0)
                        {
                            _pendingActivateConnectionOnAppReactivation = false;
                            if (!_inMouseActivate && !_inSizeMove)
                                ActivateConnection();
                        }
                        break;
                    case NativeMethods.WM_SYSCOMMAND:
                        if (m.WParam == new IntPtr(0))
                            ShowHideMenu();
                        Screen? screen = _advancedWindowMenu.GetScreenById(m.WParam.ToInt32());
                        if (screen != null)
                        {
                            Screens.SendFormToScreen(screen);
                            Console.WriteLine(screen.ToString());
                        }
                        // Block restore/maximize while session is locked (#1666):
                        // Prompt for password BEFORE base.WndProc makes the window visible.
                        if (_isAutoLocked)
                        {
                            int syscmd = m.WParam.ToInt32() & 0xFFF0;
                            if (syscmd == NativeMethods.SC_RESTORE || syscmd == NativeMethods.SC_MAXIMIZE)
                            {
                                if (!TryUnlockIfNeeded())
                                    return; // suppress the restore/maximize — window stays minimized
                            }
                        }
                        break;
                    case NativeMethods.WM_DPICHANGED:
                        {
                            // Fix #1174: Do not manually set Bounds if maximized, as this can cause
                            // the window to enter an invalid state or render incorrectly.
                            // The OS and WinForms (PerMonitorV2) handle maximized scaling.
                            if (WindowState != FormWindowState.Maximized)
                            {
                                Rect32 newRect = Marshal.PtrToStructure<Rect32>(m.LParam);
                                Bounds = new Rectangle(newRect.left, newRect.top, newRect.right - newRect.left, newRect.bottom - newRect.top);
                            }

                            // Force layout refresh for DockPanel to fix missing tabs/config
                            pnlDock.PerformLayout();
                            pnlDock.Refresh();
                        }
                        break;
                    case NativeMethods.WM_HOTKEY:
                        if (m.WParam.ToInt32() == HOTKEY_ID_ACTIVATE)
                        {
                            if (WindowState == FormWindowState.Minimized)
                            {
                                Show();
                                ShowInTaskbar = true;
                                WindowState = PreviousWindowState == FormWindowState.Maximized
                                    ? FormWindowState.Maximized
                                    : FormWindowState.Normal;
                            }

                            Activate();
                            BringToFront();
                            NativeMethods.SetForegroundWindow(Handle);
                        }
                        break;
                    case NativeMethods.WM_CLIPBOARDUPDATE:
                        _clipboardChangedEvent?.Invoke();
                        break;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("frmMain WndProc failed", ex);
            }

            base.WndProc(ref m);
        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            if (keyData == (Keys.Alt | Keys.Menu))
            {
                if (!msMain.Visible)
                {
                    msMain.Visible = true;
                }
            }
            else if (keyData == (Keys.Shift | Keys.F11))
            {
                // Ensure PresentationMode is initialized (it might be null if called too early, though unlikely for this shortcut)
                if (PresentationMode != null)
                {
                    PresentationMode.Active = !PresentationMode.Active;
                    return true;
                }
            }
            else if (keyData == (Keys.Control | Keys.F))
            {
                if (pnlDock.ActiveDocument is ConnectionWindow connectionWindow)
                {
                    connectionWindow.FindInSession();
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void HandleStartupArgs(string? argsMessage)
        {
            if (string.IsNullOrEmpty(argsMessage))
                return;

            string[] args = argsMessage.Split('\n');

            CommandLineParser commandLineParser = new(args);
            commandLineParser.ApplySwitches(applyLogPathToActiveLogger: true);

            StartupArgumentsInterpreter.ResetConnectionArgs();
            StartupArgumentsInterpreter interpreter = new(Runtime.MessageCollector);
            interpreter.ParseArguments(args);

            RootNodeInfo? root = Runtime.ConnectionsService.ConnectionTreeModel?.RootNodes
                .OfType<RootNodeInfo>()
                .FirstOrDefault();

            if (root == null)
                return;

            if (!string.IsNullOrEmpty(StartupArgumentsInterpreter.ConnectTo))
            {
                new CommandLineConnectionOpener(Runtime.ConnectionInitiator, StartupArgumentsInterpreter.ConnectTo, "--connect").Execute(root);
            }

            if (!string.IsNullOrEmpty(StartupArgumentsInterpreter.StartupConnectTo))
            {
                new CommandLineConnectionOpener(Runtime.ConnectionInitiator, StartupArgumentsInterpreter.StartupConnectTo, "--startup").Execute(root);
            }

            if (!string.IsNullOrEmpty(StartupArgumentsInterpreter.QuickConnectTo))
            {
                string protocol = StartupArgumentsInterpreter.QuickConnectProtocol
                    ?? Properties.Settings.Default.QuickConnectProtocol;
                ConnectionInfo? connectionInfo = ConnectionsService.CreateQuickConnect(
                    StartupArgumentsInterpreter.QuickConnectTo, Converter.StringToProtocol(protocol));
                if (connectionInfo != null)
                    Runtime.ConnectionInitiator.OpenConnection(connectionInfo, ConnectionInfo.Force.DoNotJump);
            }

            // Bring window to front
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = PreviousWindowState == FormWindowState.Minimized ? FormWindowState.Normal : PreviousWindowState;
            }
            Activate();
        }

        private static void SimulateClick(Control control)
        {
            Point clientMousePosition = control.PointToClient(MousePosition);
            int temp_wLow = clientMousePosition.X;
            int temp_wHigh = clientMousePosition.Y;
            NativeMethods.SendMessage(control.Handle, NativeMethods.WM_LBUTTONDOWN, (IntPtr)NativeMethods.MK_LBUTTON,
                                      (IntPtr)NativeMethods.MAKELPARAM(ref temp_wLow, ref temp_wHigh));
            clientMousePosition.X = temp_wLow;
            clientMousePosition.Y = temp_wHigh;
        }

        private void QueueActivateConnection()
        {
            if (IsDisposed || Disposing || !IsHandleCreated)
                return;

            try
            {
                BeginInvoke((MethodInvoker)ActivateConnection);
            }
            catch (ObjectDisposedException)
            {
                _ = 0; // Intentionally empty — control may be disposed
            }
            catch (InvalidOperationException)
            {
                _ = 0; // Intentionally empty — control may be disposed
            }
        }

        private static ConnectionTab? GetActiveConnectionTab(ConnectionWindow connectionWindow)
        {
            if (connectionWindow == null)
                return null;

            if (connectionWindow.ActiveControl is DockPane activePane &&
                activePane.ActiveContent is ConnectionTab activePaneTab)
            {
                return activePaneTab;
            }

            foreach (Control control in connectionWindow.Controls)
            {
                if (control is not DockPanel dockPanel)
                    continue;

                if (dockPanel.ActiveContent is ConnectionTab activeDockTab)
                    return activeDockTab;

                foreach (IDockContent document in dockPanel.DocumentsToArray())
                {
                    if (document is ConnectionTab activatedDocument &&
                        activatedDocument.DockHandler.IsActivated)
                    {
                        return activatedDocument;
                    }
                }
            }

            return null;
        }

        private static ConnectionInfo? GetConnectionInfoForTab(ConnectionTab? connectionTab)
        {
            if (connectionTab == null)
                return null;

            if (connectionTab.Tag is InterfaceControl interfaceControl)
                return interfaceControl.Info;

            if (connectionTab.Tag is ConnectionInfo connectionInfo)
                return connectionInfo;

            return connectionTab.TrackedConnectionInfo;
        }

        private void UpdateSelectedConnectionFromActiveDocument()
        {
            if (pnlDock.ActiveDocument is not ConnectionWindow connectionWindow)
                return;

            ConnectionTab? activeConnectionTab = GetActiveConnectionTab(connectionWindow);
            ConnectionInfo? activeConnectionInfo = GetConnectionInfoForTab(activeConnectionTab);
            if (activeConnectionInfo != null)
                SelectedConnection = activeConnectionInfo;
        }

        private void ActivateConnection()
        {
            ConnectionWindow? cw = pnlDock.ActiveDocument as ConnectionWindow;
            if (cw == null) return;
            ConnectionTab? tab = GetActiveConnectionTab(cw);
            if (tab == null) return;
            InterfaceControl? ifc = InterfaceControl.FindInterfaceControl(tab);
            if (ifc == null) return;

            if (ifc.Protocol is PuttyBase puttyProtocol)
                puttyProtocol.RequestPostOpenLayoutResizePass();

            ifc.Protocol?.Focus();
            Form? conFormWindow = ifc.FindForm();
            (conFormWindow as ConnectionTab)?.RefreshInterfaceController();
        }

        private void PnlDock_ActiveDocumentChanged(object sender, EventArgs e)
        {
            UpdateSelectedConnectionFromActiveDocument();
            // Do NOT call ActivateConnection() here — it steals focus from the
            // connection tree (and any other non-protocol control) by calling
            // Protocol.Focus() every time the active document changes. Focus
            // should only be given to protocols on explicit user action (click
            // on the protocol area, Alt+Tab back, etc.), not on tab switches
            // triggered by tree selection or other UI navigation.
            sessionsMenu.UpdateMenuState();
        }

        internal void UpdateWindowTitle()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(UpdateWindowTitle));
                return;
            }

            StringBuilder titleBuilder = new(Application.ProductName);
            const string separator = " - ";

            if (Runtime.ConnectionsService.IsConnectionsFileLoaded)
            {
                if (Runtime.ConnectionsService.UsingDatabase)
                {
                    titleBuilder.Append(separator);
                    titleBuilder.Append(Language.SQLServer.TrimEnd(':'));
                }
                else
                {
                    if (!string.IsNullOrEmpty(Runtime.ConnectionsService.ConnectionFileName))
                    {
                        titleBuilder.Append(separator);
                        titleBuilder.Append(Properties.OptionsAppearancePage.Default.ShowCompleteConsPathInTitle ? Runtime.ConnectionsService.ConnectionFileName : Path.GetFileName(Runtime.ConnectionsService.ConnectionFileName));
                    }
                }
            }

            if (!string.IsNullOrEmpty(SelectedConnection?.Name))
            {
                titleBuilder.Append(separator);
                titleBuilder.Append(SelectedConnection!.Name);

                if (Settings.TrackActiveConnectionInConnectionTree)
                    AppWindows.TreeForm?.JumpToNode(SelectedConnection, suppressPreview: true);
            }

            Text = titleBuilder.ToString();
        }

        public void ShowHidePanelTabs(DockContent? closingDocument = null)
        {
            DocumentStyle newDocumentStyle;

            if (Properties.OptionsTabsPanelsPage.Default.AlwaysShowPanelTabs)
            {
                newDocumentStyle = DocumentStyle.DockingWindow; // Show the panel tabs
            }
            else
            {
                int nonConnectionPanelCount = 0;
                foreach (IDockContent dockContent in pnlDock.Documents)
                {
                    DockContent document = (DockContent)dockContent;
                    if ((closingDocument == null || document != closingDocument) && document is not ConnectionWindow)
                    {
                        nonConnectionPanelCount++;
                    }
                }

                newDocumentStyle = nonConnectionPanelCount == 0
                    ? DocumentStyle.DockingSdi
                    : DocumentStyle.DockingWindow;
            }

            if (pnlDock.DocumentStyle == newDocumentStyle) return;
            pnlDock.DocumentStyle = newDocumentStyle;
            pnlDock.Size = new Size(1, 1);
        }

        public static void ShowHideConnectionTabs()
        {
            if (Runtime.WindowList == null) return;

            foreach (var window in Runtime.WindowList.OfType<ConnectionWindow>())
            {
                if (!window.IsDisposed)
                    window.ShowHideConnectionTabs();
            }
        }

        public void SetPanelLock()
        {
            if (pnlDock.Contents.Count == 0) return;

            var lockPanels = !Properties.OptionsTabsPanelsPage.Default.LockPanels;
            foreach (IDockContent dc in pnlDock.Contents)
            {
                if (dc.DockHandler != null)
                {
                    dc.DockHandler.AllowEndUserDocking = lockPanels;
                }
            }
        }

        public void SetDefaultLayout()
        {
            pnlDock.Visible = false;

            AppWindows.TreeForm?.Show(pnlDock, DockState.DockLeft);
            // Show ConfigForm in its own pane below the Connections pane (#725)
            if (AppWindows.TreeForm?.DockHandler.Pane != null)
                AppWindows.ConfigForm.Show(AppWindows.TreeForm.DockHandler.Pane, DockAlignment.Bottom, 0.35);
            else
                AppWindows.ConfigForm.Show(pnlDock, DockState.DockLeft);
            AppWindows.ErrorsForm.Show(pnlDock, DockState.DockBottomAutoHide);
            _screenshotsBottomTab.Show(pnlDock, DockState.DockBottomAutoHide);
            viewMenu._mMenViewErrorsAndInfos.Checked = true;

            ShowFileMenu();

            pnlDock.Visible = true;
        }

        public void ShowFileMenu()
        {
            msMain.Visible = true;
            viewMenu._mMenViewFileMenu.Checked = true;
        }

        public void HideFileMenu()
        {
            msMain.Visible = false;
            viewMenu._mMenViewFileMenu.Checked = false;
            MessageBox.Show(Language.FileMenuWillBeHiddenNow, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void SetLayout()
        {
            pnlDock.Visible = false;

            if (Settings.ViewMenuMessages == true)
            {
                AppWindows.ErrorsForm.Show(pnlDock, DockState.DockBottomAutoHide);
                _screenshotsBottomTab.Show(pnlDock, DockState.DockBottomAutoHide);
                viewMenu._mMenViewErrorsAndInfos.Checked = true;
            }
            else
            {
                _screenshotsBottomTab.Hide();
                viewMenu._mMenViewErrorsAndInfos.Checked = false;
            }


            if (Settings.ViewMenuExternalTools == true)
            {
                if (viewMenu.TsExternalTools is not null) viewMenu.TsExternalTools.Visible = true;
                viewMenu._mMenViewExtAppsToolbar.Checked = true;
            }
            else
            {
                if (viewMenu.TsExternalTools is not null) viewMenu.TsExternalTools.Visible = false;
                viewMenu._mMenViewExtAppsToolbar.Checked = false;
            }

            if (Settings.ViewMenuMultiSSH == true)
            {
                if (viewMenu.TsMultiSsh is not null) viewMenu.TsMultiSsh.Visible = true;
                viewMenu._mMenViewMultiSshToolbar.Checked = true;
            }
            else
            {
                if (viewMenu.TsMultiSsh is not null) viewMenu.TsMultiSsh.Visible = false;
                viewMenu._mMenViewMultiSshToolbar.Checked = false;
            }

            if (Settings.QuickyTBVisible)
            {
                if (viewMenu.TsQuickConnect is not null) viewMenu.TsQuickConnect.Visible = true;
                viewMenu._mMenViewQuickConnectToolbar.Checked = true;
            }
            else
            {
                if (viewMenu.TsQuickConnect is not null) viewMenu.TsQuickConnect.Visible = false;
                viewMenu._mMenViewQuickConnectToolbar.Checked = false;
            }

            if (Settings.LockToolbars == true)
            {
                Settings.LockToolbars = true;
                viewMenu._mMenViewLockToolbars.Checked = true;
            }
            else
            {
                Settings.LockToolbars = false;
                viewMenu._mMenViewLockToolbars.Checked = false;
            }

            pnlDock.Visible = true;
        }

        public void ShowHideMenu() => tsContainer.TopToolStripPanelVisible = !tsContainer.TopToolStripPanelVisible;

        #endregion

        #region Events

        public delegate void ClipboardchangeEventHandler();

        public static event ClipboardchangeEventHandler ClipboardChanged
        {
            add =>
                _clipboardChangedEvent =
                    (ClipboardchangeEventHandler)Delegate.Combine(_clipboardChangedEvent, value);
            remove =>
                _clipboardChangedEvent =
                    (ClipboardchangeEventHandler?)Delegate.Remove(_clipboardChangedEvent, value);
        }

        public event EventHandler? UserInterfaceResize;

        #endregion

        private void ViewMenu_Opening(object sender, EventArgs e)
        {
            viewMenu.mMenView_DropDownOpening(sender, e);
        }

        private void TsModeUser_Click(object sender, EventArgs e)
        {
            Properties.OptionsRbac.Default.ActiveRole = "UserRole";
        }

        private void TsModeAdmin_Click(object sender, EventArgs e)
        {
            Properties.OptionsRbac.Default.ActiveRole = "AdminRole";
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Rect32
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
    }
}
