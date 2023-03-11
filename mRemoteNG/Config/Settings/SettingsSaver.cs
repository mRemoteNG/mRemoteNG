using System;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Tools;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Forms;

namespace mRemoteNG.Config.Settings
{
    [SupportedOSPlatform("windows")]
    public static class SettingsSaver
    {
        public static void SaveSettings(Control quickConnectToolStrip, ExternalToolsToolStrip externalToolsToolStrip, MultiSshToolStrip multiSshToolStrip, FrmMain frmMain)
        {
            try
            {
                var windowPlacement = new WindowPlacement(FrmMain.Default);
                if (frmMain.WindowState == FormWindowState.Minimized & windowPlacement.RestoreToMaximized)
                {
                    frmMain.Opacity = 0;
                    frmMain.WindowState = FormWindowState.Maximized;
                }

                Properties.App.Default.MainFormLocation = frmMain.Location;
                Properties.App.Default.MainFormSize = frmMain.Size;

                if (frmMain.WindowState != FormWindowState.Normal)
                {
                    Properties.App.Default.MainFormRestoreLocation = frmMain.RestoreBounds.Location;
                    Properties.App.Default.MainFormRestoreSize = frmMain.RestoreBounds.Size;
                }

                Properties.App.Default.MainFormState = frmMain.WindowState;

                if (frmMain.Fullscreen != null)
                {
                    Properties.App.Default.MainFormKiosk = frmMain.Fullscreen.Value;
                }

                Properties.App.Default.FirstStart = false;
                Properties.App.Default.ResetPanels = false;
                Properties.App.Default.ResetToolbars = false;

                SaveExternalAppsToolbarLocation(externalToolsToolStrip);
                SaveQuickConnectToolbarLocation(quickConnectToolStrip);
                SaveMultiSshToolbarLocation(multiSshToolStrip);

                Properties.App.Default.Save();
                Properties.AppUI.Default.Save();
                Properties.OptionsAdvancedPage.Default.Save();
                Properties.OptionsAppearancePage.Default.Save();
                Properties.OptionsBackupPage.Default.Save();
                Properties.OptionsConnectionsPage.Default.Save();
                Properties.OptionsCredentialsPage.Default.Save();
                Properties.OptionsDBsPage.Default.Save();
                Properties.OptionsNotificationsPage.Default.Save();
                Properties.OptionsSecurityPage.Default.Save();
                Properties.OptionsStartupExitPage.Default.Save();
                Properties.OptionsTabsPanelsPage.Default.Save();
                Properties.OptionsThemePage.Default.Save();
                Properties.OptionsUpdatesPage.Default.Save();
                
                Properties.Settings.Default.Save();

                SaveDockPanelLayout();
                SaveExternalApps();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Saving settings failed", ex);
            }
        }

        private static void SaveExternalAppsToolbarLocation(ExternalToolsToolStrip externalToolsToolStrip)
        {
            Properties.Settings.Default.ExtAppsTBLocation = externalToolsToolStrip.Location;
            Properties.Settings.Default.ExtAppsTBVisible = externalToolsToolStrip.Visible;
            Properties.Settings.Default.ExtAppsTBShowText = externalToolsToolStrip.CMenToolbarShowText.Checked;

            if (externalToolsToolStrip.Parent != null)
            {
                Properties.Settings.Default.ExtAppsTBParentDock = externalToolsToolStrip.Parent.Dock.ToString();
            }
        }

        private static void SaveQuickConnectToolbarLocation(Control quickConnectToolStrip)
        {
            Properties.Settings.Default.QuickyTBLocation = quickConnectToolStrip.Location;
            Properties.Settings.Default.QuickyTBVisible = quickConnectToolStrip.Visible;

            if (quickConnectToolStrip.Parent != null)
            {
                Properties.Settings.Default.QuickyTBParentDock = quickConnectToolStrip.Parent.Dock.ToString();
            }
        }

        private static void SaveMultiSshToolbarLocation(MultiSshToolStrip multiSshToolStrip)
        {
            Properties.Settings.Default.MultiSshToolbarLocation = multiSshToolStrip.Location;
            Properties.Settings.Default.MultiSshToolbarVisible = multiSshToolStrip.Visible;

            if (multiSshToolStrip.Parent != null)
            {
                Properties.Settings.Default.MultiSshToolbarParentDock = multiSshToolStrip.Parent.Dock.ToString();
            }
        }

        private static void SaveDockPanelLayout()
        {
            var panelLayoutXmlFilePath = SettingsFileInfo.SettingsPath + "\\" + SettingsFileInfo.LayoutFileName;
            var panelLayoutSaver = new DockPanelLayoutSaver(
                                                            new DockPanelLayoutSerializer(),
                                                            new FileDataProvider(panelLayoutXmlFilePath)
                                                           );
            panelLayoutSaver.Save();
        }

        private static void SaveExternalApps()
        {
            var externalAppsSaver = new ExternalAppsSaver();
            externalAppsSaver.Save(Runtime.ExternalToolsService.ExternalTools);
        }
    }
}