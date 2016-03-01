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
		public class Serial : Connection.Protocol.PuttyBase
		{

			public Serial()
			{
				this.PuttyProtocol = Putty_Protocol.serial;
			}

			public enum Defaults
			{
				Port = 9600
			}
		}
	}
}
