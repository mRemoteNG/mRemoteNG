using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

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
                throw (new FileFormatException($"Unsupported schema version ({schemaVersion})."));
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

            Runtime.ContainerList.Add(newContainer);
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

            Runtime.ConnectionList.Add(newConnectionInfo);
        }

        private ConnectionInfo ConnectionInfoFromXml(XmlNode xmlNode)
        {
            var connectionInfo = new ConnectionInfo {Protocol = ProtocolType.RDP};

            var name = xmlNode.SelectSingleNode("./name")?.InnerText;

            var displayNameNode = xmlNode.SelectSingleNode("./displayName");
            var displayName = displayNameNode?.InnerText ?? Language.strNewConnection;

            connectionInfo.Name = displayName;
            connectionInfo.Description = xmlNode.SelectSingleNode("./comment")?.InnerText;
            connectionInfo.Hostname = name;

            var logonCredentialsNode = xmlNode.SelectSingleNode("./logonCredentials");
            if (logonCredentialsNode?.Attributes?["inherit"].Value == "None")
            {
                connectionInfo.Username = logonCredentialsNode.SelectSingleNode("userName")?.InnerText;

                var passwordNode = logonCredentialsNode.SelectSingleNode("./password");
                connectionInfo.Password = passwordNode?.Attributes?["storeAsClearText"].Value == "True" ? passwordNode.InnerText : DecryptPassword(passwordNode?.InnerText);

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
                connectionInfo.UseConsoleSession = bool.Parse(connectionSettingsNode.SelectSingleNode("./connectToConsole")?.InnerText);
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

                var passwordNode = logonCredentialsNode.SelectSingleNode("./password");
                if (passwordNode?.Attributes?["storeAsClearText"].Value == "True")
                {
                    connectionInfo.RDGatewayPassword = passwordNode.InnerText;
                }
                else
                {
                    connectionInfo.Password = DecryptPassword(passwordNode.InnerText);
                }

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
                var resolutionString = Convert.ToString(remoteDesktopNode.SelectSingleNode("./size")?.InnerText.Replace(" ", ""));
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


                connectionInfo.Colors = (ProtocolRDP.RDPColors)Enum.Parse(typeof(ProtocolRDP.RDPColors), remoteDesktopNode.SelectSingleNode("./colorDepth").InnerText);
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
                connectionInfo.RedirectDiskDrives = bool.Parse(localResourcesNode.SelectSingleNode("./redirectDrives").InnerText);
                connectionInfo.RedirectPorts = bool.Parse(localResourcesNode.SelectSingleNode("./redirectPorts").InnerText);
                connectionInfo.RedirectPrinters = bool.Parse(localResourcesNode.SelectSingleNode("./redirectPrinters").InnerText);
                connectionInfo.RedirectSmartCards = bool.Parse(localResourcesNode.SelectSingleNode("./redirectSmartCards").InnerText);
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

        private string DecryptPassword(string ciphertext)
        {
            if (string.IsNullOrEmpty(ciphertext))
            {
                return null;
            }

            var gcHandle = new GCHandle();
            var plaintextData = new NativeMethods.DATA_BLOB();
            try
            {
                var ciphertextArray = Convert.FromBase64String(ciphertext);
                gcHandle = GCHandle.Alloc(ciphertextArray, GCHandleType.Pinned);

                var ciphertextData = new NativeMethods.DATA_BLOB
                {
                    cbData = ciphertextArray.Length,
                    pbData = gcHandle.AddrOfPinnedObject()
                };

                var tempOptionalEntropy = new NativeMethods.DATA_BLOB();
                var tempPromptStruct = IntPtr.Zero;
                if (!NativeMethods.CryptUnprotectData(ref ciphertextData, null, ref tempOptionalEntropy, IntPtr.Zero, ref tempPromptStruct, 0, ref plaintextData))
                {
                    return null;
                }

                var plaintextLength = (int)((double)plaintextData.cbData / 2); // Char = 2 bytes
                var plaintextArray = new char[plaintextLength - 1 + 1];
                Marshal.Copy(plaintextData.pbData, plaintextArray, 0, plaintextLength);

                return new string(plaintextArray);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("RemoteDesktopConnectionManager.DecryptPassword() failed.", ex, logOnly: true);
                return null;
            }
            finally
            {
                if (gcHandle.IsAllocated)
                {
                    gcHandle.Free();
                }
                if (!(plaintextData.pbData == IntPtr.Zero))
                {
                    NativeMethods.LocalFree(plaintextData.pbData);
                }
            }
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class NativeMethods
        {
            // ReSharper disable InconsistentNaming
            // ReSharper disable IdentifierTypo
            // ReSharper disable StringLiteralTypo
            [DllImport("crypt32.dll", CharSet = CharSet.Unicode)]
            public static extern bool CryptUnprotectData(ref DATA_BLOB dataIn, string description, ref DATA_BLOB optionalEntropy, IntPtr reserved, ref IntPtr promptStruct, int flags, ref DATA_BLOB dataOut);

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
            public static extern void LocalFree(IntPtr ptr);

            public struct DATA_BLOB
            {
                public int cbData;
                public IntPtr pbData;
            }
            // ReSharper restore StringLiteralTypo
            // ReSharper restore IdentifierTypo
            // ReSharper restore InconsistentNaming
        }
    }
}