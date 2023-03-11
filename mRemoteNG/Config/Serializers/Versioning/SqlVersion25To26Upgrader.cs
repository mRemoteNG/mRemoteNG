using mRemoteNG.App;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;
using System;
using System.Runtime.Versioning;

namespace mRemoteNG.Config.Serializers.Versioning
{
    [SupportedOSPlatform("windows")]
    public class SqlVersion25To26Upgrader : IVersionUpgrader
    {
        private readonly IDatabaseConnector _databaseConnector;

        public SqlVersion25To26Upgrader(IDatabaseConnector databaseConnector)
        {
            if (databaseConnector == null)
                throw new ArgumentNullException(nameof(databaseConnector));

            _databaseConnector = databaseConnector;
        }

        public bool CanUpgrade(Version currentVersion)
        {
            return currentVersion.CompareTo(new Version(2, 5)) == 0;
        }

        public Version Upgrade()
        {
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                "Upgrading database from version 2.5 to version 2.6.");
            const string sqlText = @"
ALTER TABLE tblCons
ADD RDPMinutesToIdleTimeout int NOT NULL DEFAULT 0,
    RDPAlertIdleTimeout bit NOT NULL DEFAULT 0,
	SoundQuality varchar (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT 'Dynamic',
	InheritRDPMinutesToIdleTimeout bit NOT NULL DEFAULT 0,
	InheritRDPAlertIdleTimeout bit NOT NULL DEFAULT 0,
	InheritSoundQuality bit NOT NULL DEFAULT 0;
UPDATE tblRoot
    SET ConfVersion='2.6'";
            var dbCommand = _databaseConnector.DbCommand(sqlText);
            dbCommand.ExecuteNonQuery();

            return new Version(2, 6);
        }
    }
}