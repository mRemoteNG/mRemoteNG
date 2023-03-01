using mRemoteNG.App;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;
using System;
using System.Data.Common;

namespace mRemoteNG.Config.Serializers.Versioning
{
    public class SqlVersion28To29Upgrader : IVersionUpgrader
    {
        private readonly Version version = new Version(2, 9);
        private readonly IDatabaseConnector _databaseConnector;

        public SqlVersion28To29Upgrader(IDatabaseConnector databaseConnector)
        {
            _databaseConnector = databaseConnector ?? throw new ArgumentNullException(nameof(databaseConnector));
        }

        public bool CanUpgrade(Version currentVersion)
        {
            return currentVersion == new Version(2, 8) ||
                // Support upgrading during dev revisions, 2.9.1, 2.9.2, etc...
                (currentVersion <= new Version(2, 9) &&
                currentVersion < version);
        }

        public Version Upgrade()
        {
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                $"Upgrading database to version {version}.");

            // MYSQL
            const string mySqlAlter = @"
ALTER TABLE tblCons ADD COLUMN
    OpeningCommand VARCHAR(512) DEFAULT NULL,
    InheritExternalCredentialProvider bit NOT NULL DEFAULT 0,
    InheritUseRCG bit NOT NULL DEFAULT 0,
    InheritUserViaAPI bit NOT NULL DEFAULT 0;
ALTER TABLE tblRoot CHANGE COLUMN ConfVersion ConfVersion VARCHAR(15) NOT NULL;";

            const string mySqlUpdate = @"UPDATE tblRoot SET ConfVersion=?;";

            // MS-SQL
            const string msSqlAlter1 = @"
ALTER TABLE tblCons ADD 
    InheritExternalCredentialProvider bit NOT NULL DEFAULT 0,
    InheritUseRCG bit NOT NULL DEFAULT 0,
    InheritUserViaAPI bit NOT NULL DEFAULT 0;
";
            const string msSqlAlter2 = @"
ALTER TABLE tblRoot ALTER COLUMN ConfVersion VARCHAR(15)";

            const string msSqlUpdate = @"UPDATE tblRoot SET ConfVersion=@confVersion;";

            using (var sqlTran = _databaseConnector.DbConnection().BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
                DbCommand dbCommand;
                if (_databaseConnector.GetType() == typeof(MSSqlDatabaseConnector))
                {
                    dbCommand = _databaseConnector.DbCommand(msSqlAlter1);
                    dbCommand.Transaction = sqlTran;
                    dbCommand.ExecuteNonQuery();
                    dbCommand = _databaseConnector.DbCommand(msSqlAlter2);
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