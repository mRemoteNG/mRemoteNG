using Microsoft.Win32;
using mRemoteNG.App.Update;
using mRemoteNG.Connection;
using mRemoteNG.Messages;
using mRemoteNG.My;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.UI.Window;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Management;
using System.Threading;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.Config.Connections;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.TaskDialog;

namespace mRemoteNG.App
{
    public class Startup
    {
        private static readonly Startup _singletonInstance = new Startup();
        private AppUpdater _appUpdate;

        private Startup()
        {
            _appUpdate = new AppUpdater();
        }

        static Startup()
        {
        }

        public static void InitializeProgram()
        {
            CreateLogger();
            LogStartupData();
            //CheckCompatibility();
        }

        public static void CheckCompatibility()
        {
            CheckFipsPolicy();
            CheckLenovoAutoScrollUtility();
        }
        private static void CheckFipsPolicy()
        {
            RegistryKey regKey = default(RegistryKey);

            bool isFipsPolicyEnabled = false;

            // Windows XP/Windows Server 2003
            regKey = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\Lsa");
            if (regKey != null)
            {
                if ((int)regKey.GetValue("FIPSAlgorithmPolicy") != 0)
                {
                    isFipsPolicyEnabled = true;
                }
            }

            // Windows Vista/Windows Server 2008 and newer
            regKey = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\Lsa\\FIPSAlgorithmPolicy");
            if (regKey != null)
            {
                if ((int)regKey.GetValue("Enabled") != 0)
                {
                    isFipsPolicyEnabled = true;
                }
            }

            if (isFipsPolicyEnabled)
            {
                MessageBox.Show(frmMain.Default, string.Format(Language.strErrorFipsPolicyIncompatible, (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.ProductName), (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
        }
        private static void CheckLenovoAutoScrollUtility()
        {
            if (!Settings.Default.CompatibilityWarnLenovoAutoScrollUtility)
            {
                return;
            }

            Process[] proccesses = new Process[] { };
            try
            {
                proccesses = Process.GetProcessesByName("virtscrl");
            }
            catch
            {
            }
            if (proccesses.Length == 0)
            {
                return;
            }

            CTaskDialog.MessageBox(Application.ProductName, Language.strCompatibilityProblemDetected, string.Format(Language.strCompatibilityLenovoAutoScrollUtilityDetected, Application.ProductName), "", "", Language.strCheckboxDoNotShowThisMessageAgain, ETaskDialogButtons.Ok, ESysIcons.Warning, ESysIcons.Warning);
            if (CTaskDialog.VerificationChecked)
            {
                Settings.Default.CompatibilityWarnLenovoAutoScrollUtility = false;
            }
        }


        public static void CreatePanels()
        {
            Windows.configForm = new ConfigWindow(Windows.configPanel);
            Windows.configPanel = Windows.configForm;

            Windows.treeForm = new ConnectionTreeWindow(Windows.treePanel);
            Windows.treePanel = Windows.treeForm;
            ConnectionTree.TreeView = Windows.treeForm.tvConnections;

            Windows.errorsForm = new ErrorAndInfoWindow(Windows.errorsPanel);
            Windows.errorsPanel = Windows.errorsForm;

            Windows.screenshotForm = new ScreenshotManagerWindow(Windows.screenshotPanel);
            Windows.screenshotPanel = Windows.screenshotForm;

            Windows.updateForm = new UpdateWindow(Windows.updatePanel);
            Windows.updatePanel = Windows.updateForm;

            Windows.AnnouncementForm = new AnnouncementWindow(Windows.AnnouncementPanel);
            Windows.AnnouncementPanel = Windows.AnnouncementForm;
        }
        public static void SetDefaultLayout()
        {
            frmMain.Default.pnlDock.Visible = false;

            frmMain.Default.pnlDock.DockLeftPortion = frmMain.Default.pnlDock.Width * 0.2;
            frmMain.Default.pnlDock.DockRightPortion = frmMain.Default.pnlDock.Width * 0.2;
            frmMain.Default.pnlDock.DockTopPortion = frmMain.Default.pnlDock.Height * 0.25;
            frmMain.Default.pnlDock.DockBottomPortion = frmMain.Default.pnlDock.Height * 0.25;

            Windows.treePanel.Show(frmMain.Default.pnlDock, DockState.DockLeft);
            Windows.configPanel.Show(frmMain.Default.pnlDock);
            Windows.configPanel.DockTo(Windows.treePanel.Pane, DockStyle.Bottom, -1);

            Windows.screenshotForm.Hide();

            frmMain.Default.pnlDock.Visible = true;
        }
        public static void GetConnectionIcons()
        {
            string iPath = (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.DirectoryPath + "\\Icons\\";
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


        public static void CreateLogger()
        {
            Runtime.Log = Logger.GetSingletonInstance();
        }
        public static void LogStartupData()
        {
            if (Settings.Default.WriteLogFile)
            {
                LogApplicationData();
                LogCmdLineArgs();
                LogSystemData();
                LogCLRData();
                LogCultureData();
            }
        }
        private static void LogSystemData()
        {
            string osData = GetOperatingSystemData();
            string architecture = GetArchitectureData();
            Runtime.Log.InfoFormat(string.Join(" ", Array.FindAll(new string[] { osData, architecture }, s => !string.IsNullOrEmpty(Convert.ToString(s)))));
        }
        private static string GetOperatingSystemData()
        {
            string osVersion = string.Empty;
            string servicePack = string.Empty;
            string osData = string.Empty;

            try
            {
                foreach (ManagementObject managementObject in new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem WHERE Primary=True").Get())
                {
                    osVersion = GetOSVersion(osVersion, managementObject);
                    servicePack = GetOSServicePack(servicePack, managementObject);
                }
            }
            catch (Exception ex)
            {
                Runtime.Log.WarnFormat("Error retrieving operating system information from WMI. {0}", ex.Message);
            }
            osData = string.Join(" ", new string[] { osVersion, servicePack });
            return osData;
        }
        private static string GetOSVersion(string osVersion, ManagementObject managementObject)
        {
            osVersion = Convert.ToString(managementObject.GetPropertyValue("Caption")).Trim();
            return osVersion;
        }
        private static string GetOSServicePack(string servicePack, ManagementObject managementObject)
        {
            int servicePackNumber = Convert.ToInt32(managementObject.GetPropertyValue("ServicePackMajorVersion"));
            if (!(servicePackNumber == 0))
            {
                servicePack = string.Format("Service Pack {0}", servicePackNumber);
            }
            return servicePack;
        }
        private static string GetArchitectureData()
        {
            string architecture = string.Empty;
            try
            {
                foreach (ManagementObject managementObject in new ManagementObjectSearcher("SELECT * FROM Win32_Processor WHERE DeviceID=\'CPU0\'").Get())
                {
                    int addressWidth = Convert.ToInt32(managementObject.GetPropertyValue("AddressWidth"));
                    architecture = string.Format("{0}-bit", addressWidth);
                }
            }
            catch (Exception ex)
            {
                Runtime.Log.WarnFormat("Error retrieving operating system address width from WMI. {0}", ex.Message);
            }
            return architecture;
        }
        private static void LogApplicationData()
        {
#if !PORTABLE
            Runtime.Log.InfoFormat("{0} {1} starting.", System.Windows.Forms.Application.ProductName, System.Windows.Forms.Application.ProductVersion);
#else
            Runtime.Log.InfoFormat("{0} {1} {2} starting.", Application.ProductName, Application.ProductVersion, Language.strLabelPortableEdition);
#endif
        }
        private static void LogCmdLineArgs()
        {
            Runtime.Log.InfoFormat("Command Line: {0}", Environment.GetCommandLineArgs());
        }
        private static void LogCLRData()
        {
            Runtime.Log.InfoFormat("Microsoft .NET CLR {0}", Environment.Version.ToString());
        }
        private static void LogCultureData()
        {
            Runtime.Log.InfoFormat("System Culture: {0}/{1}", Thread.CurrentThread.CurrentUICulture.Name, Thread.CurrentThread.CurrentUICulture.NativeName);
        }


        public static void CreateConnectionsProvider()
        {
            if (Settings.Default.UseSQLServer == true)
            {
                SqlConnectionsProvider _sqlConnectionsProvider = new SqlConnectionsProvider();
            }
        }

        public static void CheckForUpdate()
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
        private static void GetUpdateInfoCompleted(object sender, AsyncCompletedEventArgs e)
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


        public static void CheckForAnnouncement()
        {
            if (_appUpdate == null)
                _appUpdate = new AppUpdater();
            else if (_appUpdate.IsGetAnnouncementInfoRunning)
                return;

            _appUpdate.GetAnnouncementInfoCompletedEvent += GetAnnouncementInfoCompleted;
            _appUpdate.GetAnnouncementInfoAsync();
        }
        private static void GetAnnouncementInfoCompleted(object sender, AsyncCompletedEventArgs e)
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


        public static void ParseCommandLineArgs()
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
                        if (File.Exists((new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.DirectoryPath + "\\" + cmd[ConsParam]))
                        {
                            Settings.Default.LoadConsFromCustomLocation = true;
                            Settings.Default.CustomConsPath = (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.DirectoryPath + "\\" + cmd[ConsParam];
                            return;
                        }
                        else if (File.Exists(Info.ConnectionsFileInfo.DefaultConnectionsPath + "\\" + cmd[ConsParam]))
                        {
                            Settings.Default.LoadConsFromCustomLocation = true;
                            Settings.Default.CustomConsPath = Info.ConnectionsFileInfo.DefaultConnectionsPath + "\\" + cmd[ConsParam];
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