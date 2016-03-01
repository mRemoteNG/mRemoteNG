using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.My;
using mRemoteNG.Config;
using mRemoteNG.App.Info;
using mRemoteNG.Security;

namespace mRemoteNG.Forms.OptionsPages
{
	public partial class ConnectionsPage
	{
		public override string PageName {
			get { return Language.strConnections; }
			set { }
		}

		public override void ApplyLanguage()
		{
			base.ApplyLanguage();

			chkSingleClickOnConnectionOpensIt.Text = Language.strSingleClickOnConnectionOpensIt;
			chkSingleClickOnOpenedConnectionSwitchesToIt.Text = Language.strSingleClickOnOpenConnectionSwitchesToIt;
			chkHostnameLikeDisplayName.Text = Language.strSetHostnameLikeDisplayName;

			lblRdpReconnectionCount.Text = Language.strRdpReconnectCount;

			lblAutoSave1.Text = Language.strAutoSaveEvery;
			lblAutoSave2.Text = Language.strAutoSaveMins;

			lblDefaultCredentials.Text = Language.strEmptyUsernamePasswordDomainFields;
			radCredentialsNoInfo.Text = Language.strNoInformation;
			radCredentialsWindows.Text = Language.strMyCurrentWindowsCreds;
			radCredentialsCustom.Text = Language.strTheFollowing;
			lblCredentialsUsername.Text = Language.strLabelUsername;
			lblCredentialsPassword.Text = Language.strLabelPassword;
			lblCredentialsDomain.Text = Language.strLabelDomain;

			lblClosingConnections.Text = Language.strLabelClosingConnections;
			radCloseWarnAll.Text = Language.strRadioCloseWarnAll;
			radCloseWarnMultiple.Text = Language.strRadioCloseWarnMultiple;
			radCloseWarnExit.Text = Language.strRadioCloseWarnExit;
			radCloseWarnNever.Text = Language.strRadioCloseWarnNever;
		}

		public override void LoadSettings()
		{
			base.SaveSettings();

			chkSingleClickOnConnectionOpensIt.Checked = mRemoteNG.My.Settings.SingleClickOnConnectionOpensIt;
			chkSingleClickOnOpenedConnectionSwitchesToIt.Checked = mRemoteNG.My.Settings.SingleClickSwitchesToOpenConnection;
			chkHostnameLikeDisplayName.Checked = mRemoteNG.My.Settings.SetHostnameLikeDisplayName;

			numRdpReconnectionCount.Value = mRemoteNG.My.Settings.RdpReconnectionCount;

			numAutoSave.Value = mRemoteNG.My.Settings.AutoSaveEveryMinutes;

			switch (mRemoteNG.My.Settings.EmptyCredentials) {
				// ReSharper disable once StringLiteralTypo
				case "noinfo":
					radCredentialsNoInfo.Checked = true;
					break;
				case "windows":
					radCredentialsWindows.Checked = true;
					break;
				case "custom":
					radCredentialsCustom.Checked = true;
					break;
			}

			txtCredentialsUsername.Text = mRemoteNG.My.Settings.DefaultUsername;
			txtCredentialsPassword.Text = Crypt.Decrypt(mRemoteNG.My.Settings.DefaultPassword, General.EncryptionKey);
			txtCredentialsDomain.Text = mRemoteNG.My.Settings.DefaultDomain;

			switch (mRemoteNG.My.Settings.ConfirmCloseConnection) {
				case ConfirmClose.Never:
					radCloseWarnNever.Checked = true;
					break;
				case ConfirmClose.Exit:
					radCloseWarnExit.Checked = true;
					break;
				case ConfirmClose.Multiple:
					radCloseWarnMultiple.Checked = true;
					break;
				default:
					radCloseWarnAll.Checked = true;
					break;
			}
		}

		public override void SaveSettings()
		{
			base.SaveSettings();

			mRemoteNG.My.Settings.SingleClickOnConnectionOpensIt = chkSingleClickOnConnectionOpensIt.Checked;
			mRemoteNG.My.Settings.SingleClickSwitchesToOpenConnection = chkSingleClickOnOpenedConnectionSwitchesToIt.Checked;
			mRemoteNG.My.Settings.SetHostnameLikeDisplayName = chkHostnameLikeDisplayName.Checked;

			mRemoteNG.My.Settings.RdpReconnectionCount = numRdpReconnectionCount.Value;

			mRemoteNG.My.Settings.AutoSaveEveryMinutes = numAutoSave.Value;
			if (mRemoteNG.My.Settings.AutoSaveEveryMinutes > 0) {
				My.MyProject.Forms.frmMain.tmrAutoSave.Interval = mRemoteNG.My.Settings.AutoSaveEveryMinutes * 60000;
				My.MyProject.Forms.frmMain.tmrAutoSave.Enabled = true;
			} else {
				My.MyProject.Forms.frmMain.tmrAutoSave.Enabled = false;
			}

			if (radCredentialsNoInfo.Checked) {
				// ReSharper disable once StringLiteralTypo
				mRemoteNG.My.Settings.EmptyCredentials = "noinfo";
			} else if (radCredentialsWindows.Checked) {
				mRemoteNG.My.Settings.EmptyCredentials = "windows";
			} else if (radCredentialsCustom.Checked) {
				mRemoteNG.My.Settings.EmptyCredentials = "custom";
			}

			mRemoteNG.My.Settings.DefaultUsername = txtCredentialsUsername.Text;
			mRemoteNG.My.Settings.DefaultPassword = Crypt.Encrypt(txtCredentialsPassword.Text, General.EncryptionKey);
			mRemoteNG.My.Settings.DefaultDomain = txtCredentialsDomain.Text;

			if (radCloseWarnAll.Checked)
				mRemoteNG.My.Settings.ConfirmCloseConnection = ConfirmClose.All;
			if (radCloseWarnMultiple.Checked)
				mRemoteNG.My.Settings.ConfirmCloseConnection = ConfirmClose.Multiple;
			if (radCloseWarnExit.Checked)
				mRemoteNG.My.Settings.ConfirmCloseConnection = ConfirmClose.Exit;
			if (radCloseWarnNever.Checked)
				mRemoteNG.My.Settings.ConfirmCloseConnection = ConfirmClose.Never;
		}

		private void radCredentialsCustom_CheckedChanged(object sender, EventArgs e)
		{
			lblCredentialsUsername.Enabled = radCredentialsCustom.Checked;
			lblCredentialsPassword.Enabled = radCredentialsCustom.Checked;
			lblCredentialsDomain.Enabled = radCredentialsCustom.Checked;
			txtCredentialsUsername.Enabled = radCredentialsCustom.Checked;
			txtCredentialsPassword.Enabled = radCredentialsCustom.Checked;
			txtCredentialsDomain.Enabled = radCredentialsCustom.Checked;
		}
		public ConnectionsPage()
		{
			InitializeComponent();
		}
	}
}
