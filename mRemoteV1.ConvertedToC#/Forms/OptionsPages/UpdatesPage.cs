using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using mRemoteNG.My;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Security;
using mRemoteNG.Tools;
using mRemoteNG.UI.Window;
using PSTaskDialog;

namespace mRemoteNG.Forms.OptionsPages
{
	public partial class UpdatesPage
	{
		#region "Public Methods"
		public override string PageName {
			get { return Language.strTabUpdates; }
			set { }
		}

		public override void ApplyLanguage()
		{
			base.ApplyLanguage();

			#if Not PORTABLE
			lblUpdatesExplanation.Text = Language.strUpdateCheck;
			#elif
			lblUpdatesExplanation.Text = Language.strUpdateCheckPortableEdition;
			#endif

			chkCheckForUpdatesOnStartup.Text = Language.strCheckForUpdatesOnStartup;
			btnUpdateCheckNow.Text = Language.strCheckNow;

			chkUseProxyForAutomaticUpdates.Text = Language.strCheckboxUpdateUseProxy;
			lblProxyAddress.Text = Language.strLabelAddress;
			lblProxyPort.Text = Language.strLabelPort;

			chkUseProxyAuthentication.Text = Language.strCheckboxProxyAuthentication;
			lblProxyUsername.Text = Language.strLabelUsername;
			lblProxyPassword.Text = Language.strLabelPassword;

			btnTestProxy.Text = Language.strButtonTestProxy;
		}

		public override void LoadSettings()
		{
			base.SaveSettings();

			chkCheckForUpdatesOnStartup.Checked = mRemoteNG.My.Settings.CheckForUpdatesOnStartup;
			cboUpdateCheckFrequency.Enabled = chkCheckForUpdatesOnStartup.Checked;
			cboUpdateCheckFrequency.Items.Clear();
			int nDaily = cboUpdateCheckFrequency.Items.Add(Language.strUpdateFrequencyDaily);
			int nWeekly = cboUpdateCheckFrequency.Items.Add(Language.strUpdateFrequencyWeekly);
			int nMonthly = cboUpdateCheckFrequency.Items.Add(Language.strUpdateFrequencyMonthly);
			switch (mRemoteNG.My.Settings.CheckForUpdatesFrequencyDays) {
				case  // ERROR: Case labels with binary operators are unsupported : LessThan
1:
					chkCheckForUpdatesOnStartup.Checked = false;
					cboUpdateCheckFrequency.SelectedIndex = nDaily;
					break;
				case 1:
					// Daily
					cboUpdateCheckFrequency.SelectedIndex = nDaily;
					break;
				case 7:
					// Weekly
					cboUpdateCheckFrequency.SelectedIndex = nWeekly;
					break;
				case 31:
					// Monthly
					cboUpdateCheckFrequency.SelectedIndex = nMonthly;
					break;
				default:
					int nCustom = cboUpdateCheckFrequency.Items.Add(string.Format(Language.strUpdateFrequencyCustom, mRemoteNG.My.Settings.CheckForUpdatesFrequencyDays));
					cboUpdateCheckFrequency.SelectedIndex = nCustom;
					break;
			}

			chkUseProxyForAutomaticUpdates.Checked = mRemoteNG.My.Settings.UpdateUseProxy;
			pnlProxyBasic.Enabled = mRemoteNG.My.Settings.UpdateUseProxy;
			txtProxyAddress.Text = mRemoteNG.My.Settings.UpdateProxyAddress;
			numProxyPort.Value = mRemoteNG.My.Settings.UpdateProxyPort;

			chkUseProxyAuthentication.Checked = mRemoteNG.My.Settings.UpdateProxyUseAuthentication;
			pnlProxyAuthentication.Enabled = mRemoteNG.My.Settings.UpdateProxyUseAuthentication;
			txtProxyUsername.Text = mRemoteNG.My.Settings.UpdateProxyAuthUser;
			txtProxyPassword.Text = Crypt.Decrypt(mRemoteNG.My.Settings.UpdateProxyAuthPass, General.EncryptionKey);

			btnTestProxy.Enabled = mRemoteNG.My.Settings.UpdateUseProxy;

			#if PORTABLE
			foreach (Control Control in Controls) {
				if (!object.ReferenceEquals(Control, lblUpdatesExplanation)) {
					Control.Visible = false;
				}
			}
			#endif
		}

