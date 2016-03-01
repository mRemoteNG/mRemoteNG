using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.Tools.LocalizedAttributes;

namespace mRemoteNG.Connection
{
	namespace Protocol
	{
		public class Converter
		{
			public static string ProtocolToString(Protocols protocol)
			{
				return protocol.ToString();
			}

			public static Protocols StringToProtocol(string protocol)
			{
				try {
					return Enum.Parse(typeof(Protocols), protocol, true);
				} catch (Exception ex) {
					return Protocols.RDP;
				}
			}
		}

		public enum Protocols
		{
			[LocalizedDescription("strRDP")]
			RDP = 0,
			[LocalizedDescription("strVnc")]
			VNC = 1,
			[LocalizedDescription("strSsh1")]
			SSH1 = 2,
			[LocalizedDescription("strSsh2")]
			SSH2 = 3,
			[LocalizedDescription("strTelnet")]
			Telnet = 4,
			[LocalizedDescription("strRlogin")]
			Rlogin = 5,
			[LocalizedDescription("strRAW")]
			RAW = 6,
			[LocalizedDescription("strHttp")]
			HTTP = 7,
			[LocalizedDescription("strHttps")]
			HTTPS = 8,
			[LocalizedDescription("strICA")]
			ICA = 9,
			[LocalizedDescription("strExtApp")]
			IntApp = 20
		}
	}
}
