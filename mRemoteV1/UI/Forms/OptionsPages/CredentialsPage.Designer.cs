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
            this.credentialRecordComboBox1 = new mRemoteNG.UI.Controls.CredentialRecordComboBox();
            this.radCredentialsCustom = new mRemoteNG.UI.Controls.Base.NGRadioButton();
            this.lblDefaultCredentials = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.radCredentialsNoInfo = new mRemoteNG.UI.Controls.Base.NGRadioButton();
            this.radCredentialsWindows = new mRemoteNG.UI.Controls.Base.NGRadioButton();
            this.checkBoxUnlockOnStartup = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.defaultCredsLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.defaultCredsLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // credentialRecordComboBox1
            // 
            this.credentialRecordComboBox1.CredentialRecords = null;
            this.credentialRecordComboBox1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.credentialRecordComboBox1.FormattingEnabled = true;
            this.credentialRecordComboBox1.Location = new System.Drawing.Point(127, 62);
            this.credentialRecordComboBox1.Name = "credentialRecordComboBox1";
            this.credentialRecordComboBox1.Size = new System.Drawing.Size(172, 21);
            this.credentialRecordComboBox1.TabIndex = 2;
            // 
            // radCredentialsCustom
            // 
            this.radCredentialsCustom.AutoSize = true;
            this.radCredentialsCustom.BackColor = System.Drawing.Color.Transparent;
            this.radCredentialsCustom.Location = new System.Drawing.Point(23, 62);
            this.radCredentialsCustom.Name = "radCredentialsCustom";
            this.radCredentialsCustom.Size = new System.Drawing.Size(98, 17);
            this.radCredentialsCustom.TabIndex = 3;
            this.radCredentialsCustom.Text = "the following:";
            this.radCredentialsCustom.UseVisualStyleBackColor = false;
            // 
            // lblDefaultCredentials
            // 
            this.lblDefaultCredentials.AutoSize = true;
            this.defaultCredsLayoutPanel.SetColumnSpan(this.lblDefaultCredentials, 3);
            this.lblDefaultCredentials.Location = new System.Drawing.Point(3, 0);
            this.lblDefaultCredentials.Name = "lblDefaultCredentials";
            this.lblDefaultCredentials.Size = new System.Drawing.Size(279, 13);
            this.lblDefaultCredentials.TabIndex = 0;
            this.lblDefaultCredentials.Text = "For empty Username, Password or Domain fields use:";
            // 
            // radCredentialsNoInfo
            // 
            this.radCredentialsNoInfo.AutoSize = true;
            this.radCredentialsNoInfo.BackColor = System.Drawing.Color.Transparent;
            this.radCredentialsNoInfo.Checked = true;
            this.defaultCredsLayoutPanel.SetColumnSpan(this.radCredentialsNoInfo, 2);
            this.radCredentialsNoInfo.Location = new System.Drawing.Point(23, 16);
            this.radCredentialsNoInfo.Name = "radCredentialsNoInfo";
            this.radCredentialsNoInfo.Size = new System.Drawing.Size(103, 17);
            this.radCredentialsNoInfo.TabIndex = 1;
            this.radCredentialsNoInfo.TabStop = true;
            this.radCredentialsNoInfo.Text = "no information";
            this.radCredentialsNoInfo.UseVisualStyleBackColor = false;
            // 
            // radCredentialsWindows
            // 
            this.radCredentialsWindows.AutoSize = true;
            this.radCredentialsWindows.BackColor = System.Drawing.Color.Transparent;
            this.defaultCredsLayoutPanel.SetColumnSpan(this.radCredentialsWindows, 2);
            this.radCredentialsWindows.Location = new System.Drawing.Point(23, 39);
            this.radCredentialsWindows.Name = "radCredentialsWindows";
            this.radCredentialsWindows.Size = new System.Drawing.Size(252, 17);
            this.radCredentialsWindows.TabIndex = 2;
            this.radCredentialsWindows.Text = "my current credentials (windows logon info)";
            this.radCredentialsWindows.UseVisualStyleBackColor = false;
            // 
            // checkBoxUnlockOnStartup
            // 
            this.checkBoxUnlockOnStartup._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.OUT;
            this.checkBoxUnlockOnStartup.AutoSize = true;
            this.checkBoxUnlockOnStartup.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxUnlockOnStartup.Location = new System.Drawing.Point(3, 3);
            this.checkBoxUnlockOnStartup.Name = "checkBoxUnlockOnStartup";
            this.checkBoxUnlockOnStartup.Size = new System.Drawing.Size(290, 17);
            this.checkBoxUnlockOnStartup.TabIndex = 1;
            this.checkBoxUnlockOnStartup.Text = "Prompt to unlock credential repositories on startup";
            this.checkBoxUnlockOnStartup.UseVisualStyleBackColor = true;
            // 
            // defaultCredsLayoutPanel
            // 
            this.defaultCredsLayoutPanel.AutoSize = true;
            this.defaultCredsLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.defaultCredsLayoutPanel.ColumnCount = 3;
            this.defaultCredsLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.defaultCredsLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.defaultCredsLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.defaultCredsLayoutPanel.Controls.Add(this.lblDefaultCredentials, 0, 0);
            this.defaultCredsLayoutPanel.Controls.Add(this.radCredentialsNoInfo, 1, 1);
            this.defaultCredsLayoutPanel.Controls.Add(this.radCredentialsWindows, 1, 2);
            this.defaultCredsLayoutPanel.Controls.Add(this.credentialRecordComboBox1, 2, 3);
            this.defaultCredsLayoutPanel.Controls.Add(this.radCredentialsCustom, 1, 3);
            this.defaultCredsLayoutPanel.Location = new System.Drawing.Point(3, 36);
            this.defaultCredsLayoutPanel.Name = "defaultCredsLayoutPanel";
            this.defaultCredsLayoutPanel.RowCount = 4;
            this.defaultCredsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.defaultCredsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.defaultCredsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.defaultCredsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.defaultCredsLayoutPanel.Size = new System.Drawing.Size(302, 86);
            this.defaultCredsLayoutPanel.TabIndex = 2;
            // 
            // CredentialsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.defaultCredsLayoutPanel);
            this.Controls.Add(this.checkBoxUnlockOnStartup);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CredentialsPage";
            this.Size = new System.Drawing.Size(610, 490);
            this.defaultCredsLayoutPanel.ResumeLayout(false);
            this.defaultCredsLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal Controls.Base.NGRadioButton radCredentialsCustom;
        internal Controls.Base.NGLabel lblDefaultCredentials;
        internal Controls.Base.NGRadioButton radCredentialsNoInfo;
        internal Controls.Base.NGRadioButton radCredentialsWindows;
        private Controls.Base.NGCheckBox checkBoxUnlockOnStartup;
        private Controls.CredentialRecordComboBox credentialRecordComboBox1;
        private System.Windows.Forms.TableLayoutPanel defaultCredsLayoutPanel;
    }
}
