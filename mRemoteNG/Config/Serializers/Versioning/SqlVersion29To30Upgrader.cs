using mRemoteNG.App;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;
using System;
using System.Data.Common;
using System.Runtime.Versioning;

namespace mRemoteNG.Config.Serializers.Versioning
{
    [SupportedOSPlatform("windows")]
    public class SqlVersion29To30Upgrader : IVersionUpgrader
    {
        private readonly Version _version = new Version(3, 0);
        private readonly IDatabaseConnector _databaseConnector;

        public SqlVersion29To30Upgrader(IDatabaseConnector databaseConnector)
        {
            _databaseConnector = databaseConnector ?? throw new ArgumentNullException(nameof(databaseConnector));
        }

        public bool CanUpgrade(Version currentVersion)
        {
            return currentVersion == new Version(2, 9) ||
                // Support upgrading during dev revisions, 2.9.1, 2.9.2, etc...
                (currentVersion <= new Version(3, 0) &&
                currentVersion < _version);
        }

        public Version Upgrade()
        {
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                $"Upgrading database to version {_version}.");

            // MYSQL
            const string mySqlAlter = @"
ALTER TABLE tblCons MODIFY COLUMN `RenderingEngine` varchar(32) DEFAULT NULL;
ALTER TABLE tblCons MODIFY COLUMN `RedirectDiskDrives` varchar(32) DEFAULT NULL;
ALTER TABLE tblCons ADD COLUMN `RedirectDiskDrivesCustom` varchar(32) DEFAULT NULL;
ALTER TABLE tblCons ADD COLUMN `InheritRedirectDiskDrivesCustom` tinyint NOT NULL;
ALTER TABLE tblCons ADD COLUMN `UserViaAPI` varchar(512) NOT NULL;

-- mysql tinyint(1) is deprecated - modify all tinyint(1) columns to tinyint
ALTER TABLE tblCons MODIFY COLUMN `Expanded` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `AutomaticResize` tinyint NOT NULL DEFAULT '1';
ALTER TABLE tblCons MODIFY COLUMN `CacheBitmaps` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `ConnectToConsole` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `Connected` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `DisableCursorBlinking` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `DisableCursorShadow` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `DisableFullWindowDrag` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `DisableMenuAnimations` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `DisplayThemes` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `DisplayWallpaper` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `EnableDesktopComposition` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `EnableFontSmoothing` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `Favorite` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `RDPAlertIdleTimeout` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `RedirectAudioCapture` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `RedirectClipboard` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `RedirectKeys` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `RedirectPorts` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `RedirectPrinters` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `RedirectSmartCards` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `UseCredSsp` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `UseEnhancedMode` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `UseVmId` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `VNCViewOnly` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritAutomaticResize` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritCacheBitmaps` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritColors` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritDescription` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritDisableCursorBlinking` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritDisableCursorShadow` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritDisableFullWindowDrag` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritDisableMenuAnimations` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritDisplayThemes` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritDisplayWallpaper` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritDomain` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritEnableDesktopComposition` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritEnableFontSmoothing` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritExtApp` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritFavorite` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritICAEncryptionStrength` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritIcon` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritLoadBalanceInfo` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritMacAddress` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritOpeningCommand` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritPanel` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritPassword` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritPort` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritPostExtApp` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritPreExtApp` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritProtocol` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritPuttySession` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritRDGatewayDomain` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritRDGatewayHostname` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritRDGatewayPassword` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritRDGatewayUsageMethod` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritRDGatewayUseConnectionCredentials` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritRDGatewayExternalCredentialProvider` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritRDGatewayUsername` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritRDGatewayUserViaAPI` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritRDPAlertIdleTimeout` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritRDPAuthenticationLevel` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritRDPMinutesToIdleTimeout` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritRdpVersion` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritRedirectAudioCapture` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritRedirectClipboard` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritRedirectDiskDrives` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritRedirectDiskDrivesCustom` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritRedirectKeys` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritRedirectPorts` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritRedirectPrinters` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritRedirectSmartCards` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritRedirectSound` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritRenderingEngine` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritResolution` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritSSHOptions` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritSSHTunnelConnectionName` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritSoundQuality` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritUseConsoleSession` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritUseCredSsp` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritUseRestrictedAdmin` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritUseRCG` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritExternalCredentialProvider` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritUserViaAPI` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `UseRestrictedAdmin` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `UseRCG` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritUseEnhancedMode` tinyint DEFAULT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritUseVmId` tinyint DEFAULT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritUserField` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritUsername` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritVNCAuthMode` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritVNCColors` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritVNCCompression` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritVNCEncoding` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritVNCProxyIP` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritVNCProxyPassword` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritVNCProxyPort` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritVNCProxyType` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritVNCProxyUsername` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritVNCSmartSizeMode` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritVNCViewOnly` tinyint NOT NULL;
ALTER TABLE tblCons MODIFY COLUMN `InheritVmId` tinyint NOT NULL;
";

            const string mySqlUpdate = @"SET SQL_SAFE_UPDATES=0; UPDATE tblRoot SET ConfVersion=?; SET SQL_SAFE_UPDATES=1;";

            // MS-SQL
            const string msSqlAlter = @"
ALTER TABLE tblCons ALTER COLUMN RenderingEngine varchar(32) DEFAULT NULL;
ALTER TABLE tblCons ALTER COLUMN RedirectDiskDrives varchar(32) DEFAULT NULL;
ALTER TABLE tblCons ADD RedirectDiskDrivesCustom varchar(32) DEFAULT NULL;
ALTER TABLE tblCons ADD InheritRedirectDiskDrivesCustom bit NOT NULL;
ALTER TABLE tblCons ADD `UserViaAPI` varchar(512) NOT NULL;
";

            const string msSqlUpdate = @"UPDATE tblRoot SET ConfVersion=@confVersion;";

            using (var sqlTran = _databaseConnector.DbConnection().BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
                DbCommand dbCommand;
                if (_databaseConnector.GetType() == typeof(MSSqlDatabaseConnector))
                {
                    dbCommand = _databaseConnector.DbCommand(msSqlAlter);
                    dbCommand.Transaction = sqlTran;
                    dbCommand.ExecuteNonQuery();
                    dbCommand = _databaseConnector.DbCommand(msSqlUpdate);
                    dbCommand.Transaction = sqlTran;
                }
                else if (_databaseConnector.GetType() == typeof(MySqlDatabaseConnector))
                {
                    dbCommand = _databaseConnector.DbCommand(mySqlAlter);
                    dbCommand.Transaction = sqlTran;
                    dbCommand.ExecuteNonQuery();
                    dbCommand = _databaseConnector.DbCommand(mySqlUpdate);
                    dbCommand.Transaction = sqlTran;
                }
                else
                {
                    throw new Exception("Unknown database back-end");
                }
                var pConfVersion = dbCommand.CreateParameter();
                pConfVersion.ParameterName = "confVersion";
                pConfVersion.Value = _version.ToString();
                pConfVersion.DbType = System.Data.DbType.String;
                pConfVersion.Direction = System.Data.ParameterDirection.Input;
                dbCommand.Parameters.Add(pConfVersion);

                dbCommand.ExecuteNonQuery();
                sqlTran.Commit();
            }
            return _version;
        }
    }
}