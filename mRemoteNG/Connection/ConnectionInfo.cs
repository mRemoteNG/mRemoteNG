using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using mRemoteNG.App;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.PowerShell;
using mRemoteNG.Connection.Protocol.RAW;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.Rlogin;
using mRemoteNG.Connection.Protocol.SSH;
using mRemoteNG.Connection.Protocol.Telnet;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Container;
using mRemoteNG.Properties;
using mRemoteNG.Tree;
using mRemoteNG.Resources.Language;
using mRemoteNG.Tree.Root;
using System.Runtime.Versioning;

namespace mRemoteNG.Connection
{
    [SupportedOSPlatform("windows")]
    [DefaultProperty("Name")]
    public class ConnectionInfo : AbstractConnectionRecord, IHasParent, IInheritable
    {
        private ConnectionInfoInheritance _inheritance;

        #region Public Properties

        [Browsable(false)]
        public ConnectionInfoInheritance Inheritance
        {
            get => _inheritance;
            set => _inheritance = _inheritance.Parent != this
                ? _inheritance.Clone(this)
                : value;
        }

        [Browsable(false)] public ProtocolList OpenConnections { get; protected set; }

        [Browsable(false)] public virtual bool IsContainer { get; set; }

        [Browsable(false)] public bool IsDefault { get; set; }

        [Browsable(false)] public ContainerInfo Parent { get; internal set; }

        [Browsable(false)]
        public bool IsQuickConnect { get; set; }

        [Browsable(false)]
        public bool PleaseConnect { get; set; }

        #endregion

        #region Constructors

        public ConnectionInfo()
            : this(Guid.NewGuid().ToString())
        {
        }

        public ConnectionInfo(string uniqueId)
            : base(uniqueId)
        {
            SetTreeDisplayDefaults();
            SetConnectionDefaults();
            SetProtocolDefaults();
            SetRemoteDesktopServicesDefaults();
            SetRdGatewayDefaults();
            SetAppearanceDefaults();
            SetRedirectDefaults();
            SetMiscDefaults();
            SetVncDefaults();
            SetNonBrowsablePropertiesDefaults();
            SetDefaults();
        }

        #endregion

        #region Public Methods

        public virtual ConnectionInfo Clone()
        {
            var newConnectionInfo = new ConnectionInfo();
            newConnectionInfo.CopyFrom(this);
            return newConnectionInfo;
        }

        /// <summary>
        /// Copies all connection and inheritance values
        /// from the given <see cref="sourceConnectionInfo"/>.
        /// </summary>
        /// <param name="sourceConnectionInfo"></param>
        public void CopyFrom(ConnectionInfo sourceConnectionInfo)
        {
            var properties = GetType().BaseType?.GetProperties().Where(prop => prop.CanRead && prop.CanWrite);
            if (properties == null) return;
            foreach (var property in properties)
            {
                if (property.Name == nameof(Parent)) continue;
                var remotePropertyValue = property.GetValue(sourceConnectionInfo, null);
                property.SetValue(this, remotePropertyValue, null);
            }

            var clonedInheritance = sourceConnectionInfo.Inheritance.Clone(this);
            Inheritance = clonedInheritance;
        }

        public virtual TreeNodeType GetTreeNodeType()
        {
            return TreeNodeType.Connection;
        }

        private void SetDefaults()
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

        protected virtual IEnumerable<PropertyInfo> GetProperties(string[] excludedPropertyNames)
        {
            var properties = typeof(ConnectionInfo).GetProperties();
            var filteredProperties = properties.Where((prop) => !excludedPropertyNames.Contains(prop.Name));
            return filteredProperties;
        }

        public virtual IEnumerable<PropertyInfo> GetSerializableProperties()
        {
            var excludedProperties = new[]
            {
                "Parent", "Name", "Hostname", "Port", "Inheritance", "OpenConnections",
                "IsContainer", "IsDefault", "PositionID", "ConstantID", "TreeNode", "IsQuickConnect", "PleaseConnect"
            };

            return GetProperties(excludedProperties);
        }

