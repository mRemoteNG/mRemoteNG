using System;
using System.Data.Common;
using System.Runtime.Versioning;
using mRemoteNG.App;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;

namespace mRemoteNG.Config.Serializers.Versioning
{
    [SupportedOSPlatform("windows")]
    public class SqlVersion31To32Upgrader(IDatabaseConnector databaseConnector) : IVersionUpgrader
    {
        private readonly Version _version = new(3, 2);
        private readonly IDatabaseConnector _databaseConnector = databaseConnector ?? throw new ArgumentNullException(nameof(databaseConnector));

        public bool CanUpgrade(Version currentVersion)
        {
            return currentVersion <= _version && currentVersion < _version;
        }

        public Version Upgrade()
        {
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                $"Upgrading database to version {_version}.");

            const string mySqlAlter = @"
ALTER TABLE tblCons ADD COLUMN `TerminalBackend` varchar(32) DEFAULT NULL;
ALTER TABLE tblCons ADD COLUMN `InheritTerminalBackend` tinyint DEFAULT NULL;
SET SQL_SAFE_UPDATES=0;
UPDATE tblCons SET TerminalBackend='PuttyNg' WHERE TerminalBackend IS NULL;
UPDATE tblCons SET InheritTerminalBackend=0 WHERE InheritTerminalBackend IS NULL;
ALTER TABLE tblCons MODIFY COLUMN InheritTerminalBackend tinyint NOT NULL;
SET SQL_SAFE_UPDATES=1;";
            const string mySqlUpdate = @"SET SQL_SAFE_UPDATES=0; UPDATE tblRoot SET ConfVersion=?; SET SQL_SAFE_UPDATES=1;";
            const string msSqlAlter = @"
ALTER TABLE tblCons ADD TerminalBackend varchar(32) NULL;
ALTER TABLE tblCons ADD InheritTerminalBackend bit NULL;
UPDATE tblCons SET TerminalBackend='PuttyNg' WHERE TerminalBackend IS NULL;
UPDATE tblCons SET InheritTerminalBackend=0 WHERE InheritTerminalBackend IS NULL;
ALTER TABLE tblCons ALTER COLUMN InheritTerminalBackend bit NOT NULL;";
            const string msSqlUpdate = @"UPDATE tblRoot SET ConfVersion=@confVersion;";

            using DbTransaction sqlTran = _databaseConnector.DbConnection().BeginTransaction(System.Data.IsolationLevel.Serializable);
            DbCommand dbCommand;
            if (_databaseConnector.GetType() == typeof(MSSqlDatabaseConnector))
            {
                dbCommand = _databaseConnector.DbCommand(msSqlAlter);
                dbCommand.Transaction = sqlTran;
                dbCommand.ExecuteNonQuery();
                dbCommand = _databaseConnector.DbCommand(msSqlUpdate);
                dbCommand.Transaction = sqlTran;
            }
            else if (_databaseConnector.GetType() == typeof(MySqlDatabaseConnector))
            {
                dbCommand = _databaseConnector.DbCommand(mySqlAlter);
                dbCommand.Transaction = sqlTran;
                dbCommand.ExecuteNonQuery();
                dbCommand = _databaseConnector.DbCommand(mySqlUpdate);
                dbCommand.Transaction = sqlTran;
            }
            else
            {
                throw new Exception("Unknown database back-end");
            }

            DbParameter pConfVersion = dbCommand.CreateParameter();
            pConfVersion.ParameterName = "confVersion";
            pConfVersion.Value = _version.ToString();
            pConfVersion.DbType = System.Data.DbType.String;
            pConfVersion.Direction = System.Data.ParameterDirection.Input;
            dbCommand.Parameters.Add(pConfVersion);
            dbCommand.ExecuteNonQuery();
            sqlTran.Commit();

            return _version;
        }
    }
}
