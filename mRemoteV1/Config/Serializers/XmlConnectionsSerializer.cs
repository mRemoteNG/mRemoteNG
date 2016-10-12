using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Xml;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers
{
    public class XmlConnectionsSerializer : ISerializer<string>
    {
        private SecureString _password = Runtime.EncryptionKey;
        private XmlTextWriter _xmlTextWriter;
        private ICryptographyProvider _cryptographyProvider;

        public bool Export { get; set; }
        public SaveFilter SaveFilter { get; set; } = new SaveFilter();
        public bool UseFullEncryption { get; set; }


        public string Serialize(ConnectionTreeModel connectionTreeModel)
        {
            var rootNode = (RootNodeInfo)connectionTreeModel.RootNodes.First(node => node is RootNodeInfo);
            return Serialize(rootNode);
        }

        public string Serialize(ConnectionInfo serializationTarget)
        {
            var factory = new CryptographyProviderFactory();
            _cryptographyProvider = factory.CreateAeadCryptographyProvider(mRemoteNG.Settings.Default.EncryptionEngine, mRemoteNG.Settings.Default.EncryptionBlockCipherMode);
            return SerializeConnectionsData(serializationTarget);
        }

        private string SerializeConnectionsData(ConnectionInfo serializationTarget)
        {
            var xml = "";
            try
            {
                var memoryStream = new MemoryStream();
                using (_xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8))
                {
                    SetXmlTextWriterSettings();
                    _xmlTextWriter.WriteStartDocument();
                    SerializeRootNodeInfo(GetRootNodeFromConnectionInfo(serializationTarget));
                    SaveNodesRecursive(serializationTarget);
                    _xmlTextWriter.WriteEndElement();
                    _xmlTextWriter.WriteEndDocument();
                    _xmlTextWriter.Flush();

                    var streamReader = new StreamReader(memoryStream, Encoding.UTF8, true);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    xml = streamReader.ReadToEnd();
                    if (UseFullEncryption)
                        xml = EncyrptFullFile(xml);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("SaveToXml failed", ex);
            }
            return xml;
        }

        private RootNodeInfo GetRootNodeFromConnectionInfo(ConnectionInfo connectionInfo)
        {
            while (true)
            {
                var connectionInfoAsRootNode = connectionInfo as RootNodeInfo;
                if (connectionInfoAsRootNode != null) return connectionInfoAsRootNode;
                connectionInfo = connectionInfo.Parent;
            }
        }

        private void SetXmlTextWriterSettings()
        {
            _xmlTextWriter.Formatting = Formatting.Indented;
            _xmlTextWriter.Indentation = 4;
        }

        private string EncyrptFullFile(string xml)
        {
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml(xml);
            if (xmldoc.DocumentElement == null) return xml;
            var plainTextContent = xmldoc.DocumentElement.InnerXml;
            var encryptedContent = _cryptographyProvider.Encrypt(plainTextContent, _password);
            xmldoc.DocumentElement.InnerXml = encryptedContent;
            var xmlString = WriteXmlToString(xmldoc);
            return xmlString;
        }

        private string WriteXmlToString(XmlDocument xmlDocument)
        {
            string xmlString;
            var xmlWriterSettings = new XmlWriterSettings {Indent = true, IndentChars = "    ", Encoding = Encoding.UTF8};
            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
            {
                xmlDocument.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                xmlString = stringWriter.GetStringBuilder().ToString();
            }
            return xmlString;
        }

        private void SaveNodesRecursive(ConnectionInfo node)
        {
            try
            {
                var nodeAsRoot = node as RootNodeInfo;
                var nodeAsContainer = node as ContainerInfo;
                if (nodeAsRoot != null)
                {
                    foreach (var child in nodeAsRoot.Children)
                        SaveNodesRecursive(child);
                }
                else if (nodeAsContainer != null)
                {
                    SerializeContainerInfo(nodeAsContainer);
                    foreach (var child in nodeAsContainer.Children)
                        SaveNodesRecursive(child);
                }
                else
                {
                    SerializeConnectionInfo(node);
                }

                if (nodeAsRoot == null)
                    _xmlTextWriter.WriteEndElement();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "SaveNode failed" + Environment.NewLine + ex.Message, true);
            }
        }

        private void SerializeRootNodeInfo(RootNodeInfo rootNodeInfo)
        {
            _xmlTextWriter.WriteStartElement("Connections"); // Do not localize
            _xmlTextWriter.WriteAttributeString("Name", "", rootNodeInfo.Name);
            _xmlTextWriter.WriteAttributeString("Export", "", Convert.ToString(Export));
            var cipherEngine = Enum.GetName(typeof(BlockCipherEngines), mRemoteNG.Settings.Default.EncryptionEngine);
            _xmlTextWriter.WriteAttributeString("EncryptionEngine", "", cipherEngine ?? "");
            var cipherMode = Enum.GetName(typeof(BlockCipherModes), mRemoteNG.Settings.Default.EncryptionBlockCipherMode);
            _xmlTextWriter.WriteAttributeString("BlockCipherMode", "", cipherMode ?? "");
            _xmlTextWriter.WriteAttributeString("FullFileEncryption", "", UseFullEncryption.ToString());

            if (Export)
            {
                _xmlTextWriter.WriteAttributeString("Protected", "", _cryptographyProvider.Encrypt("ThisIsNotProtected", _password));
            }
            else
            {
                if (rootNodeInfo.Password)
                {
                    _password = rootNodeInfo.PasswordString.ConvertToSecureString();
                    _xmlTextWriter.WriteAttributeString("Protected", "", _cryptographyProvider.Encrypt("ThisIsProtected", _password));
                }
                else
                {
                    _xmlTextWriter.WriteAttributeString("Protected", "", _cryptographyProvider.Encrypt("ThisIsNotProtected", _password));
                }
            }

            _xmlTextWriter.WriteAttributeString("ConfVersion", "", ConnectionsFileInfo.ConnectionFileVersion.ToString(CultureInfo.InvariantCulture));
        }

        private void SerializeContainerInfo(ContainerInfo containerInfo)
        {
            _xmlTextWriter.WriteStartElement("Node");
            _xmlTextWriter.WriteAttributeString("Name", "", containerInfo.Name);
            _xmlTextWriter.WriteAttributeString("Type", "", containerInfo.GetTreeNodeType().ToString());
            _xmlTextWriter.WriteAttributeString("Expanded", "", containerInfo.IsExpanded.ToString());
            SaveConnectionFields(containerInfo);
        }

        private void SerializeConnectionInfo(ConnectionInfo connectionInfo)
        {
            _xmlTextWriter.WriteStartElement("Node");
            _xmlTextWriter.WriteAttributeString("Name", "", connectionInfo.Name);
            _xmlTextWriter.WriteAttributeString("Type", "", connectionInfo.GetTreeNodeType().ToString());
            SaveConnectionFields(connectionInfo);
        }

        private void SaveConnectionFields(ConnectionInfo connectionInfo)
        {
            try
            {
                _xmlTextWriter.WriteAttributeString("Descr", "", connectionInfo.Description);
                _xmlTextWriter.WriteAttributeString("Icon", "", connectionInfo.Icon);
                _xmlTextWriter.WriteAttributeString("Panel", "", connectionInfo.Panel);

                if (SaveFilter.SaveUsername)
                    _xmlTextWriter.WriteAttributeString("Username", "", connectionInfo.Username);
                else
                    _xmlTextWriter.WriteAttributeString("Username", "", "");

                if (SaveFilter.SaveDomain)
                    _xmlTextWriter.WriteAttributeString("Domain", "", connectionInfo.Domain);
                else
                    _xmlTextWriter.WriteAttributeString("Domain", "", "");

                if (SaveFilter.SavePassword)
                    _xmlTextWriter.WriteAttributeString("Password", "", _cryptographyProvider.Encrypt(connectionInfo.Password, _password));
                else
                    _xmlTextWriter.WriteAttributeString("Password", "", "");

                _xmlTextWriter.WriteAttributeString("Hostname", "", connectionInfo.Hostname);
                _xmlTextWriter.WriteAttributeString("Protocol", "", connectionInfo.Protocol.ToString());
                _xmlTextWriter.WriteAttributeString("PuttySession", "", connectionInfo.PuttySession);
                _xmlTextWriter.WriteAttributeString("Port", "", Convert.ToString(connectionInfo.Port));
                _xmlTextWriter.WriteAttributeString("ConnectToConsole", "", Convert.ToString(connectionInfo.UseConsoleSession));
                _xmlTextWriter.WriteAttributeString("UseCredSsp", "", Convert.ToString(connectionInfo.UseCredSsp));
                _xmlTextWriter.WriteAttributeString("RenderingEngine", "", connectionInfo.RenderingEngine.ToString());
                _xmlTextWriter.WriteAttributeString("ICAEncryptionStrength", "", connectionInfo.ICAEncryptionStrength.ToString());
                _xmlTextWriter.WriteAttributeString("RDPAuthenticationLevel", "", connectionInfo.RDPAuthenticationLevel.ToString());
                _xmlTextWriter.WriteAttributeString("LoadBalanceInfo", "", connectionInfo.LoadBalanceInfo);
                _xmlTextWriter.WriteAttributeString("Colors", "", connectionInfo.Colors.ToString());
                _xmlTextWriter.WriteAttributeString("Resolution", "", connectionInfo.Resolution.ToString());
                _xmlTextWriter.WriteAttributeString("AutomaticResize", "", Convert.ToString(connectionInfo.AutomaticResize));
                _xmlTextWriter.WriteAttributeString("DisplayWallpaper", "", Convert.ToString(connectionInfo.DisplayWallpaper));
                _xmlTextWriter.WriteAttributeString("DisplayThemes", "", Convert.ToString(connectionInfo.DisplayThemes));
                _xmlTextWriter.WriteAttributeString("EnableFontSmoothing", "", Convert.ToString(connectionInfo.EnableFontSmoothing));
                _xmlTextWriter.WriteAttributeString("EnableDesktopComposition", "", Convert.ToString(connectionInfo.EnableDesktopComposition));
                _xmlTextWriter.WriteAttributeString("CacheBitmaps", "", Convert.ToString(connectionInfo.CacheBitmaps));
                _xmlTextWriter.WriteAttributeString("RedirectDiskDrives", "", Convert.ToString(connectionInfo.RedirectDiskDrives));
                _xmlTextWriter.WriteAttributeString("RedirectPorts", "", Convert.ToString(connectionInfo.RedirectPorts));
                _xmlTextWriter.WriteAttributeString("RedirectPrinters", "", Convert.ToString(connectionInfo.RedirectPrinters));
                _xmlTextWriter.WriteAttributeString("RedirectSmartCards", "", Convert.ToString(connectionInfo.RedirectSmartCards));
                _xmlTextWriter.WriteAttributeString("RedirectSound", "", connectionInfo.RedirectSound.ToString());
                _xmlTextWriter.WriteAttributeString("RedirectKeys", "", Convert.ToString(connectionInfo.RedirectKeys));

                if (connectionInfo.OpenConnections.Count > 0)
                    _xmlTextWriter.WriteAttributeString("Connected", "", Convert.ToString(true));
                else
                    _xmlTextWriter.WriteAttributeString("Connected", "", Convert.ToString(false));

                _xmlTextWriter.WriteAttributeString("PreExtApp", "", connectionInfo.PreExtApp);
                _xmlTextWriter.WriteAttributeString("PostExtApp", "", connectionInfo.PostExtApp);
                _xmlTextWriter.WriteAttributeString("MacAddress", "", connectionInfo.MacAddress);
                _xmlTextWriter.WriteAttributeString("UserField", "", connectionInfo.UserField);
                _xmlTextWriter.WriteAttributeString("ExtApp", "", connectionInfo.ExtApp);

                _xmlTextWriter.WriteAttributeString("VNCCompression", "", connectionInfo.VNCCompression.ToString());
                _xmlTextWriter.WriteAttributeString("VNCEncoding", "", connectionInfo.VNCEncoding.ToString());
                _xmlTextWriter.WriteAttributeString("VNCAuthMode", "", connectionInfo.VNCAuthMode.ToString());
                _xmlTextWriter.WriteAttributeString("VNCProxyType", "", connectionInfo.VNCProxyType.ToString());
                _xmlTextWriter.WriteAttributeString("VNCProxyIP", "", connectionInfo.VNCProxyIP);
                _xmlTextWriter.WriteAttributeString("VNCProxyPort", "", Convert.ToString(connectionInfo.VNCProxyPort));
                _xmlTextWriter.WriteAttributeString("VNCProxyUsername", "", connectionInfo.VNCProxyUsername);
                _xmlTextWriter.WriteAttributeString("VNCProxyPassword", "", _cryptographyProvider.Encrypt(connectionInfo.VNCProxyPassword, _password));
                _xmlTextWriter.WriteAttributeString("VNCColors", "", connectionInfo.VNCColors.ToString());
                _xmlTextWriter.WriteAttributeString("VNCSmartSizeMode", "", connectionInfo.VNCSmartSizeMode.ToString());
                _xmlTextWriter.WriteAttributeString("VNCViewOnly", "", Convert.ToString(connectionInfo.VNCViewOnly));

                _xmlTextWriter.WriteAttributeString("RDGatewayUsageMethod", "", connectionInfo.RDGatewayUsageMethod.ToString());
                _xmlTextWriter.WriteAttributeString("RDGatewayHostname", "", connectionInfo.RDGatewayHostname);
                _xmlTextWriter.WriteAttributeString("RDGatewayUseConnectionCredentials", "", connectionInfo.RDGatewayUseConnectionCredentials.ToString());

                if (SaveFilter.SaveUsername)
                    _xmlTextWriter.WriteAttributeString("RDGatewayUsername", "", connectionInfo.RDGatewayUsername);
                else
                    _xmlTextWriter.WriteAttributeString("RDGatewayUsername", "", "");

                if (SaveFilter.SavePassword)
                    _xmlTextWriter.WriteAttributeString("RDGatewayPassword", "", _cryptographyProvider.Encrypt(connectionInfo.RDGatewayPassword, _password));
                else
                    _xmlTextWriter.WriteAttributeString("RDGatewayPassword", "", "");

                if (SaveFilter.SaveDomain)
                    _xmlTextWriter.WriteAttributeString("RDGatewayDomain", "", connectionInfo.RDGatewayDomain);
                else
                    _xmlTextWriter.WriteAttributeString("RDGatewayDomain", "", "");

                if (SaveFilter.SaveInheritance)
                {
                    _xmlTextWriter.WriteAttributeString("InheritCacheBitmaps", "", Convert.ToString(connectionInfo.Inheritance.CacheBitmaps));
                    _xmlTextWriter.WriteAttributeString("InheritColors", "", Convert.ToString(connectionInfo.Inheritance.Colors));
                    _xmlTextWriter.WriteAttributeString("InheritDescription", "", Convert.ToString(connectionInfo.Inheritance.Description));
                    _xmlTextWriter.WriteAttributeString("InheritDisplayThemes", "", Convert.ToString(connectionInfo.Inheritance.DisplayThemes));
                    _xmlTextWriter.WriteAttributeString("InheritDisplayWallpaper", "", Convert.ToString(connectionInfo.Inheritance.DisplayWallpaper));
                    _xmlTextWriter.WriteAttributeString("InheritEnableFontSmoothing", "", Convert.ToString(connectionInfo.Inheritance.EnableFontSmoothing));
                    _xmlTextWriter.WriteAttributeString("InheritEnableDesktopComposition", "", Convert.ToString(connectionInfo.Inheritance.EnableDesktopComposition));
                    _xmlTextWriter.WriteAttributeString("InheritDomain", "", Convert.ToString(connectionInfo.Inheritance.Domain));
                    _xmlTextWriter.WriteAttributeString("InheritIcon", "", Convert.ToString(connectionInfo.Inheritance.Icon));
                    _xmlTextWriter.WriteAttributeString("InheritPanel", "", Convert.ToString(connectionInfo.Inheritance.Panel));
                    _xmlTextWriter.WriteAttributeString("InheritPassword", "", Convert.ToString(connectionInfo.Inheritance.Password));
                    _xmlTextWriter.WriteAttributeString("InheritPort", "", Convert.ToString(connectionInfo.Inheritance.Port));
                    _xmlTextWriter.WriteAttributeString("InheritProtocol", "", Convert.ToString(connectionInfo.Inheritance.Protocol));
                    _xmlTextWriter.WriteAttributeString("InheritPuttySession", "", Convert.ToString(connectionInfo.Inheritance.PuttySession));
                    _xmlTextWriter.WriteAttributeString("InheritRedirectDiskDrives", "", Convert.ToString(connectionInfo.Inheritance.RedirectDiskDrives));
                    _xmlTextWriter.WriteAttributeString("InheritRedirectKeys", "", Convert.ToString(connectionInfo.Inheritance.RedirectKeys));
                    _xmlTextWriter.WriteAttributeString("InheritRedirectPorts", "", Convert.ToString(connectionInfo.Inheritance.RedirectPorts));
                    _xmlTextWriter.WriteAttributeString("InheritRedirectPrinters", "", Convert.ToString(connectionInfo.Inheritance.RedirectPrinters));
                    _xmlTextWriter.WriteAttributeString("InheritRedirectSmartCards", "", Convert.ToString(connectionInfo.Inheritance.RedirectSmartCards));
                    _xmlTextWriter.WriteAttributeString("InheritRedirectSound", "", Convert.ToString(connectionInfo.Inheritance.RedirectSound));
                    _xmlTextWriter.WriteAttributeString("InheritResolution", "", Convert.ToString(connectionInfo.Inheritance.Resolution));
                    _xmlTextWriter.WriteAttributeString("InheritAutomaticResize", "", Convert.ToString(connectionInfo.Inheritance.AutomaticResize));
                    _xmlTextWriter.WriteAttributeString("InheritUseConsoleSession", "", Convert.ToString(connectionInfo.Inheritance.UseConsoleSession));
                    _xmlTextWriter.WriteAttributeString("InheritUseCredSsp", "", Convert.ToString(connectionInfo.Inheritance.UseCredSsp));
                    _xmlTextWriter.WriteAttributeString("InheritRenderingEngine", "", Convert.ToString(connectionInfo.Inheritance.RenderingEngine));
                    _xmlTextWriter.WriteAttributeString("InheritUsername", "", Convert.ToString(connectionInfo.Inheritance.Username));
                    _xmlTextWriter.WriteAttributeString("InheritICAEncryptionStrength", "", Convert.ToString(connectionInfo.Inheritance.ICAEncryptionStrength));
                    _xmlTextWriter.WriteAttributeString("InheritRDPAuthenticationLevel", "", Convert.ToString(connectionInfo.Inheritance.RDPAuthenticationLevel));
                    _xmlTextWriter.WriteAttributeString("InheritLoadBalanceInfo", "", Convert.ToString(connectionInfo.Inheritance.LoadBalanceInfo));
                    _xmlTextWriter.WriteAttributeString("InheritPreExtApp", "", Convert.ToString(connectionInfo.Inheritance.PreExtApp));
                    _xmlTextWriter.WriteAttributeString("InheritPostExtApp", "", Convert.ToString(connectionInfo.Inheritance.PostExtApp));
                    _xmlTextWriter.WriteAttributeString("InheritMacAddress", "", Convert.ToString(connectionInfo.Inheritance.MacAddress));
                    _xmlTextWriter.WriteAttributeString("InheritUserField", "", Convert.ToString(connectionInfo.Inheritance.UserField));
                    _xmlTextWriter.WriteAttributeString("InheritExtApp", "", Convert.ToString(connectionInfo.Inheritance.ExtApp));
                    _xmlTextWriter.WriteAttributeString("InheritVNCCompression", "", Convert.ToString(connectionInfo.Inheritance.VNCCompression));
                    _xmlTextWriter.WriteAttributeString("InheritVNCEncoding", "", Convert.ToString(connectionInfo.Inheritance.VNCEncoding));
                    _xmlTextWriter.WriteAttributeString("InheritVNCAuthMode", "", Convert.ToString(connectionInfo.Inheritance.VNCAuthMode));
                    _xmlTextWriter.WriteAttributeString("InheritVNCProxyType", "", Convert.ToString(connectionInfo.Inheritance.VNCProxyType));
                    _xmlTextWriter.WriteAttributeString("InheritVNCProxyIP", "", Convert.ToString(connectionInfo.Inheritance.VNCProxyIP));
                    _xmlTextWriter.WriteAttributeString("InheritVNCProxyPort", "", Convert.ToString(connectionInfo.Inheritance.VNCProxyPort));
                    _xmlTextWriter.WriteAttributeString("InheritVNCProxyUsername", "", Convert.ToString(connectionInfo.Inheritance.VNCProxyUsername));
                    _xmlTextWriter.WriteAttributeString("InheritVNCProxyPassword", "", Convert.ToString(connectionInfo.Inheritance.VNCProxyPassword));
                    _xmlTextWriter.WriteAttributeString("InheritVNCColors", "", Convert.ToString(connectionInfo.Inheritance.VNCColors));
                    _xmlTextWriter.WriteAttributeString("InheritVNCSmartSizeMode", "", Convert.ToString(connectionInfo.Inheritance.VNCSmartSizeMode));
                    _xmlTextWriter.WriteAttributeString("InheritVNCViewOnly", "", Convert.ToString(connectionInfo.Inheritance.VNCViewOnly));
                    _xmlTextWriter.WriteAttributeString("InheritRDGatewayUsageMethod", "", Convert.ToString(connectionInfo.Inheritance.RDGatewayUsageMethod));
                    _xmlTextWriter.WriteAttributeString("InheritRDGatewayHostname", "", Convert.ToString(connectionInfo.Inheritance.RDGatewayHostname));
                    _xmlTextWriter.WriteAttributeString("InheritRDGatewayUseConnectionCredentials", "", Convert.ToString(connectionInfo.Inheritance.RDGatewayUseConnectionCredentials));
                    _xmlTextWriter.WriteAttributeString("InheritRDGatewayUsername", "", Convert.ToString(connectionInfo.Inheritance.RDGatewayUsername));
                    _xmlTextWriter.WriteAttributeString("InheritRDGatewayPassword", "", Convert.ToString(connectionInfo.Inheritance.RDGatewayPassword));
                    _xmlTextWriter.WriteAttributeString("InheritRDGatewayDomain", "", Convert.ToString(connectionInfo.Inheritance.RDGatewayDomain));
                }
                else
                {
                    _xmlTextWriter.WriteAttributeString("InheritCacheBitmaps", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritColors", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritDescription", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritDisplayThemes", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritDisplayWallpaper", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritEnableFontSmoothing", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritEnableDesktopComposition", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritDomain", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritIcon", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritPanel", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritPassword", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritPort", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritProtocol", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritPuttySession", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritRedirectDiskDrives", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritRedirectKeys", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritRedirectPorts", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritRedirectPrinters", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritRedirectSmartCards", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritRedirectSound", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritResolution", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritAutomaticResize", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritUseConsoleSession", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritUseCredSsp", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritRenderingEngine", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritUsername", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritICAEncryptionStrength", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritRDPAuthenticationLevel", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritLoadBalanceInfo", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritPreExtApp", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritPostExtApp", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritMacAddress", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritUserField", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritExtApp", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritVNCCompression", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritVNCEncoding", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritVNCAuthMode", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritVNCProxyType", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritVNCProxyIP", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritVNCProxyPort", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritVNCProxyUsername", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritVNCProxyPassword", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritVNCColors", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritVNCSmartSizeMode", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritVNCViewOnly", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritRDGatewayHostname", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritRDGatewayUseConnectionCredentials", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritRDGatewayUsername", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritRDGatewayPassword", "", Convert.ToString(false));
                    _xmlTextWriter.WriteAttributeString("InheritRDGatewayDomain", "", Convert.ToString(false));
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "SaveConnectionFields failed" + Environment.NewLine + ex.Message, true);
            }
        }
    }
}