using System;
using System.Collections.Generic;
using mRemoteNG.Connection;
using mRemoteNG.Tools;
using mRemoteNG.Tree;

namespace mRemoteNG.Config.Connections
{
    public class ConnectionsLoadedEventArgs : EventArgs
    {
        /// <summary>
        /// The previous <see cref="ConnectionTreeModel"/> that is being
        /// unloaded.
        /// </summary>
        public List<ConnectionInfo> RemovedConnections { get; }

        /// <summary>
        /// True if the previous <see cref="ConnectionTreeModel"/> was loaded from
        /// a database.
        /// </summary>
        public bool PreviousSourceWasDatabase { get; }

        /// <summary>
        /// The new <see cref="ConnectionTreeModel"/> that is being loaded.
        /// </summary>
        public List<ConnectionInfo> AddedConnections { get; }

        /// <summary>
        /// True if the new <see cref="ConnectionTreeModel"/> was loaded from
        /// a database.
        /// </summary>
        public bool NewSourceIsDatabase { get; }

        /// <summary>
        /// The path to the new connections source.
        /// If <see cref="NewSourceIsDatabase"/> is True, this will be the server and database name.
        /// If False, it will be a file path to the connection file.
        /// </summary>
        public string NewSourcePath { get; }

        public ConnectionsLoadedEventArgs(
            List<ConnectionInfo> removedConnections, List<ConnectionInfo> addedConnections,
            bool previousSourceWasDatabase, bool newSourceIsDatabase,
            string newSourcePath)
        {
            RemovedConnections = removedConnections.ThrowIfNull(nameof(removedConnections));
            PreviousSourceWasDatabase = previousSourceWasDatabase;
            AddedConnections = addedConnections.ThrowIfNull(nameof(addedConnections));
            NewSourceIsDatabase = newSourceIsDatabase;
            NewSourcePath = newSourcePath.ThrowIfNull(nameof(newSourcePath));
        }
    }
}
