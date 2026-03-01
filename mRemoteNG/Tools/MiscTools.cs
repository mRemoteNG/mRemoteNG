using System;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Security;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Messages;
using mRemoteNG.UI.Forms;
using MySql.Data.Types;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.Tools
{
    [SupportedOSPlatform("windows")]
    public static class MiscTools
    {
        public static Icon? GetIconFromFile(string FileName)
        {
            try
            {
                return File.Exists(FileName) ? Icon.ExtractAssociatedIcon(FileName) : null;
            }
            catch (ArgumentException AEx)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, "GetIconFromFile failed (Tools.Misc) - using default icon" + Environment.NewLine + AEx.Message, true);
                return Properties.Resources.mRemoteNG_Icon;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, "GetIconFromFile failed (Tools.Misc)" + Environment.NewLine + ex.Message, true);
                return null;
            }
        }

        public static Optional<SecureString> PasswordDialog(string? passwordName = null, bool verify = true)
        {
            //var splash = FrmSplashScreenNew.GetInstance();
            //TODO: something not right there 
            //if (PresentationSource.FromVisual(splash))
            //    splash.Close();

            passwordName ??= string.Empty; // Ensure passwordName is not null
            FrmPassword passwordForm = new(passwordName, verify);
            return passwordForm.GetKey();
        }

        public static string LeadingZero(string Number)
        {
            if (Convert.ToInt32(Number, CultureInfo.InvariantCulture) < 10)
            {
                return "0" + Number;
            }

            return Number;
        }

        public static bool GetBooleanValue(object dataObject)
        {
            Type type = dataObject.GetType();

            if (type == typeof(bool))
            {
                return (bool)dataObject;
            }
            if (type == typeof(string))
            {
                return string.Equals((string)dataObject, "1", StringComparison.Ordinal);
            }
            if (type == typeof(sbyte))
            {
                return (sbyte)dataObject == 1;
            }

            Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, $"Conversion of object to boolean failed because the type, {type}, is not handled.");
            return false;
        }

        public static string DBDate(DateTime Dt)
		{
			switch (Properties.OptionsDBsPage.Default.SQLServerType)
			{
				case "mysql":
					return Dt.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
				case "mssql":
				default:
					return Dt.ToString("yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture);
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
					return new MySqlDateTime(DateTime.Now.ToUniversalTime());
				case "mssql":
				default:
					return DateTime.Now.ToUniversalTime();
			}
		}

        public static string PrepareValueForDB(string Text)
        {
            return Text.Replace("\'", "\'\'", StringComparison.Ordinal);
        }

        public static string GetExceptionMessageRecursive(Exception ex)
        {
            return GetExceptionMessageRecursive(ex, Environment.NewLine);
        }

        private static string GetExceptionMessageRecursive(Exception ex, string separator)
        {
            string message = ex.Message;
            if (ex.InnerException == null) return message;
            string innerMessage = GetExceptionMessageRecursive(ex.InnerException, separator);
            message = String.Join(separator, message, innerMessage);
            return message;
        }


        public static Image? TakeScreenshot(UI.Tabs.ConnectionTab sender)
        {
            try
            {
                if (sender != null)
                {
                    Bitmap bmp = new(sender.Width, sender.Height, PixelFormat.Format32bppRgb);
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

        public class EnumTypeConverter(Type type) : EnumConverter(type)
        {
            private readonly Type _enumType = type;

            public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
            {
                return destinationType == typeof(string);
            }

            public override object ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type? destinationType)
            {
                if (value == null) return string.Empty;

                string? enumName = Enum.GetName(_enumType, value);
                if (enumName == null)
                {
                    throw new ArgumentException("Invalid enum value provided.", nameof(value));
                }

                System.Reflection.FieldInfo? fi = _enumType.GetField(enumName);
                if (fi == null)
                {
                    throw new ArgumentException("FieldInfo could not be retrieved for the provided enum value.", nameof(value));
                }

                DescriptionAttribute? dna = (DescriptionAttribute?)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
                return dna?.Description ?? value.ToString() ?? string.Empty;
            }

            public override bool CanConvertFrom(ITypeDescriptorContext? context, Type? sourceType)
            {
                return sourceType == typeof(string);
            }

            public override object ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value)
            {
                if (value is string stringValue)
                {
                    foreach (System.Reflection.FieldInfo fi in _enumType.GetFields())
                    {
                        DescriptionAttribute? dna = (DescriptionAttribute?)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));

                        if (dna != null && string.Equals(stringValue, dna.Description, StringComparison.Ordinal))
                        {
                            return Enum.Parse(_enumType, fi.Name);
                        }
                    }

                    return Enum.Parse(_enumType, stringValue);
                }

                throw new ArgumentNullException(nameof(value), "Value cannot be null.");
            }
        }

        public class YesNoTypeConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
            {
                return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
            }
            public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
            {
                return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
            }

            public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value)
            {
                if (value is not string stringValue)
                {
                    // Ensure 'value' is not null before passing it to the base method
                    return value != null
                        ? base.ConvertFrom(context, culture, value)
                        : throw new ArgumentNullException(nameof(value), "Value cannot be null.");
                }

                if (string.Equals(stringValue, Language.Yes, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                if (string.Equals(stringValue, Language.No, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                throw new FormatException("Values must be \"Yes\" or \"No\"");
            }

            public override object ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    return Convert.ToBoolean(value, CultureInfo.InvariantCulture) ? Language.Yes : Language.No;
                }

                return base.ConvertTo(context, culture, value, destinationType) ?? throw new InvalidOperationException("Base conversion returned null.");
            }

            public override bool GetStandardValuesSupported(ITypeDescriptorContext? context)
            {
                return true;
            }

            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext? context)
            {
                bool[] bools = { true, false };

                StandardValuesCollection svc = new(bools);

                return svc;
            }
        }

        public class YesNoAutoTypeConverter : YesNoTypeConverter
        {
            private const string AutoText = "Auto";

            public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value)
            {
                if (value is AutoSelection autoSelection)
                {
                    if (autoSelection == AutoSelection.Yes)
                        return true;
                    if (autoSelection == AutoSelection.No)
                        return false;
                    return ConvertFromAutoSelection(context);
                }

                if (value is string stringValue &&
                    string.Equals(stringValue, AutoText, StringComparison.OrdinalIgnoreCase))
                {
                    return ConvertFromAutoSelection(context);
                }

                return base.ConvertFrom(context, culture, value);
            }

            public override object ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
            {
                if (destinationType == typeof(string) && value is AutoSelection autoSelection)
                {
                    if (autoSelection == AutoSelection.Yes)
                        return Language.Yes;
                    if (autoSelection == AutoSelection.No)
                        return Language.No;
                    return AutoText;
                }

                return base.ConvertTo(context, culture, value, destinationType);
            }

            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext? context)
            {
                AutoSelection[] values = { AutoSelection.Yes, AutoSelection.No, AutoSelection.Auto };
                return new StandardValuesCollection(values);
            }

            private static bool ConvertFromAutoSelection(ITypeDescriptorContext? context)
            {
                ConnectionInfoInheritance? inheritance = GetInheritanceFromContext(context);
                if (inheritance == null)
                    return false;

                inheritance.RequestAutomaticEverythingInheritanceEvaluation();
                return !inheritance.EverythingInherited;
            }

            private static ConnectionInfoInheritance? GetInheritanceFromContext(ITypeDescriptorContext? context)
            {
                if (context?.Instance is ConnectionInfoInheritance inheritance)
                    return inheritance;

                if (context?.Instance is object[] instances)
                {
                    foreach (object instance in instances)
                    {
                        if (instance is ConnectionInfoInheritance inheritanceInstance)
                            return inheritanceInstance;
                    }
                }

                return null;
            }

            private enum AutoSelection
            {
                Yes,
                No,
                Auto
            }
        }

        public class TabColorConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
            {
                return sourceType == typeof(string) || sourceType == typeof(Color) || base.CanConvertFrom(context, sourceType);
            }

            public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
            {
                return destinationType == typeof(string) || destinationType == typeof(Color) || base.CanConvertTo(context, destinationType);
            }

            public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value)
            {
                if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
                {
                    return string.Empty;
                }

                if (value is string stringValue)
                {
                    return stringValue;
                }

                if (value is Color colorValue)
                {
                    // Convert Color to string representation
                    // Use named color if it's a known color, otherwise use hex format
                    if (colorValue.IsNamedColor)
                    {
                        return colorValue.Name;
                    }
                    else
                    {
                        // Return hex format without alpha if fully opaque, otherwise include alpha
                        if (colorValue.A == 255)
                        {
                            return $"#{colorValue.R:X2}{colorValue.G:X2}{colorValue.B:X2}";
                        }
                        else
                        {
                            return $"#{colorValue.A:X2}{colorValue.R:X2}{colorValue.G:X2}{colorValue.B:X2}";
                        }
                    }
                }

                return base.ConvertFrom(context, culture, value);
            }

            public override object ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
                    {
                        return string.Empty;
                    }
                    return value.ToString() ?? string.Empty;
                }

                if (destinationType == typeof(Color))
                {
                    if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
                    {
                        return Color.Empty;
                    }

                    if (value is string stringValue)
                    {
                        try
                        {
                            ColorConverter converter = new ColorConverter();
                            return converter.ConvertFromString(stringValue) ?? Color.Empty;
                        }
                        catch
                        {
                            return Color.Empty;
                        }
                    }
                }

                return base.ConvertTo(context, culture, value, destinationType) ?? throw new InvalidOperationException("Base conversion returned null.");
            }

            public override bool GetStandardValuesSupported(ITypeDescriptorContext? context)
            {
                return true;
            }

            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext? context)
            {
                // Provide a list of common colors for the dropdown
                Color[] colors =
                [
                    Color.Red,
                    Color.Orange,
                    Color.Yellow,
                    Color.Green,
                    Color.Blue,
                    Color.Purple,
                    Color.Pink,
                    Color.Brown,
                    Color.Black,
                    Color.White,
                    Color.Gray,
                    Color.LightGray,
                    Color.DarkGray,
                    Color.Cyan,
                    Color.Magenta,
                    Color.Lime,
                    Color.Navy,
                    Color.Teal,
                    Color.Maroon,
                    Color.Olive
                ];

                return new StandardValuesCollection(colors);
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext? context)
            {
                // Return false to allow custom values (hex codes or other color names)
                return false;
            }
        }
    }
}