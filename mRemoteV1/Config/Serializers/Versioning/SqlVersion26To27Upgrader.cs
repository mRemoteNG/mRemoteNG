using mRemoteNG.App;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;
using System;

namespace mRemoteNG.Config.Serializers.Versioning
{
    public class SqlVersion26To27Upgrader : IVersionUpgrader
    {
        private readonly IDatabaseConnector _databaseConnector;

        public SqlVersion26To27Upgrader(IDatabaseConnector databaseConnector)
        {
            if (databaseConnector == null)
                throw new ArgumentNullException(nameof(databaseConnector));

            _databaseConnector = databaseConnector;
        }

        public bool CanUpgrade(Version currentVersion)
        {
            return currentVersion.CompareTo(new Version(2, 6)) == 0;
        }

        public Version Upgrade()
        {
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                "Upgrading database from version 2.6 to version 2.7.");
            const string sqlText = @"
ALTER TABLE tblCons
ADD RedirectClipboard bit NOT NULL DEFAULT 0,
	InheritRedirectClipboard bit NOT NULL DEFAULT 0
DROP COLUMN Username, Domain, Password, InheritUsername, InheritDomain, InheritPassword, Expanded, Connected;
UPDATE tblRoot
    SET ConfVersion='2.7'";
            var dbCommand = _databaseConnector.DbCommand(sqlText);
            dbCommand.ExecuteNonQuery();

            return new Version(2, 7);
        }
    }
}