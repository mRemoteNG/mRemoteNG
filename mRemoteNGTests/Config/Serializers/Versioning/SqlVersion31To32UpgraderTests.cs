using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Config.Serializers.Versioning;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.Versioning;

public class SqlVersion31To32UpgraderTests
{
    [Test]
    public void CanUpgradeIfVersionIs31()
    {
        var sqlConnector = Substitute.For<MSSqlDatabaseConnector>("", "", "", "");
        var versionUpgrader = new SqlVersion31To32Upgrader(sqlConnector);

        var canUpgrade = versionUpgrader.CanUpgrade(new Version(3, 1));

        Assert.That(canUpgrade, Is.True);
    }

    [Test]
    public void Upgrade_ConvertsTextColumnsToNvarchar_ForUnicodeRoundTrip()
    {
        var transaction = Substitute.For<DbTransaction>();
        var connection = new FakeDbConnection(transaction);

        DbCommand CreateCommand(string _)
        {
            var command = Substitute.For<DbCommand>();
            command.Parameters.Returns(Substitute.For<DbParameterCollection>());
            command.CreateParameter().Returns(Substitute.For<DbParameter>());
            command.ExecuteNonQuery().Returns(1);
            return command;
        }

        var connector = new TestMssqlDatabaseConnector(connection, CreateCommand);
        var versionUpgrader = new SqlVersion31To32Upgrader(connector);

        var upgradedVersion = versionUpgrader.Upgrade();

        Assert.That(upgradedVersion, Is.EqualTo(new Version(3, 2)));
        Assert.That(connector.CommandTexts, Has.Count.EqualTo(2));
        Assert.That(connector.CommandTexts[0], Does.Contain("ALTER TABLE tblCons ALTER COLUMN [Name] nvarchar(128) NOT NULL;"));
        Assert.That(connector.CommandTexts[0], Does.Contain("ALTER TABLE tblRoot ALTER COLUMN [Protected] nvarchar(4048) NOT NULL;"));
        Assert.That(connector.CommandTexts[0], Does.Contain("ALTER TABLE tblExternalTools ALTER COLUMN [Arguments] nvarchar(2048) NOT NULL;"));
        Assert.That(connector.CommandTexts[1], Does.Contain("UPDATE tblRoot SET ConfVersion=@confVersion;"));
        transaction.Received(1).Commit();
    }

    private sealed class TestMssqlDatabaseConnector(DbConnection connection, Func<string, DbCommand> commandFactory)
        : MSSqlDatabaseConnector("localhost", "mremoteng", "user", "password"), IDatabaseConnector
    {
        public List<string> CommandTexts { get; } = [];

        DbConnection IDatabaseConnector.DbConnection()
        {
            return connection;
        }

        DbCommand IDatabaseConnector.DbCommand(string dbCommand)
        {
            CommandTexts.Add(dbCommand);
            return commandFactory(dbCommand);
        }

        bool IDatabaseConnector.IsConnected => true;

        void IDatabaseConnector.Connect()
        {
        }

        Task IDatabaseConnector.ConnectAsync()
        {
            return Task.CompletedTask;
        }

        void IDatabaseConnector.Disconnect()
        {
        }

        void IDatabaseConnector.AssociateItemToThisConnector(DbCommand dbCommand)
        {
        }

        void IDisposable.Dispose()
        {
        }
    }

    private sealed class FakeDbConnection(DbTransaction transaction) : DbConnection
    {
        public override string ConnectionString { get; set; } = string.Empty;

        public override string Database => "mremoteng";

        public override string DataSource => "localhost";

        public override string ServerVersion => "1.0";

        public override ConnectionState State => ConnectionState.Open;

        public override void ChangeDatabase(string databaseName)
        {
        }

        public override void Close()
        {
        }

        public override void Open()
        {
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return transaction;
        }

        protected override DbCommand CreateDbCommand()
        {
            throw new NotSupportedException();
        }
    }
}
