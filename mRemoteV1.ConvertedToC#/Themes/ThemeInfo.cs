using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using mRemoteNG.Tools.LocalizedAttributes;
using mRemoteNG.My;

namespace mRemoteNG.Themes
{
	public class ThemeInfo : ICloneable, INotifyPropertyChanged
	{
		#region "Public Methods"
		public ThemeInfo(string themeName = null)
		{
			if (themeName != null)
				Name = themeName;
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
				return false;

			Type themeInfoType = (new ThemeInfo()).GetType();
			object myProperty = null;
			object otherProperty = null;
			foreach (System.Reflection.PropertyInfo propertyInfo in themeInfoType.GetProperties()) {
				myProperty = propertyInfo.GetValue(this, null);
				otherProperty = propertyInfo.GetValue(otherTheme, null);
				if (!myProperty.Equals(otherProperty))
					return false;
			}

			return true;
		}
		#endregion

		#region "Events"
		public event PropertyChangedEventHandler System.ComponentModel.INotifyPropertyChanged.PropertyChanged;
		public delegate void PropertyChangedEventHandler(object sender, System.ComponentModel.PropertyChangedEventArgs e);
		protected void NotifyPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion

		#region "Properties"
		private string _name = Language.strUnnamedTheme;
		[Browsable(false)]
		public string Name {
			get { return _name; }
			set {
				if (_name == value)
					return;
				_name = value;
				NotifyPropertyChanged("Name");
			}
		}

		#region "General"
		private Color _windowBackgroundColor = SystemColors.AppWorkspace;
		[LocalizedCategory("strThemeCategoryGeneral", 1), LocalizedDisplayName("strThemeNameWindowBackgroundColor"), LocalizedDescription("strThemeDescriptionWindowBackgroundColor")]
		public Color WindowBackgroundColor {
			get { return (_windowBackgroundColor); }
			set {
				if (_windowBackgroundColor == value)
					return;
				_windowBackgroundColor = value;
				NotifyPropertyChanged("WindowBackgroundColor");
			}
		}

		private Color _menuBackgroundColor = SystemColors.Control;
		[LocalizedCategory("strThemeCategoryGeneral", 1), Browsable(false), LocalizedDisplayName("strThemeNameMenuBackgroundColor"), LocalizedDescription("strThemeDescriptionMenuBackgroundColor")]
		public Color MenuBackgroundColor {
			get { return _menuBackgroundColor; }
			set {
				if (_menuBackgroundColor == value)
					return;
				_menuBackgroundColor = value;
				NotifyPropertyChanged("MenuBackgroundColor");
			}
		}

		private Color _menuTextColor = SystemColors.ControlText;
		[LocalizedCategory("strThemeCategoryGeneral", 1), Browsable(false), LocalizedDisplayName("strThemeNameMenuTextColor"), LocalizedDescription("strThemeDescriptionMenuTextColor")]
		public Color MenuTextColor {
			get { return _menuTextColor; }
			set {
				if (_menuTextColor == value)
					return;
				_menuTextColor = value;
				NotifyPropertyChanged("MenuTextColor");
			}
		}

		private Color _toolbarBackgroundColor = SystemColors.Control;
		[LocalizedCategory("strThemeCategoryGeneral", 1), Browsable(false), LocalizedDisplayName("strThemeNameToolbarBackgroundColor"), LocalizedDescription("strThemeDescriptionToolbarBackgroundColor")]
		public Color ToolbarBackgroundColor {
			get { return _toolbarBackgroundColor; }
			set {
				if (_toolbarBackgroundColor == value | value.A < 255)
					return;
				_toolbarBackgroundColor = value;
				NotifyPropertyChanged("ToolbarBackgroundColor");
			}
		}

		private Color _toolbarTextColor = SystemColors.ControlText;
		[LocalizedCategory("strThemeCategoryGeneral", 1), Browsable(false), LocalizedDisplayName("strThemeNameToolbarTextColor"), LocalizedDescription("strThemeDescriptionToolbarTextColor")]
		public Color ToolbarTextColor {
			get { return _toolbarTextColor; }
			set {
				if (_toolbarTextColor == value)
					return;
				_toolbarTextColor = value;
				NotifyPropertyChanged("ToolbarTextColor");
			}
		}
		#endregion

		#region "Connections Panel"
		private Color _connectionsPanelBackgroundColor = SystemColors.Window;
		[LocalizedCategory("strThemeCategoryConnectionsPanel", 2), LocalizedDisplayName("strThemeNameConnectionsPanelBackgroundColor"), LocalizedDescription("strThemeDescriptionConnectionsPanelBackgroundColor")]
		public Color ConnectionsPanelBackgroundColor {
			get { return _connectionsPanelBackgroundColor; }
			set {
				if (_connectionsPanelBackgroundColor == value | value.A < 255)
					return;
				_connectionsPanelBackgroundColor = value;
				NotifyPropertyChanged("ConnectionsPanelBackgroundColor");
			}
		}

		private Color _connectionsPanelTextColor = SystemColors.WindowText;
		[LocalizedCategory("strThemeCategoryConnectionsPanel", 2), LocalizedDisplayName("strThemeNameConnectionsPanelTextColor"), LocalizedDescription("strThemeDescriptionConnectionsPanelTextColor")]
		public Color ConnectionsPanelTextColor {
			get { return _connectionsPanelTextColor; }
			set {
				if (_connectionsPanelTextColor == value)
					return;
				_connectionsPanelTextColor = value;
				NotifyPropertyChanged("ConnectionsPanelTextColor");
			}
		}

