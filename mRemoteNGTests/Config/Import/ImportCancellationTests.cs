using System;
using System.Threading;
using mRemoteNG.Config.Import;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Import;

[TestFixture]
public class ImportCancellationTests
{
    [Test]
    public void InterfaceCancellation_StagesLegacyImporterAndLeavesDestinationUntouched()
    {
        ContainerInfo destination = new();
        TestImporter importer = new();
        using CancellationTokenSource cancellation = new();
        cancellation.Cancel();

        Assert.Throws<OperationCanceledException>(() => ((IConnectionImporter<string>)importer).Import("fixture", destination, cancellation.Token));

        Assert.That(destination.Children, Is.Empty);
        Assert.That(importer.WasInvoked, Is.False);
    }

    [Test]
    public void InterfaceCancellation_DuringLegacyImportLeavesDestinationUntouched()
    {
        ContainerInfo destination = new();
        using CancellationTokenSource cancellation = new();
        TestImporter importer = new(cancellation);

        Assert.Throws<OperationCanceledException>(() => ((IConnectionImporter<string>)importer).Import("fixture", destination, cancellation.Token));

        Assert.That(destination.Children, Is.Empty);
        Assert.That(importer.WasInvoked, Is.True);
    }

    [Test]
    public void StaticAdapterStaging_PreCanceled_DoesNotInvokeAdapterOrChangeDestination()
    {
        ContainerInfo destination = new();
        using CancellationTokenSource cancellation = new();
        cancellation.Cancel();
        bool adapterInvoked = false;

        Assert.Throws<OperationCanceledException>(() => ImportStager.Run(destination, cancellation.Token, staged =>
        {
            adapterInvoked = true;
            staged.AddChild(new ConnectionInfo { Name = "fixture" });
        }));

        Assert.That(adapterInvoked, Is.False);
        Assert.That(destination.Children, Is.Empty);
    }

    [Test]
    public void StaticAdapterStaging_CanceledDuringAdapter_DiscardsDetachedResults()
    {
        ContainerInfo destination = new();
        using CancellationTokenSource cancellation = new();

        Assert.Throws<OperationCanceledException>(() => ImportStager.Run(destination, cancellation.Token, staged =>
        {
            staged.AddChild(new ConnectionInfo { Name = "fixture" });
            cancellation.Cancel();
        }));

        Assert.That(destination.Children, Is.Empty);
    }

    private sealed class TestImporter(CancellationTokenSource cancellation = null) : IConnectionImporter<string>
    {
        public bool WasInvoked { get; private set; }

        public void Import(string source, ContainerInfo destinationContainer)
        {
            WasInvoked = true;
            destinationContainer.AddChild(new ConnectionInfo { Name = "fixture" });
            cancellation?.Cancel();
        }
    }
}
