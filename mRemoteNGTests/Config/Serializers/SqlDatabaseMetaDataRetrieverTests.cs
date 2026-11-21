using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Sql;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tree.Root;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers;

/// <summary>
/// Regression cover for the table lookup that decides whether mRemoteNG's tables already exist.
/// Getting this wrong is destructive: a false "missing" answer makes the retriever re-initialize
/// the schema, which drops tblCons/tblRoot/tblUpdate and with them the user's connections.
/// </summary>
public class SqlDatabaseMetaDataRetrieverTests
{
    // On SQL Server the database name is table_catalog; table_schema holds the schema (dbo).
    // Matching table_schema against the database name never matched, so every load re-initialized
    // the schema and wiped the stored connections. See issue #3498.
    [Test]
    public void SqlServerLookupMatchesTheDatabaseOnCatalogNotSchema()
    {
        string query = SqlDatabaseMetaDataRetriever.BuildTableExistsQuery(
            new MSSqlDatabaseConnector("localhost", "someCatalog", "user", "password"));

        Assert.That(query, Does.Contain("table_catalog = @DatabaseName"));
        Assert.That(query, Does.Not.Contain("table_schema = @DatabaseName"));
    }

    // Without pinning the schema, a same-named table in another schema (audit.tblRoot) would be
    // mistaken for ours, skipping initialization and leaving the unqualified reads to fail.
    [Test]
    public void SqlServerLookupIsPinnedToTheSchemaWeCreate()
    {
        string query = SqlDatabaseMetaDataRetriever.BuildTableExistsQuery(
            new MSSqlDatabaseConnector("localhost", "someCatalog", "user", "password"));

        Assert.That(query, Does.Contain("table_schema = 'dbo'"));
    }

    // MySQL is the mirror image: table_schema is the database name and table_catalog is the
    // constant 'def', so matching the catalog there would never work.
    [Test]
    public void MySqlLookupMatchesTheDatabaseOnSchema()
    {
        string query = SqlDatabaseMetaDataRetriever.BuildTableExistsQuery(
            new MySqlDatabaseConnector("localhost", "someDatabase", "user", "password"));

        Assert.That(query, Does.Contain("table_schema = @DatabaseName"));
        Assert.That(query, Does.Not.Contain("table_catalog"));
    }

    // With the Database field left blank the server falls back to the login's default
    // database, so the setting names a database the connection is not in. Comparing against
    // it never matches, the table check reports our tables as missing, and the initializer
    // drops and recreates them - destroying the connections stored there.
    [Test]
    public void DatabaseNameComesFromTheOpenConnection()
    {
        var connector = new StubDatabaseConnector("TheDatabaseWeActuallyLandedIn");

        string resolved = SqlDatabaseMetaDataRetriever.ResolveDatabaseName(connector);

        Assert.That(resolved, Is.EqualTo("TheDatabaseWeActuallyLandedIn"));
    }

    [Test]
    public void DatabaseNameFallsBackToTheSettingWhenTheConnectionReportsNone()
    {
        mRemoteNG.Properties.OptionsDBsPage.Default.SQLDatabaseName = "FromSettings";
        var connector = new StubDatabaseConnector(string.Empty);

        string resolved = SqlDatabaseMetaDataRetriever.ResolveDatabaseName(connector);

        Assert.That(resolved, Is.EqualTo("FromSettings"));
    }

    private sealed class StubDatabaseConnector(string database) : IDatabaseConnector
    {
        private readonly SqlConnection _connection = new($"Server=unused;Database={database};");

        public DbConnection DbConnection() => _connection;
        public DbCommand DbCommand(string dbCommand) => _connection.CreateCommand();
        public bool IsConnected => false;
        public void Connect() { }
        public Task ConnectAsync() => Task.CompletedTask;
        public void Disconnect() { }
        public void AssociateItemToThisConnector(DbCommand dbCommand) { }
        public void Dispose() => _connection.Dispose();
    }

