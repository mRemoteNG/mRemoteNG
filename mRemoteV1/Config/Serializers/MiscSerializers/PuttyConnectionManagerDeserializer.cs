using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Xml;
using mRemoteNG.Credential;
using mRemoteNG.Security;

namespace mRemoteNG.Config.Serializers
{
    public class PuttyConnectionManagerDeserializer
    {
        public SerializationResult Deserialize(string puttycmConnectionsXml)
        {
            var result = new SerializationResult(new List<ConnectionInfo>(), new ConnectionToCredentialMap());

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(puttycmConnectionsXml);

            var configurationNode = xmlDocument.SelectSingleNode("/configuration");

            var rootXmlNode = configurationNode?.SelectSingleNode("./root");
            if (rootXmlNode == null)
                return result;

            var rootContainer = ReadContainerProperties(rootXmlNode);
            result.ConnectionRecords.Add(rootContainer);

            foreach (XmlNode node in rootXmlNode.ChildNodes)
            {
                rootContainer.AddChild(ImportRecursive(node, result.ConnectionToCredentialMap));
            }

            return result;
        }

        private ContainerInfo ImportRecursive(XmlNode xmlNode, ConnectionToCredentialMap credentialMap)
        {
            VerifyNodeType(xmlNode);

            var newContainer = ReadContainerProperties(xmlNode);

            var childNodes = xmlNode.SelectNodes("./*");
            if (childNodes == null)
                return newContainer;

            foreach (XmlNode childNode in childNodes)
            {
                switch (childNode.Name)
                {
                    case "container":
                        newContainer.AddChild(ImportRecursive(childNode, credentialMap));
                        break;
                    case "connection":
                        newContainer.AddChild(ImportConnection(childNode, credentialMap));
                        break;
                    default:
                        throw new FileFormatException($"Unrecognized child node ({childNode.Name}).");
                }
            }

            return newContainer;
        }

        private void VerifyNodeType(XmlNode xmlNode)
        {
            var xmlNodeType = xmlNode?.Attributes?["type"].Value;
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
                    throw (new ArgumentException("Argument must be either a root or a container node.", nameof(xmlNode)));
            }
        }

        private ContainerInfo ReadContainerProperties(XmlNode containerNode)
        {
            var containerInfo = new ContainerInfo
            {
                Name = containerNode.Attributes?["name"].Value,
                IsExpanded = bool.Parse(containerNode.Attributes?["expanded"].InnerText ?? "false")
            };
            
            return containerInfo;
        }

        private ConnectionInfo ImportConnection(XmlNode connectionNode, ConnectionToCredentialMap credentialMap)
        {
            var connectionNodeType = connectionNode.Attributes?["type"].Value;
            if (string.Compare(connectionNodeType, "PuTTY", StringComparison.OrdinalIgnoreCase) != 0)
                throw (new FileFormatException($"Unrecognized connection node type ({connectionNodeType})."));

            var connectionInfo = ConnectionInfoFromXml(connectionNode);
            var cred = CredentialFromXml(connectionNode);
            credentialMap.Add(Guid.Parse(connectionInfo.ConstantID), cred);
            return connectionInfo;
        }

        private ConnectionInfo ConnectionInfoFromXml(XmlNode xmlNode)
        {
            var connectionInfoNode = xmlNode.SelectSingleNode("./connection_info");

            var name = connectionInfoNode?.SelectSingleNode("./name")?.InnerText;
            var connectionInfo = new ConnectionInfo {Name = name};

            var protocol = connectionInfoNode?.SelectSingleNode("./protocol")?.InnerText;
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

        private ICredentialRecord CredentialFromXml(XmlNode xmlNode)
        {
            var loginNode = xmlNode.SelectSingleNode("./login");
            var username = loginNode?.SelectSingleNode("login")?.InnerText ?? "";

            return new CredentialRecord
            {
                Title = username,
                Username = username,
                Domain = "",
                Password = loginNode?.SelectSingleNode("password")?.InnerText.ConvertToSecureString() ?? new SecureString()
            };
        }
    }
}