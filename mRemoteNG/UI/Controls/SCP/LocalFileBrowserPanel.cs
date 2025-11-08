using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms;
using BrightIdeasSoftware;
using mRemoteNG.App;
using mRemoteNG.Messages;

namespace mRemoteNG.UI.Controls.SCP
{
    /// <summary>
    /// File browser panel for local file system navigation.
    /// Shows directory tree and file list with full metadata.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public partial class LocalFileBrowserPanel : UserControl
    {
        #region Private Fields

        private SplitContainer _mainSplitter;
        private SplitContainer _topSplitter;
        private ComboBox _pathComboBox;
        private TreeView _directoryTree;
        private ObjectListView _fileListView;
        private Label _statusLabel;
        private ImageList _imageList;
        private string _currentPath;
        private List<FileListItem> _currentFiles;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the current directory path.
        /// </summary>
        public string CurrentPath => _currentPath;

        /// <summary>
        /// Gets the list of currently selected files/folders.
        /// </summary>
        public List<FileListItem> SelectedFiles
        {
            get
            {
                return _fileListView.SelectedObjects
                    .Cast<FileListItem>()
                    .ToList();
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Fired when the file selection changes.
        /// </summary>
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Fired when the Escape key is pressed to clear all selections.
        /// </summary>
        public event EventHandler EscapePressed;

        #endregion

        #region Constructor

        public LocalFileBrowserPanel()
        {
            InitializeComponent();
            InitializeControls();
        }

        #endregion

        #region Initialization

        private void InitializeComponent()
        {
            SuspendLayout();

            // Set control properties
            Name = "LocalFileBrowserPanel";
            Size = new Size(400, 600);

            ResumeLayout(false);
        }

        private void InitializeControls()
        {
            SuspendLayout();

            // Create main vertical splitter (path bar + content | status bar)
            _mainSplitter = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 550,
                FixedPanel = FixedPanel.Panel2,
                IsSplitterFixed = true,
                Panel2MinSize = 25
            };

            // Create path combo box
            _pathComboBox = new ComboBox
            {
                Dock = DockStyle.Top,
                DropDownStyle = ComboBoxStyle.DropDown,
                Font = new Font("Segoe UI", 9F)
            };
            _pathComboBox.KeyDown += PathComboBox_KeyDown;

            // Create top splitter (tree | file list)
            _topSplitter = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 180,
                Panel1MinSize = 100,
                Panel2MinSize = 100
            };

            // Create directory tree
            _directoryTree = new TreeView
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F),
                HideSelection = false,
                ShowLines = true,
                ShowPlusMinus = true
            };
            _directoryTree.BeforeExpand += DirectoryTree_BeforeExpand;
            _directoryTree.AfterSelect += DirectoryTree_AfterSelect;
            _directoryTree.KeyDown += Control_KeyDown;

            // Create image list for icons
            _imageList = new ImageList
            {
                ColorDepth = ColorDepth.Depth32Bit,
                ImageSize = new Size(16, 16)
            };
            InitializeIcons();

