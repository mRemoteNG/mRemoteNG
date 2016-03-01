using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using AxMSTSCLib;
using EOLWTSCOM;
using System.ComponentModel;
using mRemoteNG.Messages;
using mRemoteNG.App.Runtime;
using mRemoteNG.Tools.LocalizedAttributes;
using MSTSCLib;

namespace mRemoteNG.Connection
{
	namespace Protocol
	{
		public class RDP : Base
		{
			#region "Properties"
			public bool SmartSize {
				get { return _rdpClient.AdvancedSettings2.SmartSizing; }
				set {
					_rdpClient.AdvancedSettings2.SmartSizing = value;
					ReconnectForResize();
				}
			}

			public bool Fullscreen {
				get { return _rdpClient.FullScreen; }
				set {
					_rdpClient.FullScreen = value;
					ReconnectForResize();
				}
			}

			private bool _redirectKeys = false;
			public bool RedirectKeys {
				get { return _redirectKeys; }
				set {
					_redirectKeys = value;
					try {
						if (!_redirectKeys)
							return;

						Debug.Assert(_rdpClient.SecuredSettingsEnabled);
						MSTSCLib.IMsRdpClientSecuredSettings msRdpClientSecuredSettings = _rdpClient.SecuredSettings2;
						msRdpClientSecuredSettings.KeyboardHookMode = 1;
						// Apply key combinations at the remote server.
					} catch (Exception ex) {
						mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strRdpSetRedirectKeysFailed + Constants.vbNewLine + ex.Message, true);
					}
				}
			}
			#endregion

			#region "Private Declarations"
			private MsRdpClient5NotSafeForScripting _rdpClient;
			private Version _rdpVersion;
			private Info _connectionInfo;
				#endregion
			private bool _loginComplete;

			#region "Public Methods"
			public RDP()
			{
				Control = new AxMsRdpClient5NotSafeForScripting();
			}

			public override bool SetProps()
			{
				base.SetProps();

				try {
					Control.CreateControl();
					_connectionInfo = InterfaceControl.Info;

					try {
						while (!(Control.Created)) {
							Thread.Sleep(0);
							System.Windows.Forms.Application.DoEvents();
						}

						_rdpClient = ((AxMsRdpClient5NotSafeForScripting)Control).GetOcx();
					} catch (Runtime.InteropServices.COMException ex) {
						mRemoteNG.App.Runtime.MessageCollector.AddExceptionMessage(mRemoteNG.My.Language.strRdpControlCreationFailed, ex);
						Control.Dispose();
						return false;
					}

					_rdpVersion = new Version(_rdpClient.Version);

					_rdpClient.Server = this._connectionInfo.Hostname;

					this.SetCredentials();
					this.SetResolution();
					this._rdpClient.FullScreenTitle = this._connectionInfo.Name;

					//not user changeable
					_rdpClient.AdvancedSettings2.GrabFocusOnConnect = true;
					_rdpClient.AdvancedSettings3.EnableAutoReconnect = true;
					_rdpClient.AdvancedSettings3.MaxReconnectAttempts = mRemoteNG.My.Settings.RdpReconnectionCount;
					_rdpClient.AdvancedSettings2.keepAliveInterval = 60000;
					//in milliseconds (10.000 = 10 seconds)
					_rdpClient.AdvancedSettings5.AuthenticationLevel = 0;
					_rdpClient.AdvancedSettings2.EncryptionEnabled = 1;

					_rdpClient.AdvancedSettings2.overallConnectionTimeout = 20;

					_rdpClient.AdvancedSettings2.BitmapPeristence = this._connectionInfo.CacheBitmaps;
					if (_rdpVersion >= Versions.RDC61) {
						_rdpClient.AdvancedSettings7.EnableCredSspSupport = _connectionInfo.UseCredSsp;
					}

					this.SetUseConsoleSession();
					this.SetPort();
					RedirectKeys = _connectionInfo.RedirectKeys;
					this.SetRedirection();
					this.SetAuthenticationLevel();
					SetLoadBalanceInfo();
					this.SetRdGateway();

					_rdpClient.ColorDepth = Conversion.Int(this._connectionInfo.Colors);

					this.SetPerformanceFlags();

					_rdpClient.ConnectingText = mRemoteNG.My.Language.strConnecting;

					Control.Anchor = AnchorStyles.None;

					return true;
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strRdpSetPropsFailed + Constants.vbNewLine + ex.Message, true);
					return false;
				}
			}

