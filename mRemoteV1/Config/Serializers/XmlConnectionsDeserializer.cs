using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Windows.Forms;
using System.Xml;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.ICA;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Container;
using mRemoteNG.Credential;
using mRemoteNG.Messages;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.TaskDialog;


namespace mRemoteNG.Config.Serializers
{
    public class XmlConnectionsDeserializer : IDeserializer
    {
        private XmlDocument _xmlDocument;
        private double _confVersion;
        private XmlConnectionsDecryptor _decryptor;
        //TODO find way to inject data source info
        private string ConnectionFileName = "";
        private const double MaxSupportedConfVersion = 2.8;
        private readonly RootNodeInfo _rootNodeInfo = new RootNodeInfo(RootNodeType.Connection);
        private readonly IEnumerable<ICredentialRecord> _credentialRecords;

        public Func<SecureString> AuthenticationRequestor { get; set; }

        public XmlConnectionsDeserializer(string xml, IEnumerable<ICredentialRecord> credentialRecords, Func<SecureString> authenticationRequestor = null)
        {
            _credentialRecords = credentialRecords;
            AuthenticationRequestor = authenticationRequestor;
            LoadXmlConnectionData(xml);
            ValidateConnectionFileVersion();
        }

        private void LoadXmlConnectionData(string connections)
        {
            CreateDecryptor(new RootNodeInfo(RootNodeType.Connection));
            connections = _decryptor.LegacyFullFileDecrypt(connections);
            _xmlDocument = new XmlDocument();
            if (connections != "")
                _xmlDocument.LoadXml(connections);
        }

        private void ValidateConnectionFileVersion()
        {
            if (_xmlDocument.DocumentElement != null && _xmlDocument.DocumentElement.HasAttribute("ConfVersion"))
                _confVersion = Convert.ToDouble(_xmlDocument.DocumentElement.Attributes["ConfVersion"].Value.Replace(",", "."), CultureInfo.InvariantCulture);
            else
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, Language.strOldConffile);
            
            if (!(_confVersion > MaxSupportedConfVersion)) return;
            ShowIncompatibleVersionDialogBox();
            throw new Exception($"Incompatible connection file format (file format version {_confVersion}).");
        }

        private void ShowIncompatibleVersionDialogBox()
        {
            CTaskDialog.ShowTaskDialogBox(
                FrmMain.Default,
                Application.ProductName,
                "Incompatible connection file format",
                $"The format of this connection file is not supported. Please upgrade to a newer version of {Application.ProductName}.",
                string.Format("{1}{0}File Format Version: {2}{0}Highest Supported Version: {3}", Environment.NewLine, ConnectionFileName, _confVersion, MaxSupportedConfVersion),
                "",
                "",
                "",
                "",
                ETaskDialogButtons.Ok,
                ESysIcons.Error,
                ESysIcons.Error
                );
        }

        public ConnectionTreeModel Deserialize()
        {
            return Deserialize(false);
        }

        public ConnectionTreeModel Deserialize(bool import)
        {
            try
            {
                if (!import)
                    Runtime.IsConnectionsFileLoaded = false;

                var rootXmlElement = _xmlDocument.DocumentElement;
                InitializeRootNode(rootXmlElement);
                CreateDecryptor(_rootNodeInfo, rootXmlElement);
                var connectionTreeModel = new ConnectionTreeModel();
                connectionTreeModel.AddRootNode(_rootNodeInfo);

                
                if (_confVersion > 1.3)
                {
                    var protectedString = _xmlDocument.DocumentElement?.Attributes["Protected"].Value;
                    if (!_decryptor.ConnectionsFileIsAuthentic(protectedString, _rootNodeInfo.PasswordString.ConvertToSecureString()))
                    {
                        mRemoteNG.Settings.Default.LoadConsFromCustomLocation = false;
                        mRemoteNG.Settings.Default.CustomConsPath = "";
                        return null;
                    }
                }

                if (_confVersion >= 2.6)
                {
                    if (rootXmlElement?.Attributes["FullFileEncryption"].Value == "True")
                    {
                        var decryptedContent = _decryptor.Decrypt(rootXmlElement.InnerText);
                        rootXmlElement.InnerXml = decryptedContent;
                    }
                }

                AddNodesFromXmlRecursive(_xmlDocument.DocumentElement, _rootNodeInfo);

                if (!import)
                    Runtime.IsConnectionsFileLoaded = true;

                return connectionTreeModel;
            }
            catch (Exception ex)
            {
                Runtime.IsConnectionsFileLoaded = false;
                Runtime.MessageCollector.AddExceptionStackTrace(Language.strLoadFromXmlFailed, ex);
                throw;
            }
        }

