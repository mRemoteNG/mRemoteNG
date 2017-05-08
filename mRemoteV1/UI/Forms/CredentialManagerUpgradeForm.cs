using System;
using System.Windows.Forms;
using mRemoteNG.App;

namespace mRemoteNG.UI.Forms
{
    public partial class CredentialManagerUpgradeForm : Form
    {
        public string ConnectionFilePath { get; set; } = "C:\\somepath\\confCons.xml";
        public string NewCredentialRepoPath { get; set; }

        public CredentialManagerUpgradeForm()
        {
            InitializeComponent();
            ApplyLanguage();
            SetValues();
        }

        private void ApplyLanguage()
        {
            // tab 1
            labelDescriptionOfUpgrade.Text = "In v1.76 we have introduced a credential management system. This feature requires a significant change in how we store and interact with credentials within mRemoteNG. You will be required to perform a one-way upgrade of your mRemoteNG connections file.\r\n\r\nThis page will walk you through the process of upgrading your connections file or give you a chance to open a different connections file if you do not want to perform the upgrade.";
            labelConfConsPathHeaderOnTab1.Text = "Connection file path:";
            buttonPerformUpgrade.Text = "Upgrade";
            buttonOpenFile.Text = "Open a different file";
            buttonNewFile.Text = "Create and open new file";
            buttonExit.Text = Language.strMenuExit;

            // tab 2
            labelConfConsPathHeaderOnTab2.Text = "Connection file path:";
            labelWhereToSaveCredFile.Text = "Where should we save the new credential file?";
            labelSetPassword.Text = "Set password for the credential repository";
            buttonNewRepoPathBrowse.Text = Language.strButtonBrowse;
            buttonBack.Text = "Back";
            buttonExecuteUpgrade.Text = "Upgrade";
        }

        private void SetValues()
        {
            textBoxConfConPathTab1.Text = ConnectionFilePath;
            textBoxConfConPathTab2.Text = ConnectionFilePath;
            textBoxCredRepoPath.Text = NewCredentialRepoPath;
        }

        private void buttonPerformUpgrade_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabPageUpgradeOptions;
        }

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            var dialogResult = openDifferentFileDialog.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
                ConnectionFilePath = openDifferentFileDialog.FileName;
            SetValues();
        }

        private void buttonNewFile_Click(object sender, EventArgs e)
        {
            var dialogResult = newConnectionsFileDialog.ShowDialog(this);
            if (dialogResult != DialogResult.OK) return;
            Runtime.NewConnections(newConnectionsFileDialog.FileName);
            Close();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Shutdown.Quit();
        }

        private void buttonNewRepoPathBrowse_Click(object sender, EventArgs e)
        {
            var dialogResult = newCredRepoPathDialog.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
                NewCredentialRepoPath = newCredRepoPathDialog.FileName;
            SetValues();
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabPageWelcome;
        }
    }
}