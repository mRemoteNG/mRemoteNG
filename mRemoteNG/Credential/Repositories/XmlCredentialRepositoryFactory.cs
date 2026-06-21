using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Xml.Linq;
using mRemoteNG.Config;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;

namespace mRemoteNG.Credential.Repositories
{
    [SupportedOSPlatform("windows")]
    public class XmlCredentialRepositoryFactory
    {
        private readonly ISecureSerializer<IEnumerable<ICredentialRecord>, string> _serializer;
        private readonly ISecureDeserializer<string, IEnumerable<ICredentialRecord>> _deserializer;

        public XmlCredentialRepositoryFactory(ISecureSerializer<IEnumerable<ICredentialRecord>, string> serializer,
                                              ISecureDeserializer<string, IEnumerable<ICredentialRecord>> deserializer)
        {
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));
            if (deserializer == null)
                throw new ArgumentNullException(nameof(deserializer));

            _serializer = serializer;
            _deserializer = deserializer;
        }

        public ICredentialRepository Build(ICredentialRepositoryConfig config)
        {
            return BuildXmlRepo(config);
        }

        public ICredentialRepository Build(XElement repositoryXElement)
        {
            string? stringId = repositoryXElement.Attribute("Id")?.Value;
            Guid.TryParse(stringId, out Guid id);
            if (id.Equals(Guid.Empty)) id = Guid.NewGuid();
            CredentialRepositoryConfig config = new(id)
            {
                TypeName = repositoryXElement.Attribute("TypeName")?.Value ?? string.Empty,
                Title = repositoryXElement.Attribute("Title")?.Value ?? string.Empty,
                Source = repositoryXElement.Attribute("Source")?.Value ?? string.Empty
            };
            return BuildXmlRepo(config);
        }

        private ICredentialRepository BuildXmlRepo(ICredentialRepositoryConfig config)
        {
            FileDataProvider dataProvider = new(config.Source);
            CredentialRecordSaver saver = new(dataProvider, _serializer);
            CredentialRecordLoader loader = new(dataProvider, _deserializer);
            return new XmlCredentialRepository(config, saver, loader);
        }
    }
}