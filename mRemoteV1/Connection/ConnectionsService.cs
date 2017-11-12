using System;
using System.IO;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.Connections;
using mRemoteNG.Config.Putty;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Connection
{
    public class ConnectionsService
    {
        private readonly PuttySessionsManager _puttySessionsManager;
        private readonly ConnectionsLoader _connectionsLoader;

        public bool IsConnectionsFileLoaded { get; set; }
        public bool UsingDatabase { get; private set; }
        public string ConnectionFileName { get; private set; }
        public static DateTime LastSqlUpdate { get; private set; }

        public ConnectionTreeModel ConnectionTreeModel
        {
            get { return Windows.TreeForm.ConnectionTree.ConnectionTreeModel; }
            set { Windows.TreeForm.ConnectionTree.ConnectionTreeModel = value; }
        }

        public ConnectionsService(PuttySessionsManager puttySessionsManager)
        {
            if (puttySessionsManager == null)
                throw new ArgumentNullException(nameof(puttySessionsManager));
            _puttySessionsManager = puttySessionsManager;
            _connectionsLoader = new ConnectionsLoader();
        }

        public void NewConnectionsFile(string filename)
        {
            try
            {
                UpdateCustomConsPathSetting(filename);

                var newConnectionsModel = new ConnectionTreeModel();
                newConnectionsModel.AddRootNode(new RootNodeInfo(RootNodeType.Connection));
                var connectionSaver = new ConnectionsSaver
                {
                    ConnectionTreeModel = newConnectionsModel,
                    ConnectionFileName = filename,
                    SaveFilter = new Security.SaveFilter()
                };
                connectionSaver.SaveConnections();

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
            var newConnectionTreeModel = _connectionsLoader.LoadConnections(useDatabase, import, connectionFileName);
            IsConnectionsFileLoaded = true;
            ConnectionFileName = connectionFileName;
            UsingDatabase = useDatabase;
            ConnectionTreeModel = newConnectionTreeModel;
            RaiseConnectionsLoadedEvent(oldConnectionTreeModel, newConnectionTreeModel, oldIsUsingDatabaseValue, useDatabase, connectionFileName);
            return newConnectionTreeModel;
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

        public string GetStartupConnectionFileName()
        {
            return Settings.Default.LoadConsFromCustomLocation == false ? GetDefaultStartupConnectionFileName() : Settings.Default.CustomConsPath;
        }

        public string GetDefaultStartupConnectionFileName()
        {
            return Runtime.IsPortableEdition ? GetDefaultStartupConnectionFileNamePortableEdition() : GetDefaultStartupConnectionFileNameNormalEdition();
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

        protected virtual void RaiseConnectionsLoadedEvent(ConnectionTreeModel previousTreeModel, ConnectionTreeModel newTreeModel,
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
        #endregion
    }
}