﻿using System;
using System.Globalization;
using System.Windows.Forms;
using System.Xml;
using mRemoteNG.App;
using mRemoteNG.Config.Connections;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.ICA;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.TaskDialog;

namespace mRemoteNG.Config.Serializers
{
    public class XmlConnectionsDeserializer : IDeserializer
    {
        private const double MaxSupportedConfVersion = 2.6;
        private double _confVersion;
        private ConnectionsDecryptor _decryptor;
        private XmlDocument _xmlDocument;
        //TODO find way to inject data source info
        private readonly string ConnectionFileName = "";


        public XmlConnectionsDeserializer(string xml)
        {
            LoadXmlConnectionData(xml);
            ValidateConnectionFileVersion();
        }

        public ConnectionTreeModel Deserialize()
        {
            return Deserialize(false);
        }

        private void LoadXmlConnectionData(string connections)
        {
            _decryptor = new ConnectionsDecryptor();
            connections = _decryptor.LegacyFullFileDecrypt(connections);
            _xmlDocument = new XmlDocument();
            if (connections != "")
                _xmlDocument.LoadXml(connections);
        }

        private void ValidateConnectionFileVersion()
        {
            if ((_xmlDocument.DocumentElement != null) && _xmlDocument.DocumentElement.HasAttribute("ConfVersion"))
                _confVersion =
                    Convert.ToDouble(_xmlDocument.DocumentElement.Attributes["ConfVersion"].Value.Replace(",", "."),
                        CultureInfo.InvariantCulture);
            else
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, Language.strOldConffile);

            if (!(_confVersion > MaxSupportedConfVersion)) return;
            ShowIncompatibleVersionDialogBox();
            throw new Exception($"Incompatible connection file format (file format version {_confVersion}).");
        }

        private void ShowIncompatibleVersionDialogBox()
        {
            CTaskDialog.ShowTaskDialogBox(
                frmMain.Default,
                Application.ProductName,
                "Incompatible connection file format",
                $"The format of this connection file is not supported. Please upgrade to a newer version of {Application.ProductName}.",
                string.Format("{1}{0}File Format Version: {2}{0}Highest Supported Version: {3}", Environment.NewLine,
                    ConnectionFileName, _confVersion, MaxSupportedConfVersion),
                "",
                "",
                "",
                "",
                ETaskDialogButtons.Ok,
                ESysIcons.Error,
                ESysIcons.Error
            );
        }

        public ConnectionTreeModel Deserialize(bool import)
        {
            try
            {
                if (!import)
                    Runtime.IsConnectionsFileLoaded = false;

                var rootXmlElement = _xmlDocument.DocumentElement;
                CreateDecryptor(rootXmlElement);
                var rootInfo = InitializeRootNode(rootXmlElement);
                var connectionTreeModel = new ConnectionTreeModel();
                connectionTreeModel.AddRootNode(rootInfo);


                if (_confVersion > 1.3)
                {
                    var protectedString = _xmlDocument.DocumentElement?.Attributes["Protected"].Value;
                    if (!_decryptor.ConnectionsFileIsAuthentic(protectedString, rootInfo))
                    {
                        mRemoteNG.Settings.Default.LoadConsFromCustomLocation = false;
                        mRemoteNG.Settings.Default.CustomConsPath = "";
                        return null;
                    }
                }

                if (_confVersion >= 2.6)
                    if (rootXmlElement?.Attributes["FullFileEncryption"].Value == "True")
                    {
                        var decryptedContent = _decryptor.Decrypt(rootXmlElement.InnerText);
                        rootXmlElement.InnerXml = decryptedContent;
                    }

                if (import && !IsExportFile(rootXmlElement))
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                        Language.strCannotImportNormalSessionFile);
                    return null;
                }

                AddNodesFromXmlRecursive(_xmlDocument.DocumentElement, rootInfo);

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

        private RootNodeInfo InitializeRootNode(XmlElement connectionsRootElement)
        {
            var rootNodeName = connectionsRootElement?.Attributes["Name"].Value.Trim();
            var rootInfo = new RootNodeInfo(RootNodeType.Connection)
            {
                Name = rootNodeName
            };
            return rootInfo;
        }

