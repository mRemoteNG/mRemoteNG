using System.Collections.Generic;
using System.ComponentModel;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.ICA;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Tools;
using mRemoteNG.Tools.Attributes;


namespace mRemoteNG.Connection
{
    public abstract class AbstractConnectionRecord : INotifyPropertyChanged
    {
        #region Fields

        private string _name;
        private string _description;
        private string _icon;
        private string _panel;

        private string _hostname;
        private string _username = "";
        private string _password = "";
        private string _domain = "";
        private string _vmId = "";
        private bool _useEnhancedMode;

        private string _sshTunnelConnectionName = "";
        private ProtocolType _protocol;
        private RdpVersion _rdpProtocolVersion;
        private string _extApp;
        private int _port;
        private string _sshOptions = "";
        private string _puttySession;
        private IcaProtocol.EncryptionStrength _icaEncryption;
        private bool _useConsoleSession;
        private AuthenticationLevel _rdpAuthenticationLevel;
        private int _rdpMinutesToIdleTimeout;
        private bool _rdpAlertIdleTimeout;
        private string _loadBalanceInfo;
        private HTTPBase.RenderingEngine _renderingEngine;
        private bool _useCredSsp;
        private bool _useVmId;

        private RDGatewayUsageMethod _rdGatewayUsageMethod;
        private string _rdGatewayHostname;
        private RDGatewayUseConnectionCredentials _rdGatewayUseConnectionCredentials;
        private string _rdGatewayUsername;
        private string _rdGatewayPassword;
        private string _rdGatewayDomain;

        private RDPResolutions _resolution;
        private bool _automaticResize;
        private RDPColors _colors;
        private bool _cacheBitmaps;
        private bool _displayWallpaper;
        private bool _displayThemes;
        private bool _enableFontSmoothing;
        private bool _enableDesktopComposition;

        private bool _redirectKeys;
        private bool _redirectDiskDrives;
        private bool _redirectPrinters;
        private bool _redirectClipboard;
        private bool _redirectPorts;
        private bool _redirectSmartCards;
        private RDPSounds _redirectSound;
        private RDPSoundQuality _soundQuality;
        private bool _redirectAudioCapture;

