using System;
using System.ComponentModel;


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
				if (Order > LocalizedCategoryAttribute.MaxOrder)
				{
					this.Order = LocalizedCategoryAttribute.MaxOrder;
				}
				else
				{
					this.Order = Order;
				}
			}
				
			protected override string GetLocalizedString(string value)
			{
				string OrderPrefix = "";
				for (int x = 0; x <= LocalizedCategoryAttribute.MaxOrder - this.Order; x++)
				{
					OrderPrefix += System.Convert.ToString("\t");
				}
					
				return OrderPrefix + My.Language.ResourceManager.GetString(value);
			}
		}
			
		[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
        public class LocalizedDisplayNameAttribute : DisplayNameAttribute
		{
				
			private bool Localized;
				
			public LocalizedDisplayNameAttribute(string value) : base(value)
			{
				this.Localized = false;
			}
				
            public override string DisplayName
			{
				get
				{
					if (!this.Localized)
					{
						this.Localized = true;
						this.DisplayNameValue = My.Language.ResourceManager.GetString(this.DisplayNameValue);
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
				this.Localized = false;
			}
				
            public override string Description
			{
				get
				{
					if (!this.Localized)
					{
						this.Localized = true;
						this.DescriptionValue = My.Language.ResourceManager.GetString(this.DescriptionValue);
					}
						
					return base.Description;
				}
			}
		}
			
		[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
        public class LocalizedDefaultValueAttribute : DefaultValueAttribute
		{
			public LocalizedDefaultValueAttribute(string name) : base(My.Language.ResourceManager.GetString(name))
			{
			}
				
			// This allows localized attributes in a derived class to override a matching
			// non-localized attribute inherited from its base class
            public override object TypeId
			{
				get
				{
					return typeof(DefaultValueAttribute);
				}
			}
		}
			
        #region Special localization - with String.Format
		[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
        public class LocalizedDisplayNameInheritAttribute : DisplayNameAttribute
		{
			private bool Localized;
			public LocalizedDisplayNameInheritAttribute(string value) : base(value)
			{
					
				this.Localized = false;
			}
				
            public override string DisplayName
			{
				get
				{
					if (!this.Localized)
					{
						this.Localized = true;
						this.DisplayNameValue = string.Format(My.Language.strFormatInherit, My.Language.ResourceManager.GetString(this.DisplayNameValue));
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
					
				this.Localized = false;
			}
				
            public override string Description
			{
				get
				{
					if (!this.Localized)
					{
						this.Localized = true;
						this.DescriptionValue = string.Format(My.Language.strFormatInheritDescription, My.Language.ResourceManager.GetString(this.DescriptionValue));
					}
						
					return base.Description;
				}
			}
		}
        #endregion
	}
}
