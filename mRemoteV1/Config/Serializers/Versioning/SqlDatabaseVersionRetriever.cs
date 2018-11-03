using System;
using System.Data.Common;
using System.Globalization;
using mRemoteNG.App;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;

namespace mRemoteNG.Config.Serializers.Versioning
{
    public class SqlDatabaseVersionRetriever
    {
        public Version GetDatabaseVersion(IDatabaseConnector databaseConnector)
        {
            Version databaseVersion;
            DbDataReader dbDataReader = null;
            try
            {
                var dbCommand = databaseConnector.DbCommand("SELECT * FROM tblRoot");
                if (!databaseConnector.IsConnected)
                    databaseConnector.Connect();
                dbDataReader = dbCommand.ExecuteReader();
                if (!dbDataReader.HasRows)
                    return new Version(); // assume new empty database
                else
                    dbDataReader.Read();
                databaseVersion = new Version(Convert.ToString(dbDataReader["confVersion"], CultureInfo.InvariantCulture));
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
            return databaseVersion;
        }
    }
}