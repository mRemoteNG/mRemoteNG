using System.Runtime.Versioning;

namespace mRemoteNG.Config.DataProviders
{
    [SupportedOSPlatform("windows")]
    public class FileDataProviderWithRollingBackup(string filePath) : FileDataProvider(filePath)
    {
        private readonly FileBackupCreator _fileBackupCreator = new FileBackupCreator();

        public override void Save(string content)
        {
            FileBackupCreator.CreateBackupFile(FilePath);
            base.Save(content);
        }
    }
}