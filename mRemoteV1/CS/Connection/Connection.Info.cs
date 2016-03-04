using System.Collections.Generic;
using System;
using AxWFICALib;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using AxMSTSCLib;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using mRemoteNG.Config;
using mRemoteNG.Tools;
using System.Reflection;
using mRemoteNG.App;


namespace mRemoteNG.Connection
{
	[DefaultProperty("Name")]public partial class Info
	{
#region Public Properties
#region Display
        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameName"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionName")]
        public virtual string Name { get; set; }
			
		private string _description; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameDescription"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDescription")]
        public virtual string Description
		{
			get
			{
				return GetInheritedPropertyValue("Description", _description);
			}
			set
			{
				_description = value;
			}
		}
			
		private string _icon; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1), 
            TypeConverter(typeof(Icon)),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameIcon"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionIcon")]
        public virtual string Icon
		{
			get
			{
				return GetInheritedPropertyValue("Icon", _icon);
			}
			set
			{
				_icon = value;
			}
		}
			
		private string _panel; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNamePanel"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionPanel")]
        public virtual string Panel
		{
			get
			{
				return GetInheritedPropertyValue("Panel", _panel);
			}
			set
			{
				_panel = value;
			}
		}
#endregion
#region Connection
		private string _hostname = string.Empty;
        [LocalizedAttributes.LocalizedCategory("strCategoryConnection", 2),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameAddress"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionAddress")]
        public virtual string Hostname
		{
			get
			{
				return _hostname.Trim();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					_hostname = string.Empty;
				}
				else
				{
					_hostname = value.Trim();
				}
			}
		}
			
		private string _username; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryConnection", 2),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameUsername"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionUsername")]
        public virtual string Username
		{
			get
			{
				return GetInheritedPropertyValue("Username", _username);
			}
			set
			{
				_username = value.Trim();
			}
		}
			
		private string _password; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryConnection", 2),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNamePassword"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionPassword"), 
            PasswordPropertyText(true)]
        public virtual string Password
		{
			get
			{
				return GetInheritedPropertyValue("Password", _password);
			}
			set
			{
				_password = value;
			}
		}
			
		private string _domain; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryConnection", 2),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameDomain"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDomain")]
        public string Domain
		{
			get
			{
				return GetInheritedPropertyValue("Domain", _domain).Trim();
			}
			set
			{
				_domain = value.Trim();
			}
		}
