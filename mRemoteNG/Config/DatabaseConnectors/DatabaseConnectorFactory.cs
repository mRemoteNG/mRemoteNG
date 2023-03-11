using mRemoteNG.App;
using mRemoteNG.Properties;
using mRemoteNG.Security.SymmetricEncryption;
using System.Runtime.Versioning;

namespace mRemoteNG.Config.DatabaseConnectors
{
    [SupportedOSPlatform("windows")]
    public class DatabaseConnectorFactory
    {
        public static IDatabaseConnector DatabaseConnectorFromSettings()
        {
            var sqlType = Properties.OptionsDBsPage.Default.SQLServerType;
            var sqlHost = Properties.OptionsDBsPage.Default.SQLHost;
            var sqlCatalog = Properties.OptionsDBsPage.Default.SQLDatabaseName;
            var sqlUsername = Properties.OptionsDBsPage.Default.SQLUser;
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            var sqlPassword = cryptographyProvider.Decrypt(Properties.OptionsDBsPage.Default.SQLPass, Runtime.EncryptionKey);

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