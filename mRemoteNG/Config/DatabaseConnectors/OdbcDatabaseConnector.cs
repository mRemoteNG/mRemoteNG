using System;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Threading.Tasks;

namespace mRemoteNG.Config.DatabaseConnectors
{
    public class OdbcDatabaseConnector : IDatabaseConnector
    {
        private DbConnection _dbConnection { get; set; } = null!;
        private string _dbConnectionString = string.Empty;
        private readonly string _odbcSource;
        private readonly string _dbName;
        private readonly string _dbUsername;
        private readonly string _dbPassword;

        public OdbcDatabaseConnector(string source, string database, string username, string password)
        {
            _odbcSource = source;
            _dbName = database;
            _dbUsername = username;
            _dbPassword = password;
            Initialize();
        }

        public DbConnection DbConnection()
        {
            return _dbConnection;
        }

        public DbCommand DbCommand(string dbCommand)
        {
            return new OdbcCommand(dbCommand, (OdbcConnection)_dbConnection);
        }

        public bool IsConnected => _dbConnection.State == ConnectionState.Open;

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
            dbCommand.Connection = (OdbcConnection)_dbConnection;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Initialize()
        {
            BuildDbConnectionString();
            _dbConnection = new OdbcConnection(_dbConnectionString);
        }

        private void BuildDbConnectionString()
        {
            OdbcConnectionStringBuilder builder = CreateBaseBuilder(_odbcSource);

            if (!string.IsNullOrWhiteSpace(_dbName))
                builder["Database"] = _dbName;

            if (!string.IsNullOrWhiteSpace(_dbUsername))
                builder["Uid"] = _dbUsername;

            if (!string.IsNullOrWhiteSpace(_dbPassword))
                builder["Pwd"] = _dbPassword;

            _dbConnectionString = builder.ConnectionString;
        }

        private static OdbcConnectionStringBuilder CreateBaseBuilder(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return new OdbcConnectionStringBuilder();

            if (source.Contains('=') || source.Contains(';'))
                return new OdbcConnectionStringBuilder(source);

            OdbcConnectionStringBuilder builder = new();
            builder["Dsn"] = source;
            return builder;
        }

        private void Dispose(bool itIsSafeToFreeManagedObjects)
        {
            if (!itIsSafeToFreeManagedObjects)
                return;

            _dbConnection.Close();
            _dbConnection.Dispose();
        }
    }
}
