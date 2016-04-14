using System;
using mRemoteNG.My;
using mRemoteNG.App;
using mRemoteNG.Security;


namespace mRemoteNG.Forms.OptionsPages
{
    public partial class SqlServerPage
	{
		public SqlServerPage()
		{
			InitializeComponent();
		}
        public override string PageName
		{
			get
			{
				return Language.strSQLServer.TrimEnd(':');
			}
			set
			{
			}
		}
			
		public override void ApplyLanguage()
		{
			base.ApplyLanguage();
				
			lblExperimental.Text = Language.strExperimental.ToUpper();
			lblSQLInfo.Text = Language.strSQLInfo;
				
			chkUseSQLServer.Text = Language.strUseSQLServer;
			lblSQLServer.Text = Language.strLabelHostname;
			lblSQLDatabaseName.Text = Language.strLabelSQLServerDatabaseName;
			lblSQLUsername.Text = Language.strLabelUsername;
			lblSQLPassword.Text = Language.strLabelPassword;
		}
			
		public override void LoadSettings()
		{
			base.SaveSettings();
				
			chkUseSQLServer.Checked = System.Convert.ToBoolean(My.Settings.Default.UseSQLServer);
			txtSQLServer.Text = System.Convert.ToString(My.Settings.Default.SQLHost);
			txtSQLDatabaseName.Text = System.Convert.ToString(My.Settings.Default.SQLDatabaseName);
			txtSQLUsername.Text = System.Convert.ToString(My.Settings.Default.SQLUser);
			txtSQLPassword.Text = Crypt.Decrypt(System.Convert.ToString(My.Settings.Default.SQLPass), App.Info.General.EncryptionKey);
		}
			
		public override void SaveSettings()
		{
			base.SaveSettings();
				
			My.Settings.Default.UseSQLServer = chkUseSQLServer.Checked;
			My.Settings.Default.SQLHost = txtSQLServer.Text;
			My.Settings.Default.SQLDatabaseName = txtSQLDatabaseName.Text;
			My.Settings.Default.SQLUser = txtSQLUsername.Text;
			My.Settings.Default.SQLPass = Crypt.Encrypt(txtSQLPassword.Text, App.Info.General.EncryptionKey);
				
			Runtime.Startup.DestroySQLUpdateHandlerAndStopTimer();
			frmMain.Default.UsingSqlServer = System.Convert.ToBoolean(My.Settings.Default.UseSQLServer);
			if (My.Settings.Default.UseSQLServer)
			{
				Runtime.Startup.CreateSQLUpdateHandlerAndStartTimer();
			}
		}
			
		public void chkUseSQLServer_CheckedChanged(object sender, EventArgs e)
		{
			lblSQLServer.Enabled = chkUseSQLServer.Checked;
			lblSQLDatabaseName.Enabled = chkUseSQLServer.Checked;
			lblSQLUsername.Enabled = chkUseSQLServer.Checked;
			lblSQLPassword.Enabled = chkUseSQLServer.Checked;
			txtSQLServer.Enabled = chkUseSQLServer.Checked;
			txtSQLDatabaseName.Enabled = chkUseSQLServer.Checked;
			txtSQLUsername.Enabled = chkUseSQLServer.Checked;
			txtSQLPassword.Enabled = chkUseSQLServer.Checked;
		}
	}
}
