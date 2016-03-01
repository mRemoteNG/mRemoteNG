using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using mRemoteNG.My;
using mRemoteNG.Themes;

namespace mRemoteNG.Forms.OptionsPages
{
	public partial class ThemePage
	{
		public override string PageName {
			get { return Language.strOptionsTabTheme; }
			set { }
		}

		public override void ApplyLanguage()
		{
			base.ApplyLanguage();

			btnThemeDelete.Text = Language.strOptionsThemeButtonDelete;
			btnThemeNew.Text = Language.strOptionsThemeButtonNew;
		}

		public override void LoadSettings()
		{
			base.SaveSettings();

			_themeList = new BindingList<ThemeInfo>(ThemeManager.LoadThemes());
			cboTheme.DataSource = _themeList;
			cboTheme.SelectedItem = ThemeManager.ActiveTheme;
			cboTheme_SelectionChangeCommitted(this, new EventArgs());

			ThemePropertyGrid.PropertySort = PropertySort.Categorized;

			_originalTheme = ThemeManager.ActiveTheme;
		}

		public override void SaveSettings()
		{
			base.SaveSettings();

			ThemeManager.SaveThemes(_themeList);
			Settings.ThemeName = ThemeManager.ActiveTheme.Name;
		}

		public override void RevertSettings()
		{
			ThemeManager.ActiveTheme = _originalTheme;
		}

		#region "Private Fields"
		private BindingList<ThemeInfo> _themeList;
			#endregion
		private ThemeInfo _originalTheme;

		#region "Private Methods"
		#region "Event Handlers"
		private void cboTheme_DropDown(object sender, EventArgs e)
		{
			if (object.ReferenceEquals(ThemeManager.ActiveTheme, ThemeManager.DefaultTheme))
				return;
			ThemeManager.ActiveTheme.Name = cboTheme.Text;
		}

		private void cboTheme_SelectionChangeCommitted(object sender, EventArgs e)
		{
			if (cboTheme.SelectedItem == null)
				cboTheme.SelectedItem = ThemeManager.DefaultTheme;

			if (object.ReferenceEquals(cboTheme.SelectedItem, ThemeManager.DefaultTheme)) {
				cboTheme.DropDownStyle = ComboBoxStyle.DropDownList;
				btnThemeDelete.Enabled = false;
				ThemePropertyGrid.Enabled = false;
			} else {
				cboTheme.DropDownStyle = ComboBoxStyle.DropDown;
				btnThemeDelete.Enabled = true;
				ThemePropertyGrid.Enabled = true;
			}

			ThemeManager.ActiveTheme = cboTheme.SelectedItem;
			ThemePropertyGrid.SelectedObject = ThemeManager.ActiveTheme;
			ThemePropertyGrid.Refresh();
		}

		private void btnThemeNew_Click(object sender, EventArgs e)
		{
			ThemeInfo newTheme = ThemeManager.ActiveTheme.Clone();
			newTheme.Name = Language.strUnnamedTheme;

			_themeList.Add(newTheme);

			cboTheme.SelectedItem = newTheme;
			cboTheme_SelectionChangeCommitted(this, new EventArgs());

			cboTheme.Focus();
		}

		private void btnThemeDelete_Click(object sender, EventArgs e)
		{
			ThemeInfo theme = cboTheme.SelectedItem;
			if (theme == null)
				return;

			_themeList.Remove(theme);

			cboTheme.SelectedItem = ThemeManager.DefaultTheme;
			cboTheme_SelectionChangeCommitted(this, new EventArgs());
		}
		public ThemePage()
		{
			InitializeComponent();
		}
		#endregion
		#endregion
	}
}
