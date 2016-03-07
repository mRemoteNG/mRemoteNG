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
using System.Reflection;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using mRemoteNG.Forms;
using mRemoteNG.App;
using System.IO;
using System.Data.SqlClient;


namespace mRemoteNG.Tools
{
	public class Misc
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
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "GetIconFromFile failed (Tools.Misc)" + Constants.vbNewLine + ex.Message, true);
				return null;
			}
		}
			
		public delegate void SQLUpdateCheckFinishedEventHandler(bool UpdateAvailable);
		private static SQLUpdateCheckFinishedEventHandler SQLUpdateCheckFinishedEvent;
			
		public static event SQLUpdateCheckFinishedEventHandler SQLUpdateCheckFinished
		{
			add
			{
				SQLUpdateCheckFinishedEvent = (SQLUpdateCheckFinishedEventHandler) System.Delegate.Combine(SQLUpdateCheckFinishedEvent, value);
			}
			remove
			{
				SQLUpdateCheckFinishedEvent = (SQLUpdateCheckFinishedEventHandler) System.Delegate.Remove(SQLUpdateCheckFinishedEvent, value);
			}
		}
			
		public static void IsSQLUpdateAvailableAsync()
		{
            System.Threading.Thread t = new System.Threading.Thread(IsSQLUpdateAvailableDelegate);
			t.SetApartmentState(System.Threading.ApartmentState.STA);
			t.Start();
		}

        private static void IsSQLUpdateAvailableDelegate()
        {
            IsSQLUpdateAvailable();
        }

		public static bool IsSQLUpdateAvailable()
		{
			try
			{
				SqlConnection sqlCon = default(SqlConnection);
				SqlCommand sqlQuery = default(SqlCommand);
				SqlDataReader sqlRd = default(SqlDataReader);
					
				DateTime LastUpdateInDB = default(DateTime);
					
				if (My.Settings.Default.SQLUser != "")
				{
					sqlCon = new SqlConnection("Data Source=" + My.Settings.Default.SQLHost + ";Initial Catalog=" + My.Settings.Default.SQLDatabaseName + ";User Id=" + My.Settings.Default.SQLUser + ";Password=" + Security.Crypt.Decrypt(System.Convert.ToString(My.Settings.Default.SQLPass), App.Info.General.EncryptionKey));
				}
				else
				{
					sqlCon = new SqlConnection("Data Source=" + My.Settings.Default.SQLHost + ";Initial Catalog=" + My.Settings.Default.SQLDatabaseName + ";Integrated Security=True");
				}
					
				sqlCon.Open();
					
				sqlQuery = new SqlCommand("SELECT * FROM tblUpdate", sqlCon);
				sqlRd = sqlQuery.ExecuteReader(CommandBehavior.CloseConnection);
					
				sqlRd.Read();
					
				if (sqlRd.HasRows)
				{
					LastUpdateInDB = System.Convert.ToDateTime(sqlRd["LastUpdate"]);
						
					if (LastUpdateInDB > Runtime.LastSqlUpdate)
					{
                        if (SQLUpdateCheckFinishedEvent != null)
                        {
                            SQLUpdateCheckFinishedEvent(true);
                        }
						return true;
					}
				}
					
				if (SQLUpdateCheckFinishedEvent != null)
                {
					SQLUpdateCheckFinishedEvent(false);
                }
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "IsSQLUpdateAvailable failed (Tools.Misc)" + Constants.vbNewLine + ex.Message, true);
			}
				
			return false;
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
			if (System.Convert.ToInt32(Number) < 10)
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
				
			strDate = Dt.Year + LeadingZero(System.Convert.ToString(Dt.Month)) + LeadingZero(System.Convert.ToString(Dt.Day)) + " " + LeadingZero(System.Convert.ToString(Dt.Hour)) + ":" + LeadingZero(System.Convert.ToString(Dt.Minute)) + ":" + LeadingZero(System.Convert.ToString(Dt.Second));
				
			return strDate;
		}
			
		public static string PrepareForDB(string Text)
		{
			Text = Strings.Replace(Expression: Text, Find: "\'True\'", Replacement: "1", Compare: CompareMethod.Text);
			Text = Strings.Replace(Expression: Text, Find: "\'False\'", Replacement: "0", Compare: CompareMethod.Text);
				
			return Text;
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
			
		public static string GetExceptionMessageRecursive(Exception ex, string separator = Constants.vbNewLine)
		{
			string message = ex.Message;
			if (ex.InnerException != null)
			{
				string innerMessage = GetExceptionMessageRecursive(ex.InnerException, separator);
				message = string.Join(separator, new string[] {message, innerMessage});
			}
			return message;
		}
			
		public static Image TakeScreenshot(UI.Window.Connection sender)
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
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Taking Screenshot failed" + Constants.vbNewLine + ex.Message, true);
			}
				
			return null;
		}
			
		public class EnumTypeConverter : EnumConverter
		{
			private System.Type _enumType;
				
			public EnumTypeConverter(System.Type type) : base(type)
			{
				_enumType = type;
			}
				
			public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destType)
			{
				return destType == typeof(string);
			}
				
			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, System.Type destType)
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
				
			public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type srcType)
			{
				return srcType == typeof(string);
			}
				
			public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
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
				
			public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
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
				
			public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
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
				
			public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
			{
				bool[] bools = new bool[] {true, false};
					
				System.ComponentModel.TypeConverter.StandardValuesCollection svc = new System.ComponentModel.TypeConverter.StandardValuesCollection(bools);
					
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
			
			
		//
		//* Arguments class: application arguments interpreter
		//*
		//* Authors:		R. LOPES
		//* Contributors:	R. LOPES
		//* Created:		25 October 2002
		//* Modified:		28 October 2002
		//*
		//* Version:		1.0
		//
		public class CMDArguments
		{
			private StringDictionary Parameters;
				
			// Retrieve a parameter value if it exists
            public string this[string Param]
			{
				get
				{
					return (Parameters[Param]);
				}
			}
				
			public CMDArguments(string[] Args)
			{
				Parameters = new StringDictionary();
				Regex Spliter = new Regex("^-{1,2}|^/|=|:", (System.Text.RegularExpressions.RegexOptions) (RegexOptions.IgnoreCase | RegexOptions.Compiled));
				Regex Remover = new Regex("^[\'\"]?(.*?)[\'\"]?$", (System.Text.RegularExpressions.RegexOptions) (RegexOptions.IgnoreCase | RegexOptions.Compiled));
				string Parameter = null;
				string[] Parts = null;
					
				// Valid parameters forms:
				// {-,/,--}param{ ,=,:}((",')value(",'))
				// Examples: -param1 value1 --param2 /param3:"Test-:-work" /param4=happy -param5 '--=nice=--'
					
				try
				{
					foreach (string Txt in Args)
					{
						// Look for new parameters (-,/ or --) and a possible enclosed value (=,:)
						Parts = Spliter.Split(Txt, 3);
						switch (Parts.Length)
						{
							case 1:
								// Found a value (for the last parameter found (space separator))
								if (Parameter != null)
								{
									if (!Parameters.ContainsKey(Parameter))
									{
										Parts[0] = Remover.Replace(Parts[0], "$1");
										Parameters.Add(Parameter, Parts[0]);
									}
									Parameter = null;
								}
								// else Error: no parameter waiting for a value (skipped)
								break;
							case 2:
								// Found just a parameter
								// The last parameter is still waiting. With no value, set it to true.
								if (Parameter != null)
								{
									if (!Parameters.ContainsKey(Parameter))
									{
										Parameters.Add(Parameter, "true");
									}
								}
								Parameter = Parts[1];
								break;
							case 3:
								// Parameter with enclosed value
								// The last parameter is still waiting. With no value, set it to true.
								if (Parameter != null)
								{
									if (!Parameters.ContainsKey(Parameter))
									{
										Parameters.Add(Parameter, "true");
									}
								}
								Parameter = Parts[1];
								// Remove possible enclosing characters (",')
								if (!Parameters.ContainsKey(Parameter))
								{
									Parts[2] = Remover.Replace(Parts[2], "$1");
									Parameters.Add(Parameter, Parts[2]);
								}
								Parameter = null;
								break;
						}
					}
					// In case a parameter is still waiting
					if (Parameter != null)
					{
						if (!Parameters.ContainsKey(Parameter))
						{
							Parameters.Add(Parameter, "true");
						}
					}
				}
				catch (Exception ex)
				{
					Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Creating new Args failed" + Constants.vbNewLine + ex.Message, true);
				}
			}
		}
	}
}
