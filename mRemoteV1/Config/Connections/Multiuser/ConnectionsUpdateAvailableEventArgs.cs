using System;
using mRemoteNG.Config.DatabaseConnectors;

namespace mRemoteNG.Config.Connections.Multiuser
{
    public delegate void ConnectionsUpdateAvailableEventHandler(object sender, ConnectionsUpdateAvailableEventArgs args);

    public class ConnectionsUpdateAvailableEventArgs : EventArgs
    {
        public IDatabaseConnector DatabaseConnector { get; private set; }
        public DateTime UpdateTime { get; private set; }
        public bool Handled { get; set; }

        public ConnectionsUpdateAvailableEventArgs(IDatabaseConnector databaseConnector, DateTime updateTime)
        {
            if (databaseConnector == null)
                throw new ArgumentNullException(nameof(databaseConnector));
            DatabaseConnector = databaseConnector;
            UpdateTime = updateTime;
        }
    }
}