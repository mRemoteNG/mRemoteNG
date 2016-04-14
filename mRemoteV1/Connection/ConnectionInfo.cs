using System;
using System.Windows.Forms;
using System.ComponentModel;
using mRemoteNG.Tools;
using System.Reflection;
using mRemoteNG.App;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Connection.Protocol.SSH;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.RAW;
using mRemoteNG.Connection.Protocol.ICA;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.Telnet;
using mRemoteNG.Connection.Protocol.Rlogin;


namespace mRemoteNG.Connection
{
	[DefaultProperty("Name")]
    public partial class ConnectionInfo
    {
        #region Private Properties
        // Private properties with public get/set
        private string _name;
        private string _description;
        private string _icon;
        private string _panel;
        private string _hostname;
        private string _username;
        private string _password;
        private string _domain;
        private Protocol.ProtocolType _protocol;
        private string _extApp;
        private int _port;
        private string _puttySession;
        private ProtocolICA.EncryptionStrength _icaEncryption;
        private bool _useConsoleSession;
        private ProtocolRDP.AuthenticationLevel _rdpAuthenticationLevel;
        private string _loadBalanceInfo;
        private HTTPBase.RenderingEngine _renderingEngine;
        private bool _useCredSsp;
        private ProtocolRDP.RDGatewayUsageMethod _rdGatewayUsageMethod;
        private string _rdGatewayHostname;
        private ProtocolRDP.RDGatewayUseConnectionCredentials _rdGatewayUseConnectionCredentials;
        private string _rdGatewayUsername;
        private string _rdGatewayPassword;
        private string _rdGatewayDomain;
        private ProtocolRDP.RDPResolutions _resolution;
        private bool _automaticResize;
        private ProtocolRDP.RDPColors _colors;
        private bool _cacheBitmaps;
        private bool _displayWallpaper;
        private bool _displayThemes;
        private bool _enableFontSmoothing;
        private bool _enableDesktopComposition;
        private bool _redirectKeys;
        private bool _redirectDiskDrives;
        private bool _redirectPrinters;
        private bool _redirectPorts;
        private bool _redirectSmartCards;
        private ProtocolRDP.RDPSounds _redirectSound;
        private string _preExtApp;
        private string _postExtApp;
        private string _macAddress;
        private string _userField;
        private ProtocolVNC.Compression _vncCompression;
        private ProtocolVNC.Encoding _vncEncoding;
        private ProtocolVNC.AuthMode _vncAuthMode;
        private ProtocolVNC.ProxyType _vncProxyType;
        private string _vncProxyIP;
        private int _vncProxyPort;
        private string _vncProxyUsername;
        private string _vncProxyPassword;
        private ProtocolVNC.Colors _vncColors;
        private ProtocolVNC.SmartSizeMode _vncSmartSizeMode;
        private bool _vncViewOnly;
        private ConnectionInfoInheritance _Inherit;
        private Protocol.ProtocolList _OpenConnections;
        private bool _IsContainer;
        private bool _IsDefault;
        private int _PositionID;
        private bool _IsQuickConnect;
        private bool _PleaseConnect;
        private string _constantId;
        #endregion

