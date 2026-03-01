using System;
using System.Runtime.Versioning;
using System.Windows.Forms;
using Microsoft.Win32;
using mRemoteNG.App;
using mRemoteNG.Config.Settings.Registry;
using mRemoteNG.Properties;
using mRemoteNG.Resources.Language;
using mRemoteNG.Tools.WindowsRegistry;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    [SupportedOSPlatform("windows")]
    public sealed partial class StartupExitPage
    {
        #region Private Fields

        private OptRegistryStartupExitPage? pageRegSettingsInstance;

        private const string WindowsRunKey = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string WindowsRunValueName = "mRemoteNG";

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
            chkStartWithWindows.Text = Language.StartWithWindows;
            lblRegistrySettingsUsedInfo.Text = Language.OptionsCompanyPolicyMessage;
        }

        public override void LoadSettings()
        {
            chkReconnectOnStart.Checked = Properties.OptionsStartupExitPage.Default.OpenConsFromLastSession;
            chkSingleInstance.Checked = Properties.OptionsStartupExitPage.Default.SingleInstance;
            chkStartMinimized.Checked = Properties.OptionsStartupExitPage.Default.StartMinimized;
            chkStartFullScreen.Checked = Properties.OptionsStartupExitPage.Default.StartFullScreen;
            chkDisableRefocus.Checked = Properties.OptionsStartupExitPage.Default.DisableRefocus;
        }

        public override void SaveSettings()
        {
            base.SaveSettings();

            Properties.OptionsStartupExitPage.Default.OpenConsFromLastSession = chkReconnectOnStart.Checked;
            Properties.OptionsStartupExitPage.Default.SingleInstance = chkSingleInstance.Checked;
            Properties.OptionsStartupExitPage.Default.StartMinimized = chkStartMinimized.Checked;
            Properties.OptionsStartupExitPage.Default.StartFullScreen = chkStartFullScreen.Checked;
            Properties.OptionsStartupExitPage.Default.DisableRefocus = chkDisableRefocus.Checked;

            ApplyStartWithWindowsSetting(chkStartWithWindows.Checked);
        }

        private static bool IsStartWithWindowsEnabled()
        {
            try
            {
                using RegistryKey? key = Registry.CurrentUser.OpenSubKey(WindowsRunKey, false);
                return key?.GetValue(WindowsRunValueName) != null;
            }
            catch (Exception ex)
            {
                Logger.Instance.Log?.Warn($"[StartupExitPage] Could not read Windows startup registry entry: {ex.Message}");
                return false;
            }
        }

        private static void ApplyStartWithWindowsSetting(bool enable)
        {
            try
            {
                var winRegistry = new WinRegistry();
                if (enable)
                {
                    string executablePath = Application.ExecutablePath;
                    winRegistry.SetValue(RegistryHive.CurrentUser, WindowsRunKey, WindowsRunValueName, executablePath, RegistryValueKind.String);
                }
                else
                {
                    winRegistry.DeleteRegistryValue(RegistryHive.CurrentUser, WindowsRunKey, WindowsRunValueName);
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Log?.Warn($"[StartupExitPage] Could not update Windows startup registry entry: {ex.Message}");
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
            return (pageRegSettingsInstance?.OpenConnectionsFromLastSession.IsSet ?? false)
                || (pageRegSettingsInstance?.EnforceSingleApplicationInstance.IsSet ?? false)
                || (pageRegSettingsInstance?.StartupBehavior.IsSet ?? false);
        }

        private void StartupExitPage_Load(object sender, EventArgs e)
        {
            chkStartWithWindows.Checked = IsStartWithWindowsEnabled();
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