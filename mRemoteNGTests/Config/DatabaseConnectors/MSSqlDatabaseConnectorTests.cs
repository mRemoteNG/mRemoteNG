using Microsoft.Data.SqlClient;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Properties;
using NUnit.Framework;

namespace mRemoteNGTests.Config.DatabaseConnectors;

/// <summary>
/// The connection to Microsoft SQL Server is always encrypted, and the server certificate is
/// validated unless the user chose to trust it: accepting any certificate let a machine in the
/// middle read the login and the stored connections (CVE-2026-87867).
/// </summary>
public class MSSqlDatabaseConnectorTests
{
    private static SqlConnectionStringBuilder ConnectionStringOf(IDatabaseConnector connector)
    {
        using (connector)
            return new SqlConnectionStringBuilder(connector.DbConnection().ConnectionString);
    }

    // An empty user name is Windows authentication, any other is a SQL Server login
    [TestCase("mrng_login")]
    [TestCase("")]
    public void ServerCertificateIsValidatedByDefault(string username)
    {
        SqlConnectionStringBuilder connectionString = ConnectionStringOf(
            new MSSqlDatabaseConnector("sql01.example.local", "mRemoteNG", username, "secret"));

        Assert.Multiple(() =>
        {
            Assert.That(connectionString.Encrypt, Is.EqualTo(SqlConnectionEncryptOption.Mandatory));
            Assert.That(connectionString.TrustServerCertificate, Is.False);
        });
    }

    [TestCase("mrng_login")]
    [TestCase("")]
    public void ServerCertificateIsTrustedWhenTheUserChoseIt(string username)
    {
        SqlConnectionStringBuilder connectionString = ConnectionStringOf(
            new MSSqlDatabaseConnector("sql01.example.local", "mRemoteNG", username, "secret", trustServerCertificate: true));

        Assert.Multiple(() =>
        {
            Assert.That(connectionString.Encrypt, Is.EqualTo(SqlConnectionEncryptOption.Mandatory));
            Assert.That(connectionString.TrustServerCertificate, Is.True);
        });
    }

    [Test]
    public void FactoryPassesTheChoiceToTheConnector()
    {
        Assert.Multiple(() =>
        {
            Assert.That(ConnectionStringOf(DatabaseConnectorFactory.DatabaseConnector(
                "mssql", "sql01.example.local", "mRemoteNG", "", "")).TrustServerCertificate, Is.False);
            Assert.That(ConnectionStringOf(DatabaseConnectorFactory.DatabaseConnector(
                "mssql", "sql01.example.local", "mRemoteNG", "", "", trustServerCertificate: true)).TrustServerCertificate, Is.True);
        });
    }

    [Test]
    public void TrustServerCertificateOptionIsOffByDefault()
    {
        Assert.That(OptionsDBsPage.Default.Properties["SQLTrustServerCertificate"]?.DefaultValue, Is.EqualTo("False"));
    }

    [Test]
    public void ConnectorFromSettingsUsesTheTrustServerCertificateOption()
    {
        OptionsDBsPage settings = OptionsDBsPage.Default;
        string serverType = settings.SQLServerType;
        string host = settings.SQLHost;
        string database = settings.SQLDatabaseName;
        string user = settings.SQLUser;
        string storedPassword = settings.SQLPass;
        bool trustServerCertificate = settings.SQLTrustServerCertificate;
        try
        {
            settings.SQLServerType = "mssql";
            settings.SQLHost = "sql01.example.local";
            settings.SQLDatabaseName = "mRemoteNG";
            settings.SQLUser = "";
            settings.SQLPass = "";

            settings.SQLTrustServerCertificate = true;
            Assert.That(ConnectionStringOf(DatabaseConnectorFactory.DatabaseConnectorFromSettings()).TrustServerCertificate, Is.True);

            settings.SQLTrustServerCertificate = false;
            Assert.That(ConnectionStringOf(DatabaseConnectorFactory.DatabaseConnectorFromSettings()).TrustServerCertificate, Is.False);
        }
        finally
        {
            settings.SQLServerType = serverType;
            settings.SQLHost = host;
            settings.SQLDatabaseName = database;
            settings.SQLUser = user;
            settings.SQLPass = storedPassword;
            settings.SQLTrustServerCertificate = trustServerCertificate;
        }
    }
}
