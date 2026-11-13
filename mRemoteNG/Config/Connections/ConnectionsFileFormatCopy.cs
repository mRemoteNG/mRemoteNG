using System;
using System.IO;
using System.Runtime.Versioning;
using System.Xml;
using mRemoteNG.Tools;

namespace mRemoteNG.Config.Connections
{
    /// <summary>
    /// Keeps a copy of a connections file before it is saved in a newer format, which the versions
    /// of mRemoteNG that wrote it cannot open. The copy is named after the format it is in, such as
    /// confCons.xml.v2.6.bak, so the pruning of the rolling backups never matches or deletes it.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class ConnectionsFileFormatCopy
    {
        /// <returns>The path of the copy, or null when none was made.</returns>
        public static string KeepIfUpgrading(string filePath, string newContent)
        {
            if (!File.Exists(filePath)) return null;

            Version newVersion = ReadVersion(new StringReader(newContent));
            Version oldVersion;
            using (StreamReader reader = File.OpenText(filePath))
                oldVersion = ReadVersion(reader);
            if (newVersion == null || oldVersion == null || oldVersion >= newVersion) return null;

            string copyPath = $"{filePath}.v{oldVersion.ToString(2)}.bak";
            // Never replaced: an older version that rejects the upgraded file can write an empty
            // file in its own format, and a second copy would then overwrite the one with the data.
            if (File.Exists(copyPath)) return null;

            PathValidator.ValidatePathOrThrow(copyPath, nameof(copyPath));
            File.Copy(filePath, copyPath);
            return copyPath;
        }

        /// <summary>
        /// Reads ConfVersion from the root element only. Null when the content is not XML, as in
        /// files that old versions encrypted whole, or has no version that can be read.
        /// </summary>
        internal static Version ReadVersion(TextReader content)
        {
            XmlReaderSettings settings = new()
            {
                DtdProcessing = DtdProcessing.Prohibit,
                XmlResolver = null,
                IgnoreComments = true,
                IgnoreProcessingInstructions = true
            };
            try
            {
                using XmlReader reader = XmlReader.Create(content, settings);
                if (reader.MoveToContent() != XmlNodeType.Element) return null;
                string value = reader.GetAttribute("ConfVersion")?.Replace(",", ".");
                return Version.TryParse(value, out Version version) ? version : null;
            }
            catch (XmlException)
            {
                return null;
            }
        }
    }
}
