using System.Collections.Generic;
using System;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using mRemoteNG.App;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.My;


namespace mRemoteNG.UI.Window
{
	public partial class ExternalToolsWindow : BaseWindow
	{
        #region Constructors
		public ExternalToolsWindow(DockContent panel)
		{
			InitializeComponent();
					
			WindowType = WindowType.ExternalApps;
			DockPnl = panel;
		}
        #endregion
				
        #region Private Fields
		private Tools.ExternalTool _selectedTool = null;
        #endregion
				
        #region Private Methods
        #region Event Handlers
		public void ExternalTools_Load(object sender, EventArgs e)
		{
			ApplyLanguage();
			UpdateToolsListView();
		}
				
		static public void ExternalTools_FormClosed(System.Object sender, FormClosedEventArgs e)
		{
			mRemoteNG.Config.Settings.SettingsSaver.SaveExternalAppsToXML();
		}
				
		public void NewTool_Click(System.Object sender, EventArgs e)
		{
			try
			{
				Tools.ExternalTool externalTool = new Tools.ExternalTool(Language.strExternalToolDefaultName);
				Runtime.ExternalTools.Add(externalTool);
				UpdateToolsListView(externalTool);
				DisplayNameTextBox.Focus();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage(message: "UI.Window.ExternalTools.NewTool_Click() failed.", ex: ex, logOnly: true);
			}
		}
				
		public void DeleteTool_Click(System.Object sender, EventArgs e)
		{
			try
			{
				string message = "";
				if (ToolsListView.SelectedItems.Count == 1)
				{
					message = string.Format(Language.strConfirmDeleteExternalTool, ToolsListView.SelectedItems[0].Text);
				}
				else if (ToolsListView.SelectedItems.Count > 1)
				{
					message = string.Format(Language.strConfirmDeleteExternalToolMultiple, ToolsListView.SelectedItems.Count);
				}
				else
				{
					return ;
				}
						
				if (!(Interaction.MsgBox(message, (Microsoft.VisualBasic.MsgBoxStyle) (MsgBoxStyle.Question | MsgBoxStyle.YesNo), null) == MsgBoxResult.Yes))
				{
					return ;
				}
						
				foreach (ListViewItem listViewItem in ToolsListView.SelectedItems)
				{
					Tools.ExternalTool externalTool = listViewItem.Tag as Tools.ExternalTool;
					if (externalTool == null)
					{
						continue;
					}
							
					Runtime.ExternalTools.Remove(listViewItem.Tag);
					listViewItem.Remove();
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage(message: "UI.Window.ExternalTools.DeleteTool_Click() failed.", ex: ex, logOnly: true);
			}
		}
				
		public void LaunchTool_Click(System.Object sender, EventArgs e)
		{
			LaunchTool();
		}
				
		public void ToolsListView_SelectedIndexChanged(System.Object sender, EventArgs e)
		{
			try
			{
				if (ToolsListView.SelectedItems.Count == 1)
				{
					PropertiesGroupBox.Enabled = true;
					_selectedTool = ToolsListView.SelectedItems[0].Tag as Tools.ExternalTool;
					if (_selectedTool == null)
					{
						return ;
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
				Runtime.MessageCollector.AddExceptionMessage(message: "UI.Window.ExternalTools.ToolsListView_SelectedIndexChanged() failed.", ex: ex, logOnly: true);
			}
		}
				
		public void ToolsListView_DoubleClick(object sender, EventArgs e)
		{
			if (ToolsListView.SelectedItems.Count > 0)
			{
				LaunchTool();
			}
		}
				
		public void PropertyControl_ChangedOrLostFocus(object sender, EventArgs e)
		{
			if (_selectedTool == null)
			{
				return ;
			}
					
			try
			{
				_selectedTool.DisplayName = DisplayNameTextBox.Text;
				_selectedTool.FileName = FilenameTextBox.Text;
				_selectedTool.Arguments = ArgumentsCheckBox.Text;
				_selectedTool.WaitForExit = WaitForExitCheckBox.Checked;
				_selectedTool.TryIntegrate = TryToIntegrateCheckBox.Checked;
						
				UpdateToolsListView();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage(message: "UI.Window.ExternalTools.PropertyControl_ChangedOrLostFocus() failed.", ex: ex, logOnly: true);
			}
		}
				
		public void BrowseButton_Click(System.Object sender, EventArgs e)
		{
			try
			{
				using (OpenFileDialog browseDialog = new OpenFileDialog())
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
				Runtime.MessageCollector.AddExceptionMessage(message: "UI.Window.ExternalTools.BrowseButton_Click() failed.", ex: ex, logOnly: true);
			}
		}
				
		public void TryToIntegrateCheckBox_CheckedChanged(System.Object sender, EventArgs e)
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
				
		private void UpdateToolsListView(Tools.ExternalTool selectTool = null)
		{
			try
			{
				List<Tools.ExternalTool> selectedTools = new List<Tools.ExternalTool>();
				if (selectTool == null)
				{
					foreach (ListViewItem listViewItem in ToolsListView.SelectedItems)
					{
						Tools.ExternalTool externalTool = listViewItem.Tag as Tools.ExternalTool;
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
						
				ToolsListView.BeginUpdate();
				ToolsListView.Items.Clear();
						
				foreach (Tools.ExternalTool externalTool in Runtime.ExternalTools)
				{
					ListViewItem listViewItem = new ListViewItem();
					listViewItem.Text = externalTool.DisplayName;
					listViewItem.SubItems.Add(externalTool.FileName);
					listViewItem.SubItems.Add(externalTool.Arguments);
					listViewItem.SubItems.Add(externalTool.WaitForExit.ToString());
					listViewItem.SubItems.Add(externalTool.TryIntegrate.ToString());
					listViewItem.Tag = externalTool;
							
					ToolsListView.Items.Add(listViewItem);
							
					if (selectedTools.Contains(externalTool))
					{
						listViewItem.Selected = true;
					}
				}
						
				ToolsListView.EndUpdate();
						
				frmMain.Default.AddExternalToolsToToolBar();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage(message: "UI.Window.ExternalTools.PopulateToolsListView()", ex: ex, logOnly: true);
			}
		}
				
		private void LaunchTool()
		{
			try
			{
				foreach (ListViewItem listViewItem in ToolsListView.SelectedItems)
				{
					Tools.ExternalTool externalTool = listViewItem.Tag as Tools.ExternalTool;
					if (externalTool == null)
					{
						continue;
					}
							
					externalTool.Start();
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage(message: "UI.Window.ExternalTools.LaunchTool() failed.", ex: ex, logOnly: true);
			}
		}
        #endregion
	}
}
