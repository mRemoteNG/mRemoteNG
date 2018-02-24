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
	public class CsvConnectionsSerializerMremotengFormat : ISerializer<ConnectionInfo,string>
    {
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
            var sb = new StringBuilder();

            WriteCsvHeader(sb);
            SerializeNodesRecursive(serializationTarget, sb);
            return sb.ToString();
        }

        private void WriteCsvHeader(StringBuilder sb)
        {
            sb.Append("Name;Id;Parent;NodeType;Description;Icon;Panel;");
            if (_saveFilter.SaveUsername)
                sb.Append("Username;");
            if (_saveFilter.SavePassword)
                sb.Append("Password;");
            if (_saveFilter.SaveDomain)
                sb.Append("Domain;");
            sb.Append("Hostname;Protocol;PuttySession;Port;ConnectToConsole;UseCredSsp;RenderingEngine;ICAEncryptionStrength;RDPAuthenticationLevel;LoadBalanceInfo;Colors;Resolution;AutomaticResize;DisplayWallpaper;DisplayThemes;EnableFontSmoothing;EnableDesktopComposition;CacheBitmaps;RedirectDiskDrives;RedirectPorts;RedirectPrinters;RedirectSmartCards;RedirectSound;RedirectKeys;PreExtApp;PostExtApp;MacAddress;UserField;ExtApp;VNCCompression;VNCEncoding;VNCAuthMode;VNCProxyType;VNCProxyIP;VNCProxyPort;VNCProxyUsername;VNCProxyPassword;VNCColors;VNCSmartSizeMode;VNCViewOnly;RDGatewayUsageMethod;RDGatewayHostname;RDGatewayUseConnectionCredentials;RDGatewayUsername;RDGatewayPassword;RDGatewayDomain;");
            if (_saveFilter.SaveInheritance)
                sb.Append("InheritCacheBitmaps;InheritColors;InheritDescription;InheritDisplayThemes;InheritDisplayWallpaper;InheritEnableFontSmoothing;InheritEnableDesktopComposition;InheritDomain;InheritIcon;InheritPanel;InheritPassword;InheritPort;InheritProtocol;InheritPuttySession;InheritRedirectDiskDrives;InheritRedirectKeys;InheritRedirectPorts;InheritRedirectPrinters;InheritRedirectSmartCards;InheritRedirectSound;InheritResolution;InheritAutomaticResize;InheritUseConsoleSession;InheritUseCredSsp;InheritRenderingEngine;InheritUsername;InheritICAEncryptionStrength;InheritRDPAuthenticationLevel;InheritLoadBalanceInfo;InheritPreExtApp;InheritPostExtApp;InheritMacAddress;InheritUserField;InheritExtApp;InheritVNCCompression;InheritVNCEncoding;InheritVNCAuthMode;InheritVNCProxyType;InheritVNCProxyIP;InheritVNCProxyPort;InheritVNCProxyUsername;InheritVNCProxyPassword;InheritVNCColors;InheritVNCSmartSizeMode;InheritVNCViewOnly;InheritRDGatewayUsageMethod;InheritRDGatewayHostname;InheritRDGatewayUseConnectionCredentials;InheritRDGatewayUsername;InheritRDGatewayPassword;InheritRDGatewayDomain;InheritRDPAlertIdleTimeout;InheritRDPMinutesToIdleTimeout;InheritSoundQuality");
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
            sb.Append(string.Join(";", 
                CleanStringForCsv(con.Name),
                CleanStringForCsv(con.ConstantID),
                CleanStringForCsv(con.Parent?.ConstantID ?? ""),
                CleanStringForCsv(con.GetTreeNodeType()),
                CleanStringForCsv(con.Description),
                CleanStringForCsv(con.Icon),
                CleanStringForCsv(con.Panel)))
                .Append(";");

            if (_saveFilter.SaveUsername)
                sb.Append($"{CleanStringForCsv(con.Username)};");

            if (_saveFilter.SavePassword)
                sb.Append($"{CleanStringForCsv(con.Password)};");

            if (_saveFilter.SaveDomain)
                sb.Append($"{CleanStringForCsv(con.Domain)};");

            sb.Append(string.Join(";",
                CleanStringForCsv(con.Hostname),
                CleanStringForCsv(con.Protocol),
                CleanStringForCsv(con.PuttySession),
                CleanStringForCsv(con.Port),
                CleanStringForCsv(con.UseConsoleSession),
                CleanStringForCsv(con.UseCredSsp),
                CleanStringForCsv(con.RenderingEngine),
                CleanStringForCsv(con.ICAEncryptionStrength),
                CleanStringForCsv(con.RDPAuthenticationLevel),
                CleanStringForCsv(con.LoadBalanceInfo),
                CleanStringForCsv(con.Colors),
                CleanStringForCsv(con.Resolution),
                CleanStringForCsv(con.AutomaticResize),
                CleanStringForCsv(con.DisplayWallpaper),
                CleanStringForCsv(con.DisplayThemes),
                CleanStringForCsv(con.EnableFontSmoothing),
                CleanStringForCsv(con.EnableDesktopComposition),
                CleanStringForCsv(con.CacheBitmaps),
                CleanStringForCsv(con.RedirectDiskDrives),
                CleanStringForCsv(con.RedirectPorts),
                CleanStringForCsv(con.RedirectPrinters),
                CleanStringForCsv(con.RedirectSmartCards),
                CleanStringForCsv(con.RedirectSound),
                CleanStringForCsv(con.RedirectKeys),
                CleanStringForCsv(con.PreExtApp),
                CleanStringForCsv(con.PostExtApp),
                CleanStringForCsv(con.MacAddress),
                CleanStringForCsv(con.UserField),
                CleanStringForCsv(con.ExtApp),
                CleanStringForCsv(con.VNCCompression),
                CleanStringForCsv(con.VNCEncoding),
                CleanStringForCsv(con.VNCAuthMode),
                CleanStringForCsv(con.VNCProxyType),
                CleanStringForCsv(con.VNCProxyIP),
                CleanStringForCsv(con.VNCProxyPort),
                CleanStringForCsv(con.VNCProxyUsername),
                CleanStringForCsv(con.VNCProxyPassword),
                CleanStringForCsv(con.VNCColors),
                CleanStringForCsv(con.VNCSmartSizeMode),
                CleanStringForCsv(con.VNCViewOnly),
                CleanStringForCsv(con.RDGatewayUsageMethod),
                CleanStringForCsv(con.RDGatewayHostname),
                CleanStringForCsv(con.RDGatewayUseConnectionCredentials),
                CleanStringForCsv(con.RDGatewayUsername),
                CleanStringForCsv(con.RDGatewayPassword),
                CleanStringForCsv(con.RDGatewayDomain)))
                .Append(";");


            if (!_saveFilter.SaveInheritance)
                return;

            sb.Append(string.Join(";",
                con.Inheritance.CacheBitmaps,
                con.Inheritance.Colors,
                con.Inheritance.Description,
                con.Inheritance.DisplayThemes,
                con.Inheritance.DisplayWallpaper,
                con.Inheritance.EnableFontSmoothing,
                con.Inheritance.EnableDesktopComposition,
                con.Inheritance.Domain,
                con.Inheritance.Icon,
                con.Inheritance.Panel,
                con.Inheritance.Password,
                con.Inheritance.Port,
                con.Inheritance.Protocol,
                con.Inheritance.PuttySession,
                con.Inheritance.RedirectDiskDrives,
                con.Inheritance.RedirectKeys,
                con.Inheritance.RedirectPorts,
                con.Inheritance.RedirectPrinters,
                con.Inheritance.RedirectSmartCards,
                con.Inheritance.RedirectSound,
                con.Inheritance.Resolution,
                con.Inheritance.AutomaticResize,
                con.Inheritance.UseConsoleSession,
                con.Inheritance.UseCredSsp,
                con.Inheritance.RenderingEngine,
                con.Inheritance.Username,
                con.Inheritance.ICAEncryptionStrength,
                con.Inheritance.RDPAuthenticationLevel,
                con.Inheritance.LoadBalanceInfo,
                con.Inheritance.PreExtApp,
                con.Inheritance.PostExtApp,
                con.Inheritance.MacAddress,
                con.Inheritance.UserField,
                con.Inheritance.ExtApp,
                con.Inheritance.VNCCompression,
                con.Inheritance.VNCEncoding,
                con.Inheritance.VNCAuthMode,
                con.Inheritance.VNCProxyType,
                con.Inheritance.VNCProxyIP,
                con.Inheritance.VNCProxyPort,
                con.Inheritance.VNCProxyUsername,
                con.Inheritance.VNCProxyPassword,
                con.Inheritance.VNCColors,
                con.Inheritance.VNCSmartSizeMode,
                con.Inheritance.VNCViewOnly,
                con.Inheritance.RDGatewayUsageMethod,
                con.Inheritance.RDGatewayHostname,
                con.Inheritance.RDGatewayUseConnectionCredentials,
                con.Inheritance.RDGatewayUsername,
                con.Inheritance.RDGatewayPassword,
                con.Inheritance.RDGatewayDomain,
                con.Inheritance.RDPAlertIdleTimeout,
                con.Inheritance.RDPMinutesToIdleTimeout,
                con.Inheritance.SoundQuality));
        }

        /// <summary>
        /// Remove text that is unsafe for use in CSV files
        /// </summary>
        /// <param name="text"></param>
        private string CleanStringForCsv(object text)
        {
            return text.ToString().Replace(";", "");
        }
    }
}