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
            CredentialsChanged += (sender, args) => objectListView1.SetObjects(_credentialRecords);
            objectListView1.CellClick += HandleCellDoubleClick;
            objectListView1.SelectionChanged += ObjectListView1OnSelectionChanged;
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

        private void HandleCellDoubleClick(object sender, CellClickEventArgs cellClickEventArgs)
        {
            if (cellClickEventArgs.ClickCount < 2) return;
            var clickedCredential = cellClickEventArgs.Model as ICredentialRecord;
            if (clickedCredential == null) return;
            var credentialEditorForm = new CredentialEditorForm(clickedCredential);
            credentialEditorForm.ChangesAccepted += (o, args) => RaiseCredentialsChangedEvent(o);
            credentialEditorForm.CenterOnTarget(this);
            credentialEditorForm.Show(this);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var newCredential = new CredentialRecord();
            _credentialRecords.Add(newCredential);
            RaiseCredentialsChangedEvent(this);
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            var selectedCredential = objectListView1.SelectedObject as CredentialRecord;
            if (selectedCredential == null) return;
            _credentialRecords.Remove(selectedCredential);
            RaiseCredentialsChangedEvent(this);
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