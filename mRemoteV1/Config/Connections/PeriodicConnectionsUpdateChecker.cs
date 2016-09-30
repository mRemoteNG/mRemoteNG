using mRemoteNG.App;
using mRemoteNG.Messages;
using System;

namespace mRemoteNG.Config.Connections
{
    public class PeriodicConnectionsUpdateChecker : IDisposable
    {
        private readonly SqlUpdateTimer _updateTimer;
        private readonly IConnectionsUpdateChecker _updateChecker;


        public PeriodicConnectionsUpdateChecker()
        {
            _updateTimer = new SqlUpdateTimer();
            _updateChecker = new SqlConnectionsUpdateChecker();
            SqlUpdateTimer.SqlUpdateTimerElapsed += SqlUpdateTimer_SqlUpdateTimerElapsed;
            _updateChecker.ConnectionsUpdateAvailable += OnConnectionsUpdateAvailable;
        }

        public void Enable()
        {
            _updateTimer.Enable();
        }

        public void Disable()
        {
            _updateTimer.Disable();
        }

        private void SqlUpdateTimer_SqlUpdateTimerElapsed()
        {
            _updateChecker.IsUpdateAvailableAsync();
        }

        private void OnConnectionsUpdateAvailable(object sender, ConnectionsUpdateAvailableEventArgs args)
        {
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.strSqlUpdateCheckUpdateAvailable, true);
            Runtime.LoadConnectionsBG();
        }


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
            DestroySQLUpdateHandlers();
            _updateTimer.Dispose();
            _updateChecker.Dispose();
        }

        private void DestroySQLUpdateHandlers()
        {
            SqlUpdateTimer.SqlUpdateTimerElapsed -= SqlUpdateTimer_SqlUpdateTimerElapsed;
            _updateChecker.ConnectionsUpdateAvailable -= OnConnectionsUpdateAvailable;
        }
    }
}