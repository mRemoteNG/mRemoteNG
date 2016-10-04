using System;
using System.Drawing;
using System.Windows.Forms;
using TabControl = Crownwood.Magic.Controls.TabControl;


namespace mRemoteNG.UI.Window
{
    public partial class ConnectionWindow
    {
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


        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectionWindow));
            TabController = new TabControl();
            cmenTab = new ContextMenuStrip(components);
            cmenTabFullscreen = new ToolStripMenuItem();
            cmenTabSmartSize = new ToolStripMenuItem();
            cmenTabViewOnly = new ToolStripMenuItem();
            ToolStripSeparator1 = new ToolStripSeparator();
            cmenTabScreenshot = new ToolStripMenuItem();
            cmenTabStartChat = new ToolStripMenuItem();
            cmenTabTransferFile = new ToolStripMenuItem();
            cmenTabRefreshScreen = new ToolStripMenuItem();
            cmenTabSendSpecialKeys = new ToolStripMenuItem();
            cmenTabSendSpecialKeysCtrlAltDel = new ToolStripMenuItem();
            cmenTabSendSpecialKeysCtrlEsc = new ToolStripMenuItem();
            cmenTabExternalApps = new ToolStripMenuItem();
            cmenTabSep1 = new ToolStripSeparator();
            cmenTabRenameTab = new ToolStripMenuItem();
            cmenTabDuplicateTab = new ToolStripMenuItem();
            cmenTabReconnect = new ToolStripMenuItem();
            cmenTabDisconnect = new ToolStripMenuItem();
            cmenTabPuttySettings = new ToolStripMenuItem();
            cmenTab.SuspendLayout();
            SuspendLayout();
            //
            //TabController
            //
            TabController.Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom)
                | AnchorStyles.Left)
                | AnchorStyles.Right;
            TabController.Appearance = TabControl.VisualAppearance.MultiDocument;
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
            cmenTab.Items.AddRange(new ToolStripItem[]
            {
                cmenTabFullscreen,
                cmenTabSmartSize,
                cmenTabViewOnly,
                ToolStripSeparator1,
                cmenTabScreenshot,
                cmenTabStartChat,
                cmenTabTransferFile,
                cmenTabRefreshScreen,
                cmenTabSendSpecialKeys,
                cmenTabPuttySettings,
                cmenTabExternalApps,
                cmenTabSep1,
                cmenTabRenameTab,
                cmenTabDuplicateTab,
                cmenTabReconnect,
                cmenTabDisconnect
            });
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
            cmenTabSendSpecialKeys.DropDownItems.AddRange(new ToolStripItem[]
            {
                cmenTabSendSpecialKeysCtrlAltDel,
                cmenTabSendSpecialKeysCtrlEsc
            });
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
    }
}