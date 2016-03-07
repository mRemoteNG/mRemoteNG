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
using mRemoteNG.My;


namespace mRemoteNG.Tools
{
	public class PuttyProcessController : ProcessController
	{
		public bool Start(CommandLineArguments arguments = null)
		{
			string filename = "";
			if (My.Settings.Default.UseCustomPuttyPath)
			{
				filename = My.Settings.Default.CustomPuttyPath;
			}
			else
			{
				filename = App.Info.General.PuttyPath;
			}
			return Start(filename, arguments);
		}
	}
}
