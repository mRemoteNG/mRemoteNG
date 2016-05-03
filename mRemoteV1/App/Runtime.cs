using log4net;
using Microsoft.VisualBasic;
using mRemoteNG.App.Info;
using mRemoteNG.Config.Connections;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Container;
using mRemoteNG.Credential;
using mRemoteNG.Images;
using mRemoteNG.Messages;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.UI.Window;
using PSTaskDialog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using mRemoteNG.UI.Forms;
using WeifenLuo.WinFormsUI.Docking;


namespace mRemoteNG.App
{
	public class Runtime
	{
		private Runtime()
		{
			// Fix Warning 292 CA1053 : Microsoft.Design : Because type 'Native' contains only 'static' ('Shared' in Visual Basic) members, add a default private constructor to prevent the compiler from adding a default public constructor.
		}

        #region Private Variables
        private static ConnectionList _connectionList;
        private static ConnectionList _previousConnectionList;
        private static ContainerList _containerList;
        private static ContainerList _previousContainerList;
        private static CredentialList _credentialList;
        private static CredentialList _previousCredentialList;
        private static WindowList _windowList;
        private static MessageCollector _messageCollector;
        private static Tools.Controls.NotificationAreaIcon _notificationAreaIcon;
        private static SystemMenu _systemMenu;
        private static ILog _log;
        private static bool _isConnectionsFileLoaded;
        private static System.Timers.Timer _timerSqlWatcher;
        private static SqlConnectionsProvider _sqlConnectionsProvider;
        private static DateTime _lastSqlUpdate;
        private static string _lastSelected;
        private static ConnectionInfo _defaultConnection;
        private static ConnectionInfoInheritance _defaultInheritance;
        private static ArrayList _externalTools = new ArrayList();
        #endregion

        #region Public Properties
        public static frmMain MainForm { get; set; }
		
        public static ConnectionList ConnectionList
		{
			get { return _connectionList; }
			set { _connectionList = value; }
		}
		
        public static ConnectionList PreviousConnectionList
		{
			get { return _previousConnectionList; }
			set { _previousConnectionList = value; }
		}
	    
        public static ContainerList ContainerList
		{
			get { return _containerList; }
			set { _containerList = value; }
		}
		
        public static ContainerList PreviousContainerList
		{
			get { return _previousContainerList; }
			set { _previousContainerList = value; }
		}
		
        public static CredentialList CredentialList
		{
			get { return _credentialList; }
			set { _credentialList = value; }
		}
		
        public static CredentialList PreviousCredentialList
		{
			get { return _previousCredentialList; }
			set { _previousCredentialList = value; }
		}
		
        public static WindowList WindowList
		{
			get { return _windowList; }
			set { _windowList = value; }
		}
		
        public static MessageCollector MessageCollector
		{
			get { return _messageCollector; }
			set { _messageCollector = value; }
		}
		
        public static Tools.Controls.NotificationAreaIcon NotificationAreaIcon
		{
			get { return _notificationAreaIcon; }
			set { _notificationAreaIcon = value; }
		}
		
        public static SystemMenu SystemMenu
		{
			get { return _systemMenu; }
			set { _systemMenu = value; }
		}
		
        public static ILog Log
		{
			get { return _log; }
			set { _log = value; }
		}
		
        public static bool IsConnectionsFileLoaded
		{
			get { return _isConnectionsFileLoaded; }
			set { _isConnectionsFileLoaded = value; }
		}
		

        public static SqlConnectionsProvider SQLConnProvider
        {
            get { return _sqlConnectionsProvider; }
            set { _sqlConnectionsProvider = value; }
        }
        /*
        public static System.Timers.Timer TimerSqlWatcher
		{
			get { return _timerSqlWatcher; }
			set
			{
				_timerSqlWatcher = value;
				_timerSqlWatcher.Elapsed += tmrSqlWatcher_Elapsed;
			}
		}
         */
		
        public static DateTime LastSqlUpdate
		{
			get { return _lastSqlUpdate; }
			set { _lastSqlUpdate = value; }
		}
		
        public static string LastSelected
		{
			get { return _lastSelected; }
			set { _lastSelected = value; }
		}
		
        public static ConnectionInfo DefaultConnection
		{
			get { return _defaultConnection; }
			set { _defaultConnection = value; }
		}
		
        public static ConnectionInfoInheritance DefaultInheritance
		{
			get { return _defaultInheritance; }
			set { _defaultInheritance = value; }
		}
		
        public static ArrayList ExternalTools
		{
			get { return _externalTools; }
			set { _externalTools = value; }
		}
        #endregion
		
