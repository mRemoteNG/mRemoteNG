using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using System;
using System.Globalization;
using System.IO;
using System.Runtime.Versioning;
using System.Xml;

namespace mRemoteNG.Config.Serializers.MiscSerializers
{
    [SupportedOSPlatform("windows")]
    public static class SecureCRTFileDeserializer
    {
        enum SecureCRTNodeType { folder, session };

        public static ConnectionTreeModel Deserialize(string content)
        {
            ConnectionTreeModel connectionTreeModel = new();
            RootNodeInfo root = new(RootNodeType.Connection);
            connectionTreeModel.AddRootNode(root);

            XmlDocument xmlDocument = SecureXmlHelper.LoadXmlFromString(content);

            XmlNode? sessionsNode = xmlDocument.SelectSingleNode("/VanDyke/key[@name=\"Sessions\"]");
            if (sessionsNode == null)
                return connectionTreeModel;

            ImportRootOrContainer(sessionsNode, root);

            return connectionTreeModel;
        }

        private static void ImportRootOrContainer(XmlNode rootNode, ContainerInfo parentContainer)
        {
            ContainerInfo newContainer = ImportContainer(rootNode, parentContainer);

            if (rootNode.ChildNodes.Count == 0)
                return;

            foreach (XmlNode child in rootNode.ChildNodes)
            {
                string? name = child.Attributes?["name"]?.Value;
                if (string.Equals(name, "Default", StringComparison.Ordinal) || string.Equals(name, "Default_LocalShell", StringComparison.Ordinal))
                    continue;
                SecureCRTNodeType nodeType = GetFolderOrSession(child);
                switch (nodeType)
                {
                    case SecureCRTNodeType.folder:
                        ImportRootOrContainer(child, newContainer);
                        break;
                    case SecureCRTNodeType.session:
                        ImportConnection(child, newContainer);
                        break;
                }
            }
        }

        private static void ImportConnection(XmlNode childNode, ContainerInfo parentContainer)
        {
            ConnectionInfo? connectionInfo = ConnectionInfoFromXml(childNode);
            if (connectionInfo == null)
                return;

            parentContainer.AddChild(connectionInfo);
        }

        private static ContainerInfo ImportContainer(XmlNode containerNode, ContainerInfo parentContainer)
        {
            ContainerInfo containerInfo = new()
            {
                Name = containerNode.Attributes?["name"]?.InnerText ?? string.Empty
            };
            parentContainer.AddChild(containerInfo);
            return containerInfo;
        }

        private static SecureCRTNodeType GetFolderOrSession(XmlNode xmlNode)
        {
            if (GetHostnameFromNode(xmlNode) == null)
                return SecureCRTNodeType.folder;

            return SecureCRTNodeType.session;
        }

        private static ConnectionInfo? ConnectionInfoFromXml(XmlNode xmlNode)
        {
            ConnectionInfo connectionInfo = new();
            try
            {
                connectionInfo.Name = xmlNode.Attributes?["name"]?.InnerText ?? string.Empty;
                connectionInfo.Hostname = GetHostnameFromNode(xmlNode) ?? string.Empty;
                connectionInfo.Protocol = GetProtocolFromNode(xmlNode);
                connectionInfo.Port = GetPortFromNode(xmlNode, connectionInfo.Protocol);
                connectionInfo.Username = GetUsernameFromNode(xmlNode) ?? string.Empty;
                connectionInfo.Description = GetDescriptionFromNode(xmlNode);
            }
            catch (FileFormatException e)
            {
                Runtime.MessageCollector.AddExceptionMessage("Error when parsing SecureCRT node: ", e);
                return null;
            }

            return connectionInfo;
        }

        private static string? GetHostnameFromNode(XmlNode xmlNode)
        {
            return xmlNode.SelectSingleNode("string[@name=\"Hostname\"]")?.InnerText;
        }

        private static string? GetUsernameFromNode(XmlNode xmlNode)
        {
            return xmlNode.SelectSingleNode("string[@name=\"Username\"]")?.InnerText;
        }

        private static int GetPortFromNode(XmlNode xmlNode, ProtocolType protocol)
        {
            switch (protocol)
            {
                case ProtocolType.SSH1:
                    return Convert.ToInt32(xmlNode.SelectSingleNode("dword[@name=\"[SSH1] Port\"]")?.InnerText, CultureInfo.InvariantCulture);
                case ProtocolType.SSH2:
                    return Convert.ToInt32(xmlNode.SelectSingleNode("dword[@name=\"[SSH2] Port\"]")?.InnerText, CultureInfo.InvariantCulture);
                default:
                    return Convert.ToInt32(xmlNode.SelectSingleNode("dword[@name=\"Port\"]")?.InnerText, CultureInfo.InvariantCulture);
            }
        }

        private static ProtocolType GetProtocolFromNode(XmlNode xmlNode)
        {
            XmlNode? protocolNode = xmlNode.SelectSingleNode("string[@name=\"Protocol Name\"]");
            if (protocolNode == null)
                throw new FileFormatException($"Protocol node not found");

            string protocolText = protocolNode.InnerText.ToUpperInvariant();
            switch (protocolText)
            {
                case "RDP":
                    return ProtocolType.RDP;
                case "RAW":
                    return ProtocolType.RAW;
                case "RLOGIN":
                    return ProtocolType.Rlogin;
                case "SSH1":
                    return ProtocolType.SSH1;
                case "SSH2":
                    return ProtocolType.SSH2;
                case "TELNET":
                    return ProtocolType.Telnet;
                default:
                    throw new FileFormatException($"Unrecognized protocol ({protocolText}).");
            }
        }

        private static string GetDescriptionFromNode(XmlNode xmlNode)
        {
            string description = string.Empty;
            XmlNode? descNode = xmlNode.SelectSingleNode("array[@name=\"Description\"]");
            if (descNode == null)
                return description;
            foreach(XmlNode n in descNode.ChildNodes)
            {
                description += n.InnerText + " ";
            }

            return description.TrimEnd();
        }
    }
}
