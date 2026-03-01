using System.IO;
using System.IO.Compression;
using NUnit.Framework;
using mRemoteNG.Tools;
using mRemoteNGTests.TestHelpers;

namespace mRemoteNGTests.Tools
{
    [TestFixture]
    public class DebugDumperTests
    {
        [Test]
        public void CreateDebugBundle_CreatesZipFileWithEntries()
        {
            // Arrange
            string tempZipPath = FileTestHelpers.NewTempFilePath(".zip");
            
            try
            {
                // Act
                DebugDumper.CreateDebugBundle(tempZipPath);
                
                // Assert
                Assert.That(File.Exists(tempZipPath), Is.True, "Zip file should be created");
                
                using (var archive = ZipFile.OpenRead(tempZipPath))
                {
                    var systemInfoEntry = archive.GetEntry("SystemInfo.txt");
                    Assert.That(systemInfoEntry, Is.Not.Null, "SystemInfo.txt should exist");
                    
                    // Log file might not exist if run in test env without log setup
                    var logEntry = archive.GetEntry("mRemoteNG.log");
                    var logErrorEntry = archive.GetEntry("mRemoteNG.log.error.txt");
                    var logMissingEntry = archive.GetEntry("mRemoteNG.log.missing.txt");
                    Assert.That(logEntry != null || logErrorEntry != null || logMissingEntry != null, Is.True, "Log file, error log, or missing marker should exist");
                    
                    // Config file might not exist or be default
                    var configEntry = archive.GetEntry("confCons.xml");
                    var configErrorEntry = archive.GetEntry("confCons.xml.error.txt");
                    var configMissingEntry = archive.GetEntry("confCons.xml.missing.txt");
                    Assert.That(configEntry != null || configErrorEntry != null || configMissingEntry != null, Is.True, "Config file or error log should exist");
                }
            }
            finally
            {
                if (File.Exists(tempZipPath)) File.Delete(tempZipPath);
            }
        }
    }
}
