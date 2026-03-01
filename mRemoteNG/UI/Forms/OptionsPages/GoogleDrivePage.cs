using System;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Properties;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    [SupportedOSPlatform("windows")]
    public class GoogleDrivePage : OptionsPage
    {
        private CheckBox _chkEnable = null!;
        private TextBox _txtCredentials = null!;
        private TextBox _txtFileName = null!;
        private Button _btnBrowse = null!;
        private Button _btnTest = null!;
        private Label _lblStatus = null!;

        public GoogleDrivePage()
        {
            BuildUI();
            ApplyTheme();
            PageIcon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.SyncArrow_16x);
        }

        public override string PageName { get => "Google Drive"; set { } }

        private void BuildUI()
        {
            SuspendLayout();

            int y = 10;
            const int margin = 10;
            const int pageWidth = 470;

            var lblHelp = new Label
            {
                Text = "Automatically back up your connection file to Google Drive after each save.\r\n" +
                       "Requires a Google Drive API credentials file (JSON) from Google Cloud Console.",
                Location = new Point(margin, y),
                Size = new Size(pageWidth, 40),
                AutoSize = false
            };
            Controls.Add(lblHelp);
            y += 50;

            _chkEnable = new CheckBox
            {
                Text = "Enable Google Drive backup",
                Location = new Point(margin, y),
                AutoSize = true
            };
            _chkEnable.CheckedChanged += (s, e) => UpdateControlsState();
            Controls.Add(_chkEnable);
            y += 30;

            var lblCred = new Label
            {
                Text = "Credentials file (JSON):",
                Location = new Point(margin, y + 3),
                AutoSize = true
            };
            Controls.Add(lblCred);
            y += 22;

            _txtCredentials = new TextBox
            {
                Location = new Point(margin, y),
                Size = new Size(360, 23)
            };
            Controls.Add(_txtCredentials);

            _btnBrowse = new Button
            {
                Text = "Browse...",
                Location = new Point(margin + 365, y - 1),
                Size = new Size(80, 25)
            };
            _btnBrowse.Click += BtnBrowse_Click;
            Controls.Add(_btnBrowse);
            y += 35;

            var lblFile = new Label
            {
                Text = "Remote file name:",
                Location = new Point(margin, y + 3),
                AutoSize = true
            };
            Controls.Add(lblFile);
            y += 22;

            _txtFileName = new TextBox
            {
                Location = new Point(margin, y),
                Size = new Size(360, 23)
            };
            Controls.Add(_txtFileName);
            y += 35;

            _btnTest = new Button
            {
                Text = "Test Connection",
                Location = new Point(margin, y),
                Size = new Size(120, 25)
            };
            _btnTest.Click += BtnTest_Click;
            Controls.Add(_btnTest);

            _lblStatus = new Label
            {
                Text = "",
                Location = new Point(margin + 130, y + 5),
                AutoSize = true
            };
            Controls.Add(_lblStatus);

            Name = "GoogleDrivePage";
            Size = new Size(500, 400);
            ResumeLayout(false);
        }

        private void UpdateControlsState()
        {
            bool enabled = _chkEnable.Checked;
            _txtCredentials.Enabled = enabled;
            _btnBrowse.Enabled = enabled;
            _txtFileName.Enabled = enabled;
            _btnTest.Enabled = enabled;
        }

        public override void LoadSettings()
        {
            _chkEnable.Checked = OptionsGoogleDrivePage.Default.UseGoogleDrive;
            _txtCredentials.Text = OptionsGoogleDrivePage.Default.GoogleDriveCredentialsPath;
            _txtFileName.Text = OptionsGoogleDrivePage.Default.GoogleDriveFileName;
            UpdateControlsState();
        }

        public override void SaveSettings()
        {
            OptionsGoogleDrivePage.Default.UseGoogleDrive = _chkEnable.Checked;
            OptionsGoogleDrivePage.Default.GoogleDriveCredentialsPath = _txtCredentials.Text;
            OptionsGoogleDrivePage.Default.GoogleDriveFileName = _txtFileName.Text;
            OptionsGoogleDrivePage.Default.Save();
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Title = "Select Google Drive Credentials File",
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 1
            };
            if (dialog.ShowDialog() == DialogResult.OK)
                _txtCredentials.Text = dialog.FileName;
        }

        private async void BtnTest_Click(object sender, EventArgs e)
        {
            _btnTest.Enabled = false;
            _lblStatus.Text = "Testing...";
            _lblStatus.ForeColor = SystemColors.ControlText;

            // Update in-memory settings so TestConnectionAsync reads the current UI value
            OptionsGoogleDrivePage.Default.GoogleDriveCredentialsPath = _txtCredentials.Text;

            try
            {
                bool success = await GoogleDriveDataProvider.TestConnectionAsync();
                string? user = success ? await GoogleDriveDataProvider.GetAuthenticatedUserAsync() : null;

                if (success)
                {
                    _lblStatus.Text = user != null ? $"Connected as {user}" : "Connected";
                    _lblStatus.ForeColor = Color.Green;
                }
                else
                {
                    _lblStatus.Text = "Connection failed";
                    _lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                _lblStatus.Text = $"Error: {ex.Message}";
                _lblStatus.ForeColor = Color.Red;
            }
            finally
            {
                _btnTest.Enabled = _chkEnable.Checked;
            }
        }
    }
}
