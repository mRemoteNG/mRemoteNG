using System;
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
        public Optional<ConnectionTreeModel> PreviousConnectionTreeModel { get; }

        /// <summary>
        /// True if the previous <see cref="ConnectionTreeModel"/> was loaded from
        /// a database.
        /// </summary>
        public bool PreviousSourceWasDatabase { get; }

        /// <summary>
        /// The new <see cref="ConnectionTreeModel"/> that is being loaded.
        /// </summary>
        public ConnectionTreeModel NewConnectionTreeModel { get; }

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
            Optional<ConnectionTreeModel> previousTreeModelModel, ConnectionTreeModel newTreeModelModel,
            bool previousSourceWasDatabase, bool newSourceIsDatabase,
            string newSourcePath)
        {
            if (previousTreeModelModel == null)
                throw new ArgumentNullException(nameof(previousTreeModelModel));
            if (newTreeModelModel == null)
                throw new ArgumentNullException(nameof(newTreeModelModel));
            if (newSourcePath == null)
                throw new ArgumentNullException(nameof(newSourcePath));

            PreviousConnectionTreeModel = previousTreeModelModel;
            PreviousSourceWasDatabase = previousSourceWasDatabase;
            NewConnectionTreeModel = newTreeModelModel;
            NewSourceIsDatabase = newSourceIsDatabase;
            NewSourcePath = newSourcePath;
        }
    }
}
