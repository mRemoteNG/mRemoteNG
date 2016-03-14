using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using mRemoteNG.My;
using mRemoteNG.Tools;

namespace mRemoteNG.Themes
{
    public class ThemeColorCollection : ICloneable, INotifyPropertyChanged
    {
        MainWindowColorCollection mainWindowColors;
        ConnectionPanelColorCollection connectionPanelColors;
        ConfigPanelColorCollection configPanelColors;

        public ThemeColorCollection()
        {
            mainWindowColors = new MainWindowColorCollection();
            connectionPanelColors = new ConnectionPanelColorCollection();
            configPanelColors = new ConfigPanelColorCollection();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public void ApplyColors()
        {
            mainWindowColors.ApplyColors();
            connectionPanelColors.ApplyColors();
            configPanelColors.ApplyColors();
        }

        public override bool Equals(object obj)
        {
            if (IsAThemeColorCollection(obj) && this.AreAllPropertiesEqual(obj as ThemeColorCollection))
                return true;
            else
                return false;
        }

        private static bool IsAThemeColorCollection(object Object)
        {
            ThemeColorCollection objectWeAreTesting = Object as ThemeColorCollection;
            if (objectWeAreTesting == null)
                return false;
            else
                return true;
        }

        private bool AreAllPropertiesEqual(ThemeColorCollection OtherObject)
        {
            foreach (System.Reflection.PropertyInfo property in this.GetType().GetProperties())
            {
                if (this.IsPropertyEqual(OtherObject, property) != true)
                    return false;
            }
            return true;
        }

        private bool IsPropertyEqual(ThemeColorCollection OtherObject, System.Reflection.PropertyInfo PropertyToTest)
        {
            object myProperty = PropertyToTest.GetValue(this, null);
            object otherProperty = PropertyToTest.GetValue(OtherObject, null);
            return (myProperty.Equals(otherProperty));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}