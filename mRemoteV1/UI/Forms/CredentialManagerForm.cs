using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using mRemoteNG.Credential;


namespace mRemoteNG.UI.Forms
{
    public partial class CredentialManagerForm : Form
    {
        public CredentialManagerForm(IEnumerable<ICredentialRecord> credentialRecords)
        {
            InitializeComponent();
            ApplyLanguage();
            ApplyThemes();
            objectListView1.AddObjects(credentialRecords.ToList());
            objectListView1.CellClick += HandleCellDoubleClick;
        }

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
            var credentialEditorForm = new CredentialEditorForm(newCredential);
            credentialEditorForm.Show(this);
        }
    }
}