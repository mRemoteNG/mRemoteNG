using System;
using System.Windows.Forms;
using mRemoteNG.Credential;

namespace mRemoteNG.UI.Forms
{
    public partial class FrmCredentialManager : Form
    {
        private CredentialList _credentialList;

        public FrmCredentialManager(CredentialList credentialList)
        {
            _credentialList = credentialList;
            InitializeComponent();
            Load += OnLoad;
            olvCredentialList.SelectedIndexChanged += OlvCredentialListOnSelectedIndexChanged;
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            PopulateCredentialList();
        }

        private void PopulateCredentialList()
        {
            _credentialList.CollectionChanged += CredentialList_CollectionChanged;
            olvCredentialList.SetObjects(_credentialList);
        }

        private void CredentialList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            olvCredentialList.SetObjects(_credentialList);
        }

        private void btnAddCredential_Click(object sender, EventArgs e)
        {
            ShowCredentialEditor();
        }

        private void btnEditCredential_Click(object sender, EventArgs e)
        {
            ShowCredentialEditor(olvCredentialList.SelectedObject as CredentialInfo);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ShowCredentialEditor(CredentialInfo credentialInfo = null)
        {
            var credentialEditor = new FrmCredentialEditor(credentialInfo);
            credentialEditor.Show();
        }

        private void btnRemoveCredential_Click(object sender, EventArgs e)
        {
            foreach (CredentialInfo selectedCredentialInfoitem in olvCredentialList.SelectedObjects)
            {
                _credentialList.Remove(selectedCredentialInfoitem);
            }
        }

        private void OlvCredentialListOnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            UpdateEditButtonEnabledState();
            UpdateRemoveButtonEnabledState();
        }

        private void UpdateEditButtonEnabledState()
        {
            btnEditCredential.Enabled = olvCredentialList.SelectedItems.Count > 0;
        }

        private void UpdateRemoveButtonEnabledState()
        {
            btnRemoveCredential.Enabled = olvCredentialList.SelectedItems.Count > 0;
        }
    }
}