#endregion
#region Protocol
		private Protocol.Protocols _protocol; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameProtocol"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionProtocol"), 
            TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
        public virtual Protocol.Protocols Protocol
		{
			get
			{
				return GetInheritedPropertyValue("Protocol", _protocol);
			}
			set
			{
				_protocol = value;
			}
		}
			
		private string _extApp; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameExternalTool"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionExternalTool"), 
            TypeConverter(typeof(Tools.ExternalToolsTypeConverter))]
        public string ExtApp
		{
			get
			{
				return GetInheritedPropertyValue("ExtApp", _extApp);
			}
			set
			{
				_extApp = value;
			}
		}
			
		private int _port;
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNamePort"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionPort")]
        public virtual int Port
		{
			get
			{
				return GetInheritedPropertyValue("Port", _port);
			}
			set
			{
				_port = value;
			}
		}
			
		private string _puttySession; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNamePuttySession"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionPuttySession"), 
            TypeConverter(typeof(Putty.Sessions.SessionList))]
        public virtual string PuttySession
		{
			get
			{
				return GetInheritedPropertyValue("PuttySession", _puttySession);
			}
			set
			{
				_puttySession = value;
			}
		}
			
		private Protocol.ICA.EncryptionStrength _icaEncryption; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameEncryptionStrength"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionEncryptionStrength"), 
            TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
        public Protocol.ICA.EncryptionStrength ICAEncryption
		{
			get
			{
				return GetInheritedPropertyValue("ICAEncryption", _icaEncryption);
			}
			set
			{
				_icaEncryption = value;
			}
		}
			
		private bool _useConsoleSession; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameUseConsoleSession"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionUseConsoleSession"), 
            TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
        public bool UseConsoleSession
		{
			get
			{
				return GetInheritedPropertyValue("UseConsoleSession", _useConsoleSession);
			}
			set
			{
				_useConsoleSession = value;
			}
		}
			
		private Protocol.RDP.AuthenticationLevel _rdpAuthenticationLevel; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameAuthenticationLevel"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionAuthenticationLevel"), 
            TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
        public Protocol.RDP.AuthenticationLevel RDPAuthenticationLevel
		{
			get
			{
				return GetInheritedPropertyValue("RDPAuthenticationLevel", _rdpAuthenticationLevel);
			}
			set
			{
				_rdpAuthenticationLevel = value;
			}
		}
			
		private string _loadBalanceInfo; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameLoadBalanceInfo"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionLoadBalanceInfo")]
        public string LoadBalanceInfo
		{
			get
			{
				return GetInheritedPropertyValue("LoadBalanceInfo", _loadBalanceInfo).Trim();
			}
			set
			{
				_loadBalanceInfo = value.Trim();
			}
		}
			
		private Protocol.HTTPBase.RenderingEngine _renderingEngine; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRenderingEngine"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRenderingEngine"), 
            TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
        public Protocol.HTTPBase.RenderingEngine RenderingEngine
		{
			get
			{
				return GetInheritedPropertyValue("RenderingEngine", _renderingEngine);
			}
			set
			{
				_renderingEngine = value;
			}
		}
			
		private bool _useCredSsp; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameUseCredSsp"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionUseCredSsp"), 
            TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
        public bool UseCredSsp
		{
			get
			{
				return GetInheritedPropertyValue("UseCredSsp", _useCredSsp);
			}
			set
			{
				_useCredSsp = value;
			}
		}
#endregion
#region RD Gateway
		private Protocol.RDP.RDGatewayUsageMethod _rdGatewayUsageMethod; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayUsageMethod"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDGatewayUsageMethod"), 
            TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
        public Protocol.RDP.RDGatewayUsageMethod RDGatewayUsageMethod
		{
			get
			{
				return GetInheritedPropertyValue("RDGatewayUsageMethod", _rdGatewayUsageMethod);
			}
			set
			{
				_rdGatewayUsageMethod = value;
			}
		}
			
		private string _rdGatewayHostname; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayHostname"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDGatewayHostname")]
        public string RDGatewayHostname
		{
			get
			{
				return GetInheritedPropertyValue("RDGatewayHostname", _rdGatewayHostname).Trim();
			}
			set
			{
				_rdGatewayHostname = value.Trim();
			}
		}
			
		private Protocol.RDP.RDGatewayUseConnectionCredentials _rdGatewayUseConnectionCredentials; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayUseConnectionCredentials"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDGatewayUseConnectionCredentials"), 
            TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
        public Protocol.RDP.RDGatewayUseConnectionCredentials RDGatewayUseConnectionCredentials
		{
			get
			{
				return GetInheritedPropertyValue("RDGatewayUseConnectionCredentials", _rdGatewayUseConnectionCredentials);
			}
			set
			{
				_rdGatewayUseConnectionCredentials = value;
			}
		}
			
		private string _rdGatewayUsername; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayUsername"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDGatewayUsername")]
        public string RDGatewayUsername
		{
			get
			{
				return GetInheritedPropertyValue("RDGatewayUsername", _rdGatewayUsername).Trim();
			}
			set
			{
				_rdGatewayUsername = value.Trim();
			}
		}
			
		private string _rdGatewayPassword; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayPassword"),
            LocalizedAttributes.LocalizedDescription("strPropertyNameRDGatewayPassword"), 
            PasswordPropertyText(true)]
        public string RDGatewayPassword
		{
			get
			{
				return GetInheritedPropertyValue("RDGatewayPassword", _rdGatewayPassword);
			}
			set
			{
				_rdGatewayPassword = value;
			}
		}
			
		private string _rdGatewayDomain; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayDomain"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDGatewayDomain")]
        public string RDGatewayDomain
		{
			get
			{
				return GetInheritedPropertyValue("RDGatewayDomain", _rdGatewayDomain).Trim();
			}
			set
			{
				_rdGatewayDomain = value.Trim();
			}
		}
