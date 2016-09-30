using System;
using System.Timers;


namespace mRemoteNG.Config.Connections
{
    public class PeriodicConnectionsUpdateChecker : IConnectionsUpdateChecker
    {
        private readonly Timer _updateTimer;
        private readonly IConnectionsUpdateChecker _updateChecker;

        public double TimerIntervalInMilliseconds => _updateTimer.Interval;


        public PeriodicConnectionsUpdateChecker(IConnectionsUpdateChecker updateChecker)
        {
            _updateTimer = new Timer(3000);
            _updateChecker = updateChecker;
            _updateTimer.Elapsed += (sender, args) => _updateChecker.IsUpdateAvailableAsync();
            _updateChecker.ConnectionsUpdateAvailable += (sender, args) => ConnectionsUpdateAvailable?.Invoke(sender, args);
            _updateChecker.UpdateCheckFinished += (sender, args) => UpdateCheckFinished?.Invoke(sender, args);
        }

        public void Enable() => _updateTimer.Start();

        public void Disable() => _updateTimer.Stop();

        public bool IsUpdateAvailable() => _updateChecker.IsUpdateAvailable();

        public void IsUpdateAvailableAsync() => _updateChecker.IsUpdateAvailableAsync();

        public event UpdateCheckFinishedEventHandler UpdateCheckFinished;
        public event ConnectionsUpdateAvailableEventHandler ConnectionsUpdateAvailable;

        ~PeriodicConnectionsUpdateChecker()
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
            _updateTimer.Dispose();
            _updateChecker.Dispose();
        }
    }
}