		public override void SaveSettings()
		{
			base.SaveSettings();

			mRemoteNG.My.Settings.CheckForUpdatesOnStartup = chkCheckForUpdatesOnStartup.Checked;
			switch (cboUpdateCheckFrequency.SelectedItem.ToString()) {
				case Language.strUpdateFrequencyDaily:
					mRemoteNG.My.Settings.CheckForUpdatesFrequencyDays = 1;
					break;
				case Language.strUpdateFrequencyWeekly:
					mRemoteNG.My.Settings.CheckForUpdatesFrequencyDays = 7;
					break;
				case Language.strUpdateFrequencyMonthly:
					mRemoteNG.My.Settings.CheckForUpdatesFrequencyDays = 31;
					break;
			}

			mRemoteNG.My.Settings.UpdateUseProxy = chkUseProxyForAutomaticUpdates.Checked;
			mRemoteNG.My.Settings.UpdateProxyAddress = txtProxyAddress.Text;
			mRemoteNG.My.Settings.UpdateProxyPort = numProxyPort.Value;

			mRemoteNG.My.Settings.UpdateProxyUseAuthentication = chkUseProxyAuthentication.Checked;
			mRemoteNG.My.Settings.UpdateProxyAuthUser = txtProxyUsername.Text;
			mRemoteNG.My.Settings.UpdateProxyAuthPass = Crypt.Encrypt(txtProxyPassword.Text, General.EncryptionKey);
		}
		#endregion

		#region "Private Fields"
			#endregion
		private App.Update _appUpdate;

		#region "Private Methods"
		#region "Event Handlers"
		private void chkCheckForUpdatesOnStartup_CheckedChanged(object sender, EventArgs e)
		{
			cboUpdateCheckFrequency.Enabled = chkCheckForUpdatesOnStartup.Checked;
		}

		private void btnUpdateCheckNow_Click(object sender, EventArgs e)
		{
			Runtime.Windows.Show(Type.Update);
		}

		private void chkUseProxyForAutomaticUpdates_CheckedChanged(object sender, EventArgs e)
		{
			pnlProxyBasic.Enabled = chkUseProxyForAutomaticUpdates.Checked;
			btnTestProxy.Enabled = chkUseProxyForAutomaticUpdates.Checked;

			if (chkUseProxyForAutomaticUpdates.Checked) {
				chkUseProxyAuthentication.Enabled = true;

				if (chkUseProxyAuthentication.Checked) {
					pnlProxyAuthentication.Enabled = true;
				}
			} else {
				chkUseProxyAuthentication.Enabled = false;
				pnlProxyAuthentication.Enabled = false;
			}
		}

		private void btnTestProxy_Click(object sender, EventArgs e)
		{
			if (_appUpdate != null) {
				if (_appUpdate.IsGetUpdateInfoRunning)
					return;
			}

			_appUpdate = new App.Update();
			_appUpdate.SetProxySettings(chkUseProxyForAutomaticUpdates.Checked, txtProxyAddress.Text, numProxyPort.Value, chkUseProxyAuthentication.Checked, txtProxyUsername.Text, txtProxyPassword.Text);

			btnTestProxy.Enabled = false;
			btnTestProxy.Text = Language.strOptionsProxyTesting;

			_appUpdate.GetUpdateInfoCompletedEvent += GetUpdateInfoCompleted;

			_appUpdate.GetUpdateInfoAsync();
		}

		private void chkUseProxyAuthentication_CheckedChanged(object sender, EventArgs e)
		{
			if (chkUseProxyForAutomaticUpdates.Checked) {
				if (chkUseProxyAuthentication.Checked) {
					pnlProxyAuthentication.Enabled = true;
				} else {
					pnlProxyAuthentication.Enabled = false;
				}
			}
		}
		#endregion

		private void GetUpdateInfoCompleted(object sender, AsyncCompletedEventArgs e)
		{
			if (InvokeRequired) {
				AsyncCompletedEventHandler myDelegate = new AsyncCompletedEventHandler(GetUpdateInfoCompleted);
				Invoke(myDelegate, new object[] {
					sender,
					e
				});
				return;
			}

			try {
				_appUpdate.GetUpdateInfoCompletedEvent -= GetUpdateInfoCompleted;

				btnTestProxy.Enabled = true;
				btnTestProxy.Text = Language.strButtonTestProxy;

				if (e.Cancelled)
					return;
				if (e.Error != null)
					throw e.Error;

				cTaskDialog.ShowCommandBox(this, Application.Info.ProductName, Language.strProxyTestSucceeded, "", Language.strButtonOK, false);
			} catch (Exception ex) {
				cTaskDialog.ShowCommandBox(this, Application.Info.ProductName, Language.strProxyTestFailed, Misc.GetExceptionMessageRecursive(ex), "", "", "", Language.strButtonOK, false, eSysIcons.Error,
				0);
			}
		}
		public UpdatesPage()
		{
			InitializeComponent();
		}
		#endregion
	}
}
