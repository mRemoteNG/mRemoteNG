using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
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
        private readonly string? _passwordName;
        private SecureString _password = new();

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
        public FrmPassword(string? passwordName = null, bool newPasswordMode = true)
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
            DialogResult dialog = ShowDialog();
            return dialog == DialogResult.OK
                ? _password
                : Optional<SecureString>.Empty;
        }

        #region Event Handlers

        private void FrmPassword_Load(object sender, EventArgs e)
        {
            ApplyLanguage();
            ApplyTheme();
            DisplayProperties display = new();
            if (pbLock.Image is { } lockImage)
                pbLock.Image = display.ScaleImage(lockImage);

            if (NewPasswordMode)
            {
                pnlStrengthBar.Visible = true;
                lblStrength.Visible = true;
                lblRequirements.Visible = true;
                txtPassword.Focus();
            }
            else
            {
                pnlStrengthBar.Visible = false;
                lblStrength.Visible = false;
                lblRequirements.Visible = false;
                lblVerify.Visible = false;
                txtVerify.Visible = false;
                txtPassword.Focus();
            }

            ClientSize = new Size(ClientSize.Width, tableLayoutPanel1.PreferredSize.Height + Padding.Vertical);
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
            if (NewPasswordMode && !VerifyNewPassword())
                return;

            DialogResult = DialogResult.OK;
        }

        private void TxtPassword_TextChanged(object sender, EventArgs e)
        {
            HideStatus();

            if (NewPasswordMode)
                UpdateStrengthIndicator(txtPassword.Text);
        }

        #endregion

        #region Private Methods

        private void ApplyLanguage()
        {
            Text = string.IsNullOrEmpty(_passwordName)
                ? Language.TitlePassword
                : string.Format(CultureInfo.CurrentCulture, Language.TitlePasswordWithName, _passwordName);

            lblPassword.Text = Language.Password;
            lblVerify.Text = Language.Verify;
            btnCancel.Text = Language._Cancel;
            btnOK.Text = Language._Ok;
            lblRequirements.Text = Language.PasswordRequirements;
        }

        private void ApplyTheme()
        {
            if (!ThemeManager.getInstance().ActiveAndExtended)
                return;

            ThemeInfo activeTheme = ThemeManager.getInstance().ActiveTheme;

            if (activeTheme.ExtendedPalette is not { } palette)
                return;

            BackColor = palette.getColor("Dialog_Background");
            ForeColor = palette.getColor("Dialog_Foreground");
        }

        // ReSharper disable once UnusedMethodReturnValue.Local
        private bool VerifyNewPassword()
        {
            if (txtPassword.Text.Length < 8)
            {
                ShowStatus(Language.PasswordStatusTooShort);
                return false;
            }

            bool hasUpper = false, hasLower = false, hasDigit = false;
            foreach (char c in txtPassword.Text)
            {
                if (char.IsUpper(c)) hasUpper = true;
                else if (char.IsLower(c)) hasLower = true;
                else if (char.IsDigit(c)) hasDigit = true;
            }

            if (!hasUpper || !hasLower || !hasDigit)
            {
                ShowStatus(Language.PasswordStatusNeedsComplexity);
                return false;
            }

            if (txtPassword.Text != txtVerify.Text)
            {
                ShowStatus(Language.PasswordStatusMustMatch);
                return false;
            }

            return true;
        }

        private void UpdateStrengthIndicator(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                pnlStrengthFill.Width = 0;
                lblStrength.Text = "";
                return;
            }

            int score = CalculateStrengthScore(password);

            int barWidth = pnlStrengthBar.Width;
            Color barColor;
            string strengthText;

            if (score <= 1)
            {
                pnlStrengthFill.Width = barWidth / 4;
                barColor = Color.FromArgb(220, 53, 69);
                strengthText = Language.PasswordStrengthWeak;
            }
            else if (score == 2)
            {
                pnlStrengthFill.Width = barWidth / 2;
                barColor = Color.FromArgb(255, 165, 0);
                strengthText = Language.PasswordStrengthFair;
            }
            else if (score == 3)
            {
                pnlStrengthFill.Width = barWidth * 3 / 4;
                barColor = Color.FromArgb(0, 123, 255);
                strengthText = Language.PasswordStrengthGood;
            }
            else
            {
                pnlStrengthFill.Width = barWidth;
                barColor = Color.FromArgb(40, 167, 69);
                strengthText = Language.PasswordStrengthStrong;
            }

            pnlStrengthFill.BackColor = barColor;
            lblStrength.ForeColor = barColor;
            lblStrength.Text = strengthText;
        }

        private static int CalculateStrengthScore(string password)
        {
            int score = 0;

            if (password.Length >= 8) score++;
            if (password.Length >= 12) score++;

            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecial = password.Any(c => !char.IsLetterOrDigit(c));

            int charTypes = (hasUpper ? 1 : 0) + (hasLower ? 1 : 0) + (hasDigit ? 1 : 0) + (hasSpecial ? 1 : 0);
            if (charTypes >= 3) score++;
            if (charTypes >= 4) score++;

            return score;
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
