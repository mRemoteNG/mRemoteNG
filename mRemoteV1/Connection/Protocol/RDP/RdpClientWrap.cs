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
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == 0x0021)
                this.Parent.Parent.Focus();
            base.WndProc(ref m);
        } 
    }
}
