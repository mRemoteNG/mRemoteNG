using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Security;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Credential;
using mRemoteNG.Resources.Language;
using mRemoteNG.Security;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Forms
{
    [SupportedOSPlatform("windows")]
    public partial class CredentialSetsManager
    {
        private ThemeManager? _themeManager;
        private ICredentialRepositoryList? _credentialRepositoryList;
        private ICredentialRecord? _editingCredential;
        private bool _isAddingNew;

        public CredentialSetsManager()
        {
            InitializeComponent();
            Icon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.Key_16x);
            FontOverrider.FontOverride(this);
        }

        private void CredentialSetsManager_Load(object sender, EventArgs e)
        {
            _credentialRepositoryList = Runtime.CredentialProviderCatalog;
            ApplyTheme();
            ThemeManager.getInstance().ThemeChanged += ApplyTheme;
            ApplyLanguage();
            LoadCredentials();
            UpdateButtonStates();
        }

        private void ApplyTheme()
        {
            _themeManager = ThemeManager.getInstance();
            if (!_themeManager.ActiveAndExtended) return;
            BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
        }

        private void ApplyLanguage()
        {
            Text = Language.CredentialSetsManager;
            olvColumnTitle.Text = Language.Title;
            olvColumnUsername.Text = Language.Username;
            olvColumnDomain.Text = Language.Domain;
            btnAdd.Text = Language.Add;
            btnEdit.Text = Language.Edit;
            btnDelete.Text = Language._Delete;
            btnClose.Text = Language._Close;
            grpCredentialDetails.Text = Language.CredentialDetails;
            lblTitle.Text = Language.Title + ":";
            lblUsername.Text = Language.Username + ":";
            lblPassword.Text = Language.Password + ":";
            lblDomain.Text = Language.Domain + ":";
            btnSave.Text = Language.Save;
            btnCancelEdit.Text = Language._Cancel;
        }

        private void LoadCredentials()
        {
            if (_credentialRepositoryList == null) return;

            List<ICredentialRecord> credentials = _credentialRepositoryList.GetCredentialRecords().ToList();
            listViewCredentials.SetObjects(credentials);
            listViewCredentials.AutoResizeColumns();
        }

        private void UpdateButtonStates()
        {
            bool hasSelection = listViewCredentials.SelectedObject != null;
            btnEdit.Enabled = hasSelection && !grpCredentialDetails.Enabled;
            btnDelete.Enabled = hasSelection && !grpCredentialDetails.Enabled;
            btnAdd.Enabled = !grpCredentialDetails.Enabled;
            listViewCredentials.Enabled = !grpCredentialDetails.Enabled;
        }

        private void listViewCredentials_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtonStates();
        }

        private void listViewCredentials_DoubleClick(object sender, EventArgs e)
        {
            if (listViewCredentials.SelectedObject != null)
            {
                EditSelectedCredential();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _isAddingNew = true;
            _editingCredential = null;
            ClearEditFields();
            EnableEditMode(true);
            txtTitle.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            EditSelectedCredential();
        }

        private void EditSelectedCredential()
        {
            ICredentialRecord? selectedCredential = listViewCredentials.SelectedObject as ICredentialRecord;
            if (selectedCredential == null) return;

            _isAddingNew = false;
            _editingCredential = selectedCredential;
            PopulateEditFields(selectedCredential);
            EnableEditMode(true);
            txtTitle.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            ICredentialRecord? selectedCredential = listViewCredentials.SelectedObject as ICredentialRecord;
            if (selectedCredential == null) return;

            // Check if credential is referenced by any connections
            int referenceCount = GetCredentialReferenceCount(selectedCredential);
            
            string message = referenceCount > 0
                ? string.Format(Language.CredentialDeleteWarningWithReferences, selectedCredential.Title, referenceCount)
                : string.Format(Language.CredentialDeleteConfirmation, selectedCredential.Title);

            DialogResult result = MessageBox.Show(
                message,
                Language.Confirm,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            // Find the repository containing this credential and remove it
            ICredentialRepository? repo = FindRepositoryForCredential(selectedCredential);
            if (repo != null)
            {
                repo.CredentialRecords.Remove(selectedCredential);
                // Explicitly save the repository to persist the deletion
                PersistRepository(repo);
                LoadCredentials();
            }
        }

        private int GetCredentialReferenceCount(ICredentialRecord credential)
        {
            // For now, return 0. A full implementation would scan all connections
            // to find ones referencing this credential ID.
            // This is a placeholder for future implementation when connections
            // can reference credential sets by ID.
            return 0;
        }

        private ICredentialRepository? FindRepositoryForCredential(ICredentialRecord credential)
        {
            if (_credentialRepositoryList == null) return null;

            foreach (ICredentialRepository repo in _credentialRepositoryList.CredentialProviders)
            {
                if (repo.CredentialRecords.Contains(credential))
                {
                    return repo;
                }
            }
            return null;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show(
                    Language.CredentialTitleRequired,
                    Language.Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtTitle.Focus();
                return;
            }

            if (_isAddingNew)
            {
                SaveNewCredential();
            }
            else
            {
                SaveEditedCredential();
            }

            EnableEditMode(false);
            LoadCredentials();
            UpdateButtonStates();
        }

        private void SaveNewCredential()
        {
            CredentialRecord newCredential = new()
            {
                Title = txtTitle.Text.Trim(),
                Username = txtUsername.Text.Trim(),
                Password = txtPassword.Text.ConvertToSecureString(),
                Domain = txtDomain.Text.Trim()
            };

            // Add to the first available repository, or create one if needed
            ICredentialRepository? repo = GetOrCreateDefaultRepository();
            if (repo == null)
            {
                MessageBox.Show(
                    "No credential repository is available. Please ensure the application is properly initialized.",
                    Language.Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            repo.CredentialRecords.Add(newCredential);
            // Explicitly save the repository to persist the new credential
            PersistRepository(repo);
        }

        private void SaveEditedCredential()
        {
            if (_editingCredential == null) return;

            _editingCredential.Title = txtTitle.Text.Trim();
            _editingCredential.Username = txtUsername.Text.Trim();
            
            // Only update password if something was entered
            if (!string.IsNullOrEmpty(txtPassword.Text))
            {
                _editingCredential.Password = txtPassword.Text.ConvertToSecureString();
            }
            
            _editingCredential.Domain = txtDomain.Text.Trim();
            
            // Explicitly save the repository to persist the edited credential
            ICredentialRepository? repo = FindRepositoryForCredential(_editingCredential);
            if (repo != null)
            {
                PersistRepository(repo);
            }
        }

        private ICredentialRepository? GetDefaultRepository()
        {
            if (_credentialRepositoryList == null) return null;
            return _credentialRepositoryList.CredentialProviders.FirstOrDefault(r => r.IsLoaded);
        }

        /// <summary>
        /// Gets the default loaded repository, or creates and loads one if none exists.
        /// </summary>
        private ICredentialRepository? GetOrCreateDefaultRepository()
        {
            if (_credentialRepositoryList == null) return null;

            // First try to get an already loaded repository
            ICredentialRepository? repo = _credentialRepositoryList.CredentialProviders.FirstOrDefault(r => r.IsLoaded);
            if (repo != null) return repo;

            // No loaded repository - ensure one is created and loaded
            Runtime.EnsureDefaultCredentialRepository();
            
            // Try again to get the loaded repository
            repo = _credentialRepositoryList.CredentialProviders.FirstOrDefault(r => r.IsLoaded);
            if (repo != null) return repo;

            // Try to load an existing unloaded repository manually
            repo = _credentialRepositoryList.CredentialProviders.FirstOrDefault();
            if (repo != null)
            {
                try
                {
                    repo.LoadCredentials(Runtime.EncryptionKey);
                    return repo;
                }
                catch (Exception ex)
                {
                    Runtime.MessageCollector.AddExceptionMessage("Failed to load existing credential repository", ex);
                }
            }

            return null;
        }

        /// <summary>
        /// Explicitly persists the repository to disk using the encryption key.
        /// </summary>
        private void PersistRepository(ICredentialRepository repo)
        {
            try
            {
                repo.SaveCredentials(repo.Config.Key);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to save credentials: {ex.Message}",
                    Language.Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnCancelEdit_Click(object sender, EventArgs e)
        {
            EnableEditMode(false);
            ClearEditFields();
            UpdateButtonStates();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void EnableEditMode(bool enable)
        {
            grpCredentialDetails.Enabled = enable;
            UpdateButtonStates();
        }

        private void ClearEditFields()
        {
            txtTitle.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtDomain.Text = "";
        }

        private void PopulateEditFields(ICredentialRecord credential)
        {
            txtTitle.Text = credential.Title;
            txtUsername.Text = credential.Username;
            txtPassword.Text = ""; // Don't show the actual password, leave blank for editing
            txtDomain.Text = credential.Domain;
        }
    }
}
