.. _sql_configuration:

*****************
SQL Configuration
*****************

.. warning::

    The SQL feature is in an early beta stage and not intended for use in an productive environment! I recommend you to do a full backup of your connections and settings before switching to SQL Server.

Supported Databases
===================

The list below includes databases that are officially supported. Others may already work and this list may expand with future updates.

- MSSQL
- MySQL

Steps to configure your SQL Server
==================================
- Create a new Database called "mRemoteNG" on your SQL Server.
- Run the SQL Script for your DB type listed below in topic (SQL Table creation Scripts) on the newly created Database.
- Give the users that you want to grant access to the mRemoteNG Connections Database Read/Write permissions on the Database.

Steps to configure mRemoteNG for SQL
====================================
- Start mRemoteNG if it's not already running.
- Go to Tools - Options - SQL Server
- Check the box that says "Use SQL Server to load & save connections".
- Fill in your SQL Server hostname or ip address.
- If you do not use your Windows logon info to authenticate against the SQL Server fill in the correct Username and Password.
- Click OK to apply the changes. The main window title should now change to "mRemoteNG | SQL Server".
- Now click on File - Save to update the tables on your SQL Server with the data from the loaded connections xml file. (Do not click File - New, this doesn't work yet)
- You should now be able to do everything you were able to do with the XML storage plus see the changes live on another mRemoteNG instance that is connected to the same Database.

SQL Table creation Scripts
==========================

MSSQL
-----

.. code-block:: sql

    if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tblCons]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
    drop table [dbo].[tblCons]
    GO
    
    if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tblRoot]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
    drop table [dbo].[tblRoot]
    GO
    
    if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tblUpdate]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
    drop table [dbo].[tblUpdate]
    GO
    
    CREATE TABLE [dbo].[tblCons] (
    	[ID] [int] IDENTITY (1001, 1) NOT NULL ,
    	[ConstantID] [varchar] (128) NULL ,
    	[PositionID] [int] NOT NULL ,
    	[ParentID] [varchar] (128) NULL ,
    	[LastChange] [datetime] NOT NULL ,
    	[Name] [varchar] (128) NOT NULL ,
    	[Type] [varchar] (32) NOT NULL ,
    	[Expanded] [bit] NOT NULL ,
    	[Description] [varchar] (1024) NULL ,
    	[Icon] [varchar] (128) NOT NULL ,
    	[Panel] [varchar] (128) NOT NULL ,
    	[Username] [varchar] (512) NULL ,
    	[DomainName] [varchar] (512) NULL ,
    	[Password] [varchar] (1024) NULL ,
    	[Hostname] [varchar] (512) NULL ,
    	[Protocol] [varchar] (32) NOT NULL ,
    	[PuttySession] [varchar] (128) NULL ,
    	[Port] [int] NOT NULL ,
    	[ConnectToConsole] [bit] NOT NULL ,
    	[UseCredSsp] [bit] NOT NULL ,
    	[RenderingEngine] [varchar] (10) NULL ,
    	[ICAEncryptionStrength] [varchar] (32) NOT NULL ,
    	[RDPAuthenticationLevel] [varchar] (32) NOT NULL ,
    	[RDPMinutesToIdleTimeout] [int] NOT NULL,
    	[RDPAlertIdleTimeout] [bit] NOT NULL,
    	[Colors] [varchar] (32) NOT NULL ,
    	[Resolution] [varchar] (32) NOT NULL ,
    	[DisplayWallpaper] [bit] NOT NULL ,
    	[DisplayThemes] [bit] NOT NULL ,
    	[EnableFontSmoothing] [bit] NOT NULL ,
    	[EnableDesktopComposition] [bit] NOT NULL ,
    	[CacheBitmaps] [bit] NOT NULL ,
    	[RedirectDiskDrives] [bit] NOT NULL ,
    	[RedirectPorts] [bit] NOT NULL ,
    	[RedirectPrinters] [bit] NOT NULL ,
    	[RedirectSmartCards] [bit] NOT NULL ,
    	[RedirectSound] [varchar] (64) NOT NULL ,
    	[SoundQuality] [varchar] (20) NOT NULL,
    	[RedirectKeys] [bit] NOT NULL ,
    	[Connected] [bit] NOT NULL ,
    	[PreExtApp] [varchar] (256) NULL ,
    	[PostExtApp] [varchar] (256) NULL ,
    	[MacAddress] [varchar] (32) NULL ,
    	[UserField] [varchar] (256) NULL ,
    	[ExtApp] [varchar] (256) NULL ,
    	[VNCCompression] [varchar] (10) NULL ,
    	[VNCEncoding] [varchar] (20) NULL ,
    	[VNCAuthMode] [varchar] (10) NULL ,
    	[VNCProxyType] [varchar] (20) NULL ,
    	[VNCProxyIP] [varchar] (128) NULL ,
    	[VNCProxyPort] [int] NULL ,
    	[VNCProxyUsername] [varchar] (512) NULL ,
    	[VNCProxyPassword] [varchar] (1024) NULL ,
    	[VNCColors] [varchar] (10) NULL ,
    	[VNCSmartSizeMode] [varchar] (20) NULL ,
    	[VNCViewOnly] [bit] NOT NULL ,
    	[RDGatewayUsageMethod] [varchar] (32) NOT NULL ,
    	[RDGatewayHostname] [varchar] (512) NULL ,
    	[RDGatewayUseConnectionCredentials] [varchar] (32) NOT NULL ,
    	[RDGatewayUsername] [varchar] (512) NULL ,
    	[RDGatewayPassword] [varchar] (1024) NULL ,
    	[RDGatewayDomain] [varchar] (512) NULL ,
    	[InheritCacheBitmaps] [bit] NOT NULL ,
    	[InheritColors] [bit] NOT NULL ,
    	[InheritDescription] [bit] NOT NULL ,
    	[InheritDisplayThemes] [bit] NOT NULL ,
    	[InheritDisplayWallpaper] [bit] NOT NULL ,
    	[InheritEnableFontSmoothing] [bit] NOT NULL ,
    	[InheritEnableDesktopComposition] [bit] NOT NULL ,
    	[InheritDomain] [bit] NOT NULL ,
    	[InheritIcon] [bit] NOT NULL ,
    	[InheritPanel] [bit] NOT NULL ,
    	[InheritPassword] [bit] NOT NULL ,
    	[InheritPort] [bit] NOT NULL ,
    	[InheritProtocol] [bit] NOT NULL ,
    	[InheritPuttySession] [bit] NOT NULL ,
    	[InheritRedirectDiskDrives] [bit] NOT NULL ,
    	[InheritRedirectKeys] [bit] NOT NULL ,
    	[InheritRedirectPorts] [bit] NOT NULL ,
    	[InheritRedirectPrinters] [bit] NOT NULL ,
    	[InheritRedirectSmartCards] [bit] NOT NULL ,
    	[InheritRedirectSound] [bit] NOT NULL ,
    	[InheritSoundQuality] [bit] NOT NULL,
    	[InheritResolution] [bit] NOT NULL ,
    	[InheritUseConsoleSession] [bit] NOT NULL ,
    	[InheritUseCredSsp] [bit] NOT NULL ,
    	[InheritRenderingEngine] [bit] NOT NULL ,
    	[InheritICAEncryptionStrength] [bit] NOT NULL ,
    	[InheritRDPAuthenticationLevel] [bit] NOT NULL ,
    	[InheritRDPMinutesToIdleTimeout] [bit] NOT NULL,
    	[InheritRDPAlertIdleTimeout] [bit] NOT NULL,
    	[InheritUsername] [bit] NOT NULL ,
    	[InheritPreExtApp] [bit] NOT NULL ,
    	[InheritPostExtApp] [bit] NOT NULL ,
    	[InheritMacAddress] [bit] NOT NULL ,
    	[InheritUserField] [bit] NOT NULL ,
    	[InheritExtApp] [bit] NOT NULL ,
    	[InheritVNCCompression] [bit] NOT NULL, 
    	[InheritVNCEncoding] [bit] NOT NULL ,
    	[InheritVNCAuthMode] [bit] NOT NULL ,
    	[InheritVNCProxyType] [bit] NOT NULL ,
    	[InheritVNCProxyIP] [bit] NOT NULL ,
    	[InheritVNCProxyPort] [bit] NOT NULL ,
    	[InheritVNCProxyUsername] [bit] NOT NULL ,
    	[InheritVNCProxyPassword] [bit] NOT NULL ,
    	[InheritVNCColors] [bit] NOT NULL ,
    	[InheritVNCSmartSizeMode] [bit] NOT NULL ,
    	[InheritVNCViewOnly] [bit] NOT NULL ,
    	[InheritRDGatewayUsageMethod] [bit] NOT NULL ,
    	[InheritRDGatewayHostname] [bit] NOT NULL ,
    	[InheritRDGatewayUseConnectionCredentials] [bit] NOT NULL ,
    	[InheritRDGatewayUsername] [bit] NOT NULL ,
    	[InheritRDGatewayPassword] [bit] NOT NULL ,
    	[InheritRDGatewayDomain] [bit] NOT NULL ,
    	[LoadBalanceInfo] [varchar] (1024) NULL ,
    	[AutomaticResize] [bit] NOT NULL DEFAULT 1 ,
    	[InheritLoadBalanceInfo] [bit] NOT NULL DEFAULT 0 ,
    	[InheritAutomaticResize] [bit] NOT NULL DEFAULT 0 ,
    	[RedirectClipboard] [bit] NOT NULL DEFAULT 0 ,
    	[InheritRedirectClipboard] [bit] NOT NULL DEFAULT 0
    ) ON [PRIMARY]
    GO
     
    CREATE TABLE [dbo].[tblRoot] (
    	[Name] [varchar] (2048) NOT NULL ,
    	[Export] [bit] NOT NULL ,
    	[Protected] [varchar] (4048) NOT NULL ,
    	[ConfVersion] [float] NOT NULL 
    ) ON [PRIMARY]
    GO
     
    CREATE TABLE [dbo].[tblUpdate] (
    	[LastUpdate] [datetime] NULL 
    ) ON [PRIMARY]
    GO
    
