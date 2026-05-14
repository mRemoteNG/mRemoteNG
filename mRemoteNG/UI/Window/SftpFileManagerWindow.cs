using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Properties;
using mRemoteNG.Resources.Language;
using mRemoteNG.Tools;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Forms;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.UI.Window
{
    [SupportedOSPlatform("windows")]
    public class SftpFileManagerWindow : BaseWindow
    {
        #region Fields

        private SftpClient _sftpClient;
        private SshClient _sshClient;  // Shared SSH client for connection reuse
        private bool _isConnected;
        private string _remoteCurrentPath = "/";
        private string _localCurrentPath;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _useExistingSession;
        private int _localSortColumn = -1;
        private bool _localSortDescending;
        private int _remoteSortColumn = -1;
        private bool _remoteSortDescending;
        private ImageList _localImageList;
        private ImageList _remoteImageList;

        #endregion

        #region Controls

        private ToolStrip _toolStrip;
        private ToolStripButton _btnConnect;
        private ToolStripButton _btnDisconnect;
        private ToolStripTextBox _txtHost;
        private ToolStripTextBox _txtPort;
        private ToolStripTextBox _txtUsername;
        private ToolStripTextBox _txtPassword;
        private ToolStripButton _btnLocalUp;
        private ToolStripButton _btnLocalHome;
        private ToolStripButton _btnLocalBrowse;
        private ToolStripTextBox _txtLocalPath;
        private ToolStripButton _btnRemoteUp;
        private ToolStripButton _btnRemoteHome;
        private ContextMenuStrip _remoteContextMenu;
        private ContextMenuStrip _localContextMenu;
        private ToolStripTextBox _txtRemotePath;
        private SplitContainer _mainSplitContainer;
        private Panel _localPanel;
        private Panel _remotePanel;
        private ListView _localListView;
        private ListView _remoteListView;
        private ColumnHeader _localColName;
        private ColumnHeader _localColSize;
        private ColumnHeader _localColModified;
        private ColumnHeader _remoteColName;
        private ColumnHeader _remoteColSize;
        private ColumnHeader _remoteColModified;
        private Panel _statusPanel;
        private Label _lblLocalStatus;
        private Label _lblRemoteStatus;
        private Label _lblTransferStatus;
        private ProgressBar _progressBar;
        private Panel _queuePanel;
        private ListView _transferQueue;
        private ColumnHeader _queueColFile;
        private ColumnHeader _queueColStatus;
        private ColumnHeader _queueColProgress;

        #endregion

        #region Properties

        public string Hostname
        {
            get => _txtHost?.Text ?? "";
            set
            {
                if (_txtHost != null)
                    _txtHost.Text = value;
            }
        }

        public string Port
        {
            get => _txtPort?.Text ?? "22";
            set
            {
                if (_txtPort != null)
                    _txtPort.Text = value;
            }
        }

        public string Username
        {
            get => _txtUsername?.Text ?? "";
            set
            {
                if (_txtUsername != null)
                    _txtUsername.Text = value;
            }
        }

        public string Password
        {
            set
            {
                if (_txtPassword != null)
                    _txtPassword.Text = value;
            }
        }

        public Renci.SshNet.ConnectionInfo ConnectionInfo { get; set; }

        #endregion

        #region Constructor

        public SftpFileManagerWindow()
        {
            WindowType = WindowType.SftpFileManager;
            DockPnl = new DockContent();
            InitializeComponent();

            if (ConnectionInfo != null)
            {
                LoadFromConnectionInfo();
            }
        }

        #endregion

        #region InitializeComponent

        private void InitializeComponent()
        {
            SuspendLayout();

            // Initialize controls
            InitializeToolStrip();
            InitializePanels();
            InitializeLocalListView();
            InitializeRemoteListView();
            InitializeStatusPanel();
            InitializeQueuePanel();
            InitializeMainSplitContainer();

            // Form properties
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(1000, 600);
            MinimizeBox = false;
            MaximizeBox = true;
            MinimumSize = new Size(800, 500);
            StartPosition = FormStartPosition.CenterParent;

            ApplyLanguage();
            ApplyTheme();
            Load += SftpFileManager_Load;

            ResumeLayout(false);
            PerformLayout();
        }

        private void InitializeToolStrip()
        {
            _toolStrip = new ToolStrip
            {
                GripStyle = ToolStripGripStyle.Hidden,
                ImageScalingSize = new Size(16, 16),
                CanOverflow = false,
                AutoSize = false,
                Height = 30
            };

            // Connection group
            var lblConn = new ToolStripLabel { Text = Language.Connection + ":", Padding = new Padding(5, 0, 5, 0) };
            _txtHost = new ToolStripTextBox { Width = 120, ToolTipText = Language.Host };
            _txtPort = new ToolStripTextBox { Width = 50, Text = "22", ToolTipText = Language.Port };
            _txtUsername = new ToolStripTextBox { Width = 100, ToolTipText = Language.User };
            _txtPassword = new ToolStripTextBox { Width = 100, ToolTipText = Language.Password, CausesValidation = false };
            _btnConnect = new ToolStripButton { Text = Language._Launch, Image = Properties.Resources.Run_16x, DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            _btnDisconnect = new ToolStripButton { Text = Language._Close, Image = Properties.Resources.Stop_16x, DisplayStyle = ToolStripItemDisplayStyle.ImageAndText, Enabled = false };

            // Add items
            _toolStrip.Items.Add(lblConn);
            _toolStrip.Items.Add(new ToolStripLabel { Text = Language.Host + ":" });
            _toolStrip.Items.Add(_txtHost);
            _toolStrip.Items.Add(new ToolStripLabel { Text = Language.Port + ":" });
            _toolStrip.Items.Add(_txtPort);
            _toolStrip.Items.Add(new ToolStripLabel { Text = Language.User + ":" });
            _toolStrip.Items.Add(_txtUsername);
            _toolStrip.Items.Add(new ToolStripLabel { Text = Language.Password + ":" });
            _toolStrip.Items.Add(_txtPassword);
            _toolStrip.Items.Add(_btnDisconnect);

            // Event handlers
            _btnConnect.Click += BtnConnect_Click;
            _btnDisconnect.Click += BtnDisconnect_Click;

            Controls.Add(_toolStrip);
        }

        private void InitializePanels()
        {
            _localPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(2)
            };

            _remotePanel = new Panel
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(2)
            };

            // Local path bar
            var localPathBar = new ToolStrip
            {
                GripStyle = ToolStripGripStyle.Hidden,
                ImageScalingSize = new Size(16, 16),
                CanOverflow = false,
                AutoSize = false,
                Height = 28
            };
            _btnLocalUp = new ToolStripButton { Text = "^", ToolTipText = "Go to parent folder", Width = 25 };
            _btnLocalHome = new ToolStripButton { Text = "~", ToolTipText = "Go to home folder", Width = 25 };
            _btnLocalBrowse = new ToolStripButton { Text = "...", ToolTipText = "Browse for folder", Width = 30 };
            _btnLocalBrowse.Click += BtnLocalBrowse_Click;
            _txtLocalPath = new ToolStripTextBox { Width = 250, Text = "" };
            _txtLocalPath.KeyDown += LocalPath_KeyDown;
            _btnLocalUp.Click += (s, e) => NavigateLocalParent();
            _btnLocalHome.Click += (s, e) => NavigateLocalHome();
            localPathBar.Items.Add(_btnLocalUp);
            localPathBar.Items.Add(_btnLocalHome);
            localPathBar.Items.Add(_btnLocalBrowse);
            localPathBar.Items.Add(new ToolStripSeparator { Width = 5 });
            localPathBar.Items.Add(_txtLocalPath);

            _localListView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                AllowColumnReorder = true,
                LabelEdit = false,
                HideSelection = false,
                SmallImageList = _localImageList
            };
            _localListView.DoubleClick += LocalListView_DoubleClick;
            _localListView.ColumnClick += LocalListView_ColumnClick;
            _localListView.KeyDown += LocalListView_KeyDown;

            // Use TableLayoutPanel for local panel - path bar on top, list below
            var localTableLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowStyles = { new RowStyle(SizeType.Absolute, 28), new RowStyle(SizeType.Percent, 100) },
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 100) }
            };
            localTableLayout.Controls.Add(localPathBar, 0, 0);
            localTableLayout.Controls.Add(_localListView, 0, 1);
            _localPanel.Controls.Add(localTableLayout);

            // Remote path bar
            var remotePathBar = new ToolStrip
            {
                GripStyle = ToolStripGripStyle.Hidden,
                ImageScalingSize = new Size(16, 16),
                CanOverflow = false,
                AutoSize = false,
                Height = 28
            };
            _btnRemoteUp = new ToolStripButton { Text = "^", ToolTipText = "Go to parent folder", Width = 25 };
            _btnRemoteHome = new ToolStripButton { Text = "~", ToolTipText = "Go to home folder", Width = 25 };
            _txtRemotePath = new ToolStripTextBox { Width = 300, Text = _remoteCurrentPath };
            _txtRemotePath.KeyDown += RemotePath_KeyDown;
            _btnRemoteUp.Click += (s, e) => _ = NavigateRemoteParentAsync();
            _btnRemoteHome.Click += (s, e) => _ = NavigateRemoteHomeAsync();
            remotePathBar.Items.Add(_btnRemoteUp);
            remotePathBar.Items.Add(_btnRemoteHome);
            remotePathBar.Items.Add(_txtRemotePath);

            _remoteListView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                AllowColumnReorder = true,
                LabelEdit = false,
                HideSelection = false,
                SmallImageList = _remoteImageList
            };
            _remoteListView.DoubleClick += RemoteListView_DoubleClick;
            _remoteListView.ColumnClick += RemoteListView_ColumnClick;
            _remoteListView.KeyDown += RemoteListView_KeyDown;

            // Use TableLayoutPanel for remote panel - path bar on top, list below
            var remoteTableLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowStyles = { new RowStyle(SizeType.Absolute, 28), new RowStyle(SizeType.Percent, 100) },
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 100) }
            };
            remoteTableLayout.Controls.Add(remotePathBar, 0, 0);
            remoteTableLayout.Controls.Add(_remoteListView, 0, 1);
            _remotePanel.Controls.Add(remoteTableLayout);

            // Status bar
            _statusPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 30,
                Padding = new Padding(5, 3, 5, 3)
            };

            _lblLocalStatus = new Label
            {
                Dock = DockStyle.Left,
                Text = "Local: Ready",
                AutoSize = false,
                Width = 200,
                TextAlign = ContentAlignment.MiddleLeft
            };

            _lblRemoteStatus = new Label
            {
                Dock = DockStyle.Left,
                Text = "Remote: Not Connected",
                AutoSize = false,
                Width = 250,
                TextAlign = ContentAlignment.MiddleLeft
            };

            _lblTransferStatus = new Label
            {
                Dock = DockStyle.Fill,
                Text = "",
                TextAlign = ContentAlignment.MiddleCenter
            };

            _progressBar = new ProgressBar
            {
                Dock = DockStyle.Right,
                Width = 150,
                Style = ProgressBarStyle.Continuous,
                Visible = false
            };

            _statusPanel.Controls.Add(_lblTransferStatus);
            _statusPanel.Controls.Add(_progressBar);
            _statusPanel.Controls.Add(_lblRemoteStatus);
            _statusPanel.Controls.Add(_lblLocalStatus);

            // Queue panel
            _queuePanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 120,
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };

            var queueLabel = new Label
            {
                Dock = DockStyle.Top,
                Text = "Transfer Queue",
                TextAlign = ContentAlignment.MiddleLeft,
                Height = 22,
                Padding = new Padding(5, 0, 0, 0)
            };

            _transferQueue = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true
            };
            _transferQueue.Columns.Add("File", 250);
            _transferQueue.Columns.Add("Direction", 70);
            _transferQueue.Columns.Add("Size", 80);
            _transferQueue.Columns.Add("Status", 80);
            _transferQueue.Columns.Add("Progress", 100);

            _queuePanel.Controls.Add(_transferQueue);
            _queuePanel.Controls.Add(queueLabel);
        }

        private void InitializeLocalListView()
        {
            _localImageList = new ImageList
            {
                ImageSize = new Size(16, 16),
                ColorDepth = ColorDepth.Depth32Bit
            };
            _localImageList.Images.Add("folder", Properties.Resources.FolderClosed_16x);
            _localImageList.Images.Add("parent", Properties.Resources.GlyphUp_16x);
            _localImageList.Images.Add("file", Properties.Resources.Document_16x);

            _localColName = new ColumnHeader { Text = "Name", Width = 200 };
            _localColSize = new ColumnHeader { Text = "Size", Width = 80, TextAlign = HorizontalAlignment.Right };
            _localColModified = new ColumnHeader { Text = "Modified", Width = 130 };

            _localListView.Columns.AddRange(new ColumnHeader[] { _localColName, _localColSize, _localColModified });

            InitializeLocalContextMenu();
        }

        private void InitializeLocalContextMenu()
        {
            _localContextMenu = new ContextMenuStrip();

            var uploadMenuItem = new ToolStripMenuItem("Upload")
            {
                Image = Properties.Resources.GlyphUp_16x
            };
            uploadMenuItem.Click += BtnUpload_Click;

            var refreshMenuItem = new ToolStripMenuItem(Language._Scan)
            {
                Image = Properties.Resources.Refresh_16x
            };
            refreshMenuItem.Click += (s, e) => RefreshLocalList();

            _localContextMenu.Items.Add(uploadMenuItem);
            _localContextMenu.Items.Add(new ToolStripSeparator());
            _localContextMenu.Items.Add(refreshMenuItem);

            _localListView.ContextMenuStrip = _localContextMenu;
        }

        private void InitializeRemoteListView()
        {
            _remoteImageList = new ImageList
            {
                ImageSize = new Size(16, 16),
                ColorDepth = ColorDepth.Depth32Bit
            };
            _remoteImageList.Images.Add("folder", Properties.Resources.FolderClosed_16x);
            _remoteImageList.Images.Add("parent", Properties.Resources.GlyphUp_16x);
            _remoteImageList.Images.Add("file", Properties.Resources.Document_16x);

            _remoteColName = new ColumnHeader { Text = "Name", Width = 200 };
            _remoteColSize = new ColumnHeader { Text = "Size", Width = 80, TextAlign = HorizontalAlignment.Right };
            _remoteColModified = new ColumnHeader { Text = "Modified", Width = 130 };

            _remoteListView.Columns.AddRange(new ColumnHeader[] { _remoteColName, _remoteColSize, _remoteColModified });

            InitializeRemoteContextMenu();
        }

        private void InitializeRemoteContextMenu()
        {
            _remoteContextMenu = new ContextMenuStrip();

            var downloadMenuItem = new ToolStripMenuItem(Language.Download)
            {
                Image = Properties.Resources.GlyphDown_16x
            };
            downloadMenuItem.Click += BtnDownload_Click;

            var deleteMenuItem = new ToolStripMenuItem(Language._Delete)
            {
                Image = Properties.Resources.Remove_16x
            };
            deleteMenuItem.Click += async (s, e) => await DeleteSelectedRemoteFilesAsync();

            var refreshMenuItem = new ToolStripMenuItem(Language._Scan)
            {
                Image = Properties.Resources.Refresh_16x
            };
            refreshMenuItem.Click += async (s, e) => await RefreshRemoteListAsync();

            var newFolderMenuItem = new ToolStripMenuItem(Language._New)
            {
                Image = Properties.Resources.AddFolder_16x
            };
            newFolderMenuItem.Click += async (s, e) => await CreateNewRemoteFolderAsync();

            _remoteContextMenu.Items.Add(downloadMenuItem);
            _remoteContextMenu.Items.Add(new ToolStripSeparator());
            _remoteContextMenu.Items.Add(newFolderMenuItem);
            _remoteContextMenu.Items.Add(deleteMenuItem);
            _remoteContextMenu.Items.Add(refreshMenuItem);

            _remoteListView.ContextMenuStrip = _remoteContextMenu;
        }

        private void InitializeStatusPanel()
        {
            // Status panel is initialized in InitializePanels
        }

        private void InitializeQueuePanel()
        {
            // Queue panel is initialized in InitializePanels
        }

        private void InitializeMainSplitContainer()
        {
            _mainSplitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 450,
                IsSplitterFixed = false
            };

            var topPanel = new Panel { Dock = DockStyle.Fill };
            var splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 500,
                IsSplitterFixed = false
            };

            var localHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 25
            };
            var lblLocal = new Label
            {
                Dock = DockStyle.Fill,
                Text = "Local",
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5, 0, 0, 0),
                Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold)
            };
            localHeader.Controls.Add(lblLocal);

            var remoteHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 25
            };
            var lblRemote = new Label
            {
                Dock = DockStyle.Fill,
                Text = "Remote (SFTP)",
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5, 0, 0, 0),
                Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold)
            };
            remoteHeader.Controls.Add(lblRemote);

            var localContainer = new Panel { Dock = DockStyle.Fill };
            // Use _localPanel which already contains the path bar and list view
            localContainer.Controls.Add(_localPanel);
            localContainer.Controls.Add(localHeader);
            localHeader.SendToBack(); // Keep header on top

            var remoteContainer = new Panel { Dock = DockStyle.Fill };
            remoteContainer.Controls.Add(_remotePanel);
            remoteContainer.Controls.Add(remoteHeader);
            remoteHeader.SendToBack(); // Keep header on top

            splitContainer.Panel1.Controls.Add(localContainer);
            splitContainer.Panel2.Controls.Add(remoteContainer);

            topPanel.Controls.Add(splitContainer);

            _mainSplitContainer.Panel1.Controls.Add(topPanel);
            _mainSplitContainer.Panel2.Controls.Add(_queuePanel);
            _mainSplitContainer.Panel2.Controls.Add(_statusPanel);

            Controls.Add(_mainSplitContainer);
            Controls.Add(_toolStrip);
        }

        #endregion

        #region Event Handlers - Connection

        private async void BtnConnect_Click(object sender, EventArgs e)
        {
            if (_isConnected)
            {
                Disconnect();
                return;
            }

            if (string.IsNullOrWhiteSpace(_txtHost.Text) ||
                string.IsNullOrWhiteSpace(_txtUsername.Text))
            {
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, "Please fill in host and username.");
                return;
            }

            await ConnectAsync();
        }

        private void BtnDisconnect_Click(object sender, EventArgs e)
        {
            Disconnect();
        }

        private async Task ConnectAsync()
        {
            SetConnectionUI(false);
            UpdateStatusText(_lblRemoteStatus, "Remote: Connecting...");

            try
            {
                if (ConnectionInfo != null)
                {
                    _sftpClient = new SftpClient(ConnectionInfo);
                }
                else
                {
                    int port = 22;
                    if (!int.TryParse(_txtPort.Text, out port))
                        port = 22;

                    var connectionInfo = new Renci.SshNet.ConnectionInfo(
                        _txtHost.Text,
                        port,
                        _txtUsername.Text,
                        new PasswordAuthenticationMethod(_txtUsername.Text, _txtPassword.Text))
                    {
                        Timeout = TimeSpan.FromSeconds(30)
                    };
                    _sftpClient = new SftpClient(connectionInfo);
                }

                await Task.Run(() => _sftpClient.Connect());

                _isConnected = true;
                _remoteCurrentPath = GetRemoteHomeDirectory();
                _txtRemotePath.Text = _remoteCurrentPath;

                UpdateConnectedUI();

                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, $"Connected to {_txtHost.Text} via SFTP");

                await RefreshRemoteListAsync();
            }
            catch (OperationCanceledException)
            {
                UpdateStatusText(_lblRemoteStatus, "Remote: Connection cancelled");
                SetConnectionUI(true);
            }
            catch (Renci.SshNet.Common.SshOperationTimeoutException)
            {
                UpdateStatusText(_lblRemoteStatus, "Remote: Connection timeout");
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, "SFTP connection timed out");
                MessageBox.Show("Connection timed out. Please check the host address and try again.", "Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                SetConnectionUI(true);
            }
            catch (Renci.SshNet.Common.SshAuthenticationException ex)
            {
                UpdateStatusText(_lblRemoteStatus, "Remote: Authentication failed");
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, $"SFTP authentication failed: {ex.Message}");
                MessageBox.Show("Authentication failed. Please check your username and password.", "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                SetConnectionUI(true);
            }
            catch (Exception ex)
            {
                UpdateStatusText(_lblRemoteStatus, "Remote: Connection failed");
                Runtime.MessageCollector.AddExceptionStackTrace("SFTP connection failed", ex);
                MessageBox.Show($"Connection failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetConnectionUI(true);
            }
        }

        private void Disconnect()
        {
            try
            {
                _sftpClient?.Disconnect();
                _sftpClient?.Dispose();
                _sftpClient = null;
                _isConnected = false;

                _btnConnect.Text = Language._Launch;
                _btnConnect.Image = Properties.Resources.Run_16x;
                _btnDisconnect.Enabled = false;

                _remoteListView.Items.Clear();
                UpdateStatusText(_lblRemoteStatus, "Remote: Not Connected");
                
                _useExistingSession = false;

                SetConnectionUI(true);
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Disconnected from SFTP");
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("SFTP disconnect error", ex);
            }
        }

        private void SetConnectionUI(bool enabled)
        {
            _txtHost.Enabled = enabled;
            _txtPort.Enabled = enabled;
            _txtUsername.Enabled = enabled;
            _txtPassword.Enabled = enabled;
        }

        private string GetRemoteHomeDirectory()
        {
            try
            {
                return _sftpClient.WorkingDirectory;
            }
            catch
            {
                return "/";
            }
        }

        #endregion

        #region Event Handlers - Local File Operations

        private void LocalListView_DoubleClick(object sender, EventArgs e)
        {
            if (_localListView.SelectedItems.Count == 0) return;

            var item = _localListView.SelectedItems[0];
            var fullPath = Path.Combine(_localCurrentPath, item.Text);

            if (Directory.Exists(fullPath))
            {
                SetLocalPath(fullPath);
            }
        }

        private void LocalListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == _localSortColumn)
                _localSortDescending = !_localSortDescending;
            else
            {
                _localSortColumn = e.Column;
                _localSortDescending = false;
            }
            RefreshLocalList();
        }

        private void LocalListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                NavigateLocalParent();
                e.Handled = true;
            }
        }

        private void LocalPath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var path = _txtLocalPath.Text.Trim();
                if (Directory.Exists(path))
                {
                    SetLocalPath(path);
                }
                else
                {
                    MessageBox.Show("Directory does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void NavigateLocalParent()
        {
            var parent = Directory.GetParent(_localCurrentPath);
            if (parent != null)
            {
                SetLocalPath(parent.FullName);
            }
        }

        private void NavigateLocalHome()
        {
            SetLocalPath(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
        }

        /// <summary>
        /// Sets the local path and saves it to settings.
        /// </summary>
        private void SetLocalPath(string path)
        {
            _localCurrentPath = path;
            _txtLocalPath.Text = _localCurrentPath;
            RefreshLocalList();
            
            // Save to settings
            Settings.Default.SftpFileManagerLastLocalPath = path;
            Settings.Default.Save();
        }

        private void BtnLocalBrowse_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select a folder";
                dialog.ShowNewFolderButton = true;
                dialog.SelectedPath = _localCurrentPath;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    SetLocalPath(dialog.SelectedPath);
                }
            }
        }

        #endregion

        #region Event Handlers - Remote File Operations

        private async void RemoteListView_DoubleClick(object sender, EventArgs e)
        {
            if (!_isConnected || _remoteListView.SelectedItems.Count == 0) return;

            var item = _remoteListView.SelectedItems[0];
            var itemPath = item.Tag as RemoteFileInfo;
            if (itemPath == null) return;

            if (itemPath.IsDirectory)
            {
                _remoteCurrentPath = itemPath.FullPath;
                _txtRemotePath.Text = _remoteCurrentPath;
                await RefreshRemoteListAsync();
            }
        }

        private void RemoteListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == _remoteSortColumn)
                _remoteSortDescending = !_remoteSortDescending;
            else
            {
                _remoteSortColumn = e.Column;
                _remoteSortDescending = false;
            }
            _ = RefreshRemoteListAsync();
        }

        private async void RemoteListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back && _isConnected)
            {
                await NavigateRemoteParentAsync();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete && _isConnected)
            {
                await DeleteSelectedRemoteFilesAsync();
                e.Handled = true;
            }
        }

        private void RemotePath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && _isConnected)
            {
                var path = _txtRemotePath.Text.Trim();
                _remoteCurrentPath = path;
                _ = RefreshRemoteListAsync();
            }
        }

        private async Task NavigateRemoteParentAsync()
        {
            if (_remoteCurrentPath == "/")
                return;

            var parentPath = _remoteCurrentPath.Contains("/")
                ? "/" + string.Join("/", _remoteCurrentPath.Split('/').Where(s => !string.IsNullOrEmpty(s) && s != _remoteCurrentPath.Split('/').Last()).ToArray())
                : "/";

            if (string.IsNullOrEmpty(parentPath)) parentPath = "/";

            _remoteCurrentPath = parentPath;
            _txtRemotePath.Text = _remoteCurrentPath;
            await RefreshRemoteListAsync();
        }

        private async Task NavigateRemoteHomeAsync()
        {
            _remoteCurrentPath = GetRemoteHomeDirectory();
            _txtRemotePath.Text = _remoteCurrentPath;
            await RefreshRemoteListAsync();
        }

        #endregion

        #region Event Handlers - Transfer Operations

        private async void BtnUpload_Click(object sender, EventArgs e)
        {
            if (!_isConnected) return;
            if (_localListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select files to upload.", "Upload", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedFiles = _localListView.SelectedItems.Cast<ListViewItem>()
                .Select(item => Path.Combine(_localCurrentPath, item.Text))
                .ToList();

            await UploadFilesAsync(selectedFiles);
        }

        private async void BtnDownload_Click(object sender, EventArgs e)
        {
            if (!_isConnected) return;
            if (_remoteListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select files to download.", "Download", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedFiles = _remoteListView.SelectedItems.Cast<ListViewItem>()
                .Select(item => (item.Tag as RemoteFileInfo)?.FullPath)
                .Where(p => p != null)
                .Cast<string>()
                .ToList();

            await DownloadFilesAsync(selectedFiles);
        }

        #endregion

        #region File Operations

        private void RefreshLocalList()
        {
            try
            {
                _localListView.Items.Clear();

                if (Directory.Exists(_localCurrentPath) == false)
                {
                    _lblLocalStatus.Text = $"Local: Path not found";
                    return;
                }

                var itemList = new List<ListViewItem>();

                if (_localCurrentPath != Path.GetPathRoot(_localCurrentPath))
                {
                    itemList.Add(new ListViewItem(new[] { "..", "", "" })
                    {
                        Tag = "parent",
                        ImageKey = "parent"
                    });
                }

                var dirs = Directory.GetDirectories(_localCurrentPath)
                    .Select(d => new DirectoryInfo(d))
                    .Select(d => new
                    {
                        Item = new ListViewItem(new[]
                        {
                            d.Name,
                            "",
                            d.LastWriteTime.ToString("yyyy-MM-dd HH:mm")
                        })
                        {
                            Tag = d.FullName,
                            ImageKey = "folder"
                        },
                        IsDirectory = true,
                        SortName = d.Name.ToLowerInvariant(),
                        SortSize = 0L,
                        SortDate = d.LastWriteTime
                    });

                var files = Directory.GetFiles(_localCurrentPath)
                    .Select(f => new FileInfo(f))
                    .Select(f => new
                    {
                        Item = new ListViewItem(new[]
                        {
                            f.Name,
                            FormatFileSize(f.Length),
                            f.LastWriteTime.ToString("yyyy-MM-dd HH:mm")
                        })
                        {
                            Tag = f.FullName,
                            ImageKey = "file"
                        },
                        IsDirectory = false,
                        SortName = f.Name.ToLowerInvariant(),
                        SortSize = f.Length,
                        SortDate = f.LastWriteTime
                    });

                var allItems = dirs.Concat(files).ToList();

                var sortedItems = _localSortColumn switch
                {
                    0 => _localSortDescending
                        ? allItems.OrderByDescending(x => x.SortName)
                        : allItems.OrderBy(x => x.SortName),
                    1 => _localSortDescending
                        ? allItems.OrderByDescending(x => x.IsDirectory).ThenByDescending(x => x.SortSize)
                        : allItems.OrderByDescending(x => x.IsDirectory).ThenBy(x => x.SortSize),
                    2 => _localSortDescending
                        ? allItems.OrderByDescending(x => x.IsDirectory).ThenByDescending(x => x.SortDate)
                        : allItems.OrderByDescending(x => x.IsDirectory).ThenBy(x => x.SortDate),
                    _ => allItems.OrderBy(x => x.IsDirectory ? 0 : 1).ThenBy(x => x.SortName)
                };

                foreach (var item in sortedItems)
                    _localListView.Items.Add(item.Item);

                _lblLocalStatus.Text = $"Local: {_localCurrentPath}";
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Failed to refresh local list", ex);
                _lblLocalStatus.Text = $"Local: Error";
            }
        }

        private async Task RefreshRemoteListAsync()
        {
            if (!_isConnected) return;

            _remoteListView.Items.Clear();
            UpdateStatusText(_lblRemoteStatus, $"Remote: Loading {_remoteCurrentPath}...");

            try
            {
                var files = await Task.Run(() =>
                {
                    var result = new List<RemoteFileInfo>();

                    if (_remoteCurrentPath != "/")
                    {
                        result.Add(new RemoteFileInfo
                        {
                            Name = "..",
                            FullPath = GetParentPath(_remoteCurrentPath),
                            IsDirectory = true,
                            Size = 0,
                            ModifiedTime = DateTime.MinValue
                        });
                    }

                    var items = _sftpClient.ListDirectory(_remoteCurrentPath);
                    foreach (var item in items.Where(i => i.Name != "." && i.Name != ".."))
                    {
                        result.Add(new RemoteFileInfo
                        {
                            Name = item.Name,
                            FullPath = item.FullName,
                            IsDirectory = item.IsDirectory,
                            Size = item.Attributes.Size,
                            ModifiedTime = item.Attributes.LastWriteTime
                        });
                    }

                    IOrderedEnumerable<RemoteFileInfo> sortedFiles = _remoteSortColumn switch
                    {
                        0 => _remoteSortDescending
                            ? result.OrderByDescending(f => f.Name.ToLowerInvariant())
                            : result.OrderBy(f => f.Name.ToLowerInvariant()),
                        1 => _remoteSortDescending
                            ? result.OrderByDescending(f => f.IsDirectory).ThenByDescending(f => f.Size)
                            : result.OrderByDescending(f => f.IsDirectory).ThenBy(f => f.Size),
                        2 => _remoteSortDescending
                            ? result.OrderByDescending(f => f.IsDirectory).ThenByDescending(f => f.ModifiedTime)
                            : result.OrderByDescending(f => f.IsDirectory).ThenBy(f => f.ModifiedTime),
                        _ => result.OrderByDescending(f => f.IsDirectory).ThenBy(f => f.Name.ToLowerInvariant())
                    };

                    return sortedFiles.ToList();
                });

                if (_remoteListView.InvokeRequired)
                {
                    _remoteListView.Invoke(() =>
                    {
                        foreach (var file in files)
                        {
                            var item = new ListViewItem(new[]
                            {
                                file.Name,
                                file.IsDirectory ? "" : FormatFileSize(file.Size),
                                file.ModifiedTime == DateTime.MinValue ? "" : file.ModifiedTime.ToString("yyyy-MM-dd HH:mm")
                            })
                            {
                                Tag = file,
                                ImageKey = file.IsDirectory ? (file.Name == ".." ? "parent" : "folder") : "file"
                            };
                            _remoteListView.Items.Add(item);
                        }
                    });
                }
                else
                {
                    foreach (var file in files)
                    {
                        var item = new ListViewItem(new[]
                        {
                            file.Name,
                            file.IsDirectory ? "" : FormatFileSize(file.Size),
                            file.ModifiedTime == DateTime.MinValue ? "" : file.ModifiedTime.ToString("yyyy-MM-dd HH:mm")
                        })
                        {
                            Tag = file,
                            ImageKey = file.IsDirectory ? (file.Name == ".." ? "parent" : "folder") : "file"
                        };
                        _remoteListView.Items.Add(item);
                    }
                }

                UpdateStatusText(_lblRemoteStatus, $"Remote: {_remoteCurrentPath} ({files.Count} items)");
            }
            catch (Exception ex)
            {
                UpdateStatusText(_lblRemoteStatus, "Remote: Failed to load");
                Runtime.MessageCollector.AddExceptionStackTrace("Failed to refresh remote list", ex);
            }
        }

        private async Task UploadFilesAsync(List<string> localFiles)
        {
            if (!_isConnected) return;

            _queuePanel.Visible = true;
            _transferQueue.Items.Clear();

            foreach (var localFile in localFiles)
            {
                var fileName = Path.GetFileName(localFile);
                var remotePath = $"{_remoteCurrentPath}/{fileName}".Replace("//", "/");

                if (Directory.Exists(localFile))
                {
                    await UploadDirectoryAsync(localFile, remotePath);
                }
                else
                {
                    await UploadFileAsync(localFile, remotePath);
                }
            }

            await RefreshRemoteListAsync();
        }

        private async Task UploadDirectoryAsync(string localDir, string remotePath)
        {
            try
            {
                await Task.Run(() => _sftpClient.CreateDirectory(remotePath));

                var files = Directory.GetFiles(localDir);
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);
                    var remoteFilePath = $"{remotePath}/{fileName}";
                    await UploadFileAsync(file, remoteFilePath);
                }

                var dirs = Directory.GetDirectories(localDir);
                foreach (var dir in dirs)
                {
                    var dirName = Path.GetFileName(dir);
                    var remoteDirPath = $"{remotePath}/{dirName}";
                    await UploadDirectoryAsync(dir, remoteDirPath);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace($"Failed to upload directory {localDir}", ex);
            }
        }

        private async Task UploadFileAsync(string localFile, string remotePath)
        {
            var listItem = new ListViewItem(new[]
            {
                Path.GetFileName(localFile),
                "Upload",
                FormatFileSize(new FileInfo(localFile).Length),
                "In Progress",
                "0%"
            });
            _transferQueue.Items.Add(listItem);

            try
            {
                UpdateTransferStatus($"Uploading: {Path.GetFileName(localFile)}");

                using var fileStream = File.OpenRead(localFile);
                var fileLength = fileStream.Length;
                var uploaded = 0L;

                await Task.Run(() =>
                {
                    _sftpClient.UploadFile(fileStream, remotePath, true, uploadedBytes =>
                    {
                        UpdateProgress((long)uploadedBytes, fileLength, listItem);
                    });
                });

                UpdateListViewItemSafe(listItem, 3, "Completed");
                UpdateListViewItemSafe(listItem, 4, "100%");
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, $"Uploaded: {localFile} -> {remotePath}");
            }
            catch (Exception ex)
            {
                UpdateListViewItemSafe(listItem, 3, "Failed");
                UpdateListViewItemSafe(listItem, 4, "0%");
                Runtime.MessageCollector.AddExceptionStackTrace($"Failed to upload {localFile}", ex);
            }

            UpdateTransferStatus("");
        }

        private async Task DownloadFilesAsync(List<string> remotePaths)
        {
            if (!_isConnected) return;

            _queuePanel.Visible = true;
            _transferQueue.Items.Clear();

            foreach (var remotePath in remotePaths)
            {
                var fileName = remotePath.Split('/').Last();
                var localPath = Path.Combine(_localCurrentPath, fileName);

                var item = _remoteListView.Items.Cast<ListViewItem>()
                    .FirstOrDefault(i => (i.Tag as RemoteFileInfo)?.FullPath == remotePath);

                if (item?.Tag is RemoteFileInfo remoteInfo && remoteInfo.IsDirectory)
                {
                    await DownloadDirectoryAsync(remotePath, localPath);
                }
                else
                {
                    await DownloadFileAsync(remotePath, localPath);
                }
            }

            RefreshLocalList();
        }

        private async Task DownloadDirectoryAsync(string remotePath, string localPath)
        {
            try
            {
                if (!Directory.Exists(localPath))
                    Directory.CreateDirectory(localPath);

                var items = await Task.Run(() => _sftpClient.ListDirectory(remotePath).ToList());
                foreach (var item in items.Where(i => i.Name != "." && i.Name != ".."))
                {
                    var itemLocalPath = Path.Combine(localPath, item.Name);
                    if (item.IsDirectory)
                    {
                        await DownloadDirectoryAsync(item.FullName, itemLocalPath);
                    }
                    else
                    {
                        await DownloadFileAsync(item.FullName, itemLocalPath);
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace($"Failed to download directory {remotePath}", ex);
            }
        }

        private async Task DownloadFileAsync(string remotePath, string localPath)
        {
            var listItem = new ListViewItem(new[]
            {
                Path.GetFileName(remotePath),
                "Download",
                "",
                "In Progress",
                "0%"
            });
            _transferQueue.Items.Add(listItem);

            try
            {
                UpdateTransferStatus($"Downloading: {Path.GetFileName(remotePath)}");

                using var fileStream = File.Create(localPath);
                var fileLength = _sftpClient.GetAttributes(remotePath).Size;
                UpdateListViewItemSafe(listItem, 2, FormatFileSize(fileLength));

                await Task.Run(() =>
                {
                    _sftpClient.DownloadFile(remotePath, fileStream, downloadedBytes =>
                    {
                        UpdateProgress((long)downloadedBytes, fileLength, listItem);
                    });
                });

                UpdateListViewItemSafe(listItem, 3, "Completed");
                UpdateListViewItemSafe(listItem, 4, "100%");
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, $"Downloaded: {remotePath} -> {localPath}");
            }
            catch (Exception ex)
            {
                UpdateListViewItemSafe(listItem, 3, "Failed");
                UpdateListViewItemSafe(listItem, 4, "0%");
                Runtime.MessageCollector.AddExceptionStackTrace($"Failed to download {remotePath}", ex);
            }

            UpdateTransferStatus("");
        }

        private async Task CreateNewRemoteFolderAsync()
        {
            if (!_isConnected) return;

            using var inputBox = new FrmInputBox("Create New Folder", "Enter folder name:", "NewFolder");
            if (inputBox.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(inputBox.returnValue))
            {
                try
                {
                    var newFolderPath = $"{_remoteCurrentPath}/{inputBox.returnValue}".Replace("//", "/");
                    await Task.Run(() => _sftpClient.CreateDirectory(newFolderPath));
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, $"Created folder: {newFolderPath}");
                    await RefreshRemoteListAsync();
                }
                catch (Exception ex)
                {
                    Runtime.MessageCollector.AddExceptionStackTrace("Failed to create folder", ex);
                    MessageBox.Show($"Failed to create folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async Task DeleteSelectedRemoteFilesAsync()
        {
            if (!_isConnected || _remoteListView.SelectedItems.Count == 0) return;

            var result = MessageBox.Show(
                $"Delete {_remoteListView.SelectedItems.Count} item(s)?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            foreach (ListViewItem item in _remoteListView.SelectedItems)
            {
                var fileInfo = item.Tag as RemoteFileInfo;
                if (fileInfo == null) continue;

                try
                {
                    if (fileInfo.IsDirectory)
                    {
                        await DeleteRemoteDirectoryAsync(fileInfo.FullPath);
                    }
                    else
                    {
                        await Task.Run(() => _sftpClient.DeleteFile(fileInfo.FullPath));
                    }
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, $"Deleted: {fileInfo.FullPath}");
                }
                catch (Exception ex)
                {
                    Runtime.MessageCollector.AddExceptionStackTrace($"Failed to delete {fileInfo.FullPath}", ex);
                }
            }

            await RefreshRemoteListAsync();
        }

        private async Task DeleteRemoteDirectoryAsync(string path)
        {
            try
            {
                var items = await Task.Run(() => _sftpClient.ListDirectory(path).ToList());
                foreach (var item in items.Where(i => i.Name != "." && i.Name != ".."))
                {
                    if (item.IsDirectory)
                    {
                        await DeleteRemoteDirectoryAsync(item.FullName);
                    }
                    else
                    {
                        await Task.Run(() => _sftpClient.DeleteFile(item.FullName));
                    }
                }
                await Task.Run(() => _sftpClient.DeleteDirectory(path));
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace($"Failed to delete directory {path}", ex);
            }
        }

        private void UpdateProgress(long current, long total, ListViewItem listItem)
        {
            if (listItem == null) return;

            var percentage = total > 0 ? (int)((current * 100) / total) : 0;

            if (_localListView.InvokeRequired)
            {
                _localListView.Invoke(() => UpdateProgressInternal(percentage, listItem));
            }
            else
            {
                UpdateProgressInternal(percentage, listItem);
            }
        }

        private void UpdateProgressInternal(int percentage, ListViewItem listItem)
        {
            if (listItem?.SubItems.Count > 4)
            {
                listItem.SubItems[4].Text = $"{percentage}%";
            }

            if (_progressBar != null)
            {
                _progressBar.Maximum = 100;
                _progressBar.Value = Math.Min(percentage, 100);
                _progressBar.Visible = true;
            }
        }

        private void UpdateStatusText(Label label, string text)
        {
            if (label == null) return;

            if (label.InvokeRequired)
            {
                label.Invoke(() =>
                {
                    label.Text = text;
                });
            }
            else
            {
                label.Text = text;
            }
        }

        private void UpdateTransferStatus(string text)
        {
            UpdateStatusText(_lblTransferStatus, text);
        }

        private void UpdateListViewItemSafe(ListViewItem listItem, int subItemIndex, string text)
        {
            if (listItem == null) return;

            if (listItem.ListView?.InvokeRequired == true)
            {
                listItem.ListView.Invoke(() =>
                {
                    if (subItemIndex < listItem.SubItems.Count)
                        listItem.SubItems[subItemIndex].Text = text;
                });
            }
            else
            {
                if (subItemIndex < listItem.SubItems.Count)
                    listItem.SubItems[subItemIndex].Text = text;
            }
        }

        #endregion

        #region Helper Methods

        private string GetParentPath(string path)
        {
            if (path == "/") return "/";
            var parts = path.Split('/').Where(p => !string.IsNullOrEmpty(p)).ToArray();
            if (parts.Length <= 1) return "/";
            return "/" + string.Join("/", parts.Take(parts.Length - 1));
        }

        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            double size = bytes;
            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size /= 1024;
            }
            return $"{size:0.##} {sizes[order]}";
        }

        private void LoadFromConnectionInfo()
        {
            if (ConnectionInfo == null) return;

            _txtHost.Text = ConnectionInfo.Host;
            _txtPort.Text = ConnectionInfo.Port.ToString();
            _txtUsername.Text = ConnectionInfo.Username;
        }

        #endregion

        #region Form Events

        private void SftpFileManager_Load(object sender, EventArgs e)
        {
            ApplyTheme();
            ApplyLanguage();
            Icon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.SyncArrow_16x);
            
            // Load last used local path from settings, or default to user profile folder
            var lastPath = Settings.Default.SftpFileManagerLastLocalPath;
            _localCurrentPath = !string.IsNullOrEmpty(lastPath) && Directory.Exists(lastPath)
                ? lastPath
                : Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            
            _txtLocalPath.Text = _localCurrentPath;
            RefreshLocalList();
        }

        private void ApplyLanguage()
        {
            Text = Language.SshFileTransfer;
            TabText = Language.SshFileTransfer;

            // Update local context menu items
            if (_localContextMenu?.Items.Count >= 3)
            {
                _localContextMenu.Items[0].Text = "Upload";
                _localContextMenu.Items[2].Text = Language._Scan;
            }

            // Update remote context menu items (5 items: Download, Sep, New, Delete, Scan)
            if (_remoteContextMenu?.Items.Count >= 5)
            {
                _remoteContextMenu.Items[0].Text = Language.Download;
                _remoteContextMenu.Items[2].Text = Language._New;
                _remoteContextMenu.Items[3].Text = Language._Delete;
                _remoteContextMenu.Items[4].Text = Language._Scan;
            }
        }

        #endregion

        #region Public Methods

        public void SetConnection(ConnectionInfo connectionInfo)
        {
            ConnectionInfo = connectionInfo;
            LoadFromConnectionInfo();
            _useExistingSession = false;
            _sshClient = null;
        }

        /// <summary>
        /// Sets the connection info and attempts to reuse credentials from an existing SSH session.
        /// The SFTP client will use the same authentication information as any existing connection.
        /// </summary>
        /// <param name="connectionInfo">The connection info with authentication details</param>
        /// <param name="sshClient">An existing SSH client session (kept for reference, not used directly)</param>
        public void SetConnectionWithExistingSession(ConnectionInfo connectionInfo, SshClient sshClient = null)
        {
            ConnectionInfo = connectionInfo;
            LoadFromConnectionInfo();
            
            // Store reference to existing session for future use
            _sshClient = sshClient;
            // Mark that we're using credentials from an existing session
            _useExistingSession = sshClient != null && sshClient.IsConnected;
            
            // Automatically connect using the shared credentials
            // This avoids asking the user for password again
            Runtime.MessageCollector.AddMessage(
                mRemoteNG.Messages.MessageClass.InformationMsg,
                _useExistingSession 
                    ? $"Using existing SSH session credentials for {_txtHost.Text}"
                    : $"Connecting to {_txtHost.Text} with saved credentials");
            
            // Start connection in background
            _ = Task.Run(async () => await ConnectAsync());
        }

        private void UpdateConnectedUI()
        {
            _btnConnect.Text = Language._Close;
            _btnConnect.Image = Properties.Resources.Stop_16x;
            _btnDisconnect.Enabled = true;
            _lblRemoteStatus.Text = $"Remote: Connected to {_txtHost.Text} (reusing session)";
        }

        #endregion

        #region IDisposable

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Disconnect();
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource?.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }

    #region Helper Classes

    internal class RemoteFileInfo
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public bool IsDirectory { get; set; }
        public long Size { get; set; }
        public DateTime ModifiedTime { get; set; }
    }

    #endregion
}

