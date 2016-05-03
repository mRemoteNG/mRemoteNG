using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;


namespace mRemoteNG.Tools
{
	public class ProcessController
	{
        #region Public Methods
		public bool Start(string fileName, CommandLineArguments arguments = null)
		{
			Process.StartInfo.UseShellExecute = false;
			Process.StartInfo.FileName = fileName;
			if (arguments != null)
			{
				Process.StartInfo.Arguments = arguments.ToString();
			}
				
			if (!Process.Start())
			{
				return false;
			}
			GetMainWindowHandle();
				
			return true;
		}
			
		public bool SetControlVisible(string className, string text, bool visible = true)
		{
			if (Process == null || Process.HasExited)
			{
				return false;
			}
			if (Handle == IntPtr.Zero)
			{
				return false;
			}
				
			IntPtr controlHandle = GetControlHandle(className, text);
			if (controlHandle == IntPtr.Zero)
			{
				return false;
			}
				
			int nCmdShow = 0;
			if (visible)
			{
				nCmdShow = Win32.SW_SHOW;
			}
			else
			{
				nCmdShow = Win32.SW_HIDE;
			}
			Win32.ShowWindow(controlHandle, nCmdShow);
			return true;
		}
			
		public bool SetControlText(string className, string oldText, string newText)
		{
			if (Process == null || Process.HasExited)
			{
				return false;
			}
			if (Handle == IntPtr.Zero)
			{
				return false;
			}
				
			IntPtr controlHandle = GetControlHandle(className, oldText);
			if (controlHandle == IntPtr.Zero)
			{
				return false;
			}
				
			IntPtr result = Win32.SendMessage(controlHandle, Win32.WM_SETTEXT, (IntPtr)0, new StringBuilder(newText));
			if (!(result.ToInt32() == Win32.TRUE))
			{
				return false;
			}
				
			return true;
		}
			
		public bool SelectListBoxItem(string itemText)
		{
			if (Process == null || Process.HasExited)
			{
				return false;
			}
			if (Handle == IntPtr.Zero)
			{
				return false;
			}
				
			IntPtr listBoxHandle = GetControlHandle("ListBox");
			if (listBoxHandle == IntPtr.Zero)
			{
				return false;
			}
				
			IntPtr result = Win32.SendMessage(listBoxHandle, Win32.LB_SELECTSTRING, (IntPtr)(-1), new StringBuilder(itemText));
			if (result.ToInt32() == Win32.LB_ERR)
			{
				return false;
			}
				
			return true;
		}
			
		public bool ClickButton(string text)
		{
			if (Process == null || Process.HasExited)
			{
				return false;
			}
			if (Handle == IntPtr.Zero)
			{
				return false;
			}
				
			IntPtr buttonHandle = GetControlHandle("Button", text);
			if (buttonHandle == IntPtr.Zero)
			{
				return false;
			}
				
			int buttonControlId = Win32.GetDlgCtrlID(buttonHandle.ToInt32());
			Win32.SendMessage(Handle, Win32.WM_COMMAND, (IntPtr)buttonControlId, buttonHandle);
				
			return true;
		}
			
		public void WaitForExit()
		{
			if (Process == null || Process.HasExited)
			{
				return ;
			}
			Process.WaitForExit();
		}
#endregion
			
#region Protected Fields
		protected Process Process = new Process();
		protected IntPtr Handle = IntPtr.Zero;
		protected List<IntPtr> Controls = new List<IntPtr>();
#endregion
			
#region Protected Methods
		protected IntPtr GetMainWindowHandle()
		{
			if (Process == null || Process.HasExited)
			{
				return IntPtr.Zero;
			}
			
			Process.WaitForInputIdle(My.Settings.Default.MaxPuttyWaitTime * 1000);
			
			Handle = IntPtr.Zero;
			int startTicks = Environment.TickCount;
            while (Handle == IntPtr.Zero && Environment.TickCount < startTicks + (My.Settings.Default.MaxPuttyWaitTime * 1000))
			{
				Process.Refresh();
				Handle = Process.MainWindowHandle;
				if (Handle == IntPtr.Zero)
				{
					System.Threading.Thread.Sleep(0);
				}
			}
				
			return Handle;
		}
			
		protected IntPtr GetControlHandle(string className, string text = "")
		{
			if (Process == null || Process.HasExited)
			{
				return IntPtr.Zero;
			}
			if (Handle == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}
				
			if (Controls.Count == 0)
			{
                EnumWindows windowEnumerator = new EnumWindows();
                Controls = windowEnumerator.EnumChildWindows(Handle);
			}
				
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			IntPtr controlHandle = IntPtr.Zero;
			foreach (IntPtr control in Controls)
			{
				Win32.GetClassName(control, stringBuilder, stringBuilder.Capacity);
				if (stringBuilder.ToString() == className)
				{
					if (string.IsNullOrEmpty(text))
					{
						controlHandle = control;
						break;
					}
					else
					{
						Win32.SendMessage(control, Win32.WM_GETTEXT, new IntPtr(stringBuilder.Capacity), stringBuilder);
						if (stringBuilder.ToString() == text)
						{
							controlHandle = control;
							break;
						}
					}
				}
			}
				
			return controlHandle;
		}
#endregion
			
#region Win32
		// ReSharper disable ClassNeverInstantiated.Local
		private class Win32
		{
			// ReSharper restore ClassNeverInstantiated.Local
			// ReSharper disable InconsistentNaming
			// ReSharper disable UnusedMethodReturnValue.Local
			[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]public static  extern void GetClassName(IntPtr hWnd, System.Text.StringBuilder lpClassName, int nMaxCount);
				
			[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]public static  extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
				
			[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]public static  extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, System.Text.StringBuilder lParam);
				
			[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]public static  extern int GetDlgCtrlID(int hwndCtl);
				
			[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]public static  extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
				
			public const int LB_ERR = -1;
			public const int LB_SELECTSTRING = 0x18C;
				
			public const int WM_SETTEXT = 0xC;
			public const int WM_GETTEXT = 0xD;
			public const int WM_COMMAND = 0x111;
				
			public const int SW_HIDE = 0;
			public const int SW_SHOW = 5;
				
			public const int TRUE = 1;
			// ReSharper restore UnusedMethodReturnValue.Local
			// ReSharper restore InconsistentNaming
		}
#endregion
	}
}
