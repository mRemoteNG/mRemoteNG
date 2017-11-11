using mRemoteNG.App;
using mRemoteNG.Security.SymmetricEncryption;

namespace mRemoteNG.Config.DatabaseConnectors
{
    public class DatabaseConnectorFactory
    {
        public static SqlDatabaseConnector SqlDatabaseConnectorFromSettings()
        {
            var sqlHost = mRemoteNG.Settings.Default.SQLHost;
            var sqlCatalog = mRemoteNG.Settings.Default.SQLDatabaseName;
            var sqlUsername = mRemoteNG.Settings.Default.SQLUser;
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            var sqlPassword = cryptographyProvider.Decrypt(mRemoteNG.Settings.Default.SQLPass, Runtime.EncryptionKey);
            return new SqlDatabaseConnector(sqlHost, sqlCatalog, sqlUsername, sqlPassword);
        }
    }
}
