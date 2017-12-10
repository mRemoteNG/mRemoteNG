using System;
using mRemoteNG.Tree;

namespace mRemoteNG.Config.Connections
{
    public class ConnectionsSavedEventArgs
    {
        public ConnectionTreeModel ModelThatWasSaved { get; }
        public bool PreviouslyUsingDatabase { get; }
        public bool UsingDatabase { get; }
        public string ConnectionFileName { get; }

        public ConnectionsSavedEventArgs(ConnectionTreeModel modelThatWasSaved, bool previouslyUsingDatabase, bool usingDatabase, string connectionFileName)
        {
            if (modelThatWasSaved == null)
                throw new ArgumentNullException(nameof(modelThatWasSaved));

            ModelThatWasSaved = modelThatWasSaved;
            PreviouslyUsingDatabase = previouslyUsingDatabase;
            UsingDatabase = usingDatabase;
            ConnectionFileName = connectionFileName;
        }
    }
}
