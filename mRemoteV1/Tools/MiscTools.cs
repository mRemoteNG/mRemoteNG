using Microsoft.VisualBasic;
using mRemoteNG.App;
using mRemoteNG.Forms;
using mRemoteNG.UI.Window;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using mRemoteNG.UI.Forms;


namespace mRemoteNG.Tools
{
	public class MiscTools
	{
		private struct SHFILEINFO
		{
			public IntPtr hIcon; // : icon
			public int iIcon; // : icondex
			public int dwAttributes; // : SFGAO_ flags
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
		}
			
		[DllImport("shell32.dll")]
        private static  extern IntPtr SHGetFileInfo(string pszPath, int dwFileAttributes, ref SHFILEINFO psfi, int cbFileInfo, int uFlags);
		private const int SHGFI_ICON = 0x100;
		private const int SHGFI_SMALLICON = 0x1;
		//Private Const SHGFI_LARGEICON = &H0    ' Large icon


		public static Icon GetIconFromFile(string FileName)
		{
			try
			{
				if (File.Exists(FileName) == false)
				{
					return null;
				}
					
				IntPtr hImgSmall; //The handle to the system image list.
				//Dim hImgLarge As IntPtr  'The handle to the system image list.
				SHFILEINFO shinfo = new SHFILEINFO();
				shinfo = new SHFILEINFO();
					
				shinfo.szDisplayName = new string('\0', 260);
				shinfo.szTypeName = new string('\0', 80);
					
				//Use this to get the small icon.
				hImgSmall = SHGetFileInfo(FileName, 0, ref shinfo, Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_SMALLICON);
					
				//Use this to get the large icon.
				//hImgLarge = SHGetFileInfo(fName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_LARGEICON);
					
				//The icon is returned in the hIcon member of the
				//shinfo struct.
				System.Drawing.Icon myIcon = default(System.Drawing.Icon);
				myIcon = System.Drawing.Icon.FromHandle(shinfo.hIcon);
					
				return myIcon;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "GetIconFromFile failed (Tools.Misc)" + Environment.NewLine + ex.Message, true);
				return null;
			}
		}
		

		
		

		public static string PasswordDialog(string passwordName = null, bool verify = true)
		{
			PasswordForm passwordForm = new PasswordForm(passwordName, verify);
				
			if (passwordForm.ShowDialog() == DialogResult.OK)
			{
				return passwordForm.Password;
			}
			else
			{
				return "";
			}
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
			else
			{
				return Number;
			}
		}
		

		public static string DBDate(DateTime Dt)
		{
			string strDate = "";
			strDate = Dt.Year + LeadingZero(Convert.ToString(Dt.Month)) + LeadingZero(Convert.ToString(Dt.Day)) + " " + LeadingZero(Convert.ToString(Dt.Hour)) + ":" + LeadingZero(Convert.ToString(Dt.Minute)) + ":" + LeadingZero(System.Convert.ToString(Dt.Second));
			return strDate;
		}
		public static string PrepareForDB(string Text)
		{
			return ReplaceBooleanStringsWithNumbers(Text);
		}
        private static string ReplaceBooleanStringsWithNumbers(string Text)
        {
            Text = ReplaceTrueWith1(Text);
            Text = ReplaceFalseWith0(Text);
            return Text;
        }
        private static string ReplaceTrueWith1(string Text)
        {
            return Text.Replace("'True'", "1");
        }
        private static string ReplaceFalseWith0(string Text)
        {
            return Text.Replace("'False'", "0");
        }
		public static string PrepareValueForDB(string Text)
		{
			Text = Strings.Replace(Expression: Text, Find: "\'", Replacement: "\'\'", Compare: CompareMethod.Text);
			return Text;
		}
		

		public static object StringToEnum(Type t, string value)
		{
			return Enum.Parse(t, value);
		}


        public static string GetExceptionMessageRecursive(Exception ex)
        {
            return GetExceptionMessageRecursive(ex, Environment.NewLine);
        }
        public static string GetExceptionMessageRecursive(Exception ex, string separator)
		{
			string message = ex.Message;
			if (ex.InnerException != null)
			{
				string innerMessage = GetExceptionMessageRecursive(ex.InnerException, separator);
				message = string.Join(separator, new string[] {message, innerMessage});
			}
			return message;
		}
		

