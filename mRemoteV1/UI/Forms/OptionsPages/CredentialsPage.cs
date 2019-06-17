﻿using System;
using mRemoteNG.App;
using mRemoteNG.Security.SymmetricEncryption;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public sealed partial class CredentialsPage : OptionsPage
    {
        public CredentialsPage()
        {
            InitializeComponent();
            ApplyTheme();
            PageIcon = Resources.Key_Icon;
        }

        public override string PageName
        {
            get => Language.Credentials;
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();
            lblDefaultCredentials.Text = Language.strEmptyUsernamePasswordDomainFields;
            radCredentialsNoInfo.Text = Language.strNoInformation;
            radCredentialsWindows.Text = Language.strMyCurrentWindowsCreds;
            radCredentialsCustom.Text = Language.strTheFollowing;
            lblCredentialsUsername.Text = Language.strLabelUsername;
            lblCredentialsPassword.Text = Language.strLabelPassword;
            lblCredentialsDomain.Text = Language.strLabelDomain;
        }

        public override void LoadSettings()
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (Settings.Default.EmptyCredentials)
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

            txtCredentialsUsername.Text = Settings.Default.DefaultUsername;
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            txtCredentialsPassword.Text =
                cryptographyProvider.Decrypt(Settings.Default.DefaultPassword, Runtime.EncryptionKey);
            txtCredentialsDomain.Text = Settings.Default.DefaultDomain;
        }

        public override void SaveSettings()
        {
            if (radCredentialsNoInfo.Checked)
            {
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
            Settings.Default.DefaultPassword =
                cryptographyProvider.Encrypt(txtCredentialsPassword.Text, Runtime.EncryptionKey);
            Settings.Default.DefaultDomain = txtCredentialsDomain.Text;
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