using System;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;

namespace mRemoteNG.UI.Menu
{
    public class ViewMenu : ToolStripMenuItem
    {
        private ToolStripMenuItem _mMenViewConnectionPanels;
        private ToolStripSeparator _mMenViewSep1;
        private ToolStripMenuItem _mMenViewConnections;
        private ToolStripMenuItem _mMenViewConfig;
        private ToolStripMenuItem _mMenViewErrorsAndInfos;
        private ToolStripMenuItem _mMenViewScreenshotManager;
        private ToolStripMenuItem _mMenViewAddConnectionPanel;
        private ToolStripSeparator _mMenViewSep2;
        private ToolStripMenuItem _mMenViewFullscreen;
        private ToolStripMenuItem _mMenViewExtAppsToolbar;
        private ToolStripMenuItem _mMenViewQuickConnectToolbar;
        private ToolStripSeparator _mMenViewSep3;
        private ToolStripMenuItem _mMenViewJumpTo;
        private ToolStripMenuItem _mMenViewJumpToConnectionsConfig;
        private ToolStripMenuItem _mMenViewJumpToErrorsInfos;
        private ToolStripMenuItem _mMenViewResetLayout;
        private ToolStripSeparator _toolStripSeparator1;

        public ToolStrip TsExternalTools { get; set; }
        public ToolStrip TsQuickConnect { get; set; }
        public FullscreenHandler FullscreenHandler { get; set; }
        public FrmMain MainForm { get; set; }


        public ViewMenu()
        {
            Initialize();
            ApplyLanguage();
        }

