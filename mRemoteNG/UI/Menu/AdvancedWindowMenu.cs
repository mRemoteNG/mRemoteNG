using System;
using System.Windows.Forms;
using mRemoteNG.Tools;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Menu
{
    [SupportedOSPlatform("windows")]
    // This class creates new menu items to menu that appears when you right click the top of the app (where the window title is)
    public class AdvancedWindowMenu : IDisposable
    {
        private readonly WindowMenu _windowMenu;
        private readonly int[] _sysMenSubItems = new int[51];

        public AdvancedWindowMenu(IWin32Window boundControl)
        {
            _windowMenu = new WindowMenu(boundControl.Handle);
        }

        public Screen GetScreenById(int id)
        {
            for (var i = 0; i <= _sysMenSubItems.Length - 1; i++)
            {
                if (_sysMenSubItems[i] != id) continue;
                return Screen.AllScreens[i];
            }

            return null;
        }

        public void OnDisplayChanged(object sender, EventArgs e)
        {
            ResetScreenList();
            BuildAdditionalMenuItems();
        }

        private void ResetScreenList()
        {
            _windowMenu.Reset();
        }

        public void BuildAdditionalMenuItems()
        {
            // option to send main form to another screen
            var popMen = _windowMenu.CreatePopupMenuItem();
            for (var i = 0; i <= Screen.AllScreens.Length - 1; i++)
            {
                _sysMenSubItems[i] = 200 + i;
                _windowMenu.AppendMenuItem(popMen, WindowMenu.Flags.MF_STRING, new IntPtr(_sysMenSubItems[i]),
                                           Language.Screen + " " + Convert.ToString(i + 1));
            }
            _windowMenu.InsertMenuItem(_windowMenu.SystemMenuHandle, 0,
                WindowMenu.Flags.MF_POPUP | WindowMenu.Flags.MF_BYPOSITION, popMen,
                Language.SendTo);
            // option to show/hide menu strips
            _windowMenu.InsertMenuItem(_windowMenu.SystemMenuHandle, 1,
                WindowMenu.Flags.MF_BYPOSITION, new IntPtr(0), 
                Language.ShowHideMenu);
            // separator
            _windowMenu.InsertMenuItem(_windowMenu.SystemMenuHandle, 2,
                                       WindowMenu.Flags.MF_BYPOSITION | WindowMenu.Flags.MF_SEPARATOR, IntPtr.Zero,
                                       null);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing) return;

            _windowMenu?.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}