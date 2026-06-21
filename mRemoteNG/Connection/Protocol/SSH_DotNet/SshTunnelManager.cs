// Design Note: Generic catch clauses (catch Exception) are used intentionally in this file.
// Port forwarding cleanup must be resilient - an exception stopping one port must not
// prevent cleanup of remaining ports. All exceptions are logged via SshDotNetDiagnostics.
using System;
using System.Collections.Generic;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace mRemoteNG.Connection.Protocol.SSH_DotNet
{
    /// <summary>
    /// Manages SSH port forwarding (local, remote, dynamic) on an SshClient.
    /// Handles setup, monitoring, and cleanup of forwarded ports.
    /// Thread-safe: all public methods use locking on _syncLock.
    /// </summary>
    public class SshTunnelManager : IDisposable
    {
        private readonly SshClient _sshClient;
        private readonly List<ForwardedPort> _forwardedPorts = new();
        private readonly object _syncLock = new();
        private bool _disposed;

        /// <summary>
        /// Raised when a forwarded port encounters an error.
        /// The string argument contains the error description.
        /// </summary>
        public event EventHandler<string> TunnelError;

        public SshTunnelManager(SshClient sshClient)
        {
            _sshClient = sshClient ?? throw new ArgumentNullException(nameof(sshClient));
        }

        public IReadOnlyList<ForwardedPort> ForwardedPorts
        {
            get { lock (_syncLock) { return _forwardedPorts.AsReadOnly(); } }
        }

        /// <summary>
        /// Set up local port forwarding: binds a local port that tunnels to remoteHost:remotePort.
        /// If localPort is 0, an available port is auto-selected.
        /// Returns the actual bound port.
        /// </summary>
        public uint AddLocalForward(string localBindHost, uint localPort,
                                     string remoteHost, uint remotePort)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            var forward = new ForwardedPortLocal(localBindHost, localPort, remoteHost, remotePort);
            forward.Exception += OnForwardException;
            forward.RequestReceived += OnForwardRequestReceived;

            _sshClient.AddForwardedPort(forward);
            forward.Start();

            lock (_syncLock) { _forwardedPorts.Add(forward); }

            SshDotNetDiagnostics.LogInfo(
                $"Tunnel: Local forward started - {localBindHost}:{forward.BoundPort} -> {remoteHost}:{remotePort}");

            return forward.BoundPort;
        }

        /// <summary>
        /// Set up remote port forwarding: binds a remote port that tunnels back to localHost:localPort.
        /// </summary>
        public void AddRemoteForward(string remoteBindHost, uint remotePort,
                                      string localHost, uint localPort)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            var forward = new ForwardedPortRemote(remoteBindHost, remotePort, localHost, localPort);
            forward.Exception += OnForwardException;
            forward.RequestReceived += OnForwardRequestReceived;

            _sshClient.AddForwardedPort(forward);
            forward.Start();

            lock (_syncLock) { _forwardedPorts.Add(forward); }

            SshDotNetDiagnostics.LogInfo(
                $"Tunnel: Remote forward started - {remoteBindHost}:{remotePort} -> {localHost}:{localPort}");
        }

        /// <summary>
        /// Set up dynamic SOCKS5 proxy on the specified local port.
        /// If localPort is 0, an available port is auto-selected.
        /// </summary>
        public uint AddDynamicForward(string localBindHost, uint localPort)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            var forward = new ForwardedPortDynamic(localBindHost, localPort);
            forward.Exception += OnForwardException;
            forward.RequestReceived += OnForwardRequestReceived;

            _sshClient.AddForwardedPort(forward);
            forward.Start();

            lock (_syncLock) { _forwardedPorts.Add(forward); }

            SshDotNetDiagnostics.LogInfo(
                $"Tunnel: Dynamic SOCKS5 proxy started on {localBindHost}:{forward.BoundPort}");

            return forward.BoundPort;
        }

        /// <summary>
        /// Stops and removes all forwarded ports. Unsubscribes events to prevent leaks.
        /// </summary>
        public void StopAll()
        {
            List<ForwardedPort> portsToStop;
            lock (_syncLock)
            {
                portsToStop = new List<ForwardedPort>(_forwardedPorts);
                _forwardedPorts.Clear();
            }

            foreach (var port in portsToStop)
            {
                try
                {
                    port.Exception -= OnForwardException;
                    port.RequestReceived -= OnForwardRequestReceived;

                    if (port.IsStarted)
                        port.Stop();

                    if (_sshClient.IsConnected)
                        _sshClient.RemoveForwardedPort(port);

                    port.Dispose();
                }
                catch (Exception ex)
                {
                    SshDotNetDiagnostics.LogException("Tunnel: Error stopping forwarded port", ex);
                }
            }
        }

        /// <summary>
        /// Check if all forwarded ports are still healthy.
        /// </summary>
        public bool AreAllPortsHealthy()
        {
            if (!_sshClient.IsConnected) return false;
            lock (_syncLock)
            {
                foreach (var port in _forwardedPorts)
                {
                    if (!port.IsStarted) return false;
                }
            }
            return true;
        }

        private void OnForwardException(object sender, ExceptionEventArgs e)
        {
            SshDotNetDiagnostics.LogException("Tunnel: Port forwarding error", e.Exception);
            TunnelError?.Invoke(this, $"Tunnel port forwarding failed: {e.Exception.Message}");
        }

        private void OnForwardRequestReceived(object sender, PortForwardEventArgs e)
        {
            SshDotNetDiagnostics.LogDebug(
                $"Tunnel: Request received - {e.OriginatorHost}:{e.OriginatorPort}");
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            StopAll();
        }
    }
}
