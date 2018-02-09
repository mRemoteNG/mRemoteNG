using System;
using System.Drawing;
using System.Windows.Forms;
using AxMSTSCLib;
using mRemoteNG.App;
using MSTSCLib;

namespace mRemoteNG.Connection.Protocol.RDP
{
	/* RDP v8 requires Windows 7 with:
		* https://support.microsoft.com/en-us/kb/2592687 
		* OR
		* https://support.microsoft.com/en-us/kb/2923545
		* 
		* Windows 8+ support RDP v8 out of the box.
		*/
	public class RdpProtocol8 : RdpProtocol6
	{
		private MsRdpClient8NotSafeForScripting _rdpClient;
		private Size _controlBeginningSize;

		public override bool SmartSize
		{
			get { return base.SmartSize; }
			protected set
			{
				base.SmartSize = value;
				ReconnectForResize();
			}
		}

		public override bool Fullscreen
		{
			get => base.Fullscreen;
			protected set
			{
				base.Fullscreen = value;
				ReconnectForResize();
			}
		}

		public RdpProtocol8()
		{
			Control = new AxMsRdpClient8NotSafeForScripting();
		}

		public override bool Initialize()
		{
			base.Initialize();
			try
			{
				_rdpClient.AdvancedSettings8.AudioQualityMode = (uint)ConnectionInfo.SoundQuality;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace(Language.strRdpSetPropsFailed, ex);
				return false;
			}

			return true;
		}
		
		public override void ResizeBegin(object sender, EventArgs e)
		{
			_controlBeginningSize = Control.Size;
		}

		public override void Resize(object sender, EventArgs e)
		{
			if (DoResize() && _controlBeginningSize.IsEmpty)
			{
				ReconnectForResize();
			}
			base.Resize(sender, e);
		}

		public override void ResizeEnd(object sender, EventArgs e)
		{
			DoResize();
			if (!(Control.Size == _controlBeginningSize))
			{
				ReconnectForResize();
			}
			_controlBeginningSize = Size.Empty;
		}

		protected override object CreateRdpClientControl()
		{
			_rdpClient = (MsRdpClient8NotSafeForScripting)((AxMsRdpClient8NotSafeForScripting) Control).GetOcx();
			return _rdpClient;
		}

		private void ReconnectForResize()
		{
			if (!LoginComplete)
				return;

			if (!InterfaceControl.Info.AutomaticResize)
				return;

			if (!(InterfaceControl.Info.Resolution == RdpResolutions.FitToWindow | InterfaceControl.Info.Resolution == RdpResolutions.Fullscreen))
				return;

			if (SmartSize)
				return;

			var size = Fullscreen
				? Screen.FromControl(Control).Bounds.Size
				: Control.Size;
			_rdpClient.Reconnect((uint)size.Width, (uint)size.Height);
		}

		private bool DoResize()
		{
			Control.Location = InterfaceControl.Location;
			if (!(Control.Size == InterfaceControl.Size) && !(InterfaceControl.Size == Size.Empty)) // kmscode - this doesn't look right to me. But I'm not aware of any functionality issues with this currently...
			{
				Control.Size = InterfaceControl.Size;
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
