using mRemoteNG.App.Info;
using mRemoteNG.Config.Connections;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Credential;
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
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Forms.Input;
using mRemoteNG.UI.TaskDialog;
using WeifenLuo.WinFormsUI.Docking;
using static System.IO.Path;
using TabPage = Crownwood.Magic.Controls.TabPage;


namespace mRemoteNG.App
{
    public static class Runtime
    {
        #region Public Properties
        public static ConnectionList ConnectionList { get; set; }
        private static ConnectionList PreviousConnectionList { get; set; }
        public static ContainerList ContainerList { get; set; }
        private static ContainerList PreviousContainerList { get; set; }
        public static CredentialList CredentialList { get; set; }
        public static CredentialList PreviousCredentialList { get; set; }
        public static WindowList WindowList { get; set; }
        public static MessageCollector MessageCollector { get; set; }
        public static Tools.Controls.NotificationAreaIcon NotificationAreaIcon { get; set; }
        public static bool IsConnectionsFileLoaded { get; set; }
        public static SqlConnectionsProvider SQLConnProvider { get; set; }
        public static DateTime LastSqlUpdate { get; set; }
        public static string LastSelected { get; set; }
        public static ConnectionInfo DefaultConnection { get; set; } = DefaultConnectionInfo.Instance;
        public static ConnectionInfoInheritance DefaultInheritance { get; set; }
        public static ArrayList ExternalTools { get; set; } = new ArrayList();
        public static ConnectionTreeModel ConnectionTreeModel { get; set; }
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
            cMen.Items.AddRange(new ToolStripItem[] { cMenRen, cMenScreens });
            pnlcForm.TabPageContextMenuStrip = cMen;
        }

        private static ToolStripMenuItem CreateScreensMenuItem(DockContent pnlcForm)
        {
            ToolStripMenuItem cMenScreens = new ToolStripMenuItem
            {
                Text = Language.strSendTo,
                Image = Resources.Monitor,
                Tag = pnlcForm
            };
            cMenScreens.DropDownItems.Add("Dummy");
            cMenScreens.DropDownOpening += cMenConnectionPanelScreens_DropDownOpening;
            return cMenScreens;
        }

        private static ToolStripMenuItem CreateRenameMenuItem(DockContent pnlcForm)
        {
            ToolStripMenuItem cMenRen = new ToolStripMenuItem
            {
                Text = Language.strRename,
                Image = Resources.Rename,
                Tag = pnlcForm
            };
            cMenRen.Click += cMenConnectionPanelRename_Click;
            return cMenRen;
        }

        private static void cMenConnectionPanelRename_Click(Object sender, EventArgs e)
        {
            try
            {
                var conW = (ConnectionWindow)((ToolStripMenuItem)sender).Tag;

                string nTitle = "";
                input.InputBox(Language.strNewTitle, Language.strNewTitle + ":", ref nTitle);

                if (!string.IsNullOrEmpty(nTitle))
                {
                    conW.SetFormText(nTitle.Replace("&", "&&"));
                }
            }
            catch (Exception ex)
            {
                MessageCollector.AddExceptionStackTrace("cMenConnectionPanelRename_Click: Caught Exception: ", ex);
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
                MessageCollector.AddExceptionStackTrace("cMenConnectionPanelScreens_DropDownOpening: Caught Exception: ", ex);
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
                MessageCollector.AddExceptionStackTrace("cMenConnectionPanelScreen_Click: Caught Exception: ", ex);
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

                var dirname = GetDirectoryName(filename);
                if(dirname != null)
                    Directory.CreateDirectory(dirname);

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

                // Load config
                connectionsLoader.ConnectionFileName = filename;
                connectionsLoader.LoadConnections(false);
            }
            catch (Exception ex)
            {
                MessageCollector.AddExceptionMessage(Language.strCouldNotCreateNewConnectionsFile, ex);
            }
        }

