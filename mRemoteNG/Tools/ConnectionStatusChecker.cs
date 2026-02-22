using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Properties;
using mRemoteNG.Tree;

namespace mRemoteNG.Tools
{
    public class ConnectionStatusChecker : IDisposable
    {
        private Timer _timer;
        private ConnectionTreeModel _model;
        private bool _disposed;
        private const int CheckIntervalMs = 30000;
        private const int PingTimeoutMs = 5000;
        private const int MaxConcurrentChecks = 10;
        private readonly SemaphoreSlim _throttle = new(MaxConcurrentChecks, MaxConcurrentChecks);

        public ConnectionStatusChecker(ConnectionTreeModel model)
        {
            _model = model;
            _timer = new Timer(CheckAllConnections, null, 0, CheckIntervalMs);
        }

        public void UpdateModel(ConnectionTreeModel model)
        {
            _model = model;
        }

        private void CheckAllConnections(object state)
        {
            if (!OptionsAppearancePage.Default.ShowStatusIndicatorInTree)
                return;

            if (_model == null)
                return;

            var connections = GetAllConnections(_model);
            foreach (var connection in connections)
            {
                Task.Run(() => CheckConnectionStatusThrottled(connection));
            }
        }

        private async Task CheckConnectionStatusThrottled(ConnectionInfo connection)
        {
            await _throttle.WaitAsync().ConfigureAwait(false);
            try
            {
                CheckConnectionStatus(connection);
            }
            finally
            {
                _throttle.Release();
            }
        }

        private static void CheckConnectionStatus(ConnectionInfo connection)
        {
            if (string.IsNullOrEmpty(connection.Hostname))
            {
                connection.HostStatus = HostStatus.Unknown;
                return;
            }

            try
            {
                using var ping = new Ping();
                PingReply reply = ping.Send(connection.Hostname, PingTimeoutMs);
                connection.HostStatus = reply?.Status == IPStatus.Success
                    ? HostStatus.Online
                    : HostStatus.Offline;
            }
            catch (PingException)
            {
                connection.HostStatus = HostStatus.Offline;
            }
            catch (Exception)
            {
                connection.HostStatus = HostStatus.Unknown;
            }
        }

        private static IEnumerable<ConnectionInfo> GetAllConnections(ConnectionTreeModel model)
        {
            var result = new List<ConnectionInfo>();
            foreach (var root in model.RootNodes)
            {
                CollectConnections(root, result);
            }

            return result;
        }

        private static void CollectConnections(ConnectionInfo node, List<ConnectionInfo> result)
        {
            if (node is ContainerInfo container)
            {
                foreach (var child in container.Children)
                {
                    CollectConnections(child, result);
                }
            }
            else
            {
                result.Add(node);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                _timer?.Dispose();
                _timer = null;
                _throttle?.Dispose();
            }

            _disposed = true;
        }
    }
}
