using System.IO;
using mRemoteNG.Tools;

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

        public static void DeleteDirectory(string directory)
        {
			if (Directory.Exists(directory))
				Directory.Delete(directory, true);
        }

        public static string NewTempFilePath(string extension = "")
        {
            var newPath = Path.Combine(GetTestSpecificTempDirectory(), Path.GetRandomFileName());
            if (!string.IsNullOrWhiteSpace(extension))
	            newPath = newPath + extension;
            var folderPath = Path.GetDirectoryName(newPath);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            return newPath;
        }

		/// <summary>
		/// Gets a testing directory that should be unique for a
		/// particular mRemoteNG test.
		/// </summary>
        public static string GetTestSpecificTempDirectory()
        {
            return Path.Combine(Path.GetTempPath(), "mRemoteNGTests", Path.GetRandomFileName());
        }

		public static DisposableAction DisposableTempFile(out string filePath, string extension = "")
		{
			var file = NewTempFilePath(extension);
			filePath = file;
			File.AppendAllText(file, "");
			return new DisposableAction(
				() => {}, 
				() => DeleteDirectory(Path.GetDirectoryName(file)));
		}
    }
}