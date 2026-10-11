using System;
using System.Data.Common;
using mRemoteNG.App;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;

namespace mRemoteNG.Config.Serializers.Versioning
{
    internal static class SqlVersionUpgradeExecutor
    {
        public static Version Upgrade(IDatabaseConnector databaseConnector, Version version, string msSqlAlter, string mySqlAlter)
        {
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, $"Upgrading database to version {version}.");

            const string mySqlUpdate = @"SET SQL_SAFE_UPDATES=0; UPDATE tblRoot SET ConfVersion=?; SET SQL_SAFE_UPDATES=1;";
            const string msSqlUpdate = @"UPDATE tblRoot SET ConfVersion=@confVersion;";

            using DbTransaction sqlTran = databaseConnector.DbConnection().BeginTransaction(System.Data.IsolationLevel.Serializable);
            DbCommand dbCommand;
            if (databaseConnector.GetType() == typeof(MSSqlDatabaseConnector))
            {
                dbCommand = databaseConnector.DbCommand(msSqlAlter);
                dbCommand.Transaction = sqlTran;
                dbCommand.ExecuteNonQuery();
                dbCommand = databaseConnector.DbCommand(msSqlUpdate);
                dbCommand.Transaction = sqlTran;
            }
            else if (databaseConnector.GetType() == typeof(MySqlDatabaseConnector))
            {
                dbCommand = databaseConnector.DbCommand(mySqlAlter);
                dbCommand.Transaction = sqlTran;
                dbCommand.ExecuteNonQuery();
                dbCommand = databaseConnector.DbCommand(mySqlUpdate);
                dbCommand.Transaction = sqlTran;
            }
            else
            {
                throw new Exception("Unknown database back-end");
            }

            DbParameter pConfVersion = dbCommand.CreateParameter();
            pConfVersion.ParameterName = "confVersion";
            pConfVersion.Value = version.ToString();
            pConfVersion.DbType = System.Data.DbType.String;
            pConfVersion.Direction = System.Data.ParameterDirection.Input;
            dbCommand.Parameters.Add(pConfVersion);
            dbCommand.ExecuteNonQuery();
            sqlTran.Commit();

            return version;
        }
    }
}