#endregion
#region Appearance
		private Protocol.RDP.RDPResolutions _resolution; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameResolution"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionResolution"), 
            TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
        public Protocol.RDP.RDPResolutions Resolution
		{
			get
			{
				return GetInheritedPropertyValue("Resolution", _resolution);
			}
			set
			{
				_resolution = value;
			}
		}
			
		private bool _automaticResize; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameAutomaticResize"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionAutomaticResize"), 
            TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
        public bool AutomaticResize
		{
			get
			{
				return GetInheritedPropertyValue("AutomaticResize", _automaticResize);
			}
			set
			{
				_automaticResize = value;
			}
		}
			
		private Protocol.RDP.RDPColors _colors; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameColors"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionColors"), 
            TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
        public Protocol.RDP.RDPColors Colors
		{
			get
			{
				return GetInheritedPropertyValue("Colors", _colors);
			}
			set
			{
				_colors = value;
			}
		}
			
		private bool _cacheBitmaps; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameCacheBitmaps"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionCacheBitmaps"), 
            TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
        public bool CacheBitmaps
		{
			get
			{
				return GetInheritedPropertyValue("CacheBitmaps", _cacheBitmaps);
			}
			set
			{
				_cacheBitmaps = value;
			}
		}
			
		private bool _displayWallpaper; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameDisplayWallpaper"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDisplayWallpaper"), 
            TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
        public bool DisplayWallpaper
		{
			get
			{
				return GetInheritedPropertyValue("DisplayWallpaper", _displayWallpaper);
			}
			set
			{
				_displayWallpaper = value;
			}
		}
			
		private bool _displayThemes; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameDisplayThemes"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDisplayThemes"), 
            TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
        public bool DisplayThemes
		{
			get
			{
				return GetInheritedPropertyValue("DisplayThemes", _displayThemes);
			}
			set
			{
				_displayThemes = value;
			}
		}
			
		private bool _enableFontSmoothing; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameEnableFontSmoothing"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionEnableFontSmoothing"), 
            TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
        public bool EnableFontSmoothing
		{
			get
			{
				return GetInheritedPropertyValue("EnableFontSmoothing", _enableFontSmoothing);
			}
			set
			{
				_enableFontSmoothing = value;
			}
		}
			
		private bool _enableDesktopComposition; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameEnableDesktopComposition"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionEnableDesktopComposition"), 
            TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
        public bool EnableDesktopComposition
		{
			get
			{
				return GetInheritedPropertyValue("EnableDesktopComposition", _enableDesktopComposition);
			}
			set
			{
				_enableDesktopComposition = value;
			}
		}
