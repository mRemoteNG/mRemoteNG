using mRemoteNG.App;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;
using System;
using System.Runtime.Versioning;

namespace mRemoteNG.Config.Serializers.Versioning
{
    [SupportedOSPlatform("windows")]
    public class SqlVersion24To25Upgrader : IVersionUpgrader
    {
        private readonly IDatabaseConnector _databaseConnector;

        public SqlVersion24To25Upgrader(IDatabaseConnector databaseConnector)
        {
            if (databaseConnector == null)
                throw new ArgumentNullException(nameof(databaseConnector));

            _databaseConnector = databaseConnector;
        }

        public bool CanUpgrade(Version currentVersion)
        {
            return currentVersion.CompareTo(new Version(2, 4)) == 0;
        }

        public Version Upgrade()
        {
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                "Upgrading database from version 2.4 to version 2.5.");
            const string sqlText = @"
ALTER TABLE tblCons
ADD LoadBalanceInfo varchar (1024) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    AutomaticResize bit NOT NULL DEFAULT 1,
    InheritLoadBalanceInfo bit NOT NULL DEFAULT 0,
    InheritAutomaticResize bit NOT NULL DEFAULT 0;";
            var dbCommand = _databaseConnector.DbCommand(sqlText);
            dbCommand.ExecuteNonQuery();

            return new Version(2, 5);
        }
    }
}