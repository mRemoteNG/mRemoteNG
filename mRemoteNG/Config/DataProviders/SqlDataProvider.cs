using System;
using System.Data;
using System.Data.Common;
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
            return Load(null);
        }

        public DataTable Load(DbTransaction transaction)
        {
            DataTable dataTable = new();
            System.Data.Common.DbCommand dbQuery = DatabaseConnector.DbCommand("SELECT * FROM tblCons ORDER BY PositionID ASC");
            DatabaseConnector.AssociateItemToThisConnector(dbQuery);
            dbQuery.Transaction = transaction;
            if (!DatabaseConnector.IsConnected)
                OpenConnection();
            System.Data.Common.DbDataReader dbDataReader = dbQuery.ExecuteReader(transaction == null ? CommandBehavior.CloseConnection : CommandBehavior.Default);

            if (dbDataReader.HasRows)
                dataTable.Load(dbDataReader);
            dbDataReader.Close();
            return dataTable;
        }

        public void Save(DataTable dataTable)
        {
            Save(dataTable, null);
        }

        public void Save(DataTable dataTable, DbTransaction transaction)
        {
            if (DbUserIsReadOnly())
            {
                throw new InvalidOperationException("SQL writes are disabled while the read-only option is enabled.");
            }

            if (!DatabaseConnector.IsConnected)
                OpenConnection();
            if (DatabaseConnector.GetType() == typeof(MSSqlDatabaseConnector))
            {
                SqlConnection sqlConnection = (SqlConnection)DatabaseConnector.DbConnection();
                using SqlTransaction ownedTransaction = transaction == null ? sqlConnection.BeginTransaction(System.Data.IsolationLevel.Serializable) : null;
                using SqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.Transaction = transaction as SqlTransaction ?? ownedTransaction;
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
                dataAdapter.UpdateCommand.Transaction = sqlCommand.Transaction;
                dataAdapter.DeleteCommand.Transaction = sqlCommand.Transaction;
                dataAdapter.InsertCommand.Transaction = sqlCommand.Transaction;
                dataAdapter.Update(dataTable);
                ownedTransaction?.Commit();
            }
            else if (DatabaseConnector.GetType() == typeof(MySqlDatabaseConnector))
            {
                MySqlConnection dbConnection = (MySqlConnection) DatabaseConnector.DbConnection();
                using MySqlTransaction ownedTransaction = transaction == null ? dbConnection.BeginTransaction(System.Data.IsolationLevel.Serializable) : null;
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = dbConnection;
                sqlCommand.Transaction = transaction as MySqlTransaction ?? ownedTransaction;
                sqlCommand.CommandText = "SELECT * FROM tblCons";
                using MySqlDataAdapter dataAdapter = new(sqlCommand);
                dataAdapter.SelectCommand.Transaction = transaction as MySqlTransaction ?? ownedTransaction;
                dataAdapter.UpdateBatchSize = 1000;
                using MySqlCommandBuilder cb = new(dataAdapter);
                dataAdapter.UpdateCommand = cb.GetUpdateCommand();
                dataAdapter.DeleteCommand = cb.GetDeleteCommand();
                dataAdapter.InsertCommand = cb.GetInsertCommand();
                dataAdapter.UpdateCommand.Transaction = sqlCommand.Transaction;
                dataAdapter.DeleteCommand.Transaction = sqlCommand.Transaction;
                dataAdapter.InsertCommand.Transaction = sqlCommand.Transaction;
                dataAdapter.Update(dataTable);
                ownedTransaction?.Commit();
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