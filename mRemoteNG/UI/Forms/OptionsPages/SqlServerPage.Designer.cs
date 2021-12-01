

using mRemoteNG.UI.Controls;

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
            this.lblSQLDatabaseName = new mRemoteNG.UI.Controls.MrngLabel();
            this.txtSQLDatabaseName = new mRemoteNG.UI.Controls.MrngTextBox();
            this.lblExperimental = new mRemoteNG.UI.Controls.MrngLabel();
            this.chkUseSQLServer = new MrngCheckBox();
            this.lblSQLUsername = new mRemoteNG.UI.Controls.MrngLabel();
            this.txtSQLPassword = new mRemoteNG.UI.Controls.MrngTextBox();
            this.lblSQLInfo = new mRemoteNG.UI.Controls.MrngLabel();
            this.lblSQLServer = new mRemoteNG.UI.Controls.MrngLabel();
            this.txtSQLUsername = new mRemoteNG.UI.Controls.MrngTextBox();
            this.txtSQLServer = new mRemoteNG.UI.Controls.MrngTextBox();
            this.lblSQLPassword = new mRemoteNG.UI.Controls.MrngLabel();
            this.btnTestConnection = new MrngButton();
            this.imgConnectionStatus = new System.Windows.Forms.PictureBox();
            this.lblTestConnectionResults = new mRemoteNG.UI.Controls.MrngLabel();
            this.chkSQLReadOnly = new MrngCheckBox();
            this.lblSQLReadOnly = new mRemoteNG.UI.Controls.MrngLabel();
            this.lblSQLType = new mRemoteNG.UI.Controls.MrngLabel();
            this.txtSQLType = new MrngComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.imgConnectionStatus)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSQLDatabaseName
            // 
            this.lblSQLDatabaseName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSQLDatabaseName.Enabled = false;
            this.lblSQLDatabaseName.Location = new System.Drawing.Point(3, 52);
            this.lblSQLDatabaseName.Name = "lblSQLDatabaseName";
            this.lblSQLDatabaseName.Size = new System.Drawing.Size(154, 26);
            this.lblSQLDatabaseName.TabIndex = 5;
            this.lblSQLDatabaseName.Text = "Database:";
            this.lblSQLDatabaseName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSQLDatabaseName
            // 
            this.txtSQLDatabaseName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSQLDatabaseName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSQLDatabaseName.Enabled = false;
            this.txtSQLDatabaseName.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSQLDatabaseName.Location = new System.Drawing.Point(163, 55);
            this.txtSQLDatabaseName.Name = "txtSQLDatabaseName";
            this.txtSQLDatabaseName.Size = new System.Drawing.Size(235, 22);
            this.txtSQLDatabaseName.TabIndex = 6;
            // 
            // lblExperimental
            // 
            this.lblExperimental.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblExperimental.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblExperimental.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblExperimental.Location = new System.Drawing.Point(3, 0);
            this.lblExperimental.Name = "lblExperimental";
            this.lblExperimental.Size = new System.Drawing.Size(596, 26);
            this.lblExperimental.TabIndex = 0;
            this.lblExperimental.Text = "EXPERIMENTAL";
            this.lblExperimental.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkUseSQLServer
            // 
            this.chkUseSQLServer._mice = MrngCheckBox.MouseState.OUT;
            this.chkUseSQLServer.AutoSize = true;
            this.chkUseSQLServer.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkUseSQLServer.Location = new System.Drawing.Point(3, 76);
            this.chkUseSQLServer.Name = "chkUseSQLServer";
            this.chkUseSQLServer.Size = new System.Drawing.Size(244, 17);
            this.chkUseSQLServer.TabIndex = 2;
            this.chkUseSQLServer.Text = "Use SQL Server to load && save connections";
            this.chkUseSQLServer.UseVisualStyleBackColor = true;
            this.chkUseSQLServer.CheckedChanged += new System.EventHandler(this.chkUseSQLServer_CheckedChanged);
            // 
            // lblSQLUsername
            // 
            this.lblSQLUsername.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSQLUsername.Enabled = false;
            this.lblSQLUsername.Location = new System.Drawing.Point(3, 78);
            this.lblSQLUsername.Name = "lblSQLUsername";
            this.lblSQLUsername.Size = new System.Drawing.Size(154, 26);
            this.lblSQLUsername.TabIndex = 7;
            this.lblSQLUsername.Text = "Username:";
            this.lblSQLUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSQLPassword
            // 
            this.txtSQLPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSQLPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSQLPassword.Enabled = false;
            this.txtSQLPassword.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSQLPassword.Location = new System.Drawing.Point(163, 107);
            this.txtSQLPassword.Name = "txtSQLPassword";
            this.txtSQLPassword.Size = new System.Drawing.Size(235, 22);
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
            this.lblSQLInfo.Location = new System.Drawing.Point(3, 25);
            this.lblSQLInfo.Name = "lblSQLInfo";
            this.lblSQLInfo.Size = new System.Drawing.Size(596, 26);
            this.lblSQLInfo.TabIndex = 1;
            this.lblSQLInfo.Text = "Please see Help - Getting started - SQL Configuration for more Info!";
            this.lblSQLInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSQLServer
            // 
            this.lblSQLServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSQLServer.Enabled = false;
            this.lblSQLServer.Location = new System.Drawing.Point(3, 26);
            this.lblSQLServer.Name = "lblSQLServer";
            this.lblSQLServer.Size = new System.Drawing.Size(154, 26);
            this.lblSQLServer.TabIndex = 3;
            this.lblSQLServer.Text = "SQL Server:";
            this.lblSQLServer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSQLUsername
            // 
            this.txtSQLUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSQLUsername.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSQLUsername.Enabled = false;
            this.txtSQLUsername.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSQLUsername.Location = new System.Drawing.Point(163, 81);
            this.txtSQLUsername.Name = "txtSQLUsername";
            this.txtSQLUsername.Size = new System.Drawing.Size(235, 22);
            this.txtSQLUsername.TabIndex = 8;
            // 
            // txtSQLServer
            // 
            this.txtSQLServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSQLServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSQLServer.Enabled = false;
            this.txtSQLServer.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSQLServer.Location = new System.Drawing.Point(163, 29);
            this.txtSQLServer.Name = "txtSQLServer";
            this.txtSQLServer.Size = new System.Drawing.Size(235, 22);
            this.txtSQLServer.TabIndex = 4;
            // 
            // lblSQLPassword
            // 
            this.lblSQLPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSQLPassword.Enabled = false;
            this.lblSQLPassword.Location = new System.Drawing.Point(3, 104);
            this.lblSQLPassword.Name = "lblSQLPassword";
            this.lblSQLPassword.Size = new System.Drawing.Size(154, 26);
            this.lblSQLPassword.TabIndex = 9;
            this.lblSQLPassword.Text = "Password:";
            this.lblSQLPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnTestConnection
            // 
            this.btnTestConnection._mice = MrngButton.MouseState.OUT;
            this.btnTestConnection.Enabled = false;
            this.btnTestConnection.Location = new System.Drawing.Point(3, 267);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(153, 25);
            this.btnTestConnection.TabIndex = 11;
            this.btnTestConnection.Text = "Test Connection";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // imgConnectionStatus
            // 
            this.imgConnectionStatus.Image = global::mRemoteNG.Properties.Resources.F1Help_16x;
            this.imgConnectionStatus.Location = new System.Drawing.Point(162, 272);
            this.imgConnectionStatus.Name = "imgConnectionStatus";
            this.imgConnectionStatus.Size = new System.Drawing.Size(16, 16);
            this.imgConnectionStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.imgConnectionStatus.TabIndex = 12;
            this.imgConnectionStatus.TabStop = false;
            // 
            // lblTestConnectionResults
            // 
            this.lblTestConnectionResults.AutoSize = true;
            this.lblTestConnectionResults.Location = new System.Drawing.Point(3, 295);
            this.lblTestConnectionResults.Name = "lblTestConnectionResults";
            this.lblTestConnectionResults.Size = new System.Drawing.Size(125, 13);
            this.lblTestConnectionResults.TabIndex = 13;
            this.lblTestConnectionResults.Text = "Test connection details";
            // 
            // chkSQLReadOnly
            // 
            this.chkSQLReadOnly._mice = MrngCheckBox.MouseState.OUT;
            this.chkSQLReadOnly.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.chkSQLReadOnly.AutoSize = true;
            this.chkSQLReadOnly.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSQLReadOnly.Location = new System.Drawing.Point(163, 133);
            this.chkSQLReadOnly.Name = "chkSQLReadOnly";
            this.chkSQLReadOnly.Size = new System.Drawing.Size(15, 20);
            this.chkSQLReadOnly.TabIndex = 14;
            this.chkSQLReadOnly.UseVisualStyleBackColor = true;
            // 
            // lblSQLReadOnly
            // 
            this.lblSQLReadOnly.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSQLReadOnly.Enabled = false;
            this.lblSQLReadOnly.Location = new System.Drawing.Point(3, 130);
            this.lblSQLReadOnly.Name = "lblSQLReadOnly";
            this.lblSQLReadOnly.Size = new System.Drawing.Size(154, 26);
            this.lblSQLReadOnly.TabIndex = 15;
            this.lblSQLReadOnly.Text = "Read Only:";
            this.lblSQLReadOnly.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSQLType
            // 
            this.lblSQLType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSQLType.Enabled = false;
            this.lblSQLType.Location = new System.Drawing.Point(3, 0);
            this.lblSQLType.Name = "lblSQLType";
            this.lblSQLType.Size = new System.Drawing.Size(154, 26);
            this.lblSQLType.TabIndex = 20;
            this.lblSQLType.Text = "SQL Server Type:";
            this.lblSQLType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSQLType
            // 
            this.txtSQLType._mice = MrngComboBox.MouseState.HOVER;
            this.txtSQLType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSQLType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtSQLType.FormattingEnabled = true;
            this.txtSQLType.Items.AddRange(new object[] {
            "mssql",
            "mysql"});
            this.txtSQLType.Location = new System.Drawing.Point(163, 3);
            this.txtSQLType.Name = "txtSQLType";
            this.txtSQLType.Size = new System.Drawing.Size(235, 21);
            this.txtSQLType.TabIndex = 21;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lblSQLType, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtSQLType, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblSQLServer, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkSQLReadOnly, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblSQLReadOnly, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblSQLDatabaseName, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtSQLDatabaseName, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblSQLUsername, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblSQLPassword, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtSQLServer, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtSQLPassword, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtSQLUsername, 1, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 99);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(401, 162);
            this.tableLayoutPanel1.TabIndex = 22;
            // 
            // SqlServerPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.lblTestConnectionResults);
            this.Controls.Add(this.imgConnectionStatus);
            this.Controls.Add(this.btnTestConnection);
            this.Controls.Add(this.lblExperimental);
            this.Controls.Add(this.chkUseSQLServer);
            this.Controls.Add(this.lblSQLInfo);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SqlServerPage";
            this.Size = new System.Drawing.Size(610, 490);
            ((System.ComponentModel.ISupportInitialize)(this.imgConnectionStatus)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		internal Controls.MrngLabel lblSQLDatabaseName;
		internal Controls.MrngTextBox txtSQLDatabaseName;
		internal Controls.MrngLabel lblExperimental;
		internal MrngCheckBox chkUseSQLServer;
		internal Controls.MrngLabel lblSQLUsername;
		internal Controls.MrngTextBox txtSQLPassword;
		internal Controls.MrngLabel lblSQLInfo;
		internal Controls.MrngLabel lblSQLServer;
		internal Controls.MrngTextBox txtSQLUsername;
		internal Controls.MrngTextBox txtSQLServer;
		internal Controls.MrngLabel lblSQLPassword;
        private MrngButton btnTestConnection;
        private System.Windows.Forms.PictureBox imgConnectionStatus;
        private Controls.MrngLabel lblTestConnectionResults;
        private MrngCheckBox chkSQLReadOnly;
        internal Controls.MrngLabel lblSQLReadOnly;
		internal Controls.MrngLabel lblSQLType;
		private MrngComboBox txtSQLType;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
