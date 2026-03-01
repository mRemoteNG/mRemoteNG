using System;
using System.Globalization;
using System.IO;
using System.Runtime.Versioning;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Resources.Language;
using mRemoteNG.Tools;

namespace mRemoteNG.Config.DataProviders
{
    public class FileBackupCreator
    {
        [SupportedOSPlatform("windows")]
        public static void CreateBackupFile(string fileName)
        {
            try
            {
                PathValidator.ValidatePathOrThrow(fileName, nameof(fileName));

                if (WeDontNeedToBackup(fileName))
                    return;

                string backupFileName =
                    string.Format(CultureInfo.InvariantCulture, Properties.OptionsBackupPage.Default.BackupFileNameFormat, fileName, DateTime.Now);
                
                PathValidator.ValidatePathOrThrow(backupFileName, nameof(backupFileName));
                
                File.Copy(fileName, backupFileName);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(Language.ConnectionsFileBackupFailed, ex,
                                                             MessageClass.WarningMsg);
                throw;
            }
        }

        private static bool WeDontNeedToBackup(string filePath)
        {
            return FeatureIsTurnedOff() || FileDoesntExist(filePath);
        }

        private static bool FileDoesntExist(string filePath)
        {
            return !File.Exists(filePath);
        }

        private static bool FeatureIsTurnedOff()
        {
            return Properties.OptionsBackupPage.Default.BackupFileKeepCount == 0
                || !Properties.OptionsBackupPage.Default.BackupConnectionsOnSave;
        }
    }
}