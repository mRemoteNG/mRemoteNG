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
using System.Threading;
using mRemoteNG.Tools;
//using mRemoteNG.Tools.LocalizedAttributes;


namespace mRemoteNG.Connection.Protocol
{
	public class ICA : Base
	{
				
#region Default Instance
				
		private static ICA defaultInstance;
				
		/// <summary>
		/// Added by the VB.Net to C# Converter to support default instance behavour in C#
		/// </summary>
        public static ICA Default
		{
			get
			{
				if (defaultInstance == null)
				{
					defaultInstance = new ICA();
					defaultInstance.FormClosed += new FormClosedEventHandler(defaultInstance_FormClosed);
				}
						
				return defaultInstance;
			}
			set
			{
				defaultInstance = value;
			}
		}
				
		static void defaultInstance_FormClosed(object sender, FormClosedEventArgs e)
		{
			defaultInstance = null;
		}
				
#endregion
				
#region Private Properties
		private AxICAClient ICA_Renamed;
		private Connection.Info Info;
#endregion
				
#region Public Methods
		public ICA()
		{
			try
			{
				this.Control = new AxICAClient();
			}
			catch (Exception ex)
			{
				MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strIcaControlFailed + Constants.vbNewLine + ex.Message, true);
			}
		}
				
		public override bool SetProps()
		{
			base.SetProps();
					
			try
			{
				ICA_Renamed = this.Control;
				Info = this.InterfaceControl.Info;
						
				ICA_Renamed.Default.CreateControl();
						
				while (!this.ICA_Renamed.Created)
				{
					Thread.Sleep(10);
					System.Windows.Forms.Application.DoEvents();
				}
						
				ICA_Renamed.Address = Info.Hostname;
						
				this.SetCredentials();
						
				this.SetResolution();
				this.SetColors();
						
				this.SetSecurity();
						
				//Disable hotkeys for international users
				ICA_Renamed.Hotkey1Shift = null;
				ICA_Renamed.Hotkey1Char = null;
				ICA_Renamed.Hotkey2Shift = null;
				ICA_Renamed.Hotkey2Char = null;
				ICA_Renamed.Hotkey3Shift = null;
				ICA_Renamed.Hotkey3Char = null;
				ICA_Renamed.Hotkey4Shift = null;
				ICA_Renamed.Hotkey4Char = null;
				ICA_Renamed.Hotkey5Shift = null;
				ICA_Renamed.Hotkey5Char = null;
				ICA_Renamed.Hotkey6Shift = null;
				ICA_Renamed.Hotkey6Char = null;
				ICA_Renamed.Hotkey7Shift = null;
				ICA_Renamed.Hotkey7Char = null;
				ICA_Renamed.Hotkey8Shift = null;
				ICA_Renamed.Hotkey8Char = null;
				ICA_Renamed.Hotkey9Shift = null;
				ICA_Renamed.Hotkey9Char = null;
				ICA_Renamed.Hotkey10Shift = null;
				ICA_Renamed.Hotkey10Char = null;
				ICA_Renamed.Hotkey11Shift = null;
				ICA_Renamed.Hotkey11Char = null;
						
				ICA_Renamed.PersistentCacheEnabled = Info.CacheBitmaps;
						
				ICA_Renamed.Title = Info.Name;
						
				return true;
			}
			catch (Exception ex)
			{
				MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strIcaSetPropsFailed + Constants.vbNewLine + ex.Message, true);
				return false;
			}
		}
				
		public override bool Connect()
		{
			this.SetEventHandlers();
					
			try
			{
				ICA_Renamed.Default.Connect();
				base.Connect();
				return true;
			}
			catch (Exception ex)
			{
				MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strIcaConnectionFailed + Constants.vbNewLine + ex.Message);
				return false;
			}
		}
#endregion
				
