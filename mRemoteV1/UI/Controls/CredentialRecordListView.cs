using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using BrightIdeasSoftware;
using mRemoteNG.Credential;
using mRemoteNG.UI.Controls.PageSequence;

namespace mRemoteNG.UI.Controls
{
    public partial class CredentialRecordListView : SequencedControl
    {
        private ICredentialRepositoryList _credentialRepositoryList = new CredentialRepositoryList();

        public ICredentialRepositoryList CredentialRepositoryList
        {
            get { return _credentialRepositoryList; }
            set
            {
                _credentialRepositoryList.CollectionChanged -= CredentialRepositoryListOnCollectionChanged;
                _credentialRepositoryList = value;
                _credentialRepositoryList.CollectionChanged += CredentialRepositoryListOnCollectionChanged;
                SetObjectList();
                objectListView1.AutoResizeColumns();
            }
        }

        public KeyValuePair<ICredentialRecord, ICredentialRepository> SelectedObject => CastRowObject(objectListView1.SelectedObject);

        public CredentialRecordListView()
        {
            InitializeComponent();
            olvColumnCredentialId.AspectGetter = CredentialIdAspectGetter;
            olvColumnTitle.AspectGetter = CredentialTitleAspectGetter;
            olvColumnUsername.AspectGetter = CredentialUsernameAspectGetter;
            olvColumnDomain.AspectGetter = CredentialDomainAspectGetter;
            olvColumnRepositorySource.AspectGetter = CredentialSourceAspectGetter;
            olvColumnRepositoryTitle.AspectGetter = RepoTitleAspectGetter;
            objectListView1.SelectionChanged += (sender, args) => RaiseSelectionChangedEvent();
            objectListView1.CellClick += RaiseCellClickEvent;
            ApplyLanguage();
        }

        private void SetObjectList()
        {
            var objects = new Dictionary<ICredentialRecord, ICredentialRepository>();
            foreach (var repository in _credentialRepositoryList.CredentialProviders)
            {
                foreach (var credential in repository.CredentialRecords)
                    objects.Add(credential, repository);
            }

            objectListView1.SetObjects(objects, true);
        }

        private void ApplyLanguage()
        {
            olvColumnTitle.Text = Language.strTitle;
            olvColumnUsername.Text = Language.strPropertyNameUsername;
            olvColumnDomain.Text = Language.strPropertyNameDomain;
        }

        private object CredentialIdAspectGetter(object rowObject)
        {
            var keyValuePair = CastRowObject(rowObject);
            return keyValuePair.Key.Id;
        }

        private object CredentialTitleAspectGetter(object rowObject)
        {
            var keyValuePair = CastRowObject(rowObject);
            return keyValuePair.Key.Title;
        }

        private object CredentialUsernameAspectGetter(object rowObject)
        {
            var keyValuePair = CastRowObject(rowObject);
            return keyValuePair.Key.Username;
        }

        private object CredentialDomainAspectGetter(object rowObject)
        {
            var keyValuePair = CastRowObject(rowObject);
            return keyValuePair.Key.Domain;
        }

        private object CredentialSourceAspectGetter(object rowObject)
        {
            var keyValuePair = CastRowObject(rowObject);
            return keyValuePair.Value.Config.Source;
        }

        private object RepoTitleAspectGetter(object rowObject)
        {
            var keyValuePair = CastRowObject(rowObject);
            return keyValuePair.Value.Config.Title;
        }

        private KeyValuePair<ICredentialRecord, ICredentialRepository> CastRowObject(object model)
        {
            if (!(model is KeyValuePair<ICredentialRecord, ICredentialRepository>)) return default(KeyValuePair<ICredentialRecord, ICredentialRepository>);
            var keyValuePair = (KeyValuePair<ICredentialRecord, ICredentialRepository>)model;
            return keyValuePair;
        }

        private void CredentialRepositoryListOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            SetObjectList();
        }

        public event EventHandler SelectionChanged;
        private void RaiseSelectionChangedEvent()
        {
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler<CellClickEventArgs> CellClick;
        private void RaiseCellClickEvent(object sender, CellClickEventArgs args)
        {
            CellClick?.Invoke(sender, args);
        }
    }
}