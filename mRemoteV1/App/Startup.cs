using mRemoteNG.App.Info;
using mRemoteNG.App.Update;
using mRemoteNG.Config.Connections;
using mRemoteNG.Connection;
using mRemoteNG.Messages;
using mRemoteNG.Tools;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Management;
using System.Threading;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.App
{
    public class Startup
    {
        private static readonly Startup _singletonInstance = new Startup();
        private CompatibilityChecker _compatibilityChecker;
        private AppUpdater _appUpdate;

        public static Startup Instance
        {
            get { return _singletonInstance; }
        }

        private Startup()
        {
            _compatibilityChecker = new CompatibilityChecker();
            _appUpdate = new AppUpdater();
        }

        static Startup()
        {
        }

        public void InitializeProgram()
        {
            Debug.Print("---------------------------" + Environment.NewLine + "[START] - " + Convert.ToString(DateTime.Now, CultureInfo.InvariantCulture));
            LogStartupData();
            _compatibilityChecker.CheckCompatibility();
            ParseCommandLineArgs();
            IeBrowserEmulation.Register();
            GetConnectionIcons();
            DefaultConnectionInfo.Instance.LoadFrom(Settings.Default, (a)=>"ConDefault"+a);
            DefaultConnectionInheritance.Instance.LoadFrom(Settings.Default, (a)=>"InhDefault"+a);
        }

        public void SetDefaultLayout()
        {
            frmMain.Default.pnlDock.Visible = false;

            frmMain.Default.pnlDock.DockLeftPortion = frmMain.Default.pnlDock.Width * 0.2;
            frmMain.Default.pnlDock.DockRightPortion = frmMain.Default.pnlDock.Width * 0.2;
            frmMain.Default.pnlDock.DockTopPortion = frmMain.Default.pnlDock.Height * 0.25;
            frmMain.Default.pnlDock.DockBottomPortion = frmMain.Default.pnlDock.Height * 0.25;

            Windows.TreePanel.Show(frmMain.Default.pnlDock, DockState.DockLeft);
            Windows.ConfigPanel.Show(frmMain.Default.pnlDock);
            Windows.ConfigPanel.DockTo(Windows.TreePanel.Pane, DockStyle.Bottom, -1);

            Windows.ScreenshotForm.Hide();

            frmMain.Default.pnlDock.Visible = true;
        }

        private void GetConnectionIcons()
        {
            string iPath = GeneralAppInfo.HomePath + "\\Icons\\";
            if (Directory.Exists(iPath) == false)
            {
                return;
            }

            foreach (string f in Directory.GetFiles(iPath, "*.ico", SearchOption.AllDirectories))
            {
                FileInfo fInfo = new FileInfo(f);
                Array.Resize(ref ConnectionIcon.Icons, ConnectionIcon.Icons.Length + 1);
                ConnectionIcon.Icons.SetValue(fInfo.Name.Replace(".ico", ""), ConnectionIcon.Icons.Length - 1);
            }
        }

        private void LogStartupData()
        {
            if (!Settings.Default.WriteLogFile) return;
            LogApplicationData();
            LogCmdLineArgs();
            LogSystemData();
            LogCLRData();
            LogCultureData();
        }

        private void LogSystemData()
        {
            string osData = GetOperatingSystemData();
            string architecture = GetArchitectureData();
            Logger.Instance.InfoFormat(string.Join(" ", Array.FindAll(new[] { osData, architecture }, s => !string.IsNullOrEmpty(Convert.ToString(s)))));
        }

        private string GetOperatingSystemData()
        {
            string osVersion = string.Empty;
            string servicePack = string.Empty;

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
            string osData = string.Join(" ", new string[] { osVersion, servicePack });
            return osData;
        }

        private string GetOSServicePack(string servicePack, ManagementObject managementObject)
        {
            int servicePackNumber = Convert.ToInt32(managementObject.GetPropertyValue("ServicePackMajorVersion"));
            if (servicePackNumber != 0)
            {
                servicePack = $"Service Pack {servicePackNumber}";
            }
            return servicePack;
        }

        private string GetArchitectureData()
        {
            string architecture = string.Empty;
            try
            {
                foreach (var o in new ManagementObjectSearcher("SELECT * FROM Win32_Processor WHERE DeviceID=\'CPU0\'").Get())
                {
                    var managementObject = (ManagementObject) o;
                    int addressWidth = Convert.ToInt32(managementObject.GetPropertyValue("AddressWidth"));
                    architecture = $"{addressWidth}-bit";
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.WarnFormat($"Error retrieving operating system address width from WMI. {ex.Message}");
            }
            return architecture;
        }

        private void LogApplicationData()
        {
            #if !PORTABLE
            Logger.Instance.InfoFormat($"{Application.ProductName} {Application.ProductVersion} starting.");
            #else
            Logger.Instance.InfoFormat(
                $"{Application.ProductName} {Application.ProductVersion} {Language.strLabelPortableEdition} starting.");
            #endif
        }

        private void LogCmdLineArgs()
        {
            Logger.Instance.InfoFormat($"Command Line: {Environment.GetCommandLineArgs()}");
        }

        private void LogCLRData()
        {
            Logger.Instance.InfoFormat($"Microsoft .NET CLR {Environment.Version}");
        }

        private void LogCultureData()
        {
            Logger.Instance.InfoFormat(
                $"System Culture: {Thread.CurrentThread.CurrentUICulture.Name}/{Thread.CurrentThread.CurrentUICulture.NativeName}");
        }

        public void CreateConnectionsProvider()
        {
            if (!Settings.Default.UseSQLServer) return;
            Runtime.SqlConnProvider = new PeriodicConnectionsUpdateChecker(new SqlConnectionsUpdateChecker());
        }

        private void CheckForUpdate()
        {
            if (_appUpdate == null)
            {
                _appUpdate = new AppUpdater();
            }
            else if (_appUpdate.IsGetUpdateInfoRunning)
            {
                return;
            }

            DateTime nextUpdateCheck = Convert.ToDateTime(Settings.Default.CheckForUpdatesLastCheck.Add(TimeSpan.FromDays(Convert.ToDouble(Settings.Default.CheckForUpdatesFrequencyDays))));
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
                frmMain.Default.Invoke(new AsyncCompletedEventHandler(GetUpdateInfoCompleted), new object[] { sender, e });
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
                    throw (e.Error);
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

        private void CheckForAnnouncement()
        {
            if (_appUpdate == null)
                _appUpdate = new AppUpdater();
            else if (_appUpdate.IsGetAnnouncementInfoRunning)
                return;

            _appUpdate.GetAnnouncementInfoCompletedEvent += GetAnnouncementInfoCompleted;
            _appUpdate.GetAnnouncementInfoAsync();
        }

        private void GetAnnouncementInfoCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (frmMain.Default.InvokeRequired)
            {
                frmMain.Default.Invoke(new AsyncCompletedEventHandler(GetAnnouncementInfoCompleted), new object[] { sender, e });
                return;
            }

            try
            {
                _appUpdate.GetAnnouncementInfoCompletedEvent -= GetAnnouncementInfoCompleted;

                if (e.Cancelled)
                {
                    return;
                }
                if (e.Error != null)
                {
                    throw (e.Error);
                }

                if (_appUpdate.IsAnnouncementAvailable())
                {
                    Windows.Show(WindowType.Announcement);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("GetAnnouncementInfoCompleted() failed.", ex, MessageClass.ErrorMsg, true);
            }
        }

        private void ParseCommandLineArgs()
        {
            try
            {
                CmdArgumentsInterpreter cmd = new CmdArgumentsInterpreter(Environment.GetCommandLineArgs());

                string ConsParam = "";
                if (cmd["cons"] != null)
                {
                    ConsParam = "cons";
                }
                if (cmd["c"] != null)
                {
                    ConsParam = "c";
                }

                string ResetPosParam = "";
                if (cmd["resetpos"] != null)
                {
                    ResetPosParam = "resetpos";
                }
                if (cmd["rp"] != null)
                {
                    ResetPosParam = "rp";
                }

                string ResetPanelsParam = "";
                if (cmd["resetpanels"] != null)
                {
                    ResetPanelsParam = "resetpanels";
                }
                if (cmd["rpnl"] != null)
                {
                    ResetPanelsParam = "rpnl";
                }

                string ResetToolbarsParam = "";
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

                string NoReconnectParam = "";
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
                        else if (File.Exists(ConnectionsFileInfo.DefaultConnectionsPath + "\\" + cmd[ConsParam]))
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