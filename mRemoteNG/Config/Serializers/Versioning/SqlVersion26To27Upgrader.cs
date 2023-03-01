using mRemoteNG.App;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;
using System;
using System.Data.SqlClient;

namespace mRemoteNG.Config.Serializers.Versioning
{
    public class SqlVersion26To27Upgrader : IVersionUpgrader
    {
        private readonly IDatabaseConnector _databaseConnector;

        public SqlVersion26To27Upgrader(IDatabaseConnector databaseConnector)
        {
            _databaseConnector = databaseConnector ?? throw new ArgumentNullException(nameof(databaseConnector));
        }

        public bool CanUpgrade(Version currentVersion)
        {
            return currentVersion.CompareTo(new Version(2, 6)) == 0;
        }

        public Version Upgrade()
        {
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                "Upgrading database from version 2.6 to version 2.7.");
            try
            {
                const string sqlText = @"
ALTER TABLE tblCons
ADD RedirectClipboard bit NOT NULL,
	InheritRedirectClipboard bit NOT NULL,
    VmId varchar NOT NULL DEFAULT NULL,
    UseVmId bit NOT NULL,
    UseEnhancedMode bit NOT NULL,
    InheritVmId bit NOT NULL,
    InheritUseVmId bit NOT NULL,
    SSHTunnelConnectionName varchar NOT NULL DEFAULT NULL,
    InheritSSHTunnelConnectionName bit NOT NULL,
    SSHOptions varchar NOT NULL DEFAULT NULL,
    InheritSSHOptions bit NOT NULL,
    InheritUseEnhancedMode bit NOT NULL;
UPDATE tblRoot
    SET ConfVersion='2.7'";
                var dbCommand = _databaseConnector.DbCommand(sqlText);
                dbCommand.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                // no-op
            }

            return new Version(2, 7);
        }
    }
}