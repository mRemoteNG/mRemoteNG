using System;
using mRemoteNG.My;
using mRemoteNG.App;
using mRemoteNG.Security;
using mRemoteNG.App.Info;
using mRemoteNG.Config.Connections;


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
				
			chkUseSQLServer.Checked = My.Settings.Default.UseSQLServer;
			txtSQLServer.Text = My.Settings.Default.SQLHost;
			txtSQLDatabaseName.Text = My.Settings.Default.SQLDatabaseName;
			txtSQLUsername.Text = My.Settings.Default.SQLUser;
			txtSQLPassword.Text = Crypt.Decrypt(My.Settings.Default.SQLPass, GeneralAppInfo.EncryptionKey);
		}
			
		public override void SaveSettings()
		{
			base.SaveSettings();
				
			My.Settings.Default.UseSQLServer = chkUseSQLServer.Checked;
			My.Settings.Default.SQLHost = txtSQLServer.Text;
			My.Settings.Default.SQLDatabaseName = txtSQLDatabaseName.Text;
			My.Settings.Default.SQLUser = txtSQLUsername.Text;
			My.Settings.Default.SQLPass = Crypt.Encrypt(txtSQLPassword.Text, GeneralAppInfo.EncryptionKey);
            ReinitializeSQLUpdater();
		}

        private static void ReinitializeSQLUpdater()
        {
            Runtime.SQLConnProvider.Dispose();
            frmMain.Default.AreWeUsingSqlServerForSavingConnections = My.Settings.Default.UseSQLServer;
            if (My.Settings.Default.UseSQLServer)
            {
                Runtime.SQLConnProvider = new SqlConnectionsProvider();
                Runtime.SQLConnProvider.Enable();
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