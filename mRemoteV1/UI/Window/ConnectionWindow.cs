using System;
using System.Drawing;
using System.Windows.Forms;
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
using mRemoteNG.UI.Forms.Input;

namespace mRemoteNG.UI.Window
{
    public class ConnectionWindow : BaseWindow
    {
        #region Form Init
        internal ContextMenuStrip cmenTab;
        private System.ComponentModel.Container components;
        private ToolStripMenuItem cmenTabFullscreen;
        private ToolStripMenuItem cmenTabScreenshot;
        private ToolStripMenuItem cmenTabTransferFile;
        private ToolStripMenuItem cmenTabSendSpecialKeys;
        private ToolStripSeparator cmenTabSep1;
        private ToolStripMenuItem cmenTabRenameTab;
        private ToolStripMenuItem cmenTabDuplicateTab;
        private ToolStripMenuItem cmenTabDisconnect;
        private ToolStripMenuItem cmenTabSmartSize;
        private ToolStripMenuItem cmenTabSendSpecialKeysCtrlAltDel;
        private ToolStripMenuItem cmenTabSendSpecialKeysCtrlEsc;
        private ToolStripMenuItem cmenTabViewOnly;
        internal ToolStripMenuItem cmenTabReconnect;
        internal ToolStripMenuItem cmenTabExternalApps;
        private ToolStripMenuItem cmenTabStartChat;
        private ToolStripMenuItem cmenTabRefreshScreen;
        private ToolStripSeparator ToolStripSeparator1;
        private ToolStripMenuItem cmenTabPuttySettings;

