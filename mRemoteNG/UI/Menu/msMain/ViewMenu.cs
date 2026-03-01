using System;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Config.Settings;
using mRemoteNG.Properties;
using mRemoteNG.Resources.Language;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Panels;
using mRemoteNG.UI.Window;
using mRemoteNG.Config.Serializers; // Added
using mRemoteNG.Config.DataProviders; // Added
using mRemoteNG.App.Info; // Added
using System.IO; // Added

namespace mRemoteNG.UI.Menu
{
    [SupportedOSPlatform("windows")]
    public class ViewMenu : ToolStripMenuItem
    {
        private ToolStripMenuItem _mMenViewConnectionPanels = null!;
        private ToolStripMenuItem _mMenReconnectAll = null!;
        private ToolStripSeparator _mMenViewSep1 = null!;
        public ToolStripMenuItem _mMenViewErrorsAndInfos = null!;
        private ToolStripMenuItem _mMenViewConfigPanel = null!;
        private ToolStripMenuItem _mMenViewActiveConnections = null!;
        public ToolStripMenuItem _mMenViewFileMenu = null!;
        private ToolStripMenuItem _mMenViewAddConnectionPanel = null!;
        private ToolStripSeparator _mMenViewSep2 = null!;
        private ToolStripMenuItem _mMenViewFullscreen = null!;
        public ToolStripMenuItem _mMenViewExtAppsToolbar = null!;
        public ToolStripMenuItem _mMenViewQuickConnectToolbar = null!;
        public ToolStripMenuItem _mMenViewMultiSshToolbar = null!;
        public ToolStripMenuItem _mMenViewPresentationMode = null!;
        private ToolStripMenuItem _mMenViewResetLayout = null!;
        private ToolStripMenuItem _mMenViewLoadLayout = null!;
        private ToolStripMenuItem _mMenViewSaveLayout = null!;
        public ToolStripMenuItem _mMenViewLockToolbars = null!;


        public ToolStrip? TsExternalTools { get; set; }
        public ToolStrip? TsQuickConnect { get; set; }
        public ToolStrip? TsMultiSsh { get; set; }
        public FullscreenHandler? FullscreenHandler { get; set; }
        public PresentationModeHandler? PresentationMode { get; set; }
        public FrmMain? MainForm { get; set; }


        public ViewMenu()
        {
            Initialize();
        }

