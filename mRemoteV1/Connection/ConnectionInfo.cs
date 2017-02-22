using System;
using System.Collections.Generic;
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
using mRemoteNG.Tree;


namespace mRemoteNG.Connection
{
	[DefaultProperty("Name")]
    public class ConnectionInfo : AbstractConnectionRecord, IHasParent, IInheritable
    {        
        #region Public Properties
        [Browsable(false)]
        public ConnectionInfoInheritance Inheritance { get; set; }

	    [Browsable(false)]
	    public ProtocolList OpenConnections { get; protected set; }

	    [Browsable(false)]
        public bool IsContainer { get; set; }

	    [Browsable(false)]
        public bool IsDefault { get; set; }

	    [Browsable(false)]
	    public ContainerInfo Parent { get; internal set; }

        //[Browsable(false)]
        //private int PositionID { get; set; }

        [Browsable(false)]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public bool IsQuickConnect { get; set; }

	    [Browsable(false)]
        public bool PleaseConnect { get; set; }
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
			parent.AddChild(this);
		}
        #endregion
			
        #region Public Methods
		public virtual ConnectionInfo Clone()
		{
		    var newConnectionInfo = new ConnectionInfo();
            newConnectionInfo.CopyFrom(this);
			newConnectionInfo.ConstantID = MiscTools.CreateConstantID();
            newConnectionInfo.SetParent(Parent);
		    newConnectionInfo.Inheritance = Inheritance.Clone();
			return newConnectionInfo;
		}

	    public void CopyFrom(ConnectionInfo sourceConnectionInfo)
	    {
	        var properties = GetType().BaseType?.GetProperties();
	        if (properties == null) return;
	        foreach (var property in properties)
	        {
	            var remotePropertyValue = property.GetValue(sourceConnectionInfo, null);
                property.SetValue(this, remotePropertyValue, null);
	        }
            var clonedInheritance = sourceConnectionInfo.Inheritance.Clone();
            clonedInheritance.Parent = this;
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

	    public virtual void SetParent(ContainerInfo parent)
	    {
            RemoveParent();
            parent?.AddChild(this);
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
			NoCredentials = 32
		}
        #endregion
			
        #region Private Methods
        protected override TPropertyType GetPropertyValue<TPropertyType>(string propertyName, TPropertyType value)
        {
            return ShouldThisPropertyBeInherited(propertyName) ? GetInheritedPropertyValue<TPropertyType>(propertyName) : value;
        }

	    private bool ShouldThisPropertyBeInherited(string propertyName)
        {
            return ParentIsValidInheritanceTarget() && IsInheritanceTurnedOnForThisProperty(propertyName);
        }

        private bool ParentIsValidInheritanceTarget()
        {
            return Parent != null;
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
                Runtime.MessageCollector.AddExceptionMessage(Language.strConnectionSetDefaultPortFailed, ex);
                return 0;
			}
		}

        private void SetTreeDisplayDefaults()
        {
            Name = Language.strNewConnection;
            Description = Settings.Default.ConDefaultDescription;
            Icon = Settings.Default.ConDefaultIcon;
            Panel = Language.strGeneral;
        }

        private void SetConnectionDefaults()
        {
            Hostname = string.Empty;
        }

        private void SetProtocolDefaults()
        {
            Protocol = (ProtocolType)Enum.Parse(typeof(ProtocolType), Settings.Default.ConDefaultProtocol);
            ExtApp = Settings.Default.ConDefaultExtApp;
            Port = 0;
            PuttySession = Settings.Default.ConDefaultPuttySession;
            ICAEncryptionStrength = (ProtocolICA.EncryptionStrength) Enum.Parse(typeof(ProtocolICA.EncryptionStrength), Settings.Default.ConDefaultICAEncryptionStrength);
            UseConsoleSession = Settings.Default.ConDefaultUseConsoleSession;
            RDPAuthenticationLevel = (ProtocolRDP.AuthenticationLevel) Enum.Parse(typeof(ProtocolRDP.AuthenticationLevel), Settings.Default.ConDefaultRDPAuthenticationLevel);
            RDPMinutesToIdleTimeout = Settings.Default.ConDefaultRDPMinutesToIdleTimeout;
            RDPAlertIdleTimeout = Settings.Default.ConDefaultRDPAlertIdleTimeout;
            LoadBalanceInfo = Settings.Default.ConDefaultLoadBalanceInfo;
            RenderingEngine = (HTTPBase.RenderingEngine) Enum.Parse(typeof(HTTPBase.RenderingEngine), Settings.Default.ConDefaultRenderingEngine);
            UseCredSsp = Settings.Default.ConDefaultUseCredSsp;
        }

