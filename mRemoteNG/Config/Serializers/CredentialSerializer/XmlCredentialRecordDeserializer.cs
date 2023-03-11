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
            if (string.IsNullOrEmpty(xml)) return new ICredentialRecord[0];
            var xdoc = XDocument.Parse(xml);
            var rootElement = xdoc.Root;
            ValidateSchemaVersion(rootElement);

            var credentials = from element in xdoc.Descendants("Credential")
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

        private void ValidateSchemaVersion(XElement rootElement)
        {
            var docSchemaVersion = rootElement?.Attribute("SchemaVersion")?.Value;
            if (docSchemaVersion != SchemaVersion)
                throw new Exception($"The schema version of this document is not supported by this class. Document Version: {docSchemaVersion} Supported Version: {SchemaVersion}");
        }
    }
}