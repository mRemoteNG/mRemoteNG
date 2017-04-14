using mRemoteNG.App.Info;
using mRemoteNG.Config.Connections;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Messages;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.Window;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Security;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using mRemoteNG.Config.Connections.Multiuser;
using mRemoteNG.Credential;
using mRemoteNG.Messages.MessageWriters;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.UI;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Forms.Input;
using mRemoteNG.UI.TaskDialog;
using WeifenLuo.WinFormsUI.Docking;
using static System.IO.Path;


namespace mRemoteNG.App
{
    public static class Runtime
    {
        #region Public Properties
        public static WindowList WindowList { get; set; }
        public static MessageCollector MessageCollector { get; } = new MessageCollector();
        public static IList<IMessageWriter> MessageWriters { get; } = new List<IMessageWriter>();
        public static NotificationAreaIcon NotificationAreaIcon { get; set; }
        public static bool IsConnectionsFileLoaded { get; set; }
        public static RemoteConnectionsSyncronizer RemoteConnectionsSyncronizer { get; set; }
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private static DateTime LastSqlUpdate { get; set; }
        public static ObservableCollection<ExternalTool> ExternalTools { get; set; } = new ObservableCollection<ExternalTool>();
        public static SecureString EncryptionKey { get; set; } = new RootNodeInfo(RootNodeType.Connection).PasswordString.ConvertToSecureString();
        public static ConnectionTreeModel ConnectionTreeModel
        {
            get { return Windows.TreeForm.ConnectionTree.ConnectionTreeModel; }
            set { Windows.TreeForm.ConnectionTree.ConnectionTreeModel = value; }
        }
        public static CredentialManager CredentialManager { get; } = new CredentialManager();
        #endregion

