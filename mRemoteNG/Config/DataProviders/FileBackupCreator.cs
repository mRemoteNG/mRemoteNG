using System;
using System.IO;
using System.Runtime.Versioning;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.Config.DataProviders
{
    public class FileBackupCreator
    {
        [SupportedOSPlatform("windows")]
        public void CreateBackupFile(string fileName)
        {
            try
            {
                if (WeDontNeedToBackup(fileName))
                    return;

                var backupFileName =
                    string.Format(Properties.OptionsBackupPage.Default.BackupFileNameFormat, fileName, DateTime.Now);
                File.Copy(fileName, backupFileName);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(Language.ConnectionsFileBackupFailed, ex,
                                                             MessageClass.WarningMsg);
                throw;
            }
        }

        private bool WeDontNeedToBackup(string filePath)
        {
            return FeatureIsTurnedOff() || FileDoesntExist(filePath);
        }

        private bool FileDoesntExist(string filePath)
        {
            return !File.Exists(filePath);
        }

        private bool FeatureIsTurnedOff()
        {
            return Properties.OptionsBackupPage.Default.BackupFileKeepCount == 0;
        }
    }
}