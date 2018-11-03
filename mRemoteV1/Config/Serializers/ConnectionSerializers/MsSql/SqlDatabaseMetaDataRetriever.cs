using System;
using System.Data.Common;
using System.Globalization;
using mRemoteNG.App;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;

namespace mRemoteNG.Config.Serializers.MsSql
{
    public class SqlDatabaseMetaDataRetriever
    {
        public SqlConnectionListMetaData GetDatabaseMetaData(IDatabaseConnector databaseConnector)
        {
            SqlConnectionListMetaData metaData;
            DbDataReader dbDataReader = null;
            try
            {
                var dbCommand = databaseConnector.DbCommand("SELECT * FROM tblRoot");
                if (!databaseConnector.IsConnected)
                    databaseConnector.Connect();
                dbDataReader = dbCommand.ExecuteReader();
                if (!dbDataReader.HasRows)
                    return null; // assume new empty database
                else
                    dbDataReader.Read();

                metaData = new SqlConnectionListMetaData
                {
                    Name = dbDataReader["Name"] as string ?? "",
                    Protected = dbDataReader["Protected"] as string ?? "",
                    Export = (bool)dbDataReader["Export"],
                    ConfVersion =
                        new Version(Convert.ToString(dbDataReader["confVersion"], CultureInfo.InvariantCulture))
                };
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, $"Retrieving database version failed. {ex}");
                throw;
            }
            finally
            {
                if (dbDataReader != null && !dbDataReader.IsClosed)
                    dbDataReader.Close();
            }

            return metaData;
        }
    }
}