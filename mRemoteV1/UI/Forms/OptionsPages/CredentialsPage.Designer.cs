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
            this.chkUseAdmPwd = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.radCredentialsCustom = new mRemoteNG.UI.Controls.Base.NGRadioButton();
            this.lblDefaultCredentials = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.radCredentialsNoInfo = new mRemoteNG.UI.Controls.Base.NGRadioButton();
            this.radCredentialsWindows = new mRemoteNG.UI.Controls.Base.NGRadioButton();
            this.txtCredentialsDomain = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.lblCredentialsUsername = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.txtCredentialsPassword = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.lblCredentialsPassword = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.txtCredentialsUsername = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.lblCredentialsDomain = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.pnlDefaultCredentials.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDefaultCredentials
            // 
            this.pnlDefaultCredentials.Controls.Add(this.chkUseAdmPwd);
            this.pnlDefaultCredentials.Controls.Add(this.radCredentialsCustom);
            this.pnlDefaultCredentials.Controls.Add(this.lblDefaultCredentials);
            this.pnlDefaultCredentials.Controls.Add(this.radCredentialsNoInfo);
            this.pnlDefaultCredentials.Controls.Add(this.radCredentialsWindows);
            this.pnlDefaultCredentials.Controls.Add(this.txtCredentialsDomain);
            this.pnlDefaultCredentials.Controls.Add(this.lblCredentialsUsername);
            this.pnlDefaultCredentials.Controls.Add(this.txtCredentialsPassword);
            this.pnlDefaultCredentials.Controls.Add(this.lblCredentialsPassword);
            this.pnlDefaultCredentials.Controls.Add(this.txtCredentialsUsername);
            this.pnlDefaultCredentials.Controls.Add(this.lblCredentialsDomain);
            this.pnlDefaultCredentials.Location = new System.Drawing.Point(3, 3);
            this.pnlDefaultCredentials.Name = "pnlDefaultCredentials";
            this.pnlDefaultCredentials.Size = new System.Drawing.Size(596, 210);
            this.pnlDefaultCredentials.TabIndex = 0;
            // 
            // chkUseAdmPwd
            // 
            this.chkUseAdmPwd._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.OUT;
            this.chkUseAdmPwd.AutoSize = true;
            this.chkUseAdmPwd.Enabled = false;
            this.chkUseAdmPwd.Location = new System.Drawing.Point(140, 149);
            this.chkUseAdmPwd.Name = "chkUseAdmPwd";
            this.chkUseAdmPwd.Size = new System.Drawing.Size(214, 17);
            this.chkUseAdmPwd.TabIndex = 10;
            this.chkUseAdmPwd.Text = "use AdmPwd.E to retrieve the password";
            this.chkUseAdmPwd.UseVisualStyleBackColor = true;
            this.chkUseAdmPwd.CheckedChanged += new System.EventHandler(this.chkUseAdmPwd_CheckedChanged);
            // 
            // radCredentialsCustom
            // 
            this.radCredentialsCustom.AutoSize = true;
            this.radCredentialsCustom.BackColor = System.Drawing.Color.Transparent;
            this.radCredentialsCustom.Location = new System.Drawing.Point(16, 69);
            this.radCredentialsCustom.Name = "radCredentialsCustom";
            this.radCredentialsCustom.Size = new System.Drawing.Size(87, 17);
            this.radCredentialsCustom.TabIndex = 3;
            this.radCredentialsCustom.Text = "the following:";
            this.radCredentialsCustom.UseVisualStyleBackColor = false;
            this.radCredentialsCustom.CheckedChanged += new System.EventHandler(this.radCredentialsCustom_CheckedChanged);
            // 
            // lblDefaultCredentials
            // 
            this.lblDefaultCredentials.AutoSize = true;
            this.lblDefaultCredentials.Location = new System.Drawing.Point(3, 9);
            this.lblDefaultCredentials.Name = "lblDefaultCredentials";
            this.lblDefaultCredentials.Size = new System.Drawing.Size(257, 13);
            this.lblDefaultCredentials.TabIndex = 0;
            this.lblDefaultCredentials.Text = "For empty Username, Password or Domain fields use:";
            // 
            // radCredentialsNoInfo
            // 
            this.radCredentialsNoInfo.AutoSize = true;
            this.radCredentialsNoInfo.BackColor = System.Drawing.Color.Transparent;
            this.radCredentialsNoInfo.Checked = true;
            this.radCredentialsNoInfo.Location = new System.Drawing.Point(16, 31);
            this.radCredentialsNoInfo.Name = "radCredentialsNoInfo";
            this.radCredentialsNoInfo.Size = new System.Drawing.Size(91, 17);
            this.radCredentialsNoInfo.TabIndex = 1;
            this.radCredentialsNoInfo.TabStop = true;
            this.radCredentialsNoInfo.Text = "no information";
            this.radCredentialsNoInfo.UseVisualStyleBackColor = false;
            // 
            // radCredentialsWindows
            // 
            this.radCredentialsWindows.AutoSize = true;
            this.radCredentialsWindows.BackColor = System.Drawing.Color.Transparent;
            this.radCredentialsWindows.Location = new System.Drawing.Point(16, 50);
            this.radCredentialsWindows.Name = "radCredentialsWindows";
            this.radCredentialsWindows.Size = new System.Drawing.Size(227, 17);
            this.radCredentialsWindows.TabIndex = 2;
            this.radCredentialsWindows.Text = "my current credentials (windows logon info)";
            this.radCredentialsWindows.UseVisualStyleBackColor = false;
            // 
            // txtCredentialsDomain
            // 
            this.txtCredentialsDomain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCredentialsDomain.Enabled = false;
            this.txtCredentialsDomain.Location = new System.Drawing.Point(140, 172);
            this.txtCredentialsDomain.Name = "txtCredentialsDomain";
            this.txtCredentialsDomain.Size = new System.Drawing.Size(150, 20);
            this.txtCredentialsDomain.TabIndex = 9;
            // 
            // lblCredentialsUsername
            // 
            this.lblCredentialsUsername.Enabled = false;
            this.lblCredentialsUsername.Location = new System.Drawing.Point(37, 95);
            this.lblCredentialsUsername.Name = "lblCredentialsUsername";
            this.lblCredentialsUsername.Size = new System.Drawing.Size(97, 13);
            this.lblCredentialsUsername.TabIndex = 4;
            this.lblCredentialsUsername.Text = "Username:";
            this.lblCredentialsUsername.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtCredentialsPassword
            // 
            this.txtCredentialsPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCredentialsPassword.Enabled = false;
            this.txtCredentialsPassword.Location = new System.Drawing.Point(140, 120);
            this.txtCredentialsPassword.Name = "txtCredentialsPassword";
            this.txtCredentialsPassword.Size = new System.Drawing.Size(150, 20);
            this.txtCredentialsPassword.TabIndex = 7;
            this.txtCredentialsPassword.UseSystemPasswordChar = true;
            // 
            // lblCredentialsPassword
            // 
            this.lblCredentialsPassword.Enabled = false;
            this.lblCredentialsPassword.Location = new System.Drawing.Point(34, 123);
            this.lblCredentialsPassword.Name = "lblCredentialsPassword";
            this.lblCredentialsPassword.Size = new System.Drawing.Size(100, 13);
            this.lblCredentialsPassword.TabIndex = 6;
            this.lblCredentialsPassword.Text = "Password:";
            this.lblCredentialsPassword.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtCredentialsUsername
            // 
            this.txtCredentialsUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCredentialsUsername.Enabled = false;
            this.txtCredentialsUsername.Location = new System.Drawing.Point(140, 93);
            this.txtCredentialsUsername.Name = "txtCredentialsUsername";
            this.txtCredentialsUsername.Size = new System.Drawing.Size(150, 20);
            this.txtCredentialsUsername.TabIndex = 5;
            // 
            // lblCredentialsDomain
            // 
            this.lblCredentialsDomain.Enabled = false;
            this.lblCredentialsDomain.Location = new System.Drawing.Point(34, 175);
            this.lblCredentialsDomain.Name = "lblCredentialsDomain";
            this.lblCredentialsDomain.Size = new System.Drawing.Size(100, 13);
            this.lblCredentialsDomain.TabIndex = 8;
            this.lblCredentialsDomain.Text = "Domain:";
            this.lblCredentialsDomain.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // CredentialsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlDefaultCredentials);
            this.Name = "CredentialsPage";
            this.Size = new System.Drawing.Size(610, 489);
            this.pnlDefaultCredentials.ResumeLayout(false);
            this.pnlDefaultCredentials.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        internal System.Windows.Forms.Panel pnlDefaultCredentials;
        internal Controls.Base.NGRadioButton radCredentialsCustom;
        internal Controls.Base.NGLabel lblDefaultCredentials;
        internal Controls.Base.NGRadioButton radCredentialsNoInfo;
        internal Controls.Base.NGRadioButton radCredentialsWindows;
        internal Controls.Base.NGTextBox txtCredentialsDomain;
        internal Controls.Base.NGLabel lblCredentialsUsername;
        internal Controls.Base.NGTextBox txtCredentialsPassword;
        internal Controls.Base.NGLabel lblCredentialsPassword;
        internal Controls.Base.NGTextBox txtCredentialsUsername;
        internal Controls.Base.NGLabel lblCredentialsDomain;
        private Controls.Base.NGCheckBox chkUseAdmPwd;
    }
}