        #region Panels
        public static Form AddPanel(string title = "", bool noTabber = false)
        {
            try
            {
                var connectionForm = new ConnectionWindow(new DockContent());
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
            connectionForm.Show(FrmMain.Default.pnlDock, DockState.Document);
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
            var cMen = new ContextMenuStrip();
            var cMenRen = CreateRenameMenuItem(pnlcForm);
            var cMenScreens = CreateScreensMenuItem(pnlcForm);
            cMen.Items.AddRange(new ToolStripItem[] { cMenRen, cMenScreens });
            pnlcForm.TabPageContextMenuStrip = cMen;
        }

        private static ToolStripMenuItem CreateScreensMenuItem(DockContent pnlcForm)
        {
            var cMenScreens = new ToolStripMenuItem
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
            var cMenRen = new ToolStripMenuItem
            {
                Text = Language.strRename,
                Image = Resources.Rename,
                Tag = pnlcForm
            };
            cMenRen.Click += cMenConnectionPanelRename_Click;
            return cMenRen;
        }

        private static void cMenConnectionPanelRename_Click(object sender, EventArgs e)
        {
            try
            {
                var conW = (ConnectionWindow)((ToolStripMenuItem)sender).Tag;

                var nTitle = "";
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

        private static void cMenConnectionPanelScreens_DropDownOpening(object sender, EventArgs e)
        {
            try
            {
                var cMenScreens = (ToolStripMenuItem)sender;
                cMenScreens.DropDownItems.Clear();

                for (var i = 0; i <= Screen.AllScreens.Length - 1; i++)
                {
                    var cMenScreen = new ToolStripMenuItem(Language.strScreen + " " + Convert.ToString(i + 1))
                    {
                        Tag = new ArrayList(),
                        Image = Resources.Monitor_GoTo
                    };
                    ((ArrayList) cMenScreen.Tag).Add(Screen.AllScreens[i]);
                    ((ArrayList) cMenScreen.Tag).Add(cMenScreens.Tag);
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
                var tagEnumeration = (IEnumerable)((ToolStripMenuItem)sender).Tag;
                if (tagEnumeration == null) return;
                foreach (var obj in tagEnumeration)
                {
                    var screen1 = obj as Screen;
                    if (screen1 != null)
                    {
                        screen = screen1;
                    }
                    else if (obj is DockContent)
                    {
                        panel = (DockContent)obj;
                    }
                }
                Screens.SendPanelToScreen(panel, screen);
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
                var connectionsLoader = new ConnectionsLoader();

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
                var fileStream = File.Open(filename, FileMode.CreateNew, FileAccess.Write, FileShare.None);
                using (var xmlTextWriter = new XmlTextWriter(fileStream, System.Text.Encoding.UTF8))
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
                }

                // Load config
                connectionsLoader.ConnectionFileName = filename;
                ConnectionTreeModel = connectionsLoader.LoadConnections(CredentialManager.GetCredentialRecords(), false);
                Windows.TreeForm.ConnectionTree.ConnectionTreeModel = ConnectionTreeModel;
            }
            catch (Exception ex)
            {
                MessageCollector.AddExceptionMessage(Language.strCouldNotCreateNewConnectionsFile, ex);
            }
        }

        public static void LoadConnectionsAsync()
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
                RemoteConnectionsSyncronizer?.Disable();

                if (!Settings.Default.UseSQLServer)
                {
                    if (withDialog)
                    {
                        var loadDialog = ConnectionsLoadDialog();
                        if (loadDialog.ShowDialog() != DialogResult.OK) return;
                        connectionsLoader.ConnectionFileName = loadDialog.FileName;
                    }
                    else
                    {
                        connectionsLoader.ConnectionFileName = GetStartupConnectionFileName();
                    }

                    CreateBackupFile(connectionsLoader.ConnectionFileName);
                }

                connectionsLoader.UseDatabase = Settings.Default.UseSQLServer;
                ConnectionTreeModel = connectionsLoader.LoadConnections(CredentialManager.GetCredentialRecords(), false);
                Windows.TreeForm.ConnectionTree.ConnectionTreeModel = ConnectionTreeModel;

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
                RemoteConnectionsSyncronizer?.Enable();
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
                    NewConnections(connectionsLoader.ConnectionFileName);
                    return;
                }

                MessageCollector.AddExceptionMessage(string.Format(Language.strConnectionsFileCouldNotBeLoaded, connectionsLoader.ConnectionFileName), ex);
                if (connectionsLoader.ConnectionFileName != GetStartupConnectionFileName())
                {
                    LoadConnections(withDialog, update);
                }
                else
                {
                    MessageBox.Show(FrmMain.Default,
                        string.Format(Language.strErrorStartupConnectionFileLoad, Environment.NewLine, Application.ProductName, GetStartupConnectionFileName(), MiscTools.GetExceptionMessageRecursive(ex)),
                        @"Could not load startup file.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }
        }

        private static OpenFileDialog ConnectionsLoadDialog()
        {
            return new OpenFileDialog
            {
                CheckFileExists = true,
                InitialDirectory = ConnectionsFileInfo.DefaultConnectionsPath,
                Filter = Language.strFiltermRemoteXML + @"|*.xml|" + Language.strFilterAll + @"|*.*"
            };
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
                var backupFileName = string.Format(Settings.Default.BackupFileNameFormat, fileName, DateTime.UtcNow);
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
            var fileName = GetFileName(baseName);
            var directoryName = GetDirectoryName(baseName);

            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(directoryName)) return;

            var searchPattern = string.Format(Settings.Default.BackupFileNameFormat, fileName, "*");
            var files = Directory.GetFiles(directoryName, searchPattern);

            if (files.Length <= Settings.Default.BackupFileKeepCount) return;

            Array.Sort(files);
            Array.Resize(ref files, files.Length - Settings.Default.BackupFileKeepCount);

            foreach (var file in files)
            {
                File.Delete(file);
            }
        }

        private static string GetDefaultStartupConnectionFileName()
        {
            var newPath = ConnectionsFileInfo.DefaultConnectionsPath + "\\" + ConnectionsFileInfo.DefaultConnectionsFile;
#if !PORTABLE
			var oldPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + Application.ProductName + "\\" + ConnectionsFileInfo.DefaultConnectionsFile;
            // ReSharper disable once ConvertIfStatementToReturnStatement
			if (File.Exists(oldPath)) return oldPath;
#endif
            return newPath;
        }

        public static string GetStartupConnectionFileName()
        {
            return Settings.Default.LoadConsFromCustomLocation == false ? GetDefaultStartupConnectionFileName() : Settings.Default.CustomConsPath;
        }

        public static void SaveConnectionsAsync()
        {
            var t = new Thread(SaveConnectionsBGd);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private static readonly object SaveLock = new object();
        private static void SaveConnectionsBGd()
        {
            Monitor.Enter(SaveLock);
            SaveConnections();
            Monitor.Exit(SaveLock);
        }

        public static void SaveConnections()
        {
            if (ConnectionTreeModel == null) return;
            if (!IsConnectionsFileLoaded) return;

            try
            {
                RemoteConnectionsSyncronizer?.Disable();

                var connectionsSaver = new ConnectionsSaver();

                if (!Settings.Default.UseSQLServer)
                    connectionsSaver.ConnectionFileName = GetStartupConnectionFileName();

                connectionsSaver.SaveFilter = new SaveFilter();
                connectionsSaver.ConnectionTreeModel = ConnectionTreeModel;

                if (Settings.Default.UseSQLServer)
                {
                    connectionsSaver.SaveFormat = ConnectionsSaver.Format.SQL;
                    connectionsSaver.SQLHost = Settings.Default.SQLHost;
                    connectionsSaver.SQLDatabaseName = Settings.Default.SQLDatabaseName;
                    connectionsSaver.SQLUsername = Settings.Default.SQLUser;
                    var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
                    connectionsSaver.SQLPassword = cryptographyProvider.Decrypt(Settings.Default.SQLPass, EncryptionKey);
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
                RemoteConnectionsSyncronizer?.Enable();
            }
        }

        public static void SaveConnectionsAs()
        {
            var connectionsSave = new ConnectionsSaver();

            try
            {
                RemoteConnectionsSyncronizer?.Disable();

                using (var saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.CheckPathExists = true;
                    saveFileDialog.InitialDirectory = ConnectionsFileInfo.DefaultConnectionsPath;
                    saveFileDialog.FileName = ConnectionsFileInfo.DefaultConnectionsFile;
                    saveFileDialog.OverwritePrompt = true;

                    var fileTypes = new List<string>();
                    fileTypes.AddRange(new[] { Language.strFiltermRemoteXML, "*.xml" });
                    fileTypes.AddRange(new[] { Language.strFilterAll, "*.*" });

                    saveFileDialog.Filter = string.Join("|", fileTypes.ToArray());

                    if (saveFileDialog.ShowDialog(FrmMain.Default) != DialogResult.OK) return;

                    connectionsSave.SaveFormat = ConnectionsSaver.Format.mRXML;
                    connectionsSave.ConnectionFileName = saveFileDialog.FileName;
                    connectionsSave.SaveFilter = new SaveFilter();
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
                RemoteConnectionsSyncronizer?.Enable();
            }
        }
        #endregion

        #region Opening Connection
        public static ConnectionInfo CreateQuickConnect(string connectionString, ProtocolType protocol)
        {
            try
            {
                var uri = new Uri("dummyscheme" + Uri.SchemeDelimiter + connectionString);
                if (string.IsNullOrEmpty(uri.Host)) return null;

                var newConnectionInfo = new ConnectionInfo();
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
        public static ExternalTool GetExtAppByName(string name)
        {
            foreach (ExternalTool extA in ExternalTools)
            {
                if (extA.DisplayName == name)
                    return extA;
            }
            return null;
        }
        #endregion

        #region Misc
        private static void GoToUrl(string url)
        {
            var connectionInfo = new ConnectionInfo();
            connectionInfo.CopyFrom(DefaultConnectionInfo.Instance);

            connectionInfo.Name = "";
            connectionInfo.Hostname = url;
            connectionInfo.Protocol = url.StartsWith("https:") ? ProtocolType.HTTPS : ProtocolType.HTTP;
            connectionInfo.SetDefaultPort();
            connectionInfo.IsQuickConnect = true;
            var connectionInitiator = new ConnectionInitiator();
            connectionInitiator.OpenConnection(connectionInfo, ConnectionInfo.Force.DoNotJump);
        }

        public static void GoToWebsite()
        {
            GoToUrl(GeneralAppInfo.UrlHome);
        }

        public static void GoToDonate()
        {
            GoToUrl(GeneralAppInfo.UrlDonate);
        }

        public static void GoToForum()
        {
            GoToUrl(GeneralAppInfo.UrlForum);
        }

        public static void GoToBugs()
        {
            GoToUrl(GeneralAppInfo.UrlBugs);
        }

        // Override the font of all controls in a container with the default font based on the OS version
        public static void FontOverride(Control ctlParent)
        {
            foreach (Control tempLoopVarCtlChild in ctlParent.Controls)
            {
                var ctlChild = tempLoopVarCtlChild;
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