        private void Initialize()
        {
            _mMenViewAddConnectionPanel = new ToolStripMenuItem();
            _mMenViewConnectionPanels = new ToolStripMenuItem();
            _mMenViewSep1 = new ToolStripSeparator();
            _mMenViewConnections = new ToolStripMenuItem();
            _mMenViewConfig = new ToolStripMenuItem();
            _mMenViewErrorsAndInfos = new ToolStripMenuItem();
            _mMenViewScreenshotManager = new ToolStripMenuItem();
            _mMenViewJumpTo = new ToolStripMenuItem();
            _mMenViewJumpToConnectionsConfig = new ToolStripMenuItem();
            _mMenViewJumpToErrorsInfos = new ToolStripMenuItem();
            _mMenViewResetLayout = new ToolStripMenuItem();
            _mMenViewSep2 = new ToolStripSeparator();
            _mMenViewQuickConnectToolbar = new ToolStripMenuItem();
            _mMenViewExtAppsToolbar = new ToolStripMenuItem();
            _mMenViewSep3 = new ToolStripSeparator();
            _mMenViewFullscreen = new ToolStripMenuItem();
            _toolStripSeparator1 = new ToolStripSeparator();

            // 
            // mMenView
            // 
            DropDownItems.AddRange(new ToolStripItem[] {
            _mMenViewAddConnectionPanel,
            _mMenViewConnectionPanels,
            _mMenViewSep1,
            _mMenViewConnections,
            _mMenViewConfig,
            _mMenViewErrorsAndInfos,
            _mMenViewScreenshotManager,
            _toolStripSeparator1,
            _mMenViewJumpTo,
            _mMenViewResetLayout,
            _mMenViewSep2,
            _mMenViewQuickConnectToolbar,
            _mMenViewExtAppsToolbar,
            _mMenViewSep3,
            _mMenViewFullscreen});
            Name = "mMenView";
            Size = new System.Drawing.Size(44, 20);
            Text = Language.strMenuView;
            //DropDownOpening += mMenView_DropDownOpening;
            // 
            // mMenViewAddConnectionPanel
            // 
            _mMenViewAddConnectionPanel.Image = Resources.Panel_Add;
            _mMenViewAddConnectionPanel.Name = "mMenViewAddConnectionPanel";
            _mMenViewAddConnectionPanel.Size = new System.Drawing.Size(228, 22);
            _mMenViewAddConnectionPanel.Text = "Add Connection Panel";
            _mMenViewAddConnectionPanel.Click += mMenViewAddConnectionPanel_Click;
            // 
            // mMenViewConnectionPanels
            // 
            _mMenViewConnectionPanels.Image = Resources.Panels;
            _mMenViewConnectionPanels.Name = "mMenViewConnectionPanels";
            _mMenViewConnectionPanels.Size = new System.Drawing.Size(228, 22);
            _mMenViewConnectionPanels.Text = "Connection Panels";
            // 
            // mMenViewSep1
            // 
            _mMenViewSep1.Name = "mMenViewSep1";
            _mMenViewSep1.Size = new System.Drawing.Size(225, 6);
            // 
            // mMenViewConnections
            // 
            _mMenViewConnections.Checked = true;
            _mMenViewConnections.CheckState = CheckState.Checked;
            _mMenViewConnections.Image = Resources.Root;
            _mMenViewConnections.Name = "mMenViewConnections";
            _mMenViewConnections.Size = new System.Drawing.Size(228, 22);
            _mMenViewConnections.Text = "Connections";
            _mMenViewConnections.Click += mMenViewConnections_Click;
            // 
            // mMenViewConfig
            // 
            _mMenViewConfig.Checked = true;
            _mMenViewConfig.CheckState = CheckState.Checked;
            _mMenViewConfig.Image = Resources.cog;
            _mMenViewConfig.Name = "mMenViewConfig";
            _mMenViewConfig.Size = new System.Drawing.Size(228, 22);
            _mMenViewConfig.Text = "Config";
            _mMenViewConfig.Click += mMenViewConfig_Click;
            // 
            // mMenViewErrorsAndInfos
            // 
            _mMenViewErrorsAndInfos.Checked = true;
            _mMenViewErrorsAndInfos.CheckState = CheckState.Checked;
            _mMenViewErrorsAndInfos.Image = Resources.ErrorsAndInfos;
            _mMenViewErrorsAndInfos.Name = "mMenViewErrorsAndInfos";
            _mMenViewErrorsAndInfos.Size = new System.Drawing.Size(228, 22);
            _mMenViewErrorsAndInfos.Text = "Errors and Infos";
            _mMenViewErrorsAndInfos.Click += mMenViewErrorsAndInfos_Click;
            // 
            // mMenViewScreenshotManager
            // 
            _mMenViewScreenshotManager.Image = Resources.Screenshot;
            _mMenViewScreenshotManager.Name = "mMenViewScreenshotManager";
            _mMenViewScreenshotManager.Size = new System.Drawing.Size(228, 22);
            _mMenViewScreenshotManager.Text = "Screenshot Manager";
            _mMenViewScreenshotManager.Click += mMenViewScreenshotManager_Click;
            // 
            // ToolStripSeparator1
            // 
            _toolStripSeparator1.Name = "ToolStripSeparator1";
            _toolStripSeparator1.Size = new System.Drawing.Size(225, 6);
            // 
            // mMenViewJumpTo
            // 
            _mMenViewJumpTo.DropDownItems.AddRange(new ToolStripItem[] {
            _mMenViewJumpToConnectionsConfig,
            _mMenViewJumpToErrorsInfos});
            _mMenViewJumpTo.Image = Resources.JumpTo;
            _mMenViewJumpTo.Name = "mMenViewJumpTo";
            _mMenViewJumpTo.Size = new System.Drawing.Size(228, 22);
            _mMenViewJumpTo.Text = "Jump To";
            // 
            // mMenViewJumpToConnectionsConfig
            // 
            _mMenViewJumpToConnectionsConfig.Image = Resources.Root;
            _mMenViewJumpToConnectionsConfig.Name = "mMenViewJumpToConnectionsConfig";
            _mMenViewJumpToConnectionsConfig.ShortcutKeys = ((Keys)(((Keys.Control | Keys.Alt)
            | Keys.C)));
            _mMenViewJumpToConnectionsConfig.Size = new System.Drawing.Size(258, 22);
            _mMenViewJumpToConnectionsConfig.Text = "Connections && Config";
            _mMenViewJumpToConnectionsConfig.Click += mMenViewJumpToConnectionsConfig_Click;
            // 
            // mMenViewJumpToErrorsInfos
            // 
            _mMenViewJumpToErrorsInfos.Image = Resources.InformationSmall;
            _mMenViewJumpToErrorsInfos.Name = "mMenViewJumpToErrorsInfos";
            _mMenViewJumpToErrorsInfos.ShortcutKeys = ((Keys)(((Keys.Control | Keys.Alt)
            | Keys.E)));
            _mMenViewJumpToErrorsInfos.Size = new System.Drawing.Size(258, 22);
            _mMenViewJumpToErrorsInfos.Text = "Errors && Infos";
            _mMenViewJumpToErrorsInfos.Click += mMenViewJumpToErrorsInfos_Click;
            // 
            // mMenViewResetLayout
            // 
            _mMenViewResetLayout.Image = Resources.application_side_tree;
            _mMenViewResetLayout.Name = "mMenViewResetLayout";
            _mMenViewResetLayout.Size = new System.Drawing.Size(228, 22);
            _mMenViewResetLayout.Text = "Reset Layout";
            _mMenViewResetLayout.Click += mMenViewResetLayout_Click;
            // 
            // mMenViewSep2
            // 
            _mMenViewSep2.Name = "mMenViewSep2";
            _mMenViewSep2.Size = new System.Drawing.Size(225, 6);
            // 
            // mMenViewQuickConnectToolbar
            // 
            _mMenViewQuickConnectToolbar.Image = Resources.Play_Quick;
            _mMenViewQuickConnectToolbar.Name = "mMenViewQuickConnectToolbar";
            _mMenViewQuickConnectToolbar.Size = new System.Drawing.Size(228, 22);
            _mMenViewQuickConnectToolbar.Text = "Quick Connect Toolbar";
            _mMenViewQuickConnectToolbar.Click += mMenViewQuickConnectToolbar_Click;
            // 
            // mMenViewExtAppsToolbar
            // 
            _mMenViewExtAppsToolbar.Image = Resources.ExtApp;
            _mMenViewExtAppsToolbar.Name = "mMenViewExtAppsToolbar";
            _mMenViewExtAppsToolbar.Size = new System.Drawing.Size(228, 22);
            _mMenViewExtAppsToolbar.Text = "External Applications Toolbar";
            _mMenViewExtAppsToolbar.Click += mMenViewExtAppsToolbar_Click;
            // 
            // mMenViewSep3
            // 
            _mMenViewSep3.Name = "mMenViewSep3";
            _mMenViewSep3.Size = new System.Drawing.Size(225, 6);
            // 
            // mMenViewFullscreen
            // 
            _mMenViewFullscreen.Image = Resources.arrow_out;
            _mMenViewFullscreen.Name = "mMenViewFullscreen";
            _mMenViewFullscreen.ShortcutKeys = Keys.F11;
            _mMenViewFullscreen.Size = new System.Drawing.Size(228, 22);
            _mMenViewFullscreen.Text = "Full Screen";
            _mMenViewFullscreen.Checked = Settings.Default.MainFormKiosk;
            _mMenViewFullscreen.Click += mMenViewFullscreen_Click;
        }

