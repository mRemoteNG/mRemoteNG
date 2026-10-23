using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Sql;
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
}
