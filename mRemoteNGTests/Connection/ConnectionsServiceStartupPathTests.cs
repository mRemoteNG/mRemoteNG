using mRemoteNG.Config.Putty;
using mRemoteNG.Connection;
using mRemoteNG.Properties;
using NUnit.Framework;

namespace mRemoteNGTests.Connection;

[NonParallelizable]
public class ConnectionsServiceStartupPathTests
{
    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    public void StartupConnectionPathFallsBackToDefaultWhenConfiguredPathIsMissing(string configuredPath)
    {
        var connectionsService = new ConnectionsService(PuttySessionsManager.Instance);
        var originalPath = OptionsConnectionsPage.Default.ConnectionFilePath;
        try
        {
            OptionsConnectionsPage.Default.ConnectionFilePath = configuredPath;

            var startupPath = connectionsService.GetStartupConnectionFileName();
            var defaultPath = connectionsService.GetDefaultStartupConnectionFileName();

            Assert.That(startupPath, Is.EqualTo(defaultPath));
        }
        finally
        {
            OptionsConnectionsPage.Default.ConnectionFilePath = originalPath;
        }
    }
}
