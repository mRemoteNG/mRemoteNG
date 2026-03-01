using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;

namespace mRemoteNG.Tools
{
    public class EnumWindows
    {
        public IList<IntPtr> GetWindowHandles()
        {
            List<IntPtr> handleList = new();

            HandleLists.Add(handleList);
            IntPtr handleIndex = (IntPtr)HandleLists.IndexOf(handleList);
            NativeMethods.EnumWindows(EnumCallback, handleIndex);
            HandleLists.Remove(handleList);

            return handleList;
        }

        public IList<IntPtr> EnumChildWindows(IntPtr hWndParent)
        {
            List<IntPtr> handleList = new();

            HandleLists.Add(handleList);
            IntPtr handleIndex = (IntPtr)HandleLists.IndexOf(handleList);
            NativeMethods.EnumChildWindows(hWndParent, EnumCallback, handleIndex);
            HandleLists.Remove(handleList);

            return handleList;
        }

        private readonly List<List<IntPtr>> HandleLists = [];

        private bool EnumCallback(int hwnd, int lParam)
        {
            HandleLists[lParam].Add((IntPtr)hwnd);
            return true;
        }

        // ReSharper disable ClassNeverInstantiated.Local
        private static class NativeMethods
        {
            // ReSharper restore ClassNeverInstantiated.Local

            public delegate bool EnumWindowsProc(int hwnd, int lParam);

            [DllImport("user32", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
            public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

            [DllImport("user32", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
            public static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);
        }
    }
}