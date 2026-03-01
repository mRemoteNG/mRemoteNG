using System;
using System.Diagnostics; // Added
using System.Globalization;
using System.Security;
using System.Windows.Forms;
using System.Xml;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using mRemoteNG.Security;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.TaskDialog;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Xml
{
    [SupportedOSPlatform("windows")]
    public class XmlConnectionsDeserializer(string connectionFileName = "", Func<Optional<SecureString>>? authenticationRequestor = null) : IDeserializer<string, ConnectionTreeModel>
    {
        private XmlDocument _xmlDocument = null!;
        private double _confVersion;
        private XmlConnectionsDecryptor _decryptor = null!;
        private readonly string ConnectionFileName = connectionFileName;
        private const double MaxSupportedConfVersion = 2.8;
        private readonly RootNodeInfo _rootNodeInfo = new(RootNodeType.Connection);
        private ConnectionTreeModel _connectionTreeModel = null!;
        private BlockCipherEngines _cipherEngine;
        private BlockCipherModes _cipherMode;
        private int _kdfIterations;

        public Func<Optional<SecureString>>? AuthenticationRequestor { get; set; } = authenticationRequestor;

        public ConnectionTreeModel Deserialize(string xml)
        {
            return Deserialize(xml, false)!;
        }

        public ConnectionTreeModel? Deserialize(string xml, bool import)
        {
            if (string.IsNullOrEmpty(xml)) return null;

            var stopwatch = Stopwatch.StartNew(); // Start stopwatch

            try
            {
                _rootNodeInfo.Filename = ConnectionFileName;
                LoadXmlConnectionData(xml);
                ValidateConnectionFileVersion();

                XmlElement rootXmlElement = _xmlDocument.DocumentElement
                    ?? throw new XmlException("Failed to parse XML connection file.");
                InitializeRootNode(rootXmlElement);
                CreateDecryptor(_rootNodeInfo, rootXmlElement);
                _connectionTreeModel = new ConnectionTreeModel();
                _connectionTreeModel.AddRootNode(_rootNodeInfo);


                if (_confVersion > 1.3)
                {
                    string protectedString = _xmlDocument.DocumentElement?.Attributes["Protected"]?.Value ?? string.Empty;
                    if (!_decryptor.ConnectionsFileIsAuthentic(protectedString, _rootNodeInfo.PasswordString.ConvertToSecureString()))
                    {
                        return null;
                    }
                }

                if (_confVersion >= 2.6)
                {
                    bool fullFileEncryptionValue = rootXmlElement.GetAttributeAsBool("FullFileEncryption");
                    if (fullFileEncryptionValue)
                    {
                        string decryptedContent = _decryptor.Decrypt(rootXmlElement.InnerText);
                        rootXmlElement.InnerXml = decryptedContent;
                    }
                }

                AddNodesFromXmlRecursive(rootXmlElement, _rootNodeInfo);

                if (!import)
                    Runtime.ConnectionsService.IsConnectionsFileLoaded = true;

                stopwatch.Stop(); // Stop stopwatch
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, $"Connection deserialization completed in {stopwatch.ElapsedMilliseconds} ms."); // Log performance

                return _connectionTreeModel;
            }
            catch (Exception ex)
            {
                Runtime.ConnectionsService.IsConnectionsFileLoaded = false;
                Runtime.MessageCollector.AddExceptionStackTrace(Language.LoadFromXmlFailed, ex);

                stopwatch.Stop(); // Stop stopwatch even on error
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, $"Connection deserialization failed after {stopwatch.ElapsedMilliseconds} ms."); // Log performance on error

                throw;
            }
        }

        private void LoadXmlConnectionData(string connections)
        {
            CreateDecryptor(new RootNodeInfo(RootNodeType.Connection));
            connections = _decryptor.LegacyFullFileDecrypt(connections);
            if (connections != "")
            {
                _xmlDocument = SecureXmlHelper.LoadXmlFromString(connections);
            }
        }

        private void ValidateConnectionFileVersion()
        {
            if (_xmlDocument?.DocumentElement == null)
                throw new XmlException("Failed to parse XML connection file.");

            if (_xmlDocument.DocumentElement != null && _xmlDocument.DocumentElement.HasAttribute("ConfVersion"))
                _confVersion = Convert.ToDouble(_xmlDocument.DocumentElement.Attributes["ConfVersion"]?.Value.Replace(",", ".", StringComparison.Ordinal), CultureInfo.InvariantCulture);
            else
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, Language.OldConffile);

            if (!(_confVersion > MaxSupportedConfVersion)) return;
            ShowIncompatibleVersionDialogBox();
            throw new NotSupportedException($"Incompatible connection file format (file format version {_confVersion}).");
        }

        private void ShowIncompatibleVersionDialogBox()
        {
            CTaskDialog.ShowTaskDialogBox(FrmMain.Default, Application.ProductName ?? "mRemoteNG", "Incompatible connection file format", $"The format of this connection file is not supported. Please upgrade to a newer version of {Application.ProductName}.",
                                          string.Format(CultureInfo.InvariantCulture, "{1}{0}File Format Version: {2}{0}Highest Supported Version: {3}", Environment.NewLine, ConnectionFileName, _confVersion, MaxSupportedConfVersion),
                                          "", "", "", "", ETaskDialogButtons.Ok, ESysIcons.Error, ESysIcons.Error);
        }

        private void InitializeRootNode(XmlElement connectionsRootElement)
        {
            _rootNodeInfo.Name = connectionsRootElement.Attributes?["Name"]?.Value?.Trim() ?? string.Empty;
            _rootNodeInfo.AutoLockOnMinimize = connectionsRootElement.GetAttributeAsBool("AutoLockOnMinimize");
        }

        private void CreateDecryptor(RootNodeInfo rootNodeInfo, XmlElement? connectionsRootElement = null)
        {
            if (_confVersion >= 2.6 && connectionsRootElement != null)
            {
                _cipherEngine = connectionsRootElement.GetAttributeAsEnum<BlockCipherEngines>("EncryptionEngine");
                _cipherMode = connectionsRootElement.GetAttributeAsEnum<BlockCipherModes>("BlockCipherMode");
                _kdfIterations = connectionsRootElement.GetAttributeAsInt("KdfIterations");

                _decryptor = new XmlConnectionsDecryptor(_cipherEngine, _cipherMode, rootNodeInfo)
                {
                    AuthenticationRequestor = AuthenticationRequestor,
                    KeyDerivationIterations = _kdfIterations
                };
            }
            else
            {
                _decryptor = new XmlConnectionsDecryptor(_rootNodeInfo)
                {
                    AuthenticationRequestor = AuthenticationRequestor
                };
            }
        }

        private void AddNodesFromXmlRecursive(XmlNode parentXmlNode, ContainerInfo parentContainer)
        {
            try
            {
                if (!parentXmlNode.HasChildNodes) return;
                foreach (XmlNode xmlNode in parentXmlNode.ChildNodes)
                {
                    TreeNodeType nodeType = xmlNode.GetAttributeAsEnum("Type", TreeNodeType.Connection);

                    // ReSharper disable once SwitchStatementMissingSomeCases
                    switch (nodeType)
                    {
                        case TreeNodeType.Connection:
                            ConnectionInfo? connectionInfo = GetConnectionInfoFromXml(xmlNode);
                            if (connectionInfo != null)
                                parentContainer.AddChild(connectionInfo);
                            break;
                        case TreeNodeType.Container:
                        case TreeNodeType.Entity:
                            ContainerInfo containerInfo = new();
                            if (nodeType == TreeNodeType.Entity)
                                containerInfo.IsEntity = true;

                            if (_confVersion >= 0.9)
                            {
                                ConnectionInfo? containerProps = GetConnectionInfoFromXml(xmlNode);
                                if (containerProps != null)
                                    containerInfo.CopyFrom(containerProps);
                            }
                            if (_confVersion >= 0.8)
                            {
                                containerInfo.IsExpanded = xmlNode.GetAttributeAsBool("Expanded");
                            }

                            if (_confVersion >= 2.8)
                            {
                                containerInfo.AutoSort = xmlNode.GetAttributeAsBool("AutoSort");
                                containerInfo.ContainerPassword = DecryptField(xmlNode, "ContainerPassword");
                                containerInfo.DynamicSource = xmlNode.GetAttributeAsEnum("DynamicSource", DynamicSourceType.None);
                                containerInfo.DynamicSourceValue = xmlNode.GetAttributeAsString("DynamicSourceValue");
                                containerInfo.DynamicRefreshInterval = xmlNode.GetAttributeAsInt("DynamicRefreshInterval");
                            }

                            if (containerInfo.IsRoot)
                                _connectionTreeModel.AddRootNode(containerInfo);
                            else
                                parentContainer.AddChild(containerInfo);

                            AddNodesFromXmlRecursive(xmlNode, containerInfo);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.AddNodeFromXmlFailed, ex);
                throw;
            }
        }

        private ConnectionInfo? GetConnectionInfoFromXml(XmlNode xmlnode)
        {
            if (xmlnode?.Attributes == null)
                return null;

            string connectionId = xmlnode.GetAttributeAsString("Id");
            if (string.IsNullOrWhiteSpace(connectionId))
                connectionId = Guid.NewGuid().ToString();
            ConnectionInfo connectionInfo = new(connectionId)
            {
                LinkedConnectionId = xmlnode.GetAttributeAsString("LinkedConnectionId")
            };

            try
            {
                if (_confVersion >= 0.2)
                {
                    connectionInfo.Name = xmlnode.GetAttributeAsString("Name");
                    connectionInfo.Description = xmlnode.GetAttributeAsString("Descr");
                    connectionInfo.Hostname = xmlnode.GetAttributeAsString("Hostname");
                    connectionInfo.AlternativeAddress = xmlnode.GetAttributeAsString("AlternativeAddress");
                    connectionInfo.DisplayWallpaper = xmlnode.GetAttributeAsBool("DisplayWallpaper");
                    connectionInfo.DisplayThemes = xmlnode.GetAttributeAsBool("DisplayThemes");
                    connectionInfo.CacheBitmaps = xmlnode.GetAttributeAsBool("CacheBitmaps");

                    if (_confVersion < 1.1) //1.0 - 0.1
                    {
                        connectionInfo.Resolution = xmlnode.GetAttributeAsBool("Fullscreen")
                            ? RDPResolutions.Fullscreen
                            : RDPResolutions.FitToWindow;
                    }

                    if (!Runtime.UseCredentialManager || _confVersion <= 2.6) // 0.2 - 2.6
                    {
                        connectionInfo.Username = xmlnode.GetAttributeAsString("Username");
                        connectionInfo.Password = DecryptField(xmlnode, "Password");
                        //connectionInfo.Password = _decryptor.Decrypt(xmlnode.GetAttributeAsString("Password")).ConvertToSecureString();
                        connectionInfo.Domain = xmlnode.GetAttributeAsString("Domain");
                    }
                }

                if (_confVersion >= 0.3)
                {
                    if (_confVersion < 0.7)
                    {
                        if (xmlnode.GetAttributeAsBool("UseVNC"))
                        {
                            connectionInfo.Protocol = ProtocolType.VNC;
                            connectionInfo.Port = xmlnode.GetAttributeAsInt("VNCPort");
                        }
                        else
                        {
                            connectionInfo.Protocol = ProtocolType.RDP;
                        }
                    }
                }
                else
                {
                    connectionInfo.Port = (int)RdpProtocol.Defaults.Port;
                    connectionInfo.Protocol = ProtocolType.RDP;
                }

                if (_confVersion >= 0.4)
                {
                    if (_confVersion < 0.7)
                    {
                        connectionInfo.Port = xmlnode.GetAttributeAsBool("UseVNC")
                            ? xmlnode.GetAttributeAsInt("VNCPort")
                            : xmlnode.GetAttributeAsInt("RDPPort");
                    }

                    connectionInfo.UseConsoleSession = xmlnode.GetAttributeAsBool("ConnectToConsole");
                }
                else
                {
                    if (_confVersion < 0.7)
                    {
                        if (xmlnode.GetAttributeAsBool("UseVNC"))
                            connectionInfo.Port = (int)ProtocolVNC.Defaults.Port;
                        else
                            connectionInfo.Port = (int)RdpProtocol.Defaults.Port;
                    }

                    connectionInfo.UseConsoleSession = false;
                }

                if (_confVersion >= 0.5)
                {
                    connectionInfo.RedirectPrinters = xmlnode.GetAttributeAsBool("RedirectPrinters");
                    connectionInfo.RedirectPorts = xmlnode.GetAttributeAsBool("RedirectPorts");
                    connectionInfo.RedirectSmartCards = xmlnode.GetAttributeAsBool("RedirectSmartCards");
                }
                else
                {
                    connectionInfo.RedirectDiskDrives = RDPDiskDrives.None;
                    connectionInfo.RedirectPrinters = false;
                    connectionInfo.RedirectPorts = false;
                    connectionInfo.RedirectSmartCards = false;
                }

                if (_confVersion >= 0.7)
                {
                    connectionInfo.Protocol = xmlnode.GetAttributeAsEnum<ProtocolType>("Protocol");
                    connectionInfo.Port = xmlnode.GetAttributeAsInt("Port");
                }

                if (_confVersion >= 1.0)
                {
                    connectionInfo.RedirectKeys = xmlnode.GetAttributeAsBool("RedirectKeys");
                }

                if (_confVersion >= 1.2)
                {
                    connectionInfo.PuttySession = xmlnode.GetAttributeAsString("PuttySession");
                }

                if (_confVersion >= 1.3)
                {
                    connectionInfo.Colors = xmlnode.GetAttributeAsEnum<RDPColors>("Colors");
                    connectionInfo.Resolution = xmlnode.GetAttributeAsEnum<RDPResolutions>("Resolution");
                    connectionInfo.RedirectSound = xmlnode.GetAttributeAsEnum<RDPSounds>("RedirectSound");
                    connectionInfo.RedirectAudioCapture = xmlnode.GetAttributeAsBool("RedirectAudioCapture");
                }
                else
                {
                    connectionInfo.Colors = xmlnode.GetAttributeAsInt("Colors") switch
                    {
                        0 => RDPColors.Colors256,
                        1 => RDPColors.Colors16Bit,
                        2 => RDPColors.Colors24Bit,
                        3 => RDPColors.Colors32Bit,
                        // ReSharper disable once RedundantCaseLabel
                        _ => RDPColors.Colors15Bit,
                    };
                    connectionInfo.RedirectSound = xmlnode.GetAttributeAsEnum<RDPSounds>("RedirectSound");
                    connectionInfo.RedirectAudioCapture = xmlnode.GetAttributeAsBool("RedirectAudioCapture");
                }

                if (_confVersion >= 1.3)
                {
                    connectionInfo.Inheritance.CacheBitmaps = xmlnode.GetAttributeAsBool("InheritCacheBitmaps");
                    connectionInfo.Inheritance.Colors = xmlnode.GetAttributeAsBool("InheritColors");
                    connectionInfo.Inheritance.Description = xmlnode.GetAttributeAsBool("InheritDescription");
                    connectionInfo.Inheritance.DisplayThemes = xmlnode.GetAttributeAsBool("InheritDisplayThemes");
                    connectionInfo.Inheritance.DisplayWallpaper = xmlnode.GetAttributeAsBool("InheritDisplayWallpaper");
                    connectionInfo.Inheritance.Icon = xmlnode.GetAttributeAsBool("InheritIcon");
                    connectionInfo.Inheritance.Panel = xmlnode.GetAttributeAsBool("InheritPanel");
                    connectionInfo.Inheritance.TabColor = xmlnode.GetAttributeAsBool("InheritTabColor");
                    connectionInfo.Inheritance.ConnectionFrameColor = xmlnode.GetAttributeAsBool("InheritConnectionFrameColor");
                    connectionInfo.Inheritance.Port = xmlnode.GetAttributeAsBool("InheritPort");
                    connectionInfo.Inheritance.Protocol = xmlnode.GetAttributeAsBool("InheritProtocol");
                    connectionInfo.Inheritance.PuttySession = xmlnode.GetAttributeAsBool("InheritPuttySession");
                    connectionInfo.Inheritance.RedirectDiskDrives = xmlnode.GetAttributeAsBool("InheritRedirectDiskDrives");
                    connectionInfo.Inheritance.RedirectKeys = xmlnode.GetAttributeAsBool("InheritRedirectKeys");
                    connectionInfo.Inheritance.RedirectPorts = xmlnode.GetAttributeAsBool("InheritRedirectPorts");
                    connectionInfo.Inheritance.RedirectPrinters = xmlnode.GetAttributeAsBool("InheritRedirectPrinters");
                    connectionInfo.Inheritance.RedirectSmartCards = xmlnode.GetAttributeAsBool("InheritRedirectSmartCards");
                    connectionInfo.Inheritance.RedirectSound = xmlnode.GetAttributeAsBool("InheritRedirectSound");
                    connectionInfo.Inheritance.RedirectAudioCapture = xmlnode.GetAttributeAsBool("InheritRedirectAudioCapture");
                    connectionInfo.Inheritance.Resolution = xmlnode.GetAttributeAsBool("InheritResolution");
                    connectionInfo.Inheritance.UseConsoleSession = xmlnode.GetAttributeAsBool("InheritUseConsoleSession");

                    if (!Runtime.UseCredentialManager || _confVersion <= 2.6) // 1.3 - 2.6
                    {
                        connectionInfo.Inheritance.Domain = xmlnode.GetAttributeAsBool("InheritDomain");
                        connectionInfo.Inheritance.Password = xmlnode.GetAttributeAsBool("InheritPassword");
                        connectionInfo.Inheritance.Username = xmlnode.GetAttributeAsBool("InheritUsername");
                    }

                    connectionInfo.Inheritance.Color = xmlnode.GetAttributeAsBool("InheritColor");
                    connectionInfo.Icon = xmlnode.GetAttributeAsString("Icon");
                    connectionInfo.Panel = xmlnode.GetAttributeAsString("Panel");
                    connectionInfo.Color = xmlnode.GetAttributeAsString("Color");
                    connectionInfo.TabColor = xmlnode.GetAttributeAsString("TabColor");
                    connectionInfo.ConnectionFrameColor = xmlnode.GetAttributeAsEnum<ConnectionFrameColor>("ConnectionFrameColor");
                }
                else
                {
                    if (xmlnode.GetAttributeAsBool("Inherit"))
                        connectionInfo.Inheritance.TurnOnInheritanceCompletely();
                    connectionInfo.Icon = xmlnode.GetAttributeAsString("Icon").Replace(".ico", "", StringComparison.Ordinal);
                    connectionInfo.Panel = "General";
                }

                if (_confVersion >= 1.5)
                {
                    connectionInfo.PleaseConnect = xmlnode.GetAttributeAsBool("Connected");
                }

                if (_confVersion >= 1.6)
                {
                    connectionInfo.PreExtApp = xmlnode.GetAttributeAsString("PreExtApp");
                    connectionInfo.PostExtApp = xmlnode.GetAttributeAsString("PostExtApp");
                    connectionInfo.Inheritance.PreExtApp = xmlnode.GetAttributeAsBool("InheritPreExtApp");
                    connectionInfo.Inheritance.PostExtApp = xmlnode.GetAttributeAsBool("InheritPostExtApp");
                }

                if (_confVersion >= 1.7)
                {
                    connectionInfo.VNCCompression = xmlnode.GetAttributeAsEnum<ProtocolVNC.Compression>("VNCCompression");
                    connectionInfo.VNCEncoding = xmlnode.GetAttributeAsEnum<ProtocolVNC.Encoding>("VNCEncoding");
                    connectionInfo.VNCAuthMode = xmlnode.GetAttributeAsEnum<ProtocolVNC.AuthMode>("VNCAuthMode");
                    connectionInfo.VNCProxyType = xmlnode.GetAttributeAsEnum<ProtocolVNC.ProxyType>("VNCProxyType");
                    connectionInfo.VNCProxyIP = xmlnode.GetAttributeAsString("VNCProxyIP");
                    connectionInfo.VNCProxyPort = xmlnode.GetAttributeAsInt("VNCProxyPort");
                    connectionInfo.VNCProxyUsername = xmlnode.GetAttributeAsString("VNCProxyUsername");
                    connectionInfo.VNCProxyPassword = DecryptField(xmlnode, "VNCProxyPassword");
                    connectionInfo.VNCColors = xmlnode.GetAttributeAsEnum<ProtocolVNC.Colors>("VNCColors");
                    connectionInfo.VNCSmartSizeMode = xmlnode.GetAttributeAsEnum<ProtocolVNC.SmartSizeMode>("VNCSmartSizeMode");
                    connectionInfo.VNCViewOnly = xmlnode.GetAttributeAsBool("VNCViewOnly");
                    connectionInfo.VNCClipboardRedirect = xmlnode.GetAttributeAsBool("VNCClipboardRedirect", true);
                    connectionInfo.Inheritance.VNCCompression = xmlnode.GetAttributeAsBool("InheritVNCCompression");
                    connectionInfo.Inheritance.VNCEncoding = xmlnode.GetAttributeAsBool("InheritVNCEncoding");
                    connectionInfo.Inheritance.VNCAuthMode = xmlnode.GetAttributeAsBool("InheritVNCAuthMode");
                    connectionInfo.Inheritance.VNCProxyType = xmlnode.GetAttributeAsBool("InheritVNCProxyType");
                    connectionInfo.Inheritance.VNCProxyIP = xmlnode.GetAttributeAsBool("InheritVNCProxyIP");
                    connectionInfo.Inheritance.VNCProxyPort = xmlnode.GetAttributeAsBool("InheritVNCProxyPort");
                    connectionInfo.Inheritance.VNCProxyUsername = xmlnode.GetAttributeAsBool("InheritVNCProxyUsername");
                    connectionInfo.Inheritance.VNCProxyPassword = xmlnode.GetAttributeAsBool("InheritVNCProxyPassword");
                    connectionInfo.Inheritance.VNCColors = xmlnode.GetAttributeAsBool("InheritVNCColors");
                    connectionInfo.Inheritance.VNCSmartSizeMode = xmlnode.GetAttributeAsBool("InheritVNCSmartSizeMode");
                    connectionInfo.Inheritance.VNCViewOnly = xmlnode.GetAttributeAsBool("InheritVNCViewOnly");
                    connectionInfo.Inheritance.VNCClipboardRedirect = xmlnode.GetAttributeAsBool("InheritVNCClipboardRedirect");
                }

                if (_confVersion >= 1.8)
                {
                    connectionInfo.RDPAuthenticationLevel = xmlnode.GetAttributeAsEnum<AuthenticationLevel>("RDPAuthenticationLevel");
                    connectionInfo.Inheritance.RDPAuthenticationLevel = xmlnode.GetAttributeAsBool("InheritRDPAuthenticationLevel");
                }

                if (_confVersion >= 1.9)
                {
                    connectionInfo.RenderingEngine = xmlnode.GetAttributeAsEnum<HTTPBase.RenderingEngine>("RenderingEngine");
                    connectionInfo.MacAddress = xmlnode.GetAttributeAsString("MacAddress");
                    connectionInfo.Inheritance.RenderingEngine = xmlnode.GetAttributeAsBool("InheritRenderingEngine");
                    connectionInfo.Inheritance.MacAddress = xmlnode.GetAttributeAsBool("InheritMacAddress");
                }

                if (_confVersion >= 2.0)
                {
                    connectionInfo.UserField = xmlnode.GetAttributeAsString("UserField");
                    connectionInfo.Inheritance.UserField = xmlnode.GetAttributeAsBool("InheritUserField");
                }

                if (_confVersion >= 2.1)
                {
                    connectionInfo.ExtApp = xmlnode.GetAttributeAsString("ExtApp");
                    connectionInfo.Inheritance.ExtApp = xmlnode.GetAttributeAsBool("InheritExtApp");
                }

                if (_confVersion >= 2.2)
                {
                    // Get settings
                    connectionInfo.RDGatewayUsageMethod = GetRdGatewayUsageMethod(xmlnode);
                    connectionInfo.RDGatewayHostname = xmlnode.GetAttributeAsString("RDGatewayHostname");
                    connectionInfo.RDGatewayUseConnectionCredentials = xmlnode.GetAttributeAsEnum<RDGatewayUseConnectionCredentials>("RDGatewayUseConnectionCredentials");
                    connectionInfo.RDGatewayUsername = xmlnode.GetAttributeAsString("RDGatewayUsername");
                    connectionInfo.RDGatewayPassword = DecryptField(xmlnode, "RDGatewayPassword");
                    connectionInfo.RDGatewayDomain = xmlnode.GetAttributeAsString("RDGatewayDomain");

                    // Get inheritance settings
                    connectionInfo.Inheritance.RDGatewayUsageMethod = xmlnode.GetAttributeAsBool("InheritRDGatewayUsageMethod");
                    connectionInfo.Inheritance.RDGatewayHostname = xmlnode.GetAttributeAsBool("InheritRDGatewayHostname");
                    connectionInfo.Inheritance.RDGatewayUseConnectionCredentials = xmlnode.GetAttributeAsBool("InheritRDGatewayUseConnectionCredentials");
                    connectionInfo.Inheritance.RDGatewayUsername = xmlnode.GetAttributeAsBool("InheritRDGatewayUsername");
                    connectionInfo.Inheritance.RDGatewayPassword = xmlnode.GetAttributeAsBool("InheritRDGatewayPassword");
                    connectionInfo.Inheritance.RDGatewayDomain = xmlnode.GetAttributeAsBool("InheritRDGatewayDomain");
                }

                if (_confVersion >= 2.3)
                {
                    // Get settings
                    connectionInfo.EnableFontSmoothing = xmlnode.GetAttributeAsBool("EnableFontSmoothing");
                    connectionInfo.EnableDesktopComposition = xmlnode.GetAttributeAsBool("EnableDesktopComposition");

                    // Get inheritance settings
                    connectionInfo.Inheritance.EnableFontSmoothing = xmlnode.GetAttributeAsBool("InheritEnableFontSmoothing");
                    connectionInfo.Inheritance.EnableDesktopComposition = xmlnode.GetAttributeAsBool("InheritEnableDesktopComposition");
                }

                if (_confVersion >= 2.4)
                {
                    connectionInfo.UseCredSsp = xmlnode.GetAttributeAsBool("UseCredSsp");
                    connectionInfo.Inheritance.UseCredSsp = xmlnode.GetAttributeAsBool("InheritUseCredSsp");
                }

                if (_confVersion >= 2.5)
                {
                    connectionInfo.LoadBalanceInfo = xmlnode.GetAttributeAsString("LoadBalanceInfo");
                    connectionInfo.AutomaticResize = xmlnode.GetAttributeAsBool("AutomaticResize");
                    connectionInfo.Inheritance.LoadBalanceInfo = xmlnode.GetAttributeAsBool("InheritLoadBalanceInfo");
                    connectionInfo.Inheritance.AutomaticResize = xmlnode.GetAttributeAsBool("InheritAutomaticResize");
                }

                if (_confVersion >= 2.6)
                {
                    connectionInfo.SoundQuality = xmlnode.GetAttributeAsEnum<RDPSoundQuality>("SoundQuality");
                    connectionInfo.Inheritance.SoundQuality = xmlnode.GetAttributeAsBool("InheritSoundQuality");
                    connectionInfo.RDPMinutesToIdleTimeout = xmlnode.GetAttributeAsInt("RDPMinutesToIdleTimeout");
                    connectionInfo.Inheritance.RDPMinutesToIdleTimeout = xmlnode.GetAttributeAsBool("InheritRDPMinutesToIdleTimeout");
                    connectionInfo.RDPAlertIdleTimeout = xmlnode.GetAttributeAsBool("RDPAlertIdleTimeout");
                    connectionInfo.Inheritance.RDPAlertIdleTimeout = xmlnode.GetAttributeAsBool("InheritRDPAlertIdleTimeout");
                }

                if (_confVersion >= 2.7)
                {
                    connectionInfo.RedirectClipboard = xmlnode.GetAttributeAsBool("RedirectClipboard");
                    connectionInfo.Favorite = xmlnode.GetAttributeAsBool("Favorite");
                    connectionInfo.UseVmId = xmlnode.GetAttributeAsBool("UseVmId");
                    connectionInfo.VmId = xmlnode.GetAttributeAsString("VmId");
                    connectionInfo.UseEnhancedMode = xmlnode.GetAttributeAsBool("UseEnhancedMode");
                    connectionInfo.RdpVersion = xmlnode.GetAttributeAsEnum("RdpVersion", RdpVersion.Highest);
                    connectionInfo.SSHTunnelConnectionName = xmlnode.GetAttributeAsString("SSHTunnelConnectionName");
                    connectionInfo.OpeningCommand = xmlnode.GetAttributeAsString("OpeningCommand");
                    connectionInfo.SSHOptions = xmlnode.GetAttributeAsString("SSHOptions");
                    connectionInfo.PrivateKeyPath = xmlnode.GetAttributeAsString("PrivateKeyPath");
                    connectionInfo.RDPStartProgram = xmlnode.GetAttributeAsString("StartProgram");
                    connectionInfo.RDPStartProgramWorkDir = xmlnode.GetAttributeAsString("StartProgramWorkDir");
                    connectionInfo.DisableFullWindowDrag = xmlnode.GetAttributeAsBool("DisableFullWindowDrag");
                    connectionInfo.DisableMenuAnimations = xmlnode.GetAttributeAsBool("DisableMenuAnimations");
                    connectionInfo.DisableCursorShadow = xmlnode.GetAttributeAsBool("DisableCursorShadow");
                    connectionInfo.DisableCursorBlinking = xmlnode.GetAttributeAsBool("DisableCursorBlinking");
                    connectionInfo.RDPStartProgram = xmlnode.GetAttributeAsString("StartProgram");
                    connectionInfo.RDPStartProgramWorkDir = xmlnode.GetAttributeAsString("StartProgramWorkDir");
                    connectionInfo.Inheritance.RedirectClipboard = xmlnode.GetAttributeAsBool("InheritRedirectClipboard");
                    connectionInfo.Inheritance.Favorite = xmlnode.GetAttributeAsBool("InheritFavorite");
                    connectionInfo.Inheritance.RdpVersion = xmlnode.GetAttributeAsBool("InheritRdpVersion");
                    connectionInfo.Inheritance.UseVmId = xmlnode.GetAttributeAsBool("InheritUseVmId");
                    connectionInfo.Inheritance.VmId = xmlnode.GetAttributeAsBool("InheritVmId");
                    connectionInfo.Inheritance.UseEnhancedMode = xmlnode.GetAttributeAsBool("InheritUseEnhancedMode");
                    connectionInfo.Inheritance.SSHTunnelConnectionName = xmlnode.GetAttributeAsBool("InheritSSHTunnelConnectionName");
                    connectionInfo.Inheritance.OpeningCommand = xmlnode.GetAttributeAsBool("InheritOpeningCommand");
                    connectionInfo.Inheritance.SSHOptions = xmlnode.GetAttributeAsBool("InheritSSHOptions");
                    connectionInfo.Inheritance.PrivateKeyPath = xmlnode.GetAttributeAsBool("InheritPrivateKeyPath");
                    connectionInfo.Inheritance.DisableFullWindowDrag = xmlnode.GetAttributeAsBool("InheritDisableFullWindowDrag");
                    connectionInfo.Inheritance.DisableMenuAnimations = xmlnode.GetAttributeAsBool("InheritDisableMenuAnimations");
                    connectionInfo.Inheritance.DisableCursorShadow = xmlnode.GetAttributeAsBool("InheritDisableCursorShadow");
                    connectionInfo.Inheritance.DisableCursorBlinking = xmlnode.GetAttributeAsBool("InheritDisableCursorBlinking");
                    connectionInfo.ExternalCredentialProvider = xmlnode.GetAttributeAsEnum("ExternalCredentialProvider", ExternalCredentialProvider.None);
                    connectionInfo.Inheritance.ExternalCredentialProvider = xmlnode.GetAttributeAsBool("InheritExternalCredentialProvider");
                    connectionInfo.UserViaAPI = xmlnode.GetAttributeAsString("UserViaAPI");
                    connectionInfo.Inheritance.UserViaAPI = xmlnode.GetAttributeAsBool("InheritUserViaAPI");
                    connectionInfo.ExternalAddressProvider = xmlnode.GetAttributeAsEnum("ExternalAddressProvider", ExternalAddressProvider.None);
                    connectionInfo.VaultOpenbaoMount = xmlnode.GetAttributeAsString("VaultOpenbaoMount");
                    connectionInfo.VaultOpenbaoRole = xmlnode.GetAttributeAsString("VaultOpenbaoRole");
                    connectionInfo.VaultOpenbaoSecretEngine = xmlnode.GetAttributeAsEnum("VaultOpenbaoSecretEngine", VaultOpenbaoSecretEngine.Kv);
                    connectionInfo.EC2InstanceId = xmlnode.GetAttributeAsString("EC2InstanceId");
                    connectionInfo.EC2Region = xmlnode.GetAttributeAsString("EC2Region");
                    connectionInfo.UseRestrictedAdmin = xmlnode.GetAttributeAsBool("UseRestrictedAdmin");
                    connectionInfo.Inheritance.UseRestrictedAdmin = xmlnode.GetAttributeAsBool("InheritUseRestrictedAdmin");
                    connectionInfo.UseRCG = xmlnode.GetAttributeAsBool("UseRCG");
                    connectionInfo.Inheritance.UseRCG = xmlnode.GetAttributeAsBool("InheritUseRCG");
                    connectionInfo.RDGatewayExternalCredentialProvider = xmlnode.GetAttributeAsEnum("RDGatewayExternalCredentialProvider", ExternalCredentialProvider.None);
                    connectionInfo.RDGatewayUserViaAPI = xmlnode.GetAttributeAsString("RDGatewayUserViaAPI");
                    connectionInfo.RDGatewayAccessToken = xmlnode.GetAttributeAsString("RDGatewayAccessToken");
                    connectionInfo.Inheritance.RDGatewayExternalCredentialProvider = xmlnode.GetAttributeAsBool("InheritRDGatewayExternalCredentialProvider");
                    connectionInfo.Inheritance.RDGatewayUserViaAPI = xmlnode.GetAttributeAsBool("InheritRDGatewayUserViaAPI");
                }

                if (_confVersion >= 2.8)
                {
                    // Get settings
                    connectionInfo.IsRoot = xmlnode.GetAttributeAsBool("IsRoot");
                    connectionInfo.IsTemplate = xmlnode.GetAttributeAsBool("IsTemplate");
                    connectionInfo.UsePersistentBrowser = xmlnode.GetAttributeAsBool("UsePersistentBrowser");
                    connectionInfo.ScriptErrorsSuppressed = xmlnode.GetAttributeAsBool("ScriptErrorsSuppressed", true);
                    connectionInfo.Inheritance.ScriptErrorsSuppressed = xmlnode.GetAttributeAsBool("InheritScriptErrorsSuppressed");
                    connectionInfo.DesktopScaleFactor = xmlnode.GetAttributeAsEnum<RDPDesktopScaleFactor>("DesktopScaleFactor");
                    connectionInfo.Inheritance.DesktopScaleFactor = xmlnode.GetAttributeAsBool("InheritDesktopScaleFactor");
                    connectionInfo.RDPSignScope = xmlnode.GetAttributeAsString("RDPSignScope");
                    connectionInfo.RDPSignature = xmlnode.GetAttributeAsString("RDPSignature");
                    connectionInfo.Inheritance.RDPSignScope = xmlnode.GetAttributeAsBool("InheritRDPSignScope");
                    connectionInfo.Inheritance.RDPSignature = xmlnode.GetAttributeAsBool("InheritRDPSignature");
                    connectionInfo.IPAddress = xmlnode.GetAttributeAsString("IPAddress");
                    connectionInfo.ConnectionAddressPrimary = xmlnode.GetAttributeAsEnum<ConnectionAddressPrimary>("ConnectionAddressPrimary");
                    connectionInfo.RDPSizingMode = xmlnode.GetAttributeAsEnum<RDPSizingMode>("RDPSizingMode");
                    connectionInfo.ResolutionWidth = xmlnode.GetAttributeAsInt("ResolutionWidth");
                    connectionInfo.ResolutionHeight = xmlnode.GetAttributeAsInt("ResolutionHeight");
                    connectionInfo.RDPUseMultimon = xmlnode.GetAttributeAsBool("RDPUseMultimon");
                    connectionInfo.Notes = xmlnode.GetAttributeAsString("Notes");
                    connectionInfo.RetryOnFirstConnect = xmlnode.GetAttributeAsBool("RetryOnFirstConnect");
                    connectionInfo.WaitForIPAvailability = xmlnode.GetAttributeAsBool("WaitForIPAvailability");
                    connectionInfo.WaitForIPTimeout = xmlnode.GetAttributeAsInt("WaitForIPTimeout");
                    connectionInfo.ShowBrowserNavigationBar = xmlnode.GetAttributeAsBool("ShowBrowserNavigationBar");
                    connectionInfo.HttpPath = xmlnode.GetAttributeAsString("HttpPath");
                    connectionInfo.AlwaysPromptForCredentials = xmlnode.GetAttributeAsBool("AlwaysPromptForCredentials");
                    connectionInfo.Inheritance.IPAddress = xmlnode.GetAttributeAsBool("InheritIPAddress");
                    connectionInfo.Inheritance.ConnectionAddressPrimary = xmlnode.GetAttributeAsBool("InheritConnectionAddressPrimary");
                    connectionInfo.Inheritance.RDPSizingMode = xmlnode.GetAttributeAsBool("InheritRDPSizingMode");
                    connectionInfo.Inheritance.ResolutionWidth = xmlnode.GetAttributeAsBool("InheritResolutionWidth");
                    connectionInfo.Inheritance.ResolutionHeight = xmlnode.GetAttributeAsBool("InheritResolutionHeight");
                    connectionInfo.Inheritance.RDPUseMultimon = xmlnode.GetAttributeAsBool("InheritRDPUseMultimon");
                    connectionInfo.Inheritance.Notes = xmlnode.GetAttributeAsBool("InheritNotes");
                    connectionInfo.Inheritance.RetryOnFirstConnect = xmlnode.GetAttributeAsBool("InheritRetryOnFirstConnect");
                    connectionInfo.Inheritance.WaitForIPAvailability = xmlnode.GetAttributeAsBool("InheritWaitForIPAvailability");
                    connectionInfo.Inheritance.WaitForIPTimeout = xmlnode.GetAttributeAsBool("InheritWaitForIPTimeout");
                    connectionInfo.CredentialId = xmlnode.GetAttributeAsString("CredentialId");
                }

                switch (_confVersion)
                {
                    case >= 2.8:
                        connectionInfo.RedirectDiskDrives = xmlnode.GetAttributeAsEnum<RDPDiskDrives>("RedirectDiskDrives");
                        connectionInfo.RedirectDiskDrivesCustom = xmlnode.GetAttributeAsString("RedirectDiskDrivesCustom");
                        connectionInfo.Inheritance.RedirectDiskDrivesCustom = xmlnode.GetAttributeAsBool("InheritRedirectDiskDrivesCustom");
                        connectionInfo.EnvironmentTags = xmlnode.GetAttributeAsString("EnvironmentTags");
                        connectionInfo.Inheritance.EnvironmentTags = xmlnode.GetAttributeAsBool("InheritEnvironmentTags");
                        connectionInfo.Inheritance.AutoSort = xmlnode.GetAttributeAsBool("InheritAutoSort");
                        connectionInfo.UserField1 = xmlnode.GetAttributeAsString("UserField1");
                        connectionInfo.UserField2 = xmlnode.GetAttributeAsString("UserField2");
                        connectionInfo.UserField3 = xmlnode.GetAttributeAsString("UserField3");
                        connectionInfo.UserField4 = xmlnode.GetAttributeAsString("UserField4");
                        connectionInfo.UserField5 = xmlnode.GetAttributeAsString("UserField5");
                        connectionInfo.UserField6 = xmlnode.GetAttributeAsString("UserField6");
                        connectionInfo.UserField7 = xmlnode.GetAttributeAsString("UserField7");
                        connectionInfo.UserField8 = xmlnode.GetAttributeAsString("UserField8");
                        connectionInfo.UserField9 = xmlnode.GetAttributeAsString("UserField9");
                        connectionInfo.UserField10 = xmlnode.GetAttributeAsString("UserField10");
                        connectionInfo.Inheritance.UserField1 = xmlnode.GetAttributeAsBool("InheritUserField1");
                        connectionInfo.Inheritance.UserField2 = xmlnode.GetAttributeAsBool("InheritUserField2");
                        connectionInfo.Inheritance.UserField3 = xmlnode.GetAttributeAsBool("InheritUserField3");
                        connectionInfo.Inheritance.UserField4 = xmlnode.GetAttributeAsBool("InheritUserField4");
                        connectionInfo.Inheritance.UserField5 = xmlnode.GetAttributeAsBool("InheritUserField5");
                        connectionInfo.Inheritance.UserField6 = xmlnode.GetAttributeAsBool("InheritUserField6");
                        connectionInfo.Inheritance.UserField7 = xmlnode.GetAttributeAsBool("InheritUserField7");
                        connectionInfo.Inheritance.UserField8 = xmlnode.GetAttributeAsBool("InheritUserField8");
                        connectionInfo.Inheritance.UserField9 = xmlnode.GetAttributeAsBool("InheritUserField9");
                        connectionInfo.Inheritance.UserField10 = xmlnode.GetAttributeAsBool("InheritUserField10");
                        connectionInfo.Inheritance.Hostname = xmlnode.GetAttributeAsBool("InheritHostname");
                        connectionInfo.Inheritance.AlternativeAddress = xmlnode.GetAttributeAsBool("InheritAlternativeAddress");
                        break;

                    case >= 0.5:
                    {
                        // used to be boolean
                        bool tmpRedirect = xmlnode.GetAttributeAsBool("RedirectDiskDrives");
                        connectionInfo.RedirectDiskDrives = tmpRedirect ? RDPDiskDrives.Local : RDPDiskDrives.None;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, string.Format(CultureInfo.InvariantCulture, Language.GetConnectionInfoFromXmlFailed, connectionInfo.Name, ConnectionFileName, ex.Message));
            }

            return connectionInfo;
        }

        private string DecryptField(XmlNode xmlNode, string attributeName)
        {
            string cipherText = xmlNode.GetAttributeAsString(attributeName);
            if (string.IsNullOrEmpty(cipherText))
                return string.Empty;
            return _decryptor.Decrypt(cipherText);
        }

        private static RDGatewayUsageMethod GetRdGatewayUsageMethod(XmlNode xmlNode)
        {
            string value = xmlNode.GetAttributeAsString("RDGatewayUsageMethod");
            if (string.IsNullOrWhiteSpace(value))
                return RDGatewayUsageMethod.Never;

            if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int numericValue))
            {
                return numericValue switch
                {
                    0 => RDGatewayUsageMethod.Never,
                    1 => RDGatewayUsageMethod.Always,
                    2 => RDGatewayUsageMethod.Detect,
                    // Legacy .rdp imports can carry value 4 (do not use RD Gateway, bypass local addresses),
                    // which is unsupported by our enum and should behave as "Never".
                    4 => RDGatewayUsageMethod.Never,
                    _ => RDGatewayUsageMethod.Never,
                };
            }

            if (Enum.TryParse(value, true, out RDGatewayUsageMethod parsedValue) &&
                Enum.IsDefined<RDGatewayUsageMethod>(parsedValue))
            {
                return parsedValue;
            }

            return RDGatewayUsageMethod.Never;
        }
    }
}
