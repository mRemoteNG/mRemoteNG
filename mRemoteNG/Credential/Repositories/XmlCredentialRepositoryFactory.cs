using System.Collections.Generic;
using mRemoteNG.Config;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Tools;

namespace mRemoteNG.Credential.Repositories
{
    public class XmlCredentialRepositoryFactory : ICredentialRepositoryFactory
    {
        private readonly ISecureSerializer<IEnumerable<ICredentialRecord>, string> _serializer;
        private readonly ISecureDeserializer<string, IEnumerable<ICredentialRecord>> _deserializer;

        public XmlCredentialRepositoryFactory(
            ISecureSerializer<IEnumerable<ICredentialRecord>, string> serializer, 
            ISecureDeserializer<string, IEnumerable<ICredentialRecord>> deserializer)
        {
            _serializer = serializer.ThrowIfNull(nameof(serializer));
            _deserializer = deserializer.ThrowIfNull(nameof(deserializer));
        }

        public string SupportsConfigType { get; } = "Xml";

        /// <summary>
        /// Creates a new <see cref="XmlCredentialRepository"/> instance for
        /// the given <see cref="ICredentialRepositoryConfig"/>.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="isLoaded">
        /// Does this instance represent a repository that is already loaded?
        /// </param>
        public ICredentialRepository Build(ICredentialRepositoryConfig config, bool isLoaded = false)
        {
            var dataProvider = new FileDataProvider(config.Source);
            var saver = new CredentialRecordSaver(dataProvider, _serializer);
            var loader = new CredentialRecordLoader(dataProvider, _deserializer);

            return new XmlCredentialRepository(config, saver, loader, isLoaded);
        }
    }
}