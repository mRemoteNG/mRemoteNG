using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Tools;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Controls.PageSequence;
using mRemoteNG.UI.Forms.CredentialManagerPages.CredentialRepositoryEditorPages;
using mRemoteNG.UI.Forms.CredentialManagerPages.CredentialRepositorySelectors;

namespace mRemoteNG.UI.Forms.CredentialManagerPages
{
    public sealed partial class CredentialRepositoriesPage : SequencedControl, ICredentialManagerPage
    {
        private readonly CredentialService _credentialService;
        private readonly UnlockerFormFactory _unlockerFactory;

        public string PageName { get; } = "Sources";
        public Image PageIcon { get; } = Resources.folder_key;

        public CredentialRepositoriesPage(CredentialService credentialService, UnlockerFormFactory unlockerFactory)
        {
            _credentialService = credentialService.ThrowIfNull(nameof(credentialService));
            _unlockerFactory = unlockerFactory.ThrowIfNull(nameof(unlockerFactory));

            InitializeComponent();
            ApplyTheme();
            credentialRepositoryListView.CredentialRepositoryList = credentialService.RepositoryList;
            credentialRepositoryListView.SelectionChanged += (sender, args) => UpdateUi();
            credentialRepositoryListView.DoubleClickHandler = EditRepository;
        }



        private void UpdateUi()
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
                        new XmlCredentialRepositorySelector(_credentialService),
                        //new KeePassRepositorySelector()
                    },
                    _credentialService.RepositoryList
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
            if (selectedRepository == null)
                return;

            EditRepository(selectedRepository);
        }

        private bool EditRepository(ICredentialRepository repository)
        {
            if (!repository.IsLoaded)
                return false;

            var repositoryFactory = _credentialService.GetRepositoryFactoryForConfig(repository.Config);

            if (!repositoryFactory.Any())
                throw new CredentialRepositoryTypeNotSupportedException(repository.Config.TypeName);

            var editorPage = new XmlCredentialRepositoryEditorPage(repository.Config, _credentialService.RepositoryList, repositoryFactory.First())
            {
                Dock = DockStyle.Fill
            };

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
            if (selectedRepository == null)
                return;

            if (_credentialService.RepositoryList.Contains(selectedRepository.Config))
                _credentialService.RepositoryList.RemoveProvider(selectedRepository);
        }

        private void UpdateLoadToggleButton(ICredentialRepository selectedRepository)
        {
            if (selectedRepository == null)
                return;

            buttonToggleLoad.Text = selectedRepository.IsLoaded ? "Unload" : "Load";
        }

        private void buttonToggleLoad_Click(object sender, EventArgs e)
        {
            var selectedRepository = credentialRepositoryListView.SelectedRepository;
            if (selectedRepository.IsLoaded)
                selectedRepository.UnloadCredentials();
            else
                _unlockerFactory.Build(new[] {selectedRepository}).ShowDialog(this);
            credentialRepositoryListView.RefreshObjects();
            UpdateUi();
        }
    }
}