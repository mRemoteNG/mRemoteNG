using System.Data;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;
using mRemoteNG.App;
using MySql.Data.MySqlClient;
using Microsoft.Data.SqlClient;
using System.Runtime.Versioning;

namespace mRemoteNG.Config.DataProviders
{
    [SupportedOSPlatform("windows")]
    public class SqlDataProvider(IDatabaseConnector databaseConnector) : IDataProvider<DataTable>
    {
        public IDatabaseConnector DatabaseConnector { get; } = databaseConnector;

        public DataTable Load()
        {
            DataTable dataTable = new();
            System.Data.Common.DbCommand dbQuery = DatabaseConnector.DbCommand("SELECT * FROM tblCons ORDER BY PositionID ASC");
            DatabaseConnector.AssociateItemToThisConnector(dbQuery);
            if (!DatabaseConnector.IsConnected)
                OpenConnection();
            using System.Data.Common.DbDataReader dbDataReader = dbQuery.ExecuteReader(CommandBehavior.CloseConnection);
            // Always load the reader so table schema is available even when tblCons has 0 rows.
            dataTable.Load(dbDataReader);
            return dataTable;
        }

        public void Save(DataTable dataTable)
        {
            if (DbUserIsReadOnly())
            {
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Trying to save connections but the SQL read only checkbox is checked, aborting!");
                return;
            }

            if (!DatabaseConnector.IsConnected)
                OpenConnection();
            if (DatabaseConnector.GetType() == typeof(MSSqlDatabaseConnector))
            {
                SqlConnection sqlConnection = (SqlConnection)DatabaseConnector.DbConnection();
                using SqlTransaction transaction = sqlConnection.BeginTransaction(System.Data.IsolationLevel.Serializable);
                using SqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.Transaction = transaction;
                sqlCommand.CommandText = "SELECT * FROM tblCons";
                using SqlDataAdapter dataAdapter = new();
                dataAdapter.SelectCommand = sqlCommand;

                SqlCommandBuilder builder = new(dataAdapter)
                {
                    // Avoid optimistic concurrency, check if it is necessary.
                    ConflictOption = ConflictOption.OverwriteChanges
                };

                dataAdapter.UpdateCommand = builder.GetUpdateCommand();
                dataAdapter.DeleteCommand = builder.GetDeleteCommand();
                dataAdapter.InsertCommand = builder.GetInsertCommand();
                dataAdapter.Update(dataTable);
                transaction.Commit();
            }
            else if (DatabaseConnector.GetType() == typeof(MySqlDatabaseConnector))
            {
                MySqlConnection dbConnection = (MySqlConnection) DatabaseConnector.DbConnection();
                using MySqlTransaction transaction = dbConnection.BeginTransaction(System.Data.IsolationLevel.Serializable);
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = dbConnection;
                sqlCommand.Transaction = transaction;
                sqlCommand.CommandText = "SELECT * FROM tblCons";
                using MySqlDataAdapter dataAdapter = new(sqlCommand);
                dataAdapter.UpdateBatchSize = 1000;
                using MySqlCommandBuilder cb = new(dataAdapter);
                dataAdapter.Update(dataTable);
                transaction.Commit();
            }
        }

        public void OpenConnection()
        {
            DatabaseConnector.Connect();
        }

        public void CloseConnection()
        {
            DatabaseConnector.Disconnect();
        }

        private bool DbUserIsReadOnly()
        {
            return Properties.OptionsDBsPage.Default.SQLReadOnly;
        }
    }
}
