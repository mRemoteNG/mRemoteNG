using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;

namespace mRemoteNG.Tools
{
	public class EnumWindows
	{
	    public List<IntPtr> EnumWindows_Renamed()
		{
			var handleList = new List<IntPtr>();
				
			HandleLists.Add(handleList);
			var handleIndex = (IntPtr)HandleLists.IndexOf(handleList);
			NativeMethods.EnumWindows(EnumCallback, handleIndex);
			HandleLists.Remove(handleList);
				
			return handleList;
		}
			
		public List<IntPtr> EnumChildWindows(IntPtr hWndParent)
		{
			var handleList = new List<IntPtr>();
				
			HandleLists.Add(handleList);
            var handleIndex = (IntPtr)HandleLists.IndexOf(handleList);
			NativeMethods.EnumChildWindows(hWndParent, EnumCallback, handleIndex);
			HandleLists.Remove(handleList);
				
			return handleList;
		}
			
		private readonly List<List<IntPtr>> HandleLists = new List<List<IntPtr>>();
			
		private bool EnumCallback(int hwnd, int lParam)
		{
			HandleLists[lParam].Add((IntPtr)hwnd);
			return true;
		}
			
		// ReSharper disable ClassNeverInstantiated.Local
		private class NativeMethods
		{
			// ReSharper restore ClassNeverInstantiated.Local
			public delegate bool EnumWindowsProc(int hwnd, int lParam);
			[DllImport("user32", ExactSpelling=true, CharSet=CharSet.Ansi, SetLastError=true)]
			public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
			[DllImport("user32", ExactSpelling=true, CharSet=CharSet.Ansi, SetLastError=true)]
			public static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);
		}
	}
}
