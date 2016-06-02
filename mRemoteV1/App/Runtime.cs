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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Forms.Input;
using mRemoteNG.UI.TaskDialog;
using WeifenLuo.WinFormsUI.Docking;
using TabPage = Crownwood.Magic.Controls.TabPage;


namespace mRemoteNG.App
{
    public class Runtime
    {
        #region Private Variables

        //private static System.Timers.Timer _timerSqlWatcher;

        #endregion

        #region Public Properties
        public static ConnectionList ConnectionList { get; set; }

        public static ConnectionList PreviousConnectionList { get; set; }

        public static ContainerList ContainerList { get; set; }

        public static ContainerList PreviousContainerList { get; set; }

        public static CredentialList CredentialList { get; set; }

        public static CredentialList PreviousCredentialList { get; set; }

        public static WindowList WindowList { get; set; }

        public static MessageCollector MessageCollector { get; set; }

        public static Tools.Controls.NotificationAreaIcon NotificationAreaIcon { get; set; }

        public static bool IsConnectionsFileLoaded { get; set; }

        public static SqlConnectionsProvider SQLConnProvider { get; set; }

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

        public static DateTime LastSqlUpdate { get; set; }

        public static string LastSelected { get; set; }

        public static ConnectionInfo DefaultConnection { get; set; }

        public static ConnectionInfoInheritance DefaultInheritance { get; set; }

