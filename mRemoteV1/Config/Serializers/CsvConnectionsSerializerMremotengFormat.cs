using System;
using System.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers
{
    public class CsvConnectionsSerializerMremotengFormat : ISerializer<string>
    {
        private string _csv = "";
        private ConnectionInfo _serializationTarget;
        private readonly SaveFilter _saveFilter;


        public CsvConnectionsSerializerMremotengFormat(SaveFilter saveFilter)
        {
            if (saveFilter == null)
                throw new ArgumentNullException(nameof(saveFilter));
            _saveFilter = saveFilter;
        }

        public string Serialize(ConnectionTreeModel connectionTreeModel)
        {
            if (connectionTreeModel == null)
                throw new ArgumentNullException(nameof(connectionTreeModel));

            var rootNode = connectionTreeModel.RootNodes.First(node => node is RootNodeInfo);
            return Serialize(rootNode);
        }

        public string Serialize(ConnectionInfo serializationTarget)
        {
            if (serializationTarget == null)
                throw new ArgumentNullException(nameof(serializationTarget));

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
                csvHeader += "InheritCacheBitmaps;InheritColors;InheritDescription;InheritDisplayThemes;InheritDisplayWallpaper;InheritEnableFontSmoothing;InheritEnableDesktopComposition;InheritDomain;InheritIcon;InheritPanel;InheritPassword;InheritPort;InheritProtocol;InheritPuttySession;InheritRedirectDiskDrives;InheritRedirectKeys;InheritRedirectPorts;InheritRedirectPrinters;InheritRedirectSmartCards;InheritRedirectSound;InheritResolution;InheritAutomaticResize;InheritUseConsoleSession;InheritUseCredSsp;InheritRenderingEngine;InheritUsername;InheritICAEncryptionStrength;InheritRDPAuthenticationLevel;InheritLoadBalanceInfo;InheritPreExtApp;InheritPostExtApp;InheritMacAddress;InheritUserField;InheritExtApp;InheritVNCCompression;InheritVNCEncoding;InheritVNCAuthMode;InheritVNCProxyType;InheritVNCProxyIP;InheritVNCProxyPort;InheritVNCProxyUsername;InheritVNCProxyPassword;InheritVNCColors;InheritVNCSmartSizeMode;InheritVNCViewOnly;InheritRDGatewayUsageMethod;InheritRDGatewayHostname;InheritRDGatewayUseConnectionCredentials;InheritRDGatewayUsername;InheritRDGatewayPassword;InheritRDGatewayDomain";
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

            csvLine += con.Name + ";" + GetNodePath(con) + ";" + con.Description + ";" + con.Icon + ";" + con.Panel + ";";

            if (_saveFilter.SaveUsername)
                csvLine += con.CredentialRecord?.Username + ";";

            if (_saveFilter.SavePassword)
                csvLine += con.CredentialRecord?.Password.ConvertToUnsecureString() + ";";

            if (_saveFilter.SaveDomain)
                csvLine += con.CredentialRecord?.Domain + ";";

            csvLine += con.Hostname + ";" + 
                        con.Protocol + ";" + 
                        con.PuttySession + ";" + 
                        Convert.ToString(con.Port) + ";" + 
                        Convert.ToString(con.UseConsoleSession) + ";" + 
                        Convert.ToString(con.UseCredSsp) + ";" + 
                        con.RenderingEngine + ";" + 
                        con.ICAEncryptionStrength + ";" + 
                        con.RDPAuthenticationLevel + ";" + 
                        con.LoadBalanceInfo + ";" + 
                        con.Colors + ";" + 
                        con.Resolution + ";" + 
                        Convert.ToString(con.AutomaticResize) + ";" + 
                        Convert.ToString(con.DisplayWallpaper) + ";" + 
                        Convert.ToString(con.DisplayThemes) + ";" + 
                        Convert.ToString(con.EnableFontSmoothing) + ";" + 
                        Convert.ToString(con.EnableDesktopComposition) + ";" + 
                        Convert.ToString(con.CacheBitmaps) + ";" + 
                        Convert.ToString(con.RedirectDiskDrives) + ";" + 
                        Convert.ToString(con.RedirectPorts) + ";" + 
                        Convert.ToString(con.RedirectPrinters) + ";" + 
                        Convert.ToString(con.RedirectSmartCards) + ";" + 
                        con.RedirectSound + ";" + 
                        Convert.ToString(con.RedirectKeys) + ";" + 
                        con.PreExtApp + ";" + 
                        con.PostExtApp + ";" + 
                        con.MacAddress + ";" + 
                        con.UserField + ";" + 
                        con.ExtApp + ";" + 
                        con.VNCCompression + ";" + 
                        con.VNCEncoding + ";" + 
                        con.VNCAuthMode + ";" + 
                        con.VNCProxyType + ";" + 
                        con.VNCProxyIP + ";" + 
                        Convert.ToString(con.VNCProxyPort) + ";" + 
                        con.VNCProxyUsername + ";" + 
                        con.VNCProxyPassword + ";" + 
                        con.VNCColors + ";" + 
                        con.VNCSmartSizeMode + ";" + 
                        Convert.ToString(con.VNCViewOnly) + ";" +
                        con.RDGatewayUsageMethod + ";" +
                        con.RDGatewayHostname + ";" +
                        con.RDGatewayUseConnectionCredentials + ";" +
                        con.RDGatewayUsername + ";" +
                        con.RDGatewayPassword + ";" +
                        con.RDGatewayDomain + ";";


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
                    con.Inheritance.VNCViewOnly +
                    con.Inheritance.RDGatewayUsageMethod + ";" +
                    con.Inheritance.RDGatewayHostname + ";" +
                    con.Inheritance.RDGatewayUseConnectionCredentials + ";" +
                    con.Inheritance.RDGatewayUsername + ";" +
                    con.Inheritance.RDGatewayPassword + ";" +
                    con.Inheritance.RDGatewayDomain + ";";
            }

            _csv += csvLine;
        }

        private string GetNodePath(ConnectionInfo connectionInfo)
        {
            var nodePath = "";
            var container = connectionInfo.Parent;
            if (container == null) return nodePath;
            while (container != _serializationTarget)
            {
                container = container.Parent;
                nodePath += $@"{container.Name}\";
            }
            nodePath = nodePath.TrimEnd('\\');
            return nodePath;
        }
    }
}