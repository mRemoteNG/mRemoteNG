using System;
using System.Runtime.Versioning;
using mRemoteNG.Config.DatabaseConnectors;

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
            const string mySqlAlter = @"
ALTER TABLE tblCons ADD COLUMN `TerminalBackend` varchar(32) DEFAULT NULL;
ALTER TABLE tblCons ADD COLUMN `InheritTerminalBackend` tinyint DEFAULT NULL;
SET SQL_SAFE_UPDATES=0;
UPDATE tblCons SET TerminalBackend='PuttyNg' WHERE TerminalBackend IS NULL;
UPDATE tblCons SET InheritTerminalBackend=0 WHERE InheritTerminalBackend IS NULL;
ALTER TABLE tblCons MODIFY COLUMN InheritTerminalBackend tinyint NOT NULL;
SET SQL_SAFE_UPDATES=1;";
            const string msSqlAlter = @"
ALTER TABLE tblCons ADD TerminalBackend varchar(32) NULL;
ALTER TABLE tblCons ADD InheritTerminalBackend bit NULL;
UPDATE tblCons SET TerminalBackend='PuttyNg' WHERE TerminalBackend IS NULL;
UPDATE tblCons SET InheritTerminalBackend=0 WHERE InheritTerminalBackend IS NULL;
ALTER TABLE tblCons ALTER COLUMN InheritTerminalBackend bit NOT NULL;";
            return SqlVersionUpgradeExecutor.Upgrade(_databaseConnector, _version, msSqlAlter, mySqlAlter);
        }
    }
}
