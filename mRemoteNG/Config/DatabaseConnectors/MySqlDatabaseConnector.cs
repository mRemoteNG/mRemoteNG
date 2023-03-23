using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.Config.DatabaseConnectors
{
    public class MySqlDatabaseConnector : IDatabaseConnector
    {
        private DbConnection _dbConnection { get; set; } = default(MySqlConnection);
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
            return new MySqlCommand(dbCommand, (MySqlConnection) _dbConnection);
        }

        public bool IsConnected => (_dbConnection.State == ConnectionState.Open);

        public MySqlDatabaseConnector(string host, string database, string username, string password)
        {
            // TODO: add custom port handling?
            string[] hostParts = host.Split(new char[]{':'}, 2);
            _dbHost = hostParts[0];
            _dbPort = (hostParts.Length == 2)?hostParts[1]:"3306";
            _dbName = database;
            _dbUsername = username;
            _dbPassword = password;
            Initialize();
        }

        private void Initialize()
        {
            BuildSqlConnectionString();
            _dbConnection = new MySqlConnection(_dbConnectionString);
        }

        private void BuildSqlConnectionString()
        {
            _dbConnectionString = $"server={_dbHost};user={_dbUsername};database={_dbName};port={_dbPort};password={_dbPassword};";
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
            dbCommand.Connection = (MySqlConnection) _dbConnection;
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