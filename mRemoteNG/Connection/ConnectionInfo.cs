using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using mRemoteNG.App;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using mRemoteNG.Connection.Protocol.ARD;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.PowerShell;
using mRemoteNG.Connection.Protocol.Terminal;
using mRemoteNG.Connection.Protocol.WSL;
using mRemoteNG.Connection.Protocol.RAW;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.Rlogin;
using mRemoteNG.Connection.Protocol.Serial;
using mRemoteNG.Connection.Protocol.SSH;
using mRemoteNG.Connection.Protocol.Telnet;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Connection.Protocol.VMRC;
using mRemoteNG.Connection.Protocol.Winbox;
using mRemoteNG.Container;
using mRemoteNG.Properties;
using mRemoteNG.Tree;
using mRemoteNG.Tools;
using mRemoteNG.Resources.Language;
using mRemoteNG.Tree.Root;
using mRemoteNG.PluginSystem;
using System.Runtime.Versioning;

namespace mRemoteNG.Connection
{
    /// <summary>Runtime-only reachability status of a connection's host, updated by the background host-status monitor.</summary>
    public enum HostReachabilityStatus { Unknown, Reachable, Unreachable }

    /// <summary>
    /// Represents a single remote connection with all its configuration properties.
    /// This is the core data model for connections in mRemoteNG — each node in the
    /// connection tree is backed by a <see cref="ConnectionInfo"/> instance.
    /// Inherits connection properties (hostname, protocol, credentials, etc.) from
    /// <see cref="AbstractConnectionRecord"/> and supports property inheritance
    /// from parent folders via <see cref="ConnectionInfoInheritance"/>.
    /// </summary>
    [SupportedOSPlatform("windows")]
    [DefaultProperty("Name")]
    public class ConnectionInfo : AbstractConnectionRecord, IHasParent, IInheritable, IConnectionNode
    {
        private ConnectionInfoInheritance _inheritance = null!;
        private HostReachabilityStatus _hostReachabilityStatus = HostReachabilityStatus.Unknown;

        #region IConnectionNode Implementation
        IEnumerable<IConnectionNode> IConnectionNode.Children => Enumerable.Empty<IConnectionNode>();
        IConnectionNode IConnectionNode.Parent => Parent!;
        string IConnectionNode.Protocol => Protocol.ToString();
        #endregion

        #region Public Properties

        [Browsable(false)]
        public ConnectionInfoInheritance Inheritance
        {
            get => _inheritance;
            set => _inheritance = _inheritance.Parent != this
                ? _inheritance.Clone(this)
                : value;
        }

        [Browsable(false)] public ProtocolList OpenConnections { get; protected set; } = null!;

        [Browsable(false)]
        public bool HasDisconnectedSessions =>
            OpenConnections.Count > 0 && OpenConnections.Cast<ProtocolBase>().Any(p => p.IsSessionDisconnected);

        [Browsable(false)]
        public bool HasActiveSessions =>
            OpenConnections.Count > 0 && OpenConnections.Cast<ProtocolBase>().Any(p => !p.IsSessionDisconnected);

        [Browsable(false)] public virtual bool IsContainer { get; set; }

        [Browsable(false)] public bool IsDefault { get; set; }

        [Browsable(false)] public ContainerInfo? Parent { get; internal set; }

        [Browsable(false)]
        public string LinkedConnectionId { get; set; } = string.Empty;

        [Browsable(false)]
        public bool IsLinkedConnection => !string.IsNullOrWhiteSpace(LinkedConnectionId);

        [Browsable(false)]
        public bool IsQuickConnect { get; set; }

        [Browsable(false)]
        public bool PleaseConnect { get; set; }

        [Browsable(false)]
        public bool IncludeInMultiSsh { get; set; }

        [Browsable(false)]
        public bool ExcludeFromMultiSsh { get; set; }

        [Browsable(false)]
        public string MultiSshScript { get; set; } = string.Empty;

        [Browsable(false)]
        public string User { get; set; }

        [Browsable(false)]
        public string Role { get; set; }

        [Browsable(false)]
        public bool IsRoot { get; set; }

