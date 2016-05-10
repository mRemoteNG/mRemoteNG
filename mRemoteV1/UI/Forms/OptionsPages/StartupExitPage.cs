using System;
using mRemoteNG.My;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public partial class StartupExitPage
    {
        public StartupExitPage()
        {
            InitializeComponent();
        }

        public override string PageName
        {
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

            mRemoteNG.Settings.Default.SaveConsOnExit = chkSaveConsOnExit.Checked;
            mRemoteNG.Settings.Default.OpenConsFromLastSession = chkReconnectOnStart.Checked;
            mRemoteNG.Settings.Default.SingleInstance = chkSingleInstance.Checked;
            mRemoteNG.Settings.Default.StartupComponentsCheck = chkProperInstallationOfComponentsAtStartup.Checked;
        }

        public void StartupExitPage_Load(object sender, EventArgs e)
        {
            chkSaveConsOnExit.Checked = mRemoteNG.Settings.Default.SaveConsOnExit;
            chkReconnectOnStart.Checked = mRemoteNG.Settings.Default.OpenConsFromLastSession;
            chkSingleInstance.Checked = mRemoteNG.Settings.Default.SingleInstance;
            chkProperInstallationOfComponentsAtStartup.Checked = mRemoteNG.Settings.Default.StartupComponentsCheck;
        }
    }
}