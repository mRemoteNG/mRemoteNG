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
            this.lblManualEntryUsername = new System.Windows.Forms.Label();
            this.txtManualEntryUsername = new System.Windows.Forms.TextBox();
            this.tabControlCredentialEditor = new System.Windows.Forms.TabControl();
            this.tabPageManualEntry = new System.Windows.Forms.TabPage();
            this.lblManualEntryPassword = new System.Windows.Forms.Label();
            this.maskedTextBoxManualEntryPassword = new System.Windows.Forms.MaskedTextBox();
            this.lblManualEntryDomain = new System.Windows.Forms.Label();
            this.txtManualEntryDomain = new System.Windows.Forms.TextBox();
            this.lblEntryName = new System.Windows.Forms.Label();
            this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
            this.lblCredentialSource = new System.Windows.Forms.Label();
            this.txtEntryName = new System.Windows.Forms.TextBox();
            this.lblEntryUUID = new System.Windows.Forms.Label();
            this.txtUUID = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tabControlCredentialEditor.SuspendLayout();
            this.tabPageManualEntry.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblManualEntryUsername
            // 
            this.lblManualEntryUsername.AutoSize = true;
            this.lblManualEntryUsername.Location = new System.Drawing.Point(22, 31);
            this.lblManualEntryUsername.Name = "lblManualEntryUsername";
            this.lblManualEntryUsername.Size = new System.Drawing.Size(55, 13);
            this.lblManualEntryUsername.TabIndex = 0;
            this.lblManualEntryUsername.Text = "Username";
            // 
            // txtManualEntryUsername
            // 
            this.txtManualEntryUsername.Location = new System.Drawing.Point(25, 47);
            this.txtManualEntryUsername.Name = "txtManualEntryUsername";
            this.txtManualEntryUsername.Size = new System.Drawing.Size(170, 20);
            this.txtManualEntryUsername.TabIndex = 1;
            // 
            // tabControlCredentialEditor
            // 
            this.tabControlCredentialEditor.Controls.Add(this.tabPageManualEntry);
            this.tabControlCredentialEditor.Location = new System.Drawing.Point(121, 111);
            this.tabControlCredentialEditor.Name = "tabControlCredentialEditor";
            this.tabControlCredentialEditor.SelectedIndex = 0;
            this.tabControlCredentialEditor.Size = new System.Drawing.Size(426, 286);
            this.tabControlCredentialEditor.TabIndex = 2;
            // 
            // tabPageManualEntry
            // 
            this.tabPageManualEntry.Controls.Add(this.txtManualEntryDomain);
            this.tabPageManualEntry.Controls.Add(this.lblManualEntryDomain);
            this.tabPageManualEntry.Controls.Add(this.maskedTextBoxManualEntryPassword);
            this.tabPageManualEntry.Controls.Add(this.lblManualEntryPassword);
            this.tabPageManualEntry.Controls.Add(this.txtManualEntryUsername);
            this.tabPageManualEntry.Controls.Add(this.lblManualEntryUsername);
            this.tabPageManualEntry.Location = new System.Drawing.Point(4, 22);
            this.tabPageManualEntry.Name = "tabPageManualEntry";
            this.tabPageManualEntry.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageManualEntry.Size = new System.Drawing.Size(418, 260);
            this.tabPageManualEntry.TabIndex = 0;
            this.tabPageManualEntry.Text = "ManualEntry";
            this.tabPageManualEntry.UseVisualStyleBackColor = true;
            // 
            // lblManualEntryPassword
            // 
            this.lblManualEntryPassword.AutoSize = true;
            this.lblManualEntryPassword.Location = new System.Drawing.Point(22, 130);
            this.lblManualEntryPassword.Name = "lblManualEntryPassword";
            this.lblManualEntryPassword.Size = new System.Drawing.Size(53, 13);
            this.lblManualEntryPassword.TabIndex = 2;
            this.lblManualEntryPassword.Text = "Password";
            // 
            // maskedTextBoxManualEntryPassword
            // 
            this.maskedTextBoxManualEntryPassword.Location = new System.Drawing.Point(25, 146);
            this.maskedTextBoxManualEntryPassword.Name = "maskedTextBoxManualEntryPassword";
            this.maskedTextBoxManualEntryPassword.Size = new System.Drawing.Size(170, 20);
            this.maskedTextBoxManualEntryPassword.TabIndex = 3;
            // 
            // lblManualEntryDomain
            // 
            this.lblManualEntryDomain.AutoSize = true;
            this.lblManualEntryDomain.Location = new System.Drawing.Point(22, 81);
            this.lblManualEntryDomain.Name = "lblManualEntryDomain";
            this.lblManualEntryDomain.Size = new System.Drawing.Size(43, 13);
            this.lblManualEntryDomain.TabIndex = 4;
            this.lblManualEntryDomain.Text = "Domain";
            // 
            // txtManualEntryDomain
            // 
            this.txtManualEntryDomain.Location = new System.Drawing.Point(25, 97);
            this.txtManualEntryDomain.Name = "txtManualEntryDomain";
            this.txtManualEntryDomain.Size = new System.Drawing.Size(170, 20);
            this.txtManualEntryDomain.TabIndex = 5;
            // 
            // lblEntryName
            // 
            this.lblEntryName.AutoSize = true;
            this.lblEntryName.Location = new System.Drawing.Point(122, 9);
            this.lblEntryName.Name = "lblEntryName";
            this.lblEntryName.Size = new System.Drawing.Size(62, 13);
            this.lblEntryName.TabIndex = 6;
            this.lblEntryName.Text = "Entry Name";
            // 
            // objectListView1
            // 
            this.objectListView1.CellEditUseWholeCell = false;
            this.objectListView1.Location = new System.Drawing.Point(-2, 127);
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.Size = new System.Drawing.Size(121, 270);
            this.objectListView1.TabIndex = 7;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            // 
            // lblCredentialSource
            // 
            this.lblCredentialSource.AutoSize = true;
            this.lblCredentialSource.Location = new System.Drawing.Point(12, 111);
            this.lblCredentialSource.Name = "lblCredentialSource";
            this.lblCredentialSource.Size = new System.Drawing.Size(91, 13);
            this.lblCredentialSource.TabIndex = 8;
            this.lblCredentialSource.Text = "Credential Source";
            // 
            // txtEntryName
            // 
            this.txtEntryName.Location = new System.Drawing.Point(190, 6);
            this.txtEntryName.Name = "txtEntryName";
            this.txtEntryName.Size = new System.Drawing.Size(242, 20);
            this.txtEntryName.TabIndex = 9;
            // 
            // lblEntryUUID
            // 
            this.lblEntryUUID.AutoSize = true;
            this.lblEntryUUID.Location = new System.Drawing.Point(122, 42);
            this.lblEntryUUID.Name = "lblEntryUUID";
            this.lblEntryUUID.Size = new System.Drawing.Size(61, 13);
            this.lblEntryUUID.TabIndex = 10;
            this.lblEntryUUID.Text = "Entry UUID";
            // 
            // txtUUID
            // 
            this.txtUUID.Location = new System.Drawing.Point(189, 39);
            this.txtUUID.Name = "txtUUID";
            this.txtUUID.ReadOnly = true;
            this.txtUUID.Size = new System.Drawing.Size(242, 20);
            this.txtUUID.TabIndex = 11;
            this.txtUUID.TabStop = false;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(373, 403);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 12;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(468, 403);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmCredentialEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 432);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtUUID);
            this.Controls.Add(this.lblEntryUUID);
            this.Controls.Add(this.txtEntryName);
            this.Controls.Add(this.lblCredentialSource);
            this.Controls.Add(this.objectListView1);
            this.Controls.Add(this.lblEntryName);
            this.Controls.Add(this.tabControlCredentialEditor);
            this.Name = "FrmCredentialEditor";
            this.Text = "FrmCredentialEditor";
            this.tabControlCredentialEditor.ResumeLayout(false);
            this.tabPageManualEntry.ResumeLayout(false);
            this.tabPageManualEntry.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private BrightIdeasSoftware.ObjectListView objectListView1;
        private System.Windows.Forms.Label lblCredentialSource;
        private System.Windows.Forms.TextBox txtEntryName;
        private System.Windows.Forms.Label lblEntryUUID;
        private System.Windows.Forms.TextBox txtUUID;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}