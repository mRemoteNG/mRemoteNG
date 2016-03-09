using System.Collections.Generic;
using System;
using System.Drawing;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using mRemoteNG.Forms;
using log4net;
using mRemoteNG.Messages;
using mRemoteNG.Connection;
using mRemoteNG.Tools;
using mRemoteNG.Forms.OptionsPages;
using PSTaskDialog;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;
using System.Threading;
using System.Xml;
using System.Management;
using Microsoft.Win32;
using mRemoteNG.Connection.Protocol;


namespace mRemoteNG.App
{
	public class Runtime
	{
		private Runtime()
		{
			// Fix Warning 292 CA1053 : Microsoft.Design : Because type 'Native' contains only 'static' ('Shared' in Visual Basic) members, add a default private constructor to prevent the compiler from adding a default public constructor.
		}
			
        #region Public Properties
		public static frmMain MainForm {get; set;}
			
		private static Connection.List _connectionList;
        public static Connection.List ConnectionList
		{
			get
			{
				return _connectionList;
			}
			set
			{
				_connectionList = value;
			}
		}
			
		private static Connection.List _previousConnectionList;
        public static Connection.List PreviousConnectionList
		{
			get
			{
				return _previousConnectionList;
			}
			set
			{
				_previousConnectionList = value;
			}
		}
			
		private static Container.List _containerList;
        public static Container.List ContainerList
		{
			get
			{
				return _containerList;
			}
			set
			{
				_containerList = value;
			}
		}
			
		private static Container.List _previousContainerList;
        public static Container.List PreviousContainerList
		{
			get
			{
				return _previousContainerList;
			}
			set
			{
				_previousContainerList = value;
			}
		}
			
		private static Credential.List _credentialList;
        public static Credential.List CredentialList
		{
			get
			{
				return _credentialList;
			}
			set
			{
				_credentialList = value;
			}
		}
			
		private static Credential.List _previousCredentialList;
        public static Credential.List PreviousCredentialList
		{
			get
			{
				return _previousCredentialList;
			}
			set
			{
				_previousCredentialList = value;
			}
		}
			
			
		private static UI.Window.List _windowList;
        public static UI.Window.List WindowList
		{
			get
			{
				return _windowList;
			}
			set
			{
				_windowList = value;
			}
		}
			
		private static Messages.Collector _messageCollector;
        public static Collector MessageCollector
		{
			get
			{
				return _messageCollector;
			}
			set
			{
				_messageCollector = value;
			}
		}
			
		private static Tools.Controls.NotificationAreaIcon _notificationAreaIcon;
        public static Tools.Controls.NotificationAreaIcon NotificationAreaIcon
		{
			get
			{
				return _notificationAreaIcon;
			}
			set
			{
				_notificationAreaIcon = value;
			}
		}
			
		private static mRemoteNG.Tools.SystemMenu _systemMenu;
        public static SystemMenu SystemMenu
		{
			get
			{
				return _systemMenu;
			}
			set
			{
				_systemMenu = value;
			}
		}
			
		private static log4net.ILog _log;
        public static ILog Log
		{
			get
			{
				return _log;
			}
			set
			{
				_log = value;
			}
		}
			
		private static bool _isConnectionsFileLoaded;
        public static bool IsConnectionsFileLoaded
		{
			get
			{
				return _isConnectionsFileLoaded;
			}
			set
			{
				_isConnectionsFileLoaded = value;
			}
		}
			
		private static System.Timers.Timer _timerSqlWatcher;
        public static System.Timers.Timer TimerSqlWatcher
		{
			get
			{
				return _timerSqlWatcher;
			}
			set
			{
				_timerSqlWatcher = value;
				_timerSqlWatcher.Elapsed += tmrSqlWatcher_Elapsed;
			}
		}
			
		private static DateTime _lastSqlUpdate;
        public static DateTime LastSqlUpdate
		{
			get
			{
				return _lastSqlUpdate;
			}
			set
			{
				_lastSqlUpdate = value;
			}
		}
			
		private static string _lastSelected;
        public static string LastSelected
		{
			get
			{
				return _lastSelected;
			}
			set
			{
				_lastSelected = value;
			}
		}
			
		private static Connection.Info _defaultConnection;
        public static Connection.Info DefaultConnection
		{
			get
			{
				return _defaultConnection;
			}
			set
			{
				_defaultConnection = value;
			}
		}
			
		private static Connection.Info.Inheritance _defaultInheritance;
        public static Connection.Info.Inheritance DefaultInheritance
		{
			get
			{
				return _defaultInheritance;
			}
			set
			{
				_defaultInheritance = value;
			}
		}
			
		private static ArrayList _externalTools = new ArrayList();
        public static ArrayList ExternalTools
		{
			get
			{
				return _externalTools;
			}
			set
			{
				_externalTools = value;
			}
		}
			
        #endregion
			
        #region Classes
		public class Windows
		{
			public static UI.Window.Tree treeForm;
			public static DockContent treePanel = new DockContent();
			public static UI.Window.Config configForm;
			public static DockContent configPanel = new DockContent();
            public static UI.Window.ErrorsAndInfos errorsForm;
			public static DockContent errorsPanel = new DockContent();
            public static UI.Window.Sessions sessionsForm;
			public static DockContent sessionsPanel = new DockContent();
            public static UI.Window.ScreenshotManager screenshotForm;
			public static DockContent screenshotPanel = new DockContent();
			public static ExportForm exportForm;
			public static DockContent exportPanel = new DockContent();
            public static UI.Window.About aboutForm;
			public static DockContent aboutPanel = new DockContent();
			public static UI.Window.Update updateForm;
			public static DockContent updatePanel = new DockContent();
			public static UI.Window.SSHTransfer sshtransferForm;
			public static DockContent sshtransferPanel = new DockContent();
            public static UI.Window.ActiveDirectoryImport adimportForm;
			public static DockContent adimportPanel = new DockContent();
			public static UI.Window.Help helpForm;
			public static DockContent helpPanel = new DockContent();
            public static UI.Window.ExternalTools externalappsForm;
			public static DockContent externalappsPanel = new DockContent();
			public static UI.Window.PortScan portscanForm;
			public static DockContent portscanPanel = new DockContent();
			public static UI.Window.UltraVNCSC ultravncscForm;
			public static DockContent ultravncscPanel = new DockContent();
			public static UI.Window.ComponentsCheck componentscheckForm;
			public static DockContent componentscheckPanel = new DockContent();
            public static UI.Window.Announcement AnnouncementForm;
			public static DockContent AnnouncementPanel = new DockContent();
				
			public static void Show(UI.Window.Type windowType, bool portScanImport = false)
			{
				try
				{
                    if (windowType.Equals(UI.Window.Type.About))
                    {
                        if (aboutForm == null || aboutForm.IsDisposed)
                        {
                            aboutForm = new UI.Window.About(aboutPanel);
                            aboutPanel = aboutForm;
                        }
                        aboutForm.Show(frmMain.Default.pnlDock);
                    }
                    else if (windowType.Equals(UI.Window.Type.ActiveDirectoryImport))
                    {
                        if (adimportForm == null || adimportForm.IsDisposed)
                        {
                            adimportForm = new UI.Window.ActiveDirectoryImport(adimportPanel);
                            adimportPanel = adimportForm;
                        }
                        adimportPanel.Show(frmMain.Default.pnlDock);
                    }
                    else if (windowType.Equals(UI.Window.Type.Options))
                    {
                        using (OptionsForm optionsForm = new OptionsForm())
                        {
                            optionsForm.ShowDialog(frmMain.Default);
                        }
                    }
                    else if (windowType.Equals(UI.Window.Type.SSHTransfer))
                    {
                        sshtransferForm = new UI.Window.SSHTransfer(sshtransferPanel);
                        sshtransferPanel = sshtransferForm;
                        sshtransferForm.Show(frmMain.Default.pnlDock);
                    }
                    else if (windowType.Equals(UI.Window.Type.Update))
                    {
                        if (updateForm == null || updateForm.IsDisposed)
                        {
                            updateForm = new UI.Window.Update(updatePanel);
                            updatePanel = updateForm;
                        }
                        updateForm.Show(frmMain.Default.pnlDock);
                    }
                    else if (windowType.Equals(UI.Window.Type.Help))
                    {
                        if (helpForm == null || helpForm.IsDisposed)
						{
							helpForm = new UI.Window.Help(helpPanel);
							helpPanel = helpForm;
						}
						helpForm.Show(frmMain.Default.pnlDock);
                    }
                    else if (windowType.Equals(UI.Window.Type.ExternalApps))
                    {
                        if (externalappsForm == null || externalappsForm.IsDisposed)
						{
							externalappsForm = new UI.Window.ExternalTools(externalappsPanel);
							externalappsPanel = externalappsForm;
						}
						externalappsForm.Show(frmMain.Default.pnlDock);
                    }
                    else if (windowType.Equals(UI.Window.Type.PortScan))
                    {
                        portscanForm = new UI.Window.PortScan(portscanPanel, portScanImport);
						portscanPanel = portscanForm;
						portscanForm.Show(frmMain.Default.pnlDock);
                    }
                    else if (windowType.Equals(UI.Window.Type.UltraVNCSC))
                    {
                        if (ultravncscForm == null || ultravncscForm.IsDisposed)
                        {
                            ultravncscForm = new UI.Window.UltraVNCSC(ultravncscPanel);
                            ultravncscPanel = ultravncscForm;
                        }
                        ultravncscForm.Show(frmMain.Default.pnlDock);
                    }
                    else if (windowType.Equals(UI.Window.Type.ComponentsCheck))
                    {
                        if (componentscheckForm == null || componentscheckForm.IsDisposed)
                        {
                            componentscheckForm = new UI.Window.ComponentsCheck(componentscheckPanel);
                            componentscheckPanel = componentscheckForm;
                        }
                        componentscheckForm.Show(frmMain.Default.pnlDock);
                    }
                    else if (windowType.Equals(UI.Window.Type.Announcement))
                    {
                        if (AnnouncementForm == null || AnnouncementForm.IsDisposed)
                        {
                            AnnouncementForm = new UI.Window.Announcement(AnnouncementPanel);
                            AnnouncementPanel = AnnouncementForm;
                        }
                        AnnouncementForm.Show(frmMain.Default.pnlDock);
                    }
				}
				catch (Exception ex)
				{
					Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "App.Runtime.Windows.Show() failed." + Constants.vbNewLine + ex.Message, true);
				}
			}
				
