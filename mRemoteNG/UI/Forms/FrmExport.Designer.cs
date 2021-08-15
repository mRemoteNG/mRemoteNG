
using System.Windows.Forms;
using mRemoteNG.UI.Controls;

namespace mRemoteNG.UI.Forms
{
	public partial class FrmExport : Form
	{
        #region  Windows Form Designer generated code
			
		private void InitializeComponent()
		{
            this.btnCancel = new MrngButton();
            this.btnOK = new MrngButton();
            this.lblUncheckProperties = new mRemoteNG.UI.Controls.MrngLabel();
            this.chkUsername = new MrngCheckBox();
            this.chkPassword = new MrngCheckBox();
            this.chkDomain = new MrngCheckBox();
            this.chkInheritance = new MrngCheckBox();
            this.txtFileName = new mRemoteNG.UI.Controls.MrngTextBox();
            this.btnBrowse = new MrngButton();
            this.grpProperties = new System.Windows.Forms.GroupBox();
            this.chkAssignedCredential = new MrngCheckBox();
            this.grpFile = new System.Windows.Forms.GroupBox();
            this.lblFileFormat = new mRemoteNG.UI.Controls.MrngLabel();
            this.lblFileName = new mRemoteNG.UI.Controls.MrngLabel();
            this.cboFileFormat = new MrngComboBox();
            this.grpItems = new System.Windows.Forms.GroupBox();
            this.lblSelectedConnection = new mRemoteNG.UI.Controls.MrngLabel();
            this.lblSelectedFolder = new mRemoteNG.UI.Controls.MrngLabel();
            this.rdoExportSelectedConnection = new mRemoteNG.UI.Controls.MrngRadioButton();
            this.rdoExportSelectedFolder = new mRemoteNG.UI.Controls.MrngRadioButton();
            this.rdoExportEverything = new mRemoteNG.UI.Controls.MrngRadioButton();
            this.grpProperties.SuspendLayout();
            this.grpFile.SuspendLayout();
            this.grpItems.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel._mice = MrngButton.MouseState.HOVER;
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
            this.btnOK._mice = MrngButton.MouseState.HOVER;
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
            this.lblUncheckProperties.Size = new System.Drawing.Size(264, 13);
            this.lblUncheckProperties.TabIndex = 4;
            this.lblUncheckProperties.Text = "Uncheck the properties you want not to be saved!";
            // 
            // chkUsername
            // 
            this.chkUsername._mice = MrngCheckBox.MouseState.HOVER;
            this.chkUsername.AutoSize = true;
            this.chkUsername.Checked = true;
            this.chkUsername.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUsername.Location = new System.Drawing.Point(15, 32);
            this.chkUsername.Name = "chkUsername";
            this.chkUsername.Size = new System.Drawing.Size(77, 17);
            this.chkUsername.TabIndex = 0;
            this.chkUsername.Text = "Username";
            this.chkUsername.UseVisualStyleBackColor = true;
            // 
            // chkPassword
            // 
            this.chkPassword._mice = MrngCheckBox.MouseState.HOVER;
            this.chkPassword.AutoSize = true;
            this.chkPassword.Checked = true;
            this.chkPassword.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPassword.Location = new System.Drawing.Point(15, 55);
            this.chkPassword.Name = "chkPassword";
            this.chkPassword.Size = new System.Drawing.Size(75, 17);
            this.chkPassword.TabIndex = 1;
            this.chkPassword.Text = "Password";
            this.chkPassword.UseVisualStyleBackColor = true;
            // 
            // chkDomain
            // 
            this.chkDomain._mice = MrngCheckBox.MouseState.HOVER;
            this.chkDomain.AutoSize = true;
            this.chkDomain.Checked = true;
            this.chkDomain.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDomain.Location = new System.Drawing.Point(15, 78);
            this.chkDomain.Name = "chkDomain";
            this.chkDomain.Size = new System.Drawing.Size(66, 17);
            this.chkDomain.TabIndex = 2;
            this.chkDomain.Text = "Domain";
            this.chkDomain.UseVisualStyleBackColor = true;
            // 
            // chkInheritance
            // 
            this.chkInheritance._mice = MrngCheckBox.MouseState.HOVER;
            this.chkInheritance.AutoSize = true;
            this.chkInheritance.Checked = true;
            this.chkInheritance.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkInheritance.Location = new System.Drawing.Point(15, 101);
            this.chkInheritance.Name = "chkInheritance";
            this.chkInheritance.Size = new System.Drawing.Size(84, 17);
            this.chkInheritance.TabIndex = 3;
            this.chkInheritance.Text = "Inheritance";
            this.chkInheritance.UseVisualStyleBackColor = true;
            // 
            // txtFileName
            // 
            this.txtFileName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFileName.Location = new System.Drawing.Point(15, 47);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(396, 22);
            this.txtFileName.TabIndex = 1;
            this.txtFileName.TextChanged += new System.EventHandler(this.txtFileName_TextChanged);
            // 
            // btnBrowse
            // 
            this.btnBrowse._mice = MrngButton.MouseState.HOVER;
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
            this.chkAssignedCredential._mice = MrngCheckBox.MouseState.HOVER;
            this.chkAssignedCredential.AutoSize = true;
            this.chkAssignedCredential.Checked = true;
            this.chkAssignedCredential.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAssignedCredential.Location = new System.Drawing.Point(143, 32);
            this.chkAssignedCredential.Name = "chkAssignedCredential";
            this.chkAssignedCredential.Size = new System.Drawing.Size(129, 17);
            this.chkAssignedCredential.TabIndex = 5;
            this.chkAssignedCredential.Text = "Assigned Credential";
            this.chkAssignedCredential.UseVisualStyleBackColor = true;
            this.chkAssignedCredential.Visible = false;
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
            this.lblFileFormat.Size = new System.Drawing.Size(67, 13);
            this.lblFileFormat.TabIndex = 3;
            this.lblFileFormat.Text = "File &Format:";
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point(12, 28);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(56, 13);
            this.lblFileName.TabIndex = 0;
            this.lblFileName.Text = "Filename:";
            // 
            // cboFileFormat
            // 
            this.cboFileFormat._mice = MrngComboBox.MouseState.HOVER;
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
            this.lblSelectedConnection.Size = new System.Drawing.Size(99, 13);
            this.lblSelectedConnection.TabIndex = 4;
            this.lblSelectedConnection.Text = "Connection Name";
            // 
            // lblSelectedFolder
            // 
            this.lblSelectedFolder.AutoSize = true;
            this.lblSelectedFolder.Location = new System.Drawing.Point(48, 75);
            this.lblSelectedFolder.Name = "lblSelectedFolder";
            this.lblSelectedFolder.Size = new System.Drawing.Size(72, 13);
            this.lblSelectedFolder.TabIndex = 3;
            this.lblSelectedFolder.Text = "Folder Name";
            // 
            // rdoExportSelectedConnection
            // 
            this.rdoExportSelectedConnection.AutoSize = true;
            this.rdoExportSelectedConnection.BackColor = System.Drawing.Color.Transparent;
            this.rdoExportSelectedConnection.Location = new System.Drawing.Point(15, 91);
            this.rdoExportSelectedConnection.Name = "rdoExportSelectedConnection";
            this.rdoExportSelectedConnection.Size = new System.Drawing.Size(232, 17);
            this.rdoExportSelectedConnection.TabIndex = 2;
            this.rdoExportSelectedConnection.TabStop = true;
            this.rdoExportSelectedConnection.Text = "Export the currently selected connection";
            this.rdoExportSelectedConnection.UseVisualStyleBackColor = true;
            // 
            // rdoExportSelectedFolder
            // 
            this.rdoExportSelectedFolder.AutoSize = true;
            this.rdoExportSelectedFolder.BackColor = System.Drawing.Color.Transparent;
            this.rdoExportSelectedFolder.Location = new System.Drawing.Point(15, 55);
            this.rdoExportSelectedFolder.Name = "rdoExportSelectedFolder";
            this.rdoExportSelectedFolder.Size = new System.Drawing.Size(205, 17);
            this.rdoExportSelectedFolder.TabIndex = 1;
            this.rdoExportSelectedFolder.TabStop = true;
            this.rdoExportSelectedFolder.Text = "Export the currently selected folder";
            this.rdoExportSelectedFolder.UseVisualStyleBackColor = true;
            // 
            // rdoExportEverything
            // 
            this.rdoExportEverything.AutoSize = true;
            this.rdoExportEverything.BackColor = System.Drawing.Color.Transparent;
            this.rdoExportEverything.Checked = true;
            this.rdoExportEverything.Location = new System.Drawing.Point(15, 32);
            this.rdoExportEverything.Name = "rdoExportEverything";
            this.rdoExportEverything.Size = new System.Drawing.Size(115, 17);
            this.rdoExportEverything.TabIndex = 0;
            this.rdoExportEverything.TabStop = true;
            this.rdoExportEverything.Text = "Export everything";
            this.rdoExportEverything.UseVisualStyleBackColor = true;
            // 
            // ExportForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(534, 508);
            this.Controls.Add(this.grpItems);
            this.Controls.Add(this.grpFile);
            this.Controls.Add(this.grpProperties);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
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
		private MrngButton btnCancel;
		private MrngButton btnOK;
		private Controls.MrngLabel lblUncheckProperties;
		private MrngCheckBox chkUsername;
		private MrngCheckBox chkPassword;
		private MrngCheckBox chkDomain;
		private MrngCheckBox chkInheritance;
		private Controls.MrngTextBox txtFileName;
		private MrngButton btnBrowse;
		private System.Windows.Forms.GroupBox grpProperties;
		private System.Windows.Forms.GroupBox grpFile;
		private Controls.MrngLabel lblFileFormat;
		private Controls.MrngLabel lblFileName;
		private MrngComboBox cboFileFormat;
		private System.Windows.Forms.GroupBox grpItems;
		private Controls.MrngLabel lblSelectedConnection;
		private Controls.MrngLabel lblSelectedFolder;
		private Controls.MrngRadioButton rdoExportSelectedConnection;
		private Controls.MrngRadioButton rdoExportSelectedFolder;
		private Controls.MrngRadioButton rdoExportEverything;
        #endregion

        private MrngCheckBox chkAssignedCredential;
    }
}