#endregion
#region Redirect
		private bool _redirectKeys; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectKeys"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectKeys"), 
            TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
        public bool RedirectKeys
		{
			get
			{
				return GetInheritedPropertyValue("RedirectKeys", _redirectKeys);
			}
			set
			{
				_redirectKeys = value;
			}
		}
			
		private bool _redirectDiskDrives; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectDrives"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectDrives"), 
            TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
        public bool RedirectDiskDrives
		{
			get
			{
				return GetInheritedPropertyValue("RedirectDiskDrives", _redirectDiskDrives);
			}
			set
			{
				_redirectDiskDrives = value;
			}
		}
			
		private bool _redirectPrinters; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectPrinters"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectPrinters"), 
            TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
        public bool RedirectPrinters
		{
			get
			{
				return GetInheritedPropertyValue("RedirectPrinters", _redirectPrinters);
			}
			set
			{
				_redirectPrinters = value;
			}
		}
			
		private bool _redirectPorts; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectPorts"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectPorts"), 
            TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
        public bool RedirectPorts
		{
			get
			{
				return GetInheritedPropertyValue("RedirectPorts", _redirectPorts);
			}
			set
			{
				_redirectPorts = value;
			}
		}
			
		private bool _redirectSmartCards; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectSmartCards"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectSmartCards"), 
            TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
        public bool RedirectSmartCards
		{
			get
			{
				return GetInheritedPropertyValue("RedirectSmartCards", _redirectSmartCards);
			}
			set
			{
				_redirectSmartCards = value;
			}
		}
			
		private Protocol.RDP.RDPSounds _redirectSound; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectSounds"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectSounds"), 
            TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
        public Protocol.RDP.RDPSounds RedirectSound
		{
			get
			{
				return GetInheritedPropertyValue("RedirectSound", _redirectSound);
			}
			set
			{
				_redirectSound = value;
			}
		}
#endregion
#region Misc
		private string _preExtApp; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameExternalToolBefore"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionExternalToolBefore"), 
            TypeConverter(typeof(Tools.ExternalToolsTypeConverter))]
        public virtual string PreExtApp
		{
			get
			{
				return GetInheritedPropertyValue("PreExtApp", _preExtApp);
			}
			set
			{
				_preExtApp = value;
			}
		}
			
		private string _postExtApp; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameExternalToolAfter"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionExternalToolAfter"), 
            TypeConverter(typeof(Tools.ExternalToolsTypeConverter))]
        public virtual string PostExtApp
		{
			get
			{
				return GetInheritedPropertyValue("PostExtApp", _postExtApp);
			}
			set
			{
				_postExtApp = value;
			}
		}
			
		private string _macAddress; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameMACAddress"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionMACAddress")]
        public virtual string MacAddress
		{
			get
			{
				return GetInheritedPropertyValue("MacAddress", _macAddress);
			}
			set
			{
				_macAddress = value;
			}
		}
			
		private string _userField; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameUser1"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionUser1")]
        public virtual string UserField
		{
			get
			{
				return GetInheritedPropertyValue("UserField", _userField);
			}
			set
			{
				_userField = value;
			}
		}
