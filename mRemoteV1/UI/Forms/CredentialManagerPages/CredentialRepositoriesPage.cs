using System;
using System.Drawing;
using System.Windows.Forms;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Controls.PageSequence;
using mRemoteNG.UI.Forms.CredentialManagerPages.CredentialRepositoryEditorPages;
using mRemoteNG.UI.Forms.CredentialManagerPages.CredentialRepositorySelectors;

namespace mRemoteNG.UI.Forms.CredentialManagerPages
{
    public partial class CredentialRepositoriesPage : SequencedControl, ICredentialManagerPage
    {
        private readonly ICredentialRepositoryList _providerCatalog;
        private readonly UnlockerFormFactory _unlockerFactory;

        public string PageName { get; } = "Sources";
        public Image PageIcon { get; } = Resources.folder_key;

        public CredentialRepositoriesPage(ICredentialRepositoryList providerCatalog, UnlockerFormFactory unlockerFactory)
        {
            if (providerCatalog == null)
                throw new ArgumentNullException(nameof(providerCatalog));
            if (unlockerFactory == null)
                throw new ArgumentNullException(nameof(unlockerFactory));

            _providerCatalog = providerCatalog;
            _unlockerFactory = unlockerFactory;
            InitializeComponent();
            credentialRepositoryListView.CredentialRepositoryList = providerCatalog;
            credentialRepositoryListView.SelectionChanged += ObjectListView1OnSelectionChanged;
            credentialRepositoryListView.DoubleClickHandler = EditRepository;
        }

        private void ObjectListView1OnSelectionChanged(object sender, EventArgs eventArgs)
        {
            var selectedRepository = credentialRepositoryListView.SelectedRepository;
            buttonRemove.Enabled = selectedRepository != null;
            buttonEdit.Enabled = selectedRepository != null;
            buttonToggleLoad.Enabled = selectedRepository != null;
            UpdateLoadToggleButton(selectedRepository);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var addRepoSequence = new PageSequence(Parent,
                this,
                new CredentialRepositoryTypeSelectionPage(
                    new ISelectionTarget<ICredentialRepositoryConfig>[]
                    {
                        new XmlCredentialRepositorySelector(),
                        //new KeePassRepositorySelector()
                    },
                    _providerCatalog
                    )
                { Dock = DockStyle.Fill },
                new SequencedControl(),
                this
            );
            RaiseNextPageEvent();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            var selectedRepository = credentialRepositoryListView.SelectedRepository;
            if (selectedRepository == null) return;
            EditRepository(selectedRepository);
        }

        private bool EditRepository(ICredentialRepository repository)
        {
            var editorPage = CredentialRepositoryPageEditorFactory.BuildXmlCredentialRepositoryEditorPage(repository.Config, _providerCatalog);
            var pageSequence = new PageSequence(Parent,
                this,
                editorPage,
                this
            );
            RaiseNextPageEvent();
            return true;
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            var selectedRepository = credentialRepositoryListView.SelectedRepository;
            if (selectedRepository == null) return;
            if (_providerCatalog.Contains(selectedRepository.Config.Id))
                _providerCatalog.RemoveProvider(selectedRepository);
        }

        private void UpdateLoadToggleButton(ICredentialRepository selectedRepository)
        {
            if (selectedRepository == null) return;
            buttonToggleLoad.Text = selectedRepository.IsLoaded ? "Unload" : "Load";
        }

        private void buttonToggleLoad_Click(object sender, EventArgs e)
        {
            var selectedRepository = credentialRepositoryListView.SelectedRepository;
            if (selectedRepository.IsLoaded)
                selectedRepository.UnloadCredentials();
            else
                _unlockerFactory.Build(new[] {selectedRepository}).ShowDialog(this);
        }
    }
}