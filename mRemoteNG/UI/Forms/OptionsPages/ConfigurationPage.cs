using System;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    [SupportedOSPlatform("windows")]
    public sealed partial class ConfigurationPage
    {
        public ConfigurationPage()
        {
            InitializeComponent();
            ApplyTheme();
            PageIcon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.Settings_16x);
        }

        public override string PageName
        {
            get => Language.Config;
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            lblConfigurationDirectory.Text = "Configuration directory:";
            btnBrowseConfigurationDirectory.Text = Language.strBrowse;
            lblExtAppsFile.Text = "External apps file:";
            btnBrowseExtAppsFile.Text = Language.strBrowse;
            lblConfigurationRestartRequired.Text =
                $"{Application.ProductName ?? "mRemoteNG"} must be restarted before configuration directory changes take effect.";

            lblPortableInfo.Text = Runtime.IsPortableEdition
                ? "Portable edition always uses the application folder and does not support a custom configuration directory."
                : "Leave these values empty to use the default per-user configuration directory.";
        }

        public override void LoadSettings()
        {
            txtConfigurationDirectory.Text = Properties.Settings.Default.CustomConfigurationPath;
            txtExtAppsFilePath.Text = Properties.Settings.Default.CustomExtAppsFilePath;
            ToggleControlsForEdition();
        }

        public override void SaveSettings()
        {
            SaveExtAppsFilePathSetting();

            if (Runtime.IsPortableEdition)
                return;

            string rawInput = txtConfigurationDirectory.Text?.Trim() ?? string.Empty;
            string currentPath = NormalizePath(SettingsFileInfo.SettingsPath);
            string defaultPath = NormalizePath(SettingsFileInfo.DefaultSettingsPath);

            string targetPath;
            string settingValue;

            if (string.IsNullOrWhiteSpace(rawInput))
            {
                targetPath = defaultPath;
                settingValue = string.Empty;
            }
            else
            {
                if (!TryNormalizePath(rawInput, out string normalizedTargetPath))
                {
                    ShowInvalidPathMessage(rawInput);
                    txtConfigurationDirectory.Text = Properties.Settings.Default.CustomConfigurationPath;
                    return;
                }

                targetPath = NormalizePath(normalizedTargetPath);
                settingValue = PathsEqual(targetPath, defaultPath) ? string.Empty : targetPath;
            }

            if (PathsEqual(currentPath, targetPath)
                && string.Equals(Properties.Settings.Default.CustomConfigurationPath ?? string.Empty, settingValue,
                    StringComparison.Ordinal))
            {
                return;
            }

            bool shouldMigrate = false;
            if (DirectoryContainsConfigurationData(currentPath) && !PathsEqual(currentPath, targetPath))
            {
                DialogResult migrateResult = MessageBox.Show(
                    this,
                    $"The configuration directory will change from:{Environment.NewLine}{currentPath}{Environment.NewLine}{Environment.NewLine}" +
                    $"to:{Environment.NewLine}{targetPath}{Environment.NewLine}{Environment.NewLine}" +
                    "Copy existing configuration files to the new directory?",
                    "Configuration Directory",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (migrateResult == DialogResult.Cancel)
                {
                    txtConfigurationDirectory.Text = Properties.Settings.Default.CustomConfigurationPath;
                    return;
                }

                shouldMigrate = migrateResult == DialogResult.Yes;
            }

            if (shouldMigrate)
            {
                try
                {
                    CopyConfigurationFiles(currentPath, targetPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        this,
                        $"Failed to copy configuration files to the new directory.{Environment.NewLine}{Environment.NewLine}{ex.Message}",
                        "Configuration Directory",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
            }

            Properties.Settings.Default.CustomConfigurationPath = settingValue;
            txtConfigurationDirectory.Text = settingValue;

            MessageBox.Show(
                this,
                $"{Application.ProductName ?? "mRemoteNG"} must be restarted before the configuration directory change takes effect.",
                "Configuration Directory",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void ToggleControlsForEdition()
        {
            bool enabled = !Runtime.IsPortableEdition;
            txtConfigurationDirectory.Enabled = enabled;
            btnBrowseConfigurationDirectory.Enabled = enabled;
        }

        private void SaveExtAppsFilePathSetting()
        {
            string rawInput = txtExtAppsFilePath.Text?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(rawInput))
            {
                Properties.Settings.Default.CustomExtAppsFilePath = string.Empty;
                txtExtAppsFilePath.Text = string.Empty;
                return;
            }

            if (!TryNormalizePath(rawInput, out string normalizedPath))
            {
                MessageBox.Show(
                    this,
                    $"The external apps file path is not valid:{Environment.NewLine}{rawInput}",
                    "External Apps File",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtExtAppsFilePath.Text = Properties.Settings.Default.CustomExtAppsFilePath;
                return;
            }

            Properties.Settings.Default.CustomExtAppsFilePath = normalizedPath;
            txtExtAppsFilePath.Text = normalizedPath;
        }

        private void btnBrowseConfigurationDirectory_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog dialog = new()
            {
                Description = "Select configuration directory"
            };

            string selectedPath = txtConfigurationDirectory.Text?.Trim() ?? string.Empty;
            if (Directory.Exists(selectedPath))
            {
                dialog.SelectedPath = selectedPath;
            }

            if (dialog.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
            {
                txtConfigurationDirectory.Text = dialog.SelectedPath;
            }
        }

        private void btnBrowseExtAppsFile_Click(object sender, EventArgs e)
        {
            using OpenFileDialog dialog = new()
            {
                Title = "Select external apps file",
                Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*",
                DefaultExt = "xml",
                CheckFileExists = false
            };

            string currentPath = txtExtAppsFilePath.Text?.Trim() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(currentPath))
            {
                string? dir = Path.GetDirectoryName(currentPath);
                if (!string.IsNullOrWhiteSpace(dir) && Directory.Exists(dir))
                    dialog.InitialDirectory = dir;
                dialog.FileName = Path.GetFileName(currentPath);
            }

            if (dialog.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.FileName))
            {
                txtExtAppsFilePath.Text = dialog.FileName;
            }
        }

        private static bool DirectoryContainsConfigurationData(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
                return false;

            return Directory.EnumerateFileSystemEntries(path).Any();
        }

        private static void CopyConfigurationFiles(string sourcePath, string destinationPath)
        {
            if (PathsEqual(sourcePath, destinationPath))
                return;

            if (!Directory.Exists(sourcePath))
                return;

            Directory.CreateDirectory(destinationPath);

            foreach (string file in Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(sourcePath, file);
                string destinationFile = Path.Combine(destinationPath, relativePath);
                string? destinationDirectory = Path.GetDirectoryName(destinationFile);
                if (!string.IsNullOrWhiteSpace(destinationDirectory))
                    Directory.CreateDirectory(destinationDirectory);

                File.Copy(file, destinationFile, true);
            }
        }

        private static void ShowInvalidPathMessage(string path)
        {
            MessageBox.Show(
                $"The path is not valid:{Environment.NewLine}{path}",
                "Configuration Directory",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        private static bool TryNormalizePath(string path, out string normalizedPath)
        {
            normalizedPath = string.Empty;

            try
            {
                string expandedPath = Environment.ExpandEnvironmentVariables(path.Trim());
                normalizedPath = Path.GetFullPath(expandedPath);
                return true;
            }
            catch (Exception ex) when (ex is ArgumentException or NotSupportedException or PathTooLongException)
            {
                return false;
            }
        }

        private static string NormalizePath(string path)
        {
            if (TryNormalizePath(path, out string normalized))
                return normalized.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            return (path ?? string.Empty).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        private static bool PathsEqual(string pathA, string pathB)
        {
            return string.Equals(NormalizePath(pathA), NormalizePath(pathB), StringComparison.OrdinalIgnoreCase);
        }
    }
}
