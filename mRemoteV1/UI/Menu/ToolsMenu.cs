using System;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Credential;
using mRemoteNG.UI.Forms;

namespace mRemoteNG.UI.Menu
{
    public class ToolsMenu : ToolStripMenuItem
    {
        private ToolStripSeparator _mMenToolsSep1;
        private ToolStripMenuItem _mMenToolsOptions;
        private ToolStripMenuItem _mMenToolsSshTransfer;
        private ToolStripMenuItem _mMenToolsExternalApps;
        private ToolStripMenuItem _mMenToolsPortScan;
        private ToolStripMenuItem _mMenToolsUvncsc;
        private ToolStripMenuItem _mMenToolsComponentsCheck;
        private ToolStripMenuItem _credentialManagerToolStripMenuItem;

        public CredentialManager CredentialManager { get; set; }
        public Form MainForm { get; set; }

        public ToolsMenu()
        {
            Initialize();
        }

        private void Initialize()
        {
            _credentialManagerToolStripMenuItem = new ToolStripMenuItem();
            _mMenToolsSshTransfer = new ToolStripMenuItem();
            _mMenToolsUvncsc = new ToolStripMenuItem();
            _mMenToolsExternalApps = new ToolStripMenuItem();
            _mMenToolsPortScan = new ToolStripMenuItem();
            _mMenToolsSep1 = new ToolStripSeparator();
            _mMenToolsComponentsCheck = new ToolStripMenuItem();
            _mMenToolsOptions = new ToolStripMenuItem();

            // 
            // mMenTools
            // 
            DropDownItems.AddRange(new ToolStripItem[] {
            _credentialManagerToolStripMenuItem,
            _mMenToolsSshTransfer,
            _mMenToolsUvncsc,
            _mMenToolsExternalApps,
            _mMenToolsPortScan,
            _mMenToolsSep1,
            _mMenToolsComponentsCheck,
            _mMenToolsOptions});
            Name = "mMenTools";
            Size = new System.Drawing.Size(48, 20);
            Text = Language.strMenuTools;
            // 
            // credentialManagerToolStripMenuItem
            // 
            _credentialManagerToolStripMenuItem.Image = Resources.key;
            _credentialManagerToolStripMenuItem.Name = "credentialManagerToolStripMenuItem";
            _credentialManagerToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            _credentialManagerToolStripMenuItem.Text = "Credential Manager";
            _credentialManagerToolStripMenuItem.Click += credentialManagerToolStripMenuItem_Click;
            // 
            // mMenToolsSSHTransfer
            // 
            _mMenToolsSshTransfer.Image = Resources.SSHTransfer;
            _mMenToolsSshTransfer.Name = "mMenToolsSSHTransfer";
            _mMenToolsSshTransfer.Size = new System.Drawing.Size(184, 22);
            _mMenToolsSshTransfer.Text = Language.strMenuSSHFileTransfer;
            _mMenToolsSshTransfer.Click += mMenToolsSSHTransfer_Click;
            // 
            // mMenToolsUVNCSC
            // 
            _mMenToolsUvncsc.Image = Resources.UVNC_SC;
            _mMenToolsUvncsc.Name = "mMenToolsUVNCSC";
            _mMenToolsUvncsc.Size = new System.Drawing.Size(184, 22);
            _mMenToolsUvncsc.Text = "UltraVNC SingleClick";
            _mMenToolsUvncsc.Visible = false;
            _mMenToolsUvncsc.Click += mMenToolsUVNCSC_Click;
            // 
            // mMenToolsExternalApps
            // 
            _mMenToolsExternalApps.Image = Resources.ExtApp;
            _mMenToolsExternalApps.Name = "mMenToolsExternalApps";
            _mMenToolsExternalApps.Size = new System.Drawing.Size(184, 22);
            _mMenToolsExternalApps.Text = Language.strMenuExternalTools;
            _mMenToolsExternalApps.Click += mMenToolsExternalApps_Click;
            // 
            // mMenToolsPortScan
            // 
            _mMenToolsPortScan.Image = Resources.PortScan;
            _mMenToolsPortScan.Name = "mMenToolsPortScan";
            _mMenToolsPortScan.Size = new System.Drawing.Size(184, 22);
            _mMenToolsPortScan.Text = Language.strMenuPortScan;
            _mMenToolsPortScan.Click += mMenToolsPortScan_Click;
            // 
            // mMenToolsSep1
            // 
            _mMenToolsSep1.Name = "mMenToolsSep1";
            _mMenToolsSep1.Size = new System.Drawing.Size(181, 6);
            // 
            // mMenToolsComponentsCheck
            // 
            _mMenToolsComponentsCheck.Image = Resources.cog_error;
            _mMenToolsComponentsCheck.Name = "mMenToolsComponentsCheck";
            _mMenToolsComponentsCheck.Size = new System.Drawing.Size(184, 22);
            _mMenToolsComponentsCheck.Text = Language.strComponentsCheck;
            _mMenToolsComponentsCheck.Click += mMenToolsComponentsCheck_Click;
            // 
            // mMenToolsOptions
            // 
            _mMenToolsOptions.Image = Resources.Options;
            _mMenToolsOptions.Name = "mMenToolsOptions";
            _mMenToolsOptions.Size = new System.Drawing.Size(184, 22);
            _mMenToolsOptions.Text = Language.strMenuOptions;
            _mMenToolsOptions.Click += mMenToolsOptions_Click;
        }

        #region Tools
        private void credentialManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var credentialManagerForm = new CredentialManagerForm(CredentialManager)
            {
                DeletionConfirmer = new CredentialDeletionMsgBoxConfirmer(MessageBox.Show)
            };
            credentialManagerForm.CenterOnTarget(MainForm);
            credentialManagerForm.Show();
        }

        private void mMenToolsSSHTransfer_Click(object sender, EventArgs e)
        {
            Windows.Show(WindowType.SSHTransfer);
        }

        private void mMenToolsUVNCSC_Click(object sender, EventArgs e)
        {
            Windows.Show(WindowType.UltraVNCSC);
        }

        private void mMenToolsExternalApps_Click(object sender, EventArgs e)
        {
            Windows.Show(WindowType.ExternalApps);
        }

        private void mMenToolsPortScan_Click(object sender, EventArgs e)
        {
            Windows.Show(WindowType.PortScan);
        }

        private void mMenToolsComponentsCheck_Click(object sender, EventArgs e)
        {
            Windows.Show(WindowType.ComponentsCheck);
        }

        private void mMenToolsOptions_Click(object sender, EventArgs e)
        {
            Windows.Show(WindowType.Options);
        }
        #endregion
    }
}