#endregion
#region VNC
		private Protocol.VNC.Compression _vncCompression; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameCompression"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionCompression"), 
            TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
        public Protocol.VNC.Compression VNCCompression
		{
			get
			{
				return GetInheritedPropertyValue("VNCCompression", _vncCompression);
			}
			set
			{
				_vncCompression = value;
			}
		}
			
		private Protocol.VNC.Encoding _vncEncoding; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameEncoding"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionEncoding"), 
            TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
        public Protocol.VNC.Encoding VNCEncoding
		{
			get
			{
				return GetInheritedPropertyValue("VNCEncoding", _vncEncoding);
			}
			set
			{
				_vncEncoding = value;
			}
		}
			
			
		private Protocol.VNC.AuthMode _vncAuthMode; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryConnection", 2), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameAuthenticationMode"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionAuthenticationMode"), 
            TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
        public Protocol.VNC.AuthMode VNCAuthMode
		{
			get
			{
				return GetInheritedPropertyValue("VNCAuthMode", _vncAuthMode);
			}
			set
			{
				_vncAuthMode = value;
			}
		}
			
		private Protocol.VNC.ProxyType _vncProxyType; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyType"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyType"), 
            TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
        public Protocol.VNC.ProxyType VNCProxyType
		{
			get
			{
				return GetInheritedPropertyValue("VNCProxyType", _vncProxyType);
			}
			set
			{
				_vncProxyType = value;
			}
		}
			
		private string _vncProxyIP; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyAddress"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyAddress")]
        public string VNCProxyIP
		{
			get
			{
				return GetInheritedPropertyValue("VNCProxyIP", _vncProxyIP);
			}
			set
			{
				_vncProxyIP = value;
			}
		}
			
		private int _vncProxyPort; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyPort"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyPort")]
        public int VNCProxyPort
		{
			get
			{
				return GetInheritedPropertyValue("VNCProxyPort", _vncProxyPort);
			}
			set
			{
				_vncProxyPort = value;
			}
		}
			
		private string _vncProxyUsername; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyUsername"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyUsername")]
        public string VNCProxyUsername
		{
			get
			{
				return GetInheritedPropertyValue("VNCProxyUsername", _vncProxyUsername);
			}
			set
			{
				_vncProxyUsername = value;
			}
		}
			
		private string _vncProxyPassword; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyPassword"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyPassword"), 
            PasswordPropertyText(true)]
        public string VNCProxyPassword
		{
			get
			{
				return GetInheritedPropertyValue("VNCProxyPassword", _vncProxyPassword);
			}
			set
			{
				_vncProxyPassword = value;
			}
		}
			
		private Protocol.VNC.Colors _vncColors; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5), 
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameColors"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionColors"), 
            TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
        public Protocol.VNC.Colors VNCColors
		{
			get
			{
				return GetInheritedPropertyValue("VNCColors", _vncColors);
			}
			set
			{
				_vncColors = value;
			}
		}
			
		private Protocol.VNC.SmartSizeMode _vncSmartSizeMode; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameSmartSizeMode"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionSmartSizeMode"), 
            TypeConverter(typeof(Tools.Misc.EnumTypeConverter))]
        public Protocol.VNC.SmartSizeMode VNCSmartSizeMode
		{
			get
			{
				return GetInheritedPropertyValue("VNCSmartSizeMode", _vncSmartSizeMode);
			}
			set
			{
				_vncSmartSizeMode = value;
			}
		}
			
		private bool _vncViewOnly; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameViewOnly"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionViewOnly"), 
            TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
        public bool VNCViewOnly
		{
			get
			{
				return GetInheritedPropertyValue("VNCViewOnly", _vncViewOnly);
			}
			set
			{
				_vncViewOnly = value;
			}
		}
#endregion
			
		[Browsable(false)]private Inheritance _Inherit =
@privateprivate ;Inheritance Inherit
		{
			get
			{
				return _Inherit;
			}
			set
			{
				_Inherit = value;
			}
		}
			
		[Browsable(false)]private Protocol.List _OpenConnections =
@privateprivate ;Protocol.List OpenConnections
		{
			get
			{
				return _OpenConnections;
			}
			set
			{
				_OpenConnections = value;
			}
		}
			
		[Browsable(false)]private bool _IsContainer = false;
        private bool IsContainer
		{
			get
			{
				return _IsContainer;
			}
			set
			{
				_IsContainer = value;
			}
		}
			
		[Browsable(false)]private bool _IsDefault = false;
        private bool IsDefault
		{
			get
			{
				return _IsDefault;
			}
			set
			{
				_IsDefault = value;
			}
		}
			
		[Browsable(false)]public Container.Info Parent {get; set;}
			
		[Browsable(false)]private int _PositionID = 0;
        private int PositionID
		{
			get
			{
				return _PositionID;
			}
			set
			{
				_PositionID = value;
			}
		}
			
		[Browsable(false)]public string ConstantID {get; set;}
			
		[Browsable(false)]public TreeNode TreeNode {get; set;}
			
		[Browsable(false)]private bool _IsQuickConnect = false;
        private bool IsQuickConnect
		{
			get
			{
				return _IsQuickConnect;
			}
			set
			{
				_IsQuickConnect = value;
			}
		}
			
		[Browsable(false)]private bool _PleaseConnect = false;
        private bool PleaseConnect
		{
			get
			{
				return _PleaseConnect;
			}
			set
			{
				_PleaseConnect = value;
			}
		}
#endregion
			
