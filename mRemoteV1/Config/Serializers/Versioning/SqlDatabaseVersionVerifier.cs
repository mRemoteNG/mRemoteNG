using System;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;

namespace mRemoteNG.Config.Serializers.Versioning
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

        public bool VerifyDatabaseVersion(Version dbVersion)
        {
            var isVerified = false;
            try
            {
                var databaseVersion = dbVersion;

                if (databaseVersion.Equals(new Version()))
                {
                    return true;
                }

                var sql22To23Upgrader = new SqlVersion22To23Upgrader(_sqlDatabaseConnector);
                if (sql22To23Upgrader.CanUpgrade(databaseVersion))
                {
                    sql22To23Upgrader.Upgrade();
                    databaseVersion = new Version(2, 3);
                }

                var sql23To24Upgrader = new SqlVersion23To24Upgrader(_sqlDatabaseConnector);
                if (sql23To24Upgrader.CanUpgrade(databaseVersion))
                {
                    sql23To24Upgrader.Upgrade();
                    databaseVersion = new Version(2, 4);
                }

                var sql24To25Upgrader = new SqlVersion24To25Upgrader(_sqlDatabaseConnector);
                if (sql24To25Upgrader.CanUpgrade(databaseVersion))
                {
                    sql24To25Upgrader.Upgrade();
                    databaseVersion = new Version(2, 5);
                }

                var sql25To26Upgrader = new SqlVersion25To26Upgrader(_sqlDatabaseConnector);
                if (sql25To26Upgrader.CanUpgrade(databaseVersion))
                {
                    sql25To26Upgrader.Upgrade();
                    databaseVersion = new Version(2, 6);
                }

                if (databaseVersion.CompareTo(new Version(2, 6)) == 0) // 2.6
                    isVerified = true;

                if (isVerified == false)
                    Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, string.Format(Language.strErrorBadDatabaseVersion, databaseVersion, GeneralAppInfo.ProductName));
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, string.Format(Language.strErrorVerifyDatabaseVersionFailed, ex.Message));
            }
            return isVerified;
        }
    }
}