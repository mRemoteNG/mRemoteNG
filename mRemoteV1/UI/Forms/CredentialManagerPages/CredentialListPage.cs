using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BrightIdeasSoftware;
using mRemoteNG.Credential;
using mRemoteNG.Tree;
using mRemoteNG.UI.Controls.PageSequence;

namespace mRemoteNG.UI.Forms.CredentialManagerPages
{
    public partial class CredentialListPage : SequencedControl, ICredentialManagerPage
    {
        private readonly ICredentialRepositoryList _credentialRepositoryList;

        public string PageName { get; } = Language.strCategoryCredentials;
        public Image PageIcon { get; } = Resources.key;
        public IConfirm<ICredentialRecord> DeletionConfirmer { get; set; } = new AlwaysConfirmYes();

        public CredentialListPage(ICredentialRepositoryList credentialRepositoryList)
        {
            if (credentialRepositoryList == null)
                throw new ArgumentNullException(nameof(credentialRepositoryList));

            _credentialRepositoryList = credentialRepositoryList;
            InitializeComponent();
            ApplyLanguage();
            CredentialsChanged += (sender, args) => SetObjectList();
            objectListView1.CellClick += HandleCellDoubleClick;
            objectListView1.SelectionChanged += ObjectListView1OnSelectionChanged;
            objectListView1.KeyDown += ObjectListView1OnEnterPressed;
            objectListView1.KeyDown += OnAPressed;
            objectListView1.KeyDown += OnDeletePressed;
            olvColumnCredentialId.AspectGetter = CredentialIdAspectGetter;
            olvColumnTitle.AspectGetter = CredentialTitleAspectGetter;
            olvColumnUsername.AspectGetter = CredentialUsernameAspectGetter;
            olvColumnDomain.AspectGetter = CredentialDomainAspectGetter;
            olvColumnSource.AspectGetter = CredentialSourceAspectGetter;
            SetObjectList();
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

        private void SetObjectList()
        {
            var objects = new Dictionary<ICredentialRecord, ICredentialRepository>();
            foreach (var repository in _credentialRepositoryList.CredentialProviders)
            {
                foreach(var credential in repository.CredentialRecords)
                    objects.Add(credential, repository);
            }

            objectListView1.SetObjects(objects, true);
        }

        private void ApplyLanguage()
        {
            Text = Language.strCredentialManager;
            olvColumnTitle.Text = Language.strTitle;
            olvColumnUsername.Text = Language.strPropertyNameUsername;
            olvColumnDomain.Text = Language.strPropertyNameDomain;
            buttonAdd.Text = Language.strAdd;
            buttonRemove.Text = Language.strRemove;
        }

        private void RemoveSelectedCredential()
        {
            if (objectListView1.SelectedObject == null) return;
            var selectedCredential = CastRowObject(objectListView1.SelectedObject);
            if (!DeletionConfirmer.Confirm(selectedCredential.Key)) return;
            selectedCredential.Value.CredentialRecords.Remove(selectedCredential.Key);
            RaiseCredentialsChangedEvent(this);
        }

        private void HandleCellDoubleClick(object sender, CellClickEventArgs cellClickEventArgs)
        {
            if (cellClickEventArgs.ClickCount < 2) return;
            var clickedCredential = CastRowObject(cellClickEventArgs.Model);
            EditCredential(clickedCredential.Key);
        }

        private void AddCredential()
        {
            var sequence = new PageSequence(Parent,
                this,
                
                new CredentialEditorPage(new CredentialRecord()),
                this
            );
            RaiseNextPageEvent();
        }

        private void EditCredential(ICredentialRecord credentialRecord)
        {
            if (credentialRecord == null) return;
            var sequence = new PageSequence(Parent,
                this,
                new CredentialEditorPage(credentialRecord),
                this
            );
            RaiseNextPageEvent();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddCredential();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            var selectedCredential = CastRowObject(objectListView1.SelectedObject);
            EditCredential(selectedCredential.Key);
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            RemoveSelectedCredential();
        }

        private KeyValuePair<ICredentialRecord, ICredentialRepository> CastRowObject(object model)
        {
            if (!(model is KeyValuePair<ICredentialRecord, ICredentialRepository>)) return default(KeyValuePair<ICredentialRecord, ICredentialRepository>);
            var keyValuePair = (KeyValuePair<ICredentialRecord, ICredentialRepository>)model;
            return keyValuePair;
        }

        private void ObjectListView1OnEnterPressed(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.KeyCode != Keys.Enter) return;
            var selectedCredential = objectListView1.SelectedObject as ICredentialRecord;
            if (selectedCredential == null) return;
            EditCredential(selectedCredential);
            keyEventArgs.Handled = true;
            keyEventArgs.SuppressKeyPress = true;
        }

        private void OnAPressed(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.KeyCode != Keys.A) return;
            AddCredential();
            keyEventArgs.Handled = true;
            keyEventArgs.SuppressKeyPress = true;
        }

        private void OnDeletePressed(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.KeyCode != Keys.Delete) return;
            RemoveSelectedCredential();
            keyEventArgs.Handled = true;
            keyEventArgs.SuppressKeyPress = true;
        }

        private void ObjectListView1OnSelectionChanged(object sender, EventArgs eventArgs)
        {
            buttonRemove.Enabled = objectListView1.SelectedObjects.Count != 0;
        }

        public event EventHandler CredentialsChanged;
        private void RaiseCredentialsChangedEvent(object sender)
        {
            CredentialsChanged?.Invoke(sender, EventArgs.Empty);
        }
    }
}