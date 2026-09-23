using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using mRemoteNG.App;
using mRemoteNG.Plugins;
using mRemoteNG.Properties;
using mRemoteNG.Resources.Language;
using mRemoteNG.UI;
using mRemoteNG.UI.Controls;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    [SupportedOSPlatform("windows")]
    public sealed class PluginsPage : OptionsPage
    {
        private readonly MrngLabel _lblExplanation;
        private readonly MrngLabel _lblFolder;
        private readonly MrngTextBox _txtFolder;
        private readonly MrngLabel _lblFolderMode;
        private readonly MrngLabel _lblFolderWarning;
        private readonly MrngLabel _lblPanelOverride;
        private readonly MrngTextBox _txtPluginPanel;
        private readonly ListView _lvPlugins;
        private readonly ColumnHeader _colEnabled;
        private readonly ColumnHeader _colName;
        private readonly ColumnHeader _colId;
        private readonly ColumnHeader _colPanel;
        private readonly ColumnHeader _colVersion;
        private readonly MrngButton _btnBrowse;
        private readonly MrngButton _btnResetToDefault;
        private readonly MrngButton _btnRefresh;

        public PluginsPage()
        {
            PageIcon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.Settings_16x);

            _lblExplanation = new MrngLabel
            {
                AutoSize = true,
                Dock = DockStyle.Top,
                Padding = new Padding(0, 0, 0, 8)
            };

            _lblFolder = new MrngLabel
            {
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Padding = new Padding(0, 0, 0, 3)
            };

            _txtFolder = new MrngTextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                Width = 320,
                ReadOnly = false
            };
            _txtFolder.TextChanged += (_, _) =>
            {
                HasChanges = true;
                RefreshFolderState();
            };

            _btnBrowse = new MrngButton { AutoSize = true, MinimumSize = new Size(90, 26) };
            _btnBrowse.Click += (_, _) => BrowseForPluginFolder();

            _btnResetToDefault = new MrngButton { AutoSize = true, MinimumSize = new Size(110, 26) };
            _btnResetToDefault.Click += (_, _) => ResetPluginFolderToDefault();

            _btnRefresh = new MrngButton { AutoSize = true, MinimumSize = new Size(90, 26) };
            _btnRefresh.Click += (_, _) => RefreshPluginEntries();

            _lblFolderMode = new MrngLabel
            {
                AutoSize = true,
                ForeColor = Color.Gray,
                Dock = DockStyle.Top,
                Padding = new Padding(0, 4, 0, 0)
            };

            _lblFolderWarning = new MrngLabel
            {
                AutoSize = true,
                Dock = DockStyle.Top,
                ForeColor = Color.DarkGoldenrod,
                Visible = false,
                Padding = new Padding(0, 4, 0, 0)
            };

            _lblPanelOverride = new MrngLabel
            {
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Text = "Plugin panel:"
            };

            _txtPluginPanel = new MrngTextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                Width = 220,
                Text = string.Empty
            };
            _txtPluginPanel.TextChanged += (_, _) =>
            {
                string selectedPluginId = GetSelectedPluginId();
                if (string.IsNullOrWhiteSpace(selectedPluginId))
                {
                    return;
                }

                Runtime.PluginService.SetPluginTargetPanel(selectedPluginId, NormalizePanelName(_txtPluginPanel.Text));
                HasChanges = true;
                UpdateSelectedPluginPanelText();
            };

            _colEnabled = new ColumnHeader { Text = "Enabled", Width = 70 };
            _colName = new ColumnHeader { Text = "Plugin", Width = 180 };
            _colId = new ColumnHeader { Text = "Id", Width = 200 };
            _colPanel = new ColumnHeader { Text = "Panel", Width = 120 };
            _colVersion = new ColumnHeader { Text = "Version", Width = 100 };

            _lvPlugins = new ListView
            {
                CheckBoxes = true,
                FullRowSelect = true,
                GridLines = true,
                View = View.Details,
                Dock = DockStyle.Fill,
                MultiSelect = false,
                HeaderStyle = ColumnHeaderStyle.Nonclickable
            };
            _lvPlugins.Columns.AddRange([_colEnabled, _colName, _colId, _colPanel, _colVersion]);
            _lvPlugins.ItemChecked += (_, _) => HasChanges = true;
            _lvPlugins.SelectedIndexChanged += (_, _) => UpdateSelectedPluginPanelText();

            TableLayoutPanel folderLayout = new()
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 5,
                Padding = new Padding(0, 6, 0, 6)
            };
            folderLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            folderLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            folderLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            folderLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            folderLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            folderLayout.Controls.Add(_lblFolder, 0, 0);
            folderLayout.Controls.Add(_txtFolder, 1, 0);
            folderLayout.Controls.Add(_btnBrowse, 2, 0);
            folderLayout.Controls.Add(_btnResetToDefault, 3, 0);
            folderLayout.Controls.Add(_btnRefresh, 4, 0);

            TableLayoutPanel pluginOverrideLayout = new()
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                Padding = new Padding(0, 0, 0, 6)
            };
            pluginOverrideLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            pluginOverrideLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            pluginOverrideLayout.Controls.Add(_lblPanelOverride, 0, 0);
            pluginOverrideLayout.Controls.Add(_txtPluginPanel, 1, 0);

            Panel headerPanel = new()
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(6)
            };
            headerPanel.Controls.Add(_lblExplanation);
            headerPanel.Controls.Add(folderLayout);
            headerPanel.Controls.Add(_lblFolderMode);
            headerPanel.Controls.Add(_lblFolderWarning);
            headerPanel.Controls.Add(pluginOverrideLayout);

            Controls.Add(headerPanel);
            Controls.Add(_lvPlugins);

            ApplyTheme();
        }

        public override string PageName
        {
            get => GetLanguageString("Plugins", "Plugins");
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            _lblExplanation.Text = GetLanguageString("PluginsPageDescription", "Manage plugin folders and discovered plugins. Disabled plugins stay in the list but are not loaded. Missing folders are ignored until they are available again.");
            _btnBrowse.Text = GetLanguageString("AddFolder", "Add folder");
            _btnResetToDefault.Text = GetLanguageString("UseDefault", "Use default");
            _btnRefresh.Text = GetLanguageString("RefreshView", "Refresh view");
            _lblFolder.Text = GetLanguageString("PluginsPageFolder", "Plugins folder:");
            _lblPanelOverride.Text = GetLanguageString("PluginsPageTargetPanel", "Panel for selected plugin:");
            _lblFolderMode.Text = string.Empty;
            _colEnabled.Text = GetLanguageString("Enabled", "Enabled");
            _colName.Text = GetLanguageString("Plugin", "Plugin");
            _colId.Text = GetLanguageString("Id", "Id");
            _colPanel.Text = GetLanguageString("PluginsPagePanelColumn", "Panel");
            _colVersion.Text = Language.Version;
            UpdateSelectedPluginPanelText();
        }

        public override void LoadSettings()
        {
            _txtFolder.Text = string.Join(";", Runtime.PluginService.GetPluginDirectories());
            RefreshFolderState();
            LoadPluginEntries();
            UpdateSelectedPluginPanelText();
            HasChanges = false;
        }

        public override void SaveSettings()
        {
            List<string> disabledPluginIds = [];
            foreach (ListViewItem item in _lvPlugins.Items)
            {
                if (!item.Checked && item.Tag is string pluginId && !string.IsNullOrWhiteSpace(pluginId))
                {
                    disabledPluginIds.Add(pluginId);
                }
            }

            Settings.Default.PluginFolderPath = NormalizePluginFolderPath(_txtFolder.Text);
            Settings.Default.DisabledPlugins = string.Join(";", disabledPluginIds.Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(id => id, StringComparer.OrdinalIgnoreCase));
            Runtime.PluginService.ReloadPlugins();
            RefreshFolderState();
            LoadPluginEntries();
            UpdateSelectedPluginPanelText();
            HasChanges = false;
        }

        public override void RevertSettings()
        {
            _txtFolder.Text = string.Join(";", Runtime.PluginService.GetPluginDirectories());
            RefreshFolderState();
            LoadPluginEntries();
            UpdateSelectedPluginPanelText();
            HasChanges = false;
        }

        private void LoadPluginEntries()
        {
            _lvPlugins.BeginUpdate();
            try
            {
                _lvPlugins.Items.Clear();
                foreach (PluginCatalogEntry entry in Runtime.PluginService.GetPluginCatalog())
                {
                    string pluginPanel = Runtime.PluginService.GetPluginTargetPanel(entry.PluginId, string.Empty);
                    string displayPanel = string.IsNullOrWhiteSpace(pluginPanel) || string.Equals(pluginPanel, "General", StringComparison.OrdinalIgnoreCase)
                        ? GetLanguageString("PluginsPagePanelDefault", "Default (General)")
                        : pluginPanel;

                    ListViewItem item = new(entry.IsEnabled ? Language.Yes : Language.No)
                    {
                        Checked = entry.IsEnabled,
                        Tag = entry.PluginId
                    };
                    item.SubItems.Add(entry.DisplayName);
                    item.SubItems.Add(entry.PluginId);
                    item.SubItems.Add(displayPanel);
                    item.SubItems.Add(entry.Version);
                    _lvPlugins.Items.Add(item);
                }
            }
            finally
            {
                _lvPlugins.EndUpdate();
            }

            if (_lvPlugins.Items.Count > 0 && _lvPlugins.SelectedItems.Count == 0)
            {
                _lvPlugins.Items[0].Selected = true;
            }
        }

        private void RefreshPluginEntries()
        {
            Settings.Default.PluginFolderPath = NormalizePluginFolderPath(_txtFolder.Text);
            Runtime.PluginService.ReloadPlugins();
            RefreshFolderState();
            LoadPluginEntries();
            UpdateSelectedPluginPanelText();
            HasChanges = true;
        }

        private void BrowseForPluginFolder()
        {
            using CommonOpenFileDialog selectFolderDialog = DialogFactory.SelectFolder(GetLanguageString("PluginsPageFolder", "Plugins folder:"));
            string currentFolder = _txtFolder.Text.Trim();
            if (!string.IsNullOrWhiteSpace(currentFolder) && Directory.Exists(currentFolder))
            {
                selectFolderDialog.InitialDirectory = currentFolder;
                selectFolderDialog.DefaultDirectory = currentFolder;
            }

            if (selectFolderDialog.ShowDialog() != CommonFileDialogResult.Ok)
            {
                return;
            }

            string selectedFolder = NormalizePluginFolderPath(selectFolderDialog.FileName);
            string combinedFolders = AppendPluginFolder(selectedFolder);
            _txtFolder.Text = combinedFolders;
            RefreshPluginEntries();
        }

        private void ResetPluginFolderToDefault()
        {
            _txtFolder.Text = Runtime.PluginService.GetDefaultPluginDirectory();
            RefreshPluginEntries();
        }

        private void UpdateSelectedPluginPanelText()
        {
            string pluginId = GetSelectedPluginId();
            if (string.IsNullOrWhiteSpace(pluginId))
            {
                _txtPluginPanel.Text = string.Empty;
                _txtPluginPanel.Enabled = false;
                return;
            }

            _txtPluginPanel.Enabled = true;
            string targetPanel = Runtime.PluginService.GetPluginTargetPanel(pluginId, string.Empty);
            _txtPluginPanel.Text = string.IsNullOrWhiteSpace(targetPanel) || string.Equals(targetPanel, "General", StringComparison.OrdinalIgnoreCase)
                ? string.Empty
                : targetPanel;
        }

        private string GetSelectedPluginId()
        {
            if (_lvPlugins.SelectedItems.Count == 0)
            {
                return string.Empty;
            }

            return _lvPlugins.SelectedItems[0].Tag as string ?? string.Empty;
        }

        private void RefreshFolderState()
        {
            string[] configuredPaths = GetConfiguredFolders();
            bool hasAnyConfigured = configuredPaths.Length > 0;
            bool hasAnyExistingFolder = configuredPaths.Any(Directory.Exists);
            bool isDefaultFolder = configuredPaths.Length == 1 && IsDefaultPluginFolder(configuredPaths[0]);

            _lblFolderMode.Text = string.Format(
                GetLanguageString("PluginsPageFolderMode", "Current folder type: {0}"),
                isDefaultFolder
                    ? GetLanguageString("PluginsPageFolderModeDefault", "Default")
                    : GetLanguageString("PluginsPageFolderModeCustom", "Custom"));

            if (!hasAnyConfigured || hasAnyExistingFolder)
            {
                _lblFolderWarning.Visible = false;
                _lblFolderWarning.Text = string.Empty;
                return;
            }

            string missingFolders = string.Join(", ", configuredPaths.Where(path => !Directory.Exists(path)));
            _lblFolderWarning.Text = string.Format(GetLanguageString("PluginsPageFolderNotFound", "The selected plugin folder(s) '{0}' do not exist. You can save it now and make sure the shared or local path is available before the next rescan."), missingFolders);
            _lblFolderWarning.Visible = true;
        }

        private string[] GetConfiguredFolders()
        {
            return NormalizePluginFolderPath(_txtFolder.Text)
                .Split([';'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Where(path => !string.IsNullOrWhiteSpace(path))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();
        }

        private string AppendPluginFolder(string folderPath)
        {
            string trimmedFolder = NormalizePluginFolderPath(folderPath);
            if (string.IsNullOrWhiteSpace(trimmedFolder))
            {
                return string.Empty;
            }

            string[] existingFolders = GetConfiguredFolders();
            if (existingFolders.Any(existing => string.Equals(existing, trimmedFolder, StringComparison.OrdinalIgnoreCase)))
            {
                return string.Join(";", existingFolders);
            }

            string combined = string.Join(";", existingFolders.Append(trimmedFolder));
            return combined;
        }

        private bool IsDefaultPluginFolder(string pluginFolder)
        {
            return string.Equals(
                NormalizePluginFolderPath(pluginFolder),
                NormalizePluginFolderPath(Runtime.PluginService.GetDefaultPluginDirectory()),
                StringComparison.OrdinalIgnoreCase);
        }

        private static string NormalizePluginFolderPath(string pluginFolderPath)
        {
            return pluginFolderPath?.Trim() ?? string.Empty;
        }

        private static string NormalizePanelName(string panelName)
        {
            string normalized = panelName?.Trim() ?? string.Empty;
            if (string.Equals(normalized, "General", StringComparison.OrdinalIgnoreCase))
            {
                return string.Empty;
            }

            return normalized;
        }

        private static string GetLanguageString(string resourceName, string fallback)
        {
            return Language.ResourceManager.GetString(resourceName, CultureInfo.CurrentUICulture) ?? fallback;
        }
    }
}
