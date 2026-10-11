using System;
using System.Runtime.Versioning;
using mRemoteNG.Config.DatabaseConnectors;

namespace mRemoteNG.Config.Serializers.Versioning
{
    [SupportedOSPlatform("windows")]
    public class SqlVersion30To31Upgrader(IDatabaseConnector databaseConnector) : IVersionUpgrader
    {
        private readonly Version _version = new(3, 1);
        private readonly IDatabaseConnector _databaseConnector = databaseConnector ?? throw new ArgumentNullException(nameof(databaseConnector));

        public bool CanUpgrade(Version currentVersion)
        {
            return currentVersion == new Version(3, 0) || (currentVersion <= _version && currentVersion < _version);
        }

        public Version Upgrade()
        {
            const string mySqlAlter = @"ALTER TABLE tblCons ADD COLUMN `PluginData` longtext DEFAULT NULL;";
            const string msSqlAlter = @"ALTER TABLE tblCons ADD [PluginData] varchar(max) NULL;";
            return SqlVersionUpgradeExecutor.Upgrade(_databaseConnector, _version, msSqlAlter, mySqlAlter);
        }
    }
}
