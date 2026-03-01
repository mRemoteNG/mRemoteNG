using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.UI.Tabs
{
    class FloatWindowNG : FloatWindow
    {
        public FloatWindowNG(DockPanel dockPanel, DockPane pane)
            : base(dockPanel, pane)
        {
            setDefaultProperties();
        }

        public FloatWindowNG(DockPanel dockPanel, DockPane pane, Rectangle bounds)
            : base(dockPanel, pane, bounds)
        {
            setDefaultProperties();
        }

        private void setDefaultProperties()
        {
            FormBorderStyle = FormBorderStyle.Sizable;

            // To enable Alt+Tab between your undocked forms and your main form
            ShowInTaskbar = true;
            Owner = null;

            // Allow the Windows default behavior of maximizing/restoring the window
            DoubleClickTitleBarToDock = true;
        }

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern uint SendMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

        //[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            int WM_NCLBUTTONDOWN = 0x00A1;
            int WM_SYSCOMMAND = 0x0112;

            int SC_MINIMIZE = 0xF020;
            int SC_RESTORE = 0xF120;

            if (m.Msg == WM_NCLBUTTONDOWN)
            {
                if (IsDisposed)
                    return;

                if ((uint)m.WParam == 8) // Check if button down occured in minimize box
                {
                    if (WindowState == FormWindowState.Minimized)
                        _ = FloatWindowNG.SendMessage(Handle, (int)WM_SYSCOMMAND, (uint)SC_RESTORE, 0);
                    else
                        _ = FloatWindowNG.SendMessage(Handle, (int)WM_SYSCOMMAND, (uint)SC_MINIMIZE, 0);

                    return;
                }
            }

            base.WndProc(ref m);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (Owner != null)
            {
                Owner = null;
            }

            if (!ShowInTaskbar)
            {
                ShowInTaskbar = true;
            }
        }
    }
}