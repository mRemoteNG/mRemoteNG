using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Management;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.App.Info;
using mRemoteNG.App.Initialization;
using mRemoteNG.App.Update;
using mRemoteNG.Config.Connections;
using mRemoteNG.Config.Connections.Multiuser;
using mRemoteNG.Connection;
using mRemoteNG.Messages;
using mRemoteNG.Tools;
using mRemoteNG.UI;
using mRemoteNG.UI.Forms;

namespace mRemoteNG.App
{
    public class Startup
    {
        private AppUpdater _appUpdate;

        public static Startup Instance { get; } = new Startup();

        private Startup()
        {
            _appUpdate = new AppUpdater();
        }

        static Startup()
        {
        }

        public void InitializeProgram(MessageCollector2 messageCollector)
        {
            Debug.Print("---------------------------" + Environment.NewLine + "[START] - " + Convert.ToString(DateTime.Now, CultureInfo.InvariantCulture));
            var startupLogger = new StartupDataLogger(messageCollector);
            startupLogger.LogStartupData();
            CompatibilityChecker.CheckCompatibility();
            ParseCommandLineArgs();
            IeBrowserEmulation.Register();
            GetConnectionIcons();
            DefaultConnectionInfo.Instance.LoadFrom(Settings.Default, a=>"ConDefault"+a);
            DefaultConnectionInheritance.Instance.LoadFrom(Settings.Default, a=>"InhDefault"+a);
        }

        private static void GetConnectionIcons()
        {
            var iPath = GeneralAppInfo.HomePath + "\\Icons\\";
            if (Directory.Exists(iPath) == false)
            {
                return;
            }

            foreach (var f in Directory.GetFiles(iPath, "*.ico", SearchOption.AllDirectories))
            {
                var fInfo = new FileInfo(f);
                Array.Resize(ref ConnectionIcon.Icons, ConnectionIcon.Icons.Length + 1);
                ConnectionIcon.Icons.SetValue(fInfo.Name.Replace(".ico", ""), ConnectionIcon.Icons.Length - 1);
            }
        }


        public void CreateConnectionsProvider()
        {
            frmMain.Default.AreWeUsingSqlServerForSavingConnections = Settings.Default.UseSQLServer;

            if (!Settings.Default.UseSQLServer) return;
            Runtime.RemoteConnectionsSyncronizer = new RemoteConnectionsSyncronizer(new SqlConnectionsUpdateChecker());
            Runtime.RemoteConnectionsSyncronizer.Enable();
        }

        public void CheckForUpdate()
        {
            if (_appUpdate == null)
            {
                _appUpdate = new AppUpdater();
            }
            else if (_appUpdate.IsGetUpdateInfoRunning)
            {
                return;
            }

            var nextUpdateCheck = Convert.ToDateTime(Settings.Default.CheckForUpdatesLastCheck.Add(TimeSpan.FromDays(Convert.ToDouble(Settings.Default.CheckForUpdatesFrequencyDays))));
            if (!Settings.Default.UpdatePending && DateTime.UtcNow < nextUpdateCheck)
            {
                return;
            }

            _appUpdate.GetUpdateInfoCompletedEvent += GetUpdateInfoCompleted;
            _appUpdate.GetUpdateInfoAsync();
        }

        private void GetUpdateInfoCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (frmMain.Default.InvokeRequired)
            {
                frmMain.Default.Invoke(new AsyncCompletedEventHandler(GetUpdateInfoCompleted), sender, e);
                return;
            }

            try
            {
                _appUpdate.GetUpdateInfoCompletedEvent -= GetUpdateInfoCompleted;

                if (e.Cancelled)
                {
                    return;
                }
                if (e.Error != null)
                {
                    throw e.Error;
                }

                if (_appUpdate.IsUpdateAvailable())
                {
                    Windows.Show(WindowType.Update);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("GetUpdateInfoCompleted() failed.", ex, MessageClass.ErrorMsg, true);
            }
        }


        private static void ParseCommandLineArgs()
        {
            try
            {
                var cmd = new CmdArgumentsInterpreter(Environment.GetCommandLineArgs());

                var ConsParam = "";
                if (cmd["cons"] != null)
                {
                    ConsParam = "cons";
                }
                if (cmd["c"] != null)
                {
                    ConsParam = "c";
                }

                var ResetPosParam = "";
                if (cmd["resetpos"] != null)
                {
                    ResetPosParam = "resetpos";
                }
                if (cmd["rp"] != null)
                {
                    ResetPosParam = "rp";
                }

                var ResetPanelsParam = "";
                if (cmd["resetpanels"] != null)
                {
                    ResetPanelsParam = "resetpanels";
                }
                if (cmd["rpnl"] != null)
                {
                    ResetPanelsParam = "rpnl";
                }

                var ResetToolbarsParam = "";
                if (cmd["resettoolbar"] != null)
                {
                    ResetToolbarsParam = "resettoolbar";
                }
                if (cmd["rtbr"] != null)
                {
                    ResetToolbarsParam = "rtbr";
                }

                if (cmd["reset"] != null)
                {
                    ResetPosParam = "rp";
                    ResetPanelsParam = "rpnl";
                    ResetToolbarsParam = "rtbr";
                }

                var NoReconnectParam = "";
                if (cmd["noreconnect"] != null)
                {
                    NoReconnectParam = "noreconnect";
                }
                if (cmd["norc"] != null)
                {
                    NoReconnectParam = "norc";
                }

                if (!string.IsNullOrEmpty(ConsParam))
                {
                    if (File.Exists(cmd[ConsParam]) == false)
                    {
                        if (File.Exists(GeneralAppInfo.HomePath + "\\" + cmd[ConsParam]))
                        {
                            Settings.Default.LoadConsFromCustomLocation = true;
                            Settings.Default.CustomConsPath = GeneralAppInfo.HomePath + "\\" + cmd[ConsParam];
                            return;
                        }
                        if (File.Exists(ConnectionsFileInfo.DefaultConnectionsPath + "\\" + cmd[ConsParam]))
                        {
                            Settings.Default.LoadConsFromCustomLocation = true;
                            Settings.Default.CustomConsPath = ConnectionsFileInfo.DefaultConnectionsPath + "\\" + cmd[ConsParam];
                            return;
                        }
                    }
                    else
                    {
                        Settings.Default.LoadConsFromCustomLocation = true;
                        Settings.Default.CustomConsPath = cmd[ConsParam];
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(ResetPosParam))
                {
                    Settings.Default.MainFormKiosk = false;
                    Settings.Default.MainFormLocation = new Point(999, 999);
                    Settings.Default.MainFormSize = new Size(900, 600);
                    Settings.Default.MainFormState = FormWindowState.Normal;
                }

                if (!string.IsNullOrEmpty(ResetPanelsParam))
                {
                    Settings.Default.ResetPanels = true;
                }

                if (!string.IsNullOrEmpty(NoReconnectParam))
                {
                    Settings.Default.NoReconnect = true;
                }

                if (!string.IsNullOrEmpty(ResetToolbarsParam))
                {
                    Settings.Default.ResetToolbars = true;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strCommandLineArgsCouldNotBeParsed + Environment.NewLine + ex.Message);
            }
        }
    }
}