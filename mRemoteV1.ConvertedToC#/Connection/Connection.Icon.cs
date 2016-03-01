using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using mRemoteNG.App.Runtime;

namespace mRemoteNG.Connection
{
	public class Icon : StringConverter
	{


		public static string[] Icons = new string[];
		public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context)
		{
			return new StandardValuesCollection(Icons);
		}

		public override bool GetStandardValuesExclusive(System.ComponentModel.ITypeDescriptorContext context)
		{
			return true;
		}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public static Drawing.Icon FromString(string IconName)
		{
			try {
				string IconPath = mRemoteNG.My.MyProject.Application.Info.DirectoryPath + "\\Icons\\" + IconName + ".ico";

				if (System.IO.File.Exists(IconPath)) {
					System.Drawing.Icon nI = new System.Drawing.Icon(IconPath);

					return nI;
				}
			} catch (Exception ex) {
				mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "Couldn't get Icon from String" + Constants.vbNewLine + ex.Message);
			}

			return null;
		}
	}
}
