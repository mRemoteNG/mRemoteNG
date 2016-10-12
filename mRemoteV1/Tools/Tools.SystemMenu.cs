using System;
using System.Drawing;
using mRemoteNG.App;

namespace mRemoteNG.Tools
{
    public class SystemMenu
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

        public IntPtr FormHandle;

        public IntPtr SystemMenuHandle;

        public SystemMenu(IntPtr Handle)
        {
            FormHandle = Handle;
            SystemMenuHandle = NativeMethods.GetSystemMenu(FormHandle, false);
        }

        public void Reset()
        {
            SystemMenuHandle = NativeMethods.GetSystemMenu(FormHandle, true);
        }

        public void AppendMenuItem(IntPtr ParentMenu, Flags Flags, IntPtr ID, string Text)
        {
            NativeMethods.AppendMenu(ParentMenu, (int) Flags, ID, Text);
        }

        public IntPtr CreatePopupMenuItem()
        {
            return NativeMethods.CreatePopupMenu();
        }

        public bool InsertMenuItem(IntPtr SysMenu, int Position, Flags Flags, IntPtr SubMenu, string Text)
        {
            return NativeMethods.InsertMenu(SysMenu, Position, (int) Flags, SubMenu, Text);
        }

        public IntPtr SetBitmap(IntPtr Menu, int Position, Flags Flags, Bitmap Bitmap)
        {
            return
                new IntPtr(
                    Convert.ToInt32(NativeMethods.SetMenuItemBitmaps(Menu, Position, (int) Flags, Bitmap.GetHbitmap(),
                        Bitmap.GetHbitmap())));
        }
    }
}