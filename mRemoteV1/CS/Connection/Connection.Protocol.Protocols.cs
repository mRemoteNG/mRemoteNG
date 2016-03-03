// VBConversions Note: VB project level imports
using System.Collections.Generic;
using System;
using AxWFICALib;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using AxMSTSCLib;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
using mRemoteNG.Tools;
// End of VB project level imports

//using mRemoteNG.Tools.LocalizedAttributes;


namespace mRemoteNG.Connection.Protocol
{
	public class Converter
	{
		public static string ProtocolToString(Protocols protocol)
		{
			return protocol.ToString();
		}
				
		public static Protocols StringToProtocol(string protocol)
		{
			try
			{
				return (Protocols)Enum.Parse(typeof(Protocols), protocol, true);
			}
			catch (Exception)
			{
				return Protocols.RDP;
			}
		}
	}
			
	public enum Protocols
	{
        [LocalizedAttributes.LocalizedDescription("strRDP")]
        RDP = 0,
        [LocalizedAttributes.LocalizedDescription("strVnc")]
        VNC = 1,
        [LocalizedAttributes.LocalizedDescription("strSsh1")]
        SSH1 = 2,
        [LocalizedAttributes.LocalizedDescription("strSsh2")]
        SSH2 = 3,
        [LocalizedAttributes.LocalizedDescription("strTelnet")]
        Telnet = 4,
        [LocalizedAttributes.LocalizedDescription("strRlogin")]
        Rlogin = 5,
        [LocalizedAttributes.LocalizedDescription("strRAW")]
        RAW = 6,
        [LocalizedAttributes.LocalizedDescription("strHttp")]
        HTTP = 7,
        [LocalizedAttributes.LocalizedDescription("strHttps")]
        HTTPS = 8,
        [LocalizedAttributes.LocalizedDescription("strICA")]
        ICA = 9,
        [LocalizedAttributes.LocalizedDescription("strExtApp")]
        IntApp = 20
	}
}
