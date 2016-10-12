using System;
using mRemoteNG.App;
using mRemoteNG.Config;
using mRemoteNG.Security.SymmetricEncryption;

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

            lblRDPConTimeout.Text = Language.strRDPOverallConnectionTimeout;

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
                Convert.ToBoolean(Settings.Default.SingleClickOnConnectionOpensIt);
            chkSingleClickOnOpenedConnectionSwitchesToIt.Checked =
                Convert.ToBoolean(Settings.Default.SingleClickSwitchesToOpenConnection);
            chkHostnameLikeDisplayName.Checked = Convert.ToBoolean(Settings.Default.SetHostnameLikeDisplayName);

            numRdpReconnectionCount.Value = Convert.ToDecimal(Settings.Default.RdpReconnectionCount);

            numRDPConTimeout.Value = Convert.ToDecimal(Settings.Default.ConRDPOverallConnectionTimeout);

            numAutoSave.Value = Convert.ToDecimal(Settings.Default.AutoSaveEveryMinutes);

            // ReSharper disable once StringLiteralTypo
            if (Settings.Default.EmptyCredentials == "noinfo")
            {
                radCredentialsNoInfo.Checked = true;
            }
            else if (Settings.Default.EmptyCredentials == "windows")
            {
                radCredentialsWindows.Checked = true;
            }
            else if (Settings.Default.EmptyCredentials == "custom")
            {
                radCredentialsCustom.Checked = true;
            }

            txtCredentialsUsername.Text = Convert.ToString(Settings.Default.DefaultUsername);
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            txtCredentialsPassword.Text = cryptographyProvider.Decrypt(Convert.ToString(Settings.Default.DefaultPassword), Runtime.EncryptionKey);
            txtCredentialsDomain.Text = Convert.ToString(Settings.Default.DefaultDomain);

            if (Settings.Default.ConfirmCloseConnection == (int) ConfirmCloseEnum.Never)
            {
                radCloseWarnNever.Checked = true;
            }
            else if (Settings.Default.ConfirmCloseConnection == (int) ConfirmCloseEnum.Exit)
            {
                radCloseWarnExit.Checked = true;
            }
            else if (Settings.Default.ConfirmCloseConnection == (int) ConfirmCloseEnum.Multiple)
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
            Settings.Default.SingleClickOnConnectionOpensIt = chkSingleClickOnConnectionOpensIt.Checked;
            Settings.Default.SingleClickSwitchesToOpenConnection = chkSingleClickOnOpenedConnectionSwitchesToIt.Checked;
            Settings.Default.SetHostnameLikeDisplayName = chkHostnameLikeDisplayName.Checked;

            Settings.Default.RdpReconnectionCount = (int) numRdpReconnectionCount.Value;

            Settings.Default.ConRDPOverallConnectionTimeout = (int) numRDPConTimeout.Value;

            Settings.Default.AutoSaveEveryMinutes = (int) numAutoSave.Value;
            if (Settings.Default.AutoSaveEveryMinutes > 0)
            {
                frmMain.Default.tmrAutoSave.Interval = Convert.ToInt32(Settings.Default.AutoSaveEveryMinutes*60000);
                frmMain.Default.tmrAutoSave.Enabled = true;
            }
            else
            {
                frmMain.Default.tmrAutoSave.Enabled = false;
            }

            if (radCredentialsNoInfo.Checked)
            {
                // ReSharper disable once StringLiteralTypo
                Settings.Default.EmptyCredentials = "noinfo";
            }
            else if (radCredentialsWindows.Checked)
            {
                Settings.Default.EmptyCredentials = "windows";
            }
            else if (radCredentialsCustom.Checked)
            {
                Settings.Default.EmptyCredentials = "custom";
            }

            Settings.Default.DefaultUsername = txtCredentialsUsername.Text;
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            Settings.Default.DefaultPassword = cryptographyProvider.Encrypt(txtCredentialsPassword.Text, Runtime.EncryptionKey);
            Settings.Default.DefaultDomain = txtCredentialsDomain.Text;

            if (radCloseWarnAll.Checked)
            {
                Settings.Default.ConfirmCloseConnection = (int) ConfirmCloseEnum.All;
            }
            if (radCloseWarnMultiple.Checked)
            {
                Settings.Default.ConfirmCloseConnection = (int) ConfirmCloseEnum.Multiple;
            }
            if (radCloseWarnExit.Checked)
            {
                Settings.Default.ConfirmCloseConnection = (int) ConfirmCloseEnum.Exit;
            }
            if (radCloseWarnNever.Checked)
            {
                Settings.Default.ConfirmCloseConnection = (int) ConfirmCloseEnum.Never;
            }

            Settings.Default.Save();
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
    }
}