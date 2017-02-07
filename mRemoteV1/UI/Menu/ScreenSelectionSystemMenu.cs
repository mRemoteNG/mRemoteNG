using System;
using System.Windows.Forms;
using mRemoteNG.Tools;

namespace mRemoteNG.UI.Menu
{
    // This class creates new menu items to menu that appears when you right click the top of the app (where the window title is)
    public class ScreenSelectionSystemMenu
    {
        private readonly SystemMenu _systemMenu;
        private readonly int[] _sysMenSubItems = new int[51];

        public ScreenSelectionSystemMenu(IWin32Window boundControl)
        {
            _systemMenu = new SystemMenu(boundControl.Handle);
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
            BuildScreenList();
        }

        public void ResetScreenList()
        {
            _systemMenu.Reset();
        }

        public void BuildScreenList()
        {
            var popMen = _systemMenu.CreatePopupMenuItem();

            for (var i = 0; i <= Screen.AllScreens.Length - 1; i++)
            {
                _sysMenSubItems[i] = 200 + i;
                _systemMenu.AppendMenuItem(popMen, SystemMenu.Flags.MF_STRING, new IntPtr(_sysMenSubItems[i]), Language.strScreen + " " + Convert.ToString(i + 1));
            }

            _systemMenu.InsertMenuItem(_systemMenu.SystemMenuHandle, 0, SystemMenu.Flags.MF_POPUP | SystemMenu.Flags.MF_BYPOSITION, popMen, Language.strSendTo);
            _systemMenu.InsertMenuItem(_systemMenu.SystemMenuHandle, 1, SystemMenu.Flags.MF_BYPOSITION | SystemMenu.Flags.MF_SEPARATOR, IntPtr.Zero, null);
        }
    }
}