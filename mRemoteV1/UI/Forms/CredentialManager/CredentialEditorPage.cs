using System;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using mRemoteNG.Tools;
using mRemoteNG.UI.Controls.PageSequence;

namespace mRemoteNG.UI.Forms.CredentialManager
{
    public sealed partial class CredentialEditorPage : SequencedControl
    {
        private readonly ICredentialRecord _credentialRecord;
        private readonly CredentialService _credentialService;
        private readonly Optional<ICredentialRepository> _credentialRepository;

        public CredentialEditorPage(ICredentialRecord credentialRecord, Optional<ICredentialRepository> credentialRepository, CredentialService credentialService)
        {
            InitializeComponent();
            ApplyTheme();
            ApplyLanguage();
            _credentialRecord = credentialRecord.ThrowIfNull(nameof(credentialRecord));
            _credentialRepository = credentialRepository.ThrowIfNull(nameof(credentialRepository));
            _credentialService = credentialService.ThrowIfNull(nameof(credentialService));
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
            comboBoxRepository.DisplayMember = "Title";
            _credentialService.RepositoryList.ForEach(r => comboBoxRepository.Items.Add(r));

            if (_credentialRepository.Any())
                comboBoxRepository.SelectedItem = _credentialRepository.First();
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

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            SaveFormToCredential();

            var repo = comboBoxRepository.GetSelectedItemAs<ICredentialRepository>();
            if (!repo.CredentialRecords.Contains(_credentialRecord))
                repo.CredentialRecords.Add(_credentialRecord);

            RaiseNextPageEvent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            RaisePreviousPageEvent();
        }
    }
}