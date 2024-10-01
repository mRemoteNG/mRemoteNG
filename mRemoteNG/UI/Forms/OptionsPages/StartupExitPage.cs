using System;
using System.Runtime.Versioning;
using mRemoteNG.Config.Settings.Registry;
using mRemoteNG.Properties;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    [SupportedOSPlatform("windows")]
    public sealed partial class StartupExitPage
    {
        #region Private Fields

        private OptRegistryStartupExitPage pageRegSettingsInstance;

        #endregion

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
            lblRegistrySettingsUsedInfo.Text = Language.OptionsCompanyPolicyMessage;
        }

        public override void SaveSettings()
        {
            base.SaveSettings();

            Properties.OptionsStartupExitPage.Default.OpenConsFromLastSession = chkReconnectOnStart.Checked;
            Properties.OptionsStartupExitPage.Default.SingleInstance = chkSingleInstance.Checked;
            Properties.OptionsStartupExitPage.Default.StartMinimized = chkStartMinimized.Checked;
            Properties.OptionsStartupExitPage.Default.StartFullScreen = chkStartFullScreen.Checked;
        }

        public override void LoadRegistrySettings()
        {
            Type settingsType = typeof(OptRegistryStartupExitPage);
            RegistryLoader.RegistrySettings.TryGetValue(settingsType, out var settings);
            pageRegSettingsInstance = settings as OptRegistryStartupExitPage;

            RegistryLoader.Cleanup(settingsType);

            // Disable Controls depending on the value ("None", "Minimized", or "FullScreen")
            if (pageRegSettingsInstance.StartupBehavior.IsSet)
            {
                switch (pageRegSettingsInstance.StartupBehavior.Value)
                {
                    case "None":
                        DisableControl(chkStartMinimized);
                        DisableControl(chkStartFullScreen);
                        break;
                    case "Minimized":
                        DisableControl(chkStartMinimized);
                        DisableControl(chkStartFullScreen);
                        break;
                    case "FullScreen":
                        DisableControl(chkStartMinimized);
                        DisableControl(chkStartFullScreen);
                        break;
                }
            }

            // ***
            // Disable controls based on the registry settings.
            //
            if (pageRegSettingsInstance.OpenConnectionsFromLastSession.IsSet)
                DisableControl(chkReconnectOnStart);

            if (pageRegSettingsInstance.EnforceSingleApplicationInstance.IsSet)
                DisableControl(chkSingleInstance);

            lblRegistrySettingsUsedInfo.Visible = ShowRegistrySettingsUsedInfo();
        }

        /// <summary>
        /// Checks if specific registry settings related to appearence page are used.
        /// </summary>
        private bool ShowRegistrySettingsUsedInfo()
        {
            return pageRegSettingsInstance.OpenConnectionsFromLastSession.IsSet
                || pageRegSettingsInstance.EnforceSingleApplicationInstance.IsSet
                || pageRegSettingsInstance.StartupBehavior.IsSet;
        }

        private void StartupExitPage_Load(object sender, EventArgs e)
        {
            chkReconnectOnStart.Checked = Properties.OptionsStartupExitPage.Default.OpenConsFromLastSession;
            chkSingleInstance.Checked = Properties.OptionsStartupExitPage.Default.SingleInstance;
            chkStartMinimized.Checked = Properties.OptionsStartupExitPage.Default.StartMinimized;
            chkStartFullScreen.Checked = Properties.OptionsStartupExitPage.Default.StartFullScreen;
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