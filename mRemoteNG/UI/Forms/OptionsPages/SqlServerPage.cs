using System;
using mRemoteNG.App;
using mRemoteNG.Config.Connections.Multiuser;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Properties;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    [SupportedOSPlatform("windows")]
    public sealed partial class SqlServerPage
    {
        private readonly DatabaseConnectionTester _databaseConnectionTester;

        public SqlServerPage()
        {
            InitializeComponent();
            ApplyTheme();
            PageIcon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.SQLDatabase_16x);
            _databaseConnectionTester = new DatabaseConnectionTester();
        }

        public override string PageName
        {
            get => Language.SQLServer.TrimEnd(':');
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            lblExperimental.Text = Language.Experimental.ToUpper();
            lblSQLInfo.Text = Language.SQLInfo;

            chkUseSQLServer.Text = Language.UseSQLServer;
            lblSQLServer.Text = Language.Hostname;
            lblSQLDatabaseName.Text = Language.Database;
            lblSQLUsername.Text = Language.Username;
            lblSQLPassword.Text = Language.Password;
            lblSQLReadOnly.Text = Language.ReadOnly;
            btnTestConnection.Text = Language.TestConnection;
        }

        public override void LoadSettings()
        {
            chkUseSQLServer.Checked = Properties.OptionsDBsPage.Default.UseSQLServer;
            txtSQLType.Text = Properties.OptionsDBsPage.Default.SQLServerType;
            txtSQLServer.Text = Properties.OptionsDBsPage.Default.SQLHost;
            txtSQLDatabaseName.Text = Properties.OptionsDBsPage.Default.SQLDatabaseName;
            txtSQLUsername.Text = Properties.OptionsDBsPage.Default.SQLUser;
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            txtSQLPassword.Text = cryptographyProvider.Decrypt(Properties.OptionsDBsPage.Default.SQLPass, Runtime.EncryptionKey);
            chkSQLReadOnly.Checked = Properties.OptionsDBsPage.Default.SQLReadOnly;
            lblTestConnectionResults.Text = "";
        }

        public override void SaveSettings()
        {
            base.SaveSettings();
            var sqlServerWasPreviouslyEnabled = Properties.OptionsDBsPage.Default.UseSQLServer;

            Properties.OptionsDBsPage.Default.UseSQLServer = chkUseSQLServer.Checked;
            Properties.OptionsDBsPage.Default.SQLServerType = txtSQLType.Text;
            Properties.OptionsDBsPage.Default.SQLHost = txtSQLServer.Text;
            Properties.OptionsDBsPage.Default.SQLDatabaseName = txtSQLDatabaseName.Text;
            Properties.OptionsDBsPage.Default.SQLUser = txtSQLUsername.Text;
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            Properties.OptionsDBsPage.Default.SQLPass = cryptographyProvider.Encrypt(txtSQLPassword.Text, Runtime.EncryptionKey);
            Properties.OptionsDBsPage.Default.SQLReadOnly = chkSQLReadOnly.Checked;

            if (Properties.OptionsDBsPage.Default.UseSQLServer)
                ReinitializeSqlUpdater();
            else if (!Properties.OptionsDBsPage.Default.UseSQLServer && sqlServerWasPreviouslyEnabled)
                DisableSql();
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
            lblSQLType.Enabled = useSQLServer;
            lblSQLServer.Enabled = useSQLServer;
            lblSQLDatabaseName.Enabled = useSQLServer;
            lblSQLUsername.Enabled = useSQLServer;
            lblSQLPassword.Enabled = useSQLServer;
            lblSQLReadOnly.Enabled = useSQLServer;
            txtSQLType.Enabled = useSQLServer;
            txtSQLServer.Enabled = useSQLServer;
            txtSQLDatabaseName.Enabled = useSQLServer;
            txtSQLUsername.Enabled = useSQLServer;
            txtSQLPassword.Enabled = useSQLServer;
            chkSQLReadOnly.Enabled = useSQLServer;
            btnTestConnection.Enabled = useSQLServer;
        }

        private async void btnTestConnection_Click(object sender, EventArgs e)
        {
            var type = txtSQLType.Text;
            var server = txtSQLServer.Text;
            var database = txtSQLDatabaseName.Text;
            var username = txtSQLUsername.Text;
            var password = txtSQLPassword.Text;

            lblTestConnectionResults.Text = Language.TestingConnection;
            imgConnectionStatus.Image = Properties.Resources.Loading_Spinner;
            btnTestConnection.Enabled = false;

            var connectionTestResult =
                await _databaseConnectionTester.TestConnectivity(type, server, database, username, password);

            btnTestConnection.Enabled = true;

            switch (connectionTestResult)
            {
                case ConnectionTestResult.ConnectionSucceded:
                    UpdateConnectionImage(true);
                    lblTestConnectionResults.Text = Language.ConnectionSuccessful;
                    break;
                case ConnectionTestResult.ServerNotAccessible:
                    UpdateConnectionImage(false);
                    lblTestConnectionResults.Text =
                        BuildTestFailedMessage(string.Format(Language.ServerNotAccessible, server));
                    break;
                case ConnectionTestResult.CredentialsRejected:
                    UpdateConnectionImage(false);
                    lblTestConnectionResults.Text =
                        BuildTestFailedMessage(string.Format(Language.LoginFailedForUser, username));
                    break;
                case ConnectionTestResult.UnknownDatabase:
                    UpdateConnectionImage(false);
                    lblTestConnectionResults.Text =
                        BuildTestFailedMessage(string.Format(Language.DatabaseNotAvailable, database));
                    break;
                case ConnectionTestResult.UnknownError:
                    UpdateConnectionImage(false);
                    lblTestConnectionResults.Text = BuildTestFailedMessage(Language.RdpErrorUnknown);
                    break;
                default:
                    UpdateConnectionImage(false);
                    lblTestConnectionResults.Text = BuildTestFailedMessage(Language.RdpErrorUnknown);
                    break;
            }
        }

        private void UpdateConnectionImage(bool connectionSuccess)
        {
            imgConnectionStatus.Image = connectionSuccess ? Properties.Resources.Test_16x : Properties.Resources.LogError_16x;
        }

        private string BuildTestFailedMessage(string specificMessage)
        {
            return Language.ConnectionOpenFailed + Environment.NewLine + specificMessage;
        }
    }
}