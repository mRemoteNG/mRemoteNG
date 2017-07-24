using System;
using System.Windows.Forms;
using mRemoteNG.Credential;
using mRemoteNG.UI.Controls.PageSequence;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Forms.CredentialManagerPages
{
    public partial class CredentialRepositorySelectionPage : SequencedControl
    {
        public CredentialRepositorySelectionPage(ICredentialRepositoryList credentialRepositoryList)
        {
            if (credentialRepositoryList == null)
                throw new ArgumentNullException(nameof(credentialRepositoryList));

            InitializeComponent();
            ApplyTheme();
            Dock = DockStyle.Fill;
            credentialRepositoryListView.RepositoryFilter = repository => repository.IsLoaded;
            credentialRepositoryListView.CredentialRepositoryList = credentialRepositoryList;
            credentialRepositoryListView.DoubleClickHandler = DoubleClickHandler;
        }

        private void ApplyTheme()
        {
            BackColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            ForeColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
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