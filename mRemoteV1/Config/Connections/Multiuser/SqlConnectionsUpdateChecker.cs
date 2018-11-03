using mRemoteNG.App;
using mRemoteNG.Messages;
using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using mRemoteNG.Config.Connections.Multiuser;
using mRemoteNG.Config.DatabaseConnectors;

namespace mRemoteNG.Config.Connections
{
    public class SqlConnectionsUpdateChecker : IConnectionsUpdateChecker
    {
        private readonly IDatabaseConnector _dbConnector;
        private readonly DbCommand _dbQuery;
        private DateTime _lastUpdateTime;
        private DateTime _lastDatabaseUpdateTime;


        public SqlConnectionsUpdateChecker()
        {
            _dbConnector = DatabaseConnectorFactory.DatabaseConnectorFromSettings();
            _dbQuery = _dbConnector.DbCommand("SELECT * FROM tblUpdate");
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

        private void ConnectToSqlDb()
        {
            try
            {
                _dbConnector.Connect();
            }
            catch(Exception e)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, "Unable to connect to Sql DB to check for updates." + Environment.NewLine + e.Message, true);
            }
        }

        private bool DatabaseIsMoreUpToDateThanUs()
        {
            var lastUpdateInDb = GetLastUpdateTimeFromDbResponse();
            var IAmTheLastoneUpdated = CheckIfIAmTheLastOneUpdated(lastUpdateInDb);
            return (lastUpdateInDb > _lastUpdateTime && !IAmTheLastoneUpdated);
        }

        private bool CheckIfIAmTheLastOneUpdated(DateTime lastUpdateInDb)
        {
            DateTime LastSqlUpdateWithoutMilliseconds = new DateTime(Runtime.ConnectionsService.LastSqlUpdate.Ticks - (Runtime.ConnectionsService.LastSqlUpdate.Ticks % TimeSpan.TicksPerSecond), Runtime.ConnectionsService.LastSqlUpdate.Kind);
            return lastUpdateInDb == LastSqlUpdateWithoutMilliseconds;
        }

        private DateTime GetLastUpdateTimeFromDbResponse()
        {
            var lastUpdateInDb = default(DateTime);
            try
            {
                var sqlReader = _dbQuery.ExecuteReader(CommandBehavior.CloseConnection);
                sqlReader.Read();
                if (sqlReader.HasRows)
                    lastUpdateInDb = Convert.ToDateTime(sqlReader["LastUpdate"]);
                sqlReader.Close();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, "Error executing Sql query to get updates from the DB." + Environment.NewLine + ex.Message, true);
            }
            _lastDatabaseUpdateTime = lastUpdateInDb;
            return lastUpdateInDb;
        }


        public event EventHandler UpdateCheckStarted;
        private void RaiseUpdateCheckStartedEvent()
        {
            UpdateCheckStarted?.Invoke(this, EventArgs.Empty);
        }

        public event UpdateCheckFinishedEventHandler UpdateCheckFinished;
        private void RaiseUpdateCheckFinishedEvent(bool updateAvailable)
        {
            var args = new ConnectionsUpdateCheckFinishedEventArgs {UpdateAvailable = updateAvailable};
            UpdateCheckFinished?.Invoke(this, args);
        }

        public event ConnectionsUpdateAvailableEventHandler ConnectionsUpdateAvailable;
        private void RaiseConnectionsUpdateAvailableEvent()
        {
            var args = new ConnectionsUpdateAvailableEventArgs(_dbConnector, _lastDatabaseUpdateTime);
            ConnectionsUpdateAvailable?.Invoke(this, args);
            if(args.Handled)
                _lastUpdateTime = _lastDatabaseUpdateTime;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool itIsSafeToDisposeManagedObjects)
        {
            if (!itIsSafeToDisposeManagedObjects) return;
            _dbConnector.Disconnect();
            _dbConnector.Dispose();
            _dbQuery.Dispose();
        }
    }
}