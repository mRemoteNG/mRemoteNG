using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;

namespace mRemoteNG.Forms.OptionsPages
{
	public class OptionsPage : UserControl
	{

		#region "Public Properties"
		[Browsable(false)]
		public virtual string PageName { get; set; }

		public virtual Icon PageIcon { get; set; }
		#endregion

		#region "Public Methods"

		public virtual void ApplyLanguage()
		{
		}


		public virtual void LoadSettings()
		{
		}


		public virtual void SaveSettings()
		{
		}


		public virtual void RevertSettings()
		{
		}
		#endregion
	}
}
