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

namespace mRemoteNG.Credential
{
	public class Info
	{
		#region "1 Display"
		private string _Name;
		[LocalizedCategory("strCategoryDisplay", 1), Browsable(true), LocalizedDisplayName("strPropertyNameName"), LocalizedDescription("strPropertyDescriptionName")]
		public string Name {
			get { return _Name; }
			set { _Name = value; }
		}

		private string _Description;
		[LocalizedCategory("strCategoryDisplay", 1), Browsable(true), LocalizedDisplayName("strPropertyNameDescription"), LocalizedDescription("strPropertyDescriptionDescription")]
		public string Description {
			get { return _Description; }
			set { _Description = value; }
		}
		#endregion
		#region "2 Credentials"
		private string _Username;
		[LocalizedCategory("strCategoryCredentials", 2), Browsable(true), LocalizedDisplayName("strPropertyNameUsername"), LocalizedDescription("strPropertyDescriptionUsername")]
		public string Username {
			get { return _Username; }
			set { _Username = value; }
		}

		private string _Password;
		[LocalizedCategory("strCategoryCredentials", 2), Browsable(true), LocalizedDisplayName("strPropertyNamePassword"), LocalizedDescription("strPropertyDescriptionPassword"), PasswordPropertyText(true)]
		public string Password {
			get { return _Password; }
			set { _Password = value; }
		}

		private string _Domain;
		[LocalizedCategory("strCategoryCredentials", 2), Browsable(true), LocalizedDisplayName("strPropertyNameDomain"), LocalizedDescription("strPropertyDescriptionDomain")]
		public string Domain {
			get { return _Domain; }
			set { _Domain = value; }
		}
		#endregion
	}
}
