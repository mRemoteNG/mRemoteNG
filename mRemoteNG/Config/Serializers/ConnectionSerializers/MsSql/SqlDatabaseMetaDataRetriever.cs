using System;
using System.Data.Common;
using System.Globalization;
using System.Runtime.Versioning;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.MsSql
{
    [SupportedOSPlatform("windows")]
    public class SqlDatabaseMetaDataRetriever
    {
        public SqlConnectionListMetaData GetDatabaseMetaData(IDatabaseConnector databaseConnector)
        {
            SqlConnectionListMetaData metaData;
            DbDataReader dbDataReader = null;

            try
            {
                if (!databaseConnector.IsConnected)
                    databaseConnector.Connect();

                if (!DoesDbTableExist(databaseConnector, "tblRoot"))
                {
                    // database exists but is empty, initialize it with the schema
                    InitializeDatabaseSchema(databaseConnector);
                }

                DbCommand dbCommand = databaseConnector.DbCommand("SELECT * FROM tblRoot");
                dbDataReader = dbCommand.ExecuteReader();
                if (!dbDataReader.HasRows)
                {
                    // assume new empty database
                    return null;
                }
                else
                {
                    dbDataReader.Read();
                }

                metaData = new SqlConnectionListMetaData
                {
                    Name = dbDataReader["Name"] as string ?? "",
                    Protected = dbDataReader["Protected"] as string ?? "",
                    Export = (bool)dbDataReader["Export"],
                    ConfVersion = new Version(Convert.ToString(dbDataReader["confVersion"], CultureInfo.InvariantCulture) ?? string.Empty)
                };
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, $"Retrieving database version failed. {ex}");
                throw;
            }
            finally
            {
                if (dbDataReader != null && !dbDataReader.IsClosed)
                    dbDataReader.Close();
            }

            return metaData;
        }

        public void WriteDatabaseMetaData(RootNodeInfo rootTreeNode, IDatabaseConnector databaseConnector)
        {
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            string strProtected;
            if (rootTreeNode != null)
            {
                if (rootTreeNode.Password)
                {
                    var password = rootTreeNode.PasswordString.ConvertToSecureString();
                    strProtected = cryptographyProvider.Encrypt("ThisIsProtected", password);
                }
                else
                {
                    strProtected = cryptographyProvider.Encrypt("ThisIsNotProtected", Runtime.EncryptionKey);
                }
            }
            else
            {
                strProtected = cryptographyProvider.Encrypt("ThisIsNotProtected", Runtime.EncryptionKey);
            }

            var cmd = databaseConnector.DbCommand("DELETE FROM tblRoot");
            cmd.ExecuteNonQuery();

            if (rootTreeNode != null)
            {
                cmd = databaseConnector.DbCommand(
                        "INSERT INTO tblRoot (Name, Export, Protected, ConfVersion) VALUES('" +
                        MiscTools.PrepareValueForDB(rootTreeNode.Name) + "', 0, '" + strProtected + "','" +
                        ConnectionsFileInfo.ConnectionFileVersion.ToString() + "')");
                cmd.ExecuteNonQuery();
            }
            else
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, $"UpdateRootNodeTable: rootTreeNode was null. Could not insert!");
            }
        }

        private bool DoesDbTableExist(IDatabaseConnector databaseConnector, string tableName)
        {
            bool exists;

            try
            {
                // ANSI SQL way.  Works in PostgreSQL, MSSQL, MySQL.
                var cmd = databaseConnector.DbCommand("select case when exists((select * from information_schema.tables where table_name = '" + tableName + "')) then 1 else 0 end");
                cmd.ExecuteNonQuery();
                exists = (int)cmd.ExecuteScalar()! == 1;
            }
            catch
            {
                try
                {
                    // Other RDBMS.  Graceful degradation
                    exists = true;
                    var cmdOthers = databaseConnector.DbCommand("select 1 from " + tableName + " where 1 = 0");
                    cmdOthers.ExecuteNonQuery();
                }
                catch
                {
                    exists = false;
                }
            }

            return exists;
        }

        private void InitializeDatabaseSchema(IDatabaseConnector databaseConnector)
        {
            string sql;

            var t = databaseConnector.GetType();

            if (databaseConnector.GetType() == typeof(MSSqlDatabaseConnector))
            {
                sql = @"
if exists (select * from dbo.sysobjects
    where id = object_id(N'[dbo].[tblCons]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblCons]

if exists (select * from dbo.sysobjects
    where id = object_id(N'[dbo].[tblRoot]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblRoot]

if exists (select * from dbo.sysobjects
    where id = object_id(N'[dbo].[tblUpdate]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblUpdate]

CREATE TABLE [dbo].[tblCons] (
    [ID] int NOT NULL IDENTITY(1,1),
    [ConstantID] varchar(128) NOT NULL PRIMARY KEY,
    [PositionID] int NOT NULL,
    [ParentID] varchar(128),
    [LastChange] datetime NOT NULL,
    [Name] varchar(128) NOT NULL,
    [Type] varchar(32) NOT NULL,
    [Expanded] bit NOT NULL,
    [AutomaticResize] bit NOT NULL DEFAULT ((1)),
    [CacheBitmaps] bit NOT NULL,
    [Colors] varchar(32) NOT NULL,
    [ConnectToConsole] bit NOT NULL,
    [Connected] bit NOT NULL,
    [Description] varchar(1024),
    [DisableCursorBlinking] bit NOT NULL,
    [DisableCursorShadow] bit NOT NULL,
    [DisableFullWindowDrag] bit NOT NULL,
    [DisableMenuAnimations] bit NOT NULL,
    [DisplayThemes] bit NOT NULL,
    [DisplayWallpaper] bit NOT NULL,
    [Domain] varchar(512),
    [EnableDesktopComposition] bit NOT NULL,
    [EnableFontSmoothing] bit NOT NULL,
    [ExtApp] varchar(256),
    [Favorite] tinyint NOT NULL,
    [Hostname] varchar(512),
    [Icon] varchar(128) NOT NULL,
    [LoadBalanceInfo] varchar(1024),
    [MacAddress] varchar(32),
    [OpeningCommand] varchar(512),
    [Panel] varchar(128) NOT NULL,
    [Password] varchar(1024),
    [Port] int NOT NULL,
    [PostExtApp] varchar(256),
    [PreExtApp] varchar(256),
    [Protocol] varchar(32) NOT NULL,
    [PuttySession] varchar(128),
    [RDGatewayDomain] varchar(512),
    [RDGatewayHostname] varchar(512),
    [RDGatewayPassword] varchar(1024),
    [RDGatewayUsageMethod] varchar(32) NOT NULL,
    [RDGatewayUseConnectionCredentials] varchar(32) NOT NULL,
    [RDGatewayUsername] varchar(512),
    [RDPAlertIdleTimeout] bit NOT NULL,
    [RDPAuthenticationLevel] varchar(32) NOT NULL,
    [RDPMinutesToIdleTimeout] int NOT NULL,
    [RdpVersion] varchar(10) NULL,
    [RedirectAudioCapture] bit NOT NULL,
    [RedirectClipboard] bit NOT NULL,
    [RedirectDiskDrives] bit NOT NULL,
    [RedirectKeys] bit NOT NULL,
    [RedirectPorts] bit NOT NULL,
    [RedirectPrinters] bit NOT NULL,
    [RedirectSmartCards] bit NOT NULL,
    [RedirectSound] varchar(64) NOT NULL,
    [RenderingEngine] varchar(16) NULL,
    [Resolution] varchar(32) NOT NULL,
    [SSHOptions] varchar(1024) NOT NULL,
    [SSHTunnelConnectionName] varchar(128) NOT NULL,
    [SoundQuality] varchar(20) NOT NULL,
    [UseCredSsp] bit NOT NULL,
    [UseEnhancedMode] bit NOT NULL,
    [UseVmId] bit NOT NULL,
    [UserField] varchar(256) NULL,
    [Username] varchar(512) NULL,
    [VNCAuthMode] varchar(10) NULL,
    [VNCColors] varchar(10) NULL,
    [VNCCompression] varchar(10) NULL,
    [VNCEncoding] varchar(20) NULL,
    [VNCProxyIP] varchar(128) NULL,
    [VNCProxyPassword] varchar(1024) NULL,
    [VNCProxyPort] int NULL,
    [VNCProxyType] varchar(20) NULL,
    [VNCProxyUsername] varchar(512) NULL,
    [VNCSmartSizeMode] varchar(20) NULL,
    [VNCViewOnly] bit NOT NULL,
    [VmId] varchar(100) NULL,
    [ICAEncryptionStrength] varchar(32) NOT NULL,
    [InheritAutomaticResize] bit NOT NULL,
    [InheritCacheBitmaps] bit NOT NULL,
    [InheritColors] bit NOT NULL,
    [InheritDescription] bit NOT NULL,
    [InheritDisableCursorBlinking] bit NOT NULL,
    [InheritDisableCursorShadow] bit NOT NULL,
    [InheritDisableFullWindowDrag] bit NOT NULL,
    [InheritDisableMenuAnimations] bit NOT NULL,
    [InheritDisplayThemes] bit NOT NULL,
    [InheritDisplayWallpaper] bit NOT NULL,
    [InheritDomain] bit NOT NULL,
    [InheritEnableDesktopComposition] bit NOT NULL,
    [InheritEnableFontSmoothing] bit NOT NULL,
    [InheritExtApp] bit NOT NULL,
    [InheritFavorite] bit NOT NULL,
    [InheritICAEncryptionStrength] bit NOT NULL,
    [InheritIcon] bit NOT NULL,
    [InheritLoadBalanceInfo] bit NOT NULL,
    [InheritMacAddress] bit NOT NULL,
    [InheritOpeningCommand] bit NOT NULL,
    [InheritPanel] bit NOT NULL,
    [InheritPassword] bit NOT NULL,
    [InheritPort] bit NOT NULL,
    [InheritPostExtApp] bit NOT NULL,
    [InheritPreExtApp] bit NOT NULL,
    [InheritProtocol] bit NOT NULL,
    [InheritPuttySession] bit NOT NULL,
    [InheritRDGatewayDomain] bit NOT NULL,
    [InheritRDGatewayHostname] bit NOT NULL,
    [InheritRDGatewayPassword] bit NOT NULL,
    [InheritRDGatewayUsageMethod] bit NOT NULL,
    [InheritRDGatewayUseConnectionCredentials] bit NOT NULL,
    [InheritRDGatewayExternalCredentialProvider] bit NOT NULL,
    [InheritRDGatewayUsername] bit NOT NULL,
    [InheritRDGatewayUserViaAPI] bit NOT NULL,
    [InheritRDPAlertIdleTimeout] bit NOT NULL,
    [InheritRDPAuthenticationLevel] bit NOT NULL,
    [InheritRDPMinutesToIdleTimeout] bit NOT NULL,
    [InheritRdpVersion] bit NOT NULL,
    [InheritRedirectAudioCapture] bit NOT NULL,
    [InheritRedirectClipboard] bit NOT NULL,
    [InheritRedirectDiskDrives] bit NOT NULL,
    [InheritRedirectKeys] bit NOT NULL,
    [InheritRedirectPorts] bit NOT NULL,
    [InheritRedirectPrinters] bit NOT NULL,
    [InheritRedirectSmartCards] bit NOT NULL,
    [InheritRedirectSound] bit NOT NULL,
    [InheritRenderingEngine] bit NOT NULL,
    [InheritResolution] bit NOT NULL,
    [InheritSSHOptions] bit NOT NULL,
    [InheritSSHTunnelConnectionName] bit NOT NULL,
    [InheritSoundQuality] bit NOT NULL,
    [InheritUseConsoleSession] bit NOT NULL,
    [InheritUseCredSsp] bit NOT NULL,
    [InheritUseRestrictedAdmin] bit NOT NULL,
    [InheritUseRCG] bit NOT NULL,
    [InheritExternalCredentialProvider] bit NOT NULL,
    [InheritUserViaAPI] bit NOT NULL,
    [UseRestrictedAdmin] bit NOT NULL,
    [UseRCG] bit NOT NULL,
    [InheritUseEnhancedMode] bit NOT NULL,
    [InheritUseVmId] bit,
    [InheritUserField] bit NOT NULL,
    [InheritUsername] bit NOT NULL,
    [InheritVNCAuthMode] bit NOT NULL,
    [InheritVNCColors] bit NOT NULL,
    [InheritVNCCompression] bit NOT NULL,
    [InheritVNCEncoding] bit NOT NULL,
    [InheritVNCProxyIP] bit NOT NULL,
    [InheritVNCProxyPassword] bit NOT NULL,
    [InheritVNCProxyPort] bit NOT NULL,
    [InheritVNCProxyType] bit NOT NULL,
    [InheritVNCProxyUsername] bit NOT NULL,
    [InheritVNCSmartSizeMode] bit NOT NULL,
    [InheritVNCViewOnly] bit NOT NULL,
    [InheritVmId] bit NOT NULL,
    [StartProgram] varchar(512) NULL,
    [StartProgramWorkDir] varchar(512) NULL,
    [EC2Region] varchar(32) NULL,
    [EC2InstanceId] varchar(32) NULL,
    [ExternalCredentialProvider] varchar(256) NULL,
    [ExternalAddressProvider] varchar(256) NULL,
) ON [PRIMARY]

CREATE TABLE [dbo].[tblRoot] (
        [Name] [varchar] (2048) NOT NULL,
        [Export] [bit] NOT NULL,
        [Protected] [varchar] (4048) NOT NULL,
        [ConfVersion] [varchar] (15) NOT NULL
) ON [PRIMARY]

CREATE TABLE [dbo].[tblUpdate] (
        [LastUpdate] [datetime] NULL
) ON [PRIMARY]
";
            }
            else if (databaseConnector.GetType() == typeof(MySqlDatabaseConnector))
            {
                sql = @"
/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `tblCons`
--

DROP TABLE IF EXISTS `tblCons`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tblCons` (
    `ID` int NOT NULL AUTO_INCREMENT,
    `ConstantID` varchar(128) NOT NULL,
    `PositionID` int NOT NULL,
    `ParentID` varchar(128) DEFAULT NULL,
    `LastChange` datetime NOT NULL,
    `Name` varchar(128) NOT NULL,
    `Type` varchar(32) NOT NULL,
    `Expanded` tinyint(1) NOT NULL,
    `AutomaticResize` tinyint(1) NOT NULL DEFAULT 1,
    `CacheBitmaps` tinyint(1) NOT NULL,
    `Colors` varchar(32) NOT NULL,
    `ConnectToConsole` tinyint(1) NOT NULL,
    `Connected` tinyint(1) NOT NULL,
    `Description` varchar(1024) DEFAULT NULL,
    `DisableCursorBlinking` tinyint(1) NOT NULL,
    `DisableCursorShadow` tinyint(1) NOT NULL,
    `DisableFullWindowDrag` tinyint(1) NOT NULL,
    `DisableMenuAnimations` tinyint(1) NOT NULL,
    `DisplayThemes` tinyint(1) NOT NULL,
    `DisplayWallpaper` tinyint(1) NOT NULL,
    `Domain` varchar(512) DEFAULT NULL,
    `EnableDesktopComposition` tinyint(1) NOT NULL,
    `EnableFontSmoothing` tinyint(1) NOT NULL,
    `ExtApp` varchar(256) DEFAULT NULL,
    `Favorite` tinyint(1) NOT NULL,
    `Hostname` varchar(512) DEFAULT NULL,
    `Icon` varchar(128) NOT NULL,
    `LoadBalanceInfo` varchar(1024) DEFAULT NULL,
    `MacAddress` varchar(32) DEFAULT NULL,
    `OpeningCommand` varchar(512) DEFAULT NULL,
    `Panel` varchar(128) NOT NULL,
    `Password` varchar(1024) DEFAULT NULL,
    `Port` int NOT NULL,
    `PostExtApp` varchar(256) DEFAULT NULL,
    `PreExtApp` varchar(256) DEFAULT NULL,
    `Protocol` varchar(32) NOT NULL,
    `PuttySession` varchar(128) DEFAULT NULL,
    `RDGatewayDomain` varchar(512) DEFAULT NULL,
    `RDGatewayHostname` varchar(512) DEFAULT NULL,
    `RDGatewayPassword` varchar(1024) DEFAULT NULL,
    `RDGatewayUsageMethod` varchar(32) NOT NULL,
    `RDGatewayUseConnectionCredentials` varchar(32) NOT NULL,
    `RDGatewayUsername` varchar(512) DEFAULT NULL,
    `RDPAlertIdleTimeout` tinyint(1) NOT NULL,
    `RDPAuthenticationLevel` varchar(32) NOT NULL,
    `RDPMinutesToIdleTimeout` int(11) NOT NULL,
    `RdpVersion` varchar(10) DEFAULT NULL,
    `RedirectAudioCapture` tinyint(1) NOT NULL,
    `RedirectClipboard` tinyint(1) NOT NULL,
    `RedirectDiskDrives` tinyint(1) NOT NULL,
    `RedirectKeys` tinyint(1) NOT NULL,
    `RedirectPorts` tinyint(1) NOT NULL,
    `RedirectPrinters` tinyint(1) NOT NULL,
    `RedirectSmartCards` tinyint(1) NOT NULL,
    `RedirectSound` varchar(64) NOT NULL,
    `RenderingEngine` varchar(16) DEFAULT NULL,
    `Resolution` varchar(32) NOT NULL,
    `SSHOptions` varchar(1024) NOT NULL,
    `SSHTunnelConnectionName` varchar(128) NOT NULL,
    `SoundQuality` varchar(20) NOT NULL,
    `UseCredSsp` tinyint(1) NOT NULL,
    `UseEnhancedMode` tinyint(1) NOT NULL,
    `UseVmId` tinyint(1) NOT NULL,
    `UserField` varchar(256) DEFAULT NULL,
    `Username` varchar(512) DEFAULT NULL,
    `VNCAuthMode` varchar(10) DEFAULT NULL,
    `VNCColors` varchar(10) DEFAULT NULL,
    `VNCCompression` varchar(10) DEFAULT NULL,
    `VNCEncoding` varchar(20) DEFAULT NULL,
    `VNCProxyIP` varchar(128) DEFAULT NULL,
    `VNCProxyPassword` varchar(1024) DEFAULT NULL,
    `VNCProxyPort` int DEFAULT NULL,
    `VNCProxyType` varchar(20) DEFAULT NULL,
    `VNCProxyUsername` varchar(512) DEFAULT NULL,
    `VNCSmartSizeMode` varchar(20) DEFAULT NULL,
    `VNCViewOnly` tinyint(1) NOT NULL,
    `VmId` varchar(512) DEFAULT NULL,
    `ICAEncryptionStrength` varchar(32) NOT NULL,
    `InheritAutomaticResize` tinyint(1) NOT NULL,
    `InheritCacheBitmaps` tinyint(1) NOT NULL,
    `InheritColors` tinyint(1) NOT NULL,
    `InheritDescription` tinyint(1) NOT NULL,
    `InheritDisableCursorBlinking` tinyint(1) NOT NULL,
    `InheritDisableCursorShadow` tinyint(1) NOT NULL,
    `InheritDisableFullWindowDrag` tinyint(1) NOT NULL,
    `InheritDisableMenuAnimations` tinyint(1) NOT NULL,
    `InheritDisplayThemes` tinyint(1) NOT NULL,
    `InheritDisplayWallpaper` tinyint(1) NOT NULL,
    `InheritDomain` tinyint(1) NOT NULL,
    `InheritEnableDesktopComposition` tinyint(1) NOT NULL,
    `InheritEnableFontSmoothing` tinyint(1) NOT NULL,
    `InheritExtApp` tinyint(1) NOT NULL,
    `InheritFavorite` tinyint(1) NOT NULL,
    `InheritICAEncryptionStrength` tinyint(1) NOT NULL,
    `InheritIcon` tinyint(1) NOT NULL,
    `InheritLoadBalanceInfo` tinyint(1) NOT NULL,
    `InheritMacAddress` tinyint(1) NOT NULL,
    `InheritOpeningCommand` tinyint(1) NOT NULL,
    `InheritPanel` tinyint(1) NOT NULL,
    `InheritPassword` tinyint(1) NOT NULL,
    `InheritPort` tinyint(1) NOT NULL,
    `InheritPostExtApp` tinyint(1) NOT NULL,
    `InheritPreExtApp` tinyint(1) NOT NULL,
    `InheritProtocol` tinyint(1) NOT NULL,
    `InheritPuttySession` tinyint(1) NOT NULL,
    `InheritRDGatewayDomain` tinyint(1) NOT NULL,
    `InheritRDGatewayHostname` tinyint(1) NOT NULL,
    `InheritRDGatewayPassword` tinyint(1) NOT NULL,
    `InheritRDGatewayUsageMethod` tinyint(1) NOT NULL,
    `InheritRDGatewayUseConnectionCredentials` tinyint(1) NOT NULL,
    `InheritRDGatewayExternalCredentialProvider` tinyint(1) NOT NULL,
    `InheritRDGatewayUsername` tinyint(1) NOT NULL,
    `InheritRDGatewayUserViaAPI` tinyint(1) NOT NULL,
    `InheritRDPAlertIdleTimeout` tinyint(1) NOT NULL,
    `InheritRDPAuthenticationLevel` tinyint(1) NOT NULL,
    `InheritRDPMinutesToIdleTimeout` tinyint(1) NOT NULL,
    `InheritRdpVersion` tinyint(1) NOT NULL,
    `InheritRedirectAudioCapture` tinyint(1) NOT NULL,
    `InheritRedirectClipboard` tinyint(1) NOT NULL,
    `InheritRedirectDiskDrives` tinyint(1) NOT NULL,
    `InheritRedirectKeys` tinyint(1) NOT NULL,
    `InheritRedirectPorts` tinyint(1) NOT NULL,
    `InheritRedirectPrinters` tinyint(1) NOT NULL,
    `InheritRedirectSmartCards` tinyint(1) NOT NULL,
    `InheritRedirectSound` tinyint(1) NOT NULL,
    `InheritRenderingEngine` tinyint(1) NOT NULL,
    `InheritResolution` tinyint(1) NOT NULL,
    `InheritSSHOptions` tinyint(1) NOT NULL,
    `InheritSSHTunnelConnectionName` tinyint(1) NOT NULL,
    `InheritSoundQuality` tinyint(1) NOT NULL,
    `InheritUseConsoleSession` tinyint(1) NOT NULL,
    `InheritUseCredSsp` tinyint(1) NOT NULL,
    `InheritUseRestrictedAdmin` tinyint(1) NOT NULL,
    `InheritUseRCG` tinyint(1) NOT NULL,
    `InheritExternalCredentialProvider` tinyint(1) NOT NULL,
    `InheritUserViaAPI` tinyint(1) NOT NULL,
    `UseRestrictedAdmin` tinyint(1) NOT NULL,
    `UseRCG` tinyint(1) NOT NULL,
    `InheritUseEnhancedMode` tinyint(1) DEFAULT NULL,
    `InheritUseVmId` tinyint(1) DEFAULT NULL,
    `InheritUserField` tinyint(1) NOT NULL,
    `InheritUsername` tinyint(1) NOT NULL,
    `InheritVNCAuthMode` tinyint(1) NOT NULL,
    `InheritVNCColors` tinyint(1) NOT NULL,
    `InheritVNCCompression` tinyint(1) NOT NULL,
    `InheritVNCEncoding` tinyint(1) NOT NULL,
    `InheritVNCProxyIP` tinyint(1) NOT NULL,
    `InheritVNCProxyPassword` tinyint(1) NOT NULL,
    `InheritVNCProxyPort` tinyint(1) NOT NULL,
    `InheritVNCProxyType` tinyint(1) NOT NULL,
    `InheritVNCProxyUsername` tinyint(1) NOT NULL,
    `InheritVNCSmartSizeMode` tinyint(1) NOT NULL,
    `InheritVNCViewOnly` tinyint(1) NOT NULL,
    `InheritVmId` tinyint(1) NOT NULL,
    `StartProgram` varchar(512) DEFAULT NULL,
    `StartProgramWorkDir` varchar(512) DEFAULT NULL,
    `EC2Region` varchar(32) DEFAULT NULL,
    `EC2InstanceId` varchar(32) DEFAULT NULL,
    `ExternalCredentialProvider` varchar(256) DEFAULT NULL,
    `ExternalAddressProvider` varchar(256) DEFAULT NULL,
    PRIMARY KEY (`ConstantID`),
    UNIQUE KEY `ID_UNIQUE` (`ID`),
    UNIQUE KEY `ConstantID_UNIQUE` (`ConstantID`)
) ENGINE=InnoDB AUTO_INCREMENT=3324 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblRoot`
--

DROP TABLE IF EXISTS `tblRoot`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tblRoot` (
    `Name` varchar(2048) NOT NULL,
    `Export` tinyint(1) NOT NULL,
    `Protected` varchar(4048) NOT NULL,
    `ConfVersion` varchar(15) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tblUpdate`
--

DROP TABLE IF EXISTS `tblUpdate`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tblUpdate` (
    `LastUpdate` datetime(3) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;


/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
";
            }
            else
            {
                throw new Exception("Unknown database backend");
            }

            DbCommand cmd = databaseConnector.DbCommand(sql);
            cmd.ExecuteNonQuery();
        }
        
    }
}

//// MySql.Data.MySqlClient.MySqlException: 'You have an error in your SQL syntax; check the manual that corresponds to your MySQL server version for the right syntax to use near '(`ConstantID`),
//UNIQUE(`ID`)
//    ) ENGINE = InnoDB AUTO_INCREMENT = 3324 DEFAULT ' at line 156'