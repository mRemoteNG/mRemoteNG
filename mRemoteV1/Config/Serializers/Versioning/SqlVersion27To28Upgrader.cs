using mRemoteNG.App;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;
using System;

namespace mRemoteNG.Config.Serializers.Versioning
{
    public class SqlVersion27To28Upgrader : IVersionUpgrader
    {
        private readonly IDatabaseConnector _databaseConnector;

        public SqlVersion27To28Upgrader(IDatabaseConnector databaseConnector)
        {
            _databaseConnector = databaseConnector ?? throw new ArgumentNullException(nameof(databaseConnector));
        }

        public bool CanUpgrade(Version currentVersion)
        {
            return currentVersion.CompareTo(new Version(2, 7)) == 0;
        }

        public Version Upgrade()
        {
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                "Upgrading database from version 2.7 to version 2.8.");
            const string sqlText = @"
ALTER TABLE tblCons
ADD SSHTunnelConnectionName varchar NOT NULL DEFAULT '',
    InheritSSHTunnelConnectionName bit NOT NULL DEFAULT 0,
    SSHOptions varchar NOT NULL DEFAULT '',
    InheritSSHOptions bit NOT NULL DEFAULT 0;
UPDATE tblRoot
    SET ConfVersion='2.8'";
            var dbCommand = _databaseConnector.DbCommand(sqlText);
            dbCommand.ExecuteNonQuery();

            return new Version(2, 8);
        }
    }
}