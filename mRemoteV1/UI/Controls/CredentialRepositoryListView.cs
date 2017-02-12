using System;
using System.Collections.Specialized;
using System.Windows.Forms;
using BrightIdeasSoftware;
using mRemoteNG.Credential;

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
                _credentialRepositoryList.CollectionChanged -= UpdateList;
                _credentialRepositoryList = value;
                objectListView1.SetObjects(CredentialRepositoryList.CredentialProviders);
                objectListView1.AutoResizeColumns();
                _credentialRepositoryList.CollectionChanged += UpdateList;
            }
        }

        public ICredentialRepository SelectedRepository => GetSelectedRepository();
        public Func<ICredentialRepository,bool> DoubleClickHandler { get; set; }

        public CredentialRepositoryListView()
        {
            InitializeComponent();
            SetupObjectListView();
        }

        private void SetupObjectListView()
        {
            olvColumnTitle.AspectGetter = rowObject => ((ICredentialRepository) rowObject).Config.Title;
            olvColumnProvider.AspectGetter = rowObject => ((ICredentialRepository) rowObject).Config.TypeName;
            olvColumnSource.AspectGetter = rowObject => ((ICredentialRepository) rowObject).Config.Source;
            olvColumnId.AspectGetter = rowObject => ((ICredentialRepository) rowObject).Config.Id;
            objectListView1.SetObjects(CredentialRepositoryList.CredentialProviders);
            objectListView1.SelectionChanged += (sender, args) => RaiseSelectionChangedEvent();
            objectListView1.MouseDoubleClick += ObjectListView1OnMouseDoubleClick;
        }

        private void UpdateList(object sender, NotifyCollectionChangedEventArgs args)
        {
            objectListView1.SetObjects(CredentialRepositoryList.CredentialProviders);
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
    }
}