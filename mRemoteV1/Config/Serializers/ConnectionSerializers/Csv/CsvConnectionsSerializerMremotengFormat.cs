using System;
using System.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers.Csv
{
	public class CsvConnectionsSerializerMremotengFormat : ISerializer<ConnectionInfo,string>
    {
        private string _csv = "";
        private ConnectionInfo _serializationTarget;
        private readonly SaveFilter _saveFilter;
        private readonly ICredentialRepositoryList _credentialRepositoryList;


        public CsvConnectionsSerializerMremotengFormat(SaveFilter saveFilter, ICredentialRepositoryList credentialRepositoryList)
        {
            saveFilter.ThrowIfNull(nameof(saveFilter));
            credentialRepositoryList.ThrowIfNull(nameof(credentialRepositoryList));

            _saveFilter = saveFilter;
            _credentialRepositoryList = credentialRepositoryList;
        }

        public string Serialize(ConnectionTreeModel connectionTreeModel)
        {
            connectionTreeModel.ThrowIfNull(nameof(connectionTreeModel));

            var rootNode = connectionTreeModel.RootNodes.First(node => node is RootNodeInfo);
            return Serialize(rootNode);
        }

        public string Serialize(ConnectionInfo serializationTarget)
        {
            serializationTarget.ThrowIfNull(nameof(serializationTarget));

            _csv = "";
            _serializationTarget = serializationTarget;
            WriteCsvHeader();
            SerializeNodesRecursive(serializationTarget);
            return _csv;
        }

        private void WriteCsvHeader()
        {
            var csvHeader = string.Empty;
            csvHeader += "Name;Folder;Description;Icon;Panel;";
            if (_saveFilter.SaveUsername)
                csvHeader += "Username;";
            if (_saveFilter.SavePassword)
                csvHeader += "Password;";
            if (_saveFilter.SaveDomain)
                csvHeader += "Domain;";
            csvHeader += "Hostname;Protocol;PuttySession;Port;ConnectToConsole;UseCredSsp;RenderingEngine;ICAEncryptionStrength;RDPAuthenticationLevel;LoadBalanceInfo;Colors;Resolution;AutomaticResize;DisplayWallpaper;DisplayThemes;EnableFontSmoothing;EnableDesktopComposition;CacheBitmaps;RedirectDiskDrives;RedirectPorts;RedirectPrinters;RedirectSmartCards;RedirectSound;RedirectKeys;PreExtApp;PostExtApp;MacAddress;UserField;ExtApp;VNCCompression;VNCEncoding;VNCAuthMode;VNCProxyType;VNCProxyIP;VNCProxyPort;VNCProxyUsername;VNCProxyPassword;VNCColors;VNCSmartSizeMode;VNCViewOnly;RDGatewayUsageMethod;RDGatewayHostname;RDGatewayUseConnectionCredentials;RDGatewayUsername;RDGatewayPassword;RDGatewayDomain;";
            if (_saveFilter.SaveInheritance)
                csvHeader += "InheritCacheBitmaps;InheritColors;InheritDescription;InheritDisplayThemes;InheritDisplayWallpaper;InheritEnableFontSmoothing;InheritEnableDesktopComposition;InheritDomain;InheritIcon;InheritPanel;InheritPassword;InheritPort;InheritProtocol;InheritPuttySession;InheritRedirectDiskDrives;InheritRedirectKeys;InheritRedirectPorts;InheritRedirectPrinters;InheritRedirectSmartCards;InheritRedirectSound;InheritResolution;InheritAutomaticResize;InheritUseConsoleSession;InheritUseCredSsp;InheritRenderingEngine;InheritUsername;InheritICAEncryptionStrength;InheritRDPAuthenticationLevel;InheritLoadBalanceInfo;InheritPreExtApp;InheritPostExtApp;InheritMacAddress;InheritUserField;InheritExtApp;InheritVNCCompression;InheritVNCEncoding;InheritVNCAuthMode;InheritVNCProxyType;InheritVNCProxyIP;InheritVNCProxyPort;InheritVNCProxyUsername;InheritVNCProxyPassword;InheritVNCColors;InheritVNCSmartSizeMode;InheritVNCViewOnly;InheritRDGatewayUsageMethod;InheritRDGatewayHostname;InheritRDGatewayUseConnectionCredentials;InheritRDGatewayUsername;InheritRDGatewayPassword;InheritRDGatewayDomain;InheritRDPAlertIdleTimeout;InheritRDPMinutesToIdleTimeout;InheritSoundQuality";
            _csv += csvHeader;
        }

        private void SerializeNodesRecursive(ConnectionInfo node)
        {
            var nodeAsContainer = node as ContainerInfo;
            if (nodeAsContainer != null)
            {
                foreach (var child in nodeAsContainer.Children)
                {
                    var info = child as ContainerInfo;
                    if (info != null)
                        SerializeNodesRecursive(info);
                    else
                        SerializeConnectionInfo(child);
                }
            }
            else
                SerializeConnectionInfo(node);
        }

        private void SerializeConnectionInfo(ConnectionInfo con)
        {
            var csvLine = Environment.NewLine;

            csvLine += CleanStringForCsv(con.Name) + ";" +
                        CleanStringForCsv(GetNodePath(con)) + ";" +
                        CleanStringForCsv(con.Description) + ";" +
                        CleanStringForCsv(con.Icon) + ";" +
                        CleanStringForCsv(con.Panel) + ";";

            if (_saveFilter.SaveUsername)
                csvLine += CleanStringForCsv(con.Username) + ";";

            if (_saveFilter.SavePassword)
                csvLine += CleanStringForCsv(con.Password) + ";";

            if (_saveFilter.SaveDomain)
                csvLine += CleanStringForCsv(con.Domain) + ";";

            csvLine += CleanStringForCsv(con.Hostname) + ";" +
                       CleanStringForCsv(con.Protocol) + ";" +
                       CleanStringForCsv(con.PuttySession) + ";" +
                       CleanStringForCsv(con.Port) + ";" +
                       CleanStringForCsv(con.UseConsoleSession) + ";" +
                       CleanStringForCsv(con.UseCredSsp) + ";" +
                       CleanStringForCsv(con.RenderingEngine) + ";" +
                       CleanStringForCsv(con.ICAEncryptionStrength) + ";" +
                       CleanStringForCsv(con.RDPAuthenticationLevel) + ";" +
                       CleanStringForCsv(con.LoadBalanceInfo) + ";" +
                       CleanStringForCsv(con.Colors) + ";" +
                       CleanStringForCsv(con.Resolution) + ";" +
                       CleanStringForCsv(con.AutomaticResize) + ";" +
                       CleanStringForCsv(con.DisplayWallpaper) + ";" +
                       CleanStringForCsv(con.DisplayThemes) + ";" +
                       CleanStringForCsv(con.EnableFontSmoothing) + ";" +
                       CleanStringForCsv(con.EnableDesktopComposition) + ";" +
                       CleanStringForCsv(con.CacheBitmaps) + ";" +
                       CleanStringForCsv(con.RedirectDiskDrives) + ";" +
                       CleanStringForCsv(con.RedirectPorts) + ";" +
                       CleanStringForCsv(con.RedirectPrinters) + ";" +
                       CleanStringForCsv(con.RedirectSmartCards) + ";" +
                       CleanStringForCsv(con.RedirectSound) + ";" +
                       CleanStringForCsv(con.RedirectKeys) + ";" +
                       CleanStringForCsv(con.PreExtApp) + ";" +
                       CleanStringForCsv(con.PostExtApp) + ";" +
                       CleanStringForCsv(con.MacAddress) + ";" +
                       CleanStringForCsv(con.UserField) + ";" +
                       CleanStringForCsv(con.ExtApp) + ";" +
                       CleanStringForCsv(con.VNCCompression) + ";" +
                       CleanStringForCsv(con.VNCEncoding) + ";" +
                       CleanStringForCsv(con.VNCAuthMode) + ";" +
                       CleanStringForCsv(con.VNCProxyType) + ";" +
                       CleanStringForCsv(con.VNCProxyIP) + ";" +
                       CleanStringForCsv(con.VNCProxyPort) + ";" +
                       CleanStringForCsv(con.VNCProxyUsername) + ";" +
                       CleanStringForCsv(con.VNCProxyPassword) + ";" +
                       CleanStringForCsv(con.VNCColors) + ";" +
                       CleanStringForCsv(con.VNCSmartSizeMode) + ";" +
                       CleanStringForCsv(con.VNCViewOnly) + ";" +
                       CleanStringForCsv(con.RDGatewayUsageMethod) + ";" +
                       CleanStringForCsv(con.RDGatewayHostname) + ";" +
                       CleanStringForCsv(con.RDGatewayUseConnectionCredentials) + ";" +
                       CleanStringForCsv(con.RDGatewayUsername) + ";" +
                       CleanStringForCsv(con.RDGatewayPassword) + ";" +
                       CleanStringForCsv(con.RDGatewayDomain) + ";";


            if (_saveFilter.SaveInheritance)
            {
                csvLine += con.Inheritance.CacheBitmaps + ";" + 
                    con.Inheritance.Colors + ";" +
                    con.Inheritance.Description + ";" +
                    con.Inheritance.DisplayThemes + ";" +
                    con.Inheritance.DisplayWallpaper + ";" +
                    con.Inheritance.EnableFontSmoothing + ";" +
                    con.Inheritance.EnableDesktopComposition + ";" +
                    con.Inheritance.Domain + ";" +
                    con.Inheritance.Icon + ";" +
                    con.Inheritance.Panel + ";" +
                    con.Inheritance.Password + ";" +
                    con.Inheritance.Port + ";" +
                    con.Inheritance.Protocol + ";" +
                    con.Inheritance.PuttySession + ";" +
                    con.Inheritance.RedirectDiskDrives + ";" +
                    con.Inheritance.RedirectKeys + ";" +
                    con.Inheritance.RedirectPorts + ";" +
                    con.Inheritance.RedirectPrinters + ";" +
                    con.Inheritance.RedirectSmartCards + ";" +
                    con.Inheritance.RedirectSound + ";" +
                    con.Inheritance.Resolution + ";" +
                    con.Inheritance.AutomaticResize + ";" +
                    con.Inheritance.UseConsoleSession + ";" +
                    con.Inheritance.UseCredSsp + ";" +
                    con.Inheritance.RenderingEngine + ";" +
                    con.Inheritance.Username + ";" +
                    con.Inheritance.ICAEncryptionStrength + ";" +
                    con.Inheritance.RDPAuthenticationLevel + ";" +
                    con.Inheritance.LoadBalanceInfo + ";" +
                    con.Inheritance.PreExtApp + ";" +
                    con.Inheritance.PostExtApp + ";" +
                    con.Inheritance.MacAddress + ";" +
                    con.Inheritance.UserField + ";" +
                    con.Inheritance.ExtApp + ";" +
                    con.Inheritance.VNCCompression + ";" +
                    con.Inheritance.VNCEncoding + ";" +
                    con.Inheritance.VNCAuthMode + ";" +
                    con.Inheritance.VNCProxyType + ";" +
                    con.Inheritance.VNCProxyIP + ";" +
                    con.Inheritance.VNCProxyPort + ";" +
                    con.Inheritance.VNCProxyUsername + ";" +
                    con.Inheritance.VNCProxyPassword + ";" +
                    con.Inheritance.VNCColors + ";" +
                    con.Inheritance.VNCSmartSizeMode + ";" +
                    con.Inheritance.VNCViewOnly + ";" +
                    con.Inheritance.RDGatewayUsageMethod + ";" +
                    con.Inheritance.RDGatewayHostname + ";" +
                    con.Inheritance.RDGatewayUseConnectionCredentials + ";" +
                    con.Inheritance.RDGatewayUsername + ";" +
                    con.Inheritance.RDGatewayPassword + ";" +
                    con.Inheritance.RDGatewayDomain + ";" +
                    con.Inheritance.RDPAlertIdleTimeout + ";" +
                    con.Inheritance.RDPMinutesToIdleTimeout + ";" +
                    con.Inheritance.SoundQuality;
            }

            _csv += csvLine;
        }

        /// <summary>
        /// Remove text that is unsafe for use in CSV files
        /// </summary>
        /// <param name="text"></param>
        private string CleanStringForCsv(object text)
        {
            return text.ToString().Replace(";", "");
        }

        private string GetNodePath(ConnectionInfo connectionInfo)
        {
            var nodePath = "";
            var currentItem = connectionInfo;
            while (currentItem != _serializationTarget)
            {
                currentItem = currentItem.Parent;
                if (currentItem == null)
                    break;
                nodePath += $@"{currentItem.Name}\";
            }
            nodePath = nodePath.TrimEnd('\\');
            return nodePath;
        }
    }
}