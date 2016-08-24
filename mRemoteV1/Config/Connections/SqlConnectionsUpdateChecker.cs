using mRemoteNG.App;
using mRemoteNG.Messages;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace mRemoteNG.Config.Connections
{
    public class SqlConnectionsUpdateChecker : IDisposable
    {
        private IDatabaseConnector sqlConnector;
        private SqlCommand sqlQuery;
        private SqlDataReader sqlReader;


        public SqlConnectionsUpdateChecker()
        {
            sqlConnector = default(SqlDatabaseConnector);
            sqlQuery = default(SqlCommand);
            sqlReader = default(SqlDataReader);
        }

        ~SqlConnectionsUpdateChecker()
        {
            Dispose(false);
        }


        public void IsDatabaseUpdateAvailableAsync()
        {
            Thread t = new Thread(IsDatabaseUpdateAvailableDelegate);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void IsDatabaseUpdateAvailableDelegate()
        {
            IsDatabaseUpdateAvailable();
        }

        public bool IsDatabaseUpdateAvailable()
        {
            ConnectToSqlDB();
            BuildSqlQueryToGetUpdateStatus();
            ExecuteQuery();
            bool updateIsAvailable = DatabaseIsMoreUpToDateThanUs();
            SendUpdateCheckFinishedEvent(updateIsAvailable);
            return updateIsAvailable;
        }
        private void ConnectToSqlDB()
        {
            try
            {
                sqlConnector = new SqlDatabaseConnector();
                sqlConnector.Connect();
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
                sqlQuery = sqlCmdBuilder.BuildCommand();
                sqlConnector.AssociateItemToThisConnector(sqlQuery);
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
                sqlReader = sqlQuery.ExecuteReader(CommandBehavior.CloseConnection);
                sqlReader.Read();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, "Error executing Sql query to get updates from the DB." + Environment.NewLine + ex.Message, true);
            }
        }
        private bool DatabaseIsMoreUpToDateThanUs()
        {
            if (GetLastUpdateTimeFromDBResponse() > Runtime.LastSqlUpdate)
                return true;
            return false;
        }
        private DateTime GetLastUpdateTimeFromDBResponse()
        {
            DateTime LastUpdateInDB = default(DateTime);
            if (sqlReader.HasRows)
                LastUpdateInDB = Convert.ToDateTime(sqlReader["LastUpdate"]);
            return LastUpdateInDB;
        }
        private void SendUpdateCheckFinishedEvent(bool UpdateAvailable)
        {
            if (SQLUpdateCheckFinishedEvent != null)
                SQLUpdateCheckFinishedEvent(UpdateAvailable);
        }


        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool itIsSafeToDisposeManagedObjects)
        {
            if (itIsSafeToDisposeManagedObjects)
            {
                sqlConnector.Disconnect();
                sqlConnector.Dispose();
            }
        }


        public delegate void SQLUpdateCheckFinishedEventHandler(bool UpdateAvailable);
        private static SQLUpdateCheckFinishedEventHandler SQLUpdateCheckFinishedEvent;
        public static event SQLUpdateCheckFinishedEventHandler SQLUpdateCheckFinished
        {
            add { SQLUpdateCheckFinishedEvent = (SQLUpdateCheckFinishedEventHandler)System.Delegate.Combine(SQLUpdateCheckFinishedEvent, value); }
            remove { SQLUpdateCheckFinishedEvent = (SQLUpdateCheckFinishedEventHandler)System.Delegate.Remove(SQLUpdateCheckFinishedEvent, value); }
        }
    }
}