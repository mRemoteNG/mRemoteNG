using System;
using System.Windows.Forms;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Tools;
using mRemoteNG.UI.Controls.PageSequence;

namespace mRemoteNG.UI.Forms.CredentialManager.RepoProviders.Xml
{
    public partial class XmlCredentialRepositoryEditorPage : SequencedControl
    {
        private readonly ICredentialRepositoryConfig _repositoryConfig;
        private readonly ICredentialRepositoryList _repositoryList;
        private readonly ICredentialRepositoryFactory _repositoryFactory;
        private readonly PageWorkflowController _pageWorkflowController;

        public XmlCredentialRepositoryEditorPage(
            ICredentialRepositoryConfig repositoryConfig, 
            ICredentialRepositoryList repositoryList,
            ICredentialRepositoryFactory repositoryFactory,
            PageWorkflowController pageWorkflowController)
        {
            _repositoryConfig = repositoryConfig.ThrowIfNull(nameof(repositoryConfig));
            _repositoryList = repositoryList.ThrowIfNull(nameof(repositoryList));
            _repositoryFactory = repositoryFactory.ThrowIfNull(nameof(repositoryFactory));
            _pageWorkflowController = pageWorkflowController.ThrowIfNull(nameof(pageWorkflowController));

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
            if (!AllRequiredFieldsFilledOut())
                return;

            SaveValuesToConfig();

            if (!_repositoryList.Contains(_repositoryConfig))
            {
                AddNewRepo();
            }

            //RaiseNextPageEvent();
            _pageWorkflowController.NextPage(_pageWorkflowController.StartPage);
        }

        private void AddNewRepo()
        {
            var newCredentialRepository = _repositoryFactory.Build(_repositoryConfig, true);
            _repositoryList.AddProvider(newCredentialRepository);
            newCredentialRepository.SaveCredentials(_repositoryConfig.Key);
        }

        private bool AllRequiredFieldsFilledOut()
        {
            return newPasswordBoxes.PasswordsMatch && !string.IsNullOrEmpty(textBoxFilePath.Text);
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            _pageWorkflowController.PreviousPage();
            //RaisePreviousPageEvent();
        }
    }
}