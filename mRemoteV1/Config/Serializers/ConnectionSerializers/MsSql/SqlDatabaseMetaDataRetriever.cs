using System;
using System.Data.SqlClient;
using System.Globalization;
using mRemoteNG.App;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;

namespace mRemoteNG.Config.Serializers.MsSql
{
    public class SqlDatabaseMetaDataRetriever
    {
        public SqlConnectionListMetaData GetDatabaseMetaData(SqlDatabaseConnector sqlDatabaseConnector)
        {
            SqlConnectionListMetaData metaData;
            SqlDataReader sqlDataReader = null;
            try
            {
                var sqlCommand = new SqlCommand("SELECT * FROM tblRoot", sqlDatabaseConnector.SqlConnection);
                if (!sqlDatabaseConnector.IsConnected)
                    sqlDatabaseConnector.Connect();
                sqlDataReader = sqlCommand.ExecuteReader();
                if (!sqlDataReader.HasRows)
                    return null; // assume new empty database
                else
                    sqlDataReader.Read();

                metaData = new SqlConnectionListMetaData
                {
                    Name = sqlDataReader["Name"] as string ?? "",
                    Protected = sqlDataReader["Protected"] as string ?? "",
                    Export = (bool)sqlDataReader["Export"],
                    ConfVersion = new Version(Convert.ToString(sqlDataReader["confVersion"], CultureInfo.InvariantCulture))
                };
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, $"Retrieving database version failed. {ex}");
                throw;
            }
            finally
            {
                if (sqlDataReader != null && !sqlDataReader.IsClosed)
                    sqlDataReader.Close();
            }
            return metaData;
        }
    }
}