// VBConversions Note: VB project level imports
using System.Collections.Generic;
using System;
using AxWFICALib;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using AxMSTSCLib;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
// End of VB project level imports

//using mRemoteNG.App.Runtime;
using System.ComponentModel;
using mRemoteNG.Tools;
//using mRemoteNG.Tools.LocalizedAttributes;


namespace mRemoteNG.Connection.Protocol
{
	public class VNC : Base
	{
				
#region Properties
        public bool SmartSize
		{
			get { return VNC_Renamed.Scaled; }
			set { VNC_Renamed.Scaled = value; }
		}
				
        public bool ViewOnly
		{
			get { return VNC_Renamed.ViewOnly; }
			set { VNC_Renamed.ViewOnly = value; }
		}
#endregion
				
#region Private Declarations
		private VncSharp.RemoteDesktop VNC_Renamed;
		private Connection.Info Info;
#endregion
				
#region Public Methods
		public VNC()
		{
			this.Control = new VncSharp.RemoteDesktop();
		}
				
		public override bool SetProps()
		{
			base.SetProps();
					
			try
			{
				VNC_Renamed = this.Control;
						
				Info = this.InterfaceControl.Info;
						
				VNC_Renamed.VncPort = this.Info.Port;
						
				//If Info.VNCCompression <> Compression.CompNone Then
				//    VNC.JPEGCompression = True
				//    VNC.JPEGCompressionLevel = Info.VNCCompression
				//End If
						
				//Select Case Info.VNCEncoding
				//    Case Encoding.EncCorre
				//        VNC.Encoding = ViewerX.VNCEncoding.RFB_CORRE
				//    Case Encoding.EncHextile
				//        VNC.Encoding = ViewerX.VNCEncoding.RFB_HEXTILE
				//    Case Encoding.EncRaw
				//        VNC.Encoding = ViewerX.VNCEncoding.RFB_RAW
				//    Case Encoding.EncRRE
				//        VNC.Encoding = ViewerX.VNCEncoding.RFB_RRE
				//    Case Encoding.EncTight
				//        VNC.Encoding = ViewerX.VNCEncoding.RFB_TIGHT
				//    Case Encoding.EncZlib
				//        VNC.Encoding = ViewerX.VNCEncoding.RFB_ZLIB
				//    Case Encoding.EncZLibHex
				//        VNC.Encoding = ViewerX.VNCEncoding.RFB_ZLIBHEX
				//    Case Encoding.EncZRLE
				//        VNC.Encoding = ViewerX.VNCEncoding.RFB_ZRLE
				//End Select
						
				//If Info.VNCAuthMode = AuthMode.AuthWin Then
				//    VNC.LoginType = ViewerX.ViewerLoginType.VLT_MSWIN
				//    VNC.MsUser = Me.Info.Username
				//    VNC.MsDomain = Me.Info.Domain
				//    VNC.MsPassword = Me.Info.Password
				//Else
				//    VNC.LoginType = ViewerX.ViewerLoginType.VLT_VNC
				//    VNC.Password = Me.Info.Password
				//End If
						
				//Select Case Info.VNCProxyType
				//    Case ProxyType.ProxyNone
				//        VNC.ProxyType = ViewerX.ConnectionProxyType.VPT_NONE
				//    Case ProxyType.ProxyHTTP
				//        VNC.ProxyType = ViewerX.ConnectionProxyType.VPT_HTTP
				//    Case ProxyType.ProxySocks5
				//        VNC.ProxyType = ViewerX.ConnectionProxyType.VPT_SOCKS5
				//    Case ProxyType.ProxyUltra
				//        VNC.ProxyType = ViewerX.ConnectionProxyType.VPT_ULTRA_REPEATER
				//End Select
						
				//If Info.VNCProxyType <> ProxyType.ProxyNone Then
				//    VNC.ProxyIP = Info.VNCProxyIP
				//    VNC.ProxyPort = Info.VNCProxyPort
				//    VNC.ProxyUser = Info.VNCProxyUsername
				//    VNC.ProxyPassword = Info.VNCProxyPassword
				//End If
						
				//If Info.VNCColors = Colors.Col8Bit Then
				//    VNC.RestrictPixel = True
				//Else
				//    VNC.RestrictPixel = False
				//End If
						
				//VNC.ConnectingText = My.Language.strInheritConnecting & " (SmartCode VNC viewer)"
				//VNC.DisconnectedText = My.Language.strInheritDisconnected
				//VNC.MessageBoxes = False
				//VNC.EndInit()
						
				return true;
			}
			catch (Exception ex)
			{
				MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strVncSetPropsFailed + Constants.vbNewLine + ex.Message, true);
				return false;
			}
		}
				
