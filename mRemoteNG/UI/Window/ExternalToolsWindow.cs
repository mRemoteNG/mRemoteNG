using System;
using System.Globalization;
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
            _currentlySelectedExternalTools = [];
            _currentlySelectedExternalTools.CollectionUpdated += CurrentlySelectedExternalToolsOnCollectionUpdated;
            BrowseButton.Height = FilenameTextBox.Height;
            BrowseWorkingDir.Height = WorkingDirTextBox.Height;
            BrowsePrivateKeyButton.Height = PrivateKeyFileTextBox.Height;
            AuthenticationPasswordTextBox.UseSystemPasswordChar = true;
            PassphraseTextBox.UseSystemPasswordChar = true;
            ResizeEnd += ExternalTools_ResizeEnd;
        }


        #region Private Methods

        private void ExternalTools_Load(object sender, EventArgs e)
        {
            ApplyLanguage();
            ApplyTheme();
            UpdateToolsListObjView();

            if (!TryRestoreToolsListLayout())
            {
                ToolsListObjView.AutoResizeColumns();
            }
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
            IconPathLabel.Text = "Icon Path:";
            ArgumentsLabel.Text = Language.Arguments;
            WorkingDirLabel.Text = Language.WorkingDirectory;
            OptionsLabel.Text = Language.Options;
            AuthenticationTypeLabel.Text = "Authentication Type:";
            AuthenticationUsernameLabel.Text = "Authentication Username:";
            AuthenticationPasswordLabel.Text = "Authentication Password:";
            PrivateKeyFileLabel.Text = "Private Key File:";
            PassphraseLabel.Text = "Passphrase:";

            WaitForExitCheckBox.Text = Language.WaitForExit;
            BrowseButton.Text = Language._Browse;
            BrowseWorkingDir.Text = Language._Browse;
            BrowsePrivateKeyButton.Text = Language._Browse;
            NewToolMenuItem.Text = Language.NewExternalTool;
            DeleteToolMenuItem.Text = Language.DeleteExternalTool;
            LaunchToolMenuItem.Text = Language.LaunchExternalTool;
        }

        private new void ApplyTheme()
        {
            if (!_themeManager.ThemingActive) return;
            var theme = _themeManager.ActiveTheme.Theme;
            if (theme == null) return;
            vsToolStripExtender.SetStyle(ToolStrip, _themeManager.ActiveTheme.Version, theme);
            vsToolStripExtender.SetStyle(ToolsContextMenuStrip, _themeManager.ActiveTheme.Version,
                                         theme);
            //Apply the extended palette

            ToolStripContainer.TopToolStripPanel.BackColor =
                theme.ColorPalette.CommandBarMenuDefault.Background;
            ToolStripContainer.TopToolStripPanel.ForeColor =
                theme.ColorPalette.CommandBarMenuDefault.Text;
            PropertiesGroupBox.BackColor =
                theme.ColorPalette.CommandBarMenuDefault.Background;
            PropertiesGroupBox.ForeColor = theme.ColorPalette.CommandBarMenuDefault.Text;
        }

        private void UpdateToolsListObjView()
        {
            try
            {
                ToolsListObjView.BeginUpdate();
                ToolsListObjView.SetObjects(Runtime.ExternalToolsService.ExternalTools, true);
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
                foreach (ExternalTool externalTool in _currentlySelectedExternalTools)
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
            ExternalTool? selectedTool = _currentlySelectedExternalTools.FirstOrDefault();

            DisplayNameTextBox.Text = selectedTool?.DisplayName;
            FilenameTextBox.Text = selectedTool?.FileName;
            IconPathTextBox.Text = selectedTool?.IconPath;
            ArgumentsCheckBox.Text = selectedTool?.Arguments;
            WorkingDirTextBox.Text = selectedTool?.WorkingDir;
            AuthenticationTypeTextBox.Text = selectedTool?.AuthenticationType;
            AuthenticationUsernameTextBox.Text = selectedTool?.AuthenticationUsername;
            AuthenticationPasswordTextBox.Text = selectedTool?.AuthenticationPassword;
            PrivateKeyFileTextBox.Text = selectedTool?.PrivateKeyFile;
            PassphraseTextBox.Text = selectedTool?.Passphrase;
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

            bool atleastOneToolSelected = _currentlySelectedExternalTools.Count > 0;
            DeleteToolMenuItem.Enabled = atleastOneToolSelected;
            DeleteToolToolstripButton.Enabled = atleastOneToolSelected;
            LaunchToolMenuItem.Enabled = atleastOneToolSelected;
            LaunchToolToolstripButton.Enabled = atleastOneToolSelected;
        }

        private void SaveToolsListLayout()
        {
            try
            {
                Properties.Settings.Default.ExtAppsLayout = Convert.ToBase64String(ToolsListObjView.SaveState());
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.SaveToolsListLayout() failed.", ex);
            }
        }

        private bool TryRestoreToolsListLayout()
        {
            string layout = Properties.Settings.Default.ExtAppsLayout;
            if (string.IsNullOrWhiteSpace(layout))
                return false;

            try
            {
                return ToolsListObjView.RestoreState(Convert.FromBase64String(layout));
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.TryRestoreToolsListLayout() failed.", ex);
                return false;
            }
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
            SaveToolsListLayout();
            ExternalAppsSaver.Save(Runtime.ExternalToolsService.ExternalTools);
            _themeManager.ThemeChanged -= ApplyTheme;
            _currentlySelectedExternalTools.CollectionUpdated -= CurrentlySelectedExternalToolsOnCollectionUpdated;
            ResizeEnd -= ExternalTools_ResizeEnd;
        }

        private void NewTool_Click(object sender, EventArgs e)
        {
            try
            {
                ExternalTool externalTool = new(Language.ExternalToolDefaultName);
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
                    message = string.Format(CultureInfo.CurrentCulture, Language.ConfirmDeleteExternalTool,
                                            _currentlySelectedExternalTools[0].DisplayName);
                else if (_currentlySelectedExternalTools.Count > 1)
                    message = string.Format(CultureInfo.CurrentCulture, Language.ConfirmDeleteExternalToolMultiple,
                                            _currentlySelectedExternalTools.Count);
                else
                    return;

                if (MessageBox.Show(FrmMain.Default, message, "Question?", MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                foreach (ExternalTool externalTool in _currentlySelectedExternalTools)
                {
                    Runtime.ExternalToolsService.ExternalTools.Remove(externalTool);
                }

                ExternalTool? firstDeletedNode = _currentlySelectedExternalTools.FirstOrDefault();
                int oldSelectedIndex = ToolsListObjView.IndexOf(firstDeletedNode);
                _currentlySelectedExternalTools.Clear();
                UpdateToolsListObjView();

                int maxIndex = ToolsListObjView.GetItemCount() - 1;
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

        private void ExternalTools_ResizeEnd(object sender, EventArgs e)
        {
            SaveToolsListLayout();
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
            ExternalTool? selectedTool = _currentlySelectedExternalTools.FirstOrDefault();
            if (selectedTool == null)
                return;

            try
            {
                selectedTool.DisplayName = DisplayNameTextBox.Text;
                selectedTool.FileName = FilenameTextBox.Text;
                selectedTool.IconPath = IconPathTextBox.Text;
                selectedTool.Arguments = ArgumentsCheckBox.Text;
                selectedTool.WorkingDir = WorkingDirTextBox.Text;
                selectedTool.AuthenticationType = AuthenticationTypeTextBox.Text;
                selectedTool.AuthenticationUsername = AuthenticationUsernameTextBox.Text;
                selectedTool.AuthenticationPassword = AuthenticationPasswordTextBox.Text;
                selectedTool.PrivateKeyFile = PrivateKeyFileTextBox.Text;
                selectedTool.Passphrase = PassphraseTextBox.Text;
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
                using (OpenFileDialog browseDialog = new())
                {
                    browseDialog.Filter = string.Join("|", Language.FilterApplication, "*.exe",
                                                      Language.FilterAll, "*.*");
                    if (browseDialog.ShowDialog() != DialogResult.OK)
                        return;
                    ExternalTool? selectedItem = _currentlySelectedExternalTools.FirstOrDefault();
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

        private void BrowseIconButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog browseDialog = new())
                {
                    browseDialog.Filter = "Icons|*.ico;*.exe;*.dll|All files|*.*";
                    if (browseDialog.ShowDialog() != DialogResult.OK)
                        return;
                    ExternalTool? selectedItem = _currentlySelectedExternalTools.FirstOrDefault();
                    if (selectedItem == null)
                        return;
                    selectedItem.IconPath = browseDialog.FileName;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.BrowseIconButton_Click() failed.",
                                                             ex);
            }
        }

        private void BrowseWorkingDir_Click(object sender, EventArgs e)
        {
            try
            {
                using (FolderBrowserDialog browseDialog = new())
                {
                    if (browseDialog.ShowDialog() != DialogResult.OK)
                        return;
                    ExternalTool? selectedItem = _currentlySelectedExternalTools.FirstOrDefault();
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

        private void BrowsePrivateKeyButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog browseDialog = new())
                {
                    browseDialog.Filter = "Private Key Files|*.ppk;*.pem;*.key|All Files|*.*";
                    if (browseDialog.ShowDialog() != DialogResult.OK)
                        return;
                    ExternalTool? selectedItem = _currentlySelectedExternalTools.FirstOrDefault();
                    if (selectedItem == null)
                        return;
                    selectedItem.PrivateKeyFile = browseDialog.FileName;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("UI.Window.ExternalTools.BrowsePrivateKeyButton_Click() failed.",
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

        private void VariablesButton_Click(object sender, EventArgs e)
        {
            ContextMenuStrip variablesMenu = new();
            
            AddVariableMenuItem(variablesMenu, "Name");
            AddVariableMenuItem(variablesMenu, "Hostname");
            AddVariableMenuItem(variablesMenu, "Port");
            AddVariableMenuItem(variablesMenu, "Protocol");
            AddVariableMenuItem(variablesMenu, "Username");
            AddVariableMenuItem(variablesMenu, "Password");
            AddVariableMenuItem(variablesMenu, "Domain");
            AddVariableMenuItem(variablesMenu, "Description");
            AddVariableMenuItem(variablesMenu, "MacAddress");
            AddVariableMenuItem(variablesMenu, "UserField");
            for (int i = 1; i <= 10; i++)
            {
                AddVariableMenuItem(variablesMenu, $"UserField{i}");
            }
            AddVariableMenuItem(variablesMenu, "EnvironmentTags");
            AddVariableMenuItem(variablesMenu, "SSHOptions");
            AddVariableMenuItem(variablesMenu, "PuttySession");
            AddVariableMenuItem(variablesMenu, "AuthType");
            AddVariableMenuItem(variablesMenu, "AuthUsername");
            AddVariableMenuItem(variablesMenu, "AuthPassword");
            AddVariableMenuItem(variablesMenu, "PrivateKeyFile");
            AddVariableMenuItem(variablesMenu, "Passphrase");
            AddVariableMenuItem(variablesMenu, "IPAddress");
            AddVariableMenuItem(variablesMenu, "LoadBalanceInfo");
            AddVariableMenuItem(variablesMenu, "PrivateKeyPath");
            AddVariableMenuItem(variablesMenu, "RDPStartProgram");
            AddVariableMenuItem(variablesMenu, "RDPStartProgramWorkDir");
            AddVariableMenuItem(variablesMenu, "Notes");
            AddVariableMenuItem(variablesMenu, "Panel");
            AddVariableMenuItem(variablesMenu, "OpeningCommand");

            variablesMenu.Show(VariablesButton, new System.Drawing.Point(0, VariablesButton.Height));
        }

        private void AddVariableMenuItem(ContextMenuStrip menu, string variableName)
        {
            ToolStripMenuItem item = new(variableName);
            item.Click += (s, args) => InsertVariable(variableName);
            menu.Items.Add(item);
        }

        private void InsertVariable(string variableName)
        {
            string textToInsert = $"%{variableName}%";
            int selectionStart = ArgumentsCheckBox.SelectionStart;
            ArgumentsCheckBox.Text = ArgumentsCheckBox.Text.Insert(selectionStart, textToInsert);
            ArgumentsCheckBox.SelectionStart = selectionStart + textToInsert.Length;
            ArgumentsCheckBox.Focus();
            
            // Trigger update
            PropertyControl_ChangedOrLostFocus(ArgumentsCheckBox, EventArgs.Empty);
        }

        #endregion
    }
}