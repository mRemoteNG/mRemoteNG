// VBConversions Note: VB project level imports
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
// End of VB project level imports

using System.ComponentModel;
using mRemoteNG.My;
using mRemoteNG.App;
//using mRemoteNG.App.Info;
using mRemoteNG.Security;
using mRemoteNG.Tools;
using mRemoteNG.UI.Window;
using PSTaskDialog;


namespace mRemoteNG.Forms.OptionsPages
{
	public partial class UpdatesPage
		{
			public UpdatesPage()
			{
				InitializeComponent();
			}
#region Public Methods
public override string PageName
			{
				get
				{
					return Language.strTabUpdates;
				}
				set
				{
				}
			}
			
			public override void ApplyLanguage()
			{
				base.ApplyLanguage();
				
#if !PORTABLE
				lblUpdatesExplanation.Text = Language.strUpdateCheck;
#else
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
				
				chkCheckForUpdatesOnStartup.Checked = System.Convert.ToBoolean(My.Settings.Default.CheckForUpdatesOnStartup);
				cboUpdateCheckFrequency.Enabled = chkCheckForUpdatesOnStartup.Checked;
				cboUpdateCheckFrequency.Items.Clear();
				int nDaily = cboUpdateCheckFrequency.Items.Add(Language.strUpdateFrequencyDaily);
				int nWeekly = cboUpdateCheckFrequency.Items.Add(Language.strUpdateFrequencyWeekly);
				int nMonthly = cboUpdateCheckFrequency.Items.Add(Language.strUpdateFrequencyMonthly);
				if (My.Settings.Default.CheckForUpdatesFrequencyDays < 1)
				{
					chkCheckForUpdatesOnStartup.Checked = false;
					cboUpdateCheckFrequency.SelectedIndex = nDaily;
				} // Daily
				else if ((int) My.Settings.Default.CheckForUpdatesFrequencyDays == 1)
				{
					cboUpdateCheckFrequency.SelectedIndex = nDaily;
				} // Weekly
				else if ((int) My.Settings.Default.CheckForUpdatesFrequencyDays == 7)
				{
					cboUpdateCheckFrequency.SelectedIndex = nWeekly;
				} // Monthly
				else if ((int) My.Settings.Default.CheckForUpdatesFrequencyDays == 31)
				{
					cboUpdateCheckFrequency.SelectedIndex = nMonthly;
				}
				else
				{
					int nCustom = cboUpdateCheckFrequency.Items.Add(string.Format(Language.strUpdateFrequencyCustom, My.Settings.Default.CheckForUpdatesFrequencyDays));
					cboUpdateCheckFrequency.SelectedIndex = nCustom;
				}
				
				chkUseProxyForAutomaticUpdates.Checked = System.Convert.ToBoolean(My.Settings.Default.UpdateUseProxy);
				pnlProxyBasic.Enabled = System.Convert.ToBoolean(My.Settings.Default.UpdateUseProxy);
				txtProxyAddress.Text = System.Convert.ToString(My.Settings.Default.UpdateProxyAddress);
				numProxyPort.Value = System.Convert.ToDecimal(My.Settings.Default.UpdateProxyPort);
				
				chkUseProxyAuthentication.Checked = System.Convert.ToBoolean(My.Settings.Default.UpdateProxyUseAuthentication);
				pnlProxyAuthentication.Enabled = System.Convert.ToBoolean(My.Settings.Default.UpdateProxyUseAuthentication);
				txtProxyUsername.Text = System.Convert.ToString(My.Settings.Default.UpdateProxyAuthUser);
				txtProxyPassword.Text = Crypt.Decrypt(System.Convert.ToString(My.Settings.Default.UpdateProxyAuthPass), App.Info.General.EncryptionKey);
				
				btnTestProxy.Enabled = System.Convert.ToBoolean(My.Settings.Default.UpdateUseProxy);
				
#if PORTABLE
				foreach (Control Control in Controls)
				{
					if (Control != lblUpdatesExplanation)
					{
						Control.Visible = false;
					}
				}
#endif
			}
			