        public static ArrayList ExternalTools { get; set; } = new ArrayList();

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
            Settings.Default.ConDefaultDescription = DefaultConnection.Description;
            Settings.Default.ConDefaultIcon = DefaultConnection.Icon;
            Settings.Default.ConDefaultUsername = DefaultConnection.Username;
            Settings.Default.ConDefaultPassword = DefaultConnection.Password;
            Settings.Default.ConDefaultDomain = DefaultConnection.Domain;
            Settings.Default.ConDefaultProtocol = DefaultConnection.Protocol.ToString();
            Settings.Default.ConDefaultPuttySession = DefaultConnection.PuttySession;
            Settings.Default.ConDefaultICAEncryptionStrength = DefaultConnection.ICAEncryption.ToString();
            Settings.Default.ConDefaultRDPAuthenticationLevel = DefaultConnection.RDPAuthenticationLevel.ToString();
            Settings.Default.ConDefaultLoadBalanceInfo = DefaultConnection.LoadBalanceInfo;
            Settings.Default.ConDefaultUseConsoleSession = DefaultConnection.UseConsoleSession;
            Settings.Default.ConDefaultUseCredSsp = DefaultConnection.UseCredSsp;
            Settings.Default.ConDefaultRenderingEngine = DefaultConnection.RenderingEngine.ToString();
            Settings.Default.ConDefaultResolution = DefaultConnection.Resolution.ToString();
            Settings.Default.ConDefaultAutomaticResize = DefaultConnection.AutomaticResize;
            Settings.Default.ConDefaultColors = DefaultConnection.Colors.ToString();
            Settings.Default.ConDefaultCacheBitmaps = DefaultConnection.CacheBitmaps;
            Settings.Default.ConDefaultDisplayWallpaper = DefaultConnection.DisplayWallpaper;
            Settings.Default.ConDefaultDisplayThemes = DefaultConnection.DisplayThemes;
            Settings.Default.ConDefaultEnableFontSmoothing = DefaultConnection.EnableFontSmoothing;
            Settings.Default.ConDefaultEnableDesktopComposition = DefaultConnection.EnableDesktopComposition;
            Settings.Default.ConDefaultRedirectKeys = DefaultConnection.RedirectKeys;
            Settings.Default.ConDefaultRedirectDiskDrives = DefaultConnection.RedirectDiskDrives;
            Settings.Default.ConDefaultRedirectPrinters = DefaultConnection.RedirectPrinters;
            Settings.Default.ConDefaultRedirectPorts = DefaultConnection.RedirectPorts;
            Settings.Default.ConDefaultRedirectSmartCards = DefaultConnection.RedirectSmartCards;
            Settings.Default.ConDefaultRedirectSound = DefaultConnection.RedirectSound.ToString();
            Settings.Default.ConDefaultPreExtApp = DefaultConnection.PreExtApp;
            Settings.Default.ConDefaultPostExtApp = DefaultConnection.PostExtApp;
            Settings.Default.ConDefaultMacAddress = DefaultConnection.MacAddress;
            Settings.Default.ConDefaultUserField = DefaultConnection.UserField;
            Settings.Default.ConDefaultVNCAuthMode = DefaultConnection.VNCAuthMode.ToString();
            Settings.Default.ConDefaultVNCColors = DefaultConnection.VNCColors.ToString();
            Settings.Default.ConDefaultVNCCompression = DefaultConnection.VNCCompression.ToString();
            Settings.Default.ConDefaultVNCEncoding = DefaultConnection.VNCEncoding.ToString();
            Settings.Default.ConDefaultVNCProxyIP = DefaultConnection.VNCProxyIP;
            Settings.Default.ConDefaultVNCProxyPassword = DefaultConnection.VNCProxyPassword;
            Settings.Default.ConDefaultVNCProxyPort = DefaultConnection.VNCProxyPort;
            Settings.Default.ConDefaultVNCProxyType = DefaultConnection.VNCProxyType.ToString();
            Settings.Default.ConDefaultVNCProxyUsername = DefaultConnection.VNCProxyUsername;
            Settings.Default.ConDefaultVNCSmartSizeMode = DefaultConnection.VNCSmartSizeMode.ToString();
            Settings.Default.ConDefaultVNCViewOnly = DefaultConnection.VNCViewOnly;
            Settings.Default.ConDefaultExtApp = DefaultConnection.ExtApp;
            Settings.Default.ConDefaultRDGatewayUsageMethod = DefaultConnection.RDGatewayUsageMethod.ToString();
            Settings.Default.ConDefaultRDGatewayHostname = DefaultConnection.RDGatewayHostname;
            Settings.Default.ConDefaultRDGatewayUsername = DefaultConnection.RDGatewayUsername;
            Settings.Default.ConDefaultRDGatewayPassword = DefaultConnection.RDGatewayPassword;
            Settings.Default.ConDefaultRDGatewayDomain = DefaultConnection.RDGatewayDomain;
            Settings.Default.ConDefaultRDGatewayUseConnectionCredentials = DefaultConnection.RDGatewayUseConnectionCredentials.ToString();
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
            Settings.Default.InhDefaultDescription = DefaultInheritance.Description;
            Settings.Default.InhDefaultIcon = DefaultInheritance.Icon;
            Settings.Default.InhDefaultPanel = DefaultInheritance.Panel;
            Settings.Default.InhDefaultUsername = DefaultInheritance.Username;
            Settings.Default.InhDefaultPassword = DefaultInheritance.Password;
            Settings.Default.InhDefaultDomain = DefaultInheritance.Domain;
            Settings.Default.InhDefaultProtocol = DefaultInheritance.Protocol;
            Settings.Default.InhDefaultPort = DefaultInheritance.Port;
            Settings.Default.InhDefaultPuttySession = DefaultInheritance.PuttySession;
            Settings.Default.InhDefaultUseConsoleSession = DefaultInheritance.UseConsoleSession;
            Settings.Default.InhDefaultUseCredSsp = DefaultInheritance.UseCredSsp;
            Settings.Default.InhDefaultRenderingEngine = DefaultInheritance.RenderingEngine;
            Settings.Default.InhDefaultICAEncryptionStrength = DefaultInheritance.ICAEncryption;
            Settings.Default.InhDefaultRDPAuthenticationLevel = DefaultInheritance.RDPAuthenticationLevel;
            Settings.Default.InhDefaultLoadBalanceInfo = DefaultInheritance.LoadBalanceInfo;
            Settings.Default.InhDefaultResolution = DefaultInheritance.Resolution;
            Settings.Default.InhDefaultAutomaticResize = DefaultInheritance.AutomaticResize;
            Settings.Default.InhDefaultColors = DefaultInheritance.Colors;
            Settings.Default.InhDefaultCacheBitmaps = DefaultInheritance.CacheBitmaps;
            Settings.Default.InhDefaultDisplayWallpaper = DefaultInheritance.DisplayWallpaper;
            Settings.Default.InhDefaultDisplayThemes = DefaultInheritance.DisplayThemes;
            Settings.Default.InhDefaultEnableFontSmoothing = DefaultInheritance.EnableFontSmoothing;
            Settings.Default.InhDefaultEnableDesktopComposition = DefaultInheritance.EnableDesktopComposition;
            Settings.Default.InhDefaultRedirectKeys = DefaultInheritance.RedirectKeys;
            Settings.Default.InhDefaultRedirectDiskDrives = DefaultInheritance.RedirectDiskDrives;
            Settings.Default.InhDefaultRedirectPrinters = DefaultInheritance.RedirectPrinters;
            Settings.Default.InhDefaultRedirectPorts = DefaultInheritance.RedirectPorts;
            Settings.Default.InhDefaultRedirectSmartCards = DefaultInheritance.RedirectSmartCards;
            Settings.Default.InhDefaultRedirectSound = DefaultInheritance.RedirectSound;
            Settings.Default.InhDefaultPreExtApp = DefaultInheritance.PreExtApp;
            Settings.Default.InhDefaultPostExtApp = DefaultInheritance.PostExtApp;
            Settings.Default.InhDefaultMacAddress = DefaultInheritance.MacAddress;
            Settings.Default.InhDefaultUserField = DefaultInheritance.UserField;
            // VNC inheritance
            Settings.Default.InhDefaultVNCAuthMode = DefaultInheritance.VNCAuthMode;
            Settings.Default.InhDefaultVNCColors = DefaultInheritance.VNCColors;
            Settings.Default.InhDefaultVNCCompression = DefaultInheritance.VNCCompression;
            Settings.Default.InhDefaultVNCEncoding = DefaultInheritance.VNCEncoding;
            Settings.Default.InhDefaultVNCProxyIP = DefaultInheritance.VNCProxyIP;
            Settings.Default.InhDefaultVNCProxyPassword = DefaultInheritance.VNCProxyPassword;
            Settings.Default.InhDefaultVNCProxyPort = DefaultInheritance.VNCProxyPort;
            Settings.Default.InhDefaultVNCProxyType = DefaultInheritance.VNCProxyType;
            Settings.Default.InhDefaultVNCProxyUsername = DefaultInheritance.VNCProxyUsername;
            Settings.Default.InhDefaultVNCSmartSizeMode = DefaultInheritance.VNCSmartSizeMode;
            Settings.Default.InhDefaultVNCViewOnly = DefaultInheritance.VNCViewOnly;
            // Ext. App inheritance
            Settings.Default.InhDefaultExtApp = DefaultInheritance.ExtApp;
            // RDP gateway inheritance
            Settings.Default.InhDefaultRDGatewayUsageMethod = DefaultInheritance.RDGatewayUsageMethod;
            Settings.Default.InhDefaultRDGatewayHostname = DefaultInheritance.RDGatewayHostname;
            Settings.Default.InhDefaultRDGatewayUsername = DefaultInheritance.RDGatewayUsername;
            Settings.Default.InhDefaultRDGatewayPassword = DefaultInheritance.RDGatewayPassword;
            Settings.Default.InhDefaultRDGatewayDomain = DefaultInheritance.RDGatewayDomain;
            Settings.Default.InhDefaultRDGatewayUseConnectionCredentials = DefaultInheritance.RDGatewayUseConnectionCredentials;
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
                title = Language.strNewPanel;
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
            cMenScreens.Text = Language.strSendTo;
            cMenScreens.Image = Resources.Monitor;
            cMenScreens.Tag = pnlcForm;
            cMenScreens.DropDownItems.Add("Dummy");
            cMenScreens.DropDownOpening += cMenConnectionPanelScreens_DropDownOpening;
            return cMenScreens;
        }