#region Private Methods
		private void SetCredentials()
		{
			try
			{
				if ((Force & Info.Force.NoCredentials) == (int) Info.Force.NoCredentials)
				{
					return ;
				}
						
				string _user = this.Info.Username;
				string _pass = this.Info.Password;
				string _dom = this.Info.Domain;
						
				if (string.IsNullOrEmpty(_user))
				{
					if ((string) My.Settings.Default.EmptyCredentials == "windows")
					{
						ICA_Renamed.Username = Environment.UserName;
					}
					else if ((string) My.Settings.Default.EmptyCredentials == "custom")
					{
						ICA_Renamed.Username = My.Settings.Default.DefaultUsername;
					}
				}
				else
				{
					ICA_Renamed.Username = _user;
				}
						
				if (string.IsNullOrEmpty(_pass))
				{
					if ((string) My.Settings.Default.EmptyCredentials == "custom")
					{
						if (My.Settings.Default.DefaultPassword != "")
						{
							ICA_Renamed.SetProp("ClearPassword", Security.Crypt.Decrypt(System.Convert.ToString(My.Settings.Default.DefaultPassword), App.Info.General.EncryptionKey));
						}
					}
				}
				else
				{
					ICA_Renamed.SetProp("ClearPassword", _pass);
				}
						
				if (string.IsNullOrEmpty(_dom))
				{
					if ((string) My.Settings.Default.EmptyCredentials == "windows")
					{
						ICA_Renamed.Domain = Environment.UserDomainName;
					}
					else if ((string) My.Settings.Default.EmptyCredentials == "custom")
					{
						ICA_Renamed.Domain = My.Settings.Default.DefaultDomain;
					}
				}
				else
				{
					ICA_Renamed.Domain = _dom;
				}
			}
			catch (Exception ex)
			{
				MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strIcaSetCredentialsFailed + Constants.vbNewLine + ex.Message, true);
			}
		}
				
		private void SetResolution()
		{
			try
			{
				if ((this.Force & Connection.Info.Force.Fullscreen) == Connection.Info.Force.Fullscreen)
				{
					ICA_Renamed.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, Screen.FromControl(frmMain).Bounds.Width, Screen.FromControl(frmMain).Bounds.Height, 0);
					ICA_Renamed.FullScreenWindow();
							
					return;
				}
						
				if (this.InterfaceControl.Info.Resolution == RDP.RDPResolutions.FitToWindow)
				{
					ICA_Renamed.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, this.InterfaceControl.Size.Width, this.InterfaceControl.Size.Height, 0);
				}
				else if (this.InterfaceControl.Info.Resolution == RDP.RDPResolutions.SmartSize)
				{
					ICA_Renamed.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, this.InterfaceControl.Size.Width, this.InterfaceControl.Size.Height, 0);
				}
				else if (this.InterfaceControl.Info.Resolution == RDP.RDPResolutions.Fullscreen)
				{
					ICA_Renamed.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, Screen.FromControl(frmMain).Bounds.Width, Screen.FromControl(frmMain).Bounds.Height, 0);
					ICA_Renamed.FullScreenWindow();
				}
				else
				{
					Rectangle resolution = RDP.GetResolutionRectangle(Info.Resolution);
					ICA_Renamed.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, resolution.Width, resolution.Height, 0);
				}
			}
			catch (Exception ex)
			{
				MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strIcaSetResolutionFailed + Constants.vbNewLine + ex.Message, true);
			}
		}
				
		private void SetColors()
		{
			switch (Info.Colors)
			{
				case RDP.RDPColors.Colors256:
					ICA_Renamed.SetProp("DesiredColor", 2);
					break;
				case RDP.RDPColors.Colors15Bit:
					ICA_Renamed.SetProp("DesiredColor", 4);
					break;
				case RDP.RDPColors.Colors16Bit:
					ICA_Renamed.SetProp("DesiredColor", 4);
					break;
				default:
					ICA_Renamed.SetProp("DesiredColor", 8);
					break;
			}
		}
				
		private void SetSecurity()
		{
			switch (Info.ICAEncryption)
			{
				case EncryptionStrength.Encr128BitLogonOnly:
					ICA_Renamed.Encrypt = true;
					ICA_Renamed.EncryptionLevelSession = "EncRC5-0";
					break;
				case EncryptionStrength.Encr40Bit:
					ICA_Renamed.Encrypt = true;
					ICA_Renamed.EncryptionLevelSession = "EncRC5-40";
					break;
				case EncryptionStrength.Encr56Bit:
					ICA_Renamed.Encrypt = true;
					ICA_Renamed.EncryptionLevelSession = "EncRC5-56";
					break;
				case EncryptionStrength.Encr128Bit:
					ICA_Renamed.Encrypt = true;
					ICA_Renamed.EncryptionLevelSession = "EncRC5-128";
					break;
			}
		}
				
		private void SetEventHandlers()
		{
			try
			{
				ICA_Renamed.OnConnecting += new System.EventHandler(ICAEvent_OnConnecting);
				ICA_Renamed.OnConnect += new System.EventHandler(ICAEvent_OnConnected);
				ICA_Renamed.OnConnectFailed += new System.EventHandler(ICAEvent_OnConnectFailed);
				ICA_Renamed.OnDisconnect += new System.EventHandler(ICAEvent_OnDisconnect);
			}
			catch (Exception ex)
			{
				MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strIcaSetEventHandlersFailed + Constants.vbNewLine + ex.Message, true);
			}
		}
#endregion
				
#region Private Events & Handlers
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
					
			if (My.Settings.Default.ReconnectOnDisconnect)
			{
				ReconnectGroup = new ReconnectGroup();
				this.Load += ReconnectGroup_Load;
				ReconnectGroup.Left = (int) (((double) Control.Width / 2) - ((double) ReconnectGroup.Width / 2));
				ReconnectGroup.Top = (int) (((double) Control.Height / 2) - ((double) ReconnectGroup.Height / 2));
				ReconnectGroup.Parent = Control;
				ReconnectGroup.Show();
				tmrReconnect.Enabled = true;
			}
			else
			{
				base.Close();
			}
		}
#endregion
				
#region Reconnect Stuff
		public void tmrReconnect_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			bool srvReady = Tools.PortScan.Scanner.IsPortOpen(Info.Hostname, System.Convert.ToString(Info.Port));
					
			ReconnectGroup.ServerReady = srvReady;
					
			if (ReconnectGroup.ReconnectWhenReady && srvReady)
			{
				tmrReconnect.Enabled = false;
				ReconnectGroup.DisposeReconnectGroup();
				ICA_Renamed.Default.Connect();
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