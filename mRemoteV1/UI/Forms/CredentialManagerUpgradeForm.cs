using System;
using System.Windows.Forms;
using System.Xml.Linq;
using mRemoteNG.App;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Config.Serializers.Versioning;
using mRemoteNG.Tree;

namespace mRemoteNG.UI.Forms
{
    public partial class CredentialManagerUpgradeForm : Form, IDeserializer<string, ConnectionTreeModel>
    {
        private string _connectionFilePath;
        private string _newCredentialRepoPath;

        public XmlCredentialManagerUpgrader DecoratedDeserializer { get; set; }

        public string ConnectionFilePath
        {
            get { return _connectionFilePath; }
            set
            {
                _connectionFilePath = value;
                UpdateUi();
            }
        }

        public string NewCredentialRepoPath
        {
            get { return _newCredentialRepoPath; }
            set
            {
                _newCredentialRepoPath = value; 
                UpdateUi();
            }
        }

        public CredentialManagerUpgradeForm()
        {
            InitializeComponent();
            ApplyLanguage();
            UpdateUi();
        }

        public ConnectionTreeModel Deserialize(string serializedData)
        {
            var xdoc = XDocument.Parse(serializedData);
            if (!WeCanUpgradeFromThisVersion(xdoc))
                return DecoratedDeserializer.Deserialize(serializedData);

            var result = ShowDialog();
            if (result != DialogResult.OK) return new ConnectionTreeModel();
            DecoratedDeserializer.CredentialFilePath = NewCredentialRepoPath;
            return DecoratedDeserializer.Deserialize(serializedData);
        }

        private bool WeCanUpgradeFromThisVersion(XDocument xdoc)
        {
            return double.Parse(xdoc.Root?.Attribute("ConfVersion")?.Value ?? "999") < 2.8;
        }

        private void ApplyLanguage()
        {
            // tab 1
            labelDescriptionOfUpgrade.Text = Language.strCredentialManagerUpgradeDescription;
            labelConfConsPathHeaderOnTab1.Text = $@"{Language.strConnectionFilePath}:";
            buttonPerformUpgrade.Text = Language.strUpgrade;
            buttonOpenFile.Text = Language.strOpenADifferentFile;
            buttonNewFile.Text = Language.strCreateAndOpenNewFile;
            buttonExit.Text = Language.strMenuExit;

            // tab 2
            labelConfConsPathHeaderOnTab2.Text = $@"{Language.strConnectionFilePath}:";
            labelWhereToSaveCredFile.Text = "Where should we save the new credential file?";
            labelSetPassword.Text = "Set password for the credential repository";
            buttonNewRepoPathBrowse.Text = Language.strButtonBrowse;
            buttonBack.Text = Language.strBack;
            buttonExecuteUpgrade.Text = Language.strUpgrade;
        }

        private void UpdateUi()
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
        }

        private void buttonNewFile_Click(object sender, EventArgs e)
        {
            var dialogResult = newConnectionsFileDialog.ShowDialog(this);
            if (dialogResult != DialogResult.OK) return;
            Runtime.ConnectionsService.NewConnections(newConnectionsFileDialog.FileName);
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
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabPageWelcome;
        }
    }
}