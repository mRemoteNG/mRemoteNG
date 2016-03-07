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
using mRemoteNG.Tools;


namespace mRemoteNG.Root.PuttySessions
{
	public class Info : Root.Info
	{
		public Info() : base(RootType.PuttySessions)
		{
			// VBConversions Note: Non-static class variable initialization is below.  Class variables cannot be initially assigned non-static values in C#.
			_name = Language.strPuttySavedSessionsRootName;
			_panel = My.Language.strGeneral;
		}
				
		private string _name; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedDefaultValue("strPuttySavedSessionsRootName")]
        public override string Name
		{
			get
			{
				return _name;
			}
			set
			{
				if (_name == value)
				{
					return ;
				}
				_name = value;
				if (TreeNode != null)
				{
					TreeNode.Text = value;
				}
                My.Settings.Default.PuttySavedSessionsName = value;
			}
		}
				
		private string _panel; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNamePanel"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionPanel")]
        public string Panel
		{
			get
			{
				return _panel;
			}
			set
			{
				if (_panel == value)
				{
					return ;
				}
				_panel = value;
				My.Settings.Default.PuttySavedSessionsPanel = value;
			}
		}
	}
}