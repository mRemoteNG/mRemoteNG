using System;
using System.IO;
using System.Threading;
using mRemoteNG.App;
using mRemoteNG.Config.Connections.Multiuser;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Connections.Multiuser
{
    [TestFixture]
    public class FileConnectionsUpdateCheckerTests
    {
        private string _tempFile;

        [SetUp]
        public void Setup()
        {
            _tempFile = Path.GetTempFileName();
        }

        [TearDown]
        public void Teardown()
        {
            if (File.Exists(_tempFile))
                File.Delete(_tempFile);
        }

        [Test]
        public void IsUpdateAvailable_ReturnsTrue_WhenFileIsNewer()
        {
            // Set LastFileUpdate to past
            Runtime.ConnectionsService.LastFileUpdate = DateTime.UtcNow.AddMinutes(-10);
            
            // File created now
            var checker = new FileConnectionsUpdateChecker(_tempFile);
            
            Assert.That(checker.IsUpdateAvailable(), Is.True);
        }

        [Test]
        public void IsUpdateAvailable_ReturnsFalse_WhenFileIsOlder()
        {
            // Set LastFileUpdate to future
            Runtime.ConnectionsService.LastFileUpdate = DateTime.UtcNow.AddMinutes(10);
            
            var checker = new FileConnectionsUpdateChecker(_tempFile);
            
            Assert.That(checker.IsUpdateAvailable(), Is.False);
        }
        
        [Test]
        public void IsUpdateAvailable_ReturnsFalse_WhenFileIsSameAge()
        {
             // This is tricky because of file system precision.
             // Let's set LastFileUpdate to file's write time.
             DateTime lastWrite = File.GetLastWriteTimeUtc(_tempFile);
             
             // Truncate milliseconds as in implementation
             long ticks = lastWrite.Ticks - (lastWrite.Ticks % TimeSpan.TicksPerSecond);
             DateTime lastWriteNoMs = new DateTime(ticks, lastWrite.Kind);
             
             Runtime.ConnectionsService.LastFileUpdate = lastWriteNoMs;
             
             var checker = new FileConnectionsUpdateChecker(_tempFile);
             
             Assert.That(checker.IsUpdateAvailable(), Is.False);
        }
    }
}
