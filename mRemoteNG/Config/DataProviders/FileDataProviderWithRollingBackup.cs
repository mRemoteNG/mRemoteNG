using System.Runtime.Versioning;

namespace mRemoteNG.Config.DataProviders
{
    [SupportedOSPlatform("windows")]
    public class FileDataProviderWithRollingBackup : FileDataProvider
    {
        private readonly FileBackupCreator _fileBackupCreator;

        public FileDataProviderWithRollingBackup(string filePath) : base(filePath)
        {
            _fileBackupCreator = new FileBackupCreator();
        }

        public override void Save(string content)
        {
            _fileBackupCreator.CreateBackupFile(FilePath);
            base.Save(content);
        }
    }
}