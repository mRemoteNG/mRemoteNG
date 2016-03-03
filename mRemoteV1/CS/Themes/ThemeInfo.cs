// VBConversions Note: VB project level imports
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
// End of VB project level imports

using System.ComponentModel;
//using mRemoteNG.Tools.LocalizedAttributes;
using mRemoteNG.My;
using mRemoteNG.Tools;


namespace mRemoteNG.Themes
{
	public class ThemeInfo : ICloneable, INotifyPropertyChanged
	{
#region Public Methods
		public ThemeInfo(string themeName = null)
		{
			// VBConversions Note: Non-static class variable initialization is below.  Class variables cannot be initially assigned non-static values in C#.
			_name = Language.strUnnamedTheme;
				
			if (themeName != null)
			{
				Name = themeName;
			}
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
			ThemeInfo otherTheme = obj as ThemeInfo;
			if (otherTheme == null)
			{
				return false;
			}
				
			Type themeInfoType = (new ThemeInfo()).GetType();
			object myProperty = null;
			object otherProperty = null;
			foreach (System.Reflection.PropertyInfo propertyInfo in themeInfoType.GetProperties())
			{
				myProperty = propertyInfo.GetValue(this, null);
				otherProperty = propertyInfo.GetValue(otherTheme, null);
				if (!myProperty.Equals(otherProperty))
				{
					return false;
				}
			}
				
			return true;
		}
#endregion
			
#region Events
		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged(string propertyName)
		{
			if 
				(PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
#endregion
			
#region Properties
		private string _name; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [Browsable(false)]
        public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				if (_name == value)
				{
					return ;
				}
				_name = value;
				NotifyPropertyChanged("Name");
			}
		}
			
#region General
		private Color _windowBackgroundColor; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strThemeCategoryGeneral", 1),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameWindowBackgroundColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionWindowBackgroundColor")]
        public Color WindowBackgroundColor
		{
			get
			{
				return (_windowBackgroundColor);
			}
			set
			{
				if (_windowBackgroundColor == value)
				{
					return ;
				}
				_windowBackgroundColor = value;
				NotifyPropertyChanged("WindowBackgroundColor");
			}
		}
			
		private Color _menuBackgroundColor; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strThemeCategoryGeneral", 1), Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameMenuBackgroundColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionMenuBackgroundColor")]
        public Color MenuBackgroundColor
		{
			get
			{
				return _menuBackgroundColor;
			}
			set
			{
				if (_menuBackgroundColor == value)
				{
					return ;
				}
				_menuBackgroundColor = value;
				NotifyPropertyChanged("MenuBackgroundColor");
			}
		}
			
		private Color _menuTextColor; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strThemeCategoryGeneral", 1), Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameMenuTextColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionMenuTextColor")]
        public Color MenuTextColor
		{
			get
			{
				return _menuTextColor;
			}
			set
			{
				if (_menuTextColor == value)
				{
					return ;
				}
				_menuTextColor = value;
				NotifyPropertyChanged("MenuTextColor");
			}
		}
			
		private Color _toolbarBackgroundColor; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strThemeCategoryGeneral", 1),
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameToolbarBackgroundColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionToolbarBackgroundColor")]
        public Color ToolbarBackgroundColor
		{
			get
			{
				return _toolbarBackgroundColor;
			}
			set
			{
				if (_toolbarBackgroundColor == value || value.A < 255)
				{
					return ;
				}
				_toolbarBackgroundColor = value;
				NotifyPropertyChanged("ToolbarBackgroundColor");
			}
		}
			
		private Color _toolbarTextColor; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strThemeCategoryGeneral", 1), Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameToolbarTextColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionToolbarTextColor")]
        public Color ToolbarTextColor
		{
			get
			{
				return _toolbarTextColor;
			}
			set
			{
				if (_toolbarTextColor == value)
				{
					return ;
				}
				_toolbarTextColor = value;
				NotifyPropertyChanged("ToolbarTextColor");
			}
		}
#endregion
			
