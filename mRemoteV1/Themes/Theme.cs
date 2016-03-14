using mRemoteNG.My;
using mRemoteNG.Tools;
using System;
using System.ComponentModel;
using System.Drawing;


namespace mRemoteNG.Themes
{
	public class Theme : ICloneable, INotifyPropertyChanged
    {
        private string _name;
        private ThemeColorCollection _themeColors;

        [Browsable(false)]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (!System.String.IsNullOrEmpty(value) && _name != value)
                {
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        [Browsable(false)]
        public ThemeColorCollection ThemeColors
        {
            get 
            {
                return _themeColors;
            }
            set 
            {
                if (value != null)
                {
                    _themeColors = value;
                    NotifyPropertyChanged("ThemeColors");
                }
            }
        }

        public Theme()
        {
            Name = Language.strUnnamedTheme;
            ThemeColors = new ThemeColorCollection();
        }

        public Theme(string themeName) : this()
        {
            Name = themeName;
        }

        public void ApplyTheme()
        {
            _themeColors.ApplyColors();
        }

		public object Clone()
		{
			return MemberwiseClone();
		}
			
		public override string ToString()
		{
			return Name;
		}
			
		public override bool Equals(object obj)
		{
            if (IsATheme(obj) && this.IsEqualToTheme(obj as Theme))
                return true;
            else
                return false;
		}
        
        private static bool IsATheme(object Object)
        {
            Theme objectWeAreTesting = Object as Theme;
            if (objectWeAreTesting == null)
                return false;
            else
                return true;
        }

        private bool IsEqualToTheme(Theme OtherTheme)
        {
            foreach (System.Reflection.PropertyInfo property in this.GetType().GetProperties())
            {
                if (this.IsPropertyEqual(OtherTheme, property) != true)
                    return false;
            }
            return true;
        }

        private bool IsPropertyEqual(Theme OtherTheme, System.Reflection.PropertyInfo ThemeProperty)
        {
            object myProperty = ThemeProperty.GetValue(this, null);
            object otherProperty = ThemeProperty.GetValue(OtherTheme, null);
            return (myProperty.Equals(otherProperty));
        }

        public override int GetHashCode()
        {
            int hash = 29;
            foreach (System.Reflection.PropertyInfo property in this.GetType().GetProperties())
            {
                hash = hash * 17 + System.Convert.ToInt32(property.GetValue(this, null)).GetHashCode();
            }
            return hash;
        }
			
		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}