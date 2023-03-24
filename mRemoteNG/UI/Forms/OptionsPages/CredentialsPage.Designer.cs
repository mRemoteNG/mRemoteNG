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
            pnlDefaultCredentials = new System.Windows.Forms.Panel();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            txtCredentialsUserViaAPI = new Controls.MrngTextBox();
            txtCredentialsUsername = new Controls.MrngTextBox();
            txtCredentialsPassword = new Controls.MrngTextBox();
            txtCredentialsDomain = new Controls.MrngTextBox();
            lblCredentialsUserViaAPI = new Controls.MrngLabel();
            lblCredentialsUsername = new Controls.MrngLabel();
            lblCredentialsPassword = new Controls.MrngLabel();
            lblCredentialsDomain = new Controls.MrngLabel();
            radCredentialsCustom = new Controls.MrngRadioButton();
            lblDefaultCredentials = new Controls.MrngLabel();
            radCredentialsNoInfo = new Controls.MrngRadioButton();
            radCredentialsWindows = new Controls.MrngRadioButton();
            pnlDefaultCredentials.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // pnlDefaultCredentials
            // 
            pnlDefaultCredentials.Controls.Add(tableLayoutPanel1);
            pnlDefaultCredentials.Controls.Add(radCredentialsCustom);
            pnlDefaultCredentials.Controls.Add(lblDefaultCredentials);
            pnlDefaultCredentials.Controls.Add(radCredentialsNoInfo);
            pnlDefaultCredentials.Controls.Add(radCredentialsWindows);
            pnlDefaultCredentials.Location = new System.Drawing.Point(3, 3);
            pnlDefaultCredentials.Name = "pnlDefaultCredentials";
            pnlDefaultCredentials.Size = new System.Drawing.Size(604, 200);
            pnlDefaultCredentials.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(txtCredentialsUserViaAPI, 1, 0);
            tableLayoutPanel1.Controls.Add(txtCredentialsUsername, 1, 1);
            tableLayoutPanel1.Controls.Add(txtCredentialsPassword, 1, 2);
            tableLayoutPanel1.Controls.Add(txtCredentialsDomain, 1, 3);
            tableLayoutPanel1.Controls.Add(lblCredentialsUserViaAPI, 0, 0);
            tableLayoutPanel1.Controls.Add(lblCredentialsUsername, 0, 1);
            tableLayoutPanel1.Controls.Add(lblCredentialsPassword, 0, 2);
            tableLayoutPanel1.Controls.Add(lblCredentialsDomain, 0, 3);
            tableLayoutPanel1.Location = new System.Drawing.Point(23, 85);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new System.Drawing.Size(332, 110);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // txtCredentialsUserViaAPI
            // 
            txtCredentialsUserViaAPI.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtCredentialsUserViaAPI.Dock = System.Windows.Forms.DockStyle.Fill;
            txtCredentialsUserViaAPI.Enabled = false;
            txtCredentialsUserViaAPI.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            txtCredentialsUserViaAPI.Location = new System.Drawing.Point(163, 3);
            txtCredentialsUserViaAPI.Name = "txtCredentialsUserViaAPI";
            txtCredentialsUserViaAPI.Size = new System.Drawing.Size(166, 22);
            txtCredentialsUserViaAPI.TabIndex = 5;
            // 
            // txtCredentialsUsername
            // 
            txtCredentialsUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtCredentialsUsername.Dock = System.Windows.Forms.DockStyle.Fill;
            txtCredentialsUsername.Enabled = false;
            txtCredentialsUsername.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            txtCredentialsUsername.Location = new System.Drawing.Point(163, 29);
            txtCredentialsUsername.Name = "txtCredentialsUsername";
            txtCredentialsUsername.Size = new System.Drawing.Size(166, 22);
            txtCredentialsUsername.TabIndex = 5;
            // 
            // txtCredentialsPassword
            // 
            txtCredentialsPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtCredentialsPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            txtCredentialsPassword.Enabled = false;
            txtCredentialsPassword.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            txtCredentialsPassword.Location = new System.Drawing.Point(163, 55);
            txtCredentialsPassword.Name = "txtCredentialsPassword";
            txtCredentialsPassword.Size = new System.Drawing.Size(166, 22);
            txtCredentialsPassword.TabIndex = 7;
            txtCredentialsPassword.UseSystemPasswordChar = true;
            // 
            // txtCredentialsDomain
            // 
            txtCredentialsDomain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtCredentialsDomain.Dock = System.Windows.Forms.DockStyle.Fill;
            txtCredentialsDomain.Enabled = false;
            txtCredentialsDomain.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            txtCredentialsDomain.Location = new System.Drawing.Point(163, 81);
            txtCredentialsDomain.Name = "txtCredentialsDomain";
            txtCredentialsDomain.Size = new System.Drawing.Size(166, 22);
            txtCredentialsDomain.TabIndex = 9;
            // 
            // lblCredentialsUserViaAPI
            // 
            lblCredentialsUserViaAPI.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lblCredentialsUserViaAPI.Enabled = false;
            lblCredentialsUserViaAPI.Location = new System.Drawing.Point(3, 3);
            lblCredentialsUserViaAPI.Name = "lblCredentialsUserViaAPI";
            lblCredentialsUserViaAPI.Size = new System.Drawing.Size(154, 19);
            lblCredentialsUserViaAPI.TabIndex = 4;
            lblCredentialsUserViaAPI.Text = "User via API ID:";
            lblCredentialsUserViaAPI.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblCredentialsUsername
            // 
            lblCredentialsUsername.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lblCredentialsUsername.Enabled = false;
            lblCredentialsUsername.Location = new System.Drawing.Point(3, 29);
            lblCredentialsUsername.Name = "lblCredentialsUsername";
            lblCredentialsUsername.Size = new System.Drawing.Size(154, 19);
            lblCredentialsUsername.TabIndex = 4;
            lblCredentialsUsername.Text = "Username:";
            lblCredentialsUsername.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblCredentialsPassword
            // 
            lblCredentialsPassword.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lblCredentialsPassword.Enabled = false;
            lblCredentialsPassword.Location = new System.Drawing.Point(3, 55);
            lblCredentialsPassword.Name = "lblCredentialsPassword";
            lblCredentialsPassword.Size = new System.Drawing.Size(154, 19);
            lblCredentialsPassword.TabIndex = 6;
            lblCredentialsPassword.Text = "Password:";
            lblCredentialsPassword.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblCredentialsDomain
            // 
            lblCredentialsDomain.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lblCredentialsDomain.Enabled = false;
            lblCredentialsDomain.Location = new System.Drawing.Point(3, 84);
            lblCredentialsDomain.Name = "lblCredentialsDomain";
            lblCredentialsDomain.Size = new System.Drawing.Size(154, 19);
            lblCredentialsDomain.TabIndex = 8;
            lblCredentialsDomain.Text = "Domain:";
            lblCredentialsDomain.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // radCredentialsCustom
            // 
            radCredentialsCustom.AutoSize = true;
            radCredentialsCustom.BackColor = System.Drawing.Color.Transparent;
            radCredentialsCustom.Location = new System.Drawing.Point(3, 62);
            radCredentialsCustom.Name = "radCredentialsCustom";
            radCredentialsCustom.Size = new System.Drawing.Size(98, 17);
            radCredentialsCustom.TabIndex = 3;
            radCredentialsCustom.Text = "the following:";
            radCredentialsCustom.UseVisualStyleBackColor = false;
            radCredentialsCustom.CheckedChanged += radCredentialsCustom_CheckedChanged;
            // 
            // lblDefaultCredentials
            // 
            lblDefaultCredentials.AutoSize = true;
            lblDefaultCredentials.Location = new System.Drawing.Point(0, 0);
            lblDefaultCredentials.Name = "lblDefaultCredentials";
            lblDefaultCredentials.Size = new System.Drawing.Size(279, 13);
            lblDefaultCredentials.TabIndex = 0;
            lblDefaultCredentials.Text = "For empty Username, Password or Domain fields use:";
            // 
            // radCredentialsNoInfo
            // 
            radCredentialsNoInfo.AutoSize = true;
            radCredentialsNoInfo.BackColor = System.Drawing.Color.Transparent;
            radCredentialsNoInfo.Checked = true;
            radCredentialsNoInfo.Location = new System.Drawing.Point(3, 16);
            radCredentialsNoInfo.Name = "radCredentialsNoInfo";
            radCredentialsNoInfo.Size = new System.Drawing.Size(103, 17);
            radCredentialsNoInfo.TabIndex = 1;
            radCredentialsNoInfo.TabStop = true;
            radCredentialsNoInfo.Text = "no information";
            radCredentialsNoInfo.UseVisualStyleBackColor = false;
            // 
            // radCredentialsWindows
            // 
            radCredentialsWindows.AutoSize = true;
            radCredentialsWindows.BackColor = System.Drawing.Color.Transparent;
            radCredentialsWindows.Location = new System.Drawing.Point(3, 39);
            radCredentialsWindows.Name = "radCredentialsWindows";
            radCredentialsWindows.Size = new System.Drawing.Size(252, 17);
            radCredentialsWindows.TabIndex = 2;
            radCredentialsWindows.Text = "my current credentials (windows logon info)";
            radCredentialsWindows.UseVisualStyleBackColor = false;
            // 
            // CredentialsPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            Controls.Add(pnlDefaultCredentials);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "CredentialsPage";
            Size = new System.Drawing.Size(610, 490);
            pnlDefaultCredentials.ResumeLayout(false);
            pnlDefaultCredentials.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
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
