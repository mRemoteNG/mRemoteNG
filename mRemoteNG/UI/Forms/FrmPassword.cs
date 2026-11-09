using System;
using System.Security;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Security;
using mRemoteNG.Security.PasswordCreation;
using mRemoteNG.Themes;
using mRemoteNG.Tools;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Forms
{
    [SupportedOSPlatform("windows")]
    public partial class FrmPassword : IKeyProvider
    {
        private const string RevealGlyph = ""; // eye in Segoe MDL2 Assets
        private readonly string _passwordName;
        private SecureString _password = new();
        private readonly Button _btnShowPassword = new();
        private readonly Label _lblStrength = new();
        private ToolTip _toolTip;

        /// <summary>
        /// Shortest new password accepted; the shortest common PIN length.
        /// </summary>
        internal const int MinimumPasswordLength = 4;

        /// <summary>
        /// True while the password is shown in plain text. The verify box
        /// is not needed then, because the user can read what they typed.
        /// </summary>
        internal bool PasswordShown => !txtPassword.UseSystemPasswordChar;

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
            AddShowPasswordButton();
            AddStrengthLabel();
        }

        /// <summary>
        /// Dispaly a dialog box requesting that the user 
        /// enter their password.
        /// </summary>
        /// <returns></returns>
        public Optional<SecureString> GetKey() => GetKey(null);

        public Optional<SecureString> GetKey(IWin32Window? owner)
        {
            DialogResult dialog = owner == null
                ? ShowDialog()
                : ShowDialog(owner);
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
            pbLock.Image = display.ScaleImage(pbLock.Image);
            Height = tableLayoutPanel1.Height;

            if (NewPasswordMode)
            {
                txtPassword.Focus();
                return;
            }
            lblVerify.Visible = false;
            txtVerify.Visible = false;
            txtPassword.Focus();
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

        // Apply the dark/light title bar before the window is shown to avoid a white flash.
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ThemeManager.getInstance().ApplyThemeToTitleBar(this);
        }

        private void ApplyTheme()
        {
            ThemeManager.getInstance().ApplyThemeToTitleBar(this);
            if (!ThemeManager.getInstance().ActiveAndExtended)
                return;

            ThemeInfo activeTheme = ThemeManager.getInstance().ActiveTheme;

            BackColor = activeTheme.ExtendedPalette.getColor("Dialog_Background");
            ForeColor = activeTheme.ExtendedPalette.getColor("Dialog_Foreground");
        }

        private bool VerifyNewPassword()
        {
            if (txtPassword.Text.Length >= MinimumPasswordLength)
            {
                if (PasswordShown || txtPassword.Text == txtVerify.Text)
                    return true;
                ShowStatus(Language.PasswordStatusMustMatch);
                return false;
            }

            ShowStatus(Language.PasswordStatusTooShort);
            return false;
        }

        private void AddShowPasswordButton()
        {
            components ??= new System.ComponentModel.Container();
            _toolTip = new ToolTip(components);

            _btnShowPassword.Name = "btnShowPassword";
            _btnShowPassword.Text = RevealGlyph;
            _btnShowPassword.Font = new System.Drawing.Font("Segoe MDL2 Assets", 8.25F);
            _btnShowPassword.Dock = DockStyle.Right;
            _btnShowPassword.Width = txtPassword.ClientSize.Height + 4;
            _btnShowPassword.FlatStyle = FlatStyle.Flat;
            _btnShowPassword.FlatAppearance.BorderSize = 0;
            _btnShowPassword.Cursor = Cursors.Default;
            _btnShowPassword.TabStop = false;
            _btnShowPassword.Click += (_, _) => SetPasswordShown(!PasswordShown);
            txtPassword.Controls.Add(_btnShowPassword);

            // keep typed text out from under the button, also after the handle is recreated
            txtPassword.HandleCreated += (_, _) => SetRightMargin(txtPassword, _btnShowPassword.Width);
            txtPassword.BackColorChanged += (_, _) => _btnShowPassword.BackColor = txtPassword.BackColor;
            txtPassword.ForeColorChanged += (_, _) => _btnShowPassword.ForeColor = txtPassword.ForeColor;
            UpdateShowPasswordText();
        }

        internal void SetPasswordShown(bool shown)
        {
            txtPassword.UseSystemPasswordChar = !shown;
            lblVerify.Enabled = !shown;
            txtVerify.Enabled = !shown;
            UpdateShowPasswordText();
            HideStatus();
            txtPassword.Focus();
            txtPassword.SelectionStart = txtPassword.TextLength;
        }

        private void UpdateShowPasswordText()
        {
            string text = PasswordShown ? Language.HidePassword : Language.ShowPassword;
            _btnShowPassword.AccessibleName = text;
            _toolTip.SetToolTip(_btnShowPassword, text);
        }

        private void AddStrengthLabel()
        {
            _lblStrength.Name = "lblStrength";
            _lblStrength.AutoSize = true;
            _lblStrength.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _lblStrength.Visible = false;
            tableLayoutPanel1.Controls.Add(_lblStrength, 2, 3);
            txtPassword.TextChanged += (_, _) => UpdateStrength();
        }

        private void UpdateStrength()
        {
            if (!NewPasswordMode)
                return;

            string[] userInputs = { Environment.UserName, Environment.MachineName, "mRemoteNG" };
            PasswordStrengthResult result = PasswordStrengthMeter.Evaluate(txtPassword.Text, userInputs);
            _lblStrength.Visible = result.Strength != PasswordStrength.None;
            _lblStrength.Text = string.Format(Language.PasswordStrength, StrengthText(result.Strength));
            _lblStrength.ForeColor = StrengthColor(result.Strength);
            _toolTip.SetToolTip(_lblStrength, result.Advice);
        }

        private static string StrengthText(PasswordStrength strength) => strength switch
        {
            PasswordStrength.Weak => Language.PasswordStrengthWeak,
            PasswordStrength.Fair => Language.PasswordStrengthFair,
            PasswordStrength.Good => Language.PasswordStrengthGood,
            PasswordStrength.Strong => Language.PasswordStrengthStrong,
            _ => string.Empty
        };

        private static System.Drawing.Color StrengthColor(PasswordStrength strength) => strength switch
        {
            PasswordStrength.Weak => System.Drawing.Color.Red,
            PasswordStrength.Fair => System.Drawing.Color.DarkOrange,
            PasswordStrength.Good => System.Drawing.Color.OliveDrab,
            _ => System.Drawing.Color.ForestGreen
        };

        private static void SetRightMargin(TextBox textBox, int margin)
        {
            NativeMethods.SendMessage(textBox.Handle, (int)NativeMethods.EM_SETMARGINS,
                (IntPtr)NativeMethods.EC_RIGHTMARGIN, (IntPtr)(margin << 16));
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