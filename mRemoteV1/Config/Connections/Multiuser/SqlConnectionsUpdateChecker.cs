using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using mRemoteNG.App;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;

namespace mRemoteNG.Config.Connections
{
    public class SqlConnectionsUpdateChecker : IConnectionsUpdateChecker
    {
        private readonly SqlDatabaseConnector _sqlConnector;
        private readonly SqlCommand _sqlQuery;
        private DateTime _lastDatabaseUpdateTime;
        private DateTime _lastUpdateTime;


        public SqlConnectionsUpdateChecker()
        {
            _sqlConnector = new SqlDatabaseConnector();
            _sqlQuery = new SqlCommand("SELECT * FROM tblUpdate", _sqlConnector.SqlConnection);
            _lastUpdateTime = default(DateTime);
            _lastDatabaseUpdateTime = default(DateTime);
        }

        public bool IsUpdateAvailable()
        {
            RaiseUpdateCheckStartedEvent();
            ConnectToSqlDb();
            var updateIsAvailable = DatabaseIsMoreUpToDateThanUs();
            if (updateIsAvailable)
                RaiseConnectionsUpdateAvailableEvent();
            RaiseUpdateCheckFinishedEvent(updateIsAvailable);
            return updateIsAvailable;
        }

        public void IsUpdateAvailableAsync()
        {
            var thread = new Thread(() => IsUpdateAvailable());
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }


        public event EventHandler UpdateCheckStarted;

        public event UpdateCheckFinishedEventHandler UpdateCheckFinished;

        public event ConnectionsUpdateAvailableEventHandler ConnectionsUpdateAvailable;

        public void Dispose()
        {
            Dispose(true);
        }

        private void ConnectToSqlDb()
        {
            try
            {
                _sqlConnector.Connect();
            }
            catch (Exception e)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                    "Unable to connect to Sql DB to check for updates." + Environment.NewLine + e.Message, true);
            }
        }

        private bool DatabaseIsMoreUpToDateThanUs()
        {
            return GetLastUpdateTimeFromDbResponse() > _lastUpdateTime;
        }

        private DateTime GetLastUpdateTimeFromDbResponse()
        {
            var lastUpdateInDb = default(DateTime);
            try
            {
                var sqlReader = _sqlQuery.ExecuteReader(CommandBehavior.CloseConnection);
                sqlReader.Read();
                if (sqlReader.HasRows)
                    lastUpdateInDb = Convert.ToDateTime(sqlReader["LastUpdate"]);
                sqlReader.Close();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                    "Error executing Sql query to get updates from the DB." + Environment.NewLine + ex.Message, true);
            }
            _lastDatabaseUpdateTime = lastUpdateInDb;
            return lastUpdateInDb;
        }

        private void RaiseUpdateCheckStartedEvent()
        {
            UpdateCheckStarted?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseUpdateCheckFinishedEvent(bool updateAvailable)
        {
            var args = new ConnectionsUpdateCheckFinishedEventArgs {UpdateAvailable = updateAvailable};
            UpdateCheckFinished?.Invoke(this, args);
        }

        private void RaiseConnectionsUpdateAvailableEvent()
        {
            var args = new ConnectionsUpdateAvailableEventArgs(_sqlConnector, _lastDatabaseUpdateTime);
            ConnectionsUpdateAvailable?.Invoke(this, args);
            if (args.Handled)
                _lastUpdateTime = _lastDatabaseUpdateTime;
        }

        private void Dispose(bool itIsSafeToDisposeManagedObjects)
        {
            if (!itIsSafeToDisposeManagedObjects) return;
            _sqlConnector.Disconnect();
            _sqlConnector.Dispose();
        }
    }
}