		private Color _connectionsPanelTreeLineColor = Color.Black;
		[LocalizedCategory("strThemeCategoryConnectionsPanel", 2), LocalizedDisplayName("strThemeNameConnectionsPanelTreeLineColor"), LocalizedDescription("strThemeDescriptionConnectionsPanelTreeLineColor")]
		public Color ConnectionsPanelTreeLineColor {
			get { return _connectionsPanelTreeLineColor; }
			set {
				if (_connectionsPanelTreeLineColor == value)
					return;
				_connectionsPanelTreeLineColor = value;
				NotifyPropertyChanged("ConnectionsPanelTreeLineColor");
			}
		}

		private Color _searchBoxBackgroundColor = SystemColors.Window;
		[LocalizedCategory("strThemeCategoryConnectionsPanel", 2), LocalizedDisplayName("strThemeNameSearchBoxBackgroundColor"), LocalizedDescription("strThemeDescriptionSearchBoxBackgroundColor")]
		public Color SearchBoxBackgroundColor {
			get { return _searchBoxBackgroundColor; }
			set {
				if (_searchBoxBackgroundColor == value | value.A < 255)
					return;
				_searchBoxBackgroundColor = value;
				NotifyPropertyChanged("SearchBoxBackgroundColor");
			}
		}

		private Color _searchBoxTextPromptColor = SystemColors.GrayText;
		[LocalizedCategory("strThemeCategoryConnectionsPanel", 2), LocalizedDisplayName("strThemeNameSearchBoxTextPromptColor"), LocalizedDescription("strThemeDescriptionSearchBoxTextPromptColor")]
		public Color SearchBoxTextPromptColor {
			get { return _searchBoxTextPromptColor; }
			set {
				if (_searchBoxTextPromptColor == value)
					return;
				_searchBoxTextPromptColor = value;
				NotifyPropertyChanged("SearchBoxTextPromptColor");
			}
		}

		private Color _searchBoxTextColor = SystemColors.WindowText;
		[LocalizedCategory("strThemeCategoryConnectionsPanel", 2), LocalizedDisplayName("strThemeNameSearchBoxTextColor"), LocalizedDescription("strThemeDescriptionSearchBoxTextColor")]
		public Color SearchBoxTextColor {
			get { return _searchBoxTextColor; }
			set {
				if (_searchBoxTextColor == value)
					return;
				_searchBoxTextColor = value;
				NotifyPropertyChanged("SearchBoxTextColor");
			}
		}
		#endregion

		#region "Config Panel"
		private Color _configPanelBackgroundColor = SystemColors.Window;
		[LocalizedCategory("strThemeCategoryConfigPanel", 3), LocalizedDisplayName("strThemeNameConfigPanelBackgroundColor"), LocalizedDescription("strThemeDescriptionConfigPanelBackgroundColor")]
		public Color ConfigPanelBackgroundColor {
			get { return _configPanelBackgroundColor; }
			set {
				if (_configPanelBackgroundColor == value | value.A < 255)
					return;
				_configPanelBackgroundColor = value;
				NotifyPropertyChanged("ConfigPanelBackgroundColor");
			}
		}

		private Color _configPanelTextColor = SystemColors.WindowText;
		[LocalizedCategory("strThemeCategoryConfigPanel", 3), LocalizedDisplayName("strThemeNameConfigPanelTextColor"), LocalizedDescription("strThemeDescriptionConfigPanelTextColor")]
		public Color ConfigPanelTextColor {
			get { return _configPanelTextColor; }
			set {
				if (_configPanelTextColor == value)
					return;
				_configPanelTextColor = value;
				NotifyPropertyChanged("ConfigPanelTextColor");
			}
		}

		private Color _configPanelCategoryTextColor = SystemColors.ControlText;
		[LocalizedCategory("strThemeCategoryConfigPanel", 3), LocalizedDisplayName("strThemeNameConfigPanelCategoryTextColor"), LocalizedDescription("strThemeDescriptionConfigPanelCategoryTextColor")]
		public Color ConfigPanelCategoryTextColor {
			get { return _configPanelCategoryTextColor; }
			set {
				if (_configPanelCategoryTextColor == value)
					return;
				_configPanelCategoryTextColor = value;
				NotifyPropertyChanged("ConfigPanelCategoryTextColor");
			}
		}

		private Color _configPanelHelpBackgroundColor = SystemColors.Control;
		[LocalizedCategory("strThemeCategoryConfigPanel", 3), LocalizedDisplayName("strThemeNameConfigPanelHelpBackgroundColor"), LocalizedDescription("strThemeDescriptionConfigPanelHelpBackgroundColor")]
		public Color ConfigPanelHelpBackgroundColor {
			get { return _configPanelHelpBackgroundColor; }
			set {
				if (_configPanelHelpBackgroundColor == value | value.A < 255)
					return;
				_configPanelHelpBackgroundColor = value;
				NotifyPropertyChanged("ConfigPanelHelpBackgroundColor");
			}
		}

		private Color _configPanelHelpTextColor = SystemColors.ControlText;
		[LocalizedCategory("strThemeCategoryConfigPanel", 3), LocalizedDisplayName("strThemeNameConfigPanelHelpTextColor"), LocalizedDescription("strThemeDescriptionConfigPanelHelpTextColor")]
		public Color ConfigPanelHelpTextColor {
			get { return _configPanelHelpTextColor; }
			set {
				if (_configPanelHelpTextColor == value)
					return;
				_configPanelHelpTextColor = value;
				NotifyPropertyChanged("ConfigPanelHelpTextColor");
			}
		}

		private Color _configPanelGridLineColor = SystemColors.InactiveBorder;
		[LocalizedCategory("strThemeCategoryConfigPanel", 3), LocalizedDisplayName("strThemeNameConfigPanelGridLineColor"), LocalizedDescription("strThemeDescriptionConfigPanelGridLineColor")]
		public Color ConfigPanelGridLineColor {
			get { return _configPanelGridLineColor; }
			set {
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
