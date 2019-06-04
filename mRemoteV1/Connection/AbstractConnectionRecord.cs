﻿using System.Collections.Generic;
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

        private ProtocolType _protocol;
        private string _extApp;
        private int _port;
        private string _puttySession;
        private IcaProtocol.EncryptionStrength _icaEncryption;
        private bool _useConsoleSession;
        private AuthenticationLevel _rdpAuthenticationLevel;
        private int _rdpMinutesToIdleTimeout;
        private bool _rdpAlertIdleTimeout;
        private string _loadBalanceInfo;
        private HTTPBase.RenderingEngine _renderingEngine;
        private bool _useCredSsp;

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

        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay"),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameName"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionName")]
        public virtual string Name
        {
            get => _name;
            set => SetField(ref _name, value, "Name");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay"),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameDescription"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDescription")]
        public virtual string Description
        {
            get => GetPropertyValue("Description", _description);
            set => SetField(ref _description, value, "Description");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay"),
         TypeConverter(typeof(ConnectionIcon)),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameIcon"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionIcon")]
        public virtual string Icon
        {
            get => GetPropertyValue("Icon", _icon);
            set => SetField(ref _icon, value, "Icon");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay"),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNamePanel"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionPanel")]
        public virtual string Panel
        {
            get => GetPropertyValue("Panel", _panel);
            set => SetField(ref _panel, value, "Panel");
        }

        #endregion

        #region Connection

        [LocalizedAttributes.LocalizedCategory("strCategoryConnection", 2),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameAddress"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionAddress"),
         UsedInAllProtocolsExcept()]
        public virtual string Hostname
        {
            get => _hostname.Trim();
            set => SetField(ref _hostname, value?.Trim(), "Hostname");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryConnection", 2),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameUsername"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionUsername"),
         UsedInAllProtocolsExcept(ProtocolType.VNC, ProtocolType.Telnet, ProtocolType.Rlogin, ProtocolType.RAW)]
        public virtual string Username
        {
            get => GetPropertyValue("Username", _username);
            set => SetField(ref _username, Settings.Default.DoNotTrimUsername ? value : value?.Trim(), "Username");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryConnection", 2),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNamePassword"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionPassword"),
         PasswordPropertyText(true),
         UsedInAllProtocolsExcept(ProtocolType.Telnet, ProtocolType.Rlogin, ProtocolType.RAW)]
        public virtual string Password
        {
            get => GetPropertyValue("Password", _password);
            set => SetField(ref _password, value, "Password");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryConnection", 2),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameDomain"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDomain"),
         UsedInProtocol(ProtocolType.RDP, ProtocolType.ICA, ProtocolType.IntApp)]
        public string Domain
        {
            get => GetPropertyValue("Domain", _domain).Trim();
            set => SetField(ref _domain, value?.Trim(), "Domain");
        }

        #endregion

        #region Protocol

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameProtocol"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionProtocol"),
         TypeConverter(typeof(MiscTools.EnumTypeConverter))]
        public virtual ProtocolType Protocol
        {
            get => GetPropertyValue("Protocol", _protocol);
            set => SetField(ref _protocol, value, "Protocol");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameExternalTool"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionExternalTool"),
         TypeConverter(typeof(ExternalToolsTypeConverter)),
         UsedInProtocol(ProtocolType.IntApp)]
        public string ExtApp
        {
            get => GetPropertyValue("ExtApp", _extApp);
            set => SetField(ref _extApp, value, "ExtApp");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNamePort"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionPort"),
         UsedInAllProtocolsExcept(ProtocolType.ICA)]
        public virtual int Port
        {
            get => GetPropertyValue("Port", _port);
            set => SetField(ref _port, value, "Port");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNamePuttySession"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionPuttySession"),
         TypeConverter(typeof(Config.Putty.PuttySessionsManager.SessionList)),
         UsedInProtocol(ProtocolType.SSH1, ProtocolType.SSH2, ProtocolType.Telnet,
            ProtocolType.RAW, ProtocolType.Rlogin)]
        public virtual string PuttySession
        {
            get => GetPropertyValue("PuttySession", _puttySession);
            set => SetField(ref _puttySession, value, "PuttySession");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameEncryptionStrength"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionEncryptionStrength"),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         UsedInProtocol(ProtocolType.ICA)]
        public IcaProtocol.EncryptionStrength ICAEncryptionStrength
        {
            get => GetPropertyValue("ICAEncryptionStrength", _icaEncryption);
            set => SetField(ref _icaEncryption, value, "ICAEncryptionStrength");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameUseConsoleSession"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionUseConsoleSession"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool UseConsoleSession
        {
            get => GetPropertyValue("UseConsoleSession", _useConsoleSession);
            set => SetField(ref _useConsoleSession, value, "UseConsoleSession");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameAuthenticationLevel"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionAuthenticationLevel"),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public AuthenticationLevel RDPAuthenticationLevel
        {
            get => GetPropertyValue("RDPAuthenticationLevel", _rdpAuthenticationLevel);
            set => SetField(ref _rdpAuthenticationLevel, value, "RDPAuthenticationLevel");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDPMinutesToIdleTimeout"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDPMinutesToIdleTimeout"),
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

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDPAlertIdleTimeout"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDPAlertIdleTimeout"),
         UsedInProtocol(ProtocolType.RDP)]
        public bool RDPAlertIdleTimeout
        {
            get => GetPropertyValue("RDPAlertIdleTimeout", _rdpAlertIdleTimeout);
            set => SetField(ref _rdpAlertIdleTimeout, value, "RDPAlertIdleTimeout");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameLoadBalanceInfo"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionLoadBalanceInfo"),
         UsedInProtocol(ProtocolType.RDP)]
        public string LoadBalanceInfo
        {
            get => GetPropertyValue("LoadBalanceInfo", _loadBalanceInfo).Trim();
            set => SetField(ref _loadBalanceInfo, value?.Trim(), "LoadBalanceInfo");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameRenderingEngine"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRenderingEngine"),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         UsedInProtocol(ProtocolType.HTTP, ProtocolType.HTTPS)]
        public HTTPBase.RenderingEngine RenderingEngine
        {
            get => GetPropertyValue("RenderingEngine", _renderingEngine);
            set => SetField(ref _renderingEngine, value, "RenderingEngine");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameUseCredSsp"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionUseCredSsp"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool UseCredSsp
        {
            get => GetPropertyValue("UseCredSsp", _useCredSsp);
            set => SetField(ref _useCredSsp, value, "UseCredSsp");
        }

        #endregion

        #region RD Gateway

        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayUsageMethod"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDGatewayUsageMethod"),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public RDGatewayUsageMethod RDGatewayUsageMethod
        {
            get => GetPropertyValue("RDGatewayUsageMethod", _rdGatewayUsageMethod);
            set => SetField(ref _rdGatewayUsageMethod, value, "RDGatewayUsageMethod");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayHostname"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDGatewayHostname"),
         UsedInProtocol(ProtocolType.RDP)]
        public string RDGatewayHostname
        {
            get => GetPropertyValue("RDGatewayHostname", _rdGatewayHostname).Trim();
            set => SetField(ref _rdGatewayHostname, value?.Trim(), "RDGatewayHostname");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayUseConnectionCredentials"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDGatewayUseConnectionCredentials"),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public RDGatewayUseConnectionCredentials RDGatewayUseConnectionCredentials
        {
            get => GetPropertyValue("RDGatewayUseConnectionCredentials", _rdGatewayUseConnectionCredentials);
            set => SetField(ref _rdGatewayUseConnectionCredentials, value, "RDGatewayUseConnectionCredentials");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayUsername"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDGatewayUsername"),
         UsedInProtocol(ProtocolType.RDP)]
        public string RDGatewayUsername
        {
            get => GetPropertyValue("RDGatewayUsername", _rdGatewayUsername).Trim();
            set => SetField(ref _rdGatewayUsername, value?.Trim(), "RDGatewayUsername");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayPassword"),
         LocalizedAttributes.LocalizedDescription("strPropertyNameRDGatewayPassword"),
         PasswordPropertyText(true),
         UsedInProtocol(ProtocolType.RDP)]
        public string RDGatewayPassword
        {
            get => GetPropertyValue("RDGatewayPassword", _rdGatewayPassword);
            set => SetField(ref _rdGatewayPassword, value, "RDGatewayPassword");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayDomain"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDGatewayDomain"),
         UsedInProtocol(ProtocolType.RDP)]
        public string RDGatewayDomain
        {
            get => GetPropertyValue("RDGatewayDomain", _rdGatewayDomain).Trim();
            set => SetField(ref _rdGatewayDomain, value?.Trim(), "RDGatewayDomain");
        }

        #endregion

        #region Appearance

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameResolution"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionResolution"),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         UsedInProtocol(ProtocolType.RDP, ProtocolType.ICA)]
        public RDPResolutions Resolution
        {
            get => GetPropertyValue("Resolution", _resolution);
            set => SetField(ref _resolution, value, "Resolution");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameAutomaticResize"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionAutomaticResize"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool AutomaticResize
        {
            get => GetPropertyValue("AutomaticResize", _automaticResize);
            set => SetField(ref _automaticResize, value, "AutomaticResize");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameColors"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionColors"),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         UsedInProtocol(ProtocolType.RDP, ProtocolType.ICA)]
        public RDPColors Colors
        {
            get => GetPropertyValue("Colors", _colors);
            set => SetField(ref _colors, value, "Colors");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameCacheBitmaps"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionCacheBitmaps"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP, ProtocolType.ICA)]
        public bool CacheBitmaps
        {
            get => GetPropertyValue("CacheBitmaps", _cacheBitmaps);
            set => SetField(ref _cacheBitmaps, value, "CacheBitmaps");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameDisplayWallpaper"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDisplayWallpaper"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool DisplayWallpaper
        {
            get => GetPropertyValue("DisplayWallpaper", _displayWallpaper);
            set => SetField(ref _displayWallpaper, value, "DisplayWallpaper");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameDisplayThemes"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDisplayThemes"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool DisplayThemes
        {
            get => GetPropertyValue("DisplayThemes", _displayThemes);
            set => SetField(ref _displayThemes, value, "DisplayThemes");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameEnableFontSmoothing"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionEnableFontSmoothing"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool EnableFontSmoothing
        {
            get => GetPropertyValue("EnableFontSmoothing", _enableFontSmoothing);
            set => SetField(ref _enableFontSmoothing, value, "EnableFontSmoothing");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameEnableDesktopComposition"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionEnableDesktopComposition"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool EnableDesktopComposition
        {
            get => GetPropertyValue("EnableDesktopComposition", _enableDesktopComposition);
            set => SetField(ref _enableDesktopComposition, value, "EnableDesktopComposition");
        }

        #endregion

        #region Redirect

        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectKeys"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectKeys"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool RedirectKeys
        {
            get => GetPropertyValue("RedirectKeys", _redirectKeys);
            set => SetField(ref _redirectKeys, value, "RedirectKeys");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectDrives"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectDrives"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool RedirectDiskDrives
        {
            get => GetPropertyValue("RedirectDiskDrives", _redirectDiskDrives);
            set => SetField(ref _redirectDiskDrives, value, "RedirectDiskDrives");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectPrinters"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectPrinters"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool RedirectPrinters
        {
            get => GetPropertyValue("RedirectPrinters", _redirectPrinters);
            set => SetField(ref _redirectPrinters, value, "RedirectPrinters");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectClipboard"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectClipboard"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool RedirectClipboard
        {
            get { return GetPropertyValue("RedirectClipboard", _redirectClipboard); }
            set { SetField(ref _redirectClipboard, value, "RedirectClipboard"); }
        }


        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectPorts"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectPorts"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool RedirectPorts
        {
            get => GetPropertyValue("RedirectPorts", _redirectPorts);
            set => SetField(ref _redirectPorts, value, "RedirectPorts");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectSmartCards"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectSmartCards"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool RedirectSmartCards
        {
            get => GetPropertyValue("RedirectSmartCards", _redirectSmartCards);
            set => SetField(ref _redirectSmartCards, value, "RedirectSmartCards");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectSounds"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectSounds"),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public RDPSounds RedirectSound
        {
            get => GetPropertyValue("RedirectSound", _redirectSound);
            set => SetField(ref _redirectSound, value, "RedirectSound");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameSoundQuality"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionSoundQuality"),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public RDPSoundQuality SoundQuality
        {
            get => GetPropertyValue("SoundQuality", _soundQuality);
            set => SetField(ref _soundQuality, value, "SoundQuality");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectAudioCapture"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectAudioCapture"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter)),
         UsedInProtocol(ProtocolType.RDP)]
        public bool RedirectAudioCapture
        {
            get => GetPropertyValue(nameof(RedirectAudioCapture), _redirectAudioCapture);
            set => SetField(ref _redirectAudioCapture, value, nameof(RedirectAudioCapture));
        }

        #endregion

        #region Misc

        [Browsable(false)] public string ConstantID { get; }

        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameExternalToolBefore"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionExternalToolBefore"),
         TypeConverter(typeof(ExternalToolsTypeConverter))]
        public virtual string PreExtApp
        {
            get => GetPropertyValue("PreExtApp", _preExtApp);
            set => SetField(ref _preExtApp, value, "PreExtApp");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameExternalToolAfter"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionExternalToolAfter"),
         TypeConverter(typeof(ExternalToolsTypeConverter))]
        public virtual string PostExtApp
        {
            get => GetPropertyValue("PostExtApp", _postExtApp);
            set => SetField(ref _postExtApp, value, "PostExtApp");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameMACAddress"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionMACAddress")]
        public virtual string MacAddress
        {
            get => GetPropertyValue("MacAddress", _macAddress);
            set => SetField(ref _macAddress, value, "MacAddress");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameUser1"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionUser1")]
        public virtual string UserField
        {
            get => GetPropertyValue("UserField", _userField);
            set => SetField(ref _userField, value, "UserField");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameFavorite"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionFavorite"),
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
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameCompression"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionCompression"),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         UsedInProtocol(ProtocolType.VNC),
         Browsable(false)]
        public ProtocolVNC.Compression VNCCompression
        {
            get => GetPropertyValue("VNCCompression", _vncCompression);
            set => SetField(ref _vncCompression, value, "VNCCompression");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameEncoding"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionEncoding"),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         UsedInProtocol(ProtocolType.VNC),
         Browsable(false)]
        public ProtocolVNC.Encoding VNCEncoding
        {
            get => GetPropertyValue("VNCEncoding", _vncEncoding);
            set => SetField(ref _vncEncoding, value, "VNCEncoding");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryConnection", 2),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameAuthenticationMode"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionAuthenticationMode"),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         UsedInProtocol(ProtocolType.VNC),
         Browsable(false)]
        public ProtocolVNC.AuthMode VNCAuthMode
        {
            get => GetPropertyValue("VNCAuthMode", _vncAuthMode);
            set => SetField(ref _vncAuthMode, value, "VNCAuthMode");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryProxy", 7),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyType"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyType"),
            TypeConverter(typeof(MiscTools.EnumTypeConverter)),
            UsedInProtocol(ProtocolType.VNC),
            Browsable(false)]
        public ProtocolVNC.ProxyType VNCProxyType
        {
            get => GetPropertyValue("VNCProxyType", _vncProxyType);
            set => SetField(ref _vncProxyType, value, "VNCProxyType");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryProxy", 7),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyAddress"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyAddress"),
            UsedInProtocol(ProtocolType.VNC),
            Browsable(false)]
        public string VNCProxyIP
        {
            get => GetPropertyValue("VNCProxyIP", _vncProxyIp);
            set => SetField(ref _vncProxyIp, value, "VNCProxyIP");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryProxy", 7),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyPort"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyPort"),
            UsedInProtocol(ProtocolType.VNC),
            Browsable(false)]
        public int VNCProxyPort
        {
            get => GetPropertyValue("VNCProxyPort", _vncProxyPort);
            set => SetField(ref _vncProxyPort, value, "VNCProxyPort");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryProxy", 7),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyUsername"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyUsername"),
            UsedInProtocol(ProtocolType.VNC),
            Browsable(false)]
        public string VNCProxyUsername
        {
            get => GetPropertyValue("VNCProxyUsername", _vncProxyUsername);
            set => SetField(ref _vncProxyUsername, value, "VNCProxyUsername");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryProxy", 7),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyPassword"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyPassword"),
            PasswordPropertyText(true),
            UsedInProtocol(ProtocolType.VNC),
            Browsable(false)]
        public string VNCProxyPassword
        {
            get => GetPropertyValue("VNCProxyPassword", _vncProxyPassword);
            set => SetField(ref _vncProxyPassword, value, "VNCProxyPassword");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameColors"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionColors"),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         UsedInProtocol(ProtocolType.VNC),
         Browsable(false)]
        public ProtocolVNC.Colors VNCColors
        {
            get => GetPropertyValue("VNCColors", _vncColors);
            set => SetField(ref _vncColors, value, "VNCColors");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameSmartSizeMode"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionSmartSizeMode"),
         TypeConverter(typeof(MiscTools.EnumTypeConverter)),
         UsedInProtocol(ProtocolType.VNC)]
        public ProtocolVNC.SmartSizeMode VNCSmartSizeMode
        {
            get => GetPropertyValue("VNCSmartSizeMode", _vncSmartSizeMode);
            set => SetField(ref _vncSmartSizeMode, value, "VNCSmartSizeMode");
        }

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameViewOnly"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionViewOnly"),
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