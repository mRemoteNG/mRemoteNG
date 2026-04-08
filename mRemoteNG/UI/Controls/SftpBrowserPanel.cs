using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms;
using BrightIdeasSoftware;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Tools;
using Renci.SshNet.Common;

namespace mRemoteNG.UI.Controls
{
    [SupportedOSPlatform("windows")]
    internal class SftpBrowserPanel : UserControl
    {
        #region Fields

        private const char RemotePathSeparator = '/';
        private const string DefaultFont = "Segoe UI";

        private SftpFileService _service;
        private bool _isNavigating;
        private bool _isConnecting;
        private System.Windows.Forms.Timer _connectTimer;
        private bool _showHiddenFiles = true;
        private readonly List<string> _historyBack = new();
        private readonly List<string> _historyForward = new();
        private readonly List<FileSystemWatcher> _activeWatchers = new();
        private bool _suppressHistory;

        // Connection info for retry
        private string _host;
        private string _user;
        private string _password;
        private int _port;

        // Controls — fields kept for controls referenced in event handlers
        private ToolStripLabel _lblConnection;
        private ToolStripButton _btnBack;
        private ToolStripButton _btnForward;
        private ToolStripButton _btnHome;
        private ToolStripButton _btnUp;
        private ToolStripButton _btnRefresh;
        private ToolStripButton _btnUpload;
        private ToolStripButton _btnDownload;
        private ToolStripButton _btnNewFile;
        private ToolStripButton _btnNewFolder;
        private ToolStripButton _btnDelete;
        private ToolStripButton _btnToggleHidden;

        private TextBox _txtPath;

        private MrngListView _fileList;

        private Label _lblStatus;
        private Panel _statusDot;
        private ProgressBar _progressBar;

        private ContextMenuStrip _contextMenu;

        // File type extension sets for color coding
        private static readonly HashSet<string> _execExtensions = new(StringComparer.OrdinalIgnoreCase)
            { ".sh", ".bash", ".csh", ".ksh", ".zsh", ".py", ".pl", ".rb", ".exe", ".bat", ".cmd", ".com", ".run", ".bin", ".app" };
        private static readonly HashSet<string> _archiveExtensions = new(StringComparer.OrdinalIgnoreCase)
            { ".tar", ".gz", ".bz2", ".xz", ".zip", ".rar", ".7z", ".tgz", ".tbz2", ".deb", ".rpm", ".jar", ".war", ".iso", ".img" };
        private static readonly HashSet<string> _imageExtensions = new(StringComparer.OrdinalIgnoreCase)
            { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".svg", ".ico", ".tiff", ".tif", ".webp" };
        private static readonly HashSet<string> _configExtensions = new(StringComparer.OrdinalIgnoreCase)
            { ".conf", ".cfg", ".ini", ".yaml", ".yml", ".toml", ".json", ".xml", ".env" };

        // Editable text file extensions
        private static readonly HashSet<string> _textExtensions = new(StringComparer.OrdinalIgnoreCase)
            { ".txt", ".md", ".log", ".sh", ".bash", ".py", ".pl", ".rb", ".js", ".ts", ".css", ".html", ".htm",
              ".xml", ".json", ".yaml", ".yml", ".toml", ".ini", ".cfg", ".conf", ".env", ".csv", ".sql",
              ".c", ".cpp", ".h", ".hpp", ".cs", ".java", ".go", ".rs", ".php", ".lua", ".r", ".m",
              ".makefile", ".dockerfile", ".gitignore", ".bashrc", ".profile", ".zshrc" };

        /// <summary>
        /// Raised when the user clicks the close/toggle button to hide this panel.
        /// The parent (InterfaceControl) handles collapsing the SplitContainer.
        /// </summary>
        public event EventHandler CloseRequested;

        #endregion

        public SftpBrowserPanel()
        {
            InitializeControls();
        }

