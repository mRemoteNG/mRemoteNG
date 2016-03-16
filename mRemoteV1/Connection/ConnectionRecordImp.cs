using mRemoteNG.App;
using mRemoteNG.Tools;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using mRemoteNG.Credential;


namespace mRemoteNG.Connection
{
	[DefaultProperty("Name")]
    public partial class ConnectionRecordImp : ConnectionRecord
    {
        #region Private Properties
        ConnectionRecordMetaData _metaData;
        CredentialRecord _credential;
        ExternalToolRecord _externalTool;
        ConnectionProtocol _protocol;
        private string _constantId;




        // Meta data
        //private Protocol.Protocols _protocol;
        private ConnectionRecordInheritanceImp _Inherit;
        private Protocol.List _OpenConnections;
        

        // Display
        private string _name;
        private string _description;
        private string _icon;
        private string _panel;

        // Connection
        private string _hostname;
        private string _username; // Not used in: VNC, Telnet, Rlogin, RAW
        private string _password; // Not used in: Telnet, Rlogin, RAW
        private string _domain; // Not used in: VNC, SSH, Telnet, Rlogin, RAW, Http/Https, ExtApp
        
        // Common to all protocols except ICA
        private int _port;

        // VNC
        private Protocol.VNC.Compression _vncCompression;
        private Protocol.VNC.Encoding _vncEncoding;
        private Protocol.VNC.AuthMode _vncAuthMode;
        private Protocol.VNC.ProxyType _vncProxyType;
        private Protocol.VNC.Colors _vncColors;
        private Protocol.VNC.SmartSizeMode _vncSmartSizeMode;
        private bool _vncViewOnly;
        private string _vncProxyIP;
        private int _vncProxyPort;
        private string _vncProxyUsername;
        private string _vncProxyPassword;
        
        // SSH, Telnet, Rlogin, RAW
        private string _puttySession;

        // Http/Https
        private Protocol.HTTPBase.RenderingEngine _renderingEngine;

        // ICA
        private Protocol.ICA.EncryptionStrength _icaEncryption;

        // External App
        private string _extApp;
        #endregion

        #region Public Properties
        [Browsable(false)]
        public ConnectionRecordMetaData MetaData
        {
            get 
            {
                return _metaData;
            }
        }
        
        [Browsable(false)]
        public ConnectionRecordInheritanceController Inherit
        {
            get { return _Inherit; }
        }

        [Browsable(false)]
        public CredentialRecord Credential
        {
            get { return _credential; }
            set { _credential = value; }
        }

        [Browsable(false)]
        public string ConstantID
        {
            get { return _constantId; }
            set { _constantId = value; }
        }