        #region Default Connection
		public static ConnectionInfo DefaultConnectionFromSettings()
		{
			DefaultConnection = new ConnectionInfo();
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
		public static ConnectionInfoInheritance DefaultInheritanceFromSettings()
		{
			DefaultInheritance = new ConnectionInfoInheritance(null);
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
		public static Form AddPanel(string title = "", bool noTabber = false)
		{
			try
			{
                ConnectionWindow connectionForm = new ConnectionWindow(new DockContent());
                BuildConnectionWindowContextMenu(connectionForm);
                SetConnectionWindowTitle(title, connectionForm);
                ShowConnectionWindow(connectionForm);
                PrepareTabControllerSupport(noTabber, connectionForm);
				return connectionForm;
			}
			catch (Exception ex)
			{
                MessageCollector.AddMessage(MessageClass.ErrorMsg, "Couldn\'t add panel" + Environment.NewLine + ex.Message);
				return null;
			}
		}

        private static void ShowConnectionWindow(ConnectionWindow connectionForm)
        {
            connectionForm.Show(frmMain.Default.pnlDock, DockState.Document);
        }

        private static void PrepareTabControllerSupport(bool noTabber, ConnectionWindow connectionForm)
        {
            if (noTabber)
                connectionForm.TabController.Dispose();
            else
                WindowList.Add(connectionForm);
        }

        private static void SetConnectionWindowTitle(string title, ConnectionWindow connectionForm)
        {
            if (title == "")
                title = My.Language.strNewPanel;
            connectionForm.SetFormText(title.Replace("&", "&&"));
        }

        private static void BuildConnectionWindowContextMenu(DockContent pnlcForm)
        {
            ContextMenuStrip cMen = new ContextMenuStrip();
            ToolStripMenuItem cMenRen = CreateRenameMenuItem(pnlcForm);
            ToolStripMenuItem cMenScreens = CreateScreensMenuItem(pnlcForm);
            cMen.Items.AddRange(new ToolStripMenuItem[] { cMenRen, cMenScreens });
            pnlcForm.TabPageContextMenuStrip = cMen;
        }

        private static ToolStripMenuItem CreateScreensMenuItem(DockContent pnlcForm)
        {
            ToolStripMenuItem cMenScreens = new ToolStripMenuItem();
            cMenScreens.Text = My.Language.strSendTo;
            cMenScreens.Image = My.Resources.Monitor;
            cMenScreens.Tag = pnlcForm;
            cMenScreens.DropDownItems.Add("Dummy");
            cMenScreens.DropDownOpening += cMenConnectionPanelScreens_DropDownOpening;
            return cMenScreens;
        }

        private static ToolStripMenuItem CreateRenameMenuItem(DockContent pnlcForm)
        {
            ToolStripMenuItem cMenRen = new ToolStripMenuItem();
            cMenRen.Text = My.Language.strRename;
            cMenRen.Image = My.Resources.Rename;
            cMenRen.Tag = pnlcForm;
            cMenRen.Click += cMenConnectionPanelRename_Click;
            return cMenRen;
        }
			
		private static void cMenConnectionPanelRename_Click(Object sender, EventArgs e)
		{
			try
			{
				ConnectionWindow conW = default(ConnectionWindow);
                conW = (ConnectionWindow)((Control)sender).Tag;

                string nTitle = Interaction.InputBox(Prompt: My.Language.strNewTitle + ":", DefaultResponse: Convert.ToString(((Control)((Control)sender).Tag).Text.Replace("&&", "&")));
					
				if (!string.IsNullOrEmpty(nTitle))
				{
					conW.SetFormText(nTitle.Replace("&", "&&"));
				}
			}
			catch (Exception ex)
			{
                MessageCollector.AddMessage(MessageClass.ErrorMsg, "Couldn\'t rename panel" + Environment.NewLine + ex.Message);
			}
		}
			
		private static void cMenConnectionPanelScreens_DropDownOpening(Object sender, EventArgs e)
		{
			try
			{
                ToolStripMenuItem cMenScreens = (ToolStripMenuItem)sender;
				cMenScreens.DropDownItems.Clear();
					
				for (int i = 0; i <= Screen.AllScreens.Length - 1; i++)
				{
					ToolStripMenuItem cMenScreen = new ToolStripMenuItem(My.Language.strScreen + " " + Convert.ToString(i + 1));
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
                MessageCollector.AddMessage(MessageClass.ErrorMsg, "Couldn\'t enumerate screens" + Environment.NewLine + ex.Message);
			}
		}
			
		private static void cMenConnectionPanelScreen_Click(object sender, EventArgs e)
		{
            Screen screen = null;
            DockContent panel = null;
			try
			{
                IEnumerable tagEnumeration = (IEnumerable)((ToolStripMenuItem)sender).Tag;
                if (tagEnumeration != null)
                {
                    foreach (Object obj in tagEnumeration) 
                    {
                        if (obj is Screen)
                        {
                            screen = (Screen)obj;
                        }
                        else if (obj is DockContent)
                        {
                            panel = (DockContent)obj;
                        }
                    }
                    Screens.SendPanelToScreen(panel, screen);
                }
			}
			catch (Exception ex)
			{
                MessageCollector.AddMessage(MessageClass.ErrorMsg, "Caught Exception: " + Environment.NewLine + ex.Message);
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
				ConnectionList = new ConnectionList();
				ContainerList = new ContainerList();
				ConnectionsLoader connectionsLoader = new ConnectionsLoader();
					
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

				connectionsLoader.ConnectionList = ConnectionList;
				connectionsLoader.ContainerList = ContainerList;
                ConnectionTree.ResetTree();
				connectionsLoader.RootTreeNode = Windows.treeForm.tvConnections.Nodes[0];
					
				// Load config
				connectionsLoader.ConnectionFileName = filename;
				connectionsLoader.LoadConnections(false);
				Windows.treeForm.tvConnections.SelectedNode = connectionsLoader.RootTreeNode;
			}
			catch (Exception ex)
			{
                MessageCollector.AddExceptionMessage(My.Language.strCouldNotCreateNewConnectionsFile, ex, MessageClass.ErrorMsg);
			}
		}
		
		public static void LoadConnectionsBG(bool WithDialog = false, bool Update = false)
		{
			_withDialog = false;
			_loadUpdate = true;
				
			Thread t = new Thread(new ThreadStart(LoadConnectionsBGd));
			t.SetApartmentState(ApartmentState.STA);
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
            ConnectionsLoader connectionsLoader = new ConnectionsLoader();
			try
			{
                // disable sql update checking while we are loading updates
                if (SQLConnProvider != null)
                    SQLConnProvider.Disable();
					
				if (ConnectionList != null && ContainerList != null)
				{
					PreviousConnectionList = ConnectionList.Copy();
					PreviousContainerList = ContainerList.Copy();
				}
					
				ConnectionList = new ConnectionList();
				ContainerList = new ContainerList();
					
				if (!My.Settings.Default.UseSQLServer)
				{
					if (withDialog)
					{
						OpenFileDialog loadDialog = Tools.Controls.ConnectionsLoadDialog();
						if (loadDialog.ShowDialog() == DialogResult.OK)
						{
							connectionsLoader.ConnectionFileName = loadDialog.FileName;
						}
						else
						{
							return;
						}
					}
					else
					{
						connectionsLoader.ConnectionFileName = GetStartupConnectionFileName();
					}
					
					CreateBackupFile(Convert.ToString(connectionsLoader.ConnectionFileName));
				}
				
				connectionsLoader.ConnectionList = ConnectionList;
				connectionsLoader.ContainerList = ContainerList;
					
				if (PreviousConnectionList != null && PreviousContainerList != null)
				{
					connectionsLoader.PreviousConnectionList = PreviousConnectionList;
					connectionsLoader.PreviousContainerList = PreviousContainerList;
				}
					
				if (update == true)
				{
					connectionsLoader.PreviousSelected = LastSelected;
				}

                ConnectionTree.ResetTree();
					
				connectionsLoader.RootTreeNode = Windows.treeForm.tvConnections.Nodes[0];
				connectionsLoader.UseSQL = My.Settings.Default.UseSQLServer;
				connectionsLoader.SQLHost = My.Settings.Default.SQLHost;
				connectionsLoader.SQLDatabaseName = My.Settings.Default.SQLDatabaseName;
				connectionsLoader.SQLUsername = My.Settings.Default.SQLUser;
				connectionsLoader.SQLPassword = Security.Crypt.Decrypt(Convert.ToString(My.Settings.Default.SQLPass), GeneralAppInfo.EncryptionKey);
				connectionsLoader.SQLUpdate = update;
				connectionsLoader.LoadConnections(false);
					
				if (My.Settings.Default.UseSQLServer == true)
				{
					LastSqlUpdate = DateTime.Now;
				}
				else
				{
					if (connectionsLoader.ConnectionFileName == GetDefaultStartupConnectionFileName())
					{
						My.Settings.Default.LoadConsFromCustomLocation = false;
					}
					else
					{
						My.Settings.Default.LoadConsFromCustomLocation = true;
						My.Settings.Default.CustomConsPath = connectionsLoader.ConnectionFileName;
					}
				}

                // re-enable sql update checking after updates are loaded
                if (My.Settings.Default.UseSQLServer && SQLConnProvider != null)
				{
                    SQLConnProvider.Enable();
				}
			}
			catch (Exception ex)
			{
				if (My.Settings.Default.UseSQLServer)
				{
                    MessageCollector.AddExceptionMessage(My.Language.strLoadFromSqlFailed, ex);
					string commandButtons = string.Join("|", new[] {My.Language.strCommandTryAgain, My.Language.strCommandOpenConnectionFile, string.Format(My.Language.strCommandExitProgram, Application.ProductName)});
                    cTaskDialog.ShowCommandBox(Application.ProductName, My.Language.strLoadFromSqlFailed, My.Language.strLoadFromSqlFailedContent, MiscTools.GetExceptionMessageRecursive(ex), "", "", commandButtons, false, eSysIcons.Error, eSysIcons.Error);
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
                        MessageCollector.AddExceptionMessage(string.Format(My.Language.strConnectionsFileCouldNotBeLoadedNew, connectionsLoader.ConnectionFileName), ex, MessageClass.InformationMsg);
						NewConnections(Convert.ToString(connectionsLoader.ConnectionFileName));
						return ;
					}

                    MessageCollector.AddExceptionMessage(string.Format(My.Language.strConnectionsFileCouldNotBeLoaded, connectionsLoader.ConnectionFileName), ex);
					if (!(connectionsLoader.ConnectionFileName == GetStartupConnectionFileName()))
					{
						LoadConnections(withDialog, update);
						return ;
					}
					else
					{
						Interaction.MsgBox(string.Format(My.Language.strErrorStartupConnectionFileLoad, Environment.NewLine, Application.ProductName, GetStartupConnectionFileName(), MiscTools.GetExceptionMessageRecursive(ex)), (int) MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, null);
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
                MessageCollector.AddExceptionMessage(My.Language.strConnectionsFileBackupFailed, ex, MessageClass.WarningMsg);
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
                File.Delete(file);
			}
		}
		
		public static string GetDefaultStartupConnectionFileName()
		{
			string newPath = ConnectionsFileInfo.DefaultConnectionsPath + "\\" + ConnectionsFileInfo.DefaultConnectionsFile;
#if !PORTABLE
			string oldPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + Application.ProductName + "\\" + ConnectionsFileInfo.DefaultConnectionsFile;
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
			Thread t = new Thread(new ThreadStart(SaveConnectionsBGd));
			t.SetApartmentState(ApartmentState.STA);
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
					
				if (SQLConnProvider != null)
				{
                    SQLConnProvider.Disable();
				}
					
				ConnectionsSaver conS = new ConnectionsSaver();
					
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
					conS.SaveFormat = ConnectionsSaver.Format.SQL;
					conS.SQLHost = Convert.ToString(My.Settings.Default.SQLHost);
					conS.SQLDatabaseName = Convert.ToString(My.Settings.Default.SQLDatabaseName);
					conS.SQLUsername = Convert.ToString(My.Settings.Default.SQLUser);
					conS.SQLPassword = Security.Crypt.Decrypt(Convert.ToString(My.Settings.Default.SQLPass), GeneralAppInfo.EncryptionKey);
				}
					
				conS.SaveConnections();
					
				if (My.Settings.Default.UseSQLServer == true)
				{
					LastSqlUpdate = DateTime.Now;
				}
			}
			catch (Exception ex)
			{
                MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strConnectionsFileCouldNotBeSaved + Environment.NewLine + ex.Message);
			}
			finally
			{
                if (SQLConnProvider != null)
				{
                    SQLConnProvider.Enable();
				}
			}
		}
		
		public static void SaveConnectionsAs()
		{
			bool previousTimerState = false;
            ConnectionsSaver connectionsSave = new ConnectionsSaver();
				
			try
			{
                if (SQLConnProvider != null)
				{
                    SQLConnProvider.Disable();
				}
					
				using (SaveFileDialog saveFileDialog = new SaveFileDialog())
				{
					saveFileDialog.CheckPathExists = true;
					saveFileDialog.InitialDirectory = ConnectionsFileInfo.DefaultConnectionsPath;
					saveFileDialog.FileName = ConnectionsFileInfo.DefaultConnectionsFile;
					saveFileDialog.OverwritePrompt = true;

                    List<string> fileTypes = new List<string>();
					fileTypes.AddRange(new[] {My.Language.strFiltermRemoteXML, "*.xml"});
					fileTypes.AddRange(new[] {My.Language.strFilterAll, "*.*"});
						
					saveFileDialog.Filter = string.Join("|", fileTypes.ToArray());
						
					if (!(saveFileDialog.ShowDialog(frmMain.Default) == DialogResult.OK))
					{
						return ;
					}

                    connectionsSave.SaveFormat = ConnectionsSaver.Format.mRXML;
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
                MessageCollector.AddExceptionMessage(string.Format(My.Language.strConnectionsFileCouldNotSaveAs, connectionsSave.ConnectionFileName), ex);
			}
			finally
			{
                if (SQLConnProvider != null)
				{
                    SQLConnProvider.Enable();
				}
			}
		}
        #endregion
		
        #region Opening Connection
		public static ConnectionInfo CreateQuickConnect(string connectionString, ProtocolType protocol)
		{
			try
			{
				Uri uri = new Uri("dummyscheme" + System.Uri.SchemeDelimiter + connectionString);
				if (string.IsNullOrEmpty(uri.Host))
				{
					return null;
				}
					
				ConnectionInfo newConnectionInfo = new ConnectionInfo();
					
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
                MessageCollector.AddExceptionMessage(My.Language.strQuickConnectFailed, ex, MessageClass.ErrorMsg);
				return null;
			}
		}
			
		public static void OpenConnection()
		{
			try
			{
				OpenConnection(ConnectionInfo.Force.None);
			}
			catch (Exception ex)
			{
                MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strConnectionOpenFailed + Environment.NewLine + ex.Message);
			}
		}
			
		public static void OpenConnection(ConnectionInfo.Force Force)
		{
			try
			{
				if (Windows.treeForm.tvConnections.SelectedNode.Tag == null)
				{
					return;
				}

                if (Tree.Node.GetNodeType(ConnectionTree.SelectedNode) == Tree.TreeNodeType.Connection | Tree.Node.GetNodeType(ConnectionTree.SelectedNode) == Tree.TreeNodeType.PuttySession)
				{
					OpenConnection((ConnectionInfo)Windows.treeForm.tvConnections.SelectedNode.Tag, Force);
				}
                else if (Tree.Node.GetNodeType(ConnectionTree.SelectedNode) == Tree.TreeNodeType.Container)
				{
                    foreach (TreeNode tNode in ConnectionTree.SelectedNode.Nodes)
					{
                        if (Tree.Node.GetNodeType(tNode) == Tree.TreeNodeType.Connection | Tree.Node.GetNodeType(ConnectionTree.SelectedNode) == Tree.TreeNodeType.PuttySession)
						{
							if (tNode.Tag != null)
							{
								OpenConnection((ConnectionInfo)tNode.Tag, Force);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
                MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strConnectionOpenFailed + Environment.NewLine + ex.Message);
			}
		}
			
		public static void OpenConnection(ConnectionInfo ConnectionInfo)
		{
			try
			{
				OpenConnection(ConnectionInfo, ConnectionInfo.Force.None);
			}
			catch (Exception ex)
			{
                MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strConnectionOpenFailed + Environment.NewLine + ex.Message);
			}
		}
			
		public static void OpenConnection(ConnectionInfo ConnectionInfo, System.Windows.Forms.Form ConnectionForm)
		{
			try
			{
				OpenConnectionFinal(ConnectionInfo, ConnectionInfo.Force.None, ConnectionForm);
			}
			catch (Exception ex)
			{
                MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strConnectionOpenFailed + Environment.NewLine + ex.Message);
			}
		}
			
		public static void OpenConnection(ConnectionInfo ConnectionInfo, System.Windows.Forms.Form ConnectionForm, ConnectionInfo.Force Force)
		{
			try
			{
				OpenConnectionFinal(ConnectionInfo, Force, ConnectionForm);
			}
			catch (Exception ex)
			{
                MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strConnectionOpenFailed + Environment.NewLine + ex.Message);
			}
		}
			
		public static void OpenConnection(ConnectionInfo ConnectionInfo, mRemoteNG.Connection.ConnectionInfo.Force Force)
		{
			try
			{
				OpenConnectionFinal(ConnectionInfo, Force, null);
			}
			catch (Exception ex)
			{
                MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strConnectionOpenFailed + Environment.NewLine + ex.Message);
			}
		}

