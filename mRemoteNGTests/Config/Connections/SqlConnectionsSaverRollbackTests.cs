using System;
using System.Data.Common;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Sql;
using mRemoteNG.Connection;
using mRemoteNG.Tree.Root;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using mRemoteNG.Messages;
using mRemoteNG.App;

namespace mRemoteNGTests.Config.Connections
{
    [TestFixture]
    public class SqlDatabaseMetaDataRetrieverTests
    {
        private SqlDatabaseMetaDataRetriever _retriever;
        private IDatabaseConnector _dbConnector;
        private DbConnection _dbConnection;
        private DbTransaction _transaction;
        private DbCommand _dbCommand;

        [SetUp]
        public void Setup()
        {

            _retriever = new SqlDatabaseMetaDataRetriever();
            _dbConnector = Substitute.For<IDatabaseConnector>();
            _dbConnection = Substitute.For<DbConnection>();
            _transaction = Substitute.For<DbTransaction>();
            _dbCommand = Substitute.For<DbCommand>();
            var parameters = Substitute.For<DbParameterCollection>();
            var parameter = Substitute.For<DbParameter>();

            _dbConnector.DbConnection().Returns(_dbConnection);
            _dbConnector.DbCommand(Arg.Any<string>()).Returns(_dbCommand);
            _dbConnection.BeginTransaction().Returns(_transaction);
            _dbCommand.Parameters.Returns(parameters);
            _dbCommand.CreateParameter().Returns(parameter);
        }

        [Test]
        public void WriteDatabaseMetaData_OnFailure_RollsBackCreatedTransaction()
        {
            // Arrange
            var rootNode = new RootNodeInfo(RootNodeType.Connection);
            
            // Force an exception when ExecuteNonQuery is called
            _dbCommand.ExecuteNonQuery().Throws(new Exception("Database error"));

            // Act & Assert
            Assert.Throws<Exception>(() => _retriever.WriteDatabaseMetaData(rootNode, _dbConnector));
            
            // Verify rollback was called
            _transaction.Received(1).Rollback();
            _transaction.Received(1).Dispose();
        }

        [Test]
        public void WriteDatabaseMetaData_WithExplicitTransaction_UsesProvidedTransaction()
        {
            // Arrange
            var rootNode = new RootNodeInfo(RootNodeType.Connection);
            var externalTransaction = Substitute.For<DbTransaction>();

            // Act
            _retriever.WriteDatabaseMetaData(rootNode, _dbConnector, externalTransaction);

            // Assert
            _dbCommand.Received().Transaction = externalTransaction;
            externalTransaction.DidNotReceive().Commit();
            externalTransaction.DidNotReceive().Rollback();
            externalTransaction.DidNotReceive().Dispose();
        }

        [Test]
        public void WriteDatabaseMetaData_WithoutExplicitTransaction_CreatesAndCommitsTransaction()
        {
            // Arrange
            var rootNode = new RootNodeInfo(RootNodeType.Connection);

            // Act
            _retriever.WriteDatabaseMetaData(rootNode, _dbConnector);

            // Assert
            _dbConnection.Received(1).BeginTransaction();
            _transaction.Received(1).Commit();
            _transaction.Received(1).Dispose();
        }
    }
}