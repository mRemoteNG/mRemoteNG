using System;
using System.Security;
using mRemoteNG.App;
using mRemoteNG.Security.SymmetricEncryption;

namespace mRemoteNG.Config.DatabaseConnectors
{
    public class DatabaseConnectorFactory
    {
        private readonly Func<SecureString> _decryptionKeyRetrievalFun;

        public DatabaseConnectorFactory(Func<SecureString> decryptionKeyRetrievalFun)
        {
            _decryptionKeyRetrievalFun = decryptionKeyRetrievalFun;
        }

        public SqlDatabaseConnector SqlDatabaseConnectorFromSettings()
        {
            var sqlHost = mRemoteNG.Settings.Default.SQLHost;
            var sqlCatalog = mRemoteNG.Settings.Default.SQLDatabaseName;
            var sqlUsername = mRemoteNG.Settings.Default.SQLUser;
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            var sqlPassword = cryptographyProvider.Decrypt(mRemoteNG.Settings.Default.SQLPass, _decryptionKeyRetrievalFun());
            return new SqlDatabaseConnector(sqlHost, sqlCatalog, sqlUsername, sqlPassword);
        }
    }
}
