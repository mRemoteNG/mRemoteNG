using System.IO;

namespace mRemoteNG.Config
{
    public class FileDataProvider : IDataProvider
    {
        public string FilePath { get; set; }

        public FileDataProvider(string filePath)
        {
            FilePath = filePath;
        }

        public string Load()
        {
            return File.ReadAllText(FilePath);
        }

        public void Save(string content)
        {
            File.WriteAllText(FilePath, content);
        }
    }
}