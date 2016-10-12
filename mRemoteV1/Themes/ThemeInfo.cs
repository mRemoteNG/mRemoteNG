using System;
using System.ComponentModel;
using System.Drawing;
using mRemoteNG.Tools;

namespace mRemoteNG.Themes
{
    public class ThemeInfo : ICloneable, INotifyPropertyChanged
    {
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
                Name = themeName;
        }

        #endregion

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
            var otherTheme = obj as ThemeInfo;
            if (otherTheme == null)
                return false;

            var themeInfoType = new ThemeInfo().GetType();
            foreach (var propertyInfo in themeInfoType.GetProperties())
            {
                var myProperty = propertyInfo.GetValue(this, null);
                var otherProperty = propertyInfo.GetValue(otherTheme, null);
                if (!myProperty.Equals(otherProperty))
                    return false;
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
                    return;
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        #region General

        [LocalizedAttributes.LocalizedCategoryAttribute("strThemeCategoryGeneral")]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strThemeNameWindowBackgroundColor")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strThemeDescriptionWindowBackgroundColor")]
        public Color WindowBackgroundColor
        {
            get { return _windowBackgroundColor; }
            set
            {
                if (_windowBackgroundColor == value)
                    return;
                _windowBackgroundColor = value;
                NotifyPropertyChanged("WindowBackgroundColor");
            }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strThemeCategoryGeneral")]
        [Browsable(false)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strThemeNameMenuBackgroundColor")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strThemeDescriptionMenuBackgroundColor")]
        public Color MenuBackgroundColor
        {
            get { return _menuBackgroundColor; }
            set
            {
                if (_menuBackgroundColor == value)
                    return;
                _menuBackgroundColor = value;
                NotifyPropertyChanged("MenuBackgroundColor");
            }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strThemeCategoryGeneral")]
        [Browsable(false)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strThemeNameMenuTextColor")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strThemeDescriptionMenuTextColor")]
        public Color MenuTextColor
        {
            get { return _menuTextColor; }
            set
            {
                if (_menuTextColor == value)
                    return;
                _menuTextColor = value;
                NotifyPropertyChanged("MenuTextColor");
            }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strThemeCategoryGeneral")]
        [Browsable(false)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strThemeNameToolbarBackgroundColor")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strThemeDescriptionToolbarBackgroundColor")]
        public Color ToolbarBackgroundColor
        {
            get { return _toolbarBackgroundColor; }
            set
            {
                if ((_toolbarBackgroundColor == value) || (value.A < 255))
                    return;
                _toolbarBackgroundColor = value;
                NotifyPropertyChanged("ToolbarBackgroundColor");
            }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strThemeCategoryGeneral")]
        [Browsable(false)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strThemeNameToolbarTextColor")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strThemeDescriptionToolbarTextColor")]
        public Color ToolbarTextColor
        {
            get { return _toolbarTextColor; }
            set
            {
                if (_toolbarTextColor == value)
                    return;
                _toolbarTextColor = value;
                NotifyPropertyChanged("ToolbarTextColor");
            }
        }

        #endregion

        #region Connections Panel

        [LocalizedAttributes.LocalizedCategoryAttribute("strThemeCategoryConnectionsPanel", 2)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strThemeNameConnectionsPanelBackgroundColor")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strThemeDescriptionConnectionsPanelBackgroundColor")]
        public Color ConnectionsPanelBackgroundColor
        {
            get { return _connectionsPanelBackgroundColor; }
            set
            {
                if ((_connectionsPanelBackgroundColor == value) || (value.A < 255))
                    return;
                _connectionsPanelBackgroundColor = value;
                NotifyPropertyChanged("ConnectionsPanelBackgroundColor");
            }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strThemeCategoryConnectionsPanel", 2)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strThemeNameConnectionsPanelTextColor")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strThemeDescriptionConnectionsPanelTextColor")]
        public Color ConnectionsPanelTextColor
        {
            get { return _connectionsPanelTextColor; }
            set
            {
                if (_connectionsPanelTextColor == value)
                    return;
                _connectionsPanelTextColor = value;
                NotifyPropertyChanged("ConnectionsPanelTextColor");
            }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strThemeCategoryConnectionsPanel", 2)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strThemeNameConnectionsPanelTreeLineColor")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strThemeDescriptionConnectionsPanelTreeLineColor")]
        public Color ConnectionsPanelTreeLineColor
        {
            get { return _connectionsPanelTreeLineColor; }
            set
            {
                if (_connectionsPanelTreeLineColor == value)
                    return;
                _connectionsPanelTreeLineColor = value;
                NotifyPropertyChanged("ConnectionsPanelTreeLineColor");
            }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strThemeCategoryConnectionsPanel", 2)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strThemeNameSearchBoxBackgroundColor")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strThemeDescriptionSearchBoxBackgroundColor")]
        public Color SearchBoxBackgroundColor
        {
            get { return _searchBoxBackgroundColor; }
            set
            {
                if ((_searchBoxBackgroundColor == value) || (value.A < 255))
                    return;
                _searchBoxBackgroundColor = value;
                NotifyPropertyChanged("SearchBoxBackgroundColor");
            }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strThemeCategoryConnectionsPanel", 2)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strThemeNameSearchBoxTextPromptColor")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strThemeDescriptionSearchBoxTextPromptColor")]
        public Color SearchBoxTextPromptColor
        {
            get { return _searchBoxTextPromptColor; }
            set
            {
                if (_searchBoxTextPromptColor == value)
                    return;
                _searchBoxTextPromptColor = value;
                NotifyPropertyChanged("SearchBoxTextPromptColor");
            }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strThemeCategoryConnectionsPanel", 2)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strThemeNameSearchBoxTextColor")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strThemeDescriptionSearchBoxTextColor")]
        public Color SearchBoxTextColor
        {
            get { return _searchBoxTextColor; }
            set
            {
                if (_searchBoxTextColor == value)
                    return;
                _searchBoxTextColor = value;
                NotifyPropertyChanged("SearchBoxTextColor");
            }
        }

        #endregion

        #region Config Panel

        [LocalizedAttributes.LocalizedCategoryAttribute("strThemeCategoryConfigPanel", 3)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strThemeNameConfigPanelBackgroundColor")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strThemeDescriptionConfigPanelBackgroundColor")]
        public Color ConfigPanelBackgroundColor
        {
            get { return _configPanelBackgroundColor; }
            set
            {
                if ((_configPanelBackgroundColor == value) || (value.A < 255))
                    return;
                _configPanelBackgroundColor = value;
                NotifyPropertyChanged("ConfigPanelBackgroundColor");
            }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strThemeCategoryConfigPanel", 3)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strThemeNameConfigPanelTextColor")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strThemeDescriptionConfigPanelTextColor")]
        public Color ConfigPanelTextColor
        {
            get { return _configPanelTextColor; }
            set
            {
                if (_configPanelTextColor == value)
                    return;
                _configPanelTextColor = value;
                NotifyPropertyChanged("ConfigPanelTextColor");
            }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strThemeCategoryConfigPanel", 3)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strThemeNameConfigPanelCategoryTextColor")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strThemeDescriptionConfigPanelCategoryTextColor")]
        public Color ConfigPanelCategoryTextColor
        {
            get { return _configPanelCategoryTextColor; }
            set
            {
                if (_configPanelCategoryTextColor == value)
                    return;
                _configPanelCategoryTextColor = value;
                NotifyPropertyChanged("ConfigPanelCategoryTextColor");
            }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strThemeCategoryConfigPanel", 3)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strThemeNameConfigPanelHelpBackgroundColor")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strThemeDescriptionConfigPanelHelpBackgroundColor")]
        public Color ConfigPanelHelpBackgroundColor
        {
            get { return _configPanelHelpBackgroundColor; }
            set
            {
                if ((_configPanelHelpBackgroundColor == value) || (value.A < 255))
                    return;
                _configPanelHelpBackgroundColor = value;
                NotifyPropertyChanged("ConfigPanelHelpBackgroundColor");
            }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strThemeCategoryConfigPanel", 3)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strThemeNameConfigPanelHelpTextColor")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strThemeDescriptionConfigPanelHelpTextColor")]
        public Color ConfigPanelHelpTextColor
        {
            get { return _configPanelHelpTextColor; }
            set
            {
                if (_configPanelHelpTextColor == value)
                    return;
                _configPanelHelpTextColor = value;
                NotifyPropertyChanged("ConfigPanelHelpTextColor");
            }
        }

        [LocalizedAttributes.LocalizedCategoryAttribute("strThemeCategoryConfigPanel", 3)]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strThemeNameConfigPanelGridLineColor")]
        [LocalizedAttributes.LocalizedDescriptionAttribute("strThemeDescriptionConfigPanelGridLineColor")]
        public Color ConfigPanelGridLineColor
        {
            get { return _configPanelGridLineColor; }
            set
            {
                if (_configPanelGridLineColor == value)
                    return;
                _configPanelGridLineColor = value;
                NotifyPropertyChanged("ConfigPanelGridLineColor");
            }
        }

        #endregion

        #endregion
    }
}