﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.UI.Tabs
{
    class FloatWindowNG : FloatWindow
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