using System;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Security;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Properties;
using mRemoteNG.UI.Forms;
using MySql.Data.Types;
using mRemoteNG.Resources.Language;
using static System.String;
using System.Windows;
using System.Runtime.Versioning;

namespace mRemoteNG.Tools
{
    [SupportedOSPlatform("windows")]
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
                                                    "GetIconFromFile failed (Tools.Misc) - using default icon" +
                                                    Environment.NewLine + AEx.Message,
                                                    true);
                return Properties.Resources.mRemoteNG_Icon;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                                                    "GetIconFromFile failed (Tools.Misc)" + Environment.NewLine +
                                                    ex.Message, true);
                return null;
            }
        }

        public static Optional<SecureString> PasswordDialog(string passwordName = null, bool verify = true)
        {
            //var splash = FrmSplashScreenNew.GetInstance();
            //TODO: something not right there 
            //if (PresentationSource.FromVisual(splash))
            //    splash.Close();

            var passwordForm = new FrmPassword(passwordName, verify);
            return passwordForm.GetKey();
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
			switch (Properties.OptionsDBsPage.Default.SQLServerType)
			{
				case "mysql":
					return Dt.ToString("yyyy/MM/dd HH:mm:ss");
				case "mssql":
				default:
					return Dt.ToString("yyyyMMdd HH:mm:ss");
			}
		}

		public static Type DBTimeStampType()
		{
			switch (Properties.OptionsDBsPage.Default.SQLServerType)
			{
				case "mysql":
					return typeof(MySqlDateTime);
				case "mssql":
				default:
					return typeof(SqlDateTime);
			}
		}

		public static object DBTimeStampNow()
		{
			switch (Properties.OptionsDBsPage.Default.SQLServerType)
			{
				case "mysql":
					return new MySqlDateTime(DateTime.Now);
				case "mssql":
				default:
					return DateTime.Now;
			}
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


        public static Image TakeScreenshot(UI.Tabs.ConnectionTab sender)
        {
            try
            {
                if (sender != null)
                {
                    var bmp = new Bitmap(sender.Width, sender.Height, PixelFormat.Format32bppRgb);
                    Graphics g = Graphics.FromImage(bmp);
                    g.CopyFromScreen(sender.PointToScreen(System.Drawing.Point.Empty), System.Drawing.Point.Empty, bmp.Size, CopyPixelOperation.SourceCopy);
                    return bmp;
                }
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

            public override object ConvertTo(ITypeDescriptorContext context,
                                             CultureInfo culture,
                                             object value,
                                             Type destType)
            {
                if (value == null) return null;
                var fi = _enumType.GetField(Enum.GetName(_enumType, value));
                var dna = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));

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
                    var dna = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));

                    if (dna != null && (string)value == dna.Description)
                    {
                        return Enum.Parse(_enumType, fi.Name);
                    }
                }

                return value != null ? Enum.Parse(_enumType, (string)value) : null;
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
                if (string.Equals(value.ToString(), Language.Yes, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }

                if (string.Equals(value.ToString(), Language.No, StringComparison.CurrentCultureIgnoreCase))
                {
                    return false;
                }

                throw new Exception("Values must be \"Yes\" or \"No\"");
            }

            public override object ConvertTo(ITypeDescriptorContext context,
                                             CultureInfo culture,
                                             object value,
                                             Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    return Convert.ToBoolean(value) ? Language.Yes : Language.No;
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