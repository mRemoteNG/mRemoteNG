using mRemoteNG.Tools;
using System;
using System.ComponentModel;
using System.Drawing;


namespace mRemoteNG.Themes
{
	public class ThemeInfo : ICloneable, INotifyPropertyChanged
    {
        #region Private Variables
        private string _name;
        private Color _windowBackgroundColor;
        private Color _menuBackgroundColor;
        private Color _menuTextColor;
        private Color _toolbarBackgroundColor;
        private Color _toolbarTextColor;
        private Color _connectionsPanelBackgroundColor;
        private Color _connectionsPanelTextColor;
        private Color _connectionsPanelTreeLineColor;
        private Color _searchBoxBackgroundColor;
        private Color _searchBoxTextPromptColor;
        private Color _searchBoxTextColor;
        private Color _configPanelBackgroundColor;
        private Color _configPanelTextColor;
        private Color _configPanelCategoryTextColor;
        private Color _configPanelHelpBackgroundColor;
        private Color _configPanelHelpTextColor;
        private Color _configPanelGridLineColor;
        #endregion

        #region Constructors
        public ThemeInfo(string themeName = null)
        {
            _name = Language.strUnnamedTheme;
            _windowBackgroundColor = SystemColors.AppWorkspace;
            _menuBackgroundColor = SystemColors.Control;
            _menuTextColor = SystemColors.ControlText;
            _toolbarBackgroundColor = SystemColors.Control;
            _toolbarTextColor = SystemColors.ControlText;
            _connectionsPanelBackgroundColor = SystemColors.Window;
            _connectionsPanelTextColor = SystemColors.WindowText;
            _connectionsPanelTreeLineColor = Color.Black;
            _searchBoxBackgroundColor = SystemColors.Window;
            _searchBoxTextPromptColor = SystemColors.GrayText;
            _searchBoxTextColor = SystemColors.WindowText;
            _configPanelBackgroundColor = SystemColors.Window;
            _configPanelTextColor = SystemColors.WindowText;
            _configPanelCategoryTextColor = SystemColors.ControlText;
            _configPanelHelpBackgroundColor = SystemColors.Control;
            _configPanelHelpTextColor = SystemColors.ControlText;
            _configPanelGridLineColor = SystemColors.InactiveBorder;

            if (themeName != null)
            {
                Name = themeName;
            }
        }
        #endregion

        #region Public Methods
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
		    foreach (System.Reflection.PropertyInfo propertyInfo in themeInfoType.GetProperties())
			{
				var myProperty = propertyInfo.GetValue(this, null);
				var otherProperty = propertyInfo.GetValue(otherTheme, null);
				if (!myProperty.Equals(otherProperty))
				{
					return false;
				}
			}
				
			return true;
		}

        // just fixing a complier warning. We don't use us so returning this value shouldn't be an issue at all.
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

