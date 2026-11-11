using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.App
{
    public static class Screens
    {
        [SupportedOSPlatform("windows")]
        public static void SendFormToScreen(Screen screen)
        {
            FrmMain frmMain = FrmMain.Default;
            bool wasMax = false;

            if (frmMain.WindowState == FormWindowState.Maximized)
            {
                wasMax = true;
                frmMain.WindowState = FormWindowState.Normal;
            }

            frmMain.Location = screen.Bounds.Location;

            if (wasMax)
            {
                frmMain.WindowState = FormWindowState.Maximized;
            }
        }

        public static void SendPanelToScreen(DockContent panel, Screen screen, bool rdpFullscreen = false)
        {
            panel.DockState = DockState.Float;
            if (panel.ParentForm == null) return;
            // move first so any per-monitor DPI rescale happens before we size the window
            panel.ParentForm.Location = screen.WorkingArea.Location;
            panel.ParentForm.Bounds = screen.WorkingArea;
            // RDP fullscreen uses whichever monitor the control is on, so this must come after the move
            if (rdpFullscreen)
                (panel as ConnectionWindow)?.EnterRdpFullscreen();
        }
    }
}