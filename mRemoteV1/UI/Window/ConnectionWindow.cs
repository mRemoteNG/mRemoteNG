using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using BrightIdeasSoftware;
using mRemoteNG.Connection;
using mRemoteNG.App;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.Config;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.TaskDialog;
using mRemoteNG.App.Info;
using mRemoteNG.Container;
using mRemoteNG.Tools;
using mRemoteNG.UI.Forms.Input;
using Message = System.Windows.Forms.Message;
using TabControl = Crownwood.Magic.Controls.TabControl;
using TabPage = Crownwood.Magic.Controls.TabPage;


namespace mRemoteNG.UI.Window
{
    public partial class ConnectionWindow : BaseWindow
    {
        public TabControl TabController;
        private readonly IConnectionInitiator _connectionInitiator = new ConnectionInitiator();


        #region Public Methods
        public ConnectionWindow(DockContent panel, string formText = "")
        {
            if (formText == "")
            {
                formText = Language.strNewPanel;
            }

            WindowType = WindowType.Connection;
            DockPnl = panel;
            InitializeComponent();
            SetEventHandlers();
            // ReSharper disable once VirtualMemberCallInConstructor
            Text = formText;
            TabText = formText;
        }

        private void SetEventHandlers()
        {
            SetFormEventHandlers();
            SetTabControllerEventHandlers();
            SetContextMenuEventHandlers();
        }

        private void SetFormEventHandlers()
        {
            Load += Connection_Load;
            DockStateChanged += Connection_DockStateChanged;
            FormClosing += Connection_FormClosing;
        }

        private void SetTabControllerEventHandlers()
        {
            TabController.ClosePressed += TabController_ClosePressed;
            TabController.DoubleClickTab += TabController_DoubleClickTab;
            TabController.DragDrop += TabController_DragDrop;
            TabController.DragOver += TabController_DragOver;
            TabController.SelectionChanged += TabController_SelectionChanged;
            TabController.MouseUp += TabController_MouseUp;
            TabController.PageDragEnd += TabController_PageDragStart;
            TabController.PageDragStart += TabController_PageDragStart;
            TabController.PageDragMove += TabController_PageDragMove;
            TabController.PageDragEnd += TabController_PageDragEnd;
            TabController.PageDragQuit += TabController_PageDragEnd;
        }

        private void SetContextMenuEventHandlers()
        {
            cmenTabFullscreen.Click += (sender, args) => ToggleFullscreen();
            cmenTabSmartSize.Click += (sender, args) => ToggleSmartSize();
            cmenTabViewOnly.Click += (sender, args) => ToggleViewOnly();
            cmenTabScreenshot.Click += (sender, args) => CreateScreenshot();
            cmenTabStartChat.Click += (sender, args) => StartChat();
            cmenTabTransferFile.Click += (sender, args) => TransferFile();
            cmenTabRefreshScreen.Click += (sender, args) => RefreshScreen();
            cmenTabSendSpecialKeysCtrlAltDel.Click += (sender, args) => SendSpecialKeys(ProtocolVNC.SpecialKeys.CtrlAltDel);
            cmenTabSendSpecialKeysCtrlEsc.Click += (sender, args) => SendSpecialKeys(ProtocolVNC.SpecialKeys.CtrlEsc);
            cmenTabRenameTab.Click += (sender, args) => RenameTab();
            cmenTabDuplicateTab.Click += (sender, args) => DuplicateTab();
            cmenTabReconnect.Click += (sender, args) => Reconnect();
            cmenTabDisconnect.Click += (sender, args) => CloseTabMenu();
            cmenTabPuttySettings.Click += (sender, args) => ShowPuttySettingsDialog();
        }

