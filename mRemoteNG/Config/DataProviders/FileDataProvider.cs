using System;
using System.IO;
using System.Runtime.Versioning;
using mRemoteNG.App;
using mRemoteNG.Tools;

namespace mRemoteNG.Config.DataProviders
{
    [SupportedOSPlatform("windows")]
    public class FileDataProvider : IDataProvider<string>
    {
        private string _filePath;

        [SupportedOSPlatform("windows")]
        public string FilePath
        {
            get => _filePath;
            set
            {
                PathValidator.ValidatePathOrThrow(value, nameof(FilePath));
                _filePath = value;
            }
        }

        public FileDataProvider(string filePath)
        {
            PathValidator.ValidatePathOrThrow(filePath, nameof(filePath));
            _filePath = filePath;
        }

        public virtual string Load()
        {
            string fileContents = "";
            try
            {
                if (!File.Exists(FilePath))
                {
                    CreateMissingDirectories();
                    File.WriteAllLines(FilePath, new []{ $@"<?xml version=""1.0"" encoding=""UTF-8""?>", $@"<LocalConnections/>" });
                }
                fileContents = File.ReadAllText(FilePath);
            }
            catch (FileNotFoundException ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(
                                                                $"Could not load file. File does not exist '{FilePath}'",
                                                                ex);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace($"Failed to load file {FilePath}", ex);
            }

            return fileContents;
        }

        public virtual void Save(string content)
        {
            try
            {
                CreateMissingDirectories();
                AtomicFile.WriteAllText(FilePath, content);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace($"Failed to save file {FilePath}", ex);
                throw;
            }
        }

        public virtual void MoveTo(string newPath)
        {
            try
            {
                PathValidator.ValidatePathOrThrow(newPath, nameof(newPath));
                File.Move(FilePath, newPath);
                FilePath = newPath;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace($"Failed to move file {FilePath} to {newPath}", ex);
            }
        }

        private void CreateMissingDirectories()
        {
            string dirname = Path.GetDirectoryName(FilePath);
            if (dirname == null) return;
            Directory.CreateDirectory(dirname);
        }
    }
}
