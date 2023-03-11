using mRemoteNG.App;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;
using System;
using System.Runtime.Versioning;

namespace mRemoteNG.Config.Serializers.Versioning
{
    [SupportedOSPlatform("windows")]
    public class SqlVersion22To23Upgrader : IVersionUpgrader
    {
        private readonly IDatabaseConnector _databaseConnector;

        public SqlVersion22To23Upgrader(IDatabaseConnector databaseConnector)
        {
            if (databaseConnector == null)
                throw new ArgumentNullException(nameof(databaseConnector));

            _databaseConnector = databaseConnector;
        }

        public bool CanUpgrade(Version currentVersion)
        {
            return currentVersion.CompareTo(new Version(2, 2)) == 0;
        }

        public Version Upgrade()
        {
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Upgrading database from version 2.2 to version 2.3.");
            const string sqlText = @"
ALTER TABLE tblCons
ADD EnableFontSmoothing bit NOT NULL DEFAULT 0,
    EnableDesktopComposition bit NOT NULL DEFAULT 0, 
    InheritEnableFontSmoothing bit NOT NULL DEFAULT 0, 
    InheritEnableDesktopComposition bit NOT NULL DEFAULT 0;";
            var dbCommand = _databaseConnector.DbCommand(sqlText);
            dbCommand.ExecuteNonQuery();

            return new Version(2, 3);
        }
    }
}