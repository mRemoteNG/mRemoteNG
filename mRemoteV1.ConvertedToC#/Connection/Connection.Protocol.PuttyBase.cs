using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel;
using mRemoteNG.Messages;
using mRemoteNG.App.Native;
using System.Threading;
using Microsoft.Win32;
using mRemoteNG.App.Runtime;
using System.Text.RegularExpressions;
using mRemoteNG.Tools;

namespace mRemoteNG.Connection
{
	namespace Protocol
	{
		public class PuttyBase : Connection.Protocol.Base
		{

			#region "Constants"
				// PuTTY Settings Menu ID
			private const Int32 IDM_RECONF = 0x50;
			#endregion

			#region "Private Properties"
				#endregion
			bool _isPuttyNg;

			#region "Public Properties"
			private Putty_Protocol _PuttyProtocol;
			public Putty_Protocol PuttyProtocol {
				get { return this._PuttyProtocol; }
				set { this._PuttyProtocol = value; }
			}

			private Putty_SSHVersion _PuttySSHVersion;
			public Putty_SSHVersion PuttySSHVersion {
				get { return this._PuttySSHVersion; }
				set { this._PuttySSHVersion = value; }
			}

			private IntPtr _PuttyHandle;
			public IntPtr PuttyHandle {
				get { return this._PuttyHandle; }
				set { this._PuttyHandle = value; }
			}

			private Process _PuttyProcess;
			public Process PuttyProcess {
				get { return this._PuttyProcess; }
				set { this._PuttyProcess = value; }
			}

			private static string _PuttyPath;
			public static string PuttyPath {
				get { return _PuttyPath; }
				set { _PuttyPath = value; }
			}

			public bool Focused {
				get {
					if (mRemoteNG.App.Native.GetForegroundWindow() == PuttyHandle) {
						return true;
					} else {
						return false;
					}
				}
			}
			#endregion

			#region "Private Events & Handlers"
			private void ProcessExited(object sender, System.EventArgs e)
			{
				base.Event_Closed(this);
			}
			#endregion

			#region "Public Methods"

			public PuttyBase()
			{
			}

