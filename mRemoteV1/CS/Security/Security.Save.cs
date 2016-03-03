// VBConversions Note: VB project level imports
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
// End of VB project level imports


namespace mRemoteNG.Security
{
	public class Save
		{
			public Save(bool DisableEverything = false)
			{
				if (DisableEverything == false)
				{
					_Username = true;
					_Password = true;
					_Domain = true;
					_Inheritance = true;
				}
			}
			
			private bool _Username;
public bool Username
			{
				get
				{
					return _Username;
				}
				set
				{
					_Username = value;
				}
			}
			
			private bool _Password;
public bool Password
			{
				get
				{
					return _Password;
				}
				set
				{
					_Password = value;
				}
			}
			
			private bool _Domain;
public bool Domain
			{
				get
				{
					return _Domain;
				}
				set
				{
					_Domain = value;
				}
			}
			
			private bool _Inheritance;
public bool Inheritance
			{
				get
				{
					return _Inheritance;
				}
				set
				{
					_Inheritance = value;
				}
			}
		}
}
