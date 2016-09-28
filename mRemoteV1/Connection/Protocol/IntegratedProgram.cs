using mRemoteNG.App;
using mRemoteNG.Tools;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;


namespace mRemoteNG.Connection.Protocol
{
	public class IntegratedProgram : ProtocolBase
	{
        #region Private Fields
        private ExternalTool _externalTool;
        private IntPtr _handle;
        private Process _process;
        #endregion

        #region Public Methods
		public override bool Initialize()
		{
			if (InterfaceControl.Info != null)
			{
				_externalTool = Runtime.GetExtAppByName(Convert.ToString(InterfaceControl.Info.ExtApp));
				_externalTool.ConnectionInfo = InterfaceControl.Info;
			}
					
			return base.Initialize();
		}
				
		public override bool Connect()
		{
			try
			{
				if (_externalTool.TryIntegrate == false)
				{
					_externalTool.Start(InterfaceControl.Info);
					Close();
					return false;
				}

                ExternalToolArgumentParser argParser = new ExternalToolArgumentParser(_externalTool.ConnectionInfo);
				_process = new Process();
						
				_process.StartInfo.UseShellExecute = true;
                _process.StartInfo.FileName = argParser.ParseArguments(_externalTool.FileName);
                _process.StartInfo.Arguments = argParser.ParseArguments(_externalTool.Arguments);
						
				_process.EnableRaisingEvents = true;
				_process.Exited += ProcessExited;
						
				_process.Start();
				_process.WaitForInputIdle(Convert.ToInt32(Settings.Default.MaxPuttyWaitTime * 1000));
						
				int startTicks = Environment.TickCount;
				while (_handle.ToInt32() == 0 & Environment.TickCount < startTicks + (Settings.Default.MaxPuttyWaitTime * 1000))
				{
					_process.Refresh();
					if (_process.MainWindowTitle != "Default IME")
					{
						_handle = _process.MainWindowHandle;
					}
					if (_handle.ToInt32() == 0)
					{
						Thread.Sleep(0);
					}
				}
						
				NativeMethods.SetParent(_handle, InterfaceControl.Handle);
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, Language.strIntAppStuff, true);
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, string.Format(Language.strIntAppHandle, _handle.ToString()), true);
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, string.Format(Language.strIntAppTitle, _process.MainWindowTitle), true);
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, string.Format(Language.strIntAppParentHandle, InterfaceControl.Parent.Handle.ToString()), true);
						
				Resize(this, new EventArgs());
				base.Connect();
				return true;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage(Language.strIntAppConnectionFailed, ex);
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
				NativeMethods.SetForegroundWindow(_handle);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage(message: Language.strIntAppFocusFailed, ex: ex, logOnly: true);
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
                NativeMethods.MoveWindow(_handle, Convert.ToInt32(-SystemInformation.FrameBorderSize.Width), Convert.ToInt32(-(SystemInformation.CaptionHeight + SystemInformation.FrameBorderSize.Height)), InterfaceControl.Width + (SystemInformation.FrameBorderSize.Width * 2), InterfaceControl.Height + SystemInformation.CaptionHeight + (SystemInformation.FrameBorderSize.Height * 2), true);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage(message: Language.strIntAppResizeFailed, ex: ex, logOnly: true);
			}
		}
				
		public override void Close()
		{
			try
			{
				if (!_process.HasExited)
				{
					_process.Kill();
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage(message: Language.strIntAppKillFailed, ex: ex, logOnly: true);
			}
					
			try
			{
				if (!_process.HasExited)
				{
					_process.Dispose();
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage(message: Language.strIntAppDisposeFailed, ex: ex, logOnly: true);
			}
					
			base.Close();
		}
        #endregion
        
        #region Private Methods
		private void ProcessExited(object sender, EventArgs e)
		{
			Event_Closed(this);
		}
        #endregion
		
        #region Enumerations
		public enum Defaults
		{
			Port = 0
		}
        #endregion
	}
}