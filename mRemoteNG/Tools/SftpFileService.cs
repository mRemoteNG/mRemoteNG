using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace mRemoteNG.Tools
{
    [SupportedOSPlatform("windows")]
    internal class SftpFileService : IDisposable
    {
        private SftpClient _client;
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        public bool IsConnected => _client?.IsConnected == true;
        public string CurrentPath { get; private set; } = "/";
        public string HomePath { get; private set; } = "/";
        public string Host { get; private set; }
        public string User { get; private set; }
        public int Port { get; private set; }

        public void Connect(string host, string user, string password, int port)
        {
            Disconnect();
            Host = host;
            User = user;
            Port = port;
            _client = new SftpClient(host, port, user, password ?? string.Empty);
            _client.Connect();
            HomePath = _client.WorkingDirectory ?? "/";
            CurrentPath = HomePath;
        }

        public void Disconnect()
        {
            if (_client == null) return;
            if (_client.IsConnected)
                _client.Disconnect();
            _client.Dispose();
            _client = null;
        }

        public async Task<List<SftpFileItem>> ListDirectoryAsync(string path)
        {
            await _semaphore.WaitAsync();
            try
            {
                return await Task.Run(() =>
                {
                    var entries = _client.ListDirectory(path);
                    var items = new List<SftpFileItem>();
                    foreach (var entry in entries)
                    {
                        if (entry.Name == ".")
                            continue;
                        items.Add(new SftpFileItem
                        {
                            Name = entry.Name,
                            FullPath = entry.FullName,
                            IsDirectory = entry.IsDirectory,
                            IsSymlink = entry.IsSymbolicLink,
                            Size = entry.IsDirectory ? 0 : entry.Length,
                            LastModified = entry.LastWriteTime,
                            Permissions = FormatPermissions(entry),
                            Owner = entry.Attributes?.UserId.ToString() ?? "",
                            Group = entry.Attributes?.GroupId.ToString() ?? ""
                        });
                    }

                    CurrentPath = path;
                    return items.OrderByDescending(f => f.IsDirectory)
                                .ThenBy(f => f.Name)
                                .ToList();
                });
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task DownloadFileAsync(string remotePath, string localPath, Action<ulong> progressCallback = null)
        {
            await _semaphore.WaitAsync();
            try
            {
                await Task.Run(() =>
                {
                    using var fs = File.Create(localPath);
                    _client.DownloadFile(remotePath, fs, progressCallback);
                });
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task UploadFileAsync(string localPath, string remotePath, Action<ulong> progressCallback = null)
        {
            await _semaphore.WaitAsync();
            try
            {
                await Task.Run(() =>
                {
                    using var fs = File.OpenRead(localPath);
                    _client.UploadFile(fs, remotePath, progressCallback);
                });
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task DeleteAsync(string remotePath, bool isDirectory)
        {
            await _semaphore.WaitAsync();
            try
            {
                await Task.Run(() =>
                {
                    if (isDirectory)
                        _client.DeleteDirectory(remotePath);
                    else
                        _client.DeleteFile(remotePath);
                });
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task RenameAsync(string oldPath, string newPath)
        {
            await _semaphore.WaitAsync();
            try
            {
                await Task.Run(() => _client.RenameFile(oldPath, newPath));
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task CreateDirectoryAsync(string path)
        {
            await _semaphore.WaitAsync();
            try
            {
                await Task.Run(() => _client.CreateDirectory(path));
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private static string FormatPermissions(ISftpFile entry)
        {
            try
            {
                var a = entry.Attributes;
                if (a == null)
                    return "";
                var chars = new char[9];
                chars[0] = a.OwnerCanRead ? 'r' : '-';
                chars[1] = a.OwnerCanWrite ? 'w' : '-';
                chars[2] = a.OwnerCanExecute ? 'x' : '-';
                chars[3] = a.GroupCanRead ? 'r' : '-';
                chars[4] = a.GroupCanWrite ? 'w' : '-';
                chars[5] = a.GroupCanExecute ? 'x' : '-';
                chars[6] = a.OthersCanRead ? 'r' : '-';
                chars[7] = a.OthersCanWrite ? 'w' : '-';
                chars[8] = a.OthersCanExecute ? 'x' : '-';
                return new string(chars);
            }
            catch
            {
                return "";
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Disconnect();
                _semaphore.Dispose();
            }
        }
    }

    internal class SftpFileItem
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public bool IsDirectory { get; set; }
        public bool IsSymlink { get; set; }
        public long Size { get; set; }
        public DateTime LastModified { get; set; }
        public string Permissions { get; set; }
        public string Owner { get; set; }
        public string Group { get; set; }
    }
}
