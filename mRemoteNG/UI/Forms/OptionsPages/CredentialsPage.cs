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
        private OptRegistryCredentialsPage? pageRegSettingsInstance;
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
            Type settingsType = typeof(OptRegistryCredentialsPage);
            RegistryLoader.RegistrySettings.TryGetValue(settingsType, out var settings);
            pageRegSettingsInstance = settings as OptRegistryCredentialsPage;

            // If registry settings don't exist, create a default instance to prevent null reference exceptions
            if (pageRegSettingsInstance == null)
            {
                pageRegSettingsInstance = new OptRegistryCredentialsPage();
                Logger.Instance.Log?.Debug("[CredentialsPage.LoadRegistrySettings] pageRegSettingsInstance was null, created default instance");
            }

            RegistryLoader.Cleanup(settingsType);

            // Show registry settings info if any common setting is used.
            lblRegistrySettingsUsedInfo.Visible =  CommonRegistrySettingsUsed();

            // Enforce AllowSavePasswords/AllowSaveUsernames regardless of UseCredentials
            if (!CommonRegistrySettings.AllowSavePasswords)
                DisableControl(txtCredentialsPassword);

            if (!CommonRegistrySettings.AllowSaveUsernames)
                DisableControl(txtCredentialsUsername);

            // UseCredentials reg setting must be set (and valid).
            if (!pageRegSettingsInstance.UseCredentials.IsValid)
                return;

            lblRegistrySettingsUsedInfo.Visible = true;

            // UseCredentials  reg setting:
            //   Disable the radio controls based on value.
            //   Only proceed when "custom".
            switch (pageRegSettingsInstance.UseCredentials.Value)
            {
                case "noinfo":
                    DisableControl(radCredentialsWindows);
                    DisableControl(radCredentialsCustom);
                    return;
                case "windows":
                    DisableControl(radCredentialsNoInfo);
                    DisableControl(radCredentialsCustom);
                    return;
                case "custom":
                    DisableControl(radCredentialsNoInfo);
                    DisableControl(radCredentialsWindows);
                    break;
                default:
                    return;
            }

            // ***
            // The following is only used when set to custom!
            //      Disable controls based on the registry settings.
            //
            if (pageRegSettingsInstance.DefaultUsername.IsSet || !CommonRegistrySettings.AllowSaveUsernames)
                DisableControl(txtCredentialsUsername);

            if (pageRegSettingsInstance.DefaultPassword.IsSet || !CommonRegistrySettings.AllowSavePasswords)
                DisableControl(txtCredentialsPassword);

            if (pageRegSettingsInstance.DefaultDomain.IsSet)
                DisableControl(txtCredentialsDomain);

            if (pageRegSettingsInstance.UserViaAPIDefault.IsSet)
                DisableControl(txtCredentialsUserViaAPI);
        }

        /// <summary>
        /// Checks if any registry common setting is used.
        /// </summary>
        private static bool CommonRegistrySettingsUsed()
        {
            return !CommonRegistrySettings.AllowExportPasswords
                || !CommonRegistrySettings.AllowExportUsernames
                || !CommonRegistrySettings.AllowSavePasswords
                || !CommonRegistrySettings.AllowSaveUsernames;
        }

        #region Event Handlers

        /// <summary>
        /// Handles the CheckedChanged event for the custom credentials radio button.
        /// Enables or disables credential input fields based on the state of the radio button 
        /// and the availability of saved settings in the registry.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event data containing information about the event.</param>
        private void radCredentialsCustom_CheckedChanged(object sender, EventArgs e)
        {
            if (pageRegSettingsInstance == null)
                return;

            if (!pageRegSettingsInstance.DefaultUsername.IsSet && pageRegSettingsInstance.DefaultUsernameEnabled
                && CommonRegistrySettings.AllowSaveUsernames)
            {
                lblCredentialsUsername.Enabled = radCredentialsCustom.Checked;
                txtCredentialsUsername.Enabled = radCredentialsCustom.Checked;
            }


            if (!pageRegSettingsInstance.DefaultPassword.IsSet && pageRegSettingsInstance.DefaultPasswordEnabled
                && CommonRegistrySettings.AllowSavePasswords)
            {
                lblCredentialsPassword.Enabled = radCredentialsCustom.Checked;
                txtCredentialsPassword.Enabled = radCredentialsCustom.Checked;
            }

            if (!pageRegSettingsInstance.DefaultDomain.IsSet)
            {
                lblCredentialsDomain.Enabled = radCredentialsCustom.Checked;
                txtCredentialsDomain.Enabled = radCredentialsCustom.Checked;
            }

            if (!pageRegSettingsInstance.UserViaAPIDefault.IsSet && pageRegSettingsInstance.DefaultUserViaAPIEnabled)
            {
                lblCredentialsUserViaAPI.Enabled = radCredentialsCustom.Checked;
                txtCredentialsUserViaAPI.Enabled = radCredentialsCustom.Checked;
            }
        }

        #endregion

    }
}