using System;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using static BrightIdeasSoftware.TreeListView;

// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.Config.DatabaseConnectors
{
    public class MSSqlDatabaseConnector : IDatabaseConnector
    {
        private DbConnection _dbConnection { get; set; } = default!;
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

        public bool IsConnected => (_dbConnection.State == ConnectionState.Open);

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
            if (!string.IsNullOrEmpty(_dbUsername) || !string.IsNullOrEmpty(_dbPassword))
                BuildDbConnectionStringWithCustomCredentials();
            else
                BuildDbConnectionStringWithDefaultCredentials();
        }

        private void BuildDbConnectionStringWithCustomCredentials()
        {
            string[] hostParts = _dbHost.Split(new char[] { ':' }, 2);
            string _dbPort = (hostParts.Length == 2) ? hostParts[1] : "1433";

            _dbConnectionString = new SqlConnectionStringBuilder
            {
                ApplicationName = "mRemoteNG",
                DataSource = $"{hostParts[0]},{_dbPort}",
                InitialCatalog = _dbCatalog,
                UserID = _dbUsername,
                Password = _dbPassword,
                IntegratedSecurity = false,
                Encrypt = true,
                TrustServerCertificate = true,
                ConnectTimeout = 30,
                MultipleActiveResultSets = true
            }.ToString();
        }

        private void BuildDbConnectionStringWithDefaultCredentials()
        {
            _dbConnectionString = new SqlConnectionStringBuilder
            {
                ApplicationName = "mRemoteNG",
                DataSource = _dbHost,
                InitialCatalog = _dbCatalog,
                IntegratedSecurity = true,
                Encrypt = true,
                TrustServerCertificate = true,
                ConnectTimeout = 30,
                MultipleActiveResultSets = true
            }.ToString();
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
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool itIsSafeToFreeManagedObjects)
        {
            if (!itIsSafeToFreeManagedObjects) return;
            _dbConnection.Close();
            _dbConnection.Dispose();
        }
    }
}