using System;
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

            My.Settings.Default.SaveConsOnExit = chkSaveConsOnExit.Checked;
            My.Settings.Default.OpenConsFromLastSession = chkReconnectOnStart.Checked;
            My.Settings.Default.SingleInstance = chkSingleInstance.Checked;
            My.Settings.Default.StartupComponentsCheck = chkProperInstallationOfComponentsAtStartup.Checked;
		}
			
		public void StartupExitPage_Load(System.Object sender, EventArgs e)
		{
            chkSaveConsOnExit.Checked = My.Settings.Default.SaveConsOnExit;
            chkReconnectOnStart.Checked = My.Settings.Default.OpenConsFromLastSession;
            chkSingleInstance.Checked = My.Settings.Default.SingleInstance;
            chkProperInstallationOfComponentsAtStartup.Checked = My.Settings.Default.StartupComponentsCheck;
		}
	}
}
