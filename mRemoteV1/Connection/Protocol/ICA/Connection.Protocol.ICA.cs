using System;
using AxWFICALib;
using System.Drawing;
using System.Windows.Forms;
using mRemoteNG.App;
using System.Threading;
using mRemoteNG.Tools;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.UI.Forms;


namespace mRemoteNG.Connection.Protocol.ICA
{
	public class ProtocolICA : ProtocolBase
	{
        #region Default Instance
		private static ProtocolICA defaultInstance;
				
		/// <summary>
		/// Added by the VB.Net to C# Converter to support default instance behavour in C#
		/// </summary>
        public static ProtocolICA Default
		{
			get { return defaultInstance ?? (defaultInstance = new ProtocolICA()); }
			set
			{
				defaultInstance = value;
			}
		}

#if false
        private static void defaultInstance_FormClosed(object sender, FormClosedEventArgs e)
		{
			defaultInstance = null;
		}
#endif
#endregion

#region Private Properties
		private AxICAClient _ICAClient;
		private ConnectionInfo _Info;
#endregion
		
#region Public Methods
		public ProtocolICA()
		{
			try
			{
				Control = new AxICAClient();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strIcaControlFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		public override bool Initialize()
		{
			base.Initialize();
					
			try
			{
				_ICAClient = (AxICAClient)Control;
				_Info = InterfaceControl.Info;
				_ICAClient.CreateControl();
						
				while (!_ICAClient.Created)
				{
					Thread.Sleep(10);
					Application.DoEvents();
				}

				_ICAClient.Address = _Info.Hostname;
				SetCredentials();
				SetResolution();
				SetColors();
				SetSecurity();
						
				//Disable hotkeys for international users
				_ICAClient.Hotkey1Shift = null;
				_ICAClient.Hotkey1Char = null;
				_ICAClient.Hotkey2Shift = null;
				_ICAClient.Hotkey2Char = null;
				_ICAClient.Hotkey3Shift = null;
				_ICAClient.Hotkey3Char = null;
				_ICAClient.Hotkey4Shift = null;
				_ICAClient.Hotkey4Char = null;
				_ICAClient.Hotkey5Shift = null;
				_ICAClient.Hotkey5Char = null;
				_ICAClient.Hotkey6Shift = null;
				_ICAClient.Hotkey6Char = null;
				_ICAClient.Hotkey7Shift = null;
				_ICAClient.Hotkey7Char = null;
				_ICAClient.Hotkey8Shift = null;
				_ICAClient.Hotkey8Char = null;
				_ICAClient.Hotkey9Shift = null;
				_ICAClient.Hotkey9Char = null;
				_ICAClient.Hotkey10Shift = null;
				_ICAClient.Hotkey10Char = null;
				_ICAClient.Hotkey11Shift = null;
				_ICAClient.Hotkey11Char = null;
						
				_ICAClient.PersistentCacheEnabled = _Info.CacheBitmaps;
				_ICAClient.Title = _Info.Name;
				return true;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strIcaSetPropsFailed + Environment.NewLine + ex.Message, true);
				return false;
			}
		}
				
		public override bool Connect()
		{
			SetEventHandlers();
					
			try
			{
				_ICAClient.Connect();
				base.Connect();
				return true;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strIcaConnectionFailed + Environment.NewLine + ex.Message);
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
				
				var _user = _Info.CredentialRecord?.Username ?? "";
                var _pass = _Info.CredentialRecord?.Password ?? "".ConvertToSecureString();
                var _dom = _Info.CredentialRecord?.Domain ?? "";
						
				if (string.IsNullOrEmpty(_user))
				{
					if (Settings.Default.EmptyCredentials == "windows")
					{
						_ICAClient.Username = Environment.UserName;
					}
					else if (Settings.Default.EmptyCredentials == "custom")
					{
						_ICAClient.Username = Settings.Default.DefaultUsername;
					}
				}
				else
				{
					_ICAClient.Username = _user;
				}
						
				if (string.IsNullOrEmpty(_pass.ConvertToUnsecureString()))
				{
					if (Settings.Default.EmptyCredentials == "custom")
					{
						if (Settings.Default.DefaultPassword != "")
						{
                            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
                            _ICAClient.SetProp("ClearPassword", cryptographyProvider.Decrypt(Settings.Default.DefaultPassword, Runtime.EncryptionKey));
						}
					}
				}
				else
				{
					_ICAClient.SetProp("ClearPassword", _pass.ConvertToUnsecureString());
				}
						
				if (string.IsNullOrEmpty(_dom))
				{
					if (Settings.Default.EmptyCredentials == "windows")
					{
						_ICAClient.Domain = Environment.UserDomainName;
					}
					else if (Settings.Default.EmptyCredentials == "custom")
					{
						_ICAClient.Domain = Settings.Default.DefaultDomain;
					}
				}
				else
				{
					_ICAClient.Domain = _dom;
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strIcaSetCredentialsFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void SetResolution()
		{
			try
			{
				if ((Force & ConnectionInfo.Force.Fullscreen) == ConnectionInfo.Force.Fullscreen)
				{
                    _ICAClient.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, Screen.FromControl(FrmMain.Default).Bounds.Width, Screen.FromControl(FrmMain.Default).Bounds.Height, 0);
					_ICAClient.FullScreenWindow();
							
					return;
				}
						
				if (InterfaceControl.Info.Resolution == ProtocolRDP.RDPResolutions.FitToWindow)
				{
					_ICAClient.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, InterfaceControl.Size.Width, InterfaceControl.Size.Height, 0);
				}
				else if (InterfaceControl.Info.Resolution == ProtocolRDP.RDPResolutions.SmartSize)
				{
					_ICAClient.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, InterfaceControl.Size.Width, InterfaceControl.Size.Height, 0);
				}
				else if (InterfaceControl.Info.Resolution == ProtocolRDP.RDPResolutions.Fullscreen)
				{
					_ICAClient.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, Screen.FromControl(FrmMain.Default).Bounds.Width, Screen.FromControl(FrmMain.Default).Bounds.Height, 0);
					_ICAClient.FullScreenWindow();
				}
				else
				{
					Rectangle resolution = ProtocolRDP.GetResolutionRectangle(_Info.Resolution);
					_ICAClient.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, resolution.Width, resolution.Height, 0);
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strIcaSetResolutionFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void SetColors()
		{
			switch (_Info.Colors)
			{
				case ProtocolRDP.RDPColors.Colors256:
					_ICAClient.SetProp("DesiredColor", "2");
					break;
				case ProtocolRDP.RDPColors.Colors15Bit:
					_ICAClient.SetProp("DesiredColor", "4");
					break;
				case ProtocolRDP.RDPColors.Colors16Bit:
					_ICAClient.SetProp("DesiredColor", "4");
					break;
				default:
					_ICAClient.SetProp("DesiredColor", "8");
					break;
			}
		}
				
		private void SetSecurity()
		{
			switch (_Info.ICAEncryptionStrength)
			{
				case EncryptionStrength.Encr128BitLogonOnly:
					_ICAClient.Encrypt = true;
					_ICAClient.EncryptionLevelSession = "EncRC5-0";
					break;
				case EncryptionStrength.Encr40Bit:
					_ICAClient.Encrypt = true;
					_ICAClient.EncryptionLevelSession = "EncRC5-40";
					break;
				case EncryptionStrength.Encr56Bit:
					_ICAClient.Encrypt = true;
					_ICAClient.EncryptionLevelSession = "EncRC5-56";
					break;
				case EncryptionStrength.Encr128Bit:
					_ICAClient.Encrypt = true;
					_ICAClient.EncryptionLevelSession = "EncRC5-128";
					break;
			}
		}
				
		private void SetEventHandlers()
		{
			try
			{
				_ICAClient.OnConnecting += new EventHandler(ICAEvent_OnConnecting);
				_ICAClient.OnConnect += new EventHandler(ICAEvent_OnConnected);
				_ICAClient.OnConnectFailed += new EventHandler(ICAEvent_OnConnectFailed);
				_ICAClient.OnDisconnect += new EventHandler(ICAEvent_OnDisconnect);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strIcaSetEventHandlersFailed + Environment.NewLine + ex.Message, true);
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
		public void tmrReconnect_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			bool srvReady = PortScanner.IsPortOpen(_Info.Hostname, Convert.ToString(_Info.Port));
					
			ReconnectGroup.ServerReady = srvReady;
					
			if (ReconnectGroup.ReconnectWhenReady && srvReady)
			{
				tmrReconnect.Enabled = false;
				ReconnectGroup.DisposeReconnectGroup();
				_ICAClient.Connect();
			}
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