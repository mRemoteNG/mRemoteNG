using System.Collections.Generic;
using System.ComponentModel;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Properties;
using mRemoteNG.Tools;
using mRemoteNG.Tools.Attributes;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.Connection
{
    [SupportedOSPlatform("windows")]
    public abstract class AbstractConnectionRecord : INotifyPropertyChanged
    {
        #region Fields

        private string _name;
        private string _description;
        private string _icon;
        private string _panel;

        private string _hostname;
        private ExternalAddressProvider _externalAddressProvider;
        private string _ec2InstanceId = "";
        private string _ec2Region = "";
        private ExternalCredentialProvider _externalCredentialProvider;
        private string _userViaAPI = "";
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
        private bool _useConsoleSession;
        private AuthenticationLevel _rdpAuthenticationLevel;
        private int _rdpMinutesToIdleTimeout;
        private bool _rdpAlertIdleTimeout;
        private string _loadBalanceInfo;
        private HTTPBase.RenderingEngine _renderingEngine;
        private bool _useCredSsp;
        private bool _useRestrictedAdmin;
        private bool _useRCG;
        private bool _useVmId;

        private RDGatewayUsageMethod _rdGatewayUsageMethod;
        private string _rdGatewayHostname;
        private RDGatewayUseConnectionCredentials _rdGatewayUseConnectionCredentials;
        private string _rdGatewayUsername;
        private string _rdGatewayPassword;
        private string _rdGatewayDomain;
        private ExternalCredentialProvider _rdGatewayExternalCredentialProvider;
        private string _rdGatewayUserViaAPI = "";


        private RDPResolutions _resolution;
        private bool _automaticResize;
        private RDPColors _colors;
        private bool _cacheBitmaps;
        private bool _displayWallpaper;
        private bool _displayThemes;
        private bool _enableFontSmoothing;
        private bool _enableDesktopComposition;
        private bool _disableFullWindowDrag;
        private bool _disableMenuAnimations;
        private bool _disableCursorShadow;
        private bool _disableCursorBlinking;

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
        private string _openingCommand;
        private string _userField;
        private string _RDPStartProgram;
        private string _RDPStartProgramWorkDir;
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

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Display)),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.Name)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionName))]
        public virtual string Name
        {
            get => _name;
            set => SetField(ref _name, value, "Name");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Display)),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.Description)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionDescription))]
        public virtual string Description
        {
            get => GetPropertyValue("Description", _description);
            set => SetField(ref _description, value, "Description");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Display)),
         TypeConverter(typeof(ConnectionIcon)),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.Icon)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionIcon))]
        public virtual string Icon
        {
            get => GetPropertyValue("Icon", _icon);
            set => SetField(ref _icon, value, "Icon");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Display)),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.Panel)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionPanel))]
        public virtual string Panel
        {
            get => GetPropertyValue("Panel", _panel);
            set => SetField(ref _panel, value, "Panel");
        }

        #endregion

        #region Connection

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Connection), 2),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.HostnameIp)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionHostnameIp)),
         AttributeUsedInAllProtocolsExcept()]
        public virtual string Hostname
        {
            get => _hostname.Trim();
            set => SetField(ref _hostname, value?.Trim(), "Hostname");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Connection), 2),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.Port)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionPort)),
         AttributeUsedInAllProtocolsExcept()]
        public virtual int Port
        {
            get => GetPropertyValue("Port", _port);
            set => SetField(ref _port, value, "Port");
        }

        // external credential provider selector
        [LocalizedAttributes.LocalizedCategory(nameof(Language.Connection), 2),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.ExternalCredentialProvider)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionExternalCredentialProvider)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP, ProtocolType.SSH1, ProtocolType.SSH2)]
        public ExternalCredentialProvider ExternalCredentialProvider
        {
            get => GetPropertyValue("ExternalCredentialProvider", _externalCredentialProvider);
            set => SetField(ref _externalCredentialProvider, value, "ExternalCredentialProvider");
        }

        // credential record identifier for external credential provider
        [LocalizedAttributes.LocalizedCategory(nameof(Language.Connection), 2),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.UserViaAPI)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionUserViaAPI)),
         AttributeUsedInProtocol(ProtocolType.RDP, ProtocolType.SSH1, ProtocolType.SSH2)]
        public virtual string UserViaAPI
        {
            get => GetPropertyValue("UserViaAPI", _userViaAPI);
            set => SetField(ref _userViaAPI, value, "UserViaAPI");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Connection), 2),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.Username)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionUsername)),
         AttributeUsedInProtocol(ProtocolType.RDP, ProtocolType.SSH1, ProtocolType.SSH2)]
        public virtual string Username
        {
            get => GetPropertyValue("Username", _username);
            set => SetField(ref _username, Settings.Default.DoNotTrimUsername ? value : value?.Trim(), "Username");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Connection), 2),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.Password)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionPassword)),
         PasswordPropertyText(true),
         AttributeUsedInAllProtocolsExcept(ProtocolType.Telnet, ProtocolType.Rlogin, ProtocolType.RAW)]
        public virtual string Password
        {
            get => GetPropertyValue("Password", _password);
            set => SetField(ref _password, value, "Password");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Connection), 2),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.Domain)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionDomain)),
         AttributeUsedInProtocol(ProtocolType.RDP, ProtocolType.IntApp, ProtocolType.PowerShell)]
        public string Domain
        {
            get => GetPropertyValue("Domain", _domain).Trim();
            set => SetField(ref _domain, value?.Trim(), "Domain");
        }


        // external address provider selector
        [LocalizedAttributes.LocalizedCategory(nameof(Language.Connection), 2),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.ExternalAddressProvider)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionExternalAddressProvider)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP, ProtocolType.SSH2)]
        public ExternalAddressProvider ExternalAddressProvider
        {
            get => GetPropertyValue("ExternalAddressProvider", _externalAddressProvider);
            set => SetField(ref _externalAddressProvider, value, "ExternalAddressProvider");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Connection), 2),
        LocalizedAttributes.LocalizedDisplayName(nameof(Language.EC2InstanceId)),
        LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionEC2InstanceId)),
        AttributeUsedInProtocol(ProtocolType.RDP, ProtocolType.SSH2)]
        public string EC2InstanceId
        {
            get => GetPropertyValue("EC2InstanceId", _ec2InstanceId).Trim();
            set => SetField(ref _ec2InstanceId, value?.Trim(), "EC2InstanceId");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Connection), 2),
        LocalizedAttributes.LocalizedDisplayName(nameof(Language.EC2Region)),
        LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionEC2Region)),
        AttributeUsedInProtocol(ProtocolType.RDP, ProtocolType.SSH2)]
        public string EC2Region
        {
            get => GetPropertyValue("EC2Region", _ec2Region).Trim();
            set => SetField(ref _ec2Region, value?.Trim(), "EC2Region");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Connection), 2),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.VmId)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionVmId)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public string VmId
        {
            get => GetPropertyValue("VmId", _vmId).Trim();
            set => SetField(ref _vmId, value?.Trim(), "VmId");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Connection), 2),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.SshTunnel)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionSshTunnel)),
         TypeConverter(typeof(SshTunnelTypeConverter)),
         AttributeUsedInAllProtocolsExcept()]
        public string SSHTunnelConnectionName
        {
            get => GetPropertyValue("SSHTunnelConnectionName", _sshTunnelConnectionName).Trim();
            set => SetField(ref _sshTunnelConnectionName, value?.Trim(), "SSHTunnelConnectionName");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous), 7),
        LocalizedAttributes.LocalizedDisplayName(nameof(Language.OpeningCommand)),
        LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionOpeningCommand)),
           AttributeUsedInProtocol(ProtocolType.SSH1, ProtocolType.SSH2)]
        public virtual string OpeningCommand
        {
            get => GetPropertyValue("OpeningCommand", _openingCommand);
            set => SetField(ref _openingCommand, value, "OpeningCommand");
        }
        #endregion

        #region Protocol

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.Protocol)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionProtocol)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public virtual ProtocolType Protocol
        {
            get => GetPropertyValue("Protocol", _protocol);
            set => SetField(ref _protocol, value, "Protocol");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.RdpVersion)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionRdpVersion)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public virtual RdpVersion RdpVersion
        {
            get => GetPropertyValue("RdpVersion", _rdpProtocolVersion);
            set => SetField(ref _rdpProtocolVersion, value, nameof(RdpVersion));
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.ExternalTool)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionExternalTool)),
         TypeConverter(typeof(ExternalToolsTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.IntApp)]
        public string ExtApp
        {
            get => GetPropertyValue("ExtApp", _extApp);
            set => SetField(ref _extApp, value, "ExtApp");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.PuttySession)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionPuttySession)),
         TypeConverter(typeof(Config.Putty.PuttySessionsManager.SessionList)),
         AttributeUsedInProtocol(ProtocolType.SSH1, ProtocolType.SSH2, ProtocolType.Telnet,
            ProtocolType.RAW, ProtocolType.Rlogin)]
        public virtual string PuttySession
        {
            get => GetPropertyValue("PuttySession", _puttySession);
            set => SetField(ref _puttySession, value, "PuttySession");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.SshOptions)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionSshOptions)),
         AttributeUsedInProtocol(ProtocolType.SSH1, ProtocolType.SSH2)]
        public virtual string SSHOptions
        {
            get => GetPropertyValue("SSHOptions", _sshOptions);
            set => SetField(ref _sshOptions, value, "SSHOptions");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.UseConsoleSession)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionUseConsoleSession)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public bool UseConsoleSession
        {
            get => GetPropertyValue("UseConsoleSession", _useConsoleSession);
            set => SetField(ref _useConsoleSession, value, "UseConsoleSession");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.AuthenticationLevel)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionAuthenticationLevel)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public AuthenticationLevel RDPAuthenticationLevel
        {
            get => GetPropertyValue("RDPAuthenticationLevel", _rdpAuthenticationLevel);
            set => SetField(ref _rdpAuthenticationLevel, value, "RDPAuthenticationLevel");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.MinutesToIdleTimeout)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionRDPMinutesToIdleTimeout)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
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

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.MinutesToIdleTimeout)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionRDPAlertIdleTimeout)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public bool RDPAlertIdleTimeout
        {
            get => GetPropertyValue("RDPAlertIdleTimeout", _rdpAlertIdleTimeout);
            set => SetField(ref _rdpAlertIdleTimeout, value, "RDPAlertIdleTimeout");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.LoadBalanceInfo)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionLoadBalanceInfo)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public string LoadBalanceInfo
        {
            get => GetPropertyValue("LoadBalanceInfo", _loadBalanceInfo).Trim();
            set => SetField(ref _loadBalanceInfo, value?.Trim(), "LoadBalanceInfo");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.RenderingEngine)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionRenderingEngine)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.HTTP, ProtocolType.HTTPS)]
        public HTTPBase.RenderingEngine RenderingEngine
        {
            get => GetPropertyValue("RenderingEngine", _renderingEngine);
            set => SetField(ref _renderingEngine, value, "RenderingEngine");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.UseCredSsp)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionUseCredSsp)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public bool UseCredSsp
        {
            get => GetPropertyValue("UseCredSsp", _useCredSsp);
            set => SetField(ref _useCredSsp, value, "UseCredSsp");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.UseRestrictedAdmin)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionUseRestrictedAdmin)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public bool UseRestrictedAdmin
        {
            get => GetPropertyValue("UseRestrictedAdmin", _useRestrictedAdmin);
            set => SetField(ref _useRestrictedAdmin, value, "UseRestrictedAdmin");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.UseRCG)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionUseRCG)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public bool UseRCG
        {
            get => GetPropertyValue("UseRCG", _useRCG);
            set => SetField(ref _useRCG, value, "UseRCG");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.UseVmId)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionUseVmId)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public bool UseVmId
        {
            get => GetPropertyValue("UseVmId", _useVmId);
            set => SetField(ref _useVmId, value, "UseVmId");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 3),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.UseEnhancedMode)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionUseEnhancedMode)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public bool UseEnhancedMode
        {
            get => GetPropertyValue("UseEnhancedMode", _useEnhancedMode);
            set => SetField(ref _useEnhancedMode, value, "UseEnhancedMode");
        }
        #endregion

        #region RD Gateway

        [LocalizedAttributes.LocalizedCategory(nameof(Language.RDPGateway), 4),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.RdpGatewayUsageMethod)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionRdpGatewayUsageMethod)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public RDGatewayUsageMethod RDGatewayUsageMethod
        {
            get => GetPropertyValue("RDGatewayUsageMethod", _rdGatewayUsageMethod);
            set => SetField(ref _rdGatewayUsageMethod, value, "RDGatewayUsageMethod");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.RDPGateway), 4),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.RdpGatewayHostname)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionRDGatewayHostname)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public string RDGatewayHostname
        {
            get => GetPropertyValue("RDGatewayHostname", _rdGatewayHostname).Trim();
            set => SetField(ref _rdGatewayHostname, value?.Trim(), "RDGatewayHostname");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.RDPGateway), 4),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.RdpGatewayUseConnectionCredentials)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionRDGatewayUseConnectionCredentials)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public RDGatewayUseConnectionCredentials RDGatewayUseConnectionCredentials
        {
            get => GetPropertyValue("RDGatewayUseConnectionCredentials", _rdGatewayUseConnectionCredentials);
            set => SetField(ref _rdGatewayUseConnectionCredentials, value, "RDGatewayUseConnectionCredentials");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.RDPGateway), 4),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.RdpGatewayUsername)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionRDGatewayUsername)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public string RDGatewayUsername
        {
            get => GetPropertyValue("RDGatewayUsername", _rdGatewayUsername).Trim();
            set => SetField(ref _rdGatewayUsername, value?.Trim(), "RDGatewayUsername");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.RDPGateway), 4),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.RdpGatewayPassword)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionRdpGatewayPassword)),
         PasswordPropertyText(true),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public string RDGatewayPassword
        {
            get => GetPropertyValue("RDGatewayPassword", _rdGatewayPassword);
            set => SetField(ref _rdGatewayPassword, value, "RDGatewayPassword");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.RDPGateway), 4),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.RdpGatewayDomain)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionRDGatewayDomain)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public string RDGatewayDomain
        {
            get => GetPropertyValue("RDGatewayDomain", _rdGatewayDomain).Trim();
            set => SetField(ref _rdGatewayDomain, value?.Trim(), "RDGatewayDomain");
        }
        // external credential provider selector for rd gateway
        [LocalizedAttributes.LocalizedCategory(nameof(Language.RDPGateway), 4),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.ExternalCredentialProvider)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionExternalCredentialProvider)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public ExternalCredentialProvider RDGatewayExternalCredentialProvider
        {
            get => GetPropertyValue("RDGatewayExternalCredentialProvider", _rdGatewayExternalCredentialProvider);
            set => SetField(ref _rdGatewayExternalCredentialProvider, value, "RDGatewayExternalCredentialProvider");
        }

        // credential record identifier for external credential provider
        [LocalizedAttributes.LocalizedCategory(nameof(Language.RDPGateway), 4),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.UserViaAPI)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionUserViaAPI)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public virtual string RDGatewayUserViaAPI
        {
            get => GetPropertyValue("RDGatewayUserViaAPI", _rdGatewayUserViaAPI);
            set => SetField(ref _rdGatewayUserViaAPI, value, "RDGatewayUserViaAPI");
        }
        #endregion

        #region Appearance

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 5),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.Resolution)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionResolution)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public RDPResolutions Resolution
        {
            get => GetPropertyValue("Resolution", _resolution);
            set => SetField(ref _resolution, value, "Resolution");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 5),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.AutomaticResize)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionAutomaticResize)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public bool AutomaticResize
        {
            get => GetPropertyValue("AutomaticResize", _automaticResize);
            set => SetField(ref _automaticResize, value, "AutomaticResize");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 5),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.Colors)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionColors)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public RDPColors Colors
        {
            get => GetPropertyValue("Colors", _colors);
            set => SetField(ref _colors, value, "Colors");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 5),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.CacheBitmaps)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionCacheBitmaps)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public bool CacheBitmaps
        {
            get => GetPropertyValue("CacheBitmaps", _cacheBitmaps);
            set => SetField(ref _cacheBitmaps, value, "CacheBitmaps");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 5),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.DisplayWallpaper)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionDisplayWallpaper)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public bool DisplayWallpaper
        {
            get => GetPropertyValue("DisplayWallpaper", _displayWallpaper);
            set => SetField(ref _displayWallpaper, value, "DisplayWallpaper");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 5),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.DisplayThemes)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionDisplayThemes)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public bool DisplayThemes
        {
            get => GetPropertyValue("DisplayThemes", _displayThemes);
            set => SetField(ref _displayThemes, value, "DisplayThemes");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 5),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.FontSmoothing)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionEnableFontSmoothing)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public bool EnableFontSmoothing
        {
            get => GetPropertyValue("EnableFontSmoothing", _enableFontSmoothing);
            set => SetField(ref _enableFontSmoothing, value, "EnableFontSmoothing");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 5),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.EnableDesktopComposition)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionEnableDesktopComposition)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public bool EnableDesktopComposition
        {
            get => GetPropertyValue("EnableDesktopComposition", _enableDesktopComposition);
            set => SetField(ref _enableDesktopComposition, value, "EnableDesktopComposition");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 5),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.DisableFullWindowDrag)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionDisableFullWindowDrag)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public bool DisableFullWindowDrag
        {
            get => GetPropertyValue("DisableFullWindowDrag", _disableFullWindowDrag);
            set => SetField(ref _disableFullWindowDrag, value, "DisableFullWindowDrag");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 5),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.DisableMenuAnimations)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionDisableMenuAnimations)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public bool DisableMenuAnimations
        {
            get => GetPropertyValue("DisableMenuAnimations", _disableMenuAnimations);
            set => SetField(ref _disableMenuAnimations, value, "DisableMenuAnimations");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 5),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.DisableCursorShadow)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionDisableCursorShadow)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public bool DisableCursorShadow
        {
            get => GetPropertyValue("DisableCursorShadow", _disableCursorShadow);
            set => SetField(ref _disableCursorShadow, value, "DisableCursorShadow");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 5),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.DisableCursorShadow)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionDisableCursorShadow)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public bool DisableCursorBlinking
        {
            get => GetPropertyValue("DisableCursorBlinking", _disableCursorBlinking);
            set => SetField(ref _disableCursorBlinking, value, "DisableCursorBlinking");
        }
        #endregion

        #region Redirect

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Redirect), 6),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.RedirectKeys)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionRedirectKeys)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public bool RedirectKeys
        {
            get => GetPropertyValue("RedirectKeys", _redirectKeys);
            set => SetField(ref _redirectKeys, value, "RedirectKeys");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Redirect), 6),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.DiskDrives)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionRedirectDrives)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public bool RedirectDiskDrives
        {
            get => GetPropertyValue("RedirectDiskDrives", _redirectDiskDrives);
            set => SetField(ref _redirectDiskDrives, value, "RedirectDiskDrives");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Redirect), 6),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.Printers)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionRedirectPrinters)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public bool RedirectPrinters
        {
            get => GetPropertyValue("RedirectPrinters", _redirectPrinters);
            set => SetField(ref _redirectPrinters, value, "RedirectPrinters");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Redirect), 6),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.Clipboard)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionRedirectClipboard)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public bool RedirectClipboard
        {
            get => GetPropertyValue("RedirectClipboard", _redirectClipboard);
            set => SetField(ref _redirectClipboard, value, "RedirectClipboard");
        }


        [LocalizedAttributes.LocalizedCategory(nameof(Language.Redirect), 6),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.Ports)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionRedirectPorts)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public bool RedirectPorts
        {
            get => GetPropertyValue("RedirectPorts", _redirectPorts);
            set => SetField(ref _redirectPorts, value, "RedirectPorts");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Redirect), 6),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.SmartCard)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionRedirectSmartCards)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public bool RedirectSmartCards
        {
            get => GetPropertyValue("RedirectSmartCards", _redirectSmartCards);
            set => SetField(ref _redirectSmartCards, value, "RedirectSmartCards");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Redirect), 6),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.Sounds)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionRedirectSounds)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public RDPSounds RedirectSound
        {
            get => GetPropertyValue("RedirectSound", _redirectSound);
            set => SetField(ref _redirectSound, value, "RedirectSound");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Redirect), 6),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.SoundQuality)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionSoundQuality)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public RDPSoundQuality SoundQuality
        {
            get => GetPropertyValue("SoundQuality", _soundQuality);
            set => SetField(ref _soundQuality, value, "SoundQuality");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Redirect), 6),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.AudioCapture)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionRedirectAudioCapture)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public bool RedirectAudioCapture
        {
            get => GetPropertyValue("RedirectAudioCapture", _redirectAudioCapture);
            set => SetField(ref _redirectAudioCapture, value, nameof(RedirectAudioCapture));
        }

        #endregion

        #region Misc

        [Browsable(false)] public string ConstantID { get; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous), 7),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.ExternalToolBefore)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionExternalToolBefore)),
         TypeConverter(typeof(ExternalToolsTypeConverter))]
        public virtual string PreExtApp
        {
            get => GetPropertyValue("PreExtApp", _preExtApp);
            set => SetField(ref _preExtApp, value, "PreExtApp");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous), 7),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.ExternalToolAfter)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionExternalToolAfter)),
         TypeConverter(typeof(ExternalToolsTypeConverter))]
        public virtual string PostExtApp
        {
            get => GetPropertyValue("PostExtApp", _postExtApp);
            set => SetField(ref _postExtApp, value, "PostExtApp");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous), 7),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.MacAddress)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionMACAddress))]
        public virtual string MacAddress
        {
            get => GetPropertyValue("MacAddress", _macAddress);
            set => SetField(ref _macAddress, value, "MacAddress");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous), 7),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.UserField)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionUser1))]
        public virtual string UserField
        {
            get => GetPropertyValue("UserField", _userField);
            set => SetField(ref _userField, value, "UserField");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous), 7),
            LocalizedAttributes.LocalizedDisplayName(nameof(Language.Favorite)),
            LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionFavorite)),
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public virtual bool Favorite
        {
            get => GetPropertyValue("Favorite", _favorite);
            set => SetField(ref _favorite, value, "Favorite");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous), 7),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.RDPStartProgram)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionRDPStartProgram)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public virtual string RDPStartProgram
        {
            get => GetPropertyValue("RDPStartProgram", _RDPStartProgram);
            set => SetField(ref _RDPStartProgram, value, "RDPStartProgram");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous), 7),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.RDPStartProgramWorkDir)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionRDPStartProgramWorkDir)),
         AttributeUsedInProtocol(ProtocolType.RDP)]
        public virtual string RDPStartProgramWorkDir
        {
            get => GetPropertyValue("RDPStartProgramWorkDir", _RDPStartProgramWorkDir);
            set => SetField(ref _RDPStartProgramWorkDir, value, "RDPStartProgramWorkDir");
        }

        #endregion

        #region VNC
        // TODO: it seems all these VNC properties were added and serialized but
        // never hooked up to the VNC protocol or shown to the user
        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 5),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.Compression)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionCompression)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.VNC),
         Browsable(false)]
        public ProtocolVNC.Compression VNCCompression
        {
            get => GetPropertyValue("VNCCompression", _vncCompression);
            set => SetField(ref _vncCompression, value, "VNCCompression");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 5),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.Encoding)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionEncoding)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.VNC),
         Browsable(false)]
        public ProtocolVNC.Encoding VNCEncoding
        {
            get => GetPropertyValue("VNCEncoding", _vncEncoding);
            set => SetField(ref _vncEncoding, value, "VNCEncoding");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Connection), 2),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.AuthenticationMode)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionAuthenticationMode)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.VNC),
         Browsable(false)]
        public ProtocolVNC.AuthMode VNCAuthMode
        {
            get => GetPropertyValue("VNCAuthMode", _vncAuthMode);
            set => SetField(ref _vncAuthMode, value, "VNCAuthMode");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Proxy), 7),
            LocalizedAttributes.LocalizedDisplayName(nameof(Language.ProxyType)),
            LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionVNCProxyType)),
            TypeConverter(typeof(MiscTools.EnumTypeConverter)),
            AttributeUsedInProtocol(ProtocolType.VNC),
            Browsable(false)]
        public ProtocolVNC.ProxyType VNCProxyType
        {
            get => GetPropertyValue("VNCProxyType", _vncProxyType);
            set => SetField(ref _vncProxyType, value, "VNCProxyType");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Proxy), 7),
            LocalizedAttributes.LocalizedDisplayName(nameof(Language.ProxyAddress)),
            LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionVNCProxyAddress)),
            AttributeUsedInProtocol(ProtocolType.VNC),
            Browsable(false)]
        public string VNCProxyIP
        {
            get => GetPropertyValue("VNCProxyIP", _vncProxyIp);
            set => SetField(ref _vncProxyIp, value, "VNCProxyIP");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Proxy), 7),
            LocalizedAttributes.LocalizedDisplayName(nameof(Language.ProxyPort)),
            LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionVNCProxyPort)),
            AttributeUsedInProtocol(ProtocolType.VNC),
            Browsable(false)]
        public int VNCProxyPort
        {
            get => GetPropertyValue("VNCProxyPort", _vncProxyPort);
            set => SetField(ref _vncProxyPort, value, "VNCProxyPort");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Proxy), 7),
            LocalizedAttributes.LocalizedDisplayName(nameof(Language.ProxyUsername)),
            LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionVNCProxyUsername)),
            AttributeUsedInProtocol(ProtocolType.VNC),
            Browsable(false)]
        public string VNCProxyUsername
        {
            get => GetPropertyValue("VNCProxyUsername", _vncProxyUsername);
            set => SetField(ref _vncProxyUsername, value, "VNCProxyUsername");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Proxy), 7),
            LocalizedAttributes.LocalizedDisplayName(nameof(Language.ProxyPassword)),
            LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionVNCProxyPassword)),
            PasswordPropertyText(true),
            AttributeUsedInProtocol(ProtocolType.VNC),
            Browsable(false)]
        public string VNCProxyPassword
        {
            get => GetPropertyValue("VNCProxyPassword", _vncProxyPassword);
            set => SetField(ref _vncProxyPassword, value, "VNCProxyPassword");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 5),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.Colors)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionColors)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.VNC),
         Browsable(false)]
        public ProtocolVNC.Colors VNCColors
        {
            get => GetPropertyValue("VNCColors", _vncColors);
            set => SetField(ref _vncColors, value, "VNCColors");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 5),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.SmartSizeMode)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionSmartSizeMode)),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.VNC)]
        public ProtocolVNC.SmartSizeMode VNCSmartSizeMode
        {
            get => GetPropertyValue("VNCSmartSizeMode", _vncSmartSizeMode);
            set => SetField(ref _vncSmartSizeMode, value, "VNCSmartSizeMode");
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 5),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.ViewOnly)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionViewOnly)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         AttributeUsedInProtocol(ProtocolType.VNC)]
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