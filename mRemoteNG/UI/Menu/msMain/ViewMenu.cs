using System;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Properties;
using mRemoteNG.Resources.Language;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Panels;
using mRemoteNG.UI.Window;

namespace mRemoteNG.UI.Menu
{
    [SupportedOSPlatform("windows")]
    public class ViewMenu : ToolStripMenuItem
    {
        private ToolStripMenuItem _mMenViewConnectionPanels;
        private ToolStripMenuItem _mMenReconnectAll;
        private ToolStripSeparator _mMenViewSep1;
        public ToolStripMenuItem _mMenViewErrorsAndInfos;
        public ToolStripMenuItem _mMenViewFileMenu;
        private ToolStripMenuItem _mMenViewAddConnectionPanel;
        private ToolStripSeparator _mMenViewSep2;
        private ToolStripMenuItem _mMenViewFullscreen;
        public ToolStripMenuItem _mMenViewExtAppsToolbar;
        public ToolStripMenuItem _mMenViewQuickConnectToolbar;
        public ToolStripMenuItem _mMenViewMultiSshToolbar;
        private ToolStripMenuItem _mMenViewResetLayout;
        public ToolStripMenuItem _mMenViewLockToolbars;
        private readonly PanelAdder _panelAdder;


        public ToolStrip TsExternalTools { get; set; }
        public ToolStrip TsQuickConnect { get; set; }
        public ToolStrip TsMultiSsh { get; set; }
        public FullscreenHandler FullscreenHandler { get; set; }
        public FrmMain MainForm { get; set; }


        public ViewMenu()
        {
            Initialize();
            _panelAdder = new PanelAdder();
        }

