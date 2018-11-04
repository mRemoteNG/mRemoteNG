using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.Connections;
using mRemoteNG.Config.Connections.Multiuser;
using mRemoteNG.Config.Putty;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Messages;
using mRemoteNG.Security;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI;

namespace mRemoteNG.Connection
{
    public class ConnectionsService
    {
        private static readonly object SaveLock = new object();
        private readonly PuttySessionsManager _puttySessionsManager;
        private bool _batchingSaves = false;
        private bool _saveRequested = false;
        private bool _saveAsyncRequested = false;

        public bool IsConnectionsFileLoaded { get; set; }
        public bool UsingDatabase { get; private set; }
        public string ConnectionFileName { get; private set; }
        public RemoteConnectionsSyncronizer RemoteConnectionsSyncronizer { get; set; }
        public DateTime LastSqlUpdate { get; set; }

        public ConnectionTreeModel ConnectionTreeModel { get; private set; }

        public ConnectionsService(PuttySessionsManager puttySessionsManager)
        {
            if (puttySessionsManager == null)
                throw new ArgumentNullException(nameof(puttySessionsManager));

            _puttySessionsManager = puttySessionsManager;
        }

        public void NewConnectionsFile(string filename)
        {
            try
            {
                filename.ThrowIfNullOrEmpty(nameof(filename));
                var newConnectionsModel = new ConnectionTreeModel();
                newConnectionsModel.AddRootNode(new RootNodeInfo(RootNodeType.Connection));
                SaveConnections(newConnectionsModel, false, new SaveFilter(), filename, true);
                LoadConnections(false, false, filename);
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
                var uriBuilder = new UriBuilder();
                uriBuilder.Scheme = "dummyscheme";
                uriBuilder.Host = connectionString;

                var newConnectionInfo = new ConnectionInfo();
                newConnectionInfo.CopyFrom(DefaultConnectionInfo.Instance);

                newConnectionInfo.Name = Settings.Default.IdentifyQuickConnectTabs ? string.Format(Language.strQuick, uriBuilder.Host) : uriBuilder.Host;

                newConnectionInfo.Protocol = protocol;
                newConnectionInfo.Hostname = uriBuilder.Host;
                if (uriBuilder.Port == -1)
                {
                    newConnectionInfo.SetDefaultPort();
                }
                else
                {
                    newConnectionInfo.Port = uriBuilder.Port;
                }

                if (string.IsNullOrEmpty(newConnectionInfo.Panel))
                    newConnectionInfo.Panel = Language.strGeneral;

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
        public void LoadConnections(bool useDatabase, bool import, string connectionFileName)
        {
            var oldConnectionTreeModel = ConnectionTreeModel;
            var oldIsUsingDatabaseValue = UsingDatabase;

            var newConnectionTreeModel = useDatabase
                ? new SqlConnectionsLoader().Load()
                : new XmlConnectionsLoader(connectionFileName).Load();

            if (newConnectionTreeModel == null)
            {
                DialogFactory.ShowLoadConnectionsFailedDialog(connectionFileName, "Decrypting connection file failed", IsConnectionsFileLoaded);
                return;
            }

            IsConnectionsFileLoaded = true;
            ConnectionFileName = connectionFileName;
            UsingDatabase = useDatabase;

            if (!import)
            {
                _puttySessionsManager.AddSessions();
                newConnectionTreeModel.RootNodes.AddRange(_puttySessionsManager.RootPuttySessionsNodes);
            }

            ConnectionTreeModel = newConnectionTreeModel;
            UpdateCustomConsPathSetting(connectionFileName);
            RaiseConnectionsLoadedEvent(oldConnectionTreeModel, newConnectionTreeModel, oldIsUsingDatabaseValue, useDatabase, connectionFileName);
        }

        /// <summary>
        /// When turned on, calls to <see cref="SaveConnections()"/> or
        /// <see cref="SaveConnectionsAsync"/> will not immediately execute.
        /// Instead, they will be deferred until <see cref="EndBatchingSaves"/>
        /// is called.
        /// </summary>
        public void BeginBatchingSaves()
        {
            _batchingSaves = true;
        }

        /// <summary>
        /// Immediately executes a single <see cref="SaveConnections()"/> or
        /// <see cref="SaveConnectionsAsync"/> if one has been requested
        /// since calling <see cref="BeginBatchingSaves"/>.
        /// </summary>
        public void EndBatchingSaves()
        {
            _batchingSaves = false;

            if (_saveAsyncRequested)
                SaveConnectionsAsync();
            else if(_saveRequested)
                SaveConnections();
        }

        /// <summary>
        /// Saves the currently loaded <see cref="ConnectionTreeModel"/> with
        /// no <see cref="SaveFilter"/>.
        /// </summary>
        public void SaveConnections()
        {
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
        /// <param name="forceSave">Bypasses safety checks that prevent saving if a connection file isn't loaded.</param>
        public void SaveConnections(ConnectionTreeModel connectionTreeModel, bool useDatabase, SaveFilter saveFilter, string connectionFileName, bool forceSave = false)
        {
            if (connectionTreeModel == null)
                return;

            if (!forceSave && !IsConnectionsFileLoaded)
                return;

            if (_batchingSaves)
            {
                _saveRequested = true;
                return;
            }

            try
            {
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Saving connections...");
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
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Successfully saved connections");
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
            if (_batchingSaves)
            {
                _saveAsyncRequested = true;
                return;
            }

            var t = new Thread(SaveConnectionsBGd);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void SaveConnectionsBGd()
        {
            lock (SaveLock)
            {
                SaveConnections();
            }
        }

        public string GetStartupConnectionFileName()
        {
            return Settings.Default.LoadConsFromCustomLocation == false 
                ? GetDefaultStartupConnectionFileName() 
                : Settings.Default.CustomConsPath;
        }

        public string GetDefaultStartupConnectionFileName()
        {
            return Runtime.IsPortableEdition 
                ? GetDefaultStartupConnectionFileNamePortableEdition() 
                : GetDefaultStartupConnectionFileNameNormalEdition();
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