using System.Windows.Forms;
using mRemoteNG.Tools;
using mRemoteNG.UI.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.App
{
    public class Screens
    {
        private readonly FrmMain _frmMain;

        public Screens(FrmMain frmMain)
        {
            _frmMain = frmMain.ThrowIfNull(nameof(frmMain));
        }

        public void SendFormToScreen(Screen screen)
        {
            var wasMax = false;

            if (_frmMain.WindowState == FormWindowState.Maximized)
            {
                wasMax = true;
                _frmMain.WindowState = FormWindowState.Normal;
            }

            _frmMain.Location = screen.Bounds.Location;

            if (wasMax)
            {
                _frmMain.WindowState = FormWindowState.Maximized;
            }
        }

        public void SendPanelToScreen(DockContent panel, Screen screen)
        {
            panel.DockState = DockState.Float;
            if (panel.ParentForm == null) return;
            panel.ParentForm.Left = screen.Bounds.Location.X;
            panel.ParentForm.Top = screen.Bounds.Location.Y;
        }
    }
}