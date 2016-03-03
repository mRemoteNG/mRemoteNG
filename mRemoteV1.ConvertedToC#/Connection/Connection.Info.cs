using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using mRemoteNG.Tools.LocalizedAttributes;
using mRemoteNG.App.Runtime;
using mRemoteNG.Config;
using System.Reflection;

namespace mRemoteNG
{
    namespace Connection
    {
        [DefaultProperty("Name")]
        public partial class Info
        {
            #region "Public Properties"
            #region "Display"
            [LocalizedCategory("strCategoryDisplay", 1), LocalizedDisplayName("strPropertyNameName"), LocalizedDescription("strPropertyDescriptionName")]
            public virtual string Name { get; set; }

            private string _description = mRemoteNG.My.Settings.ConDefaultDescription;
            [LocalizedCategory("strCategoryDisplay", 1), LocalizedDisplayName("strPropertyNameDescription"), LocalizedDescription("strPropertyDescriptionDescription")]
            public virtual string Description
            {
                get { return GetInheritedPropertyValue("Description", _description); }
                set { _description = value; }
            }

            private string _icon = mRemoteNG.My.Settings.ConDefaultIcon;
            [LocalizedCategory("strCategoryDisplay", 1), TypeConverter(typeof(Icon)), LocalizedDisplayName("strPropertyNameIcon"), LocalizedDescription("strPropertyDescriptionIcon")]
            public virtual string Icon
            {
                get { return GetInheritedPropertyValue("Icon", _icon); }
                set { _icon = value; }
            }

            private string _panel = mRemoteNG.My.Language.strGeneral;
            [LocalizedCategory("strCategoryDisplay", 1), LocalizedDisplayName("strPropertyNamePanel"), LocalizedDescription("strPropertyDescriptionPanel")]
            public virtual string Panel
            {
                get { return GetInheritedPropertyValue("Panel", _panel); }
                set { _panel = value; }
            }
            #endregion
            #region "Connection"
            private string _hostname = string.Empty;
            [LocalizedCategory("strCategoryConnection", 2), LocalizedDisplayName("strPropertyNameAddress"), LocalizedDescription("strPropertyDescriptionAddress")]
            public virtual string Hostname
            {
                get { return _hostname.Trim(); }
                set
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        _hostname = string.Empty;
                    }
                    else
                    {
                        _hostname = value.Trim();
                    }
                }
            }

            private string _username = mRemoteNG.My.Settings.ConDefaultUsername;
            [LocalizedCategory("strCategoryConnection", 2), LocalizedDisplayName("strPropertyNameUsername"), LocalizedDescription("strPropertyDescriptionUsername")]
            public virtual string Username
            {
                get { return GetInheritedPropertyValue("Username", _username); }
                set { _username = value.Trim(); }
            }

            private string _password = mRemoteNG.My.Settings.ConDefaultPassword;
            [LocalizedCategory("strCategoryConnection", 2), LocalizedDisplayName("strPropertyNamePassword"), LocalizedDescription("strPropertyDescriptionPassword"), PasswordPropertyText(true)]
            public virtual string Password
            {
                get { return GetInheritedPropertyValue("Password", _password); }
                set { _password = value; }
            }

