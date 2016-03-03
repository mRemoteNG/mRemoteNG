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

//using mRemoteNG.App.Native;
using System.Threading;
//using mRemoteNG.App.Runtime;
using mRemoteNG.Tools;


namespace mRemoteNG.Connection.Protocol
{
	public class IntegratedProgram : Base
			{
#region Public Methods
				public override bool SetProps()
				{
					if (InterfaceControl.Info != null)
					{
						_externalTool = GetExtAppByName(System.Convert.ToString(InterfaceControl.Info.ExtApp));
						_externalTool.ConnectionInfo = InterfaceControl.Info;
					}
					
					return base.SetProps();
				}
				
				public override bool Connect()
				{
					try
					{
						if (_externalTool.TryIntegrate == false)
						{
							_externalTool.Start(InterfaceControl.Info);
							Close();
							return null;
						}
						
						_process = new Process();
						
						_process.StartInfo.UseShellExecute = true;
						_process.StartInfo.FileName = _externalTool.ParseArguments(_externalTool.FileName);
						_process.StartInfo.Arguments = _externalTool.ParseArguments(_externalTool.Arguments);
						
						_process.EnableRaisingEvents = true;
						_process.Exited += ProcessExited;
						
						_process.Start();
						_process.WaitForInputIdle(System.Convert.ToInt32(My.Settings.Default.MaxPuttyWaitTime * 1000));
						
						int startTicks = Environment.TickCount;
						while (_handle.ToInt32() == 0 & Environment.TickCount < startTicks + (My.Settings.Default.MaxPuttyWaitTime * 1000))
						{
							_process.Refresh();
							if (!(_process.MainWindowTitle == "Default IME"))
							{
								_handle = _process.MainWindowHandle;
							}
							if (_handle.ToInt32() == 0)
							{
								Thread.Sleep(0);
							}
						}
						
						SetParent(_handle, InterfaceControl.Handle);
						
						MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, My.Language.strIntAppStuff, true);
						
						MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, string.Format(My.Language.strIntAppHandle, _handle.ToString()), true);
						MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, string.Format(My.Language.strIntAppTitle, _process.MainWindowTitle), true);
						MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, string.Format(My.Language.strIntAppParentHandle, InterfaceControl.Parent.Handle.ToString()), true);
						
						Resize(this, new EventArgs());
						
						base.Connect();
						return true;
					}
					catch (Exception ex)
					{
						MessageCollector.AddExceptionMessage(My.Language.strIntAppConnectionFailed, ex);
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
						SetForegroundWindow(_handle);
					}
					catch (Exception ex)
					{
						MessageCollector.AddExceptionMessage(message: My.Language.strIntAppFocusFailed, ex: ex, logOnly: true);
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
						MoveWindow(_handle, System.Convert.ToInt32(- SystemInformation.FrameBorderSize.Width), System.Convert.ToInt32(- (SystemInformation.CaptionHeight + SystemInformation.FrameBorderSize.Height)), InterfaceControl.Width + (SystemInformation.FrameBorderSize.Width * 2), InterfaceControl.Height + SystemInformation.CaptionHeight + (SystemInformation.FrameBorderSize.Height * 2), true);
					}
					catch (Exception ex)
					{
						MessageCollector.AddExceptionMessage(message: My.Language.strIntAppResizeFailed, ex: ex, logOnly: true);
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
						MessageCollector.AddExceptionMessage(message: My.Language.strIntAppKillFailed, ex: ex, logOnly: true);
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
						MessageCollector.AddExceptionMessage(message: My.Language.strIntAppDisposeFailed, ex: ex, logOnly: true);
					}
					
					base.Close();
				}
#endregion
				
#region Private Fields
				private ExternalTool _externalTool;
				private IntPtr _handle;
				private Process _process;
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
