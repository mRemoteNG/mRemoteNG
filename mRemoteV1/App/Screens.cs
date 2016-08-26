using System.Windows.Forms;
using mRemoteNG.UI.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.App
{
    public static class Screens
    {
        public static void SendFormToScreen(Screen Screen)
        {
            bool wasMax = false;

            if (frmMain.Default.WindowState == FormWindowState.Maximized)
            {
                wasMax = true;
                frmMain.Default.WindowState = FormWindowState.Normal;
            }

            frmMain.Default.Location = Screen.Bounds.Location;

            if (wasMax)
            {
                frmMain.Default.WindowState = FormWindowState.Maximized;
            }
        }

        public static void SendPanelToScreen(DockContent Panel, Screen Screen)
        {
            Panel.DockState = DockState.Float;
            if (Panel.ParentForm != null)
            {
                Panel.ParentForm.Left = Screen.Bounds.Location.X;
                Panel.ParentForm.Top = Screen.Bounds.Location.Y;
            }
        }
    }
}