        private void Initialize()
        {
            _mMenViewAddConnectionPanel = new ToolStripMenuItem();
            _mMenViewConnectionPanels = new ToolStripMenuItem();
            _mMenViewSep1 = new ToolStripSeparator();
            _mMenViewFileMenu = new ToolStripMenuItem();
            _mMenViewErrorsAndInfos = new ToolStripMenuItem();
            _mMenViewResetLayout = new ToolStripMenuItem();
            _mMenViewLockToolbars = new ToolStripMenuItem();
            _mMenViewSep2 = new ToolStripSeparator();
            _mMenViewQuickConnectToolbar = new ToolStripMenuItem();
            _mMenReconnectAll = new ToolStripMenuItem();
            _mMenViewExtAppsToolbar = new ToolStripMenuItem();
            _mMenViewMultiSshToolbar = new ToolStripMenuItem();
            _mMenViewFullscreen = new ToolStripMenuItem();

            // 
            // mMenView
            // 
            DropDownItems.AddRange(new ToolStripItem[]
            {
                _mMenViewFileMenu,
                _mMenViewErrorsAndInfos,
                _mMenViewQuickConnectToolbar,
                _mMenViewExtAppsToolbar,
                _mMenViewMultiSshToolbar,
                _mMenViewSep1,
                _mMenReconnectAll,
                _mMenViewAddConnectionPanel,
                _mMenViewConnectionPanels,
                _mMenViewResetLayout,
                _mMenViewLockToolbars,
                _mMenViewSep2,
                _mMenViewFullscreen
            });
            Name = "mMenView";
            Size = new System.Drawing.Size(44, 20);
            Text = Language._View;
            //DropDownOpening += mMenView_DropDownOpening;
            // 
            // mMenViewAddConnectionPanel
            // 
            _mMenViewAddConnectionPanel.Image = Properties.Resources.InsertPanel_16x;
            _mMenViewAddConnectionPanel.Name = "mMenViewAddConnectionPanel";
            _mMenViewAddConnectionPanel.Size = new System.Drawing.Size(228, 22);
            _mMenViewAddConnectionPanel.Text = Language.AddConnectionPanel;
            _mMenViewAddConnectionPanel.Click += mMenViewAddConnectionPanel_Click;
            // 
            // mMenReconnectAll
            // 
            _mMenReconnectAll.Image = Properties.Resources.Refresh_16x;
            _mMenReconnectAll.Name = "mMenReconnectAll";
            _mMenReconnectAll.Size = new System.Drawing.Size(281, 22);
            _mMenReconnectAll.Text = Language.ReconnectAllConnections;
            _mMenReconnectAll.Click += mMenReconnectAll_Click;
            // 
            // mMenViewConnectionPanels
            // 
            _mMenViewConnectionPanels.Image = Properties.Resources.Panel_16x;
            _mMenViewConnectionPanels.Name = "mMenViewConnectionPanels";
            _mMenViewConnectionPanels.Size = new System.Drawing.Size(228, 22);
            _mMenViewConnectionPanels.Text = Language.ConnectionPanels;
            // 
            // mMenViewSep1
            // 
            _mMenViewSep1.Name = "mMenViewSep1";
            _mMenViewSep1.Size = new System.Drawing.Size(225, 6);
            // 
            // mMenViewFile
            // 
            _mMenViewFileMenu.Checked = true;
            _mMenViewFileMenu.CheckState = CheckState.Checked;
            _mMenViewFileMenu.Name = "mMenViewFile";
            _mMenViewFileMenu.Size = new System.Drawing.Size(228, 22);
            _mMenViewFileMenu.Text = Language.FileMenu;
            _mMenViewFileMenu.Click += mMenViewFileMenu_Click;
            // 
            // mMenViewErrorsAndInfos
            // 
            _mMenViewErrorsAndInfos.Checked = true;
            _mMenViewErrorsAndInfos.CheckState = CheckState.Checked;
            _mMenViewErrorsAndInfos.Name = "mMenViewErrorsAndInfos";
            _mMenViewErrorsAndInfos.Size = new System.Drawing.Size(228, 22);
            _mMenViewErrorsAndInfos.Text = Language.Notifications;
            _mMenViewErrorsAndInfos.Click += mMenViewErrorsAndInfos_Click;
            // 
            // mMenViewResetLayout
            // 
            _mMenViewResetLayout.Name = "mMenViewResetLayout";
            _mMenViewResetLayout.Size = new System.Drawing.Size(228, 22);
            _mMenViewResetLayout.Text = Language.ResetLayout;
            _mMenViewResetLayout.Click += mMenViewResetLayout_Click;
            // 
            // mMenViewLockToolbars
            // 
            _mMenViewLockToolbars.Name = "mMenViewLockToolbars";
            _mMenViewLockToolbars.Size = new System.Drawing.Size(228, 22);
            _mMenViewLockToolbars.Text = Language.LockToolbars;
            _mMenViewLockToolbars.Click += mMenViewLockToolbars_Click;
            // 
            // mMenViewSep2
            // 
            _mMenViewSep2.Name = "mMenViewSep2";
            _mMenViewSep2.Size = new System.Drawing.Size(225, 6);
            // 
            // mMenViewQuickConnectToolbar
            // 
            _mMenViewQuickConnectToolbar.Name = "mMenViewQuickConnectToolbar";
            _mMenViewQuickConnectToolbar.Size = new System.Drawing.Size(228, 22);
            _mMenViewQuickConnectToolbar.Text = Language.QuickConnectToolbar;
            _mMenViewQuickConnectToolbar.Click += mMenViewQuickConnectToolbar_Click;
            // 
            // mMenViewExtAppsToolbar
            // 
            _mMenViewExtAppsToolbar.Name = "mMenViewExtAppsToolbar";
            _mMenViewExtAppsToolbar.Size = new System.Drawing.Size(228, 22);
            _mMenViewExtAppsToolbar.Text = Language.ExternalToolsToolbar;
            _mMenViewExtAppsToolbar.Click += mMenViewExtAppsToolbar_Click;
            // 
            // mMenViewMultiSSHToolbar
            // 
            _mMenViewMultiSshToolbar.Name = "mMenViewMultiSSHToolbar";
            _mMenViewMultiSshToolbar.Size = new System.Drawing.Size(279, 26);
            _mMenViewMultiSshToolbar.Text = Language.MultiSshToolbar;
            _mMenViewMultiSshToolbar.Click += mMenViewMultiSSHToolbar_Click;
            // 
            // mMenViewFullscreen
            // 
            _mMenViewFullscreen.Image = Properties.Resources.FullScreen_16x;
            _mMenViewFullscreen.Name = "mMenViewFullscreen";
            _mMenViewFullscreen.ShortcutKeys = Keys.F11;
            _mMenViewFullscreen.Size = new System.Drawing.Size(228, 22);
            _mMenViewFullscreen.Text = Language.Fullscreen;
            _mMenViewFullscreen.Checked = Properties.App.Default.MainFormKiosk;
            _mMenViewFullscreen.Click += mMenViewFullscreen_Click;
        }


        public void ApplyLanguage()
        {
            Text = Language._View;
            _mMenViewAddConnectionPanel.Text = Language.AddConnectionPanel;
            _mMenViewConnectionPanels.Text = Language.ConnectionPanels;
            _mMenViewErrorsAndInfos.Text = Language.Notifications;
            _mMenViewResetLayout.Text = Language.ResetLayout;
            _mMenViewLockToolbars.Text = Language.LockToolbars;
            _mMenViewQuickConnectToolbar.Text = Language.QuickConnectToolbar;
            _mMenViewExtAppsToolbar.Text = Language.ExternalToolsToolbar;
            _mMenViewMultiSshToolbar.Text = Language.MultiSshToolbar;
            _mMenViewFullscreen.Text = Language.Fullscreen;
        }

        #region View

