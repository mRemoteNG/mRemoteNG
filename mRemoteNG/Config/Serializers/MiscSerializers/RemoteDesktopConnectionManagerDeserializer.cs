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
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;
using mRemoteNG.Security;

namespace mRemoteNG.Config.Serializers.MiscSerializers
{
    [SupportedOSPlatform("windows")]
    public class RemoteDesktopConnectionManagerDeserializer : IDeserializer<string, ConnectionTreeModel>
    {
        private static int _schemaVersion; /* 1 = RDCMan v2.2
                                       3 = RDCMan v2.7  */

        public ConnectionTreeModel Deserialize(string rdcmConnectionsXml)
        {
            ConnectionTreeModel connectionTreeModel = new();
            RootNodeInfo root = new(RootNodeType.Connection);

            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(rdcmConnectionsXml);


            XmlNode rdcManNode = xmlDocument.SelectSingleNode("/RDCMan");
            VerifySchemaVersion(rdcManNode);
            VerifyFileVersion(rdcManNode);

            XmlNode fileNode = rdcManNode?.SelectSingleNode("./file");
            ImportFileOrGroup(fileNode, root);

            connectionTreeModel.AddRootNode(root);
            return connectionTreeModel;
        }

        private static void VerifySchemaVersion(XmlNode rdcManNode)
        {
	        if (!int.TryParse(rdcManNode?.Attributes?["schemaVersion"]?.Value, out int version))
		        throw new FileFormatException("Could not find schema version attribute.");

            if (version != 1 && version != 3)
            {
                throw new FileFormatException($"Unsupported schema version ({version}).");
            }

            _schemaVersion = version;
        }

