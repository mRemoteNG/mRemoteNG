using System;
using System.Windows.Forms;
using mRemoteNG.Config;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers.CredentialSerializer;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Security.Factories;
using mRemoteNG.UI.Controls.PageSequence;

namespace mRemoteNG.UI.Forms.CredentialManagerPages.CredentialRepositoryEditorPages
{
    public partial class XmlCredentialRepositoryEditorPage : SequencedControl
    {
        private readonly ICredentialRepositoryConfig _repositoryConfig;
        private readonly ICredentialRepositoryList _repositoryList;

        public XmlCredentialRepositoryEditorPage(ICredentialRepositoryConfig repositoryConfig, ICredentialRepositoryList repositoryList)
        {
            if (repositoryConfig == null)
                throw new ArgumentNullException(nameof(repositoryConfig));
            if (repositoryList == null)
                throw new ArgumentNullException(nameof(repositoryList));

            _repositoryConfig = repositoryConfig;
            _repositoryList = repositoryList;
            InitializeComponent();
            PopulateFields();
        }

        private void PopulateFields()
        {
            textBoxId.Text = _repositoryConfig.Id.ToString();
            textBoxTitle.Text = _repositoryConfig.Title;
            textBoxFilePath.Text = _repositoryConfig.Source;
            newPasswordBoxes.SetPassword(_repositoryConfig.Key);
        }

        private void SaveValuesToConfig()
        {
            _repositoryConfig.Title = textBoxTitle.Text;
            _repositoryConfig.Source = textBoxFilePath.Text;
            _repositoryConfig.Key = newPasswordBoxes.SecureString;
        }

        private void buttonBrowseFiles_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_repositoryConfig.Source))
                selectFilePathDialog.FileName = _repositoryConfig.Source;
            var dialogResult = selectFilePathDialog.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                textBoxFilePath.Text = selectFilePathDialog.FileName;
            }
        }

        private void buttonConfirm_Click(object sender, EventArgs e)
        {
            if (!AllRequiredFieldsFilledOut()) return;
            SaveValuesToConfig();
            if (!_repositoryList.Contains(_repositoryConfig))
            {
                var newCredentialRepository = BuildXmlRepoFromSettings(_repositoryConfig);
                _repositoryList.AddProvider(newCredentialRepository);
                newCredentialRepository.SaveCredentials(_repositoryConfig.Key);
            }
            RaiseNextPageEvent();
        }

        private ICredentialRepository BuildXmlRepoFromSettings(ICredentialRepositoryConfig config)
        {
            var cryptoFromSettings = new CryptoProviderFactoryFromSettings();
            var credRepoDataProvider = new FileDataProvider(config.Source);
            var credRepoSerializer = new XmlCredentialPasswordEncryptorDecorator(
                cryptoFromSettings.Build(),
                new XmlCredentialRecordSerializer());
            var credRepoDeserializer = new XmlCredentialPasswordDecryptorDecorator(new XmlCredentialRecordDeserializer());

            return new XmlCredentialRepository(
                config,
                new CredentialRecordSaver(credRepoDataProvider, credRepoSerializer),
                new CredentialRecordLoader(credRepoDataProvider, credRepoDeserializer)
            );
        }

        private bool AllRequiredFieldsFilledOut()
        {
            return newPasswordBoxes.PasswordsMatch && !string.IsNullOrEmpty(textBoxFilePath.Text);
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            RaisePreviousPageEvent();
        }
    }
}