using mRemoteNG.App;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;
using System;
using System.Runtime.Versioning;

namespace mRemoteNG.Config.Serializers.Versioning
{
    [SupportedOSPlatform("windows")]
    public class SqlVersion23To24Upgrader : IVersionUpgrader
    {
        private readonly IDatabaseConnector _databaseConnector;

        public SqlVersion23To24Upgrader(IDatabaseConnector databaseConnector)
        {
            if (databaseConnector == null)
                throw new ArgumentNullException(nameof(databaseConnector));

            _databaseConnector = databaseConnector;
        }

        public bool CanUpgrade(Version currentVersion)
        {
            return currentVersion.CompareTo(new Version(2, 3)) == 0;
        }

        public Version Upgrade()
        {
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                "Upgrading database from version 2.3 to version 2.4.");
            const string sqlText = @"
ALTER TABLE tblCons
ADD UseCredSsp bit NOT NULL DEFAULT 1,
    InheritUseCredSsp bit NOT NULL DEFAULT 0;";
            var dbCommand = _databaseConnector.DbCommand(sqlText);
            dbCommand.ExecuteNonQuery();

            return new Version(2, 4);
        }
    }
}