        /// <summary>Runtime-only host reachability status — not persisted to the connections file.</summary>
        [Browsable(false)]
        public HostReachabilityStatus HostReachabilityStatus
        {
            get => _hostReachabilityStatus;
            set => SetField(ref _hostReachabilityStatus, value, nameof(HostReachabilityStatus));
        }

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
            ConnectionInfo newConnectionInfo = new();
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
            PropertyInfo[]? baseProperties = GetType().BaseType?.GetProperties();
            if (baseProperties == null) return;
            IEnumerable<PropertyInfo> properties = baseProperties.Where(prop => prop.CanRead && prop.CanWrite);

            // Temporarily suppress inheritance resolution on the source so we copy
            // the connection's own property values, not values inherited from its parent.
            // Without this, duplicated connections get the parent's resolved values baked
            // into their backing fields, causing inheritance to not work correctly (#229).
            ContainerInfo? savedParent = sourceConnectionInfo.Parent;
            sourceConnectionInfo.Parent = null;
            try
            {
                foreach (PropertyInfo property in properties)
                {
                    if (property.Name == nameof(Parent)) continue;
                    object? remotePropertyValue = property.GetValue(sourceConnectionInfo, null);
                    property.SetValue(this, remotePropertyValue, null);
                }
            }
            finally
            {
                sourceConnectionInfo.Parent = savedParent;
            }

            MultiSshScript = sourceConnectionInfo.MultiSshScript;

            ConnectionInfoInheritance clonedInheritance = sourceConnectionInfo.Inheritance.Clone(this);
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
            PropertyInfo[] properties = typeof(ConnectionInfo).GetProperties();
            IEnumerable<PropertyInfo> filteredProperties = properties.Where((prop) => !excludedPropertyNames.Contains(prop.Name));
            return filteredProperties;
        }

        public virtual IEnumerable<PropertyInfo> GetSerializableProperties()
        {
            string[] excludedProperties = new[]
            {
                "Parent", "Name", "Hostname", "Port", "Inheritance", "OpenConnections",
                "IsContainer", "IsDefault", "PositionID", "ConstantID", "TreeNode", "IsQuickConnect", "PleaseConnect",
                "IncludeInMultiSsh", "ExcludeFromMultiSsh", "MultiSshScript", "LinkedConnectionId", "IsLinkedConnection",
                "User", "Role", "IsRoot", "HasDisconnectedSessions", "HasActiveSessions", "HostReachabilityStatus",
                "CredentialId"
            };

            return GetProperties(excludedProperties);
        }

        public virtual void SetParent(ContainerInfo containerInfo)
        {
            RemoveParent();
            containerInfo?.AddChild(this);
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
            ViewOnly = 64,
            UseAlternativeAddress = 128
        }

        #endregion

        #region Private Methods

