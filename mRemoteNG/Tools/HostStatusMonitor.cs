using System;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using mRemoteNG.Connection;
using mRemoteNG.Tree;

namespace mRemoteNG.Tools
{
    /// <summary>
    /// Background service that periodically probes each connection's host port and updates
    /// <see cref="ConnectionInfo.HostReachabilityStatus"/> so the connection tree can show
    /// online/offline status overlays (issue #1109).
    ///
    /// Usage:
    ///   var monitor = new HostStatusMonitor(model);
    ///   monitor.Start();   // begins background scanning
    ///   monitor.Stop();    // cancel (also called by Dispose)
    /// </summary>
    [SupportedOSPlatform("windows")]
    public sealed class HostStatusMonitor : IDisposable
    {
        private readonly ConnectionTreeModel _model;
        private CancellationTokenSource? _cts;

        /// <summary>How long to wait between full scan cycles (default 30 s).</summary>
        public int CheckIntervalSeconds { get; set; } = 30;

        /// <summary>TCP connect timeout per individual host (default 1 000 ms).</summary>
        public int CheckTimeoutMilliseconds { get; set; } = 1000;

        /// <summary>Delay between successive host checks to avoid network bursts (default 50 ms).</summary>
        public int StaggerDelayMilliseconds { get; set; } = 50;

        public HostStatusMonitor(ConnectionTreeModel model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
        }

        /// <summary>Start (or restart) the background monitoring loop.</summary>
        public void Start()
        {
            Stop();
            _cts = new CancellationTokenSource();
            Task.Run(() => RunAsync(_cts.Token), _cts.Token);
        }

        /// <summary>Stop the background monitoring loop.</summary>
        public void Stop()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        public void Dispose() => Stop();

        private async Task RunAsync(CancellationToken ct)
        {
            // Small initial delay so the UI settles before the first scan starts.
            try { await Task.Delay(5000, ct).ConfigureAwait(false); }
            catch (OperationCanceledException) { return; }

            while (!ct.IsCancellationRequested)
            {
                try
                {
                    await CheckAllHostsAsync(ct).ConfigureAwait(false);
                    await Task.Delay(TimeSpan.FromSeconds(CheckIntervalSeconds), ct).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch
                {
                    // Swallow unexpected errors — monitoring must not crash the app.
                    try { await Task.Delay(5000, ct).ConfigureAwait(false); } catch { break; }
                }
            }
        }

        private async Task CheckAllHostsAsync(CancellationToken ct)
        {
            var connections = _model.GetRecursiveChildList()
                .Where(c => !c.IsContainer
                         && !string.IsNullOrWhiteSpace(c.Hostname))
                .ToList();

            foreach (var connection in connections)
            {
                if (ct.IsCancellationRequested) break;

                int port = connection.Port > 0 ? connection.Port : connection.GetDefaultPort();
                bool reachable = await IsReachableAsync(connection.Hostname, port, CheckTimeoutMilliseconds, ct)
                    .ConfigureAwait(false);

                connection.HostReachabilityStatus = reachable
                    ? HostReachabilityStatus.Reachable
                    : HostReachabilityStatus.Unreachable;

                if (StaggerDelayMilliseconds > 0)
                {
                    try { await Task.Delay(StaggerDelayMilliseconds, ct).ConfigureAwait(false); }
                    catch (OperationCanceledException) { break; }
                }
            }
        }

        private static async Task<bool> IsReachableAsync(string hostname, int port, int timeoutMs, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(hostname) || port <= 0)
                return false;

            try
            {
                using var tcpClient = new TcpClient();
                using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
                timeoutCts.CancelAfter(timeoutMs);
                await tcpClient.ConnectAsync(hostname, port, timeoutCts.Token).ConfigureAwait(false);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
