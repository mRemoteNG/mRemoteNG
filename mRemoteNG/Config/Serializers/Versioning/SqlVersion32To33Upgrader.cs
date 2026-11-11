using System;
using System.Runtime.Versioning;
using mRemoteNG.Config.DatabaseConnectors;

namespace mRemoteNG.Config.Serializers.Versioning
{
    [SupportedOSPlatform("windows")]
    public class SqlVersion32To33Upgrader(IDatabaseConnector databaseConnector) : IVersionUpgrader
    {
        private readonly Version _version = new(3, 3);
        private readonly IDatabaseConnector _databaseConnector = databaseConnector ?? throw new ArgumentNullException(nameof(databaseConnector));

        public bool CanUpgrade(Version currentVersion)
        {
            return currentVersion <= _version && currentVersion < _version;
        }

        public Version Upgrade()
        {
            const string mySqlAlter = @"
ALTER TABLE tblCons ADD COLUMN `RedirectWebAuthn` tinyint NOT NULL DEFAULT 0;
ALTER TABLE tblCons ADD COLUMN `InheritRedirectWebAuthn` tinyint NOT NULL DEFAULT 0;";
            const string msSqlAlter = @"
ALTER TABLE tblCons ADD RedirectWebAuthn bit NOT NULL DEFAULT 0;
ALTER TABLE tblCons ADD InheritRedirectWebAuthn bit NOT NULL DEFAULT 0;";
            return SqlVersionUpgradeExecutor.Upgrade(_databaseConnector, _version, msSqlAlter, mySqlAlter);
        }
    }
}
