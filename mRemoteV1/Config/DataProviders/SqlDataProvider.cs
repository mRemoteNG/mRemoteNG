using System.Data;
using System.Data.SqlClient;
using mRemoteNG.Config.DatabaseConnectors;


namespace mRemoteNG.Config.DataProviders
{
    public class SqlDataProvider : IDataProvider<DataTable>
    {
        public SqlDatabaseConnector SqlDatabaseConnector { get; }

        public SqlDataProvider(SqlDatabaseConnector sqlDatabaseConnector)
        {
            SqlDatabaseConnector = sqlDatabaseConnector;
        }

        public DataTable Load()
        {
            var dataTable = new DataTable();
            var sqlQuery = new SqlCommand("SELECT * FROM tblCons ORDER BY PositionID ASC");
            SqlDatabaseConnector.AssociateItemToThisConnector(sqlQuery);
            if (!SqlDatabaseConnector.IsConnected)
                OpenConnection();
            var sqlDataReader = sqlQuery.ExecuteReader(CommandBehavior.CloseConnection);

            if (sqlDataReader.HasRows)
                dataTable.Load(sqlDataReader);
            sqlDataReader.Close();
            return dataTable;
        }

        public void Save(DataTable dataTable)
        {
            if (!SqlDatabaseConnector.IsConnected)
                OpenConnection();
            using (var sqlBulkCopy = new SqlBulkCopy(SqlDatabaseConnector.SqlConnection))
            {
                foreach (DataColumn col in dataTable.Columns)
                    sqlBulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                sqlBulkCopy.DestinationTableName = "dbo.tblCons";
                sqlBulkCopy.WriteToServer(dataTable);
            }
        }

        public void OpenConnection()
        {
            SqlDatabaseConnector.Connect();
        }

        public void CloseConnection()
        {
            SqlDatabaseConnector.Disconnect();
        }
    }
}