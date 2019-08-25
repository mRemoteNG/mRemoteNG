using mRemoteNG.App;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace mRemoteNG.UI
{
    public class SystemKeyboardHook : IDisposable
    {
        private readonly IntPtr _hookId;
        private readonly Func<int, NativeMethods.KBDLLHOOKSTRUCT, int> _userCallback;
        private readonly NativeMethods.LowLevelKeyboardProc _sysCallback;
        private bool _disposed;

        public SystemKeyboardHook(Func<int, NativeMethods.KBDLLHOOKSTRUCT, int> proc)
        {
            _userCallback = proc;
            _sysCallback = SystemCallback;
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                _hookId = NativeMethods.SetWindowsHookEx(
                    NativeMethods.WH_KEYBOARD_LL,
                    _sysCallback,
                    NativeMethods.GetModuleHandle(curModule.ModuleName),
                    0);
            }
        }

        private IntPtr SystemCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var kdb = Marshal.PtrToStructure<NativeMethods.KBDLLHOOKSTRUCT>(lParam);
                return new IntPtr(_userCallback(wParam.ToInt32(), kdb));
            }

            return NativeMethods.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                NativeMethods.UnhookWindowsHookEx(_hookId);
            }

            _disposed = true;
        }
    }
}
