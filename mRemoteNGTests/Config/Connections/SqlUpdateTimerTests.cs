using mRemoteNG.Config.Connections;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Connections
{
    [TestFixture]
    public class SqlUpdateTimerTests
    {
        private SqlUpdateTimer sqlUpdateChecker;

        [SetUp]
        public void SetupSqlUpdateChecker()
        {
            sqlUpdateChecker = new SqlUpdateTimer();
        }

        [TearDown]
        public void TearDownSqlUpdateChecker()
        {
            sqlUpdateChecker.Dispose();
            sqlUpdateChecker = null;
        }

        [Test]
        public void EnableSQLUpdating()
        {
            sqlUpdateChecker.Enable();
            Assert.AreEqual(true, sqlUpdateChecker.IsUpdateCheckingEnabled());
        }

        [Test]
        public void DisableSQLUpdating()
        {
            sqlUpdateChecker.Enable();
            Assert.AreEqual(true, sqlUpdateChecker.IsUpdateCheckingEnabled());
            sqlUpdateChecker.Disable();
            Assert.AreEqual(false, sqlUpdateChecker.IsUpdateCheckingEnabled());
        }
    }
}