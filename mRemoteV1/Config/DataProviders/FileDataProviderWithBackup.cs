using System;
using System.IO;
using mRemoteNG.App;

namespace mRemoteNG.Config.DataProviders
{
    public class FileDataProviderWithBackup : FileDataProvider
    {
        protected string BackupFileSuffix = ".backup";

        public FileDataProviderWithBackup(string filePath) : base(filePath)
        {
        }

        public override void Save(string content)
        {
            CreateBackup();
            base.Save(content);
        }

        protected virtual void CreateBackup()
        {
            var backupFileName = FilePath + BackupFileSuffix;
            try
            {
                File.Copy(FilePath, backupFileName, true);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace($"Failed to create backup of file {FilePath}", ex);
                throw;
            }
        }
    }
}