            private string _domain = mRemoteNG.My.Settings.ConDefaultDomain;
            [LocalizedCategory("strCategoryConnection", 2), LocalizedDisplayName("strPropertyNameDomain"), LocalizedDescription("strPropertyDescriptionDomain")]
            public string Domain
            {
                get { return GetInheritedPropertyValue("Domain", _domain).Trim(); }
                set { _domain = value.Trim(); }
            }
            #endregion
            #region "Protocol"
            private Protocol.Protocols _protocol = mRemoteNG.Tools.Misc.StringToEnum(typeof(Protocol.Protocols), mRemoteNG.My.Settings.ConDefaultProtocol);
            [LocalizedCategory("strCategoryProtocol", 3), LocalizedDisplayName("strPropertyNameProtocol"), LocalizedDescription("strPropertyDescriptionProtocol"), TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
            public virtual Protocol.Protocols Protocol
            {
                get { return GetInheritedPropertyValue("Protocol", _protocol); }
                set { _protocol = value; }
            }

            private string _extApp = mRemoteNG.My.Settings.ConDefaultExtApp;
            [LocalizedCategory("strCategoryProtocol", 3), LocalizedDisplayName("strPropertyNameExternalTool"), LocalizedDescription("strPropertyDescriptionExternalTool"), TypeConverter(typeof(Tools.ExternalToolsTypeConverter))]
            public string ExtApp
            {
                get { return GetInheritedPropertyValue("ExtApp", _extApp); }
                set { _extApp = value; }
            }

            private int _port;
            [LocalizedCategory("strCategoryProtocol", 3), LocalizedDisplayName("strPropertyNamePort"), LocalizedDescription("strPropertyDescriptionPort")]
            public virtual int Port
            {
                get { return GetInheritedPropertyValue("Port", _port); }
                set { _port = value; }
            }

            private string _puttySession = mRemoteNG.My.Settings.ConDefaultPuttySession;
            [LocalizedCategory("strCategoryProtocol", 3), LocalizedDisplayName("strPropertyNamePuttySession"), LocalizedDescription("strPropertyDescriptionPuttySession"), TypeConverter(typeof(Putty.Sessions.SessionList))]
            public virtual string PuttySession
            {
                get { return GetInheritedPropertyValue("PuttySession", _puttySession); }
                set { _puttySession = value; }
            }

            private Protocol.ICA.EncryptionStrength _icaEncryption = mRemoteNG.Tools.Misc.StringToEnum(typeof(Protocol.ICA.EncryptionStrength), mRemoteNG.My.Settings.ConDefaultICAEncryptionStrength);
            [LocalizedCategory("strCategoryProtocol", 3), LocalizedDisplayName("strPropertyNameEncryptionStrength"), LocalizedDescription("strPropertyDescriptionEncryptionStrength"), TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
            public Protocol.ICA.EncryptionStrength ICAEncryption
            {
                get { return GetInheritedPropertyValue("ICAEncryption", _icaEncryption); }
                set { _icaEncryption = value; }
            }

            private bool _useConsoleSession = mRemoteNG.My.Settings.ConDefaultUseConsoleSession;
            [LocalizedCategory("strCategoryProtocol", 3), LocalizedDisplayName("strPropertyNameUseConsoleSession"), LocalizedDescription("strPropertyDescriptionUseConsoleSession"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
            public bool UseConsoleSession
            {
                get { return GetInheritedPropertyValue("UseConsoleSession", _useConsoleSession); }
                set { _useConsoleSession = value; }
            }

            private Protocol.RDP.AuthenticationLevel _rdpAuthenticationLevel = mRemoteNG.Tools.Misc.StringToEnum(typeof(Protocol.RDP.AuthenticationLevel), mRemoteNG.My.Settings.ConDefaultRDPAuthenticationLevel);
            [LocalizedCategory("strCategoryProtocol", 3), LocalizedDisplayName("strPropertyNameAuthenticationLevel"), LocalizedDescription("strPropertyDescriptionAuthenticationLevel"), TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
            public Protocol.RDP.AuthenticationLevel RDPAuthenticationLevel
            {
                get { return GetInheritedPropertyValue("RDPAuthenticationLevel", _rdpAuthenticationLevel); }
                set { _rdpAuthenticationLevel = value; }
            }

            private string _loadBalanceInfo = mRemoteNG.My.Settings.ConDefaultLoadBalanceInfo;
            [LocalizedCategory("strCategoryProtocol", 3), LocalizedDisplayName("strPropertyNameLoadBalanceInfo"), LocalizedDescription("strPropertyDescriptionLoadBalanceInfo")]
            public string LoadBalanceInfo
            {
                get { return GetInheritedPropertyValue("LoadBalanceInfo", _loadBalanceInfo).Trim(); }
                set { _loadBalanceInfo = value.Trim(); }
            }

            private Protocol.HTTPBase.RenderingEngine _renderingEngine = mRemoteNG.Tools.Misc.StringToEnum(typeof(Protocol.HTTPBase.RenderingEngine), mRemoteNG.My.Settings.ConDefaultRenderingEngine);
            [LocalizedCategory("strCategoryProtocol", 3), LocalizedDisplayName("strPropertyNameRenderingEngine"), LocalizedDescription("strPropertyDescriptionRenderingEngine"), TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
            public Protocol.HTTPBase.RenderingEngine RenderingEngine
            {
                get { return GetInheritedPropertyValue("RenderingEngine", _renderingEngine); }
                set { _renderingEngine = value; }
            }

            private bool _useCredSsp = mRemoteNG.My.Settings.ConDefaultUseCredSsp;
            [LocalizedCategory("strCategoryProtocol", 3), LocalizedDisplayName("strPropertyNameUseCredSsp"), LocalizedDescription("strPropertyDescriptionUseCredSsp"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
            public bool UseCredSsp
            {
                get { return GetInheritedPropertyValue("UseCredSsp", _useCredSsp); }
                set { _useCredSsp = value; }
            }
            #endregion
            #region "RD Gateway"
            private Protocol.RDP.RDGatewayUsageMethod _rdGatewayUsageMethod = mRemoteNG.Tools.Misc.StringToEnum(typeof(Protocol.RDP.RDGatewayUsageMethod), mRemoteNG.My.Settings.ConDefaultRDGatewayUsageMethod);
            [LocalizedCategory("strCategoryGateway", 4), LocalizedDisplayName("strPropertyNameRDGatewayUsageMethod"), LocalizedDescription("strPropertyDescriptionRDGatewayUsageMethod"), TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
            public Protocol.RDP.RDGatewayUsageMethod RDGatewayUsageMethod
            {
                get { return GetInheritedPropertyValue("RDGatewayUsageMethod", _rdGatewayUsageMethod); }
                set { _rdGatewayUsageMethod = value; }
            }

            private string _rdGatewayHostname = mRemoteNG.My.Settings.ConDefaultRDGatewayHostname;
            [LocalizedCategory("strCategoryGateway", 4), LocalizedDisplayName("strPropertyNameRDGatewayHostname"), LocalizedDescription("strPropertyDescriptionRDGatewayHostname")]
            public string RDGatewayHostname
            {
                get { return GetInheritedPropertyValue("RDGatewayHostname", _rdGatewayHostname).Trim(); }
                set { _rdGatewayHostname = value.Trim(); }
            }

            private Protocol.RDP.RDGatewayUseConnectionCredentials _rdGatewayUseConnectionCredentials = mRemoteNG.Tools.Misc.StringToEnum(typeof(Protocol.RDP.RDGatewayUseConnectionCredentials), mRemoteNG.My.Settings.ConDefaultRDGatewayUseConnectionCredentials);
            [LocalizedCategory("strCategoryGateway", 4), LocalizedDisplayName("strPropertyNameRDGatewayUseConnectionCredentials"), LocalizedDescription("strPropertyDescriptionRDGatewayUseConnectionCredentials"), TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
            public Protocol.RDP.RDGatewayUseConnectionCredentials RDGatewayUseConnectionCredentials
            {
                get { return GetInheritedPropertyValue("RDGatewayUseConnectionCredentials", _rdGatewayUseConnectionCredentials); }
                set { _rdGatewayUseConnectionCredentials = value; }
            }

            private string _rdGatewayUsername = mRemoteNG.My.Settings.ConDefaultRDGatewayUsername;
            [LocalizedCategory("strCategoryGateway", 4), LocalizedDisplayName("strPropertyNameRDGatewayUsername"), LocalizedDescription("strPropertyDescriptionRDGatewayUsername")]
            public string RDGatewayUsername
            {
                get { return GetInheritedPropertyValue("RDGatewayUsername", _rdGatewayUsername).Trim(); }
                set { _rdGatewayUsername = value.Trim(); }
            }

            private string _rdGatewayPassword = mRemoteNG.My.Settings.ConDefaultRDGatewayPassword;
            [LocalizedCategory("strCategoryGateway", 4), LocalizedDisplayName("strPropertyNameRDGatewayPassword"), LocalizedDescription("strPropertyNameRDGatewayPassword"), PasswordPropertyText(true)]
            public string RDGatewayPassword
            {
                get { return GetInheritedPropertyValue("RDGatewayPassword", _rdGatewayPassword); }
                set { _rdGatewayPassword = value; }
            }

            private string _rdGatewayDomain = mRemoteNG.My.Settings.ConDefaultRDGatewayDomain;
            [LocalizedCategory("strCategoryGateway", 4), LocalizedDisplayName("strPropertyNameRDGatewayDomain"), LocalizedDescription("strPropertyDescriptionRDGatewayDomain")]
            public string RDGatewayDomain
            {
                get { return GetInheritedPropertyValue("RDGatewayDomain", _rdGatewayDomain).Trim(); }
                set { _rdGatewayDomain = value.Trim(); }
            }
            #endregion
            #region "Appearance"
            private Protocol.RDP.RDPResolutions _resolution = mRemoteNG.Tools.Misc.StringToEnum(typeof(Protocol.RDP.RDPResolutions), mRemoteNG.My.Settings.ConDefaultResolution);
            [LocalizedCategory("strCategoryAppearance", 5), LocalizedDisplayName("strPropertyNameResolution"), LocalizedDescription("strPropertyDescriptionResolution"), TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
            public Protocol.RDP.RDPResolutions Resolution
            {
                get { return GetInheritedPropertyValue("Resolution", _resolution); }
                set { _resolution = value; }
            }

            private bool _automaticResize = mRemoteNG.My.Settings.ConDefaultAutomaticResize;
            [LocalizedCategory("strCategoryAppearance", 5), LocalizedDisplayName("strPropertyNameAutomaticResize"), LocalizedDescription("strPropertyDescriptionAutomaticResize"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
            public bool AutomaticResize
            {
                get { return GetInheritedPropertyValue("AutomaticResize", _automaticResize); }
                set { _automaticResize = value; }
            }

            private Protocol.RDP.RDPColors _colors = mRemoteNG.Tools.Misc.StringToEnum(typeof(Protocol.RDP.RDPColors), mRemoteNG.My.Settings.ConDefaultColors);
            [LocalizedCategory("strCategoryAppearance", 5), LocalizedDisplayName("strPropertyNameColors"), LocalizedDescription("strPropertyDescriptionColors"), TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
            public Protocol.RDP.RDPColors Colors
            {
                get { return GetInheritedPropertyValue("Colors", _colors); }
                set { _colors = value; }
            }

            private bool _cacheBitmaps = mRemoteNG.My.Settings.ConDefaultCacheBitmaps;
            [LocalizedCategory("strCategoryAppearance", 5), LocalizedDisplayName("strPropertyNameCacheBitmaps"), LocalizedDescription("strPropertyDescriptionCacheBitmaps"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
            public bool CacheBitmaps
            {
                get { return GetInheritedPropertyValue("CacheBitmaps", _cacheBitmaps); }
                set { _cacheBitmaps = value; }
            }

            private bool _displayWallpaper = mRemoteNG.My.Settings.ConDefaultDisplayWallpaper;
            [LocalizedCategory("strCategoryAppearance", 5), LocalizedDisplayName("strPropertyNameDisplayWallpaper"), LocalizedDescription("strPropertyDescriptionDisplayWallpaper"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
            public bool DisplayWallpaper
            {
                get { return GetInheritedPropertyValue("DisplayWallpaper", _displayWallpaper); }
                set { _displayWallpaper = value; }
            }

            private bool _displayThemes = mRemoteNG.My.Settings.ConDefaultDisplayThemes;
            [LocalizedCategory("strCategoryAppearance", 5), LocalizedDisplayName("strPropertyNameDisplayThemes"), LocalizedDescription("strPropertyDescriptionDisplayThemes"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
            public bool DisplayThemes
            {
                get { return GetInheritedPropertyValue("DisplayThemes", _displayThemes); }
                set { _displayThemes = value; }
            }

            private bool _enableFontSmoothing = mRemoteNG.My.Settings.ConDefaultEnableFontSmoothing;
            [LocalizedCategory("strCategoryAppearance", 5), LocalizedDisplayName("strPropertyNameEnableFontSmoothing"), LocalizedDescription("strPropertyDescriptionEnableFontSmoothing"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
            public bool EnableFontSmoothing
            {
                get { return GetInheritedPropertyValue("EnableFontSmoothing", _enableFontSmoothing); }
                set { _enableFontSmoothing = value; }
            }

            private bool _enableDesktopComposition = mRemoteNG.My.Settings.ConDefaultEnableDesktopComposition;
            [LocalizedCategory("strCategoryAppearance", 5), LocalizedDisplayName("strPropertyNameEnableDesktopComposition"), LocalizedDescription("strPropertyDescriptionEnableDesktopComposition"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
            public bool EnableDesktopComposition
            {
                get { return GetInheritedPropertyValue("EnableDesktopComposition", _enableDesktopComposition); }
                set { _enableDesktopComposition = value; }
            }
            #endregion
            #region "Redirect"
            private bool _redirectKeys = mRemoteNG.My.Settings.ConDefaultRedirectKeys;
            [LocalizedCategory("strCategoryRedirect", 6), LocalizedDisplayName("strPropertyNameRedirectKeys"), LocalizedDescription("strPropertyDescriptionRedirectKeys"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
            public bool RedirectKeys
            {
                get { return GetInheritedPropertyValue("RedirectKeys", _redirectKeys); }
                set { _redirectKeys = value; }
            }

            private bool _redirectDiskDrives = mRemoteNG.My.Settings.ConDefaultRedirectDiskDrives;
            [LocalizedCategory("strCategoryRedirect", 6), LocalizedDisplayName("strPropertyNameRedirectDrives"), LocalizedDescription("strPropertyDescriptionRedirectDrives"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
            public bool RedirectDiskDrives
            {
                get { return GetInheritedPropertyValue("RedirectDiskDrives", _redirectDiskDrives); }
                set { _redirectDiskDrives = value; }
            }

            private bool _redirectPrinters = mRemoteNG.My.Settings.ConDefaultRedirectPrinters;
            [LocalizedCategory("strCategoryRedirect", 6), LocalizedDisplayName("strPropertyNameRedirectPrinters"), LocalizedDescription("strPropertyDescriptionRedirectPrinters"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
            public bool RedirectPrinters
            {
                get { return GetInheritedPropertyValue("RedirectPrinters", _redirectPrinters); }
                set { _redirectPrinters = value; }
            }

            private bool _redirectPorts = mRemoteNG.My.Settings.ConDefaultRedirectPorts;
            [LocalizedCategory("strCategoryRedirect", 6), LocalizedDisplayName("strPropertyNameRedirectPorts"), LocalizedDescription("strPropertyDescriptionRedirectPorts"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
            public bool RedirectPorts
            {
                get { return GetInheritedPropertyValue("RedirectPorts", _redirectPorts); }
                set { _redirectPorts = value; }
            }

            private bool _redirectSmartCards = mRemoteNG.My.Settings.ConDefaultRedirectSmartCards;
            [LocalizedCategory("strCategoryRedirect", 6), LocalizedDisplayName("strPropertyNameRedirectSmartCards"), LocalizedDescription("strPropertyDescriptionRedirectSmartCards"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
            public bool RedirectSmartCards
            {
                get { return GetInheritedPropertyValue("RedirectSmartCards", _redirectSmartCards); }
                set { _redirectSmartCards = value; }
            }

            private Protocol.RDP.RDPSounds _redirectSound = mRemoteNG.Tools.Misc.StringToEnum(typeof(Protocol.RDP.RDPSounds), mRemoteNG.My.Settings.ConDefaultRedirectSound);
            [LocalizedCategory("strCategoryRedirect", 6), LocalizedDisplayName("strPropertyNameRedirectSounds"), LocalizedDescription("strPropertyDescriptionRedirectSounds"), TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
            public Protocol.RDP.RDPSounds RedirectSound
            {
                get { return GetInheritedPropertyValue("RedirectSound", _redirectSound); }
                set { _redirectSound = value; }
            }
            #endregion
            #region "Misc"
            private string _preExtApp = mRemoteNG.My.Settings.ConDefaultPreExtApp;
            [LocalizedCategory("strCategoryMiscellaneous", 7), LocalizedDisplayName("strPropertyNameExternalToolBefore"), LocalizedDescription("strPropertyDescriptionExternalToolBefore"), TypeConverter(typeof(Tools.ExternalToolsTypeConverter))]
            public virtual string PreExtApp
            {
                get { return GetInheritedPropertyValue("PreExtApp", _preExtApp); }
                set { _preExtApp = value; }
            }

            private string _postExtApp = mRemoteNG.My.Settings.ConDefaultPostExtApp;
            [LocalizedCategory("strCategoryMiscellaneous", 7), LocalizedDisplayName("strPropertyNameExternalToolAfter"), LocalizedDescription("strPropertyDescriptionExternalToolAfter"), TypeConverter(typeof(Tools.ExternalToolsTypeConverter))]
            public virtual string PostExtApp
            {
                get { return GetInheritedPropertyValue("PostExtApp", _postExtApp); }
                set { _postExtApp = value; }
            }

            private string _macAddress = mRemoteNG.My.Settings.ConDefaultMacAddress;
            [LocalizedCategory("strCategoryMiscellaneous", 7), LocalizedDisplayName("strPropertyNameMACAddress"), LocalizedDescription("strPropertyDescriptionMACAddress")]
            public virtual string MacAddress
            {
                get { return GetInheritedPropertyValue("MacAddress", _macAddress); }
                set { _macAddress = value; }
            }

            private string _userField = mRemoteNG.My.Settings.ConDefaultUserField;
            [LocalizedCategory("strCategoryMiscellaneous", 7), LocalizedDisplayName("strPropertyNameUser1"), LocalizedDescription("strPropertyDescriptionUser1")]
            public virtual string UserField
            {
                get { return GetInheritedPropertyValue("UserField", _userField); }
                set { _userField = value; }
            }
            #endregion
            #region "VNC"
            private Protocol.VNC.Compression _vncCompression = mRemoteNG.Tools.Misc.StringToEnum(typeof(Protocol.VNC.Compression), mRemoteNG.My.Settings.ConDefaultVNCCompression);
            [LocalizedCategory("strCategoryAppearance", 5), Browsable(false), LocalizedDisplayName("strPropertyNameCompression"), LocalizedDescription("strPropertyDescriptionCompression"), TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
            public Protocol.VNC.Compression VNCCompression
            {
                get { return GetInheritedPropertyValue("VNCCompression", _vncCompression); }
                set { _vncCompression = value; }
            }

            private Protocol.VNC.Encoding _vncEncoding = mRemoteNG.Tools.Misc.StringToEnum(typeof(Protocol.VNC.Encoding), mRemoteNG.My.Settings.ConDefaultVNCEncoding);
            [LocalizedCategory("strCategoryAppearance", 5), Browsable(false), LocalizedDisplayName("strPropertyNameEncoding"), LocalizedDescription("strPropertyDescriptionEncoding"), TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
            public Protocol.VNC.Encoding VNCEncoding
            {
                get { return GetInheritedPropertyValue("VNCEncoding", _vncEncoding); }
                set { _vncEncoding = value; }
            }


            private Protocol.VNC.AuthMode _vncAuthMode = mRemoteNG.Tools.Misc.StringToEnum(typeof(Protocol.VNC.AuthMode), mRemoteNG.My.Settings.ConDefaultVNCAuthMode);
            [LocalizedCategory("strCategoryConnection", 2), Browsable(false), LocalizedDisplayName("strPropertyNameAuthenticationMode"), LocalizedDescription("strPropertyDescriptionAuthenticationMode"), TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
            public Protocol.VNC.AuthMode VNCAuthMode
            {
                get { return GetInheritedPropertyValue("VNCAuthMode", _vncAuthMode); }
                set { _vncAuthMode = value; }
            }

            private Protocol.VNC.ProxyType _vncProxyType = mRemoteNG.Tools.Misc.StringToEnum(typeof(Protocol.VNC.ProxyType), mRemoteNG.My.Settings.ConDefaultVNCProxyType);
            [LocalizedCategory("strCategoryMiscellaneous", 7), Browsable(false), LocalizedDisplayName("strPropertyNameVNCProxyType"), LocalizedDescription("strPropertyDescriptionVNCProxyType"), TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
            public Protocol.VNC.ProxyType VNCProxyType
            {
                get { return GetInheritedPropertyValue("VNCProxyType", _vncProxyType); }
                set { _vncProxyType = value; }
            }

            private string _vncProxyIP = mRemoteNG.My.Settings.ConDefaultVNCProxyIP;
            [LocalizedCategory("strCategoryMiscellaneous", 7), Browsable(false), LocalizedDisplayName("strPropertyNameVNCProxyAddress"), LocalizedDescription("strPropertyDescriptionVNCProxyAddress")]
            public string VNCProxyIP
            {
                get { return GetInheritedPropertyValue("VNCProxyIP", _vncProxyIP); }
                set { _vncProxyIP = value; }
            }

            private int _vncProxyPort = mRemoteNG.My.Settings.ConDefaultVNCProxyPort;
            [LocalizedCategory("strCategoryMiscellaneous", 7), Browsable(false), LocalizedDisplayName("strPropertyNameVNCProxyPort"), LocalizedDescription("strPropertyDescriptionVNCProxyPort")]
            public int VNCProxyPort
            {
                get { return GetInheritedPropertyValue("VNCProxyPort", _vncProxyPort); }
                set { _vncProxyPort = value; }
            }

            private string _vncProxyUsername = mRemoteNG.My.Settings.ConDefaultVNCProxyUsername;
            [LocalizedCategory("strCategoryMiscellaneous", 7), Browsable(false), LocalizedDisplayName("strPropertyNameVNCProxyUsername"), LocalizedDescription("strPropertyDescriptionVNCProxyUsername")]
            public string VNCProxyUsername
            {
                get { return GetInheritedPropertyValue("VNCProxyUsername", _vncProxyUsername); }
                set { _vncProxyUsername = value; }
            }

            private string _vncProxyPassword = mRemoteNG.My.Settings.ConDefaultVNCProxyPassword;
            [LocalizedCategory("strCategoryMiscellaneous", 7), Browsable(false), LocalizedDisplayName("strPropertyNameVNCProxyPassword"), LocalizedDescription("strPropertyDescriptionVNCProxyPassword"), PasswordPropertyText(true)]
            public string VNCProxyPassword
            {
                get { return GetInheritedPropertyValue("VNCProxyPassword", _vncProxyPassword); }
                set { _vncProxyPassword = value; }
            }

            private Protocol.VNC.Colors _vncColors = mRemoteNG.Tools.Misc.StringToEnum(typeof(Protocol.VNC.Colors), mRemoteNG.My.Settings.ConDefaultVNCColors);
            [LocalizedCategory("strCategoryAppearance", 5), Browsable(false), LocalizedDisplayName("strPropertyNameColors"), LocalizedDescription("strPropertyDescriptionColors"), TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
            public Protocol.VNC.Colors VNCColors
            {
                get { return GetInheritedPropertyValue("VNCColors", _vncColors); }
                set { _vncColors = value; }
            }

            private Protocol.VNC.SmartSizeMode _vncSmartSizeMode = mRemoteNG.Tools.Misc.StringToEnum(typeof(Protocol.VNC.SmartSizeMode), mRemoteNG.My.Settings.ConDefaultVNCSmartSizeMode);
            [LocalizedCategory("strCategoryAppearance", 5), LocalizedDisplayName("strPropertyNameSmartSizeMode"), LocalizedDescription("strPropertyDescriptionSmartSizeMode"), TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
            public Protocol.VNC.SmartSizeMode VNCSmartSizeMode
            {
                get { return GetInheritedPropertyValue("VNCSmartSizeMode", _vncSmartSizeMode); }
                set { _vncSmartSizeMode = value; }
            }

            private bool _vncViewOnly = mRemoteNG.My.Settings.ConDefaultVNCViewOnly;
            [LocalizedCategory("strCategoryAppearance", 5), LocalizedDisplayName("strPropertyNameViewOnly"), LocalizedDescription("strPropertyDescriptionViewOnly"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
            public bool VNCViewOnly
            {
                get { return GetInheritedPropertyValue("VNCViewOnly", _vncViewOnly); }
                set { _vncViewOnly = value; }
            }
            #endregion

            [Browsable(false)]
            public Inheritance Inherit { get; set; }

            [Browsable(false)]
            public Protocol.List OpenConnections { get; set; }

            [Browsable(false)]
            public bool IsContainer { get; set; }

            [Browsable(false)]
            public bool IsDefault { get; set; }

            [Browsable(false)]
            public Container.Info Parent { get; set; }

            [Browsable(false)]
            public int PositionID { get; set; }

            [Browsable(false)]
            public string ConstantID { get; set; }

            [Browsable(false)]
            public TreeNode TreeNode { get; set; }

            [Browsable(false)]
            public bool IsQuickConnect { get; set; }

            [Browsable(false)]
            public bool PleaseConnect { get; set; }
            #endregion

            #region "Constructors"
            public Info()
            {
                SetDefaults();
            }

            public Info(Container.Info parent)
            {
                SetDefaults();
                IsContainer = true;
                this.Parent = parent;
            }
            #endregion

            #region "Public Methods"
            public Info Copy()
            {
                Info newConnectionInfo = MemberwiseClone();
                newConnectionInfo.ConstantID = mRemoteNG.Tools.Misc.CreateConstantID();
                newConnectionInfo._OpenConnections = new Protocol.List();
                return newConnectionInfo;
            }

            public void SetDefaults()
            {
                if (Port == 0)
                    SetDefaultPort();
            }

            public int GetDefaultPort()
            {
                return GetDefaultPort(Protocol);
            }

            public void SetDefaultPort()
            {
                Port = GetDefaultPort();
            }
            #endregion

            #region "Public Enumerations"
            [Flags()]
            public enum Force
            {
                None = 0,
                UseConsoleSession = 1,
                Fullscreen = 2,
                DoNotJump = 4,
                OverridePanel = 8,
                DontUseConsoleSession = 16,
                NoCredentials = 32
            }
            #endregion

            #region "Private Methods"
            private TPropertyType GetInheritedPropertyValue<TPropertyType>(string propertyName, TPropertyType value)
            {
                Type inheritType = Inherit.GetType();
                PropertyInfo inheritPropertyInfo = inheritType.GetProperty(propertyName);
                bool inheritPropertyValue = inheritPropertyInfo.GetValue(Inherit, BindingFlags.GetProperty, null, null, null);

                if (inheritPropertyValue & Parent != null)
                {
                    Info parentConnectionInfo = null;
                    if (IsContainer)
                    {
                        parentConnectionInfo = Parent.Parent.ConnectionInfo;
                    }
                    else
                    {
                        parentConnectionInfo = Parent.ConnectionInfo;
                    }

                    Type connectionInfoType = parentConnectionInfo.GetType();
                    PropertyInfo parentPropertyInfo = connectionInfoType.GetProperty(propertyName);
                    TPropertyType parentPropertyValue = parentPropertyInfo.GetValue(parentConnectionInfo, BindingFlags.GetProperty, null, null, null);

                    return parentPropertyValue;
                }
                else
                {
                    return value;
                }
            }

            private static int GetDefaultPort(Protocol.Protocols protocol)
            {
                try
                {
                    switch (protocol)
                    {
                        case mRemoteNG.Connection.Protocol.Protocols.RDP:
                            return mRemoteNG.Connection.Protocol.RDP.Defaults.Port;
                        case mRemoteNG.Connection.Protocol.Protocols.VNC:
                            return mRemoteNG.Connection.Protocol.VNC.Defaults.Port;
                        case mRemoteNG.Connection.Protocol.Protocols.SSH1:
                            return mRemoteNG.Connection.Protocol.SSH1.Defaults.Port;
                        case mRemoteNG.Connection.Protocol.Protocols.SSH2:
                            return mRemoteNG.Connection.Protocol.SSH2.Defaults.Port;
                        case mRemoteNG.Connection.Protocol.Protocols.Telnet:
                            return mRemoteNG.Connection.Protocol.Telnet.Defaults.Port;
                        case mRemoteNG.Connection.Protocol.Protocols.Rlogin:
                            return mRemoteNG.Connection.Protocol.Rlogin.Defaults.Port;
                        case mRemoteNG.Connection.Protocol.Protocols.RAW:
                            return mRemoteNG.Connection.Protocol.RAW.Defaults.Port;
                        case mRemoteNG.Connection.Protocol.Protocols.HTTP:
                            return mRemoteNG.Connection.Protocol.HTTP.Defaults.Port;
                        case mRemoteNG.Connection.Protocol.Protocols.HTTPS:
                            return mRemoteNG.Connection.Protocol.HTTPS.Defaults.Port;
                        case mRemoteNG.Connection.Protocol.Protocols.ICA:
                            return mRemoteNG.Connection.Protocol.ICA.Defaults.Port;
                        case mRemoteNG.Connection.Protocol.Protocols.IntApp:
                            return mRemoteNG.Connection.Protocol.IntegratedProgram.Defaults.Port;
                    }
                }
                catch (Exception ex)
                {
                    mRemoteNG.App.Runtime.MessageCollector.AddExceptionMessage(mRemoteNG.My.Language.strConnectionSetDefaultPortFailed, ex, mRemoteNG.Messages.MessageClass.ErrorMsg);
                }
            }
            #endregion
        }
    }
}
