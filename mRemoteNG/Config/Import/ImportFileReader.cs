using System;
using System.IO;
using System.Text;
using System.Threading;
using mRemoteNG.Tools;

namespace mRemoteNG.Config.Import
{
    /// <summary>Applies common byte and decoded-character budgets to text file imports.</summary>
    internal static class ImportFileReader
    {
        public const long MaximumFileBytes = 16 * 1024 * 1024;
        public const int MaximumCharacters = 16 * 1024 * 1024;

        public static string ReadText(string path, CancellationToken cancellationToken = default)
        {
            if (!cancellationToken.CanBeCanceled) cancellationToken = ImportCancellationContext.Current;
            cancellationToken.ThrowIfCancellationRequested();
            PathValidator.ValidatePathOrThrow(path, nameof(path));
            using FileStream stream = new(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            if (stream.Length > MaximumFileBytes)
                throw new InvalidDataException($"Import exceeds the {MaximumFileBytes} byte limit.");

            using StreamReader reader = new(stream, new UTF8Encoding(false, true), true);
            StringBuilder contents = new();
            char[] buffer = new char[8192];
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                int read = reader.Read(buffer, 0, buffer.Length);
                if (read == 0)
                    return contents.ToString();
                if (contents.Length + read > MaximumCharacters)
                    throw new InvalidDataException($"Decoded import exceeds the {MaximumCharacters} character limit.");
                contents.Append(buffer, 0, read);
            }
        }
    }
}
