using System;
using mRemoteNG.Config.Connections.Multiuser;
using mRemoteNG.Config.DatabaseConnectors;
using NSubstitute;
using NUnit.Framework;

// ReSharper disable ObjectCreationAsStatement

namespace mRemoteNGTests.Config.Connections.Multiuser;

public class ConnectionsUpdateAvailableEventArgsTests
{
    private IDatabaseConnector _databaseConnector;
    private DateTime _dateTime;

    [SetUp]
    public void Setup()
    {
        _databaseConnector = Substitute.For<IDatabaseConnector>();
        _dateTime = DateTime.MinValue;
    }

    [Test]
    public void CanProvideNullDatabaseConnectorToCtor()
    {
        var eventArgs = new ConnectionsUpdateAvailableEventArgs(null, _dateTime);
        Assert.That(eventArgs.DatabaseConnector, Is.Null);
    }

    [Test]
    public void DatabaseConnectorPropertySet()
    {
        var eventArgs = new ConnectionsUpdateAvailableEventArgs(_databaseConnector, _dateTime);
        Assert.That(eventArgs.DatabaseConnector, Is.EqualTo(_databaseConnector));
    }

    [Test]
    public void UpdateTimePropertySet()
    {
        var eventArgs = new ConnectionsUpdateAvailableEventArgs(_databaseConnector, _dateTime);
        Assert.That(eventArgs.UpdateTime, Is.EqualTo(_dateTime));
    }
}