        private void Initialize()
        {
            _mMenViewAddConnectionPanel = new ToolStripMenuItem();
            _mMenViewConnectionPanels = new ToolStripMenuItem();
            _mMenViewSep1 = new ToolStripSeparator();
            _mMenViewFileMenu = new ToolStripMenuItem();
            _mMenViewErrorsAndInfos = new ToolStripMenuItem();
            _mMenViewConfigPanel = new ToolStripMenuItem();
            _mMenViewActiveConnections = new ToolStripMenuItem();
            _mMenViewResetLayout = new ToolStripMenuItem();
            _mMenViewLoadLayout = new ToolStripMenuItem();
            _mMenViewSaveLayout = new ToolStripMenuItem();
            _mMenViewLockToolbars = new ToolStripMenuItem();
            _mMenViewSep2 = new ToolStripSeparator();
            _mMenViewQuickConnectToolbar = new ToolStripMenuItem();
            _mMenReconnectAll = new ToolStripMenuItem();
            _mMenViewExtAppsToolbar = new ToolStripMenuItem();
            _mMenViewMultiSshToolbar = new ToolStripMenuItem();
            _mMenViewFullscreen = new ToolStripMenuItem();
            _mMenViewPresentationMode = new ToolStripMenuItem();

            //
            // mMenView
            //
            DropDownItems.AddRange(new ToolStripItem[]
            {
                _mMenViewFileMenu,
                _mMenViewErrorsAndInfos,
                _mMenViewConfigPanel,
                _mMenViewActiveConnections,
                _mMenViewQuickConnectToolbar,
                _mMenViewExtAppsToolbar,
                _mMenViewMultiSshToolbar,
                _mMenViewSep1,
                _mMenReconnectAll,
                _mMenViewAddConnectionPanel,
                _mMenViewConnectionPanels,
                _mMenViewResetLayout,
                _mMenViewLoadLayout,
                _mMenViewSaveLayout,
                _mMenViewLockToolbars,
                _mMenViewSep2,
                _mMenViewFullscreen,
                _mMenViewPresentationMode
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
            // mMenViewConfigPanel
            //
            _mMenViewConfigPanel.Checked = true;
            _mMenViewConfigPanel.CheckState = CheckState.Checked;
            _mMenViewConfigPanel.Name = "mMenViewConfigPanel";
            _mMenViewConfigPanel.Size = new System.Drawing.Size(228, 22);
            _mMenViewConfigPanel.Text = Language.Config;
            _mMenViewConfigPanel.Click += mMenViewConfigPanel_Click;
            //
            // mMenViewActiveConnections
            //
            _mMenViewActiveConnections.Name = "mMenViewActiveConnections";
            _mMenViewActiveConnections.Size = new System.Drawing.Size(228, 22);
            _mMenViewActiveConnections.Text = "Active Connections";
            _mMenViewActiveConnections.Click += mMenViewActiveConnections_Click;
            //
            // mMenViewResetLayout
            //
            _mMenViewResetLayout.Name = "mMenViewResetLayout";
            _mMenViewResetLayout.Size = new System.Drawing.Size(228, 22);
            _mMenViewResetLayout.Text = Language.ResetLayout;
            _mMenViewResetLayout.Click += mMenViewResetLayout_Click;
            // 
            // mMenViewLoadLayout
            // 
            _mMenViewLoadLayout.Name = "mMenViewLoadLayout";
            _mMenViewLoadLayout.Size = new System.Drawing.Size(228, 22);
            _mMenViewLoadLayout.Text = "Load Layout";
            _mMenViewLoadLayout.DropDownOpening += mMenViewLoadLayout_DropDownOpening;
            // 
            // mMenViewSaveLayout
            // 
            _mMenViewSaveLayout.Name = "mMenViewSaveLayout";
            _mMenViewSaveLayout.Size = new System.Drawing.Size(228, 22);
            _mMenViewSaveLayout.Text = "Save Layout...";
            _mMenViewSaveLayout.Click += mMenViewSaveLayout_Click;
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
            // 
            // mMenViewPresentationMode
            // 
            _mMenViewPresentationMode.Image = null; // Properties.Resources.Presentation_16x;
            _mMenViewPresentationMode.Name = "mMenViewPresentationMode";
            _mMenViewPresentationMode.ShortcutKeys = Keys.Shift | Keys.F11;
            _mMenViewPresentationMode.Size = new System.Drawing.Size(228, 22);
            _mMenViewPresentationMode.Text = "Presentation Mode";
            _mMenViewPresentationMode.Click += mMenViewPresentationMode_Click;
        }


        public void ApplyLanguage()
        {
            Text = Language._View;
            _mMenViewAddConnectionPanel.Text = Language.AddConnectionPanel;
            _mMenViewConnectionPanels.Text = Language.ConnectionPanels;
            _mMenViewErrorsAndInfos.Text = Language.Notifications;
            _mMenViewConfigPanel.Text = Language.Config;
            _mMenViewActiveConnections.Text = "Active Connections";
            _mMenViewResetLayout.Text = Language.ResetLayout;
            _mMenViewLoadLayout.Text = "Load Layout";
            _mMenViewSaveLayout.Text = "Save Layout...";
            _mMenViewLockToolbars.Text = Language.LockToolbars;
            _mMenViewQuickConnectToolbar.Text = Language.QuickConnectToolbar;
            _mMenViewExtAppsToolbar.Text = Language.ExternalToolsToolbar;
            _mMenViewMultiSshToolbar.Text = Language.MultiSshToolbar;
            _mMenViewFullscreen.Text = Language.Fullscreen;
            _mMenViewPresentationMode.Text = "Presentation Mode";
        }

        #region View

        internal void mMenView_DropDownOpening(object sender, EventArgs e)
        {
            _mMenViewErrorsAndInfos.Checked = !AppWindows.ErrorsForm.IsHidden;
            _mMenViewConfigPanel.Checked = !AppWindows.ConfigForm.IsHidden;
            _mMenViewLockToolbars.Checked = mRemoteNG.Properties.Settings.Default.LockToolbars;

            if (TsExternalTools is not null)
                _mMenViewExtAppsToolbar.Checked = TsExternalTools.Visible;
            if (TsQuickConnect is not null)
                _mMenViewQuickConnectToolbar.Checked = TsQuickConnect.Visible;
            if (TsMultiSsh is not null)
                _mMenViewMultiSshToolbar.Checked = TsMultiSsh.Visible;

            _mMenViewConnectionPanels.DropDownItems.Clear();

            for (int i = 0; i <= Runtime.WindowList.Count - 1; i++)
            {
                var window = Runtime.WindowList[i];
                if (window is null) continue;
                ToolStripMenuItem tItem = new(window.Text, window.Icon?.ToBitmap(), ConnectionPanelMenuItem_Click)
                { Tag = window };
                _mMenViewConnectionPanels.DropDownItems.Add(tItem);
            }

            _mMenViewConnectionPanels.Visible = _mMenViewConnectionPanels.DropDownItems.Count > 0;
        }

        private void ConnectionPanelMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is not ToolStripMenuItem menuItem || menuItem.Tag is not BaseWindow window || MainForm is null)
                return;
            window.Show(MainForm.pnlDock);
            window.Focus();
        }

