using System;
using System.Drawing;
using System.Windows.Forms;


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
        private ToolStripSeparator cmenTabSep2;
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
        private ToolStripMenuItem cmenTabPuttySettings;


        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectionWindow));
            this.connDock = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.cmenTab = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmenTabReconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabDisconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabDisconnectOthers = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabDisconnectOthersRight = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabRenameTab = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabDuplicateTab = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmenTabFullscreen = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabSmartSize = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabViewOnly = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabStartChat = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabRefreshScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabTransferFile = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabSendSpecialKeys = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabSendSpecialKeysCtrlAltDel = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabSendSpecialKeysCtrlEsc = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmenTabScreenshot = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabPuttySettings = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTabExternalApps = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenCopyAllToClipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // connDock
            // 
            this.connDock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.connDock.DockBackColor = System.Drawing.SystemColors.Control;
            this.connDock.DockLeftPortion = 230D;
            this.connDock.DockRightPortion = 230D;
            this.connDock.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingSdi;
            this.connDock.Location = new System.Drawing.Point(0, 0);
            this.connDock.Margin = new System.Windows.Forms.Padding(4);
            this.connDock.Name = "connDock";
            this.connDock.Size = new System.Drawing.Size(632, 453);
            this.connDock.TabIndex = 13;
            // 
            // cmenTab
            // 
            this.cmenTab.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmenTab.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmenTabReconnect,
            this.cmenTabDisconnect,
            this.cmenTabDisconnectOthers,
            this.cmenTabDisconnectOthersRight,
            this.cmenTabRenameTab,
            this.cmenTabDuplicateTab,
            this.cmenTabSep1,
            this.cmenTabFullscreen,
            this.cmenTabSmartSize,
            this.cmenTabViewOnly,
            this.cmenTabStartChat,
            this.cmenTabRefreshScreen,
            this.cmenTabTransferFile,
            this.cmenTabSendSpecialKeys,
            this.cmenTabSep2,
            this.cmenTabScreenshot,
            this.cmenTabPuttySettings,
            this.cmenCopyAllToClipboard,
            this.cmenTabExternalApps});
            this.cmenTab.Name = "cmenTab";
            this.cmenTab.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.cmenTab.Size = new System.Drawing.Size(276, 486);
            // 
            // cmenTabReconnect
            // 
            this.cmenTabReconnect.Image = ((System.Drawing.Image)(resources.GetObject("cmenTabReconnect.Image")));
            this.cmenTabReconnect.Name = "cmenTabReconnect";
            this.cmenTabReconnect.Size = new System.Drawing.Size(275, 26);
            this.cmenTabReconnect.Text = "Reconnect";
            // 
            // cmenTabDisconnect
            // 
            this.cmenTabDisconnect.Image = global::mRemoteNG.Resources.tab_delete;
            this.cmenTabDisconnect.Name = "cmenTabDisconnect";
            this.cmenTabDisconnect.Size = new System.Drawing.Size(275, 26);
            this.cmenTabDisconnect.Text = "Disconnect";
            // 
            // cmenTabDisconnectOthers
            // 
            this.cmenTabDisconnectOthers.Image = global::mRemoteNG.Resources.tab_delete;
            this.cmenTabDisconnectOthers.Name = "cmenTabDisconnectOthers";
            this.cmenTabDisconnectOthers.Size = new System.Drawing.Size(275, 26);
            this.cmenTabDisconnectOthers.Text = "Disconnect Other Tabs";
            // 
            // cmenTabDisconnectOthersRight
            // 
            this.cmenTabDisconnectOthersRight.Image = global::mRemoteNG.Resources.tab_delete;
            this.cmenTabDisconnectOthersRight.Name = "cmenTabDisconnectOthersRight";
            this.cmenTabDisconnectOthersRight.Size = new System.Drawing.Size(275, 26);
            this.cmenTabDisconnectOthersRight.Text = "Disconnect Tabs To The Right";
            // 
            // cmenTabRenameTab
            // 
            this.cmenTabRenameTab.Image = global::mRemoteNG.Resources.tab_edit;
            this.cmenTabRenameTab.Name = "cmenTabRenameTab";
            this.cmenTabRenameTab.Size = new System.Drawing.Size(275, 26);
            this.cmenTabRenameTab.Text = "Rename Tab";
            // 
            // cmenTabDuplicateTab
            // 
            this.cmenTabDuplicateTab.Image = global::mRemoteNG.Resources.tab_add;
            this.cmenTabDuplicateTab.Name = "cmenTabDuplicateTab";
            this.cmenTabDuplicateTab.Size = new System.Drawing.Size(275, 26);
            this.cmenTabDuplicateTab.Text = "Duplicate Tab";
            // 
            // cmenTabSep1
            // 
            this.cmenTabSep1.Name = "cmenTabSep1";
            this.cmenTabSep1.Size = new System.Drawing.Size(272, 6);
            // 
            // cmenTabFullscreen
            // 
            this.cmenTabFullscreen.Image = global::mRemoteNG.Resources.arrow_out;
            this.cmenTabFullscreen.Name = "cmenTabFullscreen";
            this.cmenTabFullscreen.Size = new System.Drawing.Size(275, 26);
            this.cmenTabFullscreen.Text = "Fullscreen (RDP)";
            // 
            // cmenTabSmartSize
            // 
            this.cmenTabSmartSize.Image = global::mRemoteNG.Resources.SmartSize;
            this.cmenTabSmartSize.Name = "cmenTabSmartSize";
            this.cmenTabSmartSize.Size = new System.Drawing.Size(275, 26);
            this.cmenTabSmartSize.Text = "SmartSize (RDP/VNC)";
            // 
            // cmenTabViewOnly
            // 
            this.cmenTabViewOnly.Image = global::mRemoteNG.Resources.View;
            this.cmenTabViewOnly.Name = "cmenTabViewOnly";
            this.cmenTabViewOnly.Size = new System.Drawing.Size(275, 26);
            this.cmenTabViewOnly.Text = "View Only (VNC)";
            // 
            // cmenTabStartChat
            // 
            this.cmenTabStartChat.Image = global::mRemoteNG.Resources.Chat;
            this.cmenTabStartChat.Name = "cmenTabStartChat";
            this.cmenTabStartChat.Size = new System.Drawing.Size(275, 26);
            this.cmenTabStartChat.Text = "Start Chat (VNC)";
            this.cmenTabStartChat.Visible = false;
            // 
            // cmenTabRefreshScreen
            // 
            this.cmenTabRefreshScreen.Image = global::mRemoteNG.Resources.Refresh;
            this.cmenTabRefreshScreen.Name = "cmenTabRefreshScreen";
            this.cmenTabRefreshScreen.Size = new System.Drawing.Size(275, 26);
            this.cmenTabRefreshScreen.Text = "Refresh Screen (VNC)";
            // 
            // cmenTabTransferFile
            // 
            this.cmenTabTransferFile.Image = global::mRemoteNG.Resources.SSHTransfer;
            this.cmenTabTransferFile.Name = "cmenTabTransferFile";
            this.cmenTabTransferFile.Size = new System.Drawing.Size(275, 26);
            this.cmenTabTransferFile.Text = "Transfer File (SSH)";
            // 
            // cmenTabSendSpecialKeys
            // 
            this.cmenTabSendSpecialKeys.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmenTabSendSpecialKeysCtrlAltDel,
            this.cmenTabSendSpecialKeysCtrlEsc});
            this.cmenTabSendSpecialKeys.Image = global::mRemoteNG.Resources.Keyboard;
            this.cmenTabSendSpecialKeys.Name = "cmenTabSendSpecialKeys";
            this.cmenTabSendSpecialKeys.Size = new System.Drawing.Size(275, 26);
            this.cmenTabSendSpecialKeys.Text = "Send special Keys (VNC)";
            // 
            // cmenTabSendSpecialKeysCtrlAltDel
            // 
            this.cmenTabSendSpecialKeysCtrlAltDel.Name = "cmenTabSendSpecialKeysCtrlAltDel";
            this.cmenTabSendSpecialKeysCtrlAltDel.Size = new System.Drawing.Size(169, 26);
            this.cmenTabSendSpecialKeysCtrlAltDel.Text = "Ctrl+Alt+Del";
            // 
            // cmenTabSendSpecialKeysCtrlEsc
            // 
            this.cmenTabSendSpecialKeysCtrlEsc.Name = "cmenTabSendSpecialKeysCtrlEsc";
            this.cmenTabSendSpecialKeysCtrlEsc.Size = new System.Drawing.Size(169, 26);
            this.cmenTabSendSpecialKeysCtrlEsc.Text = "Ctrl+Esc";
            // 
            // cmenTabSep2
            // 
            this.cmenTabSep2.Name = "cmenTabSep2";
            this.cmenTabSep2.Size = new System.Drawing.Size(272, 6);
            // 
            // cmenTabScreenshot
            // 
            this.cmenTabScreenshot.Image = global::mRemoteNG.Resources.Screenshot_Add;
            this.cmenTabScreenshot.Name = "cmenTabScreenshot";
            this.cmenTabScreenshot.Size = new System.Drawing.Size(275, 26);
            this.cmenTabScreenshot.Text = "Screenshot";
            // 
            // cmenTabPuttySettings
            // 
            this.cmenTabPuttySettings.Image = global::mRemoteNG.Resources.PuttyConfig;
            this.cmenTabPuttySettings.Name = "cmenTabPuttySettings";
            this.cmenTabPuttySettings.Size = new System.Drawing.Size(275, 26);
            this.cmenTabPuttySettings.Text = "PuTTY Settings";
            // 
            // cmenTabExternalApps
            // 
            this.cmenTabExternalApps.Image = ((System.Drawing.Image)(resources.GetObject("cmenTabExternalApps.Image")));
            this.cmenTabExternalApps.Name = "cmenTabExternalApps";
            this.cmenTabExternalApps.Size = new System.Drawing.Size(275, 26);
            this.cmenTabExternalApps.Text = "External Applications";
            // 
            // cmenCopyAllToClipboard
            // 
            this.cmenCopyAllToClipboard.Name = "cmenCopyAllToClipboard";
            this.cmenCopyAllToClipboard.Size = new System.Drawing.Size(275, 26);
            this.cmenCopyAllToClipboard.Text = "Copy all to clipboard";
            // 
            // ConnectionWindow
            // 
            this.ClientSize = new System.Drawing.Size(632, 453);
            this.Controls.Add(this.connDock);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::mRemoteNG.Resources.mRemoteNG_Icon;
            this.Name = "ConnectionWindow";
            this.TabText = "UI.Window.Connection";
            this.Text = "UI.Window.Connection";
            this.cmenTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

         internal WeifenLuo.WinFormsUI.Docking.DockPanel connDock;
         private System.ComponentModel.IContainer components;
        private ToolStripMenuItem cmenCopyAllToClipboard;
    }
}