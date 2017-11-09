using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.App
{
    [Serializable]
    public class SupportedCultures : Dictionary<string, string>
	{
        private static SupportedCultures _Instance;

        private static SupportedCultures SingletonInstance
        {
            get { return _Instance ?? (_Instance = new SupportedCultures()); }
        }


	    private SupportedCultures()
        {
            foreach (var CultureName in Settings.Default.SupportedUICultures.Split(','))
            {
                try
                {
                    var CultureInfo = new CultureInfo(CultureName.Trim());
                    Add(CultureInfo.Name, CultureInfo.TextInfo.ToTitleCase(CultureInfo.NativeName));
                }
                catch (Exception ex)
                {
                    Debug.Print($"An exception occurred while adding the culture {CultureName} to the list of supported cultures. {ex.StackTrace}");
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
			var Names = new string[SingletonInstance.Count + 1];
			var NativeNames = new string[SingletonInstance.Count + 1];

            SingletonInstance.Keys.CopyTo(Names, 0);
            SingletonInstance.Values.CopyTo(NativeNames, 0);
				
			for (var Index = 0; Index <= SingletonInstance.Count; Index++)
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
				var ValueList = new List<string>();
				foreach (var Value in SingletonInstance.Values)
				{
					ValueList.Add(Value);
				}
				return ValueList;
			}
		}
	}
}