//
// Based on code from Stephen Toub's MSDN blog at
// http://blogs.msdn.com/b/toub/archive/2006/05/03/589423.aspx
//

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace SharedLibraryNG
{
    public class KeyboardHook
    {
        // ReSharper disable InconsistentNaming
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PostMessage(IntPtr hWnd, Int32 Msg, IntPtr wParam, HookKeyMsgData lParam);
        // ReSharper restore InconsistentNaming

        [Flags]
        public enum ModifierKeys
        {
            None = 0x0000,
            Shift = 0x0001,
            LeftShift = 0x002,
            RightShift = 0x004,
            Control = 0x0008,
            LeftControl = 0x010,
            RightControl = 0x20,
            Alt = 0x0040,
            LeftAlt = 0x0080,
            RightAlt = 0x0100,
            Win = 0x0200,
            LeftWin = 0x0400,
            RightWin = 0x0800,
        }

        protected class KeyNotificationEntry
			: IEquatable<KeyNotificationEntry>
        {
            public IntPtr WindowHandle;
            public Int32 KeyCode;
            public ModifierKeys ModifierKeys;
            public Boolean Block;

            public bool Equals(KeyNotificationEntry obj)
            {
                return (WindowHandle == obj.WindowHandle &&
                        KeyCode == obj.KeyCode &&
                        ModifierKeys == obj.ModifierKeys &&
                        Block == obj.Block);
            }
        }

        public const string HookKeyMsgName = "HOOKKEYMSG-{56BE0940-34DA-11E1-B308-C6714824019B}";
	    private static Int32 _hookKeyMsg;
        public static Int32 HookKeyMsg
        {
            get
            {
	            if (_hookKeyMsg == 0)
	            {
					_hookKeyMsg = Win32.RegisterWindowMessage(HookKeyMsgName).ToInt32();
					if (_hookKeyMsg == 0)
						throw new Win32Exception(Marshal.GetLastWin32Error());
				}
	            return _hookKeyMsg;
            }
        }

        // this is a custom structure that will be passed to
        // the requested hWnd via a WM_APP_HOOKKEYMSG message
        [StructLayout(LayoutKind.Sequential)]
        public class HookKeyMsgData
        {
            public Int32 KeyCode;
            public ModifierKeys ModifierKeys;
            public Boolean WasBlocked;
        }

        private static int _referenceCount;
        private static IntPtr _hook;
        private static readonly Win32.LowLevelKeyboardProcDelegate LowLevelKeyboardProcStaticDelegate = LowLevelKeyboardProc;
        private static readonly List<KeyNotificationEntry> NotificationEntries = new List<KeyNotificationEntry>();
        
        public KeyboardHook()
        {
            _referenceCount++;
            SetHook();
        }

        ~KeyboardHook()
        {
            _referenceCount--;
            if (_referenceCount < 1) UnsetHook();
        }

        private static void SetHook()
        {
            if (_hook != IntPtr.Zero) return;

            var curProcess = Process.GetCurrentProcess();
            var curModule = curProcess.MainModule;

            var hook = Win32.SetWindowsHookEx(Win32.WH_KEYBOARD_LL, LowLevelKeyboardProcStaticDelegate, Win32.GetModuleHandle(curModule.ModuleName), 0);
            if (hook == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            _hook = hook;
        }

        private static void UnsetHook()
        {
            if (_hook == IntPtr.Zero) return;

            Win32.UnhookWindowsHookEx(_hook);
            _hook = IntPtr.Zero;
        }

        private static IntPtr LowLevelKeyboardProc(Int32 nCode, IntPtr wParam, Win32.KBDLLHOOKSTRUCT lParam)
        {
            var wParamInt = wParam.ToInt32();
            var result = 0;

            if (nCode == Win32.HC_ACTION)
            {
                switch (wParamInt)
                {
                    case Win32.WM_KEYDOWN:
                    case Win32.WM_SYSKEYDOWN:
                    case Win32.WM_KEYUP:
                    case Win32.WM_SYSKEYUP:
                        result = OnKey(wParamInt, lParam);
                        break;
                }
            }

            if (result != 0) return new IntPtr(result);

            return Win32.CallNextHookEx(_hook, nCode, wParam, lParam);
        }

        private static int OnKey(Int32 msg, Win32.KBDLLHOOKSTRUCT key)
        {
            var result = 0;

            foreach (var notificationEntry in NotificationEntries)
                if (GetFocusWindow() == notificationEntry.WindowHandle && notificationEntry.KeyCode == key.vkCode)
                {
                    var modifierKeys = GetModifierKeyState();
					if (!ModifierKeysMatch(notificationEntry.ModifierKeys, modifierKeys)) continue;

                    var wParam = new IntPtr(msg);
                    var lParam = new HookKeyMsgData
                    {
                        KeyCode = key.vkCode,
                        ModifierKeys = modifierKeys,
                        WasBlocked = notificationEntry.Block,
                    };

                    if (!PostMessage(notificationEntry.WindowHandle, HookKeyMsg, wParam, lParam))
                        throw new Win32Exception(Marshal.GetLastWin32Error());

                    if (notificationEntry.Block) result = 1;
                }

            return result;
        }

        private static IntPtr GetFocusWindow()
        {
            var guiThreadInfo = new Win32.GUITHREADINFO();
            if (!Win32.GetGUIThreadInfo(0, guiThreadInfo))
                throw new Win32Exception(Marshal.GetLastWin32Error());
			return Win32.GetAncestor(guiThreadInfo.hwndFocus, Win32.GA_ROOT);
        }

        protected static Dictionary<Int32, ModifierKeys> ModifierKeyTable = new Dictionary<Int32, ModifierKeys>
        {
            { Win32.VK_SHIFT, ModifierKeys.Shift },
            { Win32.VK_LSHIFT, ModifierKeys.LeftShift },
            { Win32.VK_RSHIFT, ModifierKeys.RightShift },
            { Win32.VK_CONTROL, ModifierKeys.Control },
            { Win32.VK_LCONTROL, ModifierKeys.LeftControl },
            { Win32.VK_RCONTROL, ModifierKeys.RightControl },
            { Win32.VK_MENU, ModifierKeys.Alt },
            { Win32.VK_LMENU, ModifierKeys.LeftAlt },
            { Win32.VK_RMENU, ModifierKeys.RightAlt },
            { Win32.VK_LWIN, ModifierKeys.LeftWin },
            { Win32.VK_RWIN, ModifierKeys.RightWin },
        };

        public static ModifierKeys GetModifierKeyState()
        {
            var modifierKeyState = ModifierKeys.None;

            foreach (KeyValuePair<Int32, ModifierKeys> pair in ModifierKeyTable)
            {
                if ((Win32.GetAsyncKeyState(pair.Key) & Win32.KEYSTATE_PRESSED) != 0) modifierKeyState |= pair.Value;
            }

	        if ((modifierKeyState & ModifierKeys.LeftWin) != 0) modifierKeyState |= ModifierKeys.Win;
			if ((modifierKeyState & ModifierKeys.RightWin) != 0) modifierKeyState |= ModifierKeys.Win;

            return modifierKeyState;
        }

		public static Boolean ModifierKeysMatch(ModifierKeys requestedKeys, ModifierKeys pressedKeys)
		{
			if ((requestedKeys & ModifierKeys.Shift) != 0) pressedKeys &= ~(ModifierKeys.LeftShift | ModifierKeys.RightShift);
			if ((requestedKeys & ModifierKeys.Control) != 0) pressedKeys &= ~(ModifierKeys.LeftControl | ModifierKeys.RightControl);
			if ((requestedKeys & ModifierKeys.Alt) != 0) pressedKeys &= ~(ModifierKeys.LeftAlt | ModifierKeys.RightAlt);
			if ((requestedKeys & ModifierKeys.Win) != 0) pressedKeys &= ~(ModifierKeys.LeftWin | ModifierKeys.RightWin);
			return requestedKeys == pressedKeys;
		}

        public static void RequestKeyNotification(IntPtr windowHandle, Int32 keyCode, Boolean block)
        {
            RequestKeyNotification(windowHandle, keyCode, ModifierKeys.None, block);
        }

        public static void RequestKeyNotification(IntPtr windowHandle, Int32 keyCode, ModifierKeys modifierKeys = ModifierKeys.None, Boolean block = false)
        {
            var newNotificationEntry = new KeyNotificationEntry
            {
                WindowHandle = windowHandle,
                KeyCode = keyCode,
                ModifierKeys = modifierKeys,
                Block = block,
            };

            foreach (var notificationEntry in NotificationEntries)
                if (notificationEntry == newNotificationEntry) return;

            NotificationEntries.Add(newNotificationEntry);
        }

		public static void CancelKeyNotification(IntPtr windowHandle, Int32 keyCode, Boolean block)
		{
			CancelKeyNotification(windowHandle, keyCode, ModifierKeys.None, block);
		}

		public static void CancelKeyNotification(IntPtr windowHandle, Int32 keyCode, ModifierKeys modifierKeys = ModifierKeys.None, Boolean block = false)
		{
			var notificationEntry = new KeyNotificationEntry
			{
				WindowHandle = windowHandle,
				KeyCode = keyCode,
				ModifierKeys = modifierKeys,
				Block = block,
			};

			NotificationEntries.Remove(notificationEntry);
		}
    }
}
