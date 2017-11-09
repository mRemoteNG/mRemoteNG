using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Tools;

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
		    if (InterfaceControl.Info == null)
				return base.Initialize();

		    _externalTool = Runtime.ExternalToolsService.GetExtAppByName(InterfaceControl.Info.ExtApp);

			if (_externalTool == null)
			{
				Runtime.MessageCollector?.AddMessage(MessageClass.ErrorMsg, string.Format(Language.CouldNotFindExternalTool, InterfaceControl.Info.ExtApp));
				return false;
			}

			_externalTool.ConnectionInfo = InterfaceControl.Info;

		    return base.Initialize();
		}
				
		public override bool Connect()
		{
			try
			{
                Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg, $"Attempting to start: {_externalTool.DisplayName}", true);

                if (_externalTool.TryIntegrate == false)
				{
					_externalTool.Start(InterfaceControl.Info);
                    /* Don't call close here... There's nothing for the override to do in this case since 
                     * _process is not created in this scenario. When returning false, ProtocolBase.Close()
                     * will be called - which is just going to call IntegratedProgram.Close() again anyway...
                     * Close();
                     */
                    Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg, $"Assuming no other errors/exceptions occurred immediately before this message regarding {_externalTool.DisplayName}, the next \"closed by user\" message can be ignored", true);
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
				Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg, Language.strIntAppStuff, true);
				Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg, string.Format(Language.strIntAppHandle, _handle), true);
				Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg, string.Format(Language.strIntAppTitle, _process.MainWindowTitle), true);
				Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg, string.Format(Language.strIntAppParentHandle, InterfaceControl.Parent.Handle), true);
						
				Resize(this, new EventArgs());
				base.Connect();
				return true;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector?.AddExceptionMessage(Language.strIntAppConnectionFailed, ex);
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
            /* only attempt this if we have a valid process object
             * Non-integated tools will still call base.Close() and don't have a valid process object.
             * See Connect() above... This just muddies up the log.
             */
            if (_process != null)
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