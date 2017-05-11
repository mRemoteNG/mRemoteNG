using System;
using System.IO;

namespace mRemoteNG.Config.DataProviders
{
    public class FileBackupPruner
    {
        public void PruneBackupFiles(string baseName)
        {
            var fileName = Path.GetFileName(baseName);
            var directoryName = Path.GetDirectoryName(baseName);

            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(directoryName)) return;

            var searchPattern = string.Format(mRemoteNG.Settings.Default.BackupFileNameFormat, fileName, "*");
            var files = Directory.GetFiles(directoryName, searchPattern);

            if (files.Length <= mRemoteNG.Settings.Default.BackupFileKeepCount) return;

            Array.Sort(files);
            Array.Resize(ref files, files.Length - mRemoteNG.Settings.Default.BackupFileKeepCount);

            foreach (var file in files)
            {
                File.Delete(file);
            }
        }
    }
}