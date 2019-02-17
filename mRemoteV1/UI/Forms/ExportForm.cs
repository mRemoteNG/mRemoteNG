using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using mRemoteNG.Config.Connections;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Forms
{
    public partial class ExportForm
    {
        private ThemeManager _themeManager;

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
                var exportFormat = cboFileFormat.SelectedItem as ExportFormat;
                return exportFormat?.Format ?? SaveFormat.mRXML;
            }
            set
            {
                foreach (var item in cboFileFormat.Items)
                {
                    var exportFormat = item as ExportFormat;
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

        private ContainerInfo _selectedFolder;

        public ContainerInfo SelectedFolder
        {
            get => _selectedFolder;
            set
            {
                _selectedFolder = value;
                lblSelectedFolder.Text = value?.Name;
                rdoExportSelectedFolder.Enabled = value != null;
            }
        }

        private ConnectionInfo _selectedConnection;

        public ConnectionInfo SelectedConnection
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

        #endregion

        #region Constructors

        public ExportForm()
        {
            InitializeComponent();
            FontOverrider.FontOverride(this);
            SelectedFolder = null;
            SelectedConnection = null;
            btnOK.Enabled = false;
        }

        #endregion

        #region Private Methods

        #region Event Handlers

        private void ExportForm_Load(object sender, EventArgs e)
        {
            cboFileFormat.Items.Clear();
            cboFileFormat.Items.Add(new ExportFormat(SaveFormat.mRXML));
            cboFileFormat.Items.Add(new ExportFormat(SaveFormat.mRCSV));
            cboFileFormat.SelectedIndex = 0;
            ApplyTheme();
            ThemeManager.getInstance().ThemeChanged += ApplyTheme;
            ApplyLanguage();
        }

        private void txtFileName_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = !string.IsNullOrEmpty(txtFileName.Text);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.CheckPathExists = true;
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                saveFileDialog.OverwritePrompt = true;

                var fileTypes = new List<string>();
                fileTypes.AddRange(new[] {Language.strFiltermRemoteXML, "*.xml"});
                fileTypes.AddRange(new[] {Language.strFiltermRemoteCSV, "*.csv"});
                fileTypes.AddRange(new[] {Language.strFilterAll, "*.*"});

                saveFileDialog.Filter = string.Join("|", fileTypes.ToArray());
                SelectFileTypeBasedOnSaveFormat(saveFileDialog);

                if (saveFileDialog.ShowDialog(this) != DialogResult.OK)
                    return;

                txtFileName.Text = saveFileDialog.FileName;
            }
        }

        private void SelectFileTypeBasedOnSaveFormat(FileDialog saveFileDialog)
        {
            saveFileDialog.FilterIndex = SaveFormat == SaveFormat.mRCSV ? 2 : 1;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void cboFileformat_SelectedIndexChanged(object sender, EventArgs e)
        {
            // should only be active if we are using the credential manager feature
            //if (SaveFormat == SaveFormat.mRXML)
            //{
            //    chkUsername.Enabled = false;
            //    chkPassword.Enabled = false;
            //    chkDomain.Enabled = false;
            //    chkAssignedCredential.Enabled = true;
            //}
            //else
            //{
            //    chkUsername.Enabled = true;
            //    chkPassword.Enabled = true;
            //    chkDomain.Enabled = true;
            //    chkAssignedCredential.Enabled = false;
            //}
        }

        #endregion

        private void ApplyTheme()
        {
            _themeManager = ThemeManager.getInstance();
            if (!_themeManager.ActiveAndExtended) return;
            BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
        }


        private void ApplyLanguage()
        {
            Text = Language.strExport;

            grpFile.Text = Language.strExportFile;
            lblFileName.Text = Language.strLabelFilename;
            btnBrowse.Text = Language.strButtonBrowse;
            lblFileFormat.Text = Language.strFileFormatLabel;

            grpItems.Text = Language.strExportItems;
            rdoExportEverything.Text = Language.strExportEverything;
            rdoExportSelectedFolder.Text = Language.strExportSelectedFolder;
            rdoExportSelectedConnection.Text = Language.strExportSelectedConnection;

            grpProperties.Text = Language.strExportProperties;
            chkUsername.Text = Language.strCheckboxUsername;
            chkPassword.Text = Language.strCheckboxPassword;
            chkDomain.Text = Language.strCheckboxDomain;
            chkAssignedCredential.Text = Language.strAssignedCredential;
            chkInheritance.Text = Language.strCheckboxInheritance;
            lblUncheckProperties.Text = Language.strUncheckProperties;

            btnOK.Text = Language.strButtonOK;
            btnCancel.Text = Language.strButtonCancel;
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
        private class ExportFormat
        {
            #region Public Properties

            public SaveFormat Format { get; }

            #endregion

            #region Constructors

            public ExportFormat(SaveFormat format)
            {
                Format = format;
            }

            #endregion

            #region Public Methods

            public override string ToString()
            {
                switch (Format)
                {
                    case SaveFormat.mRXML:
                        return Language.strMremoteNgXml;
                    case SaveFormat.mRCSV:
                        return Language.strMremoteNgCsv;
                    default:
                        return Format.ToString();
                }
            }

            #endregion
        }

        #endregion
    }
}