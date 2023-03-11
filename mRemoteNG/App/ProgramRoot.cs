using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.Versioning;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using mRemoteNG.Properties;
using mRemoteNG.UI.Forms;

namespace mRemoteNG.App
{
    [SupportedOSPlatform("windows")]
    public static class ProgramRoot
    {
        private static Mutex _mutex;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            if (Properties.OptionsStartupExitPage.Default.SingleInstance)
                StartApplicationAsSingleInstance();
            else
                StartApplication();
        }

        private static void StartApplication()
        {
            CatchAllUnhandledExceptions();
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            var frmSplashScreen = FrmSplashScreenNew.GetInstance();

            var targetScreen = Screen.PrimaryScreen;

            Rectangle viewport = targetScreen.WorkingArea;
            frmSplashScreen.Top = viewport.Top;
            frmSplashScreen.Left = viewport.Left;
            // normaly it should be screens[1] however due DPI apply 1 size "same" as default with 100%
                frmSplashScreen.Left = viewport.Left + (targetScreen.Bounds.Size.Width / 2) - (frmSplashScreen.Width / 2);
                frmSplashScreen.Top = viewport.Top + (targetScreen.Bounds.Size.Height / 2) - (frmSplashScreen.Height / 2);
            
            frmSplashScreen.Show();
            
            System.Windows.Forms.Application.Run(FrmMain.Default);
        }

        public static void CloseSingletonInstanceMutex()
        {
            _mutex?.Close();
        }

        private static void StartApplicationAsSingleInstance()
        {
            const string mutexID = "mRemoteNG_SingleInstanceMutex";
            _mutex = new Mutex(false, mutexID, out var newInstanceCreated);
            if (!newInstanceCreated)
            {
                SwitchToCurrentInstance();
                return;
            }

            StartApplication();
            GC.KeepAlive(_mutex);
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
            System.Windows.Forms.Application.ThreadException += ApplicationOnThreadException;
            System.Windows.Forms.Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
        }

        private static void ApplicationOnThreadException(object sender, ThreadExceptionEventArgs e)
        {
           // if (PresentationSource.FromVisual(FrmSplashScreenNew))
                FrmSplashScreenNew.GetInstance().Close();

            if (FrmMain.Default.IsDisposed) return;
            
            var window = new FrmUnhandledException(e.Exception, false);
            window.ShowDialog(FrmMain.Default);
            
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //TODO: Check if splash closed properly
            //if (!FrmSplashScreenNew.GetInstance().IsDisposed)
            //    FrmSplashScreenNew.GetInstance().Close();

            var window = new FrmUnhandledException(e.ExceptionObject as Exception, e.IsTerminating);
            window.ShowDialog(FrmMain.Default);
        }
    }
}