        private void InitializeControls()
        {
            SuspendLayout();

            // Image list with distinct folder/file icons
            var imageList = new ImageList { ImageSize = new Size(16, 16), ColorDepth = ColorDepth.Depth32Bit };
            imageList.Images.Add("folder", Properties.Resources.FolderClosed_16x);
            imageList.Images.Add("file", Properties.Resources.Document_16x);

            // Toolbar — compact, icon-only with tooltips
            var toolbar = new ToolStrip { GripStyle = ToolStripGripStyle.Hidden, Dock = DockStyle.Top };

            _lblConnection = new ToolStripLabel("SFTP") { Font = new Font(DefaultFont, 7.5F, FontStyle.Bold) };
            _btnBack = new ToolStripButton("", Properties.Resources.GlyphLeft_16x, OnBackClick) { ToolTipText = "Back", Enabled = false };
            _btnForward = new ToolStripButton("", Properties.Resources.GlyphRight_16x, OnForwardClick) { ToolTipText = "Forward", Enabled = false };
            _btnUp = new ToolStripButton("", Properties.Resources.Export_16x, OnUpClick) { ToolTipText = "Up one level", Enabled = false };
            _btnHome = new ToolStripButton("", Properties.Resources.FolderClosed_16x, OnHomeClick) { ToolTipText = "Home directory", Enabled = false };
            _btnRefresh = new ToolStripButton("", Properties.Resources.Refresh_16x, OnRefreshClick) { ToolTipText = "Refresh", Enabled = false };
            _btnUpload = new ToolStripButton("", Properties.Resources.GlyphUp_16x, OnUploadClick) { ToolTipText = "Upload file", Enabled = false };
            _btnDownload = new ToolStripButton("", Properties.Resources.GlyphDown_16x, OnDownloadClick) { ToolTipText = "Download", Enabled = false };
            _btnNewFile = new ToolStripButton("", Properties.Resources.NewFile_16x, OnNewFileClick) { ToolTipText = "New file", Enabled = false };
            _btnNewFolder = new ToolStripButton("", Properties.Resources.AddFolder_16x, OnNewFolderClick) { ToolTipText = "New folder", Enabled = false };
            _btnDelete = new ToolStripButton("", Properties.Resources.Close_16x, OnDeleteClick) { ToolTipText = "Delete", Enabled = false };
            _btnToggleHidden = new ToolStripButton("", Properties.Resources.Property_16x, OnToggleHiddenClick)
            {
                ToolTipText = "Show/hide hidden files",
                Checked = _showHiddenFiles,
                CheckOnClick = true,
                Enabled = false
            };
            var btnClose = new ToolStripButton("", Properties.Resources.Close_16x, OnCloseClick)
            {
                ToolTipText = "Hide SFTP browser",
                Alignment = ToolStripItemAlignment.Right
            };

            toolbar.Items.AddRange(new ToolStripItem[]
            {
                _lblConnection,
                new ToolStripSeparator(),
                _btnBack, _btnForward, _btnUp, _btnHome,
                new ToolStripSeparator(),
                _btnRefresh,
                new ToolStripSeparator(),
                _btnUpload, _btnDownload,
                new ToolStripSeparator(),
                _btnNewFile, _btnNewFolder, _btnDelete,
                new ToolStripSeparator(),
                _btnToggleHidden,
                btnClose
            });

            // Path bar
            var pathPanel = new Panel { Height = 22, Dock = DockStyle.Top, Padding = new Padding(1) };
            _txtPath = new TextBox { Dock = DockStyle.Fill, BorderStyle = BorderStyle.FixedSingle, Font = new Font(DefaultFont, 7.5F) };
            _txtPath.KeyDown += TxtPath_KeyDown;
            var btnGo = new Button { Text = ">", Width = 24, Dock = DockStyle.Right, FlatStyle = FlatStyle.Flat, Font = new Font(DefaultFont, 7F) };
            btnGo.Click += (s, e) => _ = NavigateTo(_txtPath.Text);
            pathPanel.Controls.Add(_txtPath);
            pathPanel.Controls.Add(btnGo);

            // File list
            _fileList = new MrngListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                MultiSelect = false,
                AllowDrop = true,
                ShowGroups = false,
                CheckBoxes = false,
                SmallImageList = imageList,
                Font = new Font(DefaultFont, 7.5F)
            };

            var colName = new OLVColumn("Name", "") { Width = 200, IsEditable = false };
            colName.AspectGetter = obj => ((SftpFileItem)obj).Name;
            colName.ImageGetter = obj => ((SftpFileItem)obj).IsDirectory
                ? Properties.Resources.FolderClosed_16x
                : Properties.Resources.Document_16x;

