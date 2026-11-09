using System;
using System.Runtime.Versioning;
using mRemoteNG.App;
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
        private bool _profilePromptEnabledAtLoad;

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
            chkStartFullScreen.Text = Language.StartFullScreen;
            chkDisableRefocus.Text = Language.DisableRefocus;
            chkAskProfilePassword.Text = Language.AskProfilePasswordAtStartup;
            lblRegistrySettingsUsedInfo.Text = Language.OptionsCompanyPolicyMessage;
        }

        public override void LoadSettings()
        {
            chkReconnectOnStart.Checked = Properties.OptionsStartupExitPage.Default.OpenConsFromLastSession;
            chkSingleInstance.Checked = Properties.OptionsStartupExitPage.Default.SingleInstance;
            chkStartMinimized.Checked = Properties.OptionsStartupExitPage.Default.StartMinimized;
            chkStartFullScreen.Checked = Properties.OptionsStartupExitPage.Default.StartFullScreen;
            chkDisableRefocus.Checked = Properties.OptionsStartupExitPage.Default.DisableRefocus;
            _profilePromptEnabledAtLoad = Runtime.IsProfilePromptEnabled(Properties.OptionsDBsPage.Default);
            chkAskProfilePassword.Checked = _profilePromptEnabledAtLoad;
        }

        public override void SaveSettings()
        {
            base.SaveSettings();

            Properties.OptionsStartupExitPage.Default.OpenConsFromLastSession = chkReconnectOnStart.Checked;
            Properties.OptionsStartupExitPage.Default.SingleInstance = chkSingleInstance.Checked;
            Properties.OptionsStartupExitPage.Default.StartMinimized = chkStartMinimized.Checked;
            Properties.OptionsStartupExitPage.Default.StartFullScreen = chkStartFullScreen.Checked;
            Properties.OptionsStartupExitPage.Default.DisableRefocus = chkDisableRefocus.Checked;

            // only write when changed, so an untouched box keeps the defaults
            if (chkAskProfilePassword.Checked != _profilePromptEnabledAtLoad)
            {
                Runtime.SetProfilePromptEnabled(Properties.OptionsDBsPage.Default, chkAskProfilePassword.Checked);
                Properties.OptionsDBsPage.Default.Save();
                _profilePromptEnabledAtLoad = chkAskProfilePassword.Checked;
            }
        }

        public override void LoadRegistrySettings()
        {
            Type settingsType = typeof(OptRegistryStartupExitPage);
            RegistryLoader.RegistrySettings.TryGetValue(settingsType, out var settings);
            pageRegSettingsInstance = settings as OptRegistryStartupExitPage;

            // If registry settings don't exist, create a default instance to prevent null reference exceptions
            if (pageRegSettingsInstance == null)
            {
                pageRegSettingsInstance = new OptRegistryStartupExitPage();
                Logger.Instance.Log?.Debug("[StartupExitPage.LoadRegistrySettings] pageRegSettingsInstance was null, created default instance");
            }

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