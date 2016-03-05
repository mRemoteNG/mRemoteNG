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
using mRemoteNG.App;
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
		private AxICAClient ICAClient;
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
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strIcaControlFailed + Constants.vbNewLine + ex.Message, true);
			}
		}
				
		public override bool SetProps()
		{
			base.SetProps();
					
			try
			{
				ICAClient = (AxICAClient)this.Control;
				Info = this.InterfaceControl.Info;
				ICAClient.CreateControl();
						
				while (!this.ICAClient.Created)
				{
					Thread.Sleep(10);
					System.Windows.Forms.Application.DoEvents();
				}

				ICAClient.Address = Info.Hostname;
				this.SetCredentials();
				this.SetResolution();
				this.SetColors();
				this.SetSecurity();
						
				//Disable hotkeys for international users
				ICAClient.Hotkey1Shift = null;
				ICAClient.Hotkey1Char = null;
				ICAClient.Hotkey2Shift = null;
				ICAClient.Hotkey2Char = null;
				ICAClient.Hotkey3Shift = null;
				ICAClient.Hotkey3Char = null;
				ICAClient.Hotkey4Shift = null;
				ICAClient.Hotkey4Char = null;
				ICAClient.Hotkey5Shift = null;
				ICAClient.Hotkey5Char = null;
				ICAClient.Hotkey6Shift = null;
				ICAClient.Hotkey6Char = null;
				ICAClient.Hotkey7Shift = null;
				ICAClient.Hotkey7Char = null;
				ICAClient.Hotkey8Shift = null;
				ICAClient.Hotkey8Char = null;
				ICAClient.Hotkey9Shift = null;
				ICAClient.Hotkey9Char = null;
				ICAClient.Hotkey10Shift = null;
				ICAClient.Hotkey10Char = null;
				ICAClient.Hotkey11Shift = null;
				ICAClient.Hotkey11Char = null;
						
				ICAClient.PersistentCacheEnabled = Info.CacheBitmaps;
				ICAClient.Title = Info.Name;
				return true;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strIcaSetPropsFailed + Constants.vbNewLine + ex.Message, true);
				return false;
			}
		}
				
		public override bool Connect()
		{
			this.SetEventHandlers();
					
			try
			{
				ICAClient.Connect();
				base.Connect();
				return true;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strIcaConnectionFailed + Constants.vbNewLine + ex.Message);
				return false;
			}
		}
        #endregion
				
        #region Private Methods
		private void SetCredentials()
		{
			try
			{
				if (((int)Force & (int)Info.Force.NoCredentials) == (int) Info.Force.NoCredentials)
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
						ICAClient.Username = Environment.UserName;
					}
					else if ((string) My.Settings.Default.EmptyCredentials == "custom")
					{
						ICAClient.Username = My.Settings.Default.DefaultUsername;
					}
				}
				else
				{
					ICAClient.Username = _user;
				}
						
				if (string.IsNullOrEmpty(_pass))
				{
					if ((string) My.Settings.Default.EmptyCredentials == "custom")
					{
						if (My.Settings.Default.DefaultPassword != "")
						{
							ICAClient.SetProp("ClearPassword", Security.Crypt.Decrypt(System.Convert.ToString(My.Settings.Default.DefaultPassword), App.Info.General.EncryptionKey));
						}
					}
				}
				else
				{
					ICAClient.SetProp("ClearPassword", _pass);
				}
						
				if (string.IsNullOrEmpty(_dom))
				{
					if ((string) My.Settings.Default.EmptyCredentials == "windows")
					{
						ICAClient.Domain = Environment.UserDomainName;
					}
					else if ((string) My.Settings.Default.EmptyCredentials == "custom")
					{
						ICAClient.Domain = My.Settings.Default.DefaultDomain;
					}
				}
				else
				{
					ICAClient.Domain = _dom;
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strIcaSetCredentialsFailed + Constants.vbNewLine + ex.Message, true);
			}
		}
				
		private void SetResolution()
		{
			try
			{
				if ((this.Force & Connection.Info.Force.Fullscreen) == Connection.Info.Force.Fullscreen)
				{
                    ICAClient.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, Screen.FromControl(frmMain.Default).Bounds.Width, Screen.FromControl(frmMain.Default).Bounds.Height, 0);
					ICAClient.FullScreenWindow();
							
					return;
				}
						
				if (this.InterfaceControl.Info.Resolution == RDP.RDPResolutions.FitToWindow)
				{
					ICAClient.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, this.InterfaceControl.Size.Width, this.InterfaceControl.Size.Height, 0);
				}
				else if (this.InterfaceControl.Info.Resolution == RDP.RDPResolutions.SmartSize)
				{
					ICAClient.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, this.InterfaceControl.Size.Width, this.InterfaceControl.Size.Height, 0);
				}
				else if (this.InterfaceControl.Info.Resolution == RDP.RDPResolutions.Fullscreen)
				{
					ICAClient.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, Screen.FromControl(frmMain.Default).Bounds.Width, Screen.FromControl(frmMain.Default).Bounds.Height, 0);
					ICAClient.FullScreenWindow();
				}
				else
				{
					Rectangle resolution = RDP.GetResolutionRectangle(Info.Resolution);
					ICAClient.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, resolution.Width, resolution.Height, 0);
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strIcaSetResolutionFailed + Constants.vbNewLine + ex.Message, true);
			}
		}
				
		private void SetColors()
		{
			switch (Info.Colors)
			{
				case RDP.RDPColors.Colors256:
					ICAClient.SetProp("DesiredColor", "2");
					break;
				case RDP.RDPColors.Colors15Bit:
					ICAClient.SetProp("DesiredColor", "4");
					break;
				case RDP.RDPColors.Colors16Bit:
					ICAClient.SetProp("DesiredColor", "4");
					break;
				default:
					ICAClient.SetProp("DesiredColor", "8");
					break;
			}
		}
				
		private void SetSecurity()
		{
			switch (Info.ICAEncryption)
			{
				case EncryptionStrength.Encr128BitLogonOnly:
					ICAClient.Encrypt = true;
					ICAClient.EncryptionLevelSession = "EncRC5-0";
					break;
				case EncryptionStrength.Encr40Bit:
					ICAClient.Encrypt = true;
					ICAClient.EncryptionLevelSession = "EncRC5-40";
					break;
				case EncryptionStrength.Encr56Bit:
					ICAClient.Encrypt = true;
					ICAClient.EncryptionLevelSession = "EncRC5-56";
					break;
				case EncryptionStrength.Encr128Bit:
					ICAClient.Encrypt = true;
					ICAClient.EncryptionLevelSession = "EncRC5-128";
					break;
			}
		}
				
		private void SetEventHandlers()
		{
			try
			{
				ICAClient.OnConnecting += new System.EventHandler(ICAEvent_OnConnecting);
				ICAClient.OnConnect += new System.EventHandler(ICAEvent_OnConnected);
				ICAClient.OnConnectFailed += new System.EventHandler(ICAEvent_OnConnectFailed);
				ICAClient.OnDisconnect += new System.EventHandler(ICAEvent_OnDisconnect);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strIcaSetEventHandlersFailed + Constants.vbNewLine + ex.Message, true);
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
				ICAClient.Default.Connect();
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