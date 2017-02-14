using System;
using System.IO;
using System.Xml;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using System.Security.Cryptography;
using System.Text;


namespace mRemoteNG.Config.Serializers
{
    public class RemoteDesktopConnectionManagerDeserializer : IDeserializer
    {
        private readonly string _rdcmConnectionsXml;

        public RemoteDesktopConnectionManagerDeserializer(string xml)
        {
            _rdcmConnectionsXml = xml;
        }

        public ConnectionTreeModel Deserialize()
        {
            var connectionTreeModel = new ConnectionTreeModel();
            var root = new RootNodeInfo(RootNodeType.Connection);

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(_rdcmConnectionsXml);


            var rdcManNode = xmlDocument.SelectSingleNode("/RDCMan");
            VerifySchemaVersion(rdcManNode);
            VerifyFileVersion(rdcManNode);

            var fileNode = rdcManNode?.SelectSingleNode("./file");
            ImportFileOrGroup(fileNode, root);

            connectionTreeModel.AddRootNode(root);
            return connectionTreeModel;
        }

        private void VerifySchemaVersion(XmlNode rdcManNode)
        {
            var schemaVersion = Convert.ToInt32(rdcManNode?.Attributes?["schemaVersion"].Value);
            if (schemaVersion != 1)
            {
                throw new FileFormatException($"Unsupported schema version ({schemaVersion}).");
            }
        }

        private void VerifyFileVersion(XmlNode rdcManNode)
        {
            var versionNode = rdcManNode.SelectSingleNode("./version")?.InnerText;
            if (versionNode != null)
            {
                var version = new Version(versionNode);
                if (!(version == new Version(2, 2)))
                {
                    throw new FileFormatException($"Unsupported file version ({version}).");
                }
            }
            else
            {
                throw new FileFormatException("Unknown file version");
            }
        }

        private void ImportFileOrGroup(XmlNode xmlNode, ContainerInfo parentContainer)
        {
            var propertiesNode = xmlNode.SelectSingleNode("./properties");
            var newContainer = ImportContainer(propertiesNode, parentContainer);

            var childNodes = xmlNode.SelectNodes("./group|./server");
            if (childNodes == null) return;
            foreach (XmlNode childNode in childNodes)
            {
                switch (childNode.Name)
                {
                    case "group":
                        ImportFileOrGroup(childNode, newContainer);
                        break;
                    case "server":
                        ImportServer(childNode, newContainer);
                        break;
                }
            }
        }

        private ContainerInfo ImportContainer(XmlNode containerPropertiesNode, ContainerInfo parentContainer)
        {
            var newContainer = new ContainerInfo();
            var connectionInfo = ConnectionInfoFromXml(containerPropertiesNode);
            newContainer.CopyFrom(connectionInfo);
            newContainer.Name = containerPropertiesNode?.SelectSingleNode("./name")?.InnerText ?? Language.strNewFolder;
            newContainer.IsExpanded = bool.Parse(containerPropertiesNode?.SelectSingleNode("./expanded")?.InnerText ?? "false");
            parentContainer.AddChild(newContainer);
            return newContainer;
        }

        private void ImportServer(XmlNode serverNode, ContainerInfo parentContainer)
        {
            var newConnectionInfo = ConnectionInfoFromXml(serverNode);
            parentContainer.AddChild(newConnectionInfo);
        }

