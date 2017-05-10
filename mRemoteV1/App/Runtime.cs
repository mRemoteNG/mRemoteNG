using mRemoteNG.App.Info;
using mRemoteNG.Config.Connections;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Messages;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Security;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.Config.Connections.Multiuser;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Messages.MessageWriters;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.UI;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.TaskDialog;
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
        public static ICredentialRepositoryList CredentialProviderCatalog { get; } = new CredentialRepositoryList();
        #endregion

        #region Connections Loading/Saving
        public static void NewConnections(string filename)
        {
            try
            {
                UpdateCustomConsPathSetting(filename);

                var newConnectionsModel = new ConnectionTreeModel();
                newConnectionsModel.AddRootNode(new RootNodeInfo(RootNodeType.Connection));
                var connectionSaver = new ConnectionsSaver
                {
                    ConnectionTreeModel = newConnectionsModel,
                    ConnectionFileName = filename
                };
                connectionSaver.SaveConnections();

                // Load config
                var connectionsLoader = new ConnectionsLoader { ConnectionFileName = filename };
                ConnectionTreeModel = connectionsLoader.LoadConnections(CredentialProviderCatalog.GetCredentialRecords(), false);
                Windows.TreeForm.ConnectionTree.ConnectionTreeModel = ConnectionTreeModel;
            }
            catch (Exception ex)
            {
                MessageCollector.AddExceptionMessage(Language.strCouldNotCreateNewConnectionsFile, ex);
            }
        }

        private static void UpdateCustomConsPathSetting(string filename)
        {
            if (filename == GetDefaultStartupConnectionFileName())
            {
                Settings.Default.LoadConsFromCustomLocation = false;
            }
            else
            {
                Settings.Default.LoadConsFromCustomLocation = true;
                Settings.Default.CustomConsPath = filename;
            }
        }

        public static void LoadConnectionsAsync()
        {
            _withDialog = false;

            var t = new Thread(LoadConnectionsBGd);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private static bool _withDialog;
        private static void LoadConnectionsBGd()
        {
            LoadConnections(_withDialog);
        }

        public static void LoadConnections(bool withDialog = false)
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
                ConnectionTreeModel = connectionsLoader.LoadConnections(CredentialProviderCatalog.GetCredentialRecords(), false);
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
                            LoadConnections(withDialog);
                            return;
                        case 1:
                            Settings.Default.UseSQLServer = false;
                            LoadConnections(true);
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
                    LoadConnections(withDialog);
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
                    saveFileDialog.Filter = $@"{Language.strFiltermRemoteXML}|*.xml|{Language.strFilterAll}|*.*";

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
    }
}