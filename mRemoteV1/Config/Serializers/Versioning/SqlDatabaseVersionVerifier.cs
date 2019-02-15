using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;
using System;

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

                var dbUpgraders = new IVersionUpgrader[]
                {
                    new SqlVersion22To23Upgrader(_sqlDatabaseConnector),
                    new SqlVersion23To24Upgrader(_sqlDatabaseConnector),
                    new SqlVersion24To25Upgrader(_sqlDatabaseConnector),
                    new SqlVersion25To26Upgrader(_sqlDatabaseConnector),
                    new SqlVersion26To27Upgrader(_sqlDatabaseConnector),
                };

                foreach (var upgrader in dbUpgraders)
                {
                    if (upgrader.CanUpgrade(databaseVersion))
                    {
                        databaseVersion = upgrader.Upgrade();
                    }
                }

                // DB is at the highest current supported version
                if (databaseVersion.CompareTo(new Version(2, 7)) == 0)
                    isVerified = true;

                if (isVerified == false)
                    Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                                                        string.Format(Language.strErrorBadDatabaseVersion,
                                                                      databaseVersion,
                                                                      GeneralAppInfo.ProductName));
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    string.Format(Language.strErrorVerifyDatabaseVersionFailed,
                                                                  ex.Message));
            }

            return isVerified;
        }
    }
}