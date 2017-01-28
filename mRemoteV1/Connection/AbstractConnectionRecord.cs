using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.ICA;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Credential;
using mRemoteNG.Tools;
using mRemoteNG.UI.Controls;


namespace mRemoteNG.Connection
{
    public abstract class AbstractConnectionRecord : INotifyPropertyChanged
    {
        #region Fields
        private string _name = "";
        private string _description = "";
        private string _icon = "";
        private string _panel = "";

        private string _hostname = "";
        private ICredentialRecord _credentialRecord;

        private ProtocolType _protocol;
        private string _extApp = "";
        private int _port;
        private string _puttySession = "";
        private ProtocolICA.EncryptionStrength _icaEncryption;
        private bool _useConsoleSession;
        private ProtocolRDP.AuthenticationLevel _rdpAuthenticationLevel;
        private int _rdpMinutesToIdleTimeout;
        private bool _rdpAlertIdleTimeout;
        private string _loadBalanceInfo = "";
        private HTTPBase.RenderingEngine _renderingEngine;
        private bool _useCredSsp;

        private ProtocolRDP.RDGatewayUsageMethod _rdGatewayUsageMethod;
        private string _rdGatewayHostname = "";
        private ProtocolRDP.RDGatewayUseConnectionCredentials _rdGatewayUseConnectionCredentials;
        private string _rdGatewayUsername = "";
        private string _rdGatewayPassword = "";
        private string _rdGatewayDomain = "";

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
        private ProtocolRDP.RDPSoundQuality _soundQuality;

        private string _preExtApp = "";
        private string _postExtApp = "";
        private string _macAddress = "";
        private string _userField = "";

        private ProtocolVNC.Compression _vncCompression;
        private ProtocolVNC.Encoding _vncEncoding;
        private ProtocolVNC.AuthMode _vncAuthMode;
        private ProtocolVNC.ProxyType _vncProxyType;
        private string _vncProxyIp = "";
        private int _vncProxyPort;
        private string _vncProxyUsername = "";
        private string _vncProxyPassword = "";
        private ProtocolVNC.Colors _vncColors;
        private ProtocolVNC.SmartSizeMode _vncSmartSizeMode;
        private bool _vncViewOnly;
        #endregion

