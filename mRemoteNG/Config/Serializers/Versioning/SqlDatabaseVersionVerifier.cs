using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;
using mRemoteNG.Resources.Language;
using System;
using System.Runtime.Versioning;

namespace mRemoteNG.Config.Serializers.Versioning
{
    [SupportedOSPlatform("windows")]
    public class SqlDatabaseVersionVerifier
    {
        private readonly Version _currentSupportedVersion = new Version(3, 0);

        private readonly IDatabaseConnector _databaseConnector;

        public SqlDatabaseVersionVerifier(IDatabaseConnector databaseConnector)
        {
            _databaseConnector = databaseConnector ?? throw new ArgumentNullException(nameof(databaseConnector));
        }

        public bool VerifyDatabaseVersion(Version dbVersion)
        {
            try
            {
                Version databaseVersion = dbVersion;

                if (databaseVersion.Equals(_currentSupportedVersion))
                {
                    return true;
                }

                var dbUpgraders = new IVersionUpgrader[]
                {
                    new SqlVersion22To23Upgrader(_databaseConnector),
                    new SqlVersion23To24Upgrader(_databaseConnector),
                    new SqlVersion24To25Upgrader(_databaseConnector),
                    new SqlVersion25To26Upgrader(_databaseConnector),
                    new SqlVersion26To27Upgrader(_databaseConnector),
                    new SqlVersion27To28Upgrader(_databaseConnector),
                    new SqlVersion28To29Upgrader(_databaseConnector),
                    new SqlVersion29To30Upgrader(_databaseConnector),
                };

                foreach (IVersionUpgrader upgrader in dbUpgraders)
                {
                    if (upgrader.CanUpgrade(databaseVersion))
                    {
                        databaseVersion = upgrader.Upgrade();
                    }
                }

                // DB is at the highest current supported version
                if (databaseVersion.CompareTo(_currentSupportedVersion) == 0)
                {
                    return true;
                }

                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, string.Format(Language.ErrorBadDatabaseVersion, databaseVersion, GeneralAppInfo.ProductName));
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, string.Format(Language.ErrorVerifyDatabaseVersionFailed, ex.Message));
            }

            return false;
        }
    }
}