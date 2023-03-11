using mRemoteNG.App;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;
using System;
using System.Data.Common;
using System.Runtime.Versioning;

namespace mRemoteNG.Config.Serializers.Versioning
{
    [SupportedOSPlatform("windows")]
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
ALTER TABLE tblCons ADD COLUMN `InheritUseRestrictedAdmin` tinyint(1) NOT NULL;
ALTER TABLE tblCons ADD COLUMN `UseRCG` tinyint(1) NOT NULL;
ALTER TABLE tblCons ADD COLUMN `UseRestrictedAdmin` tinyint(1) NOT NULL;
ALTER TABLE tblCons ADD COLUMN `InheritUseRCG` tinyint(1) NOT NULL;
ALTER TABLE tblCons ADD COLUMN `InheritRDGatewayExternalCredentialProvider` tinyint(1) NOT NULL;
ALTER TABLE tblCons ADD COLUMN `InheritRDGatewayUserViaAPI` tinyint(1) NOT NULL;
ALTER TABLE tblCons ADD COLUMN `InheritExternalCredentialProvider` tinyint(1) NOT NULL;
ALTER TABLE tblCons ADD COLUMN `InheritUserViaAPI` tinyint(1) NOT NULL;
ALTER TABLE tblCons ADD COLUMN `EC2Region` varchar(32) DEFAULT NULL;
ALTER TABLE tblCons ADD COLUMN `EC2InstanceId` varchar(32) DEFAULT NULL;
ALTER TABLE tblCons ADD COLUMN `ExternalCredentialProvider` varchar(256) DEFAULT NULL;
ALTER TABLE tblCons ADD COLUMN `ExternalAddressProvider` varchar(256) DEFAULT NULL;
SET SQL_SAFE_UPDATES=0;
UPDATE tblCons SET InheritUseEnhancedMode = 0 WHERE InheritUseEnhancedMode IS NULL;
ALTER TABLE tblCons MODIFY COLUMN InheritUseEnhancedMode tinyint(1) NOT NULL;
UPDATE tblCons SET UseEnhancedMode = 0 WHERE UseEnhancedMode IS NULL;
ALTER TABLE tblCons MODIFY COLUMN UseEnhancedMode tinyint(1) NOT NULL;
UPDATE tblCons SET InheritVmId = 0 WHERE InheritVmId IS NULL;
ALTER TABLE tblCons MODIFY COLUMN InheritVmId tinyint(1) NOT NULL;
UPDATE tblCons SET InheritUseVmId = 0 WHERE InheritUseVmId IS NULL;
ALTER TABLE tblCons MODIFY COLUMN InheritUseVmId tinyint(1) NOT NULL;
UPDATE tblCons SET UseVmId = 0 WHERE UseVmId IS NULL;
ALTER TABLE tblCons MODIFY COLUMN UseVmId tinyint(1) NOT NULL;
SET SQL_SAFE_UPDATES=1;
ALTER TABLE tblRoot ALTER COLUMN ConfVersion VARCHAR(15) NOT NULL;
";

            const string mySqlUpdate = @"UPDATE tblRoot SET ConfVersion=?;";

            // MS-SQL
            const string msSqlAlter = @"
ALTER TABLE tblCons ADD InheritUseRestrictedAdmin bit NOT NULL;
ALTER TABLE tblCons ADD UseRCG bit NOT NULL;
ALTER TABLE tblCons ADD UseRestrictedAdmin bit NOT NULL;
ALTER TABLE tblCons ADD InheritUseRCG bit NOT NULL;
ALTER TABLE tblCons ADD InheritRDGatewayExternalCredentialProvider bit NOT NULL;
ALTER TABLE tblCons ADD InheritRDGatewayUserViaAPI bit NOT NULL;
ALTER TABLE tblCons ADD InheritExternalCredentialProvider bit NOT NULL;
ALTER TABLE tblCons ADD InheritUserViaAPI bit NOT NULL;
ALTER TABLE tblCons ADD EC2Region varchar(32) NULL;
ALTER TABLE tblCons ADD EC2InstanceId varchar(32) NULL;
ALTER TABLE tblCons ADD ExternalCredentialProvider varchar(256) NULL;
ALTER TABLE tblCons ADD ExternalAddressProvider varchar(256) NULL;
UPDATE tblCons SET InheritUseEnhancedMode = 0 WHERE InheritUseEnhancedMode IS NULL;
ALTER TABLE tblCons ALTER COLUMN InheritUseEnhancedMode bit NOT NULL;
UPDATE tblCons SET UseEnhancedMode = 0 WHERE UseEnhancedMode IS NULL;
ALTER TABLE tblCons ALTER COLUMN UseEnhancedMode bit NOT NULL;
UPDATE tblCons SET InheritVmId = 0 WHERE InheritVmId IS NULL;
ALTER TABLE tblCons ALTER COLUMN InheritVmId bit NOT NULL;
UPDATE tblCons SET InheritUseVmId = 0 WHERE InheritUseVmId IS NULL;
ALTER TABLE tblCons ALTER COLUMN InheritUseVmId bit NOT NULL;
UPDATE tblCons SET UseVmId = 0 WHERE UseVmId IS NULL;
ALTER TABLE tblCons ALTER COLUMN UseVmId bit NOT NULL;
ALTER TABLE tblRoot ALTER COLUMN [ConfVersion] VARCHAR(15) NOT NULL;
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