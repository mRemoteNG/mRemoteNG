using System;
using System.Diagnostics;
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
            _updateTimer.Elapsed += (sender, args) => tickdebug();
            _updateChecker.UpdateCheckStarted += UpdateCheckerOnUpdateCheckStarted;
            _updateChecker.UpdateCheckFinished += UpdateCheckerOnUpdateCheckFinished;
            _updateChecker.ConnectionsUpdateAvailable += (sender, args) => ConnectionsUpdateAvailable?.Invoke(sender, args);
        }

        private void tickdebug()
        {
            Debug.WriteLine("update_tick");
        }

        public void Enable() => _updateTimer.Start();

        public void Disable() => _updateTimer.Stop();

        public bool IsUpdateAvailable() => _updateChecker.IsUpdateAvailable();

        public void IsUpdateAvailableAsync() => _updateChecker.IsUpdateAvailableAsync();

        private void UpdateCheckerOnUpdateCheckStarted(object sender, EventArgs eventArgs)
        {
            _updateTimer.Stop();
            UpdateCheckStarted?.Invoke(sender, eventArgs);
        }

        private void UpdateCheckerOnUpdateCheckFinished(object sender, ConnectionsUpdateCheckFinishedEventArgs eventArgs)
        {
            _updateTimer.Start();
            UpdateCheckFinished?.Invoke(sender, eventArgs);
        }

        public event EventHandler UpdateCheckStarted;
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