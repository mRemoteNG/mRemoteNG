using System;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Config.Serializers.Versioning;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.Versioning;

public class SqlVersion31To32UpgraderTests
{
    private SqlVersion31To32Upgrader _versionUpgrader;

    [SetUp]
    public void Setup()
    {
        var sqlConnector = Substitute.For<MSSqlDatabaseConnector>("", "", "", "");
        _versionUpgrader = new SqlVersion31To32Upgrader(sqlConnector);
    }

    [Test]
    public void CanUpgradeIfVersionIs31()
    {
        Assert.That(_versionUpgrader.CanUpgrade(new Version(3, 1)), Is.True);
    }

    [Test]
    public void CannotUpgradeIfVersionIs32()
    {
        Assert.That(_versionUpgrader.CanUpgrade(new Version(3, 2)), Is.False);
    }
}
