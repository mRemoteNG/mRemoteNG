using System;
using mRemoteNG.App.Info;
using mRemoteNG.Config;
using mRemoteNG.My;
using mRemoteNG.Security;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public partial class ConnectionsPage
    {
        public ConnectionsPage()
        {
            InitializeComponent();
        }

        public override string PageName
        {
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

            chkSingleClickOnConnectionOpensIt.Checked =
                Convert.ToBoolean(mRemoteNG.Settings.Default.SingleClickOnConnectionOpensIt);
            chkSingleClickOnOpenedConnectionSwitchesToIt.Checked =
                Convert.ToBoolean(mRemoteNG.Settings.Default.SingleClickSwitchesToOpenConnection);
            chkHostnameLikeDisplayName.Checked = Convert.ToBoolean(mRemoteNG.Settings.Default.SetHostnameLikeDisplayName);

            numRdpReconnectionCount.Value = Convert.ToDecimal(mRemoteNG.Settings.Default.RdpReconnectionCount);

            numAutoSave.Value = Convert.ToDecimal(mRemoteNG.Settings.Default.AutoSaveEveryMinutes);

            // ReSharper disable once StringLiteralTypo
            if (mRemoteNG.Settings.Default.EmptyCredentials == "noinfo")
            {
                radCredentialsNoInfo.Checked = true;
            }
            else if (mRemoteNG.Settings.Default.EmptyCredentials == "windows")
            {
                radCredentialsWindows.Checked = true;
            }
            else if (mRemoteNG.Settings.Default.EmptyCredentials == "custom")
            {
                radCredentialsCustom.Checked = true;
            }

            txtCredentialsUsername.Text = Convert.ToString(mRemoteNG.Settings.Default.DefaultUsername);
            txtCredentialsPassword.Text = LegacyRijndaelCryptographyProvider.Decrypt(Convert.ToString(mRemoteNG.Settings.Default.DefaultPassword),
                GeneralAppInfo.EncryptionKey);
            txtCredentialsDomain.Text = Convert.ToString(mRemoteNG.Settings.Default.DefaultDomain);

            if (mRemoteNG.Settings.Default.ConfirmCloseConnection == (int) ConfirmCloseEnum.Never)
            {
                radCloseWarnNever.Checked = true;
            }
            else if (mRemoteNG.Settings.Default.ConfirmCloseConnection == (int) ConfirmCloseEnum.Exit)
            {
                radCloseWarnExit.Checked = true;
            }
            else if (mRemoteNG.Settings.Default.ConfirmCloseConnection == (int) ConfirmCloseEnum.Multiple)
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
            mRemoteNG.Settings.Default.SingleClickOnConnectionOpensIt = chkSingleClickOnConnectionOpensIt.Checked;
            mRemoteNG.Settings.Default.SingleClickSwitchesToOpenConnection = chkSingleClickOnOpenedConnectionSwitchesToIt.Checked;
            mRemoteNG.Settings.Default.SetHostnameLikeDisplayName = chkHostnameLikeDisplayName.Checked;

            mRemoteNG.Settings.Default.RdpReconnectionCount = (int) numRdpReconnectionCount.Value;

            mRemoteNG.Settings.Default.AutoSaveEveryMinutes = (int) numAutoSave.Value;
            if (mRemoteNG.Settings.Default.AutoSaveEveryMinutes > 0)
            {
                frmMain.Default.tmrAutoSave.Interval = Convert.ToInt32(mRemoteNG.Settings.Default.AutoSaveEveryMinutes*60000);
                frmMain.Default.tmrAutoSave.Enabled = true;
            }
            else
            {
                frmMain.Default.tmrAutoSave.Enabled = false;
            }

            if (radCredentialsNoInfo.Checked)
            {
                // ReSharper disable once StringLiteralTypo
                mRemoteNG.Settings.Default.EmptyCredentials = "noinfo";
            }
            else if (radCredentialsWindows.Checked)
            {
                mRemoteNG.Settings.Default.EmptyCredentials = "windows";
            }
            else if (radCredentialsCustom.Checked)
            {
                mRemoteNG.Settings.Default.EmptyCredentials = "custom";
            }

            mRemoteNG.Settings.Default.DefaultUsername = txtCredentialsUsername.Text;
            mRemoteNG.Settings.Default.DefaultPassword = LegacyRijndaelCryptographyProvider.Encrypt(txtCredentialsPassword.Text, GeneralAppInfo.EncryptionKey);
            mRemoteNG.Settings.Default.DefaultDomain = txtCredentialsDomain.Text;

            if (radCloseWarnAll.Checked)
            {
                mRemoteNG.Settings.Default.ConfirmCloseConnection = (int) ConfirmCloseEnum.All;
            }
            if (radCloseWarnMultiple.Checked)
            {
                mRemoteNG.Settings.Default.ConfirmCloseConnection = (int) ConfirmCloseEnum.Multiple;
            }
            if (radCloseWarnExit.Checked)
            {
                mRemoteNG.Settings.Default.ConfirmCloseConnection = (int) ConfirmCloseEnum.Exit;
            }
            if (radCloseWarnNever.Checked)
            {
                mRemoteNG.Settings.Default.ConfirmCloseConnection = (int) ConfirmCloseEnum.Never;
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