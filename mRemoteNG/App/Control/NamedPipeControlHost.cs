using System;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace mRemoteNG.App.AgentControl
{
    /// <summary>Session-only local transport. The pipe is current-user-only and never starts implicitly.</summary>
    internal sealed class NamedPipeControlHost : IAsyncDisposable
    {
        private readonly string _pipeName;
        private readonly ControlRequestProcessor _processor;
        private readonly CancellationTokenSource _shutdown = new();
        private Task? _loop;

        public NamedPipeControlHost(string pipeName, ControlRequestProcessor processor)
        {
            if (string.IsNullOrWhiteSpace(pipeName) || pipeName.Length > 200)
                throw new ArgumentException("A bounded pipe name is required.", nameof(pipeName));
            _pipeName = pipeName;
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        public bool IsRunning => _loop is { IsCompleted: false };
        internal string LastTransportFailureType { get; private set; } = string.Empty;

        public void Start()
        {
            if (_loop != null) throw new InvalidOperationException("The control host has already been started.");
            _loop = Task.Run(() => ServeAsync(_shutdown.Token), _shutdown.Token);
        }

        public async ValueTask DisposeAsync()
        {
            await _shutdown.CancelAsync().ConfigureAwait(false);
            if (_loop != null)
            {
                try { await _loop.ConfigureAwait(false); }
                catch (OperationCanceledException)
                {
                    // Cancellation is the expected completion path when stopping the listener.
                }
            }
            _shutdown.Dispose();
        }

        private async Task ServeAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await using NamedPipeServerStream pipe = new(_pipeName, PipeDirection.InOut, 1,
                    PipeTransmissionMode.Byte, PipeOptions.Asynchronous | PipeOptions.CurrentUserOnly,
                    ControlSchema.MaximumRequestBytes, ControlSchema.MaximumResponseBytes);
                await pipe.WaitForConnectionAsync(cancellationToken).ConfigureAwait(false);

                using CancellationTokenSource timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                timeout.CancelAfter(TimeSpan.FromSeconds(3));
                try
                {
                    string clientSid = GetClientSid(pipe);
                    byte[] request = await ReadRequestAsync(pipe, timeout.Token).ConfigureAwait(false);
                    byte[] response = _processor.Process(request, clientSid, DateTime.UtcNow);
                    await pipe.WriteAsync(response, timeout.Token).ConfigureAwait(false);
                    await pipe.WriteAsync(new byte[] { (byte)'\n' }, timeout.Token).ConfigureAwait(false);
                    await pipe.FlushAsync(timeout.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
                {
                    // A client that does not send one bounded request in time is dropped.
                }
                catch (IOException)
                {
                    LastTransportFailureType = "IOException";
                    // Disconnects are expected; do not log raw transport data.
                }
                catch (UnauthorizedAccessException)
                {
                    LastTransportFailureType = "UnauthorizedAccessException";
                    // No data from an untrusted pipe client is logged or returned.
                }
                catch (Exception ex)
                {
                    LastTransportFailureType = ex.GetType().Name;
                    // A malformed or short-lived local client must not stop the listener or leak details.
                }
            }
        }

        private static string GetClientSid(NamedPipeServerStream pipe)
        {
            // PipeOptions.CurrentUserOnly has already enforced the caller SID at connection time.
            // Bind the expiring grant to that SID and the caller's Windows logon session.
            string sid = WindowsIdentity.GetCurrent()?.User?.Value;
            if (string.IsNullOrWhiteSpace(sid) || !GetNamedPipeClientProcessId(pipe.SafePipeHandle, out uint processId))
                throw new UnauthorizedAccessException();
            try
            {
                int sessionId = System.Diagnostics.Process.GetProcessById(checked((int)processId)).SessionId;
                return $"{sid}|{sessionId}";
            }
            catch
            {
                throw new UnauthorizedAccessException();
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetNamedPipeClientProcessId(Microsoft.Win32.SafeHandles.SafePipeHandle pipe, out uint clientProcessId);

        private static async Task<byte[]> ReadRequestAsync(Stream pipe, CancellationToken cancellationToken)
        {
            using MemoryStream buffer = new();
            byte[] oneByte = new byte[1];
            while (buffer.Length <= ControlSchema.MaximumRequestBytes)
            {
                int count = await pipe.ReadAsync(oneByte, cancellationToken).ConfigureAwait(false);
                if (count == 0 || oneByte[0] == (byte)'\n')
                    return buffer.ToArray();
                if (oneByte[0] != (byte)'\r')
                    buffer.WriteByte(oneByte[0]);
            }
            return [];
        }
    }
}