		private static void OpenConnectionFinal(ConnectionInfo ConnectionInfo, ConnectionInfo.Force Force, System.Windows.Forms.Form ConForm)
		{
			try
			{
				if (ConnectionInfo.Hostname == "" && ConnectionInfo.Protocol != Connection.Protocol.ProtocolType.IntApp)
				{
                    MessageCollector.AddMessage(MessageClass.WarningMsg, My.Language.strConnectionOpenFailedNoHostname);
					return;
				}

                StartPreConnectionExternalApp(ConnectionInfo);
					
				if ((Force & ConnectionInfo.Force.DoNotJump) != ConnectionInfo.Force.DoNotJump)
				{
					if (SwitchToOpenConnection(ConnectionInfo))
					{
						return;
					}
				}
				
                ProtocolFactory protocolFactory = new ProtocolFactory();
                ProtocolBase newProtocol = protocolFactory.CreateProtocol(ConnectionInfo);
				
                string connectionPanel = SetConnectionPanel(ConnectionInfo, Force);
                Form connectionForm = SetConnectionForm(ConForm, connectionPanel);
                Control connectionContainer = SetConnectionContainer(ConnectionInfo, connectionForm);
                SetConnectionFormEventHandlers(newProtocol, connectionForm);
                SetConnectionEventHandlers(newProtocol);
                BuildConnectionInterfaceController(ConnectionInfo, newProtocol, connectionContainer);
				
				newProtocol.Force = Force;
					
				if (newProtocol.Initialize() == false)
				{
					newProtocol.Close();
					return;
				}
					
				if (newProtocol.Connect() == false)
				{
					newProtocol.Close();
					return;
				}
					
				ConnectionInfo.OpenConnections.Add(newProtocol);
                SetTreeNodeImages(ConnectionInfo);
				frmMain.Default.SelectedConnection = ConnectionInfo;
			}
			catch (Exception ex)
			{
                MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strConnectionOpenFailed + Environment.NewLine + ex.Message);
			}
		}

