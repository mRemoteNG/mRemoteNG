using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using mRemoteNG.Config.DataProviders;
using NUnit.Framework;

namespace mRemoteNGTests.Config.DataProviders
{
    [TestFixture]
    public class FileRecoveryInspectorTests
    {
        private string _directory;

        [SetUp]
        public void SetUp()
        {
            _directory = Path.Combine(Path.GetTempPath(), $"mRemoteNG-recovery-{Guid.NewGuid():N}");
            Directory.CreateDirectory(_directory);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_directory)) Directory.Delete(_directory, true);
        }

        [Test]
        public void Inspect_ReturnsOnlyValidatedSiblingCandidatesWithoutChangingThem()
        {
            string primary = Path.Combine(_directory, "connections.xml");
            string backup = Path.Combine(_directory, "connections.backup");
            string outside = Path.Combine(Path.GetTempPath(), $"mRemoteNG-outside-{Guid.NewGuid():N}.backup");
            File.WriteAllText(primary, "broken");
            File.WriteAllText(backup, "valid generation");
            File.WriteAllText(outside, "valid but outside");
            try
            {
                var candidates = FileRecoveryInspector.Inspect(primary, [primary, backup, outside], stream =>
                {
                    using StreamReader reader = new(stream, Encoding.UTF8, true, 1024, true);
                    return reader.ReadToEnd() == "valid generation";
                });

                Assert.That(candidates, Has.Count.EqualTo(1));
                Assert.That(candidates[0].Path, Is.EqualTo(backup));
                Assert.That(File.ReadAllText(backup), Is.EqualTo("valid generation"));
            }
            finally
            {
                if (File.Exists(outside)) File.Delete(outside);
            }
        }

        [Test]
        public void Restore_RequiresReviewedPrimaryAndCandidateFingerprints_AndPreservesReplacedFile()
        {
            string primary = Path.Combine(_directory, "connections.xml");
            string candidatePath = Path.Combine(_directory, "connections.backup");
            File.WriteAllText(primary, "current draft");
            File.WriteAllText(candidatePath, "validated prior generation");
            FileRecoveryCandidate candidate = new(candidatePath, DateTime.UtcNow, new FileInfo(candidatePath).Length,
                FileRecoveryRestorer.ComputeSha256(candidatePath));
            string reviewedPrimaryHash = FileRecoveryRestorer.ComputeSha256(primary);

            FileRecoveryRestorer restorer = new();
            string preservedPath = restorer.Restore(primary, candidate, candidate.Sha256, reviewedPrimaryHash,
                stream =>
                {
                    using StreamReader reader = new(stream, Encoding.UTF8, true, 1024, true);
                    return reader.ReadToEnd() == "validated prior generation";
                });

            Assert.That(File.ReadAllText(primary), Is.EqualTo("validated prior generation"));
            Assert.That(preservedPath, Does.Exist);
            Assert.That(File.ReadAllText(preservedPath), Is.EqualTo("current draft"));
            Assert.That(File.ReadAllText(candidatePath), Is.EqualTo("validated prior generation"));
        }

        [Test]
        public void Restore_StalePrimaryFingerprint_RetainsBothCurrentAndCandidateFiles()
        {
            string primary = Path.Combine(_directory, "connections.xml");
            string candidatePath = Path.Combine(_directory, "connections.backup");
            File.WriteAllText(primary, "newer current draft");
            File.WriteAllText(candidatePath, "validated prior generation");
            FileRecoveryCandidate candidate = new(candidatePath, DateTime.UtcNow, new FileInfo(candidatePath).Length,
                FileRecoveryRestorer.ComputeSha256(candidatePath));

            FileRecoveryRestorer restorer = new();
            Assert.Throws<IOException>(() => restorer.Restore(primary, candidate, candidate.Sha256,
                Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes("stale draft"))), stream => true));

            Assert.That(File.ReadAllText(primary), Is.EqualTo("newer current draft"));
            Assert.That(File.ReadAllText(candidatePath), Is.EqualTo("validated prior generation"));
            Assert.That(Directory.GetFiles(_directory, "*.restore.tmp"), Is.Empty);
        }

        [Test]
        public void Restore_WhenCommitFails_PreservesPrimaryAndCandidateAndCleansStagedGeneration()
        {
            string primary = Path.Combine(_directory, "connections.xml");
            string candidatePath = Path.Combine(_directory, "connections.backup");
            File.WriteAllText(primary, "current draft");
            File.WriteAllText(candidatePath, "candidate generation");
            FileRecoveryCandidate candidate = new(candidatePath, DateTime.UtcNow, new FileInfo(candidatePath).Length,
                FileRecoveryRestorer.ComputeSha256(candidatePath));
            FileRecoveryRestorer restorer = new((_, _, _) => throw new IOException("Injected commit fault."));

            Assert.Throws<IOException>(() => restorer.Restore(primary, candidate, candidate.Sha256,
                FileRecoveryRestorer.ComputeSha256(primary), _ => true));

            Assert.That(File.ReadAllText(primary), Is.EqualTo("current draft"));
            Assert.That(File.ReadAllText(candidatePath), Is.EqualTo("candidate generation"));
            Assert.That(Directory.GetFiles(_directory, "*.restore.tmp"), Is.Empty);
            Assert.That(Directory.GetFiles(_directory, "*.replaced-*.bak"), Is.Empty);
        }
    }
}
