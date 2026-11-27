using System;
using System.Threading;

namespace mRemoteNG.Config.Import;

internal static class ImportCancellationContext
{
    private static readonly AsyncLocal<CancellationToken> CurrentToken = new();

    public static CancellationToken Current => CurrentToken.Value;

    public static IDisposable Begin(CancellationToken token)
    {
        CancellationToken previous = CurrentToken.Value;
        CurrentToken.Value = token;
        return new Scope(previous);
    }

    private sealed class Scope(CancellationToken previous) : IDisposable
    {
        private bool _disposed;

        public void Dispose()
        {
            if (_disposed) return;
            CurrentToken.Value = previous;
            _disposed = true;
        }
    }
}