		public static Image TakeScreenshot(ConnectionWindow sender)
		{
			try
			{
				int LeftStart = sender.TabController.SelectedTab.PointToScreen(new Point(sender.TabController.SelectedTab.Left)).X; //Me.Left + Splitter.SplitterDistance + 11
				int TopStart = sender.TabController.SelectedTab.PointToScreen(new Point(sender.TabController.SelectedTab.Top)).Y; //Me.Top + Splitter.Top + TabController.Top + TabController.SelectedTab.Top * 2 - 3
				int LeftWidth = sender.TabController.SelectedTab.Width; //Me.Width - (Splitter.SplitterDistance + 16)
				int TopHeight = sender.TabController.SelectedTab.Height; //Me.Height - (Splitter.Top + TabController.Top + TabController.SelectedTab.Top * 2 + 2)
					
				Size currentFormSize = new Size(LeftWidth, TopHeight);
				Bitmap ScreenToBitmap = new Bitmap(LeftWidth, TopHeight);
				System.Drawing.Graphics gGraphics = System.Drawing.Graphics.FromImage(ScreenToBitmap);
					
				gGraphics.CopyFromScreen(new Point(LeftStart, TopStart), new Point(0, 0), currentFormSize);
					
				return ScreenToBitmap;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Taking Screenshot failed" + Environment.NewLine + ex.Message, true);
			}
				
			return null;
		}
		
		public class EnumTypeConverter : EnumConverter
		{
			private Type _enumType;
				
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
				FieldInfo fi = _enumType.GetField(Enum.GetName(_enumType, value));
				DescriptionAttribute dna = (DescriptionAttribute) (Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute)));
					
				if (dna != null)
				{
					return dna.Description;
				}
				else
				{
					return value.ToString();
				}
			}
				
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type srcType)
			{
				return srcType == typeof(string);
			}
				
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				foreach (FieldInfo fi in _enumType.GetFields())
				{
					DescriptionAttribute dna = (DescriptionAttribute) (Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute)));
						
					if ((dna != null) && (((string) value) == dna.Description))
					{
						return Enum.Parse(_enumType, fi.Name);
					}
				}
					
				return Enum.Parse(_enumType, (string) value);
			}
		}
		
		public class YesNoTypeConverter : TypeConverter
		{
				
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				if (sourceType == typeof(string))
				{
					return true;
				}
					
				return base.CanConvertFrom(context, sourceType);
			}
				
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				if (destinationType == typeof(string))
				{
					return true;
				}
					
				return base.CanConvertTo(context, destinationType);
			}
				
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				if (value.GetType() == typeof(string))
				{
					if ((value).ToString().ToLower() == My.Language.strYes.ToLower())
					{
						return true;
					}
						
					if ((value).ToString().ToLower() == My.Language.strNo.ToLower())
					{
						return false;
					}
						
					throw (new Exception("Values must be \"Yes\" or \"No\""));
				}
					
				return base.ConvertFrom(context, culture, value);
			}
				
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string))
				{
					return ((System.Convert.ToBoolean(value)) ? My.Language.strYes : My.Language.strNo);
				}
					
				return base.ConvertTo(context, culture, value, destinationType);
			}
				
			public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
			{
				return true;
			}
				
			public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
			{
				bool[] bools = new bool[] {true, false};
					
				TypeConverter.StandardValuesCollection svc = new TypeConverter.StandardValuesCollection(bools);
					
				return svc;
			}
		}
		
		public class Fullscreen
		{
			public Fullscreen(Form handledForm)
			{
				_handledForm = handledForm;
			}
				
			private Form _handledForm;
			private FormWindowState _savedWindowState;
			private FormBorderStyle _savedBorderStyle;
			private Rectangle _savedBounds;
				
			private bool _value = false;
            public bool Value
			{
				get
				{
					return _value;
				}
				set
				{
					if (_value == value)
					{
						return ;
					}
					if (!_value)
					{
						EnterFullscreen();
					}
					else
					{
						ExitFullscreen();
					}
					_value = value;
				}
			}
				
			private void EnterFullscreen()
			{
				_savedBorderStyle = _handledForm.FormBorderStyle;
				_savedWindowState = _handledForm.WindowState;
				_savedBounds = _handledForm.Bounds;
					
				_handledForm.FormBorderStyle = FormBorderStyle.None;
				if (_handledForm.WindowState == FormWindowState.Maximized)
				{
					_handledForm.WindowState = FormWindowState.Normal;
				}
				_handledForm.WindowState = FormWindowState.Maximized;
			}
			private void ExitFullscreen()
			{
				_handledForm.FormBorderStyle = _savedBorderStyle;
				_handledForm.WindowState = _savedWindowState;
				_handledForm.Bounds = _savedBounds;
			}
		}
	}
}