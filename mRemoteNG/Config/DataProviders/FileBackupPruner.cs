using System.IO;
using System.Linq;
using mRemoteNG.Config.Connections;
using mRemoteNG.Tools;

namespace mRemoteNG.Config.DataProviders
{
    public class FileBackupPruner
    {
        public void PruneBackupFiles(string filePath, int maxBackupsToKeep)
        {
            PathValidator.ValidatePathOrThrow(filePath, nameof(filePath));

            string fileName = Path.GetFileName(filePath);
            string directoryName = Path.GetDirectoryName(filePath);

            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(directoryName))
                return;

            string searchPattern = string.Format(Properties.OptionsBackupPage.Default.BackupFileNameFormat, fileName, "*");
            // A custom format such as {0}.{1}.bak also matches the copy kept before a format upgrade.
            string[] files = Directory.GetFiles(directoryName, searchPattern)
                                      .Where(f => !ConnectionsFileFormatCopy.IsCopyOf(filePath, f))
                                      .ToArray();

            if (files.Length <= maxBackupsToKeep)
                return;

            System.Collections.Generic.IEnumerable<string> filesToDelete = files
                                .OrderByDescending(s => s)
                                .Skip(maxBackupsToKeep);

            foreach (string file in filesToDelete)
            {
                File.Delete(file);
            }
        }
    }
}