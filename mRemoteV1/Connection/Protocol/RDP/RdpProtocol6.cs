using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using AxMSTSCLib;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools;
using mRemoteNG.UI.Forms;
using MSTSCLib;

namespace mRemoteNG.Connection.Protocol.RDP
{
	public class RdpProtocol6 : ProtocolBase
	{
		private MsRdpClient6NotSafeForScripting _rdpClient;
        private Version _rdpVersion;
        private bool _redirectKeys;
        private bool _alertOnIdleDisconnect;
        private readonly FrmMain _frmMain = FrmMain.Default;
        protected bool LoginComplete;
        protected ConnectionInfo ConnectionInfo;

        #region Properties
        public virtual bool SmartSize
		{
			get
			{
				return _rdpClient.AdvancedSettings2.SmartSizing;
			}
            protected set
			{
				_rdpClient.AdvancedSettings2.SmartSizing = value;
			}
		}
		
        public virtual bool Fullscreen
		{
	        get
	        {
				return _rdpClient.FullScreen;
	        }
	        protected set
	        {
				_rdpClient.FullScreen = value;
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

        public bool LoadBalanceInfoUseUtf8 { get; set; }
        #endregion

        #region Constructors
        public RdpProtocol6()
        {
            Control = new AxMsRdpClient6NotSafeForScripting();
	        Connecting += OnConnectingDebugMessage;
        }

		private void OnConnectingDebugMessage(object sender)
		{
			Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, $"Using RDP version: {ConnectionInfo.RdpProtocolVersion}");
		}

		#endregion

        #region Public Methods
		public override bool Initialize()
		{
			base.Initialize();
			try
			{
				Control.CreateControl();
				ConnectionInfo = InterfaceControl.Info;
				
				try
				{
					while (!Control.Created)
					{
						Thread.Sleep(0);
                        Application.DoEvents();
					}
                    _rdpClient = (MsRdpClient6NotSafeForScripting)CreateRdpClientControl();
				}
				catch (System.Runtime.InteropServices.COMException ex)
				{
					Runtime.MessageCollector.AddExceptionMessage(Language.strRdpControlCreationFailed, ex);
					Control.Dispose();
					return false;
				}
						
				_rdpVersion = new Version(_rdpClient.Version);
						
				_rdpClient.Server = ConnectionInfo.Hostname;

                SetCredentials();
                SetResolution();
                _rdpClient.FullScreenTitle = ConnectionInfo.Name;

                _alertOnIdleDisconnect = ConnectionInfo.RDPAlertIdleTimeout;
                _rdpClient.AdvancedSettings2.MinutesToIdleTimeout = ConnectionInfo.RDPMinutesToIdleTimeout;

                //not user changeable
                _rdpClient.AdvancedSettings2.GrabFocusOnConnect = true;
				_rdpClient.AdvancedSettings3.EnableAutoReconnect = true;
				_rdpClient.AdvancedSettings3.MaxReconnectAttempts = Settings.Default.RdpReconnectionCount;
				_rdpClient.AdvancedSettings2.keepAliveInterval = 60000; //in milliseconds (10,000 = 10 seconds)
				_rdpClient.AdvancedSettings5.AuthenticationLevel = 0;
				_rdpClient.AdvancedSettings2.EncryptionEnabled = 1;
						
				_rdpClient.AdvancedSettings2.overallConnectionTimeout = Settings.Default.ConRDPOverallConnectionTimeout;
						
				_rdpClient.AdvancedSettings2.BitmapPeristence = Convert.ToInt32(ConnectionInfo.CacheBitmaps);
				if (_rdpVersion >= RdpVersion.RDC61)
				{
					_rdpClient.AdvancedSettings7.EnableCredSspSupport = ConnectionInfo.UseCredSsp;
				}

                SetUseConsoleSession();
                SetPort();
				RedirectKeys = ConnectionInfo.RedirectKeys;
                SetRedirection();
                SetAuthenticationLevel();
				SetLoadBalanceInfo();
                SetRdGateway();
						
				_rdpClient.ColorDepth = (int)ConnectionInfo.Colors;

                SetPerformanceFlags();
						
				_rdpClient.ConnectingText = Language.strConnecting;
						
				Control.Anchor = AnchorStyles.None;
				tmrReconnect.Elapsed += tmrReconnect_Elapsed;

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
			LoginComplete = false;
			SetEventHandlers();

            try
			{
				_rdpClient.Connect();
				base.Connect();
				return true;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace(Language.strConnectionOpenFailed, ex);
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
		#endregion

		#region Private Methods
		protected virtual object CreateRdpClientControl()
		{
			return ((AxMsRdpClient6NotSafeForScripting)Control).GetOcx();
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

			    if (ConnectionInfo.RDGatewayUsageMethod != RDGatewayUsageMethod.Never)
				{
					_rdpClient.TransportSettings.GatewayUsageMethod = (uint)ConnectionInfo.RDGatewayUsageMethod;
					_rdpClient.TransportSettings.GatewayHostname = ConnectionInfo.RDGatewayHostname;
					_rdpClient.TransportSettings.GatewayProfileUsageMethod = 1; // TSC_PROXY_PROFILE_MODE_EXPLICIT
					if (ConnectionInfo.RDGatewayUseConnectionCredentials == RDGatewayUseConnectionCredentials.SmartCard)
					{
						_rdpClient.TransportSettings.GatewayCredsSource = 1; // TSC_PROXY_CREDS_MODE_SMARTCARD
					}
					if (_rdpVersion >= RdpVersion.RDC61 && (Force & ConnectionInfo.Force.NoCredentials) != ConnectionInfo.Force.NoCredentials)
					{
						if (ConnectionInfo.RDGatewayUseConnectionCredentials == RDGatewayUseConnectionCredentials.Yes)
						{
						    _rdpClient.TransportSettings2.GatewayUsername = ConnectionInfo.Username;
						    _rdpClient.TransportSettings2.GatewayPassword = ConnectionInfo.Password;
						    _rdpClient.TransportSettings2.GatewayDomain = ConnectionInfo?.Domain;
						}
						else if (ConnectionInfo.RDGatewayUseConnectionCredentials == RDGatewayUseConnectionCredentials.SmartCard)
						{
							_rdpClient.TransportSettings2.GatewayCredSharing = 0;
						}
						else
						{
                            _rdpClient.TransportSettings2.GatewayUsername = ConnectionInfo.RDGatewayUsername;
                            _rdpClient.TransportSettings2.GatewayPassword = ConnectionInfo.RDGatewayPassword;
                            _rdpClient.TransportSettings2.GatewayDomain = ConnectionInfo.RDGatewayDomain;
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
					value = ConnectionInfo.UseConsoleSession;
				}
						
				if (_rdpVersion >= RdpVersion.RDC61)
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

			    var userName = ConnectionInfo?.Username ?? "";
			    var password = ConnectionInfo?.Password ?? "";
			    var domain = ConnectionInfo?.Domain ?? "";
						
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
						
				if (string.IsNullOrEmpty(password))
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
					_rdpClient.AdvancedSettings2.ClearTextPassword = password;
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
                    _rdpClient.DesktopWidth = Screen.FromControl(_frmMain).Bounds.Width;
                    _rdpClient.DesktopHeight = Screen.FromControl(_frmMain).Bounds.Height;
							
					return;
				}
						
				if ((InterfaceControl.Info.Resolution == RdpResolutions.FitToWindow) || (InterfaceControl.Info.Resolution == RdpResolutions.SmartSize))
				{
					_rdpClient.DesktopWidth = InterfaceControl.Size.Width;
					_rdpClient.DesktopHeight = InterfaceControl.Size.Height;

				    if (InterfaceControl.Info.Resolution == RdpResolutions.SmartSize)
				    {
                        _rdpClient.AdvancedSettings2.SmartSizing = true;
                    }

                    
				}
				else if (InterfaceControl.Info.Resolution == RdpResolutions.Fullscreen)
				{
					_rdpClient.FullScreen = true;
                    _rdpClient.DesktopWidth = Screen.FromControl(_frmMain).Bounds.Width;
                    _rdpClient.DesktopHeight = Screen.FromControl(_frmMain).Bounds.Height;
				}
				else
				{
					var resolution = ConnectionInfo.Resolution.GetResolutionRectangle();
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
				if (ConnectionInfo.Port != (int)Defaults.Port)
				{
					_rdpClient.AdvancedSettings2.RDPPort = ConnectionInfo.Port;
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
				_rdpClient.AdvancedSettings2.RedirectDrives = ConnectionInfo.RedirectDiskDrives;
				_rdpClient.AdvancedSettings2.RedirectPorts = ConnectionInfo.RedirectPorts;
				_rdpClient.AdvancedSettings2.RedirectPrinters = ConnectionInfo.RedirectPrinters;
				_rdpClient.AdvancedSettings2.RedirectSmartCards = ConnectionInfo.RedirectSmartCards;
				_rdpClient.SecuredSettings2.AudioRedirectionMode = (int)ConnectionInfo.RedirectSound;
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
				if (ConnectionInfo.DisplayThemes == false)
				{
					pFlags += Convert.ToInt32(RdpPerformanceFlags.DisableThemes);
				}
						
				if (ConnectionInfo.DisplayWallpaper == false)
				{
					pFlags += Convert.ToInt32(RdpPerformanceFlags.DisableWallpaper);
				}
						
				if (ConnectionInfo.EnableFontSmoothing)
				{
					pFlags += Convert.ToInt32(RdpPerformanceFlags.EnableFontSmoothing);
				}
						
				if (ConnectionInfo.EnableDesktopComposition)
				{
					pFlags += Convert.ToInt32(RdpPerformanceFlags.EnableDesktopComposition);
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
				_rdpClient.AdvancedSettings5.AuthenticationLevel = (uint)ConnectionInfo.RDPAuthenticationLevel;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace(Language.strRdpSetAuthenticationLevelFailed, ex);
			}
		}
				
		private void SetLoadBalanceInfo()
		{
			if (string.IsNullOrEmpty(ConnectionInfo.LoadBalanceInfo))
			{
				return;
			}
			try
			{
			    _rdpClient.AdvancedSettings2.LoadBalanceInfo = LoadBalanceInfoUseUtf8
                    ? new AzureLoadBalanceInfoEncoder().Encode(ConnectionInfo.LoadBalanceInfo) 
                    : ConnectionInfo.LoadBalanceInfo;
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
                string message = "The " + ConnectionInfo.Name + " session was disconnected due to inactivity";
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
			LoginComplete = true;
		}
				
		private void RDPEvent_OnLeaveFullscreenMode()
		{
			Fullscreen = false;
			LeaveFullscreen?.Invoke(this, new EventArgs());
        }
        #endregion
		
		public delegate void LeaveFullscreenEventHandler(object sender, EventArgs e);
		public event LeaveFullscreenEventHandler LeaveFullscreen;
		
		public enum Defaults
		{
			Colors = RdpColors.Colors16Bit,
			Sounds = RdpSounds.DoNotPlay,
			Resolution = RdpResolutions.FitToWindow,
			Port = 3389
		}
		
        #region Reconnect Stuff
		public void tmrReconnect_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			var srvReady = PortScanner.IsPortOpen(ConnectionInfo.Hostname, Convert.ToString(ConnectionInfo.Port));
					
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
