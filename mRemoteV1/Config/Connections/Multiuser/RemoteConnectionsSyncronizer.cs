using System;
using System.Timers;
using mRemoteNG.App;

namespace mRemoteNG.Config.Connections
{
    public class RemoteConnectionsSyncronizer : IConnectionsUpdateChecker
    {
        private readonly ConnectionsLoader _connectionsLoader;
        private readonly ConnectionsSaver _connectionsSaver;
        private readonly IConnectionsUpdateChecker _updateChecker;
        private readonly Timer _updateTimer;

        public RemoteConnectionsSyncronizer(IConnectionsUpdateChecker updateChecker)
        {
            _updateChecker = updateChecker;
            _updateTimer = new Timer(3000);
            _connectionsLoader = new ConnectionsLoader {UseDatabase = mRemoteNG.Settings.Default.UseSQLServer};
            _connectionsSaver = new ConnectionsSaver {SaveFormat = ConnectionsSaver.Format.SQL};
            SetEventListeners();
        }

        public double TimerIntervalInMilliseconds => _updateTimer.Interval;
        public bool IsUpdateAvailable() => _updateChecker.IsUpdateAvailable();
        public void IsUpdateAvailableAsync() => _updateChecker.IsUpdateAvailableAsync();

        public event EventHandler UpdateCheckStarted;
        public event UpdateCheckFinishedEventHandler UpdateCheckFinished;
        public event ConnectionsUpdateAvailableEventHandler ConnectionsUpdateAvailable;


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void SetEventListeners()
        {
            _updateChecker.UpdateCheckStarted += OnUpdateCheckStarted;
            _updateChecker.UpdateCheckFinished += OnUpdateCheckFinished;
            _updateChecker.ConnectionsUpdateAvailable +=
                (sender, args) => ConnectionsUpdateAvailable?.Invoke(sender, args);
            _updateTimer.Elapsed += (sender, args) => _updateChecker.IsUpdateAvailableAsync();
            ConnectionsUpdateAvailable += Load;
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

        public void Save()
        {
            _connectionsSaver.SaveConnections();
        }

        public void Enable() => _updateTimer.Start();
        public void Disable() => _updateTimer.Stop();


        private void OnUpdateCheckStarted(object sender, EventArgs eventArgs)
        {
            _updateTimer.Stop();
            UpdateCheckStarted?.Invoke(sender, eventArgs);
        }

        private void OnUpdateCheckFinished(object sender, ConnectionsUpdateCheckFinishedEventArgs eventArgs)
        {
            _updateTimer.Start();
            UpdateCheckFinished?.Invoke(sender, eventArgs);
        }

        private void Dispose(bool itIsSafeToAlsoFreeManagedObjects)
        {
            if (!itIsSafeToAlsoFreeManagedObjects) return;
            _updateTimer.Dispose();
            _updateChecker.Dispose();
        }
    }
}