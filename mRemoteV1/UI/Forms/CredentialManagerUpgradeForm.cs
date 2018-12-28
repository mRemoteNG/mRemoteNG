using System;
using System.Windows.Forms;
using System.Xml.Linq;
using mRemoteNG.App;
using mRemoteNG.Config.Connections;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Config.Serializers.Versioning;
using mRemoteNG.Connection;
using mRemoteNG.Credential;
using mRemoteNG.Tree;

namespace mRemoteNG.UI.Forms
{
    public partial class CredentialManagerUpgradeForm : Form, IDeserializer<string, ConnectionTreeModel>
    {
        private string _connectionFilePath;
        private string _newCredentialRepoPath;

        public IDeserializer<string, ConnectionTreeModel> ConnectionDeserializer { get; set; }
        public ConnectionsService ConnectionsService { get; set; }
        public CredentialServiceFacade CredentialService { get; set; }

        public string ConnectionFilePath
        {
            get => _connectionFilePath;
            set
            {
                _connectionFilePath = value;
                UpdateUi();
            }
        }

        public string NewCredentialRepoPath
        {
            get => _newCredentialRepoPath;
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
            if (!XmlCredentialManagerUpgrader.CredentialManagerUpgradeNeeded(xdoc))
                return ConnectionDeserializer.Deserialize(serializedData);

            // close the splash screen during upgrade
            FrmSplashScreen.getInstance().Close();

            var result = ShowDialog();
            if (result != DialogResult.OK)
                return new ConnectionTreeModel();

            var newRepoPassword = newRepositoryPasswordEntry.SecureString;
            var upgradingDeserializer = new XmlCredentialManagerUpgrader(CredentialService, NewCredentialRepoPath, ConnectionDeserializer, newRepoPassword);
            var connectionTreeModel = upgradingDeserializer.Deserialize(serializedData);

            ConnectionsService.ConnectionsLoaded += ConnectionsServiceOnConnectionsLoaded;

            return connectionTreeModel;
        }

        /// <summary>
        /// Request that the upgraded connection file be saved immediately after the upgrade is complete
        /// </summary>
        private void ConnectionsServiceOnConnectionsLoaded(object sender, ConnectionsLoadedEventArgs connectionsLoadedEventArgs)
        {
            ConnectionsService.ConnectionsLoaded -= ConnectionsServiceOnConnectionsLoaded;
            ConnectionsService.SaveConnectionsAsync();
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
            var loadConnectionsDialog = DialogFactory.BuildLoadConnectionsDialog();
            var dialogResult = loadConnectionsDialog.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
                ConnectionFilePath = loadConnectionsDialog.FileName;
        }

        private void buttonNewFile_Click(object sender, EventArgs e)
        {
            var dialogResult = newConnectionsFileDialog.ShowDialog(this);
            if (dialogResult != DialogResult.OK) return;
            Runtime.ConnectionsService.NewConnectionsFile(newConnectionsFileDialog.FileName);
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