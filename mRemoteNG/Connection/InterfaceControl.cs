using mRemoteNG.App;
using mRemoteNG.Connection.Protocol;
using System;
using System.Drawing;
using System.Windows.Forms;
using mRemoteNG.UI.Tabs;
using WeifenLuo.WinFormsUI.Docking;
using System.Runtime.Versioning;

namespace mRemoteNG.Connection
{
    [SupportedOSPlatform("windows")]
    public sealed partial class InterfaceControl
    {
        public ProtocolBase Protocol { get; set; }
        public ConnectionInfo Info { get; set; }
        // in case the connection is through a SSH tunnel the Info is a copy of original info with hostname and port number overwritten with localhost and local tunnel port
        // and the original Info is saved in the following variable
        public ConnectionInfo OriginalInfo { get; set; }
        // in case the connection is through a SSH tunnel the Info of the SSHTunnelConnection is also saved for reference in log messages etc.
        public ConnectionInfo SSHTunnelInfo { get; set; }


        public InterfaceControl(Control parent, ProtocolBase protocol, ConnectionInfo info)
        {
            try
            {
                Protocol = protocol;
                Info = info;
                Parent = parent;
                Location = new Point(0, 0);
                Size = Parent.Size;
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg,
                                                    "Couldn\'t create new InterfaceControl" + Environment.NewLine +
                                                    ex.Message);
            }
        }

        public static InterfaceControl FindInterfaceControl(DockPanel DockPnl)
        {
            // instead of repeating the code, call the routine using ConnectionTab if called by DockPanel
            if (DockPnl.ActiveDocument is ConnectionTab ct)
                return FindInterfaceControl(ct);
            return null;
        }

        public static InterfaceControl FindInterfaceControl(ConnectionTab tab)
        {
            if (tab.Controls.Count < 1) return null;
            // if the tab has more than one controls and the second is an InterfaceControl than it must be a connection through SSH tunnel
            // and the first Control is the SSH tunnel connection and thus the second control must be returned.
            if (tab.Controls.Count > 1)
            {
                if (tab.Controls[1] is InterfaceControl ic1)
                    return ic1;
            }
            if (tab.Controls[0] is InterfaceControl ic0)
                return ic0;

            return null;
        }
    }
}