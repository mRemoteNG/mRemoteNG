using System;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using AxWFICALib;
using mRemoteNG.App;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Messages;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools;
using mRemoteNG.UI.Forms;


namespace mRemoteNG.Connection.Protocol.ICA
{
	public class IcaProtocol : ProtocolBase
	{
		private AxICAClient _icaClient;
		private ConnectionInfo _info;
        private readonly FrmMain _frmMain = FrmMain.Default;
		
        #region Public Methods
		public IcaProtocol()
		{
			try
			{
				Control = new AxICAClient();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strIcaControlFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		public override bool Initialize()
		{
			base.Initialize();
					
			try
			{
				_icaClient = (AxICAClient)Control;
				_info = InterfaceControl.Info;
				_icaClient.CreateControl();
						
				while (!_icaClient.Created)
				{
					Thread.Sleep(10);
					Application.DoEvents();
				}

				_icaClient.Address = _info.Hostname;
				SetCredentials();
				SetResolution();
				SetColors();
				SetSecurity();
						
				//Disable hotkeys for international users
				_icaClient.Hotkey1Shift = null;
				_icaClient.Hotkey1Char = null;
				_icaClient.Hotkey2Shift = null;
				_icaClient.Hotkey2Char = null;
				_icaClient.Hotkey3Shift = null;
				_icaClient.Hotkey3Char = null;
				_icaClient.Hotkey4Shift = null;
				_icaClient.Hotkey4Char = null;
				_icaClient.Hotkey5Shift = null;
				_icaClient.Hotkey5Char = null;
				_icaClient.Hotkey6Shift = null;
				_icaClient.Hotkey6Char = null;
				_icaClient.Hotkey7Shift = null;
				_icaClient.Hotkey7Char = null;
				_icaClient.Hotkey8Shift = null;
				_icaClient.Hotkey8Char = null;
				_icaClient.Hotkey9Shift = null;
				_icaClient.Hotkey9Char = null;
				_icaClient.Hotkey10Shift = null;
				_icaClient.Hotkey10Char = null;
				_icaClient.Hotkey11Shift = null;
				_icaClient.Hotkey11Char = null;
						
				_icaClient.PersistentCacheEnabled = _info.CacheBitmaps;
				_icaClient.Title = _info.Name;
				return true;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strIcaSetPropsFailed + Environment.NewLine + ex.Message, true);
				return false;
			}
		}
				
		public override bool Connect()
		{
			SetEventHandlers();
					
			try
			{
				_icaClient.Connect();
				base.Connect();
				return true;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strIcaConnectionFailed + Environment.NewLine + ex.Message);
				return false;
			}
		}
        #endregion
		
        #region Private Methods
		private void SetCredentials()
		{
			try
			{
                if (((int)Force & (int)ConnectionInfo.Force.NoCredentials) == (int)ConnectionInfo.Force.NoCredentials)
				{
					return;
				}

				var user = _info?.Username ?? "";
				var pass = _info?.Password ?? "";
				var dom = _info?.Domain ?? "";

				if (string.IsNullOrEmpty(user))
				{
					if (Settings.Default.EmptyCredentials == "windows")
					{
						user = Environment.UserName;
					}
					else if (Settings.Default.EmptyCredentials == "custom" || Settings.Default.EmptyCredentials == "admpwd")
					{
						user = Settings.Default.DefaultUsername;
					}
				}
				if (string.IsNullOrEmpty(dom))
				{
					if (Settings.Default.EmptyCredentials == "windows")
					{
						dom = Environment.UserDomainName;
					}
					else if (Settings.Default.EmptyCredentials == "custom" || Settings.Default.EmptyCredentials == "admpwd")
					{
						dom = Settings.Default.DefaultDomain;
					}
				}
				_icaClient.Username = user;
				_icaClient.Domain = dom;

				if (string.IsNullOrEmpty(pass))
				{
					if (Settings.Default.EmptyCredentials == "custom")
					{
						if (Settings.Default.DefaultPassword != "")
						{
							var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
							pass = cryptographyProvider.Decrypt(Settings.Default.DefaultPassword, Runtime.EncryptionKey);
						}
					}
					if (Settings.Default.EmptyCredentials == "admpwd")
					{
						if (dom == ".")
							pass = AdmPwd.PDSUtils.PdsWrapper.GetPassword(null, _info.Hostname, AdmPwd.Types.IdentityType.LocalComputerAdmin, false, false).Password;
						else
							pass = AdmPwd.PDSUtils.PdsWrapper.GetPassword(dom, user, AdmPwd.Types.IdentityType.ManagedDomainAccount, false, false).Password;
					}
				}

				_icaClient.SetProp("ClearPassword", pass);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strIcaSetCredentialsFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void SetResolution()
		{
			try
			{
				if ((Force & ConnectionInfo.Force.Fullscreen) == ConnectionInfo.Force.Fullscreen)
				{
                    _icaClient.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, Screen.FromControl(_frmMain).Bounds.Width, Screen.FromControl(_frmMain).Bounds.Height, 0);
					_icaClient.FullScreenWindow();
							
					return;
				}
						
				if (InterfaceControl.Info.Resolution == RdpProtocol.RDPResolutions.FitToWindow)
				{
					_icaClient.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, InterfaceControl.Size.Width, InterfaceControl.Size.Height, 0);
				}
				else if (InterfaceControl.Info.Resolution == RdpProtocol.RDPResolutions.SmartSize)
				{
					_icaClient.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, InterfaceControl.Size.Width, InterfaceControl.Size.Height, 0);
				}
				else if (InterfaceControl.Info.Resolution == RdpProtocol.RDPResolutions.Fullscreen)
				{
					_icaClient.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, Screen.FromControl(_frmMain).Bounds.Width, Screen.FromControl(_frmMain).Bounds.Height, 0);
					_icaClient.FullScreenWindow();
				}
				else
				{
					var resolution = RdpProtocol.GetResolutionRectangle(_info.Resolution);
					_icaClient.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, resolution.Width, resolution.Height, 0);
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strIcaSetResolutionFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void SetColors()
		{
		    // ReSharper disable once SwitchStatementMissingSomeCases
			switch (_info.Colors)
			{
				case RdpProtocol.RDPColors.Colors256:
					_icaClient.SetProp("DesiredColor", "2");
					break;
				case RdpProtocol.RDPColors.Colors15Bit:
					_icaClient.SetProp("DesiredColor", "4");
					break;
				case RdpProtocol.RDPColors.Colors16Bit:
					_icaClient.SetProp("DesiredColor", "4");
					break;
				default:
					_icaClient.SetProp("DesiredColor", "8");
					break;
			}
		}
				
