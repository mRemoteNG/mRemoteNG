

namespace mRemoteNG.UI.Forms.OptionsPages
{
	
    public sealed partial class SqlServerPage : OptionsPage
	{
			
		//UserControl overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]protected override void Dispose(bool disposing)
		{
				base.Dispose(disposing);
		}
			
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
		{
            this.lblSQLDatabaseName = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.txtSQLDatabaseName = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.lblExperimental = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.chkUseSQLServer = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.lblSQLUsername = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.txtSQLPassword = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.lblSQLInfo = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.lblSQLServer = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.txtSQLUsername = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.txtSQLServer = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.lblSQLPassword = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.btnTestConnection = new mRemoteNG.UI.Controls.Base.NGButton();
            this.imgConnectionStatus = new System.Windows.Forms.PictureBox();
            this.lblTestConnectionResults = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.chkSQLReadOnly = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.lblSQLReadOnly = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.lblSQLType = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.txtSQLType = new mRemoteNG.UI.Controls.Base.NGComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.imgConnectionStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSQLDatabaseName
            // 
            this.lblSQLDatabaseName.Enabled = false;
            this.lblSQLDatabaseName.Location = new System.Drawing.Point(34, 231);
            this.lblSQLDatabaseName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSQLDatabaseName.Name = "lblSQLDatabaseName";
            this.lblSQLDatabaseName.Size = new System.Drawing.Size(166, 29);
            this.lblSQLDatabaseName.TabIndex = 5;
            this.lblSQLDatabaseName.Text = "Database:";
            this.lblSQLDatabaseName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtSQLDatabaseName
            // 
            this.txtSQLDatabaseName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSQLDatabaseName.Enabled = false;
            this.txtSQLDatabaseName.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSQLDatabaseName.Location = new System.Drawing.Point(210, 231);
            this.txtSQLDatabaseName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSQLDatabaseName.Name = "txtSQLDatabaseName";
            this.txtSQLDatabaseName.Size = new System.Drawing.Size(228, 29);
            this.txtSQLDatabaseName.TabIndex = 6;
            // 
            // lblExperimental
            // 
            this.lblExperimental.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblExperimental.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblExperimental.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblExperimental.Location = new System.Drawing.Point(4, 0);
            this.lblExperimental.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblExperimental.Name = "lblExperimental";
            this.lblExperimental.Size = new System.Drawing.Size(894, 39);
            this.lblExperimental.TabIndex = 0;
            this.lblExperimental.Text = "EXPERIMENTAL";
            this.lblExperimental.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkUseSQLServer
            // 
            this.chkUseSQLServer._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.OUT;
            this.chkUseSQLServer.AutoSize = true;
            this.chkUseSQLServer.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkUseSQLServer.Location = new System.Drawing.Point(4, 114);
            this.chkUseSQLServer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkUseSQLServer.Name = "chkUseSQLServer";
            this.chkUseSQLServer.Size = new System.Drawing.Size(364, 27);
            this.chkUseSQLServer.TabIndex = 2;
            this.chkUseSQLServer.Text = "Use SQL Server to load && save connections";
            this.chkUseSQLServer.UseVisualStyleBackColor = true;
            this.chkUseSQLServer.CheckedChanged += new System.EventHandler(this.chkUseSQLServer_CheckedChanged);
            // 
            // lblSQLUsername
            // 
            this.lblSQLUsername.Enabled = false;
            this.lblSQLUsername.Location = new System.Drawing.Point(34, 270);
            this.lblSQLUsername.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSQLUsername.Name = "lblSQLUsername";
            this.lblSQLUsername.Size = new System.Drawing.Size(166, 29);
            this.lblSQLUsername.TabIndex = 7;
            this.lblSQLUsername.Text = "Username:";
            this.lblSQLUsername.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtSQLPassword
            // 
            this.txtSQLPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSQLPassword.Enabled = false;
            this.txtSQLPassword.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSQLPassword.Location = new System.Drawing.Point(210, 309);
            this.txtSQLPassword.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSQLPassword.Name = "txtSQLPassword";
            this.txtSQLPassword.Size = new System.Drawing.Size(228, 29);
            this.txtSQLPassword.TabIndex = 10;
            this.txtSQLPassword.UseSystemPasswordChar = true;
            // 
            // lblSQLInfo
            // 
            this.lblSQLInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSQLInfo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World);
            this.lblSQLInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblSQLInfo.Location = new System.Drawing.Point(4, 38);
            this.lblSQLInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSQLInfo.Name = "lblSQLInfo";
            this.lblSQLInfo.Size = new System.Drawing.Size(894, 39);
            this.lblSQLInfo.TabIndex = 1;
            this.lblSQLInfo.Text = "Please see Help - Getting started - SQL Configuration for more Info!";
            this.lblSQLInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSQLServer
            // 
            this.lblSQLServer.Enabled = false;
            this.lblSQLServer.Location = new System.Drawing.Point(34, 192);
            this.lblSQLServer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSQLServer.Name = "lblSQLServer";
            this.lblSQLServer.Size = new System.Drawing.Size(166, 29);
            this.lblSQLServer.TabIndex = 3;
            this.lblSQLServer.Text = "SQL Server:";
            this.lblSQLServer.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtSQLUsername
            // 
            this.txtSQLUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSQLUsername.Enabled = false;
            this.txtSQLUsername.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSQLUsername.Location = new System.Drawing.Point(210, 270);
            this.txtSQLUsername.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSQLUsername.Name = "txtSQLUsername";
            this.txtSQLUsername.Size = new System.Drawing.Size(228, 29);
            this.txtSQLUsername.TabIndex = 8;
            // 
            // txtSQLServer
            // 
            this.txtSQLServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSQLServer.Enabled = false;
            this.txtSQLServer.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSQLServer.Location = new System.Drawing.Point(210, 192);
            this.txtSQLServer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSQLServer.Name = "txtSQLServer";
            this.txtSQLServer.Size = new System.Drawing.Size(228, 29);
            this.txtSQLServer.TabIndex = 4;
            // 
            // lblSQLPassword
            // 
            this.lblSQLPassword.Enabled = false;
            this.lblSQLPassword.Location = new System.Drawing.Point(34, 309);
            this.lblSQLPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSQLPassword.Name = "lblSQLPassword";
            this.lblSQLPassword.Size = new System.Drawing.Size(166, 29);
            this.lblSQLPassword.TabIndex = 9;
            this.lblSQLPassword.Text = "Password:";
            this.lblSQLPassword.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnTestConnection
            // 
            this.btnTestConnection._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.OUT;
            this.btnTestConnection.Enabled = false;
            this.btnTestConnection.Location = new System.Drawing.Point(210, 378);
            this.btnTestConnection.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(230, 34);
            this.btnTestConnection.TabIndex = 11;
            this.btnTestConnection.Text = "Test Connection";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // imgConnectionStatus
            // 
            this.imgConnectionStatus.Image = global::mRemoteNG.Resources.Help;
            this.imgConnectionStatus.Location = new System.Drawing.Point(446, 382);
            this.imgConnectionStatus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.imgConnectionStatus.Name = "imgConnectionStatus";
            this.imgConnectionStatus.Size = new System.Drawing.Size(16, 16);
            this.imgConnectionStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.imgConnectionStatus.TabIndex = 12;
            this.imgConnectionStatus.TabStop = false;
            // 
            // lblTestConnectionResults
            // 
            this.lblTestConnectionResults.AutoSize = true;
            this.lblTestConnectionResults.Location = new System.Drawing.Point(206, 417);
            this.lblTestConnectionResults.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTestConnectionResults.Name = "lblTestConnectionResults";
            this.lblTestConnectionResults.Size = new System.Drawing.Size(183, 23);
            this.lblTestConnectionResults.TabIndex = 13;
            this.lblTestConnectionResults.Text = "Test connection details";
            // 
            // chkSQLReadOnly
            // 
            this.chkSQLReadOnly._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.OUT;
            this.chkSQLReadOnly.AutoSize = true;
            this.chkSQLReadOnly.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSQLReadOnly.Location = new System.Drawing.Point(210, 348);
            this.chkSQLReadOnly.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkSQLReadOnly.Name = "chkSQLReadOnly";
            this.chkSQLReadOnly.Size = new System.Drawing.Size(22, 21);
            this.chkSQLReadOnly.TabIndex = 14;
            this.chkSQLReadOnly.UseVisualStyleBackColor = true;
            // 
            // lblSQLReadOnly
            // 
            this.lblSQLReadOnly.Enabled = false;
            this.lblSQLReadOnly.Location = new System.Drawing.Point(34, 348);
            this.lblSQLReadOnly.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSQLReadOnly.Name = "lblSQLReadOnly";
            this.lblSQLReadOnly.Size = new System.Drawing.Size(166, 21);
            this.lblSQLReadOnly.TabIndex = 15;
            this.lblSQLReadOnly.Text = "Read Only:";
            this.lblSQLReadOnly.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblSQLType
            // 
            this.lblSQLType.Enabled = false;
            this.lblSQLType.Location = new System.Drawing.Point(34, 153);
            this.lblSQLType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSQLType.Name = "lblSQLType";
            this.lblSQLType.Size = new System.Drawing.Size(166, 31);
            this.lblSQLType.TabIndex = 20;
            this.lblSQLType.Text = "SQL Server Type:";
            this.lblSQLType.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtSQLType
            // 
            this.txtSQLType._mice = mRemoteNG.UI.Controls.Base.NGComboBox.MouseState.HOVER;
            this.txtSQLType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtSQLType.FormattingEnabled = true;
            this.txtSQLType.Items.AddRange(new object[] {
            "mssql",
            "mysql"});
            this.txtSQLType.Location = new System.Drawing.Point(210, 153);
            this.txtSQLType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSQLType.Name = "txtSQLType";
            this.txtSQLType.Size = new System.Drawing.Size(228, 31);
            this.txtSQLType.TabIndex = 21;
            // 
            // SqlServerPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.txtSQLType);
            this.Controls.Add(this.lblSQLType);
            this.Controls.Add(this.lblSQLReadOnly);
            this.Controls.Add(this.chkSQLReadOnly);
            this.Controls.Add(this.lblTestConnectionResults);
            this.Controls.Add(this.imgConnectionStatus);
            this.Controls.Add(this.btnTestConnection);
            this.Controls.Add(this.lblSQLDatabaseName);
            this.Controls.Add(this.txtSQLDatabaseName);
            this.Controls.Add(this.lblExperimental);
            this.Controls.Add(this.chkUseSQLServer);
            this.Controls.Add(this.lblSQLUsername);
            this.Controls.Add(this.txtSQLPassword);
            this.Controls.Add(this.lblSQLInfo);
            this.Controls.Add(this.lblSQLServer);
            this.Controls.Add(this.txtSQLUsername);
            this.Controls.Add(this.txtSQLServer);
            this.Controls.Add(this.lblSQLPassword);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "SqlServerPage";
            this.Size = new System.Drawing.Size(915, 735);
            ((System.ComponentModel.ISupportInitialize)(this.imgConnectionStatus)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		internal Controls.Base.NGLabel lblSQLDatabaseName;
		internal Controls.Base.NGTextBox txtSQLDatabaseName;
		internal Controls.Base.NGLabel lblExperimental;
		internal Controls.Base.NGCheckBox chkUseSQLServer;
		internal Controls.Base.NGLabel lblSQLUsername;
		internal Controls.Base.NGTextBox txtSQLPassword;
		internal Controls.Base.NGLabel lblSQLInfo;
		internal Controls.Base.NGLabel lblSQLServer;
		internal Controls.Base.NGTextBox txtSQLUsername;
		internal Controls.Base.NGTextBox txtSQLServer;
		internal Controls.Base.NGLabel lblSQLPassword;
        private Controls.Base.NGButton btnTestConnection;
        private System.Windows.Forms.PictureBox imgConnectionStatus;
        private Controls.Base.NGLabel lblTestConnectionResults;
        private Controls.Base.NGCheckBox chkSQLReadOnly;
        internal Controls.Base.NGLabel lblSQLReadOnly;
		internal Controls.Base.NGLabel lblSQLType;
		private Controls.Base.NGComboBox txtSQLType;
    }
}
