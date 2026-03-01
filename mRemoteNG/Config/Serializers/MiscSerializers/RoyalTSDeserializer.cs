using System;
using System.IO;
using System.Runtime.Versioning;
using System.Xml;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers.MiscSerializers
{
    [SupportedOSPlatform("windows")]
    public class RoyalTSDeserializer : IDeserializer<string, ConnectionTreeModel>
    {
        public ConnectionTreeModel Deserialize(string fileContent)
        {
            ConnectionTreeModel connectionTreeModel = new();
            RootNodeInfo root = new(RootNodeType.Connection);
            connectionTreeModel.AddRootNode(root);

            XmlDocument xmlDocument = SecureXmlHelper.LoadXmlFromString(fileContent);

            // RoyalTS .rtsx files have a root element (typically RoyalDocument or similar)
            // containing child object elements. We iterate all child nodes and import
            // based on their Type element value.
            XmlElement? documentElement = xmlDocument.DocumentElement;
            if (documentElement == null)
                return connectionTreeModel;

            ImportChildObjects(documentElement, root);

            return connectionTreeModel;
        }

        private void ImportChildObjects(XmlNode parentNode, ContainerInfo parentContainer)
        {
            foreach (XmlNode childNode in parentNode.ChildNodes)
            {
                if (childNode.NodeType != XmlNodeType.Element)
                    continue;

                string objectType = GetObjectType(childNode);

                switch (objectType)
                {
                    case "RoyalFolder":
                        ImportFolder(childNode, parentContainer);
                        break;
                    case "RoyalRDSConnection":
                        ImportRDPConnection(childNode, parentContainer);
                        break;
                    case "RoyalSSHConnection":
                        ImportSSHConnection(childNode, parentContainer);
                        break;
                    case "RoyalVNCConnection":
                        ImportVNCConnection(childNode, parentContainer);
                        break;
                    case "RoyalWebConnection":
                        ImportWebConnection(childNode, parentContainer);
                        break;
                    case "RoyalFileTransferConnection":
                        ImportFileTransferConnection(childNode, parentContainer);
                        break;
                    case "RoyalPowerShellConnection":
                        ImportPowerShellConnection(childNode, parentContainer);
                        break;
                    default:
                        // For unknown types or container elements that group objects,
                        // try to recurse into children to find importable objects
                        if (childNode.HasChildNodes)
                            ImportChildObjects(childNode, parentContainer);
                        break;
                }
            }
        }

        private static string GetObjectType(XmlNode xmlNode)
        {
            // RoyalTS uses either the element name itself as the type,
            // or has a <Type> child element, or a "Type" attribute
            string? typeAttr = xmlNode.Attributes?["Type"]?.Value;
            if (!string.IsNullOrEmpty(typeAttr))
                return typeAttr;

            string? typeElement = xmlNode.SelectSingleNode("Type")?.InnerText;
            if (!string.IsNullOrEmpty(typeElement))
                return typeElement;

            // Fall back to the element name (e.g., <RoyalRDSConnection>)
            return xmlNode.Name;
        }

        private void ImportFolder(XmlNode folderNode, ContainerInfo parentContainer)
        {
            ContainerInfo containerInfo = new()
            {
                Name = GetStringProperty(folderNode, "Name") ?? "Folder",
                Description = GetStringProperty(folderNode, "Description") ?? string.Empty,
                IsExpanded = true
            };

            parentContainer.AddChild(containerInfo);

            // Process children within the folder
            // RoyalTS folders may contain objects directly or in an <Objects> sub-element
            XmlNode? objectsNode = folderNode.SelectSingleNode("Objects");
            if (objectsNode != null)
                ImportChildObjects(objectsNode, containerInfo);
            else
                ImportChildObjects(folderNode, containerInfo);
        }

        private static void ImportRDPConnection(XmlNode connectionNode, ContainerInfo parentContainer)
        {
            ConnectionInfo connectionInfo = new() { Protocol = ProtocolType.RDP };

            SetCommonProperties(connectionNode, connectionInfo);

            // RDP-specific port (RoyalTS uses RDPPort for RDP connections)
            string? rdpPort = GetStringProperty(connectionNode, "RDPPort");
            if (int.TryParse(rdpPort, out int port) && port > 0)
                connectionInfo.Port = port;
            else if (connectionInfo.Port == 0)
                connectionInfo.Port = 3389;

            // Console/Admin session
            string? consoleSession = GetStringProperty(connectionNode, "ConnectToAdministerOrConsole");
            if (bool.TryParse(consoleSession, out bool useConsole))
                connectionInfo.UseConsoleSession = useConsole;

            // Network Level Authentication
            string? nla = GetStringProperty(connectionNode, "NetworkLevelAuthentication");
            if (bool.TryParse(nla, out bool useNla) && !useNla)
                connectionInfo.RDPAuthenticationLevel = AuthenticationLevel.NoAuth;

            // RDP Gateway
            string? gatewayHost = GetStringProperty(connectionNode, "GatewayHostName");
            if (!string.IsNullOrEmpty(gatewayHost))
            {
                connectionInfo.RDGatewayHostname = gatewayHost;
                connectionInfo.RDGatewayUsageMethod = RDGatewayUsageMethod.Always;
                connectionInfo.RDGatewayUsername = GetStringProperty(connectionNode, "GatewayUserName") ?? string.Empty;
                connectionInfo.RDGatewayPassword = GetStringProperty(connectionNode, "GatewayPassword") ?? string.Empty;
            }

            // Redirection settings
            string? redirectDrives = GetStringProperty(connectionNode, "RedirectDrives");
            if (bool.TryParse(redirectDrives, out bool drives))
                connectionInfo.RedirectDiskDrives = drives ? RDPDiskDrives.Local : RDPDiskDrives.None;

            string? redirectPrinters = GetStringProperty(connectionNode, "RedirectPrinters");
            if (bool.TryParse(redirectPrinters, out bool printers))
                connectionInfo.RedirectPrinters = printers;

            string? redirectSmartCards = GetStringProperty(connectionNode, "RedirectSmartCards");
            if (bool.TryParse(redirectSmartCards, out bool smartCards))
                connectionInfo.RedirectSmartCards = smartCards;

            string? redirectClipboard = GetStringProperty(connectionNode, "RedirectClipboard");
            if (bool.TryParse(redirectClipboard, out bool clipboard))
                connectionInfo.RedirectClipboard = clipboard;

            string? redirectPorts = GetStringProperty(connectionNode, "RedirectPorts");
            if (bool.TryParse(redirectPorts, out bool ports))
                connectionInfo.RedirectPorts = ports;

            parentContainer.AddChild(connectionInfo);
        }

        private static void ImportSSHConnection(XmlNode connectionNode, ContainerInfo parentContainer)
        {
            // Determine SSH sub-type: SSH or Telnet
            string? termType = GetStringProperty(connectionNode, "TerminalConnectionType");
            ProtocolType protocol = termType?.Equals("Telnet", StringComparison.OrdinalIgnoreCase) == true
                ? ProtocolType.Telnet
                : ProtocolType.SSH2;

            ConnectionInfo connectionInfo = new() { Protocol = protocol };

            SetCommonProperties(connectionNode, connectionInfo);

            if (connectionInfo.Port == 0)
                connectionInfo.Port = protocol == ProtocolType.Telnet ? 23 : 22;

            parentContainer.AddChild(connectionInfo);
        }

        private static void ImportVNCConnection(XmlNode connectionNode, ContainerInfo parentContainer)
        {
            ConnectionInfo connectionInfo = new() { Protocol = ProtocolType.VNC };

            SetCommonProperties(connectionNode, connectionInfo);

            if (connectionInfo.Port == 0)
                connectionInfo.Port = 5900;

            parentContainer.AddChild(connectionInfo);
        }

        private static void ImportWebConnection(XmlNode connectionNode, ContainerInfo parentContainer)
        {
            ConnectionInfo connectionInfo = new();

            SetCommonProperties(connectionNode, connectionInfo);

            // Determine HTTP vs HTTPS from the URI
            string hostname = connectionInfo.Hostname;
            if (hostname.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                connectionInfo.Protocol = ProtocolType.HTTPS;
            else
                connectionInfo.Protocol = ProtocolType.HTTP;

            parentContainer.AddChild(connectionInfo);
        }

        private static void ImportFileTransferConnection(XmlNode connectionNode, ContainerInfo parentContainer)
        {
            // RoyalTS file-transfer entries are imported as SSH2 connections.
            ConnectionInfo connectionInfo = new() { Protocol = ProtocolType.SSH2 };

            SetCommonProperties(connectionNode, connectionInfo);

            if (connectionInfo.Port == 0)
                connectionInfo.Port = 22;

            parentContainer.AddChild(connectionInfo);
        }

        private static void ImportPowerShellConnection(XmlNode connectionNode, ContainerInfo parentContainer)
        {
            ConnectionInfo connectionInfo = new() { Protocol = ProtocolType.PowerShell };

            SetCommonProperties(connectionNode, connectionInfo);

            parentContainer.AddChild(connectionInfo);
        }

        private static void SetCommonProperties(XmlNode xmlNode, ConnectionInfo connectionInfo)
        {
            connectionInfo.Name = GetStringProperty(xmlNode, "Name") ?? string.Empty;
            connectionInfo.Description = GetStringProperty(xmlNode, "Description") ?? string.Empty;

            // URI is the hostname/IP in RoyalTS
            string? uri = GetStringProperty(xmlNode, "URI");
            if (!string.IsNullOrEmpty(uri))
                connectionInfo.Hostname = uri;

            // Credentials - RoyalTS uses CredentialUsername/CredentialPassword
            string? username = GetStringProperty(xmlNode, "CredentialUsername");
            if (!string.IsNullOrEmpty(username))
                connectionInfo.Username = username;

            string? password = GetStringProperty(xmlNode, "CredentialPassword");
            if (!string.IsNullOrEmpty(password))
                connectionInfo.Password = password;

            string? domain = GetStringProperty(xmlNode, "CredentialDomain");
            if (!string.IsNullOrEmpty(domain))
                connectionInfo.Domain = domain;

            // Generic port (used by SSH, VNC, etc.)
            string? portStr = GetStringProperty(xmlNode, "Port");
            if (int.TryParse(portStr, out int port) && port > 0)
                connectionInfo.Port = port;
        }

        private static string? GetStringProperty(XmlNode parentNode, string propertyName)
        {
            // RoyalTS XML stores properties as child elements: <PropertyName>value</PropertyName>
            XmlNode? node = parentNode.SelectSingleNode(propertyName);
            return node?.InnerText;
        }
    }
}
