using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using mRemoteNG.Tools.LocalizedAttributes;

namespace mRemoteNG.Root
{
	[DefaultProperty("Name")]
	public class Info
	{
		#region "Constructors"
		public Info(RootType rootType)
		{
			Type = rootType;
		}
		#endregion

		#region "Public Properties"
		private string _name = mRemoteNG.My.Language.strConnections;
		[LocalizedCategory("strCategoryDisplay", 1), Browsable(true), LocalizedDefaultValue("strConnections"), LocalizedDisplayName("strPropertyNameName"), LocalizedDescription("strPropertyDescriptionName")]
		public virtual string Name {
			get { return _name; }
			set {
				if (_name == value)
					return;
				_name = value;
				if (TreeNode != null) {
					TreeNode.Name = value;
					TreeNode.Text = value;
				}
			}
		}

		[LocalizedCategory("strCategoryDisplay", 1), Browsable(true), LocalizedDisplayName("strPasswordProtect"), TypeConverter(typeof(Tools.Misc.YesNoTypeConverter))]
		public bool Password { get; set; }

		[Browsable(false)]
		public string PasswordString { get; set; }

		[Browsable(false)]
		public RootType Type { get; set; }

		[Browsable(false)]
		public TreeNode TreeNode { get; set; }
		#endregion

		#region "Public Enumerations"
		public enum RootType
		{
			Connection,
			Credential,
			PuttySessions
		}
		#endregion
	}
}

