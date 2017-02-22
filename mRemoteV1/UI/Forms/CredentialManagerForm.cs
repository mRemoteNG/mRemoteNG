using System;
using System.Windows.Forms;
using BrightIdeasSoftware;
using mRemoteNG.Credential;
using mRemoteNG.Tree;


namespace mRemoteNG.UI.Forms
{
    public partial class CredentialManagerForm : Form
    {
        private readonly CredentialManager _credentialManager;

        public ICredentialRecord SelectedRecord => objectListView1.SelectedObject as ICredentialRecord;
        public IConfirm<ICredentialRecord> DeletionConfirmer { get; set; } = new AlwaysConfirmYes();

        public CredentialManagerForm(CredentialManager credentialManager)
        {
            if (credentialManager == null)
                throw new ArgumentNullException(nameof(credentialManager));

            _credentialManager = credentialManager;
            InitializeComponent();
            ApplyLanguage();
            ApplyThemes();
            objectListView1.SetObjects(_credentialManager.GetCredentialRecords());
            CredentialsChanged += (sender, args) => objectListView1.SetObjects(_credentialManager.GetCredentialRecords(), true);
            objectListView1.CellClick += HandleCellDoubleClick;
            objectListView1.SelectionChanged += ObjectListView1OnSelectionChanged;
            objectListView1.KeyDown += ObjectListView1OnEnterPressed;
            objectListView1.KeyDown += OnAPressed;
            objectListView1.KeyDown += OnDeletePressed;
        }

        public CredentialManagerForm(CredentialManager credentialManager, ICredentialRecord selectedRecord) : this(credentialManager)
        {
            if (selectedRecord != null)
                objectListView1.SelectObject(selectedRecord);
        }

        

        #region Form stuff
        private void ApplyLanguage()
        {
            Text = Language.strCredentialManager;
            olvColumnTitle.Text = Language.strTitle;
            olvColumnUsername.Text = Language.strPropertyNameUsername;
            olvColumnDomain.Text = Language.strPropertyNameDomain;
            buttonAdd.Text = Language.strAdd;
            buttonRemove.Text = Language.strRemove;
            buttonClose.Text = Language.strButtonClose;
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
            _credentialManager.Add(new CredentialRecord());
            RaiseCredentialsChangedEvent(this);
        }

        private void RemoveSelectedCredential()
        {
            var selectedCredential = objectListView1.SelectedObject as CredentialRecord;
            if (selectedCredential == null) return;
            if (!DeletionConfirmer.Confirm(selectedCredential)) return;
            _credentialManager.Remove(selectedCredential);
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