        private string _preExtApp;
        private string _postExtApp;
        private string _macAddress;
        private string _userField;
        private bool _favorite;

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

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryDisplay)),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameName)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionName))]
        public virtual string Name
        {
            get => _name;
            set => SetField(ref _name, value, "Name");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryDisplay)),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameDescription)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionDescription))]
        public virtual string Description
        {
            get => GetPropertyValue("Description", _description);
            set => SetField(ref _description, value, "Description");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryDisplay)),
         TypeConverter(typeof(ConnectionIcon)),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameIcon)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionIcon))]
        public virtual string Icon
        {
            get => GetPropertyValue("Icon", _icon);
            set => SetField(ref _icon, value, "Icon");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryDisplay)),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNamePanel)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionPanel))]
        public virtual string Panel
        {
            get => GetPropertyValue("Panel", _panel);
            set => SetField(ref _panel, value, "Panel");
        }

        #endregion

        #region Connection

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryConnection), 2),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameAddress)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionAddress)),
         UsedInAllProtocolsExcept()]
        public virtual string Hostname
        {
            get => _hostname.Trim();
            set => SetField(ref _hostname, value?.Trim(), "Hostname");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryConnection), 2),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNamePort)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionPort)),
         UsedInAllProtocolsExcept(ProtocolType.ICA)]
        public virtual int Port
        {
            get => GetPropertyValue("Port", _port);
            set => SetField(ref _port, value, "Port");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryConnection), 2),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameUsername)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionUsername)),
         UsedInAllProtocolsExcept(ProtocolType.VNC, ProtocolType.Telnet, ProtocolType.Rlogin, ProtocolType.RAW)]
        public virtual string Username
        {
            get => GetPropertyValue("Username", _username);
            set => SetField(ref _username, Settings.Default.DoNotTrimUsername ? value : value?.Trim(), "Username");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryConnection), 2),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNamePassword)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionPassword)),
         PasswordPropertyText(true),
         UsedInAllProtocolsExcept(ProtocolType.Telnet, ProtocolType.Rlogin, ProtocolType.RAW)]
        public virtual string Password
        {
            get => GetPropertyValue("Password", _password);
            set => SetField(ref _password, value, "Password");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryConnection), 2),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameDomain)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionDomain)),
         UsedInProtocol(ProtocolType.RDP, ProtocolType.ICA, ProtocolType.IntApp)]
        public string Domain
        {
            get => GetPropertyValue("Domain", _domain).Trim();
            set => SetField(ref _domain, value?.Trim(), "Domain");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryConnection), 2),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameVmId)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionVmId)),
         UsedInProtocol(ProtocolType.RDP)]
        public string VmId
        {
            get => GetPropertyValue("VmId", _vmId).Trim();
            set => SetField(ref _vmId, value?.Trim(), "VmId");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryConnection), 2),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameSSHTunnelConnection)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionSSHTunnelConnection)),
         TypeConverter(typeof(SshTunnelTypeConverter)),
         UsedInAllProtocolsExcept()]
        public string SSHTunnelConnectionName
        {
            get => GetPropertyValue("SSHTunnelConnectionName", _sshTunnelConnectionName).Trim();
            set => SetField(ref _sshTunnelConnectionName, value?.Trim(), "SSHTunnelConnectionName");
        }
        #endregion

        #region Protocol

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProtocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameProtocol)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionProtocol)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public virtual ProtocolType Protocol
        {
            get => GetPropertyValue("Protocol", _protocol);
            set => SetField(ref _protocol, value, "Protocol");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProtocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameRdpVersion)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionRdpVersion)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public virtual RdpVersion RdpVersion
        {
            get => GetPropertyValue("RdpVersion", _rdpProtocolVersion);
            set => SetField(ref _rdpProtocolVersion, value, nameof(RdpVersion));
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProtocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameExternalTool)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionExternalTool)),
         TypeConverter(typeof(ExternalToolsTypeConverter)),
         UsedInProtocol(ProtocolType.IntApp)]
        public string ExtApp
        {
            get => GetPropertyValue("ExtApp", _extApp);
            set => SetField(ref _extApp, value, "ExtApp");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProtocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNamePuttySession)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionPuttySession)),
         TypeConverter(typeof(Config.Putty.PuttySessionsManager.SessionList)),
         UsedInProtocol(ProtocolType.SSH1, ProtocolType.SSH2, ProtocolType.Telnet,
            ProtocolType.RAW, ProtocolType.Rlogin)]
        public virtual string PuttySession
        {
            get => GetPropertyValue("PuttySession", _puttySession);
            set => SetField(ref _puttySession, value, "PuttySession");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProtocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameSSHOptions)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionSSHOptions)),
         UsedInProtocol(ProtocolType.SSH1, ProtocolType.SSH2)]
        public virtual string SSHOptions
        {
            get => GetPropertyValue("SSHOptions", _sshOptions);
            set => SetField(ref _sshOptions, value, "SSHOptions");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProtocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameEncryptionStrength)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionEncryptionStrength)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         UsedInProtocol(ProtocolType.ICA)]
        public IcaProtocol.EncryptionStrength ICAEncryptionStrength
        {
            get => GetPropertyValue("ICAEncryptionStrength", _icaEncryption);
            set => SetField(ref _icaEncryption, value, "ICAEncryptionStrength");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProtocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameUseConsoleSession)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionUseConsoleSession)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool UseConsoleSession
        {
            get => GetPropertyValue("UseConsoleSession", _useConsoleSession);
            set => SetField(ref _useConsoleSession, value, "UseConsoleSession");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProtocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameAuthenticationLevel)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionAuthenticationLevel)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public AuthenticationLevel RDPAuthenticationLevel
        {
            get => GetPropertyValue("RDPAuthenticationLevel", _rdpAuthenticationLevel);
            set => SetField(ref _rdpAuthenticationLevel, value, "RDPAuthenticationLevel");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProtocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameRDPMinutesToIdleTimeout)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionRDPMinutesToIdleTimeout)),
         UsedInProtocol(ProtocolType.RDP)]
        public virtual int RDPMinutesToIdleTimeout
        {
            get => GetPropertyValue("RDPMinutesToIdleTimeout", _rdpMinutesToIdleTimeout);
            set
            {
                if (value < 0)
                    value = 0;
                else if (value > 240)
                    value = 240;
                SetField(ref _rdpMinutesToIdleTimeout, value, "RDPMinutesToIdleTimeout");
            }
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProtocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameRDPAlertIdleTimeout)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionRDPAlertIdleTimeout)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool RDPAlertIdleTimeout
        {
            get => GetPropertyValue("RDPAlertIdleTimeout", _rdpAlertIdleTimeout);
            set => SetField(ref _rdpAlertIdleTimeout, value, "RDPAlertIdleTimeout");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProtocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameLoadBalanceInfo)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionLoadBalanceInfo)),
         UsedInProtocol(ProtocolType.RDP)]
        public string LoadBalanceInfo
        {
            get => GetPropertyValue("LoadBalanceInfo", _loadBalanceInfo).Trim();
            set => SetField(ref _loadBalanceInfo, value?.Trim(), "LoadBalanceInfo");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProtocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameRenderingEngine)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionRenderingEngine)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         UsedInProtocol(ProtocolType.HTTP, ProtocolType.HTTPS)]
        public HTTPBase.RenderingEngine RenderingEngine
        {
            get => GetPropertyValue("RenderingEngine", _renderingEngine);
            set => SetField(ref _renderingEngine, value, "RenderingEngine");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProtocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameUseCredSsp)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionUseCredSsp)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool UseCredSsp
        {
            get => GetPropertyValue("UseCredSsp", _useCredSsp);
            set => SetField(ref _useCredSsp, value, "UseCredSsp");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProtocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameUseVmId)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionUseVmId)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool UseVmId
        {
            get => GetPropertyValue("UseVmId", _useVmId);
            set => SetField(ref _useVmId, value, "UseVmId");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProtocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameUseEnhancedMode)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionUseEnhancedMode)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool UseEnhancedMode
        {
            get => GetPropertyValue("UseEnhancedMode", _useEnhancedMode);
            set => SetField(ref _useEnhancedMode, value, "UseEnhancedMode");
        }
        #endregion

        #region RD Gateway

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryGateway), 4),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameRDGatewayUsageMethod)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionRDGatewayUsageMethod)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public RDGatewayUsageMethod RDGatewayUsageMethod
        {
            get => GetPropertyValue("RDGatewayUsageMethod", _rdGatewayUsageMethod);
            set => SetField(ref _rdGatewayUsageMethod, value, "RDGatewayUsageMethod");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryGateway), 4),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameRDGatewayHostname)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionRDGatewayHostname)),
         UsedInProtocol(ProtocolType.RDP)]
        public string RDGatewayHostname
        {
            get => GetPropertyValue("RDGatewayHostname", _rdGatewayHostname).Trim();
            set => SetField(ref _rdGatewayHostname, value?.Trim(), "RDGatewayHostname");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryGateway), 4),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameRDGatewayUseConnectionCredentials)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionRDGatewayUseConnectionCredentials)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public RDGatewayUseConnectionCredentials RDGatewayUseConnectionCredentials
        {
            get => GetPropertyValue("RDGatewayUseConnectionCredentials", _rdGatewayUseConnectionCredentials);
            set => SetField(ref _rdGatewayUseConnectionCredentials, value, "RDGatewayUseConnectionCredentials");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryGateway), 4),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameRDGatewayUsername)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionRDGatewayUsername)),
         UsedInProtocol(ProtocolType.RDP)]
        public string RDGatewayUsername
        {
            get => GetPropertyValue("RDGatewayUsername", _rdGatewayUsername).Trim();
            set => SetField(ref _rdGatewayUsername, value?.Trim(), "RDGatewayUsername");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryGateway), 4),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameRDGatewayPassword)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyNameRDGatewayPassword)),
         PasswordPropertyText(true),
         UsedInProtocol(ProtocolType.RDP)]
        public string RDGatewayPassword
        {
            get => GetPropertyValue("RDGatewayPassword", _rdGatewayPassword);
            set => SetField(ref _rdGatewayPassword, value, "RDGatewayPassword");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryGateway), 4),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameRDGatewayDomain)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionRDGatewayDomain)),
         UsedInProtocol(ProtocolType.RDP)]
        public string RDGatewayDomain
        {
            get => GetPropertyValue("RDGatewayDomain", _rdGatewayDomain).Trim();
            set => SetField(ref _rdGatewayDomain, value?.Trim(), "RDGatewayDomain");
        }

        #endregion

        #region Appearance

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryAppearance), 5),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameResolution)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionResolution)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         UsedInProtocol(ProtocolType.RDP, ProtocolType.ICA)]
        public RDPResolutions Resolution
        {
            get => GetPropertyValue("Resolution", _resolution);
            set => SetField(ref _resolution, value, "Resolution");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryAppearance), 5),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameAutomaticResize)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionAutomaticResize)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool AutomaticResize
        {
            get => GetPropertyValue("AutomaticResize", _automaticResize);
            set => SetField(ref _automaticResize, value, "AutomaticResize");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryAppearance), 5),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameColors)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionColors)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         UsedInProtocol(ProtocolType.RDP, ProtocolType.ICA)]
        public RDPColors Colors
        {
            get => GetPropertyValue("Colors", _colors);
            set => SetField(ref _colors, value, "Colors");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryAppearance), 5),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameCacheBitmaps)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionCacheBitmaps)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP, ProtocolType.ICA)]
        public bool CacheBitmaps
        {
            get => GetPropertyValue("CacheBitmaps", _cacheBitmaps);
            set => SetField(ref _cacheBitmaps, value, "CacheBitmaps");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryAppearance), 5),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameDisplayWallpaper)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionDisplayWallpaper)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool DisplayWallpaper
        {
            get => GetPropertyValue("DisplayWallpaper", _displayWallpaper);
            set => SetField(ref _displayWallpaper, value, "DisplayWallpaper");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryAppearance), 5),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameDisplayThemes)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionDisplayThemes)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool DisplayThemes
        {
            get => GetPropertyValue("DisplayThemes", _displayThemes);
            set => SetField(ref _displayThemes, value, "DisplayThemes");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryAppearance), 5),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameEnableFontSmoothing)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionEnableFontSmoothing)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool EnableFontSmoothing
        {
            get => GetPropertyValue("EnableFontSmoothing", _enableFontSmoothing);
            set => SetField(ref _enableFontSmoothing, value, "EnableFontSmoothing");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryAppearance), 5),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameEnableDesktopComposition)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionEnableDesktopComposition)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool EnableDesktopComposition
        {
            get => GetPropertyValue("EnableDesktopComposition", _enableDesktopComposition);
            set => SetField(ref _enableDesktopComposition, value, "EnableDesktopComposition");
        }

        #endregion

        #region Redirect

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryRedirect), 6),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameRedirectKeys)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionRedirectKeys)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool RedirectKeys
        {
            get => GetPropertyValue("RedirectKeys", _redirectKeys);
            set => SetField(ref _redirectKeys, value, "RedirectKeys");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryRedirect), 6),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameRedirectDrives)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionRedirectDrives)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool RedirectDiskDrives
        {
            get => GetPropertyValue("RedirectDiskDrives", _redirectDiskDrives);
            set => SetField(ref _redirectDiskDrives, value, "RedirectDiskDrives");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryRedirect), 6),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameRedirectPrinters)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionRedirectPrinters)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool RedirectPrinters
        {
            get => GetPropertyValue("RedirectPrinters", _redirectPrinters);
            set => SetField(ref _redirectPrinters, value, "RedirectPrinters");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryRedirect), 6),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameRedirectClipboard)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionRedirectClipboard)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool RedirectClipboard
        {
            get { return GetPropertyValue("RedirectClipboard", _redirectClipboard); }
            set { SetField(ref _redirectClipboard, value, "RedirectClipboard"); }
        }


        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryRedirect), 6),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameRedirectPorts)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionRedirectPorts)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool RedirectPorts
        {
            get => GetPropertyValue("RedirectPorts", _redirectPorts);
            set => SetField(ref _redirectPorts, value, "RedirectPorts");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryRedirect), 6),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameRedirectSmartCards)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionRedirectSmartCards)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool RedirectSmartCards
        {
            get => GetPropertyValue("RedirectSmartCards", _redirectSmartCards);
            set => SetField(ref _redirectSmartCards, value, "RedirectSmartCards");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryRedirect), 6),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameRedirectSounds)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionRedirectSounds)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public RDPSounds RedirectSound
        {
            get => GetPropertyValue("RedirectSound", _redirectSound);
            set => SetField(ref _redirectSound, value, "RedirectSound");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryRedirect), 6),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameSoundQuality)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionSoundQuality)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public RDPSoundQuality SoundQuality
        {
            get => GetPropertyValue("SoundQuality", _soundQuality);
            set => SetField(ref _soundQuality, value, "SoundQuality");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryRedirect), 6),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameRedirectAudioCapture)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionRedirectAudioCapture)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool RedirectAudioCapture
        {
            get => GetPropertyValue("RedirectAudioCapture", _redirectAudioCapture);
            set => SetField(ref _redirectAudioCapture, value, nameof(RedirectAudioCapture));
        }

        #endregion

        #region Misc

        [Browsable(false)] public string ConstantID { get; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryMiscellaneous), 7),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameExternalToolBefore)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionExternalToolBefore)),
         TypeConverter(typeof(ExternalToolsTypeConverter))]
        public virtual string PreExtApp
        {
            get => GetPropertyValue("PreExtApp", _preExtApp);
            set => SetField(ref _preExtApp, value, "PreExtApp");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryMiscellaneous), 7),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameExternalToolAfter)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionExternalToolAfter)),
         TypeConverter(typeof(ExternalToolsTypeConverter))]
        public virtual string PostExtApp
        {
            get => GetPropertyValue("PostExtApp", _postExtApp);
            set => SetField(ref _postExtApp, value, "PostExtApp");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryMiscellaneous), 7),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameMACAddress)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionMACAddress))]
        public virtual string MacAddress
        {
            get => GetPropertyValue("MacAddress", _macAddress);
            set => SetField(ref _macAddress, value, "MacAddress");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryMiscellaneous), 7),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameUser1)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionUser1))]
        public virtual string UserField
        {
            get => GetPropertyValue("UserField", _userField);
            set => SetField(ref _userField, value, "UserField");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryMiscellaneous), 7),
            LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameFavorite)),
            LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionFavorite)),
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public virtual bool Favorite
        {
            get => GetPropertyValue("Favorite", _favorite);
            set => SetField(ref _favorite, value, "Favorite");
        }
        #endregion

        #region VNC
        // TODO: it seems all these VNC properties were added and serialized but
        // never hooked up to the VNC protocol or shown to the user
        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryAppearance), 5),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameCompression)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionCompression)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         UsedInProtocol(ProtocolType.VNC),
         Browsable(false)]
        public ProtocolVNC.Compression VNCCompression
        {
            get => GetPropertyValue("VNCCompression", _vncCompression);
            set => SetField(ref _vncCompression, value, "VNCCompression");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryAppearance), 5),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameEncoding)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionEncoding)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         UsedInProtocol(ProtocolType.VNC),
         Browsable(false)]
        public ProtocolVNC.Encoding VNCEncoding
        {
            get => GetPropertyValue("VNCEncoding", _vncEncoding);
            set => SetField(ref _vncEncoding, value, "VNCEncoding");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryConnection), 2),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameAuthenticationMode)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionAuthenticationMode)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         UsedInProtocol(ProtocolType.VNC),
         Browsable(false)]
        public ProtocolVNC.AuthMode VNCAuthMode
        {
            get => GetPropertyValue("VNCAuthMode", _vncAuthMode);
            set => SetField(ref _vncAuthMode, value, "VNCAuthMode");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProxy), 7),
            LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameVNCProxyType)),
            LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionVNCProxyType)),
            TypeConverter(typeof(MiscTools.EnumTypeConverter)),
            UsedInProtocol(ProtocolType.VNC),
            Browsable(false)]
        public ProtocolVNC.ProxyType VNCProxyType
        {
            get => GetPropertyValue("VNCProxyType", _vncProxyType);
            set => SetField(ref _vncProxyType, value, "VNCProxyType");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProxy), 7),
            LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameVNCProxyAddress)),
            LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionVNCProxyAddress)),
            UsedInProtocol(ProtocolType.VNC),
            Browsable(false)]
        public string VNCProxyIP
        {
            get => GetPropertyValue("VNCProxyIP", _vncProxyIp);
            set => SetField(ref _vncProxyIp, value, "VNCProxyIP");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProxy), 7),
            LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameVNCProxyPort)),
            LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionVNCProxyPort)),
            UsedInProtocol(ProtocolType.VNC),
            Browsable(false)]
        public int VNCProxyPort
        {
            get => GetPropertyValue("VNCProxyPort", _vncProxyPort);
            set => SetField(ref _vncProxyPort, value, "VNCProxyPort");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProxy), 7),
            LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameVNCProxyUsername)),
            LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionVNCProxyUsername)),
            UsedInProtocol(ProtocolType.VNC),
            Browsable(false)]
        public string VNCProxyUsername
        {
            get => GetPropertyValue("VNCProxyUsername", _vncProxyUsername);
            set => SetField(ref _vncProxyUsername, value, "VNCProxyUsername");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProxy), 7),
            LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameVNCProxyPassword)),
            LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionVNCProxyPassword)),
            PasswordPropertyText(true),
            UsedInProtocol(ProtocolType.VNC),
            Browsable(false)]
        public string VNCProxyPassword
        {
            get => GetPropertyValue("VNCProxyPassword", _vncProxyPassword);
            set => SetField(ref _vncProxyPassword, value, "VNCProxyPassword");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryAppearance), 5),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameColors)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionColors)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         UsedInProtocol(ProtocolType.VNC),
         Browsable(false)]
        public ProtocolVNC.Colors VNCColors
        {
            get => GetPropertyValue("VNCColors", _vncColors);
            set => SetField(ref _vncColors, value, "VNCColors");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryAppearance), 5),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameSmartSizeMode)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionSmartSizeMode)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         UsedInProtocol(ProtocolType.VNC)]
        public ProtocolVNC.SmartSizeMode VNCSmartSizeMode
        {
            get => GetPropertyValue("VNCSmartSizeMode", _vncSmartSizeMode);
            set => SetField(ref _vncSmartSizeMode, value, "VNCSmartSizeMode");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryAppearance), 5),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameViewOnly)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionViewOnly)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.VNC)]
        public bool VNCViewOnly
        {
            get => GetPropertyValue("VNCViewOnly", _vncViewOnly);
            set => SetField(ref _vncViewOnly, value, "VNCViewOnly");
        }

        #endregion

        #endregion

        protected AbstractConnectionRecord(string uniqueId)
        {
            ConstantID = uniqueId.ThrowIfNullOrEmpty(nameof(uniqueId));
        }

        protected virtual TPropertyType GetPropertyValue<TPropertyType>(string propertyName, TPropertyType value)
        {
            return (TPropertyType)GetType().GetProperty(propertyName)?.GetValue(this, null);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChangedEvent(object sender, PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(args.PropertyName));
        }

        protected void SetField<T>(ref T field, T value, string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            RaisePropertyChangedEvent(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}