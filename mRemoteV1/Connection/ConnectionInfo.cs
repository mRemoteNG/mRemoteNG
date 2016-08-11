using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using mRemoteNG.Tools;
using mRemoteNG.App;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Connection.Protocol.SSH;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.RAW;
using mRemoteNG.Connection.Protocol.ICA;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.Telnet;
using mRemoteNG.Connection.Protocol.Rlogin;
using mRemoteNG.Container;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Messages;


namespace mRemoteNG.Connection
{
	[DefaultProperty("Name")]
    public class ConnectionInfo : Parent, IInheritable
    {
        #region Private Properties
        private string _description;
        private string _icon;
        private string _panel;
        private string _hostname;
        private string _username;
        private string _password;
        private string _domain;
        private ProtocolType _protocol;
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
        private string _vncProxyIp;
        private int _vncProxyPort;
        private string _vncProxyUsername;
        private string _vncProxyPassword;
        private ProtocolVNC.Colors _vncColors;
        private ProtocolVNC.SmartSizeMode _vncSmartSizeMode;
        private bool _vncViewOnly;
	    #endregion

        #region Public Properties
        #region Display
	    [LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1),
	     LocalizedAttributes.LocalizedDisplayName("strPropertyNameName"),
	     LocalizedAttributes.LocalizedDescription("strPropertyDescriptionName")]
	    public virtual string Name { get; set; }
		
        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameDescription"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDescription")]
        public virtual string Description
		{
            get { return GetPropertyValue("Description", _description); }
			set { _description = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1), 
            TypeConverter(typeof(ConnectionIcon)),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameIcon"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionIcon")]
        public virtual string Icon
		{
            get { return GetPropertyValue("Icon", _icon); }
			set { _icon = value; }
		}
			
        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNamePanel"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionPanel")]
        public virtual string Panel
		{
            get { return GetPropertyValue("Panel", _panel); }
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
			set {
			    _hostname = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
			}
		}
			
        [LocalizedAttributes.LocalizedCategory("strCategoryConnection", 2),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameUsername"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionUsername")]
        public virtual string Username
		{
            get { return GetPropertyValue("Username", _username); }
			set { _username = value.Trim(); }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryConnection", 2),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNamePassword"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionPassword"), 
            PasswordPropertyText(true)]
        public virtual string Password
		{
            get { return GetPropertyValue("Password", _password); }
			set { _password = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryConnection", 2),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameDomain"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDomain")]
        public string Domain
		{
            get { return GetPropertyValue("Domain", _domain).Trim(); }
			set { _domain = value.Trim(); }
		}
        #endregion
        #region Protocol
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameProtocol"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionProtocol"), 
            TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public virtual ProtocolType Protocol
		{
            get { return GetPropertyValue("Protocol", _protocol); }
			set { _protocol = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameExternalTool"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionExternalTool"), 
            TypeConverter(typeof(ExternalToolsTypeConverter))]
        public string ExtApp
		{
            get { return GetPropertyValue("ExtApp", _extApp); }
			set { _extApp = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNamePort"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionPort")]
        public virtual int Port
		{
            get { return GetPropertyValue("Port", _port); }
			set { _port = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNamePuttySession"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionPuttySession"),
            TypeConverter(typeof(Config.Putty.Sessions.SessionList))]
        public virtual string PuttySession
		{
            get { return GetPropertyValue("PuttySession", _puttySession); }
			set { _puttySession = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameEncryptionStrength"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionEncryptionStrength"), 
            TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolICA.EncryptionStrength ICAEncryptionStrength
		{
            get { return GetPropertyValue("ICAEncryptionStrength", _icaEncryption); }
			set { _icaEncryption = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameUseConsoleSession"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionUseConsoleSession"), 
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UseConsoleSession
		{
            get { return GetPropertyValue("UseConsoleSession", _useConsoleSession); }
			set { _useConsoleSession = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameAuthenticationLevel"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionAuthenticationLevel"), 
            TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolRDP.AuthenticationLevel RDPAuthenticationLevel
		{
            get { return GetPropertyValue("RDPAuthenticationLevel", _rdpAuthenticationLevel); }
			set { _rdpAuthenticationLevel = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameLoadBalanceInfo"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionLoadBalanceInfo")]
        public string LoadBalanceInfo
		{
            get { return GetPropertyValue("LoadBalanceInfo", _loadBalanceInfo).Trim(); }
			set { _loadBalanceInfo = value.Trim(); }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRenderingEngine"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRenderingEngine"), 
            TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public HTTPBase.RenderingEngine RenderingEngine
		{
            get { return GetPropertyValue("RenderingEngine", _renderingEngine); }
			set { _renderingEngine = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameUseCredSsp"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionUseCredSsp"), 
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UseCredSsp
		{
            get { return GetPropertyValue("UseCredSsp", _useCredSsp); }
			set { _useCredSsp = value; }
		}
        #endregion
        #region RD Gateway
        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayUsageMethod"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDGatewayUsageMethod"), 
            TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolRDP.RDGatewayUsageMethod RDGatewayUsageMethod
		{
            get { return GetPropertyValue("RDGatewayUsageMethod", _rdGatewayUsageMethod); }
			set { _rdGatewayUsageMethod = value; }
		}
			
        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayHostname"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDGatewayHostname")]
        public string RDGatewayHostname
		{
            get { return GetPropertyValue("RDGatewayHostname", _rdGatewayHostname).Trim(); }
			set { _rdGatewayHostname = value.Trim(); }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayUseConnectionCredentials"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDGatewayUseConnectionCredentials"), 
            TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolRDP.RDGatewayUseConnectionCredentials RDGatewayUseConnectionCredentials
		{
            get { return GetPropertyValue("RDGatewayUseConnectionCredentials", _rdGatewayUseConnectionCredentials); }
			set { _rdGatewayUseConnectionCredentials = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayUsername"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDGatewayUsername")]
        public string RDGatewayUsername
		{
            get { return GetPropertyValue("RDGatewayUsername", _rdGatewayUsername).Trim(); }
			set { _rdGatewayUsername = value.Trim(); }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayPassword"),
            LocalizedAttributes.LocalizedDescription("strPropertyNameRDGatewayPassword"), 
            PasswordPropertyText(true)]
        public string RDGatewayPassword
		{
            get { return GetPropertyValue("RDGatewayPassword", _rdGatewayPassword); }
			set { _rdGatewayPassword = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayDomain"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDGatewayDomain")]
        public string RDGatewayDomain
		{
            get { return GetPropertyValue("RDGatewayDomain", _rdGatewayDomain).Trim(); }
			set { _rdGatewayDomain = value.Trim(); }
		}
        #endregion
        #region Appearance
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameResolution"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionResolution"), 
            TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolRDP.RDPResolutions Resolution
		{
            get { return GetPropertyValue("Resolution", _resolution); }
			set { _resolution = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameAutomaticResize"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionAutomaticResize"), 
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool AutomaticResize
		{
            get { return GetPropertyValue("AutomaticResize", _automaticResize); }
			set { _automaticResize = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameColors"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionColors"), 
            TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolRDP.RDPColors Colors
		{
            get { return GetPropertyValue("Colors", _colors); }
			set { _colors = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameCacheBitmaps"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionCacheBitmaps"), 
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool CacheBitmaps
		{
            get { return GetPropertyValue("CacheBitmaps", _cacheBitmaps); }
			set { _cacheBitmaps = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameDisplayWallpaper"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDisplayWallpaper"), 
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool DisplayWallpaper
		{
            get { return GetPropertyValue("DisplayWallpaper", _displayWallpaper); }
			set { _displayWallpaper = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameDisplayThemes"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDisplayThemes"), 
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool DisplayThemes
		{
            get { return GetPropertyValue("DisplayThemes", _displayThemes); }
			set { _displayThemes = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameEnableFontSmoothing"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionEnableFontSmoothing"), 
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool EnableFontSmoothing
		{
            get { return GetPropertyValue("EnableFontSmoothing", _enableFontSmoothing); }
			set { _enableFontSmoothing = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameEnableDesktopComposition"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionEnableDesktopComposition"), 
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool EnableDesktopComposition
		{
            get { return GetPropertyValue("EnableDesktopComposition", _enableDesktopComposition); }
			set { _enableDesktopComposition = value; }
		}
        #endregion
        #region Redirect
        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectKeys"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectKeys"), 
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectKeys
		{
            get { return GetPropertyValue("RedirectKeys", _redirectKeys); }
			set { _redirectKeys = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectDrives"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectDrives"), 
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectDiskDrives
		{
            get { return GetPropertyValue("RedirectDiskDrives", _redirectDiskDrives); }
			set { _redirectDiskDrives = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectPrinters"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectPrinters"), 
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectPrinters
		{
            get { return GetPropertyValue("RedirectPrinters", _redirectPrinters); }
			set { _redirectPrinters = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectPorts"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectPorts"), 
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectPorts
		{
            get { return GetPropertyValue("RedirectPorts", _redirectPorts); }
			set { _redirectPorts = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectSmartCards"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectSmartCards"), 
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectSmartCards
		{
            get { return GetPropertyValue("RedirectSmartCards", _redirectSmartCards); }
			set { _redirectSmartCards = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectSounds"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectSounds"), 
            TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolRDP.RDPSounds RedirectSound
		{
            get { return GetPropertyValue("RedirectSound", _redirectSound); }
			set { _redirectSound = value; }
		}
        #endregion
        #region Misc
        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameExternalToolBefore"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionExternalToolBefore"), 
            TypeConverter(typeof(ExternalToolsTypeConverter))]
        public virtual string PreExtApp
		{
            get { return GetPropertyValue("PreExtApp", _preExtApp); }
			set { _preExtApp = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameExternalToolAfter"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionExternalToolAfter"), 
            TypeConverter(typeof(ExternalToolsTypeConverter))]
        public virtual string PostExtApp
		{
            get { return GetPropertyValue("PostExtApp", _postExtApp); }
			set { _postExtApp = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameMACAddress"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionMACAddress")]
        public virtual string MacAddress
		{
            get { return GetPropertyValue("MacAddress", _macAddress); }
			set { _macAddress = value; }
		}

        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameUser1"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionUser1")]
        public virtual string UserField
        {
            get { return GetPropertyValue("UserField", _userField); }
            set { _userField = value; }
        }
        #endregion
        #region VNC
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameCompression"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionCompression"), 
            TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolVNC.Compression VNCCompression
		{
            get { return GetPropertyValue("VNCCompression", _vncCompression); }
			set { _vncCompression = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameEncoding"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionEncoding"), 
            TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolVNC.Encoding VNCEncoding
		{
            get { return GetPropertyValue("VNCEncoding", _vncEncoding); }
			set { _vncEncoding = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryConnection", 2), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameAuthenticationMode"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionAuthenticationMode"), 
            TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolVNC.AuthMode VNCAuthMode
		{
            get { return GetPropertyValue("VNCAuthMode", _vncAuthMode); }
			set { _vncAuthMode = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyType"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyType"), 
            TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolVNC.ProxyType VNCProxyType
		{
            get { return GetPropertyValue("VNCProxyType", _vncProxyType); }
			set { _vncProxyType = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyAddress"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyAddress")]
        public string VNCProxyIP
		{
            get { return GetPropertyValue("VNCProxyIP", _vncProxyIp); }
			set { _vncProxyIp = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyPort"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyPort")]
        public int VNCProxyPort
		{
            get { return GetPropertyValue("VNCProxyPort", _vncProxyPort); }
			set { _vncProxyPort = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyUsername"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyUsername")]
        public string VNCProxyUsername
		{
            get { return GetPropertyValue("VNCProxyUsername", _vncProxyUsername); }
			set { _vncProxyUsername = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyPassword"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyPassword"), 
            PasswordPropertyText(true)]
        public string VNCProxyPassword
		{
            get { return GetPropertyValue("VNCProxyPassword", _vncProxyPassword); }
			set { _vncProxyPassword = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameColors"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionColors"), 
            TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolVNC.Colors VNCColors
		{
            get { return GetPropertyValue("VNCColors", _vncColors); }
			set { _vncColors = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameSmartSizeMode"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionSmartSizeMode"), 
            TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolVNC.SmartSizeMode VNCSmartSizeMode
		{
            get { return GetPropertyValue("VNCSmartSizeMode", _vncSmartSizeMode); }
			set { _vncSmartSizeMode = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameViewOnly"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionViewOnly"), 
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool VNCViewOnly
		{
            get { return GetPropertyValue("VNCViewOnly", _vncViewOnly); }
			set { _vncViewOnly = value; }
		}
        #endregion
        #region Non-browsable public properties
        [Browsable(false)]
        public ConnectionInfoInheritance Inheritance { get; set; }

	    [Browsable(false)]
        public ProtocolList OpenConnections { get; set; }

	    [Browsable(false)]
        public bool IsContainer { get; set; }

	    [Browsable(false)]
        public bool IsDefault { get; set; }

	    [Browsable(false)]
        public ContainerInfo Parent { get; set; }

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
        #endregion

        #region Constructors
        public ConnectionInfo()
		{
            SetTreeDisplayDefaults();
            SetConnectionDefaults();
            SetProtocolDefaults();
            SetRdGatewayDefaults();
            SetAppearanceDefaults();
            SetRedirectDefaults();
            SetMiscDefaults();
            SetVncDefaults();
            SetNonBrowsablePropertiesDefaults();
            SetDefaults();
		}
			
		public ConnectionInfo(ContainerInfo parent) : this()
		{
			IsContainer = true;
			Parent = parent;
		}
        #endregion
			
        #region Public Methods
		public virtual ConnectionInfo Copy()
		{
			var newConnectionInfo = (ConnectionInfo)MemberwiseClone();
			newConnectionInfo.ConstantID = MiscTools.CreateConstantID();
			newConnectionInfo.OpenConnections = new ProtocolList();
			return newConnectionInfo;
		}

	    public void CopyFrom(ConnectionInfo sourceConnectionInfo)
	    {
	        var properties = typeof(ConnectionInfo).GetProperties();
	        foreach (var property in properties)
	        {
	            var remotePropertyValue = property.GetValue(sourceConnectionInfo, null);
                property.SetValue(this, remotePropertyValue, null);
	        }
	    }
			
		public void SetDefaults()
		{
			if (Port == 0)
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
			Port = GetDefaultPort();
		}

        public virtual IEnumerable<PropertyInfo> GetProperties(string[] excludedPropertyNames)
        {
            var properties = typeof(ConnectionInfo).GetProperties();
            var filteredProperties = properties.Where((prop) => !excludedPropertyNames.Contains(prop.Name));
            return filteredProperties;
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
        private TPropertyType GetPropertyValue<TPropertyType>(string propertyName, TPropertyType value)
        {
            return ShouldThisPropertyBeInherited(propertyName) ? GetInheritedPropertyValue<TPropertyType>(propertyName) : value;
        }

	    private bool ShouldThisPropertyBeInherited(string propertyName)
        {
            return (ParentIsValidInheritanceTarget() && IsInheritanceTurnedOnForThisProperty(propertyName));
        }

        private bool ParentIsValidInheritanceTarget()
        {
            return (Parent != null);
        }

        private bool IsInheritanceTurnedOnForThisProperty(string propertyName)
        {
            var inheritType = Inheritance.GetType();
            var inheritPropertyInfo = inheritType.GetProperty(propertyName);
            var inheritPropertyValue = Convert.ToBoolean(inheritPropertyInfo.GetValue(Inheritance, null));
            return inheritPropertyValue;
        }

        private TPropertyType GetInheritedPropertyValue<TPropertyType>(string propertyName)
        {
            var connectionInfoType = Parent.GetType();
            var parentPropertyInfo = connectionInfoType.GetProperty(propertyName);
            var parentPropertyValue = (TPropertyType)parentPropertyInfo.GetValue(Parent, null);

            return parentPropertyValue;
        }

		private static int GetDefaultPort(ProtocolType protocol)
		{
			try
			{
                switch (protocol)
                {
                    case ProtocolType.RDP:
                        return (int)ProtocolRDP.Defaults.Port;
                    case ProtocolType.VNC:
                        return (int)ProtocolVNC.Defaults.Port;
                    case ProtocolType.SSH1:
                        return (int)ProtocolSSH1.Defaults.Port;
                    case ProtocolType.SSH2:
                        return (int)ProtocolSSH2.Defaults.Port;
                    case ProtocolType.Telnet:
                        return (int)ProtocolTelnet.Defaults.Port;
                    case ProtocolType.Rlogin:
                        return (int)ProtocolRlogin.Defaults.Port;
                    case ProtocolType.RAW:
                        return (int)ProtocolRAW.Defaults.Port;
                    case ProtocolType.HTTP:
                        return (int)ProtocolHTTP.Defaults.Port;
                    case ProtocolType.HTTPS:
                        return (int)ProtocolHTTPS.Defaults.Port;
                    case ProtocolType.ICA:
                        return (int)ProtocolICA.Defaults.Port;
                    case ProtocolType.IntApp:
                        return (int)IntegratedProgram.Defaults.Port;
                }
                return 0;
			}
			catch (Exception ex)
			{
                Runtime.MessageCollector.AddExceptionMessage(Language.strConnectionSetDefaultPortFailed, ex, MessageClass.ErrorMsg);
                return 0;
			}
		}

        private void SetTreeDisplayDefaults()
        {
            Name = Language.strNewConnection;
            _description = Settings.Default.ConDefaultDescription;
            _icon = Settings.Default.ConDefaultIcon;
            _panel = Language.strGeneral;
        }

        private void SetConnectionDefaults()
        {
            _hostname = string.Empty;
            _username = Settings.Default.ConDefaultUsername;
            _password = Settings.Default.ConDefaultPassword;
            _domain = Settings.Default.ConDefaultDomain;
        }

        private void SetProtocolDefaults()
        {
            _protocol = (ProtocolType)Enum.Parse(typeof(ProtocolType), Settings.Default.ConDefaultProtocol);
            _extApp = Settings.Default.ConDefaultExtApp;
            _port = 0;
            _puttySession = Settings.Default.ConDefaultPuttySession;
            _icaEncryption = (ProtocolICA.EncryptionStrength) Enum.Parse(typeof(ProtocolICA.EncryptionStrength), Settings.Default.ConDefaultICAEncryptionStrength);
            _useConsoleSession = Settings.Default.ConDefaultUseConsoleSession;
            _rdpAuthenticationLevel = (ProtocolRDP.AuthenticationLevel) Enum.Parse(typeof(ProtocolRDP.AuthenticationLevel), Settings.Default.ConDefaultRDPAuthenticationLevel);
            _loadBalanceInfo = Settings.Default.ConDefaultLoadBalanceInfo;
            _renderingEngine = (HTTPBase.RenderingEngine) Enum.Parse(typeof(HTTPBase.RenderingEngine), Settings.Default.ConDefaultRenderingEngine);
            _useCredSsp = Settings.Default.ConDefaultUseCredSsp;
        }

        private void SetRdGatewayDefaults()
        {
            _rdGatewayUsageMethod = (ProtocolRDP.RDGatewayUsageMethod) Enum.Parse(typeof(ProtocolRDP.RDGatewayUsageMethod), Settings.Default.ConDefaultRDGatewayUsageMethod);
            _rdGatewayHostname = Settings.Default.ConDefaultRDGatewayHostname;
            _rdGatewayUseConnectionCredentials = (ProtocolRDP.RDGatewayUseConnectionCredentials) Enum.Parse(typeof(ProtocolRDP.RDGatewayUseConnectionCredentials), Settings.Default.ConDefaultRDGatewayUseConnectionCredentials); ;
            _rdGatewayUsername = Settings.Default.ConDefaultRDGatewayUsername;
            _rdGatewayPassword = Settings.Default.ConDefaultRDGatewayPassword;
            _rdGatewayDomain = Settings.Default.ConDefaultRDGatewayDomain;
        }

        private void SetAppearanceDefaults() 
        {
            _resolution = (ProtocolRDP.RDPResolutions) Enum.Parse(typeof(ProtocolRDP.RDPResolutions), Settings.Default.ConDefaultResolution);
            _automaticResize = Settings.Default.ConDefaultAutomaticResize;
            _colors = (ProtocolRDP.RDPColors) Enum.Parse(typeof(ProtocolRDP.RDPColors), Settings.Default.ConDefaultColors);
            _cacheBitmaps = Settings.Default.ConDefaultCacheBitmaps;
            _displayWallpaper = Settings.Default.ConDefaultDisplayWallpaper;
            _displayThemes = Settings.Default.ConDefaultDisplayThemes;
            _enableFontSmoothing = Settings.Default.ConDefaultEnableFontSmoothing;
            _enableDesktopComposition = Settings.Default.ConDefaultEnableDesktopComposition;
        }

        private void SetRedirectDefaults()
        {
            _redirectKeys = Settings.Default.ConDefaultRedirectKeys;
            _redirectDiskDrives = Settings.Default.ConDefaultRedirectDiskDrives;
            _redirectPrinters = Settings.Default.ConDefaultRedirectPrinters;
            _redirectPorts = Settings.Default.ConDefaultRedirectPorts;
            _redirectSmartCards = Settings.Default.ConDefaultRedirectSmartCards;
            _redirectSound = (ProtocolRDP.RDPSounds) Enum.Parse(typeof(ProtocolRDP.RDPSounds), Settings.Default.ConDefaultRedirectSound);
        }

        private void SetMiscDefaults()
        {
            ConstantID = MiscTools.CreateConstantID();
            _preExtApp = Settings.Default.ConDefaultPreExtApp;
            _postExtApp = Settings.Default.ConDefaultPostExtApp;
            _macAddress = Settings.Default.ConDefaultMacAddress;
            _userField = Settings.Default.ConDefaultUserField;
        }

        private void SetVncDefaults()
        {
            _vncCompression = (ProtocolVNC.Compression) Enum.Parse(typeof(ProtocolVNC.Compression), Settings.Default.ConDefaultVNCCompression);
            _vncEncoding = (ProtocolVNC.Encoding) Enum.Parse(typeof(ProtocolVNC.Encoding), Settings.Default.ConDefaultVNCEncoding);
            _vncAuthMode = (ProtocolVNC.AuthMode) Enum.Parse(typeof(ProtocolVNC.AuthMode), Settings.Default.ConDefaultVNCAuthMode);
            _vncProxyType = (ProtocolVNC.ProxyType) Enum.Parse(typeof(ProtocolVNC.ProxyType), Settings.Default.ConDefaultVNCProxyType);
            _vncProxyIp = Settings.Default.ConDefaultVNCProxyIP;
            _vncProxyPort = Settings.Default.ConDefaultVNCProxyPort;
            _vncProxyUsername = Settings.Default.ConDefaultVNCProxyUsername;
            _vncProxyPassword = Settings.Default.ConDefaultVNCProxyPassword;
            _vncColors = (ProtocolVNC.Colors) Enum.Parse(typeof(ProtocolVNC.Colors), Settings.Default.ConDefaultVNCColors);
            _vncSmartSizeMode = (ProtocolVNC.SmartSizeMode) Enum.Parse(typeof(ProtocolVNC.SmartSizeMode), Settings.Default.ConDefaultVNCSmartSizeMode);
            _vncViewOnly = Settings.Default.ConDefaultVNCViewOnly;
        }

        private void SetNonBrowsablePropertiesDefaults()
        {
            Inheritance = new ConnectionInfoInheritance(this);
            OpenConnections = new ProtocolList();
            PositionID = 0;
        }
        #endregion
	}
}