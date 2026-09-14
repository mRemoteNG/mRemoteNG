using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Credential;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Menu
{
    [SupportedOSPlatform("windows")]
    public class ToolsMenu : ToolStripMenuItem
    {
        private ToolStripMenuItem _mMenToolsSshTransfer;
        private ToolStripMenuItem _mMenToolsExternalApps;
        private ToolStripMenuItem _mMenToolsUvncsc;
        private readonly List<ToolStripMenuItem> _pluginMenuItems = [];

        public Form MainForm { get; set; }
        public ICredentialRepositoryList CredentialProviderCatalog { get; set; }

        public ToolsMenu()
        {
            Initialize();
        }

        private void Initialize()
        {
            _mMenToolsSshTransfer = new ToolStripMenuItem();
            _mMenToolsUvncsc = new ToolStripMenuItem();
            _mMenToolsExternalApps = new ToolStripMenuItem();
            // 
            // mMenTools
            // 
            DropDownItems.AddRange(new ToolStripItem[]
            {
                _mMenToolsSshTransfer,
                _mMenToolsUvncsc,
                _mMenToolsExternalApps
            });
            Name = "mMenTools";
            Size = new System.Drawing.Size(48, 20);
            Text = Language._Tools;
            // 
            // mMenToolsSSHTransfer
            // 
            _mMenToolsSshTransfer.Image = Properties.Resources.SyncArrow_16x;
            _mMenToolsSshTransfer.Name = "mMenToolsSSHTransfer";
            _mMenToolsSshTransfer.Size = new System.Drawing.Size(184, 22);
            _mMenToolsSshTransfer.Text = Language.SshFileTransfer;
            _mMenToolsSshTransfer.Click += mMenToolsSSHTransfer_Click;
            // 
            // mMenToolsUVNCSC
            // 
            _mMenToolsUvncsc.Name = "mMenToolsUVNCSC";
            _mMenToolsUvncsc.Size = new System.Drawing.Size(184, 22);
            _mMenToolsUvncsc.Text = Language.UltraVNCSingleClick;
            _mMenToolsUvncsc.Visible = false;
            _mMenToolsUvncsc.Click += mMenToolsUVNCSC_Click;
            // 
            // mMenToolsExternalApps
            // 
            _mMenToolsExternalApps.Image = Properties.Resources.Console_16x;
            _mMenToolsExternalApps.Name = "mMenToolsExternalApps";
            _mMenToolsExternalApps.Size = new System.Drawing.Size(184, 22);
            _mMenToolsExternalApps.Text = Language.ExternalTool;
            _mMenToolsExternalApps.Click += mMenToolsExternalApps_Click;
            // 
        }

        public void ApplyLanguage()
        {
            Text = Language._Tools;
            _mMenToolsSshTransfer.Text = Language.SshFileTransfer;
            _mMenToolsExternalApps.Text = Language.ExternalTool;
            RefreshPluginItems();
        }

        public void RefreshPluginItems()
        {
            foreach (ToolStripMenuItem item in _pluginMenuItems)
            {
                DropDownItems.Remove(item);
                item.Dispose();
            }

            _pluginMenuItems.Clear();

            foreach (var plugin in Runtime.PluginService.GetToolsMenuPlugins().OrderBy(plugin => plugin.ToolWindow.SortOrder))
            {
                ToolStripMenuItem item = new()
                {
                    Name = $"plugin_{plugin.Id}",
                    Text = plugin.ToolWindow.MenuText,
                    Image = plugin.ToolWindow.Icon,
                };
                item.Click += (_, _) => Runtime.PluginService.ShowToolWindow(plugin.Id);
                _pluginMenuItems.Add(item);
                DropDownItems.Add(item);
            }
        }

        #region Tools

        private void mMenToolsSSHTransfer_Click(object sender, EventArgs e)
        {
            AppWindows.Show(WindowType.SSHTransfer);
        }

        private void mMenToolsUVNCSC_Click(object sender, EventArgs e)
        {
            AppWindows.Show(WindowType.UltraVNCSC);
        }

        private void mMenToolsExternalApps_Click(object sender, EventArgs e)
        {
            AppWindows.Show(WindowType.ExternalApps);
        }

        private void mMenToolsOptions_Click(object sender, EventArgs e)
        {
            AppWindows.Show(WindowType.Options);
        }

        #endregion
    }
}