        #region Public Properties
        #region Display
        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameName"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionName")]
        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }
		
        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameDescription"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDescription")]
        public virtual string Description
		{
			get { return GetInheritedPropertyValue("Description", _description); }
			set { _description = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1), 
            TypeConverter(typeof(ConnectionIcon)),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameIcon"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionIcon")]
        public virtual string Icon
		{
			get { return GetInheritedPropertyValue("Icon", _icon); }
			set { _icon = value; }
		}
			
        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNamePanel"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionPanel")]
        public virtual string Panel
		{
			get { return GetInheritedPropertyValue("Panel", _panel); }
			set { _panel = value; }
		}
        #endregion
        #region Connection
        [LocalizedAttributes.LocalizedCategory("strCategoryConnection", 2),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameAddress"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionAddress")]
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
			
        [LocalizedAttributes.LocalizedCategory("strCategoryConnection", 2),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameUsername"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionUsername")]
        public virtual string Username
		{
			get { return GetInheritedPropertyValue("Username", _username); }
			set { _username = value.Trim(); }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryConnection", 2),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNamePassword"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionPassword"), 
            PasswordPropertyText(true)]
        public virtual string Password
		{
			get { return GetInheritedPropertyValue("Password", _password); }
			set { _password = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryConnection", 2),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameDomain"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDomain")]
        public string Domain
		{
			get { return GetInheritedPropertyValue("Domain", _domain).Trim(); }
			set { _domain = value.Trim(); }
		}
        #endregion
        #region Protocol
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameProtocol"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionProtocol"), 
            TypeConverter(typeof(Tools.MiscTools.EnumTypeConverter))]
        public virtual Protocol.ProtocolType Protocol
		{
			get { return GetInheritedPropertyValue("Protocol", _protocol); }
			set { _protocol = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameExternalTool"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionExternalTool"), 
            TypeConverter(typeof(Tools.ExternalToolsTypeConverter))]
        public string ExtApp
		{
			get { return GetInheritedPropertyValue("ExtApp", _extApp); }
			set { _extApp = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNamePort"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionPort")]
        public virtual int Port
		{
			get { return GetInheritedPropertyValue("Port", _port); }
			set { _port = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNamePuttySession"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionPuttySession"),
            TypeConverter(typeof(Config.Putty.Sessions.SessionList))]
        public virtual string PuttySession
		{
			get { return GetInheritedPropertyValue("PuttySession", _puttySession); }
			set { _puttySession = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameEncryptionStrength"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionEncryptionStrength"), 
            TypeConverter(typeof(Tools.MiscTools.EnumTypeConverter))]
        public ProtocolICA.EncryptionStrength ICAEncryption
		{
			get { return GetInheritedPropertyValue("ICAEncryption", _icaEncryption); }
			set { _icaEncryption = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameUseConsoleSession"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionUseConsoleSession"), 
            TypeConverter(typeof(Tools.MiscTools.YesNoTypeConverter))]
        public bool UseConsoleSession
		{
			get { return GetInheritedPropertyValue("UseConsoleSession", _useConsoleSession); }
			set { _useConsoleSession = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameAuthenticationLevel"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionAuthenticationLevel"), 
            TypeConverter(typeof(Tools.MiscTools.EnumTypeConverter))]
        public ProtocolRDP.AuthenticationLevel RDPAuthenticationLevel
		{
			get { return GetInheritedPropertyValue("RDPAuthenticationLevel", _rdpAuthenticationLevel); }
			set { _rdpAuthenticationLevel = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameLoadBalanceInfo"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionLoadBalanceInfo")]
        public string LoadBalanceInfo
		{
			get { return GetInheritedPropertyValue("LoadBalanceInfo", _loadBalanceInfo).Trim(); }
			set { _loadBalanceInfo = value.Trim(); }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRenderingEngine"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRenderingEngine"), 
            TypeConverter(typeof(Tools.MiscTools.EnumTypeConverter))]
        public HTTPBase.RenderingEngine RenderingEngine
		{
			get { return GetInheritedPropertyValue("RenderingEngine", _renderingEngine); }
			set { _renderingEngine = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameUseCredSsp"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionUseCredSsp"), 
            TypeConverter(typeof(Tools.MiscTools.YesNoTypeConverter))]
        public bool UseCredSsp
		{
			get { return GetInheritedPropertyValue("UseCredSsp", _useCredSsp); }
			set { _useCredSsp = value; }
		}
        #endregion
        #region RD Gateway
        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayUsageMethod"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDGatewayUsageMethod"), 
            TypeConverter(typeof(Tools.MiscTools.EnumTypeConverter))]
        public ProtocolRDP.RDGatewayUsageMethod RDGatewayUsageMethod
		{
			get { return GetInheritedPropertyValue("RDGatewayUsageMethod", _rdGatewayUsageMethod); }
			set { _rdGatewayUsageMethod = value; }
		}
			
        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayHostname"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDGatewayHostname")]
        public string RDGatewayHostname
		{
			get { return GetInheritedPropertyValue("RDGatewayHostname", _rdGatewayHostname).Trim(); }
			set { _rdGatewayHostname = value.Trim(); }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayUseConnectionCredentials"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDGatewayUseConnectionCredentials"), 
            TypeConverter(typeof(Tools.MiscTools.EnumTypeConverter))]
        public ProtocolRDP.RDGatewayUseConnectionCredentials RDGatewayUseConnectionCredentials
		{
			get { return GetInheritedPropertyValue("RDGatewayUseConnectionCredentials", _rdGatewayUseConnectionCredentials); }
			set { _rdGatewayUseConnectionCredentials = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayUsername"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDGatewayUsername")]
        public string RDGatewayUsername
		{
			get { return GetInheritedPropertyValue("RDGatewayUsername", _rdGatewayUsername).Trim(); }
			set { _rdGatewayUsername = value.Trim(); }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayPassword"),
            LocalizedAttributes.LocalizedDescription("strPropertyNameRDGatewayPassword"), 
            PasswordPropertyText(true)]
        public string RDGatewayPassword
		{
			get { return GetInheritedPropertyValue("RDGatewayPassword", _rdGatewayPassword); }
			set { _rdGatewayPassword = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayDomain"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDGatewayDomain")]
        public string RDGatewayDomain
		{
			get { return GetInheritedPropertyValue("RDGatewayDomain", _rdGatewayDomain).Trim(); }
			set { _rdGatewayDomain = value.Trim(); }
		}
        #endregion
        #region Appearance
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameResolution"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionResolution"), 
            TypeConverter(typeof(Tools.MiscTools.EnumTypeConverter))]
        public ProtocolRDP.RDPResolutions Resolution
		{
			get { return GetInheritedPropertyValue("Resolution", _resolution); }
			set { _resolution = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameAutomaticResize"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionAutomaticResize"), 
            TypeConverter(typeof(Tools.MiscTools.YesNoTypeConverter))]
        public bool AutomaticResize
		{
			get { return GetInheritedPropertyValue("AutomaticResize", _automaticResize); }
			set { _automaticResize = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameColors"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionColors"), 
            TypeConverter(typeof(Tools.MiscTools.EnumTypeConverter))]
        public ProtocolRDP.RDPColors Colors
		{
			get { return GetInheritedPropertyValue("Colors", _colors); }
			set { _colors = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameCacheBitmaps"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionCacheBitmaps"), 
            TypeConverter(typeof(Tools.MiscTools.YesNoTypeConverter))]
        public bool CacheBitmaps
		{
			get { return GetInheritedPropertyValue("CacheBitmaps", _cacheBitmaps); }
			set { _cacheBitmaps = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameDisplayWallpaper"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDisplayWallpaper"), 
            TypeConverter(typeof(Tools.MiscTools.YesNoTypeConverter))]
        public bool DisplayWallpaper
		{
			get { return GetInheritedPropertyValue("DisplayWallpaper", _displayWallpaper); }
			set { _displayWallpaper = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameDisplayThemes"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDisplayThemes"), 
            TypeConverter(typeof(Tools.MiscTools.YesNoTypeConverter))]
        public bool DisplayThemes
		{
			get { return GetInheritedPropertyValue("DisplayThemes", _displayThemes); }
			set { _displayThemes = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameEnableFontSmoothing"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionEnableFontSmoothing"), 
            TypeConverter(typeof(Tools.MiscTools.YesNoTypeConverter))]
        public bool EnableFontSmoothing
		{
			get { return GetInheritedPropertyValue("EnableFontSmoothing", _enableFontSmoothing); }
			set { _enableFontSmoothing = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameEnableDesktopComposition"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionEnableDesktopComposition"), 
            TypeConverter(typeof(Tools.MiscTools.YesNoTypeConverter))]
        public bool EnableDesktopComposition
		{
			get { return GetInheritedPropertyValue("EnableDesktopComposition", _enableDesktopComposition); }
			set { _enableDesktopComposition = value; }
		}
        #endregion
        #region Redirect
        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectKeys"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectKeys"), 
            TypeConverter(typeof(Tools.MiscTools.YesNoTypeConverter))]
        public bool RedirectKeys
		{
			get { return GetInheritedPropertyValue("RedirectKeys", _redirectKeys); }
			set { _redirectKeys = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectDrives"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectDrives"), 
            TypeConverter(typeof(Tools.MiscTools.YesNoTypeConverter))]
        public bool RedirectDiskDrives
		{
			get { return GetInheritedPropertyValue("RedirectDiskDrives", _redirectDiskDrives); }
			set { _redirectDiskDrives = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectPrinters"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectPrinters"), 
            TypeConverter(typeof(Tools.MiscTools.YesNoTypeConverter))]
        public bool RedirectPrinters
		{
			get { return GetInheritedPropertyValue("RedirectPrinters", _redirectPrinters); }
			set { _redirectPrinters = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectPorts"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectPorts"), 
            TypeConverter(typeof(Tools.MiscTools.YesNoTypeConverter))]
        public bool RedirectPorts
		{
			get { return GetInheritedPropertyValue("RedirectPorts", _redirectPorts); }
			set { _redirectPorts = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectSmartCards"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectSmartCards"), 
            TypeConverter(typeof(Tools.MiscTools.YesNoTypeConverter))]
        public bool RedirectSmartCards
		{
			get { return GetInheritedPropertyValue("RedirectSmartCards", _redirectSmartCards); }
			set { _redirectSmartCards = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectSounds"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectSounds"), 
            TypeConverter(typeof(Tools.MiscTools.EnumTypeConverter))]
        public ProtocolRDP.RDPSounds RedirectSound
		{
			get { return GetInheritedPropertyValue("RedirectSound", _redirectSound); }
			set { _redirectSound = value; }
		}
        #endregion
        #region Misc
        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameExternalToolBefore"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionExternalToolBefore"), 
            TypeConverter(typeof(Tools.ExternalToolsTypeConverter))]
        public virtual string PreExtApp
		{
			get { return GetInheritedPropertyValue("PreExtApp", _preExtApp); }
			set { _preExtApp = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameExternalToolAfter"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionExternalToolAfter"), 
            TypeConverter(typeof(Tools.ExternalToolsTypeConverter))]
        public virtual string PostExtApp
		{
			get { return GetInheritedPropertyValue("PostExtApp", _postExtApp); }
			set { _postExtApp = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameMACAddress"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionMACAddress")]
        public virtual string MacAddress
		{
			get { return GetInheritedPropertyValue("MacAddress", _macAddress); }
			set { _macAddress = value; }
		}

        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameUser1"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionUser1")]
        public virtual string UserField
        {
            get { return GetInheritedPropertyValue("UserField", _userField); }
            set { _userField = value; }
        }
        #endregion
        #region VNC
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameCompression"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionCompression"), 
            TypeConverter(typeof(Tools.MiscTools.EnumTypeConverter))]
        public ProtocolVNC.Compression VNCCompression
		{
			get { return GetInheritedPropertyValue("VNCCompression", _vncCompression); }
			set { _vncCompression = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameEncoding"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionEncoding"), 
            TypeConverter(typeof(Tools.MiscTools.EnumTypeConverter))]
        public ProtocolVNC.Encoding VNCEncoding
		{
			get { return GetInheritedPropertyValue("VNCEncoding", _vncEncoding); }
			set { _vncEncoding = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryConnection", 2), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameAuthenticationMode"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionAuthenticationMode"), 
            TypeConverter(typeof(Tools.MiscTools.EnumTypeConverter))]
        public ProtocolVNC.AuthMode VNCAuthMode
		{
			get { return GetInheritedPropertyValue("VNCAuthMode", _vncAuthMode); }
			set { _vncAuthMode = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyType"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyType"), 
            TypeConverter(typeof(Tools.MiscTools.EnumTypeConverter))]
        public ProtocolVNC.ProxyType VNCProxyType
		{
			get { return GetInheritedPropertyValue("VNCProxyType", _vncProxyType); }
			set { _vncProxyType = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyAddress"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyAddress")]
        public string VNCProxyIP
		{
			get { return GetInheritedPropertyValue("VNCProxyIP", _vncProxyIP); }
			set { _vncProxyIP = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyPort"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyPort")]
        public int VNCProxyPort
		{
			get { return GetInheritedPropertyValue("VNCProxyPort", _vncProxyPort); }
			set { _vncProxyPort = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyUsername"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyUsername")]
        public string VNCProxyUsername
		{
			get { return GetInheritedPropertyValue("VNCProxyUsername", _vncProxyUsername); }
			set { _vncProxyUsername = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyPassword"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyPassword"), 
            PasswordPropertyText(true)]
        public string VNCProxyPassword
		{
			get { return GetInheritedPropertyValue("VNCProxyPassword", _vncProxyPassword); }
			set { _vncProxyPassword = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameColors"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionColors"), 
            TypeConverter(typeof(Tools.MiscTools.EnumTypeConverter))]
        public ProtocolVNC.Colors VNCColors
		{
			get { return GetInheritedPropertyValue("VNCColors", _vncColors); }
			set { _vncColors = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameSmartSizeMode"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionSmartSizeMode"), 
            TypeConverter(typeof(Tools.MiscTools.EnumTypeConverter))]
        public ProtocolVNC.SmartSizeMode VNCSmartSizeMode
		{
			get { return GetInheritedPropertyValue("VNCSmartSizeMode", _vncSmartSizeMode); }
			set { _vncSmartSizeMode = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameViewOnly"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionViewOnly"), 
            TypeConverter(typeof(Tools.MiscTools.YesNoTypeConverter))]
        public bool VNCViewOnly
		{
			get { return GetInheritedPropertyValue("VNCViewOnly", _vncViewOnly); }
			set { _vncViewOnly = value; }
		}
        #endregion
        #region Non-browsable public properties
        [Browsable(false)]
        public ConnectionInfoInheritance Inherit
        {
            get { return _Inherit; }
            set { _Inherit = value; }
        }
        
        [Browsable(false)]
        public Protocol.ProtocolList OpenConnections
        {
            get { return _OpenConnections; }
            set { _OpenConnections = value; }
        }

        [Browsable(false)]
        public bool IsContainer
        {
            get { return _IsContainer; }
            set { _IsContainer = value; }
        }

        [Browsable(false)]
        public bool IsDefault
        {
            get { return _IsDefault; }
            set { _IsDefault = value; }
        }

        [Browsable(false)]
        public Container.ContainerInfo Parent { get; set; }

        [Browsable(false)]
        public int PositionID
        {
            get { return _PositionID; }
            set { _PositionID = value; }
        }

        [Browsable(false)]
        public string ConstantID
        {
            get { return _constantId; }
            set { _constantId = value; }
        }

        [Browsable(false)]
        public TreeNode TreeNode { get; set; }

        [Browsable(false)]
        public bool IsQuickConnect
        {
            get { return _IsQuickConnect; }
            set { _IsQuickConnect = value; }
        }

        [Browsable(false)]
        public bool PleaseConnect
        {
            get { return _PleaseConnect; }
            set { _PleaseConnect = value; }
        }
        #endregion
        #endregion

        #region Constructors
        public ConnectionInfo()
		{
            // initialize default values for all standard instance members
            SetTreeDisplayDefaults();
            SetConnectionDefaults();
            SetProtocolDefaults();
            SetRDGatewayDefaults();
            SetAppearanceDefaults();
            SetRedirectDefaults();
            SetMiscDefaults();
            SetVNCDefaults();
            SetNonBrowsablePropertiesDefaults();
            SetDefaults();
		}
			
		public ConnectionInfo(Container.ContainerInfo parent) : this()
		{
			IsContainer = true;
			this.Parent = parent;
		}
        #endregion
			
        #region Public Methods
		public ConnectionInfo Copy()
		{
			ConnectionInfo newConnectionInfo = (ConnectionInfo)MemberwiseClone();
			newConnectionInfo.ConstantID = Tools.MiscTools.CreateConstantID();
			newConnectionInfo._OpenConnections = new Protocol.ProtocolList();
			return newConnectionInfo;
		}
			
		public void SetDefaults()
		{
			if (this.Port == 0)
			{
				SetDefaultPort();
			}
		}
			
		public int GetDefaultPort()
		{
			return GetDefaultPort(Protocol);
		}
			
		public void SetDefaultPort()
		{
			this.Port = GetDefaultPort();
		}
        #endregion
			
        #region Public Enumerations
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
			
        #region Private Methods
		private TPropertyType GetInheritedPropertyValue<TPropertyType>(string propertyName, TPropertyType value)
		{
            Type inheritType = _Inherit.GetType();
			PropertyInfo inheritPropertyInfo = inheritType.GetProperty(propertyName);
			bool inheritPropertyValue = System.Convert.ToBoolean(inheritPropertyInfo.GetValue(Inherit, BindingFlags.GetProperty, null, null, null));
				
			if (inheritPropertyValue && Parent != null)
			{
				ConnectionInfo parentConnectionInfo = default(ConnectionInfo);
				if (IsContainer)
				{
					parentConnectionInfo = ((Container.ContainerInfo)Parent.Parent).ConnectionInfo;
				}
				else
				{
					parentConnectionInfo = Parent.ConnectionInfo;
				}
					
				Type connectionInfoType = parentConnectionInfo.GetType();
				PropertyInfo parentPropertyInfo = connectionInfoType.GetProperty(propertyName);
                TPropertyType parentPropertyValue = (TPropertyType)parentPropertyInfo.GetValue(parentConnectionInfo, BindingFlags.GetProperty, null, null, null);
					
				return parentPropertyValue;
			}
			else
			{
				return value;
			}
		}
			
		private static int GetDefaultPort(Protocol.ProtocolType protocol)
		{
			try
			{
                switch (protocol)
                {
                    case Connection.Protocol.ProtocolType.RDP:
                        return (int)ProtocolRDP.Defaults.Port;
                    case Connection.Protocol.ProtocolType.VNC:
                        return (int)ProtocolVNC.Defaults.Port;
                    case Connection.Protocol.ProtocolType.SSH1:
                        return (int)ProtocolSSH1.Defaults.Port;
                    case Connection.Protocol.ProtocolType.SSH2:
                        return (int)ProtocolSSH2.Defaults.Port;
                    case Connection.Protocol.ProtocolType.Telnet:
                        return (int)ProtocolTelnet.Defaults.Port;
                    case Connection.Protocol.ProtocolType.Rlogin:
                        return (int)ProtocolRlogin.Defaults.Port;
                    case Connection.Protocol.ProtocolType.RAW:
                        return (int)ProtocolRAW.Defaults.Port;
                    case Connection.Protocol.ProtocolType.HTTP:
                        return (int)ProtocolHTTP.Defaults.Port;
                    case Connection.Protocol.ProtocolType.HTTPS:
                        return (int)ProtocolHTTPS.Defaults.Port;
                    case Connection.Protocol.ProtocolType.ICA:
                        return (int)ProtocolICA.Defaults.Port;
                    case Connection.Protocol.ProtocolType.IntApp:
                        return (int)Connection.Protocol.IntegratedProgram.Defaults.Port;
                }
                return 0;
			}
			catch (Exception ex)
			{
                Runtime.MessageCollector.AddExceptionMessage(My.Language.strConnectionSetDefaultPortFailed, ex, Messages.MessageClass.ErrorMsg);
                return 0;
			}
		}

        private void SetTreeDisplayDefaults()
        {
            _name = My.Language.strNewConnection;
            _description = My.Settings.Default.ConDefaultDescription;
            _icon = My.Settings.Default.ConDefaultIcon;
            _panel = My.Language.strGeneral;
        }

        private void SetConnectionDefaults()
        {
            _hostname = string.Empty;
            _username = My.Settings.Default.ConDefaultUsername;
            _password = My.Settings.Default.ConDefaultPassword;
            _domain = My.Settings.Default.ConDefaultDomain;
        }

        private void SetProtocolDefaults()
        {
            _protocol = (Connection.Protocol.ProtocolType)System.Enum.Parse(typeof(Connection.Protocol.ProtocolType), My.Settings.Default.ConDefaultProtocol);
            _extApp = My.Settings.Default.ConDefaultExtApp;
            _port = 0;
            _puttySession = My.Settings.Default.ConDefaultPuttySession;
            _icaEncryption = (ProtocolICA.EncryptionStrength)System.Enum.Parse(typeof(ProtocolICA.EncryptionStrength), My.Settings.Default.ConDefaultICAEncryptionStrength);
            _useConsoleSession = My.Settings.Default.ConDefaultUseConsoleSession;
            _rdpAuthenticationLevel = (ProtocolRDP.AuthenticationLevel)System.Enum.Parse(typeof(ProtocolRDP.AuthenticationLevel), My.Settings.Default.ConDefaultRDPAuthenticationLevel);
            _loadBalanceInfo = My.Settings.Default.ConDefaultLoadBalanceInfo;
            _renderingEngine = (HTTPBase.RenderingEngine)System.Enum.Parse(typeof(HTTPBase.RenderingEngine), My.Settings.Default.ConDefaultRenderingEngine);
            _useCredSsp = My.Settings.Default.ConDefaultUseCredSsp;
        }

        private void SetRDGatewayDefaults()
        {
            _rdGatewayUsageMethod = (ProtocolRDP.RDGatewayUsageMethod)System.Enum.Parse(typeof(ProtocolRDP.RDGatewayUsageMethod), My.Settings.Default.ConDefaultRDGatewayUsageMethod);
            _rdGatewayHostname = My.Settings.Default.ConDefaultRDGatewayHostname;
            _rdGatewayUseConnectionCredentials = (ProtocolRDP.RDGatewayUseConnectionCredentials)System.Enum.Parse(typeof(ProtocolRDP.RDGatewayUseConnectionCredentials), My.Settings.Default.ConDefaultRDGatewayUseConnectionCredentials); ;
            _rdGatewayUsername = My.Settings.Default.ConDefaultRDGatewayUsername;
            _rdGatewayPassword = My.Settings.Default.ConDefaultRDGatewayPassword;
            _rdGatewayDomain = My.Settings.Default.ConDefaultRDGatewayDomain;
        }

        private void SetAppearanceDefaults() 
        {
            _resolution = (ProtocolRDP.RDPResolutions)System.Enum.Parse(typeof(ProtocolRDP.RDPResolutions), My.Settings.Default.ConDefaultResolution);
            _automaticResize = My.Settings.Default.ConDefaultAutomaticResize;
            _colors = (ProtocolRDP.RDPColors)System.Enum.Parse(typeof(ProtocolRDP.RDPColors), My.Settings.Default.ConDefaultColors);
            _cacheBitmaps = My.Settings.Default.ConDefaultCacheBitmaps;
            _displayWallpaper = My.Settings.Default.ConDefaultDisplayWallpaper;
            _displayThemes = My.Settings.Default.ConDefaultDisplayThemes;
            _enableFontSmoothing = My.Settings.Default.ConDefaultEnableFontSmoothing;
            _enableDesktopComposition = My.Settings.Default.ConDefaultEnableDesktopComposition;
        }

        private void SetRedirectDefaults()
        {
            _redirectKeys = My.Settings.Default.ConDefaultRedirectKeys;
            _redirectDiskDrives = My.Settings.Default.ConDefaultRedirectDiskDrives;
            _redirectPrinters = My.Settings.Default.ConDefaultRedirectPrinters;
            _redirectPorts = My.Settings.Default.ConDefaultRedirectPorts;
            _redirectSmartCards = My.Settings.Default.ConDefaultRedirectSmartCards;
            _redirectSound = (ProtocolRDP.RDPSounds)System.Enum.Parse(typeof(ProtocolRDP.RDPSounds), My.Settings.Default.ConDefaultRedirectSound);
        }

        private void SetMiscDefaults()
        {
            _constantId = Tools.MiscTools.CreateConstantID();
            _preExtApp = My.Settings.Default.ConDefaultPreExtApp;
            _postExtApp = My.Settings.Default.ConDefaultPostExtApp;
            _macAddress = My.Settings.Default.ConDefaultMacAddress;
            _userField = My.Settings.Default.ConDefaultUserField;
        }

        private void SetVNCDefaults()
        {
            _vncCompression = (ProtocolVNC.Compression)System.Enum.Parse(typeof(ProtocolVNC.Compression),My.Settings.Default.ConDefaultVNCCompression);
            _vncEncoding = (ProtocolVNC.Encoding)System.Enum.Parse(typeof(ProtocolVNC.Encoding),My.Settings.Default.ConDefaultVNCEncoding);
            _vncAuthMode = (ProtocolVNC.AuthMode)System.Enum.Parse(typeof(ProtocolVNC.AuthMode),My.Settings.Default.ConDefaultVNCAuthMode);
            _vncProxyType = (ProtocolVNC.ProxyType)System.Enum.Parse(typeof(ProtocolVNC.ProxyType),My.Settings.Default.ConDefaultVNCProxyType);
            _vncProxyIP = My.Settings.Default.ConDefaultVNCProxyIP;
            _vncProxyPort = My.Settings.Default.ConDefaultVNCProxyPort;
            _vncProxyUsername = My.Settings.Default.ConDefaultVNCProxyUsername;
            _vncProxyPassword = My.Settings.Default.ConDefaultVNCProxyPassword;
            _vncColors = (ProtocolVNC.Colors)System.Enum.Parse(typeof(ProtocolVNC.Colors),My.Settings.Default.ConDefaultVNCColors);
            _vncSmartSizeMode = (ProtocolVNC.SmartSizeMode)System.Enum.Parse(typeof(ProtocolVNC.SmartSizeMode),My.Settings.Default.ConDefaultVNCSmartSizeMode);
            _vncViewOnly = My.Settings.Default.ConDefaultVNCViewOnly;
        }

        private void SetNonBrowsablePropertiesDefaults()
        {
            _Inherit = new ConnectionInfoInheritance(this);
            _OpenConnections = new Protocol.ProtocolList();
            _IsContainer = false;
            _IsDefault = false;
            _PositionID = 0;
            _IsQuickConnect = false;
            _PleaseConnect = false;
        }
        #endregion
	}
}