        public static void LoadConnectionsBG()
        {
            _withDialog = false;
            _loadUpdate = true;

            var t = new Thread(LoadConnectionsBGd);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private static bool _withDialog;
        private static bool _loadUpdate;
        private static void LoadConnectionsBGd()
        {
            LoadConnections(_withDialog, _loadUpdate);
        }

        public static void LoadConnections(bool withDialog = false, bool update = false)
        {
            var connectionsLoader = new ConnectionsLoader();
            try
            {
                // disable sql update checking while we are loading updates
                SQLConnProvider?.Disable();

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
                        var loadDialog = Tools.Controls.ConnectionsLoadDialog();
                        if (loadDialog.ShowDialog() != DialogResult.OK) return;
                        connectionsLoader.ConnectionFileName = loadDialog.FileName;
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

                //ConnectionTree.ResetTree();

                //connectionsLoader.RootTreeNode = Windows.treeForm.tvConnections.Nodes[0];
                connectionsLoader.UseDatabase = Settings.Default.UseSQLServer;
                connectionsLoader.DatabaseHost = Settings.Default.SQLHost;
                connectionsLoader.DatabaseName = Settings.Default.SQLDatabaseName;
                connectionsLoader.DatabaseUsername = Settings.Default.SQLUser;
                var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
                connectionsLoader.DatabasePassword = cryptographyProvider.Decrypt(Convert.ToString(Settings.Default.SQLPass), GeneralAppInfo.EncryptionKey);
                connectionsLoader.DatabaseUpdate = update;
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
                SQLConnProvider?.Enable();
            }
            catch (Exception ex)
            {
                if (Settings.Default.UseSQLServer)
                {
                    MessageCollector.AddExceptionMessage(Language.strLoadFromSqlFailed, ex);
                    var commandButtons = string.Join("|", Language.strCommandTryAgain, Language.strCommandOpenConnectionFile, string.Format(Language.strCommandExitProgram, Application.ProductName));
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
                }
                else
                {
                    MessageBox.Show(frmMain.Default,
                        string.Format(Language.strErrorStartupConnectionFileLoad, Environment.NewLine, Application.ProductName, GetStartupConnectionFileName(), MiscTools.GetExceptionMessageRecursive(ex)),
                        "Could not load startup file.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }
        }

        private static void CreateBackupFile(string fileName)
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

        private static void PruneBackupFiles(string baseName)
        {
            string fileName = GetFileName(baseName);
            string directoryName = GetDirectoryName(baseName);

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

        private static string GetDefaultStartupConnectionFileName()
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

        private static bool _saveUpdate;
        private static object _saveLock = new object();
        private static void SaveConnectionsBGd()
        {
            Monitor.Enter(_saveLock);
            SaveConnections(_saveUpdate);
            Monitor.Exit(_saveLock);
        }

        public static void SaveConnections(bool Update = false)
        {
            //if (!IsConnectionsFileLoaded)
            //    return;

            try
            {
                if (Update && Settings.Default.UseSQLServer == false)
                    return;

                SQLConnProvider?.Disable();

                var connectionsSaver = new ConnectionsSaver();

                if (!Settings.Default.UseSQLServer)
                    connectionsSaver.ConnectionFileName = GetStartupConnectionFileName();

                connectionsSaver.ConnectionList = ConnectionList;
                connectionsSaver.ContainerList = ContainerList;
                connectionsSaver.Export = false;
                connectionsSaver.SaveSecurity = new Security.Save(false);
                connectionsSaver.ConnectionTreeModel = ConnectionTreeModel;

                if (Settings.Default.UseSQLServer)
                {
                    connectionsSaver.SaveFormat = ConnectionsSaver.Format.SQL;
                    connectionsSaver.SQLHost = Convert.ToString(Settings.Default.SQLHost);
                    connectionsSaver.SQLDatabaseName = Convert.ToString(Settings.Default.SQLDatabaseName);
                    connectionsSaver.SQLUsername = Convert.ToString(Settings.Default.SQLUser);
                    var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
                    connectionsSaver.SQLPassword = cryptographyProvider.Decrypt(Convert.ToString(Settings.Default.SQLPass), GeneralAppInfo.EncryptionKey);
                }

                connectionsSaver.SaveConnections();

                if (Settings.Default.UseSQLServer)
                    LastSqlUpdate = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConnectionsFileCouldNotBeSaved + Environment.NewLine + ex.Message);
            }
            finally
            {
                SQLConnProvider?.Enable();
            }
        }

        public static void SaveConnectionsAs()
        {
            var connectionsSave = new ConnectionsSaver();

            try
            {
                SQLConnProvider?.Disable();

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.CheckPathExists = true;
                    saveFileDialog.InitialDirectory = ConnectionsFileInfo.DefaultConnectionsPath;
                    saveFileDialog.FileName = ConnectionsFileInfo.DefaultConnectionsFile;
                    saveFileDialog.OverwritePrompt = true;

                    var fileTypes = new List<string>();
                    fileTypes.AddRange(new[] { Language.strFiltermRemoteXML, "*.xml" });
                    fileTypes.AddRange(new[] { Language.strFilterAll, "*.*" });

                    saveFileDialog.Filter = string.Join("|", fileTypes.ToArray());

                    if (saveFileDialog.ShowDialog(frmMain.Default) != DialogResult.OK)
                        return;

                    connectionsSave.SaveFormat = ConnectionsSaver.Format.mRXML;
                    connectionsSave.ConnectionFileName = saveFileDialog.FileName;
                    connectionsSave.Export = false;
                    connectionsSave.SaveSecurity = new Security.Save();
                    connectionsSave.ConnectionList = ConnectionList;
                    connectionsSave.ContainerList = ContainerList;
                    connectionsSave.ConnectionTreeModel = ConnectionTreeModel;

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
                SQLConnProvider?.Enable();
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
                newConnectionInfo.CopyFrom(DefaultConnectionInfo.Instance);

                newConnectionInfo.Name = Settings.Default.IdentifyQuickConnectTabs ? string.Format(Language.strQuick, uri.Host) : uri.Host;

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
                MessageCollector.AddExceptionMessage(Language.strQuickConnectFailed, ex);
                return null;
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
        private static void GoToURL(string URL)
        {
            ConnectionInfo connectionInfo = new ConnectionInfo();
            connectionInfo.CopyFrom(DefaultConnectionInfo.Instance);

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
            ConnectionInitiator.OpenConnection(connectionInfo, ConnectionInfo.Force.DoNotJump);
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

        private static InterfaceControl FindConnectionContainer(ConnectionInfo connectionInfo)
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
            foreach (Control tempLoopVar_ctlChild in ctlParent.Controls)
            {
                var ctlChild = tempLoopVar_ctlChild;
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