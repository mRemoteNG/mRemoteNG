using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using mRemoteNG.Forms;
using mRemoteNG.Config;
using log4net;
using mRemoteNG.Messages;
using mRemoteNG.Connection;
using mRemoteNG.Tools;
using mRemoteNG.Forms.OptionsPages;
using PSTaskDialog;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;
using Crownwood;
using System.Threading;
using System.Xml;
using System.Environment;
using System.Management;
using Microsoft.Win32;
using Timer = System.Timers.Timer;

namespace mRemoteNG.App
{
	public class Runtime
	{
		private Runtime()
		{
			// Fix Warning 292 CA1053 : Microsoft.Design : Because type 'Native' contains only 'static' ('Shared' in Visual Basic) members, add a default private constructor to prevent the compiler from adding a default public constructor.
		}

		#region "Public Properties"
		public static frmMain MainForm { get; set; }

		private static Connection.List _connectionList;
		public static List ConnectionList {
			get { return _connectionList; }
			set { _connectionList = value; }
		}

		private static Connection.List _previousConnectionList;
		public static List PreviousConnectionList {
			get { return _previousConnectionList; }
			set { _previousConnectionList = value; }
		}

		private static Container.List _containerList;
		public static Container.List ContainerList {
			get { return _containerList; }
			set { _containerList = value; }
		}

		private static Container.List _previousContainerList;
		public static Container.List PreviousContainerList {
			get { return _previousContainerList; }
			set { _previousContainerList = value; }
		}

		private static Credential.List _credentialList;
		public static Credential.List CredentialList {
			get { return _credentialList; }
			set { _credentialList = value; }
		}

		private static Credential.List _previousCredentialList;
		public static Credential.List PreviousCredentialList {
			get { return _previousCredentialList; }
			set { _previousCredentialList = value; }
		}


		private static UI.Window.List _windowList;
		public static UI.Window.List WindowList {
			get { return _windowList; }
			set { _windowList = value; }
		}

		private static Messages.Collector _messageCollector;
		public static Collector MessageCollector {
			get { return _messageCollector; }
			set { _messageCollector = value; }
		}

		private static Tools.Controls.NotificationAreaIcon _notificationAreaIcon;
		public static Tools.Controls.NotificationAreaIcon NotificationAreaIcon {
			get { return _notificationAreaIcon; }
			set { _notificationAreaIcon = value; }
		}

		private static Tools.SystemMenu _systemMenu;
		public static SystemMenu SystemMenu {
			get { return _systemMenu; }
			set { _systemMenu = value; }
		}

		private static log4net.ILog _log;
		public static ILog Log {
			get { return _log; }
			set { _log = value; }
		}

		private static bool _isConnectionsFileLoaded;
		public static bool IsConnectionsFileLoaded {
			get { return _isConnectionsFileLoaded; }
			set { _isConnectionsFileLoaded = value; }
		}

		private System.Timers.Timer withEventsField__timerSqlWatcher;
		private static System.Timers.Timer _timerSqlWatcher {
			get { return withEventsField__timerSqlWatcher; }
			set {
				if (withEventsField__timerSqlWatcher != null) {
					withEventsField__timerSqlWatcher.Elapsed -= tmrSqlWatcher_Elapsed;
				}
				withEventsField__timerSqlWatcher = value;
				if (withEventsField__timerSqlWatcher != null) {
					withEventsField__timerSqlWatcher.Elapsed += tmrSqlWatcher_Elapsed;
				}
			}
		}
		public static Timer TimerSqlWatcher {
			get { return _timerSqlWatcher; }
			set { _timerSqlWatcher = value; }
		}

		private static System.DateTime _lastSqlUpdate;
		public static System.DateTime LastSqlUpdate {
			get { return _lastSqlUpdate; }
			set { _lastSqlUpdate = value; }
		}

		private static string _lastSelected;
		public static string LastSelected {
			get { return _lastSelected; }
			set { _lastSelected = value; }
		}

		private static mRemoteNG.Connection.Info _defaultConnection;
		public static Connection.Info DefaultConnection {
			get { return _defaultConnection; }
			set { _defaultConnection = value; }
		}

		private static mRemoteNG.Connection.Info.Inheritance _defaultInheritance;
		public static Connection.Info.Inheritance DefaultInheritance {
			get { return _defaultInheritance; }
			set { _defaultInheritance = value; }
		}

		private static ArrayList _externalTools = new ArrayList();
		public static ArrayList ExternalTools {
			get { return _externalTools; }
			set { _externalTools = value; }
		}

		#endregion

