using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
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
            objectListView1.MouseDoubleClick += ObjectListView1OnMouseDoubleClick;
        }

        private void ObjectListView1OnSelectionChanged(object sender, EventArgs eventArgs)
        {
            var selectedRepository = objectListView1.SelectedObject as ICredentialRepository;
            buttonRemove.Enabled = selectedRepository != null;
            buttonEdit.Enabled = selectedRepository != null;
        }

        private void ObjectListView1OnMouseDoubleClick(object sender, MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.Clicks < 2) return;
            OLVColumn column;
            var listItem = objectListView1.GetItemAt(mouseEventArgs.X, mouseEventArgs.Y, out column);
            var clickedNode = listItem.RowObject as ICredentialRepository;
            if (clickedNode == null) return;
            EditRepository(clickedNode);
        }

        private void UpdateList()
        {
            objectListView1.SetObjects(_providerCatalog.CredentialProviders);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var addRepoSequence = new PageSequence(Parent,
                this,
                new CredentialRepositorySelectionPage(
                    new ISelectionTarget<ICredentialRepositoryConfig>[]
                    {
                        new XmlCredentialRepositorySelector(),
                        new KeePassRepositorySelector()
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
            var selectedRepository = objectListView1.SelectedObject as ICredentialRepository;
            if (selectedRepository == null) return;
            EditRepository(selectedRepository);
        }

        private void EditRepository(ICredentialRepository repository)
        {
            var editorPage = CredentialRepositoryPageEditorFactory.BuildXmlCredentialRepositoryEditorPage(repository.Config, _providerCatalog);
            var pageSequence = new PageSequence(Parent,
                this,
                editorPage,
                this
            );
            RaiseNextPageEvent();
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