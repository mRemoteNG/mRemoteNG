using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using mRemoteNG.App;

namespace mRemoteNG.UI.Forms
{
    public partial class ExportForm
    {
        #region Public Properties
        public string FileName
		{
			get
			{
				return txtFileName.Text;
			}
			set
			{
				txtFileName.Text = value;
			}
		}
			
        public Config.Connections.ConnectionsSaver.Format SaveFormat
		{
			get
			{
				ExportFormat exportFormat = cboFileFormat.SelectedItem as ExportFormat;
				if (exportFormat == null)
				{
					return Config.Connections.ConnectionsSaver.Format.mRXML;
				}
				else
				{
					return exportFormat.Format;
				}
			}
			set
			{
				foreach (object item in cboFileFormat.Items)
				{
					ExportFormat exportFormat = item as ExportFormat;
				    if (exportFormat?.Format == value)
					{
						cboFileFormat.SelectedItem = item;
						break;
					}
				}
			}
		}
			
        public ExportScope Scope
		{
			get
			{
				if (rdoExportSelectedFolder.Checked)
				{
					return ExportScope.SelectedFolder;
				}
				else if (rdoExportSelectedConnection.Checked)
				{
					return ExportScope.SelectedConnection;
				}
				else
				{
					return ExportScope.Everything;
				}
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
			
		private TreeNode _selectedFolder;
        public TreeNode SelectedFolder
		{
			get
			{
				return _selectedFolder;
			}
			set
			{
				_selectedFolder = value;
				lblSelectedFolder.Text = value == null ? string.Empty : value.Text;
				rdoExportSelectedFolder.Enabled = value != null;
			}
		}
			
		private TreeNode _selectedConnection;
        public TreeNode SelectedConnection
		{
			get
			{
				return _selectedConnection;
			}
			set
			{
				_selectedConnection = value;
				lblSelectedConnection.Text = value == null ? string.Empty : value.Text;
				rdoExportSelectedConnection.Enabled = value != null;
			}
		}
			
        public bool IncludeUsername
		{
			get
			{
				return chkUsername.Checked;
			}
			set
			{
				chkUsername.Checked = value;
			}
		}
			
        public bool IncludePassword
		{
			get
			{
				return chkPassword.Checked;
			}
			set
			{
				chkPassword.Checked = value;
			}
		}
			
        public bool IncludeDomain
		{
			get
			{
				return chkDomain.Checked;
			}
			set
			{
				chkDomain.Checked = value;
			}
		}
			
        public bool IncludeInheritance
		{
			get
			{
				return chkInheritance.Checked;
			}
			set
			{
				chkInheritance.Checked = value;
			}
		}
        #endregion
			
        #region Constructors
		public ExportForm()
		{
			InitializeComponent();
				
			Runtime.FontOverride(this);
				
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
            cboFileFormat.Items.Add(new ExportFormat(Config.Connections.ConnectionsSaver.Format.mRXML));
            cboFileFormat.Items.Add(new ExportFormat(Config.Connections.ConnectionsSaver.Format.mRCSV));
            cboFileFormat.Items.Add(new ExportFormat(Config.Connections.ConnectionsSaver.Format.vRDCSV));
			cboFileFormat.SelectedIndex = 0;
				
			ApplyLanguage();
		}

        private void txtFileName_TextChanged(object sender, EventArgs e)
		{
			btnOK.Enabled = !string.IsNullOrEmpty(txtFileName.Text);
		}

        private void btnBrowse_Click(object sender, EventArgs e)
		{
			using (SaveFileDialog saveFileDialog = new SaveFileDialog())
			{
				saveFileDialog.CheckPathExists = true;
				saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				saveFileDialog.OverwritePrompt = true;
					
				List<string> fileTypes = new List<string>();
				fileTypes.AddRange(new[] {Language.strFiltermRemoteXML, "*.xml"});
				fileTypes.AddRange(new[] {Language.strFiltermRemoteCSV, "*.csv"});
				fileTypes.AddRange(new[] {Language.strFiltervRD2008CSV, "*.csv"});
				fileTypes.AddRange(new[] {Language.strFilterAll, "*.*"});
					
				saveFileDialog.Filter = string.Join("|", fileTypes.ToArray());
					
				if (saveFileDialog.ShowDialog(this) != DialogResult.OK)
				{
					return ;
				}
					
				txtFileName.Text = saveFileDialog.FileName;
            }
				
		}

        private void btnOK_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}

        private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}
        #endregion
			
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
			private Config.Connections.ConnectionsSaver.Format _format;
            public Config.Connections.ConnectionsSaver.Format Format => _format;

		    #endregion
				
            #region Constructors
			public ExportFormat(Config.Connections.ConnectionsSaver.Format format)
			{
				_format = format;
			}
            #endregion
				
            #region Public Methods
			public override string ToString()
			{
				switch (Format)
				{
					case Config.Connections.ConnectionsSaver.Format.mRXML:
						return Language.strMremoteNgXml;
                    case Config.Connections.ConnectionsSaver.Format.mRCSV:
						return Language.strMremoteNgCsv;
                    case Config.Connections.ConnectionsSaver.Format.vRDCSV:
						return Language.strVisionAppRemoteDesktopCsv;
					default:
						return Format.ToString();
				}
			}
            #endregion
		}
        #endregion
	}
}
