using System.IO;
using System.Xml;

namespace mRemoteNG.Security
{
    /// <summary>
    /// Helper class for securely loading XML documents to prevent XXE (XML External Entity) attacks
    /// </summary>
    public static class SecureXmlHelper
    {
        private const long MaxCharactersInDocument = 64L * 1024 * 1024;
        private const int MaxElementDepth = 128;
        private const int MaxElementCount = 250_000;
        /// <summary>
        /// Creates an XmlDocument with secure settings that prevent XXE attacks
        /// </summary>
        /// <returns>A new XmlDocument with secure settings</returns>
        public static XmlDocument CreateSecureXmlDocument()
        {
            XmlDocument xmlDocument = new XmlDocument
            {
                XmlResolver = null // Disable external entity resolution
            };
            return xmlDocument;
        }

        /// <summary>
        /// Safely loads XML content from a string into an XmlDocument with XXE protection
        /// </summary>
        /// <param name="xmlContent">The XML content to load</param>
        /// <returns>An XmlDocument with the loaded content</returns>
        public static XmlDocument LoadXmlFromString(string xmlContent)
        {
            XmlReaderSettings settings = CreateSecureReaderSettings();
            using (StringReader stringReader = new StringReader(xmlContent))
            using (XmlReader reader = XmlReader.Create(stringReader, settings))
            {
                ValidateDocumentBudget(reader);
            }

            settings = CreateSecureReaderSettings();
            using (StringReader stringReader = new StringReader(xmlContent))
            using (XmlReader reader = XmlReader.Create(stringReader, settings))
            {
                XmlDocument xmlDocument = CreateSecureXmlDocument();
                xmlDocument.Load(reader);
                return xmlDocument;
            }
        }

        /// <summary>
        /// Safely loads XML content from a file into an XmlDocument with XXE protection
        /// </summary>
        /// <param name="filePath">The path to the XML file</param>
        /// <returns>An XmlDocument with the loaded content</returns>
        public static XmlDocument LoadXmlFromFile(string filePath)
        {
            XmlReaderSettings settings = CreateSecureReaderSettings();
            using FileStream stream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using (XmlReader reader = XmlReader.Create(stream, settings))
            {
                ValidateDocumentBudget(reader);
            }

            stream.Position = 0;
            settings = CreateSecureReaderSettings();
            using (XmlReader reader = XmlReader.Create(stream, settings))
            {
                XmlDocument xmlDocument = CreateSecureXmlDocument();
                xmlDocument.Load(reader);
                return xmlDocument;
            }
        }

        private static void ValidateDocumentBudget(XmlReader reader)
        {
            int elements = 0;
            while (reader.Read())
            {
                if (reader.NodeType != XmlNodeType.Element)
                    continue;

                if (reader.Depth >= MaxElementDepth)
                    throw new XmlException($"XML nesting exceeds the {MaxElementDepth} element depth limit.");
                if (++elements > MaxElementCount)
                    throw new XmlException($"XML element count exceeds the {MaxElementCount} element limit.");
            }
        }

        /// <summary>
        /// Creates XmlReaderSettings with secure defaults to prevent XXE attacks
        /// </summary>
        /// <returns>XmlReaderSettings configured for security</returns>
        private static XmlReaderSettings CreateSecureReaderSettings()
        {
            return new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit, // Prohibit DTD processing
                XmlResolver = null, // Disable external entity resolution
                MaxCharactersFromEntities = 0, // Don't allow entity expansion
                MaxCharactersInDocument = MaxCharactersInDocument,
                IgnoreProcessingInstructions = true, // Ignore processing instructions
                IgnoreComments = true // Ignore comments
            };
        }
    }
}
