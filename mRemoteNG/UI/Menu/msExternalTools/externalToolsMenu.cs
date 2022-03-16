using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mRemoteNG.UI.Window;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Panels;

namespace mRemoteNG.UI.Menu.msExternalTools
{
    public partial class externalToolsMenu : ToolStripMenuItem
    {
        public externalToolsMenu()
        {
            Initialize();
        }

        public externalToolsMenu(IContainer container)
        {
            container.Add(this);

            Initialize();
        }

        private void Initialize()
        {
        }
    }
}
