using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
namespace mRemoteNG.Connection
{
	namespace Protocol
	{
		public class HTTPS : Connection.Protocol.HTTPBase
		{

			public HTTPS(RenderingEngine RenderingEngine) : base(RenderingEngine)
			{
			}

			public override void NewExtended()
			{
				base.NewExtended();

				httpOrS = "https";
				defaultPort = Defaults.Port;
			}

			public enum Defaults
			{
				Port = 443
			}
		}
	}
}
