﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Messages;
using mRemoteNG.Themes;
using mRemoteNG.Tools;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Forms.Input;
using mRemoteNG.UI.Tabs;
using mRemoteNG.UI.TaskDialog;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.UI.Window
{
    public partial class ConnectionWindow : BaseWindow
    {
        private readonly IConnectionInitiator _connectionInitiator = new ConnectionInitiator();
        private VisualStudioToolStripExtender vsToolStripExtender;
        private readonly ToolStripRenderer _toolStripProfessionalRenderer = new ToolStripProfessionalRenderer();

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
            connDock.DocumentStyle = DocumentStyle.DockingWindow;
            connDock.ShowDocumentIcon = true;

            connDock.ActiveContentChanged += ConnDockOnActiveContentChanged;
        }

        private InterfaceControl GetInterfaceControl()
        {
            return InterfaceControl.FindInterfaceControl(connDock);
        }

        private void SetEventHandlers()
        {
            SetFormEventHandlers();
            SetContextMenuEventHandlers();
        }

        private void SetFormEventHandlers()
        {
            Load += Connection_Load;
            DockStateChanged += Connection_DockStateChanged;
            FormClosing += Connection_FormClosing;
        }

        private void SetContextMenuEventHandlers()
        {
            // event handler to adjust the items within the context menu
            cmenTab.Opening += ShowHideMenuButtons;

            // event handlers for all context menu items...
            cmenTabFullscreen.Click += (sender, args) => ToggleFullscreen();
            cmenTabSmartSize.Click += (sender, args) => ToggleSmartSize();
            cmenTabViewOnly.Click += (sender, args) => ToggleViewOnly();
            cmenTabScreenshot.Click += (sender, args) => CreateScreenshot();
            cmenTabStartChat.Click += (sender, args) => StartChat();
            cmenTabTransferFile.Click += (sender, args) => TransferFile();
            cmenTabRefreshScreen.Click += (sender, args) => RefreshScreen();
            cmenTabSendSpecialKeysCtrlAltDel.Click +=
                (sender, args) => SendSpecialKeys(ProtocolVNC.SpecialKeys.CtrlAltDel);
            cmenTabSendSpecialKeysCtrlEsc.Click += (sender, args) => SendSpecialKeys(ProtocolVNC.SpecialKeys.CtrlEsc);
            cmenTabRenameTab.Click += (sender, args) => RenameTab();
            cmenTabDuplicateTab.Click += (sender, args) => DuplicateTab();
            cmenTabReconnect.Click += (sender, args) => Reconnect();
            cmenTabDisconnect.Click += (sender, args) => CloseTabMenu();
            cmenTabDisconnectOthers.Click += (sender, args) => CloseOtherTabs();
            cmenTabDisconnectOthersRight.Click += (sender, args) => CloseOtherTabsToTheRight();
            cmenTabPuttySettings.Click += (sender, args) => ShowPuttySettingsDialog();
            GotFocus += ConnectionWindow_GotFocus;
        }

        private void ConnectionWindow_GotFocus(object sender, EventArgs e)
        {
            TabHelper.Instance.CurrentPanel = this;
        }

        public ConnectionTab AddConnectionTab(ConnectionInfo connectionInfo)
        {
            try
            {
                //Set the connection text based on name and preferences
                string titleText;
                if (Settings.Default.ShowProtocolOnTabs)
                    titleText = connectionInfo.Protocol + @": ";
                else
                    titleText = "";

                titleText += connectionInfo.Name;

                if (Settings.Default.ShowLogonInfoOnTabs)
                {
                    titleText += @" (";
                    if (connectionInfo.Domain != "")
                        titleText += connectionInfo.Domain;

                    if (connectionInfo.Username != "")
                    {
                        if (connectionInfo.Domain != "")
                            titleText += @"\";
                        titleText += connectionInfo.Username;
                    }

                    titleText += @")";
                }

                titleText = titleText.Replace("&", "&&");

                var conTab = new ConnectionTab
                {
                    Tag = connectionInfo,
                    DockAreas = DockAreas.Document | DockAreas.Float,
                    Icon = ConnectionIcon.FromString(connectionInfo.Icon),
                    TabText = titleText,
                    TabPageContextMenuStrip = cmenTab
                };

                //if (Settings.Default.AlwaysShowConnectionTabs == false)
                // TODO: See if we can make this work with DPS...
                //TabController.HideTabsMode = TabControl.HideTabsModes.HideAlways;


                //Show the tab
                conTab.Show(connDock, DockState.Document);
                conTab.Focus();
                return conTab;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("AddConnectionTab (UI.Window.ConnectionWindow) failed",
                                                             ex);
            }

            return null;
        }

        #endregion

        public void reconnectAll(IConnectionInitiator initiator)
        {
            var controlList = new List<InterfaceControl>();
            try
            {
                foreach (var dockContent in connDock.DocumentsToArray())
                {
                    var tab = (ConnectionTab)dockContent;
                    controlList.Add((InterfaceControl)tab.Tag);
                }

                foreach (var iControl in controlList)
                {
                    iControl.Protocol.Close();
                    initiator.OpenConnection(iControl.Info, ConnectionInfo.Force.DoNotJump);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("reconnectAll (UI.Window.ConnectionWindow) failed", ex);
            }

            // ReSharper disable once RedundantAssignment
            controlList = null;
        }

        #region Form

        private void Connection_Load(object sender, EventArgs e)
        {
            ApplyTheme();
            ThemeManager.getInstance().ThemeChanged += ApplyTheme;
            ApplyLanguage();
        }

        private new void ApplyTheme()
        {
            if (!ThemeManager.getInstance().ThemingActive)
            {
                connDock.Theme = ThemeManager.getInstance().DefaultTheme.Theme;
                return;
            }

            base.ApplyTheme();
            try
            {
                connDock.Theme = ThemeManager.getInstance().ActiveTheme.Theme;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("UI.Window.ConnectionWindow.ApplyTheme() failed", ex);
            }

            vsToolStripExtender = new VisualStudioToolStripExtender(components)
            {
                DefaultRenderer = _toolStripProfessionalRenderer
            };
            vsToolStripExtender.SetStyle(cmenTab, ThemeManager.getInstance().ActiveTheme.Version,
                                         ThemeManager.getInstance().ActiveTheme.Theme);

            if (!ThemeManager.getInstance().ActiveAndExtended) return;
            connDock.DockBackColor =
                ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Tab_Item_Background");
        }

        private bool _documentHandlersAdded;
        private bool _floatHandlersAdded;

        private void Connection_DockStateChanged(object sender, EventArgs e)
        {
            switch (DockState)
            {
                case DockState.Float:
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
                    break;
                }
                case DockState.Document:
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
                    break;
                }
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
            cmenTabDisconnectOthers.Text = Language.strMenuDisconnectOthers;
            cmenTabDisconnectOthersRight.Text = Language.strMenuDisconnectOthersRight;
            cmenTabPuttySettings.Text = Language.strPuttySettings;
        }

        private void Connection_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!FrmMain.Default.IsClosing &&
                (Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.All & connDock.Documents.Any() ||
                 Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.Multiple &
                 connDock.Documents.Count() > 1))
            {
                var result = CTaskDialog.MessageBox(this, GeneralAppInfo.ProductName,
                                                    string
                                                        .Format(Language.strConfirmCloseConnectionPanelMainInstruction,
                                                                Text), "", "", "",
                                                    Language.strCheckboxDoNotShowThisMessageAgain,
                                                    ETaskDialogButtons.YesNo, ESysIcons.Question,
                                                    ESysIcons.Question);
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
                foreach (var dockContent in connDock.Documents.ToArray())
                {
                    var tabP = (ConnectionTab)dockContent;
                    if (tabP.Tag == null) continue;
                    tabP.silentClose = true;
                    tabP.Close();
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("UI.Window.Connection.Connection_FormClosing() failed",
                                                             ex);
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

        #region Events

        private void ConnDockOnActiveContentChanged(object sender, EventArgs e)
        {
            var ic = GetInterfaceControl();
            if (ic?.Info == null) return;
            FrmMain.Default.SelectedConnection = ic.Info;
        }

        #endregion

        #region Tab Menu

        private void ShowHideMenuButtons(object sender, CancelEventArgs e)
        {
            try
            {
                var interfaceControl = GetInterfaceControl();
                if (interfaceControl == null) return;

                if (interfaceControl.Protocol is ISupportsViewOnly viewOnly)
                {
                    cmenTabViewOnly.Visible = true;
                    cmenTabViewOnly.Checked = viewOnly.ViewOnly;
                }
                else
                {
                    cmenTabViewOnly.Visible = false;
                }

                if (interfaceControl.Info.Protocol == ProtocolType.RDP)
                {
                    var rdp = (RdpProtocol)interfaceControl.Protocol;
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
                    cmenTabSmartSize.Visible = true;
                    cmenTabStartChat.Visible = true;
                    cmenTabRefreshScreen.Visible = true;
                    cmenTabTransferFile.Visible = false;
                    cmenTabSmartSize.Checked = vnc.SmartSize;
                }
                else
                {
                    cmenTabSendSpecialKeys.Visible = false;
                    cmenTabStartChat.Visible = false;
                    cmenTabRefreshScreen.Visible = false;
                    cmenTabTransferFile.Visible = false;
                }

                if (interfaceControl.Info.Protocol == ProtocolType.SSH1 |
                    interfaceControl.Info.Protocol == ProtocolType.SSH2)
                {
                    cmenTabTransferFile.Visible = true;
                }

                cmenTabPuttySettings.Visible = interfaceControl.Protocol is PuttyBase;

                AddExternalApps();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("ShowHideMenuButtons (UI.Window.ConnectionWindow) failed",
                                                             ex);
            }
        }

        #endregion

        #region Tab Actions

        private void ToggleSmartSize()
        {
            try
            {
                var interfaceControl = GetInterfaceControl();

                switch (interfaceControl.Protocol)
                {
                    case RdpProtocol rdp:
                        rdp.ToggleSmartSize();
                        break;
                    case ProtocolVNC vnc:
                        vnc.ToggleSmartSize();
                        break;
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
                var interfaceControl = GetInterfaceControl();
                if (interfaceControl == null) return;

                if (interfaceControl.Info.Protocol == ProtocolType.SSH1 |
                    interfaceControl.Info.Protocol == ProtocolType.SSH2)
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
                var interfaceControl = GetInterfaceControl();
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
                var interfaceControl = GetInterfaceControl();
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
                var interfaceControl = GetInterfaceControl();
                if (!(interfaceControl?.Protocol is ISupportsViewOnly viewOnly))
                    return;

                cmenTabViewOnly.Checked = !cmenTabViewOnly.Checked;
                viewOnly.ToggleViewOnly();
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
                var interfaceControl = GetInterfaceControl();
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
                var interfaceControl = GetInterfaceControl();
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
                var interfaceControl = GetInterfaceControl();
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
                var interfaceControl = GetInterfaceControl();
                var rdp = interfaceControl?.Protocol as RdpProtocol;
                rdp?.ToggleFullscreen();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("ToggleFullscreen (UI.Window.ConnectionWindow) failed",
                                                             ex);
            }
        }

        private void ShowPuttySettingsDialog()
        {
            try
            {
                var interfaceControl = GetInterfaceControl();
                var puttyBase = interfaceControl?.Protocol as PuttyBase;
                puttyBase?.ShowSettingsDialog();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(
                                                             "ShowPuttySettingsDialog (UI.Window.ConnectionWindow) failed",
                                                             ex);
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
                foreach (var externalTool in Runtime.ExternalToolsService.ExternalTools)
                {
                    var nItem = new ToolStripMenuItem
                    {
                        Text = externalTool.DisplayName,
                        Tag = externalTool,
                        /* rare failure here. While ExternalTool.Image already tries to default this
                         * try again so it's not null/doesn't crash.
                         */
                        Image = externalTool.Image ?? Resources.mRemoteNG_Icon.ToBitmap()
                    };

                    nItem.Click += (sender, args) => StartExternalApp(((ToolStripMenuItem)sender).Tag as ExternalTool);
                    cmenTabExternalApps.DropDownItems.Add(nItem);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(
                                                                "cMenTreeTools_DropDownOpening failed (UI.Window.ConnectionWindow)",
                                                                ex);
            }
        }

        private void StartExternalApp(ExternalTool externalTool)
        {
            try
            {
                var interfaceControl = GetInterfaceControl();
                externalTool.Start(interfaceControl?.Info);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(
                                                             "cmenTabExternalAppsEntry_Click failed (UI.Window.ConnectionWindow)",
                                                             ex);
            }
        }


        private void CloseTabMenu()
        {
            var selectedTab = (ConnectionTab)GetInterfaceControl()?.Parent;
            if (selectedTab == null) return;

            try
            {
                selectedTab.Close();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("CloseTabMenu (UI.Window.ConnectionWindow) failed", ex);
            }
        }

        private void CloseOtherTabs()
        {
            var selectedTab = (ConnectionTab)GetInterfaceControl()?.Parent;
            if (selectedTab == null) return;
            if (Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.Multiple)
            {
                var result = CTaskDialog.MessageBox(this, GeneralAppInfo.ProductName,
                                                    string.Format(Language.strConfirmCloseConnectionOthersInstruction,
                                                                  selectedTab.TabText), "", "", "",
                                                    Language.strCheckboxDoNotShowThisMessageAgain,
                                                    ETaskDialogButtons.YesNo, ESysIcons.Question,
                                                    ESysIcons.Question);
                if (CTaskDialog.VerificationChecked)
                {
                    Settings.Default.ConfirmCloseConnection--;
                }

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            foreach (var dockContent in connDock.DocumentsToArray())
            {
                var tab = (ConnectionTab)dockContent;
                if (selectedTab != tab)
                {
                    tab.Close();
                }
            }
        }

        private void CloseOtherTabsToTheRight()
        {
            try
            {
                var selectedTab = (ConnectionTab)GetInterfaceControl()?.Parent;
                if (selectedTab == null) return;
                var dockPane = selectedTab.Pane;

                var pastTabToKeepAlive = false;
                var connectionsToClose = new List<ConnectionTab>();
                foreach (var dockContent in dockPane.Contents)
                {
                    var tab = (ConnectionTab)dockContent;
                    if (pastTabToKeepAlive)
                        connectionsToClose.Add(tab);

                    if (selectedTab == tab)
                        pastTabToKeepAlive = true;
                }

                foreach (var tab in connectionsToClose)
                {
                    tab.Close();
                }
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
                var interfaceControl = GetInterfaceControl();
                if (interfaceControl == null) return;
                _connectionInitiator.OpenConnection(interfaceControl.Info, ConnectionInfo.Force.DoNotJump);
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
                var interfaceControl = GetInterfaceControl();
                if (interfaceControl == null)
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                                                        "Reconnect (UI.Window.ConnectionWindow) failed. Could not find InterfaceControl.");
                    return;
                }

                Invoke(new Action(() => Prot_Event_Closed(interfaceControl.Protocol)));
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
                var interfaceControl = GetInterfaceControl();
                if (interfaceControl == null) return;
                using (var frmInputBox = new FrmInputBox(Language.strNewTitle, Language.strNewTitle,
                                                         ((ConnectionTab)interfaceControl.Parent).TabText))
                {
                    var dr = frmInputBox.ShowDialog();
                    if (dr != DialogResult.OK) return;
                    if (!string.IsNullOrEmpty(frmInputBox.returnValue))
                        ((ConnectionTab)interfaceControl.Parent).TabText = frmInputBox.returnValue.Replace("&", "&&");
                }
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
            //var selectedTab = (ConnectionTab)GetInterfaceControl()?.Parent;
            if (TabHelper.Instance.CurrentTab == null) return;
            Windows.ScreenshotForm.AddScreenshot(MiscTools.TakeScreenshot(TabHelper.Instance.CurrentTab));
        }

        #endregion

        #region Protocols

        public void Prot_Event_Closed(object sender)
        {
            var protocolBase = sender as ProtocolBase;
            if (!(protocolBase?.InterfaceControl.Parent is ConnectionTab tabPage)) return;
            if (tabPage.Disposing) return;
            tabPage.protocolClose = true;
            Invoke(new Action(() => tabPage.Close()));
        }

        #endregion
    }
}