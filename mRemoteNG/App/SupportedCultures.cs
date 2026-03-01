using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Versioning;
using mRemoteNG.Properties;

// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.App
{
    [SupportedOSPlatform("windows")]
    public sealed class SupportedCultures : Dictionary<string, string>
    {
        private static SupportedCultures? _Instance;

        private static SupportedCultures SingletonInstance
        {
            get { return _Instance ?? (_Instance = new SupportedCultures()); }
        }


        private SupportedCultures()
        {
            foreach (string CultureName in Properties.AppUI.Default.SupportedUICultures.Split(','))
            {
                try
                {
                    CultureInfo CultureInfo = new(CultureName.Trim());
                    Add(CultureInfo.Name, CultureInfo.TextInfo.ToTitleCase(CultureInfo.NativeName));
                }
                catch (Exception ex)
                {
                    Debug.Print(
                                $"An exception occurred while adding the culture {CultureName} to the list of supported cultures. {ex.StackTrace}");
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

        public static string GetCultureName(string CultureNativeName)
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

        public static string GetCultureNativeName(string CultureName)
        {
            return SingletonInstance[CultureName];
        }

        public static IList<string> CultureNativeNames
        {
            get
            {
                List<string> ValueList = new();
                foreach (string Value in SingletonInstance.Values)
                {
                    ValueList.Add(Value);
                }

                return ValueList;
            }
        }
    }
}