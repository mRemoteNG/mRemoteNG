using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;


namespace mRemoteNG.Config.Serializers
{
    public class RemoteDesktopConnectionManagerDeserializer : IDeserializer<string, ConnectionTreeModel>
    {
        private static int _schemaVersion; /* 1 = RDCMan v2.2
                                       3 = RDCMan v2.7  */

        public ConnectionTreeModel Deserialize(string rdcmConnectionsXml)
        {
            var connectionTreeModel = new ConnectionTreeModel();
            var root = new RootNodeInfo(RootNodeType.Connection);

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(rdcmConnectionsXml);


            var rdcManNode = xmlDocument.SelectSingleNode("/RDCMan");
            VerifySchemaVersion(rdcManNode);
            VerifyFileVersion(rdcManNode);

            var fileNode = rdcManNode?.SelectSingleNode("./file");
            ImportFileOrGroup(fileNode, root);

            connectionTreeModel.AddRootNode(root);
            return connectionTreeModel;
        }

        private static void VerifySchemaVersion(XmlNode rdcManNode)
        {
            _schemaVersion = Convert.ToInt32(rdcManNode?.Attributes?["schemaVersion"].Value);
            if (_schemaVersion != 1 && _schemaVersion != 3)
            {
                throw (new FileFormatException($"Unsupported schema version ({_schemaVersion})."));
            }
        }

