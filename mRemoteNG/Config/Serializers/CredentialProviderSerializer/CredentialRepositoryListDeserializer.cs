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
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));
            if (deserializer == null)
                throw new ArgumentNullException(nameof(deserializer));

            _serializer = serializer;
            _deserializer = deserializer;
        }

        public IEnumerable<ICredentialRepository> Deserialize(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return new ICredentialRepository[0];
            var xdoc = XDocument.Parse(xml);
            var repoEntries = xdoc.Descendants("CredentialRepository");
            var xmlRepoFactory = new XmlCredentialRepositoryFactory(_serializer, _deserializer);
            return repoEntries.Select(xmlRepoFactory.Build);
        }
    }
}