using System;
using System.Drawing;
using System.Windows.Forms;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Forms.CredentialManagerPages.CredentialRepositorySelectors;

namespace mRemoteNG.UI.Forms.CredentialManagerPages
{
    public partial class CredentialRepositoriesPage : UserControl, ICredentialManagerPage
    {
        private readonly ICredentialProviderCatalog _providerCatalog;

        public string PageName { get; } = "Sources";
        public Image PageIcon { get; } = Resources.folder_key;

        public CredentialRepositoriesPage(ICredentialProviderCatalog providerCatalog)
        {
            if (providerCatalog == null)
                throw new ArgumentNullException(nameof(providerCatalog));

            _providerCatalog = providerCatalog;
            InitializeComponent();
            SetupObjectListView();
        }

        private void SetupObjectListView()
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
    }
}
