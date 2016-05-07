using Microsoft.VisualBasic;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Tools;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;


namespace mRemoteNG.Connection.Protocol
{
	public class PuttyBase : ProtocolBase
	{	
		private const int IDM_RECONF = 0x50; // PuTTY Settings Menu ID
		bool _isPuttyNg;
        private Putty_Protocol _PuttyProtocol;
        private Putty_SSHVersion _PuttySSHVersion;
        private IntPtr _PuttyHandle;
        private Process _PuttyProcess;
        private static string _PuttyPath;


        #region Public Properties
        public Putty_Protocol PuttyProtocol
		{
			get { return this._PuttyProtocol; }
			set { this._PuttyProtocol = value; }
		}
		
        public Putty_SSHVersion PuttySSHVersion
		{
			get { return this._PuttySSHVersion; }
			set { this._PuttySSHVersion = value; }
		}
		
        public IntPtr PuttyHandle
		{
			get { return this._PuttyHandle; }
			set { this._PuttyHandle = value; }
		}
		
        public Process PuttyProcess
		{
			get { return this._PuttyProcess; }
			set { this._PuttyProcess = value; }
		}
		
        public static string PuttyPath
		{
			get { return _PuttyPath; }
			set { _PuttyPath = value; }
		}
				
        public bool Focused
		{
			get
			{
				if (Native.GetForegroundWindow() == PuttyHandle)
					return true;
				return false;
			}
		}
        #endregion

        public PuttyBase()
        {

        }

        #region Private Events & Handlers
		private void ProcessExited(object sender, System.EventArgs e)
		{
            base.Event_Closed(this);
		}
        #endregion
				
        #region Public Methods
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
						
				if (!(InterfaceControl.Info is PuttySessionInfo))
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
								username = Convert.ToString(My.Settings.Default.DefaultUsername);
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
								password = Security.Crypt.Decrypt(Convert.ToString(My.Settings.Default.DefaultPassword), App.Info.GeneralAppInfo.EncryptionKey);
							}
						}
								
						arguments.Add("-" + (int)_PuttySSHVersion);
								
						if (!(((int)Force & (int)ConnectionInfo.Force.NoCredentials) == (int)ConnectionInfo.Force.NoCredentials))
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
				PuttyProcess.WaitForInputIdle(Convert.ToInt32(My.Settings.Default.MaxPuttyWaitTime * 1000));
						
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
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strPuttyFocusFailed + Environment.NewLine + ex.Message, true);
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
                Native.MoveWindow(PuttyHandle, Convert.ToInt32(-SystemInformation.FrameBorderSize.Width), Convert.ToInt32(-(SystemInformation.CaptionHeight + SystemInformation.FrameBorderSize.Height)), InterfaceControl.Width + (SystemInformation.FrameBorderSize.Width * 2), InterfaceControl.Height + SystemInformation.CaptionHeight + (SystemInformation.FrameBorderSize.Height * 2), true);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strPuttyResizeFailed + Environment.NewLine + ex.Message, true);
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
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strPuttyKillFailed + Environment.NewLine + ex.Message, true);
			}
					
			try
			{
				PuttyProcess.Dispose();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strPuttyDisposeFailed + Environment.NewLine + ex.Message, true);
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
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strPuttyShowSettingsDialogFailed + Environment.NewLine + ex.Message, true);
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