#region Connections Panel
		private Color _connectionsPanelBackgroundColor; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strThemeCategoryConnectionsPanel", 2),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameConnectionsPanelBackgroundColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionConnectionsPanelBackgroundColor")]
        public Color ConnectionsPanelBackgroundColor
		{
			get
			{
				return _connectionsPanelBackgroundColor;
			}
			set
			{
				if (_connectionsPanelBackgroundColor == value || value.A < 255)
				{
					return ;
				}
				_connectionsPanelBackgroundColor = value;
				NotifyPropertyChanged("ConnectionsPanelBackgroundColor");
			}
		}
			
		private Color _connectionsPanelTextColor; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strThemeCategoryConnectionsPanel", 2),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameConnectionsPanelTextColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionConnectionsPanelTextColor")]
        public Color ConnectionsPanelTextColor
		{
			get
			{
				return _connectionsPanelTextColor;
			}
			set
			{
				if (_connectionsPanelTextColor == value)
				{
					return ;
				}
				_connectionsPanelTextColor = value;
				NotifyPropertyChanged("ConnectionsPanelTextColor");
			}
		}
			
		private Color _connectionsPanelTreeLineColor; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strThemeCategoryConnectionsPanel", 2),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameConnectionsPanelTreeLineColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionConnectionsPanelTreeLineColor")]
        public Color ConnectionsPanelTreeLineColor
		{
			get
			{
				return _connectionsPanelTreeLineColor;
			}
			set
			{
				if (_connectionsPanelTreeLineColor == value)
				{
					return ;
				}
				_connectionsPanelTreeLineColor = value;
				NotifyPropertyChanged("ConnectionsPanelTreeLineColor");
			}
		}
			
		private Color _searchBoxBackgroundColor; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strThemeCategoryConnectionsPanel", 2),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameSearchBoxBackgroundColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionSearchBoxBackgroundColor")]
        public Color SearchBoxBackgroundColor
		{
			get
			{
				return _searchBoxBackgroundColor;
			}
			set
			{
				if (_searchBoxBackgroundColor == value || value.A < 255)
				{
					return ;
				}
				_searchBoxBackgroundColor = value;
				NotifyPropertyChanged("SearchBoxBackgroundColor");
			}
		}
			
		private Color _searchBoxTextPromptColor; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strThemeCategoryConnectionsPanel",2),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameSearchBoxTextPromptColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionSearchBoxTextPromptColor")]
        public Color SearchBoxTextPromptColor
		{
			get
			{
				return _searchBoxTextPromptColor;
			}
			set
			{
				if (_searchBoxTextPromptColor == value)
				{
					return ;
				}
				_searchBoxTextPromptColor = value;
				NotifyPropertyChanged("SearchBoxTextPromptColor");
			}
		}
			
		private Color _searchBoxTextColor; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strThemeCategoryConnectionsPanel", 2),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameSearchBoxTextColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionSearchBoxTextColor")]
        public Color SearchBoxTextColor
		{
			get
			{
				return _searchBoxTextColor;
			}
			set
			{
				if (_searchBoxTextColor == value)
				{
					return ;
				}
				_searchBoxTextColor = value;
				NotifyPropertyChanged("SearchBoxTextColor");
			}
		}
#endregion
			
#region Config Panel
		private Color _configPanelBackgroundColor; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strThemeCategoryConfigPanel", 3),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameConfigPanelBackgroundColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionConfigPanelBackgroundColor")]
        public Color ConfigPanelBackgroundColor
		{
			get
			{
				return _configPanelBackgroundColor;
			}
			set
			{
				if (_configPanelBackgroundColor == value || value.A < 255)
				{
					return ;
				}
				_configPanelBackgroundColor = value;
				NotifyPropertyChanged("ConfigPanelBackgroundColor");
			}
		}
			
		private Color _configPanelTextColor; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strThemeCategoryConfigPanel", 3),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameConfigPanelTextColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionConfigPanelTextColor")]
        public Color ConfigPanelTextColor
		{
			get
			{
				return _configPanelTextColor;
			}
			set
			{
				if (_configPanelTextColor == value)
				{
					return ;
				}
				_configPanelTextColor = value;
				NotifyPropertyChanged("ConfigPanelTextColor");
			}
		}
			
		private Color _configPanelCategoryTextColor; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strThemeCategoryConfigPanel", 3),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameConfigPanelCategoryTextColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionConfigPanelCategoryTextColor")]
        public Color ConfigPanelCategoryTextColor
		{
			get
			{
				return _configPanelCategoryTextColor;
			}
			set
			{
				if (_configPanelCategoryTextColor == value)
				{
					return ;
				}
				_configPanelCategoryTextColor = value;
				NotifyPropertyChanged("ConfigPanelCategoryTextColor");
			}
		}
			
		private Color _configPanelHelpBackgroundColor; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strThemeCategoryConfigPanel", 3),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameConfigPanelHelpBackgroundColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionConfigPanelHelpBackgroundColor")]
        public Color ConfigPanelHelpBackgroundColor
		{
			get
			{
				return _configPanelHelpBackgroundColor;
			}
			set
			{
				if (_configPanelHelpBackgroundColor == value || value.A < 255)
				{
					return ;
				}
				_configPanelHelpBackgroundColor = value;
				NotifyPropertyChanged("ConfigPanelHelpBackgroundColor");
			}
		}
			
		private Color _configPanelHelpTextColor; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strThemeCategoryConfigPanel", 3),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameConfigPanelHelpTextColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionConfigPanelHelpTextColor")]
        public Color ConfigPanelHelpTextColor
		{
			get
			{
				return _configPanelHelpTextColor;
			}
			set
			{
				if (_configPanelHelpTextColor == value)
				{
					return ;
				}
				_configPanelHelpTextColor = value;
				NotifyPropertyChanged("ConfigPanelHelpTextColor");
			}
		}
			
		private Color _configPanelGridLineColor; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        [LocalizedAttributes.LocalizedCategory("strThemeCategoryConfigPanel", 3),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameConfigPanelGridLineColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionConfigPanelGridLineColor")]
        public Color ConfigPanelGridLineColor
		{
			get
			{
				return _configPanelGridLineColor;
			}
			set
			{
				if (_configPanelGridLineColor == value)
				{
					return ;
				}
				_configPanelGridLineColor = value;
				NotifyPropertyChanged("ConfigPanelGridLineColor");
			}
		}
#endregion
#endregion
	}
}
