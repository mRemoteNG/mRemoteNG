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

        public void InitializeProgram()
        {
            Debug.Print("---------------------------" + Environment.NewLine + "[START] - " + Convert.ToString(DateTime.Now, CultureInfo.InvariantCulture));
            LogStartupData();
            CompatibilityChecker.CheckCompatibility();
            ParseCommandLineArgs();
            IeBrowserEmulation.Register();
            GetConnectionIcons();
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

        private static void LogStartupData()
        {
            if (!Settings.Default.WriteLogFile) return;
            LogApplicationData();
            LogCmdLineArgs();
            LogSystemData();
            LogCLRData();
            LogCultureData();
        }

        private static void LogSystemData()
        {
            var osData = GetOperatingSystemData();
            var architecture = GetArchitectureData();
            Logger.Instance.InfoFormat(string.Join(" ", Array.FindAll(new[] { osData, architecture }, s => !string.IsNullOrEmpty(Convert.ToString(s)))));
        }

        private static string GetOperatingSystemData()
        {
            var osVersion = string.Empty;
            var servicePack = string.Empty;

            try
            {
                foreach (var o in new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem WHERE Primary=True").Get())
                {
                    var managementObject = (ManagementObject) o;
                    osVersion = Convert.ToString(managementObject.GetPropertyValue("Caption")).Trim();
                    servicePack = GetOSServicePack(servicePack, managementObject);
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.WarnFormat($"Error retrieving operating system information from WMI. {ex.Message}");
            }
            var osData = string.Join(" ", osVersion, servicePack);
            return osData;
        }

        private static string GetOSServicePack(string servicePack, ManagementObject managementObject)
        {
            var servicePackNumber = Convert.ToInt32(managementObject.GetPropertyValue("ServicePackMajorVersion"));
            if (servicePackNumber != 0)
            {
                servicePack = $"Service Pack {servicePackNumber}";
            }
            return servicePack;
        }

        private static string GetArchitectureData()
        {
            var architecture = string.Empty;
            try
            {
                foreach (var o in new ManagementObjectSearcher("SELECT * FROM Win32_Processor WHERE DeviceID=\'CPU0\'").Get())
                {
                    var managementObject = (ManagementObject) o;
                    var addressWidth = Convert.ToInt32(managementObject.GetPropertyValue("AddressWidth"));
                    architecture = $"{addressWidth}-bit";
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.WarnFormat($"Error retrieving operating system address width from WMI. {ex.Message}");
            }
            return architecture;
        }

        private static void LogApplicationData()
        {
#if !PORTABLE
            Logger.Instance.InfoFormat($"{Application.ProductName} {Application.ProductVersion} starting.");
#else
            Logger.Instance.InfoFormat(
                $"{Application.ProductName} {Application.ProductVersion} {Language.strLabelPortableEdition} starting.");
#endif
        }

        private static void LogCmdLineArgs()
        {
            Logger.Instance.InfoFormat($"Command Line: {Environment.GetCommandLineArgs()}");
        }

        private static void LogCLRData()
        {
            Logger.Instance.InfoFormat($"Microsoft .NET CLR {Environment.Version}");
        }

        private static void LogCultureData()
        {
            Logger.Instance.InfoFormat(
                $"System Culture: {Thread.CurrentThread.CurrentUICulture.Name}/{Thread.CurrentThread.CurrentUICulture.NativeName}");
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
                Runtime.MessageCollector.AddExceptionMessage("GetUpdateInfoCompleted() failed.", ex);
            }
        }

        /// <summary>
        /// Returns a path to a file connections XML file for a given absolute or relative path
        /// </summary>
        /// <param name="ConsParam">The absolute or relative path to the connection XML file</param>
        /// <returns>string or null</returns>
        private static string GetCustomConsPath(string ConsParam)
        {
            // early exit condition
            if (string.IsNullOrEmpty(ConsParam))
                return null;

            // trim invalid characters for the Combine method (see: https://msdn.microsoft.com/en-us/library/fyy7a5kt.aspx#Anchor_2)
            ConsParam = ConsParam.Trim().TrimStart('\\').Trim();

            // fallback paths
            if (File.Exists(ConsParam))
                return ConsParam;

            if (File.Exists(Path.Combine(GeneralAppInfo.HomePath, ConsParam)))
                return GeneralAppInfo.HomePath + Path.DirectorySeparatorChar + ConsParam;

            if (File.Exists(Path.Combine(ConnectionsFileInfo.DefaultConnectionsPath, ConsParam)))
                return ConnectionsFileInfo.DefaultConnectionsPath + Path.DirectorySeparatorChar + ConsParam;

            // default case
            return null;
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

                // Handle custom connection file location
                Settings.Default.CustomConsPath = GetCustomConsPath(ConsParam);
                if (Settings.Default.CustomConsPath != null)
                    Settings.Default.LoadConsFromCustomLocation = true;

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

                Settings.Default.Save();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strCommandLineArgsCouldNotBeParsed + Environment.NewLine + ex.Message);
            }
        }
    }
}