        public virtual void SetParent(ContainerInfo newParent)
        {
            RemoveParent();
            newParent?.AddChild(this);
        }

        public void RemoveParent()
        {
            Parent?.RemoveChild(this);
        }

        public ConnectionInfo GetRootParent()
        {
            return Parent != null ? Parent.GetRootParent() : this;
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
            NoCredentials = 32,
            ViewOnly = 64
        }

        #endregion

        #region Private Methods

        protected override TPropertyType GetPropertyValue<TPropertyType>(string propertyName, TPropertyType value)
        {
            if (!ShouldThisPropertyBeInherited(propertyName))
                return value;

            var couldGetInheritedValue =
                TryGetInheritedPropertyValue<TPropertyType>(propertyName, out var inheritedValue);

            return couldGetInheritedValue
                ? inheritedValue
                : value;
        }

        private bool ShouldThisPropertyBeInherited(string propertyName)
        {
            return
                Inheritance.InheritanceActive &&
                ParentIsValidInheritanceTarget() &&
                IsInheritanceTurnedOnForThisProperty(propertyName);
        }

        private bool ParentIsValidInheritanceTarget()
        {
            return Parent != null;
        }

        private bool IsInheritanceTurnedOnForThisProperty(string propertyName)
        {
            var inheritType = Inheritance.GetType();
            var inheritPropertyInfo = inheritType.GetProperty(propertyName);
            var inheritPropertyValue = inheritPropertyInfo != null && Convert.ToBoolean(inheritPropertyInfo.GetValue(Inheritance, null));
            return inheritPropertyValue;
        }

        private bool TryGetInheritedPropertyValue<TPropertyType>(string propertyName, out TPropertyType inheritedValue)
        {
            try
            {
                var connectionInfoType = Parent.GetType();
                var parentPropertyInfo = connectionInfoType.GetProperty(propertyName);
                if (parentPropertyInfo == null)
                    throw new NullReferenceException(
                        $"Could not retrieve property data for property '{propertyName}' on parent node '{Parent?.Name}'"
                    );

                inheritedValue = (TPropertyType)parentPropertyInfo.GetValue(Parent, null);
                return true;
            }
            catch (Exception e)
            {
                Runtime.MessageCollector.AddExceptionStackTrace($"Error retrieving inherited property '{propertyName}'", e);
                inheritedValue = default(TPropertyType);
                return false;
            }
        }

        private static int GetDefaultPort(ProtocolType protocol)
        {
            try
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (protocol)
                {
                    case ProtocolType.RDP:
                        return (int)RdpProtocol6.Defaults.Port;
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
                        return (int)RawProtocol.Defaults.Port;
                    case ProtocolType.HTTP:
                        return (int)ProtocolHTTP.Defaults.Port;
                    case ProtocolType.HTTPS:
                        return (int)ProtocolHTTPS.Defaults.Port;
                    case ProtocolType.PowerShell:
                        return (int)ProtocolPowerShell.Defaults.Port;
                    case ProtocolType.IntApp:
                        return (int)IntegratedProgram.Defaults.Port;
                }

                return 0;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(Language.ConnectionSetDefaultPortFailed, ex);
                return 0;
            }
        }

        private void SetTreeDisplayDefaults()
        {
            Name = Language.NewConnection;
            Description = Settings.Default.ConDefaultDescription;
            Icon = Settings.Default.ConDefaultIcon;
            Panel = Language.General;
        }

        private void SetConnectionDefaults()
        {
            Hostname = string.Empty;
            ExternalAddressProvider = (ExternalAddressProvider)Enum.Parse(typeof(ExternalAddressProvider), Settings.Default.ConDefaultExternalAddressProvider);
            EC2Region = Settings.Default.ConDefaultEC2Region;
            ExternalCredentialProvider = (ExternalCredentialProvider)Enum.Parse(typeof(ExternalCredentialProvider), Settings.Default.ConDefaultExternalCredentialProvider);
            UserViaAPI = "";
        }

