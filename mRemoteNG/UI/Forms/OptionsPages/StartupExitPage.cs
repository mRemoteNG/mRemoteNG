using System;
using System.Runtime.Versioning;
using mRemoteNG.Properties;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    [SupportedOSPlatform("windows")]
    public sealed partial class StartupExitPage
    {
        [SupportedOSPlatform("windows")]
        public StartupExitPage()
        {
            InitializeComponent();
            ApplyTheme();
            PageIcon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.StartupProject_16x);
        }

        public override string PageName
        {
            get => Language.StartupExit;
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            chkReconnectOnStart.Text = Language.ReconnectAtStartup;
            chkSingleInstance.Text = Language.AllowOnlySingleInstance;
            chkStartMinimized.Text = Language.StartMinimized;
        }

        public override void SaveSettings()
        {
            base.SaveSettings();

            Properties.OptionsStartupExitPage.Default.OpenConsFromLastSession = chkReconnectOnStart.Checked;
            Properties.OptionsStartupExitPage.Default.SingleInstance = chkSingleInstance.Checked;
            Properties.OptionsStartupExitPage.Default.StartMinimized = chkStartMinimized.Checked;
            Properties.OptionsStartupExitPage.Default.StartFullScreen = chkStartFullScreen.Checked;
        }

        private void StartupExitPage_Load(object sender, EventArgs e)
        {
            chkReconnectOnStart.Checked = Properties.OptionsStartupExitPage.Default.OpenConsFromLastSession;
            chkSingleInstance.Checked = Properties.OptionsStartupExitPage.Default.SingleInstance;
            chkStartMinimized.Checked = Properties.OptionsStartupExitPage.Default.StartMinimized;
            chkStartFullScreen.Checked = Properties.OptionsStartupExitPage.Default.StartFullScreen;
            ;
        }

        private void chkStartFullScreen_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStartFullScreen.Checked && chkStartMinimized.Checked)
            {
                chkStartMinimized.Checked = false;
            } 
        }

        private void chkStartMinimized_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStartMinimized.Checked && chkStartFullScreen.Checked)
            {
                chkStartFullScreen.Checked = false;
            }
        }
    }
}