using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Npgsql;


namespace mRemoteNG.Config.DatabaseConnectors
{
    public class PostgreSQLDatabaseConnector : IDatabaseConnector
    {
        private DbConnection _dbConnection { get; set; } = default(NpgsqlConnection);
        private string _dbConnectionString = "";
        private readonly string _dbHost;
        private readonly string _dbPort;
        private readonly string _dbName;
        private readonly string _dbUsername;
        private readonly string _dbPassword;

        public DbConnection DbConnection()
        {
            return _dbConnection;
        }

        public DbCommand DbCommand(string dbCommand)
        {
            return new NpgsqlCommand(dbCommand, (NpgsqlConnection)_dbConnection);
        }

        public bool IsConnected => (_dbConnection.State == ConnectionState.Open);

        public PostgreSQLDatabaseConnector(string host, string database, string username, string password)
        {
            string[] hostParts = host.Split(new char[] { ':' }, 2);
            _dbHost = hostParts[0];
            _dbPort = (hostParts.Length == 2) ? hostParts[1] : "5432";
            _dbName = database;
            _dbUsername = username;
            _dbPassword = password;
            Initialize();
        }

        private void Initialize()
        {
            BuildSqlConnectionString();
            _dbConnection = new NpgsqlConnection(_dbConnectionString);
        }

        private void BuildSqlConnectionString()
        {
            _dbConnectionString = $"Server={_dbHost};User Id={_dbUsername};Database={_dbName};Port={_dbPort};Password={_dbPassword}";
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
            dbCommand.Connection = (NpgsqlConnection)_dbConnection;
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