            var colSize = new OLVColumn("Size", "") { Width = 55, IsEditable = false, TextAlign = HorizontalAlignment.Right };
            colSize.AspectGetter = obj =>
            {
                var item = (SftpFileItem)obj;
                if (item.IsDirectory) return "";
                long size = item.Size;
                if (size < 1024) return $"{size} B";
                if (size < 1024 * 1024) return $"{size / 1024.0:F0} KB";
                if (size < 1024 * 1024 * 1024) return $"{size / (1024.0 * 1024.0):F1} MB";
                return $"{size / (1024.0 * 1024.0 * 1024.0):F1} GB";
            };

            var colOwner = new OLVColumn("Owner", "") { Width = 70, IsEditable = false };
            colOwner.AspectGetter = obj =>
            {
                var item = (SftpFileItem)obj;
                if (string.IsNullOrEmpty(item.Group) || item.Owner == item.Group)
                    return item.Owner;
                return $"{item.Owner}:{item.Group}";
            };

            var colModified = new OLVColumn("Modified", "") { Width = 110, IsEditable = false };
            colModified.AspectGetter = obj =>
            {
                var dt = ((SftpFileItem)obj).LastModified;
                return dt == DateTime.MinValue ? "" : dt.ToString("yyyy-MM-dd HH:mm");
            };

            var colPermissions = new OLVColumn("Permissions", "") { Width = 80, IsEditable = false };
            colPermissions.AspectGetter = obj => ((SftpFileItem)obj).Permissions;

            _fileList.AllColumns.AddRange(new[] { colName, colSize, colOwner, colModified, colPermissions });
            _fileList.Columns.AddRange(new ColumnHeader[] { colName, colSize, colOwner, colModified, colPermissions });

            // Color coding for file types
            _fileList.FormatRow += FileList_FormatRow;

            _fileList.KeyDown += FileList_KeyDown;
            _fileList.DoubleClick += FileList_DoubleClick;
            _fileList.SelectedIndexChanged += FileList_SelectedIndexChanged;
            _fileList.DragEnter += FileList_DragEnter;
            _fileList.DragDrop += FileList_DragDrop;

            // Context menu with full options
            _contextMenu = new ContextMenuStrip();
            _contextMenu.Opening += ContextMenu_Opening;
            _fileList.ContextMenuStrip = _contextMenu;

