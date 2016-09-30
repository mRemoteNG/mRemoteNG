using System;
using mRemoteNG.Config.DatabaseConnectors;


namespace mRemoteNG.Config.Connections
{
    public delegate void ConnectionsUpdateAvailableEventHandler(object sender, ConnectionsUpdateAvailableEventArgs args);

    public class ConnectionsUpdateAvailableEventArgs : EventArgs
    {
        public IDatabaseConnector DatabaseConnector { get; private set; }

        public ConnectionsUpdateAvailableEventArgs(IDatabaseConnector databaseConnector)
        {
            DatabaseConnector = databaseConnector;
        }
    }
}