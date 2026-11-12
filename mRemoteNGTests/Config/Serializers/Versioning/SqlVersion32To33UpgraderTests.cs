using System;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Config.Serializers.Versioning;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.Versioning;

public class SqlVersion32To33UpgraderTests
{
    private SqlVersion32To33Upgrader _versionUpgrader;

    [SetUp]
    public void Setup()
    {
        var sqlConnector = Substitute.For<MSSqlDatabaseConnector>("", "", "", "");
        _versionUpgrader = new SqlVersion32To33Upgrader(sqlConnector);
    }

    [Test]
    public void CanUpgradeIfVersionIs32()
    {
        Assert.That(_versionUpgrader.CanUpgrade(new Version(3, 2)), Is.True);
    }

    [Test]
    public void CannotUpgradeIfVersionIs33()
    {
        Assert.That(_versionUpgrader.CanUpgrade(new Version(3, 3)), Is.False);
    }
}
