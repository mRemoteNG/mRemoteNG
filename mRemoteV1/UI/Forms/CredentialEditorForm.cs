using System;
using System.Windows.Forms;
using mRemoteNG.Credential;
using mRemoteNG.Security;


namespace mRemoteNG.UI.Forms
{
    public partial class CredentialEditorForm : Form
    {
        private readonly ICredentialRecord _credentialRecord;

        public CredentialEditorForm(ICredentialRecord credentialRecord)
        {
            if (credentialRecord == null)
                throw new ArgumentNullException(nameof(credentialRecord));

            InitializeComponent();
            _credentialRecord = credentialRecord;
            FillInForm();
        }

        private void FillInForm()
        {
            textBoxId.Text = _credentialRecord.Id.ToString();
            textBoxTitle.Text = _credentialRecord.Title;
            textBoxUsername.Text = _credentialRecord.Username;
            textBoxDomain.Text = _credentialRecord.Domain;
            textBoxPassword.Text = _credentialRecord.Password.ConvertToUnsecureString();
        }

        private void SaveFormToCredential()
        {
            if (_credentialRecord.Title != textBoxTitle.Text)
                _credentialRecord.Title = textBoxTitle.Text;

            if(_credentialRecord.Username != textBoxUsername.Text)
                _credentialRecord.Username = textBoxUsername.Text;

            if(_credentialRecord.Domain != textBoxDomain.Text)
                _credentialRecord.Domain = textBoxDomain.Text;

            if(_credentialRecord.Password.ConvertToUnsecureString() != textBoxPassword.Text)
                _credentialRecord.Password = textBoxPassword.Text.ConvertToSecureString();
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            SaveFormToCredential();
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
