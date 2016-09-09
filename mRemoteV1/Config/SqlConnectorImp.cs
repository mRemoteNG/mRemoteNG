using mRemoteNG.App.Info;
using System.Data.SqlClient;
using mRemoteNG.Security.SymmetricEncryption;

namespace mRemoteNG.Config
{
    public class SqlConnectorImp : SqlConnector
    {
        private SqlConnection _sqlConnection = default(SqlConnection);
        private string _sqlConnectionString = "";
        private string _sqlHost;
        private string _sqlCatalog;
        private string _sqlUsername;
        private string _sqlPassword;

        public SqlConnectorImp()
        {
            Initialize();
        }

        ~SqlConnectorImp()
        {
            Dispose(false);
        }

        private void Initialize()
        {
            BuildSqlConnectionString();
            _sqlConnection = new SqlConnection(_sqlConnectionString);
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
            _sqlConnectionString = string.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3}", _sqlHost, _sqlCatalog, _sqlUsername, _sqlPassword);
        }

        private void BuildSqlConnectionStringWithDefaultCredentials()
        {
            _sqlConnectionString = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True", _sqlHost, _sqlCatalog);
        }

        private void GetSqlConnectionDataFromSettings()
        {
            _sqlHost = mRemoteNG.Settings.Default.SQLHost;
            _sqlCatalog = mRemoteNG.Settings.Default.SQLDatabaseName;
            _sqlUsername = mRemoteNG.Settings.Default.SQLUser;
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            _sqlPassword = cryptographyProvider.Decrypt(mRemoteNG.Settings.Default.SQLPass, GeneralAppInfo.EncryptionKey);
        }

        public void Connect()
        {
            _sqlConnection.Open();
        }

        public void Disconnect()
        {
            _sqlConnection.Close();
        }

        public void AssociateItemToThisConnector(SqlCommand sqlCommand)
        {
            sqlCommand.Connection = _sqlConnection;
        }
        

        public void Dispose()
        {
            Dispose(true);
        }
        private void Dispose(bool itIsSafeToFreeManagedObjects)
        {
            if (itIsSafeToFreeManagedObjects)
            {
                _sqlConnection.Close();
                _sqlConnection.Dispose();
            }
        }
    }
}