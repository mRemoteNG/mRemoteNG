using System;
using mRemoteNG.Config.Connections;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Security;
using mRemoteNG.Tree;

namespace mRemoteNG.Connection
{
    public interface IConnectionsService
    {
        IConnectionTreeModel ConnectionTreeModel { get; }
        void NewConnectionsFile(string filename);
        ConnectionInfo CreateQuickConnect(string connectionString, ProtocolType protocol);

        /// <summary>
        /// Load connections from a source. <see cref="connectionFileName"/> is ignored if
        /// <see cref="useDatabase"/> is true.
        /// </summary>
        /// <param name="useDatabase"></param>
        /// <param name="import"></param>
        /// <param name="connectionFileName"></param>
        void LoadConnections(bool useDatabase, bool import, string connectionFileName);

        /// <summary>
        /// When turned on, calls to <see cref="ConnectionsService.SaveConnections()"/> or
        /// <see cref="ConnectionsService.SaveConnectionsAsync"/> will not immediately execute.
        /// Instead, they will be deferred until <see cref="ConnectionsService.EndBatchingSaves"/>
        /// is called.
        /// </summary>
        void BeginBatchingSaves();

        /// <summary>
        /// Immediately executes a single <see cref="ConnectionsService.SaveConnections()"/> or
        /// <see cref="ConnectionsService.SaveConnectionsAsync"/> if one has been requested
        /// since calling <see cref="ConnectionsService.BeginBatchingSaves"/>.
        /// </summary>
        void EndBatchingSaves();

        /// <summary>
        /// Saves the currently loaded <see cref="ConnectionsService.ConnectionTreeModel"/> with
        /// no <see cref="SaveFilter"/>.
        /// </summary>
        void SaveConnections();

        /// <summary>
        /// Saves the given <see cref="ConnectionsService.ConnectionTreeModel"/>.
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
        void SaveConnections(
            IConnectionTreeModel connectionTreeModel, 
            bool useDatabase, 
            SaveFilter saveFilter, 
            string connectionFileName, 
            bool forceSave = false,
            string propertyNameTrigger = "");

        /// <summary>
        /// Save the currently loaded connections asynchronously
        /// </summary>
        /// <param name="propertyNameTrigger">
        /// Optional. The name of the property that triggered
        /// this save.
        /// </param>
        void SaveConnectionsAsync(string propertyNameTrigger = "");

        event EventHandler<ConnectionsLoadedEventArgs> ConnectionsLoaded;
        event EventHandler<ConnectionsSavedEventArgs> ConnectionsSaved;
    }
}