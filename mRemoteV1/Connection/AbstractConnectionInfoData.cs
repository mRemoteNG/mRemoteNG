using System.Collections.Generic;
using System.ComponentModel;
using mRemoteNG.Config.Putty;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.ICA;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Tools;

namespace mRemoteNG.Connection
{
    public abstract class AbstractConnectionInfoData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual TPropertyType GetPropertyValue<TPropertyType>(string propertyName, TPropertyType value)
        {
            return (TPropertyType) GetType().GetProperty(propertyName).GetValue(this, null);
        }

        protected virtual void RaisePropertyChangedEvent(object sender, PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(args.PropertyName));
        }

        protected bool SetField<T>(ref T field, T value, string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            RaisePropertyChangedEvent(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        #region Fields

        private string _name;
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

        #region Properties

        #region Display

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryDisplay", 1)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameName")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionName")]
        public virtual string Name
        {
            get { return _name; }
            set { SetField(ref _name, value, "Name"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryDisplay", 1)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameDescription")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionDescription")]
        public virtual string Description
        {
            get { return GetPropertyValue("Description", _description); }
            set { SetField(ref _description, value, "Description"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryDisplay", 1)]
        [TypeConverter(typeof(ConnectionIcon))]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameIcon")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionIcon")]
        public virtual string Icon
        {
            get { return GetPropertyValue("Icon", _icon); }
            set { SetField(ref _icon, value, "Icon"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryDisplay", 1)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNamePanel")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionPanel")]
        public virtual string Panel
        {
            get { return GetPropertyValue("Panel", _panel); }
            set { SetField(ref _panel, value, "Panel"); }
        }

        #endregion

        #region Connection

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryConnection", 2)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameAddress")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionAddress")]
        public virtual string Hostname
        {
            get { return _hostname.Trim(); }
            set { SetField(ref _hostname, value?.Trim(), "Hostname"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryConnection", 2)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameUsername")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionUsername")]
        public virtual string Username
        {
            get { return GetPropertyValue("Username", _username); }
            set { SetField(ref _username, value?.Trim(), "Username"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryConnection", 2)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNamePassword")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionPassword")]
        [PasswordPropertyText(true)]
        public virtual string Password
        {
            get { return GetPropertyValue("Password", _password); }
            set { SetField(ref _password, value, "Password"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryConnection", 2)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameDomain")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionDomain")]
        public string Domain
        {
            get { return GetPropertyValue("Domain", _domain).Trim(); }
            set { SetField(ref _domain, value?.Trim(), "Domain"); }
        }

        #endregion

        #region Protocol

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryProtocol", 3)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameProtocol")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionProtocol")]
        [TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public virtual ProtocolType Protocol
        {
            get { return GetPropertyValue("Protocol", _protocol); }
            set { SetField(ref _protocol, value, "Protocol"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryProtocol", 3)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameExternalTool")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionExternalTool")]
        [TypeConverter(typeof(ExternalToolsTypeConverter))]
        public string ExtApp
        {
            get { return GetPropertyValue("ExtApp", _extApp); }
            set { SetField(ref _extApp, value, "ExtApp"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryProtocol", 3)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNamePort")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionPort")]
        public virtual int Port
        {
            get { return GetPropertyValue("Port", _port); }
            set { SetField(ref _port, value, "Port"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryProtocol", 3)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNamePuttySession")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionPuttySession")]
        [TypeConverter(typeof(PuttySessionsManager.SessionList))]
        public virtual string PuttySession
        {
            get { return GetPropertyValue("PuttySession", _puttySession); }
            set { SetField(ref _puttySession, value, "PuttySession"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryProtocol", 3)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameEncryptionStrength")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionEncryptionStrength")]
        [TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolICA.EncryptionStrength ICAEncryptionStrength
        {
            get { return GetPropertyValue("ICAEncryptionStrength", _icaEncryption); }
            set { SetField(ref _icaEncryption, value, "ICAEncryptionStrength"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryProtocol", 3)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameUseConsoleSession")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionUseConsoleSession")]
        [TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UseConsoleSession
        {
            get { return GetPropertyValue("UseConsoleSession", _useConsoleSession); }
            set { SetField(ref _useConsoleSession, value, "UseConsoleSession"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryProtocol", 3)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameAuthenticationLevel")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionAuthenticationLevel")]
        [TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolRDP.AuthenticationLevel RDPAuthenticationLevel
        {
            get { return GetPropertyValue("RDPAuthenticationLevel", _rdpAuthenticationLevel); }
            set { SetField(ref _rdpAuthenticationLevel, value, "RDPAuthenticationLevel"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryProtocol", 3)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameLoadBalanceInfo")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionLoadBalanceInfo")]
        public string LoadBalanceInfo
        {
            get { return GetPropertyValue("LoadBalanceInfo", _loadBalanceInfo).Trim(); }
            set { SetField(ref _loadBalanceInfo, value?.Trim(), "LoadBalanceInfo"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryProtocol", 3)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameRenderingEngine")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionRenderingEngine")]
        [TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public HTTPBase.RenderingEngine RenderingEngine
        {
            get { return GetPropertyValue("RenderingEngine", _renderingEngine); }
            set { SetField(ref _renderingEngine, value, "RenderingEngine"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryProtocol", 3)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameUseCredSsp")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionUseCredSsp")]
        [TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UseCredSsp
        {
            get { return GetPropertyValue("UseCredSsp", _useCredSsp); }
            set { SetField(ref _useCredSsp, value, "UseCredSsp"); }
        }

        #endregion

        #region RD Gateway

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryGateway", 4)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameRDGatewayUsageMethod")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionRDGatewayUsageMethod")]
        [TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolRDP.RDGatewayUsageMethod RDGatewayUsageMethod
        {
            get { return GetPropertyValue("RDGatewayUsageMethod", _rdGatewayUsageMethod); }
            set { SetField(ref _rdGatewayUsageMethod, value, "RDGatewayUsageMethod"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryGateway", 4)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameRDGatewayHostname")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionRDGatewayHostname")]
        public string RDGatewayHostname
        {
            get { return GetPropertyValue("RDGatewayHostname", _rdGatewayHostname).Trim(); }
            set { SetField(ref _rdGatewayHostname, value?.Trim(), "RDGatewayHostname"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryGateway", 4)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameRDGatewayUseConnectionCredentials")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionRDGatewayUseConnectionCredentials")]
        [TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolRDP.RDGatewayUseConnectionCredentials RDGatewayUseConnectionCredentials
        {
            get { return GetPropertyValue("RDGatewayUseConnectionCredentials", _rdGatewayUseConnectionCredentials); }
            set { SetField(ref _rdGatewayUseConnectionCredentials, value, "RDGatewayUseConnectionCredentials"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryGateway", 4)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameRDGatewayUsername")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionRDGatewayUsername")]
        public string RDGatewayUsername
        {
            get { return GetPropertyValue("RDGatewayUsername", _rdGatewayUsername).Trim(); }
            set { SetField(ref _rdGatewayUsername, value?.Trim(), "RDGatewayUsername"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryGateway", 4)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameRDGatewayPassword")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyNameRDGatewayPassword")]
        [PasswordPropertyText(true)]
        public string RDGatewayPassword
        {
            get { return GetPropertyValue("RDGatewayPassword", _rdGatewayPassword); }
            set { SetField(ref _rdGatewayPassword, value, "RDGatewayPassword"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryGateway", 4)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameRDGatewayDomain")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionRDGatewayDomain")]
        public string RDGatewayDomain
        {
            get { return GetPropertyValue("RDGatewayDomain", _rdGatewayDomain).Trim(); }
            set { SetField(ref _rdGatewayDomain, value?.Trim(), "RDGatewayDomain"); }
        }

        #endregion

        #region Appearance

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryAppearance", 5)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameResolution")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionResolution")]
        [TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolRDP.RDPResolutions Resolution
        {
            get { return GetPropertyValue("Resolution", _resolution); }
            set { SetField(ref _resolution, value, "Resolution"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryAppearance", 5)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameAutomaticResize")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionAutomaticResize")]
        [TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool AutomaticResize
        {
            get { return GetPropertyValue("AutomaticResize", _automaticResize); }
            set { SetField(ref _automaticResize, value, "AutomaticResize"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryAppearance", 5)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameColors")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionColors")]
        [TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolRDP.RDPColors Colors
        {
            get { return GetPropertyValue("Colors", _colors); }
            set { SetField(ref _colors, value, "Colors"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryAppearance", 5)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameCacheBitmaps")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionCacheBitmaps")]
        [TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool CacheBitmaps
        {
            get { return GetPropertyValue("CacheBitmaps", _cacheBitmaps); }
            set { SetField(ref _cacheBitmaps, value, "CacheBitmaps"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryAppearance", 5)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameDisplayWallpaper")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionDisplayWallpaper")]
        [TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool DisplayWallpaper
        {
            get { return GetPropertyValue("DisplayWallpaper", _displayWallpaper); }
            set { SetField(ref _displayWallpaper, value, "DisplayWallpaper"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryAppearance", 5)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameDisplayThemes")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionDisplayThemes")]
        [TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool DisplayThemes
        {
            get { return GetPropertyValue("DisplayThemes", _displayThemes); }
            set { SetField(ref _displayThemes, value, "DisplayThemes"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryAppearance", 5)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameEnableFontSmoothing")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionEnableFontSmoothing")]
        [TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool EnableFontSmoothing
        {
            get { return GetPropertyValue("EnableFontSmoothing", _enableFontSmoothing); }
            set { SetField(ref _enableFontSmoothing, value, "EnableFontSmoothing"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryAppearance", 5)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameEnableDesktopComposition")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionEnableDesktopComposition")]
        [TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool EnableDesktopComposition
        {
            get { return GetPropertyValue("EnableDesktopComposition", _enableDesktopComposition); }
            set { SetField(ref _enableDesktopComposition, value, "EnableDesktopComposition"); }
        }

        #endregion

        #region Redirect

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryRedirect", 6)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameRedirectKeys")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionRedirectKeys")]
        [TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectKeys
        {
            get { return GetPropertyValue("RedirectKeys", _redirectKeys); }
            set { SetField(ref _redirectKeys, value, "RedirectKeys"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryRedirect", 6)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameRedirectDrives")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionRedirectDrives")]
        [TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectDiskDrives
        {
            get { return GetPropertyValue("RedirectDiskDrives", _redirectDiskDrives); }
            set { SetField(ref _redirectDiskDrives, value, "RedirectDiskDrives"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryRedirect", 6)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameRedirectPrinters")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionRedirectPrinters")]
        [TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectPrinters
        {
            get { return GetPropertyValue("RedirectPrinters", _redirectPrinters); }
            set { SetField(ref _redirectPrinters, value, "RedirectPrinters"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryRedirect", 6)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameRedirectPorts")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionRedirectPorts")]
        [TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectPorts
        {
            get { return GetPropertyValue("RedirectPorts", _redirectPorts); }
            set { SetField(ref _redirectPorts, value, "RedirectPorts"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryRedirect", 6)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameRedirectSmartCards")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionRedirectSmartCards")]
        [TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectSmartCards
        {
            get { return GetPropertyValue("RedirectSmartCards", _redirectSmartCards); }
            set { SetField(ref _redirectSmartCards, value, "RedirectSmartCards"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryRedirect", 6)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameRedirectSounds")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionRedirectSounds")]
        [TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolRDP.RDPSounds RedirectSound
        {
            get { return GetPropertyValue("RedirectSound", _redirectSound); }
            set { SetField(ref _redirectSound, value, "RedirectSound"); }
        }

        #endregion

        #region Misc

        [Browsable(false)]
        public string ConstantID { get; set; }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryMiscellaneous", 7)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameExternalToolBefore")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionExternalToolBefore")]
        [TypeConverter(typeof(ExternalToolsTypeConverter))]
        public virtual string PreExtApp
        {
            get { return GetPropertyValue("PreExtApp", _preExtApp); }
            set { SetField(ref _preExtApp, value, "PreExtApp"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryMiscellaneous", 7)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameExternalToolAfter")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionExternalToolAfter")]
        [TypeConverter(typeof(ExternalToolsTypeConverter))]
        public virtual string PostExtApp
        {
            get { return GetPropertyValue("PostExtApp", _postExtApp); }
            set { SetField(ref _postExtApp, value, "PostExtApp"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryMiscellaneous", 7)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameMACAddress")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionMACAddress")]
        public virtual string MacAddress
        {
            get { return GetPropertyValue("MacAddress", _macAddress); }
            set { SetField(ref _macAddress, value, "MacAddress"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryMiscellaneous", 7)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameUser1")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionUser1")]
        public virtual string UserField
        {
            get { return GetPropertyValue("UserField", _userField); }
            set { SetField(ref _userField, value, "UserField"); }
        }

        #endregion

        #region VNC

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryAppearance", 5)]
        [Browsable(false)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameCompression")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionCompression")]
        [TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolVNC.Compression VNCCompression
        {
            get { return GetPropertyValue("VNCCompression", _vncCompression); }
            set { SetField(ref _vncCompression, value, "VNCCompression"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryAppearance", 5)]
        [Browsable(false)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameEncoding")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionEncoding")]
        [TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolVNC.Encoding VNCEncoding
        {
            get { return GetPropertyValue("VNCEncoding", _vncEncoding); }
            set { SetField(ref _vncEncoding, value, "VNCEncoding"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryConnection", 2)]
        [Browsable(false)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameAuthenticationMode")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionAuthenticationMode")]
        [TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolVNC.AuthMode VNCAuthMode
        {
            get { return GetPropertyValue("VNCAuthMode", _vncAuthMode); }
            set { SetField(ref _vncAuthMode, value, "VNCAuthMode"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryMiscellaneous", 7)]
        [Browsable(false)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameVNCProxyType")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionVNCProxyType")]
        [TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolVNC.ProxyType VNCProxyType
        {
            get { return GetPropertyValue("VNCProxyType", _vncProxyType); }
            set { SetField(ref _vncProxyType, value, "VNCProxyType"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryMiscellaneous", 7)]
        [Browsable(false)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameVNCProxyAddress")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionVNCProxyAddress")]
        public string VNCProxyIP
        {
            get { return GetPropertyValue("VNCProxyIP", _vncProxyIp); }
            set { SetField(ref _vncProxyIp, value, "VNCProxyIP"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryMiscellaneous", 7)]
        [Browsable(false)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameVNCProxyPort")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionVNCProxyPort")]
        public int VNCProxyPort
        {
            get { return GetPropertyValue("VNCProxyPort", _vncProxyPort); }
            set { SetField(ref _vncProxyPort, value, "VNCProxyPort"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryMiscellaneous", 7)]
        [Browsable(false)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameVNCProxyUsername")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionVNCProxyUsername")]
        public string VNCProxyUsername
        {
            get { return GetPropertyValue("VNCProxyUsername", _vncProxyUsername); }
            set { SetField(ref _vncProxyUsername, value, "VNCProxyUsername"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryMiscellaneous", 7)]
        [Browsable(false)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameVNCProxyPassword")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionVNCProxyPassword")]
        [PasswordPropertyText(true)]
        public string VNCProxyPassword
        {
            get { return GetPropertyValue("VNCProxyPassword", _vncProxyPassword); }
            set { SetField(ref _vncProxyPassword, value, "VNCProxyPassword"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryAppearance", 5)]
        [Browsable(false)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameColors")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionColors")]
        [TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolVNC.Colors VNCColors
        {
            get { return GetPropertyValue("VNCColors", _vncColors); }
            set { SetField(ref _vncColors, value, "VNCColors"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryAppearance", 5)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameSmartSizeMode")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionSmartSizeMode")]
        [TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolVNC.SmartSizeMode VNCSmartSizeMode
        {
            get { return GetPropertyValue("VNCSmartSizeMode", _vncSmartSizeMode); }
            set { SetField(ref _vncSmartSizeMode, value, "VNCSmartSizeMode"); }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strCategoryAppearance", 5)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPropertyNameViewOnly")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strPropertyDescriptionViewOnly")]
        [TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool VNCViewOnly
        {
            get { return GetPropertyValue("VNCViewOnly", _vncViewOnly); }
            set { SetField(ref _vncViewOnly, value, "VNCViewOnly"); }
        }

        #endregion

        #endregion
    }
}