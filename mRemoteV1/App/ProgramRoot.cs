﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.UI.Forms;

namespace mRemoteNG.App
{
    public static class ProgramRoot
    {
        private static Mutex mutex;

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            if (Settings.Default.SingleInstance)
                StartApplicationAsSingleInstance();
            else
                StartApplication();
        }

        private static void StartApplication()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Startup.Instance.InitializeProgram();
            Application.Run(frmMain.Default);
        }

        public static void CloseSingletonInstanceMutex()
        {
            mutex?.Close();
        }

        private static void StartApplicationAsSingleInstance()
        {
            var mutexID = "mRemoteNG_SingleInstanceMutex";
            bool newInstanceCreated;
            mutex = new Mutex(false, mutexID, out newInstanceCreated);
            if (!newInstanceCreated)
            {
                SwitchToCurrentInstance();
                return;
            }

            StartApplication();
            GC.KeepAlive(mutex);
        }

        private static void SwitchToCurrentInstance()
        {
            var singletonInstanceWindowHandle = GetRunningSingletonInstanceWindowHandle();
            if (singletonInstanceWindowHandle != IntPtr.Zero)
                if (NativeMethods.IsIconic(singletonInstanceWindowHandle) != 0)
                    NativeMethods.ShowWindow(singletonInstanceWindowHandle, (int) NativeMethods.SW_RESTORE);
        }

        private static IntPtr GetRunningSingletonInstanceWindowHandle()
        {
            var windowHandle = IntPtr.Zero;
            var currentProcess = Process.GetCurrentProcess();
            foreach (var enumeratedProcess in Process.GetProcessesByName(currentProcess.ProcessName))
                if ((enumeratedProcess.Id != currentProcess.Id) &&
                    (enumeratedProcess.MainModule.FileName == currentProcess.MainModule.FileName) &&
                    (enumeratedProcess.MainWindowHandle != IntPtr.Zero))
                    windowHandle = enumeratedProcess.MainWindowHandle;
            return windowHandle;
        }
    }
}