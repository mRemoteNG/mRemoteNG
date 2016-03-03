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
using mRemoteNG.My;
using mRemoteNG.Themes;


namespace mRemoteNG.Forms.OptionsPages
{
	public partial class ThemePage
		{
			public ThemePage()
			{
				InitializeComponent();
			}
public override string PageName
			{
				get
				{
					return Language.strOptionsTabTheme;
				}
				set
				{
				}
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
			
#region Private Fields
			private BindingList<ThemeInfo> _themeList;
			private ThemeInfo _originalTheme;
#endregion
			
#region Private Methods
#region Event Handlers
			public void cboTheme_DropDown(object sender, EventArgs e)
			{
				if (ThemeManager.ActiveTheme == ThemeManager.DefaultTheme)
				{
					return ;
				}
				ThemeManager.ActiveTheme.Name = cboTheme.Text;
			}
			
			public void cboTheme_SelectionChangeCommitted(object sender, EventArgs e)
			{
				if (cboTheme.SelectedItem == null)
				{
					cboTheme.SelectedItem = ThemeManager.DefaultTheme;
				}
				
				if (cboTheme.SelectedItem == ThemeManager.DefaultTheme)
				{
					cboTheme.DropDownStyle = ComboBoxStyle.DropDownList;
					btnThemeDelete.Enabled = false;
					ThemePropertyGrid.Enabled = false;
				}
				else
				{
					cboTheme.DropDownStyle = ComboBoxStyle.DropDown;
					btnThemeDelete.Enabled = true;
					ThemePropertyGrid.Enabled = true;
				}
				
				ThemeManager.ActiveTheme = cboTheme.SelectedItem;
				ThemePropertyGrid.SelectedObject = ThemeManager.ActiveTheme;
				ThemePropertyGrid.Refresh();
			}
			
			public void btnThemeNew_Click(object sender, EventArgs e)
			{
				ThemeInfo newTheme = ThemeManager.ActiveTheme.Clone();
				newTheme.Name = Language.strUnnamedTheme;
				
				_themeList.Add(newTheme);
				
				cboTheme.SelectedItem = newTheme;
				cboTheme_SelectionChangeCommitted(this, new EventArgs());
				
				cboTheme.Focus();
			}
			
			public void btnThemeDelete_Click(object sender, EventArgs e)
			{
				ThemeInfo theme = cboTheme.SelectedItem;
				if (theme == null)
				{
					return ;
				}
				
				_themeList.Remove(theme);
				
				cboTheme.SelectedItem = ThemeManager.DefaultTheme;
				cboTheme_SelectionChangeCommitted(this, new EventArgs());
			}
#endregion
#endregion
		}
}
