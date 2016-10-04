using System;
using mRemoteNG.App;

namespace mRemoteNG.Config.DataProviders
{
    public class FileDataProviderWithRollingBackup : FileDataProviderWithBackup
    {
        public FileDataProviderWithRollingBackup(string filePath) : base(filePath)
        {
        }

        protected override void CreateBackup()
        {
            CreateRollingBackup();
            base.CreateBackup();
        }

        protected virtual void CreateRollingBackup()
        {
            var timeStamp = $"{DateTime.Now:yyyyMMdd-HHmmss-ffff}";
            var normalBackup = new FileDataProviderWithBackup(FilePath + BackupFileSuffix);
            var normalBackupWithoutSuffix = normalBackup.FilePath.Replace(BackupFileSuffix, "");
            try
            {
                normalBackup.MoveTo(normalBackupWithoutSuffix + timeStamp + BackupFileSuffix);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace($"Failed to create rolling backup of file {FilePath}", ex);
                throw;
            }
        }
    }
}