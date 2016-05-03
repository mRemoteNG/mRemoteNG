using System;
using NUnit.Framework;
using mRemoteNG.Config.Connections;
using System.Data.SqlClient;

namespace mRemoteNGTests.Config.Connections
{
    [TestFixture]
    public class SqlUpdateQueryBuilderTest
    {
        private SqlUpdateQueryBuilder _sqlUpdateQueryBuilder;

        [SetUp]
        public void Setup()
        {
            _sqlUpdateQueryBuilder = new SqlUpdateQueryBuilder();
        }

        [TearDown]
        public void Teardown()
        {
            _sqlUpdateQueryBuilder = null;
        }

        [Test]
        public void SqlUpdateQueryBuilderReturnsSomeCommand()
        {
            SqlCommand command = _sqlUpdateQueryBuilder.BuildCommand();
            Assert.AreNotEqual(command.CommandText, "");
        }
    }
}