			public override bool Connect()
			{
				try {
					_isPuttyNg = (PuttyTypeDetector.GetPuttyType() == PuttyTypeDetector.PuttyType.PuttyNg);

					PuttyProcess = new Process();
					var _with1 = PuttyProcess.StartInfo;
					_with1.UseShellExecute = false;
					_with1.FileName = _PuttyPath;

					CommandLineArguments arguments = new CommandLineArguments();
					arguments.EscapeForShell = false;

					arguments.Add("-load", InterfaceControl.Info.PuttySession);

					if (!InterfaceControl.Info is PuttySession.Info) {
						arguments.Add("-" + _PuttyProtocol.ToString());

						if (_PuttyProtocol == Putty_Protocol.ssh) {
							string username = "";
							string password = "";

							if (!string.IsNullOrEmpty(InterfaceControl.Info.Username)) {
								username = InterfaceControl.Info.Username;
							} else {
								if (mRemoteNG.My.Settings.EmptyCredentials == "windows") {
									username = Environment.UserName;
								} else if (mRemoteNG.My.Settings.EmptyCredentials == "custom") {
									username = mRemoteNG.My.Settings.DefaultUsername;
								}
							}

							if (!string.IsNullOrEmpty(InterfaceControl.Info.Password)) {
								password = InterfaceControl.Info.Password;
							} else {
								if (mRemoteNG.My.Settings.EmptyCredentials == "custom") {
									password = mRemoteNG.Security.Crypt.Decrypt(mRemoteNG.My.Settings.DefaultPassword, mRemoteNG.App.Info.General.EncryptionKey);
								}
							}

							arguments.Add("-" + _PuttySSHVersion);

							if (!((Force & Info.Force.NoCredentials) == Info.Force.NoCredentials)) {
								if (!string.IsNullOrEmpty(username)) {
									arguments.Add("-l", username);
								}
								if (!string.IsNullOrEmpty(password)) {
									arguments.Add("-pw", password);
								}
							}
						}

						arguments.Add("-P", InterfaceControl.Info.Port.ToString());
						arguments.Add(InterfaceControl.Info.Hostname);
					}

					if (_isPuttyNg) {
						arguments.Add("-hwndparent", InterfaceControl.Handle.ToString());
					}

					_with1.Arguments = arguments.ToString();

					PuttyProcess.EnableRaisingEvents = true;
					PuttyProcess.Exited += ProcessExited;

					PuttyProcess.Start();
					PuttyProcess.WaitForInputIdle(mRemoteNG.My.Settings.MaxPuttyWaitTime * 1000);

					int startTicks = Environment.TickCount;
					while (PuttyHandle.ToInt32() == 0 & Environment.TickCount < startTicks + (mRemoteNG.My.Settings.MaxPuttyWaitTime * 1000)) {
						if (_isPuttyNg) {
							PuttyHandle = mRemoteNG.App.Native.FindWindowEx(InterfaceControl.Handle, 0, Constants.vbNullString, Constants.vbNullString);
						} else {
							PuttyProcess.Refresh();
							PuttyHandle = PuttyProcess.MainWindowHandle;
						}
						if (PuttyHandle.ToInt32() == 0)
							Thread.Sleep(0);
					}

					if (!_isPuttyNg) {
						mRemoteNG.App.Native.SetParent(PuttyHandle, InterfaceControl.Handle);
					}

					mRemoteNG.App.Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, mRemoteNG.My.Language.strPuttyStuff, true);

					mRemoteNG.App.Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format(mRemoteNG.My.Language.strPuttyHandle, PuttyHandle.ToString()), true);
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format(mRemoteNG.My.Language.strPuttyTitle, PuttyProcess.MainWindowTitle), true);
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format(mRemoteNG.My.Language.strPuttyParentHandle, InterfaceControl.Parent.Handle.ToString()), true);

					Resize(this, new EventArgs());

					base.Connect();
					return true;
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, mRemoteNG.My.Language.strPuttyConnectionFailed + Constants.vbNewLine + ex.Message);
					return false;
				}
			}

			public override void Focus()
			{
				try {
					if (ConnectionWindow.InTabDrag)
						return;
					mRemoteNG.App.Native.SetForegroundWindow(PuttyHandle);
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strPuttyFocusFailed + Constants.vbNewLine + ex.Message, true);
				}
			}

			public override void Resize(object sender, EventArgs e)
			{
				try {
					if (InterfaceControl.Size == Size.Empty)
						return;
					mRemoteNG.App.Native.MoveWindow(PuttyHandle, -SystemInformation.FrameBorderSize.Width, -(SystemInformation.CaptionHeight + SystemInformation.FrameBorderSize.Height), InterfaceControl.Width + (SystemInformation.FrameBorderSize.Width * 2), InterfaceControl.Height + SystemInformation.CaptionHeight + (SystemInformation.FrameBorderSize.Height * 2), true);
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strPuttyResizeFailed + Constants.vbNewLine + ex.Message, true);
				}
			}

			public override void Close()
			{
				try {
					if (PuttyProcess.HasExited == false) {
						PuttyProcess.Kill();
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strPuttyKillFailed + Constants.vbNewLine + ex.Message, true);
				}

				try {
					PuttyProcess.Dispose();
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strPuttyDisposeFailed + Constants.vbNewLine + ex.Message, true);
				}

				base.Close();
			}

			public void ShowSettingsDialog()
			{
				try {
					mRemoteNG.App.Native.PostMessage(this.PuttyHandle, mRemoteNG.App.Native.WM_SYSCOMMAND, IDM_RECONF, 0);
					mRemoteNG.App.Native.SetForegroundWindow(this.PuttyHandle);
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strPuttyShowSettingsDialogFailed + Constants.vbNewLine + ex.Message, true);
				}
			}
			#endregion

			#region "Enums"
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
}
