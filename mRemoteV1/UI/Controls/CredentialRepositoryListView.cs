using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Tools.CustomCollections;

namespace mRemoteNG.UI.Controls
{
    public partial class CredentialRepositoryListView : UserControl
    {
        private ICredentialRepositoryList _credentialRepositoryList = new CredentialRepositoryList();

        public ICredentialRepositoryList CredentialRepositoryList
        {
            get { return _credentialRepositoryList; }
            set
            {
                _credentialRepositoryList.RepositoriesUpdated -= OnRepositoriesUpdated;
                _credentialRepositoryList = value;
                SetListObjects(CredentialRepositoryList.CredentialProviders);
                objectListView1.AutoResizeColumns();
                _credentialRepositoryList.RepositoriesUpdated += OnRepositoriesUpdated;
            }
        }

        public Func<ICredentialRepository, bool> RepositoryFilter { get; set; }
        public ICredentialRepository SelectedRepository => GetSelectedRepository();
        public Func<ICredentialRepository,bool> DoubleClickHandler { get; set; }

        public CredentialRepositoryListView()
        {
            InitializeComponent();
            SetupObjectListView();
        }

        public void RefreshObjects()
        {
            var repos = CredentialRepositoryList.CredentialProviders.ToList();
            objectListView1.RefreshObjects(repos);
        }

        private void SetupObjectListView()
        {
            olvColumnTitle.AspectGetter = rowObject => ((ICredentialRepository) rowObject).Config.Title;
            olvColumnProvider.AspectGetter = rowObject => ((ICredentialRepository) rowObject).Config.TypeName;
            olvColumnSource.AspectGetter = rowObject => ((ICredentialRepository) rowObject).Config.Source;
            olvColumnId.AspectGetter = rowObject => ((ICredentialRepository) rowObject).Config.Id;
            olvColumnIsLoaded.AspectGetter = rowObject => ((ICredentialRepository) rowObject).IsLoaded;
            SetListObjects(CredentialRepositoryList.CredentialProviders);
            objectListView1.SelectionChanged += (sender, args) => RaiseSelectionChangedEvent();
            objectListView1.MouseDoubleClick += ObjectListView1OnMouseDoubleClick;
        }

        private void OnRepositoriesUpdated(object sender, CollectionUpdatedEventArgs<ICredentialRepository> args)
        {
            SetListObjects(CredentialRepositoryList.CredentialProviders);
        }

        private void SetListObjects(IEnumerable<ICredentialRepository> repositories)
        {
            var filteredRepositories = RepositoryFilter == null ?
                repositories :
                repositories.Where(RepositoryFilter);
            objectListView1.SetObjects(filteredRepositories);
        }

        private void ObjectListView1OnMouseDoubleClick(object sender, MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.Clicks < 2) return;
            OLVColumn column;
            var listItem = objectListView1.GetItemAt(mouseEventArgs.X, mouseEventArgs.Y, out column);
            var clickedNode = listItem.RowObject as ICredentialRepository;
            if (clickedNode == null) return;
            DoubleClickHandler?.Invoke(clickedNode);
        }

        private ICredentialRepository GetSelectedRepository()
        {
            return objectListView1.SelectedObject as ICredentialRepository;
        }

        public event EventHandler SelectionChanged;
        private void RaiseSelectionChangedEvent()
        {
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _credentialRepositoryList.RepositoriesUpdated -= OnRepositoriesUpdated;
                components?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}