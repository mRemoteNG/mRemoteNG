using System;
using mRemoteNG.App;


namespace mRemoteNG.Config.Connections
{
    public class RemoteConnectionsSyncronizer : IDisposable
    {
        private readonly PeriodicConnectionsUpdateChecker _periodicConnectionsUpdateChecker;
        private readonly ConnectionsLoader _connectionsLoader;
        private readonly ConnectionsSaver _connectionsSaver;

        public RemoteConnectionsSyncronizer(IConnectionsUpdateChecker updateChecker)
        {
            _periodicConnectionsUpdateChecker = new PeriodicConnectionsUpdateChecker(updateChecker);
            _connectionsLoader = new ConnectionsLoader { UseDatabase = mRemoteNG.Settings.Default.UseSQLServer };
            _connectionsSaver = new ConnectionsSaver();
            _periodicConnectionsUpdateChecker.ConnectionsUpdateAvailable += Load;
        }

        public void Load()
        {
            Runtime.ConnectionTreeModel = _connectionsLoader.LoadConnections(false);
        }

        private void Load(object sender, ConnectionsUpdateAvailableEventArgs args)
        {
            Load();
            args.Handled = true;
        }

        public void Enable() => _periodicConnectionsUpdateChecker.Enable();
        public void Disable() => _periodicConnectionsUpdateChecker.Disable();


        ~RemoteConnectionsSyncronizer()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool itIsSafeToAlsoFreeManagedObjects)
        {
            if (!itIsSafeToAlsoFreeManagedObjects) return;
            _periodicConnectionsUpdateChecker.Dispose();
        }
    }
}