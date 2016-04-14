using System;
using NUnit.Framework;
using mRemoteNG.Config;
using mRemoteNG.Config.Connections;
using NSubstitute;

namespace mRemoteNGTests.Config.Connections
{
    [TestFixture]
    public class SqlConnectionUpdateCheckerTests
    {
        SqlConnectionsUpdateChecker _updateChecker;

        [SetUp]
        public void Setup()
        {
            _updateChecker = new SqlConnectionsUpdateChecker();
        }

        [TearDown]
        public void Teardown()
        {
            _updateChecker.Dispose();
            _updateChecker = null;
        }

        [Test]
        [Ignore("Need to find a way to mock SqlConnector")]
        public void ReturnTrueIfUpdateIsAvailable()
        {
            Substitute.For<SqlConnector>();
            bool updateIsAvailable = _updateChecker.IsDatabaseUpdateAvailable();
            Assert.AreEqual(true,updateIsAvailable);
        }

        [Test]
        [Ignore("Need to find a way to mock SqlConnector")]
        public void ReturnFalseIfUpdateIsNotAvailable()
        {
            bool updateIsAvailable = _updateChecker.IsDatabaseUpdateAvailable();
            Assert.AreEqual(false, updateIsAvailable);
        }
    }
}