using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using mRemoteNG.App;
using mRemoteNG.Messages;
using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace mRemoteNG.Connection.Protocol.SCP
{
    /// <summary>
    /// Manages SFTP/SCP connections and file transfer operations using SSH.NET library.
    /// Provides file listing, upload, download, and directory navigation capabilities.
    /// </summary>
    public class ScpTransferManager : IDisposable
    {
        #region Private Fields

        private SftpClient _sftpClient;
        private ScpClient _scpClient;
        private readonly ConnectionInfo _connectionInfo;
        private bool _isConnected;
        private bool _disposed;
        private CancellationTokenSource _shutdownTokenSource;
        private HashSet<string> _mountPoints;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets whether the SFTP connection is currently active.
        /// </summary>
        public bool IsConnected => _isConnected && _sftpClient?.IsConnected == true;

        /// <summary>
        /// Gets the current working directory on the remote server.
        /// </summary>
        public string CurrentDirectory => _sftpClient?.WorkingDirectory ?? "/";

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new ScpTransferManager instance.
        /// </summary>
        /// <param name="connectionInfo">Connection information for the remote server</param>
        public ScpTransferManager(ConnectionInfo connectionInfo)
        {
            _connectionInfo = connectionInfo ?? throw new ArgumentNullException(nameof(connectionInfo));
        }

        #endregion

        #region Connection Methods

        /// <summary>
        /// Establishes SFTP connection to the remote server.
        /// </summary>
        /// <param name="hostname">Remote server hostname or IP</param>
        /// <param name="port">SSH port (default 22)</param>
        /// <param name="username">SSH username</param>
        /// <param name="password">SSH password</param>
        /// <returns>True if connection successful, false otherwise</returns>
        public bool Connect(string hostname, int port, string username, string password)
        {
            try
            {
                if (_isConnected)
                {
                    var warningMsg = "Already connected. Disconnect first before reconnecting.";
                    Logger.Instance.Log?.Warn($"[ScpTransferManager.Connect] {warningMsg}");
                    Runtime.MessageCollector?.AddMessage(MessageClass.WarningMsg, warningMsg, true);
                    return true;
                }

                // Validate inputs
                if (string.IsNullOrWhiteSpace(hostname))
                {
                    Logger.Instance.Log?.Error("[ScpTransferManager.Connect] Hostname cannot be empty");
                    throw new ArgumentException("Hostname cannot be empty", nameof(hostname));
                }
                if (string.IsNullOrWhiteSpace(username))
                {
                    Logger.Instance.Log?.Error("[ScpTransferManager.Connect] Username cannot be empty");
                    throw new ArgumentException("Username cannot be empty", nameof(username));
                }
                if (port <= 0 || port > 65535)
                {
                    Logger.Instance.Log?.Warn($"[ScpTransferManager.Connect] Invalid port {port}, using default 22");
                    port = 22;
                }

                var connectMsg = $"Connecting to {username}@{hostname}:{port} via SFTP...";
                Logger.Instance.Log?.Info($"[ScpTransferManager.Connect] {connectMsg}");
                Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg, connectMsg, true);

                // Create cancellation token source for shutdown control
                _shutdownTokenSource?.Dispose();
                _shutdownTokenSource = new CancellationTokenSource();

                // Create authentication method
                var authMethod = new PasswordAuthenticationMethod(username, password ?? string.Empty);
                var connectionInfo = new Renci.SshNet.ConnectionInfo(hostname, port, username, authMethod)
                {
                    Timeout = TimeSpan.FromSeconds(30)
                };

                // Create and connect SFTP client
                _sftpClient = new SftpClient(connectionInfo);

                // Note: We don't set KeepAliveInterval here because it can cause thread issues
                // The default behavior with proper Dispose() should be sufficient

                Logger.Instance.Log?.Debug($"[ScpTransferManager.Connect] Calling SftpClient.Connect() for {hostname}:{port}");
                _sftpClient.Connect();

                if (_sftpClient.IsConnected)
                {
                    _isConnected = true;

                    // Detect mount points for icon differentiation
                    Logger.Instance.Log?.Debug("[ScpTransferManager.Connect] Detecting mount points");
                    DetectMountPoints();

                    var successMsg = $"Successfully connected to {hostname}";
                    Logger.Instance.Log?.Info($"[ScpTransferManager.Connect] {successMsg}");
                    Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg, successMsg, true);
                    return true;
                }
                else
                {
                    var failMsg = $"Failed to connect to {hostname}";
                    Logger.Instance.Log?.Error($"[ScpTransferManager.Connect] {failMsg}");
                    Runtime.MessageCollector?.AddMessage(MessageClass.ErrorMsg, failMsg, true);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Log?.Error($"[ScpTransferManager.Connect] Error connecting to {hostname}:{port}", ex);
                Runtime.MessageCollector?.AddExceptionMessage($"Error connecting to {hostname}:{port}", ex);
                return false;
            }
        }

        /// <summary>
        /// Disconnects from the remote server. Does NOT dispose - call Dispose() for cleanup.
        /// </summary>
        public void Disconnect()
        {
            try
            {
                Logger.Instance.Log?.Debug("[ScpTransferManager.Disconnect] Starting disconnect sequence");

                // Cancel any ongoing operations
                if (_shutdownTokenSource != null && !_shutdownTokenSource.IsCancellationRequested)
                {
                    Logger.Instance.Log?.Debug("[ScpTransferManager.Disconnect] Cancelling ongoing operations");
                    _shutdownTokenSource.Cancel();
                }

                // Disconnect SFTP client (don't dispose yet)
                if (_sftpClient != null && _sftpClient.IsConnected)
                {
                    try
                    {
                        Logger.Instance.Log?.Debug("[ScpTransferManager.Disconnect] Disconnecting SFTP client");
                        _sftpClient.Disconnect();
                        Logger.Instance.Log?.Info("[ScpTransferManager.Disconnect] Disconnected from SFTP server");
                        Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg,
                            "Disconnected from SFTP server", true);
                    }
                    catch (Exception ex)
                    {
                        Logger.Instance.Log?.Error("[ScpTransferManager.Disconnect] Error disconnecting SFTP client", ex);
                        Runtime.MessageCollector?.AddExceptionMessage("Error disconnecting SFTP client", ex);
                    }
                }

                // Disconnect SCP client (don't dispose yet)
                if (_scpClient != null && _scpClient.IsConnected)
                {
                    try
                    {
                        Logger.Instance.Log?.Debug("[ScpTransferManager.Disconnect] Disconnecting SCP client");
                        _scpClient.Disconnect();
                    }
                    catch (Exception ex)
                    {
                        Logger.Instance.Log?.Error("[ScpTransferManager.Disconnect] Error disconnecting SCP client", ex);
                        Runtime.MessageCollector?.AddExceptionMessage("Error disconnecting SCP client", ex);
                    }
                }

                _isConnected = false;
                Logger.Instance.Log?.Debug("[ScpTransferManager.Disconnect] Disconnect complete");
            }
            catch (Exception ex)
            {
                Logger.Instance.Log?.Error("[ScpTransferManager.Disconnect] Error during disconnect", ex);
                Runtime.MessageCollector?.AddExceptionMessage("Error during disconnect", ex);
            }
        }

        #endregion

        #region Directory Operations

        /// <summary>
        /// Lists all files and directories in the specified remote path.
        /// </summary>
        /// <param name="remotePath">Remote directory path</param>
        /// <returns>List of files and directories, or empty list on error</returns>
        public List<ISftpFile> ListDirectory(string remotePath)
        {
            try
            {
                if (!IsConnected)
                {
                    Logger.Instance.Log?.Error("[ScpTransferManager.ListDirectory] Not connected to server");
                    throw new InvalidOperationException("Not connected to server");
                }

                if (string.IsNullOrWhiteSpace(remotePath))
                    remotePath = CurrentDirectory;

                Logger.Instance.Log?.Debug($"[ScpTransferManager.ListDirectory] Listing directory: {remotePath}");

                var files = _sftpClient.ListDirectory(remotePath)
                    .Where(f => f.Name != "." && f.Name != "..")
                    .OrderByDescending(f => f.IsDirectory)
                    .ThenBy(f => f.Name)
                    .ToList();

                Logger.Instance.Log?.Debug($"[ScpTransferManager.ListDirectory] Found {files.Count} items in {remotePath}");
                return files;
            }
            catch (Exception ex)
            {
                Logger.Instance.Log?.Error($"[ScpTransferManager.ListDirectory] Error listing directory: {remotePath}", ex);
                Runtime.MessageCollector?.AddExceptionMessage($"Error listing directory: {remotePath}", ex);
                return new List<ISftpFile>();
            }
        }

        /// <summary>
        /// Gets the user's home directory on the remote server.
        /// </summary>
        /// <returns>Home directory path, or "/" on error</returns>
        public string GetHomeDirectory()
        {
            try
            {
                if (!IsConnected)
                {
                    Logger.Instance.Log?.Warn("[ScpTransferManager.GetHomeDirectory] Not connected, returning default '/'");
                    return "/";
                }

                // SFTP WorkingDirectory is typically the home directory on connect
                var homeDir = _sftpClient.WorkingDirectory ?? "/home/" + _connectionInfo.Username;
                Logger.Instance.Log?.Debug($"[ScpTransferManager.GetHomeDirectory] Home directory: {homeDir}");
                return homeDir;
            }
            catch (Exception ex)
            {
                Logger.Instance.Log?.Error("[ScpTransferManager.GetHomeDirectory] Error getting home directory", ex);
                Runtime.MessageCollector?.AddExceptionMessage("Error getting home directory", ex);
                return "/";
            }
        }

        /// <summary>
        /// Changes the current working directory.
        /// </summary>
        /// <param name="path">Directory path to navigate to</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool ChangeDirectory(string path)
        {
            try
            {
                if (!IsConnected)
                {
                    Logger.Instance.Log?.Warn("[ScpTransferManager.ChangeDirectory] Not connected");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(path))
                {
                    Logger.Instance.Log?.Warn("[ScpTransferManager.ChangeDirectory] Path is empty");
                    return false;
                }

                Logger.Instance.Log?.Debug($"[ScpTransferManager.ChangeDirectory] Changing to: {path}");
                _sftpClient.ChangeDirectory(path);
                Logger.Instance.Log?.Info($"[ScpTransferManager.ChangeDirectory] Changed directory to: {path}");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Instance.Log?.Error($"[ScpTransferManager.ChangeDirectory] Error changing to: {path}", ex);
                Runtime.MessageCollector?.AddExceptionMessage($"Error changing directory to: {path}", ex);
                return false;
            }
        }

        /// <summary>
        /// Checks if a remote path exists and is accessible.
        /// </summary>
        /// <param name="remotePath">Remote path to check</param>
        /// <returns>True if path exists and is accessible</returns>
        public bool PathExists(string remotePath)
        {
            try
            {
                if (!IsConnected)
                {
                    Logger.Instance.Log?.Warn("[ScpTransferManager.PathExists] Not connected");
                    return false;
                }

                Logger.Instance.Log?.Debug($"[ScpTransferManager.PathExists] Checking: {remotePath}");
                var exists = _sftpClient.Exists(remotePath);
                Logger.Instance.Log?.Debug($"[ScpTransferManager.PathExists] Path {remotePath} exists: {exists}");
                return exists;
            }
            catch (Exception ex)
            {
                Logger.Instance.Log?.Error($"[ScpTransferManager.PathExists] Error checking: {remotePath}", ex);
                Runtime.MessageCollector?.AddExceptionMessage($"Error checking path existence: {remotePath}", ex);
                return false;
            }
        }

        #endregion

        #region File Transfer Operations

        /// <summary>
        /// Uploads a local file to the remote server.
        /// </summary>
        /// <param name="localPath">Local file path</param>
        /// <param name="remotePath">Remote destination path</param>
        /// <param name="progressCallback">Optional progress callback (bytes transferred)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task that completes when upload finishes</returns>
        public async Task UploadFileAsync(string localPath, string remotePath,
            Action<ulong> progressCallback = null,
            CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                try
                {
                    if (!IsConnected)
                    {
                        Logger.Instance.Log?.Error("[ScpTransferManager.UploadFileAsync] Not connected to server");
                        throw new InvalidOperationException("Not connected to server");
                    }

                    if (!File.Exists(localPath))
                    {
                        Logger.Instance.Log?.Error($"[ScpTransferManager.UploadFileAsync] Local file not found: {localPath}");
                        throw new FileNotFoundException($"Local file not found: {localPath}");
                    }

                    var uploadMsg = $"Uploading: {Path.GetFileName(localPath)} -> {remotePath}";
                    Logger.Instance.Log?.Info($"[ScpTransferManager.UploadFileAsync] {uploadMsg}");
                    Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg, uploadMsg, true);

                    using (var fileStream = File.OpenRead(localPath))
                    {
                        var fileSize = fileStream.Length;
                        Logger.Instance.Log?.Debug($"[ScpTransferManager.UploadFileAsync] File size: {fileSize} bytes");

                        _sftpClient.UploadFile(fileStream, remotePath, true, bytesUploaded =>
                        {
                            progressCallback?.Invoke(bytesUploaded);
                        });
                    }

                    var completeMsg = $"Upload complete: {Path.GetFileName(localPath)}";
                    Logger.Instance.Log?.Info($"[ScpTransferManager.UploadFileAsync] {completeMsg}");
                    Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg, completeMsg, true);
                }
                catch (Exception ex)
                {
                    Logger.Instance.Log?.Error($"[ScpTransferManager.UploadFileAsync] Error uploading: {localPath}", ex);
                    Runtime.MessageCollector?.AddExceptionMessage($"Error uploading file: {localPath}", ex);
                    throw;
                }
            }, cancellationToken);
        }

        /// <summary>
        /// Downloads a remote file to the local system.
        /// </summary>
        /// <param name="remotePath">Remote file path</param>
        /// <param name="localPath">Local destination path</param>
        /// <param name="progressCallback">Optional progress callback (bytes transferred)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task that completes when download finishes</returns>
        public async Task DownloadFileAsync(string remotePath, string localPath,
            Action<ulong> progressCallback = null,
            CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                try
                {
                    if (!IsConnected)
                    {
                        Logger.Instance.Log?.Error("[ScpTransferManager.DownloadFileAsync] Not connected to server");
                        throw new InvalidOperationException("Not connected to server");
                    }

                    if (!_sftpClient.Exists(remotePath))
                    {
                        Logger.Instance.Log?.Error($"[ScpTransferManager.DownloadFileAsync] Remote file not found: {remotePath}");
                        throw new FileNotFoundException($"Remote file not found: {remotePath}");
                    }

                    var downloadMsg = $"Downloading: {remotePath} -> {Path.GetFileName(localPath)}";
                    Logger.Instance.Log?.Info($"[ScpTransferManager.DownloadFileAsync] {downloadMsg}");
                    Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg, downloadMsg, true);

                    // Ensure local directory exists
                    var localDir = Path.GetDirectoryName(localPath);
                    if (!string.IsNullOrEmpty(localDir) && !Directory.Exists(localDir))
                    {
                        Logger.Instance.Log?.Debug($"[ScpTransferManager.DownloadFileAsync] Creating directory: {localDir}");
                        Directory.CreateDirectory(localDir);
                    }

                    using (var fileStream = File.Create(localPath))
                    {
                        _sftpClient.DownloadFile(remotePath, fileStream, bytesDownloaded =>
                        {
                            progressCallback?.Invoke(bytesDownloaded);
                        });
                        Logger.Instance.Log?.Debug($"[ScpTransferManager.DownloadFileAsync] Downloaded {fileStream.Length} bytes");
                    }

                    var completeMsg = $"Download complete: {Path.GetFileName(localPath)}";
                    Logger.Instance.Log?.Info($"[ScpTransferManager.DownloadFileAsync] {completeMsg}");
                    Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg, completeMsg, true);
                }
                catch (Exception ex)
                {
                    Logger.Instance.Log?.Error($"[ScpTransferManager.DownloadFileAsync] Error downloading: {remotePath}", ex);
                    Runtime.MessageCollector?.AddExceptionMessage($"Error downloading file: {remotePath}", ex);
                    throw;
                }
            }, cancellationToken);
        }

        /// <summary>
        /// Deletes a file from the remote server.
        /// </summary>
        /// <param name="remotePath">Remote file path to delete</param>
        public void DeleteFile(string remotePath)
        {
            if (!_isConnected || _sftpClient == null || !_sftpClient.IsConnected)
                throw new InvalidOperationException("Not connected to remote server");

            try
            {
                Logger.Instance.Log?.Debug($"[ScpTransferManager.DeleteFile] Deleting remote file: {remotePath}");
                _sftpClient.DeleteFile(remotePath);
                Logger.Instance.Log?.Info($"[ScpTransferManager.DeleteFile] Successfully deleted: {remotePath}");
            }
            catch (Exception ex)
            {
                Logger.Instance.Log?.Error($"[ScpTransferManager.DeleteFile] Error deleting file: {remotePath}", ex);
                Runtime.MessageCollector?.AddExceptionMessage($"Error deleting remote file: {remotePath}", ex);
                throw;
            }
        }

        /// <summary>
        /// Deletes a directory and all its contents from the remote server recursively.
        /// SFTP requires directories to be empty before deletion, so this method:
        /// 1. Recursively deletes all subdirectories
        /// 2. Deletes all files in the directory
        /// 3. Finally deletes the now-empty directory
        /// </summary>
        /// <param name="remotePath">Remote directory path to delete</param>
        public void DeleteDirectory(string remotePath)
        {
            if (!_isConnected || _sftpClient == null || !_sftpClient.IsConnected)
                throw new InvalidOperationException("Not connected to remote server");

            try
            {
                Logger.Instance.Log?.Debug($"[ScpTransferManager.DeleteDirectory] Recursively deleting remote directory: {remotePath}");

                // First, get all items in the directory
                var items = _sftpClient.ListDirectory(remotePath)
                    .Where(f => f.Name != "." && f.Name != "..")
                    .ToList();

                // Delete all subdirectories recursively
                foreach (var item in items.Where(f => f.IsDirectory))
                {
                    Logger.Instance.Log?.Debug($"[ScpTransferManager.DeleteDirectory] Recursively deleting subdirectory: {item.FullName}");
                    DeleteDirectory(item.FullName);
                }

                // Delete all files in the directory
                foreach (var item in items.Where(f => !f.IsDirectory))
                {
                    Logger.Instance.Log?.Debug($"[ScpTransferManager.DeleteDirectory] Deleting file: {item.FullName}");
                    _sftpClient.DeleteFile(item.FullName);
                }

                // Finally, delete the now-empty directory
                Logger.Instance.Log?.Debug($"[ScpTransferManager.DeleteDirectory] Deleting empty directory: {remotePath}");
                _sftpClient.DeleteDirectory(remotePath);

                Logger.Instance.Log?.Info($"[ScpTransferManager.DeleteDirectory] Successfully deleted directory and all contents: {remotePath}");
            }
            catch (Exception ex)
            {
                Logger.Instance.Log?.Error($"[ScpTransferManager.DeleteDirectory] Error deleting directory: {remotePath}", ex);
                Runtime.MessageCollector?.AddExceptionMessage($"Error deleting remote directory: {remotePath}", ex);
                throw;
            }
        }

        /// <summary>
        /// Creates a directory on the remote server if it doesn't exist.
        /// </summary>
        /// <param name="remotePath">Remote directory path to create</param>
        public void CreateRemoteDirectory(string remotePath)
        {
            if (!_isConnected || _sftpClient == null || !_sftpClient.IsConnected)
                throw new InvalidOperationException("Not connected to remote server");

            try
            {
                if (!_sftpClient.Exists(remotePath))
                {
                    Logger.Instance.Log?.Debug($"[ScpTransferManager.CreateRemoteDirectory] Creating remote directory: {remotePath}");
                    _sftpClient.CreateDirectory(remotePath);
                    Logger.Instance.Log?.Info($"[ScpTransferManager.CreateRemoteDirectory] Successfully created directory: {remotePath}");
                }
                else
                {
                    Logger.Instance.Log?.Debug($"[ScpTransferManager.CreateRemoteDirectory] Directory already exists: {remotePath}");
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Log?.Error($"[ScpTransferManager.CreateRemoteDirectory] Error creating directory: {remotePath}", ex);
                Runtime.MessageCollector?.AddExceptionMessage($"Error creating remote directory: {remotePath}", ex);
                throw;
            }
        }

        /// <summary>
        /// Recursively uploads a local directory and all its contents to the remote server.
        /// </summary>
        /// <param name="localPath">Local directory path</param>
        /// <param name="remotePath">Remote destination path</param>
        /// <param name="progressCallback">Optional progress callback (current file path)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task that completes when upload finishes</returns>
        public async Task UploadDirectoryAsync(string localPath, string remotePath,
            Action<string> progressCallback = null,
            CancellationToken cancellationToken = default)
        {
            if (!IsConnected)
            {
                Logger.Instance.Log?.Error("[ScpTransferManager.UploadDirectoryAsync] Not connected to server");
                throw new InvalidOperationException("Not connected to server");
            }

            if (!Directory.Exists(localPath))
            {
                Logger.Instance.Log?.Error($"[ScpTransferManager.UploadDirectoryAsync] Local directory not found: {localPath}");
                throw new DirectoryNotFoundException($"Local directory not found: {localPath}");
            }

            try
            {
                Logger.Instance.Log?.Info($"[ScpTransferManager.UploadDirectoryAsync] Starting recursive upload: {localPath} -> {remotePath}");

                // Create the remote directory if it doesn't exist
                CreateRemoteDirectory(remotePath);

                // Get all files in current directory
                var files = Directory.GetFiles(localPath);
                foreach (var file in files)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var fileName = Path.GetFileName(file);
                    var remoteFilePath = $"{remotePath}/{fileName}".Replace("\\", "/");

                    progressCallback?.Invoke(file);
                    await UploadFileAsync(file, remoteFilePath, null, cancellationToken);
                }

                // Recursively upload subdirectories
                var directories = Directory.GetDirectories(localPath);
                foreach (var directory in directories)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var dirName = Path.GetFileName(directory);
                    var remoteSubDirPath = $"{remotePath}/{dirName}".Replace("\\", "/");

                    await UploadDirectoryAsync(directory, remoteSubDirPath, progressCallback, cancellationToken);
                }

                Logger.Instance.Log?.Info($"[ScpTransferManager.UploadDirectoryAsync] Completed recursive upload: {localPath}");
            }
            catch (OperationCanceledException)
            {
                Logger.Instance.Log?.Warn($"[ScpTransferManager.UploadDirectoryAsync] Upload cancelled: {localPath}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Instance.Log?.Error($"[ScpTransferManager.UploadDirectoryAsync] Error uploading directory: {localPath}", ex);
                Runtime.MessageCollector?.AddExceptionMessage($"Error uploading directory: {localPath}", ex);
                throw;
            }
        }

        /// <summary>
        /// Recursively downloads a remote directory and all its contents to the local system.
        /// </summary>
        /// <param name="remotePath">Remote directory path</param>
        /// <param name="localPath">Local destination path</param>
        /// <param name="progressCallback">Optional progress callback (current file path)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task that completes when download finishes</returns>
        public async Task DownloadDirectoryAsync(string remotePath, string localPath,
            Action<string> progressCallback = null,
            CancellationToken cancellationToken = default)
        {
            if (!IsConnected)
            {
                Logger.Instance.Log?.Error("[ScpTransferManager.DownloadDirectoryAsync] Not connected to server");
                throw new InvalidOperationException("Not connected to server");
            }

            if (!_sftpClient.Exists(remotePath))
            {
                Logger.Instance.Log?.Error($"[ScpTransferManager.DownloadDirectoryAsync] Remote directory not found: {remotePath}");
                throw new DirectoryNotFoundException($"Remote directory not found: {remotePath}");
            }

            try
            {
                Logger.Instance.Log?.Info($"[ScpTransferManager.DownloadDirectoryAsync] Starting recursive download: {remotePath} -> {localPath}");

                // Create the local directory if it doesn't exist
                if (!Directory.Exists(localPath))
                {
                    Logger.Instance.Log?.Debug($"[ScpTransferManager.DownloadDirectoryAsync] Creating local directory: {localPath}");
                    Directory.CreateDirectory(localPath);
                }

                // Get all items in the remote directory
                var items = _sftpClient.ListDirectory(remotePath)
                    .Where(f => f.Name != "." && f.Name != "..")
                    .ToList();

                // Download files
                foreach (var item in items.Where(f => !f.IsDirectory))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var localFilePath = Path.Combine(localPath, item.Name);
                    progressCallback?.Invoke(item.FullName);
                    await DownloadFileAsync(item.FullName, localFilePath, null, cancellationToken);
                }

                // Recursively download subdirectories
                foreach (var item in items.Where(f => f.IsDirectory))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var localSubDirPath = Path.Combine(localPath, item.Name);
                    await DownloadDirectoryAsync(item.FullName, localSubDirPath, progressCallback, cancellationToken);
                }

                Logger.Instance.Log?.Info($"[ScpTransferManager.DownloadDirectoryAsync] Completed recursive download: {remotePath}");
            }
            catch (OperationCanceledException)
            {
                Logger.Instance.Log?.Warn($"[ScpTransferManager.DownloadDirectoryAsync] Download cancelled: {remotePath}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Instance.Log?.Error($"[ScpTransferManager.DownloadDirectoryAsync] Error downloading directory: {remotePath}", ex);
                Runtime.MessageCollector?.AddExceptionMessage($"Error downloading directory: {remotePath}", ex);
                throw;
            }
        }

        #endregion

        #region Mount Point Detection

        /// <summary>
        /// Checks if a given path is a mount point or drive.
        /// </summary>
        /// <param name="path">The path to check</param>
        /// <returns>True if the path is a mount point, false otherwise</returns>
        public bool IsMountPoint(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            // Normalize path - remove trailing slashes except for root
            path = path.TrimEnd('/');
            if (string.IsNullOrEmpty(path))
                path = "/";

            return _mountPoints?.Contains(path) ?? false;
        }

        /// <summary>
        /// Detects mount points on the remote system using a hybrid approach:
        /// 1. Try reading /proc/mounts (Linux)
        /// 2. Fallback to treating only / as a mount point
        /// </summary>
        private void DetectMountPoints()
        {
            _mountPoints = new HashSet<string>();

            try
            {
                Logger.Instance.Log?.Debug("[ScpTransferManager.DetectMountPoints] Starting mount point detection");

                // Always add root as a mount point
                _mountPoints.Add("/");

                // Try to read /proc/mounts (Linux-specific)
                if (TryReadProcMounts(out var mountPoints))
                {
                    foreach (var mountPoint in mountPoints)
                    {
                        _mountPoints.Add(mountPoint);
                    }
                    Logger.Instance.Log?.Info($"[ScpTransferManager.DetectMountPoints] Detected {_mountPoints.Count} mount points from /proc/mounts");
                }
                else
                {
                    Logger.Instance.Log?.Debug("[ScpTransferManager.DetectMountPoints] /proc/mounts not available, using root only");
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Log?.Warn("[ScpTransferManager.DetectMountPoints] Error detecting mount points, defaulting to root only", ex);
                _mountPoints.Clear();
                _mountPoints.Add("/");
            }
        }

        /// <summary>
        /// Attempts to read /proc/mounts to get a list of mount points.
        /// </summary>
        /// <param name="mountPoints">Output list of detected mount points</param>
        /// <returns>True if successfully read, false otherwise</returns>
        private bool TryReadProcMounts(out List<string> mountPoints)
        {
            mountPoints = new List<string>();

            try
            {
                if (!PathExists("/proc/mounts"))
                {
                    Logger.Instance.Log?.Debug("[ScpTransferManager.TryReadProcMounts] /proc/mounts does not exist");
                    return false;
                }

                Logger.Instance.Log?.Debug("[ScpTransferManager.TryReadProcMounts] Reading /proc/mounts");

                // Download /proc/mounts to a memory stream
                using (var memoryStream = new MemoryStream())
                {
                    _sftpClient.DownloadFile("/proc/mounts", memoryStream);
                    memoryStream.Position = 0;

                    using (var reader = new StreamReader(memoryStream))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            // Format: <device> <mountpoint> <filesystem> <options> <dump> <pass>
                            // Example: /dev/sda1 / ext4 rw,relatime 0 0
                            var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                            if (parts.Length >= 2)
                            {
                                var mountPoint = parts[1];

                                // Unescape octal sequences (e.g., \040 = space)
                                mountPoint = UnescapeOctalSequences(mountPoint);

                                mountPoints.Add(mountPoint);
                            }
                        }
                    }
                }

                Logger.Instance.Log?.Debug($"[ScpTransferManager.TryReadProcMounts] Found {mountPoints.Count} mount points");
                return mountPoints.Count > 0;
            }
            catch (Exception ex)
            {
                Logger.Instance.Log?.Debug($"[ScpTransferManager.TryReadProcMounts] Failed to read /proc/mounts: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Unescapes octal sequences in mount point paths (e.g., \040 becomes space).
        /// /proc/mounts escapes special characters using octal notation.
        /// </summary>
        private static string UnescapeOctalSequences(string input)
        {
            if (string.IsNullOrEmpty(input) || !input.Contains("\\"))
                return input;

            var result = input;
            // Replace common octal escape sequences
            result = result.Replace("\\040", " ");  // space
            result = result.Replace("\\011", "\t"); // tab
            result = result.Replace("\\012", "\n"); // newline
            result = result.Replace("\\134", "\\"); // backslash

            return result;
        }

        #endregion

        #region Disposal

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                Logger.Instance.Log?.Debug("[ScpTransferManager.Dispose] Already disposed, skipping");
                return;
            }

            if (disposing)
            {
                try
                {
                    Logger.Instance.Log?.Debug("[ScpTransferManager.Dispose] Starting disposal sequence");
                    _isConnected = false;

                    // Cancel any ongoing operations
                    if (_shutdownTokenSource != null)
                    {
                        Logger.Instance.Log?.Debug("[ScpTransferManager.Dispose] Cancelling shutdown token");
                        _shutdownTokenSource.Cancel();
                        _shutdownTokenSource.Dispose();
                        _shutdownTokenSource = null;
                    }

                    // IMPORTANT: Just call Dispose() - it handles Disconnect() internally
                    // Calling Disconnect() first causes deadlocks!
                    if (_sftpClient != null)
                    {
                        Logger.Instance.Log?.Debug("[ScpTransferManager.Dispose] Disposing SFTP client");
                        _sftpClient.Dispose();
                        _sftpClient = null;
                    }

                    if (_scpClient != null)
                    {
                        Logger.Instance.Log?.Debug("[ScpTransferManager.Dispose] Disposing SCP client");
                        _scpClient.Dispose();
                        _scpClient = null;
                    }

                    Logger.Instance.Log?.Info("[ScpTransferManager.Dispose] SCP Transfer Manager disposed successfully");
                    Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg,
                        "SCP Transfer Manager disposed", true);
                }
                catch (Exception ex)
                {
                    Logger.Instance.Log?.Error("[ScpTransferManager.Dispose] Error during disposal", ex);
                    Runtime.MessageCollector?.AddExceptionMessage("Error disposing SCP Transfer Manager", ex);
                }
            }

            _disposed = true;
        }

        #endregion
    }
}
