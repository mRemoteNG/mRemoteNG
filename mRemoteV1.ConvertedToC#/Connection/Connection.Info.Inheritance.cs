using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using mRemoteNG.Tools.LocalizedAttributes;

namespace mRemoteNG.Connection
{
	public partial class Info
	{
		public class Inheritance
		{
			#region "Public Properties"
			#region "General"
			[LocalizedCategory("strCategoryGeneral", 1), LocalizedDisplayNameInheritAttribute("strPropertyNameAll"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionAll"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool EverythingInherited {
				get {
					if (CacheBitmaps & Colors & Description & DisplayThemes & DisplayWallpaper & EnableFontSmoothing & EnableDesktopComposition & Domain & Icon & Password & Port & Protocol & PuttySession & RedirectDiskDrives & RedirectKeys & RedirectPorts & RedirectPrinters & RedirectSmartCards & RedirectSound & Resolution & AutomaticResize & UseConsoleSession & UseCredSsp & RenderingEngine & UserField & ExtApp & Username & Panel & ICAEncryption & RDPAuthenticationLevel & LoadBalanceInfo & PreExtApp & PostExtApp & MacAddress & VNCAuthMode & VNCColors & VNCCompression & VNCEncoding & VNCProxyIP & VNCProxyPassword & VNCProxyPort & VNCProxyType & VNCProxyUsername) {
						return true;
					} else {
						return false;
					}
				}
				set { SetAllValues(value); }
			}
			#endregion
			#region "Display"
			[LocalizedCategory("strCategoryDisplay", 2), LocalizedDisplayNameInheritAttribute("strPropertyNameDescription"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionDescription"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool Description { get; set; }

			[LocalizedCategory("strCategoryDisplay", 2), LocalizedDisplayNameInheritAttribute("strPropertyNameIcon"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionIcon"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool Icon { get; set; }

			[LocalizedCategory("strCategoryDisplay", 2), LocalizedDisplayNameInheritAttribute("strPropertyNamePanel"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionPanel"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool Panel { get; set; }
			#endregion
			#region "Connection"
			[LocalizedCategory("strCategoryConnection", 3), LocalizedDisplayNameInheritAttribute("strPropertyNameUsername"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionUsername"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool Username { get; set; }

			[LocalizedCategory("strCategoryConnection", 3), LocalizedDisplayNameInheritAttribute("strPropertyNamePassword"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionPassword"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool Password { get; set; }

			[LocalizedCategory("strCategoryConnection", 3), LocalizedDisplayNameInheritAttribute("strPropertyNameDomain"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionDomain"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool Domain { get; set; }
			#endregion
			#region "Protocol"
			[LocalizedCategory("strCategoryProtocol", 4), LocalizedDisplayNameInheritAttribute("strPropertyNameProtocol"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionProtocol"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool Protocol { get; set; }

			[LocalizedCategory("strCategoryProtocol", 4), LocalizedDisplayNameInheritAttribute("strPropertyNameExternalTool"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionExternalTool"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool ExtApp { get; set; }

			[LocalizedCategory("strCategoryProtocol", 4), LocalizedDisplayNameInheritAttribute("strPropertyNamePort"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionPort"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool Port { get; set; }

			[LocalizedCategory("strCategoryProtocol", 4), LocalizedDisplayNameInheritAttribute("strPropertyNamePuttySession"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionPuttySession"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool PuttySession { get; set; }

			[LocalizedCategory("strCategoryProtocol", 4), LocalizedDisplayNameInheritAttribute("strPropertyNameEncryptionStrength"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionEncryptionStrength"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool ICAEncryption { get; set; }

			[LocalizedCategory("strCategoryProtocol", 4), LocalizedDisplayNameInheritAttribute("strPropertyNameAuthenticationLevel"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionAuthenticationLevel"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool RDPAuthenticationLevel { get; set; }

			[LocalizedCategory("strCategoryProtocol", 4), LocalizedDisplayNameInheritAttribute("strPropertyNameLoadBalanceInfo"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionLoadBalanceInfo"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool LoadBalanceInfo { get; set; }

			[LocalizedCategory("strCategoryProtocol", 4), LocalizedDisplayNameInheritAttribute("strPropertyNameRenderingEngine"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionRenderingEngine"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool RenderingEngine { get; set; }

			[LocalizedCategory("strCategoryProtocol", 4), LocalizedDisplayNameInheritAttribute("strPropertyNameUseConsoleSession"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionUseConsoleSession"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool UseConsoleSession { get; set; }

			[LocalizedCategory("strCategoryProtocol", 4), LocalizedDisplayNameInheritAttribute("strPropertyNameUseCredSsp"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionUseCredSsp"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool UseCredSsp { get; set; }
			#endregion
			#region "RD Gateway"
			[LocalizedCategory("strCategoryGateway", 5), LocalizedDisplayNameInheritAttribute("strPropertyNameRDGatewayUsageMethod"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionRDGatewayUsageMethod"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool RDGatewayUsageMethod { get; set; }

			[LocalizedCategory("strCategoryGateway", 5), LocalizedDisplayNameInheritAttribute("strPropertyNameRDGatewayHostname"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionRDGatewayHostname"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool RDGatewayHostname { get; set; }

			[LocalizedCategory("strCategoryGateway", 5), LocalizedDisplayNameInheritAttribute("strPropertyNameRDGatewayUseConnectionCredentials"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionRDGatewayUseConnectionCredentials"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool RDGatewayUseConnectionCredentials { get; set; }

			[LocalizedCategory("strCategoryGateway", 5), LocalizedDisplayNameInheritAttribute("strPropertyNameRDGatewayUsername"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionRDGatewayUsername"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool RDGatewayUsername { get; set; }

			[LocalizedCategory("strCategoryGateway", 5), LocalizedDisplayNameInheritAttribute("strPropertyNameRDGatewayPassword"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionRDGatewayPassword"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool RDGatewayPassword { get; set; }

			[LocalizedCategory("strCategoryGateway", 5), LocalizedDisplayNameInheritAttribute("strPropertyNameRDGatewayDomain"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionRDGatewayDomain"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool RDGatewayDomain { get; set; }
			#endregion
			#region "Appearance"
			[LocalizedCategory("strCategoryAppearance", 6), LocalizedDisplayNameInheritAttribute("strPropertyNameResolution"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionResolution"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool Resolution { get; set; }

			[LocalizedCategory("strCategoryAppearance", 6), LocalizedDisplayNameInheritAttribute("strPropertyNameAutomaticResize"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionAutomaticResize"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool AutomaticResize { get; set; }

			[LocalizedCategory("strCategoryAppearance", 6), LocalizedDisplayNameInheritAttribute("strPropertyNameColors"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionColors"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool Colors { get; set; }

			[LocalizedCategory("strCategoryAppearance", 6), LocalizedDisplayNameInheritAttribute("strPropertyNameCacheBitmaps"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionCacheBitmaps"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool CacheBitmaps { get; set; }

			[LocalizedCategory("strCategoryAppearance", 6), LocalizedDisplayNameInheritAttribute("strPropertyNameDisplayWallpaper"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionDisplayWallpaper"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool DisplayWallpaper { get; set; }

			[LocalizedCategory("strCategoryAppearance", 6), LocalizedDisplayNameInheritAttribute("strPropertyNameDisplayThemes"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionDisplayThemes"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool DisplayThemes { get; set; }

			[LocalizedCategory("strCategoryAppearance", 6), LocalizedDisplayNameInheritAttribute("strPropertyNameEnableFontSmoothing"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionEnableFontSmoothing"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool EnableFontSmoothing { get; set; }

			[LocalizedCategory("strCategoryAppearance", 6), LocalizedDisplayNameInheritAttribute("strPropertyNameEnableDesktopComposition"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionEnableEnableDesktopComposition"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool EnableDesktopComposition { get; set; }
			#endregion
			#region "Redirect"
			[LocalizedCategory("strCategoryRedirect", 7), LocalizedDisplayNameInheritAttribute("strPropertyNameRedirectKeys"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionRedirectKeys"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool RedirectKeys { get; set; }

			[LocalizedCategory("strCategoryRedirect", 7), LocalizedDisplayNameInheritAttribute("strPropertyNameRedirectDrives"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionRedirectDrives"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool RedirectDiskDrives { get; set; }

			[LocalizedCategory("strCategoryRedirect", 7), LocalizedDisplayNameInheritAttribute("strPropertyNameRedirectPrinters"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionRedirectPrinters"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool RedirectPrinters { get; set; }

			[LocalizedCategory("strCategoryRedirect", 7), LocalizedDisplayNameInheritAttribute("strPropertyNameRedirectPorts"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionRedirectPorts"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool RedirectPorts { get; set; }

			[LocalizedCategory("strCategoryRedirect", 7), LocalizedDisplayNameInheritAttribute("strPropertyNameRedirectSmartCards"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionRedirectSmartCards"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool RedirectSmartCards { get; set; }

			[LocalizedCategory("strCategoryRedirect", 7), LocalizedDisplayNameInheritAttribute("strPropertyNameRedirectSounds"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionRedirectSounds"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool RedirectSound { get; set; }
			#endregion
			#region "Misc"
			[LocalizedCategory("strCategoryMiscellaneous", 8), LocalizedDisplayNameInheritAttribute("strPropertyNameExternalToolBefore"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionExternalToolBefore"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool PreExtApp { get; set; }

			[LocalizedCategory("strCategoryMiscellaneous", 8), LocalizedDisplayNameInheritAttribute("strPropertyNameExternalToolAfter"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionExternalToolAfter"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool PostExtApp { get; set; }

			[LocalizedCategory("strCategoryMiscellaneous", 8), LocalizedDisplayNameInheritAttribute("strPropertyNameMACAddress"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionMACAddress"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool MacAddress { get; set; }

			[LocalizedCategory("strCategoryMiscellaneous", 8), LocalizedDisplayNameInheritAttribute("strPropertyNameUser1"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionUser1"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool UserField { get; set; }
			#endregion
			#region "VNC"
			[LocalizedCategory("strCategoryAppearance", 9), LocalizedDisplayNameInheritAttribute("strPropertyNameCompression"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionCompression"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool VNCCompression { get; set; }

			[LocalizedCategory("strCategoryAppearance", 9), LocalizedDisplayNameInheritAttribute("strPropertyNameEncoding"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionEncoding"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool VNCEncoding { get; set; }

			[LocalizedCategory("strCategoryConnection", 9), LocalizedDisplayNameInheritAttribute("strPropertyNameAuthenticationMode"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionAuthenticationMode"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool VNCAuthMode { get; set; }

			[LocalizedCategory("strCategoryMiscellaneous", 9), LocalizedDisplayNameInheritAttribute("strPropertyNameVNCProxyType"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionVNCProxyType"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool VNCProxyType { get; set; }

			[LocalizedCategory("strCategoryMiscellaneous", 9), LocalizedDisplayNameInheritAttribute("strPropertyNameVNCProxyAddress"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionVNCProxyAddress"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool VNCProxyIP { get; set; }

			[LocalizedCategory("strCategoryMiscellaneous", 9), LocalizedDisplayNameInheritAttribute("strPropertyNameVNCProxyPort"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionVNCProxyPort"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool VNCProxyPort { get; set; }

			[LocalizedCategory("strCategoryMiscellaneous", 9), LocalizedDisplayNameInheritAttribute("strPropertyNameVNCProxyUsername"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionVNCProxyUsername"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool VNCProxyUsername { get; set; }

			[LocalizedCategory("strCategoryMiscellaneous", 9), LocalizedDisplayNameInheritAttribute("strPropertyNameVNCProxyPassword"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionVNCProxyPassword"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool VNCProxyPassword { get; set; }

			[LocalizedCategory("strCategoryAppearance", 9), LocalizedDisplayNameInheritAttribute("strPropertyNameColors"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionColors"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool VNCColors { get; set; }

			[LocalizedCategory("strCategoryAppearance", 9), LocalizedDisplayNameInheritAttribute("strPropertyNameSmartSizeMode"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionSmartSizeMode"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool VNCSmartSizeMode { get; set; }

			[LocalizedCategory("strCategoryAppearance", 9), LocalizedDisplayNameInheritAttribute("strPropertyNameViewOnly"), LocalizedDescriptionInheritAttribute("strPropertyDescriptionViewOnly"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
			public bool VNCViewOnly { get; set; }
			#endregion

			[Browsable(false)]
			public object Parent { get; set; }

			[Browsable(false)]
			public bool IsDefault { get; set; }
			#endregion

			#region "Constructors"
			public Inheritance(object parent, bool inheritEverything = false)
			{
				this.Parent = parent;
				if (inheritEverything)
					TurnOnInheritanceCompletely();
			}
			#endregion

			#region "Public Methods"
			public Inheritance Copy()
			{
				return MemberwiseClone();
			}

			public void TurnOnInheritanceCompletely()
			{
				SetAllValues(true);
			}

			public void TurnOffInheritanceCompletely()
			{
				SetAllValues(false);
			}
			#endregion

			#region "Private Methods"
			private void SetAllValues(bool value)
			{
				// Display
				Description = value;
				Icon = value;
				Panel = value;

				// Connection
				Username = value;
				Password = value;
				Domain = value;

				// Protocol
				Protocol = value;
				ExtApp = value;
				Port = value;
				PuttySession = value;
				ICAEncryption = value;
				RDPAuthenticationLevel = value;
				LoadBalanceInfo = value;
				RenderingEngine = value;
				UseConsoleSession = value;
				UseCredSsp = value;

				// RD Gateway
				RDGatewayUsageMethod = value;
				RDGatewayHostname = value;
				RDGatewayUseConnectionCredentials = value;
				RDGatewayUsername = value;
				RDGatewayPassword = value;
				RDGatewayDomain = value;

				// Appearance
				Resolution = value;
				AutomaticResize = value;
				Colors = value;
				CacheBitmaps = value;
				DisplayWallpaper = value;
				DisplayThemes = value;
				EnableFontSmoothing = value;
				EnableDesktopComposition = value;

				// Redirect
				RedirectKeys = value;
				RedirectDiskDrives = value;
				RedirectPrinters = value;
				RedirectPorts = value;
				RedirectSmartCards = value;
				RedirectSound = value;

				// Misc
				PreExtApp = value;
				PostExtApp = value;
				MacAddress = value;
				UserField = value;

				// VNC
				VNCCompression = value;
				VNCEncoding = value;
				VNCAuthMode = value;
				VNCProxyType = value;
				VNCProxyIP = value;
				VNCProxyPort = value;
				VNCProxyUsername = value;
				VNCProxyPassword = value;
				VNCColors = value;
				VNCSmartSizeMode = value;
				VNCViewOnly = value;
			}
			#endregion
		}
	}
}
