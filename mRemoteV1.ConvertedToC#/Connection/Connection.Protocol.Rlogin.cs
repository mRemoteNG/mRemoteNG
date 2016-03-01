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
		public class Rlogin : Connection.Protocol.PuttyBase
		{

			public Rlogin()
			{
				this.PuttyProtocol = Putty_Protocol.rlogin;
			}

			public enum Defaults
			{
				Port = 513
			}
		}
	}
}
