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
	public class Rlogin : PuttyBase
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