        private static void VerifyFileVersion(XmlNode rdcManNode)
        {
            string versionAttribute = rdcManNode?.Attributes?["programVersion"]?.Value;
            if (versionAttribute != null)
            {
                Version version = new(versionAttribute);
                if (!(version == new Version(2, 7)) && !(version == new Version(2, 83)))
                {
                    throw new FileFormatException($"Unsupported file version ({version}).");
                }
            }
            else
            {
                string versionNode = rdcManNode?.SelectSingleNode("./version")?.InnerText;
                if (versionNode != null)
                {
                    Version version = new(versionNode);
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
            ContainerInfo newContainer = ImportContainer(xmlNode, parentContainer);

            XmlNodeList childNodes = xmlNode.SelectNodes("./group|./server");
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

            ContainerInfo newContainer = new();
            ConnectionInfo connectionInfo = ConnectionInfoFromXml(containerPropertiesNode);
            newContainer.CopyFrom(connectionInfo);

            if (_schemaVersion == 3)
            {
                // Program Version 2.7 wraps these properties
                containerPropertiesNode = containerPropertiesNode.SelectSingleNode("./properties");
            }
            newContainer.Name = containerPropertiesNode?.SelectSingleNode("./name")?.InnerText ?? Language.NewFolder;
            if (bool.TryParse(containerPropertiesNode?.SelectSingleNode("./expanded")?.InnerText, out bool expanded))
				newContainer.IsExpanded = expanded;
            parentContainer.AddChild(newContainer);
            return newContainer;
        }

        private static void ImportServer(XmlNode serverNode, ContainerInfo parentContainer)
        {
            ConnectionInfo newConnectionInfo = ConnectionInfoFromXml(serverNode);
            parentContainer.AddChild(newConnectionInfo);
        }

        private static ConnectionInfo ConnectionInfoFromXml(XmlNode xmlNode)
        {
            ConnectionInfo connectionInfo = new() { Protocol = ProtocolType.RDP};

            XmlNode propertiesNode = xmlNode.SelectSingleNode("./properties");
            if (_schemaVersion == 1)
	            propertiesNode = xmlNode;  // Version 2.2 defines the container name at the root instead

            connectionInfo.VmId = propertiesNode?.SelectSingleNode("./vmid")?.InnerText ?? "";

            connectionInfo.Hostname = propertiesNode?.SelectSingleNode("./name")?.InnerText ?? "";

            string connectionDisplayName = propertiesNode?.SelectSingleNode("./displayName")?.InnerText;
			connectionInfo.Name = !string.IsNullOrWhiteSpace(connectionDisplayName)
                ? connectionDisplayName
	            : string.IsNullOrWhiteSpace(connectionInfo.Hostname)
	                ? connectionInfo.Name
	                : connectionInfo.Hostname;

            connectionInfo.Description = propertiesNode?.SelectSingleNode("./comment")?.InnerText ?? string.Empty;

            XmlNode logonCredentialsNode = xmlNode.SelectSingleNode("./logonCredentials");
            if (logonCredentialsNode?.Attributes?["inherit"]?.Value == "None")
            {
                connectionInfo.Username = logonCredentialsNode.SelectSingleNode("userName")?.InnerText ?? string.Empty;

                XmlNode passwordNode = logonCredentialsNode.SelectSingleNode("./password");
                if (_schemaVersion == 1) // Version 2.2 allows clear text passwords
                {
                    connectionInfo.Password = passwordNode?.Attributes?["storeAsClearText"]?.Value == "True"
                        ? passwordNode.InnerText.ConvertToSecureString()
                        : DecryptRdcManPassword(passwordNode?.InnerText).ConvertToSecureString();
                }
                else
                {
                    connectionInfo.Password = DecryptRdcManPassword(passwordNode?.InnerText).ConvertToSecureString();
                }

                connectionInfo.Domain = logonCredentialsNode.SelectSingleNode("./domain")?.InnerText ?? string.Empty;
            }
            else
            {
                connectionInfo.Inheritance.Username = true;
                connectionInfo.Inheritance.Password = true;
                connectionInfo.Inheritance.Domain = true;
            }

            XmlNode connectionSettingsNode = xmlNode.SelectSingleNode("./connectionSettings");
            if (connectionSettingsNode?.Attributes?["inherit"]?.Value == "None")
            {
				if (bool.TryParse(connectionSettingsNode.SelectSingleNode("./connectToConsole")?.InnerText, out bool useConsole))
					connectionInfo.UseConsoleSession = useConsole;
                connectionInfo.RDPStartProgram = connectionSettingsNode.SelectSingleNode("./startProgram")?.InnerText ?? string.Empty;
                connectionInfo.RDPStartProgramWorkDir = connectionSettingsNode.SelectSingleNode("./startProgramWorkDir")?.InnerText ?? string.Empty;
                if (int.TryParse(connectionSettingsNode.SelectSingleNode("./port")?.InnerText, out int port))
					connectionInfo.Port = port;
            }
            else
            {
                connectionInfo.Inheritance.UseConsoleSession = true;
                connectionInfo.Inheritance.Port = true;
            }

            XmlNode gatewaySettingsNode = xmlNode.SelectSingleNode("./gatewaySettings");
            if (gatewaySettingsNode?.Attributes?["inherit"]?.Value == "None")
            {
                connectionInfo.RDGatewayUsageMethod =
                    gatewaySettingsNode.SelectSingleNode("./enabled")?.InnerText == "True"
                        ? RDGatewayUsageMethod.Always
                        : RDGatewayUsageMethod.Never;
                connectionInfo.RDGatewayHostname = gatewaySettingsNode.SelectSingleNode("./hostName")?.InnerText ?? string.Empty;
                connectionInfo.RDGatewayUsername = gatewaySettingsNode.SelectSingleNode("./userName")?.InnerText ?? string.Empty;

                XmlNode passwordNode = gatewaySettingsNode.SelectSingleNode("./password");
                connectionInfo.RDGatewayPassword = passwordNode?.Attributes?["storeAsClearText"]?.Value == "True"
                    ? passwordNode.InnerText
                    : DecryptRdcManPassword(passwordNode?.InnerText);

                connectionInfo.RDGatewayDomain = gatewaySettingsNode.SelectSingleNode("./domain")?.InnerText ?? string.Empty;
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

            XmlNode remoteDesktopNode = xmlNode.SelectSingleNode("./remoteDesktop");
            if (remoteDesktopNode?.Attributes?["inherit"]?.Value == "None")
            {
                connectionInfo.Resolution = 
	                Enum.TryParse<RDPResolutions>(remoteDesktopNode.SelectSingleNode("./size")?.InnerText.Replace(" ", ""), true, out RDPResolutions rdpResolution)
	                ? rdpResolution
                    : RDPResolutions.FitToWindow;

                if (remoteDesktopNode.SelectSingleNode("./sameSizeAsClientArea")?.InnerText == "True")
                {
                    connectionInfo.Resolution = RDPResolutions.FitToWindow;
                }

                if (remoteDesktopNode.SelectSingleNode("./fullScreen")?.InnerText == "True")
                {
                    connectionInfo.Resolution = RDPResolutions.Fullscreen;
                }

                if (Enum.TryParse<RDPColors>(remoteDesktopNode.SelectSingleNode("./colorDepth")?.InnerText, true, out RDPColors rdpColors))
	                connectionInfo.Colors = rdpColors;
            }
            else
            {
                connectionInfo.Inheritance.Resolution = true;
                connectionInfo.Inheritance.Colors = true;
            }

            XmlNode localResourcesNode = xmlNode.SelectSingleNode("./localResources");
            if (localResourcesNode?.Attributes?["inherit"]?.Value == "None")
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (localResourcesNode.SelectSingleNode("./audioRedirection")?.InnerText)
                {
                    case "0": // Bring to this computer
                    case "Client":
                        connectionInfo.RedirectSound = RDPSounds.BringToThisComputer;
                        break;
                    case "1": // Leave at remote computer
                    case "Remote":
                        connectionInfo.RedirectSound = RDPSounds.LeaveAtRemoteComputer;
                        break;
                    case "2": // Do not play
                    case "NoSound":
                        connectionInfo.RedirectSound = RDPSounds.DoNotPlay;
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
                if (bool.TryParse(localResourcesNode?.SelectSingleNode("./redirectDrives")?.InnerText, out bool redirectDisks))
	                connectionInfo.RedirectDiskDrives = redirectDisks ? RDPDiskDrives.Local : RDPDiskDrives.None;

                if (bool.TryParse(localResourcesNode?.SelectSingleNode("./redirectPorts")?.InnerText, out bool redirectPorts))
	                connectionInfo.RedirectPorts = redirectPorts;

                if (bool.TryParse(localResourcesNode?.SelectSingleNode("./redirectPrinters")?.InnerText, out bool redirectPrinters))
	                connectionInfo.RedirectPrinters = redirectPrinters;

                if (bool.TryParse(localResourcesNode?.SelectSingleNode("./redirectSmartCards")?.InnerText, out bool redirectSmartCards))
	                connectionInfo.RedirectSmartCards = redirectSmartCards;
					
				if (bool.TryParse(localResourcesNode?.SelectSingleNode("./redirectClipboard")?.InnerText, out bool redirectClipboard))
					connectionInfo.RedirectClipboard = redirectClipboard;
            }
            else
            {
                connectionInfo.Inheritance.RedirectSound = true;
                connectionInfo.Inheritance.RedirectKeys = true;
                connectionInfo.Inheritance.RedirectDiskDrives = true;
                connectionInfo.Inheritance.RedirectPorts = true;
                connectionInfo.Inheritance.RedirectPrinters = true;
                connectionInfo.Inheritance.RedirectSmartCards = true;
                connectionInfo.Inheritance.RedirectClipboard = true;
            }

            XmlNode securitySettingsNode = xmlNode.SelectSingleNode("./securitySettings");
            if (securitySettingsNode?.Attributes?["inherit"]?.Value == "None")
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (securitySettingsNode.SelectSingleNode("./authentication")?.InnerText)
                {
                    case "0": // No authentication
                    case "None":
                        connectionInfo.RDPAuthenticationLevel = AuthenticationLevel.NoAuth;
                        break;
                    case "1": // Do not connect if authentication fails
                    case "Required":
                        connectionInfo.RDPAuthenticationLevel = AuthenticationLevel.AuthRequired;
                        break;
                    case "2": // Warn if authentication fails
                    case "Warn":
                        connectionInfo.RDPAuthenticationLevel = AuthenticationLevel.WarnOnFailedAuth;
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
                byte[] plaintextData = ProtectedData.Unprotect(Convert.FromBase64String(ciphertext), new byte[] { },
                                                            DataProtectionScope.LocalMachine);
                char[] charArray = Encoding.Unicode.GetChars(plaintextData);
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