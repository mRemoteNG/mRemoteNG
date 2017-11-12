using System;
using mRemoteNG.App;
using mRemoteNG.Config.Connections;
using mRemoteNG.Config.Connections.Multiuser;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Security.SymmetricEncryption;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public partial class SqlServerPage
    {
        private SqlDatabaseConnectionTester _databaseConnectionTester;

        public SqlServerPage()
        {
            InitializeComponent();
            base.ApplyTheme();
            _databaseConnectionTester = new SqlDatabaseConnectionTester();
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
            btnTestConnection.Text = "Test Connection";
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

            if (Settings.Default.UseSQLServer)
            {
                Runtime.RemoteConnectionsSyncronizer = new RemoteConnectionsSyncronizer(new SqlConnectionsUpdateChecker());
                Runtime.RemoteConnectionsSyncronizer.Enable();
            }
            else
            {
                Runtime.RemoteConnectionsSyncronizer?.Dispose();
                Runtime.RemoteConnectionsSyncronizer = null;
                Runtime.LoadConnections(true);
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
            btnTestConnection.Enabled = chkUseSQLServer.Checked;
        }

        private async void btnTestConnection_Click(object sender, EventArgs e)
        {
            var server = txtSQLServer.Text;
            var database = txtSQLDatabaseName.Text;
            var username = txtSQLUsername.Text;
            var password = txtSQLPassword.Text;

            lblTestConnectionResults.Text = Language.TestingConnection;
            imgConnectionStatus.Image = Resources.loading_spinner;
            btnTestConnection.Enabled = false;

            var connectionTestResult = await _databaseConnectionTester.TestConnectivity(server, database, username, password);

            btnTestConnection.Enabled = true;

            switch (connectionTestResult)
            {
                case ConnectionTestResult.ConnectionSucceded:
                    UpdateConnectionImage(true);
                    lblTestConnectionResults.Text = Language.ConnectionSuccessful;
                    break;
                case ConnectionTestResult.ServerNotAccessible:
                    UpdateConnectionImage(false);
                    lblTestConnectionResults.Text = BuildTestFailedMessage(string.Format(Language.ServerNotAccessible, server));
                    break;
                case ConnectionTestResult.CredentialsRejected:
                    UpdateConnectionImage(false);
                    lblTestConnectionResults.Text = BuildTestFailedMessage(string.Format(Language.LoginFailedForUser, username));
                    break;
                case ConnectionTestResult.UnknownDatabase:
                    UpdateConnectionImage(false);
                    lblTestConnectionResults.Text = BuildTestFailedMessage(string.Format(Language.DatabaseNotAvailable, database));
                    break;
                case ConnectionTestResult.UnknownError:
                    UpdateConnectionImage(false);
                    lblTestConnectionResults.Text = BuildTestFailedMessage(Language.strRdpErrorUnknown);
                    break;
                default:
                    UpdateConnectionImage(false);
                    lblTestConnectionResults.Text = BuildTestFailedMessage(Language.strRdpErrorUnknown);
                    break;
            }
        }

        private void UpdateConnectionImage(bool connectionSuccess)
        {
            imgConnectionStatus.Image = connectionSuccess
                ? Resources.tick
                : Resources.exclamation;
        }

        private string BuildTestFailedMessage(string specificMessage)
        {
            return Language.strConnectionOpenFailed +
                   Environment.NewLine +
                   specificMessage;
        }
    }
}