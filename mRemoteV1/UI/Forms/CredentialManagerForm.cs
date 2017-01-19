using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using mRemoteNG.Credential;


namespace mRemoteNG.UI.Forms
{
    public partial class CredentialManagerForm : Form, INotifyCollectionChanged
    {
        public CredentialManagerForm(IEnumerable<ICredentialRecord> credentialRecords)
        {
            InitializeComponent();
            ApplyLanguage();
            ApplyThemes();
            objectListView1.AddObjects(credentialRecords.ToList());
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
            credentialEditorForm.Show(this);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var newCredential = new CredentialRecord();
            objectListView1.AddObject(newCredential);
            RaiseCollectionChangedEvent(NotifyCollectionChangedAction.Add, newCredential);
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            var selectedCredential = objectListView1.SelectedObject as ICredentialRecord;
            if (selectedCredential == null) return;
            objectListView1.RemoveObject(selectedCredential);
            RaiseCollectionChangedEvent(NotifyCollectionChangedAction.Remove, selectedCredential);
        }

        private void ObjectListView1OnSelectionChanged(object sender, EventArgs eventArgs)
        {
            buttonRemove.Enabled = objectListView1.SelectedObjects.Count != 0;
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void RaiseCollectionChangedEvent(NotifyCollectionChangedAction action, ICredentialRecord changedItem)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, new[] {changedItem}));
        }
    }
}