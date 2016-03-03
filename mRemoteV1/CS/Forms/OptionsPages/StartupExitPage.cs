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

using mRemoteNG.My;


namespace mRemoteNG.Forms.OptionsPages
{
	public partial class StartupExitPage
		{
			public StartupExitPage()
			{
				InitializeComponent();
			}
public override string PageName
			{
				get
				{
					return Language.strStartupExit;
				}
				set
				{
				}
			}
			
			public override void ApplyLanguage()
			{
				base.ApplyLanguage();
				
				chkSaveConsOnExit.Text = Language.strSaveConsOnExit;
				chkReconnectOnStart.Text = Language.strReconnectAtStartup;
				chkSingleInstance.Text = Language.strAllowOnlySingleInstance;
				chkProperInstallationOfComponentsAtStartup.Text = Language.strCheckProperInstallationOfComponentsAtStartup;
			}
			
			public override void SaveSettings()
			{
				base.SaveSettings();
				
				Settings.SaveConsOnExit = chkSaveConsOnExit.Checked;
				Settings.OpenConsFromLastSession = chkReconnectOnStart.Checked;
				Settings.SingleInstance = chkSingleInstance.Checked;
				Settings.StartupComponentsCheck = chkProperInstallationOfComponentsAtStartup.Checked;
			}
			
			public void StartupExitPage_Load(System.Object sender, EventArgs e)
			{
				chkSaveConsOnExit.Checked = Settings.SaveConsOnExit;
				chkReconnectOnStart.Checked = Settings.OpenConsFromLastSession;
				chkSingleInstance.Checked = Settings.SingleInstance;
				chkProperInstallationOfComponentsAtStartup.Checked = Settings.StartupComponentsCheck;
			}
		}
}
