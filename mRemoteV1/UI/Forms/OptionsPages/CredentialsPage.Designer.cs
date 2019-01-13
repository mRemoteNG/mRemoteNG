﻿namespace mRemoteNG.UI.Forms.OptionsPages
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
            this.pnlDefaultCredentials.Size = new System.Drawing.Size(604, 175);
            this.pnlDefaultCredentials.TabIndex = 0;
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
            this.radCredentialsCustom.CheckedChanged += new System.EventHandler(this.radCredentialsCustom_CheckedChanged);
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
            // txtCredentialsDomain
            // 
            this.txtCredentialsDomain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCredentialsDomain.Enabled = false;
            this.txtCredentialsDomain.Location = new System.Drawing.Point(126, 138);
            this.txtCredentialsDomain.Name = "txtCredentialsDomain";
            this.txtCredentialsDomain.Size = new System.Drawing.Size(150, 22);
            this.txtCredentialsDomain.TabIndex = 9;
            // 
            // lblCredentialsUsername
            // 
            this.lblCredentialsUsername.Enabled = false;
            this.lblCredentialsUsername.Location = new System.Drawing.Point(20, 88);
            this.lblCredentialsUsername.Name = "lblCredentialsUsername";
            this.lblCredentialsUsername.Size = new System.Drawing.Size(100, 13);
            this.lblCredentialsUsername.TabIndex = 4;
            this.lblCredentialsUsername.Text = "Username:";
            this.lblCredentialsUsername.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtCredentialsPassword
            // 
            this.txtCredentialsPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCredentialsPassword.Enabled = false;
            this.txtCredentialsPassword.Location = new System.Drawing.Point(126, 112);
            this.txtCredentialsPassword.Name = "txtCredentialsPassword";
            this.txtCredentialsPassword.Size = new System.Drawing.Size(150, 22);
            this.txtCredentialsPassword.TabIndex = 7;
            this.txtCredentialsPassword.UseSystemPasswordChar = true;
            // 
            // lblCredentialsPassword
            // 
            this.lblCredentialsPassword.Enabled = false;
            this.lblCredentialsPassword.Location = new System.Drawing.Point(20, 115);
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
            this.txtCredentialsUsername.Location = new System.Drawing.Point(126, 86);
            this.txtCredentialsUsername.Name = "txtCredentialsUsername";
            this.txtCredentialsUsername.Size = new System.Drawing.Size(150, 22);
            this.txtCredentialsUsername.TabIndex = 5;
            // 
            // lblCredentialsDomain
            // 
            this.lblCredentialsDomain.Enabled = false;
            this.lblCredentialsDomain.Location = new System.Drawing.Point(20, 142);
            this.lblCredentialsDomain.Name = "lblCredentialsDomain";
            this.lblCredentialsDomain.Size = new System.Drawing.Size(100, 13);
            this.lblCredentialsDomain.TabIndex = 8;
            this.lblCredentialsDomain.Text = "Domain:";
            this.lblCredentialsDomain.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // CredentialsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.pnlDefaultCredentials);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "CredentialsPage";
            this.Size = new System.Drawing.Size(610, 490);
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
    }
}
