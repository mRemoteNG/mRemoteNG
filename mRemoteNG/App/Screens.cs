using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.UI.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.App
{
    public static class Screens
    {
        [SupportedOSPlatform("windows")]
        public static void SendFormToScreen(Screen screen)
        {
            var frmMain = FrmMain.Default;
            var wasMax = false;

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

        public static void SendPanelToScreen(DockContent panel, Screen screen)
        {
            panel.DockState = DockState.Float;
            if (panel.ParentForm == null) return;
            panel.ParentForm.Left = screen.Bounds.Location.X;
            panel.ParentForm.Top = screen.Bounds.Location.Y;
        }
    }
}