using System;
using System.ComponentModel;
using System.Globalization;
using mRemoteNG.Resources.Language;

// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.Tools
{
    public static class LocalizedAttributes
    {
        [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
        public class LocalizedCategoryAttribute(string value, int Order = 1) : CategoryAttribute(value)
        {
            private const int MaxOrder = 10;
            private int Order = Order > MaxOrder ? MaxOrder : Order;

            protected override string GetLocalizedString(string value)
            {
                string OrderPrefix = "";
                for (int x = 0; x <= MaxOrder - Order; x++)
                {
                    OrderPrefix += Convert.ToString("\t", CultureInfo.InvariantCulture);
                }

                return OrderPrefix + Language.ResourceManager.GetString(value, CultureInfo.CurrentCulture);
            }
        }

        [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
        public class LocalizedDisplayNameAttribute(string value) : DisplayNameAttribute(value)
        {
            private bool Localized;

            public override string DisplayName
            {
                get
                {
                    if (!Localized)
                    {
                        Localized = true;
                        DisplayNameValue = Language.ResourceManager.GetString(DisplayNameValue, CultureInfo.CurrentCulture) ?? DisplayNameValue;
                    }

                    return base.DisplayName;
                }
            }
        }

        [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
        public class LocalizedDescriptionAttribute(string value) : DescriptionAttribute(value)
        {
            private bool Localized;

            public override string Description
            {
                get
                {
                    if (!Localized)
                    {
                        Localized = true;
                        DescriptionValue = Language.ResourceManager.GetString(DescriptionValue, CultureInfo.CurrentCulture) ?? DescriptionValue;
                    }

                    return base.Description;
                }
            }
        }

        [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
        public class LocalizedDefaultValueAttribute(string name) : DefaultValueAttribute(Language.ResourceManager.GetString(name, CultureInfo.CurrentCulture))
        {

            // This allows localized attributes in a derived class to override a matching
            // non-localized attribute inherited from its base class
            public override object TypeId => typeof(DefaultValueAttribute);
        }

        #region Special localization - with String.Format

        [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
        public class LocalizedDisplayNameInheritAttribute(string value) : DisplayNameAttribute(value)
        {
            private bool Localized;

            public override string DisplayName
            {
                get
                {
                    if (!Localized)
                    {
                        Localized = true;
                        DisplayNameValue = string.Format(CultureInfo.CurrentCulture, Language.FormatInherit,
                                                         Language.ResourceManager.GetString(DisplayNameValue, CultureInfo.CurrentCulture));
                    }

                    return base.DisplayName;
                }
            }
        }

        [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
        public class LocalizedDescriptionInheritAttribute(string value) : DescriptionAttribute(value)
        {
            private bool Localized;

            public override string Description
            {
                get
                {
                    if (!Localized)
                    {
                        Localized = true;
                        DescriptionValue = string.Format(CultureInfo.CurrentCulture, Language.FormatInheritDescription,
                                                         Language.ResourceManager.GetString(DescriptionValue, CultureInfo.CurrentCulture));
                    }

                    return base.Description;
                }
            }
        }

        #endregion
    }
}