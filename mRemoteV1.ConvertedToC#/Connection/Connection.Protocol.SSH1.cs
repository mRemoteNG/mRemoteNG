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
		public class SSH1 : Connection.Protocol.PuttyBase
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
}
