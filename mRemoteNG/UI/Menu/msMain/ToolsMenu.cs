using System;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Credential;
using mRemoteNG.Resources.Language;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;

namespace mRemoteNG.UI.Menu
{
    [SupportedOSPlatform("windows")]
    public class ToolsMenu : ToolStripMenuItem
    {
        private ToolStripMenuItem _mMenToolsSshTransfer = null!;
        private ToolStripMenuItem _mMenToolsExternalApps = null!;
        private ToolStripMenuItem _mMenToolsPortScan = null!;
        private ToolStripMenuItem _mMenToolsConnectionTester = null!;
        private ToolStripMenuItem _mMenToolsUvncsc = null!;
        private ToolStripMenuItem _mMenToolsFindInSession = null!;
        private ToolStripMenuItem _mMenToolsQuickImport = null!;

        public Form? MainForm { get; set; }
        public ICredentialRepositoryList? CredentialProviderCatalog { get; set; }

        public ToolsMenu()
        {
            Initialize();
        }

        private void Initialize()
        {
            _mMenToolsSshTransfer = new ToolStripMenuItem();
            _mMenToolsUvncsc = new ToolStripMenuItem();
            _mMenToolsExternalApps = new ToolStripMenuItem();
            _mMenToolsPortScan = new ToolStripMenuItem();
            _mMenToolsConnectionTester = new ToolStripMenuItem();
            _mMenToolsFindInSession = new ToolStripMenuItem();
            _mMenToolsQuickImport = new ToolStripMenuItem();
            // 
            // mMenTools
            // 
            DropDownItems.AddRange(new ToolStripItem[]
            {
                _mMenToolsSshTransfer,
                _mMenToolsUvncsc,
                _mMenToolsExternalApps,
                _mMenToolsPortScan,
                _mMenToolsConnectionTester,
                _mMenToolsFindInSession,
                _mMenToolsQuickImport
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
            _mMenToolsSshTransfer.Text = Language.SshFileTransferMenuItem;
            _mMenToolsSshTransfer.Click += mMenToolsSSHTransfer_Click;
            // 
            // mMenToolsUVNCSC
            // 
            _mMenToolsUvncsc.Name = "mMenToolsUVNCSC";
            _mMenToolsUvncsc.Size = new System.Drawing.Size(184, 22);
            _mMenToolsUvncsc.Text = Language.UltraVNCSingleClickMenuItem;
            _mMenToolsUvncsc.Visible = false;
            _mMenToolsUvncsc.Click += mMenToolsUVNCSC_Click;
            // 
            // mMenToolsExternalApps
            // 
            _mMenToolsExternalApps.Image = Properties.Resources.Console_16x;
            _mMenToolsExternalApps.Name = "mMenToolsExternalApps";
            _mMenToolsExternalApps.Size = new System.Drawing.Size(184, 22);
            _mMenToolsExternalApps.Text = Language.ExternalToolsMenuItem;
            _mMenToolsExternalApps.Click += mMenToolsExternalApps_Click;
            // 
            // mMenToolsPortScan
            // 
            _mMenToolsPortScan.Image = Properties.Resources.SearchAndApps_16x;
            _mMenToolsPortScan.Name = "mMenToolsPortScan";
            _mMenToolsPortScan.Size = new System.Drawing.Size(184, 22);
                        _mMenToolsPortScan.Text = Language.PortScanMenuItem;
                        _mMenToolsPortScan.Click += mMenToolsPortScan_Click;
                        //
                        // mMenToolsConnectionTester
                        //
                        _mMenToolsConnectionTester.Image = Properties.Resources.SearchAndApps_16x;
                        _mMenToolsConnectionTester.Name = "mMenToolsConnectionTester";
                        _mMenToolsConnectionTester.Size = new System.Drawing.Size(184, 22);
                        _mMenToolsConnectionTester.Text = "Connection Tester";
                        _mMenToolsConnectionTester.Click += mMenToolsConnectionTester_Click;
                        //
                        // mMenToolsFindInSession
                        //              _mMenToolsFindInSession.Name = "mMenToolsFindInSession";
            _mMenToolsFindInSession.Size = new System.Drawing.Size(184, 22);
            _mMenToolsFindInSession.Text = "Find in Session";
            _mMenToolsFindInSession.ShortcutKeyDisplayString = "Ctrl+F";
            _mMenToolsFindInSession.Click += mMenToolsFindInSession_Click;
            // 
            // mMenToolsQuickImport
            // 
            _mMenToolsQuickImport.Name = "mMenToolsQuickImport";
            _mMenToolsQuickImport.Size = new System.Drawing.Size(184, 22);
            _mMenToolsQuickImport.Text = "Quick Import";
            _mMenToolsQuickImport.Click += mMenToolsQuickImport_Click;
        }

        public void ApplyLanguage()
        {
            Text = Language._Tools;
            _mMenToolsSshTransfer.Text = Language.SshFileTransferMenuItem;
            _mMenToolsExternalApps.Text = Language.ExternalToolsMenuItem;
            _mMenToolsPortScan.Text = Language.PortScanMenuItem;
            _mMenToolsConnectionTester.Text = "Connection Tester";
            _mMenToolsFindInSession.Text = "Find in Session";
            _mMenToolsQuickImport.Text = "Quick Import";
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

        private void mMenToolsPortScan_Click(object sender, EventArgs e)
        {
            AppWindows.Show(WindowType.PortScan);
        }

        private void mMenToolsConnectionTester_Click(object sender, EventArgs e)
        {
            AppWindows.Show(WindowType.ConnectionTester);
        }

        private void mMenToolsOptions_Click(object sender, EventArgs e)
        {
            AppWindows.Show(WindowType.Options);
        }

        private void mMenToolsFindInSession_Click(object sender, EventArgs e)
        {
            if (MainForm is FrmMain frmMain && frmMain.pnlDock.ActiveDocument is ConnectionWindow connectionWindow)
            {
                connectionWindow.FindInSession();
            }
        }
        
        private void mMenToolsQuickImport_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmQuickImport())
            {
                frm.ShowDialog(MainForm);
            }
        }

        #endregion
    }
}