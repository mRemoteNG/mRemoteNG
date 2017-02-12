using System;
using System.Windows.Forms;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using mRemoteNG.UI.Controls.PageSequence;

namespace mRemoteNG.UI.Forms.CredentialManagerPages
{
    public partial class CredentialEditorPage : SequencedControl
    {
        private readonly ICredentialRecord _credentialRecord;
        private readonly ICredentialRepository _credentialRepository;

        public CredentialEditorPage(ICredentialRecord credentialRecord, ICredentialRepository credentialRepository)
        {
            if (credentialRecord == null)
                throw new ArgumentNullException(nameof(credentialRecord));
            if (credentialRepository == null)
                throw new ArgumentNullException(nameof(credentialRepository));

            InitializeComponent();
            ApplyLanguage();
            _credentialRecord = credentialRecord;
            _credentialRepository = credentialRepository;
            FillInForm();
            Dock = DockStyle.Fill;
        }

        private void ApplyLanguage()
        {
            Text = Language.strCredentialEditor;
            labelId.Text = Language.strID;
            labelTitle.Text = Language.strTitle;
            labelUsername.Text = Language.strPropertyNameUsername;
            labelDomain.Text = Language.strPropertyNameDomain;
            labelPassword.Text = Language.strPropertyNamePassword;
            buttonAccept.Text = Language.strAccept;
            buttonCancel.Text = Language.strButtonCancel;
        }

        private void FillInForm()
        {
            textBoxRepositoryName.Text = _credentialRepository.Config.Title;
            textBoxId.Text = _credentialRecord.Id.ToString();
            textBoxTitle.Text = _credentialRecord.Title;
            textBoxUsername.Text = _credentialRecord.Username;
            textBoxDomain.Text = _credentialRecord.Domain;
            textBoxPassword.Text = _credentialRecord.Password.ConvertToUnsecureString();
        }

        private void SaveFormToCredential()
        {
            _credentialRecord.Title = textBoxTitle.Text;
            _credentialRecord.Username = textBoxUsername.Text;
            _credentialRecord.Domain = textBoxDomain.Text;
            _credentialRecord.Password = textBoxPassword.Text.ConvertToSecureString();
        }

        private void buttonAccept_Click_1(object sender, EventArgs e)
        {
            SaveFormToCredential();
            RaiseNextPageEvent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            RaisePreviousPageEvent();
        }
    }
}