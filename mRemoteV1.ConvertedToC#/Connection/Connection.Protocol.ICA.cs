using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.App.Runtime;
using System.Threading;
using AxWFICALib;
using mRemoteNG.Tools.LocalizedAttributes;

namespace mRemoteNG.Connection
{
	namespace Protocol
	{
		public class ICA : Connection.Protocol.Base
		{

			#region "Private Properties"
			private AxICAClient ICA;
				#endregion
			private Connection.Info Info;

			#region "Public Methods"
			public ICA()
			{
				try {
					this.Control = new AxICAClient();
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strIcaControlFailed + Constants.vbNewLine + ex.Message, true);
				}
			}

			public override bool SetProps()
			{
				base.SetProps();

				try {
					ICA = this.Control;
					Info = this.InterfaceControl.Info;

					ICA.CreateControl();

					while (!(this.ICA.Created)) {
						Thread.Sleep(10);
						System.Windows.Forms.Application.DoEvents();
					}

					ICA.Address = Info.Hostname;

					this.SetCredentials();

					this.SetResolution();
					this.SetColors();

					this.SetSecurity();

					//Disable hotkeys for international users
					ICA.Hotkey1Shift = null;
					ICA.Hotkey1Char = null;
					ICA.Hotkey2Shift = null;
					ICA.Hotkey2Char = null;
					ICA.Hotkey3Shift = null;
					ICA.Hotkey3Char = null;
					ICA.Hotkey4Shift = null;
					ICA.Hotkey4Char = null;
					ICA.Hotkey5Shift = null;
					ICA.Hotkey5Char = null;
					ICA.Hotkey6Shift = null;
					ICA.Hotkey6Char = null;
					ICA.Hotkey7Shift = null;
					ICA.Hotkey7Char = null;
					ICA.Hotkey8Shift = null;
					ICA.Hotkey8Char = null;
					ICA.Hotkey9Shift = null;
					ICA.Hotkey9Char = null;
					ICA.Hotkey10Shift = null;
					ICA.Hotkey10Char = null;
					ICA.Hotkey11Shift = null;
					ICA.Hotkey11Char = null;

					ICA.PersistentCacheEnabled = Info.CacheBitmaps;

					ICA.Title = Info.Name;

					return true;
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strIcaSetPropsFailed + Constants.vbNewLine + ex.Message, true);
					return false;
				}
			}

