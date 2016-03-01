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
	public class WindowPlacement
	{
		[DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		#region "Windows API"
		#region "Functions"
		private static extern bool GetWindowPlacement(System.IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);
		[DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		private static extern bool SetWindowPlacement(System.IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);
		#endregion

		#region "Structures"
		private struct WINDOWPLACEMENT
		{
			public uint length;
			public uint flags;
			public uint showCmd;
			public POINT ptMinPosition;
			public POINT ptMaxPosition;
			public RECT rcNormalPosition;
		}

		private struct POINT
		{
			public long x;
			public long y;
		}

		private struct RECT
		{
			public long left;
			public long top;
			public long right;
			public long bottom;
		}
		#endregion

		#region "Constants"
		// WINDOWPLACEMENT.flags values
		private const uint WPF_SETMINPOSITION = 0x1;
		private const uint WPF_RESTORETOMAXIMIZED = 0x2;

		private const uint WPF_ASYNCWINDOWPLACEMENT = 0x4;
		// WINDOWPLACEMENT.showCmd values
		private const uint SW_HIDE = 0;
		private const uint SW_SHOWNORMAL = 1;
		private const uint SW_SHOWMINIMIZED = 2;
		private const uint SW_SHOWMAXIMIZED = 3;
		private const uint SW_MAXIMIZE = 3;
		private const uint SW_SHOWNOACTIVATE = 4;
		private const uint SW_SHOW = 5;
		private const uint SW_MINIMIZE = 6;
		private const uint SW_SHOWMINNOACTIVE = 7;
		private const uint SW_SHOWNA = 8;
			#endregion
		private const uint SW_RESTORE = 9;
		#endregion

		#region "Private Variables"
			#endregion
		private System.Windows.Forms.Form _form;

		#region "Constructors/Destructors"
		public WindowPlacement(ref System.Windows.Forms.Form form)
		{
			_form = form;
		}
		#endregion

		#region "Public Properties"
		public Windows.Forms.Form Form {
			get { return _form; }
			set { _form = value; }
		}

		public bool RestoreToMaximized {
			get {
				WINDOWPLACEMENT windowPlacement = GetWindowPlacement();
				return windowPlacement.flags & WPF_RESTORETOMAXIMIZED;
			}
			set {
				WINDOWPLACEMENT windowPlacement = GetWindowPlacement();
				if (value) {
					windowPlacement.flags = windowPlacement.flags | WPF_RESTORETOMAXIMIZED;
				} else {
					windowPlacement.flags = windowPlacement.flags & !WPF_RESTORETOMAXIMIZED;
				}
				SetWindowPlacement(windowPlacement);
			}
		}
		#endregion

		#region "Private Functions"
		private WINDOWPLACEMENT GetWindowPlacement()
		{
			if (_form == null) {
				throw new System.NullReferenceException("WindowPlacement.Form is not set.");
			}
			WINDOWPLACEMENT windowPlacement = default(WINDOWPLACEMENT);
			windowPlacement.length = Marshal.SizeOf(windowPlacement);
			try {
				GetWindowPlacement(_form.Handle, ref windowPlacement);
				return windowPlacement;
			} catch (Exception ex) {
				throw;
			}
		}

		private bool SetWindowPlacement(WINDOWPLACEMENT windowPlacement)
		{
			if (_form == null) {
				throw new System.NullReferenceException("WindowPlacement.Form is not set.");
			}
			windowPlacement.length = Marshal.SizeOf(windowPlacement);
			try {
				return SetWindowPlacement(_form.Handle, ref windowPlacement);
			} catch (Exception ex) {
				throw;
			}
		}
		#endregion
	}
}
