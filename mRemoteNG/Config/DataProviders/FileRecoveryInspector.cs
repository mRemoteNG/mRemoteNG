using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace mRemoteNG.Config.DataProviders
{
    internal sealed record FileRecoveryCandidate(string Path, DateTime LastWriteTimeUtc, long Length, string Sha256);

    /// <summary>Lists sibling generations that pass a caller-supplied read-only format/decryption validator.</summary>
    internal static class FileRecoveryInspector
    {
        private const long MaximumCandidateBytes = 64L * 1024 * 1024;

        public static IReadOnlyList<FileRecoveryCandidate> Inspect(string primaryPath, IEnumerable<string> candidates,
            Func<Stream, bool> validateReadOnly)
        {
            ArgumentNullException.ThrowIfNull(candidates);
            ArgumentNullException.ThrowIfNull(validateReadOnly);
            string fullPrimaryPath = Path.GetFullPath(primaryPath ?? throw new ArgumentNullException(nameof(primaryPath)));
            string primaryDirectory = Path.GetDirectoryName(fullPrimaryPath);
            List<FileRecoveryCandidate> valid = new();

            foreach (string candidatePath in candidates.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                string fullCandidatePath = Path.GetFullPath(candidatePath);
                if (!string.Equals(Path.GetDirectoryName(fullCandidatePath), primaryDirectory, StringComparison.OrdinalIgnoreCase))
                    continue;
                if (!File.Exists(fullCandidatePath))
                    continue;

                FileInfo fileInfo = new(fullCandidatePath);
                if ((fileInfo.Attributes & FileAttributes.ReparsePoint) != 0)
                    continue;

                using FileStream stream = new(fullCandidatePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                if (stream.Length <= 0 || stream.Length > MaximumCandidateBytes || !validateReadOnly(stream))
                    continue;
                stream.Position = 0;
                string fingerprint = Convert.ToHexString(SHA256.HashData(stream));
                valid.Add(new FileRecoveryCandidate(fullCandidatePath, fileInfo.LastWriteTimeUtc, stream.Length, fingerprint));
            }

            return valid.OrderByDescending(candidate => candidate.LastWriteTimeUtc).ToArray();
        }
    }
}
