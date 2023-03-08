using System;
using System.Drawing;
using System.Windows.Forms;
using AxMSTSCLib;
using mRemoteNG.App;
using mRemoteNG.Messages;
using MSTSCLib;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.Connection.Protocol.RDP
{
    /* RDP v8 requires Windows 7 with:
		* https://support.microsoft.com/en-us/kb/2592687 
		* OR
		* https://support.microsoft.com/en-us/kb/2923545
		* 
		* Windows 8+ support RDP v8 out of the box.
		*/
    public class RdpProtocol8 : RdpProtocol7
    {
        private MsRdpClient8NotSafeForScripting RdpClient8 => (MsRdpClient8NotSafeForScripting)((AxHost)Control).GetOcx();
        private Size _controlBeginningSize;

        protected override RdpVersion RdpProtocolVersion => RdpVersion.Rdc8;

        public override bool SmartSize
        {
            get => base.SmartSize;
            protected set
            {
                base.SmartSize = value;
                DoResizeClient();
            }
        }

        public override bool Fullscreen
        {
            get => base.Fullscreen;
            protected set
            {
                base.Fullscreen = value;
                DoResizeClient();
            }
        }

        public override void ResizeBegin(object sender, EventArgs e)
        {
            _controlBeginningSize = Control.Size;
        }

        public override void Resize(object sender, EventArgs e)
        {
            if (DoResizeControl() && _controlBeginningSize.IsEmpty)
            {
                DoResizeClient();
            }
            base.Resize(sender, e);
        }

        public override void ResizeEnd(object sender, EventArgs e)
        {
            DoResizeControl();
            if (!(Control.Size == _controlBeginningSize))
            {
                DoResizeClient();
            }
            _controlBeginningSize = Size.Empty;
        }

        protected override AxHost CreateActiveXRdpClientControl()
        {
            return new AxMsRdpClient8NotSafeForScripting();
        }

        private void DoResizeClient()
        {
            if (!loginComplete)
                return;

            if (!InterfaceControl.Info.AutomaticResize)
                return;

            if (!(InterfaceControl.Info.Resolution == RDPResolutions.FitToWindow ||
                  InterfaceControl.Info.Resolution == RDPResolutions.Fullscreen))
                return;

            if (SmartSize)
                return;

            Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg,
                $"Resizing RDP connection to host '{connectionInfo.Hostname}'");

            try
            {
                var size = Fullscreen
                    ? Screen.FromControl(Control).Bounds.Size
                    : Control.Size;
                UpdateSessionDisplaySettings((uint)size.Width, (uint)size.Height);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(
                    string.Format(Language.ChangeConnectionResolutionError,
                        connectionInfo.Hostname),
                    ex, MessageClass.WarningMsg, false);
            }
        }

        private bool DoResizeControl()
        {
            Control.Location = InterfaceControl.Location;
            // kmscode - this doesn't look right to me. But I'm not aware of any functionality issues with this currently...
            if (!(Control.Size == InterfaceControl.Size) && !(InterfaceControl.Size == Size.Empty))
            {
                Control.Size = InterfaceControl.Size;
                return true;
            }
            else
            {
                return false;
            }
        }

        protected virtual void UpdateSessionDisplaySettings(uint width, uint height)
        {
            RdpClient8.Reconnect(width, height);
        }
    }
}