	    private void NotifyPropertyChanged(string propertyName)
		{
		    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

	    #endregion
			
        #region Properties
        [Browsable(false)]
        public string Name
		{
			get { return _name; }
			set
			{
				if (_name == value)
				{
					return;
				}
				_name = value;
				NotifyPropertyChanged("Name");
			}
		}
			
        #region General
        [LocalizedAttributes.LocalizedCategory("strThemeCategoryGeneral"),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameWindowBackgroundColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionWindowBackgroundColor")]
        public Color WindowBackgroundColor
		{
			get { return (_windowBackgroundColor); }
			set
			{
				if (_windowBackgroundColor == value)
				{
					return;
				}
				_windowBackgroundColor = value;
				NotifyPropertyChanged("WindowBackgroundColor");
			}
		}
		
        [LocalizedAttributes.LocalizedCategory("strThemeCategoryGeneral"), Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameMenuBackgroundColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionMenuBackgroundColor")]
        public Color MenuBackgroundColor
		{
			get { return _menuBackgroundColor; }
			set
			{
				if (_menuBackgroundColor == value)
				{
					return;
				}
				_menuBackgroundColor = value;
				NotifyPropertyChanged("MenuBackgroundColor");
			}
		}
		
        [LocalizedAttributes.LocalizedCategory("strThemeCategoryGeneral"), Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameMenuTextColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionMenuTextColor")]
        public Color MenuTextColor
		{
			get { return _menuTextColor; }
			set
			{
				if (_menuTextColor == value)
				{
					return;
				}
				_menuTextColor = value;
				NotifyPropertyChanged("MenuTextColor");
			}
		}
		
        [LocalizedAttributes.LocalizedCategory("strThemeCategoryGeneral"),
            Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameToolbarBackgroundColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionToolbarBackgroundColor")]
        public Color ToolbarBackgroundColor
		{
			get { return _toolbarBackgroundColor; }
			set
			{
				if (_toolbarBackgroundColor == value || value.A < 255)
				{
					return;
				}
				_toolbarBackgroundColor = value;
				NotifyPropertyChanged("ToolbarBackgroundColor");
			}
		}
		
        [LocalizedAttributes.LocalizedCategory("strThemeCategoryGeneral"), Browsable(false),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameToolbarTextColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionToolbarTextColor")]
        public Color ToolbarTextColor
		{
			get { return _toolbarTextColor; }
			set
			{
				if (_toolbarTextColor == value)
				{
					return;
				}
				_toolbarTextColor = value;
				NotifyPropertyChanged("ToolbarTextColor");
			}
		}
        #endregion
			
        #region Connections Panel
        [LocalizedAttributes.LocalizedCategory("strThemeCategoryConnectionsPanel", 2),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameConnectionsPanelBackgroundColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionConnectionsPanelBackgroundColor")]
        public Color ConnectionsPanelBackgroundColor
		{
			get { return _connectionsPanelBackgroundColor; }
			set
			{
				if (_connectionsPanelBackgroundColor == value || value.A < 255)
				{
					return;
				}
				_connectionsPanelBackgroundColor = value;
				NotifyPropertyChanged("ConnectionsPanelBackgroundColor");
			}
		}
		
        [LocalizedAttributes.LocalizedCategory("strThemeCategoryConnectionsPanel", 2),
            LocalizedAttributes.LocalizedDisplayName("strThemeNameConnectionsPanelTextColor"),
            LocalizedAttributes.LocalizedDescription("strThemeDescriptionConnectionsPanelTextColor")]
        public Color ConnectionsPanelTextColor
		{
			get { return _connectionsPanelTextColor; }
			set
			{
				if (_connectionsPanelTextColor == value)
				{
					return;
				}
				_connectionsPanelTextColor = value;
				NotifyPropertyChanged("ConnectionsPanelTextColor");
			}
		}
		
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
					return;
				}
				_connectionsPanelTreeLineColor = value;
				NotifyPropertyChanged("ConnectionsPanelTreeLineColor");
			}
		}
		
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
					return;
				}
				_searchBoxBackgroundColor = value;
				NotifyPropertyChanged("SearchBoxBackgroundColor");
			}
		}
		
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
					return;
				}
				_searchBoxTextPromptColor = value;
				NotifyPropertyChanged("SearchBoxTextPromptColor");
			}
		}
		
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
					return;
				}
				_searchBoxTextColor = value;
				NotifyPropertyChanged("SearchBoxTextColor");
			}
		}
        #endregion
			
        #region Config Panel
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
					return;
				}
				_configPanelBackgroundColor = value;
				NotifyPropertyChanged("ConfigPanelBackgroundColor");
			}
		}
		
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
					return;
				}
				_configPanelTextColor = value;
				NotifyPropertyChanged("ConfigPanelTextColor");
			}
		}
		
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
					return;
				}
				_configPanelCategoryTextColor = value;
				NotifyPropertyChanged("ConfigPanelCategoryTextColor");
			}
		}
		
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
					return;
				}
				_configPanelHelpBackgroundColor = value;
				NotifyPropertyChanged("ConfigPanelHelpBackgroundColor");
			}
		}
		
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
					return;
				}
				_configPanelHelpTextColor = value;
				NotifyPropertyChanged("ConfigPanelHelpTextColor");
			}
		}
		
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
					return;
				}
				_configPanelGridLineColor = value;
				NotifyPropertyChanged("ConfigPanelGridLineColor");
			}
		}
        #endregion
        #endregion
	}
}