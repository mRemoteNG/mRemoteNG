using System;
using System.IO;
using System.Security;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.Connections;
using mRemoteNG.Config.Connections.Multiuser;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Putty;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Messages;
using mRemoteNG.Security;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.TaskDialog;

namespace mRemoteNG.Connection
{
    public class ConnectionsService
    {
        private static readonly object SaveLock = new object();
        private bool _showDialogWhenLoadingConnections;
        private readonly PuttySessionsManager _puttySessionsManager;
        private readonly Import _import;

        public bool IsConnectionsFileLoaded { get; set; }
        public bool UsingDatabase { get; private set; }
        public string ConnectionFileName { get; private set; }
        public RemoteConnectionsSyncronizer RemoteConnectionsSyncronizer { get; set; }
        public DateTime LastSqlUpdate { get; set; }
        public SecureString EncryptionKey { get; set; } = new RootNodeInfo(RootNodeType.Connection).PasswordString.ConvertToSecureString();

        public ConnectionTreeModel ConnectionTreeModel { get; private set; }
        //public ConnectionTreeModel ConnectionTreeModel
        //{
        //    get { return Windows.TreeForm.ConnectionTree.ConnectionTreeModel; }
        //    set { Windows.TreeForm.ConnectionTree.ConnectionTreeModel = value; }
        //}

        public ConnectionsService(PuttySessionsManager puttySessionsManager, Import import)
        {
            _puttySessionsManager = puttySessionsManager.ThrowIfNull(nameof(puttySessionsManager));
            _import = import.ThrowIfNull(nameof(import));
        }

        public void NewConnectionsFile(string filename)
        {
            try
            {
                var newConnectionsModel = new ConnectionTreeModel();
                newConnectionsModel.AddRootNode(new RootNodeInfo(RootNodeType.Connection));
                SaveConnections(newConnectionsModel, false, new SaveFilter(), filename);
                LoadConnections(false, false, filename);
                UpdateCustomConsPathSetting(filename);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(Language.strCouldNotCreateNewConnectionsFile, ex);
            }
        }

