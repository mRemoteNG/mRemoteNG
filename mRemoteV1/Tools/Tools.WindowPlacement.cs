using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using mRemoteNG.App;

namespace mRemoteNG.Tools
{
    public class WindowPlacement
    {
        public WindowPlacement(Form form)
        {
            Form = form;
        }

        #region Public Properties

        public Form Form { get; set; }

        public bool RestoreToMaximized
        {
            get
            {
                var windowPlacement = GetWindowPlacement();
                return Convert.ToBoolean(windowPlacement.flags & NativeMethods.WPF_RESTORETOMAXIMIZED);
            }
            set
            {
                var windowPlacement = GetWindowPlacement();
                if (value)
                    windowPlacement.flags = windowPlacement.flags | NativeMethods.WPF_RESTORETOMAXIMIZED;
                else
                    windowPlacement.flags = windowPlacement.flags & ~NativeMethods.WPF_RESTORETOMAXIMIZED;
                SetWindowPlacement(windowPlacement);
            }
        }

        #endregion

        #region Private Functions

        private NativeMethods.WINDOWPLACEMENT GetWindowPlacement()
        {
            if (Form == null)
                throw new NullReferenceException("WindowPlacement.Form is not set.");
            var windowPlacement = new NativeMethods.WINDOWPLACEMENT();
            windowPlacement.length = (uint) Marshal.SizeOf(windowPlacement);
            try
            {
                NativeMethods.GetWindowPlacement(Form.Handle, ref windowPlacement);
                return windowPlacement;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool SetWindowPlacement(NativeMethods.WINDOWPLACEMENT windowPlacement)
        {
            if (Form == null)
                throw new NullReferenceException("WindowPlacement.Form is not set.");
            windowPlacement.length = (uint) Marshal.SizeOf(windowPlacement);
            try
            {
                return NativeMethods.SetWindowPlacement(Form.Handle, ref windowPlacement);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}