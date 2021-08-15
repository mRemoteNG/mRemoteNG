﻿using mRemoteNG.App;
using mRemoteNG.Security.SymmetricEncryption;

namespace mRemoteNG.Config.DatabaseConnectors
{
    public class DatabaseConnectorFactory
    {
        public static IDatabaseConnector DatabaseConnectorFromSettings()
        {
            var sqlType = Properties.Settings.Default.SQLServerType;
            var sqlHost = Properties.Settings.Default.SQLHost;
            var sqlCatalog = Properties.Settings.Default.SQLDatabaseName;
            var sqlUsername = Properties.Settings.Default.SQLUser;
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            var sqlPassword = cryptographyProvider.Decrypt(Properties.Settings.Default.SQLPass, Runtime.EncryptionKey);

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