		public override bool Connect()
		{
			this.SetEventHandlers();
					
			try
			{
				VNC_Renamed.Connect(this.Info.Hostname, this.Info.VNCViewOnly, Info.VNCSmartSizeMode != SmartSizeMode.SmartSNo);
			}
			catch (Exception ex)
			{
				MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strVncConnectionOpenFailed + Constants.vbNewLine + ex.Message);
				return false;
			}
					
			return true;
		}
				
		public override void Disconnect()
		{
			try
			{
				VNC_Renamed.Disconnect();
			}
			catch (Exception ex)
			{
				MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strVncConnectionDisconnectFailed + Constants.vbNewLine + ex.Message, true);
			}
		}
				
		public void SendSpecialKeys(SpecialKeys Keys)
		{
			try
			{
				switch (Keys)
				{
					case SpecialKeys.CtrlAltDel:
						VNC_Renamed.SendSpecialKeys(SpecialKeys.CtrlAltDel);
						break;
					case SpecialKeys.CtrlEsc:
						VNC_Renamed.SendSpecialKeys(SpecialKeys.CtrlEsc);
						break;
				}
			}
			catch (Exception ex)
			{
				MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strVncSendSpecialKeysFailed + Constants.vbNewLine + ex.Message, true);
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
				MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strVncToggleSmartSizeFailed + Constants.vbNewLine + ex.Message, true);
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
				MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strVncToggleViewOnlyFailed + Constants.vbNewLine + ex.Message, true);
			}
		}
				
				
		public void StartChat()
		{
			try
			{
				//If VNC.Capabilities.Chat = True Then
				//    VNC.OpenChat()
				//Else
				//    mC.AddMessage(Messages.MessageClass.InformationMsg, "VNC Server doesn't support chat")
				//End If
			}
			catch (Exception ex)
			{
				MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strVncStartChatFailed + Constants.vbNewLine + ex.Message, true);
			}
		}
				
		public void StartFileTransfer()
		{
			try
			{
				//If VNC.Capabilities.FileTransfer = True Then
				//    VNC.OpenFileTransfer()
				//Else
				//    mC.AddMessage(Messages.MessageClass.InformationMsg, "VNC Server doesn't support file transfers")
				//End If
			}
			catch (Exception)
			{
						
			}
		}
				
		public void RefreshScreen()
		{
			try
			{
				VNC_Renamed.FullScreenUpdate();
			}
			catch (Exception ex)
			{
				MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strVncRefreshFailed + Constants.vbNewLine + ex.Message, true);
			}
		}
#endregion
				
#region Private Methods
				private void SetEventHandlers()
				{
					try
					{
						VNC_Renamed.ConnectComplete += VNCEvent_Connected;
						VNC_Renamed.ConnectionLost += VNCEvent_Disconnected;
						mRemoteNG.frmMain.clipboardchange += VNCEvent_ClipboardChanged;
						if (!((Force & Info.Force.NoCredentials) == (int) Info.Force.NoCredentials) && !string.IsNullOrEmpty(Info.Password))
						{
							VNC_Renamed.GetPassword = new System.EventHandler(VNCEvent_Authenticate);
						}
					}
					catch (Exception ex)
					{
						MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strVncSetEventHandlersFailed + Constants.vbNewLine + ex.Message, true);
					}
				}
#endregion
				
#region Private Events & Handlers
				private void VNCEvent_Connected(object sender, EventArgs e)
				{
					base.Event_Connected(this);
					VNC_Renamed.AutoScroll = Info.VNCSmartSizeMode == SmartSizeMode.SmartSNo;
				}
				
				private void VNCEvent_Disconnected(object sender, EventArgs e)
				{
					base.Event_Disconnected(sender, e.ToString());
					base.Close();
				}
				
				private void VNCEvent_ClipboardChanged()
				{
					this.VNC_Renamed.FillServerClipboard();
				}
				
				private string VNCEvent_Authenticate()
				{
					return Info.Password;
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
