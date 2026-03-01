using System.IO;
using System.Xml;

namespace mRemoteNG.Security
{
    /// <summary>
    /// Helper class for securely loading XML documents to prevent XXE (XML External Entity) attacks
    /// </summary>
    public static class SecureXmlHelper
    {
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
            using (XmlReader reader = XmlReader.Create(filePath, settings))
            {
                XmlDocument xmlDocument = CreateSecureXmlDocument();
                xmlDocument.Load(reader);
                return xmlDocument;
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
                MaxCharactersFromEntities = 1024, // Allow standard XML character references but limit expansion
                IgnoreProcessingInstructions = true, // Ignore processing instructions
                IgnoreComments = true // Ignore comments
            };
        }
    }
}
