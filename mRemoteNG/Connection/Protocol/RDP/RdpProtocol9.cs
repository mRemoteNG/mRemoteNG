using System;
using System.Runtime.Versioning;
using System.Windows.Forms;
using AxMSTSCLib;
using mRemoteNG.App;
using mRemoteNG.Messages;
using MSTSCLib;

namespace mRemoteNG.Connection.Protocol.RDP
{
    [SupportedOSPlatform("windows")]
    public class RdpProtocol9 : RdpProtocol8
    {
        private MsRdpClient9NotSafeForScripting? RdpClient9 => (Control as AxHost)?.GetOcx() as MsRdpClient9NotSafeForScripting;

        protected override RdpVersion RdpProtocolVersion => RDP.RdpVersion.Rdc9;

        // Constructor not needed - resize handlers are wired by ProtocolBase via ConnectionTab events.

        public override bool Initialize()
        {
            if (!base.Initialize())
                return false;

            return PostInitialize();
        }

        public override async System.Threading.Tasks.Task<bool> InitializeAsync()
        {
            if (!await base.InitializeAsync())
                return false;

            return PostInitialize();
        }

        private bool PostInitialize()
        {
            if (RdpVersion < Versions.RDC81) return false; // minimum dll version checked, loaded MSTSCLIB dll version is not capable

            return true;
        }

        protected override AxHost CreateActiveXRdpClientControl()
        {
            return new AxMsRdpClient9NotSafeForScripting();
        }

        protected override void UpdateSessionDisplaySettings(uint width, uint height)
        {
            try
            {
                if (RdpClient9 != null)
                {
                    RdpClient9.UpdateSessionDisplaySettings(width, height, width, height, Orientation, DesktopScaleFactor, DeviceScaleFactor);
                }
                else
                {
                    base.UpdateSessionDisplaySettings(width, height);
                }
            }
            catch (Exception ex)
            {
                // target OS does not support newer method, fallback to an older method
                Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg,
                    $"RdpProtocol9: UpdateSessionDisplaySettings failed (falling back to Reconnect): {ex.Message}");
                base.UpdateSessionDisplaySettings(width, height);
            }
        }

    }
}