        private static void VerifyFileVersion(XmlNode rdcManNode)
        {
            var versionAttribute = rdcManNode?.Attributes?["programVersion"]?.Value;
            if (versionAttribute != null)
            {
                var version = new Version(versionAttribute);
                if (!(version == new Version(2, 7)))
                {
                    throw new FileFormatException($"Unsupported file version ({version}).");
                }
            }
            else
            {
                var versionNode = rdcManNode?.SelectSingleNode("./version")?.InnerText;
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
        }

        private static void ImportFileOrGroup(XmlNode xmlNode, ContainerInfo parentContainer)
        {
            var newContainer = ImportContainer(xmlNode, parentContainer);

            var childNodes = xmlNode.SelectNodes("./group|./server");
            if (childNodes == null) return;
            foreach (XmlNode childNode in childNodes)
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
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

        private static ContainerInfo ImportContainer(XmlNode containerPropertiesNode, ContainerInfo parentContainer)
        {
            if (_schemaVersion == 1)
            {
                // Program Version 2.2 wraps all setting inside the Properties tags 
                containerPropertiesNode = containerPropertiesNode.SelectSingleNode("./properties");
            }
            var newContainer = new ContainerInfo();
            var connectionInfo = ConnectionInfoFromXml(containerPropertiesNode);
            newContainer.CopyFrom(connectionInfo);

            if (_schemaVersion == 3)
            {
                // Program Version 2.7 wraps these properties
                containerPropertiesNode = containerPropertiesNode.SelectSingleNode("./properties");
            }
            newContainer.Name = containerPropertiesNode?.SelectSingleNode("./name")?.InnerText ?? Language.strNewFolder;
            newContainer.IsExpanded = bool.Parse(containerPropertiesNode?.SelectSingleNode("./expanded")?.InnerText ?? "false");
            parentContainer.AddChild(newContainer);
            return newContainer;
        }

        private static void ImportServer(XmlNode serverNode, ContainerInfo parentContainer)
        {
            var newConnectionInfo = ConnectionInfoFromXml(serverNode);
            parentContainer.AddChild(newConnectionInfo);
        }

        private static ConnectionInfo ConnectionInfoFromXml(XmlNode xmlNode)
        {
            var connectionInfo = new ConnectionInfo {Protocol = ProtocolType.RDP};


            var propertiesNode = xmlNode.SelectSingleNode("./properties");
            if (_schemaVersion == 1) propertiesNode = xmlNode;  // Version 2.2 defines the container name at the root instead
            connectionInfo.Hostname = propertiesNode?.SelectSingleNode("./name")?.InnerText ?? "";
            connectionInfo.Name = propertiesNode?.SelectSingleNode("./displayName")?.InnerText ?? connectionInfo.Hostname;
            connectionInfo.Description = propertiesNode?.SelectSingleNode("./comment")?.InnerText ?? string.Empty;

            var logonCredentialsNode = xmlNode.SelectSingleNode("./logonCredentials");
            if (logonCredentialsNode?.Attributes?["inherit"]?.Value == "None")
            {
                connectionInfo.Username = logonCredentialsNode.SelectSingleNode("userName")?.InnerText;

                var passwordNode = logonCredentialsNode.SelectSingleNode("./password");
                if (_schemaVersion == 1) // Version 2.2 allows clear text passwords
                {
                    connectionInfo.Password = passwordNode?.Attributes?["storeAsClearText"]?.Value == "True" 
                        ? passwordNode.InnerText 
                        : DecryptRdcManPassword(passwordNode?.InnerText);
                }
                else
                {
                    connectionInfo.Password = DecryptRdcManPassword(passwordNode?.InnerText);
                }

                connectionInfo.Domain = logonCredentialsNode.SelectSingleNode("./domain")?.InnerText;
            }
            else
            {
                connectionInfo.Inheritance.Username = true;
                connectionInfo.Inheritance.Password = true;
                connectionInfo.Inheritance.Domain = true;
            }

            var connectionSettingsNode = xmlNode.SelectSingleNode("./connectionSettings");
            if (connectionSettingsNode?.Attributes?["inherit"]?.Value == "None")
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
            if (gatewaySettingsNode?.Attributes?["inherit"]?.Value == "None")
            {
                connectionInfo.RDGatewayUsageMethod = gatewaySettingsNode.SelectSingleNode("./enabled")?.InnerText == "True" ? RdpProtocol.RDGatewayUsageMethod.Always : RdpProtocol.RDGatewayUsageMethod.Never;
                connectionInfo.RDGatewayHostname = gatewaySettingsNode.SelectSingleNode("./hostName")?.InnerText;
                connectionInfo.RDGatewayUsername = gatewaySettingsNode.SelectSingleNode("./userName")?.InnerText;

                var passwordNode = gatewaySettingsNode.SelectSingleNode("./password");
                connectionInfo.RDGatewayPassword = passwordNode?.Attributes?["storeAsClearText"]?.Value == "True" ? passwordNode.InnerText : DecryptRdcManPassword(passwordNode?.InnerText);

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
            if (remoteDesktopNode?.Attributes?["inherit"]?.Value == "None")
            {
                var resolutionString = remoteDesktopNode.SelectSingleNode("./size")?.InnerText.Replace(" ", "");
                try
                {
                    connectionInfo.Resolution = (RdpProtocol.RDPResolutions)Enum.Parse(typeof(RdpProtocol.RDPResolutions), "Res" + resolutionString);
                }
                catch (ArgumentException)
                {
                    connectionInfo.Resolution = RdpProtocol.RDPResolutions.FitToWindow;
                }

                if (remoteDesktopNode.SelectSingleNode("./sameSizeAsClientArea")?.InnerText == "True")
                {
                    connectionInfo.Resolution = RdpProtocol.RDPResolutions.FitToWindow;
                }

                if (remoteDesktopNode.SelectSingleNode("./fullScreen")?.InnerText == "True")
                {
                    connectionInfo.Resolution = RdpProtocol.RDPResolutions.Fullscreen;
                }

                var colorDepth = remoteDesktopNode.SelectSingleNode("./colorDepth")?.InnerText;
                if (colorDepth != null)
                    connectionInfo.Colors = (RdpProtocol.RDPColors)Enum.Parse(typeof(RdpProtocol.RDPColors), colorDepth);
            }
            else
            {
                connectionInfo.Inheritance.Resolution = true;
                connectionInfo.Inheritance.Colors = true;
            }

            var localResourcesNode = xmlNode.SelectSingleNode("./localResources");
            if (localResourcesNode?.Attributes?["inherit"]?.Value == "None")
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (localResourcesNode.SelectSingleNode("./audioRedirection")?.InnerText)
                {
                    case "0": // Bring to this computer
                    case "Client":
                        connectionInfo.RedirectSound = RdpProtocol.RDPSounds.BringToThisComputer;
                        break;
                    case "1": // Leave at remote computer
                    case "Remote":
                        connectionInfo.RedirectSound = RdpProtocol.RDPSounds.LeaveAtRemoteComputer;
                        break;
                    case "2": // Do not play
                    case "NoSound":
                        connectionInfo.RedirectSound = RdpProtocol.RDPSounds.DoNotPlay;
                        break;
                }

                // ./audioRedirectionQuality
                // ./audioCaptureRedirection

                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (localResourcesNode.SelectSingleNode("./keyboardHook")?.InnerText)
                {
                    case "0": // On the local computer
                    case "Client":
                        connectionInfo.RedirectKeys = false;
                        break;
                    case "1": // On the remote computer
                    case "Remote":
                        connectionInfo.RedirectKeys = true;
                        break;
                    case "2": // In full screen mode only
                    case "FullScreenClient":
                        connectionInfo.RedirectKeys = false;
                        break;
                }

                // ./redirectClipboard
                connectionInfo.RedirectDiskDrives = bool.Parse(localResourcesNode?.SelectSingleNode("./redirectDrives")?.InnerText ?? "false");
                connectionInfo.RedirectPorts = bool.Parse(localResourcesNode?.SelectSingleNode("./redirectPorts")?.InnerText ?? "false");
                connectionInfo.RedirectPrinters = bool.Parse(localResourcesNode?.SelectSingleNode("./redirectPrinters")?.InnerText ?? "false");
                connectionInfo.RedirectSmartCards = bool.Parse(localResourcesNode?.SelectSingleNode("./redirectSmartCards")?.InnerText ?? "false");
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
            if (securitySettingsNode?.Attributes?["inherit"]?.Value == "None")
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (securitySettingsNode.SelectSingleNode("./authentication")?.InnerText)
                {
                    case "0": // No authentication
                    case "None":
                        connectionInfo.RDPAuthenticationLevel = RdpProtocol.AuthenticationLevel.NoAuth;
                        break;
                    case "1": // Do not connect if authentication fails
                    case "Required":
                        connectionInfo.RDPAuthenticationLevel = RdpProtocol.AuthenticationLevel.AuthRequired;
                        break;
                    case "2": // Warn if authentication fails
                    case "Warn":
                        connectionInfo.RDPAuthenticationLevel = RdpProtocol.AuthenticationLevel.WarnOnFailedAuth;
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

        private static string DecryptRdcManPassword(string ciphertext)
        {
            if (string.IsNullOrEmpty(ciphertext))
                return string.Empty;

            try
            {
                var plaintextData = ProtectedData.Unprotect(Convert.FromBase64String(ciphertext), new byte[] { }, DataProtectionScope.LocalMachine);
                var charArray = Encoding.Unicode.GetChars(plaintextData);
                return new string(charArray);
            }
            catch (Exception /*ex*/)
            {
                //Runtime.MessageCollector.AddExceptionMessage("RemoteDesktopConnectionManager.DecryptPassword() failed.", ex, logOnly: true);
                return string.Empty;
            }
        }
    }
}