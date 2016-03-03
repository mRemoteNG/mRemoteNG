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
	public class WindowPlacement
		{
#region Windows API
#region Functions
			[DllImport("user32", ExactSpelling=true, CharSet=CharSet.Ansi, SetLastError=true)]
			private static extern bool GetWindowPlacement(System.IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);
			[DllImport("user32", ExactSpelling=true, CharSet=CharSet.Ansi, SetLastError=true)]
			private static extern bool SetWindowPlacement(System.IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);
#endregion
			
#region Structures
			private struct WINDOWPLACEMENT
			{
				public UInt32 length;
				public UInt32 flags;
				public UInt32 showCmd;
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
			
#region Constants
			// WINDOWPLACEMENT.flags values
			private const UInt32 WPF_SETMINPOSITION = 0x1;
			private const UInt32 WPF_RESTORETOMAXIMIZED = 0x2;
			private const UInt32 WPF_ASYNCWINDOWPLACEMENT = 0x4;
			
			// WINDOWPLACEMENT.showCmd values
			private const UInt32 SW_HIDE = 0;
			private const UInt32 SW_SHOWNORMAL = 1;
			private const UInt32 SW_SHOWMINIMIZED = 2;
			private const UInt32 SW_SHOWMAXIMIZED = 3;
			private const UInt32 SW_MAXIMIZE = 3;
			private const UInt32 SW_SHOWNOACTIVATE = 4;
			private const UInt32 SW_SHOW = 5;
			private const UInt32 SW_MINIMIZE = 6;
			private const UInt32 SW_SHOWMINNOACTIVE = 7;
			private const UInt32 SW_SHOWNA = 8;
			private const UInt32 SW_RESTORE = 9;
#endregion
#endregion
			
#region Private Variables
			private Windows.Forms.Form _form;
#endregion
			
#region Constructors/Destructors
			public WindowPlacement(Windows.Forms.Form form)
			{
				_form = form;
			}
#endregion
			
#region Public Properties
public Windows.Forms.Form Form
			{
				get
				{
					return _form;
				}
				set
				{
					_form = value;
				}
			}
			
public bool RestoreToMaximized
			{
				get
				{
					WINDOWPLACEMENT windowPlacement = GetWindowPlacement();
					return windowPlacement.flags & WPF_RESTORETOMAXIMIZED;
				}
				set
				{
					WINDOWPLACEMENT windowPlacement = GetWindowPlacement();
					if (value)
					{
						windowPlacement.flags = windowPlacement.flags | WPF_RESTORETOMAXIMIZED;
					}
					else
					{
						windowPlacement.flags = windowPlacement.flags & ~WPF_RESTORETOMAXIMIZED;
					}
					SetWindowPlacement(windowPlacement);
				}
			}
#endregion
			
#region Private Functions
			private WINDOWPLACEMENT GetWindowPlacement()
			{
				if (_form == null)
				{
					throw (new System.NullReferenceException("WindowPlacement.Form is not set."));
				}
				WINDOWPLACEMENT windowPlacement = new WINDOWPLACEMENT();
				windowPlacement.length = Marshal.SizeOf(windowPlacement);
				try
				{
					GetWindowPlacement(_form.Handle, ref windowPlacement);
					return windowPlacement;
				}
				catch (Exception)
				{
					throw;
				}
			}
			
			private bool SetWindowPlacement(WINDOWPLACEMENT windowPlacement)
			{
				if (_form == null)
				{
					throw (new System.NullReferenceException("WindowPlacement.Form is not set."));
				}
				windowPlacement.length = Marshal.SizeOf(windowPlacement);
				try
				{
					return SetWindowPlacement(_form.Handle, ref windowPlacement);
				}
				catch (Exception)
				{
					throw;
				}
			}
#endregion
		}
}
