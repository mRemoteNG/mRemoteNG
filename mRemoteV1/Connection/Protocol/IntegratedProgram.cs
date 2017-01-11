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
		    if (InterfaceControl.Info == null) return base.Initialize();

		    _externalTool = Runtime.GetExtAppByName(InterfaceControl.Info.ExtApp);
		    _externalTool.ConnectionInfo = InterfaceControl.Info;

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

                var argParser = new ExternalToolArgumentParser(_externalTool.ConnectionInfo);
			    _process = new Process
			    {
			        StartInfo =
			        {
			            UseShellExecute = true,
			            FileName = argParser.ParseArguments(_externalTool.FileName),
			            Arguments = argParser.ParseArguments(_externalTool.Arguments)
			        },
			        EnableRaisingEvents = true
			    };


			    _process.Exited += ProcessExited;
						
				_process.Start();
				_process.WaitForInputIdle(Settings.Default.MaxPuttyWaitTime * 1000);
						
				var startTicks = Environment.TickCount;
				while (_handle.ToInt32() == 0 & Environment.TickCount < startTicks + Settings.Default.MaxPuttyWaitTime * 1000)
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
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, string.Format(Language.strIntAppHandle, _handle), true);
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, string.Format(Language.strIntAppTitle, _process.MainWindowTitle), true);
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, string.Format(Language.strIntAppParentHandle, InterfaceControl.Parent.Handle), true);
						
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
				if (ConnectionWindow.InTabDrag) return;
				NativeMethods.SetForegroundWindow(_handle);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage(Language.strIntAppFocusFailed, ex);
			}
		}
				
		public override void Resize(object sender, EventArgs e)
		{
			try
			{
				if (InterfaceControl.Size == Size.Empty) return;
                NativeMethods.MoveWindow(_handle, -SystemInformation.FrameBorderSize.Width, -(SystemInformation.CaptionHeight + SystemInformation.FrameBorderSize.Height), InterfaceControl.Width + SystemInformation.FrameBorderSize.Width * 2, InterfaceControl.Height + SystemInformation.CaptionHeight + SystemInformation.FrameBorderSize.Height * 2, true);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage(Language.strIntAppResizeFailed, ex);
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
				Runtime.MessageCollector.AddExceptionMessage(Language.strIntAppKillFailed, ex);
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
				Runtime.MessageCollector.AddExceptionMessage(Language.strIntAppDisposeFailed, ex);
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