        private void SetProtocolDefaults()
        {
            Protocol = (ProtocolType)Enum.Parse(typeof(ProtocolType), Settings.Default.ConDefaultProtocol);
            ExtApp = Settings.Default.ConDefaultExtApp;
            Port = 0;
            PuttySession = Settings.Default.ConDefaultPuttySession;
            UseConsoleSession = Settings.Default.ConDefaultUseConsoleSession;
            RDPAuthenticationLevel = (AuthenticationLevel)Enum.Parse(typeof(AuthenticationLevel), Settings.Default.ConDefaultRDPAuthenticationLevel);
            RDPMinutesToIdleTimeout = Settings.Default.ConDefaultRDPMinutesToIdleTimeout;
            RDPAlertIdleTimeout = Settings.Default.ConDefaultRDPAlertIdleTimeout;
            LoadBalanceInfo = Settings.Default.ConDefaultLoadBalanceInfo;
            RenderingEngine = (HTTPBase.RenderingEngine)Enum.Parse(typeof(HTTPBase.RenderingEngine), Settings.Default.ConDefaultRenderingEngine);
            UseCredSsp = Settings.Default.ConDefaultUseCredSsp;
            UseRestrictedAdmin = Settings.Default.ConDefaultUseRestrictedAdmin;
            UseRCG = Settings.Default.ConDefaultUseRCG;
            UseVmId = Settings.Default.ConDefaultUseVmId;
            UseEnhancedMode = Settings.Default.ConDefaultUseEnhancedMode;
        }

        private void SetRemoteDesktopServicesDefaults()
        {
            RDPStartProgram = string.Empty;
            RDPStartProgramWorkDir = string.Empty;
        }

        private void SetRdGatewayDefaults()
        {
            RDGatewayUsageMethod = (RDGatewayUsageMethod)Enum.Parse(typeof(RDGatewayUsageMethod), Settings.Default.ConDefaultRDGatewayUsageMethod);
            RDGatewayHostname = Settings.Default.ConDefaultRDGatewayHostname;
            RDGatewayUseConnectionCredentials = (RDGatewayUseConnectionCredentials)Enum.Parse(typeof(RDGatewayUseConnectionCredentials), Settings.Default.ConDefaultRDGatewayUseConnectionCredentials);
            RDGatewayUsername = Settings.Default.ConDefaultRDGatewayUsername;
            RDGatewayPassword = Settings.Default.ConDefaultRDGatewayPassword;
            RDGatewayDomain = Settings.Default.ConDefaultRDGatewayDomain;
            RDGatewayExternalCredentialProvider = (ExternalCredentialProvider)Enum.Parse(typeof(ExternalCredentialProvider), Settings.Default.ConDefaultRDGatewayExternalCredentialProvider);
            RDGatewayUserViaAPI = Settings.Default.ConDefaultRDGatewayUserViaAPI;
        }

        private void SetAppearanceDefaults()
        {
            Resolution = (RDPResolutions)Enum.Parse(typeof(RDPResolutions), Settings.Default.ConDefaultResolution);
            AutomaticResize = Settings.Default.ConDefaultAutomaticResize;
            Colors = (RDPColors)Enum.Parse(typeof(RDPColors), Settings.Default.ConDefaultColors);
            CacheBitmaps = Settings.Default.ConDefaultCacheBitmaps;
            DisplayWallpaper = Settings.Default.ConDefaultDisplayWallpaper;
            DisplayThemes = Settings.Default.ConDefaultDisplayThemes;
            EnableFontSmoothing = Settings.Default.ConDefaultEnableFontSmoothing;
            EnableDesktopComposition = Settings.Default.ConDefaultEnableDesktopComposition;
            DisableFullWindowDrag = Settings.Default.ConDefaultDisableFullWindowDrag;
            DisableMenuAnimations = Settings.Default.ConDefaultDisableMenuAnimations;
            DisableCursorShadow = Settings.Default.ConDefaultDisableCursorShadow;
            DisableCursorBlinking = Settings.Default.ConDefaultDisableCursorBlinking;
        }

