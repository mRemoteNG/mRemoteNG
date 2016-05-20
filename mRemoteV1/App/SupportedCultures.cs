using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;


namespace mRemoteNG.App
{
	public class SupportedCultures : Dictionary<string, string>
	{
        private static SupportedCultures _Instance = null;

        private static SupportedCultures SingletonInstance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new SupportedCultures();
                return _Instance;
            }
        }
		

        private SupportedCultures()
        {
            CultureInfo CultureInfo = default(CultureInfo);
            foreach (string CultureName in Settings.Default.SupportedUICultures.Split(','))
            {
                try
                {
                    CultureInfo = new CultureInfo(CultureName.Trim());
                    Add(CultureInfo.Name, CultureInfo.TextInfo.ToTitleCase(CultureInfo.NativeName));
                }
                catch (Exception ex)
                {
                    Debug.Print(string.Format("An exception occurred while adding the culture \'{0}\' to the list of supported cultures. {1}", CultureName, ex.ToString()));
                }
            }
        }
			
		public static bool IsNameSupported(string CultureName)
		{
			return SingletonInstance.ContainsKey(CultureName);
		}
			
		public static bool IsNativeNameSupported(string CultureNativeName)
		{
			return SingletonInstance.ContainsValue(CultureNativeName);
		}
			
		public static string get_CultureName(string CultureNativeName)
		{
			string[] Names = new string[SingletonInstance.Count + 1];
			string[] NativeNames = new string[SingletonInstance.Count + 1];

            SingletonInstance.Keys.CopyTo(Names, 0);
            SingletonInstance.Values.CopyTo(NativeNames, 0);
				
			for (int Index = 0; Index <= SingletonInstance.Count; Index++)
			{
				if (NativeNames[Index] == CultureNativeName)
				{
					return Names[Index];
				}
			}
				
			throw (new KeyNotFoundException());
		}
			
		public static string get_CultureNativeName(string CultureName)
		{
			return SingletonInstance[CultureName];
		}
			
        public static List<string> CultureNativeNames
		{
			get
			{
				List<string> ValueList = new List<string>();
				foreach (string Value in SingletonInstance.Values)
				{
					ValueList.Add(Value);
				}
				return ValueList;
			}
		}
	}
}