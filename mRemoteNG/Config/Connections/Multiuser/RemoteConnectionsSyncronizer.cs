using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Properties;
using System;
using System.Runtime.Versioning;
using System.Timers;

// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.Config.Connections.Multiuser
{
    [SupportedOSPlatform("windows")]
    public class RemoteConnectionsSyncronizer : IConnectionsUpdateChecker
    {
        private readonly System.Timers.Timer _updateTimer;
        private readonly IConnectionsUpdateChecker _updateChecker;

        public double TimerIntervalInMilliseconds
        {
            get { return _updateTimer.Interval; }
        }

        /// <summary>
        /// Gets the UTC time of the last successful external sync, or null if no sync has occurred yet.
        /// </summary>
        public DateTime? LastExternalSync { get; private set; }

        /// <summary>
        /// Raised when connections have been reloaded due to an external change (file or database).
        /// </summary>
        public event EventHandler? ConnectionsReloadedExternally;

        public RemoteConnectionsSyncronizer(IConnectionsUpdateChecker updateChecker)
        {
            _updateChecker = updateChecker;
            double intervalMs = OptionsDBsPage.Default.SQLReloadInterval * 1000.0;
            _updateTimer = new System.Timers.Timer(intervalMs > 0 ? intervalMs : 30000.0);
            SetEventListeners();
        }

        private void SetEventListeners()
        {
            _updateChecker.UpdateCheckStarted += OnUpdateCheckStarted;
            _updateChecker.UpdateCheckFinished += OnUpdateCheckFinished;
            _updateChecker.ConnectionsUpdateAvailable += (_, args) => ConnectionsUpdateAvailable?.Invoke(this, args);
            _updateTimer.Elapsed += (sender, args) => _updateChecker.IsUpdateAvailableAsync();
            ConnectionsUpdateAvailable += Load;
        }

        private void Load(object sender, ConnectionsUpdateAvailableEventArgs args)
        {
            if (args.DatabaseConnector != null)
            {
                Runtime.ConnectionsService.LoadConnections(true, false, "");
            }
            else
            {
                if (Runtime.ConnectionsService.ConnectionFileName != null)
                    Runtime.ConnectionsService.LoadConnections(false, false, Runtime.ConnectionsService.ConnectionFileName);
            }
            args.Handled = true;

            LastExternalSync = DateTime.UtcNow;
            string source = args.DatabaseConnector != null ? "database" : "file";
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                $"Connections reloaded from external {source} change (team sync)");
            ConnectionsReloadedExternally?.Invoke(this, EventArgs.Empty);
        }

        public void Enable()
        {
            _updateTimer.Start();
        }

        public void Disable()
        {
            _updateTimer.Stop();
        }

        public bool IsUpdateAvailable()
        {
            return _updateChecker.IsUpdateAvailable();
        }

        public void IsUpdateAvailableAsync()
        {
            _updateChecker.IsUpdateAvailableAsync();
        }


        private void OnUpdateCheckStarted(object sender, EventArgs eventArgs)
        {
            _updateTimer.Stop();
            UpdateCheckStarted?.Invoke(this, eventArgs);
        }

        private void OnUpdateCheckFinished(object sender, ConnectionsUpdateCheckFinishedEventArgs eventArgs)
        {
            _updateTimer.Start();
            UpdateCheckFinished?.Invoke(this, eventArgs);
        }

        public event EventHandler? UpdateCheckStarted;
        public event UpdateCheckFinishedEventHandler? UpdateCheckFinished;
        public event ConnectionsUpdateAvailableEventHandler? ConnectionsUpdateAvailable;


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