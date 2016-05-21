using System;
using System.Drawing;


namespace mRemoteNG.Tools
{
	public class SystemMenu
	{
		[Flags]
		public enum Flags
		{
			MF_STRING = App.NativeMethods.MF_STRING,
			MF_SEPARATOR = App.NativeMethods.MF_SEPARATOR,
			MF_BYCOMMAND = App.NativeMethods.MF_BYCOMMAND,
			MF_BYPOSITION = App.NativeMethods.MF_BYPOSITION,
			MF_POPUP = App.NativeMethods.MF_POPUP,
			WM_SYSCOMMAND = App.NativeMethods.WM_SYSCOMMAND
		}
			
		public IntPtr SystemMenuHandle;
		public IntPtr FormHandle;
			
		public SystemMenu(IntPtr Handle)
		{
			FormHandle = Handle;
			SystemMenuHandle = App.NativeMethods.GetSystemMenu(FormHandle, false);
		}
			
		public void Reset()
		{
			SystemMenuHandle = App.NativeMethods.GetSystemMenu(FormHandle, true);
		}

        public void AppendMenuItem(IntPtr ParentMenu, Flags Flags, IntPtr ID, string Text)
		{
			App.NativeMethods.AppendMenu(ParentMenu, (int)Flags, ID, Text);
		}
			
		public IntPtr CreatePopupMenuItem()
		{
			return App.NativeMethods.CreatePopupMenu();
		}
			
		public bool InsertMenuItem(IntPtr SysMenu, int Position, Flags Flags, IntPtr SubMenu, string Text)
		{
			return App.NativeMethods.InsertMenu(SysMenu, Position, (int)Flags, SubMenu, Text);
		}
			
		public IntPtr SetBitmap(IntPtr Menu, int Position, Flags Flags, Bitmap Bitmap)
		{
			return new IntPtr(Convert.ToInt32(App.NativeMethods.SetMenuItemBitmaps(Menu, Position, (int)Flags, Bitmap.GetHbitmap(), Bitmap.GetHbitmap())));
		}
	}
}