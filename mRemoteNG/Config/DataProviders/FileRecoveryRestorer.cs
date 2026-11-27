using System;
using System.IO;
using System.Security.Cryptography;

namespace mRemoteNG.Config.DataProviders
{
    /// <summary>Promotes a user-selected, previously validated sibling recovery generation.</summary>
    internal sealed class FileRecoveryRestorer
    {
        private const long MaximumCandidateBytes = 64L * 1024 * 1024;
        private readonly Action<string, string, string> _commit;

        public FileRecoveryRestorer() : this(CommitGeneration) { }

        internal FileRecoveryRestorer(Action<string, string, string> commit)
        {
            _commit = commit ?? throw new ArgumentNullException(nameof(commit));
        }

        public string Restore(string primaryPath, FileRecoveryCandidate candidate, string expectedCandidateSha256,
            string expectedPrimarySha256, Func<Stream, bool> validateReadOnly)
        {
            ArgumentNullException.ThrowIfNull(candidate);
            ArgumentNullException.ThrowIfNull(expectedCandidateSha256);
            ArgumentNullException.ThrowIfNull(validateReadOnly);

            string fullPrimary = Path.GetFullPath(primaryPath ?? throw new ArgumentNullException(nameof(primaryPath)));
            string directory = Path.GetDirectoryName(fullPrimary)!;
            string fullCandidate = Path.GetFullPath(candidate.Path);
            if (!string.Equals(Path.GetDirectoryName(fullCandidate), directory, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Recovery candidates must be in the destination directory.");
            if (string.Equals(fullCandidate, fullPrimary, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("The primary file cannot be selected as its own recovery candidate.");

            string temporaryPath = Path.Combine(directory, $".{Path.GetFileName(fullPrimary)}.{Guid.NewGuid():N}.restore.tmp");
            string preservedPath = Path.Combine(directory, $"{Path.GetFileName(fullPrimary)}.replaced-{DateTime.UtcNow:yyyyMMddHHmmssfff}-{Guid.NewGuid():N}.bak");
            bool committed = false;
            try
            {
                byte[] validatedBytes;
                using (FileStream candidateStream = new(fullCandidate, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    if ((File.GetAttributes(fullCandidate) & FileAttributes.ReparsePoint) != 0 ||
                        candidateStream.Length <= 0 || candidateStream.Length > MaximumCandidateBytes ||
                        !validateReadOnly(candidateStream))
                        throw new InvalidDataException("The selected recovery candidate did not pass validation.");

                    candidateStream.Position = 0;
                    validatedBytes = new byte[candidateStream.Length];
                    candidateStream.ReadExactly(validatedBytes);
                }

                string actualCandidateHash = Convert.ToHexString(SHA256.HashData(validatedBytes));
                if (!CryptographicOperations.FixedTimeEquals(
                        Convert.FromHexString(actualCandidateHash), Convert.FromHexString(expectedCandidateSha256)))
                    throw new IOException("The recovery candidate changed after it was inspected.");

                using (FileStream staged = new(temporaryPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                {
                    staged.Write(validatedBytes);
                    staged.Flush(true);
                }

                string currentHash = File.Exists(fullPrimary) ? ComputeSha256(fullPrimary) : null;
                if (!string.Equals(currentHash, expectedPrimarySha256, StringComparison.OrdinalIgnoreCase))
                    throw new IOException("The primary file changed after recovery was reviewed.");

                _commit(temporaryPath, fullPrimary, preservedPath);

                committed = true;
                return committed && File.Exists(preservedPath) ? preservedPath : null;
            }
            finally
            {
                if (File.Exists(temporaryPath))
                    File.Delete(temporaryPath);
                if (!committed && File.Exists(preservedPath))
                    File.Delete(preservedPath);
            }
        }

        internal static string ComputeSha256(string path)
        {
            using FileStream stream = new(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            return Convert.ToHexString(SHA256.HashData(stream));
        }

        private static void CommitGeneration(string temporaryPath, string primaryPath, string preservedPath)
        {
            if (File.Exists(primaryPath))
                File.Replace(temporaryPath, primaryPath, preservedPath, false);
            else
                File.Move(temporaryPath, primaryPath);
        }
    }
}
