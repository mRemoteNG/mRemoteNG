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
        /// Sent when an application requests that a window be created by calling the CreateWindowEx or CreateWindow function. (The message is sent before the function returns.) The window procedure of the new window receives this message after the window is created, but before the window becomes visible.
        /// </summary>
        public const int WM_CREATE = 0x1;

        /// <summary>
        /// Sent when a window is being destroyed. It is sent to the window procedure of the window being destroyed after the window is removed from the screen. This message is sent first to the window being destroyed and then to the child windows(if any) as they are destroyed.During the processing of the message, it can be assumed that all child windows still exist.
        /// </summary>
        public const int WM_DESTROY = 0x2;

        /// <summary>
        /// Sent to both the window being activated and the window being deactivated. If the windows use the same input queue, the message is sent synchronously, first to the window procedure of the top-level window being deactivated, then to the window procedure of the top-level window being activated. If the windows use different input queues, the message is sent asynchronously, so the window is activated immediately.
        /// </summary>
        public const int WM_ACTIVATE = 0x6;

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
        /// Sent when a window belonging to a different application than the active window is about to be activated. The message is sent to the application whose window is being activated and to the application whose window is being deactivated.
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