    [Test]
    public void BothLookupsFilterByTableName()
    {
        string sqlServerQuery = SqlDatabaseMetaDataRetriever.BuildTableExistsQuery(
            new MSSqlDatabaseConnector("localhost", "someCatalog", "user", "password"));
        string mySqlQuery = SqlDatabaseMetaDataRetriever.BuildTableExistsQuery(
            new MySqlDatabaseConnector("localhost", "someDatabase", "user", "password"));

        Assert.That(sqlServerQuery, Does.Contain("table_name = @TableName"));
        Assert.That(mySqlQuery, Does.Contain("table_name = @TableName"));
    }

    // The root row holds the salt of every stored password. Emptying the table before writing
    // the row again would lose the salt, and the passwords with it, if the write failed.
    [Test]
    public void SavingChangesTheRootRowInsteadOfEmptyingTheTable()
    {
        RecordingDatabaseConnector connector = new(rowsInRoot: 1);
        DerivedKeyCryptographyProvider provider = new(DerivedKeyCryptographyProvider.NewSalt(), 1000);

        SqlDatabaseMetaDataRetriever.WriteDatabaseMetaData(new RootNodeInfo(RootNodeType.Connection), connector, provider);

        Assert.Multiple(() =>
        {
            Assert.That(connector.Executed.Select(command => command.Text), Has.None.Contains("TRUNCATE"));
            Assert.That(connector.Executed.Last().Text, Does.Contain("UPDATE tblRoot SET"));
            Assert.That(connector.Executed.Last().Parameters["@KdfSalt"], Is.EqualTo(Convert.ToBase64String(provider.GetSalt())));
        });
    }

    [Test]
    public void NewDatabaseGetsItsRootRowInserted()
    {
        RecordingDatabaseConnector connector = new(rowsInRoot: 0);
        DerivedKeyCryptographyProvider provider = new(DerivedKeyCryptographyProvider.NewSalt(), 1000);

        SqlDatabaseMetaDataRetriever.WriteDatabaseMetaData(new RootNodeInfo(RootNodeType.Connection), connector, provider);

        Assert.Multiple(() =>
        {
            Assert.That(connector.Executed.Select(command => command.Text), Has.None.Contains("TRUNCATE"));
            Assert.That(connector.Executed.Last().Text, Does.StartWith("INSERT INTO tblRoot"));
            Assert.That(connector.Executed.Last().Parameters["@KdfSalt"], Is.EqualTo(Convert.ToBase64String(provider.GetSalt())));
        });
    }

    private sealed class RecordingDatabaseConnector(int rowsInRoot) : IDatabaseConnector
    {
        public List<(string Text, Dictionary<string, object> Parameters)> Executed { get; } = [];

        private int RowsInRoot { get; } = rowsInRoot;

        public DbConnection DbConnection() => null;
        public DbCommand DbCommand(string dbCommand) => new RecordingCommand(this, dbCommand);
        public bool IsConnected => true;
        public void Connect() { }
        public Task ConnectAsync() => Task.CompletedTask;
        public void Disconnect() { }
        public void AssociateItemToThisConnector(DbCommand dbCommand) { }
        public void Dispose() { }

        private sealed class RecordingCommand(RecordingDatabaseConnector connector, string text) : DbCommand
        {
            private readonly DbParameterCollection _parameters = new SqlCommand().Parameters;

            public override string CommandText { get; set; } = text;
            public override int CommandTimeout { get; set; }
            public override CommandType CommandType { get; set; }
            public override bool DesignTimeVisible { get; set; }
            public override UpdateRowSource UpdatedRowSource { get; set; }
            protected override DbConnection DbConnection { get; set; }
            protected override DbParameterCollection DbParameterCollection => _parameters;
            protected override DbTransaction DbTransaction { get; set; }

            public override void Cancel() { }
            public override void Prepare() { }
            protected override DbParameter CreateDbParameter() => new SqlParameter();
            protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior) => throw new NotSupportedException();

            public override int ExecuteNonQuery()
            {
                Record();
                return 1;
            }

            public override object ExecuteScalar()
            {
                Record();
                return connector.RowsInRoot;
            }

            private void Record() =>
                connector.Executed.Add((CommandText, _parameters.Cast<DbParameter>().ToDictionary(p => p.ParameterName, p => p.Value)));
        }
    }
}
