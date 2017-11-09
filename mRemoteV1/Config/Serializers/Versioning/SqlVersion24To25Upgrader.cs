using System;
using System.Data.SqlClient;
using mRemoteNG.App;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;

namespace mRemoteNG.Config.Serializers.Versioning
{
    public class SqlVersion24To25Upgrader : IVersionUpgrader
    {
        private readonly SqlDatabaseConnector _sqlDatabaseConnector;

        public SqlVersion24To25Upgrader(SqlDatabaseConnector sqlDatabaseConnector)
        {
            if (sqlDatabaseConnector == null)
                throw new ArgumentNullException(nameof(sqlDatabaseConnector));

            _sqlDatabaseConnector = sqlDatabaseConnector;
        }

        public bool CanUpgrade(Version currentVersion)
        {
            return currentVersion.CompareTo(new Version(2, 4)) == 0;
        }

        public void Upgrade()
        {
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Upgrading database from version 2.4 to version 2.5.");
            const string sqlText = @"
ALTER TABLE tblCons
ADD LoadBalanceInfo varchar (1024) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    AutomaticResize bit NOT NULL DEFAULT 1,
    InheritLoadBalanceInfo bit NOT NULL DEFAULT 0,
    InheritAutomaticResize bit NOT NULL DEFAULT 0;";
            var sqlCommand = new SqlCommand(sqlText, _sqlDatabaseConnector.SqlConnection);
            sqlCommand.ExecuteNonQuery();
        }
    }
}