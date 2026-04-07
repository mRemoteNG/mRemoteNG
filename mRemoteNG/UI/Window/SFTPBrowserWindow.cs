using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms;
using BrightIdeasSoftware;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Messages;
using mRemoteNG.Tools;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Tabs;
using Renci.SshNet.Common;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.UI.Window
{
    [SupportedOSPlatform("windows")]
    public class SFTPBrowserWindow : BaseWindow
    {
        #region Fields

        private SftpFileService _service;
        private ConnectionInfo _currentConnectionInfo;
        private bool _isNavigating;
        private bool _isAutoConnecting;
        private System.Windows.Forms.Timer _autoConnectTimer;

        // Toolbar
        private ToolStrip _toolbar;
        private ToolStripLabel _lblConnection;
        private ToolStripButton _btnDisconnect;
        private ToolStripButton _btnRefresh;
        private ToolStripButton _btnUpload;
        private ToolStripButton _btnDownload;
        private ToolStripButton _btnNewFolder;
        private ToolStripButton _btnDelete;
        private ToolStripButton _btnHome;

        // Path bar
        private Panel _pathPanel;
        private MrngTextBox _txtPath;
        private MrngButton _btnGo;

        // File list
        private MrngListView _fileList;
        private OLVColumn _colName;
        private OLVColumn _colSize;
        private OLVColumn _colModified;
        private OLVColumn _colPermissions;
        private OLVColumn _colOwner;

        // Status bar
        private Panel _statusPanel;
        private MrngLabel _lblStatus;
        private MrngProgressBar _pbProgress;

        // Context menu
        private ContextMenuStrip _contextMenu;

        #endregion

        #region Constructor

        public SFTPBrowserWindow()
        {
            WindowType = WindowType.SFTPBrowser;
            DockPnl = new DockContent();
            InitializeComponent();
        }

        #endregion

        #region InitializeComponent

        private void InitializeComponent()
        {
            SuspendLayout();

            // Toolbar
            _toolbar = new ToolStrip();
            _lblConnection = new ToolStripLabel("Not connected");
            _btnHome = new ToolStripButton("Home", null, OnHomeClick) { ToolTipText = "Home directory" };
            _btnRefresh = new ToolStripButton("Refresh", null, OnRefreshClick) { ToolTipText = "Refresh" };
            _btnUpload = new ToolStripButton("Upload", null, OnUploadClick) { ToolTipText = "Upload file", Enabled = false };
            _btnDownload = new ToolStripButton("Download", null, OnDownloadClick) { ToolTipText = "Download selected", Enabled = false };
            _btnNewFolder = new ToolStripButton("New Folder", null, OnNewFolderClick) { ToolTipText = "Create directory", Enabled = false };
            _btnDelete = new ToolStripButton("Delete", null, OnDeleteClick) { ToolTipText = "Delete selected", Enabled = false };
            _btnDisconnect = new ToolStripButton("Disconnect", null, OnDisconnectClick) { ToolTipText = "Disconnect SFTP", Enabled = false };

            _toolbar.Items.AddRange(new ToolStripItem[]
            {
                _lblConnection,
                new ToolStripSeparator(),
                _btnHome,
                _btnRefresh,
                new ToolStripSeparator(),
                _btnUpload,
                _btnDownload,
                _btnNewFolder,
                _btnDelete,
                new ToolStripSeparator(),
                _btnDisconnect
            });
            _toolbar.Dock = DockStyle.Top;
            _toolbar.GripStyle = ToolStripGripStyle.Hidden;

            // Path bar
            _pathPanel = new Panel { Height = 28, Dock = DockStyle.Top, Padding = new Padding(2) };
            _txtPath = new MrngTextBox
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 8.25F)
            };
            _txtPath.KeyDown += TxtPath_KeyDown;
            _btnGo = new MrngButton
            {
                Text = "Go",
                Width = 40,
                Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat
            };
            _btnGo.Click += OnGoClick;
            _pathPanel.Controls.Add(_txtPath);
            _pathPanel.Controls.Add(_btnGo);

            // Image list for folder/file icons
            var imageList = new ImageList { ImageSize = new Size(16, 16) };
            imageList.Images.Add("folder", Properties.Resources.FolderClosed_16x);
            imageList.Images.Add("file", Properties.Resources.DocumentsFolder_16x);

            // File list
            _fileList = new MrngListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                MultiSelect = false,
                AllowDrop = true,
                ShowGroups = false,
                SmallImageList = imageList,
                CheckBoxes = false
            };

            _colName = new OLVColumn("Name", "") { Width = 220, IsEditable = false };
            _colName.AspectGetter = obj => ((SftpFileItem)obj).Name;
            _colName.ImageGetter = obj => ((SftpFileItem)obj).IsDirectory ? "folder" : "file";

            _colSize = new OLVColumn("Size", "") { Width = 70, IsEditable = false, TextAlign = HorizontalAlignment.Right };
            _colSize.AspectGetter = obj =>
            {
                var item = (SftpFileItem)obj;
                if (item.IsDirectory) return "";
                long size = item.Size;
                if (size < 1024) return $"{size} B";
                if (size < 1024 * 1024) return $"{size / 1024.0:F1} KB";
                if (size < 1024 * 1024 * 1024) return $"{size / (1024.0 * 1024.0):F1} MB";
                return $"{size / (1024.0 * 1024.0 * 1024.0):F1} GB";
            };

            _colOwner = new OLVColumn("Owner", "") { Width = 60, IsEditable = false };
            _colOwner.AspectGetter = obj => ((SftpFileItem)obj).Owner;

            _colModified = new OLVColumn("Modified", "") { Width = 130, IsEditable = false };
            _colModified.AspectGetter = obj =>
            {
                var dt = ((SftpFileItem)obj).LastModified;
                return dt == DateTime.MinValue ? "" : dt.ToString("yyyy-MM-dd HH:mm");
            };

            _colPermissions = new OLVColumn("Permissions", "") { Width = 80, IsEditable = false };
            _colPermissions.AspectGetter = obj => ((SftpFileItem)obj).Permissions;

            _fileList.AllColumns.AddRange(new[] { _colName, _colSize, _colOwner, _colModified, _colPermissions });
            _fileList.Columns.AddRange(new ColumnHeader[] { _colName, _colSize, _colOwner, _colModified, _colPermissions });

            _fileList.DoubleClick += FileList_DoubleClick;
            _fileList.SelectedIndexChanged += FileList_SelectedIndexChanged;
            _fileList.DragEnter += FileList_DragEnter;
            _fileList.DragDrop += FileList_DragDrop;

            // Context menu
            _contextMenu = new ContextMenuStrip();
            _contextMenu.Items.Add("Download", null, OnDownloadClick);
            _contextMenu.Items.Add("Rename", null, OnRenameClick);
            _contextMenu.Items.Add(new ToolStripSeparator());
            _contextMenu.Items.Add("Delete", null, OnDeleteClick);
            _fileList.ContextMenuStrip = _contextMenu;

            // Status bar
            _statusPanel = new Panel { Height = 24, Dock = DockStyle.Bottom, Padding = new Padding(2) };
            _lblStatus = new MrngLabel
            {
                Text = "Not connected",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 8.25F)
            };
            _pbProgress = new MrngProgressBar
            {
                Width = 120,
                Dock = DockStyle.Right,
                Style = ProgressBarStyle.Continuous,
                Visible = false
            };
            _statusPanel.Controls.Add(_lblStatus);
            _statusPanel.Controls.Add(_pbProgress);

            // Main form
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Controls.Add(_fileList);
            Controls.Add(_pathPanel);
            Controls.Add(_toolbar);
            Controls.Add(_statusPanel);
            Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Icon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.DocumentsFolder_16x);
            Name = "SFTPBrowserWindow";
            TabText = "SFTP Browser";
            Text = "SFTP Browser";
            Load += SFTPBrowser_Load;
            FormClosing += SFTPBrowser_FormClosing;

            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        #region Event Handlers - Form

        private void SFTPBrowser_Load(object sender, EventArgs e)
        {
            ApplyTheme();

            // Subscribe to active document changes to auto-connect
            try
            {
                FrmMain.Default.pnlDock.ActiveDocumentChanged += PnlDock_ActiveDocumentChanged;
                FrmMain.Default.pnlDock.ActiveContentChanged += PnlDock_ActiveDocumentChanged;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("SFTPBrowser: Failed to subscribe to ActiveDocumentChanged", ex);
            }

            // Timer to retry auto-connect (handles case where SSH tab opens but connection isn't ready yet)
            _autoConnectTimer = new System.Windows.Forms.Timer { Interval = 3000 };
            _autoConnectTimer.Tick += AutoConnectTimer_Tick;
            _autoConnectTimer.Start();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (!(_service?.IsConnected == true) && !_isAutoConnecting)
            {
                _autoConnectTimer?.Start();
            }
        }

        private async void AutoConnectTimer_Tick(object sender, EventArgs e)
        {
            if (_isAutoConnecting) return;

            var connectionInfo = GetActiveSSHConnectionInfo();
            if (connectionInfo == null) return;

            // Already connected to this exact connection — nothing to do
            if (_service?.IsConnected == true &&
                _currentConnectionInfo != null &&
                ReferenceEquals(connectionInfo, _currentConnectionInfo))
                return;

            _isAutoConnecting = true;
            try
            {
                await ConnectToHost(connectionInfo);
            }
            finally
            {
                _isAutoConnecting = false;
            }
        }

        private void SFTPBrowser_FormClosing(object sender, FormClosingEventArgs e)
        {
            _autoConnectTimer?.Stop();
            _autoConnectTimer?.Dispose();
            try
            {
                FrmMain.Default.pnlDock.ActiveDocumentChanged -= PnlDock_ActiveDocumentChanged;
                FrmMain.Default.pnlDock.ActiveContentChanged -= PnlDock_ActiveDocumentChanged;
            }
            catch { }

            DisconnectFromHost();
        }

        #endregion

        #region Auto-Connect

        private void PnlDock_ActiveDocumentChanged(object sender, EventArgs e)
        {
            var connectionInfo = GetActiveSSHConnectionInfo();
            if (connectionInfo == null)
                return;

            // Skip if already connected to this exact connection
            if (_service?.IsConnected == true &&
                _currentConnectionInfo != null &&
                ReferenceEquals(connectionInfo, _currentConnectionInfo))
                return;

            _ = ConnectToHost(connectionInfo);
        }

        private ConnectionInfo GetActiveSSHConnectionInfo()
        {
            try
            {
                // Try the dock panel's active document first (most accurate for tab switches)
                if (FrmMain.Default.pnlDock.ActiveDocument is ConnectionTab activeTab)
                {
                    var ifc = InterfaceControl.FindInterfaceControl(activeTab);
                    if (ifc != null)
                    {
                        var info = ifc.OriginalInfo ?? ifc.Info;
                        if (info.Protocol == ProtocolType.SSH1 || info.Protocol == ProtocolType.SSH2)
                            return info;
                    }
                }

                // Try TabHelper (tracks last focused tab, works across connection windows)
                var tab = TabHelper.Instance.CurrentTab;
                if (tab != null)
                {
                    var ifc2 = InterfaceControl.FindInterfaceControl(tab);
                    if (ifc2 != null)
                    {
                        var info2 = ifc2.OriginalInfo ?? ifc2.Info;
                        if (info2.Protocol == ProtocolType.SSH1 || info2.Protocol == ProtocolType.SSH2)
                            return info2;
                    }
                }

                // Fallback: scan all open documents for any SSH connection
                foreach (var doc in FrmMain.Default.pnlDock.Documents)
                {
                    if (doc is ConnectionTab ct)
                    {
                        var ifc3 = InterfaceControl.FindInterfaceControl(ct);
                        if (ifc3 == null) continue;
                        var info3 = ifc3.OriginalInfo ?? ifc3.Info;
                        if (info3.Protocol == ProtocolType.SSH1 || info3.Protocol == ProtocolType.SSH2)
                            return info3;
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Public Methods

        public async System.Threading.Tasks.Task ConnectToHost(ConnectionInfo connectionInfo)
        {
            await ConnectToHost(connectionInfo.Hostname, connectionInfo.Username, connectionInfo.Password, connectionInfo.Port);
            if (_service?.IsConnected == true)
                _currentConnectionInfo = connectionInfo;
        }

        public async System.Threading.Tasks.Task ConnectToHost(string host, string user, string password, int port)
        {
            try
            {
                // Clean up previous connection if any
                if (_service != null)
                {
                    _service.Dispose();
                    _service = null;
                }

                _service = new SftpFileService();
                UpdateStatus($"Connecting to {user}@{host}:{port}...");

                await System.Threading.Tasks.Task.Run(() => _service.Connect(host, user, password, port));

                UpdateConnectionLabel($"{user}@{host}:{port}");
                SetButtonsEnabled(true);
                await NavigateTo(_service.CurrentPath);
            }
            catch (Exception ex)
            {
                UpdateStatus("Not connected");
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.DebugMsg,
                    $"SFTPBrowser: Connection to {host} failed: {ex.Message}");
                _service?.Dispose();
                _service = null;
            }
        }

        public void DisconnectFromHost()
        {
            _service?.Dispose();
            _service = null;
            _currentConnectionInfo = null;
            _fileList.ClearObjects();
            _txtPath.Text = "";
            UpdateConnectionLabel("Not connected");
            UpdateStatus("Not connected");
            SetButtonsEnabled(false);
        }

        #endregion

        #region Navigation

        private async System.Threading.Tasks.Task NavigateTo(string path)
        {
            if (_service == null || !_service.IsConnected || _isNavigating)
                return;

            _isNavigating = true;
            try
            {
                UpdateStatus($"Loading {path}...");
                var items = await _service.ListDirectoryAsync(path);
                _fileList.SetObjects(items);
                _txtPath.Text = _service.CurrentPath;
                UpdateStatus($"{items.Count(i => !i.IsDirectory || i.Name == "..")} file(s), {items.Count(i => i.IsDirectory && i.Name != "..")} folder(s)");
            }
            catch (SshConnectionException)
            {
                UpdateStatus("Connection lost");
                DisconnectFromHost();
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error: {ex.Message}");
                Runtime.MessageCollector.AddExceptionStackTrace("SFTPBrowser: Navigation failed", ex);
            }
            finally
            {
                _isNavigating = false;
            }
        }

        #endregion

        #region Event Handlers - Toolbar

        private void OnHomeClick(object sender, EventArgs e)
        {
            if (_service?.IsConnected != true) return;
            _ = NavigateTo(_service.HomePath);
        }

        private void OnRefreshClick(object sender, EventArgs e)
        {
            if (_service?.IsConnected != true) return;
            _ = NavigateTo(_service.CurrentPath);
        }

        private async void OnUploadClick(object sender, EventArgs e)
        {
            if (_service?.IsConnected != true) return;
            using var ofd = new OpenFileDialog
            {
                Filter = "All Files (*.*)|*.*",
                Multiselect = true
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;

            foreach (var file in ofd.FileNames)
            {
                var fileName = Path.GetFileName(file);
                var remotePath = _service.CurrentPath.TrimEnd('/') + "/" + fileName;
                try
                {
                    _pbProgress.Visible = true;
                    _pbProgress.Value = 0;
                    UpdateStatus($"Uploading {fileName}...");

                    var fileSize = new FileInfo(file).Length;
                    await _service.UploadFileAsync(file, remotePath, uploaded =>
                    {
                        if (fileSize > 0)
                            SetProgress((int)(uploaded * 100 / (ulong)fileSize));
                    });

                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, $"SFTP: Uploaded {fileName}", true);
                }
                catch (Exception ex)
                {
                    UpdateStatus($"Upload failed: {ex.Message}");
                    Runtime.MessageCollector.AddExceptionStackTrace($"SFTPBrowser: Upload of {fileName} failed", ex);
                }
            }

            _pbProgress.Visible = false;
            await NavigateTo(_service.CurrentPath);
        }

        private async void OnDownloadClick(object sender, EventArgs e)
        {
            var item = _fileList.SelectedObject as SftpFileItem;
            if (item == null || item.IsDirectory || _service?.IsConnected != true) return;

            using var sfd = new SaveFileDialog
            {
                FileName = item.Name,
                Filter = "All Files (*.*)|*.*"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                _pbProgress.Visible = true;
                _pbProgress.Value = 0;
                UpdateStatus($"Downloading {item.Name}...");

                await _service.DownloadFileAsync(item.FullPath, sfd.FileName, downloaded =>
                {
                    if (item.Size > 0)
                        SetProgress((int)(downloaded * 100 / (ulong)item.Size));
                });

                UpdateStatus($"Downloaded {item.Name}");
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, $"SFTP: Downloaded {item.Name}", true);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Download failed: {ex.Message}");
                Runtime.MessageCollector.AddExceptionStackTrace($"SFTPBrowser: Download of {item.Name} failed", ex);
            }
            finally
            {
                _pbProgress.Visible = false;
            }
        }

        private async void OnNewFolderClick(object sender, EventArgs e)
        {
            if (_service?.IsConnected != true) return;

            var name = PromptInput("New Folder", "Enter folder name:");
            if (string.IsNullOrWhiteSpace(name)) return;

            try
            {
                var path = _service.CurrentPath.TrimEnd('/') + "/" + name;
                await _service.CreateDirectoryAsync(path);
                await NavigateTo(_service.CurrentPath);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Create folder failed: {ex.Message}");
            }
        }

        private async void OnDeleteClick(object sender, EventArgs e)
        {
            var item = _fileList.SelectedObject as SftpFileItem;
            if (item == null || item.Name == ".." || _service?.IsConnected != true) return;

            var result = MessageBox.Show(
                $"Delete '{item.Name}'?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;

            try
            {
                UpdateStatus($"Deleting {item.Name}...");
                await _service.DeleteAsync(item.FullPath, item.IsDirectory);
                await NavigateTo(_service.CurrentPath);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Delete failed: {ex.Message}");
            }
        }

        private void OnDisconnectClick(object sender, EventArgs e)
        {
            DisconnectFromHost();
        }

        private async void OnRenameClick(object sender, EventArgs e)
        {
            var item = _fileList.SelectedObject as SftpFileItem;
            if (item == null || item.Name == ".." || _service?.IsConnected != true) return;

            var newName = PromptInput("Rename", "Enter new name:", item.Name);
            if (string.IsNullOrWhiteSpace(newName) || newName == item.Name) return;

            try
            {
                var dir = _service.CurrentPath.TrimEnd('/');
                await _service.RenameAsync(item.FullPath, dir + "/" + newName);
                await NavigateTo(_service.CurrentPath);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Rename failed: {ex.Message}");
            }
        }

        #endregion

        #region Event Handlers - File List

        private void FileList_DoubleClick(object sender, EventArgs e)
        {
            var item = _fileList.SelectedObject as SftpFileItem;
            if (item == null || _service?.IsConnected != true) return;

            if (item.IsDirectory)
            {
                if (item.Name == "..")
                {
                    var parent = _service.CurrentPath.TrimEnd('/');
                    var lastSlash = parent.LastIndexOf('/');
                    var parentPath = lastSlash > 0 ? parent.Substring(0, lastSlash) : "/";
                    _ = NavigateTo(parentPath);
                }
                else
                {
                    _ = NavigateTo(item.FullPath);
                }
            }
        }

        private void FileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = _fileList.SelectedObject as SftpFileItem;
            var hasSelection = item != null && item.Name != "..";
            _btnDownload.Enabled = hasSelection && item != null && !item.IsDirectory;
            _btnDelete.Enabled = hasSelection;
        }

        private void FileList_DragEnter(object sender, DragEventArgs e)
        {
            if (_service?.IsConnected == true && e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private async void FileList_DragDrop(object sender, DragEventArgs e)
        {
            if (_service?.IsConnected != true) return;
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var file in files)
            {
                if (!File.Exists(file)) continue;
                var fileName = Path.GetFileName(file);
                var remotePath = _service.CurrentPath.TrimEnd('/') + "/" + fileName;
                try
                {
                    UpdateStatus($"Uploading {fileName}...");
                    await _service.UploadFileAsync(file, remotePath);
                }
                catch (Exception ex)
                {
                    UpdateStatus($"Upload failed: {ex.Message}");
                }
            }
            await NavigateTo(_service.CurrentPath);
        }

        #endregion

        #region Event Handlers - Path Bar

        private void TxtPath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                _ = NavigateTo(_txtPath.Text);
            }
        }

        private void OnGoClick(object sender, EventArgs e)
        {
            _ = NavigateTo(_txtPath.Text);
        }

        #endregion

        #region UI Helpers

        private void UpdateConnectionLabel(string text)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateConnectionLabel(text)));
                return;
            }
            _lblConnection.Text = text;
            TabText = _service?.IsConnected == true ? $"SFTP - {text}" : "SFTP Browser";
            Text = TabText;
        }

        private void UpdateStatus(string text)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateStatus(text)));
                return;
            }
            _lblStatus.Text = text;
        }

        private void SetProgress(int percent)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => SetProgress(percent)));
                return;
            }
            _pbProgress.Value = Math.Min(100, Math.Max(0, percent));
        }

        private void SetButtonsEnabled(bool enabled)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => SetButtonsEnabled(enabled)));
                return;
            }
            _btnDisconnect.Enabled = enabled;
            _btnRefresh.Enabled = enabled;
            _btnUpload.Enabled = enabled;
            _btnNewFolder.Enabled = enabled;
            _btnHome.Enabled = enabled;
            _btnDownload.Enabled = false;
            _btnDelete.Enabled = false;
        }

        private static string PromptInput(string title, string prompt, string defaultValue = "")
        {
            using var form = new Form
            {
                Text = title,
                Width = 350,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lbl = new Label { Left = 10, Top = 15, Width = 310, Text = prompt };
            var txt = new TextBox { Left = 10, Top = 40, Width = 310, Text = defaultValue };
            var btnOk = new Button { Text = "OK", Left = 160, Top = 75, Width = 75, DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "Cancel", Left = 245, Top = 75, Width = 75, DialogResult = DialogResult.Cancel };

            form.Controls.AddRange(new Control[] { lbl, txt, btnOk, btnCancel });
            form.AcceptButton = btnOk;
            form.CancelButton = btnCancel;

            return form.ShowDialog() == DialogResult.OK ? txt.Text : null;
        }

        #endregion
    }
}
