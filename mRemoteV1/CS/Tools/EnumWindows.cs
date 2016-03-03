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

using System.Runtime.InteropServices;

namespace mRemoteNG.Tools
{
	public class EnumWindows
		{
			public static List<IntPtr> EnumWindows_Renamed()
			{
				List<IntPtr> handleList = new List<IntPtr>();
				
				HandleLists.Add(handleList);
				int handleIndex = HandleLists.IndexOf(handleList);
				Win32.EnumWindows(EnumCallback, handleIndex);
				HandleLists.Remove(handleList);
				
				return handleList;
			}
			
			public static List<IntPtr> EnumChildWindows(IntPtr hWndParent)
			{
				List<IntPtr> handleList = new List<IntPtr>();
				
				HandleLists.Add(handleList);
				int handleIndex = HandleLists.IndexOf(handleList);
				Win32.EnumChildWindows(hWndParent, EnumCallback, handleIndex);
				HandleLists.Remove(handleList);
				
				return handleList;
			}
			
			private static readonly List<List<IntPtr>> HandleLists = new List<List<IntPtr>>();
			
			private static bool EnumCallback(int hwnd, int lParam)
			{
				HandleLists[lParam].Add(hwnd);
				return true;
			}
			
			// ReSharper disable ClassNeverInstantiated.Local
			private class Win32
			{
				// ReSharper restore ClassNeverInstantiated.Local
				public delegate bool  EnumWindowsProc(int hwnd, int lParam);
				[DllImport("user32", ExactSpelling=true, CharSet=CharSet.Ansi, SetLastError=true)]
				public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, int lParam);
				[DllImport("user32", ExactSpelling=true, CharSet=CharSet.Ansi, SetLastError=true)]
				public static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowsProc lpEnumFunc, int lParam);
			}
		}
}
