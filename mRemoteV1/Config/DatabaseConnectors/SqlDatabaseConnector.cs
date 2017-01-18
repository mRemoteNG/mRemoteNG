using System.Data;
using System.Data.SqlClient;
using mRemoteNG.App;
using mRemoteNG.Security.SymmetricEncryption;

namespace mRemoteNG.Config.DatabaseConnectors
{
    public class SqlDatabaseConnector : IDatabaseConnector
    {
        public  SqlConnection SqlConnection { get; private set; } = default(SqlConnection);
        private string _sqlConnectionString = "";
        private string _sqlHost;
        private string _sqlCatalog;
        private string _sqlUsername;
        private string _sqlPassword;

        public bool IsConnected => (SqlConnection.State == ConnectionState.Open);

        public SqlDatabaseConnector()
        {
            Initialize();
        }

        private void Initialize()
        {
            BuildSqlConnectionString();
            SqlConnection = new SqlConnection(_sqlConnectionString);
        }

        private void BuildSqlConnectionString()
        {
            GetSqlConnectionDataFromSettings();

            if (mRemoteNG.Settings.Default.SQLUser != "")
                BuildSqlConnectionStringWithCustomCredentials();
            else
                BuildSqlConnectionStringWithDefaultCredentials();
        }

        private void BuildSqlConnectionStringWithCustomCredentials()
        {
            _sqlConnectionString = $"Data Source={_sqlHost};Initial Catalog={_sqlCatalog};User Id={_sqlUsername};Password={_sqlPassword}";
        }

        private void BuildSqlConnectionStringWithDefaultCredentials()
        {
            _sqlConnectionString = $"Data Source={_sqlHost};Initial Catalog={_sqlCatalog};Integrated Security=True";
        }

        private void GetSqlConnectionDataFromSettings()
        {
            _sqlHost = mRemoteNG.Settings.Default.SQLHost;
            _sqlCatalog = mRemoteNG.Settings.Default.SQLDatabaseName;
            _sqlUsername = mRemoteNG.Settings.Default.SQLUser;
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            _sqlPassword = cryptographyProvider.Decrypt(mRemoteNG.Settings.Default.SQLPass, Runtime.EncryptionKey);
        }

        public void Connect()
        {
            SqlConnection.Open();
        }

        public void Disconnect()
        {
            SqlConnection.Close();
        }

        public void AssociateItemToThisConnector(SqlCommand sqlCommand)
        {
            sqlCommand.Connection = SqlConnection;
        }

        public void Dispose()
        {
            Dispose(true);
        }
        private void Dispose(bool itIsSafeToFreeManagedObjects)
        {
            if (!itIsSafeToFreeManagedObjects) return;
            SqlConnection.Close();
            SqlConnection.Dispose();
        }
    }
}