        private static ToolStripMenuItem CreateRenameMenuItem(DockContent pnlcForm)
        {
            ToolStripMenuItem cMenRen = new ToolStripMenuItem();
            cMenRen.Text = Language.strRename;
            cMenRen.Image = Resources.Rename;
            cMenRen.Tag = pnlcForm;
            cMenRen.Click += cMenConnectionPanelRename_Click;
            return cMenRen;
        }

        private static void cMenConnectionPanelRename_Click(Object sender, EventArgs e)
        {
            try
            {
                ConnectionWindow conW = default(ConnectionWindow);
                conW = (ConnectionWindow)((ToolStripMenuItem)sender).Tag;

                string nTitle = "";
                input.InputBox(Language.strNewTitle, Language.strNewTitle + ":", ref nTitle);

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
                    ToolStripMenuItem cMenScreen = new ToolStripMenuItem(Language.strScreen + " " + Convert.ToString(i + 1));
                    cMenScreen.Tag = new ArrayList();
                    cMenScreen.Image = Resources.Monitor_GoTo;
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
                    Settings.Default.LoadConsFromCustomLocation = false;
                }
                else
                {
                    Settings.Default.LoadConsFromCustomLocation = true;
                    Settings.Default.CustomConsPath = filename;
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
                        xmlTextWriter.WriteAttributeString("Name", Language.strConnections);
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
                MessageCollector.AddExceptionMessage(Language.strCouldNotCreateNewConnectionsFile, ex, MessageClass.ErrorMsg);
            }
        }

