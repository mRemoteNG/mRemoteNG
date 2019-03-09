using System;
using System.Linq;
using System.Text;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers.Csv
{
    public class CsvConnectionsSerializerMremotengFormat : ISerializer<ConnectionInfo, string>
    {
        private readonly SaveFilter _saveFilter;
        private readonly ICredentialRepositoryList _credentialRepositoryList;

        public Version Version { get; } = new Version(2, 7);

        public CsvConnectionsSerializerMremotengFormat(SaveFilter saveFilter,
                                                       ICredentialRepositoryList credentialRepositoryList)
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
            var sb = new StringBuilder();

            WriteCsvHeader(sb);
            SerializeNodesRecursive(serializationTarget, sb);
            return sb.ToString();
        }

        private void WriteCsvHeader(StringBuilder sb)
        {
            sb
                .Append("Name;")
                .Append("Id;")
                .Append("Parent;")
                .Append("NodeType;")
                .Append("Description;")
                .Append("Icon;")
                .Append("Panel;")
                .Append("CredentialId;")
                .Append("Hostname;")
                .Append("Protocol;")
                .Append("PuttySession;")
                .Append("Port;")
                .Append("ConnectToConsole;")
                .Append("UseCredSsp;")
                .Append("RenderingEngine;")
                .Append("ICAEncryptionStrength;")
                .Append("RDPAuthenticationLevel;")
                .Append("LoadBalanceInfo;")
                .Append("Colors;")
                .Append("Resolution;")
                .Append("AutomaticResize;")
                .Append("DisplayWallpaper;")
                .Append("DisplayThemes;")
                .Append("EnableFontSmoothing;")
                .Append("EnableDesktopComposition;")
                .Append("CacheBitmaps;")
                .Append("RedirectDiskDrives;")
                .Append("RedirectPorts;")
                .Append("RedirectPrinters;")
                .Append("RedirectClipboard;")
                .Append("RedirectSmartCards;")
                .Append("RedirectSound;")
                .Append("RedirectKeys;")
                .Append("PreExtApp;")
                .Append("PostExtApp;")
                .Append("MacAddress;")
                .Append("UserField;")
                .Append("ExtApp;")
                .Append("Favorite;")
                .Append("VNCCompression;")
                .Append("VNCEncoding;")
                .Append("VNCAuthMode;")
                .Append("VNCProxyType;")
                .Append("VNCProxyIP;")
                .Append("VNCProxyPort;")
                .Append("VNCProxyUsername;")
                .Append("VNCProxyPassword;")
                .Append("VNCColors;")
                .Append("VNCSmartSizeMode;")
                .Append("VNCViewOnly;")
                .Append("RDGatewayUsageMethod;")
                .Append("RDGatewayHostname;")
                .Append("RDGatewayUseConnectionCredentials;")
                .Append("RDGatewayUsername;")
                .Append("RDGatewayPassword;")
                .Append("RDGatewayDomain;");

            if (!_saveFilter.SaveInheritance)
                return;

            sb
                .Append("InheritCacheBitmaps;")
                .Append("InheritColors;")
                .Append("InheritDescription;")
                .Append("InheritDisplayThemes;")
                .Append("InheritDisplayWallpaper;")
                .Append("InheritEnableFontSmoothing;")
                .Append("InheritEnableDesktopComposition;")
                .Append("InheritDomain;")
                .Append("InheritIcon;")
                .Append("InheritPanel;")
                .Append("InheritPassword;")
                .Append("InheritPort;")
                .Append("InheritProtocol;")
                .Append("InheritPuttySession;")
                .Append("InheritRedirectDiskDrives;")
                .Append("InheritRedirectKeys;")
                .Append("InheritRedirectPorts;")
                .Append("InheritRedirectPrinters;")
                .Append("InheritRedirectClipboard;")
                .Append("InheritRedirectSmartCards;")
                .Append("InheritRedirectSound;")
                .Append("InheritResolution;")
                .Append("InheritAutomaticResize;")
                .Append("InheritUseConsoleSession;")
                .Append("InheritUseCredSsp;")
                .Append("InheritRenderingEngine;")
                .Append("InheritUsername;")
                .Append("InheritICAEncryptionStrength;")
                .Append("InheritRDPAuthenticationLevel;")
                .Append("InheritLoadBalanceInfo;")
                .Append("InheritPreExtApp;")
                .Append("InheritPostExtApp;")
                .Append("InheritMacAddress;")
                .Append("InheritUserField;")
                .Append("InheritFavorite;")
                .Append("InheritExtApp;")
                .Append("InheritVNCCompression;")
                .Append("InheritVNCEncoding;")
                .Append("InheritVNCAuthMode;")
                .Append("InheritVNCProxyType;")
                .Append("InheritVNCProxyIP;")
                .Append("InheritVNCProxyPort;")
                .Append("InheritVNCProxyUsername;")
                .Append("InheritVNCProxyPassword;")
                .Append("InheritVNCColors;")
                .Append("InheritVNCSmartSizeMode;")
                .Append("InheritVNCViewOnly;")
                .Append("InheritRDGatewayUsageMethod;")
                .Append("InheritRDGatewayHostname;")
                .Append("InheritRDGatewayUseConnectionCredentials;")
                .Append("InheritRDGatewayUsername;")
                .Append("InheritRDGatewayPassword;")
                .Append("InheritRDGatewayDomain;")
                .Append("InheritRDPAlertIdleTimeout;")
                .Append("InheritRDPMinutesToIdleTimeout;")
                .Append("InheritSoundQuality;")
                .Append("InheritCredentialRecord");
        }

        private void SerializeNodesRecursive(ConnectionInfo node, StringBuilder sb)
        {
            var nodeAsContainer = node as ContainerInfo;
            if (nodeAsContainer != null)
            {
                foreach (var child in nodeAsContainer.Children)
                {
                    SerializeNodesRecursive(child, sb);
                }
            }

            // dont serialize the root node
            if (node is RootNodeInfo)
                return;

            SerializeConnectionInfo(node, sb);
        }

        private void SerializeConnectionInfo(ConnectionInfo con, StringBuilder sb)
        {
            sb.AppendLine();
            sb
                .Append(FormatForCsv(con.Name))
                .Append(FormatForCsv(con.ConstantID))
                .Append(FormatForCsv(con.Parent?.ConstantID ?? ""))
                .Append(FormatForCsv(con.GetTreeNodeType()))
                .Append(FormatForCsv(con.Description))
                .Append(FormatForCsv(con.Icon))
                .Append(FormatForCsv(con.Panel))
                .Append(FormatForCsv(con.CredentialRecordId))
                .Append(FormatForCsv(con.Hostname))
                .Append(FormatForCsv(con.Protocol))
                .Append(FormatForCsv(con.PuttySession))
                .Append(FormatForCsv(con.Port))
                .Append(FormatForCsv(con.UseConsoleSession))
                .Append(FormatForCsv(con.UseCredSsp))
                .Append(FormatForCsv(con.RenderingEngine))
                .Append(FormatForCsv(con.ICAEncryptionStrength))
                .Append(FormatForCsv(con.RDPAuthenticationLevel))
                .Append(FormatForCsv(con.LoadBalanceInfo))
                .Append(FormatForCsv(con.Colors))
                .Append(FormatForCsv(con.Resolution))
                .Append(FormatForCsv(con.AutomaticResize))
                .Append(FormatForCsv(con.DisplayWallpaper))
                .Append(FormatForCsv(con.DisplayThemes))
                .Append(FormatForCsv(con.EnableFontSmoothing))
                .Append(FormatForCsv(con.EnableDesktopComposition))
                .Append(FormatForCsv(con.CacheBitmaps))
                .Append(FormatForCsv(con.RedirectDiskDrives))
                .Append(FormatForCsv(con.RedirectPorts))
                .Append(FormatForCsv(con.RedirectPrinters))
                .Append(FormatForCsv(con.RedirectClipboard))
                .Append(FormatForCsv(con.RedirectSmartCards))
                .Append(FormatForCsv(con.RedirectSound))
                .Append(FormatForCsv(con.RedirectKeys))
                .Append(FormatForCsv(con.PreExtApp))
                .Append(FormatForCsv(con.PostExtApp))
                .Append(FormatForCsv(con.MacAddress))
                .Append(FormatForCsv(con.UserField))
                .Append(FormatForCsv(con.ExtApp))
                .Append(FormatForCsv(con.Favorite))
                .Append(FormatForCsv(con.VNCCompression))
                .Append(FormatForCsv(con.VNCEncoding))
                .Append(FormatForCsv(con.VNCAuthMode))
                .Append(FormatForCsv(con.VNCProxyType))
                .Append(FormatForCsv(con.VNCProxyIP))
                .Append(FormatForCsv(con.VNCProxyPort))
                .Append(FormatForCsv(con.VNCProxyUsername))
                .Append(FormatForCsv(con.VNCProxyPassword))
                .Append(FormatForCsv(con.VNCColors))
                .Append(FormatForCsv(con.VNCSmartSizeMode))
                .Append(FormatForCsv(con.VNCViewOnly))
                .Append(FormatForCsv(con.RDGatewayUsageMethod))
                .Append(FormatForCsv(con.RDGatewayHostname))
                .Append(FormatForCsv(con.RDGatewayUseConnectionCredentials))
                .Append(FormatForCsv(con.RDGatewayUsername))
                .Append(FormatForCsv(con.RDGatewayPassword))
                .Append(FormatForCsv(con.RDGatewayDomain));


            if (!_saveFilter.SaveInheritance)
                return;

            sb
                .Append(FormatForCsv(con.Inheritance.CacheBitmaps))
                .Append(FormatForCsv(con.Inheritance.Colors))
                .Append(FormatForCsv(con.Inheritance.Description))
                .Append(FormatForCsv(con.Inheritance.DisplayThemes))
                .Append(FormatForCsv(con.Inheritance.DisplayWallpaper))
                .Append(FormatForCsv(con.Inheritance.EnableFontSmoothing))
                .Append(FormatForCsv(con.Inheritance.EnableDesktopComposition))
                .Append(FormatForCsv(con.Inheritance.Domain))
                .Append(FormatForCsv(con.Inheritance.Icon))
                .Append(FormatForCsv(con.Inheritance.Panel))
                .Append(FormatForCsv(con.Inheritance.Password))
                .Append(FormatForCsv(con.Inheritance.Port))
                .Append(FormatForCsv(con.Inheritance.Protocol))
                .Append(FormatForCsv(con.Inheritance.PuttySession))
                .Append(FormatForCsv(con.Inheritance.RedirectDiskDrives))
                .Append(FormatForCsv(con.Inheritance.RedirectKeys))
                .Append(FormatForCsv(con.Inheritance.RedirectPorts))
                .Append(FormatForCsv(con.Inheritance.RedirectPrinters))
                .Append(FormatForCsv(con.Inheritance.RedirectClipboard))
                .Append(FormatForCsv(con.Inheritance.RedirectSmartCards))
                .Append(FormatForCsv(con.Inheritance.RedirectSound))
                .Append(FormatForCsv(con.Inheritance.Resolution))
                .Append(FormatForCsv(con.Inheritance.AutomaticResize))
                .Append(FormatForCsv(con.Inheritance.UseConsoleSession))
                .Append(FormatForCsv(con.Inheritance.UseCredSsp))
                .Append(FormatForCsv(con.Inheritance.RenderingEngine))
                .Append(FormatForCsv(con.Inheritance.Username))
                .Append(FormatForCsv(con.Inheritance.ICAEncryptionStrength))
                .Append(FormatForCsv(con.Inheritance.RDPAuthenticationLevel))
                .Append(FormatForCsv(con.Inheritance.LoadBalanceInfo))
                .Append(FormatForCsv(con.Inheritance.PreExtApp))
                .Append(FormatForCsv(con.Inheritance.PostExtApp))
                .Append(FormatForCsv(con.Inheritance.MacAddress))
                .Append(FormatForCsv(con.Inheritance.UserField))
                .Append(FormatForCsv(con.Inheritance.Favorite))
                .Append(FormatForCsv(con.Inheritance.ExtApp))
                .Append(FormatForCsv(con.Inheritance.VNCCompression))
                .Append(FormatForCsv(con.Inheritance.VNCEncoding))
                .Append(FormatForCsv(con.Inheritance.VNCAuthMode))
                .Append(FormatForCsv(con.Inheritance.VNCProxyType))
                .Append(FormatForCsv(con.Inheritance.VNCProxyIP))
                .Append(FormatForCsv(con.Inheritance.VNCProxyPort))
                .Append(FormatForCsv(con.Inheritance.VNCProxyUsername))
                .Append(FormatForCsv(con.Inheritance.VNCProxyPassword))
                .Append(FormatForCsv(con.Inheritance.VNCColors))
                .Append(FormatForCsv(con.Inheritance.VNCSmartSizeMode))
                .Append(FormatForCsv(con.Inheritance.VNCViewOnly))
                .Append(FormatForCsv(con.Inheritance.RDGatewayUsageMethod))
                .Append(FormatForCsv(con.Inheritance.RDGatewayHostname))
                .Append(FormatForCsv(con.Inheritance.RDGatewayUseConnectionCredentials))
                .Append(FormatForCsv(con.Inheritance.RDGatewayUsername))
                .Append(FormatForCsv(con.Inheritance.RDGatewayPassword))
                .Append(FormatForCsv(con.Inheritance.RDGatewayDomain))
                .Append(FormatForCsv(con.Inheritance.RDPAlertIdleTimeout))
                .Append(FormatForCsv(con.Inheritance.RDPMinutesToIdleTimeout))
                .Append(FormatForCsv(con.Inheritance.SoundQuality))
                .Append(FormatForCsv(con.Inheritance.CredentialId));
        }

        private string FormatForCsv(object value)
        {
            var cleanedString = value.ToString().Replace(";", "");
            return cleanedString + ";";
        }
    }
}