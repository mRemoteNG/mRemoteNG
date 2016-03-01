using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using mRemoteNG.App.Info;
using mRemoteNG.My;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Tools;
using PSTaskDialog;

namespace mRemoteNG.Forms.OptionsPages
{
	public partial class AdvancedPage
	{
		#region "Public Methods"
		public override string PageName {
			get { return Language.strTabAdvanced; }
			set { }
		}

		public override void ApplyLanguage()
		{
			base.ApplyLanguage();

			lblSeconds.Text = Language.strLabelSeconds;
			lblMaximumPuttyWaitTime.Text = Language.strLabelPuttyTimeout;
			chkAutomaticReconnect.Text = Language.strCheckboxAutomaticReconnect;
			lblConfigurePuttySessions.Text = Language.strLabelPuttySessionsConfig;
			btnLaunchPutty.Text = Language.strButtonLaunchPutty;
			btnBrowseCustomPuttyPath.Text = Language.strButtonBrowse;
			chkUseCustomPuttyPath.Text = Language.strCheckboxPuttyPath;
			chkAutomaticallyGetSessionInfo.Text = Language.strAutomaticallyGetSessionInfo;
			chkWriteLogFile.Text = Language.strWriteLogFile;
			lblUVNCSCPort.Text = Language.strUltraVNCSCListeningPort;
			lblXulRunnerPath.Text = Language.strXULrunnerPath;
			btnBrowseXulRunnerPath.Text = Language.strButtonBrowse;
			chkEncryptCompleteFile.Text = Language.strEncryptCompleteConnectionFile;
		}

		public override void LoadSettings()
		{
			base.SaveSettings();

			chkWriteLogFile.Checked = mRemoteNG.My.Settings.WriteLogFile;
			chkEncryptCompleteFile.Checked = mRemoteNG.My.Settings.EncryptCompleteConnectionsFile;
			chkAutomaticallyGetSessionInfo.Checked = mRemoteNG.My.Settings.AutomaticallyGetSessionInfo;
			chkAutomaticReconnect.Checked = mRemoteNG.My.Settings.ReconnectOnDisconnect;
			numPuttyWaitTime.Value = mRemoteNG.My.Settings.MaxPuttyWaitTime;

			chkUseCustomPuttyPath.Checked = MySettingsProperty.Settings.UseCustomPuttyPath;
			txtCustomPuttyPath.Text = MySettingsProperty.Settings.CustomPuttyPath;
			SetPuttyLaunchButtonEnabled();

			numUVNCSCPort.Value = mRemoteNG.My.Settings.UVNCSCPort;

			txtXULrunnerPath.Text = mRemoteNG.My.Settings.XULRunnerPath;
		}

		public override void SaveSettings()
		{
			base.SaveSettings();

			mRemoteNG.My.Settings.WriteLogFile = chkWriteLogFile.Checked;
			mRemoteNG.My.Settings.EncryptCompleteConnectionsFile = chkEncryptCompleteFile.Checked;
			mRemoteNG.My.Settings.AutomaticallyGetSessionInfo = chkAutomaticallyGetSessionInfo.Checked;
			mRemoteNG.My.Settings.ReconnectOnDisconnect = chkAutomaticReconnect.Checked;

			bool puttyPathChanged = false;
			if (!(MySettingsProperty.Settings.CustomPuttyPath == txtCustomPuttyPath.Text)) {
				puttyPathChanged = true;
				MySettingsProperty.Settings.CustomPuttyPath = txtCustomPuttyPath.Text;
			}
			if (!(MySettingsProperty.Settings.UseCustomPuttyPath == chkUseCustomPuttyPath.Checked)) {
				puttyPathChanged = true;
				MySettingsProperty.Settings.UseCustomPuttyPath = chkUseCustomPuttyPath.Checked;
			}
			if (puttyPathChanged) {
				if (MySettingsProperty.Settings.UseCustomPuttyPath) {
					PuttyBase.PuttyPath = MySettingsProperty.Settings.CustomPuttyPath;
				} else {
					PuttyBase.PuttyPath = General.PuttyPath;
				}
				mRemoteNG.Config.Putty.Sessions.AddSessionsToTree();
			}

			mRemoteNG.My.Settings.MaxPuttyWaitTime = numPuttyWaitTime.Value;

			mRemoteNG.My.Settings.UVNCSCPort = numUVNCSCPort.Value;

			mRemoteNG.My.Settings.XULRunnerPath = txtXULrunnerPath.Text;
		}
		#endregion

		#region "Private Methods"
		#region "Event Handlers"
		private void chkUseCustomPuttyPath_CheckedChanged(object sender, EventArgs e)
		{
			txtCustomPuttyPath.Enabled = chkUseCustomPuttyPath.Checked;
			btnBrowseCustomPuttyPath.Enabled = chkUseCustomPuttyPath.Checked;
			SetPuttyLaunchButtonEnabled();
		}

		private void txtCustomPuttyPath_TextChanged(object sender, EventArgs e)
		{
			SetPuttyLaunchButtonEnabled();
		}

		private void btnBrowseCustomPuttyPath_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog()) {
				var _with1 = openFileDialog;
				_with1.Filter = string.Format("{0}|*.exe|{1}|*.*", Language.strFilterApplication, Language.strFilterAll);
				_with1.FileName = Path.GetFileName(General.PuttyPath);
				_with1.CheckFileExists = true;
				_with1.Multiselect = false;

				if (_with1.ShowDialog == DialogResult.OK) {
					txtCustomPuttyPath.Text = _with1.FileName;
					SetPuttyLaunchButtonEnabled();
				}
			}
		}

		private void btnLaunchPutty_Click(object sender, EventArgs e)
		{
			try {
				PuttyProcessController puttyProcess = new PuttyProcessController();
				string fileName = null;
				if (chkUseCustomPuttyPath.Checked) {
					fileName = txtCustomPuttyPath.Text;
				} else {
					fileName = General.PuttyPath;
				}
				puttyProcess.Start(fileName);
				puttyProcess.SetControlText("Button", "&Cancel", "&Close");
				puttyProcess.SetControlVisible("Button", "&Open", false);
				puttyProcess.WaitForExit();
			} catch (Exception ex) {
				cTaskDialog.MessageBox(Application.Info.ProductName, Language.strErrorCouldNotLaunchPutty, "", ex.Message, "", "", eTaskDialogButtons.OK, eSysIcons.Error, null);
			}
		}

		private void btnBrowseXulRunnerPath_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog oDlg = new FolderBrowserDialog();
			oDlg.ShowNewFolderButton = false;

			if (oDlg.ShowDialog() == DialogResult.OK) {
				txtXULrunnerPath.Text = oDlg.SelectedPath;
			}

			oDlg.Dispose();
		}
		#endregion

		private void SetPuttyLaunchButtonEnabled()
		{
			string puttyPath = null;
			if (chkUseCustomPuttyPath.Checked) {
				puttyPath = txtCustomPuttyPath.Text;
			} else {
				puttyPath = General.PuttyPath;
			}

			bool exists = false;
			try {
				exists = File.Exists(puttyPath);
			} catch {
			}

			if (exists) {
				lblConfigurePuttySessions.Enabled = true;
				btnLaunchPutty.Enabled = true;
			} else {
				lblConfigurePuttySessions.Enabled = false;
				btnLaunchPutty.Enabled = false;
			}
		}
		public AdvancedPage()
		{
			InitializeComponent();
		}
		#endregion
	}
}
