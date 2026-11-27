using System;
using System.IO;
using System.Text;
using System.Threading;
using mRemoteNG.Config.Import;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Import
{
    [TestFixture]
    public class ImportFileReaderTests
    {
        private string _directory;

        [SetUp]
        public void SetUp()
        {
            _directory = Path.Combine(Path.GetTempPath(), $"mRemoteNG-import-reader-{Guid.NewGuid():N}");
            Directory.CreateDirectory(_directory);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_directory))
                Directory.Delete(_directory, true);
        }

        [Test]
        public void ReadText_ExcessFileLength_RejectsBeforeReadingContent()
        {
            string path = Path.Combine(_directory, "large.txt");
            using (FileStream file = new(path, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                file.SetLength(ImportFileReader.MaximumFileBytes + 1);

            Assert.Throws<InvalidDataException>(() => ImportFileReader.ReadText(path));
        }

        [Test]
        public void ReadText_InvalidUtf8_RejectsMalformedInput()
        {
            string path = Path.Combine(_directory, "invalid.txt");
            File.WriteAllBytes(path, [0xC3, 0x28]);

            Assert.Throws<DecoderFallbackException>(() => ImportFileReader.ReadText(path));
        }

        [Test]
        public void ReadText_CancelledToken_StopsBeforeReading()
        {
            string path = Path.Combine(_directory, "input.txt");
            File.WriteAllText(path, "not imported", new UTF8Encoding(false));
            using CancellationTokenSource cancellation = new();
            cancellation.Cancel();

            Assert.Throws<OperationCanceledException>(() => ImportFileReader.ReadText(path, cancellation.Token));
        }
    }
}
