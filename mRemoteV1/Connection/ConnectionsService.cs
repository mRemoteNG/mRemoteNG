using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.Connections;
using mRemoteNG.Config.Connections.Multiuser;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Putty;
using mRemoteNG.Config.Serializers.MsSql;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Messages;
using mRemoteNG.Security;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.Config;
using mRemoteNG.Credential;

namespace mRemoteNG.Connection
{
    public class ConnectionsService : IConnectionsService
    {
        private static readonly object SaveLock = new object();
        private readonly PuttySessionsManager _puttySessionsManager;
        private readonly IDataProvider<string> _localConnectionPropertiesDataProvider;
        private readonly LocalConnectionPropertiesXmlSerializer _localConnectionPropertiesSerializer;
        private bool _batchingSaves = false;
        private bool _saveRequested = false;
        private bool _saveAsyncRequested = false;
        private readonly CredentialService _credentialService;

        public bool IsConnectionsFileLoaded { get; set; }
        public bool UsingDatabase { get; private set; }
        public string ConnectionFileName { get; private set; }
        public RemoteConnectionsSyncronizer RemoteConnectionsSyncronizer { get; set; }
        public DateTime LastSqlUpdate { get; set; }

        public IConnectionTreeModel ConnectionTreeModel { get; private set; } = new ConnectionTreeModel();

        public ConnectionsService(PuttySessionsManager puttySessionsManager, CredentialService credentialService)
        {
            _puttySessionsManager = puttySessionsManager.ThrowIfNull(nameof(puttySessionsManager));
            _credentialService = credentialService.ThrowIfNull(nameof(credentialService));
            var path = SettingsFileInfo.SettingsPath;
            _localConnectionPropertiesDataProvider =
                new FileDataProvider(Path.Combine(path, "LocalConnectionProperties.xml"));
            _localConnectionPropertiesSerializer = new LocalConnectionPropertiesXmlSerializer();
            
            _puttySessionsManager.RootPuttySessionsNodes.ForEach(node => ConnectionTreeModel.AddRootNode(node));
        }

        public void NewConnectionsFile(string filename)
        {
            try
            {
                filename.ThrowIfNullOrEmpty(nameof(filename));
                ConnectionTreeModel.AddRootNode(new RootNodeInfo(RootNodeType.Connection));
                SaveConnections(ConnectionTreeModel, false, new SaveFilter(), filename, true);
                LoadConnections(false, filename);
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

                newConnectionInfo.Name = Settings.Default.IdentifyQuickConnectTabs
                    ? string.Format(Language.strQuick, uriBuilder.Host)
                    : uriBuilder.Host;

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
        public void LoadConnections(bool useDatabase, string connectionFileName)
        {
            var oldIsUsingDatabaseValue = UsingDatabase;

            var connectionLoader = useDatabase
                ? (IConnectionsLoader)new SqlConnectionsLoader(_localConnectionPropertiesSerializer,
                                                               _localConnectionPropertiesDataProvider)
                : new XmlConnectionsLoader(connectionFileName, _credentialService, this);

            var serializationResult = connectionLoader.Load();

            if (useDatabase)
                LastSqlUpdate = DateTime.Now;

            if (serializationResult == null)
            {
                DialogFactory.ShowLoadConnectionsFailedDialog(connectionFileName, "Decrypting connection file failed",
                                                              IsConnectionsFileLoaded);
                return;
            }

            IsConnectionsFileLoaded = true;
            ConnectionFileName = connectionFileName;
            UsingDatabase = useDatabase;

            if (ConnectionTreeModel.RootNodes.Any())
                ConnectionTreeModel.RemoveRootNode(ConnectionTreeModel.RootNodes.First());

            var rootNode = new RootNodeInfo(RootNodeType.Connection);
            rootNode.AddChildRange(serializationResult.ConnectionRecords);
            ConnectionTreeModel.AddRootNode(rootNode);

            UpdateCustomConsPathSetting(connectionFileName);
			// TODO: fix this call
            RaiseConnectionsLoadedEvent(new List<ConnectionInfo>(), new List<ConnectionInfo>(), 
										oldIsUsingDatabaseValue, useDatabase, connectionFileName);
            Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg,
                                                $"Connections loaded using {connectionLoader.GetType().Name}");
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
            else if (_saveRequested)
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
        /// <param name="propertyNameTrigger">
        /// Optional. The name of the property that triggered
        /// this save.
        /// </param>
        public void SaveConnections(
            IConnectionTreeModel connectionTreeModel, 
            bool useDatabase, 
            SaveFilter saveFilter, 
            string connectionFileName, 
            bool forceSave = false,
            string propertyNameTrigger = "")
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

                var saver = useDatabase
                    ? (ISaver<IConnectionTreeModel>)new SqlConnectionsSaver(saveFilter, _localConnectionPropertiesSerializer,
                        _localConnectionPropertiesDataProvider)
                    : new XmlConnectionsSaver(connectionFileName, saveFilter);

                saver.Save(connectionTreeModel, propertyNameTrigger);

                if (UsingDatabase)
                    LastSqlUpdate = DateTime.Now;

                UsingDatabase = useDatabase;
                ConnectionFileName = connectionFileName;
                RaiseConnectionsSavedEvent(connectionTreeModel, previouslyUsingDatabase, UsingDatabase,
                                           connectionFileName);
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Successfully saved connections");
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector?.AddExceptionMessage(
                                                              string.Format(Language.strConnectionsFileCouldNotSaveAs,
                                                                            connectionFileName), ex, logOnly: false);
            }
            finally
            {
                RemoteConnectionsSyncronizer?.Enable();
            }
        }

        /// <summary>
        /// Save the currently loaded connections asynchronously
        /// </summary>
        /// <param name="propertyNameTrigger">
        /// Optional. The name of the property that triggered
        /// this save.
        /// </param>
        public void SaveConnectionsAsync(string propertyNameTrigger = "")
        {
            if (_batchingSaves)
            {
                _saveAsyncRequested = true;
                return;
            }

            var t = new Thread(() =>
            {
                lock (SaveLock)
                {
                    SaveConnections(
                                    ConnectionTreeModel,
                                    UsingDatabase,
                                    new SaveFilter(),
                                    ConnectionFileName,
                                    propertyNameTrigger: propertyNameTrigger);
                }
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
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

        private void RaiseConnectionsLoadedEvent(List<ConnectionInfo> removedConnections, List<ConnectionInfo> addedConnections,
            bool previousSourceWasDatabase, bool newSourceIsDatabase,
            string newSourcePath)
        {
            ConnectionsLoaded?.Invoke(this, new ConnectionsLoadedEventArgs(
                removedConnections,
                addedConnections,
                                                                           previousSourceWasDatabase,
                                                                           newSourceIsDatabase,
                                                                           newSourcePath));
        }

        private void RaiseConnectionsSavedEvent(IConnectionTreeModel modelThatWasSaved, bool previouslyUsingDatabase, bool usingDatabase, string connectionFileName)
        {
            ConnectionsSaved?.Invoke(this, new ConnectionsSavedEventArgs(modelThatWasSaved, previouslyUsingDatabase, usingDatabase, connectionFileName));
        }
        #endregion
    }
}