MYSQL
-----

.. code-block:: sql
   
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
      `ID` int(11) NOT NULL AUTO_INCREMENT,
      `ConstantID` varchar(128) DEFAULT NULL,
      `PositionID` int(11) NOT NULL,
      `ParentID` varchar(128) DEFAULT NULL,
      `LastChange` datetime NOT NULL,
      `Name` varchar(128) NOT NULL,
      `Type` varchar(32) NOT NULL,
      `Expanded` tinyint(1) NOT NULL,
      `Description` varchar(1024) DEFAULT NULL,
      `Icon` varchar(128) NOT NULL,
      `Panel` varchar(128) NOT NULL,
      `Username` varchar(512) DEFAULT NULL,
      `DomainName` varchar(512) DEFAULT NULL,
      `Password` varchar(1024) DEFAULT NULL,
      `Hostname` varchar(512) DEFAULT NULL,
      `Protocol` varchar(32) NOT NULL,
      `PuttySession` varchar(128) DEFAULT NULL,
      `Port` int(11) NOT NULL,
      `ConnectToConsole` tinyint(1) NOT NULL,
      `UseCredSsp` tinyint(1) NOT NULL,
      `RenderingEngine` varchar(10) DEFAULT NULL,
      `ICAEncryptionStrength` varchar(32) NOT NULL,
      `RDPAuthenticationLevel` varchar(32) NOT NULL,
      `RDPMinutesToIdleTimeout` int(11) NOT NULL,
      `RDPAlertIdleTimeout` tinyint(1) NOT NULL,
      `Colors` varchar(32) NOT NULL,
      `Resolution` varchar(32) NOT NULL,
      `DisplayWallpaper` tinyint(1) NOT NULL,
      `DisplayThemes` tinyint(1) NOT NULL,
      `EnableFontSmoothing` tinyint(1) NOT NULL,
      `EnableDesktopComposition` tinyint(1) NOT NULL,
      `CacheBitmaps` tinyint(1) NOT NULL,
      `RedirectDiskDrives` tinyint(1) NOT NULL,
      `RedirectPorts` tinyint(1) NOT NULL,
      `RedirectPrinters` tinyint(1) NOT NULL,
      `RedirectSmartCards` tinyint(1) NOT NULL,
      `RedirectSound` varchar(64) NOT NULL,
      `SoundQuality` varchar(20) NOT NULL,
      `RedirectKeys` tinyint(1) NOT NULL,
      `Connected` tinyint(1) NOT NULL,
      `PreExtApp` varchar(256) DEFAULT NULL,
      `PostExtApp` varchar(256) DEFAULT NULL,
      `MacAddress` varchar(32) DEFAULT NULL,
      `UserField` varchar(256) DEFAULT NULL,
      `ExtApp` varchar(256) DEFAULT NULL,
      `VNCCompression` varchar(10) DEFAULT NULL,
      `VNCEncoding` varchar(20) DEFAULT NULL,
      `VNCAuthMode` varchar(10) DEFAULT NULL,
      `VNCProxyType` varchar(20) DEFAULT NULL,
      `VNCProxyIP` varchar(128) DEFAULT NULL,
      `VNCProxyPort` int(11) DEFAULT NULL,
      `VNCProxyUsername` varchar(512) DEFAULT NULL,
      `VNCProxyPassword` varchar(1024) DEFAULT NULL,
      `VNCColors` varchar(10) DEFAULT NULL,
      `VNCSmartSizeMode` varchar(20) DEFAULT NULL,
      `VNCViewOnly` tinyint(1) NOT NULL,
      `RDGatewayUsageMethod` varchar(32) NOT NULL,
      `RDGatewayHostname` varchar(512) DEFAULT NULL,
      `RDGatewayUseConnectionCredentials` varchar(32) NOT NULL,
      `RDGatewayUsername` varchar(512) DEFAULT NULL,
      `RDGatewayPassword` varchar(1024) DEFAULT NULL,
      `RDGatewayDomain` varchar(512) DEFAULT NULL,
      `InheritCacheBitmaps` tinyint(1) NOT NULL,
      `InheritColors` tinyint(1) NOT NULL,
      `InheritDescription` tinyint(1) NOT NULL,
      `InheritDisplayThemes` tinyint(1) NOT NULL,
      `InheritDisplayWallpaper` tinyint(1) NOT NULL,
      `InheritEnableFontSmoothing` tinyint(1) NOT NULL,
      `InheritEnableDesktopComposition` tinyint(1) NOT NULL,
      `InheritDomain` tinyint(1) NOT NULL,
      `InheritIcon` tinyint(1) NOT NULL,
      `InheritPanel` tinyint(1) NOT NULL,
      `InheritPassword` tinyint(1) NOT NULL,
      `InheritPort` tinyint(1) NOT NULL,
      `InheritProtocol` tinyint(1) NOT NULL,
      `InheritPuttySession` tinyint(1) NOT NULL,
      `InheritRedirectDiskDrives` tinyint(1) NOT NULL,
      `InheritRedirectKeys` tinyint(1) NOT NULL,
      `InheritRedirectPorts` tinyint(1) NOT NULL,
      `InheritRedirectPrinters` tinyint(1) NOT NULL,
      `InheritRedirectSmartCards` tinyint(1) NOT NULL,
      `InheritRedirectSound` tinyint(1) NOT NULL,
      `InheritSoundQuality` tinyint(1) NOT NULL,
      `InheritResolution` tinyint(1) NOT NULL,
      `InheritUseConsoleSession` tinyint(1) NOT NULL,
      `InheritUseCredSsp` tinyint(1) NOT NULL,
      `InheritRenderingEngine` tinyint(1) NOT NULL,
      `InheritICAEncryptionStrength` tinyint(1) NOT NULL,
      `InheritRDPAuthenticationLevel` tinyint(1) NOT NULL,
      `InheritRDPMinutesToIdleTimeout` tinyint(1) NOT NULL,
      `InheritRDPAlertIdleTimeout` tinyint(1) NOT NULL,
      `InheritUsername` tinyint(1) NOT NULL,
      `InheritPreExtApp` tinyint(1) NOT NULL,
      `InheritPostExtApp` tinyint(1) NOT NULL,
      `InheritMacAddress` tinyint(1) NOT NULL,
      `InheritUserField` tinyint(1) NOT NULL,
      `InheritExtApp` tinyint(1) NOT NULL,
      `InheritVNCCompression` tinyint(1) NOT NULL,
      `InheritVNCEncoding` tinyint(1) NOT NULL,
      `InheritVNCAuthMode` tinyint(1) NOT NULL,
      `InheritVNCProxyType` tinyint(1) NOT NULL,
      `InheritVNCProxyIP` tinyint(1) NOT NULL,
      `InheritVNCProxyPort` tinyint(1) NOT NULL,
      `InheritVNCProxyUsername` tinyint(1) NOT NULL,
      `InheritVNCProxyPassword` tinyint(1) NOT NULL,
      `InheritVNCColors` tinyint(1) NOT NULL,
      `InheritVNCSmartSizeMode` tinyint(1) NOT NULL,
      `InheritVNCViewOnly` tinyint(1) NOT NULL,
      `InheritRDGatewayUsageMethod` tinyint(1) NOT NULL,
      `InheritRDGatewayHostname` tinyint(1) NOT NULL,
      `InheritRDGatewayUseConnectionCredentials` tinyint(1) NOT NULL,
      `InheritRDGatewayUsername` tinyint(1) NOT NULL,
      `InheritRDGatewayPassword` tinyint(1) NOT NULL,
      `InheritRDGatewayDomain` tinyint(1) NOT NULL,
      `LoadBalanceInfo` varchar(1024) DEFAULT NULL,
      `AutomaticResize` tinyint(1) NOT NULL DEFAULT 1,
      `InheritLoadBalanceInfo` tinyint(1) NOT NULL DEFAULT 0,
      `InheritAutomaticResize` tinyint(1) NOT NULL DEFAULT 0,
      `RedirectClipboard` tinyint(1) NOT NULL DEFAULT 0,
      `InheritRedirectClipboard` tinyint(1) NOT NULL DEFAULT 0,
      PRIMARY KEY (`ID`)
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
      `ConfVersion` double NOT NULL
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
    