        private void ApplyLanguage()
        {
            _mMenViewAddConnectionPanel.Text = Language.strMenuAddConnectionPanel;
            _mMenViewConnectionPanels.Text = Language.strMenuConnectionPanels;
            _mMenViewConnections.Text = Language.strMenuConnections;
            _mMenViewConfig.Text = Language.strMenuConfig;
            _mMenViewErrorsAndInfos.Text = Language.strMenuNotifications;
            _mMenViewScreenshotManager.Text = Language.strScreenshots;
            _mMenViewJumpTo.Text = Language.strMenuJumpTo;
            _mMenViewJumpToConnectionsConfig.Text = Language.strMenuConnectionsAndConfig;
            _mMenViewJumpToErrorsInfos.Text = Language.strMenuNotifications;
            _mMenViewResetLayout.Text = Language.strMenuResetLayout;
            _mMenViewQuickConnectToolbar.Text = Language.strMenuQuickConnectToolbar;
            _mMenViewExtAppsToolbar.Text = Language.strMenuExternalToolsToolbar;
            _mMenViewFullscreen.Text = Language.strMenuFullScreen;
        }

        #region View
        internal void mMenView_DropDownOpening(object sender, EventArgs e)
        {
            _mMenViewConnections.Checked = !Windows.TreeForm.IsHidden;
            _mMenViewConfig.Checked = !Windows.ConfigForm.IsHidden;
            _mMenViewErrorsAndInfos.Checked = !Windows.ErrorsForm.IsHidden;
            _mMenViewScreenshotManager.Checked = !Windows.ScreenshotForm.IsHidden;

            _mMenViewExtAppsToolbar.Checked = TsExternalTools.Visible;
            _mMenViewQuickConnectToolbar.Checked = TsQuickConnect.Visible;

            _mMenViewConnectionPanels.DropDownItems.Clear();

            for (var i = 0; i <= Runtime.WindowList.Count - 1; i++)
            {
                var tItem = new ToolStripMenuItem(Runtime.WindowList[i].Text,
                    Runtime.WindowList[i].Icon.ToBitmap(), ConnectionPanelMenuItem_Click)
                { Tag = Runtime.WindowList[i] };
                _mMenViewConnectionPanels.DropDownItems.Add(tItem);
            }

            _mMenViewConnectionPanels.Enabled = _mMenViewConnectionPanels.DropDownItems.Count > 0;
        }

