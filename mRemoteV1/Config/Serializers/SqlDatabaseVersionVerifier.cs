using System;
using System.Data.SqlClient;
using System.Globalization;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;

namespace mRemoteNG.Config.Serializers
{
    public class SqlDatabaseVersionVerifier
    {
        private readonly SqlDatabaseConnector _sqlDatabaseConnector;

        public SqlDatabaseVersionVerifier(SqlDatabaseConnector sqlDatabaseConnector)
        {
            if (sqlDatabaseConnector == null)
                throw new ArgumentNullException(nameof(sqlDatabaseConnector));

            _sqlDatabaseConnector = sqlDatabaseConnector;
        }

        public bool VerifyDatabaseVersion()
        {
            var isVerified = false;
            try
            {
                var databaseVersion = GetDatabaseVersion(_sqlDatabaseConnector);

                if (databaseVersion.Equals(new Version()))
                {
                    return true;
                }

                if (databaseVersion.CompareTo(new Version(2, 2)) == 0) // 2.2
                {
                    UpgradeFrom2_2();
                    databaseVersion = new Version(2, 3);
                }

                if (databaseVersion.CompareTo(new Version(2, 3)) == 0) // 2.3
                {
                    UpgradeFrom2_3();
                    databaseVersion = new Version(2, 4);
                }

                if (databaseVersion.CompareTo(new Version(2, 4)) == 0) // 2.4
                {
                    UpgradeFrom2_4();
                    databaseVersion = new Version(2, 5);
                }

                if (databaseVersion.CompareTo(new Version(2, 5)) == 0) // 2.5
                {
                    UpgradeFrom2_5();
                    databaseVersion = new Version(2, 6);
                }

                if (databaseVersion.CompareTo(new Version(2, 6)) == 0) // 2.6
                    isVerified = true;

                if (isVerified == false)
                    Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                        string.Format(Language.strErrorBadDatabaseVersion, databaseVersion, GeneralAppInfo.ProductName));
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                    string.Format(Language.strErrorVerifyDatabaseVersionFailed, ex.Message));
            }
            return isVerified;
        }

        private Version GetDatabaseVersion(SqlDatabaseConnector sqlDatabaseConnector)
        {
            Version databaseVersion;
            SqlDataReader sqlDataReader = null;
            try
            {
                var sqlCommand = new SqlCommand("SELECT * FROM tblRoot", sqlDatabaseConnector.SqlConnection);
                if (!sqlDatabaseConnector.IsConnected)
                    sqlDatabaseConnector.Connect();
                sqlDataReader = sqlCommand.ExecuteReader();
                if (!sqlDataReader.HasRows)
                    return new Version(); // assume new empty database
                else
                    sqlDataReader.Read();
                databaseVersion =
                    new Version(Convert.ToString(sqlDataReader["confVersion"], CultureInfo.InvariantCulture));
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, $"Retrieving database version failed. {ex}");
                throw;
            }
            finally
            {
                if (sqlDataReader != null && !sqlDataReader.IsClosed)
                    sqlDataReader.Close();
            }
            return databaseVersion;
        }

        private void UpgradeFrom2_2()
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

        private void UpgradeFrom2_3()
        {
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Upgrading database from version 2.3 to version 2.4.");
            const string sqlText = @"
ALTER TABLE tblCons
ADD UseCredSsp bit NOT NULL DEFAULT 1,
    InheritUseCredSsp bit NOT NULL DEFAULT 0;";
            var sqlCommand = new SqlCommand(sqlText, _sqlDatabaseConnector.SqlConnection);
            sqlCommand.ExecuteNonQuery();
        }

        private void UpgradeFrom2_4()
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

        private void UpgradeFrom2_5()
        {
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Upgrading database from version 2.5 to version 2.6.");
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
            var sqlCommand = new SqlCommand(sqlText, _sqlDatabaseConnector.SqlConnection);
            sqlCommand.ExecuteNonQuery();
        }
    }
}