        private ConnectionInfo ConnectionInfoFromXml(XmlNode xmlNode)
        {
            var connectionInfo = new ConnectionInfo {Protocol = ProtocolType.RDP};

            var hostname = xmlNode.SelectSingleNode("./name")?.InnerText;

            var displayName = xmlNode.SelectSingleNode("./displayName")?.InnerText ?? Language.strNewConnection;

            connectionInfo.Name = displayName;
            connectionInfo.Description = xmlNode.SelectSingleNode("./comment")?.InnerText;
            connectionInfo.Hostname = hostname;

            var logonCredentialsNode = xmlNode.SelectSingleNode("./logonCredentials");
            if (logonCredentialsNode?.Attributes?["inherit"].Value == "None")
            {
                connectionInfo.Username = logonCredentialsNode.SelectSingleNode("userName")?.InnerText;

                var passwordNode = logonCredentialsNode.SelectSingleNode("./password");
                connectionInfo.Password = passwordNode?.Attributes?["storeAsClearText"].Value == "True" ? passwordNode.InnerText : DecryptRdcManPassword(passwordNode?.InnerText);

                connectionInfo.Domain = logonCredentialsNode.SelectSingleNode("./domain")?.InnerText;
            }
            else
            {
                connectionInfo.Inheritance.Username = true;
                connectionInfo.Inheritance.Password = true;
                connectionInfo.Inheritance.Domain = true;
            }

            var connectionSettingsNode = xmlNode.SelectSingleNode("./connectionSettings");
            if (connectionSettingsNode?.Attributes?["inherit"].Value == "None")
            {
                connectionInfo.UseConsoleSession = bool.Parse(connectionSettingsNode.SelectSingleNode("./connectToConsole")?.InnerText ?? "false");
                // ./startProgram
                // ./workingDir
                connectionInfo.Port = Convert.ToInt32(connectionSettingsNode.SelectSingleNode("./port")?.InnerText);
            }
            else
            {
                connectionInfo.Inheritance.UseConsoleSession = true;
                connectionInfo.Inheritance.Port = true;
            }

            var gatewaySettingsNode = xmlNode.SelectSingleNode("./gatewaySettings");
            if (gatewaySettingsNode?.Attributes?["inherit"].Value == "None")
            {
                connectionInfo.RDGatewayUsageMethod = gatewaySettingsNode.SelectSingleNode("./enabled")?.InnerText == "True" ? ProtocolRDP.RDGatewayUsageMethod.Always : ProtocolRDP.RDGatewayUsageMethod.Never;
                connectionInfo.RDGatewayHostname = gatewaySettingsNode.SelectSingleNode("./hostName")?.InnerText;
                connectionInfo.RDGatewayUsername = gatewaySettingsNode.SelectSingleNode("./userName")?.InnerText;

                var passwordNode = gatewaySettingsNode.SelectSingleNode("./password");
                connectionInfo.RDGatewayPassword = passwordNode?.Attributes?["storeAsClearText"].Value == "True" ? passwordNode.InnerText : DecryptRdcManPassword(passwordNode?.InnerText);

                connectionInfo.RDGatewayDomain = gatewaySettingsNode.SelectSingleNode("./domain")?.InnerText;
                // ./logonMethod
                // ./localBypass
                // ./credSharing
            }
            else
            {
                connectionInfo.Inheritance.RDGatewayUsageMethod = true;
                connectionInfo.Inheritance.RDGatewayHostname = true;
                connectionInfo.Inheritance.RDGatewayUsername = true;
                connectionInfo.Inheritance.RDGatewayPassword = true;
                connectionInfo.Inheritance.RDGatewayDomain = true;
            }

            var remoteDesktopNode = xmlNode.SelectSingleNode("./remoteDesktop");
            if (remoteDesktopNode?.Attributes?["inherit"].Value == "None")
            {
                var resolutionString = remoteDesktopNode.SelectSingleNode("./size")?.InnerText.Replace(" ", "");
                try
                {
                    connectionInfo.Resolution = (ProtocolRDP.RDPResolutions)Enum.Parse(typeof(ProtocolRDP.RDPResolutions), "Res" + resolutionString);
                }
                catch (ArgumentException)
                {
                    connectionInfo.Resolution = ProtocolRDP.RDPResolutions.FitToWindow;
                }

                if (remoteDesktopNode.SelectSingleNode("./sameSizeAsClientArea")?.InnerText == "True")
                {
                    connectionInfo.Resolution = ProtocolRDP.RDPResolutions.FitToWindow;
                }

                if (remoteDesktopNode.SelectSingleNode("./fullScreen")?.InnerText == "True")
                {
                    connectionInfo.Resolution = ProtocolRDP.RDPResolutions.Fullscreen;
                }

                var colorDepth = remoteDesktopNode.SelectSingleNode("./colorDepth")?.InnerText;
                if (colorDepth != null)
                    connectionInfo.Colors = (ProtocolRDP.RDPColors)Enum.Parse(typeof(ProtocolRDP.RDPColors), colorDepth);
            }
            else
            {
                connectionInfo.Inheritance.Resolution = true;
                connectionInfo.Inheritance.Colors = true;
            }

            var localResourcesNode = xmlNode.SelectSingleNode("./localResources");
            if (localResourcesNode?.Attributes?["inherit"].Value == "None")
            {
                switch (localResourcesNode.SelectSingleNode("./audioRedirection")?.InnerText)
                {
                    case "0": // Bring to this computer
                        connectionInfo.RedirectSound = ProtocolRDP.RDPSounds.BringToThisComputer;
                        break;
                    case "1": // Leave at remote computer
                        connectionInfo.RedirectSound = ProtocolRDP.RDPSounds.LeaveAtRemoteComputer;
                        break;
                    case "2": // Do not play
                        connectionInfo.RedirectSound = ProtocolRDP.RDPSounds.DoNotPlay;
                        break;
                }

                // ./audioRedirectionQuality
                // ./audioCaptureRedirection

                switch (localResourcesNode.SelectSingleNode("./keyboardHook")?.InnerText)
                {
                    case "0": // On the local computer
                        connectionInfo.RedirectKeys = false;
                        break;
                    case "1": // On the remote computer
                        connectionInfo.RedirectKeys = true;
                        break;
                    case "2": // In full screen mode only
                        connectionInfo.RedirectKeys = false;
                        break;
                }

                // ./redirectClipboard
                connectionInfo.RedirectDiskDrives = bool.Parse(localResourcesNode.SelectSingleNode("./redirectDrives")?.InnerText ?? "false");
                connectionInfo.RedirectPorts = bool.Parse(localResourcesNode.SelectSingleNode("./redirectPorts")?.InnerText ?? "false");
                connectionInfo.RedirectPrinters = bool.Parse(localResourcesNode.SelectSingleNode("./redirectPrinters")?.InnerText ?? "false");
                connectionInfo.RedirectSmartCards = bool.Parse(localResourcesNode.SelectSingleNode("./redirectSmartCards")?.InnerText ?? "false");
            }
            else
            {
                connectionInfo.Inheritance.RedirectSound = true;
                connectionInfo.Inheritance.RedirectKeys = true;
                connectionInfo.Inheritance.RedirectDiskDrives = true;
                connectionInfo.Inheritance.RedirectPorts = true;
                connectionInfo.Inheritance.RedirectPrinters = true;
                connectionInfo.Inheritance.RedirectSmartCards = true;
            }

            var securitySettingsNode = xmlNode.SelectSingleNode("./securitySettings");
            if (securitySettingsNode?.Attributes?["inherit"].Value == "None")
            {
                switch (securitySettingsNode.SelectSingleNode("./authentication")?.InnerText)
                {
                    case "0": // No authentication
                        connectionInfo.RDPAuthenticationLevel = ProtocolRDP.AuthenticationLevel.NoAuth;
                        break;
                    case "1": // Do not connect if authentication fails
                        connectionInfo.RDPAuthenticationLevel = ProtocolRDP.AuthenticationLevel.AuthRequired;
                        break;
                    case "2": // Warn if authentication fails
                        connectionInfo.RDPAuthenticationLevel = ProtocolRDP.AuthenticationLevel.WarnOnFailedAuth;
                        break;
                }
            }
            else
            {
                connectionInfo.Inheritance.RDPAuthenticationLevel = true;
            }

            // ./displaySettings/thumbnailScale
            // ./displaySettings/liveThumbnailUpdates
            // ./displaySettings/showDisconnectedThumbnails

            return connectionInfo;
        }

        private string DecryptRdcManPassword(string ciphertext)
        {
            if (string.IsNullOrEmpty(ciphertext))
                return null;

            try
            {
                var plaintextData = ProtectedData.Unprotect(Convert.FromBase64String(ciphertext), new byte[] { }, DataProtectionScope.LocalMachine);
                var charArray = Encoding.Unicode.GetChars(plaintextData);
                return new string(charArray);
            }
            catch (Exception /*ex*/)
            {
                //Runtime.MessageCollector.AddExceptionMessage("RemoteDesktopConnectionManager.DecryptPassword() failed.", ex, logOnly: true);
                return null;
            }
        }
    }
}