using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Text;
using mRemoteNG.App;
using mRemoteNG.Properties;
using mRemoteNG.Tools.Cmdline;
using System.Runtime.Versioning;

namespace mRemoteNG.Tools
{
    [SupportedOSPlatform("windows")]
    public class ProcessController : IDisposable
    {
        #region Public Methods

        public bool Start(string fileName, CommandLineArguments arguments = null)
        {
            Process.StartInfo.UseShellExecute = false;
            Process.StartInfo.FileName = fileName;
            if (arguments != null)
                Process.StartInfo.Arguments = arguments.ToString();

            if (!Process.Start())
                return false;
            GetMainWindowHandle();

            return true;
        }

        public bool SetControlVisible(string className, string text, bool visible = true)
        {
            if (Process == null || Process.HasExited)
                return false;
            if (Handle == IntPtr.Zero)
                return false;

            var controlHandle = GetControlHandle(className, text);
            if (controlHandle == IntPtr.Zero)
                return false;

            var nCmdShow = visible ? NativeMethods.SW_SHOW : NativeMethods.SW_HIDE;
            NativeMethods.ShowWindow(controlHandle, (int)nCmdShow);
            return true;
        }

        public bool SetControlText(string className, string oldText, string newText)
        {
            if (Process == null || Process.HasExited || Handle == IntPtr.Zero)
                return false;

            var controlHandle = GetControlHandle(className, oldText);
            if (controlHandle == IntPtr.Zero)
                return false;

            var result = NativeMethods.SendMessage(controlHandle, NativeMethods.WM_SETTEXT, (IntPtr)0,
                                                   new StringBuilder(newText));
            return result.ToInt32() == NativeMethods.TRUE;
        }

        public bool SelectListBoxItem(string itemText)
        {
            if (Process == null || Process.HasExited || Handle == IntPtr.Zero)
                return false;

            var listBoxHandle = GetControlHandle("ListBox");
            if (listBoxHandle == IntPtr.Zero)
                return false;

            var result = NativeMethods.SendMessage(listBoxHandle, NativeMethods.LB_SELECTSTRING, (IntPtr)(-1),
                                                   new StringBuilder(itemText));
            return result.ToInt32() != NativeMethods.LB_ERR;
        }

        public bool ClickButton(string text)
        {
            if (Process == null || Process.HasExited || Handle == IntPtr.Zero)
                return false;

            var buttonHandle = GetControlHandle("Button", text);
            if (buttonHandle == IntPtr.Zero)
                return false;

            var buttonControlId = NativeMethods.GetDlgCtrlID(buttonHandle);
            NativeMethods.SendMessage(Handle, NativeMethods.WM_COMMAND, (IntPtr)buttonControlId, buttonHandle);

            return true;
        }

        public void WaitForExit()
        {
            if (Process == null || Process.HasExited)
                return;
            Process.WaitForExit();
        }

        #endregion

        #region Protected Fields

        private readonly Process Process = new Process();
        private IntPtr Handle = IntPtr.Zero;
        private List<IntPtr> Controls = new List<IntPtr>();

        #endregion

        #region Protected Methods

        // ReSharper disable once UnusedMethodReturnValue.Local
        private IntPtr GetMainWindowHandle()
        {
            if (Process == null || Process.HasExited)
                return IntPtr.Zero;

            Process.WaitForInputIdle(Properties.OptionsAdvancedPage.Default.MaxPuttyWaitTime * 1000);

            Handle = IntPtr.Zero;
            var startTicks = Environment.TickCount;
            while (Handle == IntPtr.Zero &&
                   Environment.TickCount < startTicks + (Properties.OptionsAdvancedPage.Default.MaxPuttyWaitTime * 1000))
            {
                Process.Refresh();
                Handle = Process.MainWindowHandle;
                if (Handle == IntPtr.Zero)
                {
                    System.Threading.Thread.Sleep(0);
                }
            }

            return Handle;
        }

        private IntPtr GetControlHandle(string className, string text = "")
        {
            if (Process == null || Process.HasExited || Handle == IntPtr.Zero)
                return IntPtr.Zero;

            if (Controls.Count == 0)
            {
                var windowEnumerator = new EnumWindows();
                Controls = windowEnumerator.EnumChildWindows(Handle);
            }

            var stringBuilder = new StringBuilder();
            var controlHandle = IntPtr.Zero;
            foreach (var control in Controls)
            {
                NativeMethods.GetClassName(control, stringBuilder, stringBuilder.Capacity);
                if (stringBuilder.ToString() != className) continue;
                if (string.IsNullOrEmpty(text))
                {
                    controlHandle = control;
                    break;
                }
                else
                {
                    NativeMethods.SendMessage(control, NativeMethods.WM_GETTEXT, new IntPtr(stringBuilder.Capacity), stringBuilder);
                    if (stringBuilder.ToString() != text) continue;
                    controlHandle = control;
                    break;
                }
            }

            return controlHandle;
        }

        #endregion

        private void Dispose(bool disposing)
        {
            if (!disposing) return;

            if(Process != null)
                Process.Dispose();

            Handle = IntPtr.Zero;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}