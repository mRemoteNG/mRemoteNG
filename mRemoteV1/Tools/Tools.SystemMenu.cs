using System;
using System.Drawing;


namespace mRemoteNG.Tools
{
	public class SystemMenu
	{
		public enum Flags
		{
			MF_STRING = App.Native.MF_STRING,
			MF_SEPARATOR = App.Native.MF_SEPARATOR,
			MF_BYCOMMAND = App.Native.MF_BYCOMMAND,
			MF_BYPOSITION = App.Native.MF_BYPOSITION,
			MF_POPUP = App.Native.MF_POPUP,
			WM_SYSCOMMAND = App.Native.WM_SYSCOMMAND
		}
			
		public IntPtr SystemMenuHandle;
		public IntPtr FormHandle;
			
		public SystemMenu(IntPtr Handle)
		{
			FormHandle = Handle;
			SystemMenuHandle = App.Native.GetSystemMenu(FormHandle, false);
		}
			
		public void Reset()
		{
			SystemMenuHandle = App.Native.GetSystemMenu(FormHandle, true);
		}

        public void AppendMenuItem(IntPtr ParentMenu, Flags Flags, IntPtr ID, string Text)
		{
			App.Native.AppendMenu(ParentMenu, (int)Flags, ID, Text);
		}
			
		public IntPtr CreatePopupMenuItem()
		{
			return App.Native.CreatePopupMenu();
		}
			
		public bool InsertMenuItem(IntPtr SysMenu, int Position, Flags Flags, IntPtr SubMenu, string Text)
		{
			return App.Native.InsertMenu(SysMenu, Position, (int)Flags, SubMenu, Text);
		}
			
		public IntPtr SetBitmap(IntPtr Menu, int Position, Flags Flags, Bitmap Bitmap)
		{
			return new IntPtr(Convert.ToInt32(App.Native.SetMenuItemBitmaps(Menu, Position, (int)Flags, Bitmap.GetHbitmap(), Bitmap.GetHbitmap())));
		}
	}
}