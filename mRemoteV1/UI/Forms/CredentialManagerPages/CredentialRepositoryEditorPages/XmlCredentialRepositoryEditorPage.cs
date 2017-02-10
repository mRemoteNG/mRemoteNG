using System;
using System.Windows.Forms;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Credential.Repositories;

namespace mRemoteNG.UI.Forms.CredentialManagerPages.CredentialRepositoryEditorPages
{
    public partial class XmlCredentialRepositoryEditorPage : UserControl
    {
        private readonly ICredentialRepositoryConfig _repositoryConfig;
        private readonly Control _previousPage;

        public XmlCredentialRepositoryEditorPage(ICredentialRepositoryConfig repositoryConfig, Control previousPage)
        {
            if (repositoryConfig == null)
                throw new ArgumentNullException(nameof(repositoryConfig));

            _repositoryConfig = repositoryConfig;
            _previousPage = previousPage;
            InitializeComponent();
            PopulateFields();
            textBoxFilePath.TextChanged += SaveValuesToConfig;
        }

        private void PopulateFields()
        {
            txtboxId.Text = _repositoryConfig.Id.ToString();
            textBoxFilePath.Text = _repositoryConfig.Source;
        }

        private void SaveValuesToConfig(object sender, EventArgs eventArgs)
        {
            _repositoryConfig.Source = textBoxFilePath.Text;
            _repositoryConfig.Key = txtboxPassword.SecureString;
        }

        private void buttonBrowseFiles_Click(object sender, EventArgs e)
        {
            var dialogResult = selectFilePathDialog.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                textBoxFilePath.Text = selectFilePathDialog.FileName;
            }
        }

        private void buttonConfirm_Click(object sender, EventArgs e)
        {
            var dataProvider = new FileDataProvider(_repositoryConfig.Source);
            var deserializer = new XmlCredentialDeserializer();
            var repository = new XmlCredentialRepository(_repositoryConfig, dataProvider, deserializer);
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            var parent = Parent;
            parent.Controls.Clear();
            parent.Controls.Add(_previousPage);
        }
    }
}