using System;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Tools;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Forms;

namespace mRemoteNG.Config.Settings
{
    public static class SettingsSaver
    {
        public static void SaveSettings(Control quickConnectToolStrip, ExternalToolsToolStrip externalToolsToolStrip, FrmMain frmMain)
        {
            try
            {
                var windowPlacement = new WindowPlacement(FrmMain.Default);
                if (frmMain.WindowState == FormWindowState.Minimized & windowPlacement.RestoreToMaximized)
                {
                    frmMain.Opacity = 0;
                    frmMain.WindowState = FormWindowState.Maximized;
                }

                mRemoteNG.Settings.Default.MainFormLocation = frmMain.Location;
                mRemoteNG.Settings.Default.MainFormSize = frmMain.Size;

                if (frmMain.WindowState != FormWindowState.Normal)
                {
                    mRemoteNG.Settings.Default.MainFormRestoreLocation = frmMain.RestoreBounds.Location;
                    mRemoteNG.Settings.Default.MainFormRestoreSize = frmMain.RestoreBounds.Size;
                }

                mRemoteNG.Settings.Default.MainFormState = frmMain.WindowState;

                if (frmMain.Fullscreen != null)
                {
                    mRemoteNG.Settings.Default.MainFormKiosk = frmMain.Fullscreen.Value;
                }

                mRemoteNG.Settings.Default.FirstStart = false;
                mRemoteNG.Settings.Default.ResetPanels = false;
                mRemoteNG.Settings.Default.ResetToolbars = false;
                mRemoteNG.Settings.Default.NoReconnect = false;

                mRemoteNG.Settings.Default.ExtAppsTBLocation = externalToolsToolStrip.Location;
                if (externalToolsToolStrip.Parent != null)
                {
                    mRemoteNG.Settings.Default.ExtAppsTBParentDock = externalToolsToolStrip.Parent.Dock.ToString();
                }
                mRemoteNG.Settings.Default.ExtAppsTBVisible = externalToolsToolStrip.Visible;
                mRemoteNG.Settings.Default.ExtAppsTBShowText = externalToolsToolStrip.CMenToolbarShowText.Checked;

                mRemoteNG.Settings.Default.QuickyTBLocation = quickConnectToolStrip.Location;
                if (quickConnectToolStrip.Parent != null)
                {
                    mRemoteNG.Settings.Default.QuickyTBParentDock = quickConnectToolStrip.Parent.Dock.ToString();
                }
                mRemoteNG.Settings.Default.QuickyTBVisible = quickConnectToolStrip.Visible;
                mRemoteNG.Settings.Default.Save();

                SaveDockPanelLayout();
                SaveExternalApps();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Saving settings failed", ex);
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
            externalAppsSaver.Save(Runtime.ExternalTools);
        }
    }
}