			public override void SaveSettings()
			{
				base.SaveSettings();
				
				My.Settings.Default.CheckForUpdatesOnStartup = chkCheckForUpdatesOnStartup.Checked;
				if (cboUpdateCheckFrequency.SelectedItem.ToString() == Language.strUpdateFrequencyDaily)
				{
					My.Settings.Default.CheckForUpdatesFrequencyDays = 1;
				}
				else if (cboUpdateCheckFrequency.SelectedItem.ToString() == Language.strUpdateFrequencyWeekly)
				{
					My.Settings.Default.CheckForUpdatesFrequencyDays = 7;
				}
				else if (cboUpdateCheckFrequency.SelectedItem.ToString() == Language.strUpdateFrequencyMonthly)
				{
					My.Settings.Default.CheckForUpdatesFrequencyDays = 31;
				}
				
				My.Settings.Default.UpdateUseProxy = chkUseProxyForAutomaticUpdates.Checked;
				My.Settings.Default.UpdateProxyAddress = txtProxyAddress.Text;
				My.Settings.Default.UpdateProxyPort = numProxyPort.Value;
				
				My.Settings.Default.UpdateProxyUseAuthentication = chkUseProxyAuthentication.Checked;
				My.Settings.Default.UpdateProxyAuthUser = txtProxyUsername.Text;
				My.Settings.Default.UpdateProxyAuthPass = Crypt.Encrypt(txtProxyPassword.Text, App.Info.General.EncryptionKey);
			}
#endregion
			
#region Private Fields
			private App.Update _appUpdate;
#endregion
			
#region Private Methods
#region Event Handlers
			public void chkCheckForUpdatesOnStartup_CheckedChanged(object sender, EventArgs e)
			{
				cboUpdateCheckFrequency.Enabled = chkCheckForUpdatesOnStartup.Checked;
			}
			
			public void btnUpdateCheckNow_Click(object sender, EventArgs e)
			{
				Runtime.Windows.Show(Type.Update);
			}
			
			public void chkUseProxyForAutomaticUpdates_CheckedChanged(object sender, EventArgs e)
			{
				pnlProxyBasic.Enabled = chkUseProxyForAutomaticUpdates.Checked;
				btnTestProxy.Enabled = chkUseProxyForAutomaticUpdates.Checked;
				
				if (chkUseProxyForAutomaticUpdates.Checked)
				{
					chkUseProxyAuthentication.Enabled = true;
					
					if (chkUseProxyAuthentication.Checked)
					{
						pnlProxyAuthentication.Enabled = true;
					}
				}
				else
				{
					chkUseProxyAuthentication.Enabled = false;
					pnlProxyAuthentication.Enabled = false;
				}
			}
			
			public void btnTestProxy_Click(object sender, EventArgs e)
			{
				if (_appUpdate != null)
				{
					if (_appUpdate.IsGetUpdateInfoRunning)
					{
						return ;
					}
				}
				
				_appUpdate = new App.Update();
				_appUpdate.Load += _appUpdate.Update_Load;
				_appUpdate.SetProxySettings(chkUseProxyForAutomaticUpdates.Checked, txtProxyAddress.Text, (int) numProxyPort.Value, chkUseProxyAuthentication.Checked, txtProxyUsername.Text, txtProxyPassword.Text);
				
				btnTestProxy.Enabled = false;
				btnTestProxy.Text = Language.strOptionsProxyTesting;
				
				_appUpdate.GetUpdateInfoCompletedEvent += GetUpdateInfoCompleted;
				
				_appUpdate.GetUpdateInfoAsync();
			}
			
			public void chkUseProxyAuthentication_CheckedChanged(object sender, EventArgs e)
			{
				if (chkUseProxyForAutomaticUpdates.Checked)
				{
					if (chkUseProxyAuthentication.Checked)
					{
						pnlProxyAuthentication.Enabled = true;
					}
					else
					{
						pnlProxyAuthentication.Enabled = false;
					}
				}
			}
#endregion
			
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
					
					btnTestProxy.Enabled = true;
					btnTestProxy.Text = Language.strButtonTestProxy;
					
					if (e.Cancelled)
					{
						return ;
					}
					if (e.Error != null)
					{
						throw (e.Error);
					}
					
					cTaskDialog.ShowCommandBox(this, System.Convert.ToString(Application.Info.ProductName), Language.strProxyTestSucceeded, "", Language.strButtonOK, false);
				}
				catch (Exception ex)
				{
					cTaskDialog.ShowCommandBox(this, System.Convert.ToString(Application.Info.ProductName), Language.strProxyTestFailed, Misc.GetExceptionMessageRecursive(ex), "", "", "", Language.strButtonOK, false, eSysIcons.Error, (PSTaskDialog.eSysIcons) 0);
				}
			}
#endregion
		}
}
