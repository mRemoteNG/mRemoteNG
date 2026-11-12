using System;
using System.ComponentModel;
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
        private const int StrengthDebounceMilliseconds = 300;
        private readonly Button _btnShowPassword = new();
        private readonly Label _lblStrength = new();
        private readonly System.Windows.Forms.Timer _strengthDebounceTimer = new();
        private readonly BackgroundWorker _strengthWorker = new();
        private ToolTip _toolTip;
        private StrengthEvaluationRequest _pendingStrengthEvaluation;
        private long _strengthVersion;
        private long _activeStrengthVersion;
        private bool _isClosing;

        private sealed record StrengthEvaluationRequest(long Version, string Password);
        private sealed record StrengthEvaluationResult(long Version, PasswordStrengthResult Result);

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

            if (!NewPasswordMode)
            {
                lblVerify.Visible = false;
                txtVerify.Visible = false;
            }
            FitToContents();
            txtPassword.Focus();
        }

        private void PasswordForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _isClosing = true;
            _strengthDebounceTimer.Stop();
            _strengthVersion++;
            _pendingStrengthEvaluation = null;
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
            components ??= new System.ComponentModel.Container();
            components.Add(_strengthDebounceTimer);
            components.Add(_strengthWorker);

            _strengthDebounceTimer.Interval = StrengthDebounceMilliseconds;
            _strengthDebounceTimer.Tick += (_, _) => QueueStrengthEvaluation();
            _strengthWorker.DoWork += StrengthWorker_DoWork;
            _strengthWorker.RunWorkerCompleted += StrengthWorker_RunWorkerCompleted;

            _lblStrength.Name = "lblStrength";
            _lblStrength.AutoSize = true;
            _lblStrength.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _lblStrength.Visible = false;
            tableLayoutPanel1.Controls.Add(_lblStrength, 2, 3);
            txtPassword.TextChanged += (_, _) => ScheduleStrengthUpdate();
        }

        private void ScheduleStrengthUpdate()
        {
            if (!NewPasswordMode || _isClosing)
                return;

            _strengthDebounceTimer.Stop();
            _pendingStrengthEvaluation = null;
            _strengthVersion++;
            string password = txtPassword.Text;

            if (password.Length == 0)
            {
                _lblStrength.Visible = false;
                _toolTip.SetToolTip(_lblStrength, string.Empty);
                return;
            }

            if (password.Length > PasswordStrengthMeter.MaximumPasswordLength)
            {
                ShowStrengthResult(new PasswordStrengthResult(PasswordStrength.NotEstimated, string.Empty));
                return;
            }

            _lblStrength.Visible = false;
            _strengthDebounceTimer.Start();
        }

        private void QueueStrengthEvaluation()
        {
            _strengthDebounceTimer.Stop();
            StrengthEvaluationRequest request = new(_strengthVersion, txtPassword.Text);
            if (_strengthWorker.IsBusy)
            {
                _pendingStrengthEvaluation = request;
                return;
            }

            StartStrengthEvaluation(request);
        }

        private void StartStrengthEvaluation(StrengthEvaluationRequest request)
        {
            _activeStrengthVersion = request.Version;
            _strengthWorker.RunWorkerAsync(request);
        }

        private void StrengthWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            StrengthEvaluationRequest request = (StrengthEvaluationRequest)e.Argument;
            string[] userInputs = { Environment.UserName, Environment.MachineName, "mRemoteNG" };
            PasswordStrengthResult result = PasswordStrengthMeter.Evaluate(request.Password, userInputs);
            e.Result = new StrengthEvaluationResult(request.Version, result);
        }

        private void StrengthWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Logger.Instance.Log.Error($"Unable to estimate password strength ({e.Error.GetType().Name}).");
                if (_activeStrengthVersion == _strengthVersion && !_isClosing && !IsDisposed)
                    ShowStrengthUnavailable();
            }
            else if (!e.Cancelled && !_isClosing && !IsDisposed)
            {
                StrengthEvaluationResult result = (StrengthEvaluationResult)e.Result;
                if (result.Version == _strengthVersion)
                    ShowStrengthResult(result.Result);
            }

            StrengthEvaluationRequest pending = _pendingStrengthEvaluation;
            _pendingStrengthEvaluation = null;
            if (pending != null && pending.Version == _strengthVersion && !_isClosing && !IsDisposed)
                StartStrengthEvaluation(pending);
        }

        private void ShowStrengthUnavailable()
        {
            _lblStrength.Text = string.Format(Language.PasswordStrength, Language.PasswordStrengthUnavailable);
            _lblStrength.ForeColor = StrengthColor(PasswordStrength.NotEstimated);
            _lblStrength.Visible = true;
            _toolTip.SetToolTip(_lblStrength, Language.PasswordStrengthUnavailable);
        }

        private void ShowStrengthResult(PasswordStrengthResult result)
        {
            _lblStrength.Visible = result.Strength != PasswordStrength.None;
            _lblStrength.Text = result.Strength == PasswordStrength.NotEstimated
                ? string.Format(Language.PasswordStrength, Language.PasswordStrengthNotEstimated)
                : string.Format(Language.PasswordStrength, StrengthText(result.Strength));
            _lblStrength.ForeColor = StrengthColor(result.Strength);
            _toolTip.SetToolTip(_lblStrength, result.Strength == PasswordStrength.NotEstimated
                ? Language.PasswordStrengthNotEstimated
                : result.Advice);
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
            PasswordStrength.NotEstimated => System.Drawing.Color.Gray,
            _ => System.Drawing.Color.ForestGreen
        };

        private static void SetRightMargin(TextBox textBox, int margin)
        {
            NativeMethods.SendMessage(textBox.Handle, (int)NativeMethods.EM_SETMARGINS,
                (IntPtr)NativeMethods.EC_RIGHTMARGIN, (IntPtr)(margin << 16));
        }

        private void ShowStatus(string status)
        {
            lblStatus.Text = status;
            lblStatus.Visible = true;
            FitToContents();
        }

        private void HideStatus()
        {
            if (!lblStatus.Visible)
                return;
            lblStatus.Visible = false;
            FitToContents();
        }

        /// <summary>
        /// Sizes the window to its contents, so a status message that wraps
        /// never squeezes the OK and Cancel buttons.
        /// </summary>
        private void FitToContents()
        {
            int height = tableLayoutPanel1.GetPreferredSize(new System.Drawing.Size(ClientSize.Width, 0)).Height;
            ClientSize = new System.Drawing.Size(ClientSize.Width, height);
        }

        #endregion
    }
}