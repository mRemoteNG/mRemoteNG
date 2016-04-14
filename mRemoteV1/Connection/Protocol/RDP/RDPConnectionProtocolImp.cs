using System;
using System.Drawing;
using System.Diagnostics;
using AxMSTSCLib;
using System.Collections;
using System.Windows.Forms;
using System.Threading;
using EOLWTSCOM;
using System.ComponentModel;
using mRemoteNG.Messages;
using mRemoteNG.App;
using MSTSCLib;
using mRemoteNG.Tools;
using mRemoteNG.Connection.Protocol.RDP;


namespace mRemoteNG.Connection.Protocol
{
    public class RDPConnectionProtocolImp : RDPConnectionProtocol
	{
        #region Private Declarations
        private MsRdpClient6NotSafeForScripting _rdpClient;
        private Version _rdpVersion;
        private ConnectionRecordImp _connectionInfo;
        private bool _loginComplete;
        private Size _controlBeginningSize = new Size();
        private bool _redirectKeys = false;
        private Control _Control;
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
							
					Debug.Assert(System.Convert.ToBoolean(_rdpClient.SecuredSettingsEnabled));
					MSTSCLib.IMsRdpClientSecuredSettings msRdpClientSecuredSettings = _rdpClient.SecuredSettings2;
					msRdpClientSecuredSettings.KeyboardHookMode = 1; // Apply key combinations at the remote server.
				}
				catch (Exception ex)
				{
					Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strRdpSetRedirectKeysFailed + Environment.NewLine + ex.Message, true);
				}
			}
		}
        #endregion
		
        #region Public Methods
		public RDPConnectionProtocolImp()
		{
            _rdpClient = new MsRdpClient6NotSafeForScriptingClass();
            _rdpVersion = new Version(_rdpClient.Version);

		}
		
		public override bool SetProps()
		{
            try
            {
                this._interfaceControl.Parent.Tag = this._interfaceControl;
                this._interfaceControl.Show();

                if (this._Control != null)
                {
                    this._Control.Name = this._Name;
                    this._Control.Parent = this._interfaceControl;
                    this._Control.Location = this._interfaceControl.Location;
                    this._Control.Size = this.InterfaceControl.Size;
                    this._Control.Anchor = this._interfaceControl.Anchor;
                }

                return true;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn\'t SetProps (Connection.Protocol.Base)" + Environment.NewLine + ex.Message, true);
                return false;
            }
					
			try
			{
				Control.CreateControl();
				_connectionInfo = InterfaceControl.Info;
						
				try
				{
					while (!Control.Created)
					{
						Thread.Sleep(0);
						System.Windows.Forms.Application.DoEvents();
					}

                    _rdpClient = (MsRdpClient6NotSafeForScripting)((AxMsRdpClient6NotSafeForScripting)Control).GetOcx();
				}
				catch (System.Runtime.InteropServices.COMException ex)
				{
					Runtime.MessageCollector.AddExceptionMessage(My.Language.strRdpControlCreationFailed, ex);
					Control.Dispose();
					return false;
				}
						
				this._rdpVersion = new Version(_rdpClient.Version);
						
				this._rdpClient.Server = this._connectionInfo.Hostname;
						
				this.SetCredentials();
				this.SetResolution();
				this._rdpClient.FullScreenTitle = this._connectionInfo.Name;
						
				//not user changeable
				_rdpClient.AdvancedSettings2.GrabFocusOnConnect = true;
				_rdpClient.AdvancedSettings3.EnableAutoReconnect = true;
				_rdpClient.AdvancedSettings3.MaxReconnectAttempts = System.Convert.ToInt32(My.Settings.Default.RdpReconnectionCount);
				_rdpClient.AdvancedSettings2.keepAliveInterval = 60000; //in milliseconds (10.000 = 10 seconds)
				_rdpClient.AdvancedSettings5.AuthenticationLevel = 0;
				_rdpClient.AdvancedSettings2.EncryptionEnabled = 1;
				_rdpClient.AdvancedSettings2.overallConnectionTimeout = 20;
				_rdpClient.AdvancedSettings2.BitmapPeristence = System.Convert.ToInt32(this._connectionInfo.CacheBitmaps);

				if (_rdpVersion >= Versions.RDC61)
				{
					_rdpClient.AdvancedSettings7.EnableCredSspSupport = _connectionInfo.UseCredSsp;
				}
						
				this.SetUseConsoleSession();
				this.SetPort();
				this.RedirectKeys = _connectionInfo.RedirectKeys;
				this.SetRedirection();
				this.SetAuthenticationLevel();
				this.SetLoadBalanceInfo();
				this.SetRdGateway();
				this._rdpClient.ColorDepth = System.Convert.ToInt32(Conversion.Int(this._connectionInfo.Colors));
				this.SetPerformanceFlags();
				this._rdpClient.ConnectingText = My.Language.strConnecting;
				base.Control.Anchor = AnchorStyles.None;
						
				return true;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strRdpSetPropsFailed + Environment.NewLine + ex.Message, true);
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
                if (ConnectedEvent != null)
                {
                    ConnectedEvent(this);
                    return true;
                }
				return true;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strRdpConnectionOpenFailed + Environment.NewLine + ex.Message);
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
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strRdpDisconnectFailed + Environment.NewLine + ex.Message, true);
				base.Close();
			}
		}
		
		public void ToggleFullscreen()
		{
			try
			{
				this.Fullscreen = !this.Fullscreen;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strRdpToggleFullscreenFailed + Environment.NewLine + ex.Message, true);
			}
		}
		
		public void ToggleSmartSize()
		{
			try
			{
				this.SmartSize = !this.SmartSize;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strRdpToggleSmartSizeFailed + Environment.NewLine + ex.Message, true);
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
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strRdpFocusFailed + Environment.NewLine + ex.Message, true);
			}
		}
		
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

            IMsRdpClient8 msRdpClient8 = (IMsRdpClient8)_rdpClient;
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
						
				if (!(_connectionInfo.RDGatewayUsageMethod == RDGatewayUsageMethod.Never))
				{
					_rdpClient.TransportSettings.GatewayUsageMethod = (uint)_connectionInfo.RDGatewayUsageMethod;
					_rdpClient.TransportSettings.GatewayHostname = _connectionInfo.RDGatewayHostname;
					_rdpClient.TransportSettings.GatewayProfileUsageMethod = 1; // TSC_PROXY_PROFILE_MODE_EXPLICIT
					if (_connectionInfo.RDGatewayUseConnectionCredentials == RDGatewayUseConnectionCredentials.SmartCard)
					{
						_rdpClient.TransportSettings.GatewayCredsSource = 1; // TSC_PROXY_CREDS_MODE_SMARTCARD
					}
					if (_rdpVersion >= Versions.RDC61 && !((Force & ConnectionRecordImp.Force.NoCredentials) == ConnectionRecordImp.Force.NoCredentials))
					{
						if (_connectionInfo.RDGatewayUseConnectionCredentials == RDGatewayUseConnectionCredentials.Yes)
						{
							_rdpClient.TransportSettings2.GatewayUsername = _connectionInfo.Username;
							_rdpClient.TransportSettings2.GatewayPassword = _connectionInfo.Password;
							_rdpClient.TransportSettings2.GatewayDomain = _connectionInfo.Domain;
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
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strRdpSetGatewayFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void SetUseConsoleSession()
		{
			try
			{
				bool value = false;
						
				if ((Force & ConnectionRecordImp.Force.UseConsoleSession) == ConnectionRecordImp.Force.UseConsoleSession)
				{
					value = true;
				}
				else if ((Force & ConnectionRecordImp.Force.DontUseConsoleSession) == ConnectionRecordImp.Force.DontUseConsoleSession)
				{
					value = false;
				}
				else
				{
					value = _connectionInfo.UseConsoleSession;
				}
						
				if (_rdpVersion >= Versions.RDC61)
				{
					Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format(My.Language.strRdpSetConsoleSwitch, "6.1"), true);
					_rdpClient.AdvancedSettings7.ConnectToAdministerServer = value;
				}
				else
				{
					Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format(My.Language.strRdpSetConsoleSwitch, "6.0"), true);
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
				if ((Force & ConnectionRecordImp.Force.NoCredentials) == ConnectionRecordImp.Force.NoCredentials)
				{
					return ;
				}
						
				string userName = _connectionInfo.Username;
				string password = _connectionInfo.Password;
				string domain = _connectionInfo.Domain;
						
				if (string.IsNullOrEmpty(userName))
				{
					if ((string) My.Settings.Default.EmptyCredentials == "windows")
					{
						_rdpClient.UserName = Environment.UserName;
					}
					else if ((string) My.Settings.Default.EmptyCredentials == "custom")
					{
						_rdpClient.UserName = System.Convert.ToString(My.Settings.Default.DefaultUsername);
					}
				}
				else
				{
					_rdpClient.UserName = userName;
				}
						
				if (string.IsNullOrEmpty(password))
				{
					if ((string) My.Settings.Default.EmptyCredentials == "custom")
					{
						if (My.Settings.Default.DefaultPassword != "")
						{
							_rdpClient.AdvancedSettings2.ClearTextPassword = Security.Crypt.Decrypt(System.Convert.ToString(My.Settings.Default.DefaultPassword), App.Info.General.EncryptionKey);
						}
					}
				}
				else
				{
					_rdpClient.AdvancedSettings2.ClearTextPassword = password;
				}
						
				if (string.IsNullOrEmpty(domain))
				{
					if ((string) My.Settings.Default.EmptyCredentials == "windows")
					{
						_rdpClient.Domain = Environment.UserDomainName;
					}
					else if ((string) My.Settings.Default.EmptyCredentials == "custom")
					{
						_rdpClient.Domain = System.Convert.ToString(My.Settings.Default.DefaultDomain);
					}
				}
				else
				{
					_rdpClient.Domain = domain;
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strRdpSetCredentialsFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void SetResolution()
		{
			try
			{
				if ((this.Force & Connection.ConnectionRecordImp.Force.Fullscreen) == Connection.ConnectionRecordImp.Force.Fullscreen)
				{
					_rdpClient.FullScreen = true;
                    _rdpClient.DesktopWidth = Screen.FromControl(frmMain.Default).Bounds.Width;
                    _rdpClient.DesktopHeight = Screen.FromControl(frmMain.Default).Bounds.Height;
							
					return;
				}
						
				if ((this.InterfaceControl.Info.Resolution == RDPResolutions.FitToWindow) || (this.InterfaceControl.Info.Resolution == RDPResolutions.SmartSize))
				{
					_rdpClient.DesktopWidth = InterfaceControl.Size.Width;
					_rdpClient.DesktopHeight = InterfaceControl.Size.Height;
				}
				else if (this.InterfaceControl.Info.Resolution == RDPResolutions.Fullscreen)
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
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strRdpSetResolutionFailed + Environment.NewLine + ex.Message, true);
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
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strRdpSetPortFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void SetRedirection()
		{
			try
			{
				_rdpClient.AdvancedSettings2.RedirectDrives = this._connectionInfo.RedirectDiskDrives;
				_rdpClient.AdvancedSettings2.RedirectPorts = this._connectionInfo.RedirectPorts;
				_rdpClient.AdvancedSettings2.RedirectPrinters = this._connectionInfo.RedirectPrinters;
				_rdpClient.AdvancedSettings2.RedirectSmartCards = this._connectionInfo.RedirectSmartCards;
				_rdpClient.SecuredSettings2.AudioRedirectionMode = System.Convert.ToInt32(Conversion.Int(this._connectionInfo.RedirectSound));
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strRdpSetRedirectionFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void SetPerformanceFlags()
		{
			try
			{
				int pFlags = 0;
				if (this._connectionInfo.DisplayThemes == false)
				{
					pFlags += System.Convert.ToInt32(Conversion.Int(Connection.Protocol.RDPConnectionProtocolImp.RDPPerformanceFlags.DisableThemes));
				}
						
				if (this._connectionInfo.DisplayWallpaper == false)
				{
					pFlags += System.Convert.ToInt32(Conversion.Int(Connection.Protocol.RDPConnectionProtocolImp.RDPPerformanceFlags.DisableWallpaper));
				}
						
				if (this._connectionInfo.EnableFontSmoothing)
				{
					pFlags += System.Convert.ToInt32(Conversion.Int(Connection.Protocol.RDPConnectionProtocolImp.RDPPerformanceFlags.EnableFontSmoothing));
				}
						
				if (this._connectionInfo.EnableDesktopComposition)
				{
					pFlags += System.Convert.ToInt32(Conversion.Int(Connection.Protocol.RDPConnectionProtocolImp.RDPPerformanceFlags.EnableDesktopComposition));
				}
						
				_rdpClient.AdvancedSettings2.PerformanceFlags = pFlags;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strRdpSetPerformanceFlagsFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void SetAuthenticationLevel()
		{
			try
			{
				_rdpClient.AdvancedSettings5.AuthenticationLevel = (uint)this._connectionInfo.RDPAuthenticationLevel;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strRdpSetAuthenticationLevelFailed + Environment.NewLine + ex.Message, true);
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
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strRdpSetEventHandlersFailed + Environment.NewLine + ex.Message, true);
			}
		}
        #endregion
		
        #region Private Events & Handlers
		private void RDPEvent_OnFatalError(int errorCode)
		{
			Event_ErrorOccured(this, System.Convert.ToString(errorCode));
		}
				
		private void RDPEvent_OnDisconnected(int discReason)
		{
			const int UI_ERR_NORMAL_DISCONNECT = 0xB08;
			if (!(discReason == UI_ERR_NORMAL_DISCONNECT))
			{
				string reason = _rdpClient.GetErrorDescription((uint)discReason, (System.UInt32) _rdpClient.ExtendedDisconnectReason);
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
		public delegate void LeaveFullscreenEventHandler(Connection.Protocol.RDPConnectionProtocolImp sender, System.EventArgs e);
		private LeaveFullscreenEventHandler LeaveFullscreenEvent;
				
		public event LeaveFullscreenEventHandler LeaveFullscreen
		{
			add
			{
				LeaveFullscreenEvent = (LeaveFullscreenEventHandler) System.Delegate.Combine(LeaveFullscreenEvent, value);
			}
			remove
			{
				LeaveFullscreenEvent = (LeaveFullscreenEventHandler) System.Delegate.Remove(LeaveFullscreenEvent, value);
			}
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
                return new Rectangle(0, 0, System.Convert.ToInt32(resolutionParts[0]), System.Convert.ToInt32(resolutionParts[1]));
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
		
        #region Terminal Sessions
		public class TerminalSessions
		{
			private WTSCOM _wtsCom;
					
			public TerminalSessions()
			{
				try
				{
					_wtsCom = new WTSCOM();
				}
				catch (Exception ex)
				{
					Runtime.MessageCollector.AddExceptionMessage("TerminalSessions.New() failed.", ex, MessageClass.ErrorMsg, true);
				}
			}
					
			public int OpenConnection(string hostname)
			{
				if (_wtsCom == null)
				{
					return 0;
				}
						
				try
				{
					return _wtsCom.WTSOpenServer(hostname);
				}
				catch (Exception ex)
				{
					Runtime.MessageCollector.AddExceptionMessage(My.Language.strRdpOpenConnectionFailed, ex, MessageClass.ErrorMsg, true);
				}
                return 0;
			}
					
			public void CloseConnection(int serverHandle)
			{
				if (_wtsCom == null)
				{
					return ;
				}
						
				try
				{
					_wtsCom.WTSCloseServer(serverHandle);
				}
				catch (Exception ex)
				{
					Runtime.MessageCollector.AddExceptionMessage(My.Language.strRdpCloseConnectionFailed, ex, MessageClass.ErrorMsg, true);
				}
			}

            public SessionsCollection GetSessions(int serverHandle)
			{
				if (_wtsCom == null)
				{
					return new SessionsCollection();
				}
						
				SessionsCollection sessions = new SessionsCollection();
						
				try
				{
					WTSSessions wtsSessions = _wtsCom.WTSEnumerateSessions(serverHandle);
							
					int sessionId = 0;
					string sessionUser = "";
					long sessionState;
					string sessionName = "";
							
					foreach (WTSSession wtsSession in wtsSessions)
					{
						sessionId = wtsSession.SessionId;
						sessionUser = _wtsCom.WTSQuerySessionInformation(serverHandle, wtsSession.SessionId, 5); // WFUsername = 5
						sessionState = long.Parse(wtsSession.State + "\r\n");
						sessionName = wtsSession.WinStationName + "\r\n";
								
						if (!string.IsNullOrEmpty(sessionUser))
						{
							if (sessionState == 0)
							{
								sessions.Add(sessionId, My.Language.strActive, sessionUser, sessionName);
							}
							else
							{
								sessions.Add(sessionId, My.Language.strInactive, sessionUser, sessionName);
							}
						}
					}
				}
				catch (Exception ex)
				{
					Runtime.MessageCollector.AddExceptionMessage(My.Language.strRdpGetSessionsFailed, ex, MessageClass.ErrorMsg, true);
				}
						
				return sessions;
			}

            public bool KillSession(int serverHandle, int sessionId)
			{
				if (_wtsCom == null)
				{
					return false;
				}
						
				bool result = false;
						
				try
				{
					result = _wtsCom.WTSLogoffSession(serverHandle, sessionId, true);
				}
				catch (Exception ex)
				{
					Runtime.MessageCollector.AddExceptionMessage("TerminalSessions.KillSession() failed.", ex, MessageClass.ErrorMsg, true);
				}
						
				return result;
			}
		}
				
		public class SessionsCollection : CollectionBase
		{
					
            public Session this[int index]
			{
				get
				{
					return ((Session) (List[index]));
				}
			}
					
            public int ItemsCount
			{
				get
				{
					return List.Count;
				}
			}
					
			public Session Add(int sessionId, string sessionState, string sessionUser, string sessionName)
			{
				Session newSession = new Session();
						
				try
				{
					newSession.SessionId = sessionId;
					newSession.SessionState = sessionState;
					newSession.SessionUser = sessionUser;
					newSession.SessionName = sessionName;
							
					List.Add(newSession);
				}
				catch (Exception ex)
				{
					Runtime.MessageCollector.AddExceptionMessage(My.Language.strRdpAddSessionFailed, ex, MessageClass.ErrorMsg, true);
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
					
			public int SessionId {get; set;}
			public string SessionState {get; set;}
			public string SessionUser {get; set;}
			public string SessionName {get; set;}
		}
        #endregion
		
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
					Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strRdpErrorGetFailure + Environment.NewLine + ex.Message, true);
					return string.Format(My.Language.strRdpErrorUnknown, id);
				}
			}
		}
        #endregion
		
        #region Reconnect Stuff
		public void tmrReconnect_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			bool srvReady = Tools.PortScan.Scanner.IsPortOpen(_connectionInfo.Hostname, System.Convert.ToString(_connectionInfo.Port));
					
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

        #region Events
        public delegate void ConnectingEventHandler(object sender);
        public event ConnectingEventHandler Connecting
        {
            add { ConnectingEvent = (ConnectingEventHandler)System.Delegate.Combine(ConnectingEvent, value); }
            remove { ConnectingEvent = (ConnectingEventHandler)System.Delegate.Remove(ConnectingEvent, value); }
        }

        public delegate void ConnectedEventHandler(object sender);
        public event ConnectedEventHandler Connected
        {
            add { ConnectedEvent = (ConnectedEventHandler)System.Delegate.Combine(ConnectedEvent, value); }
            remove { ConnectedEvent = (ConnectedEventHandler)System.Delegate.Remove(ConnectedEvent, value); }
        }

        public delegate void DisconnectedEventHandler(object sender, string DisconnectedMessage);
        public event DisconnectedEventHandler Disconnected
        {
            add { DisconnectedEvent = (DisconnectedEventHandler)System.Delegate.Combine(DisconnectedEvent, value); }
            remove { DisconnectedEvent = (DisconnectedEventHandler)System.Delegate.Remove(DisconnectedEvent, value); }
        }

        public delegate void ErrorOccuredEventHandler(object sender, string ErrorMessage);
        public event ErrorOccuredEventHandler ErrorOccured
        {
            add { ErrorOccuredEvent = (ErrorOccuredEventHandler)System.Delegate.Combine(ErrorOccuredEvent, value); }
            remove { ErrorOccuredEvent = (ErrorOccuredEventHandler)System.Delegate.Remove(ErrorOccuredEvent, value); }
        }

        public delegate void ClosingEventHandler(object sender);
        public event ClosingEventHandler Closing
        {
            add { ClosingEvent = (ClosingEventHandler)System.Delegate.Combine(ClosingEvent, value); }
            remove { ClosingEvent = (ClosingEventHandler)System.Delegate.Remove(ClosingEvent, value); }
        }

        public delegate void ClosedEventHandler(object sender);
        public event ClosedEventHandler Closed
        {
            add { ClosedEvent = (ClosedEventHandler)System.Delegate.Combine(ClosedEvent, value); }
            remove { ClosedEvent = (ClosedEventHandler)System.Delegate.Remove(ClosedEvent, value); }
        }


        public void Event_Closing(object sender)
        {
            if (ClosingEvent != null)
                ClosingEvent(sender);
        }

        public void Event_Closed(object sender)
        {
            if (ClosedEvent != null)
                ClosedEvent(sender);
        }

        public void Event_Connecting(object sender)
        {
            if (ConnectingEvent != null)
                ConnectingEvent(sender);
        }

        public void Event_Connected(object sender)
        {
            if (ConnectedEvent != null)
                ConnectedEvent(sender);
        }

        public void Event_Disconnected(object sender, string DisconnectedMessage)
        {
            if (DisconnectedEvent != null)
                DisconnectedEvent(sender, DisconnectedMessage);
        }

        public void Event_ErrorOccured(object sender, string ErrorMsg)
        {
            if (ErrorOccuredEvent != null)
                ErrorOccuredEvent(sender, ErrorMsg);
        }

        public void Event_ReconnectGroupCloseClicked()
        {
            Close();
        }
        #endregion
	}
}