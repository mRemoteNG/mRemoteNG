

using mRemoteNG.UI.Controls;

namespace mRemoteNG.UI.Forms.OptionsPages
{

    public sealed partial class SqlServerPage : OptionsPage
    {

        //UserControl overrides dispose to clean up the component list.
        [System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        //NOTE: The following procedure is required by the Windows Form Designer
        //It can be modified using the Windows Form Designer.
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            lblSQLDatabaseName = new MrngLabel();
            txtSQLDatabaseName = new MrngTextBox();
            chkUseSQLServer = new MrngCheckBox();
            lblSQLUsername = new MrngLabel();
            txtSQLPassword = new MrngTextBox();
            lblSQLServer = new MrngLabel();
            txtSQLUsername = new MrngTextBox();
            txtSQLServer = new MrngTextBox();
            lblSQLPassword = new MrngLabel();
            btnTestConnection = new MrngButton();
            imgConnectionStatus = new System.Windows.Forms.PictureBox();
            lblTestConnectionResults = new MrngLabel();
            chkSQLReadOnly = new MrngCheckBox();
            lblSQLReadOnly = new MrngLabel();
            lblSQLType = new MrngLabel();
            txtSQLType = new MrngComboBox();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)imgConnectionStatus).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // lblSQLDatabaseName
            // 
            lblSQLDatabaseName.Dock = System.Windows.Forms.DockStyle.Fill;
            lblSQLDatabaseName.Enabled = false;
            lblSQLDatabaseName.Location = new System.Drawing.Point(3, 52);
            lblSQLDatabaseName.Name = "lblSQLDatabaseName";
            lblSQLDatabaseName.Size = new System.Drawing.Size(154, 26);
            lblSQLDatabaseName.TabIndex = 5;
            lblSQLDatabaseName.Text = "Database:";
            lblSQLDatabaseName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSQLDatabaseName
            // 
            txtSQLDatabaseName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtSQLDatabaseName.Dock = System.Windows.Forms.DockStyle.Fill;
            txtSQLDatabaseName.Enabled = false;
            txtSQLDatabaseName.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            txtSQLDatabaseName.Location = new System.Drawing.Point(163, 55);
            txtSQLDatabaseName.Name = "txtSQLDatabaseName";
            txtSQLDatabaseName.Size = new System.Drawing.Size(235, 22);
            txtSQLDatabaseName.TabIndex = 6;
            // 
            // chkUseSQLServer
            // 
            chkUseSQLServer._mice = MrngCheckBox.MouseState.OUT;
            chkUseSQLServer.AutoSize = true;
            chkUseSQLServer.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkUseSQLServer.Location = new System.Drawing.Point(9, 7);
            chkUseSQLServer.Name = "chkUseSQLServer";
            chkUseSQLServer.Size = new System.Drawing.Size(244, 17);
            chkUseSQLServer.TabIndex = 2;
            chkUseSQLServer.Text = "Use SQL Server to load && save connections";
            chkUseSQLServer.UseVisualStyleBackColor = true;
            chkUseSQLServer.CheckedChanged += chkUseSQLServer_CheckedChanged;
            // 
            // lblSQLUsername
            // 
            lblSQLUsername.Dock = System.Windows.Forms.DockStyle.Fill;
            lblSQLUsername.Enabled = false;
            lblSQLUsername.Location = new System.Drawing.Point(3, 78);
            lblSQLUsername.Name = "lblSQLUsername";
            lblSQLUsername.Size = new System.Drawing.Size(154, 26);
            lblSQLUsername.TabIndex = 7;
            lblSQLUsername.Text = "Username:";
            lblSQLUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSQLPassword
            // 
            txtSQLPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtSQLPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            txtSQLPassword.Enabled = false;
            txtSQLPassword.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            txtSQLPassword.Location = new System.Drawing.Point(163, 107);
            txtSQLPassword.Name = "txtSQLPassword";
            txtSQLPassword.Size = new System.Drawing.Size(235, 22);
            txtSQLPassword.TabIndex = 10;
            txtSQLPassword.UseSystemPasswordChar = true;
            // 
            // lblSQLServer
            // 
            lblSQLServer.Dock = System.Windows.Forms.DockStyle.Fill;
            lblSQLServer.Enabled = false;
            lblSQLServer.Location = new System.Drawing.Point(3, 26);
            lblSQLServer.Name = "lblSQLServer";
            lblSQLServer.Size = new System.Drawing.Size(154, 26);
            lblSQLServer.TabIndex = 3;
            lblSQLServer.Text = "SQL Server:";
            lblSQLServer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSQLUsername
            // 
            txtSQLUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtSQLUsername.Dock = System.Windows.Forms.DockStyle.Fill;
            txtSQLUsername.Enabled = false;
            txtSQLUsername.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            txtSQLUsername.Location = new System.Drawing.Point(163, 81);
            txtSQLUsername.Name = "txtSQLUsername";
            txtSQLUsername.Size = new System.Drawing.Size(235, 22);
            txtSQLUsername.TabIndex = 8;
            // 
            // txtSQLServer
            // 
            txtSQLServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtSQLServer.Dock = System.Windows.Forms.DockStyle.Fill;
            txtSQLServer.Enabled = false;
            txtSQLServer.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            txtSQLServer.Location = new System.Drawing.Point(163, 29);
            txtSQLServer.Name = "txtSQLServer";
            txtSQLServer.Size = new System.Drawing.Size(235, 22);
            txtSQLServer.TabIndex = 4;
            // 
            // lblSQLPassword
            // 
            lblSQLPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            lblSQLPassword.Enabled = false;
            lblSQLPassword.Location = new System.Drawing.Point(3, 104);
            lblSQLPassword.Name = "lblSQLPassword";
            lblSQLPassword.Size = new System.Drawing.Size(154, 26);
            lblSQLPassword.TabIndex = 9;
            lblSQLPassword.Text = "Password:";
            lblSQLPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnTestConnection
            // 
            btnTestConnection._mice = MrngButton.MouseState.OUT;
            btnTestConnection.Enabled = false;
            btnTestConnection.Location = new System.Drawing.Point(9, 198);
            btnTestConnection.Name = "btnTestConnection";
            btnTestConnection.Size = new System.Drawing.Size(153, 25);
            btnTestConnection.TabIndex = 11;
            btnTestConnection.Text = "Test Connection";
            btnTestConnection.UseVisualStyleBackColor = true;
            btnTestConnection.Click += btnTestConnection_Click;
            // 
            // imgConnectionStatus
            // 
            imgConnectionStatus.Image = Properties.Resources.F1Help_16x;
            imgConnectionStatus.Location = new System.Drawing.Point(169, 203);
            imgConnectionStatus.Name = "imgConnectionStatus";
            imgConnectionStatus.Size = new System.Drawing.Size(16, 16);
            imgConnectionStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            imgConnectionStatus.TabIndex = 12;
            imgConnectionStatus.TabStop = false;
            // 
            // lblTestConnectionResults
            // 
            lblTestConnectionResults.AutoSize = true;
            lblTestConnectionResults.Location = new System.Drawing.Point(9, 226);
            lblTestConnectionResults.Name = "lblTestConnectionResults";
            lblTestConnectionResults.Size = new System.Drawing.Size(125, 13);
            lblTestConnectionResults.TabIndex = 13;
            lblTestConnectionResults.Text = "Test connection details";
            // 
            // chkSQLReadOnly
            // 
            chkSQLReadOnly._mice = MrngCheckBox.MouseState.OUT;
            chkSQLReadOnly.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            chkSQLReadOnly.AutoSize = true;
            chkSQLReadOnly.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkSQLReadOnly.Location = new System.Drawing.Point(163, 133);
            chkSQLReadOnly.Name = "chkSQLReadOnly";
            chkSQLReadOnly.Size = new System.Drawing.Size(15, 20);
            chkSQLReadOnly.TabIndex = 14;
            chkSQLReadOnly.UseVisualStyleBackColor = true;
            // 
            // lblSQLReadOnly
            // 
            lblSQLReadOnly.Dock = System.Windows.Forms.DockStyle.Fill;
            lblSQLReadOnly.Enabled = false;
            lblSQLReadOnly.Location = new System.Drawing.Point(3, 130);
            lblSQLReadOnly.Name = "lblSQLReadOnly";
            lblSQLReadOnly.Size = new System.Drawing.Size(154, 26);
            lblSQLReadOnly.TabIndex = 15;
            lblSQLReadOnly.Text = "Read Only:";
            lblSQLReadOnly.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSQLType
            // 
            lblSQLType.Dock = System.Windows.Forms.DockStyle.Fill;
            lblSQLType.Enabled = false;
            lblSQLType.Location = new System.Drawing.Point(3, 0);
            lblSQLType.Name = "lblSQLType";
            lblSQLType.Size = new System.Drawing.Size(154, 26);
            lblSQLType.TabIndex = 20;
            lblSQLType.Text = "SQL Server Type:";
            lblSQLType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSQLType
            // 
            txtSQLType._mice = MrngComboBox.MouseState.HOVER;
            txtSQLType.Dock = System.Windows.Forms.DockStyle.Fill;
            txtSQLType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            txtSQLType.FormattingEnabled = true;
            txtSQLType.Items.AddRange(new object[] { "mssql", "mysql" });
            txtSQLType.Location = new System.Drawing.Point(163, 3);
            txtSQLType.Name = "txtSQLType";
            txtSQLType.Size = new System.Drawing.Size(235, 21);
            txtSQLType.TabIndex = 21;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(lblSQLType, 0, 0);
            tableLayoutPanel1.Controls.Add(txtSQLType, 1, 0);
            tableLayoutPanel1.Controls.Add(lblSQLServer, 0, 1);
            tableLayoutPanel1.Controls.Add(chkSQLReadOnly, 1, 5);
            tableLayoutPanel1.Controls.Add(lblSQLReadOnly, 0, 5);
            tableLayoutPanel1.Controls.Add(lblSQLDatabaseName, 0, 2);
            tableLayoutPanel1.Controls.Add(txtSQLDatabaseName, 1, 2);
            tableLayoutPanel1.Controls.Add(lblSQLUsername, 0, 3);
            tableLayoutPanel1.Controls.Add(lblSQLPassword, 0, 4);
            tableLayoutPanel1.Controls.Add(txtSQLServer, 1, 1);
            tableLayoutPanel1.Controls.Add(txtSQLPassword, 1, 4);
            tableLayoutPanel1.Controls.Add(txtSQLUsername, 1, 3);
            tableLayoutPanel1.Location = new System.Drawing.Point(9, 30);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 7;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.Size = new System.Drawing.Size(401, 162);
            tableLayoutPanel1.TabIndex = 22;
            // 
            // SqlServerPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            Controls.Add(tableLayoutPanel1);
            Controls.Add(lblTestConnectionResults);
            Controls.Add(imgConnectionStatus);
            Controls.Add(btnTestConnection);
            Controls.Add(chkUseSQLServer);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "SqlServerPage";
            Size = new System.Drawing.Size(610, 490);
            ((System.ComponentModel.ISupportInitialize)imgConnectionStatus).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        internal Controls.MrngLabel lblSQLDatabaseName;
        internal Controls.MrngTextBox txtSQLDatabaseName;
        internal MrngCheckBox chkUseSQLServer;
        internal Controls.MrngLabel lblSQLUsername;
        internal Controls.MrngTextBox txtSQLPassword;
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
