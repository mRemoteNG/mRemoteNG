using System;
using System.Threading;
using System.Threading.Tasks;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace mRemoteNG.Connection.Protocol.SshDotNet
{
    /// <summary>
    /// Abstraction over <see cref="SshClient"/> so the protocol's connection pipeline can be
    /// unit-tested with a fake (connect ok / auth-fail / timeout / cancellation) without a real
    /// SSH server. Production code uses <see cref="SshClientAdapter"/>.
    /// </summary>
    internal interface ISshClientAdapter : IDisposable
    {
        bool IsConnected { get; }

        /// <summary>The wrapped client, for components that still require the concrete type (e.g. the tunnel manager).</summary>
        SshClient UnderlyingClient { get; }

        event EventHandler<ExceptionEventArgs> ErrorOccurred;

        void Connect();
        Task ConnectAsync(CancellationToken cancellationToken);
        void ConfigureKeepAlive(TimeSpan? interval = null);
        ShellStream CreateShellStream(string terminalName, uint columns, uint rows, uint width, uint height, int bufferSize);
        string GetConnectionInfo();
        void Disconnect();
    }

    /// <summary>Production <see cref="ISshClientAdapter"/> backed by a real <see cref="SshClient"/>.</summary>
    internal sealed class SshClientAdapter : ISshClientAdapter
    {
        private readonly SshClient _client;

        public SshClientAdapter(SshClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public bool IsConnected => _client.IsConnected;

        public SshClient UnderlyingClient => _client;

        public event EventHandler<ExceptionEventArgs> ErrorOccurred
        {
            add => _client.ErrorOccurred += value;
            remove => _client.ErrorOccurred -= value;
        }

        public void Connect() => SshConnectionManager.Connect(_client);

        public Task ConnectAsync(CancellationToken cancellationToken) => _client.ConnectAsync(cancellationToken);

        public void ConfigureKeepAlive(TimeSpan? interval = null) => SshConnectionManager.ConfigureKeepAlive(_client, interval);

        public ShellStream CreateShellStream(string terminalName, uint columns, uint rows, uint width, uint height, int bufferSize)
            => SshConnectionManager.CreateShellStream(_client, terminalName, columns, rows, width, height, bufferSize);

        public string GetConnectionInfo() => SshConnectionManager.GetConnectionInfo(_client);

        public void Disconnect()
        {
            if (_client.IsConnected)
                _client.Disconnect();
        }

        public void Dispose() => _client.Dispose();
    }
}