		#region "Classes"
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
				try {
					switch (windowType) {
						case mRemoteNG.UI.Window.Type.About:
							if (aboutForm == null || aboutForm.IsDisposed) {
								aboutForm = new UI.Window.About(aboutPanel);
								aboutPanel = aboutForm;
							}

							aboutForm.Show(My.MyProject.Forms.frmMain.pnlDock);
							break;
						case mRemoteNG.UI.Window.Type.ActiveDirectoryImport:
							if (adimportForm == null || adimportForm.IsDisposed) {
								adimportForm = new UI.Window.ActiveDirectoryImport(adimportPanel);
								adimportPanel = adimportForm;
							}

							adimportPanel.Show(My.MyProject.Forms.frmMain.pnlDock);
							break;
						case mRemoteNG.UI.Window.Type.Options:
							using (OptionsForm optionsForm = new OptionsForm()) {
								optionsForm.ShowDialog(frmMain);
							}

							break;
						case mRemoteNG.UI.Window.Type.SSHTransfer:
							sshtransferForm = new UI.Window.SSHTransfer(sshtransferPanel);
							sshtransferPanel = sshtransferForm;

							sshtransferForm.Show(My.MyProject.Forms.frmMain.pnlDock);
							break;
						case mRemoteNG.UI.Window.Type.Update:
							if (updateForm == null || updateForm.IsDisposed) {
								updateForm = new UI.Window.Update(updatePanel);
								updatePanel = updateForm;
							}

							updateForm.Show(My.MyProject.Forms.frmMain.pnlDock);
							break;
						case mRemoteNG.UI.Window.Type.Help:
							if (helpForm == null || helpForm.IsDisposed) {
								helpForm = new UI.Window.Help(helpPanel);
								helpPanel = helpForm;
							}

							helpForm.Show(My.MyProject.Forms.frmMain.pnlDock);
							break;
						case mRemoteNG.UI.Window.Type.ExternalApps:
							if (externalappsForm == null || externalappsForm.IsDisposed) {
								externalappsForm = new UI.Window.ExternalTools(externalappsPanel);
								externalappsPanel = externalappsForm;
							}

							externalappsForm.Show(My.MyProject.Forms.frmMain.pnlDock);
							break;
						case mRemoteNG.UI.Window.Type.PortScan:
							portscanForm = new UI.Window.PortScan(portscanPanel, portScanImport);
							portscanPanel = portscanForm;

							portscanForm.Show(My.MyProject.Forms.frmMain.pnlDock);
							break;
						case mRemoteNG.UI.Window.Type.UltraVNCSC:
							if (ultravncscForm == null || ultravncscForm.IsDisposed) {
								ultravncscForm = new UI.Window.UltraVNCSC(ultravncscPanel);
								ultravncscPanel = ultravncscForm;
							}

							ultravncscForm.Show(My.MyProject.Forms.frmMain.pnlDock);
							break;
						case mRemoteNG.UI.Window.Type.ComponentsCheck:
							if (componentscheckForm == null || componentscheckForm.IsDisposed) {
								componentscheckForm = new UI.Window.ComponentsCheck(componentscheckPanel);
								componentscheckPanel = componentscheckForm;
							}

							componentscheckForm.Show(My.MyProject.Forms.frmMain.pnlDock);
							break;
						case mRemoteNG.UI.Window.Type.Announcement:
							if (AnnouncementForm == null || AnnouncementForm.IsDisposed) {
								AnnouncementForm = new UI.Window.Announcement(AnnouncementPanel);
								AnnouncementPanel = AnnouncementForm;
							}

							AnnouncementForm.Show(My.MyProject.Forms.frmMain.pnlDock);
							break;
					}
				} catch (Exception ex) {
					Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "App.Runtime.Windows.Show() failed." + Constants.vbNewLine + ex.Message, true);
				}
			}

			public static void ShowUpdatesTab()
			{
				using (OptionsForm optionsForm = new OptionsForm()) {
					optionsForm.ShowDialog(frmMain, typeof(UpdatesPage));
				}
			}
		}

		public class Screens
		{
			public static void SendFormToScreen(Screen Screen)
			{
				bool wasMax = false;

				if (My.MyProject.Forms.frmMain.WindowState == FormWindowState.Maximized) {
					wasMax = true;
					My.MyProject.Forms.frmMain.WindowState = FormWindowState.Normal;
				}

				My.MyProject.Forms.frmMain.Location = Screen.Bounds.Location;

				if (wasMax) {
					My.MyProject.Forms.frmMain.WindowState = FormWindowState.Maximized;
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
				RegistryKey regKey = null;

				bool isFipsPolicyEnabled = false;

				// Windows XP/Windows Server 2003
				regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\Lsa");
				if (regKey != null) {
					if (!(regKey.GetValue("FIPSAlgorithmPolicy") == 0))
						isFipsPolicyEnabled = true;
				}

				// Windows Vista/Windows Server 2008 and newer
				regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\Lsa\\FIPSAlgorithmPolicy");
				if (regKey != null) {
					if (!(regKey.GetValue("Enabled") == 0))
						isFipsPolicyEnabled = true;
				}

				if (isFipsPolicyEnabled) {
					MessageBox.Show(frmMain, string.Format(mRemoteNG.My.Language.strErrorFipsPolicyIncompatible, mRemoteNG.My.MyProject.Application.Info.ProductName), mRemoteNG.My.MyProject.Application.Info.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					Environment.Exit(1);
				}
			}

			private static void CheckLenovoAutoScrollUtility()
			{
				if (!mRemoteNG.My.Settings.CompatibilityWarnLenovoAutoScrollUtility)
					return;

				Process[] proccesses = {
					
				};
				try {
					proccesses = Process.GetProcessesByName("virtscrl");
				} catch {
				}
				if (proccesses.Length == 0)
					return;

				cTaskDialog.MessageBox(Application.ProductName, mRemoteNG.My.Language.strCompatibilityProblemDetected, string.Format(mRemoteNG.My.Language.strCompatibilityLenovoAutoScrollUtilityDetected, System.Windows.Forms.Application.ProductName), "", "", mRemoteNG.My.Language.strCheckboxDoNotShowThisMessageAgain, eTaskDialogButtons.OK, eSysIcons.Warning, null);
				if (cTaskDialog.VerificationChecked) {
					mRemoteNG.My.Settings.CompatibilityWarnLenovoAutoScrollUtility = false;
				}
			}

			public static void CreatePanels()
			{
				Windows.configForm = new UI.Window.Config(Windows.configPanel);
				Windows.configPanel = Windows.configForm;

				Windows.treeForm = new UI.Window.Tree(Windows.treePanel);
				Windows.treePanel = Windows.treeForm;
				mRemoteNG.Tree.Node.TreeView = Windows.treeForm.tvConnections;

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
				My.MyProject.Forms.frmMain.pnlDock.Visible = false;

				My.MyProject.Forms.frmMain.pnlDock.DockLeftPortion = My.MyProject.Forms.frmMain.pnlDock.Width * 0.2;
				My.MyProject.Forms.frmMain.pnlDock.DockRightPortion = My.MyProject.Forms.frmMain.pnlDock.Width * 0.2;
				My.MyProject.Forms.frmMain.pnlDock.DockTopPortion = My.MyProject.Forms.frmMain.pnlDock.Height * 0.25;
				My.MyProject.Forms.frmMain.pnlDock.DockBottomPortion = My.MyProject.Forms.frmMain.pnlDock.Height * 0.25;

				Windows.treePanel.Show(My.MyProject.Forms.frmMain.pnlDock, DockState.DockLeft);
				Windows.configPanel.Show(My.MyProject.Forms.frmMain.pnlDock);
				Windows.configPanel.DockTo(Windows.treePanel.Pane, DockStyle.Bottom, -1);

				Windows.screenshotForm.Hide();

				My.MyProject.Forms.frmMain.pnlDock.Visible = true;
			}

			public static void GetConnectionIcons()
			{
				string iPath = mRemoteNG.My.MyProject.Application.Info.DirectoryPath + "\\Icons\\";

				if (Directory.Exists(iPath) == false) {
					return;
				}

				foreach (string f in Directory.GetFiles(iPath, "*.ico", SearchOption.AllDirectories)) {
					FileInfo fInfo = new FileInfo(f);

					Array.Resize(ref mRemoteNG.Connection.Icon.Icons, mRemoteNG.Connection.Icon.Icons.Length + 1);
					mRemoteNG.Connection.Icon.Icons.SetValue(fInfo.Name.Replace(".ico", ""), mRemoteNG.Connection.Icon.Icons.Length - 1);
				}
			}

			public static void CreateLogger()
			{
				log4net.Config.XmlConfigurator.Configure();

				string logFilePath = null;
				#if !PORTABLE
				logFilePath = Path.Combine(Environment.GetFolderPath(SpecialFolder.LocalApplicationData), Application.ProductName);
				#else
				logFilePath = Application.StartupPath;
				#endif
				string logFileName = Path.ChangeExtension(Application.ProductName, ".log");
				string logFile = Path.Combine(logFilePath, logFileName);

				log4net.Repository.ILoggerRepository repository = LogManager.GetRepository();
				log4net.Appender.IAppender[] appenders = repository.GetAppenders();
				log4net.Appender.FileAppender fileAppender = null;
				foreach (log4net.Appender.IAppender appender in appenders) {
					fileAppender = appender as Appender.FileAppender;
					if (!(fileAppender == null || !(fileAppender.Name == "LogFileAppender"))) {
						fileAppender.File = logFile;
						fileAppender.ActivateOptions();
					}
				}

				Runtime.Log = LogManager.GetLogger("Logger");

				#if !PORTABLE
				if (mRemoteNG.My.Settings.WriteLogFile) {
					Runtime.Log.InfoFormat("{0} {1} starting.", Application.ProductName, Application.ProductVersion);
					#else
					Runtime.Log.InfoFormat("{0} {1} {2} starting.", Application.ProductName, Application.ProductVersion, mRemoteNG.My.Language.strLabelPortableEdition);
					#endif
					Runtime.Log.InfoFormat("Command Line: {0}", Environment.GetCommandLineArgs());

					string osVersion = string.Empty;
					string servicePack = string.Empty;
					try {
						foreach (ManagementObject managementObject in new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem WHERE Primary=True").Get()) {
							osVersion = managementObject.GetPropertyValue("Caption").Trim();
							int servicePackNumber = managementObject.GetPropertyValue("ServicePackMajorVersion");
							if (!(servicePackNumber == 0))
								servicePack = string.Format("Service Pack {0}", servicePackNumber);
						}
					} catch (Exception ex) {
						Runtime.Log.WarnFormat("Error retrieving operating system information from WMI. {0}", ex.Message);
					}

					string architecture = string.Empty;
					try {
						foreach (ManagementObject managementObject in new ManagementObjectSearcher("SELECT * FROM Win32_Processor WHERE DeviceID='CPU0'").Get()) {
							int addressWidth = managementObject.GetPropertyValue("AddressWidth");
							architecture = string.Format("{0}-bit", addressWidth);
						}
					} catch (Exception ex) {
						Runtime.Log.WarnFormat("Error retrieving operating system address width from WMI. {0}", ex.Message);
					}

					Runtime.Log.InfoFormat(string.Join(" ", Array.FindAll(new string[] {
						osVersion,
						servicePack,
						architecture
					}, s => !string.IsNullOrEmpty(s))));

					Runtime.Log.InfoFormat("Microsoft .NET CLR {0}", Version.ToString());
					Runtime.Log.InfoFormat("System Culture: {0}/{1}", Thread.CurrentThread.CurrentUICulture.Name, Thread.CurrentThread.CurrentUICulture.NativeName);
				}
			}

			private static Update _appUpdate;
			public static void CheckForUpdate()
			{
				if (_appUpdate == null) {
					_appUpdate = new Update();
				} else if (_appUpdate.IsGetUpdateInfoRunning) {
					return;
				}

				System.DateTime nextUpdateCheck = mRemoteNG.My.Settings.CheckForUpdatesLastCheck.Add(TimeSpan.FromDays(mRemoteNG.My.Settings.CheckForUpdatesFrequencyDays));
				if (!mRemoteNG.My.Settings.UpdatePending & System.DateTime.UtcNow < nextUpdateCheck)
					return;

				_appUpdate.GetUpdateInfoCompletedEvent += GetUpdateInfoCompleted;
				_appUpdate.GetUpdateInfoAsync();
			}

			private static void GetUpdateInfoCompleted(object sender, AsyncCompletedEventArgs e)
			{
				if (Runtime.MainForm.InvokeRequired) {
					Runtime.MainForm.Invoke(new AsyncCompletedEventHandler(GetUpdateInfoCompleted), new object[] {
						sender,
						e
					});
					return;
				}

				try {
					_appUpdate.GetUpdateInfoCompletedEvent -= GetUpdateInfoCompleted;

					if (e.Cancelled)
						return;
					if (e.Error != null)
						throw e.Error;

					if (_appUpdate.IsUpdateAvailable())
						Windows.Show(mRemoteNG.UI.Window.Type.Update);
				} catch (Exception ex) {
					Runtime.MessageCollector.AddExceptionMessage("GetUpdateInfoCompleted() failed.", ex, MessageClass.ErrorMsg, true);
				}
			}

			public static void CheckForAnnouncement()
			{
				if (_appUpdate == null) {
					_appUpdate = new Update();
				} else if (_appUpdate.IsGetAnnouncementInfoRunning) {
					return;
				}

				_appUpdate.GetAnnouncementInfoCompletedEvent += GetAnnouncementInfoCompleted;
				_appUpdate.GetAnnouncementInfoAsync();
			}

			private static void GetAnnouncementInfoCompleted(object sender, AsyncCompletedEventArgs e)
			{
				if (Runtime.MainForm.InvokeRequired) {
					Runtime.MainForm.Invoke(new AsyncCompletedEventHandler(GetAnnouncementInfoCompleted), new object[] {
						sender,
						e
					});
					return;
				}

				try {
					_appUpdate.GetAnnouncementInfoCompletedEvent -= GetAnnouncementInfoCompleted;

					if (e.Cancelled)
						return;
					if (e.Error != null)
						throw e.Error;

					if (_appUpdate.IsAnnouncementAvailable())
						Windows.Show(mRemoteNG.UI.Window.Type.Announcement);
				} catch (Exception ex) {
					Runtime.MessageCollector.AddExceptionMessage("GetAnnouncementInfoCompleted() failed.", ex, MessageClass.ErrorMsg, true);
				}
			}

			public static void ParseCommandLineArgs()
			{
				try {
					Tools.Misc.CMDArguments cmd = new Tools.Misc.CMDArguments(Environment.GetCommandLineArgs());

					string ConsParam = "";
					if (cmd["cons"] != null) {
						ConsParam = "cons";
					}
					if (cmd["c"] != null) {
						ConsParam = "c";
					}

					string ResetPosParam = "";
					if (cmd["resetpos"] != null) {
						ResetPosParam = "resetpos";
					}
					if (cmd["rp"] != null) {
						ResetPosParam = "rp";
					}

					string ResetPanelsParam = "";
					if (cmd["resetpanels"] != null) {
						ResetPanelsParam = "resetpanels";
					}
					if (cmd["rpnl"] != null) {
						ResetPanelsParam = "rpnl";
					}

					string ResetToolbarsParam = "";
					if (cmd["resettoolbar"] != null) {
						ResetToolbarsParam = "resettoolbar";
					}
					if (cmd["rtbr"] != null) {
						ResetToolbarsParam = "rtbr";
					}

					if (cmd["reset"] != null) {
						ResetPosParam = "rp";
						ResetPanelsParam = "rpnl";
						ResetToolbarsParam = "rtbr";
					}

					string NoReconnectParam = "";
					if (cmd["noreconnect"] != null) {
						NoReconnectParam = "noreconnect";
					}
					if (cmd["norc"] != null) {
						NoReconnectParam = "norc";
					}

					if (!string.IsNullOrEmpty(ConsParam)) {
						if (File.Exists(cmd[ConsParam]) == false) {
							if (File.Exists(mRemoteNG.My.MyProject.Application.Info.DirectoryPath + "\\" + cmd[ConsParam])) {
								mRemoteNG.My.Settings.LoadConsFromCustomLocation = true;
								mRemoteNG.My.Settings.CustomConsPath = mRemoteNG.My.MyProject.Application.Info.DirectoryPath + "\\" + cmd[ConsParam];
								return;
							} else if (File.Exists(mRemoteNG.App.Info.Connections.DefaultConnectionsPath + "\\" + cmd[ConsParam])) {
								mRemoteNG.My.Settings.LoadConsFromCustomLocation = true;
								mRemoteNG.My.Settings.CustomConsPath = mRemoteNG.App.Info.Connections.DefaultConnectionsPath + "\\" + cmd[ConsParam];
								return;
							}
						} else {
							mRemoteNG.My.Settings.LoadConsFromCustomLocation = true;
							mRemoteNG.My.Settings.CustomConsPath = cmd[ConsParam];
							return;
						}
					}

					if (!string.IsNullOrEmpty(ResetPosParam)) {
						mRemoteNG.My.Settings.MainFormKiosk = false;
						mRemoteNG.My.Settings.MainFormLocation = new Point(999, 999);
						mRemoteNG.My.Settings.MainFormSize = new Size(900, 600);
						mRemoteNG.My.Settings.MainFormState = FormWindowState.Normal;
					}

					if (!string.IsNullOrEmpty(ResetPanelsParam)) {
						mRemoteNG.My.Settings.ResetPanels = true;
					}

					if (!string.IsNullOrEmpty(NoReconnectParam)) {
						mRemoteNG.My.Settings.NoReconnect = true;
					}

					if (!string.IsNullOrEmpty(ResetToolbarsParam)) {
						mRemoteNG.My.Settings.ResetToolbars = true;
					}
				} catch (Exception ex) {
					Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strCommandLineArgsCouldNotBeParsed + Constants.vbNewLine + ex.Message);
				}
			}

			public static void CreateSQLUpdateHandlerAndStartTimer()
			{
				if (mRemoteNG.My.Settings.UseSQLServer == true) {
					mRemoteNG.Tools.Misc.SQLUpdateCheckFinished += Runtime.SQLUpdateCheckFinished;
					Runtime.TimerSqlWatcher = new System.Timers.Timer(3000);
					Runtime.TimerSqlWatcher.Start();
				}
			}

			public static void DestroySQLUpdateHandlerAndStopTimer()
			{
				try {
					Runtime.LastSqlUpdate = null;
					mRemoteNG.Tools.Misc.SQLUpdateCheckFinished -= Runtime.SQLUpdateCheckFinished;
					if (Runtime.TimerSqlWatcher != null) {
						Runtime.TimerSqlWatcher.Stop();
						Runtime.TimerSqlWatcher.Close();
					}
				} catch (Exception ex) {
				}
			}
		}

		public class Shutdown
		{
			public static void Quit(string updateFilePath = null)
			{
				_updateFilePath = updateFilePath;
				My.MyProject.Forms.frmMain.Close();
			}

			public static void Cleanup()
			{
				try {
					mRemoteNG.Config.Putty.Sessions.StopWatcher();

					if (Runtime.NotificationAreaIcon != null) {
						if (Runtime.NotificationAreaIcon.Disposed == false) {
							Runtime.NotificationAreaIcon.Dispose();
						}
					}

					if (mRemoteNG.My.Settings.SaveConsOnExit)
						Runtime.SaveConnections();

					mRemoteNG.Config.Settings.Save.Save();

					IeBrowserEmulation.Unregister();
				} catch (Exception ex) {
					Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, mRemoteNG.My.Language.strSettingsCouldNotBeSavedOrTrayDispose + Constants.vbNewLine + ex.Message, true);
				}
			}

			public static void StartUpdate()
			{
				if (!UpdatePending())
					return;
				try {
					Process.Start(_updateFilePath);
				} catch (Exception ex) {
					Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "The update could not be started." + Constants.vbNewLine + ex.Message, true);
				}
			}


			private static string _updateFilePath = null;
			public static bool UpdatePending {
				get { return !string.IsNullOrEmpty(_updateFilePath); }
			}
		}
		#endregion

		#region "Default Connection"
		public static mRemoteNG.Connection.Info DefaultConnectionFromSettings()
		{
			DefaultConnection = new mRemoteNG.Connection.Info();
			DefaultConnection.IsDefault = true;

			return DefaultConnection;
		}

		public static void DefaultConnectionToSettings()
		{
			var _with1 = DefaultConnection;
			mRemoteNG.My.Settings.ConDefaultDescription = _with1.Description;
			mRemoteNG.My.Settings.ConDefaultIcon = _with1.Icon;
			mRemoteNG.My.Settings.ConDefaultUsername = _with1.Username;
			mRemoteNG.My.Settings.ConDefaultPassword = _with1.Password;
			mRemoteNG.My.Settings.ConDefaultDomain = _with1.Domain;
			mRemoteNG.My.Settings.ConDefaultProtocol = _with1.Protocol.ToString;
			mRemoteNG.My.Settings.ConDefaultPuttySession = _with1.PuttySession;
			mRemoteNG.My.Settings.ConDefaultICAEncryptionStrength = _with1.ICAEncryption.ToString;
			mRemoteNG.My.Settings.ConDefaultRDPAuthenticationLevel = _with1.RDPAuthenticationLevel.ToString;
			mRemoteNG.My.Settings.ConDefaultLoadBalanceInfo = _with1.LoadBalanceInfo;
			mRemoteNG.My.Settings.ConDefaultUseConsoleSession = _with1.UseConsoleSession;
			mRemoteNG.My.Settings.ConDefaultUseCredSsp = _with1.UseCredSsp;
			mRemoteNG.My.Settings.ConDefaultRenderingEngine = _with1.RenderingEngine.ToString;
			mRemoteNG.My.Settings.ConDefaultResolution = _with1.Resolution.ToString;
			mRemoteNG.My.Settings.ConDefaultAutomaticResize = _with1.AutomaticResize;
			mRemoteNG.My.Settings.ConDefaultColors = _with1.Colors.ToString;
			mRemoteNG.My.Settings.ConDefaultCacheBitmaps = _with1.CacheBitmaps;
			mRemoteNG.My.Settings.ConDefaultDisplayWallpaper = _with1.DisplayWallpaper;
			mRemoteNG.My.Settings.ConDefaultDisplayThemes = _with1.DisplayThemes;
			mRemoteNG.My.Settings.ConDefaultEnableFontSmoothing = _with1.EnableFontSmoothing;
			mRemoteNG.My.Settings.ConDefaultEnableDesktopComposition = _with1.EnableDesktopComposition;
			mRemoteNG.My.Settings.ConDefaultRedirectKeys = _with1.RedirectKeys;
			mRemoteNG.My.Settings.ConDefaultRedirectDiskDrives = _with1.RedirectDiskDrives;
			mRemoteNG.My.Settings.ConDefaultRedirectPrinters = _with1.RedirectPrinters;
			mRemoteNG.My.Settings.ConDefaultRedirectPorts = _with1.RedirectPorts;
			mRemoteNG.My.Settings.ConDefaultRedirectSmartCards = _with1.RedirectSmartCards;
			mRemoteNG.My.Settings.ConDefaultRedirectSound = _with1.RedirectSound.ToString;
			mRemoteNG.My.Settings.ConDefaultPreExtApp = _with1.PreExtApp;
			mRemoteNG.My.Settings.ConDefaultPostExtApp = _with1.PostExtApp;
			mRemoteNG.My.Settings.ConDefaultMacAddress = _with1.MacAddress;
			mRemoteNG.My.Settings.ConDefaultUserField = _with1.UserField;
			mRemoteNG.My.Settings.ConDefaultVNCAuthMode = _with1.VNCAuthMode.ToString;
			mRemoteNG.My.Settings.ConDefaultVNCColors = _with1.VNCColors.ToString;
			mRemoteNG.My.Settings.ConDefaultVNCCompression = _with1.VNCCompression.ToString;
			mRemoteNG.My.Settings.ConDefaultVNCEncoding = _with1.VNCEncoding.ToString;
			mRemoteNG.My.Settings.ConDefaultVNCProxyIP = _with1.VNCProxyIP;
			mRemoteNG.My.Settings.ConDefaultVNCProxyPassword = _with1.VNCProxyPassword;
			mRemoteNG.My.Settings.ConDefaultVNCProxyPort = _with1.VNCProxyPort;
			mRemoteNG.My.Settings.ConDefaultVNCProxyType = _with1.VNCProxyType.ToString;
			mRemoteNG.My.Settings.ConDefaultVNCProxyUsername = _with1.VNCProxyUsername;
			mRemoteNG.My.Settings.ConDefaultVNCSmartSizeMode = _with1.VNCSmartSizeMode.ToString;
			mRemoteNG.My.Settings.ConDefaultVNCViewOnly = _with1.VNCViewOnly;
			mRemoteNG.My.Settings.ConDefaultExtApp = _with1.ExtApp;
			mRemoteNG.My.Settings.ConDefaultRDGatewayUsageMethod = _with1.RDGatewayUsageMethod.ToString;
			mRemoteNG.My.Settings.ConDefaultRDGatewayHostname = _with1.RDGatewayHostname;
			mRemoteNG.My.Settings.ConDefaultRDGatewayUsername = _with1.RDGatewayUsername;
			mRemoteNG.My.Settings.ConDefaultRDGatewayPassword = _with1.RDGatewayPassword;
			mRemoteNG.My.Settings.ConDefaultRDGatewayDomain = _with1.RDGatewayDomain;
			mRemoteNG.My.Settings.ConDefaultRDGatewayUseConnectionCredentials = _with1.RDGatewayUseConnectionCredentials.ToString;
		}
		#endregion

		#region "Default Inheritance"
		public static mRemoteNG.Connection.Info.Inheritance DefaultInheritanceFromSettings()
		{
			DefaultInheritance = new mRemoteNG.Connection.Info.Inheritance(null);
			DefaultInheritance.IsDefault = true;

			return DefaultInheritance;
		}

		public static void DefaultInheritanceToSettings()
		{
			var _with2 = DefaultInheritance;
			mRemoteNG.My.Settings.InhDefaultDescription = _with2.Description;
			mRemoteNG.My.Settings.InhDefaultIcon = _with2.Icon;
			mRemoteNG.My.Settings.InhDefaultPanel = _with2.Panel;
			mRemoteNG.My.Settings.InhDefaultUsername = _with2.Username;
			mRemoteNG.My.Settings.InhDefaultPassword = _with2.Password;
			mRemoteNG.My.Settings.InhDefaultDomain = _with2.Domain;
			mRemoteNG.My.Settings.InhDefaultProtocol = _with2.Protocol;
			mRemoteNG.My.Settings.InhDefaultPort = _with2.Port;
			mRemoteNG.My.Settings.InhDefaultPuttySession = _with2.PuttySession;
			mRemoteNG.My.Settings.InhDefaultUseConsoleSession = _with2.UseConsoleSession;
			mRemoteNG.My.Settings.InhDefaultUseCredSsp = _with2.UseCredSsp;
			mRemoteNG.My.Settings.InhDefaultRenderingEngine = _with2.RenderingEngine;
			mRemoteNG.My.Settings.InhDefaultICAEncryptionStrength = _with2.ICAEncryption;
			mRemoteNG.My.Settings.InhDefaultRDPAuthenticationLevel = _with2.RDPAuthenticationLevel;
			mRemoteNG.My.Settings.InhDefaultLoadBalanceInfo = _with2.LoadBalanceInfo;
			mRemoteNG.My.Settings.InhDefaultResolution = _with2.Resolution;
			mRemoteNG.My.Settings.InhDefaultAutomaticResize = _with2.AutomaticResize;
			mRemoteNG.My.Settings.InhDefaultColors = _with2.Colors;
			mRemoteNG.My.Settings.InhDefaultCacheBitmaps = _with2.CacheBitmaps;
			mRemoteNG.My.Settings.InhDefaultDisplayWallpaper = _with2.DisplayWallpaper;
			mRemoteNG.My.Settings.InhDefaultDisplayThemes = _with2.DisplayThemes;
			mRemoteNG.My.Settings.InhDefaultEnableFontSmoothing = _with2.EnableFontSmoothing;
			mRemoteNG.My.Settings.InhDefaultEnableDesktopComposition = _with2.EnableDesktopComposition;
			mRemoteNG.My.Settings.InhDefaultRedirectKeys = _with2.RedirectKeys;
			mRemoteNG.My.Settings.InhDefaultRedirectDiskDrives = _with2.RedirectDiskDrives;
			mRemoteNG.My.Settings.InhDefaultRedirectPrinters = _with2.RedirectPrinters;
			mRemoteNG.My.Settings.InhDefaultRedirectPorts = _with2.RedirectPorts;
			mRemoteNG.My.Settings.InhDefaultRedirectSmartCards = _with2.RedirectSmartCards;
			mRemoteNG.My.Settings.InhDefaultRedirectSound = _with2.RedirectSound;
			mRemoteNG.My.Settings.InhDefaultPreExtApp = _with2.PreExtApp;
			mRemoteNG.My.Settings.InhDefaultPostExtApp = _with2.PostExtApp;
			mRemoteNG.My.Settings.InhDefaultMacAddress = _with2.MacAddress;
			mRemoteNG.My.Settings.InhDefaultUserField = _with2.UserField;
			// VNC inheritance
			mRemoteNG.My.Settings.InhDefaultVNCAuthMode = _with2.VNCAuthMode;
			mRemoteNG.My.Settings.InhDefaultVNCColors = _with2.VNCColors;
			mRemoteNG.My.Settings.InhDefaultVNCCompression = _with2.VNCCompression;
			mRemoteNG.My.Settings.InhDefaultVNCEncoding = _with2.VNCEncoding;
			mRemoteNG.My.Settings.InhDefaultVNCProxyIP = _with2.VNCProxyIP;
			mRemoteNG.My.Settings.InhDefaultVNCProxyPassword = _with2.VNCProxyPassword;
			mRemoteNG.My.Settings.InhDefaultVNCProxyPort = _with2.VNCProxyPort;
			mRemoteNG.My.Settings.InhDefaultVNCProxyType = _with2.VNCProxyType;
			mRemoteNG.My.Settings.InhDefaultVNCProxyUsername = _with2.VNCProxyUsername;
			mRemoteNG.My.Settings.InhDefaultVNCSmartSizeMode = _with2.VNCSmartSizeMode;
			mRemoteNG.My.Settings.InhDefaultVNCViewOnly = _with2.VNCViewOnly;
			// Ext. App inheritance
			mRemoteNG.My.Settings.InhDefaultExtApp = _with2.ExtApp;
			// RDP gateway inheritance
			mRemoteNG.My.Settings.InhDefaultRDGatewayUsageMethod = _with2.RDGatewayUsageMethod;
			mRemoteNG.My.Settings.InhDefaultRDGatewayHostname = _with2.RDGatewayHostname;
			mRemoteNG.My.Settings.InhDefaultRDGatewayUsername = _with2.RDGatewayUsername;
			mRemoteNG.My.Settings.InhDefaultRDGatewayPassword = _with2.RDGatewayPassword;
			mRemoteNG.My.Settings.InhDefaultRDGatewayDomain = _with2.RDGatewayDomain;
			mRemoteNG.My.Settings.InhDefaultRDGatewayUseConnectionCredentials = _with2.RDGatewayUseConnectionCredentials;
		}
		#endregion

		#region "Panels"
		public static System.Windows.Forms.Form AddPanel(string title = "", bool noTabber = false)
		{
			try {
				if (string.IsNullOrEmpty(title)) {
					title = mRemoteNG.My.Language.strNewPanel;
				}

				DockContent pnlcForm = new DockContent();
				UI.Window.Connection cForm = new UI.Window.Connection(pnlcForm);
				pnlcForm = cForm;

				//create context menu
				ContextMenuStrip cMen = new ContextMenuStrip();

				//create rename item
				ToolStripMenuItem cMenRen = new ToolStripMenuItem();
				cMenRen.Text = mRemoteNG.My.Language.strRename;
				cMenRen.Image = mRemoteNG.My.Resources.Rename;
				cMenRen.Tag = pnlcForm;
				cMenRen.Click += cMenConnectionPanelRename_Click;

				ToolStripMenuItem cMenScreens = new ToolStripMenuItem();
				cMenScreens.Text = mRemoteNG.My.Language.strSendTo;
				cMenScreens.Image = mRemoteNG.My.Resources.Monitor;
				cMenScreens.Tag = pnlcForm;
				cMenScreens.DropDownItems.Add("Dummy");
				cMenScreens.DropDownOpening += cMenConnectionPanelScreens_DropDownOpening;

				cMen.Items.AddRange(new ToolStripMenuItem[] {
					cMenRen,
					cMenScreens
				});

				pnlcForm.TabPageContextMenuStrip = cMen;

				(cForm as UI.Window.Connection).SetFormText(title.Replace("&", "&&"));

				pnlcForm.Show(My.MyProject.Forms.frmMain.pnlDock, DockState.Document);

				if (noTabber) {
					(cForm as UI.Window.Connection).TabController.Dispose();
				} else {
					WindowList.Add(cForm);
				}

				return cForm;
			} catch (Exception ex) {
				MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "Couldn't add panel" + Constants.vbNewLine + ex.Message);
				return null;
			}
		}

		private static void cMenConnectionPanelRename_Click(System.Object sender, System.EventArgs e)
		{
			try {
				UI.Window.Connection conW = null;
				conW = sender.Tag;

				string nTitle = Interaction.InputBox(mRemoteNG.My.Language.strNewTitle + ":", , sender.Tag.Text.Replace("&&", "&"));

				if (!string.IsNullOrEmpty(nTitle)) {
					conW.SetFormText(nTitle.Replace("&", "&&"));
				}
			} catch (Exception ex) {
				MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "Couldn't rename panel" + Constants.vbNewLine + ex.Message);
			}
		}

		private static void cMenConnectionPanelScreens_DropDownOpening(System.Object sender, System.EventArgs e)
		{
			try {
				ToolStripMenuItem cMenScreens = sender;
				cMenScreens.DropDownItems.Clear();

				for (int i = 0; i <= Screen.AllScreens.Length - 1; i++) {
					ToolStripMenuItem cMenScreen = new ToolStripMenuItem(mRemoteNG.My.Language.strScreen + " " + i + 1);
					cMenScreen.Tag = new ArrayList();
					cMenScreen.Image = mRemoteNG.My.Resources.Monitor_GoTo;
					(cMenScreen.Tag as ArrayList).Add(Screen.AllScreens[i]);
					(cMenScreen.Tag as ArrayList).Add(cMenScreens.Tag);
					cMenScreen.Click += cMenConnectionPanelScreen_Click;

					cMenScreens.DropDownItems.Add(cMenScreen);
				}
			} catch (Exception ex) {
				MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "Couldn't enumerate screens" + Constants.vbNewLine + ex.Message);
			}
		}

		private static void cMenConnectionPanelScreen_Click(object sender, System.EventArgs e)
		{
			try {
				Screen screen = (sender as ToolStripMenuItem).Tag(0);
				DockContent panel = (sender as ToolStripMenuItem).Tag(1);
				Screens.SendPanelToScreen(panel, screen);
			} catch (Exception ex) {
			}
		}
		#endregion

		#region "Credential Loading/Saving"

		public static void LoadCredentials()
		{
		}
		#endregion

		#region "Connections Loading/Saving"
		public static void NewConnections(string filename)
		{
			try {
				ConnectionList = new Connection.List();
				ContainerList = new Container.List();

				mRemoteNG.Config.Connections.Load connectionsLoad = new mRemoteNG.Config.Connections.Load();

				if (filename == GetDefaultStartupConnectionFileName()) {
					mRemoteNG.My.Settings.LoadConsFromCustomLocation = false;
				} else {
					mRemoteNG.My.Settings.LoadConsFromCustomLocation = true;
					mRemoteNG.My.Settings.CustomConsPath = filename;
				}

				Directory.CreateDirectory(Path.GetDirectoryName(filename));

				// Use File.Open with FileMode.CreateNew so that we don't overwrite an existing file
				using (FileStream fileStream = File.Open(filename, FileMode.CreateNew, FileAccess.Write, FileShare.None)) {
					using (XmlTextWriter xmlTextWriter = new XmlTextWriter(fileStream, System.Text.Encoding.UTF8)) {
						var _with3 = xmlTextWriter;
						_with3.Formatting = Formatting.Indented;
						_with3.Indentation = 4;

						_with3.WriteStartDocument();

						_with3.WriteStartElement("Connections");
						// Do not localize
						_with3.WriteAttributeString("Name", mRemoteNG.My.Language.strConnections);
						_with3.WriteAttributeString("Export", "", "False");
						_with3.WriteAttributeString("Protected", "", "GiUis20DIbnYzWPcdaQKfjE2H5jh//L5v4RGrJMGNXuIq2CttB/d/BxaBP2LwRhY");
						_with3.WriteAttributeString("ConfVersion", "", "2.5");

						_with3.WriteEndElement();
						_with3.WriteEndDocument();

						_with3.Close();
					}
				}

				connectionsLoad.ConnectionList = ConnectionList;
				connectionsLoad.ContainerList = ContainerList;

				mRemoteNG.Tree.Node.ResetTree();

				connectionsLoad.RootTreeNode = Windows.treeForm.tvConnections.Nodes[0];

				// Load config
				connectionsLoad.ConnectionFileName = filename;
				connectionsLoad.Load(false);

				Windows.treeForm.tvConnections.SelectedNode = connectionsLoad.RootTreeNode;
			} catch (Exception ex) {
				MessageCollector.AddExceptionMessage(mRemoteNG.My.Language.strCouldNotCreateNewConnectionsFile, ex, MessageClass.ErrorMsg);
			}
		}

		private static void LoadConnectionsBG(bool WithDialog = false, bool Update = false)
		{
			_withDialog = false;
			_loadUpdate = true;

			Thread t = new Thread(LoadConnectionsBGd);
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

			try {
				bool tmrWasEnabled = false;
				if (TimerSqlWatcher != null) {
					tmrWasEnabled = TimerSqlWatcher.Enabled;

					if (TimerSqlWatcher.Enabled == true) {
						TimerSqlWatcher.Stop();
					}
				}

				if (ConnectionList != null & ContainerList != null) {
					PreviousConnectionList = ConnectionList.Copy();
					PreviousContainerList = ContainerList.Copy();
				}

				ConnectionList = new Connection.List();
				ContainerList = new Container.List();

				if (!mRemoteNG.My.Settings.UseSQLServer) {
					if (withDialog) {
						OpenFileDialog loadDialog = mRemoteNG.Tools.Controls.ConnectionsLoadDialog();

						if (loadDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
							connectionsLoad.ConnectionFileName = loadDialog.FileName;
						} else {
							return;
						}
					} else {
						connectionsLoad.ConnectionFileName = GetStartupConnectionFileName();
					}

					CreateBackupFile(connectionsLoad.ConnectionFileName);
				}

				connectionsLoad.ConnectionList = ConnectionList;
				connectionsLoad.ContainerList = ContainerList;

				if (PreviousConnectionList != null & PreviousContainerList != null) {
					connectionsLoad.PreviousConnectionList = PreviousConnectionList;
					connectionsLoad.PreviousContainerList = PreviousContainerList;
				}

				if (update == true) {
					connectionsLoad.PreviousSelected = LastSelected;
				}

				mRemoteNG.Tree.Node.ResetTree();

				connectionsLoad.RootTreeNode = Windows.treeForm.tvConnections.Nodes[0];

				connectionsLoad.UseSQL = mRemoteNG.My.Settings.UseSQLServer;
				connectionsLoad.SQLHost = mRemoteNG.My.Settings.SQLHost;
				connectionsLoad.SQLDatabaseName = mRemoteNG.My.Settings.SQLDatabaseName;
				connectionsLoad.SQLUsername = mRemoteNG.My.Settings.SQLUser;
				connectionsLoad.SQLPassword = mRemoteNG.Security.Crypt.Decrypt(mRemoteNG.My.Settings.SQLPass, Info.General.EncryptionKey);
				connectionsLoad.SQLUpdate = update;

				connectionsLoad.Load(false);

				if (mRemoteNG.My.Settings.UseSQLServer == true) {
					LastSqlUpdate = DateAndTime.Now;
				} else {
					if (connectionsLoad.ConnectionFileName == GetDefaultStartupConnectionFileName()) {
						mRemoteNG.My.Settings.LoadConsFromCustomLocation = false;
					} else {
						mRemoteNG.My.Settings.LoadConsFromCustomLocation = true;
						mRemoteNG.My.Settings.CustomConsPath = connectionsLoad.ConnectionFileName;
					}
				}

				if (tmrWasEnabled & TimerSqlWatcher != null) {
					TimerSqlWatcher.Start();
				}
			} catch (Exception ex) {
				if (mRemoteNG.My.Settings.UseSQLServer) {
					MessageCollector.AddExceptionMessage(mRemoteNG.My.Language.strLoadFromSqlFailed, ex);
					string commandButtons = string.Join("|", {
						mRemoteNG.My.Language.strCommandTryAgain,
						mRemoteNG.My.Language.strCommandOpenConnectionFile,
						string.Format(mRemoteNG.My.Language.strCommandExitProgram, Application.ProductName)
					});
					cTaskDialog.ShowCommandBox(Application.ProductName, mRemoteNG.My.Language.strLoadFromSqlFailed, mRemoteNG.My.Language.strLoadFromSqlFailedContent, Misc.GetExceptionMessageRecursive(ex), "", "", commandButtons, false, eSysIcons.Error, null);
					switch (cTaskDialog.CommandButtonResult) {
						case 0:
							LoadConnections(withDialog, update);
							return;
						case 1:
							mRemoteNG.My.Settings.UseSQLServer = false;
							LoadConnections(true, update);
							return;
						default:
							Application.Exit();
							return;
					}
				} else {
					if (ex is FileNotFoundException & !withDialog) {
						MessageCollector.AddExceptionMessage(string.Format(mRemoteNG.My.Language.strConnectionsFileCouldNotBeLoadedNew, connectionsLoad.ConnectionFileName), ex, MessageClass.InformationMsg);
						NewConnections(connectionsLoad.ConnectionFileName);
						return;
					}

					MessageCollector.AddExceptionMessage(string.Format(mRemoteNG.My.Language.strConnectionsFileCouldNotBeLoaded, connectionsLoad.ConnectionFileName), ex);
					if (!(connectionsLoad.ConnectionFileName == GetStartupConnectionFileName())) {
						LoadConnections(withDialog, update);
						return;
					} else {
						Interaction.MsgBox(string.Format(mRemoteNG.My.Language.strErrorStartupConnectionFileLoad, Constants.vbNewLine, Application.ProductName, GetStartupConnectionFileName(), Misc.GetExceptionMessageRecursive(ex)), MsgBoxStyle.OkOnly + MsgBoxStyle.Critical);
						Application.Exit();
						return;
					}
				}
			}
		}

		protected static void CreateBackupFile(string fileName)
		{
			// This intentionally doesn't prune any existing backup files. We just assume the user doesn't want any new ones created.
			if (mRemoteNG.My.Settings.BackupFileKeepCount == 0)
				return;

			try {
				string backupFileName = string.Format(mRemoteNG.My.Settings.BackupFileNameFormat, fileName, DateTime.UtcNow);
				File.Copy(fileName, backupFileName);
				PruneBackupFiles(fileName);
			} catch (Exception ex) {
				MessageCollector.AddExceptionMessage(mRemoteNG.My.Language.strConnectionsFileBackupFailed, ex, MessageClass.WarningMsg);
				throw;
			}
		}

		protected static void PruneBackupFiles(string baseName)
		{
			string fileName = Path.GetFileName(baseName);
			string directoryName = Path.GetDirectoryName(baseName);

			if (string.IsNullOrEmpty(fileName) | string.IsNullOrEmpty(directoryName))
				return;

			string searchPattern = string.Format(mRemoteNG.My.Settings.BackupFileNameFormat, fileName, "*");
			string[] files = Directory.GetFiles(directoryName, searchPattern);

			if (files.Length <= mRemoteNG.My.Settings.BackupFileKeepCount)
				return;

			Array.Sort(files);
			Array.Resize(ref files, files.Length - mRemoteNG.My.Settings.BackupFileKeepCount);

			foreach (string file in files) {
				System.IO.File.Delete(file);
			}
		}

		public static string GetDefaultStartupConnectionFileName()
		{
			string newPath = mRemoteNG.App.Info.Connections.DefaultConnectionsPath + "\\" + Info.Connections.DefaultConnectionsFile;
			#if Not PORTABLE
			string oldPath = Environment.GetFolderPath(SpecialFolder.LocalApplicationData) + "\\" + mRemoteNG.My.MyProject.Application.Info.ProductName + "\\" + Info.Connections.DefaultConnectionsFile;
			if (File.Exists(oldPath)) {
				return oldPath;
			}
			#endif
			return newPath;
		}

		public static string GetStartupConnectionFileName()
		{
			if (mRemoteNG.My.Settings.LoadConsFromCustomLocation == false) {
				return GetDefaultStartupConnectionFileName();
			} else {
				return mRemoteNG.My.Settings.CustomConsPath;
			}
		}

		public static void SaveConnectionsBG()
		{
			_saveUpdate = true;

			Thread t = new Thread(SaveConnectionsBGd);
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
				return;

			bool previousTimerState = false;

			try {
				if (Update == true & mRemoteNG.My.Settings.UseSQLServer == false) {
					return;
				}

				if (TimerSqlWatcher != null) {
					previousTimerState = TimerSqlWatcher.Enabled;
					TimerSqlWatcher.Enabled = false;
				}

				Config.Connections.Save conS = new Config.Connections.Save();

				if (!mRemoteNG.My.Settings.UseSQLServer) {
					conS.ConnectionFileName = GetStartupConnectionFileName();
				}

				conS.ConnectionList = ConnectionList;
				conS.ContainerList = ContainerList;
				conS.Export = false;
				conS.SaveSecurity = new Security.Save(false);
				conS.RootTreeNode = Windows.treeForm.tvConnections.Nodes[0];

				if (mRemoteNG.My.Settings.UseSQLServer == true) {
					conS.SaveFormat = mRemoteNG.Config.Connections.Save.Format.SQL;
					conS.SQLHost = mRemoteNG.My.Settings.SQLHost;
					conS.SQLDatabaseName = mRemoteNG.My.Settings.SQLDatabaseName;
					conS.SQLUsername = mRemoteNG.My.Settings.SQLUser;
					conS.SQLPassword = mRemoteNG.Security.Crypt.Decrypt(mRemoteNG.My.Settings.SQLPass, mRemoteNG.App.Info.General.EncryptionKey);
				}

				conS.Save();

				if (mRemoteNG.My.Settings.UseSQLServer == true) {
					LastSqlUpdate = DateAndTime.Now;
				}
			} catch (Exception ex) {
				MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strConnectionsFileCouldNotBeSaved + Constants.vbNewLine + ex.Message);
			} finally {
				if (TimerSqlWatcher != null) {
					TimerSqlWatcher.Enabled = previousTimerState;
				}
			}
		}

		public static void SaveConnectionsAs()
		{
			bool previousTimerState = false;
			mRemoteNG.Config.Connections.Save connectionsSave = new mRemoteNG.Config.Connections.Save();

			try {
				if (TimerSqlWatcher != null) {
					previousTimerState = TimerSqlWatcher.Enabled;
					TimerSqlWatcher.Enabled = false;
				}

				using (SaveFileDialog saveFileDialog = new SaveFileDialog()) {
					var _with4 = saveFileDialog;
					_with4.CheckPathExists = true;
					_with4.InitialDirectory = Info.Connections.DefaultConnectionsPath;
					_with4.FileName = Info.Connections.DefaultConnectionsFile;
					_with4.OverwritePrompt = true;

					List<string> fileTypes = new List<string>();
					fileTypes.AddRange({
						mRemoteNG.My.Language.strFiltermRemoteXML,
						"*.xml"
					});
					fileTypes.AddRange({
						mRemoteNG.My.Language.strFilterAll,
						"*.*"
					});

					_with4.Filter = string.Join("|", fileTypes.ToArray());

					if (!(saveFileDialog.ShowDialog(frmMain) == DialogResult.OK))
						return;

					var _with5 = connectionsSave;
					_with5.SaveFormat = mRemoteNG.Config.Connections.Save.Format.mRXML;
					_with5.ConnectionFileName = saveFileDialog.FileName;
					_with5.Export = false;
					_with5.SaveSecurity = new Security.Save();
					_with5.ConnectionList = ConnectionList;
					_with5.ContainerList = ContainerList;
					_with5.RootTreeNode = Windows.treeForm.tvConnections.Nodes[0];

					connectionsSave.Save();

					if (saveFileDialog.FileName == GetDefaultStartupConnectionFileName()) {
						mRemoteNG.My.Settings.LoadConsFromCustomLocation = false;
					} else {
						mRemoteNG.My.Settings.LoadConsFromCustomLocation = true;
						mRemoteNG.My.Settings.CustomConsPath = saveFileDialog.FileName;
					}
				}
			} catch (Exception ex) {
				MessageCollector.AddExceptionMessage(string.Format(mRemoteNG.My.Language.strConnectionsFileCouldNotSaveAs, connectionsSave.ConnectionFileName), ex);
			} finally {
				if (TimerSqlWatcher != null) {
					TimerSqlWatcher.Enabled = previousTimerState;
				}
			}
		}
		#endregion

		#region "Opening Connection"
		public static Connection.Info CreateQuickConnect(string connectionString, mRemoteNG.Connection.Protocol.Protocols protocol)
		{
			try {
				Uri uri = new Uri("dummyscheme" + uri.SchemeDelimiter + connectionString);

				if (string.IsNullOrEmpty(uri.Host))
					return null;

				Connection.Info newConnectionInfo = new Connection.Info();

				if (mRemoteNG.My.Settings.IdentifyQuickConnectTabs) {
					newConnectionInfo.Name = string.Format(mRemoteNG.My.Language.strQuick, uri.Host);
				} else {
					newConnectionInfo.Name = uri.Host;
				}

				newConnectionInfo.Protocol = protocol;
				newConnectionInfo.Hostname = uri.Host;
				if (uri.Port == -1) {
					newConnectionInfo.SetDefaultPort();
				} else {
					newConnectionInfo.Port = uri.Port;
				}
				newConnectionInfo.IsQuickConnect = true;

				return newConnectionInfo;
			} catch (Exception ex) {
				MessageCollector.AddExceptionMessage(mRemoteNG.My.Language.strQuickConnectFailed, ex, MessageClass.ErrorMsg);
				return null;
			}
		}

		public static void OpenConnection()
		{
			try {
				OpenConnection(mRemoteNG.Connection.Info.Force.None);
			} catch (Exception ex) {
				MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strConnectionOpenFailed + Constants.vbNewLine + ex.Message);
			}
		}

		public static void OpenConnection(mRemoteNG.Connection.Info.Force Force)
		{
			try {
				if (Windows.treeForm.tvConnections.SelectedNode.Tag == null) {
					return;
				}

				if (mRemoteNG.Tree.Node.GetNodeType(mRemoteNG.Tree.Node.SelectedNode) == mRemoteNG.Tree.Node.Type.Connection | mRemoteNG.Tree.Node.GetNodeType(mRemoteNG.Tree.Node.SelectedNode) == mRemoteNG.Tree.Node.Type.PuttySession) {
					OpenConnection(Windows.treeForm.tvConnections.SelectedNode.Tag, Force);
				} else if (mRemoteNG.Tree.Node.GetNodeType(mRemoteNG.Tree.Node.SelectedNode) == mRemoteNG.Tree.Node.Type.Container) {
					foreach (TreeNode tNode in mRemoteNG.Tree.Node.SelectedNode.Nodes) {
						if (mRemoteNG.Tree.Node.GetNodeType(tNode) == mRemoteNG.Tree.Node.Type.Connection | mRemoteNG.Tree.Node.GetNodeType(mRemoteNG.Tree.Node.SelectedNode) == mRemoteNG.Tree.Node.Type.PuttySession) {
							if (tNode.Tag != null) {
								OpenConnection(tNode.Tag, Force);
							}
						}
					}
				}
			} catch (Exception ex) {
				MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strConnectionOpenFailed + Constants.vbNewLine + ex.Message);
			}
		}

		public static void OpenConnection(mRemoteNG.Connection.Info ConnectionInfo)
		{
			try {
				OpenConnection(ConnectionInfo, mRemoteNG.Connection.Info.Force.None);
			} catch (Exception ex) {
				MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strConnectionOpenFailed + Constants.vbNewLine + ex.Message);
			}
		}

		public static void OpenConnection(mRemoteNG.Connection.Info ConnectionInfo, System.Windows.Forms.Form ConnectionForm)
		{
			try {
				OpenConnectionFinal(ConnectionInfo, mRemoteNG.Connection.Info.Force.None, ConnectionForm);
			} catch (Exception ex) {
				MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strConnectionOpenFailed + Constants.vbNewLine + ex.Message);
			}
		}

		public static void OpenConnection(mRemoteNG.Connection.Info ConnectionInfo, System.Windows.Forms.Form ConnectionForm, Connection.Info.Force Force)
		{
			try {
				OpenConnectionFinal(ConnectionInfo, Force, ConnectionForm);
			} catch (Exception ex) {
				MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strConnectionOpenFailed + Constants.vbNewLine + ex.Message);
			}
		}

		public static void OpenConnection(mRemoteNG.Connection.Info ConnectionInfo, mRemoteNG.Connection.Info.Force Force)
		{
			try {
				OpenConnectionFinal(ConnectionInfo, Force, null);
			} catch (Exception ex) {
				MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strConnectionOpenFailed + Constants.vbNewLine + ex.Message);
			}
		}

		private static void OpenConnectionFinal(mRemoteNG.Connection.Info newConnectionInfo, mRemoteNG.Connection.Info.Force Force, System.Windows.Forms.Form ConForm)
		{
			try {
				if (string.IsNullOrEmpty(newConnectionInfo.Hostname) & newConnectionInfo.Protocol != mRemoteNG.Connection.Protocol.Protocols.IntApp) {
					MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.WarningMsg, mRemoteNG.My.Language.strConnectionOpenFailedNoHostname);
					return;
				}

				if (!string.IsNullOrEmpty(newConnectionInfo.PreExtApp)) {
					Tools.ExternalTool extA = mRemoteNG.App.Runtime.GetExtAppByName(newConnectionInfo.PreExtApp);
					if (extA != null) {
						extA.Start(newConnectionInfo);
					}
				}

				if ((Force & mRemoteNG.Connection.Info.Force.DoNotJump) != mRemoteNG.Connection.Info.Force.DoNotJump) {
					if (SwitchToOpenConnection(newConnectionInfo)) {
						return;
					}
				}

				mRemoteNG.Connection.Protocol.Base newProtocol = null;
				// Create connection based on protocol type
				switch (newConnectionInfo.Protocol) {
					case mRemoteNG.Connection.Protocol.Protocols.RDP:
						newProtocol = new mRemoteNG.Connection.Protocol.RDP();
						break;
					case mRemoteNG.Connection.Protocol.Protocols.VNC:
						newProtocol = new mRemoteNG.Connection.Protocol.VNC();
						break;
					case mRemoteNG.Connection.Protocol.Protocols.SSH1:
						newProtocol = new mRemoteNG.Connection.Protocol.SSH1();
						break;
					case mRemoteNG.Connection.Protocol.Protocols.SSH2:
						newProtocol = new mRemoteNG.Connection.Protocol.SSH2();
						break;
					case mRemoteNG.Connection.Protocol.Protocols.Telnet:
						newProtocol = new mRemoteNG.Connection.Protocol.Telnet();
						break;
					case mRemoteNG.Connection.Protocol.Protocols.Rlogin:
						newProtocol = new mRemoteNG.Connection.Protocol.Rlogin();
						break;
					case mRemoteNG.Connection.Protocol.Protocols.RAW:
						newProtocol = new mRemoteNG.Connection.Protocol.RAW();
						break;
					case mRemoteNG.Connection.Protocol.Protocols.HTTP:
						newProtocol = new mRemoteNG.Connection.Protocol.HTTP(newConnectionInfo.RenderingEngine);
						break;
					case mRemoteNG.Connection.Protocol.Protocols.HTTPS:
						newProtocol = new mRemoteNG.Connection.Protocol.HTTPS(newConnectionInfo.RenderingEngine);
						break;
					case mRemoteNG.Connection.Protocol.Protocols.ICA:
						newProtocol = new mRemoteNG.Connection.Protocol.ICA();
						break;
					case mRemoteNG.Connection.Protocol.Protocols.IntApp:
						newProtocol = new mRemoteNG.Connection.Protocol.IntegratedProgram();

						if (string.IsNullOrEmpty(newConnectionInfo.ExtApp)) {
							throw new Exception(mRemoteNG.My.Language.strNoExtAppDefined);
						}
						break;
					default:
						return;

						break;
				}

				Control cContainer = null;
				System.Windows.Forms.Form cForm = null;

				string cPnl = null;
				if (string.IsNullOrEmpty(newConnectionInfo.Panel) | (Force & mRemoteNG.Connection.Info.Force.OverridePanel) == mRemoteNG.Connection.Info.Force.OverridePanel | mRemoteNG.My.Settings.AlwaysShowPanelSelectionDlg) {
					frmChoosePanel frmPnl = new frmChoosePanel();
					if (frmPnl.ShowDialog() == DialogResult.OK) {
						cPnl = frmPnl.Panel;
					} else {
						return;
					}
				} else {
					cPnl = newConnectionInfo.Panel;
				}

				if (ConForm == null) {
					cForm = WindowList.FromString(cPnl);
				} else {
					cForm = ConForm;
				}

				if (cForm == null) {
					cForm = AddPanel(cPnl);
					cForm.Focus();
				} else {
					(cForm as UI.Window.Connection).Show(My.MyProject.Forms.frmMain.pnlDock);
					(cForm as UI.Window.Connection).Focus();
				}

				cContainer = (cForm as UI.Window.Connection).AddConnectionTab(newConnectionInfo);

				if (newConnectionInfo.Protocol == mRemoteNG.Connection.Protocol.Protocols.IntApp) {
					if (mRemoteNG.App.Runtime.GetExtAppByName(newConnectionInfo.ExtApp).Icon != null) {
						(cContainer as Magic.Controls.TabPage).Icon = mRemoteNG.App.Runtime.GetExtAppByName(newConnectionInfo.ExtApp).Icon;
					}
				}

				newProtocol.Closed += (cForm as UI.Window.Connection).Prot_Event_Closed;

				newProtocol.Disconnected += Prot_Event_Disconnected;
				newProtocol.Connected += Prot_Event_Connected;
				newProtocol.Closed += Prot_Event_Closed;
				newProtocol.ErrorOccured += Prot_Event_ErrorOccured;

				newProtocol.InterfaceControl = new Connection.InterfaceControl(cContainer, newProtocol, newConnectionInfo);

				newProtocol.Force = Force;

				if (newProtocol.SetProps() == false) {
					newProtocol.Close();
					return;
				}

				if (newProtocol.Connect() == false) {
					newProtocol.Close();
					return;
				}

				newConnectionInfo.OpenConnections.Add(newProtocol);

				if (newConnectionInfo.IsQuickConnect == false) {
					if (newConnectionInfo.Protocol != mRemoteNG.Connection.Protocol.Protocols.IntApp) {
						mRemoteNG.Tree.Node.SetNodeImage(newConnectionInfo.TreeNode, mRemoteNG.Images.Enums.TreeImage.ConnectionOpen);
					} else {
						Tools.ExternalTool extApp = GetExtAppByName(newConnectionInfo.ExtApp);
						if (extApp != null) {
							if (extApp.TryIntegrate) {
								if (newConnectionInfo.TreeNode != null) {
									mRemoteNG.Tree.Node.SetNodeImage(newConnectionInfo.TreeNode, mRemoteNG.Images.Enums.TreeImage.ConnectionOpen);
								}
							}
						}
					}
				}

				My.MyProject.Forms.frmMain.SelectedConnection = newConnectionInfo;
			} catch (Exception ex) {
				MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strConnectionOpenFailed + Constants.vbNewLine + ex.Message);
			}
		}

		public static bool SwitchToOpenConnection(Connection.Info nCi)
		{
			mRemoteNG.Connection.InterfaceControl IC = FindConnectionContainer(nCi);

			if (IC != null) {
				(IC.FindForm() as UI.Window.Connection).Focus();
				(IC.FindForm() as UI.Window.Connection).Show(My.MyProject.Forms.frmMain.pnlDock);
				Crownwood.Magic.Controls.TabPage t = IC.Parent;
				t.Selected = true;
				return true;
			}

			return false;
		}
		#endregion

		#region "Event Handlers"
		public static void Prot_Event_Disconnected(object sender, string DisconnectedMessage)
		{
			try {
				MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.InformationMsg, string.Format(mRemoteNG.My.Language.strProtocolEventDisconnected, DisconnectedMessage), true);

				Connection.Protocol.Base Prot = sender;
				if (Prot.InterfaceControl.Info.Protocol == mRemoteNG.Connection.Protocol.Protocols.RDP) {
					string[] Reason = DisconnectedMessage.Split(Constants.vbCrLf);
					string ReasonCode = Reason[0];
					string ReasonDescription = Reason[1];
					if (ReasonCode > 3) {
						if (!string.IsNullOrEmpty(ReasonDescription)) {
							MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.WarningMsg, mRemoteNG.My.Language.strRdpDisconnected + Constants.vbNewLine + ReasonDescription + Constants.vbNewLine + string.Format(mRemoteNG.My.Language.strErrorCode, ReasonCode));
						} else {
							MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.WarningMsg, mRemoteNG.My.Language.strRdpDisconnected + Constants.vbNewLine + string.Format(mRemoteNG.My.Language.strErrorCode, ReasonCode));
						}
					}
				}
			} catch (Exception ex) {
				MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, string.Format(mRemoteNG.My.Language.strProtocolEventDisconnectFailed, ex.Message), true);
			}
		}

		public static void Prot_Event_Closed(object sender)
		{
			try {
				Connection.Protocol.Base Prot = sender;

				MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.InformationMsg, mRemoteNG.My.Language.strConnenctionCloseEvent, true);

				MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ReportMsg, string.Format(mRemoteNG.My.Language.strConnenctionClosedByUser, Prot.InterfaceControl.Info.Hostname, Prot.InterfaceControl.Info.Protocol.ToString(), mRemoteNG.My.MyProject.User.Name));

				Prot.InterfaceControl.Info.OpenConnections.Remove(Prot);

				if (Prot.InterfaceControl.Info.OpenConnections.Count < 1 & Prot.InterfaceControl.Info.IsQuickConnect == false) {
					mRemoteNG.Tree.Node.SetNodeImage(Prot.InterfaceControl.Info.TreeNode, mRemoteNG.Images.Enums.TreeImage.ConnectionClosed);
				}

				if (!string.IsNullOrEmpty(Prot.InterfaceControl.Info.PostExtApp)) {
					Tools.ExternalTool extA = mRemoteNG.App.Runtime.GetExtAppByName(Prot.InterfaceControl.Info.PostExtApp);
					if (extA != null) {
						extA.Start(Prot.InterfaceControl.Info);
					}
				}
			} catch (Exception ex) {
				MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strConnenctionCloseEventFailed + Constants.vbNewLine + ex.Message, true);
			}
		}

		public static void Prot_Event_Connected(object sender)
		{
			mRemoteNG.Connection.Protocol.Base prot = sender;

			MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.InformationMsg, mRemoteNG.My.Language.strConnectionEventConnected, true);
			MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ReportMsg, string.Format(mRemoteNG.My.Language.strConnectionEventConnectedDetail, prot.InterfaceControl.Info.Hostname, prot.InterfaceControl.Info.Protocol.ToString(), mRemoteNG.My.MyProject.User.Name, prot.InterfaceControl.Info.Description, prot.InterfaceControl.Info.UserField));
		}

		public static void Prot_Event_ErrorOccured(object sender, string ErrorMessage)
		{
			try {
				MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.InformationMsg, mRemoteNG.My.Language.strConnectionEventErrorOccured, true);

				Connection.Protocol.Base Prot = sender;

				if (Prot.InterfaceControl.Info.Protocol == mRemoteNG.Connection.Protocol.Protocols.RDP) {
					if (ErrorMessage > -1) {
						MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.WarningMsg, string.Format(mRemoteNG.My.Language.strConnectionRdpErrorDetail, ErrorMessage, mRemoteNG.Connection.Protocol.RDP.FatalErrors.GetError(ErrorMessage)));
					}
				}
			} catch (Exception ex) {
				MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strConnectionEventConnectionFailed + Constants.vbNewLine + ex.Message, true);
			}
		}
		#endregion

		#region "External Apps"
		public static Tools.ExternalTool GetExtAppByName(string Name)
		{
			foreach (Tools.ExternalTool extA in ExternalTools) {
				if (extA.DisplayName == Name) {
					return extA;
				}
			}

			return null;
		}
		#endregion

		#region "Misc"
		public static void GoToURL(string URL)
		{
			mRemoteNG.Connection.Info cI = new mRemoteNG.Connection.Info();

			cI.Name = "";
			cI.Hostname = URL;
			if (URL.StartsWith("https:")) {
				cI.Protocol = mRemoteNG.Connection.Protocol.Protocols.HTTPS;
			} else {
				cI.Protocol = mRemoteNG.Connection.Protocol.Protocols.HTTP;
			}
			cI.SetDefaultPort();
			cI.IsQuickConnect = true;

			mRemoteNG.App.Runtime.OpenConnection(cI, mRemoteNG.Connection.Info.Force.DoNotJump);
		}

		public static void GoToWebsite()
		{
			GoToURL(mRemoteNG.App.Info.General.URLHome);
		}

		public static void GoToDonate()
		{
			GoToURL(mRemoteNG.App.Info.General.URLDonate);
		}

		public static void GoToForum()
		{
			GoToURL(mRemoteNG.App.Info.General.URLForum);
		}

		public static void GoToBugs()
		{
			GoToURL(mRemoteNG.App.Info.General.URLBugs);
		}

		public static void Report(string Text)
		{
			try {
				StreamWriter sWr = new StreamWriter(mRemoteNG.My.MyProject.Application.Info.DirectoryPath + "\\Report.log", true);
				sWr.WriteLine(Text);
				sWr.Close();
			} catch (Exception ex) {
				MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strLogWriteToFileFailed);
			}
		}

		public static bool SaveReport()
		{
			StreamReader streamReader = null;
			StreamWriter streamWriter = null;
			try {
				streamReader = new StreamReader(mRemoteNG.My.MyProject.Application.Info.DirectoryPath + "\\Report.log");
				string text = streamReader.ReadToEnd();
				streamReader.Close();

				streamWriter = new StreamWriter(mRemoteNG.App.Info.General.ReportingFilePath, true);
				streamWriter.Write(text);

				return true;
			} catch (Exception ex) {
				MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strLogWriteToFileFinalLocationFailed + Constants.vbNewLine + ex.Message, true);
				return false;
			} finally {
				if (streamReader != null) {
					streamReader.Close();
					streamReader.Dispose();
				}
				if (streamWriter != null) {
					streamWriter.Close();
					streamWriter.Dispose();
				}
			}
		}

		public static Connection.InterfaceControl FindConnectionContainer(Connection.Info ConI)
		{
			if (ConI.OpenConnections.Count > 0) {
				for (int i = 0; i <= WindowList.Count - 1; i++) {
					if (WindowList[i] is UI.Window.Connection) {
						UI.Window.Connection conW = WindowList[i];

						if (conW.TabController != null) {
							foreach (Crownwood.Magic.Controls.TabPage t in conW.TabController.TabPages) {
								if (t.Controls[0] != null) {
									if (t.Controls[0] is Connection.InterfaceControl) {
										Connection.InterfaceControl IC = t.Controls[0];
										if (object.ReferenceEquals(IC.Info, ConI)) {
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
		public static void FontOverride(ref Control ctlParent)
		{
			Control ctlChild = null;
			foreach (Control ctlChild_loopVariable in ctlParent.Controls) {
				ctlChild = ctlChild_loopVariable;
				ctlChild.Font = new System.Drawing.Font(SystemFonts.MessageBoxFont.Name, ctlChild.Font.Size, ctlChild.Font.Style, ctlChild.Font.Unit, ctlChild.Font.GdiCharSet);
				if (ctlChild.Controls.Count > 0) {
					FontOverride(ref ctlChild);
				}
			}
		}
		#endregion

		#region "SQL Watcher"
		private static void tmrSqlWatcher_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			mRemoteNG.Tools.Misc.IsSQLUpdateAvailableBG();
		}

		private static void SQLUpdateCheckFinished(bool UpdateAvailable)
		{
			if (UpdateAvailable == true) {
				MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.InformationMsg, mRemoteNG.My.Language.strSqlUpdateCheckUpdateAvailable, true);
				LoadConnectionsBG();
			}
		}
		#endregion

	}
}
