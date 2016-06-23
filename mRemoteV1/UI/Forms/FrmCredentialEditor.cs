using System;
using System.Drawing;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Credential;

namespace mRemoteNG.UI.Forms
{
    public partial class FrmCredentialEditor : Form
    {
        private CredentialInfo _credentialInfo;

        public FrmCredentialEditor(CredentialInfo credentialInfo = null)
        {
            _credentialInfo = credentialInfo;
            InitializeCredentialData();
            InitializeComponent();
            HideTabControllerHeader();
            PopulateFormWithCredentialData();
        }

        private void InitializeCredentialData()
        {
            if (_credentialInfo == null)
                _credentialInfo = new CredentialInfo();
        }

        private void PopulateFormWithCredentialData()
        {
            txtEntryName.Text = _credentialInfo.Name;
            txtUUID.Text = _credentialInfo.Uuid;
            comboBoxSourceSelector.SelectedText = _credentialInfo.CredentialSource;
        }

        private void HideTabControllerHeader()
        {
            tabControlCredentialEditor.Appearance = TabAppearance.FlatButtons;
            tabControlCredentialEditor.ItemSize = new Size(0, 1);
            tabControlCredentialEditor.SizeMode = TabSizeMode.Fixed;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            CommitChanges();
            Close();
        }

        private void CommitChanges()
        {
            if (Runtime.CredentialList.Contains(_credentialInfo))
                UpdateCredentialEntry();
            else
                AddCredentialEntry();
        }

        private void UpdateCredentialEntry()
        {
            Runtime.CredentialList.Replace(_credentialInfo);
        }

        private void AddCredentialEntry()
        {
            Runtime.CredentialList.Add(_credentialInfo);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtManualEntryUsername_TextChanged(object sender, EventArgs e)
        {
            _credentialInfo.Username = txtManualEntryUsername.Text;
        }

        private void txtManualEntryDomain_TextChanged(object sender, EventArgs e)
        {
            _credentialInfo.Domain = txtManualEntryDomain.Text;
        }

        private void secureTextBoxManualEntryPassword_TextChanged(object sender, EventArgs e)
        {
            _credentialInfo.Password = secureTextBoxManualEntryPassword.SecureString;
        }
    }
}