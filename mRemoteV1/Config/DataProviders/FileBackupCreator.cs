using System;
using System.IO;
using mRemoteNG.App;
using mRemoteNG.Messages;

namespace mRemoteNG.Config.DataProviders
{
    public class FileBackupCreator
    {
        public void CreateBackupFile(string fileName)
        {
            try
            {
                if (WeDontNeedToBackup(fileName))
                    return;

                var backupFileName = string.Format(mRemoteNG.Settings.Default.BackupFileNameFormat, fileName, DateTime.Now);
                File.Copy(fileName, backupFileName);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(Language.strConnectionsFileBackupFailed, ex, MessageClass.WarningMsg);
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
            return mRemoteNG.Settings.Default.BackupFileKeepCount == 0;
        }
    }
}