			public static void ShowUpdatesTab()
			{
				using (OptionsForm optionsForm = new OptionsForm())
				{
					optionsForm.ShowDialog(frmMain.Default, typeof(UpdatesPage));
				}
					
			}
		}
			
		public class Screens
		{
			public static void SendFormToScreen(Screen Screen)
			{
				bool wasMax = false;
					
				if (frmMain.Default.WindowState == FormWindowState.Maximized)
				{
					wasMax = true;
					frmMain.Default.WindowState = FormWindowState.Normal;
				}
					
				frmMain.Default.Location = Screen.Bounds.Location;
					
				if (wasMax)
				{
					frmMain.Default.WindowState = FormWindowState.Maximized;
				}
			}
				
			public static void SendPanelToScreen(DockContent Panel, Screen Screen)
			{
				Panel.DockState = DockState.Float;
				Panel.ParentForm.Left = Screen.Bounds.Location.X;
				Panel.ParentForm.Top = Screen.Bounds.Location.Y;
			}
		}
			
		public class Startup
		{
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
				regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\Lsa");
				if (regKey != null)
				{
                    if (!((int)regKey.GetValue("FIPSAlgorithmPolicy") == 0))
					{
						isFipsPolicyEnabled = true;
					}
				}
					
				// Windows Vista/Windows Server 2008 and newer
				regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\Lsa\\FIPSAlgorithmPolicy");
				if (regKey != null)
				{
                    if (!((int)regKey.GetValue("Enabled") == 0))
					{
						isFipsPolicyEnabled = true;
					}
				}
					
				if (isFipsPolicyEnabled)
				{
					MessageBox.Show(frmMain.Default, string.Format(My.Language.strErrorFipsPolicyIncompatible, (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.ProductName), (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					System.Environment.Exit(1);
				}
			}
				
			private static void CheckLenovoAutoScrollUtility()
			{
				if (!My.Settings.Default.CompatibilityWarnLenovoAutoScrollUtility)
				{
					return ;
				}
					
				Process[] proccesses = new Process[] {};
				try
				{
					proccesses = Process.GetProcessesByName("virtscrl");
				}
				catch
				{
				}
				if (proccesses.Length == 0)
				{
					return ;
				}

                cTaskDialog.MessageBox(System.Windows.Forms.Application.ProductName, My.Language.strCompatibilityProblemDetected, string.Format(My.Language.strCompatibilityLenovoAutoScrollUtilityDetected, System.Windows.Forms.Application.ProductName), "", "", My.Language.strCheckboxDoNotShowThisMessageAgain, eTaskDialogButtons.OK, eSysIcons.Warning, eSysIcons.Warning);
				if (cTaskDialog.VerificationChecked)
				{
					My.Settings.Default.CompatibilityWarnLenovoAutoScrollUtility = false;
				}
			}
				
			public static void CreatePanels()
			{
				Windows.configForm = new UI.Window.Config(Windows.configPanel);
				Windows.configPanel = Windows.configForm;
					
				Windows.treeForm = new UI.Window.Tree(Windows.treePanel);
				Windows.treePanel = Windows.treeForm;
				Tree.Node.TreeView = Windows.treeForm.tvConnections;
					
				Windows.errorsForm = new UI.Window.ErrorsAndInfos(Windows.errorsPanel);
				Windows.errorsPanel = Windows.errorsForm;
					
				Windows.sessionsForm = new UI.Window.Sessions(Windows.sessionsPanel);
				Windows.sessionsPanel = Windows.sessionsForm;
					
				Windows.screenshotForm = new UI.Window.ScreenshotManager(Windows.screenshotPanel);
				Windows.screenshotPanel = Windows.screenshotForm;
					
				Windows.updateForm = new UI.Window.Update(Windows.updatePanel);
				Windows.updatePanel = Windows.updateForm;
					
				Windows.AnnouncementForm = new UI.Window.Announcement(Windows.AnnouncementPanel);
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
						
					Array.Resize(ref Connection.Icon.Icons, Connection.Icon.Icons.Length + 1);
					Connection.Icon.Icons.SetValue(fInfo.Name.Replace(".ico", ""), Connection.Icon.Icons.Length - 1);
				}
			}
				
			public static void CreateLogger()
			{
				log4net.Config.XmlConfigurator.Configure();
					
				string logFilePath = "";
                #if !PORTABLE
					logFilePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), System.Windows.Forms.Application.ProductName);
                #else
					logFilePath = System.Windows.Forms.Application.StartupPath;
                #endif
				string logFileName = Path.ChangeExtension(System.Windows.Forms.Application.ProductName, ".log");
				string logFile = Path.Combine(logFilePath, logFileName);
					
				log4net.Repository.ILoggerRepository repository = LogManager.GetRepository();
				log4net.Appender.IAppender[] appenders = repository.GetAppenders();
				log4net.Appender.FileAppender fileAppender = default(log4net.Appender.FileAppender);
				foreach (log4net.Appender.IAppender appender in appenders)
				{
					fileAppender = appender as log4net.Appender.FileAppender;
					if (!(fileAppender == null || !(fileAppender.Name == "LogFileAppender")))
					{
						fileAppender.File = logFile;
						fileAppender.ActivateOptions();
					}
				}
					
				Log = LogManager.GetLogger("Logger");
					
				if (My.Settings.Default.WriteLogFile)
				{
                    #if !PORTABLE
						Log.InfoFormat("{0} {1} starting.", System.Windows.Forms.Application.ProductName, System.Windows.Forms.Application.ProductVersion);
                    #else
						Log.InfoFormat("{0} {1} {2} starting.", System.Windows.Forms.Application.ProductName, System.Windows.Forms.Application.ProductVersion, My.Language.strLabelPortableEdition);
                    #endif
					Log.InfoFormat("Command Line: {0}", Environment.GetCommandLineArgs());
						
					string osVersion = string.Empty;
					string servicePack = string.Empty;
					try
					{
						foreach (ManagementObject managementObject in new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem WHERE Primary=True").Get())
						{
                            osVersion = System.Convert.ToString(managementObject.GetPropertyValue("Caption")).Trim();
							int servicePackNumber = System.Convert.ToInt32(managementObject.GetPropertyValue("ServicePackMajorVersion"));
							if (!(servicePackNumber == 0))
							{
								servicePack = string.Format("Service Pack {0}", servicePackNumber);
							}
						}
					}
					catch (Exception ex)
					{
						Log.WarnFormat("Error retrieving operating system information from WMI. {0}", ex.Message);
					}
						
					string architecture = string.Empty;
					try
					{
						foreach (ManagementObject managementObject in new ManagementObjectSearcher("SELECT * FROM Win32_Processor WHERE DeviceID=\'CPU0\'").Get())
						{
							int addressWidth = System.Convert.ToInt32(managementObject.GetPropertyValue("AddressWidth"));
							architecture = string.Format("{0}-bit", addressWidth);
						}
					}
					catch (Exception ex)
					{
						Log.WarnFormat("Error retrieving operating system address width from WMI. {0}", ex.Message);
					}
						
					Log.InfoFormat(string.Join(" ", Array.FindAll(new string[] {osVersion, servicePack, architecture}, s => !string.IsNullOrEmpty(System.Convert.ToString(s)))));

                    Log.InfoFormat("Microsoft .NET CLR {0}", System.Environment.Version.ToString());
					Log.InfoFormat("System Culture: {0}/{1}", Thread.CurrentThread.CurrentUICulture.Name, Thread.CurrentThread.CurrentUICulture.NativeName);
				}
			}
				
			private static Update _appUpdate;
			public static void CheckForUpdate()
			{
				if (_appUpdate == null)
				{
					_appUpdate = new Update();
				}
				else if (_appUpdate.IsGetUpdateInfoRunning)
				{
					return ;
				}
					
				DateTime nextUpdateCheck = System.Convert.ToDateTime(My.Settings.Default.CheckForUpdatesLastCheck.Add(TimeSpan.FromDays(System.Convert.ToDouble(My.Settings.Default.CheckForUpdatesFrequencyDays))));
				if (!My.Settings.Default.UpdatePending && DateTime.UtcNow < nextUpdateCheck)
				{
					return ;
				}
					
				_appUpdate.GetUpdateInfoCompletedEvent += GetUpdateInfoCompleted;
				_appUpdate.GetUpdateInfoAsync();
			}
				
