using System.IO;

namespace mRemoteNGTests.TestHelpers
{
    public class FileTestHelpers
    {
        public static void DeleteTestFile(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

        public static void DeleteFilesInDirectory(string directory, string fileMatching)
        {
            var filesToDelete = Directory.GetFiles(directory, fileMatching, SearchOption.TopDirectoryOnly);
            foreach (var file in filesToDelete)
                if (File.Exists(file))
                    File.Delete(file);
        }

        public static string NewTempFilePath()
        {
            var newPath = Path.Combine(Path.GetTempPath(), "mRemoteNGTests", Path.GetRandomFileName());
            var folderPath = Path.GetDirectoryName(newPath);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            return newPath;
        }
    }
}