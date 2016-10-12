using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Update;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.UI.Window
{
    public partial class UpdateWindow : BaseWindow
    {
        #region Public Methods

        public UpdateWindow(DockContent panel)
        {
            WindowType = WindowType.Update;
            DockPnl = panel;
            InitializeComponent();
            Runtime.FontOverride(this);
        }

        #endregion

        #region Form Stuff

        public void Update_Load(object sender, EventArgs e)
        {
            ApplyLanguage();
            CheckForUpdate();
        }

        private void ApplyLanguage()
        {
            Text = Language.strMenuCheckForUpdates;
            TabText = Language.strMenuCheckForUpdates;
            btnCheckForUpdate.Text = Language.strCheckForUpdate;
            btnDownload.Text = Language.strDownloadAndInstall;
            lblChangeLogLabel.Text = Language.strLabelChangeLog;
            lblInstalledVersion.Text = Language.strVersion;
            lblInstalledVersionLabel.Text = $"{Language.strCurrentVersion}:";
            lblLatestVersion.Text = Language.strVersion;
            lblLatestVersionLabel.Text = $"{Language.strAvailableVersion}:";
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
            if ((linkUri == null) || linkUri.IsFile || linkUri.IsUnc || linkUri.IsLoopback)
                return;
            Process.Start(linkUri.ToString());
        }

        #endregion

        #region Private Fields

        private AppUpdater _appUpdate;
        private bool _isUpdateDownloadHandlerDeclared;

        #endregion

        #region Private Methods

        private void CheckForUpdate()
        {
            if (_appUpdate == null)
                _appUpdate = new AppUpdater();
            else if (_appUpdate.IsGetUpdateInfoRunning)
                return;

            lblStatus.Text = Language.strUpdateCheckingLabel;
            lblStatus.ForeColor = SystemColors.WindowText;
            lblLatestVersionLabel.Visible = false;
            lblInstalledVersion.Visible = false;
            lblInstalledVersionLabel.Visible = false;
            lblLatestVersion.Visible = false;
            btnCheckForUpdate.Visible = false;
            pnlUpdate.Visible = false;

            _appUpdate.GetUpdateInfoCompletedEvent += GetUpdateInfoCompleted;

            _appUpdate.GetUpdateInfoAsync();
        }

        private void GetUpdateInfoCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (InvokeRequired)
            {
                AsyncCompletedEventHandler myDelegate = GetUpdateInfoCompleted;
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
                    return;
                if (e.Error != null)
                    throw e.Error;

                if (_appUpdate.IsUpdateAvailable())
                {
                    lblStatus.Text = Language.strUpdateAvailable;
                    lblStatus.ForeColor = Color.OrangeRed;
                    pnlUpdate.Visible = true;

                    var updateInfo = _appUpdate.CurrentUpdateInfo;
                    lblLatestVersion.Text = updateInfo.Version.ToString();
                    lblLatestVersionLabel.Visible = true;
                    lblLatestVersion.Visible = true;

                    if ((updateInfo.ImageAddress == null) || string.IsNullOrEmpty(updateInfo.ImageAddress.ToString()))
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
                    lblStatus.Text = Language.strNoUpdateAvailable;
                    lblStatus.ForeColor = Color.ForestGreen;

                    if (_appUpdate.CurrentUpdateInfo != null)
                    {
                        var updateInfo = _appUpdate.CurrentUpdateInfo;
                        if (updateInfo.IsValid && (updateInfo.Version != null))
                        {
                            lblLatestVersion.Text = updateInfo.Version.ToString();
                            lblLatestVersionLabel.Visible = true;
                            lblLatestVersion.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = Language.strUpdateCheckFailedLabel;
                lblStatus.ForeColor = Color.OrangeRed;

                Runtime.MessageCollector.AddExceptionMessage(Language.strUpdateCheckCompleteFailed, ex);
            }
        }

        private void GetChangeLogCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (InvokeRequired)
            {
                AsyncCompletedEventHandler myDelegate = GetChangeLogCompleted;
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

                txtChangeLog.Text = _appUpdate.ChangeLog;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(Language.strUpdateGetChangeLogFailed, ex);
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
                Runtime.MessageCollector.AddExceptionMessage(Language.strUpdateDownloadFailed, ex);
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

                if (
                    MessageBox.Show(Language.strUpdateDownloadComplete, Language.strMenuCheckForUpdates,
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    Shutdown.Quit(_appUpdate.CurrentUpdateInfo.UpdateFilePath);
                else
                    File.Delete(_appUpdate.CurrentUpdateInfo.UpdateFilePath);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(Language.strUpdateDownloadCompleteFailed, ex);
            }
        }

        #endregion
    }
}