			private static void GetUpdateInfoCompleted(object sender, AsyncCompletedEventArgs e)
			{
				if (MainForm.InvokeRequired)
				{
					MainForm.Invoke(new AsyncCompletedEventHandler(GetUpdateInfoCompleted), new object[] {sender, e});
					return ;
				}
					
				try
				{
					_appUpdate.GetUpdateInfoCompletedEvent -= GetUpdateInfoCompleted;
						
					if (e.Cancelled)
					{
						return ;
					}
					if (e.Error != null)
					{
						throw (e.Error);
					}
						
					if (_appUpdate.IsUpdateAvailable())
					{
						Windows.Show(UI.Window.Type.Update);
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
				{
					_appUpdate = new Update();
				}
				else if (_appUpdate.IsGetAnnouncementInfoRunning)
				{
					return ;
				}
					
				_appUpdate.GetAnnouncementInfoCompletedEvent += GetAnnouncementInfoCompleted;
				_appUpdate.GetAnnouncementInfoAsync();
			}
				
			private static void GetAnnouncementInfoCompleted(object sender, AsyncCompletedEventArgs e)
			{
				if (MainForm.InvokeRequired)
				{
					MainForm.Invoke(new AsyncCompletedEventHandler(GetAnnouncementInfoCompleted), new object[] {sender, e});
					return ;
				}
					
				try
				{
					_appUpdate.GetAnnouncementInfoCompletedEvent -= GetAnnouncementInfoCompleted;
						
					if (e.Cancelled)
					{
						return ;
					}
					if (e.Error != null)
					{
						throw (e.Error);
					}
						
					if (_appUpdate.IsAnnouncementAvailable())
					{
						Windows.Show(UI.Window.Type.Announcement);
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
					Tools.Misc.CMDArguments cmd = new Tools.Misc.CMDArguments(Environment.GetCommandLineArgs());
						
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
								My.Settings.Default.LoadConsFromCustomLocation = true;
								My.Settings.Default.CustomConsPath = (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.DirectoryPath + "\\" + cmd[ConsParam];
								return;
							}
							else if (File.Exists(App.Info.Connections.DefaultConnectionsPath + "\\" + cmd[ConsParam]))
							{
								My.Settings.Default.LoadConsFromCustomLocation = true;
								My.Settings.Default.CustomConsPath = App.Info.Connections.DefaultConnectionsPath + "\\" + cmd[ConsParam];
								return;
							}
						}
						else
						{
							My.Settings.Default.LoadConsFromCustomLocation = true;
							My.Settings.Default.CustomConsPath = cmd[ConsParam];
							return;
						}
					}
						
					if (!string.IsNullOrEmpty(ResetPosParam))
					{
						My.Settings.Default.MainFormKiosk = false;
						My.Settings.Default.MainFormLocation = new Point(999, 999);
						My.Settings.Default.MainFormSize = new Size(900, 600);
						My.Settings.Default.MainFormState = FormWindowState.Normal;
					}
						
					if (!string.IsNullOrEmpty(ResetPanelsParam))
					{
						My.Settings.Default.ResetPanels = true;
					}
						
					if (!string.IsNullOrEmpty(NoReconnectParam))
					{
						My.Settings.Default.NoReconnect = true;
					}
						
					if (!string.IsNullOrEmpty(ResetToolbarsParam))
					{
						My.Settings.Default.ResetToolbars = true;
					}
				}
				catch (Exception ex)
				{
					Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strCommandLineArgsCouldNotBeParsed + Constants.vbNewLine + ex.Message);
				}
			}
				
			public static void CreateSQLUpdateHandlerAndStartTimer()
			{
				if (My.Settings.Default.UseSQLServer == true)
				{
					Tools.Misc.SQLUpdateCheckFinished += SQLUpdateCheckFinished;
					TimerSqlWatcher = new System.Timers.Timer(3000);
					TimerSqlWatcher.Start();
				}
			}
				
			public static void DestroySQLUpdateHandlerAndStopTimer()
			{
				try
				{
					//LastSqlUpdate = null;
					Tools.Misc.SQLUpdateCheckFinished -= SQLUpdateCheckFinished;
					if (TimerSqlWatcher != null)
					{
						TimerSqlWatcher.Stop();
						TimerSqlWatcher.Close();
					}
				}
				catch (Exception)
				{
				}
			}
		}
			
		public class Shutdown
		{
			public static void Quit(string updateFilePath = null)
			{
				_updateFilePath = updateFilePath;
				frmMain.Default.Close();
			}
				
			public static void Cleanup()
			{
				try
				{
					Config.Putty.Sessions.StopWatcher();
						
					if (NotificationAreaIcon != null)
					{
						if (NotificationAreaIcon.Disposed == false)
						{
							NotificationAreaIcon.Dispose();
						}
					}
						
					if (My.Settings.Default.SaveConsOnExit)
					{
						SaveConnections();
					}
						
					Config.Settings.Save.SaveSettings();
						
					IeBrowserEmulation.Unregister();
				}
				catch (Exception ex)
				{
					Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strSettingsCouldNotBeSavedOrTrayDispose + Constants.vbNewLine + ex.Message, true);
				}
			}
				
