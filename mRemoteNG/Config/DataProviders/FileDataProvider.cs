using System;
using System.IO;
using System.Runtime.Versioning;
using System.Threading;
using mRemoteNG.App;
using mRemoteNG.Tools;

namespace mRemoteNG.Config.DataProviders
{
    [SupportedOSPlatform("windows")]
    public class FileDataProvider : IDataProvider<string>
    {
        private string _filePath;

        [SupportedOSPlatform("windows")]
        public string FilePath
        {
            get => _filePath;
            set
            {
                PathValidator.ValidatePathOrThrow(value, nameof(FilePath));
                _filePath = value;
            }
        }

        public FileDataProvider(string filePath)
        {
            PathValidator.ValidatePathOrThrow(filePath, nameof(filePath));
            _filePath = filePath;
        }

        public virtual string Load()
        {
            string fileContents = "";
            try
            {
                if (!File.Exists(FilePath))
                {
                    CreateMissingDirectories();
                    File.WriteAllLines(FilePath, new []{ $@"<?xml version=""1.0"" encoding=""UTF-8""?>", $@"<LocalConnections/>" });
                }

                // Retry read in case the file is momentarily locked by a cloud sync service (e.g. OneDrive).
                const int maxAttempts = 5;
                for (int attempt = 1; attempt <= maxAttempts; attempt++)
                {
                    try
                    {
                        fileContents = File.ReadAllText(FilePath);
                        break;
                    }
                    catch (IOException ex) when (attempt < maxAttempts && ex is not FileNotFoundException)
                    {
                        Thread.Sleep(200 * attempt);
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(
                                                                $"Could not load file. File does not exist '{FilePath}'",
                                                                ex);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace($"Failed to load file {FilePath}", ex);
            }

            return fileContents;
        }

        public virtual void Save(string content)
        {
            string tempPath = FilePath + ".tmp";
            try
            {
                CreateMissingDirectories();

                // Write to a temp file first, then atomically rename/replace.
                // This avoids cloud sync services (e.g. OneDrive) getting stuck because
                // the target file is briefly open for the entire duration of a direct overwrite.
                // Use FileStream with Flush(flushToDisk: true) to ensure data reaches
                // the physical disk before replace — prevents data loss on hibernation (#2150).
                byte[] bytes = new System.Text.UTF8Encoding(false).GetBytes(content);
                using (var fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Flush(flushToDisk: true);
                }

                // Retry the rename/replace in case the target is momentarily locked by the sync process.
                const int maxAttempts = 5;
                for (int attempt = 1; attempt <= maxAttempts; attempt++)
                {
                    try
                    {
                        if (File.Exists(FilePath))
                            File.Replace(tempPath, FilePath, destinationBackupFileName: null);
                        else
                            File.Move(tempPath, FilePath);

                        // Verify saved file is non-empty (catch catastrophic zero-byte corruption)
                        if (content.Length > 0)
                        {
                            var savedInfo = new FileInfo(FilePath);
                            if (savedInfo.Length == 0)
                            {
                                Runtime.MessageCollector.AddExceptionStackTrace(
                                    $"Save verification failed: {FilePath} is empty after save",
                                    new IOException("File is 0 bytes after successful replace"));
                            }
                        }
                        return;
                    }
                    catch (IOException) when (attempt < maxAttempts)
                    {
                        Thread.Sleep(200 * attempt);
                    }
                    catch (UnauthorizedAccessException) when (attempt < maxAttempts)
                    {
                        Thread.Sleep(200 * attempt);
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace($"Failed to save file {FilePath}", ex);
            }
            finally
            {
                // Clean up temp file if it still exists after a failed save.
                try { if (File.Exists(tempPath)) File.Delete(tempPath); } catch { /* best effort */ }
            }
        }

        public virtual void MoveTo(string newPath)
        {
            try
            {
                PathValidator.ValidatePathOrThrow(newPath, nameof(newPath));
                File.Move(FilePath, newPath);
                FilePath = newPath;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace($"Failed to move file {FilePath} to {newPath}", ex);
            }
        }

        private void CreateMissingDirectories()
        {
            string? dirname = Path.GetDirectoryName(FilePath);
            if (dirname == null) return;
            Directory.CreateDirectory(dirname);
        }
    }
}