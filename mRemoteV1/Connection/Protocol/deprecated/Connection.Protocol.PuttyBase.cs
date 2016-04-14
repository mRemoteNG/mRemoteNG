using System;
using System.Drawing;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using mRemoteNG.Messages;
using System.Threading;
using mRemoteNG.App;
using mRemoteNG.Tools;


namespace mRemoteNG.Connection.Protocol
{
	public class PuttyBase : Base
	{	
        #region Constants
		private const int IDM_RECONF = 0x50; // PuTTY Settings Menu ID
        #endregion
				
        #region Private Properties
		bool _isPuttyNg;
        #endregion
				
        #region Public Properties
		private Putty_Protocol _PuttyProtocol;
        public Putty_Protocol PuttyProtocol
		{
			get
			{
				return this._PuttyProtocol;
			}
			set
			{
				this._PuttyProtocol = value;
			}
		}
				
		private Putty_SSHVersion _PuttySSHVersion;
        public Putty_SSHVersion PuttySSHVersion
		{
			get
			{
				return this._PuttySSHVersion;
			}
			set
			{
				this._PuttySSHVersion = value;
			}
		}
				
		private IntPtr _PuttyHandle;
        public IntPtr PuttyHandle
		{
			get
			{
				return this._PuttyHandle;
			}
			set
			{
				this._PuttyHandle = value;
			}
		}
				
		private Process _PuttyProcess;
        public Process PuttyProcess
		{
			get
			{
				return this._PuttyProcess;
			}
			set
			{
				this._PuttyProcess = value;
			}
		}
				
		private static string _PuttyPath;
        public static string PuttyPath
		{
			get
			{
				return _PuttyPath;
			}
			set
			{
				_PuttyPath = value;
			}
		}
				
        public bool Focused
		{
			get
			{
				if (Native.GetForegroundWindow() == PuttyHandle)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}
        #endregion
				
        #region Private Events & Handlers
		private void ProcessExited(object sender, System.EventArgs e)
		{
            base.Event_Closed(this);
		}
        #endregion
				
        #region Public Methods
		public PuttyBase()
		{
					
		}
				
		public override bool Connect()
		{
			try
			{
				_isPuttyNg = PuttyTypeDetector.GetPuttyType() == PuttyTypeDetector.PuttyType.PuttyNg;
						
				PuttyProcess = new Process();
				PuttyProcess.StartInfo.UseShellExecute = false;
				PuttyProcess.StartInfo.FileName = _PuttyPath;
						
				CommandLineArguments arguments = new CommandLineArguments();
				arguments.EscapeForShell = false;
						
				arguments.Add("-load", InterfaceControl.Info.PuttySession);
						
				if (!(InterfaceControl.Info is PuttySession.Info))
				{
					arguments.Add("-" + _PuttyProtocol.ToString());
							
					if (_PuttyProtocol == Putty_Protocol.ssh)
					{
						string username = "";
						string password = "";
								
						if (!string.IsNullOrEmpty(InterfaceControl.Info.Username))
						{
							username = InterfaceControl.Info.Username;
						}
						else
						{
							if (My.Settings.Default.EmptyCredentials == "windows")
							{
								username = Environment.UserName;
							}
							else if (My.Settings.Default.EmptyCredentials == "custom")
							{
								username = System.Convert.ToString(My.Settings.Default.DefaultUsername);
							}
						}
								
						if (!string.IsNullOrEmpty(InterfaceControl.Info.Password))
						{
							password = InterfaceControl.Info.Password;
						}
						else
						{
							if (My.Settings.Default.EmptyCredentials == "custom")
							{
								password = Security.Crypt.Decrypt(System.Convert.ToString(My.Settings.Default.DefaultPassword), App.Info.General.EncryptionKey);
							}
						}
								
						arguments.Add("-" + System.Convert.ToString(_PuttySSHVersion));
								
						if (!(((int)Force & (int)ConnectionRecordImp.Force.NoCredentials) == (int)ConnectionRecordImp.Force.NoCredentials))
						{
							if (!string.IsNullOrEmpty(username))
							{
								arguments.Add("-l", username);
							}
							if (!string.IsNullOrEmpty(password))
							{
								arguments.Add("-pw", password);
							}
						}
					}
							
					arguments.Add("-P", InterfaceControl.Info.Port.ToString());
					arguments.Add(InterfaceControl.Info.Hostname);
				}
						
				if (_isPuttyNg)
				{
					arguments.Add("-hwndparent", InterfaceControl.Handle.ToString());
				}
						
				PuttyProcess.StartInfo.Arguments = arguments.ToString();
						
				PuttyProcess.EnableRaisingEvents = true;
				PuttyProcess.Exited += ProcessExited;
						
				PuttyProcess.Start();
				PuttyProcess.WaitForInputIdle(System.Convert.ToInt32(My.Settings.Default.MaxPuttyWaitTime * 1000));
						
				int startTicks = Environment.TickCount;
				while (PuttyHandle.ToInt32() == 0 & Environment.TickCount < startTicks + (My.Settings.Default.MaxPuttyWaitTime * 1000))
				{
					if (_isPuttyNg)
					{
						PuttyHandle = Native.FindWindowEx(
                            InterfaceControl.Handle, new IntPtr(0), Constants.vbNullString, Constants.vbNullString);
					}
					else
					{
						PuttyProcess.Refresh();
						PuttyHandle = PuttyProcess.MainWindowHandle;
					}
					if (PuttyHandle.ToInt32() == 0)
					{
						Thread.Sleep(0);
					}
				}
						
				if (!_isPuttyNg)
				{
					Native.SetParent(PuttyHandle, InterfaceControl.Handle);
				}
						
				Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, My.Language.strPuttyStuff, true);
				Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format(My.Language.strPuttyHandle, PuttyHandle.ToString()), true);
				Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format(My.Language.strPuttyTitle, PuttyProcess.MainWindowTitle), true);
				Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format(My.Language.strPuttyParentHandle, InterfaceControl.Parent.Handle.ToString()), true);
						
