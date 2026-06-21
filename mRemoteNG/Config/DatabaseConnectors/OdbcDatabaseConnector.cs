using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Threading.Tasks;

// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.Config.DatabaseConnectors
{
    public class OdbcDatabaseConnector : IDatabaseConnector
    {
        private DbConnection _dbConnection { get; set; } = default(OdbcConnection);
        private string _dbConnectionString = "";
        private readonly string _connectionString;
        private readonly string _dbUsername;
        private readonly string _dbPassword;

        public DbConnection DbConnection()
        {
            return _dbConnection;
        }

        public DbCommand DbCommand(string dbCommand)
        {
            return new OdbcCommand(dbCommand, (OdbcConnection) _dbConnection);
        }

        public bool IsConnected => (_dbConnection.State == ConnectionState.Open);

        /// <summary>
        /// Creates an ODBC database connector.
        /// </summary>
        /// <param name="connectionString">
        /// An ODBC connection string or DSN name. If a plain DSN name is provided (no '=' character),
        /// it is automatically wrapped as "DSN=&lt;name&gt;".
        /// </param>
        /// <param name="username">Optional user name appended to the connection string as UID.</param>
        /// <param name="password">Optional password appended to the connection string as PWD.</param>
        public OdbcDatabaseConnector(string connectionString, string username, string password)
        {
            _connectionString = connectionString;
            _dbUsername = username;
            _dbPassword = password;
            Initialize();
        }

        private void Initialize()
        {
            BuildConnectionString();
            _dbConnection = new OdbcConnection(_dbConnectionString);
        }

        private void BuildConnectionString()
        {
            // If no '=' present in the provided string it is a plain DSN name.
            string baseString = _connectionString.Contains('=')
                ? _connectionString
                : $"DSN={_connectionString}";

            OdbcConnectionStringBuilder builder = new()
            {
                ConnectionString = baseString
            };

            if (!string.IsNullOrEmpty(_dbUsername))
                builder["UID"] = _dbUsername;

            if (!string.IsNullOrEmpty(_dbPassword))
                builder["PWD"] = _dbPassword;

            _dbConnectionString = builder.ConnectionString;
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
            dbCommand.Connection = (OdbcConnection) _dbConnection;
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
