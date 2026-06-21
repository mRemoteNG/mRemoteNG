using mRemoteNG.App;
using mRemoteNG.Security.SymmetricEncryption;
using System;
using System.Runtime.Versioning;

namespace mRemoteNG.Config.DatabaseConnectors
{
    [SupportedOSPlatform("windows")]
    public static class DatabaseConnectorFactory
    {
        public static IDatabaseConnector DatabaseConnectorFromSettings()
        {
            // TODO: add custom port handling?
            string sqlType = Properties.OptionsDBsPage.Default.SQLServerType;
            string sqlHost = Properties.OptionsDBsPage.Default.SQLHost;
            string sqlCatalog = Properties.OptionsDBsPage.Default.SQLDatabaseName;
            string sqlUsername = Properties.OptionsDBsPage.Default.SQLUser;
            LegacyRijndaelCryptographyProvider cryptographyProvider = new();
            string sqlPassword = cryptographyProvider.Decrypt(Properties.OptionsDBsPage.Default.SQLPass, Runtime.EncryptionKey);

            return DatabaseConnector(sqlType, sqlHost, sqlCatalog, sqlUsername, sqlPassword);
        }

        public static IDatabaseConnector DatabaseConnector(string type, string server, string database, string username, string password)
        {
            return type switch
            {
                "mysql" => new MySqlDatabaseConnector(server, database, username, password),
                "odbc" => throw new NotSupportedException("ODBC database connections are not supported for schema initialization. Please use a supported database backend."),
                "mssql" => new MSSqlDatabaseConnector(server, database, username, password),
                _ => new MSSqlDatabaseConnector(server, database, username, password)
            };
        }
    }
}