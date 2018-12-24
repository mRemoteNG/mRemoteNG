﻿using System;
using System.Drawing;
using System.Windows.Forms;
using TabControl = Crownwood.Magic.Controls.TabControl;


namespace mRemoteNG.UI.Window
{
    public partial class ConnectionWindow
    {
        internal ContextMenuStrip cmenTab;
        private ToolStripMenuItem cmenTabFullscreen;
        private ToolStripMenuItem cmenTabScreenshot;
        private ToolStripMenuItem cmenTabTransferFile;
        private ToolStripMenuItem cmenTabSendSpecialKeys;
        private ToolStripSeparator cmenTabSep1;
        private ToolStripMenuItem cmenTabRenameTab;
        private ToolStripMenuItem cmenTabDuplicateTab;
        private ToolStripMenuItem cmenTabDisconnect;
        private ToolStripMenuItem cmenTabDisconnectOthers;
        private ToolStripMenuItem cmenTabDisconnectOthersRight;
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectionWindow));
            this.TabController = new Crownwood.Magic.Controls.TabControl();
            this.cmenTab = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmenTabFullscreen = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabSmartSize = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabViewOnly = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmenTabScreenshot = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabStartChat = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabTransferFile = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabRefreshScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabSendSpecialKeys = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabSendSpecialKeysCtrlAltDel = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabSendSpecialKeysCtrlEsc = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabPuttySettings = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabExternalApps = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmenTabRenameTab = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabDuplicateTab = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabReconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabDisconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabDisconnectOthers = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabDisconnectOthersRight = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabController
            // 
            this.TabController.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TabController.Appearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiDocument;
            this.TabController.Cursor = System.Windows.Forms.Cursors.Hand;
            this.TabController.DragFromControl = false;
            this.TabController.IDEPixelArea = true;
            this.TabController.IDEPixelBorder = false;
            this.TabController.Location = new System.Drawing.Point(0, -1);
            this.TabController.Name = "TabController";
            this.TabController.Size = new System.Drawing.Size(632, 454);
            this.TabController.TabIndex = 0;
            // 
            // cmenTab
            // 
            this.cmenTab.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmenTabReconnect,
            this.cmenTabFullscreen,
            this.cmenTabSmartSize,
            this.cmenTabViewOnly,
            this.ToolStripSeparator1,
            this.cmenTabScreenshot,
            this.cmenTabStartChat,
            this.cmenTabTransferFile,
            this.cmenTabRefreshScreen,
            this.cmenTabSendSpecialKeys,
            this.cmenTabPuttySettings,
            this.cmenTabExternalApps,
            this.cmenTabSep1,
            this.cmenTabRenameTab,
            this.cmenTabDuplicateTab,
            this.cmenTabDisconnect,
            this.cmenTabDisconnectOthers,
            this.cmenTabDisconnectOthersRight});
            this.cmenTab.Name = "cmenTab";
            this.cmenTab.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.cmenTab.Size = new System.Drawing.Size(231, 390);
            // 
            // cmenTabFullscreen
            // 
            this.cmenTabFullscreen.Image = global::mRemoteNG.Resources.arrow_out;
            this.cmenTabFullscreen.Name = "cmenTabFullscreen";
            this.cmenTabFullscreen.Size = new System.Drawing.Size(230, 22);
            this.cmenTabFullscreen.Text = "Fullscreen (RDP)";
            // 
            // cmenTabSmartSize
            // 
            this.cmenTabSmartSize.Image = global::mRemoteNG.Resources.SmartSize;
            this.cmenTabSmartSize.Name = "cmenTabSmartSize";
            this.cmenTabSmartSize.Size = new System.Drawing.Size(230, 22);
            this.cmenTabSmartSize.Text = "SmartSize (RDP/VNC)";
            // 
            // cmenTabViewOnly
            // 
            this.cmenTabViewOnly.Name = "cmenTabViewOnly";
            this.cmenTabViewOnly.Size = new System.Drawing.Size(230, 22);
            this.cmenTabViewOnly.Text = "View Only (VNC)";
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(227, 6);
            // 
            // cmenTabScreenshot
            // 
            this.cmenTabScreenshot.Image = global::mRemoteNG.Resources.Screenshot_Add;
            this.cmenTabScreenshot.Name = "cmenTabScreenshot";
            this.cmenTabScreenshot.Size = new System.Drawing.Size(230, 22);
            this.cmenTabScreenshot.Text = "Screenshot";
            // 
            // cmenTabStartChat
            // 
            this.cmenTabStartChat.Image = global::mRemoteNG.Resources.Chat;
            this.cmenTabStartChat.Name = "cmenTabStartChat";
            this.cmenTabStartChat.Size = new System.Drawing.Size(230, 22);
            this.cmenTabStartChat.Text = "Start Chat (VNC)";
            this.cmenTabStartChat.Visible = false;
            // 
            // cmenTabTransferFile
            // 
            this.cmenTabTransferFile.Image = global::mRemoteNG.Resources.SSHTransfer;
            this.cmenTabTransferFile.Name = "cmenTabTransferFile";
            this.cmenTabTransferFile.Size = new System.Drawing.Size(230, 22);
            this.cmenTabTransferFile.Text = "Transfer File (SSH)";
            // 
            // cmenTabRefreshScreen
            // 
            this.cmenTabRefreshScreen.Image = global::mRemoteNG.Resources.Refresh;
            this.cmenTabRefreshScreen.Name = "cmenTabRefreshScreen";
            this.cmenTabRefreshScreen.Size = new System.Drawing.Size(230, 22);
            this.cmenTabRefreshScreen.Text = "Refresh Screen (VNC)";
            // 
            // cmenTabSendSpecialKeys
            // 
            this.cmenTabSendSpecialKeys.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmenTabSendSpecialKeysCtrlAltDel,
            this.cmenTabSendSpecialKeysCtrlEsc});
            this.cmenTabSendSpecialKeys.Image = global::mRemoteNG.Resources.Keyboard;
            this.cmenTabSendSpecialKeys.Name = "cmenTabSendSpecialKeys";
            this.cmenTabSendSpecialKeys.Size = new System.Drawing.Size(230, 22);
            this.cmenTabSendSpecialKeys.Text = "Send special Keys (VNC)";
            // 
            // cmenTabSendSpecialKeysCtrlAltDel
            // 
            this.cmenTabSendSpecialKeysCtrlAltDel.Name = "cmenTabSendSpecialKeysCtrlAltDel";
            this.cmenTabSendSpecialKeysCtrlAltDel.Size = new System.Drawing.Size(141, 22);
            this.cmenTabSendSpecialKeysCtrlAltDel.Text = "Ctrl+Alt+Del";
            // 
            // cmenTabSendSpecialKeysCtrlEsc
            // 
            this.cmenTabSendSpecialKeysCtrlEsc.Name = "cmenTabSendSpecialKeysCtrlEsc";
            this.cmenTabSendSpecialKeysCtrlEsc.Size = new System.Drawing.Size(141, 22);
            this.cmenTabSendSpecialKeysCtrlEsc.Text = "Ctrl+Esc";
            // 
            // cmenTabPuttySettings
            // 
            this.cmenTabPuttySettings.Name = "cmenTabPuttySettings";
            this.cmenTabPuttySettings.Size = new System.Drawing.Size(230, 22);
            this.cmenTabPuttySettings.Text = "PuTTY Settings";
            // 
            // cmenTabExternalApps
            // 
            this.cmenTabExternalApps.Image = ((System.Drawing.Image)(resources.GetObject("cmenTabExternalApps.Image")));
            this.cmenTabExternalApps.Name = "cmenTabExternalApps";
            this.cmenTabExternalApps.Size = new System.Drawing.Size(230, 22);
            this.cmenTabExternalApps.Text = "External Applications";
            // 
            // cmenTabSep1
            // 
            this.cmenTabSep1.Name = "cmenTabSep1";
            this.cmenTabSep1.Size = new System.Drawing.Size(227, 6);
            // 
            // cmenTabRenameTab
            // 
            this.cmenTabRenameTab.Image = global::mRemoteNG.Resources.Rename;
            this.cmenTabRenameTab.Name = "cmenTabRenameTab";
            this.cmenTabRenameTab.Size = new System.Drawing.Size(230, 22);
            this.cmenTabRenameTab.Text = "Rename Tab";
            // 
            // cmenTabDuplicateTab
            // 
            this.cmenTabDuplicateTab.Name = "cmenTabDuplicateTab";
            this.cmenTabDuplicateTab.Size = new System.Drawing.Size(230, 22);
            this.cmenTabDuplicateTab.Text = "Duplicate Tab";
            // 
            // cmenTabReconnect
            // 
            this.cmenTabReconnect.Image = ((System.Drawing.Image)(resources.GetObject("cmenTabReconnect.Image")));
            this.cmenTabReconnect.Name = "cmenTabReconnect";
            this.cmenTabReconnect.Size = new System.Drawing.Size(230, 22);
            this.cmenTabReconnect.Text = "Reconnect";
            // 
            // cmenTabDisconnect
            // 
            this.cmenTabDisconnect.Image = global::mRemoteNG.Resources.Pause;
            this.cmenTabDisconnect.Name = "cmenTabDisconnect";
            this.cmenTabDisconnect.Size = new System.Drawing.Size(230, 22);
            this.cmenTabDisconnect.Text = "Disconnect";
            // 
            // cmenTabDisconnectOthers
            // 
            this.cmenTabDisconnectOthers.Image = global::mRemoteNG.Resources.Pause;
            this.cmenTabDisconnectOthers.Name = "cmenTabDisconnectOthers";
            this.cmenTabDisconnectOthers.Size = new System.Drawing.Size(230, 22);
            this.cmenTabDisconnectOthers.Text = "Disconnect Other Tabs";
            // 
            // cmenTabDisconnectOthersRight
            // 
            this.cmenTabDisconnectOthersRight.Image = global::mRemoteNG.Resources.Pause;
            this.cmenTabDisconnectOthersRight.Name = "cmenTabDisconnectOthersRight";
            this.cmenTabDisconnectOthersRight.Size = new System.Drawing.Size(230, 22);
            this.cmenTabDisconnectOthersRight.Text = "Disconnect Tabs To The Right";
            // 
            // ConnectionWindow
            // 
            this.ClientSize = new System.Drawing.Size(632, 453);
            this.Controls.Add(this.TabController);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::mRemoteNG.Resources.mRemote_Icon;
            this.Name = "ConnectionWindow";
            this.TabText = "UI.Window.Connection";
            this.Text = "UI.Window.Connection";
            this.cmenTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.ComponentModel.IContainer components;
    }
}