namespace mRemoteNG.UI.Forms
{
    partial class FrmCredentialEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCredentialEditor));
            this.lblManualEntryUsername = new System.Windows.Forms.Label();
            this.txtManualEntryUsername = new System.Windows.Forms.TextBox();
            this.tabControlCredentialEditor = new System.Windows.Forms.TabControl();
            this.tabPageManualEntry = new System.Windows.Forms.TabPage();
            this.txtManualEntryDomain = new System.Windows.Forms.TextBox();
            this.lblManualEntryDomain = new System.Windows.Forms.Label();
            this.maskedTextBoxManualEntryPassword = new System.Windows.Forms.MaskedTextBox();
            this.lblManualEntryPassword = new System.Windows.Forms.Label();
            this.tabPageKeePass = new System.Windows.Forms.TabPage();
            this.lblEntryName = new System.Windows.Forms.Label();
            this.txtEntryName = new System.Windows.Forms.TextBox();
            this.lblEntryUUID = new System.Windows.Forms.Label();
            this.txtUUID = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBoxCredentialSourceDetails = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxSourceSelector = new System.Windows.Forms.ComboBox();
            this.lblSeparator = new System.Windows.Forms.Label();
            this.tabControlCredentialEditor.SuspendLayout();
            this.tabPageManualEntry.SuspendLayout();
            this.groupBoxCredentialSourceDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblManualEntryUsername
            // 
            this.lblManualEntryUsername.AutoSize = true;
            this.lblManualEntryUsername.Location = new System.Drawing.Point(6, 12);
            this.lblManualEntryUsername.Name = "lblManualEntryUsername";
            this.lblManualEntryUsername.Size = new System.Drawing.Size(55, 13);
            this.lblManualEntryUsername.TabIndex = 0;
            this.lblManualEntryUsername.Text = "Username";
            // 
            // txtManualEntryUsername
            // 
            this.txtManualEntryUsername.Location = new System.Drawing.Point(9, 28);
            this.txtManualEntryUsername.Name = "txtManualEntryUsername";
            this.txtManualEntryUsername.Size = new System.Drawing.Size(170, 20);
            this.txtManualEntryUsername.TabIndex = 1;
            this.txtManualEntryUsername.TextChanged += new System.EventHandler(this.txtManualEntryUsername_TextChanged);
            // 
            // tabControlCredentialEditor
            // 
            this.tabControlCredentialEditor.Controls.Add(this.tabPageManualEntry);
            this.tabControlCredentialEditor.Controls.Add(this.tabPageKeePass);
            this.tabControlCredentialEditor.Location = new System.Drawing.Point(1, 122);
            this.tabControlCredentialEditor.Name = "tabControlCredentialEditor";
            this.tabControlCredentialEditor.SelectedIndex = 0;
            this.tabControlCredentialEditor.Size = new System.Drawing.Size(507, 296);
            this.tabControlCredentialEditor.TabIndex = 2;
            // 
            // tabPageManualEntry
            // 
            this.tabPageManualEntry.BackColor = System.Drawing.Color.Transparent;
            this.tabPageManualEntry.Controls.Add(this.txtManualEntryDomain);
            this.tabPageManualEntry.Controls.Add(this.lblManualEntryDomain);
            this.tabPageManualEntry.Controls.Add(this.maskedTextBoxManualEntryPassword);
            this.tabPageManualEntry.Controls.Add(this.lblManualEntryPassword);
            this.tabPageManualEntry.Controls.Add(this.txtManualEntryUsername);
            this.tabPageManualEntry.Controls.Add(this.lblManualEntryUsername);
            this.tabPageManualEntry.Location = new System.Drawing.Point(4, 22);
            this.tabPageManualEntry.Name = "tabPageManualEntry";
            this.tabPageManualEntry.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageManualEntry.Size = new System.Drawing.Size(499, 270);
            this.tabPageManualEntry.TabIndex = 0;
            this.tabPageManualEntry.Text = "ManualEntry";
            // 
            // txtManualEntryDomain
            // 
            this.txtManualEntryDomain.Location = new System.Drawing.Point(9, 78);
            this.txtManualEntryDomain.Name = "txtManualEntryDomain";
            this.txtManualEntryDomain.Size = new System.Drawing.Size(170, 20);
            this.txtManualEntryDomain.TabIndex = 5;
            this.txtManualEntryDomain.TextChanged += new System.EventHandler(this.txtManualEntryDomain_TextChanged);
            // 
            // lblManualEntryDomain
            // 
            this.lblManualEntryDomain.AutoSize = true;
            this.lblManualEntryDomain.Location = new System.Drawing.Point(6, 62);
            this.lblManualEntryDomain.Name = "lblManualEntryDomain";
            this.lblManualEntryDomain.Size = new System.Drawing.Size(43, 13);
            this.lblManualEntryDomain.TabIndex = 4;
            this.lblManualEntryDomain.Text = "Domain";
            // 
            // maskedTextBoxManualEntryPassword
            // 
            this.maskedTextBoxManualEntryPassword.Location = new System.Drawing.Point(9, 127);
            this.maskedTextBoxManualEntryPassword.Name = "maskedTextBoxManualEntryPassword";
            this.maskedTextBoxManualEntryPassword.PasswordChar = '●';
            this.maskedTextBoxManualEntryPassword.Size = new System.Drawing.Size(170, 20);
            this.maskedTextBoxManualEntryPassword.TabIndex = 3;
            this.maskedTextBoxManualEntryPassword.TextChanged += new System.EventHandler(this.maskedTextBoxManualEntryPassword_TextChanged);
            // 
            // lblManualEntryPassword
            // 
            this.lblManualEntryPassword.AutoSize = true;
            this.lblManualEntryPassword.Location = new System.Drawing.Point(6, 111);
            this.lblManualEntryPassword.Name = "lblManualEntryPassword";
            this.lblManualEntryPassword.Size = new System.Drawing.Size(53, 13);
            this.lblManualEntryPassword.TabIndex = 2;
            this.lblManualEntryPassword.Text = "Password";
            // 
            // tabPageKeePass
            // 
            this.tabPageKeePass.Location = new System.Drawing.Point(4, 22);
            this.tabPageKeePass.Name = "tabPageKeePass";
            this.tabPageKeePass.Size = new System.Drawing.Size(499, 270);
            this.tabPageKeePass.TabIndex = 1;
            this.tabPageKeePass.Text = "KeePass";
            this.tabPageKeePass.UseVisualStyleBackColor = true;
            // 
            // lblEntryName
            // 
            this.lblEntryName.AutoSize = true;
            this.lblEntryName.Location = new System.Drawing.Point(11, 22);
            this.lblEntryName.Name = "lblEntryName";
            this.lblEntryName.Size = new System.Drawing.Size(35, 13);
            this.lblEntryName.TabIndex = 6;
            this.lblEntryName.Text = "Name";
            // 
            // txtEntryName
            // 
            this.txtEntryName.Location = new System.Drawing.Point(79, 19);
            this.txtEntryName.Name = "txtEntryName";
            this.txtEntryName.Size = new System.Drawing.Size(242, 20);
            this.txtEntryName.TabIndex = 9;
            // 
            // lblEntryUUID
            // 
            this.lblEntryUUID.AutoSize = true;
            this.lblEntryUUID.Location = new System.Drawing.Point(11, 48);
            this.lblEntryUUID.Name = "lblEntryUUID";
            this.lblEntryUUID.Size = new System.Drawing.Size(34, 13);
            this.lblEntryUUID.TabIndex = 10;
            this.lblEntryUUID.Text = "UUID";
            // 
            // txtUUID
            // 
            this.txtUUID.Location = new System.Drawing.Point(79, 45);
            this.txtUUID.Name = "txtUUID";
            this.txtUUID.ReadOnly = true;
            this.txtUUID.Size = new System.Drawing.Size(242, 20);
            this.txtUUID.TabIndex = 11;
            this.txtUUID.TabStop = false;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(348, 445);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 12;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(429, 445);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBoxCredentialSourceDetails
            // 
            this.groupBoxCredentialSourceDetails.Controls.Add(this.label1);
            this.groupBoxCredentialSourceDetails.Controls.Add(this.comboBoxSourceSelector);
            this.groupBoxCredentialSourceDetails.Controls.Add(this.txtEntryName);
            this.groupBoxCredentialSourceDetails.Controls.Add(this.lblEntryName);
            this.groupBoxCredentialSourceDetails.Controls.Add(this.txtUUID);
            this.groupBoxCredentialSourceDetails.Controls.Add(this.lblEntryUUID);
            this.groupBoxCredentialSourceDetails.Location = new System.Drawing.Point(-1, 12);
            this.groupBoxCredentialSourceDetails.Name = "groupBoxCredentialSourceDetails";
            this.groupBoxCredentialSourceDetails.Size = new System.Drawing.Size(517, 104);
            this.groupBoxCredentialSourceDetails.TabIndex = 14;
            this.groupBoxCredentialSourceDetails.TabStop = false;
            this.groupBoxCredentialSourceDetails.Text = "Credential Source Details";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Source";
            // 
            // comboBoxSourceSelector
            // 
            this.comboBoxSourceSelector.FormattingEnabled = true;
            this.comboBoxSourceSelector.Location = new System.Drawing.Point(79, 71);
            this.comboBoxSourceSelector.Name = "comboBoxSourceSelector";
            this.comboBoxSourceSelector.Size = new System.Drawing.Size(155, 21);
            this.comboBoxSourceSelector.TabIndex = 17;
            // 
            // lblSeparator
            // 
            this.lblSeparator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSeparator.Location = new System.Drawing.Point(-18, 430);
            this.lblSeparator.Name = "lblSeparator";
            this.lblSeparator.Size = new System.Drawing.Size(534, 2);
            this.lblSeparator.TabIndex = 16;
            // 
            // FrmCredentialEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 479);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tabControlCredentialEditor);
            this.Controls.Add(this.groupBoxCredentialSourceDetails);
            this.Controls.Add(this.lblSeparator);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmCredentialEditor";
            this.Text = "FrmCredentialEditor";
            this.tabControlCredentialEditor.ResumeLayout(false);
            this.tabPageManualEntry.ResumeLayout(false);
            this.tabPageManualEntry.PerformLayout();
            this.groupBoxCredentialSourceDetails.ResumeLayout(false);
            this.groupBoxCredentialSourceDetails.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblManualEntryUsername;
        private System.Windows.Forms.TextBox txtManualEntryUsername;
        private System.Windows.Forms.TabControl tabControlCredentialEditor;
        private System.Windows.Forms.TabPage tabPageManualEntry;
        private System.Windows.Forms.TextBox txtManualEntryDomain;
        private System.Windows.Forms.Label lblManualEntryDomain;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxManualEntryPassword;
        private System.Windows.Forms.Label lblManualEntryPassword;
        private System.Windows.Forms.Label lblEntryName;
        private System.Windows.Forms.TextBox txtEntryName;
        private System.Windows.Forms.Label lblEntryUUID;
        private System.Windows.Forms.TextBox txtUUID;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBoxCredentialSourceDetails;
        private System.Windows.Forms.TabPage tabPageKeePass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxSourceSelector;
        private System.Windows.Forms.Label lblSeparator;
    }
}