using System;
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

            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(puttycmConnectionsXml);

            XmlNode configurationNode = xmlDocument.SelectSingleNode("/configuration");

            XmlNodeList rootNodes = configurationNode?.SelectNodes("./root");
            if (rootNodes == null) return connectionTreeModel;
            foreach (XmlNode rootNode in rootNodes)
            {
                ImportRootOrContainer(rootNode, root);
            }

            return connectionTreeModel;
        }

        private void ImportRootOrContainer(XmlNode xmlNode, ContainerInfo parentContainer)
        {
            VerifyNodeType(xmlNode);

            ContainerInfo newContainer = ImportContainer(xmlNode, parentContainer);

            XmlNodeList childNodes = xmlNode.SelectNodes("./*");
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

        private void VerifyNodeType(XmlNode xmlNode)
        {
            string xmlNodeType = xmlNode?.Attributes?["type"].Value;
            switch (xmlNode?.Name)
            {
                case "root":
                    if (string.Compare(xmlNodeType, "database", StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        throw (new FileFormatException($"Unrecognized root node type ({xmlNodeType})."));
                    }

                    break;
                case "container":
                    if (string.Compare(xmlNodeType, "folder", StringComparison.OrdinalIgnoreCase) != 0)
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

        private ContainerInfo ImportContainer(XmlNode containerNode, ContainerInfo parentContainer)
        {
            ContainerInfo containerInfo = new()
            {
                Name = containerNode.Attributes?["name"].Value,
                IsExpanded = bool.Parse(containerNode.Attributes?["expanded"].InnerText ?? "false")
            };
            parentContainer.AddChild(containerInfo);
            return containerInfo;
        }

        private void ImportConnection(XmlNode connectionNode, ContainerInfo parentContainer)
        {
            string connectionNodeType = connectionNode.Attributes?["type"].Value;
            if (string.Compare(connectionNodeType, "PuTTY", StringComparison.OrdinalIgnoreCase) != 0)
                throw (new FileFormatException($"Unrecognized connection node type ({connectionNodeType})."));

            ConnectionInfo connectionInfo = ConnectionInfoFromXml(connectionNode);
            parentContainer.AddChild(connectionInfo);
        }

        private ConnectionInfo ConnectionInfoFromXml(XmlNode xmlNode)
        {
            XmlNode connectionInfoNode = xmlNode.SelectSingleNode("./connection_info");

            string name = connectionInfoNode?.SelectSingleNode("./name")?.InnerText;
            ConnectionInfo connectionInfo = new() { Name = name};

            string protocol = connectionInfoNode?.SelectSingleNode("./protocol")?.InnerText;
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

            connectionInfo.Hostname = connectionInfoNode.SelectSingleNode("./host")?.InnerText;
            connectionInfo.Port = Convert.ToInt32(connectionInfoNode.SelectSingleNode("./port")?.InnerText);
            connectionInfo.PuttySession = connectionInfoNode.SelectSingleNode("./session")?.InnerText;
            // ./commandline
            connectionInfo.Description = connectionInfoNode.SelectSingleNode("./description")?.InnerText;

            XmlNode loginNode = xmlNode.SelectSingleNode("./login");
            connectionInfo.Username = loginNode?.SelectSingleNode("login")?.InnerText;
            connectionInfo.Password = loginNode?.SelectSingleNode("password")?.InnerText.ConvertToSecureString();
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