using System;
using System.Drawing;
using System.Diagnostics;
using AxMSTSCLib;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;
using mRemoteNG.Messages;
using mRemoteNG.App;
using MSTSCLib;
using mRemoteNG.Tools;
using mRemoteNG.UI.Forms;


namespace mRemoteNG.Connection.Protocol.RDP
{
    public class ProtocolRDP : ProtocolBase
	{
        #region Private Declarations
        /* RDP v8 requires Windows 7 with:
         * https://support.microsoft.com/en-us/kb/2592687 
         * OR
         * https://support.microsoft.com/en-us/kb/2923545
         * 
         * Windows 8+ support RDP v8 out of the box.
         */
        private MsRdpClient8NotSafeForScripting _rdpClient;
        private Version _rdpVersion;
        private ConnectionInfo _connectionInfo;
        private bool _loginComplete;
        private bool _redirectKeys = false;
        #endregion

        #region Properties
        public bool SmartSize
		{
			get
			{
				return _rdpClient.AdvancedSettings2.SmartSizing;
			}
			set
			{
				_rdpClient.AdvancedSettings2.SmartSizing = value;
				ReconnectForResize();
			}
		}
		
        public bool Fullscreen
		{
			get
			{
				return _rdpClient.FullScreen;
			}
			set
			{
				_rdpClient.FullScreen = value;
				ReconnectForResize();
			}
		}
		
        public bool RedirectKeys
		{
			get
			{
				return _redirectKeys;
			}
			set
			{
				_redirectKeys = value;
				try
				{
					if (!_redirectKeys)
					{
						return ;
					}
							
					Debug.Assert(Convert.ToBoolean(_rdpClient.SecuredSettingsEnabled));
                    IMsRdpClientSecuredSettings msRdpClientSecuredSettings = _rdpClient.SecuredSettings2;
					msRdpClientSecuredSettings.KeyboardHookMode = 1; // Apply key combinations at the remote server.
				}
				catch (Exception ex)
				{
					Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strRdpSetRedirectKeysFailed + Environment.NewLine + ex.Message, true);
				}
			}
		}
        #endregion

        #region Constructors
        public ProtocolRDP()
        {
            Control = new AxMsRdpClient8NotSafeForScripting();
        }
        #endregion

