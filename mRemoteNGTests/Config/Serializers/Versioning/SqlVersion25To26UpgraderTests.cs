using System;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Config.Serializers.Versioning;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.Versioning;

public class SqlVersion25To26UpgraderTests
{
    private SqlVersion25To26Upgrader _versionUpgrader;

    [SetUp]
    public void Setup()
    {
        var sqlConnector = Substitute.For<MSSqlDatabaseConnector>("", "", "", "");
        _versionUpgrader = new SqlVersion25To26Upgrader(sqlConnector);
    }

    [Test]
    public void CanUpgradeIfVersionIs25()
    {
        var currentVersion = new Version(2, 5);
        var canUpgrade = _versionUpgrader.CanUpgrade(currentVersion);
        Assert.That(canUpgrade, Is.True);
    }
}