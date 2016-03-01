using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.My;

namespace mRemoteNG.Forms.OptionsPages
{
	public partial class StartupExitPage
	{
		public override string PageName {
			get { return Language.strStartupExit; }
			set { }
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

		private void StartupExitPage_Load(System.Object sender, EventArgs e)
		{
			chkSaveConsOnExit.Checked = Settings.SaveConsOnExit;
			chkReconnectOnStart.Checked = Settings.OpenConsFromLastSession;
			chkSingleInstance.Checked = Settings.SingleInstance;
			chkProperInstallationOfComponentsAtStartup.Checked = Settings.StartupComponentsCheck;
		}
		public StartupExitPage()
		{
			Load += StartupExitPage_Load;
			InitializeComponent();
		}
	}
}
