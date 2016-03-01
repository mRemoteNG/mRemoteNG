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
		public class HTTP : Connection.Protocol.HTTPBase
		{

			public HTTP(RenderingEngine RenderingEngine) : base(RenderingEngine)
			{
			}

			public override void NewExtended()
			{
				base.NewExtended();

				httpOrS = "http";
				defaultPort = Defaults.Port;
			}

			public enum Defaults
			{
				Port = 80
			}
		}
	}
}
