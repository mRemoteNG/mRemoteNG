using System;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Tools;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.Config.Settings
{
    public class SettingsSaver
    {
        private readonly ExternalToolsService _externalToolsService;

        public SettingsSaver(ExternalToolsService externalToolsService)
        {
            _externalToolsService = externalToolsService.ThrowIfNull(nameof(externalToolsService));
        }

        public void SaveSettings(
            Control quickConnectToolStrip, 
            ExternalToolsToolStrip externalToolsToolStrip,
            MultiSshToolStrip multiSshToolStrip,
            FrmMain frmMain)
        {
            try
            {
                var windowPlacement = new WindowPlacement(frmMain);
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

                SaveExternalAppsToolbarLocation(externalToolsToolStrip);
                SaveQuickConnectToolbarLocation(quickConnectToolStrip);
                SaveMultiSshToolbarLocation(multiSshToolStrip);
                
                mRemoteNG.Settings.Default.Save();

                SaveDockPanelLayout(frmMain.pnlDock);
                SaveExternalApps();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Saving settings failed", ex);
            }
        }

        private void SaveExternalAppsToolbarLocation(ExternalToolsToolStrip externalToolsToolStrip)
        {
            mRemoteNG.Settings.Default.ExtAppsTBLocation = externalToolsToolStrip.Location;
            mRemoteNG.Settings.Default.ExtAppsTBVisible = externalToolsToolStrip.Visible;
            mRemoteNG.Settings.Default.ExtAppsTBShowText = externalToolsToolStrip.CMenToolbarShowText.Checked;

            if (externalToolsToolStrip.Parent != null)
            {
                mRemoteNG.Settings.Default.ExtAppsTBParentDock = externalToolsToolStrip.Parent.Dock.ToString();
            }
        }

        private void SaveQuickConnectToolbarLocation(Control quickConnectToolStrip)
        {
            mRemoteNG.Settings.Default.QuickyTBLocation = quickConnectToolStrip.Location;
            mRemoteNG.Settings.Default.QuickyTBVisible = quickConnectToolStrip.Visible;

            if (quickConnectToolStrip.Parent != null)
            {
                mRemoteNG.Settings.Default.QuickyTBParentDock = quickConnectToolStrip.Parent.Dock.ToString();
            }
        }

        private void SaveMultiSshToolbarLocation(MultiSshToolStrip multiSshToolStrip)
        {
            mRemoteNG.Settings.Default.MultiSshToolbarLocation = multiSshToolStrip.Location;
            mRemoteNG.Settings.Default.MultiSshToolbarVisible = multiSshToolStrip.Visible;

            if (multiSshToolStrip.Parent != null)
            {
                mRemoteNG.Settings.Default.MultiSshToolbarParentDock = multiSshToolStrip.Parent.Dock.ToString();
            }
        }

        private void SaveDockPanelLayout(DockPanel dockPanel)
        {
            var panelLayoutXmlFilePath = SettingsFileInfo.SettingsPath + "\\" + SettingsFileInfo.LayoutFileName;
            var panelLayoutSaver = new DockPanelLayoutSaver(
                new DockPanelLayoutSerializer(),
                new FileDataProvider(panelLayoutXmlFilePath)
            );
            panelLayoutSaver.Save(dockPanel);
        }

        private void SaveExternalApps()
        {
            var externalAppsSaver = new ExternalAppsSaver();
            externalAppsSaver.Save(_externalToolsService.ExternalTools);
        }
    }
}