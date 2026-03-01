using mRemoteNG.App;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;
using System;
using System.Data.Common;
using System.Runtime.Versioning;

namespace mRemoteNG.Config.Serializers.Versioning
{
    [SupportedOSPlatform("windows")]
    public class SqlVersion30To31Upgrader(IDatabaseConnector databaseConnector) : IVersionUpgrader
    {
        private readonly Version _version = new(3, 1);
        private readonly IDatabaseConnector _databaseConnector = databaseConnector ?? throw new ArgumentNullException(nameof(databaseConnector));

        public bool CanUpgrade(Version currentVersion)
        {
            return currentVersion == new Version(3, 0) ||
                // Support upgrading during dev revisions, 3.0.1, 3.0.2, etc...
                (currentVersion <= new Version(3, 1) &&
                currentVersion < _version);
        }

        public Version Upgrade()
        {
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                $"Upgrading database to version {_version}.");

            // MYSQL
            const string mySqlAlter = @"
CREATE TABLE IF NOT EXISTS `tblExternalTools` (
    `ID` int NOT NULL AUTO_INCREMENT,
    `DisplayName` varchar(256) NOT NULL,
    `FileName` varchar(1024) NOT NULL,
    `Arguments` varchar(2048) NOT NULL DEFAULT '',
    `WorkingDir` varchar(1024) NOT NULL DEFAULT '',
    `WaitForExit` tinyint NOT NULL DEFAULT 0,
    `TryIntegrate` tinyint NOT NULL DEFAULT 0,
    `RunElevated` tinyint NOT NULL DEFAULT 0,
    `ShowOnToolbar` tinyint NOT NULL DEFAULT 1,
    `Category` varchar(256) NOT NULL DEFAULT '',
    `RunOnStartup` tinyint NOT NULL DEFAULT 0,
    `StopOnShutdown` tinyint NOT NULL DEFAULT 0,
    PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
";

            const string mySqlUpdate = @"SET SQL_SAFE_UPDATES=0; UPDATE tblRoot SET ConfVersion=?; SET SQL_SAFE_UPDATES=1;";

            // MS-SQL
            const string msSqlAlter = @"
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[tblExternalTools]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
CREATE TABLE [dbo].[tblExternalTools] (
    [ID] int NOT NULL IDENTITY(1,1),
    [DisplayName] [varchar] (256) NOT NULL,
    [FileName] [varchar] (1024) NOT NULL,
    [Arguments] [varchar] (2048) NOT NULL DEFAULT '',
    [WorkingDir] [varchar] (1024) NOT NULL DEFAULT '',
    [WaitForExit] [bit] NOT NULL DEFAULT 0,
    [TryIntegrate] [bit] NOT NULL DEFAULT 0,
    [RunElevated] [bit] NOT NULL DEFAULT 0,
    [ShowOnToolbar] [bit] NOT NULL DEFAULT 1,
    [Category] [varchar] (256) NOT NULL DEFAULT '',
    [RunOnStartup] [bit] NOT NULL DEFAULT 0,
    [StopOnShutdown] [bit] NOT NULL DEFAULT 0
)
";

            const string msSqlUpdate = @"UPDATE tblRoot SET ConfVersion=@confVersion;";

            using (DbTransaction sqlTran = _databaseConnector.DbConnection().BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
                DbCommand dbCommand;
                if (_databaseConnector.GetType() == typeof(MSSqlDatabaseConnector)
                    || _databaseConnector.GetType() == typeof(OdbcDatabaseConnector))
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
                    throw new NotSupportedException("Unknown database back-end");
                }
                DbParameter pConfVersion = dbCommand.CreateParameter();
                pConfVersion.ParameterName = "confVersion";
                pConfVersion.Value = _version.ToString();
                pConfVersion.DbType = System.Data.DbType.String;
                pConfVersion.Direction = System.Data.ParameterDirection.Input;
                dbCommand.Parameters.Add(pConfVersion);

                dbCommand.ExecuteNonQuery();
                sqlTran.Commit();
            }
            return _version;
        }
    }
}
