using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using mRemoteNG.Credential;
using mRemoteNG.Security;

namespace mRemoteNG.Config.Serializers.CredentialSerializer
{
    public class XmlCredentialRecordDeserializer : IDeserializer<string, IEnumerable<ICredentialRecord>>
    {
        public string SchemaVersion { get; } = "1.0";

        public IEnumerable<ICredentialRecord> Deserialize(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return Array.Empty<ICredentialRecord>();
            
            XDocument xdoc = XDocument.Parse(xml);
            XElement rootElement = xdoc.Root;

            // If schema version doesn't match or is missing, return empty
            // This handles uninitialized or invalid credential files gracefully
            if (!IsValidSchemaVersion(rootElement))
                return Array.Empty<ICredentialRecord>();

            IEnumerable<CredentialRecord> credentials = from element in xdoc.Descendants("Credential")
                                  select new CredentialRecord(Guid.Parse(element.Attribute("Id")?.Value ??
                                                                         Guid.NewGuid().ToString()))
                                  {
                                      Title = element.Attribute("Title")?.Value ?? "",
                                      Username = element.Attribute("Username")?.Value ?? "",
                                      Password = element.Attribute("Password")?.Value.ConvertToSecureString(),
                                      Domain = element.Attribute("Domain")?.Value ?? ""
                                  };
            return credentials.ToArray();
        }

        private bool IsValidSchemaVersion(XElement rootElement)
        {
            string docSchemaVersion = rootElement?.Attribute("SchemaVersion")?.Value;
            return docSchemaVersion == SchemaVersion;
        }

        private void ValidateSchemaVersion(XElement rootElement)
        {
            if (!IsValidSchemaVersion(rootElement))
            {
                string docSchemaVersion = rootElement?.Attribute("SchemaVersion")?.Value;
                throw new Exception($"The schema version of this document is not supported by this class. Document Version: {docSchemaVersion} Supported Version: {SchemaVersion}");
            }
        }
    }
}