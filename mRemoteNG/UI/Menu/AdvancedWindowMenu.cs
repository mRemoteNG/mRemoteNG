using System;
using System.Globalization;
using System.Windows.Forms;
using mRemoteNG.Tools;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Menu
{
    [SupportedOSPlatform("windows")]
    // This class creates new menu items to menu that appears when you right click the top of the app (where the window title is)
    public class AdvancedWindowMenu(IWin32Window boundControl) : IDisposable
    {
        private readonly WindowMenu _windowMenu = new WindowMenu(boundControl.Handle);
        private readonly int[] _sysMenSubItems = new int[51];

        public Screen? GetScreenById(int id)
        {
            for (int i = 0; i <= _sysMenSubItems.Length - 1; i++)
            {
                if (_sysMenSubItems[i] != id) continue;
                if (i >= Screen.AllScreens.Length) continue;
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
            Array.Clear(_sysMenSubItems, 0, _sysMenSubItems.Length);
            _windowMenu.Reset();
        }

        public void BuildAdditionalMenuItems()
        {
            // option to send main form to another screen
            IntPtr popMen = WindowMenu.CreatePopupMenuItem();
            for (int i = 0; i <= Screen.AllScreens.Length - 1; i++)
            {
                _sysMenSubItems[i] = 200 + i;
                WindowMenu.AppendMenuItem(popMen, WindowMenu.Flags.MF_STRING, new IntPtr(_sysMenSubItems[i]),
                                           Language.Screen + " " + Convert.ToString(i + 1, CultureInfo.InvariantCulture));
            }
            WindowMenu.InsertMenuItem(_windowMenu.SystemMenuHandle, 0,
                WindowMenu.Flags.MF_POPUP | WindowMenu.Flags.MF_BYPOSITION, popMen,
                Language.SendTo);
            // option to show/hide menu strips
            WindowMenu.InsertMenuItem(_windowMenu.SystemMenuHandle, 1,
                WindowMenu.Flags.MF_BYPOSITION, new IntPtr(0),
                Language.ShowHideMenu);
            // separator
            WindowMenu.InsertMenuItem(_windowMenu.SystemMenuHandle, 2,
                                       WindowMenu.Flags.MF_BYPOSITION | WindowMenu.Flags.MF_SEPARATOR, IntPtr.Zero,
                                       string.Empty);
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