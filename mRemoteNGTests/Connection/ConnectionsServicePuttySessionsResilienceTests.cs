using System.IO;
using mRemoteNG.Config.Putty;
using mRemoteNG.Connection;
using mRemoteNGTests.Properties;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.Connection;

[NonParallelizable]
public class ConnectionsServicePuttySessionsResilienceTests
{
    [Test]
    public void LoadConnectionsContinuesWhenPuttyProviderThrows()
    {
        using var _ = FileTestHelpers.DisposableTempFile(out var filePath, ".xml");
        File.WriteAllText(filePath, Resources.confCons_v2_6);

        var puttySessionsManager = PuttySessionsManager.Instance;
        var throwingProvider = new ThrowingPuttySessionsProvider();
        var connectionsService = new ConnectionsService(puttySessionsManager);

        puttySessionsManager.AddProvider(throwingProvider);
        try
        {
            Assert.DoesNotThrow(() => connectionsService.LoadConnections(useDatabase: false, import: false, connectionFileName: filePath));
            Assert.That(connectionsService.ConnectionTreeModel, Is.Not.Null);
            Assert.That(connectionsService.ConnectionTreeModel.RootNodes.Count, Is.GreaterThan(0));
        }
        finally
        {
            puttySessionsManager.RemoveProvider(throwingProvider);
        }
    }

    private sealed class ThrowingPuttySessionsProvider : AbstractPuttySessionsProvider
    {
        public override string[] GetSessionNames(bool raw = false) => new[] { "BrokenSession" };

        public override PuttySessionInfo GetSession(string sessionName) =>
            throw new FileNotFoundException("Simulated missing private key file.");
    }
}
