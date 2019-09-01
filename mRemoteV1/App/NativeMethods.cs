using System;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;

#pragma warning disable 649
#pragma warning disable 169

namespace mRemoteNG.App
{
    public static class NativeMethods
    {
        #region Functions

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool AppendMenu(IntPtr hMenu, int uFlags, IntPtr uIDNewItem, string lpNewItem);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr CreatePopupMenu();

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr FindWindowEx(IntPtr parentHandle,
                                                   IntPtr childAfter,
                                                   string lclassName,
                                                   string windowTitle);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool InsertMenu(IntPtr hMenu,
                                               int uPosition,
                                               int uFlags,
                                               IntPtr uIDNewItem,
                                               string lpNewItem);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern int IsIconic(IntPtr hWnd);


        internal delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool EnumChildWindows(IntPtr hwndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool MoveWindow(IntPtr hWnd, int x, int y, int cx, int cy, bool repaint);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, StringBuilder lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, string lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr SendMessage([In] IntPtr hWnd,
                                                  [In] uint msg,
                                                  [Out] StringBuilder wParam,
                                                  [In] IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool SetMenuItemBitmaps(IntPtr hMenu,
                                                       int uPosition,
                                                       int uFlags,
                                                       IntPtr hBitmapUnchecked,
                                                       IntPtr hBitmapChecked);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr WindowFromPoint(Point point);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern int GetDlgCtrlID(IntPtr hwndCtl);

        [DllImport("user32", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        internal static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        internal static extern bool SetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [DllImport("kernel32", SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        internal static extern bool CloseHandle(IntPtr handle);

        [DllImport("user32.dll")]
        internal static extern IntPtr DefWindowProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        // When you don't want the ProcessId, use this overload and pass IntPtr.Zero for the second parameter
        [DllImport("user32.dll")]
        internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr processId);

        #endregion

        #region Structures

        [StructLayout(LayoutKind.Sequential)]
        internal struct WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public int flags;
        }

        internal struct WINDOWPLACEMENT
        {
            public uint length;
            public uint flags;
            public uint showCmd;
            public POINT ptMinPosition;
            public POINT ptMaxPosition;
            public RECT rcNormalPosition;
        }

        internal struct POINT
        {
            public long x;
            public long y;
        }

        internal struct RECT
        {
            public long left;
            public long top;
            public long right;
            public long bottom;
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

        public const int TRUE = 1;

        #region GetWindowLong

        public const int GWL_STYLE = (-16);

        #endregion

        #region AppendMenu / ModifyMenu / DeleteMenu / RemoveMenu

        public const int MF_BYCOMMAND = 0x0;
        public const int MF_BYPOSITION = 0x400;
        public const int MF_STRING = 0x0;
        public const int MF_POPUP = 0x10;
        public const int MF_SEPARATOR = 0x800;

        #endregion

        #region WM_LBUTTONDOWN / WM_LBUTTONUP

        public const int MK_LBUTTON = 0x1;

        #endregion

        #region ShowWindow

        public const uint SW_HIDE = 0;
        public const uint SW_SHOWNORMAL = 1;
        public const uint SW_SHOWMINIMIZED = 2;
        public const uint SW_SHOWMAXIMIZED = 3;
        public const uint SW_MAXIMIZE = 3;
        public const uint SW_SHOWNOACTIVATE = 4;
        public const uint SW_SHOW = 5;
        public const uint SW_MINIMIZE = 6;
        public const uint SW_SHOWMINNOACTIVE = 7;
        public const uint SW_SHOWNA = 8;
        public const uint SW_RESTORE = 9;

        #endregion

        #region SetWindowPos / WM_WINDOWPOSCHANGING / WM_WINDOWPOSCHANGED

        /// <summary>
        /// Retains the current size (ignores the cx and cy parameters).
        /// </summary>
        public const int SWP_NOSIZE = 0x1;

        /// <summary>
        /// Retains the current position (ignores the x and y parameters).
        /// </summary>
        public const int SWP_NOMOVE = 0x2;

        /// <summary>
        /// Retains the current Z order (ignores the hWndInsertAfter parameter).
        /// </summary>
        public const int SWP_NOZORDER = 0x4;

        /// <summary>
        /// Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent window uncovered as a result of the window being moved. When this flag is set, the application must explicitly invalidate or redraw any parts of the window and parent window that need redrawing.
        /// </summary>
        public const int SWP_NOREDRAW = 0x8;

        /// <summary>
        /// Does not activate the window. If this flag is not set, the window is activated and moved to the top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter parameter).
        /// </summary>
        public const int SWP_NOACTIVATE = 0x10;

        /// <summary>
        /// Draws a frame (defined in the window's class description) around the window.
        /// </summary>
        public const int SWP_DRAWFRAME = 0x20;

        /// <summary>
        /// Sends a WM_NCCALCSIZE message to the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE is sent only when the window's size is being changed.
        /// </summary>
        public const int SWP_FRAMECHANGED = 0x20;

        /// <summary>
        /// Displays the window.
        /// </summary>
        public const int SWP_SHOWWINDOW = 0x40;

        /// <summary>
        /// Hides the window.
        /// </summary>
        public const int SWP_HIDEWINDOW = 0x80;

        /// <summary>
        /// Discards the entire contents of the client area. If this flag is not specified, the valid contents of the client area are saved and copied back into the client area after the window is sized or repositioned.
        /// </summary>
        public const int SWP_NOCOPYBITS = 0x100;

        /// <summary>
        /// Does not change the owner window's position in the Z order.
        /// </summary>
        public const int SWP_NOOWNERZORDER = 0x200;

        /// <summary>
        /// Prevents the window from receiving the WM_WINDOWPOSCHANGING message.
        /// </summary>
        public const int SWP_NOSENDCHANGING = 0x400;

        /// <summary>
        /// 
        /// </summary>
        public const int SWP_NOCLIENTSIZE = 0x800;

        /// <summary>
        /// 
        /// </summary>
        public const int SWP_NOCLIENTMOVE = 0x1000;

        /// <summary>
        /// Prevents generation of the WM_SYNCPAINT message.
        /// </summary>
        public const int SWP_DEFERERASE = 0x2000;

        /// <summary>
        /// If the calling thread and the thread that owns the window are attached to different input queues, the system posts the request to the thread that owns the window. This prevents the calling thread from blocking its execution while other threads process the request.
        /// </summary>
        public const int SWP_ASYNCWINDOWPOS = 0x4000;

        /// <summary>
        /// 
        /// </summary>
        public const int SWP_STATECHANGED = 0x8000;

        #endregion

        #region Window Placement Flags (WPF)

        public const uint WPF_SETMINPOSITION = 0x1;
        public const uint WPF_RESTORETOMAXIMIZED = 0x2;
        public const uint WPF_ASYNCWINDOWPLACEMENT = 0x4;

        #endregion

        #region WM_ACTIVATE

        /// <summary>
        /// 
        /// </summary>
        public const int WA_INACTIVE = 0x0;

        /// <summary>
        /// 
        /// </summary>
        public const int WA_ACTIVE = 0x1;

        /// <summary>
        /// Sent to both the window being activated and the window being deactivated.
        /// If the windows use the same input queue, the message is sent synchronously, first to the window procedure of the
        /// top-level window being deactivated, then to the window procedure of the top-level window being activated. If the
        /// windows use different input queues, the message is sent asynchronously, so the window is activated immediately.
        /// </summary>
        public const int WA_CLICKACTIVE = 0x2;

        #endregion

        #region Window Messages

        /// <summary>
        /// Sent when an application requests that a window be created by calling
        /// the CreateWindowEx or CreateWindow function. (The message is sent before
        /// the function returns.) The window procedure of the new window receives
        /// this message after the window is created, but before the window becomes
        /// visible.
        /// </summary>
        public const int WM_CREATE = 0x1;

        /// <summary>
        /// Sent when a window is being destroyed. It is sent to the window procedure
        /// of the window being destroyed after the window is removed from the screen.
        /// This message is sent first to the window being destroyed and then to the
        /// child windows(if any) as they are destroyed.During the processing of the
        /// message, it can be assumed that all child windows still exist.
        /// </summary>
        public const int WM_DESTROY = 0x2;

        /// <summary>
        /// Sent to both the window being activated and the window being deactivated.
        /// If the windows use the same input queue, the message is sent synchronously,
        /// first to the window procedure of the top-level window being deactivated,
        /// then to the window procedure of the top-level window being activated. If
        /// the windows use different input queues, the message is sent asynchronously,
        /// so the window is activated immediately.
        /// </summary>
        public const int WM_ACTIVATE = 0x6;

        /// <summary>
        /// Sent to a window after it has gained the keyboard focus.
        /// </summary>
        public const int WM_SETFOCUS = 0x7;

        /// <summary>
        /// Sent to a window immediately before it loses the keyboard focus.
        /// </summary>
        public const int WM_KILLFOCUS = 0x8;

        /// <summary>
        /// 
        /// </summary>
        public const int WM_SETTEXT = 0xC;

        /// <summary>
        /// Copies the text that corresponds to a window into a buffer provided by the caller.
        /// </summary>
        public const int WM_GETTEXT = 0xD;

        /// <summary>
        /// Sent as a signal that a window or an application should terminate.
        /// </summary>
        public const int WM_CLOSE = 0x10;

        /// <summary>
        /// Sent when a window belonging to a different application than the active window is about to be activated.
        /// The message is sent to the application whose window is being activated and to the application whose window
        /// is being deactivated.
        /// The wParam indicates whether the window is being activated or deactivated (TRUE if the
        /// window is being activated; FALSE if the window is being deactivated).
        /// The lParam is the thread identifier. If the wParam parameter is TRUE, lParam is the identifier of the
        /// thread that owns the window being deactivated. If wParam is FALSE, lParam is the identifier of the
        /// thread that owns the window being activated.
        /// </summary>
        public const int WM_ACTIVATEAPP = 0x1C;

        /// <summary>
        /// Sent to a window if the mouse causes the cursor to move within a window and mouse input is not captured.
        /// </summary>
        public const int WM_SETCURSOR = 0x20;

        /// <summary>
        /// Sent when the cursor is in an inactive window and the user presses a mouse button. The parent window receives this message only if the child window passes it to the DefWindowProc function.
        /// </summary>
        public const int WM_MOUSEACTIVATE = 0x21;

        /// <summary>
        /// Sent to a window when the size or position of the window is about to change. An application can use this message to override the window's default maximized size and position, or its default minimum or maximum tracking size.
        /// </summary>
        public const int WM_GETMINMAXINFO = 0x24;

        /// <summary>
        /// Sent to a window whose size, position, or place in the Z order is about to change as a result of a call to the SetWindowPos function or another window-management function.
        /// </summary>
        public const int WM_WINDOWPOSCHANGING = 0x46;

        /// <summary>
        /// Sent to a window whose size, position, or place in the Z order has changed as a result of a call to the SetWindowPos function or another window-management function.
        /// </summary>
        public const int WM_WINDOWPOSCHANGED = 0x47;

        /// <summary>
        /// Sent to a window when its nonclient area needs to be changed to indicate an active or inactive state.
        /// </summary>
        public const int WM_NCACTIVATE = 0x86;

        /// <summary>
        /// Posted to the window with the keyboard focus when a nonsystem key is pressed. A nonsystem key is a key that is pressed when the ALT key is not pressed.
        /// </summary>
        public const int WM_KEYDOWN = 0x100;

        /// <summary>
        /// Posted to the window with the keyboard focus when a nonsystem key is released. A nonsystem key is a key that is pressed when the ALT key is not pressed, or a keyboard key that is pressed when a window has the keyboard focus.
        /// </summary>
        public const int WM_KEYUP = 0x101;

        /// <summary>
        /// Posted to the window with the keyboard focus when a WM_KEYDOWN message is translated by the TranslateMessage function. The WM_CHAR message contains the character code of the key that was pressed.
        /// </summary>
        public const int WM_CHAR = 0x102;

        /// <summary>
        /// Posted to the window with the keyboard focus when the user presses the F10 key
        /// (which activates the menu bar) or holds down the ALT key and then presses another
        /// key. It also occurs when no window currently has the keyboard focus; in this case,
        /// the WM_SYSKEYDOWN message is sent to the active window. The window that receives the
        /// message can distinguish between these two contexts by checking the context code in
        /// the lParam parameter.
        /// </summary>
        public const int WM_SYSKEYDOWN = 0x104;

        /// <summary>
        /// Posted to the window with the keyboard focus when the user releases a key that was
        /// pressed while the ALT key was held down. It also occurs when no window currently
        /// has the keyboard focus; in this case, the WM_SYSKEYUP message is sent to the active
        /// window. The window that receives the message can distinguish between these two
        /// contexts by checking the context code in the lParam parameter.
        /// </summary>
        public const int WM_SYSKEYUP = 0x105;

        /// <summary>
        /// Sent when the user selects a command item from a menu, when a control sends a notification message to its parent window, or when an accelerator keystroke is translated.
        /// </summary>
        public const int WM_COMMAND = 0x111;

        /// <summary>
        /// A window receives this message when the user chooses a command from the Window menu (formerly known as the system or control menu) or when the user chooses the maximize button, minimize button, restore button, or close button.
        /// </summary>
        public const int WM_SYSCOMMAND = 0x112;

        /// <summary>
        /// Posted to a window when the cursor moves. If the mouse is not captured, the message is posted to the window that contains the cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        public const int WM_MOUSEMOVE = 0x200;

        /// <summary>
        /// Posted when the user presses the left mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        public const int WM_LBUTTONDOWN = 0x201;

        /// <summary>
        /// Posted when the user releases the left mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        public const int WM_LBUTTONUP = 0x202;

        /// <summary>
        /// Posted when the user presses the right mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        public const int WM_RBUTTONDOWN = 0x204;

        /// <summary>
        /// Posted when the user releases the right mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        public const int WM_RBUTTONUP = 0x205;

        /// <summary>
        /// Posted when the user presses the middle mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        public const int WM_MBUTTONDOWN = 0x207;

        /// <summary>
        /// Posted when the user releases the middle mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        public const int WM_MBUTTONUP = 0x208;

        /// <summary>
        /// Posted when the user presses the first or second X button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        public const int WM_XBUTTONDOWN = 0x20B;

        /// <summary>
        /// Posted when the user releases the first or second X button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        public const int WM_XBUTTONUP = 0x20C;

        /// <summary>
        /// Sent to a window when a significant action occurs on a descendant window. This message is now extended to include the WM_POINTERDOWN event. When the child window is being created, the system sends WM_PARENTNOTIFY just before the CreateWindow or CreateWindowEx function that creates the window returns. When the child window is being destroyed, the system sends the message before any processing to destroy the window takes place.
        /// </summary>
        public const int WM_PARENTNOTIFY = 0x210;

        /// <summary>
        /// Sent one time to a window after it enters the moving or sizing modal loop. The window enters the moving or sizing modal loop when the user clicks the window's title bar or sizing border, or when the window passes the WM_SYSCOMMAND message to the DefWindowProc function and the wParam parameter of the message specifies the SC_MOVE or SC_SIZE value. The operation is complete when DefWindowProc returns.
        /// </summary>
        public const int WM_ENTERSIZEMOVE = 0x231;

        /// <summary>
        /// Sent one time to a window, after it has exited the moving or sizing modal loop. The window enters the moving or sizing modal loop when the user clicks the window's title bar or sizing border, or when the window passes the WM_SYSCOMMAND message to the DefWindowProc function and the wParam parameter of the message specifies the SC_MOVE or SC_SIZE value. The operation is complete when DefWindowProc returns.
        /// </summary>
        public const int WM_EXITSIZEMOVE = 0x232;

        /// <summary>
        /// Sent to the first window in the clipboard viewer chain when the content of the clipboard changes. This enables a clipboard viewer window to display the new content of the clipboard.
        /// </summary>
        public const int WM_DRAWCLIPBOARD = 0x308;

        /// <summary>
        /// Sent to the first window in the clipboard viewer chain when a window is being removed from the chain.
        /// </summary>
        public const int WM_CHANGECBCHAIN = 0x30D;

        #endregion

        #region Window Styles

        public const int WS_MAXIMIZE = 0x1000000;
        public const int WS_VISIBLE = 0x10000000;
        public const int WS_CHILD = 0x40000000;
        public const int WS_EX_MDICHILD = 0x40;

        #endregion

        #region Keyboard
        public const int WH_KEYBOARD_LL = 13;

        [StructLayout(LayoutKind.Sequential)]
        public class KBDLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public KBDLLHOOKSTRUCTFlags flags;
            public uint time;
            public UIntPtr dwExtraInfo;
        }

        [Flags]
        public enum KBDLLHOOKSTRUCTFlags : uint
        {
            LLKHF_EXTENDED = 0x01,
            LLKHF_INJECTED = 0x10,
            LLKHF_ALTDOWN = 0x20,
            LLKHF_UP = 0x80,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT
        {
            internal uint type;
            internal InputUnion U;
            internal static int Size
            {
                get { return Marshal.SizeOf(typeof(INPUT)); }
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct InputUnion
        {
            [FieldOffset(0)]
            internal MOUSEINPUT mi;
            [FieldOffset(0)]
            internal KEYBDINPUT ki;
            [FieldOffset(0)]
            internal HARDWAREINPUT hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MOUSEINPUT
        {
            internal int dx;
            internal int dy;
            internal int mouseData;
            internal MOUSEEVENTF dwFlags;
            internal uint time;
            internal UIntPtr dwExtraInfo;
        }

        [Flags]
        internal enum MOUSEEVENTF : uint
        {
            ABSOLUTE = 0x8000,
            HWHEEL = 0x01000,
            MOVE = 0x0001,
            MOVE_NOCOALESCE = 0x2000,
            LEFTDOWN = 0x0002,
            LEFTUP = 0x0004,
            RIGHTDOWN = 0x0008,
            RIGHTUP = 0x0010,
            MIDDLEDOWN = 0x0020,
            MIDDLEUP = 0x0040,
            VIRTUALDESK = 0x4000,
            WHEEL = 0x0800,
            XDOWN = 0x0080,
            XUP = 0x0100
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct KEYBDINPUT
        {
            internal VirtualKeyShort wVk;
            internal ScanCodeShort wScan;
            internal KEYEVENTF dwFlags;
            internal int time;
            internal UIntPtr dwExtraInfo;
        }

        [Flags]
        internal enum KEYEVENTF : uint
        {
            EXTENDEDKEY = 0x0001,
            KEYUP = 0x0002,
            SCANCODE = 0x0008,
            UNICODE = 0x0004
        }

        internal enum VirtualKeyShort : short
        {
            ///<summary>
            ///Left mouse button
            ///</summary>
            LBUTTON = 0x01,
            ///<summary>
            ///Right mouse button
            ///</summary>
            RBUTTON = 0x02,
            ///<summary>
            ///Control-break processing
            ///</summary>
            CANCEL = 0x03,
            ///<summary>
            ///Middle mouse button (three-button mouse)
            ///</summary>
            MBUTTON = 0x04,
            ///<summary>
            ///Windows 2000/XP: X1 mouse button
            ///</summary>
            XBUTTON1 = 0x05,
            ///<summary>
            ///Windows 2000/XP: X2 mouse button
            ///</summary>
            XBUTTON2 = 0x06,
            ///<summary>
            ///BACKSPACE key
            ///</summary>
            BACK = 0x08,
            ///<summary>
            ///TAB key
            ///</summary>
            TAB = 0x09,
            ///<summary>
            ///CLEAR key
            ///</summary>
            CLEAR = 0x0C,
            ///<summary>
            ///ENTER key
            ///</summary>
            RETURN = 0x0D,
            ///<summary>
            ///SHIFT key
            ///</summary>
            SHIFT = 0x10,
            ///<summary>
            ///CTRL key
            ///</summary>
            CONTROL = 0x11,
            ///<summary>
            ///ALT key
            ///</summary>
            MENU = 0x12,
            ///<summary>
            ///PAUSE key
            ///</summary>
            PAUSE = 0x13,
            ///<summary>
            ///CAPS LOCK key
            ///</summary>
            CAPITAL = 0x14,
            ///<summary>
            ///Input Method Editor (IME) Kana mode
            ///</summary>
            KANA = 0x15,
            ///<summary>
            ///IME Hangul mode
            ///</summary>
            HANGUL = 0x15,
            ///<summary>
            ///IME Junja mode
            ///</summary>
            JUNJA = 0x17,
            ///<summary>
            ///IME final mode
            ///</summary>
            FINAL = 0x18,
            ///<summary>
            ///IME Hanja mode
            ///</summary>
            HANJA = 0x19,
            ///<summary>
            ///IME Kanji mode
            ///</summary>
            KANJI = 0x19,
            ///<summary>
            ///ESC key
            ///</summary>
            ESCAPE = 0x1B,
            ///<summary>
            ///IME convert
            ///</summary>
            CONVERT = 0x1C,
            ///<summary>
            ///IME nonconvert
            ///</summary>
            NONCONVERT = 0x1D,
            ///<summary>
            ///IME accept
            ///</summary>
            ACCEPT = 0x1E,
            ///<summary>
            ///IME mode change request
            ///</summary>
            MODECHANGE = 0x1F,
            ///<summary>
            ///SPACEBAR
            ///</summary>
            SPACE = 0x20,
            ///<summary>
            ///PAGE UP key
            ///</summary>
            PRIOR = 0x21,
            ///<summary>
            ///PAGE DOWN key
            ///</summary>
            NEXT = 0x22,
            ///<summary>
            ///END key
            ///</summary>
            END = 0x23,
            ///<summary>
            ///HOME key
            ///</summary>
            HOME = 0x24,
            ///<summary>
            ///LEFT ARROW key
            ///</summary>
            LEFT = 0x25,
            ///<summary>
            ///UP ARROW key
            ///</summary>
            UP = 0x26,
            ///<summary>
            ///RIGHT ARROW key
            ///</summary>
            RIGHT = 0x27,
            ///<summary>
            ///DOWN ARROW key
            ///</summary>
            DOWN = 0x28,
            ///<summary>
            ///SELECT key
            ///</summary>
            SELECT = 0x29,
            ///<summary>
            ///PRINT key
            ///</summary>
            PRINT = 0x2A,
            ///<summary>
            ///EXECUTE key
            ///</summary>
            EXECUTE = 0x2B,
            ///<summary>
            ///PRINT SCREEN key
            ///</summary>
            SNAPSHOT = 0x2C,
            ///<summary>
            ///INS key
            ///</summary>
            INSERT = 0x2D,
            ///<summary>
            ///DEL key
            ///</summary>
            DELETE = 0x2E,
            ///<summary>
            ///HELP key
            ///</summary>
            HELP = 0x2F,
            ///<summary>
            ///0 key
            ///</summary>
            KEY_0 = 0x30,
            ///<summary>
            ///1 key
            ///</summary>
            KEY_1 = 0x31,
            ///<summary>
            ///2 key
            ///</summary>
            KEY_2 = 0x32,
            ///<summary>
            ///3 key
            ///</summary>
            KEY_3 = 0x33,
            ///<summary>
            ///4 key
            ///</summary>
            KEY_4 = 0x34,
            ///<summary>
            ///5 key
            ///</summary>
            KEY_5 = 0x35,
            ///<summary>
            ///6 key
            ///</summary>
            KEY_6 = 0x36,
            ///<summary>
            ///7 key
            ///</summary>
            KEY_7 = 0x37,
            ///<summary>
            ///8 key
            ///</summary>
            KEY_8 = 0x38,
            ///<summary>
            ///9 key
            ///</summary>
            KEY_9 = 0x39,
            ///<summary>
            ///A key
            ///</summary>
            KEY_A = 0x41,
            ///<summary>
            ///B key
            ///</summary>
            KEY_B = 0x42,
            ///<summary>
            ///C key
            ///</summary>
            KEY_C = 0x43,
            ///<summary>
            ///D key
            ///</summary>
            KEY_D = 0x44,
            ///<summary>
            ///E key
            ///</summary>
            KEY_E = 0x45,
            ///<summary>
            ///F key
            ///</summary>
            KEY_F = 0x46,
            ///<summary>
            ///G key
            ///</summary>
            KEY_G = 0x47,
            ///<summary>
            ///H key
            ///</summary>
            KEY_H = 0x48,
            ///<summary>
            ///I key
            ///</summary>
            KEY_I = 0x49,
            ///<summary>
            ///J key
            ///</summary>
            KEY_J = 0x4A,
            ///<summary>
            ///K key
            ///</summary>
            KEY_K = 0x4B,
            ///<summary>
            ///L key
            ///</summary>
            KEY_L = 0x4C,
            ///<summary>
            ///M key
            ///</summary>
            KEY_M = 0x4D,
            ///<summary>
            ///N key
            ///</summary>
            KEY_N = 0x4E,
            ///<summary>
            ///O key
            ///</summary>
            KEY_O = 0x4F,
            ///<summary>
            ///P key
            ///</summary>
            KEY_P = 0x50,
            ///<summary>
            ///Q key
            ///</summary>
            KEY_Q = 0x51,
            ///<summary>
            ///R key
            ///</summary>
            KEY_R = 0x52,
            ///<summary>
            ///S key
            ///</summary>
            KEY_S = 0x53,
            ///<summary>
            ///T key
            ///</summary>
            KEY_T = 0x54,
            ///<summary>
            ///U key
            ///</summary>
            KEY_U = 0x55,
            ///<summary>
            ///V key
            ///</summary>
            KEY_V = 0x56,
            ///<summary>
            ///W key
            ///</summary>
            KEY_W = 0x57,
            ///<summary>
            ///X key
            ///</summary>
            KEY_X = 0x58,
            ///<summary>
            ///Y key
            ///</summary>
            KEY_Y = 0x59,
            ///<summary>
            ///Z key
            ///</summary>
            KEY_Z = 0x5A,
            ///<summary>
            ///Left Windows key (Microsoft Natural keyboard)
            ///</summary>
            LWIN = 0x5B,
            ///<summary>
            ///Right Windows key (Natural keyboard)
            ///</summary>
            RWIN = 0x5C,
            ///<summary>
            ///Applications key (Natural keyboard)
            ///</summary>
            APPS = 0x5D,
            ///<summary>
            ///Computer Sleep key
            ///</summary>
            SLEEP = 0x5F,
            ///<summary>
            ///Numeric keypad 0 key
            ///</summary>
            NUMPAD0 = 0x60,
            ///<summary>
            ///Numeric keypad 1 key
            ///</summary>
            NUMPAD1 = 0x61,
            ///<summary>
            ///Numeric keypad 2 key
            ///</summary>
            NUMPAD2 = 0x62,
            ///<summary>
            ///Numeric keypad 3 key
            ///</summary>
            NUMPAD3 = 0x63,
            ///<summary>
            ///Numeric keypad 4 key
            ///</summary>
            NUMPAD4 = 0x64,
            ///<summary>
            ///Numeric keypad 5 key
            ///</summary>
            NUMPAD5 = 0x65,
            ///<summary>
            ///Numeric keypad 6 key
            ///</summary>
            NUMPAD6 = 0x66,
            ///<summary>
            ///Numeric keypad 7 key
            ///</summary>
            NUMPAD7 = 0x67,
            ///<summary>
            ///Numeric keypad 8 key
            ///</summary>
            NUMPAD8 = 0x68,
            ///<summary>
            ///Numeric keypad 9 key
            ///</summary>
            NUMPAD9 = 0x69,
            ///<summary>
            ///Multiply key
            ///</summary>
            MULTIPLY = 0x6A,
            ///<summary>
            ///Add key
            ///</summary>
            ADD = 0x6B,
            ///<summary>
            ///Separator key
            ///</summary>
            SEPARATOR = 0x6C,
            ///<summary>
            ///Subtract key
            ///</summary>
            SUBTRACT = 0x6D,
            ///<summary>
            ///Decimal key
            ///</summary>
            DECIMAL = 0x6E,
            ///<summary>
            ///Divide key
            ///</summary>
            DIVIDE = 0x6F,
            ///<summary>
            ///F1 key
            ///</summary>
            F1 = 0x70,
            ///<summary>
            ///F2 key
            ///</summary>
            F2 = 0x71,
            ///<summary>
            ///F3 key
            ///</summary>
            F3 = 0x72,
            ///<summary>
            ///F4 key
            ///</summary>
            F4 = 0x73,
            ///<summary>
            ///F5 key
            ///</summary>
            F5 = 0x74,
            ///<summary>
            ///F6 key
            ///</summary>
            F6 = 0x75,
            ///<summary>
            ///F7 key
            ///</summary>
            F7 = 0x76,
            ///<summary>
            ///F8 key
            ///</summary>
            F8 = 0x77,
            ///<summary>
            ///F9 key
            ///</summary>
            F9 = 0x78,
            ///<summary>
            ///F10 key
            ///</summary>
            F10 = 0x79,
            ///<summary>
            ///F11 key
            ///</summary>
            F11 = 0x7A,
            ///<summary>
            ///F12 key
            ///</summary>
            F12 = 0x7B,
            ///<summary>
            ///F13 key
            ///</summary>
            F13 = 0x7C,
            ///<summary>
            ///F14 key
            ///</summary>
            F14 = 0x7D,
            ///<summary>
            ///F15 key
            ///</summary>
            F15 = 0x7E,
            ///<summary>
            ///F16 key
            ///</summary>
            F16 = 0x7F,
            ///<summary>
            ///F17 key  
            ///</summary>
            F17 = 0x80,
            ///<summary>
            ///F18 key  
            ///</summary>
            F18 = 0x81,
            ///<summary>
            ///F19 key  
            ///</summary>
            F19 = 0x82,
            ///<summary>
            ///F20 key  
            ///</summary>
            F20 = 0x83,
            ///<summary>
            ///F21 key  
            ///</summary>
            F21 = 0x84,
            ///<summary>
            ///F22 key, (PPC only) Key used to lock device.
            ///</summary>
            F22 = 0x85,
            ///<summary>
            ///F23 key  
            ///</summary>
            F23 = 0x86,
            ///<summary>
            ///F24 key  
            ///</summary>
            F24 = 0x87,
            ///<summary>
            ///NUM LOCK key
            ///</summary>
            NUMLOCK = 0x90,
            ///<summary>
            ///SCROLL LOCK key
            ///</summary>
            SCROLL = 0x91,
            ///<summary>
            ///Left SHIFT key
            ///</summary>
            LSHIFT = 0xA0,
            ///<summary>
            ///Right SHIFT key
            ///</summary>
            RSHIFT = 0xA1,
            ///<summary>
            ///Left CONTROL key
            ///</summary>
            LCONTROL = 0xA2,
            ///<summary>
            ///Right CONTROL key
            ///</summary>
            RCONTROL = 0xA3,
            ///<summary>
            ///Left MENU key
            ///</summary>
            LMENU = 0xA4,
            ///<summary>
            ///Right MENU key
            ///</summary>
            RMENU = 0xA5,
            ///<summary>
            ///Windows 2000/XP: Browser Back key
            ///</summary>
            BROWSER_BACK = 0xA6,
            ///<summary>
            ///Windows 2000/XP: Browser Forward key
            ///</summary>
            BROWSER_FORWARD = 0xA7,
            ///<summary>
            ///Windows 2000/XP: Browser Refresh key
            ///</summary>
            BROWSER_REFRESH = 0xA8,
            ///<summary>
            ///Windows 2000/XP: Browser Stop key
            ///</summary>
            BROWSER_STOP = 0xA9,
            ///<summary>
            ///Windows 2000/XP: Browser Search key
            ///</summary>
            BROWSER_SEARCH = 0xAA,
            ///<summary>
            ///Windows 2000/XP: Browser Favorites key
            ///</summary>
            BROWSER_FAVORITES = 0xAB,
            ///<summary>
            ///Windows 2000/XP: Browser Start and Home key
            ///</summary>
            BROWSER_HOME = 0xAC,
            ///<summary>
            ///Windows 2000/XP: Volume Mute key
            ///</summary>
            VOLUME_MUTE = 0xAD,
            ///<summary>
            ///Windows 2000/XP: Volume Down key
            ///</summary>
            VOLUME_DOWN = 0xAE,
            ///<summary>
            ///Windows 2000/XP: Volume Up key
            ///</summary>
            VOLUME_UP = 0xAF,
            ///<summary>
            ///Windows 2000/XP: Next Track key
            ///</summary>
            MEDIA_NEXT_TRACK = 0xB0,
            ///<summary>
            ///Windows 2000/XP: Previous Track key
            ///</summary>
            MEDIA_PREV_TRACK = 0xB1,
            ///<summary>
            ///Windows 2000/XP: Stop Media key
            ///</summary>
            MEDIA_STOP = 0xB2,
            ///<summary>
            ///Windows 2000/XP: Play/Pause Media key
            ///</summary>
            MEDIA_PLAY_PAUSE = 0xB3,
            ///<summary>
            ///Windows 2000/XP: Start Mail key
            ///</summary>
            LAUNCH_MAIL = 0xB4,
            ///<summary>
            ///Windows 2000/XP: Select Media key
            ///</summary>
            LAUNCH_MEDIA_SELECT = 0xB5,
            ///<summary>
            ///Windows 2000/XP: Start Application 1 key
            ///</summary>
            LAUNCH_APP1 = 0xB6,
            ///<summary>
            ///Windows 2000/XP: Start Application 2 key
            ///</summary>
            LAUNCH_APP2 = 0xB7,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard.
            ///</summary>
            OEM_1 = 0xBA,
            ///<summary>
            ///Windows 2000/XP: For any country/region, the '+' key
            ///</summary>
            OEM_PLUS = 0xBB,
            ///<summary>
            ///Windows 2000/XP: For any country/region, the ',' key
            ///</summary>
            OEM_COMMA = 0xBC,
            ///<summary>
            ///Windows 2000/XP: For any country/region, the '-' key
            ///</summary>
            OEM_MINUS = 0xBD,
            ///<summary>
            ///Windows 2000/XP: For any country/region, the '.' key
            ///</summary>
            OEM_PERIOD = 0xBE,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard.
            ///</summary>
            OEM_2 = 0xBF,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard.
            ///</summary>
            OEM_3 = 0xC0,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard.
            ///</summary>
            OEM_4 = 0xDB,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard.
            ///</summary>
            OEM_5 = 0xDC,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard.
            ///</summary>
            OEM_6 = 0xDD,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard.
            ///</summary>
            OEM_7 = 0xDE,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard.
            ///</summary>
            OEM_8 = 0xDF,
            ///<summary>
            ///Windows 2000/XP: Either the angle bracket key or the backslash key on the RT 102-key keyboard
            ///</summary>
            OEM_102 = 0xE2,
            ///<summary>
            ///Windows 95/98/Me, Windows NT 4.0, Windows 2000/XP: IME PROCESS key
            ///</summary>
            PROCESSKEY = 0xE5,
            ///<summary>
            ///Windows 2000/XP: Used to pass Unicode characters as if they were keystrokes.
            ///The VK_PACKET key is the low word of a 32-bit Virtual Key value used for non-keyboard input methods. For more information,
            ///see Remark in KEYBDINPUT, SendInput, WM_KEYDOWN, and WM_KEYUP
            ///</summary>
            PACKET = 0xE7,
            ///<summary>
            ///Attn key
            ///</summary>
            ATTN = 0xF6,
            ///<summary>
            ///CrSel key
            ///</summary>
            CRSEL = 0xF7,
            ///<summary>
            ///ExSel key
            ///</summary>
            EXSEL = 0xF8,
            ///<summary>
            ///Erase EOF key
            ///</summary>
            EREOF = 0xF9,
            ///<summary>
            ///Play key
            ///</summary>
            PLAY = 0xFA,
            ///<summary>
            ///Zoom key
            ///</summary>
            ZOOM = 0xFB,
            ///<summary>
            ///Reserved
            ///</summary>
            NONAME = 0xFC,
            ///<summary>
            ///PA1 key
            ///</summary>
            PA1 = 0xFD,
            ///<summary>
            ///Clear key
            ///</summary>
            OEM_CLEAR = 0xFE
        }
        internal enum ScanCodeShort : short
        {
            LBUTTON = 0,
            RBUTTON = 0,
            CANCEL = 70,
            MBUTTON = 0,
            XBUTTON1 = 0,
            XBUTTON2 = 0,
            BACK = 14,
            TAB = 15,
            CLEAR = 76,
            RETURN = 28,
            SHIFT = 42,
            CONTROL = 29,
            MENU = 56,
            PAUSE = 0,
            CAPITAL = 58,
            KANA = 0,
            HANGUL = 0,
            JUNJA = 0,
            FINAL = 0,
            HANJA = 0,
            KANJI = 0,
            ESCAPE = 1,
            CONVERT = 0,
            NONCONVERT = 0,
            ACCEPT = 0,
            MODECHANGE = 0,
            SPACE = 57,
            PRIOR = 73,
            NEXT = 81,
            END = 79,
            HOME = 71,
            LEFT = 75,
            UP = 72,
            RIGHT = 77,
            DOWN = 80,
            SELECT = 0,
            PRINT = 0,
            EXECUTE = 0,
            SNAPSHOT = 84,
            INSERT = 82,
            DELETE = 83,
            HELP = 99,
            KEY_0 = 11,
            KEY_1 = 2,
            KEY_2 = 3,
            KEY_3 = 4,
            KEY_4 = 5,
            KEY_5 = 6,
            KEY_6 = 7,
            KEY_7 = 8,
            KEY_8 = 9,
            KEY_9 = 10,
            KEY_A = 30,
            KEY_B = 48,
            KEY_C = 46,
            KEY_D = 32,
            KEY_E = 18,
            KEY_F = 33,
            KEY_G = 34,
            KEY_H = 35,
            KEY_I = 23,
            KEY_J = 36,
            KEY_K = 37,
            KEY_L = 38,
            KEY_M = 50,
            KEY_N = 49,
            KEY_O = 24,
            KEY_P = 25,
            KEY_Q = 16,
            KEY_R = 19,
            KEY_S = 31,
            KEY_T = 20,
            KEY_U = 22,
            KEY_V = 47,
            KEY_W = 17,
            KEY_X = 45,
            KEY_Y = 21,
            KEY_Z = 44,
            LWIN = 91,
            RWIN = 92,
            APPS = 93,
            SLEEP = 95,
            NUMPAD0 = 82,
            NUMPAD1 = 79,
            NUMPAD2 = 80,
            NUMPAD3 = 81,
            NUMPAD4 = 75,
            NUMPAD5 = 76,
            NUMPAD6 = 77,
            NUMPAD7 = 71,
            NUMPAD8 = 72,
            NUMPAD9 = 73,
            MULTIPLY = 55,
            ADD = 78,
            SEPARATOR = 0,
            SUBTRACT = 74,
            DECIMAL = 83,
            DIVIDE = 53,
            F1 = 59,
            F2 = 60,
            F3 = 61,
            F4 = 62,
            F5 = 63,
            F6 = 64,
            F7 = 65,
            F8 = 66,
            F9 = 67,
            F10 = 68,
            F11 = 87,
            F12 = 88,
            F13 = 100,
            F14 = 101,
            F15 = 102,
            F16 = 103,
            F17 = 104,
            F18 = 105,
            F19 = 106,
            F20 = 107,
            F21 = 108,
            F22 = 109,
            F23 = 110,
            F24 = 118,
            NUMLOCK = 69,
            SCROLL = 70,
            LSHIFT = 42,
            RSHIFT = 54,
            LCONTROL = 29,
            RCONTROL = 29,
            LMENU = 56,
            RMENU = 56,
            BROWSER_BACK = 106,
            BROWSER_FORWARD = 105,
            BROWSER_REFRESH = 103,
            BROWSER_STOP = 104,
            BROWSER_SEARCH = 101,
            BROWSER_FAVORITES = 102,
            BROWSER_HOME = 50,
            VOLUME_MUTE = 32,
            VOLUME_DOWN = 46,
            VOLUME_UP = 48,
            MEDIA_NEXT_TRACK = 25,
            MEDIA_PREV_TRACK = 16,
            MEDIA_STOP = 36,
            MEDIA_PLAY_PAUSE = 34,
            LAUNCH_MAIL = 108,
            LAUNCH_MEDIA_SELECT = 109,
            LAUNCH_APP1 = 107,
            LAUNCH_APP2 = 33,
            OEM_1 = 39,
            OEM_PLUS = 13,
            OEM_COMMA = 51,
            OEM_MINUS = 12,
            OEM_PERIOD = 52,
            OEM_2 = 53,
            OEM_3 = 41,
            OEM_4 = 26,
            OEM_5 = 43,
            OEM_6 = 27,
            OEM_7 = 40,
            OEM_8 = 0,
            OEM_102 = 86,
            PROCESSKEY = 0,
            PACKET = 0,
            ATTN = 0,
            CRSEL = 0,
            EXSEL = 0,
            EREOF = 93,
            PLAY = 0,
            ZOOM = 98,
            NONAME = 0,
            PA1 = 0,
            OEM_CLEAR = 0,
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct HARDWAREINPUT
        {
            internal int uMsg;
            internal short wParamL;
            internal short wParamH;
        }

        /// <summary>
        /// Synthesizes keystrokes, mouse motions, and button clicks.
        /// </summary>
        /// <param name="nInputs">
        /// The number of structures in the pInputs array.
        /// </param>
        /// <param name="pInputs">
        /// An array of INPUT structures. Each structure represents an event to be inserted into the keyboard or mouse input stream.
        /// </param>
        /// <param name="cbSize">
        /// The size, in bytes, of an INPUT structure. If cbSize is not the size of an INPUT structure, the function fails.
        /// </param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        internal static extern uint SendInput(uint nInputs,
            [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs,
            int cbSize);

        [DllImport("user32.dll")]
        internal static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);
        #endregion

        #region Virtual Key Codes

        public const int VK_CONTROL = 0x11;
        public const int VK_C = 0x67;

        #endregion

        #region EM

        public const uint ECM_FIRST = 0x1500;
        public const uint EM_SETCUEBANNER = ECM_FIRST + 1;
        public const uint EM_GETCUEBANNER = ECM_FIRST + 2;

        #endregion

        #region LB

        public const int LB_ERR = -1;
        public const int LB_SELECTSTRING = 0x18C;

        #endregion

        #region TCM

        internal const int TCM_ADJUSTRECT = 0x1328;

        #endregion

        #region WinEvent
        /// <summary>
        /// An application-defined callback (or hook) function that the system calls in
        /// response to events generated by an accessible object. The hook function processes
        /// the event notifications as required. Clients install the hook function and
        /// request specific types of event notifications by calling SetWinEventHook.
        /// </summary>
        /// <param name="hWinEventHook">
        /// Handle to an event hook function. This value is returned by
        /// SetWinEventHook when the hook function is installed and
        /// is specific to each instance of the hook function.
        /// </param>
        /// <param name="eventType">
        /// Specifies the event that occurred. This value is one of the event constants.
        /// </param>
        /// <param name="hwnd">
        /// Handle to the window that generates the event, or NULL if no window is
        /// associated with the event. For example, the mouse pointer is not associated
        /// with a window.
        /// </param>
        /// <param name="idObject">
        /// Identifies the object associated with the event. This is one of the object
        /// identifiers or a custom object ID.
        /// </param>
        /// <param name="idChild">
        /// Identifies whether the event was triggered by an object or a child element
        /// of the object. If this value is CHILDID_SELF, the event was triggered by
        /// the object; otherwise, this value is the child ID of the element that
        /// triggered the event.
        /// </param>
        /// <param name="dwEventThread"></param>
        /// <param name="dwmsEventTime">
        /// Specifies the time, in milliseconds, that the event was generated.
        /// </param>
        public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd,
            int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        /// <summary>
        /// Sets an event hook function for a range of events.
        /// </summary>
        /// <param name="eventMin">
        /// Specifies the event constant for the lowest event value in the range of
        /// events that are handled by the hook function. This parameter can be set
        /// to EVENT_MIN to indicate the lowest possible event value.
        /// </param>
        /// <param name="eventMax">
        /// Specifies the event constant for the highest event value in the range
        /// of events that are handled by the hook function. This parameter can be
        /// set to EVENT_MAX to indicate the highest possible event value.
        /// </param>
        /// <param name="hmodWinEventProc">
        /// Handle to the DLL that contains the hook function at lpfnWinEventProc,
        /// if the WINEVENT_INCONTEXT flag is specified in the dwFlags parameter.
        /// If the hook function is not located in a DLL, or if the WINEVENT_OUTOFCONTEXT
        /// flag is specified, this parameter is NULL.
        /// </param>
        /// <param name="lpfnWinEventProc">
        /// Pointer to the event hook function. For more information about this
        /// function, see WinEventProc.
        /// </param>
        /// <param name="idProcess">
        /// Specifies the ID of the process from which the hook function receives
        /// events. Specify zero (0) to receive events from all processes on the
        /// current desktop.
        /// </param>
        /// <param name="idThread">
        /// Specifies the ID of the thread from which the hook function receives
        /// events. If this parameter is zero, the hook function is associated
        /// with all existing threads on the current desktop.
        /// </param>
        /// <param name="dwFlags">
        /// Flag values that specify the location of the hook function and of
        /// the events to be skipped. Valid values are: WINEVENT_INCONTEXT,
        /// WINEVENT_OUTOFCONTEXT, WINEVENT_SKIPOWNPROCESS, WINEVENT_SKIPOWNTHREAD
        /// </param>
        /// <returns>
        /// If successful, returns an HWINEVENTHOOK value that identifies this
        /// event hook instance. Applications save this return value to use it
        /// with the UnhookWinEvent function. If unsuccessful, returns zero.
        /// </returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc,
            WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        /// <summary>
        /// Removes an event hook function created by a previous call to
        /// SetWinEventHook.
        /// </summary>
        /// <param name="hWinEventHook">
        /// Handle to the event hook returned in the previous call to
        /// SetWinEventHook.
        /// </param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        /// <summary>
        /// The foreground window has changed. The system sends
        /// this event even if the foreground window has changed
        /// to another window in the same thread. Server applications
        /// never send this event. For this event, the WinEventProc
        /// callback function's hwnd parameter is the handle to the
        /// window that is in the foreground, the idObject parameter
        /// is OBJID_WINDOW, and the idChild parameter is CHILDID_SELF.
        /// </summary>
        public const uint EVENT_SYSTEM_FOREGROUND = 0x0003;

        /// <summary>
        /// The user has released ALT+TAB. This event is sent by the
        /// system, never by servers. The hwnd parameter of the WinEventProc
        /// callback function identifies the window to which the user
        /// has switched.  If only one application is running when the
        /// user presses ALT+TAB, the system sends this event without
        /// a corresponding EVENT_SYSTEM_SWITCHSTART event.
        /// </summary>
        public const uint EVENT_SYSTEM_SWITCHSTART = 0x0014;

        /// <summary>
        /// The user has pressed ALT+TAB, which activates the switch
        /// window. This event is sent by the system, never by servers.
        /// The hwnd parameter of the WinEventProc callback function
        /// identifies the window to which the user is switching. If
        /// only one application is running when the user presses
        /// ALT+TAB, the system sends an EVENT_SYSTEM_SWITCHEND event
        /// without a corresponding EVENT_SYSTEM_SWITCHSTART event.
        /// </summary>
        public const uint EVENT_SYSTEM_SWITCHEND = 0x0015;

        public const uint WINEVENT_OUTOFCONTEXT = 0;

        #endregion

        #endregion
    }
}