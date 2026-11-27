using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace mRemoteNG.Config.DataProviders
{
    internal sealed class FileRecoveryManager
    {
        private const int MaximumCandidatesToInspect = 10;
        private readonly FileRecoveryInspector _inspector = new();
        private readonly FileRecoveryRestorer _restorer = new();

        public string TryRestore(string primaryPath,
            Func<IReadOnlyList<FileRecoveryCandidate>, FileRecoveryCandidate> chooseCandidate,
            Func<FileRecoveryCandidate, bool> validateSelectedCandidate)
        {
            ArgumentNullException.ThrowIfNull(chooseCandidate);
            ArgumentNullException.ThrowIfNull(validateSelectedCandidate);

            string fullPrimary = Path.GetFullPath(primaryPath ?? throw new ArgumentNullException(nameof(primaryPath)));
            string directory = Path.GetDirectoryName(fullPrimary)!;
            string pattern = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                Properties.OptionsBackupPage.Default.BackupFileNameFormat, Path.GetFileName(fullPrimary), "*");
            string[] paths = Directory.EnumerateFiles(directory, pattern)
                .Take(MaximumCandidatesToInspect)
                .ToArray();

            IReadOnlyList<FileRecoveryCandidate> candidates = _inspector.Inspect(fullPrimary, paths, IsValidXmlDocument);
            if (candidates.Count == 0)
                return null;

            FileRecoveryCandidate selected = chooseCandidate(candidates);
            if (selected == null || !validateSelectedCandidate(selected))
                return null;

            string expectedPrimaryHash = File.Exists(fullPrimary)
                ? FileRecoveryRestorer.ComputeSha256(fullPrimary)
                : null;
            return _restorer.Restore(fullPrimary, selected, selected.Sha256, expectedPrimaryHash, IsValidXmlDocument);
        }

        private static bool IsValidXmlDocument(Stream stream)
        {
            try
            {
                using StreamReader reader = new(stream, new UTF8Encoding(false, true), true, 4096, true);
                string content = reader.ReadToEnd();
                if (content.Length > Import.ImportFileReader.MaximumCharacters)
                    return false;
                System.Xml.XmlDocument document = Security.SecureXmlHelper.LoadXmlFromString(content);
                return document.DocumentElement?.Name == "Connections";
            }
            catch
            {
                return false;
            }
        }
    }
}
