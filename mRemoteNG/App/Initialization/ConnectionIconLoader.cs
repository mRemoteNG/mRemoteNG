using System;
using System.IO;
using System.Runtime.Versioning;
using mRemoteNG.Connection;


namespace mRemoteNG.App.Initialization
{
    [SupportedOSPlatform("windows")]
    public class ConnectionIconLoader
    {
        private readonly string _path;

        public ConnectionIconLoader(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath))
                throw new ArgumentException($"{nameof(folderPath)} must be a valid folder path.", nameof(folderPath));

            _path = folderPath;
        }

        public void GetConnectionIcons()
        {
            if (Directory.Exists(_path) == false)
                return;

            foreach (string f in Directory.GetFiles(_path, "*.ico", SearchOption.AllDirectories))
            {
                string relativePath = f.Substring(_path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Length)
                    .TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                    .Replace(".ico", "", StringComparison.Ordinal);
                Array.Resize(ref ConnectionIcon.Icons, ConnectionIcon.Icons.Length + 1);
                ConnectionIcon.Icons.SetValue(relativePath, ConnectionIcon.Icons.Length - 1);
            }
        }
    }
}