		private void SetSecurity()
		{
		    // ReSharper disable once SwitchStatementMissingSomeCases
			switch (_info.ICAEncryptionStrength)
			{
				case EncryptionStrength.Encr128BitLogonOnly:
					_icaClient.Encrypt = true;
					_icaClient.EncryptionLevelSession = "EncRC5-0";
					break;
				case EncryptionStrength.Encr40Bit:
					_icaClient.Encrypt = true;
					_icaClient.EncryptionLevelSession = "EncRC5-40";
					break;
				case EncryptionStrength.Encr56Bit:
					_icaClient.Encrypt = true;
					_icaClient.EncryptionLevelSession = "EncRC5-56";
					break;
				case EncryptionStrength.Encr128Bit:
					_icaClient.Encrypt = true;
					_icaClient.EncryptionLevelSession = "EncRC5-128";
					break;
			}
		}
				
		private void SetEventHandlers()
		{
			try
			{
				_icaClient.OnConnecting += ICAEvent_OnConnecting;
				_icaClient.OnConnect += ICAEvent_OnConnected;
				_icaClient.OnConnectFailed += ICAEvent_OnConnectFailed;
				_icaClient.OnDisconnect += ICAEvent_OnDisconnect;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strIcaSetEventHandlersFailed + Environment.NewLine + ex.Message, true);
			}
		}
        #endregion
		
        #region Private Events & Handlers
		private void ICAEvent_OnConnecting(object sender, EventArgs e)
		{
			Event_Connecting(this);
		}
				
		private void ICAEvent_OnConnected(object sender, EventArgs e)
		{
			Event_Connected(this);
		}
				
		private void ICAEvent_OnConnectFailed(object sender, EventArgs e)
		{
			Event_ErrorOccured(this, e.ToString());
		}
				
		private void ICAEvent_OnDisconnect(object sender, EventArgs e)
		{
			Event_Disconnected(this, e.ToString());
					
			if (Settings.Default.ReconnectOnDisconnect)
			{
				ReconnectGroup = new ReconnectGroup();
				//this.Load += ReconnectGroup_Load;
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
        #endregion
		
        #region Reconnect Stuff
		public void tmrReconnect_Elapsed(object sender, ElapsedEventArgs e)
		{
			var srvReady = PortScanner.IsPortOpen(_info.Hostname, Convert.ToString(_info.Port));
					
			ReconnectGroup.ServerReady = srvReady;

		    if (!ReconnectGroup.ReconnectWhenReady || !srvReady) return;
		    tmrReconnect.Enabled = false;
		    ReconnectGroup.DisposeReconnectGroup();
		    _icaClient.Connect();
		}
        #endregion
		
        #region Enums
		public enum Defaults
		{
			Port = 1494,
			EncryptionStrength = 0
		}
				
		public enum EncryptionStrength
		{
            [LocalizedAttributes.LocalizedDescription("strEncBasic")]
            EncrBasic = 1,
            [LocalizedAttributes.LocalizedDescription("strEnc128BitLogonOnly")]
            Encr128BitLogonOnly = 127,
            [LocalizedAttributes.LocalizedDescription("strEnc40Bit")]
            Encr40Bit = 40,
            [LocalizedAttributes.LocalizedDescription("strEnc56Bit")]
            Encr56Bit = 56,
            [LocalizedAttributes.LocalizedDescription("strEnc128Bit")]
            Encr128Bit = 128
		}
        #endregion
	}
}