        public static void LoadConnectionsBG(bool WithDialog = false, bool Update = false)
        {
            _withDialog = false;
            _loadUpdate = true;

            Thread t = new Thread(LoadConnectionsBGd);
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

                if (!Settings.Default.UseSQLServer)
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

                if (update)
                {
                    connectionsLoader.PreviousSelected = LastSelected;
                }

                ConnectionTree.ResetTree();

                connectionsLoader.RootTreeNode = Windows.treeForm.tvConnections.Nodes[0];
                connectionsLoader.UseSQL = Settings.Default.UseSQLServer;
                connectionsLoader.SQLHost = Settings.Default.SQLHost;
                connectionsLoader.SQLDatabaseName = Settings.Default.SQLDatabaseName;
                connectionsLoader.SQLUsername = Settings.Default.SQLUser;
                connectionsLoader.SQLPassword = Security.Crypt.Decrypt(Convert.ToString(Settings.Default.SQLPass), GeneralAppInfo.EncryptionKey);
                connectionsLoader.SQLUpdate = update;
                connectionsLoader.LoadConnections(false);

                if (Settings.Default.UseSQLServer)
                {
                    LastSqlUpdate = DateTime.Now;
                }
                else
                {
                    if (connectionsLoader.ConnectionFileName == GetDefaultStartupConnectionFileName())
                    {
                        Settings.Default.LoadConsFromCustomLocation = false;
                    }
                    else
                    {
                        Settings.Default.LoadConsFromCustomLocation = true;
                        Settings.Default.CustomConsPath = connectionsLoader.ConnectionFileName;
                    }
                }

                // re-enable sql update checking after updates are loaded
                if (Settings.Default.UseSQLServer && SQLConnProvider != null)
                {
                    SQLConnProvider.Enable();
                }
            }
            catch (Exception ex)
            {
                if (Settings.Default.UseSQLServer)
                {
                    MessageCollector.AddExceptionMessage(Language.strLoadFromSqlFailed, ex);
                    string commandButtons = string.Join("|", new[] { Language.strCommandTryAgain, Language.strCommandOpenConnectionFile, string.Format(Language.strCommandExitProgram, Application.ProductName) });
                    CTaskDialog.ShowCommandBox(Application.ProductName, Language.strLoadFromSqlFailed, Language.strLoadFromSqlFailedContent, MiscTools.GetExceptionMessageRecursive(ex), "", "", commandButtons, false, ESysIcons.Error, ESysIcons.Error);
                    switch (CTaskDialog.CommandButtonResult)
                    {
                        case 0:
                            LoadConnections(withDialog, update);
                            return;
                        case 1:
                            Settings.Default.UseSQLServer = false;
                            LoadConnections(true, update);
                            return;
                        default:
                            Application.Exit();
                            return;
                    }
                }
                else
                {
                    if (ex is FileNotFoundException && !withDialog)
                    {
                        MessageCollector.AddExceptionMessage(string.Format(Language.strConnectionsFileCouldNotBeLoadedNew, connectionsLoader.ConnectionFileName), ex, MessageClass.InformationMsg);
                        NewConnections(Convert.ToString(connectionsLoader.ConnectionFileName));
                        return;
                    }

                    MessageCollector.AddExceptionMessage(string.Format(Language.strConnectionsFileCouldNotBeLoaded, connectionsLoader.ConnectionFileName), ex);
                    if (connectionsLoader.ConnectionFileName != GetStartupConnectionFileName())
                    {
                        LoadConnections(withDialog, update);
                        return;
                    }
                    else
                    {
                        MessageBox.Show(frmMain.Default,
                            string.Format(Language.strErrorStartupConnectionFileLoad, Environment.NewLine, Application.ProductName, GetStartupConnectionFileName(), MiscTools.GetExceptionMessageRecursive(ex)),
                            "Could not load startup file.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                        return;
                    }
                }
            }
        }

