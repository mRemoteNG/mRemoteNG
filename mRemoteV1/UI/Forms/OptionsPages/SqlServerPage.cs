using System;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.Connections;
using mRemoteNG.My;
using mRemoteNG.Security;

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

            chkUseSQLServer.Checked = mRemoteNG.Settings.Default.UseSQLServer;
            txtSQLServer.Text = mRemoteNG.Settings.Default.SQLHost;
            txtSQLDatabaseName.Text = mRemoteNG.Settings.Default.SQLDatabaseName;
            txtSQLUsername.Text = mRemoteNG.Settings.Default.SQLUser;
            txtSQLPassword.Text = Crypt.Decrypt(mRemoteNG.Settings.Default.SQLPass, GeneralAppInfo.EncryptionKey);
        }

        public override void SaveSettings()
        {
            base.SaveSettings();

            mRemoteNG.Settings.Default.UseSQLServer = chkUseSQLServer.Checked;
            mRemoteNG.Settings.Default.SQLHost = txtSQLServer.Text;
            mRemoteNG.Settings.Default.SQLDatabaseName = txtSQLDatabaseName.Text;
            mRemoteNG.Settings.Default.SQLUser = txtSQLUsername.Text;
            mRemoteNG.Settings.Default.SQLPass = Crypt.Encrypt(txtSQLPassword.Text, GeneralAppInfo.EncryptionKey);
            ReinitializeSqlUpdater();
        }

        private static void ReinitializeSqlUpdater()
        {
            if (Runtime.SQLConnProvider != null)
            {
                Runtime.SQLConnProvider.Dispose();
                frmMain.Default.AreWeUsingSqlServerForSavingConnections = mRemoteNG.Settings.Default.UseSQLServer;
                if (mRemoteNG.Settings.Default.UseSQLServer)
                {
                    Runtime.SQLConnProvider = new SqlConnectionsProvider();
                    Runtime.SQLConnProvider.Enable();
                }
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