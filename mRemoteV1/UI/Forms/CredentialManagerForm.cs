using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using mRemoteNG.Credential;
using mRemoteNG.Tools;


namespace mRemoteNG.UI.Forms
{
    public partial class CredentialManagerForm : Form
    {
        private readonly ObservablePropertyCollection<CredentialRecord> _credentialRecords;

        public CredentialManagerForm(ObservablePropertyCollection<CredentialRecord> credentialRecords)
        {
            if (credentialRecords == null)
                throw new ArgumentNullException(nameof(credentialRecords));

            _credentialRecords = credentialRecords;
            InitializeComponent();
            ApplyLanguage();
            ApplyThemes();
            objectListView1.SetObjects(_credentialRecords.ToList());
            objectListView1.CellClick += HandleCellDoubleClick;
            objectListView1.SelectionChanged += ObjectListView1OnSelectionChanged;
            _credentialRecords.CollectionChanged += CredentialRecordsOnCollectionChanged;
        }

        private void CredentialRecordsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            objectListView1.SetObjects(_credentialRecords);
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
            credentialEditorForm.Show(this);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var newCredential = new CredentialRecord();
            _credentialRecords.Add(newCredential);
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            var selectedCredential = objectListView1.SelectedObject as CredentialRecord;
            if (selectedCredential == null) return;
            _credentialRecords.Remove(selectedCredential);
        }

        private void ObjectListView1OnSelectionChanged(object sender, EventArgs eventArgs)
        {
            buttonRemove.Enabled = objectListView1.SelectedObjects.Count != 0;
        }
    }
}