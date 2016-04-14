using System;
using System.Diagnostics;
using Microsoft.VisualBasic.CompilerServices;

namespace mRemoteNG.My
{
	// The following events are available for MyApplication:
	//
	// Startup: Raised when the application starts, before the startup form is created.
	// Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
	// UnhandledException: Raised if the application encounters an unhandled exception.
	// StartupNextInstance: Raised when launching a single-instance application and the application is already active.
	// NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
	partial class MyApplication : Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase
	{
		public System.Threading.Mutex mutex;
			
		private void MyApplication_Startup(object sender, Microsoft.VisualBasic.ApplicationServices.StartupEventArgs e)
		{
			if (My.Settings.Default.SingleInstance)
			{
				string mutexID = "mRemoteNG_SingleInstanceMutex";
				mutex = new System.Threading.Mutex(false, mutexID);
					
				if (!mutex.WaitOne(0, false))
				{
					try
					{
						SwitchToCurrentInstance();
					}
					catch (Exception)
					{
					}
						
					ProjectData.EndApp();
				}
					
				GC.KeepAlive(mutex);
			}
		}
			
		private IntPtr GetCurrentInstanceWindowHandle()
		{
			IntPtr hWnd = IntPtr.Zero;
			Process curProc = Process.GetCurrentProcess();
			foreach (Process proc in Process.GetProcessesByName(curProc.ProcessName))
			{
				if (proc.Id != curProc.Id && proc.MainModule.FileName == curProc.MainModule.FileName && proc.MainWindowHandle != IntPtr.Zero)
				{
					hWnd = proc.MainWindowHandle;
					break;
				}
			}
			return hWnd;
		}
			
		private void SwitchToCurrentInstance()
		{
			IntPtr hWnd = GetCurrentInstanceWindowHandle();
			if (hWnd != IntPtr.Zero)
			{
				//Restore window if minimized. Do not restore if already in
				//normal or maximised window state, since we don't want to
				//change the current state of the window.
				if (App.Native.IsIconic(hWnd) != 0)
				{
					App.Native.ShowWindow(hWnd, App.Native.SW_RESTORE);
				}
				App.Native.SetForegroundWindow(hWnd);
			}
		}
			
		private void MyApplication_Shutdown(object sender, System.EventArgs e)
		{
			if (mutex != null)
			{
				mutex.Close();
			}
		}
	}
}
