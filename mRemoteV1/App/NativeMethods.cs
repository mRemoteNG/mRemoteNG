using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace mRemoteNG.App
{
    public static class NativeMethods
    {
        #region Functions
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool AppendMenu(IntPtr hMenu, int uFlags, IntPtr uIDNewItem, string lpNewItem);
			
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr CreatePopupMenu();
			
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string lclassName, string windowTitle);
			
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr GetForegroundWindow();
			
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
			
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool InsertMenu(IntPtr hMenu, int uPosition, int uFlags, IntPtr uIDNewItem, string lpNewItem);
			
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern int IsIconic(IntPtr hWnd);
			
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool MoveWindow(IntPtr hWnd, int x, int y, int cx, int cy, bool repaint);
			
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);
			
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern int SendMessage(IntPtr hWnd, int msg, int wparam, int lparam);
			
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);
			
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);
			
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool SetMenuItemBitmaps(IntPtr hMenu, int uPosition, int uFlags, IntPtr hBitmapUnchecked, IntPtr hBitmapChecked);
			
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
			
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, long dwNewLong);
			
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern int ShowWindow(IntPtr hWnd, int nCmdShow);
			
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr WindowFromPoint(Point point);
        #endregion
		
        #region Structures
        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public int flags;
        }
        #endregion
		
        #region Helpers
        public static int MAKELONG(int wLow, int wHigh)
        {
            return wLow | wHigh << 16;
        }
			
        public static int MAKELPARAM(ref int wLow, ref int wHigh)
        {
            return MAKELONG(wLow, wHigh);
        }
			
        public static int LOWORD(int value)
        {
            return value & 0xFFFF;
        }
			
        public static int LOWORD(IntPtr value)
        {
            return LOWORD(value.ToInt32());
        }
			
        public static int HIWORD(int value)
        {
            return value >> 16;
        }
			
        public static int HIWORD(IntPtr value)
        {
            return HIWORD(value.ToInt32());
        }
        #endregion
		
        #region Constants
        // GetWindowLong
        public const int GWL_STYLE = (-16);
		
        // AppendMenu / ModifyMenu / DeleteMenu / RemoveMenu
        public const int MF_BYCOMMAND = 0x0;
        public const int MF_BYPOSITION = 0x400;
        public const int MF_STRING = 0x0;
        public const int MF_POPUP = 0x10;
        public const int MF_SEPARATOR = 0x800;
		
        // WM_LBUTTONDOWN / WM_LBUTTONUP
        public const int MK_LBUTTON = 0x1;
		
        // ShowWindow
        public const int SW_SHOWMAXIMIZED = 3;
        public const int SW_RESTORE = 9;
		
        // SetWindowPos / WM_WINDOWPOSCHANGING / WM_WINDOWPOSCHANGED
        public const int SWP_NOSIZE = 0x1;
        public const int SWP_NOMOVE = 0x2;
        public const int SWP_NOZORDER = 0x4;
        public const int SWP_NOREDRAW = 0x8;
        public const int SWP_NOACTIVATE = 0x10;
        public const int SWP_DRAWFRAME = 0x20;
        public const int SWP_FRAMECHANGED = 0x20;
        public const int SWP_SHOWWINDOW = 0x40;
        public const int SWP_HIDEWINDOW = 0x80;
        public const int SWP_NOCOPYBITS = 0x100;
        public const int SWP_NOOWNERZORDER = 0x200;
        public const int SWP_NOSENDCHANGING = 0x400;
        public const int SWP_NOCLIENTSIZE = 0x800;
        public const int SWP_NOCLIENTMOVE = 0x1000;
        public const int SWP_DEFERERASE = 0x2000;
        public const int SWP_ASYNCWINDOWPOS = 0x4000;
        public const int SWP_STATECHANGED = 0x8000;
		
        // WM_ACTIVATE
        public const int WA_INACTIVE = 0x0;
        public const int WA_ACTIVE = 0x1;
        public const int WA_CLICKACTIVE = 0x2;
		
        // Window Messages
        public const int WM_CREATE = 0x1;
        public const int WM_DESTROY = 0x2;
        public const int WM_ACTIVATE = 0x6;
        public const int WM_GETTEXT = 0xD;
        public const int WM_CLOSE = 0x10;
        public const int WM_ACTIVATEAPP = 0x1C;
        public const int WM_MOUSEACTIVATE = 0x21;
        public const int WM_GETMINMAXINFO = 0x24;
        public const int WM_WINDOWPOSCHANGING = 0x46;
        public const int WM_WINDOWPOSCHANGED = 0x47;
        public const int WM_KEYDOWN = 0x100;
        public const int WM_KEYUP = 0x101;
        public const int WM_SYSCOMMAND = 0x112;
        public const int WM_MOUSEMOVE = 0x200;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_RBUTTONDOWN = 0x204;
        public const int WM_RBUTTONUP = 0x205;
        public const int WM_MBUTTONDOWN = 0x207;
        public const int WM_MBUTTONUP = 0x208;
        public const int WM_XBUTTONDOWN = 0x20B;
        public const int WM_XBUTTONUP = 0x20C;
        public const int WM_PARENTNOTIFY = 0x210;
        public const int WM_ENTERSIZEMOVE = 0x231;
        public const int WM_EXITSIZEMOVE = 0x232;
        public const int WM_DRAWCLIPBOARD = 0x308;
        public const int WM_CHANGECBCHAIN = 0x30D;
		
        // Window Styles
        public const int WS_MAXIMIZE = 0x1000000;
        public const int WS_VISIBLE = 0x10000000;
        public const int WS_CHILD = 0x40000000;
        public const int WS_EX_MDICHILD = 0x40;
		
        // Virtual Key Codes
        public const int VK_CONTROL = 0x11;
        public const int VK_C = 0x67;
        #endregion
    }
}