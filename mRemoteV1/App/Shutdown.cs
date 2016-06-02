using mRemoteNG.Messages;
using mRemoteNG.Tools;
using System;
using System.Diagnostics;
using mRemoteNG.My;
using mRemoteNG.UI.Forms;

namespace mRemoteNG.App
{
    public class Shutdown
    {
        private static string _updateFilePath = null;

        public static bool UpdatePending
        {
            get
            {
                return !string.IsNullOrEmpty(_updateFilePath);
            }
        }

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
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strSettingsCouldNotBeSavedOrTrayDispose + Environment.NewLine + ex.Message, true);
            }
        }

        private static void StopPuttySessionWatcher()
        {
            Config.Putty.Sessions.StopWatcher();
        }

        private static void DisposeNotificationAreaIcon()
        {
            if (Runtime.NotificationAreaIcon != null && Runtime.NotificationAreaIcon.Disposed == false)
                Runtime.NotificationAreaIcon.Dispose();
        }

        private static void SaveConnections()
        {
            if (mRemoteNG.Settings.Default.SaveConsOnExit)
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
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "The update could not be started." + Environment.NewLine + ex.Message, true);
            }
        }

        private static void RunUpdateFile()
        {
            if (UpdatePending)
                Process.Start(_updateFilePath);
        }
    }
}