using System;
using mRemoteNG.App;
using System.ComponentModel;
using mRemoteNG.Security;
using mRemoteNG.Tools;
using mRemoteNG.UI.Forms;


namespace mRemoteNG.Connection.Protocol.VNC
{
	public class ProtocolVNC : ProtocolBase
	{
        #region Properties
        public bool SmartSize
		{
			get { return _VNC.Scaled; }
			set { _VNC.Scaled = value; }
		}
				
        public bool ViewOnly
		{
			get { return _VNC.ViewOnly; }
			set { _VNC.ViewOnly = value; }
		}
        #endregion
				
        #region Private Declarations
		private VncSharp.RemoteDesktop _VNC;
		private ConnectionInfo Info;
        #endregion
				
        #region Public Methods
		public ProtocolVNC()
		{
			Control = new VncSharp.RemoteDesktop();
		}
				
		public override bool Initialize()
		{
			base.Initialize();
					
			try
			{
                _VNC = (VncSharp.RemoteDesktop)Control;
				
				Info = InterfaceControl.Info;
					
				_VNC.VncPort = Info.Port;

                return true;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strVncSetPropsFailed + Environment.NewLine + ex.Message, true);
				return false;
			}
		}
				
		public override bool Connect()
		{
			SetEventHandlers();
					
			try
			{
				_VNC.Connect(Info.Hostname, Info.VNCViewOnly, Info.VNCSmartSizeMode != SmartSizeMode.SmartSNo);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strVncConnectionOpenFailed + Environment.NewLine + ex.Message);
				return false;
			}
					
			return true;
		}
				
		public override void Disconnect()
		{
			try
			{
				_VNC.Disconnect();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strVncConnectionDisconnectFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		public void SendSpecialKeys(SpecialKeys Keys)
		{
			try
			{
				switch (Keys)
				{
					case SpecialKeys.CtrlAltDel:
						_VNC.SendSpecialKeys(VncSharp.SpecialKeys.CtrlAltDel);
						break;
					case SpecialKeys.CtrlEsc:
						_VNC.SendSpecialKeys(VncSharp.SpecialKeys.CtrlEsc);
						break;
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strVncSendSpecialKeysFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		public void ToggleSmartSize()
		{
			try
			{
				SmartSize = !SmartSize;
				RefreshScreen();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strVncToggleSmartSizeFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		public void ToggleViewOnly()
		{
			try
			{
				ViewOnly = !ViewOnly;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strVncToggleViewOnlyFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
				
		public void StartChat()
		{
		    throw (new NotImplementedException());
		}
				
		public void StartFileTransfer()
		{
            throw (new NotImplementedException());
        }
				
		public void RefreshScreen()
		{
			try
			{
				_VNC.FullScreenUpdate();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strVncRefreshFailed + Environment.NewLine + ex.Message, true);
			}
		}
        #endregion
				
        #region Private Methods
		private void SetEventHandlers()
		{
			try
			{
				_VNC.ConnectComplete += VNCEvent_Connected;
				_VNC.ConnectionLost += VNCEvent_Disconnected;
				FrmMain.ClipboardChanged += VNCEvent_ClipboardChanged;
                if (((int)Force & (int)ConnectionInfo.Force.NoCredentials) != (int)ConnectionInfo.Force.NoCredentials && !string.IsNullOrEmpty(Info.CredentialRecord?.Password.ConvertToUnsecureString()))
				{
					_VNC.GetPassword = VNCEvent_Authenticate;
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strVncSetEventHandlersFailed + Environment.NewLine + ex.Message, true);
			}
		}
        #endregion
				
        #region Private Events & Handlers
		private void VNCEvent_Connected(object sender, EventArgs e)
		{
			Event_Connected(this);
			_VNC.AutoScroll = Info.VNCSmartSizeMode == SmartSizeMode.SmartSNo;
		}
				
		private void VNCEvent_Disconnected(object sender, EventArgs e)
		{
			Event_Disconnected(sender, e.ToString());
			Close();
		}
				
		private void VNCEvent_ClipboardChanged()
		{
			_VNC.FillServerClipboard();
		}
				
		private string VNCEvent_Authenticate()
		{
			return Info.CredentialRecord?.Password.ConvertToUnsecureString() ?? "";
		}
        #endregion
				
        #region Enums
		public enum Defaults
		{
			Port = 5900
		}
				
		public enum SpecialKeys
		{
			CtrlAltDel,
			CtrlEsc
		}
				
		public enum Compression
		{
            [LocalizedAttributes.LocalizedDescription("strNoCompression")]CompNone = 99,
			[Description("0")]Comp0 = 0,
			[Description("1")]Comp1 = 1,
			[Description("2")]Comp2 = 2,
			[Description("3")]Comp3 = 3,
			[Description("4")]Comp4 = 4,
			[Description("5")]Comp5 = 5,
			[Description("6")]Comp6 = 6,
			[Description("7")]Comp7 = 7,
			[Description("8")]Comp8 = 8,
			[Description("9")]Comp9 = 9
		}
				
		public enum Encoding
		{
			[Description("Raw")]EncRaw,
			[Description("RRE")]EncRRE,
			[Description("CoRRE")]EncCorre,
			[Description("Hextile")]EncHextile,
			[Description("Zlib")]EncZlib,
			[Description("Tight")]EncTight,
			[Description("ZlibHex")]EncZLibHex,
			[Description("ZRLE")]EncZRLE
		}
				
		public enum AuthMode
		{
            [LocalizedAttributes.LocalizedDescription("VNC")]
            AuthVNC,
            [LocalizedAttributes.LocalizedDescription("Windows")]
            AuthWin
		}
				
		public enum ProxyType
		{
            [LocalizedAttributes.LocalizedDescription("strNone")]
            ProxyNone,
            [LocalizedAttributes.LocalizedDescription("strHttp")]
            ProxyHTTP,
            [LocalizedAttributes.LocalizedDescription("strSocks5")]
            ProxySocks5,
            [LocalizedAttributes.LocalizedDescription("strUltraVncRepeater")]
            ProxyUltra
		}
				
		public enum Colors
		{
            [LocalizedAttributes.LocalizedDescription("strNormal")]
            ColNormal,
			[Description("8-bit")]Col8Bit
		}
				
		public enum SmartSizeMode
		{
            [LocalizedAttributes.LocalizedDescription("strNoSmartSize")]
            SmartSNo,
            [LocalizedAttributes.LocalizedDescription("strFree")]
            SmartSFree,
            [LocalizedAttributes.LocalizedDescription("strAspect")]
            SmartSAspect
		}
        #endregion
	}
}
