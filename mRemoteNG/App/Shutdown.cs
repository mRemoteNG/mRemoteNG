using mRemoteNG.Tools;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.Config.Connections;
using mRemoteNG.Config.Putty;
using mRemoteNG.Properties;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Forms;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.App
{
    [SupportedOSPlatform("windows")]
    public static class Shutdown
    {
        private static string _updateFilePath;

        private static bool UpdatePending
        {
            get { return !string.IsNullOrEmpty(_updateFilePath); }
        }

        public static void Quit(string updateFilePath = null)
        {
            _updateFilePath = updateFilePath;
            FrmMain.Default.Close();
            ProgramRoot.CloseSingletonInstanceMutex();
        }

        public static void Cleanup(Control quickConnectToolStrip,
                                   ExternalToolsToolStrip externalToolsToolStrip,
                                   MultiSshToolStrip multiSshToolStrip,
                                   FrmMain frmMain)
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
                Runtime.MessageCollector.AddExceptionStackTrace(Language.SettingsCouldNotBeSavedOrTrayDispose, ex);
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
            DateTime lastUpdate;
            DateTime updateDate;
            DateTime currentDate = DateTime.Now;

            //OBSOLETE: Settings.Default.SaveConsOnExit is obsolete and should be removed in a future release
            if (Properties.OptionsStartupExitPage.Default.SaveConnectionsOnExit || (Properties.OptionsBackupPage.Default.SaveConnectionsFrequency == (int)ConnectionsBackupFrequencyEnum.OnExit))
            {
                Runtime.ConnectionsService.SaveConnections();
				return;
            }	
			lastUpdate = Runtime.ConnectionsService.UsingDatabase ? Runtime.ConnectionsService.LastSqlUpdate : Runtime.ConnectionsService.LastFileUpdate;

            switch (Properties.OptionsBackupPage.Default.SaveConnectionsFrequency)
            {
                case (int)ConnectionsBackupFrequencyEnum.Daily:
                    updateDate = lastUpdate.AddDays(1);
                    break;
                case (int)ConnectionsBackupFrequencyEnum.Weekly:
                    updateDate = lastUpdate.AddDays(7);
                    break;
                default:
                    return;
            }

            if (currentDate >= updateDate)
            {
                Runtime.ConnectionsService.SaveConnections();
            }
        }

        private static void SaveSettings(Control quickConnectToolStrip,
                                         ExternalToolsToolStrip externalToolsToolStrip,
                                         MultiSshToolStrip multiSshToolStrip,
                                         FrmMain frmMain)
        {
            Config.Settings.SettingsSaver.SaveSettings(quickConnectToolStrip, externalToolsToolStrip, multiSshToolStrip,
                                                       frmMain);
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
                Process.Start(new ProcessStartInfo(_updateFilePath) { UseShellExecute = true });
        }
    }
}