			public static void StartUpdate()
			{
				if (!UpdatePending)
				{
					return ;
				}
				try
				{
					Process.Start(_updateFilePath);
				}
				catch (Exception ex)
				{
					Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "The update could not be started." + Constants.vbNewLine + ex.Message, true);
				}
			}
				
			private static string _updateFilePath = null;
				
            public static bool UpdatePending
			{
				get
				{
					return !string.IsNullOrEmpty(_updateFilePath);
				}
			}
		}
        #endregion
			
        #region Default Connection
		public static mRemoteNG.Connection.Info DefaultConnectionFromSettings()
		{
			DefaultConnection = new mRemoteNG.Connection.Info();
			DefaultConnection.IsDefault = true;
			return DefaultConnection;
		}
			
		public static void DefaultConnectionToSettings()
		{
			My.Settings.Default.ConDefaultDescription = DefaultConnection.Description;
			My.Settings.Default.ConDefaultIcon = DefaultConnection.Icon;
			My.Settings.Default.ConDefaultUsername = DefaultConnection.Username;
			My.Settings.Default.ConDefaultPassword = DefaultConnection.Password;
			My.Settings.Default.ConDefaultDomain = DefaultConnection.Domain;
			My.Settings.Default.ConDefaultProtocol = DefaultConnection.Protocol.ToString();
			My.Settings.Default.ConDefaultPuttySession = DefaultConnection.PuttySession;
			My.Settings.Default.ConDefaultICAEncryptionStrength = DefaultConnection.ICAEncryption.ToString();
			My.Settings.Default.ConDefaultRDPAuthenticationLevel = DefaultConnection.RDPAuthenticationLevel.ToString();
			My.Settings.Default.ConDefaultLoadBalanceInfo = DefaultConnection.LoadBalanceInfo;
			My.Settings.Default.ConDefaultUseConsoleSession = DefaultConnection.UseConsoleSession;
			My.Settings.Default.ConDefaultUseCredSsp = DefaultConnection.UseCredSsp;
			My.Settings.Default.ConDefaultRenderingEngine = DefaultConnection.RenderingEngine.ToString();
			My.Settings.Default.ConDefaultResolution = DefaultConnection.Resolution.ToString();
			My.Settings.Default.ConDefaultAutomaticResize = DefaultConnection.AutomaticResize;
			My.Settings.Default.ConDefaultColors = DefaultConnection.Colors.ToString();
			My.Settings.Default.ConDefaultCacheBitmaps = DefaultConnection.CacheBitmaps;
			My.Settings.Default.ConDefaultDisplayWallpaper = DefaultConnection.DisplayWallpaper;
			My.Settings.Default.ConDefaultDisplayThemes = DefaultConnection.DisplayThemes;
			My.Settings.Default.ConDefaultEnableFontSmoothing = DefaultConnection.EnableFontSmoothing;
			My.Settings.Default.ConDefaultEnableDesktopComposition = DefaultConnection.EnableDesktopComposition;
			My.Settings.Default.ConDefaultRedirectKeys = DefaultConnection.RedirectKeys;
			My.Settings.Default.ConDefaultRedirectDiskDrives = DefaultConnection.RedirectDiskDrives;
			My.Settings.Default.ConDefaultRedirectPrinters = DefaultConnection.RedirectPrinters;
			My.Settings.Default.ConDefaultRedirectPorts = DefaultConnection.RedirectPorts;
			My.Settings.Default.ConDefaultRedirectSmartCards = DefaultConnection.RedirectSmartCards;
			My.Settings.Default.ConDefaultRedirectSound = DefaultConnection.RedirectSound.ToString();
			My.Settings.Default.ConDefaultPreExtApp = DefaultConnection.PreExtApp;
			My.Settings.Default.ConDefaultPostExtApp = DefaultConnection.PostExtApp;
			My.Settings.Default.ConDefaultMacAddress = DefaultConnection.MacAddress;
			My.Settings.Default.ConDefaultUserField = DefaultConnection.UserField;
			My.Settings.Default.ConDefaultVNCAuthMode = DefaultConnection.VNCAuthMode.ToString();
			My.Settings.Default.ConDefaultVNCColors = DefaultConnection.VNCColors.ToString();
			My.Settings.Default.ConDefaultVNCCompression = DefaultConnection.VNCCompression.ToString();
			My.Settings.Default.ConDefaultVNCEncoding = DefaultConnection.VNCEncoding.ToString();
			My.Settings.Default.ConDefaultVNCProxyIP = DefaultConnection.VNCProxyIP;
			My.Settings.Default.ConDefaultVNCProxyPassword = DefaultConnection.VNCProxyPassword;
			My.Settings.Default.ConDefaultVNCProxyPort = DefaultConnection.VNCProxyPort;
			My.Settings.Default.ConDefaultVNCProxyType = DefaultConnection.VNCProxyType.ToString();
			My.Settings.Default.ConDefaultVNCProxyUsername = DefaultConnection.VNCProxyUsername;
			My.Settings.Default.ConDefaultVNCSmartSizeMode = DefaultConnection.VNCSmartSizeMode.ToString();
			My.Settings.Default.ConDefaultVNCViewOnly = DefaultConnection.VNCViewOnly;
			My.Settings.Default.ConDefaultExtApp = DefaultConnection.ExtApp;
			My.Settings.Default.ConDefaultRDGatewayUsageMethod = DefaultConnection.RDGatewayUsageMethod.ToString();
			My.Settings.Default.ConDefaultRDGatewayHostname = DefaultConnection.RDGatewayHostname;
			My.Settings.Default.ConDefaultRDGatewayUsername = DefaultConnection.RDGatewayUsername;
			My.Settings.Default.ConDefaultRDGatewayPassword = DefaultConnection.RDGatewayPassword;
			My.Settings.Default.ConDefaultRDGatewayDomain = DefaultConnection.RDGatewayDomain;
			My.Settings.Default.ConDefaultRDGatewayUseConnectionCredentials = DefaultConnection.RDGatewayUseConnectionCredentials.ToString();
		}
        #endregion
			
        #region Default Inheritance
		public static mRemoteNG.Connection.Info.Inheritance DefaultInheritanceFromSettings()
		{
			DefaultInheritance = new mRemoteNG.Connection.Info.Inheritance(null);
			DefaultInheritance.IsDefault = true;
				
			return DefaultInheritance;
		}
			
		public static void DefaultInheritanceToSettings()
		{
			My.Settings.Default.InhDefaultDescription = DefaultInheritance.Description;
			My.Settings.Default.InhDefaultIcon = DefaultInheritance.Icon;
			My.Settings.Default.InhDefaultPanel = DefaultInheritance.Panel;
			My.Settings.Default.InhDefaultUsername = DefaultInheritance.Username;
			My.Settings.Default.InhDefaultPassword = DefaultInheritance.Password;
			My.Settings.Default.InhDefaultDomain = DefaultInheritance.Domain;
			My.Settings.Default.InhDefaultProtocol = DefaultInheritance.Protocol;
			My.Settings.Default.InhDefaultPort = DefaultInheritance.Port;
			My.Settings.Default.InhDefaultPuttySession = DefaultInheritance.PuttySession;
			My.Settings.Default.InhDefaultUseConsoleSession = DefaultInheritance.UseConsoleSession;
			My.Settings.Default.InhDefaultUseCredSsp = DefaultInheritance.UseCredSsp;
			My.Settings.Default.InhDefaultRenderingEngine = DefaultInheritance.RenderingEngine;
			My.Settings.Default.InhDefaultICAEncryptionStrength = DefaultInheritance.ICAEncryption;
			My.Settings.Default.InhDefaultRDPAuthenticationLevel = DefaultInheritance.RDPAuthenticationLevel;
			My.Settings.Default.InhDefaultLoadBalanceInfo = DefaultInheritance.LoadBalanceInfo;
			My.Settings.Default.InhDefaultResolution = DefaultInheritance.Resolution;
			My.Settings.Default.InhDefaultAutomaticResize = DefaultInheritance.AutomaticResize;
			My.Settings.Default.InhDefaultColors = DefaultInheritance.Colors;
			My.Settings.Default.InhDefaultCacheBitmaps = DefaultInheritance.CacheBitmaps;
			My.Settings.Default.InhDefaultDisplayWallpaper = DefaultInheritance.DisplayWallpaper;
			My.Settings.Default.InhDefaultDisplayThemes = DefaultInheritance.DisplayThemes;
			My.Settings.Default.InhDefaultEnableFontSmoothing = DefaultInheritance.EnableFontSmoothing;
			My.Settings.Default.InhDefaultEnableDesktopComposition = DefaultInheritance.EnableDesktopComposition;
			My.Settings.Default.InhDefaultRedirectKeys = DefaultInheritance.RedirectKeys;
			My.Settings.Default.InhDefaultRedirectDiskDrives = DefaultInheritance.RedirectDiskDrives;
			My.Settings.Default.InhDefaultRedirectPrinters = DefaultInheritance.RedirectPrinters;
			My.Settings.Default.InhDefaultRedirectPorts = DefaultInheritance.RedirectPorts;
			My.Settings.Default.InhDefaultRedirectSmartCards = DefaultInheritance.RedirectSmartCards;
			My.Settings.Default.InhDefaultRedirectSound = DefaultInheritance.RedirectSound;
			My.Settings.Default.InhDefaultPreExtApp = DefaultInheritance.PreExtApp;
			My.Settings.Default.InhDefaultPostExtApp = DefaultInheritance.PostExtApp;
			My.Settings.Default.InhDefaultMacAddress = DefaultInheritance.MacAddress;
			My.Settings.Default.InhDefaultUserField = DefaultInheritance.UserField;
			// VNC inheritance
			My.Settings.Default.InhDefaultVNCAuthMode = DefaultInheritance.VNCAuthMode;
			My.Settings.Default.InhDefaultVNCColors = DefaultInheritance.VNCColors;
			My.Settings.Default.InhDefaultVNCCompression = DefaultInheritance.VNCCompression;
			My.Settings.Default.InhDefaultVNCEncoding = DefaultInheritance.VNCEncoding;
			My.Settings.Default.InhDefaultVNCProxyIP = DefaultInheritance.VNCProxyIP;
			My.Settings.Default.InhDefaultVNCProxyPassword = DefaultInheritance.VNCProxyPassword;
			My.Settings.Default.InhDefaultVNCProxyPort = DefaultInheritance.VNCProxyPort;
			My.Settings.Default.InhDefaultVNCProxyType = DefaultInheritance.VNCProxyType;
			My.Settings.Default.InhDefaultVNCProxyUsername = DefaultInheritance.VNCProxyUsername;
			My.Settings.Default.InhDefaultVNCSmartSizeMode = DefaultInheritance.VNCSmartSizeMode;
			My.Settings.Default.InhDefaultVNCViewOnly = DefaultInheritance.VNCViewOnly;
			// Ext. App inheritance
			My.Settings.Default.InhDefaultExtApp = DefaultInheritance.ExtApp;
			// RDP gateway inheritance
			My.Settings.Default.InhDefaultRDGatewayUsageMethod = DefaultInheritance.RDGatewayUsageMethod;
			My.Settings.Default.InhDefaultRDGatewayHostname = DefaultInheritance.RDGatewayHostname;
			My.Settings.Default.InhDefaultRDGatewayUsername = DefaultInheritance.RDGatewayUsername;
			My.Settings.Default.InhDefaultRDGatewayPassword = DefaultInheritance.RDGatewayPassword;
			My.Settings.Default.InhDefaultRDGatewayDomain = DefaultInheritance.RDGatewayDomain;
			My.Settings.Default.InhDefaultRDGatewayUseConnectionCredentials = DefaultInheritance.RDGatewayUseConnectionCredentials;
		}
        #endregion
			
        #region Panels
		public static System.Windows.Forms.Form AddPanel(string title = "", bool noTabber = false)
		{
			try
			{
				if (title == "")
				{
					title = My.Language.strNewPanel;
				}
					
				DockContent pnlcForm = new DockContent();
				UI.Window.Connection cForm = new UI.Window.Connection(pnlcForm);
				pnlcForm = cForm;
					
				//create context menu
				ContextMenuStrip cMen = new ContextMenuStrip();
					
				//create rename item
				ToolStripMenuItem cMenRen = new ToolStripMenuItem();
				cMenRen.Text = My.Language.strRename;
				cMenRen.Image = My.Resources.Rename;
				cMenRen.Tag = pnlcForm;
				cMenRen.Click += cMenConnectionPanelRename_Click;
					
				ToolStripMenuItem cMenScreens = new ToolStripMenuItem();
				cMenScreens.Text = My.Language.strSendTo;
				cMenScreens.Image = My.Resources.Monitor;
				cMenScreens.Tag = pnlcForm;
				cMenScreens.DropDownItems.Add("Dummy");
				cMenScreens.DropDownOpening += cMenConnectionPanelScreens_DropDownOpening;
					
				cMen.Items.AddRange(new ToolStripMenuItem[] {cMenRen, cMenScreens});
					
				pnlcForm.TabPageContextMenuStrip = cMen;
					
				(cForm as UI.Window.Connection).SetFormText(title.Replace("&", "&&"));
					
				pnlcForm.Show(frmMain.Default.pnlDock, DockState.Document);
					
				if (noTabber)
				{
					(cForm as UI.Window.Connection).TabController.Dispose();
				}
				else
				{
					WindowList.Add(cForm);
				}
					
				return cForm;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn\'t add panel" + Constants.vbNewLine + ex.Message);
				return null;
			}
		}
			
		private static void cMenConnectionPanelRename_Click(System.Object sender, System.EventArgs e)
		{
			try
			{
				UI.Window.Connection conW = default(UI.Window.Connection);
                conW = (UI.Window.Connection)((System.Windows.Forms.Control)sender).Tag;

                string nTitle = Interaction.InputBox(Prompt: My.Language.strNewTitle + ":", DefaultResponse: System.Convert.ToString(((System.Windows.Forms.Control)((System.Windows.Forms.Control)sender).Tag).Text.Replace("&&", "&")));
					
				if (!string.IsNullOrEmpty(nTitle))
				{
					conW.SetFormText(nTitle.Replace("&", "&&"));
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn\'t rename panel" + Constants.vbNewLine + ex.Message);
			}
		}
			
		private static void cMenConnectionPanelScreens_DropDownOpening(System.Object sender, System.EventArgs e)
		{
			try
			{
                ToolStripMenuItem cMenScreens = (ToolStripMenuItem)sender;
				cMenScreens.DropDownItems.Clear();
					
				for (int i = 0; i <= Screen.AllScreens.Length - 1; i++)
				{
					ToolStripMenuItem cMenScreen = new ToolStripMenuItem(My.Language.strScreen + " " + System.Convert.ToString(i + 1));
					cMenScreen.Tag = new ArrayList();
					cMenScreen.Image = My.Resources.Monitor_GoTo;
					(cMenScreen.Tag as ArrayList).Add(Screen.AllScreens[i]);
					(cMenScreen.Tag as ArrayList).Add(cMenScreens.Tag);
					cMenScreen.Click += cMenConnectionPanelScreen_Click;
					cMenScreens.DropDownItems.Add(cMenScreen);
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn\'t enumerate screens" + Constants.vbNewLine + ex.Message);
			}
		}
			
		private static void cMenConnectionPanelScreen_Click(object sender, System.EventArgs e)
		{
            System.Windows.Forms.Screen screen = null;
            WeifenLuo.WinFormsUI.Docking.DockContent panel = null;
			try
			{
                IEnumerable tagEnumeration = (IEnumerable)((ToolStripMenuItem)sender).Tag;
                if (tagEnumeration != null)
                {
                    foreach (Object obj in tagEnumeration) 
                    {
                        if (obj is System.Windows.Forms.Screen)
                        {
                            screen = (System.Windows.Forms.Screen)obj;
                        }
                        else if (obj is WeifenLuo.WinFormsUI.Docking.DockContent)
                        {
                            panel = (WeifenLuo.WinFormsUI.Docking.DockContent)obj;
                        }
                    }
                    Screens.SendPanelToScreen(panel, screen);
                }
			}
			catch (Exception)
			{
			}
		}
        #endregion
			
        #region Credential Loading/Saving
		public static void LoadCredentials()
		{
				
		}
        #endregion
			
        #region Connections Loading/Saving
		public static void NewConnections(string filename)
		{
			try
			{
				ConnectionList = new Connection.List();
				ContainerList = new Container.List();
					
				mRemoteNG.Config.Connections.Load connectionsLoad = new Config.Connections.Load();
					
				if (filename == GetDefaultStartupConnectionFileName())
				{
					My.Settings.Default.LoadConsFromCustomLocation = false;
				}
				else
				{
					My.Settings.Default.LoadConsFromCustomLocation = true;
					My.Settings.Default.CustomConsPath = filename;
				}
					
				Directory.CreateDirectory(Path.GetDirectoryName(filename));
					
				// Use File.Open with FileMode.CreateNew so that we don't overwrite an existing file
				using (FileStream fileStream = File.Open(filename, FileMode.CreateNew, FileAccess.Write, FileShare.None))
				{
					using (XmlTextWriter xmlTextWriter = new XmlTextWriter(fileStream, System.Text.Encoding.UTF8))
					{
						xmlTextWriter.Formatting = Formatting.Indented;
						xmlTextWriter.Indentation = 4;
							
						xmlTextWriter.WriteStartDocument();
							
						xmlTextWriter.WriteStartElement("Connections"); // Do not localize
						xmlTextWriter.WriteAttributeString("Name", My.Language.strConnections);
						xmlTextWriter.WriteAttributeString("Export", "", "False");
						xmlTextWriter.WriteAttributeString("Protected", "", "GiUis20DIbnYzWPcdaQKfjE2H5jh//L5v4RGrJMGNXuIq2CttB/d/BxaBP2LwRhY");
						xmlTextWriter.WriteAttributeString("ConfVersion", "", "2.5");
							
						xmlTextWriter.WriteEndElement();
						xmlTextWriter.WriteEndDocument();
							
						xmlTextWriter.Close();
					}
						
				}
					
					
				connectionsLoad.ConnectionList = ConnectionList;
				connectionsLoad.ContainerList = ContainerList;
					
				Tree.Node.ResetTree();
					
				connectionsLoad.RootTreeNode = Windows.treeForm.tvConnections.Nodes[0];
					
				// Load config
				connectionsLoad.ConnectionFileName = filename;
				connectionsLoad.LoadConnections(false);
					
				Windows.treeForm.tvConnections.SelectedNode = connectionsLoad.RootTreeNode;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage(My.Language.strCouldNotCreateNewConnectionsFile, ex, MessageClass.ErrorMsg);
			}
		}
			
		private static void LoadConnectionsBG(bool WithDialog = false, bool Update = false)
		{
			_withDialog = false;
			_loadUpdate = true;
				
			Thread t = new Thread(new System.Threading.ThreadStart(LoadConnectionsBGd));
			t.SetApartmentState(System.Threading.ApartmentState.STA);
			t.Start();
		}
			
		private static bool _withDialog = false;
		private static bool _loadUpdate = false;
		private static void LoadConnectionsBGd()
		{
			LoadConnections(_withDialog, _loadUpdate);
		}
			
		public static void LoadConnections(bool withDialog = false, bool update = false)
		{
            mRemoteNG.Config.Connections.Load connectionsLoad = new mRemoteNG.Config.Connections.Load();
				
			try
			{
				bool tmrWasEnabled = false;
				if (TimerSqlWatcher != null)
				{
					tmrWasEnabled = TimerSqlWatcher.Enabled;
						
					if (TimerSqlWatcher.Enabled == true)
					{
						TimerSqlWatcher.Stop();
					}
				}
					
				if (ConnectionList != null && ContainerList != null)
				{
					PreviousConnectionList = ConnectionList.Copy();
					PreviousContainerList = ContainerList.Copy();
				}
					
				ConnectionList = new Connection.List();
				ContainerList = new Container.List();
					
				if (!My.Settings.Default.UseSQLServer)
				{
					if (withDialog)
					{
						OpenFileDialog loadDialog = Tools.Controls.ConnectionsLoadDialog();
							
						if (loadDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
						{
							connectionsLoad.ConnectionFileName = loadDialog.FileName;
						}
						else
						{
							return;
						}
					}
					else
					{
						connectionsLoad.ConnectionFileName = GetStartupConnectionFileName();
					}
						
					CreateBackupFile(System.Convert.ToString(connectionsLoad.ConnectionFileName));
				}
					
				connectionsLoad.ConnectionList = ConnectionList;
				connectionsLoad.ContainerList = ContainerList;
					
				if (PreviousConnectionList != null && PreviousContainerList != null)
				{
					connectionsLoad.PreviousConnectionList = PreviousConnectionList;
					connectionsLoad.PreviousContainerList = PreviousContainerList;
				}
					
				if (update == true)
				{
					connectionsLoad.PreviousSelected = LastSelected;
				}
					
				Tree.Node.ResetTree();
					
				connectionsLoad.RootTreeNode = Windows.treeForm.tvConnections.Nodes[0];
					
				connectionsLoad.UseSQL = My.Settings.Default.UseSQLServer;
				connectionsLoad.SQLHost = My.Settings.Default.SQLHost;
				connectionsLoad.SQLDatabaseName = My.Settings.Default.SQLDatabaseName;
				connectionsLoad.SQLUsername = My.Settings.Default.SQLUser;
				connectionsLoad.SQLPassword = Security.Crypt.Decrypt(System.Convert.ToString(My.Settings.Default.SQLPass), Info.General.EncryptionKey);
				connectionsLoad.SQLUpdate = update;
					
				connectionsLoad.LoadConnections(false);
					
				if (My.Settings.Default.UseSQLServer == true)
				{
					LastSqlUpdate = DateTime.Now;
				}
				else
				{
					if (connectionsLoad.ConnectionFileName == GetDefaultStartupConnectionFileName())
					{
						My.Settings.Default.LoadConsFromCustomLocation = false;
					}
					else
					{
						My.Settings.Default.LoadConsFromCustomLocation = true;
						My.Settings.Default.CustomConsPath = connectionsLoad.ConnectionFileName;
					}
				}
					
				if (tmrWasEnabled && TimerSqlWatcher != null)
				{
					TimerSqlWatcher.Start();
				}
			}
			catch (Exception ex)
			{
				if (My.Settings.Default.UseSQLServer)
				{
					Runtime.MessageCollector.AddExceptionMessage(My.Language.strLoadFromSqlFailed, ex);
					string commandButtons = string.Join("|", new[] {My.Language.strCommandTryAgain, My.Language.strCommandOpenConnectionFile, string.Format(My.Language.strCommandExitProgram, System.Windows.Forms.Application.ProductName)});
                    cTaskDialog.ShowCommandBox(System.Windows.Forms.Application.ProductName, My.Language.strLoadFromSqlFailed, My.Language.strLoadFromSqlFailedContent, Misc.GetExceptionMessageRecursive(ex), "", "", commandButtons, false, eSysIcons.Error, eSysIcons.Error);
					switch (cTaskDialog.CommandButtonResult)
					{
						case 0:
							LoadConnections(withDialog, update);
							return ;
						case 1:
							My.Settings.Default.UseSQLServer = false;
							LoadConnections(true, update);
							return ;
						default:
							Application.Exit();
							return ;
					}
				}
				else
				{
					if (ex is FileNotFoundException&& !withDialog)
					{
						Runtime.MessageCollector.AddExceptionMessage(string.Format(My.Language.strConnectionsFileCouldNotBeLoadedNew, connectionsLoad.ConnectionFileName), ex, MessageClass.InformationMsg);
						NewConnections(System.Convert.ToString(connectionsLoad.ConnectionFileName));
						return ;
					}
						
					Runtime.MessageCollector.AddExceptionMessage(string.Format(My.Language.strConnectionsFileCouldNotBeLoaded, connectionsLoad.ConnectionFileName), ex);
					if (!(connectionsLoad.ConnectionFileName == GetStartupConnectionFileName()))
					{
						LoadConnections(withDialog, update);
						return ;
					}
					else
					{
						Interaction.MsgBox(string.Format(My.Language.strErrorStartupConnectionFileLoad, Constants.vbNewLine, System.Windows.Forms.Application.ProductName, GetStartupConnectionFileName(), Misc.GetExceptionMessageRecursive(ex)), (int) MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, null);
						Application.Exit();
						return ;
					}
				}
			}
		}
			
		protected static void CreateBackupFile(string fileName)
		{
			// This intentionally doesn't prune any existing backup files. We just assume the user doesn't want any new ones created.
			if (My.Settings.Default.BackupFileKeepCount == 0)
			{
				return ;
			}
				
			try
			{
				string backupFileName = string.Format(My.Settings.Default.BackupFileNameFormat, fileName, DateTime.UtcNow);
				File.Copy(fileName, backupFileName);
				PruneBackupFiles(fileName);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage(My.Language.strConnectionsFileBackupFailed, ex, MessageClass.WarningMsg);
				throw;
			}
		}
			
		protected static void PruneBackupFiles(string baseName)
		{
			string fileName = Path.GetFileName(baseName);
			string directoryName = Path.GetDirectoryName(baseName);
				
			if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(directoryName))
			{
				return ;
			}
				
			string searchPattern = string.Format(My.Settings.Default.BackupFileNameFormat, fileName, "*");
			string[] files = Directory.GetFiles(directoryName, searchPattern);
				
			if (files.Length <= My.Settings.Default.BackupFileKeepCount)
			{
				return ;
			}
				
			Array.Sort(files);
			Array.Resize(ref files, files.Length - My.Settings.Default.BackupFileKeepCount);
				
			foreach (string file in files)
			{
				System.IO.File.Delete(file);
			}
		}
			
		public static string GetDefaultStartupConnectionFileName()
		{
			string newPath = App.Info.Connections.DefaultConnectionsPath + "\\" + Info.Connections.DefaultConnectionsFile;
            #if !PORTABLE
			string oldPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData) + "\\" + (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.ProductName + "\\" + Info.Connections.DefaultConnectionsFile;
			if (File.Exists(oldPath))
			{
				return oldPath;
			}
            #endif
			return newPath;
		}
			
		public static string GetStartupConnectionFileName()
		{
			if (My.Settings.Default.LoadConsFromCustomLocation == false)
			{
				return GetDefaultStartupConnectionFileName();
			}
			else
			{
				return My.Settings.Default.CustomConsPath;
			}
		}
			
		public static void SaveConnectionsBG()
		{
			_saveUpdate = true;
				
			Thread t = new Thread(new System.Threading.ThreadStart(SaveConnectionsBGd));
			t.SetApartmentState(System.Threading.ApartmentState.STA);
			t.Start();
		}
			
		private static bool _saveUpdate = false;
		private static object _saveLock = new object();
		private static void SaveConnectionsBGd()
		{
			Monitor.Enter(_saveLock);
			SaveConnections(_saveUpdate);
			Monitor.Exit(_saveLock);
		}
			
		public static void SaveConnections(bool Update = false)
		{
			if (!IsConnectionsFileLoaded)
			{
				return;
			}
				
			bool previousTimerState = false;
				
			try
			{
				if (Update == true && My.Settings.Default.UseSQLServer == false)
				{
					return;
				}
					
				if (TimerSqlWatcher != null)
				{
					previousTimerState = TimerSqlWatcher.Enabled;
					TimerSqlWatcher.Enabled = false;
				}
					
				Config.Connections.Save conS = new Config.Connections.Save();
					
				if (!My.Settings.Default.UseSQLServer)
				{
					conS.ConnectionFileName = GetStartupConnectionFileName();
				}
					
				conS.ConnectionList = ConnectionList;
				conS.ContainerList = ContainerList;
				conS.Export = false;
				conS.SaveSecurity = new Security.Save(false);
				conS.RootTreeNode = Windows.treeForm.tvConnections.Nodes[0];
					
				if (My.Settings.Default.UseSQLServer == true)
				{
					conS.SaveFormat = Config.Connections.Save.Format.SQL;
					conS.SQLHost = System.Convert.ToString(My.Settings.Default.SQLHost);
					conS.SQLDatabaseName = System.Convert.ToString(My.Settings.Default.SQLDatabaseName);
					conS.SQLUsername = System.Convert.ToString(My.Settings.Default.SQLUser);
					conS.SQLPassword = Security.Crypt.Decrypt(System.Convert.ToString(My.Settings.Default.SQLPass), App.Info.General.EncryptionKey);
				}
					
				conS.SaveConnections();
					
				if (My.Settings.Default.UseSQLServer == true)
				{
					LastSqlUpdate = DateTime.Now;
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strConnectionsFileCouldNotBeSaved + Constants.vbNewLine + ex.Message);
			}
			finally
			{
				if (TimerSqlWatcher != null)
				{
					TimerSqlWatcher.Enabled = previousTimerState;
				}
			}
		}
			
		public static void SaveConnectionsAs()
		{
			bool previousTimerState = false;
            mRemoteNG.Config.Connections.Save connectionsSave = new mRemoteNG.Config.Connections.Save();
				
			try
			{
				if (TimerSqlWatcher != null)
				{
					previousTimerState = TimerSqlWatcher.Enabled;
					TimerSqlWatcher.Enabled = false;
				}
					
				using (SaveFileDialog saveFileDialog = new SaveFileDialog())
				{
					saveFileDialog.CheckPathExists = true;
					saveFileDialog.InitialDirectory = Info.Connections.DefaultConnectionsPath;
					saveFileDialog.FileName = Info.Connections.DefaultConnectionsFile;
					saveFileDialog.OverwritePrompt = true;
						
					List<string> fileTypes = new List<string>();
					fileTypes.AddRange(new[] {My.Language.strFiltermRemoteXML, "*.xml"});
					fileTypes.AddRange(new[] {My.Language.strFilterAll, "*.*"});
						
					saveFileDialog.Filter = string.Join("|", fileTypes.ToArray());
						
					if (!(saveFileDialog.ShowDialog(frmMain.Default) == DialogResult.OK))
					{
						return ;
					}

                    connectionsSave.SaveFormat = mRemoteNG.Config.Connections.Save.Format.mRXML;
					connectionsSave.ConnectionFileName = saveFileDialog.FileName;
					connectionsSave.Export = false;
					connectionsSave.SaveSecurity = new Security.Save();
					connectionsSave.ConnectionList = ConnectionList;
					connectionsSave.ContainerList = ContainerList;
					connectionsSave.RootTreeNode = Windows.treeForm.tvConnections.Nodes[0];
						
					connectionsSave.SaveConnections();
						
					if (saveFileDialog.FileName == GetDefaultStartupConnectionFileName())
					{
						My.Settings.Default.LoadConsFromCustomLocation = false;
					}
					else
					{
						My.Settings.Default.LoadConsFromCustomLocation = true;
						My.Settings.Default.CustomConsPath = saveFileDialog.FileName;
					}
				}
					
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage(string.Format(My.Language.strConnectionsFileCouldNotSaveAs, connectionsSave.ConnectionFileName), ex);
			}
			finally
			{
				if (TimerSqlWatcher != null)
				{
					TimerSqlWatcher.Enabled = previousTimerState;
				}
			}
		}
        #endregion
			
        #region Opening Connection
		public static Connection.Info CreateQuickConnect(string connectionString, Protocols protocol)
		{
			try
			{
				Uri uri = new Uri("dummyscheme" + System.Uri.SchemeDelimiter + connectionString);
					
				if (string.IsNullOrEmpty(uri.Host))
				{
					return null;
				}
					
				Connection.Info newConnectionInfo = new Connection.Info();
					
				if (My.Settings.Default.IdentifyQuickConnectTabs)
				{
					newConnectionInfo.Name = string.Format(My.Language.strQuick, uri.Host);
				}
				else
				{
					newConnectionInfo.Name = uri.Host;
				}
					
				newConnectionInfo.Protocol = protocol;
				newConnectionInfo.Hostname = uri.Host;
				if (uri.Port == -1)
				{
					newConnectionInfo.SetDefaultPort();
				}
				else
				{
					newConnectionInfo.Port = uri.Port;
				}
				newConnectionInfo.IsQuickConnect = true;
					
				return newConnectionInfo;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage(My.Language.strQuickConnectFailed, ex, MessageClass.ErrorMsg);
				return null;
			}
		}
			
		public static void OpenConnection()
		{
			try
			{
				OpenConnection(Connection.Info.Force.None);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strConnectionOpenFailed + Constants.vbNewLine + ex.Message);
			}
		}
			
		public static void OpenConnection(mRemoteNG.Connection.Info.Force Force)
		{
			try
			{
				if (Windows.treeForm.tvConnections.SelectedNode.Tag == null)
				{
					return;
				}
					
				if (Tree.Node.GetNodeType(Tree.Node.SelectedNode) == Tree.Node.Type.Connection | Tree.Node.GetNodeType(Tree.Node.SelectedNode) == Tree.Node.Type.PuttySession)
				{
					OpenConnection((mRemoteNG.Connection.Info)Windows.treeForm.tvConnections.SelectedNode.Tag, Force);
				}
				else if (Tree.Node.GetNodeType(Tree.Node.SelectedNode) == Tree.Node.Type.Container)
				{
					foreach (TreeNode tNode in Tree.Node.SelectedNode.Nodes)
					{
						if (Tree.Node.GetNodeType(tNode) == Tree.Node.Type.Connection | Tree.Node.GetNodeType(Tree.Node.SelectedNode) == Tree.Node.Type.PuttySession)
						{
							if (tNode.Tag != null)
							{
								OpenConnection((mRemoteNG.Connection.Info)tNode.Tag, Force);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strConnectionOpenFailed + Constants.vbNewLine + ex.Message);
			}
		}
			
		public static void OpenConnection(mRemoteNG.Connection.Info ConnectionInfo)
		{
			try
			{
				OpenConnection(ConnectionInfo, Connection.Info.Force.None);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strConnectionOpenFailed + Constants.vbNewLine + ex.Message);
			}
		}
			
		public static void OpenConnection(mRemoteNG.Connection.Info ConnectionInfo, System.Windows.Forms.Form ConnectionForm)
		{
			try
			{
				OpenConnectionFinal(ConnectionInfo, Connection.Info.Force.None, ConnectionForm);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strConnectionOpenFailed + Constants.vbNewLine + ex.Message);
			}
		}
			
		public static void OpenConnection(mRemoteNG.Connection.Info ConnectionInfo, System.Windows.Forms.Form ConnectionForm, Connection.Info.Force Force)
		{
			try
			{
				OpenConnectionFinal(ConnectionInfo, Force, ConnectionForm);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strConnectionOpenFailed + Constants.vbNewLine + ex.Message);
			}
		}
			
		public static void OpenConnection(mRemoteNG.Connection.Info ConnectionInfo, mRemoteNG.Connection.Info.Force Force)
		{
			try
			{
				OpenConnectionFinal(ConnectionInfo, Force, null);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strConnectionOpenFailed + Constants.vbNewLine + ex.Message);
			}
		}
			
		private static void OpenConnectionFinal(mRemoteNG.Connection.Info newConnectionInfo, mRemoteNG.Connection.Info.Force Force, System.Windows.Forms.Form ConForm)
		{
			try
			{
				if (newConnectionInfo.Hostname == "" && newConnectionInfo.Protocol != Connection.Protocol.Protocols.IntApp)
				{
					Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, My.Language.strConnectionOpenFailedNoHostname);
					return;
				}
					
				if (newConnectionInfo.PreExtApp != "")
				{
					Tools.ExternalTool extA = App.Runtime.GetExtAppByName(newConnectionInfo.PreExtApp);
					if (extA != null)
					{
						extA.Start(newConnectionInfo);
					}
				}
					
				if ((Force & Connection.Info.Force.DoNotJump) != Connection.Info.Force.DoNotJump)
				{
					if (SwitchToOpenConnection(newConnectionInfo))
					{
						return;
					}
				}
					
				Base newProtocol = default(Base);
				// Create connection based on protocol type
				switch (newConnectionInfo.Protocol)
				{
					case Protocols.RDP:
						newProtocol = new RDP();
						((RDP) newProtocol).tmrReconnect.Elapsed += ((RDP) newProtocol).tmrReconnect_Elapsed;
						((RDP) newProtocol).tmrReconnect.Elapsed += ((RDP) newProtocol).tmrReconnect_Elapsed;
						break;
					case Protocols.VNC:
						newProtocol = new VNC();
						break;
					case Protocols.SSH1:
						newProtocol = new SSH1();
						break;
					case Protocols.SSH2:
						newProtocol = new SSH2();
						break;
					case Protocols.Telnet:
						newProtocol = new Telnet();
						break;
					case Protocols.Rlogin:
						newProtocol = new Rlogin();
						break;
					case Protocols.RAW:
						newProtocol = new RAW();
						break;
					case Protocols.HTTP:
						newProtocol = new HTTP(newConnectionInfo.RenderingEngine);
						break;
					case Protocols.HTTPS:
						newProtocol = new HTTPS(newConnectionInfo.RenderingEngine);
						break;
					case Protocols.ICA:
						newProtocol = new ICA();
						((ICA) newProtocol).tmrReconnect.Elapsed += ((ICA) newProtocol).tmrReconnect_Elapsed;
						((ICA) newProtocol).tmrReconnect.Elapsed += ((ICA) newProtocol).tmrReconnect_Elapsed;
						break;
					case Protocols.IntApp:
						newProtocol = new IntegratedProgram();
							
						if (newConnectionInfo.ExtApp == "")
						{
							throw (new Exception(My.Language.strNoExtAppDefined));
						}
						break;
					default:
						return;
				}
					
				Control cContainer = default(Control);
				System.Windows.Forms.Form cForm = default(System.Windows.Forms.Form);
					
				string cPnl = "";
				if (newConnectionInfo.Panel == "" || (Force & Connection.Info.Force.OverridePanel) == Connection.Info.Force.OverridePanel | My.Settings.Default.AlwaysShowPanelSelectionDlg)
				{
					frmChoosePanel frmPnl = new frmChoosePanel();
					if (frmPnl.ShowDialog() == DialogResult.OK)
					{
						cPnl = frmPnl.Panel;
					}
					else
					{
						return;
					}
				}
				else
				{
					cPnl = newConnectionInfo.Panel;
				}
					
				if (ConForm == null)
				{
					cForm = WindowList.FromString(cPnl);
				}
				else
				{
					cForm = ConForm;
				}
					
				if (cForm == null)
				{
					cForm = AddPanel(cPnl);
					cForm.Focus();
				}
				else
				{
					(cForm as UI.Window.Connection).Show(frmMain.Default.pnlDock);
					(cForm as UI.Window.Connection).Focus();
				}

                cContainer = ((UI.Window.Connection)cForm).AddConnectionTab(newConnectionInfo);
					
				if (newConnectionInfo.Protocol == Connection.Protocol.Protocols.IntApp)
				{
					if (App.Runtime.GetExtAppByName(newConnectionInfo.ExtApp).Icon != null)
					{
						(cContainer as Crownwood.Magic.Controls.TabPage).Icon = App.Runtime.GetExtAppByName(newConnectionInfo.ExtApp).Icon;
					}
				}

                newProtocol.Closed += ((UI.Window.Connection)cForm).Prot_Event_Closed;
					
				newProtocol.Disconnected += Prot_Event_Disconnected;
				newProtocol.Connected += Prot_Event_Connected;
				newProtocol.Closed += Prot_Event_Closed;
				newProtocol.ErrorOccured += Prot_Event_ErrorOccured;
					
				newProtocol.InterfaceControl = new Connection.InterfaceControl(cContainer, newProtocol, newConnectionInfo);
					
				newProtocol.Force = Force;
					
				if (newProtocol.SetProps() == false)
				{
					newProtocol.Close();
					return;
				}
					
				if (newProtocol.Connect() == false)
				{
					newProtocol.Close();
					return;
				}
					
				newConnectionInfo.OpenConnections.Add(newProtocol);
					
				if (newConnectionInfo.IsQuickConnect == false)
				{
					if (newConnectionInfo.Protocol != Connection.Protocol.Protocols.IntApp)
					{
						Tree.Node.SetNodeImage(newConnectionInfo.TreeNode, Images.Enums.TreeImage.ConnectionOpen);
					}
					else
					{
						Tools.ExternalTool extApp = GetExtAppByName(newConnectionInfo.ExtApp);
						if (extApp != null)
						{
							if (extApp.TryIntegrate)
							{
								if (newConnectionInfo.TreeNode != null)
								{
									Tree.Node.SetNodeImage(newConnectionInfo.TreeNode, Images.Enums.TreeImage.ConnectionOpen);
								}
							}
						}
					}
				}
					
				frmMain.Default.SelectedConnection = newConnectionInfo;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strConnectionOpenFailed + Constants.vbNewLine + ex.Message);
			}
		}
			
		public static bool SwitchToOpenConnection(Connection.Info nCi)
		{
			mRemoteNG.Connection.InterfaceControl IC = FindConnectionContainer(nCi);
				
			if (IC != null)
			{
				(IC.FindForm() as UI.Window.Connection).Focus();
				(IC.FindForm() as UI.Window.Connection).Show(frmMain.Default.pnlDock);
				Crownwood.Magic.Controls.TabPage t = (Crownwood.Magic.Controls.TabPage) IC.Parent;
				t.Selected = true;
				return true;
			}
				
			return false;
		}
        #endregion
			
        #region Event Handlers
		public static void Prot_Event_Disconnected(object sender, string DisconnectedMessage)
		{
			try
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, string.Format(My.Language.strProtocolEventDisconnected, DisconnectedMessage), true);

                Connection.Protocol.Base Prot = (Connection.Protocol.Base)sender;
				if (Prot.InterfaceControl.Info.Protocol == Connection.Protocol.Protocols.RDP)
				{
					string[] Reason = DisconnectedMessage.Split("\r\n".ToCharArray());
					string ReasonCode = Reason[0];
					string ReasonDescription = Reason[1];
					if (System.Convert.ToInt32(ReasonCode) > 3)
					{
						if (!string.IsNullOrEmpty(ReasonDescription))
						{
							Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, My.Language.strRdpDisconnected + Constants.vbNewLine + ReasonDescription + Constants.vbNewLine + string.Format(My.Language.strErrorCode, ReasonCode));
						}
						else
						{
							Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, My.Language.strRdpDisconnected + Constants.vbNewLine + string.Format(My.Language.strErrorCode, ReasonCode));
						}
					}
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, string.Format(My.Language.strProtocolEventDisconnectFailed, ex.Message), true);
			}
		}
			
		public static void Prot_Event_Closed(object sender)
		{
			try
			{
                Connection.Protocol.Base Prot = (Connection.Protocol.Base)sender;
					
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, My.Language.strConnenctionCloseEvent, true);
					
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ReportMsg, string.Format(My.Language.strConnenctionClosedByUser, Prot.InterfaceControl.Info.Hostname, Prot.InterfaceControl.Info.Protocol.ToString(), (new Microsoft.VisualBasic.ApplicationServices.User()).Name));
					
				Prot.InterfaceControl.Info.OpenConnections.Remove(Prot);
					
				if (Prot.InterfaceControl.Info.OpenConnections.Count < 1 && Prot.InterfaceControl.Info.IsQuickConnect == false)
				{
					Tree.Node.SetNodeImage(Prot.InterfaceControl.Info.TreeNode, Images.Enums.TreeImage.ConnectionClosed);
				}
					
				if (Prot.InterfaceControl.Info.PostExtApp != "")
				{
					Tools.ExternalTool extA = App.Runtime.GetExtAppByName(Prot.InterfaceControl.Info.PostExtApp);
					if (extA != null)
					{
						extA.Start(Prot.InterfaceControl.Info);
					}
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strConnenctionCloseEventFailed + Constants.vbNewLine + ex.Message, true);
			}
		}
			
		public static void Prot_Event_Connected(object sender)
		{
            mRemoteNG.Connection.Protocol.Base prot = (Connection.Protocol.Base)sender;
				
			Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, My.Language.strConnectionEventConnected, true);
			Runtime.MessageCollector.AddMessage(Messages.MessageClass.ReportMsg, string.Format(My.Language.strConnectionEventConnectedDetail, prot.InterfaceControl.Info.Hostname, prot.InterfaceControl.Info.Protocol.ToString(), (new Microsoft.VisualBasic.ApplicationServices.User()).Name, prot.InterfaceControl.Info.Description, prot.InterfaceControl.Info.UserField));
		}
			
		public static void Prot_Event_ErrorOccured(object sender, string ErrorMessage)
		{
			try
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, My.Language.strConnectionEventErrorOccured, true);

                Connection.Protocol.Base Prot = (Connection.Protocol.Base)sender;
					
				if (Prot.InterfaceControl.Info.Protocol == Connection.Protocol.Protocols.RDP)
				{
					if (System.Convert.ToInt32(ErrorMessage) > -1)
					{
						Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, string.Format(My.Language.strConnectionRdpErrorDetail, ErrorMessage, Connection.Protocol.RDP.FatalErrors.GetError(ErrorMessage)));
					}
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strConnectionEventConnectionFailed + Constants.vbNewLine + ex.Message, true);
			}
		}
        #endregion
			
        #region External Apps
		public static Tools.ExternalTool GetExtAppByName(string Name)
		{
			foreach (Tools.ExternalTool extA in ExternalTools)
			{
				if (extA.DisplayName == Name)
				{
					return extA;
				}
			}
				
			return null;
		}
        #endregion
			
        #region Misc
		public static void GoToURL(string URL)
		{
			mRemoteNG.Connection.Info cI = new mRemoteNG.Connection.Info();
				
			cI.Name = "";
			cI.Hostname = URL;
			if (URL.StartsWith("https:"))
			{
				cI.Protocol = Connection.Protocol.Protocols.HTTPS;
			}
			else
			{
				cI.Protocol = Connection.Protocol.Protocols.HTTP;
			}
			cI.SetDefaultPort();
			cI.IsQuickConnect = true;
				
			App.Runtime.OpenConnection(cI, mRemoteNG.Connection.Info.Force.DoNotJump);
		}
			
		public static void GoToWebsite()
		{
			GoToURL(App.Info.General.URLHome);
		}
			
		public static void GoToDonate()
		{
			GoToURL(App.Info.General.URLDonate);
		}
			
		public static void GoToForum()
		{
			GoToURL(App.Info.General.URLForum);
		}
			
		public static void GoToBugs()
		{
			GoToURL(App.Info.General.URLBugs);
		}
			
		public static void Report(string Text)
		{
			try
			{
				StreamWriter sWr = new StreamWriter((new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.DirectoryPath + "\\Report.log", true);
				sWr.WriteLine(Text);
				sWr.Close();
			}
			catch (Exception)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strLogWriteToFileFailed);
			}
		}
			
		public static bool SaveReport()
		{
			StreamReader streamReader = null;
			StreamWriter streamWriter = null;
			try
			{
				streamReader = new StreamReader((new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.DirectoryPath + "\\Report.log");
				string text = streamReader.ReadToEnd();
				streamReader.Close();
					
				streamWriter = new StreamWriter(App.Info.General.ReportingFilePath, true);
				streamWriter.Write(text);
					
				return true;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strLogWriteToFileFinalLocationFailed + Constants.vbNewLine + ex.Message, true);
				return false;
			}
			finally
			{
				if (streamReader != null)
				{
					streamReader.Close();
					streamReader.Dispose();
				}
				if (streamWriter != null)
				{
					streamWriter.Close();
					streamWriter.Dispose();
				}
			}
		}
			
		public static Connection.InterfaceControl FindConnectionContainer(Connection.Info ConI)
		{
			if (ConI.OpenConnections.Count > 0)
			{
				for (int i = 0; i <= WindowList.Count - 1; i++)
				{
					if (WindowList[i] is UI.Window.Connection)
					{
                        UI.Window.Connection conW = (UI.Window.Connection)WindowList[i];
							
						if (conW.TabController != null)
						{
							foreach (Crownwood.Magic.Controls.TabPage t in conW.TabController.TabPages)
							{
								if (t.Controls[0] != null)
								{
									if (t.Controls[0] is Connection.InterfaceControl)
									{
                                        Connection.InterfaceControl IC = (Connection.InterfaceControl)t.Controls[0];
										if (IC.Info == ConI)
										{
											return IC;
										}
									}
								}
							}
						}
					}
				}
			}
				
			return null;
		}
			
		// Override the font of all controls in a container with the default font based on the OS version
		public static void FontOverride(Control ctlParent)
		{
			Control ctlChild = default(Control);
			foreach (Control tempLoopVar_ctlChild in ctlParent.Controls)
			{
				ctlChild = tempLoopVar_ctlChild;
				ctlChild.Font = new System.Drawing.Font(SystemFonts.MessageBoxFont.Name, ctlChild.Font.Size, ctlChild.Font.Style, ctlChild.Font.Unit, ctlChild.Font.GdiCharSet);
				if (ctlChild.Controls.Count > 0)
				{
					FontOverride(ctlChild);
				}
			}
		}
        #endregion
			
        #region SQL Watcher
		private static void tmrSqlWatcher_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			Tools.Misc.IsSQLUpdateAvailableAsync();
		}
			
		private static void SQLUpdateCheckFinished(bool UpdateAvailable)
		{
			if (UpdateAvailable == true)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, My.Language.strSqlUpdateCheckUpdateAvailable, true);
				LoadConnectionsBG();
			}
		}
        #endregion
	}
}