        private void mMenViewErrorsAndInfos_Click(object sender, EventArgs e)
        {
            if (_mMenViewErrorsAndInfos.Checked == false)
            {
                if (MainForm is not null)
                    AppWindows.ErrorsForm.Show(MainForm.pnlDock);
                _mMenViewErrorsAndInfos.Checked = true;
            }
            else
            {
                AppWindows.ErrorsForm.Hide();
                _mMenViewErrorsAndInfos.Checked = false;
            }
        }

        private void mMenViewConfigPanel_Click(object sender, EventArgs e)
        {
            if (_mMenViewConfigPanel.Checked == false)
            {
                if (MainForm is not null)
                    AppWindows.ConfigForm.Show(MainForm.pnlDock);
                _mMenViewConfigPanel.Checked = true;
            }
            else
            {
                AppWindows.ConfigForm.Hide();
                _mMenViewConfigPanel.Checked = false;
            }
        }

        private void mMenViewActiveConnections_Click(object sender, EventArgs e)
        {
            AppWindows.Show(WindowType.ActiveConnections);
        }

        private void mMenViewFileMenu_Click(object sender, EventArgs e)
        {
            if (MainForm is null) return;
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
            DialogResult msgBoxResult = MessageBox.Show(Language.ConfirmResetLayout, string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (msgBoxResult == DialogResult.Yes)
            {
                string layoutFilePath = Path.Combine(SettingsFileInfo.SettingsPath, SettingsFileInfo.LayoutFileName);
                if (File.Exists(layoutFilePath))
                    File.Delete(layoutFilePath);
                MainForm?.SetDefaultLayout();
            }
        }

        private void mMenViewLoadLayout_DropDownOpening(object sender, EventArgs e)
        {
            if (MainForm == null) return;
            _mMenViewLoadLayout.DropDownItems.Clear();

            var loader = new DockPanelLayoutLoader(MainForm, Runtime.MessageCollector);
            var layoutNames = DockPanelLayoutLoader.GetLayoutNames();

            if (layoutNames.Count > 0)
            {
                foreach (var name in layoutNames)
                {
                    _mMenViewLoadLayout.DropDownItems.Add(name, null, (s, args) => loader.LoadLayoutByName(name));
                }
                _mMenViewLoadLayout.DropDownItems.Add(new ToolStripSeparator());
            }

            _mMenViewLoadLayout.DropDownItems.Add("Load from file...", null, mMenViewLoadLayoutFromFile_Click);
        }

        private void mMenViewLoadLayoutFromFile_Click(object sender, EventArgs e)
        {
            if (MainForm == null) return;
            using var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Layout Files (*.xml)|*.xml|All Files (*.*)|*.*";
            openFileDialog.Title = "Load Layout";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var loader = new DockPanelLayoutLoader(MainForm, Runtime.MessageCollector);
                loader.LoadLayout(openFileDialog.FileName);
            }
        }

