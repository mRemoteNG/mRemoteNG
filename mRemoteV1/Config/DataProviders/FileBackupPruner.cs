using System.IO;
using System.Linq;

namespace mRemoteNG.Config.DataProviders
{
    public class FileBackupPruner
    {
        public void PruneBackupFiles(string filePath, int maxBackupsToKeep)
        {
            var fileName = Path.GetFileName(filePath);
            var directoryName = Path.GetDirectoryName(filePath);

            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(directoryName))
                return;

            var searchPattern = string.Format(mRemoteNG.Settings.Default.BackupFileNameFormat, fileName, "*");
            var files = Directory.GetFiles(directoryName, searchPattern);

            if (files.Length <= maxBackupsToKeep)
                return;

            var filesToDelete = files
                .OrderByDescending(s => s)
                .Skip(maxBackupsToKeep);

            foreach (var file in filesToDelete)
            {
                File.Delete(file);
            }
        }
    }
}