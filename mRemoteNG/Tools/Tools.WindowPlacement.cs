using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using mRemoteNG.App;
using System.Runtime.Versioning;

namespace mRemoteNG.Tools
{
    [SupportedOSPlatform("windows")]
    public class WindowPlacement
    {
        private Form _form;


        public WindowPlacement(Form form)
        {
            _form = form;
        }


        #region Public Properties

        public Form Form
        {
            get => _form;
            set => _form = value;
        }

        public bool RestoreToMaximized
        {
            get
            {
                NativeMethods.WINDOWPLACEMENT windowPlacement = GetWindowPlacement();
                return Convert.ToBoolean(windowPlacement.flags & NativeMethods.WPF_RESTORETOMAXIMIZED);
            }
            set
            {
                NativeMethods.WINDOWPLACEMENT windowPlacement = GetWindowPlacement();
                if (value)
                {
                    windowPlacement.flags = windowPlacement.flags | NativeMethods.WPF_RESTORETOMAXIMIZED;
                }
                else
                {
                    windowPlacement.flags = windowPlacement.flags & ~NativeMethods.WPF_RESTORETOMAXIMIZED;
                }

                SetWindowPlacement(windowPlacement);
            }
        }

        #endregion

        #region Private Functions

        private NativeMethods.WINDOWPLACEMENT GetWindowPlacement()
        {
            if (_form == null)
            {
                throw (new NullReferenceException("WindowPlacement.Form is not set."));
            }

            NativeMethods.WINDOWPLACEMENT windowPlacement = new NativeMethods.WINDOWPLACEMENT();
            windowPlacement.length = (uint)Marshal.SizeOf(windowPlacement);
            try
            {
                NativeMethods.GetWindowPlacement(_form.Handle, ref windowPlacement);
                return windowPlacement;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool SetWindowPlacement(NativeMethods.WINDOWPLACEMENT windowPlacement)
        {
            if (_form == null)
            {
                throw (new NullReferenceException("WindowPlacement.Form is not set."));
            }

            windowPlacement.length = (uint)Marshal.SizeOf(windowPlacement);
            try
            {
                return NativeMethods.SetWindowPlacement(_form.Handle, ref windowPlacement);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}