using System;
using mRemoteNG.My;
using mRemoteNG.Config;
using mRemoteNG.Security;


namespace mRemoteNG.Forms.OptionsPages
{
	public partial class ConnectionsPage
	{
		public ConnectionsPage()
		{
			InitializeComponent();
		}
        public override string PageName
		{
			get
			{
				return Language.strConnections;
			}
			set
			{
			}
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
				
			chkSingleClickOnConnectionOpensIt.Checked = System.Convert.ToBoolean(My.Settings.Default.SingleClickOnConnectionOpensIt);
			chkSingleClickOnOpenedConnectionSwitchesToIt.Checked = System.Convert.ToBoolean(My.Settings.Default.SingleClickSwitchesToOpenConnection);
			chkHostnameLikeDisplayName.Checked = System.Convert.ToBoolean(My.Settings.Default.SetHostnameLikeDisplayName);
				
			numRdpReconnectionCount.Value = System.Convert.ToDecimal(My.Settings.Default.RdpReconnectionCount);
				
			numAutoSave.Value = System.Convert.ToDecimal(My.Settings.Default.AutoSaveEveryMinutes);
				
			// ReSharper disable once StringLiteralTypo
			if ((string) My.Settings.Default.EmptyCredentials == "noinfo")
			{
				radCredentialsNoInfo.Checked = true;
			}
			else if ((string) My.Settings.Default.EmptyCredentials == "windows")
			{
				radCredentialsWindows.Checked = true;
			}
			else if ((string) My.Settings.Default.EmptyCredentials == "custom")
			{
				radCredentialsCustom.Checked = true;
			}
				
			txtCredentialsUsername.Text = System.Convert.ToString(My.Settings.Default.DefaultUsername);
			txtCredentialsPassword.Text = Crypt.Decrypt(System.Convert.ToString(My.Settings.Default.DefaultPassword), App.Info.GeneralAppInfo.EncryptionKey);
			txtCredentialsDomain.Text = System.Convert.ToString(My.Settings.Default.DefaultDomain);

            if (My.Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.Never)
			{
				radCloseWarnNever.Checked = true;
			}
            else if (My.Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.Exit)
			{
				radCloseWarnExit.Checked = true;
			}
            else if (My.Settings.Default.ConfirmCloseConnection == (int)ConfirmCloseEnum.Multiple)
			{
				radCloseWarnMultiple.Checked = true;
			}
			else
			{
				radCloseWarnAll.Checked = true;
			}
		}
			
		public override void SaveSettings()
		{
			base.SaveSettings();
				
			My.Settings.Default.SingleClickOnConnectionOpensIt = chkSingleClickOnConnectionOpensIt.Checked;
			My.Settings.Default.SingleClickSwitchesToOpenConnection = chkSingleClickOnOpenedConnectionSwitchesToIt.Checked;
			My.Settings.Default.SetHostnameLikeDisplayName = chkHostnameLikeDisplayName.Checked;

            My.Settings.Default.RdpReconnectionCount = (int)numRdpReconnectionCount.Value;

            My.Settings.Default.AutoSaveEveryMinutes = (int)numAutoSave.Value;
			if (My.Settings.Default.AutoSaveEveryMinutes > 0)
			{
				frmMain.Default.tmrAutoSave.Interval = System.Convert.ToInt32(My.Settings.Default.AutoSaveEveryMinutes * 60000);
				frmMain.Default.tmrAutoSave.Enabled = true;
			}
			else
			{
				frmMain.Default.tmrAutoSave.Enabled = false;
			}
				
			if (radCredentialsNoInfo.Checked)
			{
				// ReSharper disable once StringLiteralTypo
				My.Settings.Default.EmptyCredentials = "noinfo";
			}
			else if (radCredentialsWindows.Checked)
			{
				My.Settings.Default.EmptyCredentials = "windows";
			}
			else if (radCredentialsCustom.Checked)
			{
				My.Settings.Default.EmptyCredentials = "custom";
			}
				
			My.Settings.Default.DefaultUsername = txtCredentialsUsername.Text;
			My.Settings.Default.DefaultPassword = Crypt.Encrypt(txtCredentialsPassword.Text, App.Info.GeneralAppInfo.EncryptionKey);
			My.Settings.Default.DefaultDomain = txtCredentialsDomain.Text;
				
			if (radCloseWarnAll.Checked)
			{
                My.Settings.Default.ConfirmCloseConnection = (int)ConfirmCloseEnum.All;
			}
			if (radCloseWarnMultiple.Checked)
			{
                My.Settings.Default.ConfirmCloseConnection = (int)ConfirmCloseEnum.Multiple;
			}
			if (radCloseWarnExit.Checked)
			{
                My.Settings.Default.ConfirmCloseConnection = (int)ConfirmCloseEnum.Exit;
			}
			if (radCloseWarnNever.Checked)
			{
				My.Settings.Default.ConfirmCloseConnection = (int)ConfirmCloseEnum.Never;
			}
		}
			
		public void radCredentialsCustom_CheckedChanged(object sender, EventArgs e)
		{
			lblCredentialsUsername.Enabled = radCredentialsCustom.Checked;
			lblCredentialsPassword.Enabled = radCredentialsCustom.Checked;
			lblCredentialsDomain.Enabled = radCredentialsCustom.Checked;
			txtCredentialsUsername.Enabled = radCredentialsCustom.Checked;
			txtCredentialsPassword.Enabled = radCredentialsCustom.Checked;
			txtCredentialsDomain.Enabled = radCredentialsCustom.Checked;
		}
	}
}
