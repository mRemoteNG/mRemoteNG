using System.Data;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;
using mRemoteNG.App;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Runtime.Versioning;

namespace mRemoteNG.Config.DataProviders
{
    [SupportedOSPlatform("windows")]
    public class SqlDataProvider : IDataProvider<DataTable>
    {
        public IDatabaseConnector DatabaseConnector { get; }

        public SqlDataProvider(IDatabaseConnector databaseConnector)
        {
            DatabaseConnector = databaseConnector;
        }

        public DataTable Load()
        {
            var dataTable = new DataTable();
            var dbQuery = DatabaseConnector.DbCommand("SELECT * FROM tblCons ORDER BY PositionID ASC");
            DatabaseConnector.AssociateItemToThisConnector(dbQuery);
            if (!DatabaseConnector.IsConnected)
                OpenConnection();
            var dbDataReader = dbQuery.ExecuteReader(CommandBehavior.CloseConnection);

            if (dbDataReader.HasRows)
                dataTable.Load(dbDataReader);
            dbDataReader.Close();
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
                using SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.Transaction = transaction;
                sqlCommand.CommandText = "SELECT * FROM tblCons";
                using SqlDataAdapter dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = sqlCommand;

                SqlCommandBuilder builder = new SqlCommandBuilder(dataAdapter);
                // Avoid optimistic concurrency, check if it is necessary.
                builder.ConflictOption = ConflictOption.OverwriteChanges;

                dataAdapter.UpdateCommand = builder.GetUpdateCommand();
                dataAdapter.DeleteCommand = builder.GetDeleteCommand();
                dataAdapter.InsertCommand = builder.GetInsertCommand();
                dataAdapter.Update(dataTable);
                transaction.Commit();
            }
            else if (DatabaseConnector.GetType() == typeof(MySqlDatabaseConnector))
            {
                var dbConnection = (MySqlConnection) DatabaseConnector.DbConnection();
                using MySqlTransaction transaction = dbConnection.BeginTransaction(System.Data.IsolationLevel.Serializable);
                using MySqlCommand sqlCommand = new MySqlCommand();
                sqlCommand.Connection = dbConnection;
                sqlCommand.Transaction = transaction;
                sqlCommand.CommandText = "SELECT * FROM tblCons";
                using MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlCommand);
                dataAdapter.UpdateBatchSize = 1000;
                using MySqlCommandBuilder cb = new MySqlCommandBuilder(dataAdapter);
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