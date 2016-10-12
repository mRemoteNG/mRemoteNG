using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace mRemoteNG.Tools
{
    public class EnvironmentInfo
    {
        public static bool IsWow64
        {
            get
            {
                var isWow64ProcessDelegate = GetIsWow64ProcessDelegate();
                if (isWow64ProcessDelegate == null)
                    return false;

                var isWow64Process = false;
                var result =
                    Convert.ToBoolean(isWow64ProcessDelegate.Invoke(Process.GetCurrentProcess().Handle,
                        ref isWow64Process));
                if (!result)
                    return false;

                return isWow64Process;
            }
        }

        private static Win32.IsWow64ProcessDelegate GetIsWow64ProcessDelegate()
        {
            var moduleHandle = Win32.LoadLibrary("kernel32");
            if (moduleHandle == IntPtr.Zero)
                return null;

            var functionPointer = Win32.GetProcAddress(moduleHandle, "IsWow64Process");
            if (functionPointer == IntPtr.Zero)
                return null;

            return
                (Win32.IsWow64ProcessDelegate)
                Marshal.GetDelegateForFunctionPointer(functionPointer, typeof(Win32.IsWow64ProcessDelegate));
        }

        protected class Win32
        {
            // ReSharper disable InconsistentNaming
            [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr LoadLibrary([In] [MarshalAs(UnmanagedType.LPTStr)] string lpFileName);

            [DllImport("kernel32", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
            public static extern IntPtr GetProcAddress([In] IntPtr hModule,
                [In] [MarshalAs(UnmanagedType.LPStr)] string lpProcName);

            public delegate bool IsWow64ProcessDelegate([In] IntPtr hProcess, ref bool Wow64Process);

            // ReSharper restore InconsistentNaming
        }
    }
}