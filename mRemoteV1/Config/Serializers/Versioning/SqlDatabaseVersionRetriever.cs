using System;
using System.Data.SqlClient;
using System.Globalization;
using mRemoteNG.App;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;

namespace mRemoteNG.Config.Serializers.Versioning
{
    public class SqlDatabaseVersionRetriever
    {
        public Version GetDatabaseVersion(SqlDatabaseConnector sqlDatabaseConnector)
        {
            Version databaseVersion;
            SqlDataReader sqlDataReader = null;
            try
            {
                var sqlCommand = new SqlCommand("SELECT * FROM tblRoot", sqlDatabaseConnector.SqlConnection);
                if (!sqlDatabaseConnector.IsConnected)
                    sqlDatabaseConnector.Connect();
                sqlDataReader = sqlCommand.ExecuteReader();
                if (!sqlDataReader.HasRows)
                    return new Version(); // assume new empty database
                else
                    sqlDataReader.Read();
                databaseVersion = new Version(Convert.ToString(sqlDataReader["confVersion"], CultureInfo.InvariantCulture));
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
            return databaseVersion;
        }
    }
}