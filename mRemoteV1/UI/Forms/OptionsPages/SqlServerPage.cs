using System;
using mRemoteNG.App;
using mRemoteNG.Config.Connections;
using mRemoteNG.Config.Connections.Multiuser;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Security.SymmetricEncryption;

namespace mRemoteNG.UI.Forms.OptionsPages
{
	public sealed partial class SqlServerPage
    {
        private readonly SqlDatabaseConnectionTester _databaseConnectionTester;

        public SqlServerPage()
        {
            InitializeComponent();
            ApplyTheme();
            _databaseConnectionTester = new SqlDatabaseConnectionTester();
        }

        public override string PageName
        {
            get => Language.strSQLServer.TrimEnd(':');
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
            lblSQLReadOnly.Text = Language.strLabelReadOnly;
            btnTestConnection.Text = Language.TestConnection;
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
            chkSQLReadOnly.Checked = Settings.Default.SQLReadOnly;
	        lblTestConnectionResults.Text = "";
        }

        public override void SaveSettings()
        {
            base.SaveSettings();
            var sqlServerWasPreviouslyEnabled = Settings.Default.UseSQLServer;

            Settings.Default.UseSQLServer = chkUseSQLServer.Checked;
            Settings.Default.SQLHost = txtSQLServer.Text;
            Settings.Default.SQLDatabaseName = txtSQLDatabaseName.Text;
            Settings.Default.SQLUser = txtSQLUsername.Text;
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            Settings.Default.SQLPass = cryptographyProvider.Encrypt(txtSQLPassword.Text, Runtime.EncryptionKey);
            Settings.Default.SQLReadOnly = chkSQLReadOnly.Checked;

            if (Settings.Default.UseSQLServer)
                ReinitializeSqlUpdater();
            else if (!Settings.Default.UseSQLServer && sqlServerWasPreviouslyEnabled)
                DisableSql();

            Settings.Default.Save();
        }

        private static void ReinitializeSqlUpdater()
        {
            Runtime.ConnectionsService.RemoteConnectionsSyncronizer?.Dispose();
            Runtime.ConnectionsService.RemoteConnectionsSyncronizer = new RemoteConnectionsSyncronizer(new SqlConnectionsUpdateChecker());
            Runtime.ConnectionsService.LoadConnections(true, false, "");
        }

        private void DisableSql()
        {
            Runtime.ConnectionsService.RemoteConnectionsSyncronizer?.Dispose();
            Runtime.ConnectionsService.RemoteConnectionsSyncronizer = null;
            Runtime.LoadConnections(true);
        }

        private void chkUseSQLServer_CheckedChanged(object sender, EventArgs e)
        {
            toggleSQLPageControls(chkUseSQLServer.Checked);
        }

        private void toggleSQLPageControls(bool useSQLServer)
        {
            lblSQLServer.Enabled = useSQLServer;
            lblSQLDatabaseName.Enabled = useSQLServer;
            lblSQLUsername.Enabled = useSQLServer;
            lblSQLPassword.Enabled = useSQLServer;
            lblSQLReadOnly.Enabled = useSQLServer;
            txtSQLServer.Enabled = useSQLServer;
            txtSQLDatabaseName.Enabled = useSQLServer;
            txtSQLUsername.Enabled = useSQLServer;
            txtSQLPassword.Enabled = useSQLServer;
            chkSQLReadOnly.Enabled = useSQLServer;
            btnTestConnection.Enabled = useSQLServer;
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