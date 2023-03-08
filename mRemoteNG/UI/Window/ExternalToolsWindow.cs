using System;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using mRemoteNG.App;
using mRemoteNG.Config.Settings;
using mRemoteNG.Tools;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.UI.Forms;
using mRemoteNG.Themes;
using mRemoteNG.Tools.CustomCollections;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Window
{
    [SupportedOSPlatform("windows")]
    public partial class ExternalToolsWindow
    {
        private readonly ExternalAppsSaver _externalAppsSaver;
        private readonly ThemeManager _themeManager;
        private readonly FullyObservableCollection<ExternalTool> _currentlySelectedExternalTools;

        public ExternalToolsWindow()
        {
            InitializeComponent();
            Icon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.Console_16x);
            WindowType = WindowType.ExternalApps;
            DockPnl = new DockContent();
            _themeManager = ThemeManager.getInstance();
            _themeManager.ThemeChanged += ApplyTheme;
            _externalAppsSaver = new ExternalAppsSaver();
            _currentlySelectedExternalTools = new FullyObservableCollection<ExternalTool>();
            _currentlySelectedExternalTools.CollectionUpdated += CurrentlySelectedExternalToolsOnCollectionUpdated;
            BrowseButton.Height = FilenameTextBox.Height;
            BrowseWorkingDir.Height = WorkingDirTextBox.Height;
        }


        #region Private Methods

        private void ExternalTools_Load(object sender, EventArgs e)
        {
            ApplyLanguage();
            ApplyTheme();
            UpdateToolsListObjView();
        }

        private void ApplyLanguage()
        {
            Text = Language.ExternalTool;
            TabText = Language.ExternalTool;

            NewToolToolstripButton.Text = Language._New;
            DeleteToolToolstripButton.Text = Language.Delete;
            LaunchToolToolstripButton.Text = Language._Launch;

            DisplayNameColumnHeader.Text = Language.DisplayName;
            FilenameColumnHeader.Text = Language.Filename;
            ArgumentsColumnHeader.Text = Language.Arguments;
            WorkingDirColumnHeader.Text = Language.WorkingDirColumnHeader;
            WaitForExitColumnHeader.Text = Language.WaitForExit;
            TryToIntegrateColumnHeader.Text = Language.TryToIntegrate;
            RunElevateHeader.Text = Language.RunElevated;
            ShowOnToolbarColumnHeader.Text = Language.ShowOnToolbarColumnHeader;

            TryToIntegrateCheckBox.Text = Language.TryToIntegrate;
            ShowOnToolbarCheckBox.Text = Language.ShowOnToolbar;
            RunElevatedCheckBox.Text = Language.RunElevated;

            PropertiesGroupBox.Text = Language.ExternalToolProperties;

            DisplayNameLabel.Text = Language.DisplayName;
            FilenameLabel.Text = Language.Filename;
            ArgumentsLabel.Text = Language.Arguments;
            WorkingDirLabel.Text = Language.WorkingDirectory;
            OptionsLabel.Text = Language.Options;

            WaitForExitCheckBox.Text = Language.WaitForExit;
            BrowseButton.Text = Language._Browse;
            BrowseWorkingDir.Text = Language._Browse;
            NewToolMenuItem.Text = Language.NewExternalTool;
            DeleteToolMenuItem.Text = Language.DeleteExternalTool;
            LaunchToolMenuItem.Text = Language.LaunchExternalTool;
        }

        private new void ApplyTheme()
        {
            if (!_themeManager.ThemingActive) return;
            vsToolStripExtender.SetStyle(ToolStrip, _themeManager.ActiveTheme.Version, _themeManager.ActiveTheme.Theme);
            vsToolStripExtender.SetStyle(ToolsContextMenuStrip, _themeManager.ActiveTheme.Version,
                                         _themeManager.ActiveTheme.Theme);
            //Apply the extended palette

            ToolStripContainer.TopToolStripPanel.BackColor =
                _themeManager.ActiveTheme.Theme.ColorPalette.CommandBarMenuDefault.Background;
            ToolStripContainer.TopToolStripPanel.ForeColor =
                _themeManager.ActiveTheme.Theme.ColorPalette.CommandBarMenuDefault.Text;
            PropertiesGroupBox.BackColor =
                _themeManager.ActiveTheme.Theme.ColorPalette.CommandBarMenuDefault.Background;
            PropertiesGroupBox.ForeColor = _themeManager.ActiveTheme.Theme.ColorPalette.CommandBarMenuDefault.Text;
        }

        private void UpdateToolsListObjView()
        {
            try
            {
                ToolsListObjView.BeginUpdate();
                ToolsListObjView.SetObjects(Runtime.ExternalToolsService.ExternalTools, true);
                ToolsListObjView.AutoResizeColumns();
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
                foreach (var externalTool in _currentlySelectedExternalTools)
                {
                    externalTool.Start();
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.LaunchTool() failed.", ex);
            }
        }

        private void UpdateEditorControls()
        {
            var selectedTool = _currentlySelectedExternalTools.FirstOrDefault();

            DisplayNameTextBox.Text = selectedTool?.DisplayName;
            FilenameTextBox.Text = selectedTool?.FileName;
            ArgumentsCheckBox.Text = selectedTool?.Arguments;
            WorkingDirTextBox.Text = selectedTool?.WorkingDir;
            WaitForExitCheckBox.Checked = selectedTool?.WaitForExit ?? false;
            TryToIntegrateCheckBox.Checked = selectedTool?.TryIntegrate ?? false;
            ShowOnToolbarCheckBox.Checked = selectedTool?.ShowOnToolbar ?? false;
            RunElevatedCheckBox.Checked = selectedTool?.RunElevated ?? false;
            WaitForExitCheckBox.Enabled = !TryToIntegrateCheckBox.Checked;
        }

        private void UpdateToolstipControls()
        {
            _currentlySelectedExternalTools.Clear();
            _currentlySelectedExternalTools.AddRange(ToolsListObjView.SelectedObjects.OfType<ExternalTool>());
            PropertiesGroupBox.Enabled = _currentlySelectedExternalTools.Count == 1;

            var atleastOneToolSelected = _currentlySelectedExternalTools.Count > 0;
            DeleteToolMenuItem.Enabled = atleastOneToolSelected;
            DeleteToolToolstripButton.Enabled = atleastOneToolSelected;
            LaunchToolMenuItem.Enabled = atleastOneToolSelected;
            LaunchToolToolstripButton.Enabled = atleastOneToolSelected;
        }

        #endregion

        #region Event Handlers

        private void CurrentlySelectedExternalToolsOnCollectionUpdated(object sender,
                                                                       CollectionUpdatedEventArgs<ExternalTool>
                                                                           collectionUpdatedEventArgs)
        {
            UpdateEditorControls();
        }

        private void ExternalTools_FormClosed(object sender, FormClosedEventArgs e)
        {
            _externalAppsSaver.Save(Runtime.ExternalToolsService.ExternalTools);
            _themeManager.ThemeChanged -= ApplyTheme;
            _currentlySelectedExternalTools.CollectionUpdated -= CurrentlySelectedExternalToolsOnCollectionUpdated;
        }

        private void NewTool_Click(object sender, EventArgs e)
        {
            try
            {
                var externalTool = new ExternalTool(Language.ExternalToolDefaultName);
                Runtime.ExternalToolsService.ExternalTools.Add(externalTool);
                UpdateToolsListObjView();
                ToolsListObjView.SelectedObject = externalTool;
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
                if (_currentlySelectedExternalTools.Count == 1)
                    message = string.Format(Language.ConfirmDeleteExternalTool,
                                            _currentlySelectedExternalTools[0].DisplayName);
                else if (_currentlySelectedExternalTools.Count > 1)
                    message = string.Format(Language.ConfirmDeleteExternalToolMultiple,
                                            _currentlySelectedExternalTools.Count);
                else
                    return;

                if (MessageBox.Show(FrmMain.Default, message, "Question?", MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                foreach (var externalTool in _currentlySelectedExternalTools)
                {
                    Runtime.ExternalToolsService.ExternalTools.Remove(externalTool);
                }

                var firstDeletedNode = _currentlySelectedExternalTools.FirstOrDefault();
                var oldSelectedIndex = ToolsListObjView.IndexOf(firstDeletedNode);
                _currentlySelectedExternalTools.Clear();
                UpdateToolsListObjView();

                var maxIndex = ToolsListObjView.GetItemCount() - 1;
                ToolsListObjView.SelectedIndex = oldSelectedIndex <= maxIndex
                    ? oldSelectedIndex
                    : maxIndex;

                UpdateToolstipControls();
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
                UpdateToolstipControls();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(
                                                             "UI.Window.ExternalTools.ToolsListObjView_SelectedIndexChanged() failed.",
                                                             ex);
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
            var selectedTool = _currentlySelectedExternalTools.FirstOrDefault();
            if (selectedTool == null)
                return;

            try
            {
                selectedTool.DisplayName = DisplayNameTextBox.Text;
                selectedTool.FileName = FilenameTextBox.Text;
                selectedTool.Arguments = ArgumentsCheckBox.Text;
                selectedTool.WorkingDir = WorkingDirTextBox.Text;
                selectedTool.WaitForExit = WaitForExitCheckBox.Checked;
                selectedTool.TryIntegrate = TryToIntegrateCheckBox.Checked;
                selectedTool.ShowOnToolbar = ShowOnToolbarCheckBox.Checked;
                selectedTool.RunElevated = RunElevatedCheckBox.Checked;

                UpdateToolsListObjView();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(
                                                             "UI.Window.ExternalTools.PropertyControl_ChangedOrLostFocus() failed.",
                                                             ex);
            }
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (var browseDialog = new OpenFileDialog())
                {
                    browseDialog.Filter = string.Join("|", Language.FilterApplication, "*.exe",
                                                      Language.FilterAll, "*.*");
                    if (browseDialog.ShowDialog() != DialogResult.OK)
                        return;
                    var selectedItem = _currentlySelectedExternalTools.FirstOrDefault();
                    if (selectedItem == null)
                        return;
                    selectedItem.FileName = browseDialog.FileName;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.BrowseButton_Click() failed.",
                                                             ex);
            }
        }

        private void BrowseWorkingDir_Click(object sender, EventArgs e)
        {
            try
            {
                using (var browseDialog = new FolderBrowserDialog())
                {
                    if (browseDialog.ShowDialog() != DialogResult.OK)
                        return;
                    var selectedItem = _currentlySelectedExternalTools.FirstOrDefault();
                    if (selectedItem == null)
                        return;
                    selectedItem.WorkingDir = browseDialog.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.BrowseButton_Click() failed.",
                                                             ex);
            }
        }

        private void ToolsListObjView_CellToolTipShowing(object sender, ToolTipShowingEventArgs e)
        {
            if (e.Column != WaitForExitColumnHeader)
                return;

            if (!(e.Model is ExternalTool rowItemAsExternalTool) || !rowItemAsExternalTool.TryIntegrate)
                return;

            e.Text =
                $"'{Language.WaitForExit}' cannot be enabled if '{Language.TryToIntegrate}' is enabled";
        }

        #endregion
    }
}