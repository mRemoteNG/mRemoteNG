using System;
using System.Data.SqlClient;
using mRemoteNG.App;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;

namespace mRemoteNG.Config.Serializers.Versioning
{
    public class SqlVersion22To23Upgrader : IVersionUpgrader
    {
        private readonly SqlDatabaseConnector _sqlDatabaseConnector;

        public SqlVersion22To23Upgrader(SqlDatabaseConnector sqlDatabaseConnector)
        {
            if (sqlDatabaseConnector == null)
                throw new ArgumentNullException(nameof(sqlDatabaseConnector));

            _sqlDatabaseConnector = sqlDatabaseConnector;
        }

        public bool CanUpgrade(Version currentVersion)
        {
            return currentVersion.CompareTo(new Version(2, 2)) == 0;
        }

        public void Upgrade()
        {
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Upgrading database from version 2.2 to version 2.3.");
            const string sqlText = @"
ALTER TABLE tblCons
ADD EnableFontSmoothing bit NOT NULL DEFAULT 0,
    EnableDesktopComposition bit NOT NULL DEFAULT 0, 
    InheritEnableFontSmoothing bit NOT NULL DEFAULT 0, 
    InheritEnableDesktopComposition bit NOT NULL DEFAULT 0;";
            var sqlCommand = new SqlCommand(sqlText, _sqlDatabaseConnector.SqlConnection);
            sqlCommand.ExecuteNonQuery();
        }
    }
}