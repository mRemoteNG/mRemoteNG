using mRemoteNG.App;
using mRemoteNG.Security.SymmetricEncryption;
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
            switch (type)
            {
                case "mysql":
                    return new MySqlDatabaseConnector(server, database, username, password);
                case "mssql":
                default:
                    return new MSSqlDatabaseConnector(server, database, username, password);
            }
        }
    }
}