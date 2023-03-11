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
                throw new ArgumentException($"{nameof(folderPath)} must be a valid folder path.");

            _path = folderPath;
        }

        public void GetConnectionIcons()
        {
            if (Directory.Exists(_path) == false)
                return;

            foreach (var f in Directory.GetFiles(_path, "*.ico", SearchOption.AllDirectories))
            {
                var fInfo = new FileInfo(f);
                Array.Resize(ref ConnectionIcon.Icons, ConnectionIcon.Icons.Length + 1);
                ConnectionIcon.Icons.SetValue(fInfo.Name.Replace(".ico", ""), ConnectionIcon.Icons.Length - 1);
            }
        }
    }
}