        internal void mMenView_DropDownOpening(object sender, EventArgs e)
        {
            _mMenViewErrorsAndInfos.Checked = !Windows.ErrorsForm.IsHidden;
            _mMenViewLockToolbars.Checked = Settings.Default.LockToolbars;

            _mMenViewExtAppsToolbar.Checked = TsExternalTools.Visible;
            _mMenViewQuickConnectToolbar.Checked = TsQuickConnect.Visible;
            _mMenViewMultiSshToolbar.Checked = TsMultiSsh.Visible;

            _mMenViewConnectionPanels.DropDownItems.Clear();

            for (var i = 0; i <= Runtime.WindowList.Count - 1; i++)
            {
                var tItem = new ToolStripMenuItem(Runtime.WindowList[i].Text, Runtime.WindowList[i].Icon.ToBitmap(), ConnectionPanelMenuItem_Click)
                { Tag = Runtime.WindowList[i] };
                _mMenViewConnectionPanels.DropDownItems.Add(tItem);
            }

            _mMenViewConnectionPanels.Visible = _mMenViewConnectionPanels.DropDownItems.Count > 0;
        }

        private void ConnectionPanelMenuItem_Click(object sender, EventArgs e)
        {
            ((BaseWindow)((ToolStripMenuItem)sender).Tag).Show(MainForm.pnlDock);
            ((BaseWindow)((ToolStripMenuItem)sender).Tag).Focus();
        }

        private void mMenViewErrorsAndInfos_Click(object sender, EventArgs e)
        {
            if (_mMenViewErrorsAndInfos.Checked == false)
            {
                Windows.ErrorsForm.Show(MainForm.pnlDock);
                _mMenViewErrorsAndInfos.Checked = true;
            }
            else
            {
                Windows.ErrorsForm.Hide();
                _mMenViewErrorsAndInfos.Checked = false;
            }
        }

        private void mMenViewFileMenu_Click(object sender, EventArgs e)
        {
            if (_mMenViewFileMenu.Checked == false)
            {
                MainForm.ShowFileMenu();
            }
            else
            {
                MainForm.HideFileMenu();
            }
        }

        private void mMenViewResetLayout_Click(object sender, EventArgs e)
        {
            var msgBoxResult = MessageBox.Show(Language.ConfirmResetLayout, string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (msgBoxResult == DialogResult.Yes)
            {
                MainForm.SetDefaultLayout();
            }
        }

        private void mMenViewLockToolbars_Click(object sender, EventArgs eventArgs)
        {
            if (_mMenViewLockToolbars.Checked)
            {
                Settings.Default.LockToolbars = false;
                _mMenViewLockToolbars.Checked = false;
            }
            else
            {
                Settings.Default.LockToolbars = true;
                _mMenViewLockToolbars.Checked = true;
            }
        }

        private void mMenViewAddConnectionPanel_Click(object sender, EventArgs e)
        {
            _panelAdder.AddPanel();
        }

        private void mMenViewExtAppsToolbar_Click(object sender, EventArgs e)
        {
            if (_mMenViewExtAppsToolbar.Checked)
            {
                Settings.Default.ViewMenuExternalTools = false;
                _mMenViewExtAppsToolbar.Checked = false;
                TsExternalTools.Visible = false;
            }
            else
            {
                Settings.Default.ViewMenuExternalTools = true;
                _mMenViewExtAppsToolbar.Checked = true;
                TsExternalTools.Visible = true;
            }
        }

        private void mMenViewQuickConnectToolbar_Click(object sender, EventArgs e)
        {
            if (_mMenViewQuickConnectToolbar.Checked)
            {
                Settings.Default.ViewMenuQuickConnect = false;
                _mMenViewQuickConnectToolbar.Checked = false;
                TsQuickConnect.Visible = false;
            }
            else
            {
                Settings.Default.ViewMenuQuickConnect = true;
                _mMenViewQuickConnectToolbar.Checked = true;
                TsQuickConnect.Visible = true;
            }
        }

        private void mMenViewMultiSSHToolbar_Click(object sender, EventArgs e)
        {
            if (_mMenViewMultiSshToolbar.Checked)
            {
                Settings.Default.ViewMenuMultiSSH = false;
                _mMenViewMultiSshToolbar.Checked = false;
                TsMultiSsh.Visible = false;
            }
            else
            {
                Settings.Default.ViewMenuMultiSSH = true;
                _mMenViewMultiSshToolbar.Checked = true;
                TsMultiSsh.Visible = true;
            }
        }

        private void mMenViewFullscreen_Click(object sender, EventArgs e)
        {
            FullscreenHandler.Value = !FullscreenHandler.Value;
            _mMenViewFullscreen.Checked = FullscreenHandler.Value;
        }

        private void mMenReconnectAll_Click(object sender, EventArgs e)
        {
            if (Runtime.WindowList == null || Runtime.WindowList.Count == 0) return;
            foreach (BaseWindow window in Runtime.WindowList)
            {
                if (!(window is ConnectionWindow connectionWindow))
                    return;

                connectionWindow.reconnectAll(Runtime.ConnectionInitiator);
            }
        }

        #endregion
    }
}