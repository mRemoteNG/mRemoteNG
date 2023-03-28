using System;
using System.Drawing;
using System.Windows.Forms;
using AxMSTSCLib;
using mRemoteNG.App;
using mRemoteNG.Messages;
using MSTSCLib;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.Connection.Protocol.RDP
{
    [SupportedOSPlatform("windows")]
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

        protected override RdpVersion RdpProtocolVersion => RDP.RdpVersion.Rdc8;
        protected FormWindowState LastWindowState = FormWindowState.Minimized;

        public override bool Initialize()
        {
            if (!base.Initialize())
                return false;

            if (RdpVersion < Versions.RDC81) return false; // minimum dll version checked, loaded MSTSCLIB dll version is not capable

            // https://learn.microsoft.com/en-us/windows/win32/termserv/imsrdpextendedsettings-property
            if (connectionInfo.UseRestrictedAdmin)
            {
                SetExtendedProperty("RestrictedLogon", true);
            }
            else if (connectionInfo.UseRCG)
            {
                SetExtendedProperty("DisableCredentialsDelegation", true);
                SetExtendedProperty("RedirectedAuthentication", true);
            }
            
            return true;
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

        protected override void Resize(object sender, EventArgs e)
        {
            if (LastWindowState == _frmMain.WindowState) return;
            LastWindowState = _frmMain.WindowState;
            if (_frmMain.WindowState == FormWindowState.Minimized) return; // don't resize when going to minimized since it seems to resize anyway, as seen when window is restored
            DoResizeControl();
            DoResizeClient();
        }

        protected override void ResizeEnd(object sender, EventArgs e)
        {
            DoResizeControl();
            DoResizeClient();
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
                    string.Format(Language.ChangeConnectionResolutionError, connectionInfo.Hostname),
                    ex, MessageClass.WarningMsg, false);
            }
        }

        private bool DoResizeControl()
        {
            Control.Location = InterfaceControl.Location;
            // kmscode - this doesn't look right to me. But I'm not aware of any functionality issues with this currently...
            if (Control.Size == InterfaceControl.Size || InterfaceControl.Size == Size.Empty) return false;
            Control.Size = InterfaceControl.Size;
            return true;
        }

        protected virtual void UpdateSessionDisplaySettings(uint width, uint height)
        {
            RdpClient8.Reconnect(width, height);
        }

    }
}
