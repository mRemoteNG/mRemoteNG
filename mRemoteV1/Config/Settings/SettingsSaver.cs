using System;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Tools;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Forms;

namespace mRemoteNG.Config.Settings
{
    public static class SettingsSaver
    {
        public static void SaveSettings(Control quickConnectToolStrip, ExternalToolsToolStrip externalToolsToolStrip)
        {
            try
            {
                var frmMain = FrmMain.Default;
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

                new DockPanelLayoutSaver().Save();
                new ExternalAppsSaver().Save(Runtime.ExternalTools);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Saving settings failed", ex);
            }
        }
    }
}