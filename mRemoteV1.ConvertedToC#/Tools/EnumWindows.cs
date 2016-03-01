using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace mRemoteNG.Tools
{
	public class EnumWindows
	{
		public static List<IntPtr> EnumWindows()
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
			public delegate bool EnumWindowsProc(int hwnd, int lParam);
			[DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
			public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, int lParam);
			[DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
			public static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowsProc lpEnumFunc, int lParam);
		}
	}
}
