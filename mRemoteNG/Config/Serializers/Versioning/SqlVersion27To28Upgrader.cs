using mRemoteNG.App;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;
using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace mRemoteNG.Config.Serializers.Versioning
{
    public class SqlVersion27To28Upgrader : IVersionUpgrader
    {
        private readonly IDatabaseConnector _databaseConnector;

        public SqlVersion27To28Upgrader(IDatabaseConnector databaseConnector)
        {
            _databaseConnector = databaseConnector ?? throw new ArgumentNullException(nameof(databaseConnector));
        }

        public bool CanUpgrade(Version currentVersion)
        {
            return currentVersion.CompareTo(new Version(2, 7)) == 0;
        }

        public Version Upgrade()
        {
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                "Upgrading database from version 2.7 to version 2.8.");

            try
            {
                const string mySqlText1 = @"
ALTER TABLE tblCons MODIFY COLUMN ID INT;
ALTER TABLE tblCons DROP PRIMARY KEY, ADD PRIMARY KEY (ConstantID);
ALTER TABLE tblCons ADD INDEX `id` (ID), MODIFY ID int auto_increment;
ALTER TABLE tblCons MODIFY COLUMN ID INT AUTO_INCREMENT, ADD UNIQUE(ID);
UPDATE tblRoot SET ConfVersion='2.8'";

                const string msSqlText1 = @"
UPDATE tblCons SET UseEnhancedMode = 0 WHERE UseEnhancedMode IS NULL;
UPDATE tblCons SET InheritUseEnhancedMode = 0 WHERE InheritUseEnhancedMode IS NULL;
ALTER TABLE tblCons ALTER COLUMN ConstantID varchar(128) NOT NULL;";
                const string msSqlText2 = @"
ALTER TABLE tblCons ADD CONSTRAINT PK_tblCons PRIMARY KEY (ConstantID);
UPDATE tblRoot SET ConfVersion='2.8';";

                DbCommand dbCommand;

                if (_databaseConnector.GetType() == typeof(MSSqlDatabaseConnector))
                {
                    dbCommand = _databaseConnector.DbCommand(msSqlText1);
                    dbCommand.ExecuteNonQuery();
                    dbCommand = _databaseConnector.DbCommand(msSqlText2);
                } else if (_databaseConnector.GetType() == typeof(MySqlDatabaseConnector))
                {
                    dbCommand = _databaseConnector.DbCommand(mySqlText1);
                } else
                {
                    throw new Exception("Unknown database backend");
                }
            
                dbCommand.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                // no-op
            }
            return new Version(2, 8);
        }
    }
}