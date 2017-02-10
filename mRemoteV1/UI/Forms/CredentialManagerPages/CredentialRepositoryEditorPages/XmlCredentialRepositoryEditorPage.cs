using System;
using System.Windows.Forms;
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
        }

        private void PopulateFields()
        {
            txtboxId.Text = _repositoryConfig.Id.ToString();
            textBoxFilePath.Text = _repositoryConfig.Source;
        }

        private void buttonBrowseFiles_Click(object sender, EventArgs e)
        {
            selectFilePathDialog.ShowDialog(this);
        }

        private void buttonConfirm_Click(object sender, EventArgs e)
        {

        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            var parent = Parent;
            parent.Controls.Clear();
            parent.Controls.Add(_previousPage);
        }
    }
}