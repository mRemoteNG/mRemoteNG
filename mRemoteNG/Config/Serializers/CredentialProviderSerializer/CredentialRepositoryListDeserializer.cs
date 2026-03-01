using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Xml.Linq;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;

namespace mRemoteNG.Config.Serializers.CredentialProviderSerializer
{
    [SupportedOSPlatform("windows")]
    public class CredentialRepositoryListDeserializer
    {
        private readonly ISecureSerializer<IEnumerable<ICredentialRecord>, string> _serializer;
        private readonly ISecureDeserializer<string, IEnumerable<ICredentialRecord>> _deserializer;

        public CredentialRepositoryListDeserializer(
            ISecureSerializer<IEnumerable<ICredentialRecord>, string> serializer,
            ISecureDeserializer<string, IEnumerable<ICredentialRecord>> deserializer)
        {
            ArgumentNullException.ThrowIfNull(serializer);
            ArgumentNullException.ThrowIfNull(deserializer);
            _serializer = serializer;
            _deserializer = deserializer;
        }

        public IEnumerable<ICredentialRepository> Deserialize(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return Array.Empty<ICredentialRepository>();
            XDocument xdoc = XDocument.Parse(xml);
            IEnumerable<XElement> repoEntries = xdoc.Descendants("CredentialRepository");
            XmlCredentialRepositoryFactory xmlRepoFactory = new(_serializer, _deserializer);
            return repoEntries.Select(xmlRepoFactory.Build);
        }
    }
}