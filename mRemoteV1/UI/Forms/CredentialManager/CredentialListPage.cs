using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using mRemoteNG.App;
using mRemoteNG.Credential;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.UI.Controls.PageSequence;

namespace mRemoteNG.UI.Forms.CredentialManager
{
    public sealed partial class CredentialListPage : SequencedControl, ICredentialManagerPage
    {
        public string PageName { get; } = Language.strCategoryCredentials;
        public Image PageIcon { get; } = Resources.key;
        public IConfirm<IEnumerable<ICredentialRecord>> DeletionConfirmer { get; set; } = new AlwaysConfirmYes();

        public CredentialListPage(ICredentialRepositoryList credentialRepositoryList)
        {
            InitializeComponent();
            ApplyTheme();
            ApplyLanguage();
            credentialRecordListView.CellClick += HandleCellDoubleClick;
            credentialRecordListView.SelectionChanged += ObjectListView1OnSelectionChanged;
            credentialRecordListView.KeyDown += ObjectListView1OnEnterPressed;
            credentialRecordListView.KeyDown += OnAPressed;
            credentialRecordListView.KeyDown += OnDeletePressed;
            credentialRecordListView.CredentialRepositoryList = credentialRepositoryList.ThrowIfNull(nameof(credentialRepositoryList));
        }

       

        private void ApplyLanguage()
        {
            Text = Language.strCredentialManager;
            msAdd.Text = Language.strAddRecord;
            msRemove.Text = Language.strRemoveRecord;
            msEdit.Text = Language.strEditRecord;
        }

        private void RemoveSelectedCredential()
        {
            var selectedCredential = credentialRecordListView.SelectedObject;
            if (!DeletionConfirmer.Confirm(new[] { selectedCredential.Key }))
                return;

            selectedCredential.Value.CredentialRecords.Remove(selectedCredential.Key);
            RaiseCredentialsChangedEvent(this);
        }

        private void RemoveSelectedCredentials()
        {
            var selectedCredentials = credentialRecordListView.SelectedObjects.ToArray();
            if (!DeletionConfirmer.Confirm(selectedCredentials.Select(i => i.Key)))
                return;

            foreach(var item in selectedCredentials)
                item.Value.CredentialRecords.Remove(item.Key);
            RaiseCredentialsChangedEvent(this);
        }

        private void HandleCellDoubleClick(object sender, CellClickEventArgs cellClickEventArgs)
        {
            if (cellClickEventArgs.ClickCount < 2)
                return;

            EditCredential(credentialRecordListView.SelectedObject);
        }

        private void AddCredential()
        {
            var sequence = new PageSequence(Parent,
                this,
                new CredentialEditorPage(new CredentialRecord(), Optional<ICredentialRepository>.Empty, Runtime.CredentialService),
                this
            );
            RaiseNextPageEvent();
        }

        private void EditCredential(KeyValuePair<ICredentialRecord, ICredentialRepository> credentialAndRepository)
        {
            if (credentialAndRepository.Key == null || credentialAndRepository.Value == null)
                return;

            var sequence = new PageSequence(Parent,
                this,
                new CredentialEditorPage(credentialAndRepository.Key, credentialAndRepository.Value.ToOptional(), Runtime.CredentialService),
                this
            );
            RaiseNextPageEvent();
        }

        private void msAdd_Click(object sender, EventArgs e)
        {
            AddCredential();
        }

        private void msEdit_Click(object sender, EventArgs e)
        {
            EditCredential(credentialRecordListView.SelectedObject);
        }

        private void msRemove_Click(object sender, EventArgs e)
        {
            if (credentialRecordListView.MultipleObjectsSelected)
                RemoveSelectedCredentials();
            else
                RemoveSelectedCredential();
        }

        private void ObjectListView1OnEnterPressed(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.KeyCode != Keys.Enter)
                return;

            EditCredential(credentialRecordListView.SelectedObject);
            keyEventArgs.Handled = true;
            keyEventArgs.SuppressKeyPress = true;
        }

        private void OnAPressed(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.KeyCode != Keys.A)
                return;

            AddCredential();
            keyEventArgs.Handled = true;
            keyEventArgs.SuppressKeyPress = true;
        }

        private void OnDeletePressed(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.KeyCode != Keys.Delete)
                return;

            if (credentialRecordListView.MultipleObjectsSelected)
                RemoveSelectedCredentials();
            else
                RemoveSelectedCredential();
            keyEventArgs.Handled = true;
            keyEventArgs.SuppressKeyPress = true;
        }

        private void ObjectListView1OnSelectionChanged(object sender, EventArgs eventArgs)
        {
            msRemove.Enabled = credentialRecordListView.SelectedObjects.Any();
        }

        public event EventHandler CredentialsChanged;
        private void RaiseCredentialsChangedEvent(object sender)
        {
            CredentialsChanged?.Invoke(sender, EventArgs.Empty);
        }
    }
}