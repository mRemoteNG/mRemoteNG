using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Security;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;
using static System.String;

namespace mRemoteNG.Tools
{
    public static class MiscTools
	{
		public static Icon GetIconFromFile(string FileName)
		{
		    try
		    {
		        return File.Exists(FileName) == false ? null : Icon.ExtractAssociatedIcon(FileName);
		    }
		    catch (ArgumentException AEx)
		    {
		        Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
		            "GetIconFromFile failed (Tools.Misc) - using default icon" + Environment.NewLine + AEx.Message, true);
                return Resources.mRemote_Icon;

		    }
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, "GetIconFromFile failed (Tools.Misc)" + Environment.NewLine + ex.Message, true);
				return null;
			}
		}
		
		public static Optional<SecureString> PasswordDialog(string passwordName = null, bool verify = true)
		{
			var passwordForm = new PasswordForm(passwordName, verify);
		    return passwordForm.GetKey();
		}
		

		public static string CreateConstantID()
		{
			return Guid.NewGuid().ToString();
		}
		

		public static string LeadingZero(string Number)
		{
		    if (Convert.ToInt32(Number) < 10)
			{
				return "0" + Number;
			}
		    return Number;
		}


        public static string DBDate(DateTime Dt)
		{
		    var strDate = Dt.Year + LeadingZero(Convert.ToString(Dt.Month)) + LeadingZero(Convert.ToString(Dt.Day)) + " " + LeadingZero(Convert.ToString(Dt.Hour)) + ":" + LeadingZero(Convert.ToString(Dt.Minute)) + ":" + LeadingZero(Convert.ToString(Dt.Second));
		    return strDate;
		}


		public static string PrepareValueForDB(string Text)
		{
			return Text.Replace("\'", "\'\'");
		}

        public static string GetExceptionMessageRecursive(Exception ex)
        {
            return GetExceptionMessageRecursive(ex, Environment.NewLine);
        }
        private static string GetExceptionMessageRecursive(Exception ex, string separator)
		{
			var message = ex.Message;
		    if (ex.InnerException == null) return message;
		    var innerMessage = GetExceptionMessageRecursive(ex.InnerException, separator);
		    message = Join(separator, message, innerMessage);
		    return message;
		}
		

		public static Image TakeScreenshot(ConnectionWindow sender)
		{
			try
			{
				var LeftStart = sender.TabController.SelectedTab.PointToScreen(new Point(sender.TabController.SelectedTab.Left)).X; //Me.Left + Splitter.SplitterDistance + 11
				var TopStart = sender.TabController.SelectedTab.PointToScreen(new Point(sender.TabController.SelectedTab.Top)).Y; //Me.Top + Splitter.Top + TabController.Top + TabController.SelectedTab.Top * 2 - 3
				var LeftWidth = sender.TabController.SelectedTab.Width; //Me.Width - (Splitter.SplitterDistance + 16)
				var TopHeight = sender.TabController.SelectedTab.Height; //Me.Height - (Splitter.Top + TabController.Top + TabController.SelectedTab.Top * 2 + 2)
					
				var currentFormSize = new Size(LeftWidth, TopHeight);
				var ScreenToBitmap = new Bitmap(LeftWidth, TopHeight);
                var gGraphics = Graphics.FromImage(ScreenToBitmap);
					
				gGraphics.CopyFromScreen(new Point(LeftStart, TopStart), new Point(0, 0), currentFormSize);
					
				return ScreenToBitmap;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace("Taking Screenshot failed", ex);
			}
				
			return null;
		}
		
		public class EnumTypeConverter : EnumConverter
		{
			private readonly Type _enumType;
				
			public EnumTypeConverter(Type type) : base(type)
			{
				_enumType = type;
			}
				
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
			{
				return destType == typeof(string);
			}
				
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
			{
			    if (value == null) return null;
			    var fi = _enumType.GetField(Enum.GetName(_enumType, value));
			    var dna = (DescriptionAttribute) Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
					
			    return dna != null ? dna.Description : value.ToString();
			}
				
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type srcType)
			{
				return srcType == typeof(string);
			}
				
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
			    foreach (var fi in _enumType.GetFields())
				{
					var dna = (DescriptionAttribute) Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
						
					if (dna != null && (string) value == dna.Description)
					{
						return Enum.Parse(_enumType, fi.Name);
					}
				}

			    return value != null ? Enum.Parse(_enumType, (string) value) : null;
			}
		}
		
		public class YesNoTypeConverter : TypeConverter
		{
				
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
			    return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
			}
				
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
			    return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
			}
				
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
			    if (!(value is string)) return base.ConvertFrom(context, culture, value);
			    if (string.Equals(value.ToString(), Language.strYes, StringComparison.CurrentCultureIgnoreCase))
			    {
			        return true;
			    }
						
			    if (string.Equals(value.ToString(), Language.strNo, StringComparison.CurrentCultureIgnoreCase))
			    {
			        return false;
			    }
						
			    throw new Exception("Values must be \"Yes\" or \"No\"");
			}
				
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string))
				{
					return Convert.ToBoolean(value) ? Language.strYes : Language.strNo;
				}
					
				return base.ConvertTo(context, culture, value, destinationType);
			}
				
			public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
			{
				return true;
			}
				
			public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
			{
				bool[] bools = {true, false};
					
				var svc = new StandardValuesCollection(bools);
					
				return svc;
			}
		}
	}
}