        private void InitializeRootNode(XmlElement connectionsRootElement)
        {
            var rootNodeName = connectionsRootElement?.Attributes["Name"].Value.Trim();
            _rootNodeInfo.Name = rootNodeName;
        }

        private void CreateDecryptor(RootNodeInfo rootNodeInfo, XmlElement connectionsRootElement = null)
        {
            if (_confVersion >= 2.6)
            {
                BlockCipherEngines engine;
                Enum.TryParse(connectionsRootElement?.Attributes["EncryptionEngine"].Value, out engine);

                BlockCipherModes mode;
                Enum.TryParse(connectionsRootElement?.Attributes["BlockCipherMode"].Value, out mode);

                int keyDerivationIterations;
                int.TryParse(connectionsRootElement?.Attributes["KdfIterations"].Value, out keyDerivationIterations);

                _decryptor = new XmlConnectionsDecryptor(engine, mode, rootNodeInfo)
                {
                    AuthenticationRequestor = AuthenticationRequestor,
                    KeyDerivationIterations = keyDerivationIterations
                };
            }
            else
            {
                _decryptor = new XmlConnectionsDecryptor(_rootNodeInfo) { AuthenticationRequestor = AuthenticationRequestor };
            }
        }

        private void AddNodesFromXmlRecursive(XmlNode parentXmlNode, ContainerInfo parentContainer)
        {
            try
            {
                if (!parentXmlNode.HasChildNodes) return;
                foreach (XmlNode xmlNode in parentXmlNode.ChildNodes)
                {
                    var treeNodeTypeString = xmlNode.Attributes?["Type"].Value ?? "connection";
                    var nodeType = (TreeNodeType)Enum.Parse(typeof(TreeNodeType), treeNodeTypeString, true);

                    if (nodeType == TreeNodeType.Connection)
                    {
                        var connectionInfo = GetConnectionInfoFromXml(xmlNode);
                        parentContainer.AddChild(connectionInfo);
                    }
                    else if (nodeType == TreeNodeType.Container)
                    {
                        var containerInfo = new ContainerInfo();
                            
                        if (_confVersion >= 0.9)
                            containerInfo.CopyFrom(GetConnectionInfoFromXml(xmlNode));
                        if (_confVersion >= 0.8)
                            containerInfo.IsExpanded = xmlNode.Attributes?["Expanded"].Value == "True";

                        parentContainer.AddChild(containerInfo);
                        AddNodesFromXmlRecursive(xmlNode, containerInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.strAddNodeFromXmlFailed, ex);
                throw;
            }
        }

        private ConnectionInfo GetConnectionInfoFromXml(XmlNode xmlnode)
        {
            if (xmlnode.Attributes == null) return null;
            var connectionInfo = new ConnectionInfo();

            try
            {
                if (_confVersion >= 0.2)
                {
                    connectionInfo.Name = xmlnode.Attributes["Name"].Value;
                    connectionInfo.Description = xmlnode.Attributes["Descr"].Value;
                    connectionInfo.Hostname = xmlnode.Attributes["Hostname"].Value;
                    connectionInfo.DisplayWallpaper = bool.Parse(xmlnode.Attributes["DisplayWallpaper"].Value);
                    connectionInfo.DisplayThemes = bool.Parse(xmlnode.Attributes["DisplayThemes"].Value);
                    connectionInfo.CacheBitmaps = bool.Parse(xmlnode.Attributes["CacheBitmaps"].Value);

                    if (_confVersion < 1.1) //1.0 - 0.1
                    {
                        connectionInfo.Resolution = Convert.ToBoolean(xmlnode.Attributes["Fullscreen"].Value) ? ProtocolRDP.RDPResolutions.Fullscreen : ProtocolRDP.RDPResolutions.FitToWindow;
                    }

                    if (_confVersion <= 2.6) // 0.2 - 2.6
                    {
#pragma warning disable 618
                        connectionInfo.Username = xmlnode.Attributes["Username"].Value;
                        connectionInfo.Password = _decryptor.Decrypt(xmlnode.Attributes["Password"].Value);
                        connectionInfo.Domain = xmlnode.Attributes["Domain"].Value;
#pragma warning restore 618
                    }
                }

                if (_confVersion >= 0.3)
                {
                    if (_confVersion < 0.7)
                    {
                        if (Convert.ToBoolean(xmlnode.Attributes["UseVNC"].Value))
                        {
                            connectionInfo.Protocol = ProtocolType.VNC;
                            connectionInfo.Port = Convert.ToInt32(xmlnode.Attributes["VNCPort"].Value);
                        }
                        else
                        {
                            connectionInfo.Protocol = ProtocolType.RDP;
                        }
                    }
                }
                else
                {
                    connectionInfo.Port = (int)ProtocolRDP.Defaults.Port;
                    connectionInfo.Protocol = ProtocolType.RDP;
                }

                if (_confVersion >= 0.4)
                {
                    if (_confVersion < 0.7)
                    {
                        connectionInfo.Port = Convert.ToInt32(Convert.ToBoolean(xmlnode.Attributes["UseVNC"].Value) ? xmlnode.Attributes["VNCPort"].Value : xmlnode.Attributes["RDPPort"].Value);
                    }

                    connectionInfo.UseConsoleSession = bool.Parse(xmlnode.Attributes["ConnectToConsole"].Value);
                }
                else
                {
                    if (_confVersion < 0.7)
                    {
                        if (Convert.ToBoolean(xmlnode.Attributes["UseVNC"].Value))
                            connectionInfo.Port = (int)ProtocolVNC.Defaults.Port;
                        else
                            connectionInfo.Port = (int)ProtocolRDP.Defaults.Port;
                    }
                    connectionInfo.UseConsoleSession = false;
                }

                if (_confVersion >= 0.5)
                {
                    connectionInfo.RedirectDiskDrives = bool.Parse(xmlnode.Attributes["RedirectDiskDrives"].Value);
                    connectionInfo.RedirectPrinters = bool.Parse(xmlnode.Attributes["RedirectPrinters"].Value);
                    connectionInfo.RedirectPorts = bool.Parse(xmlnode.Attributes["RedirectPorts"].Value);
                    connectionInfo.RedirectSmartCards = bool.Parse(xmlnode.Attributes["RedirectSmartCards"].Value);
                }
                else
                {
                    connectionInfo.RedirectDiskDrives = false;
                    connectionInfo.RedirectPrinters = false;
                    connectionInfo.RedirectPorts = false;
                    connectionInfo.RedirectSmartCards = false;
                }

                if (_confVersion >= 0.7)
                {
                    connectionInfo.Protocol = (ProtocolType)Tools.MiscTools.StringToEnum(typeof(ProtocolType), xmlnode.Attributes["Protocol"].Value);
                    connectionInfo.Port = Convert.ToInt32(xmlnode.Attributes["Port"].Value);
                }

                if (_confVersion >= 1.0)
                {
                    connectionInfo.RedirectKeys = bool.Parse(xmlnode.Attributes["RedirectKeys"].Value);
                }

                if (_confVersion >= 1.2)
                {
                    connectionInfo.PuttySession = xmlnode.Attributes["PuttySession"].Value;
                }

                if (_confVersion >= 1.3)
                {
                    connectionInfo.Colors = (ProtocolRDP.RDPColors)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.RDPColors), xmlnode.Attributes["Colors"].Value);
                    connectionInfo.Resolution = (ProtocolRDP.RDPResolutions)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.RDPResolutions), Convert.ToString(xmlnode.Attributes["Resolution"].Value));
                    connectionInfo.RedirectSound = (ProtocolRDP.RDPSounds)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.RDPSounds), Convert.ToString(xmlnode.Attributes["RedirectSound"].Value));
                }
                else
                {
                    switch (Convert.ToInt32(xmlnode.Attributes["Colors"].Value))
                    {
                        case 0:
                            connectionInfo.Colors = ProtocolRDP.RDPColors.Colors256;
                            break;
                        case 1:
                            connectionInfo.Colors = ProtocolRDP.RDPColors.Colors16Bit;
                            break;
                        case 2:
                            connectionInfo.Colors = ProtocolRDP.RDPColors.Colors24Bit;
                            break;
                        case 3:
                            connectionInfo.Colors = ProtocolRDP.RDPColors.Colors32Bit;
                            break;
                        case 4:
                            connectionInfo.Colors = ProtocolRDP.RDPColors.Colors15Bit;
                            break;
                    }

                    connectionInfo.RedirectSound = (ProtocolRDP.RDPSounds)Convert.ToInt32(xmlnode.Attributes["RedirectSound"].Value);
                }

                if (_confVersion >= 1.3)
                {
                    connectionInfo.Inheritance = new ConnectionInfoInheritance(connectionInfo)
                    {
                        CacheBitmaps = bool.Parse(xmlnode.Attributes["InheritCacheBitmaps"].Value),
                        Colors = bool.Parse(xmlnode.Attributes["InheritColors"].Value),
                        Description = bool.Parse(xmlnode.Attributes["InheritDescription"].Value),
                        DisplayThemes = bool.Parse(xmlnode.Attributes["InheritDisplayThemes"].Value),
                        DisplayWallpaper = bool.Parse(xmlnode.Attributes["InheritDisplayWallpaper"].Value),
                        Icon = bool.Parse(xmlnode.Attributes["InheritIcon"].Value),
                        Panel = bool.Parse(xmlnode.Attributes["InheritPanel"].Value),
                        Port = bool.Parse(xmlnode.Attributes["InheritPort"].Value),
                        Protocol = bool.Parse(xmlnode.Attributes["InheritProtocol"].Value),
                        PuttySession = bool.Parse(xmlnode.Attributes["InheritPuttySession"].Value),
                        RedirectDiskDrives = bool.Parse(xmlnode.Attributes["InheritRedirectDiskDrives"].Value),
                        RedirectKeys = bool.Parse(xmlnode.Attributes["InheritRedirectKeys"].Value),
                        RedirectPorts = bool.Parse(xmlnode.Attributes["InheritRedirectPorts"].Value),
                        RedirectPrinters = bool.Parse(xmlnode.Attributes["InheritRedirectPrinters"].Value),
                        RedirectSmartCards = bool.Parse(xmlnode.Attributes["InheritRedirectSmartCards"].Value),
                        RedirectSound = bool.Parse(xmlnode.Attributes["InheritRedirectSound"].Value),
                        Resolution = bool.Parse(xmlnode.Attributes["InheritResolution"].Value),
                        UseConsoleSession = bool.Parse(xmlnode.Attributes["InheritUseConsoleSession"].Value),
                    };
                    if (_confVersion <= 2.6) // 1.3 - 2.6
                    {
                        connectionInfo.Inheritance.Domain = bool.Parse(xmlnode.Attributes["InheritDomain"].Value);
                        connectionInfo.Inheritance.Password = bool.Parse(xmlnode.Attributes["InheritPassword"].Value);
                        connectionInfo.Inheritance.Username = bool.Parse(xmlnode.Attributes["InheritUsername"].Value);
                    }
                    connectionInfo.Icon = xmlnode.Attributes["Icon"].Value;
                    connectionInfo.Panel = xmlnode.Attributes["Panel"].Value;
                }
                else
                {
                    connectionInfo.Inheritance = new ConnectionInfoInheritance(connectionInfo);
                    if (Convert.ToBoolean(xmlnode.Attributes["Inherit"].Value))
                        connectionInfo.Inheritance.TurnOnInheritanceCompletely();
                    connectionInfo.Icon = Convert.ToString(xmlnode.Attributes["Icon"].Value.Replace(".ico", ""));
                    connectionInfo.Panel = Language.strGeneral;
                }

                if (_confVersion >= 1.5)
                {
                    connectionInfo.PleaseConnect = bool.Parse(xmlnode.Attributes["Connected"].Value);
                }

                if (_confVersion >= 1.6)
                {
                    connectionInfo.ICAEncryptionStrength = (ProtocolICA.EncryptionStrength)Tools.MiscTools.StringToEnum(typeof(ProtocolICA.EncryptionStrength), xmlnode.Attributes["ICAEncryptionStrength"].Value);
                    connectionInfo.Inheritance.ICAEncryptionStrength = bool.Parse(xmlnode.Attributes["InheritICAEncryptionStrength"].Value);
                    connectionInfo.PreExtApp = xmlnode.Attributes["PreExtApp"].Value;
                    connectionInfo.PostExtApp = xmlnode.Attributes["PostExtApp"].Value;
                    connectionInfo.Inheritance.PreExtApp = bool.Parse(xmlnode.Attributes["InheritPreExtApp"].Value);
                    connectionInfo.Inheritance.PostExtApp = bool.Parse(xmlnode.Attributes["InheritPostExtApp"].Value);
                }

                if (_confVersion >= 1.7)
                {
                    connectionInfo.VNCCompression = (ProtocolVNC.Compression)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.Compression), xmlnode.Attributes["VNCCompression"].Value);
                    connectionInfo.VNCEncoding = (ProtocolVNC.Encoding)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.Encoding), Convert.ToString(xmlnode.Attributes["VNCEncoding"].Value));
                    connectionInfo.VNCAuthMode = (ProtocolVNC.AuthMode)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.AuthMode), xmlnode.Attributes["VNCAuthMode"].Value);
                    connectionInfo.VNCProxyType = (ProtocolVNC.ProxyType)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.ProxyType), xmlnode.Attributes["VNCProxyType"].Value);
                    connectionInfo.VNCProxyIP = xmlnode.Attributes["VNCProxyIP"].Value;
                    connectionInfo.VNCProxyPort = Convert.ToInt32(xmlnode.Attributes["VNCProxyPort"].Value);
                    connectionInfo.VNCProxyUsername = xmlnode.Attributes["VNCProxyUsername"].Value;
                    connectionInfo.VNCProxyPassword = _decryptor.Decrypt(xmlnode.Attributes["VNCProxyPassword"].Value);
                    connectionInfo.VNCColors = (ProtocolVNC.Colors)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.Colors), xmlnode.Attributes["VNCColors"].Value);
                    connectionInfo.VNCSmartSizeMode = (ProtocolVNC.SmartSizeMode)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.SmartSizeMode), xmlnode.Attributes["VNCSmartSizeMode"].Value);
                    connectionInfo.VNCViewOnly = bool.Parse(xmlnode.Attributes["VNCViewOnly"].Value);
                    connectionInfo.Inheritance.VNCCompression = bool.Parse(xmlnode.Attributes["InheritVNCCompression"].Value);
                    connectionInfo.Inheritance.VNCEncoding = bool.Parse(xmlnode.Attributes["InheritVNCEncoding"].Value);
                    connectionInfo.Inheritance.VNCAuthMode = bool.Parse(xmlnode.Attributes["InheritVNCAuthMode"].Value);
                    connectionInfo.Inheritance.VNCProxyType = bool.Parse(xmlnode.Attributes["InheritVNCProxyType"].Value);
                    connectionInfo.Inheritance.VNCProxyIP = bool.Parse(xmlnode.Attributes["InheritVNCProxyIP"].Value);
                    connectionInfo.Inheritance.VNCProxyPort = bool.Parse(xmlnode.Attributes["InheritVNCProxyPort"].Value);
                    connectionInfo.Inheritance.VNCProxyUsername = bool.Parse(xmlnode.Attributes["InheritVNCProxyUsername"].Value);
                    connectionInfo.Inheritance.VNCProxyPassword = bool.Parse(xmlnode.Attributes["InheritVNCProxyPassword"].Value);
                    connectionInfo.Inheritance.VNCColors = bool.Parse(xmlnode.Attributes["InheritVNCColors"].Value);
                    connectionInfo.Inheritance.VNCSmartSizeMode = bool.Parse(xmlnode.Attributes["InheritVNCSmartSizeMode"].Value);
                    connectionInfo.Inheritance.VNCViewOnly = bool.Parse(xmlnode.Attributes["InheritVNCViewOnly"].Value);
                }

                if (_confVersion >= 1.8)
                {
                    connectionInfo.RDPAuthenticationLevel = (ProtocolRDP.AuthenticationLevel)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.AuthenticationLevel), xmlnode.Attributes["RDPAuthenticationLevel"].Value);
                    connectionInfo.Inheritance.RDPAuthenticationLevel = bool.Parse(xmlnode.Attributes["InheritRDPAuthenticationLevel"].Value);
                }

                if (_confVersion >= 1.9)
                {
                    connectionInfo.RenderingEngine = (HTTPBase.RenderingEngine)Tools.MiscTools.StringToEnum(typeof(HTTPBase.RenderingEngine), xmlnode.Attributes["RenderingEngine"].Value);
                    connectionInfo.MacAddress = xmlnode.Attributes["MacAddress"].Value;
                    connectionInfo.Inheritance.RenderingEngine = bool.Parse(xmlnode.Attributes["InheritRenderingEngine"].Value);
                    connectionInfo.Inheritance.MacAddress = bool.Parse(xmlnode.Attributes["InheritMacAddress"].Value);
                }

                if (_confVersion >= 2.0)
                {
                    connectionInfo.UserField = xmlnode.Attributes["UserField"].Value;
                    connectionInfo.Inheritance.UserField = bool.Parse(xmlnode.Attributes["InheritUserField"].Value);
                }

                if (_confVersion >= 2.1)
                {
                    connectionInfo.ExtApp = xmlnode.Attributes["ExtApp"].Value;
                    connectionInfo.Inheritance.ExtApp = bool.Parse(xmlnode.Attributes["InheritExtApp"].Value);
                }

                if (_confVersion >= 2.2)
                {
                    // Get settings
                    connectionInfo.RDGatewayUsageMethod = (ProtocolRDP.RDGatewayUsageMethod)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.RDGatewayUsageMethod), Convert.ToString(xmlnode.Attributes["RDGatewayUsageMethod"].Value));
                    connectionInfo.RDGatewayHostname = xmlnode.Attributes["RDGatewayHostname"].Value;
                    connectionInfo.RDGatewayUseConnectionCredentials = (ProtocolRDP.RDGatewayUseConnectionCredentials)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.RDGatewayUseConnectionCredentials), Convert.ToString(xmlnode.Attributes["RDGatewayUseConnectionCredentials"].Value));
                    connectionInfo.RDGatewayUsername = xmlnode.Attributes["RDGatewayUsername"].Value;
                    connectionInfo.RDGatewayPassword = _decryptor.Decrypt(Convert.ToString(xmlnode.Attributes["RDGatewayPassword"].Value));
                    connectionInfo.RDGatewayDomain = xmlnode.Attributes["RDGatewayDomain"].Value;

                    // Get inheritance settings
                    connectionInfo.Inheritance.RDGatewayUsageMethod = bool.Parse(xmlnode.Attributes["InheritRDGatewayUsageMethod"].Value);
                    connectionInfo.Inheritance.RDGatewayHostname = bool.Parse(xmlnode.Attributes["InheritRDGatewayHostname"].Value);
                    connectionInfo.Inheritance.RDGatewayUseConnectionCredentials = bool.Parse(xmlnode.Attributes["InheritRDGatewayUseConnectionCredentials"].Value);
                    connectionInfo.Inheritance.RDGatewayUsername = bool.Parse(xmlnode.Attributes["InheritRDGatewayUsername"].Value);
                    connectionInfo.Inheritance.RDGatewayPassword = bool.Parse(xmlnode.Attributes["InheritRDGatewayPassword"].Value);
                    connectionInfo.Inheritance.RDGatewayDomain = bool.Parse(xmlnode.Attributes["InheritRDGatewayDomain"].Value);
                }

                if (_confVersion >= 2.3)
                {
                    // Get settings
                    connectionInfo.EnableFontSmoothing = bool.Parse(xmlnode.Attributes["EnableFontSmoothing"].Value);
                    connectionInfo.EnableDesktopComposition = bool.Parse(xmlnode.Attributes["EnableDesktopComposition"].Value);

                    // Get inheritance settings
                    connectionInfo.Inheritance.EnableFontSmoothing = bool.Parse(xmlnode.Attributes["InheritEnableFontSmoothing"].Value);
                    connectionInfo.Inheritance.EnableDesktopComposition = bool.Parse(xmlnode.Attributes["InheritEnableDesktopComposition"].Value);
                }

                if (_confVersion >= 2.4)
                {
                    connectionInfo.UseCredSsp = bool.Parse(xmlnode.Attributes["UseCredSsp"].Value);
                    connectionInfo.Inheritance.UseCredSsp = bool.Parse(xmlnode.Attributes["InheritUseCredSsp"].Value);
                }

                if (_confVersion >= 2.5)
                {
                    connectionInfo.LoadBalanceInfo = xmlnode.Attributes["LoadBalanceInfo"].Value;
                    connectionInfo.AutomaticResize = bool.Parse(xmlnode.Attributes["AutomaticResize"].Value);
                    connectionInfo.Inheritance.LoadBalanceInfo = bool.Parse(xmlnode.Attributes["InheritLoadBalanceInfo"].Value);
                    connectionInfo.Inheritance.AutomaticResize = bool.Parse(xmlnode.Attributes["InheritAutomaticResize"].Value);
                }

                if (_confVersion >= 2.6)
                {
                    connectionInfo.ConstantID = xmlnode.Attributes["Id"]?.Value ?? connectionInfo.ConstantID;
                    connectionInfo.SoundQuality = (ProtocolRDP.RDPSoundQuality)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.RDPSoundQuality), Convert.ToString(xmlnode.Attributes["SoundQuality"].Value));
                    connectionInfo.Inheritance.SoundQuality = bool.Parse(xmlnode.Attributes["InheritSoundQuality"].Value);
                    connectionInfo.RDPMinutesToIdleTimeout = Convert.ToInt32(xmlnode.Attributes["RDPMinutesToIdleTimeout"]?.Value ?? "0");
                    connectionInfo.Inheritance.RDPMinutesToIdleTimeout = bool.Parse(xmlnode.Attributes["InheritRDPMinutesToIdleTimeout"]?.Value ?? "False");
                    connectionInfo.RDPAlertIdleTimeout = bool.Parse(xmlnode.Attributes["RDPAlertIdleTimeout"]?.Value ?? "False");
                    connectionInfo.Inheritance.RDPAlertIdleTimeout = bool.Parse(xmlnode.Attributes["InheritRDPAlertIdleTimeout"]?.Value ?? "False");
                }

                if (_confVersion >= 2.7)
                {
                    connectionInfo.Inheritance.CredentialRecord = bool.Parse(xmlnode.Attributes["InheritCredentialRecord"]?.Value ?? "False");

                    var requestedCredentialId = xmlnode.Attributes["CredentialId"]?.Value;
                    if (!string.IsNullOrEmpty(requestedCredentialId) && _credentialRecords.Any())
                    {
                        var matchingCredential = _credentialRecords.Where(record => record.Id.ToString() == requestedCredentialId).ToArray();
                        if (matchingCredential.Any())
                            connectionInfo.CredentialRecord = matchingCredential.First();
                        else
                            Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg, string.Format(Language.strFindMatchingCredentialFailed, requestedCredentialId, connectionInfo.Name));
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, string.Format(Language.strGetConnectionInfoFromXmlFailed, connectionInfo.Name, ConnectionFileName, ex.Message));
            }
            return connectionInfo;
        }
    }
}