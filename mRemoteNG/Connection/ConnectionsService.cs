using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config;
using mRemoteNG.Config.Connections;
using mRemoteNG.Config.Connections.Multiuser;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Putty;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using mRemoteNG.Security;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Sql;
using mRemoteNG.Config.Serializers.Versioning;
using mRemoteNG.Security.SymmetricEncryption;

namespace mRemoteNG.Connection
{
    [SupportedOSPlatform("windows")]
    public class ConnectionsService(PuttySessionsManager puttySessionsManager)
    {
        private static readonly Lock SaveLock = new();
        private static readonly CompositeFormat ConnectionFileAlreadyOpenFormat = CompositeFormat.Parse("Connection file '{0}' is already open.");
        private readonly PuttySessionsManager _puttySessionsManager = puttySessionsManager ?? throw new ArgumentNullException(nameof(puttySessionsManager));
        private readonly IDataProvider<string> _localConnectionPropertiesDataProvider = new FileDataProvider(Path.Combine(SettingsFileInfo.SettingsPath, SettingsFileInfo.LocalConnectionProperties));
        private readonly LocalConnectionPropertiesXmlSerializer _localConnectionPropertiesSerializer = new LocalConnectionPropertiesXmlSerializer();
        private bool _batchingSaves;
        private bool _saveRequested;
        private bool _saveAsyncRequested;
        // Cached SQL custom encryption password — avoids re-prompting on every reload (#1646)
        private SecureString? _cachedSqlEncryptionPassword;

        public bool IsConnectionsFileLoaded { get; set; }
        public bool UsingDatabase { get; private set; }
        public string? ConnectionFileName { get; private set; }
        public RemoteConnectionsSyncronizer? RemoteConnectionsSyncronizer { get; set; }
        public DateTime LastSqlUpdate { get; set; }
		public DateTime LastFileUpdate { get; set; }

        public ConnectionTreeModel? ConnectionTreeModel { get; private set; }

        public void NewConnectionsFile(string filename)
        {
            try
            {
                filename.ThrowIfNullOrEmpty(nameof(filename));
                ConnectionTreeModel newConnectionsModel = new();
                newConnectionsModel.AddRootNode(new RootNodeInfo(RootNodeType.Connection));
                SaveConnections(newConnectionsModel, false, new SaveFilter(), filename, true);
                LoadConnections(false, false, filename);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(Language.CouldNotCreateNewConnectionsFile, ex);
            }
        }

