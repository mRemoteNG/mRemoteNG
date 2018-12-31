using mRemoteNG.App;
using mRemoteNG.Connection.Protocol;
using System;
using System.Drawing;
using System.Windows.Forms;
using mRemoteNG.UI.Tabs;
using WeifenLuo.WinFormsUI.Docking;


namespace mRemoteNG.Connection
{
	public sealed partial class InterfaceControl
    {
        public ProtocolBase Protocol { get; set; }
	    public ConnectionInfo Info { get; set; }


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
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn\'t create new InterfaceControl" + Environment.NewLine + ex.Message);
			}
		}

        public static InterfaceControl FindInterfaceControl(DockPanel DockPnl)
        {
            if (!(DockPnl.ActiveDocument is ConnectionTab ct)) return null;
            if (ct.Controls[0] is InterfaceControl ic)
                return ic;

            return null;
        }
	}
}