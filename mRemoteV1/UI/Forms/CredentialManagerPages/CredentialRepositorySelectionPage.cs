using System;
using System.Windows.Forms;
using mRemoteNG.Credential;
using mRemoteNG.UI.Controls.PageSequence;

namespace mRemoteNG.UI.Forms.CredentialManagerPages
{
    public partial class CredentialRepositorySelectionPage : SequencedControl
    {
        public CredentialRepositorySelectionPage(ICredentialRepositoryList credentialRepositoryList)
        {
            if (credentialRepositoryList == null)
                throw new ArgumentNullException(nameof(credentialRepositoryList));

            InitializeComponent();
            Dock = DockStyle.Fill;
            credentialRepositoryListView.CredentialRepositoryList = credentialRepositoryList;
            credentialRepositoryListView.DoubleClickHandler = DoubleClickHandler;
        }

        private void Continue(ICredentialRepository credentialRepository)
        {
            var newCred = new CredentialRecord();
            var newCredPage = new CredentialEditorPage(newCred, credentialRepository);
            RaisePageReplacementEvent(newCredPage, RelativePagePosition.NextPage);
            RaiseNextPageEvent();
        }

        private bool DoubleClickHandler(ICredentialRepository credentialRepository)
        {
            if (credentialRepository == null) return false;
            Continue(credentialRepository);
            return true;
        }

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            if (credentialRepositoryListView.SelectedRepository == null) return;
            Continue(credentialRepositoryListView.SelectedRepository);
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            RaisePreviousPageEvent();
        }
    }
}