        private static void BuildConnectionInterfaceController(ConnectionInfo ConnectionInfo, ProtocolBase newProtocol, Control connectionContainer)
        {
            newProtocol.InterfaceControl = new Connection.InterfaceControl(connectionContainer, newProtocol, ConnectionInfo);
        }

        private static void SetConnectionFormEventHandlers(ProtocolBase newProtocol, Form connectionForm)
        {
            newProtocol.Closed += ((UI.Window.ConnectionWindow)connectionForm).Prot_Event_Closed;
        }

        private static Control SetConnectionContainer(ConnectionInfo ConnectionInfo, System.Windows.Forms.Form connectionForm)
        {
            Control connectionContainer = default(Control);
            connectionContainer = ((ConnectionWindow)connectionForm).AddConnectionTab(ConnectionInfo);

            if (ConnectionInfo.Protocol == ProtocolType.IntApp)
            {
                if (Runtime.GetExtAppByName(ConnectionInfo.ExtApp).Icon != null)
                    (connectionContainer as Crownwood.Magic.Controls.TabPage).Icon = Runtime.GetExtAppByName(ConnectionInfo.ExtApp).Icon;
            }
            return connectionContainer;
        }

        private static void SetTreeNodeImages(ConnectionInfo ConnectionInfo)
        {
            if (ConnectionInfo.IsQuickConnect == false)
            {
                if (ConnectionInfo.Protocol != Connection.Protocol.ProtocolType.IntApp)
                {
                    Tree.Node.SetNodeImage(ConnectionInfo.TreeNode, TreeImageType.ConnectionOpen);
                }
                else
                {
                    ExternalTool extApp = GetExtAppByName(ConnectionInfo.ExtApp);
                    if (extApp != null)
                    {
                        if (extApp.TryIntegrate && ConnectionInfo.TreeNode != null)
                        {
                            Tree.Node.SetNodeImage(ConnectionInfo.TreeNode, TreeImageType.ConnectionOpen);
                        }
                    }
                }
            }
        }

