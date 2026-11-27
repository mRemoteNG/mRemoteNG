using System;
using System.Linq;
using System.Threading;
using mRemoteNG.Container;

namespace mRemoteNG.Config.Import;

/// <summary>Runs legacy import adapters against a detached tree, then publishes all results at one cancellation boundary.</summary>
internal static class ImportStager
{
    public static void Run(ContainerInfo destination, CancellationToken cancellationToken, Action<ContainerInfo> import)
    {
        ArgumentNullException.ThrowIfNull(destination);
        ArgumentNullException.ThrowIfNull(import);
        cancellationToken.ThrowIfCancellationRequested();
        using IDisposable context = ImportCancellationContext.Begin(cancellationToken);
        ContainerInfo staged = new();
        import(staged);
        cancellationToken.ThrowIfCancellationRequested();
        destination.AddChildrenAtomically(staged.Children.ToArray(), cancellationToken);
    }
}
