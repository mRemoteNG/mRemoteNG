using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.App;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.App.Runtime;
using mRemoteNG.My;

namespace mRemoteNG.UI
{
	namespace Window
	{
		public partial class ExternalTools : Base
		{
			#region "Constructors"
			public ExternalTools(DockContent panel)
			{
				FormClosed += ExternalTools_FormClosed;
				Load += ExternalTools_Load;
				InitializeComponent();

				WindowType = Type.ExternalApps;
				DockPnl = panel;
			}
			#endregion

			#region "Private Fields"
				#endregion
			private Tools.ExternalTool _selectedTool = null;

			#region "Private Methods"
			#region "Event Handlers"
			private void ExternalTools_Load(object sender, EventArgs e)
			{
				ApplyLanguage();
				UpdateToolsListView();
			}

			private static void ExternalTools_FormClosed(System.Object sender, FormClosedEventArgs e)
			{
				mRemoteNG.Config.Settings.Save.SaveExternalAppsToXML();
			}

			private void NewTool_Click(System.Object sender, EventArgs e)
			{
				try {
					Tools.ExternalTool externalTool = new Tools.ExternalTool(Language.strExternalToolDefaultName);
					Runtime.ExternalTools.Add(externalTool);
					UpdateToolsListView(externalTool);
					DisplayNameTextBox.Focus();
				} catch (Exception ex) {
					Runtime.MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.NewTool_Click() failed.", ex, , true);
				}
			}

			private void DeleteTool_Click(System.Object sender, EventArgs e)
			{
				try {
					string message = null;
					switch (ToolsListView.SelectedItems.Count) {
						case  // ERROR: Case labels with binary operators are unsupported : Equality
1:
							message = string.Format(Language.strConfirmDeleteExternalTool, ToolsListView.SelectedItems[0].Text);
							break;
						case  // ERROR: Case labels with binary operators are unsupported : GreaterThan
1:
							message = string.Format(Language.strConfirmDeleteExternalToolMultiple, ToolsListView.SelectedItems.Count);
							break;
						default:
							return;
					}

					if (!(Interaction.MsgBox(message, MsgBoxStyle.Question | MsgBoxStyle.YesNo) == MsgBoxResult.Yes))
						return;

					foreach (ListViewItem listViewItem in ToolsListView.SelectedItems) {
						Tools.ExternalTool externalTool = listViewItem.Tag as Tools.ExternalTool;
						if (externalTool == null)
							continue;

						Runtime.ExternalTools.Remove(listViewItem.Tag);
						listViewItem.Remove();
					}
				} catch (Exception ex) {
					Runtime.MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.DeleteTool_Click() failed.", ex, , true);
				}
			}

			private void LaunchTool_Click(System.Object sender, EventArgs e)
			{
				LaunchTool();
			}

			private void ToolsListView_SelectedIndexChanged(System.Object sender, EventArgs e)
			{
				try {
					if (ToolsListView.SelectedItems.Count == 1) {
						PropertiesGroupBox.Enabled = true;
						_selectedTool = ToolsListView.SelectedItems[0].Tag as Tools.ExternalTool;
						if (_selectedTool == null)
							return;

						var _with1 = _selectedTool;
						DisplayNameTextBox.Text = _with1.DisplayName;
						FilenameTextBox.Text = _with1.FileName;
						ArgumentsCheckBox.Text = _with1.Arguments;
						WaitForExitCheckBox.Checked = _with1.WaitForExit;
						TryToIntegrateCheckBox.Checked = _with1.TryIntegrate;
					} else {
						PropertiesGroupBox.Enabled = false;
					}
				} catch (Exception ex) {
					Runtime.MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.ToolsListView_SelectedIndexChanged() failed.", ex, , true);
				}
			}

			private void ToolsListView_DoubleClick(object sender, EventArgs e)
			{
				if (ToolsListView.SelectedItems.Count > 0)
					LaunchTool();
			}

			private void PropertyControl_ChangedOrLostFocus(object sender, EventArgs e)
			{
				if (_selectedTool == null)
					return;

				try {
					var _with2 = _selectedTool;
					_with2.DisplayName = DisplayNameTextBox.Text;
					_with2.FileName = FilenameTextBox.Text;
					_with2.Arguments = ArgumentsCheckBox.Text;
					_with2.WaitForExit = WaitForExitCheckBox.Checked;
					_with2.TryIntegrate = TryToIntegrateCheckBox.Checked;

					UpdateToolsListView();
				} catch (Exception ex) {
					Runtime.MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.PropertyControl_ChangedOrLostFocus() failed.", ex, , true);
				}
			}

			private void BrowseButton_Click(System.Object sender, EventArgs e)
			{
				try {
					using (OpenFileDialog browseDialog = new OpenFileDialog()) {
						var _with3 = browseDialog;
						_with3.Filter = string.Join("|", new string[] {
							Language.strFilterApplication,
							"*.exe",
							Language.strFilterAll,
							"*.*"
						});
						if (_with3.ShowDialog == DialogResult.OK)
							FilenameTextBox.Text = _with3.FileName;
					}
				} catch (Exception ex) {
					Runtime.MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.BrowseButton_Click() failed.", ex, , true);
				}
			}

			private void TryToIntegrateCheckBox_CheckedChanged(System.Object sender, EventArgs e)
			{
				if (TryToIntegrateCheckBox.Checked) {
					WaitForExitCheckBox.Enabled = false;
					WaitForExitCheckBox.Checked = false;
				} else {
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
				try {
					List<Tools.ExternalTool> selectedTools = new List<Tools.ExternalTool>();
					if (selectTool == null) {
						foreach (ListViewItem listViewItem in ToolsListView.SelectedItems) {
							Tools.ExternalTool externalTool = listViewItem.Tag as Tools.ExternalTool;
							if (externalTool != null)
								selectedTools.Add(externalTool);
						}
					} else {
						selectedTools.Add(selectTool);
					}

					ToolsListView.BeginUpdate();
					ToolsListView.Items.Clear();

					foreach (Tools.ExternalTool externalTool in Runtime.ExternalTools) {
						ListViewItem listViewItem = new ListViewItem();
						var _with4 = listViewItem;
						_with4.Text = externalTool.DisplayName;
						_with4.SubItems.Add(externalTool.FileName);
						_with4.SubItems.Add(externalTool.Arguments);
						_with4.SubItems.Add(externalTool.WaitForExit);
						_with4.SubItems.Add(externalTool.TryIntegrate);
						_with4.Tag = externalTool;

						ToolsListView.Items.Add(listViewItem);

						if (selectedTools.Contains(externalTool))
							listViewItem.Selected = true;
					}

					ToolsListView.EndUpdate();

					My.MyProject.Forms.frmMain.AddExternalToolsToToolBar();
				} catch (Exception ex) {
					Runtime.MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.PopulateToolsListView()", ex, , true);
				}
			}

			private void LaunchTool()
			{
				try {
					foreach (ListViewItem listViewItem in ToolsListView.SelectedItems) {
						Tools.ExternalTool externalTool = listViewItem.Tag as Tools.ExternalTool;
						if (externalTool == null)
							continue;

						externalTool.Start();
					}
				} catch (Exception ex) {
					Runtime.MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.LaunchTool() failed.", ex, , true);
				}
			}
			#endregion
		}
	}
}
