using System;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Credential;
using mRemoteNG.Resources.Language;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Tabs;
using mRemoteNG.UI.Window;

namespace mRemoteNG.UI.Menu
{
    [SupportedOSPlatform("windows")]
    public class ToolsMenu : ToolStripMenuItem
    {
        private ToolStripMenuItem _mMenToolsSshTransfer;
        private ToolStripMenuItem _mMenToolsSftpFileManager;
        private ToolStripMenuItem _mMenToolsExternalApps;
        private ToolStripMenuItem _mMenToolsPortScan;
        private ToolStripMenuItem _mMenToolsUvncsc;

        public Form MainForm { get; set; }
        public ICredentialRepositoryList CredentialProviderCatalog { get; set; }

        public ToolsMenu()
        {
            Initialize();
        }

        private void Initialize()
        {
            _mMenToolsSshTransfer = new ToolStripMenuItem();
            _mMenToolsSftpFileManager = new ToolStripMenuItem();
            _mMenToolsUvncsc = new ToolStripMenuItem();
            _mMenToolsExternalApps = new ToolStripMenuItem();
            _mMenToolsPortScan = new ToolStripMenuItem();
            // 
            // mMenTools
            // 
            DropDownItems.AddRange(new ToolStripItem[]
            {
                _mMenToolsSshTransfer,
                _mMenToolsSftpFileManager,
                _mMenToolsUvncsc,
                _mMenToolsExternalApps,
                _mMenToolsPortScan
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
            // mMenToolsSftpFileManager
            // 
            _mMenToolsSftpFileManager.Image = Properties.Resources.SyncArrow_16x;
            _mMenToolsSftpFileManager.Name = "mMenToolsSftpFileManager";
            _mMenToolsSftpFileManager.Size = new System.Drawing.Size(184, 22);
            _mMenToolsSftpFileManager.Text = Language.SftpFileManager;
            _mMenToolsSftpFileManager.Click += mMenToolsSftpFileManager_Click;
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
            // mMenToolsPortScan
            // 
            _mMenToolsPortScan.Image = Properties.Resources.SearchAndApps_16x;
            _mMenToolsPortScan.Name = "mMenToolsPortScan";
            _mMenToolsPortScan.Size = new System.Drawing.Size(184, 22);
            _mMenToolsPortScan.Text = Language.PortScan;
            _mMenToolsPortScan.Click += mMenToolsPortScan_Click;
        }

        public void ApplyLanguage()
        {
            Text = Language._Tools;
            _mMenToolsSshTransfer.Text = Language.SshFileTransfer;
            _mMenToolsSftpFileManager.Text = Language.SftpFileManager;
            _mMenToolsExternalApps.Text = Language.ExternalTool;
            _mMenToolsPortScan.Text = Language.PortScan;
        }

        #region Tools

        private void mMenToolsSSHTransfer_Click(object sender, EventArgs e)
        {
            AppWindows.Show(WindowType.SSHTransfer);
        }

        private void mMenToolsSftpFileManager_Click(object sender, EventArgs e)
        {
            mRemoteNG.Connection.ConnectionInfo connectionToUse = null;

            var activeWindow = FrmMain.Default.pnlDock?.ActiveDocument as ConnectionWindow;
            if (activeWindow != null)
            {
                var activeTab = activeWindow.connDock?.ActiveContent as ConnectionTab;
                if (activeTab?.Tag is InterfaceControl interfaceControl)
                {
                    var connectionInfo = interfaceControl.Info;
                    if (connectionInfo != null &&
                        (connectionInfo.Protocol == mRemoteNG.Connection.Protocol.ProtocolType.SSH1 ||
                         connectionInfo.Protocol == mRemoteNG.Connection.Protocol.ProtocolType.SSH2))
                    {
                        connectionToUse = connectionInfo;
                    }
                }
            }

            if (connectionToUse == null)
            {
                var selectedConnection = AppWindows.TreeForm?.SelectedNode;
                if (selectedConnection != null &&
                    (selectedConnection.Protocol == mRemoteNG.Connection.Protocol.ProtocolType.SSH1 ||
                     selectedConnection.Protocol == mRemoteNG.Connection.Protocol.ProtocolType.SSH2))
                {
                    connectionToUse = selectedConnection;
                }
            }

            if (connectionToUse != null)
            {
                AppWindows.Show(WindowType.SftpFileManagerWithConnection, connectionToUse);
            }
            else
            {
                AppWindows.Show(WindowType.SftpFileManagerWithConnection, null);
            }
        }

        private void mMenToolsUVNCSC_Click(object sender, EventArgs e)
        {
            AppWindows.Show(WindowType.UltraVNCSC);
        }

        private void mMenToolsExternalApps_Click(object sender, EventArgs e)
        {
            AppWindows.Show(WindowType.ExternalApps);
        }

        private void mMenToolsPortScan_Click(object sender, EventArgs e)
        {
            AppWindows.Show(WindowType.PortScan);
        }

        private void mMenToolsOptions_Click(object sender, EventArgs e)
        {
            AppWindows.Show(WindowType.Options);
        }

        #endregion
    }
}