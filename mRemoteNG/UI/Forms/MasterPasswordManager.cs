using System;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Security;
using System.Windows.Forms;

using mRemoteNG.App;
using mRemoteNG.Properties;
using mRemoteNG.Security;
using mRemoteNG.Themes;
using mRemoteNG.Tools;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Forms
{
    [SupportedOSPlatform("windows")]
    public sealed class MasterPasswordManager : Form
    {
        private readonly Label _statusLabel = new();
        private readonly Label _descriptionLabel = new();
        private readonly Label _hintLabel = new();
        private readonly Button _setOrChangeButton = new();
        private readonly Button _removeButton = new();
        private readonly Button _closeButton = new();

        public MasterPasswordManager()
        {
            InitializeComponents();
            ApplyTheme();
            RefreshState();
        }

        private void InitializeComponents()
        {
            Text = Language.MasterPassword;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(420, 200);
            Icon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.Key_16x);

            FontOverrider.FontOverride(this);

            _statusLabel.AutoSize = true;
            _statusLabel.Location = new Point(18, 18);
            _statusLabel.Font = new Font(Font, FontStyle.Bold);

            _descriptionLabel.AutoSize = false;
            _descriptionLabel.Location = new Point(18, 48);
            _descriptionLabel.Size = new Size(384, 42);

            _hintLabel.AutoSize = true;
            _hintLabel.Location = new Point(18, 96);
            _hintLabel.ForeColor = System.Drawing.Color.Gray;

            _setOrChangeButton.Location = new Point(18, 138);
            _setOrChangeButton.Size = new Size(140, 28);
            _setOrChangeButton.Click += SetOrChangeButton_Click;

            _removeButton.Location = new Point(166, 138);
            _removeButton.Size = new Size(90, 28);
            _removeButton.Click += RemoveButton_Click;

            _closeButton.Text = Language._Close;
            _closeButton.Location = new Point(312, 138);
            _closeButton.Size = new Size(90, 28);
            _closeButton.Click += (_, _) => Close();

            Controls.Add(_statusLabel);
            Controls.Add(_descriptionLabel);
            Controls.Add(_hintLabel);
            Controls.Add(_setOrChangeButton);
            Controls.Add(_removeButton);
            Controls.Add(_closeButton);
        }

        private void ApplyTheme()
        {
            if (!ThemeManager.getInstance().ActiveAndExtended)
                return;

            ThemeInfo activeTheme = ThemeManager.getInstance().ActiveTheme;
            BackColor = activeTheme.ExtendedPalette.getColor("Dialog_Background");
            ForeColor = activeTheme.ExtendedPalette.getColor("Dialog_Foreground");
        }

        private void RefreshState()
        {
            bool isConfigured = MasterPasswordService.IsConfigured;

            _statusLabel.Text = isConfigured
                ? Language.MasterPasswordEnabled
                : Language.MasterPasswordNotSet;
            _descriptionLabel.Text = isConfigured
                ? Language.MasterPasswordEnabledDescription
                : Language.MasterPasswordNotSetDescription;
            _setOrChangeButton.Text = isConfigured
                ? Language.MasterPasswordChange
                : Language.MasterPasswordSet;
            _removeButton.Text = Language._Remove;
            _removeButton.Enabled = isConfigured;

            string hint = MasterPasswordService.Hint;
            _hintLabel.Text = isConfigured && !string.IsNullOrWhiteSpace(hint)
                ? $"{Language.MasterPasswordHint}: {hint}"
                : "";
        }

        private void SetOrChangeButton_Click(object? sender, EventArgs e)
        {
            if (MasterPasswordService.IsConfigured && !PromptForCurrentMasterPassword())
                return;

            Optional<SecureString> newPassword = PromptForNewMasterPassword();
            if (!newPassword.Any())
                return;

            string hint = PromptForHint();
            MasterPasswordService.SetMasterPassword(newPassword.First(), hint);
            RefreshState();
            MessageBox.Show(this, Language.MasterPasswordSaved, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void RemoveButton_Click(object? sender, EventArgs e)
        {
            if (!MasterPasswordService.IsConfigured)
                return;

            if (!PromptForCurrentMasterPassword())
                return;

            DialogResult result = MessageBox.Show(
                this,
                Language.MasterPasswordRemoveConfirm,
                Application.ProductName,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                return;

            MasterPasswordService.RemoveMasterPassword();
            RefreshState();
            MessageBox.Show(this, Language.MasterPasswordRemoved, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool PromptForCurrentMasterPassword()
        {
            Optional<SecureString> currentPassword = MiscTools.PasswordDialog(Language.MasterPasswordCurrent, verify: false);
            if (!currentPassword.Any())
                return false;

            if (MasterPasswordService.TryUnlock(currentPassword.First()))
                return true;

            MessageBox.Show(this, Language.MasterPasswordInvalid, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        private Optional<SecureString> PromptForNewMasterPassword()
        {
            Optional<SecureString> newPassword = MiscTools.PasswordDialog(Language.MasterPasswordNew, verify: false);
            if (!newPassword.Any())
                return Optional<SecureString>.Empty;

            if (newPassword.First().Length < 3)
            {
                MessageBox.Show(this, Language.MasterPasswordTooShort, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return Optional<SecureString>.Empty;
            }

            Optional<SecureString> verifyPassword = MiscTools.PasswordDialog(Language.MasterPasswordVerify, verify: false);
            if (!verifyPassword.Any())
                return Optional<SecureString>.Empty;

            if (newPassword.First().ConvertToUnsecureString() == verifyPassword.First().ConvertToUnsecureString())
                return newPassword;

            MessageBox.Show(this, Language.PasswordStatusMustMatch, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return Optional<SecureString>.Empty;
        }

        private string PromptForHint()
        {
            using FrmInputBox inputBox = new(Language.MasterPasswordHint, Language.MasterPasswordHintDescription, MasterPasswordService.Hint);
            return inputBox.ShowDialog(this) == DialogResult.OK ? inputBox.returnValue ?? string.Empty : string.Empty;
        }
    }
}