        protected static void CreateBackupFile(string fileName)
        {
            // This intentionally doesn't prune any existing backup files. We just assume the user doesn't want any new ones created.
            if (Settings.Default.BackupFileKeepCount == 0)
            {
                return;
            }

            try
            {
                string backupFileName = string.Format(Settings.Default.BackupFileNameFormat, fileName, DateTime.UtcNow);
                File.Copy(fileName, backupFileName);
                PruneBackupFiles(fileName);
            }
            catch (Exception ex)
            {
                MessageCollector.AddExceptionMessage(Language.strConnectionsFileBackupFailed, ex, MessageClass.WarningMsg);
                throw;
            }
        }

        protected static void PruneBackupFiles(string baseName)
        {
            string fileName = Path.GetFileName(baseName);
            string directoryName = Path.GetDirectoryName(baseName);

            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(directoryName))
            {
                return;
            }

            string searchPattern = string.Format(Settings.Default.BackupFileNameFormat, fileName, "*");
            string[] files = Directory.GetFiles(directoryName, searchPattern);

            if (files.Length <= Settings.Default.BackupFileKeepCount)
            {
                return;
            }

            Array.Sort(files);
            Array.Resize(ref files, files.Length - Settings.Default.BackupFileKeepCount);

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
            if (Settings.Default.LoadConsFromCustomLocation == false)
            {
                return GetDefaultStartupConnectionFileName();
            }
            else
            {
                return Settings.Default.CustomConsPath;
            }
        }

        public static void SaveConnectionsBG()
        {
            _saveUpdate = true;
            Thread t = new Thread(SaveConnectionsBGd);
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

            try
            {
                if (Update && Settings.Default.UseSQLServer == false)
                {
                    return;
                }

                if (SQLConnProvider != null)
                {
                    SQLConnProvider.Disable();
                }

                ConnectionsSaver conS = new ConnectionsSaver();

                if (!Settings.Default.UseSQLServer)
                {
                    conS.ConnectionFileName = GetStartupConnectionFileName();
                }

                conS.ConnectionList = ConnectionList;
                conS.ContainerList = ContainerList;
                conS.Export = false;
                conS.SaveSecurity = new Security.Save(false);
                conS.RootTreeNode = Windows.treeForm.tvConnections.Nodes[0];

                if (Settings.Default.UseSQLServer)
                {
                    conS.SaveFormat = ConnectionsSaver.Format.SQL;
                    conS.SQLHost = Convert.ToString(Settings.Default.SQLHost);
                    conS.SQLDatabaseName = Convert.ToString(Settings.Default.SQLDatabaseName);
                    conS.SQLUsername = Convert.ToString(Settings.Default.SQLUser);
                    conS.SQLPassword = Security.Crypt.Decrypt(Convert.ToString(Settings.Default.SQLPass), GeneralAppInfo.EncryptionKey);
                }

                conS.SaveConnections();

                if (Settings.Default.UseSQLServer)
                {
                    LastSqlUpdate = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConnectionsFileCouldNotBeSaved + Environment.NewLine + ex.Message);
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
                    fileTypes.AddRange(new[] { Language.strFiltermRemoteXML, "*.xml" });
                    fileTypes.AddRange(new[] { Language.strFilterAll, "*.*" });

                    saveFileDialog.Filter = string.Join("|", fileTypes.ToArray());

                    if (!(saveFileDialog.ShowDialog(frmMain.Default) == DialogResult.OK))
                    {
                        return;
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
                        Settings.Default.LoadConsFromCustomLocation = false;
                    }
                    else
                    {
                        Settings.Default.LoadConsFromCustomLocation = true;
                        Settings.Default.CustomConsPath = saveFileDialog.FileName;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageCollector.AddExceptionMessage(string.Format(Language.strConnectionsFileCouldNotSaveAs, connectionsSave.ConnectionFileName), ex);
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
                Uri uri = new Uri("dummyscheme" + Uri.SchemeDelimiter + connectionString);
                if (string.IsNullOrEmpty(uri.Host))
                {
                    return null;
                }

                ConnectionInfo newConnectionInfo = new ConnectionInfo();

                if (Settings.Default.IdentifyQuickConnectTabs)
                {
                    newConnectionInfo.Name = string.Format(Language.strQuick, uri.Host);
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
                MessageCollector.AddExceptionMessage(Language.strQuickConnectFailed, ex, MessageClass.ErrorMsg);
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
                MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConnectionOpenFailed + Environment.NewLine + ex.Message);
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

                if (ConnectionTreeNode.GetNodeType(ConnectionTree.SelectedNode) == TreeNodeType.Connection | ConnectionTreeNode.GetNodeType(ConnectionTree.SelectedNode) == TreeNodeType.PuttySession)
                {
                    OpenConnection((ConnectionInfo)Windows.treeForm.tvConnections.SelectedNode.Tag, Force);
                }
                else if (ConnectionTreeNode.GetNodeType(ConnectionTree.SelectedNode) == TreeNodeType.Container)
                {
                    foreach (TreeNode tNode in ConnectionTree.SelectedNode.Nodes)
                    {
                        if (ConnectionTreeNode.GetNodeType(tNode) == TreeNodeType.Connection | ConnectionTreeNode.GetNodeType(ConnectionTree.SelectedNode) == TreeNodeType.PuttySession)
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
                MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConnectionOpenFailed + Environment.NewLine + ex.Message);
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
                MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConnectionOpenFailed + Environment.NewLine + ex.Message);
            }
        }

        public static void OpenConnection(ConnectionInfo ConnectionInfo, Form ConnectionForm)
        {
            try
            {
                OpenConnectionFinal(ConnectionInfo, ConnectionInfo.Force.None, ConnectionForm);
            }
            catch (Exception ex)
            {
                MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConnectionOpenFailed + Environment.NewLine + ex.Message);
            }
        }

        public static void OpenConnection(ConnectionInfo ConnectionInfo, Form ConnectionForm, ConnectionInfo.Force Force)
        {
            try
            {
                OpenConnectionFinal(ConnectionInfo, Force, ConnectionForm);
            }
            catch (Exception ex)
            {
                MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConnectionOpenFailed + Environment.NewLine + ex.Message);
            }
        }

        public static void OpenConnection(ConnectionInfo ConnectionInfo, ConnectionInfo.Force Force)
        {
            try
            {
                OpenConnectionFinal(ConnectionInfo, Force, null);
            }
            catch (Exception ex)
            {
                MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConnectionOpenFailed + Environment.NewLine + ex.Message);
            }
        }

        private static void OpenConnectionFinal(ConnectionInfo ConnectionInfo, ConnectionInfo.Force Force, Form ConForm)
        {
            try
            {
                if (ConnectionInfo.Hostname == "" && ConnectionInfo.Protocol != ProtocolType.IntApp)
                {
                    MessageCollector.AddMessage(MessageClass.WarningMsg, Language.strConnectionOpenFailedNoHostname);
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
                MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConnectionOpenFailed + Environment.NewLine + ex.Message);
            }
        }

        private static void BuildConnectionInterfaceController(ConnectionInfo ConnectionInfo, ProtocolBase newProtocol, Control connectionContainer)
        {
            newProtocol.InterfaceControl = new InterfaceControl(connectionContainer, newProtocol, ConnectionInfo);
        }

        private static void SetConnectionFormEventHandlers(ProtocolBase newProtocol, Form connectionForm)
        {
            newProtocol.Closed += ((ConnectionWindow)connectionForm).Prot_Event_Closed;
        }

        private static Control SetConnectionContainer(ConnectionInfo ConnectionInfo, Form connectionForm)
        {
            Control connectionContainer = default(Control);
            connectionContainer = ((ConnectionWindow)connectionForm).AddConnectionTab(ConnectionInfo);

            if (ConnectionInfo.Protocol == ProtocolType.IntApp)
            {
                if (GetExtAppByName(ConnectionInfo.ExtApp).Icon != null)
                    ((TabPage) connectionContainer).Icon = GetExtAppByName(ConnectionInfo.ExtApp).Icon;
            }
            return connectionContainer;
        }

        private static void SetTreeNodeImages(ConnectionInfo ConnectionInfo)
        {
            if (ConnectionInfo.IsQuickConnect == false)
            {
                if (ConnectionInfo.Protocol != ProtocolType.IntApp)
                {
                    ConnectionTreeNode.SetNodeImage(ConnectionInfo.TreeNode, TreeImageType.ConnectionOpen);
                }
                else
                {
                    ExternalTool extApp = GetExtAppByName(ConnectionInfo.ExtApp);
                    if (extApp != null)
                    {
                        if (extApp.TryIntegrate && ConnectionInfo.TreeNode != null)
                        {
                            ConnectionTreeNode.SetNodeImage(ConnectionInfo.TreeNode, TreeImageType.ConnectionOpen);
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
                ((ConnectionWindow)connectionForm).Show(frmMain.Default.pnlDock);

            connectionForm.Focus();
            return connectionForm;
        }

        private static string SetConnectionPanel(ConnectionInfo ConnectionInfo, ConnectionInfo.Force Force)
        {
            string connectionPanel = "";
            if (ConnectionInfo.Panel == "" || (Force & ConnectionInfo.Force.OverridePanel) == ConnectionInfo.Force.OverridePanel | Settings.Default.AlwaysShowPanelSelectionDlg)
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
                ExternalTool extA = GetExtAppByName(ConnectionInfo.PreExtApp);
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
                ((ConnectionWindow)IC.FindForm()).Focus();
                ((ConnectionWindow)IC.FindForm()).Show(frmMain.Default.pnlDock);
                TabPage tabPage = (TabPage)IC.Parent;
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
                MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format(Language.strProtocolEventDisconnected, DisconnectedMessage), true);

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
                            MessageCollector.AddMessage(MessageClass.WarningMsg, Language.strRdpDisconnected + Environment.NewLine + ReasonDescription + Environment.NewLine + string.Format(Language.strErrorCode, ReasonCode));
                        }
                        else
                        {
                            MessageCollector.AddMessage(MessageClass.WarningMsg, Language.strRdpDisconnected + Environment.NewLine + string.Format(Language.strErrorCode, ReasonCode));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageCollector.AddMessage(MessageClass.ErrorMsg, string.Format(Language.strProtocolEventDisconnectFailed, ex.Message), true);
            }
        }

        public static void Prot_Event_Closed(object sender)
        {
            try
            {
                ProtocolBase Prot = (ProtocolBase)sender;
                MessageCollector.AddMessage(MessageClass.InformationMsg, Language.strConnenctionCloseEvent, true);
                MessageCollector.AddMessage(MessageClass.ReportMsg, string.Format(Language.strConnenctionClosedByUser, Prot.InterfaceControl.Info.Hostname, Prot.InterfaceControl.Info.Protocol.ToString(), Environment.UserName));
                Prot.InterfaceControl.Info.OpenConnections.Remove(Prot);

                if (Prot.InterfaceControl.Info.OpenConnections.Count < 1 && Prot.InterfaceControl.Info.IsQuickConnect == false)
                {
                    ConnectionTreeNode.SetNodeImage(Prot.InterfaceControl.Info.TreeNode, TreeImageType.ConnectionClosed);
                }

                if (Prot.InterfaceControl.Info.PostExtApp != "")
                {
                    ExternalTool extA = GetExtAppByName(Prot.InterfaceControl.Info.PostExtApp);
                    if (extA != null)
                    {
                        extA.Start(Prot.InterfaceControl.Info);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConnenctionCloseEventFailed + Environment.NewLine + ex.Message, true);
            }
        }

        public static void Prot_Event_Connected(object sender)
        {
            ProtocolBase prot = (ProtocolBase)sender;
            MessageCollector.AddMessage(MessageClass.InformationMsg, Language.strConnectionEventConnected, true);
            MessageCollector.AddMessage(MessageClass.ReportMsg, string.Format(Language.strConnectionEventConnectedDetail, prot.InterfaceControl.Info.Hostname, prot.InterfaceControl.Info.Protocol.ToString(), Environment.UserName, prot.InterfaceControl.Info.Description, prot.InterfaceControl.Info.UserField));
        }

        public static void Prot_Event_ErrorOccured(object sender, string ErrorMessage)
        {
            try
            {
                MessageCollector.AddMessage(MessageClass.InformationMsg, Language.strConnectionEventErrorOccured, true);
                ProtocolBase Prot = (ProtocolBase)sender;

                if (Prot.InterfaceControl.Info.Protocol == ProtocolType.RDP)
                {
                    if (Convert.ToInt32(ErrorMessage) > -1)
                    {
                        MessageCollector.AddMessage(MessageClass.WarningMsg, string.Format(Language.strConnectionRdpErrorDetail, ErrorMessage, ProtocolRDP.FatalErrors.GetError(ErrorMessage)));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConnectionEventConnectionFailed + Environment.NewLine + ex.Message, true);
            }
        }
        #endregion

        #region External Apps
        public static ExternalTool GetExtAppByName(string Name)
        {
            foreach (ExternalTool extA in ExternalTools)
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
            OpenConnection(connectionInfo, ConnectionInfo.Force.DoNotJump);
        }

        public static void GoToWebsite()
        {
            GoToURL(GeneralAppInfo.UrlHome);
        }

        public static void GoToDonate()
        {
            GoToURL(GeneralAppInfo.UrlDonate);
        }

        public static void GoToForum()
        {
            GoToURL(GeneralAppInfo.UrlForum);
        }

        public static void GoToBugs()
        {
            GoToURL(GeneralAppInfo.UrlBugs);
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
                MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strLogWriteToFileFailed);
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
                streamWriter = new StreamWriter(GeneralAppInfo.ReportingFilePath, true);
                streamWriter.Write(text);
                return true;
            }
            catch (Exception ex)
            {
                MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strLogWriteToFileFinalLocationFailed + Environment.NewLine + ex.Message, true);
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
                            foreach (TabPage t in connectionWindow.TabController.TabPages)
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
                ctlChild.Font = new Font(SystemFonts.MessageBoxFont.Name, ctlChild.Font.Size, ctlChild.Font.Style, ctlChild.Font.Unit, ctlChild.Font.GdiCharSet);
                if (ctlChild.Controls.Count > 0)
                {
                    FontOverride(ctlChild);
                }
            }
        }
        #endregion
    }
}