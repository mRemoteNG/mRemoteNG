using mRemoteNG.App;
using mRemoteNG.App.Update;
using mRemoteNG.Messages;
using mRemoteNG.Resources.Language;
using mRemoteNG.Themes;

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.UI.Window
{
    [SupportedOSPlatform("windows")]
    public partial class UpdateWindow : BaseWindow
    {
        private AppUpdater _appUpdate;
        //private bool _isUpdateDownloadHandlerDeclared;

        #region Public Methods

        public UpdateWindow() : this(new DockContent())
        {
        }

        public UpdateWindow(DockContent panel)
        {
            WindowType = WindowType.Update;
            DockPnl = panel;
            InitializeComponent();
            Icon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.RunUpdate_16x);
            FontOverrider.FontOverride(this);
        }

        /// <summary>
        /// Checks for updates and displays the results in the window.
        /// Call this method when you want to trigger an update check.
        /// </summary>
        public async Task PerformUpdateCheckAsync()
        {
            await CheckForUpdateAsync();
        }

        #endregion

        #region Form Stuff

        private async void Update_Load(object sender, EventArgs e)
        {
            ApplyTheme();
            ThemeManager.getInstance().ThemeChanged += ApplyTheme;
            ApplyLanguage();
        }

        private new void ApplyTheme()
        {
            if (!ThemeManager.getInstance().ActiveAndExtended) return;

            base.ApplyTheme();
            txtChangeLog.BackColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            txtChangeLog.ForeColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
        }

        private void ApplyLanguage()
        {
            Text = Language.MenuItem_CheckForUpdates;
            TabText = Language.MenuItem_CheckForUpdates;
            btnCheckForUpdate.Text = Language.CheckAgain;
            btnDownload.Text = Runtime.IsPortableEdition
                ? Language.Download
                : Language.DownloadAndInstall;
            lblChangeLogLabel.Text = Language.Changelog;
            lblInstalledVersion.Text = Language.Version;
            lblInstalledVersionLabel.Text = $"{Language.Version}:";
            lblLatestVersion.Text = Language.Version;
            lblLatestVersionLabel.Text = $"{Language.AvailableVersion}:";
        }

        private async void btnCheckForUpdate_Click(object sender, EventArgs e)
        {
            await CheckForUpdateAsync();
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            await DownloadUpdateAsync();
        }

        private void pbUpdateImage_Click(object sender, EventArgs e)
        {
            Uri linkUri = pbUpdateImage.Tag as Uri;
            if (linkUri == null || linkUri.IsFile || linkUri.IsUnc || linkUri.IsLoopback)
            {
                return;
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = linkUri.ToString(),
                UseShellExecute = true
            };
            Process.Start(startInfo);
        }

        #endregion

        #region Private Methods

        private async Task CheckForUpdateAsync()
        {
            if (_appUpdate == null)
            {
                _appUpdate = new AppUpdater();
                //_appUpdate.Load += _appUpdate.Update_Load;
            }
            else if (_appUpdate.IsGetUpdateInfoRunning)
            {
                return;
            }

            lblStatus.Text = Language.MenuItem_CheckForUpdates;
            lblStatus.ForeColor = SystemColors.WindowText;
            lblLatestVersionLabel.Visible = false;
            lblInstalledVersion.Visible = false;
            lblInstalledVersionLabel.Visible = false;
            lblLatestVersion.Visible = false;
            btnCheckForUpdate.Visible = false;

            SetVisibilityOfUpdateControls(false);

            try
            {
                await _appUpdate.GetUpdateInfoAsync();

                lblInstalledVersion.Text = Application.ProductVersion;
                lblInstalledVersion.Visible = true;
                lblInstalledVersionLabel.Visible = true;
                btnCheckForUpdate.Visible = true;

                if (_appUpdate.IsUpdateAvailable())
                {
                    lblStatus.Text = Language.UpdateAvailable;
                    lblStatus.ForeColor = Color.OrangeRed;
                    SetVisibilityOfUpdateControls(true);

                    UpdateInfo updateInfo = _appUpdate.CurrentUpdateInfo;
                    lblLatestVersion.Text = updateInfo.Version.ToString();
                    lblLatestVersionLabel.Visible = true;
                    lblLatestVersion.Visible = true;

                    if (updateInfo.ImageAddress == null || string.IsNullOrEmpty(updateInfo.ImageAddress.ToString()))
                    {
                        pbUpdateImage.Visible = false;
                    }
                    else
                    {
                        pbUpdateImage.ImageLocation = updateInfo.ImageAddress.ToString();
                        pbUpdateImage.Tag = updateInfo.ImageLinkAddress;
                        pbUpdateImage.Visible = true;
                    }

                    try
                    {
                        string changeLog = await _appUpdate.GetChangeLogAsync();
                        txtChangeLog.Text = changeLog.Replace("\n", Environment.NewLine);
                    }
                    catch (Exception ex)
                    {
                        Runtime.MessageCollector?.AddExceptionStackTrace(Language.UpdateGetChangeLogFailed, ex);
                    }

                    btnDownload.Focus();
                }
                else
                {
                    lblStatus.Text = Language.NoUpdateAvailable;
                    lblStatus.ForeColor = Color.ForestGreen;

                    if (_appUpdate.CurrentUpdateInfo == null) return;
                    UpdateInfo updateInfo = _appUpdate.CurrentUpdateInfo;
                    if (!updateInfo.IsValid || updateInfo.Version == null) return;
                    lblLatestVersion.Text = updateInfo.Version.ToString();
                    lblLatestVersionLabel.Visible = true;
                    lblLatestVersion.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = Language.CheckFailed;
                lblStatus.ForeColor = Color.OrangeRed;

                Runtime.MessageCollector?.AddExceptionStackTrace(Language.UpdateCheckCompleteFailed, ex);
            }
        }

        private void SetVisibilityOfUpdateControls(bool visible)
        {
            lblChangeLogLabel.Visible = visible;
            txtChangeLog.Visible = visible;
            btnDownload.Visible = visible;
            prgbDownload.Visible = visible;
        }

        private async Task DownloadUpdateAsync()
        {
            try
            {
                btnDownload.Enabled = false;
                prgbDownload.Visible = true;
                prgbDownload.Value = 0;

                await _appUpdate.DownloadUpdateAsync(new Progress<int>(progress => prgbDownload.Value = progress));

                btnDownload.Enabled = true;
                prgbDownload.Visible = false;

                if (Runtime.IsPortableEdition)
                    MessageBox.Show(Language.UpdatePortableDownloadComplete, Language.MenuItem_CheckForUpdates,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                {
                    if (MessageBox.Show(Language.UpdateDownloadComplete, Language.MenuItem_CheckForUpdates,
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        Shutdown.Quit(_appUpdate.CurrentUpdateInfo.UpdateFilePath);
                    }
                    else
                    {
                        File.Delete(_appUpdate.CurrentUpdateInfo.UpdateFilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector?.AddExceptionStackTrace(Language.UpdateDownloadFailed, ex);

                Runtime.MessageCollector?.AddExceptionStackTrace(Language.UpdateDownloadCompleteFailed, ex);
                Runtime.MessageCollector?.AddMessage(MessageClass.ErrorMsg, ex.Message);

            }
        }

        #endregion
    }
}