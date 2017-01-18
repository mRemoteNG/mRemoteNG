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
                // The file that we are attempting to backup doesn't exist... No need to throw in that scenario...
                if (!File.Exists(FilePath))
                    return;

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