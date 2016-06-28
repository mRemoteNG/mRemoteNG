using System;
using System.Drawing;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Credential;
using mRemoteNG.UI.Controls;

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
            BindCredentialSourceToComboBox();
            PopulateFormWithCredentialData();
            Load += FrmCredentialEditor_OnLoad;
        }

        private void BindCredentialSourceToComboBox()
        {
            comboBoxSourceSelector.DataSource = Enum.GetValues(typeof(CredentialSource));
        }

        private void FrmCredentialEditor_OnLoad(object sender, EventArgs eventArgs)
        {
            if (_credentialInfo.Password?.Length > 0)
                secureTextBoxManualEntryPassword.CueBanner(true, Language.strPasswordHidden);
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
            comboBoxSourceSelector.SelectedText = _credentialInfo.CredentialSource.ToString();
            PopulateManualEntryForm();
        }

        private void PopulateManualEntryForm()
        {
            txtManualEntryUsername.Text = _credentialInfo.Username;
            txtManualEntryDomain.Text = _credentialInfo.Domain;
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

        private void txtEntryName_TextChanged(object sender, EventArgs e)
        {
            _credentialInfo.Name = txtEntryName.Text;
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

        private void comboBoxSourceSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            _credentialInfo.CredentialSource = (CredentialSource)comboBoxSourceSelector.SelectedValue;
        }
    }
}