        public static ConnectionInfo? CreateQuickConnect(string connectionString, ProtocolType protocol)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, Language.QuickConnectNoHostname);
                    return null;
                }

                // Extract RDP-specific flags before parsing host/port.
                // Supported flags: -ra[:true|false]  (UseRestrictedAdmin)
                //                  -rcg[:true|false] (UseRemoteCredentialGuard)
                // Example: "myserver -ra:false -rcg:false"
                bool? rdpRestrictedAdminOverride = null;
                bool? rdpRcgOverride = null;

                if (connectionString.Contains(' '))
                {
                    string[] parts = connectionString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    connectionString = parts[0];

                    foreach (string part in parts.Skip(1))
                    {
                        if (part.Equals("-ra", StringComparison.OrdinalIgnoreCase) ||
                            part.Equals("-ra:true", StringComparison.OrdinalIgnoreCase))
                            rdpRestrictedAdminOverride = true;
                        else if (part.Equals("-ra:false", StringComparison.OrdinalIgnoreCase))
                            rdpRestrictedAdminOverride = false;
                        else if (part.Equals("-rcg", StringComparison.OrdinalIgnoreCase) ||
                                 part.Equals("-rcg:true", StringComparison.OrdinalIgnoreCase))
                            rdpRcgOverride = true;
                        else if (part.Equals("-rcg:false", StringComparison.OrdinalIgnoreCase))
                            rdpRcgOverride = false;
                    }
                }

                UriBuilder uriBuilder = new()
                {
                    Scheme = "dummyscheme"
                };
                string explicitUsername = string.Empty;

                if (connectionString.Contains('@'))
                {
                    string[] x = connectionString.Split('@');
                    explicitUsername = x[0];
                    connectionString = x[1];
                }
                if (connectionString.Contains(':'))
                {
                    string[] x = connectionString.Split(':');
                    connectionString = x[0];
                    uriBuilder.Port = Convert.ToInt32(x[1], CultureInfo.InvariantCulture);
                }

                uriBuilder.Host = connectionString;

                ConnectionInfo newConnectionInfo = new();
                newConnectionInfo.CopyFrom(DefaultConnectionInfo.Instance);

                newConnectionInfo.Name = Properties.OptionsTabsPanelsPage.Default.IdentifyQuickConnectTabs
                    ? string.Format(CultureInfo.InvariantCulture, Language.Quick, connectionString)
                    : connectionString;

                newConnectionInfo.Protocol = protocol;
                newConnectionInfo.Hostname = connectionString;
                if (!string.IsNullOrWhiteSpace(explicitUsername))
                {
                    newConnectionInfo.Username = explicitUsername;
                }

                if (uriBuilder.Port == -1)
                {
                    newConnectionInfo.SetDefaultPort();
                }
                else
                {
                    newConnectionInfo.Port = uriBuilder.Port;
                }

                if (string.IsNullOrEmpty(newConnectionInfo.Panel))
                {
                    // Use the currently active panel instead of hardcoding "General" (#1682)
                    if (FrmMain.IsCreated && FrmMain.Default.pnlDock.ActiveDocument is ConnectionWindow activeCw)
                        newConnectionInfo.Panel = activeCw.TabText;
                    else
                        newConnectionInfo.Panel = "General";
                }

                newConnectionInfo.IsQuickConnect = true;

                // Apply RDP-specific flag overrides (only meaningful for RDP protocol)
                if (protocol == ProtocolType.RDP)
                {
                    if (rdpRestrictedAdminOverride.HasValue)
                        newConnectionInfo.UseRestrictedAdmin = rdpRestrictedAdminOverride.Value;
                    if (rdpRcgOverride.HasValue)
                        newConnectionInfo.UseRCG = rdpRcgOverride.Value;
                }

                return newConnectionInfo;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(Language.QuickConnectFailed, ex);
                return null;
            }
        }

        public void LoadAdditionalConnectionFile(string filename)
        {
            if (string.IsNullOrEmpty(filename)) return;

            try
            {
                // Prevent opening the same file twice (#2331)
                if (ConnectionTreeModel != null &&
                    ConnectionTreeModel.RootNodes.OfType<RootNodeInfo>()
                        .Any(r => string.Equals(r.Filename, filename, StringComparison.OrdinalIgnoreCase)))
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                        string.Format(CultureInfo.InvariantCulture, ConnectionFileAlreadyOpenFormat, filename));
                    return;
                }

                IConnectionsLoader connectionLoader = new XmlConnectionsLoader(filename);
                ConnectionTreeModel? loadedModel = connectionLoader.Load();

                if (loadedModel == null) return;

                if (ConnectionTreeModel == null)
                {
                    LoadConnections(false, false, filename);
                }
                else
                {
                    foreach (ContainerInfo root in loadedModel.RootNodes)
                    {
                        if (root is RootNodeInfo rni && string.IsNullOrEmpty(rni.Filename))
                        {
                            rni.Filename = filename;
                        }
                        ConnectionTreeModel.AddRootNode(root);
                    }
                }

                PersistAdditionalFileList();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(string.Format(CultureInfo.InvariantCulture, Language.LoadFromXmlFailed, filename), ex);
            }
        }

        public void CloseAdditionalConnectionFile(RootNodeInfo rootNode)
        {
            if (ConnectionTreeModel == null || rootNode == null) return;
            if (ConnectionTreeModel.RootNodes.Count <= 1) return;

            // Don't allow closing the primary connection file
            if (string.Equals(rootNode.Filename, ConnectionFileName, StringComparison.OrdinalIgnoreCase))
                return;

            ConnectionTreeModel.RemoveRootNode(rootNode);
            PersistAdditionalFileList();
        }

        public void LoadAdditionalConnectionFiles()
        {
            string saved = Properties.OptionsConnectionsPage.Default.AdditionalConnectionFiles;
            if (string.IsNullOrWhiteSpace(saved)) return;

            string[] files = saved.Split('|', StringSplitOptions.RemoveEmptyEntries);
            foreach (string file in files)
            {
                string expanded = Environment.ExpandEnvironmentVariables(file.Trim());
                if (File.Exists(expanded))
                {
                    LoadAdditionalConnectionFile(expanded);
                }
            }
        }

        private void PersistAdditionalFileList()
        {
            if (ConnectionTreeModel == null)
            {
                Properties.OptionsConnectionsPage.Default.AdditionalConnectionFiles = "";
                Properties.OptionsConnectionsPage.Default.Save();
                return;
            }

            var additionalFiles = ConnectionTreeModel.RootNodes
                .OfType<RootNodeInfo>()
                .Where(r => !string.IsNullOrEmpty(r.Filename) &&
                            !string.Equals(r.Filename, ConnectionFileName, StringComparison.OrdinalIgnoreCase) &&
                            r.Type == RootNodeType.Connection)
                .Select(r => r.Filename)
                .Distinct(StringComparer.OrdinalIgnoreCase);

            Properties.OptionsConnectionsPage.Default.AdditionalConnectionFiles = string.Join("|", additionalFiles);
            Properties.OptionsConnectionsPage.Default.Save();
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
            ConnectionTreeModel? oldConnectionTreeModel = ConnectionTreeModel;
            bool oldIsUsingDatabaseValue = UsingDatabase;

            IConnectionsLoader connectionLoader;
            if (useDatabase)
            {
                IDatabaseConnector dbConnector = DatabaseConnectorFactory.DatabaseConnectorFromSettings();
                SqlDataProvider sqlDataProvider = new(dbConnector);
                SqlDatabaseMetaDataRetriever metaDataRetriever = new();
                SqlDatabaseVersionVerifier versionVerifier = new(dbConnector);
                bool triedCached = false;
                connectionLoader = new SqlConnectionsLoader(
                    _localConnectionPropertiesSerializer,
                    _localConnectionPropertiesDataProvider,
                    dbConnector,
                    sqlDataProvider,
                    metaDataRetriever,
                    versionVerifier,
                    new LegacyRijndaelCryptographyProvider(),
                    (filename) =>
                    {
                        // Return cached password on first call (avoids re-prompting on every reload — #1646)
                        if (_cachedSqlEncryptionPassword != null && !triedCached)
                        {
                            triedCached = true;
                            return new Optional<SecureString>(_cachedSqlEncryptionPassword);
                        }
                        // Cached password was wrong or not set — clear cache and prompt
                        _cachedSqlEncryptionPassword?.Dispose();
                        _cachedSqlEncryptionPassword = null;
                        Optional<SecureString> result = MiscTools.PasswordDialog(filename, false);
                        if (result.Any())
                            _cachedSqlEncryptionPassword = result.First();
                        return result;
                    });
            }
            else
            {
                connectionLoader = new XmlConnectionsLoader(connectionFileName);
            }

            ConnectionTreeModel newConnectionTreeModel = null!;
            try
            {
                newConnectionTreeModel = connectionLoader.Load();
                if (useDatabase)
                {
                    LastSqlUpdate = DateTime.Now.ToUniversalTime();
                    TrySaveSqlConnectionsCache(newConnectionTreeModel);
                }
            }
            catch (Exception ex) when (useDatabase)
            {
                string cachePath = Path.Combine(SettingsFileInfo.SettingsPath, SettingsFileInfo.SqlConnectionsCache);
                if (File.Exists(cachePath))
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                        $"Could not load connections from database ({ex.Message}). Loading from local cache in read-only mode.");
                    connectionLoader = new XmlConnectionsLoader(cachePath);
                    newConnectionTreeModel = connectionLoader.Load();
                }
                else
                {
                    throw;
                }
            }

            if (newConnectionTreeModel == null)
            {
                DialogFactory.ShowLoadConnectionsFailedDialog(connectionFileName, "Decrypting connection file failed", IsConnectionsFileLoaded);
                return;
            }

            IsConnectionsFileLoaded = true;
            ConnectionFileName = connectionFileName;
            Properties.OptionsConnectionsPage.Default.ConnectionFilePath = connectionFileName;
            Properties.OptionsConnectionsPage.Default.Save();

            UsingDatabase = useDatabase;

            if (!import)
            {
                _puttySessionsManager.AddSessions();
                newConnectionTreeModel.RootNodes.AddRange(_puttySessionsManager.RootPuttySessionsNodes);
            }
            
            // Set Filename on root nodes if not set
            if (!useDatabase)
            {
                foreach (var root in newConnectionTreeModel.RootNodes.OfType<RootNodeInfo>())
                {
                     if (string.IsNullOrEmpty(root.Filename)) root.Filename = connectionFileName;
                }
            }

            ConnectionTreeModel = newConnectionTreeModel;
            UpdateCustomConsPathSetting(connectionFileName);
            RaiseConnectionsLoadedEvent(oldConnectionTreeModel is not null ? new Optional<ConnectionTreeModel>(oldConnectionTreeModel) : new Optional<ConnectionTreeModel>(), newConnectionTreeModel, oldIsUsingDatabaseValue, useDatabase, connectionFileName);
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
            if (ConnectionTreeModel is null || ConnectionFileName is null)
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

                bool previouslyUsingDatabase = UsingDatabase;

                if (useDatabase)
                {
                    ISaver<ConnectionTreeModel> saver = (ISaver<ConnectionTreeModel>)new SqlConnectionsSaver(saveFilter, _localConnectionPropertiesSerializer, _localConnectionPropertiesDataProvider);
                    saver.Save(connectionTreeModel, propertyNameTrigger);
                    LastSqlUpdate = DateTime.Now.ToUniversalTime();
                }
                else
                {
                    // XML Saving with support for multiple roots/files
                    foreach (var rootNode in connectionTreeModel.RootNodes.OfType<RootNodeInfo>())
                    {
                        // PuTTY sessions are read-only (imported from registry) — never save them
                        // to disk. Without this check, PuTTY root (which has an empty Filename)
                        // would overwrite the main connections file with only PuTTY data.
                        if (rootNode.Type == Tree.Root.RootNodeType.PuttySessions)
                            continue;

                        string targetFile = rootNode.Filename;
                        if (string.IsNullOrEmpty(targetFile)) targetFile = connectionFileName;

                        // If Save As is detected (connectionFileName arg != ConnectionFileName prop), 
                        // and this is the "main" root (checked by Filename matching ConnectionFileName or being empty),
                        // then redirect to the new connectionFileName.
                        if (connectionFileName != ConnectionFileName && (rootNode.Filename == ConnectionFileName || string.IsNullOrEmpty(rootNode.Filename)))
                        {
                            targetFile = connectionFileName;
                            // Optionally update the root's filename to the new one?
                            // rootNode.Filename = connectionFileName; // Side effect?
                        }

                        var tempModel = new ConnectionTreeModel();
                        tempModel.AddRootNode(rootNode);

                        ISaver<ConnectionTreeModel> saver = new XmlConnectionsSaver(targetFile, saveFilter);
                        saver.Save(tempModel, propertyNameTrigger);
                        
                        if (targetFile == connectionFileName && File.Exists(connectionFileName))
                             LastFileUpdate = File.GetLastWriteTimeUtc(connectionFileName);
                    }
                }

                UsingDatabase = useDatabase;
                ConnectionFileName = connectionFileName;
                RaiseConnectionsSavedEvent(connectionTreeModel, previouslyUsingDatabase, UsingDatabase, connectionFileName);
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Successfully saved connections");
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector?.AddExceptionMessage(string.Format(CultureInfo.InvariantCulture, Language.ConnectionsFileCouldNotSaveAs, connectionFileName), ex, logOnly: false);
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

            ConnectionTreeModel? treeModel = ConnectionTreeModel;
            string? fileName = ConnectionFileName;
            if (treeModel is null || fileName is null)
                return;

            Thread t = new(() =>
            {
                lock (SaveLock)
                {
                    SaveConnections(treeModel, UsingDatabase, new SaveFilter(), fileName, propertyNameTrigger: propertyNameTrigger);
                }
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        public static string GetStartupConnectionFileName()
        {
            /*
            if (Properties.OptionsBackupPage.Default.LoadConsFromCustomLocation == true && Properties.OptionsBackupPage.Default.BackupLocation != "")
            {
                return Properties.OptionsBackupPage.Default.BackupLocation;
            } else {
                return GetDefaultStartupConnectionFileName();
            }
            */
            if (!string.IsNullOrWhiteSpace(Properties.OptionsConnectionsPage.Default.ConnectionFilePath))
            {
                return Environment.ExpandEnvironmentVariables(Properties.OptionsConnectionsPage.Default.ConnectionFilePath);
            }
            else
            {
                return GetDefaultStartupConnectionFileName();
            }
        }

        public static string GetDefaultStartupConnectionFileName()
        {
            return Runtime.IsPortableEdition ? GetDefaultStartupConnectionFileNamePortableEdition() : GetDefaultStartupConnectionFileNameNormalEdition();
        }

        private static void UpdateCustomConsPathSetting(string filename)
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

        private static string GetDefaultStartupConnectionFileNameNormalEdition()
        {
            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Application.ProductName ?? "mRemoteNG", ConnectionsFileInfo.DefaultConnectionsFile);
            return File.Exists(appDataPath) ? appDataPath : GetDefaultStartupConnectionFileNamePortableEdition();
        }

        private static string GetDefaultStartupConnectionFileNamePortableEdition()
        {
            return Path.Combine(ConnectionsFileInfo.DefaultConnectionsPath, ConnectionsFileInfo.DefaultConnectionsFile);
        }

        private static void TrySaveSqlConnectionsCache(ConnectionTreeModel connectionTreeModel)
        {
            try
            {
                string cachePath = Path.Combine(SettingsFileInfo.SettingsPath, SettingsFileInfo.SqlConnectionsCache);
                ConnectionTreeModel cacheModel = new();
                foreach (RootNodeInfo root in connectionTreeModel.RootNodes.OfType<RootNodeInfo>())
                    cacheModel.AddRootNode(root);
                XmlConnectionsSaver cacheSaver = new(cachePath, new SaveFilter());
                cacheSaver.Save(cacheModel);
                Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, $"SQL connections cache saved to '{cachePath}'");
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Failed to save SQL connections cache", ex);
            }
        }

        #region Events

        public event EventHandler<ConnectionsLoadedEventArgs>? ConnectionsLoaded;
        public event EventHandler<ConnectionsSavedEventArgs>? ConnectionsSaved;

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