        public Crownwood.Magic.Controls.TabControl TabController;
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            Load += Connection_Load;
            DockStateChanged += Connection_DockStateChanged;
            FormClosing += Connection_FormClosing;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectionWindow));
            TabController = new Crownwood.Magic.Controls.TabControl();
            TabController.ClosePressed += TabController_ClosePressed;
            TabController.DoubleClickTab += TabController_DoubleClickTab;
            TabController.DragDrop += TabController_DragDrop;
            TabController.DragEnter += TabController_DragEnter;
            TabController.DragOver += TabController_DragOver;
            TabController.SelectionChanged += TabController_SelectionChanged;
            TabController.MouseUp += TabController_MouseUp;
            TabController.PageDragEnd += TabController_PageDragStart;
            TabController.PageDragStart += TabController_PageDragStart;
            TabController.PageDragMove += TabController_PageDragMove;
            TabController.PageDragEnd += TabController_PageDragEnd;
            TabController.PageDragQuit += TabController_PageDragEnd;
            cmenTab = new ContextMenuStrip(components);
            cmenTabFullscreen = new ToolStripMenuItem();
            cmenTabFullscreen.Click += cmenTabFullscreen_Click;
            cmenTabSmartSize = new ToolStripMenuItem();
            cmenTabSmartSize.Click += cmenTabSmartSize_Click;
            cmenTabViewOnly = new ToolStripMenuItem();
            cmenTabViewOnly.Click += cmenTabViewOnly_Click;
            ToolStripSeparator1 = new ToolStripSeparator();
            cmenTabScreenshot = new ToolStripMenuItem();
            cmenTabScreenshot.Click += cmenTabScreenshot_Click;
            cmenTabStartChat = new ToolStripMenuItem();
            cmenTabStartChat.Click += cmenTabStartChat_Click;
            cmenTabTransferFile = new ToolStripMenuItem();
            cmenTabTransferFile.Click += cmenTabTransferFile_Click;
            cmenTabRefreshScreen = new ToolStripMenuItem();
            cmenTabRefreshScreen.Click += cmenTabRefreshScreen_Click;
            cmenTabSendSpecialKeys = new ToolStripMenuItem();
            cmenTabSendSpecialKeysCtrlAltDel = new ToolStripMenuItem();
            cmenTabSendSpecialKeysCtrlAltDel.Click += cmenTabSendSpecialKeysCtrlAltDel_Click;
            cmenTabSendSpecialKeysCtrlEsc = new ToolStripMenuItem();
            cmenTabSendSpecialKeysCtrlEsc.Click += cmenTabSendSpecialKeysCtrlEsc_Click;
            cmenTabExternalApps = new ToolStripMenuItem();
            cmenTabSep1 = new ToolStripSeparator();
            cmenTabRenameTab = new ToolStripMenuItem();
            cmenTabRenameTab.Click += cmenTabRenameTab_Click;
            cmenTabDuplicateTab = new ToolStripMenuItem();
            cmenTabDuplicateTab.Click += cmenTabDuplicateTab_Click;
            cmenTabReconnect = new ToolStripMenuItem();
            cmenTabReconnect.Click += cmenTabReconnect_Click;
            cmenTabDisconnect = new ToolStripMenuItem();
            cmenTabDisconnect.Click += cmenTabDisconnect_Click;
            cmenTabPuttySettings = new ToolStripMenuItem();
            cmenTabPuttySettings.Click += cmenTabPuttySettings_Click;
            cmenTab.SuspendLayout();
            SuspendLayout();
            //
            //TabController
            //
            TabController.Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom)
                | AnchorStyles.Left)
                | AnchorStyles.Right;
            TabController.Appearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiDocument;
            TabController.Cursor = Cursors.Hand;
            TabController.DragFromControl = false;
            TabController.IDEPixelArea = true;
            TabController.IDEPixelBorder = false;
            TabController.Location = new Point(0, -1);
            TabController.Name = "TabController";
            TabController.Size = new Size(632, 454);
            TabController.TabIndex = 0;
            //
            //cmenTab
            //
            cmenTab.Items.AddRange(new ToolStripItem[] { cmenTabFullscreen, cmenTabSmartSize, cmenTabViewOnly, ToolStripSeparator1, cmenTabScreenshot, cmenTabStartChat, cmenTabTransferFile, cmenTabRefreshScreen, cmenTabSendSpecialKeys, cmenTabPuttySettings, cmenTabExternalApps, cmenTabSep1, cmenTabRenameTab, cmenTabDuplicateTab, cmenTabReconnect, cmenTabDisconnect });
            cmenTab.Name = "cmenTab";
            cmenTab.RenderMode = ToolStripRenderMode.Professional;
            cmenTab.Size = new Size(202, 346);
            //
            //cmenTabFullscreen
            //
            cmenTabFullscreen.Image = Resources.arrow_out;
            cmenTabFullscreen.Name = "cmenTabFullscreen";
            cmenTabFullscreen.Size = new Size(201, 22);
            cmenTabFullscreen.Text = @"Fullscreen (RDP)";
            //
            //cmenTabSmartSize
            //
            cmenTabSmartSize.Image = Resources.SmartSize;
            cmenTabSmartSize.Name = "cmenTabSmartSize";
            cmenTabSmartSize.Size = new Size(201, 22);
            cmenTabSmartSize.Text = @"SmartSize (RDP/VNC)";
            //
            //cmenTabViewOnly
            //
            cmenTabViewOnly.Name = "cmenTabViewOnly";
            cmenTabViewOnly.Size = new Size(201, 22);
            cmenTabViewOnly.Text = @"View Only (VNC)";
            //
            //ToolStripSeparator1
            //
            ToolStripSeparator1.Name = "ToolStripSeparator1";
            ToolStripSeparator1.Size = new Size(198, 6);
            //
            //cmenTabScreenshot
            //
            cmenTabScreenshot.Image = Resources.Screenshot_Add;
            cmenTabScreenshot.Name = "cmenTabScreenshot";
            cmenTabScreenshot.Size = new Size(201, 22);
            cmenTabScreenshot.Text = @"Screenshot";
            //
            //cmenTabStartChat
            //
            cmenTabStartChat.Image = Resources.Chat;
            cmenTabStartChat.Name = "cmenTabStartChat";
            cmenTabStartChat.Size = new Size(201, 22);
            cmenTabStartChat.Text = @"Start Chat (VNC)";
            cmenTabStartChat.Visible = false;
            //
            //cmenTabTransferFile
            //
            cmenTabTransferFile.Image = Resources.SSHTransfer;
            cmenTabTransferFile.Name = "cmenTabTransferFile";
            cmenTabTransferFile.Size = new Size(201, 22);
            cmenTabTransferFile.Text = @"Transfer File (SSH)";
            //
            //cmenTabRefreshScreen
            //
            cmenTabRefreshScreen.Image = Resources.Refresh;
            cmenTabRefreshScreen.Name = "cmenTabRefreshScreen";
            cmenTabRefreshScreen.Size = new Size(201, 22);
            cmenTabRefreshScreen.Text = @"Refresh Screen (VNC)";
            //
            //cmenTabSendSpecialKeys
            //
            cmenTabSendSpecialKeys.DropDownItems.AddRange(new ToolStripItem[] { cmenTabSendSpecialKeysCtrlAltDel, cmenTabSendSpecialKeysCtrlEsc });
            cmenTabSendSpecialKeys.Image = Resources.Keyboard;
            cmenTabSendSpecialKeys.Name = "cmenTabSendSpecialKeys";
            cmenTabSendSpecialKeys.Size = new Size(201, 22);
            cmenTabSendSpecialKeys.Text = @"Send special Keys (VNC)";
            //
            //cmenTabSendSpecialKeysCtrlAltDel
            //
            cmenTabSendSpecialKeysCtrlAltDel.Name = "cmenTabSendSpecialKeysCtrlAltDel";
            cmenTabSendSpecialKeysCtrlAltDel.Size = new Size(141, 22);
            cmenTabSendSpecialKeysCtrlAltDel.Text = @"Ctrl+Alt+Del";
            //
            //cmenTabSendSpecialKeysCtrlEsc
            //
            cmenTabSendSpecialKeysCtrlEsc.Name = "cmenTabSendSpecialKeysCtrlEsc";
            cmenTabSendSpecialKeysCtrlEsc.Size = new Size(141, 22);
            cmenTabSendSpecialKeysCtrlEsc.Text = @"Ctrl+Esc";
            //
            //cmenTabExternalApps
            //
            cmenTabExternalApps.Image = (Image)(resources.GetObject("cmenTabExternalApps.Image"));
            cmenTabExternalApps.Name = "cmenTabExternalApps";
            cmenTabExternalApps.Size = new Size(201, 22);
            cmenTabExternalApps.Text = @"External Applications";
            //
            //cmenTabSep1
            //
            cmenTabSep1.Name = "cmenTabSep1";
            cmenTabSep1.Size = new Size(198, 6);
            //
            //cmenTabRenameTab
            //
            cmenTabRenameTab.Image = Resources.Rename;
            cmenTabRenameTab.Name = "cmenTabRenameTab";
            cmenTabRenameTab.Size = new Size(201, 22);
            cmenTabRenameTab.Text = @"Rename Tab";
            //
            //cmenTabDuplicateTab
            //
            cmenTabDuplicateTab.Name = "cmenTabDuplicateTab";
            cmenTabDuplicateTab.Size = new Size(201, 22);
            cmenTabDuplicateTab.Text = @"Duplicate Tab";
            //
            //cmenTabReconnect
            //
            cmenTabReconnect.Image = (Image)(resources.GetObject("cmenTabReconnect.Image"));
            cmenTabReconnect.Name = "cmenTabReconnect";
            cmenTabReconnect.Size = new Size(201, 22);
            cmenTabReconnect.Text = @"Reconnect";
            //
            //cmenTabDisconnect
            //
            cmenTabDisconnect.Image = Resources.Pause;
            cmenTabDisconnect.Name = "cmenTabDisconnect";
            cmenTabDisconnect.Size = new Size(201, 22);
            cmenTabDisconnect.Text = @"Disconnect";
            //
            //cmenTabPuttySettings
            //
            cmenTabPuttySettings.Name = "cmenTabPuttySettings";
            cmenTabPuttySettings.Size = new Size(201, 22);
            cmenTabPuttySettings.Text = @"PuTTY Settings";
            //
            //Connection
            //
            ClientSize = new Size(632, 453);
            Controls.Add(TabController);
            Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Convert.ToByte(0));
            Icon = Resources.mRemote_Icon;
            Name = "Connection";
            TabText = @"UI.Window.Connection";
            Text = @"UI.Window.Connection";
            cmenTab.ResumeLayout(false);
            ResumeLayout(false);

        }
        #endregion

        #region Public Methods
        public ConnectionWindow(DockContent Panel, string FormText = "")
        {
            if (FormText == "")
            {
                FormText = Language.strNewPanel;
            }

            WindowType = WindowType.Connection;
            DockPnl = Panel;
            InitializeComponent();
            // ReSharper disable once VirtualMemberCallInConstructor
            Text = FormText;
            TabText = FormText;
        }

        public Crownwood.Magic.Controls.TabPage AddConnectionTab(ConnectionInfo conI)
        {
            try
            {
                Crownwood.Magic.Controls.TabPage nTab = new Crownwood.Magic.Controls.TabPage();
                nTab.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

                if (Settings.Default.ShowProtocolOnTabs)
                {
                    nTab.Title = conI.Protocol.ToString() + @": ";
                }
                else
                {
                    nTab.Title = "";
                }

                nTab.Title += conI.Name;

                if (Settings.Default.ShowLogonInfoOnTabs)
                {
                    nTab.Title += @" (";

                    if (conI.Domain != "")
                    {
                        nTab.Title += conI.Domain;
                    }

                    if (conI.Username != "")
                    {
                        if (conI.Domain != "")
                        {
                            nTab.Title += @"\";
                        }

                        nTab.Title += conI.Username;
                    }

                    nTab.Title += @")";
                }

                nTab.Title = nTab.Title.Replace("&", "&&");

                Icon conIcon = ConnectionIcon.FromString(conI.Icon);
                if (conIcon != null)
                {
                    nTab.Icon = conIcon;
                }

                if (Settings.Default.OpenTabsRightOfSelected)
                {
                    TabController.TabPages.Insert(TabController.SelectedIndex + 1, nTab);
                }
                else
                {
                    TabController.TabPages.Add(nTab);
                }

                nTab.Selected = true;
                _ignoreChangeSelectedTabClick = false;

                return nTab;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "AddConnectionTab (UI.Window.ConnectionWindow) failed" + Environment.NewLine + ex.Message, true);
            }

            return null;
        }
        #endregion

        #region Private Methods
        public void UpdateSelectedConnection()
        {
            if (TabController.SelectedTab == null)
            {
                frmMain.Default.SelectedConnection = null;
            }
            else
            {
                InterfaceControl interfaceControl = TabController.SelectedTab.Tag as InterfaceControl;
                if (interfaceControl == null)
                {
                    frmMain.Default.SelectedConnection = null;
                }
                else
                {
                    frmMain.Default.SelectedConnection = interfaceControl.Info;
                }
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
                    frmMain.Default.ResizeBegin -= Connection_ResizeBegin;
                    frmMain.Default.ResizeEnd -= Connection_ResizeEnd;
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
                frmMain.Default.ResizeBegin += Connection_ResizeBegin;
                frmMain.Default.ResizeEnd += Connection_ResizeEnd;
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
            if (!frmMain.Default.IsClosing &&
                ((Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.All & TabController.TabPages.Count > 0) ||
                (Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.Multiple & TabController.TabPages.Count > 1)))
            {
                DialogResult result = CTaskDialog.MessageBox(this, GeneralAppInfo.ProdName, string.Format(Language.strConfirmCloseConnectionPanelMainInstruction, Text), "", "", "", Language.strCheckboxDoNotShowThisMessageAgain, ETaskDialogButtons.YesNo, ESysIcons.Question, ESysIcons.Question);
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
                foreach (Crownwood.Magic.Controls.TabPage tabP in TabController.TabPages)
                {
                    if (tabP.Tag != null)
                    {
                        InterfaceControl interfaceControl = (InterfaceControl)tabP.Tag;
                        interfaceControl.Protocol.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "UI.Window.Connection.Connection_FormClosing() failed" + Environment.NewLine + ex.Message, true);
            }
        }

        private EventHandler ResizeBeginEvent;
        public new event EventHandler ResizeBegin
        {
            add
            {
                ResizeBeginEvent = (EventHandler)Delegate.Combine(ResizeBeginEvent, value);
            }
            remove
            {
                ResizeBeginEvent = (EventHandler)Delegate.Remove(ResizeBeginEvent, value);
            }
        }

        private void Connection_ResizeBegin(object sender, EventArgs e)
        {
            ResizeBeginEvent?.Invoke(this, e);
        }

        private EventHandler ResizeEndEvent;
        public new event EventHandler ResizeEnd
        {
            add
            {
                ResizeEndEvent = (EventHandler)Delegate.Combine(ResizeEndEvent, value);
            }
            remove
            {
                ResizeEndEvent = (EventHandler)Delegate.Remove(ResizeEndEvent, value);
            }
        }

        private void Connection_ResizeEnd(object sender, EventArgs e)
        {
            ResizeEndEvent?.Invoke(sender, e);
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
                Crownwood.Magic.Controls.TabPage selectedTab = TabController.SelectedTab;
                if (Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.All)
                {
                    DialogResult result = CTaskDialog.MessageBox(this, GeneralAppInfo.ProdName, string.Format(Language.strConfirmCloseConnectionMainInstruction, selectedTab.Title), "", "", "", Language.strCheckboxDoNotShowThisMessageAgain, ETaskDialogButtons.YesNo, ESysIcons.Question, ESysIcons.Question);
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
                    InterfaceControl interfaceControl = (InterfaceControl)selectedTab.Tag;
                    interfaceControl.Protocol.Close();
                }
                else
                {
                    CloseTab(selectedTab);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "UI.Window.Connection.CloseConnectionTab() failed" + Environment.NewLine + ex.Message, true);
            }

            UpdateSelectedConnection();
        }

        private void TabController_DoubleClickTab(Crownwood.Magic.Controls.TabControl sender, Crownwood.Magic.Controls.TabPage page)
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
            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", true))
            {
                ConnectionInitiator.OpenConnection((ConnectionInfo)((TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode", true)).Tag, this, ConnectionInfo.Force.DoNotJump);
            }
        }

        private void TabController_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", true))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void TabController_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        #endregion
        #endregion

        #region Tab Menu
        private void ShowHideMenuButtons()
        {
            try
            {
                InterfaceControl IC = (InterfaceControl)TabController.SelectedTab?.Tag;

                if (IC == null)
                {
                    return;
                }

                if (IC.Info.Protocol == ProtocolType.RDP)
                {
                    ProtocolRDP rdp = (ProtocolRDP)IC.Protocol;

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

                if (IC.Info.Protocol == ProtocolType.VNC)
                {
                    cmenTabSendSpecialKeys.Visible = true;
                    cmenTabViewOnly.Visible = true;

                    cmenTabSmartSize.Visible = true;
                    cmenTabStartChat.Visible = true;
                    cmenTabRefreshScreen.Visible = true;
                    cmenTabTransferFile.Visible = false;

                    ProtocolVNC vnc = (ProtocolVNC)IC.Protocol;
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

                if (IC.Info.Protocol == ProtocolType.SSH1 | IC.Info.Protocol == ProtocolType.SSH2)
                {
                    cmenTabTransferFile.Visible = true;
                }

                if (IC.Protocol is PuttyBase)
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
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "ShowHideMenuButtons (UI.Window.ConnectionWindow) failed" + Environment.NewLine + ex.Message, true);
            }
        }

        private void cmenTabScreenshot_Click(object sender, EventArgs e)
        {
            cmenTab.Close();
            Application.DoEvents();
            Windows.screenshotForm.AddScreenshot(Tools.MiscTools.TakeScreenshot(this));
        }

        private void cmenTabSmartSize_Click(object sender, EventArgs e)
        {
            ToggleSmartSize();
        }

        private void cmenTabReconnect_Click(object sender, EventArgs e)
        {
            Reconnect();
        }

        private void cmenTabTransferFile_Click(object sender, EventArgs e)
        {
            TransferFile();
        }

        private void cmenTabViewOnly_Click(object sender, EventArgs e)
        {
            ToggleViewOnly();
        }

        private void cmenTabStartChat_Click(object sender, EventArgs e)
        {
            StartChat();
        }

        private void cmenTabRefreshScreen_Click(object sender, EventArgs e)
        {
            RefreshScreen();
        }

        private void cmenTabSendSpecialKeysCtrlAltDel_Click(object sender, EventArgs e)
        {
            SendSpecialKeys(ProtocolVNC.SpecialKeys.CtrlAltDel);
        }

        private void cmenTabSendSpecialKeysCtrlEsc_Click(object sender, EventArgs e)
        {
            SendSpecialKeys(ProtocolVNC.SpecialKeys.CtrlEsc);
        }

        private void cmenTabFullscreen_Click(object sender, EventArgs e)
        {
            ToggleFullscreen();
        }

        private void cmenTabPuttySettings_Click(object sender, EventArgs e)
        {
            ShowPuttySettingsDialog();
        }

        private void cmenTabExternalAppsEntry_Click(object sender, EventArgs e)
        {
            StartExternalApp((Tools.ExternalTool)((ToolStripMenuItem)sender).Tag);
        }

        private void cmenTabDisconnect_Click(object sender, EventArgs e)
        {
            CloseTabMenu();
        }

        private void cmenTabDuplicateTab_Click(object sender, EventArgs e)
        {
            DuplicateTab();
        }

        private void cmenTabRenameTab_Click(object sender, EventArgs e)
        {
            RenameTab();
        }
        #endregion

        #region Tab Actions
        private void ToggleSmartSize()
        {
            try
            {
                if (TabController.SelectedTab?.Tag is InterfaceControl)
                {
                    InterfaceControl IC = (InterfaceControl)TabController.SelectedTab.Tag;

                    var protocol = IC.Protocol as ProtocolRDP;
                    if (protocol != null)
                    {
                        ProtocolRDP rdp = protocol;
                        rdp.ToggleSmartSize();
                    }
                    else if (IC.Protocol is ProtocolVNC)
                    {
                        ProtocolVNC vnc = (ProtocolVNC)IC.Protocol;
                        vnc.ToggleSmartSize();
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "ToggleSmartSize (UI.Window.ConnectionWindow) failed" + Environment.NewLine + ex.Message, true);
            }
        }

        private void TransferFile()
        {
            try
            {
                if (TabController.SelectedTab?.Tag is InterfaceControl)
                {
                    InterfaceControl IC = (InterfaceControl)TabController.SelectedTab.Tag;

                    if (IC.Info.Protocol == ProtocolType.SSH1 | IC.Info.Protocol == ProtocolType.SSH2)
                    {
                        SSHTransferFile();
                    }
                    else if (IC.Info.Protocol == ProtocolType.VNC)
                    {
                        VNCTransferFile();
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "TransferFile (UI.Window.ConnectionWindow) failed" + Environment.NewLine + ex.Message, true);
            }
        }

        private void SSHTransferFile()
        {
            try
            {

                InterfaceControl IC = (InterfaceControl)TabController.SelectedTab.Tag;

                Windows.Show(WindowType.SSHTransfer);

                ConnectionInfo conI = IC.Info;

                Windows.sshtransferForm.Hostname = conI.Hostname;
                Windows.sshtransferForm.Username = conI.Username;
                Windows.sshtransferForm.Password = conI.Password;
                Windows.sshtransferForm.Port = Convert.ToString(conI.Port);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "SSHTransferFile (UI.Window.ConnectionWindow) failed" + Environment.NewLine + ex.Message, true);
            }
        }

        private void VNCTransferFile()
        {
            try
            {
                InterfaceControl IC = (InterfaceControl)TabController.SelectedTab.Tag;
                ProtocolVNC vnc = (ProtocolVNC)IC.Protocol;
                vnc.StartFileTransfer();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "VNCTransferFile (UI.Window.ConnectionWindow) failed" + Environment.NewLine + ex.Message, true);
            }
        }

        private void ToggleViewOnly()
        {
            try
            {
                if (TabController.SelectedTab?.Tag is InterfaceControl)
                {
                    InterfaceControl IC = (InterfaceControl)TabController.SelectedTab.Tag;

                    if (IC.Protocol is ProtocolVNC)
                    {
                        cmenTabViewOnly.Checked = !cmenTabViewOnly.Checked;

                        ProtocolVNC vnc = (ProtocolVNC)IC.Protocol;
                        vnc.ToggleViewOnly();
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "ToggleViewOnly (UI.Window.ConnectionWindow) failed" + Environment.NewLine + ex.Message, true);
            }
        }

        private void StartChat()
        {
            try
            {
                InterfaceControl IC = TabController.SelectedTab?.Tag as InterfaceControl;

                if (IC?.Protocol is ProtocolVNC)
                {
                    ProtocolVNC vnc = (ProtocolVNC)IC.Protocol;
                    vnc.StartChat();
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "StartChat (UI.Window.ConnectionWindow) failed" + Environment.NewLine + ex.Message, true);
            }
        }

        private void RefreshScreen()
        {
            try
            {
                InterfaceControl IC = TabController.SelectedTab?.Tag as InterfaceControl;

                if (IC?.Protocol is ProtocolVNC)
                {
                    ProtocolVNC vnc = (ProtocolVNC)IC.Protocol;
                    vnc.RefreshScreen();
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "RefreshScreen (UI.Window.ConnectionWindow) failed" + Environment.NewLine + ex.Message, true);
            }
        }

        private void SendSpecialKeys(ProtocolVNC.SpecialKeys Keys)
        {
            try
            {
                InterfaceControl IC = TabController.SelectedTab?.Tag as InterfaceControl;

                if (IC?.Protocol is ProtocolVNC)
                {
                    ProtocolVNC vnc = (ProtocolVNC)IC.Protocol;
                    vnc.SendSpecialKeys(Keys);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "SendSpecialKeys (UI.Window.ConnectionWindow) failed" + Environment.NewLine + ex.Message, true);
            }
        }

        private void ToggleFullscreen()
        {
            try
            {
                InterfaceControl IC = TabController.SelectedTab?.Tag as InterfaceControl;

                if (IC?.Protocol is ProtocolRDP)
                {
                    ProtocolRDP rdp = (ProtocolRDP)IC.Protocol;
                    rdp.ToggleFullscreen();
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "ToggleFullscreen (UI.Window.ConnectionWindow) failed" + Environment.NewLine + ex.Message, true);
            }
        }

        private void ShowPuttySettingsDialog()
        {
            try
            {
                InterfaceControl objInterfaceControl = TabController.SelectedTab?.Tag as InterfaceControl;

                if (objInterfaceControl?.Protocol is PuttyBase)
                {
                    PuttyBase objPuttyBase = (PuttyBase)objInterfaceControl.Protocol;

                    objPuttyBase.ShowSettingsDialog();
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "ShowPuttySettingsDialog (UI.Window.ConnectionWindow) failed" + Environment.NewLine + ex.Message, true);
            }
        }

        private void AddExternalApps()
        {
            try
            {
                //clean up
                //since new items are added below, we have to dispose of any previous items first
                if (cmenTabExternalApps.DropDownItems.Count > 0)
                {
                    for (int i = cmenTabExternalApps.DropDownItems.Count - 1; i >= 0; i--)
                        cmenTabExternalApps.DropDownItems[i].Dispose();

                    cmenTabExternalApps.DropDownItems.Clear();
                }


                //add ext apps
                foreach (Tools.ExternalTool extA in Runtime.ExternalTools)
                {
                    ToolStripMenuItem nItem = new ToolStripMenuItem
                    {
                        Text = extA.DisplayName,
                        Tag = extA,
                        /* rare failure here. While ExternalTool.Image already tries to default this
                         * try again so it's not null/doesn't crash.
                         */
                        Image = extA.Image ?? Resources.mRemote_Icon.ToBitmap()
                    };


                    nItem.Click += cmenTabExternalAppsEntry_Click;

                    cmenTabExternalApps.DropDownItems.Add(nItem);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("cMenTreeTools_DropDownOpening failed (UI.Window.ConnectionWindow)", ex);
            }
        }

        private void StartExternalApp(Tools.ExternalTool ExtA)
        {
            try
            {
                if (TabController.SelectedTab?.Tag is InterfaceControl)
                {
                    InterfaceControl IC = (InterfaceControl)TabController.SelectedTab.Tag;
                    ExtA.Start(IC.Info);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "cmenTabExternalAppsEntry_Click failed (UI.Window.ConnectionWindow)" + Environment.NewLine + ex.Message, true);
            }
        }

        private void CloseTabMenu()
        {
            try
            {
                if (TabController.SelectedTab?.Tag is InterfaceControl)
                {
                    InterfaceControl IC = (InterfaceControl)TabController.SelectedTab.Tag;
                    IC.Protocol.Close();
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "CloseTabMenu (UI.Window.ConnectionWindow) failed" + Environment.NewLine + ex.Message, true);
            }
        }

        private void DuplicateTab()
        {
            try
            {
                if (TabController.SelectedTab?.Tag is InterfaceControl)
                {
                    InterfaceControl IC = (InterfaceControl)TabController.SelectedTab.Tag;
                    ConnectionInitiator.OpenConnection(IC.Info, ConnectionInfo.Force.DoNotJump);
                    _ignoreChangeSelectedTabClick = false;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "DuplicateTab (UI.Window.ConnectionWindow) failed" + Environment.NewLine + ex.Message, true);
            }
        }

        private void Reconnect()
        {
            try
            {
                if (TabController.SelectedTab?.Tag is InterfaceControl)
                {
                    InterfaceControl IC = (InterfaceControl)TabController.SelectedTab.Tag;
                    IC.Protocol.Close();
                    ConnectionInitiator.OpenConnection(IC.Info, ConnectionInfo.Force.DoNotJump);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Reconnect (UI.Window.ConnectionWindow) failed" + Environment.NewLine + ex.Message, true);
            }
        }

        private void RenameTab()
        {
            try
            {
                string nTitle = "";

                if (input.InputBox(Language.strNewTitle, Language.strNewTitle + ":", ref nTitle) == DialogResult.OK && !string.IsNullOrEmpty(nTitle))
                {
                    TabController.SelectedTab.Title = nTitle.Replace("&", "&&");
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "RenameTab (UI.Window.ConnectionWindow) failed" + Environment.NewLine + ex.Message, true);
            }
        }
        #endregion

        #region Protocols
        public void Prot_Event_Closed(object sender)
        {
            ProtocolBase Prot = (ProtocolBase)sender;
            CloseTab((Crownwood.Magic.Controls.TabPage)Prot.InterfaceControl.Parent);
        }
        #endregion

        #region Tabs
        private delegate void CloseTabCB(Crownwood.Magic.Controls.TabPage TabToBeClosed);
        private void CloseTab(Crownwood.Magic.Controls.TabPage TabToBeClosed)
        {
            if (TabController.InvokeRequired)
            {
                CloseTabCB s = CloseTab;

                try
                {
                    TabController.Invoke(s, TabToBeClosed);
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    TabController.Invoke(s, TabToBeClosed);
                }
                catch (Exception ex)
                {
                    Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn\'t close tab" + Environment.NewLine + ex.Message, true);
                }
            }
            else
            {
                try
                {
                    TabController.TabPages.Remove(TabToBeClosed);
                    _ignoreChangeSelectedTabClick = false;
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    CloseTab(TabToBeClosed);
                }
                catch (Exception ex)
                {
                    Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn\'t close tab" + Environment.NewLine + ex.Message, true);
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
            FocusIC();
            RefreshIC();
        }

        private int _firstClickTicks;
        private Rectangle _doubleClickRectangle;
        private void TabController_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (!(NativeMethods.GetForegroundWindow() == frmMain.Default.Handle) && !_ignoreChangeSelectedTabClick)
                {
                    Crownwood.Magic.Controls.TabPage clickedTab = TabController.TabPageFromPoint(e.Location);
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
                        int currentTicks = Environment.TickCount;
                        int elapsedTicks = currentTicks - _firstClickTicks;
                        if (elapsedTicks > SystemInformation.DoubleClickTime || !_doubleClickRectangle.Contains(MousePosition))
                        {
                            _firstClickTicks = currentTicks;
                            _doubleClickRectangle = new Rectangle(MousePosition.X - (SystemInformation.DoubleClickSize.Width / 2), MousePosition.Y - (SystemInformation.DoubleClickSize.Height / 2), SystemInformation.DoubleClickSize.Width, SystemInformation.DoubleClickSize.Height);
                            FocusIC();
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
                        ShowHideMenuButtons();
                        NativeMethods.SetForegroundWindow(Handle);
                        cmenTab.Show(TabController, e.Location);
                        break;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "TabController_MouseUp (UI.Window.ConnectionWindow) failed" + Environment.NewLine + ex.Message, true);
            }
        }

        private void FocusIC()
        {
            try
            {
                if (TabController.SelectedTab != null)
                {
                    if (TabController.SelectedTab.Tag is InterfaceControl)
                    {
                        InterfaceControl IC = (InterfaceControl)TabController.SelectedTab.Tag;
                        IC.Protocol.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "FocusIC (UI.Window.ConnectionWindow) failed" + Environment.NewLine + ex.Message, true);
            }
        }

        public void RefreshIC()
        {
            try
            {
                if (TabController.SelectedTab != null)
                {
                    if (TabController.SelectedTab.Tag is InterfaceControl)
                    {
                        InterfaceControl IC = (InterfaceControl)TabController.SelectedTab.Tag;

                        if (IC.Info.Protocol == ProtocolType.VNC)
                        {
                            ((ProtocolVNC)IC.Protocol).RefreshScreen();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "RefreshIC (UI.Window.Connection) failed" + Environment.NewLine + ex.Message, true);
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
                    Crownwood.Magic.Controls.TabPage selectedTab = TabController.SelectedTab;
                    if (selectedTab != null)
                    {
                        Rectangle tabClientRectangle = selectedTab.RectangleToScreen(selectedTab.ClientRectangle);
                        if (tabClientRectangle.Contains(MousePosition))
                        {
                            InterfaceControl interfaceControl = TabController.SelectedTab.Tag as InterfaceControl;
                            if (interfaceControl != null && interfaceControl.Info != null)
                            {
                                if (interfaceControl.Info.Protocol == ProtocolType.RDP)
                                {
                                    interfaceControl.Protocol.Focus();
                                    return; // Do not pass to base class
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(message: "UI.Window.Connection.WndProc() failed.", ex: ex, logOnly: true);
            }

            base.WndProc(ref m);
        }
        #endregion

        #region Tab drag and drop
        private bool _InTabDrag;
        public bool InTabDrag
        {
            get
            {
                return _InTabDrag;
            }
            set
            {
                _InTabDrag = value;
            }
        }
        private void TabController_PageDragStart(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.SizeWE;
        }

        private void TabController_PageDragMove(object sender, MouseEventArgs e)
        {
            InTabDrag = true; // For some reason PageDragStart gets raised again after PageDragEnd so set this here instead

            Crownwood.Magic.Controls.TabPage sourceTab = TabController.SelectedTab;
            Crownwood.Magic.Controls.TabPage destinationTab = TabController.TabPageFromPoint(e.Location);

            if (!TabController.TabPages.Contains(destinationTab) || sourceTab == destinationTab)
            {
                return;
            }

            int targetIndex = TabController.TabPages.IndexOf(destinationTab);

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
            InterfaceControl interfaceControl = TabController.SelectedTab.Tag as InterfaceControl;
            interfaceControl?.Protocol.Focus();
        }
        #endregion
    }
}
