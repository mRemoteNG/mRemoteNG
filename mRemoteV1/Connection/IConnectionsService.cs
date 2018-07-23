using System;
using System.Security;
using mRemoteNG.Config.Connections;
using mRemoteNG.Config.Connections.Multiuser;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Security;
using mRemoteNG.Tree;

namespace mRemoteNG.Connection
{
    public interface IConnectionsService
    {
        string ConnectionFileName { get; }
        ConnectionTreeModel ConnectionTreeModel { get; }
        DatabaseConnectorFactory DatabaseConnectorFactory { get; set; }
        SecureString EncryptionKey { get; set; }
        bool IsConnectionsFileLoaded { get; set; }
        DateTime LastSqlUpdate { get; set; }
        RemoteConnectionsSyncronizer RemoteConnectionsSyncronizer { get; set; }
        bool UsingDatabase { get; }

        event EventHandler<ConnectionsLoadedEventArgs> ConnectionsLoaded;
        event EventHandler<ConnectionsSavedEventArgs> ConnectionsSaved;

        ConnectionInfo CreateQuickConnect(string connectionString, ProtocolType protocol);
        string GetDefaultStartupConnectionFileName();
        string GetStartupConnectionFileName();
        void LoadConnections(bool withDialog = false);
        void LoadConnections(bool useDatabase, bool import, string connectionFileName);
        void LoadConnectionsAsync();
        void NewConnectionsFile(string filename);
        void SaveConnections();
        void SaveConnections(ConnectionTreeModel connectionTreeModel, bool useDatabase, SaveFilter saveFilter, string connectionFileName, bool forceSave = false);
        void SaveConnectionsAsync();
    }
}