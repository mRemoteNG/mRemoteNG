using System;
using mRemoteNG.App;
using mRemoteNG.Properties;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;
using mRemoteNG.Config.Settings.Registry;
using System.DirectoryServices;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    [SupportedOSPlatform("windows")]
    public sealed partial class CredentialsPage : OptionsPage
    {
        #region Private Fields
        private readonly OptRegistryCredentialsPage _RegistrySettings = new();
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
            lblRegistrySettingsUsedInfo.Text = Language.OptionsCompanyPolicyMessage;
        }

        public override void LoadSettings()
        {
            if (!_RegistrySettings.CredentialPageEnabled)
                return;

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
            // CredentialPageEnabled reg setting: enabled/default: true; Disabled: false.
            if (!_RegistrySettings.CredentialPageEnabled)
            {
                DisablePage();
                return;
            }

            // UseCredentials reg setting with validation:
            //  1. Is not set or valid, stop processing.
            //  2. Set the 'EmptyCredentials' option based on value
            //  3. Only proceed when "custom"
            if (!_RegistrySettings.UseCredentials.IsValid) { return; }
            else if (_RegistrySettings.UseCredentials.IsValid)
            {
                Properties.OptionsCredentialsPage.Default.EmptyCredentials = _RegistrySettings.UseCredentials.Value;

                switch (Properties.OptionsCredentialsPage.Default.EmptyCredentials)
                {
                    case "noinfo":
                        DisableControl(radCredentialsWindows);
                        DisableControl(radCredentialsCustom);
                        SetVisibilitySettingsUsedInfo();
                        return;
                    case "windows":
                        DisableControl(radCredentialsNoInfo);
                        DisableControl(radCredentialsCustom);
                        SetVisibilitySettingsUsedInfo();
                        return;
                    case "custom":
                        DisableControl(radCredentialsNoInfo);
                        DisableControl(radCredentialsWindows);
                        break;
                }
            }

            // ***
            // The following is only used when set to custom!

            // DefaultUsername reg setting: set DefaultUsername option based on value
            if (_RegistrySettings.DefaultUsername.IsSet)
            {
                Properties.OptionsCredentialsPage.Default.DefaultUsername = _RegistrySettings.DefaultUsername.Value;
                DisableControl(txtCredentialsUsername);
            }

            // DefaultPassword reg setting:
            //  1. Test decription works to prevents potential issues
            //  2. Set DefaultPassword option based on value
            //  3. Clears reg setting if fails
            if (_RegistrySettings.DefaultPassword.IsSet)
            {
                try
                {
                    LegacyRijndaelCryptographyProvider cryptographyProvider = new();
                    string decryptedPassword;
                    string defaultPassword = _RegistrySettings.DefaultPassword.Value;

                    decryptedPassword = cryptographyProvider.Decrypt(defaultPassword, Runtime.EncryptionKey);
                    Properties.OptionsCredentialsPage.Default.DefaultPassword = defaultPassword;
                    DisableControl(txtCredentialsPassword);
                }
                catch
                {
                    // Fire-and-forget: The DefaultPassword in the registry is not encrypted.
                    _RegistrySettings.DefaultPassword.Clear();
                }
            }

            // DefaultDomain reg setting: set DefaultDomain option based on value
            if (_RegistrySettings.DefaultDomain.IsSet)
            {
                Properties.OptionsCredentialsPage.Default.DefaultDomain = _RegistrySettings.DefaultDomain.Value;
                DisableControl(txtCredentialsDomain);
            }

            // UserViaAPIDefault reg setting: set UserViaAPIDefault option based on value
            if (_RegistrySettings.UserViaAPIDefault.IsSet)
            {
                Properties.OptionsCredentialsPage.Default.UserViaAPIDefault = _RegistrySettings.UserViaAPIDefault.Value;
                DisableControl(txtCredentialsUserViaAPI);
            }

            SetVisibilitySettingsUsedInfo();
        }

        /// <summary>
        /// Checks if any credantil registry settings are being used.
        /// </summary>
        /// <returns>
        /// True if any relevant registry settings are used; otherwise, false.
        /// </returns>
        public override bool ShowRegistrySettingsUsedInfo()
        {
            return !CommonRegistrySettings.AllowExportPasswords
                || !CommonRegistrySettings.AllowExportUsernames
                || !CommonRegistrySettings.AllowSavePasswords
                || !CommonRegistrySettings.AllowSaveUsernames
                || !_RegistrySettings.CredentialPageEnabled
                || _RegistrySettings.UseCredentials.IsValid;

            /* 
             * Checking these values is unnecessary because UseCredentials must be valid and set to Custom.
             * 
            ||_RegistrySettings.DefaultUsername.IsSet
            || _RegistrySettings.DefaultPassword.IsSet
            || _RegistrySettings.DefaultDomain.IsSet
            || _RegistrySettings.UserViaAPIDefault.IsSet;
            */
        }

        /// <summary>
        /// Disables the page by setting default values and disabling controls.
        /// </summary>
        public override void DisablePage()
        {
            Properties.OptionsCredentialsPage.Default.EmptyCredentials = "noinfo";
            radCredentialsWindows.Enabled = false;
            radCredentialsCustom.Enabled = false;

            txtCredentialsUsername.Enabled = false;
            txtCredentialsPassword.Enabled = false;
            txtCredentialsDomain.Enabled = false;
            txtCredentialsUserViaAPI.Enabled = false;

            SetVisibilitySettingsUsedInfo();
        }

        #region Event Handlers

        private void radCredentialsCustom_CheckedChanged(object sender, EventArgs e)
        {
            if (!_RegistrySettings.DefaultUsername.IsSet && CommonRegistrySettings.AllowSaveUsernames)
            {
                lblCredentialsUsername.Enabled = radCredentialsCustom.Checked;
                txtCredentialsUsername.Enabled = radCredentialsCustom.Checked;
            }

            if (!_RegistrySettings.DefaultPassword.IsSet && CommonRegistrySettings.AllowSavePasswords)
            {
                lblCredentialsPassword.Enabled = radCredentialsCustom.Checked;
                txtCredentialsPassword.Enabled = radCredentialsCustom.Checked;
            }

            if (!_RegistrySettings.DefaultDomain.IsSet)
            {
                lblCredentialsDomain.Enabled = radCredentialsCustom.Checked;
                txtCredentialsDomain.Enabled = radCredentialsCustom.Checked;
            }

            if (!_RegistrySettings.UserViaAPIDefault.IsSet && CommonRegistrySettings.AllowSaveUsernames)
            {
                lblCredentialsUserViaAPI.Enabled = radCredentialsCustom.Checked;
                txtCredentialsUserViaAPI.Enabled = radCredentialsCustom.Checked;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates the visibility of the information label indicating whether registry settings are used.
        /// </summary>
        private void SetVisibilitySettingsUsedInfo()
        {
            lblRegistrySettingsUsedInfo.Visible = ShowRegistrySettingsUsedInfo();
        }

        #endregion
    }
}