using System;
using System.Runtime.Versioning;
using mRemoteNG.App;

namespace mRemoteNG.Config.DataProviders
{
    [SupportedOSPlatform("windows")]
    public class FileDataProviderWithRollingBackup(string filePath) : FileDataProvider(filePath)
    {
        private readonly FileBackupCreator _fileBackupCreator = new FileBackupCreator();

        public override void Save(string content)
        {
            _fileBackupCreator.CreateBackupFile(FilePath);
            try
            {
                WriteContent(content);
            }
            catch (Exception ex)
            {
                // Connection files must not report a failed write as a successful save.
                Runtime.MessageCollector.AddExceptionStackTrace($"Failed to save file {FilePath}", ex);
                throw;
            }
        }
    }
}