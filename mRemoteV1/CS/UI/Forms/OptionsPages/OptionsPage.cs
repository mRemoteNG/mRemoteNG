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
using System.ComponentModel;
using mRemoteNG.App;


namespace mRemoteNG.Forms.OptionsPages
{
	public class OptionsPage : UserControl
	{
		public OptionsPage()
		{
			//InitializeComponent();
		}
			
        #region Public Properties
		[Browsable(false)]public virtual string PageName {get; set;}
			
		public virtual Icon PageIcon {get; set;}
        #endregion
			
        #region Public Methods
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
