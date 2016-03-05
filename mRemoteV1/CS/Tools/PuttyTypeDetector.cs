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
using mRemoteNG.Connection.Protocol;


namespace mRemoteNG.Tools
{
	public class PuttyTypeDetector
	{
		public static PuttyType GetPuttyType()
		{
			return GetPuttyType(PuttyBase.PuttyPath);
		}
			
		public static PuttyType GetPuttyType(string filename)
		{
			if (IsPuttyNg(filename))
			{
				return PuttyType.PuttyNg;
			}
			if (IsKitty(filename))
			{
				return PuttyType.Kitty;
			}
			if (IsXming(filename))
			{
				return PuttyType.Xming;
			}
				
			// Check this last
			if (IsPutty(filename))
			{
				return PuttyType.Putty;
			}
				
			return PuttyType.Unknown;
		}
			
		private static bool IsPutty(string filename)
		{
			bool result = false;
			try
			{
				result = System.Convert.ToBoolean(FileVersionInfo.GetVersionInfo(filename).InternalName.Contains("PuTTY"));
			}
			catch
			{
				result = false;
			}
			return result;
		}
			
		private static bool IsPuttyNg(string filename)
		{
			bool result = false;
			try
			{
				result = System.Convert.ToBoolean(FileVersionInfo.GetVersionInfo(filename).InternalName.Contains("PuTTYNG"));
			}
			catch
			{
				result = false;
			}
			return result;
		}
			
		private static bool IsKitty(string filename)
		{
			bool result = false;
			try
			{
				result = System.Convert.ToBoolean(FileVersionInfo.GetVersionInfo(filename).InternalName.Contains("PuTTY") && FileVersionInfo.GetVersionInfo(filename).Comments.Contains("KiTTY"));
			}
			catch
			{
				result = false;
			}
			return result;
		}
			
		private static bool IsXming(string filename)
		{
			bool result = false;
			try
			{
				result = System.Convert.ToBoolean(FileVersionInfo.GetVersionInfo(filename).InternalName.Contains("PuTTY") && FileVersionInfo.GetVersionInfo(filename).ProductVersion.Contains("Xming"));
			}
			catch
			{
				result = false;
			}
			return result;
		}
			
		public enum PuttyType
		{
			Unknown = 0,
			Putty,
			PuttyNg,
			Kitty,
			Xming
		}
	}
}