        public TabPage AddConnectionTab(ConnectionInfo connectionInfo)
        {
            try
            {
                var nTab = new TabPage
                {
                    Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
                };

                if (Settings.Default.ShowProtocolOnTabs)
                    nTab.Title = connectionInfo.Protocol + @": ";
                else
                    nTab.Title = "";

                nTab.Title += connectionInfo.Name;

                if (Settings.Default.ShowLogonInfoOnTabs)
                {
                    nTab.Title += @" (";
                    if (connectionInfo.Domain != "")
                        nTab.Title += connectionInfo.Domain;

                    if (connectionInfo.Username != "")
                    {
                        if (connectionInfo.Domain != "")
                            nTab.Title += @"\";
                        nTab.Title += connectionInfo.Username;
                    }

                    nTab.Title += @")";
                }

                nTab.Title = nTab.Title.Replace("&", "&&");

                var conIcon = ConnectionIcon.FromString(connectionInfo.Icon);
                if (conIcon != null)
                    nTab.Icon = conIcon;

                if (Settings.Default.OpenTabsRightOfSelected)
                    TabController.TabPages.Insert(TabController.SelectedIndex + 1, nTab);
                else
                    TabController.TabPages.Add(nTab);

                nTab.Selected = true;
                _ignoreChangeSelectedTabClick = false;

                return nTab;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("AddConnectionTab (UI.Window.ConnectionWindow) failed", ex);
            }

            return null;
        }

        public void UpdateSelectedConnection()
        {
            if (TabController.SelectedTab == null)
            {
                FrmMain.Default.SelectedConnection = null;
            }
            else
            {
                var interfaceControl = TabController.SelectedTab?.Tag as InterfaceControl;
                FrmMain.Default.SelectedConnection = interfaceControl?.Info;
            }
        }
        #endregion

        #region Form
        private void Connection_Load(object sender, EventArgs e)
        {
            ApplyLanguage();
        }

        private bool _documentHandlersAdded;
        private bool _floatHandlersAdded;
        private void Connection_DockStateChanged(object sender, EventArgs e)
        {
            if (DockState == DockState.Float)
            {
                if (_documentHandlersAdded)
                {
                    FrmMain.Default.ResizeBegin -= Connection_ResizeBegin;
                    FrmMain.Default.ResizeEnd -= Connection_ResizeEnd;
                    _documentHandlersAdded = false;
                }
                DockHandler.FloatPane.FloatWindow.ResizeBegin += Connection_ResizeBegin;
                DockHandler.FloatPane.FloatWindow.ResizeEnd += Connection_ResizeEnd;
                _floatHandlersAdded = true;
            }
            else if (DockState == DockState.Document)
            {
                if (_floatHandlersAdded)
                {
                    DockHandler.FloatPane.FloatWindow.ResizeBegin -= Connection_ResizeBegin;
                    DockHandler.FloatPane.FloatWindow.ResizeEnd -= Connection_ResizeEnd;
                    _floatHandlersAdded = false;
                }
                FrmMain.Default.ResizeBegin += Connection_ResizeBegin;
                FrmMain.Default.ResizeEnd += Connection_ResizeEnd;
                _documentHandlersAdded = true;
            }
        }

        private void ApplyLanguage()
        {
            cmenTabFullscreen.Text = Language.strMenuFullScreenRDP;
            cmenTabSmartSize.Text = Language.strMenuSmartSize;
            cmenTabViewOnly.Text = Language.strMenuViewOnly;
            cmenTabScreenshot.Text = Language.strMenuScreenshot;
            cmenTabStartChat.Text = Language.strMenuStartChat;
            cmenTabTransferFile.Text = Language.strMenuTransferFile;
            cmenTabRefreshScreen.Text = Language.strMenuRefreshScreen;
            cmenTabSendSpecialKeys.Text = Language.strMenuSendSpecialKeys;
            cmenTabSendSpecialKeysCtrlAltDel.Text = Language.strMenuCtrlAltDel;
            cmenTabSendSpecialKeysCtrlEsc.Text = Language.strMenuCtrlEsc;
            cmenTabExternalApps.Text = Language.strMenuExternalTools;
            cmenTabRenameTab.Text = Language.strMenuRenameTab;
            cmenTabDuplicateTab.Text = Language.strMenuDuplicateTab;
            cmenTabReconnect.Text = Language.strMenuReconnect;
            cmenTabDisconnect.Text = Language.strMenuDisconnect;
            cmenTabPuttySettings.Text = Language.strPuttySettings;
        }

