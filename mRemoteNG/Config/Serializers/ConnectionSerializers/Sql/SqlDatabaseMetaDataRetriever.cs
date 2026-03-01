using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Globalization;
using System.Runtime.Versioning;
using System.Security;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Messages;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Sql
{
    [SupportedOSPlatform("windows")]
    public class SqlDatabaseMetaDataRetriever : ISqlDatabaseMetaDataRetriever
    {
        public SqlConnectionListMetaData? GetDatabaseMetaData(IDatabaseConnector databaseConnector)
        {
            SqlConnectionListMetaData metaData;
            DbDataReader? dbDataReader = null;

            try
            {
                if (!databaseConnector.IsConnected)
                    databaseConnector.Connect();

                if (!DoesDbTableExist(databaseConnector, "tblRoot"))
                {
                    // tblRoot is absent.  Before wiping anything, check whether tblCons
                    // already exists — if it does the database is in an inconsistent state
                    // (metadata lost but connections are present).  Attempting to
                    // InitializeDatabaseSchema would DROP tblCons and erase all connections,
                    // so throw instead to surface the problem without causing data loss (#1784).
                    if (DoesDbTableExist(databaseConnector, "tblCons"))
                    {
                        throw new InvalidOperationException(
                            "Database is in an inconsistent state: tblCons exists but tblRoot is missing. " +
                            "Load aborted to prevent data loss. Please restore the tblRoot table or " +
                            "recreate the database schema manually.");
                    }

                    // Truly new/empty database — safe to initialize the schema.
                    InitializeDatabaseSchema(databaseConnector);
                }
                else
                {
                    UpgradeSchema(databaseConnector);
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
                    Export = dbDataReader["Export"] != DBNull.Value && Convert.ToBoolean(dbDataReader["Export"], CultureInfo.InvariantCulture),
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
            WriteDatabaseMetaData(rootTreeNode, databaseConnector, null);
        }

        public void WriteDatabaseMetaData(RootNodeInfo rootTreeNode, IDatabaseConnector databaseConnector, DbTransaction? transaction)
        {
            LegacyRijndaelCryptographyProvider cryptographyProvider = new();

            string strProtected;

            if (rootTreeNode != null)
            {
                if (rootTreeNode.Password)
                {
                    SecureString password = rootTreeNode.PasswordString.ConvertToSecureString();

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

            bool mustDisposeTransaction = false;
            if (transaction == null)
            {
                transaction = databaseConnector.DbConnection().BeginTransaction();
                mustDisposeTransaction = true;
            }

            try
            {
                DbCommand cmd = databaseConnector.DbCommand("DELETE FROM tblRoot");
                cmd.Transaction = transaction;
                cmd.ExecuteNonQuery();

                if (rootTreeNode != null)
                {
                    cmd = databaseConnector.DbCommand(
                            "INSERT INTO tblRoot (Name, Export, Protected, ConfVersion) VALUES(@Name, 0, @Protected, @ConfVersion)");
                    cmd.Transaction = transaction;

                    DbParameter nameParam = cmd.CreateParameter();
                    nameParam.ParameterName = "@Name";
                    nameParam.Value = rootTreeNode.Name;
                    cmd.Parameters.Add(nameParam);

                    DbParameter protectedParam = cmd.CreateParameter();
                    protectedParam.ParameterName = "@Protected";
                    protectedParam.Value = strProtected;
                    cmd.Parameters.Add(protectedParam);

                    DbParameter confVersionParam = cmd.CreateParameter();
                    confVersionParam.ParameterName = "@ConfVersion";
                    confVersionParam.Value = ConnectionsFileInfo.ConnectionFileVersion.ToString();
                    cmd.Parameters.Add(confVersionParam);

                    cmd.ExecuteNonQuery();
                }
                else
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, $"UpdateRootNodeTable: rootTreeNode was null. Could not insert!");
                }

                if (mustDisposeTransaction)
                {
                    transaction.Commit();
                }
            }
            catch
            {
                if (mustDisposeTransaction)
                {
                    transaction.Rollback();
                }
                throw;
            }
            finally
            {
                if (mustDisposeTransaction)
                {
                    transaction.Dispose();
                }
            }
        }

        private static bool IsValidTableName(string tableName)
        {
            // Table names should only contain alphanumeric characters and underscores
            // This prevents SQL injection when table names must be used directly in queries
            if (string.IsNullOrWhiteSpace(tableName))
                return false;

            foreach (char c in tableName)
            {
                if (!char.IsLetterOrDigit(c) && c != '_')
                    return false;
            }

            return true;
        }

        private static bool DoesDbTableExist(IDatabaseConnector databaseConnector, string tableName)
        {
            bool exists;

            try
            {
                // ANSI SQL way.  Works in PostgreSQL, MSSQL, MySQL.
                string database_name = Properties.OptionsDBsPage.Default.SQLDatabaseName;
                DbCommand cmd = databaseConnector.DbCommand("select case when exists((select * from information_schema.tables where table_name = @TableName and table_schema = @DatabaseName)) then 1 else 0 end");
                
                DbParameter tableNameParam = cmd.CreateParameter();
                tableNameParam.ParameterName = "@TableName";
                tableNameParam.Value = tableName;
                cmd.Parameters.Add(tableNameParam);

                DbParameter databaseNameParam = cmd.CreateParameter();
                databaseNameParam.ParameterName = "@DatabaseName";
                databaseNameParam.Value = database_name;
                cmd.Parameters.Add(databaseNameParam);

                short cmdResult = Convert.ToInt16(cmd.ExecuteScalar(), CultureInfo.InvariantCulture);
                exists = (cmdResult == 1);

                // If information_schema reports the table as absent, verify with a direct
                // query before concluding it doesn't exist.  This guards against false
                // negatives caused by schema-name case-sensitivity (Linux MySQL) or
                // insufficient information_schema permissions — either of which would
                // otherwise trigger InitializeDatabaseSchema and erase all data (#1784).
                if (!exists && IsValidTableName(tableName))
                {
                    try
                    {
                        DbCommand verifyCmd = databaseConnector.DbCommand($"select 1 from {tableName} where 1 = 0");
                        verifyCmd.ExecuteNonQuery();
                        exists = true; // table is reachable — information_schema was wrong
                    }
                    catch
                    {
                        exists = false; // table truly does not exist
                    }
                }
            }
            catch
            {
                try
                {
                    // Other RDBMS.  Graceful degradation
                    // Note: Table names cannot be parameterized in standard SQL.
                    // Validate tableName to prevent SQL injection
                    if (!IsValidTableName(tableName))
                    {
                        exists = false;
                    }
                    else
                    {
                        exists = true;
                        DbCommand cmdOthers = databaseConnector.DbCommand($"select 1 from {tableName} where 1 = 0");
                        cmdOthers.ExecuteNonQuery();
                    }
                }
                catch
                {
                    exists = false;
                }
            }

            return exists;
        }

        private static void InitializeDatabaseSchema(IDatabaseConnector databaseConnector)
        {
            string sql;
            
            if (databaseConnector.GetType() == typeof(MSSqlDatabaseConnector)
                || databaseConnector.GetType() == typeof(OdbcDatabaseConnector))
            {
                // *********************************
                // ********* MICROSOFT SQL *********
                // *********************************

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
    [ConstantID] nvarchar(128) NOT NULL PRIMARY KEY,
    [PositionID] int NOT NULL,
    [ParentID] nvarchar(128),
    [LastChange] datetime NOT NULL,
    [Name] nvarchar(128) NOT NULL,
    [Type] nvarchar(32) NOT NULL,
    [Expanded] bit NOT NULL,
    [AutomaticResize] bit NOT NULL DEFAULT ((1)),
    [CacheBitmaps] bit NOT NULL,
    [Colors] nvarchar(32) NOT NULL,
    [ConnectToConsole] bit NOT NULL,
    [Connected] bit NOT NULL,
    [Description] nvarchar(1024),
    [DisableCursorBlinking] bit NOT NULL,
    [DisableCursorShadow] bit NOT NULL,
    [DisableFullWindowDrag] bit NOT NULL,
    [DisableMenuAnimations] bit NOT NULL,
    [DisplayThemes] bit NOT NULL,
    [DisplayWallpaper] bit NOT NULL,
    [Domain] nvarchar(512),
    [EnableDesktopComposition] bit NOT NULL,
    [EnableFontSmoothing] bit NOT NULL,
    [ExtApp] nvarchar(256),
    [Favorite] tinyint NOT NULL,
    [Hostname] nvarchar(512),
    [Icon] nvarchar(128) NOT NULL,
    [LoadBalanceInfo] nvarchar(1024),
    [MacAddress] nvarchar(32),
    [OpeningCommand] nvarchar(512),
    [Panel] nvarchar(128) NOT NULL,
    [Password] nvarchar(1024),
    [Port] int NOT NULL,
    [PostExtApp] nvarchar(256),
    [PreExtApp] nvarchar(256),
    [Protocol] nvarchar(32) NOT NULL,
    [PuttySession] nvarchar(128),
    [RDGatewayDomain] nvarchar(512),
    [RDGatewayHostname] nvarchar(512),
    [RDGatewayPassword] nvarchar(1024),
    [RDGatewayUsageMethod] nvarchar(32) NOT NULL,
    [RDGatewayUseConnectionCredentials] nvarchar(32) NOT NULL,
    [RDGatewayUsername] nvarchar(512),
    [RDPAlertIdleTimeout] bit NOT NULL,
    [RDPAuthenticationLevel] nvarchar(32) NOT NULL,
    [RDPMinutesToIdleTimeout] int NOT NULL,
    [RdpVersion] nvarchar(10) NULL,
    [RedirectAudioCapture] bit NOT NULL,
    [RedirectClipboard] bit NOT NULL,
    [RedirectDiskDrives] nvarchar(32) DEFAULT NULL,
    [RedirectDiskDrivesCustom] nvarchar(32) DEFAULT NULL,
    [RedirectKeys] bit NOT NULL,
    [RedirectPorts] bit NOT NULL,
    [RedirectPrinters] bit NOT NULL,
    [RedirectSmartCards] bit NOT NULL,
    [RedirectSound] nvarchar(64) NOT NULL,
    [RenderingEngine] nvarchar(32) NULL,
    [Resolution] nvarchar(32) NOT NULL,
    [SSHOptions] nvarchar(1024) NOT NULL,
    [SSHTunnelConnectionName] nvarchar(128) NOT NULL,
    [SoundQuality] nvarchar(20) NOT NULL,
    [UseCredSsp] bit NOT NULL,
    [UseEnhancedMode] bit NOT NULL,
    [UseVmId] bit NOT NULL,
    [UserField] nvarchar(256) NULL,
    [Username] nvarchar(512) NULL,
    [VNCAuthMode] nvarchar(10) NULL,
    [VNCColors] nvarchar(10) NULL,
    [VNCCompression] nvarchar(10) NULL,
    [VNCEncoding] nvarchar(20) NULL,
    [VNCProxyIP] nvarchar(128) NULL,
    [VNCProxyPassword] nvarchar(1024) NULL,
    [VNCProxyPort] int NULL,
    [VNCProxyType] nvarchar(20) NULL,
    [VNCProxyUsername] nvarchar(512) NULL,
    [VNCSmartSizeMode] nvarchar(20) NULL,
    [VNCViewOnly] bit NOT NULL,
    [VmId] nvarchar(100) NULL,
    [ICAEncryptionStrength] nvarchar(32) NOT NULL,
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
    [InheritRedirectDiskDrivesCustom] bit NOT NULL,
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
    [StartProgram] nvarchar(512) NULL,
    [StartProgramWorkDir] nvarchar(512) NULL,
    [EC2Region] nvarchar(32) NULL,
    [EC2InstanceId] nvarchar(32) NULL,
    [ExternalCredentialProvider] nvarchar(256) NULL,
    [ExternalAddressProvider] nvarchar(256) NULL,
    [UserViaAPI] nvarchar(512) NOT NULL,
    [User] nvarchar(512) NULL,
    [Role] nvarchar(512) NULL,
    [RowVersion] rowversion NOT NULL
) ON [PRIMARY]

CREATE TABLE [dbo].[tblRoot] (
        [Name] [nvarchar] (2048) NOT NULL,
        [Export] [bit] NOT NULL,
        [Protected] [nvarchar] (4048) NOT NULL,
        [ConfVersion] [nvarchar] (15) NOT NULL
) ON [PRIMARY]

CREATE TABLE [dbo].[tblUpdate] (
        [LastUpdate] [datetime] NULL
) ON [PRIMARY]

CREATE TABLE [dbo].[tblExternalTools] (
        [ID] int NOT NULL IDENTITY(1,1),
        [DisplayName] [nvarchar] (256) NOT NULL,
        [FileName] [nvarchar] (1024) NOT NULL,
        [IconPath] [nvarchar] (1024) NOT NULL DEFAULT '',
        [Arguments] [nvarchar] (2048) NOT NULL DEFAULT '',
        [WorkingDir] [nvarchar] (1024) NOT NULL DEFAULT '',
        [WaitForExit] [bit] NOT NULL DEFAULT 0,
        [TryIntegrate] [bit] NOT NULL DEFAULT 0,
        [RunElevated] [bit] NOT NULL DEFAULT 0,
        [ShowOnToolbar] [bit] NOT NULL DEFAULT 1,
        [Category] [nvarchar] (256) NOT NULL DEFAULT '',
        [RunOnStartup] [bit] NOT NULL DEFAULT 0,
        [StopOnShutdown] [bit] NOT NULL DEFAULT 0,
        [Hotkey] [int] NOT NULL DEFAULT 0
) ON [PRIMARY]
";
            }
            else if (databaseConnector.GetType() == typeof(MySqlDatabaseConnector))
            {
                // **************************
                // ********* MY SQL *********
                // **************************

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
    `Expanded` tinyint NOT NULL,
    `AutomaticResize` tinyint NOT NULL DEFAULT 1,
    `CacheBitmaps` tinyint NOT NULL,
    `Colors` varchar(32) NOT NULL,
    `ConnectToConsole` tinyint NOT NULL,
    `Connected` tinyint NOT NULL,
    `Description` varchar(1024) DEFAULT NULL,
    `DisableCursorBlinking` tinyint NOT NULL,
    `DisableCursorShadow` tinyint NOT NULL,
    `DisableFullWindowDrag` tinyint NOT NULL,
    `DisableMenuAnimations` tinyint NOT NULL,
    `DisplayThemes` tinyint NOT NULL,
    `DisplayWallpaper` tinyint NOT NULL,
    `Domain` varchar(512) DEFAULT NULL,
    `EnableDesktopComposition` tinyint NOT NULL,
    `EnableFontSmoothing` tinyint NOT NULL,
    `ExtApp` varchar(256) DEFAULT NULL,
    `Favorite` tinyint NOT NULL,
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
    `RDPAlertIdleTimeout` tinyint NOT NULL,
    `RDPAuthenticationLevel` varchar(32) NOT NULL,
    `RDPMinutesToIdleTimeout` int(11) NOT NULL,
    `RdpVersion` varchar(10) DEFAULT NULL,
    `RedirectAudioCapture` tinyint NOT NULL,
    `RedirectClipboard` tinyint NOT NULL,
    `RedirectDiskDrives` varchar(32) DEFAULT NULL,
    `RedirectDiskDrivesCustom` varchar(32) DEFAULT NULL,
    `RedirectKeys` tinyint NOT NULL,
    `RedirectPorts` tinyint NOT NULL,
    `RedirectPrinters` tinyint NOT NULL,
    `RedirectSmartCards` tinyint NOT NULL,
    `RedirectSound` varchar(64) NOT NULL,
    `RenderingEngine` varchar(32) DEFAULT NULL,
    `Resolution` varchar(32) NOT NULL,
    `SSHOptions` varchar(1024) NOT NULL,
    `SSHTunnelConnectionName` varchar(128) NOT NULL,
    `SoundQuality` varchar(20) NOT NULL,
    `UseCredSsp` tinyint NOT NULL,
    `UseEnhancedMode` tinyint NOT NULL,
    `UseVmId` tinyint NOT NULL,
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
    `VNCViewOnly` tinyint NOT NULL,
    `VmId` varchar(512) DEFAULT NULL,
    `ICAEncryptionStrength` varchar(32) NOT NULL,
    `InheritAutomaticResize` tinyint NOT NULL,
    `InheritCacheBitmaps` tinyint NOT NULL,
    `InheritColors` tinyint NOT NULL,
    `InheritDescription` tinyint NOT NULL,
    `InheritDisableCursorBlinking` tinyint NOT NULL,
    `InheritDisableCursorShadow` tinyint NOT NULL,
    `InheritDisableFullWindowDrag` tinyint NOT NULL,
    `InheritDisableMenuAnimations` tinyint NOT NULL,
    `InheritDisplayThemes` tinyint NOT NULL,
    `InheritDisplayWallpaper` tinyint NOT NULL,
    `InheritDomain` tinyint NOT NULL,
    `InheritEnableDesktopComposition` tinyint NOT NULL,
    `InheritEnableFontSmoothing` tinyint NOT NULL,
    `InheritExtApp` tinyint NOT NULL,
    `InheritFavorite` tinyint NOT NULL,
    `InheritICAEncryptionStrength` tinyint NOT NULL,
    `InheritIcon` tinyint NOT NULL,
    `InheritLoadBalanceInfo` tinyint NOT NULL,
    `InheritMacAddress` tinyint NOT NULL,
    `InheritOpeningCommand` tinyint NOT NULL,
    `InheritPanel` tinyint NOT NULL,
    `InheritPassword` tinyint NOT NULL,
    `InheritPort` tinyint NOT NULL,
    `InheritPostExtApp` tinyint NOT NULL,
    `InheritPreExtApp` tinyint NOT NULL,
    `InheritProtocol` tinyint NOT NULL,
    `InheritPuttySession` tinyint NOT NULL,
    `InheritRDGatewayDomain` tinyint NOT NULL,
    `InheritRDGatewayHostname` tinyint NOT NULL,
    `InheritRDGatewayPassword` tinyint NOT NULL,
    `InheritRDGatewayUsageMethod` tinyint NOT NULL,
    `InheritRDGatewayUseConnectionCredentials` tinyint NOT NULL,
    `InheritRDGatewayExternalCredentialProvider` tinyint NOT NULL,
    `InheritRDGatewayUsername` tinyint NOT NULL,
    `InheritRDGatewayUserViaAPI` tinyint NOT NULL,
    `InheritRDPAlertIdleTimeout` tinyint NOT NULL,
    `InheritRDPAuthenticationLevel` tinyint NOT NULL,
    `InheritRDPMinutesToIdleTimeout` tinyint NOT NULL,
    `InheritRdpVersion` tinyint NOT NULL,
    `InheritRedirectAudioCapture` tinyint NOT NULL,
    `InheritRedirectClipboard` tinyint NOT NULL,
    `InheritRedirectDiskDrives` tinyint NOT NULL,
    `InheritRedirectDiskDrivesCustom` tinyint NOT NULL,
    `InheritRedirectKeys` tinyint NOT NULL,
    `InheritRedirectPorts` tinyint NOT NULL,
    `InheritRedirectPrinters` tinyint NOT NULL,
    `InheritRedirectSmartCards` tinyint NOT NULL,
    `InheritRedirectSound` tinyint NOT NULL,
    `InheritRenderingEngine` tinyint NOT NULL,
    `InheritResolution` tinyint NOT NULL,
    `InheritSSHOptions` tinyint NOT NULL,
    `InheritSSHTunnelConnectionName` tinyint NOT NULL,
    `InheritSoundQuality` tinyint NOT NULL,
    `InheritUseConsoleSession` tinyint NOT NULL,
    `InheritUseCredSsp` tinyint NOT NULL,
    `InheritUseRestrictedAdmin` tinyint NOT NULL,
    `InheritUseRCG` tinyint NOT NULL,
    `InheritExternalCredentialProvider` tinyint NOT NULL,
    `InheritUserViaAPI` tinyint NOT NULL,
    `UseRestrictedAdmin` tinyint NOT NULL,
    `UseRCG` tinyint NOT NULL,
    `InheritUseEnhancedMode` tinyint DEFAULT NULL,
    `InheritUseVmId` tinyint DEFAULT NULL,
    `InheritUserField` tinyint NOT NULL,
    `InheritUsername` tinyint NOT NULL,
    `InheritVNCAuthMode` tinyint NOT NULL,
    `InheritVNCColors` tinyint NOT NULL,
    `InheritVNCCompression` tinyint NOT NULL,
    `InheritVNCEncoding` tinyint NOT NULL,
    `InheritVNCProxyIP` tinyint NOT NULL,
    `InheritVNCProxyPassword` tinyint NOT NULL,
    `InheritVNCProxyPort` tinyint NOT NULL,
    `InheritVNCProxyType` tinyint NOT NULL,
    `InheritVNCProxyUsername` tinyint NOT NULL,
    `InheritVNCSmartSizeMode` tinyint NOT NULL,
    `InheritVNCViewOnly` tinyint NOT NULL,
    `InheritVmId` tinyint NOT NULL,
    `StartProgram` varchar(512) DEFAULT NULL,
    `StartProgramWorkDir` varchar(512) DEFAULT NULL,
    `EC2Region` varchar(32) DEFAULT NULL,
    `EC2InstanceId` varchar(32) DEFAULT NULL,
    `ExternalCredentialProvider` varchar(256) DEFAULT NULL,
    `ExternalAddressProvider` varchar(256) DEFAULT NULL,
    `UserViaAPI` varchar(512) NOT NULL,
    `User` varchar(512) DEFAULT NULL,
    `Role` varchar(512) DEFAULT NULL,
    `RowVersion` TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
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
    `Export` tinyint NOT NULL,
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

--
-- Table structure for table `tblExternalTools`
--

DROP TABLE IF EXISTS `tblExternalTools`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tblExternalTools` (
    `ID` int NOT NULL AUTO_INCREMENT,
    `DisplayName` varchar(256) NOT NULL,
    `FileName` varchar(1024) NOT NULL,
    `IconPath` varchar(1024) NOT NULL DEFAULT '',
    `Arguments` varchar(2048) NOT NULL DEFAULT '',
    `WorkingDir` varchar(1024) NOT NULL DEFAULT '',
    `WaitForExit` tinyint NOT NULL DEFAULT 0,
    `TryIntegrate` tinyint NOT NULL DEFAULT 0,
    `RunElevated` tinyint NOT NULL DEFAULT 0,
    `ShowOnToolbar` tinyint NOT NULL DEFAULT 1,
    `Category` varchar(256) NOT NULL DEFAULT '',
    `RunOnStartup` tinyint NOT NULL DEFAULT 0,
    `StopOnShutdown` tinyint NOT NULL DEFAULT 0,
    `Hotkey` int NOT NULL DEFAULT 0,
    PRIMARY KEY (`ID`)
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
                throw new NotSupportedException("Unknown database backend");
            }

            DbCommand cmd = databaseConnector.DbCommand(sql);
            cmd.ExecuteNonQuery();
        }

        private static void UpgradeSchema(IDatabaseConnector databaseConnector)
        {
            try
            {
                if (databaseConnector.GetType() == typeof(MSSqlDatabaseConnector))
                {
                    UpgradeMssqlSchema(databaseConnector);
                }

                if (!DoesColumnExist(databaseConnector, "tblCons", "User"))
                {
                    string sql = databaseConnector.GetType() == typeof(MySqlDatabaseConnector)
                        ? "ALTER TABLE tblCons ADD COLUMN `User` varchar(512) DEFAULT NULL"
                        : "ALTER TABLE tblCons ADD [User] nvarchar(512) NULL";
                    databaseConnector.DbCommand(sql).ExecuteNonQuery();
                }

                if (!DoesColumnExist(databaseConnector, "tblCons", "Role"))
                {
                    string sql = databaseConnector.GetType() == typeof(MySqlDatabaseConnector)
                        ? "ALTER TABLE tblCons ADD COLUMN `Role` varchar(512) DEFAULT NULL"
                        : "ALTER TABLE tblCons ADD [Role] nvarchar(512) NULL";
                    databaseConnector.DbCommand(sql).ExecuteNonQuery();
                }

                if (databaseConnector.GetType() == typeof(MSSqlDatabaseConnector))
                {
                     if (!DoesColumnExist(databaseConnector, "tblCons", "RowVersion"))
                     {
                         databaseConnector.DbCommand("ALTER TABLE tblCons ADD [RowVersion] rowversion NOT NULL").ExecuteNonQuery();
                     }
                }
                else if (databaseConnector.GetType() == typeof(MySqlDatabaseConnector))
                {
                    if (!DoesColumnExist(databaseConnector, "tblCons", "RowVersion"))
                    {
                        databaseConnector.DbCommand("ALTER TABLE tblCons ADD COLUMN `RowVersion` TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP").ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Schema upgrade failed", ex);
            }
        }

        private static void UpgradeMssqlSchema(IDatabaseConnector databaseConnector)
        {
            DataTable expectedSchema = DataTableSerializer.GetExpectedSchema();

            foreach (DataColumn expectedColumn in expectedSchema.Columns)
            {
                if (DoesColumnExist(databaseConnector, "tblCons", expectedColumn.ColumnName))
                {
                    continue;
                }

                string sqlType = expectedColumn.DataType switch
                {
                    Type t when t == typeof(bool) => "bit",
                    Type t when t == typeof(int) => "int",
                    Type t when t == typeof(SqlDateTime) || t == typeof(DateTime) => "datetime",
                    Type t when t == typeof(string) => "nvarchar(4000)",
                    _ => "nvarchar(4000)",
                };

                databaseConnector.DbCommand($"ALTER TABLE [tblCons] ADD [{expectedColumn.ColumnName}] {sqlType} NULL").ExecuteNonQuery();
            }
        }

        private static bool DoesColumnExist(IDatabaseConnector databaseConnector, string tableName, string columnName)
        {
             try
             {
                 string databaseName = Properties.OptionsDBsPage.Default.SQLDatabaseName;
                 // INFORMATION_SCHEMA is standard
                 string sql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName AND COLUMN_NAME = @ColumnName";
                 
                 // However, some DBs might need database name filter if table names are not unique across schemas
                 if (databaseConnector.GetType() == typeof(MySqlDatabaseConnector))
                 {
                     sql += " AND TABLE_SCHEMA = @DatabaseName";
                 }

                 DbCommand cmd = databaseConnector.DbCommand(sql);
                 
                 DbParameter tableNameParam = cmd.CreateParameter();
                 tableNameParam.ParameterName = "@TableName";
                 tableNameParam.Value = tableName;
                 cmd.Parameters.Add(tableNameParam);

                 DbParameter columnNameParam = cmd.CreateParameter();
                 columnNameParam.ParameterName = "@ColumnName";
                 columnNameParam.Value = columnName;
                 cmd.Parameters.Add(columnNameParam);

                 if (databaseConnector.GetType() == typeof(MySqlDatabaseConnector))
                 {
                     DbParameter dbNameParam = cmd.CreateParameter();
                     dbNameParam.ParameterName = "@DatabaseName";
                     dbNameParam.Value = databaseName;
                     cmd.Parameters.Add(dbNameParam);
                 }

                 object? result = cmd.ExecuteScalar();
                 return result != null && Convert.ToInt32(result, CultureInfo.InvariantCulture) > 0;
             }
             catch
             {
                 return false;
             }
        }
        
    }
}