            // Create file list view
            _fileListView = new ObjectListView
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F),
                FullRowSelect = true,
                GridLines = true,
                View = View.Details,
                MultiSelect = true,
                HideSelection = false,
                ShowGroups = false,
                UseCompatibleStateImageBehavior = false
            };

            // Set the image list using ObjectListView's method (not property)
            _fileListView.SetSmallImageList(_imageList);

            // Configure columns with icon support - Name column shows file/folder icons
            var nameColumn = new OLVColumn
            {
                Text = "Name",
                Width = 250,
                AspectName = "Name",
                ImageGetter = delegate(object row)
                {
                    var item = (FileListItem)row;
                    return item.IconIndex;
                }
            };
            _fileListView.Columns.Add(nameColumn);
            _fileListView.Columns.Add(new OLVColumn { Text = "Size", Width = 100, AspectName = "SizeFormatted", TextAlign = HorizontalAlignment.Right });
            _fileListView.Columns.Add(new OLVColumn { Text = "Modified", Width = 140, AspectName = "ModifiedFormatted" });
            _fileListView.Columns.Add(new OLVColumn { Text = "Type", Width = 120, AspectName = "Type" });

            _fileListView.SelectedIndexChanged += FileListView_SelectedIndexChanged;
            _fileListView.DoubleClick += FileListView_DoubleClick;
            _fileListView.KeyDown += Control_KeyDown;

            // Create status label
            _statusLabel = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 8F),
                Padding = new Padding(5, 0, 0, 0),
                AutoSize = false
            };

            // Assemble the layout
            _topSplitter.Panel1.Controls.Add(_directoryTree);
            _topSplitter.Panel2.Controls.Add(_fileListView);

            _mainSplitter.Panel1.Controls.Add(_topSplitter);
            _mainSplitter.Panel1.Controls.Add(_pathComboBox);
            _mainSplitter.Panel2.Controls.Add(_statusLabel);

            Controls.Add(_mainSplitter);

            // Set ImageList for tree view
            _directoryTree.ImageList = _imageList;

            ResumeLayout(false);

            // Initialize with user's profile directory or program directory
            string initialPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            if (string.IsNullOrEmpty(initialPath))
                initialPath = AppDomain.CurrentDomain.BaseDirectory;

            // Populate tree first, then navigate
            PopulateTreeView();
            NavigateToPath(initialPath);
        }

        /// <summary>
        /// Initializes the icon ImageList with folder, file, and drive icons using mRemoteNG resources.
        /// </summary>
        private void InitializeIcons()
        {
            try
            {
                // Icon Index 0: Regular Folder
                var folderIcon = Properties.Resources.ResourceManager.GetObject("FolderClosed_16x") as Bitmap;
                _imageList.Images.Add(folderIcon ?? SystemIcons.Application.ToBitmap());

                // Icon Index 1: File
                var fileIcon = Properties.Resources.ResourceManager.GetObject("Document_16x") as Bitmap;
                _imageList.Images.Add(fileIcon ?? SystemIcons.Application.ToBitmap());

                // Icon Index 2: Drive/Mount Point (use SQLDatabase icon as drive substitute)
                // Note: For local file browser, this represents actual Windows drives (C:\, D:\, etc.)
                var driveIcon = Properties.Resources.ResourceManager.GetObject("SQLDatabase_16x") as Bitmap;
                _imageList.Images.Add(driveIcon ?? SystemIcons.Application.ToBitmap());

                Logger.Instance.Log?.Debug("[LocalFileBrowserPanel.InitializeIcons] Icons loaded successfully");
            }
            catch (Exception ex)
            {
                Logger.Instance.Log?.Warn("[LocalFileBrowserPanel.InitializeIcons] Failed to load icons, using defaults", ex);
                // Fallback to basic icons if resources fail
                _imageList.Images.Add(SystemIcons.Application.ToBitmap());
                _imageList.Images.Add(SystemIcons.Application.ToBitmap());
                _imageList.Images.Add(SystemIcons.Application.ToBitmap());
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Navigates to the specified directory path.
        /// </summary>
        public void NavigateToPath(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    Logger.Instance.Log?.Warn("[LocalFileBrowserPanel.NavigateToPath] Path is empty");
                    return;
                }

                if (!Directory.Exists(path))
                {
                    Logger.Instance.Log?.Warn($"[LocalFileBrowserPanel.NavigateToPath] Directory not found: {path}");
                    MessageBox.Show($"Directory not found or inaccessible:\n{path}",
                        "Navigation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Logger.Instance.Log?.Debug($"[LocalFileBrowserPanel.NavigateToPath] Navigating to: {path}");

                _currentPath = path;
                _pathComboBox.Text = path;

                // Add to history if not already there
                if (!_pathComboBox.Items.Contains(path))
                {
                    _pathComboBox.Items.Insert(0, path);
                    if (_pathComboBox.Items.Count > 20)
                        _pathComboBox.Items.RemoveAt(_pathComboBox.Items.Count - 1);
                }

                // Expand tree to show current path
                ExpandTreeToPath(path);

                LoadDirectory(path);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show($"Access denied to:\n{path}",
                    "Permission Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector?.AddExceptionMessage($"Error navigating to: {path}", ex);
                MessageBox.Show($"Error navigating to directory:\n{ex.Message}",
                    "Navigation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Refreshes the current directory contents.
        /// </summary>
        public void RefreshCurrentDirectory()
        {
            if (!string.IsNullOrEmpty(_currentPath))
            {
                LoadDirectory(_currentPath);
            }
        }

        /// <summary>
        /// Refreshes the TreeView to show new/deleted directories.
        /// This reloads the children of the current directory's parent node in the tree.
        /// </summary>
        public void RefreshTreeView()
        {
            try
            {
                if (_directoryTree.Nodes.Count == 0)
                    return;

                Logger.Instance.Log?.Debug($"[LocalFileBrowserPanel.RefreshTreeView] Refreshing tree view for path: {_currentPath}");

                // Get the parent directory path
                string parentPath = Path.GetDirectoryName(_currentPath);
                if (string.IsNullOrEmpty(parentPath))
                    parentPath = _currentPath; // For drive roots

                // Find the parent node in the tree
                TreeNode parentNode = FindNodeByPath(parentPath);
                if (parentNode != null)
                {
                    // Save current expanded state of child nodes
                    var expandedChildren = new HashSet<string>();
                    foreach (TreeNode child in parentNode.Nodes)
                    {
                        if (child.IsExpanded && child.Tag is string childPath)
                        {
                            expandedChildren.Add(childPath);
                        }
                    }

                    // Reload the parent node's children
                    Logger.Instance.Log?.Debug($"[LocalFileBrowserPanel.RefreshTreeView] Reloading children for: {parentPath}");
                    LoadTreeNodeChildren(parentNode);

                    // Restore expanded state
                    foreach (TreeNode child in parentNode.Nodes)
                    {
                        if (child.Tag is string childPath && expandedChildren.Contains(childPath))
                        {
                            child.Expand();
                            LoadTreeNodeChildren(child);
                        }
                    }

                    // Re-expand to current path to ensure it's visible and selected
                    ExpandTreeToPath(_currentPath);
                }

                Logger.Instance.Log?.Debug("[LocalFileBrowserPanel.RefreshTreeView] Tree view refresh complete");
            }
            catch (Exception ex)
            {
                Logger.Instance.Log?.Error("[LocalFileBrowserPanel.RefreshTreeView] Error refreshing tree view", ex);
                Runtime.MessageCollector?.AddExceptionMessage("Error refreshing tree view", ex);
            }
        }

        /// <summary>
        /// Finds a tree node by its full path.
        /// </summary>
        /// <param name="path">The full path to find</param>
        /// <returns>The matching TreeNode, or null if not found</returns>
        private TreeNode FindNodeByPath(string path)
        {
            try
            {
                if (_directoryTree.Nodes.Count == 0)
                    return null;

                // Normalize path
                path = Path.GetFullPath(path);

                // Check each root node (drives)
                foreach (TreeNode rootNode in _directoryTree.Nodes)
                {
                    string rootPath = rootNode.Tag as string;
                    if (string.IsNullOrEmpty(rootPath))
                        continue;

                    // If this is the exact path, return the root node
                    if (string.Equals(path, rootPath, StringComparison.OrdinalIgnoreCase))
                        return rootNode;

                    // If path starts with this root, search within it
                    if (path.StartsWith(rootPath, StringComparison.OrdinalIgnoreCase))
                    {
                        TreeNode currentNode = rootNode;
                        string remainingPath = path.Substring(rootPath.Length).TrimStart(Path.DirectorySeparatorChar);

                        if (string.IsNullOrEmpty(remainingPath))
                            return currentNode;

                        string[] segments = remainingPath.Split(Path.DirectorySeparatorChar);

                        foreach (string segment in segments)
                        {
                            // Load children if needed
                            if (currentNode.Nodes.Count == 1 && currentNode.Nodes[0].Tag == null)
                            {
                                LoadTreeNodeChildren(currentNode);
                            }

                            // Find matching child
                            TreeNode matchingChild = null;
                            foreach (TreeNode child in currentNode.Nodes)
                            {
                                if (string.Equals(child.Text, segment, StringComparison.OrdinalIgnoreCase))
                                {
                                    matchingChild = child;
                                    break;
                                }
                            }

                            if (matchingChild == null)
                                return null;

                            currentNode = matchingChild;
                        }

                        return currentNode;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                Logger.Instance.Log?.Error($"[LocalFileBrowserPanel.FindNodeByPath] Error finding node: {path}", ex);
                return null;
            }
        }

        /// <summary>
        /// Deletes the currently selected files/folders from the local file system.
        /// </summary>
        public void DeleteSelectedFiles()
        {
            var selectedFiles = SelectedFiles;
            if (selectedFiles == null || selectedFiles.Count == 0)
            {
                Logger.Instance.Log?.Warn("[LocalFileBrowserPanel.DeleteSelectedFiles] No files selected");
                return;
            }

            try
            {
                foreach (var file in selectedFiles)
                {
                    if (file.IsDirectory)
                    {
                        // Delete directory recursively
                        Logger.Instance.Log?.Debug($"[LocalFileBrowserPanel.DeleteSelectedFiles] Deleting directory: {file.FullPath}");
                        Directory.Delete(file.FullPath, true);
                    }
                    else
                    {
                        // Delete file
                        Logger.Instance.Log?.Debug($"[LocalFileBrowserPanel.DeleteSelectedFiles] Deleting file: {file.FullPath}");
                        File.Delete(file.FullPath);
                    }
                }

                Logger.Instance.Log?.Info($"[LocalFileBrowserPanel.DeleteSelectedFiles] Successfully deleted {selectedFiles.Count} item(s)");

                // Check if any directories were deleted
                bool deletedDirectories = selectedFiles.Any(f => f.IsDirectory);

                // Refresh the directory view
                RefreshCurrentDirectory();

                // Refresh tree view if any directories were deleted
                if (deletedDirectories)
                {
                    RefreshTreeView();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Log?.Error("[LocalFileBrowserPanel.DeleteSelectedFiles] Error deleting files", ex);
                throw; // Re-throw to let calling code handle the error
            }
        }

        /// <summary>
        /// Clears all file selections in the file list view and directory tree view.
        /// </summary>
        public void ClearSelection()
        {
            // Suspend drawing to prevent visual artifacts during clearing
            _fileListView.BeginUpdate();
            _directoryTree.BeginUpdate();

            try
            {
                // Clear file list view selection
                _fileListView.SelectedObjects = null;
                _fileListView.SelectedIndices.Clear();

                // Clear directory tree view selection
                _directoryTree.SelectedNode = null;

                // Update status bar to reflect cleared selection
                UpdateStatusBar();

                Logger.Instance.Log?.Debug("[LocalFileBrowserPanel.ClearSelection] All selections cleared (file list and tree view)");
            }
            finally
            {
                // Resume drawing
                _directoryTree.EndUpdate();
                _fileListView.EndUpdate();
            }
        }

        /// <summary>
        /// Override ProcessCmdKey to catch Escape at a lower level in the event chain.
        /// This is more reliable than KeyDown for ensuring the key is caught.
        /// </summary>
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                Logger.Instance.Log?.Debug("[LocalFileBrowserPanel.ProcessCmdKey] Escape key detected");
                EscapePressed?.Invoke(this, EventArgs.Empty);
                return true; // Mark as handled
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        #region Private Methods

        private void LoadDirectory(string path)
        {
            try
            {
                _currentFiles = new List<FileListItem>();

                // Get directories
                var dirs = Directory.GetDirectories(path)
                    .Select(d => new DirectoryInfo(d))
                    .Where(d => (d.Attributes & FileAttributes.Hidden) == 0)
                    .Select(d => FileListItem.FromLocalFile(d));

                // Get files
                var files = Directory.GetFiles(path)
                    .Select(f => new FileInfo(f))
                    .Where(f => (f.Attributes & FileAttributes.Hidden) == 0)
                    .Select(f => FileListItem.FromLocalFile(f));

                _currentFiles.AddRange(dirs);
                _currentFiles.AddRange(files);

                _fileListView.SetObjects(_currentFiles);
                UpdateStatusBar();
            }
            catch (UnauthorizedAccessException)
            {
                _fileListView.ClearObjects();
                UpdateStatusBar();
                throw;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector?.AddExceptionMessage($"Error loading directory: {path}", ex);
                _fileListView.ClearObjects();
                UpdateStatusBar();
            }
        }

        private void PopulateTreeView()
        {
            try
            {
                _directoryTree.Nodes.Clear();

                // Add all logical drives
                var drives = DriveInfo.GetDrives()
                    .Where(d => d.IsReady)
                    .ToList();

                foreach (var drive in drives)
                {
                    var node = new TreeNode(drive.Name)
                    {
                        Tag = drive.RootDirectory.FullName,
                        ImageIndex = 2,          // Drive icon
                        SelectedImageIndex = 2
                    };

                    // Add dummy node for lazy loading
                    node.Nodes.Add(new TreeNode());
                    _directoryTree.Nodes.Add(node);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector?.AddExceptionMessage("Error populating directory tree", ex);
            }
        }

        private void LoadTreeNodeChildren(TreeNode parentNode)
        {
            try
            {
                parentNode.Nodes.Clear();

                string path = parentNode.Tag as string;
                if (string.IsNullOrEmpty(path))
                    return;

                var dirs = Directory.GetDirectories(path)
                    .Select(d => new DirectoryInfo(d))
                    .Where(d => (d.Attributes & FileAttributes.Hidden) == 0)
                    .OrderBy(d => d.Name);

                foreach (var dir in dirs)
                {
                    var node = new TreeNode(dir.Name)
                    {
                        Tag = dir.FullName,
                        ImageIndex = 0,          // Folder icon
                        SelectedImageIndex = 0
                    };

                    // Add dummy node for lazy loading if directory has subdirectories
                    try
                    {
                        if (Directory.GetDirectories(dir.FullName).Length > 0)
                        {
                            node.Nodes.Add(new TreeNode());
                        }
                    }
                    catch
                    {
                        // Ignore permission errors when checking for subdirectories
                    }

                    parentNode.Nodes.Add(node);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Silently ignore - user doesn't have access to this directory
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector?.AddExceptionMessage(
                    $"Error loading tree node children: {parentNode.Tag}", ex);
            }
        }

        private void UpdateStatusBar()
        {
            int fileCount = _currentFiles?.Count(f => !f.IsDirectory) ?? 0;
            int dirCount = _currentFiles?.Count(f => f.IsDirectory) ?? 0;
            long totalSize = _currentFiles?.Where(f => !f.IsDirectory).Sum(f => f.Size) ?? 0;

            int selectedCount = SelectedFiles.Count;
            long selectedSize = SelectedFiles.Where(f => !f.IsDirectory).Sum(f => f.Size);

            if (selectedCount > 0)
            {
                _statusLabel.Text = $"{selectedCount} item(s) selected ({FormatFileSize(selectedSize)})  |  " +
                                  $"{fileCount} file(s), {dirCount} folder(s) total";
            }
            else
            {
                _statusLabel.Text = $"{fileCount} file(s), {dirCount} folder(s) ({FormatFileSize(totalSize)})";
            }
        }

        private static string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return $"{len:0.#} {sizes[order]}";
        }

        /// <summary>
        /// Expands the tree view to show and select the specified path.
        /// </summary>
        private void ExpandTreeToPath(string targetPath)
        {
            try
            {
                if (_directoryTree.Nodes.Count == 0 || string.IsNullOrWhiteSpace(targetPath))
                    return;

                // Normalize path
                targetPath = Path.GetFullPath(targetPath);

                // Get drive root (e.g., "C:\")
                string rootPath = Path.GetPathRoot(targetPath);
                if (string.IsNullOrEmpty(rootPath))
                    return;

                // Find the drive node
                TreeNode currentNode = null;
                foreach (TreeNode node in _directoryTree.Nodes)
                {
                    if (node.Tag is string nodePath && nodePath.Equals(rootPath, StringComparison.OrdinalIgnoreCase))
                    {
                        currentNode = node;
                        break;
                    }
                }

                if (currentNode == null)
                    return;

                // If target is the drive root, just select it
                if (targetPath.Equals(rootPath, StringComparison.OrdinalIgnoreCase))
                {
                    _directoryTree.SelectedNode = currentNode;
                    return;
                }

                // Get path relative to root (e.g., "Users\MyUser\Documents")
                string relativePath = targetPath.Substring(rootPath.Length);
                string[] segments = relativePath.Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);

                // Navigate through each segment
                foreach (string segment in segments)
                {
                    // Check if node needs lazy loading
                    if (currentNode.Nodes.Count == 1 && currentNode.Nodes[0].Tag == null)
                    {
                        LoadTreeNodeChildren(currentNode);
                    }

                    // Expand current node
                    currentNode.Expand();

                    // Find child node matching this segment
                    TreeNode matchingChild = null;
                    foreach (TreeNode childNode in currentNode.Nodes)
                    {
                        if (childNode.Text.Equals(segment, StringComparison.OrdinalIgnoreCase))
                        {
                            matchingChild = childNode;
                            break;
                        }
                    }

                    if (matchingChild == null)
                    {
                        // Path segment not found in tree - might be access denied
                        return;
                    }

                    // Move to child node
                    currentNode = matchingChild;
                }

                // Select the final node and ensure it's visible
                _directoryTree.SelectedNode = currentNode;
                currentNode.EnsureVisible();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector?.AddExceptionMessage($"Error expanding tree to path: {targetPath}", ex);
            }
        }

        #endregion

        #region Event Handlers

        private void PathComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                NavigateToPath(_pathComboBox.Text);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void DirectoryTree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            // Lazy load children when expanding
            if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Tag == null)
            {
                LoadTreeNodeChildren(e.Node);
            }
        }

        private void DirectoryTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag is string path)
            {
                NavigateToPath(path);
            }
        }

        private void FileListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateStatusBar();
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        private void FileListView_DoubleClick(object sender, EventArgs e)
        {
            if (SelectedFiles.Count == 1 && SelectedFiles[0].IsDirectory)
            {
                NavigateToPath(SelectedFiles[0].FullPath);
            }
        }

        private void Control_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                EscapePressed?.Invoke(this, EventArgs.Empty);
                e.Handled = true;
            }
        }

        #endregion

        #region Disposal

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Unhook event handlers to prevent memory leaks
                if (_pathComboBox != null)
                    _pathComboBox.KeyDown -= PathComboBox_KeyDown;

                if (_directoryTree != null)
                {
                    _directoryTree.BeforeExpand -= DirectoryTree_BeforeExpand;
                    _directoryTree.AfterSelect -= DirectoryTree_AfterSelect;
                }

                if (_fileListView != null)
                {
                    _fileListView.SelectedIndexChanged -= FileListView_SelectedIndexChanged;
                    _fileListView.DoubleClick -= FileListView_DoubleClick;
                }

                // Clean up resources
                _currentFiles?.Clear();
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