        private void SetRdGatewayDefaults()
        {
            RDGatewayUsageMethod = (ProtocolRDP.RDGatewayUsageMethod) Enum.Parse(typeof(ProtocolRDP.RDGatewayUsageMethod), Settings.Default.ConDefaultRDGatewayUsageMethod);
            RDGatewayHostname = Settings.Default.ConDefaultRDGatewayHostname;
            RDGatewayUseConnectionCredentials = (ProtocolRDP.RDGatewayUseConnectionCredentials) Enum.Parse(typeof(ProtocolRDP.RDGatewayUseConnectionCredentials), Settings.Default.ConDefaultRDGatewayUseConnectionCredentials);
            RDGatewayUsername = Settings.Default.ConDefaultRDGatewayUsername;
            RDGatewayPassword = Settings.Default.ConDefaultRDGatewayPassword;
            RDGatewayDomain = Settings.Default.ConDefaultRDGatewayDomain;
        }

        private void SetAppearanceDefaults() 
        {
            Resolution = (ProtocolRDP.RDPResolutions) Enum.Parse(typeof(ProtocolRDP.RDPResolutions), Settings.Default.ConDefaultResolution);
            AutomaticResize = Settings.Default.ConDefaultAutomaticResize;
            Colors = (ProtocolRDP.RDPColors) Enum.Parse(typeof(ProtocolRDP.RDPColors), Settings.Default.ConDefaultColors);
            CacheBitmaps = Settings.Default.ConDefaultCacheBitmaps;
            DisplayWallpaper = Settings.Default.ConDefaultDisplayWallpaper;
            DisplayThemes = Settings.Default.ConDefaultDisplayThemes;
            EnableFontSmoothing = Settings.Default.ConDefaultEnableFontSmoothing;
            EnableDesktopComposition = Settings.Default.ConDefaultEnableDesktopComposition;
        }

        private void SetRedirectDefaults()
        {
            RedirectKeys = Settings.Default.ConDefaultRedirectKeys;
            RedirectDiskDrives = Settings.Default.ConDefaultRedirectDiskDrives;
            RedirectPrinters = Settings.Default.ConDefaultRedirectPrinters;
            RedirectPorts = Settings.Default.ConDefaultRedirectPorts;
            RedirectSmartCards = Settings.Default.ConDefaultRedirectSmartCards;
            RedirectSound = (ProtocolRDP.RDPSounds) Enum.Parse(typeof(ProtocolRDP.RDPSounds), Settings.Default.ConDefaultRedirectSound);
            SoundQuality = (ProtocolRDP.RDPSoundQuality)Enum.Parse(typeof(ProtocolRDP.RDPSoundQuality), Settings.Default.ConDefaultSoundQuality);
        }

        private void SetMiscDefaults()
        {
            ConstantID = MiscTools.CreateConstantID();
            PreExtApp = Settings.Default.ConDefaultPreExtApp;
            PostExtApp = Settings.Default.ConDefaultPostExtApp;
            MacAddress = Settings.Default.ConDefaultMacAddress;
            UserField = Settings.Default.ConDefaultUserField;
        }

        private void SetVncDefaults()
        {
            VNCCompression = (ProtocolVNC.Compression) Enum.Parse(typeof(ProtocolVNC.Compression), Settings.Default.ConDefaultVNCCompression);
            VNCEncoding = (ProtocolVNC.Encoding) Enum.Parse(typeof(ProtocolVNC.Encoding), Settings.Default.ConDefaultVNCEncoding);
            VNCAuthMode = (ProtocolVNC.AuthMode) Enum.Parse(typeof(ProtocolVNC.AuthMode), Settings.Default.ConDefaultVNCAuthMode);
            VNCProxyType = (ProtocolVNC.ProxyType) Enum.Parse(typeof(ProtocolVNC.ProxyType), Settings.Default.ConDefaultVNCProxyType);
            VNCProxyIP = Settings.Default.ConDefaultVNCProxyIP;
            VNCProxyPort = Settings.Default.ConDefaultVNCProxyPort;
            VNCProxyUsername = Settings.Default.ConDefaultVNCProxyUsername;
            VNCProxyPassword = Settings.Default.ConDefaultVNCProxyPassword;
            VNCColors = (ProtocolVNC.Colors) Enum.Parse(typeof(ProtocolVNC.Colors), Settings.Default.ConDefaultVNCColors);
            VNCSmartSizeMode = (ProtocolVNC.SmartSizeMode) Enum.Parse(typeof(ProtocolVNC.SmartSizeMode), Settings.Default.ConDefaultVNCSmartSizeMode);
            VNCViewOnly = Settings.Default.ConDefaultVNCViewOnly;
        }

        private void SetNonBrowsablePropertiesDefaults()
        {
            Inheritance = new ConnectionInfoInheritance(this);
            SetNewOpenConnectionList();
            //PositionID = 0;
        }

        private void SetNewOpenConnectionList()
	    {
	        OpenConnections = new ProtocolList();
	        OpenConnections.CollectionChanged += (sender, args) => RaisePropertyChangedEvent(this, new PropertyChangedEventArgs("OpenConnections"));
	    }
        #endregion
    }
}