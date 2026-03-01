using System.Data;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;
using mRemoteNG.App;
using MySql.Data.MySqlClient;
using Microsoft.Data.SqlClient;
using System.Data.Odbc;
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
            using System.Data.Common.DbDataReader dbDataReader = dbQuery.ExecuteReader();
            // Always load the reader so table schema is available even when tblCons has 0 rows.
            // Note: CommandBehavior.CloseConnection must NOT be used here because Load() is
            // called inside an open transaction (SqlConnectionsSaver.Save). Closing the
            // connection mid-transaction rolls back the transaction in MySQL, causing all
            // subsequent INSERT/UPDATE operations to fail with a stale transaction (#2290).
            dataTable.Load(dbDataReader);
            return dataTable;
        }

        public void Save(DataTable dataTable)
        {
            Save(dataTable, null);
        }

        public void Save(DataTable dataTable, System.Data.Common.DbTransaction? transaction)
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
                SqlTransaction? sqlTransaction = (SqlTransaction?)transaction;
                bool mustDisposeTransaction = false;

                if (sqlTransaction == null)
                {
                    sqlTransaction = sqlConnection.BeginTransaction(System.Data.IsolationLevel.Serializable);
                    mustDisposeTransaction = true;
                }

                try
                {
                    using SqlCommand sqlCommand = new();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.CommandText = "SELECT * FROM tblCons";
                    using SqlDataAdapter dataAdapter = new();
                    dataAdapter.SelectCommand = sqlCommand;

                    ConflictOption conflictOption = ConflictOption.OverwriteChanges;
                    if (dataTable.Columns.Contains("RowVersion"))
                        conflictOption = ConflictOption.CompareRowVersion;

                    SqlCommandBuilder builder = new(dataAdapter)
                    {
                        // Avoid optimistic concurrency, check if it is necessary.
                        ConflictOption = conflictOption
                    };

                    dataAdapter.UpdateCommand = builder.GetUpdateCommand();
                    dataAdapter.DeleteCommand = builder.GetDeleteCommand();
                    dataAdapter.InsertCommand = builder.GetInsertCommand();
                    dataAdapter.Update(dataTable);

                    if (mustDisposeTransaction)
                    {
                        sqlTransaction.Commit();
                    }
                }
                catch (DBConcurrencyException ex)
                {
                    Runtime.MessageCollector.AddExceptionStackTrace("Database concurrency conflict detected. Please reload connections.", ex);
                    throw;
                }
                finally
                {
                    if (mustDisposeTransaction)
                    {
                        sqlTransaction.Dispose();
                    }
                }
            }
            else if (DatabaseConnector.GetType() == typeof(MySqlDatabaseConnector))
            {
                MySqlConnection dbConnection = (MySqlConnection)DatabaseConnector.DbConnection();
                MySqlTransaction? mySqlTransaction = (MySqlTransaction?)transaction;
                bool mustDisposeTransaction = false;

                if (mySqlTransaction == null)
                {
                    mySqlTransaction = dbConnection.BeginTransaction(System.Data.IsolationLevel.Serializable);
                    mustDisposeTransaction = true;
                }

                try
                {
                    using MySqlCommand sqlCommand = new();
                    sqlCommand.Connection = dbConnection;
                    sqlCommand.Transaction = mySqlTransaction;
                    sqlCommand.CommandText = "SELECT * FROM tblCons";
                    using MySqlDataAdapter dataAdapter = new(sqlCommand);
                    dataAdapter.UpdateBatchSize = 1000;
                    using MySqlCommandBuilder cb = new(dataAdapter);

                    ConflictOption conflictOption = ConflictOption.OverwriteChanges;
                    if (dataTable.Columns.Contains("RowVersion"))
                        conflictOption = ConflictOption.CompareRowVersion;
                    cb.ConflictOption = conflictOption;
                    // Quote column names with backticks so the MySqlCommandBuilder internal
                    // parameter-to-column dictionary lookup succeeds on MariaDB/MySQL.
                    // Without this, GetUpdateCommand/GetDeleteCommand/GetInsertCommand throw
                    // "Given key was not present in dictionary" (#2257).
                    cb.QuotePrefix = "`";
                    cb.QuoteSuffix = "`";

                    // Explicitly retrieve commands after setting ConflictOption so the
                    // generated UPDATE/DELETE/INSERT use only the primary key in their
                    // WHERE clause (OverwriteChanges semantics). Without this, the adapter
                    // auto-generates commands at update time and may ignore the option,
                    // causing DBConcurrencyException in multi-user environments (#1934).
                    dataAdapter.UpdateCommand = cb.GetUpdateCommand();
                    dataAdapter.DeleteCommand = cb.GetDeleteCommand();
                    dataAdapter.InsertCommand = cb.GetInsertCommand();
                    dataAdapter.Update(dataTable);

                    if (mustDisposeTransaction)
                    {
                        mySqlTransaction.Commit();
                    }
                }
                catch (DBConcurrencyException ex)
                {
                    Runtime.MessageCollector.AddExceptionStackTrace("Database concurrency conflict detected. Please reload connections.", ex);
                    throw;
                }
                finally
                {
                    if (mustDisposeTransaction)
                    {
                        mySqlTransaction.Dispose();
                    }
                }
            }
            else if (DatabaseConnector.GetType() == typeof(OdbcDatabaseConnector))
            {
                OdbcConnection dbConnection = (OdbcConnection)DatabaseConnector.DbConnection();
                OdbcTransaction? odbcTransaction = (OdbcTransaction?)transaction;
                bool mustDisposeTransaction = false;

                if (odbcTransaction == null)
                {
                    odbcTransaction = dbConnection.BeginTransaction(System.Data.IsolationLevel.Serializable);
                    mustDisposeTransaction = true;
                }

                try
                {
                    using OdbcCommand sqlCommand = new();
                    sqlCommand.Connection = dbConnection;
                    sqlCommand.Transaction = odbcTransaction;
                    sqlCommand.CommandText = "SELECT * FROM tblCons";
                    using OdbcDataAdapter dataAdapter = new(sqlCommand);

                    OdbcCommandBuilder builder = new(dataAdapter)
                    {
                        // Avoid optimistic concurrency, check if it is necessary.
                        ConflictOption = ConflictOption.OverwriteChanges
                    };

                    dataAdapter.UpdateCommand = builder.GetUpdateCommand();
                    dataAdapter.DeleteCommand = builder.GetDeleteCommand();
                    dataAdapter.InsertCommand = builder.GetInsertCommand();
                    dataAdapter.Update(dataTable);

                    if (mustDisposeTransaction)
                    {
                        odbcTransaction.Commit();
                    }
                }
                catch (DBConcurrencyException ex)
                {
                    Runtime.MessageCollector.AddExceptionStackTrace("Database concurrency conflict detected. Please reload connections.", ex);
                    throw;
                }
                finally
                {
                    if (mustDisposeTransaction)
                    {
                        odbcTransaction.Dispose();
                    }
                }
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

        private static bool DbUserIsReadOnly()
        {
            return Properties.OptionsDBsPage.Default.SQLReadOnly;
        }
    }
}
