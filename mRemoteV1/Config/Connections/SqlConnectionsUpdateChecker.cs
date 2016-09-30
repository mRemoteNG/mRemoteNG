using mRemoteNG.App;
using mRemoteNG.Messages;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using mRemoteNG.Config.DatabaseConnectors;

namespace mRemoteNG.Config.Connections
{
    public class SqlConnectionsUpdateChecker : IDisposable, IConnectionsUpdateChecker
    {
        private readonly SqlDatabaseConnector _sqlConnector;
        private readonly SqlCommand _sqlQuery;
        private SqlDataReader _sqlReader;
        private DateTime _lastUpdateTime;


        public SqlConnectionsUpdateChecker()
        {
            _sqlConnector = new SqlDatabaseConnector();
            _sqlQuery = new SqlCommand("SELECT * FROM tblUpdate", _sqlConnector.SqlConnection);
            _sqlReader = default(SqlDataReader);
            _lastUpdateTime = default(DateTime);
        }

        
        public bool IsUpdateAvailable()
        {
            ConnectToSqlDb();
            ExecuteQuery();
            var updateIsAvailable = DatabaseIsMoreUpToDateThanUs();
            RaiseUpdateCheckFinishedEvent(updateIsAvailable);
            return updateIsAvailable;
        }

        public void IsUpdateAvailableAsync()
        {
            var threadStart = new ThreadStart(() => IsUpdateAvailable());
            var thread = new Thread(threadStart);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void ConnectToSqlDb()
        {
            try
            {
                _sqlConnector.Connect();
            }
            catch(Exception e)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, "Unable to connect to Sql DB to check for updates." + Environment.NewLine + e.Message, true);
            }
        }

        private void ExecuteQuery()
        {
            try
            {
                _sqlReader = _sqlQuery.ExecuteReader(CommandBehavior.CloseConnection);
                _sqlReader.Read();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, "Error executing Sql query to get updates from the DB." + Environment.NewLine + ex.Message, true);
            }
        }

        private bool DatabaseIsMoreUpToDateThanUs()
        {
            return GetLastUpdateTimeFromDbResponse() > _lastUpdateTime;
        }

        private DateTime GetLastUpdateTimeFromDbResponse()
        {
            var lastUpdateInDb = default(DateTime);
            if (_sqlReader.HasRows)
                lastUpdateInDb = Convert.ToDateTime(_sqlReader["LastUpdate"]);
            return lastUpdateInDb;
        }
        
        public delegate void SqlUpdateCheckFinishedEventHandler(bool updateAvailable);

        public static event SqlUpdateCheckFinishedEventHandler SqlUpdateCheckFinished;
        private void RaiseUpdateCheckFinishedEvent(bool updateAvailable)
        {
            SqlUpdateCheckFinished?.Invoke(updateAvailable);
        }


        ~SqlConnectionsUpdateChecker()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool itIsSafeToDisposeManagedObjects)
        {
            if (!itIsSafeToDisposeManagedObjects) return;
            _sqlConnector.Disconnect();
            _sqlConnector.Dispose();
        }
    }
}