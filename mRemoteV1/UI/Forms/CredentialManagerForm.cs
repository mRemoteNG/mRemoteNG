using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BrightIdeasSoftware;
using mRemoteNG.Credential;


namespace mRemoteNG.UI.Forms
{
    public partial class CredentialManagerForm : Form
    {
        private readonly IList<ICredentialRecord> _credentialRecords;

        public CredentialManagerForm(IList<ICredentialRecord> credentialRecords)
        {
            if (credentialRecords == null)
                throw new ArgumentNullException(nameof(credentialRecords));

            _credentialRecords = credentialRecords;
            InitializeComponent();
            ApplyLanguage();
            ApplyThemes();
            objectListView1.SetObjects(_credentialRecords);
            CredentialsChanged += (sender, args) => objectListView1.SetObjects(_credentialRecords, true);
            objectListView1.CellClick += HandleCellDoubleClick;
            objectListView1.SelectionChanged += ObjectListView1OnSelectionChanged;
            objectListView1.KeyDown += ObjectListView1OnEnterPressed;
            objectListView1.KeyDown += OnAPressed;
        }

        

        #region Form stuff

        private void ApplyLanguage()
        {
            Text = "Credential Manager";
            olvColumnTitle.Text = "Title";
            olvColumnUsername.Text = Language.strPropertyNameUsername;
            olvColumnDomain.Text = Language.strPropertyNameDomain;
        }

        private void ApplyThemes()
        {
        }

        #endregion

        private void EditCredential(ICredentialRecord credentialRecord)
        {
            if (credentialRecord == null) return;
            var credentialEditorForm = new CredentialEditorForm(credentialRecord);
            credentialEditorForm.ChangesAccepted += (o, args) => RaiseCredentialsChangedEvent(o);
            credentialEditorForm.CenterOnTarget(this);
            credentialEditorForm.Show(this);
        }

        private void AddCredential()
        {
            var newCredential = new CredentialRecord();
            _credentialRecords.Add(newCredential);
            RaiseCredentialsChangedEvent(this);
        }

        private void RemoveSelectedCredential()
        {
            var selectedCredential = objectListView1.SelectedObject as CredentialRecord;
            if (selectedCredential == null) return;
            _credentialRecords.Remove(selectedCredential);
            RaiseCredentialsChangedEvent(this);
        }

        private void HandleCellDoubleClick(object sender, CellClickEventArgs cellClickEventArgs)
        {
            if (cellClickEventArgs.ClickCount < 2) return;
            var clickedCredential = cellClickEventArgs.Model as ICredentialRecord;
            EditCredential(clickedCredential);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddCredential();
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            RemoveSelectedCredential();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ObjectListView1OnEnterPressed(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.KeyCode != Keys.Enter) return;
            var selectedCredential = objectListView1.SelectedObject as ICredentialRecord;
            if (selectedCredential == null) return;
            EditCredential(selectedCredential);
            keyEventArgs.Handled = true;
        }

        private void OnAPressed(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.KeyCode != Keys.A) return;
            AddCredential();
            keyEventArgs.Handled = true;
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