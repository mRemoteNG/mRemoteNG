using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using mRemoteNG.Config.Connections;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Themes;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Forms
{
    [SupportedOSPlatform("windows")]
    public partial class FrmExport
    {
        private ThemeManager? _themeManager;

        #region Public Properties

        public string FileName
        {
            get => txtFileName.Text;
            set => txtFileName.Text = value;
        }

        public SaveFormat SaveFormat
        {
            get
            {
                ExportFormat? exportFormat = cboFileFormat.SelectedItem as ExportFormat;
                return exportFormat?.Format ?? SaveFormat.mRXML;
            }
            set
            {
                foreach (object item in cboFileFormat.Items)
                {
                    ExportFormat? exportFormat = item as ExportFormat;
                    if (exportFormat?.Format != value) continue;
                    cboFileFormat.SelectedItem = item;
                    break;
                }
            }
        }

        public ExportScope Scope
        {
            get
            {
                if (rdoExportSelectedFolder.Checked)
                    return ExportScope.SelectedFolder;
                if (rdoExportSelectedConnection.Checked)
                    return ExportScope.SelectedConnection;
                return ExportScope.Everything;
            }
            set
            {
                switch (value)
                {
                    case ExportScope.Everything:
                        rdoExportEverything.Checked = true;
                        break;
                    case ExportScope.SelectedFolder:
                        rdoExportSelectedFolder.Checked = true;
                        break;
                    case ExportScope.SelectedConnection:
                        rdoExportSelectedConnection.Checked = true;
                        break;
                }
            }
        }

        private ContainerInfo? _selectedFolder;

        public ContainerInfo? SelectedFolder
        {
            get => _selectedFolder;
            set
            {
                _selectedFolder = value;
                lblSelectedFolder.Text = value?.Name;
                rdoExportSelectedFolder.Enabled = value != null;
            }
        }

        private ConnectionInfo? _selectedConnection;

        public ConnectionInfo? SelectedConnection
        {
            get => _selectedConnection;
            set
            {
                _selectedConnection = value;
                lblSelectedConnection.Text = value?.Name;
                rdoExportSelectedConnection.Enabled = value != null;
            }
        }

        public bool IncludeUsername
        {
            get => chkUsername.Checked;
            set => chkUsername.Checked = value;
        }

        public bool IncludePassword
        {
            get => chkPassword.Checked;
            set => chkPassword.Checked = value;
        }

        public bool IncludeDomain
        {
            get => chkDomain.Checked;
            set => chkDomain.Checked = value;
        }

        public bool IncludeAssignedCredential
        {
            get => chkAssignedCredential.Checked;
            set => chkAssignedCredential.Checked = value;
        }

        public bool IncludeInheritance
        {
            get => chkInheritance.Checked;
            set => chkInheritance.Checked = value;
        }

        public bool IsEncrypted => chkEncrypt.Checked;
        public string Password => txtPassword.Text;

        #endregion

        #region Constructors

        public FrmExport()
        {
            InitializeComponent();
            Icon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.Export_16x);
            FontOverrider.FontOverride(this);
            SelectedFolder = null;
            SelectedConnection = null;
            btnOK.Enabled = false;
            ToggleEncryptionControls();
        }

        #endregion

        #region Private Methods

        #region Event Handlers

        private void ExportForm_Load(object sender, EventArgs e)
        {
            cboFileFormat.Items.Clear();
            cboFileFormat.Items.Add(new ExportFormat(SaveFormat.mRXML));
            cboFileFormat.Items.Add(new ExportFormat(SaveFormat.mRCSV));
            cboFileFormat.Items.Add(new ExportFormat(SaveFormat.mRJSON));
            cboFileFormat.Items.Add(new ExportFormat(SaveFormat.RDP));
            cboFileFormat.SelectedIndex = 0;
            ApplyTheme();
            ThemeManager.getInstance().ThemeChanged += ApplyTheme;
            ApplyLanguage();
        }

        private void txtFileName_TextChanged(object sender, EventArgs e)
        {
            ValidateForm();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new())
            {
                saveFileDialog.CheckPathExists = true;
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                saveFileDialog.OverwritePrompt = true;

                List<string> fileTypes = new();
                fileTypes.AddRange(new[] {Language.FiltermRemoteXML, "*.xml"});
                fileTypes.AddRange(new[] {Language.FiltermRemoteCSV, "*.csv"});
                fileTypes.AddRange(new[] {"mRemoteNG JSON|*.json"});
                fileTypes.AddRange(new[] {"RDP File (*.rdp)|*.rdp"});
                fileTypes.AddRange(new[] {Language.FilterAll, "*.*"});

                saveFileDialog.Filter = string.Join("|", fileTypes.ToArray());
                SelectFileTypeBasedOnSaveFormat(saveFileDialog);

                if (saveFileDialog.ShowDialog(this) != DialogResult.OK)
                    return;

                txtFileName.Text = saveFileDialog.FileName;
            }
        }

        private void SelectFileTypeBasedOnSaveFormat(FileDialog saveFileDialog)
        {
            saveFileDialog.FilterIndex = SaveFormat switch
            {
                SaveFormat.mRCSV => 2,
                SaveFormat.mRJSON => 3,
                SaveFormat.RDP => 4,
                _ => 1
            };
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if ((SaveFormat == SaveFormat.mRCSV || SaveFormat == SaveFormat.mRJSON) && IncludePassword)
            {
                if (MessageBox.Show("Exporting to this format with passwords enabled will save passwords in clear text. Are you sure you want to continue?",
                        "Security Warning",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    return;
                }
            }

            if (IsEncrypted)
            {
                if (string.IsNullOrEmpty(txtPassword.Text))
                {
                    MessageBox.Show("Password cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (txtPassword.Text != txtConfirm.Text)
                {
                    MessageBox.Show("Passwords do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void cboFileformat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SaveFormat != SaveFormat.mRXML)
            {
                chkEncrypt.Checked = false;
                chkEncrypt.Enabled = false;
            }
            else
            {
                chkEncrypt.Enabled = true;
            }
            ToggleEncryptionControls();
        }

        private void chkEncrypt_CheckedChanged(object sender, EventArgs e)
        {
            ToggleEncryptionControls();
            ValidateForm();
        }

        private void ToggleEncryptionControls()
        {
            bool enabled = chkEncrypt.Checked && chkEncrypt.Enabled;
            txtPassword.Enabled = enabled;
            txtConfirm.Enabled = enabled;
            lblPassword.Enabled = enabled;
            lblConfirm.Enabled = enabled;
        }

        private void ValidateForm()
        {
            btnOK.Enabled = !string.IsNullOrEmpty(txtFileName.Text);
        }

        #endregion

        private void ApplyTheme()
        {
            ThemeManager themeManager = ThemeManager.getInstance();
            _themeManager = themeManager;
            if (!themeManager.ActiveAndExtended) return;
            ExtendedColorPalette? palette = themeManager.ActiveTheme.ExtendedPalette;
            if (palette == null) return;
            BackColor = palette.getColor("Dialog_Background");
            ForeColor = palette.getColor("Dialog_Foreground");
        }


        private void ApplyLanguage()
        {
            Text = Language.Export;

            grpFile.Text = Language.ExportFile;
            lblFileName.Text = Language.Filename;
            btnBrowse.Text = Language._Browse;
            lblFileFormat.Text = Language.FileFormat;

            grpItems.Text = Language.ExportItems;
            rdoExportEverything.Text = Language.ExportEverything;
            rdoExportSelectedFolder.Text = Language.ExportSelectedFolder;
            rdoExportSelectedConnection.Text = Language.ExportSelectedConnection;

            grpEncryption.Text = "Encryption"; // TODO: Localize
            chkEncrypt.Text = "Encrypt File"; // TODO: Localize
            lblPassword.Text = Language.Password;
            lblConfirm.Text = "Confirm:"; // TODO: Localize

            grpProperties.Text = Language.ExportProperties;
            chkUsername.Text = Language.Username;
            chkPassword.Text = Language.Password;
            chkDomain.Text = Language.Domain;
            chkAssignedCredential.Text = Language.AssignedCredential;
            chkInheritance.Text = Language.Inheritance;
            lblUncheckProperties.Text = Language.UncheckProperties;

            btnOK.Text = Language._Ok;
            btnCancel.Text = Language._Cancel;
        }

        #endregion

        #region Public Enumerations

        public enum ExportScope
        {
            Everything,
            SelectedFolder,
            SelectedConnection
        }

        #endregion

        #region Private Classes

        [ImmutableObject(true)]
        private class ExportFormat(SaveFormat format)
        {
            #region Public Properties

            public SaveFormat Format { get; } = format;

            #endregion
            #region Constructors

            #endregion

            #region Public Methods

            public override string ToString()
            {
                return Format switch
                {
                    SaveFormat.mRXML => Language.MremoteNgXml,
                    SaveFormat.mRCSV => Language.MremoteNgCsv,
                    SaveFormat.mRJSON => "mRemoteNG JSON",
                    SaveFormat.RDP => "RDP File",
                    _ => Format.ToString()
                };
            }

            #endregion
        }

        #endregion
    }
}