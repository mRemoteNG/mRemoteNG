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
    `RedirectAudioCapture` tinyint(1) NOT NULL,
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
    `InheritRedirectAudioCapture` tinyint(1) NOT NULL,
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
    `RedirectAudioCapture` bit NOT NULL DEFAULT 0,
    `InheritRedirectAudioCapture` bit NOT NULL DEFAULT 0,
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