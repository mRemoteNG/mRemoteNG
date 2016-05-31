

namespace mRemoteNG.UI.Forms.OptionsPages
{
	
    public partial class SqlServerPage : OptionsPage
	{
			
		//UserControl overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && components != null)
				{
					components.Dispose();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}
			
		//Required by the Windows Form Designer
		private System.ComponentModel.Container components = null;
			
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SqlServerPage));
			this.lblSQLDatabaseName = new System.Windows.Forms.Label();
			this.txtSQLDatabaseName = new System.Windows.Forms.TextBox();
			this.lblExperimental = new System.Windows.Forms.Label();
			this.chkUseSQLServer = new System.Windows.Forms.CheckBox();
			this.chkUseSQLServer.CheckedChanged += new System.EventHandler(this.chkUseSQLServer_CheckedChanged);
			this.lblSQLUsername = new System.Windows.Forms.Label();
			this.txtSQLPassword = new System.Windows.Forms.TextBox();
			this.lblSQLInfo = new System.Windows.Forms.Label();
			this.lblSQLServer = new System.Windows.Forms.Label();
			this.txtSQLUsername = new System.Windows.Forms.TextBox();
			this.txtSQLServer = new System.Windows.Forms.TextBox();
			this.lblSQLPassword = new System.Windows.Forms.Label();
			this.SuspendLayout();
			//
			//lblSQLDatabaseName
			//
			this.lblSQLDatabaseName.Enabled = false;
			this.lblSQLDatabaseName.Location = new System.Drawing.Point(23, 132);
			this.lblSQLDatabaseName.Name = "lblSQLDatabaseName";
			this.lblSQLDatabaseName.Size = new System.Drawing.Size(111, 13);
			this.lblSQLDatabaseName.TabIndex = 16;
			this.lblSQLDatabaseName.Text = "Database:";
			this.lblSQLDatabaseName.TextAlign = System.Drawing.ContentAlignment.TopRight;
			//
			//txtSQLDatabaseName
			//
			this.txtSQLDatabaseName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtSQLDatabaseName.Enabled = false;
			this.txtSQLDatabaseName.Location = new System.Drawing.Point(140, 130);
			this.txtSQLDatabaseName.Name = "txtSQLDatabaseName";
			this.txtSQLDatabaseName.Size = new System.Drawing.Size(153, 20);
			this.txtSQLDatabaseName.TabIndex = 17;
			//
			//lblExperimental
			//
			this.lblExperimental.Anchor = (System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.lblExperimental.Font = new System.Drawing.Font("Segoe UI", 12.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World);
			this.lblExperimental.ForeColor = System.Drawing.Color.FromArgb(System.Convert.ToInt32(System.Convert.ToByte(192)), System.Convert.ToInt32(System.Convert.ToByte(0)), System.Convert.ToInt32(System.Convert.ToByte(0)));
			this.lblExperimental.Location = new System.Drawing.Point(3, 0);
			this.lblExperimental.Name = "lblExperimental";
			this.lblExperimental.Size = new System.Drawing.Size(596, 25);
			this.lblExperimental.TabIndex = 11;
			this.lblExperimental.Text = "EXPERIMENTAL";
			this.lblExperimental.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			//chkUseSQLServer
			//
			this.chkUseSQLServer.AutoSize = true;
			this.chkUseSQLServer.Location = new System.Drawing.Point(3, 76);
			this.chkUseSQLServer.Name = "chkUseSQLServer";
			this.chkUseSQLServer.Size = new System.Drawing.Size(234, 17);
			this.chkUseSQLServer.TabIndex = 13;
			this.chkUseSQLServer.Text = "Use SQL Server to load && save connections";
			this.chkUseSQLServer.UseVisualStyleBackColor = true;
			//
			//lblSQLUsername
			//
			this.lblSQLUsername.Enabled = false;
			this.lblSQLUsername.Location = new System.Drawing.Point(23, 158);
			this.lblSQLUsername.Name = "lblSQLUsername";
			this.lblSQLUsername.Size = new System.Drawing.Size(111, 13);
			this.lblSQLUsername.TabIndex = 18;
			this.lblSQLUsername.Text = "Username:";
			this.lblSQLUsername.TextAlign = System.Drawing.ContentAlignment.TopRight;
			//
			//txtSQLPassword
			//
			this.txtSQLPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtSQLPassword.Enabled = false;
			this.txtSQLPassword.Location = new System.Drawing.Point(140, 182);
			this.txtSQLPassword.Name = "txtSQLPassword";
			this.txtSQLPassword.Size = new System.Drawing.Size(153, 20);
			this.txtSQLPassword.TabIndex = 21;
			this.txtSQLPassword.UseSystemPasswordChar = true;
			//
			//lblSQLInfo
			//
			this.lblSQLInfo.Anchor = (System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.lblSQLInfo.Font = new System.Drawing.Font("Segoe UI", 12.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World);
			this.lblSQLInfo.ForeColor = System.Drawing.Color.FromArgb(System.Convert.ToInt32(System.Convert.ToByte(192)), System.Convert.ToInt32(System.Convert.ToByte(0)), System.Convert.ToInt32(System.Convert.ToByte(0)));
			this.lblSQLInfo.Location = new System.Drawing.Point(3, 25);
			this.lblSQLInfo.Name = "lblSQLInfo";
			this.lblSQLInfo.Size = new System.Drawing.Size(596, 25);
			this.lblSQLInfo.TabIndex = 12;
			this.lblSQLInfo.Text = "Please see Help - Getting started - SQL Configuration for more Info!";
			this.lblSQLInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			//lblSQLServer
			//
			this.lblSQLServer.Enabled = false;
			this.lblSQLServer.Location = new System.Drawing.Point(23, 106);
			this.lblSQLServer.Name = "lblSQLServer";
			this.lblSQLServer.Size = new System.Drawing.Size(111, 13);
			this.lblSQLServer.TabIndex = 14;
			this.lblSQLServer.Text = "SQL Server:";
			this.lblSQLServer.TextAlign = System.Drawing.ContentAlignment.TopRight;
			//
			//txtSQLUsername
			//
			this.txtSQLUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtSQLUsername.Enabled = false;
			this.txtSQLUsername.Location = new System.Drawing.Point(140, 156);
			this.txtSQLUsername.Name = "txtSQLUsername";
			this.txtSQLUsername.Size = new System.Drawing.Size(153, 20);
			this.txtSQLUsername.TabIndex = 19;
			//
			//txtSQLServer
			//
			this.txtSQLServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtSQLServer.Enabled = false;
			this.txtSQLServer.Location = new System.Drawing.Point(140, 103);
			this.txtSQLServer.Name = "txtSQLServer";
			this.txtSQLServer.Size = new System.Drawing.Size(153, 20);
			this.txtSQLServer.TabIndex = 15;
			//
			//lblSQLPassword
			//
			this.lblSQLPassword.Enabled = false;
			this.lblSQLPassword.Location = new System.Drawing.Point(23, 184);
			this.lblSQLPassword.Name = "lblSQLPassword";
			this.lblSQLPassword.Size = new System.Drawing.Size(111, 13);
			this.lblSQLPassword.TabIndex = 20;
			this.lblSQLPassword.Text = "Password:";
			this.lblSQLPassword.TextAlign = System.Drawing.ContentAlignment.TopRight;
			//
			//SqlServerPage
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF((float) (6.0F), (float) (13.0F));
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
			this.Name = "SqlServerPage";
			this.PageIcon = (System.Drawing.Icon) (resources.GetObject("$this.PageIcon"));
			this.Size = new System.Drawing.Size(610, 489);
			this.ResumeLayout(false);
			this.PerformLayout();
				
		}
		internal System.Windows.Forms.Label lblSQLDatabaseName;
		internal System.Windows.Forms.TextBox txtSQLDatabaseName;
		internal System.Windows.Forms.Label lblExperimental;
		internal System.Windows.Forms.CheckBox chkUseSQLServer;
		internal System.Windows.Forms.Label lblSQLUsername;
		internal System.Windows.Forms.TextBox txtSQLPassword;
		internal System.Windows.Forms.Label lblSQLInfo;
		internal System.Windows.Forms.Label lblSQLServer;
		internal System.Windows.Forms.TextBox txtSQLUsername;
		internal System.Windows.Forms.TextBox txtSQLServer;
		internal System.Windows.Forms.Label lblSQLPassword;
			
	}
}
