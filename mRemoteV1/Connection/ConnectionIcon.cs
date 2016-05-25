using System;
using Microsoft.VisualBasic;
using System.ComponentModel;
using mRemoteNG.App;
using mRemoteNG.App.Info;


namespace mRemoteNG.Connection
{
	public class ConnectionIcon : StringConverter
	{
		public static string[] Icons = new string[] {};
		
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
		
		public static System.Drawing.Icon FromString(string IconName)
		{
			try
			{
				string IconPath = GeneralAppInfo.HomePath + "\\Icons\\" + IconName +".ico";
				
				if (System.IO.File.Exists(IconPath))
				{
					System.Drawing.Icon nI = new System.Drawing.Icon(IconPath);
					return nI;
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn\'t get Icon from String" + Environment.NewLine + ex.Message);
			}
			return null;
		}
	}
}
