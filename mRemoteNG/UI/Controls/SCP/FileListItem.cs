using System;
using System.IO;

namespace mRemoteNG.UI.Controls.SCP
{
    /// <summary>
    /// Represents a file or directory item for display in the file browser lists.
    /// Used by both LocalFileBrowserPanel and RemoteFileBrowserPanel.
    /// </summary>
    public class FileListItem
    {
        /// <summary>
        /// Name of the file or directory
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Full path to the file or directory
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// File size in bytes (0 for directories)
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// Formatted file size (e.g., "1.2 MB", "45 KB")
        /// </summary>
        public string SizeFormatted
        {
            get
            {
                if (IsDirectory)
                    return "<DIR>";

                return FormatFileSize(Size);
            }
        }

        /// <summary>
        /// Last modified date/time
        /// </summary>
        public DateTime Modified { get; set; }

        /// <summary>
        /// Formatted modified date (e.g., "Today 10:30 AM", "Yesterday", "Oct 15")
        /// </summary>
        public string ModifiedFormatted
        {
            get
            {
                var now = DateTime.Now;
                var diff = now - Modified;

                if (Modified.Date == now.Date)
                    return Modified.ToString("HH:mm");
                else if (Modified.Date == now.Date.AddDays(-1))
                    return "Yesterday";
                else if (diff.TotalDays < 7)
                    return Modified.ToString("ddd HH:mm");
                else if (Modified.Year == now.Year)
                    return Modified.ToString("MMM dd");
                else
                    return Modified.ToString("MMM dd yyyy");
            }
        }

        /// <summary>
        /// File extension (e.g., ".txt", ".log")
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Type description (e.g., "Folder", "Mount Point", "Text File", "Binary File")
        /// </summary>
        public string Type
        {
            get
            {
                if (IsDirectory)
                {
                    if (IsMountPoint)
                        return "Mount Point";
                    return "Folder";
                }

                if (string.IsNullOrEmpty(Extension))
                    return "File";

                return Extension.TrimStart('.').ToUpperInvariant() + " File";
            }
        }

        /// <summary>
        /// Whether this item is a directory
        /// </summary>
        public bool IsDirectory { get; set; }

        /// <summary>
        /// Whether this directory is a mount point or drive.
        /// Only applicable when IsDirectory is true.
        /// </summary>
        public bool IsMountPoint { get; set; }

        /// <summary>
        /// Icon index for display (could be used with ImageList)
        /// 0 = Folder, 1 = File, 2 = Drive/Mount Point
        /// </summary>
        public int IconIndex
        {
            get
            {
                if (IsMountPoint && IsDirectory)
                    return 2; // Drive/Mount Point icon
                if (IsDirectory)
                    return 0; // Folder icon
                return 1; // File icon
            }
        }

        /// <summary>
        /// Formats a file size in bytes to a human-readable string.
        /// </summary>
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
        /// Creates a FileListItem from a local FileSystemInfo object.
        /// </summary>
        public static FileListItem FromLocalFile(FileSystemInfo fileInfo)
        {
            var isDir = fileInfo is DirectoryInfo;
            var fileSize = 0L;

            if (!isDir && fileInfo is FileInfo fi)
            {
                fileSize = fi.Length;
            }

            return new FileListItem
            {
                Name = fileInfo.Name,
                FullPath = fileInfo.FullName,
                Size = fileSize,
                Modified = fileInfo.LastWriteTime,
                Extension = fileInfo.Extension,
                IsDirectory = isDir
            };
        }
    }
}
