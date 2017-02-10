using System;
using System.Drawing;
using System.Windows.Forms;
using mRemoteNG.Credential;

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
    }
}
