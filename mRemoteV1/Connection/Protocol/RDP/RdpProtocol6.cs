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
        private Version _rdpVersion;
        private bool _redirectKeys;
        private bool _alertOnIdleDisconnect;
        private readonly FrmMain _frmMain = FrmMain.Default;
		protected MsRdpClient6NotSafeForScripting RdpClient;
        protected bool LoginComplete;
        protected ConnectionInfo ConnectionInfo;

        #region Properties
        public virtual bool SmartSize
		{
			get
			{
				return RdpClient.AdvancedSettings2.SmartSizing;
			}
            protected set
			{
				RdpClient.AdvancedSettings2.SmartSizing = value;
			}
		}
		
        public virtual bool Fullscreen
		{
	        get
	        {
				return RdpClient.FullScreen;
	        }
	        protected set
	        {
				RdpClient.FullScreen = value;
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
							
					Debug.Assert(Convert.ToBoolean(RdpClient.SecuredSettingsEnabled));
                    var msRdpClientSecuredSettings = RdpClient.SecuredSettings2;
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

        public RdpProtocol6()
        {
            Control = new AxMsRdpClient6NotSafeForScripting();
	        Connecting += OnConnectingDebugMessage;
        }

		

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
				    RdpClient = CreateRdpClientControl();
				}
				catch (System.Runtime.InteropServices.COMException ex)
				{
					Runtime.MessageCollector.AddExceptionMessage(Language.strRdpControlCreationFailed, ex);
					Control.Dispose();
					return false;
				}
						
				_rdpVersion = new Version(RdpClient.Version);
						
				RdpClient.Server = ConnectionInfo.Hostname;

                SetCredentials();
                SetResolution();
                RdpClient.FullScreenTitle = ConnectionInfo.Name;

                _alertOnIdleDisconnect = ConnectionInfo.RDPAlertIdleTimeout;
                RdpClient.AdvancedSettings2.MinutesToIdleTimeout = ConnectionInfo.RDPMinutesToIdleTimeout;

                //not user changeable
                RdpClient.AdvancedSettings2.GrabFocusOnConnect = true;
				RdpClient.AdvancedSettings3.EnableAutoReconnect = true;
				RdpClient.AdvancedSettings3.MaxReconnectAttempts = Settings.Default.RdpReconnectionCount;
				RdpClient.AdvancedSettings2.keepAliveInterval = 60000; //in milliseconds (10,000 = 10 seconds)
				RdpClient.AdvancedSettings5.AuthenticationLevel = 0;
				RdpClient.AdvancedSettings2.EncryptionEnabled = 1;
						
				RdpClient.AdvancedSettings2.overallConnectionTimeout = Settings.Default.ConRDPOverallConnectionTimeout;
						
				RdpClient.AdvancedSettings2.BitmapPeristence = Convert.ToInt32(ConnectionInfo.CacheBitmaps);
				if (_rdpVersion >= RdpVersion.RDC61)
				{
					RdpClient.AdvancedSettings7.EnableCredSspSupport = ConnectionInfo.UseCredSsp;
				}

                SetUseConsoleSession();
                SetPort();
				RedirectKeys = ConnectionInfo.RedirectKeys;
                SetRedirection();
                SetAuthenticationLevel();
				SetLoadBalanceInfo();
                SetRdGateway();
						
				RdpClient.ColorDepth = (int)ConnectionInfo.Colors;

                SetPerformanceFlags();
						
				RdpClient.ConnectingText = Language.strConnecting;
						
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
				RdpClient.Connect();
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
				RdpClient.Disconnect();
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
		protected virtual MsRdpClient6NotSafeForScripting CreateRdpClientControl()
		{
			return (MsRdpClient6NotSafeForScripting)((AxMsRdpClient6NotSafeForScripting)Control).GetOcx();
		}

		private void SetRdGateway()
		{
			try
			{
				if (RdpClient.TransportSettings.GatewayIsSupported == 0)
				{
					Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.strRdpGatewayNotSupported, true);
					return;
				}
			    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.strRdpGatewayIsSupported, true);

			    if (ConnectionInfo.RDGatewayUsageMethod != RDGatewayUsageMethod.Never)
				{
					RdpClient.TransportSettings.GatewayUsageMethod = (uint)ConnectionInfo.RDGatewayUsageMethod;
					RdpClient.TransportSettings.GatewayHostname = ConnectionInfo.RDGatewayHostname;
					RdpClient.TransportSettings.GatewayProfileUsageMethod = 1; // TSC_PROXY_PROFILE_MODE_EXPLICIT
					if (ConnectionInfo.RDGatewayUseConnectionCredentials == RDGatewayUseConnectionCredentials.SmartCard)
					{
						RdpClient.TransportSettings.GatewayCredsSource = 1; // TSC_PROXY_CREDS_MODE_SMARTCARD
					}
					if (_rdpVersion >= RdpVersion.RDC61 && (Force & ConnectionInfo.Force.NoCredentials) != ConnectionInfo.Force.NoCredentials)
					{
						if (ConnectionInfo.RDGatewayUseConnectionCredentials == RDGatewayUseConnectionCredentials.Yes)
						{
						    RdpClient.TransportSettings2.GatewayUsername = ConnectionInfo.Username;
						    RdpClient.TransportSettings2.GatewayPassword = ConnectionInfo.Password;
						    RdpClient.TransportSettings2.GatewayDomain = ConnectionInfo?.Domain;
						}
						else if (ConnectionInfo.RDGatewayUseConnectionCredentials == RDGatewayUseConnectionCredentials.SmartCard)
						{
							RdpClient.TransportSettings2.GatewayCredSharing = 0;
						}
						else
						{
                            RdpClient.TransportSettings2.GatewayUsername = ConnectionInfo.RDGatewayUsername;
                            RdpClient.TransportSettings2.GatewayPassword = ConnectionInfo.RDGatewayPassword;
                            RdpClient.TransportSettings2.GatewayDomain = ConnectionInfo.RDGatewayDomain;
                            RdpClient.TransportSettings2.GatewayCredSharing = 0;
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
					RdpClient.AdvancedSettings7.ConnectToAdministerServer = value;
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
						RdpClient.UserName = Environment.UserName;
					}
					else if (Settings.Default.EmptyCredentials == "custom")
					{
						RdpClient.UserName = Settings.Default.DefaultUsername;
					}
				}
				else
				{
					RdpClient.UserName = userName;
				}
						
				if (string.IsNullOrEmpty(password))
				{
					if (Settings.Default.EmptyCredentials == "custom")
					{
						if (Settings.Default.DefaultPassword != "")
						{
                            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
                            RdpClient.AdvancedSettings2.ClearTextPassword = cryptographyProvider.Decrypt(Settings.Default.DefaultPassword, Runtime.EncryptionKey);
						}
					}
				}
				else
				{
					RdpClient.AdvancedSettings2.ClearTextPassword = password;
				}
						
				if (string.IsNullOrEmpty(domain))
				{
					if (Settings.Default.EmptyCredentials == "windows")
					{
						RdpClient.Domain = Environment.UserDomainName;
					}
					else if (Settings.Default.EmptyCredentials == "custom")
					{
						RdpClient.Domain = Settings.Default.DefaultDomain;
					}
				}
				else
				{
					RdpClient.Domain = domain;
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
					RdpClient.FullScreen = true;
                    RdpClient.DesktopWidth = Screen.FromControl(_frmMain).Bounds.Width;
                    RdpClient.DesktopHeight = Screen.FromControl(_frmMain).Bounds.Height;
							
					return;
				}
						
				if ((InterfaceControl.Info.Resolution == RdpResolutions.FitToWindow) || (InterfaceControl.Info.Resolution == RdpResolutions.SmartSize))
				{
					RdpClient.DesktopWidth = InterfaceControl.Size.Width;
					RdpClient.DesktopHeight = InterfaceControl.Size.Height;

				    if (InterfaceControl.Info.Resolution == RdpResolutions.SmartSize)
				    {
                        RdpClient.AdvancedSettings2.SmartSizing = true;
                    }

                    
				}
				else if (InterfaceControl.Info.Resolution == RdpResolutions.Fullscreen)
				{
					RdpClient.FullScreen = true;
                    RdpClient.DesktopWidth = Screen.FromControl(_frmMain).Bounds.Width;
                    RdpClient.DesktopHeight = Screen.FromControl(_frmMain).Bounds.Height;
				}
				else
				{
					var resolution = ConnectionInfo.Resolution.GetResolutionRectangle();
					RdpClient.DesktopWidth = resolution.Width;
					RdpClient.DesktopHeight = resolution.Height;
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
					RdpClient.AdvancedSettings2.RDPPort = ConnectionInfo.Port;
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
				RdpClient.AdvancedSettings2.RedirectDrives = ConnectionInfo.RedirectDiskDrives;
				RdpClient.AdvancedSettings2.RedirectPorts = ConnectionInfo.RedirectPorts;
				RdpClient.AdvancedSettings2.RedirectPrinters = ConnectionInfo.RedirectPrinters;
				RdpClient.AdvancedSettings2.RedirectSmartCards = ConnectionInfo.RedirectSmartCards;
				RdpClient.SecuredSettings2.AudioRedirectionMode = (int)ConnectionInfo.RedirectSound;
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
						
				RdpClient.AdvancedSettings2.PerformanceFlags = pFlags;
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
				RdpClient.AdvancedSettings5.AuthenticationLevel = (uint)ConnectionInfo.RDPAuthenticationLevel;
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
			    RdpClient.AdvancedSettings2.LoadBalanceInfo = LoadBalanceInfoUseUtf8
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
				RdpClient.OnConnecting += RDPEvent_OnConnecting;
				RdpClient.OnConnected += RDPEvent_OnConnected;
				RdpClient.OnLoginComplete += RDPEvent_OnLoginComplete;
				RdpClient.OnFatalError += RDPEvent_OnFatalError;
				RdpClient.OnDisconnected += RDPEvent_OnDisconnected;
				RdpClient.OnLeaveFullScreenMode += RDPEvent_OnLeaveFullscreenMode;
                RdpClient.OnIdleTimeoutNotification += RDPEvent_OnIdleTimeoutNotification;
            }
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace(Language.strRdpSetEventHandlersFailed, ex);
			}
		}

	    private void OnConnectingDebugMessage(object sender, EventArgs args)
	    {
	        Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, $"Using RDP version: {ConnectionInfo.RdpProtocolVersion}");
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
			RaiseErrorOccuredEvent(this, Convert.ToString(errorCode));
		}
				
		private void RDPEvent_OnDisconnected(int discReason)
		{
			const int UI_ERR_NORMAL_DISCONNECT = 0xB08;
			if (discReason != UI_ERR_NORMAL_DISCONNECT)
			{
				var reason = RdpClient.GetErrorDescription((uint)discReason, (uint) RdpClient.ExtendedDisconnectReason);
				RaiseConnectionDisconnectedEvent(this, discReason + "\r\n" + reason);
			}
					
			if (Settings.Default.ReconnectOnDisconnect)
			{
				ReconnectGroup = new ReconnectGroup();
				ReconnectGroup.CloseClicked += Close;
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
			RaiseConnectionConnectingEvent(this);
		}
				
		private void RDPEvent_OnConnected()
		{
			RaiseConnectionConnectedEvent(this);
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
				RdpClient.Connect();
			}
		}
        #endregion
	}
}
