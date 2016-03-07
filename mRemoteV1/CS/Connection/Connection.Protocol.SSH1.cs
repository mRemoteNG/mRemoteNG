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


namespace mRemoteNG.Connection.Protocol
{
	public class SSH1 : PuttyBase
	{
				
		public SSH1()
		{
			this.PuttyProtocol = Putty_Protocol.ssh;
			this.PuttySSHVersion = Putty_SSHVersion.ssh1;
		}
				
		public enum Defaults
		{
			Port = 22
		}
	}
}
