using System;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public sealed partial class StartupExitPage
    {
        public StartupExitPage()
        {
            InitializeComponent();
            ApplyTheme();
            PageIcon = Resources.StartupExit_Icon;
        }

        public override string PageName
        {
            get => Language.strStartupExit;
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            chkSaveConsOnExit.Text = Language.strSaveConsOnExit;
            chkReconnectOnStart.Text = Language.strReconnectAtStartup;
            chkSingleInstance.Text = Language.strAllowOnlySingleInstance;
            chkStartMinimized.Text = Language.strStartMinimized;
            chkProperInstallationOfComponentsAtStartup.Text = Language.strCheckProperInstallationOfComponentsAtStartup;
        }

        public override void SaveSettings()
        {
            base.SaveSettings();

            Settings.Default.SaveConsOnExit = chkSaveConsOnExit.Checked;
            Settings.Default.OpenConsFromLastSession = chkReconnectOnStart.Checked;
            Settings.Default.SingleInstance = chkSingleInstance.Checked;
            Settings.Default.StartMinimized = chkStartMinimized.Checked;
            Settings.Default.StartupComponentsCheck = chkProperInstallationOfComponentsAtStartup.Checked;
        }

        private void StartupExitPage_Load(object sender, EventArgs e)
        {
            chkSaveConsOnExit.Checked = Settings.Default.SaveConsOnExit;
            chkReconnectOnStart.Checked = Settings.Default.OpenConsFromLastSession;
            chkSingleInstance.Checked = Settings.Default.SingleInstance;
            chkStartMinimized.Checked = Settings.Default.StartMinimized;
            chkProperInstallationOfComponentsAtStartup.Checked = Settings.Default.StartupComponentsCheck;
        }
    }
}