        public ConnectionInfo CreateQuickConnect(string connectionString, ProtocolType protocol)
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
                Runtime.MessageCollector.AddExceptionMessage(Language.strQuickConnectFailed, ex);
                return null;
            }
        }

        /// <summary>
        /// Load connections from a source. <see cref="connectionFileName"/> is ignored if
        /// <see cref="useDatabase"/> is true.
        /// </summary>
        /// <param name="useDatabase"></param>
        /// <param name="import"></param>
        /// <param name="connectionFileName"></param>
        public ConnectionTreeModel LoadConnections(bool useDatabase, bool import, string connectionFileName)
        {
            var oldConnectionTreeModel = ConnectionTreeModel;
            var oldIsUsingDatabaseValue = UsingDatabase;

            var newConnectionTreeModel =
                (useDatabase
                    ? new SqlConnectionsLoader().Load()
                    : new XmlConnectionsLoader(connectionFileName).Load())
                ?? new ConnectionTreeModel();

            if (!import)
            {
                _puttySessionsManager.AddSessions();
                newConnectionTreeModel.RootNodes.AddRange(_puttySessionsManager.RootPuttySessionsNodes);
            }

            IsConnectionsFileLoaded = true;
            ConnectionFileName = connectionFileName;
            UsingDatabase = useDatabase;
            ConnectionTreeModel = newConnectionTreeModel;
            RaiseConnectionsLoadedEvent(oldConnectionTreeModel, newConnectionTreeModel, oldIsUsingDatabaseValue, useDatabase, connectionFileName);
            return newConnectionTreeModel;
        }

        /// <summary>
        /// Saves the currently loaded <see cref="ConnectionTreeModel"/> with
        /// no <see cref="SaveFilter"/>.
        /// </summary>
        public void SaveConnections()
        {
            if (!IsConnectionsFileLoaded)
                return;
            SaveConnections(ConnectionTreeModel, UsingDatabase, new SaveFilter(), ConnectionFileName);
        }

        /// <summary>
        /// Saves the given <see cref="ConnectionTreeModel"/>.
        /// If <see cref="useDatabase"/> is true, <see cref="connectionFileName"/> is ignored
        /// </summary>
        /// <param name="connectionTreeModel"></param>
        /// <param name="useDatabase"></param>
        /// <param name="saveFilter"></param>
        /// <param name="connectionFileName"></param>
        public void SaveConnections(ConnectionTreeModel connectionTreeModel, bool useDatabase, SaveFilter saveFilter, string connectionFileName)
        {
            if (connectionTreeModel == null) return;

            try
            {
                RemoteConnectionsSyncronizer?.Disable();

                var previouslyUsingDatabase = UsingDatabase;
                if (useDatabase)
                    new SqlConnectionsSaver(saveFilter).Save(connectionTreeModel);
                else
                    new XmlConnectionsSaver(connectionFileName, saveFilter).Save(connectionTreeModel);

                if (UsingDatabase)
                    LastSqlUpdate = DateTime.Now;

                UsingDatabase = useDatabase;
                ConnectionFileName = connectionFileName;
                RaiseConnectionsSavedEvent(connectionTreeModel, previouslyUsingDatabase, UsingDatabase, connectionFileName);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector?.AddExceptionMessage(string.Format(Language.strConnectionsFileCouldNotSaveAs, connectionFileName), ex, logOnly:false);
            }
            finally
            {
                RemoteConnectionsSyncronizer?.Enable();
            }
        }

        public void SaveConnectionsAsync()
        {
            var t = new Thread(SaveConnectionsBGd);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void SaveConnectionsBGd()
        {
            Monitor.Enter(SaveLock);
            SaveConnections();
            Monitor.Exit(SaveLock);
        }

        public void LoadConnectionsAsync()
        {
            _showDialogWhenLoadingConnections = false;

            var t = new Thread(LoadConnectionsBGd);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        
        private void LoadConnectionsBGd()
        {
            LoadConnections(_showDialogWhenLoadingConnections);
        }

        public void LoadConnections(bool withDialog = false)
        {
            var connectionFileName = "";

            try
            {
                // disable sql update checking while we are loading updates
                RemoteConnectionsSyncronizer?.Disable();

                if (!Settings.Default.UseSQLServer)
                {
                    if (withDialog)
                    {
                        var loadDialog = DialogFactory.BuildLoadConnectionsDialog();
                        if (loadDialog.ShowDialog() != DialogResult.OK) return;
                        connectionFileName = loadDialog.FileName;
                    }
                    else
                    {
                        connectionFileName = GetStartupConnectionFileName();
                    }

                    var backupFileCreator = new FileBackupCreator();
                    backupFileCreator.CreateBackupFile(connectionFileName);

                    var backupPruner = new FileBackupPruner();
                    backupPruner.PruneBackupFiles(connectionFileName);
                }

                LoadConnections(Settings.Default.UseSQLServer, false, connectionFileName);

                if (Settings.Default.UseSQLServer)
                {
                    LastSqlUpdate = DateTime.Now;
                }
                else
                {
                    if (connectionFileName == GetDefaultStartupConnectionFileName())
                    {
                        Settings.Default.LoadConsFromCustomLocation = false;
                    }
                    else
                    {
                        Settings.Default.LoadConsFromCustomLocation = true;
                        Settings.Default.CustomConsPath = connectionFileName;
                    }
                }

                // re-enable sql update checking after updates are loaded
                RemoteConnectionsSyncronizer?.Enable();
            }
            catch (Exception ex)
            {
                if (Settings.Default.UseSQLServer)
                {
                    Runtime.MessageCollector.AddExceptionMessage(Language.strLoadFromSqlFailed, ex);
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
                    Runtime.MessageCollector.AddExceptionMessage(string.Format(Language.strConnectionsFileCouldNotBeLoadedNew, connectionFileName), ex, MessageClass.InformationMsg);

                    string[] commandButtons =
                    {
                        Language.ConfigurationCreateNew,
                        Language.ConfigurationCustomPath,
                        Language.ConfigurationImportFile,
                        Language.strMenuExit
                    };

                    var answered = false;
                    while (!answered)
                    {
                        try
                        {
                            CTaskDialog.ShowTaskDialogBox(
                                GeneralAppInfo.ProductName,
                                Language.ConnectionFileNotFound,
                                "", "", "", "", "",
                                string.Join(" | ", commandButtons),
                                ETaskDialogButtons.None,
                                ESysIcons.Question,
                                ESysIcons.Question);

                            switch (CTaskDialog.CommandButtonResult)
                            {
                                case 0:
                                    NewConnectionsFile(connectionFileName);
                                    answered = true;
                                    break;
                                case 1:
                                    LoadConnections(true);
                                    answered = true;
                                    break;
                                case 2:
                                    NewConnectionsFile(connectionFileName);
                                    _import.ImportFromFile(ConnectionTreeModel.RootNodes[0]);
                                    answered = true;
                                    break;
                                case 3:
                                    Application.Exit();
                                    answered = true;
                                    break;
                            }
                        }
                        catch (Exception exc)
                        {
                            Runtime.MessageCollector.AddExceptionMessage(string.Format(Language.strConnectionsFileCouldNotBeLoadedNew, connectionFileName), exc, MessageClass.InformationMsg);
                        }
                    }
                    return;
                }

                Runtime.MessageCollector.AddExceptionStackTrace(string.Format(Language.strConnectionsFileCouldNotBeLoaded, connectionFileName), ex);
                if (connectionFileName != GetStartupConnectionFileName())
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

        public string GetStartupConnectionFileName()
        {
            return Settings.Default.LoadConsFromCustomLocation == false ? GetDefaultStartupConnectionFileName() : Settings.Default.CustomConsPath;
        }

        public string GetDefaultStartupConnectionFileName()
        {
            return Runtime.IsPortableEdition ? GetDefaultStartupConnectionFileNamePortableEdition() : GetDefaultStartupConnectionFileNameNormalEdition();
        }

        private void UpdateCustomConsPathSetting(string filename)
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

        private string GetDefaultStartupConnectionFileNameNormalEdition()
        {
            var appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Application.ProductName,
                ConnectionsFileInfo.DefaultConnectionsFile);
            return File.Exists(appDataPath) ? appDataPath : GetDefaultStartupConnectionFileNamePortableEdition();
        }

        private string GetDefaultStartupConnectionFileNamePortableEdition()
        {
            return Path.Combine(ConnectionsFileInfo.DefaultConnectionsPath, ConnectionsFileInfo.DefaultConnectionsFile);
        }

        #region Events
        public event EventHandler<ConnectionsLoadedEventArgs> ConnectionsLoaded;
        public event EventHandler<ConnectionsSavedEventArgs> ConnectionsSaved;

        private void RaiseConnectionsLoadedEvent(Optional<ConnectionTreeModel> previousTreeModel, ConnectionTreeModel newTreeModel,
            bool previousSourceWasDatabase, bool newSourceIsDatabase,
            string newSourcePath)
        {
            ConnectionsLoaded?.Invoke(this, new ConnectionsLoadedEventArgs(
                previousTreeModel,
                newTreeModel,
                previousSourceWasDatabase,
                newSourceIsDatabase,
                newSourcePath));
        }

        private void RaiseConnectionsSavedEvent(ConnectionTreeModel modelThatWasSaved, bool previouslyUsingDatabase, bool usingDatabase, string connectionFileName)
        {
            ConnectionsSaved?.Invoke(this, new ConnectionsSavedEventArgs(modelThatWasSaved, previouslyUsingDatabase, usingDatabase, connectionFileName));
        }
        #endregion
    }
}