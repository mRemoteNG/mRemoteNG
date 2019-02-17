﻿using mRemoteNG.App;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;
using System;
using System.Data.SqlClient;

namespace mRemoteNG.Config.Serializers.Versioning
{
    public class SqlVersion26To27Upgrader : IVersionUpgrader
    {
        private readonly SqlDatabaseConnector _sqlDatabaseConnector;

        public SqlVersion26To27Upgrader(SqlDatabaseConnector sqlDatabaseConnector)
        {
            if (sqlDatabaseConnector == null)
                throw new ArgumentNullException(nameof(sqlDatabaseConnector));

            _sqlDatabaseConnector = sqlDatabaseConnector;
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
	InheritRedirectClipboard bit NOT NULL DEFAULT 0;
UPDATE tblRoot
    SET ConfVersion='2.7'";
            var sqlCommand = new SqlCommand(sqlText, _sqlDatabaseConnector.SqlConnection);
            sqlCommand.ExecuteNonQuery();

            return new Version(2, 7);
        }
    }
}