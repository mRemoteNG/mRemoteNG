using System;
using System.IO;
using System.Threading;
using mRemoteNG.App;
using mRemoteNG.Messages;

namespace mRemoteNG.Config.Connections.Multiuser
{
    public class FileConnectionsUpdateChecker : IConnectionsUpdateChecker
    {
        private readonly FileSystemWatcher _watcher;
        private readonly string _connectionFilePath;
        private readonly System.Timers.Timer _debounceTimer;

        public FileConnectionsUpdateChecker(string connectionFilePath)
        {
            _connectionFilePath = connectionFilePath;
            _watcher = new FileSystemWatcher(Path.GetDirectoryName(connectionFilePath) ?? ".", Path.GetFileName(connectionFilePath));
            _watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime | NotifyFilters.Size;
            _watcher.Changed += OnFileChanged;
            _watcher.Created += OnFileChanged;
            _watcher.EnableRaisingEvents = true;

            _debounceTimer = new System.Timers.Timer(1000); // 1s debounce
            _debounceTimer.AutoReset = false;
            _debounceTimer.Elapsed += OnDebounceTimerElapsed;
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            _debounceTimer.Stop();
            _debounceTimer.Start();
        }

        private void OnDebounceTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Trigger update check
            // Since we are on a timer thread, we should probably invoke safely?
            // But IsUpdateAvailableAsync spins a thread anyway (in Sql implementation).
            // Here, we just want to signal that an update IS available.
            
            // However, the interface expects IsUpdateAvailableAsync to eventually fire UpdateCheckFinished.
            // But RemoteConnectionsSyncronizer calls IsUpdateAvailableAsync periodically.
            
            // If we want immediate updates, we should manually trigger the check.
            IsUpdateAvailableAsync();
        }

        public bool IsUpdateAvailable()
        {
            RaiseUpdateCheckStartedEvent();
            bool updateAvailable = CheckFileUpdate();
            if (updateAvailable)
                RaiseConnectionsUpdateAvailableEvent();
            RaiseUpdateCheckFinishedEvent(updateAvailable);
            return updateAvailable;
        }

        private bool CheckFileUpdate()
        {
            try
            {
                if (!File.Exists(_connectionFilePath))
                    return false;

                DateTime currentLastWrite = File.GetLastWriteTimeUtc(_connectionFilePath);
                
                // Truncate milliseconds as file systems precision varies and internal DateTime might handle it differently
                // Similar to SqlConnectionsUpdateChecker
                long currentTicks = currentLastWrite.Ticks - (currentLastWrite.Ticks % TimeSpan.TicksPerSecond);
                DateTime currentLastWriteNoMs = new DateTime(currentTicks, currentLastWrite.Kind);

                DateTime lastKnownUpdate = Runtime.ConnectionsService.LastFileUpdate.ToUniversalTime();
                long lastKnownTicks = lastKnownUpdate.Ticks - (lastKnownUpdate.Ticks % TimeSpan.TicksPerSecond);
                DateTime lastKnownUpdateNoMs = new DateTime(lastKnownTicks, lastKnownUpdate.Kind);

                return currentLastWriteNoMs > lastKnownUpdateNoMs;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, $"Error checking for file updates: {ex.Message}", true);
                return false;
            }
        }

        public void IsUpdateAvailableAsync()
        {
            Thread thread = new(() => IsUpdateAvailable());
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        public event EventHandler? UpdateCheckStarted;

        private void RaiseUpdateCheckStartedEvent()
        {
            UpdateCheckStarted?.Invoke(this, EventArgs.Empty);
        }

        public event UpdateCheckFinishedEventHandler? UpdateCheckFinished;

        private void RaiseUpdateCheckFinishedEvent(bool updateAvailable)
        {
            ConnectionsUpdateCheckFinishedEventArgs args = new() { UpdateAvailable = updateAvailable };
            UpdateCheckFinished?.Invoke(this, args);
        }

        public event ConnectionsUpdateAvailableEventHandler? ConnectionsUpdateAvailable;

        private void RaiseConnectionsUpdateAvailableEvent()
        {
            Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, "File connection update is available");
            DateTime lastWrite = File.GetLastWriteTimeUtc(_connectionFilePath);
            ConnectionsUpdateAvailableEventArgs args = new(null, lastWrite);
            ConnectionsUpdateAvailable?.Invoke(this, args);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _watcher.EnableRaisingEvents = false;
                _watcher.Dispose();
                _debounceTimer.Dispose();
            }
        }
    }
}
