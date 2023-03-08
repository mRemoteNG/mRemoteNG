using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config;
using mRemoteNG.Config.Connections;
using mRemoteNG.Config.Connections.Multiuser;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Putty;
using mRemoteNG.Config.Serializers.ConnectionSerializers.MsSql;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Messages;
using mRemoteNG.Properties;
using mRemoteNG.Security;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.Connection
{
    [SupportedOSPlatform("windows")]
    public class ConnectionsService
    {
        private static readonly object SaveLock = new object();
        private readonly PuttySessionsManager _puttySessionsManager;
        private readonly IDataProvider<string> _localConnectionPropertiesDataProvider;
        private readonly LocalConnectionPropertiesXmlSerializer _localConnectionPropertiesSerializer;
        private bool _batchingSaves = false;
        private bool _saveRequested = false;
        private bool _saveAsyncRequested = false;

        public bool IsConnectionsFileLoaded { get; set; }
        public bool UsingDatabase { get; private set; }
        public string ConnectionFileName { get; private set; }
        public RemoteConnectionsSyncronizer RemoteConnectionsSyncronizer { get; set; }
        public DateTime LastSqlUpdate { get; set; }
		public DateTime LastFileUpdate { get; set; }

        public ConnectionTreeModel ConnectionTreeModel { get; private set; }

        public ConnectionsService(PuttySessionsManager puttySessionsManager)
        {
            if (puttySessionsManager == null)
                throw new ArgumentNullException(nameof(puttySessionsManager));

            _puttySessionsManager = puttySessionsManager;
            var path = SettingsFileInfo.SettingsPath;
            _localConnectionPropertiesDataProvider =
                new FileDataProvider(Path.Combine(path, "LocalConnectionProperties.xml"));
            _localConnectionPropertiesSerializer = new LocalConnectionPropertiesXmlSerializer();
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
                Runtime.MessageCollector.AddExceptionMessage(Language.CouldNotCreateNewConnectionsFile, ex);
            }
        }

        public ConnectionInfo CreateQuickConnect(string connectionString, ProtocolType protocol)
        {
            try
            {
                var uriBuilder = new UriBuilder();
                uriBuilder.Scheme = "dummyscheme";

                if (connectionString.Contains("@"))
                {
                    var x = connectionString.Split('@');
                    uriBuilder.UserName = x[0];
                    connectionString = x[1];
                }
                if (connectionString.Contains(":"))
                {
                    var x = connectionString.Split(':');
                    connectionString = x[0];
                    uriBuilder.Port = Convert.ToInt32(x[1]);
                }

                uriBuilder.Host = connectionString;

                var newConnectionInfo = new ConnectionInfo();
                newConnectionInfo.CopyFrom(DefaultConnectionInfo.Instance);

                newConnectionInfo.Name = Properties.OptionsTabsPanelsPage.Default.IdentifyQuickConnectTabs
                    ? string.Format(Language.Quick, uriBuilder.Host)
                    : uriBuilder.Host;

                newConnectionInfo.Protocol = protocol;
                newConnectionInfo.Hostname = uriBuilder.Host;
                newConnectionInfo.Username = uriBuilder.UserName;

                if (uriBuilder.Port == -1)
                {
                    newConnectionInfo.SetDefaultPort();
                }
                else
                {
                    newConnectionInfo.Port = uriBuilder.Port;
                }

                if (string.IsNullOrEmpty(newConnectionInfo.Panel))
                    newConnectionInfo.Panel = Language.General;

                newConnectionInfo.IsQuickConnect = true;

                return newConnectionInfo;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(Language.QuickConnectFailed, ex);
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

            var connectionLoader = useDatabase
                ? (IConnectionsLoader)new SqlConnectionsLoader(_localConnectionPropertiesSerializer, _localConnectionPropertiesDataProvider)
                : new XmlConnectionsLoader(connectionFileName);

            var newConnectionTreeModel = connectionLoader.Load();

            if (useDatabase)
                LastSqlUpdate = DateTime.Now;

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
            Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, $"Connections loaded using {connectionLoader.GetType().Name}");
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
		/// All calls to <see cref="SaveConnections()"/> or <see cref="SaveConnectionsAsync"/>
		/// will be deferred until the returned <see cref="DisposableAction"/> is disposed.
		/// Once disposed, this will immediately executes a single <see cref="SaveConnections()"/>
		/// or <see cref="SaveConnectionsAsync"/> if one has been requested.
		/// Place this call in a 'using' block to represent a batched saving context.
		/// </summary>
		/// <returns></returns>
		public DisposableAction BatchedSavingContext()
        {
			return new DisposableAction(BeginBatchingSaves, EndBatchingSaves);
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
        public void SaveConnections(ConnectionTreeModel connectionTreeModel, bool useDatabase, SaveFilter saveFilter, string connectionFileName, bool forceSave = false, string propertyNameTrigger = "")
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
                    ? (ISaver<ConnectionTreeModel>)new SqlConnectionsSaver(saveFilter, _localConnectionPropertiesSerializer, _localConnectionPropertiesDataProvider)
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
                Runtime.MessageCollector?.AddExceptionMessage(string.Format(Language.ConnectionsFileCouldNotSaveAs, connectionFileName), ex, logOnly: false);
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
                    SaveConnections(ConnectionTreeModel, UsingDatabase, new SaveFilter(), ConnectionFileName, propertyNameTrigger: propertyNameTrigger);
                }
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        public string GetStartupConnectionFileName()
        {
            /*
            if (Properties.OptionsBackupPage.Default.LoadConsFromCustomLocation == true && Properties.OptionsBackupPage.Default.BackupLocation != "")
            {
                return Properties.OptionsBackupPage.Default.BackupLocation;
            } else {
                return GetDefaultStartupConnectionFileName();
            }
            */
            if (Properties.OptionsConnectionsPage.Default.ConnectrionFilePath != "")
            {
                return Properties.OptionsConnectionsPage.Default.ConnectrionFilePath;
            }
            else
            {
                return GetDefaultStartupConnectionFileName();
            }
        }

        public string GetDefaultStartupConnectionFileName()
        {
            return Runtime.IsPortableEdition ? GetDefaultStartupConnectionFileNamePortableEdition() : GetDefaultStartupConnectionFileNameNormalEdition();
        }

        private void UpdateCustomConsPathSetting(string filename)
        {
            if (filename == GetDefaultStartupConnectionFileName())
            {
                Properties.OptionsBackupPage.Default.LoadConsFromCustomLocation = false;
            }
            else
            {
                Properties.OptionsBackupPage.Default.LoadConsFromCustomLocation = true;
                Properties.OptionsBackupPage.Default.BackupLocation = filename;
            }
        }

        private string GetDefaultStartupConnectionFileNameNormalEdition()
        {
            var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Application.ProductName, ConnectionsFileInfo.DefaultConnectionsFile);
            return File.Exists(appDataPath) ? appDataPath : GetDefaultStartupConnectionFileNamePortableEdition();
        }

        private string GetDefaultStartupConnectionFileNamePortableEdition()
        {
            return Path.Combine(ConnectionsFileInfo.DefaultConnectionsPath, ConnectionsFileInfo.DefaultConnectionsFile);
        }

        #region Events

        public event EventHandler<ConnectionsLoadedEventArgs> ConnectionsLoaded;
        public event EventHandler<ConnectionsSavedEventArgs> ConnectionsSaved;

        private void RaiseConnectionsLoadedEvent(Optional<ConnectionTreeModel> previousTreeModel, ConnectionTreeModel newTreeModel, bool previousSourceWasDatabase, bool newSourceIsDatabase, string newSourcePath)
        {
            ConnectionsLoaded?.Invoke(this, new ConnectionsLoadedEventArgs(previousTreeModel, newTreeModel, previousSourceWasDatabase, newSourceIsDatabase, newSourcePath));
        }

        private void RaiseConnectionsSavedEvent(ConnectionTreeModel modelThatWasSaved, bool previouslyUsingDatabase, bool usingDatabase, string connectionFileName)
        {
            ConnectionsSaved?.Invoke(this, new ConnectionsSavedEventArgs(modelThatWasSaved, previouslyUsingDatabase, usingDatabase, connectionFileName));
        }

        #endregion
    }
}