using mRemoteNG.App;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;
using System;
using System.Data.SqlClient;

namespace mRemoteNG.Config.Serializers.Versioning
{
    public class SqlVersion27To28Upgrader : IVersionUpgrader
    {
        private readonly SqlDatabaseConnector _sqlDatabaseConnector;

        public SqlVersion27To28Upgrader(SqlDatabaseConnector sqlDatabaseConnector)
        {
            if (sqlDatabaseConnector == null)
                throw new ArgumentNullException(nameof(sqlDatabaseConnector));

            _sqlDatabaseConnector = sqlDatabaseConnector;
        }

        public bool CanUpgrade(Version currentVersion)
        {
            return currentVersion.CompareTo(new Version(2, 7)) == 0;
        }

        public Version Upgrade()
        {
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Upgrading database from version 2.7 to version 2.8.");
            const string sqlText = @"
ALTER TABLE tblCons
ADD Favorite bit NOT NULL DEFAULT 0,
	InheritFavorite bit NOT NULL DEFAULT 0;
UPDATE tblRoot
    SET ConfVersion='2.8'";
            var sqlCommand = new SqlCommand(sqlText, _sqlDatabaseConnector.SqlConnection);
            sqlCommand.ExecuteNonQuery();

            return new Version(2, 8);
        }
    }
}