        private void mMenViewSaveLayout_Click(object sender, EventArgs e)
        {
            if (MainForm == null) return;

            string layoutName = "";
            using (var inputBox = new FrmInputBox("Save Layout", "Enter a name for the layout:", layoutName))
            {
                if (inputBox.ShowDialog() == DialogResult.OK)
                {
                    layoutName = inputBox.returnValue ?? "";
                    if (!string.IsNullOrWhiteSpace(layoutName))
                    {
                        // Create dummy IDataProvider for now as we use SaveLayout(string name) which bypasses it or create a proper one
                        // We can reuse the same pattern as default saver
                        var saver = new DockPanelLayoutSaver(new DockPanelLayoutSerializer(), new FileDataProvider(Path.Combine(SettingsFileInfo.SettingsPath, SettingsFileInfo.LayoutFileName)));
                        saver.SaveLayout(layoutName);
                    }
                }
            }
        }

        private void mMenViewLockToolbars_Click(object sender, EventArgs eventArgs)
        {
            if (_mMenViewLockToolbars.Checked)
            {
                mRemoteNG.Properties.Settings.Default.LockToolbars = false;
                _mMenViewLockToolbars.Checked = false;
            }
            else
            {
                mRemoteNG.Properties.Settings.Default.LockToolbars = true;
                _mMenViewLockToolbars.Checked = true;
            }
        }

        private void mMenViewAddConnectionPanel_Click(object sender, EventArgs e)
        {
            PanelAdder.AddPanel();
        }

        private void mMenViewExtAppsToolbar_Click(object sender, EventArgs e)
        {
            if (_mMenViewExtAppsToolbar.Checked)
            {
                mRemoteNG.Properties.Settings.Default.ViewMenuExternalTools = false;
                _mMenViewExtAppsToolbar.Checked = false;
                if (TsExternalTools is not null) TsExternalTools.Visible = false;
            }
            else
            {
                mRemoteNG.Properties.Settings.Default.ViewMenuExternalTools = true;
                _mMenViewExtAppsToolbar.Checked = true;
                if (TsExternalTools is not null) TsExternalTools.Visible = true;
            }
        }

        private void mMenViewQuickConnectToolbar_Click(object sender, EventArgs e)
        {
            if (_mMenViewQuickConnectToolbar.Checked)
            {
                mRemoteNG.Properties.Settings.Default.ViewMenuQuickConnect = false;
                mRemoteNG.Properties.Settings.Default.QuickyTBVisible = false;
                _mMenViewQuickConnectToolbar.Checked = false;
                if (TsQuickConnect is not null) TsQuickConnect.Visible = false;
            }
            else
            {
                mRemoteNG.Properties.Settings.Default.ViewMenuQuickConnect = true;
                mRemoteNG.Properties.Settings.Default.QuickyTBVisible = true;
                _mMenViewQuickConnectToolbar.Checked = true;
                if (TsQuickConnect is not null) TsQuickConnect.Visible = true;
            }
        }

        private void mMenViewMultiSSHToolbar_Click(object sender, EventArgs e)
        {
            if (_mMenViewMultiSshToolbar.Checked)
            {
                mRemoteNG.Properties.Settings.Default.ViewMenuMultiSSH = false;
                _mMenViewMultiSshToolbar.Checked = false;
                if (TsMultiSsh is not null) TsMultiSsh.Visible = false;
            }
            else
            {
                mRemoteNG.Properties.Settings.Default.ViewMenuMultiSSH = true;
                _mMenViewMultiSshToolbar.Checked = true;
                if (TsMultiSsh is not null) TsMultiSsh.Visible = true;
            }
        }

        private void mMenViewFullscreen_Click(object sender, EventArgs e)
        {
            if (FullscreenHandler is null) return;
            FullscreenHandler.Value = !FullscreenHandler.Value;
            _mMenViewFullscreen.Checked = FullscreenHandler.Value;
        }

        private void mMenViewPresentationMode_Click(object sender, EventArgs e)
        {
            if (PresentationMode is null) return;
            PresentationMode.Active = !PresentationMode.Active;
            _mMenViewPresentationMode.Checked = PresentationMode.Active;
        }

        private void mMenReconnectAll_Click(object sender, EventArgs e)
        {
            if (Runtime.WindowList == null || Runtime.WindowList.Count == 0) return;
            foreach (BaseWindow window in Runtime.WindowList)
            {
                if (!(window is ConnectionWindow connectionWindow))
                    return;

                connectionWindow.ReconnectAll(Runtime.ConnectionInitiator);
            }
        }

        #endregion
    }
}