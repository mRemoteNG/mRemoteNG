using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
namespace mRemoteNG.Tools
{
	public class SystemMenu
	{
		public enum Flags
		{
			MF_STRING = mRemoteNG.App.Native.MF_STRING,
			MF_SEPARATOR = mRemoteNG.App.Native.MF_SEPARATOR,
			MF_BYCOMMAND = mRemoteNG.App.Native.MF_BYCOMMAND,
			MF_BYPOSITION = mRemoteNG.App.Native.MF_BYPOSITION,
			MF_POPUP = mRemoteNG.App.Native.MF_POPUP,

			WM_SYSCOMMAND = mRemoteNG.App.Native.WM_SYSCOMMAND
		}

		public IntPtr SystemMenuHandle;

		public IntPtr FormHandle;
		public SystemMenu(IntPtr Handle)
		{
			FormHandle = Handle;
			SystemMenuHandle = mRemoteNG.App.Native.GetSystemMenu(FormHandle, false);
		}

		public void Reset()
		{
			SystemMenuHandle = mRemoteNG.App.Native.GetSystemMenu(FormHandle, true);
		}

		public void AppendMenuItem(IntPtr ParentMenu, Flags Flags, int ID, string Text)
		{
			mRemoteNG.App.Native.AppendMenu(ParentMenu, Flags, ID, Text);
		}

		public IntPtr CreatePopupMenuItem()
		{
			return mRemoteNG.App.Native.CreatePopupMenu();
		}

		public bool InsertMenuItem(IntPtr SysMenu, int Position, Flags Flags, IntPtr SubMenu, string Text)
		{
			return mRemoteNG.App.Native.InsertMenu(SysMenu, Position, Flags, SubMenu, Text);
		}

		public IntPtr SetBitmap(IntPtr Menu, int Position, Flags Flags, Bitmap Bitmap)
		{
			return mRemoteNG.App.Native.SetMenuItemBitmaps(Menu, Position, Flags, Bitmap.GetHbitmap(), Bitmap.GetHbitmap());
		}
	}
}
