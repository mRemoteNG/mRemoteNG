using System.Collections.Generic;
using System.Windows.Forms;
using mRemoteNG.Credential;

namespace mRemoteNG.UI.Forms
{
    public partial class CredentialImportForm : Form
    {
        private List<ICredentialRecord> _credentialRecords;

        public List<ICredentialRecord> CredentialRecords
        {
            get => _credentialRecords;
            set
            {
                _credentialRecords = value;
                olvCredentials.SetObjects(_credentialRecords);
            }
        }

        public CredentialImportForm()
        {
            InitializeComponent();

            colUsername.AspectName = nameof(ICredentialRecord.Username);
            colDomain.AspectName = nameof(ICredentialRecord.Domain);
        }

        private void buttonAccept_Click(object sender, System.EventArgs e)
        {

        }
    }
}
