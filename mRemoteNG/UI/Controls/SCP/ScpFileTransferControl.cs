using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol.SCP;
using mRemoteNG.Messages;

namespace mRemoteNG.UI.Controls.SCP
{
    /// <summary>
    /// Main SCP/SFTP file transfer control with dual-pane layout.
    /// Contains local and remote file browsers with transfer buttons between them.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public partial class ScpFileTransferControl : UserControl
    {
        #region Private Fields

        private enum ActivePanel { None, Local, Remote }

        private TableLayoutPanel _mainLayout;
        private LocalFileBrowserPanel _localPanel;
        private RemoteFileBrowserPanel _remotePanel;
        private Panel _buttonPanel;
        private Button _uploadButton;
        private Button _downloadButton;
        private Button _deleteButton;
        private Label _connectionStatusLabel;
        private ScpTransferManager _transferManager;
        private ConnectionInfo _connectionInfo;
        private bool _isDisconnected = false;
        private ActivePanel _activePanel = ActivePanel.None;

        #endregion

        #region Constructor

        public ScpFileTransferControl()
        {
            InitializeComponent();
            InitializeControls();
        }

        #endregion

        #region Initialization

        private void InitializeComponent()
        {
            SuspendLayout();

            Name = "ScpFileTransferControl";
            Size = new Size(1000, 600);
            BackColor = SystemColors.Control;

            ResumeLayout(false);
        }

        private void InitializeControls()
        {
            SuspendLayout();

            // Create main table layout (Local | Buttons | Remote)
            _mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 2
            };

            // Configure columns: Local (45%) | Buttons (10%) | Remote (45%)
            _mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            _mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            _mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));

            // Configure rows: Content (95%) | Status (5%)
            _mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));

            // Create local file browser panel
            _localPanel = new LocalFileBrowserPanel
            {
                Dock = DockStyle.Fill
            };
            _localPanel.SelectionChanged += Panel_SelectionChanged;
            _localPanel.EscapePressed += Panel_EscapePressed;

            // Create remote file browser panel
            _remotePanel = new RemoteFileBrowserPanel
            {
                Dock = DockStyle.Fill
            };
            _remotePanel.SelectionChanged += Panel_SelectionChanged;
            _remotePanel.EscapePressed += Panel_EscapePressed;

            // Create button panel
            _buttonPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = SystemColors.Control
            };

            // Create upload button (→)
            _uploadButton = new Button
            {
                Text = "→\nUpload",
                Size = new Size(70, 60),
                Location = new Point(5, 150),
                Enabled = false,
                Font = new Font("Segoe UI", 9F)
            };
            _uploadButton.Click += UploadButton_Click;

            // Create download button (←)
            _downloadButton = new Button
            {
                Text = "←\nDownload",
                Size = new Size(70, 60),
                Location = new Point(5, 220),
                Enabled = false,
                Font = new Font("Segoe UI", 9F)
            };
            _downloadButton.Click += DownloadButton_Click;

            // Create delete button (×)
            _deleteButton = new Button
            {
                Text = "×\nDelete",
                Size = new Size(70, 60),
                Location = new Point(5, 80),
                Enabled = false,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.DarkRed
            };
            _deleteButton.Click += DeleteButton_Click;

            _buttonPanel.Controls.Add(_uploadButton);
            _buttonPanel.Controls.Add(_deleteButton);
            _buttonPanel.Controls.Add(_downloadButton);

            // Create connection status label
            _connectionStatusLabel = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 8F),
                Text = "Not connected"
            };

            // Add controls to layout
            _mainLayout.Controls.Add(_localPanel, 0, 0);
            _mainLayout.Controls.Add(_buttonPanel, 1, 0);
            _mainLayout.Controls.Add(_remotePanel, 2, 0);
            _mainLayout.Controls.Add(_connectionStatusLabel, 0, 1);
            _mainLayout.SetColumnSpan(_connectionStatusLabel, 3);

            Controls.Add(_mainLayout);

            ResumeLayout(false);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Connects to the remote server using the provided connection information.
        /// </summary>
        public bool Connect(ConnectionInfo connectionInfo)
        {
            try
            {
                _connectionInfo = connectionInfo ?? throw new ArgumentNullException(nameof(connectionInfo));

                var connectMsg = $"Connecting to {connectionInfo.Hostname}:{connectionInfo.Port}...";
                Logger.Instance.Log?.Info($"[ScpFileTransferControl.Connect] {connectMsg}");
                Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg, connectMsg, true);

                // Create transfer manager
                _transferManager = new ScpTransferManager(connectionInfo);

                // Connect to server
                Logger.Instance.Log?.Debug($"[ScpFileTransferControl.Connect] Calling transfer manager Connect()");
                bool connected = _transferManager.Connect(
                    connectionInfo.Hostname,
                    connectionInfo.Port,
                    connectionInfo.Username,
                    connectionInfo.Password);

                if (connected)
                {
                    // Reset disconnection flag on successful connection
                    _isDisconnected = false;

                    // Initialize remote panel
                    Logger.Instance.Log?.Debug("[ScpFileTransferControl.Connect] Initializing remote panel");
                    _remotePanel.Initialize(_transferManager);

                    // Navigate to initial remote path if specified
                    if (!string.IsNullOrWhiteSpace(connectionInfo.ScpInitialRemotePath))
                    {
                        Logger.Instance.Log?.Debug($"[ScpFileTransferControl.Connect] Navigating to remote path: {connectionInfo.ScpInitialRemotePath}");
                        _remotePanel.NavigateToPath(connectionInfo.ScpInitialRemotePath);
                    }

                    // Navigate to initial local path if specified
                    if (!string.IsNullOrWhiteSpace(connectionInfo.ScpInitialLocalPath))
                    {
                        Logger.Instance.Log?.Debug($"[ScpFileTransferControl.Connect] Navigating to local path: {connectionInfo.ScpInitialLocalPath}");
                        _localPanel.NavigateToPath(connectionInfo.ScpInitialLocalPath);
                    }

                    // Buttons will be enabled by Panel_SelectionChanged when files are selected
                    // Initial state: all buttons disabled until selection occurs

                    // Update status
                    _connectionStatusLabel.Text = $"Connected to {connectionInfo.Hostname}:{connectionInfo.Port} as {connectionInfo.Username}";

                    Logger.Instance.Log?.Info("[ScpFileTransferControl.Connect] SCP/SFTP connection established successfully");
                    Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg,
                        "SCP/SFTP connection established successfully", true);

                    return true;
                }
                else
                {
                    _connectionStatusLabel.Text = $"Failed to connect to {connectionInfo.Hostname}";
                    Logger.Instance.Log?.Error($"[ScpFileTransferControl.Connect] Connection failed to {connectionInfo.Hostname}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Log?.Error("[ScpFileTransferControl.Connect] Error connecting to remote server", ex);
                Runtime.MessageCollector?.AddExceptionMessage("Error connecting to remote server", ex);
                _connectionStatusLabel.Text = "Connection failed";
                return false;
            }
        }

        /// <summary>
        /// Disconnects from the remote server.
        /// </summary>
        public void Disconnect()
        {
            // Guard against multiple disconnect calls
            if (_isDisconnected)
            {
                Logger.Instance.Log?.Debug("[ScpFileTransferControl.Disconnect] Already disconnected, skipping");
                return;
            }

            try
            {
                Logger.Instance.Log?.Debug("[ScpFileTransferControl.Disconnect] Starting disconnect");
                _isDisconnected = true;

                _transferManager?.Disconnect();
                _remotePanel?.Disconnect();

                _uploadButton.Enabled = false;
                _downloadButton.Enabled = false;
                _connectionStatusLabel.Text = "Disconnected";

                Logger.Instance.Log?.Info("[ScpFileTransferControl.Disconnect] SCP/SFTP connection closed");
                Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg,
                    "SCP/SFTP connection closed", true);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log?.Error("[ScpFileTransferControl.Disconnect] Error disconnecting", ex);
                Runtime.MessageCollector?.AddExceptionMessage("Error disconnecting", ex);
            }
        }

        #endregion

        #region Private Methods - File Transfer

        private async void UploadButton_Click(object sender, EventArgs e)
        {
            await UploadSelectedFiles();
        }

        private async void DownloadButton_Click(object sender, EventArgs e)
        {
            await DownloadSelectedFiles();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            DeleteSelectedFiles();
        }

        private void Panel_SelectionChanged(object sender, EventArgs e)
        {
            // Track which panel is active (last interacted with)
            if (sender == _localPanel)
            {
                _activePanel = ActivePanel.Local;
            }
            else if (sender == _remotePanel)
            {
                _activePanel = ActivePanel.Remote;
            }

            if (!IsConnected)
            {
                // If not connected, disable all buttons
                _uploadButton.Enabled = false;
                _downloadButton.Enabled = false;
                _deleteButton.Enabled = false;
                return;
            }

            // Enable upload button only when local files are selected
            bool hasLocalSelection = _localPanel.SelectedFiles?.Count > 0;
            _uploadButton.Enabled = hasLocalSelection;

            // Enable download button only when remote files are selected
            bool hasRemoteSelection = _remotePanel.SelectedFiles?.Count > 0;
            _downloadButton.Enabled = hasRemoteSelection;

            // Enable delete button when either panel has files selected
            bool hasAnySelection = hasLocalSelection || hasRemoteSelection;
            _deleteButton.Enabled = hasAnySelection;
        }

        private void Panel_EscapePressed(object sender, EventArgs e)
        {
            // Clear selections in both panels when Escape is pressed in either panel
            _localPanel.ClearSelection();
            _remotePanel.ClearSelection();
            Logger.Instance.Log?.Debug("[ScpFileTransferControl.Panel_EscapePressed] Cleared selections in both panels");
        }

        /// <summary>
        /// Override ProcessCmdKey to catch Escape key at the parent control level.
        /// This provides a safety net if child panels don't receive the key event.
        /// </summary>
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                Logger.Instance.Log?.Debug("[ScpFileTransferControl.ProcessCmdKey] Escape key detected at parent level");
                // Clear selections in both panels
                _localPanel?.ClearSelection();
                _remotePanel?.ClearSelection();
                return true; // Mark as handled
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private bool IsConnected => _transferManager?.IsConnected == true;

        private async Task UploadSelectedFiles()
        {
            try
            {
                var selectedFiles = _localPanel.SelectedFiles;
                if (selectedFiles == null || selectedFiles.Count == 0)
                {
                    MessageBox.Show("Please select files or directories to upload.",
                        "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Count total files and directories
                var fileCount = selectedFiles.Count(f => !f.IsDirectory);
                var dirCount = selectedFiles.Count(f => f.IsDirectory);

                // Build confirmation message
                var itemDescription = new List<string>();
                if (fileCount > 0)
                    itemDescription.Add($"{fileCount} file(s)");
                if (dirCount > 0)
                    itemDescription.Add($"{dirCount} directory(ies) (recursive)");

                var itemList = string.Join(" and ", itemDescription);

                // Confirm transfer
                var result = MessageBox.Show(
                    $"Upload {itemList} to {_remotePanel.CurrentPath}?",
                    "Confirm Upload",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;

                // Disable buttons during transfer
                _uploadButton.Enabled = false;
                _downloadButton.Enabled = false;

                // Create progress dialog
                using (var progressForm = new Form
                {
                    Text = "Uploading Files",
                    Size = new Size(500, 150),
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    StartPosition = FormStartPosition.CenterParent,
                    MaximizeBox = false,
                    MinimizeBox = false
                })
                {
                    var progressLabel = new Label
                    {
                        Text = "Uploading...",
                        Dock = DockStyle.Top,
                        Height = 30,
                        TextAlign = ContentAlignment.MiddleCenter,
                        AutoEllipsis = true
                    };

                    var progressBar = new ProgressBar
                    {
                        Dock = DockStyle.Top,
                        Height = 30,
                        Style = ProgressBarStyle.Marquee,
                        MarqueeAnimationSpeed = 30
                    };

                    progressForm.Controls.Add(progressBar);
                    progressForm.Controls.Add(progressLabel);
                    progressForm.Show();

                    // Upload each file and directory
                    int completed = 0;
                    int totalItems = selectedFiles.Count;

                    foreach (var item in selectedFiles)
                    {
                        if (item.IsDirectory)
                        {
                            // Upload directory recursively
                            progressLabel.Text = $"Uploading directory {item.Name} ({completed + 1}/{totalItems})...";
                            Application.DoEvents();

                            string remotePath = Path.Combine(_remotePanel.CurrentPath, item.Name).Replace("\\", "/");

                            await _transferManager.UploadDirectoryAsync(item.FullPath, remotePath, currentFile =>
                            {
                                // Update progress with current file being uploaded
                                var relPath = currentFile.Substring(item.FullPath.Length).TrimStart('\\', '/');
                                progressLabel.Text = $"Uploading: {item.Name}/{relPath}";
                                Application.DoEvents();
                            });
                        }
                        else
                        {
                            // Upload single file
                            progressLabel.Text = $"Uploading {item.Name} ({completed + 1}/{totalItems})...";
                            Application.DoEvents();

                            string remotePath = Path.Combine(_remotePanel.CurrentPath, item.Name).Replace("\\", "/");

                            await _transferManager.UploadFileAsync(item.FullPath, remotePath);
                        }

                        completed++;
                    }
                }

                // Refresh remote panel
                _remotePanel.RefreshCurrentDirectory();

                // Refresh tree view if any directories were uploaded
                if (dirCount > 0)
                {
                    _remotePanel.RefreshTreeView();
                }

                MessageBox.Show($"Successfully uploaded {itemList}.",
                    "Upload Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector?.AddExceptionMessage("Error uploading files", ex);
                MessageBox.Show($"Error uploading files:\n{ex.Message}",
                    "Upload Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Restore button states based on current selection
                Panel_SelectionChanged(this, EventArgs.Empty);
            }
        }

        private async Task DownloadSelectedFiles()
        {
            try
            {
                var selectedFiles = _remotePanel.SelectedFiles;
                if (selectedFiles == null || selectedFiles.Count == 0)
                {
                    MessageBox.Show("Please select files or directories to download.",
                        "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Count total files and directories
                var fileCount = selectedFiles.Count(f => !f.IsDirectory);
                var dirCount = selectedFiles.Count(f => f.IsDirectory);

                // Build confirmation message
                var itemDescription = new List<string>();
                if (fileCount > 0)
                    itemDescription.Add($"{fileCount} file(s)");
                if (dirCount > 0)
                    itemDescription.Add($"{dirCount} directory(ies) (recursive)");

                var itemList = string.Join(" and ", itemDescription);

                // Confirm transfer
                var result = MessageBox.Show(
                    $"Download {itemList} to {_localPanel.CurrentPath}?",
                    "Confirm Download",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;

                // Disable buttons during transfer
                _uploadButton.Enabled = false;
                _downloadButton.Enabled = false;

                // Create progress dialog
                using (var progressForm = new Form
                {
                    Text = "Downloading Files",
                    Size = new Size(500, 150),
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    StartPosition = FormStartPosition.CenterParent,
                    MaximizeBox = false,
                    MinimizeBox = false
                })
                {
                    var progressLabel = new Label
                    {
                        Text = "Downloading...",
                        Dock = DockStyle.Top,
                        Height = 30,
                        TextAlign = ContentAlignment.MiddleCenter,
                        AutoEllipsis = true
                    };

                    var progressBar = new ProgressBar
                    {
                        Dock = DockStyle.Top,
                        Height = 30,
                        Style = ProgressBarStyle.Marquee,
                        MarqueeAnimationSpeed = 30
                    };

                    progressForm.Controls.Add(progressBar);
                    progressForm.Controls.Add(progressLabel);
                    progressForm.Show();

                    // Download each file and directory
                    int completed = 0;
                    int totalItems = selectedFiles.Count;

                    foreach (var item in selectedFiles)
                    {
                        if (item.IsDirectory)
                        {
                            // Download directory recursively
                            progressLabel.Text = $"Downloading directory {item.Name} ({completed + 1}/{totalItems})...";
                            Application.DoEvents();

                            string localPath = Path.Combine(_localPanel.CurrentPath, item.Name);

                            await _transferManager.DownloadDirectoryAsync(item.FullPath, localPath, currentFile =>
                            {
                                // Update progress with current file being downloaded
                                // Extract relative path from full remote path
                                var relPath = currentFile.StartsWith(item.FullPath)
                                    ? currentFile.Substring(item.FullPath.Length).TrimStart('/', '\\')
                                    : Path.GetFileName(currentFile);
                                progressLabel.Text = $"Downloading: {item.Name}/{relPath}";
                                Application.DoEvents();
                            });
                        }
                        else
                        {
                            // Download single file
                            progressLabel.Text = $"Downloading {item.Name} ({completed + 1}/{totalItems})...";
                            Application.DoEvents();

                            string localPath = Path.Combine(_localPanel.CurrentPath, item.Name);

                            await _transferManager.DownloadFileAsync(item.FullPath, localPath);
                        }

                        completed++;
                    }
                }

                // Refresh local panel
                _localPanel.RefreshCurrentDirectory();

                // Refresh tree view if any directories were downloaded
                if (dirCount > 0)
                {
                    _localPanel.RefreshTreeView();
                }

                MessageBox.Show($"Successfully downloaded {itemList}.",
                    "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector?.AddExceptionMessage("Error downloading files", ex);
                MessageBox.Show($"Error downloading files:\n{ex.Message}",
                    "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Restore button states based on current selection
                Panel_SelectionChanged(this, EventArgs.Empty);
            }
        }

        private void DeleteSelectedFiles()
        {
            try
            {
                // Determine which panel is active and has selected files
                List<FileListItem> filesToDelete = null;
                string location = "";

                if (_activePanel == ActivePanel.Local && _localPanel.SelectedFiles?.Count > 0)
                {
                    filesToDelete = _localPanel.SelectedFiles;
                    location = "local";
                }
                else if (_activePanel == ActivePanel.Remote && _remotePanel.SelectedFiles?.Count > 0)
                {
                    filesToDelete = _remotePanel.SelectedFiles;
                    location = "remote";
                }

                if (filesToDelete == null || filesToDelete.Count == 0)
                {
                    MessageBox.Show("No files selected for deletion in the active panel.",
                        "Delete Files", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Build detailed filename list (limit to first 20 files to prevent dialog overflow)
                const int maxFilesToShow = 20;
                string fileList = string.Join("\n", filesToDelete.Take(maxFilesToShow).Select(f => "  • " + f.Name));
                if (filesToDelete.Count > maxFilesToShow)
                {
                    fileList += $"\n  ... and {filesToDelete.Count - maxFilesToShow} more item(s)";
                }

                // Show confirmation dialog with warning and detailed file list
                string message = $"Delete {filesToDelete.Count} item(s) from the {location} file system?\n\n" +
                               "Files to delete:\n" +
                               fileList + "\n\n" +
                               "⚠️ WARNING: This action cannot be undone!";

                var result = MessageBox.Show(message,
                    "Confirm Deletion",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                if (result != DialogResult.Yes)
                {
                    Logger.Instance.Log?.Debug("[ScpFileTransferControl.DeleteSelectedFiles] User cancelled deletion");
                    return;
                }

                // Delete from active panel only
                if (_activePanel == ActivePanel.Local)
                {
                    Logger.Instance.Log?.Info($"[ScpFileTransferControl.DeleteSelectedFiles] Deleting {filesToDelete.Count} local item(s)");
                    _localPanel.DeleteSelectedFiles();
                }
                else if (_activePanel == ActivePanel.Remote)
                {
                    Logger.Instance.Log?.Info($"[ScpFileTransferControl.DeleteSelectedFiles] Deleting {filesToDelete.Count} remote item(s)");
                    _remotePanel.DeleteSelectedFiles();
                }

                MessageBox.Show($"Successfully deleted {filesToDelete.Count} item(s) from the {location} file system.",
                    "Delete Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log?.Error("[ScpFileTransferControl.DeleteSelectedFiles] Error deleting files", ex);
                Runtime.MessageCollector?.AddExceptionMessage("Error deleting files", ex);
                MessageBox.Show($"Error deleting files:\n{ex.Message}",
                    "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Disposal

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Logger.Instance.Log?.Debug("[ScpFileTransferControl.Dispose] Disposing resources");

                // Don't call Disconnect() here - Close() handles that
                // Just clean up resources
                _transferManager?.Dispose();
                _localPanel?.Dispose();
                _remotePanel?.Dispose();

                Logger.Instance.Log?.Debug("[ScpFileTransferControl.Dispose] Disposal complete");
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
