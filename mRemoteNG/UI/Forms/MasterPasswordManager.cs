using System;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Security;
using System.Windows.Forms;

using mRemoteNG.App;
using mRemoteNG.Security;
using mRemoteNG.Themes;
using mRemoteNG.Tools;

namespace mRemoteNG.UI.Forms
{
    [SupportedOSPlatform("windows")]
    public sealed class MasterPasswordManager : Form
    {
        private readonly Label _statusLabel = new();
        private readonly Label _descriptionLabel = new();
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
            Text = "Master Password";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(420, 170);
            Icon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.Key_16x);

            FontOverrider.FontOverride(this);

            _statusLabel.AutoSize = true;
            _statusLabel.Location = new Point(18, 18);
            _statusLabel.Font = new Font(Font, FontStyle.Bold);

            _descriptionLabel.AutoSize = false;
            _descriptionLabel.Location = new Point(18, 48);
            _descriptionLabel.Size = new Size(384, 42);

            _setOrChangeButton.Text = "Set Master Password";
            _setOrChangeButton.Location = new Point(18, 108);
            _setOrChangeButton.Size = new Size(140, 28);
            _setOrChangeButton.Click += SetOrChangeButton_Click;

            _removeButton.Text = "Remove";
            _removeButton.Location = new Point(166, 108);
            _removeButton.Size = new Size(90, 28);
            _removeButton.Click += RemoveButton_Click;

            _closeButton.Text = "Close";
            _closeButton.Location = new Point(312, 108);
            _closeButton.Size = new Size(90, 28);
            _closeButton.Click += (_, _) => Close();

            Controls.Add(_statusLabel);
            Controls.Add(_descriptionLabel);
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
                ? "Master password is enabled."
                : "Master password is not set.";
            _descriptionLabel.Text = isConfigured
                ? "The application will ask for the master password on startup before loading sensitive data."
                : "Set a master password to lock the application at startup and protect saved credentials with the same key.";
            _setOrChangeButton.Text = isConfigured
                ? "Change Master Password"
                : "Set Master Password";
            _removeButton.Enabled = isConfigured;
        }

        private void SetOrChangeButton_Click(object? sender, EventArgs e)
        {
            if (MasterPasswordService.IsConfigured && !PromptForCurrentMasterPassword())
                return;

            Optional<SecureString> newPassword = PromptForNewMasterPassword();
            if (!newPassword.Any())
                return;

            MasterPasswordService.SetMasterPassword(newPassword.First());
            RefreshState();
            MessageBox.Show(this, "Master password saved.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void RemoveButton_Click(object? sender, EventArgs e)
        {
            if (!MasterPasswordService.IsConfigured)
                return;

            if (!PromptForCurrentMasterPassword())
                return;

            DialogResult result = MessageBox.Show(
                this,
                "Remove the application master password?",
                Application.ProductName,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                return;

            MasterPasswordService.RemoveMasterPassword();
            RefreshState();
            MessageBox.Show(this, "Master password removed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool PromptForCurrentMasterPassword()
        {
            Optional<SecureString> currentPassword = MiscTools.PasswordDialog("Current Master Password", verify: false);
            if (!currentPassword.Any())
                return false;

            if (MasterPasswordService.TryUnlock(currentPassword.First()))
                return true;

            MessageBox.Show(this, "The current master password is invalid.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        private Optional<SecureString> PromptForNewMasterPassword()
        {
            Optional<SecureString> newPassword = MiscTools.PasswordDialog("Master Password", verify: false);
            if (!newPassword.Any())
                return Optional<SecureString>.Empty;

            if (newPassword.First().Length < 3)
            {
                MessageBox.Show(this, "Master password must be at least 3 characters long.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return Optional<SecureString>.Empty;
            }

            Optional<SecureString> verifyPassword = MiscTools.PasswordDialog("Verify Master Password", verify: false);
            if (!verifyPassword.Any())
                return Optional<SecureString>.Empty;

            if (newPassword.First().ConvertToUnsecureString() == verifyPassword.First().ConvertToUnsecureString())
                return newPassword;

            MessageBox.Show(this, "The entered passwords do not match.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return Optional<SecureString>.Empty;
        }
    }
}
