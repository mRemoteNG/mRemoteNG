using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        public IConfirm<IEnumerable<ICredentialRecord>> DeletionConfirmer { get; set; } = new AlwaysConfirmYes();

        public CredentialListPage(ICredentialRepositoryList credentialRepositoryList)
        {
            if (credentialRepositoryList == null)
                throw new ArgumentNullException(nameof(credentialRepositoryList));

            _credentialRepositoryList = credentialRepositoryList;
            InitializeComponent();
            ApplyLanguage();
            credentialRecordListView.CellClick += HandleCellDoubleClick;
            credentialRecordListView.SelectionChanged += ObjectListView1OnSelectionChanged;
            credentialRecordListView.KeyDown += ObjectListView1OnEnterPressed;
            credentialRecordListView.KeyDown += OnAPressed;
            credentialRecordListView.KeyDown += OnDeletePressed;
            credentialRecordListView.CredentialRepositoryList = _credentialRepositoryList;
        }

        private void ApplyLanguage()
        {
            Text = Language.strCredentialManager;
            buttonAdd.Text = Language.strAdd;
            buttonRemove.Text = Language.strRemove;
        }

        private void RemoveSelectedCredential()
        {
            var selectedCredential = credentialRecordListView.SelectedObject;
            if (!DeletionConfirmer.Confirm(new[] { selectedCredential.Key })) return;
            selectedCredential.Value.CredentialRecords.Remove(selectedCredential.Key);
            RaiseCredentialsChangedEvent(this);
        }

        private void RemoveSelectedCredentials()
        {
            var selectedCredentials = credentialRecordListView.SelectedObjects.ToArray();
            if (!DeletionConfirmer.Confirm(selectedCredentials.Select(i => i.Key))) return;
            foreach(var item in selectedCredentials)
                item.Value.CredentialRecords.Remove(item.Key);
            RaiseCredentialsChangedEvent(this);
        }

        private void HandleCellDoubleClick(object sender, CellClickEventArgs cellClickEventArgs)
        {
            if (cellClickEventArgs.ClickCount < 2) return;
            EditCredential(credentialRecordListView.SelectedObject);
        }

        private void AddCredential()
        {
            var sequence = new PageSequence(Parent,
                this,
                new CredentialRepositorySelectionPage(_credentialRepositoryList),
                new SequencedControl(),
                this
            );
            RaiseNextPageEvent();
        }

        private void EditCredential(KeyValuePair<ICredentialRecord, ICredentialRepository> credentialAndRepository)
        {
            if (credentialAndRepository.Key == null || credentialAndRepository.Value == null) return;
            var sequence = new PageSequence(Parent,
                this,
                new CredentialEditorPage(credentialAndRepository.Key, credentialAndRepository.Value),
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
            EditCredential(credentialRecordListView.SelectedObject);
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (credentialRecordListView.MultipleObjectsSelected)
                RemoveSelectedCredentials();
            else
                RemoveSelectedCredential();
        }

        private void ObjectListView1OnEnterPressed(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.KeyCode != Keys.Enter) return;
            EditCredential(credentialRecordListView.SelectedObject);
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
            if (credentialRecordListView.MultipleObjectsSelected)
                RemoveSelectedCredentials();
            else
                RemoveSelectedCredential();
            keyEventArgs.Handled = true;
            keyEventArgs.SuppressKeyPress = true;
        }

        private void ObjectListView1OnSelectionChanged(object sender, EventArgs eventArgs)
        {
            buttonRemove.Enabled = credentialRecordListView.SelectedObjects.Any();
        }

        public event EventHandler CredentialsChanged;
        private void RaiseCredentialsChangedEvent(object sender)
        {
            CredentialsChanged?.Invoke(sender, EventArgs.Empty);
        }
    }
}