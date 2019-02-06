using mRemoteNG.App;
using mRemoteNG.UI.Tabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mRemoteNG.Connection.Protocol.RDP
{
    class RdpClientWrap : AxMSTSCLib.AxMsRdpClient8NotSafeForScripting
    {
        public RdpClientWrap()
               : base()
        {
            GotFocus += RdpClientWrap_GotFocus;
        }

        private void RdpClientWrap_GotFocus(object sender, EventArgs e)
        {
             ((ConnectionTab)Parent.Parent).Focus();
        }
    }
}
