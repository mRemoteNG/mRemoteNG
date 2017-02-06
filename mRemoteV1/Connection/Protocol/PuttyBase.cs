using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Tools;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools.Cmdline;


namespace mRemoteNG.Connection.Protocol
{
	public class PuttyBase : ProtocolBase
	{	
		private const int IDM_RECONF = 0x50; // PuTTY Settings Menu ID
	    private bool _isPuttyNg;

	    #region Public Properties

	    protected Putty_Protocol PuttyProtocol { private get; set; }

        protected Putty_SSHVersion PuttySSHVersion { private get; set; }

	    private IntPtr PuttyHandle { get; set; }

	    private Process PuttyProcess { get; set; }

	    public static string PuttyPath { get; set; }

	    public bool Focused => NativeMethods.GetForegroundWindow() == PuttyHandle;

	    #endregion

	    #region Private Events & Handlers
		private void ProcessExited(object sender, EventArgs e)
		{
            Event_Closed(this);
		}
        #endregion
				
        #region Public Methods
		public override bool Connect()
		{
			try
			{
				_isPuttyNg = PuttyTypeDetector.GetPuttyType() == PuttyTypeDetector.PuttyType.PuttyNg;

			    PuttyProcess = new Process
			    {
			        StartInfo =
			        {
			            UseShellExecute = false,
			            FileName = PuttyPath
			        }
			    };

			    var arguments = new CommandLineArguments {EscapeForShell = false};

			    arguments.Add("-load", InterfaceControl.Info.PuttySession);
						
				if (!(InterfaceControl.Info is PuttySessionInfo))
				{
					arguments.Add("-" + PuttyProtocol);
							
					if (PuttyProtocol == Putty_Protocol.ssh)
					{
						var username = "";
						var password = "";
								
						if (!string.IsNullOrEmpty(InterfaceControl.Info.CredentialRecord?.Username))
						{
							username = InterfaceControl.Info.CredentialRecord?.Username;
						}
						else
						{
							if (Settings.Default.EmptyCredentials == "windows")
							{
								username = Environment.UserName;
							}
							else if (Settings.Default.EmptyCredentials == "custom")
							{
								username = Settings.Default.DefaultUsername;
							}
						}
								
						if (!string.IsNullOrEmpty(InterfaceControl.Info.CredentialRecord?.Password.ConvertToUnsecureString()))
						{
							password = InterfaceControl.Info.CredentialRecord?.Password.ConvertToUnsecureString();
						}
						else
						{
							if (Settings.Default.EmptyCredentials == "custom")
							{
                                var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
                                password = cryptographyProvider.Decrypt(Settings.Default.DefaultPassword, Runtime.EncryptionKey);
							}
						}
								
						arguments.Add("-" + (int)PuttySSHVersion);
								
						if (((int)Force & (int)ConnectionInfo.Force.NoCredentials) != (int)ConnectionInfo.Force.NoCredentials)
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
				PuttyProcess.WaitForInputIdle(Settings.Default.MaxPuttyWaitTime * 1000);
						
				var startTicks = Environment.TickCount;
				while (PuttyHandle.ToInt32() == 0 & Environment.TickCount < startTicks + Settings.Default.MaxPuttyWaitTime * 1000)
				{
					if (_isPuttyNg)
					{
						PuttyHandle = NativeMethods.FindWindowEx(
                            InterfaceControl.Handle, new IntPtr(0), null, null);
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
					NativeMethods.SetParent(PuttyHandle, InterfaceControl.Handle);
				}
						
				Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.strPuttyStuff, true);
				Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format(Language.strPuttyHandle, PuttyHandle), true);
				Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format(Language.strPuttyTitle, PuttyProcess.MainWindowTitle), true);
				Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format(Language.strPuttyParentHandle, InterfaceControl.Parent.Handle), true);
						
				Resize(this, new EventArgs());
				base.Connect();
				return true;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strPuttyConnectionFailed + Environment.NewLine + ex.Message);
				return false;
			}
		}
				
		public override void Focus()
		{
			try
			{
				if (ConnectionWindow.InTabDrag)
				{
					return;
				}
				NativeMethods.SetForegroundWindow(PuttyHandle);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strPuttyFocusFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		public override void Resize(object sender, EventArgs e)
		{
			try
			{
				if (InterfaceControl.Size == Size.Empty)
				{
					return;
				}
                NativeMethods.MoveWindow(PuttyHandle, -SystemInformation.FrameBorderSize.Width, -(SystemInformation.CaptionHeight + SystemInformation.FrameBorderSize.Height), InterfaceControl.Width + SystemInformation.FrameBorderSize.Width * 2, InterfaceControl.Height + SystemInformation.CaptionHeight + SystemInformation.FrameBorderSize.Height * 2, true);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strPuttyResizeFailed + Environment.NewLine + ex.Message, true);
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
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strPuttyKillFailed + Environment.NewLine + ex.Message, true);
			}
					
			try
			{
				PuttyProcess.Dispose();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strPuttyDisposeFailed + Environment.NewLine + ex.Message, true);
			}
					
			base.Close();
		}
				
		public void ShowSettingsDialog()
		{
			try
			{
                NativeMethods.PostMessage(PuttyHandle, NativeMethods.WM_SYSCOMMAND, (IntPtr)IDM_RECONF, (IntPtr)0);
                NativeMethods.SetForegroundWindow(PuttyHandle);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strPuttyShowSettingsDialogFailed + Environment.NewLine + ex.Message, true);
			}
		}
        #endregion
				
        #region Enums

	    protected enum Putty_Protocol
		{
			ssh = 0,
			telnet = 1,
			rlogin = 2,
			raw = 3,
			serial = 4
		}

	    protected enum Putty_SSHVersion
		{
			ssh1 = 1,
			ssh2 = 2
		}
        #endregion
	}
}