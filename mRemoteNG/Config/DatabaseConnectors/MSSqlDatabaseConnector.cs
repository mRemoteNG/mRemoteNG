using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.Config.DatabaseConnectors
{
    public class MSSqlDatabaseConnector : IDatabaseConnector
    {
        private DbConnection _dbConnection { get; set; } = default(SqlConnection);
        private string _dbConnectionString = "";
        private readonly string _dbHost;
        private readonly string _dbCatalog;
        private readonly string _dbUsername;
        private readonly string _dbPassword;

        public DbConnection DbConnection()
        {
            return _dbConnection;
        }

        public DbCommand DbCommand(string dbCommand)
        {
            return new SqlCommand(dbCommand, (SqlConnection) _dbConnection);
        }

        public bool IsConnected
        {
            get { return (_dbConnection.State == ConnectionState.Open); }
        }

        public MSSqlDatabaseConnector(string sqlServer, string catalog, string username, string password)
        {
            _dbHost = sqlServer;
            _dbCatalog = catalog;
            _dbUsername = username;
            _dbPassword = password;
            Initialize();
        }

        private void Initialize()
        {
            BuildSqlConnectionString();
            _dbConnection = new SqlConnection(_dbConnectionString);
        }

        private void BuildSqlConnectionString()
        {
            if (_dbUsername != "")
                BuildDbConnectionStringWithCustomCredentials();
            else
                BuildDbConnectionStringWithDefaultCredentials();
        }

        private void BuildDbConnectionStringWithCustomCredentials()
        {
            _dbConnectionString = $"Data Source={_dbHost};Initial Catalog={_dbCatalog};User Id={_dbUsername};Password={_dbPassword}";
        }

        private void BuildDbConnectionStringWithDefaultCredentials()
        {
            _dbConnectionString = $"Data Source={_dbHost};Initial Catalog={_dbCatalog};Integrated Security=True";
        }

        public void Connect()
        {
            _dbConnection.Open();
        }

        public async Task ConnectAsync()
        {
            await _dbConnection.OpenAsync();
        }

        public void Disconnect()
        {
            _dbConnection.Close();
        }

        public void AssociateItemToThisConnector(DbCommand dbCommand)
        {
            dbCommand.Connection = (SqlConnection) _dbConnection;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool itIsSafeToFreeManagedObjects)
        {
            if (!itIsSafeToFreeManagedObjects) return;
            _dbConnection.Close();
            _dbConnection.Dispose();
        }
    }
}