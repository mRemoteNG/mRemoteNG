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
                                                string.Format("Upgrading database to version {0}.", version));

            const string mySqlAlter = @"
ALTER TABLE tblCons ADD COLUMN StartProgram varchar(512) DEFAULT NULL;
ALTER TABLE tblCons ADD COLUMN StartProgramWorkDir varchar(512) DEFAULT NULL;
ALTER TABLE tblRoot CHANGE COLUMN ConfVersion ConfVersion VARCHAR(15) NOT NULL;";
            const string mySqlUpdate = @"UPDATE tblRoot SET ConfVersion=?;";
            const string msSqlAlter = @"
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[tblCons]') AND name = 'StartProgram')
BEGIN
 ALTER TABLE tblCons ADD StartProgram varchar(512), StartProgramWorkDir varchar(512);
END;GO;
ALTER TABLE tblRoot MODIFY COLUMN ConfVersion varchar(15);GO;";
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