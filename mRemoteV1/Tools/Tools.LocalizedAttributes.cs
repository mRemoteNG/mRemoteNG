using System;
using System.ComponentModel;
// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.Tools
{
	public class LocalizedAttributes
	{
		[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
        public class LocalizedCategoryAttribute : CategoryAttribute
		{
			private const int MaxOrder = 10;
			private int Order;
				
			public LocalizedCategoryAttribute(string value, int Order = 1) : base(value)
			{
			    this.Order = Order > MaxOrder ? MaxOrder : Order;
			}
				
			protected override string GetLocalizedString(string value)
			{
				string OrderPrefix = "";
				for (int x = 0; x <= MaxOrder - Order; x++)
				{
					OrderPrefix += Convert.ToString("\t");
				}
					
				return OrderPrefix + Language.ResourceManager.GetString(value);
			}
		}
			
		[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
        public class LocalizedDisplayNameAttribute : DisplayNameAttribute
		{
				
			private bool Localized;
				
			public LocalizedDisplayNameAttribute(string value) : base(value)
			{
				Localized = false;
			}
				
            public override string DisplayName
			{
				get
				{
					if (!Localized)
					{
						Localized = true;
						DisplayNameValue = Language.ResourceManager.GetString(DisplayNameValue);
					}
						
					return base.DisplayName;
				}
			}
		}
			
		[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
        public class LocalizedDescriptionAttribute : DescriptionAttribute
		{
			private bool Localized;
			public LocalizedDescriptionAttribute(string value) : base(value)
			{
				Localized = false;
			}
				
            public override string Description
			{
				get
				{
					if (!Localized)
					{
						Localized = true;
						DescriptionValue = Language.ResourceManager.GetString(DescriptionValue);
					}
						
					return base.Description;
				}
			}
		}
			
		[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
        public class LocalizedDefaultValueAttribute : DefaultValueAttribute
		{
			public LocalizedDefaultValueAttribute(string name) : base(Language.ResourceManager.GetString(name))
			{
			}
				
			// This allows localized attributes in a derived class to override a matching
			// non-localized attribute inherited from its base class
            public override object TypeId
            {
                get { return typeof(DefaultValueAttribute); }
            }
		}
			
        #region Special localization - with String.Format
		[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
        public class LocalizedDisplayNameInheritAttribute : DisplayNameAttribute
		{
			private bool Localized;
			public LocalizedDisplayNameInheritAttribute(string value) : base(value)
			{
					
				Localized = false;
			}
				
            public override string DisplayName
			{
				get
				{
					if (!Localized)
					{
						Localized = true;
						DisplayNameValue = string.Format(Language.strFormatInherit, Language.ResourceManager.GetString(DisplayNameValue));
					}
						
					return base.DisplayName;
				}
			}
		}
			
		[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
        public class LocalizedDescriptionInheritAttribute : DescriptionAttribute
		{
			private bool Localized;
			public LocalizedDescriptionInheritAttribute(string value) : base(value)
			{
					
				Localized = false;
			}
				
            public override string Description
			{
				get
				{
					if (!Localized)
					{
						Localized = true;
						DescriptionValue = string.Format(Language.strFormatInheritDescription, Language.ResourceManager.GetString(DescriptionValue));
					}
						
					return base.Description;
				}
			}
		}
        #endregion
	}
}
