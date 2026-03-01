using System;
using System.Globalization;
using System.IO;
using System.Linq;
using mRemoteNG.Tools;

namespace mRemoteNG.Config.DataProviders
{
    public class FileBackupPruner
    {
        public static void PruneBackupFiles(string filePath, int maxBackupsToKeep)
        {
            PathValidator.ValidatePathOrThrow(filePath, nameof(filePath));

            string fileName = Path.GetFileName(filePath);
            string? directoryName = Path.GetDirectoryName(filePath);

            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(directoryName))
                return;

            string searchPattern = string.Format(CultureInfo.InvariantCulture, Properties.OptionsBackupPage.Default.BackupFileNameFormat, fileName, "*");
            string[] files = Directory.GetFiles(directoryName, searchPattern);

            if (files.Length <= maxBackupsToKeep)
                return;

            System.Collections.Generic.IEnumerable<string> filesToDelete = files
                                .OrderByDescending(s => s, StringComparer.Ordinal)
                                .Skip(maxBackupsToKeep);

            foreach (string file in filesToDelete)
            {
                File.Delete(file);
            }
        }
    }
}