        private void SetRedirectDefaults()
        {
            RedirectKeys = Settings.Default.ConDefaultRedirectKeys;
            RedirectDiskDrives = Settings.Default.ConDefaultRedirectDiskDrives;
            RedirectPrinters = Settings.Default.ConDefaultRedirectPrinters;
            RedirectClipboard = Settings.Default.ConDefaultRedirectClipboard;
            RedirectPorts = Settings.Default.ConDefaultRedirectPorts;
            RedirectSmartCards = Settings.Default.ConDefaultRedirectSmartCards;
            RedirectAudioCapture = Settings.Default.ConDefaultRedirectAudioCapture;
            RedirectSound = (RDPSounds)Enum.Parse(typeof(RDPSounds), Settings.Default.ConDefaultRedirectSound);
            SoundQuality = (RDPSoundQuality)Enum.Parse(typeof(RDPSoundQuality), Settings.Default.ConDefaultSoundQuality);
        }

        private void SetMiscDefaults()
        {
            PreExtApp = Settings.Default.ConDefaultPreExtApp;
            PostExtApp = Settings.Default.ConDefaultPostExtApp;
            MacAddress = Settings.Default.ConDefaultMacAddress;
            UserField = Settings.Default.ConDefaultUserField;
            Favorite = Settings.Default.ConDefaultFavorite;
            RDPStartProgram = Settings.Default.ConDefaultRDPStartProgram;
            RDPStartProgramWorkDir = Settings.Default.ConDefaultRDPStartProgramWorkDir;
            OpeningCommand = Settings.Default.OpeningCommand;
        }

        private void SetVncDefaults()
        {
            VNCCompression = (ProtocolVNC.Compression)Enum.Parse(typeof(ProtocolVNC.Compression), Settings.Default.ConDefaultVNCCompression);
            VNCEncoding = (ProtocolVNC.Encoding)Enum.Parse(typeof(ProtocolVNC.Encoding), Settings.Default.ConDefaultVNCEncoding);
            VNCAuthMode = (ProtocolVNC.AuthMode)Enum.Parse(typeof(ProtocolVNC.AuthMode), Settings.Default.ConDefaultVNCAuthMode);
            VNCProxyType = (ProtocolVNC.ProxyType)Enum.Parse(typeof(ProtocolVNC.ProxyType), Settings.Default.ConDefaultVNCProxyType);
            VNCProxyIP = Settings.Default.ConDefaultVNCProxyIP;
            VNCProxyPort = Settings.Default.ConDefaultVNCProxyPort;
            VNCProxyUsername = Settings.Default.ConDefaultVNCProxyUsername;
            VNCProxyPassword = Settings.Default.ConDefaultVNCProxyPassword;
            VNCColors = (ProtocolVNC.Colors)Enum.Parse(typeof(ProtocolVNC.Colors), Settings.Default.ConDefaultVNCColors);
            VNCSmartSizeMode = (ProtocolVNC.SmartSizeMode)Enum.Parse(typeof(ProtocolVNC.SmartSizeMode), Settings.Default.ConDefaultVNCSmartSizeMode);
            VNCViewOnly = Settings.Default.ConDefaultVNCViewOnly;
        }

        private void SetNonBrowsablePropertiesDefaults()
        {
            _inheritance = new ConnectionInfoInheritance(this);
            SetNewOpenConnectionList();
        }

        private void SetNewOpenConnectionList()
        {
            OpenConnections = new ProtocolList();
            OpenConnections.CollectionChanged += (sender, args) => RaisePropertyChangedEvent(this, new PropertyChangedEventArgs("OpenConnections"));
        }

        #endregion
    }
}