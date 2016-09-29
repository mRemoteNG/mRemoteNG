using mRemoteNG.App;
using mRemoteNG.Messages;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using mRemoteNG.Config.DatabaseConnectors;

namespace mRemoteNG.Config.Connections
{
    public class SqlConnectionsUpdateChecker : IDisposable
    {
        private IDatabaseConnector _sqlConnector;
        private SqlCommand _sqlQuery;
        private SqlDataReader _sqlReader;


        public SqlConnectionsUpdateChecker()
        {
            _sqlConnector = default(SqlDatabaseConnector);
            _sqlQuery = default(SqlCommand);
            _sqlReader = default(SqlDataReader);
        }

        ~SqlConnectionsUpdateChecker()
        {
            Dispose(false);
        }


        public void IsDatabaseUpdateAvailableAsync()
        {
            var t = new Thread(IsDatabaseUpdateAvailableDelegate);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void IsDatabaseUpdateAvailableDelegate()
        {
            IsDatabaseUpdateAvailable();
        }

        public bool IsDatabaseUpdateAvailable()
        {
            ConnectToSqlDb();
            BuildSqlQueryToGetUpdateStatus();
            ExecuteQuery();
            var updateIsAvailable = DatabaseIsMoreUpToDateThanUs();
            RaiseUpdateCheckFinishedEvent(updateIsAvailable);
            return updateIsAvailable;
        }
        private void ConnectToSqlDb()
        {
            try
            {
                _sqlConnector = new SqlDatabaseConnector();
                _sqlConnector.Connect();
            }
            catch(Exception e)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, "Unable to connect to Sql DB to check for updates." + Environment.NewLine + e.Message, true);
            }
        }
        private void BuildSqlQueryToGetUpdateStatus()
        {
            try
            {
                SqlCommandBuilder sqlCmdBuilder = new SqlUpdateQueryBuilder();
                _sqlQuery = sqlCmdBuilder.BuildCommand();
                _sqlConnector.AssociateItemToThisConnector(_sqlQuery);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, "Could not build query to check for updates from the Sql server." + Environment.NewLine + ex.Message, true);
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
            return GetLastUpdateTimeFromDbResponse() > Runtime.LastSqlUpdate;
        }

        private DateTime GetLastUpdateTimeFromDbResponse()
        {
            var lastUpdateInDb = default(DateTime);
            if (_sqlReader.HasRows)
                lastUpdateInDb = Convert.ToDateTime(_sqlReader["LastUpdate"]);
            return lastUpdateInDb;
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


        public delegate void SqlUpdateCheckFinishedEventHandler(bool updateAvailable);

        public static event SqlUpdateCheckFinishedEventHandler SqlUpdateCheckFinished;
        private void RaiseUpdateCheckFinishedEvent(bool updateAvailable)
        {
            SqlUpdateCheckFinished?.Invoke(updateAvailable);
        }
    }
}