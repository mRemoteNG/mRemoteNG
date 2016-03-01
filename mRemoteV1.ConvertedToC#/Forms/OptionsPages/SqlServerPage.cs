using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.My;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Security;

namespace mRemoteNG.Forms.OptionsPages
{
	public partial class SqlServerPage
	{
		public override string PageName {
			get { return Language.strSQLServer.TrimEnd(":"); }
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

			chkUseSQLServer.Checked = mRemoteNG.My.Settings.UseSQLServer;
			txtSQLServer.Text = mRemoteNG.My.Settings.SQLHost;
			txtSQLDatabaseName.Text = mRemoteNG.My.Settings.SQLDatabaseName;
			txtSQLUsername.Text = mRemoteNG.My.Settings.SQLUser;
			txtSQLPassword.Text = Crypt.Decrypt(mRemoteNG.My.Settings.SQLPass, General.EncryptionKey);
		}

		public override void SaveSettings()
		{
			base.SaveSettings();

			mRemoteNG.My.Settings.UseSQLServer = chkUseSQLServer.Checked;
			mRemoteNG.My.Settings.SQLHost = txtSQLServer.Text;
			mRemoteNG.My.Settings.SQLDatabaseName = txtSQLDatabaseName.Text;
			mRemoteNG.My.Settings.SQLUser = txtSQLUsername.Text;
			mRemoteNG.My.Settings.SQLPass = Crypt.Encrypt(txtSQLPassword.Text, General.EncryptionKey);

			Runtime.Startup.DestroySQLUpdateHandlerAndStopTimer();
			My.MyProject.Forms.frmMain.UsingSqlServer = mRemoteNG.My.Settings.UseSQLServer;
			if (mRemoteNG.My.Settings.UseSQLServer) {
				Runtime.Startup.CreateSQLUpdateHandlerAndStartTimer();
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
		public SqlServerPage()
		{
			InitializeComponent();
		}
	}
}
