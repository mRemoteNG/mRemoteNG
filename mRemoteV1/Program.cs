using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Forms.OptionsPages;
using Microsoft.VisualBasic.CompilerServices;

namespace mRemoteNG
{
    
    public class Program
    {
        public static Mutex mutex;
        [STAThread]
        public static void Main(string[] args)
        {
            if (Settings.Default.SingleInstance)
            {
                mutex = new Mutex(false, $"Global\\{appGuid}");
                if (!mutex.WaitOne(0, false))
                {
                    try
                    {
                        SwitchToCurrentInstance();
                    }
                    catch (Exception)
                    {
                    }

                    ProjectData.EndApp();
                }

                GC.KeepAlive(mutex);
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                var mainForm = new frmMain();
                frmMain.Default = mainForm;
                Application.Run(mainForm);
            }
        }

        private static void SwitchToCurrentInstance()
        {
            IntPtr hWnd = GetCurrentInstanceWindowHandle();
            if (hWnd != IntPtr.Zero)
            {
                //Restore window if minimized. Do not restore if already in
                //normal or maximised window state, since we don't want to
                //change the current state of the window.
                if (App.Native.IsIconic(hWnd) != 0)
                {
                    App.Native.ShowWindow(hWnd, App.Native.SW_RESTORE);
                }
                App.Native.SetForegroundWindow(hWnd);
            }
        }

        private static IntPtr GetCurrentInstanceWindowHandle()
        {
            var hWnd = IntPtr.Zero;
            var curProc = Process.GetCurrentProcess();
            foreach (var proc in Process.GetProcessesByName(curProc.ProcessName))
            {
                if (proc.Id != curProc.Id && proc.MainModule.FileName == curProc.MainModule.FileName && proc.MainWindowHandle != IntPtr.Zero)
                {
                    hWnd = proc.MainWindowHandle;
                    break;
                }
            }
            return hWnd;
        }
        private static string appGuid = "0AF940D9-0700-4F95-A85C-9D99149EDE07";
    }
}
