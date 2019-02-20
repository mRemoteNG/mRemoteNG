using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Windows.Forms;
using System.Xml.Linq;
using mRemoteNG.App;
using mRemoteNG.Config;
using mRemoteNG.Config.Connections;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Config.Serializers.Versioning;
using mRemoteNG.Config.Serializers.Xml;
using mRemoteNG.Connection;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Security;
using mRemoteNG.Themes;
using mRemoteNG.Tree;

namespace mRemoteNG.UI.Forms
{
    public partial class CredentialManagerUpgradeForm : Form
    {
        private string _connectionFilePath;
        private string _newCredentialRepoPath;
        private XDocument _xdoc;
        private XmlCredentialManagerUpgrader _upgradingDeserializer;
        private ConnectionToCredentialMap _credentialMap;
        private readonly ThemeManager _themeManager = ThemeManager.getInstance();

        public XmlConnectionsDeserializer ConnectionDeserializer { get; set; }
        public ConnectionsService ConnectionsService { get; set; }
        public CredentialService CredentialService { get; set; }

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
            ApplyTheme();
            //UpdateUi();
            colPassword.AspectGetter =
                rowObject => (rowObject as ICredentialRecord)?.Password.ConvertToUnsecureString();
        }

        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                var createParams = base.CreateParams;
                createParams.ClassStyle = createParams.ClassStyle | CP_NOCLOSE_BUTTON;
                return createParams;
            }
        }

        public SerializationResult Deserialize(string serializedData)
        {
            _xdoc = XDocument.Parse(serializedData);
            if (!XmlCredentialManagerUpgrader.CredentialManagerUpgradeNeeded(_xdoc))
                return ConnectionDeserializer.Deserialize(serializedData);

            // close the splash screen during upgrade
            FrmSplashScreen.getInstance().Close();
            _upgradingDeserializer = new XmlCredentialManagerUpgrader(ConnectionDeserializer);

            var result = ShowDialog();
            if (result != DialogResult.OK)
                return new SerializationResult(new List<ConnectionInfo>(), new ConnectionToCredentialMap());

            var newRepoPassword = newRepositoryPasswordEntry.SecureString;
            SaveCredentialsToNewRepository(_credentialMap.DistinctCredentialRecords, newRepoPassword, _newCredentialRepoPath);
            var serializationResult = _upgradingDeserializer.Deserialize(serializedData, _credentialMap);

            ConnectionsService.ConnectionsLoaded += ConnectionsServiceOnConnectionsLoaded;

            return serializationResult;
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
            // Welcome tab
            labelDescriptionOfUpgrade.Text = Language.strCredentialManagerUpgradeDescription;
            labelConfConsPathHeaderOnTab1.Text = $@"{Language.strConnectionFilePath}:";
            buttonBeginUpgrade.Text = Language.strUpgrade;
            buttonOpenFile.Text = Language.strOpenADifferentFile;
            buttonNewFile.Text = Language.strCreateAndOpenNewFile;
            buttonExit.Text = Language.strMenuExit;

            // HarvestedCreds tab
            lblCredsFound.Text = $@"{Language.strCredentialsFound}:";
            colTitle.Text = Language.strPropertyNameName;
            colUsername.Text = Language.strPropertyNameUsername;
            colDomain.Text = Language.strPropertyNameDomain;
            colPassword.Text = Language.strPropertyNamePassword;
            btnCredsBack.Text = Language.strBack;
            btnCredsContinue.Text = Language.strContinue;

            // SaveRepo tab
            labelConfConsPathHeaderOnTab2.Text = $@"{Language.strConnectionFilePath}:";
            gbWhereToSaveCredFile.Text = $@"{Language.strConnectionFilePathPrompt}:";
            gbSetPassword.Text = $@"{Language.strConnectionPasswordPrompt}:";
            buttonNewRepoPathBrowse.Text = Language.strButtonBrowse;
            buttonSaveRepoBack.Text = Language.strBack;
            buttonExecuteUpgrade.Text = Language.strUpgrade;
        }

        private void ApplyTheme()
        {
            if (!_themeManager.ThemingActive)
                return;

            BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
        }

        private void UpdateUi()
        {
            textBoxConfConPathTab1.Text = ConnectionFilePath;
            textBoxConfConPathTab2.Text = ConnectionFilePath;
            textBoxCredRepoPath.Text = NewCredentialRepoPath;
        }

        #region WelcomePage
        private void buttonBeginUpgrade_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabPageHarvestedCreds;
            
            // harvest creds and update olv
            _credentialMap = _upgradingDeserializer.UpgradeUserFilesForCredentialManager(_xdoc);
            olvFoundCredentials.SetObjects(_credentialMap.DistinctCredentialRecords);
            olvFoundCredentials.AutoResizeColumns();
        }

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            var loadConnectionsDialog = DialogFactory.BuildLoadConnectionsDialog();
            var dialogResult = loadConnectionsDialog.ShowDialog(this);
            if (dialogResult != DialogResult.OK)
                return;

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
        #endregion

        #region HarvestedCredsPage

        private void btnCredsBack_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabPageWelcome;
        }

        private void btnCredsContinue_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabPageSaveRepo;
        }
        #endregion

        #region SaveRepoPage
        private void buttonNewRepoPathBrowse_Click(object sender, EventArgs e)
        {
            var dialogResult = newCredRepoPathDialog.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
                NewCredentialRepoPath = newCredRepoPathDialog.FileName;
        }

        private void buttonSaveRepoBack_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabPageHarvestedCreds;
        }

        private void textBoxCredRepoPath_TextChanged(object sender, EventArgs e)
        {
            ValidateFields();
        }

        private void newRepositoryPasswordEntry_Verified(object sender, EventArgs e)
        {
            ValidateFields();
        }

        private void newRepositoryPasswordEntry_NotVerified(object sender, EventArgs e)
        {
            ValidateFields();
        }

        /// <summary>
        /// Validate field entries to determine if we have enough information to perform the upgrade
        /// </summary>
        private void ValidateFields()
        {
            buttonExecuteUpgrade.Enabled = 
                newRepositoryPasswordEntry.PasswordsMatch && 
                newRepositoryPasswordEntry.SecureString.Length > 0 &&
                !string.IsNullOrWhiteSpace(textBoxCredRepoPath.Text);
        }
        #endregion

        private void SaveCredentialsToNewRepository(
            IEnumerable<ICredentialRecord> harvestedCredentials, 
            SecureString newRepoPassword,
            string repoPath)
        {
            var newCredentialRepository = BuildXmlCredentialRepo(newRepoPassword, repoPath);

            AddHarvestedCredentialsToRepo(harvestedCredentials, newCredentialRepository);

            newCredentialRepository.SaveCredentials(newRepoPassword);
            CredentialService.AddRepository(newCredentialRepository);
        }

        private ICredentialRepository BuildXmlCredentialRepo(SecureString newCredRepoKey, string repoPath)
        {
            var repositoryConfig = new CredentialRepositoryConfig
            {
                Source = repoPath,
                Title = "Converted Credentials",
                TypeName = "Xml",
                Key = newCredRepoKey
            };

            var xmlRepoFactory = CredentialService.GetRepositoryFactoryForConfig(repositoryConfig);

            if (!xmlRepoFactory.Any())
                throw new CredentialRepositoryTypeNotSupportedException(repositoryConfig.TypeName);

            var newRepo = xmlRepoFactory.First().Build(repositoryConfig, true);
            return newRepo;
        }

        private void AddHarvestedCredentialsToRepo(IEnumerable<ICredentialRecord> harvestedCredentials, ICredentialRepository repo)
        {
            foreach (var credential in harvestedCredentials)
                repo.CredentialRecords.Add(credential);
        }
    }
}