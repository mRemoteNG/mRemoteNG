using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.App.Native;
using System.Threading;
using mRemoteNG.App.Runtime;
using mRemoteNG.Tools;

namespace mRemoteNG.Connection
{
	namespace Protocol
	{
		public class IntegratedProgram : Base
		{
			#region "Public Methods"
			public override bool SetProps()
			{
				if (InterfaceControl.Info != null) {
					_externalTool = mRemoteNG.App.Runtime.GetExtAppByName(InterfaceControl.Info.ExtApp);
					_externalTool.ConnectionInfo = InterfaceControl.Info;
				}

				return base.SetProps();
			}

			public override bool Connect()
			{
				try {
					if (_externalTool.TryIntegrate == false) {
						_externalTool.Start(InterfaceControl.Info);
						Close();
						return null;
					}

					_process = new Process();

					var _with1 = _process.StartInfo;
					_with1.UseShellExecute = true;
					_with1.FileName = _externalTool.ParseArguments(_externalTool.FileName);
					_with1.Arguments = _externalTool.ParseArguments(_externalTool.Arguments);

					_process.EnableRaisingEvents = true;
					_process.Exited += ProcessExited;

					_process.Start();
					_process.WaitForInputIdle(mRemoteNG.My.Settings.MaxPuttyWaitTime * 1000);

					int startTicks = Environment.TickCount;
					while (_handle.ToInt32() == 0 & Environment.TickCount < startTicks + (mRemoteNG.My.Settings.MaxPuttyWaitTime * 1000)) {
						_process.Refresh();
						if (!(_process.MainWindowTitle == "Default IME"))
							_handle = _process.MainWindowHandle;
						if (_handle.ToInt32() == 0)
							Thread.Sleep(0);
					}

					mRemoteNG.App.Native.SetParent(_handle, InterfaceControl.Handle);

					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.InformationMsg, mRemoteNG.My.Language.strIntAppStuff, true);

					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.InformationMsg, string.Format(mRemoteNG.My.Language.strIntAppHandle, _handle.ToString()), true);
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.InformationMsg, string.Format(mRemoteNG.My.Language.strIntAppTitle, _process.MainWindowTitle), true);
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.InformationMsg, string.Format(mRemoteNG.My.Language.strIntAppParentHandle, InterfaceControl.Parent.Handle.ToString()), true);

					Resize(this, new EventArgs());

					base.Connect();
					return true;
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddExceptionMessage(mRemoteNG.My.Language.strIntAppConnectionFailed, ex);
					return false;
				}
			}

			public override void Focus()
			{
				try {
					if (ConnectionWindow.InTabDrag)
						return;
					mRemoteNG.App.Native.SetForegroundWindow(_handle);
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddExceptionMessage(mRemoteNG.My.Language.strIntAppFocusFailed, ex, , true);
				}
			}

			public override void Resize(object sender, EventArgs e)
			{
				try {
					if (InterfaceControl.Size == Size.Empty)
						return;
					mRemoteNG.App.Native.MoveWindow(_handle, -SystemInformation.FrameBorderSize.Width, -(SystemInformation.CaptionHeight + SystemInformation.FrameBorderSize.Height), InterfaceControl.Width + (SystemInformation.FrameBorderSize.Width * 2), InterfaceControl.Height + SystemInformation.CaptionHeight + (SystemInformation.FrameBorderSize.Height * 2), true);
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddExceptionMessage(mRemoteNG.My.Language.strIntAppResizeFailed, ex, , true);
				}
			}

			public override void Close()
			{
				try {
					if (!_process.HasExited)
						_process.Kill();
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddExceptionMessage(mRemoteNG.My.Language.strIntAppKillFailed, ex, , true);
				}

				try {
					if (!_process.HasExited)
						_process.Dispose();
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddExceptionMessage(mRemoteNG.My.Language.strIntAppDisposeFailed, ex, , true);
				}

				base.Close();
			}
			#endregion

			#region "Private Fields"
			private ExternalTool _externalTool;
			private IntPtr _handle;
				#endregion
			private Process _process;

			#region "Private Methods"
			private void ProcessExited(object sender, EventArgs e)
			{
				Event_Closed(this);
			}
			#endregion

			#region "Enumerations"
			public enum Defaults
			{
				Port = 0
			}
			#endregion
		}
	}
}
