using System;
using System.Data;
using System.Data.SqlClient;


namespace mRemoteNG.Config.DataProviders
{
    public class SqlDataProvider : IDataProvider<DataTable>
    {
        public SqlConnection SqlConnection { get; }

        public SqlDataProvider(SqlConnection sqlConnection)
        {
            SqlConnection = sqlConnection;
        }

        ~SqlDataProvider()
        {
            SqlConnection.Dispose();
        }

        public DataTable Load()
        {
            throw new NotImplementedException();
        }

        public void Save(DataTable dataTable)
        {
            if (SqlConnection.State != ConnectionState.Open)
                OpenConnection();
            var sqlBulkCopy = new SqlBulkCopy(SqlConnection) {DestinationTableName = "dbo.tblCons"};
            sqlBulkCopy.WriteToServer(dataTable);
            sqlBulkCopy.Close();
        }

        public void OpenConnection()
        {
            SqlConnection.Open();
        }

        public void CloseConnection()
        {
            SqlConnection.Close();
        }
    }
}