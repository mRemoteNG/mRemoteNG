using System;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;


namespace mRemoteNG.Config.Connections
{
    public class CsvConnectionsSerializer : ISerializer
    {
        private string _csv = "";

        public Save SaveSecurity { get; set; }

        public string Serialize(ConnectionTreeModel connectionTreeModel)
        {
            var rootNode = (RootNodeInfo)connectionTreeModel.RootNodes.First(node => node is RootNodeInfo);
            return SerializeToCsv(rootNode);
        }

        private string SerializeToCsv(RootNodeInfo rootNodeInfo)
        {
            if (Runtime.IsConnectionsFileLoaded == false)
                return "";

            WriteCsvHeader();
            SerializeNodesRecursive(rootNodeInfo);
            return _csv;
        }

        private void WriteCsvHeader()
        {
            var csvHeader = string.Empty;
            csvHeader += "Name;Folder;Description;Icon;Panel;";
            if (SaveSecurity.Username)
                csvHeader += "Username;";
            if (SaveSecurity.Password)
                csvHeader += "Password;";
            if (SaveSecurity.Domain)
                csvHeader += "Domain;";
            csvHeader += "Hostname;Protocol;PuttySession;Port;ConnectToConsole;UseCredSsp;RenderingEngine;ICAEncryptionStrength;RDPAuthenticationLevel;LoadBalanceInfo;Colors;Resolution;AutomaticResize;DisplayWallpaper;DisplayThemes;EnableFontSmoothing;EnableDesktopComposition;CacheBitmaps;RedirectDiskDrives;RedirectPorts;RedirectPrinters;RedirectSmartCards;RedirectSound;RedirectKeys;PreExtApp;PostExtApp;MacAddress;UserField;ExtApp;VNCCompression;VNCEncoding;VNCAuthMode;VNCProxyType;VNCProxyIP;VNCProxyPort;VNCProxyUsername;VNCProxyPassword;VNCColors;VNCSmartSizeMode;VNCViewOnly;RDGatewayUsageMethod;RDGatewayHostname;RDGatewayUseConnectionCredentials;RDGatewayUsername;RDGatewayPassword;RDGatewayDomain;";
            if (SaveSecurity.Inheritance)
                csvHeader += "InheritCacheBitmaps;InheritColors;InheritDescription;InheritDisplayThemes;InheritDisplayWallpaper;InheritEnableFontSmoothing;InheritEnableDesktopComposition;InheritDomain;InheritIcon;InheritPanel;InheritPassword;InheritPort;InheritProtocol;InheritPuttySession;InheritRedirectDiskDrives;InheritRedirectKeys;InheritRedirectPorts;InheritRedirectPrinters;InheritRedirectSmartCards;InheritRedirectSound;InheritResolution;InheritAutomaticResize;InheritUseConsoleSession;InheritUseCredSsp;InheritRenderingEngine;InheritUsername;InheritICAEncryptionStrength;InheritRDPAuthenticationLevel;InheritLoadBalanceInfo;InheritPreExtApp;InheritPostExtApp;InheritMacAddress;InheritUserField;InheritExtApp;InheritVNCCompression;InheritVNCEncoding;InheritVNCAuthMode;InheritVNCProxyType;InheritVNCProxyIP;InheritVNCProxyPort;InheritVNCProxyUsername;InheritVNCProxyPassword;InheritVNCColors;InheritVNCSmartSizeMode;InheritVNCViewOnly;InheritRDGatewayUsageMethod;InheritRDGatewayHostname;InheritRDGatewayUseConnectionCredentials;InheritRDGatewayUsername;InheritRDGatewayPassword;InheritRDGatewayDomain";
            _csv += csvHeader;
        }

        private void SerializeNodesRecursive(ContainerInfo containerInfo)
        {
            foreach (var child in containerInfo.Children)
            {
                if (child is ContainerInfo)
                    SerializeNodesRecursive((ContainerInfo)child);
                else
                    SerializeConnectionInfo(child);
            }
        }