        #region Properties
        #region Display
        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay"),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameName"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionName")]
        public virtual string Name
        {
            get { return _name; }
            set { SetField(ref _name, value, "Name"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay"),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameDescription"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDescription")]
        public virtual string Description
        {
            get { return GetPropertyValue("Description", _description); }
            set { SetField(ref _description, value, "Description"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay"),
            TypeConverter(typeof(ConnectionIcon)),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameIcon"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionIcon")]
        public virtual string Icon
        {
            get { return GetPropertyValue("Icon", _icon); }
            set { SetField(ref _icon, value, "Icon"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay"),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNamePanel"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionPanel")]
        public virtual string Panel
        {
            get { return GetPropertyValue("Panel", _panel); }
            set { SetField(ref _panel, value, "Panel"); }
        }
        #endregion

        #region Connection
        [LocalizedAttributes.LocalizedCategory("strCategoryConnection", 2),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameAddress"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionAddress")]
        public virtual string Hostname
        {
            get { return _hostname.Trim(); }
            set { SetField(ref _hostname, value?.Trim(), "Hostname"); }
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryConnection), 2),
            LocalizedAttributes.LocalizedDisplayName(nameof(Language.strCategoryCredentials)),
            LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionCredential))]
        [Editor(typeof(CredentialRecordListAdaptor), typeof(UITypeEditor))]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public virtual ICredentialRecord CredentialRecord
        {
            get { return GetPropertyValue(nameof(CredentialRecord), _credentialRecord); }
            set { SetField(ref _credentialRecord, value, nameof(CredentialRecord)); }
        }

        [Obsolete("Use the CredentialRecord property")]
        [Browsable(false)]
        public virtual string Username { get; set; } = "";

        [Obsolete("Use the CredentialRecord property")]
        [Browsable(false)]
        public virtual string Domain { get; set; } = "";

        [Obsolete("Use the CredentialRecord property")]
        [Browsable(false)]
        public virtual string Password { get; set; } = "";
        #endregion

        #region Protocol
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameProtocol"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionProtocol"),
            TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public virtual ProtocolType Protocol
        {
            get { return GetPropertyValue("Protocol", _protocol); }
            set { SetField(ref _protocol, value, "Protocol"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameExternalTool"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionExternalTool"),
            TypeConverter(typeof(ExternalToolsTypeConverter))]
        public string ExtApp
        {
            get { return GetPropertyValue("ExtApp", _extApp); }
            set { SetField(ref _extApp, value, "ExtApp"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNamePort"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionPort")]
        public virtual int Port
        {
            get { return GetPropertyValue("Port", _port); }
            set { SetField(ref _port, value, "Port"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNamePuttySession"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionPuttySession"),
            TypeConverter(typeof(Config.Putty.PuttySessionsManager.SessionList))]
        public virtual string PuttySession
        {
            get { return GetPropertyValue("PuttySession", _puttySession); }
            set { SetField(ref _puttySession, value, "PuttySession"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameEncryptionStrength"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionEncryptionStrength"),
            TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolICA.EncryptionStrength ICAEncryptionStrength
        {
            get { return GetPropertyValue("ICAEncryptionStrength", _icaEncryption); }
            set { SetField(ref _icaEncryption, value, "ICAEncryptionStrength"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameUseConsoleSession"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionUseConsoleSession"),
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UseConsoleSession
        {
            get { return GetPropertyValue("UseConsoleSession", _useConsoleSession); }
            set { SetField(ref _useConsoleSession, value, "UseConsoleSession"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameAuthenticationLevel"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionAuthenticationLevel"),
            TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolRDP.AuthenticationLevel RDPAuthenticationLevel
        {
            get { return GetPropertyValue("RDPAuthenticationLevel", _rdpAuthenticationLevel); }
            set { SetField(ref _rdpAuthenticationLevel, value, "RDPAuthenticationLevel"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDPMinutesToIdleTimeout"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDPMinutesToIdleTimeout")]
        public virtual int RDPMinutesToIdleTimeout
        {
            get { return GetPropertyValue("RDPMinutesToIdleTimeout", _rdpMinutesToIdleTimeout); }
            set
            {
                if (value < 0)
                    value = 0;
                else if (value > 240)
                    value = 240;
                SetField(ref _rdpMinutesToIdleTimeout, value, "RDPMinutesToIdleTimeout");
            }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDPAlertIdleTimeout"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDPAlertIdleTimeout")]
        public bool RDPAlertIdleTimeout
        {
            get { return GetPropertyValue("RDPAlertIdleTimeout", _rdpAlertIdleTimeout); }
            set { SetField(ref _rdpAlertIdleTimeout, value, "RDPAlertIdleTimeout"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameLoadBalanceInfo"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionLoadBalanceInfo")]
        public string LoadBalanceInfo
        {
            get { return GetPropertyValue("LoadBalanceInfo", _loadBalanceInfo).Trim(); }
            set { SetField(ref _loadBalanceInfo, value?.Trim(), "LoadBalanceInfo"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRenderingEngine"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRenderingEngine"),
            TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public HTTPBase.RenderingEngine RenderingEngine
        {
            get { return GetPropertyValue("RenderingEngine", _renderingEngine); }
            set { SetField(ref _renderingEngine, value, "RenderingEngine"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameUseCredSsp"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionUseCredSsp"),
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UseCredSsp
        {
            get { return GetPropertyValue("UseCredSsp", _useCredSsp); }
            set { SetField(ref _useCredSsp, value, "UseCredSsp"); }
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
            set { SetField(ref _rdGatewayUsageMethod, value, "RDGatewayUsageMethod"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayHostname"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDGatewayHostname")]
        public string RDGatewayHostname
        {
            get { return GetPropertyValue("RDGatewayHostname", _rdGatewayHostname).Trim(); }
            set { SetField(ref _rdGatewayHostname, value?.Trim(), "RDGatewayHostname"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayUseConnectionCredentials"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDGatewayUseConnectionCredentials"),
            TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolRDP.RDGatewayUseConnectionCredentials RDGatewayUseConnectionCredentials
        {
            get { return GetPropertyValue("RDGatewayUseConnectionCredentials", _rdGatewayUseConnectionCredentials); }
            set { SetField(ref _rdGatewayUseConnectionCredentials, value, "RDGatewayUseConnectionCredentials"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayUsername"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDGatewayUsername")]
        public string RDGatewayUsername
        {
            get { return GetPropertyValue("RDGatewayUsername", _rdGatewayUsername).Trim(); }
            set { SetField(ref _rdGatewayUsername, value?.Trim(), "RDGatewayUsername"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayPassword"),
            LocalizedAttributes.LocalizedDescription("strPropertyNameRDGatewayPassword"),
            PasswordPropertyText(true)]
        public string RDGatewayPassword
        {
            get { return GetPropertyValue("RDGatewayPassword", _rdGatewayPassword); }
            set { SetField(ref _rdGatewayPassword, value, "RDGatewayPassword"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayDomain"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDGatewayDomain")]
        public string RDGatewayDomain
        {
            get { return GetPropertyValue("RDGatewayDomain", _rdGatewayDomain).Trim(); }
            set { SetField(ref _rdGatewayDomain, value?.Trim(), "RDGatewayDomain"); }
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
            set { SetField(ref _resolution, value, "Resolution"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameAutomaticResize"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionAutomaticResize"),
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool AutomaticResize
        {
            get { return GetPropertyValue("AutomaticResize", _automaticResize); }
            set { SetField(ref _automaticResize, value, "AutomaticResize"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameColors"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionColors"),
            TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolRDP.RDPColors Colors
        {
            get { return GetPropertyValue("Colors", _colors); }
            set { SetField(ref _colors, value, "Colors"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameCacheBitmaps"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionCacheBitmaps"),
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool CacheBitmaps
        {
            get { return GetPropertyValue("CacheBitmaps", _cacheBitmaps); }
            set { SetField(ref _cacheBitmaps, value, "CacheBitmaps"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameDisplayWallpaper"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDisplayWallpaper"),
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool DisplayWallpaper
        {
            get { return GetPropertyValue("DisplayWallpaper", _displayWallpaper); }
            set { SetField(ref _displayWallpaper, value, "DisplayWallpaper"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameDisplayThemes"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDisplayThemes"),
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool DisplayThemes
        {
            get { return GetPropertyValue("DisplayThemes", _displayThemes); }
            set { SetField(ref _displayThemes, value, "DisplayThemes"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameEnableFontSmoothing"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionEnableFontSmoothing"),
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool EnableFontSmoothing
        {
            get { return GetPropertyValue("EnableFontSmoothing", _enableFontSmoothing); }
            set { SetField(ref _enableFontSmoothing, value, "EnableFontSmoothing"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameEnableDesktopComposition"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionEnableDesktopComposition"),
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool EnableDesktopComposition
        {
            get { return GetPropertyValue("EnableDesktopComposition", _enableDesktopComposition); }
            set { SetField(ref _enableDesktopComposition, value, "EnableDesktopComposition"); }
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
            set { SetField(ref _redirectKeys, value, "RedirectKeys"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectDrives"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectDrives"),
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectDiskDrives
        {
            get { return GetPropertyValue("RedirectDiskDrives", _redirectDiskDrives); }
            set { SetField(ref _redirectDiskDrives, value, "RedirectDiskDrives"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectPrinters"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectPrinters"),
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectPrinters
        {
            get { return GetPropertyValue("RedirectPrinters", _redirectPrinters); }
            set { SetField(ref _redirectPrinters, value, "RedirectPrinters"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectPorts"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectPorts"),
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectPorts
        {
            get { return GetPropertyValue("RedirectPorts", _redirectPorts); }
            set { SetField(ref _redirectPorts, value, "RedirectPorts"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectSmartCards"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectSmartCards"),
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectSmartCards
        {
            get { return GetPropertyValue("RedirectSmartCards", _redirectSmartCards); }
            set { SetField(ref _redirectSmartCards, value, "RedirectSmartCards"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectSounds"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectSounds"),
            TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolRDP.RDPSounds RedirectSound
        {
            get { return GetPropertyValue("RedirectSound", _redirectSound); }
            set { SetField(ref _redirectSound, value, "RedirectSound"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameSoundQuality"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionSoundQuality"),
            TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolRDP.RDPSoundQuality SoundQuality
        {
            get { return GetPropertyValue("SoundQuality", _soundQuality); }
            set { SetField(ref _soundQuality, value, "SoundQuality"); }
        }
        #endregion

        #region Misc
        [Browsable(false)]
        public string ConstantID { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameExternalToolBefore"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionExternalToolBefore"),
            TypeConverter(typeof(ExternalToolsTypeConverter))]
        public virtual string PreExtApp
        {
            get { return GetPropertyValue("PreExtApp", _preExtApp); }
            set { SetField(ref _preExtApp, value, "PreExtApp"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameExternalToolAfter"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionExternalToolAfter"),
            TypeConverter(typeof(ExternalToolsTypeConverter))]
        public virtual string PostExtApp
        {
            get { return GetPropertyValue("PostExtApp", _postExtApp); }
            set { SetField(ref _postExtApp, value, "PostExtApp"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameMACAddress"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionMACAddress")]
        public virtual string MacAddress
        {
            get { return GetPropertyValue("MacAddress", _macAddress); }
            set { SetField(ref _macAddress, value, "MacAddress"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameUser1"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionUser1")]
        public virtual string UserField
        {
            get { return GetPropertyValue("UserField", _userField); }
            set { SetField(ref _userField, value, "UserField"); }
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
            set { SetField(ref _vncCompression, value, "VNCCompression"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameEncoding"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionEncoding"),
            TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolVNC.Encoding VNCEncoding
        {
            get { return GetPropertyValue("VNCEncoding", _vncEncoding); }
            set { SetField(ref _vncEncoding, value, "VNCEncoding"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryConnection", 2),
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameAuthenticationMode"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionAuthenticationMode"),
            TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolVNC.AuthMode VNCAuthMode
        {
            get { return GetPropertyValue("VNCAuthMode", _vncAuthMode); }
            set { SetField(ref _vncAuthMode, value, "VNCAuthMode"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyType"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyType"),
            TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolVNC.ProxyType VNCProxyType
        {
            get { return GetPropertyValue("VNCProxyType", _vncProxyType); }
            set { SetField(ref _vncProxyType, value, "VNCProxyType"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyAddress"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyAddress")]
        public string VNCProxyIP
        {
            get { return GetPropertyValue("VNCProxyIP", _vncProxyIp); }
            set { SetField(ref _vncProxyIp, value, "VNCProxyIP"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyPort"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyPort")]
        public int VNCProxyPort
        {
            get { return GetPropertyValue("VNCProxyPort", _vncProxyPort); }
            set { SetField(ref _vncProxyPort, value, "VNCProxyPort"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyUsername"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyUsername")]
        public string VNCProxyUsername
        {
            get { return GetPropertyValue("VNCProxyUsername", _vncProxyUsername); }
            set { SetField(ref _vncProxyUsername, value, "VNCProxyUsername"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyPassword"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyPassword"),
            PasswordPropertyText(true)]
        public string VNCProxyPassword
        {
            get { return GetPropertyValue("VNCProxyPassword", _vncProxyPassword); }
            set { SetField(ref _vncProxyPassword, value, "VNCProxyPassword"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameColors"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionColors"),
            TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolVNC.Colors VNCColors
        {
            get { return GetPropertyValue("VNCColors", _vncColors); }
            set { SetField(ref _vncColors, value, "VNCColors"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameSmartSizeMode"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionSmartSizeMode"),
            TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public ProtocolVNC.SmartSizeMode VNCSmartSizeMode
        {
            get { return GetPropertyValue("VNCSmartSizeMode", _vncSmartSizeMode); }
            set { SetField(ref _vncSmartSizeMode, value, "VNCSmartSizeMode"); }
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameViewOnly"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionViewOnly"),
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool VNCViewOnly
        {
            get { return GetPropertyValue("VNCViewOnly", _vncViewOnly); }
            set { SetField(ref _vncViewOnly, value, "VNCViewOnly"); }
        }
        #endregion
        #endregion

        protected virtual TPropertyType GetPropertyValue<TPropertyType>(string propertyName, TPropertyType value)
        {
            return (TPropertyType)GetType().GetProperty(propertyName).GetValue(this, null);
        }

        public event PropertyChangedEventHandler PropertyChanged;
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
    }
}