using System;
using System.Globalization;
using System.IO;
using System.Runtime.Versioning;
using System.Xml;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers.MiscSerializers
{
    [SupportedOSPlatform("windows")]
    public class PuttyConnectionManagerDeserializer : IDeserializer<string, ConnectionTreeModel>
    {
        public ConnectionTreeModel Deserialize(string puttycmConnectionsXml)
        {
            ConnectionTreeModel connectionTreeModel = new();
            RootNodeInfo root = new(RootNodeType.Connection);
            connectionTreeModel.AddRootNode(root);

            XmlDocument xmlDocument = SecureXmlHelper.LoadXmlFromString(puttycmConnectionsXml);

            XmlNode? configurationNode = xmlDocument.SelectSingleNode("/configuration");

            XmlNodeList? rootNodes = configurationNode?.SelectNodes("./root");
            if (rootNodes == null) return connectionTreeModel;
            foreach (XmlNode rootNode in rootNodes)
            {
                ImportRootOrContainer(rootNode, root);
            }

            return connectionTreeModel;
        }

        private static void ImportRootOrContainer(XmlNode xmlNode, ContainerInfo parentContainer)
        {
            VerifyNodeType(xmlNode);

            ContainerInfo newContainer = ImportContainer(xmlNode, parentContainer);

            XmlNodeList? childNodes = xmlNode.SelectNodes("./*");
            if (childNodes == null) return;
            foreach (XmlNode childNode in childNodes)
            {
                switch (childNode.Name)
                {
                    case "container":
                        ImportRootOrContainer(childNode, newContainer);
                        break;
                    case "connection":
                        ImportConnection(childNode, newContainer);
                        break;
                    default:
                        throw (new FileFormatException($"Unrecognized child node ({childNode.Name})."));
                }
            }
        }

        private static void VerifyNodeType(XmlNode xmlNode)
        {
            string? xmlNodeType = xmlNode.Attributes?["type"]?.Value;
            switch (xmlNode.Name)
            {
                case "root":
                    if (!string.Equals(xmlNodeType, "database", StringComparison.OrdinalIgnoreCase))
                    {
                        throw (new FileFormatException($"Unrecognized root node type ({xmlNodeType})."));
                    }

                    break;
                case "container":
                    if (!string.Equals(xmlNodeType, "folder", StringComparison.OrdinalIgnoreCase))
                    {
                        throw (new FileFormatException($"Unrecognized root node type ({xmlNodeType})."));
                    }

                    break;
                default:
                    // ReSharper disable once LocalizableElement
                    throw (new ArgumentException("Argument must be either a root or a container node.",
                                                 nameof(xmlNode)));
            }
        }

        private static ContainerInfo ImportContainer(XmlNode containerNode, ContainerInfo parentContainer)
        {
            ContainerInfo containerInfo = new()
            {
                Name = containerNode.Attributes?["name"]?.Value ?? string.Empty,
                IsExpanded = bool.Parse(containerNode.Attributes?["expanded"]?.InnerText ?? "false")
            };
            parentContainer.AddChild(containerInfo);
            return containerInfo;
        }

        private static void ImportConnection(XmlNode connectionNode, ContainerInfo parentContainer)
        {
            string? connectionNodeType = connectionNode.Attributes?["type"]?.Value;
            if (!string.Equals(connectionNodeType, "PuTTY", StringComparison.OrdinalIgnoreCase))
                throw (new FileFormatException($"Unrecognized connection node type ({connectionNodeType})."));

            ConnectionInfo connectionInfo = ConnectionInfoFromXml(connectionNode);
            parentContainer.AddChild(connectionInfo);
        }

        private static ConnectionInfo ConnectionInfoFromXml(XmlNode xmlNode)
        {
            XmlNode? connectionInfoNode = xmlNode.SelectSingleNode("./connection_info");

            string name = connectionInfoNode?.SelectSingleNode("./name")?.InnerText ?? string.Empty;
            ConnectionInfo connectionInfo = new() { Name = name};

            string? protocol = connectionInfoNode?.SelectSingleNode("./protocol")?.InnerText;
            switch (protocol?.ToLowerInvariant())
            {
                case "telnet":
                    connectionInfo.Protocol = ProtocolType.Telnet;
                    break;
                case "ssh":
                    connectionInfo.Protocol = ProtocolType.SSH2;
                    break;
                default:
                    throw new FileFormatException($"Unrecognized protocol ({protocol}).");
            }

            connectionInfo.Hostname = connectionInfoNode?.SelectSingleNode("./host")?.InnerText ?? string.Empty;
            connectionInfo.Port = Convert.ToInt32(connectionInfoNode?.SelectSingleNode("./port")?.InnerText, CultureInfo.InvariantCulture);
            connectionInfo.PuttySession = connectionInfoNode?.SelectSingleNode("./session")?.InnerText ?? string.Empty;
            // ./commandline
            connectionInfo.Description = connectionInfoNode?.SelectSingleNode("./description")?.InnerText ?? string.Empty;

            XmlNode? loginNode = xmlNode.SelectSingleNode("./login");
            connectionInfo.Username = loginNode?.SelectSingleNode("login")?.InnerText ?? string.Empty;
            //connectionInfo.Password = loginNode?.SelectSingleNode("password")?.InnerText.ConvertToSecureString();
            connectionInfo.Password = loginNode?.SelectSingleNode("password")?.InnerText ?? string.Empty;
            // ./prompt

            // ./timeout/connectiontimeout
            // ./timeout/logintimeout
            // ./timeout/passwordtimeout
            // ./timeout/commandtimeout

            // ./command/command1
            // ./command/command2
            // ./command/command3
            // ./command/command4
            // ./command/command5

            // ./options/loginmacro
            // ./options/postcommands
            // ./options/endlinechar

            return connectionInfo;
        }
    }
}