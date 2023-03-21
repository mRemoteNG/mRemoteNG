using mRemoteNG.App;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;
using System;
using System.Data.Common;
using System.Runtime.Versioning;

namespace mRemoteNG.Config.Serializers.Versioning
{
    [SupportedOSPlatform("windows")]
    public class SqlVersion29To30Upgrader : IVersionUpgrader
    {
        private readonly Version version = new Version(3, 0);
        private readonly IDatabaseConnector _databaseConnector;

        public SqlVersion29To30Upgrader(IDatabaseConnector databaseConnector)
        {
            _databaseConnector = databaseConnector ?? throw new ArgumentNullException(nameof(databaseConnector));
        }

        public bool CanUpgrade(Version currentVersion)
        {
            return currentVersion == new Version(2, 9) ||
                // Support upgrading during dev revisions, 2.9.1, 2.9.2, etc...
                (currentVersion <= new Version(3, 0) &&
                currentVersion < version);
        }

        public Version Upgrade()
        {
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                $"Upgrading database to version {version}.");

            // MYSQL
            const string mySqlAlter = @"
ALTER TABLE tblCons ALTER COLUMN `RedirectDiskDrives` varchar(32) DEFAULT NULL;
ALTER TABLE tblCons ADD COLUMN `RedirectDiskDrivesCustom` varchar(32) DEFAULT NULL;
ALTER TABLE tblCons ADD COLUMN `InheritRedirectDiskDrivesCustom` tinyint(1) NOT NULL;
";

            const string mySqlUpdate = @"UPDATE tblRoot SET ConfVersion=?;";

            // MS-SQL
            const string msSqlAlter = @"
ALTER TABLE tblCons ALTER COLUMN RedirectDiskDrives varchar(32) DEFAULT NULL;
ALTER TABLE tblCons ADD RedirectDiskDrivesCustom varchar(32) DEFAULT NULL;
ALTER TABLE tblCons ADD InheritRedirectDiskDrivesCustom bit NOT NULL;
";

            const string msSqlUpdate = @"UPDATE tblRoot SET ConfVersion=@confVersion;";

            using (var sqlTran = _databaseConnector.DbConnection().BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
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
                var pConfVersion = dbCommand.CreateParameter();
                pConfVersion.ParameterName = "confVersion";
                pConfVersion.Value = version.ToString();
                pConfVersion.DbType = System.Data.DbType.String;
                pConfVersion.Direction = System.Data.ParameterDirection.Input;
                dbCommand.Parameters.Add(pConfVersion);

                dbCommand.ExecuteNonQuery();
                sqlTran.Commit();
            }
            return version;
        }
    }
}