using System;
using System.Security;
using System.Windows.Forms;
using mRemoteNG.Security;
using mRemoteNG.Themes;
using mRemoteNG.Tools;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Forms
{
    [SupportedOSPlatform("windows")]
    public partial class FrmPassword : IKeyProvider
    {
        private readonly string _passwordName;
        private SecureString _password = new SecureString();

        /// <summary>
        /// Puts the dialog into the New Password mode. An extra
        /// password box is shown which must match the first password
        /// to continue.
        /// </summary>
        private bool NewPasswordMode { get; }

        /// <summary>
        /// Creates a new password form for entering or setting a password.
        /// </summary>
        /// <param name="passwordName"></param>
        /// <param name="newPasswordMode">
        /// Puts the dialog into the New Password mode. An extra
        /// password box is shown which must match the first password
        /// to continue.
        /// </param>
        public FrmPassword(string passwordName = null, bool newPasswordMode = true)
        {
            InitializeComponent();
            _passwordName = passwordName;
            NewPasswordMode = newPasswordMode;
        }

        /// <summary>
        /// Dispaly a dialog box requesting that the user 
        /// enter their password.
        /// </summary>
        /// <returns></returns>
        public Optional<SecureString> GetKey()
        {
            var dialog = ShowDialog();
            return dialog == DialogResult.OK
                ? _password
                : Optional<SecureString>.Empty;
        }

        #region Event Handlers

        private void FrmPassword_Load(object sender, EventArgs e)
        {
            ApplyLanguage();
            ApplyTheme();
            var display = new DisplayProperties();
            pbLock.Image = display.ScaleImage(pbLock.Image);
            Height = tableLayoutPanel1.Height;

            if (NewPasswordMode) return;
            lblVerify.Visible = false;
            txtVerify.Visible = false;
        }

        private void PasswordForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _password = txtPassword.Text.ConvertToSecureString();
            txtPassword.Text = "";
            txtVerify.Text = "";
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (NewPasswordMode)
                VerifyNewPassword();

            DialogResult = DialogResult.OK;
        }

        private void TxtPassword_TextChanged(object sender, EventArgs e)
        {
            HideStatus();
        }

        #endregion

        #region Private Methods

        private void ApplyLanguage()
        {
            Text = string.IsNullOrEmpty(_passwordName)
                ? Language.TitlePassword
                : string.Format(Language.TitlePasswordWithName, _passwordName);

            lblPassword.Text = Language.Password;
            lblVerify.Text = Language.Verify;
            btnCancel.Text = Language._Cancel;
            btnOK.Text = Language._Ok;
        }

        private void ApplyTheme()
        {
            if (!ThemeManager.getInstance().ActiveAndExtended)
                return;

            var activeTheme = ThemeManager.getInstance().ActiveTheme;

            BackColor = activeTheme.ExtendedPalette.getColor("Dialog_Background");
            ForeColor = activeTheme.ExtendedPalette.getColor("Dialog_Foreground");
        }

        // ReSharper disable once UnusedMethodReturnValue.Local
        private bool VerifyNewPassword()
        {
            if (txtPassword.Text.Length >= 3)
            {
                if (txtPassword.Text == txtVerify.Text)
                    return true;
                ShowStatus(Language.PasswordStatusMustMatch);
                return false;
            }

            ShowStatus(Language.PasswordStatusTooShort);
            return false;
        }

        private void ShowStatus(string status)
        {
            lblStatus.Visible = true;
            lblStatus.Text = status;
        }

        private void HideStatus()
        {
            lblStatus.Visible = false;
        }

        #endregion
    }
}