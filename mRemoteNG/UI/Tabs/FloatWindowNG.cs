using System.Drawing;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.UI.Tabs
{
    internal class FloatWindowNG : FloatWindow
    {
        public FloatWindowNG(DockPanel dockPanel, DockPane pane)
            : base(dockPanel, pane)
        {
        }

        public FloatWindowNG(DockPanel dockPanel, DockPane pane, Rectangle bounds)
            : base(dockPanel, pane, bounds)
        {
        }
    }
}