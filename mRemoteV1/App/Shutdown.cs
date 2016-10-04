using mRemoteNG.Tools;
using System;
using System.Diagnostics;
using mRemoteNG.Config.Putty;
using mRemoteNG.UI.Forms;

namespace mRemoteNG.App
{
    public static class Shutdown
    {
        private static string _updateFilePath;

        private static bool UpdatePending => !string.IsNullOrEmpty(_updateFilePath);

        public static void Quit(string updateFilePath = null)
        {
            _updateFilePath = updateFilePath;
            frmMain.Default.Close();
            ProgramRoot.CloseSingletonInstanceMutex();
        }

        public static void Cleanup()
        {
            try
            {
                StopPuttySessionWatcher();
                DisposeNotificationAreaIcon();
                SaveConnections();
                SaveSettings();
                UnregisterBrowsers();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.strSettingsCouldNotBeSavedOrTrayDispose, ex);
            }
        }

        private static void StopPuttySessionWatcher()
        {
            PuttySessionsManager.Instance.StopWatcher();
        }

        private static void DisposeNotificationAreaIcon()
        {
            if (Runtime.NotificationAreaIcon != null && Runtime.NotificationAreaIcon.Disposed == false)
                Runtime.NotificationAreaIcon.Dispose();
        }

        private static void SaveConnections()
        {
            if (Settings.Default.SaveConsOnExit)
                Runtime.SaveConnections();
        }

        private static void SaveSettings()
        {
            Config.Settings.SettingsSaver.SaveSettings();
        }

        private static void UnregisterBrowsers()
        {
            IeBrowserEmulation.Unregister();
        }

        public static void StartUpdate()
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

        private static void RunUpdateFile()
        {
            if (UpdatePending)
                Process.Start(_updateFilePath);
        }
    }
}