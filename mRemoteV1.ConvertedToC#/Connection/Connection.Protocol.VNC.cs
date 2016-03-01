using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.App.Runtime;
using System.ComponentModel;
using mRemoteNG.Tools.LocalizedAttributes;

namespace mRemoteNG.Connection
{
	namespace Protocol
	{
		public class VNC : Connection.Protocol.Base
		{

			#region "Properties"
			public bool SmartSize {
				get { return VNC.Scaled; }
				set { VNC.Scaled = value; }
			}

			public bool ViewOnly {
				get { return VNC.ViewOnly; }
				set { VNC.ViewOnly = value; }
			}
			#endregion

			#region "Private Declarations"
			private VncSharp.RemoteDesktop VNC;
				#endregion
			private Connection.Info Info;

			#region "Public Methods"
			public VNC()
			{
				this.Control = new VncSharp.RemoteDesktop();
			}

			public override bool SetProps()
			{
				base.SetProps();

				try {
					VNC = this.Control;

					Info = this.InterfaceControl.Info;

					VNC.VncPort = this.Info.Port;

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
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strVncSetPropsFailed + Constants.vbNewLine + ex.Message, true);
					return false;
				}
			}

			public override bool Connect()
			{
				this.SetEventHandlers();

				try {
					VNC.Connect(this.Info.Hostname, this.Info.VNCViewOnly, Info.VNCSmartSizeMode != SmartSizeMode.SmartSNo);
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strVncConnectionOpenFailed + Constants.vbNewLine + ex.Message);
					return false;
				}

				return true;
			}

			public override void Disconnect()
			{
				try {
					VNC.Disconnect();
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strVncConnectionDisconnectFailed + Constants.vbNewLine + ex.Message, true);
				}
			}

			public void SendSpecialKeys(SpecialKeys Keys)
			{
				try {
					switch (Keys) {
						case SpecialKeys.CtrlAltDel:
							VNC.SendSpecialKeys(SpecialKeys.CtrlAltDel);
							break;
						case SpecialKeys.CtrlEsc:
							VNC.SendSpecialKeys(SpecialKeys.CtrlEsc);
							break;
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strVncSendSpecialKeysFailed + Constants.vbNewLine + ex.Message, true);
				}
			}

			public void ToggleSmartSize()
			{
				try {
					SmartSize = !SmartSize;
					RefreshScreen();
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strVncToggleSmartSizeFailed + Constants.vbNewLine + ex.Message, true);
				}
			}

			public void ToggleViewOnly()
			{
				try {
					ViewOnly = !ViewOnly;
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strVncToggleViewOnlyFailed + Constants.vbNewLine + ex.Message, true);
				}
			}


			public void StartChat()
			{
				try {
					//If VNC.Capabilities.Chat = True Then
					//    VNC.OpenChat()
					//Else
					//    mC.AddMessage(Messages.MessageClass.InformationMsg, "VNC Server doesn't support chat")
					//End If
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strVncStartChatFailed + Constants.vbNewLine + ex.Message, true);
				}
			}

			public void StartFileTransfer()
			{
				try {
					//If VNC.Capabilities.FileTransfer = True Then
					//    VNC.OpenFileTransfer()
					//Else
					//    mC.AddMessage(Messages.MessageClass.InformationMsg, "VNC Server doesn't support file transfers")
					//End If

				} catch (Exception ex) {
				}
			}

			public void RefreshScreen()
			{
				try {
					VNC.FullScreenUpdate();
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strVncRefreshFailed + Constants.vbNewLine + ex.Message, true);
				}
			}
			#endregion

			#region "Private Methods"
			private void SetEventHandlers()
			{
				try {
					VNC.ConnectComplete += VNCEvent_Connected;
					VNC.ConnectionLost += VNCEvent_Disconnected;
					mRemoteNG.frmMain.clipboardchange += VNCEvent_ClipboardChanged;
					if (!((Force & Info.Force.NoCredentials) == Info.Force.NoCredentials) & !string.IsNullOrEmpty(Info.Password)) {
						VNC.GetPassword = VNCEvent_Authenticate;
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strVncSetEventHandlersFailed + Constants.vbNewLine + ex.Message, true);
				}
			}
			#endregion

			#region "Private Events & Handlers"
			private void VNCEvent_Connected(object sender, EventArgs e)
			{
				base.Event_Connected(this);
				VNC.AutoScroll = Info.VNCSmartSizeMode == SmartSizeMode.SmartSNo;
			}

			private void VNCEvent_Disconnected(object sender, EventArgs e)
			{
				base.Event_Disconnected(sender, e.ToString());
				base.Close();
			}

			private void VNCEvent_ClipboardChanged()
			{
				this.VNC.FillServerClipboard();
			}

			private string VNCEvent_Authenticate()
			{
				return Info.Password;
			}
			#endregion

			#region "Enums"
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
				[LocalizedDescription("strNoCompression")]
				CompNone = 99,
				[Description("0")]
				Comp0 = 0,
				[Description("1")]
				Comp1 = 1,
				[Description("2")]
				Comp2 = 2,
				[Description("3")]
				Comp3 = 3,
				[Description("4")]
				Comp4 = 4,
				[Description("5")]
				Comp5 = 5,
				[Description("6")]
				Comp6 = 6,
				[Description("7")]
				Comp7 = 7,
				[Description("8")]
				Comp8 = 8,
				[Description("9")]
				Comp9 = 9
			}

			public enum Encoding
			{
				[Description("Raw")]
				EncRaw,
				[Description("RRE")]
				EncRRE,
				[Description("CoRRE")]
				EncCorre,
				[Description("Hextile")]
				EncHextile,
				[Description("Zlib")]
				EncZlib,
				[Description("Tight")]
				EncTight,
				[Description("ZlibHex")]
				EncZLibHex,
				[Description("ZRLE")]
				EncZRLE
			}

			public enum AuthMode
			{
				[LocalizedDescription("VNC")]
				AuthVNC,
				[LocalizedDescription("Windows")]
				AuthWin
			}

			public enum ProxyType
			{
				[LocalizedDescription("strNone")]
				ProxyNone,
				[LocalizedDescription("strHttp")]
				ProxyHTTP,
				[LocalizedDescription("strSocks5")]
				ProxySocks5,
				[LocalizedDescription("strUltraVncRepeater")]
				ProxyUltra
			}

			public enum Colors
			{
				[LocalizedDescription("strNormal")]
				ColNormal,
				[Description("8-bit")]
				Col8Bit
			}

			public enum SmartSizeMode
			{
				[LocalizedDescription("strNoSmartSize")]
				SmartSNo,
				[LocalizedDescription("strFree")]
				SmartSFree,
				[LocalizedDescription("strAspect")]
				SmartSAspect
			}
			#endregion
		}
	}
}