#region Constructors
		public Info()
		{
			// VBConversions Note: Non-static class variable initialization is below.  Class variables cannot be initially assigned non-static values in C#.
			ConstantID = Tools.Misc.CreateConstantID();
			SetDefaults();
		}
			
		public Info(Container.Info parent)
		{
			// VBConversions Note: Non-static class variable initialization is below.  Class variables cannot be initially assigned non-static values in C#.
			ConstantID = Tools.Misc.CreateConstantID();
			SetDefaults();
			IsContainer = true;
			this.Parent = parent;
		}
#endregion
			
#region Public Methods
		public Info Copy()
		{
			Info newConnectionInfo = (Info)MemberwiseClone();
			newConnectionInfo.ConstantID = Tools.Misc.CreateConstantID();
			newConnectionInfo._OpenConnections = new Protocol.List();
			return newConnectionInfo;
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
#endregion
			
#region Public Enumerations
		[Flags()]public enum Force
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
			Type inheritType = Inherit.GetType();
			PropertyInfo inheritPropertyInfo = inheritType.GetProperty(propertyName);
			bool inheritPropertyValue = System.Convert.ToBoolean(inheritPropertyInfo.GetValue(Inherit, BindingFlags.GetProperty, null, null, null));
				
			if (inheritPropertyValue && Parent != null)
			{
				Info parentConnectionInfo = default(Info);
				if (IsContainer)
				{
					parentConnectionInfo = Parent.Parent.ConnectionInfo;
				}
				else
				{
					parentConnectionInfo = Parent.ConnectionInfo;
				}
					
				Type connectionInfoType = parentConnectionInfo.GetType();
				PropertyInfo parentPropertyInfo = connectionInfoType.GetProperty(propertyName);
				TPropertyType parentPropertyValue = parentPropertyInfo.GetValue(parentConnectionInfo, BindingFlags.GetProperty, null, null, null);
					
				return parentPropertyValue;
			}
			else
			{
				return value;
			}
		}
			
		private static int GetDefaultPort(Protocol.Protocols protocol)
		{
			try
			{
				if (protocol == Connection.Protocol.Protocols.RDP)
				{
                    return (int)Connection.Protocol.RDP.Defaults.Port;
				}
                else if (protocol == Connection.Protocol.Protocols.VNC)
				{
                    return (int)Connection.Protocol.VNC.Defaults.Port;
				}
                else if (protocol == Connection.Protocol.Protocols.SSH1)
				{
                    return (int)Connection.Protocol.SSH1.Defaults.Port;
				}
                else if (protocol == Connection.Protocol.Protocols.SSH2)
				{
                    return (int)Connection.Protocol.SSH2.Defaults.Port;
				}
                else if (protocol == Connection.Protocol.Protocols.Telnet)
				{
                    return (int)Connection.Protocol.Telnet.Defaults.Port;
				}
                else if (protocol == Connection.Protocol.Protocols.Rlogin)
				{
                    return (int)Connection.Protocol.Rlogin.Defaults.Port;
				}
                else if (protocol == Connection.Protocol.Protocols.RAW)
				{
                    return (int)Connection.Protocol.RAW.Defaults.Port;
				}
                else if (protocol == Connection.Protocol.Protocols.HTTP)
				{
                    return (int)Connection.Protocol.HTTP.Defaults.Port;
				}
                else if (protocol == Connection.Protocol.Protocols.HTTPS)
				{
                    return (int)Connection.Protocol.HTTPS.Defaults.Port;
				}
                else if (protocol == Connection.Protocol.Protocols.ICA)
				{
                    return (int)Connection.Protocol.ICA.Defaults.Port;
				}
                else if (protocol == Connection.Protocol.Protocols.IntApp)
				{
                    return (int)Connection.Protocol.IntegratedProgram.Defaults.Port;
				}
			}
			catch (Exception ex)
			{
                Runtime.MessageCollector.AddExceptionMessage(My.Language.strConnectionSetDefaultPortFailed, ex, Messages.MessageClass.ErrorMsg);
			}
		}
#endregion
	}
}
