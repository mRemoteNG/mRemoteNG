namespace mRemoteNG.UI.Forms.OptionsPages
{
    sealed partial class CredentialsPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlDefaultCredentials = new System.Windows.Forms.Panel();
            this.credentialRecordComboBox1 = new mRemoteNG.UI.Controls.CredentialRecordComboBox();
            this.radCredentialsCustom = new mRemoteNG.UI.Controls.Base.NGRadioButton();
            this.lblDefaultCredentials = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.radCredentialsNoInfo = new mRemoteNG.UI.Controls.Base.NGRadioButton();
            this.radCredentialsWindows = new mRemoteNG.UI.Controls.Base.NGRadioButton();
            this.checkBoxUnlockOnStartup = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.pnlDefaultCredentials.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDefaultCredentials
            // 
            this.pnlDefaultCredentials.Controls.Add(this.credentialRecordComboBox1);
            this.pnlDefaultCredentials.Controls.Add(this.radCredentialsCustom);
            this.pnlDefaultCredentials.Controls.Add(this.lblDefaultCredentials);
            this.pnlDefaultCredentials.Controls.Add(this.radCredentialsNoInfo);
            this.pnlDefaultCredentials.Controls.Add(this.radCredentialsWindows);
            this.pnlDefaultCredentials.Location = new System.Drawing.Point(3, 26);
            this.pnlDefaultCredentials.Name = "pnlDefaultCredentials";
            this.pnlDefaultCredentials.Size = new System.Drawing.Size(604, 96);
            this.pnlDefaultCredentials.TabIndex = 0;
            // 
            // credentialRecordComboBox1
            // 
            this.credentialRecordComboBox1.CredentialRecords = null;
            this.credentialRecordComboBox1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.credentialRecordComboBox1.FormattingEnabled = true;
            this.credentialRecordComboBox1.Location = new System.Drawing.Point(107, 62);
            this.credentialRecordComboBox1.Name = "credentialRecordComboBox1";
            this.credentialRecordComboBox1.Size = new System.Drawing.Size(172, 21);
            this.credentialRecordComboBox1.TabIndex = 2;
            // 
            // radCredentialsCustom
            // 
            this.radCredentialsCustom.AutoSize = true;
            this.radCredentialsCustom.Location = new System.Drawing.Point(3, 62);
            this.radCredentialsCustom.Name = "radCredentialsCustom";
            this.radCredentialsCustom.Size = new System.Drawing.Size(98, 17);
            this.radCredentialsCustom.TabIndex = 3;
            this.radCredentialsCustom.Text = "the following:";
            this.radCredentialsCustom.UseVisualStyleBackColor = true;
            // 
            // lblDefaultCredentials
            // 
            this.lblDefaultCredentials.AutoSize = true;
            this.lblDefaultCredentials.Location = new System.Drawing.Point(0, 0);
            this.lblDefaultCredentials.Name = "lblDefaultCredentials";
            this.lblDefaultCredentials.Size = new System.Drawing.Size(279, 13);
            this.lblDefaultCredentials.TabIndex = 0;
            this.lblDefaultCredentials.Text = "For empty Username, Password or Domain fields use:";
            // 
            // radCredentialsNoInfo
            // 
            this.radCredentialsNoInfo.AutoSize = true;
            this.radCredentialsNoInfo.Checked = true;
            this.radCredentialsNoInfo.Location = new System.Drawing.Point(3, 16);
            this.radCredentialsNoInfo.Name = "radCredentialsNoInfo";
            this.radCredentialsNoInfo.Size = new System.Drawing.Size(103, 17);
            this.radCredentialsNoInfo.TabIndex = 1;
            this.radCredentialsNoInfo.TabStop = true;
            this.radCredentialsNoInfo.Text = "no information";
            this.radCredentialsNoInfo.UseVisualStyleBackColor = true;
            // 
            // radCredentialsWindows
            // 
            this.radCredentialsWindows.AutoSize = true;
            this.radCredentialsWindows.Location = new System.Drawing.Point(3, 39);
            this.radCredentialsWindows.Name = "radCredentialsWindows";
            this.radCredentialsWindows.Size = new System.Drawing.Size(252, 17);
            this.radCredentialsWindows.TabIndex = 2;
            this.radCredentialsWindows.Text = "my current credentials (windows logon info)";
            this.radCredentialsWindows.UseVisualStyleBackColor = true;
            // 
            // checkBoxUnlockOnStartup
            // 
            this.checkBoxUnlockOnStartup._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.HOVER;
            this.checkBoxUnlockOnStartup.AutoSize = true;
            this.checkBoxUnlockOnStartup.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxUnlockOnStartup.Location = new System.Drawing.Point(3, 3);
            this.checkBoxUnlockOnStartup.Name = "checkBoxUnlockOnStartup";
            this.checkBoxUnlockOnStartup.Size = new System.Drawing.Size(290, 17);
            this.checkBoxUnlockOnStartup.TabIndex = 1;
            this.checkBoxUnlockOnStartup.Text = "Prompt to unlock credential repositories on startup";
            this.checkBoxUnlockOnStartup.UseVisualStyleBackColor = true;
            // 
            // CredentialsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.checkBoxUnlockOnStartup);
            this.Controls.Add(this.pnlDefaultCredentials);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "CredentialsPage";
            this.Size = new System.Drawing.Size(610, 490);
            this.pnlDefaultCredentials.ResumeLayout(false);
            this.pnlDefaultCredentials.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.Panel pnlDefaultCredentials;
        internal Controls.Base.NGRadioButton radCredentialsCustom;
        internal Controls.Base.NGLabel lblDefaultCredentials;
        internal Controls.Base.NGRadioButton radCredentialsNoInfo;
        internal Controls.Base.NGRadioButton radCredentialsWindows;
        private Controls.Base.NGCheckBox checkBoxUnlockOnStartup;
        private Controls.CredentialRecordComboBox credentialRecordComboBox1;
    }
}
