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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtCredentialsUserViaAPI = new mRemoteNG.UI.Controls.MrngTextBox();
            this.txtCredentialsUsername = new mRemoteNG.UI.Controls.MrngTextBox();
            this.txtCredentialsPassword = new mRemoteNG.UI.Controls.MrngTextBox();
            this.txtCredentialsDomain = new mRemoteNG.UI.Controls.MrngTextBox();
            this.lblCredentialsDomain = new mRemoteNG.UI.Controls.MrngLabel();
            this.lblCredentialsUserViaAPI = new mRemoteNG.UI.Controls.MrngLabel();
            this.lblCredentialsUsername = new mRemoteNG.UI.Controls.MrngLabel();
            this.lblCredentialsPassword = new mRemoteNG.UI.Controls.MrngLabel();
            this.radCredentialsCustom = new mRemoteNG.UI.Controls.MrngRadioButton();
            this.lblDefaultCredentials = new mRemoteNG.UI.Controls.MrngLabel();
            this.radCredentialsNoInfo = new mRemoteNG.UI.Controls.MrngRadioButton();
            this.radCredentialsWindows = new mRemoteNG.UI.Controls.MrngRadioButton();
            this.pnlDefaultCredentials.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDefaultCredentials
            // 
            this.pnlDefaultCredentials.Controls.Add(this.tableLayoutPanel1);
            this.pnlDefaultCredentials.Controls.Add(this.radCredentialsCustom);
            this.pnlDefaultCredentials.Controls.Add(this.lblDefaultCredentials);
            this.pnlDefaultCredentials.Controls.Add(this.radCredentialsNoInfo);
            this.pnlDefaultCredentials.Controls.Add(this.radCredentialsWindows);
            this.pnlDefaultCredentials.Location = new System.Drawing.Point(3, 3);
            this.pnlDefaultCredentials.Name = "pnlDefaultCredentials";
            this.pnlDefaultCredentials.Size = new System.Drawing.Size(604, 200);
            this.pnlDefaultCredentials.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.txtCredentialsUserViaAPI, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtCredentialsUsername, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtCredentialsPassword, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtCredentialsDomain, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblCredentialsUserViaAPI, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblCredentialsUsername, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblCredentialsPassword, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblCredentialsDomain, 0, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(23, 85);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(332, 110);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // txtCredentialsUserViaAPI
            // 
            this.txtCredentialsUserViaAPI.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCredentialsUserViaAPI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCredentialsUserViaAPI.Enabled = false;
            this.txtCredentialsUserViaAPI.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCredentialsUserViaAPI.Location = new System.Drawing.Point(163, 3);
            this.txtCredentialsUserViaAPI.Name = "txtCredentialsUserViaAPI";
            this.txtCredentialsUserViaAPI.Size = new System.Drawing.Size(166, 22);
            this.txtCredentialsUserViaAPI.TabIndex = 5;
            // 
            // txtCredentialsUsername
            // 
            this.txtCredentialsUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCredentialsUsername.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCredentialsUsername.Enabled = false;
            this.txtCredentialsUsername.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCredentialsUsername.Location = new System.Drawing.Point(163, 3);
            this.txtCredentialsUsername.Name = "txtCredentialsUsername";
            this.txtCredentialsUsername.Size = new System.Drawing.Size(166, 22);
            this.txtCredentialsUsername.TabIndex = 5;
            // 
            // txtCredentialsPassword
            // 
            this.txtCredentialsPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCredentialsPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCredentialsPassword.Enabled = false;
            this.txtCredentialsPassword.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCredentialsPassword.Location = new System.Drawing.Point(163, 29);
            this.txtCredentialsPassword.Name = "txtCredentialsPassword";
            this.txtCredentialsPassword.Size = new System.Drawing.Size(166, 22);
            this.txtCredentialsPassword.TabIndex = 7;
            this.txtCredentialsPassword.UseSystemPasswordChar = true;
            // 
            // txtCredentialsDomain
            // 
            this.txtCredentialsDomain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCredentialsDomain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCredentialsDomain.Enabled = false;
            this.txtCredentialsDomain.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCredentialsDomain.Location = new System.Drawing.Point(163, 55);
            this.txtCredentialsDomain.Name = "txtCredentialsDomain";
            this.txtCredentialsDomain.Size = new System.Drawing.Size(166, 22);
            this.txtCredentialsDomain.TabIndex = 9;
            // 
            // lblCredentialsDomain
            // 
            this.lblCredentialsDomain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCredentialsDomain.Enabled = false;
            this.lblCredentialsDomain.Location = new System.Drawing.Point(3, 57);
            this.lblCredentialsDomain.Name = "lblCredentialsDomain";
            this.lblCredentialsDomain.Size = new System.Drawing.Size(154, 19);
            this.lblCredentialsDomain.TabIndex = 8;
            this.lblCredentialsDomain.Text = "Domain:";
            this.lblCredentialsDomain.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblCredentialsUserViaAPI
            // 
            this.lblCredentialsUserViaAPI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCredentialsUserViaAPI.Enabled = false;
            this.lblCredentialsUserViaAPI.Location = new System.Drawing.Point(3, 3);
            this.lblCredentialsUserViaAPI.Name = "lblCredentialsUserViaAPI";
            this.lblCredentialsUserViaAPI.Size = new System.Drawing.Size(154, 19);
            this.lblCredentialsUserViaAPI.TabIndex = 4;
            this.lblCredentialsUserViaAPI.Text = "User via API ID:";
            this.lblCredentialsUserViaAPI.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblCredentialsUsername
            // 
            this.lblCredentialsUsername.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCredentialsUsername.Enabled = false;
            this.lblCredentialsUsername.Location = new System.Drawing.Point(3, 3);
            this.lblCredentialsUsername.Name = "lblCredentialsUsername";
            this.lblCredentialsUsername.Size = new System.Drawing.Size(154, 19);
            this.lblCredentialsUsername.TabIndex = 4;
            this.lblCredentialsUsername.Text = "Username:";
            this.lblCredentialsUsername.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblCredentialsPassword
            // 
            this.lblCredentialsPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCredentialsPassword.Enabled = false;
            this.lblCredentialsPassword.Location = new System.Drawing.Point(3, 29);
            this.lblCredentialsPassword.Name = "lblCredentialsPassword";
            this.lblCredentialsPassword.Size = new System.Drawing.Size(154, 19);
            this.lblCredentialsPassword.TabIndex = 6;
            this.lblCredentialsPassword.Text = "Password:";
            this.lblCredentialsPassword.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // radCredentialsCustom
            // 
            this.radCredentialsCustom.AutoSize = true;
            this.radCredentialsCustom.BackColor = System.Drawing.Color.Transparent;
            this.radCredentialsCustom.Location = new System.Drawing.Point(3, 62);
            this.radCredentialsCustom.Name = "radCredentialsCustom";
            this.radCredentialsCustom.Size = new System.Drawing.Size(98, 17);
            this.radCredentialsCustom.TabIndex = 3;
            this.radCredentialsCustom.Text = "the following:";
            this.radCredentialsCustom.UseVisualStyleBackColor = false;
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
            this.radCredentialsNoInfo.BackColor = System.Drawing.Color.Transparent;
            this.radCredentialsNoInfo.Checked = true;
            this.radCredentialsNoInfo.Location = new System.Drawing.Point(3, 16);
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
            this.radCredentialsWindows.Location = new System.Drawing.Point(3, 39);
            this.radCredentialsWindows.Name = "radCredentialsWindows";
            this.radCredentialsWindows.Size = new System.Drawing.Size(252, 17);
            this.radCredentialsWindows.TabIndex = 2;
            this.radCredentialsWindows.Text = "my current credentials (windows logon info)";
            this.radCredentialsWindows.UseVisualStyleBackColor = false;
            // 
            // CredentialsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.pnlDefaultCredentials);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CredentialsPage";
            this.Size = new System.Drawing.Size(610, 490);
            this.pnlDefaultCredentials.ResumeLayout(false);
            this.pnlDefaultCredentials.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        internal System.Windows.Forms.Panel pnlDefaultCredentials;
        internal Controls.MrngRadioButton radCredentialsCustom;
        internal Controls.MrngLabel lblDefaultCredentials;
        internal Controls.MrngRadioButton radCredentialsNoInfo;
        internal Controls.MrngRadioButton radCredentialsWindows;
        internal Controls.MrngTextBox txtCredentialsDomain;
        internal Controls.MrngLabel lblCredentialsUserViaAPI;
        internal Controls.MrngLabel lblCredentialsUsername;
        internal Controls.MrngTextBox txtCredentialsPassword;
        internal Controls.MrngLabel lblCredentialsPassword;
        internal Controls.MrngTextBox txtCredentialsUserViaAPI;
        internal Controls.MrngTextBox txtCredentialsUsername;
        internal Controls.MrngLabel lblCredentialsDomain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
