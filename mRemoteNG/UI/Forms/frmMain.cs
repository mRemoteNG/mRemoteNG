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
using mRemoteNG.Messages;
using mRemoteNG.Messages.MessageWriters;
using mRemoteNG.Themes;
using mRemoteNG.Tools;
using mRemoteNG.UI.Menu;
using mRemoteNG.UI.Tabs;
using mRemoteNG.UI.TaskDialog;
using mRemoteNG.UI.Window;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using mRemoteNG.UI.Panels;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.UI.Controls;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;
#endregion

// ReSharper disable MemberCanBePrivate.Global

namespace mRemoteNG.UI.Forms
{
    [SupportedOSPlatform("windows")]
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
        private readonly AdvancedWindowMenu _advancedWindowMenu;
        private ConnectionInfo _selectedConnection;
        private readonly IList<IMessageWriter> _messageWriters = new List<IMessageWriter>();
        private readonly ThemeManager _themeManager;
        private readonly FileBackupPruner _backupPruner = new FileBackupPruner();
        public static FrmOptions OptionsForm;

        internal FullscreenHandler Fullscreen { get; set; }

        //Added theming support
        private readonly ToolStripRenderer _toolStripProfessionalRenderer = new ToolStripProfessionalRenderer();

        private FrmMain()
        {
            _showFullPathInTitle = Properties.OptionsAppearancePage.Default.ShowCompleteConsPathInTitle;
            InitializeComponent();

            Screen targetScreen = (Screen.AllScreens.Length > 1) ? Screen.AllScreens[1] : Screen.AllScreens[0];

            Rectangle viewport = targetScreen.WorkingArea;
            
            // normally it should be screens[1] however due DPI apply 1 size "same" as default with 100%
            this.Left = viewport.Left + (targetScreen.Bounds.Size.Width / 2) - (this.Width / 2);
            this.Top = viewport.Top + (targetScreen.Bounds.Size.Height / 2) - (this.Height / 2);

            Fullscreen = new FullscreenHandler(this);

            //Theming support
            _themeManager = ThemeManager.getInstance();
            vsToolStripExtender.DefaultRenderer = _toolStripProfessionalRenderer;
            ApplyTheme();

            _advancedWindowMenu = new AdvancedWindowMenu(this);
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

        private void FrmMain_Load(object sender, EventArgs e)
        {
            var messageCollector = Runtime.MessageCollector;

            var settingsLoader = new SettingsLoader(this, messageCollector, _quickConnectToolStrip, _externalToolsToolStrip, _multiSshToolStrip, msMain);
            settingsLoader.LoadSettings();

            MessageCollectorSetup.SetupMessageCollector(messageCollector, _messageWriters);
            MessageCollectorSetup.BuildMessageWritersFromSettings(_messageWriters);

            Startup.Instance.InitializeProgram(messageCollector);

            SetMenuDependencies();

            var uiLoader = new DockPanelLayoutLoader(this, messageCollector);
            uiLoader.LoadPanelsFromXml();

            LockToolbarPositions(Properties.Settings.Default.LockToolbars);
            Properties.Settings.Default.PropertyChanged += OnApplicationSettingChanged;

            _themeManager.ThemeChanged += ApplyTheme;

            _fpChainedWindowHandle = NativeMethods.SetClipboardViewer(Handle);

            Runtime.WindowList = new WindowList();

            if (Properties.App.Default.ResetPanels)
                SetDefaultLayout();
            else
                SetLayout();

            Runtime.ConnectionsService.ConnectionsLoaded += ConnectionsServiceOnConnectionsLoaded;
            Runtime.ConnectionsService.ConnectionsSaved += ConnectionsServiceOnConnectionsSaved;
            var credsAndConsSetup = new CredsAndConsSetup();
            credsAndConsSetup.LoadCredsAndCons();

            Windows.TreeForm.Focus();

            PuttySessionsManager.Instance.StartWatcher();

            Startup.Instance.CreateConnectionsProvider(messageCollector);

            _advancedWindowMenu.BuildAdditionalMenuItems();
            SystemEvents.DisplaySettingsChanged += _advancedWindowMenu.OnDisplayChanged;
            ApplyLanguage();

            Opacity = 1;
            //Fix MagicRemove , revision on panel strategy for mdi

            pnlDock.ShowDocumentIcon = true;

            FrmSplashScreenNew.GetInstance().Close();

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
            
            if (!Properties.OptionsTabsPanelsPage.Default.CreateEmptyPanelOnStartUp) return;
            var panelName = !string.IsNullOrEmpty(Properties.OptionsTabsPanelsPage.Default.StartUpPanelName) ? Properties.OptionsTabsPanelsPage.Default.StartUpPanelName : Language.NewPanel;

            var panelAdder = new PanelAdder();
            if (!panelAdder.DoesPanelExist(panelName))
                panelAdder.AddPanel(panelName);
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
            switch (propertyChangedEventArgs.PropertyName)
            {
                case nameof(Properties.Settings.LockToolbars):
                    LockToolbarPositions(Properties.Settings.Default.LockToolbars);
                    break;
                case nameof(Properties.Settings.ViewMenuExternalTools):
                    LockToolbarPositions(Properties.Settings.Default.LockToolbars);
                    break;
                case nameof(Properties.Settings.ViewMenuMessages):
                    LockToolbarPositions(Properties.Settings.Default.LockToolbars);
                    break;
                case nameof(Properties.Settings.ViewMenuMultiSSH):
                    LockToolbarPositions(Properties.Settings.Default.LockToolbars);
                    break;
                case nameof(Properties.Settings.ViewMenuQuickConnect):
                    LockToolbarPositions(Properties.Settings.Default.LockToolbars);
                    break;
                default:
                    return;
            }
        }

        private void LockToolbarPositions(bool shouldBeLocked)
        {
            var toolbars = new ToolStrip[] {_quickConnectToolStrip, _multiSshToolStrip, _externalToolsToolStrip, msMain};
            foreach (var toolbar in toolbars)
            {
                toolbar.GripStyle = shouldBeLocked ? ToolStripGripStyle.Hidden : ToolStripGripStyle.Visible;
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

            _backupPruner.PruneBackupFiles(connectionsSavedEventArgs.ConnectionFileName, Properties.OptionsBackupPage.Default.BackupFileKeepCount);
        }

        private void SetMenuDependencies()
        {
            fileMenu.TreeWindow = Windows.TreeForm;

            viewMenu.TsExternalTools = _externalToolsToolStrip;
            viewMenu.TsQuickConnect = _quickConnectToolStrip;
            viewMenu.TsMultiSsh = _multiSshToolStrip;
            viewMenu.FullscreenHandler = Fullscreen;
            viewMenu.MainForm = this;

            toolsMenu.MainForm = this;
            toolsMenu.CredentialProviderCatalog = Runtime.CredentialProviderCatalog;
        }

        //Theming support
        private void ApplyTheme()
        {
            if (!_themeManager.ThemingActive)
            {
                pnlDock.Theme = _themeManager.DefaultTheme.Theme;
                return;
            }

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

                if (!_themeManager.ActiveAndExtended) return;
                tsContainer.TopToolStripPanel.BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("CommandBarMenuDefault_Background");
                BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
                ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Error applying theme", ex, MessageClass.WarningMsg);
            }
        }

        private async void frmMain_Shown(object sender, EventArgs e)
        {
            PromptForUpdatesPreference();
            await CheckForUpdates();
        }

        private void PromptForUpdatesPreference()
        {
            if (Properties.OptionsUpdatesPage.Default.CheckForUpdatesAsked) return;
            string[] commandButtons =
            {
                Language.AskUpdatesCommandRecommended,
                Language.AskUpdatesCommandCustom,
                Language.AskUpdatesCommandAskLater
            };

            CTaskDialog.ShowTaskDialogBox(this, GeneralAppInfo.ProductName, Language.AskUpdatesMainInstruction, string.Format(Language.AskUpdatesContent, GeneralAppInfo.ProductName), "", "", "", "", string.Join(" | ", commandButtons), ETaskDialogButtons.None, ESysIcons.Question, ESysIcons.Question);

            if (CTaskDialog.CommandButtonResult == 0 | CTaskDialog.CommandButtonResult == 1)
            {
                Properties.OptionsUpdatesPage.Default.CheckForUpdatesAsked = true;
            }

            if (CTaskDialog.CommandButtonResult != 1) return;

            OptionsForm.SetActivatedPage(Language.Updates);
            OptionsForm.ShowDialog(this);
        }

        private async Task CheckForUpdates()
        {
            if (!Properties.OptionsUpdatesPage.Default.CheckForUpdatesOnStartup) return;

            var nextUpdateCheck =
                Convert.ToDateTime(Properties.OptionsUpdatesPage.Default.CheckForUpdatesLastCheck.Add(TimeSpan.FromDays(Convert.ToDouble(Properties.OptionsUpdatesPage.Default.CheckForUpdatesFrequencyDays))));

            if (!Properties.OptionsUpdatesPage.Default.UpdatePending && DateTime.UtcNow <= nextUpdateCheck) return;
            if (!IsHandleCreated)
                CreateHandle(); // Make sure the handle is created so that InvokeRequired returns the correct result

            await Startup.Instance.CheckForUpdate();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Runtime.WindowList != null)
            {
                foreach (BaseWindow window in Runtime.WindowList)
                {
                    window.Close();
                }
            }

            IsClosing = true;

            Hide();

            if (Properties.OptionsAppearancePage.Default.CloseToTray)
            {
                if (Runtime.NotificationAreaIcon == null)
                    Runtime.NotificationAreaIcon = new NotificationAreaIcon();

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
                var openConnections = 0;
                if (pnlDock.Contents.Count > 0)
                {
                    foreach (var dc in pnlDock.Contents)
                    {
                        if (!(dc is ConnectionWindow cw)) continue;
                        if (cw.Controls.Count < 1) continue;
                        if (!(cw.Controls[0] is DockPanel dp)) continue;
                        if (dp.Contents.Count > 0)
                            openConnections += dp.Contents.Count;
                    }
                }

                if (openConnections > 0 &&
                    (Properties.Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.All |
                     (Properties.Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.Multiple &
                      openConnections > 1) || Properties.Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.Exit))
                {
                    var result = CTaskDialog.MessageBox(this, Application.ProductName, Language.ConfirmExitMainInstruction, "", "", "", Language.CheckboxDoNotShowThisMessageAgain, ETaskDialogButtons.YesNo, ESysIcons.Question, ESysIcons.Question);
                    if (CTaskDialog.VerificationChecked)
                    {
                        Properties.Settings.Default.ConfirmCloseConnection--; //--?
                    }

                    if (result == DialogResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }

            NativeMethods.ChangeClipboardChain(Handle, _fpChainedWindowHandle);
            Shutdown.Cleanup(_quickConnectToolStrip, _externalToolsToolStrip, _multiSshToolStrip, this);

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
                if (!Properties.OptionsAppearancePage.Default.MinimizeToTray) return;
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
                        var candidateTabToFocus = FromChildHandle(NativeMethods.WindowFromPoint(MousePosition))
                                               ?? GetChildAtPoint(MousePosition);
                        if (candidateTabToFocus is InterfaceControl) candidateTabToFocus.Parent.Focus();
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
                        var windowPos =
                            (NativeMethods.WINDOWPOS)Marshal.PtrToStructure(m.LParam, typeof(NativeMethods.WINDOWPOS));
                        if ((windowPos.flags & NativeMethods.SWP_NOACTIVATE) == 0)
                        {
                            if (!_inMouseActivate && !_inSizeMove)
                                ActivateConnection();
                        }
                        break;
                    case NativeMethods.WM_SYSCOMMAND:
                        if (m.WParam == new IntPtr(0))
                            ShowHideMenu();
                        var screen = _advancedWindowMenu.GetScreenById(m.WParam.ToInt32());
                        if (screen != null)
                        {
                            Screens.SendFormToScreen(screen);
                            Console.WriteLine(_advancedWindowMenu.GetScreenById(m.WParam.ToInt32()).ToString());
                        }
                        break;
                    case NativeMethods.WM_DRAWCLIPBOARD:
                        NativeMethods.SendMessage(_fpChainedWindowHandle, m.Msg, m.LParam, m.WParam);
                        _clipboardChangedEvent?.Invoke();
                        break;
                    case NativeMethods.WM_CHANGECBCHAIN:
                        // When a clipboard viewer window receives the WM_CHANGECBCHAIN message, 
                        // it should call the SendMessage function to pass the message to the 
                        // next window in the chain, unless the next window is the window 
                        // being removed. In this case, the clipboard viewer should save 
                        // the handle specified by the lParam parameter as the next window in the chain. 
                        //
                        // wParam is the Handle to the window being removed from 
                        // the clipboard viewer chain 
                        // lParam is the Handle to the next window in the chain 
                        // following the window being removed. 
                        if (m.WParam == _fpChainedWindowHandle) {
                            // If wParam is the next clipboard viewer then it
                            // is being removed so update pointer to the next
                            // window in the clipboard chain
                            _fpChainedWindowHandle = m.LParam;
                        } else {
                            //Send to the next window
                            NativeMethods.SendMessage(_fpChainedWindowHandle, m.Msg, m.LParam, m.WParam);
                        }
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
            NativeMethods.SendMessage(control.Handle, NativeMethods.WM_LBUTTONDOWN, (IntPtr)NativeMethods.MK_LBUTTON,
                                      (IntPtr)NativeMethods.MAKELPARAM(ref temp_wLow, ref temp_wHigh));
            clientMousePosition.X = temp_wLow;
            clientMousePosition.Y = temp_wHigh;
        }

        private void ActivateConnection()
        {
            var cw = pnlDock.ActiveDocument as ConnectionWindow;
            var dp = cw?.ActiveControl as DockPane;

            if (!(dp?.ActiveContent is ConnectionTab tab)) return;
            var ifc = InterfaceControl.FindInterfaceControl(tab);
            if (ifc == null) return;

            ifc.Protocol.Focus();
            var conFormWindow = ifc.FindForm();
            ((ConnectionTab)conFormWindow)?.RefreshInterfaceController();
        }

        private void pnlDock_ActiveDocumentChanged(object sender, EventArgs e)
        {
            ActivateConnection();
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
                titleBuilder.Append(SelectedConnection.Name);

                if (Properties.Settings.Default.TrackActiveConnectionInConnectionTree)
                    Windows.TreeForm.JumpToNode(SelectedConnection);
            }

            Text = titleBuilder.ToString();
        }

        public void ShowHidePanelTabs(DockContent closingDocument = null)
        {
            DocumentStyle newDocumentStyle;

            if (Properties.OptionsTabsPanelsPage.Default.AlwaysShowPanelTabs)
            {
                newDocumentStyle = DocumentStyle.DockingWindow; // Show the panel tabs
            }
            else
            {
                var nonConnectionPanelCount = 0;
                foreach (var dockContent in pnlDock.Documents)
                {
                    var document = (DockContent)dockContent;
                    if ((closingDocument == null || document != closingDocument) && !(document is ConnectionWindow))
                    {
                        nonConnectionPanelCount++;
                    }
                }

                newDocumentStyle = nonConnectionPanelCount == 0
                    ? DocumentStyle.DockingSdi
                    : DocumentStyle.DockingWindow;
            }

            // TODO: See if we can get this to work with DPS
#if false
            foreach (var dockContent in pnlDock.Documents)
			{
				var document = (DockContent)dockContent;
				if (document is ConnectionWindow)
				{
					var connectionWindow = (ConnectionWindow)document;
					if (Settings.Default.AlwaysShowConnectionTabs == false)
					{
						connectionWindow.TabController.HideTabsMode = TabControl.HideTabsModes.HidepnlDock.DockLeftPortion = Always;
					}
					else
					{
						connectionWindow.TabController.HideTabsMode = TabControl.HideTabsModes.ShowAlways;
					}
				}
			}
#endif

            if (pnlDock.DocumentStyle == newDocumentStyle) return;
            pnlDock.DocumentStyle = newDocumentStyle;
            pnlDock.Size = new Size(1, 1);
        }

        public void SetDefaultLayout()
        {
            pnlDock.Visible = false;

            Windows.TreeForm.Show(pnlDock, DockState.DockLeft);
            Windows.ConfigForm.Show(pnlDock, DockState.DockLeft);
            Windows.ErrorsForm.Show(pnlDock, DockState.DockBottomAutoHide);
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

            if (Properties.Settings.Default.ViewMenuMessages == true)
            {
                Windows.ErrorsForm.Show(pnlDock, DockState.DockBottomAutoHide);
                viewMenu._mMenViewErrorsAndInfos.Checked = true;
            }
            else
                viewMenu._mMenViewErrorsAndInfos.Checked = false;


            if (Properties.Settings.Default.ViewMenuExternalTools == true)
            {
                viewMenu.TsExternalTools.Visible = true;
                viewMenu._mMenViewExtAppsToolbar.Checked = true;
            }
            else
            {
                viewMenu.TsExternalTools.Visible = false;
                viewMenu._mMenViewExtAppsToolbar.Checked = false;
            }

            if (Properties.Settings.Default.ViewMenuMultiSSH == true)
            {
                viewMenu.TsMultiSsh.Visible = true;
                viewMenu._mMenViewMultiSshToolbar.Checked = true;
            }
            else
            {
                viewMenu.TsMultiSsh.Visible = false;
                viewMenu._mMenViewMultiSshToolbar.Checked = false;
            }

            if (Properties.Settings.Default.ViewMenuQuickConnect == true)
            {
                viewMenu.TsQuickConnect.Visible = true;
                viewMenu._mMenViewQuickConnectToolbar.Checked = true;
            }
            else
            {
                viewMenu.TsQuickConnect.Visible = false;
                viewMenu._mMenViewQuickConnectToolbar.Checked = false;
            }

            if (Properties.Settings.Default.LockToolbars == true)
            {
                Properties.Settings.Default.LockToolbars = true;
                viewMenu._mMenViewLockToolbars.Checked = true;                
            }
            else
            {
                Properties.Settings.Default.LockToolbars = false;
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
                    (ClipboardchangeEventHandler)Delegate.Remove(_clipboardChangedEvent, value);
        }

        #endregion

        private void ViewMenu_Opening(object sender, EventArgs e)
        {
            viewMenu.mMenView_DropDownOpening(sender, e);
        }

        private void tsModeUser_Click(object sender, EventArgs e)
        {
            Properties.OptionsRbac.Default.ActiveRole = "UserRole";
        }

        private void tsModeAdmin_Click(object sender, EventArgs e)
        {
            Properties.OptionsRbac.Default.ActiveRole = "AdminRole";
        }
    }
}