			public override bool Connect()
			{
				_loginComplete = false;
				SetEventHandlers();

				try {
					_rdpClient.Connect();
					base.Connect();
					return true;
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strRdpConnectionOpenFailed + Constants.vbNewLine + ex.Message);
				}

				return false;
			}

			public override void Disconnect()
			{
				try {
					_rdpClient.Disconnect();
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strRdpDisconnectFailed + Constants.vbNewLine + ex.Message, true);
					base.Close();
				}
			}

			public void ToggleFullscreen()
			{
				try {
					this.Fullscreen = !this.Fullscreen;
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strRdpToggleFullscreenFailed + Constants.vbNewLine + ex.Message, true);
				}
			}

			public void ToggleSmartSize()
			{
				try {
					this.SmartSize = !this.SmartSize;
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strRdpToggleSmartSizeFailed + Constants.vbNewLine + ex.Message, true);
				}
			}

			public override void Focus()
			{
				try {
					if (Control.ContainsFocus == false) {
						Control.Focus();
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strRdpFocusFailed + Constants.vbNewLine + ex.Message, true);
				}
			}

			private Size _controlBeginningSize = new Size();
			public override void ResizeBegin(object sender, EventArgs e)
			{
				_controlBeginningSize = Control.Size;
			}

			public override void Resize(object sender, EventArgs e)
			{
				if (DoResize() & _controlBeginningSize.IsEmpty) {
					ReconnectForResize();
				}
				base.Resize(sender, e);
			}

			public override void ResizeEnd(object sender, EventArgs e)
			{
				DoResize();
				if (!(Control.Size == _controlBeginningSize)) {
					ReconnectForResize();
				}
				_controlBeginningSize = Size.Empty;
			}
			#endregion

			#region "Private Methods"
			private bool DoResize()
			{
				Control.Location = InterfaceControl.Location;
				if (!(Control.Size == InterfaceControl.Size) & !(InterfaceControl.Size == Size.Empty)) {
					Control.Size = InterfaceControl.Size;
					return true;
				} else {
					return false;
				}
			}

			private void ReconnectForResize()
			{
				if (_rdpVersion < Versions.RDC80)
					return;

				if (!_loginComplete)
					return;

				if (!InterfaceControl.Info.AutomaticResize)
					return;

				if (!(InterfaceControl.Info.Resolution == RDPResolutions.FitToWindow | InterfaceControl.Info.Resolution == RDPResolutions.Fullscreen))
					return;

				if (SmartSize)
					return;

				Size size = default(Size);
				if (!Fullscreen) {
					size = Control.Size;
				} else {
					size = Screen.FromControl(Control).Bounds.Size;
				}

				IMsRdpClient8 msRdpClient8 = _rdpClient;
				msRdpClient8.Reconnect(size.Width, size.Height);
			}

			private void SetRdGateway()
			{
				try {
					if (_rdpClient.TransportSettings.GatewayIsSupported == 0) {
						mRemoteNG.App.Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, mRemoteNG.My.Language.strRdpGatewayNotSupported, true);
						return;
					} else {
						mRemoteNG.App.Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, mRemoteNG.My.Language.strRdpGatewayIsSupported, true);
					}

					if (!(_connectionInfo.RDGatewayUsageMethod == RDGatewayUsageMethod.Never)) {
						_rdpClient.TransportSettings.GatewayUsageMethod = _connectionInfo.RDGatewayUsageMethod;
						_rdpClient.TransportSettings.GatewayHostname = _connectionInfo.RDGatewayHostname;
						_rdpClient.TransportSettings.GatewayProfileUsageMethod = 1;
						// TSC_PROXY_PROFILE_MODE_EXPLICIT
						if (_connectionInfo.RDGatewayUseConnectionCredentials == RDGatewayUseConnectionCredentials.SmartCard) {
							_rdpClient.TransportSettings.GatewayCredsSource = 1;
							// TSC_PROXY_CREDS_MODE_SMARTCARD
						}
						if (_rdpVersion >= Versions.RDC61 & !((Force & Info.Force.NoCredentials) == Info.Force.NoCredentials)) {
							if (_connectionInfo.RDGatewayUseConnectionCredentials == RDGatewayUseConnectionCredentials.Yes) {
								_rdpClient.TransportSettings2.GatewayUsername = _connectionInfo.Username;
								_rdpClient.TransportSettings2.GatewayPassword = _connectionInfo.Password;
								_rdpClient.TransportSettings2.GatewayDomain = _connectionInfo.Domain;
							} else if (_connectionInfo.RDGatewayUseConnectionCredentials == RDGatewayUseConnectionCredentials.SmartCard) {
								_rdpClient.TransportSettings2.GatewayCredSharing = 0;
							} else {
								_rdpClient.TransportSettings2.GatewayUsername = _connectionInfo.RDGatewayUsername;
								_rdpClient.TransportSettings2.GatewayPassword = _connectionInfo.RDGatewayPassword;
								_rdpClient.TransportSettings2.GatewayDomain = _connectionInfo.RDGatewayDomain;
								_rdpClient.TransportSettings2.GatewayCredSharing = 0;
							}
						}
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, mRemoteNG.My.Language.strRdpSetGatewayFailed + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void SetUseConsoleSession()
			{
				try {
					bool value = false;

					if ((Force & Info.Force.UseConsoleSession) == Info.Force.UseConsoleSession) {
						value = true;
					} else if ((Force & Info.Force.DontUseConsoleSession) == Info.Force.DontUseConsoleSession) {
						value = false;
					} else {
						value = _connectionInfo.UseConsoleSession;
					}

					if (_rdpVersion >= Versions.RDC61) {
						mRemoteNG.App.Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format(mRemoteNG.My.Language.strRdpSetConsoleSwitch, "6.1"), true);
						_rdpClient.AdvancedSettings7.ConnectToAdministerServer = value;
					} else {
						mRemoteNG.App.Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format(mRemoteNG.My.Language.strRdpSetConsoleSwitch, "6.0"), true);
						_rdpClient.AdvancedSettings2.ConnectToServerConsole = value;
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddExceptionMessage(mRemoteNG.My.Language.strRdpSetConsoleSessionFailed, ex, MessageClass.ErrorMsg, true);
				}
			}

			private void SetCredentials()
			{
				try {
					if ((Force & Info.Force.NoCredentials) == Info.Force.NoCredentials)
						return;

					string userName = _connectionInfo.Username;
					string password = _connectionInfo.Password;
					string domain = _connectionInfo.Domain;

					if (string.IsNullOrEmpty(userName)) {
						switch (mRemoteNG.My.Settings.EmptyCredentials) {
							case "windows":
								_rdpClient.UserName = Environment.UserName;
								break;
							case "custom":
								_rdpClient.UserName = mRemoteNG.My.Settings.DefaultUsername;
								break;
						}
					} else {
						_rdpClient.UserName = userName;
					}

					if (string.IsNullOrEmpty(password)) {
						switch (mRemoteNG.My.Settings.EmptyCredentials) {
							case "custom":
								if (!string.IsNullOrEmpty(mRemoteNG.My.Settings.DefaultPassword)) {
									_rdpClient.AdvancedSettings2.ClearTextPassword = mRemoteNG.Security.Crypt.Decrypt(mRemoteNG.My.Settings.DefaultPassword, mRemoteNG.App.Info.General.EncryptionKey);
								}
								break;
						}
					} else {
						_rdpClient.AdvancedSettings2.ClearTextPassword = password;
					}

					if (string.IsNullOrEmpty(domain)) {
						switch (mRemoteNG.My.Settings.EmptyCredentials) {
							case "windows":
								_rdpClient.Domain = Environment.UserDomainName;
								break;
							case "custom":
								_rdpClient.Domain = mRemoteNG.My.Settings.DefaultDomain;
								break;
						}
					} else {
						_rdpClient.Domain = domain;
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strRdpSetCredentialsFailed + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void SetResolution()
			{
				try {
					if ((this.Force & mRemoteNG.Connection.Info.Force.Fullscreen) == mRemoteNG.Connection.Info.Force.Fullscreen) {
						_rdpClient.FullScreen = true;
						_rdpClient.DesktopWidth = Screen.FromControl(frmMain).Bounds.Width;
						_rdpClient.DesktopHeight = Screen.FromControl(frmMain).Bounds.Height;

						return;
					}

					switch (this.InterfaceControl.Info.Resolution) {
						case RDPResolutions.FitToWindow:
						case RDPResolutions.SmartSize:
							_rdpClient.DesktopWidth = InterfaceControl.Size.Width;
							_rdpClient.DesktopHeight = InterfaceControl.Size.Height;
							break;
						case RDPResolutions.Fullscreen:
							_rdpClient.FullScreen = true;
							_rdpClient.DesktopWidth = Screen.FromControl(frmMain).Bounds.Width;
							_rdpClient.DesktopHeight = Screen.FromControl(frmMain).Bounds.Height;
							break;
						default:
							Rectangle resolution = GetResolutionRectangle(_connectionInfo.Resolution);
							_rdpClient.DesktopWidth = resolution.Width;
							_rdpClient.DesktopHeight = resolution.Height;
							break;
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strRdpSetResolutionFailed + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void SetPort()
			{
				try {
					if (_connectionInfo.Port != Defaults.Port) {
						_rdpClient.AdvancedSettings2.RDPPort = _connectionInfo.Port;
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strRdpSetPortFailed + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void SetRedirection()
			{
				try {
					_rdpClient.AdvancedSettings2.RedirectDrives = this._connectionInfo.RedirectDiskDrives;
					_rdpClient.AdvancedSettings2.RedirectPorts = this._connectionInfo.RedirectPorts;
					_rdpClient.AdvancedSettings2.RedirectPrinters = this._connectionInfo.RedirectPrinters;
					_rdpClient.AdvancedSettings2.RedirectSmartCards = this._connectionInfo.RedirectSmartCards;
					_rdpClient.SecuredSettings2.AudioRedirectionMode = Conversion.Int(this._connectionInfo.RedirectSound);
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strRdpSetRedirectionFailed + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void SetPerformanceFlags()
			{
				try {
					int pFlags = 0;
					if (this._connectionInfo.DisplayThemes == false) {
						pFlags += Conversion.Int(mRemoteNG.Connection.Protocol.RDP.RDPPerformanceFlags.DisableThemes);
					}

					if (this._connectionInfo.DisplayWallpaper == false) {
						pFlags += Conversion.Int(mRemoteNG.Connection.Protocol.RDP.RDPPerformanceFlags.DisableWallpaper);
					}

					if (this._connectionInfo.EnableFontSmoothing) {
						pFlags += Conversion.Int(mRemoteNG.Connection.Protocol.RDP.RDPPerformanceFlags.EnableFontSmoothing);
					}

					if (this._connectionInfo.EnableDesktopComposition) {
						pFlags += Conversion.Int(mRemoteNG.Connection.Protocol.RDP.RDPPerformanceFlags.EnableDesktopComposition);
					}

					_rdpClient.AdvancedSettings2.PerformanceFlags = pFlags;
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strRdpSetPerformanceFlagsFailed + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void SetAuthenticationLevel()
			{
				try {
					_rdpClient.AdvancedSettings5.AuthenticationLevel = this._connectionInfo.RDPAuthenticationLevel;
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strRdpSetAuthenticationLevelFailed + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void SetLoadBalanceInfo()
			{
				if (string.IsNullOrEmpty(_connectionInfo.LoadBalanceInfo))
					return;
				try {
					_rdpClient.AdvancedSettings2.LoadBalanceInfo = _connectionInfo.LoadBalanceInfo;
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddExceptionMessage("Unable to set load balance info.", ex);
				}
			}

			private void SetEventHandlers()
			{
				try {
					_rdpClient.OnConnecting += RDPEvent_OnConnecting;
					_rdpClient.OnConnected += RDPEvent_OnConnected;
					_rdpClient.OnLoginComplete += RDPEvent_OnLoginComplete;
					_rdpClient.OnFatalError += RDPEvent_OnFatalError;
					_rdpClient.OnDisconnected += RDPEvent_OnDisconnected;
					_rdpClient.OnLeaveFullScreenMode += RDPEvent_OnLeaveFullscreenMode;
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strRdpSetEventHandlersFailed + Constants.vbNewLine + ex.Message, true);
				}
			}
			#endregion

			#region "Private Events & Handlers"
			private void RDPEvent_OnFatalError(int errorCode)
			{
				Event_ErrorOccured(this, errorCode);
			}

			private void RDPEvent_OnDisconnected(int discReason)
			{
				const int UI_ERR_NORMAL_DISCONNECT = 0xb08;
				if (!(discReason == UI_ERR_NORMAL_DISCONNECT)) {
					string reason = _rdpClient.GetErrorDescription(discReason, _rdpClient.ExtendedDisconnectReason);
					Event_Disconnected(this, discReason + Constants.vbCrLf + reason);
				}

				if (mRemoteNG.My.Settings.ReconnectOnDisconnect) {
					ReconnectGroup = new ReconnectGroup();
					ReconnectGroup.Left = (Control.Width / 2) - (ReconnectGroup.Width / 2);
					ReconnectGroup.Top = (Control.Height / 2) - (ReconnectGroup.Height / 2);
					ReconnectGroup.Parent = Control;
					ReconnectGroup.Show();
					tmrReconnect.Enabled = true;
				} else {
					Close();
				}
			}

			private void RDPEvent_OnConnecting()
			{
				Event_Connecting(this);
			}

			private void RDPEvent_OnConnected()
			{
				Event_Connected(this);
			}

			private void RDPEvent_OnLoginComplete()
			{
				_loginComplete = true;
			}

			private void RDPEvent_OnLeaveFullscreenMode()
			{
				Fullscreen = false;
				if (LeaveFullscreen != null) {
					LeaveFullscreen(this, new EventArgs());
				}
			}
			#endregion

			#region "Public Events & Handlers"
			public event LeaveFullscreenEventHandler LeaveFullscreen;
			public delegate void LeaveFullscreenEventHandler(Connection.Protocol.RDP sender, System.EventArgs e);
			#endregion

			#region "Enums"
			public enum Defaults
			{
				Colors = RDPColors.Colors16Bit,
				Sounds = RDPSounds.DoNotPlay,
				Resolution = RDPResolutions.FitToWindow,
				Port = 3389
			}

			public enum RDPColors
			{
				[LocalizedDescription("strRDP256Colors")]
				Colors256 = 8,
				[LocalizedDescription("strRDP32768Colors")]
				Colors15Bit = 15,
				[LocalizedDescription("strRDP65536Colors")]
				Colors16Bit = 16,
				[LocalizedDescription("strRDP16777216Colors")]
				Colors24Bit = 24,
				[LocalizedDescription("strRDP4294967296Colors")]
				Colors32Bit = 32
			}

			public enum RDPSounds
			{
				[LocalizedDescription("strRDPSoundBringToThisComputer")]
				BringToThisComputer = 0,
				[LocalizedDescription("strRDPSoundLeaveAtRemoteComputer")]
				LeaveAtRemoteComputer = 1,
				[LocalizedDescription("strRDPSoundDoNotPlay")]
				DoNotPlay = 2
			}

			private enum RDPPerformanceFlags
			{
				[Description("strRDPDisableWallpaper")]
				DisableWallpaper = 0x1,
				[Description("strRDPDisableFullWindowdrag")]
				DisableFullWindowDrag = 0x2,
				[Description("strRDPDisableMenuAnimations")]
				DisableMenuAnimations = 0x4,
				[Description("strRDPDisableThemes")]
				DisableThemes = 0x8,
				[Description("strRDPDisableCursorShadow")]
				DisableCursorShadow = 0x20,
				[Description("strRDPDisableCursorblinking")]
				DisableCursorBlinking = 0x40,
				[Description("strRDPEnableFontSmoothing")]
				EnableFontSmoothing = 0x80,
				[Description("strRDPEnableDesktopComposition")]
				EnableDesktopComposition = 0x100
			}

			public enum RDPResolutions
			{
				[LocalizedDescription("strRDPFitToPanel")]
				FitToWindow,
				[LocalizedDescription("strFullscreen")]
				Fullscreen,
				[LocalizedDescription("strRDPSmartSize")]
				SmartSize,
				[Description("640x480")]
				Res640x480,
				[Description("800x600")]
				Res800x600,
				[Description("1024x768")]
				Res1024x768,
				[Description("1152x864")]
				Res1152x864,
				[Description("1280x800")]
				Res1280x800,
				[Description("1280x1024")]
				Res1280x1024,
				[Description("1400x1050")]
				Res1400x1050,
				[Description("1440x900")]
				Res1440x900,
				[Description("1600x1024")]
				Res1600x1024,
				[Description("1600x1200")]
				Res1600x1200,
				[Description("1600x1280")]
				Res1600x1280,
				[Description("1680x1050")]
				Res1680x1050,
				[Description("1900x1200")]
				Res1900x1200,
				[Description("1920x1200")]
				Res1920x1200,
				[Description("2048x1536")]
				Res2048x1536,
				[Description("2560x2048")]
				Res2560x2048,
				[Description("3200x2400")]
				Res3200x2400,
				[Description("3840x2400")]
				Res3840x2400
			}

			public enum AuthenticationLevel
			{
				[LocalizedDescription("strAlwaysConnectEvenIfAuthFails")]
				NoAuth = 0,
				[LocalizedDescription("strDontConnectWhenAuthFails")]
				AuthRequired = 1,
				[LocalizedDescription("strWarnIfAuthFails")]
				WarnOnFailedAuth = 2
			}

			public enum RDGatewayUsageMethod
			{
				[LocalizedDescription("strNever")]
				Never = 0,
				// TSC_PROXY_MODE_NONE_DIRECT
				[LocalizedDescription("strAlways")]
				Always = 1,
				// TSC_PROXY_MODE_DIRECT
				[LocalizedDescription("strDetect")]
				Detect = 2
				// TSC_PROXY_MODE_DETECT
			}

			public enum RDGatewayUseConnectionCredentials
			{
				[LocalizedDescription("strUseDifferentUsernameAndPassword")]
				No = 0,
				[LocalizedDescription("strUseSameUsernameAndPassword")]
				Yes = 1,
				[LocalizedDescription("strUseSmartCard")]
				SmartCard = 2
			}
			#endregion

			#region "Resolution"
			public static Rectangle GetResolutionRectangle(RDPResolutions resolution)
			{
				string[] resolutionParts = null;
				if (!(resolution == RDPResolutions.FitToWindow) & !(resolution == RDPResolutions.Fullscreen) & !(resolution == RDPResolutions.SmartSize)) {
					resolutionParts = resolution.ToString().Replace("Res", "").Split("x");
				}
				if (resolutionParts == null || !(resolutionParts.Length == 2)) {
					return new Rectangle(0, 0, 0, 0);
				} else {
					return new Rectangle(0, 0, resolutionParts[0], resolutionParts[1]);
				}
			}
			#endregion

			public class Versions
			{
				public static Version RDC60 = new Version(6, 0, 6000);
				public static Version RDC61 = new Version(6, 0, 6001);
				public static Version RDC70 = new Version(6, 1, 7600);
				public static Version RDC80 = new Version(6, 2, 9200);
			}

			#region "Terminal Sessions"
			public class TerminalSessions
			{

				private readonly WTSCOM _wtsCom;
				public TerminalSessions()
				{
					try {
						_wtsCom = new WTSCOM();
					} catch (Exception ex) {
						mRemoteNG.App.Runtime.MessageCollector.AddExceptionMessage("TerminalSessions.New() failed.", ex, MessageClass.ErrorMsg, true);
					}
				}

				public long OpenConnection(string hostname)
				{
					if (_wtsCom == null)
						return 0;

					try {
						return _wtsCom.WTSOpenServer(hostname);
					} catch (Exception ex) {
						mRemoteNG.App.Runtime.MessageCollector.AddExceptionMessage(mRemoteNG.My.Language.strRdpOpenConnectionFailed, ex, MessageClass.ErrorMsg, true);
					}
				}

				public void CloseConnection(long serverHandle)
				{
					if (_wtsCom == null)
						return;

					try {
						_wtsCom.WTSCloseServer(serverHandle);
					} catch (Exception ex) {
						mRemoteNG.App.Runtime.MessageCollector.AddExceptionMessage(mRemoteNG.My.Language.strRdpCloseConnectionFailed, ex, MessageClass.ErrorMsg, true);
					}
				}

				public SessionsCollection GetSessions(long serverHandle)
				{
					if (_wtsCom == null)
						return new SessionsCollection();

					SessionsCollection sessions = new SessionsCollection();

					try {
						WTSSessions wtsSessions = _wtsCom.WTSEnumerateSessions(serverHandle);

						long sessionId = 0;
						string sessionUser = null;
						long sessionState = 0;
						string sessionName = null;

						foreach (WTSSession wtsSession in wtsSessions) {
							sessionId = wtsSession.SessionId;
							sessionUser = _wtsCom.WTSQuerySessionInformation(serverHandle, wtsSession.SessionId, 5);
							// WFUsername = 5
							sessionState = wtsSession.State + Constants.vbCrLf;
							sessionName = wtsSession.WinStationName + Constants.vbCrLf;

							if (!string.IsNullOrEmpty(sessionUser)) {
								if (sessionState == 0) {
									sessions.Add(sessionId, mRemoteNG.My.Language.strActive, sessionUser, sessionName);
								} else {
									sessions.Add(sessionId, mRemoteNG.My.Language.strInactive, sessionUser, sessionName);
								}
							}
						}
					} catch (Exception ex) {
						mRemoteNG.App.Runtime.MessageCollector.AddExceptionMessage(mRemoteNG.My.Language.strRdpGetSessionsFailed, ex, MessageClass.ErrorMsg, true);
					}

					return sessions;
				}

				public bool KillSession(long serverHandle, long sessionId)
				{
					if (_wtsCom == null)
						return false;

					bool result = false;

					try {
						result = _wtsCom.WTSLogoffSession(serverHandle, sessionId, true);
					} catch (Exception ex) {
						mRemoteNG.App.Runtime.MessageCollector.AddExceptionMessage("TerminalSessions.KillSession() failed.", ex, MessageClass.ErrorMsg, true);
					}

					return result;
				}
			}

			public class SessionsCollection : CollectionBase
			{

				public Session this[int index] {
					get { return (Session)List[index]; }
				}

				public int ItemsCount {
					get { return List.Count; }
				}

				public Session Add(long sessionId, string sessionState, string sessionUser, string sessionName)
				{
					Session newSession = new Session();

					try {
						var _with1 = newSession;
						_with1.SessionId = sessionId;
						_with1.SessionState = sessionState;
						_with1.SessionUser = sessionUser;
						_with1.SessionName = sessionName;

						List.Add(newSession);
					} catch (Exception ex) {
						mRemoteNG.App.Runtime.MessageCollector.AddExceptionMessage(mRemoteNG.My.Language.strRdpAddSessionFailed, ex, MessageClass.ErrorMsg, true);
					}

					return newSession;
				}

				public void ClearSessions()
				{
					List.Clear();
				}
			}

			public class Session : CollectionBase
			{

				public long SessionId { get; set; }
				public string SessionState { get; set; }
				public string SessionUser { get; set; }
				public string SessionName { get; set; }
			}
			#endregion

			#region "Fatal Errors"
			public class FatalErrors
			{
				protected static string[] _description = {
					0 == mRemoteNG.My.Language.strRdpErrorUnknown,
					1 == mRemoteNG.My.Language.strRdpErrorCode1,
					2 == mRemoteNG.My.Language.strRdpErrorOutOfMemory,
					3 == mRemoteNG.My.Language.strRdpErrorWindowCreation,
					4 == mRemoteNG.My.Language.strRdpErrorCode2,
					5 == mRemoteNG.My.Language.strRdpErrorCode3,
					6 == mRemoteNG.My.Language.strRdpErrorCode4,
					7 == mRemoteNG.My.Language.strRdpErrorConnection,
					100 == mRemoteNG.My.Language.strRdpErrorWinsock

				};
				public static string GetError(string id)
				{
					try {
						return (_description[id]);
					} catch (Exception ex) {
						mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strRdpErrorGetFailure + Constants.vbNewLine + ex.Message, true);
						return string.Format(mRemoteNG.My.Language.strRdpErrorUnknown, id);
					}
				}
			}
			#endregion

			#region "Reconnect Stuff"
			private void tmrReconnect_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
			{
				bool srvReady = mRemoteNG.Tools.PortScan.Scanner.IsPortOpen(_connectionInfo.Hostname, _connectionInfo.Port);

				ReconnectGroup.ServerReady = srvReady;

				if (ReconnectGroup.ReconnectWhenReady & srvReady) {
					tmrReconnect.Enabled = false;
					ReconnectGroup.DisposeReconnectGroup();
					//SetProps()
					_rdpClient.Connect();
				}
			}
			#endregion
		}
	}
}