        private static void SetConnectionEventHandlers(ProtocolBase newProtocol)
        {
            newProtocol.Disconnected += Prot_Event_Disconnected;
            newProtocol.Connected += Prot_Event_Connected;
            newProtocol.Closed += Prot_Event_Closed;
            newProtocol.ErrorOccured += Prot_Event_ErrorOccured;
        }

        private static Form SetConnectionForm(Form ConForm, string connectionPanel)
        {
            Form connectionForm = default(Form);
            if (ConForm == null)
                connectionForm = WindowList.FromString(connectionPanel);
            else
                connectionForm = ConForm;

            if (connectionForm == null)
                connectionForm = AddPanel(connectionPanel);
            else
                ((ConnectionWindow) connectionForm).Show(frmMain.Default.pnlDock);

            connectionForm.Focus();
            return connectionForm;
        }

        private static string SetConnectionPanel(ConnectionInfo ConnectionInfo, ConnectionInfo.Force Force)
        {
            string connectionPanel = "";
            if (ConnectionInfo.Panel == "" || (Force & ConnectionInfo.Force.OverridePanel) == ConnectionInfo.Force.OverridePanel | My.Settings.Default.AlwaysShowPanelSelectionDlg)
            {
                frmChoosePanel frmPnl = new frmChoosePanel();
                if (frmPnl.ShowDialog() == DialogResult.OK)
                {
                    connectionPanel = frmPnl.Panel;
                }
            }
            else
            {
                connectionPanel = ConnectionInfo.Panel;
            }
            return connectionPanel;
        }

