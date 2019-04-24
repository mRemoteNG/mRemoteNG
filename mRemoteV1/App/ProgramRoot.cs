using System;
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
        /// The main entry point for the application.
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
            CatchAllUnhandledExceptions();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FrmSplashScreen frmSplashScreen = FrmSplashScreen.getInstance();
            frmSplashScreen.Show();
            Application.Run(FrmMain.Default);
        }

        public static void CloseSingletonInstanceMutex()
        {
            mutex?.Close();
        }

        private static void StartApplicationAsSingleInstance()
        {
            const string mutexID = "mRemoteNG_SingleInstanceMutex";
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
            if (singletonInstanceWindowHandle == IntPtr.Zero) return;
            if (NativeMethods.IsIconic(singletonInstanceWindowHandle) != 0)
                NativeMethods.ShowWindow(singletonInstanceWindowHandle, (int)NativeMethods.SW_RESTORE);
            NativeMethods.SetForegroundWindow(singletonInstanceWindowHandle);
        }

        private static IntPtr GetRunningSingletonInstanceWindowHandle()
        {
            var windowHandle = IntPtr.Zero;
            var currentProcess = Process.GetCurrentProcess();
            foreach (var enumeratedProcess in Process.GetProcessesByName(currentProcess.ProcessName))
            {
                if (enumeratedProcess.Id != currentProcess.Id &&
                    enumeratedProcess.MainModule.FileName == currentProcess.MainModule.FileName &&
                    enumeratedProcess.MainWindowHandle != IntPtr.Zero)
                    windowHandle = enumeratedProcess.MainWindowHandle;
            }

            return windowHandle;
        }

        private static void CatchAllUnhandledExceptions()
        {
            Application.ThreadException += ApplicationOnThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
        }

        private static void ApplicationOnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            if (!FrmSplashScreen.getInstance().IsDisposed)
                FrmSplashScreen.getInstance().Close();

            var window = new UnhandledExceptionWindow(e.Exception, false);
            window.ShowDialog(FrmMain.Default);
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (!FrmSplashScreen.getInstance().IsDisposed)
                FrmSplashScreen.getInstance().Close();

            var window = new UnhandledExceptionWindow(e.ExceptionObject as Exception, e.IsTerminating);
            window.ShowDialog(FrmMain.Default);
        }
    }
}