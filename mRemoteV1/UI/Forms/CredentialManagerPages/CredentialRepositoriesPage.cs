using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Forms.CredentialManagerPages.CredentialRepositorySelectors;

namespace mRemoteNG.UI.Forms.CredentialManagerPages
{
    public partial class CredentialRepositoriesPage : UserControl, ICredentialManagerPage
    {
        private readonly ICredentialRepositoryList _providerCatalog;

        public string PageName { get; } = "Sources";
        public Image PageIcon { get; } = Resources.folder_key;

        public CredentialRepositoriesPage(ICredentialRepositoryList providerCatalog)
        {
            if (providerCatalog == null)
                throw new ArgumentNullException(nameof(providerCatalog));

            _providerCatalog = providerCatalog;
            InitializeComponent();
            SetupObjectListView();
        }

        private void SetupObjectListView()
        {
            olvColumnProvider.AspectGetter = rowObject => ((ICredentialRepository) rowObject).Config.Name;
            olvColumnSource.AspectGetter = rowObject => ((ICredentialRepository)rowObject).Config.Source;
            objectListView1.SetObjects(_providerCatalog.CredentialProviders);
            _providerCatalog.CollectionChanged += (sender, args) => UpdateList();
            objectListView1.SelectionChanged += ObjectListView1OnSelectionChanged;
        }

        private void ObjectListView1OnSelectionChanged(object sender, EventArgs eventArgs)
        {
            var selectedRepository = objectListView1.SelectedObject as ICredentialRepository;
            buttonRemove.Enabled = selectedRepository != null;
        }

        private void UpdateList()
        {
            objectListView1.SetObjects(_providerCatalog.CredentialProviders);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var repoSelector = new CredentialRepositorySelectionPage(
                new ISelectionTarget<ICredentialRepositoryConfig>[]
                {
                    new XmlCredentialRepositorySelector(),
                    new KeePassRepositorySelector()
                },
                _providerCatalog,
                this
            ) {Dock = DockStyle.Fill};
            var parent = Parent;
            parent.Controls.Clear();
            parent.Controls.Add(repoSelector);
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            var selectedRepository = objectListView1.SelectedObject as ICredentialRepository;
            if (selectedRepository == null) return;
            if (_providerCatalog.Contains(selectedRepository))
                _providerCatalog.RemoveProvider(selectedRepository);
        }
    }
}
