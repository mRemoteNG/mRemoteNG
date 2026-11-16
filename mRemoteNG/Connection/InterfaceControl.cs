using mRemoteNG.App;
using mRemoteNG.Connection.Protocol;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using mRemoteNG.UI.Tabs;
using WeifenLuo.WinFormsUI.Docking;
using System.Runtime.Versioning;

namespace mRemoteNG.Connection
{
    [SupportedOSPlatform("windows")]
    public sealed partial class InterfaceControl
    {
        public ProtocolBase Protocol { get; set; }
        public ConnectionInfo Info { get; set; }
        // in case the connection is through a SSH tunnel the Info is a copy of original info with hostname and port number overwritten with localhost and local tunnel port
        // and the original Info is saved in the following variable
        public ConnectionInfo OriginalInfo { get; set; }
        // in case the connection is through a SSH tunnel the Info of the SSHTunnelConnection is also saved for reference in log messages etc.
        public ConnectionInfo SSHTunnelInfo { get; set; }

        // Width in pixels of the colored connection frame drawn in the non-client area.
        private const int FrameBorderWidth = 4;

        private const int WM_NCCALCSIZE = 0x0083;
        private const int WM_NCPAINT = 0x0085;


        public InterfaceControl(Control parent, ProtocolBase protocol, ConnectionInfo info)
        {
            try
            {
                Protocol = protocol;
                Info = info;
                Parent = parent;
                Location = new Point(0, 0);
                Size = Parent.Size;
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg,
                                                    "Couldn\'t create new InterfaceControl" + Environment.NewLine +
                                                    ex.Message);
            }
        }

        private bool HasFrameColor =>
            Info != null && Info.ConnectionFrameColor != ConnectionFrameColor.None;

        /// <summary>
        /// Recalculates the non-client area and repaints the connection frame. Call this after
        /// <see cref="Info"/>.ConnectionFrameColor changes so the border is shown or hidden immediately.
        /// </summary>
        public void RefreshFrameColor()
        {
            if (!IsHandleCreated)
                return;

            // Force the window to recalculate its non-client area (WM_NCCALCSIZE) and repaint the frame.
            SetWindowPos(Handle, IntPtr.Zero, 0, 0, 0, 0,
                         SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_NOACTIVATE | SWP_FRAMECHANGED);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCCALCSIZE when HasFrameColor && m.WParam != IntPtr.Zero:
                    // Reserve the frame width as non-client area so child controls cannot paint over it.
                    base.WndProc(ref m);
                    NCCALCSIZE_PARAMS nccsp = Marshal.PtrToStructure<NCCALCSIZE_PARAMS>(m.LParam);
                    nccsp.rgrc0.left += FrameBorderWidth;
                    nccsp.rgrc0.top += FrameBorderWidth;
                    nccsp.rgrc0.right -= FrameBorderWidth;
                    nccsp.rgrc0.bottom -= FrameBorderWidth;
                    Marshal.StructureToPtr(nccsp, m.LParam, false);
                    return;
                case WM_NCPAINT when HasFrameColor:
                    base.WndProc(ref m);
                    PaintFrame();
                    return;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        private void PaintFrame()
        {
            IntPtr hdc = NativeMethods.GetWindowDC(Handle);
            if (hdc == IntPtr.Zero)
                return;

            try
            {
                using Graphics g = Graphics.FromHdc(hdc);
                using Pen pen = new Pen(GetFrameColor(Info.ConnectionFrameColor), FrameBorderWidth);
                Rectangle rect = new Rectangle(
                    FrameBorderWidth / 2,
                    FrameBorderWidth / 2,
                    Width - FrameBorderWidth,
                    Height - FrameBorderWidth);
                g.DrawRectangle(pen, rect);
            }
            finally
            {
                NativeMethods.ReleaseDC(Handle, hdc);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct NCCALCSIZE_PARAMS
        {
            public RECT rgrc0;
            public RECT rgrc1;
            public RECT rgrc2;
            public IntPtr lppos;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_NOACTIVATE = 0x0010;
        private const uint SWP_FRAMECHANGED = 0x0020;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
                                                int X, int Y, int cx, int cy, uint uFlags);

        private Color GetFrameColor(ConnectionFrameColor frameColor)
        {
            return frameColor switch
            {
                ConnectionFrameColor.Red => Color.FromArgb(220, 53, 69),      // Bootstrap danger red
                ConnectionFrameColor.Yellow => Color.FromArgb(255, 193, 7),   // Warning yellow
                ConnectionFrameColor.Green => Color.FromArgb(40, 167, 69),    // Success green
                ConnectionFrameColor.Blue => Color.FromArgb(0, 123, 255),     // Primary blue
                ConnectionFrameColor.Purple => Color.FromArgb(111, 66, 193),  // Purple
                _ => Color.Transparent
            };
        }

        public static InterfaceControl FindInterfaceControl(DockPanel DockPnl)
        {
            // instead of repeating the code, call the routine using ConnectionTab if called by DockPanel
            if (DockPnl.ActiveDocument is ConnectionTab ct)
                return FindInterfaceControl(ct);
            return null;
        }

        public static InterfaceControl FindInterfaceControl(ConnectionTab tab)
        {
            if (tab.Controls.Count < 1) return null;
            // if the tab has more than one controls and the second is an InterfaceControl than it must be a connection through SSH tunnel
            // and the first Control is the SSH tunnel connection and thus the second control must be returned.
            if (tab.Controls.Count > 1)
            {
                if (tab.Controls[1] is InterfaceControl ic1)
                    return ic1;
            }
            if (tab.Controls[0] is InterfaceControl ic0)
                return ic0;

            return null;
        }
    }
}