        [Browsable(false)]
        public ConnectionRecord Parent { get; set; }

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
            TypeConverter(typeof(Icon)),
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
					_hostname = string.Empty;
				else
					_hostname = value.Trim();
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
            TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
        public virtual Protocol.Protocols Protocol
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
            TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
        public Protocol.ICA.EncryptionStrength ICAEncryption
		{
			get { return GetInheritedPropertyValue("ICAEncryption", _icaEncryption); }
			set { _icaEncryption = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameUseConsoleSession"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionUseConsoleSession"), 
            TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
        public bool UseConsoleSession
		{
			get { return GetInheritedPropertyValue("UseConsoleSession", _useConsoleSession); }
			set { _useConsoleSession = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameAuthenticationLevel"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionAuthenticationLevel"), 
            TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
        public Protocol.RDPConnectionProtocolImp.AuthenticationLevel RDPAuthenticationLevel
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
            TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
        public Protocol.HTTPBase.RenderingEngine RenderingEngine
		{
			get { return GetInheritedPropertyValue("RenderingEngine", _renderingEngine); }
			set { _renderingEngine = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameUseCredSsp"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionUseCredSsp"), 
            TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
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
            TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
        public Protocol.RDPConnectionProtocolImp.RDGatewayUsageMethod RDGatewayUsageMethod
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
            TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
        public Protocol.RDPConnectionProtocolImp.RDGatewayUseConnectionCredentials RDGatewayUseConnectionCredentials
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
            TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
        public Protocol.RDPConnectionProtocolImp.RDPResolutions Resolution
		{
			get { return GetInheritedPropertyValue("Resolution", _resolution); }
			set { _resolution = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameAutomaticResize"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionAutomaticResize"), 
            TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
        public bool AutomaticResize
		{
			get { return GetInheritedPropertyValue("AutomaticResize", _automaticResize); }
			set { _automaticResize = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameColors"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionColors"), 
            TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
        public Protocol.RDPConnectionProtocolImp.RDPColors Colors
		{
			get { return GetInheritedPropertyValue("Colors", _colors); }
			set { _colors = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameCacheBitmaps"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionCacheBitmaps"), 
            TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
        public bool CacheBitmaps
		{
			get { return GetInheritedPropertyValue("CacheBitmaps", _cacheBitmaps); }
			set { _cacheBitmaps = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameDisplayWallpaper"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDisplayWallpaper"), 
            TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
        public bool DisplayWallpaper
		{
			get { return GetInheritedPropertyValue("DisplayWallpaper", _displayWallpaper); }
			set { _displayWallpaper = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameDisplayThemes"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDisplayThemes"), 
            TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
        public bool DisplayThemes
		{
			get { return GetInheritedPropertyValue("DisplayThemes", _displayThemes); }
			set { _displayThemes = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameEnableFontSmoothing"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionEnableFontSmoothing"), 
            TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
        public bool EnableFontSmoothing
		{
			get { return GetInheritedPropertyValue("EnableFontSmoothing", _enableFontSmoothing); }
			set { _enableFontSmoothing = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameEnableDesktopComposition"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionEnableDesktopComposition"), 
            TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
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
            TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
        public bool RedirectKeys
		{
			get { return GetInheritedPropertyValue("RedirectKeys", _redirectKeys); }
			set { _redirectKeys = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectDrives"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectDrives"), 
            TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
        public bool RedirectDiskDrives
		{
			get { return GetInheritedPropertyValue("RedirectDiskDrives", _redirectDiskDrives); }
			set { _redirectDiskDrives = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectPrinters"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectPrinters"), 
            TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
        public bool RedirectPrinters
		{
			get { return GetInheritedPropertyValue("RedirectPrinters", _redirectPrinters); }
			set { _redirectPrinters = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectPorts"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectPorts"), 
            TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
        public bool RedirectPorts
		{
			get { return GetInheritedPropertyValue("RedirectPorts", _redirectPorts); }
			set { _redirectPorts = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectSmartCards"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectSmartCards"), 
            TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
        public bool RedirectSmartCards
		{
			get { return GetInheritedPropertyValue("RedirectSmartCards", _redirectSmartCards); }
			set { _redirectSmartCards = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectSounds"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectSounds"), 
            TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
        public Protocol.RDPConnectionProtocolImp.RDPSounds RedirectSound
		{
			get { return GetInheritedPropertyValue("RedirectSound", _redirectSound); }
			set { _redirectSound = value; }
		}
        #endregion
        #region VNC
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameCompression"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionCompression"), 
            TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
        public Protocol.VNC.Compression VNCCompression
		{
			get { return GetInheritedPropertyValue("VNCCompression", _vncCompression); }
			set { _vncCompression = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameEncoding"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionEncoding"), 
            TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
        public Protocol.VNC.Encoding VNCEncoding
		{
			get { return GetInheritedPropertyValue("VNCEncoding", _vncEncoding); }
			set { _vncEncoding = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryConnection", 2), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameAuthenticationMode"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionAuthenticationMode"), 
            TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
        public Protocol.VNC.AuthMode VNCAuthMode
		{
			get { return GetInheritedPropertyValue("VNCAuthMode", _vncAuthMode); }
			set { _vncAuthMode = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyType"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyType"), 
            TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
        public Protocol.VNC.ProxyType VNCProxyType
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
            TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
        public Protocol.VNC.Colors VNCColors
		{
			get { return GetInheritedPropertyValue("VNCColors", _vncColors); }
			set { _vncColors = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameSmartSizeMode"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionSmartSizeMode"), 
            TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
        public Protocol.VNC.SmartSizeMode VNCSmartSizeMode
		{
			get { return GetInheritedPropertyValue("VNCSmartSizeMode", _vncSmartSizeMode); }
			set { _vncSmartSizeMode = value; }
		}
		
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameViewOnly"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionViewOnly"), 
            TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
        public bool VNCViewOnly
		{
			get { return GetInheritedPropertyValue("VNCViewOnly", _vncViewOnly); }
			set { _vncViewOnly = value; }
		}
        #endregion

        #region Non-browsable public properties
        [Browsable(false)]
        public Protocol.List OpenConnections
        {
            get { return _OpenConnections; }
            set { _OpenConnections = value; }
        }
        
        [Browsable(false)]
        public TreeNode TreeNode { get; set; }
        #endregion
        #endregion
        
        #region Constructors
        public ConnectionRecordImp(Protocols protocol)
		{
            this.ConstantID = Tools.Misc.CreateConstantID();
            _metaData = new ConnectionRecordMetaDataImp();
            _credential = new CredentialRecordImp();
            _externalTool = new ExternalToolRecordImp();
            _protocol = new ConnectionProtocolImp(protocol);
            SetDefaults();
		}
		
		public ConnectionRecordImp(Container.Info parent) : this()
		{
			this.MetaData.IsContainer = true;
			this.Parent = parent;
		}
        #endregion
		
        #region Public Methods
		public object Clone()
		{
			ConnectionRecordImp newConnectionInfo = (ConnectionRecordImp) this.MemberwiseClone();
			newConnectionInfo.ConstantID = Tools.Misc.CreateConstantID();
			newConnectionInfo._OpenConnections = new Protocol.List();
			return newConnectionInfo;
		}
		
		public void SetDefaults()
		{
            SetTreeDisplayDefaults();
            SetConnectionDefaults();
            SetProtocolDefaults();
            SetRDGatewayDefaults();
            SetAppearanceDefaults();
            SetRedirectDefaults();
            SetVNCDefaults();
            SetNonBrowsablePropertiesDefaults();
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
				ConnectionRecord parentConnectionInfo = default(ConnectionRecord);
				if (this.MetaData.IsContainer)
				{
					parentConnectionInfo = ((Container.Info)Parent.Parent).ConnectionRecord;
				}
				else
				{
					parentConnectionInfo = Parent;
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
			
		private static int GetDefaultPort(Protocols protocol)
		{
			try
			{
                switch (protocol)
                {
                    case Connection.Protocol.Protocols.RDP:
                        return (int)Connection.Protocol.RDPConnectionProtocolImp.Defaults.Port;
                    case Connection.Protocol.Protocols.VNC:
                        return (int)Connection.Protocol.VNC.Defaults.Port;
                    case Connection.Protocol.Protocols.SSH1:
                        return (int)Connection.Protocol.SSH1.Defaults.Port;
                    case Connection.Protocol.Protocols.SSH2:
                        return (int)Connection.Protocol.SSH2.Defaults.Port;
                    case Connection.Protocol.Protocols.Telnet:
                        return (int)Connection.Protocol.Telnet.Defaults.Port;
                    case Connection.Protocol.Protocols.Rlogin:
                        return (int)Connection.Protocol.Rlogin.Defaults.Port;
                    case Connection.Protocol.Protocols.RAW:
                        return (int)Connection.Protocol.RAW.Defaults.Port;
                    case Connection.Protocol.Protocols.HTTP:
                        return (int)Connection.Protocol.HTTP.Defaults.Port;
                    case Connection.Protocol.Protocols.HTTPS:
                        return (int)Connection.Protocol.HTTPS.Defaults.Port;
                    case Connection.Protocol.Protocols.ICA:
                        return (int)Connection.Protocol.ICA.Defaults.Port;
                    case Connection.Protocol.Protocols.IntApp:
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
            _protocol = (Connection.Protocol.Protocols)System.Enum.Parse(typeof(Connection.Protocol.Protocols), My.Settings.Default.ConDefaultProtocol);
            _extApp = My.Settings.Default.ConDefaultExtApp;
            _port = 0;
            _puttySession = My.Settings.Default.ConDefaultPuttySession;
            _icaEncryption = (Protocol.ICA.EncryptionStrength)System.Enum.Parse(typeof(Protocol.ICA.EncryptionStrength), My.Settings.Default.ConDefaultICAEncryptionStrength);
            _useConsoleSession = My.Settings.Default.ConDefaultUseConsoleSession;
            _rdpAuthenticationLevel = (Protocol.RDPConnectionProtocolImp.AuthenticationLevel)System.Enum.Parse(typeof(Protocol.RDPConnectionProtocolImp.AuthenticationLevel), My.Settings.Default.ConDefaultRDPAuthenticationLevel);
            _loadBalanceInfo = My.Settings.Default.ConDefaultLoadBalanceInfo;
            _renderingEngine = (Protocol.HTTPBase.RenderingEngine)System.Enum.Parse(typeof(Protocol.HTTPBase.RenderingEngine), My.Settings.Default.ConDefaultRenderingEngine);
            _useCredSsp = My.Settings.Default.ConDefaultUseCredSsp;
        }

        private void SetRDGatewayDefaults()
        {
            _rdGatewayUsageMethod = (Protocol.RDPConnectionProtocolImp.RDGatewayUsageMethod)System.Enum.Parse(typeof(Protocol.RDPConnectionProtocolImp.RDGatewayUsageMethod), My.Settings.Default.ConDefaultRDGatewayUsageMethod);
            _rdGatewayHostname = My.Settings.Default.ConDefaultRDGatewayHostname;
            _rdGatewayUseConnectionCredentials = (Protocol.RDPConnectionProtocolImp.RDGatewayUseConnectionCredentials)System.Enum.Parse(typeof(Protocol.RDPConnectionProtocolImp.RDGatewayUseConnectionCredentials), My.Settings.Default.ConDefaultRDGatewayUseConnectionCredentials); ;
            _rdGatewayUsername = My.Settings.Default.ConDefaultRDGatewayUsername;
            _rdGatewayPassword = My.Settings.Default.ConDefaultRDGatewayPassword;
            _rdGatewayDomain = My.Settings.Default.ConDefaultRDGatewayDomain;
        }

        private void SetAppearanceDefaults() 
        {
            _resolution = (Protocol.RDPConnectionProtocolImp.RDPResolutions)System.Enum.Parse(typeof(Protocol.RDPConnectionProtocolImp.RDPResolutions), My.Settings.Default.ConDefaultResolution);
            _automaticResize = My.Settings.Default.ConDefaultAutomaticResize;
            _colors = (Protocol.RDPConnectionProtocolImp.RDPColors)System.Enum.Parse(typeof(Protocol.RDPConnectionProtocolImp.RDPColors), My.Settings.Default.ConDefaultColors);
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
            _redirectSound = (Protocol.RDPConnectionProtocolImp.RDPSounds)System.Enum.Parse(typeof(Protocol.RDPConnectionProtocolImp.RDPSounds), My.Settings.Default.ConDefaultRedirectSound);
        }

        private void SetVNCDefaults()
        {
            _vncCompression = (Protocol.VNC.Compression)System.Enum.Parse(typeof(Protocol.VNC.Compression),My.Settings.Default.ConDefaultVNCCompression);
            _vncEncoding = (Protocol.VNC.Encoding)System.Enum.Parse(typeof(Protocol.VNC.Encoding),My.Settings.Default.ConDefaultVNCEncoding);
            _vncAuthMode = (Protocol.VNC.AuthMode)System.Enum.Parse(typeof(Protocol.VNC.AuthMode),My.Settings.Default.ConDefaultVNCAuthMode);
            _vncProxyType = (Protocol.VNC.ProxyType)System.Enum.Parse(typeof(Protocol.VNC.ProxyType),My.Settings.Default.ConDefaultVNCProxyType);
            _vncProxyIP = My.Settings.Default.ConDefaultVNCProxyIP;
            _vncProxyPort = My.Settings.Default.ConDefaultVNCProxyPort;
            _vncProxyUsername = My.Settings.Default.ConDefaultVNCProxyUsername;
            _vncProxyPassword = My.Settings.Default.ConDefaultVNCProxyPassword;
            _vncColors = (Protocol.VNC.Colors)System.Enum.Parse(typeof(Protocol.VNC.Colors),My.Settings.Default.ConDefaultVNCColors);
            _vncSmartSizeMode = (Protocol.VNC.SmartSizeMode)System.Enum.Parse(typeof(Protocol.VNC.SmartSizeMode),My.Settings.Default.ConDefaultVNCSmartSizeMode);
            _vncViewOnly = My.Settings.Default.ConDefaultVNCViewOnly;
        }

        private void SetNonBrowsablePropertiesDefaults()
        {
            _constantId = Tools.Misc.CreateConstantID();
            _Inherit = new ConnectionRecordInheritanceImp(this);
            _OpenConnections = new Protocol.List();
        }
        #endregion
	}
}