        private void Connection_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!FrmMain.Default.IsClosing &&
                (Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.All & TabController.TabPages.Count > 0 ||
                Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.Multiple & TabController.TabPages.Count > 1))
            {
                var result = CTaskDialog.MessageBox(this, GeneralAppInfo.ProductName, string.Format(Language.strConfirmCloseConnectionPanelMainInstruction, Text), "", "", "", Language.strCheckboxDoNotShowThisMessageAgain, ETaskDialogButtons.YesNo, ESysIcons.Question, ESysIcons.Question);
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

            try
            {
                foreach (TabPage tabP in TabController.TabPages)
                {
                    if (tabP.Tag == null) continue;
                    var interfaceControl = (InterfaceControl)tabP.Tag;
                    interfaceControl.Protocol.Close();
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("UI.Window.Connection.Connection_FormClosing() failed", ex);
            }
        }

        public new event EventHandler ResizeBegin;
        private void Connection_ResizeBegin(object sender, EventArgs e)
        {
            ResizeBegin?.Invoke(this, e);
        }

        public new event EventHandler ResizeEnd;
        private void Connection_ResizeEnd(object sender, EventArgs e)
        {
            ResizeEnd?.Invoke(sender, e);
        }
        #endregion

        #region TabController
        private void TabController_ClosePressed(object sender, EventArgs e)
        {
            if (TabController.SelectedTab == null)
            {
                return;
            }

            CloseConnectionTab();
        }

        private void CloseConnectionTab()
        {
            try
            {
                var selectedTab = TabController.SelectedTab;
                if (selectedTab == null) return;
                if (Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.All)
                {
                    var result = CTaskDialog.MessageBox(this, GeneralAppInfo.ProductName, string.Format(Language.strConfirmCloseConnectionMainInstruction, selectedTab.Title), "", "", "", Language.strCheckboxDoNotShowThisMessageAgain, ETaskDialogButtons.YesNo, ESysIcons.Question, ESysIcons.Question);
                    if (CTaskDialog.VerificationChecked)
                    {
                        Settings.Default.ConfirmCloseConnection--;
                    }
                    if (result == DialogResult.No)
                    {
                        return;
                    }
                }

                if (selectedTab.Tag != null)
                {
                    var interfaceControl = (InterfaceControl)selectedTab.Tag;
                    interfaceControl.Protocol.Close();
                }
                else
                {
                    CloseTab(selectedTab);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("UI.Window.Connection.CloseConnectionTab() failed", ex);
            }

            UpdateSelectedConnection();
        }

        private void TabController_DoubleClickTab(TabControl sender, TabPage page)
        {
            _firstClickTicks = 0;
            if (Settings.Default.DoubleClickOnTabClosesIt)
            {
                CloseConnectionTab();
            }
        }

        #region Drag and Drop
        private void TabController_DragDrop(object sender, DragEventArgs e)
        {
            var dropDataAsOlvDataObject = e.Data as OLVDataObject;
            if (dropDataAsOlvDataObject == null) return;
            var modelObjects = dropDataAsOlvDataObject.ModelObjects;
            foreach (var model in modelObjects)
            {
                var modelAsContainer = model as ContainerInfo;
                var modelAsConnection = model as ConnectionInfo;
                if (modelAsContainer != null)
                    _connectionInitiator.OpenConnection(modelAsContainer);
                else if (modelAsConnection != null)
                    _connectionInitiator.OpenConnection(modelAsConnection);
            }
        }

        private void TabController_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            var dropDataAsOlvDataObject = e.Data as OLVDataObject;
            var modelObjects = dropDataAsOlvDataObject?.ModelObjects;
            if (modelObjects == null) return;
            if (!modelObjects.OfType<ConnectionInfo>().Any()) return;
            e.Effect = DragDropEffects.Move;
        }
        #endregion
        #endregion

        #region Tab Menu
        private void ShowHideMenuButtons()
        {
            try
            {
                var interfaceControl = (InterfaceControl)TabController.SelectedTab?.Tag;
                if (interfaceControl == null) return;

                if (interfaceControl.Info.Protocol == ProtocolType.RDP)
                {
                    var rdp = (ProtocolRDP)interfaceControl.Protocol;
                    cmenTabFullscreen.Visible = true;
                    cmenTabFullscreen.Checked = rdp.Fullscreen;
                    cmenTabSmartSize.Visible = true;
                    cmenTabSmartSize.Checked = rdp.SmartSize;
                }
                else
                {
                    cmenTabFullscreen.Visible = false;
                    cmenTabSmartSize.Visible = false;
                }

                if (interfaceControl.Info.Protocol == ProtocolType.VNC)
                {
                    var vnc = (ProtocolVNC)interfaceControl.Protocol;
                    cmenTabSendSpecialKeys.Visible = true;
                    cmenTabViewOnly.Visible = true;
                    cmenTabSmartSize.Visible = true;
                    cmenTabStartChat.Visible = true;
                    cmenTabRefreshScreen.Visible = true;
                    cmenTabTransferFile.Visible = false;
                    cmenTabSmartSize.Checked = vnc.SmartSize;
                    cmenTabViewOnly.Checked = vnc.ViewOnly;
                }
                else
                {
                    cmenTabSendSpecialKeys.Visible = false;
                    cmenTabViewOnly.Visible = false;
                    cmenTabStartChat.Visible = false;
                    cmenTabRefreshScreen.Visible = false;
                    cmenTabTransferFile.Visible = false;
                }

                if (interfaceControl.Info.Protocol == ProtocolType.SSH1 | interfaceControl.Info.Protocol == ProtocolType.SSH2)
                {
                    cmenTabTransferFile.Visible = true;
                }

                if (interfaceControl.Protocol is PuttyBase)
                {
                    cmenTabPuttySettings.Visible = true;
                }
                else
                {
                    cmenTabPuttySettings.Visible = false;
                }

                AddExternalApps();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("ShowHideMenuButtons (UI.Window.ConnectionWindow) failed", ex);
            }
        }
        #endregion

        #region Tab Actions
        private void ToggleSmartSize()
        {
            try
            {
                if (!(TabController.SelectedTab?.Tag is InterfaceControl)) return;
                var interfaceControl = (InterfaceControl)TabController.SelectedTab?.Tag;

                var protocol = interfaceControl.Protocol as ProtocolRDP;
                if (protocol != null)
                {
                    var rdp = protocol;
                    rdp.ToggleSmartSize();
                }
                else if (interfaceControl.Protocol is ProtocolVNC)
                {
                    var vnc = (ProtocolVNC)interfaceControl.Protocol;
                    vnc.ToggleSmartSize();
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("ToggleSmartSize (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private void TransferFile()
        {
            try
            {
                var interfaceControl = TabController.SelectedTab?.Tag as InterfaceControl;
                if (interfaceControl == null) return;

                if (interfaceControl.Info.Protocol == ProtocolType.SSH1 | interfaceControl.Info.Protocol == ProtocolType.SSH2)
                    SshTransferFile();
                else if (interfaceControl.Info.Protocol == ProtocolType.VNC)
                    VncTransferFile();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("TransferFile (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private void SshTransferFile()
        {
            try
            {
                var interfaceControl = TabController.SelectedTab?.Tag as InterfaceControl;
                if (interfaceControl == null) return;

                Windows.Show(WindowType.SSHTransfer);
                var connectionInfo = interfaceControl.Info;

                Windows.SshtransferForm.Hostname = connectionInfo.Hostname;
                Windows.SshtransferForm.Username = connectionInfo.Username;
                Windows.SshtransferForm.Password = connectionInfo.Password;
                Windows.SshtransferForm.Port = Convert.ToString(connectionInfo.Port);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("SSHTransferFile (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private void VncTransferFile()
        {
            try
            {
                var interfaceControl = TabController.SelectedTab?.Tag as InterfaceControl;
                var vnc = interfaceControl?.Protocol as ProtocolVNC;
                vnc?.StartFileTransfer();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("VNCTransferFile (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private void ToggleViewOnly()
        {
            try
            {
                var interfaceControl = TabController.SelectedTab?.Tag as InterfaceControl;
                var vnc = interfaceControl?.Protocol as ProtocolVNC;
                if (vnc == null) return;
                cmenTabViewOnly.Checked = !cmenTabViewOnly.Checked;
                vnc.ToggleViewOnly();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("ToggleViewOnly (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private void StartChat()
        {
            try
            {
                var interfaceControl = TabController.SelectedTab?.Tag as InterfaceControl;
                var vnc = interfaceControl?.Protocol as ProtocolVNC;
                vnc?.StartChat();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("StartChat (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private void RefreshScreen()
        {
            try
            {
                var interfaceControl = TabController.SelectedTab?.Tag as InterfaceControl;
                var vnc = interfaceControl?.Protocol as ProtocolVNC;
                vnc?.RefreshScreen();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("RefreshScreen (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private void SendSpecialKeys(ProtocolVNC.SpecialKeys keys)
        {
            try
            {
                var interfaceControl = TabController.SelectedTab?.Tag as InterfaceControl;
                var vnc = interfaceControl?.Protocol as ProtocolVNC;
                vnc?.SendSpecialKeys(keys);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("SendSpecialKeys (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private void ToggleFullscreen()
        {
            try
            {
                var interfaceControl = TabController.SelectedTab?.Tag as InterfaceControl;
                var rdp = interfaceControl?.Protocol as ProtocolRDP;
                rdp?.ToggleFullscreen();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("ToggleFullscreen (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private void ShowPuttySettingsDialog()
        {
            try
            {
                var interfaceControl = TabController.SelectedTab?.Tag as InterfaceControl;
                var puttyBase = interfaceControl?.Protocol as PuttyBase;
                puttyBase?.ShowSettingsDialog();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("ShowPuttySettingsDialog (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private void AddExternalApps()
        {
            try
            {
                //clean up. since new items are added below, we have to dispose of any previous items first
                if (cmenTabExternalApps.DropDownItems.Count > 0)
                {
                    for (var i = cmenTabExternalApps.DropDownItems.Count - 1; i >= 0; i--)
                        cmenTabExternalApps.DropDownItems[i].Dispose();
                    cmenTabExternalApps.DropDownItems.Clear();
                }

                //add ext apps
                foreach (ExternalTool externalTool in Runtime.ExternalTools)
                {
                    var nItem = new ToolStripMenuItem
                    {
                        Text = externalTool.DisplayName,
                        Tag = externalTool,
                        /* rare failure here. While ExternalTool.Image already tries to default this
                         * try again so it's not null/doesn't crash.
                         */
                        Image = externalTool.Image ?? Resources.mRemote_Icon.ToBitmap()
                    };

                    nItem.Click += (sender, args) => StartExternalApp(((ToolStripMenuItem)sender).Tag as ExternalTool);
                    cmenTabExternalApps.DropDownItems.Add(nItem);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("cMenTreeTools_DropDownOpening failed (UI.Window.ConnectionWindow)", ex);
            }
        }

        private void StartExternalApp(ExternalTool externalTool)
        {
            try
            {
                var interfaceControl = TabController.SelectedTab?.Tag as InterfaceControl;
                externalTool.Start(interfaceControl?.Info);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("cmenTabExternalAppsEntry_Click failed (UI.Window.ConnectionWindow)", ex);
            }
        }

        private void CloseTabMenu()
        {
            try
            {
                var interfaceControl = TabController.SelectedTab?.Tag as InterfaceControl;
                interfaceControl?.Protocol.Close();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("CloseTabMenu (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private void DuplicateTab()
        {
            try
            {
                var interfaceControl = TabController.SelectedTab?.Tag as InterfaceControl;
                if (interfaceControl == null) return;
                _connectionInitiator.OpenConnection(interfaceControl.Info, ConnectionInfo.Force.DoNotJump);
                _ignoreChangeSelectedTabClick = false;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("DuplicateTab (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private void Reconnect()
        {
            try
            {
                var interfaceControl = TabController.SelectedTab?.Tag as InterfaceControl;
                if (interfaceControl == null) return;
                interfaceControl.Protocol.Close();
                _connectionInitiator.OpenConnection(interfaceControl.Info, ConnectionInfo.Force.DoNotJump);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("Reconnect (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private void RenameTab()
        {
            try
            {
                var newTitle = TabController.SelectedTab.Title;
                if (input.InputBox(Language.strNewTitle, Language.strNewTitle + ":", ref newTitle) == DialogResult.OK && !string.IsNullOrEmpty(newTitle))
                    TabController.SelectedTab.Title = newTitle.Replace("&", "&&");
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("RenameTab (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private void CreateScreenshot()
        {
            cmenTab.Close();
            Application.DoEvents();
            Windows.ScreenshotForm.AddScreenshot(MiscTools.TakeScreenshot(this));
        }
        #endregion

        #region Protocols
        public void Prot_Event_Closed(object sender)
        {
            var protocolBase = sender as ProtocolBase;
            var tabPage = protocolBase?.InterfaceControl.Parent as TabPage;
            if (tabPage != null)
                CloseTab(tabPage);
        }
        #endregion

        #region Tabs
        private delegate void CloseTabDelegate(TabPage tabToBeClosed);
        private void CloseTab(TabPage tabToBeClosed)
        {
            if (TabController.InvokeRequired)
            {
                CloseTabDelegate s = CloseTab;

                try
                {
                    TabController.Invoke(s, tabToBeClosed);
                }
                catch (COMException)
                {
                    TabController.Invoke(s, tabToBeClosed);
                }
                catch (Exception ex)
                {
                    Runtime.MessageCollector.AddExceptionMessage("Couldn't close tab", ex);
                }
            }
            else
            {
                try
                {
                    TabController.TabPages.Remove(tabToBeClosed);
                    _ignoreChangeSelectedTabClick = false;
                }
                catch (COMException)
                {
                    CloseTab(tabToBeClosed);
                }
                catch (Exception ex)
                {
                    Runtime.MessageCollector.AddExceptionMessage("Couldn't close tab", ex);
                }

                if (TabController.TabPages.Count == 0)
                {
                    Close();
                }
            }
        }

        private bool _ignoreChangeSelectedTabClick;
        private void TabController_SelectionChanged(object sender, EventArgs e)
        {
            _ignoreChangeSelectedTabClick = true;
            UpdateSelectedConnection();
            FocusInterfaceController();
            RefreshInterfaceController();
        }

        private int _firstClickTicks;
        private Rectangle _doubleClickRectangle;
        private void TabController_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!(NativeMethods.GetForegroundWindow() == FrmMain.Default.Handle) && !_ignoreChangeSelectedTabClick)
                {
                    var clickedTab = TabController.TabPageFromPoint(e.Location);
                    if (clickedTab != null && TabController.SelectedTab != clickedTab)
                    {
                        NativeMethods.SetForegroundWindow(Handle);
                        TabController.SelectedTab = clickedTab;
                    }
                }
                _ignoreChangeSelectedTabClick = false;

                switch (e.Button)
                {
                    case MouseButtons.Left:
                        var currentTicks = Environment.TickCount;
                        var elapsedTicks = currentTicks - _firstClickTicks;
                        if (elapsedTicks > SystemInformation.DoubleClickTime || !_doubleClickRectangle.Contains(MousePosition))
                        {
                            _firstClickTicks = currentTicks;
                            _doubleClickRectangle = new Rectangle(MousePosition.X - SystemInformation.DoubleClickSize.Width / 2, MousePosition.Y - SystemInformation.DoubleClickSize.Height / 2, SystemInformation.DoubleClickSize.Width, SystemInformation.DoubleClickSize.Height);
                            FocusInterfaceController();
                        }
                        else
                        {
                            TabController.OnDoubleClickTab(TabController.SelectedTab);
                        }
                        break;
                    case MouseButtons.Middle:
                        CloseConnectionTab();
                        break;
                    case MouseButtons.Right:
                        if (TabController.SelectedTab?.Tag == null) return;
                        ShowHideMenuButtons();
                        NativeMethods.SetForegroundWindow(Handle);
                        cmenTab.Show(TabController, e.Location);
                        break;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("TabController_MouseUp (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private void FocusInterfaceController()
        {
            try
            {
                var interfaceControl = TabController.SelectedTab?.Tag as InterfaceControl;
                interfaceControl?.Protocol?.Focus();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("FocusIC (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        public void RefreshInterfaceController()
        {
            try
            {
                var interfaceControl = TabController.SelectedTab?.Tag as InterfaceControl;
                if (interfaceControl?.Info.Protocol == ProtocolType.VNC)
                    ((ProtocolVNC)interfaceControl.Protocol).RefreshScreen();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("RefreshIC (UI.Window.Connection) failed", ex);
            }
        }
        #endregion

        #region Window Overrides
        protected override void WndProc(ref Message m)
        {
            try
            {
                if (m.Msg == NativeMethods.WM_MOUSEACTIVATE)
                {
                    var selectedTab = TabController.SelectedTab;
                    if (selectedTab == null) return;
                    {
                        var tabClientRectangle = selectedTab.RectangleToScreen(selectedTab.ClientRectangle);
                        if (tabClientRectangle.Contains(MousePosition))
                        {
                            var interfaceControl = selectedTab.Tag as InterfaceControl;
                            if (interfaceControl?.Info?.Protocol == ProtocolType.RDP)
                            {
                                interfaceControl.Protocol.Focus();
                                return; // Do not pass to base class
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("UI.Window.Connection.WndProc() failed.", ex);
            }

            base.WndProc(ref m);
        }
        #endregion

        #region Tab drag and drop
        public bool InTabDrag { get; set; }

        private void TabController_PageDragStart(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.SizeWE;
        }

        private void TabController_PageDragMove(object sender, MouseEventArgs e)
        {
            InTabDrag = true; // For some reason PageDragStart gets raised again after PageDragEnd so set this here instead

            var sourceTab = TabController.SelectedTab;
            var destinationTab = TabController.TabPageFromPoint(e.Location);

            if (!TabController.TabPages.Contains(destinationTab) || sourceTab == destinationTab)
                return;

            var targetIndex = TabController.TabPages.IndexOf(destinationTab);

            TabController.TabPages.SuspendEvents();
            TabController.TabPages.Remove(sourceTab);
            TabController.TabPages.Insert(targetIndex, sourceTab);
            TabController.SelectedTab = sourceTab;
            TabController.TabPages.ResumeEvents();
        }

        private void TabController_PageDragEnd(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Default;
            InTabDrag = false;
            var interfaceControl = TabController?.SelectedTab?.Tag as InterfaceControl;
            interfaceControl?.Protocol.Focus();
        }
        #endregion
    }
}