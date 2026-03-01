using System;
using System.Data.Common;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Sql;
using mRemoteNG.Tree.Root;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.ConnectionSerializers.Sql
{
    [TestFixture]
    public class SqlDatabaseMetaDataRetrieverTests
    {
        private SqlDatabaseMetaDataRetriever _retriever;
        private IDatabaseConnector _mockConnector;
        private DbConnection _mockConnection;
        private DbCommand _mockCommand;
        private DbTransaction _mockTransaction;
        private DbParameterCollection _mockParameterCollection;

        [SetUp]
        public void Setup()
        {
            _retriever = new SqlDatabaseMetaDataRetriever();
            _mockConnector = Substitute.For<IDatabaseConnector>();
            _mockConnection = Substitute.For<DbConnection>();
            _mockCommand = Substitute.For<DbCommand>();
            _mockTransaction = Substitute.For<DbTransaction>();
            _mockParameterCollection = Substitute.For<DbParameterCollection>();

            _mockConnector.DbConnection().Returns(_mockConnection);
            _mockConnector.DbCommand(Arg.Any<string>()).Returns(_mockCommand);
            _mockConnection.BeginTransaction().Returns(_mockTransaction);
            _mockCommand.CreateParameter().Returns(_ => Substitute.For<DbParameter>());
            _mockCommand.Parameters.Returns(_mockParameterCollection);
        }

        [Test]
        public void WriteDatabaseMetaData_WithExplicitTransaction_UsesProvidedTransaction()
        {
            var rootNode = new RootNodeInfo(RootNodeType.Connection);
            var explicitTransaction = Substitute.For<DbTransaction>();

            _retriever.WriteDatabaseMetaData(rootNode, _mockConnector, explicitTransaction);

            // Verify that the command was associated with the explicit transaction
            Assert.That(_mockCommand.Transaction, Is.EqualTo(explicitTransaction));
            
            // Verify that Commit was NOT called on the explicit transaction by the retriever
            // (it should be committed by the caller)
            explicitTransaction.DidNotReceive().Commit();
            
            // Verify that the connector's connection was NOT used to begin a new transaction
            _mockConnection.DidNotReceive().BeginTransaction();
        }

        [Test]
        public void WriteDatabaseMetaData_WithoutExplicitTransaction_CreatesAndCommitsTransaction()
        {
            var rootNode = new RootNodeInfo(RootNodeType.Connection);

            _retriever.WriteDatabaseMetaData(rootNode, _mockConnector, null);

            // Verify that a new transaction was started
            _mockConnection.Received(1).BeginTransaction();
            
            // Verify that the command was associated with the new transaction
            Assert.That(_mockCommand.Transaction, Is.EqualTo(_mockTransaction));
            
            // Verify that Commit was called
            _mockTransaction.Received(1).Commit();
        }

        [Test]
        public void WriteDatabaseMetaData_OnFailure_RollsBackCreatedTransaction()
        {
            var rootNode = new RootNodeInfo(RootNodeType.Connection);
            _mockCommand.When(x => x.ExecuteNonQuery()).Do(x => throw new Exception("DB Error"));

            Assert.Throws<Exception>(() => _retriever.WriteDatabaseMetaData(rootNode, _mockConnector, null));

            // Verify that a new transaction was started
            _mockConnection.Received(1).BeginTransaction();
            
            // Verify that Rollback was called instead of Commit
            _mockTransaction.Received(1).Rollback();
            _mockTransaction.DidNotReceive().Commit();
        }
    }
}