        private void ConnectionPanelMenuItem_Click(object sender, EventArgs e)
        {
            ((BaseWindow)((ToolStripMenuItem)sender).Tag).Show(MainForm.pnlDock);
            ((BaseWindow)((ToolStripMenuItem)sender).Tag).Focus();
        }

        private void mMenViewConnections_Click(object sender, EventArgs e)
        {
            if (_mMenViewConnections.Checked == false)
            {
                Windows.TreeForm.Show(MainForm.pnlDock);
                _mMenViewConnections.Checked = true;
            }
            else
            {
                Windows.TreeForm.Hide();
                _mMenViewConnections.Checked = false;
            }
        }

        private void mMenViewConfig_Click(object sender, EventArgs e)
        {
            if (_mMenViewConfig.Checked == false)
            {
                Windows.ConfigForm.Show(MainForm.pnlDock);
                _mMenViewConfig.Checked = true;
            }
            else
            {
                Windows.ConfigForm.Hide();
                _mMenViewConfig.Checked = false;
            }
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

        private void mMenViewScreenshotManager_Click(object sender, EventArgs e)
        {
            if (_mMenViewScreenshotManager.Checked == false)
            {
                Windows.ScreenshotForm.Show(MainForm.pnlDock);
                _mMenViewScreenshotManager.Checked = true;
            }
            else
            {
                Windows.ScreenshotForm.Hide();
                _mMenViewScreenshotManager.Checked = false;
            }
        }

        private void mMenViewJumpToConnectionsConfig_Click(object sender, EventArgs e)
        {
            if (MainForm.pnlDock.ActiveContent == Windows.TreeForm)
            {
                Windows.ConfigForm.Activate();
            }
            else
            {
                Windows.TreeForm.Activate();
            }
        }

        private void mMenViewJumpToErrorsInfos_Click(object sender, EventArgs e)
        {
            Windows.ErrorsForm.Activate();
        }

        private void mMenViewResetLayout_Click(object sender, EventArgs e)
        {
            var msgBoxResult = MessageBox.Show(Language.strConfirmResetLayout, string.Empty, MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (msgBoxResult == DialogResult.Yes)
            {
                MainForm.SetDefaultLayout();
            }
        }

        private void mMenViewAddConnectionPanel_Click(object sender, EventArgs e)
        {
            Runtime.AddPanel();
        }

        private void mMenViewExtAppsToolbar_Click(object sender, EventArgs e)
        {
            if (_mMenViewExtAppsToolbar.Checked == false)
            {
                TsExternalTools.Visible = true;
                _mMenViewExtAppsToolbar.Checked = true;
            }
            else
            {
                TsExternalTools.Visible = false;
                _mMenViewExtAppsToolbar.Checked = false;
            }
        }

        private void mMenViewQuickConnectToolbar_Click(object sender, EventArgs e)
        {
            if (_mMenViewQuickConnectToolbar.Checked == false)
            {
                TsQuickConnect.Visible = true;
                _mMenViewQuickConnectToolbar.Checked = true;
            }
            else
            {
                TsQuickConnect.Visible = false;
                _mMenViewQuickConnectToolbar.Checked = false;
            }
        }

        private void mMenViewFullscreen_Click(object sender, EventArgs e)
        {
            FullscreenHandler.Value = !FullscreenHandler.Value;
            _mMenViewFullscreen.Checked = FullscreenHandler.Value;
        }
        #endregion
    }
}