using System;
using System.Drawing;
using mRemoteNG.App;
using Microsoft.Win32.SafeHandles;
using System.Runtime.Versioning;

// ReSharper disable MemberCanBeMadeStatic.Global

namespace mRemoteNG.Tools
{
    [SupportedOSPlatform("windows")]
    public sealed class WindowMenu : SafeHandleZeroOrMinusOneIsInvalid, IDisposable
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

        private bool _disposed;
        internal IntPtr SystemMenuHandle;
        private readonly IntPtr FormHandle;

        public WindowMenu(IntPtr Handle) : base(true)
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
            return new IntPtr(Convert.ToInt32(NativeMethods.SetMenuItemBitmaps(Menu, Position, (int)Flags,
                                                                               Bitmap.GetHbitmap(),
                                                                               Bitmap.GetHbitmap())));
        }

        protected override bool ReleaseHandle()
        {
            return NativeMethods.CloseHandle(SystemMenuHandle);
        }


        /* If we don't have the finalizer, then we get this warning: https://msdn.microsoft.com/library/ms182329.aspx (CA2216: Disposable types should declare finalizer)
         * If we DO have the finalizer, then we get this warning: https://msdn.microsoft.com/library/ms244737.aspx (CA1063: Implement IDisposable correctly)
         * 
         * Since the handle is likely going to be in use for the entierty of the process, the finalizer isn't very important since when we're calling it
         * the process is likely exiting. Leaks would be moot once it exits. CA2216 is the lesser of 2 evils as far as I can tell. Suppress.
        ~SystemMenu()
        {
            Dispose(false);
        }
        */

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2216:DisposableTypesShouldDeclareFinalizer")]
        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing) return;

            ReleaseHandle();

            _disposed = true;
        }
    }
}