        private static void StartPreConnectionExternalApp(ConnectionInfo ConnectionInfo)
        {
            if (ConnectionInfo.PreExtApp != "")
            {
                Tools.ExternalTool extA = Runtime.GetExtAppByName(ConnectionInfo.PreExtApp);
                if (extA != null)
                {
                    extA.Start(ConnectionInfo);
                }
            }
        }
			
		public static bool SwitchToOpenConnection(ConnectionInfo nCi)
		{
			InterfaceControl IC = FindConnectionContainer(nCi);
			if (IC != null)
			{
                ((ConnectionWindow) IC.FindForm()).Focus();
                ((ConnectionWindow) IC.FindForm()).Show(frmMain.Default.pnlDock);
				Crownwood.Magic.Controls.TabPage tabPage = (Crownwood.Magic.Controls.TabPage) IC.Parent;
				tabPage.Selected = true;
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
                MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format(My.Language.strProtocolEventDisconnected, DisconnectedMessage), true);

                ProtocolBase Prot = (ProtocolBase)sender;
				if (Prot.InterfaceControl.Info.Protocol == ProtocolType.RDP)
				{
					string[] Reason = DisconnectedMessage.Split("\r\n".ToCharArray());
					string ReasonCode = Reason[0];
					string ReasonDescription = Reason[1];
					if (Convert.ToInt32(ReasonCode) > 3)
					{
						if (!string.IsNullOrEmpty(ReasonDescription))
						{
                            MessageCollector.AddMessage(MessageClass.WarningMsg, My.Language.strRdpDisconnected + Environment.NewLine + ReasonDescription + Environment.NewLine + string.Format(My.Language.strErrorCode, ReasonCode));
						}
						else
						{
                            MessageCollector.AddMessage(MessageClass.WarningMsg, My.Language.strRdpDisconnected + Environment.NewLine + string.Format(My.Language.strErrorCode, ReasonCode));
						}
					}
				}
			}
			catch (Exception ex)
			{
                MessageCollector.AddMessage(MessageClass.ErrorMsg, string.Format(My.Language.strProtocolEventDisconnectFailed, ex.Message), true);
			}
		}
			
		public static void Prot_Event_Closed(object sender)
		{
			try
			{
                ProtocolBase Prot = (ProtocolBase)sender;

                MessageCollector.AddMessage(MessageClass.InformationMsg, My.Language.strConnenctionCloseEvent, true);

                MessageCollector.AddMessage(MessageClass.ReportMsg, string.Format(My.Language.strConnenctionClosedByUser, Prot.InterfaceControl.Info.Hostname, Prot.InterfaceControl.Info.Protocol.ToString(), Environment.UserName));
					
				Prot.InterfaceControl.Info.OpenConnections.Remove(Prot);
					
				if (Prot.InterfaceControl.Info.OpenConnections.Count < 1 && Prot.InterfaceControl.Info.IsQuickConnect == false)
				{
					Tree.Node.SetNodeImage(Prot.InterfaceControl.Info.TreeNode, TreeImageType.ConnectionClosed);
				}
				
				if (Prot.InterfaceControl.Info.PostExtApp != "")
				{
					Tools.ExternalTool extA = Runtime.GetExtAppByName(Prot.InterfaceControl.Info.PostExtApp);
					if (extA != null)
					{
						extA.Start(Prot.InterfaceControl.Info);
					}
				}
			}
			catch (Exception ex)
			{
                MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strConnenctionCloseEventFailed + Environment.NewLine + ex.Message, true);
			}
		}
			
		public static void Prot_Event_Connected(object sender)
		{
            ProtocolBase prot = (ProtocolBase)sender;
            MessageCollector.AddMessage(MessageClass.InformationMsg, My.Language.strConnectionEventConnected, true);
            MessageCollector.AddMessage(MessageClass.ReportMsg, string.Format(My.Language.strConnectionEventConnectedDetail, prot.InterfaceControl.Info.Hostname, prot.InterfaceControl.Info.Protocol.ToString(), Environment.UserName, prot.InterfaceControl.Info.Description, prot.InterfaceControl.Info.UserField));
		}
			
		public static void Prot_Event_ErrorOccured(object sender, string ErrorMessage)
		{
			try
			{
                MessageCollector.AddMessage(MessageClass.InformationMsg, My.Language.strConnectionEventErrorOccured, true);
                ProtocolBase Prot = (ProtocolBase)sender;
					
				if (Prot.InterfaceControl.Info.Protocol == ProtocolType.RDP)
				{
					if (Convert.ToInt32(ErrorMessage) > -1)
					{
                        MessageCollector.AddMessage(MessageClass.WarningMsg, string.Format(My.Language.strConnectionRdpErrorDetail, ErrorMessage, ProtocolRDP.FatalErrors.GetError(ErrorMessage)));
					}
				}
			}
			catch (Exception ex)
			{
                MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strConnectionEventConnectionFailed + Environment.NewLine + ex.Message, true);
			}
		}
        #endregion
		
        #region External Apps
		public static Tools.ExternalTool GetExtAppByName(string Name)
		{
			foreach (Tools.ExternalTool extA in ExternalTools)
			{
				if (extA.DisplayName == Name)
					return extA;
			}
			return null;
		}
        #endregion
		
        #region Misc
		public static void GoToURL(string URL)
		{
			ConnectionInfo connectionInfo = new ConnectionInfo();
				
			connectionInfo.Name = "";
			connectionInfo.Hostname = URL;
			if (URL.StartsWith("https:"))
			{
				connectionInfo.Protocol = ProtocolType.HTTPS;
			}
			else
			{
				connectionInfo.Protocol = ProtocolType.HTTP;
			}
			connectionInfo.SetDefaultPort();
			connectionInfo.IsQuickConnect = true;
			Runtime.OpenConnection(connectionInfo, ConnectionInfo.Force.DoNotJump);
		}
			
		public static void GoToWebsite()
		{
			GoToURL(App.Info.GeneralAppInfo.UrlHome);
		}
			
		public static void GoToDonate()
		{
			GoToURL(App.Info.GeneralAppInfo.UrlDonate);
		}
			
		public static void GoToForum()
		{
			GoToURL(App.Info.GeneralAppInfo.UrlForum);
		}
			
		public static void GoToBugs()
		{
			GoToURL(App.Info.GeneralAppInfo.UrlBugs);
		}
			
		public static void Report(string Text)
		{
			try
			{
				StreamWriter sWr = new StreamWriter(SettingsFileInfo.exePath + "\\Report.log", true);
				sWr.WriteLine(Text);
				sWr.Close();
			}
			catch (Exception)
			{
                MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strLogWriteToFileFailed);
			}
		}
			
		public static bool SaveReport()
		{
			StreamReader streamReader = null;
			StreamWriter streamWriter = null;
			try
			{
				streamReader = new StreamReader(SettingsFileInfo.exePath + "\\Report.log");
				string text = streamReader.ReadToEnd();
				streamReader.Close();
				streamWriter = new StreamWriter(App.Info.GeneralAppInfo.ReportingFilePath, true);
				streamWriter.Write(text);
				return true;
			}
			catch (Exception ex)
			{
                MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strLogWriteToFileFinalLocationFailed + Environment.NewLine + ex.Message, true);
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
			
		public static InterfaceControl FindConnectionContainer(ConnectionInfo connectionInfo)
		{
			if (connectionInfo.OpenConnections.Count > 0)
			{
				for (int i = 0; i <= WindowList.Count - 1; i++)
				{
					if (WindowList[i] is ConnectionWindow)
					{
                        ConnectionWindow connectionWindow = (ConnectionWindow)WindowList[i];
						if (connectionWindow.TabController != null)
						{
							foreach (Crownwood.Magic.Controls.TabPage t in connectionWindow.TabController.TabPages)
							{
								if (t.Controls[0] != null && t.Controls[0] is InterfaceControl)
								{
                                    InterfaceControl IC = (InterfaceControl)t.Controls[0];
									if (IC.Info == connectionInfo)
									{
										return IC;
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
	}
}