        protected override TPropertyType GetPropertyValue<TPropertyType>(string propertyName, TPropertyType value)
        {
            if (TryGetCredentialRecordValue(propertyName, out TPropertyType credentialValue))
                return credentialValue;

            if (TryGetLinkedPropertyValue(propertyName, out TPropertyType linkedValue))
                return linkedValue;

            if (!ShouldThisPropertyBeInherited(propertyName))
                return value;

            bool couldGetInheritedValue =
                TryGetInheritedPropertyValue<TPropertyType>(propertyName, out TPropertyType inheritedValue);

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

        private bool TryGetCredentialRecordValue<TPropertyType>(string propertyName, out TPropertyType credentialValue)
        {
            credentialValue = default!;

            if (!IsCredentialProperty(propertyName)) return false;

            string credId = CredentialId;
            if (string.IsNullOrEmpty(credId)) return false;
            if (!Guid.TryParse(credId, out Guid credentialId)) return false;

            ICredentialRecord? record = Runtime.CredentialProviderCatalog.GetCredentialRecord(credentialId);
            if (record == null) return false;

            object? rawValue = propertyName switch
            {
                nameof(Username) => record.Username,
                nameof(Password) => record.Password?.ConvertToUnsecureString() ?? string.Empty,
                nameof(Domain) => record.Domain,
                _ => null
            };

            if (rawValue is not TPropertyType typed) return false;
            credentialValue = typed;
            return true;
        }

        private bool TryGetLinkedPropertyValue<TPropertyType>(string propertyName, out TPropertyType linkedValue)
        {
            linkedValue = default!;

            if (!IsLinkedConnection)
                return false;

            ConnectionTreeModel? connectionTreeModel = Runtime.ConnectionsService.ConnectionTreeModel;
            ConnectionInfo? linkedSource = connectionTreeModel?.ResolveLinkedConnection(this);
            if (linkedSource == null || ReferenceEquals(linkedSource, this))
                return false;

            PropertyInfo? sourceProperty = linkedSource.GetType().GetProperty(propertyName);
            if (sourceProperty == null)
                return false;

            object? sourceValue = sourceProperty.GetValue(linkedSource, null);
            if (sourceValue is not TPropertyType typedSourceValue)
                return false;

            linkedValue = typedSourceValue;
            return true;
        }

        private bool ParentIsValidInheritanceTarget()
        {
            return Parent != null;
        }

        private bool IsInheritanceTurnedOnForThisProperty(string propertyName)
        {
            Type inheritType = Inheritance.GetType();
            PropertyInfo? inheritPropertyInfo = inheritType.GetProperty(propertyName);
            bool inheritPropertyValue = inheritPropertyInfo != null && Convert.ToBoolean(inheritPropertyInfo.GetValue(Inheritance, null), CultureInfo.InvariantCulture);
            return inheritPropertyValue;
        }

        private bool TryGetInheritedPropertyValue<TPropertyType>(string propertyName, out TPropertyType inheritedValue)
        {
            var currentParent = Parent;
            while (currentParent != null)
            {
                try
                {
                    Type connectionInfoType = currentParent.GetType();
                    PropertyInfo? parentPropertyInfo = connectionInfoType.GetProperty(propertyName);
                    if (parentPropertyInfo == null)
                        throw new InvalidOperationException(
                            $"Could not retrieve property data for property '{propertyName}' on parent node '{currentParent.Name}'"
                        );

                    object? rawValue = parentPropertyInfo.GetValue(currentParent, null);
                    inheritedValue = rawValue is TPropertyType typed ? typed : default!;

                    if (IsCredentialProperty(propertyName) && IsValueEmpty(inheritedValue))
                    {
                        currentParent = currentParent.Parent;
                        continue;
                    }

                    return true;
                }
                catch (Exception e)
                {
                    Runtime.MessageCollector.AddExceptionStackTrace($"Error retrieving inherited property '{propertyName}'", e);
                    inheritedValue = default!;
                    return false;
                }
            }

            inheritedValue = default!;
            return false;
        }

        private static bool IsCredentialProperty(string propertyName)
        {
            return propertyName == nameof(Username) ||
                   propertyName == nameof(Password) ||
                   propertyName == nameof(Domain);
        }

        private static bool IsValueEmpty(object? value)
        {
            if (value == null) return true;
            if (value is string s) return string.IsNullOrEmpty(s);
            return false;
        }

        private static int GetDefaultPort(ProtocolType protocol)
        {
            try
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (protocol)
                {
                    case ProtocolType.RDP:
                        return (int)RdpProtocol.Defaults.Port;
                    case ProtocolType.VNC:
                        return (int)ProtocolVNC.Defaults.Port;
                    case ProtocolType.ARD:
                        return (int)ProtocolARD.Defaults.Port;
                    case ProtocolType.SSH1:
                        return (int)ProtocolSSH1.Defaults.Port;
                    case ProtocolType.SSH2:
                        return (int)ProtocolSSH2.Defaults.Port;
                    case ProtocolType.OpenSSH:
                        return (int)ProtocolOpenSSH.Defaults.Port;
                    case ProtocolType.Telnet:
                        return (int)ProtocolTelnet.Defaults.Port;
                    case ProtocolType.Rlogin:
                        return (int)ProtocolRlogin.Defaults.Port;
                    case ProtocolType.RAW:
                        return (int)RawProtocol.Defaults.Port;
                    case ProtocolType.Serial:
                        return (int)ProtocolSerial.Defaults.Port;
                    case ProtocolType.HTTP:
                        return (int)ProtocolHTTP.Defaults.Port;
                    case ProtocolType.HTTPS:
                        return (int)ProtocolHTTPS.Defaults.Port;
                    case ProtocolType.PowerShell:
                        return (int)ProtocolPowerShell.Defaults.Port;
                    case ProtocolType.WSL:
                        return (int)ProtocolWSL.Defaults.Port;
                    case ProtocolType.Terminal:
                        return (int)ProtocolTerminal.Defaults.Port;
                    case ProtocolType.IntApp:
                        return (int)IntegratedProgram.Defaults.Port;
                    case ProtocolType.VMRC:
                        return (int)ProtocolVMRC.Defaults.Port;
                    case ProtocolType.Winbox:
                        return (int)ProtocolWinbox.Defaults.Port;
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
            Panel = "General";
            Color = string.Empty;
            TabColor = string.Empty;
            ConnectionFrameColor = ConnectionFrameColor.None;
        }

        private void SetConnectionDefaults()
        {
            Hostname = string.Empty;
            ExternalAddressProvider = Enum.Parse<ExternalAddressProvider>(Settings.Default.ConDefaultExternalAddressProvider);
            EC2Region = Settings.Default.ConDefaultEC2Region;
            ExternalCredentialProvider = Enum.Parse<ExternalCredentialProvider>(Settings.Default.ConDefaultExternalCredentialProvider);
            UserViaAPI = "";
        }

        private void SetProtocolDefaults()
        {
            Protocol = Enum.Parse<ProtocolType>(Settings.Default.ConDefaultProtocol);
            ExtApp = Settings.Default.ConDefaultExtApp;
            Port = 0;
            PuttySession = Settings.Default.ConDefaultPuttySession;
            UseConsoleSession = Settings.Default.ConDefaultUseConsoleSession;
            RDPAuthenticationLevel = Enum.Parse<AuthenticationLevel>(Settings.Default.ConDefaultRDPAuthenticationLevel);
            RDPMinutesToIdleTimeout = Settings.Default.ConDefaultRDPMinutesToIdleTimeout;
            RDPAlertIdleTimeout = Settings.Default.ConDefaultRDPAlertIdleTimeout;
            LoadBalanceInfo = Settings.Default.ConDefaultLoadBalanceInfo;
            RenderingEngine = Enum.Parse<HTTPBase.RenderingEngine>(Settings.Default.ConDefaultRenderingEngine);
            UseCredSsp = Settings.Default.ConDefaultUseCredSsp;
            UseRestrictedAdmin = Settings.Default.ConDefaultUseRestrictedAdmin;
            UseRCG = Settings.Default.ConDefaultUseRCG;
            UseVmId = Settings.Default.ConDefaultUseVmId;
            UseEnhancedMode = Settings.Default.ConDefaultUseEnhancedMode;
            SSHOptions = "";
            PrivateKeyPath = Settings.Default.ConDefaultPrivateKeyPath;
            UsePersistentBrowser = Settings.Default.ConDefaultUsePersistentBrowser;
        }

        private void SetRemoteDesktopServicesDefaults()
        {
            RDPStartProgram = string.Empty;
            RDPStartProgramWorkDir = string.Empty;
        }

        private void SetRdGatewayDefaults()
        {
            RDGatewayUsageMethod = Enum.Parse<RDGatewayUsageMethod>(Settings.Default.ConDefaultRDGatewayUsageMethod);
            RDGatewayHostname = Settings.Default.ConDefaultRDGatewayHostname;
            RDGatewayUseConnectionCredentials = Enum.Parse<RDGatewayUseConnectionCredentials>(Settings.Default.ConDefaultRDGatewayUseConnectionCredentials);
            RDGatewayUsername = Settings.Default.ConDefaultRDGatewayUsername;
            RDGatewayPassword = Settings.Default.ConDefaultRDGatewayPassword;
            RDGatewayDomain = Settings.Default.ConDefaultRDGatewayDomain;
            RDGatewayExternalCredentialProvider = Enum.Parse<ExternalCredentialProvider>(Settings.Default.ConDefaultRDGatewayExternalCredentialProvider);
            RDGatewayUserViaAPI = Settings.Default.ConDefaultRDGatewayUserViaAPI;
        }

        private void SetAppearanceDefaults()
        {
            Resolution = Enum.Parse<RDPResolutions>(Settings.Default.ConDefaultResolution);
            DesktopScaleFactor = RDPDesktopScaleFactor.Auto;
            AutomaticResize = Settings.Default.ConDefaultAutomaticResize;
            Colors = Enum.Parse<RDPColors>(Settings.Default.ConDefaultColors);
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
            RedirectDiskDrives = Enum.TryParse(Settings.Default.ConDefaultRedirectDiskDrives, out RDPDiskDrives parsedDiskDrives)
                ? parsedDiskDrives
                : RDPDiskDrives.None;
            RedirectDiskDrivesCustom = Settings.Default.ConDefaultRedirectDiskDrivesCustom;
            RedirectPrinters = Settings.Default.ConDefaultRedirectPrinters;
            RedirectClipboard = Settings.Default.ConDefaultRedirectClipboard;
            RedirectPorts = Settings.Default.ConDefaultRedirectPorts;
            RedirectSmartCards = Settings.Default.ConDefaultRedirectSmartCards;
            RedirectAudioCapture = Settings.Default.ConDefaultRedirectAudioCapture;
            RedirectSound = Enum.Parse<RDPSounds>(Settings.Default.ConDefaultRedirectSound);
            SoundQuality = Enum.Parse<RDPSoundQuality>(Settings.Default.ConDefaultSoundQuality);
        }

        private void SetMiscDefaults()
        {
            PreExtApp = Settings.Default.ConDefaultPreExtApp;
            PostExtApp = Settings.Default.ConDefaultPostExtApp;
            MacAddress = Settings.Default.ConDefaultMacAddress;
            UserField = Settings.Default.ConDefaultUserField;
            UserField1 = string.Empty;
            UserField2 = string.Empty;
            UserField3 = string.Empty;
            UserField4 = string.Empty;
            UserField5 = string.Empty;
            UserField6 = string.Empty;
            UserField7 = string.Empty;
            UserField8 = string.Empty;
            UserField9 = string.Empty;
            UserField10 = string.Empty;
            Notes = string.Empty;
            EnvironmentTags = Settings.Default.ConDefaultEnvironmentTags;
            Favorite = Settings.Default.ConDefaultFavorite;
            RetryOnFirstConnect = Settings.Default.ConDefaultRetryOnFirstConnect;
            RDPStartProgram = Settings.Default.ConDefaultRDPStartProgram;
            RDPStartProgramWorkDir = Settings.Default.ConDefaultRDPStartProgramWorkDir;
            OpeningCommand = Settings.Default.OpeningCommand;
            User = "";
            Role = "";
        }

        private void SetVncDefaults()
        {
            VNCCompression = Enum.Parse<ProtocolVNC.Compression>(Settings.Default.ConDefaultVNCCompression);
            VNCEncoding = Enum.Parse<ProtocolVNC.Encoding>(Settings.Default.ConDefaultVNCEncoding);
            VNCAuthMode = Enum.Parse<ProtocolVNC.AuthMode>(Settings.Default.ConDefaultVNCAuthMode);
            VNCProxyType = Enum.Parse<ProtocolVNC.ProxyType>(Settings.Default.ConDefaultVNCProxyType);
            VNCProxyIP = Settings.Default.ConDefaultVNCProxyIP;
            VNCProxyPort = Settings.Default.ConDefaultVNCProxyPort;
            VNCProxyUsername = Settings.Default.ConDefaultVNCProxyUsername;
            VNCProxyPassword = Settings.Default.ConDefaultVNCProxyPassword;
            VNCColors = Enum.Parse<ProtocolVNC.Colors>(Settings.Default.ConDefaultVNCColors);
            VNCSmartSizeMode = Enum.Parse<ProtocolVNC.SmartSizeMode>(Settings.Default.ConDefaultVNCSmartSizeMode);
            VNCViewOnly = Settings.Default.ConDefaultVNCViewOnly;
        }

        private void SetNonBrowsablePropertiesDefaults()
        {
            _inheritance = new ConnectionInfoInheritance(this);
            SetNewOpenConnectionList();
        }

        private void SetNewOpenConnectionList()
        {
            OpenConnections = [];
            OpenConnections.CollectionChanged += (sender, args) => RaisePropertyChangedEvent(this, new PropertyChangedEventArgs(nameof(OpenConnections)));
        }

        public void NotifyDisconnectedStateChanged()
        {
            RaisePropertyChangedEvent(this, new PropertyChangedEventArgs(nameof(OpenConnections)));
        }

        #endregion
    }
}