        private void SerializeConnectionInfo(ConnectionInfo con)
        {
            var nodePath = con.TreeNode.FullPath;

            var firstSlash = nodePath.IndexOf("\\", StringComparison.Ordinal);
            nodePath = nodePath.Remove(0, firstSlash + 1);
            var lastSlash = nodePath.LastIndexOf("\\", StringComparison.Ordinal);

            nodePath = lastSlash > 0 ? nodePath.Remove(lastSlash) : "";

            var csvLine = Environment.NewLine;

            csvLine += con.Name + ";" + nodePath + ";" + con.Description + ";" + con.Icon + ";" + con.Panel + ";";

            if (SaveSecurity.Username)
                csvLine += con.Username + ";";

            if (SaveSecurity.Password)
                csvLine += con.Password + ";";

            if (SaveSecurity.Domain)
                csvLine += con.Domain + ";";

            csvLine += con.Hostname + ";" + con.Protocol + ";" + con.PuttySession + ";" + Convert.ToString(con.Port) + ";" + Convert.ToString(con.UseConsoleSession) + ";" + Convert.ToString(con.UseCredSsp) + ";" + con.RenderingEngine + ";" + con.ICAEncryptionStrength + ";" + con.RDPAuthenticationLevel + ";" + con.LoadBalanceInfo + ";" + con.Colors + ";" + con.Resolution + ";" + Convert.ToString(con.AutomaticResize) + ";" + Convert.ToString(con.DisplayWallpaper) + ";" + Convert.ToString(con.DisplayThemes) + ";" + Convert.ToString(con.EnableFontSmoothing) + ";" + Convert.ToString(con.EnableDesktopComposition) + ";" + Convert.ToString(con.CacheBitmaps) + ";" + Convert.ToString(con.RedirectDiskDrives) + ";" + Convert.ToString(con.RedirectPorts) + ";" + Convert.ToString(con.RedirectPrinters) + ";" + Convert.ToString(con.RedirectSmartCards) + ";" + con.RedirectSound + ";" + Convert.ToString(con.RedirectKeys) + ";" + con.PreExtApp + ";" + con.PostExtApp + ";" + con.MacAddress + ";" + con.UserField + ";" + con.ExtApp + ";" + con.VNCCompression + ";" + con.VNCEncoding + ";" + con.VNCAuthMode + ";" + con.VNCProxyType + ";" + con.VNCProxyIP + ";" + Convert.ToString(con.VNCProxyPort) + ";" + con.VNCProxyUsername + ";" + con.VNCProxyPassword + ";" + con.VNCColors + ";" + con.VNCSmartSizeMode + ";" + Convert.ToString(con.VNCViewOnly) + ";";

            if (SaveSecurity.Inheritance)
            {
                csvLine += con.Inheritance.CacheBitmaps + ";" + Convert.ToString(con.Inheritance.Colors) + ";" + Convert.ToString(con.Inheritance.Description) + ";" + Convert.ToString(con.Inheritance.DisplayThemes) + ";" + Convert.ToString(con.Inheritance.DisplayWallpaper) + ";" + Convert.ToString(con.Inheritance.EnableFontSmoothing) + ";" + Convert.ToString(con.Inheritance.EnableDesktopComposition) + ";" + Convert.ToString(con.Inheritance.Domain) + ";" + Convert.ToString(con.Inheritance.Icon) + ";" + Convert.ToString(con.Inheritance.Panel) + ";" + Convert.ToString(con.Inheritance.Password) + ";" + Convert.ToString(con.Inheritance.Port) + ";" + Convert.ToString(con.Inheritance.Protocol) + ";" + Convert.ToString(con.Inheritance.PuttySession) + ";" + Convert.ToString(con.Inheritance.RedirectDiskDrives) + ";" + Convert.ToString(con.Inheritance.RedirectKeys) + ";" + Convert.ToString(con.Inheritance.RedirectPorts) + ";" + Convert.ToString(con.Inheritance.RedirectPrinters) + ";" + Convert.ToString(con.Inheritance.RedirectSmartCards) + ";" + Convert.ToString(con.Inheritance.RedirectSound) + ";" + Convert.ToString(con.Inheritance.Resolution) + ";" + Convert.ToString(con.Inheritance.AutomaticResize) + ";" + Convert.ToString(con.Inheritance.UseConsoleSession) + ";" + Convert.ToString(con.Inheritance.UseCredSsp) + ";" + Convert.ToString(con.Inheritance.RenderingEngine) + ";" + Convert.ToString(con.Inheritance.Username) + ";" + Convert.ToString(con.Inheritance.ICAEncryptionStrength) + ";" + Convert.ToString(con.Inheritance.RDPAuthenticationLevel) + ";" + Convert.ToString(con.Inheritance.LoadBalanceInfo) + ";" + Convert.ToString(con.Inheritance.PreExtApp) + ";" + Convert.ToString(con.Inheritance.PostExtApp) + ";" + Convert.ToString(con.Inheritance.MacAddress) + ";" + Convert.ToString(con.Inheritance.UserField) + ";" + Convert.ToString(con.Inheritance.ExtApp) + ";" + Convert.ToString(con.Inheritance.VNCCompression) + ";"
                + Convert.ToString(con.Inheritance.VNCEncoding) + ";" + Convert.ToString(con.Inheritance.VNCAuthMode) + ";" + Convert.ToString(con.Inheritance.VNCProxyType) + ";" + Convert.ToString(con.Inheritance.VNCProxyIP) + ";" + Convert.ToString(con.Inheritance.VNCProxyPort) + ";" + Convert.ToString(con.Inheritance.VNCProxyUsername) + ";" + Convert.ToString(con.Inheritance.VNCProxyPassword) + ";" + Convert.ToString(con.Inheritance.VNCColors) + ";" + Convert.ToString(con.Inheritance.VNCSmartSizeMode) + ";" + Convert.ToString(con.Inheritance.VNCViewOnly);
            }

            _csv += csvLine;
        }
    }
}