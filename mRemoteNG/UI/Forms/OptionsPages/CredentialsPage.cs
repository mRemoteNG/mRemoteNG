using System;
using mRemoteNG.App;
using mRemoteNG.Properties;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;
using mRemoteNG.Config.Settings.Registry;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    [SupportedOSPlatform("windows")]
    public sealed partial class CredentialsPage : OptionsPage
    {
        #region Private Fields
        private readonly OptRegistryCredentialsPage _RegistrySettings = new();
        private bool _pageEnabled = true;

        #endregion

        public CredentialsPage()
        {
            InitializeComponent();
            ApplyTheme();
            PageIcon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.Key_16x);
        }

        public override string PageName
        {
            get => Language.Credentials;
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();
            lblDefaultCredentials.Text = Language.EmptyUsernamePasswordDomainFields;
            radCredentialsNoInfo.Text = Language.None;
            radCredentialsWindows.Text = Language.MyCurrentWindowsCreds;
            radCredentialsCustom.Text = Language.TheFollowing;
            lblCredentialsUsername.Text = Language.Username;
            lblCredentialsPassword.Text = Language.Password;
            lblCredentialsDomain.Text = Language.Domain;
            lblCredentialsAdminInfo.Text = Language.OptionsAdminInfo;
        }

        public override void LoadSettings()
        {
            if (!_pageEnabled) { return; }

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (Properties.OptionsCredentialsPage.Default.EmptyCredentials)
            {
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

            txtCredentialsUsername.Text = Properties.OptionsCredentialsPage.Default.DefaultUsername;
            LegacyRijndaelCryptographyProvider cryptographyProvider = new();
            txtCredentialsPassword.Text = cryptographyProvider.Decrypt(Properties.OptionsCredentialsPage.Default.DefaultPassword, Runtime.EncryptionKey);
            txtCredentialsDomain.Text = Properties.OptionsCredentialsPage.Default.DefaultDomain;
            txtCredentialsUserViaAPI.Text = Properties.OptionsCredentialsPage.Default.UserViaAPIDefault;
        }

        public override void SaveSettings()
        {
            if (radCredentialsNoInfo.Checked)
            {
                Properties.OptionsCredentialsPage.Default.EmptyCredentials = "noinfo";
            }
            else if (radCredentialsWindows.Checked)
            {
                Properties.OptionsCredentialsPage.Default.EmptyCredentials = "windows";
            }
            else if (radCredentialsCustom.Checked)
            {
                Properties.OptionsCredentialsPage.Default.EmptyCredentials = "custom";
            }

            Properties.OptionsCredentialsPage.Default.DefaultUsername = txtCredentialsUsername.Text;
            LegacyRijndaelCryptographyProvider cryptographyProvider = new();
            Properties.OptionsCredentialsPage.Default.DefaultPassword = cryptographyProvider.Encrypt(txtCredentialsPassword.Text, Runtime.EncryptionKey);
            Properties.OptionsCredentialsPage.Default.DefaultDomain = txtCredentialsDomain.Text;
            Properties.OptionsCredentialsPage.Default.UserViaAPIDefault = txtCredentialsUserViaAPI.Text;
        }

        public override void LoadRegistrySettings()
        {
            if (!CommonRegistrySettings.AllowModifyCredentialSettings)
            {
                DisablePage();
                return;
            }

            if (_RegistrySettings.UseCredentials.IsKeyPresent)
            {
                Properties.OptionsCredentialsPage.Default.EmptyCredentials = _RegistrySettings.UseCredentials.Value;

                switch (Properties.OptionsCredentialsPage.Default.EmptyCredentials)
                {
                    case "noinfo":
                        DisableControl(radCredentialsWindows);
                        DisableControl(radCredentialsCustom);
                        break;
                    case "windows":
                        DisableControl(radCredentialsNoInfo);
                        DisableControl(radCredentialsCustom);
                        break;
                    case "custom":
                        DisableControl(radCredentialsNoInfo);
                        DisableControl(radCredentialsWindows);
                        break;
                }
            }

            if (_RegistrySettings.UseCredentials.Value != "custom") { return; }


            if (!CommonRegistrySettings.AllowSaveUsernames)
            {
                Properties.OptionsCredentialsPage.Default.DefaultUsername = "";
                DisableControl(txtCredentialsUsername);
            }
            else if (_RegistrySettings.DefaultUsername.IsKeyPresent)
            {
                Properties.OptionsCredentialsPage.Default.DefaultUsername = _RegistrySettings.DefaultUsername.Value;
                DisableControl(txtCredentialsUsername);
            }

            if (!CommonRegistrySettings.AllowSavePasswords)
            {
                Properties.OptionsCredentialsPage.Default.DefaultPassword = "";
                DisableControl(txtCredentialsPassword);
            }
            //else if (_RegistrySettings.DefaultPassword.IsKeyPresent)
            //{
            //    Properties.OptionsCredentialsPage.Default.DefaultPassword = _RegistrySettings.DefaultPassword.Value;
            //    DisableControl(txtCredentialsPassword);
            //}

            if (_RegistrySettings.DefaultDomain.IsKeyPresent)
            {
                Properties.OptionsCredentialsPage.Default.DefaultDomain = _RegistrySettings.DefaultDomain.Value;
                DisableControl(txtCredentialsDomain);
            }

            if (!CommonRegistrySettings.AllowSaveUsernames)
            {
                Properties.OptionsCredentialsPage.Default.UserViaAPIDefault = "";
                DisableControl(txtCredentialsUserViaAPI);
            }
            else if (_RegistrySettings.UserViaAPIDefault.IsKeyPresent)
            {
                Properties.OptionsCredentialsPage.Default.UserViaAPIDefault = _RegistrySettings.UserViaAPIDefault.Value;
                DisableControl(txtCredentialsUserViaAPI);
            }

            lblCredentialsAdminInfo.Visible = ShowAdministratorInfo();
        }

        public override bool ShowAdministratorInfo()
        {
            return !CommonRegistrySettings.AllowModifyCredentialSettings
                || !CommonRegistrySettings.AllowExportPasswords
                || !CommonRegistrySettings.AllowExportUsernames
                || !CommonRegistrySettings.AllowSavePasswords
                || !CommonRegistrySettings.AllowSaveUsernames
                || _RegistrySettings.DefaultUsername.IsKeyPresent
                //|| _RegistrySettings.DefaultPassword.IsKeyPresent
                || _RegistrySettings.DefaultDomain.IsKeyPresent
                || _RegistrySettings.UserViaAPIDefault.IsKeyPresent;
        }

        private void radCredentialsCustom_CheckedChanged(object sender, EventArgs e)
        {
            if (!_RegistrySettings.DefaultUsername.IsKeyPresent && CommonRegistrySettings.AllowSaveUsernames)
            {
                lblCredentialsUsername.Enabled = radCredentialsCustom.Checked;
                txtCredentialsUsername.Enabled = radCredentialsCustom.Checked;
            }

            if (/*!_RegistrySettings.DefaultPassword.IsKeyPresent &&*/ CommonRegistrySettings.AllowSavePasswords)
            {
                lblCredentialsPassword.Enabled = radCredentialsCustom.Checked;
                txtCredentialsPassword.Enabled = radCredentialsCustom.Checked;
            }

            if (!_RegistrySettings.DefaultDomain.IsKeyPresent)
            {
                lblCredentialsDomain.Enabled = radCredentialsCustom.Checked;
                txtCredentialsDomain.Enabled = radCredentialsCustom.Checked;
            }

            if (!_RegistrySettings.UserViaAPIDefault.IsKeyPresent)
            {
                lblCredentialsUserViaAPI.Enabled = radCredentialsCustom.Checked;
                txtCredentialsUserViaAPI.Enabled = radCredentialsCustom.Checked;
            }
        }

        public override void DisablePage()
        {
            Properties.OptionsCredentialsPage.Default.EmptyCredentials = "noinfo";
            //radCredentialsNoInfo.Enabled = false;
            radCredentialsWindows.Enabled = false;
            radCredentialsCustom.Enabled = false;

            txtCredentialsUsername.Enabled = false;
            txtCredentialsPassword.Enabled = false;
            txtCredentialsDomain.Enabled = false;
            txtCredentialsUserViaAPI.Enabled = false;

            lblCredentialsAdminInfo.Visible = true;
            _pageEnabled = false;
        }
    }
}