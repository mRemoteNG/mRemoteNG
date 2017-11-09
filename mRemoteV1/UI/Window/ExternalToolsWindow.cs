using System.Collections.Generic;
using System;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Config.Settings;
using mRemoteNG.Tools;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.UI.Forms;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Window
{
	public partial class ExternalToolsWindow
	{
        private readonly ExternalAppsSaver _externalAppsSaver;
        private ExternalTool _selectedTool;
        private ThemeManager _themeManager;

        public ExternalToolsWindow()
		{
			InitializeComponent(); 
			WindowType = WindowType.ExternalApps;
			DockPnl = new DockContent();
            _themeManager = ThemeManager.getInstance();
            _themeManager.ThemeChanged += ApplyTheme;
            _externalAppsSaver = new ExternalAppsSaver();
		}

        

        #region Private Methods
        #region Event Handlers
        private void ExternalTools_Load(object sender, EventArgs e)
		{
			ApplyLanguage();
            ApplyTheme();
            UpdateToolsListObjView();
		}

        private void ExternalTools_FormClosed(object sender, FormClosedEventArgs e)
		{
            _externalAppsSaver.Save(Runtime.ExternalToolsService.ExternalTools);
		}

        private void NewTool_Click(object sender, EventArgs e)
		{
			try
			{
				var externalTool = new ExternalTool(Language.strExternalToolDefaultName);
				Runtime.ExternalToolsService.ExternalTools.Add(externalTool);
				UpdateToolsListObjView(externalTool);
				DisplayNameTextBox.Focus();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.NewTool_Click() failed.", ex);
			}
		}

        private void DeleteTool_Click(object sender, EventArgs e)
		{
			try
			{
				string message;
				if (ToolsListObjView.SelectedItems.Count == 1)
				{
					message = string.Format(Language.strConfirmDeleteExternalTool, ToolsListObjView.SelectedItems[0].Text);
				}
				else if (ToolsListObjView.SelectedItems.Count > 1)
				{
					message = string.Format(Language.strConfirmDeleteExternalToolMultiple, ToolsListObjView.SelectedItems.Count);
				}
				else
				{
					return;
				}
				
				if (MessageBox.Show(FrmMain.Default, message, "Question?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
				{
					return;
				}
						
				foreach (Object toDeleteObj in ToolsListObjView.SelectedObjects)
				{
					var externalTool = toDeleteObj as ExternalTool;
					if (externalTool == null) continue;							
					Runtime.ExternalToolsService.ExternalTools.Remove(externalTool);
				}
                UpdateToolsListObjView();
            }
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.DeleteTool_Click() failed.", ex);
			}
		}

        private void LaunchTool_Click(object sender, EventArgs e)
		{
			LaunchTool();
		}

        private void ToolsListObjView_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (ToolsListObjView.SelectedItems.Count == 1)
				{
					PropertiesGroupBox.Enabled = true;
                    _selectedTool = ToolsListObjView.SelectedObjects[0] as ExternalTool;
					if (_selectedTool == null)
					{
						return;
					}
							
					DisplayNameTextBox.Text = _selectedTool.DisplayName;
					FilenameTextBox.Text = _selectedTool.FileName;
					ArgumentsCheckBox.Text = _selectedTool.Arguments;
					WaitForExitCheckBox.Checked = _selectedTool.WaitForExit;
					TryToIntegrateCheckBox.Checked = _selectedTool.TryIntegrate;
				}
				else
				{
					PropertiesGroupBox.Enabled = false;
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.ToolsListObjView_SelectedIndexChanged() failed.", ex);
			}
		}

        private void ToolsListObjView_DoubleClick(object sender, EventArgs e)
		{
			if (ToolsListObjView.SelectedItems.Count > 0)
			{
				LaunchTool();
			}
		}

        private void PropertyControl_ChangedOrLostFocus(object sender, EventArgs e)
		{
			if (_selectedTool == null)
			{
				return;
			}
					
			try
			{
				_selectedTool.DisplayName = DisplayNameTextBox.Text;
				_selectedTool.FileName = FilenameTextBox.Text;
				_selectedTool.Arguments = ArgumentsCheckBox.Text;
				_selectedTool.WaitForExit = WaitForExitCheckBox.Checked;
				_selectedTool.TryIntegrate = TryToIntegrateCheckBox.Checked;
						
				UpdateToolsListObjView();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.PropertyControl_ChangedOrLostFocus() failed.", ex);
			}
		}

        private void BrowseButton_Click(object sender, EventArgs e)
		{
			try
			{
				using (var browseDialog = new OpenFileDialog())
				{
					browseDialog.Filter = string.Join("|", new string[] {Language.strFilterApplication, "*.exe", Language.strFilterAll, "*.*"});
					if (browseDialog.ShowDialog() == DialogResult.OK)
					{
						FilenameTextBox.Text = browseDialog.FileName;
					}
				}
						
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.BrowseButton_Click() failed.", ex);
			}
		}

        private void TryToIntegrateCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (TryToIntegrateCheckBox.Checked)
			{
				WaitForExitCheckBox.Enabled = false;
				WaitForExitCheckBox.Checked = false;
			}
			else
			{
				WaitForExitCheckBox.Enabled = true;
			}
		}
        #endregion
				
		private void ApplyLanguage()
		{
			Text = Language.strMenuExternalTools;
			TabText = Language.strMenuExternalTools;
					
			NewToolToolstripButton.Text = Language.strButtonNew;
			DeleteToolToolstripButton.Text = Language.strOptionsKeyboardButtonDelete;
			LaunchToolToolstripButton.Text = Language.strButtonLaunch;
					
			DisplayNameColumnHeader.Text = Language.strColumnDisplayName;
			FilenameColumnHeader.Text = Language.strColumnFilename;
			ArgumentsColumnHeader.Text = Language.strColumnArguments;
			WaitForExitColumnHeader.Text = Language.strColumnWaitForExit;
			TryToIntegrateCheckBox.Text = Language.strTryIntegrate;
					
			PropertiesGroupBox.Text = Language.strGroupboxExternalToolProperties;
					
			DisplayNameLabel.Text = Language.strLabelDisplayName;
			FilenameLabel.Text = Language.strLabelFilename;
			ArgumentsLabel.Text = Language.strLabelArguments;
			OptionsLabel.Text = Language.strLabelOptions;
			WaitForExitCheckBox.Text = Language.strCheckboxWaitForExit;
			BrowseButton.Text = Language.strButtonBrowse;
					
			NewToolMenuItem.Text = Language.strMenuNewExternalTool;
			DeleteToolMenuItem.Text = Language.strMenuDeleteExternalTool;
			LaunchToolMenuItem.Text = Language.strMenuLaunchExternalTool;
		}

        private new void ApplyTheme()
        {
            if (!_themeManager.ThemingActive) return;
            vsToolStripExtender.SetStyle(ToolStrip, _themeManager.ActiveTheme.Version, _themeManager.ActiveTheme.Theme);
            vsToolStripExtender.SetStyle(ToolsContextMenuStrip, _themeManager.ActiveTheme.Version, _themeManager.ActiveTheme.Theme);
            //Apply the extended palette

            ToolStripContainer.TopToolStripPanel.BackColor = _themeManager.ActiveTheme.Theme.ColorPalette.CommandBarMenuDefault.Background;
            ToolStripContainer.TopToolStripPanel.ForeColor = _themeManager.ActiveTheme.Theme.ColorPalette.CommandBarMenuDefault.Text;
            PropertiesGroupBox.BackColor = _themeManager.ActiveTheme.Theme.ColorPalette.CommandBarMenuDefault.Background;
            PropertiesGroupBox.ForeColor = _themeManager.ActiveTheme.Theme.ColorPalette.CommandBarMenuDefault.Text;
            //Toollist grouping
            ToolsListObjView.AlwaysGroupByColumn = this.FilenameColumnHeader;
        }

        private void UpdateToolsListObjView(ExternalTool selectTool = null)
        {
            var selectedTools = new List<ExternalTool>();
            try
			{
			    if (selectTool == null)
				{
					foreach (var listViewItem in ToolsListObjView.SelectedObjects)
					{
						var externalTool = listViewItem as ExternalTool;
						if (externalTool != null)
						{
							selectedTools.Add(externalTool);
						}
					}
				}
				else
				{
					selectedTools.Add(selectTool);
				}
						
				ToolsListObjView.BeginUpdate();
				ToolsListObjView.Items.Clear(); 
                ToolsListObjView.SetObjects(Runtime.ExternalToolsService.ExternalTools);
                ToolsListObjView.EndUpdate();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.PopulateToolsListObjView()", ex);
			}
        }
				
		private void LaunchTool()
		{
            try
            {
                foreach (Object listViewObject in ToolsListObjView.SelectedObjects)
				{
					
                    ((ExternalTool)listViewObject).Start();
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.LaunchTool() failed.", ex);
			}
		}
        #endregion
	}
}
 
 
 