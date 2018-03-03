using mRemoteNG.Tools;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.Config.Putty;
using mRemoteNG.Config.Settings;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Forms;
// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.App
{
    public class Shutdown
    {
        private readonly SettingsSaver _settingsSaver;
        private static string _updateFilePath;

        public Shutdown(SettingsSaver settingsSaver)
        {
            _settingsSaver = settingsSaver.ThrowIfNull(nameof(settingsSaver));
        }

        private static bool UpdatePending
        {
            get { return !string.IsNullOrEmpty(_updateFilePath); }
        }

        public void Quit(string updateFilePath = null)
        {
            _updateFilePath = updateFilePath;
            FrmMain.Default.Close();
            ProgramRoot.CloseSingletonInstanceMutex();
        }

        public void Cleanup(Control quickConnectToolStrip, ExternalToolsToolStrip externalToolsToolStrip, MultiSshToolStrip multiSshToolStrip, FrmMain frmMain)
        {
            try
            {
                StopPuttySessionWatcher();
                DisposeNotificationAreaIcon();
                SaveConnections();
                SaveSettings(quickConnectToolStrip, externalToolsToolStrip, multiSshToolStrip, frmMain);
                UnregisterBrowsers();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.strSettingsCouldNotBeSavedOrTrayDispose, ex);
            }
        }

        private void StopPuttySessionWatcher()
        {
            PuttySessionsManager.Instance.StopWatcher();
        }

        private void DisposeNotificationAreaIcon()
        {
            if (Runtime.NotificationAreaIcon != null && Runtime.NotificationAreaIcon.Disposed == false)
                Runtime.NotificationAreaIcon.Dispose();
        }

        private void SaveConnections()
        {
            if (Settings.Default.SaveConsOnExit)
                Runtime.ConnectionsService.SaveConnections();
        }

        private void SaveSettings(Control quickConnectToolStrip, ExternalToolsToolStrip externalToolsToolStrip, MultiSshToolStrip multiSshToolStrip, FrmMain frmMain)
        {
            _settingsSaver.SaveSettings(quickConnectToolStrip, externalToolsToolStrip, multiSshToolStrip, frmMain);
        }

        private void UnregisterBrowsers()
        {
            IeBrowserEmulation.Unregister();
        }

        public void StartUpdate()
        {
            try
            {
                RunUpdateFile();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("The update could not be started.", ex);
            }
        }

        private void RunUpdateFile()
        {
            if (UpdatePending)
                Process.Start(_updateFilePath);
        }
    }
}