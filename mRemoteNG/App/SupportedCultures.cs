using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.Versioning;
using mRemoteNG.Properties;

// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.App
{
    [SupportedOSPlatform("windows")]
    [Serializable]
    public sealed class SupportedCultures : Dictionary<string, string>
    {
        private static SupportedCultures _Instance;

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

        // fix CA2229 - https://docs.microsoft.com/en-us/visualstudio/code-quality/ca2229-implement-serialization-constructors?view=vs-2017
        private SupportedCultures(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
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