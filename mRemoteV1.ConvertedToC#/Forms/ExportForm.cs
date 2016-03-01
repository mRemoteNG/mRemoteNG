using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using mRemoteNG.Config.Connections;
using mRemoteNG.App;
using mRemoteNG.My;

namespace mRemoteNG.Forms
{
	public partial class ExportForm : Form
	{

		#region "Public Properties"
		public string FileName {
			get { return txtFileName.Text; }
			set { txtFileName.Text = value; }
		}

		public Config.Connections.Save.Format SaveFormat {
			get {
				ExportFormat exportFormat = cboFileFormat.SelectedItem as ExportFormat;
				if (exportFormat == null) {
					return mRemoteNG.Config.Connections.Save.Format.mRXML;
				} else {
					return exportFormat.Format;
				}
			}
			set {
				foreach (object item in cboFileFormat.Items) {
					ExportFormat exportFormat = item as ExportFormat;
					if (exportFormat == null)
						continue;
					if (exportFormat.Format == value) {
						cboFileFormat.SelectedItem = item;
						break; // TODO: might not be correct. Was : Exit For
					}
				}
			}
		}

		public ExportScope Scope {
			get {
				if (rdoExportSelectedFolder.Checked) {
					return ExportScope.SelectedFolder;
				} else if (rdoExportSelectedConnection.Checked) {
					return ExportScope.SelectedConnection;
				} else {
					return ExportScope.Everything;
				}
			}
			set {
				switch (value) {
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
		public TreeNode SelectedFolder {
			get { return _selectedFolder; }
			set {
				_selectedFolder = value;
				if (value == null) {
					lblSelectedFolder.Text = string.Empty;
				} else {
					lblSelectedFolder.Text = value.Text;
				}
				rdoExportSelectedFolder.Enabled = (value != null);
			}
		}

		private TreeNode _selectedConnection;
		public TreeNode SelectedConnection {
			get { return _selectedConnection; }
			set {
				_selectedConnection = value;
				if (value == null) {
					lblSelectedConnection.Text = string.Empty;
				} else {
					lblSelectedConnection.Text = value.Text;
				}
				rdoExportSelectedConnection.Enabled = (value != null);
			}
		}

		public bool IncludeUsername {
			get { return chkUsername.Checked; }
			set { chkUsername.Checked = value; }
		}

		public bool IncludePassword {
			get { return chkPassword.Checked; }
			set { chkPassword.Checked = value; }
		}

		public bool IncludeDomain {
			get { return chkDomain.Checked; }
			set { chkDomain.Checked = value; }
		}

		public bool IncludeInheritance {
			get { return chkInheritance.Checked; }
			set { chkInheritance.Checked = value; }
		}
		#endregion

		#region "Constructors"
		public ExportForm()
		{
			Load += ExportForm_Load;
			InitializeComponent();

			Runtime.FontOverride(ref this);

			SelectedFolder = null;
			SelectedConnection = null;

			btnOK.Enabled = false;
		}
		#endregion

		#region "Private Methods"
		#region "Event Handlers"
		private void ExportForm_Load(object sender, EventArgs e)
		{
			cboFileFormat.Items.Clear();
			cboFileFormat.Items.Add(new ExportFormat(Save.Format.mRXML));
			cboFileFormat.Items.Add(new ExportFormat(Save.Format.mRCSV));
			cboFileFormat.Items.Add(new ExportFormat(Save.Format.vRDCSV));
			cboFileFormat.SelectedIndex = 0;

			ApplyLanguage();
		}

		private void txtFileName_TextChanged(System.Object sender, EventArgs e)
		{
			btnOK.Enabled = !string.IsNullOrEmpty(txtFileName.Text);
		}

		private void btnBrowse_Click(System.Object sender, EventArgs e)
		{
			using (SaveFileDialog saveFileDialog = new SaveFileDialog()) {
				var _with1 = saveFileDialog;
				_with1.CheckPathExists = true;
				_with1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				_with1.OverwritePrompt = true;

				List<string> fileTypes = new List<string>();
				fileTypes.AddRange({
					Language.strFiltermRemoteXML,
					"*.xml"
				});
				fileTypes.AddRange({
					Language.strFiltermRemoteCSV,
					"*.csv"
				});
				fileTypes.AddRange({
					Language.strFiltervRD2008CSV,
					"*.csv"
				});
				fileTypes.AddRange({
					Language.strFilterAll,
					"*.*"
				});

				_with1.Filter = string.Join("|", fileTypes.ToArray());

				if (!(saveFileDialog.ShowDialog(this) == DialogResult.OK))
					return;

				txtFileName.Text = saveFileDialog.FileName;
			}
		}

		private void btnOK_Click(System.Object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}

		private void btnCancel_Click(System.Object sender, EventArgs e)
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

		#region "Public Enumerations"
		public enum ExportScope : int
		{
			Everything,
			SelectedFolder,
			SelectedConnection
		}
		#endregion

		#region "Private Classes"
		[ImmutableObject(true)]
		private class ExportFormat
		{
			#region "Public Properties"
			private readonly Config.Connections.Save.Format _format;
			public Config.Connections.Save.Format Format {
				get { return _format; }
			}
			#endregion

			#region "Constructors"
			public ExportFormat(Config.Connections.Save.Format format)
			{
				_format = format;
			}
			#endregion

			#region "Public Methods"
			public override string ToString()
			{
				switch (Format) {
					case mRemoteNG.Config.Connections.Save.Format.mRXML:
						return Language.strMremoteNgXml;
					case mRemoteNG.Config.Connections.Save.Format.mRCSV:
						return Language.strMremoteNgCsv;
					case mRemoteNG.Config.Connections.Save.Format.vRDCSV:
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
