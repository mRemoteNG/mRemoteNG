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
    }
}