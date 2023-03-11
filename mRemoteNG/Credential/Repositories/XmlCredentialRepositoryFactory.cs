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
            var stringId = repositoryXElement.Attribute("Id")?.Value;
            Guid id;
            Guid.TryParse(stringId, out id);
            if (id.Equals(Guid.Empty)) id = Guid.NewGuid();
            var config = new CredentialRepositoryConfig(id)
            {
                TypeName = repositoryXElement.Attribute("TypeName")?.Value,
                Title = repositoryXElement.Attribute("Title")?.Value,
                Source = repositoryXElement.Attribute("Source")?.Value
            };
            return BuildXmlRepo(config);
        }

        private ICredentialRepository BuildXmlRepo(ICredentialRepositoryConfig config)
        {
            var dataProvider = new FileDataProvider(config.Source);
            var saver = new CredentialRecordSaver(dataProvider, _serializer);
            var loader = new CredentialRecordLoader(dataProvider, _deserializer);
            return new XmlCredentialRepository(config, saver, loader);
        }
    }
}