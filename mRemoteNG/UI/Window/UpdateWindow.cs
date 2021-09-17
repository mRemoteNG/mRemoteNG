using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Update;
using mRemoteNG.Messages;
using mRemoteNG.Themes;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Window
{
    public partial class UpdateWindow : BaseWindow
    {
        private AppUpdater _appUpdate;
        private bool _isUpdateDownloadHandlerDeclared;

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

        #endregion

        #region Form Stuff

        private void Update_Load(object sender, EventArgs e)
        {
            ApplyTheme();
            ThemeManager.getInstance().ThemeChanged += ApplyTheme;
            ApplyLanguage();
            CheckForUpdate();
        }

        private new void ApplyTheme()
        {
            if (!ThemeManager.getInstance().ActiveAndExtended) return;

            base.ApplyTheme();
            txtChangeLog.BackColor =
                ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            txtChangeLog.ForeColor =
                ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
        }

        private void ApplyLanguage()
        {
            Text = Language.CheckForUpdates;
            TabText = Language.CheckForUpdates;
            btnCheckForUpdate.Text = Language.CheckAgain;
            btnDownload.Text = Runtime.IsPortableEdition
                ? Language.Download
                : Language.DownloadAndInstall;
            lblChangeLogLabel.Text = Language.Changelog;
            lblInstalledVersion.Text = Language.Version;
            lblInstalledVersionLabel.Text = $"{Language.AvailableVersion}:";
            lblLatestVersion.Text = Language.Version;
            lblLatestVersionLabel.Text = $"{Language.AvailableVersion}:";
        }

        private void btnCheckForUpdate_Click(object sender, EventArgs e)
        {
            CheckForUpdate();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            DownloadUpdate();
        }

        private void pbUpdateImage_Click(object sender, EventArgs e)
        {
            var linkUri = pbUpdateImage.Tag as Uri;
            if (linkUri == null || linkUri.IsFile || linkUri.IsUnc || linkUri.IsLoopback)
            {
                return;
            }

            Process.Start(linkUri.ToString());
        }

        #endregion

        #region Private Methods

        private void CheckForUpdate()
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

            lblStatus.Text = Language.CheckForUpdates;
            lblStatus.ForeColor = SystemColors.WindowText;
            lblLatestVersionLabel.Visible = false;
            lblInstalledVersion.Visible = false;
            lblInstalledVersionLabel.Visible = false;
            lblLatestVersion.Visible = false;
            btnCheckForUpdate.Visible = false;

            SetVisibilityOfUpdateControls(false);

            _appUpdate.GetUpdateInfoCompletedEvent += GetUpdateInfoCompleted;

            _appUpdate.GetUpdateInfoAsync();
        }

        private void GetUpdateInfoCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (InvokeRequired)
            {
                var myDelegate = new AsyncCompletedEventHandler(GetUpdateInfoCompleted);
                Invoke(myDelegate, sender, e);
                return;
            }

            try
            {
                _appUpdate.GetUpdateInfoCompletedEvent -= GetUpdateInfoCompleted;

                lblInstalledVersion.Text = Application.ProductVersion;
                lblInstalledVersion.Visible = true;
                lblInstalledVersionLabel.Visible = true;
                btnCheckForUpdate.Visible = true;

                if (e.Cancelled)
                {
                    return;
                }

                if (e.Error != null)
                {
                    throw e.Error;
                }

                if (_appUpdate.IsUpdateAvailable())
                {
                    lblStatus.Text = Language.UpdateAvailable;
                    lblStatus.ForeColor = Color.OrangeRed;
                    SetVisibilityOfUpdateControls(true);

                    var updateInfo = _appUpdate.CurrentUpdateInfo;
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

                    _appUpdate.GetChangeLogCompletedEvent += GetChangeLogCompleted;
                    _appUpdate.GetChangeLogAsync();

                    btnDownload.Focus();
                }
                else
                {
                    lblStatus.Text = Language.NoUpdateAvailable;
                    lblStatus.ForeColor = Color.ForestGreen;

                    if (_appUpdate.CurrentUpdateInfo == null) return;
                    var updateInfo = _appUpdate.CurrentUpdateInfo;
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

        private void GetChangeLogCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (InvokeRequired)
            {
                var myDelegate = new AsyncCompletedEventHandler(GetChangeLogCompleted);
                Invoke(myDelegate, sender, e);
                return;
            }

            try
            {
                _appUpdate.GetChangeLogCompletedEvent -= GetChangeLogCompleted;

                if (e.Cancelled)
                    return;
                if (e.Error != null)
                    throw e.Error;

                txtChangeLog.Text = _appUpdate.ChangeLog.Replace("\n", Environment.NewLine);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector?.AddExceptionStackTrace(Language.UpdateGetChangeLogFailed, ex);
            }
        }

        private void DownloadUpdate()
        {
            try
            {
                btnDownload.Enabled = false;
                prgbDownload.Visible = true;
                prgbDownload.Value = 0;

                if (_isUpdateDownloadHandlerDeclared == false)
                {
                    _appUpdate.DownloadUpdateProgressChangedEvent += DownloadUpdateProgressChanged;
                    _appUpdate.DownloadUpdateCompletedEvent += DownloadUpdateCompleted;
                    _isUpdateDownloadHandlerDeclared = true;
                }

                _appUpdate.DownloadUpdateAsync();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector?.AddExceptionStackTrace(Language.UpdateDownloadFailed, ex);
            }
        }

        #endregion

        #region Events

        private void DownloadUpdateProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            prgbDownload.Value = e.ProgressPercentage;
        }

        private void DownloadUpdateCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                btnDownload.Enabled = true;
                prgbDownload.Visible = false;

                if (e.Cancelled)
                    return;
                if (e.Error != null)
                    throw e.Error;

                if (Runtime.IsPortableEdition)
                    MessageBox.Show(Language.UpdatePortableDownloadComplete, Language.CheckForUpdates,
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                {
                    if (MessageBox.Show(Language.UpdateDownloadComplete, Language.CheckForUpdates,
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
                Runtime.MessageCollector?.AddExceptionStackTrace(Language.UpdateDownloadCompleteFailed, ex);
                Runtime.MessageCollector?.AddMessage(MessageClass.ErrorMsg, ex.Message);
            }
        }

        #endregion
    }
}