using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Container;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using mRemoteNG.Connection;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using mRemoteNG.Tools;

namespace mRemoteNG.Config.Serializers
{
    public class RemoteDesktopConnectionManagerDeserializer
    {
        // 1 = RDCMan v2.2
        // 3 = RDCMan v2.7
        private static int _schemaVersion; 

        public SerializationResult Deserialize(string rdcmConnectionsXml)
        {
            var serializationResult = new SerializationResult(new List<ConnectionInfo>(), new ConnectionToCredentialMap());

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(rdcmConnectionsXml);

            var rdcManNode = xmlDocument.SelectSingleNode("/RDCMan");
            VerifySchemaVersion(rdcManNode);
            VerifyFileVersion(rdcManNode);

            var fileNode = rdcManNode?.SelectSingleNode("./file");
            var importedItem = ImportFileOrGroup(fileNode, serializationResult.ConnectionToCredentialMap);

            serializationResult.ConnectionRecords.Add(importedItem);

            return serializationResult;
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

        private static ContainerInfo ImportFileOrGroup(XmlNode xmlNode, ConnectionToCredentialMap credentialMap)
        {
            var newContainer = ImportContainer(xmlNode);

            var childNodes = xmlNode.SelectNodes("./group|./server");
            if (childNodes == null)
                return newContainer;

            foreach (XmlNode childNode in childNodes)
            {
                ConnectionInfo newChild = null;

                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (childNode.Name)
                {
                    case "group":
                        newChild = ImportFileOrGroup(childNode, credentialMap);
                        break;
                    case "server":
                        newChild = ConnectionInfoFromXml(childNode);
                        break;
                }

                if (newChild == null)
                    return newContainer;

                newContainer.AddChild(newChild);

                var cred = ParseCredentials(childNode);
                if (!cred.Any())
                    continue;

                newChild.CredentialRecordId = cred.First().Id;
                credentialMap.Add(Guid.Parse(newChild.ConstantID), cred.First());
            }

            return newContainer;
        }

        private static ContainerInfo ImportContainer(XmlNode containerPropertiesNode)
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
            newContainer.IsExpanded =
                bool.Parse(containerPropertiesNode?.SelectSingleNode("./expanded")?.InnerText ?? "false");
            return newContainer;
        }

        private static ConnectionInfo ConnectionInfoFromXml(XmlNode xmlNode)
        {
            var connectionInfo = new ConnectionInfo {Protocol = ProtocolType.RDP};


            var propertiesNode = xmlNode.SelectSingleNode("./properties");
            if (_schemaVersion == 1)
                propertiesNode = xmlNode; // Version 2.2 defines the container name at the root instead
            connectionInfo.Hostname = propertiesNode?.SelectSingleNode("./name")?.InnerText ?? "";
            connectionInfo.Name =
                propertiesNode?.SelectSingleNode("./displayName")?.InnerText ?? connectionInfo.Hostname;
            connectionInfo.Description = propertiesNode?.SelectSingleNode("./comment")?.InnerText ?? string.Empty;

            var logonCredentialsNode = xmlNode.SelectSingleNode("./logonCredentials");
            if (logonCredentialsNode?.Attributes?["inherit"]?.Value != "None")
            {
                connectionInfo.Inheritance.Username = true;
                connectionInfo.Inheritance.Password = true;
                connectionInfo.Inheritance.Domain = true;
            }

            var connectionSettingsNode = xmlNode.SelectSingleNode("./connectionSettings");
            if (connectionSettingsNode?.Attributes?["inherit"]?.Value == "None")
            {
                connectionInfo.UseConsoleSession =
                    bool.Parse(connectionSettingsNode.SelectSingleNode("./connectToConsole")?.InnerText ?? "false");
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
                connectionInfo.RDGatewayUsageMethod =
                    gatewaySettingsNode.SelectSingleNode("./enabled")?.InnerText == "True"
                        ? RdpProtocol.RDGatewayUsageMethod.Always
                        : RdpProtocol.RDGatewayUsageMethod.Never;
                connectionInfo.RDGatewayHostname = gatewaySettingsNode.SelectSingleNode("./hostName")?.InnerText;
                connectionInfo.RDGatewayUsername = gatewaySettingsNode.SelectSingleNode("./userName")?.InnerText;

                var passwordNode = gatewaySettingsNode.SelectSingleNode("./password");
                connectionInfo.RDGatewayPassword = passwordNode?.Attributes?["storeAsClearText"]?.Value == "True" 
                    ? passwordNode.InnerText 
                    : DecryptRdcManPassword(passwordNode?.InnerText).ConvertToUnsecureString();

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
                    connectionInfo.Resolution = Enum.TryParse<RdpProtocol.RDPResolutions>("Res" + resolutionString, true, out var resolution)
                        ? resolution
                        : RdpProtocol.RDPResolutions.FitToWindow;
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
                    connectionInfo.Colors =
                        (RdpProtocol.RDPColors)Enum.Parse(typeof(RdpProtocol.RDPColors), colorDepth);
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
                connectionInfo.RedirectDiskDrives =
                    bool.Parse(localResourcesNode?.SelectSingleNode("./redirectDrives")?.InnerText ?? "false");
                connectionInfo.RedirectPorts =
                    bool.Parse(localResourcesNode?.SelectSingleNode("./redirectPorts")?.InnerText ?? "false");
                connectionInfo.RedirectPrinters =
                    bool.Parse(localResourcesNode?.SelectSingleNode("./redirectPrinters")?.InnerText ?? "false");
                connectionInfo.RedirectSmartCards =
                    bool.Parse(localResourcesNode?.SelectSingleNode("./redirectSmartCards")?.InnerText ?? "false");
                connectionInfo.RedirectClipboard =
                    bool.Parse(localResourcesNode?.SelectSingleNode("./redirectClipboard")?.InnerText ?? "false");
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

        private static Optional<ICredentialRecord> ParseCredentials(XmlNode xmlNode)
        {
            var logonCredentialsNode = xmlNode.SelectSingleNode("./logonCredentials");

            if (logonCredentialsNode?.Attributes?["inherit"]?.Value != "None")
                return Optional<ICredentialRecord>.Empty;

            var username = logonCredentialsNode.SelectSingleNode("userName")?.InnerText ?? "";
            var domain = logonCredentialsNode.SelectSingleNode("./domain")?.InnerText ?? "";
            var passwordNode = logonCredentialsNode.SelectSingleNode("./password");

            var creds = new CredentialRecord
            {
                Title = domain.Length > 0 ? $"{domain}\\{username}" : $"{username}",
                Username = username,
                Domain = domain,
                Password = passwordNode?.Attributes?["storeAsClearText"]?.Value == "True"
                    ? passwordNode.InnerText.ConvertToSecureString()
                    : DecryptRdcManPassword(passwordNode?.InnerText)
            };

            return creds;
        }

        private static SecureString DecryptRdcManPassword(string ciphertext)
        {
            if (string.IsNullOrEmpty(ciphertext))
                return new SecureString();

            try
            {
                var plaintextData = ProtectedData.Unprotect(Convert.FromBase64String(ciphertext), new byte[] { },
                                                            DataProtectionScope.LocalMachine);
                return Encoding.Unicode.GetString(plaintextData).ConvertToSecureString();
            }
            catch (Exception /*ex*/)
            {
                //Runtime.MessageCollector.AddExceptionMessage("RemoteDesktopConnectionManager.DecryptPassword() failed.", ex, logOnly: true);
                return new SecureString();
            }
        }
    }
}