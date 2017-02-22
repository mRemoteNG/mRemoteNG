using System;
using mRemoteNG.App;
using mRemoteNG.Config.Connections;
using mRemoteNG.Config.Connections.Multiuser;
using mRemoteNG.Security.SymmetricEncryption;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public partial class SqlServerPage
    {
        public SqlServerPage()
        {
            InitializeComponent();
        }

        public override string PageName
        {
            get { return Language.strSQLServer.TrimEnd(':'); }
            set { }
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

            chkUseSQLServer.Checked = Settings.Default.UseSQLServer;
            txtSQLServer.Text = Settings.Default.SQLHost;
            txtSQLDatabaseName.Text = Settings.Default.SQLDatabaseName;
            txtSQLUsername.Text = Settings.Default.SQLUser;
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            txtSQLPassword.Text = cryptographyProvider.Decrypt(Settings.Default.SQLPass, Runtime.EncryptionKey);
        }

        public override void SaveSettings()
        {
            base.SaveSettings();

            Settings.Default.UseSQLServer = chkUseSQLServer.Checked;
            Settings.Default.SQLHost = txtSQLServer.Text;
            Settings.Default.SQLDatabaseName = txtSQLDatabaseName.Text;
            Settings.Default.SQLUser = txtSQLUsername.Text;
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            Settings.Default.SQLPass = cryptographyProvider.Encrypt(txtSQLPassword.Text, Runtime.EncryptionKey);
            ReinitializeSqlUpdater();

            Settings.Default.Save();
        }

        private static void ReinitializeSqlUpdater()
        {
            Runtime.RemoteConnectionsSyncronizer?.Dispose();
            FrmMain.Default.AreWeUsingSqlServerForSavingConnections = Settings.Default.UseSQLServer;

            if (Settings.Default.UseSQLServer)
            {
                Runtime.RemoteConnectionsSyncronizer = new RemoteConnectionsSyncronizer(new SqlConnectionsUpdateChecker());
                Runtime.RemoteConnectionsSyncronizer.Enable();
            }
            else
            {
                Runtime.RemoteConnectionsSyncronizer?.Dispose();
                Runtime.RemoteConnectionsSyncronizer = null;
            }
        }

        private void chkUseSQLServer_CheckedChanged(object sender, EventArgs e)
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