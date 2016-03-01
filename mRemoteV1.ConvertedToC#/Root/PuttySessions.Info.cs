using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.My;
using mRemoteNG.Tools.LocalizedAttributes;

namespace mRemoteNG.Root
{
	namespace PuttySessions
	{
		public class Info : Root.Info
		{

			public Info() : base(RootType.PuttySessions)
			{
			}

			private string _name = Language.strPuttySavedSessionsRootName;
			[LocalizedDefaultValue("strPuttySavedSessionsRootName")]
			public override string Name {
				get { return _name; }
				set {
					if (_name == value)
						return;
					_name = value;
					if (TreeNode != null) {
						TreeNode.Text = value;
					}
					Settings.PuttySavedSessionsName = value;
				}
			}

			private string _panel = mRemoteNG.My.Language.strGeneral;
			[LocalizedCategory("strCategoryDisplay", 1), LocalizedDisplayName("strPropertyNamePanel"), LocalizedDescription("strPropertyDescriptionPanel")]
			public string Panel {
				get { return _panel; }
				set {
					if (_panel == value)
						return;
					_panel = value;
					Settings.PuttySavedSessionsPanel = value;
				}
			}
		}
	}
}

