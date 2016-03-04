using System.Collections.Generic;
using System;
using AxWFICALib;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using AxMSTSCLib;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using mRemoteNG.My;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;
using mRemoteNG.App;


namespace mRemoteNG.UI.Window
{
	public partial class Update : Base
	{
        #region Public Methods
		public Update(DockContent panel)
		{
			WindowType = Type.Update;
			DockPnl = panel;
			InitializeComponent();
			FontOverride(this);
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
			lblInstalledVersionLabel.Text = string.Format("{0}:", Language.strCurrentVersion);
			lblLatestVersion.Text = Language.strVersion;
			lblLatestVersionLabel.Text = string.Format("{0}:", Language.strAvailableVersion);
		}
				
		public void btnCheckForUpdate_Click(System.Object sender, EventArgs e)
		{
			CheckForUpdate();
		}
				
		public void btnDownload_Click(System.Object sender, EventArgs e)
		{
			DownloadUpdate();
		}
				
		public void pbUpdateImage_Click(System.Object sender, EventArgs e)
		{
			Uri linkUri = pbUpdateImage.Tag as Uri;
			if (linkUri == null || linkUri.IsFile || linkUri.IsUnc || linkUri.IsLoopback)
			{
				return ;
			}
			Process.Start(linkUri.ToString());
		}
        #endregion
				
        #region Private Fields
		private App.Update _appUpdate;
		private bool _isUpdateDownloadHandlerDeclared = false;
        #endregion
				
        #region Private Methods
		private void CheckForUpdate()
		{
			if (_appUpdate == null)
			{
				_appUpdate = new App.Update();
				_appUpdate.Load += _appUpdate.Update_Load;
			}
			else if (_appUpdate.IsGetUpdateInfoRunning)
			{
				return ;
			}
					
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
				AsyncCompletedEventHandler myDelegate = new AsyncCompletedEventHandler(GetUpdateInfoCompleted);
				Invoke(myDelegate, new object[] {sender, e});
				return ;
			}
					
			try
			{
				_appUpdate.GetUpdateInfoCompletedEvent -= GetUpdateInfoCompleted;
						
				lblInstalledVersion.Text = System.Convert.ToString(Application.Info.Version.ToString());
				lblInstalledVersion.Visible = true;
				lblInstalledVersionLabel.Visible = true;
				btnCheckForUpdate.Visible = true;
						
				if (e.Cancelled)
				{
					return ;
				}
				if (e.Error != null)
				{
					throw (e.Error);
				}
						
				if (_appUpdate.IsUpdateAvailable())
				{
					lblStatus.Text = Language.strUpdateAvailable;
					lblStatus.ForeColor = Color.OrangeRed;
					pnlUpdate.Visible = true;
							
					App.Update.UpdateInfo updateInfo = _appUpdate.CurrentUpdateInfo;
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
					lblStatus.Text = Language.strNoUpdateAvailable;
					lblStatus.ForeColor = Color.ForestGreen;
							
					if (_appUpdate.CurrentUpdateInfo != null)
					{
						App.Update.UpdateInfo updateInfo = _appUpdate.CurrentUpdateInfo;
						if (updateInfo.IsValid && updateInfo.Version != null)
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
				
		private void GetChangeLogCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
		{
			if (InvokeRequired)
			{
				AsyncCompletedEventHandler myDelegate = new AsyncCompletedEventHandler(GetChangeLogCompleted);
				Invoke(myDelegate, new object[] {sender, e});
				return ;
			}
					
			try
			{
				_appUpdate.GetChangeLogCompletedEvent -= GetChangeLogCompleted;
						
				if (e.Cancelled)
				{
					return ;
				}
				if (e.Error != null)
				{
					throw (e.Error);
				}
						
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
		private void DownloadUpdateProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
		{
			prgbDownload.Value = e.ProgressPercentage;
		}
				
		private void DownloadUpdateCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
		{
			try
			{
				btnDownload.Enabled = true;
				prgbDownload.Visible = false;
						
				if (e.Cancelled)
				{
					return ;
				}
				if (e.Error != null)
				{
					throw (e.Error);
				}
						
				if (MessageBox.Show(Language.strUpdateDownloadComplete, Language.strMenuCheckForUpdates, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK)
				{
					Shutdown.Quit(_appUpdate.CurrentUpdateInfo.UpdateFilePath);
					return ;
				}
				else
				{
					File.Delete(_appUpdate.CurrentUpdateInfo.UpdateFilePath);
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage(Language.strUpdateDownloadCompleteFailed, ex);
			}
		}
        #endregion
	}
}
