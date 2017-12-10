using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.Config.DatabaseConnectors
{
    public class SqlDatabaseConnector : IDatabaseConnector
    {
        public  SqlConnection SqlConnection { get; private set; } = default(SqlConnection);
        private string _sqlConnectionString = "";
        private readonly string _sqlHost;
        private readonly string _sqlCatalog;
        private readonly string _sqlUsername;
        private readonly string _sqlPassword;

        public bool IsConnected
        {
            get { return (SqlConnection.State == ConnectionState.Open); }
        }

        public SqlDatabaseConnector(string sqlServer, string catalog, string username, string password)
        {
            _sqlHost = sqlServer;
            _sqlCatalog = catalog;
            _sqlUsername = username;
            _sqlPassword = password;
            Initialize();
        }

        private void Initialize()
        {
            BuildSqlConnectionString();
            SqlConnection = new SqlConnection(_sqlConnectionString);
        }

        private void BuildSqlConnectionString()
        {
            if (_sqlUsername != "")
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

        public void Connect()
        {
            SqlConnection.Open();
        }

        public async Task ConnectAsync()
        {
            await SqlConnection.OpenAsync();
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