				Resize(this, new EventArgs());
				base.Connect();
				return true;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strPuttyConnectionFailed + Environment.NewLine + ex.Message);
				return false;
			}
		}
				
		public override void Focus()
		{
			try
			{
				if (ConnectionWindow.InTabDrag)
				{
					return ;
				}
				Native.SetForegroundWindow(PuttyHandle);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strPuttyFocusFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		public override void Resize(object sender, EventArgs e)
		{
			try
			{
				if (InterfaceControl.Size == Size.Empty)
				{
					return ;
				}
                Native.MoveWindow(PuttyHandle, System.Convert.ToInt32(-SystemInformation.FrameBorderSize.Width), System.Convert.ToInt32(-(SystemInformation.CaptionHeight + SystemInformation.FrameBorderSize.Height)), InterfaceControl.Width + (SystemInformation.FrameBorderSize.Width * 2), InterfaceControl.Height + SystemInformation.CaptionHeight + (SystemInformation.FrameBorderSize.Height * 2), true);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strPuttyResizeFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		public override void Close()
		{
			try
			{
				if (PuttyProcess.HasExited == false)
				{
					PuttyProcess.Kill();
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strPuttyKillFailed + Environment.NewLine + ex.Message, true);
			}
					
			try
			{
				PuttyProcess.Dispose();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strPuttyDisposeFailed + Environment.NewLine + ex.Message, true);
			}
					
			base.Close();
		}
				
		public void ShowSettingsDialog()
		{
			try
			{
                Native.PostMessage(this.PuttyHandle, Native.WM_SYSCOMMAND, IDM_RECONF, 0);
                Native.SetForegroundWindow(this.PuttyHandle);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strPuttyShowSettingsDialogFailed + Environment.NewLine + ex.Message, true);
			}
		}
        #endregion
				
        #region Enums
		public enum Putty_Protocol
		{
			ssh = 0,
			telnet = 1,
			rlogin = 2,
			raw = 3,
			serial = 4
		}
				
		public enum Putty_SSHVersion
		{
			ssh1 = 1,
			ssh2 = 2
		}
        #endregion
	}
}