			public override bool Connect()
			{
				this.SetEventHandlers();

				try {
					ICA.Connect();
					base.Connect();
					return true;
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strIcaConnectionFailed + Constants.vbNewLine + ex.Message);
					return false;
				}
			}
			#endregion

			#region "Private Methods"
			private void SetCredentials()
			{
				try {
					if ((Force & Info.Force.NoCredentials) == Info.Force.NoCredentials)
						return;

					string _user = this.Info.Username;
					string _pass = this.Info.Password;
					string _dom = this.Info.Domain;

					if (string.IsNullOrEmpty(_user)) {
						switch (mRemoteNG.My.Settings.EmptyCredentials) {
							case "windows":
								ICA.Username = Environment.UserName;
								break;
							case "custom":
								ICA.Username = mRemoteNG.My.Settings.DefaultUsername;
								break;
						}
					} else {
						ICA.Username = _user;
					}

					if (string.IsNullOrEmpty(_pass)) {
						switch (mRemoteNG.My.Settings.EmptyCredentials) {
							case "custom":
								if (!string.IsNullOrEmpty(mRemoteNG.My.Settings.DefaultPassword)) {
									ICA.SetProp("ClearPassword", mRemoteNG.Security.Crypt.Decrypt(mRemoteNG.My.Settings.DefaultPassword, mRemoteNG.App.Info.General.EncryptionKey));
								}
								break;
						}
					} else {
						ICA.SetProp("ClearPassword", _pass);
					}

					if (string.IsNullOrEmpty(_dom)) {
						switch (mRemoteNG.My.Settings.EmptyCredentials) {
							case "windows":
								ICA.Domain = Environment.UserDomainName;
								break;
							case "custom":
								ICA.Domain = mRemoteNG.My.Settings.DefaultDomain;
								break;
						}
					} else {
						ICA.Domain = _dom;
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strIcaSetCredentialsFailed + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void SetResolution()
			{
				try {
					if ((this.Force & mRemoteNG.Connection.Info.Force.Fullscreen) == mRemoteNG.Connection.Info.Force.Fullscreen) {
						ICA.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, Screen.FromControl(frmMain).Bounds.Width, Screen.FromControl(frmMain).Bounds.Height, 0);
						ICA.FullScreenWindow();

						return;
					}

					switch (this.InterfaceControl.Info.Resolution) {
						case RDP.RDPResolutions.FitToWindow:
							ICA.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, this.InterfaceControl.Size.Width, this.InterfaceControl.Size.Height, 0);
							break;
						case RDP.RDPResolutions.SmartSize:
							ICA.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, this.InterfaceControl.Size.Width, this.InterfaceControl.Size.Height, 0);
							break;
						case RDP.RDPResolutions.Fullscreen:
							ICA.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, Screen.FromControl(frmMain).Bounds.Width, Screen.FromControl(frmMain).Bounds.Height, 0);
							ICA.FullScreenWindow();
							break;
						default:
							Rectangle resolution = RDP.GetResolutionRectangle(Info.Resolution);
							ICA.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, resolution.Width, resolution.Height, 0);
							break;
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strIcaSetResolutionFailed + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void SetColors()
			{
				switch (Info.Colors) {
					case RDP.RDPColors.Colors256:
						ICA.SetProp("DesiredColor", 2);
						break;
					case RDP.RDPColors.Colors15Bit:
						ICA.SetProp("DesiredColor", 4);
						break;
					case RDP.RDPColors.Colors16Bit:
						ICA.SetProp("DesiredColor", 4);
						break;
					default:
						ICA.SetProp("DesiredColor", 8);
						break;
				}
			}

			private void SetSecurity()
			{
				switch (Info.ICAEncryption) {
					case EncryptionStrength.Encr128BitLogonOnly:
						ICA.Encrypt = true;
						ICA.EncryptionLevelSession = "EncRC5-0";
						break;
					case EncryptionStrength.Encr40Bit:
						ICA.Encrypt = true;
						ICA.EncryptionLevelSession = "EncRC5-40";
						break;
					case EncryptionStrength.Encr56Bit:
						ICA.Encrypt = true;
						ICA.EncryptionLevelSession = "EncRC5-56";
						break;
					case EncryptionStrength.Encr128Bit:
						ICA.Encrypt = true;
						ICA.EncryptionLevelSession = "EncRC5-128";
						break;
				}
			}

			private void SetEventHandlers()
			{
				try {
					ICA.OnConnecting += ICAEvent_OnConnecting;
					ICA.OnConnect += ICAEvent_OnConnected;
					ICA.OnConnectFailed += ICAEvent_OnConnectFailed;
					ICA.OnDisconnect += ICAEvent_OnDisconnect;
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strIcaSetEventHandlersFailed + Constants.vbNewLine + ex.Message, true);
				}
			}
			#endregion

			#region "Private Events & Handlers"
			private void ICAEvent_OnConnecting(object sender, System.EventArgs e)
			{
				base.Event_Connecting(this);
			}

			private void ICAEvent_OnConnected(object sender, System.EventArgs e)
			{
				base.Event_Connected(this);
			}

			private void ICAEvent_OnConnectFailed(object sender, System.EventArgs e)
			{
				base.Event_ErrorOccured(this, e.ToString());
			}

			private void ICAEvent_OnDisconnect(object sender, System.EventArgs e)
			{
				base.Event_Disconnected(this, e.ToString());

				if (mRemoteNG.My.Settings.ReconnectOnDisconnect) {
					ReconnectGroup = new ReconnectGroup();
					ReconnectGroup.Left = (Control.Width / 2) - (ReconnectGroup.Width / 2);
					ReconnectGroup.Top = (Control.Height / 2) - (ReconnectGroup.Height / 2);
					ReconnectGroup.Parent = Control;
					ReconnectGroup.Show();
					tmrReconnect.Enabled = true;
				} else {
					base.Close();
				}
			}
			#endregion

			#region "Reconnect Stuff"
			private void tmrReconnect_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
			{
				bool srvReady = mRemoteNG.Tools.PortScan.Scanner.IsPortOpen(Info.Hostname, Info.Port);

				ReconnectGroup.ServerReady = srvReady;

				if (ReconnectGroup.ReconnectWhenReady & srvReady) {
					tmrReconnect.Enabled = false;
					ReconnectGroup.DisposeReconnectGroup();
					ICA.Connect();
				}
			}
			#endregion

			#region "Enums"
			public enum Defaults
			{
				Port = 1494,
				EncryptionStrength = 0
			}

			public enum EncryptionStrength
			{
				[LocalizedDescription("strEncBasic")]
				EncrBasic = 1,
				[LocalizedDescription("strEnc128BitLogonOnly")]
				Encr128BitLogonOnly = 127,
				[LocalizedDescription("strEnc40Bit")]
				Encr40Bit = 40,
				[LocalizedDescription("strEnc56Bit")]
				Encr56Bit = 56,
				[LocalizedDescription("strEnc128Bit")]
				Encr128Bit = 128
			}
			#endregion
		}
	}
}