            // Status bar with connection indicator and progress bar
            var statusPanel = new Panel { Dock = DockStyle.Bottom, Height = 20 };
            _statusDot = new Panel
            {
                Size = new Size(10, 10),
                Location = new Point(4, 5),
                BackColor = Color.Gray
            };
            _statusDot.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using var brush = new SolidBrush(_statusDot.BackColor);
                e.Graphics.FillEllipse(brush, 0, 0, 9, 9);
            };
            _lblStatus = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font(DefaultFont, 7F),
                Text = "Not connected",
                Padding = new Padding(16, 0, 0, 0)
            };
            _progressBar = new ProgressBar
            {
                Dock = DockStyle.Bottom,
                Height = 3,
                Style = ProgressBarStyle.Continuous,
                Visible = false,
                Minimum = 0,
                Maximum = 100
            };
            statusPanel.Controls.Add(_lblStatus);
            statusPanel.Controls.Add(_statusDot);

            // Layout — order matters: Fill control added first
            Controls.Add(_fileList);
            Controls.Add(_progressBar);
            Controls.Add(pathPanel);
            Controls.Add(toolbar);
            Controls.Add(statusPanel);

            ResumeLayout(false);
        }

        #region Public Methods

        public void ConnectWithRetry(string host, string user, string password, int port)
        {
            _host = host;
            _user = user;
            _password = password;
            _port = port;

            // Start retry timer
            _connectTimer?.Stop();
            _connectTimer?.Dispose();
            _connectTimer = new System.Windows.Forms.Timer { Interval = 3000 };
            _connectTimer.Tick += ConnectTimer_Tick;
            _connectTimer.Start();

            // Try immediately
            _ = TryConnect();
        }

        public void DisconnectFromHost()
        {
            _connectTimer?.Stop();
            _connectTimer?.Dispose();
            _connectTimer = null;
            _service?.Dispose();
            _service = null;
            _fileList.ClearObjects();
            _txtPath.Text = "";
            _lblConnection.Text = "SFTP";
            _lblStatus.Text = "Not connected";
            _statusDot.BackColor = Color.Gray;
            _statusDot.Invalidate();
            _progressBar.Visible = false;
            _historyBack.Clear();
            _historyForward.Clear();
            foreach (var w in _activeWatchers) { try { w.Dispose(); } catch { /* best effort */ } }
            _activeWatchers.Clear();
            SetButtonsEnabled(false);
        }

        /// <summary>
        /// Navigate the SFTP panel to the given directory. Called externally
        /// to sync with terminal cd commands.
        /// </summary>
        public void NavigateToDirectory(string path)
        {
            if (_service?.IsConnected == true && !string.IsNullOrEmpty(path))
                _ = NavigateTo(path);
        }

        #endregion

        #region Connection

        private async void ConnectTimer_Tick(object sender, EventArgs e)
        {
            if (_service?.IsConnected == true)
            {
                _connectTimer?.Stop();
                return;
            }
            if (_isConnecting) return;
            await TryConnect();
        }

        private async System.Threading.Tasks.Task TryConnect()
        {
            if (_isConnecting) return;
            _isConnecting = true;
            try
            {
                _service?.Dispose();
                _service = null;

                var newService = new SftpFileService();
                _lblStatus.Text = "Connecting...";
                _statusDot.BackColor = Color.Orange;
                _statusDot.Invalidate();

                await System.Threading.Tasks.Task.Run(() => newService.Connect(_host, _user, _password, _port));

                _service = newService;
                _lblConnection.Text = $"{_user}@{_host}";
                _statusDot.BackColor = Color.LimeGreen;
                _statusDot.Invalidate();
                SetButtonsEnabled(true);
                _connectTimer?.Stop();
                await NavigateTo(_service.CurrentPath);
            }
            catch (Exception)
            {
                _lblStatus.Text = "Connecting...";
                _statusDot.BackColor = Color.Orange;
                _statusDot.Invalidate();
            }
            finally
            {
                _isConnecting = false;
            }
        }

        #endregion

        #region Navigation

        private async System.Threading.Tasks.Task NavigateTo(string path)
        {
            if (_service == null || !_service.IsConnected || _isNavigating) return;
            _isNavigating = true;

            // Track history
            string previousPath = _txtPath.Text;

            try
            {
                _lblStatus.Text = "Loading...";
                var items = await _service.ListDirectoryAsync(path);

                // Push previous path to back history (unless suppressed or same path)
                if (!_suppressHistory && !string.IsNullOrEmpty(previousPath) && previousPath != _service.CurrentPath)
                {
                    _historyBack.Add(previousPath);
                    _historyForward.Clear();
                }
                _suppressHistory = false;

                // Filter hidden files if toggle is off
                if (!_showHiddenFiles)
                    items = items.Where(i => i.Name == ".." || !i.Name.StartsWith(".")).ToList();

                // Sort: ".." first, then folders alphabetically, then files alphabetically
                var sorted = items
                    .OrderByDescending(i => i.Name == "..")
                    .ThenByDescending(i => i.IsDirectory)
                    .ThenBy(i => i.Name, StringComparer.OrdinalIgnoreCase)
                    .ToList();

                _fileList.SetObjects(sorted);
                _txtPath.Text = _service.CurrentPath;
                int files = sorted.Count(i => !i.IsDirectory);
                int dirs = sorted.Count(i => i.IsDirectory && i.Name != "..");
                _lblStatus.Text = $"{files} file(s), {dirs} folder(s)";

                UpdateHistoryButtons();
            }
            catch (SshConnectionException)
            {
                _lblStatus.Text = "Connection lost";
                _statusDot.BackColor = Color.Red;
                _statusDot.Invalidate();
                DisconnectFromHost();
            }
            catch (Exception ex)
            {
                _lblStatus.Text = $"Error: {ex.Message}";
            }
            finally
            {
                _isNavigating = false;
            }
        }

        #endregion

        #region Event Handlers

        private void OnCloseClick(object sender, EventArgs e)
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnBackClick(object sender, EventArgs e)
        {
            if (_historyBack.Count == 0) return;
            var path = _historyBack[^1];
            _historyBack.RemoveAt(_historyBack.Count - 1);
            _historyForward.Add(_txtPath.Text);
            _suppressHistory = true;
            _ = NavigateTo(path);
        }

        private void OnForwardClick(object sender, EventArgs e)
        {
            if (_historyForward.Count == 0) return;
            var path = _historyForward[^1];
            _historyForward.RemoveAt(_historyForward.Count - 1);
            _historyBack.Add(_txtPath.Text);
            _suppressHistory = true;
            _ = NavigateTo(path);
        }

        private void OnUpClick(object sender, EventArgs e)
        {
            if (_service?.IsConnected != true) return;
            var parent = _service.CurrentPath.TrimEnd(RemotePathSeparator);
            var lastSlash = parent.LastIndexOf(RemotePathSeparator);
            var parentPath = lastSlash > 0 ? parent.Substring(0, lastSlash) : RemotePathSeparator.ToString();
            _ = NavigateTo(parentPath);
        }

        private void OnHomeClick(object sender, EventArgs e)
        {
            if (_service?.IsConnected != true) return;
            _ = NavigateTo(_service.HomePath);
        }

        private void OnRefreshClick(object sender, EventArgs e)
        {
            if (_service?.IsConnected != true) return;
            _suppressHistory = true;
            _ = NavigateTo(_service.CurrentPath);
        }

        private void OnToggleHiddenClick(object sender, EventArgs e)
        {
            _showHiddenFiles = _btnToggleHidden.Checked;
            if (_service?.IsConnected == true)
            {
                _suppressHistory = true;
                _ = NavigateTo(_service.CurrentPath);
            }
        }

        private async void OnUploadClick(object sender, EventArgs e)
        {
            if (_service?.IsConnected != true) return;
            using var ofd = new OpenFileDialog { Filter = "All Files (*.*)|*.*", Multiselect = true };
            if (ofd.ShowDialog() != DialogResult.OK) return;

            foreach (var file in ofd.FileNames)
            {
                var fileName = Path.GetFileName(file);
                var remotePath = _service.CurrentPath.TrimEnd(RemotePathSeparator) + RemotePathSeparator + fileName;
                try
                {
                    long fileSize = new FileInfo(file).Length;
                    _lblStatus.Text = $"Uploading {fileName}...";
                    ShowProgress(0);
                    await _service.UploadFileAsync(file, remotePath, bytes =>
                    {
                        if (fileSize > 0)
                            BeginInvoke(() => ShowProgress((int)(bytes * 100 / (ulong)fileSize)));
                    });
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                        $"SFTP Upload: {fileName} -> {_host}:{remotePath}", true);
                }
                catch (Exception ex)
                {
                    _lblStatus.Text = $"Upload failed: {ex.Message}";
                }
            }
            HideProgress();
            _suppressHistory = true;
            await NavigateTo(_service.CurrentPath);
        }

        private async void OnDownloadClick(object sender, EventArgs e)
        {
            var item = _fileList.SelectedObject as SftpFileItem;
            if (item == null || item.IsDirectory || _service?.IsConnected != true) return;

            using var sfd = new SaveFileDialog { FileName = item.Name, Filter = "All Files (*.*)|*.*" };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                _lblStatus.Text = $"Downloading {item.Name}...";
                long fileSize = item.Size;
                ShowProgress(0);
                await _service.DownloadFileAsync(item.FullPath, sfd.FileName, bytes =>
                {
                    if (fileSize > 0)
                        BeginInvoke(() => ShowProgress((int)((long)bytes * 100 / fileSize)));
                });
                HideProgress();
                _lblStatus.Text = $"Downloaded {item.Name}";
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                    $"SFTP Download: {_host}:{item.FullPath} -> {sfd.FileName}", true);
            }
            catch (Exception ex)
            {
                HideProgress();
                _lblStatus.Text = $"Download failed: {ex.Message}";
            }
        }

        private async void OnNewFolderClick(object sender, EventArgs e)
        {
            if (_service?.IsConnected != true) return;
            var name = InputDialog.Prompt("New Folder", "Folder name:");
            if (string.IsNullOrWhiteSpace(name)) return;
            try
            {
                await _service.CreateDirectoryAsync(_service.CurrentPath.TrimEnd(RemotePathSeparator) + RemotePathSeparator + name);
                _suppressHistory = true;
                await NavigateTo(_service.CurrentPath);
            }
            catch (Exception ex) { _lblStatus.Text = $"Error: {ex.Message}"; }
        }

        private async void OnNewFileClick(object sender, EventArgs e)
        {
            if (_service?.IsConnected != true) return;
            var name = InputDialog.Prompt("New File", "File name:");
            if (string.IsNullOrWhiteSpace(name)) return;
            try
            {
                var remotePath = _service.CurrentPath.TrimEnd(RemotePathSeparator) + RemotePathSeparator + name;
                var tempFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                try
                {
                    await _service.UploadFileAsync(tempFile, remotePath);
                }
                finally
                {
                    try { File.Delete(tempFile); } catch (IOException) { /* File may be locked */ }
                }
                _suppressHistory = true;
                await NavigateTo(_service.CurrentPath);
            }
            catch (Exception ex) { _lblStatus.Text = $"Error: {ex.Message}"; }
        }

        private async void OnDeleteClick(object sender, EventArgs e)
        {
            var item = _fileList.SelectedObject as SftpFileItem;
            if (item == null || item.Name == ".." || _service?.IsConnected != true) return;
            if (MessageBox.Show($"Delete '{item.Name}'?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                await _service.DeleteAsync(item.FullPath, item.IsDirectory);
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                    $"SFTP Delete: {_host}:{item.FullPath}", true);
                _suppressHistory = true;
                await NavigateTo(_service.CurrentPath);
            }
            catch (Exception ex) { _lblStatus.Text = $"Error: {ex.Message}"; }
        }

        private async void OnRenameClick(object sender, EventArgs e)
        {
            var item = _fileList.SelectedObject as SftpFileItem;
            if (item == null || item.Name == ".." || _service?.IsConnected != true) return;
            var newName = InputDialog.Prompt("Rename", "New name:", item.Name);
            if (string.IsNullOrWhiteSpace(newName) || newName == item.Name) return;
            try
            {
                await _service.RenameAsync(item.FullPath, _service.CurrentPath.TrimEnd(RemotePathSeparator) + RemotePathSeparator + newName);
                _suppressHistory = true;
                await NavigateTo(_service.CurrentPath);
            }
            catch (Exception ex) { _lblStatus.Text = $"Error: {ex.Message}"; }
        }

        private void OnCopyPathClick(object sender, EventArgs e)
        {
            var item = _fileList.SelectedObject as SftpFileItem;
            if (item == null) return;
            Clipboard.SetText(item.FullPath);
            _lblStatus.Text = $"Copied: {item.FullPath}";
        }

        private async void OnEditFileClick(object sender, EventArgs e)
        {
            var item = _fileList.SelectedObject as SftpFileItem;
            if (item == null || item.IsDirectory || _service?.IsConnected != true) return;

            try
            {
                string tempFile = await DownloadToTempFile(item);
                var lastWrite = File.GetLastWriteTimeUtc(tempFile);
                var proc = Process.Start(new ProcessStartInfo(tempFile) { UseShellExecute = true });

                _lblStatus.Text = $"Editing {item.Name} — save in editor to upload";

                var watcher = CreateEditWatcher(tempFile, item.FullPath, item.Name);
                if (proc != null)
                    WaitForEditorClose(proc, watcher, tempFile, item.FullPath, lastWrite);
            }
            catch (Exception ex)
            {
                _lblStatus.Text = $"Edit failed: {ex.Message}";
            }
        }

        private async System.Threading.Tasks.Task<string> DownloadToTempFile(SftpFileItem item)
        {
            var tempDir = Path.Combine(Path.GetTempPath(), "mRemoteNG_SFTP");
            Directory.CreateDirectory(tempDir);
            string safeHost = string.Join("_", _host.Split(Path.GetInvalidFileNameChars()));
            var tempFile = Path.Combine(tempDir, $"{safeHost}_{Guid.NewGuid():N}_{Path.GetFileName(item.Name)}");

            _lblStatus.Text = $"Downloading {item.Name} for editing...";
            await _service.DownloadFileAsync(item.FullPath, tempFile);
            return tempFile;
        }

        private FileSystemWatcher CreateEditWatcher(string tempFile, string remotePath, string displayName)
        {
            string dir = Path.GetDirectoryName(tempFile);
            var watcher = new FileSystemWatcher(dir, Path.GetFileName(tempFile))
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size,
                EnableRaisingEvents = true
            };
            _activeWatchers.Add(watcher);

            var debounceTimer = System.Diagnostics.Stopwatch.StartNew();
            watcher.Changed += async (ws, we) =>
            {
                if (debounceTimer.ElapsedMilliseconds < 2000) return;
                debounceTimer.Restart();
                await System.Threading.Tasks.Task.Delay(500);
                BeginInvoke(() => UploadEditedFile(tempFile, remotePath, displayName));
            };

            return watcher;
        }

        private async System.Threading.Tasks.Task UploadEditedFile(string tempFile, string remotePath, string displayName)
        {
            if (_service?.IsConnected != true) return;
            try
            {
                _lblStatus.Text = $"Uploading {displayName}...";
                await _service.UploadFileAsync(tempFile, remotePath);
                _lblStatus.Text = $"Saved {displayName}";
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                    $"SFTP Edit: {displayName} saved to {_host}:{remotePath}", true);
                _suppressHistory = true;
                await NavigateTo(_service.CurrentPath);
            }
            catch (Exception ex)
            {
                _lblStatus.Text = $"Upload failed: {ex.Message}";
            }
        }

        private void WaitForEditorClose(Process proc, FileSystemWatcher watcher,
            string tempFile, string remotePath, DateTime lastWrite)
        {
            _ = System.Threading.Tasks.Task.Run(async () =>
            {
                await proc.WaitForExitAsync();
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
                _activeWatchers.Remove(watcher);

                if (File.Exists(tempFile) && File.GetLastWriteTimeUtc(tempFile) > lastWrite)
                    BeginInvoke(() => UploadEditedFile(tempFile, remotePath, Path.GetFileName(tempFile)));

                await System.Threading.Tasks.Task.Delay(1000);
                try { if (File.Exists(tempFile)) File.Delete(tempFile); }
                catch { /* best effort cleanup */ }
            });
        }

        private void FileList_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F5:
                    OnRefreshClick(sender, e);
                    e.Handled = true;
                    break;
                case Keys.Delete:
                    OnDeleteClick(sender, e);
                    e.Handled = true;
                    break;
                case Keys.F2:
                    OnRenameClick(sender, e);
                    e.Handled = true;
                    break;
                case Keys.Enter:
                    FileList_DoubleClick(sender, e);
                    e.Handled = true;
                    break;
                case Keys.Back:
                    OnUpClick(sender, e);
                    e.Handled = true;
                    break;
            }
        }

        private void FileList_DoubleClick(object sender, EventArgs e)
        {
            var item = _fileList.SelectedObject as SftpFileItem;
            if (item == null || _service?.IsConnected != true) return;
            if (item.IsDirectory)
            {
                if (item.Name == "..")
                    OnUpClick(sender, e);
                else
                    _ = NavigateTo(item.FullPath);
            }
            else
            {
                // Double-click text file = edit, other files = download
                if (IsTextFile(item.Name))
                    OnEditFileClick(sender, e);
                else
                    OnDownloadClick(sender, e);
            }
        }

        private void FileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = _fileList.SelectedObject as SftpFileItem;
            bool hasSelection = item != null && item.Name != "..";
            _btnDownload.Enabled = hasSelection && !item.IsDirectory;
            _btnDelete.Enabled = hasSelection;
        }

        private void FileList_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = _service?.IsConnected == true && e.Data.GetDataPresent(DataFormats.FileDrop)
                ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private async void FileList_DragDrop(object sender, DragEventArgs e)
        {
            if (_service?.IsConnected != true || !e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var file in files)
            {
                if (!File.Exists(file)) continue;
                try
                {
                    var fileName = Path.GetFileName(file);
                    long fileSize = new FileInfo(file).Length;
                    _lblStatus.Text = $"Uploading {fileName}...";
                    ShowProgress(0);
                    await _service.UploadFileAsync(file, _service.CurrentPath.TrimEnd(RemotePathSeparator) + RemotePathSeparator + fileName, bytes =>
                    {
                        if (fileSize > 0)
                            BeginInvoke(() => ShowProgress((int)(bytes * 100 / (ulong)fileSize)));
                    });
                }
                catch (Exception ex) { _lblStatus.Text = $"Upload failed: {ex.Message}"; }
            }
            HideProgress();
            _suppressHistory = true;
            await NavigateTo(_service.CurrentPath);
        }

        private void TxtPath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; _ = NavigateTo(_txtPath.Text); }
        }

        private void ContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _contextMenu.Items.Clear();
            var item = _fileList.SelectedObject as SftpFileItem;

            if (item == null || item.Name == "..")
            {
                _contextMenu.Items.Add("Upload file...", Properties.Resources.GlyphUp_16x, OnUploadClick);
                _contextMenu.Items.Add(new ToolStripSeparator());
                _contextMenu.Items.Add("New file...", Properties.Resources.NewFile_16x, OnNewFileClick);
                _contextMenu.Items.Add("New folder...", Properties.Resources.AddFolder_16x, OnNewFolderClick);
                _contextMenu.Items.Add(new ToolStripSeparator());
                _contextMenu.Items.Add("Refresh", Properties.Resources.Refresh_16x, OnRefreshClick);
                return;
            }

            if (item.IsDirectory)
            {
                _contextMenu.Items.Add("Open", Properties.Resources.FolderClosed_16x, (s, a) => _ = NavigateTo(item.FullPath));
                _contextMenu.Items.Add(new ToolStripSeparator());
            }

            if (!item.IsDirectory)
            {
                _contextMenu.Items.Add("Download...", Properties.Resources.GlyphDown_16x, OnDownloadClick);
                if (IsTextFile(item.Name))
                    _contextMenu.Items.Add("Edit", Properties.Resources.OpenFile_16x, OnEditFileClick);
                _contextMenu.Items.Add(new ToolStripSeparator());
            }

            _contextMenu.Items.Add("Rename...", Properties.Resources.Rename_16x, OnRenameClick);
            _contextMenu.Items.Add("Delete", Properties.Resources.Close_16x, OnDeleteClick);
            _contextMenu.Items.Add(new ToolStripSeparator());
            _contextMenu.Items.Add("Copy path", Properties.Resources.Copy_16x, OnCopyPathClick);
        }

        private void FileList_FormatRow(object sender, FormatRowEventArgs e)
        {
            if (e.Model is not SftpFileItem item) return;

            if (item.IsDirectory)
            {
                e.Item.ForeColor = Color.FromArgb(0, 90, 160); // blue for folders
                return;
            }

            // Check if executable by permissions
            string perms = item.Permissions ?? "";
            bool isExecutable = perms.Length >= 3 && perms.Contains('x');

            if (isExecutable || _execExtensions.Contains(Path.GetExtension(item.Name)))
            {
                e.Item.ForeColor = Color.FromArgb(0, 140, 0); // green for executables
                return;
            }

            if (_archiveExtensions.Contains(Path.GetExtension(item.Name)))
            {
                e.Item.ForeColor = Color.FromArgb(180, 0, 0); // red for archives
                return;
            }

            if (_imageExtensions.Contains(Path.GetExtension(item.Name)))
            {
                e.Item.ForeColor = Color.FromArgb(140, 0, 140); // purple for images
                return;
            }

            if (_configExtensions.Contains(Path.GetExtension(item.Name)))
            {
                e.Item.ForeColor = Color.FromArgb(160, 120, 0); // dark yellow for configs
                return;
            }

            if (item.IsSymlink)
            {
                e.Item.ForeColor = Color.FromArgb(0, 160, 160); // teal for symlinks
            }
        }

        #endregion

        #region Helpers

        private void SetButtonsEnabled(bool enabled)
        {
            _btnHome.Enabled = enabled;
            _btnUp.Enabled = enabled;
            _btnRefresh.Enabled = enabled;
            _btnUpload.Enabled = enabled;
            _btnNewFile.Enabled = enabled;
            _btnNewFolder.Enabled = enabled;
            _btnToggleHidden.Enabled = enabled;
            _btnDownload.Enabled = false;
            _btnDelete.Enabled = false;
            UpdateHistoryButtons();
        }

        private void UpdateHistoryButtons()
        {
            _btnBack.Enabled = _historyBack.Count > 0;
            _btnForward.Enabled = _historyForward.Count > 0;
        }

        private void ShowProgress(int percent)
        {
            _progressBar.Value = Math.Clamp(percent, 0, 100);
            _progressBar.Visible = true;
        }

        private void HideProgress()
        {
            _progressBar.Visible = false;
            _progressBar.Value = 0;
        }

        private static readonly HashSet<string> _knownTextFiles = new(StringComparer.OrdinalIgnoreCase)
            { "Makefile", "Dockerfile", "README", "LICENSE", "CHANGELOG", "AUTHORS", "CONTRIBUTING", "COPYING", "INSTALL", "NOTICE" };

        private static bool IsTextFile(string fileName)
        {
            var ext = Path.GetExtension(fileName);
            if (_textExtensions.Contains(ext)) return true;
            var name = Path.GetFileName(fileName);
            if (string.IsNullOrEmpty(ext))
            {
                if (name.StartsWith(".")) return true;
                if (_knownTextFiles.Contains(name)) return true;
            }
            return false;
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _connectTimer?.Stop();
                _connectTimer?.Dispose();
                _service?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
