using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.My;

namespace mRemoteNG.Tools
{
	public class PuttyProcessController : ProcessController
	{
		public bool Start(CommandLineArguments arguments = null)
		{
			string filename = null;
			if (Settings.UseCustomPuttyPath) {
				filename = Settings.CustomPuttyPath;
			} else {
				filename = mRemoteNG.App.Info.General.PuttyPath;
			}
			return Start(filename, arguments);
		}
	}
}

