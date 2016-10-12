using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using mRemoteNG.App;

namespace mRemoteNG.Tools
{
    public class ProcessController
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
            if ((Process == null) || Process.HasExited)
                return false;
            if (Handle == IntPtr.Zero)
                return false;

            var controlHandle = GetControlHandle(className, text);
            if (controlHandle == IntPtr.Zero)
                return false;

            uint nCmdShow = 0;
            if (visible)
                nCmdShow = NativeMethods.SW_SHOW;
            else
                nCmdShow = NativeMethods.SW_HIDE;
            NativeMethods.ShowWindow(controlHandle, (int) nCmdShow);
            return true;
        }

        public bool SetControlText(string className, string oldText, string newText)
        {
            if ((Process == null) || Process.HasExited || (Handle == IntPtr.Zero))
                return false;

            var controlHandle = GetControlHandle(className, oldText);
            if (controlHandle == IntPtr.Zero)
                return false;

            var result = NativeMethods.SendMessage(controlHandle, NativeMethods.WM_SETTEXT, (IntPtr) 0,
                new StringBuilder(newText));
            if (!(result.ToInt32() == NativeMethods.TRUE))
                return false;

            return true;
        }

        public bool SelectListBoxItem(string itemText)
        {
            if ((Process == null) || Process.HasExited || (Handle == IntPtr.Zero))
                return false;

            var listBoxHandle = GetControlHandle("ListBox");
            if (listBoxHandle == IntPtr.Zero)
                return false;

            var result = NativeMethods.SendMessage(listBoxHandle, NativeMethods.LB_SELECTSTRING, (IntPtr) (-1),
                new StringBuilder(itemText));
            if (result.ToInt32() == NativeMethods.LB_ERR)
                return false;

            return true;
        }

        public bool ClickButton(string text)
        {
            if ((Process == null) || Process.HasExited || (Handle == IntPtr.Zero))
                return false;

            var buttonHandle = GetControlHandle("Button", text);
            if (buttonHandle == IntPtr.Zero)
                return false;

            var buttonControlId = NativeMethods.GetDlgCtrlID(buttonHandle.ToInt32());
            NativeMethods.SendMessage(Handle, NativeMethods.WM_COMMAND, (IntPtr) buttonControlId, buttonHandle);

            return true;
        }

        public void WaitForExit()
        {
            if ((Process == null) || Process.HasExited)
                return;
            Process.WaitForExit();
        }

        #endregion

        #region Protected Fields

        protected Process Process = new Process();
        protected IntPtr Handle = IntPtr.Zero;
        protected List<IntPtr> Controls = new List<IntPtr>();

        #endregion

        #region Protected Methods

        protected IntPtr GetMainWindowHandle()
        {
            if ((Process == null) || Process.HasExited)
                return IntPtr.Zero;

            Process.WaitForInputIdle(Settings.Default.MaxPuttyWaitTime*1000);

            Handle = IntPtr.Zero;
            var startTicks = Environment.TickCount;
            while ((Handle == IntPtr.Zero) &&
                   (Environment.TickCount < startTicks + Settings.Default.MaxPuttyWaitTime*1000))
            {
                Process.Refresh();
                Handle = Process.MainWindowHandle;
                if (Handle == IntPtr.Zero)
                    Thread.Sleep(0);
            }

            return Handle;
        }

        protected IntPtr GetControlHandle(string className, string text = "")
        {
            if ((Process == null) || Process.HasExited || (Handle == IntPtr.Zero))
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
                if (stringBuilder.ToString() == className)
                    if (string.IsNullOrEmpty(text))
                    {
                        controlHandle = control;
                        break;
                    }
                    else
                    {
                        NativeMethods.SendMessage(control, NativeMethods.WM_GETTEXT, new IntPtr(stringBuilder.Capacity),
                            stringBuilder);
                        if (stringBuilder.ToString() == text)
                        {
                            controlHandle = control;
                            break;
                        }
                    }
            }

            return controlHandle;
        }

        #endregion
    }
}