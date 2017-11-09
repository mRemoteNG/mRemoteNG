namespace mRemoteNG.Config.DataProviders
{
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