        #region Public Methods
		public override bool Initialize()
		{
			base.Initialize();
			try
			{
				Control.CreateControl();
				_connectionInfo = InterfaceControl.Info;
				
				try
				{
					while (!Control.Created)
					{
						Thread.Sleep(0);
                        Application.DoEvents();
					}
                    _rdpClient = (MsRdpClient8NotSafeForScripting)((AxMsRdpClient8NotSafeForScripting)Control).GetOcx();

				}
				catch (System.Runtime.InteropServices.COMException ex)
				{
					Runtime.MessageCollector.AddExceptionMessage(My.Language.strRdpControlCreationFailed, ex);
					Control.Dispose();
					return false;
				}
						
				_rdpVersion = new Version(_rdpClient.Version);
						
				_rdpClient.Server = _connectionInfo.Hostname;

                SetCredentials();
                SetResolution();
                _rdpClient.FullScreenTitle = _connectionInfo.Name;
						
				//not user changeable
				_rdpClient.AdvancedSettings2.GrabFocusOnConnect = true;
				_rdpClient.AdvancedSettings3.EnableAutoReconnect = true;
				_rdpClient.AdvancedSettings3.MaxReconnectAttempts = Convert.ToInt32(My.Settings.Default.RdpReconnectionCount);
				_rdpClient.AdvancedSettings2.keepAliveInterval = 60000; //in milliseconds (10.000 = 10 seconds)
				_rdpClient.AdvancedSettings5.AuthenticationLevel = 0;
				_rdpClient.AdvancedSettings2.EncryptionEnabled = 1;
						
				_rdpClient.AdvancedSettings2.overallConnectionTimeout = 20;
						
				_rdpClient.AdvancedSettings2.BitmapPeristence = Convert.ToInt32(_connectionInfo.CacheBitmaps);
				if (_rdpVersion >= Versions.RDC61)
				{
					_rdpClient.AdvancedSettings7.EnableCredSspSupport = _connectionInfo.UseCredSsp;
				}

                SetUseConsoleSession();
                SetPort();
				RedirectKeys = _connectionInfo.RedirectKeys;
                SetRedirection();
                SetAuthenticationLevel();
				SetLoadBalanceInfo();
                SetRdGateway();
						
				_rdpClient.ColorDepth = Convert.ToInt32(Conversion.Int(_connectionInfo.Colors));

                SetPerformanceFlags();
						
				_rdpClient.ConnectingText = My.Language.strConnecting;
						
				Control.Anchor = AnchorStyles.None;
						
				return true;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strRdpSetPropsFailed + Environment.NewLine + ex.Message, true);
				return false;
			}
		}
				
		public override bool Connect()
		{
			_loginComplete = false;
			SetEventHandlers();
					
			try
			{
				_rdpClient.Connect();
				base.Connect();
				return true;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strRdpConnectionOpenFailed + Environment.NewLine + ex.Message);
			}
					
			return false;
		}
				
		public override void Disconnect()
		{
			try
			{
				_rdpClient.Disconnect();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strRdpDisconnectFailed + Environment.NewLine + ex.Message, true);
				base.Close();
			}
		}
				
		public void ToggleFullscreen()
		{
			try
			{
                Fullscreen = !Fullscreen;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strRdpToggleFullscreenFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		public void ToggleSmartSize()
		{
			try
			{
                SmartSize = !SmartSize;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strRdpToggleSmartSizeFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		public override void Focus()
		{
			try
			{
				if (Control.ContainsFocus == false)
				{
					Control.Focus();
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strRdpFocusFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		private Size _controlBeginningSize = new Size();
		public override void ResizeBegin(object sender, EventArgs e)
		{
			_controlBeginningSize = Control.Size;
		}
				
		public override void Resize(object sender, EventArgs e)
		{
			if (DoResize() && _controlBeginningSize.IsEmpty)
			{
				ReconnectForResize();
			}
			base.Resize(sender, e);
		}
				
		public override void ResizeEnd(object sender, EventArgs e)
		{
			DoResize();
			if (!(Control.Size == _controlBeginningSize))
			{
				ReconnectForResize();
			}
			_controlBeginningSize = Size.Empty;
		}
        #endregion
		
        #region Private Methods
		private bool DoResize()
		{
			Control.Location = InterfaceControl.Location;
			if (!(Control.Size == InterfaceControl.Size) && !(InterfaceControl.Size == Size.Empty))
			{
				Control.Size = InterfaceControl.Size;
				return true;
			}
			else
			{
				return false;
			}
		}
				
		private void ReconnectForResize()
		{
			if (_rdpVersion < Versions.RDC80)
			{
				return ;
			}
					
			if (!_loginComplete)
			{
				return ;
			}
					
			if (!InterfaceControl.Info.AutomaticResize)
			{
				return ;
			}
					
			if (!(InterfaceControl.Info.Resolution == RDPResolutions.FitToWindow | InterfaceControl.Info.Resolution == RDPResolutions.Fullscreen))
			{
				return ;
			}
					
			if (SmartSize)
			{
				return ;
			}
					
			Size size = new Size();
			if (!Fullscreen)
			{
				size = Control.Size;
			}
			else
			{
				size = Screen.FromControl(Control).Bounds.Size;
			}

            IMsRdpClient8 msRdpClient8 = _rdpClient;
			msRdpClient8.Reconnect((uint)size.Width, (uint)size.Height);
		}
				
		private void SetRdGateway()
		{
			try
			{
				if (_rdpClient.TransportSettings.GatewayIsSupported == 0)
				{
					Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, My.Language.strRdpGatewayNotSupported, true);
					return ;
				}
				else
				{
					Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, My.Language.strRdpGatewayIsSupported, true);
				}
						
				if (_connectionInfo.RDGatewayUsageMethod != RDGatewayUsageMethod.Never)
				{
					_rdpClient.TransportSettings.GatewayUsageMethod = (uint)_connectionInfo.RDGatewayUsageMethod;
					_rdpClient.TransportSettings.GatewayHostname = _connectionInfo.RDGatewayHostname;
					_rdpClient.TransportSettings.GatewayProfileUsageMethod = 1; // TSC_PROXY_PROFILE_MODE_EXPLICIT
					if (_connectionInfo.RDGatewayUseConnectionCredentials == RDGatewayUseConnectionCredentials.SmartCard)
					{
						_rdpClient.TransportSettings.GatewayCredsSource = 1; // TSC_PROXY_CREDS_MODE_SMARTCARD
					}
					if (_rdpVersion >= Versions.RDC61 && !((Force & ConnectionInfo.Force.NoCredentials) == ConnectionInfo.Force.NoCredentials))
					{
						if (_connectionInfo.RDGatewayUseConnectionCredentials == RDGatewayUseConnectionCredentials.Yes)
						{
							//_rdpClient.TransportSettings2.GatewayUsername = _connectionInfo.Username;
							//_rdpClient.TransportSettings2.GatewayPassword = _connectionInfo.Password;
							//_rdpClient.TransportSettings2.GatewayDomain = _connectionInfo.Domain;
						}
						else if (_connectionInfo.RDGatewayUseConnectionCredentials == RDGatewayUseConnectionCredentials.SmartCard)
						{
							//_rdpClient.TransportSettings2.GatewayCredSharing = 0;
						}
						else
						{
							//_rdpClient.TransportSettings2.GatewayUsername = _connectionInfo.RDGatewayUsername;
							//_rdpClient.TransportSettings2.GatewayPassword = _connectionInfo.RDGatewayPassword;
							//_rdpClient.TransportSettings2.GatewayDomain = _connectionInfo.RDGatewayDomain;
							//_rdpClient.TransportSettings2.GatewayCredSharing = 0;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strRdpSetGatewayFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void SetUseConsoleSession()
		{
			try
			{
				bool value = false;
						
				if ((Force & ConnectionInfo.Force.UseConsoleSession) == ConnectionInfo.Force.UseConsoleSession)
				{
					value = true;
				}
				else if ((Force & ConnectionInfo.Force.DontUseConsoleSession) == ConnectionInfo.Force.DontUseConsoleSession)
				{
					value = false;
				}
				else
				{
					value = _connectionInfo.UseConsoleSession;
				}
						
				if (_rdpVersion >= Versions.RDC61)
				{
					Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format(My.Language.strRdpSetConsoleSwitch, _rdpVersion), true);
					//_rdpClient.AdvancedSettings7.ConnectToAdministerServer = value;
				}
				else
				{
					Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format(My.Language.strRdpSetConsoleSwitch, _rdpVersion), true);
					_rdpClient.AdvancedSettings2.ConnectToServerConsole = value;
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage(My.Language.strRdpSetConsoleSessionFailed, ex, MessageClass.ErrorMsg, true);
			}
		}
				
		private void SetCredentials()
		{
			try
			{
				if ((Force & ConnectionInfo.Force.NoCredentials) == ConnectionInfo.Force.NoCredentials)
				{
					return ;
				}
						
				string userName = _connectionInfo.Username;
				string password = _connectionInfo.Password;
				string domain = _connectionInfo.Domain;
						
				if (string.IsNullOrEmpty(userName))
				{
					if (My.Settings.Default.EmptyCredentials == "windows")
					{
						_rdpClient.UserName = Environment.UserName;
					}
					else if (My.Settings.Default.EmptyCredentials == "custom")
					{
						_rdpClient.UserName = Convert.ToString(My.Settings.Default.DefaultUsername);
					}
				}
				else
				{
					_rdpClient.UserName = userName;
				}
						
				if (string.IsNullOrEmpty(password))
				{
					if (My.Settings.Default.EmptyCredentials == "custom")
					{
						if (My.Settings.Default.DefaultPassword != "")
						{
							_rdpClient.AdvancedSettings2.ClearTextPassword = Security.Crypt.Decrypt(Convert.ToString(My.Settings.Default.DefaultPassword), App.Info.GeneralAppInfo.EncryptionKey);
						}
					}
				}
				else
				{
					_rdpClient.AdvancedSettings2.ClearTextPassword = password;
				}
						
				if (string.IsNullOrEmpty(domain))
				{
					if (My.Settings.Default.EmptyCredentials == "windows")
					{
						_rdpClient.Domain = Environment.UserDomainName;
					}
					else if (My.Settings.Default.EmptyCredentials == "custom")
					{
						_rdpClient.Domain = Convert.ToString(My.Settings.Default.DefaultDomain);
					}
				}
				else
				{
					_rdpClient.Domain = domain;
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strRdpSetCredentialsFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void SetResolution()
		{
			try
			{
				if ((Force & ConnectionInfo.Force.Fullscreen) == ConnectionInfo.Force.Fullscreen)
				{
					_rdpClient.FullScreen = true;
                    _rdpClient.DesktopWidth = Screen.FromControl(frmMain.Default).Bounds.Width;
                    _rdpClient.DesktopHeight = Screen.FromControl(frmMain.Default).Bounds.Height;
							
					return;
				}
						
				if ((InterfaceControl.Info.Resolution == RDPResolutions.FitToWindow) || (InterfaceControl.Info.Resolution == RDPResolutions.SmartSize))
				{
					_rdpClient.DesktopWidth = InterfaceControl.Size.Width;
					_rdpClient.DesktopHeight = InterfaceControl.Size.Height;
				}
				else if (InterfaceControl.Info.Resolution == RDPResolutions.Fullscreen)
				{
					_rdpClient.FullScreen = true;
                    _rdpClient.DesktopWidth = Screen.FromControl(frmMain.Default).Bounds.Width;
                    _rdpClient.DesktopHeight = Screen.FromControl(frmMain.Default).Bounds.Height;
				}
				else
				{
					Rectangle resolution = GetResolutionRectangle(_connectionInfo.Resolution);
					_rdpClient.DesktopWidth = resolution.Width;
					_rdpClient.DesktopHeight = resolution.Height;
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strRdpSetResolutionFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void SetPort()
		{
			try
			{
				if (_connectionInfo.Port != (int)Defaults.Port)
				{
					_rdpClient.AdvancedSettings2.RDPPort = _connectionInfo.Port;
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strRdpSetPortFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void SetRedirection()
		{
			try
			{
				_rdpClient.AdvancedSettings2.RedirectDrives = _connectionInfo.RedirectDiskDrives;
				_rdpClient.AdvancedSettings2.RedirectPorts = _connectionInfo.RedirectPorts;
				_rdpClient.AdvancedSettings2.RedirectPrinters = _connectionInfo.RedirectPrinters;
				_rdpClient.AdvancedSettings2.RedirectSmartCards = _connectionInfo.RedirectSmartCards;
				_rdpClient.SecuredSettings2.AudioRedirectionMode = Convert.ToInt32(Conversion.Int(_connectionInfo.RedirectSound));
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strRdpSetRedirectionFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void SetPerformanceFlags()
		{
			try
			{
				int pFlags = 0;
				if (_connectionInfo.DisplayThemes == false)
				{
					pFlags += Convert.ToInt32(Conversion.Int(RDPPerformanceFlags.DisableThemes));
				}
						
				if (_connectionInfo.DisplayWallpaper == false)
				{
					pFlags += Convert.ToInt32(Conversion.Int(RDPPerformanceFlags.DisableWallpaper));
				}
						
				if (_connectionInfo.EnableFontSmoothing)
				{
					pFlags += Convert.ToInt32(Conversion.Int(RDPPerformanceFlags.EnableFontSmoothing));
				}
						
				if (_connectionInfo.EnableDesktopComposition)
				{
					pFlags += Convert.ToInt32(Conversion.Int(RDPPerformanceFlags.EnableDesktopComposition));
				}
						
				_rdpClient.AdvancedSettings2.PerformanceFlags = pFlags;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strRdpSetPerformanceFlagsFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void SetAuthenticationLevel()
		{
			try
			{
				_rdpClient.AdvancedSettings5.AuthenticationLevel = (uint)_connectionInfo.RDPAuthenticationLevel;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strRdpSetAuthenticationLevelFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void SetLoadBalanceInfo()
		{
			if (string.IsNullOrEmpty(_connectionInfo.LoadBalanceInfo))
			{
				return ;
			}
			try
			{
				_rdpClient.AdvancedSettings2.LoadBalanceInfo = _connectionInfo.LoadBalanceInfo;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage("Unable to set load balance info.", ex);
			}
		}
				
		private void SetEventHandlers()
		{
			try
			{
				_rdpClient.OnConnecting += RDPEvent_OnConnecting;
				_rdpClient.OnConnected += RDPEvent_OnConnected;
				_rdpClient.OnLoginComplete += RDPEvent_OnLoginComplete;
				_rdpClient.OnFatalError += RDPEvent_OnFatalError;
				_rdpClient.OnDisconnected += RDPEvent_OnDisconnected;
				_rdpClient.OnLeaveFullScreenMode += RDPEvent_OnLeaveFullscreenMode;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strRdpSetEventHandlersFailed + Environment.NewLine + ex.Message, true);
			}
		}
        #endregion
		
        #region Private Events & Handlers
		private void RDPEvent_OnFatalError(int errorCode)
		{
			Event_ErrorOccured(this, Convert.ToString(errorCode));
		}
				
		private void RDPEvent_OnDisconnected(int discReason)
		{
			const int UI_ERR_NORMAL_DISCONNECT = 0xB08;
			if (discReason != UI_ERR_NORMAL_DISCONNECT)
			{
				string reason = _rdpClient.GetErrorDescription((uint)discReason, (uint) _rdpClient.ExtendedDisconnectReason);
				Event_Disconnected(this, discReason + "\r\n" + reason);
			}
					
			if (My.Settings.Default.ReconnectOnDisconnect)
			{
				ReconnectGroup = new ReconnectGroup();
				ReconnectGroup.CloseClicked += Event_ReconnectGroupCloseClicked;
				ReconnectGroup.Left = (int) (((double) Control.Width / 2) - ((double) ReconnectGroup.Width / 2));
				ReconnectGroup.Top = (int) (((double) Control.Height / 2) - ((double) ReconnectGroup.Height / 2));
				ReconnectGroup.Parent = Control;
				ReconnectGroup.Show();
				tmrReconnect.Enabled = true;
			}
			else
			{
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
			if (LeaveFullscreenEvent != null)
				LeaveFullscreenEvent(this, new EventArgs());
		}
        #endregion
		
        #region Public Events & Handlers
		public delegate void LeaveFullscreenEventHandler(ProtocolRDP sender, EventArgs e);
		private LeaveFullscreenEventHandler LeaveFullscreenEvent;
				
		public event LeaveFullscreenEventHandler LeaveFullscreen
		{
			add
			{
				LeaveFullscreenEvent = (LeaveFullscreenEventHandler)Delegate.Combine(LeaveFullscreenEvent, value);
			}
			remove
			{
				LeaveFullscreenEvent = (LeaveFullscreenEventHandler)Delegate.Remove(LeaveFullscreenEvent, value);
			}
		}
        #endregion
		
        #region Enums
		public enum Defaults
		{
			Colors = RDPColors.Colors16Bit,
			Sounds = RDPSounds.DoNotPlay,
			Resolution = RDPResolutions.FitToWindow,
			Port = 3389
		}
				
		public enum RDPColors
		{
            [LocalizedAttributes.LocalizedDescription("strRDP256Colors")]
            Colors256 = 8,
            [LocalizedAttributes.LocalizedDescription("strRDP32768Colors")]
            Colors15Bit = 15,
            [LocalizedAttributes.LocalizedDescription("strRDP65536Colors")]
            Colors16Bit = 16,
            [LocalizedAttributes.LocalizedDescription("strRDP16777216Colors")]
            Colors24Bit = 24,
            [LocalizedAttributes.LocalizedDescription("strRDP4294967296Colors")]
            Colors32Bit = 32
		}
				
		public enum RDPSounds
		{
            [LocalizedAttributes.LocalizedDescription("strRDPSoundBringToThisComputer")]
            BringToThisComputer = 0,
            [LocalizedAttributes.LocalizedDescription("strRDPSoundLeaveAtRemoteComputer")]
            LeaveAtRemoteComputer = 1,
            [LocalizedAttributes.LocalizedDescription("strRDPSoundDoNotPlay")]
            DoNotPlay = 2
		}
				
		private enum RDPPerformanceFlags
		{
			[Description("strRDPDisableWallpaper")]DisableWallpaper = 0x1,
			[Description("strRDPDisableFullWindowdrag")]DisableFullWindowDrag = 0x2,
			[Description("strRDPDisableMenuAnimations")]DisableMenuAnimations = 0x4,
			[Description("strRDPDisableThemes")]DisableThemes = 0x8,
			[Description("strRDPDisableCursorShadow")]DisableCursorShadow = 0x20,
			[Description("strRDPDisableCursorblinking")]DisableCursorBlinking = 0x40,
			[Description("strRDPEnableFontSmoothing")]EnableFontSmoothing = 0x80,
			[Description("strRDPEnableDesktopComposition")]EnableDesktopComposition = 0x100
		}
				
		public enum RDPResolutions
		{
            [LocalizedAttributes.LocalizedDescription("strRDPFitToPanel")]
            FitToWindow,
            [LocalizedAttributes.LocalizedDescription("strFullscreen")]
            Fullscreen,
            [LocalizedAttributes.LocalizedDescription("strRDPSmartSize")]
            SmartSize,
			[Description("640x480")]Res640x480,
			[Description("800x600")]Res800x600,
			[Description("1024x768")]Res1024x768,
			[Description("1152x864")]Res1152x864,
			[Description("1280x800")]Res1280x800,
			[Description("1280x1024")]Res1280x1024,
			[Description("1400x1050")]Res1400x1050,
			[Description("1440x900")]Res1440x900,
			[Description("1600x1024")]Res1600x1024,
			[Description("1600x1200")]Res1600x1200,
			[Description("1600x1280")]Res1600x1280,
			[Description("1680x1050")]Res1680x1050,
			[Description("1900x1200")]Res1900x1200,
			[Description("1920x1200")]Res1920x1200,
			[Description("2048x1536")]Res2048x1536,
			[Description("2560x2048")]Res2560x2048,
			[Description("3200x2400")]Res3200x2400,
			[Description("3840x2400")]Res3840x2400
		}
				
		public enum AuthenticationLevel
		{
            [LocalizedAttributes.LocalizedDescription("strAlwaysConnectEvenIfAuthFails")]
            NoAuth = 0,
            [LocalizedAttributes.LocalizedDescription("strDontConnectWhenAuthFails")]
            AuthRequired = 1,
            [LocalizedAttributes.LocalizedDescription("strWarnIfAuthFails")]
            WarnOnFailedAuth = 2
		}
				
		public enum RDGatewayUsageMethod
		{
            [LocalizedAttributes.LocalizedDescription("strNever")]
            Never = 0, // TSC_PROXY_MODE_NONE_DIRECT
            [LocalizedAttributes.LocalizedDescription("strAlways")]
            Always = 1, // TSC_PROXY_MODE_DIRECT
            [LocalizedAttributes.LocalizedDescription("strDetect")]
            Detect = 2 // TSC_PROXY_MODE_DETECT
		}
				
		public enum RDGatewayUseConnectionCredentials
		{
            [LocalizedAttributes.LocalizedDescription("strUseDifferentUsernameAndPassword")]
            No = 0,
            [LocalizedAttributes.LocalizedDescription("strUseSameUsernameAndPassword")]
            Yes = 1,
            [LocalizedAttributes.LocalizedDescription("strUseSmartCard")]
            SmartCard = 2
		}
        #endregion
		
        #region Resolution
		public static Rectangle GetResolutionRectangle(RDPResolutions resolution)
		{
			string[] resolutionParts = null;
			if (!(resolution == RDPResolutions.FitToWindow) & !(resolution == RDPResolutions.Fullscreen) & !(resolution == RDPResolutions.SmartSize))
			{
				resolutionParts = resolution.ToString().Replace("Res", "").Split('x');
			}
			if (resolutionParts == null || !(resolutionParts.Length == 2))
			{
				return new Rectangle(0, 0, 0, 0);
			}
			else
			{
                return new Rectangle(0, 0, Convert.ToInt32(resolutionParts[0]), Convert.ToInt32(resolutionParts[1]));
			}
		}
        #endregion
		
		public class Versions
		{
			public static Version RDC60 = new Version(6, 0, 6000);
			public static Version RDC61 = new Version(6, 0, 6001);
			public static Version RDC70 = new Version(6, 1, 7600);
			public static Version RDC80 = new Version(6, 2, 9200);
            public static Version RDC81 = new Version(6, 3, 9600);
		}
		
        #region Fatal Errors
		public class FatalErrors
		{
            protected static Hashtable _description;
            protected static void InitDescription()
            {
                _description = new Hashtable();
                _description.Add("0", "My.Language.strRdpErrorUnknown");
                _description.Add("1", "My.Language.strRdpErrorCode1");
                _description.Add("2", "My.Language.strRdpErrorOutOfMemory");
                _description.Add("3", "My.Language.strRdpErrorWindowCreation");
                _description.Add("4", "My.Language.strRdpErrorCode2");
                _description.Add("5", "My.Language.strRdpErrorCode3");
                _description.Add("6", "My.Language.strRdpErrorCode4");
                _description.Add("7", "My.Language.strRdpErrorConnection");
                _description.Add("100", "My.Language.strRdpErrorWinsock");
            }
					
			public static string GetError(string id)
			{
                if (_description == null)
                {
                    InitDescription();
                }
				try
				{
					return ((string)_description[id]);
				}
				catch (Exception ex)
				{
					Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strRdpErrorGetFailure + Environment.NewLine + ex.Message, true);
					return string.Format(My.Language.strRdpErrorUnknown, id);
				}
			}
		}
        #endregion
		
        #region Reconnect Stuff
		public void tmrReconnect_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			bool srvReady = Tools.PortScan.Scanner.IsPortOpen(_connectionInfo.Hostname, Convert.ToString(_connectionInfo.Port));
					
			ReconnectGroup.ServerReady = srvReady;
					
			if (ReconnectGroup.ReconnectWhenReady && srvReady)
			{
				tmrReconnect.Enabled = false;
				ReconnectGroup.DisposeReconnectGroup();
				//SetProps()
				_rdpClient.Connect();
			}
		}
        #endregion
	}
}