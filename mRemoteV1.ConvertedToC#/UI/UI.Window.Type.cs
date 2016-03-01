using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
namespace mRemoteNG.UI
{
	namespace Window
	{
		public enum Type
		{
			Tree = 0,
			Connection = 1,
			Config = 2,
			Sessions = 3,
			ErrorsAndInfos = 4,
			ScreenshotManager = 5,
			Options = 6,
			About = 8,
			Update = 9,
			SSHTransfer = 10,
			ActiveDirectoryImport = 11,
			Help = 12,
			ExternalApps = 13,
			PortScan = 14,
			UltraVNCSC = 16,
			ComponentsCheck = 17,
			Announcement = 18
		}
	}
}
