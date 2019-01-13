using System.Collections.Generic;
using System.Windows.Forms;
using mRemoteNG.Credential;

namespace mRemoteNG.UI.Controls
{
    public partial class CredentialRecordComboBox : ComboBox
    {
        private IEnumerable<ICredentialRecord> _credentialRecords;

        public IEnumerable<ICredentialRecord> CredentialRecords
        {
            get => _credentialRecords;
            set
            {
                _credentialRecords = value;
                PopulateItems(_credentialRecords);
            }
        }

        public CredentialRecordComboBox()
        {
            InitializeComponent();
            PopulateItems(CredentialRecords);
        }

        private void PopulateItems(IEnumerable<ICredentialRecord> credentialRecords)
        {
            if (credentialRecords == null) return;
            Items.Clear();
            foreach (var credential in credentialRecords)
                Items.Add(credential);
        }
    }
}