using System;
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
    public class MtputtyDeserializer : IDeserializer<string, ConnectionTreeModel>
    {
        public ConnectionTreeModel Deserialize(string mtputtyXml)
        {
            ConnectionTreeModel connectionTreeModel = new();
            RootNodeInfo root = new(RootNodeType.Connection);
            connectionTreeModel.AddRootNode(root);

            XmlDocument xmlDocument = SecureXmlHelper.LoadXmlFromString(mtputtyXml);

            XmlNode? arrayOfSession = xmlDocument.SelectSingleNode("/ArrayOfSession");
            if (arrayOfSession == null) return connectionTreeModel;

            XmlNodeList? topLevelSessions = arrayOfSession.SelectNodes("./Session");
            if (topLevelSessions == null) return connectionTreeModel;

            foreach (XmlNode sessionNode in topLevelSessions)
            {
                ImportSession(sessionNode, root);
            }

            return connectionTreeModel;
        }

        private static void ImportSession(XmlNode sessionNode, ContainerInfo parentContainer)
        {
            bool isFolder = string.Equals(
                sessionNode.SelectSingleNode("./IsFolder")?.InnerText,
                "true",
                StringComparison.OrdinalIgnoreCase);

            if (isFolder)
            {
                string folderName = sessionNode.SelectSingleNode("./SessionDisplayName")?.InnerText ?? string.Empty;
                ContainerInfo container = new() { Name = folderName };
                parentContainer.AddChild(container);

                XmlNodeList? subSessions = sessionNode.SelectNodes("./Subsessions/Session");
                if (subSessions != null)
                {
                    foreach (XmlNode subSession in subSessions)
                    {
                        ImportSession(subSession, container);
                    }
                }
            }
            else
            {
                ConnectionInfo connectionInfo = ConnectionInfoFromNode(sessionNode);
                parentContainer.AddChild(connectionInfo);
            }
        }

        private static ConnectionInfo ConnectionInfoFromNode(XmlNode sessionNode)
        {
            string name = sessionNode.SelectSingleNode("./SessionDisplayName")?.InnerText ?? string.Empty;
            ConnectionInfo connectionInfo = new() { Name = name };

            string? protocolStr = sessionNode.SelectSingleNode("./SessionProtocol")?.InnerText;
            if (int.TryParse(protocolStr, out int protocol))
            {
                connectionInfo.Protocol = MapProtocol(protocol);
            }
            else
            {
                connectionInfo.Protocol = ProtocolType.SSH2;
            }

            connectionInfo.Hostname = sessionNode.SelectSingleNode("./ServerName")?.InnerText ?? string.Empty;

            string? portStr = sessionNode.SelectSingleNode("./ServerPort")?.InnerText;
            if (int.TryParse(portStr, out int port) && port > 0)
            {
                connectionInfo.Port = port;
            }
            else
            {
                connectionInfo.Port = DefaultPort(connectionInfo.Protocol);
            }

            connectionInfo.Username = sessionNode.SelectSingleNode("./SessionUsername")?.InnerText ?? string.Empty;

            string? puttySession = sessionNode.SelectSingleNode("./PuttySession")?.InnerText;
            if (!string.IsNullOrEmpty(puttySession))
            {
                connectionInfo.PuttySession = Uri.UnescapeDataString(puttySession);
            }

            return connectionInfo;
        }

        private static ProtocolType MapProtocol(int mtputtyProtocol)
        {
            return mtputtyProtocol switch
            {
                0 => ProtocolType.SSH2,
                1 => ProtocolType.Telnet,
                2 => ProtocolType.RAW,
                3 => ProtocolType.RAW,
                4 => ProtocolType.Telnet,
                _ => ProtocolType.SSH2
            };
        }

        private static int DefaultPort(ProtocolType protocol)
        {
            return protocol switch
            {
                ProtocolType.SSH2 => 22,
                ProtocolType.SSH1 => 22,
                ProtocolType.Telnet => 23,
                ProtocolType.RAW => 23,
                _ => 22
            };
        }
    }
}
