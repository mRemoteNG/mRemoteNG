using System;
using System.Drawing;
using System.Diagnostics;
using AxMSTSCLib;
using System.Collections;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;
using System.Security;
using mRemoteNG.Messages;
using mRemoteNG.App;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
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
        private bool _redirectKeys;
        private bool _alertOnIdleDisconnect;
        #endregion

        #region Properties
        public bool SmartSize
		{
			get
			{
				return _rdpClient.AdvancedSettings2.SmartSizing;
			}
            private set
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
            private set
			{
				_rdpClient.FullScreen = value;
				ReconnectForResize();
			}
		}

	    private bool RedirectKeys
		{
/*
			get
			{
				return _redirectKeys;
			}
*/
			set
			{
				_redirectKeys = value;
				try
				{
					if (!_redirectKeys)
					{
						return;
					}
							
					Debug.Assert(Convert.ToBoolean(_rdpClient.SecuredSettingsEnabled));
                    var msRdpClientSecuredSettings = _rdpClient.SecuredSettings2;
					msRdpClientSecuredSettings.KeyboardHookMode = 1; // Apply key combinations at the remote server.
				}
				catch (Exception ex)
				{
					Runtime.MessageCollector.AddExceptionStackTrace(Language.strRdpSetRedirectKeysFailed, ex);
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
					Runtime.MessageCollector.AddExceptionMessage(Language.strRdpControlCreationFailed, ex);
					Control.Dispose();
					return false;
				}
						
				_rdpVersion = new Version(_rdpClient.Version);
						
				_rdpClient.Server = _connectionInfo.Hostname;

                SetCredentials();
                SetResolution();
                _rdpClient.FullScreenTitle = _connectionInfo.Name;

                _alertOnIdleDisconnect = _connectionInfo.RDPAlertIdleTimeout;
                _rdpClient.AdvancedSettings2.MinutesToIdleTimeout = _connectionInfo.RDPMinutesToIdleTimeout;

                //not user changeable
                _rdpClient.AdvancedSettings2.GrabFocusOnConnect = true;
				_rdpClient.AdvancedSettings3.EnableAutoReconnect = true;
				_rdpClient.AdvancedSettings3.MaxReconnectAttempts = Settings.Default.RdpReconnectionCount;
				_rdpClient.AdvancedSettings2.keepAliveInterval = 60000; //in milliseconds (10,000 = 10 seconds)
				_rdpClient.AdvancedSettings5.AuthenticationLevel = 0;
				_rdpClient.AdvancedSettings2.EncryptionEnabled = 1;
						
				_rdpClient.AdvancedSettings2.overallConnectionTimeout = Settings.Default.ConRDPOverallConnectionTimeout;
						
				_rdpClient.AdvancedSettings2.BitmapPeristence = Convert.ToInt32(_connectionInfo.CacheBitmaps);
				if (_rdpVersion >= Versions.RDC61)
				{
					_rdpClient.AdvancedSettings7.EnableCredSspSupport = _connectionInfo.UseCredSsp;
				    _rdpClient.AdvancedSettings8.AudioQualityMode = (uint)_connectionInfo.SoundQuality;
				}

                SetUseConsoleSession();
                SetPort();
				RedirectKeys = _connectionInfo.RedirectKeys;
                SetRedirection();
                SetAuthenticationLevel();
				SetLoadBalanceInfo();
                SetRdGateway();
						
				_rdpClient.ColorDepth = (int)_connectionInfo.Colors;

                SetPerformanceFlags();
						
				_rdpClient.ConnectingText = Language.strConnecting;
						
				Control.Anchor = AnchorStyles.None;
						
				return true;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace(Language.strRdpSetPropsFailed, ex);
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
				Runtime.MessageCollector.AddExceptionStackTrace(Language.strRdpConnectionOpenFailed, ex);
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
				Runtime.MessageCollector.AddExceptionStackTrace(Language.strRdpDisconnectFailed, ex);
				Close();
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
				Runtime.MessageCollector.AddExceptionStackTrace(Language.strRdpToggleFullscreenFailed, ex);
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
				Runtime.MessageCollector.AddExceptionStackTrace(Language.strRdpToggleSmartSizeFailed, ex);
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
				Runtime.MessageCollector.AddExceptionStackTrace(Language.strRdpFocusFailed, ex);
			}
		}
				
		private Size _controlBeginningSize;
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
			if (!(Control.Size == InterfaceControl.Size) && !(InterfaceControl.Size == Size.Empty)) // kmscode - this doesn't look right to me. But I'm not aware of any functionality issues with this currently...
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
				return;
			}
					
			if (!_loginComplete)
			{
				return;
			}
					
			if (!InterfaceControl.Info.AutomaticResize)
			{
				return;
			}
					
			if (!(InterfaceControl.Info.Resolution == RDPResolutions.FitToWindow | InterfaceControl.Info.Resolution == RDPResolutions.Fullscreen))
			{
				return;
			}
					
			if (SmartSize)
			{
				return;
			}

		    var size = !Fullscreen ? Control.Size : Screen.FromControl(Control).Bounds.Size;

            IMsRdpClient8 msRdpClient8 = _rdpClient;
			msRdpClient8.Reconnect((uint)size.Width, (uint)size.Height);
		}
				
		private void SetRdGateway()
		{
			try
			{
				if (_rdpClient.TransportSettings.GatewayIsSupported == 0)
				{
					Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.strRdpGatewayNotSupported, true);
					return;
				}
			    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.strRdpGatewayIsSupported, true);

			    if (_connectionInfo.RDGatewayUsageMethod != RDGatewayUsageMethod.Never)
				{
					_rdpClient.TransportSettings.GatewayUsageMethod = (uint)_connectionInfo.RDGatewayUsageMethod;
					_rdpClient.TransportSettings.GatewayHostname = _connectionInfo.RDGatewayHostname;
					_rdpClient.TransportSettings.GatewayProfileUsageMethod = 1; // TSC_PROXY_PROFILE_MODE_EXPLICIT
					if (_connectionInfo.RDGatewayUseConnectionCredentials == RDGatewayUseConnectionCredentials.SmartCard)
					{
						_rdpClient.TransportSettings.GatewayCredsSource = 1; // TSC_PROXY_CREDS_MODE_SMARTCARD
					}
					if (_rdpVersion >= Versions.RDC61 && (Force & ConnectionInfo.Force.NoCredentials) != ConnectionInfo.Force.NoCredentials)
					{
						if (_connectionInfo.RDGatewayUseConnectionCredentials == RDGatewayUseConnectionCredentials.Yes)
						{
						    if (_connectionInfo.CredentialRecord != null)
						    {
                                _rdpClient.TransportSettings2.GatewayUsername = _connectionInfo.CredentialRecord.Username;
                                _rdpClient.TransportSettings2.GatewayPassword = _connectionInfo.CredentialRecord.Password.ConvertToUnsecureString();
                                _rdpClient.TransportSettings2.GatewayDomain = _connectionInfo.CredentialRecord.Domain;
                            }
						}
						else if (_connectionInfo.RDGatewayUseConnectionCredentials == RDGatewayUseConnectionCredentials.SmartCard)
						{
							_rdpClient.TransportSettings2.GatewayCredSharing = 0;
						}
						else
						{
                            _rdpClient.TransportSettings2.GatewayUsername = _connectionInfo.RDGatewayUsername;
                            _rdpClient.TransportSettings2.GatewayPassword = _connectionInfo.RDGatewayPassword;
                            _rdpClient.TransportSettings2.GatewayDomain = _connectionInfo.RDGatewayDomain;
                            _rdpClient.TransportSettings2.GatewayCredSharing = 0;
                        }
					}
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace(Language.strRdpSetGatewayFailed, ex);
			}
		}
				
		private void SetUseConsoleSession()
		{
			try
			{
				bool value;
						
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
					Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format(Language.strRdpSetConsoleSwitch, _rdpVersion), true);
					_rdpClient.AdvancedSettings7.ConnectToAdministerServer = value;
				}
				else
				{
					Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format(Language.strRdpSetConsoleSwitch, _rdpVersion) + Environment.NewLine + "No longer supported in this RDP version. Reference: https://msdn.microsoft.com/en-us/library/aa380863(v=vs.85).aspx", true);
                    // ConnectToServerConsole is deprecated
                    //https://msdn.microsoft.com/en-us/library/aa380863(v=vs.85).aspx
                    //_rdpClient.AdvancedSettings2.ConnectToServerConsole = value;
                }
            }
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace(Language.strRdpSetConsoleSessionFailed, ex);
			}
		}
				
		private void SetCredentials()
		{
			try
			{
				if ((Force & ConnectionInfo.Force.NoCredentials) == ConnectionInfo.Force.NoCredentials)
				{
					return;
				}

                var userName = _connectionInfo.CredentialRecord?.Username ?? "";
				var password = _connectionInfo.CredentialRecord?.Password ?? new SecureString();
				var domain = _connectionInfo.CredentialRecord?.Domain ?? "";
						
				if (string.IsNullOrEmpty(userName))
				{
					if (Settings.Default.EmptyCredentials == "windows")
					{
						_rdpClient.UserName = Environment.UserName;
					}
					else if (Settings.Default.EmptyCredentials == "custom")
					{
						_rdpClient.UserName = Settings.Default.DefaultUsername;
					}
				}
				else
				{
					_rdpClient.UserName = userName;
				}
						
				if (string.IsNullOrEmpty(password.ConvertToUnsecureString()))
				{
					if (Settings.Default.EmptyCredentials == "custom")
					{
						if (Settings.Default.DefaultPassword != "")
						{
                            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
                            _rdpClient.AdvancedSettings2.ClearTextPassword = cryptographyProvider.Decrypt(Settings.Default.DefaultPassword, Runtime.EncryptionKey);
						}
					}
				}
				else
				{
					_rdpClient.AdvancedSettings2.ClearTextPassword = password.ConvertToUnsecureString();
				}
						
				if (string.IsNullOrEmpty(domain))
				{
					if (Settings.Default.EmptyCredentials == "windows")
					{
						_rdpClient.Domain = Environment.UserDomainName;
					}
					else if (Settings.Default.EmptyCredentials == "custom")
					{
						_rdpClient.Domain = Settings.Default.DefaultDomain;
					}
				}
				else
				{
					_rdpClient.Domain = domain;
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace(Language.strRdpSetCredentialsFailed, ex);
			}
		}
				
		private void SetResolution()
		{
			try
			{
				if ((Force & ConnectionInfo.Force.Fullscreen) == ConnectionInfo.Force.Fullscreen)
				{
					_rdpClient.FullScreen = true;
                    _rdpClient.DesktopWidth = Screen.FromControl(FrmMain.Default).Bounds.Width;
                    _rdpClient.DesktopHeight = Screen.FromControl(FrmMain.Default).Bounds.Height;
							
					return;
				}
						
				if ((InterfaceControl.Info.Resolution == RDPResolutions.FitToWindow) || (InterfaceControl.Info.Resolution == RDPResolutions.SmartSize))
				{
					_rdpClient.DesktopWidth = InterfaceControl.Size.Width;
					_rdpClient.DesktopHeight = InterfaceControl.Size.Height;

				    if (InterfaceControl.Info.Resolution == RDPResolutions.SmartSize)
				    {
                        _rdpClient.AdvancedSettings2.SmartSizing = true;
                    }

                    
				}
				else if (InterfaceControl.Info.Resolution == RDPResolutions.Fullscreen)
				{
					_rdpClient.FullScreen = true;
                    _rdpClient.DesktopWidth = Screen.FromControl(FrmMain.Default).Bounds.Width;
                    _rdpClient.DesktopHeight = Screen.FromControl(FrmMain.Default).Bounds.Height;
				}
				else
				{
					var resolution = GetResolutionRectangle(_connectionInfo.Resolution);
					_rdpClient.DesktopWidth = resolution.Width;
					_rdpClient.DesktopHeight = resolution.Height;
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace(Language.strRdpSetResolutionFailed, ex);
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
				Runtime.MessageCollector.AddExceptionStackTrace(Language.strRdpSetPortFailed, ex);
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
				_rdpClient.SecuredSettings2.AudioRedirectionMode = (int)_connectionInfo.RedirectSound;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace(Language.strRdpSetRedirectionFailed, ex);
			}
		}
				
		private void SetPerformanceFlags()
		{
			try
			{
				var pFlags = 0;
				if (_connectionInfo.DisplayThemes == false)
				{
					pFlags += Convert.ToInt32(RDPPerformanceFlags.DisableThemes);
				}
						
				if (_connectionInfo.DisplayWallpaper == false)
				{
					pFlags += Convert.ToInt32(RDPPerformanceFlags.DisableWallpaper);
				}
						
				if (_connectionInfo.EnableFontSmoothing)
				{
					pFlags += Convert.ToInt32(RDPPerformanceFlags.EnableFontSmoothing);
				}
						
				if (_connectionInfo.EnableDesktopComposition)
				{
					pFlags += Convert.ToInt32(RDPPerformanceFlags.EnableDesktopComposition);
				}
						
				_rdpClient.AdvancedSettings2.PerformanceFlags = pFlags;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace(Language.strRdpSetPerformanceFlagsFailed, ex);
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
				Runtime.MessageCollector.AddExceptionStackTrace(Language.strRdpSetAuthenticationLevelFailed, ex);
			}
		}
				
		private void SetLoadBalanceInfo()
		{
			if (string.IsNullOrEmpty(_connectionInfo.LoadBalanceInfo))
			{
				return;
			}
			try
			{
				_rdpClient.AdvancedSettings2.LoadBalanceInfo = _connectionInfo.LoadBalanceInfo;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace("Unable to set load balance info.", ex);
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
                _rdpClient.OnIdleTimeoutNotification += RDPEvent_OnIdleTimeoutNotification;
            }
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace(Language.strRdpSetEventHandlersFailed, ex);
			}
		}
        #endregion
		
        #region Private Events & Handlers
        private void RDPEvent_OnIdleTimeoutNotification()
        {
            Close(); //Simply close the RDP Session if the idle timeout has been triggered.

            if (_alertOnIdleDisconnect)
            {
                string message = "The " + _connectionInfo.Name + " session was disconnected due to inactivity";
                const string caption = "Session Disconnected";
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void RDPEvent_OnFatalError(int errorCode)
		{
			Event_ErrorOccured(this, Convert.ToString(errorCode));
		}
				
		private void RDPEvent_OnDisconnected(int discReason)
		{
			const int UI_ERR_NORMAL_DISCONNECT = 0xB08;
			if (discReason != UI_ERR_NORMAL_DISCONNECT)
			{
				var reason = _rdpClient.GetErrorDescription((uint)discReason, (uint) _rdpClient.ExtendedDisconnectReason);
				Event_Disconnected(this, discReason + "\r\n" + reason);
			}
					
			if (Settings.Default.ReconnectOnDisconnect)
			{
				ReconnectGroup = new ReconnectGroup();
				ReconnectGroup.CloseClicked += Event_ReconnectGroupCloseClicked;
				ReconnectGroup.Left = (int) ((double) Control.Width / 2 - (double) ReconnectGroup.Width / 2);
				ReconnectGroup.Top = (int) ((double) Control.Height / 2 - (double) ReconnectGroup.Height / 2);
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
            LeaveFullscreenEvent?.Invoke(this, new EventArgs());
        }
        #endregion
		
        #region Public Events & Handlers
		public delegate void LeaveFullscreenEventHandler(object sender, EventArgs e);
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

	    public enum RDPSoundQuality
	    {
            [LocalizedAttributes.LocalizedDescription("strRDPSoundQualityDynamic")]
            Dynamic = 0,
            [LocalizedAttributes.LocalizedDescription("strRDPSoundQualityMedium")]
            Medium = 1,
            [LocalizedAttributes.LocalizedDescription("strRDPSoundQualityHigh")]
            High = 2
        }


        private enum RDPPerformanceFlags
		{
			[Description("strRDPDisableWallpaper")]DisableWallpaper = 0x1,
//			[Description("strRDPDisableFullWindowdrag")]DisableFullWindowDrag = 0x2,
//			[Description("strRDPDisableMenuAnimations")]DisableMenuAnimations = 0x4,
			[Description("strRDPDisableThemes")]DisableThemes = 0x8,
//			[Description("strRDPDisableCursorShadow")]DisableCursorShadow = 0x20,
//			[Description("strRDPDisableCursorblinking")]DisableCursorBlinking = 0x40,
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
            [Description("1366x768")]
            Res1366x768,
            [Description("1440x900")]
            Res1440x900,
            [Description("1600x900")]
            Res1600x900,
            [Description("1600x1200")]
            Res1600x1200,
            [Description("1680x1050")]
            Res1680x1050,
            [Description("1920x1080")]
            Res1920x1080,
            [Description("1920x1200")]
            Res1920x1200,
            [Description("2048x1536")]
            Res2048x1536,
            [Description("2560x1440")]
            Res2560x1440,
            [Description("2560x1600")]
            Res2560x1600,
            [Description("2560x2048")]
            Res2560x2048,
            [Description("3840x2160")]
            Res3840x2160
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
			if (resolution != RDPResolutions.FitToWindow & resolution != RDPResolutions.Fullscreen & resolution != RDPResolutions.SmartSize)
			{
				resolutionParts = resolution.ToString().Replace("Res", "").Split('x');
			}
			if (resolutionParts == null || resolutionParts.Length != 2)
			{
				return new Rectangle(0, 0, 0, 0);
			}
			else
			{
                return new Rectangle(0, 0, Convert.ToInt32(resolutionParts[0]), Convert.ToInt32(resolutionParts[1]));
			}
		}
        #endregion

        public static class Versions
		{
			public static readonly Version RDC60 = new Version(6, 0, 6000);
			public static readonly Version RDC61 = new Version(6, 0, 6001);
			public static readonly Version RDC70 = new Version(6, 1, 7600);
			public static readonly Version RDC80 = new Version(6, 2, 9200);
            public static readonly Version RDC81 = new Version(6, 3, 9600);
		}
		
        #region Fatal Errors
		public static class FatalErrors
		{
		    private static Hashtable _description;

		    private static void InitDescription()
            {
                _description = new Hashtable
                {
                    {"0", "Language.strRdpErrorUnknown"},
                    {"1", "Language.strRdpErrorCode1"},
                    {"2", "Language.strRdpErrorOutOfMemory"},
                    {"3", "Language.strRdpErrorWindowCreation"},
                    {"4", "Language.strRdpErrorCode2"},
                    {"5", "Language.strRdpErrorCode3"},
                    {"6", "Language.strRdpErrorCode4"},
                    {"7", "Language.strRdpErrorConnection"},
                    {"100", "Language.strRdpErrorWinsock"}
                };
            }
					
			public static string GetError(string id)
			{
				try
				{
				    if (_description == null)
                        InitDescription();

				    return (string)_description?[id];
				}
				catch (Exception ex)
				{
					Runtime.MessageCollector.AddExceptionStackTrace(Language.strRdpErrorGetFailure, ex);
					return string.Format(Language.strRdpErrorUnknown, id);
				}
			}
		}
        #endregion
		
        #region Reconnect Stuff
		public void tmrReconnect_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			var srvReady = PortScanner.IsPortOpen(_connectionInfo.Hostname, Convert.ToString(_connectionInfo.Port));
					
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
