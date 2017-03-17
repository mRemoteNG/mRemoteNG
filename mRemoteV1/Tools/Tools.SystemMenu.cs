using System;
using System.Drawing;
using mRemoteNG.App;
using Microsoft.Win32.SafeHandles;
// ReSharper disable MemberCanBeMadeStatic.Global

namespace mRemoteNG.Tools
{
	public sealed class SystemMenu : SafeHandleZeroOrMinusOneIsInvalid, IDisposable
    {
		[Flags]
		public enum Flags
		{
			MF_STRING = NativeMethods.MF_STRING,
			MF_SEPARATOR = NativeMethods.MF_SEPARATOR,
			MF_BYCOMMAND = NativeMethods.MF_BYCOMMAND,
			MF_BYPOSITION = NativeMethods.MF_BYPOSITION,
			MF_POPUP = NativeMethods.MF_POPUP,
			WM_SYSCOMMAND = NativeMethods.WM_SYSCOMMAND
		}

        private bool disposed;
        internal IntPtr SystemMenuHandle;
        private readonly IntPtr FormHandle;

        public SystemMenu(IntPtr Handle) :base(true)
		{
			FormHandle = Handle;
            SystemMenuHandle = NativeMethods.GetSystemMenu(FormHandle, false);
            SetHandle(SystemMenuHandle);
        }
			
		public void Reset()
		{
			SystemMenuHandle = NativeMethods.GetSystemMenu(FormHandle, true);
		}

        public void AppendMenuItem(IntPtr ParentMenu, Flags Flags, IntPtr ID, string Text)
		{
			NativeMethods.AppendMenu(ParentMenu, (int)Flags, ID, Text);
		}
			
		public IntPtr CreatePopupMenuItem()
		{
			return NativeMethods.CreatePopupMenu();
		}
			
		public bool InsertMenuItem(IntPtr SysMenu, int Position, Flags Flags, IntPtr SubMenu, string Text)
		{
			return NativeMethods.InsertMenu(SysMenu, Position, (int)Flags, SubMenu, Text);
		}
			
		public IntPtr SetBitmap(IntPtr Menu, int Position, Flags Flags, Bitmap Bitmap)
		{
			return new IntPtr(Convert.ToInt32(NativeMethods.SetMenuItemBitmaps(Menu, Position, (int)Flags, Bitmap.GetHbitmap(), Bitmap.GetHbitmap())));
		}

        protected override bool ReleaseHandle()
        {
            return NativeMethods.CloseHandle(SystemMenuHandle);
        }

        ~SystemMenu()
        {
            Dispose(false);
        }

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposed) return;
            if (!disposing) return;

            ReleaseHandle();

            disposed = true;
        }
    }
}