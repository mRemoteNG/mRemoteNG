using System.Windows.Forms;

namespace mRemoteNG.UI.Forms
{
	public partial class ExportForm : Form
	{
        #region  Windows Form Designer generated code
			
		private void InitializeComponent()
		{
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblUncheckProperties = new System.Windows.Forms.Label();
            this.chkUsername = new System.Windows.Forms.CheckBox();
            this.chkPassword = new System.Windows.Forms.CheckBox();
            this.chkDomain = new System.Windows.Forms.CheckBox();
            this.chkInheritance = new System.Windows.Forms.CheckBox();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.grpProperties = new System.Windows.Forms.GroupBox();
            this.chkAssignedCredential = new System.Windows.Forms.CheckBox();
            this.grpFile = new System.Windows.Forms.GroupBox();
            this.lblFileFormat = new System.Windows.Forms.Label();
            this.lblFileName = new System.Windows.Forms.Label();
            this.cboFileFormat = new System.Windows.Forms.ComboBox();
            this.grpItems = new System.Windows.Forms.GroupBox();
            this.lblSelectedConnection = new System.Windows.Forms.Label();
            this.lblSelectedFolder = new System.Windows.Forms.Label();
            this.rdoExportSelectedConnection = new System.Windows.Forms.RadioButton();
            this.rdoExportSelectedFolder = new System.Windows.Forms.RadioButton();
            this.rdoExportEverything = new System.Windows.Forms.RadioButton();
            this.grpProperties.SuspendLayout();
            this.grpFile.SuspendLayout();
            this.grpItems.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(447, 473);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(366, 473);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblUncheckProperties
            // 
            this.lblUncheckProperties.AutoSize = true;
            this.lblUncheckProperties.Location = new System.Drawing.Point(12, 134);
            this.lblUncheckProperties.Name = "lblUncheckProperties";
            this.lblUncheckProperties.Size = new System.Drawing.Size(244, 13);
            this.lblUncheckProperties.TabIndex = 4;
            this.lblUncheckProperties.Text = "Uncheck the properties you want not to be saved!";
            // 
            // chkUsername
            // 
            this.chkUsername.AutoSize = true;
            this.chkUsername.Checked = true;
            this.chkUsername.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUsername.Location = new System.Drawing.Point(15, 32);
            this.chkUsername.Name = "chkUsername";
            this.chkUsername.Size = new System.Drawing.Size(74, 17);
            this.chkUsername.TabIndex = 0;
            this.chkUsername.Text = "Username";
            this.chkUsername.UseVisualStyleBackColor = true;
            // 
            // chkPassword
            // 
            this.chkPassword.AutoSize = true;
            this.chkPassword.Checked = true;
            this.chkPassword.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPassword.Location = new System.Drawing.Point(15, 55);
            this.chkPassword.Name = "chkPassword";
            this.chkPassword.Size = new System.Drawing.Size(72, 17);
            this.chkPassword.TabIndex = 1;
            this.chkPassword.Text = "Password";
            this.chkPassword.UseVisualStyleBackColor = true;
            // 
            // chkDomain
            // 
            this.chkDomain.AutoSize = true;
            this.chkDomain.Checked = true;
            this.chkDomain.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDomain.Location = new System.Drawing.Point(15, 78);
            this.chkDomain.Name = "chkDomain";
            this.chkDomain.Size = new System.Drawing.Size(62, 17);
            this.chkDomain.TabIndex = 2;
            this.chkDomain.Text = "Domain";
            this.chkDomain.UseVisualStyleBackColor = true;
            // 
            // chkInheritance
            // 
            this.chkInheritance.AutoSize = true;
            this.chkInheritance.Checked = true;
            this.chkInheritance.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkInheritance.Location = new System.Drawing.Point(15, 101);
            this.chkInheritance.Name = "chkInheritance";
            this.chkInheritance.Size = new System.Drawing.Size(79, 17);
            this.chkInheritance.TabIndex = 3;
            this.chkInheritance.Text = "Inheritance";
            this.chkInheritance.UseVisualStyleBackColor = true;
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(15, 48);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(396, 20);
            this.txtFileName.TabIndex = 1;
            this.txtFileName.TextChanged += new System.EventHandler(this.txtFileName_TextChanged);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(417, 46);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "&Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // grpProperties
            // 
            this.grpProperties.Controls.Add(this.chkAssignedCredential);
            this.grpProperties.Controls.Add(this.lblUncheckProperties);
            this.grpProperties.Controls.Add(this.chkInheritance);
            this.grpProperties.Controls.Add(this.chkUsername);
            this.grpProperties.Controls.Add(this.chkDomain);
            this.grpProperties.Controls.Add(this.chkPassword);
            this.grpProperties.Location = new System.Drawing.Point(12, 304);
            this.grpProperties.Name = "grpProperties";
            this.grpProperties.Size = new System.Drawing.Size(510, 163);
            this.grpProperties.TabIndex = 1;
            this.grpProperties.TabStop = false;
            this.grpProperties.Text = "Export Properties";
            // 
            // chkAssignedCredential
            // 
            this.chkAssignedCredential.AutoSize = true;
            this.chkAssignedCredential.Checked = true;
            this.chkAssignedCredential.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAssignedCredential.Location = new System.Drawing.Point(143, 32);
            this.chkAssignedCredential.Name = "chkAssignedCredential";
            this.chkAssignedCredential.Size = new System.Drawing.Size(119, 17);
            this.chkAssignedCredential.TabIndex = 5;
            this.chkAssignedCredential.Text = "Assigned Credential";
            this.chkAssignedCredential.UseVisualStyleBackColor = true;
            // 
            // grpFile
            // 
            this.grpFile.Controls.Add(this.lblFileFormat);
            this.grpFile.Controls.Add(this.lblFileName);
            this.grpFile.Controls.Add(this.cboFileFormat);
            this.grpFile.Controls.Add(this.txtFileName);
            this.grpFile.Controls.Add(this.btnBrowse);
            this.grpFile.Location = new System.Drawing.Point(12, 12);
            this.grpFile.Name = "grpFile";
            this.grpFile.Size = new System.Drawing.Size(510, 140);
            this.grpFile.TabIndex = 0;
            this.grpFile.TabStop = false;
            this.grpFile.Text = "Export File";
            // 
            // lblFileFormat
            // 
            this.lblFileFormat.AutoSize = true;
            this.lblFileFormat.Location = new System.Drawing.Point(12, 80);
            this.lblFileFormat.Name = "lblFileFormat";
            this.lblFileFormat.Size = new System.Drawing.Size(61, 13);
            this.lblFileFormat.TabIndex = 3;
            this.lblFileFormat.Text = "File &Format:";
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point(12, 28);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(52, 13);
            this.lblFileName.TabIndex = 0;
            this.lblFileName.Text = "Filename:";
            // 
            // cboFileFormat
            // 
            this.cboFileFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFileFormat.FormattingEnabled = true;
            this.cboFileFormat.Location = new System.Drawing.Point(15, 100);
            this.cboFileFormat.Name = "cboFileFormat";
            this.cboFileFormat.Size = new System.Drawing.Size(294, 21);
            this.cboFileFormat.TabIndex = 4;
            this.cboFileFormat.SelectedIndexChanged += new System.EventHandler(this.cboFileformat_SelectedIndexChanged);
            // 
            // grpItems
            // 
            this.grpItems.Controls.Add(this.lblSelectedConnection);
            this.grpItems.Controls.Add(this.lblSelectedFolder);
            this.grpItems.Controls.Add(this.rdoExportSelectedConnection);
            this.grpItems.Controls.Add(this.rdoExportSelectedFolder);
            this.grpItems.Controls.Add(this.rdoExportEverything);
            this.grpItems.Location = new System.Drawing.Point(12, 158);
            this.grpItems.Name = "grpItems";
            this.grpItems.Size = new System.Drawing.Size(510, 140);
            this.grpItems.TabIndex = 4;
            this.grpItems.TabStop = false;
            this.grpItems.Text = "Export Items";
            // 
            // lblSelectedConnection
            // 
            this.lblSelectedConnection.AutoSize = true;
            this.lblSelectedConnection.Location = new System.Drawing.Point(48, 111);
            this.lblSelectedConnection.Name = "lblSelectedConnection";
            this.lblSelectedConnection.Size = new System.Drawing.Size(92, 13);
            this.lblSelectedConnection.TabIndex = 4;
            this.lblSelectedConnection.Text = "Connection Name";
            // 
            // lblSelectedFolder
            // 
            this.lblSelectedFolder.AutoSize = true;
            this.lblSelectedFolder.Location = new System.Drawing.Point(48, 75);
            this.lblSelectedFolder.Name = "lblSelectedFolder";
            this.lblSelectedFolder.Size = new System.Drawing.Size(67, 13);
            this.lblSelectedFolder.TabIndex = 3;
            this.lblSelectedFolder.Text = "Folder Name";
            // 
            // rdoExportSelectedConnection
            // 
            this.rdoExportSelectedConnection.AutoSize = true;
            this.rdoExportSelectedConnection.Location = new System.Drawing.Point(15, 91);
            this.rdoExportSelectedConnection.Name = "rdoExportSelectedConnection";
            this.rdoExportSelectedConnection.Size = new System.Drawing.Size(215, 17);
            this.rdoExportSelectedConnection.TabIndex = 2;
            this.rdoExportSelectedConnection.TabStop = true;
            this.rdoExportSelectedConnection.Text = "Export the currently selected connection";
            this.rdoExportSelectedConnection.UseVisualStyleBackColor = true;
            // 
            // rdoExportSelectedFolder
            // 
            this.rdoExportSelectedFolder.AutoSize = true;
            this.rdoExportSelectedFolder.Location = new System.Drawing.Point(15, 55);
            this.rdoExportSelectedFolder.Name = "rdoExportSelectedFolder";
            this.rdoExportSelectedFolder.Size = new System.Drawing.Size(188, 17);
            this.rdoExportSelectedFolder.TabIndex = 1;
            this.rdoExportSelectedFolder.TabStop = true;
            this.rdoExportSelectedFolder.Text = "Export the currently selected folder";
            this.rdoExportSelectedFolder.UseVisualStyleBackColor = true;
            // 
            // rdoExportEverything
            // 
            this.rdoExportEverything.AutoSize = true;
            this.rdoExportEverything.Checked = true;
            this.rdoExportEverything.Location = new System.Drawing.Point(15, 32);
            this.rdoExportEverything.Name = "rdoExportEverything";
            this.rdoExportEverything.Size = new System.Drawing.Size(107, 17);
            this.rdoExportEverything.TabIndex = 0;
            this.rdoExportEverything.TabStop = true;
            this.rdoExportEverything.Text = "Export everything";
            this.rdoExportEverything.UseVisualStyleBackColor = true;
            // 
            // ExportForm
            // 
            this.AcceptButton = this.btnOK;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(534, 508);
            this.Controls.Add(this.grpItems);
            this.Controls.Add(this.grpFile);
            this.Controls.Add(this.grpProperties);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = global::mRemoteNG.Resources.Connections_SaveAs_Icon;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export Connections";
            this.Load += new System.EventHandler(this.ExportForm_Load);
            this.grpProperties.ResumeLayout(false);
            this.grpProperties.PerformLayout();
            this.grpFile.ResumeLayout(false);
            this.grpFile.PerformLayout();
            this.grpItems.ResumeLayout(false);
            this.grpItems.PerformLayout();
            this.ResumeLayout(false);

		}
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Label lblUncheckProperties;
		private System.Windows.Forms.CheckBox chkUsername;
		private System.Windows.Forms.CheckBox chkPassword;
		private System.Windows.Forms.CheckBox chkDomain;
		private System.Windows.Forms.CheckBox chkInheritance;
		private System.Windows.Forms.TextBox txtFileName;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.GroupBox grpProperties;
		private System.Windows.Forms.GroupBox grpFile;
		private System.Windows.Forms.Label lblFileFormat;
		private System.Windows.Forms.Label lblFileName;
		private System.Windows.Forms.ComboBox cboFileFormat;
		private System.Windows.Forms.GroupBox grpItems;
		private System.Windows.Forms.Label lblSelectedConnection;
		private System.Windows.Forms.Label lblSelectedFolder;
		private System.Windows.Forms.RadioButton rdoExportSelectedConnection;
		private System.Windows.Forms.RadioButton rdoExportSelectedFolder;
		private System.Windows.Forms.RadioButton rdoExportEverything;
        #endregion

        private CheckBox chkAssignedCredential;
    }
}