        private void CreateDecryptor(XmlElement connectionsRootElement)
        {
            if (_confVersion >= 2.6)
            {
                BlockCipherEngines engine;
                Enum.TryParse(connectionsRootElement.Attributes["EncryptionEngine"].Value, out engine);

                BlockCipherModes mode;
                Enum.TryParse(connectionsRootElement.Attributes["BlockCipherMode"].Value, out mode);

                _decryptor = new ConnectionsDecryptor(engine, mode);
            }
            else
            {
                _decryptor = new ConnectionsDecryptor();
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
                    var nodeType = (TreeNodeType) Enum.Parse(typeof(TreeNodeType), treeNodeTypeString, true);

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
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            try
            {
                if (_confVersion >= 0.2)
                {
                    connectionInfo.Name = xmlnode.Attributes["Name"].Value;
                    connectionInfo.Description = xmlnode.Attributes["Descr"].Value;
                    connectionInfo.Hostname = xmlnode.Attributes["Hostname"].Value;
                    connectionInfo.Username = xmlnode.Attributes["Username"].Value;
                    connectionInfo.Password = cryptographyProvider.Decrypt(xmlnode.Attributes["Password"].Value,
                        Runtime.EncryptionKey);
                    connectionInfo.Domain = xmlnode.Attributes["Domain"].Value;
                    connectionInfo.DisplayWallpaper = bool.Parse(xmlnode.Attributes["DisplayWallpaper"].Value);
                    connectionInfo.DisplayThemes = bool.Parse(xmlnode.Attributes["DisplayThemes"].Value);
                    connectionInfo.CacheBitmaps = bool.Parse(xmlnode.Attributes["CacheBitmaps"].Value);

                    if (_confVersion < 1.1) //1.0 - 0.1
                        connectionInfo.Resolution = Convert.ToBoolean(xmlnode.Attributes["Fullscreen"].Value)
                            ? ProtocolRDP.RDPResolutions.Fullscreen
                            : ProtocolRDP.RDPResolutions.FitToWindow;
                }

                if (_confVersion >= 0.3)
                {
                    if (_confVersion < 0.7)
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
                else
                {
                    connectionInfo.Port = (int) ProtocolRDP.Defaults.Port;
                    connectionInfo.Protocol = ProtocolType.RDP;
                }

                if (_confVersion >= 0.4)
                {
                    if (_confVersion < 0.7)
                        if (Convert.ToBoolean(xmlnode.Attributes["UseVNC"].Value))
                            connectionInfo.Port = Convert.ToInt32(xmlnode.Attributes["VNCPort"].Value);
                        else
                            connectionInfo.Port = Convert.ToInt32(xmlnode.Attributes["RDPPort"].Value);

                    connectionInfo.UseConsoleSession = bool.Parse(xmlnode.Attributes["ConnectToConsole"].Value);
                }
                else
                {
                    if (_confVersion < 0.7)
                        if (Convert.ToBoolean(xmlnode.Attributes["UseVNC"].Value))
                            connectionInfo.Port = (int) ProtocolVNC.Defaults.Port;
                        else
                            connectionInfo.Port = (int) ProtocolRDP.Defaults.Port;
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
                    connectionInfo.Protocol =
                        (ProtocolType)
                        MiscTools.StringToEnum(typeof(ProtocolType), xmlnode.Attributes["Protocol"].Value);
                    connectionInfo.Port = Convert.ToInt32(xmlnode.Attributes["Port"].Value);
                }

                if (_confVersion >= 1.0)
                    connectionInfo.RedirectKeys = bool.Parse(xmlnode.Attributes["RedirectKeys"].Value);

                if (_confVersion >= 1.2)
                    connectionInfo.PuttySession = xmlnode.Attributes["PuttySession"].Value;

                if (_confVersion >= 1.3)
                {
                    connectionInfo.Colors =
                        (ProtocolRDP.RDPColors)
                        MiscTools.StringToEnum(typeof(ProtocolRDP.RDPColors), xmlnode.Attributes["Colors"].Value);
                    connectionInfo.Resolution =
                        (ProtocolRDP.RDPResolutions)
                        MiscTools.StringToEnum(typeof(ProtocolRDP.RDPResolutions),
                            Convert.ToString(xmlnode.Attributes["Resolution"].Value));
                    connectionInfo.RedirectSound =
                        (ProtocolRDP.RDPSounds)
                        MiscTools.StringToEnum(typeof(ProtocolRDP.RDPSounds),
                            Convert.ToString(xmlnode.Attributes["RedirectSound"].Value));
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

                    connectionInfo.RedirectSound =
                        (ProtocolRDP.RDPSounds) Convert.ToInt32(xmlnode.Attributes["RedirectSound"].Value);
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
                        Domain = bool.Parse(xmlnode.Attributes["InheritDomain"].Value),
                        Icon = bool.Parse(xmlnode.Attributes["InheritIcon"].Value),
                        Panel = bool.Parse(xmlnode.Attributes["InheritPanel"].Value),
                        Password = bool.Parse(xmlnode.Attributes["InheritPassword"].Value),
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
                        Username = bool.Parse(xmlnode.Attributes["InheritUsername"].Value)
                    };
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
                    connectionInfo.PleaseConnect = bool.Parse(xmlnode.Attributes["Connected"].Value);

                if (_confVersion >= 1.6)
                {
                    connectionInfo.ICAEncryptionStrength =
                        (ProtocolICA.EncryptionStrength)
                        MiscTools.StringToEnum(typeof(ProtocolICA.EncryptionStrength),
                            xmlnode.Attributes["ICAEncryptionStrength"].Value);
                    connectionInfo.Inheritance.ICAEncryptionStrength =
                        bool.Parse(xmlnode.Attributes["InheritICAEncryptionStrength"].Value);
                    connectionInfo.PreExtApp = xmlnode.Attributes["PreExtApp"].Value;
                    connectionInfo.PostExtApp = xmlnode.Attributes["PostExtApp"].Value;
                    connectionInfo.Inheritance.PreExtApp = bool.Parse(xmlnode.Attributes["InheritPreExtApp"].Value);
                    connectionInfo.Inheritance.PostExtApp = bool.Parse(xmlnode.Attributes["InheritPostExtApp"].Value);
                }

                if (_confVersion >= 1.7)
                {
                    connectionInfo.VNCCompression =
                        (ProtocolVNC.Compression)
                        MiscTools.StringToEnum(typeof(ProtocolVNC.Compression),
                            xmlnode.Attributes["VNCCompression"].Value);
                    connectionInfo.VNCEncoding =
                        (ProtocolVNC.Encoding)
                        MiscTools.StringToEnum(typeof(ProtocolVNC.Encoding),
                            Convert.ToString(xmlnode.Attributes["VNCEncoding"].Value));
                    connectionInfo.VNCAuthMode =
                        (ProtocolVNC.AuthMode)
                        MiscTools.StringToEnum(typeof(ProtocolVNC.AuthMode), xmlnode.Attributes["VNCAuthMode"].Value);
                    connectionInfo.VNCProxyType =
                        (ProtocolVNC.ProxyType)
                        MiscTools.StringToEnum(typeof(ProtocolVNC.ProxyType), xmlnode.Attributes["VNCProxyType"].Value);
                    connectionInfo.VNCProxyIP = xmlnode.Attributes["VNCProxyIP"].Value;
                    connectionInfo.VNCProxyPort = Convert.ToInt32(xmlnode.Attributes["VNCProxyPort"].Value);
                    connectionInfo.VNCProxyUsername = xmlnode.Attributes["VNCProxyUsername"].Value;
                    connectionInfo.VNCProxyPassword =
                        cryptographyProvider.Decrypt(xmlnode.Attributes["VNCProxyPassword"].Value, Runtime.EncryptionKey);
                    connectionInfo.VNCColors =
                        (ProtocolVNC.Colors)
                        MiscTools.StringToEnum(typeof(ProtocolVNC.Colors), xmlnode.Attributes["VNCColors"].Value);
                    connectionInfo.VNCSmartSizeMode =
                        (ProtocolVNC.SmartSizeMode)
                        MiscTools.StringToEnum(typeof(ProtocolVNC.SmartSizeMode),
                            xmlnode.Attributes["VNCSmartSizeMode"].Value);
                    connectionInfo.VNCViewOnly = bool.Parse(xmlnode.Attributes["VNCViewOnly"].Value);
                    connectionInfo.Inheritance.VNCCompression =
                        bool.Parse(xmlnode.Attributes["InheritVNCCompression"].Value);
                    connectionInfo.Inheritance.VNCEncoding = bool.Parse(xmlnode.Attributes["InheritVNCEncoding"].Value);
                    connectionInfo.Inheritance.VNCAuthMode = bool.Parse(xmlnode.Attributes["InheritVNCAuthMode"].Value);
                    connectionInfo.Inheritance.VNCProxyType = bool.Parse(xmlnode.Attributes["InheritVNCProxyType"].Value);
                    connectionInfo.Inheritance.VNCProxyIP = bool.Parse(xmlnode.Attributes["InheritVNCProxyIP"].Value);
                    connectionInfo.Inheritance.VNCProxyPort = bool.Parse(xmlnode.Attributes["InheritVNCProxyPort"].Value);
                    connectionInfo.Inheritance.VNCProxyUsername =
                        bool.Parse(xmlnode.Attributes["InheritVNCProxyUsername"].Value);
                    connectionInfo.Inheritance.VNCProxyPassword =
                        bool.Parse(xmlnode.Attributes["InheritVNCProxyPassword"].Value);
                    connectionInfo.Inheritance.VNCColors = bool.Parse(xmlnode.Attributes["InheritVNCColors"].Value);
                    connectionInfo.Inheritance.VNCSmartSizeMode =
                        bool.Parse(xmlnode.Attributes["InheritVNCSmartSizeMode"].Value);
                    connectionInfo.Inheritance.VNCViewOnly = bool.Parse(xmlnode.Attributes["InheritVNCViewOnly"].Value);
                }

                if (_confVersion >= 1.8)
                {
                    connectionInfo.RDPAuthenticationLevel =
                        (ProtocolRDP.AuthenticationLevel)
                        MiscTools.StringToEnum(typeof(ProtocolRDP.AuthenticationLevel),
                            xmlnode.Attributes["RDPAuthenticationLevel"].Value);
                    connectionInfo.Inheritance.RDPAuthenticationLevel =
                        bool.Parse(xmlnode.Attributes["InheritRDPAuthenticationLevel"].Value);
                }

                if (_confVersion >= 1.9)
                {
                    connectionInfo.RenderingEngine =
                        (HTTPBase.RenderingEngine)
                        MiscTools.StringToEnum(typeof(HTTPBase.RenderingEngine),
                            xmlnode.Attributes["RenderingEngine"].Value);
                    connectionInfo.MacAddress = xmlnode.Attributes["MacAddress"].Value;
                    connectionInfo.Inheritance.RenderingEngine =
                        bool.Parse(xmlnode.Attributes["InheritRenderingEngine"].Value);
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
                    connectionInfo.RDGatewayUsageMethod =
                        (ProtocolRDP.RDGatewayUsageMethod)
                        MiscTools.StringToEnum(typeof(ProtocolRDP.RDGatewayUsageMethod),
                            Convert.ToString(xmlnode.Attributes["RDGatewayUsageMethod"].Value));
                    connectionInfo.RDGatewayHostname = xmlnode.Attributes["RDGatewayHostname"].Value;
                    connectionInfo.RDGatewayUseConnectionCredentials =
                        (ProtocolRDP.RDGatewayUseConnectionCredentials)
                        MiscTools.StringToEnum(typeof(ProtocolRDP.RDGatewayUseConnectionCredentials),
                            Convert.ToString(xmlnode.Attributes["RDGatewayUseConnectionCredentials"].Value));
                    connectionInfo.RDGatewayUsername = xmlnode.Attributes["RDGatewayUsername"].Value;
                    connectionInfo.RDGatewayPassword =
                        cryptographyProvider.Decrypt(Convert.ToString(xmlnode.Attributes["RDGatewayPassword"].Value),
                            Runtime.EncryptionKey);
                    connectionInfo.RDGatewayDomain = xmlnode.Attributes["RDGatewayDomain"].Value;

                    // Get inheritance settings
                    connectionInfo.Inheritance.RDGatewayUsageMethod =
                        bool.Parse(xmlnode.Attributes["InheritRDGatewayUsageMethod"].Value);
                    connectionInfo.Inheritance.RDGatewayHostname =
                        bool.Parse(xmlnode.Attributes["InheritRDGatewayHostname"].Value);
                    connectionInfo.Inheritance.RDGatewayUseConnectionCredentials =
                        bool.Parse(xmlnode.Attributes["InheritRDGatewayUseConnectionCredentials"].Value);
                    connectionInfo.Inheritance.RDGatewayUsername =
                        bool.Parse(xmlnode.Attributes["InheritRDGatewayUsername"].Value);
                    connectionInfo.Inheritance.RDGatewayPassword =
                        bool.Parse(xmlnode.Attributes["InheritRDGatewayPassword"].Value);
                    connectionInfo.Inheritance.RDGatewayDomain =
                        bool.Parse(xmlnode.Attributes["InheritRDGatewayDomain"].Value);
                }

                if (_confVersion >= 2.3)
                {
                    // Get settings
                    connectionInfo.EnableFontSmoothing = bool.Parse(xmlnode.Attributes["EnableFontSmoothing"].Value);
                    connectionInfo.EnableDesktopComposition =
                        bool.Parse(xmlnode.Attributes["EnableDesktopComposition"].Value);

                    // Get inheritance settings
                    connectionInfo.Inheritance.EnableFontSmoothing =
                        bool.Parse(xmlnode.Attributes["InheritEnableFontSmoothing"].Value);
                    connectionInfo.Inheritance.EnableDesktopComposition =
                        bool.Parse(xmlnode.Attributes["InheritEnableDesktopComposition"].Value);
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
                    connectionInfo.Inheritance.LoadBalanceInfo =
                        bool.Parse(xmlnode.Attributes["InheritLoadBalanceInfo"].Value);
                    connectionInfo.Inheritance.AutomaticResize =
                        bool.Parse(xmlnode.Attributes["InheritAutomaticResize"].Value);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                    string.Format(Language.strGetConnectionInfoFromXmlFailed, connectionInfo.Name, ConnectionFileName,
                        ex.Message));
            }
            return connectionInfo;
        }

        private bool IsExportFile(XmlElement rootConnectionsNode)
        {
            var isExportFile = false;
            if (_confVersion < 1.0) return isExportFile;
            if (Convert.ToBoolean(rootConnectionsNode.Attributes["Export"].Value))
                isExportFile = true;
            return isExportFile;
        }
    }
}