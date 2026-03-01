using System.Reflection;
using mRemoteNG.Config.Putty;
using mRemoteNG.Connection;
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
        var optionsType = typeof(ConnectionsService).Assembly.GetType("mRemoteNG.Properties.OptionsConnectionsPage", throwOnError: true);
        var defaultProperty = optionsType!.GetProperty("Default", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        var settingsInstance = defaultProperty!.GetValue(null);
        var connectionFilePathProperty = optionsType.GetProperty("ConnectionFilePath", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        var originalPath = (string)connectionFilePathProperty!.GetValue(settingsInstance);
        try
        {
            connectionFilePathProperty.SetValue(settingsInstance, configuredPath);

            var startupPath = ConnectionsService.GetStartupConnectionFileName();
            var defaultPath = ConnectionsService.GetDefaultStartupConnectionFileName();

            Assert.That(startupPath, Is.EqualTo(defaultPath));
        }
        finally
        {
            connectionFilePathProperty.SetValue(settingsInstance, originalPath);
        }
    }

    [Test]
    public void StartupConnectionPathReturnsConfiguredPathWhenSet()
    {
        var connectionsService = new ConnectionsService(PuttySessionsManager.Instance);
        var optionsType = typeof(ConnectionsService).Assembly.GetType("mRemoteNG.Properties.OptionsConnectionsPage", throwOnError: true);
        var defaultProperty = optionsType!.GetProperty("Default", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        var settingsInstance = defaultProperty!.GetValue(null);
        var connectionFilePathProperty = optionsType.GetProperty("ConnectionFilePath", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        var originalPath = (string)connectionFilePathProperty!.GetValue(settingsInstance);
        try
        {
            const string customPath = @"C:\MyConnections\custom.xml";
            connectionFilePathProperty.SetValue(settingsInstance, customPath);

            var startupPath = ConnectionsService.GetStartupConnectionFileName();

            Assert.That(startupPath, Is.EqualTo(customPath));
        }
        finally
        {
            connectionFilePathProperty.SetValue(settingsInstance, originalPath);
        }
    }

    [Test]
    public void DefaultStartupConnectionFileNameIsNotNullOrEmpty()
    {
        var connectionsService = new ConnectionsService(PuttySessionsManager.Instance);

        var defaultPath = ConnectionsService.GetDefaultStartupConnectionFileName();

        Assert.That(defaultPath, Is.Not.Null.And.Not.Empty);
    }
}
